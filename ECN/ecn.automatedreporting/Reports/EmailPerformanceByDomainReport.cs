using System;
using System.Collections.Generic;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_Entities.Activity.Report;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;
using ActivityEmailPerformanceByDomainReport = ECN_Framework_BusinessLayer.Activity.Report.EmailPerformanceByDomainReport;

namespace ecn.automatedreporting.Reports
{
    public class EmailPerformanceByDomainReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();
            try
            {
                var drillDownOther = LoadReportParameters();

                var startDate = new DateTime();
                var endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultMasterDateOneWeekAgoWithoutAddHours(ReportSchedule.RecurrenceType, ref startDate, ref endDate);


                ReportsHelper.WriteToLog("Getting Email Performance By Domain data.");
                var emailperformance = ActivityEmailPerformanceByDomainReport.Get(ReportSchedule.CustomerID.Value, startDate, endDate, drillDownOther);

                ReportsHelper.WriteToLog("Got Email Performance By Domain data.");
                if (emailperformance?.Count > 0)
                {
                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        RenderPdfOrExcel(emailperformance, startDate, endDate);
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        ReportProcessor.WriteAndAttachXml(
                            ReportSchedule,
                            Message,
                            EmailPerformancePrefix,
                            startDate,
                            endDate,
                            typeof(List<EmailPerformanceByDomain>),
                            ActivityEmailPerformanceByDomainReport.GetReportdetails(emailperformance));
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        var lDomainReportDetails = ActivityEmailPerformanceByDomainReport.GetReportdetails(emailperformance);
                        var content = ActivityEmailPerformanceByDomainReport.AddDelimiter(lDomainReportDetails);
                        WriteAndAttachCsv(ReportSchedule, Message, startDate, endDate, content, EmailPerformancePrefix);
                    }

                    ReportLogger.WriteLogSucceedAttachedWithReportName(reportMessage, EmailPerformanceReportName);
                    return reportMessage;
                }

                ReportLogger.WriteLogNoDataAttached(reportMessage, EmailPerformanceReportName);
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithDefaultBodyMessage(
                    reportMessage,
                    ex,
                    EmailPerformanceReportName,
                    ReportSchedule.ReportScheduleID.ToString(),
                    EmailPerformanceReportName,
                    EmailPerformanceReportIdentifier);
                return reportMessage;
            }
        }

        private void RenderPdfOrExcel(IEnumerable<EmailPerformanceByDomain> emailperformance, DateTime startDate, DateTime endDate)
        {
            var reportViewer = new ReportViewer();

            var stream = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_EmailPerformanceByDomain.rdlc");
            reportViewer.LocalReport.LoadReportDefinition(stream);

            ReportsHelper.WriteToLog("Attaching report");
            var rds = new ReportDataSource("DataSet1", emailperformance);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);

            ReportsHelper.WriteToLog("Adding parameters");
            ReportParameter[] parameters =
            {
                new ReportParameter("StartDate", startDate.ToShortDateString()),
                new ReportParameter("EndDate", endDate.ToShortDateString())
            };
            reportViewer.LocalReport.SetParameters(parameters);

            ReportsHelper.WriteToLog("Added parameters");
            reportViewer.LocalReport.Refresh();


            ReportsHelper.WriteToLog("Generated report.");
            ReportsHelper.WriteToLog("Rendering report.");

            if (ReportSchedule.ExportFormat.ToUpper() == FileTypePdf)
            {
                ReportProcessor.RenderAndAttachPdf(Message, reportViewer, EmailPerformancePrefix, startDate, endDate);
            }
            else if (ReportSchedule.ExportFormat.ToUpper() == FileTypeXls)
            {
                ReportProcessor.RenderAndAttachXls(Message, reportViewer, EmailPerformancePrefix, startDate, endDate);
            }
        }

        private bool LoadReportParameters()
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            var drillDownOther = false;
            var drillDown = xDoc.DocumentElement.GetElementsByTagName("DrillDownOther");
            if (drillDown.Count > 0)
            {
                bool.TryParse(drillDown[0].InnerText, out drillDownOther);
            }

            return drillDownOther;
        }

        public EmailPerformanceByDomainReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
