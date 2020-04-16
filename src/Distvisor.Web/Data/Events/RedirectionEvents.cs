using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads;

namespace Distvisor.Web.Data.Events
{
    public class SetRedirectionEvent
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class RemoveRedirectionEvent
    {
        public string Name { get; set; }
    }

    public class RedirectionEventHandler : IEventHandler<SetRedirectionEvent>, IEventHandler<RemoveRedirectionEvent>
    {
        private readonly ReadStore _context;

        public RedirectionEventHandler(ReadStore context)
        {
            _context = context;
        }

        public void Handle(SetRedirectionEvent payload)
        {
            var entity = _context.Redirections.FindOne(x => x.Name == payload.Name) ?? new RedirectionEntity();
            entity.Name = payload.Name;
            entity.Url = payload.Url;
            _context.Redirections.Upsert(entity);
        }

        public void Handle(RemoveRedirectionEvent payload)
        {
            _context.Redirections.DeleteMany(x => x.Name == payload.Name);
        }
    }
}
