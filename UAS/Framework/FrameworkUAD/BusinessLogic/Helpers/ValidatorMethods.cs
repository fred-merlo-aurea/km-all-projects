using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Functions;
using ImportErrorEntity = FrameworkUAD.Entity.ImportError;
using ImportFileObject = FrameworkUAD.Object.ImportFile;
using ObjectFunctions = Core_AMS.Utilities.ObjectFunctions;


namespace FrameworkUAD.BusinessLogic.Helpers
{
    public static class ValidatorMethods
    {
        private const char CommaJoin = ',';
        private const string CommaSeparator = ",";
        private const string OriginalImportrowKey = "originalimportrow";

        public static string SetGeneratedValue(
            IEnumerable<AdHocDimension> adHocList,
            int sourceInt,
            string sourceString,
            string generatedValue,
            bool isTypeAnd)
        {
            Guard.NotNull(adHocList, nameof(adHocList));

            if (string.IsNullOrWhiteSpace(sourceString))
            {
                throw new ArgumentNullException(nameof(sourceString));
            }

            foreach (var adHoc in adHocList)
            {
                int match;
                int.TryParse(adHoc.MatchValue, out match);
                switch (adHoc.Operator)
                {
                    case "greater_than":
                        generatedValue = sourceInt > match ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "less_than":
                        generatedValue = sourceInt < match ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "greater_than_or_equal_to":
                        generatedValue = sourceInt >= match ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "less_than_or_equal_to":
                        generatedValue = sourceInt <= match ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "is_not_less_than":
                        generatedValue = !(sourceInt < match) ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "is_not_greater_than":
                        generatedValue = !(sourceInt > match) ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "equal":
                        generatedValue = SetGenerateValueWhenOperatorIsEqual(adHoc, sourceInt, sourceString, isTypeAnd, match);
                        break;
                    case "not_equal":
                        generatedValue = SetGenerateValueWhenOperatorIsNotEqual(adHoc, sourceInt, sourceString, isTypeAnd, match);
                        break;
                    case "contains":
                        generatedValue = sourceString.IndexOf(adHoc.MatchValue, StringComparison.OrdinalIgnoreCase) >= 0 ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "starts_with":
                        generatedValue = sourceString.ToLower().StartsWith(adHoc.MatchValue.ToLower()) ? adHoc.DimensionValue : string.Empty;
                        break;
                    case "ends_with":
                        generatedValue = sourceString.ToLower().EndsWith(adHoc.MatchValue.ToLower()) ? adHoc.DimensionValue : string.Empty;
                        break;
                }
            }
            return generatedValue;
        }

        private static string SetGenerateValueWhenOperatorIsEqual(AdHocDimension adHoc, int sourceInt, string sourceString, bool isTypeAnd, int match)
        {
            string generatedValue;
            if (isTypeAnd)
            {
                generatedValue = sourceInt == match &&
                                 sourceString.Equals(adHoc.MatchValue, StringComparison.CurrentCultureIgnoreCase)
                    ? adHoc.DimensionValue
                    : string.Empty;
            }
            else
            {
                generatedValue = sourceInt == match ||
                                 sourceString.Equals(adHoc.MatchValue, StringComparison.CurrentCultureIgnoreCase)
                    ? adHoc.DimensionValue
                    : string.Empty;
            }
            return generatedValue;
        }

        private static string SetGenerateValueWhenOperatorIsNotEqual(AdHocDimension adHoc, int sourceInt, string sourceString, bool isTypeAnd, int match)
        {
            string generatedValue;
            if (isTypeAnd)
            {
                generatedValue = sourceInt != match &&
                                 !sourceString.Equals(adHoc.MatchValue, StringComparison.CurrentCultureIgnoreCase)
                    ? adHoc.DimensionValue
                    : string.Empty;
            }
            else
            {
                generatedValue = sourceInt != match ||
                                 !sourceString.Equals(adHoc.MatchValue, StringComparison.CurrentCultureIgnoreCase)
                    ? adHoc.DimensionValue
                    : string.Empty;
            }
            return generatedValue;
        }

