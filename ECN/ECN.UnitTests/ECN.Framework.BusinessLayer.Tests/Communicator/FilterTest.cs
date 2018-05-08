using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_BusinessLayer.Communicator;
using ECN.Tests.Helpers;
using NUnit.Framework;
using Shouldly;
using CommunicatorFilterCondition = ECN_Framework_Entities.Communicator.FilterCondition;
using Entities = ECN_Framework_Entities;
using static ECN_Framework_Common.Objects.Enums;
using static KMPlatform.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FilterTest
    {
        private const string DummyCompareValue = "CompareValue";
        private const string MultipleCompareValues = "CompareValue,CompareValue,CompareValue";
        private const string EqualsComparator = "equals";
        private const string IsInComparator = "is in";
        private const string ContainsComparator = "contains";
        private const string EndsWithComparator = "ends with";
        private const string StartsWithComparator = "starts with";
        private const string InvalidComparator = "invalid";
        private const string FullDatePart = "full";
        private const string DayDatePart = "day";
        private const string MonthDatePart = "month";
        private const string YearDatePart = "year";
        private const string InvalidDatePart = "invalid";
        private const string LessThanComparator = "less than";
        private const string DateFieldType = "Date";
        private const string StringFieldType = "String";
        private const string GreaterThanComparator = "greater than";
        private const string NumberFieldType = "Number";
        private const string MoneyFieldType = "Money";
        private const string ConvertValueMethodName = "ConvertValue";
        
        private const Entity _entity = Entity.Filter;
        private static readonly Services _serviceCode = Services.EMAILMARKETING;
        private static readonly ServiceFeatures _serviceFeatureCode = ServiceFeatures.GroupFilter;

        private static string[] ComparisonComparators => new string[]
        {
            GreaterThanComparator,
            LessThanComparator,
            EqualsComparator
        };

        private static string[] NonFullDateParts => new string[]
        {
            DayDatePart,
            MonthDatePart,
            YearDatePart
        };

        private static string[] NonCommonSwitchCaseConstants => new string[]
        {
            "Birthdate",
            "UserEvent1Date",
            "UserEvent2Date",
            "CreatedOn",
            "LastChanged",
        };

        private static string[] CommonSwitchCaseConstants => new string[]
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

        [Test]
        public void CreateWhereClause_NullFilterGroupList_ReturnsEmpty()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = null
            };

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBeEmpty();
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupList_ReturnsEmpty()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
            };

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBeEmpty();
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithNullFilterConditionList_ReturnsEmpty()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = null
                    }
                }
            };

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBeEmpty();
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithGenericFilterConditionList_ReturnsEmpty()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>()
                    }
                }
            };

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBeEmpty();
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndEmptyComparatorAndNotComparatorIs1_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "is empty",
                             NotComparator = 1,
                             Field = "FieldName"
                         }
                     }
                    }
                }
            };
            string expectedResult = $"((rtrim(ltrim(ISNULL([{filter.FilterGroupList[0].FilterConditionList[0].Field}], ''))) <> '') )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert            
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndEmptyComparatorAndNotComparatorIs0_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "is empty",
                             NotComparator = 0,
                             Field = "FieldName"
                         }
                     }
                    }
                }
            };
            string expectedResult = $"((rtrim(ltrim(ISNULL([{filter.FilterGroupList[0].FilterConditionList[0].Field}], ''))) = '') )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndContainsComparatorAndNotComparatorIs0_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "contains",
                             NotComparator = 0,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} LIKE ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndContainsComparatorAndNotComparatorIs1_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "contains",
                             NotComparator = 1,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} NOT LIKE ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndIsInComparatorAndNotComparatorIs0_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "is in",
                             NotComparator = 0,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} IN ()) )";

            //Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndIsInComparatorAndNotComparatorIs1_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "is in",
                             NotComparator = 1,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} NOT IN ()) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndEqualsComparatorAndNotComparatorIs0_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "equals",
                             NotComparator = 0,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} = ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndEqualsComparatorAndNotComparatorIs1_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "equals",
                             NotComparator = 1,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} != ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert            
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndGreaterThanComparatorAndNotComparatorIs0_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "greater than",
                             NotComparator = 0,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} > ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndGreaterThanComparatorAndNotComparatorIs1_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "greater than",
                             NotComparator = 1,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} <= ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndLessThanComparatorAndNotComparatorIs0_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "less than",
                             NotComparator = 0,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} < ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test]
        public void CreateWhereClause_GenericFilterGroupWithFilterConditionListAndLessThanComparatorAndNotComparatorIs1_ReturnsWhereClause()
        {
            // Arrange
            var filter = new Entities.Communicator.Filter()
            {
                FilterGroupList = new List<Entities.Communicator.FilterGroup>()
                {
                    new Entities.Communicator.FilterGroup{
                     FilterConditionList = new List<CommunicatorFilterCondition>(){
                         new CommunicatorFilterCondition{
                             Comparator = "less than",
                             NotComparator = 1,
                             Field = "FieldName",
                         }
                     }
                    }
                }
            };
            string expectedResult = $"(({filter.FilterGroupList[0].FilterConditionList[0].Field} >= ) )";

            // Act
            string whereClause = Filter.CreateWhereClause(filter);

            // Assert
            whereClause.ShouldBe(expectedResult);
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_CommonSwitchCaseConstantsConditionField_ReturnsCompareValue(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                switchCaseConstant,
                                null,
                                EqualsComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"'{DummyCompareValue}'");
        }

        [Test]
        public void ConvertValue_StringConditionField_ReturnsStringValueOfConvertVarchar()
        {
            // Arrange
            var stringFieldType = "String";
            var condition = CreateConditionWithFieldType(
                                stringFieldType,
                                stringFieldType,
                                EqualsComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(500), '{ DummyCompareValue }')");
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertField_CommonSwitchCaseConstantsConditionField_ReturnsStringValueOfISNULLConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISNULL({switchCaseConstant}, '')");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_NonCommonSwitchCaseConstantsConditionField_ReturnsStringValueOfISDateConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, null, null, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND CONVERT(VARCHAR(10), {switchCaseConstant }, 101)");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_EqualComparatorWithYearDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, null, null, YearDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {YearDatePart}({switchCaseConstant })");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_EqualComparatorWithMonthDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, null, null, MonthDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {MonthDatePart}({switchCaseConstant })");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_EqualComparatorWithDayDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, null, null, DayDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {DayDatePart}({switchCaseConstant })");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_EqualComparatorWithInvalidDatePartWithNonCommonFields_ReturnsEmptyString(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, null, null, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNull();
            newField.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_LessThanComparatorWithFullDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, LessThanComparator, null, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {switchCaseConstant }");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_LessThanComparatorWithYearDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, LessThanComparator, null, YearDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {YearDatePart}({switchCaseConstant })");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_LessThanComparatorWithMonthDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, LessThanComparator, null, MonthDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {MonthDatePart}({switchCaseConstant })");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_LessThanComparatorWithDayDatePartWithNonCommonFields_ReturnsStringValueOfISDateFieldAndConditionField(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, LessThanComparator, null, DayDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"ISDATE({switchCaseConstant}) = 1 AND {DayDatePart}({switchCaseConstant })");
        }

        [Test, TestCaseSource(nameof(NonCommonSwitchCaseConstants))]
        public void ConvertField_LessThanComparatorWithInvalidDatePartWithNonCommonFields_ReturnsEmptyString(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(switchCaseConstant, null, LessThanComparator, null, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNull();
            newField.ShouldBeEmpty();
        }

        [Test]
        public void ConvertField_LessThanComparatorWithFullDatePartWithDifferentDateField_ReturnsStringValueOfCaseWhenIsDateThenConvert()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                DateFieldType,
                                DateFieldType,
                                LessThanComparator,
                                null,
                                FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"CASE WHEN ISDATE([{ DateFieldType }]) = 1 THEN CONVERT(DATETIME, [{ DateFieldType }]) end");
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertField_LessThanComparatorWithNonFullDatePartWithDifferentDateField_ReturnsStringValueOfCaseWhenIsDateThenDatePartConvert(string datePart)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                DateFieldType,
                                DateFieldType,
                                LessThanComparator,
                                null,
                                datePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"CASE WHEN ISDATE([{ DateFieldType }]) = 1 THEN { datePart }(CONVERT(DATETIME, [{ DateFieldType }])) end");
        }

        [Test]
        public void ConvertField_LessThanComparatorWithInvalidDatePartWithDifferentDateField_ReturnsEmptyString()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                DateFieldType,
                                DateFieldType,
                                LessThanComparator,
                                null,
                                InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNull();
            newField.ShouldBeEmpty();
        }

        [Test]
        public void ConvertField_EqualsComparatorWithFullDatePartWithDifferentDateField_ReturnsStringValueOfCaseWhenIsDateThenConvertWithThreeParameters()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                DateFieldType,
                                DateFieldType,
                                EqualsComparator,
                                null,
                                FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"CASE WHEN ISDATE([{ DateFieldType }]) = 1 then CONVERT(VARCHAR(10),CONVERT(DATETIME, [{ DateFieldType }]), 101) end");
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertField_EqualsComparatorWithNonFullDatePartWithDifferentDateField_ReturnsStringValueOfCaseWhenIsDateThenDatePartConvert(string datePart)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                DateFieldType,
                                DateFieldType,
                                EqualsComparator,
                                null,
                                datePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNullOrWhiteSpace();
            newField.ShouldBe($"CASE WHEN ISDATE([{ DateFieldType }]) = 1 then { datePart }({ DateFieldType }) end");
        }

        [Test]
        public void ConvertField_EqualsComparatorWithInvalidDatePartWithDifferentDateField_ReturnsEmptyString()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                DateFieldType,
                                DateFieldType,
                                EqualsComparator,
                                null,
                                InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newField = typeof(Filter).CallMethod("ConvertField", parameters) as string;

            // Assert
            newField.ShouldNotBeNull();
            newField.ShouldBeEmpty();
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var days = "Sunday";
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"DATEADD(dd, { days }, GETDATE())");
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithInvalidDatePartAndExpTodaySquarBracket_ReturnsEmptyString()
        {
            // Arrange
            var days = "Sunday";
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_LessThanComparatorWithNonFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var days = "Sunday";
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, datePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"{ datePart }(DATEADD(dd, { days }, GETDATE()))");
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithFullDatePartAndExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe("GETDATE()");
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithInvalidDatePartAndExpToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_LessThanComparatorWithNonFullDatePartAndExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, datePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"{ datePart }(GETDATE())");
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(DATETIME, '" + compareValue + "')");
        }

        [Test]
        public void ConvertValue_LessThanComparatorWithInvalidDatePartAndNotExpToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_LessThanComparatorWithNonFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, LessThanComparator, compareValue, datePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe(compareValue);
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var days = "Sunday";
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(10),DATEADD(dd, { days }, GETDATE()),101)");
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithInvalidDatePartAndExpTodaySquarBracket_ReturnsEmptyString()
        {
            // Arrange
            var days = "Sunday";
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_EqualsComparatorWithNonFullDatePartAndExpTodaySquarBracket_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var days = "Sunday";
            var compareValue = $"EXP:Today[{ days }]";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, datePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR,{ condition.DatePart }(DATEADD(dd, " + days + ", GETDATE())))");
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithFullDatePartAndExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe("CONVERT(VARCHAR(10),GETDATE(),101)");
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithInvalidDatePartAndExpToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_EqualsComparatorWithNonFullDatePartAndExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:Today";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, datePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR,{ datePart }(GETDATE()))");
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, FullDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(10),CONVERT(DATETIME, '{ compareValue }'),101)");
        }

        [Test]
        public void ConvertValue_EqualsComparatorWithInvalidDatePartAndExpNotToday_ReturnsEmptyString()
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, InvalidDatePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test, TestCaseSource(nameof(NonFullDateParts))]
        public void ConvertValue_EqualsComparatorWithNonFullDatePartAndNoExpToday_ReturnsStringValueOfDateAdd(string datePart)
        {
            // Arrange
            var compareValue = $"EXP:NotToday";
            var condition = CreateConditionWithFieldType(DateFieldType, DateFieldType, EqualsComparator, compareValue, datePart);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe(compareValue);
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_IsInComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsCompareValues(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                switchCaseConstant,
                                null,
                                IsInComparator,
                                MultipleCompareValues);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"'{DummyCompareValue}','{DummyCompareValue}','{DummyCompareValue}'");
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_ContainsComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsContainsCompareValue(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                switchCaseConstant,
                                null,
                                ContainsComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"'%{ DummyCompareValue }%'");
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_StartsWithComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsStartsWithCompareValue(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                switchCaseConstant,
                                null,
                                StartsWithComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"'{ DummyCompareValue }%'");
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_EndsWithComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsEndsWithCompareValue(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                switchCaseConstant,
                                null, 
                                EndsWithComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"'%{ DummyCompareValue }'");
        }

        [Test, TestCaseSource(nameof(CommonSwitchCaseConstants))]
        public void ConvertValue_InvalidComparatorWithCommonSwitchCaseConstantsConditionField_ReturnsEmptyString(string switchCaseConstant)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                switchCaseConstant,
                null, 
                InvalidComparator,
                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test]
        public void ConvertValue_IsInComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsConvertVarcharCompareValues()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                StringFieldType,
                                StringFieldType,
                                IsInComparator,
                                MultipleCompareValues);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(500), '{DummyCompareValue}'),CONVERT(VARCHAR(500), '{DummyCompareValue}'),CONVERT(VARCHAR(500), '{DummyCompareValue}')");
        }

        [Test]
        public void ConvertValue_ContainsComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsConvertVarcharContainsCompareValue()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                StringFieldType,
                                StringFieldType,
                                ContainsComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(500), '%{ DummyCompareValue }%')");
        }

        [Test]
        public void ConvertValue_StartsWithComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsConvertVarcharStartsWithCompareValue()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                StringFieldType,
                                StringFieldType,
                                StartsWithComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(500), '{ DummyCompareValue }%')");
        }

        [Test]
        public void ConvertValue_EndsWithComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsConvertVarcharEndsWithCompareValue()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                StringFieldType,
                                StringFieldType,
                                EndsWithComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNullOrWhiteSpace();
            newValue.ShouldBe($"CONVERT(VARCHAR(500), '%{ DummyCompareValue }')");
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithNonCommonSwitchCaseConstantsConditionField_ReturnsEmptyString()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                StringFieldType,
                                StringFieldType,
                                InvalidComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod("ConvertValue", parameters) as string;

            // Assert
            newValue.ShouldNotBeNull();
            newValue.ShouldBeEmpty();
        }

        [Test]
        public void ConvertValue_IsInComparatorWithNumberConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                 NumberFieldType,
                                 NumberFieldType,
                                 IsInComparator,
                                 MultipleCompareValues);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(INT, {DummyCompareValue}),CONVERT(INT, {DummyCompareValue}),CONVERT(INT, {DummyCompareValue})"));
        }

        [Test, TestCaseSource(nameof(ComparisonComparators))]
        public void ConvertValue_ComparisonComparatorsWithNumberConditionField_ReturnsConvertVarchar(string comparator)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                NumberFieldType,
                                NumberFieldType,
                                comparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(INT, {DummyCompareValue})"));
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithNumberConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                NumberFieldType,
                                NumberFieldType,
                                InvalidComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test]
        public void ConvertValue_IsInComparatorWithMoneyConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                MoneyFieldType,
                                MoneyFieldType,
                                IsInComparator,
                                MultipleCompareValues);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(MONEY, {DummyCompareValue}),CONVERT(MONEY, {DummyCompareValue}),CONVERT(MONEY, {DummyCompareValue})"));
        }

        [Test, TestCaseSource(nameof(ComparisonComparators))]
        public void ConvertValue_ComparisonComparatorsWithMoneyConditionField_ReturnsConvertVarchar(string comparator)
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                MoneyFieldType,
                                MoneyFieldType,
                                comparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNullOrWhiteSpace(),
                () => newValue.ShouldBe($"CONVERT(MONEY, {DummyCompareValue})"));
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithMoneyConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                MoneyFieldType,
                                MoneyFieldType,
                                InvalidComparator,
                                DummyCompareValue);
            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        [Test]
        public void ConvertValue_InvalidComparatorWithInvalidConditionField_ReturnsConvertVarchar()
        {
            // Arrange
            var condition = CreateConditionWithFieldType(
                                InvalidComparator,
                                InvalidComparator,
                                InvalidComparator,
                                DummyCompareValue);

            // Act
            var parameters = new object[] { condition };
            var newValue = typeof(Filter).CallMethod(ConvertValueMethodName, parameters) as string;

            // Assert
            newValue.ShouldSatisfyAllConditions(
                () => newValue.ShouldNotBeNull(),
                () => newValue.ShouldBeEmpty());
        }

        private static CommunicatorFilterCondition CreateConditionWithFieldType(string field, string fieldType = null, string comparator = null, string compareValue = null, string datePart = null)
        {
            return new CommunicatorFilterCondition()
            {
                Field = field,
                FieldType = fieldType,
                Comparator = comparator,
                CompareValue = compareValue,
                DatePart = datePart
            };
        }
    }
}