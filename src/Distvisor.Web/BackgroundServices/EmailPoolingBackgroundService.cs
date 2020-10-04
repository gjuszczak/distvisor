using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.BackgroundServices
{
    public class EmailPoolingBackgroundService : BackgroundService
    {
        private readonly TimeSpan _poolTimeSpan = TimeSpan.FromSeconds(5);
        private readonly IEmailReceivedNotifier _emailReceivedNotifier;

        public EmailPoolingBackgroundService(IEmailReceivedNotifier emailReceivedNotifier)
        {
            _emailReceivedNotifier = emailReceivedNotifier;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var isNotified = _emailReceivedNotifier.Wait(_poolTimeSpan, stoppingToken);
                        PoolEmailsAsync().Wait();
                    }
                    catch { }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private async Task PoolEmailsAsync()
        {
            await Task.CompletedTask;
        }
    }
}
