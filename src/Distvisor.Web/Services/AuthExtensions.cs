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

    public class AuthHandler: AuthenticationHandler<AuthOptions>
    {
        private readonly IAuthService _users;

        public AuthHandler(
            IOptionsMonitor<AuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthService users)
            : base(options, logger, encoder, clock)
        {
            _users = users;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
                return AuthenticateResult.NoResult();

            if (!"Bearer".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var authResult = await _users.AuthenticateAsync(headerValue.Parameter);
            if (!authResult.IsAuthenticated)
                return AuthenticateResult.Fail(authResult.Message);

            var claims = new[] { new Claim(ClaimTypes.Name, authResult.Username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }

    public static class AuthExtensions
    {
        public static AuthenticationBuilder AddDistvisorAuth(this IServiceCollection services)
        {
            return services.AddAuthentication("Distvisor")
                .AddScheme<AuthOptions, AuthHandler>(
                    "Distvisor", x => { });
        }
    }
}
