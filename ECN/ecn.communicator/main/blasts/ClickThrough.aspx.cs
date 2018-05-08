using System;
using System.Web.UI;
using System.Data;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ecn.communicator.main.blasts
{
	public partial class ClickThrough : ECN_Framework.WebPageHelper
    {
        private const string HelpContent = "<p><b>Click-Through</b><br />Lists unique recepients who clicked on the URL links in your email Blast<br />Displays the time clicked, the URL link clicked.<br />Click on the email address to view the profile of that email address.";
        private const string ClickThroughRatioReport = "Click Through Ratio Report";

        public int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            else
                return 0;
        }

        public int getCampaignItemID()
        {
            if (Request.QueryString["CampaignItemID"] != null)
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            else
                return 0;
        }

        public string getClicksLinkURL()
        {
            try
            {
                string returnURL = "";

                if (getBlastID() > 0)
                {
                    returnURL = "?BlastID=" + getBlastID();

                    if (getUDFData() != string.Empty)
                        returnURL += "&UDFName=" + getUDFName() + "&UDFData=" + getUDFData();

                    return returnURL;
                }
                else
                {
                    return "?CampaignItemID=" + getCampaignItemID();
                }
            }
            catch (Exception E) { return "?BlastID=" + getBlastID(); }
        }

        private string getISP()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
                    return Request.QueryString["isp"].ToString();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.SetValues(MenuCode.REPORTS, string.Empty, ClickThroughRatioReport, HelpContent, BlastManager);

            if(!Page.IsPostBack)
            {
                if (KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, Services.EMAILMARKETING, ServiceFeatures.BlastReportClickThroughRatio, Access.ViewDetails))
                {
                    DownloadPanel.Visible = KMPlatform.BusinessLogic.User.HasAccess(
                        Master.UserSession.CurrentUser, 
                        Services.EMAILMARKETING, 
                        ServiceFeatures.BlastReportClickThroughRatio, 
                        Access.DownloadDetails);
                    LoadData();
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
            }
        }

        private void LoadData()
        {
            DataSet dtCampaignItems = new DataSet();
            if (getBlastID() > 0)
            {
                dtCampaignItems = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGroupClicksData(null, getBlastID(), "",
                           getISP(), "uniqueclicks", "", dgClickThrough.PageIndex + 1, dgClickThrough.PageSize, getUDFName(), getUDFData());
            }
            else
            {
                dtCampaignItems = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastGroupClicksData(getCampaignItemID(), null, "",
                           getISP(), "uniqueclicks", "", dgClickThrough.PageIndex + 1, dgClickThrough.PageSize, getUDFName(), getUDFData());
            }
            dgClickThrough.DataSource = dtCampaignItems.Tables[0];
            dgClickThrough.DataBind();
            ClicksPager.RecordCount = Convert.ToInt32(dtCampaignItems.Tables[0].Rows.Count > 0 ? dtCampaignItems.Tables[0].Rows[0]["TotalCount"] : 0);

        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        //protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dgClickThrough.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue.ToString());
        //    LoadData();
        //}

        //protected void txtGoToPage_TextChanged(object sender, EventArgs e)
        //{
        //    int newPage = Convert.ToInt32(txtGoToPage.Text);
        //    if (newPage > 0 && newPage < Convert.ToInt32(lblTotalNumberOfPages.Text))
        //    {
        //        dgClickThrough.PageIndex = newPage;
        //        LoadData();
        //    }
        //}

        //protected void btnPrevious_Click(object sender, EventArgs e)
        //{
        //    if (dgClickThrough.PageIndex > 0)
        //    {
        //        dgClickThrough.PageIndex--;
        //        LoadData();
        //    }
        //}

        //protected void btnNext_Click(object sender, EventArgs e)
        //{
        //    if (dgClickThrough.PageIndex + 1 < Convert.ToInt32(lblTotalNumberOfPages.Text))
        //    {
        //        dgClickThrough.PageIndex++;
        //        LoadData();
        //    }
        //}

        protected void DownloadEmailsButton_Click(object sender, EventArgs e)
        {
            DataTable emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "uniqueclicks", "", getISP(), Master.UserSession.CurrentUser);

            ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToTab(ECN_Framework_Common.Functions.DataTableFunctions.ToTabDelimited(emailstable),"UniqueClicks-" + (getBlastID() > 0 ? getBlastID() : getCampaignItemID()).ToString()) ;
        }

        protected void ClicksPager_IndexChanged(object sender, EventArgs e)
        {
            dgClickThrough.PageIndex = ClicksPager.CurrentPage - 1;
            LoadData();
        }
    }
}