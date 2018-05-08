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
    public class GatewayValueTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (GatewayValue) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            var gatewayValueId = Fixture.Create<int>();
            var gatewayId = Fixture.Create<int>();
            var field = Fixture.Create<string>();
            var isLoginValidator = Fixture.Create<bool>();
            var isCaptureValue = Fixture.Create<bool>();
            var isStatic = Fixture.Create<bool>();
            var label = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool>();
            var comparator = Fixture.Create<string>();
            var nOT = Fixture.Create<bool>();
            var fieldType = Fixture.Create<string>();
            var datePart = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var hasFailed = Fixture.Create<bool>();
            var isBlank = Fixture.Create<bool>();

            // Act
            gatewayValue.GatewayValueID = gatewayValueId;
            gatewayValue.GatewayID = gatewayId;
            gatewayValue.Field = field;
            gatewayValue.IsLoginValidator = isLoginValidator;
            gatewayValue.IsCaptureValue = isCaptureValue;
            gatewayValue.IsStatic = isStatic;
            gatewayValue.Label = label;
            gatewayValue.Value = value;
            gatewayValue.IsDeleted = isDeleted;
            gatewayValue.Comparator = comparator;
            gatewayValue.NOT = nOT;
            gatewayValue.FieldType = fieldType;
            gatewayValue.DatePart = datePart;
            gatewayValue.CreatedUserID = createdUserId;
            gatewayValue.CreatedDate = createdDate;
            gatewayValue.UpdatedUserID = updatedUserId;
            gatewayValue.UpdatedDate = updatedDate;
            gatewayValue.HasFailed = hasFailed;
            gatewayValue.IsBlank = isBlank;

            // Assert
            gatewayValue.GatewayValueID.ShouldBe(gatewayValueId);
            gatewayValue.GatewayID.ShouldBe(gatewayId);
            gatewayValue.Field.ShouldBe(field);
            gatewayValue.IsLoginValidator.ShouldBe(isLoginValidator);
            gatewayValue.IsCaptureValue.ShouldBe(isCaptureValue);
            gatewayValue.IsStatic.ShouldBe(isStatic);
            gatewayValue.Label.ShouldBe(label);
            gatewayValue.Value.ShouldBe(value);
            gatewayValue.IsDeleted.ShouldBe(isDeleted);
            gatewayValue.Comparator.ShouldBe(comparator);
            gatewayValue.NOT.ShouldBe(nOT);
            gatewayValue.FieldType.ShouldBe(fieldType);
            gatewayValue.DatePart.ShouldBe(datePart);
            gatewayValue.CreatedUserID.ShouldBe(createdUserId);
            gatewayValue.CreatedDate.ShouldBe(createdDate);
            gatewayValue.UpdatedUserID.ShouldBe(updatedUserId);
            gatewayValue.UpdatedDate.ShouldBe(updatedDate);
            gatewayValue.HasFailed.ShouldBe(hasFailed);
            gatewayValue.IsBlank.ShouldBe(isBlank);
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (Comparator) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Comparator_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.Comparator = Fixture.Create<string>();
            var stringType = gatewayValue.Comparator.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (Comparator) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_ComparatorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComparator = "ComparatorNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameComparator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Comparator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComparator = "Comparator";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameComparator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var gatewayValue = Fixture.Create<GatewayValue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = gatewayValue.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(gatewayValue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            gatewayValue.CreatedUserID = random;

            // Assert
            gatewayValue.CreatedUserID.ShouldBe(random);
            gatewayValue.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();

            // Act , Set
            gatewayValue.CreatedUserID = null;

            // Assert
            gatewayValue.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var gatewayValue = Fixture.Create<GatewayValue>();
            var propertyInfo = gatewayValue.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(gatewayValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            gatewayValue.CreatedUserID.ShouldBeNull();
            gatewayValue.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (DatePart) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_DatePart_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.DatePart = Fixture.Create<string>();
            var stringType = gatewayValue.DatePart.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (DatePart) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_DatePartNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDatePart = "DatePartNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameDatePart));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_DatePart_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDatePart = "DatePart";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameDatePart);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (Field) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Field_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.Field = Fixture.Create<string>();
            var stringType = gatewayValue.Field.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (Field) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_FieldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField = "FieldNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameField));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Field_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField = "Field";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameField);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (FieldType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_FieldType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.FieldType = Fixture.Create<string>();
            var stringType = gatewayValue.FieldType.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (FieldType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_FieldTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFieldType = "FieldTypeNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameFieldType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_FieldType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFieldType = "FieldType";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameFieldType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (GatewayID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_GatewayID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.GatewayID = Fixture.Create<int>();
            var intType = gatewayValue.GatewayID.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (GatewayID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_GatewayIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGatewayID = "GatewayIDNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameGatewayID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_GatewayID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGatewayID = "GatewayID";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameGatewayID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (GatewayValueID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_GatewayValueID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.GatewayValueID = Fixture.Create<int>();
            var intType = gatewayValue.GatewayValueID.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (GatewayValueID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_GatewayValueIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGatewayValueID = "GatewayValueIDNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameGatewayValueID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_GatewayValueID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGatewayValueID = "GatewayValueID";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameGatewayValueID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (HasFailed) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_HasFailed_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.HasFailed = Fixture.Create<bool>();
            var boolType = gatewayValue.HasFailed.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (HasFailed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_HasFailedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHasFailed = "HasFailedNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameHasFailed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_HasFailed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHasFailed = "HasFailed";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameHasFailed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsBlank) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsBlank_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.IsBlank = Fixture.Create<bool>();
            var boolType = gatewayValue.IsBlank.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsBlank) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_IsBlankNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsBlank = "IsBlankNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameIsBlank));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsBlank_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsBlank = "IsBlank";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameIsBlank);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsCaptureValue) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsCaptureValue_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.IsCaptureValue = Fixture.Create<bool>();
            var boolType = gatewayValue.IsCaptureValue.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsCaptureValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_IsCaptureValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsCaptureValue = "IsCaptureValueNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameIsCaptureValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsCaptureValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsCaptureValue = "IsCaptureValue";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameIsCaptureValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.IsDeleted = Fixture.Create<bool>();
            var boolType = gatewayValue.IsDeleted.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsLoginValidator) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsLoginValidator_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.IsLoginValidator = Fixture.Create<bool>();
            var boolType = gatewayValue.IsLoginValidator.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsLoginValidator) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_IsLoginValidatorNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsLoginValidator = "IsLoginValidatorNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameIsLoginValidator));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsLoginValidator_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsLoginValidator = "IsLoginValidator";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameIsLoginValidator);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsStatic) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsStatic_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.IsStatic = Fixture.Create<bool>();
            var boolType = gatewayValue.IsStatic.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (IsStatic) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_IsStaticNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsStatic = "IsStaticNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameIsStatic));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_IsStatic_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsStatic = "IsStatic";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameIsStatic);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (Label) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Label_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.Label = Fixture.Create<string>();
            var stringType = gatewayValue.Label.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (Label) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_LabelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLabel = "LabelNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameLabel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Label_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLabel = "Label";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameLabel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (NOT) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_NOT_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.NOT = Fixture.Create<bool>();
            var boolType = gatewayValue.NOT.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (NOT) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_NOTNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNOT = "NOTNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameNOT));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_NOT_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNOT = "NOT";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameNOT);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var gatewayValue = Fixture.Create<GatewayValue>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = gatewayValue.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(gatewayValue, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            var random = Fixture.Create<int>();

            // Act , Set
            gatewayValue.UpdatedUserID = random;

            // Assert
            gatewayValue.UpdatedUserID.ShouldBe(random);
            gatewayValue.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();

            // Act , Set
            gatewayValue.UpdatedUserID = null;

            // Assert
            gatewayValue.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var gatewayValue = Fixture.Create<GatewayValue>();
            var propertyInfo = gatewayValue.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(gatewayValue, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            gatewayValue.UpdatedUserID.ShouldBeNull();
            gatewayValue.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GatewayValue) => Property (Value) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Value_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gatewayValue = Fixture.Create<GatewayValue>();
            gatewayValue.Value = Fixture.Create<string>();
            var stringType = gatewayValue.Value.GetType();

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

        #region General Getters/Setters : Class (GatewayValue) => Property (Value) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Class_Invalid_Property_ValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValue = "ValueNotPresent";
            var gatewayValue  = Fixture.Create<GatewayValue>();

            // Act , Assert
            Should.NotThrow(() => gatewayValue.GetType().GetProperty(propertyNameValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GatewayValue_Value_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValue = "Value";
            var gatewayValue  = Fixture.Create<GatewayValue>();
            var propertyInfo  = gatewayValue.GetType().GetProperty(propertyNameValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (GatewayValue) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GatewayValue_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GatewayValue());
        }

        #endregion

        #region General Constructor : Class (GatewayValue) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GatewayValue_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGatewayValue = Fixture.CreateMany<GatewayValue>(2).ToList();
            var firstGatewayValue = instancesOfGatewayValue.FirstOrDefault();
            var lastGatewayValue = instancesOfGatewayValue.Last();

            // Act, Assert
            firstGatewayValue.ShouldNotBeNull();
            lastGatewayValue.ShouldNotBeNull();
            firstGatewayValue.ShouldNotBeSameAs(lastGatewayValue);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GatewayValue_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGatewayValue = new GatewayValue();
            var secondGatewayValue = new GatewayValue();
            var thirdGatewayValue = new GatewayValue();
            var fourthGatewayValue = new GatewayValue();
            var fifthGatewayValue = new GatewayValue();
            var sixthGatewayValue = new GatewayValue();

            // Act, Assert
            firstGatewayValue.ShouldNotBeNull();
            secondGatewayValue.ShouldNotBeNull();
            thirdGatewayValue.ShouldNotBeNull();
            fourthGatewayValue.ShouldNotBeNull();
            fifthGatewayValue.ShouldNotBeNull();
            sixthGatewayValue.ShouldNotBeNull();
            firstGatewayValue.ShouldNotBeSameAs(secondGatewayValue);
            thirdGatewayValue.ShouldNotBeSameAs(firstGatewayValue);
            fourthGatewayValue.ShouldNotBeSameAs(firstGatewayValue);
            fifthGatewayValue.ShouldNotBeSameAs(firstGatewayValue);
            sixthGatewayValue.ShouldNotBeSameAs(firstGatewayValue);
            sixthGatewayValue.ShouldNotBeSameAs(fourthGatewayValue);
        }

        #endregion

        #region General Constructor : Class (GatewayValue) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GatewayValue_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var gatewayValueId = -1;
            var gatewayId = -1;
            var field = string.Empty;
            var isLoginValidator = false;
            var isCaptureValue = false;
            var isStatic = false;
            var label = "";
            var value = string.Empty;
            var isDeleted = false;
            var comparator = string.Empty;
            var nOT = false;
            var fieldType = string.Empty;
            var datePart = string.Empty;
            var hasFailed = false;
            var isBlank = false;

            // Act
            var gatewayValue = new GatewayValue();

            // Assert
            gatewayValue.GatewayValueID.ShouldBe(gatewayValueId);
            gatewayValue.GatewayID.ShouldBe(gatewayId);
            gatewayValue.Field.ShouldBe(field);
            gatewayValue.IsLoginValidator.ShouldBeFalse();
            gatewayValue.IsCaptureValue.ShouldBeFalse();
            gatewayValue.IsStatic.ShouldBeFalse();
            gatewayValue.Label.ShouldBe(label);
            gatewayValue.Value.ShouldBe(value);
            gatewayValue.IsDeleted.ShouldBeFalse();
            gatewayValue.Comparator.ShouldBe(comparator);
            gatewayValue.NOT.ShouldBeFalse();
            gatewayValue.FieldType.ShouldBe(fieldType);
            gatewayValue.DatePart.ShouldBe(datePart);
            gatewayValue.CreatedUserID.ShouldBeNull();
            gatewayValue.CreatedDate.ShouldBeNull();
            gatewayValue.UpdatedUserID.ShouldBeNull();
            gatewayValue.UpdatedDate.ShouldBeNull();
            gatewayValue.HasFailed.ShouldBeFalse();
            gatewayValue.IsBlank.ShouldBeFalse();
        }

        #endregion

        #endregion

        #endregion
    }
}