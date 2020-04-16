using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Reads;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
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
        private readonly ReadStore _context;

        public AuthTokenStore(IMemoryCache cache, ReadStore context)
        {
            _cache = cache;
            _context = context;
        }

        public async Task StoreUserTokenAsync(OAuthToken token, Guid userId)
        {
            var user = _context.Users.FindOne(x=> x.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to store user token. User with id = {userId} does not exists.");
            }

            var tokenEntity = _context.OAuthTokens.Query()
                .Where(x => x.UserId == userId && x.Issuer == token.Issuer)
                .FirstOrDefault();

            if (tokenEntity == null)
            {
                tokenEntity = new OAuthTokenEntity();
                tokenEntity.UserId = userId;
            }

            tokenEntity.Issuer = token.Issuer;
            tokenEntity.AccessToken = token.AccessToken;
            tokenEntity.TokenType = token.TokenType;
            tokenEntity.ExpiresIn = token.ExpiresIn;
            tokenEntity.Scope = token.Scope;
            tokenEntity.RefreshToken = token.RefreshToken;
            tokenEntity.UtcIssueDate = token.UtcIssueDate;

            _context.OAuthTokens.Upsert(tokenEntity);

            SetCache(token.Issuer, userId, token);
            SetCache(token.Issuer, token.AccessToken, userId);
        }

        public async Task ClearUserTokenAsync(OAuthTokenIssuer issuer, Guid userId)
        {
            var token = _context.OAuthTokens.Query()
                  .Where(x => x.UserId == userId && x.Issuer == issuer)
                  .SingleOrDefault();

            if (token == null)
            {
                return;
            }

            _context.OAuthTokens.Delete(token.Id);

            _cache.Remove(GetTokenCacheKey(issuer, userId));
            _cache.Remove(GetUserIdCacheKey(issuer, token.AccessToken));
        }

        public async Task<OAuthToken> GetUserStoredTokenAsync(OAuthTokenIssuer issuer, Guid userId)
        {
            if (!_cache.TryGetValue(GetTokenCacheKey(issuer, userId), out OAuthToken token))
            {
                var tokenEntity = _context.OAuthTokens.Query()
                  .Where(x => x.UserId == userId && x.Issuer == issuer)
                  .SingleOrDefault();

                token = ConvertEntityToToken(tokenEntity);

                SetCache(issuer, userId, token);

                if (token != null)
                {
                    SetCache(issuer, token.AccessToken, userId);
                }
            }

            return token;
        }

        public async Task<Guid?> GetUserIdForTokenAsync(OAuthTokenIssuer issuer, string accessToken)
        {
            if (!_cache.TryGetValue(GetUserIdCacheKey(issuer, accessToken), out Guid? userId))
            {
                var tokenEntity = _context.OAuthTokens.Query()
                    .Where(x => x.AccessToken == accessToken && x.Issuer == issuer)
                    .SingleOrDefault();

                var token = ConvertEntityToToken(tokenEntity);
                userId = tokenEntity?.UserId;

                SetCache(issuer, accessToken, userId);

                if (userId != null && token != null)
                {
                    SetCache(issuer, userId.Value, token);
                }
            }

            return userId;
        }

        private void SetCache(OAuthTokenIssuer issuer, Guid userId, OAuthToken token)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.Set(GetTokenCacheKey(issuer, userId), token, cacheEntryOptions);
        }

        private void SetCache(OAuthTokenIssuer issuer, string accessToken, Guid? userId)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.Set(GetUserIdCacheKey(issuer, accessToken), userId, cacheEntryOptions);
        }

        private string GetTokenCacheKey(OAuthTokenIssuer issuer, Guid userId)
        {
            return $"AuthTokenStore_GetToken_{issuer}_{userId}";
        }

        private string GetUserIdCacheKey(OAuthTokenIssuer issuer, string accessToken)
        {
            return $"AuthTokenStore_GetUserId_{issuer}_{accessToken}";
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
