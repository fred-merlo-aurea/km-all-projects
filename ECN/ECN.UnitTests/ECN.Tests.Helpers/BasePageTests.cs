using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using ECN_Framework_BusinessLayer.Application.Interfaces;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ECN.Tests.Helpers
{
    [TestClass]
    public class BasePageTests
    {
        protected const string DefaultCulture = "en-US";
        protected const int CustomerId = 123;
        protected const string RequestFileName = "SampleFileName";
        protected const string RequestUrl = "http://sampledomain.com/";
        protected const string RequestQueryString = "SampleQueryString";

        protected PrivateObject _privateObject;
        protected Page _portalPage;
        protected object _responseContent;
        protected NameValueCollection _responseHeaders = new NameValueCollection();
        protected Mock<ISessionDataProvider> _sessionDataProviderMock;
        protected User CurrentUserMock;

        public BasePageTests()
        {
            CurrentUserMock = new User
            {
                IsActive = true,
                IsPlatformAdministrator = true,
                CurrentClient = new Client(),
                CustomerID = CustomerId
            };
            
            _sessionDataProviderMock = new Mock<ISessionDataProvider>();
            _sessionDataProviderMock.Setup(proxy => proxy.GetCurrentUser()).Returns(CurrentUserMock);
        }

        protected virtual void InitializePage(Page page)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            _responseHeaders.Clear();
            _portalPage = page;
            _privateObject = new PrivateObject(_portalPage);
            
            InitializeAllControls(_portalPage);

            var response = new System.Web.HttpResponse(TextWriter.Null);
            ReflectionHelper.SetValue(page, "_response", response);
            var request = new HttpRequest(RequestFileName, RequestUrl, RequestQueryString);
            ReflectionHelper.SetValue(page, typeof(Page), "_request", request);
            if (page is ECN_Framework.WebPageHelper)
            {
                ReflectionHelper.SetValue(
                    page,
                    typeof(ECN_Framework.WebPageHelper),
                    "_sessionDataProvider",
                    _sessionDataProviderMock.Object);
            }

            ShimHttpResponse.AllInstances.BinaryWriteByteArray = (_, content) =>
            {
                _responseContent = content;
            };

            ShimHttpResponse.AllInstances.AddHeaderStringString = (_, headerKey, headerValue) =>
            {
                _responseHeaders.Add(headerKey, headerValue);
            };

            ShimHttpResponse.AllInstances.End = _ => { };
        }

        protected virtual void GrantUserAccess(string serviceCode, string serviceFeatureCode)
        {
            var service = new Service
            {
                ServiceCode = serviceCode
            };
            var serviceFeature = new ServiceFeature
            {
                SFCode = serviceFeatureCode
            };
            service.ServiceFeatures.Add(serviceFeature);
            CurrentUserMock.CurrentClient.Services.Add(service);
        }

        protected virtual void RevokeUserAccess(string serviceCode)
        {
            CurrentUserMock.CurrentClient.Services.RemoveAll(service => service.ServiceCode == serviceCode);
        }

        private void InitializeAllControls(object page)
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
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        protected T Get<T>(string control)
        {
            return (T)_privateObject.GetField(control);
        }
    }
}
