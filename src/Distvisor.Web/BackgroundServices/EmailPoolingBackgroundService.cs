using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.BackgroundServices
{
    public class EmailPoolingBackgroundService : BackgroundService
    {
        private readonly Timer _timer;
        private readonly IEmailReceivedNotifier _emailReceivedNotifier;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public EmailPoolingBackgroundService(
            IEmailReceivedNotifier emailReceivedNotifier, 
            IServiceProvider serviceProvider,
            ILogger<EmailPoolingBackgroundService> logger)
        {
            _emailReceivedNotifier = emailReceivedNotifier;
            _serviceProvider = serviceProvider;
            _logger = logger;

            async void TimerCallback(object _)
            {
                await _emailReceivedNotifier.NotifyAsync(new EmailReceivedNotification { Key = "timer" });
            }

            _timer = new Timer(TimerCallback, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _timer.Dispose());

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var notification in _emailReceivedNotifier.ConsumeAsync(stoppingToken))
                    {
                        await PoolEmailAsync(notification);
                    }
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, "Unhandled exception while handling email received notification");
                }
            }
        }

        private async Task PoolEmailAsync(EmailReceivedNotification notification)
        {
            using var scope = _serviceProvider.CreateScope();
            var emailReceivingService = scope.ServiceProvider.GetService<IEmailReceivingService>();
            var eventStore = scope.ServiceProvider.GetService<IEventStore>();
            await foreach (var receivedEmail in emailReceivingService.PoolStoredEmailsAsync())
            {
                await eventStore.Publish(new EmailReceivedEvent
                {
                    StorageKey = receivedEmail.StorageKey,
                    Timestamp = receivedEmail.Timestamp,
                    BodyMime = receivedEmail.BodyMime
                });
            }
        }
    }
}
