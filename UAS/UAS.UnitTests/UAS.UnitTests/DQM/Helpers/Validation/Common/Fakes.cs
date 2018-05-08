using System;
using System.Collections.Generic;
using DQM.Helpers.Validation.Fakes;
using FrameworkServices;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAD_Lookup_WS.Interface;
using UAD_Lookup_WS.Interface.Fakes;
using UAD_WS.Interface;
using UAD_WS.Interface.Fakes;
using UAS_WS.Interface;
using UAS_WS.Interface.Fakes;
using FrameworkUASService = FrameworkUAS.Service;
using UADLookUp = FrameworkUAD_Lookup;
using CoreAMSUtilitiesEnums = Core_AMS.Utilities.Enums;
using List = Telerik.Windows.Documents.Fixed.Model.Editing.Lists.List;

namespace UAS.UnitTests.DQM.Helpers.Validation.Common
{
    public class Fakes
    {
        protected IDisposable shimObject;

        public void SetupFakes()
        {
            shimObject = ShimsContext.Create();
            ShimMethod();
            ShimFileValidator.AllInstances.ApplyAdHocDimensions = (x) => { };
            ShimFileValidator.AllInstances.ValidateData = (x) => { };
        }
        public void InitializeFakes()
        {
            shimObject = ShimsContext.Create();
            ShimMethod();
        }

        private void ShimMethod()
        {
            CreateUadImportErrorSummaryClient();
            CreateUasAdHocDimensionGroupClient();
            CreateUasSourceFileClient();
            CreateUasDbWorkerClient();
            CreateUasServiceClient();
            CreateUASTransformationClient();
            CreateUasServiceFeatureClient();
            CreateUadSubscriberTransformedClient();
            CreateUASTransformSplitClient();
            CreateUADResponseGroupClient();
            CreateUadFileValidatorImportErrorClient();
            CreateUADSubscriberInvalidClient();
            CreateUadCodeSheetClient();
            CreateUadOperationsClient();
            CreateUadLookupCodeClient();
            CreateUasTransformAssignClient();
            CreateUasTransformDataMapClient();
            CreateUASTransformJoinClient();
            CreateUASTransformSplitTransClient();
        }

        [TearDown]
        public void DisposeContext()
        {
            shimObject.Dispose();
        }

        protected void CreateUADSubscriberInvalidClient(bool result = true)
        {
            ShimServiceClient.UAD_SubscriberInvalidClient = () =>
            {
                return new ShimServiceClient<ISubscriberInvalid>
                {
                    ProxyGet = () =>
                    {
                        return new StubISubscriberInvalid
                        {
                            SaveBulkSqlInsertGuidListOfSubscriberInvalidClientConnections = (accessKey, dedupList, Conn) =>
                            {
                                return new FrameworkUASService.Response<bool>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = result
                                };
                            }
                        };
                    }
                };
            };
        }

