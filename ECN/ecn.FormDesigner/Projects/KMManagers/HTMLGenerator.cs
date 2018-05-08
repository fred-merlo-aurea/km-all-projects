using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using KM.Common.Entity;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMModels.PostModels;
using Newtonsoft.Json;
using ControlType = KMEnums.ControlType;

namespace KMManagers
{
    public class HTMLGenerator
    {
        private static TranslateService ts = null;
        private static List<string> LanguageCodes = new List<string>();

        private static Dictionary<string, string> Languages = new Dictionary<string, string>();
        private static Dictionary<int, DateTime> LastAccessTime = new Dictionary<int, DateTime>();
        private static readonly string HTMLTemplate;
        private static readonly string InactiveHTMLTemplate;
        private string UrlToContentValue;
        //private static readonly string GetFormHandlerUrlValue;
        private string SubmitFormHandlerUrlValue;
        private string PrepopulateFromDbHandlerUrlValue;
        private static readonly int PrepopulateDelayValue = 3000;
        public static readonly int IntervalSec;
        private static readonly string GoogleCapthaSiteKey;
        private RuleManager RM = new RuleManager();

        private const string HTMLTemplatePathKey = "HTMLTemplatePath";
        private const string InactiveHTMLTemplatePathKey = "InactiveHTMLTemplatePath";
        private const string ApplicationName = "GoogleAppName";
        private const string ApiKey = "GoogleApiKey";
        private const string GCSiteKey = "GoogleCapthcaSiteKey";
        private const string GSiteKeyMacros = "%gsitekey%";
        private const string RecaptchaLangCodeMacros = "%recaptcha_lang_code%";
        private const string UrlToContentKey = "UrlToContent";
        private const string SubmitFormHandlerUrlKey = "SubmitFormHandlerUrl";
        private const string PrepopulateFromDbHandlerUrlKey = "PrepopulateFromDbHandlerUrl";
        private const string PrepopulateDelayKey = "PrepopulateDelayMs";
        private const string IntervalSecKey = "IntervalSec";
        private const string Title = "%title%";
        public const string CSS = "%css%";
        private const string Header = "%html_header%";
        private const string TranslateBlock = "%translate_block%";
        private const string Footer = "%html_footer%";
        private const string BeginControl = "%begin_control%";
        private const string EndControl = "%end_control%";
        private const string Control = "%control%";
        private const string BeginForm = "%begin_form%";
        private const string EndForm = "%end_form%";
        private const string FormName = "%formName%";
        private const string HiddenPart = "%hiddenPart%";
        private const string NavigatePart = "%navigatePart%";
        private const string radio = "radio";
        private const string IDAttribute = "id";
        private const string NameAttribute = "name";
        private const string ValueAttribute = "value";
        private const string ForAttribute = "for";
        private const string OnClickAttribute = "onclick";
        private const string SizeAttribute = "size";
        private const string StyleAttribute = "style";
        private const string Multiple = "multiple";
        private const string Checked = "checked";
        private const string Selected = "selected";
        private const string StandardSelector_DefaultValue = "";
        private const string DivGridError = "<div class=\"grid_error\"></div>";
        private const string TextboxHTML = "<input type=\"text\" />";
        private const string PasswordHTML = "<input type=\"password\" />";
        private const string HiddenHTML = "<input type=\"hidden\" />";
        public const string Subscribe = "Subscribe";
        public const string Unsubscribe = "Unsubscribe";
        private const string emailRexMacros = "%emailRex%";
        private const string emailControlIDMacros = "%emailControlID%";
        private const string emailControlAllowChangesMacros = "%allowChanges%";
        private const string countryControlIDMacros = "%countryControlID%";
        private const string stateControlIDMacros = "%stateControlID%";
        private const string passwordControlIDMacros = "%passwordControlID%";
        private const string PreviousButton = "<button class=\"btn_prev_form nav\">%name%</button>";
        public const string PreviousButton_Property = "Previous";
        private const string NextButton = "<button class=\"btn_next_form nav\">%name%</button>";
        public const string NextButton_Property = "Next";
        private const string SubmitButton = "<button class=\"btn_submit_form nav\">%name%</button>";
        public const string SubmitButton_Property = "Submit";
        //public const string Captcha_Property = "Captcha";
        private const string ButtonNameMacros = "%name%";
        public const string PrevButtonDefaultText = "Previous";
        public const string NextButtonDefaultText = "Next";
        public const string Columns_Property = "Columns";
        public const string NumberofColumns_Property = "Number of Columns";
        public const string Rows_Property = "Rows";
        public const string DataType_Property = "Data Type";
        public const string Regex_Property = "Regular expression";
        public const string StrLen_Property = "Characters";
        public const string ValuesCount_Property = "Number of Values allowed";
        public const string FieldSize_Property = "Field Size";
        public const string PrepopulateFrom_Property = "Prepopulate from";
        public const string QueryString_Property = "Querystring Parameter";
        public const string Letter_Type_Property = "LetterType";
        public const string CustomerID_Property = "CustomerID";
        public const string GroupID_Property = "GroupID";
        public const string AccessKey_Property = "CustomerAccessKey";
        public const string GridValidation_Property = "Grid Validation";
        public const string GridControls_Property = "Grid Controls";
        public const string Value_Property = "Value";
        private const string validationJsonMacros = "%validation%";
        private const string captchaCallbackMacros = "%captchaCallback%";
        private const string prepopulateJsonMacros = "%prepopulate%";
        private const string fieldRulesJsonMacros = "%fieldRules%";
        private const string pageRulesJsonMacros = "%pageRules%";
        private const string formRulesJsonMacros = "%formRules%";
        private const string buttonNamesJsonMacros = "%buttonNames%";
        private const string comparisonTypesMacros = "%comparisonTypes%";
        private const string url = "url";
        private const string message = "message";
        public const string urlMacros = "%url%";
        private const string messageMacros = "%message%";
        private const string urlToContentMacros = "%urlToContent%";
        //private const string getFormHandlerUrlMacros = "%GetFormHandlerUrl%";
        private const string submitFormHandlerUrlMacros = "%submit_url%";
        private const string prepopulateUrlMacros = "%PrepopulateUrl%";
        private const string prepopulateDelayMacros = "%PrepopulateDelay%";
        public const string HiddenIDForToken = "km_form_token";
        public const string HiddenIDForTimers = "km_form_timers";
        private const string HiddenWithFormToken = "<input id=\"" + HiddenIDForToken + "\" name=\"" + HiddenIDForToken + "\" type=\"hidden\" value=\"{0}\" />";
        private const string HiddenWithFormTimers = "<input id=\"" + HiddenIDForTimers + "\" name=\"" + HiddenIDForTimers + "\" type=\"hidden\" value=\"{0}\" />";
        public const string HiddenIDForRecaptcha = "g_resp";
        private const string HiddenWithRecaptcha = "<input id=\"" + HiddenIDForRecaptcha + "\" name=\"" + HiddenIDForRecaptcha + "\" type=\"hidden\" />";
        private string RecaptchaHTML = "<div class=\"g-recaptcha\" data-sitekey=\"" + ConfigurationManager.AppSettings["GoogleCapthcaSiteKey"].ToString() + "\"></div>";
        private const string RecaptchaValidateHTML = "<br /><input type=\"hidden\" class=\"dontIgnore\" id=\"captchaValidate\" name=\"captchaValidate\" />";
        private const string RecaptchaCallBackJS = "";
        private const string en = "en";
        public const string NewLine = "\r\n";
        private const string OpenDiv = "<div>";
        private const string CloseDiv = "</div>";
        private const string OpenSpan = "<span>";
        private const string OpenInvisibleSpan = "<span style=\"display:none\">";
        private const string CloseSpan = "</span>";
        private const string RequiredAsterisk = " <span class=\"km_asterisk\">*</span>";
        private const string MessageDelay = "%messageDelay%";
        private const string InIframe = "%inIframe%";
        private const string SubLoginJson = "%subLoginJson%";
        private const string TrueValue = "true";
        private const string CannotBeDisplayed = "This is an Autosubmit Form and cannot be displayed.";
        private const string HttpHeader = "http://";
        private const string HttpsHeader = "https://";
        private const string Slash = "/";
        private const char SlashCharacter = '/';
        private const string AllowChanges = "Allow Changes";
        private const string Yes = "yes";
        private const string ConfirmPassword = "Confirm Password";
        private const string FalseValue = "false";
        private const string ConfirmPasswordLabelHtml = "Confirm Password LabelHTML";
        private const string NameButton = "Button";
        private const string KmCommonApplication = "KMCommon_Application";
        private const string CannotDisplayForm = "Cannot display Form.";
        private string _lang;

