using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        var username = ctx.Principal.FindFirstValue("preferred_username");
                        var role = (roles?.Admin?.Contains(username) ?? false) ? "admin" : "guest";
                        var appClaims = new[]
                        {
                            new Claim(ClaimTypes.Name, username),
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
                policy.RequireRole("admin");
                options.DefaultPolicy = policy.Build();
            });
        }
    }
}
