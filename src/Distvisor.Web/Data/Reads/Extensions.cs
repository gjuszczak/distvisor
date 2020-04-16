using Microsoft.Extensions.DependencyInjection;

namespace Distvisor.Web.Data.Reads
{
    public static class Extensions
    {
        public static void AddReadStore(this IServiceCollection services)
        {
            services.AddScoped<ReadStore>();
        }
    }
}
