using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distvisor.Web
{
    public static class StartupExtensions
    {
        public static IHttpClientBuilder AddProdOrDevHttpClient<TClient, TImplementation, TFakeImplementation>(
            this IServiceCollection services,
            IConfiguration config
            )
            where TClient : class
            where TImplementation : class, TClient
            where TFakeImplementation : class, TClient
        {
            var useFakeApi = config.GetValue<bool>("UseFakeApi");

            if (useFakeApi)
            {
                return services.AddHttpClient<TClient, TFakeImplementation>();
            }
            else
            {
                return services.AddHttpClient<TClient, TImplementation>();
            }
        }
    }
}
