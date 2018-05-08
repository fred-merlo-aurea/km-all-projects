using System.Data;
using System.Collections.Generic;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using DataLayerCommunicator = ECN_Framework_DataLayer.Communicator;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ECN.Webservice.Tests.Facades.ListFacade
{
    [TestFixture]
    public partial class ListFacadeTest
    {
        [Test]
        [TestCase("cnt")]
        [TestCase("grp")]
        public void DeleteFolder_ForFolderId_ReturnSuccessResponse(string param)
        {
            // Arrange
            InitializeDeleteFolder(param);

            // Act
            var result = _facade.DeleteFolder(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void DeleteFolder_ForNullFolder_ReturnFailResponse()
        {
            // Arrange
            InitializeDeleteFolder("cnt");
            ShimFolder.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.DeleteFolder(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void DeleteList_ForGroupExist_ReturnSuccessResponse()
        {
            // Arrange
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimGroup.DeleteInt32User = (x, y) => new Group();
            Initialize();

            // Act
            var result = _facade.DeleteList(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void DeleteList_ForGroupExistFalse_ReturnFailResponse()
        {
            // Arrange
            ShimGroup.ExistsInt32Int32 = (x, y) => false;
            ShimGroup.DeleteInt32User = (x, y) => new Group();
            Initialize();

            // Act
            var result = _facade.DeleteList(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void DeleteSubscriber_ForEmailGroup_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new DeleteSubscriberParams();
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x, y, z) => new EmailGroup() { EmailID = Id };
            ShimEmailGroup.DeleteInt32Int32User = (x, y, z) => new EmailGroup();
            Initialize();

            // Act
            var result = _facade.DeleteSubscriber(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void DeleteSubscriber_ForNullEmailGroup_ReturnFailResponse()
        {
            // Arrange
            var parameters = new DeleteSubscriberParams();
            ShimEmailGroup.GetByEmailAddressGroupIDStringInt32User = (x, y, z) => null;
            ShimEmailGroup.DeleteInt32Int32User = (x, y, z) => new EmailGroup();
            Initialize();

            // Act
            var result = _facade.DeleteSubscriber(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void DeleteCustomField_ForGroupDataField_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new DeleteCustomFieldParams();
            InitializeDeleteCustomField();

            // Act
            var result = _facade.DeleteCustomField(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void DeleteCustomField_ForGroupDataFieldFalse_ReturnFailResponse()
        {
            // Arrange
            var parameters = new DeleteCustomFieldParams();
            InitializeDeleteCustomField();
            ShimGroupDataFields.ExistsInt32Int32Int32 = (x, y, z) => false;

            // Act
            var result = _facade.DeleteCustomField(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        private void InitializeDeleteCustomField()
        {
            ShimGroupDataFields.ExistsInt32Int32 = (x, y) => true;
            ShimGroupDataFields.ExistsInt32Int32Int32 = (x, y, z) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            DataLayerCommunicatorFakes.ShimGroupDataFields.GetByIDInt32 = (x) => new GroupDataFields()
            {
                CustomerID = Id,
                ShortName = Name
            };
            ShimGroupDataFields.IsReservedWordString = (x) => false;
            DataLayerCommunicatorFakes.ShimBlast.GetByGroupIDInt32 = (x) => null;
            DataLayerCommunicatorFakes.ShimEmailDataValues.DeleteInt32Int32Int32Int32 = 
                (x, y, z, q) => new DataLayerCommunicator.EmailDataValues();
            DataLayerCommunicatorFakes.ShimGroupDataFields.DeleteInt32Int32Int32Int32 = 
                (x, y, z, q) => new DataLayerCommunicator.GroupDataFields();
            DataLayerCommunicatorFakes.ShimEmailDataValues.GetListSqlCommand = 
                (x) => new List<EmailDataValues> { new EmailDataValues() { CustomerID = Id } };
            Initialize();
        }

        private void InitializeDeleteFolder(string param)
        {
            Initialize();
            ShimFolder.ExistsInt32Int32 = (x, y) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            DataLayerCommunicatorFakes.ShimFolder.GetByFolderIDInt32 = (x) => new Folder()
            {
                CustomerID = Id,
                FolderType = param
            };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
        }
    }
}
