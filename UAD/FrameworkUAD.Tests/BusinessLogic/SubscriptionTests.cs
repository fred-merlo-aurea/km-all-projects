using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Transactions.Fakes;
using System.Xml.Serialization;
using FrameworkUAD.BusinessLogic;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using KmCommonFake = KM.Common.Fakes;
using UadBusinessLogic = FrameworkUAD.BusinessLogic.Fakes;
using UadDataAccess = FrameworkUAD.DataAccess.Fakes;

namespace FrameworkUAD.Tests.BusinessLogic
{
    /// <summary>
    /// Unit test for <see cref="Subscription"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SubscriptionTests
    {
        private const string DefaultText = "Unit Test";
        private const string NoImportEmailAddress = "NcoaImport@TeamKM.com";
        private const string MailStop1 = "admin1@unittest.com";
        private const string MailStop2 = "admin2@unittest.com";
        private const string MailStop = "admin@unittest.com";
        private const string ZipCode = "012345678";
        private const string CountryCanada = "Canada";
        private const string CountryUsa = "UNITED STATES";
        private const int CountryIdUsa = 1;
        private const int CountryIdCanada = 2;
        private const int TestOne = 1;
        private IDisposable _shimContext;
        private Subscription _subscription;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _subscription = new Subscription();
            KmCommonFake.ShimDataFunctions.ExecuteNonQuerySqlCommand = (x) => true;
            UadDataAccess.ShimDataFunctions.ExecuteScalarSqlCommand = (x) => true;
            ShimTransactionScope.AllInstances.Complete = (sender) => { };
            UadDataAccess.ShimDataFunctions.GetClientSqlConnectionClient = (x) => new SqlConnection();
            KmCommonFake.ShimDataFunctions.GetDataTableViaAdapterSqlCommand = (x) => { return new DataTable(); };
            KmCommonFake.ShimDataFunctions.GetDataTableViaAdapterSqlCommandString = (x, y) => { return new DataTable(); };
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext?.Dispose();
        }

