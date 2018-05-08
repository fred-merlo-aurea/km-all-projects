using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using ECN.TestHelpers;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Interfaces;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN_Framework_EntitiesTests.Salesforce.Tests
{
    [TestFixture]
    public class SF_LeadTest : SalesForceTestBase
    {
        private const string EmailBouncedDateProperty = "EmailBouncedDate:true";
        private static CultureInfo _previousCulture;

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }

        [Test]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            string id = string.Empty;
            string masterRecordId = string.Empty;
            string title = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            string salutation = string.Empty;
            string name = string.Empty;
            string company = string.Empty;
            string street = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string postalCode = string.Empty;
            string country = string.Empty;
            double latitude = 0.0;
            double longitude = 0.0;
            string email = string.Empty;
            string fax = string.Empty;
            string mobilePhone = string.Empty;
            string phone = string.Empty;
            string website = string.Empty;
            string description = string.Empty;        

            // Act
            SF_Lead sF_Lead = new SF_Lead();    

            // Assert
            sF_Lead.Id.ShouldBe(id);
            sF_Lead.IsDeleted.ShouldBeFalse();
            sF_Lead.MasterRecordId.ShouldBe(masterRecordId);
            sF_Lead.Title.ShouldBe(title);
            sF_Lead.FirstName.ShouldBe(firstName);
            sF_Lead.LastName.ShouldBe(lastName);
            sF_Lead.Salutation.ShouldBe(salutation);
            sF_Lead.Name.ShouldBe(name);
            sF_Lead.Company.ShouldBe(company);
            sF_Lead.Street.ShouldBe(street);
            sF_Lead.City.ShouldBe(city);
            sF_Lead.State.ShouldBe(state);
            sF_Lead.PostalCode.ShouldBe(postalCode);
            sF_Lead.Country.ShouldBe(country);
            sF_Lead.Latitude.ShouldBe(latitude);
            sF_Lead.Longitude.ShouldBe(longitude);
            sF_Lead.Email.ShouldBe(email);
            sF_Lead.Fax.ShouldBe(fax);
            sF_Lead.MobilePhone.ShouldBe(mobilePhone);
            sF_Lead.Phone.ShouldBe(phone);
            sF_Lead.Website.ShouldBe(website);
            sF_Lead.Description.ShouldBe(description);
        }

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Arrange, Act
            var exception = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { null }));

            // Assert
            exception.InnerException.ShouldBeOfType<NullReferenceException>();
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyLeadList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result = typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { json }) as List<SF_Lead>;

            // Assert
            result.ShouldBeEmpty();
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyLeadList()
        {
            // Arrange
            var json = SfLeadJsonWithNonNullValues;
            var expectedList = SfLeadListWithNonNullValues;

            // Act
            var result = typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { json }) as List<SF_Lead>;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            expectedList.IsListContentMatched(result).ShouldBeTrue();
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyLeadList()
        {
            // Arrange
            var json = SfLeadJsonWithNullValues;
            var expectedList = SfLeadListWithNullValues;

            // Act
            var result = typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { json }) as List<SF_Lead>;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            expectedList.IsListContentMatched(result).ShouldBeTrue();
        }

        [Test]
        public void GetList_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasEmailBouncedDate_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(EmailBouncedDateProperty);
            var webRequest = CreateWebRequest(webResponse);
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(EmailBouncedDateProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetList(string.Empty, string.Empty);

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
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(EmailBouncedDateProperty);
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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetList(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetCampaignMembers_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasEmailBouncedDate_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(EmailBouncedDateProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(EmailBouncedDateProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetCampaignMembers(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldNotBeNull();
            contactsList.Count.ShouldBe(2);
            sfUtilities.VerifyAll();
        }

        [Test]
        public void GetCampaignMembers_ExceptionWhenGetResponseStream_ReturnsEmptyList()
        {
            // Arrange
            var webException = new WebException();
            var webResponse = CreateWebResponse(webException);
            var webRequest = CreateWebRequest(webResponse);
            var queryParameter = string.Empty;
            var sfUtilities = CreateSFUtilities(EmailBouncedDateProperty);
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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetCampaignMembers(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetAll_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasEmailBouncedDate_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(EmailBouncedDateProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(EmailBouncedDateProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetAll(string.Empty);

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
            var sfUtilities = CreateSFUtilities(EmailBouncedDateProperty);
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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        private static List<string> SfLeadJsonWithNonNullValues => new List<string>
        {
            " \"Id\": \"Id\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"MasterRecordId\": \"MasterRecordId\" ,",
            " \"Title\": \"Title\" ,",
            " \"FirstName\": \"FirstName\" ,",
            " \"LastName\": \"LastName\" ,",
            " \"Salutation\": \"Salutation\" ,",
            " \"Name\": \"Name\" ,",
            " \"Company\": \"Company\" ,",
            " \"City\": \"City\" ,",
            " \"State\": \"State\" ,",
            " \"Country\": \"Country\" ,",
            " \"PostalCode\": \"PostalCode\" ,",
            " \"Street\": \"Street\" ,",
            " \"Latitude\": \"10.0\" ,",
            " \"Longitude\": \"20.0\" ,",
            " \"Email\": \"Email\" ,",
            " \"Fax\": \"Fax\" ,",
            " \"MobilePhone\": \"MobilePhone\" ,",
            " \"Phone\": \"Phone\" ,",
            " \"Website\": \"Website\" ,",
            " \"Description\": \"Description\" ,",
            " \"LeadSource\": \"LeadSource\" ,",
            " \"Status\": \"Status\" ,",
            " \"Industry\": \"Industry\" ,",
            " \"Rating\": \"Rating\" ,",
            " \"AnnualRevenue\": \"89\" ,",
            " \"BirthDate\": \"01/25/2018 15:50:31.000+0000\" ,",
            " \"OwnerId\": \"OwnerId\" ,",
            " \"HasOptedOutOfEmail\": \"true\" ,",
            " \"IsConverted\": \"true\" ,",
            " \"ConvertedDate\": \"01/11/2018 15:52:31.000+0000\" ,",
            " \"ConvertedAccountId\": \"ConvertedAccountId\" ,",
            " \"ConvertedContactId\": \"ConvertedContactId\" ,",
            " \"ConvertedOpportunityId\": \"ConvertedOpportunityId\" ,",
            " \"IsUnreadByOwner\": \"true\" ,",
            " \"DoNotCall\": \"true\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"CreatedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"SystemModstamp\": \"SystemModstamp\" ,",
            " \"LastActivityDate\": \"01/22/2018 14:55:30.000+0000\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"LastModifiedDate\": \"01/18/2018 14:55:30.000+0000\" ,",
            " \"LastViewedDate\": \"01/21/2018 14:55:30.000+0000\" ,",
            " \"LastReferencedDate\": \"01/17/2018 14:55:30.000+0000\" ,",
            " \"JigsawContactId\": \"JigsawContactId\" ,",
            " \"EmailBounceReason\": \"EmailBounceReason\" ,",
            " \"EmailBouncedDate\": \"01/19/2018 14:55:30.000+0000\" ,"
        };

        private static List<SF_Lead> SfLeadListWithNonNullValues => new List<SF_Lead>
        {
            new SF_Lead
            {
                Id = "Id",
                IsDeleted = true,
                MasterRecordId = "MasterRecordId",
                Email = "Email",
                Fax = "Fax",
                FirstName = "FirstName",
                Salutation = "Salutation",
                Name = "Name",
                City = "City",
                State = "State",
                Country = "Country",
                PostalCode = "PostalCode",
                Street = "Street",
                Latitude = 10.0,
                Longitude = 20.0,
                LeadSource = "LeadSource",
                ConvertedDate = new DateTime(2018, 1, 11, 15, 52, 31),
                Description = "Description",
                OwnerId = "OwnerId",
                LastName = "LastName",
                MobilePhone = "MobilePhone",
                Phone = "Phone",
                Title = "Title",
                HasOptedOutOfEmail = true,
                DoNotCall = true,
                CreatedById = "CreatedById",
                CreatedDate = new DateTime(2018, 1, 26, 15, 52, 31),
                SystemModstamp = "SystemModstamp",
                LastActivityDate = new DateTime(2018, 1, 22, 14, 55, 30),
                LastModifiedById = "LastModifiedById",
                LastModifiedDate = new DateTime(2018, 1, 18, 14, 55, 30),
                LastViewedDate = new DateTime(2018, 1, 17, 14, 55, 30),
                LastReferencedDate = new DateTime(0001, 1, 1),
                EmailBouncedReason = "EmailBounceReason",
                EmailBouncedDate = new DateTime(2018, 1, 19, 14, 55, 30),
                JigsawContactId = "JigsawContactId",
                AnnualRevenue = 89,
                Company = "Company",
                ConvertedAccountId = "ConvertedAccountId",
                ConvertedContactId = "ConvertedContactId",
                ConvertedOpportunityId = "ConvertedOpportunityId",
                Industry = "Industry",
                IsConverted = true,
                IsUnreadByOwner = true,
                Rating = "Rating",
                Status = "Status",
                Website = "Website",
            }
        };

        private static List<string> SfLeadJsonWithNullValues => new List<string>
        {
            " \"Id\": null",
            " \"IsDeleted\": null",
            " \"MasterRecordId\": null",
            " \"Title\": null",
            " \"FirstName\": null",
            " \"LastName\": null",
            " \"Salutation\": null",
            " \"Name\": null",
            " \"Company\": null",
            " \"City\": null",
            " \"State\": null",
            " \"Country\": null",
            " \"PostalCode\": null",
            " \"Street\": null",
            " \"Latitude\": null",
            " \"Longitude\": null",
            " \"Email\": null",
            " \"Fax\": null",
            " \"MobilePhone\": null",
            " \"Phone\": null",
            " \"Website\": null",
            " \"Description\": null",
            " \"LeadSource\": null",
            " \"Status\": null",
            " \"Industry\": null",
            " \"Rating\": null",
            " \"AnnualRevenue\": null",
            " \"BirthDate\": null",
            " \"OwnerId\": null",
            " \"HasOptedOutOfEmail\": null",
            " \"IsConverted\": null",
            " \"ConvertedDate\": null",
            " \"ConvertedAccountId\": null",
            " \"ConvertedContactId\": null",
            " \"ConvertedOpportunityId\": null",
            " \"IsUnreadByOwner\": null",
            " \"DoNotCall\": null",
            " \"CreatedById\": null",
            " \"CreatedDate\": null",
            " \"SystemModstamp\": null",
            " \"LastActivityDate\": null",
            " \"LastModifiedById\": null",
            " \"LastModifiedDate\": null",
            " \"LastViewedDate\": null",
            " \"LastReferencedDate\": null",
            " \"JigsawContactId\": null",
            " \"EmailBounceReason\": null",
            " \"EmailBouncedDate\": null"
        };

        private static List<SF_Lead> SfLeadListWithNullValues => new List<SF_Lead>
        {
            new SF_Lead
            {
                Id = "null",
                IsDeleted = false,
                MasterRecordId = string.Empty,
                Industry = string.Empty,
                Email = string.Empty,
                Fax = string.Empty,
                FirstName = string.Empty,
                Salutation = string.Empty,
                Name = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Country = string.Empty,
                PostalCode = string.Empty,
                Street = string.Empty,
                Latitude = 0.0,
                Longitude = 0.0,
                Rating = string.Empty,
                Status = string.Empty,
                LeadSource = string.Empty,
                ConvertedDate = new DateTime(1900, 1, 1),
                Description = string.Empty,
                OwnerId = string.Empty,
                Website = string.Empty,
                LastName = string.Empty,
                Company = string.Empty,
                IsUnreadByOwner = false,
                MobilePhone = string.Empty,
                Phone = string.Empty,
                Title = string.Empty,
                HasOptedOutOfEmail = false,
                DoNotCall = false,
                CreatedById = string.Empty,
                CreatedDate = new DateTime(1900, 1, 1),
                SystemModstamp = string.Empty,
                LastActivityDate = new DateTime(1900, 1, 1),
                LastModifiedById = string.Empty,
                LastModifiedDate = new DateTime(1900, 1, 1),
                LastViewedDate = new DateTime(1900, 1, 1),
                LastReferencedDate = new DateTime(0001, 1, 1),
                EmailBouncedReason = string.Empty,
                EmailBouncedDate = new DateTime(1900, 1, 1),
                JigsawContactId = string.Empty,
                ConvertedOpportunityId = string.Empty,
                IsConverted = false,
                ConvertedAccountId = string.Empty,
                ConvertedContactId = string.Empty,
                AnnualRevenue = 0.0
            }
        };
    }
}