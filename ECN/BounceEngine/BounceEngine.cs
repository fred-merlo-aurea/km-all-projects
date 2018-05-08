using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using aspNetMime;
using aspNetPOP3;
using BounceEngine;
using ECN.Communicator.Bounces;
using KM.Common;
using KM.Common.Entity;
using ListNanny;

namespace ECN.engines
{
    /// <summary>
    /// Summary description for BounceEngine.
    /// </summary>
    class BounceEngine
    {
        private const string NotificationValues = "NotificationValues";
        private const char CommaSeparator = ',';
        private const string OutLog = "OutLog";
        private const string Slash = @"/";
        private const string DisconnectingPop3Mailbox = "Disconnecting POP3 Mailbox";
        private const string ProcessingBounceRecords = "->Processing Bounce records.";
        private const string ProcessingMasterSuppressionRecords = "->Processing Master Suppression records.";
        private const string LineOfDashes = "---------------------------------";
        private const string BouncesInBags = "Bounces in bags: ";
        private const string Inserted = "Inserted: ";
        private const string NonBounces = "Non bounces: ";
        private const string CommitDeletesOnPop3Mailbox = "Commit Deletes on POP3 Mailbox";
        private const string Line = "____________________________";
        private const string ProblemsInsertingLogRecordInDatabase = "Problems inserting log record in Database.\n";
        private const string ExceptionPrefix = "Exception : ";
        private const string FromAddressIsNullError = "FromAddress is null.";
        private const string EmailAddressIsNull = "EmailAddress is null.";
        private const string MessageDateCouldNotBeReadError = "MessageDate could not be read.";
        private const string QuoteSymbol = "\"";
        private const string SpecialEmailAddressSeperatorPlus = "+._-";
        private const string SpecialEmailAddressSeperatorDot = "._-";
        private const string PeriodicDashedLine = "---------------- -------------------- ---------------------- ----------------";
        private const string MessageIsNullDeleting = "- Message is NULL ::  Deleting It";
        private const string NewLine = "\n";
        private const char EmailAdressAtSymbol = '@';
        private const string EnterpriseCommunicatioNetworkComDomain = "@enterprisecommunicationnetwork.com";
        private const string NonBounceIgnoring = "--->Non-Bounce! Ignoring";
        private const string NonBounceIgnoringCheckMessage = "--->Non-Bounce! Ignoring.. Check Message # ";
        private const string MessageSubjectNonBounce = "Message Subject: ";
        private const string DeleteSpamHeader = "Header: ";
        private const string ReceivedHeaderName = "Received";
        private const string SpamDeletingText = "---> SPAM! Deleting It";
        private const string DeleteSpamTextWithEcnSignature = "---> SPAM FOR SURE with ECN Signature! Deleting It";
        private const string BounceSignaturesFileName = "bounce_signatures.xml";
        private const string YLetter = "Y";
        private const string MalformedEmailErrorMessage = "--->Malformed Email! Deleting It";
        private const string MalformedEmailIgnoringMessage = "--->Malformed Email! Ignoring";
        private const string MimeExceptionMalformedEmailErrorMessage = "---> ArgumentException: Malformed EmailAddress Deleting It";
        private const string MimeExceptionMalformedEmailIgnoringMessage = "---> ArgumentException: Malformed EmailAddress! Ignoring";
        private const string ArgumenExceptionMalformedEmailErrorMessage = "---> ArgumentException: Malformed MimeMessage! Deleting It";
        private const string ArgumenExceptionMalformedEmailIgnoreMessage = "---> ArgumentException: Malformed MimeMessage! Ignoring";
        private const string InsertBouncesXmlList = "insertBouncesXMLList";
        private const string KmCommonApplicationName = "KMCommon_Application";
        private const string BounceEngineMain = "BounceEngine.Main";
        private const string Pop3ProtocolExceptionErrorMessage = "---> POP3ProtocolException: ";
        private const string PoP3ProtocolExceptionName = "POP3ProtocolException";
        private const string PoP3ProtocolCheckMessageText = "POP3ProtocolException: Check Message # ";
        private const string GeneralErrorExceptionCheckMessage = "General Error / Exception. Check Message # ";
        private const string ExceptionStackTraceMessage = "Exception stack trace:  \n";
        private const string EmailAddressText = "EmailAddress: ";
        private const string MasterSuppressXmlEmailList = "masterSuppressXMLEmailList";
        private const string Pop3ServerProblemsText = "POP3 server problems";
        private const string GeneralErrorMessageText = "General Error / Exception \n";
        private const string OutOfMemoryExceptionErrorMessage = "---> OutOfMemoryException: OutOfMemoryException";
        private const int Pop3TimeoutInMinutes = 20;
        private static readonly TimeSpan Pop3Timeout = TimeSpan.FromMinutes(20);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string mailServer = "";
        public static string mailUser = "";
        public static string mailPass = "";
        public static string popLog = "";
        public static string outLog = "";
        public static string deleteNonBounces = "";
        public static string masterSuppressBounceDomains = "";

        public static Dictionary<string, string> dBounceDomains = new Dictionary<string, string>();
        public static Dictionary<string, string> dMSBounceDomains = new Dictionary<string, string>();

        public static ConcurrentBag<string> masterSuppressBag = new ConcurrentBag<string>();
        public static ConcurrentBag<string> insertBouncesBag = new ConcurrentBag<string>();
        public static DataTable dtErrorMessage = new DataTable();

        public static ConcurrentBag<ECN.Communicator.Bounces.BounceHolder> BounceList = new ConcurrentBag<ECN.Communicator.Bounces.BounceHolder>();

        public static XmlDocument NDRXML = null;
        public static string errorPage = "";
        public static string lastestNDR_DefPath = ConfigurationManager.AppSettings["LastestNDR_DefPath"];
        public static int msgsToProcess = Convert.ToInt32(ConfigurationManager.AppSettings["msgsToProcess"].ToString());
        public static bool testMode = Convert.ToBoolean(ConfigurationManager.AppSettings["TestMode"].ToString());
        public static bool deleteCompleted = Convert.ToBoolean(ConfigurationManager.AppSettings["DeleteCompleted"].ToString());

