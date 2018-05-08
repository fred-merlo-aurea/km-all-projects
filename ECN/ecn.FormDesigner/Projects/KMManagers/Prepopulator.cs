using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using KM.Common;
using KM.Common.Extensions;
using KMDbManagers;
using KMEntities;
using KMEnums;
using CommonApplicationLog = KM.Common.Entity.ApplicationLog;

namespace KMManagers
{
    public class Prepopulator : APIRunnerBase
    {
        private const string FilterChild = "child=true";
        private const string AppSettingMasterAccesKey = "MasterAccessKey";
        private const string FieldNamePhone = "Phone";
        private const string FieldNameVoice = "Voice";
        private const string FieldNameState = "State";
        private const string FieldNamePassword = "Password";
        private const string FieldNameKmpsFormPwd = "KMPS_Form_Pwd";
        private const string ControlValueTemplate = "\"{0}\":\"{1}\"";
        private const string GroupItemTemplate = "\"{0}_{1}_{2}\":\"{3}\"";
        private const string CodeSubscrive = "S";
        private const string CodeUnsubscribe = "U";
        private const string HideControlTemplate = "\"{0}_{1}_{2}\":\"hide_control\"";
        private const string TokenEmailAgentTemplate = "FormToken: {0}, Email:{1}, User Agent: {2}";
        private const string SourceMethodValidate = "KMManagers.Prepopulator.Validate";
        private const string AppSettingCommonApplication = "KMCommon_Application";
        private const string JsonValuesTemplate = "{{{0}}}";

        private Form form;

        public string Get(HttpRequest req)
        {
            var list = new List<string>();
            var email = string.Empty;
            try
            {
                var fManager = new FormDbManager();
                var isChild = req.UrlReferrer?.Query?.IndexOf(FilterChild, StringComparison.OrdinalIgnoreCase) != -1;
                form = isChild ? 
                    fManager.GetChildByTokenUID(req[HTMLGenerator.HiddenIDForToken]) : 
                    fManager.GetByTokenUID(req[HTMLGenerator.HiddenIDForToken]);
                if (Validate(req, out email))
                {
                    var propertyManager = new ControlPropertyManager();
                    var response = CheckEmailByNewsLetter(
                        ConfigurationManager.AppSettings[AppSettingMasterAccesKey], 
                        form.CustomerID, 
                        form.GroupID, 
                        email);
                    var scriptSerializer = new JavaScriptSerializer();
                    var deserializeDictionary = (Dictionary<string, object>)scriptSerializer.DeserializeObject(response);
                    if (deserializeDictionary != null)
                    {
                        FillDictionaryAllControls(deserializeDictionary, propertyManager, list);
                    }
                    FillDictionaryGroups(propertyManager, email, scriptSerializer, list);
                }
                else
                {
                    list = null;
                }
            }
            catch (Exception exception)
            {
                var note = string.Format(
                    TokenEmailAgentTemplate, 
                    req[HTMLGenerator.HiddenIDForToken], 
                    email, 
                    req.UserAgent);
                CommonApplicationLog.LogNonCriticalError(
                    exception, 
                    SourceMethodValidate, 
                    Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingCommonApplication]), 
                    note);
            }

            var json = (list != null) ?
                string.Format(JsonValuesTemplate, string.Join(", ", list)) :
                email;

