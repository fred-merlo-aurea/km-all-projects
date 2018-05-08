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
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Application;

namespace ECN.Communicator.MVC.Tests
{
    public partial class GroupControllerTest
    {
        [Test]
        public void AddNoThresholdSuppression_NoEmail_ReturnJson()
        {
            // Arrange
            InitilizeAddNoThresholdSuppressionTests();
            var controller = new GroupController();

            // Act
            var result = controller.AddNoThresholdSuppression(String.Empty) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains("0" + UpdatedMessage));
        }

        [Test]
        public void AddNoThresholdSuppression_NoDataUpdate_ReturnJson()
        {
            // Arrange
            InitilizeAddNoThresholdSuppressionTests();
            var controller = new GroupController();
            ShimEmailGroup.ImportEmailsToNoThresholdUserInt32String = (p1, p2, p3) => new DataTable();

            // Act
            var result = controller.AddNoThresholdSuppression(SampleEmail) as JsonResult;

            // Assert
            result.ShouldNotBeNull();
            result.Data.ShouldNotBeNull();
            dynamic jsonResult = result.Data;
            Assert.AreEqual(jsonResult.Count, 3);
            Assert.AreEqual(jsonResult[0], HttpStatusOk);
            Assert.IsTrue(jsonResult[1].Contains("0" + UpdatedMessage));
        }

        [Test]
        public void AddNoThresholdSuppression_UpdateData_ReturnJson()
        {
            // Arrange
            InitilizeAddNoThresholdSuppressionTests();
            var controller = new GroupController();
            ShimEmailGroup.ImportEmailsToNoThresholdUserInt32String = (p1, p2, p3) => new DataTable
            {
                Columns = { "Action", "Counts" },
                Rows = { { TotalRecordKeyT, "1" }, { TotalRecordKeyT, "2" }, { NewRecordKeyI, "4" },
                    { ChangedRecordKeyU, "5" }, { DublicateRecordKeyD, "6" }, { SkippedRecordKeyS, "7" } }
            };

            // Act
            var result = controller.AddNoThresholdSuppression(SampleEmail) as JsonResult;

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

        private void InitilizeAddNoThresholdSuppressionTests()
        {
            ShimChannelNoThresholdList.GetByBaseChannelIDInt32User = (p1, p2) => new List<ChannelNoThresholdList>
            {
                new ChannelNoThresholdList { EmailAddress = SampleEmail}
            };

            ShimGroupController.AllInstances.CurrentBaseChannelGet = (p) => new ShimBaseChannel();
            ShimHtmlHelperMethods.RenderViewToStringControllerContextStringObject = (p1, p2, p3) => String.Empty;
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentBaseChannel = new ShimBaseChannel();
                return session;
            };
            ShimConvenienceMethods.GetCurrentUser = () => new KMPlatform.Entity.User();
        }
    }
}
