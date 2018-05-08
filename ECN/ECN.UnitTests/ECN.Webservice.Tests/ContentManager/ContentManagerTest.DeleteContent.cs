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
        private const string DeleteContentResult = "DeleteContentResult";

        [Test]
        public void DeleteContent_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.DeleteContent(string.Empty, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.DeleteContentMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void DeleteContent_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.DeleteContentMethodName);

            // Act
            _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.ContentIdLogInput, SampleContentId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void DeleteContent_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.DeleteContentMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteContent_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteContent(_contentFacadeMock, DeleteContentResult);

            // Act
            var response = _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            response.ShouldBe(DeleteContentResult);
        }

        [Test]
        public void DeleteContent_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteContent(_contentFacadeMock, DeleteContentResult, ecnException);

            // Act
            _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.DeleteContentMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteContent_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteContent(_contentFacadeMock, null, securityException);

            // Act
            _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.DeleteContentMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteContent_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteContent(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.DeleteContentMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteContent_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteContent(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.DeleteContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.DeleteContentMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}