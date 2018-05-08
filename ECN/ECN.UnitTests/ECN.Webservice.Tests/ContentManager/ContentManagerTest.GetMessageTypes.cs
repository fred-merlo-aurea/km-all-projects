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
        private const string GetMessageTypesResult = "GetMessageTypesResult";

        [Test]
        public void GetMessageTypes_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetMessageTypes(string.Empty);

            // Assert
            AssertResponseFailed(Consts.GetMessageTypesMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetMessageTypes_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetMessageTypesMethodName);

            // Act
            _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(Consts.EmptyRootLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetMessageTypes_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetMessageTypesMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageTypes_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageTypes(_contentFacadeMock, GetMessageTypesResult);

            // Act
            var response = _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(GetMessageTypesResult);
        }

        [Test]
        public void GetMessageTypes_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageTypes(_contentFacadeMock, GetMessageTypesResult, ecnException);

            // Act
            _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetMessageTypesMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageTypes_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageTypes(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetMessageTypesMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageTypes_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageTypes(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetMessageTypesMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageTypes_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageTypes(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetMessageTypes(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetMessageTypesMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}