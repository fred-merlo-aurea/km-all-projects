using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.pagesmanager {

	public partial class eventseditor : System.Web.UI.Page {
		//protected TextBox Description;
		protected System.Web.UI.WebControls.Calendar myCalendar;
		
		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
		public eventseditor() {
			Page.Init += new System.EventHandler(Page_Init);
		}

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SubMenu = "add new event";
            Master.Heading = "Add/Edit Event";

            Master.HelpTitle = "Add/Edit Event";
            val_FromToDates.Visible = false;
           // Description.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID.ToString() + "/images" };
            if (Page.IsPostBack == false)
            {
                int requestEventID = getEventID();
                if (requestEventID > 0)
                { // updating page
                    ECN_Framework.Common.SecurityAccess.canI("Events", requestEventID.ToString());
                    LoadFormData(requestEventID);
                    SetUpdateInfo(requestEventID);
                }
                else
                { // adding new page
                    LoadDropDowns();
                    SetDefaultDate();
                }
            }
        }

		private int getEventID() {
			int theEventID = 0;
			try {
				theEventID = Convert.ToInt32(Request.QueryString["EventID"].ToString());
			}
			catch(Exception E) {
				string devnull = E.ToString();
			}
			return theEventID;
		}

		#region Initialize Wysiswyg Editor
		
		#endregion

		#region date validations
		private bool datesAreOkay() 
		{
			if (StartDate.SelectedDate > EndDate.SelectedDate)
				return false;
			else
				return true;
		}
		
		#endregion

		#region Form Prep

		private void SetUpdateInfo(int setEventID) {
			EventID.Text = setEventID.ToString();
			SaveButton.Text = "Update";
			SaveButton.Visible = false;
			UpdateButton.Visible = true;
            
		}
		#endregion

		#region Data Load
		private void LoadDropDowns() {
			String sqlQuery = "SELECT * FROM Code WHERE CodeType='EventType' order by SortCode;";
			DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
			WebControlFunctions.PopulateDropDownList(EventTypeCode, codeDT, "CodeDisplay", "CodeValue");
		}
		private void SetDefaultDate() 
		{
			StartDate.SelectedDate = DateTime.Today;
			StartDate.VisibleDate = StartDate.SelectedDate;
			EndDate.SelectedDate = DateTime.Today;
			EndDate.VisibleDate = EndDate.SelectedDate;
		}
		private void LoadFormData(int setEventID) {
			String sqlQuery = "SELECT * FROM Events WHERE EventID = " + setEventID;
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			string etc = Convert.ToString(dt.Rows[0]["EventTypeCode"]);
			DataRow dr = dt.Rows[0];
/*			foreach ( DataRow dr in dt.Rows ) {
				EventName.Text = dr["EventName"].ToString();
				Times.Text	= dr["Times"].ToString();
				Location.Text = dr["Location"].ToString();
				if(dr["DisplayFlag"].ToString() == "Y") {
					DisplayFlag.Checked = true;
				}
				StartDate.SelectedDate = Convert.ToDateTime(dr["StartDate"].ToString());
				StartDate.VisibleDate = StartDate.SelectedDate;
				EndDate.SelectedDate = Convert.ToDateTime(dr["EndDate"].ToString());
				EndDate.VisibleDate = EndDate.SelectedDate;
				sqlQuery = "SELECT * FROM Codes WHERE CodeType='EventType';";
				DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
				WebControlFunctions.PopulateDropDownList(EventTypeCode, codeDT, "CodeDisplay", "CodeValue");
				WebControlFunctions.SetSelectedIndex(EventTypeCode, dr["EventTypeCode"].ToString());
				Description.Text = dr["Description"].ToString();
			}*/
			if ((etc == "event") || (etc == "show") || (etc == "training")) 
			{
				EventName.Text = dr["EventName"].ToString();
				Times.Text	= StringFunctions.DBString(dr["Times"],null);
				Location.Text = StringFunctions.DBString(dr["Location"],null);
				if(dr["DisplayFlag"].ToString() == "Y") 
				{
					DisplayFlag.Checked = true;
				}
				StartDate.SelectedDate = Convert.ToDateTime(dr["StartDate"].ToString());
				StartDate.VisibleDate = StartDate.SelectedDate;
				EndDate.SelectedDate = Convert.ToDateTime(dr["EndDate"].ToString());
				EndDate.VisibleDate = EndDate.SelectedDate;
				sqlQuery = "SELECT * FROM Code WHERE CodeType='EventType';";
				DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
				WebControlFunctions.PopulateDropDownList(EventTypeCode, codeDT, "CodeDisplay", "CodeValue");
				WebControlFunctions.SetSelectedIndex(EventTypeCode, dr["EventTypeCode"].ToString());
				Description.Text = StringFunctions.DBString(dr["Description"],null);

				specialPanel.Visible = true;
			}
			else 
			{
				EventName.Text = dr["EventName"].ToString();
				//Times.Text	= StringFunctions.DBString(dr["Times"],null);
				//Location.Text = StringFunctions.DBString(dr["Location"],null);
				if(dr["DisplayFlag"].ToString() == "Y") 
				{
					DisplayFlag.Checked = true;
				}
				//StartDate.SelectedDate = Convert.ToDateTime(dr["StartDate"].ToString());
				//StartDate.VisibleDate = StartDate.SelectedDate;
				//EndDate.SelectedDate = Convert.ToDateTime(dr["EndDate"].ToString());
				//EndDate.VisibleDate = EndDate.SelectedDate;
				sqlQuery = "SELECT * FROM Code WHERE CodeType='EventType';";
				DataTable codeDT = DataFunctions.GetDataTable(sqlQuery);
				WebControlFunctions.PopulateDropDownList(EventTypeCode, codeDT, "CodeDisplay", "CodeValue");
				WebControlFunctions.SetSelectedIndex(EventTypeCode, dr["EventTypeCode"].ToString());
                Description.Text = StringFunctions.DBString(dr["Description"], null);

				specialPanel.Visible = false;
			}
		}
		#endregion

		#region Data Handlers

		public void CreateEvent(object sender, System.EventArgs e) {
			if (datesAreOkay()) 
			{
				string ename       = DataFunctions.CleanString(EventName.Text);
                string description = DataFunctions.CleanString(Description.Text);
				char displayFlag = 'N';
				if(DisplayFlag.Checked) 
				{
					displayFlag = 'Y';
				}
				string sqlquery=
					" INSERT INTO Events ( "+
					" CustomerID, EventTypeCode, EventName, Times, "+
					" Location, Description, StartDate, EndDate, DisplayFlag "+
					" ) VALUES ( "+
					" '"+sc.CustomerID()+"', '"+EventTypeCode.SelectedValue+"', '"+ename+"', '"+Times.Text+"', "+
					" '"+Location.Text+"', '"+description+"', '"+StartDate.SelectedDate+"', '"+EndDate.SelectedDate+"', '"+displayFlag+"' "+
					" ) ";
				DataFunctions.Execute(sqlquery);
				Response.Redirect("default.aspx");
			} 
			else 
			{
				val_FromToDates.Visible = true;
			}
		}

		public void UpdateEvent(object sender, System.EventArgs e) {
			if (datesAreOkay()) 
			{
				string ename=DataFunctions.CleanString(EventName.Text);
                string description = DataFunctions.CleanString(Description.Text);
				char displayFlag = 'N';
				if(DisplayFlag.Checked) 
				{
					displayFlag = 'Y';
				}
				string sqlquery = "";
				string etc = EventTypeCode.Items[EventTypeCode.SelectedIndex].Value;
				if ((etc == "event") || (etc == "show") || (etc == "training")) 
				{
					sqlquery=
						" UPDATE Events SET "+
						" EventTypeCode='"+EventTypeCode.SelectedValue+"', "+
						" EventName='"+ename+"', "+
						" Times='"+Times.Text+"', "+
						" Location='"+Location.Text+"', "+
						" StartDate='"+StartDate.SelectedDate+"', "+
						" EndDate='"+EndDate.SelectedDate+"', "+
						" Description='"+description+"', "+
						" DisplayFlag='"+displayFlag+"' "+
						" WHERE EventID="+EventID.Text;
				} 
				else 
				{
					sqlquery=
						" UPDATE Events SET "+
						" EventTypeCode='"+EventTypeCode.SelectedValue+"', "+
						" EventName='"+ename+"', "+
						" Description='"+description+"', "+
						" DisplayFlag='"+displayFlag+"' "+
						" WHERE EventID="+EventID.Text;
				}
				DataFunctions.Execute(sqlquery);
				Response.Redirect("default.aspx");
			} 
			else 
			{
				val_FromToDates.Visible = true;
			}
		}

		#endregion

		protected void Page_Init(object sender, EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		#region Web Form Designer generated code
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
		#endregion

		private void StartDate_SelectionChanged(object sender, System.EventArgs e)
		{
		
		}

		protected void EventTypeCode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if ((EventTypeCode.SelectedValue.CompareTo("show") == 0) || (EventTypeCode.SelectedValue.CompareTo("events") == 0) || (EventTypeCode.SelectedValue.CompareTo("training") == 0)) 
			{
				specialPanel.Visible = true;
			} 
			else 
			{
				specialPanel.Visible = false;
			}
		
		}


	}
}
