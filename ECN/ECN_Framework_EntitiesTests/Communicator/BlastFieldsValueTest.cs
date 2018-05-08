using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Communicator;

namespace ECN_Framework_Entities.Communicator
{
    [TestFixture]
    public class BlastFieldsValueTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastFieldsValue) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var blastFieldsValueId = Fixture.Create<int>();
            var blastFieldId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var value = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            blastFieldsValue.BlastFieldsValueID = blastFieldsValueId;
            blastFieldsValue.BlastFieldID = blastFieldId;
            blastFieldsValue.CustomerID = customerId;
            blastFieldsValue.Value = value;
            blastFieldsValue.CreatedUserID = createdUserId;
            blastFieldsValue.CreatedDate = createdDate;
            blastFieldsValue.UpdatedUserID = updatedUserId;
            blastFieldsValue.UpdatedDate = updatedDate;
            blastFieldsValue.IsDeleted = isDeleted;

            // Assert
            blastFieldsValue.BlastFieldsValueID.ShouldBe(blastFieldsValueId);
            blastFieldsValue.BlastFieldID.ShouldBe(blastFieldId);
            blastFieldsValue.CustomerID.ShouldBe(customerId);
            blastFieldsValue.Value.ShouldBe(value);
            blastFieldsValue.CreatedUserID.ShouldBe(createdUserId);
            blastFieldsValue.CreatedDate.ShouldBe(createdDate);
            blastFieldsValue.UpdatedUserID.ShouldBe(updatedUserId);
            blastFieldsValue.UpdatedDate.ShouldBe(updatedDate);
            blastFieldsValue.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (BlastFieldID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_BlastFieldID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            blastFieldsValue.BlastFieldID = Fixture.Create<int>();
            var intType = blastFieldsValue.BlastFieldID.GetType();

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

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (BlastFieldID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_BlastFieldIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastFieldID = "BlastFieldIDNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameBlastFieldID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_BlastFieldID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastFieldID = "BlastFieldID";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameBlastFieldID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (BlastFieldsValueID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_BlastFieldsValueID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            blastFieldsValue.BlastFieldsValueID = Fixture.Create<int>();
            var intType = blastFieldsValue.BlastFieldsValueID.GetType();

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

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (BlastFieldsValueID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_BlastFieldsValueIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastFieldsValueID = "BlastFieldsValueIDNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameBlastFieldsValueID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_BlastFieldsValueID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastFieldsValueID = "BlastFieldsValueID";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameBlastFieldsValueID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastFieldsValue.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastFieldsValue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFieldsValue.CreatedUserID = random;

            // Assert
            blastFieldsValue.CreatedUserID.ShouldBe(random);
            blastFieldsValue.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();    

            // Act , Set
            blastFieldsValue.CreatedUserID = null;

            // Assert
            blastFieldsValue.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var propertyInfo = blastFieldsValue.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastFieldsValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsValue.CreatedUserID.ShouldBeNull();
            blastFieldsValue.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFieldsValue.CustomerID = random;

            // Assert
            blastFieldsValue.CustomerID.ShouldBe(random);
            blastFieldsValue.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();    

            // Act , Set
            blastFieldsValue.CustomerID = null;

            // Assert
            blastFieldsValue.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var propertyInfo = blastFieldsValue.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(blastFieldsValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsValue.CustomerID.ShouldBeNull();
            blastFieldsValue.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastFieldsValue.IsDeleted = random;

            // Assert
            blastFieldsValue.IsDeleted.ShouldBe(random);
            blastFieldsValue.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();    

            // Act , Set
            blastFieldsValue.IsDeleted = null;

            // Assert
            blastFieldsValue.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var propertyInfo = blastFieldsValue.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(blastFieldsValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsValue.IsDeleted.ShouldBeNull();
            blastFieldsValue.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastFieldsValue.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastFieldsValue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFieldsValue.UpdatedUserID = random;

            // Assert
            blastFieldsValue.UpdatedUserID.ShouldBe(random);
            blastFieldsValue.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();    

            // Act , Set
            blastFieldsValue.UpdatedUserID = null;

            // Assert
            blastFieldsValue.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            var propertyInfo = blastFieldsValue.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastFieldsValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFieldsValue.UpdatedUserID.ShouldBeNull();
            blastFieldsValue.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFieldsValue = Fixture.Create<BlastFieldsValue>();
            blastFieldsValue.Value = Fixture.Create<string>();
            var stringType = blastFieldsValue.Value.GetType();

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

        #region General Getters/Setters : Class (BlastFieldsValue) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();

            // Act , Assert
            Should.NotThrow(() => blastFieldsValue.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFieldsValue_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var blastFieldsValue  = Fixture.Create<BlastFieldsValue>();
            var propertyInfo  = blastFieldsValue.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastFieldsValue) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFieldsValue_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastFieldsValue());
        }

        #endregion

        #region General Constructor : Class (BlastFieldsValue) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFieldsValue_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastFieldsValue = Fixture.CreateMany<BlastFieldsValue>(2).ToList();
            var firstBlastFieldsValue = instancesOfBlastFieldsValue.FirstOrDefault();
            var lastBlastFieldsValue = instancesOfBlastFieldsValue.Last();

            // Act, Assert
            firstBlastFieldsValue.ShouldNotBeNull();
            lastBlastFieldsValue.ShouldNotBeNull();
            firstBlastFieldsValue.ShouldNotBeSameAs(lastBlastFieldsValue);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFieldsValue_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastFieldsValue = new BlastFieldsValue();
            var secondBlastFieldsValue = new BlastFieldsValue();
            var thirdBlastFieldsValue = new BlastFieldsValue();
            var fourthBlastFieldsValue = new BlastFieldsValue();
            var fifthBlastFieldsValue = new BlastFieldsValue();
            var sixthBlastFieldsValue = new BlastFieldsValue();

            // Act, Assert
            firstBlastFieldsValue.ShouldNotBeNull();
            secondBlastFieldsValue.ShouldNotBeNull();
            thirdBlastFieldsValue.ShouldNotBeNull();
            fourthBlastFieldsValue.ShouldNotBeNull();
            fifthBlastFieldsValue.ShouldNotBeNull();
            sixthBlastFieldsValue.ShouldNotBeNull();
            firstBlastFieldsValue.ShouldNotBeSameAs(secondBlastFieldsValue);
            thirdBlastFieldsValue.ShouldNotBeSameAs(firstBlastFieldsValue);
            fourthBlastFieldsValue.ShouldNotBeSameAs(firstBlastFieldsValue);
            fifthBlastFieldsValue.ShouldNotBeSameAs(firstBlastFieldsValue);
            sixthBlastFieldsValue.ShouldNotBeSameAs(firstBlastFieldsValue);
            sixthBlastFieldsValue.ShouldNotBeSameAs(fourthBlastFieldsValue);
        }

        #endregion

        #region General Constructor : Class (BlastFieldsValue) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFieldsValue_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var blastFieldsValueId = -1;
            var blastFieldId = -1;
            var customerId = -1;
            var value = string.Empty;

            // Act
            var blastFieldsValue = new BlastFieldsValue();

            // Assert
            blastFieldsValue.BlastFieldsValueID.ShouldBe(blastFieldsValueId);
            blastFieldsValue.BlastFieldID.ShouldBe(blastFieldId);
            blastFieldsValue.CustomerID.ShouldBe(customerId);
            blastFieldsValue.Value.ShouldBe(value);
            blastFieldsValue.CreatedUserID.ShouldBeNull();
            blastFieldsValue.CreatedDate.ShouldBeNull();
            blastFieldsValue.UpdatedUserID.ShouldBeNull();
            blastFieldsValue.UpdatedDate.ShouldBeNull();
            blastFieldsValue.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}