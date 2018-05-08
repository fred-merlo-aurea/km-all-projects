using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using KM.Common;
using KMEntities;
using KMDbManagers;
using KMEnums;
using KMControlType = KMEnums.ControlType;

namespace KMManagers
{
    public static class KMEntitiesWrapper
    {
        private const string and = "\"and\"";
        private const string or = "\"or\"";
        private const string tab = "\t";
        private const string requiredRule = "required: true";
        private const string dateRule = "date: true";
        private const string minlength = "minlength: ";
        private const string maxlength = "maxlength: ";
        private const string regexRule = "customrex: ";
        private const string gridtype = "gridtype: ";
        private const string OnlyLettersAndDigitsRex = "[^A-z0-9- ]";
        private const string http = "http://";
        private const string https = "https://";
        private const string FindMacros = "%find%";
        private const string input = " input";
        private static readonly string newRuleLine = NewLine + GetTab(3);
        private const string ErrorMessageStart = "messages: {";
        private const string JQueryMinError = "jQuery.validator.format(\"Please, select at least {0} items\")";
        private const string JQueryMaxError = "jQuery.validator.format(\"Please, select no more than {0} items\")";
        private const char DelimComma = ',';
        private const char DelimUnderscore = '_';
        private const char DelimPipe = '|';
        private const string DelimOctothorp = "#";
        private const char DelimSemicolon = ';';
        private const string DelimCommaStr = ",";

        private const int IndexFirst = 1;
        private const int IndexSecond = 2;
        private const int Offset1 = 1;
        private const int Offset65 = 65;
        private const int Length1 = 1;
        private const int Tab1 = 1;
        private const int Tab2 = 2;
        private const string CaptchaValidate = "$(\"[name='captchaValidate']\").rules(\"add\",";
        private const string RequiredTrue = " {required:true});";
        private const string RuleTemplate = "{0}$(\"[name='{1}']{2}\").rules(\"add\",{{";
        private const string RuleToAddTemplate = "{0}{1}\"{2}\",";
        private const string RuleToAddRewriteTemplate = "{0}{1}}},";
        private const string RuleEndsWithComaDelimTemplate = "{0}{1}{2}}});";

        private static string NewLine
        {
            get
            {
                return HTMLGenerator.NewLine;
            }
        }

        private static string GetAdvancedRule(Guid id, string find, string rule, string value)
        {
            string res = GetTab(2) + "$(\"#" + id + (find ?? input) + "\").each(function(i, el){";
            res += newRuleLine + "$(el).rules(\"add\",{";
            res += NewLine + GetTab(4) + rule + value;
            res += newRuleLine + "});";
            res += NewLine + GetTab(2) + "});";

            return res;
        }

        public static bool IsActive(this Form f)
        {
            return (f.Active == (int)FormActive.UseActivationDates && f.ActivationDateFrom <= DateTime.Now && f.ActivationDateTo > DateTime.Now) || f.Active == (int)FormActive.Active;
        }
        
        public static string HTMLControlTypeName(this Control c)
        {
            return (c.ControlType.MainType_ID.HasValue ? c.ControlType.ControlType2 : c.ControlType).Name;
        }

        public static KMEnums.ControlType HTMLControlType(this Control c)
        {
            return (KMEnums.ControlType)(c.ControlType.MainType_ID.HasValue ? c.ControlType.MainType_ID : c.Type_Seq_ID);
        }

        public static bool IsCustom(this Control c)
        {
            return !c.ControlType.MainType_ID.HasValue;
        }

        public static FormControlProperty GetFormProperty(this Control c, ControlProperty p)
        {
            return c.FormControlProperties.SingleOrDefault(x => x.ControlProperty_ID == p.ControlProperty_Seq_ID);
        }

        public static string GetFormPropertyValue(this Control c, string name)
        {
            ControlPropertyManager cpm = new ControlPropertyManager();
            ControlProperty p = cpm.GetPropertyByNameAndControl(name, c);

            return c.GetFormPropertyValue(p);
        }

        public static string GetFormPropertyValue(this Control c, ControlProperty p)
        {
            FormControlProperty fp = c.GetFormProperty(p);

            return fp == null ? null : fp.Value;
        }

