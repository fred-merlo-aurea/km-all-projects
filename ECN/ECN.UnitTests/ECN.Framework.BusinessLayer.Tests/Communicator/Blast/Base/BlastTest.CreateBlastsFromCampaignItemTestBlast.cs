using System.Collections.Generic;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using Blast = ECN_Framework_BusinessLayer.Communicator.Blast;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class BlastTest
    {
        [TestCase(CampaignItemType.Regular)]
        [TestCase(CampaignItemType.AB)]
        [TestCase(CampaignItemType.Champion)]
        [TestCase(CampaignItemType.SMS)]
        public void CreateBlastsFromCampaignItemTestBlast_Success_CreatesAndUpdatesBlast(CampaignItemType campaignItemType)
        {
            // Arrange
            var blastSaved = false;
            SetupShimsForCreateBlast(campaignItemType);
            ShimBlastExtendedAbstract.AllInstances.SaveBlastAbstractUser = (a, b, c) => 
            {
                blastSaved = true;
                return 0;
            };

            // Act	
            Blast.CreateBlastsFromCampaignItemTestBlast(1, new User());

            // Assert
            blastSaved.ShouldBeTrue();
        }

        [TestCase(CampaignItemType.Regular)]
        [TestCase(CampaignItemType.AB)]
        [TestCase(CampaignItemType.Champion)]
        [TestCase(CampaignItemType.SMS)]
        public void CreateBlastsFromQuickTestBlast_Success_CreatesAndUpdatesBlast(CampaignItemType campaignItemType)
        {
            // Arrange
            var blastSaved = false;
            SetupShimsForCreateBlast(campaignItemType);
            ShimBlastExtendedAbstract.AllInstances.SaveBlastAbstractUser = (a, b, c) =>
            {
                blastSaved = true;
                return 0;
            };

            // Act	
            Blast.CreateBlastsFromQuickTestBlast(1, new User());

            // Assert
            blastSaved.ShouldBeTrue();
        }

        private void SetupShimsForCreateBlast(CampaignItemType campaignItemType)
        {
            var campaignItem = ReflectionHelper.CreateInstance(typeof(CampaignItem));
            campaignItem.CampaignItemType = campaignItemType.ToString();
            var campaignItemTestBlast = ReflectionHelper.CreateInstance(typeof(CampaignItemTestBlast));
            campaignItemTestBlast.CampaignItemTestBlastID = 1;
            campaignItem.TestBlastList = new List<CampaignItemTestBlast> { campaignItemTestBlast };
            ShimCampaignItem.GetByCampaignItemTestBlastIDInt32UserBoolean = (a, b, c) => campaignItem;
            ShimCampaignItemTestBlast.UpdateBlastIDInt32Int32Int32 = (a, b, c) => { };
        }
    }
}