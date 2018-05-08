using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Reporting.WebForms;
using ecn.communicator.CommonControls;
using ECN_Framework.Common;
using ECN_Framework.Common.Interfaces;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using KMPlatform.Entity;

namespace ecn.communicator.main.blasts.Report
{
    public partial class EmailsPerformanceByDomainReport : ReportPageBase
    {
        private IEmailPerformanceByDomainReportProxy _emailPerformanceProxy;

        public EmailsPerformanceByDomainReport(
            IEmailPerformanceByDomainReportProxy emailPerformanceProxy,
            IReportDefinitionProvider reportDefinitionProvider,
            IReportContentGenerator reportContentGenerator)
            : base(reportDefinitionProvider, reportContentGenerator)
        {
            _emailPerformanceProxy = emailPerformanceProxy;
        }

        public EmailsPerformanceByDomainReport()
        {
            _emailPerformanceProxy = new EmailPerformanceByDomainReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "Email Performance by Domain report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";
            phError.Visible = false;
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPerformanceByDomainReport, KMPlatform.Enums.Access.View))
                {
                txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

                    if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPerformanceByDomainReport, KMPlatform.Enums.Access.ViewDetails))
                    {
                        chkDrillDownOther.Visible = true;
                    }
                    else
                    {
                        chkDrillDownOther.Visible = false;
                    }

                    if(!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPerformanceByDomainReport, KMPlatform.Enums.Access.Download))
                    {
                        drpExport.Items.Clear();
                        drpExport.Items.Add(new ListItem() { Value = "html", Text = "HTML", Selected = true });
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
            DateValidator dateValidator = new DateValidator();
            bool datesValid = dateValidator.ValidateDates(txtstartDate, txtendDate, 2, phError, lblErrorMessage, true);
            if (datesValid)
            {
                RenderReport(drpExport.SelectedItem.Text);
            }
        }

        private void RenderReport(string exportFormat)
        {
            var sessionDataProvider = SafeGetSessionDataProvider(Master.UserSession);
            var currentUser = sessionDataProvider.GetCurrentUser();
            var customerId = currentUser.CustomerID;

            if (!exportFormat.Equals(ReportConsts.OutputTypeHTML, StringComparison.OrdinalIgnoreCase))
            {
                var hasAccessToReport = HasAccessToReport(currentUser);
                if (!hasAccessToReport)
                {
                    setECNError("You do not have permission to download the details of this report");
                    return;
                }
            }

            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            var downloadDetails = chkDrillDownOther.Checked;
            
            var emailPerformanceByDomainList = _emailPerformanceProxy.Get(
                customerId,
                startDate,
                endDate,
                downloadDetails);
            var dataSource = new ReportDataSource("DataSet1", emailPerformanceByDomainList);
            var parameters = new[]
            {
                new ReportParameter("StartDate", txtstartDate.Text),
                new ReportParameter("EndDate", txtendDate.Text)
            };

            var outputType = drpExport.SelectedItem.Text.ToUpper();
            var outputFileName = string.Format("EmailPerformanceByDomain.{0}", drpExport.SelectedItem.Text);

            var stream = GetReportDefinitionStream();
            ReportViewer1.LocalReport.LoadReportDefinition(stream);
            ReportViewer1.Visible = true;

            if (!exportFormat.Equals(ReportConsts.OutputTypeHTML, StringComparison.OrdinalIgnoreCase))
            {
                CreateReportResponse(ReportViewer1, stream, dataSource, parameters, outputType, outputFileName);
            }
        }

        private bool HasAccessToReport(User user)
        {
            return KM.Platform.User.HasAccess(
                user,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.EmailPerformanceByDomainReport,
                KMPlatform.Enums.Access.DownloadDetails);
        }

        private Stream GetReportDefinitionStream()
        {
            var assemblyLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var reportName = "ECN_Framework_Common.Reports.rpt_EmailPerformanceByDomain.rdlc";
            var reportDefinitionProvider = GetReportDefinitionProvider();

            return reportDefinitionProvider.GetReportDefinitionStream(assemblyLocation, reportName);
        }
        
        private void setECNError(string message)
        {
            phError.Visible = true;
            lblErrorMessage.Text = message;
        }
    }
}