using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shim = System.Web.Fakes.ShimHttpServerUtility;
using UserControlShim = System.Web.UI.Fakes.ShimUserControl;
using ECN.Editor.Tests.Setup.Interfaces;
using Moq;
using System.Web;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace ECN.Editor.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ServerMock:Mock<IServer>
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
            Shim.AllInstances.MapPathString = MapPtah;
            Shim.AllInstances.UrlPathEncodeString = UrlPathEncode;
        }

        private string UrlPathEncode(HttpServerUtility instance, string url)
        {
            return url;
        }

        private string MapPtah(HttpServerUtility instance, string path)
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
