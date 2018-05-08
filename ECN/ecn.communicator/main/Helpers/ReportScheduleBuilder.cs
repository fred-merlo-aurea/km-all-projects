using System;
using System.Collections.Generic;
using System.Text;
using ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.Helpers
{
    public class ReportScheduleBuilder
    {
        private const string USADateFormat = "MM/dd/yyyy";
        private readonly ReportSchedule _reportSchedule;
        private StringBuilder _reportParameters;

        public ReportScheduleBuilder(ReportSchedule reportSchedule)
        {
            _reportSchedule = reportSchedule;
            _reportParameters = null;
        }

        public ReportScheduleBuilder SetReportId(int reportId)
        {
            _reportSchedule.ReportID = reportId;
            return this;
        }

        public ReportScheduleBuilder SetCommonHeader(
            int userId,
            int customerId,
            string reportTime,
            DateTime reportDate,
            string fromEmail,
            string toEmail,
            string fromName,
            string emailSubject)
        {
            _reportSchedule.CreatedUserID = userId;
            _reportSchedule.CustomerID = customerId;
            _reportSchedule.ScheduleType = "One-Time";
            _reportSchedule.StartTime = reportTime;
            _reportSchedule.StartDate = reportDate.ToString(USADateFormat);
            _reportSchedule.EndDate = reportDate.ToString(USADateFormat);
            _reportSchedule.FromEmail = fromEmail;
            _reportSchedule.ToEmail = toEmail;
            _reportSchedule.FromName = fromName;
            _reportSchedule.EmailSubject = emailSubject;
            return this;
        }

        public ReportScheduleBuilder AddExportsSection(List<string> ftpExports)
        {
            if (_reportParameters == null)
            {
                AddParametersHeader();
            }

            _reportParameters.Append("<Exports>");
            foreach (var ftpExport in ftpExports)
            {
                _reportParameters.Append($"<{ftpExport}>true</{ftpExport}>");
            }

            _reportParameters.Append("</Exports>");
            return this;
        }

        public ReportScheduleBuilder AddFtpCredentials(string ftpUrl, string ftpUsername, string ftpPassword, string ftpExportFormat)
        {
            if (_reportParameters == null)
            {
                AddParametersHeader();
            }

            _reportParameters.Append($"<FtpUrl>{ftpUrl}</FtpUrl>");
            _reportParameters.Append($"<FtpUsername>{ftpUsername}</FtpUsername>");
            _reportParameters.Append($"<FtpPassword>{ftpPassword}</FtpPassword>");
            _reportParameters.Append($"<ExportFormat>{ftpExportFormat}</ExportFormat>");
            return this;
        }

        public ReportScheduleBuilder AddCcList(List<string> ccList)
        {
            if (_reportParameters == null)
            {
                AddParametersHeader();
            }

            _reportParameters.Append("<ccEmails>");
            foreach (var ccToAdd in ccList)
            {
                _reportParameters.Append($"<ccEmail>{ccToAdd}</ccEmail>");
            }
            _reportParameters.Append("</ccEmails>");

            return this;
        }

        public ReportScheduleBuilder AddExportSettings()
        {
            if (_reportParameters == null)
            {
                AddParametersHeader();
            }

            _reportParameters.Append("<ExportSettings>ProfilePlusAllUdfs</ExportSettings>");
            _reportParameters.Append("<ExportSubscribeTypeCode>*</ExportSubscribeTypeCode>");
            return this;
        }

        private void AddParametersHeader()
        {
            _reportParameters = new StringBuilder();
            _reportParameters.Append("<XML>");
        }

        public void Build()
        {
            if (_reportParameters == null)
            {
                return;
            }

            _reportParameters.Append("</XML>");
            _reportSchedule.ReportParameters = _reportParameters.ToString();
        }
    }
}