using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.SessionState;
using System.Web.SessionState.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ECN_Framework.Accounts.Entity.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Accounts.Enums;
using AccountsMasterPage = ecn.accounts.MasterPages.Accounts;
using EntityShimUser = KMPlatform.Entity.Fakes.ShimUser;
using Menu = System.Web.UI.WebControls.Menu;
using KMShimUser= KM.Platform.Fakes.ShimUser;
using PrivateObject = Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject;
using Staff = ECN_Framework.Accounts.Entity.Staff;

namespace ECN.Accounts.Tests.MasterPages
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class AccountsTests
    {
        private const string SiteMapSecnodLevelFieldName = "SiteMapSecondLevel";
        private const int Zero = 0;
        private AccountsMasterPage _accountsMasterPage;
        private PrivateObject _accountsMasterPagePrivate;
        private IDisposable _shimsContext;
        private SiteMapDataSource _siteMapSecondLevelField;
        private Menu _menuField;

        [SetUp]
        public void Setup()
        {
            _accountsMasterPage = new AccountsMasterPage();
            _accountsMasterPagePrivate = new PrivateObject(_accountsMasterPage);
            _siteMapSecondLevelField = new SiteMapDataSource();
            _menuField = new Menu();
            _shimsContext = ShimsContext.Create();
            InitializeFields();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
            _accountsMasterPage.Dispose();
        }

        [Test]
        public void Menu_MenuItemDataBound_HasSiteMapCurrentNodeAndItemSelectedWithParent_SetStartingUrlToParentUrl()
        {
            // Arrange
            const string ParentUrl = "Http://SomeDomain.Extension";
            var parentMenuItem = new MenuItem
            {
                Selectable = true,
                NavigateUrl = ParentUrl
            };
            var menuItem = new MenuItem
            {
                Selected = true,
            };
            parentMenuItem.ChildItems.Add(menuItem);
            ShimSiteMapCurrentNode(false);
            ShimCurrentUser();

            // Act
            CallMenu_MenuItemDataBound(menuItem);

            // Assert
            parentMenuItem.Selected.ShouldBeTrue();
            _siteMapSecondLevelField.StartingNodeUrl.ShouldBe(ParentUrl);
        }

        [Test]
        public void Menu_MenuItemDataBound_HasSiteMapCurrentNodeAndItemSelectedWithoutParent_SetStartingUrlToCurrentNodeUrl()
        {
            // Arrange
            const string SiteMapCurrentNodeUrl = "Http://SomeItemUrl";
            var menuItem = new MenuItem
            {
                Selected = true,
            };
            ShimSiteMapCurrentNode(false, SiteMapCurrentNodeUrl);
            ShimCurrentUser();

            // Act
            CallMenu_MenuItemDataBound(menuItem);

            // Assert
            _siteMapSecondLevelField.StartingNodeUrl.ShouldBe(SiteMapCurrentNodeUrl);
        }

        [Test]
        public void Menu_MenuItemDataBound_HasSiteMapCurrentNodeAndItemNotSelectedAndHome_SetStartingUrlToMain()
        {
            // Arrange
            const string HomePage = "HOME";
            const string MainUrl = "/ecn.accounts/main/";
            var menuItem = new MenuItem(HomePage);
            menuItem.Selected = false;
            ShimSiteMapCurrentNode(false);
            ShimCurrentUser();

            // Act
            CallMenu_MenuItemDataBound(menuItem);

            // Assert
            _siteMapSecondLevelField.StartingNodeUrl.ShouldBe(MainUrl);
        }

        [Test]
        public void Menu_MenuItemDataBound_CurrentMenuCode_ShouldSetAppropriateStartingNodeUrl()
        {
            // Arrange
            var inputAndExpectedOutputs = new[]
            {
                new
                {
                    MenuCode = MenuCode.USERS,
                    StartingNodeUrl="/ecn.accounts/main/users/"
                },
                new
                {
                    MenuCode = MenuCode.CUSTOMERS,
                    StartingNodeUrl="/ecn.accounts/main/customers/"
                },
                new
                {
                    MenuCode = MenuCode.LEADS,
                    StartingNodeUrl="/ecn.accounts/main/Leads/"
                },
                new
                {
                    MenuCode = MenuCode.BILLINGSYSTEM,
                    StartingNodeUrl="/ecn.accounts/main/billingSystem/"
                },
                new
                {
                    MenuCode = MenuCode.CHANNELS,
                    StartingNodeUrl="/ecn.accounts/main/channels/"
                },
                new
                {
                    MenuCode = MenuCode.REPORTS,
                    StartingNodeUrl="/ecn.accounts/main/reports/"
                },
                new
                {
                    MenuCode = MenuCode.INDEX,
                    StartingNodeUrl="/ecn.accounts/main/"
                },
                new
                {
                    MenuCode = MenuCode.NOTIFICATION,
                    StartingNodeUrl="/ecn.accounts/main/"
                }
            };
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            KMShimUser.IsAdministratorUser = (_) => true;
            EntityShimUser.AllInstances.CurrentClientGet = (_) => new Client();
            EntityShimUser.AllInstances.IsKMStaffGet = (_) => true;

            foreach (var input in inputAndExpectedOutputs)
            {
                var menuItem = new MenuItem(input.MenuCode.ToString());
                _accountsMasterPage.CurrentMenuCode = input.MenuCode;

                // Act
                CallMenu_MenuItemDataBound(menuItem);


                // Assert
                _siteMapSecondLevelField.StartingNodeUrl.ShouldBe(input.StartingNodeUrl);
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_RightMenuTextAndCurrentMenuCode_ShouldSetItemSelected()
        {
            // Arrange
            var inputs = new[]
            {
                new
                {
                    ItemText = "users",
                    MenuCode =MenuCode.USERS
                },
                new
                {
                    ItemText = "customers",
                    MenuCode =MenuCode.CUSTOMERS
                },
                new
                {
                    ItemText = "leads",
                    MenuCode =MenuCode.LEADS
                },
                new
                {
                    ItemText = "billing",
                    MenuCode =MenuCode.BILLINGSYSTEM
                },
                new
                {
                    ItemText = "channel partners",
                    MenuCode =MenuCode.CHANNELS
                },
                new
                {
                    ItemText = "reports",
                    MenuCode =MenuCode.REPORTS
                },
                new
                {
                    ItemText = "notifications",
                    MenuCode =MenuCode.NOTIFICATION
                },
            };
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            foreach (var input in inputs)
            {
                var menuItem = new MenuItem(input.ItemText);
                _accountsMasterPage.CurrentMenuCode = input.MenuCode;

                // Act
                CallMenu_MenuItemDataBound(menuItem);

                // Assert
                menuItem.Selected.ShouldBeTrue();
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_UsersMenuForNonAdministrators_ShouldBeRemoved()
        {
            // Arrange
            const string UsersMenu = "users";
            var users = new[]
            {
                new User(),
                new User
                {
                    IsActive = true,
                    IsPlatformAdministrator = true,
                    CurrentClient = new Client()
                }
            };
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            foreach (var user in users)
            {
                _accountsMasterPage.UserSession.CurrentUser = user;
                var menuItem = new MenuItem(UsersMenu);
                _menuField.Items.Clear();
                _menuField.Items.Add(menuItem);

                // Act
                CallMenu_MenuItemDataBound(menuItem);

                // Assert
                _menuField.Items.Count.ShouldBe(Zero);
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_AddNewUser_ShouldSetUrlToUserDetails()
        {
            // Arrange
            const string AddNewUSerMenu = "add new user";
            const string UserDetailsUrl = "/ecn.accounts/main/users/userdetail.aspx";
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            var menuItem = new MenuItem(AddNewUSerMenu);

            // Act
            CallMenu_MenuItemDataBound(menuItem);

            // Assert
            menuItem.NavigateUrl.ShouldBe(UserDetailsUrl);
        }

        [Test]
        public void Menu_MenuItemDataBound_AdministrativeItemsAndNoneSystemAdministrators_ShouldBeRemovedFromMenu()
        {
            // Arrange
            var items = new[]
            {
                "customers",
                "billing",
                "channel partners",
                "digital edition",
                "notifications",
                "view all roles",
                "create new role",
                "reports"
            };
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            foreach (var item in items)
            {
                var menuItem = new MenuItem(item);
                _menuField.Items.Add(menuItem);

                // Act
                CallMenu_MenuItemDataBound(menuItem);

                // Assert
                _menuField.Items.Count.ShouldBe(Zero);
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_AdministrativeItemsAndNoneSystemAdministrators_ShouldBeRemovedFromParent()
        {
            // Arrange
            var items = new[]
            {
                "add channel partner",
                "channel report",
                "ecn today",
                //"channel Look", //BUG this case will never tested as .ToLower() and check for L
                "billing report",
                "disk space",
            };
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            foreach (var item in items)
            {
                var parentMenu = new MenuItem();
                var menuItem = new MenuItem(item);
                parentMenu.ChildItems.Add(menuItem);

                // Act
                CallMenu_MenuItemDataBound(menuItem);

                // Assert
                parentMenu.ChildItems.Count.ShouldBe(Zero);
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_LeadsMenuForNonSystemAdministrators_ShouldBeRemoved()
        {
            // Arrange
            const string LeadsMenu = "leads";
            var menuItem = new MenuItem(LeadsMenu);
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            _menuField.Items.Add(menuItem);
            _accountsMasterPage.UserSession.CurrentUser.IsKMStaff = true;

            // Act
            CallMenu_MenuItemDataBound(menuItem);

            // Assert
            _menuField.Items.Count.ShouldBe(Zero);
        }

        [Test]
        public void Menu_MenuItemDataBound_DemoSetupMenuForNonSystemAdministrators_ShouldBeRemoved()
        {
            // Arrange
            const string DemoSetupMenu = "demo setup";
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            _accountsMasterPage.UserSession.CurrentUser.IsKMStaff = true;
            var roles = new[]
            {
                StaffRoleEnum.DemoManager,
                StaffRoleEnum.AccountManager
            };
            foreach (var role in roles)
            {
                var parentItem = new MenuItem();
                var menuItem = new MenuItem(DemoSetupMenu);
                parentItem.ChildItems.Add(menuItem);
                ShimStaffRoles(role);

                // Act
                CallMenu_MenuItemDataBound(menuItem);

                // Assert
                parentItem.ChildItems.Count.ShouldBe(Zero);
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_DemoManagerMenuForNonSystemAdministrators_ShouldBeRemoved()
        {
            // Arrange
            const string DemoManagerMenu = "demo manager";
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            _accountsMasterPage.UserSession.CurrentUser.IsKMStaff = true;
            var roles = new[]
            {
                StaffRoleEnum.DemoManager,
                StaffRoleEnum.AccountManager
            };
            foreach (var role in roles)
            {
                var parentItem = new MenuItem();
                var menuItem = new MenuItem(DemoManagerMenu);
                parentItem.ChildItems.Add(menuItem);
                ShimStaffRoles(role);

                // Act
                CallMenu_MenuItemDataBound(menuItem);

                // Assert
                parentItem.ChildItems.Count.ShouldBe(Zero);
            }
        }

        [Test]
        public void Menu_MenuItemDataBound_ItemsToHideFromNonKMStaffAndNonAdministrators_ShouldBeRemovedFromParent()
        {
            // Arrange
            var items = new[]
            {
                //"ir-km Clicks", //BUG This case is not reachable because of ToLower() and C
                "km clicks",
                "account intensity",
                "total blasts for day",
                "blast status"
            };
            var users = new[]
            {
                new User
                {
                    IsActive = true,
                    IsPlatformAdministrator = true
                },
                new User
                {
                    IsKMStaff = false
                }
            };
            ShimSiteMapCurrentNode();
            ShimSession();
            ShimCurrentUser();
            foreach (var item in items)
            {
                foreach (var user in users)
                {
                    _accountsMasterPage.UserSession.CurrentUser = user;
                    var menuItem = new MenuItem(item);
                    var parent = new MenuItem();
                    parent.ChildItems.Add(menuItem);

                    // Act
                    CallMenu_MenuItemDataBound(menuItem);

                    // Assert
                    parent.ChildItems.Count.ShouldBe(Zero);
                }
            }
        }

        private void ShimStaffRoles(StaffRoleEnum staffRole)
        {
            var staff = new Staff
            {
                Roles = (int)staffRole
            };
            ShimStaff.CurrentStaffGet = () => staff;
        }

        private void ShimCurrentUser()
        {
            ShimECNSession.Constructor = (instance) => { };
            var ecnSession = CreateECNSession();
            ShimECNSession.CurrentSession = () =>
            {
                ecnSession.CurrentUser = new User();
                return ecnSession;
            };
        }

        private ECNSession CreateECNSession()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var result = typeof(ECNSession).GetConstructor(flags, null, new Type[0], null)
                ?.Invoke(new object[0]) as ECNSession;
            return result;
        }

        private void ShimSession()
        {
            var session = new Dictionary<string, object>();
            ShimHttpSessionState.AllInstances.ItemSetStringObject = (instance, key, item) =>
            {
                session[key] = item;
            };
            ShimHttpSessionState.AllInstances.ItemGetString = (instance, key) =>
            {
                return session[key];
            };
            var sessionObject = CreateSessionObject();
            ShimUserControl.AllInstances.SessionGet = instance =>
            {
                return sessionObject;
            };
        }

        private HttpSessionState CreateSessionObject()
        {
            var sessionState = new StubIHttpSessionState();
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var session = typeof(HttpSessionState)
                .GetConstructor(flags, null, new[] { typeof(IHttpSessionState) }, null)
                ?.Invoke(new object[] { sessionState }) as HttpSessionState;
            return session;
        }

        private void InitializeFields()
        {
            _accountsMasterPagePrivate.SetField(SiteMapSecnodLevelFieldName, _siteMapSecondLevelField);
            _accountsMasterPagePrivate.SetField("Menu", _menuField);
        }

        private void ShimSiteMapCurrentNode(bool returnsNull = true, string url = "")
        {
            ShimSiteMap.CurrentNodeGet = () =>
            {
                if (returnsNull)
                {
                    return null;
                }
                else
                {
                    var provider = new StubSiteMapProvider();
                    return new SiteMapNode(provider, Guid.NewGuid().ToString(), url);
                }
            };
        }

        private void CallMenu_MenuItemDataBound(MenuItem menuItem)
        {
            const string MethodName = "Menu_MenuItemDataBound";
            var eventArguments = new MenuEventArgs(menuItem);
            _accountsMasterPagePrivate.Invoke(MethodName, new object[] { null, eventArguments });
        }
    }
}