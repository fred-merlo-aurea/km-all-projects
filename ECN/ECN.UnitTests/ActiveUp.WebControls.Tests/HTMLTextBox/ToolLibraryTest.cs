using System;
using System.Collections.Specialized;
using System.Collections.Specialized.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Fakes;
using System.Web.Configuration.Fakes;
using System.Web.UI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using ActiveUp.WebControls.Fakes;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolLibraryTest
    {
        private const string DummyString = "DummyString";
        private const string MethodPage_Load = "Page_Load";
        private IDisposable _shimObject;
        private ToolLibrary _toolLibrary;
        private MSTest::PrivateObject _privateObject;
        private EventArgs eventArgs;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            eventArgs = new EventArgs();
            _toolLibrary = new ToolLibrary();
            _privateObject = new MSTest::PrivateObject(_toolLibrary);
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase("IE")]
        [TestCase("")]
        public void Page_Load_OnValidCall_RenderContentText(string browser)
        {
            // Arrange
            _toolLibrary.Page = GetMockOfPage(browser).Instance;
            _toolLibrary.Directories = new FileDirectoryCollection
            {
                new FileDirectory(string.Empty, string.Empty, string.Empty)
            };
            const string Table = "<table class=HTB_clsPopup cellpadding=0 cellspacing=0>";
            ShimToolLibrary.AllInstances.UploadImageString = (obj, str) =>
                new ImageUploadedEventArgs(string.Empty, string.Empty, 0, true);

            // Act	
            _privateObject.Invoke(MethodPage_Load, new object[] { null, eventArgs });

            // Assert
            _toolLibrary.PopupContents.ShouldSatisfyAllConditions(
                () => _toolLibrary.PopupContents.ShouldNotBeNull(),
                () => _toolLibrary.PopupContents.ContentText.ShouldNotBeNullOrWhiteSpace(),
                () => _toolLibrary.PopupContents.ContentText.ShouldContain(Table)
                );
        }

        private ShimPage GetMockOfPage(string browser)
        {
            var shimPage = new ShimPage();
            var shimHttpRequest = new ShimHttpRequest();
            var shimHttpBrowserCapabilities = new ShimHttpBrowserCapabilities();
            ShimHttpCapabilitiesBase.AllInstances.BrowserGet = (obj) => browser;
            ShimHttpRequest.AllInstances.BrowserGet = (obj) => shimHttpBrowserCapabilities.Instance;
            ShimPage.AllInstances.RequestGet = (obj) => shimHttpRequest.Instance;
            ShimPage.AllInstances.ClientScriptGet = (obj) => new ShimClientScriptManager().Instance;
            ShimClientScriptManager.AllInstances.GetWebResourceUrlTypeString = (obj, type, url) => DummyString;
            ShimHttpRequest.AllInstances.ParamsGet = (obj) =>
            {
                var collection = new NameValueCollection();
                collection.Add("__EVENTARGUMENT", "DELETEFILE:");
                return collection;
            };
            ShimNameObjectCollectionBase.AllInstances.CountGet = (obj) => 1;
            ShimHttpRequest.AllInstances.FilesGet = (obj) => new ShimHttpFileCollection();
            ShimPage.AllInstances.TraceGet = (obj) => new TraceContext(new ShimHttpContext());
            ShimTraceContext.AllInstances.WriteString = (obj, str) => { };
            return shimPage;
        }

		[Test]
		public void DeleteDisabled_DefaultValue_ReturnsFalse()
		{
			// Arrange
			using (var testObject = new ToolLibrary())
			{
				// Act, Assert
				testObject.DeleteButtonDisabled.ShouldBeFalse();
			}
		}

		[Test]
		public void DeleteDisabled_SetValue_ReturnsSetValue()
		{
			// Arrange
			using (var testObject = new ToolLibrary())
			{
				// Act
				testObject.DeleteButtonDisabled = true;

				// Assert
				testObject.DeleteButtonDisabled.ShouldBeTrue();
			}
		}
	}
}
