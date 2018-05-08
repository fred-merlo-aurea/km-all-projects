using System;
using System.Collections.Generic;
using System.Linq;
using ADMS.Services;
using ADMS.Services.Validator.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD.ServiceResponse;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using KMPlatform.Object;
using Moq;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Product = FrameworkUAD.Entity.Product;
using ProductSubscription = FrameworkUAD.Object.ProductSubscription;
using SubscriberOriginal = FrameworkUAD.BusinessLogic.SubscriberOriginal;
using SubscriberTransformed = FrameworkUAD.BusinessLogic.SubscriberTransformed;
using Subscription = FrameworkUAD.Entity.Subscription;

using SubscriberTransformedEntity = FrameworkUAD.Entity.SubscriberTransformed;
using SubscriberOriginalEntity = FrameworkUAD.Entity.SubscriberOriginal;
using ProductSubscriptionsExtensionMapperBusiness = FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper;
using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Test Data for SaveSubscriber Method
    /// </summary>
    public partial class SaveSubscriberTransformedTest : Fakes
    {
        private List<ProductSubscription> subscriptions;
        private List<List<SubscriberOriginalEntity>> originals;
        private List<List<SubscriberTransformedEntity>> transformeds;

        protected override bool SaveBulkSqlInsert(SubscriberTransformed subscriberTransformed, List<SubscriberTransformedEntity> subscriberTransformeds, ClientConnections clientConnections, bool isDataCompare)
        {
            base.SaveBulkSqlInsert(subscriberTransformed, subscriberTransformeds, clientConnections,
                isDataCompare);

            transformeds.Add(subscriberTransformeds);
            return true;
        }

        protected override bool SaveBulkSqlInsert(SubscriberOriginal subscriberOriginal, List<SubscriberOriginalEntity> subscriberOriginals, ClientConnections clientConnections)
        {
            base.SaveBulkSqlInsert(subscriberOriginal, subscriberOriginals, clientConnections);
            originals.Add(subscriberOriginals);

            return true;
        }

        protected override void ConsoleMessage(ServiceBase baseService, string message, string prCode = "",
            bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
        {
            base.ConsoleMessage(baseService, message, prCode, createLog, sourceFileId, updatedByUserId);

            ProcessCode = prCode;
        }

        private void Init()
        {
            testEntity.SourceFile = new SourceFile { SourceFileID = Constants.SourceFileId, ClientID = Constants.ClientId };
            testEntity.Client = typeof(Client).CreateInstance();
            testEntity.Client.ClientID = Constants.ClientId;
            testEntity.Subscriber = new SaveSubscriber { Products = null };

            ProductSubscription instanceWithNullValues = typeof(ProductSubscription).CreateInstance(true);
            instanceWithNullValues.SubscriberProductDemographics = null;
            testEntity.Subscriber.ConsensusDemographics = new List<SubscriberConsensusDemographic>
            {
                typeof(SubscriberConsensusDemographic).CreateInstance()
            };

            originals = new List<List<SubscriberOriginalEntity>>();
            transformeds = new List<List<SubscriberTransformedEntity>>();

            var newSubscription = new ProductSubscription
            {
                PubCategoryID = Constants.CategoryCodeValue,
                PubTransactionID = Constants.TransactionCodeValue,
                SubscriberProductDemographics = new List<SubscriberProductDemographic>
                {
                    typeof(SubscriberProductDemographic).CreateInstance()
                }
            };

            subscriptions = new List<ProductSubscription>
            {
                newSubscription,
                instanceWithNullValues,
                new ProductSubscription(typeof(FrameworkUADEntity.ProductSubscription).CreateInstance()),
                new ProductSubscription(typeof(Subscription).CreateInstance()),
                new ProductSubscription(typeof(IssueCompDetail).CreateInstance())
            };
        }

        private void Initialize()
        {
            ShimValidator.AllInstances
                .ValidateProductSubscriptionSavedSubscriberSaveSubscriberRefClientListOfEmailStatusInt32 = ValidateProductSubscription;
            ShimValidator.AllInstances.ValidateConsensusDemographicsSaveSubscriberRefClientString = ValidateConsensusDemographics;
            ShimValidator.AllInstances.SetProductIDSavedSubscriberSaveSubscriberRefListOfProductInt32 = SetProductId;

            Init();
        }

        /// <summary>
        ///     Skip Method inside logic used by ShimValidator
        /// </summary>
        /// <param name="this"></param>
        /// <param name="subscription"></param>
        /// <param name="client"></param>
        /// <param name="processCode"></param>

        private void ValidateConsensusDemographics(global::ADMS.Services.Validator.Validator @this, ref SaveSubscriber subscription, Client client, string processCode)
        {
        }

        private SavedSubscriber ValidateProductSubscription(global::ADMS.Services.Validator.Validator @this, SavedSubscriber response, ref SaveSubscriber subscription, Client client, List<EmailStatus> statuses, int threadId)
        {
            return response;
        }

        private SavedSubscriber SetProductId(global::ADMS.Services.Validator.Validator @this, SavedSubscriber response,
            ref SaveSubscriber subscription, List<Product> products, int threadId)
        {
            response.IsPubCodeValid = true;
            testEntity.Mocks.ResetCalls();
            return response;
        }

        private void VerifyMethodInteractions()
        {
            testEntity.Mocks.Code.Verify(x => x.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update));
            testEntity.Mocks.CategoryCode.Verify(x => x.SelectActiveIsFree(true));
            testEntity.Mocks.TransactionCode.Verify(x => x.SelectActiveIsFree(true));

            if (testEntity.Subscriber.Products != null && testEntity.Subscriber.Products.Any())
            {
                testEntity.Mocks.ProductSubscriptionsExtensionMapper.Verify(x => x.SelectAll(It.IsAny<ClientConnections>()), Times.AtLeastOnce);

                for (var index = 0; index < testEntity.Subscriber.Products.Count; index++)
                {
                    var ps = testEntity.Subscriber.Products[index];

                    List<FrameworkUADEntity.SubscriberOriginal> addSo = originals[index];
                    testEntity.Mocks.SubscriberOriginal.Verify(x => x.SaveBulkSqlInsert(addSo, It.IsAny<ClientConnections>()));

                    List<FrameworkUADEntity.SubscriberTransformed> addSt = transformeds[index];
                    testEntity.Mocks.SubscriberTransformed.Verify(x => x.SaveBulkSqlInsert(addSt, It.IsAny<ClientConnections>(), false));

                    VerifyNewSubscriberOriginalAsserts(ps, addSo.First());
                }
            }

            testEntity.Mocks.AdmsLog.ResetCalls();
            testEntity.Mocks.AddressClean.ResetCalls();
            testEntity.Mocks.DQmQue.ResetCalls();

            testEntity.Mocks.VerifyNoOtherCalls();
        }

        private void VerifyNewSubscriberOriginalAsserts(ProductSubscription ps, FrameworkUADEntity.SubscriberOriginal newSo)
        {
            Assert.AreEqual(newSo.ProcessCode, ProcessCode);
            Assert.AreEqual(newSo.Address, !string.IsNullOrEmpty(ps.Address1) ? ps.Address1 : testEntity.Subscriber.Address);
            Assert.AreEqual(newSo.Address3, !string.IsNullOrEmpty(ps.Address3) ? ps.Address3 : testEntity.Subscriber.Address3);
            Assert.AreEqual(newSo.City, !string.IsNullOrEmpty(ps.City) ? ps.City : testEntity.Subscriber.City);
            Assert.AreEqual(newSo.Company, !string.IsNullOrEmpty(ps.Company) ? ps.Company : testEntity.Subscriber.Company);
            Assert.AreEqual(newSo.Country, !string.IsNullOrEmpty(ps.Country) ? ps.Country : testEntity.Subscriber.Country);
            Assert.AreEqual(newSo.County, !string.IsNullOrEmpty(ps.County) ? ps.County : testEntity.Subscriber.County);
            Assert.AreEqual(newSo.CreatedByUserID, 1);
            Assert.AreEqual(newSo.DateCreated, DateTime.Now);
            Assert.AreEqual(newSo.MailPermission, ps.MailPermission ?? testEntity.Subscriber.Demo31);
            Assert.AreEqual(newSo.FaxPermission, ps.FaxPermission ?? testEntity.Subscriber.Demo32);
            Assert.AreEqual(newSo.PhonePermission, ps.PhonePermission ?? testEntity.Subscriber.Demo33);
            Assert.AreEqual(newSo.OtherProductsPermission, ps.OtherProductsPermission ?? testEntity.Subscriber.Demo34);
            Assert.AreEqual(newSo.ThirdPartyPermission, ps.ThirdPartyPermission ?? testEntity.Subscriber.Demo35);
            Assert.AreEqual(newSo.EmailRenewPermission, ps.EmailRenewPermission ?? testEntity.Subscriber.Demo36);
            Assert.AreEqual(newSo.Demo7, ps.Demo7.Length > 0 ? ps.Demo7 : string.Empty);
            Assert.AreEqual(newSo.Email, ps.Email.Length > 0 ? ps.Email : string.Empty);
            Assert.AreEqual(newSo.FName, !string.IsNullOrEmpty(ps.FirstName) ? ps.FirstName : testEntity.Subscriber.FName);
            Assert.AreEqual(newSo.ForZip, testEntity.Subscriber.ForZip);
            Assert.AreEqual(newSo.Gender, !string.IsNullOrEmpty(ps.Gender) ? ps.Gender : testEntity.Subscriber.Gender);
            Assert.AreEqual(newSo.Home_Work_Address, testEntity.Subscriber.Home_Work_Address);
            Assert.AreEqual(newSo.LName, !string.IsNullOrEmpty(ps.LastName) ? ps.LastName : testEntity.Subscriber.LName);
            Assert.AreEqual(newSo.MailStop, !string.IsNullOrEmpty(ps.Address2) ? ps.Address2 : testEntity.Subscriber.MailStop);
            Assert.AreEqual(newSo.Plus4, !string.IsNullOrEmpty(ps.Plus4) ? ps.Plus4 : testEntity.Subscriber.Plus4);
            Assert.AreEqual(newSo.PubCode, ps.PubCode);
            Assert.AreEqual(newSo.QDate, ps.QualificationDate.ToString().Length > 0 ? ps.QualificationDate : DateTime.Now);
            Assert.AreEqual(newSo.QSourceID, ps.PubQSourceID > 0 ? ps.PubQSourceID : 0);
            Assert.AreEqual(newSo.Sequence, ps.SequenceID > 0 ? ps.SequenceID : testEntity.Subscriber.Sequence);
            Assert.AreEqual(newSo.State, !string.IsNullOrEmpty(ps.RegionCode) ? ps.RegionCode : testEntity.Subscriber.State);
            Assert.AreEqual(newSo.Title, !string.IsNullOrEmpty(ps.Title) ? ps.Title : testEntity.Subscriber.Title);
            Assert.AreEqual(newSo.Zip, !string.IsNullOrEmpty(ps.ZipCode) ? ps.ZipCode : testEntity.Subscriber.Zip);
            Assert.AreEqual(newSo.IsActive, true);
        }
    }
}