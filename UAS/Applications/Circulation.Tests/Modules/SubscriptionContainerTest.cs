using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Circulation.Modules;
using Circulation.Modules.Fakes;
using Core_AMS.Utilities.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAS.Object.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using AppData = FrameworkUAS.Object.AppData;
using TransactionCodeEntity = FrameworkUAD_Lookup.Entity.TransactionCode;
using UserAuthorization = FrameworkUAS.Object.UserAuthorization;

namespace Circulation.Tests.Modules
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class SubscriptionContainerTest
    {
        private const int AddressOnlyChangeCode = 21;
        private const int RequalOnlyChangeCode = 22;
        private const int RenewalPaymentCode = 40;
        private const int PaymentStatus1 = 1;
        private const int PaymentStatus3 = 3;

        private const string PropertyChangedHandlerMethod = "PropertyChangedHandler";
        private const string SetInfoChangedMethod = "SetInfoChanged";
        private const string SetInfoChangedAndAddressChangedMethod = "SetInfoChangedAndAddressChanged";
        private const string ValidateAddressPropertiesMethod = "ValidateAddressProperties";
        private const string ValidateGeneralPropertiesMethod = "ValidateGeneralProperties";

        private SubscriptionContainer _subscriptionContainer;
        private PrivateObject _privateObject;
        private IEnumerable<TransactionCodeEntity> _transactionList;
        private TransactionCodeEntity _transactionCodeEntity;
        private int _transactionCodeId;
        private bool _renewalPayment;
        private bool _isPaid;
        private bool _addressOnlyChange;
        private bool _requalOnlyChange;
        private bool _infoChanged;
        private bool _addressChanged;

        private IDisposable _context;

        [SetUp]
        public void Initiliaze()
        {
            _context = ShimsContext.Create();
            ShimForRequiredDependencies();
            ShimForWorkerDependencies();

            _subscriptionContainer = new SubscriptionContainer(null, null, null, null);
            _privateObject = new PrivateObject(_subscriptionContainer, new PrivateType(typeof(SubscriptionContainer)));
            _transactionList = null;
            _transactionCodeEntity = null;

            _transactionCodeId = 0;
            _renewalPayment = false;
            _isPaid = false;
            _addressOnlyChange = false;
            _requalOnlyChange = false;
            _infoChanged = false;
            _addressChanged = false;
        }

        [TearDown]
        public void CleanUp() => _context?.Dispose();

        [Test]
        public void PropertyChangedHandler_WhenRenewalPaymentAndIsPaidAreTrue_ShouldSetTransactionCodeId()
        {
            // Arrange
            _renewalPayment = true;
            _isPaid = true;
            _transactionList = CreateTransactionCodeList();
            _transactionCodeEntity = _transactionList.FirstOrDefault(item =>
                item.TransactionCodeValue == RenewalPaymentCode && item.TransactionCodeTypeID == PaymentStatus3);

            ShimSubscriptionContainer.AllInstances.TransactionCodeIDSetInt32 =
                (_, transactionCodeId) => { _transactionCodeId = transactionCodeId; };

            // Act
            _privateObject.Invoke(
                PropertyChangedHandlerMethod, _renewalPayment, _isPaid, _addressOnlyChange, _requalOnlyChange);

            // Assert
            _transactionCodeEntity.ShouldNotBeNull();
            _transactionCodeEntity.ShouldSatisfyAllConditions(
                () => _transactionCodeId.ShouldBe(_transactionCodeEntity.TransactionCodeID));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PropertyChangedHandler_WhenAddressOnlyChangeIsTrueAndRequalOnlyChangeIsFalse_ShouldSetCompleteChangeToFalse(bool isPaid)
        {
            // Arrange
            _addressOnlyChange = true;
            _requalOnlyChange = false;
            _transactionList = CreateTransactionCodeList();
            var transactionTypeId = isPaid ? PaymentStatus3 : PaymentStatus1;
            _transactionCodeEntity = _transactionList.FirstOrDefault(item => 
                item.TransactionCodeValue == AddressOnlyChangeCode && item.TransactionCodeTypeID == transactionTypeId);

            ShimSubscriptionContainer.AllInstances.TransactionCodeIDSetInt32 =
                (_, transactionCodeId) => { _transactionCodeId = transactionCodeId; };

            // Act
            _privateObject.Invoke(
                PropertyChangedHandlerMethod, _renewalPayment, isPaid, _addressOnlyChange, _requalOnlyChange);

            // Assert
            _transactionCodeEntity.ShouldNotBeNull();
            _transactionCodeEntity.ShouldSatisfyAllConditions(
                () => _transactionCodeId.ShouldBe(_transactionCodeEntity.TransactionCodeID),
                () => _subscriptionContainer.CompleteChange.ShouldBeFalse());
        }

        [Test]
        public void PropertyChangedHandler_WhenAddressOnlyChangeIsTrueAndRequalOnlyChangeIsTrue_ShouldSetCompleteChangeToTrue()
        {
            // Arrange
            _addressOnlyChange = true;
            _requalOnlyChange = true;

            // Act
            _privateObject.Invoke(
                PropertyChangedHandlerMethod, _renewalPayment, _isPaid, _addressOnlyChange, _requalOnlyChange);

            // Assert
            _subscriptionContainer.CompleteChange.ShouldBeTrue();
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PropertyChangedHandler_WhenRequalOnlyChangeIsTrue_ShouldSetTransactionCodeId(bool isPaid)
        {
            // Arrange
            _addressOnlyChange = false;
            _requalOnlyChange = true;
            _transactionList = CreateTransactionCodeList();
            var transactionTypeId = isPaid ? PaymentStatus3 : PaymentStatus1;
            _transactionCodeEntity = _transactionList.FirstOrDefault(item => 
                item.TransactionCodeValue == RequalOnlyChangeCode && item.TransactionCodeTypeID == transactionTypeId);

            ShimSubscriptionContainer.AllInstances.TransactionCodeIDSetInt32 =
                (_, transactionCodeId) => { _transactionCodeId = transactionCodeId; };

            // Act
            _privateObject.Invoke(
                PropertyChangedHandlerMethod, _renewalPayment, isPaid, _addressOnlyChange, _requalOnlyChange);

            // Assert
            _transactionCodeEntity.ShouldNotBeNull();
            _transactionCodeEntity.ShouldSatisfyAllConditions(
                () => _transactionCodeId.ShouldBe(_transactionCodeEntity.TransactionCodeID),
                () => _subscriptionContainer.CompleteChange.ShouldBeFalse());
        }

        [Test]
        public void SetTransactionCodeId_WhenTransactionCodeIsNull_ShouldNotUpdateTransactionCodeId()
        {
            // Arrange
            _renewalPayment = false;
            _addressOnlyChange = false;
            _requalOnlyChange = false;
            var transactionCodeId = 0;

            ShimSubscriptionContainer.AllInstances.TransactionCodeIDSetInt32 =
                (_, transCodeId) => { _transactionCodeId = transCodeId; };

            // Act
            _privateObject.Invoke(
                PropertyChangedHandlerMethod, _renewalPayment, _isPaid, _addressOnlyChange, _requalOnlyChange);

            // Assert
            transactionCodeId.ShouldSatisfyAllConditions(
                () => transactionCodeId.ShouldBe(0),
                () => _subscriptionContainer.CompleteChange.ShouldBeFalse());
        }

        [Test]
        public void ValidateGeneralProperties_ShouldCallSetInfoChanged_ShouldSetPropertiesToTrue()
        {
            // Arrange
            object[] args = { _infoChanged};

            // Act
            _privateObject.Invoke(ValidateGeneralPropertiesMethod, args);

            // Assert
            args[0].ToString().ShouldBe(bool.TrueString);
        }

        [Test]
        [TestCase("", " ")]
        [TestCase("Sample 1", "")]
        [TestCase("Sample 1", "Sample 1")]
        public void SetInfoChanged_WhenValuesIsDifferent_ShouldSetInfoChangedToTrue(string propertyValue1, string propertyValue2)
        {
            // Arrange
            var addressChangedResult = propertyValue1 != propertyValue2;
            object[] args =
            {
                propertyValue1,
                propertyValue2,
                _infoChanged
            };

            // Act
            _privateObject.Invoke(SetInfoChangedMethod, args);

            // Assert
            args[2].ShouldBe(addressChangedResult);
        }

        [Test]
        public void ValidateAddressProperties_ShouldCallSetInfoChangedAndAddressChanged_ShouldSetPropertiesToTrue()
        {
            // Arrange
            object[] args = { _infoChanged, _addressChanged };

            // Act
            _privateObject.Invoke(ValidateAddressPropertiesMethod, args);

            // Assert
            _infoChanged.ShouldSatisfyAllConditions(
                () => args[0].ToString().ShouldBe(bool.TrueString),
                () => args[1].ToString().ShouldBe(bool.TrueString));
        }

        [Test]
        [TestCase("", " ")]
        [TestCase("Sample 1", "")]
        [TestCase("Sample 1", "Sample 1")]
        public void SetInfoChangedAndAddressChanged_WhenValuesIsDifferent_ShouldSetPropertiesToTrue(string propertyValue1, string propertyValue2)
        {
            // Arrange
            var addressChangedResult = propertyValue1 != propertyValue2;
            var infoChangedResult = propertyValue1 != propertyValue2;
            object[] args =
            {
                propertyValue1,
                propertyValue2,
                _infoChanged,
                _addressChanged
            };

            // Act
            _privateObject.Invoke(SetInfoChangedAndAddressChangedMethod, args);

            // Assert
            _infoChanged.ShouldSatisfyAllConditions(
                () => args[2].ShouldBe(addressChangedResult),
                () => args[3].ShouldBe(infoChangedResult));
        }

        private static IEnumerable<TransactionCodeEntity> CreateTransactionCodeList()
        {
            return new List<TransactionCodeEntity>
            {
                new TransactionCodeEntity
                {
                    TransactionCodeID = 10,
                    TransactionCodeValue = RenewalPaymentCode,
                    TransactionCodeTypeID = PaymentStatus3
                },
                new TransactionCodeEntity
                {
                    TransactionCodeID = 20,
                    TransactionCodeValue = AddressOnlyChangeCode,
                    TransactionCodeTypeID = PaymentStatus1
                },
                new TransactionCodeEntity
                {
                    TransactionCodeID = 30,
                    TransactionCodeValue = AddressOnlyChangeCode,
                    TransactionCodeTypeID = PaymentStatus3
                },
                new TransactionCodeEntity
                {
                    TransactionCodeID = 40,
                    TransactionCodeValue = RequalOnlyChangeCode,
                    TransactionCodeTypeID = PaymentStatus1
                },
                new TransactionCodeEntity
                {
                    TransactionCodeID = 50,
                    TransactionCodeValue = RequalOnlyChangeCode,
                    TransactionCodeTypeID = PaymentStatus3
                }
            };
        }

        private static void ShimForRequiredDependencies()
        {
            ShimSubscriptionContainer.AllInstances.Setup = _ => { };
            ShimSubscriptionContainer.AllInstances.InitializeComponent = _ => { };
            ShimWPF.MessageStringMessageBoxButtonMessageBoxImageString = (s, button, arg3, arg4) => { };
            ShimAppData.myAppDataGet = () => new AppData
            {
                AuthorizedUser = new UserAuthorization
                {
                    User = new User
                    {
                        CurrentClientGroup = new ClientGroup
                        {
                            Clients = new List<Client>()
                        }
                    }
                }
            };

            ShimHome.TransactionCodesGet = () => new List<TransactionCodeEntity>
            {
                new TransactionCodeEntity { TransactionCodeID = 10, TransactionCodeValue = 40, TransactionCodeTypeID = 3 },
                new TransactionCodeEntity { TransactionCodeID = 20, TransactionCodeValue = 21, TransactionCodeTypeID = 1 },
                new TransactionCodeEntity { TransactionCodeID = 30, TransactionCodeValue = 21, TransactionCodeTypeID = 3 },
                new TransactionCodeEntity { TransactionCodeID = 40, TransactionCodeValue = 22, TransactionCodeTypeID = 1 },
                new TransactionCodeEntity { TransactionCodeID = 50, TransactionCodeValue = 22, TransactionCodeTypeID = 3 }
            };
        }

        private static void ShimForWorkerDependencies()
        {
            ShimServiceClient.UAD_Lookup_TransactionCodeClient = () => null;
            ShimServiceClient.UAD_Lookup_TransactionCodeTypeClient = () => null;
            ShimServiceClient.UAD_Lookup_CategoryCodeClient = () => null;
            ShimServiceClient.UAD_ProductSubscriptionClient = () => null;
            ShimServiceClient.UAD_SubscriptionClient = () => null;
            ShimServiceClient.UAD_PubSubscriptionDetailClient = () => null;
            ShimServiceClient.UAD_Lookup_SubscriptionStatusClient = () => null;
            ShimServiceClient.UAD_Lookup_SubscriptionStatusMatrixClient = () => null;
            ShimServiceClient.UAD_SubscriptionPaidClient = () => null;
            ShimServiceClient.UAD_SubscriptionSearchResultClient = () => null;
            ShimServiceClient.UAD_ProductClient = () => null;
            ShimServiceClient.UAD_Lookup_ActionClient = () => null;
            ShimServiceClient.UAD_Lookup_RegionClient = () => null;
            ShimServiceClient.UAD_Lookup_CountryClient = () => null;
            ShimServiceClient.UAD_HistoryClient = () => null;
            ShimServiceClient.UAD_Lookup_CodeClient = () => null;
            ShimServiceClient.UAD_Lookup_CategoryCodeTypeClient = () => null;
            ShimServiceClient.UAD_MarketingMapClient = () => null;
            ShimServiceClient.UAS_UserLogClient = () => null;
            ShimServiceClient.UAD_Lookup_CodeTypeClient = () => null;
            ShimServiceClient.UAD_WaveMailingDetailClient = () => null;
            ShimServiceClient.UAS_ClientClient = () => null;
            ShimServiceClient.UAD_HistorySubscriptionClient = () => null;
            ShimServiceClient.UAD_PaidBillToClient = () => null;
            ShimServiceClient.UAD_HistoryResponseMapClient = () => null;
            ShimServiceClient.UAD_HistoryMarketingMapClient = () => null;
            ShimServiceClient.UAS_UserLogClient = () => null;
            ShimServiceClient.UAD_Lookup_ZipCodeClient = () => null;
        }
    }
}
