using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.digitaledition
{
    /// <summary>
    /// Summary description for Global.
    /// </summary>
    public class Global : HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string CommunicatorVirtualPathKey = "Communicator_VirtualPath";

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

        protected void Application_Error(Object sender, EventArgs e)
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];
            var communicatorPageUrl = ConfigurationManager.AppSettings[CommunicatorVirtualPathKey];

            var errorHandler = new DigitalEditionApplicationErrorHandler(
                Server,
                Request,
                Response,
                Application,
                applicationId,
                "~",
                communicatorPageUrl);

            try
            {
                errorHandler.HandleApplicationError();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
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

    public class Enums
    {
        public enum ErrorMessage
        {
            HardError,
            InvalidLink,
            PageNotFound,
            Timeout,
            Unknown
        }
    }
}
