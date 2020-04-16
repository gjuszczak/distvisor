using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IRedirectionsService
    {
        Task<RedirectionDetails> GetRedirectionAsync(string name);
        Task RemoveRedirectionAsync(string name);
        Task<IEnumerable<RedirectionDetails>> ListRedirectionsAsync();
        Task ConfigureRedirectionAsync(RedirectionDetails redirection);
    }

    public class RedirectionsService : IRedirectionsService
    {
        private readonly ReadStore _context;
        private readonly IEventStore _eventStore;
        private readonly IMemoryCache _cache;

        public RedirectionsService(ReadStore context, IEventStore eventStore, IMemoryCache cache)
        {
            _context = context;
            _eventStore = eventStore;
            _cache = cache;
        }

        public async Task<RedirectionDetails> GetRedirectionAsync(string name)
        {
            if (!_cache.TryGetValue(name, out RedirectionDetails redirection))
            {
                var entity = _context.Redirections.FindOne(x => x.Name == name);

                if (entity != null)
                {
                    redirection = new RedirectionDetails(entity.Name, entity.Url);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.NeverRemove);

                _cache.Set(name, redirection, cacheEntryOptions);
            }

            return redirection;
        }

        public async Task RemoveRedirectionAsync(string name)
        {
            _eventStore.Publish(new RemoveRedirectionEvent
            {
                Name = name,
            });

            _cache.Remove(name);
        }

        public async Task<IEnumerable<RedirectionDetails>> ListRedirectionsAsync()
        {
            var entities = _context.Redirections.FindAll();
            return entities.Select(x => new RedirectionDetails(x.Name, x.Url)).ToList();
        }

        public async Task ConfigureRedirectionAsync(RedirectionDetails redirection)
        {
            _eventStore.Publish(new SetRedirectionEvent
            {
                Name = redirection.Name,
                Url = redirection.Url.ToString()
            }); ;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(redirection.Name, redirection, cacheEntryOptions);
        }
    }

    public class RedirectionDetails
    {
        public RedirectionDetails()
        {
        }

        public RedirectionDetails(string name, string url)
        {
            this.Name = name;
            this.Url = new Uri(url);
        }

        public string Name { get; set; }
        public Uri Url { get; set; }
    }
}
