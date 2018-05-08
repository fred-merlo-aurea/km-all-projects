using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.View;
using ECN_Framework_Entities.Communicator;

namespace ecn.automatedreporting.Reports
{
    public abstract class BaseReport: IDeliveryReport
    {
        protected const string FileTypePdf = "PDF";
        protected const string FileTypeXls = "XLS";
        protected const string RenderedLogMessage = "Rendered report.";
        protected const string AttachingLogMessage = "Attaching report.";
        protected const string GroupAttributeReportTableName = "GroupAttributeReport";
        protected const string GroupAttributeReportSucceedMessage = "</BR>Your scheduled export of Group Attribute Report has been moved to {0}";
        protected const string GroupAttributeReportFailMessage = "</BR>Your scheduled export of Group Attribute Report has failed";
        protected const string GroupAttributeReportNoDataMessage = "</BR>Your scheduled export of Group Attribute Report didn't return any data {0}";
        protected const string DataDumpReportIdentifier = "ecn.automatedReporting.GetDataDumpReport";
        protected const string BlastDelivery = "Blast Delivery";
        protected const string BlastDeliveryPrefix = "BlastDeliveryReport_";
        protected const string BlastDeliveryIdentifier = "ecn.automatedReporting.GetBlastDeliveryReport";
        protected const string BlastDeliveryExceptionBodyMessage = "</BR>Your scheduled report of Blast Delivery has either failed or didn't return any data.";
        protected const string EmailPreviewPrefixPdf = "EmailPreviewUsage";
        protected const string EmailPreviewPrefixXls = "EmailPreviewUsage_";
        protected const string EmailPreviewPrefixXml = "EmailPreviewUsageDetails_";
        protected const string EmailPreviewPrefixCsv = "EmailPreviewUsageDetails_";
        protected const string EmailPreviewIdentifier = "ecn.automatedReporting.GetEmailPreviewUsageDetails";
        protected const string EmailPreviewReportNameOnSucceed = "EmailPreview Usage";
        protected const string EmailPreviewReportNameOnException = "Email Preview Usage";
        protected const string EmailPerformanceReportName = "Email Performance By Domain";
        protected const string EmailPerformancePrefix = "EmailPerformanceByDomainReport_";
        protected const string EmailPerformanceReportIdentifier = "ecn.automatedReporting.GetEmailPerformanceByDomainReport";
        protected const string GroupStatisticsReportName = "Group Statistics";
        protected const string GroupStatisticsPrefix = "GroupStatisticsReport_";
        protected const string GroupStatisticsReportIdentifier = "ecn.automatedReporting.GetGroupStatisticsReport";
        protected const string GroupStatisticsExceptionBodyMessage = "</BR>Your scheduled report of Group Statistics has failed";
        protected const string AudienceEngagementReportName = "Audience Engagement";
        protected const string AudienceEngagementPrefix = "AudienceEngagementReport_";
        protected const string AudienceEngagementReportIdentifier = "ecn.automatedReporting.GetAudienceEngagementReport";
        protected const string AdvertiserClickReportName = "Advertiser Click";
        protected const string AdvertiserClickPrefix = "AdvertiserClickReport";
        protected const string AdvertiserClickReportIdentifier = "ecn.automatedReporting.GetAdvertiserClickReport";
        protected const string AdvertiserClickExceptionBodyMessage = "</BR>Your scheduled report of Advertiser Click has failed";

        private const string FileExtensionCsv = ".csv";
        private const string Monthly = "monthly";
        private const string Weekly = "weekly";
        private const string Daily = "daily";
        private const string SetExportedDataBeginLog = "Getting {0} data.";
        private const string SetExportedDataEndLog = "Got {0} data.";

        public string Body { get; set; }
        protected EmailDirect Message;
        protected ReportSchedule ReportSchedule;
        protected DateTime MasterStartDate;
        protected readonly ReportLogger ReportLogger;
        protected readonly ReportProcessor ReportProcessor;

        protected BaseReport(
            EmailDirect message,
            string body,
            ReportSchedule reportSchedule,
            DateTime masterStartDate)
        {
            Message = message;
            Body = body;
            ReportSchedule = reportSchedule;
            MasterStartDate = masterStartDate;
            ReportLogger = new ReportLogger(this);
            ReportProcessor = new ReportProcessor();
        }

        public abstract ReturnReport Execute();

