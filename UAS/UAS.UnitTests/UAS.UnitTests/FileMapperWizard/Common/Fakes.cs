using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Windows.Controls;
using System.Windows.Controls.Fakes;
using Core_AMS.Utilities.Fakes;
using FileMapperWizard.Controls.Fakes;
using FileMapperWizard.Modules.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FileMapperWizard.Controls;
using KM.Common.Import;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using NUnit.Framework;
using UAD_Lookup_WS.Interface;
using UAD_Lookup_WS.Interface.Fakes;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using UASService = FrameworkUAS.Service;
using UASWSInterface = UAS_WS.Interface;
using UASWSInterfaceFakes = UAS_WS.Interface.Fakes;
using static FrameworkUAD_Lookup.Enums;

namespace UAS.UnitTests.FileMapperWizard.Common
{
    public class Fakes
    {
        protected IDisposable shimObject;
        protected string _messageBoxMessage = string.Empty;
        protected bool _isPubMapSaved = false;
        protected TransformationPubMap _savedPubMap;
        protected bool _isDataMapSaved = false;
        protected TransformDataMap _savedDataMap;

        protected void SetupFakes()
        {
            shimObject = ShimsContext.Create();

            CreateUADFileMappingColumnClient();
            CreateUAS_ClientClient();
            CreateUAS_ServiceClient();
            CreateUAS_ServiceFeatureClient();
            CreateUAS_SourceFileClient();
            CreateUAS_FieldMappingClient();
            CreateUAS_FieldMultiMapClient();
            CreateUAS_TransformationFieldMapClient();
            CreateUAD_Lookup_CodeClient();
            CreateUAS_DBWorkerClient();
            CreateUAD_ProductClient();

            CreateUAS_TransformationClient();
            CreateUAS_TransformationPubMapClient();
            CreateUAS_TransformationFieldMapClient();
            CreateUAS_TransformDataMapClient();
            CreateUAD_Lookup_CodeClient();
            CreateUAS_TransformAssignClient();
            CreateUAS_TransformJoinClient();
            CreateUAS_TransformSplitClient();
            CreateUAS_TransformSplitTransClient();
            CreateUAS_TransformationFieldMultiMapClient();
            CreateUAD_SubscriptionsExtensionMapperClient();
            CreateUAD_ResponseGroupClient();
            CreateUAD_ProductSubscriptionsExtensionClient();

            ShimEditSetup.AllInstances.LoadData = (e) => { };
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageMessageBoxResultString = (message, y, z, p, q) => { _messageBoxMessage = message; };
            ShimWPF.FindVisualChildrenOf1DependencyObject<Border>(c => new List<Border>() { new Border { Name = "StepTwoContainer" } });
            ShimMapColumns.ConstructorFMUniversal = (map, container) => new ShimMapColumns();
            ShimSpecialFile.ConstructorFMUniversal = (map, container) => new ShimMapColumns();
            ShimDecorator.AllInstances.ChildSetUIElement = (c, u) => { };
            
            ShimColumnMapper.AllInstances.HasTransformationSetBoolean = (c, v) => { };
            ShimColumnMapper.AllInstances.ButtonTagSetString = (b, v) => { };
            ShimColumnMapper.AllInstances.CloseLabelRow = (c) => { };
        }

        protected void CreateUAS_FieldMappingClientWithEmptyResult()
        {
            ShimServiceClient.UAS_FieldMappingClient = () =>
            {
                return new ShimServiceClient<UASWSInterface.IFieldMapping>
                {
                    ProxyGet = () =>
                    {
                        return new UASWSInterfaceFakes.StubIFieldMapping
                        {
                            SelectGuidInt32Boolean = (g, i, b) =>
                            {
                                return new UASService.Response<List<FieldMapping>>
                                {
                                    Result = new List<FieldMapping>()
                                };
                            }
                        };
                    }
                };
            };
        }