        [Test]
        public void AddressUpdate_SubscriptionObjectIsnotNull_ReturnsTrueStatus()
        {
            // Arrange
            var listSubscription = CreateSubscriptionListObject();
            var client = new KMPlatform.Object.ClientConnections();

            // Act
            var result = _subscription.AddressUpdate(listSubscription, client);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void AddressUpdate_BySubscriptionObjectXmlString_ReturnsTrueStatus()
        {
            // Arrange
            var listSubscription = CreateSubscriptionListObject();
            var client = new KMPlatform.Object.ClientConnections();
            var xmlString = ConvertObjectToXMLString(listSubscription);

            // Act
            var result = _subscription.AddressUpdate(xmlString, client);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void NcoaUpdateAddress_BySubscriptionObjectXmlString_ReturnsTrueStatus()
        {
            // Arrange
            var emailExist = false;
            var listSubscription = CreateSubscriptionListObject();
            var client = new KMPlatform.Object.ClientConnections();
            var xmlString = ConvertObjectToXMLString(listSubscription);
            ShimUser.AllInstances.SearchEmailString = (sender, emailAddress) =>
            {
                emailExist = true;
                return new KMPlatform.Entity.User
                {
                    UserID = TestOne,
                    UserName = DefaultText,
                    EmailAddress = NoImportEmailAddress
                };
            };
            // Act
            var result = _subscription.NcoaUpdateAddress(xmlString, client, TestOne);

            // Assert
            emailExist.ShouldBeTrue();
            result.ShouldBeTrue();
        }

        [Test]
        public void ExistsStandardFieldName_NameIsNotNull_ReturnsTrueStatus()
        {
            // Arrange
            var client = new KMPlatform.Object.ClientConnections();
            var name = DefaultText;

            // Act
            var result = _subscription.ExistsStandardFieldName(name, client);

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void FindMatches_MethodsParametrsHaveValue_ReturnsObjectResult()
        {
            // Arrange
            var productId = 1;
            var client = new KMPlatform.Object.ClientConnections();

            // Act
            var result = _subscription.FindMatches(productId, DefaultText, DefaultText, DefaultText, DefaultText, DefaultText, DefaultText, DefaultText, DefaultText, DefaultText, client);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void FormatZipCode_EntitySubscriptionObjectIsNotNull_returnsObjectValue()
        {
            // Arrange
            var listSubscription = CreateSubscriptionListObject();

            // Act
            var result = _subscription.FormatZipCode(listSubscription);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(4));
        }

        [Test]
        public void Save_SubscriptionObjectIsNoTNull_ReturnsSubscriptionIdValue()
        {
            // Arrange
            var subscription = new Entity.Subscription();
            var client = new KMPlatform.Object.ClientConnections();

            // Act
            var result = _subscription.Save(subscription, client);

            // Assert
            result.ShouldBe(TestOne);
        }

        [Test]
        public void Select_ClientDisplayNameParameterIsNotNull_ReturnObjectValue()
        {
            //Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadBusinessLogic.ShimSubscriberConsensusDemographic.AllInstances.SelectInt32ClientConnections = (sender, id, clientConnecton) => CreateSubscriberConsensusDemographic();
            UadBusinessLogic.ShimProductSubscription.AllInstances.SelectInt32ClientConnectionsStringBoolean = (x, y, z, m, n) => CreateProductSubscription();
            UadDataAccess.ShimSubscription.SelectClientConnections = (x) => CreateSubscriptionListObject();

            // Act
            var result = _subscription.Select(client, string.Empty, true);

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(4),
                () => result[0].ProductList.Count.ShouldBe(4),
                () => result[0].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[1].ProductList.Count.ShouldBe(4),
                () => result[1].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[2].ProductList.Count.ShouldBe(4),
                () => result[2].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[3].ProductList.Count.ShouldBe(4),
                () => result[3].SubscriptionConsensusDemographics.Count.ShouldBe(2));
        }

        [Test]
        public void Select_EmailAndClientDisplayNameParameterIsNotNull_ReturnObjectValue()
        {
            //Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadBusinessLogic.ShimSubscriberConsensusDemographic.AllInstances.SelectInt32ClientConnections = (sender, id, clientConnecton) => CreateSubscriberConsensusDemographic();
            UadBusinessLogic.ShimProductSubscription.AllInstances.SelectInt32ClientConnectionsStringBoolean = (x, y, z, m, n) => CreateProductSubscription();
            UadDataAccess.ShimSubscription.SelectStringClientConnectionsBoolean = (x, y, z) => CreateSubscriptionListObject();

            // Act
            var result = _subscription.Select(string.Empty, client, string.Empty, true, true);

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(4),
                () => result[0].ProductList.Count.ShouldBe(4),
                () => result[0].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[1].ProductList.Count.ShouldBe(4),
                () => result[1].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[2].ProductList.Count.ShouldBe(4),
                () => result[2].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[3].ProductList.Count.ShouldBe(4),
                () => result[3].SubscriptionConsensusDemographics.Count.ShouldBe(2));
        }

        [Test]
        public void SelectInValidAddresses_ClientDisplayNameParameterIsNotNull_ReturnObjectValue()
        {
            //Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadBusinessLogic.ShimSubscriberConsensusDemographic.AllInstances.SelectInt32ClientConnections = (sender, id, clientConnecton) => CreateSubscriberConsensusDemographic();
            UadBusinessLogic.ShimProductSubscription.AllInstances.SelectInt32ClientConnectionsStringBoolean = (x, y, z, m, n) => CreateProductSubscription();
            UadDataAccess.ShimSubscription.SelectInValidAddressesClientConnections = (x) => CreateSubscriptionListObject();

            // Act
            var result = _subscription.SelectInValidAddresses(client, string.Empty, true);

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(4),
                () => result[0].ProductList.Count.ShouldBe(4),
                () => result[0].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[1].ProductList.Count.ShouldBe(4),
                () => result[1].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[2].ProductList.Count.ShouldBe(4),
                () => result[2].SubscriptionConsensusDemographics.Count.ShouldBe(2),
                () => result[3].ProductList.Count.ShouldBe(4),
                () => result[3].SubscriptionConsensusDemographics.Count.ShouldBe(2));
        }

