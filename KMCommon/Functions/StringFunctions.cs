using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Windows.Media;
using KM.Common.Functions;

namespace KM.Common
{
    public class StringFunctions
    {
        private const string DateFormat = "MMddyyyy_HH:mm:ss";
        private const string PhoneNumberGroupMatchingPattern = @"^(...)(...)(....)$";
        private const string PhoneNumberDashReplacement = "$1-$2-$3";
        private const string PhoneNumberMatchingPattern = @"^\d{1,20}$";
        private const string PhoneNumberWithDashesMatchingPattern = @"^\d{3}-\d{3}-\d{4}$";
        private const string NonAlphaNumericRegExMatchingPattern = @"[^a-zA-Z0-9]";
        private const string NonNumericRegExMatchingPattern = @"[^\d]";
        private const string EmailMatchingPattern = @"[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}";
        private const string AnchorMatchingPattern = "href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))";
        private const string FileInvalidCharsMatchingPattern = @"([{0}]*\.+$)|([{0}]+)";
        private const string MultipleSpacesMatchingPattern = " {2,}";
        private const string HtmlTagBr = "<br>";
        private const string HtmlTagP = "<p>";
        private const string HtmlTagSpace = "&nbsp;";
        private const string HtmlTagCopyright = "&copy;";
        private const string HtmlTagRegistered = "&reg;";
        private const string HtmlTagTrademark = "&trade;";
        private const string HtmlTagMatchingPattern = "<[^>]*>";
        private const string QuoteChar = "\"";
        private const string SingleQuoteChar = "'";
        private const string EmptySpaceChar = " ";
        private const string UnderscoreChar = "_";
        private const string SharpChar = "#";
        private const string SharpEncodedChar = "%23";
        private const char BackspaceChar = '\b';
        private const int MaxPhoneNumberLength = 10;

        public const string FriendlyServiceError = "An error has occurred with your service call.  " +
                                                   "Technical support has been notified.  If the problem is " +
                                                   "still present in 24 hours please contact Customer Support.";

        private const char DashChar = '-';
        private const int Hexadecimal = 16;
        private const int UnicodeRangeBegin = 0x10000;
        private const int UnicodeRangeEnd = 0x10FFFF;
        private const int UnicodeDivisor = 0x400;
        private const int UnicodeHighOffset = 0xD800;
        private const int UnicodeLowOffset = 0xDC00;

        private static readonly Dictionary<string, string> DirtyChars = new Dictionary<string, string>
        {
            {"’", SingleQuoteChar},
            {"–", "-"},
            {"“", QuoteChar},
            {"”", QuoteChar},
            {"…", "..."}
        };

        public static readonly IList<string> SqlExcludedChars = new List<string>
        {
            ",",
            "'",
            "%",
            "*",
            "#",
            "--",
            "&",
            "<",
            ">",
            ";",
            "xp_",
            "_",
            "/*",
            "*/"
        };

        public static string RemoveTrailingCharacter(string input, string character)
        {
            string output = string.Empty;
            if (input.Length > 0 && input.Substring(input.Length - 1, 1) == character)
            {
                output = input.Substring(0, input.Length - 1);
            }
            else
            {
                output = input;
            }
            return output;
        }

        public static string RemoveLeadingCharacter(string input, string character)
        {
            string output = string.Empty;
            if (input.Length > 0 && input.Substring(0, 1) == character)
            {
                output = input.Substring(1, input.Length - 1);
            }
            else
            {
                output = input;
            }
            return output;
        }

