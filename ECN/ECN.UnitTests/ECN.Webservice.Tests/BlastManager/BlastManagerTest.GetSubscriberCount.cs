using System;
using System.Data;
using ecn.webservice;
using ECN_Framework_Common.Objects;
using Moq;
using NUnit.Framework;
using Shouldly;
using APILogging = ECN_Framework_Entities.Communicator.APILogging;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class BlastManagerTest
    {
        private const int SampleGroupId = 4444;

        [Test]
        public void GetSubscriberCount_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetSubscriberCount(string.Empty, SampleGroupId);

            // Assert
            AssertResponseFailed(Consts.GetSubscriberCountMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetSubscriberCount_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.BlastManagerServiceMethodName, 
                Consts.GetSubscriberCountMethodName);

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.GetSubscriberCountLogInput, SampleGroupId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetSubscriberCount_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseFailed(Consts.GetSubscriberCountMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetSubscriberCount_BlastCountRetrievedCorrectly_ResponseSuccessful()
        {
            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseSuccess(Consts.GetSubscriberCountMethodName, SampleGetBlastEmailListCount.ToString());
            AssertLogUpdated();
        }

        [Test]
        public void GetSubscriberCount_BlastCountNotRetrieved_ResponseSuccessfulWithZeroResult()
        {
            // Arrange
            var expectedCount = 0;
            MockGetBlastEmailListForDynamicContent(_blastManagerMock, new DataTable());

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseSuccess(Consts.GetSubscriberCountMethodName, expectedCount.ToString());
            AssertLogUpdated();
        }

        [Test]
        public void GetSubscriberCount_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            MockGetBlastEmailListForDynamicContent(_blastManagerMock, null, ecnException);

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseFailed(Consts.GetSubscriberCountMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetSubscriberCount_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            MockGetBlastEmailListForDynamicContent(_blastManagerMock, null, securityException);

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseFailed(Consts.GetSubscriberCountMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void GetSubscriberCount_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            MockGetBlastEmailListForDynamicContent(_blastManagerMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseFailed(Consts.GetSubscriberCountMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetSubscriberCount_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            MockGetBlastEmailListForDynamicContent(
                _blastManagerMock,
                null,
                new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetSubscriberCount(SampleEcnAccessKey, SampleGroupId);

            // Assert
            AssertResponseFailed(Consts.GetSubscriberCountMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
