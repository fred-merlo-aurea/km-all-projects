using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public abstract class PageHelper
    {
        protected NameValueCollection QueryString { get; set; } = new NameValueCollection();
        protected bool PageRedirect { get; set; } = false;
        protected string RedirectUrl { get; set; } = string.Empty;
        protected IDictionary<string, object> ApplicationState { get; set; } = new Dictionary<string, object>();
        private IDisposable _shimObject;
        private HttpContext context;
        private static CultureInfo _previousCulture;

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }
        [SetUp]
        protected virtual void SetPageSessionContext()
        {
            RedirectUrl = string.Empty;
            ApplicationState.Clear();

            _shimObject = ShimsContext.Create();
            context = new HttpContext(new HttpRequest(string.Empty, "http://km.com", string.Empty),
                new HttpResponse(TextWriter.Null));

            ShimHttpContext.CurrentGet = () => { return context; };

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                     new HttpStaticObjectsCollection(), 10, true,
                                                     HttpCookieMode.AutoDetect,
                                                     SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;

            ShimPage.AllInstances.SessionGet = (p) =>
            {
                return sessionState;
            };

            ShimHttpContext.AllInstances.SessionGet = (p) =>
            {
                return sessionState;
            };

            ShimPage.AllInstances.ResponseGet = (p) =>
            {
                return new System.Web.HttpResponse(TextWriter.Null);
            };

            ShimHttpResponse.AllInstances.RedirectStringBoolean = (r, url, s) =>
            {
                RedirectUrl = url;
                PageRedirect = true;
            };
            ShimHttpResponse.AllInstances.RedirectString = (r, url) =>
            {
                RedirectUrl = url;
                PageRedirect = true;
            };
            ShimPage.AllInstances.RequestGet = (p) =>
            {
                return new HttpRequest("test", "http://km.com", string.Empty);
            };

            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return QueryString;
            };
            ShimHttpRequest.AllInstances.RawUrlGet = (r) => string.Empty;
            ShimHttpRequest.AllInstances.ServerVariablesGet = (r) => QueryString;
            ShimHttpApplication.AllInstances.ResponseGet = (h) => new System.Web.HttpResponse(TextWriter.Null);
            ShimHttpApplication.AllInstances.RequestGet = (h) => new HttpRequest("test", "http://km.com", "");
            ShimHttpApplication.AllInstances.ApplicationGet = (h) => new ShimHttpApplicationState();
            ShimHttpApplicationState.AllInstances.ItemSetStringObject = (a, o, h) => { ApplicationState.Add(o, h); };
        }

        protected void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        protected T Get<T>(PrivateObject privateObject, string propertyName)
        {
            return (T)privateObject.GetFieldOrProperty(propertyName);
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public |
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Static |
                                                                  BindingFlags.Instance);

                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }

        [TearDown]
        public void CleanUp()
        {
            _shimObject.Dispose();
            context = null;
        }
    }
}
