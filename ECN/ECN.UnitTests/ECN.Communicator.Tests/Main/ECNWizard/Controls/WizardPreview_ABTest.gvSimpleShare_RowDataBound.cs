using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN.Communicator.Tests.Main.Salesforce.SF_Pages;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardPreview_ABTest
    {
        private bool _handlerCalled;
        private Label _lblAccountName;
        private Label _lblNetworkName = new Label();

        [Test]
        public void gvSimpleShare_RowDataBound_Facebook_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdFacebook };
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth();
            ShimSocialMediaHelper.GetFBUserProfileString = (token) => new Dictionary<string, string>()
            {
                {"first_name", "TestName" },
                {"last_name", "TestLastName" },
            };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<CampaignItemSocialMedia> {
                new CampaignItemSocialMedia {
                    SocialMediaID = SocialMediaIdFacebook,
                    SimpleShareDetailID = 1,
                    SocialMediaAuthID = 1
                }
            };
            ShimControl.AllInstances.FindControlString = (ctrl, controlName) =>
            {
                if (controlName == "lblSocialMediaName")
                {
                    return _lblAccountName;
                }
                else if (controlName == "lblSocialMedia")
                {
                    return _lblNetworkName;
                }

                return new Label();
            };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();

            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("TestName TestLastName"));

            _lblNetworkName.ShouldSatisfyAllConditions(
                () => _lblNetworkName.ShouldNotBeNull(),
                () => _lblNetworkName.Text.ShouldBe("Facebook"));
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
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth();
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            ShimOAuthHelper.Constructor = helper => { };
            ShimOAuthHelper.AllInstances.GetTwitterProfileStringStringString = (helper, s, arg3, arg4) => null;
            ShimSocialMediaHelper.GetJSONDictString = (token) => new Dictionary<string, string>()
            {
                {"name", "TestName" },
            };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<CampaignItemSocialMedia> {
                new CampaignItemSocialMedia {
                    SocialMediaID = SocialMediaIdTwitter,
                    SimpleShareDetailID = 1,
                    SocialMediaAuthID = 1
                }
            };
            ShimControl.AllInstances.FindControlString = (ctrl, controlName) =>
            {
                if (controlName == "lblSocialMediaName")
                {
                    return _lblAccountName;
                }
                else if (controlName == "lblSocialMedia")
                {
                    return _lblNetworkName;
                }

                return new Label();
            };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();

            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("TestName"));

            _lblNetworkName.ShouldSatisfyAllConditions(
                () => _lblNetworkName.ShouldNotBeNull(),
                () => _lblNetworkName.Text.ShouldBe("Twitter"));
        }

        [Test]
        public void gvSimpleShare_RowDataBound_LinkedIn_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeRDBFakes();
            InitilizeRDBObject();
            ShimSocialMedia.GetSocialMediaByIDInt32 = (id) => new SocialMedia { SocialMediaID = SocialMediaIdTwitter };
            ShimSocialMediaAuth.GetBySocialMediaAuthIDInt32 = (id) => new SocialMediaAuth();
            ShimSocialMediaHelper.GetLIUserProfileString = (token) => new Dictionary<string, string>()
            {
                {"first-name", "TestName" },
                {"last-name", "TestLastName" },
            };
            var gvSimpleShare = _testObject.GetField("gvSimpleShare") as GridView;
            gvSimpleShare.DataSource = new List<CampaignItemSocialMedia> {
                new CampaignItemSocialMedia {
                    SocialMediaID = SocialMediaIdLinkedin,
                    SimpleShareDetailID = 1,
                    SocialMediaAuthID = 1
                }
            };
            ShimControl.AllInstances.FindControlString = (ctrl, controlName) =>
            {
                if (controlName == "lblSocialMediaName")
                {
                    return _lblAccountName;
                }
                else if (controlName == "lblSocialMedia")
                {
                    return _lblNetworkName;
                }

                return new Label();
            };

            // Act
            gvSimpleShare.DataBind();

            // Assert
            _handlerCalled.ShouldBeTrue();

            _lblAccountName.ShouldSatisfyAllConditions(
                () => _lblAccountName.ShouldNotBeNull(),
                () => _lblAccountName.Text.ShouldBe("TestName TestLastName"));

            _lblNetworkName.ShouldSatisfyAllConditions(
                () => _lblNetworkName.ShouldNotBeNull(),
                () => _lblNetworkName.Text.ShouldBe("LinkedIn"));
        }

        private void TestEventHanler(object sender, GridViewRowEventArgs e)
        {
            _handlerCalled = true;
            _testObject.Invoke("gvSimpleShare_RowDataBound", new object[] { sender, e });
        }

        private void InitilizeRDBObject()
        {
            var socialShare = new ecn.communicator.main.ECNWizard.Controls.WizardPreview_AB();
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
                ItemTemplate = new TestTemplateItem { control = new Label { ID = "lblSocialMedia" } }
            });
            _lblAccountName = new Label { ID = "lblSocialMediaName" };
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

        private void InitilizeRDBFakes()
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
