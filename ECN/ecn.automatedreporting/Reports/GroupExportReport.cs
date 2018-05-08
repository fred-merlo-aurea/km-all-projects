using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using BusinessEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup;

namespace ecn.automatedreporting.Reports
{
    public class GroupExportReport : BaseReport
    {
        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();
            var outfileName = string.Empty;

            try
            {
                ReportsHelper.WriteToLog("Getting xml info for data export.");
                int groupId;
                string exportSettingsAbbrev;
                string filter;
                string exportSubscribeTypeCode;
                string exportFormat;
                string exportSettings;
                XmlNode ftpurl;
                XmlNode ftpUsername;
                XmlNode ftpPassword;
                LoadReportParameters(
                    out groupId,
                    out ftpurl,
                    out ftpUsername,
                    out ftpPassword,
                    out exportSettings,
                    out exportFormat,
                    out exportSubscribeTypeCode,
                    out filter,
                    out exportSettingsAbbrev);

                if (groupId > 0)
                {
                    var emailProfilesDt = BusinessEmailGroup.GetGroupEmailProfilesWithUDF(groupId, ReportSchedule.CustomerID.Value, filter, exportSubscribeTypeCode, exportSettings);
                    if (emailProfilesDt.Rows.Count > 0)
                    {
                        string remoteFilePath;
                        BuildFile(out remoteFilePath, groupId, exportSubscribeTypeCode, exportSettingsAbbrev, exportFormat, emailProfilesDt, out outfileName);

                        if (ReportsHelper.MoveFileToFtp(ftpurl.InnerText, remoteFilePath, ftpUsername.InnerText, ftpPassword.InnerText, outfileName))
                        {
                            Body = $"{Body}</BR>Your scheduled export of Group Export has been moved to {ftpurl.InnerText}";
                            reportMessage.success = true;
                            if (File.Exists(outfileName))
                            {
                                File.Delete(outfileName);
                            }
                            return reportMessage;
                        }

                        Body = $"{Body}</BR>Your scheduled export of Group Export has failed";
                        reportMessage.success = false;
                        reportMessage.message = "Error moving export to FTP";
                        return reportMessage;

                    }

                    ReportsHelper.WriteToLog("No Data.");
                    Body = $"{Body}</BR>Your scheduled export of Group Export didn't return any data";
                    reportMessage.success = false;
                    reportMessage.message = "No Data";
                    if (!string.IsNullOrWhiteSpace(outfileName) && File.Exists(outfileName))
                    {
                        File.Delete(outfileName);
                    }
                    return reportMessage;
                }
                Body = $"{Body}</BR>Your scheduled export of Group Export has failed";
                reportMessage.success = false;
                reportMessage.message = "Error running report";
                if (!string.IsNullOrWhiteSpace(outfileName) && File.Exists(outfileName))
                {
                    File.Delete(outfileName);
                }
                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportsHelper.WriteToLog("Error running Group Export to FTP report");
                ReportsHelper.WriteToLog("ReportScheduleID: " + ReportSchedule.ReportScheduleID.ToString());
                ReportsHelper.WriteToLog(ex.Message);
                Body = $"{Body}</BR>Your scheduled export of Group Export has failed";
                ApplicationLog.LogCriticalError(ex, "ecn.automatedReporting.GetGroupExport", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                reportMessage.success = false;
                reportMessage.message = "Error running report";
                if (!string.IsNullOrWhiteSpace(outfileName) && File.Exists(outfileName))
                {
                    File.Delete(outfileName);
                }

                return reportMessage;
            }
        }

