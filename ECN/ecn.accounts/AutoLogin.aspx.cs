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
using ecn.common.classes.utilities;
using ecn.common.classes;

namespace ecn.accounts
{
    public partial class AutoLogin : System.Web.UI.Page
    {
        SecureQueryString objSecureQS;
        string redirectPage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["qs"] != null)
            {
                objSecureQS = new SecureQueryString(Request["qs"].ToString());
                Response.Write("UN = " + objSecureQS["un"] + "<BR>");
                Response.Write("pwd = " + objSecureQS["pwd"] + "<BR>");
                //Response.Write("returnURL = " + Server.UrlDecode(objSecureQS["returnURL"]) + "<BR>");
                //Response.Write("UDFName = " + objSecureQS["UDFName"] + "<BR>");
                //Response.Write("UDFdata = " + objSecureQS["UDFdata"] + "<BR>");

                redirectPage = Server.UrlDecode(objSecureQS["returnURL"]);

                ProcessLogin(objSecureQS["un"], objSecureQS["pwd"]);
            }
            else
            {
                //Response.Write("Invalid Login Parameters");
                Response.Redirect("default.aspx");
            }
        }

        public void ProcessLogin(String strUser, String strPassword)
        {

            FormsAuthentication.Initialize();

            Boolean blnIsAuthenticated = false;
            String UserID = "";
            String BaseChannelID = "";
            String CustomerID = "";
            String UD = "";
            String ActiveStatus = "";

            String sqlQuery =
                " SELECT * " +
                " FROM Users u, Customer c " +
                " WHERE u.UserName='" + strUser + "' " +
                " AND u.Password='" + strPassword + "' " +
                " AND u.CustomerID = c.CustomerID ";
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                blnIsAuthenticated = true;
                UserID = dr["UserID"].ToString();
                BaseChannelID = dr["BaseChannelID"].ToString();
                CustomerID = dr["CustomerID"].ToString();
                UD = dr["CustomerID"].ToString() + "," +
                    dr["BaseChannelID"].ToString() + "," +
                    dr["CommunicatorChannelID"].ToString() + dr["CollectorChannelID"].ToString() + dr["CreatorChannelID"].ToString() + dr["PublisherChannelID"].ToString() + dr["CharityChannelID"].ToString() + "," +
                    dr["AccountsOptions"].ToString() + dr["CommunicatorOptions"].ToString() + dr["CollectorOptions"].ToString() + dr["CreatorOptions"].ToString() + "," +
                    dr["CommunicatorLevel"].ToString() + dr["CollectorLevel"].ToString() + dr["CreatorLevel"].ToString() + dr["AccountsLevel"].ToString() + dr["PublisherLevel"].ToString() + dr["CharityLevel"].ToString();
                ActiveStatus = dr["ActiveFlag"].ToString();

            }

            if (blnIsAuthenticated)
            {
                if (ActiveStatus == "Y")
                {
                    FormsAuthentication.SetAuthCookie(UserID, false);

                    // Create a new ticket used for authentication
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // Ticket version
                        UserID, // UserID associated with ticket
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
                    
                    Response.Redirect(redirectPage, true);
                }
                else
                {
                    
                }
            }
            else
            {
                //Force Logout even if he's logged in previously.
                FormsAuthentication.SignOut();

                //Since we don't have a BaseChannelID at this point, send'em back to the page where he logged in.
                Response.Redirect("default.aspx");
            }
        }
    }
}
