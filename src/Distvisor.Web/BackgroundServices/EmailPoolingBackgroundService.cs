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

        public EmailPoolingBackgroundService(IEmailReceivedNotifier emailReceivedNotifier)
        {
            _emailReceivedNotifier = emailReceivedNotifier;

            async void TimerCallback(object _)
            {
                await _emailReceivedNotifier.NotifyAsync(new EmailReceivedNotification { Key = "timer" });
            }

            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
            await Task.CompletedTask;
        }
    }
}
