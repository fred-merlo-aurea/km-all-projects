using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using Moq;
using NUnit.Framework;
using Shouldly;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Interfaces;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_Account"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_AccountTest : SalesForceTestBase
    {
        private const string JigsawCompanyIdProperty = "JigsawCompanyId: true";
        private List<SF_Account> accounts;
        private static CultureInfo previousCulture;

        /// <summary>
        ///     Setup up <see cref="accounts"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => accounts = new List<SF_Account>
        {
            new SF_Account(),
            new SF_Account(),
            new SF_Account()
        };

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        [Test]
        public void Sort_SortByPropertyInAscendingOrder_ReturnSortedList(
            [ValueSource(nameof(SortProperties))] string sortBy)
        {
            // Arrange
            accounts.ForEach(x => x.SetProperty(sortBy, accounts.IndexOf(x)));
            List<SF_Account> expected = accounts.OrderBy(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            var result = SF_Account.Sort(accounts, sortBy, System.Web.UI.WebControls.SortDirection.Ascending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Sort_SortByPropertyInDescendingOrder_ReturnSortedList(
            [ValueSource(nameof(SortProperties))] string sortBy)
        {
            // Arrange
            accounts.ForEach(x => x.SetProperty(sortBy, accounts.IndexOf(x)));
            List<SF_Account> expected = accounts.OrderByDescending(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            var result = SF_Account.Sort(accounts, sortBy, System.Web.UI.WebControls.SortDirection.Descending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        private static readonly string[] SortProperties =
        {
            "Name",
            "BillingStreet",
            "BillingCity",
            "BillingState",
            "BillingCountry",
            "BillingPostalCode",
            "Phone",
            "Fax",
            "Website",
            "Industry",
            "AnnualRevenue"
        };

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            TargetInvocationException exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_Account).CallMethod("ConvertJsonList", new object[] { null }));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyAccountList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (List<SF_Account>)typeof(SF_Account).CallMethod("ConvertJsonList", new object[] { json });

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyAccountList()
        {
            // Arrange
            List<string> json = SfAccountJsonWithNonNullValues;
            List<SF_Account> expectedList = SfAccountListWithNonNullValues;

            // Act
            var result =
                (List<SF_Account>)typeof(SF_Account).CallMethod("ConvertJsonList", new object[] { json });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyAccountList()
        {
            // Arrange
            List<string> json = SfAccountJsonWithNullValues;

            List<SF_Account> expectedList = SfAccountListWithNullValues;

            // Act
            var result =
                (List<SF_Account>)typeof(SF_Account).CallMethod("ConvertJsonList", new object[] { json });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void GetList_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasJigsawCompanyId_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(JigsawCompanyIdProperty);
            var webRequest = CreateWebRequest(webResponse);
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(JigsawCompanyIdProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Account.GetList(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldNotBeNull();
            contactsList.Count.ShouldBe(2);
            sfUtilities.VerifyAll();
        }

        [Test]
        public void GetList_ExceptionWhenGetResponseStream_ReturnsEmptyList()
        {
            // Arrange
            var webException = new WebException();
            Mock<WebResponse> webResponse = CreateWebResponse(webException);
            Mock<WebRequest> webRequest = CreateWebRequest(webResponse);
            var queryParameter = string.Empty;
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(JigsawCompanyIdProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                                        It.IsAny<string>(),
                                        It.IsAny<string>(),
                                        It.IsAny<Method>(),
                                        It.IsAny<ResponseType>()))
                                            .Returns(webRequest.Object)
                                            .Callback<string, string, Method, ResponseType>((token, query, method, response) =>
                                            {
                                                queryParameter = query;
                                            });
            sfUtilities.Setup(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()));
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Account.GetList(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetAll_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasJigsawCompanyId_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(JigsawCompanyIdProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(JigsawCompanyIdProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Account.GetAll(string.Empty);

            // Assert
            contactsList.ShouldNotBeNull();
            contactsList.Count.ShouldBe(2);
            sfUtilities.VerifyAll();
        }

        [Test]
        public void GetAll_ExceptionWhenGetResponseStream_ReturnsEmptyList()
        {
            // Arrange
            var webException = new WebException();
            var webResponse = CreateWebResponse(webException);
            var webRequest = CreateWebRequest(webResponse);
            var queryParameter = string.Empty;
            var sfUtilities = CreateSFUtilities(JigsawCompanyIdProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                                        It.IsAny<string>(),
                                        It.IsAny<string>(),
                                        It.IsAny<Method>(),
                                        It.IsAny<ResponseType>()))
                                            .Returns(webRequest.Object)
                                            .Callback<string, string, Method, ResponseType>((token, query, method, response) =>
                                            {
                                                queryParameter = query;
                                            });
            sfUtilities.Setup(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()));
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Account.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void Update_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            var token = "accessToken";
            var accountId = "Id";
            var account = new SF_Account() { Id = accountId };
            var sfUtilities = new Mock<ISFUtilities>();
            sfUtilities.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<SalesForceBase>(), It.IsAny<SFObject>(), It.IsAny<string>())).Returns(true);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Account.Update(token, account);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Update(
                It.Is<string>(t => t == token),
                It.Is<SalesForceBase>(sf => sf == account),
                It.Is<SFObject>(sf => sf == SFObject.Account),
                It.Is<string>(id => id == accountId)),
                Times.Once);
        }

        [Test]
        public void Insert_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            const string token = "accessToken";
            var account = new SF_Account();
            var sfUtilities = new Mock<ISFUtilities>();
            sfUtilities.Setup(x => x.Insert(token, account, SFObject.Account)).Returns(true);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Account.Insert(token, account);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Insert(
                    It.Is<string>(t => t == token),
                    It.Is<SalesForceBase>(sf => sf == account),
                    It.Is<SFObject>(sf => sf == SFObject.Account)),
                Times.Once);
        }

        private static List<SF_Account> SfAccountListWithNullValues => new List<SF_Account>
        {
            new SF_Account
            {
                Id = "null",
                Description = string.Empty,
                AnnualRevenue = 0.0,
                BillingStreet = string.Empty,
                BillingCity = string.Empty,
                BillingState = string.Empty,
                BillingPostalCode = string.Empty,
                BillingCountry = string.Empty,
                BillingLatitude = 0.0,
                BillingLongitude = 0.0,
                Phone = string.Empty,
                Fax = string.Empty,
                LastModifiedDate = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                IsDeleted = false,
                MasterRecordId = string.Empty,
                Name = string.Empty,
                Type = string.Empty,
                ParentId = string.Empty,
                Website = string.Empty,
                Industry = string.Empty,
                OwnerId = string.Empty,
                CreatedDate = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                CreatedById = string.Empty,
                LastModifiedById = string.Empty,
                SystemModstamp = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                LastActivityDate = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                LastViewedDate = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                LastReferencedDate = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                JigsawCompanyId = string.Empty
            }
        };

        private static List<string> SfAccountJsonWithNullValues => new List<string>
        {
            " \"Id: null",
            " \"Description\": null",
            " \"AnnualRevenue\": null",
            " \"BillingStreet\": null",
            " \"BillingCity\": null",
            " \"BillingState\": null",
            " \"BillingPostalCode\": null",
            " \"BillingCountry\": null",
            " \"BillingLatitude\": null",
            " \"BillingLongitude\": null",
            " \"Phone\": null",
            " \"Fax\": null",
            " \"LastModifiedDate\": null",
            " \"IsDeleted\": null",
            " \"MasterRecordId\": null",
            " \"Name\": null",
            " \"Type\": null",
            " \"ParentId\": null",
            " \"Website\": null",
            " \"Industry\": null",
            " \"OwnerId\": null",
            " \"CreatedDate\": null",
            " \"CreatedById\": null",
            " \"LastModifiedById\": null",
            " \"SystemModstamp\": null",
            " \"LastActivityDate\": null",
            " \"LastViewedDate\": null",
            " \"LastReferencedDate\": null",
            " \"JigsawCompanyId\": \"null"
        };

        private static List<string> SfAccountJsonWithNonNullValues => new List<string>
        {
            " \"Id\": \"1\" ,",
            " \"Description\": \"Description\" ,",
            " \"AnnualRevenue\": \"100\" ,",
            " \"BillingStreet\": \"BillingStreet\" ,",
            " \"BillingCity\": \"BillingCity\" ,",
            " \"BillingState\": \"BillingState\" ,",
            " \"BillingPostalCode\": \"BillingPostalCode\" ,",
            " \"BillingCountry\": \"BillingCountry\" ,",
            " \"BillingLatitude\": \"10.0\" ,",
            " \"BillingLongitude\": \"15.0\" ,",
            " \"Phone\": \"Phone\" ,",
            " \"Fax\": \"Fax\" ,",
            " \"LastModifiedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"MasterRecordId\": \"MasterRecordId\" ,",
            " \"Name\": \"Name\" ,",
            " \"Type\": \"Type\" ,",
            " \"ParentId\": \"ParentId\" ,",
            " \"Website\": \"Website\" ,",
            " \"Industry\": \"Industry\" ,",
            " \"OwnerId\": \"OwnerId\" ,",
            " \"CreatedDate\": \"01/25/2018 14:55:30.000+0000\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"SystemModstamp\": \"01/24/2018 12:51:30.000+0000\" ,",
            " \"LastActivityDate\": \"01/22/2018 14:55:30.000+0000\" ,",
            " \"LastViewedDate\": \"01/21/2018 14:55:30.000+0000\" ,",
            " \"LastReferencedDate\": \"01/20/2018 14:55:30.000+0000\" ,",
            " \"JigsawCompanyId\": \"JigsawCompanyId\" ,",
            " \"done\": \"false\" ,",
            " \"nextRecordsUrl\": \"nextRecordsUrl\" ,"
        };

        private static List<SF_Account> SfAccountListWithNonNullValues => new List<SF_Account>
        {
            new SF_Account
            {
                Id = "1",
                Description = "Description",
                AnnualRevenue = 100,
                BillingStreet = "BillingStreet",
                BillingCity = "BillingCity",
                BillingState = "BillingState",
                BillingPostalCode = "BillingPostalCode",
                BillingCountry = "BillingCountry",
                BillingLatitude = 10.0,
                BillingLongitude = 15.0,
                Phone = "Phone",
                Fax = "Fax",
                LastModifiedDate = new DateTime(2018, 1, 26, 15, 52, 31, 0),
                IsDeleted = true,
                MasterRecordId = "MasterRecordId",
                Name = "Name",
                Type = "Type",
                ParentId = "ParentId",
                Website = "Website",
                Industry = "Industry",
                OwnerId = "OwnerId",
                CreatedDate = new DateTime(2018, 1, 25, 14, 55, 30, 0),
                CreatedById = "CreatedById",
                LastModifiedById = "LastModifiedById",
                SystemModstamp = new DateTime(2018, 1, 24, 12, 51, 30, 0),
                LastActivityDate = new DateTime(2018, 1, 22, 14, 55, 30, 0),
                LastViewedDate = new DateTime(2018, 1, 21, 14, 55, 30, 0),
                LastReferencedDate = new DateTime(2018, 1, 20, 14, 55, 30, 0),
                JigsawCompanyId = "JigsawCompanyId"
            }
        };
    }
}