        public static IEnumerable<FormControlPropertyGrid> GetProperties(this Control c, ControlProperty p)
        {
            IEnumerable<FormControlPropertyGrid> res = new List<FormControlPropertyGrid>();
            if (p != null)
            {
                res = c.FormControlPropertyGrids.Where(x => x.ControlProperty_ID == p.ControlProperty_Seq_ID);
            }

            return res;
        }

        public static IEnumerable<FormControlPropertyGrid> GetProperties(this Control c, string name)
        {
            ControlPropertyManager cpm = new ControlPropertyManager();
            ControlProperty p = cpm.GetPropertyByNameAndControl(name, c);

            return c.GetProperties(p);
        }

        public static bool IsRequired(this Control c)
        {
            bool required = false;
            ControlPropertyManager cpm = new ControlPropertyManager();
            ControlProperty property = cpm.GetRequiredPropertyByControl(c);
            if (property != null)
            {
                FormControlProperty value = c.FormControlProperties.SingleOrDefault(x => x.ControlProperty_ID == property.ControlProperty_Seq_ID);
                required = value != null && value.Value != null && (value.Value == "1" || value.Value.ToLower() == "true");
            }

            return required;
        }

        public static bool IsSelectable(this Control c)
        {
            KMEnums.ControlType c_type = c.HTMLControlType();

            return c_type == KMEnums.ControlType.CheckBox || c_type == KMEnums.ControlType.RadioButton ||
                    c_type == KMEnums.ControlType.ListBox || c_type == KMEnums.ControlType.DropDown;
        }

        public static string Text(this FormControlPropertyGrid p)
        {
            string text = p.DataText;
            if (string.IsNullOrEmpty(text))
            {
                text = p.DataValue;
            }

            return text;
        }

        public static int Form_ID(this ThirdPartyQueryValue tp)
        {
            return tp.FormResult.Form_Seq_ID;
        }

        public static string ToJson(this ConditionGroup gr)
        {
            string res = gr.LogicGroup ? and : or;
            res += ":[";
            foreach (var inner in gr.ConditionGroup1)
            {
                res += inner.ToJson() + ',';
            }
            foreach (var condition in gr.Conditions)
            {
                res += condition.ToJson() + ',';
            }
            res = res.Substring(0, res.Length - 1) + ']';

            return '{' + res + '}';
        }

        public static string ToJson(this Condition c)
        {
            string value = c.Value ?? string.Empty;
            if (c.Control.IsSelectable())
            {
                FormControlPropertyGrid grid_item = c.Control.FormControlPropertyGrids.SingleOrDefault(x => x.FormControlPropertyGrid_Seq_ID == int.Parse(c.Value));
                if (grid_item != null)
                {
                    value = grid_item.DataValue ?? string.Empty;
                }
            }

            return "[\"" + c.Control.HTMLID + "\",\"" + c.Operation_ID + "\",\"" + value + "\"]";
        }

        public static string RealFileName(this CssFile css)
        {
            return css.CssFileUID + CssFileDbManager.CssExt;
        }

        public static string Read(this CssFile css)
        {
            string data = string.Empty;
            try
            {
                data = File.ReadAllText(CssFileDbManager.GetFullName(css));
            }
            catch
            { }

            return data;
        }

        public static void Write(this CssFile css, string data)
        {
            try
            {
                File.WriteAllText(CssFileDbManager.GetFullName(css), AddLines(data));
            }
            catch
            { }
        }

