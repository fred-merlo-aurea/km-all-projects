using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Principal;
using AjaxControlToolkit;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using KMPS.MD.Objects;
using KMPlatform.BusinessLogic;

namespace KMPS.MD.MasterPages
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
            lblUserName.Text = "";

            if (!IsPostBack)
            {
                //Tools
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.SummaryReport, KMPlatform.Enums.Access.View) &&
                    !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(7);
                }

                //Campaign
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CampaignComparison, KMPlatform.Enums.Access.View) &&
                   !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Campaign, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(6);
                }

                //Filter
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.View) &&
                   !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.View) &&
                   !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.View) &&
                   !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.View) &&
                   !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(5);
                }

                //Markets
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.View) &&
                    !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(4);
                }

                //SalesView
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.SalesView, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(3);
                }

                //Audience View
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.View) &&
                    !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CrossProductView, KMPlatform.Enums.Access.View) &&
                    !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ProductView, KMPlatform.Enums.Access.View) &&
                    !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecencyView, KMPlatform.Enums.Access.View) &&
                    !KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ConsensusView, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(2);
                }

               
                //Dashboard
                if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Dashboard, KMPlatform.Enums.Access.View))
                {
                    NavigationMenu.Items.RemoveAt(1);
                }
                else if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) 
                {
                    List<Brand> b = Brand.GetByUserID(clientconnections, LoggedInUser);

                    if (b.Count > 0)
                    {
                        NavigationMenu.Items.RemoveAt(1);
                    }
                }

                foreach (MenuItem item in NavigationMenu.Items)
                {
                    switch (item.Text.ToUpper())
                    {
                        case "AUDIENCE VIEWS":
                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(4);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CrossProductView, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(3);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ProductView, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(2);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecencyView, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(1);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ConsensusView, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(0);
                            }
                            break;
                        case "MARKETS":
                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(1);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Market, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(0);
                            }
                            break;
                        case "FILTERS":
                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.QuestionCategory, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(4);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterCategory, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(3);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(2);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ScheduledExport, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(1);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(0);
                            }
                            break;
                        case "CAMPAIGNS":
                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CampaignComparison, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(1);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Campaign, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(0);
                            }
                            break;
                        case "TOOLS":
                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) //User Data Masking
                            {
                                item.ChildItems.RemoveAt(8);
                            }
                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) //CrossTab Report Setup
                            {
                                item.ChildItems.RemoveAt(7);
                            }
                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) //Merge Subscriber
                            {
                                item.ChildItems.RemoveAt(6);
                            }
                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) //Download Template
                            {
                                item.ChildItems.RemoveAt(5);
                            }
                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) //user brand setup
                            {
                                item.ChildItems.RemoveAt(4);
                            }

                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) // brand setup
                            {
                                item.ChildItems.RemoveAt(3);
                            }

                            if (!KM.Platform.User.IsAdministrator(UserSession.CurrentUser)) //record view setup
                            {
                                item.ChildItems.RemoveAt(2);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.SummaryReport, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(1);
                            }

                            if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.View))
                            {
                                item.ChildItems.RemoveAt(0);
                            }
                            break;
                    }
                }

                foreach (MenuItem item in NavigationMenu.Items)
                {
                    switch (item.Text.ToUpper())
                    {
                        case "AUDIENCE VIEWS":
                            if (string.Compare(item.ChildItems[0].Text, "Consensus", true) == 0)
                            {
                                item.NavigateUrl = "~/AudienceViews/report.aspx?ViewType=ConsensusView";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Recency", true) == 0)
                            {
                                item.NavigateUrl = "~/AudienceViews/report.aspx?ViewType=RecencyView";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Product", true) == 0)
                            {
                                item.NavigateUrl = "~/AudienceViews/ProductView.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Cross Product", true) == 0)
                            {
                                item.NavigateUrl = "~/AudienceViews/CrossProductView.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Record View", true) == 0)
                            {
                                item.NavigateUrl = "~/AudienceViews/CustomerService.aspx";
                            }
                            break;
                        case "MARKETS":
                            if (string.Compare(item.ChildItems[0].Text, "Market Creation", true) == 0)
                            {
                                item.NavigateUrl = "~/main/Markets.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Market Comparison", true) == 0)
                            {
                                item.NavigateUrl = "~/main/MarketPenetration.aspx";
                            }
                            break;
                        case "FILTERS":
                            if (string.Compare(item.ChildItems[0].Text, "View Filters/Filter Segmentations", true) == 0)
                            {
                                item.NavigateUrl = "~/main/FilterList.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Scheduled Export", true) == 0)
                            {
                                item.NavigateUrl = "~/main/FilterSchedules.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Filter Comparison", true) == 0)
                            {
                                item.NavigateUrl = "~/main/filterPenetration.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Filter Category", true) == 0)
                            {
                                item.NavigateUrl = "~/main/filterCategory.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Question Category", true) == 0)
                            {
                                item.NavigateUrl = "~/main/QuestionCategory.aspx";
                            }
                            break;
                        case "CAMPAIGNS":
                            if (string.Compare(item.ChildItems[0].Text, "View Campaign", true) == 0)
                            {
                                item.NavigateUrl = "~/main/Campaign.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Campaign Comparison", true) == 0)
                            {
                                item.NavigateUrl = "~/main/CampaignSegmentation.aspx";
                            }
                            break;
                        case "TOOLS":
                            if (string.Compare(item.ChildItems[0].Text, "GeoCoding", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/Geocode.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Summary Report", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/SummaryReport.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Record View Setup", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/RecordViewPDFSetup.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Brand Setup", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/BrandSetup.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "User Brand Setup", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/UserBrandSetup.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "Download Template Setup", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/DownloadTemplateSetup.aspx";
                            }
                            else if (string.Compare(item.ChildItems[0].Text, "UserData Mask Setup", true) == 0)
                            {
                                item.NavigateUrl = "~/Tools/UserDataMaskSetUp.aspx";
                            }
                            //else if (string.Compare(item.ChildItems[0].Text, "Merge Subscriber", true) == 0)
                            //{
                            //    item.NavigateUrl = "~/Tools/MergeSubscriber.aspx";
                            //}
                            break;
                    }
                }

                MenuItem mi = NavigationMenu.FindItem(Menu);
                if (mi != null)
                {
                    mi.Selected = true;
                    if (string.Compare(mi.Text, "Dashboard", true) == 0 || string.Compare(mi.Text, "Sales View", true) == 0)
                    {
                        pnlHeader.Visible = false;
                    }

                    if (SubMenu == "Schedule Filter Export")
                    {
                        lblMenuSelected.Text = "Schedule Filter Export";
                    }

                    foreach (MenuItem childItem in mi.ChildItems)
                    {
                        if (string.Compare(childItem.Text, SubMenu, true) == 0)
                        {
                            childItem.Text = "<div style='color: #cc0000'>" + SubMenu + "</div>";
                            lblMenuSelected.Text = SubMenu;
                        }
                    }
                }
            }

            if (KM.Platform.User.IsSystemAdministrator(UserSession.CurrentUser))
            {
                HyperLinkAdmin.Visible = true;
            }

            loadBrandLogo();
            updateNavigationLinks();
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
            HideServices();

            if (pnlUAD.Visible)
            {
                KMPlatform.Entity.ClientGroup cg = (new KMPlatform.BusinessLogic.ClientGroup()).Select(UserSession.CurrentUser.CurrentClientGroup.ClientGroupID);

                if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
                    hlMAF.NavigateUrl = !String.IsNullOrEmpty(cg.UADUrl) ? generateLoginURL_Base64(cg.UADUrl) : HttpContext.Current.Request.Url.AbsoluteUri;
                else
                    hlMAF.NavigateUrl = generateLoginURL_Base64("http://localhost/kmps.md/login.aspx");
            }

            //if (pnlPRT.Visible)
            //{

            //    if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
            //        hlPRT.NavigateUrl = generateLoginURL_Base64(ConfigurationManager.AppSettings["WQTUrl"]);
            //    else
            //        hlPRT.NavigateUrl = generateLoginURL_Base64("http://localhost/kmps.wqt/login.aspx");
            //}

            hlEmailMarketing.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.communicator/main/default.aspx";
            hlSurveys.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.collector/main/survey/";
            //hlDigitalEditions.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.publisher/main/edition/default.aspx";
            hlJointForms.NavigateUrl = "http://eforms.kmpsgroup.com/jointformssetup/";
            hlDomainTracking.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.domaintracking/Main/Index/";
            hlHome.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.accounts/main/";
            hlFormsDesigner.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "KMWeb/Forms";
            hlMarketingAutomation.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.MarketingAutomation/";
            hlDataCompare.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "uad.datacompare";
            hlAMS.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "UAS.Web";
            foreach (MenuItem item in NavigationMenu.Items)
            {
                if (string.Compare(item.Text, "Home", true) == 0)
                {
                    item.NavigateUrl = ConfigurationManager.AppSettings["ECNPath"].ToString() + "ecn.accounts/main/";
                }
            }
        }

        private void HideServices()
        {
            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING))
                pnlEmailMarketing.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD))
                pnlUAD.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.SURVEY))
                pnlSurveys.Visible = false;

            //if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.DIGITALEDITION))
            //    pnlDigitalEditions.Visible = false;

            //if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.PRT))
            //    pnlPRT.Visible = false;

            if (!KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING))
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

            if (!(KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.FULFILLMENT)) && !(KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD)))
                pnlAMS.Visible = false;
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
            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = identity.Ticket;
            int userID = Convert.ToInt32(ticket.Name);

            UserTracking ut = new UserTracking();
            ut.UserID = userID;
            ut.Activity = "Logout";
            ut.IPAddress = Request.UserHostAddress;
            ut.BrowserInfo = Request.Browser.Browser + "/" + Request.Browser.Version + "/" + Request.Browser.Platform + "/" + Request.UserAgent;
            UserTracking.Save(clientconnections, ut);

            ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(UserSession.BaseChannelID);
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect(ConfigurationManager.AppSettings["ECNLoginPath"].ToString() + "/Logout", false);
        }

        public  void ShowProcess(bool display)
        {
            if (display)
            {
                mpeProgressUpdate.Show();
            }
            else
            {
                mpeProgressUpdate.Hide();
            }

        }

        protected void lbEditProfile_Click(object sender, EventArgs e)
        {
            string url = Request.Path;
            if (Request.QueryString.Count > 0)
            { 
                url += "?" + Request.QueryString.ToString();
            }
            Response.Redirect("~/EditUserProfile.aspx?redirecturl=" + Server.UrlEncode(url), false);
        }
    }
}