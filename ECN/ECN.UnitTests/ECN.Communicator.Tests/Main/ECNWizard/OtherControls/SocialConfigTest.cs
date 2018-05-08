using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web;
using System.Web.Fakes;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using ecn.communicator.main.ECNWizard.OtherControls;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects.Fakes;
using static ECN_Framework_Common.Objects.SocialMediaHelper;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.OtherControls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SocialConfigTest
    {
        private const string SampleHost = "km.com";
        private const string SampleHttpHost = "http://km.com";
        private const string SampleHostPath = "http://km.com/addTemplate";
        private const string SampleUserAgent = "http://km.com/addTemplate";
        private const string CampaignItemTemplateID = "1";
        private const string KMCommon_Application = "1";
        private const string MethodDisplayControls = "DisplayControls";
        private const string DummyString = "dummyString";
        private const string NoAccounts = "No Accounts";
        private const int FaceBook = 1;
        private const int Twitter = 2;
        private const int LinkedIn = 3;
        private StateBag _viewState;
        private Page _page;
        private IDisposable _context;
        private object[] _methodArgs;
        private Type _typeSocialConfig;
        private SocialConfig _objectSocialConfig;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void Initialize()
        {
            _typeSocialConfig = typeof(SocialConfig);
            _objectSocialConfig = CreateInstance(_typeSocialConfig);
            InitializeSession();
        }

        [TestCase(FaceBook)]
        [TestCase(LinkedIn)]
        public void DisplayControls_WhenAuthorizedAccount_AccountListIsShown(int socialMediaID)
        {
            // Arrange
            Initialize();
            ShimSocialMediaHelper.GetUserAccountsString = (x) =>
            {
                var fbAccount = CreateInstance(typeof(FBAccount));
                fbAccount.id = "1";
                var fbAccountList = new List<FBAccount> { fbAccount };
                return fbAccountList;
            };
            ShimSocialMediaHelper.GetLICompaniesString = (x) =>
            {
                var liAccount = CreateInstance(typeof(LIAccount));
                liAccount.id = "1";
                var liAccountList = new List<LIAccount> { liAccount };
                return liAccountList;
            };
            _methodArgs = new object[] { socialMediaID, DummyString, DummyString };

            // Act
            CallMethod(_typeSocialConfig, MethodDisplayControls, _methodArgs, _objectSocialConfig);

            // Assert
            var accountList = GetField(_objectSocialConfig, "ddlAccounts") as DropDownList;
            _objectSocialConfig.ShouldSatisfyAllConditions(
                () => accountList.ShouldNotBeNull(),
                () => accountList.Visible.ShouldBeTrue());
        }

        [TestCase(FaceBook)]
        [TestCase(LinkedIn)]
        public void DisplayControls_WhenNoAccounts_AccountListIsHidden(int socialMediaID)
        {
            // Arrange
            Initialize();
            ShimSocialMediaHelper.GetUserAccountsString = (x) =>
            {
                var fbAccountList = new List<FBAccount>();
                return fbAccountList;
            };
            ShimSocialMediaHelper.GetLICompaniesString = (x) =>
            {
                var liAccountList = new List<LIAccount> ();
                return liAccountList;
            };
            _methodArgs = new object[] { socialMediaID, DummyString, DummyString };

            // Act
            CallMethod(_typeSocialConfig, MethodDisplayControls, _methodArgs, _objectSocialConfig);

            // Assert
            var accountList = GetField(_objectSocialConfig, "ddlAccounts") as DropDownList;
            _objectSocialConfig.ShouldSatisfyAllConditions(
                () => accountList.ShouldNotBeNull(),
                () => accountList.Items[0].ShouldNotBeNull(),
                () => accountList.Items[0].Text.ShouldBe(NoAccounts),
                () => accountList.Visible.ShouldBeTrue());
        }

        [TestCase(FaceBook, true)]
        [TestCase(Twitter, false)]
        [TestCase(LinkedIn, true)]
        public void DisplayControls_AccountError_ErrorIsDisplayed(int socialMediaID, bool visibility)
        {
            // Arrange
            Initialize();
            _methodArgs = new object[] { socialMediaID, DummyString, DummyString };

            // Act
            CallMethod(_typeSocialConfig, MethodDisplayControls, _methodArgs, _objectSocialConfig);

            // Assert
            AssertControls(visibility);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private void SetDefaults()
        {
            ShimGridView.AllInstances.DataBind = (x) => { };
            var pnlOptOutSpecificGroups = CreateInstance(typeof(UpdatePanel));
            pnlOptOutSpecificGroups.UpdateMode = UpdatePanelUpdateMode.Conditional;
            SetField(_objectSocialConfig, "pnlOptOutSpecificGroups", pnlOptOutSpecificGroups);
            SetField(_objectSocialConfig, "upMain", pnlOptOutSpecificGroups);
            _viewState = new StateBag();
            var campaignItemTemplateGroupObject = CreateInstance(typeof(CampaignItemTemplateGroup));
            var campaignItemTemplateGroupList = new List<CampaignItemTemplateGroup> { campaignItemTemplateGroupObject };
            var campaignItemTemplateSuppressionGroup = CreateInstance(typeof(CampaignItemTemplateSuppressionGroup));
            var campaignItemTemplateSuppressionGroupList = new List<CampaignItemTemplateSuppressionGroup> { campaignItemTemplateSuppressionGroup };
            var campaignItemTemplateOptoutGroup = CreateInstance(typeof(CampaignItemTemplateOptoutGroup));
            var campaignItemTemplateOptoutGroupList = new List<CampaignItemTemplateOptoutGroup> { campaignItemTemplateOptoutGroup };
            _viewState["SelectedGroups"] = campaignItemTemplateGroupList;
            _viewState["SelectedSuppressionGroups"] = campaignItemTemplateSuppressionGroupList;
            _viewState["OptoutGroups_DT"] = campaignItemTemplateOptoutGroupList;
            SetField(_objectSocialConfig, "ViewState", _viewState);
            SetField(_objectSocialConfig, "drpdownCampaign", new DropDownList
            {
                Items =
                {
                    new ListItem
                    {
                        Selected = true,
                        Value = CampaignItemTemplateID
                    }
                }
            });
        }

        private void InitializeSession()
        {
            ShimECNSession.AllInstances.RefreshSession = (item) => { };
            ShimECNSession.AllInstances.ClearSession = (itme) => { };
            var CustomerID = 1;
            var UserID = 1;
            var config = new NameValueCollection();
            var reqParams = new NameValueCollection();
            var queryString = new NameValueCollection();
            var dummyCustormer = CreateInstance(typeof(Customer));
            var dummyUser = CreateInstance(typeof(User));
            var authTkt = CreateInstance(typeof(ECN_Framework_Entities.Application.AuthenticationTicket));
            var ecnSession = CreateInstance(typeof(ECNSession));
            var baseChannel = CreateInstance(typeof(BaseChannel));
            config.Add("KMCommon_Application", KMCommon_Application);
            queryString.Add("HTTP_HOST", SampleHttpHost);
            queryString.Add("CampaignItemTemplateID", CampaignItemTemplateID);
            dummyCustormer.CustomerID = CustomerID;
            dummyUser.UserID = UserID;
            baseChannel.BaseChannelID = UserID;
            SetField(authTkt, "CustomerID", CustomerID);
            SetField(ecnSession, "CurrentUser", dummyUser);
            SetField(ecnSession, "CurrentCustomer", dummyCustormer);
            SetField(ecnSession, "CurrentBaseChannel", baseChannel);
            _page = new Page();
            HttpContext.Current = MockHelpers.FakeHttpContext();
            ShimECNSession.CurrentSession = () => ecnSession;
            ShimAuthenticationTicket.getTicket = () => authTkt;
            ShimUserControl.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimUserControl.AllInstances.ResponseGet = (x) => HttpContext.Current.Response;
            ShimConfigurationManager.AppSettingsGet = () => config;
            ShimHttpRequest.AllInstances.UserAgentGet = (h) => SampleUserAgent;
            ShimHttpRequest.AllInstances.QueryStringGet = (h) => queryString;
            ShimHttpRequest.AllInstances.UserHostAddressGet = (h) => SampleHost;
            ShimHttpRequest.AllInstances.UrlReferrerGet = (h) => new Uri(SampleHostPath);
            ShimPage.AllInstances.SessionGet = x => HttpContext.Current.Session;
            ShimPage.AllInstances.RequestGet = (x) => HttpContext.Current.Request;
            ShimHttpRequest.AllInstances.ParamsGet = (x) => reqParams;
            ShimControl.AllInstances.ParentGet = (control) => _page;
            InitializeAllControls(_objectSocialConfig);
            SetDefaults();
        }

        private void AssertControls(bool visibility)
        {
            var errorMessage = GetField(_objectSocialConfig, "AccountErrrorMsg") as Label;
            var message = GetField(_objectSocialConfig, "txtMessage") as HiddenField;
            var title = GetField(_objectSocialConfig, "trTitle") as HtmlTableRow;
            var subTitle = GetField(_objectSocialConfig, "trSubTitle") as HtmlTableRow;
            var thumbnail = GetField(_objectSocialConfig, "trThumbnail") as HtmlTableRow;
            _objectSocialConfig.ShouldSatisfyAllConditions(
                () => errorMessage.ShouldNotBeNull(),
                () => message.ShouldNotBeNull(),
                () => title.ShouldNotBeNull(),
                () => subTitle.ShouldNotBeNull(),
                () => thumbnail.ShouldNotBeNull(),
                () => errorMessage.Visible.ShouldBeTrue(),
                () => message.Visible.ShouldBeTrue(),
                () => message.Value.ShouldBe(DummyString),
                () => title.Visible.ShouldBe(visibility),
                () => subTitle.Visible.ShouldBe(visibility),
                () => thumbnail.Visible.ShouldBe(visibility));
        }

        private void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }

        private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private void SetField(dynamic obj, string fieldName, dynamic fieldValue)
        {
            ReflectionHelper.SetField(obj, fieldName, fieldValue);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetField(obj, fieldName);
        }

        private void SetSessionVariable(string name, object fieldValue)
        {
            HttpContext.Current.Session.Add(name, fieldValue);
        }

        private void SetProperty(dynamic instance, string propertyName, dynamic value)
        {
            ReflectionHelper.SetProperty(instance, propertyName, value);
        }

        private dynamic GetProperty(dynamic instance, string propertyName)
        {
            return ReflectionHelper.GetPropertyValue(instance, propertyName);
        }
    }
}