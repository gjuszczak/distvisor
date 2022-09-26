using Distvisor.App.Core.Serialization;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.PayloadMasking
{
    public abstract class PayloadMaskingService<TPayload> : IPayloadMaskingService<TPayload>
    {
        public static readonly string MaskString = "*****";

        public virtual JsonDocument Mask(JsonDocument payload)
        {
            var deserializedPayload = payload.Deserialize<TPayload>(JsonDefaults.SerializerOptions);
            var maskedObject = Mask(deserializedPayload);
            var maskedPayload = maskedObject.SerializeToDocument(JsonDefaults.SerializerOptions);
            return maskedPayload;
        }

        protected abstract TPayload Mask(TPayload payload);
    }
}
