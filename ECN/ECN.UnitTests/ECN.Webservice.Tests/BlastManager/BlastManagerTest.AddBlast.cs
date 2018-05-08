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
        private const string ExpectedAddBlastLogInput = "<ROOT><MessageID>9876</MessageID><ListID>8765</ListID><DeptID>7654</DeptID><FilterID>6543</FilterID><Subject>SampleSubject</Subject><FromEmail>SampleFromEmail</FromEmail><FromName>SampleFromName</FromName><ReplyEmail>SampleReplyEmail</ReplyEmail><IsTest>True</IsTest><RefBlasts></RefBlasts><OverRideAmount></OverRideAmount><IsOverRideAmount></IsOverRideAmount></ROOT>";
        private const string SampleAddBlastResponse = "SampleAddBlastResponse";
        
        [Test]
        public void AddBlast_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.BlastFacade = GetBlastFacade(true);
            ExecuteAddBlast(string.Empty);

            // Assert
            AssertResponseFailed(Consts.AddBlastMainMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void AddBlast_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            var expectedApiMethod = string.Format(Consts.BlastManagerServiceMethodName, Consts.AddBlastMainMethodName);

            // Act
            ExecuteAddBlast();

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(ExpectedAddBlastLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void AddBlast_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            ExecuteAddBlast();

            // Assert
            AssertResponseFailed(Consts.AddBlastMainMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddBlast_AddBlastSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddBlast(_blastFacadeMock, SampleAddBlastResponse);

            // Act
            var response = ExecuteAddBlast();

            // Assert
            response.ShouldBe(SampleAddBlastResponse);
        }

        [Test]
        public void AddBlast_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddBlast(_blastFacadeMock, string.Empty, ecnException);

            // Act
            ExecuteAddBlast();

            // Assert
            AssertResponseFailed(Consts.AddBlastMainMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void AddBlast_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddBlast(_blastFacadeMock, string.Empty, securityException);

            // Act
            ExecuteAddBlast();

            // Assert
            AssertResponseFailed(Consts.AddBlastMainMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void AddBlast_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddBlast(_blastFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            ExecuteAddBlast();

            // Assert
            AssertResponseFailed(Consts.AddBlastMainMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void AddBlast_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.BlastFacade = GetBlastFacade(true);
            MockAddBlast(_blastFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            ExecuteAddBlast();

            // Assert
            AssertResponseFailed(Consts.AddBlastMainMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }

        private string ExecuteAddBlast(string ecnAccessKey = SampleEcnAccessKey)
        {
            return _manager.AddBlast(
                ecnAccessKey,
                SampleMessageId,
                SampleListId,
                SampleDeptId,
                SampleFilterId,
                SampleSubject,
                SampleFromEmail,
                SampleFromName,
                SampleReplyMail,
                true);
        }
    }
}
