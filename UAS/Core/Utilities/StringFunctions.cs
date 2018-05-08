using System;
using System.Text;
using System.Text.RegularExpressions;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace Core_AMS.Utilities
{
    public class StringFunctions
    {
        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string FriendlyServiceError()
        {
            return CommonStringFunctions.FriendlyServiceError;
        }

        public static string GenerateTempPassword()
        {

            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string GenerateProcessCode()
        {
            return CommonStringFunctions.GenerateProcessCode();
        }
        
        public static string CleanProcessCodeForFileName(string processCode)
        {
            char[] invalidFileChars = System.IO.Path.GetInvalidFileNameChars();
            string cleanProcessCode = processCode;
            foreach (char v in invalidFileChars)
            {
                cleanProcessCode = cleanProcessCode.Replace(v, '_');
            }

            return cleanProcessCode;
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static void WriteLineRepeater(string message, ConsoleColor color = ConsoleColor.White)
        {
            CommonStringFunctions.WriteLineRepeater(message, color);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string FormatPhoneWithDashes(string numbers)
        {
            return CommonStringFunctions.FormatPhoneNumber(numbers, true);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string FormatPhoneNumbersOnly(string numbers)
        {
            return CommonStringFunctions.FormatPhoneNumber(numbers);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string RandomAlphaString(int length)
        {
            return CommonStringFunctions.GetRandomAlphaString(length);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string RandomAlphaNumericString(int length)
        {
            return CommonStringFunctions.GetRandomAlphaNumericString(length);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string CleanXMLString(string text)
        {
            return CommonStringFunctions.EscapeXmlString(text);
        }
        
        public static string CleanDangerousFormString(string text)
        {
            text = text.Replace("&", "");
            text = text.Replace("\"", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace("%", "");
            text = text.Replace("/", "");
            text = text.Replace("*", "");
            text = text.Replace("#", "");
            text = text.Replace("?", "");
            text = text.Replace(":", "");

            return text;
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string RemoveNonAlphaNumeric(string text)
        {
            return CommonStringFunctions.RemoveNonAlphaNumeric(text);
        }
        
        public static string GetNumbersOnly(string dirtyString)
        {
            return Regex.Replace(dirtyString, "[^0-9]", string.Empty);
        }
        
        public static string Allow_Numbers_Letters_Spaces_Dashes(string dirtyString)
        {
            //[^A-Za-z0-9 -]
            return Regex.Replace(dirtyString, "[^A-Za-z0-9 -]", string.Empty);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static bool isEmail(string inputEmail, string regEx = @"[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}")
        {
            return CommonStringFunctions.IsEmail(inputEmail, regEx);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string Replace(string strText, string strFind, string strReplace)
        {
            return CommonStringFunctions.Replace(strText, strFind, strReplace);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string CleanStringSqlInjection(string dirty)
        {
            return CommonStringFunctions.CleanSqlString(dirty);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string MakeValidFileName(string name)
        {
            return CommonStringFunctions.CleanFileName(name);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string FormatException(Exception ex)
        {
            return CommonStringFunctions.FormatException(ex);
        }

        [Obsolete("This method is deprecated. Please use KM.Common.StringFunctions class.")]
        public static string FormatExceptionForHtml(Exception exception)
        {
            return CommonStringFunctions.FormatExceptionForHTML(exception);
        }
    }
}
