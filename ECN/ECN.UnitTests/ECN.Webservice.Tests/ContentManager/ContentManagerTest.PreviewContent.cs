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
        private const string PreviewContentResult = "PreviewContentResult";

        [Test]
        public void PreviewContent_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.PreviewContent(string.Empty, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.PreviewContentMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void PreviewContent_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.PreviewContentMethodName);

            // Act
            _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.PreviewContentLogInput, SampleContentId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void PreviewContent_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.PreviewContentMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewContent_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewContent(_contentFacadeMock, PreviewContentResult);

            // Act
            var response = _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            response.ShouldBe(PreviewContentResult);
        }

        [Test]
        public void PreviewContent_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewContent(_contentFacadeMock, PreviewContentResult, ecnException);

            // Act
            _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.PreviewContentMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewContent_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewContent(_contentFacadeMock, null, securityException);

            // Act
            _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.PreviewContentMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewContent_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewContent(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.PreviewContentMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void PreviewContent_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockPreviewContent(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.PreviewContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.PreviewContentMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}