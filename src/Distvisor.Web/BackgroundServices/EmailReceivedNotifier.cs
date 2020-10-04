using System;
using System.Threading;

namespace Distvisor.Web.BackgroundServices
{
    public interface IEmailReceivedNotifier
    {
        void Notify();
        bool Wait(TimeSpan timeout, CancellationToken cancellationToken);
    }

    public class EmailReceivedNotifier : IEmailReceivedNotifier
    {
        private readonly ManualResetEventSlim _isNotified = new ManualResetEventSlim(false);

        public void Notify()
        {
            _isNotified.Set();
        }

        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var isNotified = _isNotified.Wait(timeout, cancellationToken);
            _isNotified.Reset();

            return isNotified;
        }
    }
}
