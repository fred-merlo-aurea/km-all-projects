using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ECN_Framework.Accounts.Object;
using ECN_Framework.Accounts.Entity;
using ECN_Framework.Communicator.Entity;
using ecn.common.classes;


using BaseChannel = ECN_Framework_Entities.Accounts.BaseChannel;
using System.Web.Security;

namespace ecn.accounts
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/EmailMarketing.Site/login", true);
            //ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByDomain(Request.Url.Host);
            //if(bc==null)
            //{
            //    bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(12);
            //}
            //imgBrandLogo.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Channels/" + bc.BaseChannelID + "/" + bc.BrandLogo;


            //if (!IsPostBack)
            //{
            //    if (!string.IsNullOrEmpty(ClientQueryString))
            //    {
            //        string userNameValue = string.Empty;
            //        string passwordValue = string.Empty;
            //        string redirectApp = string.Empty;

            //        try
            //        {
            //            int applicationID = int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]);
            //            KM.Common.QueryString.ParseEncryptedSSOQuerystring(ClientQueryString, applicationID, out userNameValue, out passwordValue, out redirectApp);
            //        }
            //        catch
            //        {
            //            Response.Redirect("Login.aspx", true);
            //        }

            //        if (!string.IsNullOrWhiteSpace(userNameValue) && !string.IsNullOrWhiteSpace(passwordValue))
            //        {

            //            ProcessLogin(userNameValue, passwordValue, false, redirectApp);
            //        }
            //    }
            //}
        }

        public void ProcessLogin(String strUser, String strPassword, bool chkPersistLogin, String redirectApp)
        {
            FormsAuthentication.Initialize();

            Boolean blnIsAuthenticated = false;

            string UD = string.Empty;
            string redirectPage = string.Empty;

            KMPlatform.Entity.User u = null;
            try
            {
                KMPlatform.BusinessLogic.User bu = new KMPlatform.BusinessLogic.User();
                u = bu.LogIn(strUser, strPassword, true);
            }
            catch (ECN_Framework_Common.Objects.ECNException)
            {
                Response.Redirect(Request.UrlReferrer.AbsoluteUri.ToString() + "?error=invalid username or password");
            }

            if (u != null && u.UserID != null && u.CustomerID > 0)
            {
                ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByUserID(u.UserID, u.CustomerID, false);

                if (c != null)
                {
                    blnIsAuthenticated = true;
                    UD = c.CustomerID + "," +
                        c.BaseChannelID + "," +
                        c.CommunicatorChannelID.Value.ToString() + c.CollectorChannelID.Value.ToString() + c.CreatorChannelID.Value.ToString() + c.PublisherChannelID.Value.ToString() + c.CharityChannelID.Value.ToString();
                        //+ "," +u.AccountsOptions + u.CommunicatorOptions + u.CollectorOptions + u.CreatorOptions + "," +
                        //c.CommunicatorLevel + c.CollectorLevel + c.CreatorLevel + c.AccountsLevel + c.PublisherLevel + c.CharityLevel;

                    //char[] AccountsOptionsArray = u.AccountsOptions.ToCharArray();

                    if (KM.Platform.User.IsSystemAdministrator(u) || KM.Platform.User.IsChannelAdministrator(u))
                        UD = UD + "," + u.UserID.ToString();

                    FormsAuthentication.SetAuthCookie(u.UserID.ToString(), chkPersistLogin);

                    // Create a new ticket used for authentication
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // Ticket version
                        u.UserID.ToString(), // UserID associated with ticket
                        DateTime.Now, // Date/time issued
                        DateTime.Now.AddDays(30), // Date/time to expire
                        true, // "true" for a persistent user cookie
                        UD, // User-data, in this case the roles
                        FormsAuthentication.FormsCookiePath); // Path cookie valid for

                    // Hash the cookie for transport
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(
                        FormsAuthentication.FormsCookieName, // Name of auth cookie
                        hash); // Hashed ticket

                    // Add the cookie to the list for outgoing response
                    Response.Cookies.Add(cookie);
                    if (redirectApp.ToLower() == "communicator")
                        Response.Redirect("/ecn.communicator/main/default.aspx", true);
                    else if (redirectApp.ToLower() == "surveys")
                        Response.Redirect("/ecn.collector/main/survey/", true);
                    else if (redirectApp.ToLower() == "digitaleditions")
                        Response.Redirect("/ecn.publisher/main/edition/default.aspx", true);
                    else if (redirectApp.ToLower() == "domaintracking")
                        Response.Redirect("/ecn.domaintracking/Main/Index/");
                    else
                        Response.Redirect("/ecn.accounts/main/default.aspx", true);
                }
            }

            if (!blnIsAuthenticated)
            {
                //Force Logout even if he's logged in previously.
                FormsAuthentication.SignOut();
                Response.Redirect(Request.UrlReferrer.AbsoluteUri.ToString() + "?error=invalid username or password");
            }
        }
    }
}