        [Test]
        public void SelectIds_ByClientConnectionObject_ReturnsObjectValue()
        {
            // Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadDataAccess.ShimSubscription.SelectIDsClientConnections = (x) => Enumerable.Range(1, 10).ToList();

            // Act
            var result = _subscription.SelectIDs(client);

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNull(),
                 () => result.Any().ShouldBeTrue(),
                 () => result.Count.ShouldBe(10),
                 () => result.First().ShouldBe(1),
                 () => result.Last().ShouldBe(10));
        }

        [Test]
        public void SelectForFileAudit_ProcessCodeisNotNull_ReturnsObjectValue()
        {
            // Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadDataAccess.ShimSubscription.GetListSqlCommandBoolean = (x, y) => CreateSubscriptionListObject();

            // Act
            var result = _subscription.SelectForFileAudit(DefaultText, TestOne, DateTime.Now, DateTime.Now, client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Any().ShouldBeTrue(),
                () => result.Count.ShouldBe(4));
        }

        [Test]
        public void Select_ClientDisplayNameIsNotNullAndSubscriptionIdHaveValue_ReturnsObjectValue()
        {
            // Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadDataAccess.ShimSubscription.SelectInt32ClientConnections = (x, y) => new Entity.Subscription();

            // Act
            var result = _subscription.Select(TestOne, client, DefaultText, true);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Plus4.ShouldBeNullOrEmpty(),
                () => result.Priority.ShouldBeNullOrEmpty(),
                () => result.ProductCode.ShouldBeNullOrEmpty(),
                () => result.RegCode.ShouldBeNullOrEmpty());
        }

        [Test]
        public void SearchAddressZip_AddressAndZipParameterISNotNull_ReturnsObjectValue()
        {
            // Arrange
            var client = new KMPlatform.Object.ClientConnections();
            UadDataAccess.ShimSubscription.SearchAddressZipStringStringClientConnections = (x, y, z) => CreateSubscriptionListObject();
            // Act
            var result = _subscription.SearchAddressZip(DefaultText, ZipCode, client);

            // Assert
            result.ShouldSatisfyAllConditions(
               () => result.ShouldNotBeNull(),
               () => result.Any().ShouldBeTrue(),
               () => result.Count.ShouldBe(4));
        }

        private static string ConvertObjectToXMLString(object resultObject)
        {
            var xmlString = string.Empty;
            var xmlSerializer = new XmlSerializer(resultObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, resultObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }

        private static List<Entity.Subscription> CreateSubscriptionListObject()
        {
            return new List<Entity.Subscription>
            {
                new Entity.Subscription(){
                    Country = DefaultText,
                    CountryID = CountryIdUsa,
                },
                new Entity.Subscription(true)
                {
                    Address = DefaultText,
                    MailStop = MailStop,
                    City = DefaultText,
                    State = DefaultText,
                    Zip = ZipCode,
                    Plus4 = DefaultText,
                    Latitude = 0,
                    Longitude = 0,
                    IsLatLonValid = true,
                    LatLonMsg = string.Empty,
                    Country = CountryCanada,
                    CountryID = CountryIdCanada,
                    County = string.Empty
                },
                new Entity.Subscription(new Entity.Subscription())
                {
                    Address = DefaultText,
                    MailStop =MailStop1,
                    City = DefaultText,
                    State =DefaultText,
                    Zip = ZipCode,
                    Plus4 = string.Empty,
                    Latitude = 0,
                    Longitude = 0,
                    IsLatLonValid = true,
                    LatLonMsg = string.Empty,
                    Country = CountryUsa,
                    CountryID= CountryIdUsa,
                    County = string.Empty
                },
                new Entity.Subscription(new Entity.IssueCompDetail())
                {
                    Address = DefaultText,
                    MailStop = MailStop2,
                    City =DefaultText,
                    State = DefaultText,
                    Zip = ZipCode,
                    Plus4 = string.Empty,
                    Latitude = 0,
                    Longitude = 0,
                    IsLatLonValid = true,
                    LatLonMsg = string.Empty,
                     Country = CountryUsa,
                    CountryID= CountryIdUsa,
                    County = string.Empty
                }
            };
        }

        private static List<Entity.ProductSubscription> CreateProductSubscription()
        {
            return new List<Entity.ProductSubscription>
            {
                new Entity.ProductSubscription(),
                new Entity.ProductSubscription(new Entity.ProductSubscription()),
                new Entity.ProductSubscription(new Entity.ProductSubscription()),
                new Entity.ProductSubscription(new Entity.Subscription())
            };
        }

        private static List<Object.SubscriberConsensusDemographic> CreateSubscriberConsensusDemographic()
        {
            return new List<Object.SubscriberConsensusDemographic>
            {
                 new Object.SubscriberConsensusDemographic(),
                 new Object.SubscriberConsensusDemographic(DefaultText,DefaultText,DefaultText)
            };
        }
    }
}
