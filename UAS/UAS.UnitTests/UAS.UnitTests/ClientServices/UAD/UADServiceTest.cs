using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkSubGen.Entity;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Object;
using KM.Common;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.UAD_WS.Service.Common;
using WebServiceFramework.Fakes;
using CustomField = FrameworkUAD.Object.CustomField;
using EntityAddress = FrameworkSubGen.Entity.Address;
using EntityClient = KMPlatform.Entity.Client;
using EntityCustomField = FrameworkSubGen.Entity.CustomField;
using EntityPaidSubscription = FrameworkUAD.Object.PaidSubscription;
using EntitySubscriber = FrameworkSubGen.Entity.Subscriber;
using Enums = FrameworkUAD_Lookup.Enums;
using ShimAccountWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimAccount;
using ShimAddressApiWorker = FrameworkSubGen.BusinessLogic.API.Fakes.ShimAddress;
using ShimAddressWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimAddress;
using ShimBundleWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimBundle;
using ShimClient = KMPlatform.BusinessLogic.Fakes.ShimClient;
using ShimCustomFieldApiWorker = FrameworkSubGen.BusinessLogic.API.Fakes.ShimCustomField;
using ShimCustomFieldWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimCustomField;
using ShimDemoDetailWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimSubscriberDemographicDetail;
using ShimDemoWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimSubscriberDemographic;
using ShimFileLogWorker = FrameworkUAS.BusinessLogic.Fakes.ShimFileLog;
using ShimOrderWorker = FrameworkSubGen.BusinessLogic.API.Fakes.ShimOrder;
using ShimPaymentWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimPayment;
using ShimSubscriberWorker = FrameworkSubGen.BusinessLogic.Fakes.ShimSubscriber;
using UadService = ClientServices.UAD.UADService;

