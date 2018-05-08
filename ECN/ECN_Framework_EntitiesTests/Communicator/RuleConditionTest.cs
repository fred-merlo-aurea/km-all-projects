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
    public class RuleConditionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (RuleCondition) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            var ruleConditionId = Fixture.Create<int>();
            var ruleId = Fixture.Create<int?>();
            var field = Fixture.Create<string>();
            var dataType = Fixture.Create<string>();
            var comparator = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            ruleCondition.RuleConditionID = ruleConditionId;
            ruleCondition.RuleID = ruleId;
            ruleCondition.Field = field;
            ruleCondition.DataType = dataType;
            ruleCondition.Comparator = comparator;
            ruleCondition.Value = value;
            ruleCondition.CreatedUserID = createdUserId;
            ruleCondition.UpdatedUserID = updatedUserId;
            ruleCondition.IsDeleted = isDeleted;

            // Assert
            ruleCondition.RuleConditionID.ShouldBe(ruleConditionId);
            ruleCondition.RuleID.ShouldBe(ruleId);
            ruleCondition.Field.ShouldBe(field);
            ruleCondition.DataType.ShouldBe(dataType);
            ruleCondition.Comparator.ShouldBe(comparator);
            ruleCondition.Value.ShouldBe(value);
            ruleCondition.CreatedUserID.ShouldBe(createdUserId);
            ruleCondition.CreatedDate.ShouldBeNull();
            ruleCondition.UpdatedUserID.ShouldBe(updatedUserId);
            ruleCondition.UpdatedDate.ShouldBeNull();
            ruleCondition.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (Comparator) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Comparator_Property_String_Type_Verify_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            ruleCondition.Comparator = Fixture.Create<string>();
            var stringType = ruleCondition.Comparator.GetType();

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

        #region General Getters/Setters : Class (RuleCondition) => Property (Comparator) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_ComparatorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComparator = "ComparatorNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameComparator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Comparator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComparator = "Comparator";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameComparator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var ruleCondition = Fixture.Create<RuleCondition>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = ruleCondition.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(ruleCondition, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            ruleCondition.CreatedUserID = random;

            // Assert
            ruleCondition.CreatedUserID.ShouldBe(random);
            ruleCondition.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();

            // Act , Set
            ruleCondition.CreatedUserID = null;

            // Assert
            ruleCondition.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var ruleCondition = Fixture.Create<RuleCondition>();
            var propertyInfo = ruleCondition.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(ruleCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            ruleCondition.CreatedUserID.ShouldBeNull();
            ruleCondition.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (DataType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_DataType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            ruleCondition.DataType = Fixture.Create<string>();
            var stringType = ruleCondition.DataType.GetType();

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

        #region General Getters/Setters : Class (RuleCondition) => Property (DataType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_DataTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDataType = "DataTypeNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameDataType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_DataType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDataType = "DataType";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameDataType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (Field) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Field_Property_String_Type_Verify_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            ruleCondition.Field = Fixture.Create<string>();
            var stringType = ruleCondition.Field.GetType();

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

        #region General Getters/Setters : Class (RuleCondition) => Property (Field) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_FieldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField = "FieldNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameField));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Field_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField = "Field";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameField);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            var random = Fixture.Create<bool>();

            // Act , Set
            ruleCondition.IsDeleted = random;

            // Assert
            ruleCondition.IsDeleted.ShouldBe(random);
            ruleCondition.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();

            // Act , Set
            ruleCondition.IsDeleted = null;

            // Assert
            ruleCondition.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var ruleCondition = Fixture.Create<RuleCondition>();
            var propertyInfo = ruleCondition.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(ruleCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            ruleCondition.IsDeleted.ShouldBeNull();
            ruleCondition.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (RuleConditionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_RuleConditionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            ruleCondition.RuleConditionID = Fixture.Create<int>();
            var intType = ruleCondition.RuleConditionID.GetType();

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

        #region General Getters/Setters : Class (RuleCondition) => Property (RuleConditionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_RuleConditionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRuleConditionID = "RuleConditionIDNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameRuleConditionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_RuleConditionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRuleConditionID = "RuleConditionID";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameRuleConditionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (RuleID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_RuleID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            ruleCondition.RuleID = random;

            // Assert
            ruleCondition.RuleID.ShouldBe(random);
            ruleCondition.RuleID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_RuleID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();

            // Act , Set
            ruleCondition.RuleID = null;

            // Assert
            ruleCondition.RuleID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_RuleID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameRuleID = "RuleID";
            var ruleCondition = Fixture.Create<RuleCondition>();
            var propertyInfo = ruleCondition.GetType().GetProperty(propertyNameRuleID);

            // Act , Set
            propertyInfo.SetValue(ruleCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            ruleCondition.RuleID.ShouldBeNull();
            ruleCondition.RuleID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (RuleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_RuleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRuleID = "RuleIDNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameRuleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_RuleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRuleID = "RuleID";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameRuleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var ruleCondition = Fixture.Create<RuleCondition>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = ruleCondition.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(ruleCondition, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            var random = Fixture.Create<int>();

            // Act , Set
            ruleCondition.UpdatedUserID = random;

            // Assert
            ruleCondition.UpdatedUserID.ShouldBe(random);
            ruleCondition.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();

            // Act , Set
            ruleCondition.UpdatedUserID = null;

            // Assert
            ruleCondition.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var ruleCondition = Fixture.Create<RuleCondition>();
            var propertyInfo = ruleCondition.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(ruleCondition, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            ruleCondition.UpdatedUserID.ShouldBeNull();
            ruleCondition.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (RuleCondition) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var ruleCondition = Fixture.Create<RuleCondition>();
            ruleCondition.Value = Fixture.Create<string>();
            var stringType = ruleCondition.Value.GetType();

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

        #region General Getters/Setters : Class (RuleCondition) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var ruleCondition  = Fixture.Create<RuleCondition>();

            // Act , Assert
            Should.NotThrow(() => ruleCondition.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RuleCondition_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var ruleCondition  = Fixture.Create<RuleCondition>();
            var propertyInfo  = ruleCondition.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (RuleCondition) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RuleCondition_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new RuleCondition());
        }

        #endregion

        #region General Constructor : Class (RuleCondition) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RuleCondition_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfRuleCondition = Fixture.CreateMany<RuleCondition>(2).ToList();
            var firstRuleCondition = instancesOfRuleCondition.FirstOrDefault();
            var lastRuleCondition = instancesOfRuleCondition.Last();

            // Act, Assert
            firstRuleCondition.ShouldNotBeNull();
            lastRuleCondition.ShouldNotBeNull();
            firstRuleCondition.ShouldNotBeSameAs(lastRuleCondition);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RuleCondition_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstRuleCondition = new RuleCondition();
            var secondRuleCondition = new RuleCondition();
            var thirdRuleCondition = new RuleCondition();
            var fourthRuleCondition = new RuleCondition();
            var fifthRuleCondition = new RuleCondition();
            var sixthRuleCondition = new RuleCondition();

            // Act, Assert
            firstRuleCondition.ShouldNotBeNull();
            secondRuleCondition.ShouldNotBeNull();
            thirdRuleCondition.ShouldNotBeNull();
            fourthRuleCondition.ShouldNotBeNull();
            fifthRuleCondition.ShouldNotBeNull();
            sixthRuleCondition.ShouldNotBeNull();
            firstRuleCondition.ShouldNotBeSameAs(secondRuleCondition);
            thirdRuleCondition.ShouldNotBeSameAs(firstRuleCondition);
            fourthRuleCondition.ShouldNotBeSameAs(firstRuleCondition);
            fifthRuleCondition.ShouldNotBeSameAs(firstRuleCondition);
            sixthRuleCondition.ShouldNotBeSameAs(firstRuleCondition);
            sixthRuleCondition.ShouldNotBeSameAs(fourthRuleCondition);
        }

        #endregion

        #region General Constructor : Class (RuleCondition) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_RuleCondition_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var ruleConditionId = -1;

            // Act
            var ruleCondition = new RuleCondition();

            // Assert
            ruleCondition.RuleConditionID.ShouldBe(ruleConditionId);
            ruleCondition.RuleID.ShouldBeNull();
            ruleCondition.Field.ShouldBeNull();
            ruleCondition.DataType.ShouldBeNull();
            ruleCondition.Comparator.ShouldBeNull();
            ruleCondition.Value.ShouldBeNull();
            ruleCondition.CreatedUserID.ShouldBeNull();
            ruleCondition.CreatedDate.ShouldBeNull();
            ruleCondition.UpdatedUserID.ShouldBeNull();
            ruleCondition.UpdatedDate.ShouldBeNull();
            ruleCondition.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}