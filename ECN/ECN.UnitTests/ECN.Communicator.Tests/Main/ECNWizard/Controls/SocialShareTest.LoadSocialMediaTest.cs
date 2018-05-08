using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class SocialShareTest
    {
        [Test]
        public void LoadSocialMedia_Facebook_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeLSMFakes();
            InitilizeLSMObject();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadSocialMedia"));
        }

        [Test]
        public void LoadSocialMedia_Linkedin_WithImages_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeLSMFakes(SocialMediaIdLinkedin);
            InitilizeLSMObject();

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadSocialMedia"));
        }

        [Test]
        public void LoadSocialMedia_HandledExceptions_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeLSMFakes(SocialMediaIdFacebook);
            InitilizeLSMObject();
            ShimSocialMediaHelper.GetUserAccountsString = (token) => throw new Exception("Test");
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth { };
            ShimCampaignItemMetaTag.GetByCampaignItemIDInt32 = (id) => new List<CampaignItemMetaTag> {
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebook, Property = "og:title", Content = "title" },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebook, Property = "og:title", CampaignItemMetaTagID = 1 },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebook, Property = "og:description", Content = "description" },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebook, Property = "og:description", CampaignItemMetaTagID = 1},
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdTwitter, Property = "hashtags", Content = "hashtags" },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdTwitter, Property = "hashtags", CampaignItemMetaTagID = 1},
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdLinkedin, Property = "og:title", Content = "title" },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdLinkedin, Property = "og:title", CampaignItemMetaTagID = 1 },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdLinkedin, Property = "og:description", Content = "description" },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdLinkedin, Property = "og:description", CampaignItemMetaTagID = 1} };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("LoadSocialMedia"));
        }

        private void InitilizeLSMObject()
        {
            var socialShare = new SocialShare();
            _testObject = new PrivateObject(socialShare);
            InitializeAllControls(socialShare);

            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataKeyNames = new string[] { "SocialMediaAuthID" };

            gvSimpleShare.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { Checked = true, ID = "chkEnableSimpleShare" } }
            });
            var upSocialConfig = new UpdatePanel();
            upSocialConfig.Visible = true;
            upSocialConfig.ID = "upSocialConfig";
            upSocialConfig.ContentTemplateContainer.Controls.Add(ConfigureSocialConfig(
                SocialMediaIdFacebook, "facebookPageId", "message", "title", "subtitle"));
            gvSimpleShare.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = upSocialConfig }
            });
        }

        private void InitilizeLSMFakes(int socialMedia = SocialMediaIdFacebook)
        {
            ShimSocialMediaAuth.GetByCustomerIDInt32 = (id) => new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };
            ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (id) => new List<CampaignItemSocialMedia>{
                new CampaignItemSocialMedia { SocialMediaID = socialMedia, SocialMediaAuthID = 1, SimpleShareDetailID = 1 },
                new CampaignItemSocialMedia { SocialMediaID = SocialMediaIdFacebook, SocialMediaAuthID = 1},
                new CampaignItemSocialMedia { SocialMediaID = SocialMediaIdTwitter, SocialMediaAuthID = 1},
                new CampaignItemSocialMedia { SocialMediaID = SocialMediaIdLinkedin, SocialMediaAuthID = 1},
                new CampaignItemSocialMedia { SocialMediaID = SocialMediaIdFacebookLike, SocialMediaAuthID = 1},
                new CampaignItemSocialMedia { SocialMediaID = SocialMediaIdF2F, SocialMediaAuthID = 1}};
            ShimCampaignItemMetaTag.GetByCampaignItemIDInt32 = (id) => new List<CampaignItemMetaTag> {
                socialMedia == SocialMediaIdFacebook ? new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebook } :
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebook, Property = "og:image", Content = "image" },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdTwitter },
                socialMedia == SocialMediaIdFacebook ? new CampaignItemMetaTag { SocialMediaID = SocialMediaIdLinkedin } :
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdLinkedin, Property = "og:image", Content = "image"  },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdFacebookLike },
                new CampaignItemMetaTag { SocialMediaID = SocialMediaIdF2F }};
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth { SocialMediaAuthID = 1 };
            ShimSimpleShareDetail.GetBySimpleShareDetailIDInt32 = (id) =>
                new SimpleShareDetail { Title = "title", SubTitle = "subtitle", Content = "content", UseThumbnail = true, SocialMediaID = SocialMediaIdFacebook, PageID = "facebookPageId" };
        }
    }
}
