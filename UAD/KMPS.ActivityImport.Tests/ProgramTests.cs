using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using KMPS.ActivityImport.Entity;
using KMPS.ActivityImport.Entity.Fakes;
using KMPS.ActivityImport.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static KMPS.MD.Objects.Enums;

namespace KMPS.ActivityImport.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class ProgramTests
    {
        private const string TestExceptionMessage = "Test Exception";
        private const string InnerExceptionMessage = "Inner Exception";
        private const string MethodImportData = "ImportData";
        private const string CurrentCustomer = "CurrentCustomer";
        private const string WriteXMLProperty = "WriteXML";
        private const string DummyString = "DummyString";
        private const int DummyId = 1;
        private const string ClickFileImports = "ClickFileImports";
        private const string OpenFileImports = "OpenFileImports";
        private const string TopicFileImports = "TopicFileImports";
        private const string VisitFileImports = "VisitFileImports";
        private const string StatusUpdateFileImports = "StatusUpdateFileImports";
        public readonly string ExpectedException = "**********************" + Environment.NewLine +
                                                   "-- Data --" + Environment.NewLine +
                                                   "-- Message --" + Environment.NewLine +
                                                   "Test Exception" + Environment.NewLine +
                                                   "-- InnerException --" + Environment.NewLine +
                                                   "System.Exception: Inner Exception" + Environment.NewLine +
                                                   "**********************" + Environment.NewLine;

        private Program _target;
        private IDisposable _shims;
        private PrivateObject _privateObject;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            ConfigurationManager.AppSettings["KMCommon_Application"] = "1";
            _target = new Program();
            _privateObject = new PrivateObject(_target);
        }

        [TearDown]
        public void TearDown()
        {
            if (_shims != null)
            {
                _shims.Dispose();
            }
        }
        
        [Test]
        public void LogCustomerExeception_ExceptionWithInnerException_LogCorrectString()
        {
            // Arrange
            var innerException = new Exception(InnerExceptionMessage);
            var exception = new Exception(TestExceptionMessage, innerException);
            var expectedLog = string.Empty;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };

            // Act
            _target.LogCustomerExeception(exception, string.Empty);

            // Assert
            expectedLog.ShouldBe(ExpectedException);
        }

        [Test]
        public void LogMainExeception_ExceptionWithInnerException_LogCorrectString()
        {
            // Arrange
            var innerException = new Exception(InnerExceptionMessage);
            var exception = new Exception(TestExceptionMessage, innerException);
            var expectedLog = string.Empty;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.MainLogWriteString = (_, s) => { expectedLog = s; };

            // Act
            _target.LogMainExeception(exception, string.Empty);

            // Assert
            expectedLog.ShouldBe(ExpectedException);
        }

        [Test]
        [TestCase(ProcessType.ClickImport)]
        [TestCase(ProcessType.OpenImport)]
        [TestCase(ProcessType.TopicImport)]
        [TestCase(ProcessType.StatusUpdateImport)]
        [TestCase(ProcessType.VisitImport)]
        public void ImportData_OnValidCall_ImportDataAndWriteLog(ProcessType processType)
        {
            // Arrange
            var isWriteLog = false;
            var isImportCalled = false;
            var process = new Process
            {
                ProcessType = processType.ToString()
            };
            SetupForImportData();
            var xmlString = string.Empty;
            ShimProgram.AllInstances.CustomerLogWriteString = (_, __) => { isWriteLog = true; };
            Action<string> verifyImportAction = (xmlStr) =>
            {
                xmlString = xmlStr;
                isImportCalled = true;
            };
            ShimSubscriberClickActivity.ImportStringString = (xmlStr, __) => verifyImportAction(xmlStr);
            ShimSubscriberOpenActivity.ImportStringString = (xmlStr, __) => verifyImportAction(xmlStr);
            ShimTopicActivity.ImportStringString = (xmlStr, __) => verifyImportAction(xmlStr);
            ShimSubscriberVisitActivity.ImportStringString = (xmlStr, __) => verifyImportAction(xmlStr);
            ShimSubscriberStatusUpdate.ImportStringString = (xmlStr, __) => verifyImportAction(xmlStr);

            // Act	
            _privateObject.Invoke(MethodImportData, process);

            // Assert
            _privateObject.ShouldSatisfyAllConditions(
                () => isWriteLog.ShouldBeTrue(),
                () => isImportCalled.ShouldBeTrue(),
                () => xmlString.ShouldContain(DummyString),
                () => xmlString.ShouldContain(DummyId.ToString())
            );
        }

        private void SetupForImportData()
        {
            var emailAddress = $"{DummyString}@test.test";
            _privateObject.SetField(CurrentCustomer, new Customer
            {
                SQL = DummyString
            });
            _privateObject.SetField(WriteXMLProperty, true);
            _privateObject.SetField(ClickFileImports, new List<ClickFile>
            {
                new ClickFile
                {
                    BlastID = DummyId,
                    ClickTime = DateTime.Now,
                    SendTime = DateTime.Now,
                    Address = DummyString,
                    EmailAddress = emailAddress
                }
            });
            _privateObject.SetField(OpenFileImports, new List<OpenFile>
            {
                new OpenFile
                {
                    BlastID = DummyId,
                    OpenTime = DateTime.Now,
                    SendTime = DateTime.Now,
                    Address = DummyString,
                    EmailAddress = emailAddress
                }
            });
            _privateObject.SetField(TopicFileImports, new List<TopicFile>
            {
                new TopicFile
                {
                    ActivityDateTime= DateTime.Now,
                    Address = DummyString,
                    EmailAddress = emailAddress,
                    Pubcode = DummyString,
                    TopicCode = DummyString,
                    TopicCodeText = DummyString
                }
            });
            _privateObject.SetField(VisitFileImports, new List<VisitFile>
            {
                new VisitFile
                {
                    VisitTime = DateTime.Now,
                    EmailAddress = emailAddress
                }
            });
            _privateObject.SetField(StatusUpdateFileImports, new List<StatusUpdateFile>
            {
                new StatusUpdateFile
                {
                    EmailStatus = EmailStatus.Active,
                    EmailAddress = emailAddress
                }
            });
        }
    }
}
