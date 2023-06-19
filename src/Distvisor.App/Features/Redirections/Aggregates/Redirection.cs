using Distvisor.App.Core.Aggregates;
using Distvisor.App.Features.Redirections.Events;
using Distvisor.App.Features.Redirections.Exceptions;
using System;

namespace Distvisor.App.Features.Redirections.Aggregates
{
    public class Redirection : AggregateRoot
    {
        public string Name { get; private set; }
        public Uri Url { get; private set; }
        public bool IsDeleted { get; private set; }

        public Redirection()
        {
            RegisterEventHandler<RedirectionCreated>(Apply);
            RegisterEventHandler<RedirectionEdited>(Apply);
            RegisterEventHandler<RedirectionDeleted>(Apply);
        }

        public Redirection(Guid aggregateId, string name, Uri url) : this()
        {
            AggregateId = aggregateId;
            ApplyEvent(new RedirectionCreated(name, url));
        }

        public void Edit(string name, Uri url)
        {
            if (IsDeleted)
            {
                throw new RedirectionDeletedException();
            }
            ApplyEvent(new RedirectionEdited(name, url));
        }

        public void Delete()
        {
            if (!IsDeleted)
            {
                ApplyEvent(new RedirectionDeleted());
            }
        }

        private void Apply(RedirectionBaseEvent @event)
        {
            Name = @event.Name;
            Url = @event.Url;
            IsDeleted = false;
        }

        private void Apply(RedirectionDeleted obj)
        {
            IsDeleted = true;
        }
    }
}
