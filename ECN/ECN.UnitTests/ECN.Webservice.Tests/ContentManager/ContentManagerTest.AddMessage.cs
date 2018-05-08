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
        private const string AddMessageResult = "AddMessageResult";
        private const int SampleMessageTypeId = 999;
        
        [Test]
        public void AddMessage_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            ExecuteAddMessage(string.Empty);

            // Assert
            AssertResponseFailed(Consts.AddMessageMainMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void AddMessage_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.AddMessageMainMethodName);
            var expectedLogInput = string.Format(
                Consts.AddMessageLogInput,
                SampleLayoutName,
                SampleFolderId,
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
                SampleContent8
            );

            // Act
            ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void AddMessage_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddMessageMainMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddMessage_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockAddMessage(_contentFacadeMock, AddMessageResult);

            // Act
            var response = ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(AddMessageResult);
        }

        [Test]
        public void AddMessage_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockAddMessage(_contentFacadeMock, AddMessageResult, ecnException);

            // Act
            ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddMessageMainMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void AddMessage_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockAddMessage(_contentFacadeMock, null, securityException);

            // Act
            ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddMessageMainMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddMessage_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockAddMessage(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddMessageMainMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddMessage_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockAddMessage(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteAddMessage(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.AddMessageMainMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteAddMessage(string accessKey)
        {
            return _manager.AddMessageWithType(
                accessKey,
                SampleLayoutName,
                SampleMessageTypeId,
                SampleFolderId,
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
                SampleContent8);
        }
    }
}