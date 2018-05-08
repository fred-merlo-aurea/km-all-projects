using System;
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
        [Test]
        public void DeleteBlast_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.DeleteBlast(string.Empty, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void DeleteBlast_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.DeleteBlastMethodName);

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.DeleteBlastLogInput, SampleBlastId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void DeleteBlast_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteBlast_BlastDeletedCorrectly_ResponseSuccessful()
        {
            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseSuccess(Consts.DeleteBlastMethodName, Consts.DeleteBlastResposneOutput, SampleBlastId);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteBlast_BlastNotFound_ResponseFailWithBlastNotFound()
        {
            // Arrange
            MockGetByBlastId(_campaignItemBlastManagerMock, null);
            MockGetByBlastId(_campaignItemTestBlastManagerMock, null);

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, Consts.BlastDoesntExistResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteBlast_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            MockGetByBlastId(_campaignItemBlastManagerMock, null, ecnException);

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteBlast_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            MockGetByBlastId(_campaignItemBlastManagerMock, null, securityException);

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void DeleteBlast_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            MockGetByBlastId(_campaignItemBlastManagerMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void DeleteBlast_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            MockGetByBlastId(_campaignItemBlastManagerMock, null , new Exception(GeneralExceptionDescription));

            // Act
            _manager.DeleteBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.DeleteBlastMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
