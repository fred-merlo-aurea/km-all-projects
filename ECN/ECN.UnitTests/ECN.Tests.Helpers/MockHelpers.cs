using System.IO;
using System.Web;
using System.Web.SessionState;

namespace ECN.Tests.Helpers
{
    public static class MockHelpers
    {
        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://localhost/", "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);
            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            return httpContext;
        }
    }
}
