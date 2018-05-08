using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UAS.Web.Controllers.Dashboard;
using UAS.Web.Models.Circulations;
using NUnit.Framework;
using KM.Platform.Fakes;
using KMPFakes = KMPlatform.BusinessLogic.Fakes;
using KMPDataFakes = KMPlatform.DataAccess.Fakes;
using KMPEntity = KMPlatform.Entity;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using FrameworkUAS.BusinessLogic.Fakes;

namespace UAS.Web.Tests.Controllers.Dashboard
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class DashboardControllerTest : ControllerTestBase
    {
        private DashboardController _controller;        
        private bool _hasAccessReturn = true;
        private List<Product> _products;
        private List<AdmsLog> _admsLogs;
        private List<SourceFile> _sourceFiles;
        private bool _errorWasLogged;
        private List<Code> _codeList;
        private const string DummyStringValue = "dummyStringValue";
        private const string DummyIntValue = "0";
        private const string PubCode = "PBC";
        private const string RecordSource_CiRC = "CIRC";
        private const string RecordSource_API = "API";
        private const string RecordSource_DataCompare = "Data Compare";
        private const string SessionKeyCodeList = "BaseControlller_CodeList";
        private const int PubID = 10;
        private const int DummyInt = 36;        

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _controller = new DashboardController();
            Initialize(_controller);            
            InitValues();
            InitShims();
        }

        private void InitShims()
        {
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (currentUser, service, feature, access) => _hasAccessReturn;

            KMPFakes.ShimService.Constructor = (service) =>
            {
                new KMPFakes.ShimService(service)
                {
                    SelectBoolean = (inclueObj) => new List<KMPEntity.Service>()
                    {
                        new KMPEntity.Service()
                    }
                };
            };

            _products = new List<Product>()
            {
                new Product()
                {
                    PubID = PubID,
                    PubCode = PubCode
                }
            };
            ShimProduct.Constructor = (product) =>
            {
                new ShimProduct(product)
                {
                    SelectClientConnectionsBoolean = (conn, includeProp) => _products
                };
            };

            _codeList = new List<Code>()
            {
                NewCode(FrameworkUAD_Lookup.Enums.CodeType.File_Status.ToString(), "",DummyInt, DummyInt, DummyStringValue, DummyInt),
                NewCode(FrameworkUAD_Lookup.Enums.RecordProcessTimeType.RecordProcessTime_0_15K.ToString().Replace("_", " "), DummyIntValue),
                NewCode(FrameworkUAD_Lookup.Enums.RecordProcessTimeType.RecordProcessTime_15_50K.ToString().Replace("_", " "), DummyIntValue, DummyInt),
                NewCode(FrameworkUAD_Lookup.Enums.RecordProcessTimeType.RecordProcessTime_50_100K.ToString().Replace("_", " "), DummyIntValue),
                NewCode(FrameworkUAD_Lookup.Enums.RecordProcessTimeType.RecordProcessTime_100_Max.ToString().Replace("_", " "),DummyIntValue)                
            };
            ShimCode.Constructor = (code) =>
            {
                new ShimCode(code)
                {
                    SelectEnumsCodeType = (type) => _codeList,
                    Select = () => _codeList
                };
            };

            _admsLogs = new List<AdmsLog>()
            {
                new AdmsLog()
                {
                    SourceFileId = DummyInt,
                    RecordSource = DummyStringValue,
                    FileStatusId = DummyInt,
                    StatusMessage = DummyStringValue,
                    ProcessingStatusId = DummyInt,
                    FileNameExact = DummyStringValue,
                    FileStart = DateTime.Now,
                    OriginalRecordCount = DummyInt
                }
            };
            ShimAdmsLog.Constructor = (adms) =>
            {
                new ShimAdmsLog(adms)
                {
                    SelectInt32AdmsLogRecordSourceNullableOfDateTimeNullableOfDateTime = (CurrentClientID, recordSource, StartDate, EndDate) => _admsLogs,
                    SelectNotCompleteNotFailedInt32 = (clientId) => _admsLogs
                };
            };

            _sourceFiles = new List<SourceFile>()
            {
                new SourceFile()
                {
                    SourceFileID = DummyInt,
                    FileName = DummyStringValue,
                    PublicationID = PubID,
                    DatabaseFileTypeId = DummyInt
                }
            };

            ShimSourceFile.Constructor = (sourceFile) =>
            {
                new ShimSourceFile(sourceFile)
                {
                    SelectListOfInt32Boolean = (sourceFields, includeProp) => _sourceFiles
                };
            };

            KMPFakes.ShimApplicationLog.Constructor = (appLog) =>
            {
                new KMPFakes.ShimApplicationLog(appLog)
                {
                    LogCriticalErrorStringStringEnumsApplicationsStringInt32String = (ex, source, application, note, clientID, subject) => DummyInt
                };
                _errorWasLogged = true;
            };

            ShimEngineLog.Constructor = (engineLog) =>
            {
                new ShimEngineLog(engineLog)
                {
                    SelectInt32 = (clientId) => new List<EngineLog>()
                };
            };

            KMPFakes.ShimClient.Constructor = (client) =>
            {
                new KMPFakes.ShimClient(client)
                {
                    SelectInt32Boolean = (clientId, props) => new KMPEntity.Client()
                };
            };

            KMPDataFakes.ShimClient.SelectProductClient = (client) => new List<KMPlatform.Object.Product>()
            {
                new KMPlatform.Object.Product()
                {
                    ProductID = PubID,
                    ProductCode = PubCode
                }
            };
        }

        private static Code NewCode(string codeName, string codeValue = "", int codeId = 0, int codeTypeId = 0, string displayName = "", int displayOrder = 0)
        {
            return new Code()
            {
                CodeId = codeId,
                CodeName = codeName,
                CodeValue = codeValue,
                CodeTypeId = codeTypeId,
                DisplayName = displayName,
                DisplayOrder = displayOrder
            };
        }

        private void InitValues()
        {
            _hasAccessReturn = true;
            _errorWasLogged = false;
        }

        private static FileHistorySearch CreateModel(bool isCirc = true, string recordSource = RecordSource_CiRC)
        {
            var fileHistory = new FileHistorySearch()
            {
                RecordSource = recordSource,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.MaxValue,
                FileName = DummyStringValue,
                PubID = PubID,
                isCirc = isCirc,
                FileTypeID = DummyInt
            };

            return fileHistory;
        }
    }
}
