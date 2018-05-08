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
        private const string PreviewMessageResult = "PreviewMessageResult";
        private const int SampleMessageId = 44332211;

        [Test]
        public void PreviewMessage_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.PreviewMessage(string.Empty, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.PreviewMessageMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void PreviewMessage_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.PreviewMessageMethodName);

            // Act
            _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.PreviewMessageLogInput, SampleMessageId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void PreviewMessage_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.PreviewMessageMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewMessage_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewMessage(_contentFacadeMock, PreviewMessageResult);

            // Act
            var response = _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            response.ShouldBe(PreviewMessageResult);
        }

        [Test]
        public void PreviewMessage_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewMessage(_contentFacadeMock, PreviewMessageResult, ecnException);

            // Act
            _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.PreviewMessageMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewMessage_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewMessage(_contentFacadeMock, null, securityException);

            // Act
            _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.PreviewMessageMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewMessage_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewMessage(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.PreviewMessageMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewMessage_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewMessage(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.PreviewMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.PreviewMessageMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}