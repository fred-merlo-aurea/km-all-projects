using System;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.accounts
{
    /// <summary>
    /// Summary description for Global.
    /// </summary>
    public class Global : HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string CommunicatorVirtualPathKey = "Communicator_VirtualPath";
        private const string AccountsVirtualPathKey = "Accounts_VirtualPath";

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        public Global()
        {
            InitializeComponent();
        }

        protected void Application_Start(Object sender, EventArgs e)
        {

        }

        protected void Session_Start(Object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

        }

        void activePage_PreInit(object sender, EventArgs e)
        {
            Page activePage = HttpContext.Current.Handler as Page;
            if (activePage == null)
            {
                return;
            } 
            
            try
            {
                ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession(); 
                string selectedTheme = "1";

                if (es != null && es.CurrentUser.UserName.Length > 0)
                {
                    selectedTheme = es.CurrentBaseChannel.BaseChannelID.ToString(); 
                    activePage.Theme = selectedTheme;
                }
            }
            catch { }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];
            var pageUrl = ConfigurationManager.AppSettings[AccountsVirtualPathKey];
            var communicatorPageUrl = ConfigurationManager.AppSettings[CommunicatorVirtualPathKey];

            var errorHandler = new AccountApplicationErrorHandler(
                Server, 
                Request, 
                Response, 
                Application,
                applicationId,
                pageUrl,
                communicatorPageUrl);

            errorHandler.HandleApplicationError();
        }

        protected void Session_End(Object sender, EventArgs e)
        {

        }

        protected void Application_End(Object sender, EventArgs e)
        {

        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        #endregion
    }
}

