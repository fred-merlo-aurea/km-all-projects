using System;
using System.Data;
using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using ecn.communicator.mvc.Controllers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ecn.communicator.mvc.Infrastructure.Fakes;
using Shouldly;
using ecn.communicator.mvc.Controllers.Fakes;
using ECN_Framework_Entities.Accounts.Fakes;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        [Test]
        public void AddGlobalSuppression_NoEmail_ReturnJson()
        {
            // Arrange
            InitilizeAddGlobalSuppressionTests();
            var controller = new GroupController();

            // Act
            var result = controller.AddGlobalSuppression(String.Empty) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains("0" + UpdatedMessage));
        }

        [Test]
        public void AddGlobalSuppression_NoDataUpdate_ReturnJson()
        {
            // Arrange
            InitilizeAddGlobalSuppressionTests();
            var controller = new GroupController();
            ShimEmailGroup.ImportEmailsToGlobalMSUserInt32String = (p1, p2, p3) => new DataTable();

            // Act
            var result = controller.AddGlobalSuppression(SampleEmail) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains("0" + UpdatedMessage));
        }

        [Test]
        public void AddGlobalSuppression_UpdateData_ReturnJson()
        {
            // Arrange
            InitilizeAddGlobalSuppressionTests();
            var controller = new GroupController();
            ShimEmailGroup.ImportEmailsToGlobalMSUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { TotalRecordKeyT, "1" }, { TotalRecordKeyT, "2" }, { NewRecordKeyI, "4" },
                    { ChangedRecordKeyU, "5" }, { DublicateRecordKeyD, "6" }, { SkippedRecordKeyS, "7" } }
            };

            // Act
            var result = controller.AddGlobalSuppression(SampleEmail) as JsonResult;

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

        private void InitilizeAddGlobalSuppressionTests()
        {
            ShimChannelNoThresholdList.GetByBaseChannelIDInt32User = (p1, p2) => new List<ChannelNoThresholdList>
            {
                new ChannelNoThresholdList { EmailAddress = SampleEmail}
            };

            ShimGroupController.AllInstances.CurrentBaseChannelGet = (p) => new ShimBaseChannel();
            ShimHtmlHelperMethods.RenderViewToStringControllerContextStringObject = (p1, p2, p3) => String.Empty;
            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
        }
    }
}
