using System;
using System.Collections.Generic;
using System.Linq;
using ADMS.Services;
using ADMS.Services.DataCleanser;
using ADMS.Services.Validator.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD.ServiceResponse;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Enums = FrameworkUAD_Lookup.Enums;
using UasFramework = FrameworkUAS.BusinessLogic;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Test Data for SaveSubscribers Method
    /// </summary>
    public partial class SaveSubscribersTest : Fakes
    {
        private AdmsLog expectedAdmsLog;
        private DQMQue expectedDqmQue;
        private SavedSubscriber expectedSavedSubscriber;

        private List<AdmsLog> actualAdmsLog;
        private DQMQue actualDqMque;
        private Client actualClient;

        protected override void ConsoleMessage(ServiceBase baseService, string message, string prCode = "",
            bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
        {
            base.ConsoleMessage(baseService, message, prCode, createLog, sourceFileId, updatedByUserId);

            ProcessCode = prCode;
            expectedDqmQue.ProcessCode = prCode;
            expectedAdmsLog.ProcessCode = prCode;
            expectedSavedSubscriber.ProcessCode = prCode;
        }

        protected override Client Select(KMPlatform.BusinessLogic.Client clientp, int cliendId, bool includeObjects)
        {
            base.Select(clientp, cliendId, includeObjects);

            actualClient = typeof(Client).CreateInstance();
            actualClient.ClientID = cliendId;

            return testEntity.Client;
        }

        protected override int Save(UasFramework.AdmsLog admLog, AdmsLog log)
        {
            base.Save(admLog, log);

            log.DQM = expectedDqmQue;
            actualAdmsLog.Add(log);

            return 1;
        }

        protected override SourceFile Select(UasFramework.SourceFile file, int clientId,
            string includeCustomProperties, bool isDeleted)
        {
            base.Select(file, clientId, includeCustomProperties, isDeleted);
            return testEntity.SourceFile;
        }

        protected override void ExecuteAddressCleanse(AddressClean addrClean, AdmsLog admLog, Client kClient)
        {
            base.ExecuteAddressCleanse(addrClean, admLog, kClient);

            admLog.DQM = expectedDqmQue;
            actualAdmsLog.Add(admLog);
        }

        protected override bool Save(UasFramework.DQMQue dqmQue, DQMQue que)
        {
            base.Save(dqmQue, que);

            actualDqMque = que;
            return true;
        }

        protected override bool Update(UasFramework.AdmsLog admLog, string prCode,
            Enums.FileStatusType fileStatus,
            Enums.ADMS_StepType step,
            Enums.ProcessingStatusType status, Enums.ExecutionPointType ep, int userId, string currentStatus,
            bool createLog, int sourceFileId,
            bool setFileEnd)
        {
            base.Update(admLog, prCode, fileStatus, step, status, ep, userId, currentStatus, createLog, sourceFileId,
                setFileEnd);

            testEntity.SourceFile.SourceFileID = sourceFileId;
            return true;
        }

        private void Init()
        {
            actualAdmsLog = new List<AdmsLog>();
            testEntity.SourceFile =
                new SourceFile {SourceFileID = Constants.SourceFileId, ClientID = Constants.ClientId};
            expectedDqmQue = new DQMQue(ProcessCode, Constants.ClientId, true, false, Constants.SourceFileId);
            expectedSavedSubscriber = new SavedSubscriber();

            testEntity.Client = typeof(Client).CreateInstance();
            testEntity.Client.ClientID = Constants.ClientId;
            testEntity.Subscriber = new SaveSubscriber();

            expectedAdmsLog = new AdmsLog
            {
                AdmsLogId = 1,
                ClientId = Constants.ClientId,
                StatusMessage = Enums.FileStatusType.Detected.ToString(),
                DateCreated = DateTime.Now,
                FileLogDetails = new List<FileLog>(),
                FileNameExact = Constants.UadWsAddSubscriberFile,
                FileStart = DateTime.Now,
                RecordSource = Constants.API,
                SourceFileId = Constants.SourceFileId,
                ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId,
                DQM = expectedDqmQue
            };
        }

        private void Initialize()
        {
            ShimValidator.AllInstances
                    .ValidateProductSubscriptionSavedSubscriberSaveSubscriberRefClientListOfEmailStatusInt32 =
                ValidateProductSubscription;
            ShimValidator.AllInstances.SetProductIDSavedSubscriberSaveSubscriberRefListOfProductInt32 = SetProductId;
            ShimValidator.AllInstances.SaveSubscriberTransformedSavedSubscriberClientSaveSubscriberSourceFileString =
                SaveSubscriberTransformed;

            Init();
        }

        private SavedSubscriber SaveSubscriberTransformed(global::ADMS.Services.Validator.Validator valid,
            SavedSubscriber savedSubscriber, Client kClient, SaveSubscriber saveSubscriber, SourceFile file,
            string message)
        {
            testEntity.SaveSubscriberTransformed = true;

            return savedSubscriber;
        }

        private SavedSubscriber ValidateProductSubscription(global::ADMS.Services.Validator.Validator @this,
            SavedSubscriber response, ref SaveSubscriber subscription, Client kClient, List<EmailStatus> statuses,
            int threadId)
        {
            testEntity.ValidateProductSubscription = true;

            return response;
        }

        private SavedSubscriber SetProductId(global::ADMS.Services.Validator.Validator @this, SavedSubscriber response,
            ref SaveSubscriber subscription, List<Product> products, int threadId)
        {
            testEntity.SetProductId = true;
            response.IsPubCodeValid = testEntity.IsPubCodeValid;
            response.IsProductSubscriberCreated = testEntity.IsProductSubscriberCreated;

            return response;
        }

        private void VerifyIfPubCodeValid()
        {
            VerifyHeader();
            VerifyWithNoCirculation();

            Assert.IsTrue(testEntity.ValidateProductSubscription);
            Assert.IsTrue(testEntity.SaveSubscriberTransformed);

            VerifyFooter();
        }

        private void VerifyIfSourceFileNotNullWithCirculation()
        {
            VerifyHeader();

            testEntity.Mocks.SourceFile.Verify(x =>
                x.Select(Constants.ClientId, Constants.CircWsAddSubscriberFile, false));

            VerifyFooter();
        }

        private void VerifyIfSourceFileNotNullWithoutCirculation()
        {
            VerifyHeader();
            VerifyWithNoCirculation();
            VerifyFooter();
        }

        private void VerifyFooter()
        {
            testEntity.Mocks.AdmsLog.Verify(x => x.Save(actualAdmsLog.First()));

            Assert.IsTrue(expectedAdmsLog.IsContentMatched(actualAdmsLog.First(), nameof(AdmsLog.FileLogDetails)));
            actualAdmsLog.RemoveAt(0);

            testEntity.Mocks.Product.Verify(x => x.Select(testEntity.Client.ClientConnections, false));
            testEntity.Mocks.EmailStatus.Verify(x => x.Select(testEntity.Client.ClientConnections));

            testEntity.Mocks.DQmQue.Verify(x => x.Save(actualDqMque));
            Assert.IsTrue(expectedDqmQue.IsContentMatched(actualDqMque));

            testEntity.Mocks.AdmsLog.Verify(x => x.Update(ProcessCode,
                Enums.FileStatusType.ApiProcessing,
                Enums.ADMS_StepType.Validator_Api_End,
                Enums.ProcessingStatusType.ApiValidated,
                Enums.ExecutionPointType.Post_ApiSaveSubscriber, 1,
                $"End: api data validation {DateTime.Now.TimeOfDay}", true,
                Constants.SourceFileId, false));

            testEntity.Mocks.AddressClean
                .Verify(x => x.ExecuteAddressCleanse(actualAdmsLog.First(), testEntity.Client));

            Assert.IsTrue(actualAdmsLog.First().IsContentMatched(new AdmsLog
            {
                ProcessCode = ProcessCode,
                SourceFileId = Constants.SourceFileId,
                ClientId = Constants.ClientId,
                DQM = expectedDqmQue
            }, nameof(AdmsLog.FileLogDetails)));

            testEntity.Mocks.VerifyNoOtherCalls();
        }

        private void VerifyWithNoCirculation()
        {
            testEntity.Mocks.SourceFile.Verify(x =>
                x.Select(Constants.ClientId, Constants.UadWsAddSubscriberFile, false));
        }

        private void VerifyHeader()
        {
            var msg =
                $"{DateTime.Now.TimeOfDay} Entered SaveSubscribers service method client: {testEntity.Client.FtpFolder}";

            testEntity.Mocks.ServiceBase.Verify(x => x.ConsoleMessage(msg, ProcessCode, true, 0, 0));

            testEntity.Mocks.KmClient.Verify(x => x.Select(Constants.ClientId, true));
        }
    }
}