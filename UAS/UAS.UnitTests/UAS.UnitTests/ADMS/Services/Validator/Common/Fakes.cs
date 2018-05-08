using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Fakes;
using ADMS.Services;
using ADMS.Services.DataCleanser;
using ADMS.Services.DataCleanser.Fakes;
using ADMS.Services.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic;
using FrameworkUAS.BusinessLogic.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using UAS.UnitTests.Helpers;
using System.IO;
using Enums = FrameworkUAD_Lookup.Enums;
using Product = FrameworkUAD.Entity.Product;
using SourceFile = FrameworkUAS.Entity.SourceFile;
using SubscriberOriginal = FrameworkUAD.BusinessLogic.SubscriberOriginal;
using SubscriberTransformed = FrameworkUAD.BusinessLogic.SubscriberTransformed;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using UasBusinessLogic = FrameworkUAS.BusinessLogic;
using UasEntity = FrameworkUAS.Entity;
using DataAccessFakes = FrameworkUAD.DataAccess.Fakes;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;

using FrameworkUADEntity = FrameworkUAD.Entity;
using FrameworkUADBusinessLogic = FrameworkUAD.BusinessLogic;
using KMCommonShimDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.ADMS.Services.Validator.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        protected string ProcessCode;
        protected string sqlCommandText = string.Empty;
        protected bool isSqlCommandExecuted = false;
        private Mocks mocks;
        private IDisposable context;

        public void SetupFakes(Mocks validatorMocks)
        {
            context = ShimsContext.Create();

            mocks = validatorMocks;
            var dateTime = new DateTime(2018, 2, 08, 10, 10, 10);

            ShimDateTime.NowGet = () => dateTime;
            ShimDateTime.TodayGet = () => dateTime;
            ShimForSqlConnection();
            ShimForSqlCommand(1);
            ShimServiceBase.AllInstances.ConsoleMessageStringStringBooleanInt32Int32 = ConsoleMessage;
            ShimServiceBase.AllInstances.LogErrorExceptionClientStringBooleanBoolean = LogError;
            ShimProduct.AllInstances.SelectClientConnectionsBoolean = Select;
            ShimEmailStatus.AllInstances.SelectClientConnections = Select;
            ShimConfigurationManager.AppSettingsGet = () => new NameValueCollection
            {
                {"IsDemo", "true"},
                {"LogTransformationDetail", "true"}
            };
            ShimClient.AllInstances.SelectInt32Boolean = Select;
            ShimSourceFile.AllInstances.SelectInt32StringBoolean = Select;
            ShimCode.AllInstances.SelectEnumsCodeType = Select;
            ShimService.AllInstances.SelectEnumsServicesBoolean = Select;
            ShimAdmsLog.AllInstances.SaveAdmsLog = Save;
            ShimDQMQue.AllInstances.SaveDQMQue = Save;
            ShimAddressClean.AllInstances.ExecuteAddressCleanseAdmsLogClient = ExecuteAddressCleanse;
            ShimAdmsLog.AllInstances
                    .UpdateStringEnumsFileStatusTypeEnumsADMS_StepTypeEnumsProcessingStatusTypeEnumsExecutionPointTypeInt32StringBooleanInt32Boolean
                = Update;
            ShimTransactionCode.AllInstances.SelectActiveIsFreeBoolean = SelectActiveIsFree;
            ShimCategoryCode.AllInstances.SelectActiveIsFreeBoolean = SelectActive;
            ShimProductSubscriptionsExtensionMapper.AllInstances.SelectAllClientConnections = Select;
            ShimSubscriberOriginal.AllInstances.SaveBulkSqlInsertListOfSubscriberOriginalClientConnections = SaveBulkSqlInsert;
            ShimSubscriberTransformed.AllInstances.SaveBulkSqlInsertListOfSubscriberTransformedClientConnectionsBoolean = SaveBulkSqlInsert;
            ShimTransformation.AllInstances.SelectInt32Int32Boolean = Select;
            ShimTransformAssign.AllInstances.SelectSourceFileIDInt32 = SelectSourceFileId;
            ShimTransformJoin.AllInstances.SelectSourceFileIDInt32 = SelectSourceFileId;
            ShimTransformSplit.AllInstances.SelectSourceFileIDInt32 = SelectSourceFileId;
            ShimTransformSplit.AllInstances.Select = Select;
            ShimTransformSplitTrans.AllInstances.SelectSourceFileIDInt32 = SelectSourceFileId;
            ShimTransformDataMap.AllInstances.SelectSourceFileIDInt32 = SelectSourceFileId;
            ShimTransformDataMap.AllInstances.Select = Select;
            ShimServiceFeature.AllInstances.SelectServiceFeatureInt32 = SelectServiceFeature;
            KMCommonShimDataFunctions.GetSqlConnectionString = s => new SqlConnection();
            DataAccessFakes.ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new SqlConnection();
        }

        [TearDown]
        public void DisposeContext()
        {
            context.Dispose();
            var testOperationsFilesDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "FinalFiles");
            if (Directory.Exists(testOperationsFilesDirectoryPath))
            {
                Directory.Delete(testOperationsFilesDirectoryPath, recursive: true);
            }
        }

        protected virtual List<UasEntity.TransformDataMap> Select(TransformDataMap tr)
        {
            mocks.TransformDataMap.Object.Select();

            return new List<UasEntity.TransformDataMap>();
        }

        protected virtual List<UasEntity.TransformSplit> Select(TransformSplit tr)
        {
            mocks.TransformSplit.Object.Select();

            return new List<UasEntity.TransformSplit>();
        }

        protected virtual ServiceFeature SelectServiceFeature(KMPlatform.BusinessLogic.ServiceFeature serviceFeature,
            int serviceFeatureId)
        {
            mocks.ServiceFeature.Object.SelectServiceFeature(serviceFeatureId);

            return typeof(ServiceFeature).CreateInstance();
        }

        protected virtual List<UasEntity.TransformAssign> SelectSourceFileId(TransformAssign tr, int id)
        {
            mocks.TransformAssign.Object.SelectSourceFileId(id);

            return new List<UasEntity.TransformAssign>();
        }

        protected virtual List<UasEntity.TransformDataMap> SelectSourceFileId(TransformDataMap tr, int id)
        {
            mocks.TransformDataMap.Object.SelectSourceFileID(id);

            return new List<UasEntity.TransformDataMap>();
        }

        protected virtual List<UasEntity.TransformJoin> SelectSourceFileId(TransformJoin tr, int id)
        {
            mocks.TransformJoin.Object.SelectSourceFileId(id);

            return new List<UasEntity.TransformJoin>();
        }

        protected virtual List<UasEntity.TransformSplit> SelectSourceFileId(TransformSplit tr, int id)
        {
            mocks.TransformSplit.Object.SelectSourceFileID(id);

            return new List<UasEntity.TransformSplit>();
        }

        protected virtual List<UasEntity.TransformSplitTrans> SelectSourceFileId(TransformSplitTrans tr, int id)
        {
            mocks.TransformSplitTrans.Object.SelectSourceFileId(id);

            return new List<UasEntity.TransformSplitTrans>();
        }

        protected virtual List<UasEntity.Transformation> Select(Transformation transformation,
            int clientId, int sourceFileId, bool includeCustomProperties)
        {
            mocks.Transformation.Object.Select(clientId, sourceFileId, includeCustomProperties);

            return new List<UasEntity.Transformation>();
        }

        protected virtual bool SaveBulkSqlInsert(SubscriberTransformed subscriberTransformed,
            List<FrameworkUADEntity.SubscriberTransformed> subscriberTransformeds, ClientConnections clientConnections,
            bool isDataCompare)
        {
            mocks.SubscriberTransformed.Object.SaveBulkSqlInsert(subscriberTransformeds, clientConnections,
                isDataCompare);
            return true;
        }

        protected virtual bool SaveBulkSqlInsert(SubscriberOriginal subscriberOriginal,
            List<FrameworkUADEntity.SubscriberOriginal> subscriberOriginals, ClientConnections clientConnections)
        {
            mocks.SubscriberOriginal.Object.SaveBulkSqlInsert(subscriberOriginals, clientConnections);

            return true;
        }

        protected virtual List<ProductSubscriptionsExtensionMapper> Select(
            FrameworkUADBusinessLogic.ProductSubscriptionsExtensionMapper productSubscriptionsExtensionMapper,
            ClientConnections clientConnections)
        {
            mocks.ProductSubscriptionsExtensionMapper.Object.SelectAll(clientConnections);

            return new List<ProductSubscriptionsExtensionMapper>
            {
                typeof(ProductSubscriptionsExtensionMapper).CreateInstance()
            };
        }

        protected virtual List<CategoryCode> SelectActive(FrameworkUAD_Lookup.BusinessLogic.CategoryCode catCode,
            bool isFree)
        {
            mocks.CategoryCode.Object.SelectActiveIsFree(isFree);
            CategoryCode instance = typeof(CategoryCode).CreateInstance();
            instance.IsActive = isFree;
            instance.CategoryCodeValue = CategoryCodeValue;

            return new List<CategoryCode> { instance };
        }

        protected virtual List<TransactionCode> SelectActiveIsFree(
            FrameworkUAD_Lookup.BusinessLogic.TransactionCode trCode, bool isFree)
        {
            mocks.TransactionCode.Object.SelectActiveIsFree(isFree);
            TransactionCode instance = typeof(TransactionCode).CreateInstance();
            instance.IsActive = isFree;
            instance.TransactionCodeValue = TransactionCodeValue;

            return new List<TransactionCode> { instance };
        }

        protected virtual void ConsoleMessage(ServiceBase baseService, string message, string prCode = "",
            bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
        {
            ProcessCode = prCode;
            mocks.ServiceBase.Object.ConsoleMessage(message, ProcessCode, createLog, sourceFileId, updatedByUserId);
        }

        protected virtual void LogError(ServiceBase baseService, Exception exception, Client kClient, string msg,
            bool removeThread = true, bool removeQue = true)
        {
            mocks.ServiceBase.Object.LogError(exception, kClient, msg, removeThread, removeQue);
        }

        protected virtual List<Product> Select(FrameworkUADBusinessLogic.Product pr,
            ClientConnections clientConnections, bool includeCustomProperties)
        {
            mocks.Product.Object.Select(clientConnections, includeCustomProperties);
            Product pro = typeof(Product).CreateInstance();
            return new List<Product> { pro };
        }

        protected virtual List<EmailStatus> Select(FrameworkUADBusinessLogic.EmailStatus status,
            ClientConnections clientConnections)
        {
            mocks.EmailStatus.Object.Select(clientConnections);
            return new List<EmailStatus> { typeof(EmailStatus).CreateInstance() };
        }

        protected virtual Client Select(KMPlatform.BusinessLogic.Client clientp, int cliendId, bool includeObjects)
        {
            mocks.KmClient.Object.Select(cliendId, includeObjects);
            return typeof(Client).CreateInstance();
        }

        protected virtual SourceFile Select(UasBusinessLogic.SourceFile file, int clientId,
            string includeCustomProperties, bool isDeleted)
        {
            mocks.SourceFile.Object.Select(clientId, includeCustomProperties, isDeleted);
            return typeof(SourceFile).CreateInstance();
        }

        protected virtual List<Code> Select(FrameworkUAD_Lookup.BusinessLogic.Code businessCode,
            Enums.CodeType codeType)
        {
            mocks.Code.Object.Select(codeType);

            return new List<Code>
            {
                new Code {CodeTypeId = (int) codeType, CodeName = "Recurring", CodeId = 1},
                new Code {CodeTypeId = (int) codeType, CodeName = "Append", CodeId = 2},
                new Code {CodeTypeId = (int) codeType, CodeName = "Replace", CodeId = 3},
                new Code {CodeTypeId = (int) codeType, CodeName = "Overwrite", CodeId = 6},
                new Code {CodeTypeId = (int) codeType, CodeName = "Or", CodeId = 4},
                new Code {CodeTypeId = (int) codeType, CodeName = "And", CodeId = 5},
                new Code {CodeTypeId = (int) codeType, CodeName = "BreakFalse", CodeId = 7},
                new Code {CodeTypeId = (int) codeType, CodeName = "BreakTrue", CodeId = 8},
                new Code {CodeTypeId = (int) codeType, CodeName = "BreakAlways", CodeId = 9},
            };
        }

        protected virtual Service Select(KMPlatform.BusinessLogic.Service kmService, KMPlatform.Enums.Services services,
            bool includeObjects)
        {
            mocks.Service.Object.Select(services, includeObjects);

            Service instance = typeof(Service).CreateInstance();
            instance.ServiceFeatures.Add(new ServiceFeature { SFName = "UAD Api" });
            instance.ServiceFeatures.Add(new ServiceFeature { SFName = "File Import" });

            return instance;
        }

        protected virtual int Save(AdmsLog admLog, UasEntity.AdmsLog log)
        {
            mocks.AdmsLog.Object.Save(log);
            return 1;
        }

        protected virtual bool Save(DQMQue dqmQue, UasEntity.DQMQue que)
        {
            mocks.DQmQue.Object.Save(que);
            return true;
        }

        protected virtual void ExecuteAddressCleanse(AddressClean addrClean, UasEntity.AdmsLog admLog,
            Client kClient)
        {
            mocks.AddressClean.Object.ExecuteAddressCleanse(admLog, kClient);
        }

        protected virtual bool Update(AdmsLog admLog, string prCode, Enums.FileStatusType fileStatus,
            Enums.ADMS_StepType step,
            Enums.ProcessingStatusType status, Enums.ExecutionPointType ep, int userId, string currentStatus,
            bool createLog, int sourceFileId,
            bool setFileEnd)
        {
            mocks.AdmsLog.Object.Update(prCode, fileStatus, step, status, ep, userId, currentStatus, createLog,
                sourceFileId, setFileEnd);

            return true;
        }

        protected virtual void ShimForSqlConnection()
        {
            ShimSqlConnection.AllInstances.Open = connection => { };
        }

        protected virtual void ShimForSqlCommand(int affectedRows)
        {
            ShimSqlCommand.AllInstances.ExecuteNonQuery = command =>
            {
                sqlCommandText = command.CommandText;
                isSqlCommandExecuted = true;
                return affectedRows;
            };
        }
    }
}