using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Threading;
using ecn.common.classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ecn.communicator.classes;
using ECN_Framework.Common;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using IoPath = System.IO.Path;
using Process = System.Diagnostics.Process;

using static KM.Common.Entity.ApplicationLog;
using BlastSetupInfo = ECN_Framework_BusinessLayer.Communicator.BlastSetupInfo;
using BlastHoldException = ECN_Framework_Common.Objects.BlastHoldException;
using BusinessBlast = ECN_Framework_BusinessLayer.Communicator.Blast;
using CommonLoggingFunctions = ECN_Framework_Common.Functions.LoggingFunctions;
using CommonECNException = ECN_Framework_Common.Objects.ECNException;
using DataFunctions = ecn.common.classes.DataFunctions;
using EmailFunctions = ecn.communicator.classes.EmailFunctions;

namespace ecn.blastengine
{
    class ECNBlastEngine
    {
        private const string FilterBlastSingle = "BlastSingleFilter";
        private const string QuerySelectBlastSingles = 
            "Select bs.*,isnull( b.GroupID, 0) as GroupID from BlastSingles bs join [blast] b on bs.BlastID = b.BlastID  join ECN5_ACCOUNTS..Customer c on b.CustomerID = c.CustomerID Where bs.SendTime < getdate() AND bs.Processed='n'{0} order  by sendtime asc";
        private const string FieldRefBlastId = "refBlastID";
        private const string LogMsgGetttingBlastStatus = "Getting blast single status---{0}";
        private const string OneString = "1";
        private const string FieldBlastSingleId = "BlastSingleID";
        private const string SqlQueryExecSpGetBlastSingleEmailStatusTemplate = "exec sp_getBlastSingleEmailStatus {0}";
        private const string LogMsgBlastingStatusGood = "BlastSingle status good ---{0}";
        private const string FieldEmailId = "EmailID";
        private const string FieldBlastId = "BlastID";
        private const string FieldLayoutPlanId = "LayoutPlanID";
        private const string FieldGroupId = "GroupID";
        private const string LogActivityStartingBlastSingle = "Starting Blast Single: {0}";
        private const string LogActivityStatisticsPrefixTemplate = "statistics_{0}_{1}";

        private const string DelimSlash = "/";
        private const string DelimHypen = "-";
        private const char DelimHypenChar = '-';
        private const string LogMsgGetRefBlast = "Get ref blast ---{0}";
        private const string LogMsgGetRefTriggerId = "Get refTriggerID ---{0}";
        private const string LogActiviryCheckForRefTrigger = "Check for ref trigger ";
        private const string LogActovityCheckForOpen = "Check for Open ";
        private const string LogMsgCheckIfOpen = "Check if there is an open ---{0}";
        private const string LogMsgNoOpensInBlastActivity = 
            "No opens in Blast activity table, check emailactivitylog ---{0}";
        private const string LogActivityDeleteNoOpen = "Delete No Open ";
        private const string LogMsgDeleteNoOpenTrigger = "Delete No Open trigger ---{0}";
        private const string LogMsgHandleSingleBlast = 
            "Handle Single Blast {0} for customer {1} at {2} :: Opens Found. DELETING Record ";
        private const string LogMsgSingleBlastBkasdCustomerId = 
            "Single Blasts = BlastSingleID={0} / BlastID ={1} / customerID {2} at {3}";
        private const string LogActivityUpdateStartTime = "Updating Start Time Blast SingleID:  {0}";
        private const string AppSettingCommunicatorVirtualPath = "Communicator_VirtualPath";
        private const string LogActivityUpdateBlastSingle = "Update Blast single End Time Blast SingleID: {0}";
        private const string DbActivity = "activity";
        private const string ErrorCouldntParseActivitySendCount = "Couldn't parse blast actiity sends count.";
        private const string LogActivityUpdateBlastSendTotal = "Update Blast Send total ";
        private const string LogActivityUpdateBlastSingleStartTime = "Update Blast Single Start Time ";
        private const string LogActivityUpdateBlastSingleEndTime = "Update Blast Single End Time ";
        private const string NoteTemplate = "<br/>{0}: {1}";
        private const string ErrorBlastEngineEncounteredAnException = 
            "Blast engine({0}) encountered an exception when Handle Blast Single.";
        private const string AppSettingKmCommonApplication = "KMCommon_Application";
        private const string ErrorCouldntParseApplicationId = "Couldn't parse application id.";
        private const int HyphenLen = 80;
        private const string CriticalErrorTemplate = 
            "An exception Happened when handling Blast Engine ID = {5} and Blast Single ID = {6}{0} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0} Validation Error: {7}{0}";
        private const string ErrorSingleBlastEngineException = 
            "Blast engine({0}) encountered an exception when Handle Blast Single.";
        private const string CommonExceptionErrorTemplate = 
            "An exception Happened when handling Blast Engine ID = {5} and Blast Single ID = {6}{0} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}";
        private const int SleepHandleBlast = 10000;
        private const string ErrorCancelledUnsubscribed = 
            "Cancelled - EmailID is either unsubscribed or in Master Suppression List.";
        private const string LogOutName = "BlastLog_";
        private const string LogActivityStartingBlast = "Starting Blast: {0}";
        private const string LogMsgStartingBlastTemplate = "Starting Blast: {0} at {1}";
        private const string LogMsgCheckingForMissingFieldsTemplate = 
            "---Checking for missing fields for Blast: {0} at {1}";
        private const string EmailSubjectBlastIssueForCustomer = "Blast Issue for Customer: {0}, Blast ID: {1}";
        private const string EmailBodyBlastCancelled = 
            "Blast has been cancelled. \r\n Key data is missing - {0}.";
        private const string LogMsgKeyDataIsMissingForBlast = "Key data is missing for Blast: {0}";
        private const string LogMsgCheckingAvailableLicensec = 
            "---Checking available licenses for Blast: {0} at {1}";
        private const string EmailBodyNoLicences = "No licenses available. Blast has been set to NOLICENSE.";
        private const string LogMsgNoLicences = "No licenses available for Blast: {0}";
        private const string LogMsgStartBlastForCustomer = "---START BLAST: {0} for customer {1} at {2}";
        private const string LogMsgEndBlastForCustomer = "---END BLAST: {0} for customer {1} at {2}";
        private const string LogMsgSendTotalUpdated = "Send Total updated in all 3 columns = {0}";
        private const string LogMsgErrorException = "ERROR : {0}";
        private const string LogActivityEndingBlastTemplate = "Ending Blast: {0}";
        private const string StatusHold = "HOLD";
        private const string LogMsgBlastIssueHasBeenSet = 
            "Blast Issue for Customer: {0}, Blast ID: {1} Has been set to {2}";
        private const string EmailBodyHoldExceptionTemplate = "{0}\r\n{1}";
        private const string LogMessageSendingBlastTo = "Setting Blast to {0}. {1} ";
        private const string LogActivityBlastEngineEncounteredAnException = 
            "Blast engine({0}) encountered an exception when Handle Blast.";
        private const string LogMsgCommonException =
            "An exception Happened when handling Blast Engine ID = {5} and Blast ID = {6}{0} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0} Validation Error: {7}{0}";
        private const string LogMsgSettingBlastToError = "Setting Blast to error. {0}";
        private const string LogActivityBlastEngineEncounteredException = 
            "Blast engine({0}) encountered an exception when Handle Blast.";
        private const string LogMsgExceptionHappenedWhenHandlingBlastEngine = 
            "An exception Happened when handling Blast Engine ID = {5} and Blast ID = {6}{0} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0}";
        private const string ErrorLoggingCriticalIssue = "Error logging critical issue \n";
        private const string BlastIdTemplate = "BlastID:{0}";
        private const string HeaderOrdinalErrorStack = "Original Error Stack:\n";
        private const string EolTemplate = "{0}\n";
        private const string HeaderExceptionStackTrace = "Exception stack trace:  \n";
        private const string ErrorSettingBlastStatusToError = "Error setting blast status to error \n";
        private const string DelimSpace = " ";
        private const string LogActivityExceptionHandlingBlast = 
            "Blast engine({0}) encountered an exception when Handle Blast.";
        private int _BlastEngineID = Convert.ToInt32(ConfigurationManager.AppSettings["EngineID"]);
        private int _BlastID = int.MinValue;
        private int _BlastSingleID = int.MinValue;
        private int _PlanID = int.MinValue;
        private int _CustomerID = int.MinValue;
        private string _StatusCode = ConfigurationManager.AppSettings["StatusCode"];
        private string _BlastFunction = ConfigurationManager.AppSettings["DoBlast"].ToString().Trim().ToUpper();
        private KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            if (args.Length == 1)
            {

                if (args[0] != "/test" || args[0] == "/?")
                {
                    Console.WriteLine(string.Format("Usage: {0} [/test]", "BlastEngine.exe"));
                    return;
                }
                new ECNBlastEngine().SendEmailNotification("Testing email for SMTP Email Environment.", "Test passed. :)");
                Console.WriteLine(string.Format("Testing email is send out to {0}. Please check the email.", ConfigurationManager.AppSettings["AdminSendTo"]));
                return;
            }

