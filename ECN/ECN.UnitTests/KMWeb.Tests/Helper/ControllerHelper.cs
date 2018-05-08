using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.Mvc.Fakes;
using System.Web.SessionState;
using System.Web.UI;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMWeb.Tests.Helper
{
    [ExcludeFromCodeCoverage]
    public abstract class ControllerHelper
    {
        protected NameValueCollection QueryString { get; set; } = new NameValueCollection();
        protected bool PageRedirect { get; set; } = false;
        protected string RedirectUrl { get; set; } = string.Empty;
        protected IDictionary<string, object> ApplicationState { get; set; } = new Dictionary<string, object>();
        protected IDictionary<string, string> ResponseHeaders { get; set; } = new Dictionary<string, string>();
        private IDisposable _shimObject;
        private HttpContext context;

        [SetUp]
        protected virtual void SetPageSessionContext()
        {
            RedirectUrl = string.Empty;
            ApplicationState.Clear();
            ResponseHeaders.Clear();

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


            ShimController.AllInstances.RequestGet = (c) =>
            {
                return new HttpRequestWrapper(new HttpRequest("test", "http://km.com", string.Empty));
            };
            ShimController.AllInstances.ResponseGet = (c) => 
            {
                return new HttpResponseWrapper(new HttpResponse(TextWriter.Null));
            };
            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return QueryString;
            };

            ShimHttpRequestBase.AllInstances.QueryStringGet = (r) =>
            {
                return QueryString;
            };
            ShimHttpResponse.AllInstances.AddHeaderStringString = (r, header, value) =>
            {
                ResponseHeaders.Add(header, value);
            };
            ShimController.AllInstances.SessionGet = (c) =>
            {
                return new HttpSessionStateWrapper(sessionState);
            };
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

        protected T Get<T>(PrivateObject privateObject, string propertyName) where T : class
        {
            var fieldOrProperty = privateObject.GetFieldOrProperty(propertyName) as T;

            fieldOrProperty.ShouldNotBeNull();

            return fieldOrProperty;
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
