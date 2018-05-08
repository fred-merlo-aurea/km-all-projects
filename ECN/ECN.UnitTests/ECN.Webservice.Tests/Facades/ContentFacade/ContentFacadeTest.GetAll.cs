using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Platform.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ECN.Webservice.Tests.Facades.ContentFacade
{
    [TestFixture]
    public partial class ContentFacadeTest
    {
        [Test]
        public void GetContentListByFolderId_ForFolderId_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            // Act
            var result = _facade.GetContentListByFolderId(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetMessageListByFolderId_ForFolderId_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            // Act
            var result = _facade.GetMessageListByFolderId(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetContent_ForContentList_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            // Act
            var result = _facade.GetContent(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetContent_ForNullContentList_ReturnFailResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => null;

            // Act
            var result = _facade.GetContent(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetMessage_ForLayoutList_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            //Act
            var result = _facade.GetMessage(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetMessage_ForNullLayoutList_ReturnFailResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => null;

            //Act
            var result = _facade.GetMessage(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void GetFolders_ForFolderType_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            // Act
            var result = _facade.GetFolders(_context);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetTemplates_ForCustomer_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            // Act
            var result = _facade.GetTemplates(_context);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetMessageTypes_ForCustomer_ReturnSuccessResponse()
        {
            // Arrange
            InitializeGetContentListByFolderId();

            // Act
            var result = _facade.GetMessageTypes(_context);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void GetCustomerDepts_ForCustomer_ReturnSuccessResponse()
        {
            // Arrange
            Initialize();
            ShimCustomerDepartment.GetByCustomerIDInt32Boolean = 
                (x, y) => new List<CustomerDepartment> { new CustomerDepartment() };

            // Act
            var result = _facade.GetCustomerDepts(_context);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        private void InitializeGetContentListByFolderId()
        {
            Initialize();
            ShimLayout.GetByFolderIDCustomerIDInt32UserBoolean = (x, y, z) => new List<Layout> { new Layout() };
            DataLayerCommunicatorFakes.ShimContent.GetByFolderIDCustomerIDInt32Int32String = (x, y, z) => new List<Content>
            {
                new Content()
                {
                    CustomerID = Id
                }
            };
            DataLayerCommunicatorFakes.ShimContent.GetByContentIDInt32 = (x) => new Content() { CustomerID = Id };
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => new Layout();
            ShimFolder.GetByTypeInt32StringUser = (x, y, z) => new List<Folder> { new Folder() };
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => new Customer() { BaseChannelID = Id };
            DataLayerCommunicatorFakes.ShimTemplate.GetListSqlCommand = (x) => new List<Template>
            {
                new Template()
                {
                    BaseChannelID = Id
                }
            };
            ShimMessageType.GetByBaseChannelIDInt32User = (x, y) => new List<MessageType> { new MessageType() };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
        }
    }
}
