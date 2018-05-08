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
    public class PlatformDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (PlatformDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var platformDetail = Fixture.Create<PlatformDetail>();
            var platformName = Fixture.Create<string>();
            var column1 = Fixture.Create<int>();
            var usage = Fixture.Create<string>();
            var emailClientName = Fixture.Create<string>();

            // Act
            platformDetail.PlatformName = platformName;
            platformDetail.Column1 = column1;
            platformDetail.Usage = usage;
            platformDetail.EmailClientName = emailClientName;

            // Assert
            platformDetail.PlatformName.ShouldBe(platformName);
            platformDetail.Column1.ShouldBe(column1);
            platformDetail.Usage.ShouldBe(usage);
            platformDetail.EmailClientName.ShouldBe(emailClientName);
        }

        #endregion

        #region General Getters/Setters : Class (PlatformDetail) => Property (Column1) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Column1_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var platformDetail = Fixture.Create<PlatformDetail>();
            platformDetail.Column1 = Fixture.Create<int>();
            var intType = platformDetail.Column1.GetType();

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

        #region General Getters/Setters : Class (PlatformDetail) => Property (Column1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Class_Invalid_Property_Column1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameColumn1 = "Column1NotPresent";
            var platformDetail  = Fixture.Create<PlatformDetail>();

            // Act , Assert
            Should.NotThrow(() => platformDetail.GetType().GetProperty(propertyNameColumn1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Column1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameColumn1 = "Column1";
            var platformDetail  = Fixture.Create<PlatformDetail>();
            var propertyInfo  = platformDetail.GetType().GetProperty(propertyNameColumn1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PlatformDetail) => Property (EmailClientName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_EmailClientName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var platformDetail = Fixture.Create<PlatformDetail>();
            platformDetail.EmailClientName = Fixture.Create<string>();
            var stringType = platformDetail.EmailClientName.GetType();

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

        #region General Getters/Setters : Class (PlatformDetail) => Property (EmailClientName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Class_Invalid_Property_EmailClientNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailClientName = "EmailClientNameNotPresent";
            var platformDetail  = Fixture.Create<PlatformDetail>();

            // Act , Assert
            Should.NotThrow(() => platformDetail.GetType().GetProperty(propertyNameEmailClientName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_EmailClientName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailClientName = "EmailClientName";
            var platformDetail  = Fixture.Create<PlatformDetail>();
            var propertyInfo  = platformDetail.GetType().GetProperty(propertyNameEmailClientName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PlatformDetail) => Property (PlatformName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_PlatformName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var platformDetail = Fixture.Create<PlatformDetail>();
            platformDetail.PlatformName = Fixture.Create<string>();
            var stringType = platformDetail.PlatformName.GetType();

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

        #region General Getters/Setters : Class (PlatformDetail) => Property (PlatformName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Class_Invalid_Property_PlatformNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformName = "PlatformNameNotPresent";
            var platformDetail  = Fixture.Create<PlatformDetail>();

            // Act , Assert
            Should.NotThrow(() => platformDetail.GetType().GetProperty(propertyNamePlatformName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_PlatformName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformName = "PlatformName";
            var platformDetail  = Fixture.Create<PlatformDetail>();
            var propertyInfo  = platformDetail.GetType().GetProperty(propertyNamePlatformName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PlatformDetail) => Property (Usage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Usage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var platformDetail = Fixture.Create<PlatformDetail>();
            platformDetail.Usage = Fixture.Create<string>();
            var stringType = platformDetail.Usage.GetType();

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

        #region General Getters/Setters : Class (PlatformDetail) => Property (Usage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Class_Invalid_Property_UsageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUsage = "UsageNotPresent";
            var platformDetail  = Fixture.Create<PlatformDetail>();

            // Act , Assert
            Should.NotThrow(() => platformDetail.GetType().GetProperty(propertyNameUsage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PlatformDetail_Usage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUsage = "Usage";
            var platformDetail  = Fixture.Create<PlatformDetail>();
            var propertyInfo  = platformDetail.GetType().GetProperty(propertyNameUsage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (PlatformDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PlatformDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new PlatformDetail());
        }

        #endregion

        #region General Constructor : Class (PlatformDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PlatformDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfPlatformDetail = Fixture.CreateMany<PlatformDetail>(2).ToList();
            var firstPlatformDetail = instancesOfPlatformDetail.FirstOrDefault();
            var lastPlatformDetail = instancesOfPlatformDetail.Last();

            // Act, Assert
            firstPlatformDetail.ShouldNotBeNull();
            lastPlatformDetail.ShouldNotBeNull();
            firstPlatformDetail.ShouldNotBeSameAs(lastPlatformDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PlatformDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstPlatformDetail = new PlatformDetail();
            var secondPlatformDetail = new PlatformDetail();
            var thirdPlatformDetail = new PlatformDetail();
            var fourthPlatformDetail = new PlatformDetail();
            var fifthPlatformDetail = new PlatformDetail();
            var sixthPlatformDetail = new PlatformDetail();

            // Act, Assert
            firstPlatformDetail.ShouldNotBeNull();
            secondPlatformDetail.ShouldNotBeNull();
            thirdPlatformDetail.ShouldNotBeNull();
            fourthPlatformDetail.ShouldNotBeNull();
            fifthPlatformDetail.ShouldNotBeNull();
            sixthPlatformDetail.ShouldNotBeNull();
            firstPlatformDetail.ShouldNotBeSameAs(secondPlatformDetail);
            thirdPlatformDetail.ShouldNotBeSameAs(firstPlatformDetail);
            fourthPlatformDetail.ShouldNotBeSameAs(firstPlatformDetail);
            fifthPlatformDetail.ShouldNotBeSameAs(firstPlatformDetail);
            sixthPlatformDetail.ShouldNotBeSameAs(firstPlatformDetail);
            sixthPlatformDetail.ShouldNotBeSameAs(fourthPlatformDetail);
        }

        #endregion

        #endregion

        #endregion
    }
}