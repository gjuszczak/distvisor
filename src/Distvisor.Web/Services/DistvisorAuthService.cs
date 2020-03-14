using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IDistvisorAuthService
    {
        Task<AuthResult> AuthenticateAsync(string accessToken);
        Task<AuthResult> LoginAsync(string username, string password);
        Task LogoutAsync(Guid userId);
        Task<AuthResult> RefreshAccessTokenAsync(string refreshToken);
    }

    public class DistvisorAuthService : IDistvisorAuthService
    {
        private readonly DistvisorContext _context;
        private readonly ICryptoService _crypto;
        private readonly IAuthTokenStore _tokenStore;
        private readonly IMemoryCache _cache;

        public DistvisorAuthService(
            DistvisorContext context,
            ICryptoService crypto,
            IAuthTokenStore tokenStore,
            IMemoryCache cache)
        {
            _context = context;
            _crypto = crypto;
            _tokenStore = tokenStore;
            _cache = cache;
        }

        public async Task<AuthResult> AuthenticateAsync(string accessToken)
        {
            var userId = await _tokenStore.GetUserIdForTokenAsync(OAuthTokenIssuer.Distvisor, accessToken);
            if (userId == null)
            {
                return AuthResult.Fail("Token invalid or expired");
            }

            var token = await _tokenStore.GetUserStoredTokenAsync(OAuthTokenIssuer.Distvisor, userId.Value);
            if (userId == null)
            {
                return AuthResult.Fail("Token invalid or expired");
            }

            if (DateTime.UtcNow > token.UtcIssueDate.AddSeconds(token.ExpiresIn))
            {
                return AuthResult.Fail("Token invalid or expired");
            }

            var username = await GetUsernameAsync(userId.Value);
            var result = AuthResult.Success(userId.Value, username, token);
            return result;
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            if (!await _context.Users.AnyAsync())
            {
                await CreateUserAsync(username, password);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return AuthResult.Fail("Invalid username or password");
            }

            var lockoutSeconds = (int)(user.LockoutUtc - DateTime.UtcNow).TotalSeconds - 50;
            if (lockoutSeconds > 0)
            {
                return AuthResult.Fail($"Lockout for {lockoutSeconds} seconds.");
            }

            var authenticated = (_crypto.ValidatePasswordHash(password, user.PasswordHash));
            if (!authenticated)
            {
                user.LockoutUtc = user.LockoutUtc > DateTime.UtcNow ? user.LockoutUtc.AddSeconds(10) : DateTime.UtcNow.AddSeconds(10);
                await _context.SaveChangesAsync();
                return AuthResult.Fail("Invalid username or password");
            }

            var token = GenerateNewToken();
            await _tokenStore.StoreUserTokenAsync(token, user.Id);
            return AuthResult.Success(user.Id, user.Username, token);
        }

        public Task LogoutAsync(Guid userId)
        {
            return _tokenStore.ClearUserTokenAsync(OAuthTokenIssuer.Distvisor, userId);
        }

        public async Task<AuthResult> RefreshAccessTokenAsync(string refreshToken)
        {
            var user = await _context.OAuthTokens
                .Where(x => x.RefreshToken == refreshToken && x.Issuer == OAuthTokenIssuer.Distvisor)
                .Select(x => x.User)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return AuthResult.Fail("Invalid refresh token");
            }

            var token = GenerateNewToken();
            await _tokenStore.StoreUserTokenAsync(token, user.Id);
            return AuthResult.Success(user.Id, user.Username, token);
        }

        private OAuthToken GenerateNewToken()
        {
            return new OAuthToken
            {
                Issuer = OAuthTokenIssuer.Distvisor,
                AccessToken = _crypto.GenerateRandomToken(),
                RefreshToken = _crypto.GenerateRandomToken(),
                ExpiresIn = 3600,
                Scope = "distvisor",
                TokenType = "bearer",
                UtcIssueDate = DateTime.UtcNow,
            };
        }

        private async Task<string> GetUsernameAsync(Guid userId)
        {
            var cacheKey = $"DistvisorAuthService_Username_{userId}";
            if (!_cache.TryGetValue(cacheKey, out string username))
            {
                var user = await _context.Users.FindAsync(userId);
                username = user.Username;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.NeverRemove);

                _cache.Set(cacheKey, username, cacheEntryOptions);
            }

            return username;
        }

        private Task CreateUserAsync(string username, string password)
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
    }

    public class AuthResult
    {
        private AuthResult() { }

        public bool IsAuthenticated { get; private set; }
        public string Message { get; private set; }
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public OAuthToken Token { get; private set; }

        public static AuthResult Success(Guid userId, string username, OAuthToken token)
        {
            return new AuthResult
            {
                IsAuthenticated = true,
                Message = "Ok",
                UserId = userId,
                Username = username,
                Token = token,
            };
        }

        public static AuthResult Fail(string message)
        {
            return new AuthResult
            {
                IsAuthenticated = false,
                Message = message,
                UserId = Guid.Empty,
                Username = null,
                Token = null,
            };
        }
    }
}
