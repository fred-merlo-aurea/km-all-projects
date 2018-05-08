using System;
using System.Linq;
using System.Text;
using KM.Common;
using ServiceStack.Text;

namespace ECN_Framework_Common.Functions
{
    public class JsonFunctions
    {
        private const char Separator = ':';
        private const string NullValue = "null";

        public static string ToJson<T>(T x)
        {
            string jsonString = JsonSerializer.SerializeToString<T>(x);
            return jsonString;
        }
        public static T FromJson<T>(string jsonString)
        {
            var fromJson = JsonSerializer.DeserializeFromString<T>(jsonString);
            return fromJson;
        }

        public static string[] CleanJsonText(string jsonText)
        {
            var clean = jsonText.Trim();
            clean = clean.Replace("\"", string.Empty);
            clean = clean.Replace(",", string.Empty).Trim();
            var parse = clean.Split(Separator);
            return parse;
        }

        public static string GetPropertyName(string[] parsedJson)
        {
            Guard.NotNull(parsedJson, nameof(parsedJson));
            Guard.For(() => parsedJson.Length == 0, () => new ArgumentOutOfRangeException(nameof(parsedJson)));

            return parsedJson[0];
        }

        public static string GetPropertyValue(string[] parsedJson)
        {
            Guard.NotNull(parsedJson, nameof(parsedJson));

            var valueBuilder = new StringBuilder();
            for (var i = 1; i < parsedJson.Length; i++)
            {
                valueBuilder.Append(parsedJson[i]);
                valueBuilder.Append(Separator);
            }
            if (valueBuilder.Length > 0)
            {
                valueBuilder.Remove(valueBuilder.Length - 1, 1);
            }

            return valueBuilder.ToString();
        }

        public static DateTime GetDateFromJson(string jsonText)
        {
            jsonText = $" {Separator}{jsonText}";
            return GetDateFromJson(jsonText.Split(Separator));
        }

        public static DateTime GetDateFromJson(string[] parsedJson)
        {
            if (!parsedJson.Any() || parsedJson.Length < 2)
            {
                throw new ArgumentNullException("parsedJson must contain at least 2 elements");
            }

            var date = string.Empty;
            try
            {
                date = string.Format("{0}:{1}: {2}",
                        parsedJson[1].Trim(),
                        parsedJson[2].Trim(),
                        parsedJson[3].Trim().ToString().Replace(".000+0000", string.Empty));
            }
            catch
            {
                date = parsedJson[1].Trim().Equals(NullValue)
                        ? "1/1/1900"
                        : parsedJson[1].Trim();
            }
            var dateTime = DateTime.Now;
            DateTime.TryParse(date, out dateTime);
            return dateTime;
        }

        public static bool GetBooleanValue(string jsonText)
        {
            return jsonText.Trim().Equals(NullValue)
                    ? false
                    : Convert.ToBoolean(jsonText.Trim().ToString());
        }

        public static double GetDoubleValue(string jsonText)
        {
            return jsonText.Trim().Equals(NullValue, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(jsonText)
                    ? 0.0
                    : Convert.ToDouble(jsonText.Trim().ToString());
        }

        public static string GetStringValue(string jsonText)
        {
            return jsonText.Trim().ToString() == NullValue
                    ? string.Empty
                    : jsonText.Trim();
        }

        public static string GetNormalizedString(string jsonText)
        {
            return jsonText.Trim().ToString() == NullValue
                    ? string.Empty
                    : jsonText.Trim().Replace("\\r", " ").Replace("\\n", " ").Replace("\"", " ");
        }

        public static int GetIntValue(string jsonText)
        {
            return Convert.ToInt32(jsonText.Trim());
        }
    }
}
