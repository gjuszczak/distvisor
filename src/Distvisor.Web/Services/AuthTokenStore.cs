using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IAuthTokenStore
    {
        Task ClearUserTokenAsync(OAuthTokenIssuer issuer, Guid userId);
        Task<OAuthToken> GetUserStoredTokenAsync(OAuthTokenIssuer issuer, Guid userId);
        Task<Guid?> GetUserIdForTokenAsync(OAuthTokenIssuer issuer, string accessToken);
        Task StoreUserTokenAsync(OAuthToken token, Guid userId);
    }

    public class AuthTokenStore : IAuthTokenStore
    {
        private readonly IMemoryCache _cache;
        private readonly DistvisorContext _context;

        public AuthTokenStore(IMemoryCache cache, DistvisorContext context)
        {
            ;
            _cache = cache;
            _context = context;
        }

        public async Task StoreUserTokenAsync(OAuthToken token, Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to store user token. User with id = {userId} does not exists.");
            }

            var tokenEntity = await _context.OAuthTokens
                .Where(x => x.User == user && x.Issuer == token.Issuer)
                .SingleOrDefaultAsync();

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

            SetCache(userId, token);
        }

        public async Task ClearUserTokenAsync(OAuthTokenIssuer issuer, Guid userId)
        {
            var token = await _context.OAuthTokens
                  .Where(x => x.User.Id == userId && x.Issuer == issuer)
                  .SingleOrDefaultAsync();
            _context.OAuthTokens.Remove(token);
            await _context.SaveChangesAsync();

            _cache.Remove(TokenCacheKey(issuer, userId));
            _cache.Remove(UserIdCacheKey(token.AccessToken));
        }

        public async Task<OAuthToken> GetUserStoredTokenAsync(OAuthTokenIssuer issuer, Guid userId)
        {
            if (!_cache.TryGetValue(TokenCacheKey(issuer, userId), out OAuthToken token))
            {
                var tokenEntity = await _context.OAuthTokens
                  .Where(x => x.User.Id == userId && x.Issuer == issuer)
                  .SingleOrDefaultAsync();

                token = ConvertEntityToToken(tokenEntity);

                SetCache(userId, token);
            }

            return token;
        }

        public async Task<Guid?> GetUserIdForTokenAsync(OAuthTokenIssuer issuer, string accessToken)
        {
            if (!_cache.TryGetValue(UserIdCacheKey(accessToken), out Guid? userId))
            {
                var tokenEntity = await _context.OAuthTokens
                    .Include(x => x.User)
                    .Where(x => x.AccessToken == accessToken && x.Issuer == issuer)
                    .SingleOrDefaultAsync();

                var token = ConvertEntityToToken(tokenEntity);
                userId = tokenEntity?.User?.Id;

                SetCache(userId, token);
            }

            return userId;
        }

        private void SetCache(Guid? userId, OAuthToken token)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            if (userId.HasValue)
            {
                _cache.Set(TokenCacheKey(token.Issuer, userId.Value), token, cacheEntryOptions);
            }
            _cache.Set(UserIdCacheKey(token.AccessToken), (userId, token), cacheEntryOptions);
        }

        private string TokenCacheKey(OAuthTokenIssuer issuer, Guid userId)
        {
            return $"AuthTokenStore_Token_{issuer.ToString()}_{userId}";
        }

        private string UserIdCacheKey(string accessToken)
        {
            return $"AuthTokenStore_UserId_{accessToken}";
        }

        private OAuthToken ConvertEntityToToken(OAuthTokenEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new OAuthToken
            {
                Issuer = entity.Issuer,
                AccessToken = entity.AccessToken,
                ExpiresIn = entity.ExpiresIn,
                RefreshToken = entity.RefreshToken,
                Scope = entity.Scope,
                TokenType = entity.TokenType,
                UtcIssueDate = entity.UtcIssueDate,
            };
        }
    }

    public class OAuthToken
    {
        public OAuthTokenIssuer Issuer { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime UtcIssueDate { get; set; }
    }
}
