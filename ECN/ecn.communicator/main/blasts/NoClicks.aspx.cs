using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ecn.communicator.main.blasts
{
	public partial class NoClicks : ECN_Framework.WebPageHelper
    {
        private const string NoClickReport = "No Click Report";
        ArrayList columnHeadings = new ArrayList();
        IEnumerator aListEnum = null;

        DataTable emailstable;

        int _pagerCurrentPage = 1;
        public int pagerCurrentPage
        {
            set
            {
                _pagerCurrentPage = value;
            }
            get
            {
                return _pagerCurrentPage - 1;
            }
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SetValues(MenuCode.REPORTS, string.Empty, NoClickReport, string.Empty, BlastManager);

            if (KM.Platform.User.HasAccess(
                Master.UserSession.CurrentUser, 
                Services.EMAILMARKETING, 
                ServiceFeatures.BlastReportNoClicks, 
                Access.ViewDetails))
            {
                if (!Page.IsPostBack)
                    loadNoClicksGrid();
                ShowHideDownload();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void loadNoClicksGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "noclick", "", getISP(), pagerCurrentPage, NoClicksPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            NoClicksGrid.DataSource = ds.Tables[1].DefaultView;
            NoClicksGrid.CurrentPageIndex = 0;
            NoClicksGrid.DataBind();
            NoClicksPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public void downloadUnsubscribedEmails(object sender, System.EventArgs e)
        {
            string newline = "";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

            string downloadType = DownloadType.SelectedItem.Value.ToString();
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID.ToString() + "-" + getBlastID() + "-noClicks-emails";

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "noclick", "", getISP(), Master.UserSession.CurrentUser);

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
            this.NoClicksPager.IndexChanged += new EventHandler(NoClicksPager_IndexChanged);
        }
        #endregion

        private void NoClicksPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = NoClicksPager.CurrentPage;
            loadNoClicksGrid();
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportNoClicks, KMPlatform.Enums.Access.Download))
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