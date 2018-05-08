using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using ADMS.Services.Emailer.Fakes;
using ADMS.Services.Fakes;
using ADMS.Services.Validator.Fakes;
using Core.ADMS.Events;
using Core.ADMS.Events.Fakes;
using Core.ADMS.Fakes;
using Core_AMS.Utilities;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.DataAccess.Fakes;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using KM.Common.Import;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using CommonEnums = KM.Common.Enums;
using FrameworkUADBusinessLogic = FrameworkUAD.BusinessLogic;
using FrameworkUADEntity = FrameworkUAD.Entity;
using FrameworkUADObject = FrameworkUAD.Object;
using FrameworkUASFake = FrameworkUAS.BusinessLogic.Fakes;
using KmCommonShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    /// Unit test for <see cref="ADMS_Validator"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ProcessFileAsObjectTest : Fakes
    {
        private const string qDateFormat = "MDDYY";
        private const string validateFileAsObjectTest = "ValidateFileAsObjectTest.txt";
        private const string ftpFolder = "FtpFolder";
        private const string isDemo = "IsDemo";
        private const string isNetworkDeployed = "IsNetworkDeployed";
        private const string code = "Web Forms Short Form";
        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private TestEntity testEntity;
        private bool AppsettingExist = false;
        private bool ApplyAdHocDimensionsImportFile = false;

        [SetUp]
        public void Setup()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            ShimBaseDirs.GetFulfillmentZipDir = () =>
            {
                return path;
            };
            ShimServiceBase.AllInstances.clientGet = (x) =>
            {
                return new Client();
            };
            CreateDimensionCountString();
            CreateAppSettingsObject();
            CreateFileProcessed();
            CreateFrameworkUADLookupObject();
            CreateFrameworkUASFakeObject();
            CreateShimEmailerObject();
        }

        [Test]
        public void ProcessFileAsObject_AcceptableFileTypeIsTrue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, string.Empty);
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = CreateFileMovedObject(myCheckFile, true);
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;

            var isEmptyDataTableReturl = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isEmptyDataTableReturl = true;
                return new DataTable();
            };
            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var getServiceCircFileMapper = false;
            ShimService.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                getServiceCircFileMapper = true;
                return new Service
                {
                    ServiceCode = KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(),
                };
            };
            var isAcceptableFileTypeFileInfo = false;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                isAcceptableFileTypeFileInfo = true;
                return true;
            };
            ShimValidator.AddDQMReadyFileFileAddressGeocoded = (x) => { };

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isEmptyDataTableReturl.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
            getServiceCircFileMapper.ShouldBeTrue();
        }

        [Test]
        public void ProcessFileAsObject_PubCodeMissingIsFalse_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, CreateFileContent());
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = CreateFileMovedObject(myCheckFile);
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            var isEmptyDataTableString = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isEmptyDataTableString = true;
                return CreateDataTableString();
            };
            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var getServiceCircFileMapper = false;
            ShimService.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                getServiceCircFileMapper = true;
                return new Service
                {
                    ServiceCode = KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(),
                };
            };
            var isAcceptableFileTypeFileInfo = false;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                isAcceptableFileTypeFileInfo = true;
                return true;
            };
            CreateValidatorShimObject(myCheckFile);

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
            isEmptyDataTableString.ShouldBeTrue();
            getServiceCircFileMapper.ShouldBeTrue();
            ApplyAdHocDimensionsImportFile.ShouldBeTrue();
        }


        [Test]
        public void ProcessFileAsObject_IsTextQualifierTrue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, CreateFileContent());
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = new FileMoved
            {
                SourceFile = new SourceFile
                {
                    IsSpecialFile = true,
                    IsTextQualifier = true,
                    QDateFormat = qDateFormat,
                    DatabaseFileTypeId = 1,
                    Extension = ".txt",
                    FieldMappings = new HashSet<FieldMapping>(),
                    SourceFileID = 1,
                    Delimiter = ","
                },
                AdmsLog = new AdmsLog
                {
                    ProcessCode = "110023",
                    ImportFile = myCheckFile
                },
                Client = new Client
                {
                    ClientConnections = new KMPlatform.Object.ClientConnections(),
                    FtpFolder = "FtpFolder"
                },
                ImportFile = myCheckFile,
                ThreadId = 1101
            };
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            CreateAppSettingsObject();
            var isPubCodeExist = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isPubCodeExist = true;
                return CreateDataTableString();
            };
            var isWebShortFormCodeExist = false;
            FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode.AllInstances.SelectCodeIdInt32 = (x, y) =>
            {
                isWebShortFormCodeExist = true;
                return new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeName = code
                };
            };

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isPubCodeExist.ShouldBeTrue();
            isWebShortFormCodeExist.ShouldBeTrue();
        }

        [Test]
        public void ProcessFileAsObject_AllCircMappedColumnsExistIsFalse_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, CreateFileContent());
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = CreateFileMovedObject(myCheckFile);
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            var isDataPubCodeExist = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isDataPubCodeExist = true;
                return CreateDataTableString();
            };
            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var isServiceInstanceExist = false;
            ShimService.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                isServiceInstanceExist = true;
                return new Service
                {
                    ServiceCode = KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(),
                };
            };
            var isAcceptableFileTypeFileInfo = false;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                isAcceptableFileTypeFileInfo = true;
                return true;
            };
            ShimValidator.AddDQMReadyFileFileAddressGeocoded = (x) => { };
            var isAdHocDimensionGroupExist = false;
            FrameworkUASFake.ShimAdHocDimensionGroup.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                isAdHocDimensionGroupExist = true;
                return new List<AdHocDimensionGroup>{

                    new AdHocDimensionGroup
                    {
                        IsActive=true,
                        CreatedDimension="PubCode"
                    },
                     new AdHocDimensionGroup
                     {
                         CreatedDimension = Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                     }
            };
            };
            CreateValidatorShimObject(myCheckFile);

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isDataPubCodeExist.ShouldBeTrue();
            isServiceInstanceExist.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
            isAdHocDimensionGroupExist.ShouldBeTrue();
            ApplyAdHocDimensionsImportFile.ShouldBeTrue();
        }

        [Test]
        public void ProcessFileAsObject_FinalUnexpectedHaveValue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var isDataPubCodeExist = false;
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, CreateFileContent(true));
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = CreateFileMovedObject(myCheckFile);
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isDataPubCodeExist = true;
                return CreateDataTableString();
            };
            CreateValidatorShimObject(myCheckFile);
            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var isServiceClientExist = false;
            ShimService.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                isServiceClientExist = true;
                return new Service
                {
                    ServiceCode = KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(),
                };
            };
            var isAcceptableFileTypeFileInfo = false;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                isAcceptableFileTypeFileInfo = true; return true;
            };

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isDataPubCodeExist.ShouldBeTrue();
            isServiceClientExist.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
        }

        [Test]
        public void ProcessFileAsObject_IsallCircMappedColumnsExistFalse_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, CreateFileContent(true));
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = new FileMoved
            {
                SourceFile = new SourceFile
                {
                    IsSpecialFile = false,
                    IsTextQualifier = false,
                    QDateFormat = qDateFormat,
                    DatabaseFileTypeId = 1,
                    Extension = ".txt",
                    FieldMappings = new HashSet<FieldMapping>
                    {
                        new FieldMapping
                        {
                            IsNonFileColumn = false,
                            MAFField="QUALIFICATIONDATE",
                            ColumnOrder=1,
                            IncomingField="QUALIFICATIONDATE",
                            DataType=string.Empty,
                            PreviewData=string.Empty,
                            FieldMappingTypeID=2
                        },
                         new FieldMapping
                        {
                            IsNonFileColumn = false,
                            MAFField="a",
                            ColumnOrder=1,
                            IncomingField="a",
                            DataType=string.Empty,
                            PreviewData=string.Empty,
                            FieldMappingTypeID=5
                        }
                    },
                    SourceFileID = 1,
                    Delimiter = ","
                },
                AdmsLog = new AdmsLog
                {
                    ProcessCode = "110023",
                    ImportFile = myCheckFile
                },
                Client = new Client
                {
                    ClientConnections = new KMPlatform.Object.ClientConnections(),
                    FtpFolder = "FtpFolder"
                },
                ImportFile = myCheckFile,
                ThreadId = 1101
            };
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            var isDataPubCodeExist = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isDataPubCodeExist = true;
                return CreateDataTableString();
            };

            CreateValidatorShimObject(myCheckFile);

            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var isServiceClientExist = false;
            ShimService.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                isServiceClientExist = true;
                return new Service
                {
                    ServiceCode = KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(),
                };
            };
            var isAcceptableFileTypeFileInfo = false;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                isAcceptableFileTypeFileInfo = true;
                return true;
            };

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isDataPubCodeExist.ShouldBeTrue();
            isServiceClientExist.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
        }

        [Test]
        public void ProcessFileAsObject_IsPubCodeMissingTrue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            var content = new StringBuilder(string.Empty);
            content.AppendLine(string.Format("{0}", "QUALIFICATIONDATE"));
            content.AppendLine(string.Format("{0}", "999"));
            CreateSettings(filePath, content.ToString());
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = new FileMoved
            {
                SourceFile = new SourceFile
                {
                    IsSpecialFile = false,
                    IsTextQualifier = false,
                    QDateFormat = qDateFormat,
                    DatabaseFileTypeId = 1,
                    Extension = ".txt",
                    FieldMappings = new HashSet<FieldMapping>
                    {
                        new FieldMapping
                        {
                            IsNonFileColumn = false,
                            MAFField="QUALIFICATIONDATE",
                            ColumnOrder=1,
                            IncomingField="QUALIFICATIONDATE",
                            DataType=string.Empty,
                            PreviewData=string.Empty,
                            FieldMappingTypeID=2
                        }
                    },
                    SourceFileID = 1,
                    Delimiter = ","
                },
                AdmsLog = new AdmsLog
                {
                    ProcessCode = "110023",
                    ImportFile = myCheckFile
                },
                Client = new Client
                {
                    ClientConnections = new KMPlatform.Object.ClientConnections(),
                    FtpFolder = "FtpFolder"
                },
                ImportFile = myCheckFile,
                ThreadId = 1101
            };
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var isDataPubCodeExist = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isDataPubCodeExist = true;
                return CreateDataTableString();
            };

            CreateValidatorShimObject(myCheckFile);

            var isAcceptableFileTypeFileInfo = false;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                isAcceptableFileTypeFileInfo = true;
                return true;
            };

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isDataPubCodeExist.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
            ApplyAdHocDimensionsImportFile.ShouldBeTrue();
        }

        [Test]
        public void ProcessFileAsObject_DateParsingFailureIsTrue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, validateFileAsObjectTest);
            CreateSettings(filePath, CreateFileContent());
            var myCheckFile = new FileInfo(filePath);
            var eventMessage = CreateFileMovedObject(myCheckFile);
            var isSpecialFile = false;
            var serviceFeature = new ServiceFeature();
            var cSpecialFile = new SourceFile();
            var sfr = new FrameworkUAD_Lookup.Entity.Code();
            var enumDatabaseFileType = string.Empty;
            var isDataPubCodeExist = false;
            KmCommonShimDataFunctions.GetDataTableStringSqlConnection = (x, y) =>
            {
                isDataPubCodeExist = true;
                return CreateDataTableString();
            };

            CreateValidatorShimObject(myCheckFile);

            ShimServiceBase.AllInstances.clientGet = (x) => new Client();
            var isServiceCodeExist = false;
            ShimService.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                isServiceCodeExist = true;
                return new Service
                {
                    ServiceCode = KMPlatform.Enums.Services.CREATOR.ToString(),
                };
            };

            var isAcceptableFileTypeFileInfo = true;
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) =>
            {
                return true;
            };
            ShimValidator.AddDQMReadyFileFileAddressGeocoded = (x) => { };

            // Act
            testEntity.Validator.ProcessFileAsObject(eventMessage,
                isSpecialFile,
                serviceFeature,
                cSpecialFile,
                sfr,
                enumDatabaseFileType);

            // Assert
            AppsettingExist.ShouldBeTrue();
            isDataPubCodeExist.ShouldBeTrue();
            isServiceCodeExist.ShouldBeTrue();
            isAcceptableFileTypeFileInfo.ShouldBeTrue();
            ApplyAdHocDimensionsImportFile.ShouldBeTrue();
        }

        private void CreateSettings(string path, string content)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = new FileInfo(path);
            file.Directory.Create();
            File.WriteAllText(path, content, Encoding.ASCII);
        }

        private string CreateFileContent(bool unexpecterdheader = false)
        {
            var content = new StringBuilder(string.Empty);
            if (unexpecterdheader == true)
            {
                content.AppendLine(string.Format("{0}, {1},{2}", "PubCode", "QUALIFICATIONDATE", "XERTYUI"));
            }
            else
            {
                content.AppendLine(string.Format("{0}, {1}", "PubCode", "QUALIFICATIONDATE"));
            }
            content.AppendLine(string.Format("{0}, {1}", "9918", "9918"));
            content.AppendLine(string.Format("{0}, {1}", "9918", DateTime.Now.ToString()));
            content.AppendLine(string.Format("{0}, {1}", "9918", DateTime.UtcNow.ToString()));
            content.AppendLine(string.Format("{0}, {1}", "9918", "9"));
            return content.ToString();
        }

        private void CreateFileProcessed()
        {
            ShimFileProcessed.ConstructorClientInt32AdmsLogFileInfoBooleanBooleanBooleanValidationResult =
            (sender, client, sourceFileID, admsLog, fileName, isKnownCustomerFileName, isValidFileType, isFileSchemaValid, validationResult) => { };
        }

        private void CreateDimensionCountString()
        {
            ShimSubscriberTransformed.AllInstances.SelectDimensionCountStringClientConnections = (x, y, z) =>
            {
                return new FrameworkUADObject.DimensionErrorCount
                {
                    DimensionErrorTotal = 10,
                    DimensionDistinctSubscriberCount = 20
                };
            };
        }

        private void CreateAppSettingsObject()
        {
            AppsettingExist = true;
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add(isDemo, bool.TrueString.ToString().ToLower());
            nameValueCollection.Add(isNetworkDeployed, bool.FalseString.ToString().ToLower());
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                return nameValueCollection;
            };
        }

        private void CreateFrameworkUADLookupObject()
        {
            FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode.AllInstances.SelectCodeIdInt32 = (x, y) =>
            {
                return new FrameworkUAD_Lookup.Entity.Code
                {
                    CodeName = "Web Forms Short Form"
                };
            };
            FrameworkUAD_Lookup.BusinessLogic.Fakes.ShimCode.AllInstances.SelectEnumsCodeType = (x, y) =>
            {
                return new List<FrameworkUAD_Lookup.Entity.Code>
                {
                    new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(),
                        CodeId=1
                    },
                     new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString(),
                        CodeId=2
                    },
                     new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(),
                        CodeId=3
                    },
                     new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '),
                        CodeId=4
                    },
                     new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' '),
                        CodeId=5
                    },
                     new FrameworkUAD_Lookup.Entity.Code
                    {
                        CodeName=FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString(),
                        CodeId=5
                    },
                };
            };
        }

        private void CreateFrameworkUASFakeObject()
        {
            FrameworkUASFake.ShimAdmsLog.AllInstances
            .UpdateStringEnumsFileStatusTypeEnumsADMS_StepTypeEnumsProcessingStatusTypeEnumsExecutionPointTypeInt32StringBooleanInt32Boolean =
            (q, w, e, r, t, y, u, i, o, k, l) =>
            {
                return true;
            };
            FrameworkUASFake.ShimAdmsLog.AllInstances.UpdateProcessingStatusStringEnumsProcessingStatusTypeInt32StringBooleanInt32 =
            (x, y, z, r, n, m, c) =>
            {
                return 1122;
            };
            FrameworkUASFake.ShimAdmsLog.AllInstances.UpdateDimensionStringInt32Int32Int32BooleanInt32 = (x, y, z, d, f, g, m) =>
            {
                return true;
            };
            FrameworkUASFake.ShimAdmsLog.AllInstances.SaveAdmsLog = (x, y) => { return 1; };
            FrameworkUASFake.ShimClientCustomProcedure.AllInstances.SelectClientInt32 = (x, y) =>
            {
                return new List<ClientCustomProcedure>();
            };
            FrameworkUASFake.ShimAdHocDimensionGroup.AllInstances.SelectInt32Boolean = (x, y, z) =>
            {
                return new List<AdHocDimensionGroup>
                {

                    new AdHocDimensionGroup
                    {
                        IsActive=true,
                        CreatedDimension="a",
                    },
                    new AdHocDimensionGroup
                    {
                        IsActive=true,
                        CreatedDimension="PubCode"
                    },
                    new AdHocDimensionGroup
                    {
                        IsActive=true,
                        CreatedDimension="c"
                    },
                     new AdHocDimensionGroup
                    {
                        IsActive=true,
                        CreatedDimension="b"
                    },
                      new AdHocDimensionGroup
                    {
                        IsActive=true,
                        CreatedDimension="x"
                    },
                     new AdHocDimensionGroup
                     {
                         CreatedDimension = Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                     }
                };
            };
            FrameworkUASFake.ShimTransformSplit.AllInstances.SelectObjectInt32 = (x, y) =>
            {
                return new List<TransformSplitInfo>
                {
                    new TransformSplitInfo
                    {
                        PubID=1,
                        Delimiter= CommonEnums.ColumnDelimiter.colon.ToString()
                    },
                    new TransformSplitInfo
                    {
                        PubID=2,
                        Delimiter= CommonEnums.ColumnDelimiter.comma.ToString()
                    },
                    new TransformSplitInfo
                    {
                        PubID=3,
                        Delimiter= CommonEnums.ColumnDelimiter.semicolon.ToString()
                    },
                    new TransformSplitInfo
                    {
                        PubID=4,
                        Delimiter= CommonEnums.ColumnDelimiter.tab.ToString()
                    },
                    new TransformSplitInfo
                    {
                        PubID=5,
                        Delimiter= CommonEnums.ColumnDelimiter.tild.ToString()
                    },
                    new TransformSplitInfo
                    {
                        PubID=6,
                        Delimiter= CommonEnums.ColumnDelimiter.pipe.ToString()
                    }
                };
            };
        }

        private void CreateShimEmailerObject()
        {
            ShimEmailer.AllInstances.BackupReportBuilderFileProcessed = (x, y) => { };
            ShimEmailer.AllInstances.HandleFileProcessedFileProcessed = (x, y) => { };
            ShimEmailer.AllInstances.RejectFileErrorLimitExceededImportFile = (x, y) => { };
            ShimEmailer.AllInstances.RejectFileClientFileInfoString = (x, y, z, n) => { };
        }

        private void CreateValidatorShimObject(FileInfo myCheckFile)
        {
            ApplyAdHocDimensionsImportFile = true;
            ShimValidator.AddDQMReadyFileFileAddressGeocoded = (x) => { };

            var fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = ",",
                IsQuoteEncapsulated = true,
            };
            var ifWorker = new FrameworkUADBusinessLogic.ImportFile();
            var dataIV = ifWorker.GetImportFile(myCheckFile, fileConfig);
            ShimValidator.AllInstances.ApplyAdHocDimensionsImportFile = (x, y) =>
            {
                dataIV.DataTransformed = dataIV.DataOriginal;
                dataIV.HeadersTransformed = dataIV.HeadersOriginal;
                return dataIV;
            };
            ShimValidator.AllInstances.ValidateDataImportFileFileMovedServiceFeatureEnumsFileTypes = (x, y, z, m, n) =>
            {
                dataIV.DataTransformed = dataIV.DataOriginal;
                dataIV.HeadersTransformed = dataIV.HeadersOriginal;
                dataIV.HasError = true;
                dataIV.ImportErrorCount = 1;
                dataIV.ImportErrors = new HashSet<FrameworkUADEntity.ImportError>();
                return dataIV;
            };

            ShimValidator.AllInstances.ValidateDataImportFileFileMovedServiceFeatureEnumsFileTypes = (x, y, z, b, n) =>
            {
                return dataIV;
            };
        }

        private DataTable CreateDataTableString()
        {
            var dataTable = new DataTable();
            var dataColumn = new DataColumn("PubID", typeof(int));
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("PubCode", typeof(String));
            dataTable.Columns.Add(dataColumn);
            dataTable.Rows.Add(new Object[] { 1, CommonEnums.ColumnDelimiter.colon.ToString() });
            dataTable.Rows.Add(new Object[] { 2, CommonEnums.ColumnDelimiter.comma.ToString() });
            dataTable.Rows.Add(new Object[] { 3, CommonEnums.ColumnDelimiter.semicolon.ToString() });
            dataTable.Rows.Add(new Object[] { 4, CommonEnums.ColumnDelimiter.tab.ToString() });
            dataTable.Rows.Add(new Object[] { 5, CommonEnums.ColumnDelimiter.tild.ToString() });
            dataTable.Rows.Add(new Object[] { 6, CommonEnums.ColumnDelimiter.pipe.ToString() });
            return dataTable;
        }

        private FileMoved CreateFileMovedObject(FileInfo myCheckFile, bool isSpecialFile = false)
        {
            return new FileMoved
            {
                SourceFile = new SourceFile
                {
                    IsSpecialFile = isSpecialFile,
                    IsTextQualifier = false,
                    QDateFormat = qDateFormat,
                    DatabaseFileTypeId = 1,
                    Extension = ".txt",
                    FieldMappings = new HashSet<FieldMapping>
                    {
                        new FieldMapping
                        {
                            IsNonFileColumn = false,
                            MAFField="PubCode",
                            ColumnOrder=1,
                            IncomingField="pubcode",
                            DataType=string.Empty,
                            PreviewData=string.Empty,
                            FieldMappingTypeID=1
                        },
                        new FieldMapping
                        {
                            IsNonFileColumn = false,
                            MAFField="QUALIFICATIONDATE",
                            ColumnOrder=1,
                            IncomingField="QUALIFICATIONDATE",
                            DataType=string.Empty,
                            PreviewData=string.Empty,
                            FieldMappingTypeID=2
                        },
                         new FieldMapping
                        {
                            IsNonFileColumn = false,
                            MAFField="a",
                            ColumnOrder=1,
                            IncomingField="a",
                            DataType=string.Empty,
                            PreviewData=string.Empty,
                            FieldMappingTypeID=5
                        }
                    },
                    SourceFileID = 1,
                    Delimiter = ","
                },
                AdmsLog = new AdmsLog
                {
                    ProcessCode = "110023",
                    ImportFile = myCheckFile
                },
                Client = new Client
                {
                    ClientConnections = new KMPlatform.Object.ClientConnections(),
                    FtpFolder = "FtpFolder"
                },
                ImportFile = myCheckFile,
                ThreadId = 1101
            };
        }
    }
}
