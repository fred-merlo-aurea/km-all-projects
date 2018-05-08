using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.templatemanager {

	public partial class templateeditor : Page {
		protected HtmlForm newTemplateForm;

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		public templateeditor() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {
            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
				if (Page.IsPostBack == false) {
					int requestTemplateID = getTemplateID();
					if (requestTemplateID > 0) {
                        ECN_Framework.Common.SecurityAccess.canI("Templates",requestTemplateID.ToString());
						LoadFormData(requestTemplateID);
						SetUpdateInfo(requestTemplateID);
					}
				}
				InitializeHeaderEditor();
				InitializeFooterEditor();
			}
			else {
				Response.Redirect("../default.aspx");
			}
		}

		private int getTemplateID() {
			int theTemplateID = 0;
			try { theTemplateID = Convert.ToInt32(Request.QueryString["TemplateID"].ToString()); }
			catch(Exception E) { string devnull = E.ToString(); }
			return theTemplateID;
		}

		#region Initialize Wysiswyg Editor
		private void InitializeHeaderEditor() {
			HeaderSource.EnsureToolsCreated();
            string ImagesWebPath = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "/images/";
			
			string ImagesFilePath=Server.MapPath(ImagesWebPath);
				
			ActiveUp.WebControls.HtmlTextBox.Tools.Image imageLibrary = (ActiveUp.WebControls.HtmlTextBox.Tools.Image) HeaderSource.Toolbars[0].Tools["Image"];
			imageLibrary.AutoLoad = true;
			imageLibrary.Directories.Add("Images", ImagesFilePath, ImagesWebPath);
        
			// Create a new code snippets list
			ActiveUp.WebControls.HtmlTextBox.Tools.CodeSnippets snipLibrary = (ActiveUp.WebControls.HtmlTextBox.Tools.CodeSnippets) HeaderSource.Toolbars[0].Tools["CodeSnippets"];
			snipLibrary.Snippets.Add("Page Name", "%%pagename%%");
			snipLibrary.Snippets.Add("BreadCrumb Trail", "%%breadcrumb%%");
			snipLibrary.Snippets.Add("Horizontal Menu", "%%horizontal-menu%%");
			snipLibrary.Snippets.Add("Vertical Menu", "%%vertical-menu%%");
			snipLibrary.Snippets.Add("Events List", "%%eventslist%%");
			snipLibrary.Snippets.Add("News List", "%%newslist%%");

			// set the icons dir
            HeaderSource.IconsDir = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/icons/";

			// force to absolute paths
			//HeaderSource.RelativePathsFixDisabled = true;
		}
		private void InitializeFooterEditor() {
			FooterSource.EnsureToolsCreated();
            string ImagesWebPath = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "/images/";
			string ImagesFilePath=Server.MapPath(ImagesWebPath);
				
			ActiveUp.WebControls.HtmlTextBox.Tools.Image imageLibrary = (ActiveUp.WebControls.HtmlTextBox.Tools.Image) FooterSource.Toolbars[0].Tools["Image"];
			imageLibrary.AutoLoad = true;
			imageLibrary.Directories.Add("Images", ImagesFilePath, ImagesWebPath);
        
			// Create a new code snippets list
			ActiveUp.WebControls.HtmlTextBox.Tools.CodeSnippets snipLibrary = (ActiveUp.WebControls.HtmlTextBox.Tools.CodeSnippets) FooterSource.Toolbars[0].Tools["CodeSnippets"];
			snipLibrary.Snippets.Add("Page Name", "%%pagename%%");
			snipLibrary.Snippets.Add("BreadCrumb Trail", "%%breadcrumb%%");
			snipLibrary.Snippets.Add("Horizontal Menu", "%%horizontal-menu%%");
			snipLibrary.Snippets.Add("Vertical Menu", "%%vertical-menu%%");
			snipLibrary.Snippets.Add("Events List", "%%eventslist%%");
			snipLibrary.Snippets.Add("News List", "%%newslist%%");

			// set the icons dir
            FooterSource.IconsDir = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/icons/";

			// force to absolute paths
			FooterSource.RelativePathsFixDisabled = true;
		}
		#endregion

		#region Form Prep

		private void SetUpdateInfo(int setTemplateID) {
			TemplateID.Text = setTemplateID.ToString();
			SaveButton.Text = "Update";
			SaveButton.Visible = false;
			UpdateButton.Visible = true;
			
		}

		#endregion

		#region Data Load
		private void LoadFormData(int setTemplateID) {
			string sqlQuery = "SELECT * FROM Template WHERE TemplateID = " + setTemplateID;
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				TextBox[] tbArr = {TemplateName, HeaderCode, SourceCode};
				string[] strArr = {dr["TemplateName"].ToString(), dr["HeaderCode"].ToString(), dr["SourceCode"].ToString()};
				WebControlFunctions.SetText(tbArr, strArr);

				TemplateName.Text = dr["TemplateName"].ToString();
				HeaderSource.Text = dr["HeaderCode"].ToString();
				FooterSource.Text = dr["SourceCode"].ToString();
			}
		}
		#endregion

		#region Data Handlers

		public void CreateTemplate(object sender, System.EventArgs e) {
			string tname = DataFunctions.CleanString(TemplateName.Text);
			string sqlquery =
				" INSERT INTO Template ( "+
				" CustomerID, TemplateName, HeaderCode, SourceCode "+
				" ) VALUES ( "+
				" '" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "', '" + tname + "', '" + HeaderSource.Text + "', '" + FooterSource.Text + "' "+
				" ) ";
			DataFunctions.Execute(sqlquery);
			Response.Redirect("default.aspx");
		}

		public void UpdateTemplate(object sender, System.EventArgs e) {
			string tname = DataFunctions.CleanString(TemplateName.Text);
			string sqlquery =
				" UPDATE Template SET "+
				" TemplateName = '" + tname + "', "+
				" HeaderCode   = '" + HeaderSource.Text + "', "+
				" SourceCode   = '" + FooterSource.Text + "' "+
				" WHERE TemplateID = " + TemplateID.Text;
			DataFunctions.Execute(sqlquery);
			Response.Redirect("default.aspx");
		}

		#endregion

		protected void Page_Init(object sender, EventArgs e) {
			InitializeComponent();
		}

		#region Web Form Designer generated code
		private void InitializeComponent() {    
		}
		#endregion
	}
}
