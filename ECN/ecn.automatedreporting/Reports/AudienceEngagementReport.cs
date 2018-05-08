using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using System.Xml.Serialization;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;
using BusinessAudienceEngagementReport = ECN_Framework_BusinessLayer.Activity.Report.AudienceEngagementReport;
using EntityAudienceEngagementReport = ECN_Framework_Entities.Activity.Report.AudienceEngagementReport;

namespace ecn.automatedreporting.Reports
{
    public class AudienceEngagementReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();
            try
            {
                var rv = new ReportViewer();

                var startDate = new DateTime();
                var endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultNowOneWeekAgoWithoutAddHours(ReportSchedule.RecurrenceType, ref startDate, ref endDate);

                XmlNode groupId;
                XmlNode clickPercent;
                Group grp;
                LoadReportParameters(out grp, out groupId, out clickPercent);

                ReportsHelper.WriteToLog("Generating Audience Engagement report data.");
                var audienceEngagementReport = BusinessAudienceEngagementReport.GetListByRange(
                    Convert.ToInt32(groupId.InnerText),
                    Convert.ToInt32(clickPercent.InnerText),
                    "N",
                    "",
                    startDate,
                    endDate);
                ReportsHelper.WriteToLog("Generated Audience Engagement report data.");
                if (audienceEngagementReport?.Count > 0)
                {
                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        RenderPdfOrExcel(rv, audienceEngagementReport, grp, clickPercent, startDate, endDate);
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        RenderXml(startDate, endDate, grp, audienceEngagementReport);
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        RenderCsv(audienceEngagementReport, startDate, endDate, grp);
                    }

                    ReportLogger.WriteLogSucceedAttachedWithReportName(reportMessage, AudienceEngagementReportName);
                    return reportMessage;
                }

                ReportLogger.WriteLogNoDataAttached(reportMessage, AudienceEngagementReportName);
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithDefaultBodyMessage(
                    reportMessage,
                    ex,
                    AudienceEngagementReportName,
                    ReportSchedule.ReportScheduleID.ToString(),
                    AudienceEngagementReportName,
                    AudienceEngagementReportIdentifier);
                return reportMessage;
            }
        }

        private void RenderCsv(IEnumerable<EntityAudienceEngagementReport> audienceEngagementReport, DateTime startDate, DateTime endDate, Group @group)
        {
            if (ReportSchedule.CustomerID == null)
            {
                throw new InvalidOperationException();
            }

            var laer = (from a in audienceEngagementReport
                select new {a.SortOrder, a.SubscriberType, a.Counts, a.Percentage, a.Description}).ToList();

            var filepath = ReportsHelper.GetFilePath(ReportSchedule.CustomerID.Value);
            var filename =
                $"{filepath}{AudienceEngagementPrefix}{startDate.ToShortDateString().Replace("/", "_")}_{endDate.ToShortDateString().Replace("/", "_")}_{@group.GroupID}.csv";
            File.AppendAllText(filename, ReportViewerExport.GetCSV(laer));
            ReportsHelper.WriteToLog(RenderedLogMessage);
            ReportsHelper.WriteToLog(AttachingLogMessage);
            Message.Attachments.Add(new Attachment(filename));
        }

        private void RenderXml(DateTime startDate, DateTime endDate, Group @group, IReadOnlyCollection<EntityAudienceEngagementReport> audienceEngagementReport)
        {
            if (ReportSchedule.CustomerID == null)
            {
                throw new InvalidOperationException();
            }

            var filepath = ReportsHelper.GetFilePath(ReportSchedule.CustomerID.Value);
            var file = new FileInfo(
                $"{filepath}{AudienceEngagementPrefix}{startDate.ToShortDateString().Replace("/", "_")}_{endDate.ToShortDateString().Replace("/", "_")}{@group.GroupID}.xml");
            var sw = file.AppendText();
            var writer = new XmlSerializer(typeof(List<EntityAudienceEngagementReport>));
            writer.Serialize(sw, audienceEngagementReport);
            ReportsHelper.WriteToLog(RenderedLogMessage);
            sw.Close();
            ReportsHelper.WriteToLog(AttachingLogMessage);
            Message.Attachments.Add(new Attachment(file.FullName));
        }

        private void RenderPdfOrExcel(
            ReportViewer reportViewer,
            IEnumerable<EntityAudienceEngagementReport> audienceEngagementReport,
            Group grp,
            XmlNode clickPercent,
            DateTime startDate,
            DateTime endDate)
        {
            var stream =
                ReportsHelper.Assembly.GetManifestResourceStream(
                    "ECN_Framework_Common.Reports.rpt_AudienceEngagementReport.rdlc");
            reportViewer.LocalReport.LoadReportDefinition(stream);
            ReportsHelper.WriteToLog("Attaching report");
            var rds = new ReportDataSource("DS_AudienceEngagementReport", audienceEngagementReport);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.EnableHyperlinks = true;
            ReportsHelper.WriteToLog("Adding parameters.");
            var parameters = new[]
            {
                new ReportParameter("GroupName", grp.GroupName),
                new ReportParameter("Format", ReportSchedule.ExportFormat.ToUpper()),
                new ReportParameter("ClickPercentage", clickPercent.InnerText),
                new ReportParameter("GroupID", grp.GroupID.ToString()),
                new ReportParameter("Days", "0"),
                new ReportParameter("URL", ""),
                new ReportParameter("ProfileFilter", ""),
                new ReportParameter("EnableHyperLinks", reportViewer.LocalReport.EnableHyperlinks.ToString())
            };

            reportViewer.LocalReport.SetParameters(parameters);
            ReportsHelper.WriteToLog("Added parameters.");
            reportViewer.LocalReport.Refresh();
            ReportsHelper.WriteToLog("Generated report.");
            ReportsHelper.WriteToLog("Rendering report.");

            if (ReportProcessor.ExportFormatPdf(ReportSchedule))
            {
                ReportProcessor.RenderAndAttachPdf(Message,
                    reportViewer,
                    AudienceEngagementPrefix,
                    startDate,
                    endDate,
                    grp.GroupID.ToString());
            }
            else if (ReportProcessor.ExportFormatXls(ReportSchedule))
            {
                ReportProcessor.RenderAndAttachXls(Message,
                    reportViewer,
                    AudienceEngagementPrefix,
                    startDate,
                    endDate,
                    grp.GroupID.ToString());
            }
        }

        private void LoadReportParameters(out Group grp, out XmlNode groupId, out XmlNode clickPercent)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);
            groupId = xDoc.DocumentElement.GetElementsByTagName("GroupID")[0];
            clickPercent = xDoc.DocumentElement.GetElementsByTagName("ClickPercentage")[0];
            grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(
                Convert.ToInt32(groupId.InnerText));
        }

        public AudienceEngagementReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
