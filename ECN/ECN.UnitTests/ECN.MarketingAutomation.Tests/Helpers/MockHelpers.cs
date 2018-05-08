using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ECN.MarketingAutomation.Tests.Helpers
{
	[ExcludeFromCodeCoverage]
	public static class MockHelpers
	{
		private const string requestUrl = "http://localhost/";
		public static HttpContext FakeHttpContext()
		{
			var httpRequest = new HttpRequest(String.Empty, requestUrl, String.Empty);
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
