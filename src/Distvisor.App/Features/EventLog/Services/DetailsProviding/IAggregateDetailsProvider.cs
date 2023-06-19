using Distvisor.App.Core.Aggregates;

namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public interface IAggregateDetailsProvider
    {
        AggregateDetails GetDetails(IAggregateRoot value);
    }
}
