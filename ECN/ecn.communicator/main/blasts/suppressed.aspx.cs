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
    public partial class suppressed : ECN_Framework.WebPageHelper
    {

        ArrayList columnHeadings = new ArrayList();
        IEnumerator aListEnum = null;

        DataTable emailstable;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "";
            Master.Heading = "Suppressed Reports";
            Master.HelpContent = "";
            Master.HelpTitle = "Blast Manager";

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "blastpriv") 
            //|| KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "viewreport") 
            //|| KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Delivery_Report, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportSuppressed, KMPlatform.Enums.Access.ViewDetails))
            {
                if (getBlastID() > 0 || getCampaignItemID() > 0)
                {
                    if (getBlastID() > 0)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItem ciSource =
                        ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(getBlastID(), false);
                        if (ciSource != null)
                        {
                            if (ciSource.CustomerID.Value == Master.UserSession.CurrentUser.CustomerID)
                            {
                                setSuppressedVisibile(true);
                            }
                            else
                            {
                                setSuppressedVisibile(false);
                            }
                        }
                        else
                        {
                            setSuppressedVisibile(false);
                        }
                    }
                    else
                    {
                        setSuppressedVisibile(false);
                    }
                }

                if (!(Page.IsPostBack))
                {
                    loadSuppressedGrid();
                    ShowHideDownload();

                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }

        }

        private void setSuppressedVisibile(bool visibility)
        {
            SendSuppressedButton.Enabled = visibility;
            SendSuppressedButton.Visible = visibility;
        }

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
            catch
            {
                return 0;
            }
        }

        private string getValue()
        {
            if (Request.QueryString["value"] != null)
                return Request.QueryString["value"].ToString();
            else
                return "ALL";
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
            try
            {
                return Request.QueryString["UDFName"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getUDFData()
        {
            try
            {
                return Request.QueryString["UDFdata"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private void loadSuppressedGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "suppressed", getValue(), getISP(), pagerCurrentPage, SuppressedPager.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            SuppressedGrid.DataSource = ds.Tables[1].DefaultView;
            SuppressedGrid.CurrentPageIndex = 0;
            SuppressedGrid.DataBind();
            SuppressedPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

        }

        public void SuppressedPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = SuppressedPager.CurrentPage;
            loadSuppressedGrid();
        }

        public void downloadSuppressedEmails(object sender, System.EventArgs e)
        {
            string newline = "";
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            if (!Directory.Exists(txtoutFilePath))
                Directory.CreateDirectory(txtoutFilePath);


            string downloadType = DownloadType.SelectedItem.Value.ToString();
            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-suppressed-emails";

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "suppressed", getValue(), getISP(), Master.UserSession.CurrentUser);

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

        protected void SendSuppressed(object sender, EventArgs e)
        {

            Response.Redirect("../../main/ecnwizard/wizardsetup.aspx?PrePopBlastID=" + getBlastID() + "&PrePopSmartSegmentID=4");
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportSuppressed, KMPlatform.Enums.Access.Download))
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