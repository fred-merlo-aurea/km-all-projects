using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KM.Common.Utilities.Csv
{
    public static class CsvUtility
    {
        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };

        public static string Escape(string s)
        {
            if (s.Contains(QUOTE))
            {
                s = s.Replace(QUOTE, ESCAPED_QUOTE);
            }

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
            {
                s = QUOTE + s + QUOTE;
            }

            return s;
        }

        public static string Unescape(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                {
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
                }
            }

            return s;
        }

        public static string ConvertToCsv<T>(List<T> listToConvert)
        {
            string returnString = string.Empty;

            if (!typeof(T).IsPrimitive && !(typeof(T) == typeof(String)))
            {
                StringBuilder stringBuilder = new StringBuilder(1024);
                PropertyInfo[] propertyArray = typeof(T).GetProperties();

                stringBuilder.AppendLine(string.Join<string>(",", propertyArray.Select(x => x.Name)));

                foreach (var item in listToConvert)
                {
                    stringBuilder.AppendLine(string.Join<string>(",", propertyArray.Select(x => x.GetValue(item, null) != null ? x.GetValue(item, null).ToString() : string.Empty)));
                }

                returnString = stringBuilder.ToString();
            }
            else
            {
                returnString = string.Join<T>(",", listToConvert);
            }

            return returnString;
        }
    }
}
