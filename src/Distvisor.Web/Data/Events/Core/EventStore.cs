using Distvisor.Web.Data.Events.Entities;
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
        private readonly IEventHandler<object> _handler;

        public EventStore(EventStoreContext db, IEventHandler<object> handler)
        {
            _db = db;
            _handler = handler;
        }

        public async Task Publish(object payload)
        {
            var entity = new EventEntity(payload);
            _db.Events.Add(entity);
            await _db.SaveChangesAsync();
            await _handler.Handle(payload);
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
