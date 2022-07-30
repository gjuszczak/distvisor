using Distvisor.App.EventLog.Services.EventDetails;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.ValueObjects;

namespace Distvisor.Infrastructure.Services.EventLog
{
    public class GatewaySessionOpenedMaskingService : EventMaskingService<GatewaySessionOpened>
    {
        protected override object Mask(GatewaySessionOpened @event)
        {
            var maskedToken = new GatewayToken(MaskString, MaskString, @event.Token.GeneratedAt);
            return new GatewaySessionOpened(@event.Username, maskedToken);
        }
    }
}
