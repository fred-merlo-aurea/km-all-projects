using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Group;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class FiltersTest : PageHelper
    {
        private const int StringSelectedIndex = 0;
        private const int DateSelectedIndex = 1;
        private const string TestUserName = "TestUser";
        private const string TestUrl = "http://km.com";
        private filters _filter;
        private PrivateObject _privateFilterObj;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _filter = new filters();
            InitializeAllControls(_filter);
            _privateFilterObj = new PrivateObject(_filter);
            InitializeSessionFakes();
            QueryString.Clear();
            ShimUserControl.AllInstances.RequestGet = (p) =>
            {
                return new HttpRequest(string.Empty, TestUrl, string.Empty);
            };
            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return QueryString;
            };
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void SetupFieldType_CommonSwitchCaseConstantsConditionField_SetsFieldTypeDropdownListEnableToFalseAndSelectString(string switchCaseConstant)
        {
            // Arrange
            var filter = new filters();
            var fieldTypeDropDownList = CreateDropdownList(true);
            ReflectionHelper.SetValue(filter, "ddlFieldType", fieldTypeDropDownList);

            // Act
            var parameters = new object[] { switchCaseConstant };
            typeof(filters).CallMethod("SetupFieldType", parameters, filter);

            // Assert
            var fieldTypeDropdownList = ReflectionHelper.GetFieldInfoFromInstanceByName(filter, "ddlFieldType").GetValue(filter) as DropDownList;
            fieldTypeDropdownList.ShouldSatisfyAllConditions(
                () => fieldTypeDropdownList.ShouldNotBeNull(),
                () => fieldTypeDropdownList.Enabled.ShouldBeFalse(),
                () => fieldTypeDropDownList.SelectedIndex.ShouldBe(StringSelectedIndex)
            );
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void SetupFieldType_CommonSwitchCaseConstantsConditionField_SetsFieldTypeDropdownListEnableToFalseAndSelectDate(string switchCaseConstant)
        {
            // Arrange
            var filter = new filters();
            var fieldTypeDropDownList = CreateDropdownList(true);
            ReflectionHelper.SetValue(filter, "ddlFieldType", fieldTypeDropDownList);

            // Act
            var parameters = new object[] { switchCaseConstant };
            typeof(filters).CallMethod("SetupFieldType", parameters, filter);

            // Assert
            var fieldTypeDropdownList = ReflectionHelper.GetFieldInfoFromInstanceByName(filter, "ddlFieldType").GetValue(filter) as DropDownList;
            fieldTypeDropdownList.ShouldNotBeNull();
            fieldTypeDropdownList.Enabled.ShouldBeFalse();
            fieldTypeDropDownList.SelectedIndex.ShouldBe(DateSelectedIndex);
        }

        [Test, TestCaseSource(nameof(UserSwitchCaseConstants))]
        public void SetupFieldType_CommonSwitchCaseConstantsConditionField_SetsFieldTypeDropdownListEnableToTrueAndSelectString(string switchCaseConstant)
        {
            // Arrange
            var filter = new filters();
            var fieldTypeDropDownList = CreateDropdownList(false);
            ReflectionHelper.SetValue(filter, "ddlFieldType", fieldTypeDropDownList);

            // Act
            var parameters = new object[] { switchCaseConstant };
            typeof(filters).CallMethod("SetupFieldType", parameters, filter);

            // Assert
            var fieldTypeDropdownList = ReflectionHelper.GetFieldInfoFromInstanceByName(filter, "ddlFieldType").GetValue(filter) as DropDownList;
            fieldTypeDropdownList.ShouldNotBeNull();
            fieldTypeDropdownList.Enabled.ShouldBeTrue();
            fieldTypeDropDownList.SelectedIndex.ShouldBe(StringSelectedIndex);
        }

        [Test]
        public void SetupFieldType_InvalidConditionField_SetsFieldTypeDropdownListEnableToTrueAndSelectString()
        {
            // Arrange
            var filter = new filters();
            var fieldTypeDropDownList = CreateDropdownList(false);
            ReflectionHelper.SetValue(filter, "ddlFieldType", fieldTypeDropDownList);

            // Act
            var invalidField = "invalid field";
            var parameters = new object[] { invalidField };
            typeof(filters).CallMethod("SetupFieldType", parameters, filter);

            // Assert
            var fieldTypeDropdownList = ReflectionHelper.GetFieldInfoFromInstanceByName(filter, "ddlFieldType").GetValue(filter) as DropDownList;
            fieldTypeDropdownList.ShouldNotBeNull();
            fieldTypeDropdownList.Enabled.ShouldBeTrue();
            fieldTypeDropDownList.SelectedIndex.ShouldBe(StringSelectedIndex);
        }

        private DropDownList CreateDropdownList(bool enabled)
        {
            var dropdownList = new DropDownList()
            {
                Enabled = enabled
            };

            dropdownList.SelectedIndex = -1;
            dropdownList.Items.Add("String");
            dropdownList.Items.Add("Date");

            return dropdownList;
        }

        private static string[] UserSwitchCaseConstants => new string[]
        {
            "User1",
            "User2",
            "User3",
            "User4",
            "User5",
            "User6",
        };

        private static string[] NonCommonSwitchCaseConstants => new string[]
        {
            "Birthdate",
            "UserEvent1Date",
            "UserEvent2Date",
            "CreatedOn",
            "LastChanged",
        };

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
            "UserEvent1",
            "UserEvent2"
        };

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUserName,
                IsActive = true
            };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}
