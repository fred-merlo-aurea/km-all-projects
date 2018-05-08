using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests
{
    [ExcludeFromCodeCoverage]
    public class BaseControlTests
    {
        private const string ContentTypeImage = "image/png";
        private const string FieldPage = "_page";
        private const string FieldRequest = "_request";

        protected UserControl PortalControl { get; set; }
        protected PrivateObject PrivateControl { get;set; }
        protected PrivateType PrivateType { get; set; }
        protected Page ParentPage { get;set; }
        protected FakeHttpContext.FakeHttpContext HttpContextFake { get; set; }
        protected IDisposable ShimContext { get; private set; }
        protected NameValueCollection QueryString { get; } = new NameValueCollection();
        protected string RedirectedUrl { get; private set; } = string.Empty;
        protected bool IsPageRedirected { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            ShimContext = ShimsContext.Create();
            HttpContextFake = CreateFakeHttpContext();
            ShimRegisteredScripts();
            ShimForContext();
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpContextFake?.Dispose();
            ParentPage?.Dispose();
            ShimContext?.Dispose();
        }

        protected virtual void InitializeUserControl(UserControl control)
        {
            PortalControl = control ?? throw new ArgumentNullException(nameof(control));
            PrivateControl = new PrivateObject(PortalControl);
            PrivateType = new PrivateType(control.GetType());
            InitializeParentPage();
        }

        protected virtual FakeHttpContext.FakeHttpContext CreateFakeHttpContext()
        {
            var context = new FakeHttpContext.FakeHttpContext();
            ShimHttpRequest.AllInstances.QueryStringGet = _ => QueryString;
            return context;
        }

        protected virtual void InitializeParentPage()
        {
            ParentPage = new Page();
            var privateObject = new PrivateObject(ParentPage);
            privateObject.SetField(FieldRequest, HttpContext.Current.Request);
            PrivateControl.SetField(FieldPage, ParentPage);
        }

        protected virtual void InitializeAllControls(object control)
        {
            var fields = control.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(control) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(control, obj);
                        }
                    }
                }
            }
        }

        protected virtual HttpPostedFile ShimPostedFileForHtmlInputFile(string fileName, string contentType = ContentTypeImage)
        {
            var file = (HttpPostedFile)typeof(HttpPostedFile)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First()
                .Invoke(new object[] { fileName, contentType, null });
            ShimHtmlInputFile.AllInstances.PostedFileGet = _ => file;

            return file;
        }

        protected void ShimForFileSystem(bool directoryExists, bool fileExists)
        {
            ShimHttpServerUtility.AllInstances.MapPathString = (utility, path) => path;
            ShimDirectory.ExistsString = _ => directoryExists;
            ShimFile.ExistsString = _ => fileExists;
        }

        protected void ShimRegisteredScripts()
        {
            ShimScriptManager.GetCurrentPage = _ => new ShimScriptManager();
            ShimScriptManager.AllInstances.RegisterPostBackControlControl = (_, __) => { };
        }

        protected virtual T GetField<T>(string fieldName) where T : class
        {
            var field = PrivateControl.GetField(fieldName) as T;

            field.ShouldNotBeNull($"The field {field} of type {typeof(T).Name} cannot be null");

            return field;
        }

        protected void ShimForContext()
        {
            ShimPage.AllInstances.RequestGet = _ =>  new ShimHttpRequest();
            ShimHttpRequest.AllInstances.QueryStringGet = _ => QueryString;

            ShimPage.AllInstances.ResponseGet = _ => new ShimHttpResponse();
            ShimHttpResponse.AllInstances.RedirectString = (r, url) => 
            {
                IsPageRedirected = true;
                RedirectedUrl = url;
            };
        }
    }
}
