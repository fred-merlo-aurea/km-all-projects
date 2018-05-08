using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
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
        [TestCase("cnt")]
        [TestCase("grp")]
        public void DeleteFolder_ForFolderId_ReturnSuccessResponse(string folderType)
        {
            // Arrange
            InitializeDelete(folderType);

            // Act
            var result = _facade.DeleteFolder(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void DeleteContent_ForContentId_ReturnSuccessResponse()
        {
            // Arrange
            InitializeDeleteContent();

            // Act
            var result = _facade.DeleteContent(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        [Test]
        public void DeleteMessage_ForMessageId_ReturnSuccessResponse()
        {
            // Arrange
            InitializeDeleteMessage();

            // Act
            var result = _facade.DeleteMessage(_context, Id);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldBe(Success));
        }

        private void InitializeDeleteMessage()
        {
            Initialize();
            ShimLayout.ExistsInt32Int32 = (x, y) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (x, y, z) => new Layout();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
        }

        private void InitializeDeleteContent()
        {
            Initialize();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, q) => true;
            ShimContent.ExistsInt32Int32 = (x, y) => true;
            DataLayerCommunicatorFakes.ShimLayout.ContentUsedInLayoutInt32 = (x) => false;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            ShimContent.GetByContentIDInt32UserBoolean = (x, y, z) => new Content();
            ShimLinkAlias.ExistsInt32Int32 = (x, y) => false;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
        }

        private void InitializeDelete(string folderType)
        {
            Initialize();
            ShimFolder.ExistsInt32Int32 = (x, y) => true;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => 0;
            DataLayerCommunicatorFakes.ShimFolder.GetSqlCommand = (x) => new Folder()
            {
                CustomerID = Id,
                FolderType = folderType
            };
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
        }
    }
}
