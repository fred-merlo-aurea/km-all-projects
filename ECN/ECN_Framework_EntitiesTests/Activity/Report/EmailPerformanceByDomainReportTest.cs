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
    public class EmailPerformanceByDomainTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            var domain = Fixture.Create<string>();
            var sendTotal = Fixture.Create<int>();
            var sendTotalPercentage = Fixture.Create<decimal>();
            var opens = Fixture.Create<int>();
            var opensPercentage = Fixture.Create<decimal>();
            var clicks = Fixture.Create<int>();
            var clicksPercentage = Fixture.Create<decimal>();
            var bounce = Fixture.Create<int>();
            var bouncePercentage = Fixture.Create<decimal>();
            var unsubscribe = Fixture.Create<int>();
            var unsubscribePercentage = Fixture.Create<decimal>();
            var forward = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var deliveredPercentage = Fixture.Create<decimal>();

            // Act
            emailPerformanceByDomain.Domain = domain;
            emailPerformanceByDomain.SendTotal = sendTotal;
            emailPerformanceByDomain.SendTotalPercentage = sendTotalPercentage;
            emailPerformanceByDomain.Opens = opens;
            emailPerformanceByDomain.OpensPercentage = opensPercentage;
            emailPerformanceByDomain.Clicks = clicks;
            emailPerformanceByDomain.ClicksPercentage = clicksPercentage;
            emailPerformanceByDomain.Bounce = bounce;
            emailPerformanceByDomain.BouncePercentage = bouncePercentage;
            emailPerformanceByDomain.Unsubscribe = unsubscribe;
            emailPerformanceByDomain.UnsubscribePercentage = unsubscribePercentage;
            emailPerformanceByDomain.Forward = forward;
            emailPerformanceByDomain.Delivered = delivered;
            emailPerformanceByDomain.DeliveredPercentage = deliveredPercentage;

            // Assert
            emailPerformanceByDomain.Domain.ShouldBe(domain);
            emailPerformanceByDomain.SendTotal.ShouldBe(sendTotal);
            emailPerformanceByDomain.SendTotalPercentage.ShouldBe(sendTotalPercentage);
            emailPerformanceByDomain.Opens.ShouldBe(opens);
            emailPerformanceByDomain.OpensPercentage.ShouldBe(opensPercentage);
            emailPerformanceByDomain.Clicks.ShouldBe(clicks);
            emailPerformanceByDomain.ClicksPercentage.ShouldBe(clicksPercentage);
            emailPerformanceByDomain.Bounce.ShouldBe(bounce);
            emailPerformanceByDomain.BouncePercentage.ShouldBe(bouncePercentage);
            emailPerformanceByDomain.Unsubscribe.ShouldBe(unsubscribe);
            emailPerformanceByDomain.UnsubscribePercentage.ShouldBe(unsubscribePercentage);
            emailPerformanceByDomain.Forward.ShouldBe(forward);
            emailPerformanceByDomain.Delivered.ShouldBe(delivered);
            emailPerformanceByDomain.DeliveredPercentage.ShouldBe(deliveredPercentage);
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Bounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Bounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Bounce = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.Bounce.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Bounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_BounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounce = "BounceNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Bounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounce = "Bounce";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (BouncePercentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_BouncePercentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.BouncePercentage = Fixture.Create<decimal>();
            var decimalType = emailPerformanceByDomain.BouncePercentage.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (BouncePercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_BouncePercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBouncePercentage = "BouncePercentageNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameBouncePercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_BouncePercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBouncePercentage = "BouncePercentage";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameBouncePercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Clicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Clicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Clicks = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.Clicks.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Clicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_ClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicks = "ClicksNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Clicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicks = "Clicks";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (ClicksPercentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_ClicksPercentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.ClicksPercentage = Fixture.Create<decimal>();
            var decimalType = emailPerformanceByDomain.ClicksPercentage.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (ClicksPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_ClicksPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicksPercentage = "ClicksPercentageNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameClicksPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_ClicksPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicksPercentage = "ClicksPercentage";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameClicksPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Delivered = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.Delivered.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (DeliveredPercentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_DeliveredPercentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.DeliveredPercentage = Fixture.Create<decimal>();
            var decimalType = emailPerformanceByDomain.DeliveredPercentage.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (DeliveredPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_DeliveredPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDeliveredPercentage = "DeliveredPercentageNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameDeliveredPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_DeliveredPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDeliveredPercentage = "DeliveredPercentage";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameDeliveredPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Domain) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Domain_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Domain = Fixture.Create<string>();
            var stringType = emailPerformanceByDomain.Domain.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Domain) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_DomainNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDomain = "DomainNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameDomain));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Domain_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDomain = "Domain";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameDomain);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Forward) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Forward_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Forward = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.Forward.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Forward) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_ForwardNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameForward = "ForwardNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameForward));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Forward_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameForward = "Forward";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameForward);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Opens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Opens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Opens = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.Opens.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Opens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_OpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpens = "OpensNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Opens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpens = "Opens";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (OpensPercentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_OpensPercentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.OpensPercentage = Fixture.Create<decimal>();
            var decimalType = emailPerformanceByDomain.OpensPercentage.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (OpensPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_OpensPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpensPercentage = "OpensPercentageNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameOpensPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_OpensPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpensPercentage = "OpensPercentage";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameOpensPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (SendTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_SendTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.SendTotal = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.SendTotal.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (SendTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_SendTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotalNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameSendTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_SendTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotal";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameSendTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (SendTotalPercentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_SendTotalPercentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.SendTotalPercentage = Fixture.Create<decimal>();
            var decimalType = emailPerformanceByDomain.SendTotalPercentage.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (SendTotalPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_SendTotalPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotalPercentage = "SendTotalPercentageNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameSendTotalPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_SendTotalPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotalPercentage = "SendTotalPercentage";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameSendTotalPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Unsubscribe) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Unsubscribe_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.Unsubscribe = Fixture.Create<int>();
            var intType = emailPerformanceByDomain.Unsubscribe.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (Unsubscribe) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_UnsubscribeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribe = "UnsubscribeNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameUnsubscribe));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Unsubscribe_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribe = "Unsubscribe";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameUnsubscribe);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (UnsubscribePercentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_UnsubscribePercentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var emailPerformanceByDomain = Fixture.Create<EmailPerformanceByDomain>();
            emailPerformanceByDomain.UnsubscribePercentage = Fixture.Create<decimal>();
            var decimalType = emailPerformanceByDomain.UnsubscribePercentage.GetType();

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

        #region General Getters/Setters : Class (EmailPerformanceByDomain) => Property (UnsubscribePercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_Class_Invalid_Property_UnsubscribePercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribePercentage = "UnsubscribePercentageNotPresent";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();

            // Act , Assert
            Should.NotThrow(() => emailPerformanceByDomain.GetType().GetProperty(propertyNameUnsubscribePercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPerformanceByDomain_UnsubscribePercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribePercentage = "UnsubscribePercentage";
            var emailPerformanceByDomain  = Fixture.Create<EmailPerformanceByDomain>();
            var propertyInfo  = emailPerformanceByDomain.GetType().GetProperty(propertyNameUnsubscribePercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailPerformanceByDomain) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPerformanceByDomain_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailPerformanceByDomain());
        }

        #endregion

        #region General Constructor : Class (EmailPerformanceByDomain) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPerformanceByDomain_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailPerformanceByDomain = Fixture.CreateMany<EmailPerformanceByDomain>(2).ToList();
            var firstEmailPerformanceByDomain = instancesOfEmailPerformanceByDomain.FirstOrDefault();
            var lastEmailPerformanceByDomain = instancesOfEmailPerformanceByDomain.Last();

            // Act, Assert
            firstEmailPerformanceByDomain.ShouldNotBeNull();
            lastEmailPerformanceByDomain.ShouldNotBeNull();
            firstEmailPerformanceByDomain.ShouldNotBeSameAs(lastEmailPerformanceByDomain);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPerformanceByDomain_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailPerformanceByDomain = new EmailPerformanceByDomain();
            var secondEmailPerformanceByDomain = new EmailPerformanceByDomain();
            var thirdEmailPerformanceByDomain = new EmailPerformanceByDomain();
            var fourthEmailPerformanceByDomain = new EmailPerformanceByDomain();
            var fifthEmailPerformanceByDomain = new EmailPerformanceByDomain();
            var sixthEmailPerformanceByDomain = new EmailPerformanceByDomain();

            // Act, Assert
            firstEmailPerformanceByDomain.ShouldNotBeNull();
            secondEmailPerformanceByDomain.ShouldNotBeNull();
            thirdEmailPerformanceByDomain.ShouldNotBeNull();
            fourthEmailPerformanceByDomain.ShouldNotBeNull();
            fifthEmailPerformanceByDomain.ShouldNotBeNull();
            sixthEmailPerformanceByDomain.ShouldNotBeNull();
            firstEmailPerformanceByDomain.ShouldNotBeSameAs(secondEmailPerformanceByDomain);
            thirdEmailPerformanceByDomain.ShouldNotBeSameAs(firstEmailPerformanceByDomain);
            fourthEmailPerformanceByDomain.ShouldNotBeSameAs(firstEmailPerformanceByDomain);
            fifthEmailPerformanceByDomain.ShouldNotBeSameAs(firstEmailPerformanceByDomain);
            sixthEmailPerformanceByDomain.ShouldNotBeSameAs(firstEmailPerformanceByDomain);
            sixthEmailPerformanceByDomain.ShouldNotBeSameAs(fourthEmailPerformanceByDomain);
        }

        #endregion

        #endregion

        #endregion
    }
}