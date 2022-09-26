using Distvisor.App.EventLog.Services.PayloadMasking;
using Distvisor.App.HomeBox.Aggregates;

namespace Distvisor.Infrastructure.Services.EventLog
{
    public class GatewaySessionMaskingService : PayloadMaskingService<GatewaySession>
    {
        protected override GatewaySession Mask(GatewaySession aggregate)
        {
            return aggregate; // todo: use JsonObject (.NET6+)
        }
    }
}
