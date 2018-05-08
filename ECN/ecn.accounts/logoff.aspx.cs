using System;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using ecn.accounts.classes;
using ecn.common.classes;

namespace ecn.accounts {
	public partial class logoff : System.Web.UI.Page {

		protected void Page_Load(object sender, System.EventArgs e) {

            //string redirectURL = FormsAuthentication.LoginUrl;
			//string channelRedirectURL = "";
			/*if(Master.UserSession.CurrentBaseChannel.BaseChannelID.Equals(ConfigurationManager.AppSettings["GotUsedBaseChannelID"].ToString())){
				redirectURL = "/ecn.gotused.wizard/default.aspx";
			}else{
				try{
					channelRedirectURL = ConfigurationManager.AppSettings[Master.UserSession.CurrentBaseChannel.BaseChannelID+"_LogOffRedirectURL"].ToString();
				}catch(Exception ex){ channelRedirectURL = ""; }
			}*/

			/*if(channelRedirectURL.Length > 0){
				Response.Redirect(channelRedirectURL);
			}else{
				Response.Redirect(redirectURL);			
            }*/

            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            string redirectURL = "";
            try {
                redirectURL = ConfigurationManager.AppSettings[es.CurrentBaseChannel.BaseChannelID.ToString() + "_Logoff_Redirect"].ToString();
            } catch {
                redirectURL = ConfigurationManager.AppSettings["ECN_Logoff_Redirect"].ToString();
            }

            Session.Abandon();
			FormsAuthentication.SignOut();
			Response.Redirect(redirectURL);	
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