        public static string ToProcessFolderPath = ConfigurationManager.AppSettings["MessageFiles.ToProcessFolderPath"] ?? "ToProcess\\";
        public static string ProcessingFailedFolderPath = ConfigurationManager.AppSettings["MessageFiles.ProcessingFailedFolderPath"] ?? "Failed\\";
        public static string ProcessingCompletedFolderPath = ConfigurationManager.AppSettings["MessageFiles.ProcessingCompletedFolderPath"] ?? "Completed\\";

        public static bool WriteMessageToFileIfWeCannotParseTheToAddress = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteMessageToFileIfWeCannotParseTheToAddress"] ?? "false");

        public static string pager_fromEmail = "";
        public static string pager_toEmail = "";
        public static string pager_subject = "";
        public static string pager_notifyToEmail = "";
        public static string signature = "";
        public static int messageindex = 0;

        public static string[] tokens;

        static Regex ValidBouceToAddressPattern = new Regex(@"^bounce_(\d+)-(\d+)@", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        [STAThread]
        static void Main(string[] args)
        {
            InitNdr();
            var bprocessnonstop = true;
            while (bprocessnonstop)
            {
                SetConsoleTitle();
                ReadTokens();
                var pop = GetPOPObject();
                try
                {
                    pop.TimeOut = (int)Pop3Timeout.TotalMilliseconds;
                    pop.Connect();
                    ReadLogSettings();
                    var msgCount = GetMessageCount(pop);
                    ReadPagerSettings();

                    if (msgCount > 0)
                    {
                        msgCount = (msgCount > msgsToProcess) ? msgsToProcess : msgCount;
                        ProcessAllMessages(msgCount, pop);
                        ProcessSavedBounces();

                        if (!testMode)
                        {
                            ProcessAllBouncedEmails();
                        }

                        BounceList = new ConcurrentBag<BounceHolder>();
                        CommitDeletes(pop);
                    }
                    else
                    {
                        bprocessnonstop = false;
                        FileFunctions.LogConsoleActivity(DisconnectingPop3Mailbox, outLog);
                        pop.Disconnect();
                    }
                }
                catch (POP3Exception pop3Exception)
                {
                    LogException(pop3Exception);
                    pop.CancelDeletes();
                }
                catch (OutOfMemoryException outOfMemoryException)
                {
                    LogException(outOfMemoryException);
                    pop.CancelDeletes();
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    pop.CancelDeletes();
                }
            }
        }

        private static void Move_DeleteFiles()
        {
            string targetFolder = ToProcessFolderPath;
            foreach (ECN.Communicator.Bounces.BounceHolder bh in BounceList)
            {
                string targetFilename = targetFolder + bh.fileName;

                if (deleteCompleted)
                {

                    if (File.Exists(targetFilename))
                    {
                        File.Delete(targetFilename);
                    }
                }
                else
                {
                    targetFilename = ProcessingCompletedFolderPath + bh.fileName;
                    string source = ToProcessFolderPath + bh.fileName;
                    if (File.Exists(targetFilename))
                    {
                        FileFunctions.LogConsoleActivity(String.Format("Removing file {0} for overwriting with message {1}", targetFilename, bh.MessageIndex), outLog);
                        File.Delete(targetFilename);
                    }
                    try
                    {
                        File.Move(source, targetFilename);
                    }
                    catch (IOException ex)
                    {
                        // duplicate emails within a batch, ignore
                    }
                }
            }
        }

        private static void ProcessSavedBounces()
        {
            GC.Collect();
            FileFunctions.LogConsoleActivity("Starting threads:" + BounceList.Count.ToString());

            ParallelOptions po = new ParallelOptions();
            try
            {
                po.MaxDegreeOfParallelism = Convert.ToInt32(ConfigurationManager.AppSettings["ParallelThreads"].ToString());
            }
            catch (Exception ex)
            {
                po.MaxDegreeOfParallelism = 1;
            }
            int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["NDR.Timeout"].ToString());
            DataSet ds = LoadXMLFile("bounce_signatures.xml");
            dtErrorMessage = ds.Tables["errormessage"];
            Parallel.ForEach<ECN.Communicator.Bounces.BounceHolder>(BounceList, po, bl =>
            {
                ProcessNDR(bl.MessageIndex, bl.fileName, bl.emailID, bl.blastID, bl.ReceivedDate, timeout);

            });


            FileFunctions.LogConsoleActivity(string.Format("Completed Threads -  {0} ", DateTime.Now));

        }

        private static void DeleteCompletedEmails(POP3 pop)
        {
            if (!testMode)
            {
                foreach (ECN.Communicator.Bounces.BounceHolder bh in BounceList)
                {
                    if (pop.IsConnected)
                    {
                        pop.Delete(bh.MessageIndex);
                    }
                    else
                    {
                        try
                        {
                            pop.Reconnect();
                            pop.Delete(bh.MessageIndex);
                        }
                        catch (POP3ProtocolException popProtoExc)
                        {
                            FileFunctions.LogConsoleActivity("POP3 server problems", outLog);
                            FileFunctions.LogConsoleActivity("Server:" + mailServer + " User:" + mailUser, outLog);
                            FileFunctions.LogConsoleActivity(popProtoExc.ToString(), outLog);
                            KM.Common.Entity.ApplicationLog.LogNonCriticalError(popProtoExc.ToString(), "BounceEngine.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Server:" + mailServer + ", User:" + mailUser + ", insertBouncesXMLList: " + String.Join("", insertBouncesBag) + ", masterSuppressXMLEmailList: " + String.Join("", masterSuppressBag));
                        }
                        catch (POP3Exception popExc)
                        {
                            FileFunctions.LogConsoleActivity("POP3 server problems", outLog);
                            FileFunctions.LogConsoleActivity("Server:" + mailServer + " User:" + mailUser, outLog);
                            FileFunctions.LogConsoleActivity(popExc.ToString(), outLog);
                            KM.Common.Entity.ApplicationLog.LogNonCriticalError(popExc.ToString(), "BounceEngine.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Server:" + mailServer + ", User:" + mailUser + ", insertBouncesXMLList: " + String.Join("", insertBouncesBag) + ", masterSuppressXMLEmailList: " + String.Join("", masterSuppressBag));
                        }
                    }
                }
            }
        }

