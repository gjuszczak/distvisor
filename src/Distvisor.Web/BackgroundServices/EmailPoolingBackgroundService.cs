using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.BackgroundServices
{
    public class EmailPoolingBackgroundService : BackgroundService
    {
        private readonly Timer _timer;
        private readonly IEmailReceivedNotifier _emailReceivedNotifier;
        private readonly IEmailReceivingService _emailReceivingService;
        private readonly IEventStore _eventStore;

        public EmailPoolingBackgroundService(
            IEmailReceivedNotifier emailReceivedNotifier, 
            IEmailReceivingService emailReceivingService,
            IEventStore eventStore)
        {
            _emailReceivedNotifier = emailReceivedNotifier;
            _emailReceivingService = emailReceivingService;
            _eventStore = eventStore;

            async void TimerCallback(object _)
            {
                await _emailReceivedNotifier.NotifyAsync(new EmailReceivedNotification { Key = "timer" });
            }

            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _timer.Dispose());

            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach(var notification in _emailReceivedNotifier.ConsumeAsync(stoppingToken))
                {
                    await PoolEmailAsync(notification);
                }
            }
        }

        private async Task PoolEmailAsync(EmailReceivedNotification notification)
        {
            await foreach (var receivedEmail in _emailReceivingService.PoolStoredEmailsAsync())
            {
                await _eventStore.Publish(new EmailReceivedEvent
                {
                    StorageKey = receivedEmail.StorageKey,
                    Timestamp = receivedEmail.Timestamp,
                    BodyMime = receivedEmail.BodyMime
                });
            }
        }
    }
}
