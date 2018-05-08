using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;

namespace ECN_Framework_Common.Functions
{
    public class CacheHelper
    {
        //public static KM.Common.Encryption GetEncryption()
        //{
        //    KM.Common.Encryption encrypt = new KM.Common.Encryption();

        //    encrypt.HashAlgorithm = Properties.Settings.Default.HashAlgorithm;
        //    encrypt.InitVector = Properties.Settings.Default.InitVector;
        //    encrypt.KeySize = Properties.Settings.Default.KeySize;
        //    encrypt.PassPhrase = Properties.Settings.Default.PassPhrase;
        //    encrypt.PasswordIterations = Properties.Settings.Default.PasswordIterations;
        //    encrypt.SaltValue = Properties.Settings.Default.SaltValue;

        //    return encrypt;
        //}

        public static Object GetCurrentCache(string key)
        {
            if (!IsCacheEnabled())
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Cache[key];
            }
        }

        public static void AddToCache(string key, object value)
        {
            if (IsCacheEnabled())
            {
                HttpContext.Current.Cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.High, null);
            }
        }

        public static void ClearCache(string key)
        {
            if (IsCacheEnabled() && HttpContext.Current.Cache[key] != null)
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }

        public static void ClearCacheAll()
        {
            if (IsCacheEnabled())
            {
                System.Web.Caching.Cache currentCache = HttpContext.Current.Cache;

                foreach (DictionaryEntry entry in currentCache)
                {
                    currentCache.Remove(entry.Key.ToString());
                }
            }
        }

        public static bool IsCacheEnabled()
        {
            return HttpContext.Current != null;
        }   
    }
}

