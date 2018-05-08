using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.includes {
	
	
	
	public partial class login : System.Web.UI.Page {
		protected void Page_Load(object sender, System.EventArgs e) {
			
			ProcessLogin(getUser(), getPassword(), getPersist());
		}

		private string getUser() {
			string theUser = "";
			try {
				theUser = Request["user"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theUser;
		}
		private string getPassword() {
			string thePass = "";
			try {
				thePass = Request["password"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return thePass;
		}
		private bool getPersist() {
			bool thePersist = false;
			try {
				if (Request["persist"].ToString()=="Y"){
					thePersist = true;
				}
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return thePersist;
		}

		public void ProcessLogin(String strUser, String strPassword, bool chkPersistLogin) {
			ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
			
			FormsAuthentication.Initialize();

			String strEncPassword = sc.GetHashedPass(strPassword);
			Boolean blnIsAuthenticated = false;
			String UserID="";
			String ChannelID="";
			String UD="";
			String ActiveStatus="";
 
			String sqlQuery=
				" SELECT * "+
				" FROM Users u, Customers c "+
				" WHERE u.UserName='"+strUser+"' "+
				" AND u.Password='"+strPassword+"' "+
				" AND u.CustomerID = c.CustomerID ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				blnIsAuthenticated = true;
				UserID=dr["UserID"].ToString();
				ChannelID=dr["ChannelID"].ToString();

				//user options on products & their security levels
				string commOptions	= dr["CommunicatorOptions"].ToString();
				string collOptions		= dr["CollectorOptions"].ToString();
				string crtrOptions		= dr["CreatorOptions"].ToString();
				string acctOptions	= dr["AccountsOptions"].ToString();
				string secOptions		= acctOptions + commOptions + collOptions + crtrOptions;

				//Customer Level
				string commLevel		= dr["CommunicatorLevel"].ToString();
				string collLevel			= dr["CollectorLevel"].ToString();
				string crtrLevel		= dr["CreatorLevel"].ToString();
				string acctLevel		= dr["AccountsLevel"].ToString();
				string custLevels		= commLevel + collLevel + crtrLevel + acctLevel;

				//CustomerID [0]+channelID[1]+securityOptions [2]+customerlevel[3]
				UD=dr["CustomerID"].ToString()+","+dr["ChannelID"].ToString()+","+secOptions.ToString()+","+custLevels.ToString();
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
						DateTime.Now.AddMinutes(30), // Date/time to expire
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
					Response.Redirect("../main/default.aspx",true);
				}else{
					Response.Redirect("../front/login.aspx?user="+strUser+"&error=InActive+Login",true);
				}
			} 
			else {
				Response.Redirect("../front/login.aspx?user="+strUser+"&error=Invalid+password",true);
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
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    
		}
		#endregion
	}
}
