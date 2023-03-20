using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Distvisor.App.Core.Serialization;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class SensitiveDataMaskJsonConverter : JsonConverterFactory
    {
        private readonly Type _type;
        private readonly string _propName;

        public SensitiveDataMaskJsonConverter(Type type, string propName)
        {
            var camelCasePropName = propName.Length == 1
                ? propName[..1].ToLowerInvariant()
                : propName[..1].ToLowerInvariant() + propName[1..];

            _type = type;
            _propName = camelCasePropName;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == _type;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(SensitiveDataMaskJsonConverterInner<>).MakeGenericType(
                    new Type[] { typeToConvert }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { _propName },
                culture: null)!;

            return converter;
        }

        private class SensitiveDataMaskJsonConverterInner<T> : JsonConverter<T>
        {
            private readonly string _propName;

            public SensitiveDataMaskJsonConverterInner(string propName)
            {
                _propName = propName;
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new InvalidOperationException("SensitiveDataMaskJsonConverterInner does not support deserialization.");
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                var jsonNode = JsonSerializer.SerializeToNode(value, JsonDefaults.SerializerOptions);
                jsonNode[_propName] = "*****";
                JsonSerializer.Serialize(writer, jsonNode, JsonDefaults.SerializerOptions);
            }
        }
    }
}
