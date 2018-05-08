using System.Web.UI;
using ActiveUp.WebControls.Common.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ActiveUp.WebControls.Tests.Util
{
    [TestClass]
    public class PageExtensionTest
    {
        private const string DummyScriptKey = "DummyScriptKey";
        private const string ScriptKeyPostFix = "_startup";
        private const string DummyScriptDirectory = "DummyScriptDirectory";
        private const string DummyTestIfScriptPresentFunc = "APN_TestIfScriptPresent()";

        [TestMethod]
        public void TestAndRegisterScriptBlock_ScriptBlockRegistered()
        {
            // Arrange:
            var dummyPage = new Page();

            // Act:
            dummyPage.TestAndRegisterScriptBlock(DummyScriptKey, DummyScriptDirectory, DummyTestIfScriptPresentFunc);


            // Assert:
            Assert.IsTrue(dummyPage.IsClientScriptBlockRegistered(string.Concat(DummyScriptKey, ScriptKeyPostFix)));
        }
    }
}
