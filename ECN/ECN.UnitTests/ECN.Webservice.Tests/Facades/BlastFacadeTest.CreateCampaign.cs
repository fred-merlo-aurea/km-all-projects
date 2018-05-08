using System;
using ecn.webservice;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Webservice.Tests.Facades
{
    [TestFixture]
    public partial class BlastFacadeTest
    {
        private PrivateType _privateType;
        private PrivateObject _privateObject;
        private const string MethodCreateCampaignItemTestBlast = "CreateCampaignItemTestBlast";
        private const string MethodCreateCampaignItemBlast = "CreateCampaignItemBlast";
        private const string MethodCreateCampaignItem = "CreateCampaignItem";
        private const string MethodGetOrCreateCampaign = "GetOrCreateCampaign";

        [Test]
        public void CreateCampaignItemTestBlast_ForCampaignItem_ReturnCampaignItemTestBlast()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };
            var parameters = new AddBlastParams() { ListId = BlastId, FilterId = BlastId };
            var campaignItem = new CampaignItem() { CampaignItemID = BlastId };
            _privateType = new PrivateType(typeof(BlastFacade));
            var param = new object[] { context, parameters, campaignItem };
            InitializeCampaignItem();

            // Act
            var result = _privateType.InvokeStatic(MethodCreateCampaignItemTestBlast, param) as CampaignItemTestBlast;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.CampaignItemID.ShouldBe(BlastId),
                () => result.GroupID.ShouldBe(BlastId));
        }

        [Test]
        public void CreateCampaignItemBlast_ForSmartSegment_ReturnCampaignItemBlast()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };
            var parameters = new AddBlastParams() { ListId = BlastId, FilterId = BlastId, RefBlasts = Open };
            var campaignItem = new CampaignItem() { CampaignItemID = BlastId };
            _privateObject = new PrivateObject(_testEntity);
            var param = new object[] { context, parameters, BlastId, true, campaignItem };
            InitializeCampaignItemBlast();
            InitializeCampaignItem();

            // Act
            var result = _privateObject.Invoke(MethodCreateCampaignItemBlast, param) as CampaignItemBlast;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.CampaignItemID.ShouldBe(BlastId),
                () => result.GroupID.ShouldBe(BlastId));
        }

        [Test]
        public void CreateCampaignItemBlast_ForSmartSegmentFalse_ReturnCampaignItemBlast()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };
            var parameters = new AddBlastParams() { ListId = BlastId, FilterId = BlastId, RefBlasts = Open };
            var campaignItem = new CampaignItem() { CampaignItemID = BlastId };
            _privateObject = new PrivateObject(_testEntity);
            var param = new object[] { context, parameters, BlastId, false, campaignItem };
            InitializeCampaignItemBlast();
            InitializeCampaignItem();

            // Act
            var result = _privateObject.Invoke(MethodCreateCampaignItemBlast, param) as CampaignItemBlast;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.CampaignItemID.ShouldBe(BlastId),
                () => result.GroupID.ShouldBe(BlastId));
        }

        [Test]
        public void CreateCampaignItem_ForIsTest_ReturnCampaign()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };
            var parameters = new AddBlastParams() { ListId = BlastId, FilterId = BlastId, RefBlasts = Open, IsTest = true };
            var campaign = new Campaign() { CampaignID = BlastId };
            _privateType = new PrivateType(typeof(BlastFacade));
            var param = new object[] { context, parameters, campaign };
            ShimCampaignItem.SaveCampaignItemUser = (x, y) => BlastId;

            // Act
            var result = _privateType.InvokeStatic(MethodCreateCampaignItem, param) as CampaignItem;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.CustomerID.ShouldBe(BlastId));
        }

        [Test]
        public void GetOrCreateCampaign_WhenCampaignIsNotNull_ReturnCampaign()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User() { CustomerID = BlastId },
                ApiLogId = BlastId,
                MethodName = ActionTypeCode
            };
            _privateObject = new PrivateObject(_testEntity);
            ShimCampaign.GetByCampaignNameStringUserBoolean = (x, y, z) => new Campaign() { CampaignID = 0 };
            ShimCampaign.SaveCampaignUser = (x, y) => BlastId;

            // Act
            var result = _privateObject.Invoke(MethodGetOrCreateCampaign, new object[] { context }) as Campaign;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.CustomerID.ShouldBe(BlastId));
        }

        private void InitializeCampaignItem()
        {
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (x, y, z) => BlastId;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => BlastId;
        }

        private void InitializeCampaignItemBlast()
        {
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (x, y, z) => BlastId;
        }
    }
}