        private static void DeleteMessageWithCheck(POP3 pop, int messageIndex)
        {
            if (!testMode)
            {
                if (pop.IsConnected)
                {
                    pop.Delete(messageIndex);
                }
                else
                {
                    try
                    {
                        pop.Reconnect();
                        pop.Delete(messageIndex);
                    }
                    catch (POP3ProtocolException popProtoExc)
                    {
                        FileFunctions.LogConsoleActivity("POP3 server problems", outLog);
                        FileFunctions.LogConsoleActivity("Server:" + mailServer + " User:" + mailUser, outLog);
                        FileFunctions.LogConsoleActivity(popProtoExc.ToString(), outLog);
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError(popProtoExc.ToString(), "BounceEngine.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Server:" + mailServer + ", User:" + mailUser + ", insertBouncesXMLList: " + String.Join("", insertBouncesBag) + ", masterSuppressXMLEmailList: " + String.Join("", masterSuppressBag));
                    }
                    catch (POP3Exception popExc)
                    {
                        FileFunctions.LogConsoleActivity("POP3 server problems", outLog);
                        FileFunctions.LogConsoleActivity("Server:" + mailServer + " User:" + mailUser, outLog);
                        FileFunctions.LogConsoleActivity(popExc.ToString(), outLog);
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError(popExc.ToString(), "BounceEngine.Main", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "Server:" + mailServer + ", User:" + mailUser + ", insertBouncesXMLList: " + String.Join("", insertBouncesBag) + ", masterSuppressXMLEmailList: " + String.Join("", masterSuppressBag));
                    }
                }
            }
        }

