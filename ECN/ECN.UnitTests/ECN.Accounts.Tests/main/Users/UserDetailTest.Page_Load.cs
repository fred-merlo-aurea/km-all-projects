using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.accounts.MasterPages.Fakes;
using ecn.accounts.usersmanager;
using ecn.accounts.usersmanager.Fakes;
using ECN.Accounts.Tests.Helper;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
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
    public class UserDetailTestPage_Load : PageHelper
    {
        private const int Zero = 0;
        private const int One = 1;
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
        private const string UserRolesTableField = "dtUserRoles";
        private IDisposable _shimsContext;
        private userdetail _userDetail;
        private PrivateObject _userDetailPrivate;
        private User _currentUser;
        private NameValueCollection _queryString;
        private bool _isPostBack;
        private string _redirectUrl;
        private GridView _gvUserRoles;
        private readonly Random _random = new Random();
        private readonly BindingFlags _flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        [SetUp]
        public void Setup()
        {
            _queryString = new NameValueCollection();
            _shimsContext = ShimsContext.Create();
            CommonShims();
            _userDetail = new userdetail();
            _userDetailPrivate = new PrivateObject(_userDetail);
            base.InitializeAllControls(_userDetail);
            InitializeFields();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void Page_Load_UserNotAdmin_ShouldRedirectToErrorPage()
        {
            //Arrange, Act
            CallPage_Load();
            //Assert
            _redirectUrl.ShouldBe("~/main/securityAccessError.aspx");
        }

        [Test]
        public void Page_Load_UserAdminPostBackAndUserRolesNull_ShouldCreateUserRolesTable()
        {
            //Arrange
            const int ColumnsCount = 15;
            _currentUser.IsActive = true;
            _currentUser.IsPlatformAdministrator = true;
            _isPostBack = true;
            _userDetailPrivate.SetField(UserRolesTableField, null);
            //Act
            CallPage_Load();
            //Assert
            var rolesTable = GetReferenceField<DataTable>(UserRolesTableField);
            rolesTable.ShouldSatisfyAllConditions(
                () => rolesTable.ShouldNotBeNull(),
                () => rolesTable.Columns.Count.ShouldBe(ColumnsCount));
        }

        [Test]
        public void Page_Load_RolesGridHasRowsAndPostBack_ShouldAdThemToRolesTable()
        {
            //Arrange
            _currentUser.IsActive = true;
            _currentUser.IsPlatformAdministrator = true;
            _isPostBack = true;
            var rolesTable = CreateUserRolesTable(GetString());
            var row = rolesTable.Rows[0];
            _gvUserRoles.DataSource = rolesTable;
            _gvUserRoles.DataBind();
            var ids = new[]
            {
                "hfID",
                "hfBaseChannel",
                "hfBaseChannelID",
                "hfCustomer",
                "hfCustomerID",
                "hfRole",
                "hfSecurityGroupID",
                "hfInactiveReason",
                "hfIsBCAdmin",
                "hfIsCAdmin",
                "hfIsActive",
                "hfIsDeleted",
                "hfDisplay",
                "hfDoHardDelete",
                "hfIsChannelRole"
            };
            foreach (var id in ids)
            {
                var control = new HiddenField
                {
                    ID = id,
                    Value = (row[id.Replace("hf", string.Empty)] ?? string.Empty).ToString()
                };
                var tableCell = new TableCell();
                tableCell.Controls.Add(control);
                _gvUserRoles.Rows[0].Cells.Add(tableCell);
            }
            SetUserRolesTable(null);
            //Act
            CallPage_Load();
            //Assert
            var rolesTableField = GetReferenceField<DataTable>(UserRolesTableField);
            rolesTableField.ShouldNotBeNull();
            rolesTableField.Rows.ShouldNotBeNull();
            rolesTableField.ShouldSatisfyAllConditions(
                ()=>rolesTableField.Rows.Count.ShouldBe(One),
                ()=>
                {
                    var fieldRow = rolesTableField.Rows[0];
                    fieldRow.ItemArray.ShouldBeSubsetOf(row.ItemArray);
                });
        }

        [Test]
        [TestCase(true, false, true, true, true, false, true)]
        [TestCase(true, false, false, false, false, true, true)]
        [TestCase(false, true, true, true, true, false, false)]
        [TestCase(false, true, false, false, false, true, false)]
        [TestCase(false, false, true, true, true, false, false)]
        [TestCase(false, false, false, false, false, true, false)]
        public void Page_Load_UserAdminAndNotPostBack_ShoulInitializeControls(
            bool isPlatformAdmin,
            bool isChannelAdmin,
            bool hasUserId,
            bool showResetPasswrod,
            bool showPasswordText,
            bool enableUserName,
            bool enableStatus)
        {
            //Arrange
            _currentUser.IsActive = true;
            _currentUser.IsPlatformAdministrator = isPlatformAdmin;
            if (isChannelAdmin)
            {
                _currentUser.CurrentSecurityGroup = new SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.ChannelAdministrator
                };
            }
            if (!isPlatformAdmin)
            {
                Shimuserdetail.AllInstances.LoadUser = instance => { };
            }
            if (!isChannelAdmin && !isPlatformAdmin)
            {
                _currentUser.CurrentSecurityGroup = new SecurityGroup
                {
                    AdministrativeLevel = SecurityGroupAdministrativeLevel.Administrator
                };
            }
            var userId = hasUserId
                ? GetNumber()
                : Zero;
            _queryString.Add("UserID", userId.ToString());
            _isPostBack = false;
            var btnResetPassword = GetReferenceField<Button>("btnResetPassword");
            var txtPassword = GetReferenceField<TextBox>("txtPassword");
            var txtUserName = GetReferenceField<TextBox>("txtUserName");
            var ddlStatus = GetReferenceField<DropDownList>("ddlStatus");
            //Act
            CallPage_Load();
            //Assert
            btnResetPassword.Visible.ShouldBe(showResetPasswrod);
            txtPassword.Visible.ShouldBe(showPasswordText);
            txtUserName.Enabled.ShouldBe(enableUserName);
            ddlStatus.Enabled.ShouldBe(enableStatus);
        }

        private void CommonShims()
        {
            var session = CreateUserSession();
            var clientId = GetNumber();
            _currentUser = new User
            {
                CurrentSecurityGroup = new SecurityGroup(),
                UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                {
                    new UserClientSecurityGroupMap
                    {
                        ClientID = clientId
                    }
                }
            };
            session.CurrentUserClientGroupClients = new List<Client>
            {
                new Client
                {
                    ClientID = clientId
                }
            };
            session.CurrentUser = _currentUser;
            Shimuserdetail.AllInstances.MasterGet = instance =>
            {
                return new ShimAccounts
                {
                    UserSessionGet = () => session
                };
            };
            ShimPage.AllInstances.RequestGet = instance =>
            {
                return new HttpRequest("test", "http://km.com", string.Empty);
            };
            ShimHttpRequest.AllInstances.QueryStringGet = (r) =>
            {
                return _queryString;
            };
            ShimPage.AllInstances.ResponseGet = instance => new HttpResponse(TextWriter.Null);
            ShimPage.AllInstances.IsPostBackGet = instance => _isPostBack;
            ShimHttpResponse.AllInstances.RedirectString = (instacne, url) => _redirectUrl = url;
            ShimUser.AllInstances.SelectUserInt32Boolean = (instance, id, includeObjects) =>
            {
                return _currentUser;
            };
            ShimClient.AllInstances.SelectInt32Boolean = (instance, id, includeObjects) =>
            {
                return new Client
                {
                    ClientID = clientId
                };
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
            ShimClientGroup.AllInstances.SelectInt32Boolean = (insta, id, includeObjects) =>
            {
                return new ClientGroup();
            };
            ShimSecurityGroup.AllInstances.SelectInt32BooleanBoolean = (instan, goupId, isKmUser, includeObjects) =>
            {
                return new SecurityGroup();
            };
            ShimUserGroup.GetInt32 = id => new List<UserGroup>();
            ShimUpdatePanel.AllInstances.Update = instance => { };
            ShimClient.AllInstances.SelectForClientGroupLiteInt32Boolean = (instance, id, include) =>
            {
                return new List<Client>
                {
                    new Client()
                };
            };
        }

        private ECNSession CreateUserSession()
        {
            ShimECNSession.Constructor = instance => { };
            var constructor = typeof(ECNSession).GetConstructor(_flags, null, new Type[0], null);
            return constructor?.Invoke(new object[0]) as ECNSession;
        }

        private void CallPage_Load()
        {
            _userDetailPrivate.Invoke("Page_Load", new object[] { null, EventArgs.Empty });
        }

        private void SetUserRolesTable(DataTable table)
        {
            _userDetailPrivate.SetField(UserRolesTableField, table);
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
            foreach (var column in columns)
            {
                if (column.DataType.IsValueType)
                {
                    row[column.ColumnName] = Activator.CreateInstance(column.DataType);
                }
                else
                {
                    row[column.ColumnName] = GetString();
                }
            }
            row[IdColumn] = id;
            row[SecurityGroupIdColumn] = securityGroupId ?? GetNumber();
            row[BaseChannelIdColumn] = baseChannelId ?? GetNumber();
            row[CustomerIdColumn] = customerId ?? GetNumber();

            result.Rows.Add(row);
            return result;
        }

        private void InitializeFields()
        {
            _gvUserRoles = GetReferenceField<GridView>("gvUserRoles");
        }

        private T GetReferenceField<T>(string name) where T : class
        {
            var result = _userDetailPrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
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
