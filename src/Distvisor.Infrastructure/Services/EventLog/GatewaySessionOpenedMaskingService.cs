using Distvisor.App.EventLog.Services.PayloadMasking;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.ValueObjects;

namespace Distvisor.Infrastructure.Services.EventLog
{
    public class GatewaySessionOpenedMaskingService : PayloadMaskingService<GatewaySessionOpened>
    {
        protected override GatewaySessionOpened Mask(GatewaySessionOpened @event)
        {
            var maskedToken = new GatewayToken(MaskString, MaskString, @event.Token.GeneratedAt);
            return new GatewaySessionOpened(@event.Username, maskedToken);
        }
    }
}
