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
        private const string GetFoldersResult = "GetFoldersResult";

        [Test]
        public void GetFolders_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetFolders(string.Empty);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetFolders_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetFoldersMethodName);

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(Consts.EmptyRootLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetFolders_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetFolders(_contentFacadeMock, GetFoldersResult);

            // Act
            var response = _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(GetFoldersResult);
        }

        [Test]
        public void GetFolders_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetFolders(_contentFacadeMock, GetFoldersResult, ecnException);

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetFolders(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetFolders(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetFolders(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}