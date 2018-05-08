using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.Communicator.Report;
using Microsoft.Reporting.WebForms;
using BusinessEmailPreviewUsage = ECN_Framework_BusinessLayer.Communicator.Report.EmailPreviewUsage;

namespace ecn.automatedreporting.Reports
{
    public class EmailPreviewUsageReport : BaseReport
    {
        public override ReturnReport Execute()
        {

            var reportMessage = new ReturnReport();
            try
            {
                var strCustIdsBuilder = LoadReportParameters();

                var startDate = new DateTime();
                var endDate = new DateTime();
                SetStartDateAndEndDateNoDefault(ReportSchedule.RecurrenceType, ref startDate, ref endDate);

                ReportsHelper.WriteToLog("Getting Email Preview Usage Data.");
                var emailPreviewUsage = BusinessEmailPreviewUsage.GetUsageDetailsAutomated(strCustIdsBuilder.ToString(), startDate, endDate);

                ReportsHelper.WriteToLog("Got Email Preview Usage Data.");
                if (emailPreviewUsage?.Count > 0)
                {
                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        RenderPdfOrExcel(emailPreviewUsage, startDate, endDate);
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        ReportProcessor.WriteAndAttachXml(ReportSchedule, Message, EmailPreviewPrefixXml, startDate, endDate, typeof(List<EmailPreviewUsage>), emailPreviewUsage);
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        var csvContent = ReportViewerExport.GetCSV(emailPreviewUsage);
                        WriteAndAttachCsv(ReportSchedule, Message, startDate, endDate, csvContent, EmailPreviewPrefixCsv);
                    }

                    ReportLogger.WriteLogSucceedAttachedWithReportName(reportMessage, EmailPreviewReportNameOnSucceed);
                    return reportMessage;
                }

                ReportLogger.WriteLogNoDataAttached(reportMessage, BlastDelivery);
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithDefaultBodyMessage(reportMessage,
                        ex,
                        EmailPreviewReportNameOnException,
                        ReportSchedule.ReportScheduleID.ToString(),
                        EmailPreviewReportNameOnException,
                        EmailPreviewIdentifier
                        );
                return reportMessage;
            }
        }

        private void RenderPdfOrExcel(IEnumerable<EmailPreviewUsage> emailPreviewUsage, DateTime startDate, DateTime endDate)
        {
            var reportViewer = new ReportViewer();
            var stream =
                ReportsHelper.Assembly.GetManifestResourceStream(
                    "ECN_Framework_Common.Reports.rpt_EmailPreviewUsageReport.rdlc");
            reportViewer.LocalReport.LoadReportDefinition(stream);
            var rds = new ReportDataSource("DS_EmailPreviewUsage", emailPreviewUsage);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            ReportsHelper.WriteToLog("Adding parameters.");
            var parameters = new[]
            {
                new ReportParameter("StartDate", startDate.ToShortDateString()),
                new ReportParameter("EndDate", endDate.ToShortDateString()),
            };
            reportViewer.LocalReport.SetParameters(parameters);

            ReportsHelper.WriteToLog("Added parameters.");
            reportViewer.LocalReport.Refresh();

            ReportsHelper.WriteToLog("Generated report.");
            ReportsHelper.WriteToLog("Rendering report.");
            if (ReportSchedule.ExportFormat.ToUpper() == FileTypePdf)
            {
                ReportProcessor.RenderAndAttachPdf(Message, reportViewer, EmailPreviewPrefixPdf, startDate, endDate);
            }
            else if (ReportSchedule.ExportFormat.ToUpper() == FileTypeXls)
            {
                ReportProcessor.RenderAndAttachXls(Message, reportViewer, EmailPreviewPrefixXls, startDate, endDate);
            }
        }

        private StringBuilder LoadReportParameters()
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            var custIDs = xDoc.GetElementsByTagName("CustomerID");

            var strCustIdsBuilder = new StringBuilder();
            foreach (XmlNode node in custIDs)
            {
                if (strCustIdsBuilder.Length == 0)
                {
                    strCustIdsBuilder.Append(", ");
                }

                strCustIdsBuilder.Append(node.InnerText);
            }

            return strCustIdsBuilder;
        }

        public EmailPreviewUsageReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