        public static string FormatExceptionForHTML(Exception ex)
        {
            var sbEx = new StringBuilder();
            try
            {
                sbEx.AppendLine("<br/>**********************<br/>");

                if (ex.HelpLink != null)
                {
                    sbEx.AppendLine("-- Help Link --<br/>");
                    sbEx.AppendLine(ex.HelpLink + "<br/>");
                }

                if (ex.TargetSite != null)
                {
                    sbEx.AppendLine("-- Target Site --<br/>");
                    sbEx.AppendLine("Name: " + ex.TargetSite.Name + "<br/>");
                    if (ex.TargetSite.Module != null)
                    {
                        sbEx.AppendLine("Module FullyQualifiedName: " + ex.TargetSite.Module.FullyQualifiedName + "<br/>");
                        sbEx.AppendLine("Module Name: " + ex.TargetSite.Module.Name + "<br/>");
                    }
                    if (ex.TargetSite.Module.Assembly != null)
                        sbEx.AppendLine("Module Assembly FullName: " + ex.TargetSite.Module.Assembly.FullName + "<br/>");
                }

                if (ex.Source != null)
                {
                    sbEx.AppendLine("-- Source --<br/>");
                    sbEx.AppendLine(ex.Source + "<br/>");
                }

                if (ex.Data != null)
                {
                    sbEx.AppendLine("-- Data --<br/>");
                    foreach (var d in ex.Data)
                    {
                        sbEx.AppendLine(d.ToString() + "<br/>");
                    }
                }

                if (ex.Message != null)
                {
                    sbEx.AppendLine("-- Message --<br/>");
                    sbEx.AppendLine(ex.Message + "<br/>");
                }

                if (ex.InnerException != null)
                {
                    sbEx.AppendLine("-- InnerException --<br/>");
                    sbEx.AppendLine(ex.InnerException.ToString() + "<br/>");
                }

                if (ex.StackTrace != null)
                {
                    sbEx.AppendLine("-- Stack Trace --<br/>");
                    sbEx.AppendLine(ex.StackTrace + "<br/>");
                }
                sbEx.AppendLine("**********************<br/>");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                throw;
            }

            return sbEx.ToString();
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            bool result = false;
            if (String.IsNullOrEmpty(email))
                return result;
            email = email.Trim();
            result = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z|0-9]{2,63})(\]?)$");
            return result;
        }

        /// <summary>
        /// uses double.TryParse()
        /// </summary>
        /// <param name="stringToTest"></param>
        /// <returns></returns>
        public static bool IsNumeric(string stringToTest)
        {
            double result;
            return double.TryParse(stringToTest, out result);
        }

        /// <summary>
        /// will remove ( ) - . _ from phone number and return just 10 digit number
        /// </summary>
        /// <param name="dirtyPhone"></param>
        /// <returns></returns>
        public static string PhoneNumber(string dirtyPhone)
        {
            return dirtyPhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("_", "").ToString();
        }

        /// <summary>
        /// take any string and remove anything that is NOT a letter number or space
        /// </summary>
        /// <param name="dirty"></param>
        /// <returns></returns>
        public static string LettersNumbersSpaces(string dirty)
        {
            //SAME AS BELOW string clean = Regex.Replace(q, @"[^a-zA-Z0-9\s]", string.Empty);
            string clean = Regex.Replace(dirty, @"[^\w\s]", string.Empty);
            return clean;
        }

        public static void WriteLineRepeater(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var line = new string(BackspaceChar, message.Length);

            Console.Write(line);
            WriteLine(message, color);
        }

        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        public static string GenerateProcessCode()
        {
            var randomAlphaNumericString = GetRandomAlphaNumericString(12);

            return $"{randomAlphaNumericString}_{DateTime.Now.ToString(DateFormat)}";
        }

        public static string GetRandomAlphaNumericString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return GetRandomString(length, 0);
        }

        public static string GetRandomAlphaString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var randomString = GetRandomAlphaNumericString(length);
            if (String.IsNullOrWhiteSpace(randomString))
            {
                return randomString;
            }

            randomString
                .Replace("0", "a")
                .Replace("1", "b")
                .Replace("2", "c")
                .Replace("3", "d")
                .Replace("4", "e")
                .Replace("5", "f")
                .Replace("6", "g")
                .Replace("7", "h")
                .Replace("8", "i")
                .Replace("9", "j");

            return randomString;
        }

        public static string GetRandomString(int length, int numberOfNonAlphanumericChars)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (numberOfNonAlphanumericChars < 0)
            {
                numberOfNonAlphanumericChars = 0;
            }

            return Membership.GeneratePassword(length, numberOfNonAlphanumericChars);
        }

        public static string FormatPhoneNumber(string phoneNumber, bool useDashes = false)
        {
            if (String.IsNullOrWhiteSpace(phoneNumber))
            {
                return String.Empty;
            }

            var formattedPhoneNumber = Regex.Replace(phoneNumber, NonNumericRegExMatchingPattern, String.Empty);            
            if (useDashes && formattedPhoneNumber.Length == MaxPhoneNumberLength)
            {
                formattedPhoneNumber = Regex.Replace(
                    formattedPhoneNumber,
                    PhoneNumberGroupMatchingPattern,
                    PhoneNumberDashReplacement);
            }

            var formattingPattern = useDashes ? PhoneNumberWithDashesMatchingPattern : PhoneNumberMatchingPattern;
            if (!Regex.IsMatch(formattedPhoneNumber, formattingPattern))
            {
                formattedPhoneNumber = String.Empty;
            }

            return formattedPhoneNumber;
        }

        public static string ReplaceNonAlphaNumeric(string text, string replacement)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Regex.Replace(text, NonAlphaNumericRegExMatchingPattern, replacement);
        }

        public static string RemoveNonAlphaNumeric(string text)
        {
            return ReplaceNonAlphaNumeric(text, String.Empty);
        }

        public static string CleanString(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            foreach (var dirtyChar in DirtyChars)
            {
                text = Replace(text, dirtyChar.Key, dirtyChar.Value);
            }

            return text;
        }

        public static Color GetColor(string colorString)
        {
            if (String.IsNullOrWhiteSpace(colorString))
            {
                throw new ArgumentNullException(nameof(colorString));
            }

            return (Color) ColorConverter.ConvertFromString(colorString);
        }

        public static string EscapeXmlString(string xmlString)
        {
            if (String.IsNullOrWhiteSpace(xmlString))
            {
                return xmlString;
            }

            xmlString = SecurityElement.Escape(xmlString);
            xmlString = XMLFunctions.RemoveNonAscii(xmlString);
            xmlString = XMLFunctions.CleanNonValidXMLCharacters(xmlString);

            return xmlString;
        }

        public static bool IsEmail(string email, string emailMatchingPattern = EmailMatchingPattern)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (email.All(x => x != '@') || email.Any(Char.IsWhiteSpace))
            {
                return false;
            }
            
            return Regex.IsMatch(email, emailMatchingPattern);
        }

        public static bool IsPalindrome(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var min = 0;
            var max = text.Length - 1;
            while (true)
            {
                if (min > max)
                {
                    return true;
                }

                var lowerBoundChar = text[min];
                var upperBoundChar = text[max];
                if (Char.ToUpperInvariant(lowerBoundChar) != Char.ToUpperInvariant(upperBoundChar))
                {
                    return false;
                }

                min++;
                max--;
            }
        }
        
        public static string ToTitleCase(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        public static string ToSingleSpace(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            
            return Regex.Replace(text, MultipleSpacesMatchingPattern, EmptySpaceChar);
        }

        public static string ToLower(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return text.ToLower();
        }

        public static string ToUpper(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return text.ToUpper();
        }

        public static string Reverse(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var reversedChars = text.Reverse().ToArray();
            return new string(reversedChars);
        }
        
        public static string Left(string text, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            maxLength = Math.Abs(maxLength);

            return text.Length <= maxLength ? text : text.Substring(0, maxLength);
        }
        
        public static string Right(string text, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            maxLength = Math.Abs(maxLength);
            return text.Length <= maxLength ? text : text.Substring(text.Length - maxLength, maxLength);            
        }

        public static string RemoveDoubleQuotes(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Replace(text, QuoteChar, String.Empty);
        }

        public static string RemoveSingleQuotes(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Replace(text, SingleQuoteChar, String.Empty);
        }

        public static string TrimQuotes(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return text.Trim(SingleQuoteChar.First(), QuoteChar.First());
        }
        
        public static string Remove(string text, string removeSet)
        {
            if (String.IsNullOrWhiteSpace(text) ||
                String.IsNullOrEmpty(removeSet))
            {
                return text;
            }

            var output = new StringBuilder();
            for (var j = 0; j < text.Length; j++)
            {
                var buffer = text.Substring(j, 1);
                for (var i = 0; i < removeSet.Length; i++)
                {
                    buffer = Replace(buffer, removeSet.Substring(i, 1), String.Empty);
                    if (buffer.Length == 0)
                    {
                        break;
                    }
                }
                output.Append(buffer);
            }
            return output.ToString();
        }

        public static string Replace(string text, string textToFind, string textToReplaceWith)
        {
            if (String.IsNullOrWhiteSpace(text) ||
                String.IsNullOrEmpty(textToFind))
            {
                return text;
            }

            var index = text.IndexOf(textToFind, StringComparison.OrdinalIgnoreCase);
            var newText = new StringBuilder();
            while (index != -1)
            {
                newText.Append(text.Substring(0, index));
                newText.Append(textToReplaceWith);
                text = text.Substring(index + textToFind.Length);
                index = text.IndexOf(textToFind, StringComparison.OrdinalIgnoreCase);
            }

            if (text.Length > 0)
            {
                newText.Append(text);
            }

            return newText.ToString();
        }

        public static string ReplaceAnchor(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var matches = Regex.Matches(
                text,
                AnchorMatchingPattern,
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            foreach (var match in matches)
            {
                var matchItem = match as Match;
                if (matchItem == null)
                {
                    continue;
                }

                foreach (var matchGroupItem in matchItem.Groups)
                {
                    var groupItem = matchGroupItem as Group;
                    if (groupItem?.Value.Length > 0 && 
                        groupItem.Value.IndexOf(SharpChar, StringComparison.OrdinalIgnoreCase) > 10)
                    {
                        text = text.Replace(
                            groupItem.Value,
                            groupItem.Value.Replace(
                                SharpChar,
                                SharpEncodedChar));
                    }
                }
            }

            return text;
        }

        public static string StripString(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Remove(text, GetNonAlphaCharacters());
        }

        public static string GetNonAlphaCharacters()
        {
            var thebadones = new StringBuilder();
            for (var i = 0; i < 48; i++)
            {
                thebadones.Append((char)i);
            }

            for (var i = 58; i < 65; i++)
            {
                thebadones.Append((char)i);
            }

            for (var i = 91; i < 97; i++)
            {
                thebadones.Append((char)i);
            }

            for (var i = 123; i < 128; i++)
            {
                thebadones.Append((char)i);
            }

            return thebadones.ToString();
        }

        public static string GetNonDomainCharacters()
        {
            var thebadones = new StringBuilder();
            for (var i = 0; i < 43; i++)
            {
                thebadones.Append((char)i);
            }

            thebadones.Append((char)44);
            thebadones.Append((char)47);

            for (int i = 58; i < 64; i++)
            {
                thebadones.Append((char)i);
            }

            for (int i = 91; i < 95; i++)
            {
                thebadones.Append((char)i);
            }

            thebadones.Append((char)96);
            for (int i = 123; i < 128; i++)
            {
                thebadones.Append((char)i);
            }

            return thebadones.ToString();
        }

        public static int CountChars(string source, string strToCount, bool ignoreCase = false)
        {
            if (String.IsNullOrEmpty(source) ||
                String.IsNullOrEmpty(strToCount))
            {
                return 0;
            }

            var comparer = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            int counter = 0;
            int index = source.IndexOf(strToCount, comparer);
            while (index != -1)
            {
                counter++;
                source = source.Substring(index + 1);
                index = source.IndexOf(strToCount, comparer);
            }

            return counter;
        }
        
        public static string DbTypeToString(object dbObject, string predefinedValue)
        {
            if (String.IsNullOrWhiteSpace(predefinedValue))
            {
                predefinedValue = EmptySpaceChar;
            }

            if (dbObject == null || dbObject.Equals(DBNull.Value))
            {
                return predefinedValue;
            }

            try
            {
                var convertedValue = Convert.ToString(dbObject);
                if (String.IsNullOrWhiteSpace(convertedValue))
                {
                    return predefinedValue;
                }

                return convertedValue;
            }
            // NO exception should be left to propagate further, 
            // since this should be a non-braking process (silent)
            catch 
            {
                return predefinedValue;
            }
        }

        public static string CleanHtmlString(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = text
                .Replace(HtmlTagBr, Environment.NewLine)
                .Replace(HtmlTagBr.ToUpper(), Environment.NewLine)
                .Replace(HtmlTagP, Environment.NewLine)
                .Replace(HtmlTagP.ToUpper(), Environment.NewLine)
                .Replace(HtmlTagSpace, EmptySpaceChar)
                .Replace(HtmlTagCopyright, String.Empty)
                .Replace(HtmlTagRegistered, String.Empty)
                .Replace(HtmlTagTrademark, String.Empty);

            return Regex.Replace(text, HtmlTagMatchingPattern, String.Empty);
        }

        public static string CleanSqlString(string text)
        {
            if (String.IsNullOrWhiteSpace(text) ||
                SqlExcludedChars == null)
            {
                return text;
            }
            
            foreach (var excludedChar in SqlExcludedChars)
            {
                text = text.Replace(excludedChar, String.Empty);
            }

            return text;
        }

        public static string GetNumberInWordsFormat(int number)
        {
            return new NumberConverter().GetNumberInWord(number).Trim();
        }

        public static string CleanFileName(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                return fileName;
            }

            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var escapedInvalidFileNameChars = Regex.Escape(new string(invalidFileNameChars));
            var invalidRegStr = String.Format(FileInvalidCharsMatchingPattern, escapedInvalidFileNameChars);

            return Regex.Replace(fileName, invalidRegStr, UnderscoreChar);
        }

        public static string ToUnicode(string codepoint)
        {
            if (string.IsNullOrWhiteSpace(codepoint))
            {
                throw new ArgumentNullException(nameof(codepoint));
            }

            if (codepoint.Contains(DashChar))
            {
                var pair = codepoint.Split(DashChar);
                var highsAndLows = new string[pair.Length];
                var chars = new char[pair.Length];
                for (var index = 0; index < pair.Length; index++)
                {
                    var part = Convert.ToInt32(pair[index], Hexadecimal);
                    if (part >= UnicodeRangeBegin && part <= UnicodeRangeEnd)
                    {
                        var high = Math.Floor((decimal)(part - UnicodeRangeBegin) / UnicodeDivisor) + UnicodeHighOffset;
                        var low = ((part - UnicodeRangeBegin) % UnicodeDivisor) + UnicodeLowOffset;
                        highsAndLows[index] = new string(new char[] { (char)high, (char)low });
                    }
                    else
                    {
                        chars[index] = (char)part;
                    }
                }

                return highsAndLows.Any(x => x != null) ? string.Concat(highsAndLows) : new string(chars);
            }
            else
            {
                var codePointInt = Convert.ToInt32(codepoint, Hexadecimal);
                return char.ConvertFromUtf32(codePointInt);
            }
        }

        public static string FormatException(Exception exception, string header = null)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var formattedException = new StringBuilder();
            formattedException.AppendLine("**********************");

            if (header != null)
            {
                formattedException.AppendLine(header);
            }

            if (exception.HelpLink != null)
            {
                formattedException.AppendLine("-- Help Link --");
                formattedException.AppendLine(exception.HelpLink);
            }

            if (exception.TargetSite != null)
            {
                formattedException.AppendLine("-- Target Site --");
                formattedException.AppendLine($"Name: {exception.TargetSite.Name}");
                if (exception.TargetSite.Module != null)
                {
                    formattedException.AppendLine($"Module FullyQualifiedName: {exception.TargetSite.Module.FullyQualifiedName}");
                    formattedException.AppendLine($"Module Name: {exception.TargetSite.Module.Name}");
                }

                if (exception.TargetSite.Module.Assembly != null)
                {
                    formattedException.AppendLine($"Module Assembly FullName: {exception.TargetSite.Module.Assembly.FullName}");
                }
            }

            if (exception.Source != null)
            {
                formattedException.AppendLine("-- Source --");
                formattedException.AppendLine(exception.Source);
            }

            if (exception.Data != null)
            {
                formattedException.AppendLine("-- Data --");
                foreach (var d in exception.Data)
                {
                    formattedException.AppendLine(d.ToString());
                }
            }

            if (exception.Message != null)
            {
                formattedException.AppendLine("-- Message --");
                formattedException.AppendLine(exception.Message);
            }

            if (exception.InnerException != null)
            {
                formattedException.AppendLine("-- InnerException --");
                formattedException.AppendLine(exception.InnerException.ToString());
            }

            if (exception.StackTrace != null)
            {
                formattedException.AppendLine("-- Stack Trace --");
                formattedException.AppendLine(exception.StackTrace);
            }

            formattedException.AppendLine("**********************");

            return formattedException.ToString();
        }
    }    
}
