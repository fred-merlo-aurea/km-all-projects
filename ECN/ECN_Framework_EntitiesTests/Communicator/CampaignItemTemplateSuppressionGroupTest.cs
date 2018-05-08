using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class CampaignItemTemplateSuppressionGroupTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            int campaignItemTemplateSuppressionGroupID = -1;        

            CampaignItemTemplateSuppressionGroup campaignItemTemplateSuppressionGroup = new CampaignItemTemplateSuppressionGroup();    

            campaignItemTemplateSuppressionGroup.CampaignItemTemplateSuppressionGroupID.ShouldBe(campaignItemTemplateSuppressionGroupID);
            campaignItemTemplateSuppressionGroup.CampaignItemTemplateID.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.GroupID.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.CreatedUserID.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.CreatedDate.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.UpdatedUserID.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.UpdatedDate.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.IsDeleted.ShouldBeNull();
            campaignItemTemplateSuppressionGroup.CustomerID.ShouldBeNull();
        }
    }
}