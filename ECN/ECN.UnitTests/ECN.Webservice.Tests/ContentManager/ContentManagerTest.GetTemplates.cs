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
        private const string GetTemplatesResult = "GetTemplatesResult";

        [Test]
        public void GetTemplates_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetTemplates(string.Empty);

            // Assert
            AssertResponseFailed(Consts.GetTemplatesMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetTemplates_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetTemplatesMethodName);

            // Act
            _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(Consts.EmptyRootLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetTemplates_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetTemplatesMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetTemplates_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetTemplates(_contentFacadeMock, GetTemplatesResult);

            // Act
            var response = _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(GetTemplatesResult);
        }

        [Test]
        public void GetTemplates_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetTemplates(_contentFacadeMock, GetTemplatesResult, ecnException);

            // Act
            _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetTemplatesMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetTemplates_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetTemplates(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetTemplatesMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetTemplates_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetTemplates(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetTemplatesMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetTemplates_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetTemplates(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetTemplates(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetTemplatesMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}