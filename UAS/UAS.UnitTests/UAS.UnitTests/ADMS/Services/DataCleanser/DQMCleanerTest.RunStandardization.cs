using System;
using System.Collections.Generic;
using System.IO;
using ADMS.Services.DataCleanser;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using ADMS.Services.Fakes;
using Core.ADMS.Events;
using Core.ADMS.Events.Fakes;
using ADMS.Services.DataCleanser.Fakes;
using static KMPlatform.BusinessLogic.Enums;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using ADMS;
using UASObject = FrameworkUAS.Object;
using ADMS.ClientMethods.Fakes;
using UASBusinessLogicFakes = FrameworkUAS.BusinessLogic.Fakes;
using UADBusinessLogicFakes = FrameworkUAD.BusinessLogic.Fakes;
using static FrameworkUAS.BusinessLogic.Enums.Clients;
using UADEnums = FrameworkUAD_Lookup.Enums;
using FrameworkUAD.Object;
using ADMS.Services.UAD.Fakes;

namespace UAS.UnitTests.ADMS.Services.DataCleanser
{
    public partial class DQMCleanerTest
    {
        private void Cleaner_FileProcessed(FileProcessed fp)
        {
        }

        private void Cleaner_FileCleansed(FileCleansed obj)
        {
        }

        [Test]
        public void RunStandardization_OnException_LogError()
        {
            // Arrange
            var client = new Client
            {
                FtpFolder = "TestString"
            };
            var cleaner = new DQMCleaner();
            var exceptionLogged = false;
            var resultMsg = "An unexpected exception occured during data cleansing. Customer support has been notified of this issue.";
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };

            // Act	
            var actualResult = cleaner.RunStandardization(client, null, null, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
            actualResult.ShouldContain(resultMsg);
        }

        [Test]
        public void RunStandardization_IsDQMReadyIsFalse_LogError()
        {
            // Arrange
            var client = new Client
            {
                FtpFolder = "TestString"
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = false
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            var resultMsg = "Not ready for DQM....Please ensure the Add Subscriber feature is enabled for your account.";
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) =>
                {
                    exceptionLogged = true;
                };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
            actualResult.ShouldContain(resultMsg);
        }

