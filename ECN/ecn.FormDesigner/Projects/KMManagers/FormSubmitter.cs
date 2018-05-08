using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using KM.Common;
using ECN_Framework_Entities.Communicator;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMManagers.APITypes;
using static ECN_Framework_BusinessLayer.Communicator.EmailGroup;
using Rule = KMEntities.Rule;

namespace KMManagers
{
    public class FormSubmitter : APIRunnerBase
    {
        private const string SourceKey = "Source";
        private const string ConfirmationTemplatePathKey = "ConfirmationTemplatePath";
        private const string ConfirmationHandlerUrlKey = "ConfirmationHandlerUrl";
        private const string GCSecret = "GoogleCaptchaSecret";
        private const string DoubleOptInEmailTemplateKey = "DoubleOptInEmailTemplate";
        public const string SubscribeTypeCode = "SubscribeTypeCode";
        private const string ContentType = "application/x-www-form-urlencoded";
        private const string SiteVerify = "https://www.google.com/recaptcha/api/siteverify";
        private const string GroupIDMacros = "%groupId%";
        private const string CustomMacros = "%custom%";
        private const string StandartMacros = "%standart%";
        private const string SourceMacros = "%source%";
        private const string EmailMacros = "%emailTo%";
        private const string ReplyEmailMacros = "%replyEmailTo%";
        private const string FromEmailMacros = "%fromEmail%";
        private const string FromNameMacros = "%fromName%";
        private const string DefaultReplyEmail = "formdesigner@ecn5.com";
        private const string DefaultFromEmail = DefaultReplyEmail;
        private const string SubjectMacros = "%subject%";
        private const string HTMLMacros = "%html%";
        private const string LandingHTMLMacros = "%landing_html%";
        private const string SubscribeMacros = "%subscribe%";
        private const string FieldsJson = "{\"GroupID\":%groupId%,\"Format\":\"HTML\",\"SubscribeType\":\"%subscribe%\",\"Profiles\":[{%standart%,\"CustomFields\":[%custom%]}]}";
        private const string NotificationJson = "{\"Source\":\"%source%\",\"EmailAddress\":\"%emailTo%\",\"EmailSubject\":\"%subject%\",\"Content\":\"%html%\",\"ReplyEmailAddress\":\"%replyEmailTo%\",\"FromEmailAddress\":\"%fromEmail%\",\"FromName\":\"%fromName%\"}";
        private const string html_closetag = "</html>";
        private const string HTokenMacros = "%htoken%";
        private const string SnippetMacros = "%%";
        private const string SnippetRex = SnippetMacros + "<span cid=\"(\\d+)\">";
        private const string SnippetEnd = "</span>" + SnippetMacros;
        private const string urlMacros_prev = " href=";
        private const string FormAbandonCacheName = "FORM_ABANDON_";
        private const string FormSubmitCacheName = "FORM_SUBMIT_";
        private const char SignGreater = '>';
        private const string SignPercent = "%%";
        private const string PercentWrappedRegexTemplate = "%%(.+?)%%";
        private const string ColumnPassword = "Password";
        private const string LiteralEmpty = "empty";
        private const char DelimComma = ',';
        private const char DelimPipe = '|';

        private const string HtmlBr = "<br/>";
        private const string LabelHtmlTemplate = "{0}: ";
        private const string SubscribedTemplate = "{0}d";
        private const int TypeSeqGrid = 7;
        private const int TypeSeqIdLiteral = 11;
        private const string ControlStrGrid = "%%Grid%%";
        private const string ControlStrLiteral = "%%Literal%%";
        private const string FieldNamePassword = "Password";

        private const int TypeSeqId103 = 103;
        private const int TypeSeqIdCountry = 112;
        private const int TypeSeqIdState = 110;
        private const int TypeSeq103 = 103;
        private const int TypeSeq217 = 217;
        private const string SendValuesDone = "SendValues done; result is ";
        private const string SendValuesStarting = "SendValues starting";
        private const string CountryField = "Country";
        private const string StateField = "State";
        private const int Response500 = 500;
        private const int UnknownResponse = -1;
        private const string MasterAccessKey = "MasterAccessKey";
        private const string SubscribeTypeCodeS = "S";
        private const string SubscribeTypeCodeU = "U";
        private const char CommaSeparator = ',';
        private static readonly string GoogleCapthaSecret;
        private static readonly string ConfirmationUrl;
        private static readonly string DoubleOptInEmailTemplate;
        private static readonly string DoubleOptInEmailSubject;
        private static readonly string ConfirmationTemplate;
        private static readonly string Source;

        private static string[] subject_separators = new string[] { "<subject>", "</subject>" };

        private Form form;
        private Dictionary<int, string> values;
        private string ClientEmail = null;
        private Dictionary<string, int> fieldnames;
        private static Dictionary<int, DateTime> LastAccessTime = new Dictionary<int, DateTime>();
        private static Regex snippet = new Regex(SnippetRex);

        static FormSubmitter()
        {
            GoogleCapthaSecret = WebConfigurationManager.AppSettings[GCSecret];
            ConfirmationUrl = WebConfigurationManager.AppSettings[ConfirmationHandlerUrlKey];
            Source = WebConfigurationManager.AppSettings[SourceKey];

            //get templates
            StreamReader sr = new StreamReader
            (
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[DoubleOptInEmailTemplateKey]
            );
            DoubleOptInEmailTemplate = sr.ReadToEnd();
            sr.Close();
            DoubleOptInEmailSubject = string.Empty;
            int start = DoubleOptInEmailTemplate.ToLower().IndexOf(subject_separators[0]);
            int end = DoubleOptInEmailTemplate.ToLower().IndexOf(subject_separators[1]);
            if (end > start && start > -1)
            {
                DoubleOptInEmailSubject = DoubleOptInEmailTemplate.Substring(start + subject_separators[0].Length, end - start - subject_separators[0].Length);
                DoubleOptInEmailTemplate = DoubleOptInEmailTemplate.Substring(end + subject_separators[1].Length);
            }
            DoubleOptInEmailTemplate = DoubleOptInEmailTemplate.Replace(HTMLGenerator.NewLine, string.Empty);

            sr = new StreamReader
            (
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[ConfirmationTemplatePathKey]
            );
            ConfirmationTemplate = sr.ReadToEnd();
            sr.Close();
        }

