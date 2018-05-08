using System;
using System.Collections.Generic;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Webservice.Tests.Facades.ContentFacade
{
    [TestFixture]
    public partial class ContentFacadeTest
    {
        private const string MethodRemoveTroublesomeCharacters = "RemoveTroublesomeCharacters";
        private const string MethodBuildLayoutReturnXML = "BuildLayoutReturnXML";
        private const string XmlString = "<>";

        [Test]
        public void UpdateContent_ForContent_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new ContentParams()
            {
                ContentId = Id,
                Title = MethodName
            };
            InitializeUpdateContent();

            // Act
            var result = _facade.UpdateContent(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void UpdateContent_ForContentExistTrue_ReturnFailResponse()
        {
            // Arrange
            var parameters = new ContentParams()
            {
                ContentId = Id,
                Title = MethodName
            };
            InitializeUpdateContent();
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => Id;

            // Act
            var result = _facade.UpdateContent(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void UpdateContent_ForNullContent_ReturnFailResponse()
        {
            // Arrange
            var parameters = new ContentParams()
            {
                ContentId = Id,
                Title = MethodName
            };
            InitializeUpdateContent();
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => null;

            // Act
            var result = _facade.UpdateContent(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void UpdateMessage_ForTemplate_ReturnSuccessResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                MessageId = Id,
                Content1 = Id,
                Content2 = Id,
                Content3 = Id,
                Content4 = Id,
                Content5 = Id,
                Content6 = Id,
                Content7 = Id,
                Content0 = Id,
                Content8 = Id
            };
            InitializeUpdateMessage();

            // Act
            var result = _facade.UpdateMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void UpdateMessage_ForTemplateExistFalse_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                MessageId = Id
            };
            InitializeUpdateMessage();
            ShimTemplate.ExistsInt32Int32 = (x, y) => false;

            // Act
            var result = _facade.UpdateMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void UpdateMessage_ForLayoutExist_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                MessageId = Id
            };
            InitializeUpdateMessage();
            ShimLayout.ExistsInt32StringInt32Int32 = (x, y, z, q) => true;

            // Act
            var result = _facade.UpdateMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void UpdateMessage_ForNullLayout_ReturnFailResponse()
        {
            // Arrange
            var parameters = new MessageParams()
            {
                MessageId = Id
            };
            InitializeUpdateMessage();
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => null;

            // Act
            var result = _facade.UpdateMessage(_context, parameters);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Fail));
        }

        [Test]
        public void RemoveTroublesomeCharacters_ForEmptyString_ReturnNull()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);

            // Act
            var result = privateObject.Invoke(MethodRemoveTroublesomeCharacters, new object[] { null });

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void RemoveTroublesomeCharacters_ForXmlString_ReturnXml()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);

            // Act
            var result = privateObject.Invoke(MethodRemoveTroublesomeCharacters, new object[] { XmlString });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBe(XmlString));
        }

        [Test]
        public void BuildLayoutReturnXML_ForLayoutList_ReturnValue()
        {
            // Arrange
            var privateObject = new PrivateObject(_facade);
            var layoutList = new List<Layout>
            {
                new Layout()
                {
                    LayoutName =MethodName,
                    FolderID = Id,
                    TemplateID = Id,
                    UpdatedDate = new DateTime(),
                    CreatedDate = new DateTime(),
                    ContentSlot1 = Id,
                    ContentSlot2 = Id,
                    ContentSlot3 = Id,
                    ContentSlot4 = Id,
                    ContentSlot5 = Id,
                    ContentSlot6 = Id,
                    ContentSlot7 = Id,
                    ContentSlot8 = Id,
                    ContentSlot9 = Id
                }
            };

            // Act
            var result = privateObject.Invoke(MethodBuildLayoutReturnXML, new object[] { layoutList }) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldContain(MethodName));
        }

        private void InitializeUpdateMessage()
        {
            Initialize();
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => new Layout()
            {
                FolderID = Id
            };
            ShimLayout.ExistsInt32StringInt32Int32 = (x, y, z, q) => false;
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => new Customer() { BaseChannelID = Id };
            ShimTemplate.ExistsInt32Int32 = (x, y) => true;
            ShimLayout.SaveLayoutUser = (x, y) => new Layout();
        }

        private void InitializeUpdateContent()
        {
            Initialize();
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => new Content()
            {
                FolderID = Id,
                UpdatedUserID = Id,
                ContentSource = ContentValue,
                ContentMobile = ContentValue,
                ContentText = ContentValue,
                ContentTitle = ContentValue,
                ContentID = Id
            };
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            ShimContent.ReadyContentContentBoolean = (x, y) => new Content();
            ShimContent.SaveContentUser = (x, y) => Id;
        }
    }
}
