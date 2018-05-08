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
    public class FlashReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (FlashReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var flashReport = Fixture.Create<FlashReport>();
            var promoCode = Fixture.Create<string>();
            var totalEmails = Fixture.Create<int>();
            var uniqueEmails = Fixture.Create<int>();

            // Act
            flashReport.PromoCode = promoCode;
            flashReport.TotalEmails = totalEmails;
            flashReport.UniqueEmails = uniqueEmails;

            // Assert
            flashReport.PromoCode.ShouldBe(promoCode);
            flashReport.TotalEmails.ShouldBe(totalEmails);
            flashReport.UniqueEmails.ShouldBe(uniqueEmails);
        }

        #endregion

        #region General Getters/Setters : Class (FlashReport) => Property (PromoCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_PromoCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var flashReport = Fixture.Create<FlashReport>();
            flashReport.PromoCode = Fixture.Create<string>();
            var stringType = flashReport.PromoCode.GetType();

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

        #region General Getters/Setters : Class (FlashReport) => Property (PromoCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_Class_Invalid_Property_PromoCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePromoCode = "PromoCodeNotPresent";
            var flashReport  = Fixture.Create<FlashReport>();

            // Act , Assert
            Should.NotThrow(() => flashReport.GetType().GetProperty(propertyNamePromoCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_PromoCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePromoCode = "PromoCode";
            var flashReport  = Fixture.Create<FlashReport>();
            var propertyInfo  = flashReport.GetType().GetProperty(propertyNamePromoCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FlashReport) => Property (TotalEmails) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_TotalEmails_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var flashReport = Fixture.Create<FlashReport>();
            flashReport.TotalEmails = Fixture.Create<int>();
            var intType = flashReport.TotalEmails.GetType();

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

        #region General Getters/Setters : Class (FlashReport) => Property (TotalEmails) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_Class_Invalid_Property_TotalEmailsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalEmails = "TotalEmailsNotPresent";
            var flashReport  = Fixture.Create<FlashReport>();

            // Act , Assert
            Should.NotThrow(() => flashReport.GetType().GetProperty(propertyNameTotalEmails));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_TotalEmails_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalEmails = "TotalEmails";
            var flashReport  = Fixture.Create<FlashReport>();
            var propertyInfo  = flashReport.GetType().GetProperty(propertyNameTotalEmails);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (FlashReport) => Property (UniqueEmails) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_UniqueEmails_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var flashReport = Fixture.Create<FlashReport>();
            flashReport.UniqueEmails = Fixture.Create<int>();
            var intType = flashReport.UniqueEmails.GetType();

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

        #region General Getters/Setters : Class (FlashReport) => Property (UniqueEmails) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_Class_Invalid_Property_UniqueEmailsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueEmails = "UniqueEmailsNotPresent";
            var flashReport  = Fixture.Create<FlashReport>();

            // Act , Assert
            Should.NotThrow(() => flashReport.GetType().GetProperty(propertyNameUniqueEmails));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void FlashReport_UniqueEmails_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueEmails = "UniqueEmails";
            var flashReport  = Fixture.Create<FlashReport>();
            var propertyInfo  = flashReport.GetType().GetProperty(propertyNameUniqueEmails);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (FlashReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FlashReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new FlashReport());
        }

        #endregion

        #region General Constructor : Class (FlashReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FlashReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfFlashReport = Fixture.CreateMany<FlashReport>(2).ToList();
            var firstFlashReport = instancesOfFlashReport.FirstOrDefault();
            var lastFlashReport = instancesOfFlashReport.Last();

            // Act, Assert
            firstFlashReport.ShouldNotBeNull();
            lastFlashReport.ShouldNotBeNull();
            firstFlashReport.ShouldNotBeSameAs(lastFlashReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_FlashReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstFlashReport = new FlashReport();
            var secondFlashReport = new FlashReport();
            var thirdFlashReport = new FlashReport();
            var fourthFlashReport = new FlashReport();
            var fifthFlashReport = new FlashReport();
            var sixthFlashReport = new FlashReport();

            // Act, Assert
            firstFlashReport.ShouldNotBeNull();
            secondFlashReport.ShouldNotBeNull();
            thirdFlashReport.ShouldNotBeNull();
            fourthFlashReport.ShouldNotBeNull();
            fifthFlashReport.ShouldNotBeNull();
            sixthFlashReport.ShouldNotBeNull();
            firstFlashReport.ShouldNotBeSameAs(secondFlashReport);
            thirdFlashReport.ShouldNotBeSameAs(firstFlashReport);
            fourthFlashReport.ShouldNotBeSameAs(firstFlashReport);
            fifthFlashReport.ShouldNotBeSameAs(firstFlashReport);
            sixthFlashReport.ShouldNotBeSameAs(firstFlashReport);
            sixthFlashReport.ShouldNotBeSameAs(fourthFlashReport);
        }

        #endregion

        #endregion

        #endregion
    }
}