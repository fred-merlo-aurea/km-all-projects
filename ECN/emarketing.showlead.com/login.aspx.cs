using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Security;
using ecn.common.classes;
using ecn.accounts.classes;
namespace ecn.showcare.wizard {
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public class login : System.Web.UI.Page	{
		
		public string encryptionKey{
            get { return (ConfigurationManager.AppSettings["EncryptionKey"].ToString()); }
		}
		public string RedirecttoShowcare{
			get {return this.redirectURL.ToString();} // + "?msg=" + Server.UrlEncode("Incorrect login information"
		}
		public string goodLoginRedirectCampaign{
            get { return (ConfigurationManager.AppSettings["GoodLogin_Campaign"].ToString()); }
		}
		public string goodLoginRedirectReports{
            get { return (ConfigurationManager.AppSettings["GoodLogin_Reports"].ToString()); }
		}
		public string accountsConnString{
            get { return (ConfigurationManager.AppSettings["act"].ToString()); }
		}

		public string accessKey{
			get{ 
				string theAccessKey= "";
				try {
					theAccessKey	= Request.QueryString["accesskey"].ToString();
				}
				catch(Exception E) {
					string devnull=E.ToString();
				}
				return theAccessKey;
			}
		}

		public string groupID{
			get{ 
				string theGroupID= "";
				try {
					theGroupID	= Request.QueryString["groupID"].ToString();
				}
				catch(Exception E) {
					string devnull=E.ToString();
				}
				return theGroupID;
			}
		}

		public string redirect{
			get{ 
				string theRedirect= "";
				try {
					theRedirect	= Request.QueryString["redirect"].ToString();
				}
				catch(Exception E) {
					string devnull=E.ToString();
				}
				return theRedirect;
			}
		}

		public string getShowcareURL
		{
			get
			{ 
				string url= "";
				try 
				{
					url = Server.UrlDecode(Request.QueryString["redirectURL"].ToString());
				}
				catch(Exception) 
				{
					url = ConfigurationManager.AppSettings["BadLogin"].ToString();
				}
				return url;
			}
		}

		public string redirectURL
		{
			get
			{ 
				return Session["SCRedirectPage"].ToString();
			}
		}

		private void Page_Load(object sender, System.EventArgs e){
			
			//Session.Add("SCRedirectPage", Request.ServerVariables["HTTP_REFERER"]);

			Session.Add("SCRedirectPage", getShowcareURL);

			extractLoginInformation(accessKey);
		}
		
		public string extractLoginInformation(string accessKey){
			string strToDecrypt	= accessKey; 
			string userName	= "", password = "";
			string userID		= "";
			string customerID	= "";
			string cIDuID		= "";
			if(strToDecrypt.Length > 0){
				try{
					cIDuID = ecn.accounts.classes.Encryption.Decrypt(strToDecrypt, encryptionKey);
					StringTokenizer st = new StringTokenizer(cIDuID,'|');
					customerID = st.FirstToken().Trim().ToString();
					userID		= st.LastToken().Trim().ToString();
				}catch(Exception){
					Response.Redirect(RedirecttoShowcare);
				}
				string sql = string.Format(@"SELECT UserName, Password FROM Users WHERE UserID = {0} AND CustomerID = {1}",userID,customerID);
				try{
					DataTable dt = DataFunctions.GetDataTable(sql, accountsConnString);
					DataRow dr = dt.Rows[0];
					userName	= dr["UserName"].ToString();
					password	= dr["Password"].ToString();
				}catch(Exception){ 
					Response.Redirect(RedirecttoShowcare);
				}

				if(userName.Length > 0 && password.Length > 0){
					ProcessLogin(userName, password, true);
				}
			}else{
				Response.Redirect(RedirecttoShowcare);
			}

			return "cIDuID:"+cIDuID+"##UserName:"+userName+"##"+customerID+"|"+userID;			
		}

		public void ProcessLogin(String strUser, String strPassword, bool chkPersistLogin) {
			SecurityCheck securityCheck = new SecurityCheck();
			ChannelCheck channelCheck = new ChannelCheck();
			FormsAuthentication.Initialize();

			String strEncPassword = securityCheck.GetHashedPass(strPassword);
			Boolean blnIsAuthenticated = false;
			string UserID="";
			string BaseChannelID="";
			string GroupID = groupID;
			string UD="";
			string ActiveStatus="";
 
			String sqlQuery=
				" SELECT * "+
				" FROM Users u JOIN Customer c "+
				" ON u.CustomerID = c.CustomerID"+
				" WHERE "+
				" u.UserName='"+strUser+"' AND u.Password='"+strPassword+"' ";
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
					if(redirect.ToLower().Equals("campaign")){
						Response.Redirect(goodLoginRedirectCampaign,true);
					}else if(redirect.ToLower().Equals("report")){
						Response.Redirect(goodLoginRedirectReports,true);						
					}else{
						Response.Redirect(goodLoginRedirectCampaign,true);					
					}
				}else{
					Response.Redirect(RedirecttoShowcare);
				}
			} else {
				Response.Redirect(RedirecttoShowcare);
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
