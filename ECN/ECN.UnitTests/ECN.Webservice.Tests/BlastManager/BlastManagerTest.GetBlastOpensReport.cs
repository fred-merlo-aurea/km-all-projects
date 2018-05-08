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
        public void GetBlastOpensReport_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            _manager.GetBlastOpensReport(string.Empty, SampleBlastId, SampleFilterType, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastOpensReportMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlastOpensReport_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastOpensReportMethodName);
            var expectedLogInput = string.Format(Consts.GetBlastOpensReportLogInput, SampleBlastId, SampleFilterType, true);

            // Act
            _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlastOpensReport_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastOpensReportMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastOpensReport_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastOpensReport(_blastFacadeMock, SampleGetBlastReportResponse);

            // Act
            var response = _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            response.ShouldBe(SampleGetBlastReportResponse);
        }

        [Test]
        public void GetBlastOpensReport_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastOpensReport(_blastFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastOpensReportMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastOpensReport_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastOpensReport(_blastFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastOpensReportMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void GetBlastOpensReport_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastOpensReport(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastOpensReportMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastOpensReport_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastOpensReport(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlastOpensReport(SampleEcnAccessKey, SampleBlastId, SampleFilterType, true);

            // Assert
            AssertResponseFailed(Consts.GetBlastOpensReportMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
