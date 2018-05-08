using System;
using System.Data;
using System.Configuration.Fakes;
using System.Collections.Generic;
using System.Collections.Specialized;
using ecn.webservice;
using ecn.webservice.Fakes;
using ecn.webservice.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using AccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using DataLayerFakes = ECN_Framework_DataLayer.Accounts.Fakes;
using EcnCommonFakes = ecn.common.classes.Fakes;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    public partial class ListManagerTest
    {
        private DataTable _dataTable;
        private DataTable _dataTable1;
        private const string EmptyList = "LIST DOESN'T EXIST FOR CUSTOMER";
        private const string ListUpdated = "LIST UPDATED";
        private const string ListAlreadyExist = "ALREADY EXISTS FOR CUSTOMER";
        private const string Name = "Name";
        private const string GroupValue = "Groups xmlns";
        private const string UasMasterAccessKey = "UASMasterAccessKey";
        private const string Act = "act";

        [TearDown]
        public void TearDownList()
        {
            _dataTable?.Dispose();
            _dataTable1?.Dispose();
        }

        [Test]
        public void UpdateList_ForInvalidKey_ReturnResponseFailed()
        {
            // Arrange
            _listManager = new ListManager();

            // Act
            var result = _listManager.UpdateList(InvalidKey, Id, List, List);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(InvalidAccessKey);
        }

        [Test]
        [TestCase(false, ListUpdated, true)]
        [TestCase(false, EmptyList, false)]
        [TestCase(true, ListAlreadyExist, true)]
        public void UpdateList_ForValidKey_ReturnSuccessResponse(bool exist, string response, bool isGroup)
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
            if (isGroup)
            {
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group();
            }
            else
            {
                ShimGroup.GetByGroupIDInt32User = (x, y) => null;
            }
            ShimGroup.ExistsInt32StringInt32Int32 = (x, y, z, q) => exist;
            ShimGroup.SaveGroupUser = (x, y) => Id;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.UpdateList(SampleEcnAccessKey, Id, List, List, Id);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(response);
        }

        [Test]
        public void UpdateList_SecurityExceptionRaised_ResponseFailWithSecurityViolationMessage()
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
            var result = _listManager.UpdateList(SampleEcnAccessKey, Id, List, List, Id);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(SecurityViolation);
        }
        [Test]
        public void UpdateList_EcnExceptionRaised_ResponseFailWithEcnExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new ECNException(EcnException, new List<ECNError> { new ECNError() }, Enums.ExceptionLayer.Business);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;

            // Act
            var result = _listManager.UpdateList(SampleEcnAccessKey, Id, List, List, Id);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(EcnException);
        }

        [Test]
        public void UpdateList_GeneralExceptionRaised_ResponseFailWithGeneralExceptionMessage()
        {
            // Arrange
            _listManager = new ListManager();
            ShimAPILogging.InsertAPILogging = (x) => Id;
            ShimUser.AllInstances.LogInGuidBoolean = (x, y, z) => throw new Exception(Exception);
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimListManager.AllInstances.LogUnspecifiedExceptionExceptionStringString = (x, y, z, q) => Id;
            ShimSendResponse.responseStringSendResponseResponseCodeInt32String = (x, y, z, q) => Exception;

            // Act
            var result = _listManager.UpdateList(SampleEcnAccessKey, Id, List, List, Id);

            // Assert
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(Exception);
        }

        [Test]
        public void GetListInternal_ForValidAccessKey_ReturnSuccessResponse()
        {
            // Arrange
            _listManager = new ListManager();
            InitializeList();

            // Act
            var result = _listManager.GetLists_Internal(SampleEcnAccessKey, Id);

            // Arrange
            result.ShouldNotBeNullOrEmpty();
            result.ShouldContain(GroupValue);
        }

        private void InitializeList()
        {
            _dataTable = new DataTable()
            {
                Columns = { "BaseChannelID", "CustomerID", "UserID", "AccountsOptions", "GroupID", "GroupName" },
                Rows = { { Id, Id, Id, "1", Id, Id } }
            };
            _dataTable1 = new DataTable()
            {
                Columns = { "CustomerID", "BaseChannelID", "CustomerName", "ActiveFlag", "WebAddress", "TechContact",
                    "TechEmail", "TechPhone", "SubscriptionsEmail", "CustomerType", "DemoFlag", "AccountsLevel",
                    "Salutation", "ContactName", "ContactTitle", "Phone", "Fax", "Email", "Address", "City", "State",
                    "Country", "Zip", "AccountExecutiveID", "AccountManagerID", "IsStrategic", "customer_udf1",
                    "customer_udf2", "customer_udf3", "customer_udf4", "customer_udf5" },
                Rows = { { Id, Id, Name, Name, Name, Name, Name, Name, Name, Name, Name, Id, Name, Name, Name, Name,
                        Name, Name, Name, Name, Name, Name, Name, Id, Id, true, Name, Name, Name, Name, Name } }
            };
            EcnCommonFakes.ShimDataFunctions.GetDataTableString = (x) => _dataTable1;
            EcnCommonFakes.ShimDataFunctions.GetDataTableStringString = (x, y) => _dataTable;
            var collection = new NameValueCollection()
            {
                { UasMasterAccessKey, SampleEcnAccessKey },
                { Act, Act }
            };
            ShimConfigurationManager.AppSettingsGet = () => collection;
            ShimUser.GetByAccessKeyStringBoolean = (x, y) => new User()
            {
                UserID = Id
            };
            ShimGroup.GetGroupDRInt32Int32User = (x, y, z) => _dataTable;
        }
    }
}
