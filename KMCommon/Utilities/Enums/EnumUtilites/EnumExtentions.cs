using KM.Common.Utilities.Enums.EnumAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KM.Common.Utilities.Enums.EnumUtilites
{
    public static class EnumExtentions
    {
        public static string Description(this Enum value)
        {
            string descriptionAttributeValue = string.Empty;

            DescriptionAttribute attribute = GetCustomAttribute<DescriptionAttribute>(value);

            if (attribute != null)
            {
                descriptionAttributeValue = attribute.Description;
            }

            return descriptionAttributeValue;
        }

        public static string CodeStringValue(this Enum value)
        {
            string codeStringValue = string.Empty;

            CodeValueAttribute attribute = GetCustomAttribute<CodeValueAttribute>(value);

            if (attribute != null)
            {
                codeStringValue = attribute.CodeStringValue;
            }

            return codeStringValue;
        }

        public static char CodeCharValue(this Enum value)
        {
            char codeCharValue = ' ';

            CodeValueAttribute attribute = GetCustomAttribute<CodeValueAttribute>(value);

            if (attribute != null)
            {
                codeCharValue = attribute.CodeCharValue;
            }

            return codeCharValue;
        }

        public static string ColumnStringValue(this Enum value)
        {
            string columnStringValue = string.Empty;

            ColumnValueAttribute attribute = GetCustomAttribute<ColumnValueAttribute>(value);

            if (attribute != null)
            {
                columnStringValue = attribute.ColumnStringValue;
            }

            return columnStringValue;
        }

        private static T GetCustomAttribute<T>(Enum value) where T : Attribute
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            T attribute = null;

            if (!string.IsNullOrEmpty(name))
            {
                FieldInfo field = type.GetField(name);

                if (field != null)
                {
                    attribute = Attribute.GetCustomAttribute(field, typeof(T)) as T;
                }
            }

            return attribute;
        }
    }
}
