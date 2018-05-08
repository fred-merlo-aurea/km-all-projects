using System;
using System.Collections.Generic;
using ecn.webservice;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using Moq;
using NUnit.Framework;
using Shouldly;
using APILogging = ECN_Framework_Entities.Communicator.APILogging;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class BlastManagerTest
    {
        private const string SearchForBlastsExpectedResponseOutput = "<DocumentElement xmlns=\"\"><Blast><BlastID>-1</BlastID><LayoutID></LayoutID><GroupID></GroupID><UserID></UserID><FilterID></FilterID><SmartSegmentID></SmartSegmentID><EmailSubject></EmailSubject><EmailFrom></EmailFrom><EmailFromName></EmailFromName><ReplyTo></ReplyTo><BlastType></BlastType><SendTime></SendTime><TestBlast></TestBlast></Blast></DocumentElement>";
        private const string SearchNoResultsExpectedResponseOutput = "<DocumentElement xmlns=\"\"></DocumentElement>";

        [Test]
        public void SearchForBlasts_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.SearchForBlasts(string.Empty, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForBlastsMethodName, Consts.InvalidEcnAccessKeyResponseOutput);
            
            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void SearchForBlasts_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.BlastManagerServiceMethodName,
                Consts.SearchForBlastsMethodName);

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(string.Format(Consts.SearchLogInput, SampleXmlSearch)),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void SearchForBlasts_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForBlastsMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForBlasts_BlastsRetrievedCorrectly_ResponseSuccessful()
        {
            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseSuccess(Consts.SearchForBlastsMethodName, SearchForBlastsExpectedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForBlasts_BlastNotFound_ResponseSuccessWithEmptyXml()
        {
            // Arrange
            MockGetBySearch(_blastManagerMock, new List<BlastAbstract>());

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseSuccess(Consts.SearchForBlastsMethodName, SearchNoResultsExpectedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForBlasts_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            MockGetBySearch(_blastManagerMock, null, ecnException);

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForBlastsMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForBlasts_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            MockGetBySearch(_blastManagerMock, null, securityException);

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForBlastsMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }
        
        [Test]
        public void SearchForBlasts_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            MockGetBySearch(_blastManagerMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForBlastsMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void SearchForBlasts_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            MockGetBySearch(_blastManagerMock, null , new Exception(GeneralExceptionDescription));

            // Act
            _manager.SearchForBlasts(SampleEcnAccessKey, SampleXmlSearch);

            // Assert
            AssertResponseFailed(Consts.SearchForBlastsMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