            return json;
        }

        private void FillDictionaryGroups(
            ControlPropertyManager propertyManager, 
            string email,
            JavaScriptSerializer scriptSerializer, 
            ICollection<string> list)
        {
            Guard.NotNull(propertyManager, nameof(propertyManager));
            Guard.NotNull(scriptSerializer, nameof(scriptSerializer));
            Guard.NotNull(list, nameof(list));

            foreach (var control in form.Controls.Where(x => x.HTMLControlType() == KMEnums.ControlType.NewsLetter))
            {
                var property = propertyManager.GetPropertyByNameAndControl(HTMLGenerator.PrepopulateFrom_Property, control);
                if (property != null)
                {
                    var formPropertyValue = control.GetFormPropertyValue(property);
                    if (formPropertyValue != null)
                    {
                        var type = ToEnum<PopulationType>(formPropertyValue);
                        if (type == PopulationType.Database)
                        {
                            var nlRemovedFromNlControl = 0;
                            foreach (var nlGroup in control.NewsletterGroups)
                            {
                                var groupId = nlGroup.GroupID;
                                if (groupId != form.GroupID)
                                {
                                    var customerId = nlGroup.CustomerID;
                                    var accessKey = ConfigurationManager.AppSettings[AppSettingMasterAccesKey];
                                    var nResp = CheckEmailByNewsLetter(accessKey, customerId, groupId, email);
                                    var dictionary = (Dictionary<string, object>) scriptSerializer.DeserializeObject(nResp);
                                    if (dictionary != null && dictionary.ContainsKey(FormSubmitter.SubscribeTypeCode))
                                    {
                                        list.Add(string.Format(GroupItemTemplate,
                                            control.HTMLID,
                                            groupId,
                                            HTMLGenerator.Subscribe.ToLower(),
                                            dictionary[FormSubmitter.SubscribeTypeCode]));
                                        if (CodeSubscrive.EqualsIgnoreCase(dictionary[FormSubmitter.SubscribeTypeCode].ToString()) &&
                                            CodeUnsubscribe.EqualsIgnoreCase(dictionary[FormSubmitter.SubscribeTypeCode].ToString()))
                                        {
                                            nlRemovedFromNlControl++;
                                        }
                                    }
                                }
                            }
                            AddNewLetterControl(list, nlRemovedFromNlControl, control);
                        }
                    }
                }
            }
        }

        private static void AddNewLetterControl(ICollection<string> list, int nlRemovedFromNlControl, Control control)
        {
            Guard.NotNull(control, nameof(control));

            if (nlRemovedFromNlControl > 0 &&
                nlRemovedFromNlControl == control.NewsletterGroups.Count())
            {
                list.Add(string.Format(
                    HideControlTemplate,
                    control.HTMLID,
                    control.NewsletterGroups.First().GroupID,
                    HTMLGenerator.Subscribe.ToLower()));
            }
        }

        private void FillDictionaryAllControls(
            Dictionary<string, object> responseDictionary, 
            ControlPropertyManager propertyManager, 
            ICollection<string> list)
        {
            Guard.NotNull(responseDictionary, nameof(responseDictionary));

            var dictionary = new Dictionary<string, object>(responseDictionary, StringComparer.OrdinalIgnoreCase);
            var formManager = new FormManager();
            var fields = formManager.GetFieldsByForm(form).ToList();
            var controls = form.Controls.OrderBy(x => x.Order).ToList();
            foreach (var control in controls)
            {
                var property = propertyManager.GetPropertyByNameAndControl(HTMLGenerator.PrepopulateFrom_Property, control);
                if (property != null)
                {
                    var formPropertyValue = control.GetFormPropertyValue(property);
                    if (formPropertyValue != null)
                    {
                        var type = ToEnum<PopulationType>(formPropertyValue);
                        if (type == PopulationType.Database)
                        {
                            var fieldName = GetFieldNameFromControl(control, fields);
                            if (fieldName != null)
                            {
                                if (FieldNamePhone.EqualsIgnoreCase(fieldName))
                                {
                                    fieldName = FieldNameVoice;
                                }

                                if (FieldNameState.EqualsIgnoreCase(fieldName) && dictionary[fieldName] != null)
                                {
                                    var controlManager = new ControlManager();
                                    dictionary[fieldName] = controlManager.GetStateName(dictionary[fieldName].ToString());
                                }
                                else if (FieldNamePassword.EqualsIgnoreCase(fieldName) &&
                                         dictionary[FieldNamePassword] != null)
                                {
                                    dictionary[fieldName] = FieldNameKmpsFormPwd;
                                }
                            }

                            if (fieldName != null && dictionary.ContainsKey(fieldName))
                            {
                                list.Add(string.Format(ControlValueTemplate, control.HTMLID, dictionary[fieldName]));
                            }
                        }
                    }
                }
            }
        }

        private bool Validate(HttpRequest req, out string email)
        {
            ControlPropertyManager cpm = new ControlPropertyManager();
            Control email_control = form.Controls.Single(x => x.Type_Seq_ID == (int)KMEnums.ControlType.Email);
            bool res = cpm.Validate(email_control, req[email_control.HTMLID.ToString()], out email);
            res &= email != null;
            if (res)
            {
                res = CheckEmail(ref email);
            }

            return res;
        }

        private static T ToEnum<T>(string str) where T: struct
        {
            T result;
            Enum.TryParse(str, out result);
            return result;
        }
    }
}
