using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly DistvisorContext _context;
        private readonly IMemoryCache _cache;

        public RedirectionsService(DistvisorContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<RedirectionDetails> GetRedirectionAsync(string name)
        {
            if (!_cache.TryGetValue(name, out RedirectionDetails redirection))
            {
                var entity = await _context.Redirections.FindAsync(name);

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
            var e = new RedirectionEntity() { Name = name };
            _context.Redirections.Attach(e);
            _context.Redirections.Remove(e);
            await _context.SaveChangesAsync();

            _cache.Remove(name);
        }

        public async Task<IEnumerable<RedirectionDetails>> ListRedirectionsAsync()
        {
            var entities = await _context.Redirections.ToListAsync();
            return entities.Select(x => new RedirectionDetails(x.Name, x.Url)).ToList();
        }

        public async Task ConfigureRedirectionAsync(RedirectionDetails redirection)
        {
            var entity = await _context.Redirections.FindAsync(redirection.Name);
            if (entity == null)
            {
                entity = new RedirectionEntity { Name = redirection.Name };
                _context.Redirections.Add(entity);
            }

            entity.Url = redirection.Url.ToString();

            await _context.SaveChangesAsync();

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
