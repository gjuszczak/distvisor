using Distvisor.Web.Data;
using Distvisor.Web.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class DistvisorAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class DistvisorAuthenticationHandler
        : AuthenticationHandler<DistvisorAuthenticationOptions>
    {
        private readonly DistvisorContext _context;

        public DistvisorAuthenticationHandler(
            IOptionsMonitor<DistvisorAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DistvisorContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
                return AuthenticateResult.NoResult();

            if (!"Bearer".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var user = await GetUserAsync(headerValue.Parameter);
            if (user == null)
                return AuthenticateResult.Fail("User session not found");

            var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        private async Task<User> GetUserAsync(string sessionId)
        {
            var session = await _context.Sessions
                .Include(x => x.User)
                .Where(x => DateTime.UtcNow < x.ExpireOnUtc)
                .FirstOrDefaultAsync(x => x.Id == sessionId);
            return session?.User;
        }
    }

    public static class DistvisorAuthenticationExtensions
    {
        public static AuthenticationBuilder AddDistvisorAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication("Distvisor")
                .AddScheme<DistvisorAuthenticationOptions, DistvisorAuthenticationHandler>(
                    "Distvisor", x => { });
        }
    }
}
