using System.Net;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Interfaces;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN_Framework_EntitiesTests.Salesforce.Tests
{
    [TestFixture]
    public class SF_CampaignMemberTest : SalesForceTestBase
    {
        private const string LastModifiedDateProperty = "LastModifiedDate:1:1:1900";

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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

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
            SalesForceBase.InitializeSFUtilities(sfUtilities.Object);

            // Act
            var contactsList = SF_CampaignMember.GetAll(string.Empty);

            // Assert
            contactsList.ShouldBeEmpty();
            sfUtilities.Verify(x => x.LogWebException(webException, queryParameter), Times.Once());
        }
    }
}
