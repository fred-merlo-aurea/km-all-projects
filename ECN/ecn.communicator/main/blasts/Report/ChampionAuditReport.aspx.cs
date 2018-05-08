using System;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using ECN_Framework.Common;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Common.Functions;

namespace ecn.communicator.main.blasts.Report
{
    public partial class ChampionAuditReport : ReportPageBase
    {
        private IChampionAuditReportProxy _championAuditReportProxy;

        public ChampionAuditReport(
            IChampionAuditReportProxy championAuditReportProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _championAuditReportProxy = championAuditReportProxy;
        }

        public ChampionAuditReport()
        {
            _championAuditReportProxy = new ChampionAuditReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "Champion Audit Report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ChampionAuditReport, KMPlatform.Enums.Access.View))
                {
                    txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    if(!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ChampionAuditReport, KMPlatform.Enums.Access.Download))
                    {
                        drpExport.Items.Clear();
                        drpExport.Items.Add(new ListItem() { Text = "HTML", Value = "html", Selected = true });
                    }
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            RenderReport(drpExport.SelectedItem.Text);
        }

        private void RenderReport(string exportFormat)
        {
            phError.Visible = false;

            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            if (ValidateDateRange(startDate, endDate))
            {
                var sessionDataProvider = SafeGetSessionDataProvider(Master.UserSession);
                var customerId = sessionDataProvider.GetCurrentUser().CustomerID;
                var championAuditReportList = _championAuditReportProxy.Get(customerId, startDate, endDate);

                foreach (var championAuditReport in championAuditReportList)
                {
                    championAuditReport.EmailSubject = EmojiFunctions.GetSubjectUTF(championAuditReport.EmailSubject);
                }

                var dataSource = new ReportDataSource("DataSet1", championAuditReportList);
                var reportLocation = ConfigurationManager.AppSettings["ReportPath"] + "rpt_ChampionAudit.rdlc";
                var filePath = Server.MapPath(reportLocation);
                var outputType = exportFormat.ToUpper();
                var outputFileName = string.Format("ChampionAuditReport.{0}", exportFormat);
                var parameters = new[]
                {
                    new ReportParameter("StartDate", startDate.ToShortDateString()),
                    new ReportParameter("EndDate", endDate.ToShortDateString())
                };

                ReportViewer1.Visible = true;
                ReportViewer1.PageCountMode = PageCountMode.Actual;
                ReportViewer1.ShowBackButton = false;

                if (exportFormat.ToUpper() != "HTML")
                {
                    CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
                }
            }
            else
            {
                ReportViewer1.Visible = false;
                lblErrorMessage.Text = "Please enter a valid date starting no earlier than the last year";
                phError.Visible = true;
            }
        }

        private static bool ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.Now;
            TimeSpan dateRange = now - startDate;
            TimeSpan yearSpan = new TimeSpan(365, 0, 0, 0);

            if (startDate <= endDate && dateRange < yearSpan)
            {
                return true;
            }
            else 
                return false;

        }
    }
}
