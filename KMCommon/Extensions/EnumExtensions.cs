using System;
using System.Linq;

namespace KM.Common.Extensions
{
    public static class EnumExtensions
    {
        private const string MessageExpectedParamsIsEmpty = "expectedParams is null or empty.";

        /// <summary>
        /// Checks if the current Enum equals with any one of expected parameters.
        /// </summary>
        /// <param name="current">The Enum to be tested.</param>
        /// <param name="expectedParams">one or more Enum values to compare with</param>
        /// <returns>true if a match is found; otherwise false</returns>
        public static bool IsAny<TEnum>(this TEnum current, params TEnum[] expectedParams) where TEnum : struct
        {
            if (!(current is Enum))
            {
                throw new ArgumentException("Only Enum types are allowed.");
            }

            if (expectedParams == null || expectedParams.Length == 0)
            {
                throw new ArgumentException(MessageExpectedParamsIsEmpty);
            }

            return expectedParams.Any(expected => current.Equals(expected));
        }
    }
}
