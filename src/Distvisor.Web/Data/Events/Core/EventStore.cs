using Distvisor.Web.Data.Events.Entities;
using Distvisor.Web.Data.Reads.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events.Core
{
    public interface IEventStore
    {
        Task Publish<T>(T payload);
        Task ReplayEvents();
    }

    public class EventStore : IEventStore
    {
        private readonly EventStoreContext _db;
        private readonly IReadStoreTransactionProvider _rsTransactionProvider;
        private readonly IEventHandlerResolver _handler;

        public EventStore(EventStoreContext db, IReadStoreTransactionProvider rsTransactionProvider, IEventHandlerResolver handler)
        {
            _db = db;
            _rsTransactionProvider = rsTransactionProvider;
            _handler = handler;
        }

        public async Task Publish<T>(T payload)
        {
            var type = typeof(T);
            var entity = new EventEntity(payload, type);
            _db.Events.Add(entity);
            try
            {
                using var rsTransaction = await _rsTransactionProvider.BeginTransactionAsync();
                await _handler.GetHandler<T>().Handle(payload);
                await _db.SaveChangesAsync();
                await rsTransaction.CommitAsync();
            }
            catch
            {
                entity.Success = false;
                await _db.SaveChangesAsync();
                throw;
            }
        }

        public async Task ReplayEvents()
        {
            var collection = _db.Events.OrderBy(ev => ev.PublishDateUtc);
            foreach (var e in collection)
            {
                var payload = e.ToPayload();
                await _handler.GetHandler(payload.GetType()).Handle(payload);
                await _db.SaveChangesAsync();
            }
        }
    }
}
