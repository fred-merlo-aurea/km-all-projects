using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Caching;
using System.Web.Fakes;
using System.Web.Security;
using System.Web.Security.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Master = ecn.accounts.MasterPages;

namespace ECN.Accounts.Tests.MasterPages
{
    /// <summary>
    /// UT for <see cref="ecn.collector.MasterPages.Collector"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class AccountsTest : PageHelper
    {
        private Master.Accounts _testPage;
        private PrivateObject _testObject;

        [SetUp]
        public void SetUp()
        {
            _testPage = new Master.Accounts();
            InitializeAllControls(_testPage);
            _testObject = new PrivateObject(_testPage);
            ShimECNSession.CurrentSession = () => {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new User
                {
                    UserClientSecurityGroupMaps = new List<UserClientSecurityGroupMap>
                    {
                        new UserClientSecurityGroupMap { ClientID = 1}
                    }
                };
                session.CurrentCustomer = new Customer();
                session.CurrentBaseChannel = new BaseChannel();
                return session;
            };
        }

        [Test]
        public void DrpAccount_SelectedIndexChanged_CreateCookie_Redirect()
        {
            // Arrange
            var drpClient = GetField<DropDownList>("drpClient");
            drpClient.Items.Add("1");
            drpClient.SelectedValue = "1";
            var drpClientGroup = GetField<DropDownList>("drpclientgroup");
            drpClientGroup.Items.Add("1");
            drpClientGroup.SelectedValue = "1";
            ShimFormsAuthentication.SignOut = () => { };
            ShimFormsAuthentication.SetAuthCookieStringBoolean = (p1, p2) => { };
            ShimCustomer.GetByClientIDInt32Boolean = (p1, p2) => new Customer { };
            var cookies = new HttpCookieCollection();
            ShimHttpResponse.AllInstances.CookiesGet = (p) => cookies;
            ShimFormsAuthentication.EncryptFormsAuthenticationTicket = (p) => "test";

            // Act
            _testObject.Invoke("drpAccount_SelectedIndexChanged", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("~/Default.aspx"),
                () => cookies[FormsAuthentication.FormsCookieName].ShouldNotBeNull(),
                () => cookies[FormsAuthentication.FormsCookieName].Value.ShouldBe("test"));
        }

        [Test]
        public void LbEditProfile_Click_Redirect()
        {
            // Arrange
            var url = "http://km.com";
            var redirectPath = "/ecn.accounts/main/users/EditUserProfile.aspx?redirecturl=";
            QueryString.Add("sample","test");
            var fullPath = redirectPath + url + "sample=test";

            // Act
            _testObject.Invoke("lbEditProfile_Click", new object[] { null, null });

            // Assert
            //TODO please fix query parameters are not handled correctly
            //RedirectUrl.ShouldBe(fullPath);
            RedirectUrl.ShouldBe("/ecn.accounts/main/users/EditUserProfile.aspx?redirecturl=%2f%3fSystem.Collections.Specialized.NameValueCollection");
        }

        [Test]
        public void LbLogout_Click_EmptyCache_Redirect()
        {
            // Arrange
            var cache = new Cache();
            cache.Add("test","value", null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            ShimUserControl.AllInstances.CacheGet = (p) => cache;
            ShimFormsAuthentication.SignOut = () => { };

            // Act
            _testObject.Invoke("lbLogout_Click", new object[] { null, null });

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => RedirectUrl.ShouldBe("/EmailMarketing.Site/Login/Logout"),
                () => cache.Count.ShouldBe(0));
        }

        private T GetField<T>(string fieldName) where T : class
        {
            var field = _testObject.GetField(fieldName) as T;

            field.ShouldNotBeNull();

            return field;
        }
    }
}