        private string lang;

        static HTMLGenerator()
        {
            //get templates
            StreamReader sr = new StreamReader
            (
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[HTMLTemplatePathKey]
            );
            HTMLTemplate = sr.ReadToEnd();
            sr.Close();
            sr = new StreamReader
            (
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[InactiveHTMLTemplatePathKey]
            );
            InactiveHTMLTemplate = sr.ReadToEnd();
            sr.Close();

            //load languages
            ts = new TranslateService(new BaseClientService.Initializer()
            {
                ApplicationName = WebConfigurationManager.AppSettings[ApplicationName],
                ApiKey = WebConfigurationManager.AppSettings[ApiKey]
            });
            IList<Google.Apis.Translate.v2.Data.LanguagesResource> languages = null;
            try
            {
                languages = ts.Languages.List().Execute().Languages;
            }
            catch
            { }
            if (languages != null)
            {

                LanguageCodes.AddRange(languages.Select(x => x.Language));
                FillLanguages();
            }
            
            //GetFormHandlerUrlValue = WebConfigurationManager.AppSettings[GetFormHandlerUrlKey];
            //SubmitFormHandlerUrlValue = WebConfigurationManager.AppSettings[SubmitFormHandlerUrlKey];
            //PrepopulateFromDbHandlerUrlValue = WebConfigurationManager.AppSettings[PrepopulateFromDbHandlerUrlKey];
            try
            {
                PrepopulateDelayValue = int.Parse(WebConfigurationManager.AppSettings[PrepopulateDelayKey]);
            }
            catch
            { }
            if (PrepopulateDelayValue < 10)
            {
                PrepopulateDelayValue *= 1000;
            }
            try
            {
                IntervalSec = int.Parse(WebConfigurationManager.AppSettings[IntervalSecKey]);
            }
            catch
            { }
            if (IntervalSec < 1)
            {
                IntervalSec = -1;
            }
            GoogleCapthaSiteKey = WebConfigurationManager.AppSettings[GCSiteKey];
            //GoogleCapthaSecret = WebConfigurationManager.AppSettings[GCSecret];
        }

        private static void FillLanguages()
        {
            List<string> acceptedLangs = new List<string>() { "af", "eu", "ca", "da", "nl", "en", "fi", "fr", "gl", "de", "is", "ga", "it", "no", "pt", "es", "sv" };
            foreach (var l in LanguageCodes)
            {
                if (acceptedLangs.Contains(l))
                {
                    LanguagesResource.ListRequest request = ts.Languages.List();
                    request.Target = l;
                    IList<Google.Apis.Translate.v2.Data.LanguagesResource> res = null;
                    try
                    {
                        res = request.Execute().Languages;

                        Languages.Add(l, res.Single(x => x.Language == l).Name);

                    }
                    catch
                    { }
                }
            }
        }

        public HTMLGenerator(string lang)
        {
            this.lang = (lang ?? string.Empty).ToLower();
            if (!LanguageCodes.Contains(this.lang))
            {
                this.lang = null;
            }            
        }

        private FormSubscriberLoginPostModel GetFormSubscriberModel(int formId, int groupId)
        {
            var SL = new SubscriberLoginManager();
            var formSL = SL.GetByID<FormSubscriberLoginPostModel>(formId);
            if (formSL == null)
            {
                formSL = new FormSubscriberLoginPostModel();
            }
            formSL.GroupID = groupId;
            formSL.SubIdLabel = "";
            if (!string.IsNullOrEmpty(formSL.OtherIdentification))
            {
                int udfID = 0;
                if (Int32.TryParse(formSL.OtherIdentification, out udfID))
                {
                    formSL.SubIdLabel = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(udfID, KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["MasterAccessKey"].ToString(), true)).ShortName;
                }
                else
                {
                    formSL.SubIdLabel = formSL.OtherIdentification;
                }
            }
            return formSL;
        }