        protected ShimFMUniversal GetShimFMUniversalContainer(bool isExcelZipFile = true)
        {
            var shim = new ShimFMUniversal();

            shim.InstanceBehavior = ShimBehaviors.Fallthrough;
            shim.Instance.AllPublishers = new List<Client>();
            shim.Instance.AllFeatures = new List<ServiceFeature> { new ServiceFeature { SFName = "Special Files", ServiceFeatureID = 1 } };
            shim.Instance.DatabaseFileTypeList = new List<Code> { new Code { CodeId = 1, CodeName = FileTypes.ACS.ToString() } };
            shim.Instance.myClient = new Client() { ClientID = 1 };
            shim.Instance.FileName = "SampleFile";
            shim.Instance.saveFileName = "SampleFile";
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
                GetDuplicateColumnsFileInfoFileConfiguration = (f, c) => new List<string> { "SomeConfig" },
                GetFileHeadersFileInfoFileConfigurationBoolean = (f, c, b) => new StringDictionary { { "PubCodeName", "TestPubCode" } },
                GetRowCountFileInfo = (f) => 1,
                GetDataFileInfoFileConfiguration = (f, c) => new DataTable()
            };

            shim.SetupToMapColumns = () => { };
            shim.SetupToSpecialFile = () => { };
            shim.ShowRules = () => { };
            shim.myFileConfigGet = () => new FileConfiguration();
            
            return shim;
        }

