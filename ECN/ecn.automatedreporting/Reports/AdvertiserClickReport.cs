using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;
using ActivityBusinessAdvertiserClickReport = ECN_Framework_BusinessLayer.Activity.Report.AdvertiserClickReport;
using ActivityEntityAdvertiserClickReport = ECN_Framework_Entities.Activity.Report.AdvertiserClickReport;

namespace ecn.automatedreporting.Reports
{
    public class AdvertiserClickReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();
            try
            {
                KMPlatform.BusinessLogic.User.GetByUserID(ReportSchedule.CreatedUserID.Value, false);

                var startDate = new DateTime();
                var endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultNowOneWeekAgoWithoutAddHours(ReportSchedule.RecurrenceType, ref startDate, ref endDate);

                XmlNode groupId;
                Group grp;
                LoadParameters(out grp, out groupId);

                ReportsHelper.WriteToLog("Getting Advertiser Click Report data.");
                var advertiserClickReports = ActivityBusinessAdvertiserClickReport.GetList(grp.GroupID, startDate, endDate);
                ReportsHelper.WriteToLog("Got Advertiser Click Report data.");

                if (advertiserClickReports?.Count > 0)
                {
                    foreach (var a in advertiserClickReports)
                    {
                        a.EmailSubject = ReportProcessor.ExportFormatCsv(ReportSchedule)
                            ? EmojiFunctions.ReplaceEmojiWithQuestion(a.EmailSubject)
                            : EmojiFunctions.GetSubjectUTF(a.EmailSubject);
                    }

                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        BuildPdfOrExcelResult(advertiserClickReports, grp, startDate, endDate);
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        ReportProcessor.WriteAndAttachXml(ReportSchedule, Message, AdvertiserClickPrefix, startDate, endDate, typeof(List<ECN_Framework_Entities.Activity.Report.AdvertiserClickReport>), advertiserClickReports, grp.GroupID.ToString());
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        BuildCsvResult(advertiserClickReports, startDate, endDate);
                    }

                    ReportLogger.WriteLogSucceedAttachedWithReportName(reportMessage, AdvertiserClickReportName);
                    return reportMessage;
                }

                ReportLogger.WriteLogAttachedNoDataWithoutDotAtheEnd(reportMessage, AdvertiserClickReportName);
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithCustomBodyMessage(
                    reportMessage,
                    ex,
                    AdvertiserClickReportName,
                    ReportSchedule.ReportScheduleID.ToString(),
                    AdvertiserClickReportIdentifier,
                    AdvertiserClickExceptionBodyMessage);
                return reportMessage;
            }
        }

        private void LoadParameters(out Group @group, out XmlNode groupId)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            groupId = xDoc.DocumentElement.GetElementsByTagName("GroupID")[0];
            @group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(
                Convert.ToInt32(groupId.InnerText));
        }

        private void BuildCsvResult(IEnumerable<ActivityEntityAdvertiserClickReport> advertiserclickreport, DateTime startDate, DateTime endDate)
        {
            var lacr = (from a in advertiserclickreport
                select new
                {
                    a.BlastID,
                    a.EmailSubject,
                    a.Date,
                    LinkAlias = a.Alias,
                    a.LinkURL,
                    a.LinkOwner,
                    a.UniqueCount,
                    a.TotalCount
                }).ToList();
            var csvContent = ReportViewerExport.GetCSV(lacr);
            WriteAndAttachCsv(ReportSchedule, Message, startDate, endDate, csvContent, AdvertiserClickPrefix);
        }

        private void BuildPdfOrExcelResult(
            IEnumerable<ActivityEntityAdvertiserClickReport> advertiserclickreport,
            Group @group,
            DateTime startDate,
            DateTime endDate)
        {
            var reportViewer = new ReportViewer();
            var stream = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_AdvertiserClickReport.rdlc");
            reportViewer.LocalReport.LoadReportDefinition(stream);

            ReportsHelper.WriteToLog("Attaching report");
            var rds = new ReportDataSource("DS_AdvertiserClickReport", advertiserclickreport);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);

            ReportsHelper.WriteToLog("Adding parameters.");
            var parameters = new[]
            {
                new ReportParameter("GroupName", @group.GroupName),
                new ReportParameter("StartDate", startDate.ToShortDateString()),
                new ReportParameter("EndDate", endDate.ToShortDateString()),
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
                    AdvertiserClickPrefix,
                    startDate,
                    endDate,
                    @group.GroupID.ToString());
            }
            else if (ReportProcessor.ExportFormatXls(ReportSchedule))
            {
                ReportProcessor.RenderAndAttachXls(Message,
                    reportViewer,
                    AdvertiserClickPrefix,
                    startDate,
                    endDate,
                    @group.GroupID.ToString());
            }
        }

        public AdvertiserClickReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
