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
    public class FilterTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Filter) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var filterId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var groupId = Fixture.Create<int?>();
            var filterName = Fixture.Create<string>();
            var whereClause = Fixture.Create<string>();
            var dynamicWhere = Fixture.Create<string>();
            var groupCompareType = Fixture.Create<string>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var archived = Fixture.Create<bool?>();
            var filterGroupList = Fixture.Create<List<FilterGroup>>();

            // Act
            filter.FilterID = filterId;
            filter.CustomerID = customerId;
            filter.GroupID = groupId;
            filter.FilterName = filterName;
            filter.WhereClause = whereClause;
            filter.DynamicWhere = dynamicWhere;
            filter.GroupCompareType = groupCompareType;
            filter.CreatedDate = createdDate;
            filter.UpdatedDate = updatedDate;
            filter.CreatedUserID = createdUserId;
            filter.UpdatedUserID = updatedUserId;
            filter.IsDeleted = isDeleted;
            filter.Archived = archived;
            filter.FilterGroupList = filterGroupList;

            // Assert
            filter.FilterID.ShouldBe(filterId);
            filter.CustomerID.ShouldBe(customerId);
            filter.GroupID.ShouldBe(groupId);
            filter.FilterName.ShouldBe(filterName);
            filter.WhereClause.ShouldBe(whereClause);
            filter.DynamicWhere.ShouldBe(dynamicWhere);
            filter.GroupCompareType.ShouldBe(groupCompareType);
            filter.CreatedDate.ShouldBe(createdDate);
            filter.UpdatedDate.ShouldBe(updatedDate);
            filter.CreatedUserID.ShouldBe(createdUserId);
            filter.UpdatedUserID.ShouldBe(updatedUserId);
            filter.IsDeleted.ShouldBe(isDeleted);
            filter.Archived.ShouldBe(archived);
            filter.FilterGroupList.ShouldBe(filterGroupList);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (Archived) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Archived_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var random = Fixture.Create<bool>();

            // Act , Set
            filter.Archived = random;

            // Assert
            filter.Archived.ShouldBe(random);
            filter.Archived.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Archived_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();

            // Act , Set
            filter.Archived = null;

            // Assert
            filter.Archived.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Archived_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameArchived = "Archived";
            var filter = Fixture.Create<Filter>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameArchived);

            // Act , Set
            propertyInfo.SetValue(filter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filter.Archived.ShouldBeNull();
            filter.Archived.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (Archived) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_ArchivedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameArchived = "ArchivedNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameArchived));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Archived_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameArchived = "Archived";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameArchived);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var filter = Fixture.Create<Filter>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(filter, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var random = Fixture.Create<int>();

            // Act , Set
            filter.CreatedUserID = random;

            // Assert
            filter.CreatedUserID.ShouldBe(random);
            filter.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();

            // Act , Set
            filter.CreatedUserID = null;

            // Assert
            filter.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var filter = Fixture.Create<Filter>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(filter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filter.CreatedUserID.ShouldBeNull();
            filter.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var random = Fixture.Create<int>();

            // Act , Set
            filter.CustomerID = random;

            // Assert
            filter.CustomerID.ShouldBe(random);
            filter.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();

            // Act , Set
            filter.CustomerID = null;

            // Assert
            filter.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var filter = Fixture.Create<Filter>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(filter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filter.CustomerID.ShouldBeNull();
            filter.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (DynamicWhere) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_DynamicWhere_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            filter.DynamicWhere = Fixture.Create<string>();
            var stringType = filter.DynamicWhere.GetType();

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

        #region General Getters/Setters : Class (Filter) => Property (DynamicWhere) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_DynamicWhereNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDynamicWhere = "DynamicWhereNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameDynamicWhere));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_DynamicWhere_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDynamicWhere = "DynamicWhere";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameDynamicWhere);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (FilterGroupList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_FilterGroupListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterGroupList = "FilterGroupListNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameFilterGroupList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_FilterGroupList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterGroupList = "FilterGroupList";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameFilterGroupList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (FilterID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_FilterID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            filter.FilterID = Fixture.Create<int>();
            var intType = filter.FilterID.GetType();

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

        #region General Getters/Setters : Class (Filter) => Property (FilterID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_FilterIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterIDNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameFilterID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_FilterID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterID = "FilterID";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameFilterID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (FilterName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_FilterName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            filter.FilterName = Fixture.Create<string>();
            var stringType = filter.FilterName.GetType();

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

        #region General Getters/Setters : Class (Filter) => Property (FilterName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_FilterNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterNameNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameFilterName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_FilterName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterName";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameFilterName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (GroupCompareType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_GroupCompareType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            filter.GroupCompareType = Fixture.Create<string>();
            var stringType = filter.GroupCompareType.GetType();

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

        #region General Getters/Setters : Class (Filter) => Property (GroupCompareType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_GroupCompareTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupCompareType = "GroupCompareTypeNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameGroupCompareType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_GroupCompareType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupCompareType = "GroupCompareType";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameGroupCompareType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (GroupID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_GroupID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var random = Fixture.Create<int>();

            // Act , Set
            filter.GroupID = random;

            // Assert
            filter.GroupID.ShouldBe(random);
            filter.GroupID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_GroupID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();

            // Act , Set
            filter.GroupID = null;

            // Assert
            filter.GroupID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_GroupID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameGroupID = "GroupID";
            var filter = Fixture.Create<Filter>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameGroupID);

            // Act , Set
            propertyInfo.SetValue(filter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filter.GroupID.ShouldBeNull();
            filter.GroupID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var random = Fixture.Create<bool>();

            // Act , Set
            filter.IsDeleted = random;

            // Assert
            filter.IsDeleted.ShouldBe(random);
            filter.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();

            // Act , Set
            filter.IsDeleted = null;

            // Assert
            filter.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var filter = Fixture.Create<Filter>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(filter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filter.IsDeleted.ShouldBeNull();
            filter.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var filter = Fixture.Create<Filter>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(filter, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            var random = Fixture.Create<int>();

            // Act , Set
            filter.UpdatedUserID = random;

            // Assert
            filter.UpdatedUserID.ShouldBe(random);
            filter.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();

            // Act , Set
            filter.UpdatedUserID = null;

            // Assert
            filter.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var filter = Fixture.Create<Filter>();
            var propertyInfo = filter.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(filter, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            filter.UpdatedUserID.ShouldBeNull();
            filter.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Filter) => Property (WhereClause) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_WhereClause_Property_String_Type_Verify_Test()
        {
            // Arrange
            var filter = Fixture.Create<Filter>();
            filter.WhereClause = Fixture.Create<string>();
            var stringType = filter.WhereClause.GetType();

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

        #region General Getters/Setters : Class (Filter) => Property (WhereClause) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_Class_Invalid_Property_WhereClauseNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWhereClause = "WhereClauseNotPresent";
            var filter  = Fixture.Create<Filter>();

            // Act , Assert
            Should.NotThrow(() => filter.GetType().GetProperty(propertyNameWhereClause));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Filter_WhereClause_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWhereClause = "WhereClause";
            var filter  = Fixture.Create<Filter>();
            var propertyInfo  = filter.GetType().GetProperty(propertyNameWhereClause);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Filter) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Filter_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Filter());
        }

        #endregion

        #region General Constructor : Class (Filter) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Filter_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfFilter = Fixture.CreateMany<Filter>(2).ToList();
            var firstFilter = instancesOfFilter.FirstOrDefault();
            var lastFilter = instancesOfFilter.Last();

            // Act, Assert
            firstFilter.ShouldNotBeNull();
            lastFilter.ShouldNotBeNull();
            firstFilter.ShouldNotBeSameAs(lastFilter);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Filter_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstFilter = new Filter();
            var secondFilter = new Filter();
            var thirdFilter = new Filter();
            var fourthFilter = new Filter();
            var fifthFilter = new Filter();
            var sixthFilter = new Filter();

            // Act, Assert
            firstFilter.ShouldNotBeNull();
            secondFilter.ShouldNotBeNull();
            thirdFilter.ShouldNotBeNull();
            fourthFilter.ShouldNotBeNull();
            fifthFilter.ShouldNotBeNull();
            sixthFilter.ShouldNotBeNull();
            firstFilter.ShouldNotBeSameAs(secondFilter);
            thirdFilter.ShouldNotBeSameAs(firstFilter);
            fourthFilter.ShouldNotBeSameAs(firstFilter);
            fifthFilter.ShouldNotBeSameAs(firstFilter);
            sixthFilter.ShouldNotBeSameAs(firstFilter);
            sixthFilter.ShouldNotBeSameAs(fourthFilter);
        }

        #endregion

        #region General Constructor : Class (Filter) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Filter_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var filterId = -1;
            var filterName = string.Empty;
            var whereClause = string.Empty;
            var dynamicWhere = string.Empty;
            var groupCompareType = string.Empty;
            var archived = false;

            // Act
            var filter = new Filter();

            // Assert
            filter.FilterID.ShouldBe(filterId);
            filter.CustomerID.ShouldBeNull();
            filter.GroupID.ShouldBeNull();
            filter.FilterName.ShouldBe(filterName);
            filter.WhereClause.ShouldBe(whereClause);
            filter.DynamicWhere.ShouldBe(dynamicWhere);
            filter.GroupCompareType.ShouldBe(groupCompareType);
            filter.CreatedDate.ShouldBeNull();
            filter.UpdatedDate.ShouldBeNull();
            filter.CreatedUserID.ShouldBeNull();
            filter.UpdatedUserID.ShouldBeNull();
            filter.IsDeleted.ShouldBeNull();
            filter.FilterGroupList.ShouldBeNull();
            filter.Archived.ShouldBe(archived);
        }

        #endregion

        #endregion

        #endregion
    }
}