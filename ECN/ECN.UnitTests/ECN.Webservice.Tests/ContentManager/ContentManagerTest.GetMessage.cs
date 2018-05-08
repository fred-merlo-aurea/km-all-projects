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
        private const string GetMessageResult = "GetMessageResult";
        private const int SampleLayoutId = 7891;

        [Test]
        public void GetMessage_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetMessage(string.Empty, SampleLayoutId);

            // Assert
            AssertResponseFailed(Consts.GetMessageMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetMessage_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetMessageMethodName);

            // Act
            _manager.GetMessage(SampleEcnAccessKey, SampleLayoutId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.GetMessageLogInput, SampleLayoutId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetMessage_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetMessage(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessage_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessage(_contentFacadeMock, GetMessageResult);

            // Act
            var response = _manager.GetMessage(SampleEcnAccessKey, SampleFolderId);

            // Assert
            response.ShouldBe(GetMessageResult);
        }

        [Test]
        public void GetMessage_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessage(_contentFacadeMock, GetMessageResult, ecnException);

            // Act
            _manager.GetMessage(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessage_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessage(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetMessage(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessage_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessage(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetMessage(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessage_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessage(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetMessage(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}