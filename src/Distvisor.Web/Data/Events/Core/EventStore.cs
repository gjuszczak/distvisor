namespace Distvisor.Web.Data.Events.Core
{
    public interface IEventStore
    {
        void Publish(object payload);
        void ReplayEvents();
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

        public void Publish(object payload)
        {
            var entity = new EventEntity(payload);
            _db.Events.Add(entity);
            _db.SaveChanges();
            _handler.Handle(payload);
        }

        public void ReplayEvents()
        {
            var collection = _db.Events;
            foreach (var e in collection)
            {
                var payload = e.ToPayload();
                _handler.Handle(payload);
            }
        }
    }
}
