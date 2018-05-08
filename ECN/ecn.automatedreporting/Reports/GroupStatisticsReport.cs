using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;
using ApplicationLog = KM.Common.Entity.ApplicationLog;
using ActivityGroupStatisticsReport = ECN_Framework_BusinessLayer.Activity.Report.GroupStatisticsReport;

namespace ecn.automatedreporting.Reports
{
    public class GroupStatisticsReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();
            try
            {                
                XmlNode groupId;
                XmlNode showBrowserDetails;
                Group grp;
                LoadReportParameters(out groupId, out showBrowserDetails, out grp);

                var startDate = new DateTime();
                var endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultNowOneWeekAgoWithoutAddHours(ReportSchedule.RecurrenceType, ref startDate, ref endDate);

                ReportsHelper.WriteToLog("Generating Group Statistics data.");
                var groupStatisticsReport = ActivityGroupStatisticsReport.Get(Convert.ToInt32(groupId.InnerText), startDate, endDate);

                ReportsHelper.WriteToLog("Generated Group Statistics data.");

                if (groupStatisticsReport != null && groupStatisticsReport.Count > 0)
                {
                    foreach (var g in groupStatisticsReport)
                    {
                        if (ReportSchedule.ExportFormat.Equals("CSV", StringComparison.OrdinalIgnoreCase))
                        {
                            g.EmailSubject = EmojiFunctions.ReplaceEmojiWithQuestion(g.EmailSubject);
                        }
                        else
                        {
                            g.EmailSubject = EmojiFunctions.GetSubjectUTF(g.EmailSubject);
                        }
                    }
                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        var rv = new ReportViewer();
                        var stream = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_GroupStatisticsReport.rdlc");
                        rv.LocalReport.LoadReportDefinition(stream);
                        ReportsHelper.WriteToLog("Attaching report");
                        rv.LocalReport.LoadSubreportDefinition(
                            "rpt_Platform_SubReport",
                            ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_Platform_SubReport.rdlc"));
                        var rds = new ReportDataSource("DS_GroupStatisticsReport", groupStatisticsReport);
                        rv.LocalReport.DataSources.Clear();
                        rv.LocalReport.DataSources.Add(rds);
                        ReportsHelper.WriteToLog("Adding parameters.");
                        var parameters = new ReportParameter[4];
                        parameters[0] = new ReportParameter("GroupName", grp.GroupName);
                        parameters[1] = new ReportParameter("StartDate", startDate.ToShortDateString());
                        parameters[2] = new ReportParameter("EndDate", endDate.ToShortDateString());
                        parameters[3] = new ReportParameter("Details", showBrowserDetails.InnerText.ToLower().Equals("yes") ? "true" : "false");
                        ReportsHelper.WriteToLog("Added parameters.");
                        if (showBrowserDetails.InnerText.ToLower().Equals("yes"))
                        {
                            rv.LocalReport.SubreportProcessing += Platform_SubReportProcessing;
                        }
                        rv.LocalReport.SetParameters(parameters);
                        rv.LocalReport.Refresh();

                        ReportsHelper.WriteToLog("Generated report.");
                        ReportsHelper.WriteToLog("Rendering report.");

                        if (ReportProcessor.ExportFormatPdf(ReportSchedule))
                        {
                            ReportProcessor.RenderAndAttachPdf(Message, rv, GroupStatisticsPrefix, startDate, endDate);
                        }
                        else if (ReportProcessor.ExportFormatXls(ReportSchedule))
                        {
                            ReportProcessor.RenderAndAttachXls(Message, rv, GroupStatisticsPrefix, startDate, endDate);
                        }
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        ReportProcessor.WriteAndAttachXml(
                            ReportSchedule,
                            Message,
                            GroupStatisticsPrefix,
                            startDate,
                            endDate,
                            typeof(List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport>),
                            ActivityGroupStatisticsReport.GetReportDetails(
                                groupStatisticsReport,
                                showBrowserDetails.InnerText.Equals("yes", StringComparison.OrdinalIgnoreCase)),
                            grp.GroupID.ToString());
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        var newgroupStatistics = ActivityGroupStatisticsReport.GetReportDetails(
                            groupStatisticsReport,
                            showBrowserDetails.InnerText.Equals("yes", StringComparison.OrdinalIgnoreCase));
                        var sb = ActivityGroupStatisticsReport.AddDelimiter(newgroupStatistics).ToString();
                        WriteAndAttachCsv(ReportSchedule, Message, startDate, endDate, sb, GroupStatisticsPrefix, grp.GroupID.ToString());
                    }
                    ReportLogger.WriteLogSucceedAttachedWithReportName(reportMessage, GroupStatisticsReportName);
                    return reportMessage;
                }

                ReportLogger.WriteLogNoDataAttached(reportMessage, GroupStatisticsReportName);
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithCustomBodyMessage(
                    reportMessage,
                    ex,
                    GroupStatisticsReportName,
                    ReportSchedule.ReportScheduleID.ToString(),
                    GroupStatisticsReportIdentifier,
                    GroupStatisticsExceptionBodyMessage);
                return reportMessage;
            }
        }

        private void LoadReportParameters(out XmlNode groupId, out XmlNode showBrowserDetails, out Group grp)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);
            groupId = xDoc.DocumentElement?.GetElementsByTagName("GroupID")[0];
            showBrowserDetails = xDoc.DocumentElement?.GetElementsByTagName("ShowBroswerDetails")[0];
            grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(
                Convert.ToInt32(groupId?.InnerText));
        }

        private static void Platform_SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.Parameters["BlastID"] == null)
            {
                return;
            }

            try
            {
                ReportsHelper.WriteToLog("Starting subreport processing.");
                e.DataSources.Clear();
                int blastId;
                int.TryParse(e.Parameters["BlastID"].Values[0], out blastId);
                var openslist = ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByBlastID(blastId).ToList();
                var plist = ECN_Framework_BusinessLayer.Activity.Platforms.Get();
                var eclist = ECN_Framework_BusinessLayer.Activity.EmailClients.Get();
                var platforms = openslist.Select(x => x.PlatformID).Distinct().ToList();
                var listPlatform = new List<PlatformData>();
                foreach (var i in platforms)
                {
                    if (i == 5)
                    {
                        continue;
                    }

                    var listOrder = new List<PlatformData>();
                    var tempList = openslist.Where(x => x.PlatformID == i).ToList();
                    var listEmailClients = tempList.Select(x => x.EmailClientID).Distinct().ToList();
                    foreach (var j in listEmailClients)
                    {
                        if (j == 15)
                        {
                            continue;
                        }

                        var pd = new PlatformData
                        {
                            PlatformName = plist.First(x => x.PlatformID == i).PlatformName,
                            EmailClientName = eclist.First(x => x.EmailClientID == j).EmailClientName,
                            Column1 = tempList.Count(x => x.EmailClientID == j)
                        };
                        pd.Usage = Math.Round(((float)pd.Column1 * 100 / openslist.Count), 2) + "%";

                        listOrder.Add(pd);
                    }

                    listPlatform.AddRange(listOrder.OrderByDescending(x => x.Column1).ToList());
                }


                var rds = new ReportDataSource("DataSet1", listPlatform);

                e.DataSources.Add(rds);
                ReportsHelper.WriteToLog("Finishing subreport processing.");

            }
            catch (Exception ex)
            {
                ReportsHelper.WriteToLog("Error running Platform sub report report");
                ReportsHelper.WriteToLog("ReportScheduleID: ");
                ReportsHelper.WriteToLog(ex.Message);
                ApplicationLog.LogNonCriticalError(ex, "ecn.automatedReporting.Platform_SubReportProcessing", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }

        }

        public GroupStatisticsReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
