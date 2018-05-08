using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class CampaignTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            int campaignID = -1;
            string dripDesign = string.Empty;
            string campaignName = string.Empty;
            bool? isArchived = false;        

            Campaign campaign = new Campaign();    

            campaign.CampaignID.ShouldBe(campaignID);
            campaign.DripDesign.ShouldBe(dripDesign);
            campaign.CustomerID.ShouldBeNull();
            campaign.CreatedUserID.ShouldBeNull();
            campaign.CreatedDate.ShouldBeNull();
            campaign.UpdatedUserID.ShouldBeNull();
            campaign.UpdatedDate.ShouldBeNull();
            campaign.IsDeleted.ShouldBeNull();
            campaign.CampaignName.ShouldBe(campaignName);
            campaign.IsArchived.ShouldBe(isArchived);
            campaign.ItemList.ShouldBeNull();
        }
    }
}