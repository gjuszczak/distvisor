using Distvisor.Web.Data.Events.Entities;
using Distvisor.Web.Data.Reads.Core;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events.Core
{
    public interface IEventStore
    {
        Task Publish(object payload);
        Task ReplayEvents();
    }

    public class EventStore : IEventStore
    {
        private readonly EventStoreContext _db;
        private readonly IReadStoreTransactionProvider _rsTransactionProvider;
        private readonly IEventHandler<object> _handler;

        public EventStore(EventStoreContext db, IReadStoreTransactionProvider rsTransactionProvider, IEventHandler<object> handler)
        {
            _db = db;
            _rsTransactionProvider = rsTransactionProvider;
            _handler = handler;
        }

        public async Task Publish(object payload)
        {
            var entity = new EventEntity(payload);
            _db.Events.Add(entity);
            try
            {
                using var rsTransaction = await _rsTransactionProvider.BeginTransactionAsync();
                await _handler.Handle(payload);
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
            var collection = _db.Events;
            foreach (var e in collection)
            {
                var payload = e.ToPayload();
                await _handler.Handle(payload);
            }
        }
    }
}
