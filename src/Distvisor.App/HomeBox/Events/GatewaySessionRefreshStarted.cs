using Distvisor.App.Core.Events;
using System;

namespace Distvisor.App.HomeBox.Events
{
    public class GatewaySessionRefreshStarted : Event
    {
        public DateTimeOffset Timeout { get; init; }

        public GatewaySessionRefreshStarted(DateTimeOffset timeout)
        {
            Timeout = timeout;
        }
    }
}
