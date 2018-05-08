using System;
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

namespace ECN.Webservice.Tests.Facades.ContentFacade
{
    [TestFixture]
    public partial class ContentFacadeTest
    {
        private const string ContentValue = "source";
        private const string Success = "Success";
        private const string Fail = "Fail";

        [Test]
        public void AddContent_ForContentExistFalse_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new ContentParams()
            {
                FolderId = 0,
                Title = MethodName
            };
            InitializeAddContent();

            // Act
            var result = _facade.AddContent(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void AddContent_ForContentExistTrue_ReturnFailResponse()
        {
            // Arrange
            var parameters = new ContentParams()
            {
                FolderId = 0,
                Title = MethodName
            };
            InitializeAddContent();
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => Id;

            // Act
            var result = _facade.AddContent(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddContent_ForFolderExistFalse_ReturnFailResponse()
        {
            // Arrange
            var parameters = new ContentParams()
            {
                FolderId = Id,
                Title = MethodName
            };
            InitializeAddContent();
            ShimFolder.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.AddContent(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddMessage_ForTemplate_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                FolderId = 0,
                MessageTypeId = Id,
                TemplateId = Id,
                LayoutName = ContentValue
            };
            InitializeAddMessage();

            // Act
            var result = _facade.AddMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void AddMessage_ForTemplateFalse_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                FolderId = 0,
                MessageTypeId = Id,
                TemplateId = Id,
                LayoutName = ContentValue
            };
            InitializeAddMessage();
            ShimTemplate.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.AddMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddMessage_ForMessageExistFalse_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                FolderId = 0,
                MessageTypeId = Id,
                TemplateId = Id,
                LayoutName = ContentValue
            };
            InitializeAddMessage();
            ShimMessageType.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.AddMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddMessage_ForLayoutExistTrue_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                FolderId = 0,
                MessageTypeId = Id,
                TemplateId = Id,
                LayoutName = ContentValue
            };
            InitializeAddMessage();
            ShimLayout.ExistsInt32StringInt32Int32 = (x, y, z, q) => true;

            // Act
            var result = _facade.AddMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddMessage_ForFolderExistTrue_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                FolderId = Id,
                MessageTypeId = Id,
                TemplateId = Id,
                LayoutName = ContentValue
            };
            InitializeAddMessage();
            ShimFolder.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.AddMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddFolder_ForFolderExistFalse_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new FolderParams()
            {
                ParentFolderId = Id,
                FolderName = ContentValue
            };
            InitializeAddFolder();

            // Act
            var result = _facade.AddFolder(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void AddFolder_ForFolderExistTrue_ReturnFailResponse()
        {
            // Arrange
            var parameters = new FolderParams()
            {
                ParentFolderId = Id,
                FolderName = ContentValue
            };
            InitializeAddFolder();
            ShimFolder.ExistsInt32StringInt32Int32String = (x, y, z, q, e) => true;

            // Act
            var result = _facade.AddFolder(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void AddFolder_ForFolderIdFalse_ReturnFailResponse()
        {
            // Arrange
            var parameters = new FolderParams()
            {
                ParentFolderId = Id,
                FolderName = ContentValue
            };
            InitializeAddFolder();
            ShimFolder.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.AddFolder(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        private void InitializeAddFolder()
        {
            Initialize();
            ShimFolder.ExistsInt32Int32 = (x, y) => true;
            ShimFolder.ExistsInt32StringInt32Int32String = (x, y, z, q, e) => false;
            ShimFolder.ValidateFolder = (x) => new Folder();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => Id;
        }

        private void InitializeAddMessage()
        {
            Initialize();
            ShimFolder.ExistsInt32Int32 = (x, y) => true;
            DataLayerCommunicatorFakes.ShimLayout.ExistsInt32StringInt32Int32 = (x, y, z, q) => false;
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => new Customer() { BaseChannelID = Id };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => Id;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimCustomer.ExistsInt32 = (x) => true;
            ShimUserValidation.InvalidateIUserValidate = (x) => string.Empty;
        }

        private void InitializeAddContent()
        {
            Initialize();
            ShimFolder.ExistsInt32Int32 = (x, y) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            ShimContent.ReadyContentContentBoolean = (x, y) => new Content();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimContent.ValidateContentUser = (x, y) => new Content();
            ShimContent.CreateUniqueLinkIDsString = (x) => ContentValue;
        }

        private void Initialize()
        {
            ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 = (x, y, z) => new APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Success;
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, z, q) => Fail;
        }
    }
}
