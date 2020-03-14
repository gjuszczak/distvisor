using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class AuthOptions : AuthenticationSchemeOptions
    {
    }

    public class AuthHandler : AuthenticationHandler<AuthOptions>
    {
        private readonly IDistvisorAuthService _authService;

        public AuthHandler(
            IOptionsMonitor<AuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IDistvisorAuthService authService)
            : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = GetTokenFromHeader() ?? GetTokenFromUrlParam();
            if (token == null)
            {
                return AuthenticateResult.NoResult();
            }

            var authResult = await _authService.AuthenticateAsync(token);
            if (!authResult.IsAuthenticated)
            {
                return AuthenticateResult.Fail(authResult.Message);
            }

            var claims = new[] { 
                new Claim(ClaimTypes.Name, authResult.Username),
                new Claim(ClaimTypes.NameIdentifier, authResult.UserId.ToString()),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        private string GetTokenFromHeader()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return null;

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
                return null;

            if (!"Bearer".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
                return null;

            return headerValue.Parameter;
        }

        private string GetTokenFromUrlParam()
        {
            if (!Request.Query.ContainsKey("access_token"))
                return null;

            if (!Request.Path.StartsWithSegments("/notificationshub"))
                return null;

            var sessionId = Request.Query["access_token"];

            if (string.IsNullOrEmpty(sessionId))
                return null;

            return sessionId;
        }
    }

    public static class DistvisorAuthExtensions
    {
        public static AuthenticationBuilder AddDistvisorAuth(this IServiceCollection services)
        {
            return services.AddAuthentication("Distvisor")
                .AddScheme<AuthOptions, AuthHandler>(
                    "Distvisor", x => { });
        }
    }
}
