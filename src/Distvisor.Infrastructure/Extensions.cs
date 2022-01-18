using Distvisor.App.Common;
using Distvisor.App.Core.Events;
using Distvisor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Distvisor.Infrastructure
{
    public static class Extensions
    {
        public static void AddDistvisorInfrastructure(this IServiceCollection services, IConfiguration Config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=TestsDistvisor;Username=postgres;Password=mysecretpassword"));

            services.AddScoped<IEventStorage, SqlEventStorage>();
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
        }
    }
}
