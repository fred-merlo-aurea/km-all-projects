using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmailMarketing.Site
{
    public partial class MenuUserControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private ECN_Framework_BusinessLayer.Application.ECNSession usersession;
        public ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (usersession == null)
                {
                    usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
                }

                return usersession;
            }
        }

        public ECN_Framework_Common.Objects.Accounts.Enums.MenuCode CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.INDEX;


        protected void Menu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            #region menu now handled outside the project
            /*
            //This is for making menu items unselectable
            if (e.Item.Text.ToLower().Equals("page setup"))
                e.Item.Selectable = false;

            if (SiteMap.CurrentNode != null)
            {
                if (e.Item.Selected == true)
                {
                    if (e.Item.Parent != null)
                    {
                        if (e.Item.Parent.Selectable)
                            e.Item.Parent.Selected = true;
                        SiteMapSecondLevel.StartingNodeUrl = e.Item.Parent.NavigateUrl;
                    }
                    else
                    {
                        e.Item.Selected = true;
                        SiteMapSecondLevel.StartingNodeUrl = SiteMap.CurrentNode.Url;
                    }
                }
                else
                {
                    if (e.Item.Text.ToUpper() == "HOME")
                    {
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
                    }
                }
            }
            else
            {
                switch (CurrentMenuCode)
                {
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.USERS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/users/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/customers/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/Leads/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/billingSystem/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/channels/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/reports/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.INDEX:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
                        break;
                    case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.NOTIFICATION:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
                        break;
                }

                switch (e.Item.Text.ToLower())
                {
                    case "users":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.USERS)
                            e.Item.Selected = true;
                        break;

                    case "customers":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS)
                            e.Item.Selected = true;
                        break;
                    case "leads":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS)
                            e.Item.Selected = true;
                        break;
                    case "billing":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM)
                            e.Item.Selected = true;
                        break;
                    case "channel partners":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS)
                            e.Item.Selected = true;
                        break;
                    case "reports":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS)
                            e.Item.Selected = true;
                        break;
                    case "notifications":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.NOTIFICATION)
                            e.Item.Selected = true;
                        break;
                }
            }

            switch (e.Item.Text.ToLower())
            {
                //user menu

                case "users":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) && !(UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) && UserSession.CurrentUser.HasUserAccess) && !(UserSession.CurrentUser.IsAdmin && UserSession.CurrentUser.HasUserAccess))
                        Menu.Items.Remove(e.Item);

                    break;

                case "add new user":

                    e.Item.NavigateUrl = "/ecn.accounts/main/users/userdetail.aspx";

                    break;


                //customer menu

                case "customers":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) && !(UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) && UserSession.CurrentUser.HasCustomerAccess))
                        Menu.Items.Remove(e.Item);

                    break;

                //lead menu

                case "leads":

                    if (!(UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) && ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null))
                        Menu.Items.Remove(e.Item);

                    break;

                case "demo setup":

                    if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null)
                    {
                        if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles != (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.DemoManager) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles != (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.AccountManager))
                        {
                            e.Item.Parent.ChildItems.Remove(e.Item);
                        }
                    }

                    break;

                case "demo manager":

                    if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null)
                    {
                        if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles == (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.DemoManager) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles == (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.AccountManager))
                        {
                            e.Item.Parent.ChildItems.Remove(e.Item);
                        }
                    }

                    break;

                //billing menu

                case "billing":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                        Menu.Items.Remove(e.Item);

                    break;


                //channel menu

                case "channel partners":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))  //  && !(UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user)) && sc.CheckChannelAccess()
                        Menu.Items.Remove(e.Item);

                    break;

                case "add channel partner":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;

                //report menu

                case "reports":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) && !(UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) && ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null))
                        Menu.Items.Remove(e.Item);

                    break;

                case "channel report":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;

                case "ecn today":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;


                case "channel Look":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;


                case "billing report":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;


                case "disk space":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;

                case "digital edition":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                    {
                        Menu.Items.Remove(e.Item);
                    }

                    break;

                case "ir-km Clicks":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;

                case "km clicks":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;

                case "account intensity":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null || ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles != (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.DemoManager))
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }

                    break;

                case "total blasts for day":
                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }
                    break;

                case "blast status":
                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
                    {
                        e.Item.Parent.ChildItems.Remove(e.Item);
                    }
                    break;

                //notification menu

                case "notifications":

                    if (!UserSession.CurrentKM.Platform.User.IsSystemAdministrator(user))
                        Menu.Items.Remove(e.Item);

                    break;
            }
            */
            #endregion menu now handled outside the project

        }
    }
}