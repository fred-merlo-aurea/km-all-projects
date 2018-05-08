using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace ECN_Framework_Common.Functions
{
    public class RegexUtilities
    {
        private const string ValidateUrlRegex = @"(http|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?";
        public static bool IsValidEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z|0-9]{2,63})(\]?)$");
        }

        public static bool isValidPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"\d{10}");
            return regex.IsMatch(phoneNumber);
        }

        public static bool IsValidObjectName(string objectName)
        {
            Regex regex = new Regex(@"^[\u0020-\u0021\u0026-\u002A\u002D-\u003A\u003F-\u007A\u007C\u007E]*$");
            if (objectName == null) return false;
            return regex.IsMatch(objectName);
        }

        public static string IsValidEmailSubject(string emailSubject)
        {
            string retString = "";

            Regex lineFeedRegex = new Regex("\r?\n|\r", RegexOptions.Singleline);
            if (lineFeedRegex.IsMatch(emailSubject))
                retString += "line feed(invisible character),";

            Regex htmlElements = new Regex("<[\\s\\S]*?>", RegexOptions.Singleline);
            if(htmlElements.IsMatch(emailSubject))
                retString += "html elements(invisible characters)";

            return retString;
        }

        public static bool IsAplhaNumeric(string input)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            return regex.IsMatch(input);
        }

        public static bool IsValidUDFName(string UDFName)
        {
            int result = 0;
            if(int.TryParse(UDFName.Substring(0,1), out result).Equals(true))
            {
                return false;
            }

            Regex regex = new Regex(@"^[0-9a-zA-Z&_-]*$");
            return regex.IsMatch(UDFName);
        }

        public static bool IsValidPassword(string Password)
        {
            //One upper case letter, one lower case letter, one number 
            Regex regex = new Regex(@"^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?!.*\s).*$");
            bool check1=  regex.IsMatch(Password);
            //Check if alphanumeric
            bool check2 = IsAplhaNumeric(Password);
            return check1 && check2;
        }

        public static bool ContainsScriptTags(string content)
        {
            Regex regex = new Regex("<script.*?</script>", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            bool check = regex.IsMatch(content);
            return check;
        }

        public static string GetCleanUrlContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException("content");
            }
            string cont = string.Empty;
            string[] arr = content.Split(new char[] { '\r' });
            for (int count = 0; count < arr.Length; count++)
            {
                if (Regex.Matches(arr[count], "%23").Count == 1)
                {
                    string url = Regex.Match(arr[count], ValidateUrlRegex, RegexOptions.IgnoreCase).Value;
                    if (url.EndsWith("%23"))
                    {
                        arr[count] = arr[count].Replace("%23", "#");
                    }
                }
            }
            cont = String.Join("\r", arr);
            return cont;
        }
    }
}