        public static void NotifyIT(string subject, string errorMsg, string to, string from)
        {

            MailMessage message = new MailMessage(from, to, subject, "<html><body>" + errorMsg + "</body></html>");
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]);
            client.Send(message);


        }

        #region Identify Email for a Bounce
        private static string IdentifyBounce(string strBodyText, out string signature)
        {

            string IdentifyBounce = "unknown";
            signature = "";
            string body = strBodyText.ToLower();
            foreach (DataRow dr in dtErrorMessage.Rows)
            {
                signature = dr["signature"].ToString().ToLower();
                signature = cleanAndTrimSignature(signature);

                if (body.IndexOf(signature) > 1)
                {
                    IdentifyBounce = dr["type"].ToString();

                    break;
                }
                else
                {
                    signature = "";
                }
            }
            return IdentifyBounce;
        }

        private static string IdentifyBounceNDR(NDR ndrMessage, string strBodyText, out string signature)
        {
            string bounceType = "";
            signature = "";
            bool skipListNanny = false;
            try
            {
                // Fix for 5.4.7 which is technically a hard bounce but is more like a soft bounce from the ECN side of things
                // It stands for "message too old"
                // if it doesn't match, do the typical bounce processing

                if (strBodyText.IndexOf("5.4.7") > -1 || strBodyText.IndexOf("547") > -1 || strBodyText.IndexOf("message too old") > -1)
                {
                    // Make certain that it isn't a 500 error
                    if (strBodyText.IndexOf("5.0.0") > -1 || strBodyText.IndexOf("500") > -1)
                    {

                        bounceType = ndrMessage.Type.ToString();
                        signature = cleanAndTrimSignature(ndrMessage.HelpMessage.ToString().ToLower());
                    }
                    else
                    {

                        bounceType = NDRType.SoftBounce.ToString();
                        signature = cleanAndTrimSignature(ndrMessage.HelpMessage.ToString().ToLower());
                    }
                }
                // Fix for 4.4.1 which should be categorized as a soft bounce - WGH 10/14/20106
                else if (strBodyText.IndexOf("4.4.1") > -1)
                {
                    bounceType = NDRType.DnsError.ToString();
                    signature = cleanAndTrimSignature(ndrMessage.HelpMessage.ToString().ToLower());
                    skipListNanny = true;
                }
                else
                {

                    bounceType = ndrMessage.Type.ToString();
                    signature = cleanAndTrimSignature(ndrMessage.HelpMessage.ToString().ToLower());
                }
                // Catch the wierd list nanny types and force them into hard bounces
                if (!skipListNanny && ((ndrMessage.Type == NDRType.AddressChange) ||
                    (ndrMessage.Type == NDRType.Custom1) ||
                    (ndrMessage.Type == NDRType.ChallengeVerification) ||
                    (ndrMessage.Type == NDRType.Custom2) ||
                    (ndrMessage.Type == NDRType.OpenRelayTest) ||
                    (ndrMessage.Type == NDRType.Subscribe) ||
                    (ndrMessage.Type == NDRType.Transient) ||
                    (ndrMessage.Type == NDRType.Unknown) ||
                    (ndrMessage.Type == NDRType.Unsubscribe) ||
                    (ndrMessage.Type == NDRType.VirusNotification)))
                {
                    bounceType = NDRType.HardBounce.ToString();
                }
            }
            catch (Exception)
            {
                bounceType = "";
            }

            return bounceType.ToLower();
        }

        private static string cleanAndTrimSignature(string sign)
        {
            string cleanSignature = sign.Trim();
            cleanSignature = cleanSignature.Replace("\"", "");
            cleanSignature = cleanSignature.Replace("'", "");
            cleanSignature = cleanSignature.Replace(",", "");
            cleanSignature = cleanSignature.Replace("<", "");
            cleanSignature = cleanSignature.Replace(">", "");

            cleanSignature = cleanSignature.Replace("&amp;", "&");
            cleanSignature = cleanSignature.Replace("&amp", "&");
            cleanSignature = cleanSignature.Replace("&am", "&");
            cleanSignature = cleanSignature.Replace("&a", "&");
            cleanSignature = cleanSignature.Replace("&", "&amp;");
            cleanSignature = cleanSignature.Replace("á", "a");

            if (cleanSignature.Length > 353)
            {
                cleanSignature = cleanSignature.Substring(0, 353);
            }

            if (cleanSignature.Length > 4)
            {
                string last4 = cleanSignature.Substring((cleanSignature.Length - 4), 4);
                if (last4.Contains("&"))
                {
                    cleanSignature = cleanSignature.Substring(0, cleanSignature.IndexOf("&", (cleanSignature.Length - 4)));
                }
            }

            // Clear Emojis
            try
            {
                cleanSignature = Regex.Replace(cleanSignature, @"[^\u0000-\u007F]+", string.Empty);
            }
            catch { }

            return cleanSignature;
        }
        #endregion

        private static void LoadNDRLicense()
        {
            NDR.LoadLicenseKey(ConfigurationManager.AppSettings["NDR.LicenseKey"]);
        }

        private static void LoadXMLNDR(string fileLocation)
        {
            if (NDRXML == null)
            {
                if (File.Exists(fileLocation))
                {
                    NDRXML = new XmlDocument();
                    NDRXML.Load(fileLocation);

                }
                else
                {
                    FileFunctions.LogConsoleActivity("XML File: " + fileLocation + " not found", outLog);
                }
            }

        }


        private static void ProcessNDR(int messageIndex, string fileName, int emailID, int blastID, DateTime ReceivedDate, int timeout)
        {
            string messageFromFile = "";
            MimeMessage MIMEFromFile = null;
            string fileWithPath = ToProcessFolderPath + fileName;
            if (File.Exists(fileWithPath))
            {
                messageFromFile = File.ReadAllText(fileWithPath);
                MIMEFromFile = new MimeMessage(messageFromFile);
            }

            if (MIMEFromFile != null)
            {
                string signature = "";
                string BounceWeight = ProcessNDRInternal(messageFromFile, emailID, blastID, ReceivedDate, timeout, out signature);
                if (BounceWeight.Length == 0)
                {
                    BounceWeight = IdentifyBounce(messageFromFile, out signature);
                }

                if (BounceWeight == "unknown")
                {
                    if (MIMEFromFile.MainBody == null)
                    {
                        throw new ApplicationException("WRITE FILE"); // this will put it into the failed folder


                    }
                    else if (MIMEFromFile.MainBody.Length > 353)
                    {
                        signature = MIMEFromFile.MainBody.Substring(0, 353);
                        signature = cleanAndTrimSignature(signature);
                    }
                    else
                    {
                        signature = MIMEFromFile.MainBody.ToString();
                        signature = cleanAndTrimSignature(signature);

                        if (signature.Length > 1)
                        {
                            signature = signature.Substring(0, (signature.Length - 1));
                        }
                        else
                        {
                            signature = "";
                        }
                    }

                    insertBouncesBag.Add(String.Format("<BOUNCE EmailID=\"{0}\" BlastID=\"{1}\" BounceWeight=\"{2}\" Signature=\"{3}\" ReceivedDate=\"{4}\" />", emailID.ToString(), blastID.ToString(), BounceWeight.ToString().Replace("\r", "").Replace("\n", ""), signature.Replace("\r", "").Replace("\n", ""), ReceivedDate.ToString()));

                }
                else
                {
                    insertBouncesBag.Add(String.Format("<BOUNCE EmailID=\"{0}\" BlastID=\"{1}\" BounceWeight=\"{2}\" Signature=\"{3}\" ReceivedDate=\"{4}\"  />", emailID.ToString(), blastID.ToString(), BounceWeight.ToString().Replace("\r", "").Replace("\n", ""), signature.Replace("\r", "").Replace("\n", ""), ReceivedDate.ToString()));

                }
            }
            else
            {
                //Could not read from file

            }

        }

        #region Process NDR for Master Suppression - ProcessNDR()
        private static string ProcessNDRInternal(string txt, int emailID, int blastID, DateTime ReceivedDate, int timeout, out string signature)
        {
            string ndrType = "", ndrHlpMsg = "", ndrBouncedEmailAddress = "";
            signature = "";
            try
            {
                NDR ndr = new NDR();
                bool timedOut = false;
                int timeOut = timeout;
                ndr.LoadFromString(txt, Encoding.Default, timeOut, out timedOut);
                if (!timedOut)
                {
                    ndrType = IdentifyBounceNDR(ndr, txt, out signature);
                    ndrHlpMsg = ndr.HelpMessage.ToString();
                    ndrBouncedEmailAddress = ndr.BouncedEmailAddress.ToString();
                    //FileFunctions.LogConsoleActivity("NDR BOUNCE: [" + ndrBouncedEmailAddress + "] [" + ndrType + "] :: " + ndrHlpMsg, outLog);


                    if (ndrType.Trim().ToLower().Equals("hardbounce") || ndrType.Trim().ToLower().Equals("unknown"))
                    {
                        string bouncedEmailDomain = ndrBouncedEmailAddress.Substring(ndrBouncedEmailAddress.IndexOf('@') + 1);

                        if ((masterSuppressBounceDomains.Equals("Y") && dMSBounceDomains.ContainsKey(bouncedEmailDomain.ToLower())) || txt.IndexOf("5.1.1") > -1)  // || txt.IndexOf("511") > -1 - sunil - removed this condition - 4/14/2014 - 511 cannot be used (it can be in regular text, bouncetags, or in an IP address
                        {

                            ndrHlpMsg = cleanAndTrimSignature(ndrHlpMsg);
                            ndrHlpMsg = "AUTO MASTERSUPPRESSED: " + ndrHlpMsg;

                            masterSuppressBag.Add(String.Format("<BOUNCE EmailID=\"{0}\" BlastID=\"{1}\" Comments=\"{2}\" SubscribeTypeCode=\"{3}\" ReceivedDate=\"{4}\" />", emailID.ToString(), blastID.ToString(), ndrHlpMsg.ToString().Replace("\n", "").Replace("\r", ""), "?", ReceivedDate.ToString()));

                        }
                        else if((txt.IndexOf("5.0.0") > -1 && txt.IndexOf("bad-mailbox") > -1) || (txt.IndexOf("5.7.1") > -1 && txt.IndexOf("does not exist") > -1))
                        {
                            ndrHlpMsg = cleanAndTrimSignature(ndrHlpMsg);
                            ndrHlpMsg = "AUTO MASTERSUPPRESSED: " + ndrHlpMsg;

                            masterSuppressBag.Add(String.Format("<BOUNCE EmailID=\"{0}\" BlastID=\"{1}\" Comments=\"{2}\" SubscribeTypeCode=\"{3}\" ReceivedDate=\"{4}\" />", emailID.ToString(), blastID.ToString(), ndrHlpMsg.ToString().Replace("\n", "").Replace("\r", ""), "B", ReceivedDate.ToString()));
                        }

                    }
                   
                }
                else
                {

                    KM.Common.Entity.ApplicationLog.LogNonCriticalError("NDR timed out after " + timeOut.ToString() + " ms.", "BounceEngine.ProcessNDR", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "EmailID:" + emailID + ", BlastID:" + blastID + "\nMSG TEXT:" + txt);
                }
            }
            catch (NullReferenceException ne)
            {

            }
            catch (Exception ex)
            {

                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BounceEngine.ProcessNDR", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), "EmailID:" + emailID + ", BlastID:" + blastID + "\nMSG TEXT:" + txt);
            }
            return ndrType;

        }
        #endregion

        #region Process Bounced Emails thru SPROC - ProcessBouncedEmails()
        private static int ProcessBouncedEmails()
        {
            int retCount = 0;
            if (insertBouncesBag.Count > 0)
            {
                FileFunctions.LogConsoleActivity("Processing Bounced Emails : EmailActivityLog.InsertBounce '" + String.Join("", insertBouncesBag) + "'", outLog);
                retCount = insertBouncesBag.Count;
                if (false == testMode)
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertBounce("<ROOT>" + String.Join("", insertBouncesBag) + "</ROOT>", Convert.ToInt32(ConfigurationManager.AppSettings["DefaultBounceThreshold"]));
                }
                insertBouncesBag = new ConcurrentBag<string>();

            }
            else
            {
                FileFunctions.LogConsoleActivity("insertBouncesXMLList - No Data Found ", outLog);
            }
            return retCount;
        }
        #endregion

        #region Remove Spam Email from DB thru SPROC - ProcessPermanantBouncedEmails()
        private static int ProcessPermanantBouncedEmails()
        {
            int retCount = 0;
            if (masterSuppressBag.Count > 0)
            {
                FileFunctions.LogConsoleActivity("Updating Database : EmailActivityLog.InsertPermanentBounce '" + String.Join("", masterSuppressBag) + "', 'MASTSUP_UNSUB'", outLog);
                retCount = masterSuppressBag.Count;
                if (false == testMode)
                {
                    ECN_Framework_BusinessLayer.Communicator.EmailActivityLog.InsertSpamFeedbackXML("<ROOT>" + String.Join("", masterSuppressBag) + "</ROOT>", "MASTSUP_UNSUB");
                }
                masterSuppressBag = new ConcurrentBag<string>();
            }
            else
            {
                //FileFunctions.LogConsoleActivity("masterSuppressXMLEmailList: " + String.Join("", masterSuppressBag), outLog);
                FileFunctions.LogConsoleActivity("masterSuppressXMLEmailList - No Data Found ", outLog);
            }
            return retCount;
        }
        #endregion

        #region Load Data from XML's
        private static DataSet LoadXMLFile(string filename)
        {
            DataSet ds = new DataSet();
            if (File.Exists(filename))
            {
                ds.ReadXml(filename);
            }
            else
            {
                FileFunctions.LogConsoleActivity("XML File: " + filename + " not found", outLog);
            }
            return ds;
        }

        private static POP3 GetPOPObject()
        {
            DataSet ds = LoadXMLFile("bounce_conf.xml");
            DataTable dt = ds.Tables["mailsettings"];
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mailServer = dr["mailServer"].ToString();
                    mailUser = dr["mailUser"].ToString();
                    mailPass = dr["mailPass"].ToString();
                    popLog = dr["popLog"].ToString();
                    outLog = dr["outLog"].ToString();
                    deleteNonBounces = dr["deleteNonBounces"].ToString();

                    foreach (string s in dr["bounceDomains"].ToString().Split(','))
                    {
                        if (s != string.Empty && !dBounceDomains.ContainsKey(s.ToLower()))
                            dBounceDomains.Add(s.ToLower(), "");
                    }

                    masterSuppressBounceDomains = dr["masterSuppressBounceDomains"].ToString();
                    string stMSBounceDomains = dr["masterSuppressBounceDomainsList"].ToString();
                    foreach (string s in stMSBounceDomains.Split(','))
                    {
                        if (s != string.Empty && !dMSBounceDomains.ContainsKey(s.ToLower()))
                            dMSBounceDomains.Add(s.ToLower(), "");
                    }

                }
            }
            catch (ArgumentException AE)
            {
                FileFunctions.LogConsoleActivity("Malformed Config File.\n " + AE, outLog);
            }
            POP3 pop = new POP3(mailServer, mailUser, mailPass);
            //if we have write permissions we can log the session
            pop.LogPath = popLog;
            pop.LogOverwrite = true;
            //if we don't have write permissions (as in an ASP.NET application, we can maintain the log in memory)
            pop.LogInMemory = false;//changing this to false to avoid consuming too much memory JWelter 02082017
            return pop;
        }

        private static void ReadPagerSettings()
        {
            DataSet ds = LoadXMLFile("pager_conf.xml");
            DataTable dt = ds.Tables["mailsettings"];
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    pager_fromEmail = dr["fromEmail"].ToString();
                    pager_toEmail = dr["toEmail"].ToString();
                    pager_subject = dr["subject"].ToString();
                    pager_notifyToEmail = dr["notificationToEmail"].ToString();
                }
            }
            catch (ArgumentException AE)
            {
                FileFunctions.LogConsoleActivity("Malformed Config File.\n " + AE, outLog);
            }
        }
        #endregion

        #region Notify Admin on Error - NotifyAdmin()
        private static void NotifyAdmin(string pager_toEmail, string pager_fromEmail, string pager_subject, string errorPage)
        {
            try
            {
                MailMessage msg = new MailMessage(pager_fromEmail, pager_toEmail);
                msg.Subject = pager_subject;
                msg.Body = "Error Occured in the Bounce Engine:" + "<br><br>" + errorPage;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient(ConfigurationSettings.AppSettings["SmtpServer"]);
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                FileFunctions.LogConsoleActivity("Exception when sending notification email: " + ex.Message, outLog);
            }
        }
        #endregion

        private static void ReadTokens()
        {
            tokens = ConfigurationManager.AppSettings[NotificationValues].Split(CommaSeparator);
        }

        private static void ReadLogSettings()
        {
            outLog = ConfigurationManager.AppSettings[OutLog];
            outLog = outLog.Replace(Slash, string.Empty);
        }

        private static void SetConsoleTitle()
        {
            Console.Title = Process.GetCurrentProcess().MainModule.FileName;
        }

        private static void ProcessAllBouncedEmails()
        {
            var bouncesToInsert = 0;

            FileFunctions.LogConsoleActivity(ProcessingBounceRecords, outLog);
            bouncesToInsert = ProcessBouncedEmails();

            FileFunctions.LogConsoleActivity(ProcessingMasterSuppressionRecords, outLog);
            bouncesToInsert += ProcessPermanantBouncedEmails();

            FileFunctions.LogConsoleActivity(LineOfDashes, outLog);
            FileFunctions.LogConsoleActivity($"{BouncesInBags}{(BounceList.Count + masterSuppressBag.Count)}", outLog);
            FileFunctions.LogConsoleActivity($"{Inserted}{bouncesToInsert}", outLog);
            FileFunctions.LogConsoleActivity($"{NonBounces}{((BounceList.Count) - bouncesToInsert)}", outLog);

            Move_DeleteFiles();
        }

        private static void CommitDeletes(POP3 pop)
        {
            try
            {
                FileFunctions.LogConsoleActivity(LineOfDashes, outLog);
                FileFunctions.LogConsoleActivity(CommitDeletesOnPop3Mailbox, outLog);
                if (!testMode)
                {
                    pop.CommitDeletes();
                }
            }
            finally
            {
                FileFunctions.LogConsoleActivity(DisconnectingPop3Mailbox, outLog);
                pop.Disconnect();
            }
        }

        private static void ProcessAllMessages(int messageCount, POP3 pop)
        {
            FileFunctions.LogConsoleActivity(Line, outLog);
            FileFunctions.LogConsoleActivity($"There are {messageCount} messages waiting. {DateTime.Now}", outLog);
            var startMessageIndex = 0;
            for (messageindex = startMessageIndex; messageindex < startMessageIndex + messageCount; messageindex++)
            {
                if (!ProcessMessage(pop))
                {
                    break;
                }
            }
        }

        private static bool ProcessMessage(POP3 pop)
        {
            var properties = new MessageProperties();
            try
            {
                if (!pop.IsConnected)
                {
                    pop.Reconnect();
                }
                ReadTextAndMimeMessage(pop, properties);

                if (properties.MimeMessage != null && properties.Text != null)
                {
                    ReadMessageProperties(properties);
                    LogMessageDetails(properties);
                    ProcessBouncedMessage(pop, properties);
                }
                else
                {
                    DeleteNullMessage(pop);
                }
            }
            catch (SqlException sqlException)
            {
                FileFunctions.LogConsoleActivity($"{ProblemsInsertingLogRecordInDatabase}{sqlException}",
                    outLog);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogExceptionForMessage(ex, pop);
            }
            catch (MimeException ex)
            {
                LogExceptionForMessage(ex, pop);
            }
            catch (ArgumentException ex)
            {
                LogExceptionForMessage(ex, pop);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                LogExceptionForMessage(outOfMemoryException);
                return false;
            }
            catch (POP3ProtocolException pop3ProtocolException)
            {
                LogExceptionForMessage(pop3ProtocolException);
                pop.Disconnect();
                pop.Connect();
                DeleteMessageWithCheck(pop, messageindex);
            }
            catch (Exception ex)
            {
                LogExceptionForMessage(ex, properties.EmailAddress);
            }

            return true;
        }

        private static void ReadTextAndMimeMessage(POP3 pop, MessageProperties properties)
        {
            try
            {
                properties.Text = pop.GetMessageAsText(messageindex);
                properties.MimeMessage = new MimeMessage(properties.Text);
            }
            catch (Exception ex)
            {
                properties.MimeMessage = null;
                properties.Text = null;
                FileFunctions.LogConsoleActivity(ExceptionPrefix + ex.ToString(), outLog);
            }
        }

        private static void ReadMessageProperties(MessageProperties properties)
        {
            properties.EmailAddress = GetEmailAddress(properties.MimeMessage);
            properties.FromAddress = GetFromAddress(properties.MimeMessage);
            properties.MessageDate = GetMessageDate(properties.MimeMessage);
        }

        private static void DeleteNullMessage(POP3 pop)
        {
            FileFunctions.LogConsoleActivity($"{PeriodicDashedLine}{NewLine}", outLog);
            FileFunctions.LogConsoleActivity($"{messageindex}{MessageIsNullDeleting}", outLog);
            DeleteMessageWithCheck(pop, messageindex);
        }

        private static void ProcessBouncedMessage(POP3 pop, MessageProperties messageProperties)
        {
            var bounceContainsEmailAddressSub = BounceContainsEmailAddressSub(messageProperties);
            if (bounceContainsEmailAddressSub)
            {
                if (IsInEcnDomain(messageProperties))
                {
                    var match = ValidBouceToAddressPattern.Match(messageProperties.EmailAddress);
                    int emailId;
                    int blastId;
                    var emailIdParsed = Int32.TryParse(match.Groups[1].Value, out emailId);
                    var blastIdParsed = Int32.TryParse(match.Groups[2].Value, out blastId);

                    if (match.Success && emailIdParsed && blastIdParsed)
                    {
                        ProcessBouncedEmailWithValidEmailAddress(pop, messageProperties);
                    }
                    else
                    {
                        ProcessBouncedEmailWithInValidEmailAddress(pop, messageProperties, emailId, blastId);
                    }
                }
                else
                {
                    DeleteSpamMessageWithEcnSignature(pop);
                }
            }
            else
            {
                if (deleteNonBounces == YLetter)
                {
                    DeleteSpamMessage(pop, messageProperties.MimeMessage);
                }
                else
                {
                    LogNonBounce(messageProperties.MimeMessage);
                }
            }
        }

        private static bool IsInEcnDomain(MessageProperties messageProperties)
        {
            return messageProperties.Text.IndexOf(EnterpriseCommunicatioNetworkComDomain, StringComparison.OrdinalIgnoreCase) > 0;
        }

        private static bool BounceContainsEmailAddressSub(MessageProperties messageProperties)
        {
            var emailAddressSub =
                messageProperties.EmailAddress.Substring(messageProperties.EmailAddress.IndexOf(EmailAdressAtSymbol) + 1);
            return dBounceDomains.ContainsKey(emailAddressSub.ToLower());
        }

        private static void ProcessBouncedEmailWithValidEmailAddress(POP3 pop, MessageProperties messageProperties)
        {
            var fileName = $"BOUNCE_EMAIL_{GetEmailAddressUserName(messageProperties)}.txt";
            var filePathName = $"{ToProcessFolderPath}{fileName}";

            DeleteBouncedFileIfExist(filePathName);
            pop.SaveToFile(messageindex, filePathName);

            var holder = new BounceHolder
            {
                blastID = messageProperties.BlastId,
                emailID = messageProperties.EmailId,
                fileName = fileName,
                MessageIndex = messageindex
            };

            SetHolderReceivedDate(messageProperties, holder);
            BounceList.Add(holder);
            DeleteMessageWithCheck(pop, messageindex);
        }

        private static void SetHolderReceivedDate(MessageProperties messageProperties, BounceHolder holder)
        {
            DateTime bounceDate;
            try
            {
                DateTime.TryParse(messageProperties.MimeMessage.Date, out bounceDate);
            }
            catch
            {
                bounceDate = DateTime.Now;
            }

            if (bounceDate.ToString("yyyy") == "0001" && bounceDate.Month == 1 && bounceDate.Day == 1)
            {
                bounceDate = DateTime.Now;
            }

            holder.ReceivedDate = bounceDate;
        }

        private static void DeleteBouncedFileIfExist(string filePathName)
        {
            if (File.Exists(filePathName))
            {
                FileFunctions.LogConsoleActivity(
                    $"Removing message file {filePathName} for overwriting with message {messageindex}", outLog);
                File.Delete(filePathName);
            }
        }

        private static void ProcessBouncedEmailWithInValidEmailAddress(POP3 pop, MessageProperties messageProperties, int emailId, int blastId)
        {
            messageProperties.EmailId = emailId;
            messageProperties.BlastId = blastId;

            FileFunctions.LogConsoleActivity(
                $"---> message #{messageindex} has invalid TO email address" +
                $" {messageProperties.EmailAddress}", outLog);

            if (WriteMessageToFileIfWeCannotParseTheToAddress)
            {
                var filePathName =
                    $"{ProcessingFailedFolderPath}BAD_TO_EMAIL_{messageindex}" +
                    $"_{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.txt";

                try
                {
                    pop.SaveToFile(messageindex, filePathName);
                }
                catch (Exception ex)
                {
                    FileFunctions.LogConsoleActivity(
                        $@" ---> Failed writing bad message to file ""{filePathName}"": {ex}", outLog);
                }
            }

            DeleteMessageWithCheck(pop, messageindex);
        }

        private static string GetEmailAddressUserName(MessageProperties messageProperties)
        {
            return messageProperties.EmailAddress.Substring(0, messageProperties.EmailAddress.IndexOf(EmailAdressAtSymbol));
        }

        private static void LogNonBounce(MimeMessage msg)
        {
            FileFunctions.LogConsoleActivity(NonBounceIgnoring, outLog);
            errorPage = $"<b>{NonBounceIgnoringCheckMessage}{messageindex}</b><br>";
            errorPage += $"<b>{MessageSubjectNonBounce}</b>{msg.Subject}<br><br>";
        }

        private static void DeleteSpamMessage(POP3 pop, MimeMessage msg)
        {
            FileFunctions.LogConsoleActivity($"{DeleteSpamHeader}{msg.GetHeader(ReceivedHeaderName)}", outLog);
            FileFunctions.LogConsoleActivity(SpamDeletingText, outLog);
            DeleteMessageWithCheck(pop, messageindex);
        }

        private static void DeleteSpamMessageWithEcnSignature(POP3 pop)
        {
            FileFunctions.LogConsoleActivity(DeleteSpamTextWithEcnSignature, outLog);
            DeleteMessageWithCheck(pop, messageindex);
        }

        private static void LogMessageDetails(MessageProperties messageProperties)
        {
            FileFunctions.LogConsoleActivity($"{PeriodicDashedLine}{NewLine}", outLog);
            FileFunctions.LogConsoleActivity(
                $"{messageindex}- TimeStamp:{messageProperties.MessageDate}  " +
                $"To:{messageProperties.EmailAddress}  " +
                $"From:{messageProperties.FromAddress}", outLog);
        }

        private static string GetMessageDate(MimeMessage mimeMessage)
        {
            string messageDate;
            try
            {
                messageDate = mimeMessage.Date;
            }
            catch (Exception ex)
            {
                messageDate = DateTime.Now.ToString();
                Trace.TraceError(MessageDateCouldNotBeReadError, ex);
            }

            return messageDate;
        }

        private static string GetFromAddress(MimeMessage mimeMessage)
        {
            string fromAddress;
            try
            {
                fromAddress = mimeMessage.From.ToString();
            }
            catch (NullReferenceException ex)
            {
                fromAddress = string.Empty;
                Trace.TraceError(FromAddressIsNullError, ex);
            }

            return fromAddress;
        }

        private static string GetEmailAddress(MimeMessage mimeMessage)
        {
            string emailAddress;

            try
            {
                emailAddress = mimeMessage.To[0].EmailAddress;
                emailAddress = emailAddress.Replace(SpecialEmailAddressSeperatorPlus, string.Empty);
                emailAddress = emailAddress.Replace(SpecialEmailAddressSeperatorDot, string.Empty);

                if (emailAddress.Contains(QuoteSymbol))
                {
                    emailAddress = emailAddress.Replace(QuoteSymbol, string.Empty);
                }
            }
            catch (NullReferenceException ex)
            {
                emailAddress = string.Empty;
                Trace.TraceError(EmailAddressIsNull, ex);
            }

            return emailAddress;
        }

        private static int GetMessageCount(POP3 pop)
        {
            FileFunctions.LogConsoleActivity($"connected to {pop.Server} as {pop.Username}, running pop3.PopulateInboxStats", outLog);

            pop.PopulateInboxStats();
            FileFunctions.LogConsoleActivity($"There are {pop.InboxMessageCount} messages waiting.", outLog);

            return pop.InboxMessageCount;
        }

        private static void InitNdr()
        {
            LoadNDRLicense();
            LoadXMLNDR(BounceSignaturesFileName);
            NDR.ImportDefinitionFile(lastestNDR_DefPath);
        }

        private static void LogExceptionForMessage(ArgumentOutOfRangeException exception, POP3 pop)
        {
            if (deleteNonBounces == YLetter)
            {
                FileFunctions.LogConsoleActivity(MalformedEmailErrorMessage, outLog);
                DeleteMessageWithCheck(pop, messageindex);
            }
            else
            {
                FileFunctions.LogConsoleActivity(MalformedEmailIgnoringMessage, outLog);
            }
        }

        private static void LogExceptionForMessage(MimeException exception, POP3 pop)
        {
            if (deleteNonBounces == YLetter)
            {
                FileFunctions.LogConsoleActivity(MimeExceptionMalformedEmailErrorMessage, outLog);
                DeleteMessageWithCheck(pop, messageindex);
            }
            else
            {
                FileFunctions.LogConsoleActivity(MimeExceptionMalformedEmailIgnoringMessage, outLog);
            }
        }

        private static void LogExceptionForMessage(ArgumentException exception, POP3 pop)
        {
            if (deleteNonBounces == YLetter)
            {
                FileFunctions.LogConsoleActivity(ArgumenExceptionMalformedEmailErrorMessage, outLog);
                DeleteMessageWithCheck(pop, messageindex);
            }
            else
            {
                FileFunctions.LogConsoleActivity(ArgumenExceptionMalformedEmailIgnoreMessage, outLog);
            }
        }

        private static void LogExceptionForMessage(OutOfMemoryException outOfMemoryException)
        {
            FileFunctions.LogConsoleActivity(OutOfMemoryExceptionErrorMessage, outLog);

            FileFunctions.LogConsoleActivity(
                $"{InsertBouncesXmlList}:<BR>" +
                $"{string.Join(string.Empty, insertBouncesBag)}", outLog);

            LogCriticalError(outOfMemoryException, $"{InsertBouncesXmlList}: {string.Join(string.Empty, insertBouncesBag)}");
        }

        private static void LogExceptionForMessage(POP3ProtocolException pop3ProtocolException)
        {
            FileFunctions.LogConsoleActivity($"{Pop3ProtocolExceptionErrorMessage}{pop3ProtocolException.Message}", outLog);
            LogNonCriticalError(PoP3ProtocolExceptionName, $"{PoP3ProtocolCheckMessageText}{messageindex}");
        }

        private static void LogExceptionForMessage(Exception exception, string emailAddress)
        {
            LogCriticalError(exception, $"{GeneralErrorExceptionCheckMessage}{messageindex}");
            FileFunctions.LogConsoleActivity($"{GeneralErrorExceptionCheckMessage}{messageindex}", outLog);
            FileFunctions.LogConsoleActivity(ExceptionStackTraceMessage, outLog);
            FileFunctions.LogConsoleActivity(exception.ToString(), outLog);
            FileFunctions.LogConsoleActivity($"{EmailAddressText}{emailAddress}", outLog);
        }

        private static void LogException(POP3Exception pop3Exception)
        {
            FileFunctions.LogConsoleActivity(Pop3ServerProblemsText, outLog);
            FileFunctions.LogConsoleActivity($"Server:{mailServer} User:{mailUser}", outLog);
            FileFunctions.LogConsoleActivity(pop3Exception.ToString(), outLog);
            LogNonCriticalError(pop3Exception.ToString(), $"Server:{mailServer}, User:{mailUser}, {GetBouncesXmlLogMessage()}");
        }

        private static void LogException(OutOfMemoryException outOfMemoryException)
        {
            FileFunctions.LogConsoleActivity(OutOfMemoryExceptionErrorMessage, outLog);
            FileFunctions.LogConsoleActivity(InsertBouncesXmlList + string.Join(string.Empty, insertBouncesBag), outLog);
            LogCriticalError(outOfMemoryException, GetBouncesXmlLogMessage());
        }

        private static void LogException(Exception exception)
        {
            FileFunctions.LogConsoleActivity(GeneralErrorMessageText, outLog);
            FileFunctions.LogConsoleActivity($"Server:{mailServer} User:{mailUser}", outLog);
            FileFunctions.LogConsoleActivity(ExceptionStackTraceMessage, outLog);
            FileFunctions.LogConsoleActivity(exception.ToString(), outLog);
            FileFunctions.LogConsoleActivity(
                $"exception report of {InsertBouncesXmlList}  (NO DB Call made):<BR>" +
                $"{string.Join(string.Empty, insertBouncesBag)}",
                outLog);

            errorPage = "<b>General Error.</b><br><br>";
            errorPage += exception.ToString();
            LogCriticalError(exception, GetBouncesXmlLogMessage());
        }

        private static string GetBouncesXmlLogMessage()
        {
            var note = $"{InsertBouncesXmlList}: {string.Join(string.Empty, insertBouncesBag)}, {MasterSuppressXmlEmailList}: {string.Join(string.Empty, masterSuppressBag)}";
            return note;
        }

        private static void LogCriticalError(Exception exception, string note)
        {
            ApplicationLog.LogCriticalError(
                exception,
                BounceEngineMain,
                Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplicationName]),
                note);
        }

        private static void LogNonCriticalError(string error, string note)
        {
            ApplicationLog.LogNonCriticalError(
                error,
                BounceEngineMain,
                Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplicationName]),
                note);
        }
    }
}