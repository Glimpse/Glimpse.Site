using System;
using System.Collections.Generic;

namespace Glimpse.Issues.Test
{
    public class CacheProvider
    {
        private readonly int _minutesToExpire;
        private readonly Dictionary<string, GlimpseCacheItem> _cache = new Dictionary<string, GlimpseCacheItem>();

        public CacheProvider() :this(30)
        {
            
        }

        public CacheProvider(int minutesToExpire)
        {
            _minutesToExpire = minutesToExpire;
        }

        public virtual object Get(string key)
        {
            GlimpseCacheItem cacheItem;
            _cache.TryGetValue(key, out cacheItem);
            return cacheItem != null ? cacheItem.Value : null;
        }

        public void Add(string key, object value)
        {
            var glimpseCacheItem = new GlimpseCacheItem(value, DateTime.Now.AddMinutes(_minutesToExpire));
            if(!_cache.ContainsKey(key))
                _cache.Add(key, glimpseCacheItem);
            else
                _cache[key] = glimpseCacheItem;
        }
    }


    internal class GlimpseCacheItem
    {
        private readonly object _value;
        private readonly DateTime _expireTime;

        public GlimpseCacheItem(object value, DateTime expireTime)
        {
            _value = value;
            _expireTime = expireTime;
        }

        public object Value { get { return HasExpired() ? null : _value; } }

        private bool HasExpired()
        {
            return _expireTime <= DateTime.Now;
        }
    }
}