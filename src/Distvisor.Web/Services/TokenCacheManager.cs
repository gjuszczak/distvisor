using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface ITokenProvider
    {
        Task<Token> GetTokenAsync();
        Task<Token> RefreshTokenAsync(string refreshToken);
    }
    public interface ITokenCacheManager
    {
        Task RetryWithToken(ITokenProvider provider, Func<string, ReadOnlyDictionary<string, string>, Task> action);
        Task<T> RetryWithToken<T>(ITokenProvider tokenProvider, Func<string, ReadOnlyDictionary<string, string>, Task<T>> action);
    }

    public class TokenCacheManager : ITokenCacheManager
    {
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _defaultCacheEntryOptions;

        public TokenCacheManager(IMemoryCache cache)
        {
            _cache = cache;
            _defaultCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);
        }

        public async Task RetryWithToken(ITokenProvider provider, Func<string, ReadOnlyDictionary<string, string>, Task> action)
        {
            await RetryWithToken(provider, async (t, meta) =>
            {
                await action(t, meta);
                return true;
            });
        }

        public async Task<T> RetryWithToken<T>(ITokenProvider provider, Func<string, ReadOnlyDictionary<string, string>, Task<T>> action)
        {
            var token = await this.GetTokenAsync(provider);
            for (int i = 0; true; i++)
            {
                try
                {
                    if (token == null)
                    {
                        throw new InvalidTokenException();
                    }

                    return await action(token.AccessToken, token.Metadata);
                }
                catch (InvalidTokenException)
                {
                    switch (i)
                    {
                        case 0:
                            token = await this.GetRefreshedTokenAsync(provider, token?.RefreshToken);
                            break;
                        case 1:
                            token = await this.GetFreshTokenAsync(provider);
                            break;
                        default:
                            throw;
                    }
                }
            }
        }

        private async Task<Token> GetTokenAsync(ITokenProvider provider)
        {
            var cacheKey = provider.GetType().FullName;
            if (_cache.TryGetValue(cacheKey, out Token token))
            {
                return token;
            }
            return await GetFreshTokenAsync(provider, cacheKey);
        }

        private async Task<Token> GetRefreshedTokenAsync(ITokenProvider provider, string refreshToken)
        {
            if (refreshToken == null)
            {
                return null;
            }

            var cacheKey = provider.GetType().FullName;
            var token = await provider.RefreshTokenAsync(refreshToken);
            _cache.Set(cacheKey, token, _defaultCacheEntryOptions);
            return token;
        }

        private async Task<Token> GetFreshTokenAsync(ITokenProvider provider, string cacheKey = null)
        {
            if (cacheKey == null)
            {
                cacheKey = provider.GetType().FullName;
            }
            var token = await provider.GetTokenAsync();
            _cache.Set(cacheKey, token, _defaultCacheEntryOptions);
            return token;
        }

    }

    public class Token
    {
        public Token(string accessToken, string refreshToken, params (string key, string value)[] metadata)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Metadata = new ReadOnlyDictionary<string, string>(metadata.ToDictionary(meta => meta.key, meta => meta.value));
        }

        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public ReadOnlyDictionary<string, string> Metadata { get; init; }
    }

    public class InvalidTokenException : Exception
    {
    }
}
