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
        private const string DeleteFolderResult = "DeleteFolderResult";

        [Test]
        public void DeleteFolder_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.DeleteFolder(string.Empty, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.DeleteFolderMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void DeleteFolder_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.DeleteFolderMethodName);

            // Act
            _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.FolderIdLogInput, SampleFolderId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void DeleteFolder_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.DeleteFolderMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteFolder_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteFolder(_contentFacadeMock, DeleteFolderResult);

            // Act
            var response = _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            response.ShouldBe(DeleteFolderResult);
        }

        [Test]
        public void DeleteFolder_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteFolder(_contentFacadeMock, DeleteFolderResult, ecnException);

            // Act
            _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.DeleteFolderMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteFolder_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteFolder(_contentFacadeMock, null, securityException);

            // Act
            _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.DeleteFolderMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteFolder_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteFolder(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.DeleteFolderMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteFolder_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockDeleteFolder(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.DeleteFolder(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.DeleteFolderMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}