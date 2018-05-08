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
    public class StatisticsByFieldTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (StatisticsByField) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            var field = Fixture.Create<string>();
            var uSend = Fixture.Create<int>();
            var uHBounce = Fixture.Create<int>();
            var uSBounce = Fixture.Create<int>();
            var tOpen = Fixture.Create<int>();
            var uOpen = Fixture.Create<int>();
            var tClick = Fixture.Create<int>();
            var uClick = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var totalOpenPercentage = Fixture.Create<Decimal>();
            var uniqueOpenPercentage = Fixture.Create<Decimal>();
            var totalClickPercentage = Fixture.Create<Decimal>();
            var uniqueClickPercentage = Fixture.Create<Decimal>();
            var clickThrough = Fixture.Create<int>();
            var clickThroughPercentage = Fixture.Create<Decimal>();

            // Act
            statisticsByField.Field = field;
            statisticsByField.USend = uSend;
            statisticsByField.UHBounce = uHBounce;
            statisticsByField.USBounce = uSBounce;
            statisticsByField.TOpen = tOpen;
            statisticsByField.UOpen = uOpen;
            statisticsByField.TClick = tClick;
            statisticsByField.UClick = uClick;
            statisticsByField.Delivered = delivered;
            statisticsByField.TotalOpenPercentage = totalOpenPercentage;
            statisticsByField.UniqueOpenPercentage = uniqueOpenPercentage;
            statisticsByField.TotalClickPercentage = totalClickPercentage;
            statisticsByField.UniqueClickPercentage = uniqueClickPercentage;
            statisticsByField.ClickThrough = clickThrough;
            statisticsByField.ClickThroughPercentage = clickThroughPercentage;

            // Assert
            statisticsByField.Field.ShouldBe(field);
            statisticsByField.USend.ShouldBe(uSend);
            statisticsByField.UHBounce.ShouldBe(uHBounce);
            statisticsByField.USBounce.ShouldBe(uSBounce);
            statisticsByField.TOpen.ShouldBe(tOpen);
            statisticsByField.UOpen.ShouldBe(uOpen);
            statisticsByField.TClick.ShouldBe(tClick);
            statisticsByField.UClick.ShouldBe(uClick);
            statisticsByField.Delivered.ShouldBe(delivered);
            statisticsByField.TotalOpenPercentage.ShouldBe(totalOpenPercentage);
            statisticsByField.UniqueOpenPercentage.ShouldBe(uniqueOpenPercentage);
            statisticsByField.TotalClickPercentage.ShouldBe(totalClickPercentage);
            statisticsByField.UniqueClickPercentage.ShouldBe(uniqueClickPercentage);
            statisticsByField.ClickThrough.ShouldBe(clickThrough);
            statisticsByField.ClickThroughPercentage.ShouldBe(clickThroughPercentage);
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (ClickThrough) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_ClickThrough_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.ClickThrough = Fixture.Create<int>();
            var intType = statisticsByField.ClickThrough.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (ClickThrough) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_ClickThroughNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThroughNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameClickThrough));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_ClickThrough_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThrough";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameClickThrough);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (ClickThroughPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_ClickThroughPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var statisticsByField = Fixture.Create<StatisticsByField>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = statisticsByField.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(statisticsByField, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (ClickThroughPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_ClickThroughPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentageNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameClickThroughPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_ClickThroughPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.Delivered = Fixture.Create<int>();
            var intType = statisticsByField.Delivered.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (Field) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Field_Property_String_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.Field = Fixture.Create<string>();
            var stringType = statisticsByField.Field.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (Field) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_FieldNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField = "FieldNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameField));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Field_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField = "Field";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameField);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (TClick) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TClick_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.TClick = Fixture.Create<int>();
            var intType = statisticsByField.TClick.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (TClick) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_TClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTClick = "TClickNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameTClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TClick_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTClick = "TClick";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameTClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (TOpen) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TOpen_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.TOpen = Fixture.Create<int>();
            var intType = statisticsByField.TOpen.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (TOpen) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_TOpenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTOpen = "TOpenNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameTOpen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TOpen_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTOpen = "TOpen";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameTOpen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (TotalClickPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TotalClickPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalClickPercentage = "TotalClickPercentage";
            var statisticsByField = Fixture.Create<StatisticsByField>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = statisticsByField.GetType().GetProperty(propertyNameTotalClickPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(statisticsByField, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (TotalClickPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_TotalClickPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalClickPercentage = "TotalClickPercentageNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameTotalClickPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TotalClickPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalClickPercentage = "TotalClickPercentage";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameTotalClickPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (TotalOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TotalOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalOpenPercentage = "TotalOpenPercentage";
            var statisticsByField = Fixture.Create<StatisticsByField>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = statisticsByField.GetType().GetProperty(propertyNameTotalOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(statisticsByField, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (TotalOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_TotalOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalOpenPercentage = "TotalOpenPercentageNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameTotalOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_TotalOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalOpenPercentage = "TotalOpenPercentage";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameTotalOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UClick) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UClick_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.UClick = Fixture.Create<int>();
            var intType = statisticsByField.UClick.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (UClick) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_UClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUClick = "UClickNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UClick_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUClick = "UClick";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UHBounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UHBounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.UHBounce = Fixture.Create<int>();
            var intType = statisticsByField.UHBounce.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (UHBounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_UHBounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUHBounce = "UHBounceNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUHBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UHBounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUHBounce = "UHBounce";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUHBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UniqueClickPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UniqueClickPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClickPercentage = "UniqueClickPercentage";
            var statisticsByField = Fixture.Create<StatisticsByField>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = statisticsByField.GetType().GetProperty(propertyNameUniqueClickPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(statisticsByField, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UniqueClickPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_UniqueClickPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClickPercentage = "UniqueClickPercentageNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUniqueClickPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UniqueClickPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueClickPercentage = "UniqueClickPercentage";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUniqueClickPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UniqueOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UniqueOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpenPercentage = "UniqueOpenPercentage";
            var statisticsByField = Fixture.Create<StatisticsByField>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = statisticsByField.GetType().GetProperty(propertyNameUniqueOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(statisticsByField, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UniqueOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_UniqueOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpenPercentage = "UniqueOpenPercentageNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUniqueOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UniqueOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueOpenPercentage = "UniqueOpenPercentage";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUniqueOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (UOpen) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UOpen_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.UOpen = Fixture.Create<int>();
            var intType = statisticsByField.UOpen.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (UOpen) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_UOpenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUOpen = "UOpenNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUOpen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_UOpen_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUOpen = "UOpen";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUOpen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (USBounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_USBounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.USBounce = Fixture.Create<int>();
            var intType = statisticsByField.USBounce.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (USBounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_USBounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUSBounce = "USBounceNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUSBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_USBounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUSBounce = "USBounce";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUSBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (StatisticsByField) => Property (USend) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_USend_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var statisticsByField = Fixture.Create<StatisticsByField>();
            statisticsByField.USend = Fixture.Create<int>();
            var intType = statisticsByField.USend.GetType();

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

        #region General Getters/Setters : Class (StatisticsByField) => Property (USend) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_Class_Invalid_Property_USendNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUSend = "USendNotPresent";
            var statisticsByField  = Fixture.Create<StatisticsByField>();

            // Act , Assert
            Should.NotThrow(() => statisticsByField.GetType().GetProperty(propertyNameUSend));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void StatisticsByField_USend_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUSend = "USend";
            var statisticsByField  = Fixture.Create<StatisticsByField>();
            var propertyInfo  = statisticsByField.GetType().GetProperty(propertyNameUSend);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (StatisticsByField) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_StatisticsByField_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new StatisticsByField());
        }

        #endregion

        #region General Constructor : Class (StatisticsByField) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_StatisticsByField_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfStatisticsByField = Fixture.CreateMany<StatisticsByField>(2).ToList();
            var firstStatisticsByField = instancesOfStatisticsByField.FirstOrDefault();
            var lastStatisticsByField = instancesOfStatisticsByField.Last();

            // Act, Assert
            firstStatisticsByField.ShouldNotBeNull();
            lastStatisticsByField.ShouldNotBeNull();
            firstStatisticsByField.ShouldNotBeSameAs(lastStatisticsByField);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_StatisticsByField_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstStatisticsByField = new StatisticsByField();
            var secondStatisticsByField = new StatisticsByField();
            var thirdStatisticsByField = new StatisticsByField();
            var fourthStatisticsByField = new StatisticsByField();
            var fifthStatisticsByField = new StatisticsByField();
            var sixthStatisticsByField = new StatisticsByField();

            // Act, Assert
            firstStatisticsByField.ShouldNotBeNull();
            secondStatisticsByField.ShouldNotBeNull();
            thirdStatisticsByField.ShouldNotBeNull();
            fourthStatisticsByField.ShouldNotBeNull();
            fifthStatisticsByField.ShouldNotBeNull();
            sixthStatisticsByField.ShouldNotBeNull();
            firstStatisticsByField.ShouldNotBeSameAs(secondStatisticsByField);
            thirdStatisticsByField.ShouldNotBeSameAs(firstStatisticsByField);
            fourthStatisticsByField.ShouldNotBeSameAs(firstStatisticsByField);
            fifthStatisticsByField.ShouldNotBeSameAs(firstStatisticsByField);
            sixthStatisticsByField.ShouldNotBeSameAs(firstStatisticsByField);
            sixthStatisticsByField.ShouldNotBeSameAs(fourthStatisticsByField);
        }

        #endregion

        #endregion

        #endregion
    }
}