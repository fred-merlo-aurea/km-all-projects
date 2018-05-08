using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using Microsoft.Reporting.WebForms;
using BlastDelivery = ECN_Framework_Entities.Activity.Report.BlastDelivery;
using BusinessBlastDelivery = ECN_Framework_BusinessLayer.Activity.Report.BlastDelivery;

namespace ecn.automatedreporting.Reports
{
    public class BlastDeliveryReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();

            try
            {
                var startDate = MasterStartDate;
                var endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultNowOneWeekAgoWithAddHours(ReportSchedule.RecurrenceType, ref startDate, ref endDate);

                StringBuilder strCustIdsBuilder;
                LoadReportParameters(out strCustIdsBuilder);

                ReportsHelper.WriteToLog("Getting Blast Delivery Report data.");
                var blastDelivery = BusinessBlastDelivery.Get(strCustIdsBuilder.ToString(), startDate, endDate, true);
                foreach (var blast in blastDelivery)
                {
                    blast.Spam = (blast.Abuse + blast.Feedback);
                    if (blast.Delivered != 0)
                    {
                        var spam = blast.Spam;
                        var delivered = blast.Delivered;
                        var value = spam / delivered;
                        blast.SpamPercent = value.ToString("#0.##%");
                    }

                    blast.EmailSubject = ReportProcessor.ExportFormatCsv(ReportSchedule)
                        ? EmojiFunctions.ReplaceEmojiWithQuestion(blast.EmailSubject)
                        : EmojiFunctions.GetSubjectUTF(blast.EmailSubject);
                }

                ReportsHelper.WriteToLog("Got Blast Delivery Report data.");
                if (blastDelivery.Count > 0)
                {
                    if (ReportProcessor.ExportFormatPdfOrExcel(ReportSchedule))
                    {
                        RenderPdfOrExcel(blastDelivery, startDate, endDate);
                    }
                    else if (ReportProcessor.ExportFormatXml(ReportSchedule))
                    {
                        var blastDeliveryList = BusinessBlastDelivery.GetReportdetails(blastDelivery);
                        ReportProcessor.WriteAndAttachXml(ReportSchedule, Message, BlastDeliveryPrefix, startDate, endDate, typeof(List<BlastDelivery>), blastDeliveryList);
                    }
                    else if (ReportProcessor.ExportFormatCsv(ReportSchedule))
                    {
                        var newBlastDelivery = BusinessBlastDelivery.GetReportdetails(blastDelivery);
                        var csvContent = ReportViewerExport.GetCSV(newBlastDelivery);
                        WriteAndAttachCsv(ReportSchedule, Message, startDate, endDate, csvContent, BlastDeliveryPrefix);
                    }

                    ReportLogger.WriteLogSucceedAttachedWithReportName(reportMessage, BlastDelivery);
                    return reportMessage;
                }

                ReportLogger.WriteLogAttachedNoDataWithoutDotAtheEnd(reportMessage, BlastDelivery);
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportLogger.LogExceptionWithCustomBodyMessage(reportMessage,
                      ex,
                      BlastDelivery,
                      ReportSchedule.ReportScheduleID.ToString(),
                      BlastDeliveryIdentifier,
                      BlastDeliveryExceptionBodyMessage
                      );
                return reportMessage;
            }
        }

        private void RenderPdfOrExcel(IEnumerable<BlastDelivery> blastDelivery, DateTime startDate, DateTime endDate)
        {
            var reportViewer = new ReportViewer();
            var stream =
                ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastDelivery.rdlc");
            reportViewer.LocalReport.LoadReportDefinition(stream);
            var rds = new ReportDataSource("DS_BlastDelivery", blastDelivery);
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            ReportsHelper.WriteToLog("Adding parameters.");
            var parameters = new[]
            {
                new ReportParameter("StartDate", startDate.ToShortDateString()),
                new ReportParameter("EndDate", endDate.ToShortDateString()),
                new ReportParameter("Type", ReportSchedule.ExportFormat.ToUpper())
            };
            reportViewer.LocalReport.SetParameters(parameters);
            ReportsHelper.WriteToLog("Added parameters.");
            reportViewer.LocalReport.Refresh();

            ReportsHelper.WriteToLog("Generated report.");
            ReportsHelper.WriteToLog("Rendering report.");

            if (ReportSchedule.ExportFormat.ToUpper() == FileTypePdf)
            {
                ReportProcessor.RenderAndAttachPdf(Message, reportViewer, BlastDeliveryPrefix, startDate, endDate);
            }
            else if (ReportSchedule.ExportFormat.ToUpper() == FileTypeXls)
            {
                ReportProcessor.RenderAndAttachXls(Message, reportViewer, BlastDeliveryPrefix, startDate, endDate);
            }
        }

        private void LoadReportParameters(out StringBuilder strCustIdsBuilder)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            var custIDs = xDoc.GetElementsByTagName("CustomerID");

            strCustIdsBuilder = new StringBuilder();
            foreach (XmlNode node in custIDs)
            {
                if (strCustIdsBuilder.Length > 0)
                {
                    strCustIdsBuilder.Append(", ");
                }

                strCustIdsBuilder.Append(node.InnerText);
            }
        }

        public BlastDeliveryReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
