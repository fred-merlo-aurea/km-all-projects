using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using ecn.webservice.classes;
using ecn.webservice;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using KM.Common.Managers;
using KMPlatform.BusinessLogic.Interfaces;
using KMPlatform.Entity;
using KMPlatform.Object;
using Application = KM.Common.Entity.Application;
using APILogging = ECN_Framework_Entities.Communicator.APILogging;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [ExcludeFromCodeCoverage]
    public class ManagerTestBase
    {
        protected const string SampleEcnAccessKey = "{2B1BDF1E-E365-41FA-BEDA-7BDCAF6F1D76}";
        protected const string GeneralExceptionDescription = "GeneralException";
        protected const string SampleEcnException = "SampleECNException";
        protected const string SampleXmlSearch = "<DocumentElement xmlns=\"\"></DocumentElement>";
        protected const string NullReferenceException = "System.NullReferenceException";
        protected const int SampleCustomerId = 12345;
        protected const string SampleResponseOutput = "SampleResponse";
        protected const int SampleLogId = 1234;
        protected const int SampleListId = 556677;
        protected const int SampleFolderId = 123;
        protected const int SampleContentId = 112233;

        protected IWebMethodExecutionWrapper _executionWrapper;
        protected APILogging _loggedEntity;

        protected Mock<IResponseManager> _responseManagerMock;
        protected Mock<IAPILoggingManager> _loggingManagerMock;
        protected Mock<ICustomerManager> _customerManagerMock;
        protected Mock<IUser> _userManagerMock;
        protected Mock<IApplicationLogManager> _applicationLogManagerMock;
        
        protected void InitExecutionWrapperMocks()
        {
            _responseManagerMock = GetResponseManagerMock();
            _loggingManagerMock = GetLoggingManagerMock();
            _customerManagerMock = GetCustomerManagerMock(GetSampleCustomer());
            _userManagerMock = GetUserManagerMock(GetSampleUser());
            _applicationLogManagerMock = GetApplicationLogManagerMock();
            
            _executionWrapper.ResponseManager = _responseManagerMock.Object;
            _executionWrapper.ApiLoggingManager = _loggingManagerMock.Object;
            _executionWrapper.CustomerManager = _customerManagerMock.Object;
            _executionWrapper.UserManager = _userManagerMock.Object;
            _executionWrapper.ApplicationLogManager = _applicationLogManagerMock.Object;
        }

        protected Customer GetSampleCustomer()
        {
            var customer = new Customer
            {
                CustomerID = SampleCustomerId
            };

            return customer;
        }

        protected User GetSampleUser()
        {
            var user = new User();

            return user;
        }

        protected Mock<IResponseManager> GetResponseManagerMock()
        {
            var responseManagerMock = new Mock<IResponseManager>();

            responseManagerMock
                .Setup(mock => mock.GetResponse(
                    It.IsAny<string>(),
                    It.IsAny<SendResponse.ResponseCode>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(SampleResponseOutput);

            responseManagerMock
                .Setup(mock => mock.GetResponse(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .Returns(SampleResponseOutput);

            return responseManagerMock;
        }

        protected Mock<IAPILoggingManager> GetLoggingManagerMock()
        {
            var loggingManagerMock = new Mock<IAPILoggingManager>();

            loggingManagerMock
                .Setup(mock => mock.Insert(
                    It.IsAny<APILogging>()))
                .Callback((APILogging entityToLog) =>
                {
                    _loggedEntity = entityToLog;
                }).Returns(SampleLogId);

            loggingManagerMock
                .Setup(mock => mock.UpdateLog(
                    It.IsAny<int>(),
                    It.IsAny<int?>()))
                .Callback((int apiLogId, int? logId) => { });

            return loggingManagerMock;
        }

        protected Mock<IApplicationLogManager> GetApplicationLogManagerMock()
        {
            var applicationLogManagerMock = new Mock<IApplicationLogManager>();

            applicationLogManagerMock
                .Setup(mock => mock.LogCriticalError(
                    It.IsAny<Exception>(),
                    It.IsAny<string>(),
                    It.IsAny<Application.Applications>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            applicationLogManagerMock
                .Setup(mock => mock.LogCriticalError(
                    It.IsAny<Exception>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()));

            return applicationLogManagerMock;
        }

        protected Mock<IUser> GetUserManagerMock(User logInResult)
        {
            var userManagerMock = new Mock<IUser>();

            userManagerMock
                .Setup(mock => mock.LogIn(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>()))
                .Returns(logInResult);

            return userManagerMock;
        }

        protected Mock<ICustomerManager> GetCustomerManagerMock(Customer getCustomerResult)
        {
            var customerManagerMock = new Mock<ICustomerManager>();

            customerManagerMock
                .Setup(mock => mock.GetByClientID(
                    It.IsAny<int>(),
                    It.IsAny<bool>()))
                .Returns(getCustomerResult);

            return customerManagerMock;
        }

        protected void AssertResponseSuccess(string methodName, string expectedOutput, int expectedId = 0)
        {
            _responseManagerMock.Verify(mock => mock.GetResponse(
                It.Is<string>(webMethod => webMethod == methodName),
                It.Is<SendResponse.ResponseCode>(responseCode => responseCode == SendResponse.ResponseCode.Success),
                It.Is<int>(id => id == expectedId),
                It.Is<string>(output => output.Contains(expectedOutput))), Times.Once);
        }

        protected void AssertResponseFailed(string methodName, string expectedOutput)
        {
            _responseManagerMock.Verify(mock => mock.GetResponse(
                It.Is<string>(webMethod => webMethod == methodName),
                It.Is<SendResponse.ResponseCode>(responseCode => responseCode == SendResponse.ResponseCode.Fail),
                It.Is<int>(id => id == 0),
                It.Is<string>(output => output.Contains(expectedOutput))), Times.Once);
        }

        protected void AssertLogUpdated(int? expectedLogId = null)
        {
            _loggingManagerMock.Verify(mock => mock.UpdateLog(
                It.Is<int>(apiLogId => apiLogId == SampleLogId),
                It.Is<int?>(logId => logId == expectedLogId)), Times.Once);
        }

        protected void AssertLogNotUpdated()
        {
            _loggingManagerMock.Verify(mock => mock.UpdateLog(
                It.Is<int>(apiLogId => apiLogId == SampleLogId),
                It.Is<int?>(logId => logId == null)), Times.Never);
        }

        protected ECNException GetEcnException()
        {
            return new ECNException(new List<ECNError>
            {
                new ECNError(Enums.Entity.Blast, Enums.Method.Get, SampleEcnException)
            });
        }

        protected UserLoginException GetUserLoginException()
        {
            return new UserLoginException
            {
                UserStatus = KMPlatform.Enums.UserLoginStatus.InvalidPassword
            };
        }
    }
}
