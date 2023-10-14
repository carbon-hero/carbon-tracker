using System;
using CacheManager.Core;

namespace Core
{
    public static class Cache
    {
        private static ICacheManager<object> _cache;

        private static ICacheManager<object> CacheMan
        {
            get
            {
                if (_cache != null) return _cache;
                _cache = CacheFactory.Build(settings =>
                {
                    settings
                        .WithUpdateMode(CacheUpdateMode.Up)
                        .WithDictionaryHandle()
                        .EnablePerformanceCounters()
                        .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(90));
                });
                return _cache;
            }
        }

        public static bool Set(string key, object value)
        {
            return CacheMan.Add(key, value);
        }

        public static object Get(string key)
        {
            return CacheMan.Get(key);
        }

        public static bool Exists(string key)
        {
            if (!Utility.CacheEnabled) return false;
            return CacheMan.Get(key) != null;
        }

        public static bool Remove(string key)
        {
            if (Exists(key))
                return CacheMan.Remove(key);
            return true;
        }
    }
}