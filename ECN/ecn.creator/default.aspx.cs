using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator {
	
	
	
	public partial class homepage : System.Web.UI.Page {
		
		protected void Page_Load(object sender, System.EventArgs e) {
			//Put user code to initialize the page here
			Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Creator_VirtualPath"]+"/main/default.aspx");
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
