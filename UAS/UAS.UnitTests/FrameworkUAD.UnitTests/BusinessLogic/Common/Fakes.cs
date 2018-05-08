using System;
using System.Data;
using System.Data.Fakes;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace FrameworkUAD.UnitTests.BusinessLogic.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        protected const string CountryUnitedStates = "UNITED STATES";
        protected const string CountryIdKey = "CountryID";
        protected const string CountryKey = "Country";
        protected const string Plus4Key = "Plus4";
        protected const string ZipCodeKey = "ZipCode";
        private const int ValidUsaZipLength = 5;
        private const string InvalidUsaZipPrefix = "000";
        private const string ZipSeparatorUsa = "-";
        private const string ZipCodePadding = "0";
        private const int ZipPlus4MaxLength = 4;
        protected DataTable DataTable;
        private IDisposable _context;

        public void SetupFakes()
        {
            DataTable = new DataTable();
            _context = ShimsContext.Create();
            ShimDataTable.AllInstances.AcceptChanges = _ => {};
        }

        [TearDown]
        public void CleanUp() => _context?.Dispose();

        protected string GetValueByKeyFromDatable(DataTable dataTable, int column, string key)
        {
            return dataTable.Rows[column][key].ToString();
        }

        protected void SetDataTableColumns(int countryId, string countryName, string zipCode)
        {
            DataTable.Columns.Add(CountryIdKey, typeof(int));
            DataTable.Columns.Add(CountryKey, typeof(string));
            DataTable.Columns.Add(ZipCodeKey, typeof(string));
            DataTable.Columns.Add(Plus4Key, typeof(string));

            var dataRow = DataTable.NewRow();
            dataRow[CountryIdKey] = countryId;
            dataRow[CountryKey] = countryName;
            dataRow[ZipCodeKey] = zipCode;
            dataRow[Plus4Key] = string.Empty;
            DataTable.Rows.Add(dataRow);
        }

        protected void VerifyZipCodeTransformation(string zipCode, string plus4Code, out string zipCodeOut, out string plus4CodeOut)
        {
            const int maxTotalLength = ValidUsaZipLength + ZipPlus4MaxLength;
            const int zipMinLength = ValidUsaZipLength - 1;

            if (zipCode.Length == ValidUsaZipLength && zipCode.StartsWith(InvalidUsaZipPrefix))
            {
                zipCode = string.Empty;
            }

            if (zipCode.Length < zipMinLength)
            {
                zipCode = string.Empty;
            }
            else if (zipCode.Length == zipMinLength)
            {
                zipCode = ZipCodePadding + zipCode;
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

            if (plus4Code.Length > ZipPlus4MaxLength)
            {
                plus4Code = string.Empty;
            }

            var zipCheck = Core_AMS.Utilities.StringFunctions.GetNumbersOnly(zipCode);
            if (zipCheck.Length < ValidUsaZipLength)
            {
                zipCode = string.Empty;
            }

            zipCodeOut = zipCode;
            plus4CodeOut = plus4Code;
        }
    }
}
