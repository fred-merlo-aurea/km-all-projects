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
    public class EmailsDeliveredByPercentageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailsDeliveredByPercentage = Fixture.Create<EmailsDeliveredByPercentage>();
            var range = Fixture.Create<string>();
            var totalCount = Fixture.Create<int>();
            var percentage = Fixture.Create<decimal>();

            // Act
            emailsDeliveredByPercentage.Range = range;
            emailsDeliveredByPercentage.TotalCount = totalCount;
            emailsDeliveredByPercentage.Percentage = percentage;

            // Assert
            emailsDeliveredByPercentage.Range.ShouldBe(range);
            emailsDeliveredByPercentage.TotalCount.ShouldBe(totalCount);
            emailsDeliveredByPercentage.Percentage.ShouldBe(percentage);
        }

        #endregion

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => Property (Percentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Percentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailsDeliveredByPercentage = Fixture.Create<EmailsDeliveredByPercentage>();
            emailsDeliveredByPercentage.Percentage = Fixture.Create<decimal>();
            var decimalType = emailsDeliveredByPercentage.Percentage.GetType();

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

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => Property (Percentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Class_Invalid_Property_PercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePercentage = "PercentageNotPresent";
            var emailsDeliveredByPercentage  = Fixture.Create<EmailsDeliveredByPercentage>();

            // Act , Assert
            Should.NotThrow(() => emailsDeliveredByPercentage.GetType().GetProperty(propertyNamePercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Percentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePercentage = "Percentage";
            var emailsDeliveredByPercentage  = Fixture.Create<EmailsDeliveredByPercentage>();
            var propertyInfo  = emailsDeliveredByPercentage.GetType().GetProperty(propertyNamePercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => Property (Range) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Range_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailsDeliveredByPercentage = Fixture.Create<EmailsDeliveredByPercentage>();
            emailsDeliveredByPercentage.Range = Fixture.Create<string>();
            var stringType = emailsDeliveredByPercentage.Range.GetType();

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

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => Property (Range) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Class_Invalid_Property_RangeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRange = "RangeNotPresent";
            var emailsDeliveredByPercentage  = Fixture.Create<EmailsDeliveredByPercentage>();

            // Act , Assert
            Should.NotThrow(() => emailsDeliveredByPercentage.GetType().GetProperty(propertyNameRange));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Range_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRange = "Range";
            var emailsDeliveredByPercentage  = Fixture.Create<EmailsDeliveredByPercentage>();
            var propertyInfo  = emailsDeliveredByPercentage.GetType().GetProperty(propertyNameRange);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => Property (TotalCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_TotalCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailsDeliveredByPercentage = Fixture.Create<EmailsDeliveredByPercentage>();
            emailsDeliveredByPercentage.TotalCount = Fixture.Create<int>();
            var intType = emailsDeliveredByPercentage.TotalCount.GetType();

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

        #region General Getters/Setters : Class (EmailsDeliveredByPercentage) => Property (TotalCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_Class_Invalid_Property_TotalCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCountNotPresent";
            var emailsDeliveredByPercentage  = Fixture.Create<EmailsDeliveredByPercentage>();

            // Act , Assert
            Should.NotThrow(() => emailsDeliveredByPercentage.GetType().GetProperty(propertyNameTotalCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailsDeliveredByPercentage_TotalCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCount = "TotalCount";
            var emailsDeliveredByPercentage  = Fixture.Create<EmailsDeliveredByPercentage>();
            var propertyInfo  = emailsDeliveredByPercentage.GetType().GetProperty(propertyNameTotalCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailsDeliveredByPercentage) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailsDeliveredByPercentage_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailsDeliveredByPercentage());
        }

        #endregion

        #region General Constructor : Class (EmailsDeliveredByPercentage) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailsDeliveredByPercentage_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailsDeliveredByPercentage = Fixture.CreateMany<EmailsDeliveredByPercentage>(2).ToList();
            var firstEmailsDeliveredByPercentage = instancesOfEmailsDeliveredByPercentage.FirstOrDefault();
            var lastEmailsDeliveredByPercentage = instancesOfEmailsDeliveredByPercentage.Last();

            // Act, Assert
            firstEmailsDeliveredByPercentage.ShouldNotBeNull();
            lastEmailsDeliveredByPercentage.ShouldNotBeNull();
            firstEmailsDeliveredByPercentage.ShouldNotBeSameAs(lastEmailsDeliveredByPercentage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailsDeliveredByPercentage_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailsDeliveredByPercentage = new EmailsDeliveredByPercentage();
            var secondEmailsDeliveredByPercentage = new EmailsDeliveredByPercentage();
            var thirdEmailsDeliveredByPercentage = new EmailsDeliveredByPercentage();
            var fourthEmailsDeliveredByPercentage = new EmailsDeliveredByPercentage();
            var fifthEmailsDeliveredByPercentage = new EmailsDeliveredByPercentage();
            var sixthEmailsDeliveredByPercentage = new EmailsDeliveredByPercentage();

            // Act, Assert
            firstEmailsDeliveredByPercentage.ShouldNotBeNull();
            secondEmailsDeliveredByPercentage.ShouldNotBeNull();
            thirdEmailsDeliveredByPercentage.ShouldNotBeNull();
            fourthEmailsDeliveredByPercentage.ShouldNotBeNull();
            fifthEmailsDeliveredByPercentage.ShouldNotBeNull();
            sixthEmailsDeliveredByPercentage.ShouldNotBeNull();
            firstEmailsDeliveredByPercentage.ShouldNotBeSameAs(secondEmailsDeliveredByPercentage);
            thirdEmailsDeliveredByPercentage.ShouldNotBeSameAs(firstEmailsDeliveredByPercentage);
            fourthEmailsDeliveredByPercentage.ShouldNotBeSameAs(firstEmailsDeliveredByPercentage);
            fifthEmailsDeliveredByPercentage.ShouldNotBeSameAs(firstEmailsDeliveredByPercentage);
            sixthEmailsDeliveredByPercentage.ShouldNotBeSameAs(firstEmailsDeliveredByPercentage);
            sixthEmailsDeliveredByPercentage.ShouldNotBeSameAs(fourthEmailsDeliveredByPercentage);
        }

        #endregion

        #endregion

        #endregion
    }
}