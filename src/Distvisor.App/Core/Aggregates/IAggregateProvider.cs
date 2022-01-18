namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateProvider
	{
		TAggregate Create<TAggregate>() where TAggregate : IAggregateRoot;
	}
}
