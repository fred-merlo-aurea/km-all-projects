using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Entities.Accounts;
using System.Web.Security;
using System.Collections;
using System.Web.Caching;
using ECN.Common;

namespace ecn.creator
{
    public partial class Creator : MasterPageEx
    {
        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        public ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession() : _usersession;
            }
        }

        public bool HasAuthorized(int userID, int clientID)
        {
            if (UserSession.CurrentUser.UserClientSecurityGroupMaps.Find(x => x.ClientID == clientID) != null)
                return true;

            return false;
        }

        public string virtualPath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Communicator_VirtualPath"];
                }
                catch
                {
                    return "/";
                }
            }
        }

        public ECN_Framework_Common.Objects.Communicator.Enums.MenuCode CurrentMenuCode
        {
            get
            {
                try
                {
                    return (ECN_Framework_Common.Objects.Communicator.Enums.MenuCode)Session["currentMenuCode"];
                }
                catch
                {
                    return ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.INDEX;
                }
            }
            set
            {
                Session["currentMenuCode"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserSession == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["Accounts_VirtualPath"].ToString() + "/login.aspx", false);
                return;
            }
            else if (!KMPlatform.BusinessLogic.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.CREATOR, KMPlatform.Enums.ServiceFeatures.Creator, KMPlatform.Enums.Access.FullAccess))
            {
                Response.Redirect(ConfigurationManager.AppSettings["Accounts_VirtualPath"].ToString() + "/main", false);
                return;
            }





            //ECN_Framework_Entities.Accounts.BaseChannel channel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID);
            //List<KMPlatform.Entity.User> users = KMPlatform.BusinessLogic.User.GetUsersByChannelID(channel.BaseChannelID);
            //List<ECN_Framework_Entities.Accounts.Customer> customers = ECN_Framework_BusinessLayer.Accounts.Customer.GetCustomersByChannelID(channel.BaseChannelID);
            string displayLearn = ConfigurationManager.AppSettings["kmlearning"].ToString();
            if (displayLearn.Equals("false"))
                kmLearnImage.Visible = false;

            //if (KM.Platform.User.IsChannelAdministrator(this.UserSession.CurrentUser))
            //{
            //    var cuList = (from us in users
            //                  join cu in customers on
            //                     us.CustomerID equals cu.CustomerID
            //                  where cu.ActiveFlag.ToUpper() == "Y" && us.ActiveFlag.ToUpper() == "Y"
            //                  select cu).Distinct().ToList().OrderBy(x => x.CustomerName);

            //    //lnkUserLoginDropDown.HRef = "/ecn.accounts/main/users/userlogin.aspx?userID=" + AuthenticationTicket.getTicket().MasterUserID;

            //    StringBuilder sbCustListHTML = new StringBuilder();
            //    foreach (var cu in cuList)
            //    {
            //        sbCustListHTML.Append("<a href='" + ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/main/customers/customerlogin.aspx?CustomerID=" + cu.CustomerID.ToString() + "'>" + cu.CustomerName + "</a>");
            //    }

            //    //m1.InnerHtml = sbCustListHTML.ToString();
            //}

            try
            {

                if (UserSession.CurrentUser != null)
                {
                    spanCurrentUser.InnerText = UserSession.CurrentUser.UserName;

                }

            }
            catch { }

            if (!IsPostBack)
            {
                loadBrandLogo();
                updateNavigationLinks();

                List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();

                drpclientgroup.DataSource = UserSession.CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName);

                drpclientgroup.DataBind();

                drpclientgroup.Items.FindByValue(UserSession.ClientGroupID.ToString()).Selected = true;
                hfPreviousClientGroup.Value = drpclientgroup.SelectedValue;
                if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(UserSession.CurrentUser))
                {
                    drpClient.DataSource = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(Convert.ToInt32(drpclientgroup.SelectedValue.ToString()));
                }
                else
                {
                    drpClient.DataSource = UserSession.CurrentUserClientGroupClients;
                }

                drpClient.DataBind();

                drpClient.Items.FindByValue(UserSession.ClientID.ToString()).Selected = true;
            }
        }

        private void loadBrandLogo()
        {
            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID);
            if (bc.IsBranding == false)
            {
                bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(12);
            }
            imgBrandLogo.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Channels/" + bc.BaseChannelID + "/" + bc.BrandLogo;
        }

        private void updateNavigationLinks()
        {
            HideServices();

            if (pnlUAD.Visible)
            {
                KMPlatform.Entity.ClientGroup cg = (new KMPlatform.BusinessLogic.ClientGroup()).Select(UserSession.CurrentUser.CurrentClientGroup.ClientGroupID);
                hlMAF.NavigateUrl = !String.IsNullOrEmpty(cg.UADUrl) ? generateLoginURL_Base64(cg.UADUrl) : HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if (pnlPRT.Visible)
                hlPRT.NavigateUrl = generateLoginURL_Base64(ConfigurationManager.AppSettings["WQTUrl"]);

            hlJointForms.NavigateUrl = "http://eforms.kmpsgroup.com/jointformssetup/";
        }

        private void HideServices()
        {
            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING))
                pnlEmailMarketing.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD))
                pnlUAD.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.SURVEY))
                pnlSurveys.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.DIGITALEDITION))
                pnlDigitalEditions.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.PRT))
                pnlPRT.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING))
                pnlDomainTracking.Visible = false;


            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.FORMSDESIGNER))
                pnlFormsDesigner.Visible = false;

        }

        private string generateLoginURL(string ApplicationUrl)
        {
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));
            string postbackUrl = ApplicationUrl;
            string queryString = string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}", KM.Common.ECNParameterTypes.UserName, UserSession.CurrentUser.UserName, KM.Common.ECNParameterTypes.Password, UserSession.CurrentUser.Password, "ClientGroupID", UserSession.ClientGroupID, "ClientID", UserSession.CurrentUser.CurrentClient.ClientID);
            string queryStringHash = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
            string completePostbackUrl = string.Concat(postbackUrl, "?", queryStringHash);
            return completePostbackUrl;
        }

        private string generateLoginURL_Base64(string ApplicationUrl)
        {
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));
            string postbackUrl = ApplicationUrl;
            string queryString = string.Format("{0}|&|{1}|&|{2}|&|{3}|&|{4}|&|{5}|&|{6}|&|{7}", KM.Common.ECNParameterTypes.UserName, UserSession.CurrentUser.UserName, KM.Common.ECNParameterTypes.Password, UserSession.CurrentUser.Password, "ClientGroupID", UserSession.ClientGroupID, "ClientID", UserSession.CurrentUser.CurrentClient.ClientID);
            string queryStringHash = KM.Common.Encryption.Base64Encrypt(queryString, ec);
            string completePostbackUrl = string.Concat(postbackUrl, "?", queryStringHash);
            return completePostbackUrl;
        }

        public void MasterRegisterButtonForPostBack(Control bt)
        {
            ScriptManager1.RegisterPostBackControl(bt);
        }

        protected void Menu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            #region menu now handled in separate project

            //if (SiteMap.CurrentNode != null)
            //{
            //    if (e.Item.Selected == true)
            //    {
            //        if (e.Item.Parent != null)
            //        {
            //            if (e.Item.Parent.Selectable)
            //                e.Item.Parent.Selected = true;
            //            SiteMapSecondLevel.StartingNodeUrl = e.Item.Parent.NavigateUrl;
            //        }
            //        else
            //        {
            //            e.Item.Selected = true;
            //            SiteMapSecondLevel.StartingNodeUrl = SiteMap.CurrentNode.Url;
            //        }
            //    }
            //    else
            //    {
            //        if (e.Item.Text.ToUpper() == "HOME")
            //        {
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
            //        }
            //    }
            //}
            //else
            //{
            //    switch (CurrentMenuCode)
            //    {
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.USERS:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/users/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/customers/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/Leads/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/billingSystem/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/channels/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/reports/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.INDEX:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.PAGESETUP:
            //            SiteMapSecondLevel.StartingNodeUrl = "";
            //            break;
            //        case ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.NOTIFICATION:
            //            SiteMapSecondLevel.StartingNodeUrl = "/ecn.accounts/main/";
            //            break;
            //    }

            //    switch (e.Item.Text.ToLower())
            //    {
            //        case "users":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.USERS)
            //                e.Item.Selected = true;
            //            break;

            //        case "customers":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS)
            //                e.Item.Selected = true;
            //            break;
            //        case "leads":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.LEADS)
            //                e.Item.Selected = true;
            //            break;
            //        case "billing":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM)
            //                e.Item.Selected = true;
            //            break;
            //        case "channel partners":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS)
            //                e.Item.Selected = true;
            //            break;
            //        case "reports":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS)
            //                e.Item.Selected = true;
            //            break;
            //        case "notifications":
            //            if (CurrentMenuCode == ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.NOTIFICATION)
            //                e.Item.Selected = true;
            //            break;
            //    }
            //}

            //switch (e.Item.Text.ToLower())
            //{

            //    case "users":

            //        if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser))
            //            Menu.Items.Remove(e.Item);

            //        break;

            //    case "add new user":

            //        e.Item.NavigateUrl = "/ecn.accounts/main/users/userdetail.aspx";

            //        break;


            //    //customer menu

            //    case "customers":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //            Menu.Items.Remove(e.Item);

            //        break;

            //    //lead menu

            //    case "leads":

            //        if (!(KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) && ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null))
            //            Menu.Items.Remove(e.Item);

            //        break;

            //    case "demo setup":

            //        if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null)
            //        {
            //            if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles != (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.DemoManager) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles != (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.AccountManager))
            //            {
            //                e.Item.Parent.ChildItems.Remove(e.Item);
            //            }
            //        }

            //        break;

            //    case "demo manager":

            //        if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null)
            //        {
            //            if (ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles == (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.DemoManager) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles == (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.AccountManager))
            //            {
            //                e.Item.Parent.ChildItems.Remove(e.Item);
            //            }
            //        }

            //        break;

            //    //billing menu

            //    case "billing":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //            Menu.Items.Remove(e.Item);

            //        break;


            //    //channel menu

            //    case "channel partners":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))  //  && !(KM.Platform.User.IsChannelAdministrator(UserSession.CurrentUser)) && sc.CheckChannelAccess()
            //            Menu.Items.Remove(e.Item);

            //        break;

            //    case "add channel partner":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;

            //    //report menu

            //    case "reports":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) && !(KM.Platform.User.IsChannelAdministrator(UserSession.CurrentUser) && ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null))
            //            Menu.Items.Remove(e.Item);

            //        break;

            //    case "channel report":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;

            //    case "ecn today":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;


            //    case "channel Look":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;


            //    case "billing report":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;


            //    case "disk space":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;

            //    case "digital edition":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //        {
            //            Menu.Items.Remove(e.Item);
            //        }

            //        break;

            //    case "ir-km clicks":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;

            //    case "km clicks":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }

            //        break;

            //    case "account intensity":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) || (ECN_Framework.Accounts.Entity.Staff.CurrentStaff != null && ECN_Framework.Accounts.Entity.Staff.CurrentStaff.Roles != (int)(ECN_Framework.Accounts.Entity.StaffRoleEnum.DemoManager)))
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }
            //        break;

            //    case "total blasts for day":
            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }
            //        break;

            //    case "blast status":
            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) || ECN_Framework.Accounts.Entity.Staff.CurrentStaff == null)
            //        {
            //            e.Item.Parent.ChildItems.Remove(e.Item);
            //        }
            //        break;

            //    //notification menu

            //    case "notifications":

            //        if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            //            Menu.Items.Remove(e.Item);

            //        break;
            //    case "view all roles":
            //        if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser))
            //            Menu.Items.Remove(e.Item);
            //        break;
            //    case "create new role":
            //        if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser))
            //            Menu.Items.Remove(e.Item);
            //        break;
            //}

            #endregion menu now handled in separate project

        }

        protected void drpclientgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();

            if (KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            {
                clients = (new KMPlatform.BusinessLogic.Client()).SelectActiveForClientGroupLite(int.Parse(drpclientgroup.SelectedItem.Value));
            }
            else
            {
                clients = (new KMPlatform.BusinessLogic.Client()).SelectbyUserIDclientgroupID(UserSession.CurrentUser.UserID, int.Parse(drpclientgroup.SelectedItem.Value), false);
            }

            if (clients != null && clients.Count > 0)
            {
                drpClient.DataSource = clients.OrderBy(x => x.ClientName);
                drpClient.DataBind();

                drpClient.SelectedIndex = 0;
                hfPreviousClientGroup.Value = drpclientgroup.SelectedValue;
                drpAccount_SelectedIndexChanged(sender, e);

            }
            else
            {
                drpclientgroup.SelectedValue = hfPreviousClientGroup.Value;
            }
        }

        protected void drpAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userID = UserSession.CurrentUser.UserID;

            if (int.Parse(drpClient.SelectedItem.Value) > 0 && (KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser) || HasAuthorized(UserSession.CurrentUser.UserID, int.Parse(drpClient.SelectedItem.Value))))
            {

                FormsAuthentication.SignOut();

                FormsAuthentication.SetAuthCookie(userID.ToString(), false);

                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    userID.ToString(), // UserID associated with ticket
                    DateTime.Now, // Date/time issued
                    DateTime.Now.AddDays(30), // Date/time to expire
                    true, // "true" for a persistent user cookie
                    CreateAuthenticationTicketUserData(UserSession.CurrentUser, int.Parse(drpclientgroup.SelectedItem.Value), int.Parse(drpClient.SelectedItem.Value)), // User-data, in this case the roles
                    FormsAuthentication.FormsCookiePath); // Path cookie valid for

                // Hash the cookie for transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Add the cookie to the list for outgoing response
                Response.Cookies.Add(cookie);
                ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClearSession();
                Response.Redirect("~/Default.aspx", true);
            }
        }

        internal static string CreateAuthenticationTicketUserData(KMPlatform.Entity.User u, int clientgroupID, int clientID)
        {
            ECN_Framework_Entities.Accounts.Customer ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);

            return String.Join(",",
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                clientgroupID,
                clientID,
                u.AccessKey
                );
        }

        protected void lbEditProfile_Click(object sender, EventArgs e)
        {
            string url = Request.Path;
            if (Request.QueryString.Count > 0)
            {
                url += "?" + Request.QueryString.ToString();
            }
            Response.Redirect("/ecn.accounts/main/users/EditUserProfile.aspx?redirecturl=" + Server.UrlEncode(url), false);
        }

        protected void lbLogout_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry CachedItem in Cache)
            {
                string CacheKey = CachedItem.Key.ToString();
                Cache.Remove(CacheKey);
            }
            FormsAuthentication.SignOut();
            Response.Redirect("/EmailMarketing.Site/Login/Logout", false);
        }
    }
}