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
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_Contact"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_ContactTest : SalesForceTestBase
    {
        private const string MasterSuppressedCProperty = "Master_Suppressed__c:true";
        private List<SF_Contact> contacts;
        private static CultureInfo previousCulture;

        /// <summary>
        ///     Setup up <see cref="contacts"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => contacts = new List<SF_Contact>
        {
            new SF_Contact(),
            new SF_Contact(),
            new SF_Contact()
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
            contacts.ForEach(x => x.SetProperty(sortBy, contacts.IndexOf(x)));
            var expected = contacts.OrderBy(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            var result = SF_Contact.Sort(contacts, sortBy, System.Web.UI.WebControls.SortDirection.Ascending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Sort_SortByPropertyInDescendingOrder_ReturnSortedList(
            [ValueSource(nameof(SortProperties))] string sortBy)
        {
            // Arrange
            contacts.ForEach(x => x.SetProperty(sortBy, contacts.IndexOf(x)));
            var expected = contacts.OrderByDescending(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            var result = SF_Contact.Sort(contacts, sortBy, System.Web.UI.WebControls.SortDirection.Descending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test, TestCaseSource(nameof(GetNonSortProperties))]
        public void Sort_SortByNonSortPropertyInAscendingOrder_ReturnOriginalList(string sortBy)
        {
            // Arrange
            contacts.ForEach(x => x.SetProperty(sortBy, contacts.IndexOf(x)));

            // Act
            var result = SF_Contact.Sort(contacts, sortBy, System.Web.UI.WebControls.SortDirection.Ascending);

            // Assert
            CollectionAssert.AreEqual(contacts, result);
        }

        [Test, TestCaseSource(nameof(GetNonSortProperties))]
        public void Sort_SortByNonSortPropertyInDescendingOrder_ReturnOriginalList(string sortBy)
        {
            // Arrange
            contacts.ForEach(x => x.SetProperty(sortBy, contacts.IndexOf(x)));

            // Act
            var result = SF_Contact.Sort(contacts, sortBy, System.Web.UI.WebControls.SortDirection.Descending);

            // Assert
            CollectionAssert.AreEqual(contacts, result);
        }

        private static readonly string[] SortProperties =
        {
            "Id",
            "IsDeleted",
            "MasterRecordId",
            "AccountId",
            "Email",
            "Fax",
            "FirstName",
            "Salutation",
            "Name",
            "OtherCity",
            "OtherStreet",
            "OtherState",
            "OtherPostalCode",
            "OtherCountry",
            "OtherLatitude",
            "OtherLongitude",
            "OtherPhone",
            "AssistantPhone",
            "AssistantName",
            "LeadSource",
            "BirthDate",
            "Description",
            "HomePhone",
            "LastName",
            "MailingCity",
            "MailingState",
            "MailingCountry",
            "MailingPostalCode",
            "MailingStreet",
            "MailingLatitude",
            "MailingLongitude",
            "MobilePhone",
            "Phone",
            "Title",
            "Department",
            "HasOptedOutOfEmail",
            "DoNotCall",
            "CreatedDate",
            "CreatedById",
            "LastModifiedDate",
            "LastModifiedById",
            "SystemModstamp",
            "LastActivityDate",
            "LastCURequestDate",
            "LastViewedDate",
            "LastReferencedDate",
            "EmailBouncedReason",
            "EmailBouncedDate",
            "JigsawContactId"
        };

        private static string[] GetNonSortProperties()
        {
            return typeof(SF_Contact).GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !SortProperties.Contains(x.Name) && x.Name != "OwnerId")
                .Select(x => x.Name).ToArray();
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
            var result =
                (List<SF_Contact>)typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { json });

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
            var result =
                (List<SF_Contact>)typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { json });

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
            var result =
                (List<SF_Contact>)typeof(SF_Contact).CallMethod("ConvertJsonList", new object[] { json });

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
            var sfUtilities = CreateSFUtilities(MasterSuppressedCProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Contact.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void Update_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            var token = "accessToken";
            var contactId = "Id";
            var contact = new SF_Contact() { Id = contactId };
            var sfUtilities = new Mock<ECN_Framework_Entities.Salesforce.Interfaces.ISFUtilities>();
            sfUtilities.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<ECN_Framework_Entities.Salesforce.SalesForceBase>(), It.IsAny<SFObject>(), It.IsAny<string>())).Returns(true);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Contact.Update(token, contact);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Update(
                It.Is<string>(t => t == token),
                It.Is<ECN_Framework_Entities.Salesforce.SalesForceBase>(sf => sf == contact),
                It.Is<SFObject>(sf => sf == SFObject.Contact),
                It.Is<string>(id => id == contactId)),
                Times.Once);
        }

        [Test]
        public void GetSingle_PassCorrectParams_ReturnsNotNullEntity()
        {
            // Arrange
            const string whereParam = "where";
            const string token = "accessToken";
            const string contactId = "Id";
            var expectedContact = new SF_Contact() { Id = contactId };
            var sfUtilities = new Mock<ECN_Framework_Entities.Salesforce.Interfaces.ISFUtilities>();
            sfUtilities.Setup(x => x.GetSingle<SF_Contact>(token, whereParam, It.IsAny<ContactConverter>())).Returns(expectedContact);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Contact.GetSingle(token, whereParam);

            // Assert
            result.ShouldBe(expectedContact);
            sfUtilities.Verify(
                x => x.GetSingle<SF_Contact>(
                    It.Is<string>(t => t == token),
                    It.Is<string>(id => id == whereParam),
                    It.IsAny<ContactConverter>()),
                Times.Once);
        }

        [Test]
        public void Insert_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            var token = "accessToken";
            var contactId = "Id";
            var contact = new SF_Contact() { Id = contactId };
            var sfUtilities = new Mock<ECN_Framework_Entities.Salesforce.Interfaces.ISFUtilities>();
            sfUtilities.Setup(x => x.Insert(token, contact, SFObject.Contact)).Returns(true);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Contact.Insert(token, contact);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Insert(
                    It.Is<string>(t => t == token),
                    It.Is<ECN_Framework_Entities.Salesforce.SalesForceBase>(sf => sf == contact),
                    It.Is<SFObject>(sf => sf == SFObject.Contact)),
                Times.Once);
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