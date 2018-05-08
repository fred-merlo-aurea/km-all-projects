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
    public class BlastActivityReferTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityRefer) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            var referId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var referTime = Fixture.Create<DateTime>();
            var emailAddress = Fixture.Create<string>();
            var eAId = Fixture.Create<int>();

            // Act
            blastActivityRefer.ReferID = referId;
            blastActivityRefer.BlastID = blastId;
            blastActivityRefer.EmailID = emailId;
            blastActivityRefer.ReferTime = referTime;
            blastActivityRefer.EmailAddress = emailAddress;
            blastActivityRefer.EAID = eAId;

            // Assert
            blastActivityRefer.ReferID.ShouldBe(referId);
            blastActivityRefer.BlastID.ShouldBe(blastId);
            blastActivityRefer.EmailID.ShouldBe(emailId);
            blastActivityRefer.ReferTime.ShouldBe(referTime);
            blastActivityRefer.EmailAddress.ShouldBe(emailAddress);
            blastActivityRefer.EAID.ShouldBe(eAId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            blastActivityRefer.BlastID = Fixture.Create<int>();
            var intType = blastActivityRefer.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();

            // Act , Assert
            Should.NotThrow(() => blastActivityRefer.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();
            var propertyInfo  = blastActivityRefer.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            blastActivityRefer.EAID = Fixture.Create<int>();
            var intType = blastActivityRefer.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();

            // Act , Assert
            Should.NotThrow(() => blastActivityRefer.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();
            var propertyInfo  = blastActivityRefer.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            blastActivityRefer.EmailAddress = Fixture.Create<string>();
            var stringType = blastActivityRefer.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();

            // Act , Assert
            Should.NotThrow(() => blastActivityRefer.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();
            var propertyInfo  = blastActivityRefer.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            blastActivityRefer.EmailID = Fixture.Create<int>();
            var intType = blastActivityRefer.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();

            // Act , Assert
            Should.NotThrow(() => blastActivityRefer.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();
            var propertyInfo  = blastActivityRefer.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (ReferID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_ReferID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            blastActivityRefer.ReferID = Fixture.Create<int>();
            var intType = blastActivityRefer.ReferID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (ReferID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_Invalid_Property_ReferIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReferID = "ReferIDNotPresent";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();

            // Act , Assert
            Should.NotThrow(() => blastActivityRefer.GetType().GetProperty(propertyNameReferID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_ReferID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReferID = "ReferID";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();
            var propertyInfo  = blastActivityRefer.GetType().GetProperty(propertyNameReferID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (ReferTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_ReferTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameReferTime = "ReferTime";
            var blastActivityRefer = Fixture.Create<BlastActivityRefer>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityRefer.GetType().GetProperty(propertyNameReferTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityRefer, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityRefer) => Property (ReferTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_Class_Invalid_Property_ReferTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReferTime = "ReferTimeNotPresent";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();

            // Act , Assert
            Should.NotThrow(() => blastActivityRefer.GetType().GetProperty(propertyNameReferTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityRefer_ReferTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReferTime = "ReferTime";
            var blastActivityRefer  = Fixture.Create<BlastActivityRefer>();
            var propertyInfo  = blastActivityRefer.GetType().GetProperty(propertyNameReferTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityRefer) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityRefer_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityRefer());
        }

        #endregion

        #region General Constructor : Class (BlastActivityRefer) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityRefer_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityRefer = Fixture.CreateMany<BlastActivityRefer>(2).ToList();
            var firstBlastActivityRefer = instancesOfBlastActivityRefer.FirstOrDefault();
            var lastBlastActivityRefer = instancesOfBlastActivityRefer.Last();

            // Act, Assert
            firstBlastActivityRefer.ShouldNotBeNull();
            lastBlastActivityRefer.ShouldNotBeNull();
            firstBlastActivityRefer.ShouldNotBeSameAs(lastBlastActivityRefer);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityRefer_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityRefer = new BlastActivityRefer();
            var secondBlastActivityRefer = new BlastActivityRefer();
            var thirdBlastActivityRefer = new BlastActivityRefer();
            var fourthBlastActivityRefer = new BlastActivityRefer();
            var fifthBlastActivityRefer = new BlastActivityRefer();
            var sixthBlastActivityRefer = new BlastActivityRefer();

            // Act, Assert
            firstBlastActivityRefer.ShouldNotBeNull();
            secondBlastActivityRefer.ShouldNotBeNull();
            thirdBlastActivityRefer.ShouldNotBeNull();
            fourthBlastActivityRefer.ShouldNotBeNull();
            fifthBlastActivityRefer.ShouldNotBeNull();
            sixthBlastActivityRefer.ShouldNotBeNull();
            firstBlastActivityRefer.ShouldNotBeSameAs(secondBlastActivityRefer);
            thirdBlastActivityRefer.ShouldNotBeSameAs(firstBlastActivityRefer);
            fourthBlastActivityRefer.ShouldNotBeSameAs(firstBlastActivityRefer);
            fifthBlastActivityRefer.ShouldNotBeSameAs(firstBlastActivityRefer);
            sixthBlastActivityRefer.ShouldNotBeSameAs(firstBlastActivityRefer);
            sixthBlastActivityRefer.ShouldNotBeSameAs(fourthBlastActivityRefer);
        }

        #endregion

        #endregion

        #endregion
    }
}