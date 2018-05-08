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
    public class MasterSuppressionSourceReportDetailsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var masterSuppressionSourceReportDetails = Fixture.Create<MasterSuppressionSourceReportDetails>();
            var suppressionCode = Fixture.Create<string>();
            var emailAddress = Fixture.Create<string>();
            var suppressedDateTime = Fixture.Create<DateTime>();
            var reason = Fixture.Create<string>();

            // Act
            masterSuppressionSourceReportDetails.SuppressionCode = suppressionCode;
            masterSuppressionSourceReportDetails.EmailAddress = emailAddress;
            masterSuppressionSourceReportDetails.SuppressedDateTime = suppressedDateTime;
            masterSuppressionSourceReportDetails.Reason = reason;

            // Assert
            masterSuppressionSourceReportDetails.SuppressionCode.ShouldBe(suppressionCode);
            masterSuppressionSourceReportDetails.EmailAddress.ShouldBe(emailAddress);
            masterSuppressionSourceReportDetails.SuppressedDateTime.ShouldBe(suppressedDateTime);
            masterSuppressionSourceReportDetails.Reason.ShouldBe(reason);
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReportDetails = Fixture.Create<MasterSuppressionSourceReportDetails>();
            masterSuppressionSourceReportDetails.EmailAddress = Fixture.Create<string>();
            var stringType = masterSuppressionSourceReportDetails.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();
            var propertyInfo  = masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (Reason) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Reason_Property_String_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReportDetails = Fixture.Create<MasterSuppressionSourceReportDetails>();
            masterSuppressionSourceReportDetails.Reason = Fixture.Create<string>();
            var stringType = masterSuppressionSourceReportDetails.Reason.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (Reason) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Class_Invalid_Property_ReasonNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReason = "ReasonNotPresent";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameReason));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Reason_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReason = "Reason";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();
            var propertyInfo  = masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameReason);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (SuppressedDateTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_SuppressedDateTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedDateTime = "SuppressedDateTime";
            var masterSuppressionSourceReportDetails = Fixture.Create<MasterSuppressionSourceReportDetails>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameSuppressedDateTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(masterSuppressionSourceReportDetails, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (SuppressedDateTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Class_Invalid_Property_SuppressedDateTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedDateTime = "SuppressedDateTimeNotPresent";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameSuppressedDateTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_SuppressedDateTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedDateTime = "SuppressedDateTime";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();
            var propertyInfo  = masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameSuppressedDateTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (SuppressionCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_SuppressionCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var masterSuppressionSourceReportDetails = Fixture.Create<MasterSuppressionSourceReportDetails>();
            masterSuppressionSourceReportDetails.SuppressionCode = Fixture.Create<string>();
            var stringType = masterSuppressionSourceReportDetails.SuppressionCode.GetType();

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

        #region General Getters/Setters : Class (MasterSuppressionSourceReportDetails) => Property (SuppressionCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_Class_Invalid_Property_SuppressionCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressionCode = "SuppressionCodeNotPresent";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();

            // Act , Assert
            Should.NotThrow(() => masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameSuppressionCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void MasterSuppressionSourceReportDetails_SuppressionCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressionCode = "SuppressionCode";
            var masterSuppressionSourceReportDetails  = Fixture.Create<MasterSuppressionSourceReportDetails>();
            var propertyInfo  = masterSuppressionSourceReportDetails.GetType().GetProperty(propertyNameSuppressionCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}