        protected void SetExportedData(
            ReportSchedule reportViewer,
            BlastAbstract blast,
            string reportType,
            ref DataForExportContainer container,
            string reportType1,
            string filterType,
            string isp,
            DateTime startDate,
            DateTime endDate)
        {
            if (reportViewer == null)
            {
                throw new ArgumentNullException(nameof(reportViewer));
            }

            if (blast == null)
            {
                throw new ArgumentNullException(nameof(blast));
            }

            if (reportViewer.BlastID == null)
            {
                throw new ArgumentException("BlastID is null", nameof(reportViewer));
            }

            if (blast.CustomerID == null)
            {
                throw new ArgumentException("CustomerID is null", nameof(blast));
            }

            ReportsHelper.WriteToLog(string.Format(SetExportedDataBeginLog, reportType));
            container.DataToBeExported = BlastActivity.DownloadBlastReportDetails_NoAccessCheck(
                reportViewer.BlastID.Value,
                blast.CustomerID.Value,
                false, reportType1,
                filterType, 
                isp, 
                startDate.ToString(CultureInfo.CurrentCulture),
                endDate.ToString(CultureInfo.CurrentCulture));
            ReportsHelper.WriteToLog(string.Format(SetExportedDataEndLog, reportType));
        }

        protected void WriteTableAsXml(string outfileName, DataTable dtNew, string tableName)
        {
            using (var fs = new FileStream(outfileName, FileMode.Append))
            {
                try
                {
                    dtNew.TableName = tableName;
                    dtNew.WriteXml(fs, false);
                }
                catch (Exception)
                {
                    //POSSIBLE BUG: no logging there
                }
            }
        }

        protected void WriteAndAttachCsv(
            ReportSchedule reportSchedule,
            EmailDirect message,
            DateTime startDate,
            DateTime endDate,
            string csvContent,
            string fileNamePrefix,
            string fileNameId = "")
        {
            if (reportSchedule == null)
            {
                throw new ArgumentNullException(nameof(reportSchedule));
            }

            if (reportSchedule.CustomerID == null)
            {
                throw new ArgumentException("CustomerID is null", nameof(reportSchedule));
            }

            var filepath = ReportsHelper.GetFilePath(reportSchedule.CustomerID.Value);
            var fileName = ReportProcessor.BuildFilePath(filepath, fileNamePrefix, startDate, endDate, FileExtensionCsv, fileNameId);
            File.AppendAllText(fileName, csvContent);
            ReportsHelper.WriteToLog(RenderedLogMessage);
            ReportsHelper.WriteToLog(AttachingLogMessage);
            message.Attachments.Add(new Attachment(fileName));
        }

        protected void SetStartDateAndEndDateNoDefault(string recurrenceType, ref DateTime startDate, ref DateTime endDate,  DateTime? noRecurrenceDefaultStartDate = null)
        {
            if (recurrenceType.Equals(Monthly, StringComparison.OrdinalIgnoreCase))
            {
                startDate = new DateTime(MasterStartDate.Month == 1 ? MasterStartDate.Year - 1 : MasterStartDate.Year, MasterStartDate.Month == 1 ? 12 : MasterStartDate.Month - 1, 1, 0, 0, 0);
                endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month) - 1).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else if (recurrenceType.Equals(Weekly))
            {
                startDate = ReportsHelper.GetPreviousWeekStartDate(MasterStartDate);
                endDate = startDate.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else if (recurrenceType.Equals(Daily))
            {

                startDate = MasterStartDate.AddDays(-1);
                endDate = startDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                if (noRecurrenceDefaultStartDate.HasValue)
                {
                    startDate = noRecurrenceDefaultStartDate.Value;
                }
            }
        }

        protected void SetStartDateAndEndDateDefaultNowOneWeekAgoWithAddHours(string recurrenceType, ref DateTime startDate, ref DateTime endDate)
        {
            DateTime? noRecurrenceDefaultStartDate = DateTime.Now.AddDays(-7).AddHours(23).AddMinutes(59).AddSeconds(59);
            SetStartDateAndEndDateNoDefault(recurrenceType, ref startDate, ref endDate, noRecurrenceDefaultStartDate);
        }

        protected void SetStartDateAndEndDateDefaultNowOneWeekAgoWithoutAddHours(string recurrenceType, ref DateTime startDate, ref DateTime endDate)
        {
            DateTime? noRecurrenceDefaultStartDate = DateTime.Now.AddDays(-7);
            SetStartDateAndEndDateNoDefault(recurrenceType, ref startDate, ref endDate, noRecurrenceDefaultStartDate);
        }

        protected void SetStartDateAndEndDateDefaultMasterDateOneWeekAgoWithoutAddHours(string recurrenceType, ref DateTime startDate, ref DateTime endDate)
        {
            DateTime? noRecurrenceDefaultStartDate = MasterStartDate.AddDays(-7);
            SetStartDateAndEndDateNoDefault(recurrenceType, ref startDate, ref endDate,  noRecurrenceDefaultStartDate);
        }
    }
}
