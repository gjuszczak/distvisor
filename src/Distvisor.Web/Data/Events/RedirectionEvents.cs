using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task Handle(SetRedirectionEvent payload)
        {
            var entity = _context.Redirections.FirstOrDefault(x => x.Name == payload.Name);
            if (entity == null)
            {
                entity = new RedirectionEntity();
                _context.Redirections.Add(entity);
            }
            entity.Name = payload.Name;
            entity.Url = payload.Url;
            await _context.SaveChangesAsync();
        }

        public async Task Handle(RemoveRedirectionEvent payload)
        {
            var toRemove = _context.Redirections.Where(x => x.Name == payload.Name);
            _context.Redirections.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}
