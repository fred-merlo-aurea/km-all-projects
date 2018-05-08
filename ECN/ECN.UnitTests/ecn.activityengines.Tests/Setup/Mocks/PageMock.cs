using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Caching.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using ecn.activityengines.Tests.Setup.Interfaces;
using Moq;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class PageMock : Mock<IPage>, IDisposable
    {
        private readonly Dictionary<string, object> _cache;
        private readonly Cache _webCach;
        private readonly MemoryStream _responseStream;
        private readonly StreamWriter _responseStreamWriter;
        private readonly HttpResponse _response;

        public PageMock()
        {
            _cache = new Dictionary<string, object>();
            _webCach = new Cache();
            _responseStream = new MemoryStream();
            _responseStreamWriter = new StreamWriter(_responseStream);
            _response = new HttpResponse(_responseStreamWriter);
            SetupShims();
        }

        public void Dispose()
        {
            _responseStreamWriter.Dispose();
            _responseStream.Dispose();
        }

        private void SetupShims()
        {
            ShimPage.AllInstances.RequestGet = RequestGet;
            ShimPage.AllInstances.CacheGet = CacheGet;
            ShimPage.AllInstances.ServerGet = ServerGet;
            ShimPage.AllInstances.ResponseGet = ResponseGet;
            ShimCache.AllInstances
                .AddStringObjectCacheDependencyDateTimeTimeSpanCacheItemPriorityCacheItemRemovedCallback =
                CacheItemAdd;
            ShimCache.AllInstances.ItemGetString = CacheItemGet;
        }

        private HttpResponse ResponseGet(Page page)
        {
            return _response;
        }

        private object CacheItemGet(Cache cache, string key)
        {
            object result = null;
            return _cache.TryGetValue(key, out result)
                ? result
                : Object.CacheItemGet(key);
        }

        private object CacheItemAdd(
            Cache cache,
            string key,
            object value,
            CacheDependency dependency,
            DateTime absoluteExpiration,
            TimeSpan slidingExpiration,
            CacheItemPriority priority,
            CacheItemRemovedCallback itemRemovedCallback)
        {
            _cache[key] = value;
            return Object.CacheItemAdd(key, value, dependency, absoluteExpiration, slidingExpiration, priority,
                itemRemovedCallback);
        }

        private HttpServerUtility ServerGet(Page page)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructor = typeof(HttpServerUtility)
                .GetConstructor(flags, null, new Type[] { typeof(HttpContext)}, null);
            return constructor?.Invoke(new object[] { null}) as HttpServerUtility;
        }

        private Cache CacheGet(Page page)
        {
            return _webCach;
        }

        private HttpRequest RequestGet(Page page)
        {
            const string url = "http://domain.some";
            return new HttpRequest(string.Empty, url, string.Empty);
        }
    }
}
