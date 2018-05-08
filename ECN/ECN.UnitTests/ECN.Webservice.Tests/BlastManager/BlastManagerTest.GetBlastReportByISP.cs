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
        public void GetBlastReportByISP_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            _manager.GetBlastReportByISP(string.Empty, SampleBlastId, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportByISPMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlastReportByISP_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastReportByISPMethodName);
            var expectedLogInput = string.Format(Consts.GetBlastReportByISPLogInput, SampleBlastId, SampleXmlSearch);

            // Act
            _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlastReportByISP_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportByISPMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastReportByISP_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReportByISP(_blastFacadeMock, SampleGetBlastReportResponse);

            // Act
            var response = _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            response.ShouldBe(SampleGetBlastReportResponse);
        }

        [Test]
        public void GetBlastReportByISP_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReportByISP(_blastFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportByISPMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastReportByISP_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReportByISP(_blastFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportByISPMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void GetBlastReportByISP_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReportByISP(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportByISPMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlastReportByISP_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockGetBlastReportByISP(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlastReportByISP(SampleEcnAccessKey, SampleBlastId, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.GetBlastReportByISPMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
