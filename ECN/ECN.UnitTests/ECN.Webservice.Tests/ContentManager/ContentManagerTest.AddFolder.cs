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
        private const string AddFolderResult = "AddFolderResult";
        private const string SampleFolderName = "SampleFolderName";
        private const string SampleFolderDescription = "SampleFolderDescription";
        private const int SampleParentFolderId = 778;

        [Test]
        public void AddFolder_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            ExecuteAddFolder(string.Empty);

            // Assert
            AssertResponseFailed(Consts.AddFolderMainMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void AddFolder_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.AddFolderMainMethodName);
            var expectedLogInput = string.Format
                (Consts.AddFolderLogInput,
                SampleFolderName,
                SampleFolderDescription,
                SampleParentFolderId);

            // Act
            ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void AddFolder_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddFolderMainMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddFolder_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockAddFolder(_contentFacadeMock, AddFolderResult);

            // Act
            var response = ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(AddFolderResult);
        }

        [Test]
        public void AddFolder_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockAddFolder(_contentFacadeMock, AddFolderResult, ecnException);

            // Act
            ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddFolderMainMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void AddFolder_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockAddFolder(_contentFacadeMock, null, securityException);

            // Act
            ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddFolderMainMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddFolder_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockAddFolder(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddFolderMainMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddFolder_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockAddFolder(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteAddFolder(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddFolderMainMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteAddFolder(string accessKey)
        {
            return _manager.AddFolder(
                accessKey,
                SampleFolderName,
                SampleFolderDescription,
                SampleParentFolderId);
        }
    }
}