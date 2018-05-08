using System;
using System.Configuration;
using System.Web;
using KMPlatform.Entity;

namespace KMWeb.Services
{
    public class UserCacheService : IUserCacheService
    {
        private static readonly string MasterAccessKey = ConfigurationManager.AppSettings["MasterAccessKey"];
        private readonly string _cacheKey = $"cache_user_by_AccessKey_{MasterAccessKey}";

        public User GetUser()
        {
            User user;
            
            if (HttpRuntime.Cache[_cacheKey] == null)
            {
                user = KMPlatform.BusinessLogic.User.GetByAccessKey(MasterAccessKey, true);
                HttpRuntime.Cache.Add(_cacheKey, user, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(15), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            else
            {
                user = (User)HttpRuntime.Cache[_cacheKey];
            }

            return user;
        }
    }
}