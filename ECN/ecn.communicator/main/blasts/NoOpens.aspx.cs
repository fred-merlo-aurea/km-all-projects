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
using System.IO;
using System.Configuration;
using ECN_Framework_Common.Objects;
using System.Linq;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework_BusinessLayer.Application;
using static KMPlatform.Enums;

namespace ecn.communicator.main.blasts
{

    public partial class NoOpens : ECN_Framework.WebPageHelper
    {
        private const string UnopenedEmailReports = "Unopened Email Reports";
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
            Master.SetValues(MenuCode.REPORTS, string.Empty, UnopenedEmailReports, string.Empty, BlastManager);

            if (KM.Platform.User.HasAccess(
                ECNSession.CurrentSession().CurrentUser, 
                Services.EMAILMARKETING, 
                ServiceFeatures.BlastReportUnopened, 
                Access.ViewDetails))
            {
                if (!Page.IsPostBack)
                    loadNoOpensGrid();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        private void loadNoOpensGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "noopen", "", getISP(), pagerCurrentPage, NoOpensPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            NoOpensGrid.DataSource = ds.Tables[1].DefaultView;
            NoOpensGrid.CurrentPageIndex = 0;
            NoOpensGrid.DataBind();
            NoOpensPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public void downloadUnsubscribedEmails(object sender, System.EventArgs e)
        {
            string newline = "";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/Customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            string downloadType = DownloadType.SelectedItem.Value.ToString();

            DateTime date = DateTime.Now;
            string tfile = Master.UserSession.CurrentUser.CustomerID.ToString() + "-" + getBlastID() + "-noOpens-emails";

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "noopen", "", getISP(), Master.UserSession.CurrentUser);

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
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

            this.NoOpensPager.IndexChanged += new EventHandler(NoOpensPager_IndexChanged);
        }
        #endregion

        private void NoOpensPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = NoOpensPager.CurrentPage;
            loadNoOpensGrid();
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