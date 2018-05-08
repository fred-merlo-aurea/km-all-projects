using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Transactions.Fakes;
using System.Web.UI.WebControls;
using ecn.accounts.MasterPages.Fakes;
using ecn.accounts.usersmanager;
using ecn.accounts.usersmanager.Fakes;
using ECN.Accounts.Tests.Helper;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ShimApplicationLog = KM.Common.Entity.Fakes.ShimApplicationLog;
using ShimEntityUser = KMPlatform.Entity.Fakes.ShimUser;
using ShimUser = KMPlatform.BusinessLogic.Fakes.ShimUser;

namespace ECN.Accounts.Tests.main.Users
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class UserDetailTestSave : PageHelper
    {
        private const int Zero = 0;
        private const int AboveZero = 10;
        private const int Negative = -1;
        private const int UserId = 20;
        private const string IsActiveColumn = "IsActive";
        private const string IsDeletedColumn = "IsDeleted";
        private const string InactiveReasonColumn = "InactiveReason";
        private const string DisplayColumn = "Display";
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
        private const string RequestUserIdKey = "UserID";
        private const string Pending = "Pending";
        private userdetail _page;
        private PrivateObject _pagePrivate;
        private User _currentUser;
        private Exception _userSessionGetException;
        private NameValueCollection _appSettings;
        private bool _validateUserResult;
        private bool _emailValidResult;
        private Exception _doUserClientSecurityGroupsException;
        private string _applicationLogCriticalErrorSourceMethod;
        private readonly BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        [SetUp]
        public void Setup()
        {
            _appSettings = new NameValueCollection();
            _page = new userdetail();
            _applicationLogCriticalErrorSourceMethod = null;
            _validateUserResult = false;
            _pagePrivate = new PrivateObject(_page);
            InitializeAllControls(_page);
            CommonShims();
        }

        [Test]
        public void Save_UserWithoutFirstAndLastNames_ShouldShowError()
        {
            //Arrange
            _currentUser.FirstName = string.Empty;
            _currentUser.LastName = string.Empty;
            var lblErrorMessagePhError = GetReferenceField<Label>("lblErrorMessagePhError");
            var expectedMessage = "Before you can complete this action you need to update your own user profile " +
                "with first and last name. Your first and last name is used as the \"From Name\" in the email " +
                "invitation.";
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessagePhError.Text.ShouldContain(expectedMessage);
        }

        [Test]
        public void Save_WhenECNExceptionThrown_ShouldShowError()
        {
            //Arrange
            var error = new ECNError { ErrorMessage = GetString() };
            var errors = new List<ECNError> { error };
            _userSessionGetException = new ECNException(errors);
            var lblErrorMessage = GetReferenceField<Label>("lblErrorMessage");
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessage.Text.ShouldContain(error.ErrorMessage);
        }

        [Test]
        public void Save_WhenExceptionThrown_ShouldShowError()
        {
            //Arrange
            var errorMessage = "An error has occurred";
            _userSessionGetException = new Exception();
            var lblErrorMessagePhError = GetReferenceField<Label>("lblErrorMessagePhError");
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessagePhError.Text.ShouldContain(errorMessage);
        }

        [Test]
        public void Save_UseNotAdminAndHasNoRoles_ShouldShowError()
        {
            //Arrange
            var expectedMessage = "Please assign at least one role for this user";
            var lblErrorMessagePhError = GetReferenceField<Label>("lblErrorMessagePhError");
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessagePhError.Text.ShouldContain(expectedMessage);
        }

        [Test]
        public void Save_UseIsPlatformAdministratorAndRequestUserIdAboveZero_ValidateUser()
        {
            //Arrange
            QueryString[RequestUserIdKey] = Zero.ToString();
            var expectedMessage = "UserName already exists";
            var lblErrorMessagePhError = GetReferenceField<Label>("lblErrorMessagePhError");
            _currentUser.IsPlatformAdministrator = true;
            ShimEntityUser.Constructor = instance =>
            {
                instance.IsPlatformAdministrator = true;
            };
            _validateUserResult = true;
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessagePhError.Text.ShouldContain(expectedMessage);
        }

        [Test]
        public void Save_UseIsPlatformAdministratorAndRequestUserIdZero_ValidateUser()
        {
            //Arrange
            QueryString[RequestUserIdKey] = AboveZero.ToString();
            var expectedMessage = "UserName already exists";
            var lblErrorMessagePhError = GetReferenceField<Label>("lblErrorMessagePhError");
            _currentUser.IsPlatformAdministrator = true;
            _validateUserResult = true;
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessagePhError.Text.ShouldContain(expectedMessage);
        }

        [Test]
        public void Save_ValidatedUserWhenExceptionThrown_WillBeHandled()
        {
            //Arrange
            var customerId = AboveZero;
            var baseChannelId = AboveZero;
            _currentUser.DefaultClientID = customerId;
            _currentUser.DefaultClientGroupID = baseChannelId;
            _currentUser.IsPlatformAdministrator = false;
            var table = CreateUserRolesTable(GetString());
            var row = table.Rows[0];
            row[CustomerIdColumn] = customerId;
            row[IsActiveColumn] = false;
            row[BaseChannelIdColumn] = baseChannelId;
            SetUserRolesTable(table);
            //Act, Assert
            _page.Save(null, EventArgs.Empty);
        }

        [Test]
        public void Save_ValidatedUserAndDefaultClientIdAboveZero_ShouldUpdateCurrentUser()
        {
            //Arrange
            var customerId = AboveZero;
            var baseChannelId = AboveZero;
            _currentUser.DefaultClientID = customerId;
            _currentUser.DefaultClientGroupID = baseChannelId;
            _currentUser.IsPlatformAdministrator = false;
            var table = CreateUserRolesTable(GetString());
            var row = table.Rows[0];
            row[CustomerIdColumn] = customerId;
            row[IsActiveColumn] = false;
            row[BaseChannelIdColumn] = baseChannelId;
            SetUserRolesTable(table);
            row = table.NewRow();
            row[IsActiveColumn] = true;
            row[InactiveReasonColumn] = Pending;
            row[customerId] = AboveZero;
            table.Rows.Add(row);
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            _currentUser.DefaultClientGroupID.ShouldBe(AboveZero);
        }

        [Test]
        public void Save_ValidatedUserAndDefaultClientGroupIdAboveZero_ShouldUpdateCurrentUser()
        {
            //Arrange
            var customerId = AboveZero;
            var baseChannelId = AboveZero;
            _currentUser.DefaultClientID = customerId;
            _currentUser.DefaultClientGroupID = baseChannelId;
            _currentUser.IsPlatformAdministrator = false;
            var table = CreateUserRolesTable(GetString());
            var row = table.Rows[0];
            row[CustomerIdColumn] = customerId;
            row[IsActiveColumn] = false;
            row[BaseChannelIdColumn] = baseChannelId;
            SetUserRolesTable(table);
            row = table.NewRow();
            row[IsActiveColumn] = true;
            row[InactiveReasonColumn] = Pending;
            row[BaseChannelIdColumn] = AboveZero;
            row[CustomerIdColumn] = customerId;
            row[BaseChannelIdColumn] = baseChannelId;
            table.Rows.Add(row);
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            _currentUser.DefaultClientID.ShouldBe(AboveZero);
        }

        [Test]
        public void Save_ValidatedUserAndPlatformAdministrator_ShouldUpdateCurrentUser()
        {
            //Arrange
            QueryString[RequestUserIdKey] = Negative.ToString();
            var table = CreateUserRolesTable(GetString());
            SetUserRolesTable(table);
            var defaultClientId = AboveZero;
            var defaultClientGroupId = AboveZero + 1;
            _appSettings.Add("MasterClientID", defaultClientId.ToString());
            _appSettings.Add("MasterClientGroupID", defaultClientGroupId.ToString());
            User user = null;
            ShimEntityUser.Constructor = instance =>
            {
                instance.IsPlatformAdministrator = true;
                user = instance;
            };
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            user.ShouldNotBeNull();
            user.DefaultClientID.ShouldBe(defaultClientId);
            user.DefaultClientGroupID.ShouldBe(defaultClientGroupId);
        }

        [Test]
        public void Save_ValidatedUserAndPlatformAdministratorAndNotValidEmail_ShouldShowError()
        {
            //Arrange
            QueryString[RequestUserIdKey] = Negative.ToString();
            var table = CreateUserRolesTable(GetString());
            SetUserRolesTable(table);
            var defaultClientId = AboveZero;
            var defaultClientGroupId = AboveZero + 1;
            _appSettings.Add("MasterClientID", defaultClientId.ToString());
            _appSettings.Add("MasterClientGroupID", defaultClientGroupId.ToString());
            User user = null;
            ShimEntityUser.Constructor = instance =>
            {
                instance.IsPlatformAdministrator = true;
                user = instance;
            };
            _emailValidResult = false;
            var expectedMessge = "Email address is not valid";
            var lblErrorMessagePhError = GetReferenceField<Label>("lblErrorMessagePhError");
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            lblErrorMessagePhError.Text.ShouldContain(expectedMessge);
        }

        [Test]
        public void Save_ValidatedUserAndPlatformAdministratorAndValidEmail_ShouldUpdateUser()
        {
            //Arrange
            QueryString[RequestUserIdKey] = Negative.ToString();
            var table = CreateUserRolesTable(GetString());
            var row = table.NewRow();
            row[IsDeletedColumn] = false;
            row[CustomerIdColumn] = AboveZero;
            row[IsBCAdminColumn] = false;
            row[IsChannelRoleColumn] = false;
            row[BaseChannelIdColumn] = AboveZero;
            table.Rows.Add(row);
            SetUserRolesTable(table);
            var defaultClientId = AboveZero;
            var defaultClientGroupId = AboveZero + 1;
            _appSettings.Add("MasterClientID", defaultClientId.ToString());
            _appSettings.Add("MasterClientGroupID", defaultClientGroupId.ToString());
            User user = null;
            ShimEntityUser.Constructor = instance =>
            {
                instance.IsPlatformAdministrator = true;
                instance.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap()
                };
                user = instance;
            };
            _emailValidResult = true;


            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            RedirectUrl.ShouldBe("default.aspx");
        }

        [Test]
        public void Save_ValidatedUserAndPlatformAdministratorAndValidEmailAndTransactionException_ShouldLog()
        {
            //Arrange
            QueryString[RequestUserIdKey] = Negative.ToString();
            var table = CreateUserRolesTable(GetString());
            SetUserRolesTable(table);
            var defaultClientId = AboveZero;
            var defaultClientGroupId = AboveZero + 1;
            _appSettings.Add("MasterClientID", defaultClientId.ToString());
            _appSettings.Add("MasterClientGroupID", defaultClientGroupId.ToString());
            User user = null;
            ShimEntityUser.Constructor = instance =>
            {
                instance.IsPlatformAdministrator = true;
                instance.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap()
                };
                user = instance;
            };
            ShimTransactionScope.AllInstances.Complete = instance => { throw new Exception(); };
            _emailValidResult = true;
            var expectedMessge = "UserDetail.SaveUser - Error sending user opt in email";

            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            _applicationLogCriticalErrorSourceMethod.ShouldBe(expectedMessge);
            RedirectUrl.ShouldBe("default.aspx");
        }

        [Test]
        public void Save_ValidatedUserAndPlatformAdministratorAndValidEmailAndExcepiton_ShouldLog()
        {
            //Arrange
            QueryString[RequestUserIdKey] = Negative.ToString();
            var table = CreateUserRolesTable(GetString());
            SetUserRolesTable(table);
            var defaultClientId = AboveZero;
            var defaultClientGroupId = AboveZero + 1;
            _appSettings.Add("MasterClientID", defaultClientId.ToString());
            _appSettings.Add("MasterClientGroupID", defaultClientGroupId.ToString());
            User user = null;
            ShimEntityUser.Constructor = instance =>
            {
                instance.IsPlatformAdministrator = true;
                instance.UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap()
                };
                user = instance;
            };
            _emailValidResult = true;
            var expectedMessge = "UserDetail.btnSave_Click";
            _doUserClientSecurityGroupsException = new Exception();
            //Act
            _page.Save(null, EventArgs.Empty);
            //Assert
            _applicationLogCriticalErrorSourceMethod.ShouldBe(expectedMessge);
        }

        private void CommonShims()
        {
            _appSettings.Add("KMCommon_Application", $"{Zero}");
            QueryString.Add(RequestUserIdKey, $"{UserId}");
            var session = CreateUserSession();
            _currentUser = new User
            {
                UserName = GetString(),
                FirstName = GetString(),
                LastName = GetString()
            };
            session.CurrentUser = _currentUser;
            Shimuserdetail.AllInstances.MasterGet = instance =>
            {
                return new ShimAccounts
                {
                    UserSessionGet = () =>
                    {
                        ThrowExceptionIfExistsAndReset(ref _userSessionGetException);
                        return session;
                    }
                };
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, sourceMethod, applicationId, note, charityId, customerId) =>
                {
                    _applicationLogCriticalErrorSourceMethod = sourceMethod;
                    return Zero;
                };
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            ShimUser.AllInstances.SelectUserInt32Boolean = (instance, id, includeObjects) => _currentUser;
            ShimUser.AllInstances.Validate_UserNameStringInt32 = (instance, name, userId) => _validateUserResult;
            ShimClientGroupClientMap.AllInstances.SelectForClientIDInt32 = (instance, id) =>
                new List<ClientGroupClientMap>
                {
                    new ClientGroupClientMap
                    {
                        IsActive = true,
                        ClientGroupID = AboveZero
                    }
                };
            ShimClientGroupClientMap.AllInstances.SelectForClientGroupInt32 = (instance, id) =>
                new List<ClientGroupClientMap>
                {
                    new ClientGroupClientMap
                    {
                        IsActive =true,
                        ClientID = AboveZero
                    }
                };
            ShimEmail.IsValidEmailAddressString = email => _emailValidResult;
            Shimuserdetail.AllInstances.DoUserClientSecurityGroupsUserBoolean = (instance, user, isUpdate) =>
            {
                ThrowExceptionIfExistsAndReset(ref _doUserClientSecurityGroupsException);
                return new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap(),
                    new UserClientSecurityGroupMap()
                };
            };
            ShimUser.AllInstances.SaveUser = (instance, user) => AboveZero;
            ShimUserClientSecurityGroupMap.AllInstances.SelectForUserInt32 = (instance, id) =>
                new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap
                    {
                        IsActive = true,
                        ClientID = AboveZero
                    }
                };
        }

        private void ThrowExceptionIfExistsAndReset(ref Exception exception)
        {
            if (exception != null)
            {
                var backupException = exception;
                exception = null;
                throw backupException;
            }
        }

        private ECNSession CreateUserSession()
        {
            ShimECNSession.Constructor = instance => { };
            var constructor = typeof(ECNSession).GetConstructor(_flags, null, new Type[0], null);
            return constructor?.Invoke(new object[0]) as ECNSession;
        }

        private void SetUserRolesTable(DataTable table)
        {
            _pagePrivate.SetField("dtUserRoles", table);
        }

        private DataTable CreateUserRolesTable(
            string id,
            int? securityGroupId = null,
            int? baseChannelId = null,
            int? customerId = null)
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
            row[SecurityGroupIdColumn] = securityGroupId ?? AboveZero;
            row[BaseChannelIdColumn] = baseChannelId ?? AboveZero;
            row[CustomerIdColumn] = customerId ?? AboveZero;

            result.Rows.Add(row);
            return result;
        }

        private T GetReferenceField<T>(string name) where T : class
        {
            var result = _pagePrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}