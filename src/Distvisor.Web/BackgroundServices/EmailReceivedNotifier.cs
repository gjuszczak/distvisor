using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Distvisor.Web.BackgroundServices
{
    public interface IEmailReceivedNotifier
    {
        Task NotifyAsync(EmailReceivedNotification notification);
        IAsyncEnumerable<EmailReceivedNotification> ConsumeAsync(CancellationToken cancellationToken);
    }

    public class EmailReceivedNotifier : IEmailReceivedNotifier
    {
        private readonly Channel<EmailReceivedNotification> _channel;

        public EmailReceivedNotifier()
        {
            _channel = Channel.CreateUnbounded<EmailReceivedNotification>(new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = false,
            });
        }

        public async Task NotifyAsync(EmailReceivedNotification notification)
        {
            await _channel.Writer.WriteAsync(notification);
        }

        public IAsyncEnumerable<EmailReceivedNotification> ConsumeAsync(CancellationToken cancellationToken)
        {
            // warning: must be consumed by single reader only !
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
    }

    public class EmailReceivedNotification
    {
        public string Key { get; set; }
    }
}
