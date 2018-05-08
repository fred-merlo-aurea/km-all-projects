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
        public void GetBlastUnsubscribeReport_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            _manager.GetBlastUnsubscribeReport(string.Empty, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastUnsubscribeReportMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlastUnsubscribeReport_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastUnsubscribeReportMethodName);
            var expectedLogInput = string.Format(Consts.GetBlastUnsubscribeReportLogInput, SampleBlastId, true);

            // Act
            _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlastUnsubscribeReport_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastUnsubscribeReportMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastUnsubscribeReport_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastUnsubscribeReport(_blastFacadeMock, SampleGetBlastReportResponse);

            // Act
            var response = _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            response.ShouldBe(SampleGetBlastReportResponse);
        }

        [Test]
        public void GetBlastUnsubscribeReport_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastUnsubscribeReport(_blastFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastUnsubscribeReportMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastUnsubscribeReport_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastUnsubscribeReport(_blastFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastUnsubscribeReportMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastUnsubscribeReport_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastUnsubscribeReport(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastUnsubscribeReportMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastUnsubscribeReport_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastUnsubscribeReport(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlastUnsubscribeReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastUnsubscribeReportMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
