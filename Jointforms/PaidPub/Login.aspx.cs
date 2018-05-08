using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.common.classes;

namespace PaidPub
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {

        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            int userID = 0, customerID = 0, baseChannelID = 0;
            string activeStatus = "N";
            SqlCommand cmdLogin = new SqlCommand(" SELECT u.UserID, c.CustomerID, c.BaseChannelID, u.ActiveFlag  FROM Users u   JOIN Customer c ON u.CustomerID = c.CustomerID   JOIN BaseChannel bc ON c.BaseChannelID = bc.BaseChannelID  WHERE u.UserName = @userName AND u.Password = @password  ");
            cmdLogin.CommandType = CommandType.Text;
            cmdLogin.CommandTimeout = 0;

            cmdLogin.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar));
            cmdLogin.Parameters["@username"].Value = Login1.UserName.ToString();

            cmdLogin.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar));
            cmdLogin.Parameters["@password"].Value = Login1.Password;

            DataTable dtLoginDetails = DataFunctions.GetDataTable("accounts", cmdLogin);

            if (dtLoginDetails.Rows.Count > 0)
            {

                userID = Convert.ToInt32(dtLoginDetails.Rows[0]["UserID"].ToString());
                customerID = Convert.ToInt32(dtLoginDetails.Rows[0]["customerID"].ToString());
                baseChannelID = Convert.ToInt32(dtLoginDetails.Rows[0]["BaseChannelID"].ToString());
                activeStatus = dtLoginDetails.Rows[0]["ActiveFlag"].ToString();

                if (userID > 0)
                {
                    if (activeStatus.ToUpper().Equals("Y"))
                    {
                        Session.Add("CustomerID", customerID);
                        Session.Add("ChannelID", baseChannelID);
                        Session.Add("UserID", userID);

                        createTicket(userID, Login1.UserName.ToString());
                        e.Authenticated = true;
                        Response.Redirect("main/enewsletter/default.aspx");
                    }
                    else
                    {
                        e.Authenticated = false;
                        Session.Clear();
                        Login1.FailureText = "This User is NOT an active User.<br>Please call customer support.";
                    }
                }
                else
                {
                    e.Authenticated = false;
                    Session.Clear();
                }
            }
            else
            {
                e.Authenticated = false;
                Session.Clear();
                Login1.FailureText = "Invalid Username (or) Password.";

            }
        }

        private void createTicket(int userID, string username)
        {
            FormsAuthentication.Initialize();
            FormsAuthentication.SetAuthCookie("PAIDPUB_COOKIE", false);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                6, // Ticket version
                username,  // emailID associated with ticket
                DateTime.Now, // Date/time issued
                DateTime.Now.AddDays(30), // Date/time to expire
                true, // "true" for a persistent user cookie
                userID.ToString(), // User-data, nothing
                FormsAuthentication.FormsCookiePath); // Path cookie valid for

            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
            Response.Cookies.Add(cookie);
        }
    }
}
