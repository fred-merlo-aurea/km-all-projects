using System;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;


namespace ecn.communicator.main.blasts.Report
{
    public partial class ABSummaryReport : ReportPageBase
    {
        private IABSummaryReportProxy _abSummaryReportProxy;

        public ABSummaryReport(
            IABSummaryReportProxy abSummaryReportProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _abSummaryReportProxy = abSummaryReportProxy;
        }

        public ABSummaryReport()
        {
            _abSummaryReportProxy = new ABSummaryReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "ab summary report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ABSummaryReport, KMPlatform.Enums.Access.View))
                {
                    txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
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
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            var sessionDataProvider = SafeGetSessionDataProvider(Master.UserSession);
            var customerId = sessionDataProvider.GetCurrentUser().CustomerID;
            var abSummaryReportList = _abSummaryReportProxy.Get(customerId, startDate, endDate);

            var dataSource = new ReportDataSource("DataSet1", abSummaryReportList);
            var reportLocation = ConfigurationManager.AppSettings["ReportPath"] + "rpt_ABSummary.rdlc";
            var filePath = Server.MapPath(reportLocation);
            var outputType = exportFormat.ToUpper();
            var outputFileName = string.Format("ABSummary.{0}", exportFormat);
            var parameters = new[]
            {
                new ReportParameter("StartDate", txtstartDate.Text),
                new ReportParameter("EndDate", txtendDate.Text)
            };

            ReportViewer1.Visible = true;

            if (!exportFormat.Equals(ReportConsts.OutputTypeHTML, StringComparison.OrdinalIgnoreCase))
            {
                CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
            }
        }
    }
}
