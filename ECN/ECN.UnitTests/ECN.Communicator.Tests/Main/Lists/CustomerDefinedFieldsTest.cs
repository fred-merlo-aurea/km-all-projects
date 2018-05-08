using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.listsmanager;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
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
    /// <summary>
    /// UT for <see cref="customerdefinedfields"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class CustomerDefinedFieldsTest : BaseListsTest<customerdefinedfields>
    {
        [Test]
        public void Page_Load_HasAccess_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => true;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimDataFieldSets.GetByGroupIDInt32 = (p) => new List<DataFieldSets> { new DataFieldSets { } };
            QueryString.Add("GroupID", "1");
            var CustomDataGrid = privateObject.GetFieldOrProperty("CustomDataGrid") as DataGrid;
            for (int i = 0; i < 7; i++)
            {
                CustomDataGrid.Columns.Add(new BoundColumn());
            }
            var btnAddTransaction = privateObject.GetFieldOrProperty("btnAddTransaction") as Button;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => btnAddTransaction.Visible.ShouldBeTrue(),
                () => CustomDataGrid.Columns[6].Visible.ShouldBeTrue());
        }

        [Test]
        public void Page_Load_DefaultUser_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (p1, p2, p3, p4) => false;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimDataFieldSets.GetByGroupIDInt32 = (p) => new List<DataFieldSets> { new DataFieldSets { } };
            QueryString.Add("GroupID", "1");
            var CustomDataGrid = privateObject.GetFieldOrProperty("CustomDataGrid") as DataGrid;
            for (int i = 0; i < 7; i++)
            {
                CustomDataGrid.Columns.Add(new BoundColumn());
            }
            var btnAddTransaction = privateObject.GetFieldOrProperty("btnAddTransaction") as Button;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => btnAddTransaction.Visible.ShouldBeFalse(),
                () => CustomDataGrid.Columns[6].Visible.ShouldBeFalse());
        }

        [Test]
        public void Add_Button_Click_Default_Success()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            var drpTransactionName = privateObject.GetFieldOrProperty("drpTransactionName") as DropDownList;
            drpTransactionName.Items.Add("1");
            drpTransactionName.SelectedValue = "1";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.Items.Add("default");
            ddlDefaultType.SelectedValue = "default";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            txtDefaultValue.Text = "test";
            GroupDataFieldsDefault result = null;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => result = p;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };

            // Act
            privateObject.Invoke("add_button_Click", new object[] { null, null });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.DataValue.ShouldBe("test"),
                () => result.SystemValue.ShouldBeEmpty());
        }

        [Test]
        public void Add_Button_Click_DefaultException()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            var drpTransactionName = privateObject.GetFieldOrProperty("drpTransactionName") as DropDownList;
            drpTransactionName.Items.Add("1");
            drpTransactionName.SelectedValue = "1";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.Items.Add("default");
            ddlDefaultType.SelectedValue = "default";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            txtDefaultValue.Text = string.Empty;
            GroupDataFieldsDefault result = null;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => result = p;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("add_button_Click", new object[] { null, null });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>GroupDataFieldsDefault: Please enter a value for Default Value"));
        }

        [Test]
        public void Add_Button_Click_SystemValue_Success()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            var drpTransactionName = privateObject.GetFieldOrProperty("drpTransactionName") as DropDownList;
            drpTransactionName.Items.Add("1");
            drpTransactionName.SelectedValue = "1";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.Items.Add("system");
            ddlDefaultType.SelectedValue = "system";
            var ddlSystemValues = privateObject.GetFieldOrProperty("ddlSystemValues") as DropDownList;
            ddlSystemValues.Items.Add("systemTest");
            ddlSystemValues.SelectedValue = "systemTest";
            GroupDataFieldsDefault result = null;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => result = p;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };

            // Act
            privateObject.Invoke("add_button_Click", new object[] { null, null });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.DataValue.ShouldBeEmpty(),
                () => result.SystemValue.ShouldBe("systemTest"));
        }

        [Test]
        public void Add_Button_Click_SystemValue_SaveException()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            var drpTransactionName = privateObject.GetFieldOrProperty("drpTransactionName") as DropDownList;
            drpTransactionName.Items.Add("1");
            drpTransactionName.SelectedValue = "1";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.Items.Add("system");
            ddlDefaultType.SelectedValue = "system";
            var ddlSystemValues = privateObject.GetFieldOrProperty("ddlSystemValues") as DropDownList;
            ddlSystemValues.Items.Add("systemTest");
            ddlSystemValues.SelectedValue = "systemTest";
            GroupDataFieldsDefault result = null;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") });
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("add_button_Click", new object[] { null, null });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Customer: Test Exception"));
        }

        [Test]
        public void BtnCopy_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            var drpSourceGroup = privateObject.GetFieldOrProperty("drpSourceGroup") as DropDownList;
            drpSourceGroup.Items.Add("1");
            drpSourceGroup.SelectedValue = "1";
            var gvUDF = privateObject.GetFieldOrProperty("gvUDF") as GridView;
            gvUDF.DataKeyNames = new string[] { "id" };
            gvUDF.DataSource = new DataTable { Columns = { "id" }, Rows = { { "1" } } };
            gvUDF.DataBind();
            gvUDF.Rows[0].Cells[0].Controls.Add(new CheckBox { ID = "chkCopyUDF", Checked = true });
            gvUDF.Rows[0].Cells[0].Controls.Add(new Label { ID = "lblName" });
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { DatafieldSetID = 1 };
            ShimDataFieldSets.GetByGroupIDNameInt32String = (p1, p2) => null;
            ShimDataFieldSets.SaveDataFieldSetsUser = (p1, p2) => 0;
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 2;
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => new GroupDataFieldsDefault { GDFID = 1 };
            GroupDataFieldsDefault result = null;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => result = p;
            ShimDataFieldSets.GetByDataFieldsetIDInt32Int32Boolean = (p1, p2, p3) =>  new DataFieldSets { };
            ShimDataFieldSets.GetByGroupIDInt32 = (p) => new List<DataFieldSets> { new DataFieldSets { } };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };

            // Act
            privateObject.Invoke("btnCopy_Click", new object[] { null, null });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.GDFID.ShouldBe(2));
        }

        [Test]
        public void DrpSourceGroup_SelectedIndexChanged_Success()
        {
            // Arrange
            InitilizeTestObjects();
            QueryString.Add("GroupID", "1");
            var drpSourceGroup = privateObject.GetFieldOrProperty("drpSourceGroup") as DropDownList;
            drpSourceGroup.Items.Add("2");
            drpSourceGroup.SelectedValue = "2";
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> {  p1 == 1 ? new GroupDataFields { ShortName = "destination" } : new GroupDataFields { ShortName = "source"} };
            var gvUDF = privateObject.GetFieldOrProperty("gvUDF") as GridView;

            // Act
            privateObject.Invoke("drpSourceGroup_SelectedIndexChanged", new object[] { null, null });

            // Assert
            gvUDF.Rows.Count.ShouldBe(1);
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer { CommunicatorLevel = "1"};
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
	}
}