        public static string GetValidationRules(
            this ControlPropertyManager propertyManager, 
            Control control, 
            DataTypePatternManager patternManager)
        {
            Guard.NotNull(control, nameof(control));

            var rewriteAll = false;
            var result = string.Format(RuleTemplate, GetTab(Tab2), control.HTMLID, FindMacros);
            var rulesToAdd = string.Empty;
            var isNeedRequired = control.IsRequired();
            var controlType = control.HTMLControlType();
            switch (controlType)
            {
                case KMControlType.TextBox:
                    var type = (TextboxDataTypes)int.Parse(control.GetFormPropertyValue(HTMLGenerator.DataType_Property));
                    var pattern = string.Empty;
                    if (type == TextboxDataTypes.Custom)
                    {
                        pattern = control.GetFormPropertyValue(HTMLGenerator.Regex_Property);
                    }
                    else
                    {
                        pattern = patternManager.GetPatternByType(type);
                    }
                    if (!string.IsNullOrWhiteSpace(pattern))
                    {
                        rulesToAdd += string.Format(
                            RuleToAddTemplate, 
                            newRuleLine, regexRule, HttpUtility.JavaScriptStringEncode(pattern));
                    }
                    break;
                case KMControlType.Grid:
                    rewriteAll = true;
                    result = string.Empty;
                    var gvType = (GridValidation)int.Parse(control.GetFormPropertyValue(HTMLGenerator.GridValidation_Property));
                    if (gvType != GridValidation.NotRequired)
                    {
                        result = GetAdvancedRule(control.HTMLID, null, gridtype, ((int)gvType).ToString());
                    }
                    break;
                case KMControlType.Captcha:
                    result = string.Concat(newRuleLine, CaptchaValidate, RequiredTrue);
                    rewriteAll = true;
                    break;
            }

            if (!rewriteAll)
            {
                var noRewriteArgs = new NoRewriteArgs {
                    PropertyManager = propertyManager,
                    Control = control,
                    RulesToAdd = rulesToAdd,
                    ControlType = controlType,
                    IsNeedRequired = isNeedRequired,
                    Result = result};
                result = Result(noRewriteArgs);
            }

            return result;
        }

        private static string Result(NoRewriteArgs noRewriteArgs)
        {
            Guard.NotNull(noRewriteArgs, nameof(noRewriteArgs));

            var strLen = noRewriteArgs.PropertyManager.GetPropertyByNameAndControl(
                HTMLGenerator.StrLen_Property, noRewriteArgs.Control);
            var valuesCount = noRewriteArgs.PropertyManager.GetPropertyByNameAndControl(
                HTMLGenerator.ValuesCount_Property, noRewriteArgs.Control);
            var properties = string.Empty;
            if (strLen != null)
            {
                properties = noRewriteArgs.Control.GetFormPropertyValue(strLen);
            }

            if (string.IsNullOrWhiteSpace(properties) && valuesCount != null)
            {
                properties = noRewriteArgs.Control.GetFormPropertyValue(valuesCount);
            }

            var hasMin = false;
            var hasMax = false;
            if (!string.IsNullOrWhiteSpace(properties))
            {
                var data = properties.Split(DelimSemicolon);
                if (data[0] != string.Empty)
                {
                    noRewriteArgs.RulesToAdd += string.Concat(newRuleLine, minlength, data[0], DelimComma);
                    hasMin = true;
                    if ((noRewriteArgs.ControlType == KMControlType.CheckBox ||
                         noRewriteArgs.ControlType == KMControlType.ListBox) && 
                        int.Parse(data[0]) > 0 && !noRewriteArgs.IsNeedRequired)
                    {
                        noRewriteArgs.IsNeedRequired = true;
                    }
                }

                if (data[IndexFirst] != string.Empty)
                {
                    noRewriteArgs.RulesToAdd += newRuleLine + maxlength + data[IndexFirst] + DelimComma;
                    hasMax = true;
                }
            }

            if (noRewriteArgs.IsNeedRequired)
            {
                noRewriteArgs.Result += newRuleLine + requiredRule + DelimComma;
            }

            if ((noRewriteArgs.ControlType == KMControlType.CheckBox ||
                 noRewriteArgs.ControlType == KMControlType.ListBox) && (hasMin || hasMax))
            {
                noRewriteArgs.RulesToAdd = GetValidationRulesForCheckListBox(hasMin, hasMax, noRewriteArgs.RulesToAdd);
            }

            noRewriteArgs.Result += noRewriteArgs.RulesToAdd;

            noRewriteArgs.Result = GetValidationResultOnEndsWithComma(noRewriteArgs.Result);
            noRewriteArgs.Result = noRewriteArgs.Result.Replace(FindMacros, string.Empty);
            return noRewriteArgs.Result;
        }

