using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.UI.WebControls;
using ecn.activityengines.Tests.Setup;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using KmEntities = KMPlatform.Entity;
namespace ecn.activityengines.Tests.Engines
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class pubSubscriptions_SO_subscribeTest
    {
        private const int Zero = 0;
        private const int AboveZero = 100;
        private const int KMCommonApplicaion = 100;
        private const int GroupId = 200;
        private string _pageId;
        private const string EmptyString = "";
        private const string NoneEmptyString = "D8C67381-CE28-413D-9C32-BCB8C86D3D41";
        private const string HttpUserScreen = "http://" + NoneEmptyString;
        private const string ECNEngineAccessKey = "Some Access Key";
        private const string EmailAddressField = "EmailAddress";
        private const string EmailIdField = "EmailID";
        private const string SmartFormIdField = "SmartFormID";
        private const string GroupIdFeild = "GroupID";
        private MocksContext _mocksContext;
        private pubSubscriptions_SO_subscribe _page;
        private PrivateObject _pagePrivate;
        private Random _random = new Random();
        private bool _insertSmartFormTrackingCalled;
        private int _smartFormField;
        private readonly string _userCackeKey = $"cache_user_by_AccessKey_{ECNEngineAccessKey}";

        [SetUp]
        public void Setup()
        {
            _mocksContext = new MocksContext();
            _page = new pubSubscriptions_SO_subscribe();
            _pagePrivate = new PrivateObject(_page);
            _insertSmartFormTrackingCalled = false;
            _pageId = GetAnyString();
            InitializeFields(_page);
        }

        [TearDown]
        public void TearDown()
        {
            _page.Dispose();
            _mocksContext.Dispose();
        }

        [Test]
        public void Page_Load_NoUserCached_ShouldAddUserToCache()
        {
            //Arrange
            PageLoadCommonShims(false);

            //Act
            CallPage_Load();

            //Assert
            _insertSmartFormTrackingCalled.ShouldBeTrue();
            _page.Cache.ShouldNotBeNull();
            _page.Cache[_userCackeKey].ShouldNotBeNull();
        }

        [Test]
        public void Page_Load_UserCached_ShouldNotAddUserToCache()
        {
            //Arrange
            PageLoadCommonShims();
            var user = new KmEntities.User();
            _page.Cache.Add(_userCackeKey, user, null, default(DateTime), default(TimeSpan),
                default(CacheItemPriority), null);
            //Act
            CallPage_Load();

            //Assert
            _page.Cache[_userCackeKey].ShouldBe(user);
        }

        [Test]
        public void Page_Load_WhenEmailLengthBelowFive_WillBeSetToDummyEmail()
        {
            //Arrange
            const int MinimumLength = 4;
            PageLoadCommonShims(false);
            var initialEmail = new string('q', MinimumLength);
            SetField(EmailAddressField, initialEmail);
            var error = $"Unknown GroupID: {GroupId}";
            var sourceMethod = "pubSubscriptions_SO_subscribe.Page_Load";
            //Act
            CallPage_Load();

            //Assert
            var email = GetField<string>(EmailAddressField);
            email.ShouldSatisfyAllConditions(
                () => email.ShouldNotBe(initialEmail),
                () => email.Length.ShouldBeGreaterThan(MinimumLength));
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, sourceMethod, KMCommonApplicaion);
        }

        [Test]
        [TestCase(AboveZero, Zero)]
        [TestCase(AboveZero, AboveZero)]
        [TestCase(AboveZero, AboveZero * 2)]
        [TestCase(Zero, AboveZero)]
        public void Page_Load_HasGroupIdAndGroup_ShouldNotShowMsgLabel(
            int emailId,
            int oldEmailId)
        {
            //Arrange
            SetField(EmailIdField, emailId);

            var msgLabel = GetField<Label>("MsgLabel");
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(It.IsAny<int>(),
                    It.IsAny<int>(), It.IsAny<string>()))
                .Returns(oldEmailId);
            _mocksContext.GroupBusiness.Setup(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group());
            PageLoadCommonShims();

            //Act
            CallPage_Load();

            //Assert
            msgLabel.Visible.ShouldBeFalse();
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Page_Load_HasSmartFormIdEmailNullOrGroupNull_ShouldLogNocCriticalError(
            bool emailNull,
            bool groupNull)
        {
            //Arrange
            var emailResult = emailNull
                ? null
                : new CommunicatorEntities.Email();
            var groupResult = groupNull
                ? null
                : new CommunicatorEntities.Group();
            _smartFormField = GetAnyNumber();
            _mocksContext.Email.Setup(email => email.GetByEmailIDNoAccessCheck(It.IsAny<int>()))
                .Returns(emailResult);
            _mocksContext.GroupBusiness.SetupSequence(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group())
                .Returns(groupResult);
            var emailId = GetAnyNumber();
            SetField(EmailIdField, emailId);
            var error = $"Unknown GroupID: {GroupId} or EmailID: {emailId}";
            var source = "pubSubscriptions_SO_subscribe.Page_Load";
            PageLoadCommonShims(false);

            //Act
            CallPage_Load();

            //Assert
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, source, KMCommonApplicaion);
        }

        [Test]
        public void Page_Load_HasGroupAndEmailAndGroupIdAboveZeroAndHasPageId_ShouldRedirect()
        {
            //Arrange
            _smartFormField = GetAnyNumber();
            var emailResult = new CommunicatorEntities.Email();
            var groupResult = new CommunicatorEntities.Group();
            var history = new CommunicatorEntities.SmartFormsHistory
            {
                Response_AdminEmail = GetAnyString(),
                Response_AdminMsgBody = GetAnyString(),
                Response_AdminMsgSubject = GetAnyString(),
                Response_FromEmail = GetAnyString(),
                Response_UserMsgBody = GetAnyString(),
                Response_UserMsgSubject = GetAnyString(),
                Response_UserScreen = GetAnyString(),
            };
            _mocksContext.Email.Setup(email => email.GetByEmailIDNoAccessCheck(It.IsAny<int>()))
               .Returns(emailResult);
            _mocksContext.GroupBusiness.SetupSequence(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group())
                .Returns(groupResult);
            _mocksContext.SmartFormsHistory
                .Setup(smartFormsHistory => smartFormsHistory.GetBySmartFormID_NoAccessCheck(It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(history);
            _mocksContext.Request.ServerVariables.Add("REMOTE_ADDR", "127.0.0.1");
            var expectedUrl = $"http://serve.ifficient.com/asp/serve.aspx?w=5854&CRID={_pageId}";
            PageLoadCommonShims();

            //Act
            CallPage_Load();

            //Assert
            _mocksContext.EmailDirect
                .Verify(emailDirect => emailDirect.Save(It.IsAny<CommunicatorEntities.EmailDirect>()),
                    Times.Exactly(2));
            _mocksContext.Response.Verify(response => response.Redirect(It.Is<string>(url =>
                url.Contains(expectedUrl)), true));
        }

        [Test]
        public void Page_Load_HasGroupAndEmailAndGroupIdAboveZeroAndPageIdNull_ShouldNotRedirect()
        {
            //Arrange
            _pageId = null;
            _smartFormField = GetAnyNumber();
            var emailResult = new CommunicatorEntities.Email();
            var groupResult = new CommunicatorEntities.Group();
            var history = new CommunicatorEntities.SmartFormsHistory
            {
                Response_AdminEmail = GetAnyString(),
                Response_AdminMsgBody = GetAnyString(),
                Response_AdminMsgSubject = GetAnyString(),
                Response_FromEmail = GetAnyString(),
                Response_UserMsgBody = GetAnyString(),
                Response_UserMsgSubject = GetAnyString(),
                Response_UserScreen = GetAnyString(),
            };
            _mocksContext.Email.Setup(email => email.GetByEmailIDNoAccessCheck(It.IsAny<int>()))
               .Returns(emailResult);
            _mocksContext.GroupBusiness.SetupSequence(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group())
                .Returns(groupResult);
            _mocksContext.SmartFormsHistory
                .Setup(smartFormsHistory => smartFormsHistory.GetBySmartFormID_NoAccessCheck(It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(history);
            PageLoadCommonShims();

            //Act
            CallPage_Load();

            //Assert
            _mocksContext.EmailDirect
                .Verify(emailDirect => emailDirect.Save(It.IsAny<CommunicatorEntities.EmailDirect>()),
                    Times.Exactly(2));
            _mocksContext.Response.Verify(response => response.Redirect(It.IsAny<string>(), true), Times.Never);
        }

        [Test]
        public void Page_Load_HasGroupAndEmailAndGroupIdAndPageIdWhenException_ShouldLogCriticalError()
        {
            //Arrange
            _smartFormField = GetAnyNumber();
            var emailResult = new CommunicatorEntities.Email();
            var groupResult = new CommunicatorEntities.Group();
            var history = new CommunicatorEntities.SmartFormsHistory
            {
                Response_AdminEmail = GetAnyString(),
                Response_AdminMsgBody = GetAnyString(),
                Response_AdminMsgSubject = GetAnyString(),
                Response_FromEmail = GetAnyString(),
                Response_UserMsgBody = GetAnyString(),
                Response_UserMsgSubject = GetAnyString(),
                Response_UserScreen = GetAnyString(),
            };
            _mocksContext.Email.Setup(email => email.GetByEmailIDNoAccessCheck(It.IsAny<int>()))
               .Returns(emailResult);
            _mocksContext.GroupBusiness.SetupSequence(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group())
                .Returns(groupResult);
            _mocksContext.SmartFormsHistory
                .Setup(smartFormsHistory => smartFormsHistory.GetBySmartFormID_NoAccessCheck(It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(history);
            PageLoadCommonShims();
            var source = "pubSubscriptions_SO_subscribe.Page_Load(Canon Redirect)";
            //Act
            CallPage_Load();

            //Assert
            _mocksContext.ApplicationLog.Verify(applicationLog => applicationLog.LogCriticalError(
                It.IsAny<Exception>(), source, KMCommonApplicaion, It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>()));
        }

        [Test]
        [TestCase(AboveZero, AboveZero + 1, HttpUserScreen, HttpUserScreen, false)]
        [TestCase(AboveZero, AboveZero, HttpUserScreen, HttpUserScreen, false)]
        [TestCase(AboveZero, AboveZero + 1, NoneEmptyString, NoneEmptyString, false)]
        [TestCase(AboveZero, AboveZero + 1, EmptyString, null, true)]
        public void Page_Load_smartFormHistoryUserScreenHttp_ShouldWriteToResponseOrShowMessage(
            int emailId,
            int oldEmailId,
            string responseUserScreen,
            string responseWrite,
            bool messageLabelVisible)
        {
            //Arrange
            _smartFormField = GetAnyNumber();
            SetField(EmailIdField, emailId);
            var emailResult = new CommunicatorEntities.Email();
            var groupResult = new CommunicatorEntities.Group();
            var history = new CommunicatorEntities.SmartFormsHistory
            {
                Response_AdminEmail = GetAnyString(),
                Response_AdminMsgBody = GetAnyString(),
                Response_AdminMsgSubject = GetAnyString(),
                Response_FromEmail = GetAnyString(),
                Response_UserMsgBody = GetAnyString(),
                Response_UserMsgSubject = GetAnyString(),
                Response_UserScreen = responseUserScreen,
            };
            _mocksContext.Email.Setup(email => email.GetByEmailIDNoAccessCheck(It.IsAny<int>()))
               .Returns(emailResult);
            _mocksContext.GroupBusiness.SetupSequence(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group())
                .Returns(groupResult);
            _mocksContext.SmartFormsHistory
                .Setup(smartFormsHistory => smartFormsHistory.GetBySmartFormID_NoAccessCheck(It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(history);
            _mocksContext.EmailGroup
                 .Setup(emailGroup => emailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(It.IsAny<int>(),
                     It.IsAny<int>(), It.IsAny<string>()))
                 .Returns(oldEmailId);
            PageLoadCommonShims();
            var msgLabel = GetField<Label>("MsgLabel");

            //Act
            CallPage_Load();

            //Assert
            if (responseWrite != null)
            {
                _mocksContext.Response.Verify(response => response.Write(It.Is<string>(content =>
                      content.Contains(responseWrite))));
            }
            msgLabel.Visible.ShouldBe(messageLabelVisible);
        }

        [Test]
        [TestCase(AboveZero, AboveZero + 1, HttpUserScreen, HttpUserScreen, false)]
        [TestCase(AboveZero, AboveZero, HttpUserScreen, HttpUserScreen, false)]
        [TestCase(AboveZero, AboveZero + 1, NoneEmptyString, NoneEmptyString, false)]
        [TestCase(AboveZero, AboveZero + 1, EmptyString, null, true)]
        public void Page_Load_WhenExceptionThrown_LogCriticalExceptionAndWriteToResponseOrShowMessage(
            int emailId,
            int oldEmailId,
            string responseUserScreen,
            string responseWrite,
            bool messageLabelVisible)
        {
            //Arrange
            var history = new CommunicatorEntities.SmartFormsHistory
            {
                Response_AdminEmail = GetAnyString(),
                Response_AdminMsgBody = GetAnyString(),
                Response_AdminMsgSubject = GetAnyString(),
                Response_FromEmail = GetAnyString(),
                Response_UserMsgBody = GetAnyString(),
                Response_UserMsgSubject = GetAnyString(),
                Response_UserScreen = responseUserScreen,
            };
            _smartFormField = GetAnyNumber();
            SetField(EmailIdField, emailId);
            var exception = new Exception();
            _mocksContext.EmailDirect.Setup(email => email.Save(It.IsAny<CommunicatorEntities.EmailDirect>()))
                .Throws(exception);
            _mocksContext.SmartFormsHistory
                .Setup(smartFormsHistory => smartFormsHistory.GetBySmartFormID_NoAccessCheck(It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(history);
            _mocksContext.GroupBusiness.Setup(group => group.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Group());
            _mocksContext.Email.Setup(email => email.GetByEmailIDNoAccessCheck(It.IsAny<int>()))
                .Returns(new CommunicatorEntities.Email());
            _mocksContext.EmailGroup
                 .Setup(emailGroup => emailGroup.GetEmailIDFromWhatEmail_NoAccessCheck(It.IsAny<int>(),
                     It.IsAny<int>(), It.IsAny<string>()))
                 .Returns(oldEmailId);
            PageLoadCommonShims();
            var source = "pubSubscriptions_SO_subscribe.Page_Load";

            //Act
            CallPage_Load();

            //Assert
            _mocksContext.ApplicationLog.Verify(log => log.LogCriticalError(exception, source,
                KMCommonApplicaion, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        private void PageLoadCommonShims(bool urlQueryString = true)
        {
            _mocksContext.AppSettings.Add("Image_DomainPath", GetAnyString());
            _mocksContext.AppSettings.Add("Activity_DomainPath", GetAnyString());
            _mocksContext.AppSettings.Add("KMCommon_Application", KMCommonApplicaion.ToString());
            _mocksContext.AppSettings.Add($"kmps_g_{GroupId}", _pageId);
            _mocksContext.AppSettings.Add("ECNEngineAccessKey", ECNEngineAccessKey);
            var url = GetUrl(urlQueryString);
            _mocksContext.Request.Setup(request => request.Url)
                .Returns(url);
            ShimSmartFormTracking.InsertSmartFormTracking = smartFromTracking =>
            {
                _insertSmartFormTrackingCalled = true;
            };
            _mocksContext.UserBusiness.Setup(user => user.GetByAccessKey(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new KmEntities.User());
            _mocksContext.GroupDataFields
                .Setup(groupDataFields => groupDataFields.GetByGroupID_NoAccessCheck(It.IsAny<int>()))
                .Returns(new List<CommunicatorEntities.GroupDataFields>
                {
                    new CommunicatorEntities.GroupDataFields()
                });
            SetField("CustomerID", GetAnyNumber());
            SetField(SmartFormIdField, _smartFormField);
            SetField(GroupIdFeild, GroupId);
        }

        private Uri GetUrl(bool addQueryString)
        {
            var urlBuilder = new UriBuilder("http://doman.some/");
            if (addQueryString)
            {
                var query = new NameValueCollection();
                query["b"] = "blastId";
                query["bid"] = "blastId";
                query["blastid"] = "blastId";
                query["campaignitemid"] = "campaignitemid";
                query["s"] = "Subscribe";
                query["f"] = "Format";
                query["g"] = "GroupID";
                query["gid"] = "GroupID";
                query["e"] = "EmailAddress";
                query["e"] = "EmailAddress@someDomain";
                query["ei"] = "EmailAddress";
                query["eid"] = "EmailAddress";
                query["emailid"] = "Format";
                query["lid"] = "BlastLinkID";
                query["m"] = "SocialMediaID";
                query["c"] = "CustomerID";
                query["preview"] = "preview";
                query["sfid"] = $"{GetAnyNumber()}";
                query["url"] = "URL";
                query["layoutid"] = "layoutid";
                query["monitor"] = "Monitor";
                query["bcid"] = "BaseChannelID";
                query["newemail"] = "NewEmail";
                query["oldemail"] = "OldEmail";
                query["edid"] = "EmailDirectID";
                urlBuilder.Query = ToQueryString(query);
            }
            return urlBuilder.Uri;
        }

        private string ToQueryString(NameValueCollection nameValueCollection)
        {
            var array = (from key in nameValueCollection.AllKeys
                         from value in nameValueCollection.GetValues(key)
                         select $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}")
                         .ToArray();
            return string.Join("&", array);
        }

        private void CallPage_Load()
        {
            const string MethodName = "Page_Load";
            _pagePrivate.Invoke(MethodName, new object[] { null, null });
        }

        private T GetField<T>(string fieldName)
        {
            var fieldValue = _pagePrivate.GetField(fieldName);
            fieldValue.ShouldNotBeNull();
            return (T)fieldValue;
        }

        private void SetField<T>(string fieldName, T fieldValue)
        {
            _pagePrivate.SetField(fieldName, fieldValue);
        }

        private void InitializeFields(object item)
        {
            var flags = BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Public;
            var fields = item.GetType().GetFields(flags)
                .Where(field => field.GetValue(item) == null)
                .ToList();
            foreach (var field in fields)
            {
                var constructor = field.FieldType.GetConstructor(flags, null, new Type[0], null);
                field.SetValue(item, constructor?.Invoke(new object[0]));
            }
        }

        private string GetAnyString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetAnyNumber()
        {
            const int Minimum = 10;
            const int Maximum = 1000;
            return _random.Next(Minimum, Maximum);
        }
    }
}
