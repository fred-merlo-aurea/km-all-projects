using System;
using System.Text;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class FilterBase
    {
        private const string FullDatePart = "full";
        private const string YearDatePart = "year";
        private const string MonthDatePart = "month";
        private const string DayDatePart = "day";
        protected const string GreaterThanComparator = "greater than";
        protected const string LessThanComparator = "less than";
        protected const string CommaString = ",";
        protected const char CommaCharacter = ',';
        protected const string EqualsComparator = "equals";
        protected const string IsInComparator = "is in";

        public static string[] CommonFiltersFields => new string[]
        {
            "EmailAddress",
            "FormatTypeCode",
            "SubscribeTypeCode",
            "Title",
            "FirstName",
            "LastName",
            "FullName",
            "Company",
            "Occupation",
            "Address",
            "Address2",
            "City",
            "State",
            "Zip",
            "Country",
            "Voice",
            "Mobile",
            "Fax",
            "Website",
            "Age",
            "Income",
            "Gender",
            "UserEvent1",
            "UserEvent2",
            "Notes"
        };

        public static string[] NonCommonFiltersFields => new string[]
        {
            "Birthdate",
            "UserEvent1Date",
            "UserEvent2Date",
            "CreatedOn",
            "LastChanged",
        };

        protected static string GetDateFieldBasedOnDatePart(string datePart, string fullDateField, string nonFullDateField)
        {
            switch (datePart)
            {
                case FullDatePart:
                    return fullDateField;
                case MonthDatePart:
                case DayDatePart:
                case YearDatePart:
                    return nonFullDateField;
                default:
                    return string.Empty;
            }
        }

        protected static string GetDateField(
            string datePart,
            string comparator,
            string nonEqualAndFullDateField,
            string nonEqualAndNonFullDateField,
            string equalAndFullDateField,
            string equalAndNonFullDateField)
        {
            if (comparator == LessThanComparator || comparator == GreaterThanComparator)
            {
                return GetDateFieldBasedOnDatePart(datePart, nonEqualAndFullDateField, nonEqualAndNonFullDateField);
            }
            else
            {
                return GetDateFieldBasedOnDatePart(datePart, equalAndFullDateField, equalAndNonFullDateField);
            }
        }

        protected static string GetIsInCompareValue(string compareValue, string isInFormat)
        {
            if (string.IsNullOrWhiteSpace(compareValue))
            {
                throw new ArgumentNullException(nameof(compareValue));
            }

            var newValue = new StringBuilder();
            var first = true;
            foreach (var splittedValue in compareValue.Split(CommaCharacter))
            {
                if (!first)
                {
                    newValue.Append(CommaString);
                }
                first = false;
                newValue.AppendFormat(isInFormat, splittedValue);
            }

            return newValue.ToString();
        }

        protected static string GetNumberAndMoneyConvertValue(string comparator, string compareValue, string isInFormat)
        {
            switch (comparator)
            {
                case IsInComparator:
                    return GetIsInCompareValue(compareValue, isInFormat);
                case GreaterThanComparator:
                case LessThanComparator:
                case EqualsComparator:
                    return string.Format(isInFormat, compareValue);
            }

            return string.Empty;
        }

        protected static string GetDays(string compareValue)
        {
            var indexOfSquarBracketOpenning = compareValue.IndexOf("[");
            return compareValue.Substring(indexOfSquarBracketOpenning + 1, compareValue.IndexOf("]") - (indexOfSquarBracketOpenning + 1));
        }
    }
}
