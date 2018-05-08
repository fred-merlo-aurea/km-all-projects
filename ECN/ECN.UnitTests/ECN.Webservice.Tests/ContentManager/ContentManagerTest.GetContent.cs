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
        private const string GetContentResult = "GetContentResult";

        [Test]
        public void GetContent_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetContent(string.Empty, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.GetContentMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetContent_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetContentMethodName);

            // Act
            _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.GetContentLogInput, SampleContentId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetContent_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.GetContentMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetContent_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContent(_contentFacadeMock, GetContentResult);

            // Act
            var response = _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            response.ShouldBe(GetContentResult);
        }

        [Test]
        public void GetContent_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContent(_contentFacadeMock, GetContentResult, ecnException);

            // Act
            _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.GetContentMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetContent_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContent(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.GetContentMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetContent_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContent(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.GetContentMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetContent_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContent(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetContent(SampleEcnAccessKey, SampleContentId);

            // Assert
            AssertResponseFailed(Consts.GetContentMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}