        public string GenerateHTML(string tokenuid, string child, bool isSecureConnection)
        {
            const string CurrentMethodName = "KMManagers.HTMLGenerator.GenerateHTML";
            string html = null;

            try
            {
                var isChild = child?.Equals(TrueValue, StringComparison.OrdinalIgnoreCase) == true;
                var formManager = new FormManager();
                var formDbManager = new FormDbManager();
                var form = isChild
                               ? formDbManager.GetChildByTokenUID(tokenuid)
                               : formDbManager.GetByTokenUID(tokenuid);
                var user = new KMPlatform.BusinessLogic.User().SelectUser(form.UserID, false);

                if (form.Status == FormStatus.Saved.ToString() && form.PublishAfter.HasValue && form.PublishAfter.Value < DateTime.Now)
                {
                    formManager.PublishFormByID(user, form.CustomerID, form.Form_Seq_ID);
                }

                if (form.FormType == FormType.AutoSubmit.ToString())
                {
                    return CannotBeDisplayed;
                }

                UrlToContentValue = isSecureConnection
                                        ? WebConfigurationManager.AppSettings.Get(UrlToContentKey).Replace(HttpHeader, HttpsHeader)
                                        : WebConfigurationManager.AppSettings[UrlToContentKey];
                SubmitFormHandlerUrlValue = isSecureConnection
                                                ? WebConfigurationManager.AppSettings.Get(SubmitFormHandlerUrlKey).Replace(HttpHeader, HttpsHeader)
                                                : WebConfigurationManager.AppSettings[SubmitFormHandlerUrlKey];
                PrepopulateFromDbHandlerUrlValue = isSecureConnection
                                                       ? WebConfigurationManager.AppSettings.Get(PrepopulateFromDbHandlerUrlKey).Replace(HttpHeader, HttpsHeader)
                                                       : WebConfigurationManager.AppSettings[PrepopulateFromDbHandlerUrlKey];

                if (!UrlToContentValue.EndsWith(Slash))
                {
                    UrlToContentValue += SlashCharacter;
                }

                html = GenerateHtml(isSecureConnection, form);
            }
            catch (Exception ex)
            {
                html = ex.Message;
                var note = $"FormToken: {tokenuid}, Child:{child}";
                ApplicationLog.LogNonCriticalError(ex, CurrentMethodName, Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplication]), note);
            }

            return html ?? CannotDisplayForm;
        }

        private string GenerateHtml(bool isSecureConnection, Form form)
        {
            string html;
            var cssFileManager = new CssFileManager();
            var isActive = form.IsActive();

            if (isActive)
            {
                html = GeneralHtmlActiveForm(isSecureConnection, form, cssFileManager);
            }
            else
            {
                html = InactiveHTMLTemplate;
                html = html.Replace(Title, form.Name)
                    .Replace(
                        CSS,
                        isSecureConnection
                            ? cssFileManager.GetURLByForm(form).Replace(HttpHeader, HttpsHeader)
                            : cssFileManager.GetURLByForm(form))
                    .Replace(urlToContentMacros, UrlToContentValue);
            }

            SetUrlOrMessage(ref html, form, isActive);
            return html;
        }

        private string GeneralHtmlActiveForm(bool isSecureConnection, Form form, CssFileManager cssFileManager)
        {
            var controls = form.Controls.OrderBy(x => x.Order).ToList();
            var formsCount = controls.Count(x => x.HTMLControlType() == ControlType.PageBreak);

            if (controls.Count == 0 || !ControlIsPageBreak(controls.Last()))
            {
                formsCount++;
            }

            var rules = RM.GetAllByFormID(form.Form_Seq_ID);
            var prepopulateJson = new List<string>();
            var pageRulesJson = new List<string>();

            foreach (var rule in rules.Where(x => x.Type == (int)RuleTypes.Page))
            {
                pageRulesJson.Add(GetPageRule(rule));
            }

            var htmlParts = HTMLTemplate.Split(new[] { BeginForm, EndForm }, StringSplitOptions.None);
            var formParts = htmlParts[1].Split(new[] { BeginControl, EndControl }, StringSplitOptions.None);
            var emailControlId = ProcessControls(controls, prepopulateJson, formParts, form, formsCount);

            var buttonsHtml = new StringBuilder(PreviousButton.Replace(ButtonNameMacros, Translate(PrevButtonDefaultText)));
            var headerJs = form.HeaderJs ?? string.Empty;
            var footerJs = form.FooterJs ?? string.Empty;

            buttonsHtml.Append(NextButton.Replace(ButtonNameMacros, Translate(NextButtonDefaultText)))
                .Append(SubmitButton.Replace(ButtonNameMacros, Translate(form.SubmitButtonText)));
            return PopulateHtml(isSecureConnection, htmlParts, form, cssFileManager, headerJs, footerJs, buttonsHtml.ToString(), prepopulateJson, pageRulesJson, emailControlId);
        }

        private string ProcessControls(List<Control> controls, List<string> prepopulateJson, string[] formParts, Form form, int formsCount)
        {
            var emailControlId = string.Empty;
            var content = string.Empty;
            var hiddenPart = string.Empty;

            for (var controlIndex = 0; controlIndex < controls.Count; controlIndex++)
            {
                var controlIsPageBreak = ControlIsPageBreak(controls[controlIndex]);

                if (controlIsPageBreak)
                {
                    HtmlParams.ButtonNamesJson.Add(GetButtonNames(controls[controlIndex]));
                    HtmlParams.PageBreakIds.Add($"{'"'}{controls[controlIndex].HTMLID}{'"'}");
                }
                else
                {
                    emailControlId = ProcessControlNotPageBreak(controls, prepopulateJson, formParts, form, controlIndex, emailControlId, ref hiddenPart, ref content);
                }

                if (controlIndex == controls.Count - 1 || controlIsPageBreak)
                {
                    if (controlIndex == controls.Count - 1)
                    {
                        var hiddenPartStringBuilder = new StringBuilder(hiddenPart);
                        hiddenPartStringBuilder.Append($"{NewLine}{string.Format(HiddenWithFormToken, form.TokenUID)}")
                            .Append($"{NewLine}{HiddenWithRecaptcha}")
                            .Append($"{NewLine}{string.Format(HiddenWithFormTimers, GetZeros(formsCount))}");
                        hiddenPart = hiddenPartStringBuilder.ToString();
                    }

                    var newForm = $"{formParts[0]}{content}{formParts[2]}";
                    content = string.Empty;
                    var replaceString = controlIsPageBreak
                                            ? $"id=\"{controls[controlIndex].HTMLID}{'"'}"
                                            : string.Empty;
                    newForm = newForm.Replace(FormName, replaceString);
                    newForm = newForm.Replace(HiddenPart, hiddenPart);
                    hiddenPart = string.Empty;
                    HtmlParams.Body += newForm;
                }
            }

            return emailControlId;
        }

        private string ProcessControlNotPageBreak(List<Control> controls, List<string> prepopulateJson, string[] formParts, Form form, int controlIndex, string emailControlId, ref string hiddenPart, ref string content)
        {
            var item = HtmlParams.ControlPropertyManager.GetValidationRules(controls[controlIndex], HtmlParams.DataTypePatternManager);

            if (!string.IsNullOrWhiteSpace(item))
            {
                HtmlParams.ValidationRules.Add(item);
            }

            item = GetPrepopulateOption(controls[controlIndex], HtmlParams.ControlPropertyManager);

            if (!string.IsNullOrWhiteSpace(item))
            {
                prepopulateJson.Add(item);
            }

            HtmlParams.FieldRulesJson.AddRange(GetFieldRules(controls[controlIndex]));

            var htmlControlType = controls[controlIndex].HTMLControlType();
            var typeSeqId = (ControlType)controls[controlIndex].Type_Seq_ID;
            var controlName = controls[controlIndex].HTMLControlTypeName().ToLower();
            var cssClass = $"{controlName.Substring(0, 1).ToUpper()}{controlName.Substring(1)}";
            var newControl = formParts[1];
            var htmlId = controls[controlIndex].HTMLID.ToString();
            var translation = Translate(controls[controlIndex].FieldLabelHTML ?? string.Empty);

            if (controls[controlIndex].IsRequired() || htmlControlType == ControlType.Grid && CheckGrid(controls[controlIndex]))
            {
                translation += RequiredAsterisk;
            }

            if (controls[controlIndex].Type_Seq_ID == (int)ControlType.Email)
            {
                emailControlId = htmlId;
                HtmlParams.AllowChanges = controls[controlIndex].GetFormPropertyValue(AllowChanges) == null
                                              ? Yes
                                              : controls[controlIndex].GetFormPropertyValue(AllowChanges).ToLower();
                var formSubscriber = GetFormSubscriberModel(form.Form_Seq_ID, form.GroupID);

                if (formSubscriber.LoginRequired)
                {
                    var listDiv = new StringBuilder();
                    listDiv.Append("<div class=\"kmButton\" style=\"display:inline; margin: 0; padding: 0\">")
                        .Append("<button type=\"button\" style=\"margin-bottom: 0;\" id=\"FormChangeEmail\" onclick=\"ChangeEmailShow(false)\">Change Email</button>")
                        .Append("</div>");
                    newControl = newControl.Replace("</li>", $"{listDiv}</li>");
                }
            }

            SetHtmlParamsIds(controls, controlIndex, htmlId);

            newControl = ProcessHtmlControlType(htmlControlType, controls, controlIndex, newControl, typeSeqId, htmlId, translation, cssClass, ref hiddenPart);

            content += newControl;
            return emailControlId;
        }

        private static void SetHtmlParamsIds(List<Control> controls, int controlIndex, string htmlId)
        {
            if (controls[controlIndex].Type_Seq_ID == (int)ControlType.Country)
            {
                HtmlParams.CountryControlId = htmlId;
            }

            if (controls[controlIndex].Type_Seq_ID == (int)ControlType.State)
            {
                HtmlParams.StateControlId = htmlId;
            }

            if (controls[controlIndex].Type_Seq_ID == (int)ControlType.Password)
            {
                HtmlParams.PasswordControlId = htmlId;
            }
        }

        private string PopulateHtml(
            bool isSecureConnection,
            string[] htmlParts,
            Form form,
            CssFileManager cssM,
            string headerJs,
            string footerJs,
            string buttonsHtml,
            List<string> prepopulateJson,
            List<string> pageRulesJson,
            string emailControlId)
        {
            return $"{htmlParts[0]}{HtmlParams.Body}{htmlParts[2]}".Replace(Title, form.Name)
                .Replace(
                    CSS,
                    isSecureConnection
                        ? cssM.GetURLByForm(form).Replace(HttpHeader, HttpsHeader)
                        : cssM.GetURLByForm(form))
                .Replace(urlToContentMacros, UrlToContentValue)
                .Replace(submitFormHandlerUrlMacros, SubmitFormHandlerUrlValue)
                .Replace(prepopulateUrlMacros, PrepopulateFromDbHandlerUrlValue)
                .Replace(prepopulateDelayMacros, PrepopulateDelayValue.ToString())
                .Replace(Header, $"{headerJs}{TranslateHTML(form.HeaderHTML ?? string.Empty)}")
                .Replace(Footer, $"{footerJs}{TranslateHTML(form.FooterHTML ?? string.Empty)}")
                .Replace(
                    TranslateBlock,
                    form.LanguageTranslationType
                        ? GetTranslateBlock()
                        : string.Empty)
                .Replace(NavigatePart, WrapDiv(null, buttonsHtml, NameButton))
                .Replace(
                    RecaptchaLangCodeMacros,
                    _lang == null
                        ? string.Empty
                        : $"&hl={_lang}")
                .Replace(GSiteKeyMacros, GoogleCapthaSiteKey)
                .Replace(validationJsonMacros, string.Join(NewLine, HtmlParams.ValidationRules))
                .Replace(prepopulateJsonMacros, $"{'{'}{string.Join(",", prepopulateJson)}{'}'}")
                .Replace(fieldRulesJsonMacros, $"{'['}{string.Join(",", HtmlParams.FieldRulesJson)}{']'}")
                .Replace(pageRulesJsonMacros, $"{'['}{string.Join(",", pageRulesJson)}{']'}")
                .Replace(formRulesJsonMacros, $"{'['}{string.Join(",", GetFormRules(form))}{']'}")
                .Replace(buttonNamesJsonMacros, $"{'{'}{string.Join(",", HtmlParams.ButtonNamesJson)}{'}'}")
                .Replace(comparisonTypesMacros, GetComparisonTypesJson())
                .Replace(emailRexMacros, HttpUtility.JavaScriptStringEncode(HtmlParams.DataTypePatternManager.GetPatternByType(TextboxDataTypes.Email)))
                .Replace(emailControlIDMacros, emailControlId)
                .Replace(emailControlAllowChangesMacros, HtmlParams.AllowChanges)
                .Replace(countryControlIDMacros, HtmlParams.CountryControlId)
                .Replace(stateControlIDMacros, HtmlParams.StateControlId)
                .Replace(passwordControlIDMacros, HtmlParams.PasswordControlId)
                .Replace(MessageDelay, form.Delay.ToString())
                .Replace(InIframe, form.Iframe.ToString().ToLower())
                .Replace(SubLoginJson, JsonConvert.SerializeObject(GetFormSubscriberModel(form.Form_Seq_ID, form.GroupID)));
        }

        private string ProcessHtmlControlType(
            ControlType htmlControlType,
            List<Control> controls,
            int i,
            string newControl,
            ControlType typeSeqId,
            string htmlId,
            string translation,
            string cssClass,
            ref string hiddenPart)
        {
            switch (htmlControlType)
            {
                case ControlType.TextBox:
                    newControl = ProcessHtmlControlTypeTextBox(controls, i, newControl, typeSeqId, htmlId, translation, cssClass);
                    break;
                case ControlType.TextArea:
                    newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, GetTextarea(controls[i], cssClass), translation));
                    break;
                case ControlType.RadioButton:
                case ControlType.CheckBox:
                    newControl = newControl.Replace(Control, GetInputWithType(controls[i], htmlControlType, translation, cssClass, HtmlParams.ControlPropertyManager));
                    break;
                case ControlType.DropDown:
                    newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, GetSelect(controls[i], false, HtmlParams.ControlPropertyManager), translation, cssClass));
                    break;
                case ControlType.ListBox:
                    newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, GetSelect(controls[i], true, HtmlParams.ControlPropertyManager), translation, cssClass));
                    break;
                case ControlType.Grid:
                    newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, GetGrid(controls[i]), translation, cssClass));
                    break;
                case ControlType.Hidden:
                    newControl = string.Empty;
                    var hiddenHtml = AddIDAndNameAttributes(HiddenHTML, htmlId);
                    hiddenHtml = AddValueAttribute(hiddenHtml, controls[i].GetFormPropertyValue(Value_Property), false);
                    hiddenPart += $"{NewLine}{hiddenHtml}";
                    break;
                case ControlType.NewsLetter:
                    newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, GetNewsletter(controls[i]), translation, cssClass));
                    break;
                case ControlType.Literal:
                    var literalHtml = $"{AddIDAndNameAttributes(OpenDiv, htmlId)}{controls[i].GetFormPropertyValue(Value_Property)}{CloseDiv}";
                    newControl = newControl.Replace(Control, literalHtml);
                    break;
                case ControlType.Captcha:
                    newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, GetCaptcha(controls[i].HTMLID), translation, cssClass));
                    break;
            }

            return newControl;
        }

        private string ProcessHtmlControlTypeTextBox(List<Control> controls, int i, string newControl, ControlType typeSeqId, string htmlId, string translation, string cssClass)
        {
            if (controls[i].Type_Seq_ID == (int)ControlType.Password)
            {
                newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, AddIDAndNameAttributes(PasswordHTML, htmlId), translation, cssClass));
                var confirmPassword = controls[i].GetFormPropertyValue(ConfirmPassword) == null
                                          ? FalseValue
                                          : controls[i].GetFormPropertyValue(ConfirmPassword).ToLower();
                var confirmPasswordLabelHtml = controls[i].GetFormPropertyValue(ConfirmPasswordLabelHtml) == null
                                                   ? ConfirmPassword
                                                   : controls[i].GetFormPropertyValue(ConfirmPasswordLabelHtml);

                if (confirmPassword == TrueValue)
                {
                    var listElementBuilder = new StringBuilder();

                    listElementBuilder.Append("\r\n <li class=\"liElement\">\r\n\t\t <div class=\"kmTextbox\">\r\n<label class=\"cleanLabel\">")
                        .Append(confirmPasswordLabelHtml)
                        .Append("</label>\r\n\r\n<input type=\"password\" id=\"ConfirmPassword_Control\" name=\"ConfirmPassword_Control\" />\r\n</div>\r\n </li>\r\n\t ");

                    newControl += listElementBuilder.ToString();
                }
            }
            else
            {
                newControl = newControl.Replace(Control, GetFullHTML(typeSeqId, AddIDAndNameAttributes(TextboxHTML, htmlId), translation, cssClass));
            }

            return newControl;
        }

        private bool CheckGrid(Control grid)
        {
            GridValidation validation = (GridValidation)int.Parse(grid.GetFormPropertyValue(GridValidation_Property));

            return validation != GridValidation.NotRequired;
        }

        private void SetUrlOrMessage(ref string html, Form form, bool isActive)
        {
            FormResult fr = form.FormResults.SingleOrDefault(x => x.ResultType == (int)(isActive ? FormResultType.ConfirmationPage : FormResultType.InactiveRedirect));
            string url = string.Empty;
            string message = string.Empty;
            if (fr != null)
            {
                url = fr.URL ?? string.Empty;
                message = fr.Message ?? string.Empty;
            }
            url = KMEntitiesWrapper.GetURL(url);
            html = html.Replace(urlMacros, HttpUtility.UrlEncode(url));
            //html = html.Replace(messageMacros, HttpUtility.JavaScriptStringEncode(Translate(message)));
            html = html.Replace(messageMacros, HttpUtility.JavaScriptStringEncode(message));
        }

        private bool CheckLastTime(int formId)
        {
            bool allow = true;
            if (IntervalSec > -1)
            {
                lock (typeof(HTMLGenerator))
                {
                    DateTime now = DateTime.Now;
                    if (LastAccessTime.ContainsKey(formId))
                    {
                        DateTime prev = LastAccessTime[formId];
                        allow = now.Subtract(prev).TotalSeconds >= IntervalSec;
                        if (allow)
                        {
                            LastAccessTime[formId] = now;
                        }
                    }
                    else
                    {
                        LastAccessTime.Add(formId, now);
                    }
                }
            }

            return allow;
        }

        private string TranslateHTML(string html)
        {
            string res = html;
            if (lang != null && !string.IsNullOrEmpty(res))
            {
                string[] data = res.Split('>');
                for (int i = 0; i < data.Length; i++)
                {
                    int index = data[i].IndexOf('<');
                    if (index == -1)
                    {
                        data[i] = Translate(data[i]);
                    }
                    else
                    {
                        data[i] = Translate(data[i].Substring(0, index)) + data[i].Substring(index);
                    }
                }
                res = string.Join(">", data);
            }

            return res;
        }

        private string Translate(string text)
        {
            string res = text;
            if (lang != null && res != string.Empty)
            {
                List<string> data = new List<string>();
                data.Add(res);
                try
                {
                    res = ts.Translations.List(data, lang).Execute().Translations[0].TranslatedText;
                }
                catch
                { }
                if (string.IsNullOrEmpty(res))
                {
                    res = text;
                }
            }

            return res;
        }

       

        private string GetTranslateBlock()
        {
            string res = "<select onchange=\"Translate(this.options[this.selectedIndex].value);\">";
            foreach (var item in Languages)
            {
                bool selected = (lang == null && item.Key == en) || (lang != null && lang == item.Key);
                res += NewLine + "<option value=\"" + item.Key + "\"" + (selected ? " selected" : "") + ">" + item.Value + "</option>";
            }
            res += NewLine + "</select>";

            return res;
        }

        #region JavaScript
        private string GetPrepopulateOption(Control c, ControlPropertyManager cpm)
        {
            string res = string.Empty;
            ControlProperty p = cpm.GetPropertyByNameAndControl(QueryString_Property, c);
            string queryParam = null;
            if (p != null)
            {
                queryParam = c.GetFormPropertyValue(p);
            }
            if (queryParam != null)
            {
                res = '"' + c.HTMLID.ToString() + "\":\"" + queryParam + '"';
            }

            return res;
        }

        private string[] GetFieldRules(Control c)
        {
            IEnumerable<Rule> rules = c.Rules.Where(x => x.Type == (int)RuleTypes.Field);
            string[] res = new string[rules.Count()];
            int i = 0;
            foreach (var r in rules)
            {
                res[i] = '[' + r.ConditionGroup.ToJson() + ",\"" + c.HTMLID + "\",\"" + r.Action + "\"]";
                i++;
            }

            return res;
        }

        private string[] GetPageRules(Form form)
        {
            IEnumerable<Rule> rules = form.Rules.Where(x => x.Type == (int)RuleTypes.Page);
            string[] res = new string[rules.Count()];
            int i = 0;
            foreach (var r in rules)
            {
                res[i] = '[' + r.ConditionGroup.ToJson() + ",\"" + Guid.Empty + "\",[]]";
                i++;
            }

            return res;
        }

        private string[] GetPageRules(Control c, List<string> previousIDs)
        {
            IEnumerable<Rule> rules = c.Rules.Where(x => x.Type == (int)RuleTypes.Page);
            string[] res = new string[rules.Count()];
            int i = 0;
            foreach (var r in rules)
            {
                res[i] = '[' + r.ConditionGroup.ToJson() + ",\"" + c.HTMLID + "\",[" + string.Join(",", previousIDs) + "]]";
                i++;
            }

            return res;
        }

        private string GetPageRule(Rule r)
        {
            return '[' + r.ConditionGroup.ToJson() + ",\"" + (r.Control_ID.HasValue ? r.Control.HTMLID : Guid.Empty) + "\",[]]";
        }

        private string[] GetFormRules(Form form)
        {
            IEnumerable<Rule> rules = form.Rules.Where(x => x.Type == (int)RuleTypes.Form);
            string[] res = new string[rules.Count()];
            int i = 0;
            foreach (var r in rules)
            {
                string target = null;
                string type = null;
                if (r.UrlToRedirect == null)
                {
                    type = message;
                    target = HttpUtility.JavaScriptStringEncode(r.Action);
                }
                else
                {
                    type = url;
                    target = HttpUtility.UrlEncode(r.UrlToRedirect);
                }
                res[i] = '[' + r.ConditionGroup.ToJson() + ",\"" + type + "\",\"" + target + "\"]";
                i++;
            }

            return res;
        }

        private string GetButtonNames(Control pagebreak)
        {
            string prev = pagebreak.GetFormPropertyValue(PreviousButton_Property) ?? PrevButtonDefaultText;
            string next = pagebreak.GetFormPropertyValue(NextButton_Property) ?? NextButtonDefaultText;

            return '\"' + pagebreak.HTMLID.ToString() + "\":[\"" + HttpUtility.JavaScriptStringEncode(Translate(prev)) +
                                                        "\",\"" + HttpUtility.JavaScriptStringEncode(Translate(next)) + "\"]";
        }

        private string GetComparisonTypesJson()
        {
            string res = string.Empty;
            foreach (var item in Enum.GetValues(typeof(ComparisonType)))
            {
                ComparisonType type = (ComparisonType)item;
                res += (int)type + ":\"" + type.ToString() + "\",";
            }

            return '{' + res.Substring(0, res.Length - 1) + '}';
        }
        #endregion

        #region HTML elements methods
        private string GetFullHTML(KMEnums.ControlType? type, string element, string label)
        {
            return GetFullHTML(type, element, label, null);
        }

        private string GetFullHTML(KMEnums.ControlType? type, string element, string label, string cssClass)
        {
            return InsertLabel(type, WrapDiv(type, element, cssClass), label);
        }

        private string InsertLabel(KMEnums.ControlType? type, string element, string label)
        {
            return element.Insert(element.IndexOf('>') + 1, NewLine + GetLabel(label));
        }

        private string WrapDiv(KMEnums.ControlType? type, string element, string cssClass)
        {
            if (cssClass == null)
            {
                return element;
            }
            return
                "<div " +
                ((type != null && type == KMEnums.ControlType.Email) ? " style=\"display: inline\"" : "") +
                "class=\"km" + cssClass + "\">" + NewLine + element + NewLine + CloseDiv;
        }

        private string GetTextarea(Control c, string cssClass)
        {
            string res = "<textarea></textarea>";
            res = AddIDAndNameAttributes(res, c.HTMLID);
            string value = c.GetFormPropertyValue(FieldSize_Property);
            if (value != null)
            {
                res = AddAttribute(res, Rows_Property.ToLower(), value);
            }
            res = WrapDiv(null, res, cssClass);

            return res;
        }

        private string GetInputWithType(Control c, KMEnums.ControlType type, string translation, string cssClass, ControlPropertyManager cpm)
        {
            string res = string.Empty;
            ControlProperty property = cpm.GetValuePropertyByControl(c);
            string input_type = type.ToString().ToLower();
            if (type == KMEnums.ControlType.RadioButton)
            {
                input_type = radio;
            }
            int i = 0;
            int columnsCount = 0;
            try
            {
                columnsCount = int.Parse(c.GetFormPropertyValue(NumberofColumns_Property));
            }
            catch
            { }
            if (columnsCount < 1)
            {
                columnsCount = 1;
            }
            string div_ItemWrapper = "<div class=\"km_item\">";
            var items = c.GetProperties(property).OrderBy(g => g.Order);
            //if (columnsCount < items.Count())
            div_ItemWrapper = AddAttribute(div_ItemWrapper, StyleAttribute, "width:" + ((100 / columnsCount) - 1) + '%');
            string hintPart = AddNameAttribute("<input type=\"" + input_type + "\" />", c.HTMLID);
            res += "<div class=\"km_hint\">" + hintPart + CloseDiv;
            
            if (c.ControlCategories != null && c.ControlCategories.Count() > 0)
            {
                string lastItem = string.Empty;
                bool closeLastCategoriesDiv = false;
                foreach (var p in items)
                {
                    if (lastItem != p.CategoryName)
                    {
                        closeLastCategoriesDiv = true;
                        if (!string.IsNullOrEmpty(lastItem))
                            res += CloseDiv + NewLine;
                        if (!string.IsNullOrEmpty(p.CategoryName) && p.CategoryID.HasValue && p.CategoryID != 0 && p.CategoryName != " -- Select -- ")
                            res += div_ItemWrapper + NewLine + "<span class=\"category\">" + p.CategoryName + "</span>" + NewLine;
                        else
                            res += div_ItemWrapper + NewLine;
                    }
                    lastItem = p.CategoryName;
                    string text = Translate(p.Text());
                    string item = "<input type=\"" + input_type + "\" style=\"width: 13px;height: 13px;padding: 0;margin: 0;\" />";
                    string textHTML = OpenSpan + text + CloseSpan;
                    string id = c.HTMLID.ToString() + '_' + i;
                    item = AddIDAttribute(item, id);
                    item = AddNameAttribute(item, c.HTMLID);
                    item = AddValueAttribute(item, p.DataValue);
                    if (p.IsDefault.HasValue && p.IsDefault.Value)
                    {
                        item = AddAttribute(item, Checked, Checked);
                    }
                    textHTML = AddAttribute(textHTML, OnClickAttribute, GetOnClickFunction(id));
                    res += AddAttribute(OpenDiv, StyleAttribute, "min-width: 100px;") + NewLine + item + textHTML + CloseDiv + NewLine;
                    i++;
                }
                if (closeLastCategoriesDiv)
                    res += CloseDiv + NewLine;
                res = "<label class=\"cleanLabel\">" + translation + "</label>" + NewLine + "<div style=\"width: 99.3%;\">" + NewLine + res + NewLine + CloseDiv;
            }
            else
            {
                foreach (var p in items)
                {
                    string text = Translate(p.Text());
                    string item = "<input type=\"" + input_type + "\" style=\"width: 13px;height: 13px;padding: 0;margin: 0;\" />";
                    string textHTML = OpenSpan + text + CloseSpan;
                    string id = c.HTMLID.ToString() + '_' + i;
                    item = AddIDAttribute(item, id);
                    item = AddNameAttribute(item, c.HTMLID);
                    item = AddValueAttribute(item, p.DataValue);
                    if (p.IsDefault.HasValue && p.IsDefault.Value)
                    {
                        item = AddAttribute(item, Checked, Checked);
                    }
                    textHTML = AddAttribute(textHTML, OnClickAttribute, GetOnClickFunction(id));
                    res += div_ItemWrapper + NewLine + item + textHTML + CloseDiv + NewLine;
                    i++;
                }
                res = "<label class=\"cleanLabel\">" + translation + "</label>" + NewLine + "<div style=\"width: 99.3%;\">" + NewLine + res + NewLine + CloseDiv;
            }
            res = WrapDiv(null, res, cssClass);

            return res;
        }

        private string GetInputWithType(string id, string name, string type, string cssClass)
        {
            string item = "<input type=\"" + type + "\" />";
            item = AddIDAttribute(item, id);
            item = AddNameAttribute(item, name);

            return WrapDiv(null, item, cssClass);
        }

        private string GetSelect(Control c, bool isListbox, ControlPropertyManager cpm)
        {
            string res = NewLine + "<select>";
            res = AddIDAndNameAttributes(res, c.HTMLID);
            ControlProperty property = cpm.GetValuePropertyByControl(c);
            List<FormControlPropertyGrid> properties = c.GetProperties(property).ToList();
            if (isListbox)
            {
                int rows = properties.Count;
                string value = c.GetFormPropertyValue(FieldSize_Property);
                if (value != null)
                {
                    rows = int.Parse(value);
                }
                res = AddAttribute(res, SizeAttribute, rows.ToString());
                res = AddAttribute(res, Multiple, Multiple);
            }
            else
            {
                //add 1st default option for standard
                //if (!c.IsCustom())
                //{
                //for all dropboxes
                res += NewLine + "<option>" + Translate(StandardSelector_DefaultValue) + "</option>";
                //}
            }

            int i = 0;
            string lastItem = string.Empty;
            foreach (var p in properties)
            {
                string item = string.Empty;
                string cat = string.Empty;
                if (lastItem != p.CategoryName && p.CategoryID.HasValue && p.CategoryID != 0)
                {
                    cat += NewLine + "<option class=\"category\" disabled>-- " + p.CategoryName + " --</option>";
                }
                lastItem = p.CategoryName;
                item = NewLine + "<option>" + Translate(p.Text()) + "</option>";
                string id = c.HTMLID.ToString() + '_' + i;
                item = AddIDAttribute(item, id);
                item = AddValueAttribute(item, p.DataValue);
                if (p.IsDefault.HasValue && p.IsDefault.Value)
                {
                    item = AddAttribute(item, Selected, Selected);
                }
                res += cat + item;
                i++;
            }

            res += NewLine + "</select>";

            return res;
        }

        private string GetGrid(Control c)
        {
            List<FormControlPropertyGrid> columnsProperties = c.GetProperties(Columns_Property).OrderBy(x => x.FormControlPropertyGrid_Seq_ID).ToList();
            List<FormControlPropertyGrid> rowsProperties = c.GetProperties(Rows_Property).OrderBy(x => x.FormControlPropertyGrid_Seq_ID).ToList();
            string res = string.Empty;
            GridControl gridItems = (GridControl)int.Parse(c.GetFormPropertyValue(GridControls_Property));
            string itemType = (gridItems == GridControl.RadioButtons ?
                                KMEnums.ControlType.RadioButton : KMEnums.ControlType.CheckBox).ToString().ToLower();
            string cssClass = itemType.Substring(0, 1).ToUpper() + itemType.Substring(1).ToLower();
            if (itemType == KMEnums.ControlType.RadioButton.ToString().ToLower())
            {
                itemType = radio;
            }

            if (columnsProperties.Count > 0 && rowsProperties.Count > 0)
            {
                res = NewLine + "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">";
                string id = c.HTMLID.ToString();
                string itemsName = id + "_cell";
                GridValidation validation = (GridValidation)int.Parse(c.GetFormPropertyValue(GridValidation_Property));
                res = AddIDAttribute(res, c.HTMLID);
                for (int i = -1; i < rowsProperties.Count; i++)
                {
                    res += NewLine + "<tr>";
                    for (int j = -1; j < columnsProperties.Count; j++)
                    {
                        if (i == -1)
                        {
                            if (j == -1)
                            {
                                res += NewLine + "<th></th>";
                            }
                            else
                            {
                                res += NewLine + "<th>" + Translate(columnsProperties[j].Text()) + "</th>";
                            }
                        }
                        else
                        {
                            if (validation == GridValidation.AtLeastOnePerLine || validation == GridValidation.AtLeastOne)
                            {
                                itemsName = id + '_' + i;
                            }
                            if (j == -1)
                            {
                                res += NewLine + "<td>" + Translate(rowsProperties[i].Text()) + "</td>";
                            }
                            else
                            {
                                string inputId = c.HTMLID.ToString() + '_' + i + '_' + j;
                                res += NewLine + "<td>" + GetInputWithType(inputId, itemsName, itemType, cssClass) + "</td>";
                            }
                        }
                    }
                    res += NewLine + "</tr>";
                }
                res += NewLine + "</table>";

                //div for error
                res += NewLine + DivGridError;

                //hidden part for values
                string hidden_part = HiddenHTML;
                hidden_part = AddIDAttribute(hidden_part, c.HTMLID.ToString() + '_' + ValueAttribute);
                hidden_part = AddNameAttribute(hidden_part, c.HTMLID);
                res += NewLine + hidden_part;
            }

            return res;
        }

        private string GetNewsletter(Control c)
        {
            bool def = int.Parse(c.GetFormPropertyValue(Letter_Type_Property)) == 1;
            string res = NewLine;
            string nlGroupCheckbox = string.Empty;
            string nlGroupLabel = string.Empty;
            string subscribeID = string.Empty;
            int columnsCount = 0;
            try
            {
                columnsCount = int.Parse(c.GetFormPropertyValue(NumberofColumns_Property));
            }
            catch
            { }
            if (columnsCount < 1)
            {
                columnsCount = 1;
            }
            string div_ItemWrapper = "<div class=\"km_item\">";
            //if (columnsCount < c.NewsletterGroups.Count())
            div_ItemWrapper = AddAttribute(div_ItemWrapper, StyleAttribute, "width:" + ((100 / columnsCount) - 1) + '%');
            if (c.ControlCategories != null && c.ControlCategories.Count() > 0)
            {
                string lastCategory = string.Empty;
                foreach (var group in c.NewsletterGroups.OrderBy(g => g.Order))
                {
                    if (group.ControlCategory == null)
                        group.ControlCategory = new ControlCategory();
                    if (lastCategory != group.ControlCategory.LabelHTML)
                    {
                        if (!string.IsNullOrEmpty(lastCategory))
                            res += CloseDiv + NewLine;
                        if (!string.IsNullOrEmpty(group.ControlCategory.LabelHTML))
                            res += div_ItemWrapper + NewLine + "<span class=\"category\">" + group.ControlCategory.LabelHTML + "</span>" + NewLine;
                        else
                            res += div_ItemWrapper + NewLine;
                    }
                    lastCategory = group.ControlCategory.LabelHTML;
                    nlGroupCheckbox = "<input type=\"checkbox\" />";
                    nlGroupLabel = OpenSpan + Translate(group.LabelHTML) + CloseSpan;
                    subscribeID = c.HTMLID.ToString() + '_' + group.GroupID + '_' + Subscribe.ToLower();
                    nlGroupCheckbox = AddIDAttribute(nlGroupCheckbox, subscribeID);
                    nlGroupCheckbox = AddNameAttribute(nlGroupCheckbox, c.HTMLID);
                    nlGroupCheckbox = AddValueAttribute(nlGroupCheckbox, group.GroupID.ToString());
                    if (group.IsPreSelected)
                    {
                        nlGroupCheckbox = AddAttribute(nlGroupCheckbox, Checked, Checked);
                    }
                    nlGroupLabel = AddAttribute(nlGroupLabel, OnClickAttribute, GetOnClickFunction(subscribeID));
                    res += OpenDiv + NewLine + nlGroupCheckbox + nlGroupLabel + CloseDiv + NewLine;
                }
                res = "<div style=\"width: 99.3%;\">" + NewLine + res + NewLine + CloseDiv + NewLine + CloseDiv;
            }
            else
            {
                foreach (var group in c.NewsletterGroups.OrderBy(g => g.Order))
                {
                    nlGroupCheckbox = "<input type=\"checkbox\" />";
                    nlGroupLabel = OpenSpan + Translate(group.LabelHTML) + CloseSpan;
                    subscribeID = c.HTMLID.ToString() + '_' + group.GroupID + '_' + Subscribe.ToLower();
                    nlGroupCheckbox = AddIDAttribute(nlGroupCheckbox, subscribeID);
                    nlGroupCheckbox = AddNameAttribute(nlGroupCheckbox, c.HTMLID);
                    nlGroupCheckbox = AddValueAttribute(nlGroupCheckbox, group.GroupID.ToString());
                    if (group.IsPreSelected)
                    {
                        nlGroupCheckbox = AddAttribute(nlGroupCheckbox, Checked, Checked);
                    }
                    nlGroupLabel = AddAttribute(nlGroupLabel, OnClickAttribute, GetOnClickFunction(subscribeID));
                    res += div_ItemWrapper + NewLine + nlGroupCheckbox + nlGroupLabel + CloseDiv + NewLine;
                }
                res = "<div style=\"width: 99.3%;\">" + NewLine + res + NewLine + CloseDiv;
            }

            return res;
        }

        private string GetCaptcha(Guid id)
        {
            return AddIDAttribute(RecaptchaHTML, ("g-recaptcha-" + id.ToString()).Replace("-", "")) + RecaptchaValidateHTML;
        }
        #endregion

        #region other methods
        private string AddIDAttribute(string html, Guid id)
        {
            return AddAttribute(html, IDAttribute, id.ToString());
        }

        private string AddIDAttribute(string html, string id)
        {
            return AddAttribute(html, IDAttribute, id);
        }

        private string AddNameAttribute(string html, Guid id)
        {
            return AddAttribute(html, NameAttribute, id.ToString());
        }

        private string AddNameAttribute(string html, string name)
        {
            return AddAttribute(html, NameAttribute, name);
        }

        private string AddIDAndNameAttributes(string html, Guid id)
        {
            return AddIDAndNameAttributes(html, id.ToString());
        }

        private string AddIDAndNameAttributes(string html, string id)
        {
            html = AddIDAttribute(html, id);

            return AddNameAttribute(html, id);
        }

        private string AddValueAttribute(string html, string value)
        {
            return AddValueAttribute(html, value, true);
        }

        private string AddValueAttribute(string html, string value, bool isNeedModify)
        {
            return AddAttribute(html, ValueAttribute, isNeedModify ? OnlyLettersAndDigits(value) : HttpUtility.HtmlAttributeEncode(value));
        }

        private string AddAttribute(string htmlElement, string attribute, string value)
        {
            if (value != string.Empty)
            {
                //Regex rex = new Regex(StartHTMLElementPattern);
                //string element = rex.Match(htmlElement).Groups[0].Value;
                bool isNeedAddSpace = false;
                int start = htmlElement.IndexOf(" />");
                if (start == -1)
                {
                    isNeedAddSpace = true;
                    start = htmlElement.IndexOf("/>");
                }
                if (start == -1)
                {
                    isNeedAddSpace = false;
                    start = htmlElement.IndexOf('>');
                }
                htmlElement = htmlElement.Insert(start, ' ' + attribute + "=\"" + value + '"' + (isNeedAddSpace ? " " : string.Empty));
            }

            return htmlElement;
        }

        private string OnlyLettersAndDigits(Guid id)
        {
            return OnlyLettersAndDigits(id.ToString());
        }

        private string OnlyLettersAndDigits(string label)
        {
            return KMEntitiesWrapper.OnlyLettersAndDigits(label);
        }

        private string GetLabel(string label)
        {
            return GetLabel(label, true);
        }

        private string GetLabel(string label, bool isNeedNewLine)
        {
            string res = string.Empty;
            if (label != string.Empty)
            {
                res = "<label class=\"cleanLabel\">" + HttpUtility.HtmlDecode(label) + "</label>";
                if (isNeedNewLine)
                {
                    res += NewLine;
                }
            }

            return res;
        }

        private string GetOnClickFunction(string id)
        {
            return "ClickById('" + id + "');";
        }

        private bool ControlIsPageBreak(Control c)
        {
            return c.HTMLControlType() == KMEnums.ControlType.PageBreak;
        }

        private string GetZeros(int formsCount)
        {
            string res = string.Empty;
            for (int i = 0; i < formsCount; i++)
            {
                res += "0;";
            }

            return res.Substring(0, res.Length - 1);
        }

        #endregion

        private struct HtmlParams
        {
            public static List<string> ButtonNamesJson = new List<string>();
            public static List<string> PageBreakIds = new List<string>();
            public static List<string> ValidationRules = new List<string>();
            public static List<string> FieldRulesJson = new List<string>();
            public static ControlPropertyManager ControlPropertyManager = new ControlPropertyManager();
            public static DataTypePatternManager DataTypePatternManager = new DataTypePatternManager();
            public static string AllowChanges = string.Empty;
            public static string CountryControlId = string.Empty;
            public static string StateControlId = string.Empty;
            public static string PasswordControlId = string.Empty;
            public static string Body = string.Empty;
        }
    }
}