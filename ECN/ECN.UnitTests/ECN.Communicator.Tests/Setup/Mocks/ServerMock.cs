using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using ECN.Communicator.Tests.Setup.Interfaces;
using Moq;
using Shim = System.Web.Fakes.ShimHttpServerUtility;
using UserControlShim = System.Web.UI.Fakes.ShimUserControl;

namespace ECN.Communicator.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ServerMock : Mock<IServer>
    {
        private HttpServerUtility _server;

        public ServerMock()
        {
            _server = CreateHttpServerUtility();
            SetupShims();
        }

        private void SetupShims()
        {
            UserControlShim.AllInstances.ServerGet = (instance) => _server;
            Shim.AllInstances.MapPathString = MapPath;
            Shim.AllInstances.UrlPathEncodeString = UrlPathEncode;
        }

        private string UrlPathEncode(HttpServerUtility instance, string url)
        {
            return url;
        }

        private string MapPath(HttpServerUtility instance, string path)
        {
            return Object.MapPath(path);
        }

        private HttpServerUtility CreateHttpServerUtility()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructor = typeof(HttpServerUtility)
                .GetConstructor(flags, null, new Type[] { typeof(HttpContext) }, null);
            return constructor?.Invoke(new object[] { null }) as HttpServerUtility;
        }
    }
}
