using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic;
using Microsoft.Reporting.WebForms;
using BusinessDataDumpReport = ECN_Framework_BusinessLayer.Activity.Report.DataDumpReport;
using BusinessBlastFieldsName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName;
using ECN_Framework_BusinessLayer.Activity.Report;

namespace ecn.automatedreporting.Reports
{
    public class DataDumpReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();
            try
            {
                ReportsHelper.WriteToLog("Getting xml info for data export.");
                XmlNode nodeCust;
                XmlNode ftpUrl;
                XmlNode ftpUsername;
                XmlNode ftpPassword;
                var nodeGroups = LoadReportParameters(out nodeCust, out ftpUrl, out ftpUsername, out ftpPassword);

                ReportsHelper.WriteToLog("Got xml info for data export.");
                var osFilePath = $"{ConfigurationManager.AppSettings["FilePath"]}\\customers\\{ReportSchedule.CustomerID.Value}\\downloads\\";
                var tfile = $"{ReportSchedule.CustomerID.Value}-{nodeGroups.InnerText}-GroupAttribute-{DateTime.Now:MMddyyyy}.{ReportSchedule.ExportFormat.ToLower()}";
                var outfileName = $"{osFilePath}{tfile}";

                if (!(Directory.Exists(osFilePath)))
                {
                    Directory.CreateDirectory(osFilePath);
                }

                if (File.Exists(outfileName))
                {
                    File.Delete(outfileName);
                }

                //getting start and end date
                var startDate = MasterStartDate;
                var endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultNowOneWeekAgoWithAddHours(ReportSchedule.RecurrenceType, ref startDate, ref endDate);

                var dt = BusinessDataDumpReport.GetList(Convert.ToInt32(nodeCust.InnerText), startDate, endDate, nodeGroups.InnerText);
                if (dt.Count > 0)
                {
                    CorrectFields(dt);
                    var currentUser = User.GetByUserID(ReportSchedule.CreatedUserID.Value, false);
                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        RenderPdfOrExcel(currentUser, dt, startDate, endDate, outfileName);
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        var dtNew = ReportViewerExport.ToDataTable(dt);
                        WriteTableAsXml(outfileName, dtNew, GroupAttributeReportTableName);
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        var csv = ReportViewerExport.GetCSV(dt);
                        File.WriteAllText(outfileName, csv);
                    }

                    if (ReportsHelper.MoveFileToFtp(ftpUrl.InnerText, tfile, ftpUsername.InnerText, ftpPassword.InnerText, outfileName))
                    {
                        ReportLogger.WriteLogSucceed(reportMessage, string.Format(GroupAttributeReportSucceedMessage, ftpUrl.InnerText));
                        return reportMessage;
                    }

                    ReportLogger.WriteLogError(reportMessage, GroupAttributeReportFailMessage);
                    return reportMessage;
                }

