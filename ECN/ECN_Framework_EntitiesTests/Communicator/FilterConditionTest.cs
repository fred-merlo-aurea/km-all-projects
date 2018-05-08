using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class FilterConditionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (FilterCondition) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            var filterConditionId = Fixture.Create<int>();
            var filterGroupId = Fixture.Create<int>();
            var sortOrder = Fixture.Create<int>();
            var field = Fixture.Create<string>();
            var fieldType = Fixture.Create<string>();
            var comparator = Fixture.Create<string>();
            var compareValue = Fixture.Create<string>();
            var notComparator = Fixture.Create<int?>();
            var datePart = Fixture.Create<string>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();

            // Act
            filterCondition.FilterConditionID = filterConditionId;
            filterCondition.FilterGroupID = filterGroupId;
            filterCondition.SortOrder = sortOrder;
            filterCondition.Field = field;
            filterCondition.FieldType = fieldType;
            filterCondition.Comparator = comparator;
            filterCondition.CompareValue = compareValue;
            filterCondition.NotComparator = notComparator;
            filterCondition.DatePart = datePart;
            filterCondition.CreatedDate = createdDate;
            filterCondition.UpdatedDate = updatedDate;
            filterCondition.CreatedUserID = createdUserId;
            filterCondition.UpdatedUserID = updatedUserId;
            filterCondition.IsDeleted = isDeleted;
            filterCondition.CustomerID = customerId;

            // Assert
            filterCondition.FilterConditionID.ShouldBe(filterConditionId);
            filterCondition.FilterGroupID.ShouldBe(filterGroupId);
            filterCondition.SortOrder.ShouldBe(sortOrder);
            filterCondition.Field.ShouldBe(field);
            filterCondition.FieldType.ShouldBe(fieldType);
            filterCondition.Comparator.ShouldBe(comparator);
            filterCondition.CompareValue.ShouldBe(compareValue);
            filterCondition.NotComparator.ShouldBe(notComparator);
            filterCondition.DatePart.ShouldBe(datePart);
            filterCondition.CreatedDate.ShouldBe(createdDate);
            filterCondition.UpdatedDate.ShouldBe(updatedDate);
            filterCondition.CreatedUserID.ShouldBe(createdUserId);
            filterCondition.UpdatedUserID.ShouldBe(updatedUserId);
            filterCondition.IsDeleted.ShouldBe(isDeleted);
            filterCondition.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (Comparator) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Comparator_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.Comparator = Fixture.Create<string>();
            var stringType = filterCondition.Comparator.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (Comparator) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_ComparatorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComparator = "ComparatorNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameComparator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Comparator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComparator = "Comparator";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameComparator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CompareValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CompareValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.CompareValue = Fixture.Create<string>();
            var stringType = filterCondition.CompareValue.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CompareValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_CompareValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompareValue = "CompareValueNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameCompareValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CompareValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompareValue = "CompareValue";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameCompareValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var filterCondition = Fixture.Create<FilterCondition>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(filterCondition, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterCondition.CreatedUserID = random;

            // Assert
            filterCondition.CreatedUserID.ShouldBe(random);
            filterCondition.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();

            // Act , Set
            filterCondition.CreatedUserID = null;

            // Assert
            filterCondition.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var filterCondition = Fixture.Create<FilterCondition>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(filterCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterCondition.CreatedUserID.ShouldBeNull();
            filterCondition.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterCondition.CustomerID = random;

            // Assert
            filterCondition.CustomerID.ShouldBe(random);
            filterCondition.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();

            // Act , Set
            filterCondition.CustomerID = null;

            // Assert
            filterCondition.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var filterCondition = Fixture.Create<FilterCondition>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(filterCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterCondition.CustomerID.ShouldBeNull();
            filterCondition.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (DatePart) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_DatePart_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.DatePart = Fixture.Create<string>();
            var stringType = filterCondition.DatePart.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (DatePart) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_DatePartNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDatePart = "DatePartNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameDatePart));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_DatePart_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDatePart = "DatePart";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameDatePart);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (Field) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Field_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.Field = Fixture.Create<string>();
            var stringType = filterCondition.Field.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (Field) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_FieldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField = "FieldNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameField));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Field_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField = "Field";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameField);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (FieldType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_FieldType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.FieldType = Fixture.Create<string>();
            var stringType = filterCondition.FieldType.GetType();

            // Act
            var isTypeString = typeof(string) == (stringType);
            var isTypeInt = typeof(int) == (stringType);
            var isTypeDecimal = typeof(decimal) == (stringType);
            var isTypeLong = typeof(long) == (stringType);
            var isTypeBool = typeof(bool) == (stringType);
            var isTypeDouble = typeof(double) == (stringType);
            var isTypeFloat = typeof(float) == (stringType);

            // Assert
            isTypeString.ShouldBeTrue();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (FieldType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_FieldTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldType = "FieldTypeNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameFieldType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_FieldType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldType = "FieldType";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameFieldType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (FilterConditionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_FilterConditionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.FilterConditionID = Fixture.Create<int>();
            var intType = filterCondition.FilterConditionID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (FilterConditionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_FilterConditionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterConditionID = "FilterConditionIDNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameFilterConditionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_FilterConditionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterConditionID = "FilterConditionID";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameFilterConditionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (FilterGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_FilterGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.FilterGroupID = Fixture.Create<int>();
            var intType = filterCondition.FilterGroupID.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (FilterGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_FilterGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterGroupID = "FilterGroupIDNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameFilterGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_FilterGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterGroupID = "FilterGroupID";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameFilterGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            var random = Fixture.Create<bool>();

            // Act , Set
            filterCondition.IsDeleted = random;

            // Assert
            filterCondition.IsDeleted.ShouldBe(random);
            filterCondition.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();

            // Act , Set
            filterCondition.IsDeleted = null;

            // Assert
            filterCondition.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var filterCondition = Fixture.Create<FilterCondition>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(filterCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterCondition.IsDeleted.ShouldBeNull();
            filterCondition.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (NotComparator) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_NotComparator_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterCondition.NotComparator = random;

            // Assert
            filterCondition.NotComparator.ShouldBe(random);
            filterCondition.NotComparator.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_NotComparator_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();

            // Act , Set
            filterCondition.NotComparator = null;

            // Assert
            filterCondition.NotComparator.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_NotComparator_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameNotComparator = "NotComparator";
            var filterCondition = Fixture.Create<FilterCondition>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameNotComparator);

            // Act , Set
            propertyInfo.SetValue(filterCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterCondition.NotComparator.ShouldBeNull();
            filterCondition.NotComparator.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (NotComparator) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_NotComparatorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNotComparator = "NotComparatorNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameNotComparator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_NotComparator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNotComparator = "NotComparator";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameNotComparator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (SortOrder) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_SortOrder_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            filterCondition.SortOrder = Fixture.Create<int>();
            var intType = filterCondition.SortOrder.GetType();

            // Act
            var isTypeInt = typeof(int) == (intType);
            var isTypeNullableInt = typeof(int?) == (intType);
            var isTypeString = typeof(string) == (intType);
            var isTypeDecimal = typeof(decimal) == (intType);
            var isTypeLong = typeof(long) == (intType);
            var isTypeBool = typeof(bool) == (intType);
            var isTypeDouble = typeof(double) == (intType);
            var isTypeFloat = typeof(float) == (intType);
            var isTypeDecimalNullable = typeof(decimal?) == (intType);
            var isTypeLongNullable = typeof(long?) == (intType);
            var isTypeBoolNullable = typeof(bool?) == (intType);
            var isTypeDoubleNullable = typeof(double?) == (intType);
            var isTypeFloatNullable = typeof(float?) == (intType);

            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (SortOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_SortOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrderNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameSortOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_SortOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrder";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var filterCondition = Fixture.Create<FilterCondition>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(filterCondition, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterCondition.UpdatedUserID = random;

            // Assert
            filterCondition.UpdatedUserID.ShouldBe(random);
            filterCondition.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterCondition = Fixture.Create<FilterCondition>();

            // Act , Set
            filterCondition.UpdatedUserID = null;

            // Assert
            filterCondition.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var filterCondition = Fixture.Create<FilterCondition>();
            var propertyInfo = filterCondition.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(filterCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterCondition.UpdatedUserID.ShouldBeNull();
            filterCondition.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterCondition) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var filterCondition  = Fixture.Create<FilterCondition>();

            // Act , Assert
            Should.NotThrow(() => filterCondition.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterCondition_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var filterCondition  = Fixture.Create<FilterCondition>();
            var propertyInfo  = filterCondition.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (FilterCondition) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterCondition_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new FilterCondition());
        }

        #endregion

        #region General Constructor : Class (FilterCondition) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterCondition_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfFilterCondition = Fixture.CreateMany<FilterCondition>(2).ToList();
            var firstFilterCondition = instancesOfFilterCondition.FirstOrDefault();
            var lastFilterCondition = instancesOfFilterCondition.Last();

            // Act, Assert
            firstFilterCondition.ShouldNotBeNull();
            lastFilterCondition.ShouldNotBeNull();
            firstFilterCondition.ShouldNotBeSameAs(lastFilterCondition);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterCondition_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstFilterCondition = new FilterCondition();
            var secondFilterCondition = new FilterCondition();
            var thirdFilterCondition = new FilterCondition();
            var fourthFilterCondition = new FilterCondition();
            var fifthFilterCondition = new FilterCondition();
            var sixthFilterCondition = new FilterCondition();

            // Act, Assert
            firstFilterCondition.ShouldNotBeNull();
            secondFilterCondition.ShouldNotBeNull();
            thirdFilterCondition.ShouldNotBeNull();
            fourthFilterCondition.ShouldNotBeNull();
            fifthFilterCondition.ShouldNotBeNull();
            sixthFilterCondition.ShouldNotBeNull();
            firstFilterCondition.ShouldNotBeSameAs(secondFilterCondition);
            thirdFilterCondition.ShouldNotBeSameAs(firstFilterCondition);
            fourthFilterCondition.ShouldNotBeSameAs(firstFilterCondition);
            fifthFilterCondition.ShouldNotBeSameAs(firstFilterCondition);
            sixthFilterCondition.ShouldNotBeSameAs(firstFilterCondition);
            sixthFilterCondition.ShouldNotBeSameAs(fourthFilterCondition);
        }

        #endregion

        #region General Constructor : Class (FilterCondition) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterCondition_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var filterConditionId = -1;
            var filterGroupId = -1;
            var sortOrder = -1;
            var field = string.Empty;
            var fieldType = string.Empty;
            var comparator = string.Empty;
            var compareValue = string.Empty;
            var datePart = string.Empty;

            // Act
            var filterCondition = new FilterCondition();

            // Assert
            filterCondition.FilterConditionID.ShouldBe(filterConditionId);
            filterCondition.FilterGroupID.ShouldBe(filterGroupId);
            filterCondition.SortOrder.ShouldBe(sortOrder);
            filterCondition.Field.ShouldBe(field);
            filterCondition.FieldType.ShouldBe(fieldType);
            filterCondition.Comparator.ShouldBe(comparator);
            filterCondition.CompareValue.ShouldBe(compareValue);
            filterCondition.NotComparator.ShouldBeNull();
            filterCondition.DatePart.ShouldBe(datePart);
            filterCondition.CreatedDate.ShouldBeNull();
            filterCondition.UpdatedDate.ShouldBeNull();
            filterCondition.CreatedUserID.ShouldBeNull();
            filterCondition.UpdatedUserID.ShouldBeNull();
            filterCondition.IsDeleted.ShouldBeNull();
            filterCondition.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}