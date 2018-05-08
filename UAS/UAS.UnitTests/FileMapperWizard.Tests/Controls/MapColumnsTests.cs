using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Shouldly;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Helpers.Fakes;
using FileMapperWizard.Modules;
using FileMapperWizard.Modules.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using KM.Common.Import;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAD_Lookup_WS.Interface.Fakes;
using UAD_WS.Interface.Fakes;
using UAS_WS.Interface.Fakes;
using UAS_WS.Interface;
using static FrameworkUAD_Lookup.Enums;

namespace FileMapperWizard.Tests.Controls
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class MapColumnsTests
    {
        private const string StepThreeContainer = "StepThreeContainer";
        private const string SpecialFiles = "Special Files";
        private const string ExampleConfig = "SomeConfig";
        private const string PubCodeName = "PubCodeName";
        private const string TestPubCode = "TestPubCode";
        private const string SampleFile = "SampleFile";
        private const string BitFiled = "bit";
        private const string TestOne = "1";
        private const string DefaultGroupName = "UnitTest";
        private const string FlowLayout = "flowLayout";
        private const string ButtonStepTwoNextClick = "btnStep2Next_Click";
        private const string ContainerField = "thisContainer";
        private const string SuccessMessage = "Success";
        private const string SaveFileName = "UnitTest.txt";
        private IDisposable _shimObject;
        private MapColumns _testEntity;
        private PrivateObject _privateTestObj;
        private FMUniversal _container;
        private bool _visualChildExist;
        private bool _parseFiledMapping;
        private bool _saveFiledMapping;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            ShimMapColumns.ConstructorFMUniversal = null;
            ShimServiceClientSet.Constructor = (sender) => { };
            ShimMapColumns.AllInstances.LoadData = (m) => { };
            CreateFieldMappingObject();
            CreateProductSubscriptionsExtensionWorkerObject();
            CreateSubscriptionsExtensionMapperWorkerObject();
            CreateResponseGroupWorkerObject();
            CreateSourceFileObject();
            CreateLookUpCodeObject();
            CreateTransformationFieldMapDataObject();

            ShimFMUniversal.AllInstances.TileStateChangedObjectRadRoutedEventArgs = (x, y, z) => { };
            ShimFMUniversal.AllInstances.LoadDataBooleanBooleanBoolean = (x, y, z, m) => { };

            _container = GetShimFmUniversalContainer().Instance;
            ShimServiceClient.UAS_TransformationFieldMapClient = () => new ShimServiceClient<ITransformationFieldMap>();
            ShimServiceClient.UAS_TransformationFieldMultiMapClient = () => new ShimServiceClient<ITransformationFieldMultiMap>();
            ShimServiceClient.UAS_FieldMappingClient = () => new ShimServiceClient<IFieldMapping>();
            ShimServiceClient.UAS_FieldMultiMapClient = () => new ShimServiceClient<IFieldMultiMap>();
            _testEntity = new MapColumns(_container);
            _privateTestObj = new PrivateObject(_testEntity);
        }

        [TearDown]
        public void TearDown()
        {
            _shimObject?.Dispose();
        }

        [Test]
        public void ButtonStepTwoNextClick_ResponseStatusTypeIsSucess_UpdateControlValues()
        {
            // Arrange
            SetContainerFieldValue();
            var flowLayout = CreateColumnMapperObject();
            _privateTestObj.SetFieldOrProperty(FlowLayout, flowLayout);
            CreateAuthorizedUser();
            ShimMapColumns.AllInstances.PassFieldMappingSaving = (sender) => _parseFiledMapping = true;
            var parameters = new object[] { this, new RoutedEventArgs() };
            FindVisualChildrenObject();

            // Act
            _privateTestObj.Invoke(ButtonStepTwoNextClick, parameters);

            // Assert
            _visualChildExist.ShouldBeTrue();
            _parseFiledMapping.ShouldBeTrue();
            _saveFiledMapping.ShouldBeTrue();
        }

        [Test]
        public void ButtonStepTwoNextClick_DatabaseFileTypeListIsNull_UpdateControlValues()
        {
            // Arrange
            SetContainerFieldValue(false);
            var flowLayout = CreateColumnMapperObject();
            _privateTestObj.SetFieldOrProperty(FlowLayout, flowLayout);
            CreateAuthorizedUser(false);
            ShimMapColumns.AllInstances.PassFieldMappingSaving = (sender) => _parseFiledMapping = true;
            var parameters = new object[] { this, new RoutedEventArgs() };
            FindVisualChildrenObject();

            // Act
            _privateTestObj.Invoke(ButtonStepTwoNextClick, parameters);

            // Assert
            _visualChildExist.ShouldBeTrue();
            _parseFiledMapping.ShouldBeTrue();
            _saveFiledMapping.ShouldBeTrue();
        }

        private void SetContainerFieldValue(bool isDataFileExist = true)
        {
            _privateTestObj.SetFieldOrProperty(ContainerField, new FMUniversal(true)
            {
                saveFileName = SaveFileName,
                isCirculation = false,
                DatabaseFileType = 0,
                DatabaseFileTypeList = isDataFileExist ? CreateDatabaseFileTypeList() : null,
                sourceFileID = 1,
                myClient = new Client { ClientID = 1 },
                selectedProduct = new Product { PubID = 1 },
                currentPublication = new Product { PubID = 1 }
            });
        }

        private static List<Code> CreateDatabaseFileTypeList()
        {
            return new List<Code>
            {
                new Code { CodeId = 1, CodeName = DemographicUpdate.Replace.ToString().Replace("_", " ") },
                new Code { CodeId = 1, CodeName =FileTypes.Audience_Data.ToString().Replace("_", " ") }
            };
        }

        private void FindVisualChildrenObject()
        {
            FakesDelegates.Func<DependencyObject, IEnumerable<Border>> visulaChild = (x) =>
            {
                _visualChildExist = true;
                return new List<Border>
                {
                     new Border
                     {
                         Name = StepThreeContainer
                     }
                };
            };
            ShimWPF.FindVisualChildrenOf1DependencyObject(visulaChild);
        }

        private static StackPanel CreateColumnMapperObject()
        {
            var flowLayout = new StackPanel();
            int i = 0;
            foreach (FieldMappingTypes mappingType in Enum.GetValues(typeof(FieldMappingTypes)))
            {
                var columnMapper = new ColumnMapper
                (
                    string.Empty,
                    new AppData(),
                    new Client(),
                    new List<FrameworkUAD.Object.FileMappingColumn>(),
                    string.Empty,
                    string.Empty,
                    i == 0 ? string.Empty : SuccessMessage,
                    new List<Code>
                    {
                        new Code { CodeId = 1, CodeName = DemographicUpdate.Replace.ToString().Replace("_", " ") },
                        new Code { CodeId = 1, CodeName = DemographicUpdate.Append.ToString().Replace("_", " ") },
                        new Code { CodeId = 1, CodeName = DemographicUpdate.Overwrite.ToString().Replace("_", " ") }
                    },
                    new List<string>(),
                    new List<string>())
                {
                    FieldMapType = mappingType.ToString(),
                    IsUserDefinedColumn = true,
                    TypeOfRow = i == 0 ? ColumnMapperRowType.Remove.ToString() : ColumnMapperRowType.Normal.ToString(),
                    ButtonTag = TestOne
                };
                flowLayout.Children.Add(columnMapper);
                i++;
            }
            return flowLayout;
        }

        private void CreateAuthorizedUser(bool sourceFilesList = true)
        {
            var sourceFileList = new List<FrameworkUAS.Entity.SourceFile>();
            var clientAdditionalProperties = new Dictionary<int, ClientAdditionalProperties>();
            if (sourceFilesList)
            {
                sourceFileList.Add(new FrameworkUAS.Entity.SourceFile { SourceFileID = 1 });
                clientAdditionalProperties.Add(1, new ClientAdditionalProperties { SourceFilesList = sourceFileList });
            }

            ShimAppData.AllInstances.AuthorizedUserGet = (x) =>
            {
                return new UserAuthorization
                {
                    User = new User
                    {
                        CurrentClient = new Client
                        {
                            ClientConnections = new KMPlatform.Object.ClientConnections()
                        },
                        AccessKey = Guid.NewGuid(),
                        UserID = 1
                    },
                    ClientAdditionalProperties = clientAdditionalProperties
                };
            };
        }

        private ShimFMUniversal GetShimFmUniversalContainer(bool isExcelZipFile = true)
        {
            var shim = new ShimFMUniversal();

            shim.InstanceBehavior = ShimBehaviors.Fallthrough;
            shim.Instance.AllPublishers = new List<Client>();
            shim.Instance.AllFeatures = new List<ServiceFeature> { new ServiceFeature { SFName = SpecialFiles, ServiceFeatureID = 1 } };
            shim.Instance.DatabaseFileTypeList = new List<Code> { new Code { CodeId = 1, CodeName = FileTypes.ACS.ToString() } };
            shim.Instance.myClient = new Client() { ClientID = 1 };
            shim.Instance.FileName = SampleFile;
            shim.Instance.saveFileName = SampleFile;
            shim.Instance.sourceFileID = 1;
            shim.Instance.myFeatureID = 1;
            shim.Instance.selectedProduct = new Product { PubID = 1 };
            shim.Instance.currentPublication = new Product { PubID = 1 };
            shim.Instance.columnMapperList = new Dictionary<int, ColumnMapper>();
            shim.Instance.fieldMappingsWithTransformations = new List<int>();
            shim.fwGet = () => new ShimFileWorker
            {
                IsExcelFileFileInfo = (w) => isExcelZipFile,
                IsZipFileFileInfo = (w) => isExcelZipFile,
                GetDuplicateColumnsFileInfoFileConfiguration = (f, c) => new List<string> { ExampleConfig },
                GetFileHeadersFileInfoFileConfigurationBoolean = (f, c, b) => new StringDictionary { { PubCodeName, TestPubCode } },
                GetRowCountFileInfo = (f) => 1,
                GetDataFileInfoFileConfiguration = (f, c) => new DataTable()
            };

            shim.SetupToMapColumns = () => { };
            shim.SetupToSpecialFile = () => { };
            shim.ShowRules = () => { };
            shim.myFileConfigGet = () => new FileConfiguration();
            return shim;
        }

        private static List<Code> CreateCodeListObject()
        {
            return new List<Code>
            {
                new Code
                {
                    CodeId = 1,
                    CodeName = FieldMappingTypes.Ignored.ToString()
                },
                new Code
                {
                    CodeId = 1,
                    CodeName = FieldMappingTypes.kmTransform.ToString()
                },
                new Code
                {
                    CodeId = 1,
                    CodeName = FieldMappingTypes.Standard.ToString()
                },
                new Code
                {
                    CodeId = 1,
                    CodeName = FieldMappingTypes.Demographic.ToString()
                },
                new Code
                {
                    CodeId = 1,
                    CodeName = FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' ')
                },
                new Code
                {
                    CodeId = 1,
                    CodeName = FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' ')
                }
            };
        }

        private static void CreateTransformationFieldMapDataObject()
        {
            ShimServiceClientSet.AllInstances.TransformationFieldMapDataGet = (x) => new ShimServiceClient<ITransformationFieldMap>
            {
                ProxyGet = () => new StubITransformationFieldMap
                {
                    DeleteFieldMappingGuidInt32 = (m, n) => new FrameworkUAS.Service.Response<int>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = 1,
                        Status = ServiceResponseStatusTypes.Success
                    }
                }
            };
        }

        private static void CreateLookUpCodeObject()
        {
            ShimServiceClientSet.AllInstances.LookUpCodeGet = (x) =>
            new ShimServiceClient<UAD_Lookup_WS.Interface.ICode>
            {
                ProxyGet = () => new StubICode
                {
                    SelectCodeValueGuidEnumsCodeTypeString = (accessKey, codeType, codeValue) =>
                     new FrameworkUAS.Service.Response<Code>
                     {
                         Message = SuccessMessage,
                         ProcessCode = Guid.NewGuid().ToString(),
                         Result = new Code { CodeId = 1 },
                         Status = ServiceResponseStatusTypes.Success
                     },
                    SelectCodeNameGuidEnumsCodeTypeString = (m, y, z) => new FrameworkUAS.Service.Response<Code>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = new Code { CodeId = 1 },
                        Status = ServiceResponseStatusTypes.Success
                    },
                    SaveGuidCode = (m, y) =>
                    new FrameworkUAS.Service.Response<int>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = 1,
                        Status = ServiceResponseStatusTypes.Success
                    },
                    SelectGuidEnumsCodeType = (m, n) => new FrameworkUAS.Service.Response<List<Code>>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = CreateCodeListObject(),
                        Status = ServiceResponseStatusTypes.Success
                    }
                }
            };
        }

        private static void CreateSourceFileObject()
        {
            ShimServiceClientSet.AllInstances.SourceFileGet = (x) => new ShimServiceClient<ISourceFile>
            {
                ProxyGet = () => new StubISourceFile
                {
                    SaveGuidSourceFileBoolean = (m, n, k) => new FrameworkUAS.Service.Response<int>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = 1,
                        Status = ServiceResponseStatusTypes.Success
                    },
                    SelectGuidInt32Boolean = (m, n, v) => new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.SourceFile>>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = new List<FrameworkUAS.Entity.SourceFile>
                        {
                            new FrameworkUAS.Entity.SourceFile()
                        },
                        Status = ServiceResponseStatusTypes.Success
                    }
                }
            };
        }

        private static void CreateResponseGroupWorkerObject()
        {
            ShimServiceClientSet.AllInstances.ResponseGroupWorkerGet = (x) => new ShimServiceClient<UAD_WS.Interface.IResponseGroup>
            {
                ProxyGet = () => new StubIResponseGroup
                {
                    SelectGuidClientConnections = (m, n) => new FrameworkUAS.Service.Response<List<ResponseGroup>>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = new List<ResponseGroup> {
                            new ResponseGroup { IsRequired = true, PubID = 1, ResponseGroupName = DefaultGroupName } },
                        Status = ServiceResponseStatusTypes.Success
                    }
                }
            };
        }

        private static void CreateSubscriptionsExtensionMapperWorkerObject()
        {
            ShimServiceClientSet.AllInstances.SubscriptionsExtensionMapperWorkerGet = (x) => new ShimServiceClient<UAD_WS.Interface.ISubscriptionsExtensionMapper>
            {
                ProxyGet = () => new StubISubscriptionsExtensionMapper
                {
                    SelectAllGuidClientConnections = (m, n) => new FrameworkUAS.Service.Response<List<SubscriptionsExtensionMapper>>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = new List<SubscriptionsExtensionMapper> { new SubscriptionsExtensionMapper { CustomFieldDataType = BitFiled, CustomField = TestOne } }
                    }
                }
            };
        }

        private static void CreateProductSubscriptionsExtensionWorkerObject()
        {
            ShimServiceClientSet.AllInstances.ProductSubscriptionsExtensionWorkerGet = (x) => new ShimServiceClient<UAD_WS.Interface.IProductSubscriptionsExtension>
            {
                ProxyGet = () => new StubIProductSubscriptionsExtension
                {
                    SelectAllGuidClientConnections = (m, n) => new FrameworkUAS.Service.Response<List<ProductSubscriptionsExtensionMapper>>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = new List<ProductSubscriptionsExtensionMapper>
                        {
                            new ProductSubscriptionsExtensionMapper
                            {
                                CustomFieldDataType = BitFiled,
                                CustomField = TestOne
                            }
                        },
                        Status = ServiceResponseStatusTypes.Success
                    }
                }
            };
        }

        private void CreateFieldMappingObject()
        {
            ShimServiceClientSet.AllInstances.FieldMappingGet = (x) => new ShimServiceClient<IFieldMapping>
            {
                ProxyGet = () => new StubIFieldMapping
                {
                    SelectGuidInt32Boolean = (m, n, v) => new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FieldMapping>>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = new List<FrameworkUAS.Entity.FieldMapping> { new FrameworkUAS.Entity.FieldMapping() },
                        Status = ServiceResponseStatusTypes.Success
                    },
                    DeleteGuidInt32 = (m, n) => new FrameworkUAS.Service.Response<int>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = 1,
                        Status = ServiceResponseStatusTypes.Success
                    },
                    DeleteMappingGuidInt32 = (m, n) => new FrameworkUAS.Service.Response<int>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = 1,
                        Status = ServiceResponseStatusTypes.Success
                    },
                    SaveGuidFieldMapping = (m, n) =>
                    {
                        _saveFiledMapping = true;
                        return new FrameworkUAS.Service.Response<int>
                        {

                            Message = SuccessMessage,
                            ProcessCode = Guid.NewGuid().ToString(),
                            Result = 1,
                            Status = ServiceResponseStatusTypes.Success
                        };
                    },
                    ColumnReorderGuidInt32 = (m, n) => new FrameworkUAS.Service.Response<bool>
                    {
                        Message = SuccessMessage,
                        ProcessCode = Guid.NewGuid().ToString(),
                        Result = true,
                        Status = ServiceResponseStatusTypes.Success
                    }
                }
            };
        }
    }
}
