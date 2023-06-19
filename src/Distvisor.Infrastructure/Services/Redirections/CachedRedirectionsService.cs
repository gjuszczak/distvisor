using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Features.Redirections.Queries;
using Distvisor.App.Features.Redirections.Queries.GetRedirectionByName;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Redirections
{
    public class CachedRedirectionsService : ICachedRedirectionsService
    {
        private readonly IMemoryCache _cache;
        private readonly IDispatcher _dispatcher;

        public CachedRedirectionsService(IDispatcher dispatcher, IMemoryCache cache)
        {
            _dispatcher = dispatcher;
            _cache = cache;
        }

        public async Task<RedirectionDto> GetByNameAsync(string name)
        {
            var cacheKey = GetRedirectionCacheKey(name);
            if (_cache.TryGetValue(cacheKey, out RedirectionDto cachedRedirection))
            {
                return cachedRedirection;
            }

            var redirection = await _dispatcher.DispatchAsync(new GetRedirectionByName { Name = name });
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };
            return _cache.Set(cacheKey, redirection, cacheEntryOptions);
        }

        private string GetRedirectionCacheKey(string name)
            => $"{nameof(RedirectionDto)}_{name}";
    }
}
