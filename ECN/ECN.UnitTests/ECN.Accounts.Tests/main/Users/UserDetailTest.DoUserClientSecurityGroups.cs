using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ecn.accounts.MasterPages.Fakes;
using ecn.accounts.usersmanager;
using ecn.accounts.usersmanager.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static KMPlatform.Enums;

namespace ECN.Accounts.Tests.main.Users
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class UserDetailTestDoUserClientSecurityGroups
    {
        private const int Zero = 0;
        private const int AboveZero = 123456;
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
        private const string Pending = "pending";
        private IDisposable _shimsContext;
        private userdetail _userDetail;
        private PrivateObject _userDetailPrivate;
        private int _userClientSecurityGroupMapDeletedId;
        private UserClientSecurityGroupMap _userClientSecurityGroupMapSaved;
        private User _currentUser;
        private List<UserClientSecurityGroupMap> _userClientSecurityGroupMapSelectForUserResult;
        private SecurityGroup _securityGroupResult;
        private Client _clientResult;
        private readonly Random _random = new Random();
        private readonly BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        [SetUp]
        public void Setup()
        {
            _shimsContext = ShimsContext.Create();
            CommonShims();
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
        public void DoUserClientSecurityGroups_UserIsPlatformAdmin_ShouldDeleteGroupMaps()
        {
            //Arrange
            var id = GetNumber();
            var table = CreateUserRolesTable(id.ToString());
            SetUserRolesTable(table);
            var user = new User
            {
                IsPlatformAdministrator = true
            };
            //Act
            var result = CallDoUserClientSecurityGroups(user, true);
            //Assert
            _userClientSecurityGroupMapDeletedId.ShouldBe(id);
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Test]
        public void DoUserClientSecurityGroups_UserIsNotPlatformAdminActiveAndNotDelete_ShouldBeSaved()
        {
            //Arrange
            var id = GetNumber();
            var securityGroupId = GetNumber();
            var table = CreateUserRolesTable(id.ToString(), securityGroupId);
            var row = table.Rows[0];
            row[IsActiveColumn] = true;
            row[IsDeletedColumn] = false;
            SetUserRolesTable(table);
            var user = new User
            {
                IsPlatformAdministrator = false,
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap
                    {
                        UserClientSecurityGroupMapID = id
                    }
                }
            };
            //Act
            var result = CallDoUserClientSecurityGroups(user, true);
            //Assert
            _userClientSecurityGroupMapSaved.ShouldNotBeNull();
            _userClientSecurityGroupMapSaved.SecurityGroupID.ShouldBe(securityGroupId);
        }

        [Test]
        public void DoUserClientSecurityGroups_UserNotPlatformAdminNotActiveHardDeleted_ShouldBeDeleted()
        {
            //Arrange
            var id = GetNumber();
            var securityGroupId = GetNumber();
            var table = CreateUserRolesTable(id.ToString(), securityGroupId);
            var row = table.Rows[0];
            row[IsActiveColumn] = false;
            row[IsDeletedColumn] = true;
            row[DoHardDeleteColumn] = true;
            SetUserRolesTable(table);
            var user = new User
            {
                IsPlatformAdministrator = false,
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap
                    {
                        UserClientSecurityGroupMapID = id
                    }
                }
            };
            //Act
            var result = CallDoUserClientSecurityGroups(user, true);
            //Assert
            _userClientSecurityGroupMapDeletedId.ShouldBe(id);
        }

        [Test]
        public void DoUserClientSecurityGroups_UserNotPlatformAdminNotActiveDeleted_ShouldBeSaved()
        {
            //Arrange
            var id = GetNumber();
            var securityGroupId = GetNumber();
            var table = CreateUserRolesTable(id.ToString(), securityGroupId);
            var row = table.Rows[0];
            row[IsActiveColumn] = false;
            row[IsDeletedColumn] = true;
            row[DoHardDeleteColumn] = false;
            SetUserRolesTable(table);
            var user = new User
            {
                IsPlatformAdministrator = false,
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap
                    {
                        UserClientSecurityGroupMapID = id,
                        InactiveReason = Pending
                    }
                }
            };
            _currentUser.UserID = GetNumber();
            //Act
            var result = CallDoUserClientSecurityGroups(user, true);
            //Assert
            _userClientSecurityGroupMapSaved.ShouldNotBeNull();
            _userClientSecurityGroupMapSaved.IsActive.ShouldBeFalse();
            _userClientSecurityGroupMapSaved.InactiveReason.ShouldBe("Disabled");
            _userClientSecurityGroupMapSaved.UpdatedByUserID.ShouldBe(_currentUser.UserID);
        }

        [Test]
        [TestCase(true, false, Zero, false, true)]
        [TestCase(true, false, Zero, false, false)]
        [TestCase(false, true, Zero, false, true)]
        [TestCase(false, true, Zero, false, false)]
        [TestCase(false, false, AboveZero, true, true)]
        [TestCase(false, false, AboveZero, true, false)]
        [TestCase(false, false, Zero, false, true)]
        [TestCase(false, false, Zero, false, false)]
        public void DoUserClientSecurityGroups_UserNotPlatformAdmin_ShouldReturnGroupMaps(
            bool isBCAdmin,
            bool isCAAdmin,
            int baseChannelId,
            bool isChannelRole,
            bool returnGroupMaps)
        {
            //Arrange
            var id = GetNumber();
            var securityGroupId = GetNumber();
            var customerId = _clientResult.ClientID;
            var table = CreateUserRolesTable(id.ToString(), securityGroupId, customerId:customerId);
            var row = table.Rows[0];
            row[IsActiveColumn] = false;
            row[IsDeletedColumn] = false;
            row[IsBCAdminColumn] = isBCAdmin;
            row[IsCAdminColumn] = isCAAdmin;
            row[IsChannelRoleColumn] = isChannelRole;
            row[InactiveReasonColumn] = Pending;
            SetUserRolesTable(table);
            _securityGroupResult.SecurityGroupID = securityGroupId;
            _securityGroupResult.ClientID = customerId;
            if (returnGroupMaps)
            {
                _securityGroupResult.SecurityGroupID = securityGroupId;
                _userClientSecurityGroupMapSelectForUserResult.Add(new UserClientSecurityGroupMap
                {
                    ClientID = _clientResult.ClientID,
                    SecurityGroupID = _securityGroupResult.SecurityGroupID
                });
            }
            var user = new User
            {
                UserID = GetNumber(),
                IsPlatformAdministrator = false,
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap
                    {
                        UserClientSecurityGroupMapID = id,
                        InactiveReason = Pending
                    }
                }
            };
            _currentUser.UserID = GetNumber();
            //Act
            var result = CallDoUserClientSecurityGroups(user, false);
            //Assert
            _userClientSecurityGroupMapSaved.ShouldNotBeNull();
            _userClientSecurityGroupMapSaved.ShouldSatisfyAllConditions(
                () => _userClientSecurityGroupMapSaved.ClientID.ShouldBe(_clientResult.ClientID),
                () => _userClientSecurityGroupMapSaved.InactiveReason.ShouldBe(row[InactiveReasonColumn]),
                () => _userClientSecurityGroupMapSaved.IsActive.ShouldBe(row[IsActiveColumn]),
                () => _userClientSecurityGroupMapSaved.UserID.ShouldBe(user.UserID));
           if(returnGroupMaps)
            {
                result.ShouldSatisfyAllConditions(
                    () => result.ShouldNotBeEmpty(),
                    () => result.ShouldHaveSingleItem());
                var item = result.Single();
                _userClientSecurityGroupMapSaved.ShouldBe(item);
            }
        }

        private void CommonShims()
        {
            ShimUserClientSecurityGroupMap.AllInstances.DeleteInt32 = (instance, id) =>
            {
                _userClientSecurityGroupMapDeletedId = id;
            };
            ShimUserClientSecurityGroupMap.AllInstances.SaveUserClientSecurityGroupMap = (instance, groupMap) =>
            {
                _userClientSecurityGroupMapSaved = groupMap;
                return GetNumber();
            };
            var session = CreateUserSession();
            _currentUser = new User();
            session.CurrentUser = _currentUser;
            Shimuserdetail.AllInstances.MasterGet = instance =>
            {
                return new ShimAccounts
                {
                    UserSessionGet = () => session
                };
            };
            _userClientSecurityGroupMapSelectForUserResult = new List<UserClientSecurityGroupMap>();
            ShimUserClientSecurityGroupMap.AllInstances.SelectForUserInt32 = (instance, id) =>
            {
                return _userClientSecurityGroupMapSelectForUserResult;
            };
            ShimSecurityGroupOptIn.AllInstances.SelectBySecurityGroup_UserIDInt32Int32 = (instance, groupId, userId) =>
            {
                return new List<SecurityGroupOptIn>
                {
                    new SecurityGroupOptIn()
                };
            };
            ShimSecurityGroupOptIn.AllInstances.DeleteInt32 = (instance, gorupId) => { };
            ShimSecurityGroupOptIn.AllInstances.DeleteInt32Int32 = (instance, gorupId, userId) => { };
            _securityGroupResult = new SecurityGroup
            {
                AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator,
                SecurityGroupID = GetNumber()
            };
            ShimSecurityGroup.AllInstances.SelectForClientGroupInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    return new List<SecurityGroup>
                    {
                        _securityGroupResult
                    };
                };
            ShimSecurityGroup.AllInstances.SelectForClientInt32Boolean =
                (instance, clientGroupId, includeServices) =>
                {
                    _securityGroupResult.AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator;
                    return new List<SecurityGroup>
                    {
                        _securityGroupResult
                    };
                };
            _clientResult = new Client
            {
                ClientID = GetNumber()
            };
            ShimClient.AllInstances.SelectForClientGroupLiteInt32Boolean = (instance, clientGroupId, includeObjects) =>
            {
                return new List<Client>
                {
                    _clientResult
                };
            };
            ShimClient.AllInstances.SelectForClientGroupInt32Boolean =(instance, clientGroupId, includeObjects) =>
            {
                return new List<Client>
                {
                    _clientResult
                };
            };
        }

        private ECNSession CreateUserSession()
        {
            ShimECNSession.Constructor = instance => { };
            var constructor = typeof(ECNSession).GetConstructor(_flags, null, new Type[0], null);
            return constructor?.Invoke(new object[0]) as ECNSession;
        }

        private List<UserClientSecurityGroupMap> CallDoUserClientSecurityGroups(User user, bool isUpdate)
        {
            var result = _userDetailPrivate.Invoke("DoUserClientSecurityGroups", new object[] { user, isUpdate });
            return result as List<UserClientSecurityGroupMap>;
        }

        private void SetUserRolesTable(DataTable table)
        {
            _userDetailPrivate.SetField("dtUserRoles", table);
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
            row[SecurityGroupIdColumn] = securityGroupId ?? GetNumber();
            row[BaseChannelIdColumn] = baseChannelId ?? GetNumber();
            row[CustomerIdColumn] = customerId ?? GetNumber();

            result.Rows.Add(row);
            return result;
        }

        private void InitializeFields()
        {
            var fields = _userDetail.GetType()
                .GetFields(_flags)
                .Where(field => field.GetValue(_userDetail) == null)
                .ToList();
            foreach (var field in fields)
            {
                field.SetValue(_userDetail, CreateInstance(field.FieldType));
            }
        }

        private object CreateInstance(Type type)
        {

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                return type.GetConstructor(_flags, null, new Type[0], null)
                    ?.Invoke(new object[0]);
            }
        }

        private int GetNumber()
        {
            return _random.Next(10, 1000);
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
