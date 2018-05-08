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
    public class BlastActivityClicksTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityClicks) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            var clickId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var clickTime = Fixture.Create<DateTime>();
            var uRL = Fixture.Create<string>();
            var blastLinkId = Fixture.Create<int>();
            var eAId = Fixture.Create<int>();
            var uniqueLinkId = Fixture.Create<int?>();

            // Act
            blastActivityClicks.ClickID = clickId;
            blastActivityClicks.BlastID = blastId;
            blastActivityClicks.EmailID = emailId;
            blastActivityClicks.ClickTime = clickTime;
            blastActivityClicks.URL = uRL;
            blastActivityClicks.BlastLinkID = blastLinkId;
            blastActivityClicks.EAID = eAId;
            blastActivityClicks.UniqueLinkID = uniqueLinkId;

            // Assert
            blastActivityClicks.ClickID.ShouldBe(clickId);
            blastActivityClicks.BlastID.ShouldBe(blastId);
            blastActivityClicks.EmailID.ShouldBe(emailId);
            blastActivityClicks.ClickTime.ShouldBe(clickTime);
            blastActivityClicks.URL.ShouldBe(uRL);
            blastActivityClicks.BlastLinkID.ShouldBe(blastLinkId);
            blastActivityClicks.EAID.ShouldBe(eAId);
            blastActivityClicks.UniqueLinkID.ShouldBe(uniqueLinkId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            blastActivityClicks.BlastID = Fixture.Create<int>();
            var intType = blastActivityClicks.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (BlastLinkID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_BlastLinkID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            blastActivityClicks.BlastLinkID = Fixture.Create<int>();
            var intType = blastActivityClicks.BlastLinkID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (BlastLinkID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_BlastLinkIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastLinkID = "BlastLinkIDNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameBlastLinkID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_BlastLinkID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastLinkID = "BlastLinkID";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameBlastLinkID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (ClickID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_ClickID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            blastActivityClicks.ClickID = Fixture.Create<int>();
            var intType = blastActivityClicks.ClickID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (ClickID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_ClickIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickID = "ClickIDNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameClickID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_ClickID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickID = "ClickID";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameClickID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (ClickTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_ClickTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickTime = "ClickTime";
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityClicks.GetType().GetProperty(propertyNameClickTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityClicks, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (ClickTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_ClickTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickTime = "ClickTimeNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameClickTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_ClickTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickTime = "ClickTime";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameClickTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            blastActivityClicks.EAID = Fixture.Create<int>();
            var intType = blastActivityClicks.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            blastActivityClicks.EmailID = Fixture.Create<int>();
            var intType = blastActivityClicks.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (UniqueLinkID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_UniqueLinkID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivityClicks.UniqueLinkID = random;

            // Assert
            blastActivityClicks.UniqueLinkID.ShouldBe(random);
            blastActivityClicks.UniqueLinkID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_UniqueLinkID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();    

            // Act , Set
            blastActivityClicks.UniqueLinkID = null;

            // Assert
            blastActivityClicks.UniqueLinkID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_UniqueLinkID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUniqueLinkID = "UniqueLinkID";
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            var propertyInfo = blastActivityClicks.GetType().GetProperty(propertyNameUniqueLinkID);

            // Act , Set
            propertyInfo.SetValue(blastActivityClicks, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivityClicks.UniqueLinkID.ShouldBeNull();
            blastActivityClicks.UniqueLinkID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (UniqueLinkID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_UniqueLinkIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueLinkID = "UniqueLinkIDNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameUniqueLinkID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_UniqueLinkID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueLinkID = "UniqueLinkID";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameUniqueLinkID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (URL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_URL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityClicks = Fixture.Create<BlastActivityClicks>();
            blastActivityClicks.URL = Fixture.Create<string>();
            var stringType = blastActivityClicks.URL.GetType();

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

        #region General Getters/Setters : Class (BlastActivityClicks) => Property (URL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_Class_Invalid_Property_URLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameURL = "URLNotPresent";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();

            // Act , Assert
            Should.NotThrow(() => blastActivityClicks.GetType().GetProperty(propertyNameURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityClicks_URL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameURL = "URL";
            var blastActivityClicks  = Fixture.Create<BlastActivityClicks>();
            var propertyInfo  = blastActivityClicks.GetType().GetProperty(propertyNameURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityClicks) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityClicks_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityClicks());
        }

        #endregion

        #region General Constructor : Class (BlastActivityClicks) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityClicks_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityClicks = Fixture.CreateMany<BlastActivityClicks>(2).ToList();
            var firstBlastActivityClicks = instancesOfBlastActivityClicks.FirstOrDefault();
            var lastBlastActivityClicks = instancesOfBlastActivityClicks.Last();

            // Act, Assert
            firstBlastActivityClicks.ShouldNotBeNull();
            lastBlastActivityClicks.ShouldNotBeNull();
            firstBlastActivityClicks.ShouldNotBeSameAs(lastBlastActivityClicks);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityClicks_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityClicks = new BlastActivityClicks();
            var secondBlastActivityClicks = new BlastActivityClicks();
            var thirdBlastActivityClicks = new BlastActivityClicks();
            var fourthBlastActivityClicks = new BlastActivityClicks();
            var fifthBlastActivityClicks = new BlastActivityClicks();
            var sixthBlastActivityClicks = new BlastActivityClicks();

            // Act, Assert
            firstBlastActivityClicks.ShouldNotBeNull();
            secondBlastActivityClicks.ShouldNotBeNull();
            thirdBlastActivityClicks.ShouldNotBeNull();
            fourthBlastActivityClicks.ShouldNotBeNull();
            fifthBlastActivityClicks.ShouldNotBeNull();
            sixthBlastActivityClicks.ShouldNotBeNull();
            firstBlastActivityClicks.ShouldNotBeSameAs(secondBlastActivityClicks);
            thirdBlastActivityClicks.ShouldNotBeSameAs(firstBlastActivityClicks);
            fourthBlastActivityClicks.ShouldNotBeSameAs(firstBlastActivityClicks);
            fifthBlastActivityClicks.ShouldNotBeSameAs(firstBlastActivityClicks);
            sixthBlastActivityClicks.ShouldNotBeSameAs(firstBlastActivityClicks);
            sixthBlastActivityClicks.ShouldNotBeSameAs(fourthBlastActivityClicks);
        }

        #endregion

        #endregion

        #endregion
    }
}