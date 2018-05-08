using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ECN_Framework_Common.Objects;
using System.Linq;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ecn.communicator.blastsmanager
{
    public partial class resends : ECN_Framework.WebPageHelper
    {
        private const string ResendReports = "Resend Reports";
        private const string HelpContent = "<p><b>Resend Soft Bounces</b><br />Lists all email address who Unsubscribed for this email. <br /><br />Displays the<br />-&nbsp;&nbsp;<i>Time</i> unsubscribed<br />-&nbsp;&nbsp;<i>email address</i> unsubscribed <br />-&nbsp;&nbsp;<i>change</i>";
        ArrayList columnHeadings = new ArrayList();
        IEnumerator aListEnum = null;
        DataTable emailstable;

        int _pagerCurrentPage = 1;
        public int pagerCurrentPage
        {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage - 1; }
        }

        private int getBlastID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getCampaignItemID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SetValues(MenuCode.REPORTS, string.Empty, ResendReports, HelpContent, BlastManager);

            if (KM.Platform.User.HasAccess(
                Master.UserSession.CurrentUser, 
                Services.EMAILMARKETING, 
                ServiceFeatures.BlastReportResends, 
                Access.View))
            {
                if (!(Page.IsPostBack))
                {
                    loadResendsGrid();
                    ShowHideDownload();
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private string getISP()
        {
            string sISP = "";
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

        private void loadResendsGrid()
        {

            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails_NoAccessCheck(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "resend", "", getISP(), pagerCurrentPage, ResendsPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            ResendsGrid.DataSource = ds.Tables[1].DefaultView;
            ResendsGrid.CurrentPageIndex = 0;
            ResendsGrid.DataBind();
            ResendsPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public void downloadResentEmails(object sender, System.EventArgs e)
        {
            string newline = "";

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            string downloadType = DownloadType.SelectedItem.Value.ToString();

            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-resend-emails";

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "resend", "", getISP(), Master.UserSession.CurrentUser);

            if (downloadType == ".xls")
            {
                ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToExcelFromDataTbl<DataRow>(emailstable.AsEnumerable().ToList(), tfile);
            }
            else
            {
                string emailsSCV = ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.GetCsvFromDataTable(emailstable);
                if (downloadType == ".csv")
                {
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToCSV(emailsSCV, tfile);
                }
                else
                {
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToTXT(emailsSCV, tfile);
                }
            }
        }
        private static string CleanString(string text)
        {

            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            return text;
        }

        public void ResendsPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = ResendsPager.CurrentPage;
            loadResendsGrid();
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportResends, KMPlatform.Enums.Access.Download))
            {
                DownloadPanel.Visible = false;
            }
            else
            {
                DownloadPanel.Visible = true;
            }

        }
    }
}