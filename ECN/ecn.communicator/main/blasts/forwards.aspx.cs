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
using ECN_Framework_Common.Objects;

namespace ecn.communicator.blastsmanager 
{
    public partial class forwards : ECN_Framework.WebPageHelper 
    {
       
        int _pagerCurrentPage = 1;
        public int pagerCurrentPage {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage - 1; }
        }

		protected void Page_Load(object sender, System.EventArgs e) {

           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS; 
            Master.SubMenu = "";
            Master.Heading = "Forwards Report";
            Master.HelpContent = "<p><b>Forwards</b><br />Lists all email address who forwarded the email that was sent in the blast.";
            Master.HelpTitle = "Blast Manager";	

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "viewreport") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportForwards, KMPlatform.Enums.Access.ViewDetails))
            {
                if (getBlastID() >0 || getCampaignItemID() > 0)
                {
                    loadForwardsGrid();
                }
			}
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };			
			}
		}

        private int getBlastID() {
            try {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            } catch {
                return 0;
            }
        }

        private int getCampaignItemID() {
            try {
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            } catch (Exception E) {
                return 0;
            }
        }
	

		private string getISP() 
		{
			string  sISP = "";
			try 
			{
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
					sISP = Request.QueryString["isp"].ToString();
			}
			catch 
			{
			}
			return sISP;
		}

		private void loadForwardsGrid() 
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "refer", "", getISP(), pagerCurrentPage, ForwardsPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            ForwardsGrid.DataSource = ds.Tables[1].DefaultView;
            ForwardsGrid.CurrentPageIndex = 0;
            ForwardsGrid.DataBind();
            ForwardsPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
		}

        public void ForwardsPager_IndexChanged(object sender, EventArgs e) {
            pagerCurrentPage = ForwardsPager.CurrentPage;
            loadForwardsGrid();
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

	}
}