        private void BuildFile(
            out string tfile,
            int groupId,
            string exportSubscribeTypeCode,
            string exportSettingsAbbrev,
            string exportFormat,
            DataTable emailProfilesDt,
            out string outfileName)
        {
            var osFilePath = $"{ConfigurationManager.AppSettings["FilePath"].TrimEnd('\\')}\\customers\\{ReportSchedule.CustomerID.Value}\\downloads\\";
            tfile = $"{ReportSchedule.CustomerID.Value}-{groupId}-emails-{exportSubscribeTypeCode}-{exportSettingsAbbrev}-{DateTime.Now:MMddyyyy}{exportFormat}";
            outfileName = osFilePath + tfile;

            if (!Directory.Exists(osFilePath))
            {
                Directory.CreateDirectory(osFilePath);
            }

            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            var delimiter = exportFormat.Equals(".xls") || exportFormat.Equals(".txt") ? "\t" : ",";

            TextWriter txtfile = File.AppendText(outfileName);

            if (exportFormat.Equals(".xls") || exportFormat.Equals(".csv") || exportFormat.Equals(".txt"))
            {
                var sbNewLine = new StringBuilder();

                var columnHeadings = ReportsHelper.GetDataTableColumns(emailProfilesDt);
                var aListEnum = columnHeadings.GetEnumerator();

                while (aListEnum.MoveNext())
                {
                    sbNewLine.Append(exportFormat.Equals(".csv") ? "\"" : "");
                    sbNewLine.Append(aListEnum.Current);
                    sbNewLine.Append(exportFormat.Equals(".csv") ? "\"" : "");
                    sbNewLine.Append(delimiter);
                }

                txtfile.WriteLine(sbNewLine.ToString());

                foreach (DataRow dr in emailProfilesDt.Rows)
                {
                    sbNewLine = new StringBuilder();
                    aListEnum.Reset();
                    while (aListEnum.MoveNext())
                    {
                        var columnText = dr[aListEnum.Current.ToString()].ToString().Replace("\n", "");
                        columnText = columnText.Replace("\r", "");
                        columnText = columnText.Replace(@"""", "");
                        sbNewLine.Append(exportFormat.Equals(".csv") ? "\"" : "");
                        sbNewLine.Append(columnText);
                        sbNewLine.Append(exportFormat.Equals(".csv") ? "\"" : "");
                        sbNewLine.Append(delimiter);
                    }

                    txtfile.WriteLine(sbNewLine.ToString());
                }
            }
            else if (exportFormat.Equals(".xml"))
            {
                var emailProfilesDs = new DataSet();
                emailProfilesDs.Tables.Add(emailProfilesDt);
                emailProfilesDs.WriteXml(txtfile);
            }

            txtfile.Close();
        }

        private void LoadReportParameters(
            out int groupId,
            out XmlNode ftpurl,
            out XmlNode ftpUsername,
            out XmlNode ftpPassword,
            out string exportSettings,
            out string exportFormat,
            out string exportSubscribeTypeCode,
            out string filter,
            out string exportSettingsAbbrev)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            var groupNode = xDoc.GetElementsByTagName("GroupID")[0];
            int.TryParse(groupNode.InnerText, out groupId);

            if (groupId <= 0)
            {
                ftpurl = null;
                ftpUsername = null;
                ftpPassword = null;
                exportSettings = null;
                exportFormat = null;
                exportSubscribeTypeCode = null;
                filter = null;
                exportSettingsAbbrev = null;
                return;
            }

            var exportSettingsNode = xDoc.GetElementsByTagName("ExportSettings")[0];
            var exportFormatNode = xDoc.GetElementsByTagName("ExportFormat")[0];
            var exportSubscribeTypeCodeNode = xDoc.GetElementsByTagName("ExportSubscribeTypeCode")[0];
            ftpurl = xDoc.GetElementsByTagName("FTPURL")[0];
            ftpUsername = xDoc.GetElementsByTagName("FTPUsername")[0];
            ftpPassword = xDoc.GetElementsByTagName("FTPPassword")[0];
            var filterIdNode = xDoc.GetElementsByTagName("FilterID")[0];
            ReportsHelper.WriteToLog("Got xml info for data export.");

            exportSettings = exportSettingsNode.InnerText;
            exportFormat = exportFormatNode.InnerText;
            exportSubscribeTypeCode = exportSubscribeTypeCodeNode.InnerText;

            exportSubscribeTypeCode = exportSubscribeTypeCode.Equals("*")
                ? " 'S', 'U', 'D', 'P', 'B', 'M' "
                : $" '{exportSubscribeTypeCode}' ";

            var filterId = -1;
            if (filterIdNode != null)
            {
                filterId = Convert.ToInt32(filterIdNode.InnerText);
            }

            filter = string.Empty;
            if (filterId > 0)
            {
                var filterstr = ECN_Framework_BusinessLayer.Communicator.Filter
                    .GetByFilterID_NoAccessCheck(filterId)
                    .WhereClause;
                if (filterstr != string.Empty)
                {
                    filter = $" AND ( {filterstr})";
                }
            }

            exportSubscribeTypeCode = exportSubscribeTypeCode.Length > 5
                ? "ALL"
                : exportSubscribeTypeCode.Replace("'", "").Trim();

            if (exportSettings.Equals("profileplusstandalone", StringComparison.OrdinalIgnoreCase))
            {
                exportSettingsAbbrev = "Standalone";
            }
            else if (exportSettings.Equals("profileplusalludfs", StringComparison.OrdinalIgnoreCase))
            {
                exportSettingsAbbrev = "AllUDFs";
            }
            else
            {
                exportSettingsAbbrev = "Profile";
            }
        }

        public GroupExportReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
