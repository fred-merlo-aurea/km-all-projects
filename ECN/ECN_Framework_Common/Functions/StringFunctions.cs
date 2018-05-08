using System;
using System.Text;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ECN_Framework_Common.Functions
{
    public class StringFunctions
    {
        public static string ConvertToInlineCSS(StringBuilder source)
        {
            System.Text.StringBuilder sbSource = new StringBuilder();
            CssConvert_2_0.Convert c = new CssConvert_2_0.Convert();

            sbSource.Append(source);

            StringBuilder sbResult = new StringBuilder();

            return c.InlineCss(sbSource).ToString();
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Replace(string strText, string strFind, string strReplace)
        {
            return CommonStringFunctions.Replace(strText, strFind, strReplace);
        }
        
        //gets a trimmed string(even if null)
        public static string GetTrimmed(string original)
        {
            if (original != null)
                return original.Trim();
            else
                return string.Empty;
        }
        
        //checks for valid string(even if null)
        public static bool HasValue(string original)
        {
            if (GetTrimmed(original).Length > 0)
                return true;
            else
                return false;
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string CleanXMLString(string text)
        {
            return CommonStringFunctions.EscapeXmlString(text);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string CleanString(string text)
        {
            return CommonStringFunctions.CleanString(text);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Left(string text, int length)
        {
            return CommonStringFunctions.Left(text, length);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Right(string text, int length)
        {
            return CommonStringFunctions.Right(text, length);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string TrimQuotes(string text)
        {
            return CommonStringFunctions.TrimQuotes(text);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Remove(string strText, string removeSet)
        {
            return CommonStringFunctions.Remove(strText, removeSet);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string NonDomain()
        {
            return CommonStringFunctions.GetNonDomainCharacters();
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string ReplaceNonAlphaNumeric(string text, string replacement)
        {
            return CommonStringFunctions.ReplaceNonAlphaNumeric(text, replacement);
        }
    }
}

