using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Fakes;
using ecn.activityengines.Tests.Setup.Interfaces;
using Moq;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class RequestMock : Mock<IRequest>
    {
        public NameValueCollection ServerVariables { get; }

        public RequestMock()
        {
            ServerVariables = new NameValueCollection();
            SetupShims();
        }

        private void SetupShims()
        {
            ShimHttpRequest.AllInstances.UrlGet = UrlGet;
            ShimHttpRequest.AllInstances.ServerVariablesGet = ServerVariablesGet;
        }

        private NameValueCollection ServerVariablesGet(HttpRequest request)
        {
            return ServerVariables;
        }

        private Uri UrlGet(HttpRequest request)
        {
            return Object.Url ?? GetDefaultUrl();
        }

        private Uri GetDefaultUrl()
        {
            return new Uri("http://anyDomain.ext");
        }
    }
}
