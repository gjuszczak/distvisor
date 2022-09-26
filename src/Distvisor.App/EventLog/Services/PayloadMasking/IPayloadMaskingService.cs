using System.Text.Json;

namespace Distvisor.App.EventLog.Services.PayloadMasking
{
    public interface IPayloadMaskingService
    {
        JsonDocument Mask(JsonDocument eventPayload);
    }

    public interface IPayloadMaskingService<TPayload> : IPayloadMaskingService 
    {
    }
}
