namespace Distvisor.App.Core.Events
{
    public interface IEventEntityBuilder
	{
		EventEntity ToEventEntity(IEvent @event);
		IEvent FromEventEntity(EventEntity eventEntity);
	}
}
