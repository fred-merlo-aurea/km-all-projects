using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_Entities.Communicator;

namespace ecn.automatedreporting.Reports
{
    public class FtpExportsReport : BaseReport
    {
        private const string ExtensionXls = ".xls";
        private const string ExtensionTxt = ".txt";
        private const string ExtensionCsv = ".csv";
        private const string SendsToken = "sends";
        private const string OpensToken = "opens";
        private const string UnopenedToken = "unopened";
        private const string ClicksToken = "clicks";
        private const string NoClicksToken = "no-clicks";
        private const string BouncesToken = "bounces";
        private const string UnsubscribesToken = "unsubscribes";
        private const string ResendsToken = "resends";
        private const string SuppressedToken = "suppressed";
        private const string ForwardsToken = "forwards";

        public override ReturnReport Execute()
        {
            var reportMessage = new ReturnReport();

            try
            {
                string ftpUrl;
                string ftpUserName;
                string ftpPassword;
                List<DataForExportContainer> dataForExportList;
                string exportFormat;
                string exportSubscribeTypeCode;
                string exportSettingsAbbrev;
                LoadReportParameters(out ftpUrl, out ftpUserName, out ftpPassword, out dataForExportList, out exportFormat, out exportSubscribeTypeCode, out exportSettingsAbbrev);

                foreach (var report in dataForExportList)
                {
                    if (report.DataToBeExported.Rows.Count > 0)
                    {
                        string remoteFileName;
                        string outfilePath;
                        BuildDataExportFile(out remoteFileName, report, exportSubscribeTypeCode, exportSettingsAbbrev, exportFormat, out outfilePath);
                        if (ReportsHelper.MoveFileToFtp(ftpUrl, remoteFileName, ftpUserName, ftpPassword, outfilePath))
                        {
                            ReportsHelper.WriteToLog("File moved to FTP successfully.");
                            Body = $"{Body}<br \\>Your scheduled export of {report.NameOfReport} has been moved to {ftpUrl}<br \\> ";
                            reportMessage.success = true;
                        }
                        else
                        {
                            ReportsHelper.WriteToLog("File not moved to FTP successfully.");
                            Body = $"{Body}<br \\> There was a problem moving your scheduled export of {report.NameOfReport} emails to {ftpUrl}. Please contact KM.<br \\> ";
                            reportMessage.success = false;
                            reportMessage.message = "Error moving export to FTP";
                        }
                    }
                    else
                    {
                        ReportsHelper.WriteToLog("No Data.");
                        Body = $"{Body}<br \\> Your scheduled export of {report.NameOfReport} emails had no records.<br \\> ";
                        reportMessage.success = false;
                        reportMessage.message = "No Data";
                    }
                }

                return reportMessage;
            }
            catch (Exception ex)
            {
                ReportsHelper.WriteToLog("Error running report");
                ReportsHelper.WriteToLog($"ReportScheduleID: {ReportSchedule.ReportScheduleID}");
                ReportsHelper.WriteToLog(ex.Message);
                Body += "<br \\> Your scheduled export of blast details failed.<br \\> ";
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ecn.automatedReporting.FTPExports", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                reportMessage.success = false;
                reportMessage.message = "Error running report";
                return reportMessage;
            }
        }

