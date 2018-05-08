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
    public class BlastActivityOpensReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivityOpensReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivityOpensReport = Fixture.Create<BlastActivityOpensReport>();
            var usage = Fixture.Create<string>();
            var opens = Fixture.Create<int>();
            var emailClientName = Fixture.Create<string>();
            var platformName = Fixture.Create<string>();

            // Act
            blastActivityOpensReport.Usage = usage;
            blastActivityOpensReport.Opens = opens;
            blastActivityOpensReport.EmailClientName = emailClientName;
            blastActivityOpensReport.PlatformName = platformName;

            // Assert
            blastActivityOpensReport.Usage.ShouldBe(usage);
            blastActivityOpensReport.Opens.ShouldBe(opens);
            blastActivityOpensReport.EmailClientName.ShouldBe(emailClientName);
            blastActivityOpensReport.PlatformName.ShouldBe(platformName);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (EmailClientName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_EmailClientName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpensReport = Fixture.Create<BlastActivityOpensReport>();
            blastActivityOpensReport.EmailClientName = Fixture.Create<string>();
            var stringType = blastActivityOpensReport.EmailClientName.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (EmailClientName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Class_Invalid_Property_EmailClientNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailClientName = "EmailClientNameNotPresent";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpensReport.GetType().GetProperty(propertyNameEmailClientName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_EmailClientName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailClientName = "EmailClientName";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();
            var propertyInfo  = blastActivityOpensReport.GetType().GetProperty(propertyNameEmailClientName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (Opens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Opens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpensReport = Fixture.Create<BlastActivityOpensReport>();
            blastActivityOpensReport.Opens = Fixture.Create<int>();
            var intType = blastActivityOpensReport.Opens.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (Opens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Class_Invalid_Property_OpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpens = "OpensNotPresent";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpensReport.GetType().GetProperty(propertyNameOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Opens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpens = "Opens";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();
            var propertyInfo  = blastActivityOpensReport.GetType().GetProperty(propertyNameOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (PlatformName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_PlatformName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpensReport = Fixture.Create<BlastActivityOpensReport>();
            blastActivityOpensReport.PlatformName = Fixture.Create<string>();
            var stringType = blastActivityOpensReport.PlatformName.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (PlatformName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Class_Invalid_Property_PlatformNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformName = "PlatformNameNotPresent";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpensReport.GetType().GetProperty(propertyNamePlatformName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_PlatformName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformName = "PlatformName";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();
            var propertyInfo  = blastActivityOpensReport.GetType().GetProperty(propertyNamePlatformName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (Usage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Usage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivityOpensReport = Fixture.Create<BlastActivityOpensReport>();
            blastActivityOpensReport.Usage = Fixture.Create<string>();
            var stringType = blastActivityOpensReport.Usage.GetType();

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

        #region General Getters/Setters : Class (BlastActivityOpensReport) => Property (Usage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Class_Invalid_Property_UsageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUsage = "UsageNotPresent";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();

            // Act , Assert
            Should.NotThrow(() => blastActivityOpensReport.GetType().GetProperty(propertyNameUsage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivityOpensReport_Usage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUsage = "Usage";
            var blastActivityOpensReport  = Fixture.Create<BlastActivityOpensReport>();
            var propertyInfo  = blastActivityOpensReport.GetType().GetProperty(propertyNameUsage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivityOpensReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityOpensReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivityOpensReport());
        }

        #endregion

        #region General Constructor : Class (BlastActivityOpensReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityOpensReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivityOpensReport = Fixture.CreateMany<BlastActivityOpensReport>(2).ToList();
            var firstBlastActivityOpensReport = instancesOfBlastActivityOpensReport.FirstOrDefault();
            var lastBlastActivityOpensReport = instancesOfBlastActivityOpensReport.Last();

            // Act, Assert
            firstBlastActivityOpensReport.ShouldNotBeNull();
            lastBlastActivityOpensReport.ShouldNotBeNull();
            firstBlastActivityOpensReport.ShouldNotBeSameAs(lastBlastActivityOpensReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivityOpensReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivityOpensReport = new BlastActivityOpensReport();
            var secondBlastActivityOpensReport = new BlastActivityOpensReport();
            var thirdBlastActivityOpensReport = new BlastActivityOpensReport();
            var fourthBlastActivityOpensReport = new BlastActivityOpensReport();
            var fifthBlastActivityOpensReport = new BlastActivityOpensReport();
            var sixthBlastActivityOpensReport = new BlastActivityOpensReport();

            // Act, Assert
            firstBlastActivityOpensReport.ShouldNotBeNull();
            secondBlastActivityOpensReport.ShouldNotBeNull();
            thirdBlastActivityOpensReport.ShouldNotBeNull();
            fourthBlastActivityOpensReport.ShouldNotBeNull();
            fifthBlastActivityOpensReport.ShouldNotBeNull();
            sixthBlastActivityOpensReport.ShouldNotBeNull();
            firstBlastActivityOpensReport.ShouldNotBeSameAs(secondBlastActivityOpensReport);
            thirdBlastActivityOpensReport.ShouldNotBeSameAs(firstBlastActivityOpensReport);
            fourthBlastActivityOpensReport.ShouldNotBeSameAs(firstBlastActivityOpensReport);
            fifthBlastActivityOpensReport.ShouldNotBeSameAs(firstBlastActivityOpensReport);
            sixthBlastActivityOpensReport.ShouldNotBeSameAs(firstBlastActivityOpensReport);
            sixthBlastActivityOpensReport.ShouldNotBeSameAs(fourthBlastActivityOpensReport);
        }

        #endregion

        #endregion

        #endregion
    }
}