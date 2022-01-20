using System;

namespace Distvisor.App.Core.Events
{
    public interface IEventEntityBuilder
	{
		EventEntity ToEventEntity(IEvent @event, Type aggregateRootType);
		IEvent FromEventEntity(EventEntity eventEntity);
	}
}