namespace UAS.UnitTests.ClientServices.UAD
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class UADServiceTest : Fakes
    {
        private const int AffectedCountPositive = 1;
        private const int SampleId = 100;
        private const int SampleBillingId = 200;
        private const int SampleMailId = 300;
        private const string SampleName = "name2";
        private const string SampleString = "sample";
        private const string SampleMailString = "sampleMail";
        private const string SampleBillingString = "sampleBilling";
        private const string ProductCode = "Code11";
        private const string AccessKeyValidated = "AccessKey Validated";

        private UadService _testEntity;

        [SetUp]
        public void Setup()
        {
            SetupFakes();
            ShimForAuthenticate();
            ShimClient.AllInstances.SelectInt32Boolean = (a, b, c) => new EntityClient
            {
                AccessKey = Guid.Empty,
                ClientCode = ClientCode
            };
            ShimFrameworkServiceBase.GetCallingIp = () => string.Empty;
            ShimFileLogWorker.AllInstances.SaveFileLog = (a, b) => true;
            _testEntity = new UadService();
        }

        [Test]
        public void SavePaidSubscriber_AccountWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityPaidSubscription();
            ShimForJsonFunction<EntityPaidSubscription>();
            ShimAccountWorker.AllInstances.SelectEnumsClients = (a, b) => throw new InvalidOperationException();

            // Act
            var result = _testEntity.SavePaidSubscriber(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FriendlyServiceError);
        }

        [Test]
        public void SavePaidSubscriber_SubscriberWorkerThrows_ReturnsErrorResponse()
        {
            // Arrange
            var entity = new EntityPaidSubscription();
            ShimWorkersForSavePaidSubscriber(false);
            var exception = new InvalidOperationException();
            ShimSubscriberWorker.AllInstances.FindSubscribersStringStringString = (a, b, c, d) => throw exception;

            // Act
            var result = _testEntity.SavePaidSubscriber(Guid.Empty, entity);

            // Assert
            VerifyErrorResponse(result, 0, StringFunctions.FormatException(exception));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SavePaidSubscriber_WithSubscriber_ReturnsSuccessResponse(bool addExistingSubscriber)
        {
            // Arrange
            var entity = CreatePaidSubscription();
            ShimWorkersForSavePaidSubscriber(addExistingSubscriber);

            // Act
            var result = _testEntity.SavePaidSubscriber(Guid.Empty, entity);

            // Assert
            VerifyResponse(
                result,
                AffectedCountPositive,
                AccessKeyValidated,
                Enums.ServiceResponseStatusTypes.Access_Validated);
        }

        [Test]
        public void SavePaidSubscriber_WithExistingAddresses_ReturnsSuccessResponse()
        {
            // Arrange
            var entity = CreatePaidSubscription();
            var billingAddress = CreateAddress(true);
            var mailingAddress = CreateAddress(false);
            var addresses = new List<EntityAddress> { billingAddress, mailingAddress };
            SetAddresses(entity, billingAddress, mailingAddress);
            ShimWorkersForSavePaidSubscriber(true);
            ShimAddressWorker.AllInstances.SelectInt32 = (a, b) => addresses;

            // Act
            var result = _testEntity.SavePaidSubscriber(Guid.Empty, entity);

            // Assert
            VerifyResponse(
                result,
                AffectedCountPositive,
                AccessKeyValidated,
                Enums.ServiceResponseStatusTypes.Access_Validated);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SavePaidSubscriber_WithBillingAndMailAddressSame_ReturnsSuccessResponse(bool addExistingSubscriber)
        {
            // Arrange
            var addressSavedTimes = 0;
            var entity = CreatePaidSubscription();
            entity.BillingAddress = entity.MailingAddress;
            ShimWorkersForSavePaidSubscriber(addExistingSubscriber);
            ShimAddressWorker.AllInstances.SaveAddress = (a, address) =>
            {
                addressSavedTimes++;
                return true;
            };

            // Act
            var result = _testEntity.SavePaidSubscriber(Guid.Empty, entity);

            // Assert
            addressSavedTimes.ShouldBe(1);
            VerifyResponse(
                result,
                AffectedCountPositive,
                AccessKeyValidated,
                Enums.ServiceResponseStatusTypes.Access_Validated);
        }

        [Test]
        [TestCase(Enums.PaymentType.Cash)]
        [TestCase(Enums.PaymentType.PayPal)]
        [TestCase(Enums.PaymentType.Check)]
        [TestCase(Enums.PaymentType.Imported)]
        [TestCase(Enums.PaymentType.Other)]
        public void SavePaidSubscriber_WithPaymentType_SavesPaymentTypeAndReturnsSuccessResponse(Enums.PaymentType paymentType)
        {
            // Arrange
            var savedType = string.Empty;
            var entity = CreatePaidSubscription();
            entity.Payment.PaymentType = paymentType;
            ShimWorkersForSavePaidSubscriber(false);
            ShimPaymentWorker.AllInstances.SaveBulkXmlListOfPayment = (a, types) =>
            {
                savedType = types.First().type.ToString();
                return true;
            };

            // Act
            var result = _testEntity.SavePaidSubscriber(Guid.Empty, entity);

            // Assert
            savedType.ShouldBe(paymentType.ToString());
            VerifyResponse(
                result,
                AffectedCountPositive,
                AccessKeyValidated,
                Enums.ServiceResponseStatusTypes.Access_Validated);
        }

        [Test]
        public void GetCustomFields_ByAccessKey_ReturnsSuccessResult()
        {
            // Arrange
            var list = new List<CustomField>();
            ShimObjects.AllInstances.GetCustomFieldsClientConnectionsString = (a, b, c) => list;

            // Act
            var result = _testEntity.GetCustomFields(Guid.Empty);

            //
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void GetConsensusCustomFields_ByAccessKey_ReturnsSuccessResult()
        {
            // Arrange
            var list = new List<CustomField>();
            ShimObjects.AllInstances.GetConsensusCustomFieldsClientConnections = (a, b) => list;

            // Act
            var result = _testEntity.GetConsensusCustomFields(Guid.Empty);

            //
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void GetCustomFieldValues_ByAccessKey_ReturnsSuccessResult()
        {
            // Arrange
            var list = new List<CustomFieldValue>();
            ShimObjects.AllInstances.GetCustomFieldValuesClientConnectionsStringString = (a, b, c, d) => list;

            // Act
            var result = _testEntity.GetCustomFieldValues(Guid.Empty);

            //
            VerifySuccessResponse(result, list);
        }

        [Test]
        public void GetConsensusCustomFieldValues_ByAccessKey_ReturnsSuccessResult()
        {
            // Arrange
            var list = new List<CustomFieldValue>();
            ShimObjects.AllInstances.GetConsensusCustomFieldValuesClientConnectionsString = (a, b, c) => list;

            // Act
            var result = _testEntity.GetConsensusCustomFieldValues(Guid.Empty);

            //
            VerifySuccessResponse(result, list);
        }

        private EntityPaidSubscription CreatePaidSubscription()
        {
            var entity = new EntityPaidSubscription
            {
                BillingAddress = SampleBillingString,
                MailingAddress = SampleMailString
            };
            var demo = new PaidSubscriptionProductDemographic
            {
                ProductCode = ProductCode,
                DemographicName = SampleName
            };
            demo.Values.Add(SampleString);
            entity.BundleItems.Add(new PaidBundleItem());
            entity.ProductDemographics.Add(demo);

            return entity;
        }

        private EntityAddress CreateAddress(bool isBilling)
        {
            var address = new EntityAddress
            {
                address_id = isBilling ? SampleBillingId : SampleMailId,
                address = isBilling ? SampleBillingString : SampleMailString,
                first_name = SampleName,
                last_name = SampleName,
                city = SampleString,
                state = SampleString,
                zip_code = SampleString
            };
            return address;
        }

        private void SetAddresses(EntityPaidSubscription entity, EntityAddress billingAddress, EntityAddress mailAddress)
        {
            entity.BillingAddress = billingAddress.address;
            entity.BillingFirstName = billingAddress.first_name;
            entity.BillingLastName = billingAddress.last_name;
            entity.BillingCity = billingAddress.city;
            entity.BillingState = billingAddress.state;
            entity.BillingZip = billingAddress.zip_code;

            entity.MailingAddress = mailAddress.address;
            entity.MailingFirstName = mailAddress.first_name;
            entity.MailingLastName = mailAddress.last_name;
            entity.MailingCity = mailAddress.city;
            entity.MailingState = mailAddress.state;
            entity.MailingZip = mailAddress.zip_code;
        }

        private EntityCustomField CreateCustomField()
        {
            var customField = new EntityCustomField
            {
                name = SampleName,
                field_id = SampleId
            };
            customField.value_options.Add(new ValueOption
            {
                value = SampleString
            });

            return customField;
        }

        private void ShimWorkersForSavePaidSubscriber(bool addSubscriber)
        {
            var subscribers = new List<EntitySubscriber>();
            if (addSubscriber)
            {
                subscribers.Add(new EntitySubscriber());
            }

            var customFields = new List<EntityCustomField>();
            customFields.Add(CreateCustomField());
            ShimSubscriberWorker.AllInstances.FindSubscribersStringStringString = (a, b, c, d) => subscribers;
            ShimAccountWorker.AllInstances.SelectEnumsClients = (a, b) => new Account();
            ShimAddressApiWorker.AllInstances.CreateEnumsClientAddress = (a, b, c) => SampleId;
            ShimAddressWorker.AllInstances.SaveAddress = (a, b) => true;
            ShimAddressWorker.AllInstances.SelectInt32 = (a, b) => new List<EntityAddress>();
            ShimBundleWorker.AllInstances.SelectStringInt32 = (a, b, c) => new Bundle();
            ShimOrderWorker.AllInstances.CreateOrderEnumsClientCreateOrder = (a, b, c) => AffectedCountPositive;
            ShimPaymentWorker.AllInstances.SaveBulkXmlListOfPayment = (a, b) => true;
            ShimCustomFieldApiWorker.AllInstances.SubscriberFieldUpdateEnumsClientSubscriberFieldUpdate =
                (a, b, c) => string.Empty;
            ShimCustomFieldApiWorker.AllInstances.GetCustomFieldsEnumsClient = (a, b) => customFields;
            ShimCustomFieldWorker.AllInstances.SelectInt32 = (a, b) => customFields;
            ShimDemoDetailWorker.AllInstances.SaveListOfSubscriberDemographicDetail = (a, b) => true;
            ShimDemoWorker.AllInstances.SaveListOfSubscriberDemographic = (a, b) => true;
            ShimForJsonFunction<EntityPaidSubscription>();
        }
    }
}
