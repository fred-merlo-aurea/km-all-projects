using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
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
        private bool _handlerCalled;
        private Label _lblAccountName;

        [Test]
        public void gvSimpleShare_RowDataBound_Exception()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => throw new Exception("Test Exception");
            ShimSocialMediaHelper.GetFBUserProfileString = (token) => new Dictionary<string, string> { { "url", string.Empty } };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };
            var imageTraceMessage = string.Empty;
            ShimUserControl.AllInstances.TraceGet = page =>
            {
                return new ShimTraceContext()
                {
                    WarnStringStringException = (issueType, issueMessage, exception) =>
                    {
                        imageTraceMessage = string.Format(issueMessage, exception);
                    }
                };
            };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBeEmpty());
            imageTraceMessage.ShouldContain("Unable to retrieve the image: System.Exception: Test Exception");
        }

        [Test]
        public void gvSimpleShare_RowDataBound_Facebook_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID =  SocialMediaIdFacebook};
            ShimSocialMediaHelper.GetFBUserProfileString = (token) => new Dictionary<string, string> { { "url", string.Empty } };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1} };
            _queryString["campaignitemtype"] = "champion";

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("testProfile"));
        }

        [Test]
        public void gvSimpleShare_RowDataBound_Facebook_Exception()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdFacebook };
            ShimSocialMediaHelper.GetFBUserProfileString = (token) => throw new Exception("Test Exception");
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("Unable to get profile for testProfile"));
        }

        [Test]
        public void gvSimpleShare_RowDataBound_Twitter_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdTwitter };
            ShimOAuthHelper.Constructor = (instance) => { };
            ShimOAuthHelper.AllInstances.GetTwitterProfileStringStringString = (instance, p1, p2, p3) => string.Empty;
            ShimSocialMediaHelper.GetJSONDictString = (data) => new Dictionary<string, string> { { "profile_image_url_https", string.Empty } };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };

            // Act
                gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("testProfile"));
        }

        [Test]
        public void gvSimpleShare_RowDataBound_Twitter_Exception()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdTwitter };
            ShimOAuthHelper.Constructor = (instance) => throw new Exception("Test Exception");
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("Unable to get profile for testProfile"));
        }

        [Test]
        public void gvSimpleShare_RowDataBound_Linkedin_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdLinkedin };
            ShimSocialMediaHelper.GetLIUserProfileString = (token) => new Dictionary<string, string> { };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("testProfile"));
        }

        [Test]
        public void gvSimpleShare_RowDataBound_Linkedin_Exception()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdLinkedin };
            ShimSocialMediaHelper.GetLIUserProfileString = (token) => throw new Exception("Test Exception");
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<SocialMediaAuth> { new SocialMediaAuth { SocialMediaAuthID = 1 } };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();
            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("Unable to get profile for testProfile"));
        }

        private void TestEventHanler(object sender, GridViewRowEventArgs e)
        {
            _handlerCalled = true;
            _testObject.Invoke("gvSimpleShare_RowDataBound", new object[] { sender, e });
        }

        private void InitilizeRDBObject()
        {
            var socialShare = new SocialShare();
            _testObject = new PrivateObject(socialShare);
            InitializeAllControls(socialShare);
            _queryString = new NameValueCollection { {"campaignitemtype", string.Empty } };
            _appSettings.Add("TW_CONSUMER_KEY", string.Empty);
            _appSettings.Add("TW_CONSUMER_SECRET", string.Empty);
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataKeyNames = new string[] { "SocialMediaAuthID" };
            gvSimpleShare.RowDataBound += TestEventHanler;

            gvSimpleShare.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new CheckBox { Checked = true, ID = "chkEnableSimpleShare" } }
            });
            gvSimpleShare.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new Label { ID = "lblSocialNetwork" } }
            });
            _lblAccountName = new Label { ID = "lblAccountName" };
            gvSimpleShare.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = _lblAccountName }
            });
            gvSimpleShare.Columns.Add(new TemplateField
            {
                ItemTemplate = new TestTemplateItem { control = new ImageButton { ID = "imgbtnDelete" } }
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

        private void InitilizeRDBFakes(int socialMedia = SocialMediaIdFacebook)
        {
            ShimUserControl.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                QueryStringGet = () => _queryString,
                UrlGet = () => new Uri("http://www.km.com")
            };

            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth { SocialMediaAuthID = 1, ProfileName = "testProfile" };
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) => new List<CampaignItemBlast> { new CampaignItemBlast {  } };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (p1, p2, p3) => new CampaignItem { SampleID = 1};
            ShimSample.GetBySampleIDInt32User = (id, user) => new Sample { };
            ShimBlastActivity.ChampionByProcInt32BooleanUserString = (p1, p2, p3, p4) => new DataTable {
                Columns = {"Winner","EmailSubject" },
                Rows = { {"true", "subject" } }
            };
        }
    }
}
