using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.IO.Fakes;
using System.IO;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using System.Data;
using WATT_NXTBook_Engine.Fakes;
using Shouldly;
using KM.Common.Entity.Fakes;

namespace WATT_NXTBook_Engine.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        private IDisposable _shimObject;
        private readonly NameValueCollection _appSettings = new NameValueCollection();
        private const string _logPath = "LogPath";
        private const string _accessKey = "651A1297-59D1-4857-93CB-0B049635762E";
        private const string _doFullSync = "true";
        private const string _kmCommon_Application = "87";
        private const string _doFullNXTBookSync = "true";
        private const string _subGroup = "34307";
        private const string _methodName = "Main";

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            ShimFileStream.ConstructorStringFileMode = (fs, file, mode) => { };
            ShimStreamWriter.ConstructorStream = (sw, stream) => { };

            _appSettings.Add("LogPath", _logPath);
            _appSettings.Add("AccessKey", _accessKey);
            _appSettings.Add("DoFullSync", _doFullSync);
            _appSettings.Add("SubGroups", _subGroup);
            _appSettings.Add("KMCommon_Application", _kmCommon_Application);
            _appSettings.Add("DoFullNXTBookSync", _doFullNXTBookSync);
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        [TearDown]
        public void TestCleanUp()
        {
            _appSettings.Clear();
            _shimObject.Dispose();
        }

        [Test]
        public void Main_NoJob1AndNojob2_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.FalseString.ToLower();
            var _doJob2 = bool.FalseString.ToLower();
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            var dataLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Skipping Job1") || log.Contains("Skipping Job2"))
                {
                    dataLogged = true;
                }
            };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataLogged.ShouldBeTrue();
        }

        [Test]
        public void Main_Job1IsTrueAndNojob2_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.TrueString.ToLower();
            var _doJob2 = bool.FalseString.ToLower();
            var targetGroup = $"Sub_{_subGroup}";
            var field = "DEMO7";
            var fieldValue = "B";
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            _appSettings.Add("Field", field);
            _appSettings.Add("FieldValue", fieldValue);
            _appSettings.Add(targetGroup, _subGroup);
            var dataImported = false;
            var dataLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Starting Job 1"))
                {
                    dataLogged = true;
                }
            };

            ShimEmail.GetEmailsForWATT_NXTBookSyncInt32BooleanNullableOfDateTimeStringStringBoolean =
                (groupID, job1, dateFrom, Field, FieldValue, DoFullNXTBookSync) =>
                {
                    return new DataTable();
                };
            ShimProgram.DoImportDataTableInt32 = (table, id) =>
            {
                dataImported = true;
            };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataImported.ShouldBeTrue();
            dataLogged.ShouldBeTrue();
        }

        [Test]
        public void Main_Job1IsTrueAndImportException_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.TrueString.ToLower();
            var _doJob2 = bool.FalseString.ToLower();
            var targetGroup = $"Sub_{_subGroup}";
            var field = "DEMO7";
            var fieldValue = "B";
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            _appSettings.Add("Field", field);
            _appSettings.Add("FieldValue", fieldValue);
            _appSettings.Add(targetGroup, _subGroup);
            var dataImported = false;
            var dataLogged = false;
            var exceptionLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Starting Job 1"))
                {
                    dataLogged = true;
                }
            };

            ShimEmail.GetEmailsForWATT_NXTBookSyncInt32BooleanNullableOfDateTimeStringStringBoolean =
                (groupID, job1, dateFrom, Field, FieldValue, DoFullNXTBookSync) =>
                {
                    throw new Exception(nameof(Exception));
                };
            ShimProgram.DoImportDataTableInt32 = (table, id) =>
            {
                dataImported = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, ourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataImported.ShouldBeFalse();
            dataLogged.ShouldBeTrue();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void Main_Job1IsTrueAndFieldsNotFound_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.TrueString.ToLower();
            var _doJob2 = bool.FalseString.ToLower();
            var targetGroup = $"Sub_{_subGroup}";
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            _appSettings.Add(targetGroup, _subGroup);
            var dataImported = false;
            var dataLogged = false;
            var exceptionLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Starting Job 1"))
                {
                    dataLogged = true;
                }
            };

            ShimEmail.GetEmailsForWATT_NXTBookSyncInt32BooleanNullableOfDateTimeStringStringBoolean =
                (groupID, job1, dateFrom, Field, FieldValue, DoFullNXTBookSync) =>
                {
                    throw new Exception(nameof(Exception));
                };
            ShimProgram.DoImportDataTableInt32 = (table, id) =>
            {
                dataImported = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, ourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataImported.ShouldBeFalse();
            dataLogged.ShouldBeTrue();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void Main_Job2IsTrueAndNojob2_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.FalseString.ToLower();
            var _doJob2 = bool.TrueString.ToLower();
            var targetGroup = $"Sub_{_subGroup}";
            var field = "DEMO7";
            var fieldValue = "B";
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            _appSettings.Add("Field2", field);
            _appSettings.Add("FieldValue2", fieldValue);
            _appSettings.Add(targetGroup, _subGroup);
            var dataImported = false;
            var dataLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Starting Job 2"))
                {
                    dataLogged = true;
                }
            };

            ShimEmail.GetEmailsForWATT_NXTBookSyncInt32BooleanNullableOfDateTimeStringStringBoolean =
                (groupID, job1, dateFrom, Field, FieldValue, DoFullNXTBookSync) =>
                {
                    return new DataTable();
                };
            ShimProgram.DoNXTBookPostDataTableInt32 = (table, id) =>
            {
                dataImported = true;
            };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataImported.ShouldBeTrue();
            dataLogged.ShouldBeTrue();
        }

        [Test]
        public void Main_Job2IsTrueAndImportException_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.FalseString.ToLower();
            var _doJob2 = bool.TrueString.ToLower();
            var targetGroup = $"Sub_{_subGroup}";
            var field = "DEMO7";
            var fieldValue = "B";
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            _appSettings.Add("Field2", field);
            _appSettings.Add("FieldValue2", fieldValue);
            _appSettings.Add(targetGroup, _subGroup);
            var dataImported = false;
            var dataLogged = false;
            var exceptionLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Starting Job 2"))
                {
                    dataLogged = true;
                }
            };

            ShimEmail.GetEmailsForWATT_NXTBookSyncInt32BooleanNullableOfDateTimeStringStringBoolean =
                (groupID, job1, dateFrom, Field, FieldValue, DoFullNXTBookSync) =>
                {
                    throw new Exception(nameof(Exception));
                };
            ShimProgram.DoNXTBookPostDataTableInt32 = (table, id) =>
            {
                dataImported = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, ourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataImported.ShouldBeFalse();
            dataLogged.ShouldBeTrue();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void Main_Job2IsTrueAndFieldsNotFound_ReachEnd()
        {
            // Arrange
            var _doJob1 = bool.FalseString.ToLower();
            var _doJob2 = bool.TrueString.ToLower();
            var targetGroup = $"Sub_{_subGroup}";
            _appSettings.Add("DoJob1", _doJob1);
            _appSettings.Add("DoJob2", _doJob2);
            _appSettings.Add(targetGroup, _subGroup);
            var dataImported = false;
            var dataLogged = false;
            var exceptionLogged = false;
            ShimProgram.writeToLogString = (log) =>
            {
                if (log.Contains("Starting Job 2"))
                {
                    dataLogged = true;
                }
            };

            ShimEmail.GetEmailsForWATT_NXTBookSyncInt32BooleanNullableOfDateTimeStringStringBoolean =
                (groupID, job1, dateFrom, Field, FieldValue, DoFullNXTBookSync) =>
                {
                    throw new Exception(nameof(Exception));
                };
            ShimProgram.DoNXTBookPostDataTableInt32 = (table, id) =>
            {
                dataImported = true;
            };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, ourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    exceptionLogged = true;
                    return 1;
                };

            // Act
            typeof(Program).CallMethod(_methodName, new object[] { new string[0] }, null);

            //Assert
            dataImported.ShouldBeFalse();
            dataLogged.ShouldBeTrue();
            exceptionLogged.ShouldBeTrue();
        }
    }
}
