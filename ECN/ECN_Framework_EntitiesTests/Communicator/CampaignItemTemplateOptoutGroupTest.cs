using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class CampaignItemTemplateOptoutGroupTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            const int campaignItemTemplateOptOutGroupID = -1;
            const int campaignItemTemplateID = -1;
            const int groupID = -1;
            const bool isDeleted = false;        

            var campaignItemTemplateOptoutGroup = new CampaignItemTemplateOptoutGroup();    

            campaignItemTemplateOptoutGroup.CampaignItemTemplateOptOutGroupID.ShouldBe(campaignItemTemplateOptOutGroupID);
            campaignItemTemplateOptoutGroup.CampaignItemTemplateID.ShouldBe(campaignItemTemplateID);
            campaignItemTemplateOptoutGroup.GroupID.ShouldBe(groupID);
            campaignItemTemplateOptoutGroup.IsDeleted.ShouldBe(isDeleted);
            campaignItemTemplateOptoutGroup.CreatedDate.ShouldBeNull();
            campaignItemTemplateOptoutGroup.CreatedUserID.ShouldBeNull();
            campaignItemTemplateOptoutGroup.UpdatedDate.ShouldBeNull();
            campaignItemTemplateOptoutGroup.UpdatedUserID.ShouldBeNull();
        }
    }
}