using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.headerfooter {
	public partial class HeaderFooterList : Page {

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		protected void Page_Load(object sender, System.EventArgs e) {
            Master.SubMenu = "header-footer list";
            Master.Heading = "Header-Footer List";

            Master.HelpTitle = "Header-Footer List";
			if(!(Request.QueryString["HeaderFooterID"] == null)) {
				deleteHeaderFooter(Convert.ToInt32(Request.QueryString["HeaderFooterID"]));
			}
			loadHeaderFootersGrid();
		}
		private void deleteHeaderFooter(int headerfooterID) {
            ECN_Framework.Common.SecurityAccess.canI("HeaderFooters",headerfooterID.ToString());
			string sqlquery = "DELETE FROM HeaderFooters WHERE HeaderFooterID = " + headerfooterID;
			DataFunctions.Execute(sqlquery);
		}
		private void loadHeaderFootersGrid() {
			string sqlquery = "SELECT HeaderFooterID, (convert(varchar,HeaderFooterID)+'&chID="+Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()+"&cuID="+Master.UserSession.CurrentCustomer.CustomerID.ToString()+"') as HeaderFooterIDplus, "+
                " HeaderFooterName, (left (Keywords, 50)+ '...') as keywords FROM HeaderFooters WHERE CustomerID = " + Master.UserSession.CurrentCustomer.CustomerID.ToString() + 
				" ORDER BY HeaderFooterName;";
			HeaderFootersGrid.DataSource=DataFunctions.GetDataTable(sqlquery);
			HeaderFootersGrid.DataBind();
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
