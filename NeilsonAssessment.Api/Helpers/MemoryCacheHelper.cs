using Microsoft.Extensions.Caching.Memory;

namespace NeilsonAssessment.Api.Helpers
{
    public class MemoryCacheHelper
    {
        private IMemoryCache _cache;

        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public object CacheTryGetValue(string key)
        {
            _cache.TryGetValue(key, out var value);

            return value;
        }

        public void CacheTrySetValue(string key, object value)
        {
            if(!_cache.TryGetValue(key, out var cacheValue))
            {
                cacheValue = value;

                _cache.Set(key, cacheValue);
            }
        }

        public void CacheTryUpdateValue(string key, object value)
        {
            _cache.Set(key, value);
        }

        public void CacheTryRemoveValue(string key)
        {
            if(_cache.TryGetValue(key, out var value))
            {
                _cache.Remove(key);
            }
        }
    }
}
