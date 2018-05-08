using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.events {
	public partial class EventList : Page {

		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

		protected void Page_Load(object sender, System.EventArgs e) {
            Master.SubMenu = "event list";
            Master.Heading = "Event List";

            Master.HelpTitle = "Event List";
			if (Page.IsPostBack==false){
				if(!(Request.QueryString["EventID"] == null)) {
					deleteEvent(Convert.ToInt32(Request.QueryString["EventID"]));
				}
				loadEventTypeCodeDR();
				loadEventsGrid();
			}else {
				//loadEventsGrid();
			}
		}
		private void deleteEvent(int eventID) {
            ECN_Framework.Common.SecurityAccess.canI("Events",eventID.ToString());
			string sqlquery = "DELETE FROM Events WHERE EventID = " + eventID;
			DataFunctions.Execute(sqlquery);
		}
		private void loadEventsGrid() {
			string evenTypeWhereClause = "";
			try{
				evenTypeWhereClause = " AND EventTypeCode = '"+EventTypeCode.SelectedItem.Value +"'";
			}catch{

			}
			string sqlquery =	" SELECT EventID, EventName, CONVERT(VARCHAR,StartDate,101) AS 'StartDate', CONVERT(VARCHAR,EndDate,101) as 'EndDate', "+
									" Times, DisplayFlag FROM Events WHERE CustomerID = " + Master.UserSession.CurrentCustomer.CustomerID.ToString() + evenTypeWhereClause + " ORDER BY EventName";
			EventsGrid.DataSource = DataFunctions.GetDataTable(sqlquery);
			EventsGrid.DataBind();
		}

		private void loadEventTypeCodeDR() {
			EventTypeCode.DataSource = DataLists.GetCodesDR("EventType");
			EventTypeCode.DataBind();
		}

		public  void EventType_SelectedIndexChanged(object sender, System.EventArgs e) {
			loadEventsGrid();
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
