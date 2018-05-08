using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace KM.Common
{
    public class CacheUtil
    {

        private static DataCacheFactory _factory = null;
        private static DataCache _cache = null;

        public static void SafeRemoveFromCache(string cacheKey, string regionKey)
        {
            if (GetFromCache(cacheKey, regionKey) != null)
            {
                RemoveFromCache(cacheKey, regionKey);
            }
        }

        public static T GetFromCache<T>(string key, string region, Func<T> fetchDataAction) where T : class
        {
            var cachedData = GetFromCache(key, region) as T;

            if (cachedData != null)
            {
                return cachedData;
            }

            cachedData = fetchDataAction();
            AddToCache(key, cachedData, region);

            return cachedData;
        }

        private static bool? _logUnsuccessful
        {
            get
            {
                try
                {
                    return bool.Parse(ConfigurationManager.AppSettings["LogUnsuccessfulCacheRequest"].ToString());
                }
                catch
                {
                    return null;
                }
            }
        }

        private static DataCache GetCache(bool checkIsEnabled = true)
        {
            if (checkIsEnabled && !IsCacheEnabled())
                return null;

            if (_cache != null)
                return _cache;

            //-------------------------
            // Configure Cache Client 
            //-------------------------

            //Define Array for 1 Cache Host
            List<DataCacheServerEndpoint> servers = new List<DataCacheServerEndpoint>(1);

            //Specify Cache Host Details 
            //  Parameter 1 = host name
            //  Parameter 2 = cache port number
            servers.Add(new DataCacheServerEndpoint(ConfigurationManager.AppSettings["AppFabricServer"].ToString(), 22233));

            //Create cache configuration
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();
            //Set the cache host(s)
            configuration.Servers = servers;

            //Set default properties for local cache (local cache disabled)
            configuration.LocalCacheProperties = new DataCacheLocalCacheProperties();

            configuration.SecurityProperties = new DataCacheSecurity(DataCacheSecurityMode.None, DataCacheProtectionLevel.None);

            //Disable tracing to avoid informational/verbose messages on the web page
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);

            //Pass configuration settings to cacheFactory constructor
            _factory = new DataCacheFactory(configuration);
            //Get reference to named cache called "default"
            _cache = _factory.GetCache(ConfigurationManager.AppSettings["AppFabricServer-NamedCache"].ToString());

            return _cache;
        }

        public static bool IsCacheEnabled()
        {
            bool AppFabricCacheEnabled = false;

            try
            {
                Boolean.TryParse(ConfigurationManager.AppSettings["AppFabricCacheEnabled"].ToString(), out AppFabricCacheEnabled);
            }
            catch
            {
            }

            return AppFabricCacheEnabled;
        }

        public static void AddToCache(string key, object value, bool checkIsEnabled = true)
        {
            AddToCache(key, value, string.Empty, checkIsEnabled);
        }

        public static void AddToCache(string key, object value, string region, bool checkIsEnabled = true)
        {
            try
            {
                DataCache cache = GetCache(checkIsEnabled);

                if (cache == null)
                    return;

                if (region != string.Empty)
                {
                    CreateRegion(region.ToUpper());
                    cache.Put(key, value, region.ToUpper());
                }
                else
                {
                    cache.Put(key, value);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "KM.Common.CacheUtil.AddToCache", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "<BR><BR>Key: " + key + "<BR>Region: " + region);
            }
        }

        public static void Flush()
        {
            DataCache cache = GetCache();

            if (cache == null)
                return;

            foreach (string regionName in cache.GetSystemRegions())
            {
                cache.ClearRegion(regionName);
            }
        }

        public static void FlushRegion(string region)
        {
            DataCache cache = GetCache();

            if (cache == null)
                return;

            try
            {
                cache.ClearRegion(region);
            }
            catch
            { }
        }

        public static void RemoveFromCache(string key)
        {
            RemoveFromCache(key, string.Empty);
        }

        public static void RemoveFromCache(string key, string region)
        {
            bool success = false;
            try
            {
                //passing false here cause we don't care if cache is enabled
                DataCache cache = GetCache(false);

                if (cache == null)
                    return;


                //DataCacheNotificationDescriptor itemRemoved = null;
                if (region != string.Empty)
                {
                    //itemRemoved = cache.AddItemLevelCallback(key, DataCacheOperations.RemoveItem, clientCallback, region);
                    success = cache.Remove(key, region.ToUpper());
                }
                else
                {
                    // itemRemoved = cache.AddItemLevelCallback(key, DataCacheOperations.RemoveItem, clientCallback);
                    success = cache.Remove(key);

                }
                //cache.RemoveCallback(itemRemoved);
                if (!success && _logUnsuccessful.HasValue && _logUnsuccessful.Value)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(new Exception(), "KM.Common.CacheUtil.RemoveFromCache", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "<BR><BR>Key: " + key + "<BR>Region: " + region + "<BR>Notes: Unsuccessful removal from cache");
                }
                else if (!success && _logUnsuccessful.HasValue && !_logUnsuccessful.Value)
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(new Exception(), "KM.Common.CacheUtil.RemoveFromCache", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "<BR><BR>Key: " + key + "<BR>Region: " + region + "<BR>Notes: Unsuccessful removal from cache");
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "KM.Common.CacheUtil.RemoveFromCache", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "<BR><BR>Key: " + key + "<BR>Region: " + region);
            }

        }

        static void clientCallback(string cacheName, string region, string key, DataCacheItemVersion version, DataCacheOperations operationID, DataCacheNotificationDescriptor notificationDescriptor)
        {

        }



        public static object GetFromCache(string key, bool checkIsEnabled = true)
        {
            return GetFromCache(key, string.Empty, checkIsEnabled);
        }

        public static object GetFromCache(string key, string region, bool checkIsEnabled = true)
        {

            try
            {
                DataCache cache = GetCache(checkIsEnabled);

                if (cache == null)
                    return null;

                if (region != string.Empty)
                {
                    return cache.Get(key, region.ToUpper());
                }
                else
                {
                    return cache.Get(key);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "KM.Common.CacheUtil.GetFromCache", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "<BR><BR>Key: " + key + "<BR>Region: " + region);
                return null;
            }
        }

        private static void CreateRegion(string region)
        {
            DataCache cache = GetCache();

            try
            {
                cache.CreateRegion(region.ToUpper());
            }
            catch
            {

            }
        }


    }
}
