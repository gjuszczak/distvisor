using Distvisor.App.Core.Serialization;
using Distvisor.App.HomeBox.ValueObjects;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public abstract class DetailsProvider<TKey, TValue, TDetails>
    {
        private readonly ConcurrentDictionary<TKey, Func<TValue, TDetails>> _detailsFactories;

        protected DetailsProvider()
        {
            _detailsFactories = new();
        }

        public TDetails GetDetails(TValue value)
        {
            var factory = _detailsFactories.GetOrAdd(GetDetailsFactoryKey(value), factoryKey => GetDetailsFactory(factoryKey));
            return factory(value);
        }

        protected abstract TKey GetDetailsFactoryKey(TValue value);

        protected abstract Func<TValue, TDetails> GetDetailsFactory(TKey factoryKey);

        protected JsonSerializerOptions GetMaskSensitiveDataSerializerOptions()
        {
            var serializerOptions = new JsonSerializerOptions(JsonDefaults.SerializerOptions);
            serializerOptions.Converters.Add(new SensitiveDataMaskJsonConverter(typeof(GatewayToken), nameof(GatewayToken.AccessToken)));
            return serializerOptions;
        }

        protected string GetDisplayName(Type type)
        {
            return Regex.Replace(type.Name, "([a-z0-9])([A-Z])|([A-Z])([A-Z][a-z])", "$1$3 $2$4");
        }
    }
}
