using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient; 
using System.IO;

namespace KMPS_JF_Setup
{
    public partial class _Login : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e) 
        {
            if (!IsPostBack)
            {
                if (getFromQS("un") != string.Empty && getFromQS("pwd") != string.Empty)
                {
                    ProcessLogin(decryptString(getFromQS("un")), decryptString(getFromQS("pwd")), false);
                }

                Session.Abandon();
                FormsAuthentication.SignOut();
            }
            //Response.Redirect("PUblisher/publisherlist.aspx");

            //FormsAuthenticationTicket tk = new FormsAuthenticationTicket(1, "1", DateTime.Now, DateTime.Now.AddMinutes(1), true, "1", FormsAuthentication.FormsCookiePath);
        }

        private string enryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encryptedString = Convert.ToBase64String(b);
            return encryptedString;
        }

        private string decryptString(string encrString)
        {
            byte[] b = Convert.FromBase64String(encrString);
            string decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(b);
            return decryptedString;
        }

        public void ProcessLogin(String strUser, String strPassword, bool chkPersistLogin)
        {
            SqlCommand cmdLogin = new SqlCommand("SELECT * FROM Users WHERE UserName=@UserName AND Password=@Password");   
            cmdLogin.CommandType = CommandType.Text; 
            cmdLogin.Parameters.Add(new SqlParameter("@UserName",strUser.Replace("'", "''")));    
            cmdLogin.Parameters.Add(new SqlParameter("@Password",strPassword.Replace("'", "''")));                

            try
            {
                DataTable dt = DataFunctions.GetDataTable(cmdLogin);  

                if (dt.Rows.Count > 0)
                {
                    lblMessage.Text = "";
                    string userdata = dt.Rows[0]["UserName"].ToString() + "|" + dt.Rows[0]["CustomerIDs"].ToString();

                    FormsAuthenticationTicket tk = new FormsAuthenticationTicket(1, dt.Rows[0]["UserId"].ToString(), DateTime.Now, DateTime.Now.AddDays(30), true, userdata, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(tk);
                    HttpCookie Logincookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    Response.Cookies.Add(Logincookie);
                    //if (tk.IsPersistent)
                    //{
                    //    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    //    ck.Expires = tk.Expiration;
                    //    Response.Cookies.Add(ck);
                    //}
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (returnUrl == null) returnUrl = "~/Publisher/PublisherList.aspx";
                    Response.Redirect(returnUrl);
                }
            }
            catch
            {
                lblMessage.Text = "Incorrect Username/Password.";
            }
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlDataSourcePLoginConnect.SelectParameters["iMod"].DefaultValue = "4";
            DataView mydataview = (DataView)SqlDataSourcePLoginConnect.Select(DataSourceSelectArguments.Empty);

            if (mydataview.Count > 0)
            {
                lblMessage.Text = "";

                string userdata = mydataview.Table.Rows[0]["UserName"].ToString() + "|" + mydataview.Table.Rows[0]["CustomerIDs"].ToString();


                FormsAuthenticationTicket tk = new FormsAuthenticationTicket(1, mydataview.Table.Rows[0]["UserId"].ToString(), DateTime.Now, DateTime.Now.AddDays(30), true, userdata, FormsAuthentication.FormsCookiePath);
                string hash = FormsAuthentication.Encrypt(tk);
                HttpCookie Logincookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                Response.Cookies.Add(Logincookie);
                //if (tk.IsPersistent)
                //{
                //    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                //    ck.Expires = tk.Expiration;
                //    Response.Cookies.Add(ck);
                //}
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (returnUrl == null) returnUrl = "~/Publisher/PublisherList.aspx";
                Response.Redirect(returnUrl);
            }
            else
            {
                lblMessage.Text = "Incorrect Username/Password.";
            }
        }
    }
}
