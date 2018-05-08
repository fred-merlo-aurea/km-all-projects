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
using ECN_Framework_Entities.Activity;

namespace ECN_Framework_Entities.Activity
{
    [TestFixture]
    public class PlatformsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Platforms) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var platforms = Fixture.Create<Platforms>();
            var platformId = Fixture.Create<int>();
            var platformName = Fixture.Create<string>();

            // Act
            platforms.PlatformID = platformId;
            platforms.PlatformName = platformName;

            // Assert
            platforms.PlatformID.ShouldBe(platformId);
            platforms.PlatformName.ShouldBe(platformName);
        }

        #endregion

        #region General Getters/Setters : Class (Platforms) => Property (PlatformID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_PlatformID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var platforms = Fixture.Create<Platforms>();
            platforms.PlatformID = Fixture.Create<int>();
            var intType = platforms.PlatformID.GetType();

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

        #region General Getters/Setters : Class (Platforms) => Property (PlatformID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_Class_Invalid_Property_PlatformIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformID = "PlatformIDNotPresent";
            var platforms  = Fixture.Create<Platforms>();

            // Act , Assert
            Should.NotThrow(() => platforms.GetType().GetProperty(propertyNamePlatformID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_PlatformID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformID = "PlatformID";
            var platforms  = Fixture.Create<Platforms>();
            var propertyInfo  = platforms.GetType().GetProperty(propertyNamePlatformID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Platforms) => Property (PlatformName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_PlatformName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var platforms = Fixture.Create<Platforms>();
            platforms.PlatformName = Fixture.Create<string>();
            var stringType = platforms.PlatformName.GetType();

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

        #region General Getters/Setters : Class (Platforms) => Property (PlatformName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_Class_Invalid_Property_PlatformNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformName = "PlatformNameNotPresent";
            var platforms  = Fixture.Create<Platforms>();

            // Act , Assert
            Should.NotThrow(() => platforms.GetType().GetProperty(propertyNamePlatformName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Platforms_PlatformName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformName = "PlatformName";
            var platforms  = Fixture.Create<Platforms>();
            var propertyInfo  = platforms.GetType().GetProperty(propertyNamePlatformName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Platforms) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Platforms_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Platforms());
        }

        #endregion

        #region General Constructor : Class (Platforms) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Platforms_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfPlatforms = Fixture.CreateMany<Platforms>(2).ToList();
            var firstPlatforms = instancesOfPlatforms.FirstOrDefault();
            var lastPlatforms = instancesOfPlatforms.Last();

            // Act, Assert
            firstPlatforms.ShouldNotBeNull();
            lastPlatforms.ShouldNotBeNull();
            firstPlatforms.ShouldNotBeSameAs(lastPlatforms);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Platforms_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstPlatforms = new Platforms();
            var secondPlatforms = new Platforms();
            var thirdPlatforms = new Platforms();
            var fourthPlatforms = new Platforms();
            var fifthPlatforms = new Platforms();
            var sixthPlatforms = new Platforms();

            // Act, Assert
            firstPlatforms.ShouldNotBeNull();
            secondPlatforms.ShouldNotBeNull();
            thirdPlatforms.ShouldNotBeNull();
            fourthPlatforms.ShouldNotBeNull();
            fifthPlatforms.ShouldNotBeNull();
            sixthPlatforms.ShouldNotBeNull();
            firstPlatforms.ShouldNotBeSameAs(secondPlatforms);
            thirdPlatforms.ShouldNotBeSameAs(firstPlatforms);
            fourthPlatforms.ShouldNotBeSameAs(firstPlatforms);
            fifthPlatforms.ShouldNotBeSameAs(firstPlatforms);
            sixthPlatforms.ShouldNotBeSameAs(firstPlatforms);
            sixthPlatforms.ShouldNotBeSameAs(fourthPlatforms);
        }

        #endregion

        #endregion

        #endregion
    }
}