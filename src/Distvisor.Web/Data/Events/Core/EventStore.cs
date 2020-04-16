using LiteDB;

namespace Distvisor.Web.Data.Events.Core
{
    public interface IEventStore
    {
        void Publish(object payload);
        void ReplayEvents();
    }

    public class EventStore : IEventStore
    {
        private readonly string _collectionName;
        private readonly ILiteDatabase _db;
        private readonly IEventHandler<object> _handler;

        public EventStore(IDbProvider dbProvider, IEventHandler<object> handler)
        {
            _collectionName = "events";
            _db = dbProvider.EventStoreDatabase;
            _handler = handler;
        }

        public void Publish(object payload)
        {
            var entity = new EventEntity(payload);
            var collection = _db.GetCollection<EventEntity>(_collectionName);
            collection.Insert(entity);
            _handler.Handle(payload);
        }

        public void ReplayEvents()
        {
            var collection = _db.GetCollection<EventEntity>(_collectionName);
            var entities = collection.FindAll();
            foreach (var e in entities)
            {
                var payload = e.ToPayload();
                _handler.Handle(payload);
            }
        }
    }
}
