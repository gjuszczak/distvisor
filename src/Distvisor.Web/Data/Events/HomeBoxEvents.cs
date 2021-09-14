using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events
{
    public class HomeBoxDeviceUpdatedEvent : HomeBoxDevice
    {
    }

    public class HomeBoxTriggerUpsertedEvent : HomeBoxTrigger
    {
        public HomeBoxTriggerSource[] Sources { get; set; }
        public HomeBoxTriggerTarget[] Targets { get; set; }
        public HomeBoxTriggerAction[] Actions { get; set; }
    }

    public class HomeBoxTriggerDeletedEvent
    {
        public Guid Id { get; set; }
    }

    public class HomeBoxTriggerToggledEvent
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
    }

    public class HomeBoxEventHandlers :
       IEventHandler<HomeBoxDeviceUpdatedEvent>,
       IEventHandler<HomeBoxTriggerUpsertedEvent>,
       IEventHandler<HomeBoxTriggerDeletedEvent>,
       IEventHandler<HomeBoxTriggerToggledEvent>
    {
        private readonly ReadStoreContext _context;
        public HomeBoxEventHandlers(ReadStoreContext context)
        {
            _context = context;
        }

        public async Task Handle(HomeBoxDeviceUpdatedEvent payload)
        {
            var entity = await _context.HomeboxDevices.FirstOrDefaultAsync(d => d.Id == payload.Id);
            if (entity == null)
            {
                entity = PropMapper<HomeBoxDeviceUpdatedEvent, HomeBoxDeviceEntity>.From(payload);
                _context.HomeboxDevices.Add(entity);
            }
            else
            {
                PropMapper<HomeBoxDeviceUpdatedEvent, HomeBoxDeviceEntity>.CopyTo(payload, entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Handle(HomeBoxTriggerUpsertedEvent payload)
        {
            var entity = PropMapper<HomeBoxTriggerUpsertedEvent, HomeBoxTriggerEntity>.From(payload);
            entity.Sources = payload.Sources.Select(p => PropMapper<HomeBoxTriggerSource, HomeBoxTriggerSourceEntity>.From(p)).ToList();
            entity.Targets = payload.Targets.Select(t => PropMapper<HomeBoxTriggerTarget, HomeBoxTriggerTargetEntity>.From(t)).ToList();
            entity.Actions = payload.Actions.Select(a => PropMapper<HomeBoxTriggerAction, HomeBoxTriggerActionEntity>.From(a)).ToList();

            var storedEntity = await _context.HomeboxTriggers.FindAsync(payload.Id);
            if (storedEntity != null)
            {
                _context.HomeboxTriggers.Remove(storedEntity);
                await _context.SaveChangesAsync();
            }

            _context.HomeboxTriggers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Handle(HomeBoxTriggerDeletedEvent payload)
        {
            var entity = PropMapper<HomeBoxTriggerDeletedEvent, HomeBoxTriggerEntity>.From(payload);

            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task Handle(HomeBoxTriggerToggledEvent payload)
        {
            var storedEntity = await _context.HomeboxTriggers.FindAsync(payload.Id);
            if(storedEntity == null)
            {
                throw new Exception($"Unable to toggle trigger {payload.Id}. Entity not found.");
            }

            storedEntity.Enabled = payload.Enabled;
            await _context.SaveChangesAsync();
        }
    }
}
