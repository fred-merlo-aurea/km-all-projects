using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.Salesforce.SF_Pages;
using ECN.Tests.Helpers;

namespace ECN.Communicator.Tests.Main.Salesforce.SF_Pages
{
    [TestFixture]
    public class SessionStoragePageTest : PageHelper
    {
        [Test]
        public void Get_ReturnValueFromSession_Success()
        {
            // Arrange
            const string propertyName = "property";
            const string valueToSet = "value";
            var page = new JustForTestPage();
            page.Session[propertyName] = valueToSet;

            // Act
            var result = page.Get<string>(propertyName, null);

            // Assert
            result.ShouldBe(valueToSet);
        }
        [Test]
        public void Get_ReturnDefaultValueFromSession_Success()
        {
            // Arrange
            const string propertyName = "property";
            const string defaultValue = "defaultValue";
            var page = new JustForTestPage();

            // Act
            var result = page.Get(propertyName, defaultValue);

            // Assert
            result.ShouldBe(defaultValue);
        }

        [Test]
        public void Set_PushValueIntoSession_Success()
        {
            // Arrange
            const string propertyName = "property";
            const string valueToSet = "value";
            var page = new JustForTestPage();

            // Act
            page.Set(propertyName, valueToSet);

            // Assert
            page.Session[propertyName].ShouldBe(valueToSet);
        }

        public class JustForTestPage : SessionStoragePage { }
    }
}
