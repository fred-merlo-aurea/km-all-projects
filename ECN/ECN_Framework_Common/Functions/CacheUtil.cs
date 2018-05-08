using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

namespace ECN_Framework_Common.Functions
{
    public class CacheUtil
    {

        private static DataCacheFactory _factory = null;
        private static DataCache _cache = null;

        private static DataCache GetCache()
        {
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
            _cache = _factory.GetCache("default");



            return _cache;
        }


        public static void AddToCache(string key, object value)
        {
            try
            {
                DataCache cache = GetCache();
                cache.Put(key, value);
            }
            catch 
            {
            }
        }

        public static void RemoveFromCache(string key)
        {
            try
            {
                DataCache cache = GetCache();
                cache.Remove(key);
            }
            catch 
            {
            }
        }

    }
}
