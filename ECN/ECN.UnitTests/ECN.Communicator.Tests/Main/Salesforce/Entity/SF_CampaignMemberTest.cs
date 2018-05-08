using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using ecn.communicator.main.Salesforce.Entity;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_Entities.Salesforce.Interfaces;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN.Communicator.Tests.Main.Salesforce.Entity
{
    /// <summary>
    ///     Unit test for <see cref="SF_Campaign"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SF_CampaignMemberTest : SalesForceTestBase
    {
        private const string LastModifiedDateProperty = "LastModifiedDate:1:1:1900";
        private List<SF_CampaignMember> campaignMembers;
        private static CultureInfo previousCulture;

        /// <summary>
        ///     Setup up <see cref="campaignMembers"/> which can be utilized in test scope
        /// </summary>
        [SetUp]
        public void SetUp() => campaignMembers = new List<SF_CampaignMember>
        {
            new SF_CampaignMember(),
            new SF_CampaignMember(),
            new SF_CampaignMember()
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
        public void ConvertJsonList_PassNullStringList_ThrowNullReferenceException()
        {
            // Act
            TargetInvocationException exp = Assert.Throws<TargetInvocationException>(() =>
                typeof(SF_CampaignMember).CallMethod("ConvertJsonList", new object[] {null}));

            // Assert
            Assert.That(exp.InnerException, Is.TypeOf<NullReferenceException>());
        }

        [Test]
        public void ConvertJsonList_PassEmptyStringList_ReturnEmptyCampaignMemberList()
        {
            // Arrange
            var json = new List<string>();

            // Act
            var result =
                (List<SF_CampaignMember>) typeof(SF_CampaignMember).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNonNullValues_VerifyCampaignMemberList()
        {
            // Arrange
            List<string> json = SfCampaignJsonWithNonNullValues;
            List<SF_CampaignMember> expectedList = SfCampaignMemberListWithNonNullValues;

            // Act
            var result =
                (List<SF_CampaignMember>) typeof(SF_CampaignMember).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void ConvertJsonList_SetupJsonWithNullValues_VerifyCampaignMemberList()
        {
            // Arrange
            dateTimeForJsonNullValue = "01/01/1900 00:00:00";
            List<string> json = SfCampaignJsonWithNullValues;

            List<SF_CampaignMember> expectedList = SfCampaignMemberListWithNullValues;

            // Act
            var result =
                (List<SF_CampaignMember>) typeof(SF_CampaignMember).CallMethod("ConvertJsonList", new object[] {json});

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.True(expectedList.IsListContentMatched(result));
        }

        [Test]
        public void GetList_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasLastModifiedDate_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(LastModifiedDateProperty);
            var webRequest = CreateWebRequest(webResponse);
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(LastModifiedDateProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_CampaignMember.GetList(string.Empty, string.Empty);

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
            Mock<ISFUtilities> sfUtilities = CreateSFUtilities(LastModifiedDateProperty);
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
            var contactsList = SF_CampaignMember.GetList(string.Empty, string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        [Test]
        public void GetAll_TwoResponsesOneWithFalseDoneAndOneWithTrueDoneBothHasLastModifiedDate_ReturnsListWithTwoItems()
        {
            // Arrange
            var webResponse = CreateWebResponse(LastModifiedDateProperty);
            var webRequest = CreateWebRequest(webResponse);
            var sfUtilities = CreateSFUtilities(LastModifiedDateProperty);
            sfUtilities.Setup(x => x.CreateQueryRequest(
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<Method>(),
                            It.IsAny<ResponseType>())).Returns(webRequest.Object);
            ECN_Framework_Entities.Salesforce.SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_CampaignMember.GetAll(string.Empty);

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
            var sfUtilities = CreateSFUtilities(LastModifiedDateProperty);
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
            var contactsList = SF_CampaignMember.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }

        private static List<string> SfCampaignJsonWithNonNullValues => new List<string>
        {
            " \"Id\": \"Id\" ,",
            " \"CampaignId\": \"CampaignId\" ,",
            " \"ContactId\": \"ContactId\" ,",
            " \"CreatedById\": \"CreatedById\" ,",
            " \"CreatedDate\": \"01/26/2018 15:52:31.000+0000\" ,",
            " \"IsDeleted\": \"true\" ,",
            " \"FirstRespondedDate\": \"01/22/2018 14:55:30.000+0000\" ,",
            " \"LastModifiedById\": \"LastModifiedById\" ,",
            " \"LeadId\": \"LeadId\" ,",
            " \"HasResponded\": \"true\" ,",
            " \"Status\": \"Status\" ,",
            " \"SystemModstamp\": \"01/24/2018 12:51:30.000+0000\" ,",
            " \"LastModifiedDate\": \"01/18/2018 14:55:30.000+0000\" ,"
        };

        private static List<SF_CampaignMember> SfCampaignMemberListWithNonNullValues => new List<SF_CampaignMember>
        {
            new SF_CampaignMember
            {
                Id = "Id",
                CampaignId = "CampaignId",
                ContactId = "ContactId",
                CreatedById = "CreatedById",
                CreatedDate = new DateTime(2018, 1, 26, 15, 52, 31),
                IsDeleted = true,
                FirstRespondedDate = new DateTime(2018, 1, 22, 14, 55, 30),
                LastModifiedById = "LastModifiedById",
                LastModifiedDate = new DateTime(2018, 1, 18, 14, 55, 30),
                Status = "Status",
                SystemModstamp = new DateTime(2018, 1, 24, 12, 51, 30),
                LeadId = "LeadId",
                HasResponded = true
            }
        };

        private string dateTimeForJsonNullValue;

        private List<string> SfCampaignJsonWithNullValues => new List<string>
        {
            " \"Id\": null",
            " \"CampaignId\": null",
            " \"ContactId\": null",
            " \"CreatedById\": null",
            $" \"CreatedDate\": {dateTimeForJsonNullValue}",
            " \"IsDeleted\": false",
            " \"FirstRespondedDate\": null",
            " \"LastModifiedById\": null",
            " \"LeadId\": null",
            " \"HasResponded\": false",
            " \"Status\": null",
            " \"SystemModstamp\": null",
            $" \"LastModifiedDate\": {dateTimeForJsonNullValue}"
        };

        private static List<SF_CampaignMember> SfCampaignMemberListWithNullValues => new List<SF_CampaignMember>
        {
            new SF_CampaignMember
            {
                Id = "null",
                CampaignId = string.Empty,
                ContactId = string.Empty,
                CreatedById = string.Empty,
                CreatedDate = new DateTime(1900, 1, 1),
                IsDeleted = false,
                FirstRespondedDate = new DateTime(1900, 1, 1),
                LastModifiedById = string.Empty,
                LastModifiedDate = new DateTime(1900, 1, 1),
                LeadId = "null",
                HasResponded = false,
                Status = "null",
                SystemModstamp = new DateTime(1900, 1, 1)
            }
        };
    }
}