using System;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using aspNetIMAP;
using aspNetMime;
using KM.Common;
using KM.Common.Entity;
using BusinessEmailActivityLog = ECN_Framework_BusinessLayer.Communicator.EmailActivityLog;


namespace ecn.engines.FBL
{
    class FeedbackEngine
    {
        public static string EmailMessage;        
        public static IMAP4 imap = new IMAP4();
        public static string XMLEmailList = string.Empty;
        private const string seenFlagOfMessageClient = "\\seen";
        private const string EmailId = "emailID";
        private const string BlastId = "blastID";
        private static string _CurrentESP;

        [STAThread]
        static void Main(string[] args)
        {
            string[] espList = GetESPs();

            for (int i = 0; i <= espList.GetUpperBound(0); i++)
            {
                _CurrentESP = espList[i].ToString();

                
                Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                EmailMessage = string.Empty;                
                XMLEmailList = string.Empty;

                FileFunctions.LogConsoleActivity("____________________________________________________________________________________________", _CurrentESP);
                FileFunctions.LogConsoleActivity(String.Format("Start Date/Time : {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ssss")), _CurrentESP);

                try
                {
                    // Connect and Login to IMAP a/c.
                    ConnectToImapAccount();

                    // Process all Messages in the INBOX
                    ProcessMessages();

                    //Disconnect IMAP account
                    if (imap.IsConnected)
                    {
                        FileFunctions.LogConsoleActivity("Disconnecting IMAP Mailbox", _CurrentESP);
                        imap.Disconnect();
                        imap.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "FBLEngine.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                    FileFunctions.LogConsoleActivity("Exception when Processing Email messages: <BR><BR>", _CurrentESP);
                }
                finally
                {                    
                    RemoveSpamedEmail();

                    if (EmailMessage != string.Empty)
                        SendEmail(EmailMessage);
                    FileFunctions.LogConsoleActivity("Closing Log file", _CurrentESP);
                    FileFunctions.LogConsoleActivity("____________________________________________________________________________________________", _CurrentESP);
                }
                
            }
        }

        private static void SendEmail(string msg)
        {
            MailMessage message = new MailMessage(GetEmailFrom(), GetEmailTo());
            message.Subject = GetEmailSubject();
            message.IsBodyHtml = true;
            message.Body = msg;
            message.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient(GetSMTPServer());
            client.Send(message);
        }

        private static void ConnectToImapAccount()
        {
            bool bConnected = false;
            int iTriedConnection = 3;            
            imap = new IMAP4(ConfigurationManager.AppSettings[_CurrentESP + "_MailServer"]);
            imap.Username = ConfigurationManager.AppSettings[_CurrentESP + "_MailUser"];
            imap.Password = ConfigurationManager.AppSettings[_CurrentESP + "_MailPassword"];
            imap.Port = 993;
            while (!bConnected && iTriedConnection > 0)
            {
                try
                {
                    iTriedConnection--;
                    imap.Connect();
                    imap.Login();
                    MailFolder inbox = imap.SelectInbox();
                    bConnected = true;
                    break;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(2000);
                    FileFunctions.LogConsoleActivity("Error which connecting to IMAP : " + ex.Message, _CurrentESP);
                    FileFunctions.LogConsoleActivity("......" + iTriedConnection + " more tries ", _CurrentESP);
                }
            }
            if (!bConnected)
            {                
                throw new Exception("Cannot connect to IMAP account - check log file");
            }

        }

        private static void ProcessMessages()
        {
            var inbox = imap.SelectInbox();
            var mc = inbox.MessageClient;

            var messageNumber = 1;
            var count = inbox.MessageCount;

            FileFunctions.LogConsoleActivity(
                $"There are {count} messages waiting. {DateTime.Now.ToString(CultureInfo.CurrentCulture)}",
                _CurrentESP);
            FileFunctions.LogConsoleActivity(" ", _CurrentESP);

            for (var i = 1; i <= count; i++)
            {
                ProcessMessage(i, mc, ref messageNumber, inbox);
            }
        }

