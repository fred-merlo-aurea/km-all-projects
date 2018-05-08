using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    [ExcludeFromCodeCoverage]
    public class SF_ContactTest : SalesForceTestBase
    {
        private const string MasterSuppressedCProperty = "Master_Suppressed__c:true";
        private static CultureInfo previousCulture;

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
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {
            // Arrange    
            string id = "null";
            string masterRecordId = "null";
            string accountId = "null";
            string email = "null";
            string fax = "null";
            string firstName = "null";
            string salutation = "null";
            string name = "null";
            string otherCity = "null";
            string otherStreet = "null";
            string otherState = "null";
            string otherPostalCode = "null";
            string otherCountry = "null";
            double otherLatitude = 0.0;
            double otherLongitude = 0.0;
            string otherPhone = "null";
            string assistantPhone = "null";
            string assistantName = "null";
            string leadSource = "null";
            DateTime birthDate = new DateTime(1900, 1, 1);
            string description = "null";
            string ownerId = "null";
            string homePhone = "null";
            string lastName = "null";
            string mailingCity = "null";
            string mailingState = "null";
            string mailingCountry = "null";
            string mailingPostalCode = "null";
            string mailingStreet = "null";
            string mobilePhone = "null";
            string phone = "null";
            string title = "null";
            string department = "null";
            DateTime createdDate = new DateTime(1900, 1, 1);
            string createdById = "null";
            DateTime lastModifiedDate = new DateTime(1900, 1, 1);
            string lastModifiedById = "null";
            DateTime systemModstamp = new DateTime(1900, 1, 1);
            DateTime lastActivityDate = new DateTime(1900, 1, 1);
            DateTime lastCURequestDate = new DateTime(1900, 1, 1);
            DateTime lastViewedDate = new DateTime(1900, 1, 1);
            DateTime lastReferencedDate = new DateTime(1900, 1, 1);
            string emailBouncedReason = "null";
            DateTime emailBouncedDate = new DateTime(1900, 1, 1);
            string jigsawContactId = "null";        

            // Act
            SF_Contact sF_Contact = new SF_Contact();    

            // Assert
            sF_Contact.Id.ShouldBe(id);
            sF_Contact.IsDeleted.ShouldBeFalse();
            sF_Contact.MasterRecordId.ShouldBe(masterRecordId);
            sF_Contact.AccountId.ShouldBe(accountId);
            sF_Contact.Email.ShouldBe(email);
            sF_Contact.Fax.ShouldBe(fax);
            sF_Contact.FirstName.ShouldBe(firstName);
            sF_Contact.Salutation.ShouldBe(salutation);
            sF_Contact.Name.ShouldBe(name);
            sF_Contact.OtherCity.ShouldBe(otherCity);
            sF_Contact.OtherStreet.ShouldBe(otherStreet);
            sF_Contact.OtherState.ShouldBe(otherState);
            sF_Contact.OtherPostalCode.ShouldBe(otherPostalCode);
            sF_Contact.OtherCountry.ShouldBe(otherCountry);
            sF_Contact.OtherLatitude.ShouldBe(otherLatitude);
            sF_Contact.OtherLongitude.ShouldBe(otherLongitude);
            sF_Contact.OtherPhone.ShouldBe(otherPhone);
            sF_Contact.AssistantPhone.ShouldBe(assistantPhone);
            sF_Contact.AssistantName.ShouldBe(assistantName);
            sF_Contact.LeadSource.ShouldBe(leadSource);
            sF_Contact.BirthDate.ShouldBe(birthDate);
            sF_Contact.Description.ShouldBe(description);
            sF_Contact.OwnerId.ShouldBe(ownerId);
            sF_Contact.HomePhone.ShouldBe(homePhone);
            sF_Contact.LastName.ShouldBe(lastName);
            sF_Contact.MailingCity.ShouldBe(mailingCity);
            sF_Contact.MailingState.ShouldBe(mailingState);
            sF_Contact.MailingCountry.ShouldBe(mailingCountry);
            sF_Contact.MailingPostalCode.ShouldBe(mailingPostalCode);
            sF_Contact.MailingStreet.ShouldBe(mailingStreet);
            sF_Contact.MobilePhone.ShouldBe(mobilePhone);
            sF_Contact.Phone.ShouldBe(phone);
            sF_Contact.Title.ShouldBe(title);
            sF_Contact.Department.ShouldBe(department);
            sF_Contact.HasOptedOutOfEmail.ShouldBeFalse();
            sF_Contact.DoNotCall.ShouldBeFalse();
            sF_Contact.CreatedDate.ShouldBe(createdDate);
            sF_Contact.CreatedById.ShouldBe(createdById);
            sF_Contact.LastModifiedDate.ShouldBe(lastModifiedDate);
            sF_Contact.LastModifiedById.ShouldBe(lastModifiedById);
            sF_Contact.SystemModstamp.ShouldBe(systemModstamp);
            sF_Contact.LastActivityDate.ShouldBe(lastActivityDate);
            sF_Contact.LastCURequestDate.ShouldBe(lastCURequestDate);
            sF_Contact.LastViewedDate.ShouldBe(lastViewedDate);
            sF_Contact.LastReferencedDate.ShouldBe(lastReferencedDate);
            sF_Contact.EmailBouncedReason.ShouldBe(emailBouncedReason);
            sF_Contact.EmailBouncedDate.ShouldBe(emailBouncedDate);
            sF_Contact.JigsawContactId.ShouldBe(jigsawContactId);
        }

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            var exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { null }));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyContactList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result = typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { json }) as List<SF_Contact>;

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyContactList()
        {
            // Arrange
            var json = SfContactJsonWithNonNullValues;
            var expectedList = SfContactListWithNonNullValues;

            // Act
            var result = typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { json }) as List<SF_Contact>;

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyContactList()
        {
            // Arrange
            var json = SfContactJsonWithNullValues;
            var expectedList = SfContactListWithNullValues;

            // Act
            var result = typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { json }) as List<SF_Contact>;

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void GetList_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasMasterSuppressedC_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(MasterSuppressedCProperty);
            var webRequest = CreateWebRequest(webResponse);
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Contact.GetList(string.Empty, string.Empty);

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
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
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
            var contactsList = SF_Contact.GetList(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetCampaignMembers_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasMasterSuppressedC_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(MasterSuppressedCProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Contact.GetCampaignMembers(string.Empty, string.Empty);

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
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
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
            var contactsList = SF_Contact.GetCampaignMembers(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetListForMS_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasMasterSuppressedC_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(MasterSuppressedCProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Contact.GetListForMS(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldNotBeNull();
            contactsList.Count.ShouldBe(2);
            sfUtilities.VerifyAll();
        }

        [Test]
        public void GetListForMS_ExceptionWhenGetResponseStream_ReturnsEmptyList()
        {
            // Arrange
            var webException = new WebException();
            var webResponse = CreateWebResponse(webException);
            var webRequest = CreateWebRequest(webResponse);
            var queryParameter = string.Empty;
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
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
            var contactsList = SF_Contact.GetListForMS(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void GetAll_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasMasterSuppressedC_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(MasterSuppressedCProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Contact.GetAll(string.Empty);

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
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
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
            var contactsList = SF_Contact.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        private static List<string> SfContactJsonWithNonNullValues => new List<string>
        {
            " \"done\": \"true\" ,",
            " \"nextRecordsUrl\": \"nextRecordsUrl\" ,",
            " \"Id\": \"Id\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"MasterRecordId\": \"MasterRecordId\" ,",
            " \"AccountId\": \"AccountId\" ,",
            " \"Email\": \"Email\" ,",
            " \"Fax\": \"Fax\" ,",
            " \"FirstName\": \"FirstName\" ,",
            " \"Salutation\": \"Salutation\" ,",
            " \"Name\": \"Name\" ,",
            " \"OtherCity\": \"OtherCity\" ,",
            " \"OtherState\": \"OtherState\" ,",
            " \"OtherCountry\": \"OtherCountry\" ,",
            " \"OtherPostalCode\": \"OtherPostalCode\" ,",
            " \"OtherStreet\": \"OtherStreet\" ,",
            " \"OtherLatitude\": \"10.0\" ,",
            " \"OtherLongitude\": \"20.0\" ,",
            " \"AssistantName\": \"AssistantName\" ,",
            " \"AssistantPhone\": \"AssistantPhone\" ,",
            " \"LeadSource\": \"LeadSource\" ,",
            " \"BirthDate\": \"01/25/2018 15:50:31.000+0000\" ,",
            " \"Description\": \"Description\" ,",
            " \"OwnerId\": \"OwnerId\" ,",
            " \"HomePhone\": \"HomePhone\" ,",
            " \"LastName\": \"LastName\" ,",
            " \"MailingCity\": \"MailingCity\" ,",
            " \"MailingState\": \"MailingState\" ,",
            " \"MailingCountry\": \"MailingCountry\" ,",
            " \"MailingPostalCode\": \"MailingPostalCode\" ,",
            " \"MailingStreet\": \"MailingStreet\" ,",
            " \"MailingLatitude\": \"35.00\" ,",
            " \"MailingLongitude\": \"-40.00\" ,",
            " \"MobilePhone\": \"MobilePhone\" ,",
            " \"Phone\": \"Phone\" ,",
            " \"Title\": \"Title\" ,",
            " \"Department\": \"Department\" ,",
            " \"HasOptedOutOfEmail\": \"true\" ,",
            " \"DoNotCall\": \"true\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"CreatedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"SystemModstamp\": \"01/15/2018 15:52:31.000+0000\" ,",
            " \"LastActivityDate\": \"01/22/2018 14:55:30.000+0000\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"LastModifiedDate\": \"01/18/2018 14:55:30.000+0000\" ,",
            " \"LastCURequestDate\": \"01/20/2018 14:55:30.000+0000\" ,",
            " \"LastViewedDate\": \"01/21/2018 14:55:30.000+0000\" ,",
            " \"LastReferencedDate\": \"01/17/2018 14:55:30.000+0000\" ,",
            " \"EmailBounceReason\": \"EmailBounceReason\" ,",
            " \"EmailBounceDate\": \"01/19/2018 14:55:30.000+0000\" ,",
            " \"JigsawContactId\": \"JigsawContactId\" ,",
            " \"Master_Suppressed__c\": \"true\" ,"
        };

        private static List<SF_Contact> SfContactListWithNonNullValues => new List<SF_Contact>
        {
            new SF_Contact
            {
                Id = "Id",
                IsDeleted = true,
                MasterRecordId = "MasterRecordId",
                AccountId = "AccountId",
                Email = "Email",
                Fax = "Fax",
                FirstName = "FirstName",
                Salutation = "Salutation",
                Name = "Name",
                OtherCity = "OtherCity",
                OtherState = "OtherState",
                OtherCountry = "OtherCountry",
                OtherPostalCode = "OtherPostalCode",
                OtherStreet = "OtherStreet",
                OtherLatitude = 10.0,
                OtherLongitude = 20.0,
                AssistantName = "AssistantName",
                AssistantPhone = "AssistantPhone",
                LeadSource = "LeadSource",
                BirthDate = new DateTime(2018, 1, 25, 15, 50, 31),
                Description = "Description",
                OwnerId = "OwnerId",
                HomePhone = "HomePhone",
                LastName = "LastName",
                MailingCity = "MailingCity",
                MailingState = "MailingState",
                MailingCountry = "MailingCountry",
                MailingPostalCode = "MailingPostalCode",
                MailingStreet = "MailingStreet",
                MailingLatitude = 35.00,
                MailingLongitude = -40.00,
                MobilePhone = "MobilePhone",
                Phone = "Phone",
                Title = "Title",
                Department = "Department",
                HasOptedOutOfEmail = true,
                DoNotCall = true,
                CreatedById = "CreatedById",
                CreatedDate = new DateTime(2018, 1, 26, 15, 52, 31),
                SystemModstamp = new DateTime(2018, 1, 15, 15, 52, 31),
                LastActivityDate = new DateTime(2018, 1, 22, 14, 55, 30),
                LastModifiedById = "LastModifiedById",
                LastModifiedDate = new DateTime(2018, 1, 18, 14, 55, 30),
                LastCURequestDate = new DateTime(2018, 1, 20, 14, 55, 30),
                LastViewedDate = new DateTime(2018, 1, 21, 14, 55, 30),
                LastReferencedDate = new DateTime(2018, 1, 17, 14, 55, 30),
                EmailBouncedReason = "EmailBounceReason",
                EmailBouncedDate = new DateTime(2018, 1, 19, 14, 55, 30),
                JigsawContactId = "JigsawContactId",
                Master_Suppressed__c = true
            }
        };

        private List<string> SfContactJsonWithNullValues => new List<string>
        {
            " \"done\": null",
            " \"nextRecordsUrl\": null",
            " \"Id\": null",
            " \"IsDeleted\": false",
            " \"MasterRecordId\": null",
            " \"AccountId\": null",
            " \"Email\": null",
            " \"Fax\": null",
            " \"FirstName\": null",
            " \"Salutation\": null",
            " \"Name\": null",
            " \"OtherCity\": null",
            " \"OtherState\": null",
            " \"OtherCountry\": null",
            " \"OtherPostalCode\": null",
            " \"OtherStreet\": null",
            " \"OtherLatitude\": null",
            " \"OtherLongitude\": null",
            " \"AssistantName\": null",
            " \"AssistantPhone\": null",
            " \"LeadSource\": null",
            " \"BirthDate\": null",
            " \"Description\": null",
            " \"OwnerId\": null",
            " \"HomePhone\": null",
            " \"LastName\": null",
            " \"MailingCity\": null",
            " \"MailingState\": null",
            " \"MailingCountry\": null",
            " \"MailingPostalCode\": null",
            " \"MailingStreet\": null",
            " \"MailingLatitude\": null",
            " \"MailingLongitude\": null",
            " \"MobilePhone\": null",
            " \"Phone\": null",
            " \"Title\": null",
            " \"Department\": null",
            " \"HasOptedOutOfEmail\": false",
            " \"DoNotCall\": false",
            " \"CreatedById\": null",
            " \"CreatedDate\": null",
            " \"SystemModstamp\": null",
            " \"LastActivityDate\": null",
            " \"LastModifiedById\": null",
            " \"LastModifiedDate\": null",
            " \"LastCURequestDate\": null",
            " \"LastViewedDate\": null",
            " \"LastReferencedDate\": null",
            " \"EmailBounceReason\": null",
            " \"EmailBounceDate\": null",
            " \"JigsawContactId\": null",
            " \"Master_Suppressed__c\": false"
        };

        private static List<SF_Contact> SfContactListWithNullValues => new List<SF_Contact>
        {
            new SF_Contact
            {
                Id = "null",
                IsDeleted = false,
                MasterRecordId = string.Empty,
                AccountId = string.Empty,
                Email = string.Empty,
                Fax = string.Empty,
                FirstName = string.Empty,
                Salutation = string.Empty,
                Name = string.Empty,
                OtherCity = string.Empty,
                OtherState = string.Empty,
                OtherCountry = string.Empty,
                OtherPostalCode = string.Empty,
                OtherStreet = string.Empty,
                OtherLatitude = 0.0,
                OtherLongitude = 0.0,
                AssistantName = string.Empty,
                AssistantPhone = string.Empty,
                LeadSource = string.Empty,
                BirthDate = new DateTime(1900, 1, 1),
                Description = string.Empty,
                OwnerId = string.Empty,
                HomePhone = string.Empty,
                LastName = string.Empty,
                MailingCity = string.Empty,
                MailingState = string.Empty,
                MailingCountry = string.Empty,
                MailingPostalCode = string.Empty,
                MailingStreet = string.Empty,
                MailingLatitude = 0.0,
                MailingLongitude = 0.0,
                MobilePhone = string.Empty,
                Phone = string.Empty,
                Title = string.Empty,
                Department = string.Empty,
                HasOptedOutOfEmail = false,
                DoNotCall = false,
                CreatedById = string.Empty,
                CreatedDate = new DateTime(1900, 1, 1),
                SystemModstamp = new DateTime(1900, 1, 1),
                LastActivityDate = new DateTime(1900, 1, 1),
                LastModifiedById = string.Empty,
                LastModifiedDate = new DateTime(1900, 1, 1),
                LastCURequestDate = new DateTime(1900, 1, 1),
                LastViewedDate = new DateTime(1900, 1, 1),
                LastReferencedDate = new DateTime(1900, 1, 1),
                EmailBouncedReason = string.Empty,
                EmailBouncedDate = new DateTime(1900, 1, 1),
                JigsawContactId = string.Empty,
                Master_Suppressed__c = false,
            }
        };
    }
}