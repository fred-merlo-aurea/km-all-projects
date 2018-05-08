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
    public class BlastFieldsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastFields) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            var blastId = Fixture.Create<int>();
            var field1 = Fixture.Create<string>();
            var field2 = Fixture.Create<string>();
            var field3 = Fixture.Create<string>();
            var field4 = Fixture.Create<string>();
            var field5 = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var customerId = Fixture.Create<int?>();

            // Act
            blastFields.BlastID = blastId;
            blastFields.Field1 = field1;
            blastFields.Field2 = field2;
            blastFields.Field3 = field3;
            blastFields.Field4 = field4;
            blastFields.Field5 = field5;
            blastFields.CreatedUserID = createdUserId;
            blastFields.UpdatedUserID = updatedUserId;
            blastFields.IsDeleted = isDeleted;
            blastFields.CustomerID = customerId;

            // Assert
            blastFields.BlastID.ShouldBe(blastId);
            blastFields.Field1.ShouldBe(field1);
            blastFields.Field2.ShouldBe(field2);
            blastFields.Field3.ShouldBe(field3);
            blastFields.Field4.ShouldBe(field4);
            blastFields.Field5.ShouldBe(field5);
            blastFields.CreatedUserID.ShouldBe(createdUserId);
            blastFields.CreatedDate.ShouldBeNull();
            blastFields.UpdatedUserID.ShouldBe(updatedUserId);
            blastFields.UpdatedDate.ShouldBeNull();
            blastFields.IsDeleted.ShouldBe(isDeleted);
            blastFields.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            blastFields.BlastID = Fixture.Create<int>();
            var intType = blastFields.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastFields) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastFields = Fixture.Create<BlastFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastFields.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFields.CreatedUserID = random;

            // Assert
            blastFields.CreatedUserID.ShouldBe(random);
            blastFields.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();    

            // Act , Set
            blastFields.CreatedUserID = null;

            // Assert
            blastFields.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastFields = Fixture.Create<BlastFields>();
            var propertyInfo = blastFields.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFields.CreatedUserID.ShouldBeNull();
            blastFields.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFields.CustomerID = random;

            // Assert
            blastFields.CustomerID.ShouldBe(random);
            blastFields.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();    

            // Act , Set
            blastFields.CustomerID = null;

            // Assert
            blastFields.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var blastFields = Fixture.Create<BlastFields>();
            var propertyInfo = blastFields.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(blastFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFields.CustomerID.ShouldBeNull();
            blastFields.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (Field1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            blastFields.Field1 = Fixture.Create<string>();
            var stringType = blastFields.Field1.GetType();

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

        #region General Getters/Setters : Class (BlastFields) => Property (Field1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_Field1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField1 = "Field1NotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameField1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField1 = "Field1";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameField1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (Field2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            blastFields.Field2 = Fixture.Create<string>();
            var stringType = blastFields.Field2.GetType();

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

        #region General Getters/Setters : Class (BlastFields) => Property (Field2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_Field2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField2 = "Field2NotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameField2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField2 = "Field2";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameField2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (Field3) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field3_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            blastFields.Field3 = Fixture.Create<string>();
            var stringType = blastFields.Field3.GetType();

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

        #region General Getters/Setters : Class (BlastFields) => Property (Field3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_Field3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField3 = "Field3NotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameField3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField3 = "Field3";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameField3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (Field4) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field4_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            blastFields.Field4 = Fixture.Create<string>();
            var stringType = blastFields.Field4.GetType();

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

        #region General Getters/Setters : Class (BlastFields) => Property (Field4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_Field4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField4 = "Field4NotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameField4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField4 = "Field4";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameField4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (Field5) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field5_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            blastFields.Field5 = Fixture.Create<string>();
            var stringType = blastFields.Field5.GetType();

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

        #region General Getters/Setters : Class (BlastFields) => Property (Field5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_Field5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField5 = "Field5NotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameField5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Field5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField5 = "Field5";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameField5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            var random = Fixture.Create<bool>();

            // Act , Set
            blastFields.IsDeleted = random;

            // Assert
            blastFields.IsDeleted.ShouldBe(random);
            blastFields.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();    

            // Act , Set
            blastFields.IsDeleted = null;

            // Assert
            blastFields.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var blastFields = Fixture.Create<BlastFields>();
            var propertyInfo = blastFields.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(blastFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFields.IsDeleted.ShouldBeNull();
            blastFields.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastFields = Fixture.Create<BlastFields>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastFields.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastFields, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastFields.UpdatedUserID = random;

            // Assert
            blastFields.UpdatedUserID.ShouldBe(random);
            blastFields.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastFields = Fixture.Create<BlastFields>();    

            // Act , Set
            blastFields.UpdatedUserID = null;

            // Assert
            blastFields.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastFields = Fixture.Create<BlastFields>();
            var propertyInfo = blastFields.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(blastFields, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastFields.UpdatedUserID.ShouldBeNull();
            blastFields.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastFields) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var blastFields  = Fixture.Create<BlastFields>();

            // Act , Assert
            Should.NotThrow(() => blastFields.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastFields_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var blastFields  = Fixture.Create<BlastFields>();
            var propertyInfo  = blastFields.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastFields) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFields_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastFields());
        }

        #endregion

        #region General Constructor : Class (BlastFields) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFields_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastFields = Fixture.CreateMany<BlastFields>(2).ToList();
            var firstBlastFields = instancesOfBlastFields.FirstOrDefault();
            var lastBlastFields = instancesOfBlastFields.Last();

            // Act, Assert
            firstBlastFields.ShouldNotBeNull();
            lastBlastFields.ShouldNotBeNull();
            firstBlastFields.ShouldNotBeSameAs(lastBlastFields);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFields_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastFields = new BlastFields();
            var secondBlastFields = new BlastFields();
            var thirdBlastFields = new BlastFields();
            var fourthBlastFields = new BlastFields();
            var fifthBlastFields = new BlastFields();
            var sixthBlastFields = new BlastFields();

            // Act, Assert
            firstBlastFields.ShouldNotBeNull();
            secondBlastFields.ShouldNotBeNull();
            thirdBlastFields.ShouldNotBeNull();
            fourthBlastFields.ShouldNotBeNull();
            fifthBlastFields.ShouldNotBeNull();
            sixthBlastFields.ShouldNotBeNull();
            firstBlastFields.ShouldNotBeSameAs(secondBlastFields);
            thirdBlastFields.ShouldNotBeSameAs(firstBlastFields);
            fourthBlastFields.ShouldNotBeSameAs(firstBlastFields);
            fifthBlastFields.ShouldNotBeSameAs(firstBlastFields);
            sixthBlastFields.ShouldNotBeSameAs(firstBlastFields);
            sixthBlastFields.ShouldNotBeSameAs(fourthBlastFields);
        }

        #endregion

        #region General Constructor : Class (BlastFields) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastFields_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var blastId = -1;
            var field1 = string.Empty;
            var field2 = string.Empty;
            var field3 = string.Empty;
            var field4 = string.Empty;
            var field5 = string.Empty;

            // Act
            var blastFields = new BlastFields();

            // Assert
            blastFields.BlastID.ShouldBe(blastId);
            blastFields.Field1.ShouldBe(field1);
            blastFields.Field2.ShouldBe(field2);
            blastFields.Field3.ShouldBe(field3);
            blastFields.Field4.ShouldBe(field4);
            blastFields.Field5.ShouldBe(field5);
            blastFields.CreatedUserID.ShouldBeNull();
            blastFields.CreatedDate.ShouldBeNull();
            blastFields.UpdatedUserID.ShouldBeNull();
            blastFields.UpdatedDate.ShouldBeNull();
            blastFields.IsDeleted.ShouldBeNull();
            blastFields.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}