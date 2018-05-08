using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EmailMarketing.Site.Models;

using EmailMarketing.Site.Infrastructure.Abstract;
using EmailMarketing.Site.Infrastructure.Abstract.Settings;
using EmailMarketing.Site.Infrastructure.Authorization;


namespace EmailMarketing.Site.Controllers
{
    [Authorize]
    public class MenuController : ControllerBase
    {
        public MenuController(
            IUserSessionProvider userSessionProvider, 
            IPathProvider pathProvider, 
            IBaseChannelProvider baseChannelProvider)
            :base(userSessionProvider,pathProvider,baseChannelProvider)
        {

        }

        // GET: Menu
        public PartialViewResult Main()
        {
            return PartialView( mainMenuModel );
        }

        [ChildActionOnly]
        public PartialViewResult Impersonate()
        {
            if (HasRole.IsChannelMaster( AuthenticationTicket )
                || HasRole.IsSystemAdministrator( CurrentUser )
                || HasRole.IsChannelAdministrator( CurrentUser))
            {
                int currentCustomerBaseChannelId = UserSessionProvider.GetUserSession().CurrentCustomer.BaseChannelID.Value;
                IEnumerable<ECN_Framework_Entities.Accounts.Customer> cuList =
                    (from user in KMPlatform.BusinessLogic.User.GetUsersByChannelID(currentCustomerBaseChannelId)
                     join customer in ECN_Framework_BusinessLayer.Accounts.Customer.GetCustomersByChannelID(currentCustomerBaseChannelId)
                       on user.CustomerID equals customer.CustomerID
                     where customer.ActiveFlag.ToUpper() == "Y"
                        && user.IsActive
                     select customer
                    ).Distinct().OrderBy(x => x.CustomerName);
                return PartialView(cuList);
            }

            return null;
        }

        #region Main Menu
        private static readonly List<MenuItemViewModel> mainMenuModel = new List<MenuItemViewModel>()
        {
            #region Home
            { 
                new MenuItemViewModel()
                {
                    Label = "Home",
                    Href  = "~/"
                }
            },
            #endregion Home
            #region Users
            { 
                new MenuItemViewModel()
                {
                    Label = "Users",
                    Href  = "~/Accounts/Users",
                    SubMenu = new List<MenuItemViewModel>()
                    {
                        { new MenuItemViewModel() { Label = "Users List", Href  = "~/Accounts/Users" } },
                        { new MenuItemViewModel() { Label = "Add New User", Href  = "~/Accounts/Users/Add" } }
                    }
                }
            },
            #endregion Users
            #region Customers
            { 
                new MenuItemViewModel()
                {
                    Label = "Customers", Href  = "~/Accounts/Customers",
                    SubMenu = new List<MenuItemViewModel>()
                    {
                        { new MenuItemViewModel() { Label = "Customer List", Href  = "~/Accounts/Customers" } },
                        { new MenuItemViewModel() { Label = "ADD NEW USER", Href  = "~/Accounts/Customers/Add" } },
                        { new MenuItemViewModel() { Label = "Customer Inquiries", Href  = "~/Accounts/Customers/Inquiries" } },
                        { new MenuItemViewModel() { Label = "Add Customer Template", Href  = "~/Accounts/Customers/Templates" } }
                    }
                }
            },
            #endregion Customers
            #region Billing
            { 
                new MenuItemViewModel()
                {
                    Label = "Billing", Href  = "~/Billing",
                    SubMenu = new List<MenuItemViewModel>()
                    {
                        {  new MenuItemViewModel() { Label = "Quote List",         Href  = "~/Billing/Quotes" } },
                        {  new MenuItemViewModel() { Label = "Add Customer Quote", Href  = "~/Billing/Quotes/Add" } },
                        {  new MenuItemViewModel() { Label = "Process Invoice",    Href  = "~/Billing/Invoices" } },
                        {  new MenuItemViewModel() { Label = "Billing History",    Href  = "~/Billing/Invoices/History" } },
                        {  new MenuItemViewModel() { Label = "Price Items",        Href  = "~/Billing/Pricing" } },
                        {  new MenuItemViewModel() { Label = "Billing Reports",    Href  = "~/Billing/Reports" } },
                    }
                }
            },
            #endregion Billing
            #region Channels
            { 
                new MenuItemViewModel()
                {
                    Label = "Channel Partners", Href  = "~/Channels",
                    SubMenu = new List<MenuItemViewModel>()
                    {
                        {  new MenuItemViewModel() { Label = "Channel Partner List", Href  = "~/Channels" } },
                        {  new MenuItemViewModel() { Label = "Add Channel Partner",  Href  = "~/Channels/Add" } },
                        {  new MenuItemViewModel() { Label = "Add New Templates",    Href  = "~/Channels/Templates/Add" } },
                    }
                }
            },
            #endregion Channels
            #region Reports
            { 
                new MenuItemViewModel()
                {
                    Label = "Reports", Href  = "~/Reports",
                    SubMenu = new List<MenuItemViewModel>()
                    {
                        {  new MenuItemViewModel() { Label = "Channel Report",       Href  = "~/Reports/Channels" } },
                        {  new MenuItemViewModel() { Label = "ECN Today",            Href  = "~/Reports/ECN-Today" } },
                        {  new MenuItemViewModel() { Label = "Channel Look",         Href  = "~/Reports/Channel-Look" } },
                        {  new MenuItemViewModel() { Label = "Billing Report",       Href  = "~/Reports/Billing" } },
                        {  new MenuItemViewModel() { Label = "Disk Space",           Href  = "~/Reports/Disk-Space" } },
                        {  new MenuItemViewModel() { Label = "Digital Edition",      Href  = "~/Reports/Digital-Edition" } },
                        {  new MenuItemViewModel() { Label = "KM CLicks",            Href  = "~/Reports/KM-Logo-Click" } },
                        {  new MenuItemViewModel() { Label = "Account Intensity",    Href  = "~/Reports/AccountIntensity" } },
                        {  new MenuItemViewModel() { Label = "Total Blasts For Day", Href  = "~/Reports/Total-Blasts-For-Day" } },
                        {  new MenuItemViewModel() { Label = "Blast Status",         Href  = "~/Reports/Blast-Status" } },
                    }
                }
            },
            #endregion Reports
            #region Notifications
            { 
                new MenuItemViewModel()
                {
                    Label = "Notifications",
                    Href  = "~/Notifications"
                }
            },
            #endregion Notifications
        };
        #endregion Main Menu
    }
}
 