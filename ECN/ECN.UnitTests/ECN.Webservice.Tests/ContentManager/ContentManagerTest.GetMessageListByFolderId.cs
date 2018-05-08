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
        private const string GetMessageListByFolderIdResult = "GetMessageListByFolderIdResult";

        [Test]
        public void GetMessageListByFolderId_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetMessageListByFolderID(string.Empty, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageListByFolderIdMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetMessageListByFolderId_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetMessageListByFolderIdMethodName);

            // Act
            _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.GetByFolderIdLogInput, SampleFolderId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetMessageListByFolderId_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageListByFolderIdMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageListByFolderId_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageListByFolderId(_contentFacadeMock, GetMessageListByFolderIdResult);

            // Act
            var response = _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            response.ShouldBe(GetMessageListByFolderIdResult);
        }

        [Test]
        public void GetMessageListByFolderId_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageListByFolderId(_contentFacadeMock, GetMessageListByFolderIdResult, ecnException);

            // Act
            _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageListByFolderIdMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageListByFolderId_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageListByFolderId(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageListByFolderIdMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageListByFolderId_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageListByFolderId(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageListByFolderIdMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetMessageListByFolderId_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetMessageListByFolderId(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetMessageListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetMessageListByFolderIdMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}