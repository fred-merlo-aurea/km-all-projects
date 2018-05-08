using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Caching;
using System.Web.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.activityengines.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using ecn.communicator.classes.Fakes;
using ecn.communicator.classes;
using ecn.activityengines.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Shouldly;
using ShimApplicationLog = KM.Common.Entity.Fakes.ShimApplicationLog;
using ShimDataFunctions = ecn.common.classes.Fakes.ShimDataFunctions;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SO_multigroup_subscribeTests : PageHelper
    {
        private const string TestUser = "TestUser";
        private const string ECNEngineAccessKey = "ECNEngineAccessKey";
        private const string SmartFormIDKey = "sfID";
        private const string SampleEmail = "test@test.com";
        private const string SampleSubject = "SampleSubject";
        private const string SampleSubscriber = "SampleSubscriber";
        private const string SampleFormat = "html";
        private const string YesNotation = "Y";
        private const string NoNotation = "N";
        private const string SampleUserScreen = "SampleUserScreen";
        private const string SampleBody = "SampleBody";
        private const string SampleDb = "SampleDb";
        private const string AccountDbKey = "accountsdb";
        private const string EmailKey ="e";
        private const string SubscriberKey ="s";
        private const string FormatKey ="f";
        private const string SoptionNameKey = "soptin";
        private const string EmailIdKey = "ei";
        private const string SFModeKey = "SFmode";
        private const string SFmodeValue = "Manage";
        private const string EmailSource = "Activity Engine";
        private const string NoneEmptyString = "D8C67381-CE28-413D-9C32-BCB8C86D3D41";
        private const string HttpUserScreen = "http://" + NoneEmptyString;
        private const string UTException = "UT Exception";
        private const string PageLoadMethodName = "Page_Load";
        private SO_multigroup_subscribe _testEntity;
        private PrivateObject _privateTestObject;
        private static readonly string ECNEngineAccessToken = Guid.NewGuid().ToString();
        private NameValueCollection _requestParams;
        private List<int> _addedGroupIds;
        private List<EmailDirect> _savedEmailDirects;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _requestParams = new NameValueCollection();
            _testEntity = new SO_multigroup_subscribe();
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
            ClearCacheItems();
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
        private void ClearCacheItems()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator enumerator = _testEntity.Cache.GetEnumerator();

            while (enumerator.MoveNext())
                keys.Add(enumerator.Key.ToString());

            for (var i = 0; i < keys.Count; i++)
            {
                _testEntity.Cache.Remove(keys[i]);
            }
        }

        [Test]
        public void Page_Load_WhenUserIsNotInCache_AddsUserToCacheAndSetsSmartFieldDetails()
        {
            // Arrange
            const string userCacheKey = "cache_user_by_AccessKey_";
            SetFakesForPageLoadMethod();

            // Act
            _privateTestObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _testEntity.Cache.ShouldNotBeNull(),
                () => _testEntity.Cache.Count.ShouldBe(1),
                () => _testEntity.Cache[$"{userCacheKey}{ECNEngineAccessToken}"].ShouldNotBeNull(),
                () =>
                    {
                        var user = _testEntity.Cache[$"{userCacheKey}{ECNEngineAccessToken}"].ShouldBeOfType<User>();
                        user.ShouldSatisfyAllConditions(
                             () => user.ShouldNotBeNull(),
                             () => user.UserID.ShouldBe(1));
                    },
                () => _addedGroupIds.Count.ShouldBe(2),
                () => _addedGroupIds.ShouldBe(new[] { 1, 1 }),
                () => _testEntity.Response_UserScreen.ShouldBe(SampleUserScreen),
                () => ResponseWriteText.ShouldNotBeNullOrWhiteSpace(),
                () => ResponseWriteText.ShouldBe("1"));
        }

        [Test]
        [TestCase(NoNotation)]
        [TestCase(YesNotation)]
        public void Page_Load_WhenCacheHasUser_RetrieviesUserFromCacheAndSetsSmartFieldVariables(string QSparam)
        {
            // Arrange
            const int groupId = 1;
            SetFakesForPageLoadMethod();
            _requestParams.Set(SoptionNameKey, YesNotation);
            _requestParams.Set($"g_{groupId}", QSparam);
            var cacheKey = $"cache_user_by_AccessKey_{ECNEngineAccessToken}";
            var user = new User { UserID = 1, UserName = TestUser };
            _testEntity.Cache.Add(
                cacheKey,
                user,
                null,
                Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(15),
                CacheItemPriority.Normal,
                null);

            _privateTestObject = new PrivateObject(_testEntity);

            // Act
            _privateTestObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _savedEmailDirects.ShouldNotBeEmpty(),
                () => _savedEmailDirects.Count.ShouldBe(2),
                () => _savedEmailDirects.ShouldContain(x => x.ReplyEmailAddress.Contains(SampleEmail)),
                () => _savedEmailDirects.ShouldContain(x => x.EmailAddress.Contains(SampleEmail)),
                () => _savedEmailDirects.ShouldContain(x => x.EmailSubject.Contains(SampleSubject)),
                () => _savedEmailDirects.ShouldContain(x => x.Source.Contains(EmailSource)),
                () => _testEntity.Response_UserScreen.ShouldBe(SampleUserScreen),
                () => ResponseWriteText.ShouldBe(SampleUserScreen));
            if (QSparam == YesNotation)
            {
                _privateTestObject.ShouldSatisfyAllConditions(
                    () => _addedGroupIds.ShouldNotBeEmpty(),
                    () => _addedGroupIds.Count.ShouldBe(2),
                    () => _addedGroupIds.ShouldBe(new[] { 1, 1 }));
            }
        }

        [Test]
        [TestCase(NoNotation)]
        [TestCase(YesNotation)]
        public void Page_Load_WhenCacheHasUserAndUserScreenStartsWithHttp_RedirectsUserScreen(string soption)
        {
            // Arrange
            const int groupId = 1;
            SetFakesForPageLoadMethod();
            _requestParams.Set(SoptionNameKey, soption);
            _requestParams.Set($"g_{groupId}", NoNotation );
            var cacheKey = $"cache_user_by_AccessKey_{ECNEngineAccessToken}";
            var user = new User { UserID = 1, UserName = TestUser };
            _testEntity.Cache.Add(
                cacheKey,
                user,
                null,
                Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(15),
                CacheItemPriority.Normal,
                null);
            _privateTestObject = new PrivateObject(_testEntity);
            var dataTable = GetDataTable();
            dataTable.Rows[0]["Response_UserScreen"] = HttpUserScreen;
            ShimDataFunctions.GetDataTableString = (query) => dataTable;

            // Act
            _privateTestObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);

            // Assert
            if (soption == NoNotation)
            {
                _privateTestObject.ShouldSatisfyAllConditions(
                    () => _addedGroupIds.ShouldNotBeEmpty(),
                    () => _addedGroupIds.Count.ShouldBe(2),
                    () => _addedGroupIds.ShouldBe(new[] { 1, 1 }),
                    () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                    () => RedirectUrl.ShouldBe("1"));
            }
            if(soption == YesNotation)
            {
                _privateTestObject.ShouldSatisfyAllConditions(
                    () => _savedEmailDirects.ShouldNotBeEmpty(),
                    () => _savedEmailDirects.Count.ShouldBe(2),
                    () => _savedEmailDirects.ShouldContain(x => x.ReplyEmailAddress.Contains(SampleEmail)),
                    () => _savedEmailDirects.ShouldContain(x => x.EmailAddress.Contains(SampleEmail)),
                    () => _savedEmailDirects.ShouldContain(x => x.EmailSubject.Contains(SampleSubject)),
                    () => _savedEmailDirects.ShouldContain(x => x.Source.Contains(EmailSource)),
                    () => _testEntity.Response_UserScreen.ShouldBe(SampleUserScreen),
                    () => RedirectUrl.ShouldNotBeNullOrWhiteSpace(),
                    () => RedirectUrl.ShouldBe(SampleUserScreen));
            }
        }
        
        [Test]
        [TestCase(SampleUserScreen)]
        [TestCase(HttpUserScreen)]
        public void Page_Load_WhenCurrentProfileIsNull_AddsGroupObjectAndRedirects(string userScreen)
        {
            // Arrange
            SetFakesForPageLoadMethod();
            ShimEmails.GetEmailByIDInt32 = _ => null;
            var dataTable = GetDataTable();
            dataTable.Rows[0]["Response_UserScreen"] = userScreen;
            ShimDataFunctions.GetDataTableString = (query) => dataTable;

            // Act
            _privateTestObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);

            // Assert
            _privateTestObject.ShouldSatisfyAllConditions(
                () => _addedGroupIds.ShouldNotBeEmpty(),
                () => _addedGroupIds.ShouldBe(new[] { 1, 1 }));
            if(userScreen == SampleUserScreen)
            {
                ResponseWriteText.ShouldBe(SampleUserScreen);
            }
            if(userScreen == HttpUserScreen)
            {
                RedirectUrl.ShouldBe(HttpUserScreen);
            }
        }

        [Test]
        public void Page_Load_WhenGetCustomerEmailThrowsException_LogsException()
        {
            // Arrange
            var isExceptionLogged = false;
            var expectionMessage = string.Empty;
            SetFakesForPageLoadMethod();
            ShimGroups.AllInstances.WhatEmailForCustomerString = (_, __) => throw new InvalidOperationException(UTException);
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (ex, name, appid, _, __, ___) =>
            {
                isExceptionLogged = true;
                expectionMessage = ex.Message;
                return 1;
            };

            // Act
            _privateTestObject.Invoke(PageLoadMethodName, this, EventArgs.Empty);

            // Assert
            isExceptionLogged.ShouldSatisfyAllConditions(
                () => isExceptionLogged.ShouldBeTrue(),
                () => expectionMessage.ShouldContain(UTException),
                () => ResponseWriteText.ShouldNotBeNullOrWhiteSpace(),
                () => ResponseWriteText.ShouldContain("Error in Multi-Group Subscribe.  Customer Service has been notified."));
        }

        private void SetFakesForPageLoadMethod()
        {
            _addedGroupIds = new List<int>();
            _savedEmailDirects = new List<EmailDirect>();

            var settings = new NameValueCollection();
            settings.Add(ECNEngineAccessKey, ECNEngineAccessToken);
            settings.Add(AccountDbKey, SampleDb);
            ShimConfigurationManager.AppSettingsGet = () => settings;
            ShimUser.GetByAccessKeyStringBoolean = (_, __) => new User { UserName = TestUser, UserID = 1, IsActive = true };

            _requestParams.Add(SmartFormIDKey, "1");
            _requestParams.Add(EmailKey, SampleEmail);
            _requestParams.Add(SubscriberKey, SampleSubscriber);
            _requestParams.Add(FormatKey, SampleFormat);
            _requestParams.Add(SoptionNameKey, NoNotation);
            _requestParams.Add(EmailIdKey, "1");
            _requestParams.Add(SFModeKey, SFmodeValue);
            ShimHttpRequest.AllInstances.ParamsGet = (h) => _requestParams;

            ShimDataFunctions.GetDataTableString = (query) => GetDataTable();
            ShimDataFunctions.ExecuteScalarString = (query) => "1";
            ShimDataFunctions.ExecuteString = (query) => 1;
            ShimGroups.AllInstances.WhatEmailForCustomerString = (_, __) => new Emails(input_id: 1) { };
            ShimGroups.AllInstances.HasPendingEmailEmails = (_, __) => true;
            ShimSO_multigroup_subscribe.AllInstances.AddtoGroupInt32 = (m, groupId) => { _addedGroupIds.Add(groupId); };
            ShimSO_multigroup_subscribe.AllInstances.ReplaceCodeSnippetsGroupsEmailsString = (m, groups,e,s) => SampleUserScreen;

            ShimEmails.GetEmailByIDInt32 = (id) => new Emails(input_id: 1);

            ShimEmailDirect.SaveEmailDirect = (email) =>
            {
                _savedEmailDirects.Add(email);
                return _savedEmailDirects[0].CustomerID;
            };
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Response_FromEmail", typeof(string));
            dataTable.Columns.Add("Response_UserMsgSubject", typeof(string));
            dataTable.Columns.Add("Response_UserMsgBody", typeof(string));
            dataTable.Columns.Add("Response_UserScreen", typeof(string));
            dataTable.Columns.Add("Response_AdminEmail", typeof(string));
            dataTable.Columns.Add("Response_AdminMsgSubject", typeof(string));
            dataTable.Columns.Add("Response_AdminMsgBody", typeof(string));
            dataTable.Columns.Add("GroupID", typeof(int));
            dataTable.Columns.Add("CustomerID", typeof(int));
            dataTable.Columns.Add("SubscriptionGroupIDs", typeof(string));
            dataTable.Columns.Add("DoubleOptIn", typeof(int));

            var row = dataTable.NewRow();
            row["Response_FromEmail"] = SampleEmail;
            row["Response_UserMsgSubject"] = SampleSubject;
            row["Response_UserMsgBody"] = SampleBody;
            row["Response_UserScreen"] = SampleUserScreen;
            row["Response_AdminEmail"] = SampleEmail;
            row["Response_AdminMsgSubject"] = SampleSubject;
            row["Response_AdminMsgBody"] = SampleBody;
            row["GroupID"] = 1;
            row["CustomerID"] = 1;
            row["SubscriptionGroupIDs"] = 1;
            row["DoubleOptIn"] = 1;

            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}
