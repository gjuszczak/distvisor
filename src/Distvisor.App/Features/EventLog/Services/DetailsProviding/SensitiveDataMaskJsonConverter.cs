using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Distvisor.App.Core.Serialization;

namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public class SensitiveDataMaskJsonConverter : JsonConverterFactory
    {
        private readonly ISensitiveDataMaskConfiguration _configuration;

        public SensitiveDataMaskJsonConverter(ISensitiveDataMaskConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return _configuration.IsTypeMasked(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(SensitiveDataMaskJsonConverterInner<>).MakeGenericType(
                    new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { _configuration.GetMaskedProperties(typeToConvert), _configuration.MaskString },
                culture: null)!;

            return converter;
        }

        private class SensitiveDataMaskJsonConverterInner<T> : JsonConverter<T>
        {
            private readonly IEnumerable<string> _propNames;
            private readonly string _maskString;

            public SensitiveDataMaskJsonConverterInner(IEnumerable<string> propNames, string maskString)
            {
                _propNames = propNames;
                _maskString = maskString;
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new InvalidOperationException("SensitiveDataMaskJsonConverterInner does not support deserialization.");
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                var jsonNode = JsonSerializer.SerializeToNode(value, JsonDefaults.SerializerOptions);
                foreach (var propName in _propNames)
                {
                    jsonNode[propName] = _maskString;
                }
                JsonSerializer.Serialize(writer, jsonNode, JsonDefaults.SerializerOptions);
            }
        }
    }
}
