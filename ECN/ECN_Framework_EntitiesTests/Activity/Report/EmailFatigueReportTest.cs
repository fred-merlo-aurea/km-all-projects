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
    public class EmailFatigueReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailFatigueReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            var grouping = Fixture.Create<int>();
            var action = Fixture.Create<string>();
            var touch1 = Fixture.Create<int>();
            var touch2 = Fixture.Create<int>();
            var touch3 = Fixture.Create<int>();
            var touch4 = Fixture.Create<int>();
            var touch5 = Fixture.Create<int>();
            var touch6 = Fixture.Create<int>();
            var touch7 = Fixture.Create<int>();
            var touch8 = Fixture.Create<int>();
            var touch9 = Fixture.Create<int>();
            var touch10 = Fixture.Create<int>();
            var touch11_20 = Fixture.Create<int>();
            var touch21_30 = Fixture.Create<int>();
            var touch31_40 = Fixture.Create<int>();
            var touch41_50 = Fixture.Create<int>();
            var touch51Plus = Fixture.Create<int>();

            // Act
            emailFatigueReport.Grouping = grouping;
            emailFatigueReport.Action = action;
            emailFatigueReport.Touch1 = touch1;
            emailFatigueReport.Touch2 = touch2;
            emailFatigueReport.Touch3 = touch3;
            emailFatigueReport.Touch4 = touch4;
            emailFatigueReport.Touch5 = touch5;
            emailFatigueReport.Touch6 = touch6;
            emailFatigueReport.Touch7 = touch7;
            emailFatigueReport.Touch8 = touch8;
            emailFatigueReport.Touch9 = touch9;
            emailFatigueReport.Touch10 = touch10;
            emailFatigueReport.Touch11_20 = touch11_20;
            emailFatigueReport.Touch21_30 = touch21_30;
            emailFatigueReport.Touch31_40 = touch31_40;
            emailFatigueReport.Touch41_50 = touch41_50;
            emailFatigueReport.Touch51Plus = touch51Plus;

            // Assert
            emailFatigueReport.Grouping.ShouldBe(grouping);
            emailFatigueReport.Action.ShouldBe(action);
            emailFatigueReport.Touch1.ShouldBe(touch1);
            emailFatigueReport.Touch2.ShouldBe(touch2);
            emailFatigueReport.Touch3.ShouldBe(touch3);
            emailFatigueReport.Touch4.ShouldBe(touch4);
            emailFatigueReport.Touch5.ShouldBe(touch5);
            emailFatigueReport.Touch6.ShouldBe(touch6);
            emailFatigueReport.Touch7.ShouldBe(touch7);
            emailFatigueReport.Touch8.ShouldBe(touch8);
            emailFatigueReport.Touch9.ShouldBe(touch9);
            emailFatigueReport.Touch10.ShouldBe(touch10);
            emailFatigueReport.Touch11_20.ShouldBe(touch11_20);
            emailFatigueReport.Touch21_30.ShouldBe(touch21_30);
            emailFatigueReport.Touch31_40.ShouldBe(touch31_40);
            emailFatigueReport.Touch41_50.ShouldBe(touch41_50);
            emailFatigueReport.Touch51Plus.ShouldBe(touch51Plus);
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Action) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Action_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Action = Fixture.Create<string>();
            var stringType = emailFatigueReport.Action.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Action) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_ActionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAction = "ActionNotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameAction));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Action_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAction = "Action";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameAction);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Grouping) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Grouping_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Grouping = Fixture.Create<int>();
            var intType = emailFatigueReport.Grouping.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Grouping) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_GroupingNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGrouping = "GroupingNotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameGrouping));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Grouping_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGrouping = "Grouping";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameGrouping);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch1) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch1_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch1 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch1.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch1 = "Touch1NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch1 = "Touch1";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch10) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch10_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch10 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch10.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch10) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch10NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch10 = "Touch10NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch10));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch10_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch10 = "Touch10";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch10);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch11_20) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch11_20_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch11_20 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch11_20.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch11_20) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch11_20NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch11_20 = "Touch11_20NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch11_20));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch11_20_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch11_20 = "Touch11_20";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch11_20);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch2) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch2_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch2 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch2.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch2 = "Touch2NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch2 = "Touch2";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch21_30) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch21_30_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch21_30 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch21_30.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch21_30) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch21_30NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch21_30 = "Touch21_30NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch21_30));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch21_30_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch21_30 = "Touch21_30";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch21_30);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch3) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch3_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch3 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch3.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch3 = "Touch3NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch3 = "Touch3";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch31_40) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch31_40_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch31_40 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch31_40.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch31_40) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch31_40NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch31_40 = "Touch31_40NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch31_40));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch31_40_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch31_40 = "Touch31_40";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch31_40);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch4) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch4_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch4 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch4.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch4 = "Touch4NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch4 = "Touch4";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch41_50) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch41_50_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch41_50 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch41_50.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch41_50) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch41_50NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch41_50 = "Touch41_50NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch41_50));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch41_50_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch41_50 = "Touch41_50";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch41_50);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch5) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch5_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch5 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch5.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch5 = "Touch5NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch5 = "Touch5";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch51Plus) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch51Plus_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch51Plus = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch51Plus.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch51Plus) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch51PlusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch51Plus = "Touch51PlusNotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch51Plus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch51Plus_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch51Plus = "Touch51Plus";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch51Plus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch6) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch6_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch6 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch6.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch6) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch6 = "Touch6NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch6 = "Touch6";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch6);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch7) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch7_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch7 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch7.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch7) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch7NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch7 = "Touch7NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch7));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch7_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch7 = "Touch7";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch7);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch8) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch8_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch8 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch8.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch8) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch8NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch8 = "Touch8NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch8));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch8_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch8 = "Touch8";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch8);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch9) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch9_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailFatigueReport = Fixture.Create<EmailFatigueReport>();
            emailFatigueReport.Touch9 = Fixture.Create<int>();
            var intType = emailFatigueReport.Touch9.GetType();

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

        #region General Getters/Setters : Class (EmailFatigueReport) => Property (Touch9) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Class_Invalid_Property_Touch9NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTouch9 = "Touch9NotPresent";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();

            // Act , Assert
            Should.NotThrow(() => emailFatigueReport.GetType().GetProperty(propertyNameTouch9));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailFatigueReport_Touch9_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTouch9 = "Touch9";
            var emailFatigueReport  = Fixture.Create<EmailFatigueReport>();
            var propertyInfo  = emailFatigueReport.GetType().GetProperty(propertyNameTouch9);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailFatigueReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailFatigueReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailFatigueReport());
        }

        #endregion

        #region General Constructor : Class (EmailFatigueReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailFatigueReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailFatigueReport = Fixture.CreateMany<EmailFatigueReport>(2).ToList();
            var firstEmailFatigueReport = instancesOfEmailFatigueReport.FirstOrDefault();
            var lastEmailFatigueReport = instancesOfEmailFatigueReport.Last();

            // Act, Assert
            firstEmailFatigueReport.ShouldNotBeNull();
            lastEmailFatigueReport.ShouldNotBeNull();
            firstEmailFatigueReport.ShouldNotBeSameAs(lastEmailFatigueReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailFatigueReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailFatigueReport = new EmailFatigueReport();
            var secondEmailFatigueReport = new EmailFatigueReport();
            var thirdEmailFatigueReport = new EmailFatigueReport();
            var fourthEmailFatigueReport = new EmailFatigueReport();
            var fifthEmailFatigueReport = new EmailFatigueReport();
            var sixthEmailFatigueReport = new EmailFatigueReport();

            // Act, Assert
            firstEmailFatigueReport.ShouldNotBeNull();
            secondEmailFatigueReport.ShouldNotBeNull();
            thirdEmailFatigueReport.ShouldNotBeNull();
            fourthEmailFatigueReport.ShouldNotBeNull();
            fifthEmailFatigueReport.ShouldNotBeNull();
            sixthEmailFatigueReport.ShouldNotBeNull();
            firstEmailFatigueReport.ShouldNotBeSameAs(secondEmailFatigueReport);
            thirdEmailFatigueReport.ShouldNotBeSameAs(firstEmailFatigueReport);
            fourthEmailFatigueReport.ShouldNotBeSameAs(firstEmailFatigueReport);
            fifthEmailFatigueReport.ShouldNotBeSameAs(firstEmailFatigueReport);
            sixthEmailFatigueReport.ShouldNotBeSameAs(firstEmailFatigueReport);
            sixthEmailFatigueReport.ShouldNotBeSameAs(fourthEmailFatigueReport);
        }

        #endregion

        #endregion

        #endregion
    }
}