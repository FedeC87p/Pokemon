using Microsoft.Extensions.Caching.Memory;
using PokemonAPI.ApplicationCore.CacheKeys;

namespace PokemonAPI.ApplicationCore
{
    public class CacheService : ICacheService
    {
        readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(ICacheKey<T> key) where T : class
        {
            if (_memoryCache == null)
            {
                return null;
            }
            return _memoryCache.Get<T>(key.CacheKey);
        }

        public void Set<T>(ICacheKey<T> key, T value) where T : class
        {
            if (_memoryCache == null)
            {
                return;
            }
            _memoryCache.Set(key.CacheKey, value);
        }
    }
}
