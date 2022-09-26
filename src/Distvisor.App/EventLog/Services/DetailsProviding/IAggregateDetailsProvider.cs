using Distvisor.App.Core.Aggregates;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public interface IAggregateDetailsProvider
    {
        AggregateDetails GetDetails(IAggregateRoot value);
    }
}
