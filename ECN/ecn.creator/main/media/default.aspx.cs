using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.media {
	
	
	
	public partial class filemanager : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Button SaveButton;
		protected System.Web.UI.WebControls.Button UpdateButton;

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        //ECN_Framework_BusinessLayer.Application.ECNSession es;

		public filemanager() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {

            Master.SubMenu = "media";
            Master.Heading = "Browse Your Images";

            Master.HelpTitle = "Browse Your Images";
            //es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

			//if(es.HasPermission("ecn.communicator", "contentpriv") || sc.CheckChannelAdmin() || sc.CheckSysAdmin() || sc.CheckAdmin())
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ManageImages, KMPlatform.Enums.Access.Edit))
            {
                string ImagesWebPath = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] +  "/customers/" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "/";
				string ImagesFilePath=Server.MapPath(ImagesWebPath);
				maingallery.imageDirectory=ImagesWebPath;
			}else{
				Response.Redirect("../default.aspx");				
			}
		}

		protected void Page_Init(object sender, EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

        #region Web Form Designer generated code
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
        #endregion

	}
}
