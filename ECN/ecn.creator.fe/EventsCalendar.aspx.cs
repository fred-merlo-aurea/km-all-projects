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

using ecn.common.classes;
using BWare.UI.Web.WebControls;

namespace ecn.creator.includes
{
	/// <summary>
	/// Summary description for EventsCalendar.
	/// </summary>
	public class EventsCalendar : System.Web.UI.Page
	{
		protected BWare.UI.Web.WebControls.DataPanel calendarPanel;
		protected BWare.UI.Web.WebControls.DataPanel detailsPanel;
		protected System.Web.UI.WebControls.Calendar eCal;
		protected System.Web.UI.WebControls.Label lblDetails;
		protected System.Web.UI.HtmlControls.HtmlForm Form1;
		private DataRow []cdr;		

		private void DayRender(object sender, DayRenderEventArgs e)  {
			bool found = false;
			cdr = (DataRow[])Session["dr"];
			string strTip = "";
			if (! e.Day.IsOtherMonth ) {
				foreach (DataRow dr in cdr)  {
					if ((dr["StartDate"].ToString() != DBNull.Value.ToString())) {
						DateTime dtFromEvent= (DateTime)dr["StartDate"];
						DateTime dtToEvent= (DateTime)dr["EndDate"];
						if (dtFromEvent.Equals(e.Day.Date) || dtToEvent.Equals(e.Day.Date) || 
							(DateTime.Compare(dtFromEvent, e.Day.Date) < 0 && DateTime.Compare(dtToEvent, e.Day.Date) > 0)) {
							string eventCode = dr["EventTypeCode"].ToString();
							if(eventCode == "training"){
								//strTip = "Ends On: "+ StringFunctions.DBString(dr["EndDate"],null)+"\n"+"Time: "+StringFunctions.DBString(dr["Times"],null)+"\n"+
								//	"Location: "+StringFunctions.DBString(dr["Location"],null);
								e.Cell.Text += "<a href='EventsCalendar.aspx?status=pb&evntID="+dr["EventID"].ToString()+"#aDetail ' style=\"text-decoration: none;color:#000000;\"><div align=left style=\"padding-left:2px;background-color:#FFCC66\"><font size=1><b>"+StringFunctions.DBString(dr["EventName"],null)+"</b></a><br><u>Time</u>:<br>"+dr["Times"].ToString()+"</font></div>";
							}else if(eventCode == "show"){
								//strTip = "Ends On: "+ StringFunctions.DBString(dr["EndDate"],null)+"\n"+"Time: "+StringFunctions.DBString(dr["Times"],null)+"\n"+
								//	"Location: "+StringFunctions.DBString(dr["Location"],null);
								e.Cell.Text += "<a href='EventsCalendar.aspx?status=pb&evntID="+dr["EventID"].ToString()+"#aDetail ' style=\"text-decoration: none;color:#000000;\"><div align=left style=\"padding-left:2px;background-color:#aa9900\"><font size=1><b>"+StringFunctions.DBString(dr["EventName"],null)+"</b></a><br><u>Time</u>:<br>"+dr["Times"].ToString()+"</font></div>";						
							}else if(eventCode == "event"){
								//strTip = "Ends On: "+ StringFunctions.DBString(dr["EndDate"],null)+"\n"+"Time: "+StringFunctions.DBString(dr["Times"],null)+"\n"+
								//	"Location: "+StringFunctions.DBString(dr["Location"],null);
								e.Cell.Text += "<a href='EventsCalendar.aspx?status=pb&evntID="+dr["EventID"].ToString()+"#aDetail ' style=\"text-decoration: none;color:#000000;\"><div align=left style=\"padding-left:2px;background-color:#ccffcc\"><font size=1><b>"+StringFunctions.DBString(dr["EventName"],null)+"</b></a><br><u>Time</u>:<br>"+dr["Times"].ToString()+"</font></div>";						
							}
							e.Cell.Height = System.Web.UI.WebControls.Unit.Pixel(50);
							e.Cell.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Top;
							e.Cell.BackColor = Color.FromArgb(255,239,213);

							//found = true;
							//break;
						}else{
							e.Cell.Height = System.Web.UI.WebControls.Unit.Pixel(60);					
						}
					}
				}
			}else {
				//If the month is not CurrentMonth then hide the Dates
				//e.Cell.Text = "";
			}
			/*if (!found) 
			{
				e.Cell.Controls.Clear();
				e.Cell.Controls.AddAt(0,new LiteralControl(Convert.ToString(e.Day.Date.Day)));
				found = false;
			}*/
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			eCal.DayRender += new DayRenderEventHandler(DayRender);
			if (Request.QueryString["status"] == "pb")
				showDetails();
		}

		private void showDetails() 
		{
			string details = "";
			cdr = (DataRow[])Session["dr"];
			try 
			{
				foreach (DataRow dr in cdr) 
				{
					if (Request.QueryString["evntID"] == Convert.ToString(dr["EventID"])) 
					{
						if (dr["EndDate"] != null)
							details = "<b>Ends On:</b> "+((DateTime)dr["EndDate"]).ToShortDateString()+"<br>";
						details += "<b>Time:</b> "+StringFunctions.DBString(dr["Times"],"No Timings are Specified")+"<br>";
						details += "<b>Location:</b> "+StringFunctions.DBString(dr["Location"],"No Location is Specified")+"<br>";
						details += "<b>Description:</b> "+StringFunctions.DBString(dr["Description"],"No Description is Specified");
						//break;
					}
				}
			} 
			catch {}
			lblDetails.Text = details;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.eCal.SelectionChanged += new System.EventHandler(this.eCal_SelectionChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void eCal_SelectionChanged(object sender, System.EventArgs e)
		{
		}
	}
}
