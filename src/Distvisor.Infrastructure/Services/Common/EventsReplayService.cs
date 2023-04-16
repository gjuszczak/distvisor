using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Events;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Common
{
    public class EventsReplayService : IEventsReplayService
    {
        private readonly IEventStore _eventStore;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAppDbContext _appDbContext;

        public EventsReplayService(IEventStore eventStore, IAppDbContext appDbContext, IEventPublisher eventPublisher)
        {
            _eventStore = eventStore;
            _appDbContext = appDbContext;
            _eventPublisher = eventPublisher;
        }

        public async Task ReplayAsync(CancellationToken cancellationToken)
        {
            var batchMaxSize = 1000;
            var batchCurrentSize = batchMaxSize;
            var offsetId = 0L;

            using var transaction = await _appDbContext.DeleteAllData(cancellationToken);
            while (batchMaxSize == batchCurrentSize)
            {
                var events = await _eventStore.GetAsync(offsetId, batchMaxSize, cancellationToken);
                batchCurrentSize = events.Count();
                if (batchCurrentSize > 0)
                {
                    await _eventPublisher.PublishAsync(events, cancellationToken);
                    offsetId = events.Last().EventId;
                }
            }
            await transaction.CommitAsync(cancellationToken);
        }
    }
}
