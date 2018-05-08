using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Group;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.controls;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicatorEntites = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class TestGroupExplorerTest : PageHelper
    {
        private const string GvSelectedGroupsCommandMethodName = "gvSelectedGroups_Command";
        private const string EditCustomFilterCommand = "EditCustomFilter";
        private const string AddFilterCommand = "AddFilter";
        private const string CreatecustomfilterCommand = "createcustomfilter";
        private const string DeletesfilterCommand = "deletessfilter";
        private const string Deletecustomfilter = "deletecustomfilter";
        private const string RemoveGroupCommand = "RemoveGroup";
        private const string ImportsubsCommand = "importsubs";
        private const string AddsubsCommand = "addsubs";
        private const string GvSelectedGroupsPropertyName = "gvSelectedGroups";
        private const string LabelGroupID = "lblGroupID";
        private const string FilterGridID = "ucFilterGrid";
        private const string SampleGroup = "SampleGroup";
        private const string ViewStatePropertyName = "ViewState";
        private const string SelectedTestGroupsKey = "SelectedTestGroups_List";
        private const string FilterEditControl = "filterEdit1";
        private const string ShowSelectControl = "hfShowSelect";
        private const string BtnFilterEditClose = "btnFilterEdit_Close";
        private const string ImgbtnCreateFilter = "imgbtnCreateFilter";
        private const string DdlSmartSegment = "ddlSmartSegment";
        private const string ListboxBlast = "lstboxBlast";
        private const string EmptyGridLabel = "lblEmptyGrid_Selected";
        private const string SelectedGroupIDPropertyName = "SelectedGroupID";
        private const string ImportSubscribers1 = "importSubscribers1";
        private const string AddSubscribers1 = "addSubscribers1";
        private bool _isPnlFilterConfigUpdated;
        private bool _isFilterDataLoaded;
        private bool _isNewGroupDropdownLoaded;
        private const string GroupIDKey = "GroupID";
        private const string FilterIDKey = "FilterID";
        private const string TestUserName = "TestUser";
        private const string TestUrl = "http://km.com";
        private testGroupExplorer _testGroupExplorer;
        private PrivateObject _privateGroupExplorerObj;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testGroupExplorer = new testGroupExplorer();
            InitializeAllControls(_testGroupExplorer);
            _privateGroupExplorerObj = new PrivateObject(_testGroupExplorer);
            InitializeSessionFakes();
            QueryString.Clear();
            QueryString.Add(GroupIDKey, null);
            QueryString.Add(FilterIDKey, null);

            ShimUserControl.AllInstances.RequestGet = (p) =>
            {
                return new HttpRequest(string.Empty, TestUrl, string.Empty);
            };
            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return QueryString;
            };
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1 };
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUserName, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        [Test]
        public void GvSelectedGroups_Command_WhenEditFilterCommand_SetPageControlValues()
        {
            // Arrange
            const string filterId = "7";
            var commandArgs = GetViewCommandEventArgs(EditCustomFilterCommand, filterId);
            SetFakesForGvSelectedGroupCommandMethod();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);
            var editFilter = Get<filters>(_privateGroupExplorerObj, FilterEditControl);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => editFilter.selectedFilterID.ShouldBe(7),
                () => editFilter.selectedGroupID.ShouldBe(13),
                () => Get<HiddenField>(_privateGroupExplorerObj, ShowSelectControl).Value.ShouldBe("0"));
        }

        [Test]
        public void GvSelectedGroups_Command_WhenAddFilterCommand_SetPageControlValues()
        {
            // Arrange
            var filterId = "0";
            var commandArgs = GetViewCommandEventArgs(AddFilterCommand, filterId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);
            var closeButton = Get<Button>(_privateGroupExplorerObj, BtnFilterEditClose);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => Get<ImageButton>(_privateGroupExplorerObj, ImgbtnCreateFilter).CommandArgument.ShouldBe(filterId),
                () => closeButton.CommandArgument.ShouldBe(filterId),
                () => closeButton.CommandName.ShouldBe("testselect"),
                () => Get<HiddenField>(_privateGroupExplorerObj, ShowSelectControl).Value.ShouldBe("1"),
                () => _testGroupExplorer.SuppressOrSelect.ShouldBe("testselect"),
                () => _isPnlFilterConfigUpdated.ShouldBeTrue());
        }

        [Test]
        public void GvSelectedGroups_Command_WhenCreatecustomfilterCommand_SetPageControlValues()
        {
            // Arrange
            var filterId = "0";
            var commandArgs = GetViewCommandEventArgs(CreatecustomfilterCommand, filterId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);
            var editFilter = Get<filters>(_privateGroupExplorerObj, FilterEditControl);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => editFilter.selectedGroupID.ShouldBe(1),
                () => editFilter.selectedFilterID.ShouldBe(0),
                () => Get<HiddenField>(_privateGroupExplorerObj, ShowSelectControl).Value.ShouldBe("1"),
                () => _isFilterDataLoaded.ShouldBeTrue());
        }

        [Test]
        public void GvSelectedGroups_Command_WhenDeletesfilterCommand_SetPageControlValues()
        {
            // Arrange
            var filterId = "0_1";
            var commandArgs = GetViewCommandEventArgs(DeletesfilterCommand, filterId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);
            var stateBag = Get<StateBag>(_privateGroupExplorerObj, ViewStatePropertyName);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSelectedGroupsPropertyName);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.DataSource.ShouldBeOfType(typeof(List<GroupObject>)),
                () => grid.Rows.Count.ShouldBe(1),
                () => stateBag.Values.Count.ShouldBe(1),
                () => stateBag[SelectedTestGroupsKey].ShouldBeOfType(typeof(List<GroupObject>)),
                () => (stateBag[SelectedTestGroupsKey] as List<GroupObject>)?.First().GroupID.ShouldBe(1),
                () => _isFilterDataLoaded.ShouldBeFalse());
        }

        [Test]
        public void GvSelectedGroups_Command_WhenDeletecustomfilterCommand_SetPageControlValues()
        {
            // Arrange
            var filterId = "0_1";
            var commandArgs = GetViewCommandEventArgs(Deletecustomfilter, filterId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);
            var stateBag = Get<StateBag>(_privateGroupExplorerObj, ViewStatePropertyName);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSelectedGroupsPropertyName);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.DataSource.ShouldBeOfType(typeof(List<GroupObject>)),
                () => grid.Rows.Count.ShouldBe(1),
                () => stateBag.Values.Count.ShouldBe(1),
                () => stateBag[SelectedTestGroupsKey].ShouldBeOfType(typeof(List<GroupObject>)),
                () => (stateBag[SelectedTestGroupsKey] as List<GroupObject>)?.First().GroupID.ShouldBe(1),
                () => _isFilterDataLoaded.ShouldBeFalse());
        }

        [Test]
        public void GvSelectedGroups_Command_WhenRemovedGroupCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = "1";
            var commandArgs = GetViewCommandEventArgs(RemoveGroupCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);
            var stateBag = Get<StateBag>(_privateGroupExplorerObj, ViewStatePropertyName);
            var grid = Get<ecnGridView>(_privateGroupExplorerObj, GvSelectedGroupsPropertyName);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => grid.Visible.ShouldBeFalse(),
                () => Get<Label>(_privateGroupExplorerObj, EmptyGridLabel).Visible.ShouldBeTrue(),
                () => grid.DataSource.ShouldBeOfType(typeof(List<GroupObject>)),
                () => grid.Rows.Count.ShouldBe(0),
                () => stateBag.Values.Count.ShouldBe(1),
                () => stateBag[SelectedTestGroupsKey].ShouldBeOfType(typeof(List<GroupObject>)),
                () => (stateBag[SelectedTestGroupsKey] as List<GroupObject>).ShouldBeEmpty(),
                () => _isFilterDataLoaded.ShouldBeFalse());
        }

        [Test]
        public void GvSelectedGroups_Command_WhenImportsubsCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = "1";
            var commandArgs = GetViewCommandEventArgs(ImportsubsCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => Get<HiddenField>(_privateGroupExplorerObj, SelectedGroupIDPropertyName).Value.ShouldBe(groupId),
                () => Get<newGroup_Import>(_privateGroupExplorerObj, ImportSubscribers1).GroupID.ShouldBe(1));
        }

        [Test]
        public void GvSelectedGroups_Command_WhenAddsubsCommand_SetPageControlValues()
        {
            // Arrange
            var groupId = "1";
            var commandArgs = GetViewCommandEventArgs(AddsubsCommand, groupId);
            SetFakesForGvSelectedGroupCommandMethod();
            SetGvSelectedGroupsGrid();

            // Act
            _privateGroupExplorerObj.Invoke(GvSelectedGroupsCommandMethodName, this, commandArgs);

            // Assert
            _privateGroupExplorerObj.ShouldSatisfyAllConditions(
                () => Get<HiddenField>(_privateGroupExplorerObj, SelectedGroupIDPropertyName).Value.ShouldBe(groupId),
                () => Get<newGroup_add>(_privateGroupExplorerObj, AddSubscribers1).GroupID.ShouldBe(1),
                () => _isNewGroupDropdownLoaded.ShouldBeTrue());
        }

        private void SetFakesForGvSelectedGroupCommandMethod()
        {
            _isPnlFilterConfigUpdated = false;
            _isFilterDataLoaded = false;
            _isNewGroupDropdownLoaded = false;
            var viewState = (StateBag)_privateGroupExplorerObj.GetProperty(ViewStatePropertyName);
            viewState.Add(SelectedTestGroupsKey, GetGroupList());

            ShimFilter.GetByFilterIDInt32User = (fid, user) => new CommunicatorEntites.Filter
            {
                GroupID = 1,
            };
            ShimBlast.GetBySearchInt32StringNullableOfInt32NullableOfInt32NullableOfBooleanStringNullableOfDateTimeNullableOfDateTimeNullableOfInt32StringStringUserBoolean =
                (q, w, e, r, t, y, u, c, g, h, j, d, i) => new List<CommunicatorEntites.BlastAbstract>
                {
                    new CommunicatorEntites.BlastChampion{ }
                };
            KMPlatform.BusinessLogic.Fakes.ShimUser.
                HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, b, h, g) => true;
            ShimFilter.GetByGroupID_NoAccessCheckInt32BooleanString = (id, b, s) => new List<CommunicatorEntites.Filter>
            {
                new CommunicatorEntites.Filter()
            };
            ShimSmartSegment.GetSmartSegments = () => new List<CommunicatorEntites.SmartSegment>
            {
                new CommunicatorEntites.SmartSegment()
            };
            Shimfilters.AllInstances.loadData = (f) => { _isFilterDataLoaded = true; };
            ShimUpdatePanel.AllInstances.Update = (u) => { _isPnlFilterConfigUpdated = true; };
            ShimnewGroup_add.AllInstances.loadDropDownsInt32 = (g, i) => { _isNewGroupDropdownLoaded = true; };
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (x) => ReflectionHelper.CreateInstance(typeof(Filter));
        }

        private GridViewCommandEventArgs GetViewCommandEventArgs(string commandName, string filterId)
        {
            var argument = $"{filterId}";
            return new GridViewCommandEventArgs(null, new CommandEventArgs(commandName, argument));
        }

        private void SetGvSelectedGroupsGrid()
        {
            var gvSelectedGroups = (ecnGridView)_privateGroupExplorerObj.GetFieldOrProperty(GvSelectedGroupsPropertyName);
            gvSelectedGroups.DataSource = new List<GroupObject>
            {
                new GroupObject { GroupID = 1, GroupName = SampleGroup }
            };
            gvSelectedGroups.DataBind();
            var tableCell = new TableCell();
            tableCell.Controls.Add(new Label { ID = LabelGroupID, Text = "1" });
            var filterstableCell = new TableCell();
            filterstableCell.Controls.Add(new filtergrid() { ID = FilterGridID });
            gvSelectedGroups.Rows[0].Controls.Add(tableCell);
            gvSelectedGroups.Rows[0].Controls.Add(filterstableCell);
        }

        private List<GroupObject> GetGroupList()
        {
            return new List<GroupObject>
            {
                new GroupObject
                {
                    GroupID = 1,
                    filters = new List<CommunicatorEntites.CampaignItemBlastFilter>
                    {
                        new CommunicatorEntites.CampaignItemBlastFilter { SmartSegmentID = 0 }
                    }
                }
            };
        }
    }
}