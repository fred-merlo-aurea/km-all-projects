using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Data.SqlClient.Fakes;
using System.IO;
using System.Net.Fakes;
using System.Web.Caching;
using System.Web.Caching.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ecn.activityengines.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KM.Common.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using Blast = ECN_Framework.Communicator.Entity.Blast;
using CommFakeDataLayer = ECN_Framework_DataLayer.Communicator.Fakes;
using CommFakeEntities = ECN_Framework.Communicator.Entity.Fakes;
using FrameworkFakeDataLayer = ECN_Framework_DataLayer.Fakes;
using ShimEncryption = KM.Common.Fakes.ShimEncryption;

namespace ecn.activityengines.Tests
{
    public partial class SClickTest
    {
        private const int PageLoadBlastId = 10;
        private const int PageLoadEmailId = 20;
        private const int PageLoadSocialId = 1;
        private const int PageLoadGroupId = 40;
        private const int PageLoadCampaignItemID = 10;
        private const string PageLoadShareLink = "ShareLink";
        private const string PageLoadEmailSubject = "EMailSubject";
        private string _pageLoadRedirectString;
        private string _pageLoadResponseScript;

        [TestCase(1, 1, 1, 0, true)]
        [TestCase(1, 1, 0, 1, true)]
        [TestCase(1, 0, 1, 1, true)]
        [TestCase(0, 1, 1, 1, false)]
        public void Page_Load_InvalidQueryString_Error(int blastId, int emailId, int groupId, int socialId, bool cacheUser)
        {
            // Arrange
            ShimSClick.AllInstances.getRefBlast = (page) => { };
            var queryString = CreateQueryStringForPage_Load(blastId, emailId, groupId, socialId);
            InitTestForPage_Load(queryString: queryString, cacheUser: cacheUser);

            // Act
            _sClicktPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            _errorMsgPanel.Visible.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_BlackBerryAgent_ResponseRedirectCalled()
        {
            // Arrange
            InitTestForPage_Load(requestUserAgent: "blackberry");

            // Act
            _sClicktPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            _responseRedirectMethodCalled.ShouldBeTrue();
        }

        [Test]
        public void Page_Load_NotBlackBerryAgent_RedirectScriptWritten()
        {
            // Arrange
            InitTestForPage_Load(requestUserAgent: "NotBB");

            // Act
            _sClicktPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            _pageLoadResponseScript.ShouldSatisfyAllConditions(
                () => _pageLoadResponseScript.ShouldNotBeNullOrEmpty(),
                () => _pageLoadResponseScript.ShouldStartWith("<script"));
        }

        [TestCase(2, true)]
        [TestCase(2, false)]
        [TestCase(100, false)]
        public void Page_Load_DifferentSocialMediaValues_DirectUrlInitialized(int socialMediaId, bool campaignItemExist)
        {
            // Arrange
            InitTestForPage_Load(requestUserAgent: "blackberry", socialMediaId: socialMediaId, campaignItemExist: campaignItemExist);
            var expectedRedirectUrl = PageLoadShareLink;
            if (socialMediaId != 2)
            {
                var queryString = $"b={PageLoadBlastId}&g={PageLoadGroupId}&e={PageLoadEmailId}&m={PageLoadSocialId}";
                var encodedQueryString = System.Web.HttpUtility.UrlEncode(queryString);
                expectedRedirectUrl = $"{PageLoadShareLink}{AppSettingsSocialDomainPathValue}{AppSettingsSocialPreviewKey}{encodedQueryString}";
            }

            // Act
            _sClicktPrivateObject.Invoke("Page_Load", new object[] { null, EventArgs.Empty });

            // Assert
            _pageLoadRedirectString.ShouldSatisfyAllConditions(
                () => _errorMsgPanel.Visible.ShouldBeFalse(),
                () => _pageLoadRedirectString.ShouldBe(expectedRedirectUrl),
                () => _sClicktInstance.Title.ShouldContain(PageLoadEmailSubject));
        }

        private void InitTestForPage_Load(bool cacheUser = false, string queryString = "", bool getBlastByIdThrowException = false, int socialMediaId = 2, bool campaignItemExist = true, string requestUserAgent = "")
        {
            _responseRedirectMethodCalled = false;
            _pageLoadRedirectString = string.Empty;
            _pageLoadResponseScript = string.Empty;
            queryString = string.IsNullOrEmpty(queryString)
                ? CreateQueryStringForPage_Load()
                : queryString;
            SetPagePropertiesForPage_Load();
            ShimHttpRequest.AllInstances.UserAgentGet = (p) => requestUserAgent;
            ShimHttpRequest.AllInstances.UrlGet = (p) => new Uri($"http://fakeUrl00000fakeUrl.com?{queryString}");
            SetPageControlsForPage_Load();
            ShimCache.AllInstances.ItemGetString = (cache, key) =>
            {
                return cacheUser
                             ? new User()
                             : null;
            };
            KMPlatform.BusinessLogic.Fakes.ShimUser.GetByAccessKeyStringBoolean = (key, getChildren) => new User();
            ShimSqlCommand.AllInstances.ExecuteReaderCommandBehavior = (command, behaviour) => new ShimSqlDataReader();
            ShimSqlCommand.AllInstances.ConnectionGet = (command) => new ShimSqlConnection();
            ShimSqlConnection.AllInstances.Open = (conn) => { };
            ShimSqlConnection.AllInstances.Close = (conn) => { };
            ShimDataFunctions.GetSqlConnectionString = (connString) => new ShimSqlConnection();
            ShimDataFunctions.GetSqlConnection = () => new ShimSqlConnection();
            ShimEncryption.DecryptStringEncryption = (txt, enc) => queryString;
            ShimEncryption.EncryptStringEncryption = (txt, enc) => txt;
            ShimHttpWebResponse.AllInstances.GetResponseStream = (response) => new MemoryStream();
            ShimHttpWebRequest.AllInstances.GetResponse = (request) => new ShimHttpWebResponse();
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, getChildren) => new BlastSMS()
            {
                BlastType = "LAYOUT",
                CustomerID = 100,
                GroupID = PageLoadGroupId
            };
            FrameworkFakeDataLayer.ShimDataFunctions.ExecuteScalarSqlCommandString = (cmd, connString) =>
             {
                 if (cmd.CommandText == "e_BlastSingle_GetRefBlastID" || cmd.CommandText.Contains("select COUNT(eg.EmailID)"))
                 {
                     return 1;
                 }
                 return 0;
             };
            CommFakeEntities.ShimBlast.GetByBlastIDInt32 = (id) =>
            {
                return getBlastByIdThrowException
                  ? throw new Exception()
                  : new Blast()
                      {
                          EmailSubject = $"{PageLoadEmailSubject}1%%{PageLoadEmailSubject}2%%{PageLoadEmailSubject}3"
                      };
            };
            CommFakeDataLayer.ShimSocialMedia.GetSqlCommand = (cmd) => new SocialMedia()
            {
                SocialMediaID = socialMediaId,
                ShareLink = PageLoadShareLink
            };
            CommFakeDataLayer.ShimCampaignItem.GetSqlCommand = (cmd) =>
            {
                return campaignItemExist
                ? new CampaignItem()
                    {
                        CampaignItemID = PageLoadCampaignItemID
                    }
                : null;
            };
            CommFakeDataLayer.ShimCampaignItemTestBlast.GetSqlCommand = (cmd) => new CampaignItemTestBlast()
            {
                CampaignItemID = PageLoadCampaignItemID
            };
            CommFakeDataLayer.ShimCampaignItemMetaTag.GetListSqlCommand = (cmd) =>
            {
                return new List<CampaignItemMetaTag>()
                {
                    new CampaignItemMetaTag()
                    {
                         SocialMediaID  = 2,
                         Property = "hashtags"
                    }
                };
            };
        }