        private static string GetValidationRulesForCheckListBox(bool hasMin, bool hasMax, string rulesToAdd)
        {
            var builder = new StringBuilder();
            builder.Append(newRuleLine);
            builder.Append(ErrorMessageStart);
            if (hasMin)
            {
                builder.Append(newRuleLine);
                builder.Append(GetTab(Tab1));
                builder.Append(minlength);
                builder.Append(JQueryMinError);
                builder.Append(DelimComma);
            }

            if (hasMax)
            {
                builder.Append(newRuleLine);
                builder.Append(GetTab(Tab1));
                builder.Append(maxlength);
                builder.Append(JQueryMinError);
                builder.Append(DelimComma);
            }

            var errorMessagePart = builder.ToString();

            rulesToAdd +=
                string.Format(
                    RuleToAddRewriteTemplate,
                    errorMessagePart.Substring(0, errorMessagePart.Length - 1), newRuleLine);
            return rulesToAdd;
        }

        private static string GetValidationResultOnEndsWithComma(string result)
        {
            Guard.NotNull(result, nameof(result));

            if (result.EndsWith(DelimCommaStr))
            {
                result = string.Format(
                            RuleEndsWithComaDelimTemplate,
                            result.Substring(0, result.Length - 1), NewLine, GetTab(Tab2));
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }


        public static bool Validate(
            this ControlPropertyManager controlPropertyManager, 
            Control control, 
            string paramValue, 
            out string storedValue)
        {
            Guard.NotNull(control, nameof(control));

            paramValue = paramValue ?? string.Empty;
            var result = !control.IsRequired() || paramValue != string.Empty;
            if (result)
            {
                switch (control.HTMLControlType())
                {
                    case KMEnums.ControlType.TextBox:
                        result = ValidateTextBox(controlPropertyManager, control, paramValue);
                        break;
                    case KMEnums.ControlType.TextArea:
                        result = CheckStrLen(controlPropertyManager, control, paramValue);
                        break;
                    case KMEnums.ControlType.CheckBox:
                    case KMEnums.ControlType.ListBox:
                        result = CheckValuesAllowed(controlPropertyManager, control, paramValue);
                        break;
                    case KMEnums.ControlType.Grid:
                        paramValue = ValidateGrid(control, paramValue, ref result);
                        break;
                }
            }
            storedValue = paramValue;

            return result;
        }

        private static string ValidateGrid(Control control, string paramValue, ref bool result)
        {
            Guard.NotNull(control, nameof(control));

            RemoveON(ref paramValue);
            var values = paramValue.Split(new[] {DelimComma}, StringSplitOptions.RemoveEmptyEntries);
            var valType = (GridValidation)int.Parse(
                control.GetFormPropertyValue(HTMLGenerator.GridValidation_Property));
            switch (valType)
            {
                case GridValidation.OneResponse:
                    result = values.Length == Length1;
                    break;
                case GridValidation.AtLeastOne:
                    result = values.Length > 0;
                    break;
                case GridValidation.AtLeastOnePerLine:
                    var rows = new List<int>();
                    foreach (var curValue in values)
                    {
                        var secondValue = int.Parse(curValue.Split(DelimUnderscore)[1]);
                        rows.Add(secondValue);
                    }

                    result = false;
                    if (rows.Count > 0)
                    {
                        var rowsCount = control.GetProperties(HTMLGenerator.Rows_Property).Count();
                        if (rows.Count >= rowsCount)
                        {
                            result = true;
                            for (var iRow = 0; iRow < rowsCount; iRow++)
                            {
                                result = rows.Contains(iRow);
                                if (!result)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }

            var dict = ValidateGridFillDictionary(values);

            var gridValue = new string[dict.Keys.Count];
            var index = 0;
            foreach (var item in dict)
            {
                gridValue[index++] = string.Concat(item.Key.ToString(), DelimPipe, item.Value);
            }

            paramValue = string.Join(DelimOctothorp, gridValue);
            return paramValue;
        }

        private static Dictionary<char, string> ValidateGridFillDictionary(string[] values)
        {
            Guard.NotNull(values, nameof(values));

            var dict = new Dictionary<char, string>();
            foreach (var curValue in values)
            {
                var data = curValue.Split(DelimUnderscore);
                var row = (char) (int.Parse(data[IndexFirst]) + Offset65);
                var column = int.Parse(data[IndexSecond]) + Offset1;
                if (dict.ContainsKey(row))
                {
                    dict[row] += string.Concat(DelimComma, column);
                }
                else
                {
                    dict.Add(row, column.ToString());
                }
            }

            return dict;
        }

        private static bool ValidateTextBox(ControlPropertyManager propertyManager, Control control, string paramValue)
        {
            bool result;
            result = CheckStrLen(propertyManager, control, paramValue);
            if (result)
            {
                var type = (TextboxDataTypes) int.Parse(control.GetFormPropertyValue(HTMLGenerator.DataType_Property));
                if (type != TextboxDataTypes.Text)
                {
                    var pattern = string.Empty;
                    if (type == TextboxDataTypes.Custom)
                    {
                        pattern = control.GetFormPropertyValue(HTMLGenerator.Regex_Property);
                    }
                    else
                    {
                        var dataTypePatternManager = new DataTypePatternManager();
                        pattern = dataTypePatternManager.GetPatternByType(type);
                    }

                    if (string.IsNullOrWhiteSpace(pattern))
                    {
                        switch (type)
                        {
                            case TextboxDataTypes.Date:
                                var date = DateTime.MinValue;
                                result = DateTime.TryParse(paramValue, out date);
                                break;
                        }
                    }
                    else
                    {
                        result = Regex.IsMatch(paramValue, pattern);
                    }
                }
            }

            return result;
        }

        private static bool CheckStrLen(ControlPropertyManager cpm, Control c, string value)
        {
            int min = -1;
            int max = -1;
            FillMinMax(ref min, ref max, c, cpm, HTMLGenerator.StrLen_Property);

            return (min == -1 || value.Length >= min) && (max == -1 || value.Length <= max);
        }

        private static bool CheckValuesAllowed(ControlPropertyManager cpm, Control c, string value)
        {
            int min = -1;
            int max = -1;
            FillMinMax(ref min, ref max, c, cpm, HTMLGenerator.ValuesCount_Property);
            string[] values = string.IsNullOrEmpty(value) ? new string[0] : value.Split(',');

            return (min == -1 || values.Length >= min) && (max == -1 || values.Length <= max);
        }

        private static void FillMinMax(ref int min, ref int max, Control c, ControlPropertyManager cpm, string name)
        {
            ControlProperty property = cpm.GetPropertyByNameAndControl(name, c);
            string data = c.GetFormPropertyValue(property);
            if (data != null)
            {
                string[] values = data.Split(';');
                if (values.Length == 2)
                {
                    if (values[0] != string.Empty)
                    {
                        min = int.Parse(values[0]);
                    }
                    if (values[1] != string.Empty)
                    {
                        max = int.Parse(values[1]);
                    }
                }
            }
        }

        public static string OnlyLettersAndDigits(string label)
        {
            return Regex.Replace(label, OnlyLettersAndDigitsRex, string.Empty);
        }

        public static bool RemoveON(ref string value)
        {
            bool res = false;
            if (value.StartsWith("on,"))
            {
                res = true;
                value = value.Substring(3);
            }
            if (value.EndsWith(",on"))
            {
                res = true;
                value = value.Substring(0, value.Length - 3);
            }
            if (value.IndexOf(",on,") > -1)
            {
                res = true;
                value = value.Replace(",on,", ",");
            }

            return res;
        }

        public static string GetURL(string url)
        {
            if (url != string.Empty && !url.ToLower().StartsWith(http) && !url.ToLower().StartsWith(https))
            {
                url = http + url;
            }

            return url;
        }

        private static string AddLines(string data)
        {
            return data.Replace("{", '{' + NewLine + tab).Replace(";", ';' + NewLine + tab).Replace(tab + '}', '}' + NewLine + NewLine).TrimEnd(NewLine.ToCharArray());
        }

        private static string GetTab(int count)
        {
            string res = string.Empty;
            for (int i = 0; i < count; i++)
            {
                res += tab;
            }

            return res;
        }
    }
}