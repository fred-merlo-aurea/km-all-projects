using System;
using System.Collections.Generic;
using System.Data;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.webservice.classes.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using EcnCommonObject = ECN_Framework_Common.Objects;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private const string RecordNotFound = "Records not found";

        [Test]
        public void GetSubscriberStatus_ForInvalidKey_ReturnResponseFail()
        {
            // Arrange
            _manager = new ListManager();

            // Act
            var result = _manager.GetSubscriberStatus(SubscriberInvalidKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(InvalidEcnKey));
        }

        [Test]
        public void GetSubscriberStatus_ForValidKey_ReturnSuccessResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscriberStatus();

            // Act
            var result = _manager.GetSubscriberStatus(SampleEcnAccessKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ResponseBody));
        }

        [Test]
        public void GetSubscriberStatus_ForValidKeyAndNullDataTable_ReturnFailResponse()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscriberStatus();
            ShimEmailGroup.GetSubscriberStatusStringUser = (x, y) => new DataTable();

            // Act
            var result = _manager.GetSubscriberStatus(SampleEcnAccessKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(RecordNotFound));
        }

        [Test]
        public void GetSubscriberStatus_ForEcnException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscriberStatus();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.ECNException(InvalidEcnKey,
                new List<EcnCommonObject.ECNError> { new EcnCommonObject.ECNError() }, EcnCommonObject.Enums.ExceptionLayer.Business);

            // Act
            var result = _manager.GetSubscriberStatus(SampleEcnAccessKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void GetSubscriberStatus_ForSecurityException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscriberStatus();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new EcnCommonObject.SecurityException();

            // Act
            var result = _manager.GetSubscriberStatus(SampleEcnAccessKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void GetSubscriberStatus_ForUserLoginException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscriberStatus();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();

            // Act
            var result = _manager.GetSubscriberStatus(SampleEcnAccessKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        [Test]
        public void GetSubscriberStatus_ForGeneralException_ReturnExceptionMessage()
        {
            // Arrange
            _manager = new ListManager();
            InitializeSubscriberStatus();
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception();
            ShimListManager.AllInstances.LogUnspecifiedExceptionExceptionStringString = (x, y, z, q) => ListId;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => ErrorCode.ToString();

            // Act
            var result = _manager.GetSubscriberStatus(SampleEcnAccessKey, ResponseBody);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldContain(ErrorCode.ToString()));
        }

        private void InitializeSubscriberStatus()
        {
            ShimAPILogging.InsertAPILogging = (x) => ListId;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => new User()
            {
                CustomerID = ListId,
                DefaultClientID = ListId,
                UserID = ListId
            };
            ShimCustomer.GetByClientIDInt32Boolean = (x, y) => new Customer() { CustomerID = ListId };
            _subscriberDataTable = new DataTable()
            {
                Columns = { "GroupID", "GroupName", "SubscribeTypeCode" },
                Rows = { { ListId, ResponseBody, ResponseBody } }
            };
            ShimEmailGroup.GetSubscriberStatusStringUser = (x, y) => _subscriberDataTable;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
        }
    }
}
