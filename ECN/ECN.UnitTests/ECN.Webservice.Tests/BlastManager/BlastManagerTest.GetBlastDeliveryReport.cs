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
        public void GetBlastDeliveryReport_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            _manager.GetBlastDeliveryReport(string.Empty, DateTime.Today, DateTime.Today);

            // Assert
            AssertResponseFailed(Consts.GetBlastDeliveryReportMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlastDeliveryReport_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastDeliveryReportMethodName);
            var expectedLogInput = string.Format(Consts.GetBlastDeliveryReportLogInput, DateTime.Today, DateTime.Today);

            // Act
            _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlastDeliveryReport_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            AssertResponseFailed(Consts.GetBlastDeliveryReportMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastDeliveryReport_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastDeliveryReport(_blastFacadeMock, SampleGetBlastReportResponse);

            // Act
            var response = _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            response.ShouldBe(SampleGetBlastReportResponse);
        }

        [Test]
        public void GetBlastDeliveryReport_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastDeliveryReport(_blastFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            AssertResponseFailed(Consts.GetBlastDeliveryReportMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastDeliveryReport_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastDeliveryReport(_blastFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            AssertResponseFailed(Consts.GetBlastDeliveryReportMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastDeliveryReport_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastDeliveryReport(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            AssertResponseFailed(Consts.GetBlastDeliveryReportMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastDeliveryReport_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastDeliveryReport(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlastDeliveryReport(SampleEcnAccessKey, DateTime.Today, DateTime.Today);

            // Assert
            AssertResponseFailed(Consts.GetBlastDeliveryReportMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
