using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.menusmanager {

	public partial class menueditor : System.Web.UI.Page {
		protected HtmlForm newMenuForm;
        private static string currentCustomerID;
		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		public menueditor() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {
			//if(sc.CheckChannelAdmin() || sc.CheckSysAdmin()){
				if (Page.IsPostBack == false) {
                    currentCustomerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
					int requestMenuID = getMenuID();
					if (requestMenuID > 0) { // updating page
                        ECN_Framework.Common.SecurityAccess.canI("Menus",requestMenuID.ToString());
						LoadFormData(requestMenuID);
						SetUpdateInfo(requestMenuID);
					}
					else { // adding new page
						LoadDropDowns();
					}
				}
			//}
			//else {
			//	Response.Redirect("../default.aspx");
			//}
		}

		private int getMenuID() {
			int theMenuID = 0;
			try { theMenuID = Convert.ToInt32(Request.QueryString["MenuID"].ToString()); }
			catch(Exception E) { string devnull = E.ToString(); }
			return theMenuID;
		}

		#region Form Prep

		private void SetUpdateInfo(int setMenuID) {
			MenuID.Text = setMenuID.ToString();
			SaveButton.Text = "Update";
			SaveButton.Visible = false;
			UpdateButton.Visible = true;
			
		}

		#endregion

		#region Data Load
		private void LoadDropDowns() {
			String sqlQuery = "SELECT * FROM Code  WHERE CodeType = 'MenuType'";
			DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
			WebControlFunctions.PopulateDropDownList(MenuTypeCode, codeDT, "CodeDisplay", "CodeValue");

            sqlQuery = "SELECT * FROM Page WHERE CustomerID = " + currentCustomerID;
			DataTable menuTargetDT = DataFunctions.GetDataTable(sqlQuery);
			WebControlFunctions.PopulateDropDownList(MenuTarget,menuTargetDT, "PageName", "QueryValue");

			LoadParentMenuDD();
		}
		private void LoadFormData(int setMenuID) {
			String sqlQuery = "SELECT * FROM Menus WHERE MenuID = " + setMenuID;
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				TextBox[] tbArr = {MenuCode, MenuName, SortOrder};
				string[] strArr = {dr["MenuCode"].ToString(), dr["MenuName"].ToString(), dr["SortOrder"].ToString()};
				WebControlFunctions.SetText(tbArr, strArr);

                sqlQuery = "SELECT * FROM Page WHERE CustomerID = " + currentCustomerID;
				DataTable menuTargetDT = DataFunctions.GetDataTable(sqlQuery);
				WebControlFunctions.PopulateDropDownList(MenuTarget,menuTargetDT, "PageName", "QueryValue");
				WebControlFunctions.SetSelectedIndex(MenuTarget, dr["MenuTarget"].ToString());

				sqlQuery =  "SELECT * FROM Code  WHERE CodeType = 'MenuType'";
				DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
				WebControlFunctions.PopulateDropDownList(MenuTypeCode, codeDT, "CodeDisplay", "CodeValue");
				WebControlFunctions.SetSelectedIndex(MenuTypeCode, dr["MenuTypeCode"].ToString());

                sqlQuery = "SELECT * FROM Menus WHERE CustomerID = " + currentCustomerID;
				DataTable parentIDDT = DataFunctions.GetDataTable(sqlQuery);
				WebControlFunctions.PopulateDropDownList(ParentID,parentIDDT, "MenuName", "MenuID");
				ParentID.Items.Add("--Parent Menu--");
				ParentID.Items.FindByText("--Parent Menu--").Value="0";
				ParentID.SelectedValue = dr["ParentID"].ToString();
			}
		}
		
		private void LoadParentMenuDD(){
            string sqlQuery = "SELECT * FROM Menus WHERE MenuTypeCode='" + MenuTypeCode.SelectedItem.Value + "'  AND CustomerID = " + currentCustomerID;
			DataTable parentIDDT = DataFunctions.GetDataTable(sqlQuery);
			WebControlFunctions.PopulateDropDownList(ParentID,parentIDDT, "MenuName", "MenuID");
			ParentID.Items.Add("--Parent Menu--");
			ParentID.Items.FindByText("--Parent Menu--").Value="0";
		}
		
		public void LoadParentMenuDD(object sender, EventArgs e) {
            string sqlQuery = "SELECT * FROM Menus WHERE MenuTypeCode='" + MenuTypeCode.SelectedItem.Value + "' AND CustomerID = " + currentCustomerID;
			DataTable parentIDDT = DataFunctions.GetDataTable(sqlQuery);
			WebControlFunctions.PopulateDropDownList(ParentID,parentIDDT, "MenuName", "MenuID");
			ParentID.Items.Add("--Parent Menu--");
			ParentID.Items.FindByText("--Parent Menu--").Value="0";
		}
		#endregion

		#region Data Handlers

		public void CreateMenu(object sender, System.EventArgs e) {
			string mname       = DataFunctions.CleanString(MenuName.Text);
			string sqlquery=
				" INSERT INTO Menus ( "+
				" CustomerID, MenuCode, MenuName, MenuTarget, SortOrder, MenuTypeCode, ParentID "+
				" ) VALUES ( "+
                " '" + currentCustomerID + "', '" + StringFunctions.Replace(MenuCode.Text, " ", "-") + "', '" + mname + "', '" + MenuTarget.SelectedItem + "', " +
				" '"+SortOrder.Text+"', '"+MenuTypeCode.SelectedValue+"', '"+ParentID.SelectedValue+"' "+
				" ) ";
			DataFunctions.Execute(sqlquery);
			Response.Redirect("default.aspx");
		}

		public void UpdateMenu(object sender, System.EventArgs e) {
			string mname=DataFunctions.CleanString(MenuName.Text);
			string sqlquery=
				" UPDATE Menus SET "+
				" MenuCode     = '" + MenuCode.Text + "', "+
				" MenuName     = '" + mname + "', "+
				" MenuTarget   = '" + MenuTarget.SelectedItem.Value + "', "+
				" SortOrder    = '" + SortOrder.Text + "', "+
				" MenuTypeCode = '" + MenuTypeCode.SelectedValue + "', "+
				" ParentID     = '" + ParentID.SelectedValue + "' "+
				" WHERE MenuID = " + MenuID.Text;
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
