using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Security;
using ecn.common.classes;
using ecn.accounts.classes;

namespace ecn.wizard.LoginHandlers {
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public partial class WebServiceLogin : System.Web.UI.Page	{

		#region GETTERS
		public string encryptionKey{
			get{ return (ConfigurationManager.AppSettings["EncryptionKey"].ToString()); }
		}
		public string badLoginRedirect{
			get{ return (ConfigurationManager.AppSettings["BadLogin"].ToString()); }
		}
		public string goodLoginRedirect{
			get{ return (ConfigurationManager.AppSettings["GoodLogin"].ToString()); }
		}
		public string accountsConnString{
			get{ return (ConfigurationManager.AppSettings["act"].ToString()); }
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
		#endregion

		protected void Page_Load(object sender, System.EventArgs e){

			Session.Add("redirectURL", Request.ServerVariables["HTTP_REFERER"]);
			if (Request.QueryString["contentID"] != null)
			{
				Session.Add("ContentID", Request.QueryString["contentID"]);
			}
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
					cIDuID = ecn.common.classes.Encryption.Decrypt(strToDecrypt, encryptionKey);
					StringTokenizer st = new StringTokenizer(cIDuID,'|');
					customerID = st.FirstToken().Trim().ToString();
					userID		= st.LastToken().Trim().ToString();
				}catch{
					Response.Redirect(badLoginRedirect);
				}
				string sql = string.Format(@"SELECT UserName, Password FROM Users WHERE UserID = {0} AND CustomerID = {1}",userID,customerID);
				try{
					DataTable dt = DataFunctions.GetDataTable(sql, accountsConnString);
					DataRow dr = dt.Rows[0];
					userName	= dr["UserName"].ToString();
					password	= dr["Password"].ToString();
				}catch { 
					Response.Redirect(badLoginRedirect);
				}

				if(userName.Length > 0 && password.Length > 0){
					ProcessLogin(userName, password, true);
				}
			}else{
				Response.Redirect(badLoginRedirect);
			}

			return "cIDuID:"+cIDuID+"##UserName:"+userName+"##"+customerID+"|"+userID;			
		}

		public void ProcessLogin(String strUser, String strPassword, bool chkPersistLogin) {
			ECN_Framework.Common.SecurityCheck securityCheck = new ECN_Framework.Common.SecurityCheck();
			ECN_Framework.Common.ChannelCheck channelCheck = new ECN_Framework.Common.ChannelCheck();
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
					Response.Redirect(goodLoginRedirect,true);					
				}else{
					Response.Redirect(badLoginRedirect);
				}
			} else {
				Response.Redirect(badLoginRedirect);
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {    
		}
		#endregion
	}
}
