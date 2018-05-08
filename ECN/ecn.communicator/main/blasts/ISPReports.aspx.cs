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
using System.Configuration;

namespace ecn.communicator.main.blasts
{
	public partial class ISPReports : ECN_Framework.WebPageHelper
	{
		protected System.Web.UI.WebControls.DataGrid TopGrid;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Blast ISP Reporting";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icoblasts.gif><b>Reports</b><br />Gives a report of the Blast in progress.<br />Click on <i>view log</i> to view the log of the emails that has received the blast.<br /><i>Clicks</i> specify the total number of URL clicks in your email by the recepients who received the email. Click on the '[number]' to see who clicked &amp; what link was clicked<br /><i>Bounces</i> specify the number of bounced emails recepients or the email recepients who did not received the blast. Click on the '[number]' to see who did not receive the blast.";
            Master.HelpTitle = "Blast Manager";	

			if (!IsPostBack)
			{
                if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
					throw new ECN_Framework_Common.Objects.SecurityException("SECURITY VIOLATION!"); 
				else
					LoadFormData();
			}
		}
        	
		private void LoadFormData() 
		{
            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(getBlastID(), Master.UserSession.CurrentUser, true);
            ECN_Framework_Entities.Communicator.CampaignItemBlast cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID(getBlastID(), Master.UserSession.CurrentUser, true);
            GroupTo.Text = blast.Group.GroupName;
            GroupTo.NavigateUrl = "../lists/groupeditor.aspx?GroupID=" + blast.GroupID;
            if (cib.Filters != null && cib.Filters.Count > 0)
            {
                gvFilters.DataSource = cib.Filters;
                gvFilters.DataBind();
                //Filter.Text = blast.Filter.FilterName;
                //Filter.NavigateUrl = "../lists/filters.aspx?FilterID=" + blast.FilterID;
            }
            Campaign.Text = blast.Layout.LayoutName;
            Campaign.NavigateUrl = "../content/layouteditor.aspx?LayoutID=" + blast.LayoutID;
            EmailSubject.Text = blast.EmailSubject;
            EmailFrom.Text = blast.EmailFromName + " &lt; " + blast.EmailFrom + " &gt;";
            SendTime.Text = blast.SendTime.ToString();
            FinishTime.Text = blast.FinishTime.ToString();
		}

        public int getBlastID() 
		{
			int theBlastID = 0;
			try 
			{
				theBlastID = Convert.ToInt32(Request.QueryString["BlastID"].ToString());
			}
			catch(Exception E) 
			{
				string devnull=E.ToString();
			}
			return theBlastID;
		}

		protected void btnReport_Click(object sender, System.EventArgs e)
		{
			string SelectedISPs=string.Empty;
			foreach (ListItem item in lstISP.Items)
			{
				if(item.Selected)
				{
					if (SelectedISPs == string.Empty)
						SelectedISPs = item.Value.ToString();
					else
						SelectedISPs += "," + item.Value.ToString();
				}
			}
            dgReport.DataSource = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetISPReport(getBlastID(), SelectedISPs);
            dgReport.DataBind(); 
		}

        protected void gvFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter)e.Row.DataItem;
                ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(cibf.FilterID.Value, Master.UserSession.CurrentUser);
                HyperLink hlFilter = (HyperLink)e.Row.FindControl("hlFilterName");
                hlFilter.Text = f.FilterName;
                hlFilter.NavigateUrl = "../lists/filters.aspx?FilterID=" + f.FilterID.ToString();
            }
        }
	}
}
