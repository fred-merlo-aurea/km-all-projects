using ecn.communicator.classes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    public partial class FilterTest
    {
        private const string ConvertValueMethodName = "ConvertValue";
        private const string DummyCompareValue = "CompareValue";
        private const string EqualsComparator = "equals";
        private const string FullDatePart = "full";
        private const string DayDatePart = "day";
        private const string MonthDatePart = "month";
        private const string YearDatePart = "year";
        private const string InvalidDatePart = "invalid";
        private const string LessThanComparator = "less than";
        private const string DateFieldType = "Date";
        private const string InvalidComparator = "invalid";
        private const string DayOfWeekSunday = "Sunday";
        private const string MultipleCompareValues = "CompareValue,CompareValue,CompareValue";
        private const string IsInComparator = "is in";
        private const string GreaterThanComparator = "greater than";
        private const string NumberFieldType = "Number";
        private const string MoneyFieldType = "Money";
        private const string StringFieldType = "String";
        private const string ContainsComparator = "contains";

        private static string[] NonFullDateParts => new string[]
        {
            DayDatePart,
            MonthDatePart,
            YearDatePart
        };

        private static string[] ComparisonComparators => new string[]
        {
            GreaterThanComparator,
            LessThanComparator,
            EqualsComparator
        };

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_CommonSwitchCaseConstantsConditionField_ReturnsCompareValue(string switchCaseConstant)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = switchCaseConstant,
                Comparator = EqualsComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"'{DummyCompareValue}'"));
        }

        [Test]
        public void ConvertValue_StringConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = StringFieldType,
                FieldType = StringFieldType,
                Comparator = EqualsComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR, '{ DummyCompareValue }')"));
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var days = DayOfWeekSunday;
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(LessThanComparator, FullDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"DATEADD(dd, { days }, GETDATE())"));
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithInvalidDatePartAndExpTodaySquarBracket_ReturnsEmptyString()
        {
            // Arrange
            var days = DayOfWeekSunday;
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(LessThanComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_LessThanComparatorWithNonFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var days = DayOfWeekSunday;
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(LessThanComparator, datePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"{ datePart }(DATEADD(dd, { days }, GETDATE()))"));
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithFullDatePartAndExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(LessThanComparator, FullDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe("GETDATE()"));
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithInvalidDatePartAndExpToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(LessThanComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_LessThanComparatorWithNonFullDatePartAndExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(LessThanComparator, datePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"{ datePart }(GETDATE())"));
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(LessThanComparator, FullDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(DATETIME, '" + compareValue + "')"));
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithInvalidDatePartAndNotExpToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(LessThanComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_LessThanComparatorWithNonFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(LessThanComparator, datePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"{datePart}(CONVERT(DATETIME, '{compareValue}'))"));
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var days = DayOfWeekSunday;
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(EqualsComparator, FullDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR(10),DATEADD(dd, { days }, GETDATE()),101)"));
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithInvalidDatePartAndExpTodaySquarBracket_ReturnsEmptyString()
        {
            // Arrange
            var days = DayOfWeekSunday;
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(EqualsComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_EqualsComparatorWithNonFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var days = DayOfWeekSunday;
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(EqualsComparator, datePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR,{ condition.DatePart }(DATEADD(dd, " + days + ", GETDATE())))"));
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithFullDatePartAndExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(EqualsComparator, FullDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe("CONVERT(VARCHAR(10),GETDATE(),101)"));
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithInvalidDatePartAndExpToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(EqualsComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_EqualsComparatorWithNonFullDatePartAndExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(EqualsComparator, datePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR,{ datePart }(GETDATE()))"));
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(EqualsComparator, FullDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR(10),CONVERT(DATETIME, '{ compareValue }'),101)"));
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithInvalidDatePartAndExpNotToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(EqualsComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_EqualsComparatorWithNonFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(EqualsComparator, datePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR,{ condition.DatePart }(CONVERT(DATETIME, '{ condition.CompareValue }')))"));
        }

        [Test]
        public void ConvertValue_InvalidComparator_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = InvalidComparator;
            var condition = CreateConditionWithFieldType(InvalidComparator, InvalidDatePart, compareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test]
        public void ConvertValue_IsInComparatorWithNumberConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = NumberFieldType,
                FieldType = NumberFieldType,
                Comparator = IsInComparator,
                CompareValue = MultipleCompareValues
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(INT, {DummyCompareValue}),CONVERT(INT, {DummyCompareValue}),CONVERT(INT, {DummyCompareValue})"));
        }

        [Test, TestCaseSource(nameof(ComparisonComparators))]
        public void ConvertValue_ComparisonComparatorsWithNumberConditionField_ReturnsConvertVarchar(string comparator)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = NumberFieldType,
                FieldType = NumberFieldType,
                Comparator = comparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(INT, {DummyCompareValue})"));
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithNumberConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = NumberFieldType,
                FieldType = NumberFieldType,
                Comparator = InvalidComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test]
        public void ConvertValue_IsInComparatorWithMoneyConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = MoneyFieldType,
                FieldType = MoneyFieldType,
                Comparator = IsInComparator,
                CompareValue = MultipleCompareValues
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(MONEY, {DummyCompareValue}),CONVERT(MONEY, {DummyCompareValue}),CONVERT(MONEY, {DummyCompareValue})"));
        }

        [Test, TestCaseSource(nameof(ComparisonComparators))]
        public void ConvertValue_ComparisonComparatorsWithMoneyConditionField_ReturnsConvertVarchar(string comparator)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = MoneyFieldType,
                FieldType = MoneyFieldType,
                Comparator = comparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(MONEY, {DummyCompareValue})"));
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithMoneyConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = MoneyFieldType,
                FieldType = MoneyFieldType,
                Comparator = InvalidComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithInvalidConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = InvalidComparator,
                FieldType = InvalidComparator,
                Comparator = InvalidComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_IsInComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsCompareValues(string switchCaseConstant)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = switchCaseConstant,
                Comparator = IsInComparator,
                CompareValue = MultipleCompareValues
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"'{DummyCompareValue}','{DummyCompareValue}','{DummyCompareValue}'"));
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_ContainsComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsContainsCompareValue(string switchCaseConstant)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = switchCaseConstant,
                Comparator = ContainsComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"'%{ DummyCompareValue }%'"));
        }

        [Test]
        public void ConvertValue_IsInComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsConvertVarcharCompareValues()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = StringFieldType,
                FieldType = StringFieldType,
                Comparator = IsInComparator,
                CompareValue = MultipleCompareValues
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR, '{DummyCompareValue}'),CONVERT(VARCHAR, '{DummyCompareValue}'),CONVERT(VARCHAR, '{DummyCompareValue}')"));
        }

        [Test]
        public void ConvertValue_ContainsComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsConvertVarcharContainsCompareValue()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = StringFieldType,
                FieldType = StringFieldType,
                Comparator = ContainsComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(VARCHAR, '%{ DummyCompareValue }%')"));
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsEmptyString()
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = StringFieldType,
                FieldType = StringFieldType,
                Comparator = InvalidComparator,
                CompareValue = DummyCompareValue
            };

            // Act
            var newValue = _privateFilterType.InvokeStatic(ConvertValueMethodName, condition) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }
        private FilterCondition CreateConditionWithFieldType(string comparator, string datePart, string compareValue)
        {
            return new FilterCondition()
            {
                Field = DateFieldType,
                FieldType = DateFieldType,
                Comparator = comparator,
                CompareValue = compareValue,
                DatePart = datePart
            };
        }
    }
}
