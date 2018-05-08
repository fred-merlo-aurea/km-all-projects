using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using ecn.automatedreporting.Reports;
using ecn.automatedreporting.Reports.Helpers;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects.EmailDirect;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using Microsoft.Reporting.WebForms;
using AdvertiserClickReport = ecn.automatedreporting.Reports.AdvertiserClickReport;
using AudienceEngagementReport = ecn.automatedreporting.Reports.AudienceEngagementReport;
using BlastClickReport = ECN_Framework_BusinessLayer.Activity.Report.BlastClickReport;
using BlastReport = ECN_Framework_Entities.Activity.Report.BlastReport;
using BlastReportDetail = ECN_Framework_Entities.Activity.Report.BlastReportDetail;
using BlastReportPerformance = ECN_Framework_BusinessLayer.Activity.Report.BlastReportPerformance;
using BlastResponseDetail = ECN_Framework_BusinessLayer.Activity.Report.BlastResponseDetail;
using DataDumpReport = ecn.automatedreporting.Reports.DataDumpReport;
using EmailPerformanceByDomainReport = ecn.automatedreporting.Reports.EmailPerformanceByDomainReport;
using GroupStatisticsReport = ecn.automatedreporting.Reports.GroupStatisticsReport;
using ReportQueue = ECN_Framework_BusinessLayer.Communicator.ReportQueue;
using UnsubscribeReasonDetail = ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail;
using User = KMPlatform.Entity.User;

namespace ecn.automatedreporting
{
    class Program
    {
        private const string FileTypePdf = "PDF";
        private const string FileTypeXLS = "XLS";
        private const string FileTypeXML = "XML";
        private const string FileTypeCSV = "CSV";
        private const string FileTypeExcel = "Excel";        
        private const string FileExtensionPdf = ".pdf";        
        private const string RenderedLogMessage = "Rendered report.";
        private const string AttachingLogMessage = "Attaching report.";
        private const string AttachedLogMessage = "Attached report.";
        private const string ErrorLogMessage = "Error running report";
        private const string ErrorLogMessageFormatted = "Error running {0} report";
        private const string ScheduleIdLogMessageFormattedText = "ReportScheduleID: {0}";        
        private const string NoDataLogText = "No Data.";
        private const string NoDataReportMessageText = "No Data";        
        private const string ErrorMovingToFtp = "Error moving export to FTP";        
        private const string Montly = "monthly";
        private const string Weekly = "weekly";
        private const string Daily = "daily";
        private const string BlastDetailsReportName = "Blast Details";
        private const string BlastDetailsPrefix = "BlastDetailsReport_";
        private const string BlastDetailsReportIdentifier = "ecn.automatedReporting.GetBlastDetailsReport";
        private const string BlastDetailsExceptionBodyMessage = "<br />Your scheduled Blast Details Report has failed.<br />";
        private const string BlastDetailsSucceedBodyMessage = "<br />Your scheduled Blast Details Report has been attached.<br />";
        private const string BlastDetailsNoDataBodyMessage = "<br />Your scheduled Blast Details Report didn't return any data.<br />";
        private const string UnsunscribeReasonReportTableName = "UnsubscribeReasonReport";
        private const string UnsubscribeReasonReportIdentifier = "ecn.automatedReporting.GetDataDumpReport";
        private const string UnsubscribeReasonExceptionBodyMessage = "</BR>Your scheduled export of Unsubscribe Reason Report has failed";
        private const string UnsubscribeReasonSucceedBodyMessageFormatted = "</BR>Your scheduled export of Unsubscribe Reason Report has been moved to {0}";
        private const string UnsubscribeReasonFailBodyMessage = "</BR>Your scheduled export of Unsubscribe Reason Report has failed due to an FTP issue";
        private const string UnsubscribeReasonNoDataBodyMessage = "</BR>Your scheduled export of Unsubscribe Reason Report didn't return any data";
        private const string FtpIssue = "FTP Issue.";