        private static void ProcessMessage(int i, MessageClient mc, ref int messageNumber, MailFolder inbox)
        {
            try
            {
                FileFunctions.LogConsoleActivity(
                    $"======================Processing Message {i} - {DateTime.Now:MM/dd/yyyy hh:mm:ssss}===============================",
                    _CurrentESP);

                var uniqueId = mc.ToUniqueId(messageNumber);
                var isInternetRetailer = false;
                var emailId = 0;
                var blastId = 0;
                var msg = inbox.FetchClient.Message(uniqueId, IndexType.UniqueId, true);
                var ip = GetIpFromMessage(msg);
                if (getHeader(msg.Headers, GetHeaderCheckField()).IndexOf(GetHeaderCheckValue(), StringComparison.Ordinal) >= 0)
                {
                    if (ValidateAttachments(i, msg, ref emailId, ref blastId, ref isInternetRetailer))
                    {
                        ProcessValidMessage(mc, ip, emailId, blastId, uniqueId);
                    }
                    else
                    {
                        ProcessBadMessage(mc, isInternetRetailer, uniqueId);
                    }

                    imap.Expunge();
                }
                else
                {
                    mc.Move(uniqueId, "Other Mails", IndexType.UniqueId);
                    imap.Expunge();
                }

                FileFunctions.LogConsoleActivity(
                    $"======================End Message {i}===============================",
                    _CurrentESP);
                FileFunctions.LogConsoleActivity(" ", _CurrentESP);

                if (i % 100 == 0)
                {
                    RemoveSpamedEmail();
                }
            }
            catch (IMAPConnectionException exc)
            {
                throw;
            }
            catch (Exception ex)
            {
                ApplicationLog.LogNonCriticalError(ex.ToString(),
                    "FBLEngine.ProcessMessages.Outer",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                FileFunctions.LogConsoleActivity("Exception when Processing Email messages: ", _CurrentESP);
                messageNumber++;
            }
        }

        private static void ProcessBadMessage(MessageClient mc, bool isInternetRetailer, int uniqueId)
        {
            if (isInternetRetailer)
            {
                FileFunctions.LogConsoleActivity("Internet Retailer Message", _CurrentESP);
                mc.UnMark(uniqueId, seenFlagOfMessageClient, IndexType.UniqueId);
                mc.Move(uniqueId, "InternetRetailer", IndexType.UniqueId);
                FileFunctions.LogConsoleActivity("Message Moved to 'InternetRetailer' folder", _CurrentESP);
            }
            else
            {
                FileFunctions.LogConsoleActivity("Invalid EmailID or BlastID", _CurrentESP);

                mc.UnMark(uniqueId, seenFlagOfMessageClient, IndexType.UniqueId);
                mc.Move(uniqueId, "Failed Mails", IndexType.UniqueId);
                FileFunctions.LogConsoleActivity("Message Moved to 'Failed Mails' folder", _CurrentESP);
            }
        }

        private static void ProcessValidMessage(MessageClient mc, string ip, int emailId, int blastId, int uniqueId)
        {
            if (ip.Length > 0)
            {
                ip += " - ";
            }

            XMLEmailList +=
                $"<BOUNCE EmailID=\"{emailId}\" BlastID=\"{blastId}\" Comments=\"{ip + _CurrentESP + " Feedback"}\" SubscribeTypeCode=\"F\" />";
            FileFunctions.LogConsoleActivity($"EmailID : {emailId}/ BlastID : {blastId}/ IP : {ip}", _CurrentESP);

            mc.UnMark(uniqueId, seenFlagOfMessageClient, IndexType.UniqueId);
            mc.Delete(uniqueId, IndexType.UniqueId);
            FileFunctions.LogConsoleActivity("Processed message deleted", _CurrentESP);
        }

        private static bool ValidateAttachments(
            int i,
            MimeMessage msg,
            ref int emailId,
            ref int blastId,
            ref bool isInternetRetailer)
        {
            var isValidMessage = false;
            foreach (MimePart mp in msg.AttachmentInLineParts())
            {
                if (!ValidateSingleAttachment(i, msg, out emailId, out blastId, out isInternetRetailer, mp))
                {
                    break;
                }
            }

            return isValidMessage;
        }

        private static bool ValidateSingleAttachment(
            int i,
            MimePart msg,
            out int emailId,
            out int blastId,
            out bool isInternetRetailer,
            MimePart mp)
        {
            emailId = -1;
            blastId = -1;
            isInternetRetailer = false;

            if (!mp.IsInline() && !mp.IsMessage())
            {
                return true;
            }

            var data = mp.Data();

            var attachedMsg = MimeMessage.ParseBinary(data);
            try
            {
                var returnPath = getHeader(attachedMsg.Headers, "Return-Path");
                var messageId = getHeader(attachedMsg.Headers, "Message-ID");
                var xSender = getHeader(attachedMsg.Headers, "X-Sender");

                if (returnPath.Trim().Length > 0 ||
                    messageId.Trim().Length > 0 ||
                    xSender.Trim().Length > 0)
                {
                    //Check the return path for EmailID and BlastID
                    emailId = FindID(EmailId, returnPath);
                    blastId = FindID(BlastId, returnPath);

                    if (emailId > 0 && blastId > 0)
                    {
                        return false;
                    }

                    //Check the message id for EmailID and BlastID
                    emailId = FindIDByMessageID(EmailId, messageId);
                    blastId = FindIDByMessageID(BlastId, messageId);
                    if (emailId > 0 && blastId > 0)
                    {
                        return false;
                    }

                    //Check the x - sender for EmailID and BlastID
                    emailId = FindID(EmailId, xSender);
                    blastId = FindID(BlastId, xSender);
                    if (emailId > 0 && blastId > 0)
                    {
                        return false;
                    }

                    if (_CurrentESP == "YAHOO")
                    {
                        var sender = getHeader(attachedMsg.Headers, "Sender");
                        if (sender.IndexOf("internetretailer.com", StringComparison.Ordinal) >= 0)
                        {
                            isInternetRetailer = true;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.LogNonCriticalError(ex.ToString(),
                    "FBLEngine.ProcessMessages.Inner",
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                var sbErrorMessage = new StringBuilder();
                sbErrorMessage.Append($"Processing Message {i}===============================<BR>");
                sbErrorMessage.Append($"<B>Message Header:</B><br> {getHeaders(msg.Headers)}<br>");
                sbErrorMessage.Append($"<B>Attachment Header:</B><br> {getHeaders(attachedMsg.Headers)}<br>");
                EmailMessage += sbErrorMessage.ToString();
                throw;
            }

            return true;
        }

        private static string GetIpFromMessage(MimeMessage msg)
        {
            const string pattern = @"\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?";

            var subject = msg.Subject.ToString();
            var regex = new Regex(pattern);
            var match = regex.Match(subject);
            return match.Success ? match.Value : string.Empty;
        }

        private static void RemoveSpamedEmail()
        {
            try
            {
                if (XMLEmailList.Length > 0)
                {
                    BusinessEmailActivityLog.InsertSpamFeedbackXML($"<ROOT>{XMLEmailList}</ROOT>", "FEEDBACK_UNSUB");
                    FileFunctions.LogConsoleActivity(
                        $"Updating Database : exec sp_update_Spam_Feedback_XML '<ROOT>{XMLEmailList}</ROOT>', 'FEEDBACK_UNSUB'",
                        _CurrentESP);
                }
                else
                {
                    FileFunctions.LogConsoleActivity("No Data Found! - Empty ebID's ", _CurrentESP);
                }
                XMLEmailList = string.Empty;
            }

            catch (Exception ex)
            {
                EmailMessage += $"<p><font size='2' color='red'><b>Exception when updating database : </b><BR>{ex.Message}</font><br></p>";
                FileFunctions.LogConsoleActivity($"Exception when updating database: {ex.Message}", _CurrentESP);
                ApplicationLog.LogNonCriticalError(
                    ex.ToString(),
                    "FBLEngine.RemoveSpamedEmail", 
                    Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
            }
        }

        private static string getHeaders(HeaderCollection hc)
        {
            string headers = "";

            foreach (Header h in hc)
            {
                headers += "<B>" + h.Name + "</B>: " + h.Value + "<BR>";
            }
            return headers;
        }

        private static string getHeader(HeaderCollection hc, string headername)
        {
            string sheader = "";

            foreach (Header h in hc)
            {
                if (h.Name.ToLower().Equals(headername.ToLower()))
                {
                    sheader = h.Value.ToLower().ToString();
                }
            }
            return sheader;
        }

        private static int FindIDByMessageID(string wants, string messageID)
        {
            int id = 0;
            if (messageID != string.Empty)
            {
                try
                {
                    messageID = messageID.Replace("<", "");
                    messageID = messageID.Substring(0, messageID.IndexOf("x"));

                    char[] splitter1 = { '.' };
                    string[] email_and_blast = messageID.Split(splitter1);

                    if (wants.Equals(EmailId))
                        id = Convert.ToInt32(email_and_blast[1]);
                    else
                        id = Convert.ToInt32(email_and_blast[0]);
                }
                catch (Exception ex) 
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex.ToString(), "FBLEngine.FindIDByMessageID", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
            }
            return id;
        }


        private static int FindID(string wants, string returnpath)
        {
            int id = 0;
            if (returnpath != string.Empty)
            {
                try
                {                    
                    returnpath = returnpath.Replace("<", "");
                    returnpath = returnpath.Replace("bounce_", "");
                    returnpath = returnpath.Substring(0, returnpath.IndexOf("@"));

                    char[] splitter1 = { '-' };
                    string[] email_and_blast = returnpath.Split(splitter1);

                    if (wants.Equals(EmailId))
                        id = Convert.ToInt32(email_and_blast[0]);
                    else
                        id = Convert.ToInt32(email_and_blast[1]);
                }
                catch (Exception ex) 
                {
                    KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex.ToString(), "FBLEngine.FindID", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                }
            }
            return id;
        }

        private static string GetSMTPServer()
        {
            string smtpServer = string.Empty;

            if (ConfigurationManager.AppSettings["SMTPServer"] != null)
                smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();

            return smtpServer;
        }

        private static string GetEmailTo()
        {
            string emailTo = string.Empty;

            if (ConfigurationManager.AppSettings["EmailTo"] != null)
                emailTo = ConfigurationManager.AppSettings["EmailTo"].ToString();

            return emailTo;
        }

        private static string GetEmailFrom()
        {
            string emailFrom = string.Empty;

            if (ConfigurationManager.AppSettings["EmailFrom"] != null)
                emailFrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();

            return emailFrom;
        }

        private static string GetEmailSubject()
        {
            string emailSubject = string.Empty;

            if (ConfigurationManager.AppSettings[_CurrentESP + "_EmailSubject"] != null)
                emailSubject = ConfigurationManager.AppSettings[_CurrentESP + "_EmailSubject"].ToString();

            return emailSubject;
        }

        private static string GetMailServer()
        {
            string mailServer = string.Empty;

            if (ConfigurationManager.AppSettings[_CurrentESP + "_MailServer"] != null)
                mailServer = ConfigurationManager.AppSettings[_CurrentESP + "_MailServer"].ToString();

            return mailServer;
        }

        private static string GetMailUser()
        {
            string mailUser = string.Empty;

            if (ConfigurationManager.AppSettings[_CurrentESP + "_MailUser"] != null)
                mailUser = ConfigurationManager.AppSettings[_CurrentESP + "_MailUser"].ToString();

            return mailUser;
        }

        private static string GetMailPassword()
        {
            string mailPassword = string.Empty;

            if (ConfigurationManager.AppSettings[_CurrentESP + "_MailPassword"] != null)
                mailPassword = ConfigurationManager.AppSettings[_CurrentESP + "_MailPassword"].ToString();

            return mailPassword;
        }

        private static string GetHeaderCheckField()
        {
            string checkField = string.Empty;

            if (ConfigurationManager.AppSettings[_CurrentESP + "_HeaderCheckField"] != null)
                checkField = ConfigurationManager.AppSettings[_CurrentESP + "_HeaderCheckField"].ToString();

            return checkField;
        }

        private static string GetHeaderCheckValue()
        {
            string checkValue = string.Empty;

            if (ConfigurationManager.AppSettings[_CurrentESP + "_HeaderCheckValue"] != null)
                checkValue = ConfigurationManager.AppSettings[_CurrentESP + "_HeaderCheckValue"].ToString();

            return checkValue;
        }

        private static string[] GetESPs()
        {
            string espList = string.Empty;
            string[] esps = null;

            if (ConfigurationManager.AppSettings["ESPs"] != null)
                espList = ConfigurationManager.AppSettings["ESPs"].ToString();
            if (espList.Length > 0)
            {
                char[] splitter1 = { ';' };
                esps = espList.Split(splitter1);
            }

            return esps;
        }
    }
}
