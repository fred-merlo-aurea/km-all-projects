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
using ecn.communicator.classes;

namespace ecn.communicator.blastsmanager
{
	
	
	
	public partial class BlastPlanner : ECN_Framework.WebPageHelper
	{
        protected int _userID;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            _userID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.EVENTS; 
            Master.SubMenu = "blast events";
            Master.Heading = "New Events";
            Master.HelpContent = "<b>Blast setup</b><p>Schedule the emails to blast.<br /><font color=#FF0000> Note: System prohibits sending the same campaign to the same group within 7 days.</font></p><p><ul><li>Select the campaign from the <i>Campaign</i> Dropdown list.</li><br/><br/><li>Select the group you want the emails to be sent from the <i>Groups</i> Dropdown list.</li><br/><br/><li>Enter the <i>From email address</i>, <i>From Name</i> and the <i>Subject</i> of the email.</li><br /><br /><li>If you want the Blast to be scheduled now, hit the <i>BlastNow!</i> button. If you want the blast to be scheduled for a later date, set the date and time from <i>Send Time</i> dropdown lists and click on <i>Create Schedule<i> button.</li></ul></p>";
            Master.HelpTitle = "Blast Planner";	

            BlastPlans plan = new BlastPlans();
            if(BlastPlanID()>0) 
            {
                ECN_Framework_BusinessLayer.Communicator.BlastPlans.Delete(BlastPlanID(), Master.UserSession.CurrentUser);
                Response.Redirect("BlastPlanner.aspx");
            }
            plan.CustomerID(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID));
			BlastEvents.DataSource = plan.GetBlastsPlansGrid();
            BlastEvents.CurrentPageIndex = 0;
            BlastEvents.DataBind();
		}

        private int BlastPlanID() 
        {
            int BlastPlanID = 0;
            try 
            {
                BlastPlanID = Convert.ToInt32(Request.QueryString["BlastPlanID"].ToString());
            }
            catch
            {

            }
            return BlastPlanID;
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
