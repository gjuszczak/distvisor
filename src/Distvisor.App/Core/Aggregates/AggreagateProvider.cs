using System;

namespace Distvisor.App.Core.Aggregates
{
	public class AggreagateProvider : IAggregateProvider
	{
		private readonly IServiceProvider _serviceProvider;

		public AggreagateProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public TAggregate Create<TAggregate>() where TAggregate : IAggregateRoot
		{
			return (TAggregate)_serviceProvider.GetService(typeof(TAggregate));
		}
	}
}
