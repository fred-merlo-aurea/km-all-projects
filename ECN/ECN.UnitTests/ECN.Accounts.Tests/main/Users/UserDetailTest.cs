using ecn.accounts.includes.Fakes;
using ecn.accounts.MasterPages.Fakes;
using ecn.accounts.usersmanager;
using ecn.accounts.usersmanager.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static KMPlatform.Enums;
using MasterPageAccount = ecn.accounts.MasterPages.Accounts;
using PrivateObject = Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject;

namespace ECN.Accounts.Tests.main.Users
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class UserDetailTest
    {
        private const int Zero = 0;
        private const string No = "no";
        private const string Yes = "yes";
        private const string EditRoleCommand = "editrole";
        private const string DeleteRoleCommand = "deleterole";
        private const string RestrictCommand = "restrict";
        private const string IsActiveColumn = "IsActive";
        private const string IsDeletedColumn = "IsDeleted";
        private const string InactiveReasonColumn = "InactiveReason";
        private const string DisplayColumn = "Display";
        private const string Disabled = "Disabled";
        private const string Pending = "Pending";
        private const string CustomerIdColumn = "CustomerID";
        private const string DoHardDeleteColumn = "DoHardDelete";
        private const string IsBCAdminColumn = "IsBCAdmin";
        private const string IsCAdminColumn = "IsCAdmin";
        private const string BaseChannelColumn = "BaseChannel";
        private const string IdColumn = "ID";
        private const string SecurityGroupIdColumn = "SecurityGroupID";
        private const string BaseChannelIdColumn = "BaseChannelID";
        private const string CustomerColumn = "Customer";
        private const string RoleColumn = "Role";
        private const string IsChannelRoleColumn = "IsChannelRole";
        private IDisposable _shimsContext;
        private userdetail _userDetail;
        private PrivateObject _userDetailPrivate;
        private Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            _userDetail = new userdetail();
            _userDetailPrivate = new PrivateObject(_userDetail);
            InitializeFields();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void gvUserRoles_RowCommand_EditRole_WorksAsExpected()
        {
            //Arrange
            var firstId = GetAnyString();
            var secondId = GetAnyString();
            var commandArguments = GetViewCommandEventArgs(EditRoleCommand, firstId, secondId);
            var securityGroupId = GetAnyNumber();
            var clientId = GetAnyNumber();
            var rowNames = new[] { "trBaseChannel", "trCustomer", "trRole", "trBCRole", "trIsBCRole" };
            var radioButtonListNames = new[] { "rblBCRoles", "rblIsBCAdmin", "rblIsCAdmin" };
            var rowControls = rowNames
                .Select(name => _userDetailPrivate.GetField(name) as HtmlTableRow)
                .ToList();
            var radioButtonListControls = radioButtonListNames
                .Select(name => _userDetailPrivate.GetField(name) as RadioButtonList)
                .ToList();
            var dropDownListRole = _userDetailPrivate.GetField("ddlRole") as DropDownList;
            dropDownListRole.Items.Add(securityGroupId.ToString());
            var dropDownListBaseChannel = _userDetailPrivate.GetField("ddlBaseChannel") as DropDownList;
            dropDownListBaseChannel.Items.Add(clientId.ToString());
            foreach (var radioButton in radioButtonListControls)
            {
                radioButton.Items.Add(No);
            }
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean =
                (instance, securityGroupIdParameter, isKMUser, includeServices) =>
                {
                    return new SecurityGroup
                    {
                        SecurityGroupID = securityGroupId,
                        ClientID = clientId
                    };
                };
            var resetRolePopupCalled = false;
            var loadCustomersCalled = false;
            var loadRolesForCustomer = false;
            var loadRolesForBaseChannel = false;
            var loadBaseChannels = false;
            var updatePanelUpdateCalled = false;
            SetUserRolesTable(CreateUserRolesTable(secondId, securityGroupId));
            Shimuserdetail.AllInstances.ResetRolePopup = instance =>
            {
                resetRolePopupCalled = true;
            };
            Shimuserdetail.AllInstances.LoadCustomers = instance =>
            {
                loadCustomersCalled = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForCustomer = instance =>
            {
                loadRolesForCustomer = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForBaseChannel = instance =>
            {
                loadRolesForBaseChannel = true;
            };
            Shimuserdetail.AllInstances.LoadBaseChannels = instance =>
            {
                loadBaseChannels = true;
            };
            ShimClientGroupClientMap.AllInstances.SelectForClientIDInt32 = (instance, id) =>
            {
                return new List<ClientGroupClientMap>
                {
                    new ClientGroupClientMap
                    {
                        ClientID = clientId
                    }
                };
            };
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            var saveRoleButton = _userDetailPrivate.GetField("btnSaveRole") as Button;
            saveRoleButton.CommandArgument.ShouldBe(secondId);
            resetRolePopupCalled.ShouldBeTrue();
            loadCustomersCalled.ShouldBeTrue();
            loadRolesForCustomer.ShouldBeTrue();
            loadRolesForBaseChannel.ShouldBeTrue();
            loadBaseChannels.ShouldBeTrue();
            updatePanelUpdateCalled.ShouldBeTrue();
            rowControls.SingleOrDefault(row => !row.Visible).ShouldNotBeNull();
            radioButtonListControls.ShouldAllBe(list => list.SelectedValue == No);
            dropDownListRole.SelectedValue.ShouldBe(securityGroupId.ToString());
            dropDownListBaseChannel.SelectedValue.ShouldBe(clientId.ToString());
        }

        [Test]
        public void gvUserRoles_RowCommand_EditRoleAndClientGroupIdAboveZero_WorksAsExpected()
        {
            //Arrange
            var firstId = GetAnyString();
            var secondId = GetAnyString();
            var commandArguments = GetViewCommandEventArgs(EditRoleCommand, firstId, secondId);
            var clientGroupId = GetAnyNumber();
            var securityGroupId = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean =
                (instance, securityGroupIdParameter, isKMUser, includeServices) =>
                {
                    return new SecurityGroup
                    {
                        SecurityGroupID = securityGroupId,
                        ClientGroupID = clientGroupId,
                        ClientID = Zero
                    };
                };
            var dropDownListBaseChannel = _userDetailPrivate.GetField("ddlBaseChannel") as DropDownList;
            dropDownListBaseChannel.Items.Add(clientGroupId.ToString());
            var dropDownBCRoles = _userDetailPrivate.GetField("ddlBCRoles") as DropDownList;
            dropDownBCRoles.Items.Add(securityGroupId.ToString());
            var radioButtonListIsBCAdmin = _userDetailPrivate.GetField("rblIsBCAdmin") as RadioButtonList;
            radioButtonListIsBCAdmin.Items.Add(No);
            var radioButtonListBCRoles = _userDetailPrivate.GetField("rblBCRoles") as RadioButtonList;
            radioButtonListBCRoles.Items.Add(Yes);
            var customerRow = _userDetailPrivate.GetField("trCustomer") as HtmlTableRow;
            var bcRoleRow = _userDetailPrivate.GetField("trBCRole") as HtmlTableRow;
            var isBCRoleRow = _userDetailPrivate.GetField("trIsBCRole") as HtmlTableRow;
            var baseChannelRow = _userDetailPrivate.GetField("trBaseChannel") as HtmlTableRow;
            var roleRow = _userDetailPrivate.GetField("trRole") as HtmlTableRow;
            var updatePanelUpdateCalled = false;
            var loadCustomersCalled = false;
            var loadRolesForCustomer = false;
            var loadRolesForBaseChannel = false;
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };
            Shimuserdetail.AllInstances.LoadCustomers = instance =>
            {
                loadCustomersCalled = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForCustomer = instance =>
            {
                loadRolesForCustomer = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForBaseChannel = instance =>
            {
                loadRolesForBaseChannel = true;
            };
            Shimuserdetail.AllInstances.ResetRolePopup = instance => { };
            SetUserRolesTable(CreateUserRolesTable(secondId, securityGroupId));

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            updatePanelUpdateCalled.ShouldBeTrue();
            loadCustomersCalled.ShouldBeTrue();
            loadRolesForCustomer.ShouldBeTrue();
            loadRolesForBaseChannel.ShouldBeTrue();
            dropDownListBaseChannel.SelectedValue.ShouldBe(clientGroupId.ToString());
            dropDownBCRoles.SelectedValue.ShouldBe(securityGroupId.ToString());
            radioButtonListIsBCAdmin.SelectedValue.ShouldBe(No);
            baseChannelRow.Visible.ShouldBeTrue();
            bcRoleRow.Visible.ShouldBeTrue();
            isBCRoleRow.Visible.ShouldBeTrue();
            radioButtonListBCRoles.SelectedValue.ShouldBe(Yes);
            customerRow.Visible.ShouldBeFalse();
            roleRow.Visible.ShouldBeFalse();
        }

        [Test]
        public void gvUserRoles_RowCommand_EditRoleAndClientGroupIdAboveZeroAndSecurityGroupChannelAdministrator_WorksAsExpected()
        {
            //Arrange
            var firstId = GetAnyString();
            var secondId = GetAnyString();
            var commandArguments = GetViewCommandEventArgs(EditRoleCommand, firstId, secondId);
            var clientGroupId = GetAnyNumber();
            var securityGroupId = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean =
                (instance, securityGroupIdParameter, isKMUser, includeServices) =>
                {
                    return new SecurityGroup
                    {
                        SecurityGroupID = securityGroupId,
                        ClientGroupID = clientGroupId,
                        ClientID = Zero,
                        AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                    };
                };
            var dropDownListBaseChannel = _userDetailPrivate.GetField("ddlBaseChannel") as DropDownList;
            dropDownListBaseChannel.Items.Add(clientGroupId.ToString());
            var dropDownBCRoles = _userDetailPrivate.GetField("ddlBCRoles") as DropDownList;
            dropDownBCRoles.Items.Add(securityGroupId.ToString());
            var radioButtonListIsBCAdmin = _userDetailPrivate.GetField("rblIsBCAdmin") as RadioButtonList;
            radioButtonListIsBCAdmin.Items.Add(Yes);
            var radioButtonListBCRoles = _userDetailPrivate.GetField("rblBCRoles") as RadioButtonList;
            radioButtonListBCRoles.Items.Add(No);
            var customerRow = _userDetailPrivate.GetField("trCustomer") as HtmlTableRow;
            var bcRoleRow = _userDetailPrivate.GetField("trBCRole") as HtmlTableRow;
            var isBCRoleRow = _userDetailPrivate.GetField("trIsBCRole") as HtmlTableRow;
            var baseChannelRow = _userDetailPrivate.GetField("trBaseChannel") as HtmlTableRow;
            var roleRow = _userDetailPrivate.GetField("trRole") as HtmlTableRow;
            var updatePanelUpdateCalled = false;
            var loadCustomersCalled = false;
            var loadRolesForCustomer = false;
            var loadRolesForBaseChannel = false;
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };
            Shimuserdetail.AllInstances.LoadCustomers = instance =>
            {
                loadCustomersCalled = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForCustomer = instance =>
            {
                loadRolesForCustomer = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForBaseChannel = instance =>
            {
                loadRolesForBaseChannel = true;
            };
            Shimuserdetail.AllInstances.ResetRolePopup = instance => { };
            SetUserRolesTable(CreateUserRolesTable(secondId, securityGroupId));
            ShimUser.IsChannelAdministratorUser = user =>
            {
                return false;
            };
            var userSession = CreateUserSession();
            ShimAccounts.AllInstances.UserSessionGet = instance => userSession;
            userSession.CurrentUser = new User
            {
                IsActive = true
            };

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            updatePanelUpdateCalled.ShouldBeTrue();
            loadCustomersCalled.ShouldBeTrue();
            loadRolesForCustomer.ShouldBeTrue();
            loadRolesForBaseChannel.ShouldBeTrue();
            dropDownListBaseChannel.SelectedValue.ShouldBe(clientGroupId.ToString());
            dropDownBCRoles.SelectedValue.ShouldBe(securityGroupId.ToString());
            radioButtonListIsBCAdmin.SelectedValue.ShouldBe(Yes);
            radioButtonListIsBCAdmin.Enabled.ShouldBeFalse();
            baseChannelRow.Visible.ShouldBeTrue();
            bcRoleRow.Visible.ShouldBeFalse();
            isBCRoleRow.Visible.ShouldBeFalse();
            radioButtonListBCRoles.SelectedValue.ShouldBe(No);
            customerRow.Visible.ShouldBeFalse();
            roleRow.Visible.ShouldBeFalse();
        }

        [Test]
        public void gvUserRoles_RowCommand_EditRoleAndClientGroupIdAboveZeroAndSecurityGroupChannelAndCurrentUserAdministrator_WorksAsExpected()
        {
            //Arrange
            var firstId = GetAnyString();
            var secondId = GetAnyString();
            var commandArguments = GetViewCommandEventArgs(EditRoleCommand, firstId, secondId);
            var clientGroupId = GetAnyNumber();
            var securityGroupId = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean =
                (instance, securityGroupIdParameter, isKMUser, includeServices) =>
                {
                    return new SecurityGroup
                    {
                        SecurityGroupID = securityGroupId,
                        ClientGroupID = clientGroupId,
                        ClientID = Zero,
                        AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                    };
                };
            var dropDownListBaseChannel = _userDetailPrivate.GetField("ddlBaseChannel") as DropDownList;
            dropDownListBaseChannel.Items.Add(clientGroupId.ToString());
            var dropDownBCRoles = _userDetailPrivate.GetField("ddlBCRoles") as DropDownList;
            dropDownBCRoles.Items.Add(securityGroupId.ToString());
            var radioButtonListIsBCAdmin = _userDetailPrivate.GetField("rblIsBCAdmin") as RadioButtonList;
            radioButtonListIsBCAdmin.Items.Add(Yes);
            var radioButtonListBCRoles = _userDetailPrivate.GetField("rblBCRoles") as RadioButtonList;
            radioButtonListBCRoles.Items.Add(No);
            var customerRow = _userDetailPrivate.GetField("trCustomer") as HtmlTableRow;
            var bcRoleRow = _userDetailPrivate.GetField("trBCRole") as HtmlTableRow;
            var isBCRoleRow = _userDetailPrivate.GetField("trIsBCRole") as HtmlTableRow;
            var baseChannelRow = _userDetailPrivate.GetField("trBaseChannel") as HtmlTableRow;
            var roleRow = _userDetailPrivate.GetField("trRole") as HtmlTableRow;
            var updatePanelUpdateCalled = false;
            var loadCustomersCalled = false;
            var loadRolesForCustomer = false;
            var loadRolesForBaseChannel = false;
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };
            Shimuserdetail.AllInstances.LoadCustomers = instance =>
            {
                loadCustomersCalled = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForCustomer = instance =>
            {
                loadRolesForCustomer = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForBaseChannel = instance =>
            {
                loadRolesForBaseChannel = true;
            };
            Shimuserdetail.AllInstances.ResetRolePopup = instance => { };
            SetUserRolesTable(CreateUserRolesTable(secondId, securityGroupId));
            ShimUser.IsChannelAdministratorUser = user =>
            {
                return true;
            };
            var userSession = CreateUserSession();
            ShimAccounts.AllInstances.UserSessionGet = instance => userSession;
            userSession.CurrentUser = new User
            {
                IsActive = true,
                IsPlatformAdministrator = true
            };

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            updatePanelUpdateCalled.ShouldBeTrue();
            loadCustomersCalled.ShouldBeTrue();
            loadRolesForCustomer.ShouldBeTrue();
            loadRolesForBaseChannel.ShouldBeTrue();
            dropDownListBaseChannel.SelectedValue.ShouldBe(clientGroupId.ToString());
            dropDownBCRoles.SelectedValue.ShouldBe(securityGroupId.ToString());
            radioButtonListIsBCAdmin.SelectedValue.ShouldBe(Yes);
            radioButtonListIsBCAdmin.Enabled.ShouldBeTrue();
            baseChannelRow.Visible.ShouldBeTrue();
            bcRoleRow.Visible.ShouldBeFalse();
            isBCRoleRow.Visible.ShouldBeFalse();
            radioButtonListBCRoles.SelectedValue.ShouldBe(No);
            customerRow.Visible.ShouldBeFalse();
            roleRow.Visible.ShouldBeFalse();
        }

        [Test]
        public void gvUserRoles_RowCommand_EditRoleAndAdminstrator_WorksAsExpected()
        {
            //Arrange
            var firstId = GetAnyString();
            var secondId = GetAnyString();
            var commandArguments = GetViewCommandEventArgs(EditRoleCommand, firstId, secondId);
            var clientGroupId = GetAnyNumber();
            var clientId = GetAnyNumber();
            var securityGroupId = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean =
                (instance, securityGroupIdParameter, isKMUser, includeServices) =>
                {
                    return new SecurityGroup
                    {
                        SecurityGroupID = securityGroupId,
                        ClientGroupID = clientGroupId,
                        ClientID = clientId,
                        AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator
                    };
                };
            var dropDownListBaseChannel = _userDetailPrivate.GetField("ddlBaseChannel") as DropDownList;
            dropDownListBaseChannel.Items.Add(clientGroupId.ToString());
            var radioButtonListIsBCAdmin = _userDetailPrivate.GetField("rblIsBCAdmin") as RadioButtonList;
            radioButtonListIsBCAdmin.Items.Add(No);
            var radioButtonListIssCAdmin = _userDetailPrivate.GetField("rblIsCAdmin") as RadioButtonList;
            radioButtonListIssCAdmin.Items.Add(Yes);
            var radioButtonListBCRoles = _userDetailPrivate.GetField("rblBCRoles") as RadioButtonList;
            radioButtonListBCRoles.Items.Add(No);
            var dropDownCustomers = _userDetailPrivate.GetField("ddlCustomer") as DropDownList;
            dropDownCustomers.Items.Add(clientId.ToString());
            var customerRow = _userDetailPrivate.GetField("trCustomer") as HtmlTableRow;
            var bcRoleRow = _userDetailPrivate.GetField("trBCRole") as HtmlTableRow;
            var isBCRoleRow = _userDetailPrivate.GetField("trIsBCRole") as HtmlTableRow;
            var baseChannelRow = _userDetailPrivate.GetField("trBaseChannel") as HtmlTableRow;
            var roleRow = _userDetailPrivate.GetField("trRole") as HtmlTableRow;
            var updatePanelUpdateCalled = false;
            var loadCustomersCalled = false;
            var loadRolesForCustomer = false;
            var loadRolesForBaseChannel = false;
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };
            Shimuserdetail.AllInstances.LoadCustomers = instance =>
            {
                loadCustomersCalled = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForCustomer = instance =>
            {
                loadRolesForCustomer = true;
            };
            Shimuserdetail.AllInstances.LoadRolesForBaseChannel = instance =>
            {
                loadRolesForBaseChannel = true;
            };
            Shimuserdetail.AllInstances.ResetRolePopup = instance => { };
            SetUserRolesTable(CreateUserRolesTable(secondId, securityGroupId));
            ShimUser.IsChannelAdministratorUser = user =>
            {
                return true;
            };
            var userSession = CreateUserSession();
            ShimAccounts.AllInstances.UserSessionGet = instance => userSession;
            userSession.CurrentUser = new User
            {
                IsActive = true,
                IsPlatformAdministrator = true
            };
            ShimClientGroupClientMap.AllInstances.SelectForClientIDInt32 = (instance, id) =>
            {
                return new List<ClientGroupClientMap>
                {
                    new ClientGroupClientMap
                    {
                        ClientID = clientId
                    }
                };
            };
            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            baseChannelRow.Visible.ShouldBeTrue();
            customerRow.Visible.ShouldBeTrue();
            roleRow.Visible.ShouldBeFalse();
            isBCRoleRow.Visible.ShouldBeTrue();
            bcRoleRow.Visible.ShouldBeFalse();
            radioButtonListBCRoles.SelectedValue.ShouldBe(No);
            radioButtonListIsBCAdmin.SelectedValue.ShouldBe(No);
            radioButtonListIssCAdmin.SelectedValue.ShouldBe(Yes);
            dropDownListBaseChannel.SelectedValue.ShouldBe(clientGroupId.ToString());
            loadCustomersCalled.ShouldBeTrue();
            dropDownCustomers.SelectedValue.ShouldBe(clientId.ToString());
            updatePanelUpdateCalled.ShouldBeTrue();
            loadRolesForCustomer.ShouldBeTrue();
            loadRolesForBaseChannel.ShouldBeTrue();
        }

        [Test]
        public void gvUserRoles_RowCommand_DeleteRoleWithoutDash_WorksAsExpected()
        {
            //Arrange
            const char ExcludedChar = '-';
            var firstId = GetAnyString();
            var secondId = GetAnyString(ExcludedChar);
            var commandArguments = GetViewCommandEventArgs(DeleteRoleCommand, firstId, secondId);
            var securityGroupId = GetAnyNumber();
            var rolesTable = CreateUserRolesTable(secondId, GetAnyNumber());
            SetUserRolesTable(rolesTable);
            var row = rolesTable.Rows[0];
            var bindRolesGridCalled = false;
            Shimuserdetail.AllInstances.BindRolesGrid = instance =>
            {
                bindRolesGridCalled = true;
            };

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            row[IsActiveColumn].ShouldBe(false);
            row[IsDeletedColumn].ShouldBe(true);
            row[InactiveReasonColumn].ShouldBe(Disabled);
            row[DisplayColumn].ShouldBe(true);
            bindRolesGridCalled.ShouldBeTrue();
        }

        [Test]
        public void gvUserRoles_RowCommand_DeleteRoleWithDash_WorksAsExpected()
        {
            //Arrange
            var firstId = GetAnyString();
            var secondId = GetAnyString();
            var commandArguments = GetViewCommandEventArgs(DeleteRoleCommand, firstId, secondId);
            var securityGroupId = GetAnyNumber();
            var rolesTable = CreateUserRolesTable(secondId, GetAnyNumber());
            SetUserRolesTable(rolesTable);
            var row = rolesTable.Rows[0];
            var bindRolesGridCalled = false;
            Shimuserdetail.AllInstances.BindRolesGrid = instance =>
            {
                bindRolesGridCalled = true;
            };
            var viewState = _userDetailPrivate.GetProperty("ViewState") as StateBag;
            var viewStateKey = $"RestrictGroups_{row[CustomerIdColumn]}";
            viewState.Add(viewStateKey, GetAnyString());

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert
            row[IsActiveColumn].ShouldBe(false);
            row[IsDeletedColumn].ShouldBe(true);
            row[InactiveReasonColumn].ShouldBe(Disabled);
            row[DisplayColumn].ShouldBe(false);
            bindRolesGridCalled.ShouldBeTrue();
            viewState[viewStateKey].ShouldBeNull();
        }

        [Test]
        public void gvUserRoles_RowCommand_RestrictWithDashInUserId_WorksAsExpected()
        {
            //Arrange
            var userId = GetAnyString();
            var customerId = GetAnyNumber();
            var customerIdString = $"{customerId}";
            var commandArguments = GetViewCommandEventArgs(RestrictCommand, userId, customerIdString);
            ShimCustomer.GetByClientIDInt32Boolean = (customerIdParameter, getChildren) =>
            {
                return new Customer
                {
                    CustomerID = customerId
                };
            };
            var ultiSelectGroupExplorerResetCalled = false;
            ShimMultiSelectGroupExplorer.AllInstances.resetInt32 = (instance, customerIdParameter) =>
            {
                ultiSelectGroupExplorerResetCalled = true;
            };
            var selectedGroups = new List<int>();
            ShimMultiSelectGroupExplorer.AllInstances.setSelectedGroupInt32 = (instance, groupIdParameter) =>
            {
                selectedGroups.Add(groupIdParameter);
            };
            var updatePanelUpdateCalled = false;
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };
            var viewState = _userDetailPrivate.GetProperty("ViewState") as StateBag;
            var key = $"RestrictGroups_{customerId}";
            var groupId = GetAnyNumber();
            viewState.Add(key, new[] { groupId }.ToList());
            var saveRestrictGroupsButton = _userDetailPrivate.GetField("btnSaveRestrictGroups") as Button;

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert

            ultiSelectGroupExplorerResetCalled.ShouldBeTrue();
            updatePanelUpdateCalled.ShouldBeTrue();
            saveRestrictGroupsButton.CommandArgument.ShouldBe(commandArguments.CommandArgument.ToString());
            selectedGroups.ShouldContain(groupId);
        }

        [Test]
        public void gvUserRoles_RowCommand_Restrict_WorksAsExpected()
        {
            //Arrange
            var userId = $"{GetAnyNumber()}";
            var customerId = GetAnyNumber();
            var customerIdString = $"{customerId}";
            var commandArguments = GetViewCommandEventArgs(RestrictCommand, userId, customerIdString);
            ShimCustomer.GetByClientIDInt32Boolean = (customerIdParameter, getChildren) =>
            {
                return new Customer
                {
                    CustomerID = customerId
                };
            };
            var ultiSelectGroupExplorerResetCalled = false;
            ShimMultiSelectGroupExplorer.AllInstances.resetInt32 = (instance, customerIdParameter) =>
            {
                ultiSelectGroupExplorerResetCalled = true;
            };
            var selectedGroups = new List<int>();
            ShimMultiSelectGroupExplorer.AllInstances.setSelectedGroupInt32 = (instance, groupIdParameter) =>
            {
                selectedGroups.Add(groupIdParameter);
            };
            var updatePanelUpdateCalled = false;
            ShimUpdatePanel.AllInstances.Update = instance =>
            {
                updatePanelUpdateCalled = true;
            };
            var groupId = GetAnyNumber();
            ShimUserGroup.GetInt32 = id =>
            {
                return new List<UserGroup>
                {
                    new UserGroup
                    {
                        GroupID = groupId
                    }
                };
            };
            var viewState = _userDetailPrivate.GetProperty("ViewState") as StateBag;
            var key = $"RestrictGroups_{customerId}";
            var saveRestrictGroupsButton = _userDetailPrivate.GetField("btnSaveRestrictGroups") as Button;

            //Act
            CallgvUserRoles_RowCommand(commandArguments);

            //Assert

            ultiSelectGroupExplorerResetCalled.ShouldBeTrue();
            updatePanelUpdateCalled.ShouldBeTrue();
            saveRestrictGroupsButton.CommandArgument.ShouldBe(commandArguments.CommandArgument.ToString());
            selectedGroups.ShouldContain(groupId);
            var updatedRestrictedGroups = viewState[key] as List<int>;
            updatedRestrictedGroups.ShouldNotBeNull();
            updatedRestrictedGroups.FirstOrDefault().ShouldBe(groupId);
        }

        [Test]
        public void btnSaveRole_Click_CurrentRoleIdAndBaseChannelIdAndIsBCAdmin_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = Yes;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            saveRoleButton.CommandArgument = currentRoleId;
            SetUserRolesTable(CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId));
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, row) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act, Assert
            CallbtnSaveRole_Click();
        }

        [Test]
        public void btnSaveRole_Click_CurrentRoleIdAndBaseChannelIdAndBCRolesYesNotIsBCAdmin_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = Yes;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            SetUserRolesTable(CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId));
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, row) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act, Assert
            CallbtnSaveRole_Click();
        }

        [Test]
        public void btnSaveRole_Click_CurrentRoleIdAndBaseChannelIdAndIsCAAdmin_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = Yes;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            SetUserRolesTable(CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId));
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, id, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                    {
                        AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator
                    }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, row) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act, Assert
            CallbtnSaveRole_Click();
        }

        [Test]
        public void btnSaveRole_Click_CurrentRoleIdAndBaseChannelId_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var rolesTable = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = rolesTable.Rows[0];
            SetUserRolesTable(rolesTable);
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsDeletedColumn].ShouldBe(true),
                () => row[IsActiveColumn].ShouldBe(false),
                () => row[InactiveReasonColumn].ShouldBe(Disabled),
                () => row[DisplayColumn].ShouldBe(false),
                () => row[DoHardDeleteColumn].ShouldBe(true));
        }

        [Test]
        public void btnSaveRole_Click_CurrentRoleIdAndIsBCAdminYesAndBCRolesYes_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = Yes;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsDeletedColumn].ShouldBe(true),
                () => row[IsActiveColumn].ShouldBe(false),
                () => row[InactiveReasonColumn].ShouldBe(Disabled),
                () => row[DisplayColumn].ShouldBe(false),
                () => row[DoHardDeleteColumn].ShouldBe(true));
        }

        [Test]
        public void btnSaveRole_Click_CurrentRoleId_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsDeletedColumn].ShouldBe(true),
                () => row[IsActiveColumn].ShouldBe(false),
                () => row[InactiveReasonColumn].ShouldBe(Disabled),
                () => row[DisplayColumn].ShouldBe(false),
                () => row[DoHardDeleteColumn].ShouldBe(true));
        }

        [Test]
        public void btnSaveRole_Click_WithoutCurrentRoleId_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = string.Empty;
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsDeletedColumn].ShouldBe(false),
                () => row[IsActiveColumn].ShouldBe(false),
                () => row[InactiveReasonColumn].ShouldBe(Pending),
                () => row[DisplayColumn].ShouldBe(true));
        }

        [Test]
        public void btnSaveRole_Click_IsBCAdminAndIsBCRoleNotVisibleAndSelectedBaseChannel_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = Yes;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            ddlBaseChannel.SelectedIndex = 0;
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsBCAdminColumn].ShouldBe(true),
                () => row[IsCAdminColumn].ShouldBe(false),
                () => row[BaseChannelColumn].ShouldBe(ddlBaseChannel.SelectedItem.Text),
                () => row[BaseChannelIdColumn].ToString().ShouldBe(ddlBaseChannel.SelectedValue),
                () => row[CustomerIdColumn].ShouldBe(Zero),
                () => row[SecurityGroupIdColumn].ShouldBe(securityGroupID),
                () => row[RoleColumn].ShouldBe("Channel Administrator"),
                () => row[IsChannelRoleColumn].ShouldBe(true));
        }

        [Test]
        public void btnSaveRole_Click_IsBCAdminAndIsBCRoleNotVisible_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = Yes;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            trBaseChannelError.Visible.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_Click_BCRoleYesAndIsBCAdminNoAndBaseChannelSelectedAndBCRolesSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = Yes;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            ddlBCRoles.Items.Add(GetAnyNumber().ToString());
            ddlBCRoles.SelectedIndex = 0;
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsCAdminColumn].ShouldBe(false),
                () => row[IsBCAdminColumn].ShouldBe(false),
                () => row[SecurityGroupIdColumn].ToString().ShouldBe(ddlBCRoles.SelectedValue),
                () => row[BaseChannelColumn].ShouldBe(ddlBaseChannel.SelectedItem.Text),
                () => row[BaseChannelIdColumn].ToString().ShouldBe(ddlBaseChannel.SelectedValue),
                () => row[CustomerColumn].ShouldBe(string.Empty),
                () => row[CustomerIdColumn].ShouldBe(Zero),
                () => row[RoleColumn].ShouldBe(ddlBCRoles.SelectedItem.Text),
                () => row[IsChannelRoleColumn].ShouldBe(true));
        }

        [Test]
        public void btnSaveRole_Click_BCRoleYesAndIsBCAdminNoAndBaseChannelSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = Yes;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            lblBCRoleError.Visible.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_Click_BCRoleYesAndIsBCAdminNoAndBaseChannelNotSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = Yes;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            trBaseChannelError.Visible.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_Click_IsCAAdminYesAndCustomerSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(Yes);
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = Yes;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            ddlCustomer.SelectedIndex = 0;
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsCAdminColumn].ShouldBe(true),
                () => row[IsBCAdminColumn].ShouldBe(false),
                () => row[SecurityGroupIdColumn].ShouldBe(securityGroupID),
                () => row[BaseChannelColumn].ShouldBe(ddlBaseChannel.SelectedItem.Text),
                () => row[BaseChannelIdColumn].ShouldBe(Zero),
                () => row[CustomerColumn].ShouldBe(ddlCustomer.SelectedItem.Text),
                () => row[CustomerIdColumn].ToString().ShouldBe(ddlCustomer.SelectedValue),
                () => row[SecurityGroupIdColumn].ShouldBe(securityGroupID),
                () => row[RoleColumn].ShouldBe("Administrator"),
                () => row[IsChannelRoleColumn].ShouldBe(false));
        }

        [Test]
        public void btnSaveRole_Click_IsCAAdminYesAndCustomerNotSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(Yes);
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = Yes;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            trCustomerError.Visible.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_Click_BaseChannelSelectedAndCustomerSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(Yes);
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            row.ShouldSatisfyAllConditions(
                () => row[IsCAdminColumn].ShouldBe(false),
                () => row[IsBCAdminColumn].ShouldBe(false),
                () => row[SecurityGroupIdColumn].ToString().ShouldBe(ddlRole.SelectedValue),
                () => row[BaseChannelColumn].ShouldBe(ddlBaseChannel.SelectedItem.Text),
                () => row[BaseChannelIdColumn].ShouldBe(Zero),
                () => row[CustomerColumn].ToString().ShouldBe(ddlCustomer.SelectedItem.Text),
                () => row[CustomerIdColumn].ToString().ShouldBe(ddlCustomer.SelectedValue),
                () => row[RoleColumn].ShouldBe(ddlRole.SelectedItem.Text),
                () => row[IsChannelRoleColumn].ShouldBe(false));
        }

        [Test]
        public void btnSaveRole_Click_BaseChannelSelectedAndCustomerNotSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(Yes);
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            trCustomerError.Visible.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_Click_BaseChannelNotSelected_WorksAsExpected()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = Zero;
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };

            //Act
            CallbtnSaveRole_Click();

            //Assert
            trBaseChannelError.Visible.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_Click_UserRolesNull_WillBeFilled()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = No;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            SetUserRolesTable(null);
            var rowExists = false;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };
            var buildUserRoleTableCalled = false;
            Shimuserdetail.AllInstances.BuildUserRoleDTListOfUserClientSecurityGroupMap = (instance, list) =>
            {
                buildUserRoleTableCalled = true;
                SetUserRolesTable(table);
            };
            //Act
            CallbtnSaveRole_Click();

            //Assert
            var expectedTable = _userDetailPrivate.GetField("dtUserRoles");
            buildUserRoleTableCalled.ShouldBeTrue();
            table.ShouldNotBeNull();
        }

        [Test]
        public void btnSaveRole_RowExistsAndCurrentRoleIdEmpty_SouldCallthrowECNException()
        {
            //Arrange
            var currentRoleId = string.Empty;
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = Yes;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            SetUserRolesTable(table);
            var rowExists = true;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };
            var throwECNExceptionCalled = false;
            Shimuserdetail.AllInstances.throwECNExceptionStringPlaceHolderLabel =
                (instance, message, placeHolder, label) =>
                {
                    throwECNExceptionCalled = true;
                };
            //Act
            CallbtnSaveRole_Click();

            //Assert
            throwECNExceptionCalled.ShouldBeTrue();
        }

        [Test]
        public void btnSaveRole_RowExistsAndCurrentRoleIdNotEmpty_SouldCallthrowECNException()
        {
            //Arrange
            var currentRoleId = GetAnyString();
            var baseChannelId = GetAnyNumber();
            var saveRoleButton = GetPrivateField<Button>("btnSaveRole");
            saveRoleButton.CommandArgument = currentRoleId;
            var rblIsBCAdmin = GetIsBCAdminRadioButtonList();
            rblIsBCAdmin.SelectedValue = No;
            var trIsBCRole = GetPrivateField<HtmlTableRow>("trIsBCRole");
            trIsBCRole.Visible = false;
            var ddlBaseChannel = GetPrivateField<DropDownList>("ddlBaseChannel");
            ddlBaseChannel.Items.Add(GetAnyNumber().ToString());
            var rblBCRoles = GetPrivateField<RadioButtonList>("rblBCRoles");
            rblBCRoles.Items.Add(Yes);
            rblBCRoles.Items.Add(No);
            rblBCRoles.SelectedValue = No;
            var ddlBCRoles = GetPrivateField<DropDownList>("ddlBCRoles");
            var rblIsCAdmin = GetPrivateField<RadioButtonList>("rblIsCAdmin");
            rblIsCAdmin.Items.Add(Yes);
            rblIsCAdmin.Items.Add(No);
            rblIsCAdmin.SelectedValue = Yes;
            var ddlCustomer = GetPrivateField<DropDownList>("ddlCustomer");
            ddlCustomer.Items.Add(GetAnyNumber().ToString());
            var ddlRole = GetPrivateField<DropDownList>("ddlRole");
            ddlRole.Items.Add(GetAnyNumber().ToString());
            ddlRole.SelectedIndex = 0;
            var trBaseChannelError = GetPrivateField<HtmlTableRow>("trBaseChannelError");
            var lblBCRoleError = GetPrivateField<Label>("lblBCRoleError");
            var trCustomerError = GetPrivateField<HtmlTableRow>("trCustomerError");
            var table = CreateUserRolesTable(currentRoleId, baseChannelId: baseChannelId);
            var row = table.Rows[0];
            SetUserRolesTable(table);
            var rowExists = true;
            var securityGroupID = GetAnyNumber();
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                            SecurityGroupID = securityGroupID
                        }
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean = (instance, clinetId, includeServices) =>
            {
                return new List<SecurityGroup>
                {
                    new SecurityGroup
                        {
                            AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator,
                            SecurityGroupID = securityGroupID
                        }
                };
            };
            Shimuserdetail.AllInstances.DRExistsDataRow = (instance, rowParameter) =>
            {
                row = rowParameter;
                return rowExists;
            };
            Shimuserdetail.AllInstances.BindRolesGrid = instance => { };
            var rowsCountBefore = table.Rows.Count;

            //Act
            CallbtnSaveRole_Click();

            //Assert
            table.Rows.Count.ShouldBeGreaterThan(rowsCountBefore);
        }

        private RadioButtonList GetIsBCAdminRadioButtonList()
        {
            var result = GetPrivateField<RadioButtonList>("rblIsBCAdmin");
            result.Items.Add(Yes);
            result.Items.Add(No);
            return result;
        }

        private ECNSession CreateUserSession()
        {
            ShimPage.AllInstances.MasterGet = instance =>
            {
                return new MasterPageAccount();
            };
            ShimECNSession.Constructor = instance => { };
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var constructor = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null);
            return constructor?.Invoke(new object[0]) as ECNSession;
        }

        private GridViewCommandEventArgs GetViewCommandEventArgs(string commandName, string firstId, string secondId)
        {
            var argument = $"{firstId}_{secondId}";
            return new GridViewCommandEventArgs(null, new CommandEventArgs(commandName, argument));
        }

        private string GetAnyString(params char[] charsToExclude)
        {
            var result = Guid.NewGuid().ToString();
            if (charsToExclude!=null && charsToExclude.Any())
            {
                var pattern = $"[{string.Join(string.Empty, charsToExclude)}]";
                result = Regex.Replace(result, pattern, string.Empty);
            }
            return result;
        }

        private void SetUserRolesTable(DataTable table)
        {
            const string FieldName = "dtUserRoles";
            _userDetailPrivate.SetField(FieldName, table);
        }

        private int GetAnyNumber()
        {
            const int randomStart = 10;
            const int randomEnd = randomStart * 100;
            return _random.Next(randomStart, randomEnd);
        }

        private DataTable CreateUserRolesTable(string id, int? securityGroupId = null, int? baseChannelId = null)
        {
            var result = new DataTable();
            var columns = new[]
            {
                new DataColumn(IdColumn),
                new DataColumn(BaseChannelColumn),
                new DataColumn(BaseChannelIdColumn, typeof(int)),
                new DataColumn(CustomerColumn),
                new DataColumn(CustomerIdColumn, typeof(int)),
                new DataColumn(RoleColumn),
                new DataColumn(SecurityGroupIdColumn, typeof(int)),
                new DataColumn(InactiveReasonColumn),
                new DataColumn(IsBCAdminColumn, typeof(bool)),
                new DataColumn(IsCAdminColumn, typeof(bool)),
                new DataColumn(IsActiveColumn, typeof(bool)),
                new DataColumn(IsDeletedColumn, typeof(bool)),
                new DataColumn(DisplayColumn, typeof(bool)),
                new DataColumn(DoHardDeleteColumn, typeof(bool)),
                new DataColumn(IsChannelRoleColumn, typeof(bool))
            };
            result.Columns.AddRange(columns);
            var row = result.NewRow();
            row[IdColumn] = id;
            row[SecurityGroupIdColumn] = securityGroupId ?? GetAnyNumber();
            row[BaseChannelIdColumn] = baseChannelId ?? GetAnyNumber();
            row[CustomerIdColumn] = GetAnyNumber();
            result.Rows.Add(row);
            return result;
        }

        private void CallgvUserRoles_RowCommand(GridViewCommandEventArgs args)
        {
            const string MethodName = "gvUserRoles_RowCommand";
            _userDetailPrivate.Invoke(MethodName, new object[] { null, args });
        }

        private void CallbtnSaveRole_Click()
        {
            const string MethodName = "btnSaveRole_Click";
            _userDetailPrivate.Invoke(MethodName, new object[] { null, null });
        }

        private T GetPrivateField<T>(string fieldName) where T : class
        {
            var field = _userDetailPrivate.GetField(fieldName) as T;
            field.ShouldNotBeNull($"Private field {fieldName} of type {typeof(T).Name} should not be null");
            return field;
        }

        private void InitializeFields()
        {
            Func<string> getIdFunc = () => $"ID__{GetAnyNumber()}";
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var fields = _userDetail.GetType()
                .GetFields(flags)
                .Where(field => field.GetValue(_userDetail) == null);
            foreach (var field in fields)
            {
                var fieldValue = field.FieldType
                    .GetConstructor(new Type[0])
                    ?.Invoke(new object[0]);
                if (fieldValue != null)
                {
                    var idProperty = field.FieldType.GetProperty("ID");
                    idProperty?.SetValue(fieldValue, getIdFunc());
                }
                field.SetValue(_userDetail, fieldValue);
            }
        }
    }
}
