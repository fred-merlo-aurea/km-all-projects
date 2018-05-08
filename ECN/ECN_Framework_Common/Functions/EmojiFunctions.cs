using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ECN_Framework_Common.Functions
{
    public class EmojiFunctions
    {
        private static readonly Regex UnicodeSplit = new Regex("([\\\\][u][d0][08-9a-bA-B][0-9a-fA-F][0-9a-fA-F][\\\\][u][d0-9][0-9c-fC-F][a-fA-F0-9][a-fA-F0-9])", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly string EmojiPattern = "([\\\\][u][2-3][0-7a-bA-B][0-9a-fA-F][0-9a-fA-F])";
        private static readonly string UTFPattern = "([\\\\][u][02-3e][0-7a-bA-B][0-9a-fA-F][0-9a-fA-F])";
        private const string Question = "?";

        public static string GetSubjectUTF(string dynamicSubject)
        {
            var dynamicTemp = String.Format(@"{0}", dynamicSubject);
            var nonSurrogate = new Regex(UTFPattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var matches = UnicodeSplit.Matches(dynamicTemp);
            var nonSMatches = nonSurrogate.Matches(dynamicTemp);

            dynamicSubject = ReplaceUTF(matches, dynamicSubject);
            dynamicSubject = ReplaceUTF(nonSMatches, dynamicSubject);

            return dynamicSubject.ToString();
        }

        private static string ReplaceUTF(MatchCollection matchCollection, string dynamicSubject)
        {
            foreach (Match match in matchCollection)
            {
                var uniValue = match.Groups[1].Value;
                uniValue = uniValue.Replace("\\u", "");
                var chars = new char[2];
                var charIndex = 0;
                var hex = string.Empty;

                for (var i = 0; i < uniValue.Length; i += 4)
                {
                    chars[charIndex] = (char)Int16.Parse(uniValue.Substring(i, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    charIndex++;
                }

                if (!char.IsSurrogatePair(chars[0], chars[1]))
                {
                    var toHex = uniValue.Substring(0, 4);
                    if (uniValue.Length > 4)
                    {
                        toHex += "-" + uniValue.Substring(4);
                    }

                    hex = CommonStringFunctions.ToUnicode(toHex);
                }
                else
                {
                    hex = char.ConvertFromUtf32(Char.ConvertToUtf32(chars[0], chars[1]));
                }

                dynamicSubject = dynamicSubject.Replace(match.Value, hex.ToString());
            }

            return dynamicSubject;
        }

        public static string ReplaceEmojiWithQuestion(string dynamicSubject)
        {
            var dynamicTemp = String.Format(@"{0}", dynamicSubject);
            var nonSurrogate = new Regex(EmojiPattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var matches = UnicodeSplit.Matches(dynamicTemp);
            var nonSMatches = nonSurrogate.Matches(dynamicTemp);

            dynamicSubject = ReplaceMatches(matches, dynamicSubject, Question);
            dynamicSubject = ReplaceMatches(nonSMatches, dynamicSubject, Question);

            return dynamicSubject.ToString();
        }

        private static string ReplaceMatches(MatchCollection matchCollection, string dynamicSubject, string replaceChar)
        {
            foreach (Match match in matchCollection)
            {
                dynamicSubject = dynamicSubject.Replace(match.Value, replaceChar);
            }

            return dynamicSubject;
        }

        public static string GetDecimalCodePoint(string character)
        {

            UTF32Encoding encoding = new UTF32Encoding();
            byte[] bytes = encoding.GetBytes(character.ToCharArray());
            return BitConverter.ToInt32(bytes, 0).ToString();

        }

        public static string GetUnicode(string character)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in character)
            {
                sb.Append("\\u");
                sb.Append(String.Format("{0:x4}", (int)c));
            }
            return sb.ToString();
        }
    }
}