using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMDbManagers.Fakes;
using KMEntities;
using KMEnums;
using KMManagers.Fakes;
using KMModels.PostModels;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using HtmlControlType = KMEnums.ControlType;
using ShimApplicationLog = KM.Common.Entity.Fakes.ShimApplicationLog;

namespace KMManagers.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class HTMLGeneratorTest
    {
        private const string Language = "EN";
        private const string UrlToContentKey = "UrlToContent";
        private const string SubmitFormHandlerUrlKey = "SubmitFormHandlerUrl";
        private const string PrepopulateFromDbHandlerUrlKey = "PrepopulateFromDbHandlerUrl";
        private const string BeginControl = "%begin_control%";
        private const string EndControl = "%end_control%";
        private const string Control = "%control%";
        private const string BeginForm = "%begin_form%";
        private const string EndForm = "%end_form%";
        private const string EmailRexPattern = "emailRexPattern";
        private const string UrlToContent = "UrlToContent";
        private const string SubmitFormHandlerUrl = "SubmitFormHandlerUrl";
        private const string PrepopulateFromDbHandlerUrl = "PrepopulateFromDbHandlerUrl";
        private const string KMCommonapplication = "12345";
        private const string PageRulesJsonMacros = "%pageRules%";
        private const string ButtonNamesJsonMacros = "%buttonNames%";
        private const string HiddenPart = "%hiddenPart%";
        private const int ConfirmPasswordSeqId = 100;
        private const int NumericPropertySeqId = 1000;
        private IDisposable _shimsContext;
        private HTMLGenerator _htmlGenerator;
        private List<string> _languageCodes;
        private NameValueCollection _appSettings;
        private bool _logNoneCriticalErrorCalled;
        private Form _resultForm;
        private bool _publishFormByIdCalled;
        private Exception _formManagerConstructorException;
        private List<KMEntities.Rule> _rules;
        private FormSubscriberLoginPostModel _subscriberLoginManagerGetByIdResult;
        private readonly Random _random = new Random();

        [SetUp]
        public void SetUp()
        {
            _logNoneCriticalErrorCalled = false;
            _appSettings = new NameValueCollection();
            _shimsContext = ShimsContext.Create();
            CommonShims();
            InitializeStaticFields();
        }

        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();
        }

        [Test]
        public void GenerateHTML_AutoSubmitForm_ReturnsWarning()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            _resultForm.FormType = FormType.AutoSubmit.ToString();
            _resultForm.Status = FormStatus.Saved.ToString();
            _resultForm.PublishAfter = DateTime.Now.AddMonths(-1);
            _htmlGenerator = new HTMLGenerator(Language);
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldBe("This is an Autosubmit Form and cannot be displayed.");
            _publishFormByIdCalled.ShouldBeTrue();
        }

        [Test]
        public void GenerateHTML_WhenExceptionThrown_ShouldLog()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            _htmlGenerator = new HTMLGenerator(Language);
            _formManagerConstructorException = new Exception(GetString());
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            _logNoneCriticalErrorCalled.ShouldBeTrue();
            result.ShouldBe(_formManagerConstructorException.Message);
        }

        [Test]
        public void GenerateHTML_NullHtml_ReturnsWarning()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{BeginForm}{BeginControl}{ButtonNamesJsonMacros}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = "";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            ShimSetUrlOrMessage(null);
            _htmlGenerator = new HTMLGenerator(Language);
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldBe("Cannot display Form.");
        }

        [Test]
        [TestCaseSource(nameof(GetControlIdAndExpectedGuid))]
        public void GenerateHTML_PageRules_AddedToHtml(int? controlId, Guid expectedGuid)
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{PageRulesJsonMacros}{BeginForm}{BeginControl}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = "";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            var rule = new KMEntities.Rule
            {
                ConditionGroup = new ConditionGroup(),
                Control_ID = controlId,
                Control = new Control
                {
                    HTMLID = expectedGuid
                },
                Type = (int)RuleTypes.Page
            };
            _rules.Add(rule);
            var expectedHtml = $"[{rule.ConditionGroup.ToJson()},\"{expectedGuid}\",[]]";
            _resultForm.Controls.Add(GetControl(HtmlControlType.TextArea, HtmlControlType.TextArea));
            _htmlGenerator = new HTMLGenerator(Language);
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldContain(expectedHtml);
        }

        [Test]
        public void GenerateHTML_PageBreakControl_AddedToHtml()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{ButtonNamesJsonMacros}{BeginForm}{BeginControl}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = "";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            var control = GetControl(HtmlControlType.PageBreak, HtmlControlType.PageBreak);
            _resultForm.Controls.Add(control);
            _htmlGenerator = new HTMLGenerator(Language);
            var expected = $"{control.HTMLID}";
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldContain(expected);
        }

        [Test]
        [TestCaseSource(nameof(GetControlTypes))]
        public void GenerateHTML_Controls_WillAddIdToHtml(HtmlControlType controlType, HtmlControlType seqType)
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{GetMacros()}{BeginForm}{HiddenPart}{BeginControl}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = "";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            var control = GetControl(controlType, seqType);
            _resultForm.Controls.Add(control);
            _htmlGenerator = new HTMLGenerator(Language);
            var expectedWithDashes = control.HTMLID.ToString();
            var expectedWithoutDashes = control.HTMLID.ToString().Replace("-", string.Empty);
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            AssertContainingAny(result, expectedWithDashes, expectedWithoutDashes);
        }

        [Test]
        public void GenerateHTML_SubscriberLoginManagerFormNull_ShouldCreateNew()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{GetMacros()}{BeginForm}{HiddenPart}{BeginControl}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = "";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            _subscriberLoginManagerGetByIdResult = null;
            _htmlGenerator = new HTMLGenerator(Language);
            var expectedHtml = $@"""GroupID"".+""SubIdLabel""";
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldMatch(expectedHtml);
        }

        [Test]
        public void GenerateHTML_SubscriberLoginManagerFormHasNoneNumericIdentification_ShouldUseItAsSubIdLabel()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{GetMacros()}{BeginForm}{HiddenPart}{BeginControl}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = "";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            _subscriberLoginManagerGetByIdResult.OtherIdentification = GetString();
            _htmlGenerator = new HTMLGenerator(Language);
            var expectedHtml = $@"""SubIdLabel"":""{_subscriberLoginManagerGetByIdResult.OtherIdentification}""";
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldContain(expectedHtml);
        }

        [Test]
        public void GenerateHTML_FormInactive_UseInactiveTemplate()
        {
            //Arrange
            var tokenUid = GetString();
            var child = true;
            var isSecureConnection = false;
            var htmlTemplate = $"{GetMacros()}{BeginForm}{HiddenPart}{BeginControl}{Control}{EndControl}{EndForm}";
            var inactiveHtmlTemplate = $"{GetString()}";
            GetStaticField("HTMLTemplate", htmlTemplate);
            GetStaticField("InactiveHTMLTemplate", inactiveHtmlTemplate);
            _resultForm.Active = (int)FormActive.Inactive;
            _htmlGenerator = new HTMLGenerator(Language);
            //Act
            var result = _htmlGenerator.GenerateHTML(tokenUid, child.ToString(), isSecureConnection);
            //Assert
            result.ShouldContain(inactiveHtmlTemplate);
        }

        private void AssertContainingAny(string text, string expectedWithDashes, string expectedWithoutDashes)
        {
            var results = new[]
            {
                text.Contains(expectedWithDashes),
                text.Contains(expectedWithoutDashes)
            };
            results.ShouldContain(true);
        }

        private static IEnumerable<HtmlControlType[]> GetControlTypes()
        {
            var typicalTypes = new[]
            {
                HtmlControlType.TextBox,
                HtmlControlType.Email,
                HtmlControlType.Country,
                HtmlControlType.State,
                HtmlControlType.TextArea,
                HtmlControlType.RadioButton,
                HtmlControlType.CheckBox,
                HtmlControlType.DropDown,
                HtmlControlType.ListBox,
                HtmlControlType.Grid,
                HtmlControlType.Hidden,
                HtmlControlType.NewsLetter,
                HtmlControlType.Literal,
                HtmlControlType.Captcha,
            };
            var result = typicalTypes.Select(type => new[] { type, type }).ToList();
            result.Add(new[] { HtmlControlType.TextBox, HtmlControlType.Password });
            return result;
        }

        private string GetMacros()
        {
            var macros = new[]
            {
                "%emailRex%",
                "%emailControlID%",
                "%allowChanges%",
                "%countryControlID%",
                "%stateControlID%",
                "%passwordControlID%",
                "%name%",
                "%validation%",
                "%captchaCallback%",
                "%prepopulate%",
                "%fieldRules%",
                "%pageRules%",
                "%formRules%",
                "%buttonNames%",
                "%comparisonTypes%",
                "%url%",
                "%message%",
                "%urlToContent%",
                "%GetFormHandlerUrl%",
                "%submit_url%",
                "%PrepopulateUrl%",
                "%PrepopulateDelay%",
                "%subLoginJson%"
            };
            return string.Join(string.Empty, macros);
        }

        private static IEnumerable<object[]> GetControlIdAndExpectedGuid()
        {
            return new object[][]
            {
                new object[]{null, Guid.Empty },
                new object[]{1, Guid.NewGuid() },
            };
        }

        private Control GetControl(HtmlControlType type, HtmlControlType seqId)
        {
            var control = new Control
            {
                HTMLID = Guid.NewGuid(),
                Type_Seq_ID = (int)seqId,
                ControlType = new KMEntities.ControlType
                {
                    MainType_ID = (int)type,
                    ControlType2 = new KMEntities.ControlType
                    {
                        Name = type.ToString()
                    }
                },
            };
            control.FormControlProperties.Add(new FormControlProperty
            {
                ControlProperty_ID = NumericPropertySeqId,
                ControlProperty = new ControlProperty
                {
                    PropertyName = "Grid Controls"
                },
                Value = "1"
            });
            control.FormControlProperties.Add(new FormControlProperty
            {
                ControlProperty_ID = ConfirmPasswordSeqId,
                ControlProperty = new ControlProperty(),
                Value = "true"
            });
            return control;
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetNumber()
        {
            return _random.Next(10, 1000);
        }

        private void CommonShims()
        {
            _appSettings.Add("MasterAccessKey", GetString());
            _appSettings.Add("GoogleCapthcaSiteKey", GetString());
            _appSettings.Add(UrlToContentKey, UrlToContent);
            _appSettings.Add(SubmitFormHandlerUrlKey, SubmitFormHandlerUrl);
            _appSettings.Add(PrepopulateFromDbHandlerUrlKey, PrepopulateFromDbHandlerUrl);
            _appSettings.Add("MasterAccessKey", GetString());
            _appSettings.Add("KMCommon_Application", KMCommonapplication);

            ShimHTMLGenerator.StaticConstructor = () => { };
            ShimUser.GetByAccessKeyStringBoolean = (accessKey, getChildren) =>
            {
                return new User();
            };
            ShimUser.AllInstances.SelectUserInt32Boolean = (instance, userId, includeObjects) =>
            {
                return new User();
            };
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var actualSettings = ShimsContext.ExecuteWithoutShims(() => ConfigurationManager.AppSettings);
                var result = new NameValueCollection(_appSettings);
                result.Add(actualSettings);
                return result;
            };
            ShimManagerBase.Constructor = instance => { };
            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, sourceMethod, applicationId, note, gdCharityId, ecnCustomerId) =>
                {
                    _logNoneCriticalErrorCalled = true;
                };
            ShimFormManager.Constructor = instance =>
            {
                if (_formManagerConstructorException != null)
                {
                    throw _formManagerConstructorException;
                }
            };
            ShimFormManager.AllInstances.PublishFormByIDUserInt32Int32 = (instance, user, channelId, id) =>
            {
                _publishFormByIdCalled = true;
            };
            _resultForm = new Form
            {
                GroupID = GetNumber()
            };
            ShimFormDbManager.AllInstances.GetChildByTokenUIDString = (instance, token) =>
            {
                return _resultForm;
            };
            ShimFormDbManager.AllInstances.GetByTokenUIDString = (instance, uid) =>
            {
                return _resultForm;
            };
            ShimCssFileManager.Constructor = instance => { };
            _rules = new List<KMEntities.Rule>();
            ShimRuleManager.AllInstances.GetAllByFormIDInt32 = (instance, id) =>
            {
                return _rules;
            };
            ShimDataTypePatternManager.AllInstances.GetPatternByTypeTextboxDataTypes = (instance, textBoxDataTypes) =>
            {
                return EmailRexPattern;
            };
            _subscriberLoginManagerGetByIdResult = new FormSubscriberLoginPostModel
            {
                OtherIdentification = GetNumber().ToString()
            };
            ShimSubscriberLoginManager.AllInstances.GetByIDOf1Int32((instance, id) =>
            {
                return _subscriberLoginManagerGetByIdResult;
            });
            ShimGroupDataFields.GetByIDInt32User = (id, user) =>
            {
                return new GroupDataFields();
            };
            ShimControlPropertyManager.AllInstances.GetPropertyByNameAndControlStringControl = (instance, name, 
                control) =>
            {
                return GetProperty(name);
            };
            ShimControlPropertyManager.AllInstances.GetRequiredPropertyByControlControl = (instance, control) =>
             {
                 return new ControlProperty();
             };
            ShimControlPropertyManager.AllInstances.GetValuePropertyByControlControl = (instance, control) =>
            {
                return new ControlProperty();
            };
            ShimKMEntitiesWrapper.IsRequiredControl = control => true;
            ShimKMEntitiesWrapper.GetValidationRulesControlPropertyManagerControlDataTypePatternManager =
                (instance, control, patternManager) =>
                {
                    return GetString();
                };
            ShimHTMLGenerator.AllInstances.GetPrepopulateOptionControlControlPropertyManager =
                (instance, control, propertyManager) =>
                {
                    return GetString();
                };
            ShimHTMLGenerator.AllInstances.GetGridControl = (instance, control) =>
            {
                return $"Grid for control {control.HTMLID}";
            };
            ShimHTMLGenerator.AllInstances.GetNewsletterControl = (instacne, control) =>
            {
                return $"News Letter form control {control.HTMLID}";
            };
        }

        private static ControlProperty GetProperty(string name)
        {
            var value = name;
            var seqId = 0;
            var numericProperties = new[]
            {
                "Grid Controls",
                "Number of Columns"
            };
            if (name == "Confirm Password")
            {
                seqId = ConfirmPasswordSeqId;
                value = "true";
            }
            else if (numericProperties.Contains(name))
            {
                value = "10";
                seqId = NumericPropertySeqId;
            }
            return new ControlProperty
            {
                ControlProperty_Seq_ID = seqId,
                PropertyName = name,
                PropertyValues = value
            };
        }

        private void ShimSetUrlOrMessage(string htmlResult)
        {
            ShimHTMLGenerator.AllInstances.SetUrlOrMessageStringRefFormBoolean =
                (HTMLGenerator instance, ref string html, Form form, bool isActive) =>
                {
                    html = htmlResult;
                };
        }

        private void InitializeStaticFields()
        {
            _languageCodes = GetStaticField<List<string>>("LanguageCodes", new List<string>());
            _languageCodes.Add(Language);
        }

        private T GetStaticField<T>(string name, T initializeValue = null) where T : class
        {
            var staticFlags = BindingFlags.Static | BindingFlags.NonPublic;
            var field = typeof(HTMLGenerator)
                .GetField(name, staticFlags);
            field.ShouldNotBeNull();
            field.FieldType.ShouldBe(typeof(T));
            if (initializeValue != null)
            {
                field.SetValue(null, initializeValue);
            }
            var result = field.GetValue(null) as T;
            result.ShouldNotBeNull();
            return result;
        }
    } 
}
