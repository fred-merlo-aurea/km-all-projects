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
        private const string UpdateMessageResult = "UpdateMessageResult";
        private const string SampleLayoutName = "SampleLayoutName";
        private const string SampleTableBorder = "SampleTableBorder";
        private const int SampleTemplateId = 880077;
        private const string SampleAddress = "SampleAddress";
        private const int SampleDeptId = 111;
        private const int SampleContent0 = 0000;
        private const int SampleContent1 = 1111;
        private const int SampleContent2 = 2222;
        private const int SampleContent3 = 3333;
        private const int SampleContent4 = 4444;
        private const int SampleContent5 = 5555;
        private const int SampleContent6 = 6666;
        private const int SampleContent7 = 7777;
        private const int SampleContent8 = 8888;

        [Test]
        public void UpdateMessage_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            ExecuteUpdateMessage(string.Empty);

            // Assert
            AssertResponseFailed(Consts.UpdateMessageMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void UpdateMessage_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.UpdateMessageMethodName);
            var expectedLogInput = string.Format(
                Consts.UpdateMessageLogInput,
                SampleLayoutName,
                SampleTableBorder,
                SampleTemplateId,
                SampleAddress,
                SampleDeptId,
                SampleContent0,
                SampleContent1,
                SampleContent2,
                SampleContent3,
                SampleContent4,
                SampleContent5,
                SampleContent6,
                SampleContent7,
                SampleContent8,
                SampleMessageId
            );

            // Act
            ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void UpdateMessage_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.UpdateMessageMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateMessage_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateMessage(_contentFacadeMock, UpdateMessageResult);

            // Act
            var response = ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(UpdateMessageResult);
        }

        [Test]
        public void UpdateMessage_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateMessage(_contentFacadeMock, UpdateMessageResult, ecnException);

            // Act
            ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.UpdateMessageMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateMessage_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateMessage(_contentFacadeMock, null, securityException);

            // Act
            ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.UpdateMessageMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateMessage_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateMessage(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.UpdateMessageMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateMessage_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockUpdateMessage(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteUpdateMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.UpdateMessageMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteUpdateMessage(string accessKey)
        {
            return _manager.UpdateMessage(
                accessKey,
                SampleLayoutName,
                SampleTableBorder,
                SampleTemplateId,
                SampleAddress,
                SampleDeptId,
                SampleContent0,
                SampleContent1,
                SampleContent2,
                SampleContent3,
                SampleContent4,
                SampleContent5,
                SampleContent6,
                SampleContent7,
                SampleContent8,
                SampleMessageId);
        }
    }
}