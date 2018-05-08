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
    public class GroupDataFieldsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (GroupDataFields) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var groupDataFieldsId = Fixture.Create<int>();
            var groupId = Fixture.Create<int>();
            var shortName = Fixture.Create<string>();
            var longName = Fixture.Create<string>();
            var surveyId = Fixture.Create<int?>();
            var datafieldSetId = Fixture.Create<int?>();
            var isPublic = Fixture.Create<string>();
            var isPrimaryKey = Fixture.Create<bool?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();
            var dataValuesList = Fixture.Create<List<EmailDataValues>>();
            var defaultValue = Fixture.Create<GroupDataFieldsDefault>();

            // Act
            groupDataFields.GroupDataFieldsID = groupDataFieldsId;
            groupDataFields.GroupID = groupId;
            groupDataFields.ShortName = shortName;
            groupDataFields.LongName = longName;
            groupDataFields.SurveyID = surveyId;
            groupDataFields.DatafieldSetID = datafieldSetId;
            groupDataFields.IsPublic = isPublic;
            groupDataFields.IsPrimaryKey = isPrimaryKey;
            groupDataFields.CreatedUserID = createdUserId;
            groupDataFields.UpdatedUserID = updatedUserId;
            groupDataFields.IsDeleted = isDeleted;
            groupDataFields.CustomerID = customerId;
            groupDataFields.DataValuesList = dataValuesList;
            groupDataFields.DefaultValue = defaultValue;

            // Assert
            groupDataFields.GroupDataFieldsID.ShouldBe(groupDataFieldsId);
            groupDataFields.GroupID.ShouldBe(groupId);
            groupDataFields.ShortName.ShouldBe(shortName);
            groupDataFields.LongName.ShouldBe(longName);
            groupDataFields.SurveyID.ShouldBe(surveyId);
            groupDataFields.DatafieldSetID.ShouldBe(datafieldSetId);
            groupDataFields.IsPublic.ShouldBe(isPublic);
            groupDataFields.IsPrimaryKey.ShouldBe(isPrimaryKey);
            groupDataFields.CreatedUserID.ShouldBe(createdUserId);
            groupDataFields.CreatedDate.ShouldBeNull();
            groupDataFields.UpdatedUserID.ShouldBe(updatedUserId);
            groupDataFields.UpdatedDate.ShouldBeNull();
            groupDataFields.IsDeleted.ShouldBe(isDeleted);
            groupDataFields.CustomerID.ShouldBe(customerId);
            groupDataFields.DataValuesList.ShouldBe(dataValuesList);
            groupDataFields.DefaultValue.ShouldBe(defaultValue);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupDataFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupDataFields.CreatedUserID = random;

            // Assert
            groupDataFields.CreatedUserID.ShouldBe(random);
            groupDataFields.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.CreatedUserID = null;

            // Assert
            groupDataFields.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.CreatedUserID.ShouldBeNull();
            groupDataFields.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupDataFields.CustomerID = random;

            // Assert
            groupDataFields.CustomerID.ShouldBe(random);
            groupDataFields.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.CustomerID = null;

            // Assert
            groupDataFields.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.CustomerID.ShouldBeNull();
            groupDataFields.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (DatafieldSetID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DatafieldSetID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupDataFields.DatafieldSetID = random;

            // Assert
            groupDataFields.DatafieldSetID.ShouldBe(random);
            groupDataFields.DatafieldSetID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DatafieldSetID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.DatafieldSetID = null;

            // Assert
            groupDataFields.DatafieldSetID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DatafieldSetID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDatafieldSetID = "DatafieldSetID";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameDatafieldSetID);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.DatafieldSetID.ShouldBeNull();
            groupDataFields.DatafieldSetID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (DatafieldSetID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_DatafieldSetIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDatafieldSetID = "DatafieldSetIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameDatafieldSetID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DatafieldSetID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDatafieldSetID = "DatafieldSetID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameDatafieldSetID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (DataValuesList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_DataValuesListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDataValuesList = "DataValuesListNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameDataValuesList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DataValuesList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDataValuesList = "DataValuesList";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameDataValuesList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (DefaultValue) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DefaultValue_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDefaultValue = "DefaultValue";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameDefaultValue);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupDataFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (DefaultValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_DefaultValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDefaultValue = "DefaultValueNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameDefaultValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_DefaultValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDefaultValue = "DefaultValue";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameDefaultValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (GroupDataFieldsID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_GroupDataFieldsID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            groupDataFields.GroupDataFieldsID = Fixture.Create<int>();
            var intType = groupDataFields.GroupDataFieldsID.GetType();

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

        #region General Getters/Setters : Class (GroupDataFields) => Property (GroupDataFieldsID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_GroupDataFieldsIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupDataFieldsID = "GroupDataFieldsIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameGroupDataFieldsID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_GroupDataFieldsID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupDataFieldsID = "GroupDataFieldsID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameGroupDataFieldsID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            groupDataFields.GroupID = Fixture.Create<int>();
            var intType = groupDataFields.GroupID.GetType();

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

        #region General Getters/Setters : Class (GroupDataFields) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<bool>();

            // Act , Set
            groupDataFields.IsDeleted = random;

            // Assert
            groupDataFields.IsDeleted.ShouldBe(random);
            groupDataFields.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.IsDeleted = null;

            // Assert
            groupDataFields.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.IsDeleted.ShouldBeNull();
            groupDataFields.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (IsPrimaryKey) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsPrimaryKey_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<bool>();

            // Act , Set
            groupDataFields.IsPrimaryKey = random;

            // Assert
            groupDataFields.IsPrimaryKey.ShouldBe(random);
            groupDataFields.IsPrimaryKey.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsPrimaryKey_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.IsPrimaryKey = null;

            // Assert
            groupDataFields.IsPrimaryKey.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsPrimaryKey_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsPrimaryKey = "IsPrimaryKey";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameIsPrimaryKey);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.IsPrimaryKey.ShouldBeNull();
            groupDataFields.IsPrimaryKey.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (IsPrimaryKey) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_IsPrimaryKeyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsPrimaryKey = "IsPrimaryKeyNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameIsPrimaryKey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsPrimaryKey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsPrimaryKey = "IsPrimaryKey";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameIsPrimaryKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (IsPublic) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsPublic_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            groupDataFields.IsPublic = Fixture.Create<string>();
            var stringType = groupDataFields.IsPublic.GetType();

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

        #region General Getters/Setters : Class (GroupDataFields) => Property (IsPublic) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_IsPublicNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsPublic = "IsPublicNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameIsPublic));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_IsPublic_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsPublic = "IsPublic";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameIsPublic);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (LongName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_LongName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            groupDataFields.LongName = Fixture.Create<string>();
            var stringType = groupDataFields.LongName.GetType();

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

        #region General Getters/Setters : Class (GroupDataFields) => Property (LongName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_LongNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLongName = "LongNameNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameLongName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_LongName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLongName = "LongName";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameLongName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (ShortName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_ShortName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            groupDataFields.ShortName = Fixture.Create<string>();
            var stringType = groupDataFields.ShortName.GetType();

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

        #region General Getters/Setters : Class (GroupDataFields) => Property (ShortName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_ShortNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShortName = "ShortNameNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameShortName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_ShortName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShortName = "ShortName";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameShortName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (SurveyID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_SurveyID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupDataFields.SurveyID = random;

            // Assert
            groupDataFields.SurveyID.ShouldBe(random);
            groupDataFields.SurveyID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_SurveyID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.SurveyID = null;

            // Assert
            groupDataFields.SurveyID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_SurveyID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSurveyID = "SurveyID";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameSurveyID);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.SurveyID.ShouldBeNull();
            groupDataFields.SurveyID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (SurveyID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_SurveyIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameSurveyID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_SurveyID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSurveyID = "SurveyID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameSurveyID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupDataFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            groupDataFields.UpdatedUserID = random;

            // Assert
            groupDataFields.UpdatedUserID.ShouldBe(random);
            groupDataFields.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var groupDataFields = Fixture.Create<GroupDataFields>();

            // Act , Set
            groupDataFields.UpdatedUserID = null;

            // Assert
            groupDataFields.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var groupDataFields = Fixture.Create<GroupDataFields>();
            var propertyInfo = groupDataFields.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(groupDataFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            groupDataFields.UpdatedUserID.ShouldBeNull();
            groupDataFields.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GroupDataFields) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var groupDataFields  = Fixture.Create<GroupDataFields>();

            // Act , Assert
            Should.NotThrow(() => groupDataFields.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupDataFields_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var groupDataFields  = Fixture.Create<GroupDataFields>();
            var propertyInfo  = groupDataFields.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (GroupDataFields) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFields_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GroupDataFields());
        }

        #endregion

        #region General Constructor : Class (GroupDataFields) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFields_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGroupDataFields = Fixture.CreateMany<GroupDataFields>(2).ToList();
            var firstGroupDataFields = instancesOfGroupDataFields.FirstOrDefault();
            var lastGroupDataFields = instancesOfGroupDataFields.Last();

            // Act, Assert
            firstGroupDataFields.ShouldNotBeNull();
            lastGroupDataFields.ShouldNotBeNull();
            firstGroupDataFields.ShouldNotBeSameAs(lastGroupDataFields);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFields_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGroupDataFields = new GroupDataFields();
            var secondGroupDataFields = new GroupDataFields();
            var thirdGroupDataFields = new GroupDataFields();
            var fourthGroupDataFields = new GroupDataFields();
            var fifthGroupDataFields = new GroupDataFields();
            var sixthGroupDataFields = new GroupDataFields();

            // Act, Assert
            firstGroupDataFields.ShouldNotBeNull();
            secondGroupDataFields.ShouldNotBeNull();
            thirdGroupDataFields.ShouldNotBeNull();
            fourthGroupDataFields.ShouldNotBeNull();
            fifthGroupDataFields.ShouldNotBeNull();
            sixthGroupDataFields.ShouldNotBeNull();
            firstGroupDataFields.ShouldNotBeSameAs(secondGroupDataFields);
            thirdGroupDataFields.ShouldNotBeSameAs(firstGroupDataFields);
            fourthGroupDataFields.ShouldNotBeSameAs(firstGroupDataFields);
            fifthGroupDataFields.ShouldNotBeSameAs(firstGroupDataFields);
            sixthGroupDataFields.ShouldNotBeSameAs(firstGroupDataFields);
            sixthGroupDataFields.ShouldNotBeSameAs(fourthGroupDataFields);
        }

        #endregion

        #region General Constructor : Class (GroupDataFields) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupDataFields_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var groupDataFieldsId = -1;
            var groupId = -1;
            var shortName = string.Empty;
            var longName = string.Empty;
            var isPublic = string.Empty;

            // Act
            var groupDataFields = new GroupDataFields();

            // Assert
            groupDataFields.GroupDataFieldsID.ShouldBe(groupDataFieldsId);
            groupDataFields.GroupID.ShouldBe(groupId);
            groupDataFields.ShortName.ShouldBe(shortName);
            groupDataFields.LongName.ShouldBe(longName);
            groupDataFields.SurveyID.ShouldBeNull();
            groupDataFields.DatafieldSetID.ShouldBeNull();
            groupDataFields.IsPublic.ShouldBe(isPublic);
            groupDataFields.IsPrimaryKey.ShouldBeNull();
            groupDataFields.CreatedUserID.ShouldBeNull();
            groupDataFields.CreatedDate.ShouldBeNull();
            groupDataFields.UpdatedUserID.ShouldBeNull();
            groupDataFields.UpdatedDate.ShouldBeNull();
            groupDataFields.IsDeleted.ShouldBeNull();
            groupDataFields.CustomerID.ShouldBeNull();
            groupDataFields.DataValuesList.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}