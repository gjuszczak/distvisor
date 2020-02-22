using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftAuthTokenStore
    {
        Task<OAuthToken> GetUserActiveToken();
        Task<OAuthToken> GetUserStoredToken();
        Task StoreUserTokenAsync(OAuthToken token);
        Task StoreUserTokenAsync(OAuthToken token, Guid userId);
    }

    public class MicrosoftAuthTokenStore : IMicrosoftAuthTokenStore
    {
        private readonly IUserInfoProvider _userInfo;
        private readonly IMemoryCache _cache;
        private readonly IMicrosoftAuthService _authService;
        private readonly DistvisorContext _context;

        public MicrosoftAuthTokenStore(IUserInfoProvider userInfo, IMemoryCache cache, IMicrosoftAuthService authService, DistvisorContext context)
        {
            _userInfo = userInfo;
            _cache = cache;
            _authService = authService;
            _context = context;
        }

        public Task StoreUserTokenAsync(OAuthToken token)
        {
            return StoreUserTokenAsync(token, _userInfo.UserId);
        }

        public async Task StoreUserTokenAsync(OAuthToken token, Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to store user token. User with id = {userId} does not exists.");
            }

            var tokenEntity = await _context.OAuthTokens
                .Where(x => x.User == user && x.Issuer == OAuthTokenIssuer.MicrosoftService)
                .FirstOrDefaultAsync();

            if (tokenEntity == null)
            {
                tokenEntity = new OAuthTokenEntity();
                tokenEntity.User = user;
                _context.OAuthTokens.Add(tokenEntity);
            }

            tokenEntity.Issuer = token.Issuer;
            tokenEntity.AccessToken = token.AccessToken;
            tokenEntity.TokenType = token.TokenType;
            tokenEntity.ExpiresIn = token.ExpiresIn;
            tokenEntity.Scope = token.Scope;
            tokenEntity.RefreshToken = token.RefreshToken;
            tokenEntity.UtcIssueDate = token.UtcIssueDate;

            await _context.SaveChangesAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(TokenCacheKey(userId), token, cacheEntryOptions);
        }

        public async Task<OAuthToken> GetUserStoredToken()
        {
            if (!_cache.TryGetValue(TokenCacheKey(), out OAuthToken token))
            {
                var tokenEntity = await _context.OAuthTokens
                  .Where(x => x.User.Id == _userInfo.UserId && x.Issuer == OAuthTokenIssuer.MicrosoftService)
                  .FirstOrDefaultAsync();

                if (tokenEntity != null)
                {
                    token = new OAuthToken
                    {
                        AccessToken = tokenEntity.AccessToken,
                        ExpiresIn = 0,
                        RefreshToken = tokenEntity.RefreshToken,
                        Scope = tokenEntity.Scope,
                        TokenType = tokenEntity.TokenType,
                    };
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.NeverRemove);

                _cache.Set(TokenCacheKey(), token, cacheEntryOptions);
            }

            return token;
        }

        public async Task<OAuthToken> GetUserActiveToken()
        {
            var token = await GetUserStoredToken();
            var tokenExpiration = token.UtcIssueDate
                .ToLocalTime()
                .AddSeconds(token.ExpiresIn)
                .AddMinutes(-1);
            
            if (tokenExpiration > DateTime.Now)
            {
                return token;
            }

            var refreshedToken = await _authService.RefreshAccessToken(token.RefreshToken);
            return refreshedToken;
        }
        
        private string TokenCacheKey()
        {
            return TokenCacheKey(_userInfo.UserId);
        }

        private string TokenCacheKey(Guid userId)
        {
            return $"MicrosoftToken_{userId}";
        }
    }
}
