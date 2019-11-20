using System.Collections.Concurrent;

namespace Distvisor.Web.Services
{
    public class AuthCache : IAuthCache
    {
        private readonly ConcurrentDictionary<string, AuthResult> _cache;

        public AuthCache()
        {
            _cache = new ConcurrentDictionary<string, AuthResult>();
        }

        public AuthResult Get(string sessionId)
        {
            _cache.TryGetValue(sessionId, out AuthResult value);
            return value;
        }

        public void Set(string sessionId, AuthResult value)
        {
            _cache[sessionId] = value;
        }

        public void Remove(string sessionId)
        {
            _cache.TryRemove(sessionId, out _);
        }
    }
}
