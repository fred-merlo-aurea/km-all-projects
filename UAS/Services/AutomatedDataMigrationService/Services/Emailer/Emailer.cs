using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Core.ADMS.Events;
using Core.ADMS;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace ADMS.Services.Emailer
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value == true ? "Yes" : "No";
        }
        public static string ToGoodBadString(this bool value)
        {
            return value == true ? "Processed" : "Invalid";
        }
    }

    public class Emailer : ServiceBase, IEmailer
    {
        public Emailer(FrameworkUAD_Lookup.Enums.MailMessageType mailMessage)
        {

        }
        public Emailer()
        {

        }

        [ExcludeFromCodeCoverage] // Excluding because function only writes to Console and executes tested functions
        public void HandleFileProcessed(FileProcessed eventMessage)
        {
            try
            {
                KMPlatform.BusinessLogic.ServiceFeature sfData = new KMPlatform.BusinessLogic.ServiceFeature();
                KMPlatform.Entity.ServiceFeature feature = sfData.SelectServiceFeature(eventMessage.SourceFile.ServiceFeatureID);
                bool isDataCompare = false;
                if (feature != null)
                {
                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                        isDataCompare = true;
                }

                FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
                ConsoleMessage("EMAIL: File Processed Emailing Report.");


                //Used to send one email per request
                StringBuilder reportBody = GetReportBodyForSingleEmail(eventMessage);
                string badDataAttachment = string.Empty;
                if (eventMessage.ValidationResult.RecordImportErrorCount > 0)
                    badDataAttachment = vrWorker.GetBadData(eventMessage.ValidationResult, eventMessage.SourceFile);

                ConsoleMessage(reportBody.ToString());

                //Log in ImportError table
                FrameworkUAD.BusinessLogic.ImportError ieWorker = new FrameworkUAD.BusinessLogic.ImportError();
                FrameworkUAD.Entity.ImportError ie = new FrameworkUAD.Entity.ImportError(-1, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID, "Import Details", string.Empty, 0, reportBody.ToString(), badDataAttachment, false);
                ieWorker.Save(ie, eventMessage.Client.ClientConnections);

                MailPriority mp = MailPriority.Normal;
                if (eventMessage.ValidationResult.HasError == true || eventMessage.ValidationResult.DimensionImportErrorCount > 0 || eventMessage.ValidationResult.RecordImportErrorCount > 0 || eventMessage.ValidationResult.UnexpectedColumns.Count > 0 || eventMessage.ValidationResult.NotFoundColumns.Count > 0 || eventMessage.ValidationResult.DuplicateColumns.Count > 0)
                    mp = MailPriority.High;

                SendEmail(GetMailMessage(eventMessage.Client, reportBody.ToString(), eventMessage.ImportFile, eventMessage.IsValidFileType, eventMessage.IsFileSchemaValid, "text/html", eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID, badDataAttachment, mp, true, isDataCompare), eventMessage.Client);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileProcessed");
            }
        }

        [ExcludeFromCodeCoverage] // Excluding because function only writes to Console and executes tested functions
        public void HandleCustomFileProcessed(CustomFileProcessed eventMessage)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.Subject = eventMessage.Client.FtpFolder + ": " + eventMessage.ImportFile.Name + " - " + BooleanExtensions.ToGoodBadString(eventMessage.IsValid);
                if (!string.IsNullOrEmpty(eventMessage.Client.AccountManagerEmails))
                    message.To.Add(eventMessage.Client.AccountManagerEmails);
                message.Bcc.Add(Core.ADMS.Settings.AdminEmail);
                message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
                message.IsBodyHtml = false;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Custom File was ran");
                sb.AppendLine(string.Empty);
                message.Body = sb.ToString();
                string userState = "Message";
                SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                smtp.SendAsync(message, userState);
                ConsoleMessage("Custom file processed...Awaiting new file....");
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleCustomFileProcessed");
            }
        }

        public void SendFileNotValidMessage(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, List<string> unexpectedColumnsList, List<string> notFoundColumnsList, List<string> duplicateColumnsList, System.IO.FileInfo file, bool isKnownCustomerFileName, bool isValidFileType, bool isFileSchemaValid, FrameworkUAD.Object.ValidationResult vr, string processCode)
        {
            try
            {
                int sourceFileID = 0;
                if (sourceFile != null)
                    int.TryParse(sourceFile.SourceFileID.ToString(), out sourceFileID);

                FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
                ConsoleMessage("EMAIL: File Processed Emailing Report.");
                //Used to send one email per request
                StringBuilder reportBody = GetReportBodyForSingleEmail(unexpectedColumnsList, notFoundColumnsList, duplicateColumnsList, file, isKnownCustomerFileName, isValidFileType, isFileSchemaValid, vr, processCode, sourceFile);
                string badDataAttachment = vrWorker.GetCustomerErrorMessage(vr, sourceFile);

                ConsoleMessage(reportBody.ToString());

                //Log in ImportError table
                FrameworkUAD.BusinessLogic.ImportError ieWorker = new FrameworkUAD.BusinessLogic.ImportError();
                FrameworkUAD.Entity.ImportError ie = new FrameworkUAD.Entity.ImportError(-1, processCode, sourceFileID, "Import Details", "", 0, reportBody.ToString(), badDataAttachment);
                ieWorker.Save(ie, client.ClientConnections);

                SendEmail(GetMailMessage(client, reportBody.ToString(), file, isValidFileType, isFileSchemaValid, "text/html", processCode, sourceFileID, badDataAttachment, MailPriority.High, false), client);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SendFileNotValidMessage");
            }
        }

        public void EmailError(string error, string processCode = "", int sourceFileID = 0)
        {
            MailMessage message = new MailMessage();
            string subject = "ADMS - ERROR";
            if (sourceFileID > 0)
                subject += " - SourceFileID: " + sourceFileID.ToString();
            if (!string.IsNullOrEmpty(processCode))
                subject += " - ProcessCode: " + processCode;
            message.Subject = subject;
            message.To.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = false;
            message.Body = error;
            message.Priority = MailPriority.High;
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void EmailException(Exception ex, string msg = "")
        {
            MailMessage message = new MailMessage();
            message.Subject = "ADMS - EXCEPTION";
            message.To.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = false;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(msg);
            sb.AppendLine(string.Empty);
            sb.AppendLine(Core_AMS.Utilities.StringFunctions.FormatException(ex));
            message.Body = sb.ToString();
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public MailMessage GetMailMessage(KMPlatform.Entity.Client client, string reportBody, FileInfo file, bool isValidFileType, bool isFileSchemaValid, string emailType, string processCode, int sourceFileID, string badDataAttachment = "", MailPriority mailPriority = MailPriority.Normal, bool createSummaryReports = false, bool isDataCompare = false)
        {
            MailMessage message = new MailMessage();
            try
            {
                message.Priority = mailPriority;

                if (isValidFileType == false || isFileSchemaValid == false)
                    message.Priority = MailPriority.High;

                KMPlatform.BusinessLogic.Service serviceWorker = new KMPlatform.BusinessLogic.Service();
                FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(sourceFileID);
                KMPlatform.Entity.Service service = null;
                bool isFieldUpdate = false;
                string serviceName = "Unified Audience Database";
                if (sf != null)
                {
                    service = serviceWorker.Select(sf.ServiceID);
                    if (service != null)
                        serviceName = " " + service.ServiceName;

                    FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    FrameworkUAD_Lookup.Entity.Code dbFileType = codeWorker.SelectCodeId(sf.DatabaseFileTypeId);
                    if (dbFileType != null && (FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbFileType.CodeName) == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update))
                        isFieldUpdate = true;

                }

                if (isDataCompare == false)
                    message.Subject = client.FtpFolder + ": " + file.Name + " - Valid File Type: " + isValidFileType.ToYesNoString() + " Valid File Schema: " + isFileSchemaValid.ToYesNoString();
                else
                    message.Subject = client.FtpFolder + ": " + file.Name + " - DATA COMPARE - Valid File Type: " + isValidFileType.ToYesNoString() + " Valid File Schema: " + isFileSchemaValid.ToYesNoString();
                if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                    message.To.Add(client.AccountManagerEmails);
                if (client.ClientEmails != null && !string.IsNullOrEmpty(client.ClientEmails))
                    message.To.Add(client.ClientEmails);
                message.CC.Add(Core.ADMS.Settings.AdminEmail);

                message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
                //Generic
                Message myMsgs = new Message();
                string htmlMsg = myMsgs.GetMessage(Message.MessageTag.ImportFileReport_HTML).Replace("%%CLIENT%%", client.FtpFolder).Replace("%%FILETYPE%%", serviceName);
                if (htmlMsg == null || string.IsNullOrEmpty(htmlMsg))
                    htmlMsg = string.Empty;

                var emailView = AlternateView.CreateAlternateViewFromString(htmlMsg, null, emailType);
                message.AlternateViews.Add(emailView);

                Attachment details = Attachment.CreateAttachmentFromString(reportBody, "Details.html");
                if(details != null)
                    message.Attachments.Add(details);

                if (!string.IsNullOrEmpty(badDataAttachment))
                {
                    message.Priority = MailPriority.High;
                    Attachment badData = Attachment.CreateAttachmentFromString(badDataAttachment, "BadData_" + file.Name);//application/excel text/plain text/csv
                    if(badData != null)
                        message.Attachments.Add(badData);
                }

                //LinkedResource logo = new LinkedResource("C:\\source\\ADMS\\Emailer\\Images\\KM Logo.png");
                //logo.ContentId = "companylogo";

                //htmlView.LinkedResources.Add(logo);

                //save the email message to the Server
                string path = ConfigurationManager.AppSettings["DetailPath"].ToString() + @"\" + client.FtpFolder + @"\";
                DateTime time = DateTime.Now;
                string format = "MMddyyyy_HH-mm-ss";
                string emailName = time.ToString(format) + "_" + sourceFileID.ToString() + "_" + file.Name.Replace(file.Extension, "") + ".eml";
                message.Save(path + emailName);

                Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                ff.CreateFile(path + time.ToString(format) + "_" + sourceFileID.ToString() + "_Details.html", reportBody);

                ff.CreateFile(path + time.ToString(format) + "_" + sourceFileID.ToString() + "_EmailBody.html", htmlMsg);

                if (!string.IsNullOrEmpty(badDataAttachment))
                {
                    string archive = time.ToString(format) + "_" + sourceFileID.ToString() + "_BadData_" + file.Name;
                    ff.CreateFile(path + archive, badDataAttachment);
                }

                if (createSummaryReports == true)
                {
                    ConsoleMessage("Attaching Summary Reports.");
                    FrameworkUAD.BusinessLogic.Reports rworker = new FrameworkUAD.BusinessLogic.Reports();
                    FrameworkUAD.BusinessLogic.ImportErrorSummary rSum = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                    message.Attachments.Add(new Attachment(rworker.CreateFileSummaryReport(sourceFileID, processCode, System.IO.Path.GetFileNameWithoutExtension(file.Name), client.FtpFolder, client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(rworker.CreatePubCodeSummaryReport(sourceFileID, processCode, System.IO.Path.GetFileNameWithoutExtension(file.Name), client.FtpFolder, client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(rSum.CreateDimensionErrorsSummaryReport(sourceFileID, processCode, client.FtpFolder, client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                }
                if (service != null && service.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase) && !(isFieldUpdate))
                {
                    ConsoleMessage("Attaching Circ Reports.");
                    FrameworkUAD.BusinessLogic.CircIntegration ci = new FrameworkUAD.BusinessLogic.CircIntegration();
                    message.Attachments.Add(new Attachment(ci.SelectCircImportSummaryOne(processCode, client.ClientConnections, client.FtpFolder, sourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(ci.SelectCircImportSummaryTwo(processCode, client.ClientConnections, client.FtpFolder, sourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".GetMailMessage");
            }
            return message;
        }
        public void SendEmail(MailMessage message, KMPlatform.Entity.Client client)
        {
            string random = Core_AMS.Utilities.StringFunctions.RandomAlphaNumericString(6);
            string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + client.FtpFolder + @"\" + random + @"\";
            List<Attachment> deleteAttachment = new List<Attachment>();

            foreach (Attachment a in message.Attachments)
            {
                if (a.ContentStream.Length > 10000000)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    byte[] buffer = new byte[a.ContentStream.Length];
                    a.ContentStream.Read(buffer, 0, buffer.Length);
                    FileStream file = new FileStream(path + a.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    file.Write(buffer, 0, buffer.Length);
                    file.Dispose();
                    deleteAttachment.Add(a);
                }
            }

            foreach (Attachment a in deleteAttachment)
                message.Attachments.Remove(a);

            //now go through and zip all the files
            Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();

            if (Directory.Exists(path))
            {
                foreach (var f in Directory.GetFiles(path))
                {
                    FileInfo info = new FileInfo(f);
                    FileInfo zippedFile = ff.CreateZipFile(info);

                    Attachment zipAttach = new Attachment(zippedFile.FullName);
                    message.Attachments.Add(zipAttach);
                }
            }

            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            smtp.SendAsync(message, userState);
        }

        [ExcludeFromCodeCoverage] // Excluding as function only writes to console
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            //String token = (string)e.UserState;

            //comment out on 8/23 Wagner - we will not have a console so why do this - do not see point in logging for now
            //if (e.Cancelled)
            //{
            //    ConsoleMessageStatic(string.Format("[{0}] Send canceled.", token));
            //}
            //if (e.Error != null)
            //{
            //    ConsoleMessageStatic(string.Format("[{0}] {1}", token, e.Error.ToString()));
            //}
            //else
            //{
            //    ConsoleMessageStatic("Message sent.");
            //}
        }

        public StringBuilder GetReportBodyForSingleEmail(List<string> unexpectedColumnsList, List<string> notFoundColumnsList, List<string> duplicateColumnsList, System.IO.FileInfo file, bool isKnownCustomerFileName, bool isValidFileType, bool isFileSchemaValid, FrameworkUAD.Object.ValidationResult vr, string processCode, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            string unexpectedColumns = unexpectedColumnsList != null && unexpectedColumnsList.Count > 0 ? string.Join(", ", unexpectedColumnsList) : null;
            string notFoundColumns = notFoundColumnsList != null && notFoundColumnsList.Count > 0 ? string.Join(", ", notFoundColumnsList) : null;
            string duplicateColumns = duplicateColumnsList != null && duplicateColumnsList.Count > 0 ? string.Join(", ", duplicateColumnsList) : null;
            StringBuilder clientReportToBeAppended = new StringBuilder();
            try
            {
                if (file != null && vr != null && vr.ImportFile != null)
                {
                    #region valid file processed
                    FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    clientReportToBeAppended.Append("<b>Results for file processed.</b><br/><br/>");
                    clientReportToBeAppended.Append("Processed File Name: " + file.Name + "<br/>");
                    clientReportToBeAppended.Append("Source File Name: " + sourceFile.FileName + "<br/>");
                    clientReportToBeAppended.Append("Source File Id: " + sourceFile.SourceFileID.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Registered: " + BooleanExtensions.ToYesNoString(isKnownCustomerFileName) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Type Valid: " + BooleanExtensions.ToYesNoString(isValidFileType) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Schema Valid: " + BooleanExtensions.ToYesNoString(isFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("Process Code: " + processCode.ToString() + "<br/><br/>");

                    //int profileIssues = eventMessage.ValidationResult.TransformedRowCount - eventMessage.ValidationResult.TotalRowCount;
                    int dimensionIssues = vr.DimensionImportErrorCount;
                    clientReportToBeAppended.AppendLine("<b>*** Import details for file ***</b><br/>");
                    clientReportToBeAppended.AppendLine("Total row count: " + vr.TotalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Original row count: " + vr.OriginalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Post transformations row count: " + vr.TransformedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Inserted to Transformed: " + vr.ImportedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Were errors encountered at the row level: " + vr.HasError.ToYesNoString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Record Error total: " + vr.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension Error total: " + vr.DimensionImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Excluded records: " + vr.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension issues: " + dimensionIssues.ToString() + "<br/><br/>");

                    bool custErrorsAdded = false;

                    if (isKnownCustomerFileName == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: File is unknown</b><br/><br/>");
                        if (vr.HasError == true)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (isValidFileType == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status:</b> Extension type could not be processed<br/><br/>");
                        if (vr.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (isFileSchemaValid == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: Exceptions found</b><br/>");
                        if (!string.IsNullOrEmpty(unexpectedColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Unexpected Columns Found: " + unexpectedColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(notFoundColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Columns Not Found: " + notFoundColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(duplicateColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Duplicate Columns: " + duplicateColumns + "</div><br/>");
                        if (vr.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (vr.HasError == true && custErrorsAdded == false)
                    {
                        clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(vr, sourceFile));
                        custErrorsAdded = true;
                    }
                    #endregion
                }
                else
                {
                    clientReportToBeAppended.Append("<b>An unrecognized error has occured.</b><br/><br/>");
                    clientReportToBeAppended.Append("<b>File Name:</b> " + file.Name + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>File Status:</b> " + BooleanExtensions.ToGoodBadString(isFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>Process Code:</b> " + processCode.ToString() + "<br/>");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".GetReportBodyForSingleEmail");
            }
            return clientReportToBeAppended;
        }
        public StringBuilder GetReportBodyForSingleEmail(FileProcessed eventMessage)
        {
            string unexpectedColumns = eventMessage.ValidationResult.UnexpectedColumns != null && eventMessage.ValidationResult.UnexpectedColumns.Count > 0 ? string.Join(", ", eventMessage.ValidationResult.UnexpectedColumns) : null;
            string notFoundColumns = eventMessage.ValidationResult.NotFoundColumns != null && eventMessage.ValidationResult.NotFoundColumns.Count > 0 ? string.Join(", ", eventMessage.ValidationResult.NotFoundColumns) : null;
            string duplicateColumns = eventMessage.ValidationResult.DuplicateColumns != null && eventMessage.ValidationResult.DuplicateColumns.Count > 0 ? string.Join(", ", eventMessage.ValidationResult.DuplicateColumns) : null;
            StringBuilder clientReportToBeAppended = new StringBuilder();
            try
            {
                if (eventMessage.ImportFile != null && eventMessage.ValidationResult != null)
                {
                    FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
                    #region valid file processed
                    clientReportToBeAppended.Append("<b>Results for file processed.</b><br/><br/>");
                    clientReportToBeAppended.Append("Processed File Name: " + eventMessage.ImportFile.Name + "<br/>");
                    clientReportToBeAppended.Append("Source File Name: " + eventMessage.SourceFile.FileName + "<br/>");
                    clientReportToBeAppended.Append("Source File Id: " + eventMessage.SourceFile.SourceFileID.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Registered: " + BooleanExtensions.ToYesNoString(eventMessage.IsKnownCustomerFileName) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Type Valid: " + BooleanExtensions.ToYesNoString(eventMessage.IsValidFileType) + "<br/>");
                    clientReportToBeAppended.AppendLine("Is File Schema Valid: " + BooleanExtensions.ToYesNoString(eventMessage.IsFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("Process Code: " + eventMessage.AdmsLog.ProcessCode.ToString() + "<br/><br/>");

                    //int profileIssues = eventMessage.ValidationResult.TransformedRowCount - eventMessage.ValidationResult.TotalRowCount;
                    int dimensionIssues = eventMessage.ValidationResult.DimensionImportErrorCount;
                    clientReportToBeAppended.AppendLine("<b>*** Import details for file ***</b><br/>");
                    clientReportToBeAppended.AppendLine("Total row count: " + eventMessage.ValidationResult.TotalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Original row count: " + eventMessage.ValidationResult.OriginalRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Post transformations row count: " + eventMessage.ValidationResult.TransformedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Original Duplicate Record count: " + eventMessage.ValidationResult.OriginalDuplicateRecordCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Transformed Duplicate Record count: " + eventMessage.ValidationResult.TransformedDuplicateRecordCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Inserted to Transformed: " + eventMessage.ValidationResult.ImportedRowCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Were errors encountered at the row level: " + eventMessage.ValidationResult.HasError.ToYesNoString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Record Error total: " + eventMessage.ValidationResult.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension Error total: " + eventMessage.ValidationResult.DimensionImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Excluded records: " + eventMessage.ValidationResult.RecordImportErrorCount.ToString() + "<br/>");
                    clientReportToBeAppended.AppendLine("Dimension issues: " + dimensionIssues.ToString() + "<br/><br/>");

                    bool custErrorsAdded = false;

                    if (eventMessage.IsKnownCustomerFileName == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: File is unknown</b><br/><br/>");
                        if (eventMessage.ValidationResult.HasError == true)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(eventMessage.ValidationResult, eventMessage.SourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (eventMessage.IsValidFileType == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status:</b> Extension type could not be processed<br/><br/>");
                        if (eventMessage.ValidationResult.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(eventMessage.ValidationResult, eventMessage.SourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (eventMessage.IsFileSchemaValid == false)
                    {
                        clientReportToBeAppended.Append("<b>File Status: Exceptions found</b><br/>");
                        if (!string.IsNullOrEmpty(unexpectedColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Unexpected Columns Found: " + unexpectedColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(notFoundColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Columns Not Found: " + notFoundColumns + "</div><br/>");
                        if (!string.IsNullOrEmpty(duplicateColumns))
                            clientReportToBeAppended.Append("<div style=\"margin-left:20px\">Duplicate Columns: " + duplicateColumns + "</div><br/>");
                        if (eventMessage.ValidationResult.HasError == true && custErrorsAdded == false)
                        {
                            clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(eventMessage.ValidationResult, eventMessage.SourceFile));
                            custErrorsAdded = true;
                        }
                    }

                    if (eventMessage.ValidationResult.HasError == true && custErrorsAdded == false)
                    {
                        clientReportToBeAppended.AppendLine(vrWorker.GetCustomerErrorMessage(eventMessage.ValidationResult, eventMessage.SourceFile));
                        custErrorsAdded = true;
                    }
                    #endregion
                }
                else
                {
                    clientReportToBeAppended.Append("<b>An unrecognized error has occured.</b><br/><br/>");
                    clientReportToBeAppended.Append("<b>File Name:</b> " + eventMessage.ImportFile.Name + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>File Status:</b> " + BooleanExtensions.ToGoodBadString(eventMessage.IsFileSchemaValid) + "<br/>");
                    clientReportToBeAppended.AppendLine("<b>Process Code:</b> " + eventMessage.AdmsLog.ProcessCode.ToString() + "<br/>");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".GetReportBodyForSingleEmail:FileProcessed");
            }
            return clientReportToBeAppended;
        }

        public void BackupReportBuilder(FileProcessed eventMessage)
        {
            FrameworkUAD.BusinessLogic.ValidationResult vrWorker = new FrameworkUAD.BusinessLogic.ValidationResult();
            StringBuilder reportBody = GetReportBodyForSingleEmail(eventMessage);
            string badDataAttachment = string.Empty;
            if (eventMessage.ValidationResult.RecordImportErrorCount > 0)
                badDataAttachment = vrWorker.GetBadData(eventMessage.ValidationResult, eventMessage.SourceFile);

            Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
            string path = ConfigurationManager.AppSettings["DetailPath"].ToString() + @"\" + eventMessage.Client.FtpFolder + @"\";
            //string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + eventMessage.Client.FtpFolder + @"\";
            string location = eventMessage.AdmsLog.ProcessCode.ToString() + "_Details.html";
            char[] invalidFileChars = System.IO.Path.GetInvalidFileNameChars();
            string cleanLocation = location;
            foreach (char v in invalidFileChars)
            {
                cleanLocation = cleanLocation.Replace(v, '_');
            }
            string detLoc = path + cleanLocation;
            ff.CreateFile(detLoc, reportBody.ToString());

            if (!string.IsNullOrEmpty(badDataAttachment))
            {
                string archive = eventMessage.AdmsLog.ProcessCode.ToString() + "_BadData_" + eventMessage.SourceFile.FileName + ".csv";
                string cleanArchive = archive;
                foreach (char v in invalidFileChars)
                {
                    cleanArchive = cleanArchive.Replace(v, '_');
                }
                ff.CreateFile(path + cleanArchive, badDataAttachment);
            }
        }

        public void SecondaryHandleFileProcessed(FileProcessed eventMessage)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.Priority = MailPriority.High;

                KMPlatform.BusinessLogic.Service serviceWorker = new KMPlatform.BusinessLogic.Service();
                KMPlatform.Entity.Service service = serviceWorker.Select(eventMessage.SourceFile.ServiceID);
                string serviceName = "Unified Audience Database";
                if (service != null)                
                    serviceName = " " + service.ServiceName;

                bool isFieldUpdate = false;
                FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                FrameworkUAD_Lookup.Entity.Code dbFileType = codeWorker.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
                if (dbFileType != null && (FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbFileType.CodeName) == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update))
                    isFieldUpdate = true;

                message.Subject = eventMessage.Client.FtpFolder + ": " + eventMessage.SourceFile.FileName + " - Valid File Type: " + eventMessage.IsValidFileType.ToYesNoString() + " Valid File Schema: " + eventMessage.IsFileSchemaValid.ToYesNoString();
                if (!string.IsNullOrEmpty(eventMessage.Client.AccountManagerEmails))
                    message.To.Add(eventMessage.Client.AccountManagerEmails);
                if (!string.IsNullOrEmpty(eventMessage.Client.ClientEmails))
                    message.To.Add(eventMessage.Client.ClientEmails);
                if (eventMessage.SourceFile.DatabaseFileTypeId > 0)
                {
                    FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data;
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    FrameworkUAD_Lookup.Entity.Code dbft = cWorker.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
                    dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);
                    if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms || dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form)
                    {
                        message.To.Add(Core.ADMS.Settings.WebFormEmail);
                    }
                }
                message.CC.Add(Core.ADMS.Settings.AdminEmail);

                message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
                //Generic
                Message myMsgs = new Message();
                string htmlMsg = myMsgs.GetMessage(Message.MessageTag.ImportFileReport_HTML).Replace("%%CLIENT%%", eventMessage.Client.FtpFolder).Replace("%%FILETYPE%%", serviceName);

                var emailView = AlternateView.CreateAlternateViewFromString(htmlMsg, null, "text/html");
                message.AlternateViews.Add(emailView);

                string path = ConfigurationManager.AppSettings["DetailPath"].ToString() + @"\" + eventMessage.Client.FtpFolder + @"\";
                string location = eventMessage.AdmsLog.ProcessCode.ToString() + "_Details.html";
                char[] invalidFileChars = System.IO.Path.GetInvalidFileNameChars();
                string cleanLocation = location;
                foreach (char v in invalidFileChars)
                {
                    cleanLocation = cleanLocation.Replace(v, '_');
                }
                string archive = eventMessage.AdmsLog.ProcessCode.ToString() + "_BadData_" + eventMessage.SourceFile.FileName + ".csv";
                string cleanArchive = archive;
                foreach (char v in invalidFileChars)
                {
                    cleanArchive = cleanArchive.Replace(v, '_');
                }

                if (System.IO.File.Exists(path + cleanLocation))
                {
                    Attachment details = new Attachment(path + cleanLocation);
                    message.Attachments.Add(details);
                }

                if (System.IO.File.Exists(path + cleanArchive))
                {
                    message.Priority = MailPriority.High;
                    Attachment badData = new Attachment(path + cleanArchive);// Attachment.CreateAttachmentFromString(badDataAttachment, "BadData_" + file.Name);//application/excel text/plain text/csv
                    message.Attachments.Add(badData);
                }

                //LinkedResource logo = new LinkedResource("C:\\source\\ADMS\\Emailer\\Images\\KM Logo.png");
                //logo.ContentId = "companylogo";

                //htmlView.LinkedResources.Add(logo);

                //save the email message to the Server                               
                bool createSummaryReports = true;
                if (createSummaryReports == true)
                {
                    ConsoleMessage("Attaching Summary Reports.");
                    FrameworkUAD.BusinessLogic.Reports rworker = new FrameworkUAD.BusinessLogic.Reports();
                    FrameworkUAD.BusinessLogic.ImportErrorSummary rSum = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                    message.Attachments.Add(new Attachment(rworker.CreateFileSummaryReport(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.FileName, eventMessage.Client.FtpFolder, eventMessage.Client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(rworker.CreatePubCodeSummaryReport(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.FileName, eventMessage.Client.FtpFolder, eventMessage.Client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(rSum.CreateDimensionErrorsSummaryReport(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.Client.FtpFolder, eventMessage.Client.ClientConnections, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                }
                if (service != null && service.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase) && !(isFieldUpdate))
                {
                    ConsoleMessage("Attaching Circ Reports.");
                    FrameworkUAD.BusinessLogic.CircIntegration ci = new FrameworkUAD.BusinessLogic.CircIntegration();
                    message.Attachments.Add(new Attachment(ci.SelectCircImportSummaryOne(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections, eventMessage.Client.FtpFolder, eventMessage.SourceFile.SourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                    message.Attachments.Add(new Attachment(ci.SelectCircImportSummaryTwo(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections, eventMessage.Client.FtpFolder, eventMessage.SourceFile.SourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));
                }

                SendEmail(message, eventMessage.Client);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SecondaryHandleFileProcessed");
            }
        }

        //need to move the email messages to database
        public void DataCompareComplete(KMPlatform.Entity.Client client, string sourceFileName, string emailList, string summaryFileFullPath)
        {
            StringBuilder sbDetail = new StringBuilder();
            //build our email and send to Derek's team.
            sbDetail.AppendLine("<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "The Data Compare for file <b>" + sourceFileName + "</b> has completed.  " +
                          "You may now use the AMS application to review the results and download files. For your convenience we have attached the summary file.<br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>");

            MailMessage message = new MailMessage();
            try
            {
                Attachment summaryAttach = new Attachment(summaryFileFullPath);
                message.Attachments.Add(summaryAttach);
            }
            catch(Exception ex) 
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".DataCompareComplete:FileProcessed");
            }

            message.Subject = client.FtpFolder + ": Data Compare complete for " + sourceFileName;
            if(!string.IsNullOrEmpty(emailList))
                message.To.Add(emailList);
            message.Bcc.Add(Core.ADMS.Settings.AdminEmail);
            message.Bcc.Add(client.AccountManagerEmails);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = sbDetail.ToString();
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.Send(message, userState);

        }
        public void DataCompareEstimatedCompletionTime(KMPlatform.Entity.Client client, string sourceFileName, string emailList, int matchProfile, int matchDemo, int likeProfile, int likeDemo, int noData, int summary)
        {
            int totalRecords = matchProfile + matchDemo + likeProfile + likeDemo + noData + summary;
            int completionMinutes = 1;
            if (totalRecords > 84000)
                completionMinutes = (totalRecords / 84000) + 1;

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br/><br/>" +
                         "The estimated Data Compare completion time for file <b>" + sourceFileName + "</b> will be " + completionMinutes.ToString() + " minutes.<br/><br/>" +
                         "Matching Profile record count: " + matchProfile.ToString() + "<br/>" +
                         "Matching Demographic record count: " + matchDemo.ToString() + "<br/>" +
                         "Like Profile record count: " + likeProfile.ToString() + "<br/>" +
                         "Like Demographic record count: " + likeDemo.ToString() + "<br/>" +
                         "No Data record count: " + noData.ToString() + "<br/><br/>" +
                         "Sincerely,<br/>" +
                         "-Knowledge Marketing<br/><br/><br/>" +
                      "</td>" +
                  "</tr>" +
              "</table>");

            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": Data Compare estimated completion time " + sourceFileName;
            if (!string.IsNullOrEmpty(emailList))
                message.To.Add(emailList);
            message.Bcc.Add(Core.ADMS.Settings.AdminEmail);
            message.Bcc.Add(client.AccountManagerEmails);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = sbDetail.ToString();
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.Send(message, userState);
        }
        public void RejectFileErrorLimitExceeded(FrameworkUAD.Object.ImportFile dataIV)
        {
            KMPlatform.BusinessLogic.Client cworker = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client client = cworker.Select(dataIV.ClientId);

            FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(dataIV.SourceFileId);
            
            StringBuilder sbDetail = new StringBuilder();
            //build our email and send to Derek's team.
            sbDetail.AppendLine("<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "The file " + dataIV.ProcessFile.Name + " has reached the error limit threshold of 100,000 errors or 25% of the records and therefore has been rejected.  " +
                          "Please correct the errors in your file and reupload for processing. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>");

            sbDetail.AppendLine("<br/><br/>");
            sbDetail.AppendLine("<b>File processed.</b><br/>");
            sbDetail.AppendLine("Processed File Name: " + dataIV.ProcessFile.Name + "<br/>");
            sbDetail.AppendLine("Source File Name: " + sf.FileName + "<br/>");
            sbDetail.AppendLine("Source File Id: " + sf.SourceFileID.ToString() + "<br/>");
            sbDetail.AppendLine("Process Code: " + dataIV.ProcessCode.ToString() + "<br/><br/>");
            sbDetail.AppendLine(System.Environment.NewLine);
            sbDetail.AppendLine("<b>*** Import details for file ***</b><br/>");
            sbDetail.AppendLine("Total row count: " + dataIV.TotalRowCount.ToString() + "<br/>");
            sbDetail.AppendLine("Original row count: " + dataIV.OriginalRowCount.ToString() + "<br/>");
            sbDetail.AppendLine("Post transformations row count: " + dataIV.TransformedRowCount.ToString() + "<br/>");
            sbDetail.AppendLine("Record Error total: " + dataIV.ImportErrorCount.ToString() + "<br/>");
            sbDetail.AppendLine("<br/><br/>");

            foreach (FrameworkUAD.Entity.ImportError error in dataIV.ImportErrors)
            {
                if (error.RowNumber > 0)
                    sbDetail.AppendLine("<b>Row Number:</b> " + error.RowNumber.ToString() + "<br/>");
                if (error.ClientMessage != null && error.ClientMessage.Length > 0)
                    sbDetail.AppendLine(error.ClientMessage + "<br/>");
                if (error.BadDataRow != null && error.BadDataRow.Length > 0)
                {
                    //sbDetail.AppendLine("<b>Data:</b> " + string.Join(",", error.BadDataRow) + "<br/>");
                    error.BadDataRow = error.BadDataRow.Trim().TrimEnd(',');
                    if (sf.IsTextQualifier == true)
                    {
                        StringBuilder data = new StringBuilder();
                        string[] quoteRow = error.BadDataRow.Split(',');
                        foreach (string x in quoteRow)
                            data.AppendLine("\"" + x + "\",");
                        sbDetail.AppendLine("<b>Data:</b> " + data.ToString().Trim().TrimEnd(','));
                        sbDetail.AppendLine(System.Environment.NewLine);
                    }
                    else
                    {
                        sbDetail.AppendLine(error.BadDataRow.Trim().TrimEnd(','));
                        sbDetail.AppendLine(System.Environment.NewLine);
                    }
                }
                sbDetail.AppendLine(System.Environment.NewLine);
                sbDetail.AppendLine(System.Environment.NewLine);
                sbDetail.AppendLine("<br/>");
                sbDetail.AppendLine("<br/>");
            }

            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + dataIV.ProcessFile.Name + " - Status: Rejected - Reason: Error Threshold Exceeded";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = sbDetail.ToString();
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.Send(message, userState);
        }
        public void EmailRejectCircFileLockedProducts(KMPlatform.Entity.Client client, FileInfo file, List<string> product)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + file.Name + " - Status: Rejected";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "Knowledge Marketing has analyzed your recent circulation file submission. " +
                         "For your convenience, we have determined the following results from the analysis.  <br /><br />" +
                         "File Rejected Product(s) Locked: (" + string.Join(",", product) + ").<br /><br />" +
                         "The products listed above are locked and imports cannot currently process.  Please unlock the product and resubmit " +
                         "the exception file(s) to the KM FTP site for processing. <br /><br />" +
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative " +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void EmailRejectCircFileTelemarketingMultipleProducts(KMPlatform.Entity.Client client, FileInfo file)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + file.Name + " - Status: Rejected";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "Knowledge Marketing has analyzed your recent circulation file submission. " +
                         "For your convenience, we have determined the following results from the analysis.  <br /><br />" +
                         "File Rejected Telemarketing Contained Multiple Products.<br /><br />" +
                         "The file listed above cannot process as it contains data for multiple products.  Please make the necessary changes to " +
                         "your input file and resubmit the exception file(s) to the KM FTP site for processing. <br /><br />" +
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative" +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void FileNotDefined(KMPlatform.Entity.Client client, FileInfo file)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + file.Name + " - Status: Rejected - Reason: File Not Defined";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "The file " + file.Name + " has been uploaded to the ADMS ftp site for processing but the file is not defined and therefore deleted and rejected.  " +
                          "Please map your file in AMS and reupload for processing. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void RejectFile(KMPlatform.Entity.Client client, FileInfo file, string rejectReason)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + file.Name + " - Status: Rejected";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "The file " + file.Name + " has been uploaded to the ADMS ftp site for processing but has been rejected due to the following reason:<br/><br/>" +
                         rejectReason + "<br/><br/>" +
                         "Sincerely,<br/>" +
                         "-Knowledge Marketing<br/><br/><br/>" +
                      "</td>" +
                  "</tr>" +
              "</table>";
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void EmailFieldUpdateIssues(KMPlatform.Entity.Client client, FileInfo file, string processCode, string badData)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + file.Name + " - Records Not Updated";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "Knowledge Marketing has analyzed your recent circulation file submission. " +
                         "For your convenience, we have determined the following results from the analysis.  <br /><br />" +
                         "The attached records were not processed for one of the following reasons. Please correct the data and reimport, or fix manually.<br />" +
                         "- Paid record<br />" +
                         "- Missing SequenceID<br /><br />" +                         
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative " +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";

            Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
            string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + client.FtpFolder + @"\";
            string location = processCode.ToString() + "_FieldUpdate.html";
            char[] invalidFileChars = System.IO.Path.GetInvalidFileNameChars();

            if (!string.IsNullOrEmpty(badData))
            {
                string archive = processCode.ToString() + "_FieldUpdate.csv";
                string cleanArchive = archive;
                foreach (char v in invalidFileChars)
                {
                    cleanArchive = cleanArchive.Replace(v, '_');
                }
                ff.CreateFile(path + cleanArchive, badData);

                Attachment badDataFile = new Attachment(path + cleanArchive);
                message.Attachments.Add(badDataFile);
            }

            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void EmailFieldUpdateIssues(KMPlatform.Entity.Client client, string fileName, string processCode, string badData)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + fileName + " - Records Not Updated";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "Knowledge Marketing has analyzed your recent circulation file submission. " +
                         "For your convenience, we have determined the following results from the analysis.  <br /><br />" +
                         "The attached records were not processed for one of the following reasons. Please correct the data and reimport, or fix manually.<br />" +
                         "- Paid record<br />" +
                         "- Missing SequenceID<br /><br />" +  
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative " +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";

            Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
            string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + client.FtpFolder + @"\";
            string location = processCode.ToString() + "_FieldUpdate.html";
            char[] invalidFileChars = System.IO.Path.GetInvalidFileNameChars();

            if (!string.IsNullOrEmpty(badData))
            {
                string archive = processCode.ToString() + "_FieldUpdate.csv";
                string cleanArchive = archive;
                foreach (char v in invalidFileChars)
                {
                    cleanArchive = cleanArchive.Replace(v, '_');
                }
                ff.CreateFile(path + cleanArchive, badData);

                Attachment badDataFile = new Attachment(path + cleanArchive);
                message.Attachments.Add(badDataFile);
            }

            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        public void SubGenAddressesNotAbleToUpdate(List<FrameworkUAD.Entity.ProductSubscription> list, KMPlatform.Entity.Client client)
        {
            MailMessage message = new MailMessage();
            #region attach list to email - compress as zip file if needed
            try
            {
                string xml = Core_AMS.Utilities.XmlFunctions.ToXML<List<FrameworkUAD.Entity.ProductSubscription>>(list);
                Attachment details = Attachment.CreateAttachmentFromString(xml, "Addresses_Not_Updated_in_SubGen.xml");
                if (details != null)
                    message.Attachments.Add(details);

                string random = Core_AMS.Utilities.StringFunctions.RandomAlphaNumericString(6);
                string path = Core.ADMS.BaseDirs.getClientFileResultEmail() + @"\" + client.FtpFolder + @"\" + random + @"\";
                List<Attachment> deleteAttachment = new List<Attachment>();

                foreach (Attachment a in message.Attachments)
                {
                    if (a.ContentStream.Length > 10000000)
                    {
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        byte[] buffer = new byte[a.ContentStream.Length];
                        a.ContentStream.Read(buffer, 0, buffer.Length);
                        FileStream file = new FileStream(path + a.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        file.Write(buffer, 0, buffer.Length);
                        file.Dispose();
                        deleteAttachment.Add(a);
                    }
                }

                foreach (Attachment a in deleteAttachment)
                    message.Attachments.Remove(a);

                //now go through and zip all the files
                Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();

                if (Directory.Exists(path))
                {
                    foreach (var f in Directory.GetFiles(path))
                    {
                        FileInfo info = new FileInfo(f);
                        FileInfo zippedFile = ff.CreateZipFile(info);

                        Attachment zipAttach = new Attachment(zippedFile.FullName);
                        message.Attachments.Add(zipAttach);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SubGenAddressesNotAbleToUpdate");
            }
            #endregion

            message.Subject = client.FtpFolder + ": Addresses not able to update";
            message.Bcc.Add(Core.ADMS.Settings.AdminEmail);
            message.To.Add(client.AccountManagerEmails);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "The attached file contains addresses which were not able to be automatically updated. " +
                         "Please update each address manually.  <br /><br />" +
                         
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative " +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.Send(message, userState);
        }
        public void ACSFileComplete(KMPlatform.Entity.Client client, int sourceFileID, string fileName, string processCode)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + fileName + " - ACS File Processed";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "Knowledge Marketing has analyzed your recent circulation file submission. " +
                         "For your convenience, we have determined the following results from the analysis.  <br /><br />" +                          
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative " +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";

            ConsoleMessage("Attaching Circ Reports.");
            FrameworkUAD.BusinessLogic.CircIntegration ci = new FrameworkUAD.BusinessLogic.CircIntegration();
            message.Attachments.Add(new Attachment(ci.SelectCircACSSummary(processCode, client.ClientConnections, client.FtpFolder, sourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));

            //string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(CustomSendCompletedCallback);
            var userState = message;
            smtp.SendAsync(message, userState);            
            ConsoleMessage("ACS File Complete Message sent.");
        }
        public void NCOAFileComplete(KMPlatform.Entity.Client client, int sourceFileID, string fileName, string processCode)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + fileName + " - NCOA File Processed";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "Knowledge Marketing has analyzed your recent circulation file submission. " +                         
                         "If you feel this message has reached you in error, or if you did not submit files for processing, please contact your account representative " +
                         "immediately at 763.746.2780. Unfortunately, this email is a system generated notification which is unable to receive replies. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";

            //ConsoleMessage("Attaching Circ Reports.", processCode, 0, 0);
            //FrameworkUAD.BusinessLogic.CircIntegration ci = new FrameworkUAD.BusinessLogic.CircIntegration();
            //message.Attachments.Add(new Attachment(ci.SelectCircACSSummary(processCode, client.ClientConnections, client.FtpFolder, sourceFileID, Core.ADMS.BaseDirs.getClientArchiveDir().ToString())));

            //string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(CustomSendCompletedCallback);
            var userState = message;
            smtp.SendAsync(message, userState);
        }
        public void NotifyDashboardReportsGenerated(KMPlatform.Entity.Client client, FileInfo file)
        {
            MailMessage message = new MailMessage();
            message.Subject = client.FtpFolder + ": " + file.Name + " - Status: Dashboard file reports ready";
            if (!string.IsNullOrEmpty(client.AccountManagerEmails))
                message.To.Add(client.AccountManagerEmails);
            message.CC.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = true;
            message.Body = "<table border='0' cellpadding='0' cellspacing='0' style=\"padding: 5px; width: 700px;border: 1px solid #CCCCCC\">" +
                "<tr>" +
                      "<td style=\"font-family: Arial; font-size: 8pt;\">" +
                         "Dear " + client.FtpFolder + " <br /><br />" +
                         "The file " + file.Name + " has completed ADMS processing and Dashboard reports are now ready for download. <br /><br />" +
                         "Sincerely,<br />" +
                         "-Knowledge Marketing<br /><br /><br />" +
                      "</td>" +
                  "</tr>" +
              "</table>";
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtp.SendAsync(message, userState);
        }
        private static void CustomSendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage msg = (MailMessage)e.UserState;
            if (msg != null && msg.Attachments != null && msg.Attachments.Count > 0)
                msg.Attachments.Dispose();

            if (msg != null)
                msg.Dispose();
            //String token = (string)e.UserState;
            //if (e.Cancelled)
            //{
            //    Console.WriteLine("Send cancelled.");
            //}
            //if (e.Error != null)
            //{
            //    Console.WriteLine("An error occurred.");
            //}
            //else
            //{
            //    Console.WriteLine("Message sent.");
            //}
        }
    }
}
