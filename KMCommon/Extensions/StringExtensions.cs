using System;
using System.Linq;

namespace KM.Common.Extensions
{
    public static class StringExtensions
    {
        private const string MessageExpectedParamsIsEmpty = "expectedParams is null or empty.";

        /// <summary>
        /// Checks if the current string equals (ordinal ignore case) with any one of expected parameters.
        /// </summary>
        /// <param name="current">The string to be tested.</param>
        /// <param name="expectedParams">one or more strings to compare with</param>
        /// <returns>true if a match is found; otherwise false</returns>
        public static bool EqualsAnyIgnoreCase(this string current, params string[] expectedParams)
        {
            if (expectedParams == null || expectedParams.Length == 0)
            {
                throw new ArgumentException(MessageExpectedParamsIsEmpty);
            }

            return expectedParams.Any(expected => string.Equals(current, expected, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if the current string equals (ordinal ignore case) with any one of expected parameters.
        /// </summary>
        /// <param name="current">The string to be tested.</param>
        /// <param name="expected">The strings to compare with</param>
        /// <returns>true if a match is found; otherwise false</returns>
        public static bool EqualsIgnoreCase(this string current, string expected)
        {
            return string.Equals(current, expected, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current string contains (ordinal ignore case) with any one of expected parameters.
        /// </summary>
        /// <param name="current">The string to be tested.</param>
        /// <param name="expectedParams">one or more strings to check with</param>
        /// <returns>true if a match is found; otherwise false</returns>
        public static bool ContainsAnyIgnoreCase(this string current, params string[] expectedParams)
        {
            if (expectedParams == null || expectedParams.Length == 0)
            {
                throw new ArgumentException(MessageExpectedParamsIsEmpty);
            }

            return current != null &&
                   expectedParams.Any(expected => current.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Checks if the current string contains specified sub-string.
        /// </summary>
        /// <param name="current">The string to be tested.</param>
        /// <param name="subString">The sub-string to search with</param>
        /// <param name="comparisonType">Enumeration that specify the rule for the search.</param>
        /// <returns>true if a match is found; otherwise false</returns>
        public static bool Contains(this string current, string subString, StringComparison comparisonType)
        {
            Guard.NotNull(current, nameof(current));

            return current.IndexOf(subString, comparisonType) >= 0;
        }

        /// <summary>
        /// Checks if the current string contains (ordinal ignore case) specified sub-string.
        /// </summary>
        /// <param name="current">The string to be tested.</param>
        /// <param name="subString">The sub-string to search with</param>
        /// <returns>true if a match is found; otherwise false</returns>
        public static bool ContainsIgnoreCase(this string current, string subString)
        {
            Guard.NotNull(current, nameof(current));

            return current.Contains(subString, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current string is null, empty or white space(s).
        /// </summary>
        /// <param name="current">The string to be tested.</param>
        /// <returns>true if the string is null, empty or white space(s); otherwise false</returns>
        public static bool IsNullOrWhiteSpace(this string current)
        {
            return string.IsNullOrWhiteSpace(current);
        }

        /// <summary>
        /// Returns parsed integer value
        /// </summary>
        /// <param name="current">input string</param>
        /// <returns>parsed integer value</returns>
        public static int IntTryParse(this string input)
        {
            int returnValue;

            if (!int.TryParse(input, out returnValue))
            {
                throw new InvalidCastException($"{input} cannot be parsed to integer");
            }

            return returnValue;
        }

        /// <summary>
        /// Returns parsed boolean value
        /// </summary>
        /// <param name="current">input string</param>
        /// <returns>parsed boolean value</returns>
        public static bool BoolTryParse(this string input)
        {
            bool returnValue;

            if (!bool.TryParse(input, out returnValue))
            {
                throw new InvalidCastException($"{input} cannot be parsed to boolean");
            }

            return returnValue;
        }
    }
}
