namespace KM.Common.Functions
{
    internal class DateFormatParsingInfo
    {
        private const string DayIdentifier = "d";
        private const string DayDoubleFormatIdentifier = "dd";
        private const string MonthIdentifier = "m";
        private const string MonthDoubleFormatIdentifier = "mm";
        private const string YearIdentifier = "y";

        public int DayStartIndex { get; private set; }

        public int DayEndIndex { get; private set; }

        public int DayLength { get; private set; }

        public int MonthStartIndex { get; private set; }

        public int MonthEndIndex { get; private set; }

        public int MonthLength { get; private set; }

        public int YearStartIndex { get; private set; }

        public int YearEndIndex { get; private set; }

        public int YearLength { get; private set; }

        public int FirstDayOfMonth => 1;

        public int FirstMonthOfYear => 1;

        public int MaxValidDateLength => DayLength + MonthLength + YearLength;

        public bool DayWithSingleFormatHasTwoDigits { get; private set; }

        public bool MonthWithSingleFormatHasTwoDigits { get; private set; }

        public DateFormatParsingInfo(string dateFormatString, int dateLength)
        {
            CalculateIndexes(dateFormatString);
            AdjustIndexes(dateFormatString, dateLength);
        }

        private void CalculateIndexes(string dateFormatString)
        {
            DayStartIndex = dateFormatString.IndexOf(DayIdentifier);
            DayEndIndex = dateFormatString.LastIndexOf(DayIdentifier);
            DayLength = DayEndIndex - DayStartIndex + 1;

            MonthStartIndex = dateFormatString.IndexOf(MonthIdentifier);
            MonthEndIndex = dateFormatString.LastIndexOf(MonthIdentifier);
            MonthLength = MonthEndIndex - MonthStartIndex + 1;

            YearStartIndex = dateFormatString.IndexOf(YearIdentifier);
            YearEndIndex = dateFormatString.LastIndexOf(YearIdentifier);
            YearLength = YearEndIndex - YearStartIndex + 1;
        }

        private void AdjustIndexes(string dateFormatString, int dateLength)
        {
            var maxValidDateLength = MaxValidDateLength;
            var dayIsInSingleFormat = false;
            var monthIsInSingleFormat = false;
            if (DayLength == 1)
            {
                maxValidDateLength++;
                dayIsInSingleFormat = true;
            }

            if (MonthLength == 1)
            {
                maxValidDateLength++;
                monthIsInSingleFormat = true;
            }

            if (dateFormatString.Length % 2 != 0 && dateLength != dateFormatString.Length)
            {
                if (dayIsInSingleFormat && monthIsInSingleFormat && maxValidDateLength - dateLength == 0)
                {
                    DayWithSingleFormatHasTwoDigits = true;
                    MonthWithSingleFormatHasTwoDigits = true;
                    dateFormatString = dateFormatString
                        .Replace(MonthIdentifier, MonthDoubleFormatIdentifier)
                        .Replace(DayIdentifier, DayDoubleFormatIdentifier);
                    CalculateIndexes(dateFormatString);
                }
                else if (dayIsInSingleFormat && !monthIsInSingleFormat && maxValidDateLength - dateLength == 0)
                {
                    DayWithSingleFormatHasTwoDigits = true;
                    dateFormatString = dateFormatString.Replace(DayIdentifier, DayDoubleFormatIdentifier);
                    CalculateIndexes(dateFormatString);
                }
                else if (!dayIsInSingleFormat && monthIsInSingleFormat && maxValidDateLength - dateLength == 0)
                {
                    MonthWithSingleFormatHasTwoDigits = false;
                    dateFormatString = dateFormatString.Replace(MonthIdentifier, MonthDoubleFormatIdentifier);
                    CalculateIndexes(dateFormatString);
                }
            }
        }
    }
}
