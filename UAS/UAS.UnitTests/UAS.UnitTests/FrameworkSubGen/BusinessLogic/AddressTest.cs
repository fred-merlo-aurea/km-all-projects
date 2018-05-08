using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core_AMS.Utilities.Fakes;
using FrameworkSubGen.BusinessLogic;
using FrameworkSubGen.DataAccess.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Account = FrameworkSubGen.Entity.Account;
using Entity = FrameworkSubGen.Entity;
using EntityAddress = FrameworkSubGen.Entity.Address;
using Enums = FrameworkSubGen.Entity.Enums;
using FrameworkSubGenBusinessLogicAPI = FrameworkSubGen.BusinessLogic.API.Fakes;
using FrameworkSubGenBusinessLogicFakes = FrameworkSubGen.BusinessLogic.Fakes;
using ProductSubscription = FrameworkUAD.Entity.ProductSubscription;
using ShimAccount = FrameworkSubGen.BusinessLogic.Fakes.ShimAccount;

namespace UAS.UnitTests.FrameworkSubGen.BusinessLogic
{
    /// <summary>
    ///     Unit tests for <see cref="Address"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AddressTest
    {
        private const int KmClientId = 10;
        private const string PubCode = "pub-code";
        private const int SubscriberId = 20;
        private const int PublicationId = 15;
        private const string OldAddress1 = "old-address1";
        private const string OldAddress2 = "old-address2";
        private const string OldCity = "old-city";
        private const string OldStateAbbrevation = "old-state-abbreviation";
        private const string OldZipCode = "old-zip-code";

        private Address _address;
        private List<ProductSubscription> _productSubscriptions;
        private List<AcsFileDetail> _acsFileDetails;
        private List<NCOA> _ncoaList;
        private IDisposable _context;
        private Account _account;
        private Entity.Address _expectedAddress;
        private bool _updateForAcs;
        private List<Entity.Address> _actualAddressList;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _address = new Address();
            _account = typeof(Account).CreateInstance();
            _account.KMClientId = KmClientId;
            _account.company_name = "Master";
            _updateForAcs = false;

            _productSubscriptions = new List<ProductSubscription>();
            _acsFileDetails = new List<AcsFileDetail>();
            _ncoaList = new List<NCOA>();
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void UpdateForACS_EmptyProductSubscriptions_ReturnEmptyList()
        {
            // Arrange, Act
            List<ProductSubscription> result = _address.UpdateForACS(
                _productSubscriptions,
                _acsFileDetails,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _updateForAcs.ShouldBeTrue();
            _actualAddressList.ShouldBeEmpty();
        }

        [Test]
        public void UpdateForACS_HaveSubGenMailId_UpdateAddressAndReturnEmptyList()
        {
            // Arrange
            ProductSubscription ps = typeof(ProductSubscription).CreateInstance();
            ps.SubGenMailingAddressId = 20;
            _productSubscriptions.Add(ps);

            Entity.Address expectedAddress = new Entity.Address
            {
                address = ps.Address1,
                address_id = ps.SubGenMailingAddressId,
                address_line_2 = ps.Address2,
                city = ps.City,
                country = ps.Country,
                state = ps.RegionCode,
                zip_code = ps.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            List<ProductSubscription> result = _address.UpdateForACS(
                _productSubscriptions,
                _acsFileDetails,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }

        [Test]
        public void UpdateForACS_HaveNonSubGenMailIdWithSubscriberIDGreaterThenZero_UpdateSubscriberAddressAndReturnEmptyList()
        {
            // Arrange
            ProductSubscription ps = typeof(ProductSubscription).CreateInstance();
            ps.SubGenMailingAddressId = 0;
            ps.SubGenSubscriberID = SubscriberId;
            ps.PubCode = PubCode;

            _productSubscriptions.Add(ps);

            Entity.Address expectedAddress = new Entity.Address
            {
                address = ps.Address1,
                address_line_2 = ps.Address2,
                city = ps.City,
                country = ps.Country,
                state = ps.RegionCode,
                zip_code = ps.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            List<ProductSubscription> result = _address.UpdateForACS(
                _productSubscriptions,
                _acsFileDetails,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }

        [Test]
        public void UpdateForACS_HaveNonSubGenMailIdWithSubscriptionIDGreaterThenZero_UpdateSubscriptionAddressAndReturnEmptyList()
        {
            // Arrange
            ProductSubscription ps = typeof(ProductSubscription).CreateInstance();
            ps.SubGenMailingAddressId = 0;
            ps.SubGenSubscriberID = 0;
            ps.SubGenSubscriptionID = SubscriberId;
            ps.PubCode = PubCode;

            _productSubscriptions.Add(ps);

            Entity.Address expectedAddress = new Entity.Address
            {
                address = ps.Address1,
                address_line_2 = ps.Address2,
                city = ps.City,
                country = ps.Country,
                state = ps.RegionCode,
                zip_code = ps.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            List<ProductSubscription> result = _address.UpdateForACS(
                _productSubscriptions,
                _acsFileDetails,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }

        [Test]
        public void UpdateForACS_HaveNonSubGenMailIdWithoutSubscription_UpdateOldAddressAndReturnEmptyList()
        {
            // Arrange
            ProductSubscription ps = typeof(ProductSubscription).CreateInstance();
            ps.SubGenMailingAddressId = 0;
            ps.SubGenSubscriberID = 0;
            ps.SubGenSubscriptionID = 0;
            ps.PubCode = PubCode;

            _productSubscriptions.Add(ps);

            AcsFileDetail oldAddress = typeof(AcsFileDetail).CreateInstance();
            oldAddress.SequenceID = ps.SequenceID;
            oldAddress.OldAddress1 = OldAddress1;
            oldAddress.OldAddress2 = OldAddress2;
            oldAddress.OldCity = OldCity;
            oldAddress.OldStateAbbreviation = OldStateAbbrevation;
            oldAddress.OldZipCode = OldZipCode;

            _acsFileDetails.Add(oldAddress);

            Entity.Address expectedAddress = new Entity.Address
            {
                address = ps.Address1,
                address_line_2 = ps.Address2,
                city = ps.City,
                country = ps.Country,
                state = ps.RegionCode,
                zip_code = ps.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            List<ProductSubscription> result = _address.UpdateForACS(
                _productSubscriptions,
                _acsFileDetails,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }

        [Test]
        public void SaveBulkXml_ValidSubscribersList_SavesXml()
        {
            // Arrange
            var addressesList = CreateAddressesList(1);
            var actualNrOfLinesWrittenToConsole = 0;
            var actualNrOfXmlsSaved = 0;
            var actualSavedXml = String.Empty;
            var expectedNrOfLinesWrittenToConsole = 1;
            var expectedNrOfXmlsSaved = 1;
            var expectedSavedXml = $"<XML>xml_string{Environment.NewLine}</XML>";
            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, color) =>
            {
                actualNrOfLinesWrittenToConsole++;
            };
            ShimDataFunctions.CleanSerializedXMLString = xml =>
            {
                return "xml_string";
            };
            ShimAddress.SaveBulkXmlString = xml =>
            {
                actualNrOfXmlsSaved++;
                actualSavedXml = xml;
                return true;
            };

            // Act
            _address.SaveBulkXml(addressesList);

            // Assert
            actualNrOfLinesWrittenToConsole.ShouldBe(expectedNrOfLinesWrittenToConsole);
            actualSavedXml.ShouldBe(expectedSavedXml);
            actualNrOfXmlsSaved.ShouldBe(expectedNrOfXmlsSaved);
        }

        [Test]
        public void UpdateForNCOA_EmptyProductSubscriptions_ReturnEmptyList()
        {
            // Arrange, Act
            var result = _address.UpdateForNCOA(
                _productSubscriptions,
                _ncoaList,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _updateForAcs.ShouldBeTrue();
            _actualAddressList.ShouldBeEmpty();
        }

        [Test]
        public void UpdateForNCOA_HaveSubGenMailId_UpdateAddressAndReturnEmptyList()
        {
            // Arrange
            var productSubscription = typeof(ProductSubscription).CreateInstance();
            productSubscription.SubGenMailingAddressId = 20;
            _productSubscriptions.Add(productSubscription);

            var expectedAddress = new EntityAddress
            {
                address = productSubscription.Address1,
                address_id = productSubscription.SubGenMailingAddressId,
                address_line_2 = productSubscription.Address2,
                city = productSubscription.City,
                country = productSubscription.Country,
                state = productSubscription.RegionCode,
                zip_code = productSubscription.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            var result = _address.UpdateForNCOA(
                _productSubscriptions,
                _ncoaList,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }

        [Test]
        public void UpdateForNCOA_HaveNonSubGenMailIdWithSubscriberIDGreaterThenZero_UpdateSubscriberAddressAndReturnEmptyList()
        {
            // Arrange
            var productSubscription = typeof(ProductSubscription).CreateInstance();
            productSubscription.SubGenMailingAddressId = 0;
            productSubscription.SubGenSubscriberID = SubscriberId;
            productSubscription.PubCode = PubCode;

            _productSubscriptions.Add(productSubscription);

            var expectedAddress = new EntityAddress
            {
                address = productSubscription.Address1,
                address_line_2 = productSubscription.Address2,
                city = productSubscription.City,
                country = productSubscription.Country,
                state = productSubscription.RegionCode,
                zip_code = productSubscription.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            var result = _address.UpdateForNCOA(
                _productSubscriptions,
                _ncoaList,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }

        [Test]
        public void UpdateForNCOA_HaveNonSubGenMailIdWithSubscriptionIDGreaterThenZero_UpdateSubscriptionAddressAndReturnEmptyList()
        {
            // Arrange
            var productSubscription = typeof(ProductSubscription).CreateInstance();
            productSubscription.SubGenMailingAddressId = 0;
            productSubscription.SubGenSubscriberID = 0;
            productSubscription.SubGenSubscriptionID = SubscriberId;
            productSubscription.PubCode = PubCode;

            _productSubscriptions.Add(productSubscription);

            var expectedAddress = new EntityAddress
            {
                address = productSubscription.Address1,
                address_line_2 = productSubscription.Address2,
                city = productSubscription.City,
                country = productSubscription.Country,
                state = productSubscription.RegionCode,
                zip_code = productSubscription.ZipCode
            };

            _expectedAddress = expectedAddress;

            // Act
            var result = _address.UpdateForNCOA(
                _productSubscriptions,
                _ncoaList,
                KmClientId);

            // Assert
            result.ShouldBeEmpty();
            _actualAddressList.First().IsContentMatched(_expectedAddress).ShouldBeTrue();
            _updateForAcs.ShouldBeTrue();
        }
        
        [Test]
        public void SaveBulkXml_ValidSubscribersList_SavesXmlInBatches()
        {
            // Arrange
            var addressesList = CreateAddressesList(600);
            var actualNrOfLinesWrittenToConsole = 0;
            var actualNrOfXmlsSaved = 0;
            var expectedNrOfLinesWrittenToConsole = 600;
            var expectedNrOfXmlsSaved = 2;
            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, color) =>
            {
                actualNrOfLinesWrittenToConsole++;
            };
            ShimDataFunctions.CleanSerializedXMLString = xml =>
            {
                return "xml_string";
            };
            ShimAddress.SaveBulkXmlString = xml =>
            {
                actualNrOfXmlsSaved++;
                return true;
            };

            // Act
            _address.SaveBulkXml(addressesList);

            // Assert
            actualNrOfLinesWrittenToConsole.ShouldBe(expectedNrOfLinesWrittenToConsole);
            actualNrOfXmlsSaved.ShouldBe(expectedNrOfXmlsSaved);
        }

        [Test]
        public void SaveBulkXml_SaveXmlException_ExceptionIsCaughtAndLogged()
        {
            // Arrange
            var addressesList = CreateAddressesList(600);
            var actualNrOfLinesWrittenToConsole = 0;
            var actualNrOfXmlsSaved = 0;
            var actualNrOfLoggedExceptions = 0;
            var expectedNrOfLinesWrittenToConsole = 600;
            var expectedNrOfXmlsSaved = 1;
            var expectedNrOfLoggedExceptions = 1;
            var exceptionWasThrown = false;
            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, color) =>
            {
                actualNrOfLinesWrittenToConsole++;
            };
            ShimDataFunctions.CleanSerializedXMLString = xml =>
            {
                return "xml_string";
            };
            ShimAddress.SaveBulkXmlString = xml =>
            {
                if (!exceptionWasThrown)
                {
                    exceptionWasThrown = true;
                    throw new InvalidOperationException();
                }
                actualNrOfXmlsSaved++;
                return true;
            };
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
                (instance, ex, sourceMethod, application, note, clientId, subject) =>
                {
                    actualNrOfLoggedExceptions++;
                    return 1;
                };

            // Act
            _address.SaveBulkXml(addressesList);

            // Assert
            actualNrOfLinesWrittenToConsole.ShouldBe(expectedNrOfLinesWrittenToConsole);
            actualNrOfXmlsSaved.ShouldBe(expectedNrOfXmlsSaved);
            actualNrOfLoggedExceptions.ShouldBe(expectedNrOfLoggedExceptions);
        }

        private List<Entity.Address> CreateAddressesList(int count)
        {
            var accountId = 1;
            var subscriberId = 1;
            return Enumerable.Repeat(
                new Entity.Address
                {
                    account_id = accountId++,
                    subscriber_id = subscriberId++,
                    first_name = "",
                    last_name = "",
                    address = "",
                    address_id = 1,
                    address_line_2 = ""
                },
                count).ToList();
        }

        private void SetupFakes()
        {
            ShimAddress.UpdateForACSString = x =>
            {
                _updateForAcs = true;
                return true;
            };

            ShimAccount.AllInstances.SelectInt32 = (account, clientId) =>
            {
                clientId.ShouldBe(KmClientId);
                return _account;
            };

            ShimStringFunctions.WriteLineRepeaterStringConsoleColor = (msg, consoleColor) => { };

            FrameworkSubGenBusinessLogicAPI.ShimAddress.AllInstances.UpdateEnumsClientAddress = (addr, client, address) =>
            {
                client.ShouldBe(Enums.Client.Master);
                address.IsContentMatched(_expectedAddress).ShouldBeTrue();
                return null;
            };

            FrameworkSubGenBusinessLogicFakes.ShimAddress.AllInstances.GetXmlListOfAddress = (address, addresses) =>
            {
                _actualAddressList = addresses;
                return ShimsContext.ExecuteWithoutShims(() => address.GetXml(addresses));
            };

            FrameworkSubGenBusinessLogicFakes.ShimPublication.AllInstances.SelectKmPubCodeStringInt32 =
                (publication, pubCode, clientId) =>
                {
                    pubCode.ShouldBe(PubCode);
                    clientId.ShouldBe(KmClientId);

                    Entity.Publication publ = typeof(Entity.Publication).CreateInstance();
                    publ.publication_id = PublicationId;

                    return publ;
                };

            SetupFakesForSelectOnMailingId();
        }

        private void SetupFakesForSelectOnMailingId()
        {
            FrameworkSubGenBusinessLogicFakes.ShimAddress.AllInstances.SelectOnMailingIdInt32Int32 =
                (address, subscriberId, publisherId) =>
                {
                    subscriberId.ShouldBe(SubscriberId);
                    publisherId.ShouldBe(PublicationId);

                    return new List<Entity.Address>
                    {
                        new Entity.Address()
                    };
                };

            FrameworkSubGenBusinessLogicFakes.ShimAddress.AllInstances.SelectOnMailingIdInt32 =
                (address, subscriptionId) =>
                {
                    subscriptionId.ShouldBe(SubscriberId);

                    return new List<Entity.Address>
                    {
                        new Entity.Address()
                    };
                };

            FrameworkSubGenBusinessLogicFakes.ShimAddress.AllInstances
                    .SelectOnMailingIdInt32Int32StringStringStringStringString =
                (
                    address,
                    accountId,
                    publicationId,
                    oldAddress1,
                    oldAddress2,
                    oldCity,
                    oldStateAbbrevation,
                    oldZipCode) =>
                {
                    accountId.ShouldBe(_account.account_id);
                    publicationId.ShouldBe(PublicationId);
                    oldAddress1.ShouldBe(OldAddress1);
                    oldAddress2.ShouldBe(OldAddress2);
                    oldCity.ShouldBe(OldCity);
                    oldStateAbbrevation.ShouldBe(oldStateAbbrevation);
                    oldZipCode.ShouldBe(OldZipCode);

                    return new List<Entity.Address>
                    {
                        new Entity.Address()
                    };
                };
        }
    }
}
