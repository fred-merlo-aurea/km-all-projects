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
        private const string AddContentResult = "AddContentResult";

        [Test]
        public void AddContent_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            ExecuteAddContent(string.Empty);

            // Assert
            AssertResponseFailed(Consts.AddContentMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void AddContent_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.AddContentMethodName);
            var expectedLogInput = string.Format(
                Consts.AddContentLogInput,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleFolderId,
                true
            );

            // Act
            ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void AddContent_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddContentMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddContent_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockAddContent(_contentFacadeMock, AddContentResult);

            // Act
            var response = ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(AddContentResult);
        }

        [Test]
        public void AddContent_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockAddContent(_contentFacadeMock, AddContentResult, ecnException);

            // Act
            ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddContentMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void AddContent_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockAddContent(_contentFacadeMock, null, securityException);

            // Act
            ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddContentMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddContent_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockAddContent(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddContentMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddContent_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockAddContent(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteAddContent(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddContentMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteAddContent(string accessKey)
        {
            return _manager.AddContentWithPreference(
                accessKey,
                SampleTitle,
                SampleContentHtml,
                SampleContentText,
                SampleFolderId,
                true);
        }
    }
}