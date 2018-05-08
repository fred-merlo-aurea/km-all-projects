using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using ecn.webservice;
using ECN_Framework_Common.Objects;
using APILogging = ECN_Framework_Entities.Communicator.APILogging;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private const string SampleGetFoldersResult = "SampleGetFoldersResult";

        [Test]
        public void GetFolders_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.ListFacade = GetListFacade(true);
            _manager.GetFolders(string.Empty);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetFolders_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            var expectedApiMethod = string.Format(Consts.ListManagerServiceMethodName, Consts.GetFoldersMethodName);

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(Consts.EmptyRootLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetFolders_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetFolders(_listFacadeMock, SampleGetFoldersResult);

            // Act
            var response = _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(SampleGetFoldersResult);
        }

        [Test]
        public void GetFolders_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ListFacade = GetListFacade(true);
            MockGetFolders(_listFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ListFacade = GetListFacade(true);
            MockGetFolders(_listFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ListFacade = GetListFacade(true);
            MockGetFolders(_listFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFolders_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetFolders(_listFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetFolders(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetFoldersMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
