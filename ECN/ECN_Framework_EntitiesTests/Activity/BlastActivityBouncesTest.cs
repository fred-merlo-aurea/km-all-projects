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
    public class BlastActivityBouncesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityBounces) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            var bounceId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var bounceTime = Fixture.Create<DateTime>();
            var bounceCodeId = Fixture.Create<int>();
            var bounceMessage = Fixture.Create<string>();
            var bounceCode = Fixture.Create<string>();
            var eAId = Fixture.Create<int>();

            // Act
            blastActivityBounces.BounceID = bounceId;
            blastActivityBounces.BlastID = blastId;
            blastActivityBounces.EmailID = emailId;
            blastActivityBounces.BounceTime = bounceTime;
            blastActivityBounces.BounceCodeID = bounceCodeId;
            blastActivityBounces.BounceMessage = bounceMessage;
            blastActivityBounces.BounceCode = bounceCode;
            blastActivityBounces.EAID = eAId;

            // Assert
            blastActivityBounces.BounceID.ShouldBe(bounceId);
            blastActivityBounces.BlastID.ShouldBe(blastId);
            blastActivityBounces.EmailID.ShouldBe(emailId);
            blastActivityBounces.BounceTime.ShouldBe(bounceTime);
            blastActivityBounces.BounceCodeID.ShouldBe(bounceCodeId);
            blastActivityBounces.BounceMessage.ShouldBe(bounceMessage);
            blastActivityBounces.BounceCode.ShouldBe(bounceCode);
            blastActivityBounces.EAID.ShouldBe(eAId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.BlastID = Fixture.Create<int>();
            var intType = blastActivityBounces.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.BounceCode = Fixture.Create<string>();
            var stringType = blastActivityBounces.BounceCode.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_BounceCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceCode = "BounceCodeNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameBounceCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceCode = "BounceCode";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameBounceCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceCodeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceCodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.BounceCodeID = Fixture.Create<int>();
            var intType = blastActivityBounces.BounceCodeID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceCodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_BounceCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceCodeID = "BounceCodeIDNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameBounceCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceCodeID = "BounceCodeID";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameBounceCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.BounceID = Fixture.Create<int>();
            var intType = blastActivityBounces.BounceID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_BounceIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceID = "BounceIDNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameBounceID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceID = "BounceID";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameBounceID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceMessage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceMessage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.BounceMessage = Fixture.Create<string>();
            var stringType = blastActivityBounces.BounceMessage.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceMessage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_BounceMessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceMessage = "BounceMessageNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameBounceMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceMessage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceMessage = "BounceMessage";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameBounceMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceTime = "BounceTime";
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityBounces.GetType().GetProperty(propertyNameBounceTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityBounces, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (BounceTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_BounceTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceTime = "BounceTimeNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameBounceTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_BounceTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceTime = "BounceTime";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameBounceTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.EAID = Fixture.Create<int>();
            var intType = blastActivityBounces.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityBounces = Fixture.Create<BlastActivityBounces>();
            blastActivityBounces.EmailID = Fixture.Create<int>();
            var intType = blastActivityBounces.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityBounces) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();

            // Act , Assert
            Should.NotThrow(() => blastActivityBounces.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityBounces_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityBounces  = Fixture.Create<BlastActivityBounces>();
            var propertyInfo  = blastActivityBounces.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityBounces) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityBounces_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityBounces());
        }

        #endregion

        #region General Constructor : Class (BlastActivityBounces) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityBounces_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityBounces = Fixture.CreateMany<BlastActivityBounces>(2).ToList();
            var firstBlastActivityBounces = instancesOfBlastActivityBounces.FirstOrDefault();
            var lastBlastActivityBounces = instancesOfBlastActivityBounces.Last();

            // Act, Assert
            firstBlastActivityBounces.ShouldNotBeNull();
            lastBlastActivityBounces.ShouldNotBeNull();
            firstBlastActivityBounces.ShouldNotBeSameAs(lastBlastActivityBounces);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityBounces_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityBounces = new BlastActivityBounces();
            var secondBlastActivityBounces = new BlastActivityBounces();
            var thirdBlastActivityBounces = new BlastActivityBounces();
            var fourthBlastActivityBounces = new BlastActivityBounces();
            var fifthBlastActivityBounces = new BlastActivityBounces();
            var sixthBlastActivityBounces = new BlastActivityBounces();

            // Act, Assert
            firstBlastActivityBounces.ShouldNotBeNull();
            secondBlastActivityBounces.ShouldNotBeNull();
            thirdBlastActivityBounces.ShouldNotBeNull();
            fourthBlastActivityBounces.ShouldNotBeNull();
            fifthBlastActivityBounces.ShouldNotBeNull();
            sixthBlastActivityBounces.ShouldNotBeNull();
            firstBlastActivityBounces.ShouldNotBeSameAs(secondBlastActivityBounces);
            thirdBlastActivityBounces.ShouldNotBeSameAs(firstBlastActivityBounces);
            fourthBlastActivityBounces.ShouldNotBeSameAs(firstBlastActivityBounces);
            fifthBlastActivityBounces.ShouldNotBeSameAs(firstBlastActivityBounces);
            sixthBlastActivityBounces.ShouldNotBeSameAs(firstBlastActivityBounces);
            sixthBlastActivityBounces.ShouldNotBeSameAs(fourthBlastActivityBounces);
        }

        #endregion

        #endregion

        #endregion
    }
}