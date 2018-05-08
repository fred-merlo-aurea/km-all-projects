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
        private const string DeleteMessageResult = "DeleteMessageResult";

        [Test]
        public void DeleteMessage_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.DeleteMessage(string.Empty, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.DeleteMessageMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void DeleteMessage_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.DeleteMessageMethodName);

            // Act
            _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.MessageIdLogInput, SampleMessageId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void DeleteMessage_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.DeleteMessageMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteMessage_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteMessage(_contentFacadeMock, DeleteMessageResult);

            // Act
            var response = _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            response.ShouldBe(DeleteMessageResult);
        }

        [Test]
        public void DeleteMessage_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteMessage(_contentFacadeMock, DeleteMessageResult, ecnException);

            // Act
            _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.DeleteMessageMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteMessage_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteMessage(_contentFacadeMock, null, securityException);

            // Act
            _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.DeleteMessageMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteMessage_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteMessage(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.DeleteMessageMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteMessage_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteMessage(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.DeleteMessage(SampleEcnAccessKey, SampleMessageId);

            // Assert
            AssertResponseFailed(Consts.DeleteMessageMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}