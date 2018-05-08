using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMModels.Wrappers
{
    public static class ControlWrapper
    {
        private const string LabelPropertyName = "Values";
        private const string RequiredPropertyName = "Required";

        public static ControlProperty GetValuePropertyByControl(this IEnumerable<ControlProperty> properties, Control c)
        {
            return properties.GetPropertyByNameAndControl(LabelPropertyName, c);
        }

        public static ControlProperty GetRequiredPropertyByControl(this IEnumerable<ControlProperty> properties, Control c)
        {
            return properties.GetPropertyByNameAndControl(RequiredPropertyName, c);
        }

        public static ControlProperty GetPropertyByNameAndControl(this IEnumerable<ControlProperty> properties, string name, Control c)
        {
            ControlProperty res = properties.SingleOrDefault(x => x.Type_ID == (c.ControlType.MainType_ID.HasValue ? c.ControlType.MainType_ID : c.Type_Seq_ID) && x.PropertyName == name);
            if (res == null && c.ControlType.MainType_ID.HasValue)
            {
                res = properties.SingleOrDefault(x => x.Type_ID == c.Type_Seq_ID && x.PropertyName == name);
            }

            return res;
        }      

        public static FormControlProperty GetFormProperty(this Control c, ControlProperty p)
        {
            return c.FormControlProperties.SingleOrDefault(x => x.ControlProperty_ID == p.ControlProperty_Seq_ID);
        }

        public static string GetFormPropertyValue(this Control c, string name, IEnumerable<ControlProperty> properties)
        {
            ControlProperty p = properties.GetPropertyByNameAndControl(name, c);

            return c.GetFormPropertyValue(p);
        }

        private static string GetFormPropertyValue(this Control c, ControlProperty p)
        {
            FormControlProperty fp = c.GetFormProperty(p);

            return fp == null ? null : fp.Value;
        }

        public static IEnumerable<FormControlPropertyGrid> GetProperties(this Control c, ControlProperty p)
        {
            IEnumerable<FormControlPropertyGrid> res = null;
            if (p != null)
            {
                res = c.FormControlPropertyGrids.Where(x => x.ControlProperty_ID == p.ControlProperty_Seq_ID);
            }

            return res;
        }

        public static IEnumerable<FormControlPropertyGrid> GetProperties(this Control c, string name, IEnumerable<ControlProperty> properties)
        {
            ControlProperty p = properties.GetPropertyByNameAndControl(name, c);

            return c.GetProperties(p);
        }

        public static bool IsRequired(this Control c, IEnumerable<ControlProperty> properties)
        {
            bool required = false;
            ControlProperty property = properties.GetRequiredPropertyByControl(c);
            if (property != null)
            {
                FormControlProperty value = c.FormControlProperties.SingleOrDefault(x => x.ControlProperty_ID == property.ControlProperty_Seq_ID);
                required = value != null && value.Value != null && (value.Value == "1" || value.Value.ToLower() == "true");
            }

            return required;
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
    }
}