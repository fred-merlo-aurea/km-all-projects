using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Fakes;
using System.IO;
using System.Reflection;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using KM.Common.Fakes;
using KM.Common.Functions.Fakes;
using MAF.NorthStarImport.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace MAF.NorthStarImport.Tests
{
    [TestFixture]
    public partial class ProgramTests
    {
        private const string TestExceptionMessage = "Test Exception";
        private const string InnerExceptionMessage = "Inner Exception";
        private const string SampleFile = @"c:\filename.txt";
        private const string ExpectedLastLog = "Done Instance";
        private const string ExpectedLastCustomerLog = "End Customer Process";
        private const string MethodStart = "Start";
        private const string CustomerList = "CustomerList";
        private const string LogPath = @"c:\";
        private const string CustomerName = "TestCustomer";
        private const string ExpectedCustomerHeader = "Customer: TestCustomer";
        private const string ProcessTypeUnsubscribe = "Unsubscribe";
        private const string ProcessTypeSpamComplaints = "SpamComplaints";
        private const string ProcessTypeUndeliverableEmails = "UndeliverableEmails";
        private const string ProcessTypeSubscribe = "Subscribe";
        private const string CurrentCustomer = "CurrentCustomer";
        private const string MethodImportData = "ImportData";
        private const string UnsubscribeListFieldName = "UnsubscribeList";
        private const string SpamComplaintListFieldName = "SpamComplaintList";
        private const string ImportUnsubscribeMethodName = "ImportUnsubscribe";
        private const string ImportSpamComplaintsMethodName = "ImportSpamComplaints";
        private const string BatchSizeFieldName = "BatchSize";
        private const int DefaultBatchSize = 500;
        private const string MyTestSqlConnectionString = "MyTestConnectionString";
        private const string WriteXmlFieldName = "WriteXML";
        private const string TildeXml = "~~XML~~";
        public readonly string ExpectedException = "**********************" + Environment.NewLine +
                                                   "-- Data --" + Environment.NewLine +
                                                   "-- Message --" + Environment.NewLine +
                                                   "Test Exception" + Environment.NewLine +
                                                   "-- InnerException --" + Environment.NewLine +
                                                   "System.Exception: Inner Exception" + Environment.NewLine +
                                                   "**********************" + Environment.NewLine;

        public readonly string ExpectedExecuteNullReferenceException = "-- Data --" + Environment.NewLine +
                                                                       "-- Message --" + Environment.NewLine +
                                                                       "Object reference not set to an instance of an object." + Environment.NewLine +
                                                                       "-- Stack Trace --" + Environment.NewLine +
                                                                       "   at MAF.NorthStarImport";

        private Program _target = new Program();
        private IDisposable _shims;
        
        private static Program CreateProgram(bool writeXml = false)
        {
            var program = new Program();
            program.SetField(BatchSizeFieldName, DefaultBatchSize);
            if (writeXml)
            {
                program.SetField(WriteXmlFieldName, true);
            }

            return program;
        }
        
        private static Program CreateProgramUnsubscribe(bool writeXml = false)
        {
            var program = CreateProgram(writeXml);
            program.SetField(UnsubscribeListFieldName, GetListOfUnsubscribes());
           
            return program;
        } 
        private static Program CreateProgramSpamComplaints(bool writeXml = false)
        {
            var program = CreateProgram(writeXml);
            program.SetField(SpamComplaintListFieldName, GetListOfSpamComplaints());
           
            return program;
        }
        
        private static List<Unsubscribe> GetListOfUnsubscribes()
        {
            var unsubscribeList = new List<Unsubscribe>()
            {
                new Unsubscribe() 
                { 
                    EmailAddress = "email@dontmailme.com",
                    PubCode = "PubCode1",
                    Reason = "Reason1"
                },
                new Unsubscribe()
                {
                    EmailAddress = "done@testmailma.com",
                    PubCode = "TestPubCode",
                    Reason = "TestReason1"
                }
            };

            return unsubscribeList;
        } 
        
        private static List<UpdateEmailStatus> GetListOfSpamComplaints()
        {
            var unsubscribeList = new List<UpdateEmailStatus>()
            {
                new UpdateEmailStatus() 
                { 
                    EmailAddress = "email@dontmailme.com",
                    UpdateReason = "Reason1"
                },
                new UpdateEmailStatus()
                {
                    EmailAddress = "done@testmailma.com",
                    UpdateReason = "TestReason1"
                }
            };

            return unsubscribeList;
        }

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
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
        public void Start_CreateCustomerListRaiseException_ConstructCorrectString()
        {
            // Arrange
            ConfigurationManager.AppSettings["MainLog"] = "log";
            ConfigurationManager.AppSettings["BatchSize"] = "500";
            ConfigurationManager.AppSettings["WriteXML"] = "True";

            ShimProgram.AllInstances.CreateCustomerList = program =>
            {
                throw new NullReferenceException();
            };
            ShimProgram.AllInstances.SetCustomerLogString = (_, __) => { };
            ShimProgram.AllInstances.SetMainLogString = (_, __) => { };
            
            var lastLog = string.Empty;
            var previousLog = string.Empty;

            ShimProgram.AllInstances.MainLogWriteString = (program, s) =>
            {
                previousLog = lastLog;
                lastLog = s;
            };

            // Act
            ReflectionHelper.CallMethod(_target, MethodStart);

            // Assert
            previousLog.ShouldContain(ExpectedExecuteNullReferenceException);
            lastLog.ShouldBe(ExpectedLastLog);
        }

        [Test]
        public void Start_RaiseCustomerException_ConstructCorrectString()
        {
            // Arrange
            var lastLog = string.Empty;
            var previousLog = string.Empty;
            
            ConfigurationManager.AppSettings["MainLog"] = "log";
            ConfigurationManager.AppSettings["BatchSize"] = "500";
            ConfigurationManager.AppSettings["WriteXML"] = "True";

            ShimProgram.AllInstances.CreateCustomerList = program => { };
            ShimProgram.AllInstances.SetCustomerLogString = (_, __) => { };
            ShimProgram.AllInstances.SetMainLogString = (_, __) => { };
            ShimProgram.AllInstances.MainLogWriteString = (_, __) => { };
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) =>
            {
                previousLog = lastLog;
                lastLog = s;
            };

            ReflectionHelper.SetValue(_target, CustomerList, new CustomerConfig
            {
                Customers = new List<Customer>
                {
                    new Customer
                    {
                        LogPath = LogPath,
                        CustomerName = CustomerName
                    }
                }
            });

            // Act
            ReflectionHelper.CallMethod(_target, MethodStart);

            // Assert
            previousLog.ShouldContain(ExpectedCustomerHeader);
            previousLog.ShouldContain(ExpectedExecuteNullReferenceException);
            lastLog.ShouldBe(ExpectedLastCustomerLog);
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
        public void Execute_NullReferenceInProcess_LogCorrectString()
        {
            // Arrange
            var process = new Process
            {
                ProcessType = string.Empty
            };
            var expectedLog = string.Empty;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };

            // Act
            ReflectionHelper.CallMethod(_target, "Execute", process);

            // Assert
            expectedLog.ShouldContain(ExpectedExecuteNullReferenceException);
        }

        [Test]
        public void Execute_NullReferenceInProcessFlatFile_LogCorrectString()
        {
            // Arrange
            var process = new Process
            {
                ProcessType = string.Empty,
                FileFolder = string.Empty
            };
            var expectedLog = string.Empty;

            ReflectionHelper.SetValue(_target, CurrentCustomer, new Customer {FilePath = string.Empty});

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.GetFilesString = (_, __) => new FileInfo[]{new FileInfo(SampleFile) };
            ShimProgram.AllInstances.ProcessFlatFileFileInfoProcess = (_, __, ___) => throw new NullReferenceException();

            // Act
            ReflectionHelper.CallMethod(_target, "Execute", process);

            // Assert
            expectedLog.ShouldContain(ExpectedExecuteNullReferenceException);
        }

        [Test]
        public void ImportData_UnsubscribeNullReference_LogCorrectString()
        {
            // Arrange
            var process = new Process
            {
                ProcessType = ProcessTypeUnsubscribe
            };
            var expectedLog = string.Empty;

            ReflectionHelper.SetValue(_target, CurrentCustomer, new Customer {FilePath = string.Empty});

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.MainLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.ImportUnsubscribeString = (_, __) => throw new NullReferenceException();

            // Act
            ReflectionHelper.CallMethod(_target, MethodImportData, process);

            // Assert
            expectedLog.ShouldContain(ExpectedExecuteNullReferenceException);
        }

        [Test]
        public void ImportData_SpamComplaintsNullReference_LogCorrectString()
        {
            // Arrange
            var process = new Process
            {
                ProcessType = ProcessTypeSpamComplaints
            };
            var expectedLog = string.Empty;

            ReflectionHelper.SetValue(_target, CurrentCustomer, new Customer {FilePath = string.Empty});

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.MainLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.ImportSpamComplaintsString = (_, __) => throw new NullReferenceException();

            // Act
            ReflectionHelper.CallMethod(_target, MethodImportData, process);

            // Assert
            expectedLog.ShouldContain(ExpectedExecuteNullReferenceException);
        }

        [Test]
        public void ImportData_UndeliverableEmailsNullReference_LogCorrectString()
        {
            // Arrange
            var process = new Process
            {
                ProcessType = ProcessTypeUndeliverableEmails
            };
            var expectedLog = string.Empty;

            ReflectionHelper.SetValue(_target, CurrentCustomer, new Customer {FilePath = string.Empty});

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.MainLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.ImportUndeliverableEmailsString = (_, __) => throw new NullReferenceException();

            // Act
            ReflectionHelper.CallMethod(_target, MethodImportData, process);

            // Assert
            expectedLog.ShouldContain(ExpectedExecuteNullReferenceException);
        }

        [Test]
        public void ImportData_SubscribeNullReference_LogCorrectString()
        {
            // Arrange
            var process = new Process
            {
                ProcessType = ProcessTypeSubscribe
            };
            var expectedLog = string.Empty;

            ReflectionHelper.SetValue(_target, CurrentCustomer, new Customer {FilePath = string.Empty});

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.MainLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };
            ShimProgram.AllInstances.ImportSubscribeString = (_, __) => throw new NullReferenceException();

            // Act
            ReflectionHelper.CallMethod(_target, MethodImportData, process);

            // Assert
            expectedLog.ShouldContain(ExpectedExecuteNullReferenceException);
        }

        [Test]
        public void ImportUnsubscribe_CorrectParameters_DataImported()
        {
            // Arrange
            var actualLogMessage = string.Empty;
            var actualXmlString = string.Empty;
            var actualSqlConnectionString = string.Empty;
            var program = CreateProgramUnsubscribe();

            ShimDateTime.NowGet = () => new DateTime(1997, 10, 9);
            ShimProgram.AllInstances.CustomerLogWriteString = (_, logMessage) => { actualLogMessage = logMessage; };
            ShimDataFunctions.GetSqlConnection = () => new SqlConnection();
            ShimData.ImportUnsubscribesStringString = (xmlString, sqlConnectionString) =>
            {
                actualXmlString = xmlString;
                actualSqlConnectionString = sqlConnectionString;
                return true;
            };

            // Act
            program.GetType()
                .CallMethod(
                    ImportUnsubscribeMethodName,
                    new object[] { MyTestSqlConnectionString },
                    program);

            // Assert
            actualXmlString.ShouldSatisfyAllConditions(
                () => actualLogMessage.ShouldBe("ImportUnsubscribe : 2 of 2 / 10/9/1997 12:00:00 AM Done DB"),
                () => actualXmlString.ShouldBe("<XML><Emails><EmailAddress>email@dontmailme.com</EmailAddress><PubCode>PubCode1</PubCode><Reason>Reason1</Reason><RequestDate>1/1/0001 12:00:00 AM</RequestDate></Emails><Emails><EmailAddress>done@testmailma.com</EmailAddress><PubCode>TestPubCode</PubCode><Reason>TestReason1</Reason><RequestDate>1/1/0001 12:00:00 AM</RequestDate></Emails></XML>"),
                () => actualSqlConnectionString.ShouldBe(MyTestSqlConnectionString));
        }

        [Test]
        public void ImportUnsubscribe_CorrectParametersWriteXml_DataImported()
        {
            // Arrange
            var actualLogMessages = new List<string>();
            var actualXmlString = string.Empty;
            var actualSqlConnectionString = string.Empty;
            var program = CreateProgramUnsubscribe(true);

            ShimDateTime.NowGet = () => new DateTime(1997, 10, 9);
            ShimProgram.AllInstances.CustomerLogWriteString = (_, logMessage) => { actualLogMessages.Add(logMessage); };
            ShimDataFunctions.GetSqlConnection = () => new SqlConnection();
            ShimData.ImportUnsubscribesStringString = (xmlString, sqlConnectionString) =>
            {
                actualXmlString = xmlString;
                actualSqlConnectionString = sqlConnectionString;
                return true;
            };

            // Act
            program.GetType()
                .CallMethod(
                    ImportUnsubscribeMethodName,
                    new object[] { MyTestSqlConnectionString },
                    program);

            // Assert
            actualXmlString.ShouldSatisfyAllConditions(
                () => actualLogMessages.Count.ShouldBe(5),
                () => actualLogMessages[0].ShouldBe("ImportUnsubscribe : 10/9/1997 12:00:00 AM Send to DB"),
                () => actualLogMessages[1].ShouldBe(TildeXml),
                () => actualLogMessages[2].ShouldBe("<XML><Emails><EmailAddress>email@dontmailme.com</EmailAddress><PubCode>PubCode1</PubCode><Reason>Reason1</Reason><RequestDate>1/1/0001 12:00:00 AM</RequestDate></Emails><Emails><EmailAddress>done@testmailma.com</EmailAddress><PubCode>TestPubCode</PubCode><Reason>TestReason1</Reason><RequestDate>1/1/0001 12:00:00 AM</RequestDate></Emails></XML>"),
                () => actualLogMessages[3].ShouldBe(TildeXml),
                () => actualLogMessages[4].ShouldBe("ImportUnsubscribe : 2 of 2 / 10/9/1997 12:00:00 AM Done DB"),
                () => actualXmlString.ShouldBe("<XML><Emails><EmailAddress>email@dontmailme.com</EmailAddress><PubCode>PubCode1</PubCode><Reason>Reason1</Reason><RequestDate>1/1/0001 12:00:00 AM</RequestDate></Emails><Emails><EmailAddress>done@testmailma.com</EmailAddress><PubCode>TestPubCode</PubCode><Reason>TestReason1</Reason><RequestDate>1/1/0001 12:00:00 AM</RequestDate></Emails></XML>"),
                () => actualSqlConnectionString.ShouldBe(MyTestSqlConnectionString));
        } 
        
        [Test]
        public void ImportUnsubscribe_ExceptionRaised_ErrorLogged()
        {
            // Arrange
            var actualLogMessages = new List<string>();
            var actualXmlString = string.Empty;
            var actualSourceMethod = string.Empty;
            var actualApplicationId = 0;
            var actualNote = string.Empty;
            var actualCharityId = 0;
            var actualCustomerId = 0;
            var program = CreateProgramUnsubscribe();

            ShimProgram.AllInstances.CustomerLogWriteString = (_, logMessage) => { actualLogMessages.Add(logMessage); };
            ShimData.ImportUnsubscribesStringString = (xmlString, sqlConnectionString) => throw new InvalidOperationException();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (
                exception,
                sourceMethod,
                applicationId,
                note,
                charityId,
                customerId) =>
            {
                actualSourceMethod = sourceMethod;
                actualApplicationId = applicationId;
                actualNote = note;
                actualCharityId = charityId;
                actualCustomerId = customerId;
                return 0;
            };

            // Act
            program.GetType()
                .CallMethod(
                    ImportUnsubscribeMethodName,
                    new object[] { MyTestSqlConnectionString },
                    program);

            // Assert
            actualXmlString.ShouldSatisfyAllConditions(
                () => actualSourceMethod.ShouldBe("KMPS.MAF.NorthStarImport.Program.ImportUnsubscribe"),
                () => actualApplicationId.ShouldBe(0),
                () => actualNote.ShouldBe("KMPS.MAF.NorthStarImport: Unhandled Exception"),
                () => actualCharityId.ShouldBe(-1),
                () => actualCustomerId.ShouldBe(-1),
                () => actualLogMessages.Count.ShouldBe(1));
        }

        [Test]
        public void ImportSpamComplaints_CorrectParameters_DataImported()
        {
            // Arrange
            var actualLogMessage = string.Empty;
            var actualXmlString = string.Empty;
            var actualEmailStatusType = string.Empty;
            var actualSqlConnectionString = string.Empty;
            var program = CreateProgramSpamComplaints();

            ShimDateTime.NowGet = () => new DateTime(1997, 10, 9);
            ShimProgram.AllInstances.CustomerLogWriteString = (_, logMessage) => { actualLogMessage = logMessage; };
            ShimDataFunctions.GetSqlConnection = () => new SqlConnection();
            ShimData.UpdateEmailStatusStringStringString = (xmlString, emailStatusType, sqlConnectionString) =>
            {
                actualXmlString = xmlString;
                actualEmailStatusType = emailStatusType;
                actualSqlConnectionString = sqlConnectionString;
                return true;
            };

            // Act
            program.GetType()
                .CallMethod(
                    ImportSpamComplaintsMethodName,
                    new object[] { MyTestSqlConnectionString },
                    program);

            // Assert
            actualXmlString.ShouldSatisfyAllConditions(
                () => actualLogMessage.ShouldBe("ImportSpamComplaints : 10/9/1997 12:00:00 AM Done DB"),
                () => actualXmlString.ShouldBe("<XML><Emails><EmailAddress>email@dontmailme.com</EmailAddress><UpdateDate>1/1/0001 12:00:00 AM</UpdateDate><UpdateReason>Reason1</UpdateReason></Emails><Emails><EmailAddress>done@testmailma.com</EmailAddress><UpdateDate>1/1/0001 12:00:00 AM</UpdateDate><UpdateReason>TestReason1</UpdateReason></Emails></XML>"),
                () => actualEmailStatusType.ShouldBe("Spam"),
                () => actualSqlConnectionString.ShouldBe(MyTestSqlConnectionString));
        }

        [Test]
        public void ImportSpamComplaints_CorrectParametersWriteXml_DataImported()
        {
            // Arrange
            var actualLogMessages = new List<string>();
            var actualXmlString = string.Empty;
            var actualEmailStatusType = string.Empty;
            var actualSqlConnectionString = string.Empty;
            var program = CreateProgramSpamComplaints(true);

            ShimDateTime.NowGet = () => new DateTime(1997, 10, 9);
            ShimProgram.AllInstances.CustomerLogWriteString = (_, logMessage) => { actualLogMessages.Add(logMessage); };
            ShimDataFunctions.GetSqlConnection = () => new SqlConnection();
            ShimData.UpdateEmailStatusStringStringString = (xmlString, emailStatusType, sqlConnectionString) =>
            {
                actualXmlString = xmlString;
                actualEmailStatusType = emailStatusType;
                actualSqlConnectionString = sqlConnectionString;
                return true;
            };

            // Act
            program.GetType()
                .CallMethod(
                    ImportSpamComplaintsMethodName,
                    new object[] { MyTestSqlConnectionString },
                    program);

            // Assert
            actualXmlString.ShouldSatisfyAllConditions(
                () => actualLogMessages.Count.ShouldBe(5),
                () => actualLogMessages[0].ShouldBe("ImportSpamComplaints : 10/9/1997 12:00:00 AM Send to DB"),
                () => actualLogMessages[1].ShouldBe(TildeXml),
                () => actualLogMessages[2].ShouldBe("<XML><Emails><EmailAddress>email@dontmailme.com</EmailAddress><UpdateDate>1/1/0001 12:00:00 AM</UpdateDate><UpdateReason>Reason1</UpdateReason></Emails><Emails><EmailAddress>done@testmailma.com</EmailAddress><UpdateDate>1/1/0001 12:00:00 AM</UpdateDate><UpdateReason>TestReason1</UpdateReason></Emails></XML>"),
                () => actualLogMessages[3].ShouldBe(TildeXml),
                () => actualLogMessages[4].ShouldBe("ImportSpamComplaints : 10/9/1997 12:00:00 AM Done DB"),
                () => actualXmlString.ShouldBe("<XML><Emails><EmailAddress>email@dontmailme.com</EmailAddress><UpdateDate>1/1/0001 12:00:00 AM</UpdateDate><UpdateReason>Reason1</UpdateReason></Emails><Emails><EmailAddress>done@testmailma.com</EmailAddress><UpdateDate>1/1/0001 12:00:00 AM</UpdateDate><UpdateReason>TestReason1</UpdateReason></Emails></XML>"),
                () => actualEmailStatusType.ShouldBe("Spam"),
                () => actualSqlConnectionString.ShouldBe(MyTestSqlConnectionString));
        } 

        [Test]
        public void ImportSpamComplaints_ExceptionRaised_ErrorLogged()
        {
            // Arrange
            var actualLogMessages = new List<string>();
            var actualXmlString = string.Empty;
            var actualSourceMethod = string.Empty;
            var actualApplicationId = 0;
            var actualNote = string.Empty;
            var actualCharityId = 0;
            var actualCustomerId = 0;
            var program = CreateProgramSpamComplaints();

            ShimProgram.AllInstances.CustomerLogWriteString = (_, logMessage) => { actualLogMessages.Add(logMessage); };
            ShimData.ImportUnsubscribesStringString = (xmlString, sqlConnectionString) => throw new InvalidOperationException();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (
                exception,
                sourceMethod,
                applicationId,
                note,
                charityId,
                customerId) =>
            {
                actualSourceMethod = sourceMethod;
                actualApplicationId = applicationId;
                actualNote = note;
                actualCharityId = charityId;
                actualCustomerId = customerId;
                return 0;
            };

            // Act
            program.GetType()
                .CallMethod(
                    ImportSpamComplaintsMethodName,
                    new object[] { MyTestSqlConnectionString },
                    program);

            // Assert
            actualXmlString.ShouldSatisfyAllConditions(
                () => actualSourceMethod.ShouldBe("KMPS.MAF.NorthStarImport.Program.ImportSpamComplaints"),
                () => actualApplicationId.ShouldBe(0),
                () => actualNote.ShouldBe("KMPS.MAF.NorthStarImport: Unhandled Exception"),
                () => actualCharityId.ShouldBe(-1),
                () => actualCustomerId.ShouldBe(-1),
                () => actualLogMessages.Count.ShouldBe(1));
        }
    }
}
