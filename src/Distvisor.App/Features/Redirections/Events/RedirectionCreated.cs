using Distvisor.App.Core.Events;
using Distvisor.App.Features.Common.Interfaces;
using Distvisor.App.Features.Redirections.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Redirections.Events
{
    public class RedirectionCreated : RedirectionBaseEvent
    {
        public RedirectionCreated(string name, Uri url)
            : base(name, url) { }
    }

    public class RedirectionCreatedHandler : IEventHandler<RedirectionCreated>
    {
        private readonly IAppDbContext _appDbContext;

        public RedirectionCreatedHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(RedirectionCreated @event, CancellationToken cancellationToken)
        {
            _appDbContext.Redirections.Add(new RedirectionEntity
            {
                Id = @event.AggregateId,
                Name = @event.Name,
                Url = @event.Url.ToString(),
            });
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