        public static bool TryParseDateColumnHeader(StringDictionary row, string qDateFormat, string qDateColumnHeader,
            string qDatePattern2, string qDateValue)
        {
            Guard.NotNull(row, nameof(row));

            var defaultDateTime = new DateTime();
            var qDateConvertValue = defaultDateTime;
            DateTime.TryParseExact(qDateValue, qDatePattern2, null, DateTimeStyles.None, out qDateConvertValue);

            if (qDateConvertValue == defaultDateTime)
            {
                DateTime.TryParse(row[qDateColumnHeader], out qDateConvertValue);
                if (qDateConvertValue == defaultDateTime)
                {
                    var dateParsingFailure = true;
                    DateFormat formattedDate;
                    Enum.TryParse(qDateFormat, out formattedDate);
                    qDateConvertValue = DateTimeFunctions.ParseDate(formattedDate, qDateValue);

                    if (qDateConvertValue < defaultDateTime)
                    {
                        dateParsingFailure = false;
                        row[qDateColumnHeader] = qDateConvertValue.ToShortDateString();
                    }

                    return dateParsingFailure;
                }
            }

            row[qDateColumnHeader] = qDateConvertValue.ToShortDateString();
            return false;
        }

        public static void ResetKeyValuesOnTransformedDictionary(ImportFileObject importFile, out Dictionary<int, int> keyRest, out int newKey)
        {
            Guard.NotNull(importFile, nameof(importFile));
            keyRest = new Dictionary<int, int>();
            newKey = 1;

            foreach (var key in importFile.DataTransformed.Keys)
            {
                keyRest.Add(key, newKey);
                newKey++;
            }
            foreach (var row in keyRest)
            {
                ObjectFunctions.ChangeKey(importFile.DataTransformed, row.Key, row.Value);
            }
        }

        public static ImportErrorEntity CreateImportErrorEntity(
            ImportFileObject importFile,
            StringDictionary rowData,
            int rowNumber,
            string errorMsg,
            bool insertSeparator)
        {
            Guard.NotNull(importFile, nameof(importFile));

            var importError = new ImportErrorEntity();
            var sbBadData = new StringBuilder();

            if (rowData != null)
            {
                var originalHeaders = new string[importFile.BadDataOriginalHeaders.Count];
                importFile.BadDataOriginalHeaders.Keys.CopyTo(originalHeaders, 0);

                foreach (var header in originalHeaders.OrderBy(x => x))
                {
                    if (header.Equals(OriginalImportrowKey, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (importFile.BadDataOriginalHeaders.ContainsKey(header))
                    {
                        sbBadData.Append(rowData.ContainsKey(header) 
                            ? $"{rowData[header]}{CommaSeparator}" 
                            : CommaSeparator);
                    }
                }
            }

            importError.BadDataRow = insertSeparator ? sbBadData.ToString().Trim().TrimEnd(CommaJoin) : sbBadData.ToString().Trim();
            importError.ClientMessage = errorMsg;
            importError.RowNumber = rowNumber;
            return importError;
        }

        public static int SetImportErrorsValue(ImportFileObject importFile, ImportErrorEntity importErrorEntity)
        {
            Guard.NotNull(importFile, nameof(importFile));
            Guard.NotNull(importErrorEntity, nameof(importErrorEntity));

            if (importFile.ImportErrors == null)
            {
                importFile.ImportErrors = new HashSet<ImportErrorEntity>();
            }

            importFile.HasError = true;
            importFile.ImportErrorCount++;
            importFile.ImportErrors.Add(importErrorEntity);

            return importFile.DataTransformed.Count > 0
                ? (int)(importFile.ImportErrorCount / (decimal)importFile.DataTransformed.Count * 100)
                : 0;
        }
    }
}