        public string[] Submit(HttpRequest req)
        {
            string[] res = new string[4]; // 0 - response message, 1 - Form.Url, 2 - Form.Message, 3 - formRulesJson
            res[0] = "incorrect data";
            string responses = string.Empty;
            try
            {
                FormDbManager FM = new FormDbManager();
                if (HttpUtility.ParseQueryString(req.UrlReferrer.Query)["child"] != null && HttpUtility.ParseQueryString(req.UrlReferrer.Query)["child"].ToString().ToLower().Equals("true"))
                    form = FM.GetChildByTokenUID(req[HTMLGenerator.HiddenIDForToken]);
                else
                    form = FM.GetByTokenUID(req[HTMLGenerator.HiddenIDForToken]);

                values = new Dictionary<int, string>();
                int mustChecked = 0;
                bool ruleNotSubmit = true;
                responses = req[HTMLGenerator.HiddenIDForRecaptcha];
                if (form != null && form.IsActive() && (!RecaptchaOnForm(responses, ref mustChecked) || Recaptcha(responses, mustChecked)) && Validate(req, ref ruleNotSubmit)) //&& CheckLastTime() 
                {
                    res[0] = null;
                    Guid token = Guid.NewGuid();
                    if (SendConfirmationResponse(token))
                    {
                        SaveInDB(token);
                    }
                    else
                    {
                        int status = ProcessForm();
                        if (status != 200 && status != 201)
                        {
                            res[0] = "Your data hasn't been sent";
                        }
                    }
                }
                else
                {
                    if (ruleNotSubmit)
                    {
                        if (ClientEmail != null)
                        {
                            res[0] = ClientEmail;
                        }
                        if (responses.Split(';').Length == 1 && mustChecked > 0)
                        {
                            res[0] = "Please Reload your Form.";
                        }
                        string note = "FormToken: " + req[HTMLGenerator.HiddenIDForToken] + ", Email:" + ClientEmail ?? "" + ", Captcha:" + responses + ", FormHasCaptcha:" + mustChecked + ", User Agent: " + req.UserAgent.ToString();
                        KM.Common.Entity.ApplicationLog.LogNonCriticalError(new Exception("Error with Submit"), "KMManagers.FormSubmitter.Submit", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                    }
                    else
                    {
                        res[0] = null;
                    }
                }
                FormResult fr = form.FormResults.SingleOrDefault(x => x.ResultType == (int)(form.IsActive() ? FormResultType.ConfirmationPage : FormResultType.InactiveRedirect));
                if (fr != null)
                {
                    if (!string.IsNullOrEmpty(fr.JsMessage))
                    {
                        fr.Message = fr.JsMessage + "<html>" + fr.Message + "</html>";
                    }
                    res[1] = !String.IsNullOrEmpty(fr.URL) ? KMEntitiesWrapper.GetURL(ApplySnippets(form.Controls, fr.URL)) : string.Empty;
                    res[2] = !String.IsNullOrEmpty(fr.Message) ? ApplySnippets(form.Controls, fr.Message) : string.Empty;
                }

                List<string> formRulesJson = new List<string>();
                formRulesJson.AddRange(GetFormRules(form));
                res[3] = '[' + string.Join(",", formRulesJson) + ']';
            }
            catch (Exception ex)
            {
                res[0] = ex.Message;
                string note = "FormToken: " + req[HTMLGenerator.HiddenIDForToken] + ", Captcha:" + responses + ", User Agent: " + req.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "KMManagers.FormSubmitter.Submit", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
            }

            return res;
        }

        public Dictionary<string, string> AutoSubmit(HttpRequest req)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            try
            {
                FormDbManager FM = new FormDbManager();
                if (req.QueryString["child"] != null && req.QueryString["child"].ToString().ToLower().Equals("true"))
                    form = FM.GetChildByTokenUID(req["tokenuid"]);
                else
                    form = FM.GetByTokenUID(req["tokenuid"]);

                values = new Dictionary<int, string>();
                if (form.FormType == FormType.AutoSubmit.ToString())
                {
                    res = ValidateFromQueryString(req);
                    if (form != null && form.IsActive() && res["status"] == "200") //&& CheckLastTime()
                    {
                        FormStatisticLoader fsl = new FormStatisticLoader();
                        long FormStatistic_ID = Convert.ToInt64(fsl.CreateStatistic(req["tokenuid"], 1, ClientEmail));
                        Guid token = Guid.NewGuid();
                        if (SendConfirmationResponse(token))
                        {
                            SaveInDB(token);
                            fsl.SubmitStatistic(FormStatistic_ID, 1, ClientEmail);
                        }
                        else
                        {
                            int status = ProcessForm();
                            if (status == 200 || status == 201)
                            {
                                res["status"] = status.ToString();
                                res["message"] = "Form Submitted";
                                fsl.SubmitStatistic(FormStatistic_ID, 1, ClientEmail);
                            }
                            else
                            {
                                res["status"] = "400";
                                res["message"] = "Your data hasn't been sent. Status: " + res["status"];
                                SendSubmissionErrorNotifications("<b>Error Message:</b> " + res["message"] + "<br><b>Status:</b> " + res["status"] + "<br><b>QueryString:</b>: " + req.QueryString);
                            }
                        }
                        fsl.UnloadFormStatistic(FormStatistic_ID, 1);
                    }
                    else if (!form.IsActive())
                    {
                        res["status"] = "400";
                        res["message"] = "Autosubmit Form is Inactive ";
                        SendSubmissionErrorNotifications("<b>Error Message:</b> " + res["message"] + "<br><b>Status:</b> " + res["status"] + "<br><b>QueryString:</b>: " + req.QueryString);
                    }
                }
                else
                {
                    res["status"] = "400";
                    res["message"] = "Form is not AutoSubmit Type.";
                }
            }
            catch (Exception ex)
            {
                res["status"] = "500";
                res["message"] = ex.Message;
                SendSubmissionErrorNotifications("<b>Error Message:</b> " + ex.Message + "<br><b>QueryString:</b>: " + req.QueryString);
                string note = "FormToken: " + req[HTMLGenerator.HiddenIDForToken] + ", User Agent: " + req.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "KMManagers.FormSubmitter.AutoSubmit", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
            }

            return res;
        }

        public bool Confirm(Guid token, out string html)
        {
            html = "null";
            bool result = false;
            try
            {
                SubmitHistoryDbManager manager = new SubmitHistoryDbManager();
                SubmitHistory submitHistory = manager.GetByToken(token);
                if (submitHistory != null)
                {
                    //load values
                    values = new Dictionary<int, string>();
                    if (submitHistory.Form.Form_Seq_ID != submitHistory.Form_Seq_ID)
                        form = new FormDbManager().GetChildByTokenUID(form.TokenUID.ToString());
                    else
                        form = submitHistory.Form;
                    submitHistory.SubmitDatas = GetSubmitData(manager);
                    foreach (var n in submitHistory.SubmitDatas)
                    {
                        values.Add(n.Control_ID, n.Value);
                    }

                    //prepare email field
                    Control email = form.Controls.Single(x => x.Type_Seq_ID == (int)KMEnums.ControlType.Email);
                    ClientEmail = values[email.Control_ID];

                    //process
                    if (ProcessForm() == (int)HttpStatusCode.OK)
                    {
                        //delete after confirm
                        result = true;
                        manager.Delete(submitHistory);
                        manager.SaveChanges();
                    }

                    //load template
                    html = GetLandingHTML(token);
                }
            }
            catch (Exception ex)
            {
                MethodBase mb = ex.TargetSite;
                Logger.Log(mb.DeclaringType.FullName + '.' + mb.Name, ex);
                html = ex.Message;
            }

            return result;
        }

        private int ProcessForm()
        {
            Logger.WriteLog("starting process form");

            int status = SendValues();
            if (status == 200 || status == 201)
            {
                SendNotifications();
                Logger.WriteLog("SendNotifications - done");
                Process3rdPartyOutput();
            }

            return status;
        }


