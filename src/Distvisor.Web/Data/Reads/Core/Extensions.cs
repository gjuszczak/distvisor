using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Distvisor.Web.Data.Reads.Core
{
    public static class Extensions
    {
        public static void AddReadStore(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ReadStoreContext>(options => options.UseNpgsql(connectionString));
            services.AddHostedService<ReadStoreBootstrap>();
        }
    }
}