        private string CreateQueryStringForPage_Load(int blastId = PageLoadBlastId, int emailId = PageLoadEmailId, int groupId = PageLoadGroupId, int socialId = PageLoadSocialId)
        {
            return $"blastid={blastId}&emailid={emailId}&m={socialId}&gid={groupId}";
        }

        private void SetPageControlsForPage_Load()
        {
            _errorMsgPanel = InitField<Panel>(ErrorMsgPanelId);
            _errorMsgPanel.Visible = false;
        }

        private void SetPagePropertiesForPage_Load()
        {
            var appSettingsCollection = new NameValueCollection();
            appSettingsCollection.Add(AppSettingsEngineAccessKey, AppSettingsEngineAccessKeyValue);
            appSettingsCollection.Add(AppSettingsEngineAccessKey, AppSettingsEngineAccessKeyValue);
            appSettingsCollection.Add(AppSettingsSocialDomainPathKey, AppSettingsSocialDomainPathValue);
            appSettingsCollection.Add(AppSettingsSocialPreviewKey, AppSettingsSocialPreviewValue);
            ShimPage.AllInstances.CacheGet = (p) => new Cache(); ;
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (response, url, endResponse) =>
            {
                _responseRedirectMethodCalled = true;
                _pageLoadRedirectString = url;
            };
            ShimHttpResponse.AllInstances.WriteString = (response, txt) =>
            {
                _pageLoadResponseScript = txt;
            };
            ShimPage.AllInstances.ResponseGet = (p) => new ShimHttpResponse();
            ShimPage.AllInstances.RequestGet = (p) => new ShimHttpRequest();
            ShimConfigurationManager.AppSettingsGet = () => appSettingsCollection;
            ShimConnectionStringSettingsCollection.AllInstances.ItemGetInt32 = (col, id) => new ConnectionStringSettings();
            ShimConnectionStringSettingsCollection.AllInstances.ItemGetString = (col, id) => new ConnectionStringSettings();
            ShimConfigurationManager.ConnectionStringsGet = () => new ShimConnectionStringSettingsCollection();
        }
    }
}
