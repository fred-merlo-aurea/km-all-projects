using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class CampaignItemTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            int campaignItemID = -1;
            string campaignItemName = string.Empty;
            string campaignItemFormatType = string.Empty;
            string nodeID = string.Empty;
            string fromName = string.Empty;
            string fromEmail = string.Empty;
            string replyTo = string.Empty;
            string blastField1 = string.Empty;
            string blastField2 = string.Empty;
            string blastField3 = string.Empty;
            string blastField4 = string.Empty;
            string blastField5 = string.Empty;
            string campaignItemType = string.Empty;
            string campaignItemNameOriginal = string.Empty;
            bool? ignoreSuppression = false;        

            CampaignItem campaignItem = new CampaignItem();    

            campaignItem.CampaignItemID.ShouldBe(campaignItemID);
            campaignItem.CampaignItemTemplateID.ShouldBeNull();
            campaignItem.CampaignID.ShouldBeNull();
            campaignItem.CampaignItemName.ShouldBe(campaignItemName);
            campaignItem.CampaignItemFormatType.ShouldBe(campaignItemFormatType);
            campaignItem.NodeID.ShouldBe(nodeID);
            campaignItem.FromName.ShouldBe(fromName);
            campaignItem.FromEmail.ShouldBe(fromEmail);
            campaignItem.ReplyTo.ShouldBe(replyTo);
            campaignItem.SendTime.ShouldBeNull();
            campaignItem.SampleID.ShouldBeNull();
            campaignItem.PageWatchID.ShouldBeNull();
            campaignItem.BlastScheduleID.ShouldBeNull();
            campaignItem.OverrideAmount.ShouldBeNull();
            campaignItem.OverrideIsAmount.ShouldBeNull();
            campaignItem.BlastField1.ShouldBe(blastField1);
            campaignItem.BlastField2.ShouldBe(blastField2);
            campaignItem.BlastField3.ShouldBe(blastField3);
            campaignItem.BlastField4.ShouldBe(blastField4);
            campaignItem.BlastField5.ShouldBe(blastField5);
            campaignItem.CompletedStep.ShouldBeNull();
            campaignItem.CampaignItemType.ShouldBe(campaignItemType);
            campaignItem.CreatedUserID.ShouldBeNull();
            campaignItem.CreatedDate.ShouldBeNull();
            campaignItem.UpdatedUserID.ShouldBeNull();
            campaignItem.UpdatedDate.ShouldBeNull();
            campaignItem.IsDeleted.ShouldBeNull();
            campaignItem.CustomerID.ShouldBeNull();
            campaignItem.SFCampaignID.ShouldBeNull();
            campaignItem.IsHidden.ShouldBeNull();
            campaignItem.CampaignItemNameOriginal.ShouldBe(campaignItemNameOriginal);
            campaignItem.CampaignItemIDOriginal.ShouldBeNull();
            campaignItem.EnableCacheBuster.ShouldBeNull();
            campaignItem.IgnoreSuppression.ShouldBe(ignoreSuppression);
            campaignItem.BlastList.ShouldBeNull();
            campaignItem.TestBlastList.ShouldBeNull();
            campaignItem.SuppressionList.ShouldBeNull();
            campaignItem.OptOutGroupList.ShouldBeNull();
        }
    }
}