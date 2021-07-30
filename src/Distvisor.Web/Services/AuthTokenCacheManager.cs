using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IAuthTokenProvider
    {
        Task<AuthToken> RefreshAuthTokenAsync(string refreshToken);
    }
    public interface IAuthTokenCacheFactory
    {
        IAuthTokenCacheManager Create(IAuthTokenProvider provider, string cacheKey);
    }
    public interface IAuthTokenCacheManager
    {
        void SetAuthToken(AuthToken token);
        Task RetryWithTokenAsync(Func<string, ReadOnlyDictionary<string, string>, Task> action);
        Task<T> RetryWithTokenAsync<T>(Func<string, ReadOnlyDictionary<string, string>, Task<T>> action);
    }

    public class AuthTokenCacheFactory : IAuthTokenCacheFactory
    {
        private readonly IMemoryCache _cache;

        public AuthTokenCacheFactory(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IAuthTokenCacheManager Create(IAuthTokenProvider provider, string cacheKey)
        {
            return new AuthTokenCacheManager(_cache, provider, cacheKey);
        }
    }

    public class AuthTokenCacheManager : IAuthTokenCacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly IAuthTokenProvider _provider;
        private readonly string _cacheKey;
        private readonly MemoryCacheEntryOptions _defaultCacheEntryOptions;

        public AuthTokenCacheManager(IMemoryCache cache, IAuthTokenProvider provider, string cacheKey)
        {
            _cache = cache;
            _provider = provider;
            _cacheKey = cacheKey;
            _defaultCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);
        }

        public async Task RetryWithTokenAsync(Func<string, ReadOnlyDictionary<string, string>, Task> action)
        {
            await RetryWithTokenAsync(async (t, meta) =>
            {
                await action(t, meta);
                return true;
            });
        }

        public async Task<T> RetryWithTokenAsync<T>(Func<string, ReadOnlyDictionary<string, string>, Task<T>> action)
        {
            var token = GetAuthToken();
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    if (token?.AccessToken == null)
                    {
                        throw new InvalidAccessTokenException();
                    }

                    return await action(token.AccessToken, token.Metadata);
                }
                catch (InvalidAccessTokenException)
                {
                    if (i == 0)
                    {
                        token = await RefreshAuthTokenAsync(token?.RefreshToken);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new InvalidOperationException("Unexpected execution state reached.");
        }

        public void SetAuthToken(AuthToken token)
        {
            _cache.Set(_cacheKey, token, _defaultCacheEntryOptions);
        }

        private AuthToken GetAuthToken()
        {
            if (_cache.TryGetValue(_cacheKey, out AuthToken token))
            {
                return token;
            }
            return null;
        }

        private async Task<AuthToken> RefreshAuthTokenAsync( string refreshToken)
        {
            if (refreshToken == null)
            {
                return null;
            }
            var token = await _provider.RefreshAuthTokenAsync(refreshToken);
            _cache.Set(_cacheKey, token, _defaultCacheEntryOptions);
            return token;
        }
    }

    public class AuthToken
    {
        public AuthToken(string accessToken, string refreshToken, params (string key, string value)[] metadata)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Metadata = new ReadOnlyDictionary<string, string>(metadata.ToDictionary(meta => meta.key, meta => meta.value));
        }

        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public ReadOnlyDictionary<string, string> Metadata { get; init; }
    }

    public class InvalidAccessTokenException : Exception
    {
    }
}
