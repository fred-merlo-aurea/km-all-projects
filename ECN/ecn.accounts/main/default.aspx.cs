using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Routing;
using ECN_Framework.Accounts.Entity;
using ECN_Framework.Accounts.Object;
using ECN_Framework.Accounts;
using ecn.accounts.MasterPages;
using ECN_Framework;
using ECN_Framework.Common;
using System.Configuration;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;


namespace ecn.accounts.main
{
    public partial class _default : WebPageHelper
    {
        public void HideServicesForCustomer()
        {
            HideServices();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.INDEX;

            SetHelpContent();

            if (!IsPostBack)
            {
                updateNavigationLinks();
                //ValidatePassword();


            }
            //Have to call this on every page_load because we don't know if they changed their customer from the master page
            HideServices();
        }

        private void HideServices()
        {
            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING))
            {
                hlCommunicator.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlCommunicatorImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.SURVEY))
            {
                hlSurvey.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlSurveyImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.DIGITALEDITION))
            {
                //hlDigitalEditions.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlDigitalEditionsImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD))
            {
                hlMAF2.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlMAF2_Icon.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.PRT))
            {
                //hlWQT2.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlWQT2_Icon.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.DOMAINTRACKING))
            {
                hlDomainTracking.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlDomainTrackingImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            if(!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.FULFILLMENT) && !KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD))
            {
                hlUASWeb.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlUASWebImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            bool maVisible = false;
            maVisible = Convert.ToBoolean(ConfigurationManager.AppSettings["MAVisible"].ToString());
            if (maVisible)
            {
                //pnlMA.Visible = true;
                if (!KM.Platform.User.HasServiceFeature(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.MARKETINGAUTOMATION, KMPlatform.Enums.ServiceFeatures.MarketingAutomation))
                {
                    hlMA.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                    //hlMAImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                }
            }
            else
            {
                //pnlMA.Visible = false;
            }

            if (!KM.Platform.User.HasService(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.FORMSDESIGNER))
            {
                hlFormsDesigner.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                //hlFormsDesignerImage.NavigateUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            }
        }

        protected void Page_Unload()
        {

        }

        private void ValidatePassword()
        {
            if (!KMPlatform.BusinessLogic.User.ValidatePassword_Temp(Master.UserSession.CurrentUser.Password))
            {
                modalPopupPwdReset.Show();
            }
        }

        private void updateNavigationLinks()
        {
            KMPlatform.Entity.ClientGroup cg = (new KMPlatform.BusinessLogic.ClientGroup()).Select(Master.UserSession.CurrentUser.CurrentClientGroup.ClientGroupID);

            string MAFURL = string.Empty;

            if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
                MAFURL = !String.IsNullOrEmpty(cg.UADUrl) ? generateLoginURL_Base64(cg.UADUrl) : HttpContext.Current.Request.Url.AbsoluteUri;
            else
                MAFURL = generateLoginURL_Base64("http://localhost/kmps.md/login.aspx");

            hlMAF2.NavigateUrl = MAFURL;
            //hlMAF2_Icon.NavigateUrl = MAFURL;

            string WQTURL = string.Empty;

            if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
                WQTURL = generateLoginURL_Base64(ConfigurationManager.AppSettings["WQTUrl"]);
            else
                WQTURL = generateLoginURL_Base64("http://localhost/kmps.wqt/login.aspx");

            //hlWQT2.NavigateUrl = WQTURL;
            //hlWQT2_Icon.NavigateUrl = WQTURL;


        }


        private void SetHelpContent()
        {
            Master.HelpContent = "On each page this box contains information to help you.";
            Master.HelpTitle = "Help Box";
        }


        private string generateLoginURL(string ApplicationUrl)
        {
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));
            string postbackUrl = ApplicationUrl;
            string queryString = string.Format("{0}={1}&{2}={3}&{4}={5}&{6}={7}", KM.Common.ECNParameterTypes.UserName, Master.UserSession.CurrentUser.UserName, KM.Common.ECNParameterTypes.Password, Master.UserSession.CurrentUser.Password, "ClientGroupID", Master.UserSession.ClientGroupID, "ClientID", Master.UserSession.CurrentUser.CurrentClient.ClientID);
            string queryStringHash = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
            string completePostbackUrl = string.Concat(postbackUrl, "?", queryStringHash);
            return completePostbackUrl;
        }

        private string generateLoginURL_Base64(string ApplicationUrl)
        {
            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]));
            string postbackUrl = ApplicationUrl;
            string queryString = string.Format("{0}|&|{1}|&|{2}|&|{3}|&|{4}|&|{5}|&|{6}|&|{7}", KM.Common.ECNParameterTypes.UserName, Master.UserSession.CurrentUser.UserName, KM.Common.ECNParameterTypes.Password, Master.UserSession.CurrentUser.Password, "ClientGroupID", Master.UserSession.ClientGroupID, "ClientID", Master.UserSession.CurrentUser.CurrentClient.ClientID);
            string queryStringHash = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Base64Encrypt(queryString, ec));
            string completePostbackUrl = string.Concat(postbackUrl, "?", queryStringHash);
            return completePostbackUrl;
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.User, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtOldPasswrd.Text.Equals(Master.UserSession.CurrentUser.Password))
            {
                throwECNException("Incorrect Old Password");
                return;
            }
            if (!txtNewPasswrd.Text.Equals(txtReNewPasswrd.Text))
            {
                throwECNException("New Password entries do not match");
                return;
            }
            if (!KMPlatform.BusinessLogic.User.ValidatePassword(txtReNewPasswrd.Text))
            {
                throwECNException("New Password does not meet the password requirements");
                return;
            }

            KMPlatform.Entity.User currentUser = Master.UserSession.CurrentUser;
            currentUser.Password = txtNewPasswrd.Text;
            currentUser.UpdatedByUserID = currentUser.UserID;
            //sunil --todo 10/23/2015
            new KMPlatform.BusinessLogic.User().Save(currentUser); //, currentUser, Master.UserSession.CurrentBaseChannel.BaseChannelID


            this.modalPopupPwdReset.Hide();
        }
    }
}