        protected void CreateUadSubscriberTransformedClient(bool result = false)
        {
            ShimServiceClient.UAD_SubscriberTransformedClient = () =>
            {
                return new ShimServiceClient<ISubscriberTransformed>
                {
                    ProxyGet = () =>
                    {
                        return new StubISubscriberTransformed
                        {
                            SaveBulkSqlInsertGuidListOfSubscriberTransformedClientConnectionsBoolean = (key, deduplist, clientConn, isDataCompare) =>
                            {
                                return new FrameworkUASService.Response<bool>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = result
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUasTransformAssignClient()
        {
            ShimServiceClient.UAS_TransformAssignClient = () =>
            {
                return new ShimServiceClient<ITransformAssign>()
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformAssign()
                        {
                            SelectForSourceFileGuidInt32 = (_, __) => 
                                new FrameworkUASService.Response<List<TransformAssign>>()
                                {
                                    Result = new List<TransformAssign>()
                                }
                        };
                    }
                };
            };
        }

        private void CreateUasTransformDataMapClient()
        {
            ShimServiceClient.UAS_TransformDataMapClient = () =>
            {
                return new ShimServiceClient<ITransformDataMap>()
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformDataMap()
                        {
                            SelectForSourceFileGuidInt32 = (_, __) =>
                                new FrameworkUASService.Response<List<TransformDataMap>>()
                                {
                                    Result = new List<TransformDataMap>()
                                },
                            SelectGuid = _ => 
                                new FrameworkUASService.Response<List<TransformDataMap>>()
                                {
                                    Result = new List<TransformDataMap>()
                                    {
                                        new TransformDataMap()
                                        {
                                            TransformationID = 0,
                                            IsActive = true,
                                            MatchType = string.Empty,
                                            SourceData = string.Empty,
                                            DesiredData = string.Empty
                                        }
                                    }
                                }
                        };
                    }
                };
            };
        }

        private void CreateUasSourceFileClient()
        {
            ShimServiceClient.UAS_SourceFileClient = () =>
            {
                return new ShimServiceClient<ISourceFile>
                {
                    ProxyGet = () =>
                    {
                        return new StubISourceFile()
                        {
                            SelectForSourceFileGuidInt32Boolean = (x, y, z) =>
                            {
                                return new FrameworkUASService.Response<SourceFile>
                                {
                                    Result = new SourceFile
                                    {
                                        IsSpecialFile = true,
                                        IsTextQualifier = false,
                                        QDateFormat = "MDDYY",
                                        FieldMappings = new HashSet<FieldMapping>
                                        {
                                            new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField="PubCode",
                                                ColumnOrder=1,
                                                IncomingField=string.Empty,
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=1,
                                            },
                                            new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField="b",
                                                ColumnOrder=1,
                                                IncomingField="b",
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=3
                                            },
                                            new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField=CoreAMSUtilitiesEnums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                                                ColumnOrder=1,
                                                IncomingField=CoreAMSUtilitiesEnums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=4
                                            },
                                             new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField=CoreAMSUtilitiesEnums.MAFFieldStandardFields.COMPANY.ToString(),
                                                ColumnOrder=1,
                                                IncomingField=CoreAMSUtilitiesEnums.MAFFieldStandardFields.COMPANY.ToString(),
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=5
                                            },
                                              new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField=CoreAMSUtilitiesEnums.MAFFieldStandardFields.COPIES.ToString(),
                                                ColumnOrder=1,
                                                IncomingField=CoreAMSUtilitiesEnums.MAFFieldStandardFields.COPIES.ToString(),
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=5
                                            }
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUasAdHocDimensionGroupClient()
        {
            ShimServiceClient.UAS_AdHocDimensionGroupClient = () =>
            {
                return new ShimServiceClient<IAdHocDimensionGroup>
                {
                    ProxyGet = () =>
                    {
                        return new StubIAdHocDimensionGroup()
                        {
                            SelectByAdHocDimensionGroupIdGuidInt32Boolean = (accessKey, clientID, status) =>
                            {
                                return new FrameworkUASService.Response<AdHocDimensionGroup>
                                {
                                    Result = new AdHocDimensionGroup
                                    {

                                    }
                                };
                            },
                            SelectGuidInt32Boolean = (accessKey, clientID, status) =>
                            {
                                return new FrameworkUASService.Response<List<AdHocDimensionGroup>>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<AdHocDimensionGroup>
                                              {
                                                    new AdHocDimensionGroup
                                                    {
                                                        IsActive=true,
                                            CreatedDimension = "a",
                                            AdHocDimensionGroupName = "Scranton_Company_Survey",
                                            AdHocDimensionGroupId = 1,
                                            AdHocDimensions = CreateAdHocDimension()
                                                    },
                                                    new AdHocDimensionGroup
                                                    {
                                                        IsActive=true,
                                            CreatedDimension = "PubCode",
                                            AdHocDimensionGroupName ="Scranton_Domains_Survey",
                                            AdHocDimensionGroupId = 2,
                                            AdHocDimensions = CreateAdHocDimension()
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
                                              }
                                };
                            }

                        };
                    }
                };
            };
        }

        private void CreateUadImportErrorSummaryClient()
        {
            ShimServiceClient.UAD_ImportErrorSummaryClient = () =>
            {
                return new ShimServiceClient<IImportErrorSummary>
                {
                    ProxyGet = () =>
                    {
                        return new StubIImportErrorSummary()
                        {
                            SelectGuidInt32StringClientConnections = (x, y, z, c) =>
                            {
                                return new FrameworkUASService.Response<List<ImportErrorSummary>>
                                {
                                    Result = new List<ImportErrorSummary>
                                    {
                                        new ImportErrorSummary
                                        {
                                             ClientMessage=string.Empty,
                                              ErrorCount=0,
                                               MAFField="a",
                                                PubCode="a",
                                                 Value="1"
                                        },
                                        new ImportErrorSummary
                                        {
                                             ClientMessage=string.Empty,
                                              ErrorCount=0,
                                               MAFField="b",
                                                PubCode="b",
                                                 Value="2"
                                        }
                                    },
                                    Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUadOperationsClient()
        {
            ShimServiceClient.UAD_OperationsClient = () =>
            {
                return new ShimServiceClient<IOperations>
                {
                    ProxyGet = () =>
                    {
                        return new StubIOperations();
                    }
                };
            };
        }

        private void CreateUadCodeSheetClient()
        {
            ShimServiceClient.UAD_CodeSheetClient = () =>
            {
                return new ShimServiceClient<ICodeSheet>
                {
                    ProxyGet = () =>
                    {
                        return new StubICodeSheet();
                    }
                };
            };
        }

        private void CreateUasServiceFeatureClient()
        {
            ShimServiceClient.UAS_ServiceFeatureClient = () =>
            {
                return new ShimServiceClient<IServiceFeature>
                {
                    ProxyGet = () =>
                    {
                        return new StubIServiceFeature()
                        {
                            SelectServiceFeatureGuidInt32 = (accessKey, serviceID) =>
                            {
                                return new FrameworkUASService.Response<ServiceFeature>
                                {
                                    Result = new ServiceFeature()
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUasServiceClient()
        {
            ShimServiceClient.UAS_ServiceClient = () =>
            {
                return new ShimServiceClient<IService>
                {
                    ProxyGet = () =>
                    {
                        return new StubIService()
                        {
                            SelectGuidInt32Boolean = (accessKey, serviceID, includeObjects) =>
                            {
                                return new FrameworkUASService.Response<Service>
                                {
                                    Result = new Service()
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUASTransformationClient()
        {
            ShimServiceClient.UAS_TransformationClient = () =>
            {
                return new ShimServiceClient<ITransformation>
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformation()
                        {
                            SelectGuidInt32Int32Boolean = (key, clientId, sourceFileId, isCustomProperties) =>
                            {
                                return new FrameworkUASService.Response<List<Transformation>>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<Transformation>
                                    {
                                        new Transformation
                                        {
                                            IsActive = true,
                                            TransformationTypeID = 6,
                                            FieldMap = new HashSet<TransformationFieldMap>
                                            {
                                                new TransformationFieldMap { SourceFileID = sourceFileId },
                                                new TransformationFieldMap
                                                {
                                                    SourceFileID = sourceFileId,
                                                    CreatedByUserID = 1
                                                }
                                            },
                                            TransformationName = string.Empty,
                                            TransformationDescription = string.Empty,
                                            PubMap = new HashSet<TransformationPubMap>()
                                            {
                                                new TransformationPubMap()
                                                {
                                                    PubID = 1
                                                }
                                            }
                                        },
                                        new Transformation()
                                        {
                                            IsActive = true,
                                            TransformationTypeID = 10,
                                            FieldMap = new HashSet<TransformationFieldMap>
                                            {
                                                new TransformationFieldMap { SourceFileID = sourceFileId },
                                                new TransformationFieldMap
                                                {
                                                    SourceFileID = sourceFileId,
                                                    CreatedByUserID = 1
                                                }
                                            },
                                            TransformationName = string.Empty,
                                            TransformationDescription = string.Empty
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUASTransformSplitClient()
        {
            ShimServiceClient.UAS_TransformSplitClient = () =>
            {
                return new ShimServiceClient<ITransformSplit>()
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformSplit()
                        {
                            SelectForSourceFileGuidInt32 = (key, sourceFileId) =>
                            {
                                return new FrameworkUASService.Response<List<TransformSplit>>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<TransformSplit>
                                    {
                                        new TransformSplit
                                        {
                                            IsActive = true,
                                            Delimiter = "comma"
                                        }
                                    }
                                };
                            },
                            SelectGuid = _ =>
                            {
                                return new FrameworkUASService.Response<List<TransformSplit>>()
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<TransformSplit>
                                    {
                                        new TransformSplit
                                        {
                                            TransformationID = 1,
                                            IsActive = true,
                                            Delimiter = "comma"
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUASTransformJoinClient()
        {
            ShimServiceClient.UAS_TransformJoinClient = () =>
            {
                return new ShimServiceClient<ITransformJoin>()
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformJoin()
                        {
                            SelectForSourceFileGuidInt32 = (key, sourceFileId) =>
                            {
                                return new FrameworkUASService.Response<List<TransformJoin>>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<TransformJoin>() 
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUASTransformSplitTransClient()
        {
            ShimServiceClient.UAS_TransformSplitTransClient = () =>
            {
                return new ShimServiceClient<ITransformSplitTrans>()
                {
                    ProxyGet = () =>
                    {
                        return new StubITransformSplitTrans()
                        {
                            SelectSourceFileIDGuidInt32  = (key, sourceFileId) =>
                            {
                                return new FrameworkUASService.Response<List<TransformSplitTrans>>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<TransformSplitTrans>()
                                    {
                                        new TransformSplitTrans()
                                        {
                                            IsActive = true,
                                            TransformationID = 0,
                                            DataMapID = 0,
                                            Column = "TEST",
                                            SplitAfterID = 1,
                                            SplitBeforeID = 1,
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUADResponseGroupClient()
        {
            ShimServiceClient.UAD_ResponseGroupClient = () =>
            {
                return new ShimServiceClient<IResponseGroup>()
                {
                    ProxyGet = () =>
                    {
                        return new StubIResponseGroup()
                        {
                            SelectGuidClientConnectionsInt32 = (key, sourceFileId,conn) =>
                            {
                                return new FrameworkUASService.Response<List<ResponseGroup>>
                                {
                                    Status = UADLookUp.Enums.ServiceResponseStatusTypes.Success,
                                    Result = new List<ResponseGroup>
                                    {
                                        new ResponseGroup
                                        {
                                            IsActive = true,
                                            IsRequired = true,
                                            ResponseGroupName = "SampleResponseGroup"
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }
        
        private void CreateUadFileValidatorImportErrorClient()
        {
            ShimServiceClient.UAD_FileValidator_ImportErrorClient = () =>
            {
                return new ShimServiceClient<IFileValidator_ImportError>
                {
                    ProxyGet = () =>
                    {
                        return new StubIFileValidator_ImportError();
                    }
                };
            };
        }

        private void CreateUasDbWorkerClient()
        {
            ShimServiceClient.UAS_DBWorkerClient = () =>
            {
                return new ShimServiceClient<IDBWorker>
                {
                    ProxyGet = () =>
                    {
                        return new StubIDBWorker()
                        {
                            GetPubIDAndCodesByClientGuidClient = (x, y) =>
                            {
                                return new FrameworkUASService.Response<Dictionary<int, string>>
                                {
                                    Result = new Dictionary<int, string> { { 1, "PUBCODE" } }
                                };
                            }
                        };
                    }
                };
            };
        }

        private void CreateUadLookupCodeClient()
        {
            ShimServiceClient.UAD_Lookup_CodeClient = () =>
            {
                List<Code> listCode = new List<Code>();
                listCode.Add(new Code
                {
                    CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(),
                    CodeId = 1
                });
                listCode.Add(new Code
                {
                    CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString(),
                    CodeId = 2
                });
                listCode.Add(new Code
                {
                    CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(),
                    CodeId = 3
                });
                listCode.Add(new Code
                {
                    CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '),
                    CodeId = 4
                });
                listCode.Add(new Code
                {
                    CodeName = CoreAMSUtilitiesEnums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                    CodeId = 5
                });
                listCode.Add(new Code
                {
                    CodeName = UADLookUp.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' '),
                    CodeId = 6
                });
                listCode.Add(new Code
                {
                    CodeName = UADLookUp.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' '),
                    CodeId = 7
                });
                listCode.Add(new Code
                {
                    CodeName = UADLookUp.Enums.TransformationTypes.Join_Columns.ToString().Replace('_', ' '),
                    CodeId = 8
                });
                listCode.Add(new Code
                {
                    CodeName = UADLookUp.Enums.TransformationTypes.Assign_Value.ToString().Replace('_', ' '),
                    CodeId = 9
                });
                listCode.Add(new Code
                {
                    CodeName = UADLookUp.Enums.TransformationTypes.Split_Transform.ToString().Replace('_', ' '),
                    CodeId = 10
                });
                return new ShimServiceClient<ICode>
                {
                    ProxyGet = () =>
                    {
                        return new StubICode()
                        {
                            SelectGuidEnumsCodeType = (accessKey, codeType) =>
                            {
                                return new FrameworkUASService.Response<List<Code>>
                                {
                                    Result = listCode
                                };
                            },
                            SelectCodeIdGuidInt32 = (accessKey, databaseFileTypeId) =>
                            {
                                return new FrameworkUASService.Response<Code>
                                {
                                    Result = new Code
                                    {
                                        CodeId = 1,
                                        CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(),
                                    }
                                };
                            },
                        };
                    }
                };
            };
        }

        private List<AdHocDimension> CreateAdHocDimension()
        {
            return new List<AdHocDimension>
            {
                new AdHocDimension
                {
                    MatchValue = "KMAllUAS:test@unittest.com:unittest.com",
                },
                new AdHocDimension
                {
                    MatchValue = "KMAllDQM:admin@unittest.com:unittest.com",
                },
                 new AdHocDimension
                {
                    MatchValue = "com",
                },
                 new AdHocDimension
                {
                    MatchValue = "uk",
                },
            };
    }
}
}
