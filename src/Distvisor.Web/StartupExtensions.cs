using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Distvisor.Web
{
    public static class StartupExtensions
    {
        public static IHttpClientBuilder AddHttpClient<TClient, TImplementation, TFakeImplementation>(
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

        public static void AddDistvisorAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var azureAd = config.GetSection("AzureAd").Get<AzureAdConfiguration>();
                var roles = config.GetSection("Roles").Get<RolesConfiguration>();
                var issuer = azureAd.Instance + azureAd.TenantId + "/v2.0";
                options.Authority = issuer;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new[] { azureAd.ClientId },
                    ValidIssuers = new[] { issuer },
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        var username = ctx.Principal.FindFirst("preferred_username").Value;
                        var role = (roles?.User?.Contains(username) ?? false) ? "user" : "guest";
                        var appClaims = new[]
                        {
                                new Claim(ClaimTypes.Role, role)
                            };
                        ctx.Principal.AddIdentity(new ClaimsIdentity(appClaims));
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder(options.DefaultPolicy);
                policy.RequireRole("user");
                options.DefaultPolicy = policy.Build();
            });
        }
    }
}
