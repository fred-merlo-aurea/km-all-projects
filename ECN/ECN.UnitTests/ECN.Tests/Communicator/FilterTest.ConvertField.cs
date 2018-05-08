using NUnit.Framework;
using Shouldly;
using ecn.communicator.classes;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    public partial class FilterTest
    {
        private const string ConvertFieldMethodName = "ConvertField";

        [Test]
        public void ConvertField_FilterConditionWithFieldEmailAddress_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "EmailAddress"
            };
            var expectedResult = $"ISNULL({filterCondition.Field}, '')";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldBirthdateWithComparatorLessThanAndDatePartFull_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "Birthdate",
                Comparator = "less than",
                DatePart = "full"
            };
            var expectedResult = $"ISDATE({filterCondition.Field}) = 1 AND {filterCondition.Field}";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldBirthdateWithComparatorLessThanAndDatePartMonth_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "Birthdate",
                Comparator = "less than",
                DatePart = "month"
            };
            var expectedResult = $"ISDATE({filterCondition.Field}) = 1 AND {filterCondition.DatePart}(" + filterCondition.Field + ")";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldBirthdateWithDatePartFull_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "Birthdate",
                DatePart = "full"
            };
            var expectedResult = $"ISDATE({filterCondition.Field}) = 1 AND CONVERT(VARCHAR(10), {filterCondition.Field}, 101)";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldBirthdateWithDatePartMonth_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "Birthdate",
                DatePart = "month"
            };
            var expectedResult = $"ISDATE({filterCondition.Field}) = 1 AND CONVERT(VARCHAR, {filterCondition.DatePart}({filterCondition.Field}))";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeString_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "String"
            };
            var expectedResult = $"CONVERT(VARCHAR, ISNULL([{filterCondition.Field}], ''))";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeDateWithComparatorLessThanAndDatePartFull_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Date",
                Comparator = "less than",
                DatePart ="full"
            };
            var expectedResult = $"CASE WHEN ISDATE([{filterCondition.Field}]) = 1 THEN CONVERT(DATETIME, [{filterCondition.Field}]) end";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeDateWithComparatorLessThanAndDatePartMonth_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Date",
                Comparator = "less than",
                DatePart = "month"
            };
            var expectedResult = $"CASE WHEN ISDATE([{filterCondition.Field}]) = 1 THEN {filterCondition.DatePart}(CONVERT(DATETIME, [{filterCondition.Field}])) end";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeDateWithDatePartFull_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Date",
                DatePart = "full"
            };
            var expectedResult = $"CASE WHEN ISDATE([{filterCondition.Field}]) = 1 then CONVERT(VARCHAR(10),CONVERT(DATETIME, [{filterCondition.Field}]), 101) end";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeDateWithDatePartMonth_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Date",
                DatePart = "month"
            };
            var expectedResult = $"CASE WHEN ISDATE([{filterCondition.Field}]) = 1 then CONVERT(VARCHAR,{filterCondition.DatePart}(CONVERT(DATETIME, [{filterCondition.Field}]))) end";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeNumber_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Number"
            };
            var expectedResult = $"ISNUMERIC([{filterCondition.Field}]) = 1 AND CONVERT(INT, [{filterCondition.Field}])";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }        

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeMoney_ReturnsConvertedField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Money"
            };
            var expectedResult = $"ISNUMERIC([{filterCondition.Field}]) = 1 AND CONVERT(MONEY, [{filterCondition.Field}])";

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(expectedResult);
        }

        [Test]
        public void ConvertField_FilterConditionWithFieldTypeCustom_ReturnsField()
        {
            // Arrange
            var filterCondition = new FilterCondition()
            {
                Field = "FieldName",
                FieldType = "Custom"
            };

            // Act
            var convertedField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, filterCondition);

            // Assert
            convertedField.ShouldBe(filterCondition.Field);
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertField_DifferentSwitchCaseConstantsConditionField_ReturnsStringValueOfISNULLConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = switchCaseConstant
            };

            // Act
            var newField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, condition) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISNULL({switchCaseConstant}, '')");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_NonCommonSwitchCaseConstantsConditionField_ReturnsStringValueOfISDateConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = new FilterCondition()
            {
                Field = switchCaseConstant,
                DatePart = FullDatePart
            };

            // Act
            var newField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, condition) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND CONVERT(VARCHAR(10), {switchCaseConstant }, 101)");
        }

        [Test]
        public void ConvertField_StringConditionField_ReturnsStringValueOfConvertVarchar()
        {
            // Arrange
            var stringFieldType = "String";
            var condition = new FilterCondition()
            {
                Field = stringFieldType,
                FieldType = stringFieldType,
            };

            // Act
            var newField = _privateFilterType.InvokeStatic(ConvertFieldMethodName, condition) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"CONVERT(VARCHAR, ISNULL([{stringFieldType}], ''))");
        }
    }
}
