using System.Web.UI.Design.Fakes;
using ActiveUp.WebControls.ActiveToolbar;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveUp.WebControls.Tests.Common
{
    [TestClass]
    public class CoreControlDesignerTest
    {
        private const string DummyDesignTimeHtmlText = "Dummy Design Time Html Text";

        [TestMethod]
        public void GetEmptyDesignTimeHtml_ReturnString()
        {
            // Arrange:
            var coreWebControl = new CoreControlDesigner();
            var privateObject = new PrivateObject(coreWebControl);

            using (ShimsContext.Create())
            {
                ShimControlDesigner.AllInstances.CreatePlaceHolderDesignTimeHtmlString =
                    (designer, s) => DummyDesignTimeHtmlText;

                // Act:
                var result = privateObject.Invoke("GetEmptyDesignTimeHtml");


                // Assert:
                Assert.AreEqual(DummyDesignTimeHtmlText, result);
            }
        }
    }
}
