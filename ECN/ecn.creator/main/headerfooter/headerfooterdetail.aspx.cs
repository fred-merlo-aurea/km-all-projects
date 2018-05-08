using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.headerfootermanager {

	public partial class headerfootereditor : Page {
		protected HtmlForm newHeaderFooterForm;

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		public headerfootereditor() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {

           
            Master.SubMenu = "add new header-footer";
            Master.Heading = "Add/Edit Header-Footer";

            Master.HelpTitle = "Add/Edit Header-Footer";
			//if(sc.CheckChannelAdmin() || sc.CheckSysAdmin()){
            //HeaderCode.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID.ToString() + "/images" };
            //FooterCode.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID.ToString() + "/images" };
				if (Page.IsPostBack == false) {
					int requestHeaderFooterID = getHeaderFooterID();
					if (requestHeaderFooterID > 0) 
					{
						ECN_Framework.Common.SecurityAccess.canI("HeaderFooters",requestHeaderFooterID.ToString());
						LoadFormData(requestHeaderFooterID);
						SetUpdateInfo(requestHeaderFooterID);
					}
				}
				//InitializeHeaderEditor();
				//InitializeFooterEditor();

				SetPageHeaderStartTags();
				SetFooterEndTags();
			//}
			//else {
			//	Response.Redirect("../default.aspx");
			//}
		}

		private int getHeaderFooterID() {
			int theHeaderFooterID = 0;
			try { theHeaderFooterID = Convert.ToInt32(Request.QueryString["HeaderFooterID"].ToString()); }
			catch(Exception E) { string devnull = E.ToString(); }
			return theHeaderFooterID;
		}

		#region Initialize Wysiswyg Editor
		
		#endregion

		#region Form Prep
		private void SetUpdateInfo(int setHeaderFooterID) {
			HeaderFooterID.Text = setHeaderFooterID.ToString();
			SaveButton.Text = "Update";
			SaveButton.Visible = false;
			UpdateButton.Visible = true;
			
		}
		#endregion

		#region Data Load
		private void LoadFormData(int setHeaderFooterID) {
			string sqlQuery = "SELECT * FROM HeaderFooters WHERE CustomerID = "+Master.UserSession.CurrentCustomer.CustomerID.ToString()+" and HeaderFooterID = " + setHeaderFooterID;
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				HeaderFooterName.Text	= dr["HeaderFooterName"].ToString();
				Keywords.Text					= dr["Keywords"].ToString();
				JavaScriptCode.Text			= dr["JavaScriptCode"].ToString();
				StyleSheet.Text			= dr["StyleSheet"].ToString();
                HeaderCode.Text = dr["HeaderCode"].ToString();
                FooterCode.Text = dr["FooterCode"].ToString();
			}
		}

		private void SetPageHeaderStartTags(){
			PageHeaderStartTags.Text =	
											"<html>"+"\n"+
											"<head>"+"\n"+
											"<meta   http-equiv='Content-Type' "+"\n"+
											"	content='text/html; charset=windows-1252'>"+"\n"+
											"</head>"+"\n"+
											"<body>";
		}

		private void SetFooterEndTags(){
			FooterEndTags.Text = "</body></html>";
		}
		#endregion

		#region Data Handlers

		public void CreateHeaderFooter(object sender, System.EventArgs e) {
			string tname = DataFunctions.CleanString(HeaderFooterName.Text);
			string sqlquery =
				" INSERT INTO HeaderFooters ( "+
				" CustomerID, HeaderFooterName, HeaderCode, FooterCode, Keywords, JavaScriptCode "+
				" ) VALUES ( "+
                " '" + Master.UserSession.CurrentCustomer.CustomerID.ToString() + "', '" + DataFunctions.CleanString(tname) + "', '" + DataFunctions.CleanString(HeaderCode.Text) + "', '" + DataFunctions.CleanString(FooterCode.Text) + "' , '" + DataFunctions.CleanString(Keywords.Text) + "' , '" + DataFunctions.CleanString(JavaScriptCode.Text) + "' " +
				" ) ";
			DataFunctions.Execute(sqlquery);
			Response.Redirect("default.aspx");
		}

		public void UpdateHeaderFooter(object sender, System.EventArgs e) {
			string tname = DataFunctions.CleanString(HeaderFooterName.Text);
			string sqlquery =
				" UPDATE HeaderFooters SET "+
				" HeaderFooterName = '" + DataFunctions.CleanString(tname) + "', "+
                " HeaderCode   = '" + DataFunctions.CleanString(HeaderCode.Text) + "', " +
                " FooterCode   = '" + DataFunctions.CleanString(FooterCode.Text) + "', " +
				" JavaScriptCode   = '" + DataFunctions.CleanString(JavaScriptCode.Text) + "', "+				
				" StyleSheet   = '" + DataFunctions.CleanString(StyleSheet.Text) + "', "+				
				" Keywords   = '" + DataFunctions.CleanString(Keywords.Text) + "' "+
				" WHERE HeaderFooterID = " + HeaderFooterID.Text;
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
