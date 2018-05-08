using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Configuration;

namespace KM.Common
{
    public class CacheHelper
    {
        //wgh - unused at this time
        public enum CacheItems 
        {
            Encryption_Cache, 
            Database_Cache 
        }

        private static ObjectCache cache = MemoryCache.Default;
        public static System.Object GetCurrentCache(string key)
        {
            if (!IsCacheEnabled())
            {
                return null;
            }
            else
            {
                return cache[key];
            }
        }

        public static System.Object AddToCache(string key, object value)
        {
            if (IsCacheEnabled())
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["CacheLimit"]));
                policy.Priority = CacheItemPriority.Default;

                CacheItem ci = new CacheItem(key, value);
                cache.Add(ci, policy);
                return GetCurrentCache(key);
            }
            else
            {
                return null;
            }
        }

        public static void ClearCache(string key)
        {
            if (IsCacheEnabled() && cache[key] != null)
            {
                cache.Remove(key);
            }
        }

        public static void ClearCacheAll()
        {
            if (IsCacheEnabled())
            {
                MemoryCache.Default.Dispose();
            }
        }

        public static bool IsCacheEnabled()
        {
            return cache != null;
        }
    }
}

