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
        private const string SearchForMessagesResult = "SearchForMessagesExpectedResponseOutput";

        [Test]
        public void SearchForMessages_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.SearchForMessages(string.Empty, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForMessagesMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void SearchForMessages_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.SearchForMessagesMethodName);

            // Act
            _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.SearchLogInput, SampleXmlSearch)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void SearchForMessages_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForMessagesMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForMessages_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForMessages(_contentFacadeMock, SearchForMessagesResult);

            // Act
            var response = _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            response.ShouldBe(SearchForMessagesResult);
        }

        [Test]
        public void SearchForMessages_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForMessages(_contentFacadeMock, null, ecnException);

            // Act
            _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForMessagesMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForMessages_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForMessages(_contentFacadeMock, null, securityException);

            // Act
            _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForMessagesMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForMessages_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForMessages(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForMessagesMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForMessages_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockSearchForMessages(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.SearchForMessages(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForMessagesMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}