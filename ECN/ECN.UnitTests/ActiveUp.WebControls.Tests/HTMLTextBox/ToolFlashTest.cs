using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Configuration.Fakes;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using ActiveUp.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ActiveUp.WebControls.Tests.HTMLTextBox
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ToolFlashTest
    {
        private const string _MethodInit = "_Init";
        private const string NotEmpty = "NotEmpty";
        private const string EditorIdText = "$EDITOR_ID$";
        private const string ClientSideClickText = "HTB_SetPopupPosition(\'\',\'FlashPopup\'); HTB_FL_InitFlashEditor(\'\');";
        private const string OnPreRenderMethodName = "OnPreRender";
        private const string Ie = "IE";
        private ToolFlash _toolFlash;
        private MSTest::PrivateObject _privateObject;
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _toolFlash = new ToolFlash();
            _privateObject = new MSTest::PrivateObject(_toolFlash);
        }

        [TearDown]
        public void TestCleanup()
        {
            _context.Dispose();
        }

        [Test]
        public void Init_IdNotEmpty_SetContentText()
        {
            // Arrange
            const string id = "TestID";
            const string table = "<table class=\'HTB_clsPopup\'>";

            // Act	
            _privateObject.Invoke(_MethodInit, new object[] { id });

            // Assert
            _toolFlash.PopupContents.ShouldSatisfyAllConditions(
                () => _toolFlash.PopupContents.ShouldNotBeNull(),
                () => _toolFlash.PopupContents.ContentText.ShouldNotBeNull(),
                () => _toolFlash.PopupContents.ContentText.ShouldContain(table)
                );
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnPreRender_Ns6True_SetsPopupContentValue(bool isIe)
        {
            // Arrange
            MockBrowser(isIe);
            var mockObject = new Mock<ToolFlash>();
            mockObject.Setup(x => x.Parent.Parent.Parent).Returns(new Editor());
            var popupContent = new Popup();
            mockObject.Setup(x => x.PopupContents).Returns(popupContent);
            var testObject = mockObject.Object;
            var privateObject = new MSTest.PrivateObject(testObject);
            mockObject.CallBase = true;
            testObject.ImageURL = NotEmpty;
            testObject.OverImageURL = NotEmpty;

            // Act
            privateObject.Invoke(OnPreRenderMethodName, new EventArgs());

            // Assert
            testObject.ShouldSatisfyAllConditions(
                () => testObject.PopupContents.Height.ShouldBe(isIe ? 460 : 485),
                () => testObject.PopupContents.Width.ShouldBe(430),
                () => testObject.PopupContents.AutoContent.ShouldBe(isIe),
                () => testObject.PopupContents.ContentText.Length.ShouldBe(5450),
                () => testObject.PopupContents.ContentText.ShouldNotContain(EditorIdText),
                () => testObject.ClientSideClick.ShouldBe(ClientSideClickText));
        }

        private static void MockBrowser(bool isIe)
        {
            ShimHttpCapabilitiesBase.AllInstances.BrowserGet = (x) => isIe ? Ie : string.Empty;
            ShimHttpRequest.AllInstances.BrowserGet = (x) => new HttpBrowserCapabilities();
            ShimPage.AllInstances.RequestGet = (x) => new ShimHttpRequest();
            ShimControl.AllInstances.PageGet = (x) => new ShimPage();
            ShimToolButton.AllInstances.UsePopupOnClickSetBoolean = (obj, value) => { };
        }
    }
}
