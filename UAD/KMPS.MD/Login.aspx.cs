using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Security;
using System.Configuration;
using System.Web.Security;
using System.Data.SqlClient;
using KMPS.MD.Objects;
using System.Collections.Specialized;
using System.Linq;
using PlatformUserManager = KMPlatform.BusinessLogic.User;
using PlatformClientManager = KMPlatform.BusinessLogic.Client;
namespace KMPS.MD
{

    //test
    public partial class Login : KMPS.MD.Main.WebPageHelper
    {

        private string getFromQS(string QSName)
        {
            try
            {
                return Request[QSName].ToString();
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

        private string getPassword()
        {
            string thePass = "";
            try
            {
                thePass = txtPassword.Text;
            }
            catch { }
            return thePass;
        }

        private string getUser()
        {
            string theUser = "";
            try
            {
                theUser = txtUserName.Text;
            }
            catch { }
            return theUser;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                //if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost" && !HttpContext.Current.Request.Url.Host.ToLower().Contains("refresh") && !HttpContext.Current.Request.Url.Host.ToLower().Contains("sandbox"))
                //{

                    if (HttpContext.Current.Request.Url.Host.ToLower() == "localhost" || !string.IsNullOrEmpty(ClientQueryString))
                    {
                        string userNameValue = string.Empty;
                        string passwordValue = string.Empty;
                        int clientgroupID = 0;
                        int clientID = 0;

                        try
                        {
                            KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                            string unencrypted = KM.Common.Encryption.Base64Decrypt(System.Web.HttpUtility.UrlDecode(ClientQueryString), ec);

                            string[] str = unencrypted.Split(new string[] { "|&|" }, StringSplitOptions.None);
                            userNameValue = str[1];
                            passwordValue = str[3];
                            clientgroupID = Convert.ToInt32(str[5]);
                            clientID = Convert.ToInt32(str[7]);
                        }
                        catch
                        {
                            RedirectToPlatformLogin();
                        }

                        if (!string.IsNullOrWhiteSpace(userNameValue) && !string.IsNullOrWhiteSpace(passwordValue))
                        {
                            ProcessLogin(userNameValue, passwordValue, clientgroupID, clientID, false);
                        }
                    }
                    else
                        RedirectToPlatformLogin();
                //}
            }
        }

        private void RedirectToPlatformLogin()
        {
            Response.Redirect(ConfigurationManager.AppSettings["ECNLoginPath"].ToString(), true);
        }

        private void ProcessLogin(string username, string password, int clientgroupID, int clientID, bool persist)
        {
            bool authenticationSuccessful = false;

            KMPlatform.Entity.Client c = null;

            try
            {
                KMPlatform.Entity.User u = new PlatformUserManager().SearchUserName(username);
                if (u != null && u.Password.Equals(password))
                {
                    if (u.IsActive)
                        u = new PlatformUserManager().ECN_SetAuthorizedUserObjects(u, clientgroupID, clientID);
                }
                else
                {
                    u = null;
                }

                if (u != null && u.UserID > 0 && u.CustomerID > 0)  //.HasValue
                {
                    //sunil - TODO - check if client exists in the client group
                    //sunil - TOOD - check if clientgroup UAD URL matches the current page URL -- need to add clientgroup UAD url in the database.

                    if (KM.Platform.User.IsSystemAdministrator(u))
                    {
                        c = (new KMPlatform.BusinessLogic.Client()).Select(clientID, false);

                        authenticationSuccessful = Authenticate(u, c, clientgroupID, persist);
                    }
                    else
                    {
                        List<KMPlatform.Entity.Client> lc = new PlatformClientManager().SelectbyUserID(u.UserID, false);
                        if (lc != null)
                        {
                            c = lc.SingleOrDefault(x => x.ClientID == clientID);

                            authenticationSuccessful = Authenticate(u, c, clientgroupID, persist);
                        }
                    }
                }

                if (authenticationSuccessful)
                {
                    
                    UserTracking ut = new UserTracking();
                    ut.UserID = u.UserID;
                    ut.Activity = "Login";
                    ut.IPAddress = Request.UserHostAddress;
                    ut.BrowserInfo = Request.Browser.Browser + "/" + Request.Browser.Version + "/" + Request.Browser.Platform + "/" + Request.UserAgent;
                    UserTracking.Save(new KMPlatform.Object.ClientConnections(c), ut);

                    if (!KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Dashboard, KMPlatform.Enums.Access.View))
                    {
                        RedirectPage(u);
                    }
                    else
                    { 
                        Response.Redirect("main/Dashboard.aspx", true);
                    }
                }
                else
                {
                    RedirectToPlatformLogin();
                }
            }
            catch
            { }
        }


        private void RedirectPage(KMPlatform.Entity.User u)
        {
            if (KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ConsensusView, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("~/AudienceViews/report.aspx?ViewType=ConsensusView", true);
            }
            else if (KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecencyView, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("~/AudienceViews/report.aspx?ViewType=RecencyView", true);
            }
            else if (KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.ProductView, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("~/AudienceViews/ProductView.aspx", true);
            }
            else if (KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.CrossProductView, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("~/AudienceViews/CrossProductView.aspx", true);
            }
            else if (KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.RecordDetails, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("~/AudienceViews/CustomerService.aspx", true);
            }
            else if (KM.Platform.User.HasAccess(u, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.SalesView, KMPlatform.Enums.Access.View))
            {
                Response.Redirect("~/main/Questions.aspx", true);
            }
            else
            {
                Response.Redirect("default.aspx", true);
            }
        }

