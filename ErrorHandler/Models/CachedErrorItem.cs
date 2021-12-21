using CacheManager;

namespace ErrorHandler.Models
{
    public class CachedErrorItem : ICacheItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
