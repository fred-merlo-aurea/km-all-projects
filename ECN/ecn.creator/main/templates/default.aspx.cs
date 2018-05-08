using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.templates {
	public partial class TemplateList : Page {

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		protected void Page_Load(object sender, System.EventArgs e) {
			if(!(Request.QueryString["TemplateID"] == null)) {
				deleteTemplate(Convert.ToInt32(Request.QueryString["TemplateID"]));
			}
			loadTemplatesGrid();
		}
		private void deleteTemplate(int templateID) {
            ECN_Framework.Common.SecurityAccess.canI("Templates",templateID.ToString());
			string sqlquery = "DELETE FROM Template WHERE TemplateID = " + templateID;
			DataFunctions.Execute(sqlquery);
		}
		private void loadTemplatesGrid() {
			string sqlquery = "SELECT * FROM Template WHERE CustomerID = " + Master.UserSession.CurrentCustomer.CustomerID.ToString()+ " ORDER BY TemplateName;";
			TemplatesGrid.DataSource=DataFunctions.GetDataTable(sqlquery);
			TemplatesGrid.DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit(e);
		}
		private void InitializeComponent() {    

		}
		#endregion
	}
}
