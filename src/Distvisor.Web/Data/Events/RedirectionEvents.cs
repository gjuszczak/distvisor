using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using System.Linq;

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
        private readonly ReadStoreContext _context;

        public RedirectionEventHandler(ReadStoreContext context)
        {
            _context = context;
        }

        public void Handle(SetRedirectionEvent payload)
        {
            var entity = _context.Redirections.FirstOrDefault(x => x.Name == payload.Name) ?? new RedirectionEntity();
            entity.Name = payload.Name;
            entity.Url = payload.Url;
            _context.Redirections.Add(entity);
            _context.SaveChanges();
        }

        public void Handle(RemoveRedirectionEvent payload)
        {
            var toRemove = _context.Redirections.Where(x => x.Name == payload.Name);
            _context.Redirections.RemoveRange(toRemove);
            _context.SaveChanges();
        }
    }
}
