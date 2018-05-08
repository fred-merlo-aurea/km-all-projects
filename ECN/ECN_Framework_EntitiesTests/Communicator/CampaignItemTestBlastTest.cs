using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class CampaignItemTestBlastTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            int campaignItemTestBlastID = -1;
            bool? hasEmailPreview = false;
            string campaignItemTestBlastType = "HTML";
            string emailSubject = string.Empty;
            string fromName = string.Empty;
            string fromEmail = string.Empty;
            string replyTo = string.Empty;        

            CampaignItemTestBlast campaignItemTestBlast = new CampaignItemTestBlast();    

            campaignItemTestBlast.CampaignItemTestBlastID.ShouldBe(campaignItemTestBlastID);
            campaignItemTestBlast.CampaignItemID.ShouldBeNull();
            campaignItemTestBlast.GroupID.ShouldBeNull();
            campaignItemTestBlast.FilterID.ShouldBeNull();
            campaignItemTestBlast.HasEmailPreview.ShouldBe(hasEmailPreview);
            campaignItemTestBlast.BlastID.ShouldBeNull();
            campaignItemTestBlast.CreatedUserID.ShouldBeNull();
            campaignItemTestBlast.CreatedDate.ShouldBeNull();
            campaignItemTestBlast.UpdatedUserID.ShouldBeNull();
            campaignItemTestBlast.UpdatedDate.ShouldBeNull();
            campaignItemTestBlast.IsDeleted.ShouldBeNull();
            campaignItemTestBlast.CustomerID.ShouldBeNull();
            campaignItemTestBlast.CampaignItemTestBlastType.ShouldBe(campaignItemTestBlastType);
            campaignItemTestBlast.LayoutID.ShouldBeNull();
            campaignItemTestBlast.EmailSubject.ShouldBe(emailSubject);
            campaignItemTestBlast.FromName.ShouldBe(fromName);
            campaignItemTestBlast.FromEmail.ShouldBe(fromEmail);
            campaignItemTestBlast.ReplyTo.ShouldBe(replyTo);
            campaignItemTestBlast.Blast.ShouldBeNull();
            campaignItemTestBlast.RefBlastList.ShouldBeNull();
            campaignItemTestBlast.Filters.ShouldBeNull();
        }
    }
}