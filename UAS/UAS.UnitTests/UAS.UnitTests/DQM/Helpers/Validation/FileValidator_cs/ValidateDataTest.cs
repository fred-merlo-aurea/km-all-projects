using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using ADMS.Services.Fakes;
using DQM.Helpers.Validation;
using DQM.Helpers.Validation.Fakes;
using FrameworkServices;
using FrameworkUAD.BusinessLogic.Transformations.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using FrameworkUAD_Lookup.Entity;
using UAD_Lookup_WS.Interface;
using UAD_Lookup_WS.Interface.Fakes;
using Core_AMS.Utilities;
using UAD_WS.Interface.Fakes;
using FrameworkUAD.Object;
using KM.Common.Import;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.DQM.Helpers.Validation.Common;

namespace UAS.UnitTests.DQM.Helpers.Validation.FileValidator_cs
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class ValidateDataTest : Fakes
    {
        private const int StandarTypeId = 0;
        private const int DemoTypeId = 1;
        private const string SampleValue = "Test";
        private const string SampleResponseGroup = "SampleResponseGroup";
        private const string StandarTypeIDField = "standarTypeID";
        private const string DemoTypeIDField = "demoTypeID"; 
        private const string SourceFileField = "sourceFile";
        private const string ClientField = "client";
        private const string ServiceFeatureField = "serviceFeature";
        private const string CodeWorkerField = "codeWorker";
        private const string ClientPubCodesField = "clientPubCodes";
        private const string DataIVField = "dataIV";
        private const string ErrorMessagesField = "ErrorMessages";
        private const string DbFileTypeField = "dbFileType";
        private const string NotDataCompare = "Not_Data_Compare";
        private const string PubCodeName = "PubCode";
        private const string TestPubCode = "Test_pubcode";
        private const string SampleTextFile = "test.txt";
        private const string ValidateDataMethodName = "ValidateData";
        
        private FileValidator _testEntity;
        private PrivateObject _privateObject;
        private SourceFile _sourceFile;
        private ServiceFeature _serviceFeature;
        private Client _client;
        private ImportFile _dataIV;
        private Dictionary<int, string> _clientPubCodes;
        private List<string> _errorMessages;
        private Code _dbFileType;
        private List<string> _incomingColumns;

        [SetUp]
        public void Setup()
        {
            _testEntity = new FileValidator();
            _serviceFeature = new ServiceFeature { SFName = NotDataCompare };
            _errorMessages = new List<string>();
            _dbFileType = new Code { CodeName = FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms.ToString() };
            _sourceFile = new SourceFile
            {
                IsDQMReady = true,
                FieldMappings = new HashSet<FieldMapping>
                {
                    new FieldMapping
                    {
                        MAFField = PubCodeName,
                        IncomingField = PubCodeName,
                        DataType  = "string",
                        PreviewData = string.Empty
                    }
                }
            };
            _clientPubCodes = new Dictionary<int, string>
            {
                {1, TestPubCode.ToUpper() }
            };
            _dataIV = new ImportFile(new System.IO.FileInfo(SampleTextFile))
            {
                DataTransformed = new Dictionary<int, StringDictionary>
                {
                    {1 , new StringDictionary { { PubCodeName, TestPubCode } } },
                },
                TransformedRowToOriginalRowMap = new Dictionary<int, int> { { 1, 1} }
            };
            _client = new Client();
            SetupFakes();
            SetFileValidatorPrivateObject();
            SetValidateDataFakes();
            ShimServiceBase.AllInstances.clientPubCodesGet = _ => 
            new Dictionary<int, string>()
            {
                {1, TestPubCode}
            };
            ShimFileValidator.AllInstances.ValidateData = null;
        }

        [Test]
        public void ValidateData_IsDataCompareFalseAndNoFileDataToImport_SetsErrorMessageAndDataIVErrorCount()
        {
            // Arrange
            SetFileValidatorPrivateObject();

            // Act
             _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(0);
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(1);
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldSatisfyAllConditions(
                () => errorMessages.Any(x => x.Contains("Row missing Subscriber Profile and Email @ Row 1, you must have one or the other")),
                () => errorMessages.Any(x => x.Contains("NO VALID Transformed data to insert")));
        }

        [Test]
        public void ValidateData_WithDQMReadyAndSubscriberTransformedResponseTrue_SetsErrorMessageAndDataIVErrorCount()
        {
            // Arrange
            SetFileValidatorPrivateObject();
            CreateUadSubscriberTransformedClient(result: true);

            // Act
            _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(0);
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(1);
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldContain(x => x.Contains("Row missing Subscriber Profile and Email @ Row 1, you must have one or the other"));
            errorMessages.ShouldContain(x => x.Contains("NO VALID Transformed data to insert"));
        }

        [Test]
        public void ValidateData_WithCreateTransformedSubscriberThrowsException_SetsOnlyErrorMessages()
        {
            // Arrange
            _serviceFeature.SFName = "Data_Compare";
            SetFileValidatorPrivateObject();
            ShimFileValidatorBase
                    .AllInstances
                    .CreateTransformedSubscriberListOfTransformSplitStringDictionaryInt32Int32Int32DictionaryOfInt32GuidListOfAdHocDimensionGroupListOfTransformationFieldMapListOfTransformation =
                (q, w, e, t, y, u, i, d, x, k) => { throw new InvalidOperationException("Test Exception"); };

            // Act
            _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(0);
            dataIV.ImportErrorCount.ShouldBe(0);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(0);
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(x => x.Contains("NO VALID Transformed data to insert"));
        }

        [Test]
        public void ValidateData_WithClientFolderScranton_SetsErrorMessagesAndDataIV()
        {
            // Arrange
            _client.FtpFolder = "Scranton";
            SetFileValidatorPrivateObject();
           
            // Act
            _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(1);
            dataIV.ImportErrorCount.ShouldBe(2);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(2);
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains("ERROR: Insert to Transformed Bulk Data Insert"));
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldContain(x => x.Contains("Row missing Subscriber Profile and Email @ Row 1, you must have one or the other"));
            errorMessages.ShouldContain(x => x.Contains("ERROR: Insert to Transformed Bulk Data Insert"));
        }

        [Test]
        public void ValidateData_WithClientFolderScrantonAndSubscriberInvalidClientResponseFalse_SetsErrorMessageAndDataIV()
        {
            // Arrange
            _client.FtpFolder = "Scranton";
            SetFileValidatorPrivateObject();
            CreateUADSubscriberInvalidClient(result: false);

            // Act
            _privateObject.Invoke(ValidateDataMethodName);

            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(1);
            dataIV.ImportErrorCount.ShouldBe(3);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(3);
            dataIV.ImportErrors.ShouldContain(x => 
                x.ClientMessage.Contains("An unexplained error occurred while inserting records into Transformed or DemoTransformed tables"));
            dataIV.ImportErrors.ShouldContain(x =>
                x.ClientMessage.Contains("An unexplained error occurred while inserting records into Invalid or DemoInvalid tables"));
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldContain(x => 
                x.Contains("Row missing Subscriber Profile and Email @ Row 1, you must have one or the other"));
            errorMessages.ShouldContain(x => 
                x.Contains("An unexplained error occurred while inserting records into Invalid or DemoInvalid tables"));
            errorMessages.ShouldContain(x => 
                x.Contains("An unexplained error occurred while inserting records into Transformed or DemoTransformed tables"));
        }

        [Test]
        public void ValidateData_WithdbFileTypeAndSampleGrouName_SetsErrorMessageAndDataIV()
        {
            // Arrange
            _incomingColumns = new List<string>();
            SetFieldMappings(_sourceFile.FieldMappings, _incomingColumns);
            _dataIV = new ImportFile(new System.IO.FileInfo(SampleTextFile))
            {
                DataTransformed = new Dictionary<int, StringDictionary>
                {
                    { 0 , new StringDictionary
                        {
                            { PubCodeName, TestPubCode },
                            { "FirstName","TFNAME" },
                            { "LastName", "TLNAME" },
                            { "Company", "TCompany" },
                            { "Title", "TTitle" },
                        }
                    },
                },
                TransformedRowToOriginalRowMap = new Dictionary<int, int> { { 0, 0 } }
            };
            SetFileValidatorPrivateObject();

            // Act
            _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(0);
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(1);
            dataIV.ImportErrors.ShouldContain(x => 
            x.ClientMessage.Contains($"WEB FORM: Missing/Blank Required Codesheet: {SampleResponseGroup.ToLower()} @ Row 0"));
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldContain(x => 
            x.Contains($"WEB FORM: Missing/Blank Required Codesheet: {SampleResponseGroup.ToLower()} @ Row 0"));
        }

        [Test]
        public void ValidateData_WithdbFileTypeWithSubTranWithDemoGraphicsValueEmpty_SetsErrorMessageAndDataIV()
        {
            // Arrange
            _incomingColumns = new List<string>();
            SetFieldMappings(_sourceFile.FieldMappings, _incomingColumns);
            ShimFileValidatorBase.
                AllInstances.
                CreateTransformedSubscriberListOfTransformSplitStringDictionaryInt32Int32Int32DictionaryOfInt32GuidListOfAdHocDimensionGroupListOfTransformationFieldMapListOfTransformation =
                (q, w, e, t, y, u, i, d, x, k) => 
                {
                    return new SubscriberTransformed
                    {
                        FName = "TFName",
                        LName = "TLName",
                        Company = "TCompany",
                        DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>
                        {
                            new SubscriberDemographicTransformed { MAFField = SampleResponseGroup,Value = string.Empty }
                        },
                        ImportRowNumber = 1,
                        PubCode = TestPubCode
                    };
                };
            SetFileValidatorPrivateObject();

            // Act
            _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(0);
            dataIV.ImportErrorCount.ShouldBe(1);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(1);
            dataIV.ImportErrors.ShouldContain(x => 
            x.ClientMessage.Contains($"WEB FORM: Blank Required Codesheet: {SampleResponseGroup} value is null/empty @ Row 1"));
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldContain(x => 
            x.Contains($"WEB FORM: Blank Required Codesheet: {SampleResponseGroup} value is null/empty @ Row 1"));
        }

        [Test]
        public void ValidateData_WithdbFileTypeAndPubCodeEmpty_ReturnsInValidProfile()
        {
            // Arrange
            _incomingColumns = new List<string>();
            SetFieldMappings(_sourceFile.FieldMappings, _incomingColumns);
            ShimFileValidatorBase.
                AllInstances.
                CreateTransformedSubscriberListOfTransformSplitStringDictionaryInt32Int32Int32DictionaryOfInt32GuidListOfAdHocDimensionGroupListOfTransformationFieldMapListOfTransformation =
                (q, w, e, t, y, u, i, d, x, k) =>
                {
                    return new SubscriberTransformed
                    {
                        FName = "TFName",
                        LName = "TLName",
                        Company = "TCompany",
                        DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>
                        {
                            new SubscriberDemographicTransformed { MAFField = "SampleResponseGroup",Value = string.Empty }
                        },
                        ImportRowNumber = 1,
                        PubCode = string.Empty
                    };
                };

            SetFileValidatorPrivateObject();

            // Act
            _privateObject.Invoke(ValidateDataMethodName);
            var errorMessages = _privateObject.GetField(ErrorMessagesField) as List<string>;
            var dataIV = _privateObject.GetField(DataIVField) as ImportFile;

            // Assert
            dataIV.ShouldNotBeNull();
            dataIV.ImportedRowCount.ShouldBe(0);
            dataIV.ImportErrorCount.ShouldBe(2);
            dataIV.OriginalRowCount.ShouldBe(0);
            dataIV.ImportErrors.ShouldNotBeEmpty();
            dataIV.ImportErrors.Count.ShouldBe(2);
            dataIV.ImportErrors.ShouldContain(x =>
            x.ClientMessage.Contains($"Blank {PubCodeName.ToUpper()} @ Row 1"));
            dataIV.ImportErrors.ShouldContain(x => x.ClientMessage.Contains($"{PubCodeName.ToUpper()} not found in UAD @ Row 1"));
            errorMessages.ShouldNotBeNull();
            errorMessages.ShouldNotBeEmpty();
            errorMessages.ShouldContain(x =>
                x.Contains($"{PubCodeName.ToUpper()} not found in UAD @ Row 1"));
            errorMessages.ShouldContain(x => x.Contains($"Blank {PubCodeName.ToUpper()} @ Row 1"));
        }

        private void SetFileValidatorPrivateObject()
        {
            _privateObject = new PrivateObject(_testEntity);
            _privateObject.SetField(StandarTypeIDField, StandarTypeId);
            _privateObject.SetField(DemoTypeIDField, DemoTypeId);
            _privateObject.SetField(SourceFileField, _sourceFile);
            _privateObject.SetField(ClientField, _client);
            _privateObject.SetField(ServiceFeatureField, _serviceFeature);
            _privateObject.SetField(CodeWorkerField, ServiceClient.UAD_Lookup_CodeClient());
            _privateObject.SetField(ClientPubCodesField, _clientPubCodes);
            _privateObject.SetField(DataIVField, _dataIV);
            _privateObject.SetField(ErrorMessagesField, _errorMessages);
            _privateObject.SetField(DbFileTypeField, _dbFileType);
        }

        private void SetValidateDataFakes()
        {
            ShimFileValidator.AllInstances.InsertOriginalSubscribers = (x) =>
            {
                var subOriginal = new List<SubscriberOriginal>
                {
                    new SubscriberOriginal
                    {
                        ImportRowNumber = 1,
                        SORecordIdentifier = Guid.Empty
                    }
                };
                return subOriginal;
            };
            ShimFileValidator.AllInstances.Scranton_CompanySurveyListOfSubscriberTransformedDictionaryOfInt32StringInt32 =
               (r, u, v, s) =>
               {
                   return new List<SubscriberTransformed>
                   {
                        new SubscriberTransformed()
                   };
               };
        }

        private void SetFieldMappings(HashSet<FieldMapping> fieldMappings, List<string> incomingColumns)
        {
            var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
            foreach (PropertyDescriptor prop in properties)
            {
                incomingColumns.Add(prop.DisplayName);
                fieldMappings.Add(new FieldMapping
                {
                    MAFField = prop.DisplayName,
                    IncomingField = prop.DisplayName,
                    DataType = "string",
                    PreviewData = string.Empty
                });
            }
        }
    }
}
