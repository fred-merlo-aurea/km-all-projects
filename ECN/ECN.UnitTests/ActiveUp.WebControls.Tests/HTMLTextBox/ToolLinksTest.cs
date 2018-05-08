using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Configuration.Fakes;
using System.Web.Fakes;
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
    public partial class ToolLinksTest
    {
        private const string Ie = "IE";
        private const string NotEmpty = "NotEmpty";
        private const string OnPreRenderMethodName = "OnPreRender";
        private const string ClientSideClickText = "HTB_SetPopupPosition('','Popup');HTB_InitLink('');";
        private IDisposable _context;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanup()
        {
            _context.Dispose();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnPreRender_Ns6True_SetsPopupContentValue(bool isIe)
        {
            // Arrange
            MockBrowser(isIe);
            var mockObject = new Mock<ToolLink>();
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
                () => testObject.PopupContents.Height.ShouldBe(isIe ? 213 : 200),
                () => testObject.PopupContents.Width.ShouldBe(isIe ? 345 : 330),
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
