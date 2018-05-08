using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class CodeTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var code  = new Code();
            var codeID = Fixture.Create<int>();
            var codeType = Fixture.Create<string>();
            var codeTypeCode = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.CodeType>();
            var codeName = Fixture.Create<string>();
            var codeValue = Fixture.Create<string>();
            var codeDescription = Fixture.Create<string>();
            var systemFlag = Fixture.Create<string>();
            var sortCode = Fixture.Create<int?>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            code.CodeID = codeID;
            code.CodeType = codeType;
            code.CodeTypeCode = codeTypeCode;
            code.CodeName = codeName;
            code.CodeValue = codeValue;
            code.CodeDescription = codeDescription;
            code.SystemFlag = systemFlag;
            code.SortCode = sortCode;
            code.CreatedUserID = createdUserID;
            code.UpdatedUserID = updatedUserID;
            code.IsDeleted = isDeleted;

            // Assert
            code.CodeID.ShouldBe(codeID);
            code.CodeType.ShouldBe(codeType);
            code.CodeTypeCode.ShouldBe(codeTypeCode);
            code.CodeName.ShouldBe(codeName);
            code.CodeValue.ShouldBe(codeValue);
            code.CodeDescription.ShouldBe(codeDescription);
            code.SystemFlag.ShouldBe(systemFlag);
            code.SortCode.ShouldBe(sortCode);
            code.CreatedUserID.ShouldBe(createdUserID);
            code.UpdatedUserID.ShouldBe(updatedUserID);
            code.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : Code => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.IsDeleted = null;

            // Assert
            code.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.IsDeleted.ShouldBeNull();
            code.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Code => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var code = Fixture.Create<Code>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = code.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(code, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Code => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var code = Fixture.Create<Code>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = code.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(code, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Code => CodeTypeCode

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeTypeCode_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCodeTypeCode = "CodeTypeCode";
            var code = Fixture.Create<Code>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = code.GetType().GetProperty(constCodeTypeCode);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(code, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeTypeCode_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCodeTypeCode = "CodeTypeCode";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCodeTypeCode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeTypeCode_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCodeTypeCode = "CodeTypeCode";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCodeTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Code => CodeID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeID_Int_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var intType = code.CodeID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCodeID = "CodeID";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCodeID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeID_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCodeID = "CodeID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Code => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.CreatedUserID = null;

            // Assert
            code.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.CreatedUserID.ShouldBeNull();
            code.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Code => SortCode

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortCode_Data_Without_Null_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var random = Fixture.Create<int>();

            // Act , Set
            code.SortCode = random;

            // Assert
            code.SortCode.ShouldBe(random);
            code.SortCode.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortCode_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.SortCode = null;

            // Assert
            code.SortCode.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortCode_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constSortCode = "SortCode";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(constSortCode);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.SortCode.ShouldBeNull();
            code.SortCode.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_SortCode_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSortCode = "SortCode";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constSortCode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SortCode_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constSortCode = "SortCode";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constSortCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Code => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
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
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();    

            // Act , Set
            code.UpdatedUserID = null;

            // Assert
            code.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var code = Fixture.Create<Code>();
            var propertyInfo = code.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(code, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            code.UpdatedUserID.ShouldBeNull();
            code.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Code => CodeDescription

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeDescription_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var stringType = code.CodeDescription.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeDescription_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCodeDescription = "CodeDescription";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCodeDescription));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeDescription_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCodeDescription = "CodeDescription";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCodeDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Code => CodeName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeName_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var stringType = code.CodeName.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCodeName = "CodeName";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCodeName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeName_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCodeName = "CodeName";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCodeName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Code => CodeType

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeType_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var stringType = code.CodeType.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeType_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCodeType = "CodeType";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCodeType));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeType_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCodeType = "CodeType";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCodeType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Code => CodeValue

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeValue_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var stringType = code.CodeValue.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_CodeValue_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCodeValue = "CodeValue";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constCodeValue));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CodeValue_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constCodeValue = "CodeValue";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constCodeValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Code => SystemFlag

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SystemFlag_String_Type_Verify_Test()
        {
            // Arrange
            var code = Fixture.Create<Code>();
            var stringType = code.SystemFlag.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Code_Class_Invalid_Property_SystemFlag_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSystemFlag = "SystemFlag";
            var code  = Fixture.Create<Code>();

            // Act , Assert
            Should.NotThrow(() => code.GetType().GetProperty(constSystemFlag));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_SystemFlag_Is_Present_In_Code_Class_As_Public_Test()
        {
            // Arrange
            const string constSystemFlag = "SystemFlag";
            var code  = Fixture.Create<Code>();
            var propertyInfo  = code.GetType().GetProperty(constSystemFlag);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Code());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Code>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var codeID = -1;
            var codeType = string.Empty;
            var codeTypeCode = ECN_Framework_Common.Objects.Accounts.Enums.CodeType.Unknown;
            var codeName = string.Empty;
            var codeValue = string.Empty;
            var codeDescription = string.Empty;
            var systemFlag = string.Empty;
            int? sortCode = null;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var code = new Code();    

            // Assert
            code.CodeID.ShouldBe(codeID);
            code.CodeType.ShouldBe(codeType);
            code.CodeTypeCode.ShouldBe(codeTypeCode);
            code.CodeName.ShouldBe(codeName);
            code.CodeValue.ShouldBe(codeValue);
            code.CodeDescription.ShouldBe(codeDescription);
            code.SystemFlag.ShouldBe(systemFlag);
            code.SortCode.ShouldBeNull();
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