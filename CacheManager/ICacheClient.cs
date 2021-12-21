using System;

namespace CacheManager
{
    public interface ICacheClient
    {
        bool Delete(string key);

        T Get<T>(string key) where T : ICacheItem;

        bool Set<T>(string key, T value, TimeSpan? expiry = null) where T : ICacheItem;
    }
}
