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
        private const string GetBlastMethodExpectedResponseOutput = "<DocumentElement xmlns=\"\"><Blast><BlastID>-1</BlastID><LayoutID></LayoutID><GroupID></GroupID><UserID></UserID><FilterID></FilterID><SmartSegmentID></SmartSegmentID><EmailSubject></EmailSubject><EmailFrom></EmailFrom><EmailFromName></EmailFromName><ReplyTo></ReplyTo><BlastType></BlastType><SendTime></SendTime><TestBlast></TestBlast></Blast></DocumentElement>";

        [Test]
        public void GetBlast_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetBlast(string.Empty, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetBlast_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastMethodName);

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.GetBlastLogInput, SampleBlastId)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetBlast_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlast_BlastRetrievedCorrectly_ResponseSuccessful()
        {
            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseSuccess(Consts.GetBlastMethodName, GetBlastMethodExpectedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlast_BlastNotFound_ResponseFailWithBlastNotFound()
        {
            // Arrange
            MockGetByBlastId(_blastManagerMock, null);
            var expectedOutput = string.Format(Consts.BlastNotFoundResponseOutput, SampleBlastId);

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlast_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            MockGetByBlastId(_blastManagerMock, null, ecnException);

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlast_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            MockGetByBlastId(_blastManagerMock, null, securityException);

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void GetBlast_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            MockGetByBlastId(_blastManagerMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetBlast_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            MockGetByBlastId(_blastManagerMock, null , new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetBlast(SampleEcnAccessKey, SampleBlastId);

            // Assert
            AssertResponseFailed(Consts.GetBlastMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
