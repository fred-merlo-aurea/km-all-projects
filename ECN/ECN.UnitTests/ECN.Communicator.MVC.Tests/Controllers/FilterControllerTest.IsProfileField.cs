using ecn.communicator.mvc.Controllers;
using ECN.Tests.Helpers;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.MVC.Tests
{
    [TestFixture]
    public partial class FilterControllerTest
    {
        private const string _invalidShortName = "Invalid Short Name";

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void IsProfileField_CommonSwitchCaseShortNameConstants_ReturnsTrue(string switchCaseConstant)
        {
            // Arrange
            var controller = new FilterController();

            // Act
            var parameters = new object[] { switchCaseConstant };
            var isProfileField = (bool)typeof(FilterController).CallMethod("IsProfileField", parameters, controller);

            // Assert
            isProfileField.ShouldBeTrue();
        }

        [Test]
        public void IsProfileField_InvalidShortName_ReturnsFalse()
        {
            // Arrange
            var controller = new FilterController();

            // Act
            var parameters = new object[] { _invalidShortName };
            var isProfileField = (bool)typeof(FilterController).CallMethod("IsProfileField", parameters, controller);

            // Assert
            isProfileField.ShouldBeFalse();
        }

        private static string[] CommonSwitchCaseConstants => new string[]
        {
            "EmailAddress",
            "FormatTypeCode",
            "SubscribeTypeCode",
            "Title",
            "FirstName",
            "LastName",
            "FullName",
            "Company",
            "Occupation",
            "Address",
            "Address2",
            "City",
            "State",
            "Zip",
            "Country",
            "Voice",
            "Mobile",
            "Fax",
            "Website",
            "Age",
            "Income",
            "Gender",
            "UserEvent1",
            "UserEvent2",
            "Notes",
            "Birthdate",
            "UserEvent1Date",
            "UserEvent2Date",
            "CreatedOn",
            "LastChanged"
        };
    }
}
