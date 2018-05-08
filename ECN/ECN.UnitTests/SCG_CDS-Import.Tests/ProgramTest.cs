using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using System.Reflection;
using KM.Common.Entity.Fakes;
using KM.Common.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SCG_CDS_Import.Fakes;
using SCG_CDS_Import.Tests.Setup;

namespace SCG_CDS_Import.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ProgramTest
    {
        private IDisposable _shimsContext;
        private NameValueCollection _fakedAppSettings;
        private ProgramTestContext _programTestContext;
        private DateTime _now;

        [SetUp]
        public void Setup()
        {
            _fakedAppSettings = new NameValueCollection();
            _now = DateTime.Now;
            _programTestContext = new ProgramTestContext();
            _shimsContext = ShimsContext.Create();
            ShimConfigurationManager.AppSettingsGet = ConfigurationManagerAppSettingsGet;
            ShimProgram.UpdateToDbStringStringImportFile = ProgramUpdateToDB;
            ShimProgram.WriteToImportFileLogString = ProgramWriteToImportFileLog;
            ShimEmailFunctions.NotifyAdminStringString = EmailFunctionsNotifyAdmin;
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                ApplicationLogLogCriticalErrorException;
            ShimDateTime.NowGet = DateTimeNowGet;
            ShimProgram.WriteToLogString = ProgramWriteToLog;
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        private DateTime DateTimeNowGet()
        {
            return _now;
        }

        private NameValueCollection ConfigurationManagerAppSettingsGet()
        {
            var exception = _programTestContext.AppSettingsExceptionToThrow;
            if (exception != null)
            {
                _programTestContext.AppSettingsExceptionToThrow = null;
                throw exception;
            }
            var realAppSettings = ShimsContext.ExecuteWithoutShims(() => ConfigurationManager.AppSettings);
            var result = new NameValueCollection();
            result.Add(realAppSettings);
            result.Add(_fakedAppSettings);
            return result;
        }

        private void ProgramWriteToImportFileLog(string text)
        {
            _programTestContext.ImportFileLogs.Add(text);
        }

        private void ProgramUpdateToDB(string xmlProfile, string xmlUDF, ImportFile importFile)
        {
            _programTestContext.UpdateToDBXmlProfile.Add(xmlProfile);
            _programTestContext.UpdateToDBXmlUDF.Add(xmlUDF);
            _programTestContext.UpdateToDBImportFile.Add(importFile);
        }

        private string EmailFunctionsNotifyAdmin(string subject, string textMessage)
        {
            _programTestContext.EmailFunctionsNotifyAdminSubject = subject;
            _programTestContext.EmailFunctionsNotifyAdminTextMessage = textMessage;
            return _programTestContext.EmailFunctionsNotifyAdminReturnValue;
        }

        private int ApplicationLogLogCriticalErrorException(
            Exception exception,
            string sourceMethod,
            int applicationID,
            string note,
            int gdCharityID,
            int ecnCustomerID)
        {
            _programTestContext.LogCriticalErrorException = exception;
            _programTestContext.LogCriticalErrorSourceMethod = sourceMethod;
            _programTestContext.LogCriticalErrorApplicationID = applicationID;
            _programTestContext.LogCriticalErrorNote = note;
            _programTestContext.LogCriticalErrorGDCharityID = gdCharityID;
            _programTestContext.LogCriticalErrorECNCustomerID = ecnCustomerID;
            return _programTestContext.LogCriticalErrorReturnValue;
        }

        private void ProgramWriteToLog(string log)
        {
            _programTestContext.Logs.Add(log);
        }

        private string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        private void SetField(string fieldName, object value)
        {
            var caller = new PrivateObject(typeof(Program));
            caller.SetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static, value);
        }
    }
}
