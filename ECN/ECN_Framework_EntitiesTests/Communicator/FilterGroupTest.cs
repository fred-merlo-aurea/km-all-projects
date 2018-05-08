using System;
using System.Collections.Generic;
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
    public class FilterGroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (FilterGroup) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            var filterGroupId = Fixture.Create<int>();
            var filterId = Fixture.Create<int>();
            var sortOrder = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var conditionCompareType = Fixture.Create<string>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var filterConditionList = Fixture.Create<List<FilterCondition>>();

            // Act
            filterGroup.FilterGroupID = filterGroupId;
            filterGroup.FilterID = filterId;
            filterGroup.SortOrder = sortOrder;
            filterGroup.Name = name;
            filterGroup.ConditionCompareType = conditionCompareType;
            filterGroup.CreatedDate = createdDate;
            filterGroup.UpdatedDate = updatedDate;
            filterGroup.CreatedUserID = createdUserId;
            filterGroup.UpdatedUserID = updatedUserId;
            filterGroup.IsDeleted = isDeleted;
            filterGroup.CustomerID = customerId;
            filterGroup.FilterConditionList = filterConditionList;

            // Assert
            filterGroup.FilterGroupID.ShouldBe(filterGroupId);
            filterGroup.FilterID.ShouldBe(filterId);
            filterGroup.SortOrder.ShouldBe(sortOrder);
            filterGroup.Name.ShouldBe(name);
            filterGroup.ConditionCompareType.ShouldBe(conditionCompareType);
            filterGroup.CreatedDate.ShouldBe(createdDate);
            filterGroup.UpdatedDate.ShouldBe(updatedDate);
            filterGroup.CreatedUserID.ShouldBe(createdUserId);
            filterGroup.UpdatedUserID.ShouldBe(updatedUserId);
            filterGroup.IsDeleted.ShouldBe(isDeleted);
            filterGroup.CustomerID.ShouldBe(customerId);
            filterGroup.FilterConditionList.ShouldBe(filterConditionList);
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (ConditionCompareType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_ConditionCompareType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            filterGroup.ConditionCompareType = Fixture.Create<string>();
            var stringType = filterGroup.ConditionCompareType.GetType();

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

        #region General Getters/Setters : Class (FilterGroup) => Property (ConditionCompareType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_ConditionCompareTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConditionCompareType = "ConditionCompareTypeNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameConditionCompareType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_ConditionCompareType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConditionCompareType = "ConditionCompareType";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameConditionCompareType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var filterGroup = Fixture.Create<FilterGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = filterGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(filterGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterGroup.CreatedUserID = random;

            // Assert
            filterGroup.CreatedUserID.ShouldBe(random);
            filterGroup.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();

            // Act , Set
            filterGroup.CreatedUserID = null;

            // Assert
            filterGroup.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var filterGroup = Fixture.Create<FilterGroup>();
            var propertyInfo = filterGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(filterGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterGroup.CreatedUserID.ShouldBeNull();
            filterGroup.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterGroup.CustomerID = random;

            // Assert
            filterGroup.CustomerID.ShouldBe(random);
            filterGroup.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();

            // Act , Set
            filterGroup.CustomerID = null;

            // Assert
            filterGroup.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var filterGroup = Fixture.Create<FilterGroup>();
            var propertyInfo = filterGroup.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(filterGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterGroup.CustomerID.ShouldBeNull();
            filterGroup.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (FilterConditionList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_FilterConditionListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterConditionList = "FilterConditionListNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameFilterConditionList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_FilterConditionList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterConditionList = "FilterConditionList";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameFilterConditionList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (FilterGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_FilterGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            filterGroup.FilterGroupID = Fixture.Create<int>();
            var intType = filterGroup.FilterGroupID.GetType();

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

        #region General Getters/Setters : Class (FilterGroup) => Property (FilterGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_FilterGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterGroupID = "FilterGroupIDNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameFilterGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_FilterGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterGroupID = "FilterGroupID";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameFilterGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (FilterID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_FilterID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            filterGroup.FilterID = Fixture.Create<int>();
            var intType = filterGroup.FilterID.GetType();

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

        #region General Getters/Setters : Class (FilterGroup) => Property (FilterID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_FilterIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterIDNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameFilterID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_FilterID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterID";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameFilterID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            var random = Fixture.Create<bool>();

            // Act , Set
            filterGroup.IsDeleted = random;

            // Assert
            filterGroup.IsDeleted.ShouldBe(random);
            filterGroup.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();

            // Act , Set
            filterGroup.IsDeleted = null;

            // Assert
            filterGroup.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var filterGroup = Fixture.Create<FilterGroup>();
            var propertyInfo = filterGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(filterGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterGroup.IsDeleted.ShouldBeNull();
            filterGroup.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            filterGroup.Name = Fixture.Create<string>();
            var stringType = filterGroup.Name.GetType();

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

        #region General Getters/Setters : Class (FilterGroup) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (SortOrder) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_SortOrder_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            filterGroup.SortOrder = Fixture.Create<int>();
            var intType = filterGroup.SortOrder.GetType();

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

        #region General Getters/Setters : Class (FilterGroup) => Property (SortOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_SortOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrderNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameSortOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_SortOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrder";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var filterGroup = Fixture.Create<FilterGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = filterGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(filterGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            filterGroup.UpdatedUserID = random;

            // Assert
            filterGroup.UpdatedUserID.ShouldBe(random);
            filterGroup.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filterGroup = Fixture.Create<FilterGroup>();

            // Act , Set
            filterGroup.UpdatedUserID = null;

            // Assert
            filterGroup.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var filterGroup = Fixture.Create<FilterGroup>();
            var propertyInfo = filterGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(filterGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filterGroup.UpdatedUserID.ShouldBeNull();
            filterGroup.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (FilterGroup) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var filterGroup  = Fixture.Create<FilterGroup>();

            // Act , Assert
            Should.NotThrow(() => filterGroup.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FilterGroup_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var filterGroup  = Fixture.Create<FilterGroup>();
            var propertyInfo  = filterGroup.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (FilterGroup) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterGroup_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new FilterGroup());
        }

        #endregion

        #region General Constructor : Class (FilterGroup) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterGroup_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfFilterGroup = Fixture.CreateMany<FilterGroup>(2).ToList();
            var firstFilterGroup = instancesOfFilterGroup.FirstOrDefault();
            var lastFilterGroup = instancesOfFilterGroup.Last();

            // Act, Assert
            firstFilterGroup.ShouldNotBeNull();
            lastFilterGroup.ShouldNotBeNull();
            firstFilterGroup.ShouldNotBeSameAs(lastFilterGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterGroup_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstFilterGroup = new FilterGroup();
            var secondFilterGroup = new FilterGroup();
            var thirdFilterGroup = new FilterGroup();
            var fourthFilterGroup = new FilterGroup();
            var fifthFilterGroup = new FilterGroup();
            var sixthFilterGroup = new FilterGroup();

            // Act, Assert
            firstFilterGroup.ShouldNotBeNull();
            secondFilterGroup.ShouldNotBeNull();
            thirdFilterGroup.ShouldNotBeNull();
            fourthFilterGroup.ShouldNotBeNull();
            fifthFilterGroup.ShouldNotBeNull();
            sixthFilterGroup.ShouldNotBeNull();
            firstFilterGroup.ShouldNotBeSameAs(secondFilterGroup);
            thirdFilterGroup.ShouldNotBeSameAs(firstFilterGroup);
            fourthFilterGroup.ShouldNotBeSameAs(firstFilterGroup);
            fifthFilterGroup.ShouldNotBeSameAs(firstFilterGroup);
            sixthFilterGroup.ShouldNotBeSameAs(firstFilterGroup);
            sixthFilterGroup.ShouldNotBeSameAs(fourthFilterGroup);
        }

        #endregion

        #region General Constructor : Class (FilterGroup) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FilterGroup_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var filterGroupId = -1;
            var filterId = -1;
            var sortOrder = -1;
            var name = string.Empty;
            var conditionCompareType = string.Empty;

            // Act
            var filterGroup = new FilterGroup();

            // Assert
            filterGroup.FilterGroupID.ShouldBe(filterGroupId);
            filterGroup.FilterID.ShouldBe(filterId);
            filterGroup.SortOrder.ShouldBe(sortOrder);
            filterGroup.Name.ShouldBe(name);
            filterGroup.ConditionCompareType.ShouldBe(conditionCompareType);
            filterGroup.CreatedDate.ShouldBeNull();
            filterGroup.UpdatedDate.ShouldBeNull();
            filterGroup.CreatedUserID.ShouldBeNull();
            filterGroup.UpdatedUserID.ShouldBeNull();
            filterGroup.IsDeleted.ShouldBeNull();
            filterGroup.CustomerID.ShouldBeNull();
            filterGroup.FilterConditionList.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}