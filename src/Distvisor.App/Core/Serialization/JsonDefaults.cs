using System.Text.Json;
using System.Text.Json.Serialization;

namespace Distvisor.App.Core.Serialization
{
    public static class JsonDefaults
    {
        static JsonDefaults()
        {
            // it's done this 'dirty' way because I want to share the same serializer options
            // between App/Core, EntityFrameworkCore and ASP Net Core MVC

            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            Configure(serializerOptions);
            SerializerOptions = serializerOptions;
        }

        public static JsonSerializerOptions SerializerOptions { get; }

        public static void Configure(JsonSerializerOptions options)
        {
            options.Converters.Add(new JsonStringEnumConverter());
        }
    }
}
