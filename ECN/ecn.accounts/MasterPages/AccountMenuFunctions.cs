using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ECN_Framework.Accounts.Entity;
using ECN_Framework_BusinessLayer.Application;
using KM.Platform;
using MenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode;

namespace ecn.accounts.MasterPages
{
    public class AccountMenuFunctions
    {
        private const string Home = "HOME";
        private const string Users = "users";
        private const string AddNewUser = "add new user";
        private const string Leads = "leads";
        private const string DemoSetup = "demo setup";
        private const string DemoManager = "demo manager";
        private const string Customers = "customers";
        private const string Billing = "billing";
        private const string ChannelPartners = "channel partners";
        private const string Notifications = "notifications";
        private const string AddChannelPartner = "add channel partner";
        private const string ChannelReport = "channel report";
        private const string EcnToday = "ecn today";
        private const string ChannelLook = "channel Look";
        private const string BillingReport = "billing report";
        private const string DiskSpace = "disk space";
        private const string DigitalEdition = "digital edition";
        private const string Reports = "reports";
        private const string IrKmClicks = "ir-km Clicks";
        private const string KmClicks = "km clicks";
        private const string AccountIntensity = "account intensity";
        private const string TotalBlastsForDay = "total blasts for day";
        private const string BlastStatus = "blast status";
        private const string ViewAllRoles = "view all roles";
        private const string CreateNewRole = "create new role";

        public static void MenuMenuItemDataBound(MenuEventArgs menuEvent, MenuCode currentMenuCode,
           SiteMapDataSource siteMapSecondLevel, bool homePage)
        {
            if (SiteMap.CurrentNode != null)
            {
                if (menuEvent.Item.Selected)
                {
                    if (menuEvent.Item.Parent != null)
                    {
                        if (menuEvent.Item.Parent.Selectable)
                        {
                            menuEvent.Item.Parent.Selected = true;
                        }

                        siteMapSecondLevel.StartingNodeUrl = menuEvent.Item.Parent.NavigateUrl;
                    }
                    else
                    {
                        menuEvent.Item.Selected = true;
                        siteMapSecondLevel.StartingNodeUrl = SiteMap.CurrentNode.Url;
                    }
                }
                else
                {
                    if (menuEvent.Item.Text.Equals(Home, StringComparison.OrdinalIgnoreCase))
                    {
                        siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
                    }
                }
            }
            else
            {
                if (currentMenuCode == MenuCode.PAGESETUP && !homePage)
                {
                    siteMapSecondLevel.StartingNodeUrl = "";
                }
                else
                {
                    SetSiteMapSecondLevel(currentMenuCode, siteMapSecondLevel);
                }

                SetSelectedMenu(menuEvent, currentMenuCode);
            }
        }

        public static void RemoveMenuIfNonAuthorized(MenuEventArgs menuEvent, ECNSession userSession, Menu menu)
        {
            bool? removeFromMenu = null;
            bool? removeFromParentMenu = null;

            switch (menuEvent.Item.Text.ToLower())
            {
                case Users:
                    removeFromMenu = !User.IsAdministrator(userSession.CurrentUser)
                                     || !userSession.CurrentUser.CurrentClient.Services.Any();
                    break;
                case AddNewUser:
                    menuEvent.Item.NavigateUrl = "/ecn.accounts/main/users/userdetail.aspx";
                    break;
                case Leads:
                    removeFromMenu = !(User.IsSystemAdministrator(userSession.CurrentUser) &&
                                       userSession.CurrentUser.IsKMStaff);
                    break;
                case DemoSetup:
                    removeFromParentMenu = userSession.CurrentUser.IsKMStaff &&
                                           (Staff.CurrentStaff.Roles != (int)StaffRoleEnum.DemoManager ||
                                            Staff.CurrentStaff.Roles != (int)StaffRoleEnum.AccountManager);
                    break;
                case DemoManager:
                    removeFromParentMenu = userSession.CurrentUser.IsKMStaff &&
                                           (Staff.CurrentStaff.Roles == (int)StaffRoleEnum.DemoManager ||
                                            Staff.CurrentStaff.Roles == (int)StaffRoleEnum.AccountManager);
                    break;
                case Customers:
                case Billing:
                case ChannelPartners:
                case Notifications:
                case DigitalEdition:
                    removeFromMenu = !User.IsSystemAdministrator(userSession.CurrentUser);
                    break;
                case AddChannelPartner:
                case ChannelReport:
                case EcnToday:
                case ChannelLook:
                case BillingReport:
                case DiskSpace:
                    removeFromParentMenu = !User.IsSystemAdministrator(userSession.CurrentUser);
                    break;
                case Reports:
                    removeFromMenu = !User.IsSystemAdministrator(userSession.CurrentUser) &&
                                     !(User.IsChannelAdministrator(userSession.CurrentUser) &&
                                       userSession.CurrentUser.IsKMStaff);
                    break;
                case IrKmClicks:
                case KmClicks:
                case AccountIntensity:
                case TotalBlastsForDay:
                case BlastStatus:
                    removeFromParentMenu = !User.IsSystemAdministrator(userSession.CurrentUser) ||
                                           !userSession.CurrentUser.IsKMStaff;
                    break;
                case ViewAllRoles:
                case CreateNewRole:
                    removeFromMenu = !User.IsAdministrator(userSession.CurrentUser);
                    break;
            }

            if (removeFromParentMenu == true)
            {
                menuEvent.Item.Parent?.ChildItems.Remove(menuEvent.Item);
            }

            if (removeFromMenu == true)
            {
                menu.Items.Remove(menuEvent.Item);
            }
        }

        private static void SetSelectedMenu(MenuEventArgs menuEvent, MenuCode currentMenuCode)
        {
            switch (menuEvent.Item.Text.ToLower())
            {
                case Users:
                    if (currentMenuCode == MenuCode.USERS)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
                case Customers:
                    if (currentMenuCode == MenuCode.CUSTOMERS)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
                case Leads:
                    if (currentMenuCode == MenuCode.LEADS)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
                case Billing:
                    if (currentMenuCode == MenuCode.BILLINGSYSTEM)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
                case ChannelPartners:
                    if (currentMenuCode == MenuCode.CHANNELS)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
                case Reports:
                    if (currentMenuCode == MenuCode.REPORTS)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
                case Notifications:
                    if (currentMenuCode == MenuCode.NOTIFICATION)
                    {
                        menuEvent.Item.Selected = true;
                    }
                    break;
            }
        }

        private static void SetSiteMapSecondLevel(MenuCode currentMenuCode, SiteMapDataSource siteMapSecondLevel)
        {
            switch (currentMenuCode)
            {
                case MenuCode.USERS:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/users/";
                    break;
                case MenuCode.CUSTOMERS:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/customers/";
                    break;
                case MenuCode.LEADS:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/Leads/";
                    break;
                case MenuCode.BILLINGSYSTEM:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/billingSystem/";
                    break;
                case MenuCode.CHANNELS:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/channels/";
                    break;
                case MenuCode.REPORTS:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/reports/";
                    break;
                case MenuCode.INDEX:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
                    break;
                case MenuCode.NOTIFICATION:
                    siteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
                    break;
            }
        }
    }
}