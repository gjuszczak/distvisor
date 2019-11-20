using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly DistvisorContext _context;
        private readonly ICryptoService _crypto;
        private readonly IAuthCache _cache;

        public AuthService(DistvisorContext context, ICryptoService crypto, IAuthCache cache)
        {
            _context = context;
            _crypto = crypto;
            _cache = cache;
        }

        public Task CreateUserAsync(string username, string password)
        {
            var newUser = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = _crypto.GeneratePasswordHash(password),
                LockoutUtc = DateTime.UtcNow,
            };
            _context.Add(newUser);
            return _context.SaveChangesAsync();
        }

        public Task<bool> AnyAsync()
        {
            return _context.Users.AnyAsync();
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return AuthResult.Fail("Invalid username or password");

            var lockoutSeconds = (int)(user.LockoutUtc - DateTime.UtcNow).TotalSeconds - 50;
            if (lockoutSeconds > 0)
                return AuthResult.Fail($"Lockout for {lockoutSeconds} seconds.");

            var authenticated = (_crypto.ValidatePasswordHash(password, user.PasswordHash));
            if (!authenticated)
            {
                user.LockoutUtc = user.LockoutUtc > DateTime.UtcNow ? user.LockoutUtc.AddSeconds(10) : DateTime.UtcNow.AddSeconds(10);
                await _context.SaveChangesAsync();
                return AuthResult.Fail("Invalid username or password");
            }

            if (user.SessionId == null || user.SessionExpirationUtc < DateTime.UtcNow)
            {
                user.SessionId = _crypto.GenerateRandomSessionId();
                user.SessionStartUtc = DateTime.UtcNow;
                user.SessionExpirationUtc = DateTime.Now.AddDays(1);

                await _context.SaveChangesAsync();
            }
            return AuthResult.Success(user.Username, user.SessionId, user.SessionExpirationUtc);
        }

        public async Task<AuthResult> AuthenticateAsync(string sessionId)
        {
            var cachedResult = _cache.Get(sessionId);
            if (cachedResult != null && DateTime.UtcNow < cachedResult.SessionExpirationUtc)
                return cachedResult;

            _cache.Remove(sessionId);

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.SessionId == sessionId && DateTime.UtcNow < x.SessionExpirationUtc);

            if (user == null)
                return AuthResult.Fail("Session invalid or expired");

            var result = AuthResult.Success(user.Username, user.SessionId, user.SessionExpirationUtc);
            _cache.Set(sessionId, result);
            return result;
        }

        public async Task LogoutAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return;

            if (user.SessionId != null)
                _cache.Remove(user.SessionId);
            
            user.SessionId = null;
            await _context.SaveChangesAsync();
        }
    }

    public class AuthResult
    {
        private AuthResult() { }

        public bool IsAuthenticated { get; private set; }
        public string Message { get; private set; }
        public string Username { get; private set; }
        public string SessionId { get; private set; }
        public DateTime SessionExpirationUtc {get; private set;}

        public static AuthResult Success(string username, string sessionId, DateTime sessionExpirationUtc)
        {
            return new AuthResult
            {
                IsAuthenticated = true,
                Message = "Ok",
                Username = username,
                SessionId = sessionId,
                SessionExpirationUtc = sessionExpirationUtc,
            };
        }

        public static AuthResult Fail(string message)
        {
            return new AuthResult
            {
                IsAuthenticated = false,
                Message = message,
                Username = null,
                SessionId = null,
                SessionExpirationUtc = DateTime.MinValue,
            };
        }
    }
}
