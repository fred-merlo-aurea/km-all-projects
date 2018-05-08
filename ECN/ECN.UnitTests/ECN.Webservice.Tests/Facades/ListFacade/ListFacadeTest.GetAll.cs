using System;
using System.Data;
using System.Collections.Generic;
using ecn.webservice.Facades.Fakes;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Base.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Accounts;
using KM.Platform.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN.Webservice.Tests.Facades.ListFacade
{
    [TestFixture]
    public partial class ListFacadeTest
    {
        private const string Success = "Success";
        private const string Fail = "Fail";

        [Test]
        public void GetFolders_ForFolder_ReturnSuccessResponse()
        {
            // Arrange
            ShimFolder.GetByTypeInt32StringUser = 
                (x, y, z) => new List<EntitiesCommunicator.Folder> { new EntitiesCommunicator.Folder() };
            Initialize();

            // Act
            var result = _facade.GetFolders(_context);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetListEmailProfilesByEmailAddress_ForDataTable_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new GetListEmailProfilesParams()
            {
                ListId = Id,
                EmailAddress = Name
            };
            InitializeGetEmailList();

            // Act
            var result = _facade.GetListEmailProfilesByEmailAddress(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetCustomFields_ForGroupExistTrue_ReturnSuccessResponse()
        {
            // Arrange
            Initialize();
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimGroupDataFields.GetByGroupIDInt32UserBoolean = 
                (x, y, z) => new List<EntitiesCommunicator.GroupDataFields> { new EntitiesCommunicator.GroupDataFields() };

            // Act
            var result = _facade.GetCustomFields(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetCustomFields_ForGroupExistFalse_ReturnFailResponse()
        {
            // Arrange
            Initialize();
            ShimGroup.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.GetCustomFields(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetFilters_ForGroupExistTrue_ReturnSuccessResponse()
        {
            // Arrange
            Initialize();
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimFilter.GetByGroupIDInt32BooleanUserString = (x, y, z, q) => new List<EntitiesCommunicator.Filter>
            {
                new EntitiesCommunicator.Filter()
            };

            // Act
            var result = _facade.GetFilters(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetFilters_ForGroupExistFalse_ReturnFailResponse()
        {
            // Arrange
            Initialize();
            ShimGroup.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.GetFilters(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetLists_ForGroup_ReturnSuccessResponse()
        {
            // Arrange
            ShimCustomer.GetByClientIDInt32Boolean = (x, y) => new Customer() { CustomerID = Id };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => new DataTable();
            Initialize();

            // Act
            var result = _facade.GetLists(_context);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetListByName_ForGroupName_ReturnSuccessResponse()
        {
            // Arrange
            ShimGroup.GetByGroupNameStringStringUserInt32Int32Int32BooleanStringStringString = 
                (x, y, z, q, w, e, r, t, u, i) => new DataTable();
            Initialize();

            // Act
            var result = _facade.GetListByName(_context, Name);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetListsByFolderId_ForFolderId_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetListByFolderId();

            // Act
            var result = _facade.GetListsByFolderId(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetSubscriberCount_ForSubscriberCount_ReturnSuccessResponse()
        {
            // Arrange
            ShimEmailGroup.GetSubscriberCountInt32Int32User = (x, y, z) => Id;
            Initialize();

            // Act
            var result = _facade.GetSubscriberCount(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        private void InitializeGetListByFolderId()
        {
            Initialize();
            DataLayerCommunicatorFakes.ShimGroup.GetListSqlCommand = (x) => new List<EntitiesCommunicator.Group>
            {
                new EntitiesCommunicator.Group(){ CustomerID = Id }
            };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
        }

        private void Initialize()
        {
            ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 = (x, y, z) => new APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;
        }

        private void InitializeGetEmailList()
        {
            Initialize();
            ShimEmailGroup.GetGroupEmailProfilesWithUDFInt32Int32StringStringString = (x, y, z, q, e) => new DataTable();
        }
    }
}
