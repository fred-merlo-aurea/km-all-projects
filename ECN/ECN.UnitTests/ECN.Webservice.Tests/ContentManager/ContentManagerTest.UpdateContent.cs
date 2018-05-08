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
        private const string UpdateContentResult = "UpdateContentResult";
        private const string SampleTitle = "SampleTitle";
        private const string SampleContentText = "SampleContentText";
        private const string SampleContentHtml = "SampleContentHtml";

        [Test]
        public void UpdateContent_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.UpdateContent(string.Empty, SampleTitle, SampleContentHtml, SampleContentText, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.UpdateContentMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void UpdateContent_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.UpdateContentMethodName);
            var expectedLogInput = string.Format(
                Consts.UpdateContentLogInput,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Act
            _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void UpdateContent_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            AssertResponseFailed(Consts.UpdateContentMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateContent_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateContent(_contentFacadeMock, UpdateContentResult);

            // Act
            var response = _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            response.ShouldBe(UpdateContentResult);
        }

        [Test]
        public void UpdateContent_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateContent(_contentFacadeMock, UpdateContentResult, ecnException);

            // Act
            _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            AssertResponseFailed(Consts.UpdateContentMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateContent_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateContent(_contentFacadeMock, null, securityException);

            // Act
            _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            AssertResponseFailed(Consts.UpdateContentMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateContent_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateContent(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            AssertResponseFailed(Consts.UpdateContentMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateContent_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateContent(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.UpdateContent(
                SampleEcnAccessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleContentId);

            // Assert
            AssertResponseFailed(Consts.UpdateContentMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}