        private void BuildDataExportFile(
            out string remoteFileName,
            DataForExportContainer report,
            string exportSubscribeTypeCode,
            string exportSettingsAbbrev,
            string exportFormat,
            out string outfilePath)
        {
            ReportsHelper.WriteToLog("Building file for data export.");
            var emailProfilesDt = report.DataToBeExported;
            var osFilePath = $"{ConfigurationManager.AppSettings["FilePath"]}\\customers\\{ReportSchedule.CustomerID.Value}\\downloads\\";
            remoteFileName = $"{ReportSchedule.CustomerID.Value}-{report.NameOfReport}-emails-{exportSubscribeTypeCode}-{exportSettingsAbbrev}-{ReportSchedule.BlastID}-{DateTime.Now:MMddyyyy}{exportFormat}";
            outfilePath = osFilePath + remoteFileName;

            if (!Directory.Exists(osFilePath))
            {
                Directory.CreateDirectory(osFilePath);
            }

            if (File.Exists(outfilePath))
            {
                File.Delete(outfilePath);
            }

            var delimiter = exportFormat.Equals(ExtensionXls) || exportFormat.Equals(ExtensionTxt) ? "\t" : ",";
            TextWriter txtfile = File.AppendText(outfilePath);
            if (exportFormat.Equals(ExtensionXls) || exportFormat.Equals(ExtensionCsv) || exportFormat.Equals(ExtensionTxt))
            {
                var sbNewLine = new StringBuilder();

                var columnHeadings = ReportsHelper.GetDataTableColumns(emailProfilesDt);
                var aListEnum = columnHeadings.GetEnumerator();

                while (aListEnum.MoveNext())
                {
                    sbNewLine.AppendFormat(
                        "{0}{1}{2}{3}",
                        exportFormat.Equals(ExtensionCsv) ? "\"" : "",
                        aListEnum.Current,
                        exportFormat.Equals(ExtensionCsv) ? "\"" : "",
                        delimiter);
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
                        sbNewLine.Append(exportFormat.Equals(ExtensionCsv) ? "\"" : "");
                        sbNewLine.Append(columnText);
                        sbNewLine.Append(exportFormat.Equals(ExtensionCsv) ? "\"" : "");
                        sbNewLine.Append(delimiter);
                    }

                    txtfile.WriteLine(sbNewLine.ToString());
                }
            }
            else if (exportFormat.Equals(".xml"))
            {
                var emailProfilesDS = new DataSet();
                emailProfilesDS.Tables.Add(emailProfilesDt);
                emailProfilesDS.WriteXml(txtfile);
            }

            txtfile.Close();
            ReportsHelper.WriteToLog("Built file for data export.");
        }

        private void LoadReportParameters(
            out string ftpUrl,
            out string ftpUserName,
            out string ftpPassword,
            out List<DataForExportContainer> dataForExportList,
            out string exportFormat,
            out string exportSubscribeTypeCode,
            out string exportSettingsAbbrev)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(ReportSchedule.ReportParameters);

            ReportsHelper.WriteToLog("Getting xml info for data export.");
            var exportSettingsNode = xDoc.GetElementsByTagName("ExportSettings")[0];
            var exportFormatNode = xDoc.GetElementsByTagName("ExportFormat")[0];
            var exportSubscribeTypeCodeNode = xDoc.GetElementsByTagName("ExportSubscribeTypeCode")[0];
            ftpUrl = xDoc.GetElementsByTagName("FtpUrl")[0].InnerText;
            ftpUserName = xDoc.GetElementsByTagName("FtpUsername")[0].InnerText;
            ftpPassword = xDoc.GetElementsByTagName("FtpPassword")[0].InnerText;
            dataForExportList = new List<DataForExportContainer>();

            //For each of the elements in the ftp exports section of the XML doc with a value of true, add an element to dataForExportList 
            var exports = xDoc.GetElementsByTagName("Exports")[0];
            ReportsHelper.WriteToLog("Got xml info for data export.");

