using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Caching.Fakes;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShimKMUser = KMPlatform.BusinessLogic.Fakes.ShimUser;
using KM.Common.Entity.Fakes;
using ecn.activityengines.Fakes;
using ecn.activityengines.Tests.Helpers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class OptinTest: PageHelper
    {
        private Optin _testedEntity;
        private PrivateObject _privateTestedObject;        
        private bool _savedEmailDirect = false;      
        private KMPlatform.Entity.User _user;
        private Group _group;
        private ShimCache _shimCache;
        private DateTime _dummyDate = DateTime.Now.Date;
        private Email _email;
        private bool _loggedInNonCriticalError;
        private SmartFormsHistory _smartFormHistory;
        private const int DummyInt = 100;        
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
        private const string BlastIdValue = "111";
        private const string EmailAddressKey = "e";
        private const string EmailAddressValue = "email@email.com";
        private const string UrlKey = "url";
        private const string UrlValue = "http://localhost:8080";
        private const string SubscribeTypeCodeKey = "s";
        private const string SubscribeTypeCodeValue = "S";
        private const string FormatKey = "f";
        private const string FormatValue = "html";         
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
        private const string PrivateFieldNameUser = "User";
        private const string ImportEmailExceptionError = "violation of unique key";
        private const string urlQuery = "{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={4}&{9}={5}&{10}={6}&{11}={7}&{12}={8}";
        private const string MsgSnippet = @"%%GroupID%% %%GroupName%%  %%EmailID%% %%EmailAddress%% %%Title%% %%FirstName%%   %%LastName%%   %%FullName%%
                                             %%Company%%  %%Occupation%% %%Address%% %%Address2%%  %%City%%, %%State%%, %%Zip%% %%Country%% %%Voice%%, %%Mobile%% %%Fax%%, 
                                             %%Website%% %Age%%, %%Income%% %%Gender%% %%Notes%%, %%BirthDat %%User1%%, %%User2%%, %%User3%%,%%User4%% %%UserEvent2Date%% 
                                             %%UserEvent2%% %%UserEvent1Date%% %%UserEvent1%% %%User5%% %%User6%%";
        
        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testedEntity = new Optin();

            _loggedInNonCriticalError = false;
            _user = new KMPlatform.Entity.User()
            {   
                UserID = DummyInt
            };
            _group = new Group()
            {
                GroupID = int.Parse(GroupIdValue),
                GroupName = DummyString
            };
            _smartFormHistory = new SmartFormsHistory()
            {
                Response_FromEmail = DummyString,
                Response_UserMsgSubject = DummyString,
                Response_UserScreen = MsgSnippet,
                Response_UserMsgBody = MsgSnippet,
                Response_AdminEmail = MsgSnippet,
                Response_AdminMsgSubject = DummyString,
                Response_AdminMsgBody = MsgSnippet
            };

            _privateTestedObject = new PrivateObject(_testedEntity);
            _privateTestedObject.SetFieldOrProperty(PrivateFieldNameUser, _user);            

            InitializeEmail();
            InitializeFakes();
        }

        private void InitializeFakes()
        {          
            var queryStringWithValues = String.Format(urlQuery,
                                                        CustomerIdKey, CustomerIdValue,
                                                        GroupIdKey, GroupIdValue,
                                                        SmartFormIdKey, SmartFormIdValue,
                                                        EmailAddressKey, EmailAddressValue,
                                                        SubscribeTypeCodeKey, SubscribeTypeCodeValue,
                                                        FormatKey, FormatValue,
                                                        BlastIdKey, BlastIdValue,
                                                        UrlKey, UrlValue,
                                                        EmailIdKey, EmailIdValue);

            ShimHttpRequest.AllInstances.UrlGet = (req) => { return new Uri(UrlValue + "?" + queryStringWithValues); };
            ShimHttpRequest.AllInstances.RawUrlGet = (req) => UrlValue;                        
            ShimHttpServerUtility.AllInstances.UrlDecodeString = (server, url) => url;
            
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

            ShimKMUser.GetByAccessKeyStringBoolean = (key, getChildren) => _user;

            ShimEmailDirect.SaveEmailDirect =
                (ed) =>
                {
                    _savedEmailDirect = true;
                    return DummyInt;
                };

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, sourceMethod, applicationId, note, charityId, customerId) => 
                {
                    _loggedInNonCriticalError = true;
                    return DummyInt;
                };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 =
                (error, sourceMethod, applicationId, note, charityId, customerId) =>
                {
                    _loggedInNonCriticalError = true;
                    return DummyInt;
                };

            ShimEmailGroup.GetEmailIDFromWhatEmail_NoAccessCheckInt32Int32String = (GroupID, CustomerID, EmailAddress) => int.Parse(EmailIdValue);

            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = (groupID) =>
            {
                return new List<GroupDataFields>()
                {
                    new GroupDataFields()
                    {
                        GroupDataFieldsID = DummyInt,
                        ShortName = DummyString
                    }
                };
            };

            ShimSmartFormTracking.InsertSmartFormTracking = (sft) => { };
            
            ShimSmartFormsHistory.GetGroupIDInt32Int32 = (customerID, smartFormID) => _group.GroupID;
            ShimSmartFormsHistory.GetBySmartFormID_NoAccessCheckInt32Int32 = (smartFormID, groupID) => _smartFormHistory;

            ShimEmail.IsValidEmailAddressString = (emailAddress) => true;            
            ShimEmail.GetByEmailID_NoAccessCheckInt32 = (eID) => _email;
            ShimEmailGroup.ImportEmails_NoAccessCheckUserInt32Int32StringStringStringStringBooleanStringString  =
                (user, getGroupID, xml, xmlUdf, format, subscribeTypeCode, emailOnly, compositeKey, overvwrite, source) => new DataTable();

            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (groupID) => _group;
            
            ShimEmailActivityLog.InsertInt32Int32StringStringStringUser = (blastID, eID, subscriber, sub, subNotes, User) => DummyInt;

            ShimEventOrganizer.EventInt32Int32Int32UserNullableOfInt32 = (customerID, groupID, eID, user, smartFormID) => { };

            ShimOptin.AllInstances.getQSString = (optin, qs) => DummyString;
        }

        private void InitializeEmail()
        {
            _email = new Email()
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
                User1 = DummyString,
                User2 = DummyString,
                User3 = DummyString,
                User4 = DummyString,
                User5 = DummyString,
                User6 = DummyString,
                UserEvent1 = DummyString,
                UserEvent1Date = _dummyDate,
                UserEvent2 = DummyString,
                UserEvent2Date = _dummyDate,
                EmailID = DummyInt,
                EmailAddress = EmailAddressValue
            };
        }
    }
}
