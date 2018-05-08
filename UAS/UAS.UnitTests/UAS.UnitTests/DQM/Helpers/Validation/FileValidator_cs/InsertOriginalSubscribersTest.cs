using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using ADMS.Services.Fakes;
using DQM.Helpers.Validation;
using DQM.Helpers.Validation.Fakes;
using FrameworkServices;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS.UnitTests.DQM.Helpers.Validation.Common;
using static FrameworkUAD_Lookup.Enums;
using FrameworkUASService = FrameworkUAS.Service;
using UADLookUp = FrameworkUAD_Lookup;

namespace UAS.UnitTests.DQM.Helpers.Validation.FileValidator_cs
{
    /// <summary>
    /// Unit Tests for <see cref="FileValidator"/>class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class InsertOriginalSubscribersTest : Fakes
    {
        private const string InsertOriginalSubscribers = "InsertOriginalSubscribers";
        private const string ClientField = "client";
        private const int StandarTypeId = 0;
        private const int DemoTypeId = 1;
        private const string SampleValue = "Test";
        private const string StandarTypeIDField = "standarTypeID";
        private const string DemoTypeIDField = "demoTypeID";
        private const string SourceFileField = "sourceFile";
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
        private const string ProcessCode = "processCode";
        private const string ErrorMessages = "ErrorMessages";
        private FileValidator _fileValidator;
        private PrivateObject _privateObject;
        private SourceFile _sourceFile;
        private ServiceFeature _serviceFeature;
        private Client _client;
        private ImportFile _dataIV;
        private Dictionary<int, string> _clientPubCodes;
        private List<string> _errorMessages;
        private Code _dbFileType;

        [SetUp]
        public void Setup()
        {
            _fileValidator = new FileValidator();
            SetupFakes();
            CreateSourceFile();
            _serviceFeature = new ServiceFeature { SFName = NotDataCompare };
            _errorMessages = new List<string>();
            _dbFileType = new Code { CodeName = FileTypes.Web_Forms.ToString() };
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
                TransformedRowToOriginalRowMap = new Dictionary<int, int> { { 1, 1 } }
            };
            _client = new Client
            {
                ClientID = 1,
                ClientConnections = new KMPlatform.Object.ClientConnections()
            };
        }

        [Test]
        public void InsertOriginalSubscribers_DataOriginalIsNull_ReturnEmptyObject()
        {
            // Arrange
            SetFileValidatorPrivateObject();
            SetValidateDataFakes();

            // Act
            var result = _privateObject.Invoke(InsertOriginalSubscribers) as List<SubscriberOriginal>;
            var errorList = _privateObject.GetFieldOrProperty(ErrorMessages) as List<string>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeFalse()
            );
            errorList.ShouldSatisfyAllConditions(
                () => errorList.ShouldNotBeNull(),
                () => errorList.Any().ShouldBeTrue(),
                () => errorList.Count().ShouldBe(2)
            );
        }

        [Test]
        public void InsertOriginalSubscribers_DataOriginalIsNotNull_ReturndeDuplicateSubscriberOriginalObject()
        {
            // Arrange
            var stringDictionary = new StringDictionary { { PubCodeName, TestPubCode } };
            _dataIV.DataOriginal = new Dictionary<int, StringDictionary>
                {
                    {1 , stringDictionary },
                    {2 , stringDictionary },
                    {3 , stringDictionary }
                };
            SetFileValidatorPrivateObject();
            SetValidateDataFakes();

            // Act
            var result = _privateObject.Invoke(InsertOriginalSubscribers) as List<SubscriberOriginal>;
            var errorList = _privateObject.GetFieldOrProperty(ErrorMessages) as List<string>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count().ShouldBe(1)
            );
            errorList.ShouldSatisfyAllConditions(
                () => errorList.ShouldNotBeNull(),
                () => errorList.Any().ShouldBeFalse()
            );
        }

        private void SetFileValidatorPrivateObject()
        {
            _privateObject = new PrivateObject(_fileValidator);
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
            _privateObject.SetField(ProcessCode, Guid.NewGuid().ToString());
        }
        private void SetValidateDataFakes()
        {
            ShimServiceClient.UAD_SubscriberOriginalClient = () =>
            {
                return new ShimServiceClient<ISubscriberOriginal>
                {
                    ProxyGet = () =>
                    {
                        return new StubISubscriberOriginal
                        {
                            SaveBulkSqlInsertGuidListOfSubscriberOriginalClientConnections = (accessKey, dedupList, Conn) =>
                            {
                                var result = false;
                                if (dedupList.Any())
                                {
                                    result = true;
                                }
                                return new FrameworkUASService.Response<bool>
                                {
                                    Status = ServiceResponseStatusTypes.Success,
                                    Result = result
                                };
                            }
                        };
                    }
                };
            };
            ShimFileValidator.AllInstances.Scranton_CompanySurveyListOfSubscriberTransformedDictionaryOfInt32StringInt32 =
               (r, u, v, s) =>
               {
                   return new List<SubscriberTransformed>
                   {
                        new SubscriberTransformed()
                   };
               };

            ShimServiceBase.AllInstances.clientPubCodesGet = _ =>
                new Dictionary<int, string>()
                {
                    {1, TestPubCode}
                };
            ShimFileValidator.AllInstances.ValidateData = null;
        }

        private void CreateSourceFile()
        {
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
        }
    }
}