        [Test]
        public void RunStandardization_OnAddressCleanException_LogError()
        {
            // Arrange
            var client = new Client
            {
                FtpFolder = "TestString"
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) =>
            {
                throw new Exception();
            };

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_OnClientSpecialCommonException_LogError()
        {
            // Arrange
            var client = new Client
            {
                FtpFolder = "TestString"
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = "CodeName"
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Data_Compare;
            UASBusinessLogicFakes::ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean = (obj, id, deleted) => 
            {
                return new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                };
            };
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => 
                {
                    throw new Exception(nameof(Exception));
                };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString = 
                (obj, conn, code, dft) => true;

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_ClientAdditionalPropertiesContainsClient_ReturnString()
        {
            // Arrange
            var client = new Client
            {
                FtpFolder = "TestString",
                ClientID = 1
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = "CodeName"
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Data_Compare;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_FieldUpdateDataMatchingThrowException_LogError()
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Meister.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Field_Update.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
                .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
                (obj, code, type, clt) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
                .FieldUpdateDataMatchingClientConnectionsString =
                (obj, code, con) => throw new Exception();

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_QuickFillDataMatchingThrowException_LogError()
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Meister.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.QuickFill.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
                .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
                (obj, code, type, clt) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
                .QuickFillDataMatchingClientConnectionsString =
                (obj, code, con) => throw new Exception();

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_DataMatchingMultipleThrowException_LogError()
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Meister.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_multipleClientConnectionsInt32StringString =
                (obj, con, id, code, match) => throw new Exception();
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName"
                                },
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName2"
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
               .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
               (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_DataMatchingSingleThrowException_LogError()
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Atcom.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_singleClientConnectionsStringString =
                (obj, con, code, match) => throw new Exception();
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName"
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean =
               (obj, id, del) =>
               {
                   return new UASObject::ClientAdditionalProperties
                   {
                       ClientCustomProceduresList = new List<ClientCustomProcedure>()
                   };
               };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
                (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBoolean =
                (obj, con, id, code, per, faxPer, phPer, oPer, tPer, renew, txtPer, upd, updP, updF, updM) => true;

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        [TestCase(UADEnums.ExecutionPointType.Post_DataMatch)]
        [TestCase(UADEnums.ExecutionPointType.Pre_Suppression)]
        [TestCase(UADEnums.ExecutionPointType.Post_Suppression)]
        [TestCase(UADEnums.ExecutionPointType.Post_DQM)]
        public void RunStandardization_ExecuteClientCustomCodeThrowException_LogError(UADEnums.ExecutionPointType excType)
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.AHACoding.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                2,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => 
                {
                    if (match.Equals(excType))
                    {
                        throw new Exception();
                    }
                };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_singleClientConnectionsStringString =
                (obj, con, code, match) => throw new Exception();
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName"
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean =
                (obj, id, del) =>
                {
                    return new UASObject::ClientAdditionalProperties
                    {
                        ClientCustomProceduresList = new List<ClientCustomProcedure>()
                    };
                };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
                (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBoolean =
                (obj, con, id, code, per, faxPer, phPer, oPer, tPer, renew, txtPer, upd, updP, updF, updM) => true;

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        [TestCase("SAETB")]
        [TestCase("Meister")]
        public void RunStandardization_ECNThirdPartySuppresionThrowException_LogError(string ftpFolder)
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = ftpFolder
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            var resultMsg = "An unexpected exception occured during data cleansing. Customer support has been notified of this issue.";
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                2,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_singleClientConnectionsStringString =
                (obj, con, code, match) => throw new Exception();
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBoolean =
                (obj, con, id, code, per, faxPer, phPer, oPer, tPer, renew, txtPer, upd, updP, updF, updM) => true;
           UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName"
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean =
                (obj, id, del) =>
                {
                    return new UASObject::ClientAdditionalProperties
                    {
                        ClientCustomProceduresList = new List<ClientCustomProcedure>()
                    };
                };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
                (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances
                .ECN_ThirdPartySuppresionClientConnectionsStringListOfInt32 =
                (obj, con, code, ids) => throw new Exception(); 

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            actualResult.ShouldContain(resultMsg);
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        [TestCase("MailPermissionOverRide")]
        [TestCase("FaxPermissionOverRide")]
        [TestCase("PhonePermissionOverRide")]
        [TestCase("OtherProductsPermissionOverRide")]
        [TestCase("ThirdPartyPermissionOverRide")]
        [TestCase("EmailRenewPermissionOverRide")]
        [TestCase("TextPermissionOverRide")]
        [TestCase("UpdateEmail")]
        [TestCase("UpdatePhone")]
        [TestCase("UpdateFax")]
        [TestCase("UpdateMobile")]
        public void RunStandardization_RuleValues_LogError(string ruleName)
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Meister.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_singleClientConnectionsStringString =
                (obj, con, code, match) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleValues = new HashSet<RuleValue>
                                    {
                                        new RuleValue()
                                    },
                                    RuleName = ruleName
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
               .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
               (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeTrue();
        }

        [Test]
        public void RunStandardization_FileInfoIsNulll_ReturnString()
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Atcom.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fileInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_singleClientConnectionsStringString =
                (obj, con, code, match) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName"
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean =
               (obj, id, del) =>
               {
                   return new UASObject::ClientAdditionalProperties
                   {
                       ClientCustomProceduresList = new List<ClientCustomProcedure>()
                   };
               };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
                (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBoolean =
                (obj, con, id, code, per, faxPer, phPer, oPer, tPer, renew, txtPer, upd, updP, updF, updM) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SelectResultCountStringClientConnections =
                (obj, code, conn) =>
                {
                    return new AdmsResultCount
                    {
                        FinalProfileCount = 1,
                        FinalDemoCount = 1,
                        MatchedRecordCount = 1,
                        UadConsensusCount = 1,
                        ArchiveDemoCount = 1,
                        ArchiveProfileCount = 1
                    };
                };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateFinalCountsStringInt32Int32Int32Int32Int32Int32BooleanInt32Int32 =
                (obj, code, fpCnt, frCnt, fdCnt, mrCnt, uadCnt, id, createLog, fId, afCnt) => true;
            ShimUADProcessor.AllInstances.ImportToUADClientAdmsLogEnumsFileTypesSourceFile =
                (obj, clt, log, type, srcFile) => { };

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, null, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeFalse();
        }

        [Test]
        public void RunStandardization_FileInfoNotNulll_ReturnString()
        {
            // Arrange
            var client = new Client
            {
                ClientID = 1,
                FtpFolder = Clients.Atcom.ToString()
            };
            var sourceFile = new SourceFile
            {
                IsDQMReady = true
            };
            var admsLog = new AdmsLog
            {
                ProcessCode = "TestCode"
            };
            var cleaner = new DQMCleaner();
            cleaner.FileProcessed += Cleaner_FileProcessed;
            var exceptionLogged = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (obj, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    exceptionLogged = true;
                    return 1;
                };
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 =
                (svc, message, processCode, createLog, sourceFileId, updatedByUserId) => { };
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
                (obj, clt, srcFileID, log, fInfo, custFileName, fileType, schemaValid, validationResult) => { };
            ShimAddressClean.AllInstances.CountryRegionCleanseInt32StringClient = (ac, id, code, clt) => { };
            ShimCode.AllInstances.SelectCodeIdInt32 = (obj, id) =>
            {
                return new Code
                {
                    CodeName = UADEnums.FileTypes.Telemarketing_Long_Form.ToString()
                };
            };
            ShimEnums.GetDatabaseFileTypeString = (type) => FileTypes.Telemarketing_Long_Form;
            BillTurner.ClientAdditionalProperties = new Dictionary<int, UASObject::ClientAdditionalProperties>();
            BillTurner.ClientAdditionalProperties.Add(
                1,
                new UASObject::ClientAdditionalProperties
                {
                    ClientCustomProceduresList = new List<ClientCustomProcedure>()
                }
                );
            ShimClientSpecialCommon.AllInstances
                .ExecuteClientCustomCodeEnumsExecutionPointTypeClientListOfClientCustomProcedureSourceFileString =
                (obj, match, clt, list, file, code) => { };
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SaveDQMCleanClientConnectionsStringString =
               (obj, conn, code, dft) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.NullifyKMPSGroupEmailsClientConnectionsString =
                (obj, con, code) => true;
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .DataMatching_singleClientConnectionsStringString =
                (obj, con, code, match) => true;
            UADBusinessLogicFakes::ShimCircIntegration.AllInstances
               .CircFileTypeUpdateIGrpBySequenceStringEnumsFileTypesClientConnections =
               (obj, code, type, clt) => true;
            ShimServiceBase.AllInstances.GetRuleSetsByExecutionPointEnumsExecutionPointTypeSourceFile =
                (obj, match, file) =>
                {
                    return new HashSet<UASObject::RuleSet>
                    {
                        new UASObject::RuleSet
                        {
                            Rules = new HashSet<UASObject.Rule>
                            {
                                new UASObject::Rule
                                {
                                    RuleName = "TestRuleName"
                                }
                            }
                        }
                    };
                };
            UASBusinessLogicFakes::ShimClientAdditionalProperties.AllInstances.SetObjectsInt32Boolean =
               (obj, id, del) =>
               {
                   return new UASObject::ClientAdditionalProperties
                   {
                       ClientCustomProceduresList = new List<ClientCustomProcedure>()
                   };
               };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
                (obj, code, type, id, sts, create, srcd) => 1;
            ShimDQMCleaner.AllInstances.PerformSuppressionAdmsLog = (obj, log) => { };
            UADBusinessLogicFakes::ShimSubscriberTransformed.AllInstances
                .StandardRollUpToMasterClientConnectionsInt32StringBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBooleanBoolean =
                (obj, con, id, code, per, faxPer, phPer, oPer, tPer, renew, txtPer, upd, updP, updF, updM) => true;
            UADBusinessLogicFakes::ShimSubscriberFinal.AllInstances.SelectResultCountStringClientConnections =
                (obj, code, conn) =>
                {
                    return new AdmsResultCount
                    {
                        FinalProfileCount = 1,
                        FinalDemoCount = 1,
                        MatchedRecordCount = 1,
                        UadConsensusCount = 1,
                        ArchiveDemoCount = 1,
                        ArchiveProfileCount = 1
                    };
                };
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances
                .UpdateFinalCountsStringInt32Int32Int32Int32Int32Int32BooleanInt32Int32 =
                (obj, code, fpCnt, frCnt, fdCnt, mrCnt, uadCnt, id, createLog, fId, afCnt) => true;
            var fileInfo = new FileInfo("FileName");
            cleaner.FileCleansed += Cleaner_FileCleansed;
            UASBusinessLogicFakes::ShimAdmsLog.AllInstances.SaveAdmsLog = (obj, log) => 1;

            // Act	
            var actualResult = cleaner.RunStandardization(client, admsLog, sourceFile, fileInfo, false, false, false, null);

            // Assert
            actualResult.ShouldNotBeNullOrEmpty();
            exceptionLogged.ShouldBeFalse();
        }
    }
}