            new ECNBlastEngine().StartBlastEngine();
        }

        public void StartBlastEngine()
        {
            Console.WriteLine(string.Format("Email Blast Engine({1}) starts at {0}", DateTime.Now.ToString(), System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
            Console.WriteLine(string.Format("With additonal StatusCode = {0}", _StatusCode));
            EmailFunctions emailFunctions = new EmailFunctions();
            //ECN_Framework.Common.ChannelCheck cc = new ECN_Framework.Common.ChannelCheck();            

            while (true)
            {
                DoBlast(emailFunctions);
            }

        }

        private void DoBlast(EmailFunctions emailFunctions)
        {
            if (_BlastFunction == "SINGLE")
            {
                HandleSingleBlast(emailFunctions);
            }
            else if (_BlastFunction == "MULTIPLE")
            {
                HandleBlast(emailFunctions);
            }
        }

        private void HandleBlast(EmailFunctions emailFunctions)
        {
            var possibleBlast = GetPossibleBlast();
            if (null != possibleBlast)
            {
                try
                {
                    HandleBlastExisting(emailFunctions, possibleBlast);
                }
                catch (BlastHoldException blastHoldException)
                {
                    HandleBlastOnException(blastHoldException);
                }
                catch (CommonECNException commonException)
                {
                    HandleBlastOnException(commonException);
                }
                catch (Exception exception)
                {
                    HandleBlastOnException(exception);
                }
            }
            else
            {
                //no blast in the queue.
                Thread.Sleep(SleepHandleBlast); // Wait for 10 seconds before check for pending blasts again
            }
        }

        private void HandleBlastExisting(EmailFunctions emailFunctions, object possibleBlast)
        {
            Guard.NotNull(emailFunctions, nameof(emailFunctions));

            int blastId;
            int.TryParse(possibleBlast?.ToString(), out blastId);
            _BlastID = Convert.ToInt32(possibleBlast);

            Removecache(_BlastID); //clear all cache for blast
            CreateCache(_BlastID);

            HandleBlastLogStartingBlast();

            Console.WriteLine(LogMsgStartingBlastTemplate, _BlastID, DateTime.Now);

            //Latha: update all status fields except AlreadySent and SendTotal
            //set the actual start time
            DataFunctions.Execute(string.Format(
                "update blast set starttime = getdate() where blastID={0}",
                _BlastID));

            var toBlast = new Blasts(_BlastID);
            var customerID = toBlast.CustomerID();
            Console.WriteLine(
                LogMsgCheckingForMissingFieldsTemplate, _BlastID, DateTime.Now);
            var missingField = VerifyForeignKeysExist(_BlastID);
            if (missingField.Length > 0)
            {
                CancelBlast(_BlastID);
                SendAccountManagersEmailNotification(
                    string.Format(EmailSubjectBlastIssueForCustomer, customerID, _BlastID),
                    string.Format(EmailBodyBlastCancelled, missingField));
                Console.WriteLine(LogMsgKeyDataIsMissingForBlast, _BlastID);
            }
            else
            {
                HandleBlastOnNoMissingField(emailFunctions, customerID, toBlast);
            }

            HandleBlastEnding();
        }

        private void HandleBlastOnNoMissingField(EmailFunctions emailFunctions, int customerID, Blasts toBlast)
        {
            Guard.NotNull(emailFunctions, nameof(emailFunctions));
            Guard.NotNull(toBlast, nameof(toBlast));

            //wgh - check license here before sending blast
            Console.WriteLine(LogMsgCheckingAvailableLicensec, _BlastID, DateTime.Now);
            if (!GetAvailableLicenses(_BlastID))
            {
                //wgh - don't send, set status to "NOLICENSE"
                SetBlastToNoLicense(_BlastID);
                SendAccountManagersEmailNotification(
                    string.Format(EmailSubjectBlastIssueForCustomer, customerID, _BlastID),
                    EmailBodyNoLicences);
                Console.WriteLine(LogMsgNoLicences, _BlastID);
            }
            else
            {
                var channelCheck = new ChannelCheck(customerID);
                toBlast.SetActive();
                Console.WriteLine(LogMsgStartBlastForCustomer, _BlastID, customerID, DateTime.Now);
                emailFunctions.SendBlast(
                    toBlast.ID(),
                    ConfigurationManager.AppSettings[AppSettingCommunicatorVirtualPath],
                    channelCheck.getHostName(),
                    channelCheck.getBounceDomain());
                Console.WriteLine(LogMsgEndBlastForCustomer, _BlastID, customerID, DateTime.Now);
                var setupInfo = BlastSetupInfo.GetNextScheduledBlastSetupInfo(toBlast.ID());
                if (setupInfo != null)
                {
                    CloneBlast(toBlast.ID(), setupInfo);
                }

                try
                {
                    var selectQuery = string.Format(
                        "select count(EAID) from emailactivitylog where actiontypecode in ('send','testsend') and blastID={0}",
                        _BlastID);
                    var finalSends = Convert.ToInt32(DataFunctions.ExecuteScalar(selectQuery));
                    var updateQuery = string.Format(
                        "update blast set attempttotal={0},successtotal={0},sendtotal={0} where blastID={1}",
                        finalSends,
                        _BlastID);
                    DataFunctions.Execute(updateQuery);
                    Console.WriteLine(LogMsgSendTotalUpdated, finalSends);
                }
                catch (Exception ex)
                {
                    Removecache(_BlastID);
                    Console.WriteLine(LogMsgErrorException, ex.ToString());
                }
            }

            Console.WriteLine(DelimSpace);
        }

        private void HandleBlastOnException(BlastHoldException blastHoldException)
        {
            Guard.NotNull(blastHoldException, nameof(blastHoldException));

            var newStatus = blastHoldException.NewStatus ?? StatusHold;
            SetBlastToNewStatus(_BlastID, newStatus);
            var message = string.Format(
                LogMsgBlastIssueHasBeenSet,
                blastHoldException.CustomerID,
                blastHoldException.BlastID,
                blastHoldException.NewStatus);
            SendAccountManagersEmailNotification(
                message,
                string.Format(EmailBodyHoldExceptionTemplate, message, blastHoldException.Message));
            Console.WriteLine(LogMessageSendingBlastTo, newStatus, blastHoldException.Message);

            Removecache(_BlastID);
        }

        private void HandleBlastOnException(CommonECNException commonException)
        {
            Guard.NotNull(commonException, nameof(commonException));

            var builder = new StringBuilder();
            foreach (var ecnError in commonException.ErrorList)
            {
                builder.AppendFormat(NoteTemplate, ecnError.Entity, ecnError.ErrorMessage);
            }

            var note = builder.ToString();

            int applicationId;
            int.TryParse(ConfigurationManager.AppSettings[AppSettingKmCommonApplication], out applicationId);

            var idPostfix = string.Concat(
                Environment.NewLine,
                new string(DelimHypenChar, HyphenLen),
                Environment.NewLine);
            LogCriticalError(
                commonException,
                string.Format(
                    LogActivityBlastEngineEncounteredAnException,
                    IoPath.GetFileNameWithoutExtension(
                        Process.GetCurrentProcess().MainModule.FileName)),
                applicationId,
                string.Format(
                    LogMsgCommonException,
                    idPostfix,
                    commonException.Message,
                    commonException.Source,
                    commonException.StackTrace,
                    commonException.InnerException,
                    _BlastEngineID,
                    _BlastID,
                    note));
            SetBlastToError(_BlastID);
            Console.WriteLine(LogMsgSettingBlastToError, commonException.Message);

            Removecache(_BlastID);
        }

        private void HandleBlastOnException(Exception exception)
        {
            Guard.NotNull(exception, nameof(exception));

            try
            {
                int applicationId;
                int.TryParse(ConfigurationManager.AppSettings[AppSettingKmCommonApplication], out applicationId);

                var idPostfix = string.Concat(
                    Environment.NewLine,
                    new string(DelimHypenChar, HyphenLen),
                    Environment.NewLine);

                LogCriticalError(
                    exception,
                    string.Format(
                        LogActivityBlastEngineEncounteredException,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName)),
                    applicationId,
                    string.Format(
                        LogMsgExceptionHappenedWhenHandlingBlastEngine,
                        idPostfix,
                        exception.Message,
                        exception.Source,
                        exception.StackTrace,
                        exception.InnerException,
                        _BlastEngineID,
                        _BlastID));
            }
            catch (Exception kmExc)
            {
                HandleBlastLogLogCriticalError(exception, kmExc);
            }

            try
            {
                SetBlastToError(_BlastID);
            }
            catch (Exception kmExc2)
            {
                HandleBlastLogErrorSetting(kmExc2);
            }

            Console.WriteLine(exception.Message);
            Removecache(_BlastID);
        }

        private void HandleBlastLogLogCriticalError(Exception exception, Exception kmExc)
        {
            Guard.NotNull(kmExc, nameof(kmExc));

            FileFunctions.LogConsoleActivity(ErrorLoggingCriticalIssue, LogOutName);
            FileFunctions.LogConsoleActivity(string.Format(BlastIdTemplate, _BlastID), LogOutName);
            FileFunctions.LogConsoleActivity(HeaderOrdinalErrorStack, LogOutName);
            FileFunctions.LogConsoleActivity(string.Format(EolTemplate, exception), LogOutName);
            FileFunctions.LogConsoleActivity(HeaderExceptionStackTrace, LogOutName);
            FileFunctions.LogConsoleActivity(kmExc.ToString(), LogOutName);
        }

        private void HandleBlastLogErrorSetting(Exception kmExc2)
        {
            Guard.NotNull(kmExc2, nameof(kmExc2));

            FileFunctions.LogConsoleActivity(ErrorSettingBlastStatusToError, LogOutName);
            FileFunctions.LogConsoleActivity(string.Format(BlastIdTemplate, _BlastID), LogOutName);
            FileFunctions.LogConsoleActivity(HeaderExceptionStackTrace, LogOutName);
            FileFunctions.LogConsoleActivity(kmExc2.ToString(), LogOutName);
        }

        private void HandleBlastEnding()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    string.Format(LogActivityEndingBlastTemplate, _BlastID),
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private void HandleBlastLogStartingBlast()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    string.Format(LogActivityStartingBlast, _BlastID),
                    string.Format(LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace("/", "-")));
            }
        }

        private object GetPossibleBlast()
        {
            // Get one unfinished blast and send it Need to use spinlock when multiple engines are running.
            //Latha: update status, status date
            var sqlQuery = "SELECT TOP 1 BlastID" +
                      " FROM Blast" +
                      " WHERE SendTime < GetDate() AND StatusCode = @StatusCode AND BlastEngineID = @BlastEngineID" +
                      " ORDER BY SendTime ASC, blastID ASC";
            var command = new SqlCommand(sqlQuery);
            command.Parameters.AddWithValue("@BlastEngineID", _BlastEngineID);
            command.Parameters.AddWithValue("@StatusCode", _StatusCode);

            object possibleBlast = null;
            try
            {
                possibleBlast = DataFunctions.ExecuteScalar(command);
            }
            catch (Exception outerEx)
            {
                try
                {
                    GetPossibleBlastOnCriticalError(outerEx);
                }
                catch (Exception kmExc)
                {
                    HandleBlastLogLogCriticalError(outerEx, kmExc);
                }
            }

            return possibleBlast;
        }

        private void GetPossibleBlastOnCriticalError(Exception outerEx)
        {
            Guard.NotNull(outerEx, nameof(outerEx));

            int applicationId;
            int.TryParse(ConfigurationManager.AppSettings[AppSettingKmCommonApplication], out applicationId);

            var idPostfix = string.Concat(
                Environment.NewLine,
                new string(DelimHypenChar, HyphenLen),
                Environment.NewLine);

            LogCriticalError(
                outerEx,
                string.Format(
                    LogActivityExceptionHandlingBlast,
                    IoPath.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)),
                applicationId,
                string.Format(
                    LogMsgExceptionHappenedWhenHandlingBlastEngine,
                    idPostfix,
                    outerEx.Message,
                    outerEx.Source,
                    outerEx.StackTrace,
                    outerEx.InnerException,
                    _BlastEngineID,
                    _BlastID));
        }

        private void CreateCache(int blastID)
        {
            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);

            try
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message); // TODO - send notification
            }

            try
            {
                ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(blast.LayoutID.Value, false);
            }
            catch (Exception ex)
            {
                //Dont do anything, might be a champion
                //throw new Exception(ex.Message); // TODO - send notification
            }

            //  TODO - Adding caching
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(blast.CustomerID.Value, false);

            //  TODO - Adding caching for Clientservicefeature (LIST<>)
            //KMPlatform.BusinessLogic.Client.GetServiceFeatures(customer.PlatformClientID);

            //  TODO - Adding caching
            List<ECN_Framework_Entities.Accounts.SubscriptionManagement> smList = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.GetByBaseChannelID(customer.BaseChannelID.Value);
            
        }

        private void Removecache(int blastID)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                KM.Common.CacheUtil.RemoveFromCache(blastID.ToString(), "Blast");

                ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);

                if (blast.GroupID.HasValue)
                    KM.Common.CacheUtil.RemoveFromCache(blast.GroupID.Value.ToString(), "Group");
                if (blast.CustomerID.HasValue)
                    KM.Common.CacheUtil.RemoveFromCache(blast.CustomerID.Value.ToString(), "Customer");

                KM.Common.CacheUtil.RemoveFromCache("ALL", "Basechannel");

                if (blast.LayoutID.HasValue && blast.LayoutID.Value > 0)
                {
                    KM.Common.CacheUtil.RemoveFromCache(blast.LayoutID.Value.ToString(), "Layout");

                    KM.Common.CacheUtil.RemoveFromCache(blast.LayoutID.Value.ToString(), "BlastLink");
                }
                KM.Common.CacheUtil.RemoveFromCache("CACHE_BLASTLINK_" + "_" + blastID.ToString(), "BlastLink");
                KM.Common.CacheUtil.RemoveFromCache("CACHE_BLASTLINK_" + "_" + blastID.ToString() + "_Dictionary", "BlastLink");


                KM.Common.CacheUtil.RemoveFromCache("CACHE_UNIQUELINK_" + "_" + blastID.ToString(), "UNIQUELINK");
                KM.Common.CacheUtil.RemoveFromCache("CACHE_UNIQUELINK_" + "_" + blastID.ToString() + "_Dictionary", "UNIQUELINK");

            }


        }
        private void CloneBlast(int currentBlastID, ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo)
        {
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Starting CloneBlast: {0}", currentBlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(currentBlastID, false);
            int userID = b.CreatedUserID.Value;

            //build user object 
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(userID, false);
            try
            {
                //ECN_Framework_Entities.Accounts.User user = ECN_Framework_BusinessLayer.Accounts.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"], true);

                ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(b.CustomerID.Value, false);
                ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(c.BaseChannelID.Value);
                user.CurrentClient = new KMPlatform.BusinessLogic.Client().Select(c.PlatformClientID, true);
                user.CurrentClientGroup = new KMPlatform.BusinessLogic.ClientGroup().Select(bc.PlatformClientGroupID, true);
                user.CurrentSecurityGroup = new KMPlatform.BusinessLogic.SecurityGroup().Select(user.UserID, user.CurrentClient.ClientID, false, true);


                ECN_Framework_Entities.Communicator.CampaignItem ciCurrent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(currentBlastID, true);
                DateTime oldSendTime = ciCurrent.SendTime.Value;
                if (ciCurrent != null && ciCurrent.CampaignItemType == ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString())
                {
                    //create new campaign item
                    ECN_Framework_Entities.Communicator.CampaignItem ciNew = new ECN_Framework_Entities.Communicator.CampaignItem();
                    ciNew.CampaignID = ciCurrent.CampaignID;
                    if (ciCurrent.CampaignItemIDOriginal == null)
                        ciNew.CampaignItemIDOriginal = ciCurrent.CampaignItemID;
                    else
                        ciNew.CampaignItemIDOriginal = ciCurrent.CampaignItemIDOriginal;
                    ciNew.CampaignItemNameOriginal = ciCurrent.CampaignItemNameOriginal;
                    if (ciNew.CampaignItemNameOriginal.Trim() == string.Empty)
                        ciNew.CampaignItemNameOriginal = ciCurrent.CampaignItemName;
                    ciNew.CampaignItemName = ciCurrent.CampaignItemNameOriginal + ": " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                    ciNew.CampaignItemType = ciCurrent.CampaignItemType;
                    ciNew.FromName = ciCurrent.FromName;
                    ciNew.FromEmail = ciCurrent.FromEmail;
                    ciNew.ReplyTo = ciCurrent.ReplyTo;
                    ciNew.SendTime = setupInfo.SendTime;
                    ciNew.BlastScheduleID = setupInfo.BlastScheduleID;
                    ciNew.OverrideAmount = setupInfo.SendNowAmount;
                    ciNew.OverrideIsAmount = setupInfo.SendNowIsAmount;
                    ciNew.BlastField1 = ciCurrent.BlastField1;
                    ciNew.BlastField2 = ciCurrent.BlastField2;
                    ciNew.BlastField3 = ciCurrent.BlastField3;
                    ciNew.BlastField4 = ciCurrent.BlastField4;
                    ciNew.BlastField5 = ciCurrent.BlastField5;
                    ciNew.CompletedStep = ciCurrent.CompletedStep;
                    ciNew.CreatedUserID = ciCurrent.CreatedUserID;
                    ciNew.CampaignItemFormatType = ciCurrent.CampaignItemFormatType;
                    ciNew.IsHidden = ciCurrent.IsHidden;
                    ciNew.CustomerID = ciCurrent.CustomerID;
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save_NoAccessCheck(ciNew, user);

                    ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlastCurrent = ciCurrent.BlastList.SingleOrDefault(x => x.BlastID == currentBlastID);
                    //create new campaign item suppression as necessary
                    ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppressionNew;
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppressionCurrent in ciCurrent.SuppressionList)
                    {
                        ciSuppressionNew = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                        ciSuppressionNew.CampaignItemID = ciNew.CampaignItemID;
                        ciSuppressionNew.GroupID = ciSuppressionCurrent.GroupID;
                        ciSuppressionNew.CreatedUserID = ciCurrent.CreatedUserID;
                        ciSuppressionNew.CustomerID = ciSuppressionCurrent.CustomerID;
                        int CISuppressionID = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Save_NoAccessCheck(ciSuppressionNew, user);

                        if (ciSuppressionCurrent.Filters != null && ciSuppressionCurrent.Filters.Count(x => !x.IsDeleted.HasValue || !x.IsDeleted.Value) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibfSupp in ciSuppressionCurrent.Filters.Where(x => !x.IsDeleted.HasValue || !x.IsDeleted.Value))
                            {
                                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                cibf.CampaignItemSuppresionID = CISuppressionID;
                                cibf.SuppressionGroupID = ciSuppressionNew.GroupID;
                                cibf.FilterID = cibfSupp.FilterID;
                                cibf.SmartSegmentID = cibfSupp.SmartSegmentID;
                                cibf.RefBlastIDs = cibfSupp.RefBlastIDs;
                                cibf.IsDeleted = false;
                                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                            }
                        }
                    }

                    //create new campaign item blast

                    ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlastNew = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    ciBlastNew.CampaignItemID = ciNew.CampaignItemID;
                    ciBlastNew.EmailSubject = ciBlastCurrent.EmailSubject;
                    ciBlastNew.DynamicFromName = ciBlastCurrent.DynamicFromName;
                    ciBlastNew.DynamicFromEmail = ciBlastCurrent.DynamicFromEmail;
                    ciBlastNew.DynamicReplyTo = ciBlastCurrent.DynamicReplyTo;
                    ciBlastNew.LayoutID = ciBlastCurrent.LayoutID;
                    ciBlastNew.GroupID = ciBlastCurrent.GroupID;

                    ciBlastNew.CreatedUserID = ciCurrent.CreatedUserID;
                    ciBlastNew.AddOptOuts_to_MS = ciBlastCurrent.AddOptOuts_to_MS;
                    ciBlastNew.CustomerID = ciBlastCurrent.CustomerID;
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save_NoAccessCheck(ciBlastNew, user);

                    if (ciBlastCurrent.Filters.Count(x => x.FilterID != null) > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlastCurrent.Filters.Where(x => x.FilterID != null))
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemBlastFilter newCIBF = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                            newCIBF.CampaignItemBlastID = ciBlastNew.CampaignItemBlastID;
                            newCIBF.FilterID = cibf.FilterID;
                            newCIBF.IsDeleted = false;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(newCIBF);
                        }
                    }
                    if (ciBlastCurrent.Filters.Count(x => x.SmartSegmentID != null) > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlastCurrent.Filters.Where(x => x.SmartSegmentID != null))
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemBlastFilter newCIBF = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                            newCIBF.CampaignItemBlastID = ciBlastNew.CampaignItemBlastID;
                            newCIBF.SmartSegmentID = cibf.SmartSegmentID;
                            newCIBF.RefBlastIDs = cibf.RefBlastIDs;
                            newCIBF.IsDeleted = false;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(newCIBF);
                        }
                    }

                    //create new campaign item blast ref blast as necessary
                    //ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast cibRefBlastNew;
                    //foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast cibRefBlastCurrent in ciBlastCurrent.RefBlastList)
                    //{
                    //    cibRefBlastNew = new ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast();
                    //    cibRefBlastNew.CampaignItemBlastID = ciBlastNew.CampaignItemBlastID;
                    //    cibRefBlastNew.RefBlastID = cibRefBlastCurrent.RefBlastID;
                    //    cibRefBlastNew.CreatedUserID = ciCurrent.CreatedUserID;
                    //    cibRefBlastNew.CustomerID = cibRefBlastCurrent.CustomerID;
                    //    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Save(cibRefBlastNew, user);
                    //}

                    //create actual blast
                    ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem_NoAccessCheck(ciNew.CampaignItemID, user);
                    ECN_Framework_Entities.Communicator.BlastAbstract blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID_NoAccessCheck(ciBlastNew.CampaignItemBlastID, false);

                    //wgh - for smart segment blasts
                    int? ssBlastIDOld = GetSmartSegmentBlast(currentBlastID);
                    if (ssBlastIDOld != null && ssBlastIDOld.Value > 0)
                    {
                        ciCurrent = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(ssBlastIDOld.Value, true);
                        //create new campaign item
                        ciNew = new ECN_Framework_Entities.Communicator.CampaignItem();
                        ciNew.CampaignID = ciCurrent.CampaignID;
                        if (ciCurrent.CampaignItemIDOriginal == null)
                            ciNew.CampaignItemIDOriginal = ciCurrent.CampaignItemID;
                        else
                            ciNew.CampaignItemIDOriginal = ciCurrent.CampaignItemIDOriginal;
                        ciNew.CampaignItemNameOriginal = ciCurrent.CampaignItemNameOriginal;
                        if (ciNew.CampaignItemNameOriginal.Trim() == string.Empty)
                            ciNew.CampaignItemNameOriginal = ciCurrent.CampaignItemName;
                        ciNew.CampaignItemName = ciCurrent.CampaignItemNameOriginal + ": " + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        ciNew.CampaignItemType = ciCurrent.CampaignItemType;
                        ciNew.FromName = ciCurrent.FromName;
                        ciNew.FromEmail = ciCurrent.FromEmail;
                        ciNew.ReplyTo = ciCurrent.ReplyTo;
                        DateTime tempDateTime = ciCurrent.SendTime.Value.AddDays((ciCurrent.SendTime.Value - oldSendTime).Days);
                        tempDateTime = tempDateTime.Date + ciCurrent.SendTime.Value.TimeOfDay;
                        ciNew.SendTime = tempDateTime;
                        ciNew.BlastScheduleID = setupInfo.BlastScheduleID;
                        ciNew.OverrideAmount = setupInfo.SendNowAmount;
                        ciNew.OverrideIsAmount = setupInfo.SendNowIsAmount;
                        ciNew.BlastField1 = ciCurrent.BlastField1;
                        ciNew.BlastField2 = ciCurrent.BlastField2;
                        ciNew.BlastField3 = ciCurrent.BlastField3;
                        ciNew.BlastField4 = ciCurrent.BlastField4;
                        ciNew.BlastField5 = ciCurrent.BlastField5;
                        ciNew.CompletedStep = ciCurrent.CompletedStep;
                        ciNew.CreatedUserID = ciCurrent.CreatedUserID;
                        ciNew.CampaignItemFormatType = ciCurrent.CampaignItemFormatType;
                        ciNew.IsHidden = ciCurrent.IsHidden;
                        ciNew.CustomerID = ciCurrent.CustomerID;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save_NoAccessCheck(ciNew, user);

                        //create new campaign item suppression as necessary
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppressionCurrent in ciCurrent.SuppressionList)
                        {
                            ciSuppressionNew = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                            ciSuppressionNew.CampaignItemID = ciNew.CampaignItemID;
                            ciSuppressionNew.GroupID = ciSuppressionCurrent.GroupID;
                            ciSuppressionNew.CreatedUserID = ciCurrent.CreatedUserID;
                            ciSuppressionNew.CustomerID = ciSuppressionCurrent.CustomerID;
                            int CISuppressionID = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Save_NoAccessCheck(ciSuppressionNew, user);

                            if (ciSuppressionCurrent.Filters != null && ciSuppressionCurrent.Filters.Count(x => !x.IsDeleted.HasValue || !x.IsDeleted.Value) > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibfSupp in ciSuppressionCurrent.Filters.Where(x => !x.IsDeleted.HasValue || !x.IsDeleted.Value))
                                {
                                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                    cibf.CampaignItemSuppresionID = CISuppressionID;
                                    cibf.SuppressionGroupID = ciSuppressionNew.GroupID;
                                    cibf.FilterID = cibfSupp.FilterID;
                                    cibf.SmartSegmentID = cibfSupp.SmartSegmentID;
                                    cibf.RefBlastIDs = cibfSupp.RefBlastIDs;
                                    cibf.IsDeleted = false;
                                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                                }
                            }
                        }

                        //create new campaign item blast
                        ciBlastCurrent = ciCurrent.BlastList.SingleOrDefault(x => x.BlastID == ssBlastIDOld);
                        ciBlastNew = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                        ciBlastNew.CampaignItemID = ciNew.CampaignItemID;
                        ciBlastNew.EmailSubject = ciBlastCurrent.EmailSubject;
                        ciBlastNew.DynamicFromName = ciBlastCurrent.DynamicFromName;
                        ciBlastNew.DynamicFromEmail = ciBlastCurrent.DynamicFromEmail;
                        ciBlastNew.DynamicReplyTo = ciBlastCurrent.DynamicReplyTo;
                        ciBlastNew.LayoutID = ciBlastCurrent.LayoutID;
                        ciBlastNew.GroupID = ciBlastCurrent.GroupID;
                        //ciBlastNew.FilterID = ciBlastCurrent.FilterID;
                        //ciBlastNew.SmartSegmentID = ciBlastCurrent.SmartSegmentID;
                        ciBlastNew.CreatedUserID = ciCurrent.CreatedUserID;
                        ciBlastNew.AddOptOuts_to_MS = ciBlastCurrent.AddOptOuts_to_MS;
                        ciBlastNew.CustomerID = ciBlastCurrent.CustomerID;
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save_NoAccessCheck(ciBlastNew, user);

                        if (ciBlastCurrent.Filters.Count(x => x.FilterID != null) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlastCurrent.Filters.Where(x => x.FilterID != null))
                            {
                                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter newCIBF = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                newCIBF.CampaignItemBlastID = ciBlastNew.CampaignItemBlastID;
                                newCIBF.FilterID = cibf.FilterID;
                                newCIBF.IsDeleted = false;
                                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(newCIBF);
                            }
                        }
                        if (ciBlastCurrent.Filters.Count(x => x.SmartSegmentID != null) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ciBlastCurrent.Filters.Where(x => x.SmartSegmentID != null))
                            {
                                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter newCIBF = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                                newCIBF.CampaignItemBlastID = ciBlastNew.CampaignItemBlastID;
                                newCIBF.SmartSegmentID = cibf.SmartSegmentID;
                                newCIBF.RefBlastIDs = cibf.RefBlastIDs;
                                newCIBF.IsDeleted = false;
                                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(newCIBF);
                            }
                        }

                        //create new campaign item blast ref blast                    
                        //cibRefBlastNew = new ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast();
                        //cibRefBlastNew.CampaignItemBlastID = ciBlastNew.CampaignItemBlastID;
                        //cibRefBlastNew.RefBlastID = blast.BlastID;
                        //cibRefBlastNew.CreatedUserID = ciCurrent.CreatedUserID;
                        //cibRefBlastNew.CustomerID = ciBlastNew.CustomerID;
                        //ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Save(cibRefBlastNew, user);

                        //create actual blast
                        ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem_NoAccessCheck(ciNew.CampaignItemID, user);
                        blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID_NoAccessCheck(ciNew.CampaignItemID, false);
                    }

                    ////create dept item ref as necessary
                    //ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
                    //if (sc.hasProductFeature("ecn.communicator", "Users Departments", blast.CustomerID.ToString()))
                    //{
                    //    string deptID = DataFunctions.ExecuteScalar("communicator", "SELECT DepartmentID FROM DeptItemReferences WHERE ItemID = " + currentBlastID + " AND Item = 'BLST' ").ToString();
                    //    string sqlquery = " INSERT INTO DeptItemReferences (DepartmentID, Item, ItemID) VALUES (" + deptID + ", 'BLST', " + blast.BlastID.ToString() + ") ";
                    //    DataFunctions.Execute("communicator", sqlquery).ToString();
                    //}

                }
            }
            catch (ECN_Framework_Common.Objects.SecurityException se)
            {
                SendAccountManagersEmailNotification("Recurring Blast issue", "UserID: " + user.UserID + " No longer has access to blasts");
                Console.WriteLine(string.Format("User cannot set up recurring blasts.  UserID: {0}", user.UserID.ToString()));
            }

            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Ending CloneBlast: {0}", currentBlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));
        }

        private int? GetSmartSegmentBlast(int blastID)
        {
            int? refBlastID = null;
            try
            {
                string sql = "SELECT TOP 1 BlastID FROM Blast WHERE RefBlastID LIKE '%" + blastID + "%' and StatusCode <> 'Deleted'";// AND FilterID IN (2147483644,2147483645,2147483647,2147483643,2147483642)";
                SqlCommand cmd = new SqlCommand(sql);
                object possible_blast = DataFunctions.ExecuteScalar(cmd);
                refBlastID = Convert.ToInt32(possible_blast);
            }
            catch (Exception)
            {
            }

            return refBlastID;
        }

         private void CancelBlast(int blastID)
        {
            SqlCommand cmd;
            string sql = "Update [Blast] set StatusCode='cancelled' where BlastID = @BlastID";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
            DataFunctions.Execute(cmd);

            Removecache(blastID);
        }

        private void SetBlastToError(int blastID)
        {
            SqlCommand cmd;
            string sql = "Update [Blast] set StatusCode='error' where BlastID = @BlastID";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
            DataFunctions.Execute(cmd);
            Removecache(blastID);
        }

        private void SetBlastToNewStatus(int blastID, string newStatus)
        {
            SqlCommand cmd;
            string sql = "Update [Blast] set StatusCode=@NewStatus where BlastID = @BlastID";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
            cmd.Parameters.Add(new SqlParameter("@NewStatus", newStatus));
            DataFunctions.Execute(cmd);

            Removecache(blastID);
        }

        private void SetBlastSingleToError(int blastSingleID)
        {
            SqlCommand cmd;
            string sql = "Update [BlastSingles] set Processed='e' where BlastSingleID = @BlastSingleID";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@BlastSingleID", blastSingleID));
            DataFunctions.Execute(cmd);
        }

        private void SetBlastToNoLicense(int blastID)
        {
            SqlCommand cmd;
            string sql = "Update [Blast] set StatusCode='NOLICENSE' where BlastID = @BlastID";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@BlastID", blastID));
            DataFunctions.Execute(cmd);

            Removecache(blastID);
        }

        private bool GetAvailableLicenses(int blastID)
        {
            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);

            LicenseCheck lc = new LicenseCheck();

            string BlastLicensed = lc.Current(blast.CustomerID.Value.ToString());
            string BlastUsed = lc.Used(blast.CustomerID.Value.ToString());
            string BlastAvailable = lc.Available(blast.CustomerID.Value.ToString());
            bool TestBlast = false;
            if (blast.TestBlast.ToString().ToUpper() == "Y")
            {
                TestBlast = true;
            }
            if (BlastLicensed.ToString().Equals("UNLIMITED"))
            {
                BlastAvailable = "N/A";
            }

            int available = 0;

            switch (BlastAvailable)
            {
                case "N/A":
                    return true;
                case "NO LICENSE":
                    return false;
                default:
                    int filterID = 0;
                    //int.TryParse(blast_info["FilterID"].ToString(), out filterID); -- TODO SUNIL 
                    int smartSegmentID = 0;
                    //int.TryParse(blast.SmartSegmentID); -- TODO SUNIL 
                    int countToSend = EmailFunctions.GetBlastRemainingCount(filterID, smartSegmentID, blast.CustomerID.Value, blast.GroupID.Value, "", blastID.ToString(), TestBlast);
                    available = Convert.ToInt32(BlastAvailable);
                    if (countToSend.ToString() == "0")
                    {
                        return true;
                    }
                    if ((available < 1) || (countToSend > available))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
        }

        private string VerifyForeignKeysExist(int blastID)
        {
            string missingField = "Exception while checking key data";
            string blastType = string.Empty;
            string sql = string.Empty;
            SqlCommand cmd;
            DataTable dt;
            ECN_Framework_Entities.Communicator.Blast blast;

            try
            {
                //check if blast exists
                blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);

                if (blast == null || blast.BlastID == 0)
                {
                    return "Blast ID: " + blastID.ToString();
                }

                blastType = blast.BlastType.ToUpper();

                //check if group exists
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(blast.GroupID.Value);

                if (group == null || group.GroupID == 0)
                {
                    return "Group ID: " + blast.GroupID.Value.ToString();
                }

                ////check if filter exists
                //int filterID = 0;
                //int.TryParse(blastRow["FilterID"].ToString(), out filterID);
                //if (filterID > 0)
                //{
                //    sql = "Select * From Filter Where FilterID = @FilterID and IsDeleted = 0"; ;
                //    cmd = new SqlCommand(sql);
                //    cmd.Parameters.Add(new SqlParameter("@FilterID", Convert.ToInt32(blastRow["FilterID"].ToString())));
                //    dt = DataFunctions.GetDataTable(cmd);
                //    if (dt.Rows.Count <= 0)
                //    {
                //        return "Filter ID: " + blastRow["FilterID"].ToString();
                //    }
                //}

                //check if code exists
                if (blast.CodeID.HasValue && blast.CodeID.Value > 0)
                {
                    sql = "Select * From [Code] Where CodeID = @CodeID and IsDeleted = 0";
                    cmd = new SqlCommand(sql);
                    cmd.Parameters.Add(new SqlParameter("@CodeID", blast.CodeID.Value));
                    dt = DataFunctions.GetDataTable(cmd);
                    if (dt.Rows.Count <= 0)
                    {
                        return "Code ID: " + blast.CodeID.Value.ToString();
                    }
                }

                //check if refblast exists
                string refBlastID = blast.RefBlastID.Trim();
                if (refBlastID != "-1" && refBlastID.Length > 0)
                {
                    char[] splitter = { ',' };
                    string[] splitBlasts = refBlastID.Split(splitter);
                    sql = "Select * From Blast Where BlastID in (" + refBlastID + ")  and StatusCode <> 'Deleted'";
                    cmd = new SqlCommand(sql);
                    //cmd.Parameters.Add(new SqlParameter("@RefBlastID", refBlastID));
                    dt = DataFunctions.GetDataTable(cmd);
                    if (dt.Rows.Count < (splitBlasts.GetUpperBound(0) + 1))
                    {
                        return "Ref Blast ID(s): " + refBlastID;
                    }
                }

                //check if layout exists if not champion blast
                if (blastType != "CHAMPION")
                {
                    #region Not a Champion

                    ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(blast.LayoutID.Value, false);

                    if (layout == null || layout.LayoutID == 0)
                    {
                        return "Layout ID: " + blast.LayoutID.Value;
                    }

                    //check if 1st content slot exists
                    missingField = ContentExists((int)layout.ContentSlot1.Value);

                    //check if other content slots exist
                    for (int i = 2; i < 10; i++)
                    {
                        if (missingField.Length > 0)
                        {
                            break;
                        }
                        int contentID = 0;

                        switch (i)
                        {
                            case 2:
                                contentID = (layout.ContentSlot2.HasValue ? layout.ContentSlot2.Value : 0);
                                break;
                            case 3:
                                contentID = (layout.ContentSlot3.HasValue ? layout.ContentSlot3.Value : 0);
                                break;
                            case 4:
                                contentID = (layout.ContentSlot4.HasValue ? layout.ContentSlot4.Value : 0);
                                break;
                            case 5:
                                contentID = (layout.ContentSlot5.HasValue ? layout.ContentSlot5.Value : 0);
                                break;
                            case 6:
                                contentID = (layout.ContentSlot6.HasValue ? layout.ContentSlot6.Value : 0);
                                break;
                            case 7:
                                contentID = (layout.ContentSlot7.HasValue ? layout.ContentSlot7.Value : 0);
                                break;
                            case 8:
                                contentID = (layout.ContentSlot8.HasValue ? layout.ContentSlot8.Value : 0);
                                break;
                            case 9:
                                contentID = (layout.ContentSlot9.HasValue ? layout.ContentSlot9.Value : 0);
                                break;
                        }

                        if (contentID == 0)
                        {
                            missingField = "";
                        }
                        else
                        {
                            missingField = ContentExists(contentID);
                        }
                    }

                    //check if codesnippets are in the template
                    if (layout.TemplateID.HasValue && layout.TemplateID.Value > 0)
                    {
                        ECN_Framework_Entities.Communicator.Template t = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.Value);
                        List<string> listCheck = new List<string>();
                        listCheck.Add(t.TemplateSource);
                        listCheck.Add(t.TemplateText);

                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.Group.ValidateDynamicStringsForTemplate(listCheck, blast.GroupID.Value, user);
                        }
                        catch (ECN_Framework_Common.Objects.ECNException ecn)
                        {
                            missingField = "Required UDFs do not exist in the group";
                            ECN_Framework_Common.Objects.ECNError error = ecn.ErrorList[0];
                            string UDFs = error.ErrorMessage.Substring(error.ErrorMessage.IndexOf("subject line.") + 13).Replace("<br /> %%", "").Replace("\r\n", "");
                            string[] cleanUDFs = UDFs.Split(new string[] { "%%" }, StringSplitOptions.None);
                            UDFs = "";
                            foreach (string s in cleanUDFs)
                            {
                                UDFs += s + ",";
                            }
                            return "UDF(s): " + UDFs.TrimEnd(',') + " doesn't exist in group";
                        }
                    }
                    #endregion
                }
                else
                {
                    //need to validate AB blast templates for champion

                    blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(blastID, false);

                    if (blast == null || blast.BlastID == 0)
                    {
                        return "Blast ID: " + blastID.ToString();
                    }


                    if (blast.SampleID.HasValue && blast.SampleID.Value > 0)
                    {
                        sql = "Select * From Blast Where SampleID = @SampleID and StatusCode <> 'Deleted' and BlastID <> @ChampionBlastID";
                        cmd = new SqlCommand(sql);
                        cmd.Parameters.Add(new SqlParameter("@SampleID", blast.SampleID.Value));
                        cmd.Parameters.Add(new SqlParameter("@ChampionBlastID", blastID));

                        dt = DataFunctions.GetDataTable(cmd);
                        if (dt.Rows.Count <= 1)
                        {
                            return "Blast ID: " + blastID.ToString();
                        }

                        foreach (DataRow dr in dt.Rows)
                        {
                            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(Convert.ToInt32(dr["LayoutID"].ToString()), false);

                            if (layout == null || layout.LayoutID == 0)
                            {
                                return "Layout ID: " + blast.LayoutID.Value;
                            }

                            if (layout.TemplateID.HasValue && layout.TemplateID.Value > 0)
                            {
                                ECN_Framework_Entities.Communicator.Template t = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.Value);
                                List<string> listCheck = new List<string>();
                                listCheck.Add(t.TemplateSource);
                                listCheck.Add(t.TemplateText);

                                try
                                {
                                    ECN_Framework_BusinessLayer.Communicator.Group.ValidateDynamicStringsForTemplate(listCheck, blast.GroupID.Value, user);
                                }
                                catch (ECN_Framework_Common.Objects.ECNException ecn)
                                {
                                    missingField = "Required UDFs do not exist in the group";
                                    ECN_Framework_Common.Objects.ECNError error = ecn.ErrorList[0];
                                    string UDFs = error.ErrorMessage.Substring(error.ErrorMessage.IndexOf("subject line.") + 13).Replace("<br /> %%", "").Replace("\r\n", "");
                                    string[] cleanUDFs = UDFs.Split(new string[] { "%%" }, StringSplitOptions.None);
                                    UDFs = "";
                                    foreach (string s in cleanUDFs)
                                    {
                                        UDFs += s + ",";
                                    }
                                    return "UDF(s): " + UDFs.TrimEnd(',') + " doesn't exist in group";
                                }
                            }
                        }
                    }
                    missingField = "";
                }
            }
            catch (Exception)
            {
            }

            return missingField;
        }

        private string ContentExists(int contentID)
        {
            string missingField = "Content ID: " + contentID.ToString();
            string sql = "SELECT COUNT(ContentID) FROM Content WHERE ContentID = @ContentID and IsDeleted = 0";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add(new SqlParameter("@ContentID", contentID));
            try
            {
                Int32 count = (Int32)DataFunctions.ExecuteScalar(cmd);
                if (count == 1)
                {
                    ECN_Framework_Entities.Communicator.Content c = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(contentID, false);
                    bool bValid = ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(c.ContentSource);
                    if (bValid)
                        missingField = "";
                    else
                        missingField = "Invalid Content processed in Blast Engine";
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException excEx)
            {
                foreach (ECN_Framework_Common.Objects.ECNError ecnError in excEx.ErrorList)
                {
                    missingField = missingField + "\r\n" + ecnError.Entity + ": " + ecnError.ErrorMessage;
                }
                missingField=missingField.Replace("&lt;", "<").Replace("&gt;", ">");
            }
            catch (Exception excEx)
            { 
                LogCriticalError(excEx, string.Format("Blast engine({0}) encountered an exception when Handle Blast.", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)), Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), string.Format("An exception Happened when handling Blast Engine ID = {5} and Blast ID = {6}{0} Exception Message: {1}{0} Exception Source: {2}{0} Stack Trace: {3}{0} Inner Exception: {4}{0} Validation Error: {7}{0}",
                    string.Format("{0}{1}{0}", Environment.NewLine, new string('-', 80)),
                    excEx.Message, excEx.Source, excEx.StackTrace, excEx.InnerException,
                    _BlastEngineID, _BlastID, "Invalid Content processed in Blast Engine"));
                SetBlastToError(_BlastID);
            }
            return missingField;
        }

        private void HandleSingleBlast(EmailFunctions emailFunctions)
        {
            Guard.NotNull(emailFunctions, nameof(emailFunctions));

            try
            {
                var query = string.Format(
                                    QuerySelectBlastSingles, 
                                    ConfigurationManager.AppSettings[FilterBlastSingle]);
                var table = DataFunctions.GetDataTable(query);
               
                var groupId = 0;
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine(LogMsgGetttingBlastStatus, DateTime.Now);
                    var sqlQuery = string.Format(SqlQueryExecSpGetBlastSingleEmailStatusTemplate, row[FieldBlastSingleId]);
                    if (!row.IsNull(FieldRefBlastId) && 
                        DataFunctions.ExecuteScalar(sqlQuery).ToString() == OneString)
                    {
                        HandleSingleBlastOnRecordPresent(emailFunctions, row);
                    }
                    else
                    {
                        HandleSingleBlastOnNoRecordFound(row);
                    }
                }
            }
            catch (CommonECNException commonException)
            {
                HandleSingleBlastOnException(commonException);
            }
            catch (Exception exception)
            {
                HandleSingleBlastOnException(exception);
            }

            Thread.Sleep(SleepHandleBlast);
        }

        private void HandleSingleBlastOnRecordPresent(EmailFunctions emailFunctions, DataRow dataRow)
        {
            Guard.NotNull(emailFunctions, nameof(emailFunctions));
            Guard.NotNull(dataRow, nameof(dataRow));

            int groupId;
            Console.WriteLine(LogMsgBlastingStatusGood, DateTime.Now);

            var emailId = (int) dataRow[FieldEmailId];
            _BlastID = (int) dataRow[FieldBlastId];

            var blast = BusinessBlast.GetByBlastID_NoAccessCheck(_BlastID, false);

            _CustomerID = blast.CustomerID.Value;
            var channelCheck = new ChannelCheck(_CustomerID);
            _PlanID = (int) dataRow[FieldLayoutPlanId];
            _BlastSingleID = (int) dataRow[FieldBlastSingleId];
            int.TryParse(dataRow[FieldGroupId].ToString(), out groupId);
            var refBlastID = 0;
            int.TryParse(dataRow[FieldRefBlastId].ToString(), out refBlastID);
            HandleSingleBlastLogStart();

            groupId = HandleSingleBlastLogBlastId(refBlastID, emailId, groupId, blast);

            var refTriggerId = GetRefTriggerId();

            if (refTriggerId > 0)
            {
                HandleSingleBlastLogCheckForRefTrigger();

                HandleSingleBlastLogCheckForOpen();

                var querySelectOpensForRefTrigId = QuerySelectOpensForRefTrigId(dataRow, refTriggerId);

                Console.WriteLine(LogMsgCheckIfOpen, DateTime.Now);
                int opensCount;
                int.TryParse(DataFunctions.ExecuteScalar(querySelectOpensForRefTrigId).ToString(), out opensCount);

                if (opensCount == 0)
                {
                    opensCount = HandleSingleBlastOnNoOpensCount(dataRow, refTriggerId, opensCount);
                }

                if (opensCount > 0)
                {
                    HandleSingleBlastDelete();
                }
                else
                {
                    HandleSingleBlastOnNoOpensCountFinally(emailFunctions, dataRow, blast, groupId, channelCheck);
                }
            }
            else
            {
                HandleSingleBlastOnNoRefTrigger(emailFunctions, dataRow, blast, groupId, channelCheck);
            }
        }

        private int GetRefTriggerId()
        {
            var noOpenEmailSelect =
                string.Format("SELECT RefTriggerID FROM TriggerPlans WHERE TriggerPlanID = {0}", _PlanID);
            Console.WriteLine(LogMsgGetRefTriggerId, DateTime.Now);
            int refTriggerId;
            int.TryParse(DataFunctions.ExecuteScalar(noOpenEmailSelect).ToString(), out refTriggerId);
            return refTriggerId;
        }

        private static string QuerySelectOpensForRefTrigId(DataRow dataRow, int refTriggerId)
        {
            Guard.NotNull(dataRow, nameof(dataRow));

            var querySelectOpensForRefTrigId = string.Format(
                " SELECT COUNT(EmailID) AS 'OpensCount' " +
                " FROM ECN_ACTIVITY..BlastActivityOpens " +
                " WHERE BlastID = {0} AND EmailID = {1}",
                refTriggerId,
                dataRow[FieldEmailId]);
            return querySelectOpensForRefTrigId;
        }

        private static int HandleSingleBlastOnNoOpensCount(DataRow dataRow, int refTriggerId, int opensCount)
        {
            Guard.NotNull(dataRow, nameof(dataRow));

            var selectOpensFromEmailActivity = string.Format(
                "SELECT COUNT(EmailID) AS 'OpensCount' " +
                " FROM ECN5_Communicator..EmailActivityLog eal with(nolock) " +
                " WHERE eal.BlastID = {0} AND EmailID = {1} AND ActionTypeCode = 'open'",
                refTriggerId,
                dataRow[FieldEmailId]);

            Console.WriteLine(LogMsgNoOpensInBlastActivity, DateTime.Now);
            int count;
            if (int.TryParse(DataFunctions.ExecuteScalar(selectOpensFromEmailActivity).ToString(), out count))
            {
                opensCount = count;
            }

            return opensCount;
        }

        private void HandleSingleBlastOnNoOpensCountFinally(
            EmailFunctions emailFunctions, 
            DataRow dataRow, 
            BlastAbstract blast, 
            int groupId, 
            ChannelCheck channelCheck)
        {
            Guard.NotNull(emailFunctions, nameof(emailFunctions));
            Guard.NotNull(dataRow, nameof(dataRow));
            Guard.NotNull(channelCheck, nameof(channelCheck));

            Console.WriteLine(string.Empty);
            Console.WriteLine(
                LogMsgSingleBlastBkasdCustomerId,
                _BlastSingleID,
                _BlastID,
                _CustomerID,
                DateTime.Now);

            HandleSingleBlastLogUpdateStartTime();

            var updateStartTime = string.Format(
                "UPDATE BlastSingles SET StartTime = GETDATE() WHERE BlastSingleID = {0}",
                _BlastSingleID);
            DataFunctions.ExecuteScalar(updateStartTime);

            emailFunctions.SendBlastSingle(
                blast,
                (int) dataRow[FieldEmailId],
                groupId,
                ConfigurationManager.AppSettings[AppSettingCommunicatorVirtualPath],
                channelCheck.getHostName(),
                channelCheck.getBounceDomain());
            HandleSingleBlastUpdateBlastSingle();

            DataFunctions.Execute(string.Format(
                "Update BlastSingles set Processed='y', EndTime = GETDATE() where BlastSingleID={0}",
                dataRow[FieldBlastSingleId]));
            DataFunctions.Execute(string.Format(
                "Update [Blast] set SendTime=GetDate() where BlastID={0}",
                dataRow[FieldBlastId]));
            //somewhere earlier this is being set incorrectly as we get multiple records for NEEBO
            var finalSendsObj = DataFunctions.ExecuteScalar(
                DbActivity,
                string.Format(
                    "select count(SendID) from BlastActivitySends where BlastID={0}",
                    _BlastID));
            int finalSends;
            if (!int.TryParse(finalSendsObj?.ToString(), out finalSends))
            {
                throw new InvalidOperationException(ErrorCouldntParseActivitySendCount);
            }

            HandleSingleBlastLogUpdateBlastSendTotal();

            DataFunctions.Execute(string.Format(
                "update [blast] set attempttotal={0},successtotal={0},sendtotal={0} where blastID={1}",
                finalSends, _BlastID));
            Console.WriteLine(string.Empty);
        }

        private static void HandleSingleBlastLogUpdateBlastSendTotal()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActivityUpdateBlastSendTotal,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private void HandleSingleBlastUpdateBlastSingle()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    string.Format(LogActivityUpdateBlastSingle, _BlastSingleID),
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private void HandleSingleBlastLogUpdateStartTime()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    string.Format(LogActivityUpdateStartTime, _BlastSingleID),
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private void HandleSingleBlastDelete()
        {
            HandleSingleBlastLogDeleteNoOpen();

            Console.WriteLine(LogMsgDeleteNoOpenTrigger, DateTime.Now);
            var deleteBlastSingleRecSql =
                string.Format("DELETE FROM BlastSingles WHERE BlastSingleID = {0}", _BlastSingleID);
            DataFunctions.ExecuteScalar(deleteBlastSingleRecSql);
            Console.WriteLine(
                LogMsgHandleSingleBlast,
                _BlastID,
                _CustomerID,
                DateTime.Now);
            Console.WriteLine(string.Empty);
        }

        private static void HandleSingleBlastLogDeleteNoOpen()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActivityDeleteNoOpen,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private void HandleSingleBlastOnNoRefTrigger(
            EmailFunctions emailFunctions, 
            DataRow dataRow, 
            BlastAbstract blast,
            int groupId, 
            ChannelCheck channelCheck)
        {
            Guard.NotNull(emailFunctions, nameof(emailFunctions));
            Guard.NotNull(dataRow, nameof(dataRow));
            Guard.NotNull(blast, nameof(blast));
            Guard.NotNull(channelCheck, nameof(channelCheck));

            Console.WriteLine(string.Empty);
            Console.WriteLine(
                LogMsgSingleBlastBkasdCustomerId,
                _BlastSingleID,
                _BlastID,
                _CustomerID,
                DateTime.Now);
            var updateStartTime = string.Format(
                "UPDATE BlastSingles SET StartTime = GETDATE() WHERE BlastSingleID = {0}",
                _BlastSingleID);
            HandleSingleBlastLogUpdateBlastStartTime();

            DataFunctions.Execute(updateStartTime);
            emailFunctions.SendBlastSingle(
                blast,
                (int) dataRow[FieldEmailId],
                groupId,
                ConfigurationManager.AppSettings[AppSettingCommunicatorVirtualPath],
                channelCheck.getHostName(),
                channelCheck.getBounceDomain());
            HandleSingleBlastLogUpdateBlastSingleEndTime();

            DataFunctions.Execute(string.Format(
                "Update BlastSingles set Processed='y' , EndTime = GETDATE() where BlastSingleID={0}",
                dataRow[FieldBlastSingleId]));
            DataFunctions.Execute(
                string.Format("Update [Blast] set SendTime=GetDate() where BlastID={0}", dataRow[FieldBlastId]));
            //somewhere earlier this is being set incorrectly as we get multiple records for NEEBO
            var finalSendsObj = DataFunctions.ExecuteScalar(
                DbActivity,
                string.Format(
                    "select count(SendID) from BlastActivitySends where BlastID={0}",
                    _BlastID));
            int finalSends;
            if (!int.TryParse(finalSendsObj?.ToString(), out finalSends))
            {
                throw new InvalidOperationException(ErrorCouldntParseActivitySendCount);
            }

            HandleSingleBlastSendTotal();

            DataFunctions.Execute(string.Format(
                "update [blast] set attempttotal={0},successtotal={0},sendtotal={0} where blastID={1}",
                finalSends,
                _BlastID));
            Console.WriteLine(string.Empty);
        }

        private static void HandleSingleBlastLogCheckForOpen()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActovityCheckForOpen,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private static void HandleSingleBlastSendTotal()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActivityUpdateBlastSendTotal,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private static void HandleSingleBlastLogUpdateBlastSingleEndTime()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActivityUpdateBlastSingleEndTime,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private static void HandleSingleBlastLogUpdateBlastStartTime()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActivityUpdateBlastSingleStartTime,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private static void HandleSingleBlastLogCheckForRefTrigger()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    LogActiviryCheckForRefTrigger,
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private void HandleSingleBlastLogStart()
        {
            if (CommonLoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    string.Format(LogActivityStartingBlastSingle, _BlastSingleID),
                    string.Format(
                        LogActivityStatisticsPrefixTemplate,
                        IoPath.GetFileNameWithoutExtension(
                            Process.GetCurrentProcess().MainModule.FileName),
                        DateTime.Now.ToShortDateString().Replace(DelimSlash, DelimHypen)));
            }
        }

        private int HandleSingleBlastLogBlastId(int refBlastId, int emailId, int groupId, BlastAbstract blast)
        {
            if (refBlastId > 0)
            {
                Console.WriteLine(LogMsgGetRefBlast, DateTime.Now);
                var refBlast = BusinessBlast.GetMasterRefBlast(_BlastID, emailId, user, false);

                if (refBlast != null)
                {
                    groupId = refBlast.GroupID.Value;
                    blast.GroupID = refBlast.GroupID;
                }
            }

            return groupId;
        }

        private void HandleSingleBlastOnNoRecordFound(DataRow row)
        {
            Guard.NotNull(row, nameof(row));

            Console.WriteLine(string.Empty);
            Console.WriteLine(
                LogMsgSingleBlastBkasdCustomerId,
                _BlastSingleID,
                _BlastID,
                _CustomerID,
                DateTime.Now);
            Console.WriteLine(
                ErrorCancelledUnsubscribed);
            DataFunctions.Execute(
                string.Format(
                    "Update BlastSingles set Processed='U' where BlastSingleID={0}",
                    row[FieldBlastSingleId]));
            Console.WriteLine(string.Empty);
        }

        private void HandleSingleBlastOnException(CommonECNException commonException)
        {
            Guard.NotNull(commonException, nameof(commonException));

            var noteBuilder = new StringBuilder();
            foreach (var ecnError in commonException.ErrorList)
            {
                noteBuilder.AppendFormat(NoteTemplate, ecnError.Entity, ecnError.ErrorMessage);
            }
            var note = noteBuilder.ToString();

            int applicationId;
            int.TryParse(ConfigurationManager.AppSettings[AppSettingKmCommonApplication], out applicationId);

            var idPostfix = string.Concat(
                Environment.NewLine,
                new string(DelimHypenChar, HyphenLen),
                Environment.NewLine);
            LogCriticalError(
                commonException,
                string.Format(
                    ErrorBlastEngineEncounteredAnException,
                    IoPath.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)),
                applicationId,
                string.Format(
                    CriticalErrorTemplate,
                    idPostfix,
                    commonException.Message,
                    commonException.Source,
                    commonException.StackTrace,
                    commonException.InnerException,
                    _BlastEngineID,
                    _BlastSingleID,
                    note)
            );
            SetBlastSingleToError(_BlastSingleID);
            Console.WriteLine(commonException.Message);
        }

        private void HandleSingleBlastOnException(Exception exception)
        {
            Guard.NotNull(exception, nameof(exception));

            int applicationId;
            int.TryParse(ConfigurationManager.AppSettings[AppSettingKmCommonApplication], out applicationId);
            var idPostfix = string.Concat(
                Environment.NewLine,
                new string(DelimHypenChar, HyphenLen),
                Environment.NewLine);
            LogCriticalError(
                exception,
                string.Format(
                    ErrorSingleBlastEngineException,
                    IoPath.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)),
                applicationId,
                string.Format(
                    CommonExceptionErrorTemplate,
                    idPostfix,
                    exception.Message,
                    exception.Source,
                    exception.StackTrace,
                    exception.InnerException,
                    _BlastEngineID,
                    _BlastSingleID));
            SetBlastSingleToError(_BlastSingleID);
            Console.WriteLine(exception.Message);
        }

        private void SendEmailNotification(string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["AdminSendFrom"]);
            message.To.Add(ConfigurationManager.AppSettings["AdminSendTo"]);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }

        private void SendAccountManagersEmailNotification(string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["AdminSendFrom"]);
            message.To.Add(ConfigurationManager.AppSettings["AcctManagersSendTo"]);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }
    }
}