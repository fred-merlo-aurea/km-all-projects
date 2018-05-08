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
using ECN_Framework_Entities.Activity;

namespace ECN_Framework_Entities.Activity
{
    [TestFixture]
    public class BlastActivityOpensTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityOpens) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            var openId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var openTime = Fixture.Create<DateTime>();
            var browserInfo = Fixture.Create<string>();
            var eAId = Fixture.Create<int>();
            var emailClientId = Fixture.Create<int>();
            var platformId = Fixture.Create<int>();

            // Act
            blastActivityOpens.OpenID = openId;
            blastActivityOpens.BlastID = blastId;
            blastActivityOpens.EmailID = emailId;
            blastActivityOpens.OpenTime = openTime;
            blastActivityOpens.BrowserInfo = browserInfo;
            blastActivityOpens.EAID = eAId;
            blastActivityOpens.EmailClientID = emailClientId;
            blastActivityOpens.PlatformID = platformId;

            // Assert
            blastActivityOpens.OpenID.ShouldBe(openId);
            blastActivityOpens.BlastID.ShouldBe(blastId);
            blastActivityOpens.EmailID.ShouldBe(emailId);
            blastActivityOpens.OpenTime.ShouldBe(openTime);
            blastActivityOpens.BrowserInfo.ShouldBe(browserInfo);
            blastActivityOpens.EAID.ShouldBe(eAId);
            blastActivityOpens.EmailClientID.ShouldBe(emailClientId);
            blastActivityOpens.PlatformID.ShouldBe(platformId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.BlastID = Fixture.Create<int>();
            var intType = blastActivityOpens.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (BrowserInfo) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_BrowserInfo_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.BrowserInfo = Fixture.Create<string>();
            var stringType = blastActivityOpens.BrowserInfo.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (BrowserInfo) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_BrowserInfoNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBrowserInfo = "BrowserInfoNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameBrowserInfo));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_BrowserInfo_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBrowserInfo = "BrowserInfo";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameBrowserInfo);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.EAID = Fixture.Create<int>();
            var intType = blastActivityOpens.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (EmailClientID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_EmailClientID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.EmailClientID = Fixture.Create<int>();
            var intType = blastActivityOpens.EmailClientID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (EmailClientID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_EmailClientIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailClientID = "EmailClientIDNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameEmailClientID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_EmailClientID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailClientID = "EmailClientID";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameEmailClientID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.EmailID = Fixture.Create<int>();
            var intType = blastActivityOpens.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (OpenID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_OpenID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.OpenID = Fixture.Create<int>();
            var intType = blastActivityOpens.OpenID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (OpenID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_OpenIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenID = "OpenIDNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameOpenID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_OpenID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpenID = "OpenID";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameOpenID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (OpenTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_OpenTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenTime = "OpenTime";
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityOpens.GetType().GetProperty(propertyNameOpenTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityOpens, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (OpenTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_OpenTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenTime = "OpenTimeNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNameOpenTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_OpenTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpenTime = "OpenTime";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNameOpenTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (PlatformID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_PlatformID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpens = Fixture.Create<BlastActivityOpens>();
            blastActivityOpens.PlatformID = Fixture.Create<int>();
            var intType = blastActivityOpens.PlatformID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpens) => Property (PlatformID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_Class_Invalid_Property_PlatformIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformID = "PlatformIDNotPresent";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpens.GetType().GetProperty(propertyNamePlatformID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpens_PlatformID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformID = "PlatformID";
            var blastActivityOpens  = Fixture.Create<BlastActivityOpens>();
            var propertyInfo  = blastActivityOpens.GetType().GetProperty(propertyNamePlatformID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityOpens) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityOpens_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityOpens());
        }

        #endregion

        #region General Constructor : Class (BlastActivityOpens) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityOpens_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityOpens = Fixture.CreateMany<BlastActivityOpens>(2).ToList();
            var firstBlastActivityOpens = instancesOfBlastActivityOpens.FirstOrDefault();
            var lastBlastActivityOpens = instancesOfBlastActivityOpens.Last();

            // Act, Assert
            firstBlastActivityOpens.ShouldNotBeNull();
            lastBlastActivityOpens.ShouldNotBeNull();
            firstBlastActivityOpens.ShouldNotBeSameAs(lastBlastActivityOpens);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityOpens_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityOpens = new BlastActivityOpens();
            var secondBlastActivityOpens = new BlastActivityOpens();
            var thirdBlastActivityOpens = new BlastActivityOpens();
            var fourthBlastActivityOpens = new BlastActivityOpens();
            var fifthBlastActivityOpens = new BlastActivityOpens();
            var sixthBlastActivityOpens = new BlastActivityOpens();

            // Act, Assert
            firstBlastActivityOpens.ShouldNotBeNull();
            secondBlastActivityOpens.ShouldNotBeNull();
            thirdBlastActivityOpens.ShouldNotBeNull();
            fourthBlastActivityOpens.ShouldNotBeNull();
            fifthBlastActivityOpens.ShouldNotBeNull();
            sixthBlastActivityOpens.ShouldNotBeNull();
            firstBlastActivityOpens.ShouldNotBeSameAs(secondBlastActivityOpens);
            thirdBlastActivityOpens.ShouldNotBeSameAs(firstBlastActivityOpens);
            fourthBlastActivityOpens.ShouldNotBeSameAs(firstBlastActivityOpens);
            fifthBlastActivityOpens.ShouldNotBeSameAs(firstBlastActivityOpens);
            sixthBlastActivityOpens.ShouldNotBeSameAs(firstBlastActivityOpens);
            sixthBlastActivityOpens.ShouldNotBeSameAs(fourthBlastActivityOpens);
        }

        #endregion

        #endregion

        #endregion
    }
}