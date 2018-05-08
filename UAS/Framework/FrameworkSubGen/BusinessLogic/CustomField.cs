using System;
using System.Collections.Generic;
using FrameworkSubGen.Entity;
using static Core_AMS.Utilities.XmlFunctions;
using static FrameworkSubGen.BusinessLogic.API.Authentication;

namespace FrameworkSubGen.BusinessLogic
{
    public class CustomField : BusinessLogicBase
    {
        private const string EntityName = "CustomField";

        public bool SaveBulkXml(IList<Entity.CustomField> customFields, int accountId)
        {
            if (customFields == null)
            {
                throw new ArgumentNullException(nameof(customFields));
            }

            CleanForXml(customFields);
            foreach (var customField in customFields)
            {
                customField.account_id = accountId;

                if (customField.value_options == null)
                {
                    continue;
                }

                CleanForXml(customField.value_options);
                foreach (var valueOption in customField.value_options)
                {
                    valueOption.account_id = accountId;
                    valueOption.field_id = customField.field_id;
                }
            }

            return SaveBulkXml(customFields, EntityName);
        }

        public List<Entity.CustomField> Select(int accountId)
        {
            var customFields = DataAccess.CustomField.Select(accountId);

            foreach (var customField in customFields)
            {
                customField.value_options = DataAccess.ValueOption.Select(customField.field_id);
            }

            return customFields;
        }

        protected override void FormatData(IEntity entity)
        {
            try
            {
                var customField = entity as Entity.CustomField;

                if (customField == null)
                {
                    return;
                }

                customField.name = TruncateString(customField.name, FieldLength255);
                customField.display_as = TruncateString(customField.display_as, FieldLength255);
                customField.text_value = TruncateString(customField.text_value, FieldLength255);
                customField.KMProductCode = TruncateString(customField.KMProductCode, FieldLength50);
            }
            catch (FormatException formatException)
            {
                SaveApiLog(formatException, GetType().ToString(), GetType().Name);
            }
        }

        protected override bool Save(string xml)
        {
            return DataAccess.CustomField.SaveBulkXml(xml);
        }

        private void CleanForXml(IEnumerable<Entity.CustomField> customFields)
        {
            foreach (var customField in customFields)
            {
                if (!string.IsNullOrWhiteSpace(customField.name))
                {
                    customField.name = CleanAllXml(customField.name);
                }

                if (!string.IsNullOrWhiteSpace(customField.display_as))
                {
                    customField.display_as = CleanAllXml(customField.display_as);
                }

                if (!string.IsNullOrWhiteSpace(customField.text_value))
                {
                    customField.text_value = CleanAllXml(customField.text_value);
                }

                if (!string.IsNullOrWhiteSpace(customField.KMProductCode))
                {
                    customField.KMProductCode = CleanAllXml(customField.KMProductCode);
                }

                if (customField.type != Enums.HtmlFieldType.checkbox
                    && customField.type != Enums.HtmlFieldType.radio
                    && customField.type != Enums.HtmlFieldType.select
                    && customField.type != Enums.HtmlFieldType.text
                    && customField.type != Enums.HtmlFieldType.textarea)
                {
                    customField.type = Enums.HtmlFieldType.text;
                }
            }
        }

        private void CleanForXml(IEnumerable<Entity.ValueOption> valueOptions)
        {
            foreach (var valueOption in valueOptions)
            {
                if (!string.IsNullOrWhiteSpace(valueOption.value))
                {
                    valueOption.value = CleanAllXml(valueOption.value);
                }

                if (!string.IsNullOrWhiteSpace(valueOption.display_as))
                {
                    valueOption.display_as = CleanAllXml(valueOption.display_as);
                }

                if (!string.IsNullOrWhiteSpace(valueOption.KMProductCode))
                {
                    valueOption.KMProductCode = CleanAllXml(valueOption.KMProductCode);
                }
            }
        }
    }
}
