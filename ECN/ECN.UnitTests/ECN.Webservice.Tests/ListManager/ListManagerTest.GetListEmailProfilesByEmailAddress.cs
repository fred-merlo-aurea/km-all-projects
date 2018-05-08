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
        private const string SampleGetListEmailProfilesByEmailAddressResult = "SampleGetListEmailProfilesByEmailAddressResult";
        private const int SampleListId = 556677;
        private const string SampleEmail = "SampleEmail";

        [Test]
        public void GetListEmailProfilesByEmailAddress_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.ListFacade = GetListFacade(true);
            _manager.GetListEmailProfilesByEmailAddress(string.Empty, SampleListId, SampleEmail);

            // Assert
            AssertResponseFailed(Consts.GetListEmailProfilesByEmailAddressMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            var expectedApiMethod = string.Format(
                Consts.ListManagerServiceMethodName,
                Consts.GetListEmailProfilesByEmailAddressMethodName);
            var expectedLogInput = string.Format(
                Consts.GetListEmailProfilesByEmailAddressLogInput,
                SampleListId,
                SampleEmail);

            // Act
            _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            AssertResponseFailed(Consts.GetListEmailProfilesByEmailAddressMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetListEmailProfilesByEmailAddress(_listFacadeMock, SampleGetListEmailProfilesByEmailAddressResult);

            // Act
            var response = _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            response.ShouldBe(SampleGetListEmailProfilesByEmailAddressResult);
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ListFacade = GetListFacade(true);
            MockGetListEmailProfilesByEmailAddress(_listFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            AssertResponseFailed(Consts.GetListEmailProfilesByEmailAddressMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ListFacade = GetListFacade(true);
            MockGetListEmailProfilesByEmailAddress(_listFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            AssertResponseFailed(Consts.GetListEmailProfilesByEmailAddressMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ListFacade = GetListFacade(true);
            MockGetListEmailProfilesByEmailAddress(_listFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            AssertResponseFailed(Consts.GetListEmailProfilesByEmailAddressMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetListEmailProfilesByEmailAddress(_listFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetListEmailProfilesByEmailAddress(SampleEcnAccessKey, SampleListId, SampleEmail);

            // Assert
            AssertResponseFailed(Consts.GetListEmailProfilesByEmailAddressMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
