using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.ValueObjects;

namespace Distvisor.App.HomeBox.Events
{
    public class GatewaySessionRefreshSucceeded : Event
    {
        public GatewayToken Token { get; init; }

        public GatewaySessionRefreshSucceeded(GatewayToken token)
        {
            Token = token;
        }
    }
}
