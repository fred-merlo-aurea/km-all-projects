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

namespace ecn.communicator.blastsmanager
{
    public partial class subscribes : ECN_Framework.WebPageHelper
    {

        ArrayList columnHeadings = new ArrayList();
        IEnumerator aListEnum = null;
        DataTable emailstable;
        bool _onlyUnique = false;
        int _pagerCurrentPage = 1;

        public int pagerCurrentPage
        {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage - 1; }
        }
        public bool OnlyUnique
        {
            set { _onlyUnique = value; }
            get { return _onlyUnique; }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "";
            Master.Heading = "Subscription Reports";
            Master.HelpContent = "<p><b>Unsubscribes</b><br />Lists all email address who Unsubscribed for this email. <br /><br />Displays the<br />-&nbsp;&nbsp;<i>Time</i> unsubscribed<br />-&nbsp;&nbsp;<i>email address</i> unsubscribed <br />-&nbsp;&nbsp;<i>change</i>";
            Master.HelpTitle = "Blast Manager";
            try
            {
                OnlyUnique = Request.QueryString["OnlyUnique"].ToString().Equals("1");// Convert.ToBoolean(Convert.ToInt32(Request.QueryString["OnlyUnique"]));
            }
            catch
            { }

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "viewreport") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportUnsubscribes, KMPlatform.Enums.Access.ViewDetails))
            {
                if (!(Page.IsPostBack))
                {
                    loadSubscribesGrid();
                    ShowHideDownload();
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }



        private int getBlastID()
        {
            if (Request.QueryString["BlastID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            else
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


        private string getCode()
        {
            if (Request.QueryString["code"] != null)
            {
                return Request.QueryString["code"].ToString();
            }
            else
            {
                return "subscribe";
            }
        }

        private string getISP()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
                    return Request.QueryString["isp"].ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }



        public string getUDFName()
        {
            if (Request.QueryString["UDFName"] != null)
            {
                return Request.QueryString["UDFName"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string getUDFData()
        {
            if (Request.QueryString["UDFdata"] != null)
            {
                return Request.QueryString["UDFdata"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private void loadSubscribesGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "subscribe", getCode(), getISP(), pagerCurrentPage, SubscribesPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser, OnlyUnique);
            SubscribesGrid.DataSource = ds.Tables[1].DefaultView;
            SubscribesGrid.CurrentPageIndex = 0;
            SubscribesGrid.DataBind();
            SubscribesPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        public void SubscribesPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = SubscribesPager.CurrentPage;
            loadSubscribesGrid();
        }

        public void downloadUnsubscribedEmails(object sender, System.EventArgs e)
        {
            string newline = "";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);

            string downloadType = DownloadType.SelectedItem.Value.ToString();
            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-unsubscribed-emails";

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "unsubscribe", getCode(), getISP(), Master.UserSession.CurrentUser, "", "", "ProfilePlusStandAlone", OnlyUnique);

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


        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportUnsubscribes, KMPlatform.Enums.Access.Download))
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