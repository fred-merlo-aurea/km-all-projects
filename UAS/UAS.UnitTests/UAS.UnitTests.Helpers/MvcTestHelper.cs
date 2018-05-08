using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace UAS.UnitTests.Helpers
{
    public static class MvcMockHelpers
    {
        private static readonly char[] InterrogationCharArray = "?".ToCharArray();
        private static readonly char[] AmpersandCharArray = "&".ToCharArray();
        private static readonly char[] EqualsCharArray = "=".ToCharArray();

        public static HttpContextBase MockHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            request.Setup(r => r.AppRelativeCurrentExecutionFilePath).Returns("/");
            request.Setup(r => r.ApplicationPath).Returns("/");

            response.Setup(s => s.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            response.SetupProperty(res => res.StatusCode, (int)System.Net.HttpStatusCode.OK);

            var coockieCollection = new HttpCookieCollection();
            response.SetupGet(r => r.Cookies).Returns(coockieCollection);

            context.Setup(h => h.Request).Returns(request.Object);
            context.Setup(h => h.Response).Returns(response.Object);

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }

        public static HttpContextBase MockHttpContext(string url)
        {
            var context = MockHttpContext();
            context.Request.SetupRequestUrl(url);
            return context;
        }

        public static void SetMockControllerContext(this Controller controller,
            HttpContextBase httpContext = null,
            RouteData routeData = null,
            RouteCollection routes = null)
        {
            //If values not passed then initialise
            routeData = routeData ?? new RouteData();
            routes = routes ?? RouteTable.Routes;
            httpContext = httpContext ?? MockHttpContext();

            var requestContext = new RequestContext(httpContext, routeData);
            var context = new ControllerContext(requestContext, controller);

            controller.Url = new UrlHelper(requestContext, routes);
            controller.ControllerContext = context;
        }

        public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
        {
            Mock.Get(request).Setup(req => req.HttpMethod).Returns(httpMethod);
        }

        public static void SetupRequestUrl(this HttpRequestBase request, string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!url.StartsWith("~/"))
            {
                throw new ArgumentException("Virtual url starting with \"~/\". expected");
            }

            var mock = Mock.Get(request);

            mock.Setup(req => req.QueryString).Returns(GetQueryStringParameters(url));
            mock.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns(GetUrlFileName(url));
            mock.Setup(req => req.PathInfo).Returns(string.Empty);
        }

        public static T ThrowIfNull<T>(this T variableValue, string variableName) where T : class
        {
            if (variableValue == null)
            {
                throw new InvalidOperationException($"Value is Null: {variableName}");
            }

            return variableValue;
        }

        private static string GetUrlFileName(string url)
        {
            return (url.Contains("?"))
                ? url.Substring(0, url.IndexOf("?", StringComparison.Ordinal))
                : url;
        }

        private static NameValueCollection GetQueryStringParameters(string url)
        {
            if (url.Contains("?"))
            {
                var parameters = new NameValueCollection();

                var parts = url.Split(InterrogationCharArray);
                var keys = parts[1].Split(AmpersandCharArray);

                foreach (var key in keys)
                {
                    var part = key.Split(EqualsCharArray);
                    parameters.Add(part[0], part[1]);
                }

                return parameters;
            }

            return null;
        }
    }
}