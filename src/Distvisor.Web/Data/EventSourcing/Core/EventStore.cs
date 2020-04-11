using LiteDB;
using Microsoft.Extensions.Configuration;

namespace Distvisor.Web.Data.EventSourcing.Core
{
    public interface IEventStore
    {
        void Publish(object payload);
        void ReplayEvents();
    }

    public class EventStore : IEventStore
    {
        private readonly string _dbPath;
        private readonly string _collectionName;
        private readonly IEventHandler<object> _handler;

        public EventStore(IConfiguration configuration, IEventHandler<object> handler)
        {
            _dbPath = configuration.GetConnectionString("EventStore");
            _collectionName = "events";
            _handler = handler;
        }

        public void Publish(object payload)
        {
            var entity = new EventEntity(payload);
            using var db = new LiteDatabase(_dbPath);
            var collection = db.GetCollection<EventEntity>(_collectionName);
            collection.Insert(entity);
        }

        public void ReplayEvents()
        {
            using var db = new LiteDatabase(_dbPath);
            var collection = db.GetCollection<EventEntity>(_collectionName);
            var entities = collection.FindAll();
            foreach (var e in entities)
            {
                var payload = e.ToPayload();
                _handler.Handle(payload);
            }
        }
    }
}
