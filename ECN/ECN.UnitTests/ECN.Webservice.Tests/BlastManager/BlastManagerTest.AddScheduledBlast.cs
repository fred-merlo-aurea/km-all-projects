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
        private const string ExpectedAddScheduledBlastLogInput = "<ROOT><MessageID>9876</MessageID><ListID>8765</ListID><DeptID>7654</DeptID><FilterID>6543</FilterID><Subject>SampleSubject</Subject><FromEmail>SampleFromEmail</FromEmail><FromName>SampleFromName</FromName><ReplyEmail>SampleReplyEmail</ReplyEmail><XMLSchedule><![CDATA[SampleXmlSchedule]]></XMLSchedule><RefBlasts>SampleRefBlasts</RefBlasts></ROOT>";
        private const string SampleAddScheduledBlastResponse = "SampleAddScheduledBlastResponse";
        
        [Test]
        public void AddScheduledBlast_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            ExecuteAddScheduledBlast(string.Empty);

            // Assert
            AssertResponseFailed(Consts.AddScheduledBlastMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void AddScheduledBlast_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.AddScheduledBlastMethodName);

            // Act
            ExecuteAddScheduledBlast();

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(ExpectedAddScheduledBlastLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void AddScheduledBlast_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteAddScheduledBlast();

            // Assert
            AssertResponseFailed(Consts.AddScheduledBlastMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddScheduledBlast_AddScheduledBlastSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddScheduledBlast(_blastFacadeMock, SampleAddScheduledBlastResponse);

            // Act
            var response = ExecuteAddScheduledBlast();

            // Assert
            response.ShouldBe(SampleAddScheduledBlastResponse);
        }

        [Test]
        public void AddScheduledBlast_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddScheduledBlast(_blastFacadeMock, string.Empty, ecnException);

            // Act
            ExecuteAddScheduledBlast();

            // Assert
            AssertResponseFailed(Consts.AddScheduledBlastMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void AddScheduledBlast_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddScheduledBlast(_blastFacadeMock, string.Empty, securityException);

            // Act
            ExecuteAddScheduledBlast();

            // Assert
            AssertResponseFailed(Consts.AddScheduledBlastMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void AddScheduledBlast_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddScheduledBlast(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteAddScheduledBlast();

            // Assert
            AssertResponseFailed(Consts.AddScheduledBlastMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddScheduledBlast_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddScheduledBlast(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteAddScheduledBlast();

            // Assert
            AssertResponseFailed(Consts.AddScheduledBlastMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteAddScheduledBlast(string ecnAccessKey = SampleEcnAccessKey)
        {
            return _manager.AddScheduledAdvancedBlast(
                ecnAccessKey,
                SampleMessageId,
                SampleListId,
                SampleDeptId,
                SampleFilterId,
                SampleSubject,
                SampleFromEmail,
                SampleFromName,
                SampleReplyMail,
                true,
                SampleXmlSchedule,
                SampleRefBlasts);
        }
    }
}
