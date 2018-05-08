using System;
using System.Data;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.menus {
	
	
	
	public partial class previewMenu : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Label LabelPreview;
		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;
		public static string body				= "";
		public static string keywords			= "";
		public static string headerFooterTitle	= "";
	
		protected void Page_Load(object sender, System.EventArgs e) {
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
                        
			if(KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
				ShowPreview();
			}else{
				Response.Redirect("../default.aspx");				
			}
		}

		private void ShowPreview(){
			body =TemplateFunctions.MenuPreview("%%vertical-menu%%",Master.UserSession.CurrentCustomer.CustomerID);
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
