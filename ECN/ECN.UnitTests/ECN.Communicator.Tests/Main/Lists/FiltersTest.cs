using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    [TestFixture]
    public class FiltersTest : BaseListsTest<filtersplus>
    {
        private const string _invalidShortName = "Invalid Short Name";

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void IsProfileField_CommonSwitchCaseShortNameConstants_ReturnsTrue(string switchCaseConstant)
        {
            // Arrange
            var filters = new filtersplus();

            // Act
            var parameters = new object[] { switchCaseConstant };
            var isProfileField = (bool)typeof(filtersplus).CallMethod("IsProfileField", parameters, filters);

            // Assert
            isProfileField.ShouldBeTrue();
        }

        [Test]
        public void IsProfileField_InvalidShortName_ReturnsFalse()
        {
            // Arrange
            var filters = new filtersplus();

            // Act
            var parameters = new object[] { _invalidShortName };
            var isProfileField = (bool)typeof(filtersplus).CallMethod("IsProfileField", parameters, filters);

            // Assert
            isProfileField.ShouldBeFalse();
        }

        [Test]
        public void Page_Load_DefaultUser_Success()
        {
            // Arrange
            InitilizeTestObjects();

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("../default.aspx");
        }

        [Test]
        public void Page_Load_HasAccess_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimGroup.GetByGroupIDInt32User = (p1,p2) => new Group { GroupName = "Test Name"};
            var groupNameDisplay = privateObject.GetFieldOrProperty("GroupNameDisplay") as Label;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            groupNameDisplay.Text.ShouldBe("Test Name");
        }

        [Test]
        public void BtnCopy_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimFilter.GetByGroupIDInt32BooleanUserString = (p1, p2, p3, p4) => new List<Filter> { new Filter { CreatedDate = new DateTime(), GroupID = 1 } };
            var viewState = new StateBag();
            viewState.Add("SortField", "GroupID");
            viewState.Add("SortDirection", "asc");
            ShimControl.AllInstances.ViewStateGet = (p) => viewState;
            QueryString.Add("GroupID", "1");
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            var gvFilters = privateObject.GetFieldOrProperty("gvFilters") as GridView;
            gvFilters.DataKeyNames = new string[] { "id" };
            gvFilters.DataSource = new DataTable { Columns = { "id" }, Rows = { { "1" } } };
            gvFilters.DataBind();
            gvFilters.Rows[0].Cells[0].Controls.Add(new CheckBox { ID = "chkCopyFilter", Checked = true });
            ShimFilter.GetByFilterIDInt32User = (p1, p2) => new Filter { WhereClause = "[test]",
                FilterGroupList = new List<FilterGroup> { new FilterGroup { FilterConditionList = new List<FilterCondition> { new FilterCondition { } } } } };
            ShimGroupDataFields.ExistsStringNullableOfInt32Int32Int32 = (p1, p2, p3, p4) => true;
            var filterSaved = false;
            ShimFilter.SaveFilterUser = (p1, p2) => { filterSaved = true; return 0; };

            // Act
            privateObject.Invoke("btnCopy_Click", new object[] { null, null });

            // Assert
            filterSaved.ShouldBeTrue();
        }

        [Test]
        public void BtnCopy_Click_SaveException()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            var gvFilters = privateObject.GetFieldOrProperty("gvFilters") as GridView;
            gvFilters.DataKeyNames = new string[] { "id" };
            gvFilters.DataSource = new DataTable { Columns = { "id" }, Rows = { { "1" } } };
            gvFilters.DataBind();
            gvFilters.Rows[0].Cells[0].Controls.Add(new CheckBox { ID = "chkCopyFilter", Checked = true });
            ShimFilter.GetByFilterIDInt32User = (p1, p2) => new Filter
            {
                WhereClause = "[test]",
                FilterGroupList = new List<FilterGroup> { new FilterGroup { FilterConditionList = new List<FilterCondition> { new FilterCondition { } } } }
            };
            ShimGroupDataFields.ExistsStringNullableOfInt32Int32Int32 = (p1, p2, p3, p4) => true;
            ShimFilter.SaveFilterUser = (p1, p2) => throw new ECNException(new List<ECNError> { new ECNError( Enums.Entity.Filter, Enums.Method.Save, "Test Error")});
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("btnCopy_Click", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Filter: Test Error"));
        }

        [Test]
        public void BtnCopy_Click_MissingFieldsException()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            var gvFilters = privateObject.GetFieldOrProperty("gvFilters") as GridView;
            gvFilters.DataKeyNames = new string[] { "id" };
            gvFilters.DataSource = new DataTable { Columns = { "id" }, Rows = { { "1" } } };
            gvFilters.DataBind();
            gvFilters.Rows[0].Cells[0].Controls.Add(new CheckBox { ID = "chkCopyFilter", Checked = true });
            ShimFilter.GetByFilterIDInt32User = (p1, p2) => new Filter { WhereClause = "[test]" };
            ShimGroupDataFields.ExistsStringNullableOfInt32Int32Int32 = (p1, p2, p3, p4) => false;
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("btnCopy_Click", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Filter: Please add the following UDFs to the group: test"));
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
            var filterGrid = privateObject.GetFieldOrProperty("FilterGrid") as GridView;
            if (filterGrid != null)
            {
                filterGrid.Columns.Add(new BoundField());
                filterGrid.Columns.Add(new BoundField());
                filterGrid.Columns.Add(new BoundField());
                filterGrid.Columns.Add(new BoundField());
            }
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1" };
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                         new HttpStaticObjectsCollection(), 10, true,
                                         HttpCookieMode.AutoDetect,
                                         SessionStateMode.InProc, false);
            var sessionState = typeof(HttpSessionState).GetConstructor(
                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                     null, CallingConventions.Standard,
                                     new[] { typeof(HttpSessionStateContainer) },
                                     null)
                                .Invoke(new object[] { sessionContainer }) as HttpSessionState;
            ShimUserControl.AllInstances.SessionGet = (p) => sessionState;
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
