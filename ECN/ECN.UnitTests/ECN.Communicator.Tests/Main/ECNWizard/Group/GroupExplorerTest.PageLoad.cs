using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient.Fakes;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using System.Xml.Fakes;
using AjaxControlToolkit.Fakes;
using ecn.communicator.includes.Fakes;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.controls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Accounts;
using ECN_Framework_DataLayer.Fakes;
using KM.Framework.Web.WebForms.FolderSystem.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;
using CommFakeDataLayer = ECN_Framework_DataLayer.Communicator.Fakes;
using CommunicatorEntites = ECN_Framework_Entities.Communicator;
using PlatformFake = KM.Platform.Fakes;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    public partial class GroupExplorerTest
    {
        private const string MethodPageLoad = "Page_Load";
        private const string MethodbtnSaveFilterClick = "btnSaveFilter_Click";
        private const string MethodGroupsGridCommand = "GroupsGrid_Command";
        private const string MethodBtndownload_Click = "btndownload_Click";
        private const string MethodSetSelectedGroupMethod = "setSelectedGroup";
        private const string MethodSetSuppressionGroup = "setSuppressionGroup";
        private const string MethodImgbtnCreateFilterClick = "imgbtnCreateFilter_Click";
        private const string BlastLicensed = "BlastLicensed";
        private const string PnlDownloadId = "pnlDownload";
        private const string DDLArchiveFilterId = "ddlArchiveFilter";
        private const string PnlSelectedGroupId = "pnlSelectedGroup";
        private const string LnkbtnAddGroupId = "lnkbtnAddGroup";
        private const string ChkAllFoldersId = "chkAllFolders";
        private const string SearchCriteriaId = "SearchCriteria";
        private const string HfShowSelectId = "hfShowSelect";
        private const string RBlFilterTypeId = "rblFilterType";
        private const string DDLSmartSegmentId = "ddlSmartSegment";
        private const string LSTboxBlastId = "lstboxBlast";
        private const string LSTboxAvailableFiltersId = "lbAvailableFilters";
        private const string LblRefBlastErrorId = "lblRefBlastError";
        private const string SearchGrpsDRId = "SearchGrpsDR";
        private const string SearchTypeDRId = "SearchTypeDR";
        private const string LblTotalRecordsId = "lblTotalRecords";
        private const string UpMainId = "upMain";
        private const string BlastThisId = "BlastThis";
        private const string PnlPagerId = "pnlPager";
        private const string TotalRecord = "20";
        private const string Blast = "100";
        private const int FilterId = 100;
        private StateBag _viewState;
        private bool _groupDeleted;

        [TestCase(Enums.LicenseOption.unlimited, false)]
        [TestCase(Enums.LicenseOption.unlimited, true)]
        [TestCase(Enums.LicenseOption.limited, true)]
        public void Page_Load_RequestGroupIdNotZero_GroupDeleted(Enums.LicenseOption licenseOpt, bool isSelect)
        {
            // Arrange
            InitTestPageLoad();
            ECN_Framework_DataLayer.Accounts.Fakes.ShimCustomerLicense.GetCurrentLicensesByCustomerIDInt32EnumsLicenseTypeCode = (custId, licCode) => new ECN_Framework_Entities.Accounts.License() { LicenseOption = licenseOpt, Allowed = 10 };
            var blastLicenseLabel = Get<Label>(_privateGroupExplorerObj, BlastLicensed);
            QueryString.Add(GroupIDKey, "2");
            _privateGroupExplorerObj.SetField("IsSelect", BindingFlags.Static | BindingFlags.NonPublic, isSelect);
            var licenseLabelExpectedText = string.Empty;
            if (isSelect)
            {
                licenseLabelExpectedText = licenseOpt == Enums.LicenseOption.unlimited
                    ? "UNLIMITED"
                    : "10";
            }

            // Act
            _privateGroupExplorerObj.Invoke(MethodPageLoad, new object[] { null, EventArgs.Empty });

            // Assert
            blastLicenseLabel.ShouldSatisfyAllConditions(
                () => _groupDeleted.ShouldBeTrue(),
                () => blastLicenseLabel.ShouldNotBeNull(),
                () => blastLicenseLabel.Text.ShouldBe(licenseLabelExpectedText));
        }

        [TestCase("", "Group")]
        [TestCase("searchCriteria", "Profile")]
        [TestCase("searchCriteria", "Group")]
        public void Reset_ControlsReset(string searchCriteria, string searchType)
        {
            // Arrange 
            InitTestReset(searchCriteria, searchType);
            var blastThis = Get<Label>(_privateGroupExplorerObj, BlastThisId);
            var lblTotalRecords = Get<Label>(_privateGroupExplorerObj, LblTotalRecordsId);
            var pnlPager = Get<Panel>(_privateGroupExplorerObj, PnlPagerId);
            var chkAllFoldersChkBox = Get<CheckBox>(_privateGroupExplorerObj, ChkAllFoldersId);
            pnlPager.Visible = false;
            chkAllFoldersChkBox.Checked = true;

            // Act
            _groupExplorer.reset();

            // Assert
            chkAllFoldersChkBox.ShouldSatisfyAllConditions(
                () => chkAllFoldersChkBox.Checked.ShouldBeFalse(),
                () => lblTotalRecords.Text.ShouldBe(TotalRecord),
                () => blastThis.Text.ShouldBe(Blast),
                () => pnlPager.Visible.ShouldBeTrue());
        }

        [TestCase("select", "smart")]
        public void BtnSaveFilter_Click_NoRefBlast_Error(string suppressOrSelect, string filterType)
        {
            // Arrange
            var lblBlastError = Get<Label>(_privateGroupExplorerObj, LblRefBlastErrorId);
            lblBlastError.Visible = false;
            InitTestBtnSaveFilter(suppressOrSelect, 100, filterType, false);

            // Act
            _privateGroupExplorerObj.Invoke(MethodbtnSaveFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            lblBlastError.ShouldSatisfyAllConditions(
                () => lblBlastError.Visible.ShouldBeTrue());
        }

        [Test]
        public void BtnSaveFilter_Click_NoMatchGroupId_PopupHide()
        {
            // Arrange
            var popupHide = false;
            ShimModalPopupExtender.AllInstances.Hide = (popup) => { popupHide = true; };
            InitTestBtnSaveFilter("select", 100, "smart", addMatchedGroupId: false);

            // Act
            _privateGroupExplorerObj.Invoke(MethodbtnSaveFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            popupHide.ShouldSatisfyAllConditions(
                () => popupHide.ShouldBeTrue());
        }

        [TestCase("select", "smart")]
        [TestCase("suppress", "smart")]
        [TestCase("testselect", "custom")]
        public void BtnSaveFilter_Click_MatchedGroupId_BindMethodCalled(string suppressOrSelect, string filterType)
        {
            // Arrange
            var dataBindSelectCalled = false;
            var dataBindSuppressionCalled = false;
            var blastLicenseCountUpdatedCalled = false;
            InitTestBtnSaveFilter(suppressOrSelect, 100, filterType);
            ShimgroupExplorer.AllInstances.blastLicenseCount_Update = (p) => { blastLicenseCountUpdatedCalled = true; };
            ShimgroupExplorer.AllInstances.DataBindSelected = (p) => { dataBindSelectCalled = true; };
            ShimgroupExplorer.AllInstances.DataBindSuppression = (p) => { dataBindSuppressionCalled = true; };

            // Act
            _privateGroupExplorerObj.Invoke(MethodbtnSaveFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            blastLicenseCountUpdatedCalled.ShouldSatisfyAllConditions(
                () => blastLicenseCountUpdatedCalled.ShouldBeTrue(),
                () => dataBindSelectCalled.ShouldBe(suppressOrSelect == "select"),
                () => dataBindSuppressionCalled.ShouldBe(suppressOrSelect == "suppress"));
        }

        [TestCase("DeleteGroup")]
        [TestCase("SelectGroup")]
        [TestCase("SuppressGroup")]
        public void GroupsGrid_Command_DifferentCommand_ProperMethodCalled(string command)
        {
            // Arrange
            InitTestGroupsGridCommand();
            var deleteGroupCalled = false;
            var setSelectedGroupCalled = false;
            var setSuppressionGroupCalled = false;
            ShimgroupExplorer.AllInstances.DeleteGroupInt32 = (p, id) => { deleteGroupCalled = true; };
            ShimgroupExplorer.AllInstances.setSuppressionGroupInt32NullableOfInt32StringInt32 = (p, groupId, filterId, blast, cib) => { setSuppressionGroupCalled = true; };
            ShimgroupExplorer.AllInstances.setSelectedGroupInt32NullableOfInt32StringInt32 = (p, groupId, filterId, blast, cib) => { setSelectedGroupCalled = true; };
            var commandArg = new GridViewCommandEventArgs(null, new CommandEventArgs(command, "1"));

            // Act
            _privateGroupExplorerObj.Invoke(MethodGroupsGridCommand, new object[] { null, commandArg });

            // Assert
            setSuppressionGroupCalled.ShouldSatisfyAllConditions(
                () => deleteGroupCalled.ShouldBe(command == "DeleteGroup"),
                () => setSuppressionGroupCalled.ShouldBe(command == "SuppressGroup"),
                () => setSelectedGroupCalled.ShouldBe(command == "SelectGroup"));
        }

        [TestCase("", true)]
        [TestCase("refBlastList", true)]
        [TestCase("", false)]
        [TestCase("refBlastList", false)]
        public void SetSelectedGroup_DifferentRefBlast_GroupObjectInitialized(string refBlastList, bool groupIdExist)
        {
            // Arrange 
            InitTestSetSelectedGroup(10, addMatchedGroupId: groupIdExist);

            // Act
            _privateGroupExplorerObj.Invoke(MethodSetSelectedGroupMethod, new object[] { 100, FilterId, refBlastList, 100 });

            // Assert
            var groupList = _viewState["SelectedGroups_List"] as List<GroupObject>;
            var modifiedGroupListIndex = groupIdExist
                ? 0
                : 1;
            groupList.ShouldSatisfyAllConditions(
                () => groupList.ShouldNotBeNull(),
                () => groupList.Count.ShouldBe(modifiedGroupListIndex + 1),
                () => groupList[modifiedGroupListIndex].filters.ShouldNotBeNull(),
                () => groupList[modifiedGroupListIndex].filters.FirstOrDefault().ShouldNotBeNull(),
                () => groupList[modifiedGroupListIndex].filters[0].FilterID.ShouldBe(
                    string.IsNullOrWhiteSpace(refBlastList)
                    ? (int?)FilterId
                    : null),
                () => groupList[modifiedGroupListIndex].filters[0].RefBlastIDs.ShouldBe(
                    !string.IsNullOrWhiteSpace(refBlastList)
                    ? refBlastList
                    : null));
        }

        [TestCase("", true)]
        [TestCase("refBlastList", true)]
        [TestCase("", false)]
        [TestCase("refBlastList", false)]
        public void SetSelectedGroup_DifferentRefBlastNoSelectedGroups_GroupObjectInitialized(string refBlastList, bool groupIdExist)
        {
            // Arrange 
            InitTestSetSelectedGroup(10, addMatchedGroupId: groupIdExist);
            _viewState["SelectedGroups_List"] = null;

            // Act
            _privateGroupExplorerObj.Invoke(MethodSetSelectedGroupMethod, new object[] { 100, FilterId, refBlastList, 100 });

            // Assert
            var groupList = _viewState["SelectedGroups_List"] as List<GroupObject>;
            groupList.ShouldSatisfyAllConditions(
                () => groupList.ShouldNotBeNull(),
                () => groupList.Count.ShouldBe(1),
                () => groupList[0].filters.ShouldNotBeNull(),
                () => groupList[0].filters.FirstOrDefault().ShouldNotBeNull(),
                () => groupList[0].filters[0].FilterID.ShouldBe(
                    string.IsNullOrWhiteSpace(refBlastList)
                    ? (int?)FilterId
                    : null),
                () => groupList[0].filters[0].RefBlastIDs.ShouldBe(
                    !string.IsNullOrWhiteSpace(refBlastList)
                    ? refBlastList
                    : null));
        }

        [TestCase("", true)]
        [TestCase("refBlastList", true)]
        [TestCase("", false)]
        [TestCase("refBlastList", false)]
        public void SetSuppressionGroup_DifferentRefBlast_GroupObjectInitialized(string refBlastList, bool groupIdExist)
        {
            // Arrange 
            InitTestSetSuppressionGroup(10, addMatchedGroupId: groupIdExist);

            // Act
            _privateGroupExplorerObj.Invoke(MethodSetSuppressionGroup, new object[] { 100, FilterId, refBlastList, 100 });

            // Assert
            var groupList = _viewState["SupressionGroups_List"] as List<GroupObject>;
            var modifiedGroupListIndex = groupIdExist
                ? 0
                : 1;
            groupList.ShouldSatisfyAllConditions(
                () => groupList.ShouldNotBeNull(),
                () => groupList.Count.ShouldBe(modifiedGroupListIndex + 1),
                () => groupList[modifiedGroupListIndex].filters.ShouldNotBeNull(),
                () => groupList[modifiedGroupListIndex].filters.FirstOrDefault().ShouldNotBeNull(),
                () => groupList[modifiedGroupListIndex].filters[0].FilterID.ShouldBe(
                    string.IsNullOrWhiteSpace(refBlastList)
                    ? (int?)FilterId
                    : null),
                () => groupList[modifiedGroupListIndex].filters[0].RefBlastIDs.ShouldBe(
                    !string.IsNullOrWhiteSpace(refBlastList)
                    ? refBlastList
                    : null));
        }

        [TestCase("", true)]
        [TestCase("refBlastList", true)]
        [TestCase("", false)]
        [TestCase("refBlastList", false)]
        public void SetSuppressionGroup_DifferentRefBlastNoSelectedGroups_GroupObjectInitialized(string refBlastList, bool groupIdExist)
        {
            // Arrange 
            InitTestSetSuppressionGroup(10, addMatchedGroupId: groupIdExist);
            _viewState["SupressionGroups_List"] = null;

            // Act
            _privateGroupExplorerObj.Invoke(MethodSetSuppressionGroup, new object[] { 100, FilterId, refBlastList, 100 });

            // Assert
            var groupList = _viewState["SupressionGroups_List"] as List<GroupObject>;
            groupList.ShouldSatisfyAllConditions(
                () => groupList.ShouldNotBeNull(),
                () => groupList.Count.ShouldBe(1),
                () => groupList[0].filters.ShouldNotBeNull(),
                () => groupList[0].filters.FirstOrDefault().ShouldNotBeNull(),
                () => groupList[0].filters[0].FilterID.ShouldBe(
                    string.IsNullOrWhiteSpace(refBlastList)
                    ? (int?)FilterId
                    : null),
                () => groupList[0].filters[0].RefBlastIDs.ShouldBe(
                    !string.IsNullOrWhiteSpace(refBlastList)
                    ? refBlastList
                    : null));
        }

        [Test]
        public void BtnDownload_Click_ResponseWriteFile()
        {
            // Arrange
            InitTestBtnDownloadClick();
            var responseWriteFileCalled = false;
            var responseContentType = string.Empty;
            ShimHttpResponse.AllInstances.WriteFileString = (r, path) => { responseWriteFileCalled = true; };
            ShimHttpResponse.AllInstances.ContentTypeSetString = (r, contentType) => { responseContentType = contentType; };

            // Act
            _privateGroupExplorerObj.Invoke(MethodBtndownload_Click, new object[] { null, EventArgs.Empty });

            // Assert
            responseWriteFileCalled.ShouldSatisfyAllConditions(
               () => responseWriteFileCalled.ShouldBeTrue(),
               () => responseContentType.ShouldBe("application/xml"));
        }

        [Test]
        public void EnableSelectMode_SelectModeEnabled()
        {
            // Arrange
            InitTestEnableSelectMode(out ecnGridView groupGrid, out Panel pnlDownload, out DropDownList archiveFilterDropDown);

            // Act
            _groupExplorer.enableSelectMode();

            // Assert
            var isSelect = (bool)_privateGroupExplorerObj.GetField("IsSelect", BindingFlags.Static | BindingFlags.NonPublic);
            groupGrid.ShouldSatisfyAllConditions(
               () => isSelect.ShouldBeTrue(),
               () => groupGrid.Columns[2].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[3].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[4].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[5].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[6].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[7].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[8].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[9].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[10].Visible.ShouldBeTrue(),
               () => groupGrid.Columns[11].Visible.ShouldBeTrue(),
               () => pnlDownload.Visible.ShouldBeFalse(),
               () => archiveFilterDropDown.Enabled.ShouldBeFalse());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EnableEditMode_EditModeEnabled(bool userHasAccess)
        {
            // Arrange
            InitTestEnableEditMode(out ecnGridView groupGrid, out Panel pnlSelectGroup, out Button lnkbtnAddGroup, userHasAccess);

            // Act
            _groupExplorer.enableEditMode();

            // Assert
            var isSelect = (bool)_privateGroupExplorerObj.GetField("IsSelect", BindingFlags.Static | BindingFlags.NonPublic);
            groupGrid.ShouldSatisfyAllConditions(
                () => isSelect.ShouldBeFalse(),
               () => groupGrid.Columns[2].Visible.ShouldBeTrue(),
               () => groupGrid.Columns[3].Visible.ShouldBeTrue(),
               () => groupGrid.Columns[4].Visible.ShouldBeTrue(),
               () => groupGrid.Columns[5].Visible.ShouldBeTrue(),
               () => groupGrid.Columns[6].Visible.ShouldBeTrue(),
               () => groupGrid.Columns[7].Visible.ShouldBe(userHasAccess),
               () => groupGrid.Columns[8].Visible.ShouldBe(userHasAccess),
               () => groupGrid.Columns[9].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[10].Visible.ShouldBeFalse(),
               () => groupGrid.Columns[11].Visible.ShouldBeFalse(),
               () => pnlSelectGroup.Visible.ShouldBeFalse(),
               () => lnkbtnAddGroup.Visible.ShouldBeFalse());
        }

        [TestCase("select")]
        [TestCase("suppress")]
        public void ImgbtnCreateFilter_Click_FilterGridInitialized(string suppressOrSelectValue)
        {
            // Arrange 
            InitTestImgbtnCreateFilterClick(out filtergrid filterGrid, out HiddenField hfShowSelect, suppressOrSelectValue);
            var source = new ImageButton() { CommandArgument = "10" };

            //Act
            _privateGroupExplorerObj.Invoke(MethodImgbtnCreateFilterClick, new object[] { source, new ImageClickEventArgs(0, 0) });

            // Assert
            filterGrid.ShouldSatisfyAllConditions(
                () => filterGrid.SuppressOrSelect.ShouldBe(suppressOrSelectValue),
                () => hfShowSelect.Value.ShouldBe("1"));
        }

        private void InitCommonShim()
        {
            _viewState = Get<StateBag>(_privateGroupExplorerObj, "ViewState");
            ShimSqlConnection.AllInstances.Open = (conn) => { };
            ShimSqlConnection.AllInstances.Close = (conn) => { };
            ShimSqlCommand.AllInstances.ConnectionGet = (cmd) => new ShimSqlConnection();
            PlatformFake.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, code, access) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, code, access) => true;
            ShimAccessCheck.CanAccessByCustomerOf1M0User<List<CommunicatorEntites.Filter>>((filter, user) => true);
            ShimAccessCheck.CanAccessByCustomerOf1M0User<CommunicatorEntites.CampaignItem>((filter, user) => true);
            ShimGroup.ExistsInt32Int32 = (groupId, custId) => true;
            ShimFolderSystemBase.AllInstances.LoadFolderTree = (f) => { };
            ShimDirectory.ExistsString = (path) => false;
            ShimDirectory.CreateDirectoryString = (path) => { return null; };
            ShimFileStream.ConstructorStringFileModeFileAccess = (st, path, mode, access) => { };
            ShimFileStream.AllInstances.CanWriteGet = (st) => true;
            ShimStream.AllInstances.Close = (s) => { };
            ShimXmlTextWriter.AllInstances.Flush = (writer) => { };
            ShimXmlTextWriter.AllInstances.Close = (writer) => { };
            ShimPath.CombineStringString = (path1, path2) => string.Empty;
            ShimHttpResponse.AllInstances.WriteFileString = (r, path) => { };
            ShimHttpResponse.AllInstances.Flush = (r) => { };
            ShimHttpResponse.AllInstances.End = (r) => { };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            var updatePanel = Get<UpdatePanel>(_privateGroupExplorerObj, UpMainId);
            updatePanel.ID = "upId";
            updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
        }

        private void InitTestImgbtnCreateFilterClick(out filtergrid filterGrid, out HiddenField hfShowSelect, string suppressOrSelectValue)
        {
            InitCommonShim();
            filterGrid = new filtergrid();
            var findControlFilterGrid = filterGrid;
            hfShowSelect = Get<HiddenField>(_privateGroupExplorerObj, HfShowSelectId);
            Shimfilters.AllInstances.loadData = (p) => { };
            ShimgroupExplorer.AllInstances.SuppressOrSelectGet = (p) => suppressOrSelectValue;
            ShimGridViewRowCollection.AllInstances.ItemGetInt32 = (col, id) => new ShimGridViewRow();
            ShimControl.AllInstances.FindControlString = (control, id) =>
            {
                if (id == "lblGroupID")
                {
                    return new Label() { Text = "10" };
                }
                else if (id == "ucFilterGrid" || id == "fgSuppressionFilterGrid")
                {
                    return findControlFilterGrid;
                }
                return null;
            };
        }

        private void InitTestEnableEditMode(out ecnGridView groupGrid, out Panel pnlSelectGroup, out Button lnkbtnAddGroup, bool userHasAccess = true)
        {
            InitCommonShim();
            ShimCustomer.GetByCustomerIDInt32Boolean = (custId, getChild) => new ECN_Framework_Entities.Accounts.Customer();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (platform, serviceCode, servicefeatureCode) => true;
            groupGrid = Get<ecnGridView>(_privateGroupExplorerObj, GvGroupsGridId);
            for (var i = 0; i < 12; i++)
            {
                groupGrid.Columns.Add(new TemplateField());
            }
            pnlSelectGroup = Get<Panel>(_privateGroupExplorerObj, PnlSelectedGroupId);
            lnkbtnAddGroup = Get<Button>(_privateGroupExplorerObj, LnkbtnAddGroupId);
            PlatformFake.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, code, access) => userHasAccess;
            ShimUser.IsSystemAdministratorUser = (user) => true;
        }

        private void InitTestEnableSelectMode(out ecnGridView groupGrid, out Panel pnlDownload, out DropDownList archiveFilterDropDown)
        {
            InitCommonShim();
            ShimCustomer.GetByCustomerIDInt32Boolean = (custId, getChild) => new ECN_Framework_Entities.Accounts.Customer();
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (platform, serviceCode, servicefeatureCode) => true;
            groupGrid = Get<ecnGridView>(_privateGroupExplorerObj, GvGroupsGridId);
            for (var i = 0; i < 12; i++)
            {
                groupGrid.Columns.Add(new TemplateField());
            }
            pnlDownload = Get<Panel>(_privateGroupExplorerObj, PnlDownloadId);
            archiveFilterDropDown = Get<DropDownList>(_privateGroupExplorerObj, DDLArchiveFilterId);
        }

        private void InitTestPageLoad()
        {
            InitCommonShim();
            _groupDeleted = false;
            ShimSqlCommand.AllInstances.ExecuteNonQuery = (cmd) =>
            {
                if (cmd.CommandText == "e_Group_Delete")
                {
                    _groupDeleted = true;
                }
                return 1;
            };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, conn) =>
            {
                if (cmd.CommandText == "e_CampaignItemBlastFilter_Select_FilterID_CanDelete")
                {
                    return 1;
                }
                return 0;
            };
            CommFakeDataLayer.ShimMarketingAutomation.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.MarketingAutomation>();
            ShimGroup.GetByGroupIDInt32User = (id, user) => null;
            CommFakeDataLayer.ShimFilter.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.Filter>() { new CommunicatorEntites.Filter() };
            ShimFilterGroup.GetByFilterIDInt32User = (id, user) => new List<CommunicatorEntites.FilterGroup>() { new CommunicatorEntites.FilterGroup() };
            ShimFilter.ExistsInt32Int32 = (id, custId) => true;
            ShimFilter.GetByFilterIDInt32User = (id, user) => null;
            ShimFilterCondition.GetByFilterGroupIDInt32User = (id, user) => null;
            ShimFilterCondition.ReSortInt32User = (id, user) => { };
            ShimEmailGroup.GetByGroupIDInt32User = (id, user) => null;
            KM.Common.Fakes.ShimDataFunctions.GetSqlConnectionString = (connString) => new ShimSqlConnection();
            KM.Common.Fakes.ShimDataFunctions.ConfigureAndOpenCommandSqlCommand = (cmd) => { };
            CommFakeDataLayer.ShimFilter.DeleteInt32Int32Int32 = (filterId, custId, userId) => { };
            CommFakeDataLayer.ShimEmailDataValues.DeleteInt32Int32Int32 = (groupId, custId, userId) => { };
            CommFakeDataLayer.ShimEmailDataValues.DeleteInt32Int32Int32Int32 = (groupId, fieldId, custId, userId) => { };
            CommFakeDataLayer.ShimEmailGroup.DeleteInt32Int32 = (grpId, userId) => { };
            var selectedGroupGrid = Get<ecnGridView>(_privateGroupExplorerObj, GvSelectedGroupsPropertyName);
            for (var i = 0; i < 8; i++)
            {
                selectedGroupGrid.Columns.Add(new TemplateField());
            }
            var groupsGrid = Get<ecnGridView>(_privateGroupExplorerObj, GvGroupsGridId);
            for (var i = 0; i < 10; i++)
            {
                groupsGrid.Columns.Add(new TemplateField());
            }
            var gvSupressionGird = Get<ecnGridView>(_privateGroupExplorerObj, GvSupressionPropertyName);
            for (var i = 0; i < 8; i++)
            {
                gvSupressionGird.Columns.Add(new TemplateField());
            }
        }

        private void InitTestReset(string searchCriteria, string searchType)
        {
            InitCommonShim();
            var groupList = new List<GroupObject>()
            {
                new GroupObject()
                {
                     GroupID = 1,
                     filters = new List<CommunicatorEntites.CampaignItemBlastFilter>()
                     {
                         new CommunicatorEntites.CampaignItemBlastFilter()
                         {
                            FilterID =10,
                            SmartSegmentID = 10,
                            RefBlastIDs  = "RefBlastIDs"
                         }
                     }
                }
            };
            ShimgroupExplorer.AllInstances.getSelectedGroups = (p) => groupList;
            ShimgroupExplorer.AllInstances.getSuppressionGroups = (p) => groupList;
            ShimgroupExplorer.AllInstances.LoadGroupFolder = (p) => { };
            ShimgroupExplorer.AllInstances.loadLicense = (p) => { };
            _privateGroupExplorerObj.SetField("IsSelect", BindingFlags.Static | BindingFlags.NonPublic, true);
            CommFakeDataLayer.ShimCampaignItem.GetSqlCommand = (command) => new CommunicatorEntites.CampaignItem();
            var searchCriteriaText = Get<TextBox>(_privateGroupExplorerObj, SearchCriteriaId);
            searchCriteriaText.Text = searchCriteria;
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, connString) =>
            {
                if (cmd.CommandText == "v_Group_Get"
                       || cmd.CommandText == "v_Group_Get_GroupName"
                       || cmd.CommandText == "v_Group_Get_ProfileName"
                       || cmd.CommandText == "v_Blast_GetEstimatedSendsCount")
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add(new DataColumn());
                    dataTable.Columns.Add(new DataColumn());
                    var row = dataTable.NewRow();
                    row[0] = Blast;
                    row[1] = TotalRecord;
                    dataTable.Rows.Add(row);
                    return dataTable;
                }
                return null;
            };
            var searchGrpsDR = Get<DropDownList>(_privateGroupExplorerObj, SearchGrpsDRId);
            searchGrpsDR.Items.Add(new ListItem("searchCriteria", "searchCriteria"));
            searchGrpsDR.SelectedValue = "searchCriteria";
            var searchTypeDR = Get<DropDownList>(_privateGroupExplorerObj, SearchTypeDRId);
            searchTypeDR.Items.Add(new ListItem("Group", "Group"));
            searchTypeDR.Items.Add(new ListItem("Profile", "Profile"));
            searchTypeDR.SelectedValue = searchType;
            ShimCustomer.GetByCustomerIDInt32Boolean = (id, getChild) => new ECN_Framework_Entities.Accounts.Customer() { BaseChannelID = 1 };
            var groupsGrid = Get<ecnGridView>(_privateGroupExplorerObj, GvGroupsGridId);
            for (var i = 0; i < 10; i++)
            {
                groupsGrid.Columns.Add(new TemplateField());
            }
        }

        private void InitTestBtnSaveFilter(string suppressOrSelect, int groupId, string selectedFilterType, bool selectBlastId = true, bool addMatchedGroupId = true)
        {
            InitCommonShim();
            ShimgroupExplorer.AllInstances.SuppressOrSelectGet = (p) => suppressOrSelect;
            ShimgroupExplorer.AllInstances.blastLicenseCount_Update = (p) => { };
            ShimgroupExplorer.AllInstances.DataBindSelected = (p) => { };
            ShimgroupExplorer.AllInstances.DataBindSuppression = (p) => { };
            _privateGroupExplorerObj.SetFieldOrProperty("_GroupID", BindingFlags.Static | BindingFlags.NonPublic, groupId);
            var rbFilterType = Get<RadioButtonList>(_privateGroupExplorerObj, RBlFilterTypeId);
            rbFilterType.Items.Add(new ListItem("smart", "smart"));
            rbFilterType.Items.Add(new ListItem("custom", "custom"));
            rbFilterType.SelectedValue = selectedFilterType;
            var ddlSmartSegment = Get<DropDownList>(_privateGroupExplorerObj, DDLSmartSegmentId);
            ddlSmartSegment.Items.Add(new ListItem("1000", "1000") { Selected = true });
            var lstBlast = Get<ListBox>(_privateGroupExplorerObj, LSTboxBlastId);
            lstBlast.Items.Add(new ListItem("1", "1") { Selected = selectBlastId });
            lstBlast.Items.Add(new ListItem("2", "2") { Selected = selectBlastId });
            var lstAvailableFilters = Get<ListBox>(_privateGroupExplorerObj, LSTboxAvailableFiltersId);
            lstAvailableFilters.Items.Add(new ListItem("1", "1") { Selected = true });
            lstAvailableFilters.Items.Add(new ListItem("2", "2") { Selected = true });
            var groupList = new List<GroupObject>()
            {
                new GroupObject()
                {
                     GroupID = addMatchedGroupId ?  groupId : 333,
                     filters = new List<CommunicatorEntites.CampaignItemBlastFilter>()
                     {
                         new CommunicatorEntites.CampaignItemBlastFilter()
                         {
                            FilterID =10,
                            SmartSegmentID = 10,
                            RefBlastIDs  = "RefBlastIDs"
                         }
                     }
                },
                 new GroupObject()
                {
                     GroupID = 555,
                     filters = new List<CommunicatorEntites.CampaignItemBlastFilter>()
                     {
                         new CommunicatorEntites.CampaignItemBlastFilter()
                         {
                              FilterID =20,
                               SmartSegmentID = 20,
                               RefBlastIDs  = "RefBlastIDs"
                             }
                     }
                }
            };
            _viewState.Add("SelectedGroups_List", groupList);
            _viewState.Add("SupressionGroups_List", groupList);
            _viewState.Add("SelectedTestGroups_List", groupList);
        }

        private void InitTestGroupsGridCommand()
        {
            InitCommonShim();
            ShimgroupExplorer.AllInstances.loadGroupsGridInt32 = (p, id) => { };
            ShimgroupExplorer.AllInstances.blastLicenseCount_Update = (p) => { };
        }

        private void InitTestSetSelectedGroup(int groupId, bool addMatchedGroupId = true)
        {
            InitCommonShim();
            ShimgroupExplorer.AllInstances.DataBindSelected = (p) => { };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new CommunicatorEntites.Group()
            {
                GroupID = groupId,
                GroupName = "groupName"
            };
            var groupList = new List<GroupObject>();
            _viewState.Add("SelectedGroups_List", groupList);
            groupList.Add(new GroupObject()
            {
                GroupID = addMatchedGroupId
                ? groupId
                : groupId + 10,
                filters = new List<CommunicatorEntites.CampaignItemBlastFilter>()
            });
        }

        private void InitTestSetSuppressionGroup(int groupId, bool addMatchedGroupId = true)
        {
            InitCommonShim();
            ShimgroupExplorer.AllInstances.DataBindSuppression = (p) => { };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new CommunicatorEntites.Group()
            {
                GroupID = groupId,
                GroupName = "groupName"
            };

            var groupList = new List<GroupObject>();
            _viewState.Add("SupressionGroups_List", groupList);
            groupList.Add(new GroupObject()
            {
                GroupID = addMatchedGroupId
                ? groupId
                : groupId + 10,
                filters = new List<CommunicatorEntites.CampaignItemBlastFilter>()
            });
        }

        private void InitTestBtnDownloadClick()
        {
            InitCommonShim();
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, connString) =>
                {
                    if (cmd.CommandText == "v_EmailGroup_Get_UserID")
                    {
                        var dataTable = new DataTable();
                        dataTable.Columns.Add(new DataColumn("FolderID"));
                        dataTable.Columns.Add(new DataColumn("FolderName"));
                        dataTable.Columns.Add(new DataColumn("GroupName"));
                        dataTable.Columns.Add(new DataColumn("Subscribers"));
                        var row = dataTable.NewRow();
                        dataTable.Rows.Add(row);
                        row[0] = "FolderID";
                        row[1] = "FolderName";
                        row[2] = "GroupName";
                        row[3] = "Subscribers";
                        return dataTable;
                    }
                    return null;
                };
        }
    }
}
