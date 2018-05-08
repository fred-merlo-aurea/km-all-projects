using System;
using System.Collections.Generic;
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
using NUnit.Framework;
using Shouldly;
using AccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private const string FolderName = "name";
        private const string FolderCreated = "FOLDER CREATED";
        private const string FolderAlreadyExist = "ALREADY EXISTS";
        private const string FolderDoesNotExist = "PARENT FOLDER DOES NOT EXIST FOR CUSTOMER";

        [Test]
        public void AddFolder_ForInvalidKey_ReturnResponseFail()
        {
            // Arrange
            _listManager = new ListManager();

            // Act
            var result = _listManager.AddFolder(InvalidKey, FolderName, FolderName);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(InvalidAccessKey);
        }

        [Test]
        [TestCase(0, FolderCreated)]
        [TestCase(Id, FolderAlreadyExist)]
        public void AddFolder_ForValidKey_ResponseCodeSuccess(int exist, string response)
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
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => exist;
            ShimFolder.SaveFolderUser = (x, y) => Id;

            // Act
            var result = _listManager.AddFolder(SampleEcnAccessKey, FolderName, FolderName);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(response);
        }

        [Test]
        public void AddFolder_ForValidKeyAndParentFolderId_ReturnResponse()
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
            ShimFolder.SaveFolderUser = (x, y) => Id;

            // Act
            var result = _listManager.AddFolder(SampleEcnAccessKey, FolderName, FolderName, SampleFolderId);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(FolderDoesNotExist);
        }

        [Test]
        public void AddFolder_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
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
            var result = _listManager.AddFolder(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(SecurityViolation);
        }

        [Test]
        public void AddFolder_UserExceptionRaised_ResponseFailWithUserExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new UserLoginException();
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddFolder(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(DisabledUser);
        }

        [Test]
        public void AddFolder_EcnExceptionRaised_ResponseFailWithEcnExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new ECNException(EcnException, new List<ECNError> { new ECNError() }, Enums.ExceptionLayer.Business);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.AddFolder(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(EcnException);
        }

        [Test]
        public void AddFoldert_GeneralExceptionRaised_ResponseFailWithGeneralExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception(Exception);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimListManager.AllInstances.LogUnspecifiedExceptionExceptionStringString = (x, y, z, q) => Id;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Exception;

            // Act
            var result = _listManager.AddFolder(SampleEcnAccessKey, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(Exception);
        }
    }
}