                ReportLogger.WriteLogNoData(reportMessage, string.Format(GroupAttributeReportNoDataMessage, ftpUrl.InnerText));
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithCustomBodyMessageWithoutLogging(reportMessage, ex, DataDumpReportIdentifier, GroupAttributeReportFailMessage);
                return reportMessage;
            }
        }

        private void CorrectFields(List<ECN_Framework_Entities.Activity.Report.DataDumpReport> dt)
        {
            foreach (var ddr in dt)
            {
                ddr.Delivery = ddr.usend - ddr.tbounce;
                ddr.EmailSubject = ReportSchedule.ExportFormat.ToLower().Equals("csv")
                    ? ECN_Framework_Common.Functions.EmojiFunctions.ReplaceEmojiWithQuestion(ddr.EmailSubject)
                    : ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(ddr.EmailSubject);

                ddr.OpensPercentOfDelivered = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.topen) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.SuccessPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.usend) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uOpensPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uopen) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.tClickPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.tClick) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uClickPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uClick) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.tClicksOpensPerc = ddr.topen > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.tClick) / Convert.ToDecimal(ddr.topen),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uClicksOpensPerc = ddr.uopen > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uClick) / Convert.ToDecimal(ddr.uopen),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.SuppressedPerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.Suppressed) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uAbuseRpt_UnsubPerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uAbuseRpt_Unsub) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uFeedBack_UnsubPerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uFeedBack_Unsub) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uHardBouncePerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uHardBounce) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uMastSup_UnsubPerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uMastSup_Unsub) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uOtherBouncePerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uOtherBounce) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uSoftBouncePerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uSoftBounce) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.uSubscribePerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.uSubscribe) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.treferPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.trefer) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.tresendPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.tresend) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.tbouncePerc = ddr.usend > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.tbounce) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.sendPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.usend) / Convert.ToDecimal(ddr.usend),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
                ddr.ClickThroughPerc = ddr.Delivery > 0
                    ? decimal.Round(Convert.ToDecimal(ddr.ClickThrough) / Convert.ToDecimal(ddr.Delivery),
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0;
            }
        }

        private void RenderPdfOrExcel(KMPlatform.Entity.User currentUser, List<ECN_Framework_Entities.Activity.Report.DataDumpReport> dt, DateTime startDate, DateTime endDate, string outfileName)
        {
            var blastFieldsName1 = BusinessBlastFieldsName.GetByBlastFieldID(1, currentUser);
            var blastFieldsName2 = BusinessBlastFieldsName.GetByBlastFieldID(2, currentUser);
            var blastFieldsName3 = BusinessBlastFieldsName.GetByBlastFieldID(3, currentUser);
            var blastFieldsName4 = BusinessBlastFieldsName.GetByBlastFieldID(4, currentUser);
            var blastFieldsName5 = BusinessBlastFieldsName.GetByBlastFieldID(5, currentUser);

            //run PDF or xls
            var reportViewer1 = new ReportViewer();
            var stream =
                ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_DataDumpReport.rdlc");
            reportViewer1.LocalReport.LoadReportDefinition(stream);

            var rds = new ReportDataSource("DataSet1", dt);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);


            var parameters = new[]
            {
                new ReportParameter("Field1", blastFieldsName1 != null ? blastFieldsName1.Name : "Field1"),
                new ReportParameter("Field2", blastFieldsName2 != null ? blastFieldsName2.Name : "Field2"),
                new ReportParameter("Field3", blastFieldsName3 != null ? blastFieldsName3.Name : "Field3"),
                new ReportParameter("Field4", blastFieldsName4 != null ? blastFieldsName4.Name : "Field4"),
                new ReportParameter("Field5", blastFieldsName5 != null ? blastFieldsName5.Name : "Field5"),
                new ReportParameter("StartDate", startDate.ToShortDateString()),
                new ReportParameter("EndDate", endDate.ToShortDateString()),
            };
            reportViewer1.LocalReport.SetParameters(parameters);

            reportViewer1.LocalReport.Refresh();
            if (ReportProcessor.ExportFormatPdf(ReportSchedule))
            {
                ReportProcessor.RenderAndWritePdf(reportViewer1, outfileName);
            }
            else if (ReportProcessor.ExportFormatXls(ReportSchedule))
            {
                ReportProcessor.RenderAndWriteXls(reportViewer1, outfileName);
            }
        }

        private XmlNode LoadReportParameters(
            out XmlNode nodeCust,
            out XmlNode ftpUrl,
            out XmlNode ftpUsername,
            out XmlNode ftpPassword)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            var nodeGroups = xDoc.DocumentElement.SelectSingleNode("GroupIDs");
            nodeCust = xDoc.DocumentElement.SelectSingleNode("CustomerID");
            ftpUrl = xDoc.DocumentElement.SelectSingleNode("FTPURL");
            ftpUsername = xDoc.DocumentElement.SelectSingleNode("FTPUsername");
            ftpPassword = xDoc.DocumentElement.SelectSingleNode("FTPPassword");
            return nodeGroups;
        }

        public DataDumpReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
