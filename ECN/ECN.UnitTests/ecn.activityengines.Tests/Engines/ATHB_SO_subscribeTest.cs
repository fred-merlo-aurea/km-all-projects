using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using System.Web.Caching.Fakes;
using System.Web.UI.Fakes;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KM.Common.Entity.Fakes;
using ShimKMUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using ecn.activityengines.Fakes;
using ecn.activityengines.Tests.Helpers;
using ecn.common.classes.Fakes;
using ecn.communicator.classes;
using ecn.communicator.classes.Fakes;
using ShimComEventOrganizer = ecn.communicator.classes.Fakes.ShimEventOrganizer;
using ECN_Framework_BusinessLayer.Communicator.Fakes;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ATHB_SO_subscribeTest : PageHelper
    {
        private ATHB_SO_subscribe _testedEntity;
        private PrivateObject _privateTestedObject;
        private DataTable _smartFormsHistoryTable;
        private bool _savedEmailDirect = false;
        private Groups _groups;
        private Emails _emails;
        private KMPlatform.Entity.User _user;
        private List<KMPlatform.Entity.User> _users;
        private DataTable _groupDataFieldsTable;
        private ShimCache _shimCache;
        private const string EmailIdKey = "ei";
        private const string EmailIdValue = "5";
        private const string CustomerIdKey = "c";
        private const string CustomerIdValue = "10";
        private const string CustomerIdValueNewSubscriber = "2553";
        private const string GroupIdKey = "g";
        private const string GroupIdValue = "11";
        private const string SmartFormIdKey = "sfID";
        private const string SmartFormIdValue = "12";
        private const string BlastIdKey = "b";
        private const string BlastIdValue = "1";
        private const string EmailAddressKey = "e";
        private const string EmailAddressValue = "email@email.com";
        private const string UrlKey = "url";
        private const string UrlValue = "http://localhost:8080";
        private const string SubscribeTypeCodeKey = "s";
        private const string SubscribeTypeCodeValue = "S";
        private const string FormatKey = "f";
        private const string FormatValue = "html";
        private const string UserHostAddress = "192.168.10.0";
        private const string AccountsDBKey = "accountsdb";
        private const string ActivityDomainPathKey = "Activity_DomainPath";
        private const string Image_DomainPathKey = "Image_DomainPath";
        private const string ECNEngineAccessKey = "ECNEngineAccessKey";
        private const string ConnectionStringKey = "connString";
        private const string ConnectionStringValue = "test_connection_string";
        private const string DummyString = "dummyStringValue";
        private const string DummyPathString = "km/ecn";
        private const string ColumnResponseFromEmail = "Response_FromEmail";
        private const string ColumnResponseUserMsgSubject = "Response_UserMsgSubject";
        private const string ColumnResponseUserMsgBody = "Response_UserMsgBody";
        private const string ColumnResponseUserScreen = "Response_UserScreen";
        private const string ColumnResponseAdminEmail = "Response_AdminEmail";
        private const string ColumnResponseAdminMsgSubject = "Response_AdminMsgSubject";
        private const string ColumnResponseAdminMsgBody = "Response_AdminMsgBody";
        private const string ColumnGroupDataFieldsID = "GroupDataFieldsID";
        private const string ColumnGroupShortName = "ShortName";
        private const string SnippetUnsubscribe = "dummyStringValue%%value%%unsubscribelink%%";
        private const string PrivateFieldNameUser = "User";
        private const string PrivateFieldNameGroup = "group";        
        private const string SqlForSmartFormHistory = "SmartFormsHistory";
        private const string SqlForGroupDatafields = "GroupDatafields";
        private const string MsgSnippet = @"%%GroupID%% %%GroupName%%  %%EmailID%% %%EmailAddress%% %%Title%% %%FirstName%%   %%LastName%%   %%FullName%%
                                             %%Company%%  %%Occupation%% %%Address%% %%Address2%%  %%City%%, %%State%%, %%Zip%% %%Country%% %%Voice%%, %%Mobile%% %%Fax%%, 
                                             %%Website%% %Age%%, %%Income%% %%Gender%% %%Notes%%, %%BirthDat %%User1%%, %%User2%%, %%User3%%,%%User4%% %%UserEvent2Date%% 
                                             %%UserEvent2%% %%UserEvent1Date%% %%UserEvent1%% %%User5%% %%User6%%";
        private const int DummyInt = 100;
        private DateTime _dummyDate = DateTime.Now.Date;        

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testedEntity = new ATHB_SO_subscribe();
            _smartFormsHistoryTable = new DataTable();
            _groupDataFieldsTable = new DataTable();
            _groups = new Groups();
            _user = new KMPlatform.Entity.User();
            _users = new List<KMPlatform.Entity.User>() { _user };

            _privateTestedObject = new PrivateObject(_testedEntity);
            _privateTestedObject.SetFieldOrProperty(PrivateFieldNameUser, _user);
            _privateTestedObject.SetFieldOrProperty(PrivateFieldNameGroup, _groups);

            InitializeSmartFormsHistoryDT();
            InitializeGroupDT();
            InitializeEmails();
            InitializeFakes();
        }

        [TearDown]
        public void TestCleanup()
        {
            base.CleanUp();
        }
                
        private void InitializeFakes()
        {
            QueryString = new NameValueCollection
               {
                    {EmailIdKey,  EmailIdValue },
                    {CustomerIdKey,  CustomerIdValue },
                    {GroupIdKey, GroupIdValue },
                    {SmartFormIdKey,SmartFormIdValue },
                    {BlastIdKey, BlastIdValue },
                    {EmailAddressKey, EmailAddressValue },
                    {UrlKey, UrlValue },
                    {SubscribeTypeCodeKey, SubscribeTypeCodeValue },
                    {FormatKey, FormatValue }
               };

            ShimHttpRequest.AllInstances.UrlReferrerGet = (r) => new Uri(UrlValue);
            ShimHttpRequest.AllInstances.UserHostAddressGet = (r) => UserHostAddress;
            ShimHttpRequest.AllInstances.RawUrlGet = (r) => UrlValue;

            ShimConfigurationManager.AppSettingsGet =
               () => new NameValueCollection
               {
                   {AccountsDBKey,  DummyString },
                   {ActivityDomainPathKey, DummyString },
                   {ActivityDomainPathKey, DummyPathString },
                   {Image_DomainPathKey, DummyPathString },
                   {ECNEngineAccessKey, CustomerIdValue },
                   {ConnectionStringKey, ConnectionStringValue }
               };

            _shimCache = new ShimCache()
            {
                AddStringObjectCacheDependencyDateTimeTimeSpanCacheItemPriorityCacheItemRemovedCallback =
                (key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback) => new object()
            };
            _shimCache.Bind(new Dictionary<string, KMPlatform.Entity.User>());
            ShimPage.AllInstances.CacheGet = (page) => _shimCache;

            ShimDataFunctions.GetDataTableString = (sql) =>
            {
                if (sql.Contains(SqlForSmartFormHistory))
                {
                    return _smartFormsHistoryTable;
                }
                else if (sql.Contains(SqlForGroupDatafields))
                {
                    return _groupDataFieldsTable;
                }
                return new DataTable();
            };

            ShimDataFunctions.ExecuteScalarStringString = (db, sql) => DummyString;

            ShimDatabaseAccessor.Constructor = (database) =>
            {
                var shimDB = new ShimDatabaseAccessor()
                {
                    ID = () => DummyInt
                };
            };

            ShimEmailDirect.SaveEmailDirect =
                (ed) =>
                {
                    _savedEmailDirect = true;
                    return DummyInt;
                };

            ShimEmails.GetEmailByIDInt32 = (id) => _emails;

            ShimKMUser.GetByClientGroupIDInt32 = (customerID) => _users;
            ShimKMUser.GetByCustomerIDInt32 = (customerID) => _users;
            ShimKMUser.GetByAccessKeyStringBoolean = (key, getChildren) => _user;

            KM.Platform.Fakes.ShimUser.IsAdministratorUser = (user) => true;
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (user, serviceCode, serviceFeature, accessCode) => true;

            ShimEmailGroup.ImportEmailsWithDupesUserInt32StringStringStringStringBooleanStringBooleanString =
                (user, getGroupID, xml, xmlUdf, format, subscribeTypeCode, emailOnly, compositeKey, overvwrite, source) => new DataTable();

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, sourceMethod, applicationId, note, charityId, customerId) => DummyInt;

            ShimComEventOrganizer.AllInstances.EventEmailActivityLog = (eventOrg, log) => { };

            ShimATHB_SO_subscribe.AllInstances.getQSString = (subscribeValue, qs) => DummyString;

            ShimGroups.AllInstances.WhatEmailString = (groups, email) => _emails;
            ShimGroups.AllInstances.CustomerID = (groups) => int.Parse(CustomerIdValue);
            ShimGroups.AllInstances.UDFHashGet = (groups) =>
            {
                var groupList = new System.Collections.SortedList
                {
                    { DummyInt.ToString(), DummyString }
                };
                return groupList;
            };
        }

        private void InitializeSmartFormsHistoryDT()
        {
            _smartFormsHistoryTable.Columns.Add(ColumnResponseFromEmail);
            _smartFormsHistoryTable.Columns.Add(ColumnResponseUserMsgSubject);
            _smartFormsHistoryTable.Columns.Add(ColumnResponseUserMsgBody);
            _smartFormsHistoryTable.Columns.Add(ColumnResponseUserScreen);
            _smartFormsHistoryTable.Columns.Add(ColumnResponseAdminEmail);
            _smartFormsHistoryTable.Columns.Add(ColumnResponseAdminMsgSubject);
            _smartFormsHistoryTable.Columns.Add(ColumnResponseAdminMsgBody);
            _smartFormsHistoryTable.Rows.Add(DummyString, DummyString, MsgSnippet, MsgSnippet, DummyString, DummyString, MsgSnippet);
        }

        private void InitializeGroupDT()
        {
            _groupDataFieldsTable.Columns.Add(ColumnGroupDataFieldsID);
            _groupDataFieldsTable.Columns.Add(ColumnGroupShortName);
            _groupDataFieldsTable.Rows.Add(DummyInt, DummyString);
        }


        private void InitializeEmails()
        {
            _emails = new Emails()
            {
                Title = DummyString,
                FirstName = DummyString,
                LastName = DummyString,
                FullName = DummyString,
                Company = DummyString,
                Occupation = DummyString,
                Address = DummyString,
                Address2 = DummyString,
                City = DummyString,
                State = DummyString,
                Zip = DummyString,
                Country = DummyString,
                Voice = DummyString,
                Mobile = DummyString,
                Fax = DummyString,
                Website = DummyString,
                Age = DummyString,
                Income = DummyString,
                Gender = DummyString,
                Notes = DummyString,
                BirthDate = _dummyDate,
                User1 = DummyString,
                User2 = DummyString,
                User3 = DummyString,
                User4 = DummyString,
                User5 = DummyString,
                User6 = DummyString,
                UserEvent1 = DummyString,
                UserEvent1Date = _dummyDate,
                UserEvent2 = DummyString,
                UserEvent2Date = _dummyDate
            };

            _emails.ID(DummyInt);
            _emails.EmailAddress(EmailIdValue);
        }
    }
}
