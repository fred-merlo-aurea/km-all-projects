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
    public class BlastActivitySendsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivitySends) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            var sendId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var sendTime = Fixture.Create<DateTime>();
            var isOpened = Fixture.Create<bool>();
            var isClicked = Fixture.Create<bool>();
            var sMTPMessage = Fixture.Create<string>();
            var isResend = Fixture.Create<bool>();
            var eAId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();

            // Act
            blastActivitySends.SendID = sendId;
            blastActivitySends.BlastID = blastId;
            blastActivitySends.EmailID = emailId;
            blastActivitySends.SendTime = sendTime;
            blastActivitySends.IsOpened = isOpened;
            blastActivitySends.IsClicked = isClicked;
            blastActivitySends.SMTPMessage = sMTPMessage;
            blastActivitySends.IsResend = isResend;
            blastActivitySends.EAID = eAId;
            blastActivitySends.EmailAddress = emailAddress;

            // Assert
            blastActivitySends.SendID.ShouldBe(sendId);
            blastActivitySends.BlastID.ShouldBe(blastId);
            blastActivitySends.EmailID.ShouldBe(emailId);
            blastActivitySends.SendTime.ShouldBe(sendTime);
            blastActivitySends.IsOpened.ShouldBe(isOpened);
            blastActivitySends.IsClicked.ShouldBe(isClicked);
            blastActivitySends.SMTPMessage.ShouldBe(sMTPMessage);
            blastActivitySends.IsResend.ShouldBe(isResend);
            blastActivitySends.EAID.ShouldBe(eAId);
            blastActivitySends.EmailAddress.ShouldBe(emailAddress);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.BlastID = Fixture.Create<int>();
            var intType = blastActivitySends.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.EAID = Fixture.Create<int>();
            var intType = blastActivitySends.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.EmailAddress = Fixture.Create<string>();
            var stringType = blastActivitySends.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.EmailID = Fixture.Create<int>();
            var intType = blastActivitySends.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (IsClicked) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_IsClicked_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.IsClicked = Fixture.Create<bool>();
            var boolType = blastActivitySends.IsClicked.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (IsClicked) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_IsClickedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsClicked = "IsClickedNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameIsClicked));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_IsClicked_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsClicked = "IsClicked";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameIsClicked);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (IsOpened) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_IsOpened_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.IsOpened = Fixture.Create<bool>();
            var boolType = blastActivitySends.IsOpened.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (IsOpened) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_IsOpenedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsOpened = "IsOpenedNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameIsOpened));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_IsOpened_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsOpened = "IsOpened";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameIsOpened);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (IsResend) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_IsResend_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.IsResend = Fixture.Create<bool>();
            var boolType = blastActivitySends.IsResend.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (IsResend) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_IsResendNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsResend = "IsResendNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameIsResend));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_IsResend_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsResend = "IsResend";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameIsResend);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (SendID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_SendID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.SendID = Fixture.Create<int>();
            var intType = blastActivitySends.SendID.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (SendID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_SendIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendID = "SendIDNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameSendID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_SendID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendID = "SendID";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameSendID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivitySends.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivitySends, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivitySends) => Property (SMTPMessage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_SMTPMessage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivitySends = Fixture.Create<BlastActivitySends>();
            blastActivitySends.SMTPMessage = Fixture.Create<string>();
            var stringType = blastActivitySends.SMTPMessage.GetType();

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

        #region General Getters/Setters : Class (BlastActivitySends) => Property (SMTPMessage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_Class_Invalid_Property_SMTPMessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSMTPMessage = "SMTPMessageNotPresent";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();

            // Act , Assert
            Should.NotThrow(() => blastActivitySends.GetType().GetProperty(propertyNameSMTPMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivitySends_SMTPMessage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSMTPMessage = "SMTPMessage";
            var blastActivitySends  = Fixture.Create<BlastActivitySends>();
            var propertyInfo  = blastActivitySends.GetType().GetProperty(propertyNameSMTPMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivitySends) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySends_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivitySends());
        }

        #endregion

        #region General Constructor : Class (BlastActivitySends) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySends_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivitySends = Fixture.CreateMany<BlastActivitySends>(2).ToList();
            var firstBlastActivitySends = instancesOfBlastActivitySends.FirstOrDefault();
            var lastBlastActivitySends = instancesOfBlastActivitySends.Last();

            // Act, Assert
            firstBlastActivitySends.ShouldNotBeNull();
            lastBlastActivitySends.ShouldNotBeNull();
            firstBlastActivitySends.ShouldNotBeSameAs(lastBlastActivitySends);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivitySends_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivitySends = new BlastActivitySends();
            var secondBlastActivitySends = new BlastActivitySends();
            var thirdBlastActivitySends = new BlastActivitySends();
            var fourthBlastActivitySends = new BlastActivitySends();
            var fifthBlastActivitySends = new BlastActivitySends();
            var sixthBlastActivitySends = new BlastActivitySends();

            // Act, Assert
            firstBlastActivitySends.ShouldNotBeNull();
            secondBlastActivitySends.ShouldNotBeNull();
            thirdBlastActivitySends.ShouldNotBeNull();
            fourthBlastActivitySends.ShouldNotBeNull();
            fifthBlastActivitySends.ShouldNotBeNull();
            sixthBlastActivitySends.ShouldNotBeNull();
            firstBlastActivitySends.ShouldNotBeSameAs(secondBlastActivitySends);
            thirdBlastActivitySends.ShouldNotBeSameAs(firstBlastActivitySends);
            fourthBlastActivitySends.ShouldNotBeSameAs(firstBlastActivitySends);
            fifthBlastActivitySends.ShouldNotBeSameAs(firstBlastActivitySends);
            sixthBlastActivitySends.ShouldNotBeSameAs(firstBlastActivitySends);
            sixthBlastActivitySends.ShouldNotBeSameAs(fourthBlastActivitySends);
        }

        #endregion

        #endregion

        #endregion
    }
}