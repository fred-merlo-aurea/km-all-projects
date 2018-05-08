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
        private const string SampleGetCustomFieldsResult = "SampleGetCustomFieldsResult";

        [Test]
        public void GetCustomFields_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.ListFacade = GetListFacade(true);
            _manager.GetCustomFields(string.Empty, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetCustomFieldsMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetCustomFields_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            var expectedApiMethod = string.Format(
                Consts.ListManagerServiceMethodName,
                Consts.GetCustomFieldsMethodName);
            var expectedLogInput = string.Format(Consts.ListIdLogInput, SampleListId);

            // Act
            _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(expectedLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetCustomFields_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetCustomFieldsMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomFields_GetBlastReportSuccessful_ResponseSuccess()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetCustomFields(_listFacadeMock, SampleGetCustomFieldsResult);

            // Act
            var response = _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            response.ShouldBe(SampleGetCustomFieldsResult);
        }

        [Test]
        public void GetCustomFields_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ListFacade = GetListFacade(true);
            MockGetCustomFields(_listFacadeMock, string.Empty, ecnException);

            // Act
            _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetCustomFieldsMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomFields_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ListFacade = GetListFacade(true);
            MockGetCustomFields(_listFacadeMock, string.Empty, securityException);

            // Act
            _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetCustomFieldsMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomFields_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ListFacade = GetListFacade(true);
            MockGetCustomFields(_listFacadeMock, string.Empty, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetCustomFieldsMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomFields_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ListFacade = GetListFacade(true);
            MockGetCustomFields(_listFacadeMock, string.Empty, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetCustomFields(SampleEcnAccessKey, SampleListId);

            // Assert
            AssertResponseFailed(Consts.GetCustomFieldsMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}
