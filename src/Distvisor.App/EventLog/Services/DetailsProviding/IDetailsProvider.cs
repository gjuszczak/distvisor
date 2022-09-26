namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public interface IDetailsProvider<TValue, TDetails>
    {
        TDetails GetDetails(TValue value);
    }
}