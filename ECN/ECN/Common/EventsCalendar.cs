using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

using System.Data;

namespace ecn.common.classes
{
	
	
	
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:EventsCalendar runat=server></{0}:EventsCalendar>")]
	public class EventsCalendar : System.Web.UI.WebControls.Calendar
	{
		private DataTable dt;

		public object DataSource 
		{
			set 
			{
				dt = (DataTable) value;
			}
		}
		 
		/// Render this control to the output parameter specified.
		
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			base.Render(output);
		}

		public void DayWriter(object sender, DayRenderEventArgs e) 
		{
			foreach (DataRow dr in dt.Rows) 
			{
				if (e.Day.Date == ((DateTime)dr["StartDate"]).Date) 
				{
					e.Cell.Text = (string) dr["EventName"];
				}
			}
		}


	}
}
