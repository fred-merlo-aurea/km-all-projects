using System;
using System.Text.RegularExpressions;

namespace Core.Utilities
{
    public class RegexUtilities
    {
        public static bool IsValidEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool isValidPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"\d{10}");
            return regex.IsMatch(phoneNumber);
        }
    }
}
