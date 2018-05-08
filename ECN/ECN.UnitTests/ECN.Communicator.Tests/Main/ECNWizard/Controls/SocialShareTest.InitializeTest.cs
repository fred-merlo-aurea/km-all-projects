using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Fakes;
using System.Net.Fakes;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class SocialShareTest
    {
        private string _redirectUrl;

        [Test]
        public void Initialize_Facebook_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            _queryString.Add("simple", "fb");
            _queryString.Add("campaignitemtype", "1");

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize"));
        }

        [Test]
        public void Initialize_Security_Exception()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, p1, p2, p3) => false;

            // Act, Assert
            Should.Throw<Exception>(() => _testObject.Invoke("Initialize"));
        }

        [Test]
        public void Initialize_Facebook_WithCode_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            _queryString.Add("simple", "fb");
            _queryString.Add("campaignitemtype", "1");
            _queryString.Add("code", "1");
            ShimSocialMediaHelper.GetFBAccessTokenStringString = (code, url) => new Dictionary<string, string>
            {
                { "access_token", "1"}
            };

            ShimSocialMediaHelper.GetFBLongLivedTokenString = (code) => new Dictionary<string, string>
            {
                { "access_token", "1"}
            };

            ShimSocialMediaHelper.GetFBUserProfileString = (token) => new Dictionary<string, string>
            {
                { "id", "1"},
                { "first_name", "first_name"},
                { "last_name", "last_name"}
            };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize"));
        }

        [Test]
        public void Initialize_Facebook_WithoutToken_Error()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            _appSettings["FBUserID"] = "0";
            _queryString.Add("simple", "fb");
            _queryString.Add("campaignitemtype", "1");
            _queryString.Add("code", "1");
            ShimSocialMediaHelper.GetFBAccessTokenStringString = (code, url) => new Dictionary<string, string> { };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize"));
        }

        [Test]
        public void Initialize_Facebook_WithoutLongLiveToken_Error()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            _appSettings["FBUserID"] = "2";
            _queryString.Add("simple", "fb");
            _queryString.Add("campaignitemtype", "1");
            _queryString.Add("code", "1");
            ShimSocialMediaHelper.GetFBAccessTokenStringString = (code, url) => new Dictionary<string, string>
            {
                { "access_token", "1"}
            };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize"));
        }

        [Test]
        public void Initialize_Linkedin_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            _queryString.Add("simple", "li");
            _queryString.Add("state", "00000000-0000-0000-0000-000000000000");
            _queryString.Add("code", "1");
            _queryString.Add("access_token", "1");
            ShimWebRequest.CreateString = (p) =>
            {
                return new ShimHttpWebRequest
                {
                    GetResponse = () => new ShimHttpWebResponse()
                };
            };

            ShimStreamReader.ConstructorStream = (instance, stream) => { };
            ShimStreamReader.AllInstances.ReadToEnd =  (isntance) => string.Empty;
            ShimSocialMediaHelper.GetJSONDictString = (vals) => new Dictionary<string, string>
            {
                { "access_token", "1"}
            };
            ShimSocialMediaHelper.GetLIUserProfileString = (s) => new Dictionary<string, string>
            {
                { "id", "1"},
                { "first-name", "first-name"},
                { "last-name", "last-name"}
            };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize"));
        }

        [Test]
        public void Initialize_Twitter_Success()
        {
            // Arrange
            InitilizeAssertObjects();
            InitilizeFakes();
            InitilizeIntFakes();
            InitilizeIntObject();
            _queryString.Add("simple", "tw");
            _queryString.Add("oauth_token", "1");
            _queryString.Add("oauth_verifier", "1");
            _appSettings.Add("TW_CONSUMER_KEY", "1");
            _appSettings.Add("TW_CONSUMER_SECRET", "1");
            ShimOAuthHelper.AllInstances.GetUserTwAccessTokenStringStringString = (instance, p1, p2, p3) => { };
            ShimOAuthHelper.AllInstances.GetTwitterProfileStringStringString = (instance, p1, p2, p3) => string.Empty;
            ShimSocialMediaHelper.GetJSONDictString = (vals) => new Dictionary<string, string>
            {
                { "name", "name"}
            };

            // Act, Assert
            Should.NotThrow(() => _testObject.Invoke("Initialize"));
        }

        private void InitilizeIntObject()
        {
            var socialShare = new SocialShare();
            _testObject = new PrivateObject(socialShare);
            InitializeAllControls(socialShare);
            _queryString = new NameValueCollection();
            _appSettings.Add("FBUserID", "1");

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

        private void InitilizeIntFakes(int socialMedia = SocialMediaIdFacebook)
        {
            ShimSocialMediaAuth.ExistsByUserIDStringInt32Int32 = (p1, p2, p3) => true;
            ShimSocialMediaAuth.SaveSocialMediaAuthUser = (auth, user) => 1;
            ShimSocialMediaAuth.GetByUserID_CustomerID_SocialMediaIDStringInt32Int32 = (p1, p2, p3) =>
                new List<SocialMediaAuth> { new SocialMediaAuth { } };
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, p1, p2, p3) => true;
            ShimUserControl.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                QueryStringGet = () => _queryString,
                UrlGet = () => new Uri("http://www.km.com")
            };

            ShimUserControl.AllInstances.ResponseGet = (instance) => new ShimHttpResponse
            {
                RedirectString = (url) => { _redirectUrl = url; }
            };

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
