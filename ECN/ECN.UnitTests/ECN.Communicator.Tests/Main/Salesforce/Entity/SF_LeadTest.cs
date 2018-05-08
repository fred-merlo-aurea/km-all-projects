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
using ECN_Framework_Entities.Salesforce.Interfaces;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_Lead"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_LeadTest : SalesForceTestBase
    {
        private const string EmailBouncedDateProperty = "EmailBouncedDate:true";
        private List<SF_Lead> leads;
        private static CultureInfo previousCulture;

        /// <summary>
        ///     Setup up <see cref="leads"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => leads = new List<SF_Lead>
        {
            new SF_Lead(),
            new SF_Lead(),
            new SF_Lead()
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
            leads.ForEach(x => x.SetProperty(sortBy, leads.IndexOf(x)));
            var expected = leads.OrderBy(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            var result = SF_Lead.Sort(leads, sortBy, System.Web.UI.WebControls.SortDirection.Ascending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Sort_SortByPropertyInDescendingOrder_ReturnSortedList(
            [ValueSource(nameof(SortProperties))] string sortBy)
        {
            // Arrange
            leads.ForEach(x => x.SetProperty(sortBy, leads.IndexOf(x)));
            var expected = leads.OrderByDescending(x => x.GetPropertyValue(sortBy)).ToList();

            // Act
            var result = SF_Lead.Sort(leads, sortBy, System.Web.UI.WebControls.SortDirection.Descending);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        private static readonly string[] SortProperties =
        {
            "Email",
            "FirstName",
            "LastName",
            "Company",
            "State",
            "Title",
        };

        [Test]
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            var exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { null }));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyLeadList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (List<SF_Lead>)typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { json });

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyLeadList()
        {
            // Arrange
            var json = SfLeadJsonWithNonNullValues;
            var expectedList = SfLeadListWithNonNullValues;

            // Act
            var result =
                (List<SF_Lead>)typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { json });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyLeadList()
        {
            // Arrange
            var json = SfLeadJsonWithNullValues;

            var expectedList = SfLeadListWithNullValues;

            // Act
            var result =
                (List<SF_Lead>)typeof(SF_Lead).CallMethod("ConvertJsonList", new object[] { json });

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonItem_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            var exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_Lead).CallMethod("ConvertJsonItem", new object[] { null }));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonItem_PassEmptyStringList_ReturnEmptyLead()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (SF_Lead)typeof(SF_Lead).CallMethod("ConvertJsonItem", new object[] { json });

            // Assert
            Assert.True(new SF_Lead().IsContentMatched(result));
        }

        [Test]
        public void ConvertJsonItem_SetupJsonWithNonNullValues_VerifyLead()
        {
            // Arrange
            var json = SfLeadJsonWithNonNullValues;

            var expected = SfLeadListWithNonNullValues.First();

            // Act
            var result = (SF_Lead)typeof(SF_Lead).CallMethod("ConvertJsonItem", new object[] { json });

            // Assert
            Assert.True(expected.IsContentMatched(result));
        }

        [Test]
        public void ConvertJsonItem_SetupJsonWithNullValues_VerifyLead()
        {
            // Arrange
            var json = SfLeadJsonWithNullValues;

            var expected = SfLeadListWithNullValues.First();

            // Act
            var result = (SF_Lead)typeof(SF_Lead).CallMethod("ConvertJsonItem", new object[] { json });

            // Assert
            Assert.True(expected.IsContentMatched(result));
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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetCampaignMembers(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetListForMS_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasEmailBouncedDate_ReturnsListWithTwoItems()
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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetByTagList(string.Empty, new List<SF_LeadTag>());

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetByTagList(string.Empty, new List<SF_LeadTag>());

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, It.IsAny<string>()), Times.Once());
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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_Lead.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void Update_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            var token = "accessToken";
            var leadId = "Id";
            var lead = new SF_Lead() { Id = leadId };
            var sfUtilities = new Mock<ISFUtilities>();
            sfUtilities.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<ECN_Framework_Entities.Salesforce.SalesForceBase>(), It.IsAny<SFObject>(), It.IsAny<string>())).Returns(true);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Lead.Update(token, lead);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Update(
                It.Is<string>(t => t == token),
                It.Is<ECN_Framework_Entities.Salesforce.SalesForceBase>(sf => sf == lead),
                It.Is<SFObject>(sf => sf == SFObject.Lead),
                It.Is<string>(id => id == leadId)),
                Times.Once);
        }

        [Test]
        public void GetSingle_PassCorrectParams_ReturnsNotNullEntity()
        {
            // Arrange
            const string whereParam = "where";
            const string token = "accessToken";
            const string leadId = "Id";
            var expected = new SF_Lead() { Id = leadId };
            var sfUtilities = new Mock<ECN_Framework_Entities.Salesforce.Interfaces.ISFUtilities>();
            sfUtilities.Setup(x => x.GetSingle<SF_Lead>(token, whereParam, It.IsAny<LeadConverter>())).Returns(expected);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Lead.GetSingle(token, whereParam);

            // Assert
            result.ShouldBe(expected);
            sfUtilities.Verify(
                x => x.GetSingle<SF_Lead>(
                    It.Is<string>(t => t == token),
                    It.Is<string>(id => id == whereParam),
                    It.IsAny<LeadConverter>()),
                Times.Once);
        }

        [Test]
        public void Insert_PassCorrectPameters_ReturnTrue()
        {
            // Arrange
            var token = "accessToken";
            var lead = new SF_Lead();
            var sfUtilities = new Mock<ISFUtilities>();
            sfUtilities.Setup(x => x.Insert(token, lead, SFObject.Lead)).Returns(true);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var result = SF_Lead.Insert(token, lead);

            // Assert
            result.ShouldBeTrue();
            sfUtilities.Verify(
                x => x.Insert(
                    It.Is<string>(t => t == token),
                    It.Is<ECN_Framework_Entities.Salesforce.SalesForceBase>(sf => sf == lead),
                    It.Is<SFObject>(sf => sf == SFObject.Lead)),
                Times.Once);
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