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
        private const string GetContentListByFolderIdResult = "GetContentListByFolderIdResult";

        [Test]
        public void GetContentListByFolderId_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetContentListByFolderID(string.Empty, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetContentListByFolderIdMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetContentListByFolderId_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetContentListByFolderIdMethodName);

            // Act
            _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.GetByFolderIdLogInput, SampleFolderId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetContentListByFolderId_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetContentListByFolderIdMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetContentListByFolderId_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContentListByFolderId(_contentFacadeMock, GetContentListByFolderIdResult);

            // Act
            var response = _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            response.ShouldBe(GetContentListByFolderIdResult);
        }

        [Test]
        public void GetContentListByFolderId_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContentListByFolderId(_contentFacadeMock, GetContentListByFolderIdResult, ecnException);

            // Act
            _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetContentListByFolderIdMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetContentListByFolderId_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContentListByFolderId(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetContentListByFolderIdMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetContentListByFolderId_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContentListByFolderId(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetContentListByFolderIdMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetContentListByFolderId_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetContentListByFolderId(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetContentListByFolderID(SampleEcnAccessKey, SampleFolderId);

            // Assert
            AssertResponseFailed(Consts.GetContentListByFolderIdMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}