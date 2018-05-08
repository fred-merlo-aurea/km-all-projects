using System;
using Core_AMS.Utilities;

namespace FrameworkUAD.BusinessLogic.Helpers
{
    internal class ZipCodeMethodsHelper
    {
        private const int ValidUsaZipLength = 5;
        private const string InvalidUsaZipPrefix = "000";
        private const string ZipSeparatorUsa = "-";
        private const string ZipCodePadding = "0";
        private const int ZipPlus4MaxLength = 4;

        public static FillZipCodeArgs ExecuteUsaFormatting(string zipCode, string plus4Code)
        {
            const int maxTotalLength = ValidUsaZipLength + ZipPlus4MaxLength;
            const int zipMinLength = ValidUsaZipLength - 1;

            if (zipCode.Length == ValidUsaZipLength && zipCode.StartsWith(InvalidUsaZipPrefix))
            {
                zipCode = string.Empty;
            }

            if (zipCode.Length >= zipMinLength)
            {
                if (zipCode.Length == zipMinLength)
                {
                    zipCode = $"{ZipCodePadding}{zipCode}";
                }
                else if (zipCode.Length >= ValidUsaZipLength)
                {
                    var zipOriginal = zipCode.Replace(ZipSeparatorUsa, string.Empty);
                    if (zipCode.Length >= ValidUsaZipLength && zipCode.Length <= maxTotalLength)
                    {
                        zipCode = zipOriginal.Substring(0, ValidUsaZipLength);
                        plus4Code = string.Empty;
                    }

                    if (zipOriginal.Length == maxTotalLength)
                    {
                        plus4Code = zipOriginal.Substring(ValidUsaZipLength, ZipPlus4MaxLength);
                    }
                }
            }
            else
            {
                zipCode = string.Empty;
            }

            if (plus4Code.Length > ZipPlus4MaxLength)
            {
                plus4Code = string.Empty;
            }

            var zipCheck = StringFunctions.GetNumbersOnly(zipCode);
            if (zipCheck.Length < ValidUsaZipLength)
            {
                zipCode = string.Empty;
            }

            return new FillZipCodeArgs {ZipCode = zipCode, Plus4Code = plus4Code};
        }
    }
}
