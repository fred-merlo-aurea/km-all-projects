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
using System.Web.WebPages;
using System.Web.WebPages.Html;
using System.Web.Mvc;
using Role = KM.Platform.User;
using System.Web.Security;
using System.Collections;
using System.Web.Caching;
using ECN.Common;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.communicator.MasterPages
{
    public partial class Communicator : MasterPageEx
    {
        private const string CommunicatorPageName = "communicator.mvc";
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
            // userId is not used but we keep to avoid breaking class interface
            return KMAuthenticationManager.HasAuthorized(clientID);
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
                Response.Redirect(ConfigurationManager.AppSettings["Accounts_VirtualPath"].ToString() + "/login.aspx");
                return;
            }
            ECN_Framework_Entities.Accounts.BaseChannel channel = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(UserSession.CurrentBaseChannel.BaseChannelID);
            //List<KMPlatform.Entity.User> users = KMPlatform.BusinessLogic.User.GetUsersByChannelID(channel.BaseChannelID);
            //List<ECN_Framework_Entities.Accounts.Customer> customers = ECN_Framework_BusinessLayer.Accounts.Customer.GetCustomersByChannelID(channel.BaseChannelID);
            string displayLearn = ConfigurationManager.AppSettings["kmlearning"].ToString();
            if (displayLearn.Equals("false"))
                kmLearnImage.Visible = false;

            //if (Role.IsChannelAdministrator(this.UserSession.CurrentUser))
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
                spanCurrentUser.InnerText = UserSession.CurrentUser.UserName;

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

            //if (pnlPRT.Visible)
            //    hlPRT.NavigateUrl = generateLoginURL_Base64(ConfigurationManager.AppSettings["WQTUrl"]);

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

            if (!(KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD) || KM.Platform.User.HasService(UserSession.CurrentUser, KMPlatform.Enums.Services.FULFILLMENT)))
                pnlAMS.Visible = false;

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

        public void MasterRegisterButtonForPostBack(Control bt)
        {
            ScriptManager1.RegisterPostBackControl(bt);
        }

        protected void Menu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (e.Item.Text.ToLower().Equals("admin"))
                e.Item.Selectable = false;
            if (e.Item.Text.ToLower().Equals("basechannel"))
            {
                e.Item.Selectable = false;

            }
            if (e.Item.Text.ToLower().Equals("customer"))
            {
                e.Item.Selectable = false;

            }


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
                        //subMenu.FindItem(SiteMap.CurrentNode.Url).Selected = true;
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
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/lists/";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/content/";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/ecnwizard/";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.EVENTS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/events/messagetriggers.aspx";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.INDEX:
                        SiteMapSecondLevel.StartingNodeUrl = "~/ecn.accounts/main/";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.PAGEKNOWTICE:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/PageWatch/PageWatchEditor.aspx";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/Reports/SentCampaignsReport.aspx#";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.SALESFORCE:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/SalesForce/ECN_SF_Integration.aspx";
                        break;
                    case ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.OMNITURE:
                        SiteMapSecondLevel.StartingNodeUrl = "/ecn.communicator/main/Omniture/OmnitureCustomerSetup.aspx";
                        break;
                }

                switch (e.Item.Text.ToLower())
                {
                    case "groups":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS)
                            e.Item.Selected = true;
                        break;

                    case "content/messages":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT)
                            e.Item.Selected = true;
                        break;
                    case "blasts/reporting":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS)
                            e.Item.Selected = true;
                        break;
                    case "folders":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.EVENTS)
                            e.Item.Selected = true;
                        break;
                    case "salesforce":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.SALESFORCE)
                            e.Item.Selected = true;
                        break;
                    case "omniture":
                        if (CurrentMenuCode == ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.OMNITURE)
                            break;
                        //e.Item.Selected = true;
                        break;
                }

            }

            if (e.Item.Text.ToLower() == "home")
            {
                e.Item.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/main/";
            }

            if (e.Item.NavigateUrl.IndexOf(CommunicatorPageName, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                e.Item.NavigateUrl = e.Item.NavigateUrl.Replace("/ecn.communicator/", "/");
            }

            if (!(Role.IsChannelAdministrator(this.UserSession.CurrentUser) || Role.IsAdministrator(UserSession.CurrentUser)))
            {
                switch (e.Item.Text.ToLower())
                {
                    //groups menu

                    case "groups":

                        if (!(
                                Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.View) ||
                                Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.View) ||
                                Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails) ||
                                Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.AddEmails) ||
                                Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.CleanEmails) ||
                                Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailSearch, KMPlatform.Enums.Access.FullAccess)
                                )
                        )
                            Menu.Items.Remove(e.Item);

                        break;

                    //content/messages menu

                    case "content/messages":
                        if (!(Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View)))
                            Menu.Items.Remove(e.Item);

                        break;

                    // blasts/reporting menu

                    case "blasts/reporting":
                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View))
                            Menu.Items.Remove(e.Item);

                        break;

                    // folders menu

                    case "folders":

                        //if (!(Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFolder, KMPlatform.Enums.Access.View) ||
                        //    Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ContentFolder, KMPlatform.Enums.Access.View) ||
                        //    Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ImagesFolder, KMPlatform.Enums.Access.View)))
                        //    Menu.Items.Remove(e.Item);

                        break;

                }
            }

            if (!(Role.IsChannelAdministrator(this.UserSession.CurrentUser)))
            {
                switch (e.Item.Text.ToLower())
                {
                    //groups menu

                    case "basechannel":
                        e.Item.Parent.ChildItems.Remove(e.Item);
                        break;
                    case "update email":
                        e.Item.Parent.ChildItems.Remove(e.Item);
                        break;
                }
            }

            if (!Role.IsChannelAdministrator(this.UserSession.CurrentUser) && !Role.IsAdministrator(this.UserSession.CurrentUser))
            {
                if (e.Item.Text.ToLower().Equals("admin"))
                {
                    Menu.Items.Remove(e.Item);
                }
            }


            if (!Role.IsSystemAdministrator(UserSession.CurrentUser))
            {

                switch (e.Item.Text.ToLower())
                {

                    //groups menu

                    case "add group":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "add emails":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.AddEmails))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "import data":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "clean emails":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.CleanEmails))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    //content/messages menu

                    case "create content":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "create message":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "manage images/storage":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ManageImages, KMPlatform.Enums.Access.View))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "link source":

                        if (!KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "message types":

                        if (!(
                                Role.IsChannelAdministrator(UserSession.CurrentUser) &&
                                KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessagePriority)
                              ))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "message type priority":

                        if (!(Role.IsChannelAdministrator(UserSession.CurrentUser) &&
                            KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessagePriority)))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    // blast menu

                    case "blast status":

                        if (!(Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View)))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "setup blast":

                        if (!Role.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "link report":

                        if (!KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.LinkOwner))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "auto-responder":

                        if (!Role.IsChannelAdministrator(UserSession.CurrentUser))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "active customer blasts":

                        if (!Role.IsChannelAdministrator(UserSession.CurrentUser))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;

                    case "message thresholds":

                        if (!(Role.IsChannelAdministrator(UserSession.CurrentUser) &&
                            KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessageThresholds)))
                            e.Item.Parent.ChildItems.Remove(e.Item);

                        break;
                    case "view blast links":
                        if (!Role.IsSystemAdministrator(UserSession.CurrentUser))
                            e.Item.Parent.ChildItems.Remove(e.Item);
                        break;
                    case "email search":
                        if (!KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailSearch, KMPlatform.Enums.Access.FullAccess))
                            e.Item.Parent.ChildItems.Remove(e.Item);
                        break;


                }
            }

            switch (e.Item.Text.ToLower())
            {
                // page knowtice menu
                case "page knowtice":
                    if (!((Role.IsChannelAdministrator(UserSession.CurrentUser) || Role.IsSystemAdministrator(UserSession.CurrentUser) || Role.IsAdministrator(UserSession.CurrentUser)) &&
                        KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PageKnowtice)))
                        Menu.Items.Remove(e.Item);
                    break;

                // events menu

                case "events":
                    if (!(KMPlatform.BusinessLogic.Client.HasServiceFeature(UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EventTriggers)))
                        Menu.Items.Remove(e.Item);

                    break;

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

        public int CutomerDDValue()
        {
            return Convert.ToInt32(drpClient.SelectedValue);
        }

        public void SetValues(MenuCode menuCode, string subMenu, string heading, string helpContent, string helpTitle)
        {
            CurrentMenuCode = menuCode;
            SubMenu = subMenu;
            Heading = heading;
            HelpContent = helpContent;
            HelpTitle = helpTitle;
        }
    }
}



