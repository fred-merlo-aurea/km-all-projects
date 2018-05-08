using System;
using System.Web.Caching;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IPage
    {
        object CacheItemAdd(
            string key,
            object value,
            CacheDependency dependency,
            DateTime absoluteExpiration,
            TimeSpan slidingExpiration,
            CacheItemPriority priority,
            CacheItemRemovedCallback itemRemovedCallback);

        object CacheItemGet(string key);
    }
}
