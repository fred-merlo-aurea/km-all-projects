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
        private const string SampleGetBlastReportResponse = "SampleGetBlastReportResponse";
        
        [Test]
        public void GetBlastReport_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            _manager.GetBlastReport(string.Empty, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlastReport_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastReportMethodName);
            var expectedLogInput = string.Format(Consts.GetBlastReportLogInput, SampleBlastId);

            // Act
            _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlastReport_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastReport_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReport(_blastFacadeMock, SampleGetBlastReportResponse);

            // Act
            var response = _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            response.ShouldBe(SampleGetBlastReportResponse);
        }

        [Test]
        public void GetBlastReport_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReport(_blastFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastReport_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReport(_blastFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void GetBlastReport_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReport(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastReport_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReport(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlastReport(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
