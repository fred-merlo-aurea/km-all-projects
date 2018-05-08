using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Net.Mail.Fakes;
using System.Data;
using ecn.communicator.classes.Fakes;
using ecn.common.classes.Fakes;
using ecn.blastengine.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework.Common.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions.Fakes;
using KM.Common.Entity.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CommunicatorClasses = ecn.communicator.classes;
using KMCommonFakes = KM.Common.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.BlastEngine.Tests
{
    public partial class BlastEngineTest
    {
        private const string UTException = "UT Exception";
        private const string StatusHold = "HOLD";
        private const string SampleMissingField = "SampleMissingField";
        private const string HandleBlastMethodName = "HandleBlast";
        private StringBuilder ConsoleActivityLogFileText;
        private StringBuilder ActivityLogFileText;
        private string _sqlCommandExceuted;
        private MailMessage _mailMessage;
        private bool _isCacheRemoved = false;
        private bool _isCacheCreated = false;
        private bool _isBlastCloned = false;
        private bool _isBlastSend = false;

        [Test]
        public void HandleBlast_WhenBlastIDFetchQueryAndLoggingThrowsException_ExceptionLoggedAtConsole()
        {
            // Arrange
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            ConsoleActivityLogFileText = new StringBuilder();
            KMCommonFakes.ShimFileFunctions.LogConsoleActivityStringString = (message, suffix) =>
            {
                ConsoleActivityLogFileText.AppendLine($"{suffix}{message}");
            };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            ConsoleActivityLogFileText.ShouldSatisfyAllConditions(
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Error logging critical issue "),
                () => ConsoleActivityLogFileText.ToString().ShouldContain($"BlastLog_BlastID:{int.MinValue}"),
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Original Error Stack:"),
                () => ConsoleActivityLogFileText.ToString().
                    ShouldContain("BlastLog_System.InvalidOperationException: The ConnectionString property has not been initialized."),
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Exception stack trace:"),
                () => ConsoleActivityLogFileText.ToString().
                    ShouldContain("BlastLog_System.NullReferenceException: Object reference not set to an instance of an object."));
        }

        [Test]
        public void HandleBlast_WhenCreateCacheThrowsException_ExceptionLoggedAtConsole()
        {
            // Arrange
            var isCacheRemoved = false;
            const int blastId = 1;
            const int customerId = 1;
            var sqlCommandExceuted = string.Empty;
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            _mailMessage = null;
            ShimDataFunctions.ExecuteScalarSqlCommand = (cmd) => "0";
            ShimECNBlastEngine.AllInstances.RemovecacheInt32 = (b, bid) => { isCacheRemoved = true; };
            ShimECNBlastEngine.AllInstances.CreateCacheInt32 = (b, bid) =>
                throw new BlastHoldException(UTException, new InvalidOperationException(), StatusHold, blastId, customerId);

            ShimDataFunctions.ExecuteSqlCommand = (cmd) => 
            {
                sqlCommandExceuted = cmd.CommandText;
                return 1;
            };
            ShimSmtpClient.AllInstances.SendMailMessage = (smtp, msg) => { _mailMessage = msg; };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            sqlCommandExceuted.ShouldSatisfyAllConditions(
                () => sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => sqlCommandExceuted.ShouldContain("Update [Blast] set StatusCode=@NewStatus where BlastID = "),
                () => _mailMessage.ShouldNotBeNull(),
                () => _mailMessage.From.ToString().ShouldBe(FromEmailAddress),
                () => _mailMessage.To.Count.ShouldBe(1),
                () => _mailMessage.To.ShouldContain(new MailAddress(ToEmailAddress)),
                () => _mailMessage.Body.ShouldContain($"Blast Issue for Customer: {customerId}, Blast ID: {blastId} Has been set to {StatusHold}"),
                () => _mailMessage.Body.ShouldContain(UTException),
                () => _mailMessage.Subject.ShouldContain($"Blast Issue for Customer: {customerId}, Blast ID: {blastId} Has been set to {StatusHold}"),
                () => isCacheRemoved.ShouldBeTrue());
        }

        [Test]
        public void HandleBlast_WhenCreateCacheThrowsECNException_ExceptionLoggedAtConsole()
        {
            // Arrange
            var isCacheRemoved = false;
            var exceptionLogged = false;
            _sqlCommandExceuted = string.Empty;
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            ShimDataFunctions.ExecuteScalarSqlCommand = (cmd) => "0";
            ShimECNBlastEngine.AllInstances.RemovecacheInt32 = (b, bid) => { isCacheRemoved = true; };
            ShimECNBlastEngine.AllInstances.CreateCacheInt32 = (b, bid) =>
                throw new ECNException(new List<ECNError>
                {
                    new ECNError{ ErrorMessage = UTException }
                });
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimDataFunctions.ExecuteSqlCommand = (cmd) =>
            {
                _sqlCommandExceuted = cmd.CommandText;
                return 1;
            };

            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            exceptionLogged.ShouldSatisfyAllConditions(
                () => exceptionLogged.ShouldBeTrue(),
                () => isCacheRemoved.ShouldBeTrue(),
                () => _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExceuted.ShouldContain("Update [Blast] set StatusCode='error' where BlastID = "));
        }

        [Test]
        public void HandleBlast_WhenCreateCacheThrowsGeneralException_ExceptionLoggedAtConsole()
        {
            // Arrange
            var isCacheRemoved = false;
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            ConsoleActivityLogFileText = new StringBuilder();
            KMCommonFakes.ShimFileFunctions.LogConsoleActivityStringString = (message, suffix) =>
            {
                ConsoleActivityLogFileText.AppendLine($"{suffix}{message}");
            };
            ShimDataFunctions.ExecuteScalarSqlCommand = (cmd) => "0";
            ShimECNBlastEngine.AllInstances.RemovecacheInt32 = (b, bid) => { isCacheRemoved = true; };
            ShimECNBlastEngine.AllInstances.CreateCacheInt32 = (b, bid) =>
                throw new InvalidOperationException(UTException);
            
            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            ConsoleActivityLogFileText.ShouldSatisfyAllConditions(
                () => isCacheRemoved.ShouldBeTrue(),
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Error logging critical issue "),
                () => ConsoleActivityLogFileText.ToString().ShouldContain($"BlastLog_BlastID:{0}"),
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Original Error Stack:"),
                () => ConsoleActivityLogFileText.ToString().
                    ShouldContain($"BlastLog_System.InvalidOperationException: {UTException}"),
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Exception stack trace:"),
                () => ConsoleActivityLogFileText.ToString().
                    ShouldContain("BlastLog_System.NullReferenceException: Object reference not set to an instance of an object."),
                () => ConsoleActivityLogFileText.ToString().ShouldContain("BlastLog_Error setting blast status to error"),
                () => ConsoleActivityLogFileText.ToString().
                    ShouldContain("BlastLog_System.InvalidOperationException: The ConnectionString property has not been initialized."));
        }

        [Test]
        public void HandleBlast_WithBlastIDHavingNoLicense_HandlesBlastSuccessfully()
        {
            // Arrange
            SetFakesForHandleBlastMethod();
            int blastId = 1, customerId = 1;
            SetECNBlastEngineFakes();
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            
            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            _isCacheCreated.ShouldSatisfyAllConditions(
                () => _isCacheCreated.ShouldBeTrue(),
                () => _isCacheRemoved.ShouldBeTrue(),
                () => _isBlastCloned.ShouldBeFalse(),
                () => _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExceuted.ShouldContain("Update [Blast] set StatusCode='NOLICENSE' where BlastID = "),
                () => _mailMessage.From.ToString().ShouldBe(FromEmailAddress),
                () => _mailMessage.To.Count.ShouldBe(1),
                () => _mailMessage.To.ShouldContain(new MailAddress(ToEmailAddress)),
                () => _mailMessage.Body.ShouldContain($"No licenses available. Blast has been set to NOLICENSE."),
                () => _mailMessage.Subject.ShouldContain($"Blast Issue for Customer: {customerId}, Blast ID: {blastId}"));
        }

        [Test]
        public void HandleBlast_WithBlastIDWithAvailableLicenses_HandlesBlastSuccessfully()
        {
            // Arrange
            const int blastId = 1;
            SetFakesForHandleBlastMethod();
            SetECNBlastEngineFakes(isLicenseAvailable: true);
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            
            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            _isCacheCreated.ShouldSatisfyAllConditions(
                () => _isCacheCreated.ShouldBeTrue(),
                () => _isCacheRemoved.ShouldBeTrue(),
                () => _isBlastSend.ShouldBeTrue(),
                () => _isBlastCloned.ShouldBeTrue(),
                () => _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExceuted.ShouldContain($"update blast set starttime = getdate() where blastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"Update Blast set StatusCode='Active' where BlastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"update blast set attempttotal=1,successtotal=1,sendtotal=1 where blastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain("select count(EAID) from emailactivitylog where "+ 
                                                            $"actiontypecode in ('send','testsend') and blastID={blastId}"));
        }

        [Test]
        public void HandleBlast_WithBlastIDWithMissingField_HandlesBlastSuccessfully()
        {
            // Arrange
            const int blastId = 1;
            const int customerId = 1;
            SetFakesForHandleBlastMethod();
            SetECNBlastEngineFakes(missingField: SampleMissingField, isLicenseAvailable: true);
            var emailFunctions = new CommunicatorClasses.EmailFunctions();
            
            // Act
            _privateECNBlastEngineObj.Invoke(HandleBlastMethodName, emailFunctions);

            // Assert
            _isCacheRemoved.ShouldSatisfyAllConditions(
                () => _isCacheRemoved.ShouldBeTrue(),
                () => _isCacheCreated.ShouldBeTrue(),
                () => _isBlastCloned.ShouldBeFalse(),
                () => _isBlastSend.ShouldBeFalse(),
                () => _sqlCommandExceuted.ShouldNotBeNullOrWhiteSpace(),
                () => _sqlCommandExceuted.ShouldContain($"update blast set starttime = getdate() where blastID={blastId}"),
                () => _sqlCommandExceuted.ShouldContain($"Update [Blast] set StatusCode='cancelled' where BlastID ="),
                () => _mailMessage.From.ToString().ShouldBe(FromEmailAddress),
                () => _mailMessage.To.Count.ShouldBe(1),
                () => _mailMessage.To.ShouldContain(new MailAddress(ToEmailAddress)),
                () => _mailMessage.Body.ShouldContain($"Blast has been cancelled."),
                () => _mailMessage.Body.ShouldContain($"Key data is missing - {SampleMissingField}."),
                () => _mailMessage.Subject.ShouldContain($"Blast Issue for Customer: {customerId}, Blast ID: {blastId}"));
        }

        private void SetECNBlastEngineFakes(string missingField = "",bool isLicenseAvailable = false)
        {
            _isBlastCloned = false;
            _isCacheCreated = false;
            _isCacheRemoved = false;
            ShimECNBlastEngine.AllInstances.RemovecacheInt32 = (b, bid) => { _isCacheRemoved = true; };
            ShimECNBlastEngine.AllInstances.CreateCacheInt32 = (b, bid) => { _isCacheCreated = true; };
            ShimECNBlastEngine.AllInstances.VerifyForeignKeysExistInt32 = (b, bid) => missingField;
            ShimECNBlastEngine.AllInstances.GetAvailableLicensesInt32 = (b, bid) => isLicenseAvailable;
            ShimECNBlastEngine.AllInstances.CloneBlastInt32BlastSetupInfo = (b, bid, info) => { _isBlastCloned = true; };
        }

        private void SetFakesForHandleBlastMethod()
        {
            ConsoleActivityLogFileText = new StringBuilder();
            ActivityLogFileText = new StringBuilder();
            _sqlCommandExceuted = string.Empty;
            _mailMessage = null;
            _isBlastSend = false;
            KMCommonFakes.ShimFileFunctions.LogConsoleActivityStringString = (message, suffix) =>
            {
                ConsoleActivityLogFileText.AppendLine($"{suffix}{message}");
            };
            KMCommonFakes.ShimFileFunctions.LogActivityBooleanStringString = (toConsole, message, suffix) =>
            {
                ActivityLogFileText.AppendLine(message);
            };
            ShimDataFunctions.ExecuteScalarSqlCommand = (cmd) =>
            {
                if(cmd.CommandText.Contains("SELECT TOP 1 BlastID"))
                {
                    return "1";
                }
                return "0";
            };

            ShimLoggingFunctions.LogStatistics = () => true;
            ShimDataFunctions.ExecuteSqlCommand = (cmd) =>
            {
                _sqlCommandExceuted += cmd.CommandText;
                return 1;
            };
            ShimDataFunctions.ExecuteString = (query) =>
            {
                _sqlCommandExceuted += query;
                return 1;
            };
            ShimDataFunctions.ExecuteScalarString = (query) =>
            {
                _sqlCommandExceuted += query;
                return 1;
            };

            ShimDataFunctions.ExecuteScalarStringString = (db, query) =>
            {
                _sqlCommandExceuted += query;
                return 1;
            };

            ShimBlasts.AllInstances.CustomerID = (b) => 1;
            ShimSmtpClient.AllInstances.SendMailMessage = (smtp, msg) => { _mailMessage = msg; };
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimBaseChannel.GetByBaseChannelIDInt32 = (cid) =>
                new ECN_Framework_Entities.Accounts.BaseChannel { };

            ShimEmailFunctions.AllInstances.SendBlastInt32StringStringString = (ef, id, path, host, domain) => { _isBlastSend = true; };
            ShimEmailFunctions.AllInstances.SendBlastSingleBlastInt32Int32StringStringString = 
                (b, bid, id, q, path, host, domain) => { _isBlastSend = true; };
            ShimBlastSetupInfo.GetNextScheduledBlastSetupInfoInt32 = (id) =>
                new BlastSetupInfo { };

            ShimChannelCheck.AllInstances.ChannelIDGet = (c) => 1;

            ShimDataFunctions.GetDataTableString = (cmd) =>
            {
                var table = GetDataTable();
                table.Rows[0]["refBlastID"] = DBNull.Value;
                return table;
            };
            
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return GetBlast();
            };

            ShimBlast.GetMasterRefBlastInt32Int32UserBoolean = (id, emailId, user, child) =>
            {
                return GetBlast();
            };
        }

        private DataTable GetDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("EmailID", typeof(int));
            table.Columns.Add("BlastID", typeof(int));
            table.Columns.Add("LayoutPlanID", typeof(int));
            table.Columns.Add("BlastSingleID", typeof(int));
            table.Columns.Add("GroupID", typeof(int));
            table.Columns.Add("refBlastID", typeof(int));
            var row = table.NewRow();
            row[0] = 1;
            row[1] = 1;
            row[2] = 1;
            row[3] = 1;
            row[4] = 1;
            row[5] = 1;
            table.Rows.Add(row);
            return table;
        }

        private BlastRegular GetBlast()
        {
            var blast = new BlastRegular
            {
                BlastID = 1,
                GroupID = 1,
                BlastType = BlastType.HTML.ToString(),
                CustomerID = 1
            };
            return blast;
        }
    }
}
