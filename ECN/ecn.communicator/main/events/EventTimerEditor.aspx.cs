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

namespace ecn.communicator.main.events
{
	
	
	
	public partial class EventTimerEditor : ECN_Framework.WebPageHelper
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
             Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.EVENTS; 
            Master.SubMenu = "";
            Master.Heading = "Event Timer";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icoblasts.gif><b>Blast setup</b><br />Schedule the emails to blast. <br /> <font color=#FF0000> Note: System prohibits sending the same campaign to the same group within 7 days.</font><br /><br />Select the campaign from the <i>Campaign</i> Dropdown list.<br />Select the group you want the emails to be sent from the <i>Groups</i> Dropdown list.<br /><br />Enter the <i>From email address</i>, <i>From Name</i> and the <i>Subject</i> of the email.<br /><br />If you want the Blast to be scheduled now, hit the <i>BlastNow!</i> button.<br />If you want the blast to be scheduled for a later date, set the date and time from <i>Send Time</i> dropdown lists and click on <i>Create Schedule<i> button.";
            Master.HelpTitle = "Event Timer";	
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
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
