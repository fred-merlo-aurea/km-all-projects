using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.accounts.classes;
using ecn.common.classes;
using System.Data.SqlClient;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Entities.Accounts;

namespace ecn.accounts.includes
{

    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e) 
        {
            Response.Redirect("/EmailMarketing.Site/login", true);
            //ProcessLogin(getUser(), getPassword(), getPersist());
        }

        private string getUser()
        {
            try
            {
                return Request["user"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        private string getPassword()
        {
            try
            {
                return Request["password"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        private bool getPersist()
        {
            bool thePersist = false;
            try
            {
                if (Request["persist"].ToString() == "Y")
                {
                    thePersist = true;
                }
            }
            catch { }
            return thePersist;
        }

        public void ProcessLogin(String strUser, String strPassword, bool chkPersistLogin)
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
                if(Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri != null)
                    Response.Redirect(Request.UrlReferrer.AbsoluteUri.ToString() + "?error=invalid username or password");
                else
                    throw new ECN_Framework_Common.Objects.SecurityException("Invalid UserName or Password");
            }

            if (u != null)
            {
                ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByUserID(u.UserID, u.CustomerID, false);

                if (c != null)
                {

                    blnIsAuthenticated = true;

                    //-- TODO sunil -- get rid of storing the user details in UserData & use User or Customer objects from ECNSession 
                    // -- USE this store User Role - redefine UserRoles in ECN database.

                    UD = c.CustomerID + "," +
                        c.BaseChannelID + "," +
                        c.CommunicatorChannelID.Value.ToString() + c.CollectorChannelID.Value.ToString() + c.CreatorChannelID.Value.ToString() + c.PublisherChannelID.Value.ToString() + c.CharityChannelID.Value.ToString();
                        //+ "," + u.AccountsOptions + u.CommunicatorOptions + u.CollectorOptions + u.CreatorOptions + "," +
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
                    try
                    {
                        redirectPage = ConfigurationManager.AppSettings[c.BaseChannelID.Value + "_" + c.CustomerID + "_Home_Redirect"].ToString();
                    }
                    catch
                    {
                        try
                        {
                            redirectPage = ConfigurationManager.AppSettings[c.BaseChannelID.Value + "_Home_Redirect"].ToString();
                        }
                        catch
                        {
                            redirectPage = ConfigurationManager.AppSettings["ECN_Home_Redirect"].ToString();
                        }
                    }
                    Response.Redirect(redirectPage, true);
                }
            }

            if (!blnIsAuthenticated)
            {
                //Force Logout even if he's logged in previously.
                FormsAuthentication.SignOut();

                //Since we don't have a BaseChannelID at this point, send'em back to the page where he logged in.
                Response.Redirect(Request.UrlReferrer.AbsoluteUri.ToString() + "?error=invalid username or password");
                    
                //}
                //else
                //{
                //    try { redirectPage = ConfigurationManager.AppSettings[BaseChannelID + "_" + CustomerID + "_Login_Redirect"].ToString(); }
                //    catch
                //    {
                //        try { redirectPage = ConfigurationManager.AppSettings[BaseChannelID + "_Login_Redirect"].ToString(); }
                //        catch { redirectPage = ConfigurationManager.AppSettings["ECN_Login_Redirect"].ToString(); }
                //    }
                //    Response.Redirect(redirectPage, true);
                //}
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {
        }
        #endregion
    }
}