        public bool Authenticate(KMPlatform.Entity.User u, KMPlatform.Entity.Client c, int clientgroupID, bool persist)
        {
            try
            {
                FormsAuthentication.SignOut();

                FormsAuthentication.SetAuthCookie(u.UserID.ToString(), false);

                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    u.UserID.ToString(), // UserID associated with ticket
                    DateTime.Now, // Date/time issued
                    DateTime.Now.AddDays(30), // Date/time to expire
                    true, // "true" for a persistent user cookie
                    CreateAuthenticationTicketUserData(u, clientgroupID, c.ClientID), // User-data, in this case the roles
                    FormsAuthentication.FormsCookiePath); // Path cookie valid for

                // Hash the cookie for transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Add the cookie to the list for outgoing response
                Response.Cookies.Add(cookie);

                return true;
            }
            catch
            {
                
            }

            return false;
        }

        private string CreateAuthenticationTicketUserData(KMPlatform.Entity.User u, int clientgroupID, int clientID)
        {
            ECN_Framework_Entities.Accounts.Customer ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);

            return String.Join(",",
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                clientgroupID,
                clientID,
                u.AccessKey
                );
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //ProcessLogin(getUser(), getPassword(), getPersist());
        }

        //public void ProcessLogin(string strUser, string strPassword, bool chkPersistLogin)
        //{
        //    FormsAuthentication.Initialize();

        //    Boolean blnIsAuthenticated = false;
        //    //String UserID = "";
        //    //String BaseChannelID = "";
        //    //String ActiveStatus = "";
        //    string UD = "";
        //    int userID = 0;
        //    try
        //    {
        //        string server = (string)Request.ServerVariables["SERVER_NAME"];

        //        //String sqlQuery =
        //        //    " SELECT * " +
        //        //    " FROM Users u, Customers c, ecn_misc.dbo.MasterDatabaseSecurity mds" +
        //        //    " WHERE u.UserName=@UserName AND u.Password=@Password " +
        //        //    " AND u.CustomerID = c.CustomerID AND mds.UserID = u.UserID and c.customerID in(" + ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(server) + "_CustomerIDs"].ToString() + ")";

        //        String sqlQuery =
        //            " SELECT userID FROM Users u join [CUSTOMER] c on u.CustomerID = c.CustomerID  " +
        //            " WHERE u.UserName=@UserName AND u.Password=@Password and u.activeflag='y' and u.IsDeleted = 0 AND c.customerID in(" + ConfigurationManager.AppSettings[DataFunctions.GetSubDomain(server) + "_CustomerIDs"].ToString() + ")";

        //        SqlCommand cmd = new SqlCommand(sqlQuery);
        //        cmd.CommandTimeout = 0;
        //        cmd.CommandType = CommandType.Text;

        //        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar));
        //        cmd.Parameters["@UserName"].Value = strUser;

        //        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar, 25));
        //        cmd.Parameters["@Password"].Value = strPassword;

        //        userID = Convert.ToInt32(DataFunctions.executeScalar(cmd, new SqlConnection(ConfigurationManager.ConnectionStrings["ecnAccountsDB"].ConnectionString)));

        //        Users u = new Users();

        //        if (userID != 0)
        //        {
        //            u = Users.GetUserByID(userID);
        //            UD = u.Permission;
        //            blnIsAuthenticated = true;
        //        }

        //        if (blnIsAuthenticated)
        //        {
        //            FormsAuthentication.SetAuthCookie(userID.ToString(), chkPersistLogin);

        //            // Create a new ticket used for authentication
        //            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
        //                1, // Ticket version
        //                userID.ToString(), // UserID associated with ticket
        //                DateTime.Now, // Date/time issued
        //                DateTime.Now.AddDays(30), // Date/time to expire
        //                true, // "true" for a persistent user cookie
        //                UD, // User-data, in this case the roles
        //                FormsAuthentication.FormsCookiePath); // Path cookie valid for

        //            // Hash the cookie for transport
        //            string hash = FormsAuthentication.Encrypt(ticket);
        //            HttpCookie cookie = new HttpCookie(
        //                FormsAuthentication.FormsCookieName, // Name of auth cookie
        //                hash); // Hashed ticket

        //            // Add the cookie to the list for outgoing response
        //            Response.Cookies.Add(cookie);

        //            UserTracking ut = new UserTracking();
        //            ut.UserID = userID;
        //            ut.Activity = "Login";
        //            ut.IPAddress = Request.UserHostAddress;
        //            ut.BrowserInfo = Request.Browser.Browser + "/" + Request.Browser.Version + "/" + Request.Browser.Platform + "/" + Request.UserAgent;
        //            UserTracking.Save(ut);

        //            if (u.ShowSalesView == true && u.Permission.Equals("readonly", StringComparison.OrdinalIgnoreCase))
        //                Response.Redirect("main/Questions.aspx", true);
        //            else
        //            {
        //                List<Brand> b = Brand.GetByUserID(userID);

        //                if (b.Count > 0)
        //                {
        //                    Response.Redirect("main/Report.aspx?ViewType=ConsensusView", true);
        //                }
        //                else
        //                {
        //                    Response.Redirect("main/Dashboard.aspx", true);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (txtUserName.Text == string.Empty)
        //            {
        //                txtUserName.Text = strUser;
        //            }

        //            lblMessage.Text = "Invalid UserName or Password";
        //            lblMessage.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.ToString();
        //        lblMessage.Visible = true;
        //    }

        //}
    }
}