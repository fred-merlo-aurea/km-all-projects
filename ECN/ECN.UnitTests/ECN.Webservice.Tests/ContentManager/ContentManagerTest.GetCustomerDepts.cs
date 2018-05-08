using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using ecn.webservice;
using ECN_Framework_Common.Objects;
using APILogging = ECN_Framework_Entities.Communicator.APILogging;

namespace ECN.Webservice.Tests.ContentManager
{
    [TestFixture]
    public partial class ContentManagerTest
    {
        private const string GetCustomerDeptsResult = "GetCustomerDeptsResult";

        [Test]
        public void GetCustomerDepts_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Act
            _manager.GetCustomerDepts(string.Empty);

            // Assert
            AssertResponseFailed(Consts.GetCustomerDeptsMethodName, Consts.InvalidEcnAccessKeyResponseOutput);

            _loggingManagerMock.Verify(mock => mock.Insert(
                It.IsAny<APILogging>()), Times.Never);
        }

        [Test]
        public void GetCustomerDepts_EcnAccessKeyValid_LogCreated()
        {
            // Arrange
            var expectedApiMethod = string.Format(
                Consts.ContentManagerServiceMethodName,
                Consts.GetCustomerDeptsMethodName);

            // Act
            _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            _loggedEntity.ShouldNotBeNull();
            _loggedEntity.ShouldSatisfyAllConditions(
                () => _loggedEntity.AccessKey.ShouldBe(SampleEcnAccessKey),
                () => _loggedEntity.APIMethod.ShouldBe(expectedApiMethod),
                () => _loggedEntity.Input.ShouldBe(Consts.EmptyRootLogInput),
                () => _loggedEntity.APILogID.ShouldBe(SampleLogId));
        }

        [Test]
        public void GetCustomerDepts_UserNotLoggedIn_AuthenticationFailedResponse()
        {
            // Arrange
            _executionWrapper.UserManager = GetUserManagerMock(null).Object;

            // Act
            _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetCustomerDeptsMethodName, Consts.LoginFailedResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomerDepts_MessagesRetrievedSuccessfully_ResponseSuccessful()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetCustomerDepts(_contentFacadeMock, GetCustomerDeptsResult);

            // Act
            var response = _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            response.ShouldBe(GetCustomerDeptsResult);
        }

        [Test]
        public void GetCustomerDepts_EcnExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            var ecnException = GetEcnException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetCustomerDepts(_contentFacadeMock, GetCustomerDeptsResult, ecnException);

            // Act
            _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetCustomerDeptsMethodName, SampleEcnException);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomerDepts_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            var securityException = new SecurityException(string.Empty);
            _manager.ContentFacade = GetContentFacade(true);
            MockGetCustomerDepts(_contentFacadeMock, null, securityException);

            // Act
            _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetCustomerDeptsMethodName, Consts.SecurityViolationResponseOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomerDepts_UserLoginExceptionRaised_ResponseFailWithUserStatusMessage()
        {
            // Arrange
            var loginException = GetUserLoginException();
            _manager.ContentFacade = GetContentFacade(true);
            MockGetCustomerDepts(_contentFacadeMock, null, loginException);
            var expectedOutput = KMPlatform.Enums.UserLoginStatus.InvalidPassword.ToString();

            // Act
            _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetCustomerDeptsMethodName, expectedOutput);
            AssertLogUpdated();
        }

        [Test]
        public void GetCustomerDepts_GeneralExceptionRaised_ResponseFailWithExceptionMessage()
        {
            // Arrange
            _manager.ContentFacade = GetContentFacade(true);
            MockGetCustomerDepts(_contentFacadeMock, null, new Exception(GeneralExceptionDescription));

            // Act
            _manager.GetCustomerDepts(SampleEcnAccessKey);

            // Assert
            AssertResponseFailed(Consts.GetCustomerDeptsMethodName, GeneralExceptionDescription);
            AssertLogNotUpdated();
        }
    }
}