        #region testing
        private static ReportSchedule testSetup()
        {
            Console.WriteLine("Starting GetNextDate Tests");
            ReportSchedule rs = new ReportSchedule();
            rs.StartDate = DateTime.Now.AddYears(-2).ToString();
            rs.EndDate = DateTime.Parse(rs.StartDate).AddYears(4).ToString();
            rs.StartTime = "11:00:00";
            rs.ScheduleType = "Recurring";
            rs.MonthScheduleDay = 21;
            rs.RecurrenceType = "Monthly";
            rs.MonthLastDay = false;
            return rs;
        }
        private static void testTearDown()
        {
            Console.WriteLine("Tests Complete");
        }
        public static void testGetNextData()
        {
            ReportSchedule rs = testSetup();
            int total_tests = 1000000;
            for (int tests = 0; tests < total_tests; tests++)
            {
                DateTime? newDT = ReportQueue.GetNextDateToRun(rs, false);
                DateTime? cur;
                DateTime sendDate = Convert.ToDateTime(DateTime.Now.Month.ToString() + '/' + rs.MonthScheduleDay + '/' + "2016" + " " + rs.StartTime);
                if (DateTime.Now > sendDate)
                    cur = sendDate.AddMonths(1);
                else
                    cur = sendDate;
                if (newDT == null)
                {
                    Console.WriteLine("Failed case -- newDT == null");
                    Console.WriteLine("newDT = " + newDT);
                }
                if (newDT != cur)
                    Console.WriteLine("Failed Case -- " + newDT + "!=" + cur);
                rs.StartTime = Convert.ToDateTime(rs.StartTime).AddSeconds(1).TimeOfDay.ToString();
                if (rs.StartTime == "23:59:59")
                    rs.MonthScheduleDay++;
                if (rs.MonthScheduleDay > 30)
                    rs.MonthScheduleDay = 1;
            }
            testTearDown();
            Console.WriteLine(DateTime.Now.Month.ToString() + '/' + rs.MonthScheduleDay + '/' + "2016" + " " + rs.StartTime);
            //DateTime.Now.ToString("MM/dd/yyyy") + " " + rs.StartTime
        }
        #endregion testing
        private static DateTime MasterStartDate;
        public static string log = "";
        public static string LogFile = "";        
        public struct dataForExportContainer
        {
            public DataTable dataToBeExported;
            public string nameOfReport;
        }
        public static int GlobalBlastId { get; private set; }
        static DateTime CurrentDate { get; set; }
        private static void CheckLogFileDate()
        {
            if (false == CurrentDate.Date.Equals(DateTime.Now.Date) || String.IsNullOrEmpty(log) || null == ReportsHelper.LogFile)
            {
                if (ReportsHelper.LogFile != null)
                {
                    ReportsHelper.LogFile.Dispose();
                }
                log = "Log/" + DateTime.Now.Date.ToString("MM-dd-yyyy") + ".log";
                try
                {
                    ReportsHelper.LogFile = new StreamWriter(new FileStream(log, FileMode.Append));
                }
                catch (IOException ioException)
                {
                    // still want to the program to die if we can't open our log-file, this ensure logging to KMCommon 
                    // and makes the message we will find there and in the the System's .NET RUNTIME Event log more specific
                    ApplicationException ae = new ApplicationException(String.Format(@"Automated Reporting: FAILED TO OPEN LOG FILE ""{0}""", log), ioException);
                    ApplicationLog.LogCriticalError(ae, "ECN_Automated_Reporting.CheckLogFileDate", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                    throw ae;
                }
                MasterStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                CurrentDate = DateTime.Now;
            }
        }
        static void Main(string[] args)
        {
            User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"], false);

            CheckLogFileDate();
            ReportsHelper.WriteToLog("");
            ReportsHelper.WriteToLog("");
            ReportsHelper.WriteToLog("-START--------------------------------------");


            EmailDirect ed = new EmailDirect();
            int? blastID = null;
            string body = string.Empty;
            bool success = false;
            while (true)
            {
                ReturnReport results = new ReturnReport();

                CheckLogFileDate();

                ECN_Framework_Entities.Communicator.ReportQueue lstSchedule = ReportQueue.GetNextToSend(blastID);
                ReportSchedule rsFirst = null;

                try
                {
                    try
                    {
                        rsFirst = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(lstSchedule.ReportScheduleID, user);
                        if (rsFirst == null)
                        {
                            if (lstSchedule.ReportQueueID > 0)
                            {

                                lstSchedule.Status = "Failed";
                                lstSchedule.FailureReason = "Report was deleted";
                                ReportQueue.UpdateStatus(lstSchedule.ReportQueueID, "Failed", "Report Was Deleted");

                            }
                        }
                    }
                    catch
                    {
                        //No more reports to run, sleeping for 15 min
                        ReportsHelper.WriteToLog("No reports to run--- Sleeping for 15 min");
                        Thread.Sleep(900000);
                    }
                    if (rsFirst != null)
                    {
                        ReportQueue.UpdateStatus(lstSchedule.ReportQueueID, "Active", "");
                        ReportsHelper.WriteToLog("Running ReportQueueID: " + lstSchedule.ReportQueueID);
                        ReportsHelper.Assembly = Assembly.LoadFrom("ECN_Framework_Common.dll");

                        ed.EmailAddress = rsFirst.ToEmail;
                        ed.EmailSubject = rsFirst.EmailSubject;
                        ed.FromEmailAddress = "automatedreports@ecn5.com";
                        ed.ReplyEmailAddress = "automatedreports@ecn5.com";
                        ed.EmailAddress = rsFirst.ToEmail;
                        ed.EmailSubject = rsFirst.EmailSubject;
                        ed.CustomerID = rsFirst.CustomerID.Value;
                        ed.CreatedUserID = user.UserID;
                        ed.Status = Enums.Status.Active.ToString();
                        ed.FromName = rsFirst.FromName;
                        ed.Process = "Automated Reporting - Main";
                        ed.Source = "Automated Reporting";
                        if (rsFirst.ReportID == 9 || rsFirst.ReportID == 10)
                        {
                            XmlDocument xDoc = new XmlDocument();
                            xDoc.LoadXml(rsFirst.ReportParameters);
                            XmlNode ccEmails = xDoc.GetElementsByTagName("ccEmails")[0];
                            foreach (XmlNode ccEmail in ccEmails)
                            {
                                if (!ed.CCAddresses.Contains(ccEmail.InnerText))
                                    ed.CCAddresses.Add(ccEmail.InnerText);
                            }
                        }
                        //keep adding content to the current message
                        results = RunReport(ed, ref body, rsFirst);
                        bool anotherOneToProcess = false;
                        if (rsFirst.BlastID != null)
                        {
                            ECN_Framework_Entities.Communicator.ReportQueue rqToCheck = ReportQueue.GetNextToSend(rsFirst.BlastID);

                            ReportSchedule rsToCheck = null;

                            if (rqToCheck != null)
                            {
                                rsToCheck = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(rqToCheck.ReportScheduleID, user);
                            }

                            if (rsToCheck == null || rsToCheck.BlastID != rsFirst.BlastID)
                            {
                                ed.Content = getBodyHTML().Replace("%%BODY%%", body);
                                ReportsHelper.WriteToLog("Done with ReportQueueID: " + lstSchedule.ReportQueueID + ".  Sending Email");
                                success = ProcessEmail(ed);
                                body = string.Empty;
                                ed = new EmailDirect();
                            }
                            else
                                anotherOneToProcess = true;

                        }
                        else
                        {
                            ed.Content = getBodyHTML().Replace("%%BODY%%", body);
                            ReportsHelper.WriteToLog("Done with ReportQueueID: " + lstSchedule.ReportQueueID + ".  Sending Email");
                            success = ProcessEmail(ed);
                            body = string.Empty;
                            ed = new EmailDirect();
                        }
                        if (results.success && success)
                            ReportQueue.UpdateStatus(lstSchedule.ReportQueueID, "Sent", "");
                        else
                        {
                            if (!success)
                            {
                                ReportQueue.UpdateStatus(lstSchedule.ReportQueueID, "Failed", "Error sending email");

                            }
                            else
                            {
                                ReportQueue.UpdateStatus(lstSchedule.ReportQueueID, "Sent", results.message);
                            }

                        }
                        if (!anotherOneToProcess)
                        {
                            body = string.Empty;
                            ed = new EmailDirect();
                        }
                        //schedule next report
                        if (rsFirst != null && rsFirst.ScheduleType.ToLower().Equals("recurring"))
                        {
                            DateTime? nextToSend = ReportQueue.GetNextDateToRun(rsFirst, false);
                            if (nextToSend.HasValue)
                            {
                                ECN_Framework_Entities.Communicator.ReportQueue rqNew = new ECN_Framework_Entities.Communicator.ReportQueue();
                                rqNew.ReportID = rsFirst.ReportID.Value;
                                rqNew.ReportScheduleID = rsFirst.ReportScheduleID;
                                rqNew.SendTime = nextToSend.Value;
                                rqNew.Status = "Pending";

                                ReportQueue.Save(rqNew, false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    body = string.Empty;
                    ed = new EmailDirect();
                    ReportsHelper.WriteToLog("Error in main loop");

                    ReportsHelper.WriteToLog(ex.Message);

                    ApplicationLog.LogCriticalError(ex, "ecn.automatedReporting.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                    if (lstSchedule != null && rsFirst != null)
                    {

                        ReportQueue.UpdateStatus(lstSchedule.ReportQueueID, "Failed", "Error running report");

                        if (rsFirst != null && rsFirst.ScheduleType.ToLower().Equals("recurring"))
                        {
                            //schedule next report
                            DateTime? nextToSend = ReportQueue.GetNextDateToRun(rsFirst, false);
                            if (nextToSend.HasValue)
                            {
                                ECN_Framework_Entities.Communicator.ReportQueue rqNew = new ECN_Framework_Entities.Communicator.ReportQueue();
                                rqNew.ReportID = rsFirst.ReportID.Value;
                                rqNew.ReportScheduleID = rsFirst.ReportScheduleID;
                                rqNew.SendTime = nextToSend.Value;
                                rqNew.Status = "Pending";

                                ReportQueue.Save(rqNew, false);
                            }
                        }
                    }
                }

                Thread.Sleep(2000); //sleep for 2 seconds
            }
        }

        private static bool ProcessEmail(EmailDirect ed)
        {
            try
            {

                int edID = EmailDirectEngine.Program.ProcessEmailDirectWithAttachments(ed);
                //body = string.Empty;
                //ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                if (edID > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                try
                {
                    int edID = EmailDirectEngine.Program.ProcessEmailDirectWithAttachments(ed);
                    //body = string.Empty;
                    //ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                    if (edID > 0)
                        return true;
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static ReturnReport RunReport(EmailDirect message, ref string body, ReportSchedule rs)
        {
            ReturnReport hasData = new ReturnReport();
            try
            {
                ReportsHelper.WriteToLog("Starting " + rs.Report.ReportName);
                IDeliveryReport reportToExecute = null;
                switch (rs.ReportID.Value)
                {
                    case 1:
                        reportToExecute = new BlastDeliveryReport(message, body, rs, MasterStartDate);
                        break;
                    case 2:
                        reportToExecute = new EmailPreviewUsageReport(message, body, rs, MasterStartDate);
                        break;
                    case 3:
                        reportToExecute = new EmailPerformanceByDomainReport(message, body, rs, MasterStartDate);
                        break;
                    case 4:
                        reportToExecute = new GroupStatisticsReport(message, body, rs, MasterStartDate);
                        break;
                    case 5:
                        reportToExecute = new AudienceEngagementReport(message, body, rs, MasterStartDate);
                        break;
                    case 6:
                        reportToExecute = new AdvertiserClickReport(message, body, rs, MasterStartDate);
                        break;
                    case 8:
                        reportToExecute = new GroupExportReport(message, body, rs, MasterStartDate);
                        break;
                    case 9:
                        hasData = GetBlastDetailsReport(message, ref body, rs);
                        break;
                    case 10:
                        reportToExecute = new FtpExportsReport(message, body, rs, MasterStartDate);
                        break;
                    case 11:
                        reportToExecute = new DataDumpReport(message, body, rs, MasterStartDate);
                        break;
                    case 12:
                        hasData = GetUnsubscribeReasonReport(message, ref body, rs);
                        break;
                }

                if (reportToExecute != null)
                {
                    hasData = reportToExecute?.Execute();
                    body += reportToExecute.Body;
                }

                ReportsHelper.WriteToLog("Done with " + rs.Report.ReportName);

                return hasData;
            }
            catch (Exception ex)
            {
                ReportsHelper.WriteToLog("Error with " + rs.Report.ReportName);
                ReportsHelper.WriteToLog(ex.Message);

                hasData.success = false;
                hasData.message = "Error running report";
                return hasData;
            }

        }

        #region standard reports
        private static ReturnReport GetBlastDetailsReport(EmailDirect message, ref string body, ReportSchedule rs)
        {
            ReturnReport reportMessage = new ReturnReport();
            ReportViewer rv = new ReportViewer();
            try
            {
                User user = KMPlatform.BusinessLogic.User.GetByUserID(rs.CreatedUserID.Value, false);

                BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(rs.BlastID.Value, false);
                DateTime startDate = blast.SendTime.Value;
                DateTime endDate = DateTime.Now;
                ReportsHelper.WriteToLog("Getting Blast Details Report data.");

                GlobalBlastId = blast.BlastID;  //for subReports
                List<BlastReport> lstBlastReport = ECN_Framework_BusinessLayer.Activity.Report.BlastReport.Get(blast.BlastID);
                List<BlastReportDetail> lstBlastReportDetail = ECN_Framework_BusinessLayer.Activity.Report.BlastReportDetail.Get(blast.BlastID);
                if (lstBlastReport != null && lstBlastReport.Count > 0)
                {
                    foreach (BlastReport br in lstBlastReport)
                    {
                        br.EmailSubject = EmojiFunctions.GetSubjectUTF(br.EmailSubject);
                    }
                    ReportDataSource rdsBlastReport = new ReportDataSource("DS_BlastReport", lstBlastReport);
                    ReportDataSource rdsBlastReportDetail = new ReportDataSource("DS_BlastReportDetail", lstBlastReportDetail);

                    rv.Visible = false;
                    rv.LocalReport.DataSources.Clear();
                    rv.LocalReport.DataSources.Add(rdsBlastReport);
                    rv.LocalReport.DataSources.Add(rdsBlastReportDetail);
                    Stream stream = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastPDFReport.rdlc");
                    rv.LocalReport.LoadReportDefinition(stream);

                    Stream streamSubRpt = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastPerformance.rdlc");
                    rv.LocalReport.LoadSubreportDefinition("rpt_BlastPerformance", streamSubRpt);
                    streamSubRpt = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastResponseDetail.rdlc");
                    rv.LocalReport.LoadSubreportDefinition("rpt_BlastResponseDetail", streamSubRpt);
                    streamSubRpt = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastClickReport.rdlc");
                    rv.LocalReport.LoadSubreportDefinition("rpt_BlastClickReport", streamSubRpt);

                    rv.LocalReport.SubreportProcessing += ReportViewer1_SubReport;
                    rv.LocalReport.Refresh();

                    Warning[] warnings = null;
                    string[] streamids = null;
                    String mimeType = null;
                    String encoding = null;
                    String extension = null;
                    Byte[] bytes = null;

                    ReportsHelper.WriteToLog("Generating report.");
                    bytes = rv.LocalReport.Render(FileTypePdf, "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    ReportsHelper.WriteToLog("Generated report.");

                    var attach = new Attachment(new MemoryStream(bytes), $"{BlastDetailsPrefix}{blast.BlastID}{FileExtensionPdf}");
                    ReportsHelper.WriteToLog(AttachingLogMessage);
                    message.Attachments.Add(attach);
                    WriteLogSucceedAttached(reportMessage, ref body, BlastDetailsSucceedBodyMessage);
                    return reportMessage;
                }

                WriteLogNoData(reportMessage, ref body, BlastDetailsNoDataBodyMessage);
                return reportMessage;
            }
            catch (Exception ex)
            {
                LogExceptionWithCustomBodyMessage(
                    reportMessage,
                    ex,
                    BlastDetailsReportName,
                    rs.ReportScheduleID.ToString(),
                    ref body,
                    BlastDetailsReportIdentifier,
                    BlastDetailsExceptionBodyMessage);
                return reportMessage;
            }
        }

        static void ReportViewer1_SubReport(object sender, SubreportProcessingEventArgs e)
        {
            var tmp = GlobalBlastId;

            if (e.ReportPath == "rpt_BlastPerformance")
            {
                e.DataSources.Add(new ReportDataSource("DS_BlastReportPerformance", BlastReportPerformance.Get(GlobalBlastId)));
            }
            else if (e.ReportPath == "rpt_BlastResponseDetail")
            {
                e.DataSources.Add(new ReportDataSource("DS_BlastResponseDetail", BlastResponseDetail.Get(GlobalBlastId)));
            }
            else
            {
                e.DataSources.Add(new ReportDataSource("DS_BlastClickReport", BlastClickReport.Get(GlobalBlastId)));
            }
        }
        #endregion
        #region DataExports
        private static ReturnReport GetUnsubscribeReasonReport(EmailDirect message, ref string body, ReportSchedule rs)
        {
            ReturnReport reportMessage = new ReturnReport();
            try
            {
                ReportsHelper.WriteToLog("Getting xml info for data export.");
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rs.ReportParameters);

                XmlNode nodeSearchBy = xDoc.DocumentElement.SelectSingleNode("SearchBy");
                XmlNode nodeSearchCriteria = xDoc.DocumentElement.SelectSingleNode("SearchText");
                XmlNode FTPURL = xDoc.DocumentElement.SelectSingleNode("FTPURL");
                XmlNode FTPUsername = xDoc.DocumentElement.SelectSingleNode("FTPUsername");
                XmlNode FTPPassword = xDoc.DocumentElement.SelectSingleNode("FTPPassword");
                ReportsHelper.WriteToLog("Got xml info for data export.");

                string OSFilePath = ConfigurationManager.AppSettings["FilePath"] + "\\customers\\" + rs.CustomerID.Value + "\\downloads\\";
                String tfile = rs.CustomerID.Value + "-UnsubscribeReasonReport-" + DateTime.Now.ToString("MMddyyyy") + "." + rs.ExportFormat.ToLower();
                string outfileName = OSFilePath + tfile;

                if (!(Directory.Exists(OSFilePath)))
                {
                    Directory.CreateDirectory(OSFilePath);
                }

                if (File.Exists(outfileName))
                {
                    File.Delete(outfileName);
                }

                //getting start and end date
                DateTime startDate = MasterStartDate;
                DateTime endDate = DateTime.Now;
                SetStartDateAndEndDateDefaultNowOneWeekAgoWithAddHours(rs.RecurrenceType, out startDate, out endDate, startDate, endDate);

                    startDate = ReportsHelper.GetPreviousWeekStartDate(MasterStartDate);// new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now. DateTime.Now.AddDays(-7);

                List<UnsubscribeReasonDetail> listUnsubDetail = ECN_Framework_BusinessLayer.Activity.Report.UnsubscribeReasonDetail.GetReport(nodeSearchBy.InnerText, nodeSearchCriteria.InnerText, startDate, endDate, rs.CustomerID.Value, string.Empty);
                //List<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail> listUnsubDetail = ECN_Framework_BusinessLayer.Activity.Report.UnsubscribeReasonDetail.Get(string.Empty, string.Empty, rs.CustomerID.Value, new DateTime(2015, 1, 1), endDate, string.Empty);
                if (listUnsubDetail.Count > 0)
                {
                    foreach (var a in listUnsubDetail)
                    {
                        if (rs.ExportFormat.ToLower().Equals("csv"))
                        {
                            a.EmailSubject = EmojiFunctions.ReplaceEmojiWithQuestion(a.EmailSubject);
                        }
                        else
                        {
                            a.EmailSubject = EmojiFunctions.GetSubjectUTF(a.EmailSubject);
                        }
                    }
                    ReportViewer report = new ReportViewer();
                    ReportDataSource rds = new ReportDataSource("DataSet1", listUnsubDetail);
                    report.Visible = true;
                    report.LocalReport.DataSources.Clear();
                    report.LocalReport.DataSources.Add(rds);

                    Stream stream = ReportsHelper.Assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_UnsubscribeReasonDetail.rdlc");
                    report.LocalReport.LoadReportDefinition(stream);
                    report.LocalReport.Refresh();

                    switch (rs.ExportFormat.ToUpper())
                    {
                        case FileTypePdf:
                            RenderAndWritePDF(report, outfileName);
                            break;
                        case FileTypeXLS:
                            RenderAndWriteXLS(report, outfileName);
                            break;
                        case FileTypeXML:
                            DataTable dtNew = ReportViewerExport.ToDataTable(listUnsubDetail);
                            WriteTableAsXml(outfileName, dtNew, UnsunscribeReasonReportTableName);
                            break;
                        case FileTypeCSV:
                            File.WriteAllBytes(outfileName, Encoding.ASCII.GetBytes(ReportViewerExport.GetCSV(listUnsubDetail)));
                            break;
                    }

                    ReportsHelper.WriteToLog(RenderedLogMessage);

                    #region File Building

                    //moving file to ftp
                    string remoteFilePath = tfile;
                    bool success = ReportsHelper.MoveFileToFtp(FTPURL.InnerText, remoteFilePath, FTPUsername.InnerText, FTPPassword.InnerText, outfileName);

                    #endregion
                    if (success)
                    {
                        string bodyMessage = string.Format(UnsubscribeReasonSucceedBodyMessageFormatted, FTPURL.InnerText);
                        WriteLogSucceed(reportMessage, ref body, bodyMessage);
                        return reportMessage;
                    }

                    ReportsHelper.WriteToLog(FtpIssue);
                    WriteLogError(reportMessage, ref body, UnsubscribeReasonFailBodyMessage);
                    return reportMessage;
                }

                WriteLogNoData(reportMessage, ref body, UnsubscribeReasonNoDataBodyMessage);
                return reportMessage;
            }
            catch (Exception ex)
            {
                LogExceptionWithCustomBodyMessageWithoutLogging(
                    reportMessage,
                    ex,
                    ref body,
                    UnsubscribeReasonReportIdentifier,
                    UnsubscribeReasonExceptionBodyMessage);
                return reportMessage;
            }
        }
        #endregion

        private static string getBodyHTML()
        {
            string retHTML = "<html> <head>         <title></title>    </head>    <body>        <table border='0' width='750'>            <tbody>                <tr>                    <td align='center' style='font-family: Arial, Helvetica, sans-serif; font-size: 10px;'>&nbsp;</td>" +
                "</tr>                <tr>                    <td align='left'><a href='http://www.ecn5.com/index.htm'><img border='0' src='http://www.ecn5.com/ecn.images/Channels/12/kmlogo.jpg' /></a></td>                </tr>                <tr>                    <td align='center' bgcolor='#666666' height='10'>&nbsp;</td>" +
                "</tr>                <tr>                    <td align='center' style='font-family: Arial, Helvetica, sans-serif; font-size: 10px;'>&nbsp;</td>                </tr>                <tr>                    <td align='left' style='font-family: Arial, Helvetica, sans-serif; font-size: 12px;'>%%BODY%%</td>" +
                "</tr>                <tr>                    <td align='center' style='font-family: Arial, Helvetica, sans-serif; font-size: 20px;'>&nbsp;</td>                </tr>            </tbody>        </table>    </body></html>";

            return retHTML;
        }

        private static void WriteTableAsXml(string outfileName, DataTable dtNew, string tableName)
        {
            using (var exportFile = new FileStream(outfileName, FileMode.Append))
            {
                try
                {
                    dtNew.TableName = tableName;
                    dtNew.WriteXml(exportFile, false);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error: {0}", ex);
                }
            }
        }

        private static void RenderAndWritePDF(ReportViewer reportViewer, string filePath)
        {
            RenderAndWriteFile(reportViewer, FileTypePdf, filePath);
        }

        private static void RenderAndWriteXLS(ReportViewer reportViewer, string filePath)
        {
            RenderAndWriteFile(reportViewer, FileTypeExcel, filePath);
        }

        private static void RenderAndWriteFile(ReportViewer reportViewer, string fileType, string filePath)
        {
            Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;

            byte[] bytes = reportViewer.LocalReport.Render(fileType, string.Empty, out mimeType, out encoding, out extension, out streamids, out warnings);
            File.WriteAllBytes(filePath, bytes);
        }              

        private static void LogExceptionWithCustomBodyMessage(
            ReturnReport reportMessage,
            Exception ex,
            string reportName,
            string scheduleId,
            ref string body,
            string reportIdentifier,
            string bodyMessage)
        {
            if (string.IsNullOrWhiteSpace(reportName))
            {
                ReportsHelper.WriteToLog(string.Format(ErrorLogMessage));
            }
            else
            {
                ReportsHelper.WriteToLog(string.Format(ErrorLogMessageFormatted, reportName));
            }
            ReportsHelper.WriteToLog(string.Format(ScheduleIdLogMessageFormattedText, scheduleId));
            ReportsHelper.WriteToLog(ex.Message);
            LogExceptionWithCustomBodyMessageWithoutLogging(reportMessage, ex, ref body, reportIdentifier, bodyMessage);
        }

        private static void LogExceptionWithCustomBodyMessageWithoutLogging(
            ReturnReport reportMessage,
            Exception ex,
            ref string body,
            string reportIdentifier,
            string bodyMessage)
        {
            ProcessLog(reportMessage, ref body, bodyMessage, false, ErrorLogMessage);
            ApplicationLog.LogCriticalError(ex, reportIdentifier, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
        }

        private static void WriteLogNoData(
            ReturnReport reportMessage,
            ref string body,
            string bodyMessage)
        {
            WriteLog(reportMessage, ref body, bodyMessage, false, NoDataLogText, NoDataReportMessageText);
        }
        private static void WriteLog(
            ReturnReport reportMessage,
            ref string body,
            string bodyMessage,
            bool result,
            string logMessage,
            string reportMessageText)
        {
            ReportsHelper.WriteToLog(logMessage);
            ProcessLog(reportMessage, ref body, bodyMessage, result, reportMessageText);
        }

        private static void WriteLogSucceedAttached(
            ReturnReport reportMessage,
            ref string body,
            string bodyMessage)
        {
            ReportsHelper.WriteToLog(AttachedLogMessage);
            WriteLogSucceed(reportMessage, ref body, bodyMessage);
        }

        private static void WriteLogSucceed(
            ReturnReport reportMessage,
            ref string body,
            string bodyMessage)
        {
            string reportMessageText = null;
            ProcessLog(reportMessage, ref body, bodyMessage, true, reportMessageText);
        }

        private static void WriteLogError(
            ReturnReport reportMessage,
            ref string body,
            string bodyMessage)
        {
            ProcessLog(reportMessage, ref body, bodyMessage, false, ErrorMovingToFtp);
        }

        private static void ProcessLog(
            ReturnReport reportMessage,
            ref string body,
            string bodyMessage,
            bool? result,
            string reportMessageText)
        {
            if (bodyMessage != null)
            {
                body += bodyMessage;
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

        private static void SetStartDateAndEndDateNoDefault(
            string recurrenceType,
            out DateTime startDate,
            out DateTime endDate,
            DateTime initialStartDate,
            DateTime initialEndDate,
            DateTime? noRecurrenceDefaultStartDate = null)
        {
            startDate = initialStartDate;
            endDate = initialEndDate;
            if (recurrenceType.Equals(Montly, StringComparison.OrdinalIgnoreCase))
            {
                startDate = new DateTime(MasterStartDate.Month == 1
                    ? MasterStartDate.Year - 1
                    : MasterStartDate.Year, MasterStartDate.Month == 1
                        ? 12
                        : MasterStartDate.Month - 1, 1, 0, 0, 0);
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

        private static void SetStartDateAndEndDateDefaultNowOneWeekAgoWithAddHours(
            string recurrenceType,
            out DateTime startDate,
            out DateTime endDate,
            DateTime initialStartDate,
            DateTime initialEndDate)
        {
            DateTime? noRecurrenceDefaultStartDate = DateTime.Now.AddDays(-7).AddHours(23).AddMinutes(59).AddSeconds(59);
            SetStartDateAndEndDateNoDefault(recurrenceType, out startDate, out endDate, initialStartDate, initialEndDate, noRecurrenceDefaultStartDate);
        }
    }
    public class ReturnReport
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
}
