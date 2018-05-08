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
        private const string ExpectedUpdateBlastLogInput = "<ROOT><MessageID>9876</MessageID><ListID>8765</ListID><BlastID>111</BlastID><FilterID>6543</FilterID><Subject>SampleSubject</Subject><FromEmail>SampleFromEmail</FromEmail><FromName>SampleFromName</FromName><ReplyEmail>SampleReplyEmail</ReplyEmail></ROOT>";
        private const string SampleUpdateBlastResponse = "SampleUpdateBlastResponse";
        
        [Test]
        public void UpdateBlast_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            ExecuteUpdateBlast(string.Empty);

            // Assert
            AssertResponseFailed(Consts.UpdateBlastMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void UpdateBlast_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.UpdateBlastMethodName);

            // Act
            ExecuteUpdateBlast();

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(ExpectedUpdateBlastLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void UpdateBlast_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteUpdateBlast();

            // Assert
            AssertResponseFailed(Consts.UpdateBlastMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateBlast_UpdateBlastSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockUpdateBlast(_blastFacadeMock, SampleUpdateBlastResponse);

            // Act
            var response = ExecuteUpdateBlast();

            // Assert
            response.ShouldBe(SampleUpdateBlastResponse);
        }

        [Test]
        public void UpdateBlast_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockUpdateBlast(_blastFacadeMock, string.Empty, ecnException);

            // Act
            ExecuteUpdateBlast();

            // Assert
            AssertResponseFailed(Consts.UpdateBlastMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateBlast_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockUpdateBlast(_blastFacadeMock, string.Empty, securityException);

            // Act
            ExecuteUpdateBlast();

            // Assert
            AssertResponseFailed(Consts.UpdateBlastMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void UpdateBlast_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockUpdateBlast(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteUpdateBlast();

            // Assert
            AssertResponseFailed(Consts.UpdateBlastMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void UpdateBlast_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockUpdateBlast(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteUpdateBlast();

            // Assert
            AssertResponseFailed(Consts.UpdateBlastMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteUpdateBlast(string ecnAccessKey = SampleEcnAccessKey)
        {
            return _manager.UpdateBlast(
                ecnAccessKey,
                SampleMessageId,
                SampleListId,
                SampleBlastId,
                SampleFilterId,
                SampleSubject,
                SampleFromEmail,
                SampleFromName,
                SampleReplyMail);
        }
    }
}
