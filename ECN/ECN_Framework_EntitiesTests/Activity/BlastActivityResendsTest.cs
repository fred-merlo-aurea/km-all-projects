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
    public class BlastActivityResendsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityResends) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityResends = Fixture.Create<BlastActivityResends>();
            var resendId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var resendTime = Fixture.Create<DateTime>();
            var eAId = Fixture.Create<int>();

            // Act
            blastActivityResends.ResendID = resendId;
            blastActivityResends.BlastID = blastId;
            blastActivityResends.EmailID = emailId;
            blastActivityResends.ResendTime = resendTime;
            blastActivityResends.EAID = eAId;

            // Assert
            blastActivityResends.ResendID.ShouldBe(resendId);
            blastActivityResends.BlastID.ShouldBe(blastId);
            blastActivityResends.EmailID.ShouldBe(emailId);
            blastActivityResends.ResendTime.ShouldBe(resendTime);
            blastActivityResends.EAID.ShouldBe(eAId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityResends) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityResends = Fixture.Create<BlastActivityResends>();
            blastActivityResends.BlastID = Fixture.Create<int>();
            var intType = blastActivityResends.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityResends) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();

            // Act , Assert
            Should.NotThrow(() => blastActivityResends.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();
            var propertyInfo  = blastActivityResends.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityResends) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityResends = Fixture.Create<BlastActivityResends>();
            blastActivityResends.EAID = Fixture.Create<int>();
            var intType = blastActivityResends.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityResends) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();

            // Act , Assert
            Should.NotThrow(() => blastActivityResends.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();
            var propertyInfo  = blastActivityResends.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityResends) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityResends = Fixture.Create<BlastActivityResends>();
            blastActivityResends.EmailID = Fixture.Create<int>();
            var intType = blastActivityResends.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityResends) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();

            // Act , Assert
            Should.NotThrow(() => blastActivityResends.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();
            var propertyInfo  = blastActivityResends.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityResends) => Property (ResendID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_ResendID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityResends = Fixture.Create<BlastActivityResends>();
            blastActivityResends.ResendID = Fixture.Create<int>();
            var intType = blastActivityResends.ResendID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityResends) => Property (ResendID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_Class_Invalid_Property_ResendIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResendID = "ResendIDNotPresent";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();

            // Act , Assert
            Should.NotThrow(() => blastActivityResends.GetType().GetProperty(propertyNameResendID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_ResendID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResendID = "ResendID";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();
            var propertyInfo  = blastActivityResends.GetType().GetProperty(propertyNameResendID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityResends) => Property (ResendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_ResendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameResendTime = "ResendTime";
            var blastActivityResends = Fixture.Create<BlastActivityResends>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityResends.GetType().GetProperty(propertyNameResendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityResends, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityResends) => Property (ResendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_Class_Invalid_Property_ResendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameResendTime = "ResendTimeNotPresent";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();

            // Act , Assert
            Should.NotThrow(() => blastActivityResends.GetType().GetProperty(propertyNameResendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityResends_ResendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameResendTime = "ResendTime";
            var blastActivityResends  = Fixture.Create<BlastActivityResends>();
            var propertyInfo  = blastActivityResends.GetType().GetProperty(propertyNameResendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityResends) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityResends_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityResends());
        }

        #endregion

        #region General Constructor : Class (BlastActivityResends) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityResends_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityResends = Fixture.CreateMany<BlastActivityResends>(2).ToList();
            var firstBlastActivityResends = instancesOfBlastActivityResends.FirstOrDefault();
            var lastBlastActivityResends = instancesOfBlastActivityResends.Last();

            // Act, Assert
            firstBlastActivityResends.ShouldNotBeNull();
            lastBlastActivityResends.ShouldNotBeNull();
            firstBlastActivityResends.ShouldNotBeSameAs(lastBlastActivityResends);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityResends_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityResends = new BlastActivityResends();
            var secondBlastActivityResends = new BlastActivityResends();
            var thirdBlastActivityResends = new BlastActivityResends();
            var fourthBlastActivityResends = new BlastActivityResends();
            var fifthBlastActivityResends = new BlastActivityResends();
            var sixthBlastActivityResends = new BlastActivityResends();

            // Act, Assert
            firstBlastActivityResends.ShouldNotBeNull();
            secondBlastActivityResends.ShouldNotBeNull();
            thirdBlastActivityResends.ShouldNotBeNull();
            fourthBlastActivityResends.ShouldNotBeNull();
            fifthBlastActivityResends.ShouldNotBeNull();
            sixthBlastActivityResends.ShouldNotBeNull();
            firstBlastActivityResends.ShouldNotBeSameAs(secondBlastActivityResends);
            thirdBlastActivityResends.ShouldNotBeSameAs(firstBlastActivityResends);
            fourthBlastActivityResends.ShouldNotBeSameAs(firstBlastActivityResends);
            fifthBlastActivityResends.ShouldNotBeSameAs(firstBlastActivityResends);
            sixthBlastActivityResends.ShouldNotBeSameAs(firstBlastActivityResends);
            sixthBlastActivityResends.ShouldNotBeSameAs(fourthBlastActivityResends);
        }

        #endregion

        #endregion

        #endregion
    }
}