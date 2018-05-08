using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class MAControlTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MAControl) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            var mAControlId = Fixture.Create<int>();
            var controlId = Fixture.Create<string>();
            var eCNId = Fixture.Create<int>();
            var xPosition = Fixture.Create<decimal>();
            var yPosition = Fixture.Create<decimal>();
            var marketingAutomationId = Fixture.Create<int>();
            var text = Fixture.Create<string>();
            var extraText = Fixture.Create<string>();
            var controlType = Fixture.Create<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType>();

            // Act
            mAControl.MAControlID = mAControlId;
            mAControl.ControlID = controlId;
            mAControl.ECNID = eCNId;
            mAControl.xPosition = xPosition;
            mAControl.yPosition = yPosition;
            mAControl.MarketingAutomationID = marketingAutomationId;
            mAControl.Text = text;
            mAControl.ExtraText = extraText;
            mAControl.ControlType = controlType;

            // Assert
            mAControl.MAControlID.ShouldBe(mAControlId);
            mAControl.ControlID.ShouldBe(controlId);
            mAControl.ECNID.ShouldBe(eCNId);
            mAControl.xPosition.ShouldBe(xPosition);
            mAControl.yPosition.ShouldBe(yPosition);
            mAControl.MarketingAutomationID.ShouldBe(marketingAutomationId);
            mAControl.Text.ShouldBe(text);
            mAControl.ExtraText.ShouldBe(extraText);
            mAControl.ControlType.ShouldBe(controlType);
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (ControlID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ControlID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.ControlID = Fixture.Create<string>();
            var stringType = mAControl.ControlID.GetType();

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

        #region General Getters/Setters : Class (MAControl) => Property (ControlID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_ControlIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControlID = "ControlIDNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameControlID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ControlID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControlID = "ControlID";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameControlID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (ControlType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ControlType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameControlType = "ControlType";
            var mAControl = Fixture.Create<MAControl>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = mAControl.GetType().GetProperty(propertyNameControlType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(mAControl, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (ControlType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_ControlTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameControlType = "ControlTypeNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameControlType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ControlType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameControlType = "ControlType";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameControlType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (ECNID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ECNID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.ECNID = Fixture.Create<int>();
            var intType = mAControl.ECNID.GetType();

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

        #region General Getters/Setters : Class (MAControl) => Property (ECNID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_ECNIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameECNID = "ECNIDNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameECNID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ECNID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameECNID = "ECNID";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameECNID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (ExtraText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ExtraText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.ExtraText = Fixture.Create<string>();
            var stringType = mAControl.ExtraText.GetType();

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

        #region General Getters/Setters : Class (MAControl) => Property (ExtraText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_ExtraTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameExtraText = "ExtraTextNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameExtraText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_ExtraText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameExtraText = "ExtraText";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameExtraText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (MAControlID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_MAControlID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.MAControlID = Fixture.Create<int>();
            var intType = mAControl.MAControlID.GetType();

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

        #region General Getters/Setters : Class (MAControl) => Property (MAControlID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_MAControlIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMAControlID = "MAControlIDNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameMAControlID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_MAControlID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMAControlID = "MAControlID";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameMAControlID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (MarketingAutomationID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_MarketingAutomationID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.MarketingAutomationID = Fixture.Create<int>();
            var intType = mAControl.MarketingAutomationID.GetType();

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

        #region General Getters/Setters : Class (MAControl) => Property (MarketingAutomationID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_MarketingAutomationIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationIDNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameMarketingAutomationID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_MarketingAutomationID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMarketingAutomationID = "MarketingAutomationID";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameMarketingAutomationID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (Text) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Text_Property_String_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.Text = Fixture.Create<string>();
            var stringType = mAControl.Text.GetType();

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

        #region General Getters/Setters : Class (MAControl) => Property (Text) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_TextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameText = "TextNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Text_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameText = "Text";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (xPosition) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_xPosition_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.xPosition = Fixture.Create<decimal>();
            var decimalType = mAControl.xPosition.GetType();

            // Act
            var isTypeDecimal = typeof(decimal) == (decimalType);
            var isTypeNullableDecimal = typeof(decimal?) == (decimalType);
            var isTypeString = typeof(string) == (decimalType);
            var isTypeInt = typeof(int) == (decimalType);
            var isTypeLong = typeof(long) == (decimalType);
            var isTypeBool = typeof(bool) == (decimalType);
            var isTypeDouble = typeof(double) == (decimalType);
            var isTypeFloat = typeof(float) == (decimalType);
            var isTypeIntNullable = typeof(int?) == (decimalType);
            var isTypeLongNullable = typeof(long?) == (decimalType);
            var isTypeBoolNullable = typeof(bool?) == (decimalType);
            var isTypeDoubleNullable = typeof(double?) == (decimalType);
            var isTypeFloatNullable = typeof(float?) == (decimalType);

            // Assert
            isTypeDecimal.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDecimal.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (xPosition) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_xPositionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamexPosition = "xPositionNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNamexPosition));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_xPosition_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamexPosition = "xPosition";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNamexPosition);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (yPosition) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_yPosition_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var mAControl = Fixture.Create<MAControl>();
            mAControl.yPosition = Fixture.Create<decimal>();
            var decimalType = mAControl.yPosition.GetType();

            // Act
            var isTypeDecimal = typeof(decimal) == (decimalType);
            var isTypeNullableDecimal = typeof(decimal?) == (decimalType);
            var isTypeString = typeof(string) == (decimalType);
            var isTypeInt = typeof(int) == (decimalType);
            var isTypeLong = typeof(long) == (decimalType);
            var isTypeBool = typeof(bool) == (decimalType);
            var isTypeDouble = typeof(double) == (decimalType);
            var isTypeFloat = typeof(float) == (decimalType);
            var isTypeIntNullable = typeof(int?) == (decimalType);
            var isTypeLongNullable = typeof(long?) == (decimalType);
            var isTypeBoolNullable = typeof(bool?) == (decimalType);
            var isTypeDoubleNullable = typeof(double?) == (decimalType);
            var isTypeFloatNullable = typeof(float?) == (decimalType);

            // Assert
            isTypeDecimal.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDecimal.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (MAControl) => Property (yPosition) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_Class_Invalid_Property_yPositionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameyPosition = "yPositionNotPresent";
            var mAControl  = Fixture.Create<MAControl>();

            // Act , Assert
            Should.NotThrow(() => mAControl.GetType().GetProperty(propertyNameyPosition));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MAControl_yPosition_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameyPosition = "yPosition";
            var mAControl  = Fixture.Create<MAControl>();
            var propertyInfo  = mAControl.GetType().GetProperty(propertyNameyPosition);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (MAControl) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAControl_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new MAControl());
        }

        #endregion

        #region General Constructor : Class (MAControl) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAControl_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfMAControl = Fixture.CreateMany<MAControl>(2).ToList();
            var firstMAControl = instancesOfMAControl.FirstOrDefault();
            var lastMAControl = instancesOfMAControl.Last();

            // Act, Assert
            firstMAControl.ShouldNotBeNull();
            lastMAControl.ShouldNotBeNull();
            firstMAControl.ShouldNotBeSameAs(lastMAControl);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAControl_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstMAControl = new MAControl();
            var secondMAControl = new MAControl();
            var thirdMAControl = new MAControl();
            var fourthMAControl = new MAControl();
            var fifthMAControl = new MAControl();
            var sixthMAControl = new MAControl();

            // Act, Assert
            firstMAControl.ShouldNotBeNull();
            secondMAControl.ShouldNotBeNull();
            thirdMAControl.ShouldNotBeNull();
            fourthMAControl.ShouldNotBeNull();
            fifthMAControl.ShouldNotBeNull();
            sixthMAControl.ShouldNotBeNull();
            firstMAControl.ShouldNotBeSameAs(secondMAControl);
            thirdMAControl.ShouldNotBeSameAs(firstMAControl);
            fourthMAControl.ShouldNotBeSameAs(firstMAControl);
            fifthMAControl.ShouldNotBeSameAs(firstMAControl);
            sixthMAControl.ShouldNotBeSameAs(firstMAControl);
            sixthMAControl.ShouldNotBeSameAs(fourthMAControl);
        }

        #endregion

        #region General Constructor : Class (MAControl) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_MAControl_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var mAControlId = -1;
            var controlId = "";
            var eCNId = -1;
            var xPosition = 0.0M;
            var yPosition = 0.0M;
            var marketingAutomationId = -1;
            var text = "";
            var extraText = "";

            // Act
            var mAControl = new MAControl();

            // Assert
            mAControl.MAControlID.ShouldBe(mAControlId);
            mAControl.ControlID.ShouldBe(controlId);
            mAControl.ECNID.ShouldBe(eCNId);
            mAControl.xPosition.ShouldBe(xPosition);
            mAControl.yPosition.ShouldBe(yPosition);
            mAControl.MarketingAutomationID.ShouldBe(marketingAutomationId);
            mAControl.Text.ShouldBe(text);
            mAControl.ExtraText.ShouldBe(extraText);
        }

        #endregion

        #endregion

        #endregion
    }
}