        protected void CreateUADFileMappingColumnClient()
        {
            ShimServiceClient.UAD_FileMappingColumnClient = () =>
            {
                return new ShimServiceClient<IFileMappingColumn>
                {
                    ProxyGet = () => 
                    {
                        return new StubIFileMappingColumn
                        {
                            SelectGuidClientConnections = (g, c) =>
                            {
                                return new UASService.Response<List<FileMappingColumn>>
                                {
                                    Result = new List<FileMappingColumn>
                                    {
                                        new FileMappingColumn()
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        protected void CreateUAS_FieldMappingClient()
        {
            ShimServiceClient.UAS_FieldMappingClient = () =>
            {
                return new ShimServiceClient<UASWSInterface.IFieldMapping>
                {
                    ProxyGet = () =>
                    {
                        return new UASWSInterfaceFakes.StubIFieldMapping
                        {
                            SelectGuidInt32Boolean = (g, i, b) =>
                            {
                                return new UASService.Response<List<FieldMapping>>
                                {
                                    Result = new List<FieldMapping>
                                    {
                                        new FieldMapping { FieldMappingTypeID = 0, IsNonFileColumn = true },
                                        new FieldMapping { FieldMappingTypeID = 1, IncomingField = string.Empty },
                                        new FieldMapping { FieldMappingTypeID = 2, IncomingField = string.Empty },
                                        new FieldMapping { FieldMappingTypeID = 3, IncomingField = string.Empty },
                                        new FieldMapping { FieldMappingTypeID = 4, IncomingField = string.Empty },
                                        new FieldMapping { FieldMappingTypeID = 5, IncomingField = string.Empty },
                                        new FieldMapping { FieldMappingTypeID = 6, IncomingField = string.Empty },
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_ClientClient()
        {
            ShimServiceClient.UAS_ClientClient = () =>
            {
                return new ShimServiceClient<IClient>
                {
                    ProxyGet = () =>
                    {
                        return new StubIClient
                        {
                            SelectGuidBoolean = (i, b) =>
                            {
                                return new UASService.Response<List<Client>>
                                {
                                    Result = new List<Client>
                                    {
                                        new Client { ClientID = 1, ClientName = "SomeClientName"}
                                    }
                                };
                            }
                        };
                    }

                };
            };
        }

        private void CreateUAS_ServiceClient()
        {
            ShimServiceClient.UAS_ServiceClient = () =>
            {
                return new ShimServiceClient<IService>
                {
                    ProxyGet = () =>
                    {
                        return new StubIService
                        {
                            SelectGuidBoolean = (g, b) =>
                            {
                                return new UASService.Response<List<Service>>
                                {
                                    Result = new List<Service>
                                    {
                                        new Service()
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_ServiceFeatureClient()
        {
            ShimServiceClient.UAS_ServiceFeatureClient = () =>
            {
                return new ShimServiceClient<IServiceFeature>();
            };
        }

        private void CreateUAS_SourceFileClient()
        {
            ShimServiceClient.UAS_SourceFileClient = () =>
            {
                return new ShimServiceClient<ISourceFile>
                {
                    ProxyGet = () =>
                    {
                        return new StubISourceFile
                        {
                            SelectGuidBoolean = (g, b) =>
                            {
                                return new UASService.Response<List<SourceFile>>
                                {
                                    Result = new List<SourceFile>
                                    {
                                        new SourceFile
                                        {
                                            ClientID = 1,
                                            FileName = "SampleFile",
                                            SourceFileID = 1,
                                            IsDeleted = false
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_FieldMultiMapClient()
        {
            ShimServiceClient.UAS_FieldMultiMapClient = () =>
            {
                return new ShimServiceClient<IFieldMultiMap>();
            };
        }

        private void CreateUAS_TransformationFieldMapClient()
        {
            ShimServiceClient.UAS_TransformationFieldMapClient = () =>
            {
                return new ShimServiceClient<ITransformationFieldMap>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformationFieldMap
                        {
                            SelectGuid = (g) =>
                            {
                                return new UASService.Response<List<TransformationFieldMap>>
                                {
                                    Result = new List<TransformationFieldMap>
                                    {
                                        new TransformationFieldMap { SourceFileID = 1 }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_DBWorkerClient()
        {
            ShimServiceClient.UAS_DBWorkerClient = () =>
            {
                return new ShimServiceClient<IDBWorker>
                {
                    ProxyGet = () =>
                    {
                        return new StubIDBWorker
                        {
                            GetPubIDAndCodesByClientGuidClient = (g, c) =>
                            {
                                return new UASService.Response<Dictionary<int, string>>
                                {
                                    Result = new Dictionary<int, string>
                                    {
                                        [1] = "TestPubCode" 
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_TransformationClient()
        {
            ShimServiceClient.UAS_TransformationClient = () =>
            {
                return new ShimServiceClient<ITransformation>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformation
                        {
                            SelectGuid = (g) =>
                            {
                                return new UASService.Response<List<Transformation>>
                                {
                                    Result = new List<Transformation>
                                    {
                                        new Transformation
                                        {
                                            TransformationID = 1,
                                            MapsPubCode = true,
                                            LastStepDataMap = true
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_TransformationPubMapClient()
        {
            ShimServiceClient.UAS_TransformationPubMapClient = () =>
            {
                return new ShimServiceClient<ITransformationPubMap>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformationPubMap
                        {
                            SelectGuid = (g) =>
                            {
                                return new UASService.Response<List<TransformationPubMap>>
                                {
                                    Result = new List<TransformationPubMap>
                                    {
                                        new TransformationPubMap()
                                    }
                                };
                            },
                            SaveGuidTransformationPubMap = (g, pubMap) =>
                            {
                                _isPubMapSaved = true;
                                _savedPubMap = pubMap;
                                return new UASService.Response<int>
                                {
                                    Result = 1
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAD_Lookup_CodeClient()
        {
            ShimServiceClient.UAD_Lookup_CodeClient = () =>
            {
                return new ShimServiceClient<ICode>
                {
                    ProxyGet = () =>
                    {
                        return new StubICode
                        {
                            SelectGuidEnumsCodeType = (g, c) =>
                            {
                                return new UASService.Response<List<Code>>
                                {
                                    Result = new List<Code>
                                    {
                                        new Code
                                        {
                                            CodeName = TransformationTypes.Data_Mapping.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 1
                                        },
                                        new Code
                                        {
                                            CodeName = FieldMappingTypes.Ignored.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 1
                                        },
                                        new Code
                                        {
                                            CodeName = FieldMappingTypes.kmTransform.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 2
                                        },
                                        new Code
                                        {
                                            CodeName = FieldMappingTypes.Standard.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 3
                                        },
                                        new Code
                                        {
                                            CodeName = FieldMappingTypes.Demographic.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 4
                                        },
                                        new Code
                                        {
                                            CodeName = FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 5
                                        },
                                        new Code
                                        {
                                            CodeName = FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' '),
                                            IsActive = true,
                                            CodeId = 6
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAS_TransformDataMapClient()
        {
            ShimServiceClient.UAS_TransformDataMapClient = () =>
            {
                return new ShimServiceClient<ITransformDataMap>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformDataMap
                        {
                            SelectForTransformationGuidInt32 = (g, i) =>
                            {
                                return new UASService.Response<List<TransformDataMap>>
                                {
                                    Result = new List<TransformDataMap>
                                    {
                                        new TransformDataMap{ PubID = 1}
                                    }
                                };
                            },
                            SaveGuidTransformDataMap = (g, dataMap) =>
                            {
                                _isDataMapSaved = true;
                                _savedDataMap = dataMap;
                                return new UASService.Response<int>
                                {
                                    Result = 1
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAD_ProductClient()
        {
            ShimServiceClient.UAD_ProductClient = () =>
            {
                return new ShimServiceClient<IProduct>();
            };
        }

        private void CreateUAS_TransformAssignClient()
        {
            ShimServiceClient.UAS_TransformAssignClient = () =>
            {
                return new ShimServiceClient<ITransformAssign>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformAssign()
                        {

                        };
                    }
                };
            };
        }

        private void CreateUAS_TransformJoinClient()
        {
            ShimServiceClient.UAS_TransformJoinClient = () =>
            {
                return new ShimServiceClient<ITransformJoin>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformJoin
                        {

                        };
                    }
                };
            };
        }

        private void CreateUAS_TransformSplitClient()
        {
            ShimServiceClient.UAS_TransformSplitClient = () =>
           {
               return new ShimServiceClient<ITransformSplit>
               {
                   ProxyGet = () =>
                   {
                       return new StubITransformSplit
                       {

                       };
                   }
               };
           };
        }

        private void CreateUAS_TransformSplitTransClient()
        {
            ShimServiceClient.UAS_TransformSplitTransClient = () =>
            {
                return new ShimServiceClient<ITransformSplitTrans>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformSplitTrans
                        {

                        };
                    }
                };
            };
        }

        private void CreateUAS_TransformationFieldMultiMapClient()
        {
            ShimServiceClient.UAS_TransformationFieldMultiMapClient = () =>
            {
                return new ShimServiceClient<ITransformationFieldMultiMap>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformationFieldMultiMap
                        {
                            SelectGuid = (g) => 
                            {
                                return new UASService.Response<List<TransformationFieldMultiMap>>
                                {
                                    Result = new List<TransformationFieldMultiMap>
                                    {
                                        new TransformationFieldMultiMap()
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAD_SubscriptionsExtensionMapperClient()
        {
            ShimServiceClient.UAD_SubscriptionsExtensionMapperClient = () =>
            {
                return new ShimServiceClient<ISubscriptionsExtensionMapper>
                {
                    ProxyGet = () =>
                    {
                        return new StubISubscriptionsExtensionMapper
                        {
                            SelectAllGuidClientConnections = (g, c) =>
                            {
                                return new UASService.Response<List<SubscriptionsExtensionMapper>>
                                {
                                    Result = new List<SubscriptionsExtensionMapper>
                                    {
                                        new SubscriptionsExtensionMapper { CustomFieldDataType  = "bit" }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAD_ProductSubscriptionsExtensionClient()
        {
            ShimServiceClient.UAD_ProductSubscriptionsExtensionClient = () =>
            {
                return new ShimServiceClient<IProductSubscriptionsExtension>
                {
                    ProxyGet = () =>
                    {
                        return new StubIProductSubscriptionsExtension
                        {
                            SelectAllGuidClientConnections = (g,c) => 
                            {
                                return new UASService.Response<List<ProductSubscriptionsExtensionMapper>>
                                {
                                    Result = new List<ProductSubscriptionsExtensionMapper>
                                    {
                                        new ProductSubscriptionsExtensionMapper { CustomFieldDataType = "bit"}
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUAD_ResponseGroupClient()
        {
            ShimServiceClient.UAD_ResponseGroupClient = () =>
            {
                return new ShimServiceClient<IResponseGroup>
                {
                    ProxyGet = () =>
                    {
                        return new StubIResponseGroup
                        {
                            SelectGuidClientConnections = (g,c) => 
                            {
                                return new UASService.Response<List<ResponseGroup>>
                                {
                                    Result = new List<ResponseGroup>
                                    {
                                        new ResponseGroup{ IsRequired = true, PubID = 1}
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        [TearDown]
        public void DisposeContext()
        {
            shimObject.Dispose();
        }
    }
}