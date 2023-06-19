using Distvisor.App.Core.Events;
using System;

namespace Distvisor.App.Features.Redirections.Events
{
    public abstract class RedirectionBaseEvent : Event
    {
        public string Name { get; init; }
        public Uri Url { get; init; }

        protected RedirectionBaseEvent(string name, Uri url)
        {
            Name = name;
            Url = url;
        }
    }
}
