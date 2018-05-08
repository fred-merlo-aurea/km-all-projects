using System.Collections.Generic;
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
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Lists
{
    /// <summary>
    /// UT for <see cref="customdefinedfieldeditor"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class CustomDefinedFieldEditorTest : BaseListsTest<customdefinedfieldeditor>
    {
        [Test]
        public void Page_Load_Delete_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");
            QueryString.Add("delete", "1");
            ShimEmailDataValues.DeleteInt32User = (p1, p2) => { };
            ShimGroupDataFields.DeleteInt32Int32User = (p1, p2, p3) => { };
            ShimGroupDataFieldsDefault.DeleteInt32 = (p) => { };
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields {  DatafieldSetID = 0 };

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("customerdefinedfields.aspx?GroupID=1");
        }

        [Test]
        public void Page_Load_Delete_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { DatafieldSetID = 0 };
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");
            QueryString.Add("delete", "1");
            ShimEmailDataValues.DeleteInt32User = (p1, p2) => throw new ECNException(new List<ECNError> { new ECNError( Enums.Entity.Customer, Enums.Method.Delete, "Test Exception")} );
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Customer: Test Exception"));
        }

        [Test]
        public void Page_Load_GDFDefault_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => new GroupDataFieldsDefault { GDFID = 1, DataValue = "testDefault" };
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            txtDefaultValue.Text.ShouldBe("testDefault");
        }

        [Test]
        public void Page_Load_GDFSystem_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");
            var ddlSystemValues = privateObject.GetFieldOrProperty("ddlSystemValues") as DropDownList;
            ddlSystemValues.Items.Add("systemTest");
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => new GroupDataFieldsDefault { GDFID = 1, SystemValue = "systemTest" };

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            ddlSystemValues.SelectedValue.ShouldBe("systemTest");
        }

        [Test]
        public void Update_Button_Click_SaveDefault_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => null;
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.SelectedValue = "default";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            txtDefaultValue.Text = "data";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => { };
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");

            // Act
            privateObject.Invoke("update_button_Click", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("customerdefinedfields.aspx?GroupID=1");
        }

        [Test]
        public void Update_Button_Click_NoDefaultValue_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => null;
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.SelectedValue = "default";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            txtDefaultValue.Text = string.Empty;
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("update_button_Click", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>GroupDataFieldsDefault: Please enter a value for Default Value"));
        }

        [Test]
        public void Update_Button_Click_Save_ExceptionSuccess()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => null;
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.SelectedValue = "default";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            txtDefaultValue.Text = "data";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.Customer, Enums.Method.Save, "Test Exception") }); ;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => { };
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("update_button_Click", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => phError.Visible.ShouldBeTrue(),
                () => lblErrorMessage.Text.ShouldBe("<br/>Customer: Test Exception"));
        }

        [Test]
        public void Update_Button_Click_SaveSystem_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => null;
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.SelectedValue = "system";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            txtDefaultValue.Text = "data";
            var ddlSystemValues = privateObject.GetFieldOrProperty("ddlSystemValues") as DropDownList;
            ddlSystemValues.Items.Add("systemTest");
            ddlSystemValues.SelectedValue = "system";
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            ShimGroupDataFieldsDefault.SaveGroupDataFieldsDefault = (p) => { };
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");

            // Act
            privateObject.Invoke("update_button_Click", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("customerdefinedfields.aspx?GroupID=1");
        }

        [Test]
        public void Update_Button_Click_Delete_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimGroupDataFieldsDefault.GetByGDFIDInt32 = (p) => null;
            ShimGroupDataFields.GetByIDInt32Int32User = (p1, p2, p3) => new GroupDataFields { IsPublic = "Y", DatafieldSetID = 2 };
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = false;
            ShimGroupDataFields.SaveGroupDataFieldsUser = (p1, p2) => 0;
            ShimGroupDataFieldsDefault.DeleteInt32 = (p) => { };
            QueryString.Add("GroupID", "1");
            QueryString.Add("GroupDatafieldsID", "1");

            // Act
            privateObject.Invoke("update_button_Click", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("customerdefinedfields.aspx?GroupID=1");
        }

        [Test]
        public void ChkUseDefaultValue_CheckedChanged_Checked_Default_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.SelectedValue = "default";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            var ddlSystemValues = privateObject.GetFieldOrProperty("ddlSystemValues") as DropDownList;

            // Act
            privateObject.Invoke("chkUseDefaultValue_CheckedChanged", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => txtDefaultValue.Visible.ShouldBeTrue(),
                () => ddlSystemValues.Visible.ShouldBeFalse());
        }

        [Test]
        public void ChkUseDefaultValue_CheckedChanged_Checked_System_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = true;
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.SelectedValue = "system";
            var txtDefaultValue = privateObject.GetFieldOrProperty("txtDefaultValue") as TextBox;
            var ddlSystemValues = privateObject.GetFieldOrProperty("ddlSystemValues") as DropDownList;

            // Act
            privateObject.Invoke("chkUseDefaultValue_CheckedChanged", new object[] { null, null });

            // Assert
            privateObject.ShouldSatisfyAllConditions(
                () => txtDefaultValue.Visible.ShouldBeFalse(),
                () => ddlSystemValues.Visible.ShouldBeTrue());
        }

        [Test]
        public void ChkUseDefaultValue_CheckedChanged_NotChecked_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var chkUseDefaultValue = privateObject.GetFieldOrProperty("chkUseDefaultValue") as CheckBox;
            chkUseDefaultValue.Checked = false;
            var pnlDefaultValue = privateObject.GetFieldOrProperty("pnlDefaultValue") as Panel;

            // Act
            privateObject.Invoke("chkUseDefaultValue_CheckedChanged", new object[] { null, null });

            // Assert
            pnlDefaultValue.Visible.ShouldBeFalse();
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
            var ddlDefaultType = privateObject.GetFieldOrProperty("ddlDefaultType") as DropDownList;
            ddlDefaultType.Items.Add("default");
            ddlDefaultType.Items.Add("system");
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