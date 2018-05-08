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
        private const string SampleGetFiltersResult = "SampleGetFiltersResult";

        [Test]
        public void GetFilters_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.ListFacade = GetListFacade(true);
            _manager.GetFilters(string.Empty, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetFiltersMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetFilters_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            var expectedApiMethod = string.Format(
                Consts.ListManagerServiceMethodName,
                Consts.GetFiltersMethodName);
            var expectedLogInput = string.Format(Consts.ListIdLogInput, SampleListId);

            // Act
            _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetFilters_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetFiltersMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFilters_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetFilters(_listFacadeMock, SampleGetFiltersResult);

            // Act
            var response = _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            response.ShouldBe(SampleGetFiltersResult);
        }

        [Test]
        public void GetFilters_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ListFacade = GetListFacade(true);
            MockGetFilters(_listFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetFiltersMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetFilters_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ListFacade = GetListFacade(true);
            MockGetFilters(_listFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetFiltersMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFilters_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ListFacade = GetListFacade(true);
            MockGetFilters(_listFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetFiltersMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetFilters_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetFilters(_listFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetFilters(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetFiltersMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
