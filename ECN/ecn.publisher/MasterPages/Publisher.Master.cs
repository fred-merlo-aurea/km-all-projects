using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;
using System.Collections;
using System.Web.Caching;
using ECN.Common;

namespace ecn.publisher.MasterPages
{
	public partial class Publisher : MasterPageEx
    {
        public bool HasAuthorized(int userID, int clientID)
        {
            // userId is not used but we keep to avoid breaking class interface
            return KMAuthenticationManager.HasAuthorized(clientID);
        }

        private ECN_Framework_BusinessLayer.Application.ECNSession _usersession = null;
        public ECN_Framework_BusinessLayer.Application.ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession() : _usersession;
            }
        }

        /*The code below must be deleted and the UserSession obj must be used-ROHITP*/
        //private ECN_Framework.Common.SecurityCheck _sc = null;
        //public ECN_Framework.Common.SecurityCheck sc
        //{
        //    get
        //    {
        //        return _sc == null ? new ECN_Framework.Common.SecurityCheck() : _sc;
        //    }
        //}

        //private ECN_Framework_BusinessLayer.Application.ECNSession _es = null;
        //public ECN_Framework_BusinessLayer.Application.ECNSession es
        //{
        //    get
        //    {
        //        return _es == null ? ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession() : _es;
        //    }
        //}
        /*The code above must be deleted and the UserSession obj must be used-ROHITP*/


        public string virtualPath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Publisher_VirtualPath"];
                }
                catch
                {
                    return "/";
                }
            }
        }

        public ECN_Framework_Common.Objects.Publisher.Enums.MenuCode CurrentMenuCode
        {
            get
            {
                try
                {
                    return (ECN_Framework_Common.Objects.Publisher.Enums.MenuCode)Session["currentMenuCode"];
                }
                catch
                {
                    return ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.PUBLICATION;
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
            else if (!KMPlatform.BusinessLogic.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.DIGITALEDITION, KMPlatform.Enums.ServiceFeatures.DigitalEdition, KMPlatform.Enums.Access.FullAccess))
            {
                Response.Redirect(ConfigurationManager.AppSettings["Accounts_VirtualPath"].ToString() + "/main", false);
            }


            string displayLearn = ConfigurationManager.AppSettings["kmlearning"].ToString();
            if (displayLearn.Equals("false"))
                kmLearnImage.Visible = false;
            /*if (KM.Platform.User.IsChannelAdministrator(this.UserSession.CurrentUser))
            {
                var cuList = (from us in users
                              join cu in customers on
                                 us.CustomerID equals cu.CustomerID
                              where cu.ActiveFlag.ToUpper() == "Y" && us.ActiveFlag.ToUpper() == "Y"
                              select cu).Distinct().ToList().OrderBy(x => x.CustomerName);

                lnkUserLoginDropDown.HRef = "/ecn.accounts/main/users/userlogin.aspx?userID=" + AuthenticationTicket.getTicket().MasterUserID;

                StringBuilder sbCustListHTML = new StringBuilder();
                foreach (var cu in cuList)
                {
                    sbCustListHTML.Append("<a href='" + ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/main/customers/customerlogin.aspx?CustomerID=" + cu.CustomerID.ToString() + "'>" + cu.CustomerName + "</a>");
                }

                m1.InnerHtml = sbCustListHTML.ToString();
            }*/

            try
            {

                if (UserSession.CurrentUser != null && UserSession.CurrentCustomer != null)
                {
                    spanCurrentUser.InnerText = UserSession.CurrentUser.UserName;

                }

            }
            catch { }

            if (!IsPostBack)
            {
                loadBrandLogo();
                updateNavigationLinks();
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
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "Accounts.HomePageMaster.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommmon_Application"].ToString()));
                    Response.Redirect("/EmailMarketing.Site/Login/Logout");
                }
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

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.MARKETINGAUTOMATION))
                pnlMarketingAutomation.Visible = false;

            if (!KM.Platform.User.HasServiceFeature(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare))
                pnlDataCompare.Visible = false;
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

        protected void Menu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (SiteMap.CurrentNode != null)
            {
                if (e.Item.Selected == true)
                {
                    if (e.Item.Parent != null)
                    {
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
                        SiteMapSecondLevel.StartingNodeUrl = "~/ecn.accounts/main/";
                    }
                }
            }
            else
            {
                switch (CurrentMenuCode)
                {
                    case ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.PUBLICATION:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.publisher/main/Publication/";
                        break;
                    case ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.publisher/main/Edition/";
                        break;
                    case ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.INDEX:
                        SiteMapSecondLevel.StartingNodeUrl = "~/ecn.accounts/main/";
                        break;
                }
            }

            if (!(KM.Platform.User.IsChannelAdministrator(this.UserSession.CurrentUser)))
            {
                switch (e.Item.Text.ToLower())
                {
                    //groups menu
                    case "email search":
                        e.Item.Parent.ChildItems.Remove(e.Item);
                        break;
                }
            }

            if (e.Item.Text.ToLower() == "home")
            {
                e.Item.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/main/";
            }
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
            if (KMAuthenticationManager.AddFormsAuthenticationCookie(
                    int.Parse(drpClient.SelectedItem.Value),
                    int.Parse(drpclientgroup.SelectedItem.Value),
                    Response.Cookies))
            {
                Response.Redirect("~/Default.aspx", true);
            }
        }

        protected void lbEditProfile_Click(object sender, EventArgs e)
        {
            EditProfileHandler();
        }

        protected void lbLogout_Click(object sender, EventArgs e)
        {
            LogoutHandler();
        }
    }
}