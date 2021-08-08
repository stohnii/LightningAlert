using LightningAlert.BAL.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace LightningAlert.BAL
{
    public class InMemoryCache : ICacheProvider
    {
        private IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddKey(string quadKey)
        { 
            _cache.Set(quadKey, quadKey); 
        }

        public bool KeyExists(string key)
        {
            return _cache.TryGetValue(key, out object val);
        }
    }
}
