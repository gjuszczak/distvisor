namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public interface IDetailsProvider<TValue, TDetails>
    {
        TDetails GetDetails(TValue value);
    }
}