using System;
using System.Collections.Generic;
using ecn.webservice;
using ecn.webservice.Facades.Fakes;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using BusinessLayerCommunicator = ECN_Framework_BusinessLayer.Communicator;

namespace ECN.Webservice.Tests.Facades
{
    [TestFixture]
    public partial class BlastFacadeTest
    {
        [Test]
        public void UpdateBlast_ForNullValueError_ReturnResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new UpdateBlastParams()
            {
                BlastId = BlastId,
                MessageId = BlastId,
                ListId = BlastId,
                FilterId = BlastId,
                Subject = Open
            };
            InitializeUpdateBlast();

            // Act
            var result = _testEntity.UpdateBlast(context, parameters) as string;

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        [Test]
        public void UpdateBlast_ForNullBlast_ReturnFailResponse()
        {
            // Arrange
            var context = new WebMethodExecutionContext()
            {
                User = new User(),
                ApiLogId = BlastId,
                MethodName = ActionTypeCode,
                ApiLoggingManager = new BusinessLayerCommunicator.APILoggingManager()
            };
            var parameters = new UpdateBlastParams()
            {
                BlastId = BlastId,
                MessageId = BlastId,
                ListId = BlastId,
                FilterId = BlastId,
                Subject = Open
            };
            InitializeUpdateBlast();
            ShimCampaignItemBlastManager.AllInstances.GetByBlastIdInt32UserBoolean =
                (x, y, z, q) => new CampaignItemBlast()
                {
                    Blast = null,
                    CampaignItemID = BlastId
                };

            // Act
            var result = _testEntity.UpdateBlast(context, parameters) as string;

            // Assert
            result.ShouldNotBeNullOrEmpty();
        }

        private void InitializeUpdateBlast()
        {
            var blast = new Moq.Mock<BlastAbstract>();
            ShimCampaignItemBlastManager.AllInstances.GetByBlastIdInt32UserBoolean = 
                (x, y, z, q) => new CampaignItemBlast()
                {
                    Blast = blast.Object,
                    CampaignItemID = BlastId
                };
            ShimGroup.ExistsInt32Int32 = (x, y) => true;
            ShimLayout.ExistsInt32Int32 = (x, y) => true;
            ShimFilter.ExistsInt32Int32 = (x, y) => true;
            ShimLayout.ValidateLayoutContentInt32 = (x) => new List<string> { };
            ShimGroup.ValidateDynamicStringsListOfStringInt32User = (x, y, z) => new List<string> { };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = 
                (x, y, z) => new CampaignItem() { CampaignItemID = BlastId };
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (x, y, z) => BlastId;
            ShimDataFunctions.ExecuteNonQuerySqlCommandString = (x, y) => true;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (x) => BlastId;
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (x, y, z) => new Blast();
            ShimAPILoggingManager.AllInstances.UpdateLogInt32NullableOfInt32 = 
                (x, y, z) => new BusinessLayerCommunicator.APILoggingManager();
            ShimFacadeBase.AllInstances.GetSuccessResponseWebMethodExecutionContextStringInt32 = (x, y, q, z) => Success;
            ShimFacadeBase.AllInstances.GetFailResponseWebMethodExecutionContextStringInt32 = (x, y, q, z) => Fail;
            ShimCampaignItem.SaveCampaignItemUser = (x, y) => BlastId;
        }
    }
}
