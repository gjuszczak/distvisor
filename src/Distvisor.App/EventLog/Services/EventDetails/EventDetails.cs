namespace Distvisor.App.EventLog.Services.EventDetails
{
    public class EventDetails
    {
        public string EventTypeDisplayName { get; set; }
        public string AggregateTypeDisplayName { get; set; }
        public object MaskedPayload { get; set; }
    }
}
