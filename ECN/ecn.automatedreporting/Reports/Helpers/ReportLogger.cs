using System;
using System.Configuration;
using KM.Common.Entity;

namespace ecn.automatedreporting.Reports.Helpers
{
    public class ReportLogger
    {
        private const string AttachedLogMessage = "Attached report.";
        private const string ErrorLogMessage = "Error running report";
        private const string ErrorLogMessageFormatted = "Error running {0} report";
        private const string ScheduleIdLogMessageFormattedText = "ReportScheduleID: {0}";
        private const string LogDefaultBodyMessageFormattedText = "</BR>Your scheduled report of {0} has failed.";
        private const string LogBodyMessageNoDataAttachedFormattedText = "</BR>Your scheduled report of {0} didn't return any data";
        private const string LogBodyMessageAttachedFormattedText = "</BR>Your scheduled report of {0} has been attached";
        private const string NoDataLogText = "No Data.";
        private const string NoDataReportMessageText = "No Data";
        private const string Dot = ".";
        private const string ErrorMovingToFtp = "Error moving export to FTP";

        private readonly IDeliveryReport _report;

        public ReportLogger(IDeliveryReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            _report = report;
        }

        public void LogExceptionWithDefaultBodyMessage(ReturnReport reportMessage, Exception ex, string reportName, string scheduleId, string bodyReportName, string reportIdentifier)
        {
            var bodyMessage = string.Format(LogDefaultBodyMessageFormattedText, bodyReportName);
            LogExceptionWithCustomBodyMessage(reportMessage, ex, reportName, scheduleId, reportIdentifier, bodyMessage);
        }

        public void LogExceptionWithCustomBodyMessage(ReturnReport reportMessage, Exception ex, string reportName, string scheduleId, string reportIdentifier, string bodyMessage)
        {
            ReportsHelper.WriteToLog(string.IsNullOrWhiteSpace(reportName)
                ? ErrorLogMessage
                : string.Format(ErrorLogMessageFormatted, reportName));
            ReportsHelper.WriteToLog(string.Format(ScheduleIdLogMessageFormattedText, scheduleId));
            ReportsHelper.WriteToLog(ex.Message);
            LogExceptionWithCustomBodyMessageWithoutLogging(reportMessage, ex, reportIdentifier, bodyMessage);
        }

        public void LogExceptionWithCustomBodyMessageWithoutLogging(ReturnReport reportMessage, Exception ex, string reportIdentifier, string bodyMessage)
        {
            ProcessLog(reportMessage, bodyMessage, false, ErrorLogMessage);
            ApplicationLog.LogCriticalError(ex, reportIdentifier, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
        }

        public void WriteLogAttachedNoDataWithoutDotAtheEnd(ReturnReport reportMessage, string reportName)
        {
            WriteLogNoDataAttached(reportMessage, reportName, false);
        }

        public void WriteLogsSuccess(ReturnReport reportMessage, string bodyMessage, string logMessage)
        {
            WriteLog(reportMessage, bodyMessage, true, logMessage, null);
        }

        public void WriteLogNoData(ReturnReport reportMessage, string bodyMessage)
        {
            WriteLog(reportMessage, bodyMessage, false, NoDataLogText, NoDataReportMessageText);
        }

        public void WriteLog(ReturnReport reportMessage, string bodyMessage, bool result, string logMessage, string reportMessageText)
        {
            ReportsHelper.WriteToLog(logMessage);
            ProcessLog(reportMessage, bodyMessage, result, reportMessageText);
        }

        public void WriteLogNoDataAttached(ReturnReport reportMessage, string reportName, bool dotAtTheEnd = true)
        {
            ReportsHelper.WriteToLog(NoDataLogText);
            var bodyMesssage = string.Format(LogBodyMessageNoDataAttachedFormattedText, reportName);
            if (dotAtTheEnd)
            {
                bodyMesssage += Dot;
            }
            ProcessLog(reportMessage, bodyMesssage, false, NoDataReportMessageText);
        }

        public void WriteLogSucceedAttachedWithReportName(ReturnReport reportMessage, string reportName)
        {
            var bodyMessage = string.Format(LogBodyMessageAttachedFormattedText, reportName);
            WriteLogSucceedAttached(reportMessage, bodyMessage);
        }

        public void WriteLogSucceedAttached(ReturnReport reportMessage, string bodyMessage)
        {
            ReportsHelper.WriteToLog(AttachedLogMessage);
            WriteLogSucceed(reportMessage, bodyMessage);
        }

        public void WriteLogSucceed(ReturnReport reportMessage, string bodyMessage)
        {
            ProcessLog(reportMessage, bodyMessage, true, null);
        }

        public void WriteLogError(ReturnReport reportMessage, string bodyMessage)
        {
            ProcessLog(reportMessage, bodyMessage, false, ErrorMovingToFtp);
        }

        private void ProcessLog(ReturnReport reportMessage, string bodyMessage, bool? result, string reportMessageText)
        {
            if (bodyMessage != null)
            {
                _report.Body += bodyMessage;
            }
            if (result.HasValue)
            {
                reportMessage.success = result.Value;
            }
            if (reportMessage != null)
            {
                reportMessage.message = reportMessageText;
            }
        }
    }
}