        #region Validation
        private bool CheckLastTime()
        {
            int formId = form.Form_Seq_ID;
            bool allow = true;
            if (HTMLGenerator.IntervalSec > -1)
            {
                lock (typeof(FormSubmitter))
                {
                    DateTime now = DateTime.Now;
                    if (LastAccessTime.ContainsKey(formId))
                    {
                        DateTime prev = LastAccessTime[formId];
                        allow = now.Subtract(prev).TotalSeconds >= HTMLGenerator.IntervalSec;
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

        private bool RecaptchaOnForm(string responses, ref int mustChecked)
        {
            bool result = !string.IsNullOrEmpty(responses);
            if (result)
            {
                result = false;
                string submitId = responses.Split(';')[0].ToLower();
                int start = -1;
                int end = 0;
                foreach (var pb in form.Controls.Where(x => x.Type_Seq_ID == (int)KMEnums.ControlType.PageBreak).OrderBy(x => x.Order))
                {
                    if (pb.HTMLID.ToString().ToLower() == submitId)
                    {
                        end = pb.Order;
                        break;
                    }
                    else
                    {
                        start = pb.Order;
                    }
                }
                if (end > 0)
                {
                    mustChecked = form.Controls.Count(x => x.Type_Seq_ID == (int)KMEnums.ControlType.Captcha && x.Order > start && x.Order < end);
                    result = mustChecked > 0;
                }
            }

            return result;
        }

        private bool Recaptcha(string responses, int mustChecked)
        {
            int checks = 0;
            bool result = !string.IsNullOrEmpty(responses);
            if (result)
            {
                string[] data = responses.Split(';');
                for (int i = 1; i < data.Length; i++)
                {
                    bool res = false;
                    if (data[i] != string.Empty)
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SiteVerify);
                        req.Timeout = 30000;
                        req.Method = PostVerb;
                        req.ContentType = ContentType;
                        string formdata = String.Format(
                                            "secret={0}&response={1}",
                                            HttpUtility.UrlEncode(GoogleCapthaSecret),
                                            HttpUtility.UrlEncode(data[i]));
                        byte[] formbytes = Encoding.ASCII.GetBytes(formdata);
                        using (Stream stream = req.GetRequestStream())
                        {
                            stream.Write(formbytes, 0, formbytes.Length);
                        }

                        string responseData = null;
                        try
                        {
                            using (WebResponse resp = req.GetResponse())
                            {
                                if (resp != null)
                                {
                                    StreamReader reader = new StreamReader(resp.GetResponseStream());
                                    responseData = reader.ReadToEnd();
                                    checks++;
                                    reader.Close();
                                }
                                resp.Close();
                            }
                        }
                        catch
                        { }
                        req.Abort();
                        try
                        {
                            res = serializer.Deserialize<RecaptchaJsonResponse>(responseData).success;
                        }
                        catch
                        { }
                    }
                    result &= res;
                    if (!result)
                    {
                        break;
                    }
                }
            }

            return result && checks == mustChecked;
        }

        private bool Validate(HttpRequest req, ref bool ruleNotSubmit)
        {
            bool res = true;
            ControlPropertyManager cpm = new ControlPropertyManager();
            List<Control> controls = form.Controls.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].HTMLControlType() != KMEnums.ControlType.PageBreak && controls[i].HTMLControlType() != KMEnums.ControlType.Literal && req[controls[i].HTMLID.ToString()] != null)
                {
                    string value = null;
                    if (cpm.Validate(controls[i], req[controls[i].HTMLID.ToString()], out value))
                    {
                        if (controls[i].Type_Seq_ID == (int)KMEnums.ControlType.Email)
                        {
                            ClientEmail = value;
                        }
                        values.Add(controls[i].Control_ID, value);
                    }
                }
            }
            res &= ClientEmail != null;
            if (res)
            {
                res = CheckEmail(ref ClientEmail);
            }

            List<Rule> rule = form.Rules.Where(x => x.Type == (int)RuleTypes.Form).OrderBy(x => x.Order).ToList();

            // List<Rule> rule = form.Rules.ToList();
            List<int> groupsDone = new List<int>();
            if (rule != null)
            {
                foreach (Rule r in rule)
                {
                    if (!groupsDone.Contains(r.ConditionGroup_Seq_ID))
                    {
                        groupsDone.Add(r.ConditionGroup_Seq_ID);
                        List<Rule> rulesinGroup = rule.Where(x => x.ConditionGroup_Seq_ID == r.ConditionGroup_Seq_ID).ToList();
                        Rule ruleToCommit = null;
                        foreach (Rule ru in rulesinGroup)
                        {
                            if (CheckGroup(ru.ConditionGroup))
                            {
                                ruleToCommit = ru;
                                break;
                            }
                        }
                        if (ruleToCommit != null)
                        {
                            //do whatever rule says
                            if (ruleToCommit.SuspendpostDB.HasValue && ruleToCommit.SuspendpostDB.Value == 1)
                            {
                                res = false;
                                ruleNotSubmit = false;
                            }
                            if (ruleToCommit.Overwritedatapost.HasValue && ruleToCommit.Overwritedatapost.Value == 1)
                            {
                                //modify req object with overwrited values
                                foreach (OverwritedataPostValue o in ruleToCommit.OverwritedataPostValues)
                                {
                                    //if (!o.IsDeleted)
                                        values[o.Control_ID] = o.Value;
                                }
                            }
                        }
                    }
                }
            }


            return res;
        }

