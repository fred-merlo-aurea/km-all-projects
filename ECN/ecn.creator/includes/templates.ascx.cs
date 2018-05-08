namespace ecn.creator.includes {
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.creator.classes;
	using ecn.common.classes;

	
	///		Summary description for gallery.
	
	public partial  class templates : System.Web.UI.UserControl {

		public static string communicatordb	= ConfigurationManager.AppSettings["communicatordb"];

		public string TemplateID {
			set {
				int totaltemplates= templaterepeater.Items.Count;
				int theindex=0;
				for (int i=0; i<totaltemplates; i++){
					string currid=((TextBox)templaterepeater.Items[i].FindControl("TemplateID")).Text;
					if (currid==value){
						theindex=i;
					}
				}
				templaterepeater.SelectedIndex = theindex;
				RefreshChanges();
			}
			get {
				if (templaterepeater.SelectedIndex>0){
					return ((TextBox)templaterepeater.SelectedItem.FindControl("TemplateID")).Text;
				} else {
					//return "1";
					return ((TextBox) templaterepeater.Items[0].FindControl("TemplateID")).Text;
				}
			}
		}

		public string SlotsTotal {
			set {
				templaterepeater.BorderWidth = Convert.ToInt32(value);
			}
			get {
				if (templaterepeater.SelectedIndex>0){
					return ((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text;
				} else {
					return "0";
				}
			}
		}

		protected void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack) {
				loadTable();
			}
		}

		public void loadTable(){
			ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

            string ChannelID = sc.BasechannelID().ToString();
			string SQLQuery="SELECT * FROM "+communicatordb+".dbo.Template WHERE TemplateStyleCode='creator' AND BaseChannelID="+ChannelID;
			DataView dv = new DataView(DataFunctions.GetDataTable(SQLQuery));
			templaterepeater.DataSource = dv;
			templaterepeater.DataBind();
		}

		public void DoItemSelect(object objSource, DataListCommandEventArgs objArgs) {	
			templaterepeater.SelectedIndex=objArgs.Item.ItemIndex;
			RefreshChanges();
		}

		public void RefreshChanges(){
			loadTable();
			RaiseBubbleEvent(((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text, null);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent() {

		}
		#endregion
	}
}
