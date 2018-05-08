using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Role = KM.Platform.User;

namespace ecn.communicator.blastsmanager 
{
	
	public partial class active : ECN_Framework.WebPageHelper 
    {
		protected System.Web.UI.WebControls.HyperLinkColumn ActiveGridStatusLink;
		protected System.Web.UI.WebControls.HyperLinkColumn ActiveGridCancelLink;

        KMPlatform.Entity.User SessionCurrentUser { get { return Master.UserSession.CurrentUser; } }

		protected void Page_Load(object sender, System.EventArgs e) {

            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "active customer blasts";
            Master.Heading = "Blasts/Reporting > Active Customer Blasts";
            Master.HelpContent = "<b>Blast List</b><p>Lists the emails scheduled for future blasts and emails that are sent past.</p><b>Setup Blast</b><p>Allows you to schedule the created emails for blasts.</p><b>Summary Reports</b><p>Individual statistics of number of clickthroughs on your email.</p><b>Active Emails</b><p>Lists the active blasts in progress.</p><b>Scheduled Emails</b><p>This section lists the emails<br />Created and <i>scheduled</i> for a Blast</p>";
            Master.HelpTitle = "Blast Manager";	

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") || Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) ||KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            //if (Role.IsAdministratorOrHasUserPermission(SessionCurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (Role.HasAccess(SessionCurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View) ||
                Role.HasAccess(SessionCurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View) 
                )
            {
				loadActiveGrid();
				loadScheduledGrid();
			}
            else
            {
				Response.Redirect("../default.aspx");			
			}
		}

		private void loadActiveGrid()
        {
            DataTable dt;
            if (Master.UserSession.CurrentBaseChannel.BaseChannelID.Equals("1"))
            {
                dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGridByStatus(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active, null);
			}
            else
            {
				dt= ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGridByStatus(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active, Master.UserSession.CurrentBaseChannel.BaseChannelID);
			}

            dt.Columns["EmailSubject"].ReadOnly = false;

            foreach(DataRow dr in dt.Rows)
            {
                dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
            }

			ActiveGrid.DataSource=dt.DefaultView;
			ActiveGrid.DataBind();
			if (dt.Rows.Count>0)
            {
				ActivePanel.Visible=true;
			}
		}

		private void loadScheduledGrid() 
        {
			 DataTable dt;
            if (Master.UserSession.CurrentBaseChannel.BaseChannelID.Equals("1"))
            {
                dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGridByStatus(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active, null);
			}
            else
            {
                dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGridByStatus(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active, Master.UserSession.CurrentBaseChannel.BaseChannelID);
			}
            dt.Columns["EmailSubject"].ReadOnly = false;

            foreach(DataRow dr in dt.Rows)
            {
                dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
            }

            ScheduledGrid.DataSource=dt.DefaultView;
			ScheduledGrid.DataBind();
			ScheduledPager.RecordCount = dt.Rows.Count;
		}

	}
}
