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
        public void GetBlastBounceReport_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            _manager.GetBlastBounceReport(string.Empty, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastBounceReportMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlastBounceReport_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastBounceReportMethodName);
            var expectedLogInput = string.Format(Consts.GetBlastBounceReportLogInput, SampleBlastId, true);

            // Act
            _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlastBounceReport_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastBounceReportMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastBounceReport_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastBounceReport(_blastFacadeMock, SampleGetBlastReportResponse);

            // Act
            var response = _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            response.ShouldBe(SampleGetBlastReportResponse);
        }

        [Test]
        public void GetBlastBounceReport_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastBounceReport(_blastFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastBounceReportMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastBounceReport_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastBounceReport(_blastFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastBounceReportMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastBounceReport_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastBounceReport(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastBounceReportMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastBounceReport_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastBounceReport(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlastBounceReport(SampleEcnAccessKey, SampleBlastId, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastBounceReportMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
