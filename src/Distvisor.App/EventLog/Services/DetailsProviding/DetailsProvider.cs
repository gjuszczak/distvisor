using Distvisor.App.EventLog.Services.PayloadMasking;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public abstract class DetailsProvider<TKey, TValue, TDetails>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<TKey, Func<TValue, TDetails>> _detailsFactories;

        protected DetailsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _detailsFactories = new();
        }

        public TDetails GetDetails(TValue value)
        {
            var factory = _detailsFactories.GetOrAdd(GetDetailsFactoryKey(value), factoryKey => GetDetailsFactory(factoryKey));
            return factory(value);
        }

        protected abstract TKey GetDetailsFactoryKey(TValue value);

        protected abstract Func<TValue, TDetails> GetDetailsFactory(TKey factoryKey);

        protected IPayloadMaskingService GetMaskingService(Type type)
        {
            var maskingServiceType = typeof(IPayloadMaskingService<>).MakeGenericType(type);
            var maskingService = (IPayloadMaskingService)_serviceProvider.GetService(maskingServiceType);
            return maskingService;
        }

        protected string GetDisplayName(Type type)
        {
            return Regex.Replace(type.Name, "([a-z0-9])([A-Z])|([A-Z])([A-Z][a-z])", "$1$3 $2$4");
        }
    }
}
