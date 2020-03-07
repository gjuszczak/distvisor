using Distvisor.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Distvisor.Web
{
    public static class StartupExtensions
    {
        public static IHttpClientBuilder AddProdOrDevHttpClient<TClient, TProdImplementation, TDevImplementation>(
            this IServiceCollection services,
            IWebHostEnvironment env,
            IConfiguration config
            )
            where TClient : class
            where TProdImplementation : class, TClient
            where TDevImplementation : class, TClient
        {
            var isDevelop = env.IsDevelopment();
            var isRemoteRequestDisabledOnDev = config.GetValue<bool>("DisableRemoteRequestsOnDevEnv");

            if (isDevelop && isRemoteRequestDisabledOnDev)
            {
                return services.AddHttpClient<TClient, TDevImplementation>();
            }
            else
            {
                return services.AddHttpClient<TClient, TProdImplementation>();
            }
        }
    }
}
