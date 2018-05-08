using System.Collections.Generic;
using ECN.Framework.BusinessLayer.Tests.DTO;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class QuickTestBlastTest
    {
        public ValidateDTO GetValidateTestCase1()
        {
            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => new Campaign { IsArchived = true };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => false;
            ShimLayout.ExistsInt32Int32 = (u, c) => true;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };

            var paramObj = GetDefaultValidateDTO();
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase2()
        {

            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AllowAdhocEmails = false;
            paramObj.CampaignItemName = "SampleCampagin";
            paramObj.CampaignItemID = 0;
            paramObj.CampaignItemName = new string('T', 51);
            paramObj.GroupName = "SampleGroupName";
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase3()
        {

            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = new string('T', 51);
            paramObj.GroupID = 1;
            paramObj.LayoutID = 0;
            paramObj.GroupName = "SampleGroupName";
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase4()
        {

            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 1;
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase5()
        {

            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 0;
            paramObj.EmailsToAdd = string.Empty;
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase6()
        {

            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => false;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 1;
            paramObj.EmailsToAdd = string.Empty;
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase7()
        {
            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimGroup.IsArchivedInt32Int32 = (u, c) => true;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 1;
            paramObj.EmailsToAdd = string.Empty;
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase8()
        {
            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimGroup.IsArchivedInt32Int32 = (u, c) => false;
            ShimEmailGroup.GetSubscriberCountInt32Int32User = (c, b, u) => 11;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 1;
            paramObj.EmailsToAdd = string.Empty;
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase9()
        {
            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 10 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimGroup.IsArchivedInt32Int32 = (u, c) => false;
            ShimEmailGroup.GetSubscriberCountInt32Int32User = (c, b, u) => 11;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = true;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.GroupName = "SampleGroupName";
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 0;
            paramObj.EmailsToAdd = string.Empty;
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        public ValidateDTO GetValidateTestCase10()
        {
            ShimCustomer.GetByCustomerIDInt32Boolean = (c, b) => new Customer { TestBlastLimit = 1 };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (b) => new BaseChannel { TestBlastLimit = 10 };
            ShimEmail.IsValidEmailAddressString = (e) => false;
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (c, u, b) => null;
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (c, u, b) => null;
            ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (c, e, s) => true;
            ShimLayout.ExistsInt32Int32 = (u, c) => false;
            ShimLayout.GetByLayoutIDInt32UserBoolean = (c, b, u) => new Layout { Archived = true };
            ShimGroup.ExistsInt32Int32 = (u, c) => true;
            ShimGroup.IsArchivedInt32Int32 = (u, c) => false;
            ShimEmailGroup.GetSubscriberCountInt32Int32User = (c, b, u) => 11;
            ShimContent.ValidateLinksInt32 = (l) => throw new ECNException("UT Exception", new List<ECNError>
            {
                new ECNError
                {
                    ErrorMessage = "UT Error"
                }
            });

            var paramObj = GetDefaultValidateDTO();
            paramObj.QTB.AutoCreateGroup = false;
            paramObj.CampaignName = "SampleCampagin";
            paramObj.CampaignID = 0;
            paramObj.CampaignItemName = string.Empty;
            paramObj.GroupID = 0;
            paramObj.GroupName = "SampleGroupName";
            paramObj.LayoutID = 0;
            paramObj.EmailSubject = new string('S', 256);
            return paramObj;
        }

        private ValidateDTO GetDefaultValidateDTO()
        {
            return new ValidateDTO
            {
                EmailsToAdd = "test@test.com,sample@test.com,",
                LayoutID = 1,
                EmailPreview = true,
                EmailFrom = "Select From Email",
                ReplyTo = "Select Reply To Email",
                FromName = string.Empty,
                EmailSubject = "TestMail",
                CustomerID = 1,
                GroupID = 0,
                GroupName = string.Empty,
                BaseChannelID = 1,
                CampaignID = 1,
                CampaignName = string.Empty,
                CampaignItemID = 1,
                CampaignItemName = string.Empty,
                CurrentUser = new User(),
                QTB = new QuickTestBlastConfig
                {
                    AllowAdhocEmails = true,
                }
            };
        }
    }
   
}
