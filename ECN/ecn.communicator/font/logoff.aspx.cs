using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.front {

    public partial class logoff : System.Web.UI.Page {

		protected void Page_Load(object sender, System.EventArgs e) {
			FormsAuthentication.SignOut();
			Response.Redirect(FormsAuthentication.LoginUrl);
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
