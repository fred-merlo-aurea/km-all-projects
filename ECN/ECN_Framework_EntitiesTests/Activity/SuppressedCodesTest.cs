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
    public class SuppressedCodesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (SuppressedCodes) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var suppressedCodes = Fixture.Create<SuppressedCodes>();
            var suppressedCodeId = Fixture.Create<int>();
            var supressedCode = Fixture.Create<string>();

            // Act
            suppressedCodes.SuppressedCodeID = suppressedCodeId;
            suppressedCodes.SupressedCode = supressedCode;

            // Assert
            suppressedCodes.SuppressedCodeID.ShouldBe(suppressedCodeId);
            suppressedCodes.SupressedCode.ShouldBe(supressedCode);
        }

        #endregion

        #region General Getters/Setters : Class (SuppressedCodes) => Property (SuppressedCodeID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_SuppressedCodeID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var suppressedCodes = Fixture.Create<SuppressedCodes>();
            suppressedCodes.SuppressedCodeID = Fixture.Create<int>();
            var intType = suppressedCodes.SuppressedCodeID.GetType();

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

        #region General Getters/Setters : Class (SuppressedCodes) => Property (SuppressedCodeID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_Class_Invalid_Property_SuppressedCodeIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedCodeID = "SuppressedCodeIDNotPresent";
            var suppressedCodes  = Fixture.Create<SuppressedCodes>();

            // Act , Assert
            Should.NotThrow(() => suppressedCodes.GetType().GetProperty(propertyNameSuppressedCodeID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_SuppressedCodeID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedCodeID = "SuppressedCodeID";
            var suppressedCodes  = Fixture.Create<SuppressedCodes>();
            var propertyInfo  = suppressedCodes.GetType().GetProperty(propertyNameSuppressedCodeID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SuppressedCodes) => Property (SupressedCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_SupressedCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var suppressedCodes = Fixture.Create<SuppressedCodes>();
            suppressedCodes.SupressedCode = Fixture.Create<string>();
            var stringType = suppressedCodes.SupressedCode.GetType();

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

        #region General Getters/Setters : Class (SuppressedCodes) => Property (SupressedCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_Class_Invalid_Property_SupressedCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSupressedCode = "SupressedCodeNotPresent";
            var suppressedCodes  = Fixture.Create<SuppressedCodes>();

            // Act , Assert
            Should.NotThrow(() => suppressedCodes.GetType().GetProperty(propertyNameSupressedCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SuppressedCodes_SupressedCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSupressedCode = "SupressedCode";
            var suppressedCodes  = Fixture.Create<SuppressedCodes>();
            var propertyInfo  = suppressedCodes.GetType().GetProperty(propertyNameSupressedCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (SuppressedCodes) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SuppressedCodes_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SuppressedCodes());
        }

        #endregion

        #region General Constructor : Class (SuppressedCodes) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SuppressedCodes_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSuppressedCodes = Fixture.CreateMany<SuppressedCodes>(2).ToList();
            var firstSuppressedCodes = instancesOfSuppressedCodes.FirstOrDefault();
            var lastSuppressedCodes = instancesOfSuppressedCodes.Last();

            // Act, Assert
            firstSuppressedCodes.ShouldNotBeNull();
            lastSuppressedCodes.ShouldNotBeNull();
            firstSuppressedCodes.ShouldNotBeSameAs(lastSuppressedCodes);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SuppressedCodes_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSuppressedCodes = new SuppressedCodes();
            var secondSuppressedCodes = new SuppressedCodes();
            var thirdSuppressedCodes = new SuppressedCodes();
            var fourthSuppressedCodes = new SuppressedCodes();
            var fifthSuppressedCodes = new SuppressedCodes();
            var sixthSuppressedCodes = new SuppressedCodes();

            // Act, Assert
            firstSuppressedCodes.ShouldNotBeNull();
            secondSuppressedCodes.ShouldNotBeNull();
            thirdSuppressedCodes.ShouldNotBeNull();
            fourthSuppressedCodes.ShouldNotBeNull();
            fifthSuppressedCodes.ShouldNotBeNull();
            sixthSuppressedCodes.ShouldNotBeNull();
            firstSuppressedCodes.ShouldNotBeSameAs(secondSuppressedCodes);
            thirdSuppressedCodes.ShouldNotBeSameAs(firstSuppressedCodes);
            fourthSuppressedCodes.ShouldNotBeSameAs(firstSuppressedCodes);
            fifthSuppressedCodes.ShouldNotBeSameAs(firstSuppressedCodes);
            sixthSuppressedCodes.ShouldNotBeSameAs(firstSuppressedCodes);
            sixthSuppressedCodes.ShouldNotBeSameAs(fourthSuppressedCodes);
        }

        #endregion

        #endregion

        #endregion
    }
}