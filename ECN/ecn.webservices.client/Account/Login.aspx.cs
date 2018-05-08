using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace IMAPTester.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            //if (LoginUser.UserName == "test" && LoginUser.Password == "test")
            //{

            //}
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            bool Authenticated = false;
            TextBox localTextbox = (TextBox)LoginUser.FindControl("API");

            Authenticated = SiteSpecificAuthenticationMethod(LoginUser.UserName, LoginUser.Password, localTextbox.Text);

            e.Authenticated = Authenticated;
            //^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$
        }

        private bool SiteSpecificAuthenticationMethod(string UserName, string Password, string API)
        {
            bool validated = false;
            try
            {
                KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().LogIn(UserName, Password, false);
                if (user != null && user.UserID > 0)
                    validated = true;
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                validated = false;
            }
            catch (Exception)
            {

                throw;
            }



            return validated;
        }

        private object ExecuteScalar(string sql, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }

        private SqlConnection GetSqlConnection()
        {
            SqlConnection connection = null;
            string connectionString = string.Empty;
            string server = (string)System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            if (System.Configuration.ConfigurationManager.AppSettings["connString"] != null)
                connectionString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();

            connection = new SqlConnection(connectionString);

            return connection;
        }

    }
}
