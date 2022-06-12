using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Events
{
    public class GatewaySessionDeleted : Event
    {
    }

    public class GatewaySessionDeletedHandler : IEventHandler<GatewaySessionDeleted>
    {
        private readonly IAppDbContext _appDbContext;
        public GatewaySessionDeletedHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(GatewaySessionDeleted @event, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.HomeboxGatewaySessions.FindAsync(new object[] { @event.AggregateId }, cancellationToken);
            _appDbContext.HomeboxGatewaySessions.Remove(entity);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
