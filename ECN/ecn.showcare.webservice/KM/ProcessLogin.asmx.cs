using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using ecn.showcare.webservice.Objects;

namespace ecn.showcare.webservice.KM {
	[WebService(
		 Namespace="http://showcare.ecn5.com/ecn.showcare.webservice/KM/ProcessLogin.asmx", 
		 Description="Provides Access to Process Login to ECN.<br>* Use setupLogin() to setup the Login & get AutoLogin Key for ECN. <br>* Use getAutoLoginURL() to Auto Login to ECN using Login Key [Usage: Response.redirect(getAutoLoginURL() + ##accesskey##].")
	] 
	public class ProcessLogin : System.Web.Services.WebService {
		public ProcessLogin() {
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		public string encryptionKey{
			get{ return (ConfigurationManager.AppSettings["EncryptionKey"].ToString()); }
		}

		public string autoLoginRedirectURL{
			get{ return (ConfigurationManager.AppSettings["AutoLoginURL"].ToString()); }
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Login Setup
		[WebMethod(
			 Description="Provides Access to SetUp Login to ECN for User under a Customer Account.<br>- Parameters passed are UserID & CustomerID.<br>- Returns String AutoLogin Key value.<br>- This is is used only for FirstTime / New Customers who don't have a AutoLogin Key")
		]
		public string setupLogin(int customerID, int userID){
			string strToEncrypt	= customerID+"|"+userID; 
			string userName	= "";
			string accessKey	= "";
			string sql = string.Format(@"SELECT UserName FROM Users WHERE UserID = {0} AND CustomerID = {1}",userID,customerID);
			try{
				userName = DataFunctions.ExecuteScalar(sql).ToString();
			}catch(Exception){ }

			if(userName.Length > 0){
				accessKey	= Encryption.Encrypt(strToEncrypt, encryptionKey);
			}

			return accessKey;
		}
		#endregion

		#region Auto Login
		[WebMethod(
			 Description="Provides Access to get Auto Login URL.")
		]
		public string getAutoLoginURL(){
			return autoLoginRedirectURL;			
		}
		#endregion
	}
}
