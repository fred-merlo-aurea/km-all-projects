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
    public class BlastActivityConversionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityConversion) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            var conversionId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var conversionTime = Fixture.Create<DateTime>();
            var uRL = Fixture.Create<string>();
            var eAId = Fixture.Create<int>();

            // Act
            blastActivityConversion.ConversionID = conversionId;
            blastActivityConversion.BlastID = blastId;
            blastActivityConversion.EmailID = emailId;
            blastActivityConversion.ConversionTime = conversionTime;
            blastActivityConversion.URL = uRL;
            blastActivityConversion.EAID = eAId;

            // Assert
            blastActivityConversion.ConversionID.ShouldBe(conversionId);
            blastActivityConversion.BlastID.ShouldBe(blastId);
            blastActivityConversion.EmailID.ShouldBe(emailId);
            blastActivityConversion.ConversionTime.ShouldBe(conversionTime);
            blastActivityConversion.URL.ShouldBe(uRL);
            blastActivityConversion.EAID.ShouldBe(eAId);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            blastActivityConversion.BlastID = Fixture.Create<int>();
            var intType = blastActivityConversion.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();

            // Act , Assert
            Should.NotThrow(() => blastActivityConversion.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();
            var propertyInfo  = blastActivityConversion.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (ConversionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_ConversionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            blastActivityConversion.ConversionID = Fixture.Create<int>();
            var intType = blastActivityConversion.ConversionID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (ConversionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_Invalid_Property_ConversionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConversionID = "ConversionIDNotPresent";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();

            // Act , Assert
            Should.NotThrow(() => blastActivityConversion.GetType().GetProperty(propertyNameConversionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_ConversionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConversionID = "ConversionID";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();
            var propertyInfo  = blastActivityConversion.GetType().GetProperty(propertyNameConversionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (ConversionTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_ConversionTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameConversionTime = "ConversionTime";
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivityConversion.GetType().GetProperty(propertyNameConversionTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivityConversion, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (ConversionTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_Invalid_Property_ConversionTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConversionTime = "ConversionTimeNotPresent";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();

            // Act , Assert
            Should.NotThrow(() => blastActivityConversion.GetType().GetProperty(propertyNameConversionTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_ConversionTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConversionTime = "ConversionTime";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();
            var propertyInfo  = blastActivityConversion.GetType().GetProperty(propertyNameConversionTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            blastActivityConversion.EAID = Fixture.Create<int>();
            var intType = blastActivityConversion.EAID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();

            // Act , Assert
            Should.NotThrow(() => blastActivityConversion.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();
            var propertyInfo  = blastActivityConversion.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            blastActivityConversion.EmailID = Fixture.Create<int>();
            var intType = blastActivityConversion.EmailID.GetType();

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

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();

            // Act , Assert
            Should.NotThrow(() => blastActivityConversion.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();
            var propertyInfo  = blastActivityConversion.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (URL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_URL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityConversion = Fixture.Create<BlastActivityConversion>();
            blastActivityConversion.URL = Fixture.Create<string>();
            var stringType = blastActivityConversion.URL.GetType();

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

        #region General Getters/Setters : Class (BlastActivityConversion) => Property (URL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_Class_Invalid_Property_URLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameURL = "URLNotPresent";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();

            // Act , Assert
            Should.NotThrow(() => blastActivityConversion.GetType().GetProperty(propertyNameURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityConversion_URL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameURL = "URL";
            var blastActivityConversion  = Fixture.Create<BlastActivityConversion>();
            var propertyInfo  = blastActivityConversion.GetType().GetProperty(propertyNameURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityConversion) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityConversion_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityConversion());
        }

        #endregion

        #region General Constructor : Class (BlastActivityConversion) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityConversion_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityConversion = Fixture.CreateMany<BlastActivityConversion>(2).ToList();
            var firstBlastActivityConversion = instancesOfBlastActivityConversion.FirstOrDefault();
            var lastBlastActivityConversion = instancesOfBlastActivityConversion.Last();

            // Act, Assert
            firstBlastActivityConversion.ShouldNotBeNull();
            lastBlastActivityConversion.ShouldNotBeNull();
            firstBlastActivityConversion.ShouldNotBeSameAs(lastBlastActivityConversion);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityConversion_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityConversion = new BlastActivityConversion();
            var secondBlastActivityConversion = new BlastActivityConversion();
            var thirdBlastActivityConversion = new BlastActivityConversion();
            var fourthBlastActivityConversion = new BlastActivityConversion();
            var fifthBlastActivityConversion = new BlastActivityConversion();
            var sixthBlastActivityConversion = new BlastActivityConversion();

            // Act, Assert
            firstBlastActivityConversion.ShouldNotBeNull();
            secondBlastActivityConversion.ShouldNotBeNull();
            thirdBlastActivityConversion.ShouldNotBeNull();
            fourthBlastActivityConversion.ShouldNotBeNull();
            fifthBlastActivityConversion.ShouldNotBeNull();
            sixthBlastActivityConversion.ShouldNotBeNull();
            firstBlastActivityConversion.ShouldNotBeSameAs(secondBlastActivityConversion);
            thirdBlastActivityConversion.ShouldNotBeSameAs(firstBlastActivityConversion);
            fourthBlastActivityConversion.ShouldNotBeSameAs(firstBlastActivityConversion);
            fifthBlastActivityConversion.ShouldNotBeSameAs(firstBlastActivityConversion);
            sixthBlastActivityConversion.ShouldNotBeSameAs(firstBlastActivityConversion);
            sixthBlastActivityConversion.ShouldNotBeSameAs(fourthBlastActivityConversion);
        }

        #endregion

        #endregion

        #endregion
    }
}