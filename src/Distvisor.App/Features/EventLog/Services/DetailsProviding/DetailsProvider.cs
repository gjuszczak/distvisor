using Distvisor.App.Core.Serialization;
using Distvisor.App.Features.EventLog.Services.DetailsProviding;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public abstract partial class DetailsProvider<TValue, TDetails>
    {
        private readonly ConcurrentDictionary<Type, Func<TValue, TDetails>> _detailsFactories;
        private readonly JsonSerializerOptions _serializerOptionsMaskSensitiveData;

        protected DetailsProvider(ISensitiveDataMaskConfiguration configuration)
        {
            _detailsFactories = new();
            _serializerOptionsMaskSensitiveData = new JsonSerializerOptions(JsonDefaults.SerializerOptions);
            _serializerOptionsMaskSensitiveData.Converters.Add(new SensitiveDataMaskJsonConverter(configuration));
        }

        public TDetails GetDetails(TValue value)
        {
            var factory = _detailsFactories.GetOrAdd(value.GetType(), _ => GetDetailsFactory(value));
            return factory(value);
        }

        protected abstract Func<TValue, TDetails> GetDetailsFactory(TValue firstValueOfType);

        protected JsonSerializerOptions GetMaskSensitiveDataSerializerOptions() 
            => _serializerOptionsMaskSensitiveData;

        protected string GetDisplayName(Type type) 
            => GetDisplayNameRegex().Replace(type.Name, "$1$3 $2$4");

        [GeneratedRegex("([a-z0-9])([A-Z])|([A-Z])([A-Z][a-z])")]
        private static partial Regex GetDisplayNameRegex();
    }
}
