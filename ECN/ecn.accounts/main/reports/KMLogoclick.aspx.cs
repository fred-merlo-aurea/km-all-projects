using System;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;

namespace ecn.accounts.main.reports
{
    public partial class KMLogoclick : ReportPageBase
    {
        private IKMLogoClickReportProxy _logoClickReportProxy;

        public KMLogoclick(IKMLogoClickReportProxy logoClickReportProxy, IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _logoClickReportProxy = logoClickReportProxy;
        }

        public KMLogoclick()
        {
            _logoClickReportProxy = new KMLogoClickReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;
        }

        protected void Page_Unload(object sender, System.EventArgs e)
        {
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            var dateFrom = Convert.ToDateTime(txtstartDate.Text);
            var dateTo = Convert.ToDateTime(txtendDate.Text);
            var logoClickReportList = _logoClickReportProxy.Get(dateFrom, dateTo);
            var dataSource = new ReportDataSource("DS_KMLogoClickReport", logoClickReportList);

            var parameters = new []
            {
                new ReportParameter("StartDate", txtstartDate.Text),
                new ReportParameter("EndDate", txtendDate.Text)
            };
            
            var filePath = Server.MapPath("~/main/reports/rpt_KMLogoClick.rdlc");
            var outputType = drpExport.SelectedItem.Text.ToUpper();
            var outputFileName = string.Format("KMLogoClickReport.{0}", drpExport.SelectedItem.Text);

            ReportViewer1.Visible = false;
            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
        }
    }
}
