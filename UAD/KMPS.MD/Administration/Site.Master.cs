using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.Security.Principal;
using System.Web.Security;
using System.Configuration;
using System.Linq;
using KMPlatform.BusinessLogic;

namespace KMPS.MDAdmin
{
    public partial class Site : System.Web.UI.MasterPage
    {

        private string _menu = string.Empty;
        private string _submenu = string.Empty;

        public string Menu
        {
            get
            {
                return _menu;
            }

            set
            {
                _menu = value;
            }
        }

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

        public int LoggedInUser
        {
            get
            {
                return UserSession.CurrentUser.UserID;
            }
        }

        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                MenuItem item = NavigationMenu.FindItem(Menu);
                if (item != null)
                {
                    item.Selected = true;

                    foreach (MenuItem childItem in item.ChildItems)
                    {
                        if (string.Compare(childItem.Text, SubMenu, true) == 0)
                        {
                            childItem.Text = "<div style='color: #cc0000'>" + SubMenu + "</div>";
                        }
                    }
                }
                loadBrandLogo();
                updateNavigationLinks();
            }
        }

        private void loadBrandLogo()
        {
            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(UserSession.BaseChannelID);
            if (bc.IsBranding == false)
            {
                bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(12);
            }
            imgBrandLogo.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/Channels/" + bc.BaseChannelID + "/" + bc.BrandLogo;
        }

        private void updateNavigationLinks()
        {
            KMPlatform.Entity.ClientGroup cg = (new KMPlatform.BusinessLogic.ClientGroup()).Select(UserSession.CurrentUser.CurrentClientGroup.ClientGroupID);

            if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
                hlMAF.NavigateUrl = !String.IsNullOrEmpty(cg.UADUrl) ? generateLoginURL_Base64(cg.UADUrl) : HttpContext.Current.Request.Url.AbsoluteUri;
            else
                hlMAF.NavigateUrl = generateLoginURL_Base64("http://localhost/kmps.md/login.aspx");

            if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
                hlWQT.NavigateUrl = generateLoginURL_Base64(ConfigurationManager.AppSettings["WQTUrl"]);
            else
                hlWQT.NavigateUrl = generateLoginURL_Base64("http://localhost/kmps.wqt/login.aspx");

            hlEmailMarketing.NavigateUrl =  ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.communicator/main/default.aspx";
            hlSurveys.NavigateUrl =   ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.collector/main/survey/";
            hlDigitalEditions.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.publisher/main/edition/default.aspx";
            hlJointForms.NavigateUrl = "http://eforms.kmpsgroup.com/jointformssetup/";
            hlDomainTracking.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.accounts/main/";
            hlHome.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.accounts/main/";
            hlFormsDesigner.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "KMWeb/Forms";
            hlMarketingAutomation.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.MarketingAutomation/";
            hlDataCompare.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "uad.datacompare";
            HideServices();
        }

        private void HideServices()
        {
            if (!KMPlatform.BusinessLogic.Client.HasService(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING))
                pnlEmailMarketing.Visible = false;

            if (!KMPlatform.BusinessLogic.Client.HasService(UserSession.ClientID, KMPlatform.Enums.Services.UAD))
                pnlUAD.Visible = false;

            if (!KMPlatform.BusinessLogic.Client.HasService(UserSession.ClientID, KMPlatform.Enums.Services.SURVEY))
                pnlSurveys.Visible = false;

            if (!KMPlatform.BusinessLogic.Client.HasService(UserSession.ClientID, KMPlatform.Enums.Services.DIGITALEDITION))
                pnlDigitalEditions.Visible = false;

            if (!KMPlatform.BusinessLogic.Client.HasService(UserSession.ClientID, KMPlatform.Enums.Services.PRT))
                pnlWQT.Visible = false;

            if (!KMPlatform.BusinessLogic.Client.HasService(UserSession.ClientID, KMPlatform.Enums.Services.DOMAINTRACKING))
                pnlDomainTracking.Visible = false;

            if (ConfigurationManager.AppSettings["HideForms"].ToLower().Equals("false") && ConfigurationManager.AppSettings["FormsShow_" + UserSession.BaseChannelID.ToString()] == null)
            {
                pnlFormsDesigner.Visible = false;
            }
            else
            {
                if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.FORMSDESIGNER))
                    pnlFormsDesigner.Visible = false;
            }

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.MARKETINGAUTOMATION))
                pnlMarketingAutomation.Visible = false;

            if (!KM.Platform.User.HasServiceFeature(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.DataCompare))
                pnlDataCompare.Visible = false;
        }

        private string generateLoginURL_Base64(string ApplicationUrl)
        {
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));
            string postbackUrl = ApplicationUrl;
            string queryString = string.Format("{0}|&|{1}|&|{2}|&|{3}|&|{4}|&|{5}|&|{6}|&|{7}", KM.Common.ECNParameterTypes.UserName, UserSession.CurrentUser.UserName, KM.Common.ECNParameterTypes.Password, UserSession.CurrentUser.Password, "ClientGroupID", UserSession.ClientGroupID, "ClientID", UserSession.CurrentUser.CurrentClient.ClientID);
            string queryStringHash = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Base64Encrypt(queryString, ec));
            string completePostbackUrl = string.Concat(postbackUrl, "?", queryStringHash);
            return completePostbackUrl;
        }

        protected void lnkbtnLogout_Click(object sender, EventArgs e)
        {
            UserTracking ut = new UserTracking();
            ut.UserID = LoggedInUser;
            ut.Activity = "Logout";
            ut.IPAddress = Request.UserHostAddress;
            ut.BrowserInfo = Request.Browser.Browser + "/" + Request.Browser.Version + "/" + Request.Browser.Platform + "/" + Request.UserAgent;
            UserTracking.Save(clientconnections, ut);

            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(UserSession.BaseChannelID);
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect(ConfigurationManager.AppSettings["ECNLoginPath"].ToString() + "/Logout", false);
        }
    }
}