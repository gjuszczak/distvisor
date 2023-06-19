namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public class EventDetails
    {
        public string EventTypeDisplayName { get; set; }
        public string AggregateTypeDisplayName { get; set; }
        public object MaskedPayload { get; set; }
    }
}