        private Dictionary<string, string> ValidateFromQueryString(HttpRequest req)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            ControlPropertyManager cpm = new ControlPropertyManager();
            List<Control> controls = form.Controls.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].HTMLControlType() != KMEnums.ControlType.PageBreak && controls[i].HTMLControlType() != KMEnums.ControlType.Literal)
                {
                    string value = null;
                    string querystring = string.Empty;
                    foreach (var ctrlProps in controls[i].FormControlProperties)
                        if (ctrlProps.ControlProperty.PropertyType == 1 && ctrlProps.ControlProperty.PropertyName == "Querystring Parameter")
                            querystring = ctrlProps.Value;
                    if (!string.IsNullOrEmpty(querystring))
                    {
                        if (cpm.Validate(controls[i], req[querystring], out value))
                        {
                            if (controls[i].Type_Seq_ID == (int)KMEnums.ControlType.Email)
                            {
                                ClientEmail = value;
                            }
                            values.Add(controls[i].Control_ID, value);
                        }
                    }
                }
            }

            if (ClientEmail != null)
            {
                if (CheckEmail(ref ClientEmail))
                {
                    res["status"] = "200";
                    res["message"] = "Valid Email and querystring data loaded.";
                }
                else
                {
                    res["status"] = "400";
                    res["message"] = ClientEmail;
                    SendSubmissionErrorNotifications("<b>Error Message:</b> " + res["message"] + "<br><b>QueryString:</b> " + req.QueryString);
                }
            }
            else
            {
                res["status"] = "400";
                res["message"] = "Form could not be Submitted. Email value could not be found or is invalid.";
                SendSubmissionErrorNotifications("<b>Error Message:</b> " + res["message"] + "<br><b>QueryString:</b>: " + req.QueryString);
            }

            return res;
        }
        #endregion

        #region Process
        private void SaveInDB(Guid token)
        {
            SubmitHistoryDbManager shm = new SubmitHistoryDbManager();
            SubmitHistory history = new SubmitHistory();
            history.Form_Seq_ID = form.Form_Seq_ID;
            history.Added = DateTime.Now;
            history.HistoryToken = token;
            history.SubmitDatas = GetSubmitData(shm);
            shm.Add(history);
            shm.SaveChanges();
        }

        private int SendValues()
        {
            Logger.WriteLog(SendValuesStarting);
            var formManager = new FormManager();
            var fields = formManager.GetFieldsByForm(form).ToList();
            var custom = new Dictionary<string, string>();
            var standard = new Dictionary<string, string>();
            var subscribeParameters = new Dictionary<ApiData, bool>();
            var formApiData = new ApiData(form);
            subscribeParameters.Add(formApiData, true);
            FillDictionaries(fields, custom, standard);

            // process newsletters
            ProcessNewsLetters(formApiData, subscribeParameters);

            var result = UnknownResponse;
            foreach (var parameter in subscribeParameters)
            {
                var tempStandard = standard;

                string managerSubsciberResponse;
                var response = ManageSubscriberWithProfile(parameter.Key, custom, tempStandard, parameter.Value, out managerSubsciberResponse);
                if (parameter.Key.Equals(formApiData))
                {
                    result = response;
                    if (result == Response500)
                    {
                        return result;
                    }
                }
            }

            Logger.WriteLog(SendValuesDone + result);

            return result;
        }

        private void ProcessNewsLetters(ApiData formApiData, IDictionary<ApiData, bool> subscribeParameters)
        {
            var newsLetters = form.Controls.Where(x => x.Type_Seq_ID == (int)KMEnums.ControlType.NewsLetter)
                .OrderBy(x => x.Order);
            foreach (var newsLetter in newsLetters)
            {
                foreach (var newsletterGroup in newsLetter.NewsletterGroups)
                {
                    bool isSubscribe;
                    if (values.ContainsKey(newsletterGroup.Control_ID))
                    {
                        var groupsSubs = values[newsletterGroup.Control_ID].Split(CommaSeparator);
                        isSubscribe = groupsSubs.Contains(newsletterGroup.GroupID.ToString());
                    }
                    else
                    {
                        continue;
                    }

                    var groupId = newsletterGroup.GroupID.ToString();
                    string customerId = null;
                    string accessKey = null;
                    ApiData newsletterApiData = null;
                    if (!string.IsNullOrEmpty(groupId))
                    {
                        customerId = newsletterGroup.CustomerID.ToString();
                        accessKey = ConfigurationManager.AppSettings[MasterAccessKey];
                        newsletterApiData = new ApiData(customerId, accessKey, groupId, true);
                    }

                    //Check if email exists in group
                    if (!Exists(ClientEmail, Convert.ToInt32(groupId), Convert.ToInt32(customerId)))
                    {
                        // If it doesn't then check if they are subscribing
                        if (!isSubscribe)
                        {
                            // if they aren't subscribing then don't make the call to UNSUBSCRIBE them
                            newsletterApiData = null;
                        }
                    }

                    newsletterApiData = GetNewsletterApiData(newsLetter, newsletterApiData, accessKey, customerId, groupId);

                    if (newsletterApiData != null && !newsletterApiData.Equals(formApiData))
                    {
                        if (subscribeParameters.ContainsKey(newsletterApiData))
                        {
                            subscribeParameters[newsletterApiData] = isSubscribe;
                        }
                        else
                        {
                            subscribeParameters.Add(newsletterApiData, isSubscribe);
                        }
                    }
                }
            }
        }

        private ApiData GetNewsletterApiData(
            Control newsLetter,
            ApiData newsletterApiData,
            string accessKey,
            string customerId,
            string groupId)
        {
            var formPropertyValue = newsLetter.GetFormPropertyValue(HTMLGenerator.PrepopulateFrom_Property);
            if (formPropertyValue != null)
            {
                PopulationType populationType;
                if (!Enum.TryParse(formPropertyValue, out populationType))
                {
                    populationType = PopulationType.None;
                }

                if (populationType == PopulationType.Database)
                {
                    newsletterApiData = GetNewsletterApiDataByCheckNewsLetter(accessKey, customerId, groupId, newsletterApiData);
                }
            }

            return newsletterApiData;
        }

        private ApiData GetNewsletterApiDataByCheckNewsLetter(string accessKey, string customerId, string groupId, ApiData newsletterApiData)
        {
            var checkEmailByNewsLetter = CheckEmailByNewsLetter(
                accessKey,
                Convert.ToInt32(customerId),
                Convert.ToInt32(groupId),
                ClientEmail);

            var checkEmailDictionary = (Dictionary<string, object>)serializer.DeserializeObject(checkEmailByNewsLetter);
            if (checkEmailDictionary != null && checkEmailDictionary.ContainsKey(SubscribeTypeCode))
            {
                var subscribeTypeCode = checkEmailDictionary[SubscribeTypeCode].ToString();

                if (!string.IsNullOrWhiteSpace(subscribeTypeCode)
                    && subscribeTypeCode != SubscribeTypeCodeS
                    && subscribeTypeCode != SubscribeTypeCodeU)
                {
                    return null;
                }
            }

            return newsletterApiData;
        }

        private void FillDictionaries(List<GroupDataFields> fields, IDictionary<string, string> custom, IDictionary<string, string> standard)
        {
            foreach (var item in values)
            {
                var control = form.Controls.Single(x => x.Control_ID == item.Key);
                var fieldName = GetFieldNameFromControl(control, fields);
                if (control.IsCustom())
                {
                    if (fieldName == null)
                    {
                        continue;
                    }

                    if (custom.ContainsKey(fieldName))
                    {
                        var builder = new StringBuilder(custom[fieldName]);
                        builder.AppendFormat(",{0}", item.Value);
                        custom[fieldName] = builder.ToString();
                    }
                    else
                    {
                        custom.Add(fieldName, item.Value);
                    }
                }
                else
                {
                    standard.Add(fieldName, item.Value);
                }
            }

            var controlManager = new ControlManager();
            ChangeValueForKey(standard, CountryField, controlManager.GetCountryName);
            ChangeValueForKey(standard, StateField, controlManager.GetStateCode);
        }

        private static void ChangeValueForKey(
            IDictionary<string, string> controls,
            string key,
            Func<int, string> changeFunction)
        {
            if (controls.ContainsKey(key))
            {
                int value;
                if (int.TryParse(controls[key], out value))
                {
                    controls[key] = changeFunction(value);
                }
            }
        }

        private int ManageSubscriberWithProfile(ApiData apiData, Dictionary<string, string> custom, Dictionary<string, string> tempStandard, bool isSubscribe, out string resp)
        {
            try
            {
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(apiData.AccessKey, false);


                string profile = BuildProfileXML(tempStandard, isSubscribe);
                string udfs = "";
                List<ECN_Framework_Entities.Communicator.GroupDataFields> udfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(Convert.ToInt32(apiData.GroupID));
                Dictionary<string, string> customFields = new Dictionary<string, string>();
                foreach (var udf in udfList)
                {
                    if (custom.ContainsKey(udf.ShortName))
                    {
                        customFields.Add(udf.ShortName, custom[udf.ShortName]);
                    }
                }
                if (customFields.Count > 0)
                    udfs = BuildUDFXML(customFields, tempStandard["EmailAddress"], Convert.ToInt32(apiData.GroupID), udfList);
                else
                    udfs = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>";

                string source = "";
                if (apiData.IsNewsletter)
                    source = "FormDesigner.FormSubmitter.SendValues.Newsletter";
                else
                    source = "FormDesigner.FormSubmitter.SendValues.Form";

                ImportEmails_NoAccessCheck(user, Convert.ToInt32(apiData.CustomerID), Convert.ToInt32(apiData.GroupID), profile, udfs, "HTML", isSubscribe ? "S" : "U", false, "", source);



                // Check for any triggers                
                List <ECN_Framework_Entities.Communicator.LayoutPlans> lop = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByGroupID_NoAccessCheck(Convert.ToInt32(apiData.GroupID), Convert.ToInt32(apiData.CustomerID));
                ECN_Framework_Entities.Communicator.EmailGroup emailGroup = GetByEmailAddressGroupID_NoAccessCheck(tempStandard["EmailAddress"], Convert.ToInt32(apiData.GroupID));
                if (!apiData.IsNewsletter)
                {
                    int FormAbandonCache = -1;
                    int FormSubmitCache = -1;
                    try
                    {
                        FormAbandonCache = (int)KM.Common.CacheUtil.GetFromCache(FormAbandonCacheName + emailGroup.EmailID.ToString(), false);
                    }
                    catch { }
                    try
                    {
                        FormSubmitCache = (int)KM.Common.CacheUtil.GetFromCache(FormSubmitCacheName + emailGroup.EmailID.ToString(), false);
                    }
                    catch { }
                    List<ECN_Framework_Entities.Communicator.LayoutPlans> lplist = ECN_Framework_BusinessLayer.Communicator.LayoutPlans.GetByFormTokenUID_NoAccessCheck(apiData.TokenUID);
                    foreach (ECN_Framework_Entities.Communicator.LayoutPlans lp in lplist)
                    {
                        if (emailGroup != null && emailGroup.EmailID > 0)
                        {
                            if (lp.EventType == "abandon" && lp.LayoutPlanID == FormAbandonCache)
                            {
                                ECN_Framework_BusinessLayer.Communicator.BlastSingle.DeleteByEmailID_NoAccessCheck(emailGroup.EmailID, lp.LayoutPlanID, user);
                                //need to find NoOpen triggers on the abandon and cancel them
                                ECN_Framework_BusinessLayer.Communicator.BlastSingle.DeleteNoOpenFromAbandon_NoAccessCheck(emailGroup.EmailID, lp.LayoutPlanID, user);
                            }
                            else if (lp.EventType == "submit" && (lp.CampaignItemID.HasValue && FormSubmitCache == lp.CampaignItemID.Value))
                                ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(lp, emailGroup.EmailID, user);
                        }
                    }
                    try
                    {
                        KM.Common.CacheUtil.RemoveFromCache(FormAbandonCacheName + emailGroup.EmailID.ToString());
                        KM.Common.CacheUtil.RemoveFromCache(FormSubmitCacheName + emailGroup.EmailID.ToString());
                    }
                    catch { }
                }
                // Group Triggers
                foreach (ECN_Framework_Entities.Communicator.LayoutPlans groupTrigger in lop)
                {
                    if (emailGroup != null && emailGroup.EmailID > 0 && groupTrigger.Status == "Y" && (groupTrigger.Criteria == emailGroup.SubscribeTypeCode))
                        ECN_Framework_BusinessLayer.Communicator.EventOrganizer.Event(groupTrigger, emailGroup.EmailID, user);
                }                
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ecn, "FormSubmitter.ManageSubscriberWithProfile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                resp = "500";
                return 500;
            }
            catch (TimeoutException tex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(tex, "FormSubmitter.ManageSubscriberWithProfile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                resp = "500";
                return 500;
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "FormSubmitter.ManageSubscriberWithProfile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                resp = "500";
                return 500;
            }            
            resp = "200";
            return 200;

            //send
            //NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, apiData.AccessKey);
            //data.Add(X_Customer_ID, apiData.CustomerID);

            //return SendCommand(GetCURLWithItem("emailgroup/methods/ManageSubscriberWithProfile"), data, GetJson(apiData.GroupID, custom, tempStandard, isSubscribe, apiData.IsNewsletter), out resp);
        }

        private string BuildProfileXML(Dictionary<string, string> standardFields, bool isSubscribe)
        {
            StringBuilder sbProfile = new StringBuilder();
            sbProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
            foreach (KeyValuePair<string, string> x in standardFields)
            {
                if (x.Key.ToLower() == "phone")
                    sbProfile.Append("<" + "voice" + ">" + cleanXMLString(Convert.ToString(x.Value)) + "</" + "voice" + ">");
                else
                    sbProfile.Append("<" + x.Key.ToLower() + ">" + cleanXMLString(Convert.ToString(x.Value)) + "</" + x.Key.ToLower() + ">");
            }
            sbProfile.Append("<subscribetypecode>" + (isSubscribe ? "S" : "U") + "</subscribetypecode>");
            sbProfile.Append("</Emails></XML>");
            return sbProfile.ToString();
        }

        private string BuildUDFXML(Dictionary<string, string> customFields, string emailAddress, int groupID, List<ECN_Framework_Entities.Communicator.GroupDataFields> udfList)
        {
            StringBuilder sbUDF = new StringBuilder();
            bool rowCreated = false;
            sbUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");

            foreach (KeyValuePair<string, string> x in customFields)
            {
                if (!rowCreated)
                {
                    sbUDF.Append("<row>");
                    sbUDF.Append("<ea>" + cleanXMLString(emailAddress) + "</ea>");
                    rowCreated = true;

                }

                if (x.Value != null)
                {
                    sbUDF.Append("<udf id=\"" + udfList.First(y => y.ShortName.ToLower().Equals(x.Key.ToLower())).GroupDataFieldsID.ToString() + "\">");

                    sbUDF.Append("<v><![CDATA[" + cleanXMLString(Convert.ToString(x.Value)) + "]]></v>");

                    sbUDF.Append("</udf>");
                }

            }
            if (rowCreated)
                sbUDF.Append("</row>");
            sbUDF.Append("</XML>");
            return sbUDF.ToString();
        }

        private static string cleanXMLString(string text)
        {

            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("á", "a");
            return text;
        }

        private bool SendNotifications()
        {
            return SendNotifications(Guid.Empty);
        }

        private bool SendNotifications(Guid token)
        {
            bool res = false;
            NameValueCollection data = new NameValueCollection();
            //data.Add(APIAccessKey, form.CustomerAccessKey);
            //data.Add(X_Customer_ID, form.CustomerID.ToString());
            foreach (var n in form.Notifications.Where(x => !x.IsDoubleOptIn))
            {
                if (CheckGroup(n.ConditionGroup) && n.Message != "SubmissionErrorNotification")
                {
                    ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                    ed.CustomerID = form.CustomerID;
                    ed.Process = "FormDesigner.Notification";
                    ed.Status = "pending";
                    ed.SendTime = DateTime.Now;
                    ed.CreatedDate = DateTime.Now;
                    ed.CreatedUserID = form.UserID;
                    ed.EmailAddress = n.IsInternalUser ? n.ToEmail : ClientEmail;

                    string message = ApplySnippets(n.Form.Controls, n.Message);
                    ed.Content = message;
                    ed.EmailSubject = n.Subject;
                    ed.FromEmailAddress = DefaultFromEmail;
                    ed.ReplyEmailAddress = DefaultReplyEmail;
                    ed.FromName = n.FromName;
                    ed.Source = Source;

                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

                    res |= n.IsConfirmation;
                    //string response = null;
                    //SendCommand(GetCURLWithItem("internal/emaildirect"), data, GetNotificationJson(n, token), out response);
                }
            }

            return res;
        }

        private bool SendConfirmationResponse(Guid token)
        {
            bool res = false;
            if (form.OptInType == (int)OptInType.Double)
            {
                res = true;
                Notification n = form.Notifications.SingleOrDefault(x => x.IsDoubleOptIn);
                //NameValueCollection data = new NameValueCollection();
                //data.Add(APIAccessKey, form.CustomerAccessKey);
                //data.Add(X_Customer_ID, form.CustomerID.ToString());
                ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                ed.CustomerID = form.CustomerID;
                ed.Process = "FormDesigner.Confirmation";
                ed.Status = "pending";
                ed.SendTime = DateTime.Now;
                ed.CreatedDate = DateTime.Now;
                ed.CreatedUserID = form.UserID;
                ed.EmailAddress = n.IsInternalUser ? n.ToEmail : ClientEmail;
                string mes = ApplySnippets(n.Form.Controls, n.Message);
                //.Replace(UrlMacros, "href=\\\"" + ConfirmationUrl + "?htoken=" + token + "\\\"")
                mes = AddConfirmationUrl(mes, token);
                ed.Content = mes;
                ed.EmailSubject = n.Subject;
                ed.FromEmailAddress = DefaultFromEmail;
                ed.ReplyEmailAddress = DefaultReplyEmail;
                ed.FromName = n.FromName;
                ed.Source = Source;

                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);
                //string response = null;
                //SendCommand(GetCURLWithItem("internal/emaildirect"), data, GetConfirmationJson(n, token), out response);
            }

            return res;
        }

        private void Process3rdPartyOutput()
        {
            FormResult party = form.FormResults.SingleOrDefault(x => x.ResultType == (int)FormResultType.ThirdpartyOutput);
            if (party != null)
            {
                IEnumerable<string> query = party.ThirdPartyQueryValues.Select
                                                (x => HttpUtility.UrlEncode(x.Name) + '=' + (values.ContainsKey(x.Control_ID) ? HttpUtility.UrlEncode(values[x.Control_ID]) : string.Empty));
                string url = party.URL;
                if (query.Count() > 0)
                {
                    url += (url.IndexOf('?') == -1 ? '?' : '&') + string.Join("&", query);
                }
                SendRequest(KMEntitiesWrapper.GetURL(url));
            }
        }

        private bool SendSubmissionErrorNotifications(string message)
        {
            bool res = false;
            foreach (var n in form.Notifications.Where(x => x.IsInternalUser))
            {
                if (n.Message == "SubmissionErrorNotification" && !string.IsNullOrEmpty(n.ToEmail))
                {
                    ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();
                    ed.CustomerID = form.CustomerID;
                    ed.Process = "FormDesigner.Notification";
                    ed.Status = "pending";
                    ed.SendTime = DateTime.Now;
                    ed.CreatedDate = DateTime.Now;
                    ed.CreatedUserID = form.UserID;
                    ed.EmailAddress = n.ToEmail;
                    ed.Content = message;
                    ed.EmailSubject = n.Subject;
                    ed.FromEmailAddress = DefaultFromEmail;
                    ed.ReplyEmailAddress = DefaultReplyEmail;
                    ed.FromName = n.FromName;
                    ed.Source = Source;

                    ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

                    res |= n.IsConfirmation;
                }
            }

            return res;
        }
        #endregion

        #region other methods
        private ICollection<SubmitData> GetSubmitData(SubmitHistoryDbManager shm)
        {
            List<SubmitData> res = new List<SubmitData>();
            foreach (var item in values)
            {
                SubmitData data = new SubmitData();
                data.Control_ID = item.Key;
                data.Value = item.Value;
                shm.AddData(data);
            }

            return res;
        }

        private int SendRequest(string uri)
        {
            Logger.WriteLog("SendRequest: url is " + uri);
            int statusCode = -1;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Timeout = 30000;
            req.Method = GetVerb;

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    statusCode = (int)resp.StatusCode;
                    resp.Close();
                }
            }
            catch (Exception ex)
            {
                string note = "FormID: " + form.Form_Seq_ID + ", ThirdPartyURL:" + uri;
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "KMManagers.FormSubmitter.SendRequest", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
            }
            req.Abort();
            Logger.WriteLog("SendRequest done; status: " + statusCode);

            return statusCode;
        }

        private string GetJson(string groupID, List<string> custom, List<string> tempStandard, bool isSubscribe, bool isNewsletter)
        {
            //string customJson = string.Join(",", custom);
            //if (customJson != string.Empty)
            //{
            //    customJson = ",\"CustomFields\":{" + customJson + '}';
            //}

            string subscribe_str = isSubscribe ? HTMLGenerator.Subscribe.Substring(0, 1) : HTMLGenerator.Unsubscribe.Substring(0, 1);

            if (tempStandard.Contains("\"SubscribeType\":\"S\""))
                tempStandard[tempStandard.IndexOf("\"SubscribeType\":\"S\"")] = "\"SubscribeType\":\"" + subscribe_str + "\"";
            else if (tempStandard.Contains("\"SubscribeType\":\"U\""))
                tempStandard[tempStandard.IndexOf("\"SubscribeType\":\"U\"")] = "\"SubscribeType\":\"" + subscribe_str + "\"";
            else
            {
                tempStandard.Add("\"SubscribeType\":\"" + subscribe_str + "\"");
            }

            //return FieldsJson.Replace(GroupIDMacros, groupID).Replace(SubscribeMacros, subscribe_str).Replace(SubsMacros, subscribe_str.Substring(0, 1))
            //                    .Replace(CustomMacros, customJson).Replace(StandartMacros, string.Join(",", standart));

            return FieldsJson.Replace(GroupIDMacros, groupID).Replace(SubscribeMacros, subscribe_str)
                .Replace(CustomMacros, isNewsletter ? "" : string.Join(",", custom)).Replace(StandartMacros, string.Join(",", tempStandard));
        }

        private string GetNotificationJson(Notification n, Guid token)
        {
            return NotificationJson.Replace(SourceMacros, Source).Replace(SubjectMacros, n.Subject).Replace(FromNameMacros, n.FromName)
                    .Replace(EmailMacros, n.IsInternalUser ? n.ToEmail : ClientEmail).Replace(ReplyEmailMacros, DefaultReplyEmail).Replace(FromEmailMacros, DefaultFromEmail)
                    .Replace(HTMLMacros, ApplySnippets(n.Form.Controls, n.Message));
        }

        private string GetConfirmationJson(Notification n, Guid token)
        {
            if (n == null)
            {
                return NotificationJson.Replace(SourceMacros, Source).Replace(SubjectMacros, DoubleOptInEmailSubject).Replace(FromNameMacros, string.Empty)
                        .Replace(EmailMacros, ClientEmail).Replace(ReplyEmailMacros, DefaultReplyEmail).Replace(FromEmailMacros, DefaultFromEmail)
                        .Replace(HTMLMacros, DoubleOptInEmailTemplate.Replace(HTMLGenerator.urlMacros, "href=\\\"" + ConfirmationUrl + "?htoken=" + token + "\\\""));
            }
            else
            {
                string res = ApplySnippets(n.Form.Controls, n.Message);
                //.Replace(UrlMacros, "href=\\\"" + ConfirmationUrl + "?htoken=" + token + "\\\"")
                res = AddConfirmationUrl(res, token);

                return NotificationJson.Replace(SourceMacros, Source).Replace(SubjectMacros, n.Subject).Replace(FromNameMacros, n.FromName)
                        .Replace(EmailMacros, ClientEmail).Replace(ReplyEmailMacros, DefaultReplyEmail).Replace(FromEmailMacros, DefaultFromEmail)
                        .Replace(HTMLMacros, res);
            }
        }

        private string AddConfirmationUrl(string res, Guid token)
        {
            string[] data = res.Split(new string[] { HTMLGenerator.urlMacros }, StringSplitOptions.None);
            for (int i = 0; i < data.Length - 1; i++)
            {
                if (data[i].EndsWith(urlMacros_prev))
                {
                    data[i] = data[i].Insert(data[i].Length - 1, "\\");
                    data[i + 1] = '\\' + data[i + 1];
                }
            }

            return string.Join(ConfirmationUrl + "?htoken=" + token, data);
        }

        public string ApplySnippets(IEnumerable<Control> controls, string html)
        {
            Guard.NotNull(controls, nameof(controls));

            fieldnames = new Dictionary<string, int>();
            var ids = controls.Select(x => x.Control_ID);
            var result = html;
            var isSuccess = true;
            var start = 0;
            DataTable table = null;
            while (isSuccess)
            {
                var match = snippet.Match(result, start);
                if (match.Success)
                {
                    start = match.Index + 1;
                    var id = int.Parse(match.Groups[1].Value);
                    var replacement = string.Empty;
                    var strValue = string.Empty;
                    if (ids.Contains(id))
                    {
                        if (values.ContainsKey(id))
                        {
                            ApplySnippetsValueContainsKey(controls, id, out replacement, out strValue);
                        }
                        else
                        {
                            replacement = ApplySnippetsValueContainsNoKey(controls, id, replacement);
                        }
                    }
                    else
                    {
                        var labelStart = result.IndexOf(SignGreater, start) + 1;
                        var labelEnd = result.IndexOf(SnippetEnd, start);
                        replacement = string.Concat(
                            SnippetMacros, result.Substring(labelStart, labelEnd - labelStart), SnippetMacros);
                    }
                    result = string.Concat(
                        result.Substring(0,start - 1),
                        replacement, 
                        result.Substring(result.IndexOf(SnippetEnd, start), SnippetEnd.Length));
                }
                else
                {
                    isSuccess = false;
                }
            }
            //Grid Snippet replaced with empty 
            result = ApplySnippetsReplaceWithEmpty(controls, result);
            return result;
        }

        private string ApplySnippetsReplaceWithEmpty(IEnumerable<Control> controls, string result)
        {
            Guard.NotNull(controls, nameof(controls));

            var reg = new Regex(PercentWrappedRegexTemplate, RegexOptions.IgnoreCase);
            var matchList = reg.Matches(result);
            foreach (var control in controls)
            {
                if (control.FieldLabel != null)
                {
                    fieldnames.Add(control.FieldLabel, control.Control_ID);
                }
            }

            foreach (Match match in matchList)
            {
                var replaceName = match.Groups[1].Value;
                var replaceValue = string.Empty;
                if (fieldnames.ContainsKey(replaceName))
                {
                    replaceValue = ProcessReplacement(controls, replaceName);
                }
                else if (!fieldnames.ContainsKey(replaceName))
                {
                    replaceValue = string.Format("{0}{1}{0}", SignPercent, replaceName);
                }

                result = result.Replace(SignPercent + replaceName + SignPercent, replaceValue);
            }

            return result;
        }

        private string ApplySnippetsValueContainsNoKey(IEnumerable<Control> controls, int id, string replacement)
        {
            Guard.NotNull(controls, nameof(controls));

            DataTable table;
            var control = controls.Single(x => x.Control_ID == id);
            if (control.Type_Seq_ID == TypeSeq217)
            {
                var eControl = controls.Single(x => x.Type_Seq_ID == TypeSeq103);
                table = GetBestProfileForEmailAddress(form.GroupID, form.CustomerID, values[eControl.Control_ID]);
                if (table != null)
                {
                    if (table.Columns.Contains(ColumnPassword))
                    {
                        replacement = table.Rows[0][ColumnPassword].ToString();
                    }
                }
            }

            return replacement;
        }

        private void ApplySnippetsValueContainsKey(
            IEnumerable<Control> controls, int id, out string replacement, out string ctlValue)
        {
            var control = controls.Single(x => x.Control_ID == id);
            ctlValue = values[id] ?? string.Empty;

            replacement = ProcessControlsByType(control, ctlValue);
        }

        private string ProcessReplacement(IEnumerable<Control> controls, string replacename)
        {
            Guard.NotNull(controls, nameof(controls));

            DataTable table;
            string replaceValue;
            var control = controls.Single(x => x.Control_ID == fieldnames[replacename]);
            var ctlvalue = string.Empty;
            if (control.Type_Seq_ID == TypeSeqGrid) // not supported Control
            {
                ctlvalue = ControlStrGrid;
            }
            else if (control.Type_Seq_ID == TypeSeqIdLiteral)
            {
                ctlvalue = ControlStrLiteral; //not supported Control
            }
            else
            {
                if (values.ContainsKey(fieldnames[replacename]))
                {
                    ctlvalue = values[fieldnames[replacename]];
                }
            }

            if (!values.ContainsKey(fieldnames[replacename]) && 
                string.Equals(replacename, FieldNamePassword, StringComparison.OrdinalIgnoreCase))
            {
                var eCtl = controls.Single(x => x.Type_Seq_ID == TypeSeqId103);
                table = GetBestProfileForEmailAddress(
                    form.GroupID, form.CustomerID, values[eCtl.Control_ID]);
                if (table != null)
                {
                    if (table.Columns.Contains(replacename))
                    {
                        ctlvalue = table.Rows[0][replacename].ToString();
                    }
                }
            }

            replaceValue = ProcessControlsByType(control, ctlvalue);
            return replaceValue;
        }

        private static string ProcessControlsByType(Control control, string ctlvalue)
        {
            Guard.NotNull(control, nameof(control));

            string replaceValue;
            if (control.Type_Seq_ID == TypeSeqIdCountry && !string.IsNullOrWhiteSpace(ctlvalue))
            {
                // Country Control
                var controlManager = new ControlManager();
                ctlvalue = controlManager.GetCountryName(Convert.ToInt32(ctlvalue));
            }

            if (control.Type_Seq_ID == TypeSeqIdState && !string.IsNullOrWhiteSpace(ctlvalue))
            {
                // State Control
                var controlManager = new ControlManager();
                ctlvalue = controlManager.GetStateCode(Convert.ToInt32(ctlvalue));
            }

            if (control.HTMLControlType() == KMEnums.ControlType.NewsLetter)
            {
                if (string.IsNullOrWhiteSpace(ctlvalue))
                {
                    ctlvalue = LiteralEmpty;
                }

                var nlSubs = ctlvalue.Split(DelimComma);
                var builder = new StringBuilder();
                foreach (var newsletterGroup in control.NewsletterGroups)
                {
                    builder.AppendFormat(LabelHtmlTemplate, newsletterGroup.LabelHTML);
                    var subscribeTerm =
                        nlSubs.Any(newsletterGroup.GroupID.ToString().Contains) ? 
                            HTMLGenerator.Subscribe : 
                            HTMLGenerator.Unsubscribe;
                    builder.AppendFormat(SubscribedTemplate, subscribeTerm);
                    builder.Append(HtmlBr);
                }

                ctlvalue = builder.ToString();

                ctlvalue = ctlvalue.Remove(ctlvalue.Length - 5);
            }

            replaceValue = ctlvalue;
            return replaceValue;
        }

        private string AddLanding(Notification n, string html, Guid token)
        {
            if (n.IsConfirmation)
            {
                int start = html.IndexOf(html_closetag);
                if (start == -1)
                {
                    html += HTMLGenerator.NewLine + "<a href=\\\"" + ConfirmationUrl + "?htoken=" + token + "\\\">confirm!</a>";
                }
                else
                {
                    html = html.Insert(start, "<br />" + "<a href=\\\"" + ConfirmationUrl + "?htoken=" + token + "\\\">confirm!</a>");
                }
            }

            return html;
        }

        private string GetLandingHTML(Guid token)
        {
            Notification optIn = form.Notifications.SingleOrDefault(x => x.IsDoubleOptIn);
            string landing = string.Empty;
            if (optIn != null && optIn.LandingPage != null)
            {
                landing = ApplySnippets(form.Controls, optIn.LandingPage);
            }

            return ConfirmationTemplate.Replace(LandingHTMLMacros, landing).Replace(HTokenMacros, token.ToString());
        }

        private bool CheckGroup(ConditionGroup group)
        {
            bool res = true;
            if (group != null)
            {
                res = group.LogicGroup ? true : false;
                foreach (var child in group.ConditionGroup1)
                {
                    if (group.LogicGroup)
                    {
                        res &= CheckGroup(child);
                    }
                    else
                    {
                        res |= CheckGroup(child);
                    }
                }
                foreach (var c in group.Conditions)
                {
                    if (group.LogicGroup)
                    {
                        res &= CheckCondition(c);
                    }
                    else
                    {
                        res |= CheckCondition(c);
                    }
                }
            }

            return res;
        }

        private bool CheckCondition(Condition condition)
        {
            Guard.NotNull(condition, nameof(condition));

            var result = false;
            var control = condition.Control;
            var value = (values.ContainsKey(control.Control_ID) ? values[control.Control_ID] : string.Empty).ToLower();
            if (control.IsSelectable())
            {
                result = CheckConditionForSelectable(condition, control, value, result);
            }
            else if (control.HTMLControlType() == KMEnums.ControlType.NewsLetter)
            {
                result = CheckConditionForNewLetter(condition, value);
            }
            else
            {
                result = CheckConditionsProcessDefault(condition, result, value);
            }
            return result;
        }

        private static bool CheckConditionsProcessDefault(Condition condition, bool result, string strValue)
        {
            Guard.NotNull(condition, nameof(condition));

            var cValueLower = condition.Value.ToLower();
            switch ((ComparisonType) condition.Operation_ID)
            {
                case ComparisonType.Is:
                    result = strValue == cValueLower;
                    break;
                case ComparisonType.IsNot:
                    result = strValue != cValueLower;
                    break;
                case ComparisonType.Contains:
                    result = strValue.Contains(cValueLower);
                    break;
                case ComparisonType.DoesNotContain:
                    result = !strValue.Contains(cValueLower);
                    break;
                case ComparisonType.StartsWith:
                    result = strValue.StartsWith(cValueLower);
                    break;
                case ComparisonType.EndsWith:
                    result = strValue.EndsWith(cValueLower);
                    break;
                case ComparisonType.Equals:
                    result = CompareDoubleEquals(condition, strValue, ComparisonType.Equals);
                    break;
                case ComparisonType.LessThan:
                    result = CompareDoubleEquals(condition, strValue, ComparisonType.LessThan);
                    break;
                case ComparisonType.GreaterThan:
                    result = CompareDoubleEquals(condition, strValue, ComparisonType.GreaterThan);
                    break;
                case ComparisonType.After:
                    result = CompareDateTimeEquals(condition, strValue, ComparisonType.After);
                    break;
                case ComparisonType.Before:
                    result = CompareDateTimeEquals(condition, strValue, ComparisonType.Before);
                    break;
                case ComparisonType.IsNull:
                    result = strValue == string.Empty;
                    break;
                case ComparisonType.IsNotNull:
                    result = strValue != string.Empty;
                    break;
            }

            return result;
        }

        private static bool CompareDoubleEquals(Condition condition, string value, ComparisonType comparisonType)
        {
            Guard.NotNull(condition, nameof(condition));

            var result = false;
            try
            {
                var dValue = double.Parse(value);
                var dConditionValue = double.Parse(condition.Value);
                switch (comparisonType)
                {
                    case ComparisonType.Equals:
                        result = dValue == dConditionValue;
                        break;
                    case ComparisonType.LessThan:
                        result = dValue < dConditionValue;
                        break;
                    case ComparisonType.GreaterThan:
                        result = dValue > dConditionValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(comparisonType));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return result;
        }

        private static bool CompareDateTimeEquals(Condition condition, string value, ComparisonType comparisonType)
        {
            Guard.NotNull(condition, nameof(condition));

            var result = false;
            try
            {
                var dValue = DateTime.Parse(value);
                var dConditionValue = DateTime.Parse(condition.Value);
                switch (comparisonType)
                {
                    case ComparisonType.After:
                        result = dValue > dConditionValue;
                        break;
                    case ComparisonType.Before:
                        result = dValue < dConditionValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(comparisonType));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return result;
        }

        private static bool CheckConditionForNewLetter(Condition condition, string strValue)
        {
            Guard.NotNull(condition, nameof(condition));

            bool res;
            var cbValues = strValue == string.Empty ? 
                new string[0] : 
                strValue.Split(DelimComma);

            var sType = condition.Operation_ID == (int) SelectionComparisonType.Is
                ? SelectionComparisonType.Is
                : SelectionComparisonType.IsNot;

            res = (sType == SelectionComparisonType.Is) ? 
                cbValues.Contains(condition.Value) : 
                !cbValues.Contains(condition.Value);

            return res;
        }

        private static bool CheckConditionForSelectable(Condition condition, Control control, string value, bool result)
        {
            Guard.NotNull(condition, nameof(condition));
            Guard.NotNull(control, nameof(control));

            var sType = condition.Operation_ID == (int) SelectionComparisonType.Is
                ? SelectionComparisonType.Is
                : SelectionComparisonType.IsNot;
            FormControlPropertyGrid item;
            if (control.Type_Seq_ID == TypeSeqIdState)
            {
                var controlManager = new ControlManager();
                var allStatesData = controlManager.GetStatesAll();
                var states = new List<string>();
                foreach (var alias in allStatesData)
                {
                    var arr = alias.Split(DelimPipe);
                    states.Add(arr[1]);
                }

                var theState = states.SingleOrDefault(x => x == condition.Value);

                item = new FormControlPropertyGrid();
                item.DataValue = theState;
            }
            else
            {
                item = control.FormControlPropertyGrids.SingleOrDefault(
                    x => x.FormControlPropertyGrid_Seq_ID.ToString() == condition.Value);
            }

            if (item != null)
            {
                var cb_values = value == string.Empty ? new string[0] : value.Split(DelimComma);
                var valueToCheck = item.DataValue.ToLower();
                if (valueToCheck != string.Empty)
                {
                    if (sType == SelectionComparisonType.Is)
                    {
                        result = cb_values.Contains(valueToCheck);
                    }
                    else
                    {
                        result = !cb_values.Contains(valueToCheck);
                    }
                }
            }

            return result;
        }

        public string GetQueryString(string token)
        {
            string qs = string.Empty;
            FormDbManager FM = new FormDbManager();
            Form f = FM.GetByTokenUID(token);
            List<Control> controls = f.Controls.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].HTMLControlType() != KMEnums.ControlType.PageBreak && controls[i].HTMLControlType() != KMEnums.ControlType.Literal)
                {
                    string querystring = string.Empty;
                    foreach (var ctrlProps in controls[i].FormControlProperties)
                        if (ctrlProps.ControlProperty.PropertyType == 1 && ctrlProps.ControlProperty.PropertyName == "Querystring Parameter")
                            querystring = ctrlProps.Value;
                    if (!string.IsNullOrEmpty(querystring))
                        qs += "&" + querystring + "=[" + querystring + "]";
                }
            }
            return qs;
        }

        private string[] GetFormRules(Form form)
        {
            IEnumerable<Rule> rules = form.Rules.Where(x => x.Type == (int)RuleTypes.Form).OrderBy(x => x.Order);
            FormManager FM = new FormManager();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> fields = FM.GetFieldsByForm(form).ToList();
            string[] res = new string[rules.Count()];
            int i = 0;
            foreach (var r in rules)
            {
                string target = null;
                string type = null;
                if (r.UrlToRedirect == null)
                {
                    type = "message";
                    if (r.ActionJs != null)
                        r.Action = r.ActionJs + "<html>" + r.Action + "</html>";
                    target = HttpUtility.JavaScriptStringEncode(ApplySnippets(form.Controls, r.Action));
                }
                else
                {
                    string formattedstring = string.Empty;
                    if (r.RequestQueryValues != null)
                    {

                        bool alreadyHasParams = r.UrlToRedirect.Contains("?");
                        foreach (var item in values)
                        {
                            string theValue = item.Value;
                            // Conversion for Countries and States
                            Control c = form.Controls.Single(x => x.Control_ID == item.Key);
                            string cTypeName = GetFieldNameFromControl(c, fields);
                            if (!string.IsNullOrEmpty(cTypeName))
                            {
                                if (cTypeName.Equals("Country"))
                                {
                                    ControlManager CM = new ControlManager();
                                    int value;
                                    if (int.TryParse(item.Value, out value))
                                        theValue = CM.GetCountryName(value);
                                }
                                if (cTypeName.Equals("State"))
                                {
                                    ControlManager CM = new ControlManager();
                                    int value;
                                    if (int.TryParse(item.Value, out value))
                                        theValue = CM.GetStateCode(value);
                                }
                            }

                            string prechar = "?";
                            RequestQueryValue rqv = null;
                            try
                            {
                                rqv = r.RequestQueryValues.FirstOrDefault(x => x.Control_ID == item.Key);//&& !x.IsDeleted);

                                if (rqv != null)
                                {
                                    if (alreadyHasParams)
                                    {
                                        prechar = "&";
                                        formattedstring += prechar + rqv.Name + "=" + theValue;
                                    }
                                    else
                                    {
                                        formattedstring += prechar + rqv.Name + "=" + theValue;
                                        alreadyHasParams = true;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                MethodBase mb = ex.TargetSite;
                                Logger.Log(mb.DeclaringType.FullName + '.' + mb.Name, ex);
                            }

                        }
                    }
                    r.UrlToRedirect = r.UrlToRedirect + formattedstring;
                    type = "url";
                    target = HttpUtility.UrlEncode(r.UrlToRedirect);
                }
                res[i] = '[' + r.ConditionGroup.ToJson() + ",\"" + type + "\",\"" + target + "\"]";
                i++;
            }

            return res;
        }

        #endregion
    }
}