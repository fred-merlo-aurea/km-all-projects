using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace ecn.communicator.main.events
{
	public partial class _default : ECN_Framework.WebPageHelper
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.EVENTS; 
            Master.SubMenu = "";
            Master.Heading = "Default Event";
            Master.HelpContent = "<b>Blast setup</b><p>Schedule the emails to blast. <br /> <font color=#FF0000> Note: System prohibits sending the same campaign to the same group within 7 days.</font></p><p><ul><li>Select the campaign from the <i>Campaign</i> Dropdown list.</.li><br /><br /><li>Select the group you want the emails to be sent from the <i>Groups</i> Dropdown list.</li><br/><br/><li>Enter the <i>From email address</i>, <i>From Name</i> and the <i>Subject</i> of the email.</li><br/><br/><li>If you want the Blast to be scheduled now, hit the <i>BlastNow!</i> button. If you want the blast to be scheduled for a later date, set the date and time from <i>Send Time</i> dropdown lists and click on <i>Create Schedule<i> button.</li></ul></p>";
            Master.HelpTitle = "Blast Planner";	
		}

	}
}
