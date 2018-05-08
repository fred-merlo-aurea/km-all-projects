using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient.Fakes;
using System.Linq;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using AjaxControlToolkit.Fakes;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.controls;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using NUnit.Framework;
using Shouldly;
using CommFakeDataLayer = ECN_Framework_DataLayer.Communicator.Fakes;
using CommunicatorEntites = ECN_Framework_Entities.Communicator;
using PlatformFake = KM.Platform.Fakes;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    public partial class TestGroupExplorerTest
    {
        private const string MethodGroupsGridRowDeleting = "GroupsGrid_RowDeleting";
        private const string MethodbtnSaveFilterClick = "btnSaveFilter_Click";
        private const string MethodImgbtnCreateFilterClick = "imgbtnCreateFilter_Click";
        private const string MethodSetSelectedGroup = "setSelectedGroup";
        private const string MethodGoToPageGroupTextChanged = "GoToPageGroup_TextChanged";
        private const string HfShowSelectId = "hfShowSelect";
        private const string LSTboxBlastId = "lstboxBlast";
        private const string SearchCriteriaId = "SearchCriteria";
        private const string SearchGrpsDRId = "SearchGrpsDR";
        private const string SearchTypeDRId = "SearchTypeDR";
        private const string UpMainId = "upMain";
        private const string LblTotalRecordsId = "lblTotalRecords";
        private const string PnlPagerId = "pnlPager";
        private const string GroupsGridId = "GroupsGrid";
        private const string DDLSmartSegmentId = "ddlSmartSegment";
        private const string LblRefBlastErrorId = "lblRefBlastError";
        private const string LSTboxAvailableFiltersId = "lbAvailableFilters";
        private const string RBlFilterTypeId = "rblFilterType";
        private const string TotalRecord = "20";
        private const int FilterId = 100;
        private StateBag _viewState;
        private bool _groupDeleted;

        [Test]
        public void GroupsGrid_RowDeleting_GroupDeleted()
        {
            // Arrange
            InitTestGroupsGridRowDeleting();
            var arg = new GridViewDeleteEventArgs(0);

            // Act
            _privateGroupExplorerObj.Invoke(MethodGroupsGridRowDeleting, new object[] { null, arg });

            // Assert
            _groupDeleted.ShouldBeTrue();
        }

        [Test]
        public void BtnSaveFilter_Click_NoRefBlast_Error()
        {
            // Arrange
            InitTestBtnSaveFilter("testselect", 100, "smart", false);
            var lblBlastError = Get<Label>(_privateGroupExplorerObj, LblRefBlastErrorId);
            lblBlastError.Visible = false;

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
            InitTestBtnSaveFilter("testselect", 100, "smart", addMatchedGroupId: false);

            // Act
            _privateGroupExplorerObj.Invoke(MethodbtnSaveFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            popupHide.ShouldSatisfyAllConditions(
                () => popupHide.ShouldBeTrue());
        }

        [TestCase("testselect", "smart")]
        [TestCase("testselect", "custom")]
        public void BtnSaveFilter_Click_MatchedGroupId_BindMethodCalled(string suppressOrSelect, string filterType)
        {
            // Arrange
            var dataBindSelectCalled = false;
            InitTestBtnSaveFilter(suppressOrSelect, 100, filterType);
            ShimtestGroupExplorer.AllInstances.DataBindSelected = (p) => { dataBindSelectCalled = true; };
            var expectedRefBlastIDs = filterType == "smart"
                ? "1,2"
                : null;

            // Act
            _privateGroupExplorerObj.Invoke(MethodbtnSaveFilterClick, new object[] { null, EventArgs.Empty });

            // Assert
            var groupObjects = _viewState["SelectedTestGroups_List"] as List<GroupObject>;
            groupObjects.ShouldSatisfyAllConditions(
                () => dataBindSelectCalled.ShouldBeTrue(),
                () => groupObjects.ShouldNotBeNull(),
                () => groupObjects.Count.ShouldNotBe(0),
                () => groupObjects[0].filters.ShouldNotBeNull(),
                () => groupObjects[0].filters.Count.ShouldBeGreaterThan(1),
                () => groupObjects[0].filters.Last().RefBlastIDs.ShouldBe(expectedRefBlastIDs));
        }

        [Test]
        public void ImgbtnCreateFilter_Click_FilterGridInitialized()
        {
            // Arrange 
            InitTestImgbtnCreateFilterClick(out filtergrid filterGrid, out HiddenField hfShowSelect);
            var source = new ImageButton() { CommandArgument = "10" };

            //Act
            _privateGroupExplorerObj.Invoke(MethodImgbtnCreateFilterClick, new object[] { source, new ImageClickEventArgs(0, 0) });

            // Assert
            filterGrid.ShouldSatisfyAllConditions(
                () => filterGrid.SuppressOrSelect.ShouldBe("testselect"),
                () => hfShowSelect.Value.ShouldBe("1"));
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
            _privateGroupExplorerObj.Invoke(MethodSetSelectedGroup, new object[] { 100, FilterId, refBlastList });

            // Assert
            var groupList = _viewState["SelectedTestGroups_List"] as List<GroupObject>;
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
            _viewState["SelectedTestGroups_List"] = null;

            // Act
            _privateGroupExplorerObj.Invoke(MethodSetSelectedGroup, new object[] { 100, FilterId, refBlastList });

            // Assert
            var groupList = _viewState["SelectedTestGroups_List"] as List<GroupObject>;
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

        [TestCase(-1, "", "Group")]
        [TestCase(10, "searchCriteria", "Profile")]
        [TestCase(10, "searchCriteria", "Group")]
        public void GoToPageGroup_TextChanged_PageChangedAndGridReloaded(int goToPageText, string searchCriteria, string searchType)
        {
            //Arrange
            var gridDataBound = false;
            ShimGridView.AllInstances.DataBind = (grid) => gridDataBound = true;
            var gotoPageTextBox = new TextBox() { Text = goToPageText.ToString() };
            ShimGridView.AllInstances.PageCountGet = (g) => 20;
            InitTestGoToPageGroupTextChanged(searchCriteria, searchType, out ecnGridView groupsGrid);
            var expectedPageIndex = goToPageText > 0
                ? goToPageText - 1
                : 0;
            var lblTotalRecords = Get<Label>(_privateGroupExplorerObj, LblTotalRecordsId);
            var pnlPager = Get<Panel>(_privateGroupExplorerObj, PnlPagerId);
            pnlPager.Visible = false;

            // Act
            _privateGroupExplorerObj.Invoke(MethodGoToPageGroupTextChanged, new object[] { gotoPageTextBox, EventArgs.Empty });

            // Assert
            groupsGrid.ShouldSatisfyAllConditions(
                  () => groupsGrid.PageIndex.ShouldBe(expectedPageIndex),
                  () => gridDataBound.ShouldBeTrue(),
                  () => lblTotalRecords.Text.ShouldBe(TotalRecord),
                  () => pnlPager.Visible.ShouldBeTrue());
        }

        private void InitTestGoToPageGroupTextChanged(string searchCriteria, string searchType, out ecnGridView groupsGrid)
        {
            InitCommonShimForTestGroupExplorer();
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
                    dataTable.Columns.Add(new DataColumn("GroupID"));
                    dataTable.Columns.Add(new DataColumn("TotalRecord"));
                    dataTable.Columns.Add(new DataColumn("GroupName"));
                    dataTable.Columns.Add(new DataColumn("Subscribers"));
                    var row = dataTable.NewRow();
                    row[1] = TotalRecord;
                    row["GroupID"] = "100";
                    row["GroupName"] = "GroupName";
                    row["Subscribers"] = "100";
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
            groupsGrid = Get<ecnGridView>(_privateGroupExplorerObj, GroupsGridId);
            for (var i = 0; i < 10; i++)
            {
                groupsGrid.Columns.Add(new TemplateField());
            }
        }

        private void InitTestSetSelectedGroup(int groupId, bool addMatchedGroupId = true)
        {
            InitCommonShimForTestGroupExplorer();
            ShimgroupExplorer.AllInstances.DataBindSelected = (p) => { };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new CommunicatorEntites.Group()
            {
                GroupID = groupId,
                GroupName = "groupName"
            };
            var groupList = new List<GroupObject>();
            _viewState.Add("SelectedTestGroups_List", groupList);
            groupList.Add(new GroupObject()
            {
                GroupID = addMatchedGroupId
                ? groupId
                : groupId + 10,
                filters = new List<CommunicatorEntites.CampaignItemBlastFilter>()
            });
        }

        private void InitCommonShimForTestGroupExplorer()
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
            ShimHttpResponse.AllInstances.WriteFileString = (r, path) => { };
            ShimHttpResponse.AllInstances.Flush = (r) => { };
            ShimHttpResponse.AllInstances.End = (r) => { };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            var updatePanel = Get<UpdatePanel>(_privateGroupExplorerObj, UpMainId);
            updatePanel.ID = "upId";
            updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
        }

        private void InitTestGroupsGridRowDeleting()
        {
            InitCommonShimForTestGroupExplorer();
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
            var groupsGrid = Get<ecnGridView>(_privateGroupExplorerObj, GroupsGridId);
            DataTable groupsGridDataTable = new DataTable();
            groupsGridDataTable.Columns.Add(new DataColumn("key"));
            var row = groupsGridDataTable.NewRow();
            row[0] = 10;
            groupsGridDataTable.Rows.Add(row);
            groupsGrid.DataSource = groupsGridDataTable;
            groupsGrid.DataKeyNames = new string[] { "key" };
            groupsGrid.DataBind();
        }

        private void InitTestBtnSaveFilter(string suppressOrSelect, int groupId, string selectedFilterType, bool selectBlastId = true, bool addMatchedGroupId = true)
        {
            InitCommonShimForTestGroupExplorer();
            ShimtestGroupExplorer.AllInstances.SuppressOrSelectGet = (p) => suppressOrSelect;
            ShimtestGroupExplorer.AllInstances.blastLicenseCount_Update = (p) => { };
            ShimtestGroupExplorer.AllInstances.DataBindSelected = (p) => { };
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
            _viewState.Add("SelectedTestGroups_List", groupList);
        }

        private void InitTestImgbtnCreateFilterClick(out filtergrid filterGrid, out HiddenField hfShowSelect)
        {
            InitCommonShimForTestGroupExplorer();
            filterGrid = new filtergrid();
            var findControlFilterGrid = filterGrid;
            hfShowSelect = Get<HiddenField>(_privateGroupExplorerObj, HfShowSelectId);
            Shimfilters.AllInstances.loadData = (p) => { };
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
    }
}
