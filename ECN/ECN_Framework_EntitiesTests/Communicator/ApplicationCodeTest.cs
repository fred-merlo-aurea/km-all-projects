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
    public class ApplicationCodeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var applicationCode  = new ApplicationCode();
            var appCodeID = Fixture.Create<int>();
            var codeType = Fixture.Create<ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode>();
            var codeValue = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool>();

            // Act
            applicationCode.AppCodeID = appCodeID;
            applicationCode.CodeType = codeType;
            applicationCode.CodeValue = codeValue;
            applicationCode.IsDeleted = isDeleted;

            // Assert
            applicationCode.AppCodeID.ShouldBe(appCodeID);
            applicationCode.CodeType.ShouldBe(codeType);
            applicationCode.CodeValue.ShouldBe(codeValue);
            applicationCode.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region int property type test : ApplicationCode => AppCodeID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_AppCodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var applicationCode = Fixture.Create<ApplicationCode>();
            var intType = applicationCode.AppCodeID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_Class_Invalid_Property_AppCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAppCodeID = "AppCodeIDNotPresent";
            var applicationCode  = Fixture.Create<ApplicationCode>();

            // Act , Assert
            Should.NotThrow(() => applicationCode.GetType().GetProperty(propertyNameAppCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_AppCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAppCodeID = "AppCodeID";
            var applicationCode  = Fixture.Create<ApplicationCode>();
            var propertyInfo  = applicationCode.GetType().GetProperty(propertyNameAppCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : ApplicationCode => CodeType

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_CodeType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeType = "CodeType";
            var applicationCode = Fixture.Create<ApplicationCode>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = applicationCode.GetType().GetProperty(propertyNameCodeType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(applicationCode, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_Class_Invalid_Property_CodeTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeType = "CodeTypeNotPresent";
            var applicationCode  = Fixture.Create<ApplicationCode>();

            // Act , Assert
            Should.NotThrow(() => applicationCode.GetType().GetProperty(propertyNameCodeType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_CodeType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCodeType = "CodeType";
            var applicationCode  = Fixture.Create<ApplicationCode>();
            var propertyInfo  = applicationCode.GetType().GetProperty(propertyNameCodeType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : ApplicationCode => CodeValue

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_CodeValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var applicationCode = Fixture.Create<ApplicationCode>();
            var stringType = applicationCode.CodeValue.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_Class_Invalid_Property_CodeValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCodeValue = "CodeValueNotPresent";
            var applicationCode  = Fixture.Create<ApplicationCode>();

            // Act , Assert
            Should.NotThrow(() => applicationCode.GetType().GetProperty(propertyNameCodeValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_CodeValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCodeValue = "CodeValue";
            var applicationCode  = Fixture.Create<ApplicationCode>();
            var propertyInfo  = applicationCode.GetType().GetProperty(propertyNameCodeValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : ApplicationCode => IsDeleted

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var applicationCode = Fixture.Create<ApplicationCode>();
            var boolType = applicationCode.IsDeleted.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var applicationCode  = Fixture.Create<ApplicationCode>();

            // Act , Assert
            Should.NotThrow(() => applicationCode.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ApplicationCode_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var applicationCode  = Fixture.Create<ApplicationCode>();
            var propertyInfo  = applicationCode.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ApplicationCode_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ApplicationCode());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<ApplicationCode>(2).ToList();
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
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ApplicationCode_Instantiated_With_Default_Assignments_No_Change_Test()
        {
            // Arrange
            var appCodeID = -1;
            var codeType = ECN_Framework_Common.Objects.Communicator.Enums.ApplicationCode.UNKNOWN;
            var codeValue = string.Empty;
            var isDeleted = false;

            // Act
            var applicationCode = new ApplicationCode();

            // Assert
            applicationCode.AppCodeID.ShouldBe(appCodeID);
            applicationCode.CodeType.ShouldBe(codeType);
            applicationCode.CodeValue.ShouldBe(codeValue);
            applicationCode.IsDeleted.ShouldBeFalse();
        }

        #endregion


        #endregion


        #endregion
    }
}