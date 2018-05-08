using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.menus {
	public partial class MenuList : Page {

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		protected void Page_Load(object sender, System.EventArgs e) {
			if (Page.IsPostBack==false){
				if(!(Request.QueryString["MenuID"] == null)) {
					deleteMenu(Convert.ToInt32(Request.QueryString["MenuID"]));
				}
				loadMenuTypeCodeDR();
				loadMenuGrid();
			}else {
				loadMenuGrid();
			}
		}
		private void deleteMenu(int menuID) {
            ECN_Framework.Common.SecurityAccess.canI("Menus",menuID.ToString());
			string sqlquery = "DELETE FROM Menus WHERE MenuID = " + menuID;
			DataFunctions.Execute(sqlquery);
		}
		private void loadMenuGrid() {
			string sqlquery = "SELECT * FROM Menus WHERE CustomerID = " + Master.UserSession.CurrentCustomer.CustomerID.ToString() + " and MenuTypeCode = '"+MenuTypeCode.SelectedItem.Value +"' ORDER BY SortOrder";
			MenuGrid.DataSource = DataFunctions.GetDataTable(sqlquery);
			MenuGrid.DataBind();
		}

		private void loadMenuTypeCodeDR() {
			MenuTypeCode.DataSource = DataLists.GetCodesDR("MenuType");
			MenuTypeCode.DataBind();
		}

		public  void MenuType_SelectedIndexChanged(object sender, System.EventArgs e) {
			loadMenuGrid();
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
