using System;
using NUnit.Framework;
using ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN_Framework_Entities.Communicator.Tests
{
    [TestFixture()]
    public class CampaignItemTemplateTest
    {
        [Test()]
        public void Constructor_InstantiatedWithNoArguments_PropertiesRemainWithDefaultValues()
        {

            int campaignItemTemplateID = -1;
            string templateName = string.Empty;
            string fromName = string.Empty;
            string fromEmail = string.Empty;
            string replyTo = string.Empty;
            string subject = string.Empty;
            string blastField1 = string.Empty;
            string blastField2 = string.Empty;
            string blastField3 = string.Empty;
            string blastField4 = string.Empty;
            string blastField5 = string.Empty;
            string omniture1 = string.Empty;
            string omniture2 = string.Empty;
            string omniture3 = string.Empty;
            string omniture4 = string.Empty;
            string omniture5 = string.Empty;
            string omniture6 = string.Empty;
            string omniture7 = string.Empty;
            string omniture8 = string.Empty;
            string omniture9 = string.Empty;
            string omniture10 = string.Empty;
            bool? archived = false;
            bool? optOutMasterSuppression = false;
            bool? optOutSpecificGroup = false;
            bool? omnitureCustomerSetup = false;        

            CampaignItemTemplate campaignItemTemplate = new CampaignItemTemplate();    

            campaignItemTemplate.CampaignItemTemplateID.ShouldBe(campaignItemTemplateID);
            campaignItemTemplate.TemplateName.ShouldBe(templateName);
            campaignItemTemplate.FromName.ShouldBe(fromName);
            campaignItemTemplate.FromEmail.ShouldBe(fromEmail);
            campaignItemTemplate.ReplyTo.ShouldBe(replyTo);
            campaignItemTemplate.Subject.ShouldBe(subject);
            campaignItemTemplate.BlastField1.ShouldBe(blastField1);
            campaignItemTemplate.BlastField2.ShouldBe(blastField2);
            campaignItemTemplate.BlastField3.ShouldBe(blastField3);
            campaignItemTemplate.BlastField4.ShouldBe(blastField4);
            campaignItemTemplate.BlastField5.ShouldBe(blastField5);
            campaignItemTemplate.Omniture1.ShouldBe(omniture1);
            campaignItemTemplate.Omniture2.ShouldBe(omniture2);
            campaignItemTemplate.Omniture3.ShouldBe(omniture3);
            campaignItemTemplate.Omniture4.ShouldBe(omniture4);
            campaignItemTemplate.Omniture5.ShouldBe(omniture5);
            campaignItemTemplate.Omniture6.ShouldBe(omniture6);
            campaignItemTemplate.Omniture7.ShouldBe(omniture7);
            campaignItemTemplate.Omniture8.ShouldBe(omniture8);
            campaignItemTemplate.Omniture9.ShouldBe(omniture9);
            campaignItemTemplate.Omniture10.ShouldBe(omniture10);
            campaignItemTemplate.CreatedUserID.ShouldBeNull();
            campaignItemTemplate.CreatedDate.ShouldBeNull();
            campaignItemTemplate.UpdatedUserID.ShouldBeNull();
            campaignItemTemplate.UpdatedDate.ShouldBeNull();
            campaignItemTemplate.IsDeleted.ShouldBeNull();
            campaignItemTemplate.CustomerID.ShouldBeNull();
            campaignItemTemplate.Archived.ShouldBe(archived);
            campaignItemTemplate.LayoutID.ShouldBeNull();
            campaignItemTemplate.OptOutMasterSuppression.ShouldBe(optOutMasterSuppression);
            campaignItemTemplate.OptOutSpecificGroup.ShouldBe(optOutSpecificGroup);
            campaignItemTemplate.OmnitureCustomerSetup.ShouldBe(omnitureCustomerSetup);
            campaignItemTemplate.SuppressionGroupList.ShouldBeNull();
            campaignItemTemplate.SelectedGroupList.ShouldBeNull();
            campaignItemTemplate.OptoutGroupList.ShouldBeNull();
        }
    }
}