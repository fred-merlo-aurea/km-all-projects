using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using NUnit.Framework;
using ecn.communicator.mvc.Controllers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using ecn.communicator.mvc.Infrastructure.Fakes;
using Shouldly;
using ecn.communicator.mvc.Controllers.Fakes;
using ECN_Framework_Entities.Accounts.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using BusinessLogicFakes =  KMPlatform.BusinessLogic.Fakes;
using BusinessLayerAccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using DataLayerCommunicatorFakes = ECN_Framework_DataLayer.Communicator.Fakes;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        private const string RbType = "Channel";

        [Test]
        public void AddChannelSuppression_NoEmail_ReturnJson()
        {
            // Arrange
            InitilizeAddChannelSuppressionTests();
            var controller = new GroupController();

            // Act
            var result = controller.AddChannelSuppression(String.Empty) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains("0" + UpdatedMessage));
        }

        [Test]
        public void AddChannelSuppression_NoDataUpdate_ReturnJson()
        {
            // Arrange
            InitilizeAddChannelSuppressionTests();
            var controller = new GroupController();
            ShimEmailGroup.ImportEmailsToCSUserInt32String = (p1, p2, p3) => new DataTable();

            // Act
            var result = controller.AddChannelSuppression(SampleEmail) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains("0" + UpdatedMessage));
        }

        [Test]
        public void AddChannelSuppression_UpdateData_ReturnJson()
        {
            // Arrange
            InitilizeAddChannelSuppressionTests();
            var controller = new GroupController();
            ShimEmailGroup.ImportEmailsToCSUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { TotalRecordKeyT, "1" }, { TotalRecordKeyT, "2" }, { NewRecordKeyI, "4" },
                    { ChangedRecordKeyU, "5" }, { DublicateRecordKeyD, "6" }, { SkippedRecordKeyS, "7" } }
            };

            // Act
            var result = controller.AddChannelSuppression(SampleEmail) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains(TotalRecordsMessage + "3"));
            Assert.IsTrue(jsonResult[1].Contains(NewRecordsMessage + "4"));
            Assert.IsTrue(jsonResult[1].Contains(ChangedMessage + "5"));
            Assert.IsTrue(jsonResult[1].Contains(DublicateMessage + "6"));
            Assert.IsTrue(jsonResult[1].Contains(SkippedMessage + "7"));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Suppression_DataSet_ReturnActionResult(bool param)
        {
            // Arrange
            var controller = new GroupController();
            InitializeSuppression(param);

            // Act
            var result = controller.Suppression() as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.View.ShouldBeNull(),
                () => result.MasterName.ShouldBe(string.Empty));
        }

        [Test]
        public void AddDomainSuppression_ForCurrentUser_ReturnActionResult()
        {
            // Arrange
            var controller = new GroupController();
            InitializeDomain();

            // Act
            var result = controller.AddDomainSuppression(GroupId, RbType, Text) as JsonResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Data.ShouldNotBeNull());
        }

        private void InitializeDomain()
        {
            CreateEcnSession();
            CreateCurrentUser();
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => GroupId;
            BusinessLayerAccountsFakes.ShimBaseChannel.ExistsInt32 = (x) => true;
            BusinessLogicFakes.ShimUser.GetByUserIDInt32Boolean = (x, y) => new User();
            ShimUser.IsSystemAdministratorUser = (x) => true;
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            ShimDomainSuppression.GetByDomainStringNullableOfInt32NullableOfInt32User = 
                (x, y, z, q) => new List<DomainSuppression> { new DomainSuppression() };
            ShimControllerBase.AllInstances.ControllerContextGet = (x) => new ControllerContext();
            ShimHtmlHelperMethods.RenderViewToStringControllerContextStringObject = (x, y, z) => FileName;
        }

        private void InitializeSuppression(bool param)
        {
            CreateCurrentUser();
            DataLayerCommunicatorFakes.ShimGroup.GetMasterSuppressionGroupInt32 = 
                (x) => new Group() { CustomerID = GroupId, GroupID = GroupId };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (x, y, z, a) => true;
            ShimEmailGroup.GetBySearchStringPagingInt32Int32Int32Int32DateTimeDateTimeBooleanStringStringString = 
                (q, w, e, r, t, y, u, i, o, p) => new DataSet()
                {
                    Tables = {"Table", "Table1"}
                };
            if (param)
            {
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group() { MasterSupression = GroupId };
            }
            else
            {
                ShimGroup.GetByGroupIDInt32User = (x, y) => new Group();
            }
        }

        private void InitilizeAddChannelSuppressionTests()
        {
            ShimChannelMasterSuppressionList.GetByBaseChannelIDInt32User = (p1, p2) => new List<ChannelMasterSuppressionList>
            {
                new ChannelMasterSuppressionList { EmailAddress = SampleEmail}
            };

            ShimGroupController.AllInstances.CurrentBaseChannelGet = (p) => new ShimBaseChannel();
            ShimHtmlHelperMethods.RenderViewToStringControllerContextStringObject = (p1, p2, p3) => String.Empty;
            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
        }
    }
}