            foreach (XmlNode exportTypeNode in exports.ChildNodes)
            {
                if (!exportTypeNode.InnerText.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                switch (exportTypeNode.Name.ToLower())
                {
                    case SendsToken:
                        dataForExportList.Add(GetDataForFtpExport(SendsToken, ReportSchedule));
                        break;
                    case OpensToken:
                        dataForExportList.Add(GetDataForFtpExport(OpensToken, ReportSchedule));
                        break;
                    case UnopenedToken:
                        dataForExportList.Add(GetDataForFtpExport(UnopenedToken, ReportSchedule));
                        break;
                    case ClicksToken:
                        dataForExportList.Add(GetDataForFtpExport(ClicksToken, ReportSchedule));
                        break;
                    case NoClicksToken:
                        dataForExportList.Add(GetDataForFtpExport(NoClicksToken, ReportSchedule));
                        break;
                    case BouncesToken:
                        dataForExportList.Add(GetDataForFtpExport(BouncesToken, ReportSchedule));
                        break;
                    case UnsubscribesToken:
                        dataForExportList.Add(GetDataForFtpExport(UnsubscribesToken, ReportSchedule));
                        break;
                    case ResendsToken:
                        dataForExportList.Add(GetDataForFtpExport(ResendsToken, ReportSchedule));
                        break;
                    case SuppressedToken:
                        dataForExportList.Add(GetDataForFtpExport(SuppressedToken, ReportSchedule));
                        break;
                    case ForwardsToken:
                        dataForExportList.Add(GetDataForFtpExport(ForwardsToken, ReportSchedule));
                        break;
                }
            }

            var exportSettings = exportSettingsNode.InnerText;
            exportFormat = exportFormatNode.InnerText;
            exportSubscribeTypeCode = exportSubscribeTypeCodeNode.InnerText;

            exportSubscribeTypeCode = exportSubscribeTypeCode.Equals("*")
                ? " 'S', 'U', 'D', 'P', 'B', 'M' "
                : $" '{exportSubscribeTypeCode}' ";

            exportSubscribeTypeCode = exportSubscribeTypeCode.Length > 5
                ? "ALL"
                : exportSubscribeTypeCode.Replace("'", "").Trim();
            exportSettingsAbbrev = "";

            if (exportSettings.ToLower().Equals("profileplusstandalone"))
            {
                exportSettingsAbbrev = "Standalone";
            }
            else if (exportSettings.ToLower().Equals("profileplusalludfs"))
            {
                exportSettingsAbbrev = "AllUDFs";
            }
            else
            {
                exportSettingsAbbrev = "Profile";
            }
        }

        //This method returns a list of the file paths with file names of the locally stored files to be exported to the ftp site specified in the credentials.
        private DataForExportContainer GetDataForFtpExport(string reportType, ReportSchedule reportSchedule)
        {
            ReportsHelper.WriteToLog("Running " + reportType + " export.");
            var container = new DataForExportContainer
            {
                DataToBeExported = new DataTable(),
                NameOfReport = reportType
            };

            var blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(reportSchedule.BlastID.Value, false);
            var startDate = blast.SendTime.Value;
            var endDate = DateTime.Now;

            switch (reportType.ToLower())
            {
                case SendsToken:
                    SetExportedData(reportSchedule, blast, reportType, ref container, "send", "", "", startDate, endDate);
                    break;
                case OpensToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "open", "all", "", startDate, endDate);
                    break;
                case UnopenedToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "noopen", "", "", startDate, endDate);
                    break;
                case ClicksToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "click", "all", "", startDate, endDate);
                    break;
                case NoClicksToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "noclick", "", "", startDate, endDate);
                    break;

                case BouncesToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "bounce", "", "", startDate, endDate);
                    break;
                case UnsubscribesToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "unsubscribe", "all", "", startDate, endDate);
                    break;
                case ResendsToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "resend", "all", "", startDate, endDate);
                    break;
                case SuppressedToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, SuppressedToken, "all", "", startDate, endDate);
                    break;
                case ForwardsToken:
                SetExportedData(reportSchedule, blast, reportType, ref container, "forward", "", "", startDate, endDate);
                    break;
                default:
                    ReportsHelper.WriteToLog("Tried to generate a report that wasn't accounted for.");
                    break;
            }
            return container;
        }

        public FtpExportsReport(EmailDirect message, string body, ReportSchedule reportSchedule, DateTime masterStartDate) : base(message, body, reportSchedule, masterStartDate)
        {
        }
    }
}
