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
    public class CodeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Code) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var codeId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var codeType = Fixture.Create<string>();
            var codeValue = Fixture.Create<string>();
            var codeDisplay = Fixture.Create<string>();
            var sortOrder = Fixture.Create<int?>();
            var displayFlag = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            code.CodeID = codeId;
            code.CustomerID = customerId;
            code.CodeType = codeType;
            code.CodeValue = codeValue;
            code.CodeDisplay = codeDisplay;
            code.SortOrder = sortOrder;
            code.DisplayFlag = displayFlag;
            code.CreatedUserID = createdUserId;
            code.UpdatedUserID = updatedUserId;
            code.IsDeleted = isDeleted;

            // Assert
            code.CodeID.ShouldBe(codeId);
            code.CustomerID.ShouldBe(customerId);
            code.CodeType.ShouldBe(codeType);
            code.CodeValue.ShouldBe(codeValue);
            code.CodeDisplay.ShouldBe(codeDisplay);
            code.SortOrder.ShouldBe(sortOrder);
            code.DisplayFlag.ShouldBe(displayFlag);
            code.CreatedUserID.ShouldBe(createdUserId);
            code.CreatedDate.ShouldBeNull();
            code.UpdatedUserID.ShouldBe(updatedUserId);
            code.UpdatedDate.ShouldBeNull();
            code.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CodeDisplay) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeDisplay_Property_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            code.CodeDisplay = Fixture.Create<string>();
            var stringType = code.CodeDisplay.GetType();

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

        #region General Getters/Setters : Class (Code) => Property (CodeDisplay) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeDisplayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeDisplay = "CodeDisplayNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCodeDisplay));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeDisplay_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCodeDisplay = "CodeDisplay";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCodeDisplay);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CodeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            code.CodeID = Fixture.Create<int>();
            var intType = code.CodeID.GetType();

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

        #region General Getters/Setters : Class (Code) => Property (CodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeID = "CodeIDNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCodeID = "CodeID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CodeType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            code.CodeType = Fixture.Create<string>();
            var stringType = code.CodeType.GetType();

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

        #region General Getters/Setters : Class (Code) => Property (CodeType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeType = "CodeTypeNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCodeType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCodeType = "CodeType";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCodeType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CodeValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            code.CodeValue = Fixture.Create<string>();
            var stringType = code.CodeValue.GetType();

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

        #region General Getters/Setters : Class (Code) => Property (CodeValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeValue = "CodeValueNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCodeValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CodeValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCodeValue = "CodeValue";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCodeValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var code = Fixture.Create<Code>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = code.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(code, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var random = Fixture.Create<int>();

            // Act , Set
            code.CreatedUserID = random;

            // Assert
            code.CreatedUserID.ShouldBe(random);
            code.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.CreatedUserID = null;

            // Assert
            code.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.CreatedUserID.ShouldBeNull();
            code.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var random = Fixture.Create<int>();

            // Act , Set
            code.CustomerID = random;

            // Assert
            code.CustomerID.ShouldBe(random);
            code.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.CustomerID = null;

            // Assert
            code.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.CustomerID.ShouldBeNull();
            code.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (DisplayFlag) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_DisplayFlag_Property_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            code.DisplayFlag = Fixture.Create<string>();
            var stringType = code.DisplayFlag.GetType();

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

        #region General Getters/Setters : Class (Code) => Property (DisplayFlag) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_DisplayFlagNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayFlag = "DisplayFlagNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameDisplayFlag));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_DisplayFlag_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayFlag = "DisplayFlag";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameDisplayFlag);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var random = Fixture.Create<bool>();

            // Act , Set
            code.IsDeleted = random;

            // Assert
            code.IsDeleted.ShouldBe(random);
            code.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.IsDeleted = null;

            // Assert
            code.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.IsDeleted.ShouldBeNull();
            code.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (SortOrder) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_SortOrder_Property_Data_Without_Null_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var random = Fixture.Create<int>();

            // Act , Set
            code.SortOrder = random;

            // Assert
            code.SortOrder.ShouldBe(random);
            code.SortOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_SortOrder_Property_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.SortOrder = null;

            // Assert
            code.SortOrder.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_SortOrder_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSortOrder = "SortOrder";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(propertyNameSortOrder);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.SortOrder.ShouldBeNull();
            code.SortOrder.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (SortOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_SortOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrderNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameSortOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_SortOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrder";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var code = Fixture.Create<Code>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = code.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(code, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var random = Fixture.Create<int>();

            // Act , Set
            code.UpdatedUserID = random;

            // Assert
            code.UpdatedUserID.ShouldBe(random);
            code.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.UpdatedUserID = null;

            // Assert
            code.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.UpdatedUserID.ShouldBeNull();
            code.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Code) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Code) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Code_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Code());
        }

        #endregion

        #region General Constructor : Class (Code) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Code_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCode = Fixture.CreateMany<Code>(2).ToList();
            var firstCode = instancesOfCode.FirstOrDefault();
            var lastCode = instancesOfCode.Last();

            // Act, Assert
            firstCode.ShouldNotBeNull();
            lastCode.ShouldNotBeNull();
            firstCode.ShouldNotBeSameAs(lastCode);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Code_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCode = new Code();
            var secondCode = new Code();
            var thirdCode = new Code();
            var fourthCode = new Code();
            var fifthCode = new Code();
            var sixthCode = new Code();

            // Act, Assert
            firstCode.ShouldNotBeNull();
            secondCode.ShouldNotBeNull();
            thirdCode.ShouldNotBeNull();
            fourthCode.ShouldNotBeNull();
            fifthCode.ShouldNotBeNull();
            sixthCode.ShouldNotBeNull();
            firstCode.ShouldNotBeSameAs(secondCode);
            thirdCode.ShouldNotBeSameAs(firstCode);
            fourthCode.ShouldNotBeSameAs(firstCode);
            fifthCode.ShouldNotBeSameAs(firstCode);
            sixthCode.ShouldNotBeSameAs(firstCode);
            sixthCode.ShouldNotBeSameAs(fourthCode);
        }

        #endregion

        #region General Constructor : Class (Code) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Code_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var codeId = -1;
            var codeType = string.Empty;
            var codeValue = string.Empty;
            var codeDisplay = string.Empty;
            var displayFlag = string.Empty;

            // Act
            var code = new Code();

            // Assert
            code.CodeID.ShouldBe(codeId);
            code.CustomerID.ShouldBeNull();
            code.CodeType.ShouldBe(codeType);
            code.CodeValue.ShouldBe(codeValue);
            code.CodeDisplay.ShouldBe(codeDisplay);
            code.SortOrder.ShouldBeNull();
            code.DisplayFlag.ShouldBe(displayFlag);
            code.CreatedUserID.ShouldBeNull();
            code.CreatedDate.ShouldBeNull();
            code.UpdatedUserID.ShouldBeNull();
            code.UpdatedDate.ShouldBeNull();
            code.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}