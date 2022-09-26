using Distvisor.App.EventLog.Services.PayloadMasking;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.ValueObjects;

namespace Distvisor.Infrastructure.Services.EventLog
{
    public class GatewaySessionRefreshSucceededMaskingService : PayloadMaskingService<GatewaySessionRefreshSucceeded>
    {
        protected override GatewaySessionRefreshSucceeded Mask(GatewaySessionRefreshSucceeded @event)
        {
            var maskedToken = new GatewayToken(MaskString, MaskString, @event.Token.GeneratedAt);
            return new GatewaySessionRefreshSucceeded(maskedToken);
        }
    }
}
