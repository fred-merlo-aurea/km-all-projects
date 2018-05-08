using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.lists;
using ecn.communicator.main.lists.Fakes;
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
    /// UT for <see cref="groupsubscribePrePopSF"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GroupsubscribePrePopSFTest : BaseListsTest<groupsubscribePrePopSF>
    {
        private const int GroupId = 10;
        private const string GroupIdQueryStringKey = "GroupID";
        private const string GroupIdPropertyName = "GroupId";
        private const int SFID = 20;
        private const string SFIDPropertyName = "SFID";
        private const string SFIDQueryStringKey = "SFID";
        private const string Action = "Edit";
        private const string ActionPropertyName = "RequestedAction";
        private const string ActionQueryStringKey = "action";
        private const int ChannelId = 30;
        private const string ChannelIdPropertyName = "ChannelId";
        private const string ChannelIdQueryStringKey = "chID";
        private const int CustomerId = 40;
        private const string CustomerIdPropertyName = "CustomerId";
        private const string CustomerIdQueryStringKey = "cuID";

        [Test]
        public void Page_Load_DeleteRequest_Admin_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimUser.IsSystemAdministratorUser = (p) => true;
            ShimSmartFormsHistory.GetByGroupIDInt32User = (p1, p2) => new List<SmartFormsHistory> { new SmartFormsHistory { } };
            ShimSmartFormsPrePopFields.DeleteInt32User = (p1, p2) => { };
            ShimSmartFormsHistory.DeleteInt32User = (p1, p2) => { };
            QueryString.Add("chID", "1");
            QueryString.Add("cuID", "1");
            QueryString.Add("SFID", "1");
            QueryString.Add("action", "DELETE");
            var msgLabel = privateObject.GetFieldOrProperty("msglabel") as Label;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("groupsubscribePrePopSF.aspx?GroupID=0&chID=1&cuID=1");
            msgLabel.Visible.ShouldBeFalse();
        }

        [Test]
        public void Page_Load_NewRequest_Admin_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimUser.IsSystemAdministratorUser = (p) => true;
            ShimSmartFormsHistory.GetByGroupIDInt32User = (p1, p2) => new List<SmartFormsHistory> { new SmartFormsHistory { Type = "PPSO" } };
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (p1, p2, p3) => new SmartFormsHistory { };
            ShimSmartFormsPrePopFields.GetBySFIDInt32User = (p1, p2) => new List<SmartFormsPrePopFields> { new SmartFormsPrePopFields { } };
            QueryString.Add("chID", "1");
            QueryString.Add("cuID", "1");
            QueryString.Add("SFID", "1");
            QueryString.Add("action", "NEW");
            var msgLabel = privateObject.GetFieldOrProperty("msglabel") as Label;
            var so_Save = privateObject.GetFieldOrProperty("SO_Save") as Button;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBeEmpty();
            msgLabel.Visible.ShouldBeFalse();
            so_Save.Visible.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_DefaultUser_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => true;
            ShimUser.IsSystemAdministratorUser = (p) => false;

            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBe("../default.aspx");
        }

        [Test]
        public void Page_Load_NoProductFeature_Success()
        {
            // Arrange
            InitilizeTestObjects();
            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (p1, p2, p3) => false;
            var msgLabel = privateObject.GetFieldOrProperty("msglabel") as Label;
            
            // Act
            privateObject.Invoke("Page_Load", new object[] { null, null });

            // Assert
            RedirectUrl.ShouldBeEmpty();
            msgLabel.Visible.ShouldBeTrue();
        }

        [Test]
        public void PrePopProfileFieldsList_ItemCommand_Edit_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var args = new DataListCommandEventArgs(new DataListItem(0, ListItemType.Item), null, new CommandEventArgs("Edit", null));
            ShimSmartFormsPrePopFields.GetBySFIDInt32User = (p1, p2) => new List<SmartFormsPrePopFields> { new SmartFormsPrePopFields { } };
            var prePopProfileFieldsList = privateObject.GetFieldOrProperty("PrePopProfileFieldsList") as DataList;

            // Act
            privateObject.Invoke("PrePopProfileFieldsList_ItemCommand", new object[] { null, args });

            // Assert
            prePopProfileFieldsList.EditItemIndex.ShouldBe(0);
        }

        [Test]
        public void PrePopProfileFieldsList_ItemCommand_Update_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.Item);
            item.Controls.Add(new DropDownList { ID = "Edit_ProfileFieldNameDR" });
            item.Controls.Add(new TextBox { ID = "Edit_DisplayNameTXT" });
            item.Controls.Add(new DropDownList { ID = "Edit_DataTypeDR" });
            item.Controls.Add(new DropDownList { ID = "Edit_ControlTypeDR" });
            item.Controls.Add(new TextBox { ID = "Edit_DataValuesTXT" });
            item.Controls.Add(new CheckBox { ID = "Edit_RequiredCHKBX", Checked = true });
            item.Controls.Add(new CheckBox { ID = "Edit_PrePopulateCHKBX", Checked = true });
            item.Controls.Add(new TextBox { ID = "Edit_SortOrderTXT", Text = "1"});
            var args = new DataListCommandEventArgs(item, null, new CommandEventArgs("Update", null));
            ShimSmartFormsPrePopFields.GetBySFIDInt32User = (p1, p2) => new List<SmartFormsPrePopFields> { new SmartFormsPrePopFields { ProfileFieldName = "EmailAddress" } };
            var prePopProfileFieldsList = privateObject.GetFieldOrProperty("PrePopProfileFieldsList") as DataList;
            ShimBaseDataList.AllInstances.DataKeysGet = (p) => new DataKeyCollection(new ArrayList { 1 });
            ShimSmartFormsPrePopFields.GetByPrePopFieldIDInt32User = (p1, p2) => new SmartFormsPrePopFields { };
            ShimSmartFormsPrePopFields.SaveSmartFormsPrePopFieldsUser = (p1, p2) =>  0;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (p1, p2, p3) => new SmartFormsHistory { };
            ShimSmartFormsHistory.SaveSmartFormsHistoryUser = (p1,p2) => 0;

            // Act
            privateObject.Invoke("PrePopProfileFieldsList_ItemCommand", new object[] { null, args });

            // Assert
            prePopProfileFieldsList.EditItemIndex.ShouldBe(-1);
        }

        [Test]
        public void PrePopProfileFieldsList_ItemCommand_Delete_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.Item);
            var args = new DataListCommandEventArgs(item, null, new CommandEventArgs("Delete", null));
            ShimSmartFormsPrePopFields.GetBySFIDInt32User = (p1, p2) => new List<SmartFormsPrePopFields> { new SmartFormsPrePopFields { ProfileFieldName = "EmailAddress" } };
            var prePopProfileFieldsList = privateObject.GetFieldOrProperty("PrePopProfileFieldsList") as DataList;
            ShimBaseDataList.AllInstances.DataKeysGet = (p) => new DataKeyCollection(new ArrayList { 1 });
            ShimSmartFormsPrePopFields.DeleteInt32Int32User = (p1, p2, p3) => { };
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = (p1, p2, p3) => new List<GroupDataFields> { new GroupDataFields { } };
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (p1, p2, p3) => new SmartFormsHistory { };
            ShimSmartFormsHistory.SaveSmartFormsHistoryUser = (p1, p2) => 0;

            // Act
            privateObject.Invoke("PrePopProfileFieldsList_ItemCommand", new object[] { null, args });

            // Assert
            prePopProfileFieldsList.EditItemIndex.ShouldBe(-1);
        }

        [Test]
        public void PrePopProfileFieldsList_ItemCommand_Cancel_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var args = new DataListCommandEventArgs(new DataListItem(0, ListItemType.Item), null, new CommandEventArgs("Cancel", null));
            ShimSmartFormsPrePopFields.GetBySFIDInt32User = (p1, p2) => new List<SmartFormsPrePopFields> { new SmartFormsPrePopFields { } };
            var prePopProfileFieldsList = privateObject.GetFieldOrProperty("PrePopProfileFieldsList") as DataList;

            // Act
            privateObject.Invoke("PrePopProfileFieldsList_ItemCommand", new object[] { null, args });

            // Assert
            prePopProfileFieldsList.EditItemIndex.ShouldBe(-1);
        }

        [Test]
        public void ProfileFieldAdd_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.Item);
            item.Controls.Add(new DropDownList { ID = "Add_ProfileFieldNameDR" });
            item.Controls.Add(new TextBox { ID = "Add_DisplayNameTXT", Text = "Test"});
            item.Controls.Add(new DropDownList { ID = "Add_DataTypeDR" });
            item.Controls.Add(new DropDownList { ID = "Add_ControlTypeDR" });
            item.Controls.Add(new TextBox { ID = "Add_DataValuesTXT" });
            item.Controls.Add(new CheckBox { ID = "Add_RequiredCHKBX", Checked = true });
            item.Controls.Add(new CheckBox { ID = "Add_PrePopulateCHKBX", Checked = true });
            item.Controls.Add(new TextBox { ID = "Add_SortOrderTXT", Text = "1" });
            var button = new LinkButton { };
            item.Controls.Add(button);
            SmartFormsPrePopFields smartFormsPrePopFields = null;
            ShimSmartFormsPrePopFields.SaveSmartFormsPrePopFieldsUser = (p1, p2) => 
            {
                smartFormsPrePopFields = p1;
                return 0;
            };

            // Act
            privateObject.Invoke("ProfileFieldAdd_Click", new object[] { button, null });

            // Assert
            smartFormsPrePopFields.ShouldSatisfyAllConditions(
                () => smartFormsPrePopFields.ShouldNotBeNull(),
                () => smartFormsPrePopFields.DisplayName.ShouldBe("Test"));
        }

        [Test]
        public void ProfileFieldAdd_Click_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.Item);
            item.Controls.Add(new DropDownList { ID = "Add_ProfileFieldNameDR" });
            item.Controls.Add(new TextBox { ID = "Add_DisplayNameTXT", Text = "Test" });
            item.Controls.Add(new DropDownList { ID = "Add_DataTypeDR" });
            item.Controls.Add(new DropDownList { ID = "Add_ControlTypeDR" });
            item.Controls.Add(new TextBox { ID = "Add_DataValuesTXT" });
            item.Controls.Add(new CheckBox { ID = "Add_RequiredCHKBX", Checked = true });
            item.Controls.Add(new CheckBox { ID = "Add_PrePopulateCHKBX", Checked = true });
            item.Controls.Add(new TextBox { ID = "Add_SortOrderTXT", Text = "1" });
            var button = new LinkButton { };
            item.Controls.Add(button);
            ShimSmartFormsPrePopFields.SaveSmartFormsPrePopFieldsUser = (p1, p2) =>
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.DomainSuppression, Enums.Method.Save, "Test Exception") });
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("ProfileFieldAdd_Click", new object[] { button, null });

            // Assert
            phError.Visible.ShouldBeTrue();
            lblErrorMessage.Text.ShouldBe("<br/>DomainSuppression: Test Exception");
        }

        [Test]
        public void SO_New_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            SmartFormsHistory smartFormsHistory = null;
            ShimSmartFormsHistory.SaveSmartFormsHistoryUser = (p1, p2) =>
            {
                smartFormsHistory = p1;
                return 0;
            };
            ShimSmartFormsPrePopFields.SaveSmartFormsPrePopFieldsUser = (p1, p2) => 0;
            ShimgroupsubscribePrePopSF.AllInstances.LoadSmartFormGridInt32 = (p1, p2) => { };

            // Act
            privateObject.Invoke("SO_New_Click", new object[] { null, null });

            // Assert
            smartFormsHistory.ShouldNotBeNull();
            RedirectUrl.ShouldBe("groupsubscribePrePopSF.aspx?SFID=-1&GroupID=0&chID=0&cuID=0");
        }

        [Test]
        public void SO_New_Click_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            ShimSmartFormsHistory.SaveSmartFormsHistoryUser = (p1, p2) =>
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.DomainSuppression, Enums.Method.Save, "Test Exception") });
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("SO_New_Click", new object[] { null, null });

            // Assert
            phError.Visible.ShouldBeTrue();
            lblErrorMessage.Text.ShouldBe("<br/>DomainSuppression: Test Exception");
            RedirectUrl.ShouldBeEmpty();
        }

        [Test]
        public void PrePopProfileFieldsList_ItemDataBound_Edit_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.EditItem);
            item.Controls.Add(new DropDownList { ID = "Edit_ProfileFieldNameDR" });
            var edit_DisplayNameTXT = new TextBox { ID = "Edit_DisplayNameTXT" };
            item.Controls.Add(edit_DisplayNameTXT);
            item.Controls.Add(new DropDownList { ID = "Edit_DataTypeDR" });
            item.Controls.Add(new DropDownList { ID = "Edit_ControlTypeDR" });
            item.Controls.Add(new TextBox { ID = "Edit_DataValuesTXT" });
            item.Controls.Add(new CheckBox { ID = "Edit_RequiredCHKBX", Checked = true });
            item.Controls.Add(new CheckBox { ID = "Edit_PrePopulateCHKBX", Checked = true });
            item.Controls.Add(new TextBox { ID = "Edit_SortOrderTXT", Text = "1" });
            var args = new DataListItemEventArgs(item);
            ShimSmartFormsPrePopFields.GetColumnNamesInt32Int32 = (p1, p2) => new DataTable { };
            ShimBaseDataList.AllInstances.DataKeysGet = (p) => new DataKeyCollection(new ArrayList { 1 });
            var smartFormsPrePopFields = new SmartFormsPrePopFields { DisplayName = "Test" };
            ShimSmartFormsPrePopFields.GetByPrePopFieldIDInt32User = (p1, p2) => smartFormsPrePopFields;  

            // Act
            privateObject.Invoke("PrePopProfileFieldsList_ItemDataBound", new object[] { null, args });

            // Assert
            edit_DisplayNameTXT.Text.ShouldBe("Test");
        }

        [Test]
        public void PrePopProfileFieldsList_ItemDataBound_Item_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.Item);
            var prePopFieldDelete = new LinkButton { ID = "PrePopFieldDelete" };
            item.Controls.Add(prePopFieldDelete);
            var args = new DataListItemEventArgs(item);
            ShimSmartFormsPrePopFields.GetColumnNamesInt32Int32 = (p1, p2) => new DataTable { };
            ShimBaseDataList.AllInstances.DataKeysGet = (p) => new DataKeyCollection(new ArrayList { 1 });

            // Act
            privateObject.Invoke("PrePopProfileFieldsList_ItemDataBound", new object[] { null, args });

            // Assert
            prePopFieldDelete.Attributes["onclick"].ShouldBe("return confirm('LINK ID: 1 - Are you sure that you want to delete this Tracking Link ?')");
        }

        [Test]
        public void PrePopProfileFieldsList_ItemDataBound_Footer_Success()
        {
            // Arrange
            InitilizeTestObjects();
            var item = new DataListItem(0, ListItemType.Footer);
            var Add_ProfileFieldNameDR = new DropDownList { ID = "Add_ProfileFieldNameDR" };
            item.Controls.Add(Add_ProfileFieldNameDR);
            var args = new DataListItemEventArgs(item);
            ShimSmartFormsPrePopFields.GetColumnNamesInt32Int32 = (p1, p2) => new DataTable { };

            // Act, Assert
            Should.NotThrow(() => privateObject.Invoke("PrePopProfileFieldsList_ItemDataBound", new object[] { null, args }));
        }

        [Test]
        public void SO_Save_Click_Success()
        {
            // Arrange
            InitilizeTestObjects();
            SmartFormsHistory smartFormsHistory = null;
            ShimSmartFormsHistory.SaveSmartFormsHistoryUser = (p1, p2) =>
            {
                smartFormsHistory = p1;
                return 0;
            };
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (p1, p2, p3) => new SmartFormsHistory { };

            // Act
            privateObject.Invoke("SO_Save_Click", new object[] { null, null });

            // Assert
            smartFormsHistory.ShouldNotBeNull();
            RedirectUrl.ShouldBe("groupsubscribePrePopSF.aspx?GroupID=0&chID=0&cuID=0");
        }

        [Test]
        public void SO_Save_Click_Exception()
        {
            // Arrange
            InitilizeTestObjects();
            ShimSmartFormsHistory.GetBySmartFormIDInt32Int32User = (p1, p2, p3) =>
                throw new ECNException(new List<ECNError> { new ECNError(Enums.Entity.DomainSuppression, Enums.Method.Save, "Test Exception") });
            var phError = privateObject.GetFieldOrProperty("phError") as PlaceHolder;
            var lblErrorMessage = privateObject.GetFieldOrProperty("lblErrorMessage") as Label;

            // Act
            privateObject.Invoke("SO_Save_Click", new object[] { null, null });

            // Assert
            phError.Visible.ShouldBeTrue();
            lblErrorMessage.Text.ShouldBe("<br/>DomainSuppression: Test Exception");
            RedirectUrl.ShouldBeEmpty();
        }

        [Test]
        public void GroupIdGetter_IfQueryStringContainsGroupId_ReturnsGroupId()
        {
            // Arrange
            QueryString.Add(GroupIdQueryStringKey, GroupId.ToString());

            // Act
            var returnedValue = privateObject.GetProperty(GroupIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(GroupId));
        }

        [Test]
        public void GroupIdGetter_IfQueryStringDoesNotContainGroupId_ReturnsDefaultValue()
        {
            // Arrange
            // set no groupId to query string

            // Act
            var returnedValue = privateObject.GetProperty(GroupIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        [Test]
        public void SFIDGetter_IfQueryStringContainsSFID_ReturnsSFID()
        {
            // Arrange
            QueryString.Add(SFIDQueryStringKey, SFID.ToString());

            // Act
            var returnedValue = privateObject.GetProperty(SFIDPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(SFID));
        }

        [Test]
        public void SFIDGetter_IfQueryStringDoesNotContainSFID_ReturnsDefaultValue()
        {
            // Arrange
            // set no sfid to query string

            // Act
            var returnedValue = privateObject.GetProperty(SFIDPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        [Test]
        public void RequestedActionGetter_IfQueryStringContainsAction_ReturnsAction()
        {
            // Arrange
            QueryString.Add(ActionQueryStringKey, Action);

            // Act
            var returnedValue = privateObject.GetProperty(ActionPropertyName) as string;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<string>(),
                () => returnedValue.ShouldBe(Action));
        }

        [Test]
        public void RequestedActionGetter_IfQueryStringDoesNotContainAction_ReturnsDefaultValue()
        {
            // Arrange
            // set no action to query string

            // Act
            var returnedValue = privateObject.GetProperty(ActionPropertyName) as string;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<string>(),
                () => returnedValue.ShouldBeNull());
        }

        [Test]
        public void ChannelIdGetter_IfQueryStringContainsChannelId_ReturnsChannelId()
        {
            // Arrange
            QueryString.Add(ChannelIdQueryStringKey, ChannelId.ToString());

            // Act
            var returnedValue = privateObject.GetProperty(ChannelIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(ChannelId));
        }

        [Test]
        public void ChannelIdGetter_IfQueryStringDoesNotContainChannelId_ReturnsDefaultValue()
        {
            // Arrange
            // set no channelId to query string

            // Act
            var returnedValue = privateObject.GetProperty(ChannelIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        [Test]
        public void CustomerId_IfQueryStringContainsCustomerId_ReturnsCustomerId()
        {
            // Arrange
            QueryString.Add(CustomerIdQueryStringKey, CustomerId.ToString());

            // Act
            var returnedValue = privateObject.GetProperty(CustomerIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(CustomerId));
        }

        [Test]
        public void CustomerId_IfQueryStringDoesNotContainCustomerId_ReturnsDefaultValue()
        {
            // Arrange
            // set no customerId to query string

            // Act
            var returnedValue = privateObject.GetProperty(CustomerIdPropertyName) as int?;

            // Assert
            returnedValue.ShouldSatisfyAllConditions(
                () => returnedValue.ShouldBeOfType<int>(),
                () => returnedValue.ShouldBe(default(int)));
        }

        private void InitilizeTestObjects()
        {
            InitializeAllControls(testObject);
            CreateMasterPage();
            QueryString = new NameValueCollection();
            RedirectUrl = string.Empty;
        }

        private void CreateMasterPage()
        {
            var master = new ecn.communicator.MasterPages.Communicator();
            InitializeAllControls(master);
            ShimPage.AllInstances.MasterGet = (instance) => master;
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User();
                session.CurrentCustomer = new Customer();
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