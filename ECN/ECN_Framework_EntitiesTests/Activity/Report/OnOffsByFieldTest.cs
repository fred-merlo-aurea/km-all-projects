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
using ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_Entities.Activity.Report
{
    [TestFixture]
    public class OnOffsByFieldTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (OnOffsByField) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var onOffsByField = Fixture.Create<OnOffsByField>();
            var field = Fixture.Create<string>();
            var months = Fixture.Create<DateTime>();
            var subscribeTypeCode = Fixture.Create<string>();
            var counts = Fixture.Create<int>();

            // Act
            onOffsByField.Field = field;
            onOffsByField.Months = months;
            onOffsByField.SubscribeTypeCode = subscribeTypeCode;
            onOffsByField.Counts = counts;

            // Assert
            onOffsByField.Field.ShouldBe(field);
            onOffsByField.Months.ShouldBe(months);
            onOffsByField.SubscribeTypeCode.ShouldBe(subscribeTypeCode);
            onOffsByField.Counts.ShouldBe(counts);
        }

        #endregion

        #region General Getters/Setters : Class (OnOffsByField) => Property (Counts) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Counts_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var onOffsByField = Fixture.Create<OnOffsByField>();
            onOffsByField.Counts = Fixture.Create<int>();
            var intType = onOffsByField.Counts.GetType();

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

        #region General Getters/Setters : Class (OnOffsByField) => Property (Counts) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Class_Invalid_Property_CountsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCounts = "CountsNotPresent";
            var onOffsByField  = Fixture.Create<OnOffsByField>();

            // Act , Assert
            Should.NotThrow(() => onOffsByField.GetType().GetProperty(propertyNameCounts));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Counts_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCounts = "Counts";
            var onOffsByField  = Fixture.Create<OnOffsByField>();
            var propertyInfo  = onOffsByField.GetType().GetProperty(propertyNameCounts);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (OnOffsByField) => Property (Field) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Field_Property_String_Type_Verify_Test()
        {
            // Arrange
            var onOffsByField = Fixture.Create<OnOffsByField>();
            onOffsByField.Field = Fixture.Create<string>();
            var stringType = onOffsByField.Field.GetType();

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

        #region General Getters/Setters : Class (OnOffsByField) => Property (Field) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Class_Invalid_Property_FieldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField = "FieldNotPresent";
            var onOffsByField  = Fixture.Create<OnOffsByField>();

            // Act , Assert
            Should.NotThrow(() => onOffsByField.GetType().GetProperty(propertyNameField));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Field_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField = "Field";
            var onOffsByField  = Fixture.Create<OnOffsByField>();
            var propertyInfo  = onOffsByField.GetType().GetProperty(propertyNameField);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (OnOffsByField) => Property (Months) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Months_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameMonths = "Months";
            var onOffsByField = Fixture.Create<OnOffsByField>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = onOffsByField.GetType().GetProperty(propertyNameMonths);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(onOffsByField, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (OnOffsByField) => Property (Months) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Class_Invalid_Property_MonthsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMonths = "MonthsNotPresent";
            var onOffsByField  = Fixture.Create<OnOffsByField>();

            // Act , Assert
            Should.NotThrow(() => onOffsByField.GetType().GetProperty(propertyNameMonths));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Months_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMonths = "Months";
            var onOffsByField  = Fixture.Create<OnOffsByField>();
            var propertyInfo  = onOffsByField.GetType().GetProperty(propertyNameMonths);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (OnOffsByField) => Property (SubscribeTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_SubscribeTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var onOffsByField = Fixture.Create<OnOffsByField>();
            onOffsByField.SubscribeTypeCode = Fixture.Create<string>();
            var stringType = onOffsByField.SubscribeTypeCode.GetType();

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

        #region General Getters/Setters : Class (OnOffsByField) => Property (SubscribeTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_Class_Invalid_Property_SubscribeTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscribeTypeCode = "SubscribeTypeCodeNotPresent";
            var onOffsByField  = Fixture.Create<OnOffsByField>();

            // Act , Assert
            Should.NotThrow(() => onOffsByField.GetType().GetProperty(propertyNameSubscribeTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void OnOffsByField_SubscribeTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscribeTypeCode = "SubscribeTypeCode";
            var onOffsByField  = Fixture.Create<OnOffsByField>();
            var propertyInfo  = onOffsByField.GetType().GetProperty(propertyNameSubscribeTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (OnOffsByField) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_OnOffsByField_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new OnOffsByField());
        }

        #endregion

        #region General Constructor : Class (OnOffsByField) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_OnOffsByField_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfOnOffsByField = Fixture.CreateMany<OnOffsByField>(2).ToList();
            var firstOnOffsByField = instancesOfOnOffsByField.FirstOrDefault();
            var lastOnOffsByField = instancesOfOnOffsByField.Last();

            // Act, Assert
            firstOnOffsByField.ShouldNotBeNull();
            lastOnOffsByField.ShouldNotBeNull();
            firstOnOffsByField.ShouldNotBeSameAs(lastOnOffsByField);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_OnOffsByField_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstOnOffsByField = new OnOffsByField();
            var secondOnOffsByField = new OnOffsByField();
            var thirdOnOffsByField = new OnOffsByField();
            var fourthOnOffsByField = new OnOffsByField();
            var fifthOnOffsByField = new OnOffsByField();
            var sixthOnOffsByField = new OnOffsByField();

            // Act, Assert
            firstOnOffsByField.ShouldNotBeNull();
            secondOnOffsByField.ShouldNotBeNull();
            thirdOnOffsByField.ShouldNotBeNull();
            fourthOnOffsByField.ShouldNotBeNull();
            fifthOnOffsByField.ShouldNotBeNull();
            sixthOnOffsByField.ShouldNotBeNull();
            firstOnOffsByField.ShouldNotBeSameAs(secondOnOffsByField);
            thirdOnOffsByField.ShouldNotBeSameAs(firstOnOffsByField);
            fourthOnOffsByField.ShouldNotBeSameAs(firstOnOffsByField);
            fifthOnOffsByField.ShouldNotBeSameAs(firstOnOffsByField);
            sixthOnOffsByField.ShouldNotBeSameAs(firstOnOffsByField);
            sixthOnOffsByField.ShouldNotBeSameAs(fourthOnOffsByField);
        }

        #endregion

        #endregion

        #endregion
    }
}