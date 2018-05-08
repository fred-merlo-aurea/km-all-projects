using FrameworkSubGen.BusinessLogic;
using FrameworkSubGen.BusinessLogic.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using KMPlatform.BusinessLogic;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Object;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using BundleType = FrameworkSubGen.Entity.Enums.SubscriptionType;
using Enums = FrameworkUAD_Lookup.Enums;
using FrameworkSubGenEntity = FrameworkSubGen.Entity;
using KM_BusinessLogic = KMPlatform.BusinessLogic;
using PaymentType = FrameworkSubGen.Entity.Enums.PaymentType;
using ProductSubscription = FrameworkUAD.Entity.ProductSubscription;
using ShimSubscription = FrameworkSubGen.BusinessLogic.Fakes.ShimSubscription;
using UADLookup_BusinessLogic = FrameworkUAD_Lookup.BusinessLogic;
using UAD_BusinessLogic = FrameworkUAD.BusinessLogic;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    /// <summary>
    ///     Unit Tests for <see cref="Payment"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class PaymentTest : Fakes
    {
        private const int PubSubscriptionId = 10;

        private Mocks _mocks;
        private Payment _payment;
        private ClientConnections _client;
        private List<RecordIdentifier> _recordIdentifiers;
        private string _sourceMethod;
        private KM_BusinessLogic.Enums.Applications _applications;
        private FrameworkSubGenEntity.Payment _pay;
        private FrameworkSubGenEntity.Bundle _bundle;

        private List<SubscriptionPaid> _actualSubscriptionPaids;

        [SetUp]
        public void SetUp()
        {
            _mocks = new Mocks();
            SetupFakes(_mocks);

            _payment = new Payment();
            ProcessCode = "process-code";
            _client = new ClientConnections();

            ShimSubscriptionPaid.AllInstances.SaveListOfSubscriptionPaidClientConnections = Save;
            ShimSubscriberFinal.AllInstances.SelectRecordIdentifiersStringClientConnections = SelectRecordIdentifiers;
            ShimPayment.AllInstances.SelectGuid = (payment, guid) => _pay;
            ShimBundle.AllInstances.SelectInt32 = (bundle, i) => _bundle;
            ShimProductSubscription.AllInstances.SelectGuidClientConnections = Select;
            ShimSubscription.AllInstances.SelectInt32 =
                (sub, i) => typeof(FrameworkSubGenEntity.Subscription).CreateInstance();
        }

        [Test]
        public void Save_IfNullRecordIndentifiersList_ReturnFalse()
        {
            // Arrange
            _recordIdentifiers = null;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                LogCriticalError;

            // Act
            bool result = _payment.Save(ProcessCode, _client);

            // Assert
            result.ShouldBeFalse();
            _sourceMethod.ShouldBe("SubscriptionPaid.Save");
            _applications.ShouldBe(KM_BusinessLogic.Enums.Applications.ADMS_Engine);
        }

        [Test]
        public void Save_IfEmptyRecordIndentifiersList_SaveAndReturnTrue()
        {
            // Arrange
            _recordIdentifiers = new List<RecordIdentifier>();

            // Act
            bool result = _payment.Save(ProcessCode, _client);

            // Assert
            result.ShouldBeTrue();
            _actualSubscriptionPaids.ShouldBeEmpty();
        }

        [Test]
        [TestCase(PaymentType.Cash, BundleType.Both)]
        [TestCase(PaymentType.Check, BundleType.Digital)]
        [TestCase(PaymentType.Credit, BundleType.Print)]
        [TestCase(PaymentType.Imported, BundleType.Both)]
        [TestCase(PaymentType.Other, BundleType.Digital)]
        [TestCase(PaymentType.PayPal, BundleType.Print)]
        public void Save_ContainRecordIndentifiers_SaveAndReturnTrue(PaymentType paymentType, BundleType bundleType)
        {
            // Arrange
            Guid stRecordIdentifier = new Guid();
            Guid grpNo = new Guid();
            Guid sfRecordIdentifier = new Guid();
            Guid soRecordIdentifier = new Guid();

            _recordIdentifiers = new List<RecordIdentifier>
            {
                new RecordIdentifier
                {
                    STRecordIdentifier = stRecordIdentifier,
                    IGrp_No = grpNo,
                    SFRecordIdentifier = sfRecordIdentifier,
                    SORecordIdentifier = soRecordIdentifier
                }
            };

            _pay = typeof(FrameworkSubGenEntity.Payment).CreateInstance();
            _pay.type = paymentType;

            _bundle = typeof(FrameworkSubGenEntity.Bundle).CreateInstance();
            _bundle.type = bundleType;

            // Act
            bool result = _payment.Save(ProcessCode, _client);

            // Assert
            result.ShouldBeTrue();
            _actualSubscriptionPaids.ShouldBeEmpty();
        }

        protected override List<Code> Select(UADLookup_BusinessLogic.Code businessCode, Enums.CodeType codeType)
        {
            base.Select(businessCode, codeType);

            return new List<Code>
            {
                new Code {CodeTypeId = (int) codeType, CodeName = "Cash", CodeId = 1},
                new Code {CodeTypeId = (int) codeType, CodeName = "Check", CodeId = 2},
                new Code {CodeTypeId = (int) codeType, CodeName = "Credit Card", CodeId = 3},
                new Code {CodeTypeId = (int) codeType, CodeName = "Imported", CodeId = 6},
                new Code {CodeTypeId = (int) codeType, CodeName = "Other", CodeId = 4},
                new Code {CodeTypeId = (int) codeType, CodeName = "PayPal", CodeId = 5},
                new Code {CodeTypeId = (int) codeType, CodeName = "Both", CodeId = 7},
                new Code {CodeTypeId = (int) codeType, CodeName = "Digital", CodeId = 8},
                new Code {CodeTypeId = (int) codeType, CodeName = "Print", CodeId = 9}
            };
        }

        private bool Save(
            UAD_BusinessLogic.SubscriptionPaid subscriptionPaid,
            List<SubscriptionPaid> subscriptionPaids,
            ClientConnections clientConnections)
        {
            _actualSubscriptionPaids = subscriptionPaids;
            return true;
        }

        private List<RecordIdentifier> SelectRecordIdentifiers(
            UAD_BusinessLogic.SubscriberFinal subscriberFinal,
            string processCode,
            ClientConnections clientConnections)
        {
            return _recordIdentifiers;
        }

        private ProductSubscription Select(
            UAD_BusinessLogic.ProductSubscription productSubscription,
            Guid sfRecordIndentifier,
            ClientConnections client)
        {
            ProductSubscription instance = typeof(ProductSubscription).CreateInstance();
            instance.SFRecordIdentifier = sfRecordIndentifier;
            instance.PubSubscriptionID = PubSubscriptionId;

            return instance;
        }

        private int LogCriticalError(
            ApplicationLog applicationLog,
            string ex,
            string sourceMethod,
            KM_BusinessLogic.Enums.Applications applications,
            string note,
            int clientId,
            string subject)
        {
            _sourceMethod = sourceMethod;
            _applications = applications;

            return 0;
        }
    }
}