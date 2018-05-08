using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Group;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Collector.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BussinessCommFakes = ECN_Framework_BusinessLayer.Communicator.Fakes;
using CommunicatorEntites = ECN_Framework_Entities.Communicator;
using DataLayerCommFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using KMPlatformFakes = KMPlatform.BusinessLogic.Fakes;

namespace ECN.Communicator.Tests.Main.ECNWizard.Group
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.ECNWizard.Group.addGroup"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class AddGroupTest : PageHelper
    {
        private const int DropFolderValue = 10;
        private const string TestUrl = "http://km.com";
        private const string TestUserName = "TestUser";
        private const string PublicFolderId = "PublicFolder";
        private const string PhErrorId = "phError";
        private const string GroupNameId = "GroupName";
        private const string GroupDescriptionId = "GroupDescription";
        private const string DrpFolderId = "drpFolder";
        private const string RbSeedListId = "rbSeedList";
        private const string GroupName = "GroupName";
        private const string GroupDescription = "GroupDescription";
        private const string ReservedShortName = "BLASTID";
        private const string SeedListPanelID = "SeedListPanel";
        private addGroup _addGroupInstance;
        private PrivateObject _privateObjectAddGroup;
        private PlaceHolder _phError;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _addGroupInstance = new addGroup();
            InitializeAllControls(_addGroupInstance);
            _privateObjectAddGroup = new PrivateObject(_addGroupInstance);
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

        [Test]
        public void Save_InvalidCustomerId_Error()
        {
            // Arrange
            InitTestSaveInvalidCustomerId();

            // Act
            _addGroupInstance.Save();

            // Assert
            _phError.Visible.ShouldBeTrue();
        }

        [TestCase(10)]
        [TestCase(-10)]
        public void Save_NotExistFolder_Error(int groupId)
        {
            // Arrange
            InitTestSaveNotExistFolder(groupId: groupId);

            // Act
            _addGroupInstance.Save();

            // Assert
            _phError.Visible.ShouldBeTrue();
        }

        [TestCase("", true, true)]
        [TestCase(GroupName, true, true)]
        [TestCase(GroupName, false, false)]
        public void Save_InvalidGroupName_Error(string groupName, bool groupExist, bool isValidObjectName)
        {
            // Arrange
            InitTestSaveInvalidGroupName(groupName: groupName, groupExist: groupExist, isValidObjectName: isValidObjectName);

            // Act
            _addGroupInstance.Save();

            // Assert
            _phError.Visible.ShouldBeTrue();
        }

        [TestCase(10)]
        [TestCase(-10)]
        public void Save_NoValidationErrors_Saved(int groupId)
        {
            // Arrange
            InitTestSaveNoValidationErrors(groupId: groupId);

            // Act
            var result = _addGroupInstance.Save();

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeFalse(),
                () => result.ShouldBeTrue());
        }

        [Test]
        public void Save_InvalidGroupDataFieldsCustomer_Error()
        {
            // Arrange
            InitTestSaveInvalidGroupDataFieldsCustomer();

            // Act
            var result = _addGroupInstance.Save();

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => result.ShouldBeFalse());
        }

        [Test]
        public void Save_EmptyGroupDataFieldsShortName_Error()
        {
            // Arrange
            InitTestSaveEmptyGroupDataFieldsShortName();

            // Act
            var result = _addGroupInstance.Save();

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => result.ShouldBeFalse());
        }

        [Test]
        public void Save_ReservedGroupDataFieldsShortName_Error()
        {
            // Arrange
            InitTestSaveReservedGroupDataFieldsShortName();

            // Act
            var result = _addGroupInstance.Save();

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => result.ShouldBeFalse());
        }

        [Test]
        public void Save_ExistGroupDataFieldsShortName_Error()
        {
            // Arrange
            InitTestSaveExistGroupDataFieldsShortName();

            // Act
            var result = _addGroupInstance.Save();

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeTrue(),
                () => result.ShouldBeFalse());
        }

        [Test]
        public void Reset_ControlsReset()
        {
            // Arrange
            InitCommon();
            _phError.Visible = true;
            var txtGroupName = Get<TextBox>(_privateObjectAddGroup, GroupNameId);
            var txtGroupDesc = Get<TextBox>(_privateObjectAddGroup, GroupDescriptionId);

            // Act
            _addGroupInstance.Reset();

            //Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeFalse(),
                () => txtGroupName.Text.ShouldBeEmpty(),
                () => txtGroupDesc.Text.ShouldBeEmpty());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PageLoad_ControlsInitialized(bool hasServiceFeature)
        {
            // Arrange 
            InitTestPageLoad(hasServiceFeature);
            var seedPanel = Get<Panel>(_privateObjectAddGroup, SeedListPanelID);
            _phError.Visible = true;

            // Act
            _privateObjectAddGroup.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            _phError.ShouldSatisfyAllConditions(
                () => _phError.Visible.ShouldBeFalse(),
                () => seedPanel.Visible.ShouldBe(hasServiceFeature));
        }

        private void InitCommon()
        {
            var publicFolterCheckbox = Get<CheckBox>(_privateObjectAddGroup, PublicFolderId);
            publicFolterCheckbox.Checked = true;
            _phError = Get<PlaceHolder>(_privateObjectAddGroup, PhErrorId);
            _phError.Visible = false;
            var txtGroupName = Get<TextBox>(_privateObjectAddGroup, GroupNameId);
            txtGroupName.Text = GroupName;
            var txtGroupDesc = Get<TextBox>(_privateObjectAddGroup, GroupDescriptionId);
            txtGroupDesc.Text = GroupDescription;
            var dropFolder = Get<DropDownList>(_privateObjectAddGroup, DrpFolderId);
            dropFolder.Items.Add(new ListItem(DropFolderValue.ToString(), DropFolderValue.ToString()) { Selected = true });
            var rbSeedList = Get<RadioButtonList>(_privateObjectAddGroup, RbSeedListId);
            rbSeedList.Items.Add(new ListItem(true.ToString(), true.ToString()) { Selected = true });
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, code, access) => true;
            BussinessCommFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<CommunicatorEntites.Group>((filter, user) => true);
            BussinessCommFakes.ShimAccessCheck.CanAccessByCustomerOf1M0User<CommunicatorEntites.GroupDataFields>((filter, user) => true);
        }

        private void InitTestPageLoad(bool hasServiceFeature = false)
        {
            InitCommon();
            KMPlatformFakes.ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (custId, service, feature) => hasServiceFeature;
            DataLayerCommFakes.ShimFolder.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.Folder>();
        }

        private void InitTestSaveExistGroupDataFieldsShortName()
        {
            InitCommon();
            BussinessCommFakes.ShimGroupDataFields.IsReservedWordString = (word) => false;
            BussinessCommFakes.ShimGroup.ValidateGroup = (grpData) => { };
            ShimGroupDataFields.AllInstances.CustomerIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.ShortNameGet = (gdf) => "ShortName";
            ShimGroupDataFields.AllInstances.IsPublicGet = (gdf) => "Y";
            ShimGroupDataFields.AllInstances.DatafieldSetIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.CreatedUserIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.UpdatedUserIDGet = (gdf) => 50;
            ShimUser.IsSystemAdministratorUser = (user) => false;
            KMPlatformFakes.ShimUser.ExistsInt32Int32 = (id, grpId) => false;
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => -10;
            ShimCustomer.ExistsInt32 = (id) => false; 
            BussinessCommFakes.ShimGroup.ExistsInt32Int32 = (groupId, custId) => false;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) => 1;
            DataLayerCommFakes.ShimGroupConfig.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.GroupConfig>()
            {
                new CommunicatorEntites.GroupConfig()
            };
            ShimGroupDataFields.AllInstances.GroupDataFieldsIDGet = (groupDataField) => 10;
            ShimSurvey.IsSurveyGroupInt32 = (id) => true;
        }

        private void InitTestSaveReservedGroupDataFieldsShortName()
        {
            InitCommon();
            BussinessCommFakes.ShimGroup.ValidateGroup = (grpData) => { };
            ShimGroupDataFields.AllInstances.CustomerIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.ShortNameGet = (gdf) => ReservedShortName;
            ShimGroupDataFields.AllInstances.IsPublicGet = (gdf) => "Y";
            ShimGroupDataFields.AllInstances.DatafieldSetIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.CreatedUserIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.UpdatedUserIDGet = (gdf) => 50;
            ShimUser.IsSystemAdministratorUser = (user) => false;
            KMPlatformFakes.ShimUser.ExistsInt32Int32 = (id, grpId) => false;
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => -10;
            ShimCustomer.ExistsInt32 = (id) => false;
            BussinessCommFakes.ShimGroup.ExistsInt32Int32 = (groupId, custId) => false;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) =>
            {
                if (command.CommandText.ToLower().Contains("if exists(select top 1 datafieldsetid"))
                {
                    return 0;
                }
                return 1;
            };
            DataLayerCommFakes.ShimGroupConfig.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.GroupConfig>()
            {
                new CommunicatorEntites.GroupConfig()
            };
            ShimGroupDataFields.AllInstances.GroupDataFieldsIDGet = (groupDataField) => 10;
            ShimDataFunctions.GetDataTableSqlCommandString = (cmd, connString) =>
            {
                var reservedNamesDataTable = new DataTable();
                reservedNamesDataTable.Columns.Add("columnName");
                var row = reservedNamesDataTable.NewRow();
                row[0] = "reserved";
                reservedNamesDataTable.Rows.Add(row);
                return reservedNamesDataTable;
            };
            ShimSurvey.IsSurveyGroupInt32 = (id) => true;
        }

        private void InitTestSaveEmptyGroupDataFieldsShortName()
        {
            InitCommon();
            BussinessCommFakes.ShimGroup.ValidateGroup = (grpData) => { };
            ShimGroupDataFields.AllInstances.CustomerIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.ShortNameGet = (gdf) => string.Empty;
            ShimGroupDataFields.AllInstances.IsPublicGet = (gdf) => "Y";
            ShimGroupDataFields.AllInstances.DatafieldSetIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.CreatedUserIDGet = (gdf) => 50;
            ShimGroupDataFields.AllInstances.UpdatedUserIDGet = (gdf) => 50;
            ShimUser.IsSystemAdministratorUser = (user) => false;
            KMPlatformFakes.ShimUser.ExistsInt32Int32 = (id, grpId) => false;
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => -10;
            ShimCustomer.ExistsInt32 = (id) => false;
            BussinessCommFakes.ShimGroup.ExistsInt32Int32 = (groupId, custId) => false;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) =>
            {
                if (command.CommandText.ToLower().Contains("if exists(select top 1 datafieldsetid"))
                {
                    return 0;
                }
                return 1;
            };

            DataLayerCommFakes.ShimGroupConfig.GetListSqlCommand = (cmd) => new System.Collections.Generic.List<CommunicatorEntites.GroupConfig>()
            {
                new CommunicatorEntites.GroupConfig()
            };
            ShimGroupDataFields.AllInstances.GroupDataFieldsIDGet = (groupDataField) => 10;
        }

        private void InitTestSaveInvalidGroupDataFieldsCustomer()
        {
            InitCommon();
            BussinessCommFakes.ShimGroup.ValidateGroup = (grpData) => { };
            ShimGroupDataFields.AllInstances.CustomerIDGet = (gdf) => null;
            ShimGroupDataFields.AllInstances.IsPublicGet = (gdf) => "Invalid";
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => -10;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) => 1;
            DataLayerCommFakes.ShimGroupConfig.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.GroupConfig>()
            {
                new CommunicatorEntites.GroupConfig()
            };
            ShimGroupDataFields.AllInstances.GroupDataFieldsIDGet = (groupDataField) => -10;
        }

        private void InitTestSaveNoValidationErrors(int groupId = 10)
        {
            InitCommon();
            BussinessCommFakes.ShimGroup.ValidateGroup = (grpData) => { };
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => groupId;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) => 1;
            DataLayerCommFakes.ShimGroupConfig.GetListSqlCommand = (cmd) => new List<CommunicatorEntites.GroupConfig>()
            {
                new CommunicatorEntites.GroupConfig()
            };
            ShimGroupDataFields.AllInstances.GroupDataFieldsIDGet = (groupDataField) => 10;
            BussinessCommFakes.ShimGroupDataFields.ValidateGroupDataFields = (grpDatafields) => { };
        }

        private void InitTestSaveInvalidGroupName(string groupName = "", bool groupExist = false, bool isValidObjectName = false)
        {
            InitCommon();
            ShimGroup.AllInstances.CustomerIDGet = (dataGroup) => 10;
            ShimGroup.AllInstances.FolderIDGet = (dataGroup) => 5;
            ShimGroup.AllInstances.MasterSupressionGet = (dataGroup) => 0;
            ShimGroup.AllInstances.GroupNameGet = (dataGroup) => groupName;
            ShimGroup.AllInstances.FolderIDGet = (dataGroup) => 5;
            BussinessCommFakes.ShimFolder.ExistsInt32Int32 = (id, groupId) => true;
            KMPlatformFakes.ShimUser.ExistsInt32Int32 = (id, custId) => true;
            ShimCustomer.ExistsInt32 = (id) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) =>
            {
                if (command.CommandText == "e_Group_Exists_ByName")
                {
                    return groupExist;
                }
                return true;
            };
            ShimRegexUtilities.IsValidObjectNameString = (name) => isValidObjectName;
        }

        private void InitTestSaveNotExistFolder(int groupId = 10)
        {
            InitCommon();
            ShimGroup.AllInstances.CustomerIDGet = (dataGroup) => 10;
            ShimGroup.AllInstances.FolderIDGet = (dataGroup) => 5;
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => groupId;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) =>
            {
                return false;
            };
            ShimCustomer.ExistsInt32 = (id) => false;
            ShimUser.IsSystemAdministratorUser = (User) => false;
            KMPlatformFakes.ShimUser.ExistsInt32Int32 = (id, custId) => false;
        }

        private void InitTestSaveInvalidCustomerId()
        {
            InitCommon();
            ShimGroup.AllInstances.CustomerIDGet = (dataGroup) => -1;
            ShimGroup.AllInstances.GroupIDGet = (dataGroup) => 5;
            ShimGroup.AllInstances.OwnerTypeCodeGet = (dataGroup) => "notCustomer";
            ShimGroup.AllInstances.MasterSupressionGet = (dataGroup) => 10;
            ShimGroup.AllInstances.PublicFolderGet = (dataGroup) => 10;
            ShimGroup.AllInstances.AllowUDFHistoryGet = (dataGroup) => "Invalid";
            ShimGroup.AllInstances.IsSeedListGet = (dataGroup) => true;
            ShimGroup.AllInstances.CreatedUserIDGet = (dataGroup) => null;
            ShimGroup.AllInstances.UpdatedUserIDGet = (dataGroup) => 10;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (command, connString) =>
            {
                if (command.CommandText == "e_Group_CheckForExistingSeedlist")
                {
                    return true;
                }
                return false;
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
    }
}
