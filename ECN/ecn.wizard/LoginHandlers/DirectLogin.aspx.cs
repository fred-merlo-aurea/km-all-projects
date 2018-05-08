using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using ecn.common.classes;

namespace ecn.wizard.LoginHandlers {
	/// <summary>
	/// Summary description for DirectLogin.
	/// </summary>
	public partial class DirectLogin : System.Web.UI.Page {

		#region Getters & Setters 
		// Get values from queryString
		public string userName{
			get{ 
				string theUserName= "";
				try {
					theUserName	= Request["userName"].ToString();
				}
				catch(Exception E) {
					string devnull=E.ToString();
				}
				return theUserName;
			}
		}

		public string password{
			get{ 
				string thePassword= "";
				try {
					thePassword	= Request["password"].ToString();
				}
				catch(Exception E) {
					string devnull=E.ToString();
				}
				return thePassword;
			}
		}

		public string channelID{
			get{ 
				string theChannelID= "";
				try {
					theChannelID	= Request["channelID"].ToString();
				}
				catch(Exception E) {
					string devnull=E.ToString();
				}
				return theChannelID;
			}
		}

		// getValues from Config
		public string BadLogin{
			get{ return (ConfigurationManager.AppSettings["BadLogin"].ToString()); }
		}
		public string accountsConnString{
			get{ return (ConfigurationManager.AppSettings["act"].ToString()); }
		}
		public string badLoginRedirect{
			get{ return (ConfigurationManager.AppSettings["BadLogin"].ToString()); }
		}
		public string goodLoginRedirect{
			get{ return (ConfigurationManager.AppSettings["GoodLogin"].ToString()); }
		}
		#endregion

		protected void Page_Load(object sender, System.EventArgs e) {
			// Put user code to initialize the page here
			string badLoginRedirect = BadLogin+"wizchannel_"+channelID+"_login.aspx";
			if( (userName.Trim().Length > 0) && (password.Trim().Length > 0) ){
				processLogin(userName.Trim(), password.Trim(), true, channelID.Trim());
			}else{
				Response.Redirect(badLoginRedirect);
			}
		}

		#region Process Login
		public void processLogin(string strUser, string strPassword, bool chkPersistLogin, string strChannelID) {
			ECN_Framework.Common.SecurityCheck securityCheck = new ECN_Framework.Common.SecurityCheck();
			ECN_Framework.Common.ChannelCheck channelCheck = new ECN_Framework.Common.ChannelCheck();
			FormsAuthentication.Initialize();
			string badLoginRedirect = BadLogin+"wizchannel_"+channelID+"_login.aspx";

			Boolean blnIsAuthenticated = false;
			string UserID="";
			string BaseChannelID="";
			string GroupID = "0";
			string UD="";
			string ActiveStatus="";
 
			String sqlQuery=
				" SELECT * "+
				" FROM Users u JOIN Customer c "+
				" ON u.CustomerID = c.CustomerID"+
				" WHERE "+
				" u.UserName='"+strUser+"' AND u.Password='"+strPassword+"' " +
				" AND c.BaseChannelID = "+strChannelID;
			DataTable dt = DataFunctions.GetDataTable(sqlQuery, accountsConnString);
			foreach ( DataRow dr in dt.Rows ) {
				blnIsAuthenticated = true;
				UserID=dr["UserID"].ToString();
				BaseChannelID=dr["BaseChannelID"].ToString();
				
				UD=dr["CustomerID"].ToString()+","+
					dr["BaseChannelID"].ToString()+","+
					dr["CommunicatorChannelID"].ToString()+dr["CollectorChannelID"].ToString()+dr["CreatorChannelID"].ToString()+","+
					dr["AccountsOptions"].ToString()+dr["CommunicatorOptions"].ToString()+dr["CollectorOptions"].ToString()+dr["CreatorOptions"].ToString()+","+
					dr["CommunicatorLevel"].ToString()+dr["CollectorLevel"].ToString()+dr["CreatorLevel"].ToString()+dr["AccountsLevel"].ToString()+","+
					GroupID;
				ActiveStatus = dr["ActiveFlag"].ToString();
			}
   
			if (blnIsAuthenticated) {
				if(ActiveStatus == "Y"){
					FormsAuthentication.SetAuthCookie(UserID, chkPersistLogin);

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

					Session["UserName"]	= strUser;

					Response.Redirect(goodLoginRedirect,true);
				}else{
					Response.Redirect(badLoginRedirect+"?msg=InActiveLogin");
				}
			} else {
				Response.Redirect(badLoginRedirect+"?msg=InvalidLogin");
			}
		}
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
