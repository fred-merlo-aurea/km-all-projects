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

namespace ecn.communicator.main.blasts
{

	public partial class Sends : ECN_Framework.WebPageHelper
    {

        int _pagerCurrentPage = 1;
        public int pagerCurrentPage
        {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage - 1; }
        }

        ArrayList columnHeadings = new ArrayList();
        IEnumerator aListEnum = null;
        DataTable emailstable;

        private int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            else
                return 0;
        }

        private int getCampaignItemID()
        {
            if (Request.QueryString["CampaignItemID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            }
            else
                return 0;
        }

        private string getISP()
        {
            if (Request.QueryString["isp"] != null)
            {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
                    return Request.QueryString["isp"].ToString();
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SetValues(MenuCode.REPORTS, string.Empty, "Sends Report", string.Empty, BlastManager);

            if (KM.Platform.User.HasAccess(
                Master.UserSession.CurrentUser,
                Services.EMAILMARKETING,
                ServiceFeatures.BlastReportSends,
                Access.ViewDetails))
            { 
                if (!(Page.IsPostBack))
                {
                    loadSendsBlastGrid();
                    ShowHideDownload();
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void loadSendsBlastGrid()
        {
            DataSet ds = new DataSet();
            ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "send", "", getISP(), pagerCurrentPage, SendsPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);


            SendsGrid.DataSource = ds.Tables[1].DefaultView;
            SendsGrid.CurrentPageIndex = 0;
            SendsGrid.DataBind();
            SendsPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public void downloadSendEmails(object sender, System.EventArgs e)
        {
            string newline = "";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            string downloadType = DownloadType.SelectedItem.Value.ToString();
            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-open-emails";

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "send", "", getISP(), Master.UserSession.CurrentUser);

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

        protected void SendsPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = SendsPager.CurrentPage;
            loadSendsBlastGrid();
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportUnopened, KMPlatform.Enums.Access.Download))
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
