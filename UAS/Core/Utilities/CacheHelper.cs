using System;

namespace Core_AMS.Utilities
{
    public class CacheHelper
    {
        //public static Object GetCurrentCache(string key)
        //{
        //    if (!IsCacheEnabled())
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return HttpContext.Current.Cache[key];
        //    }
        //}

        //public static void AddToCache(string key, object value)
        //{
        //    if (IsCacheEnabled())
        //    {
        //        HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.High, null);
        //    }
        //}

        //public static void ClearCache(string key)
        //{
        //    if (IsCacheEnabled() && HttpContext.Current.Cache[key] != null)
        //    {
        //        HttpContext.Current.Cache.Remove(key);
        //    }
        //}

        //public static void ClearCacheAll()
        //{
        //    if (IsCacheEnabled())
        //    {
        //        System.Web.Caching.Cache currentCache = HttpContext.Current.Cache;

        //        foreach (DictionaryEntry entry in currentCache)
        //        {
        //            currentCache.Remove(entry.Key.ToString());
        //        }
        //    }
        //}

        //public static bool IsCacheEnabled()
        //{
        //    return HttpContext.Current != null;
        //}
    }
}
