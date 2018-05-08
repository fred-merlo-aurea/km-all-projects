using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using ecn.webservice;
using ECN_Framework_Common.Objects;
using APILogging = ECN_Framework_Entities.Communicator.APILogging;

namespace ECN.Webservice.Tests.ContentManager
{
    [TestFixture]
    public partial class ContentManagerTest
    {
        private const string SearchForContentResult = "SearchForContentExpectedResponseOutput";

        [Test]
        public void SearchForContent_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.SearchForContent(string.Empty, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForContentMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void SearchForContent_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.SearchForContentMethodName);

            // Act
            _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.SearchLogInput, SampleXmlSearch)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void SearchForContent_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForContentMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForContent_ContentRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForContent(_contentFacadeMock, SearchForContentResult);

            // Act
            var response = _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            response.ShouldBe(SearchForContentResult);
        }

        [Test]
        public void SearchForContent_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForContent(_contentFacadeMock, null, ecnException);

            // Act
            _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForContentMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForContent_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForContent(_contentFacadeMock, null, securityException);

            // Act
            _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForContentMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForContent_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForContent(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForContentMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForContent_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForContent(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.SearchForContent(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForContentMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}