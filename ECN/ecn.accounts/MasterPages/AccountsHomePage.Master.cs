using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN.Common;
using AccountsBLL = ECN_Framework_BusinessLayer.Accounts;
using AccountsEntity = ECN_Framework_Entities.Accounts;

namespace ecn.accounts.MasterPages
{
    public partial class AccountsHomePage : MasterPageEx
    {
        private const string PageSetup = "page setup";

        public bool HasAuthorized(int userID, int clientID)
        {
            if (UserSession.CurrentUser.UserClientSecurityGroupMaps.Find(x => x.ClientID == clientID) != null)
                return true;

            return false;
        }

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        public ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                if (_usersession == null)
                    _usersession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                return _usersession;
            }
        }

        public string virtualPath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Accounts_VirtualPath"];
                }
                catch
                {
                    return "/";
                }
            }
        }

        public ECN_Framework_Common.Objects.Accounts.Enums.MenuCode CurrentMenuCode
        {
            get
            {
                try
                {
                    return (ECN_Framework_Common.Objects.Accounts.Enums.MenuCode)Session["currentMenuCode"];
                }
                catch
                {
                    return ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.INDEX;
                }
            }
            set
            {
                Session["currentMenuCode"] = value;
            }
        }

        public string HelpContent
        {
            set
            {
                lblHelpContent.Text = value;
            }
        }

        public string HelpTitle
        {
            set
            {
                lblHelpHeading.Text = value;
            }
        }

        public string Heading
        {
            set
            {
                lblHeading.Text = value;
                if (lblHeading.Text != "")
                {
                    HideHeading.Visible = true;
                }
            }
        }

        private string _submenu = string.Empty;

        public string SubMenu
        {
            get
            {
                return _submenu;
            }

            set
            {
                _submenu = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string kmlearn = ConfigurationManager.AppSettings["kmlearn"].ToString();
            if (kmlearn.Equals("false"))
                kmlearning.Visible = false;

            AccountsEntity.Notification notification = AccountsBLL.Notification.GetByCurrentDateTime();
            if (notification != null)
            {
                colorOne.Text = notification.BackGroundColor;
                colorTwo.Text = notification.CloseButtonColor;
                var notificationMessage = notification.NotificationText
                    .Replace("\r", "")   // remove carriage-return
                    .Replace("\n", "")   // remove line-break
                    .Replace("\"", "'"); // convert double quotes to single quotes
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "toastrNotification(\"" + notification.NotificationName + "\",\"" + notificationMessage + "\");", true);
            }


            //if (AuthenticationTicket.getTicket().MasterUserID > 0)
            //{  /// Accepting BUG: after Admin with User priv does login impersonation "revert" triangle won't be re-hidden
            //    lnkUserLoginDropDown.Visible = true;
            //    lnkUserLoginDropDown.HRef = "/ecn.accounts/main/users/userlogin.aspx?userID=" + AuthenticationTicket.getTicket().MasterUserID;
            //}
            //else
            //{
            //    lnkUserLoginDropDown.Visible = false;
            //}
            //if (KM.Platform.User.IsChannelAdministrator(this.UserSession.CurrentUser))
            //{

            //    var cuList = (from us in users
            //                  join cu in customers on
            //                     us.CustomerID equals cu.CustomerID
            //                  where cu.ActiveFlag.ToUpper() == "Y" && us.IsActive
            //                  select cu).Distinct().ToList().OrderBy(x => x.CustomerName);


            //    StringBuilder sbCustListHTML = new StringBuilder();
            //    foreach (var cu in cuList)
            //    {
            //        sbCustListHTML.Append("<a href='" + virtualPath + "/main/customers/customerlogin.aspx?CustomerID=" + cu.CustomerID.ToString() + "'>" + cu.CustomerName + "</a>");
            //    }

            //    m1.InnerHtml = sbCustListHTML.ToString();
            //}

            try
            {
                spanCurrentUser.InnerText = UserSession.CurrentUser.UserName;

                if (!IsPostBack)
                {
                    loadBrandLogo();

                    try
                    {
                        List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();

                        drpclientgroup.DataSource = UserSession.CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName);

                        drpclientgroup.DataBind();

                        drpclientgroup.Items.FindByValue(UserSession.ClientGroupID.ToString()).Selected = true;
                        hfPreviousClientGroup.Value = drpclientgroup.SelectedValue;
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(UserSession.CurrentUser))
                        {
                            drpClient.DataSource = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(Convert.ToInt32(drpclientgroup.SelectedValue.ToString())).OrderBy(x => x.ClientName);
                        }
                        else
                        {
                            drpClient.DataSource = UserSession.CurrentUserClientGroupClients.OrderBy(x => x.ClientName);
                        }

                        drpClient.DataBind();

                        drpClient.Items.FindByValue(UserSession.ClientID.ToString()).Selected = true;
                    }
                    catch (Exception ex)
                    {
                        KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Accounts.HomePageMaster.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                        Response.Redirect("/EmailMarketing.Site/Login/Logout");
                    }
                }
            }
            catch (Exception ex)//adding this here to catch old ticket issues
            {
                Response.Redirect("/EmailMarketing.Site/Login/Logout");
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

        protected void Menu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (e.Item.Text.Equals(PageSetup, StringComparison.OrdinalIgnoreCase))
            {
                e.Item.Selectable = false;
            }

            AccountMenuFunctions.MenuMenuItemDataBound(
                e,
                CurrentMenuCode,
                SiteMapSecondLevel,
                true);

            AccountMenuFunctions.RemoveMenuIfNonAuthorized(e, UserSession, Menu);
        }

        private string EncryptString(string clearTextString)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(clearTextString);
            string encryptedString = Convert.ToBase64String(b);
            return encryptedString;
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
                ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().RefreshSession();
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