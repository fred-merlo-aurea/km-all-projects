using ecn.activityengines.Tests.Setup.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Shim = System.Web.Fakes.ShimHttpResponse;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ResponseMock:Mock<IResponse>
    {
        public ResponseMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.AllInstances.RedirectStringBoolean = Redirect;
            Shim.AllInstances.WriteString = Write;
        }

        private void Write(HttpResponse response, string content)
        {
            Object.Write(content);
        }

        private void Redirect(HttpResponse response, string url, bool endResponse)
        {
            Object.Redirect(url, endResponse);
        }
    }
}
