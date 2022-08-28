using System;
using System.Buffers;
using System.Text.Json;

namespace Distvisor.App.Core.Serialization
{
    public static class JsonExtensions
    {
        public static TValue Deserialize<TValue>(this JsonDocument document, JsonSerializerOptions options = null)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            return document.RootElement.Deserialize<TValue>(options);
        }

        public static object Deserialize(this JsonDocument document, Type returnType, JsonSerializerOptions options = null)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            return document.RootElement.Deserialize(returnType, options);
        }

        public static TValue Deserialize<TValue>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            return JsonSerializer.Deserialize<TValue>(bufferWriter.WrittenSpan, options);
        }

        public static object Deserialize(this JsonElement element, Type returnType, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
        }

        public static JsonDocument SerializeToDocument(this object value, Type inputType, JsonSerializerOptions options = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, inputType, options);
            return JsonDocument.Parse(bytes);
        }

        public static JsonDocument SerializeToDocument<TValue>(this TValue value, JsonSerializerOptions options = default)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, options);
            return JsonDocument.Parse(bytes);
        }
    }
}
