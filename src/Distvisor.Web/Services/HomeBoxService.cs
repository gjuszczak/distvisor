using Distvisor.Web.Data;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IHomeBoxService
    {
        Task AddTriggerAsync(AddHomeBoxTriggerDto trigger);
        Task<DeviceDto[]> GetDevicesAsync();
        Task RfCodeReceivedAsync(string code);
        Task SetDeviceParamsAsync(string deviceId, object deviceParams);
        Task DeleteTriggerAsync(Guid id);
        Task<HomeBoxTriggerDto[]> ListTriggersAsync();
    }

    public class HomeBoxService : IHomeBoxService
    {
        private readonly IEwelinkClient _ewelinkClient;
        private readonly INotificationService _notifications;
        private readonly IEventStore _eventStore;
        private readonly ReadStoreContext _readStore;
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<int, DeviceTypeDto> _deviceTypes;

        public HomeBoxService(
            IEwelinkClient ewelinkClient,
            INotificationService notifications,
            IEventStore eventStore,
            ReadStoreContext readStore,
            IMemoryCache memoryCache)
        {
            _ewelinkClient = ewelinkClient;
            _notifications = notifications;
            _eventStore = eventStore;
            _readStore = readStore;
            _memoryCache = memoryCache;
            _deviceTypes = new Dictionary<int, DeviceTypeDto>
            {
                {1, DeviceTypeDto.Switch },
                {59, DeviceTypeDto.RgbLight },
                {104, DeviceTypeDto.RgbwLight }
            };
        }

        public async Task<DeviceDto[]> GetDevicesAsync()
        {
            var devices = await _ewelinkClient.GetDevices();

            var result = devices.devicelist.Select(d => new DeviceDto
            {
                Identifier = d.deviceid,
                Type = _deviceTypes.GetValueOrDefault(d.uiid, DeviceTypeDto.Unknown),
                Online = d.online,
                Params = d.@params
            }).ToArray();

            return result;
        }

        public async Task SetDeviceParamsAsync(string deviceId, object deviceParams)
        {
            await _ewelinkClient.SetDeviceParams(deviceId, deviceParams);
        }

        public async Task RfCodeReceivedAsync(string code)
        {
            var cacheKey = TriggerSourceCacheKey(HomeBoxTriggerSourceType.Rf433Receiver, code);
            if (!_memoryCache.TryGetValue(cacheKey, out Guid[] triggerIds))
            {
                triggerIds = await _readStore.HomeboxTriggerSources
                    .Where(x => x.Type == HomeBoxTriggerSourceType.Rf433Receiver && x.MatchParam == code)
                    .Select(x => x.TriggerId)
                    .Distinct()
                    .ToArrayAsync();

                _memoryCache.Set(cacheKey, triggerIds, DateTimeOffset.Now.AddDays(1));
            }

            foreach (var tid in triggerIds)
            {
                try
                {
                    await ExecuteTriggerAsync(tid);
                }
                catch (Exception exc)
                {
                    if (exc.Message.Contains($"Trigger not found"))
                    {
                        _memoryCache.Remove(cacheKey);
                    }
                }
            }
        }

        public async Task AddTriggerAsync(AddHomeBoxTriggerDto trigger)
        {
            trigger.Id = trigger.Id.GenerateIfEmpty();
            foreach (var source in trigger.Sources)
            {
                source.Id = source.Id.GenerateIfEmpty();
                source.TriggerId = trigger.Id;
            }
            foreach (var target in trigger.Targets)
            {
                target.Id = target.Id.GenerateIfEmpty();
                target.TriggerId = trigger.Id;
            }
            foreach (var action in trigger.Actions)
            {
                action.Id = action.Id.GenerateIfEmpty();
                action.TriggerId = trigger.Id;
            }

            await _eventStore.Publish<HomeBoxTriggerAddedEvent>(trigger);

            _memoryCache.Remove(TriggerListCacheKey());

            await _notifications.PushSuccessAsync("Trigger added successfully.");
        }

        public async Task DeleteTriggerAsync(Guid id)
        {
            await _eventStore.Publish(new HomeBoxTriggerDeletedEvent
            {
                Id = id,
            });
            _memoryCache.Remove(TriggerListCacheKey());
            _memoryCache.Remove(TriggerExecutionMemoryCacheKey(id));
            await _notifications.PushSuccessAsync("Trigger deleted successfully.");
        }

        public async Task<HomeBoxTriggerDto[]> ListTriggersAsync()
        {
            var cacheKey = TriggerListCacheKey();
            if (!_memoryCache.TryGetValue(cacheKey, out HomeBoxTriggerDto[] triggers))
            {
                var entities = await _readStore.HomeboxTriggers
                    .Include(x => x.Sources)
                    .Include(x => x.Targets)
                    .Include(x => x.Actions)
                    .ToListAsync();

                triggers = entities.Select(e =>
                {
                    var trigger = PropMapper<HomeBoxTriggerEntity, HomeBoxTriggerDto>.From(e);
                    trigger.Sources = e.Sources.Select(s => PropMapper<HomeBoxTriggerSourceEntity, HomeBoxTriggerSource>.From(s)).ToArray();
                    trigger.Targets = e.Targets.Select(t => PropMapper<HomeBoxTriggerTargetEntity, HomeBoxTriggerTarget>.From(t)).ToArray();
                    trigger.Actions = e.Actions.Select(a => PropMapper<HomeBoxTriggerActionEntity, HomeBoxTriggerAction>.From(a)).ToArray();
                    return trigger;
                }).ToArray();

                _memoryCache.Set(cacheKey, triggers, DateTimeOffset.Now.AddDays(1));
            }

            foreach (var t in triggers)
            {
                t.ExecutionMemory = LoadTriggerExecutionMemory(t.Id);
            }

            return triggers;
        }

        public async Task ExecuteTriggerAsync(Guid triggerId)
        {
            var triggerList = await ListTriggersAsync();
            var trigger = triggerList.FirstOrDefault(t => t.Id == triggerId);

            if (trigger == null)
            {
                throw new Exception($"Unable to execute trigger {triggerId}. Trigger not found.");
            }

            var msExecutionDelay = (int)(DateTime.Now - trigger.ExecutionMemory.LastExecutedActionDateTime).TotalMilliseconds;
            var devicesStatus = (await GetDevicesAsync())
                .Where(d => trigger.Targets.Any(t => t.DeviceIdentifier == d.Identifier))
                .Select<DeviceDto, (string deviceId, bool isOn, bool isOnline)>(d =>
                {
                    var isOn = d.Params.GetProperty("switch").GetString() == "on";
                    return (d.Identifier, isOn, d.Online);
                })
                .ToArray();

            if (devicesStatus.All(d => d.deviceId == null))
            {
                throw new Exception($"Unable to execute trigger {triggerId}. All target devices are not available.");
            }

            var selectedAction = trigger.Actions
                .Select((action, number) => (action, number))
                .Where(t => t.action.LastExecutedActionNumber == null || t.action.LastExecutedActionNumber == trigger.ExecutionMemory.LastExecutedActionNumber)
                .Where(t => t.action.LastExecutedActionMinDelayMs == null || t.action.LastExecutedActionMinDelayMs > msExecutionDelay)
                .Where(t => t.action.LastExecutedActionMaxDelayMs == null || t.action.LastExecutedActionMaxDelayMs <= msExecutionDelay)
                .Where(t => t.action.IsDeviceOn == null || devicesStatus.Any(tt => tt.isOn == t.action.IsDeviceOn))
                .Where(t => t.action.IsDeviceOnline == null || devicesStatus.Any(tt => tt.isOnline == t.action.IsDeviceOnline))
                .FirstOrDefault();

            if (selectedAction.action == null)
            {
                throw new Exception($"Unable to execute trigger {triggerId}. No matching action available.");
            }

            foreach (var t in trigger.Targets)
            {
                await SetDeviceParamsAsync(t.DeviceIdentifier, selectedAction.action.Payload);
            }

            trigger.ExecutionMemory.LastExecutedActionNumber = selectedAction.number;
            trigger.ExecutionMemory.LastExecutedActionDateTime = DateTime.Now;
            StoreTriggerExecutionMemory(triggerId, trigger.ExecutionMemory);
        }

        private HomeBoxTriggerExecutionMemory LoadTriggerExecutionMemory(Guid triggerId)
        {
            var cacheKey = TriggerExecutionMemoryCacheKey(triggerId);
            if (_memoryCache.TryGetValue(cacheKey, out HomeBoxTriggerExecutionMemory executionMemory))
            {
                return executionMemory;
            }
            return new HomeBoxTriggerExecutionMemory { LastExecutedActionNumber = -1, LastExecutedActionDateTime = DateTime.MinValue };
        }
        private void StoreTriggerExecutionMemory(Guid triggerId, HomeBoxTriggerExecutionMemory executionMemory)
        {
            var cacheKey = TriggerExecutionMemoryCacheKey(triggerId);
            _memoryCache.Set(cacheKey, executionMemory, DateTime.Now.AddDays(1));
        }
        private string TriggerListCacheKey() => $"{typeof(HomeBoxTriggerDto).FullName}.List";
        private string TriggerExecutionMemoryCacheKey(Guid triggerId) => $"{typeof(HomeBoxTriggerExecutionMemory)}.{triggerId}";
        private string TriggerSourceCacheKey(HomeBoxTriggerSourceType triggerSourceType, string matchParam) => $"{typeof(HomeBoxTriggerSourceType)}.{triggerSourceType}.{matchParam}";
    }

    public class DeviceDto
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public DeviceTypeDto Type { get; set; }
        public string Location { get; set; }
        public bool Online { get; set; }
        public JsonElement Params { get; set; }
    }

    public enum DeviceTypeDto
    {
        Unknown,
        RgbLight,
        RgbwLight,
        Switch,
    }

    public class AddHomeBoxTriggerDto : HomeBoxTriggerAddedEvent
    {
    }

    public class HomeBoxTriggerDto : HomeBoxTrigger
    {
        public HomeBoxTriggerSource[] Sources { get; set; }
        public HomeBoxTriggerTarget[] Targets { get; set; }
        public HomeBoxTriggerAction[] Actions { get; set; }
        public HomeBoxTriggerExecutionMemory ExecutionMemory { get; set; }
    }
}
