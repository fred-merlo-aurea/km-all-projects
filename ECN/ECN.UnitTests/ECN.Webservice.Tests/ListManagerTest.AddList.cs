using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.webservice.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using AccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private ListManager _listManager;
        private const string InvalidKey = "key";
        private const string List = "name";
        private const string InvalidAccessKey = "INVALID ECN ACCESS KEY FORMAT";
        private const string Created = "LIST CREATED";
        private const string AlreadyExist = "ALREADY EXISTS FOR CUSTOMER";
        private const string FolderExist = "FOLDER DOES NOT EXIST FOR CUSTOMER";
        private const string SecurityViolation = "SECURITY VIOLATION";
        private const string DisabledUser = "DisabledUser";
        private const string EcnException = "Entity";
        private const string Exception = "Entity";
        private const int Id = 10;

        [Test]
        public void AddList_EcnAccessKeyInvalid_ResponseCodeFail()
        {
            // Arrange
            _listManager = new ListManager();

            // Act
            var result = _listManager.AddList(InvalidKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(InvalidAccessKey);
        }

        [Test]
        [TestCase(false, Created)]
        [TestCase(true, AlreadyExist)]
        public void AddList_ForValidKey_ResponseCodeSuccess(bool exist, string response)
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => new User()
            {
                DefaultClientID = Id,
                UserID = Id,
                CustomerID = Id
            };
            AccountsFakes.ShimCustomer.GetByClientIDInt32Boolean = (x, y) => new Customer()
            {
                CustomerID = Id
            };
            ShimGroup.ExistsInt32StringInt32Int32 = (x, y, z, q) => exist;
            ShimGroup.SaveGroupUser = (x, y) => Id;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddList(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(response);
        }

        [Test]
        public void AddList_ForValidKeyAndFolderDoesnotExist_ResponseCodeSuccess()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => new User()
            {
                DefaultClientID = Id,
                UserID = Id,
                CustomerID = Id
            };
            AccountsFakes.ShimCustomer.GetByClientIDInt32Boolean = (x, y) => new Customer()
            {
                CustomerID = Id
            };
            ShimFolder.ExistsInt32Int32 = (x, y) => false;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddList(SampleEcnAccessKey, List, List, Id);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(FolderExist);
        }

        [Test]
        public void AddList_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => new User()
            {
                DefaultClientID = Id,
                UserID = Id,
                CustomerID = Id
            };
            DataLayerFakes.ShimCustomer.GetByClientIDInt32 = (x) => null;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddList(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(SecurityViolation);
        }

        [Test]
        public void AddList_UserExceptionRaised_ResponseFailWithUserExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddList(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(DisabledUser);
        }

        [Test]
        public void AddList_EcnExceptionRaised_ResponseFailWithEcnExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new ECNException(EcnException, new List<ECNError> { new ECNError() }, Enums.ExceptionLayer.Business);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddList(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(EcnException);
        }

        [Test]
        public void AddList_GeneralExceptionRaised_ResponseFailWithGeneralExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception(Exception);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimListManager.AllInstances.LogUnspecifiedExceptionExceptionStringString = (x, y, z, q) => Id;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Exception;

            // Act
            var result = _listManager.AddList(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(Exception);
        }
    }
}
