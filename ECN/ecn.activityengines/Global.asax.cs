using System;
using System.Configuration;
using System.Diagnostics;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.activityengines
{
    public class Global : System.Web.HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
        }
        void Application_Error(object sender, EventArgs e)
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];

            var errorHandler = new ActivityEnginesApplicationErrorHandler(
                Server,
                Request,
                Response,
                Application,
                applicationId,
                string.Empty);

            try
            {
                errorHandler.HandleApplicationError();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}

