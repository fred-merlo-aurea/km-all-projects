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
    public class SocialShareChartTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (SocialShareChart) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var socialShareChart = Fixture.Create<SocialShareChart>();
            var displayName = Fixture.Create<string>();
            var share = Fixture.Create<int?>();

            // Act
            socialShareChart.DisplayName = displayName;
            socialShareChart.Share = share;

            // Assert
            socialShareChart.DisplayName.ShouldBe(displayName);
            socialShareChart.Share.ShouldBe(share);
        }

        #endregion

        #region General Getters/Setters : Class (SocialShareChart) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var socialShareChart = Fixture.Create<SocialShareChart>();
            socialShareChart.DisplayName = Fixture.Create<string>();
            var stringType = socialShareChart.DisplayName.GetType();

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

        #region General Getters/Setters : Class (SocialShareChart) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var socialShareChart  = Fixture.Create<SocialShareChart>();

            // Act , Assert
            Should.NotThrow(() => socialShareChart.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var socialShareChart  = Fixture.Create<SocialShareChart>();
            var propertyInfo  = socialShareChart.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (SocialShareChart) => Property (Share) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Share_Property_Data_Without_Null_Test()
        {
            // Arrange
            var socialShareChart = Fixture.Create<SocialShareChart>();
            var random = Fixture.Create<int>();

            // Act , Set
            socialShareChart.Share = random;

            // Assert
            socialShareChart.Share.ShouldBe(random);
            socialShareChart.Share.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Share_Property_Only_Null_Data_Test()
        {
            // Arrange
            var socialShareChart = Fixture.Create<SocialShareChart>();    

            // Act , Set
            socialShareChart.Share = null;

            // Assert
            socialShareChart.Share.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Share_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameShare = "Share";
            var socialShareChart = Fixture.Create<SocialShareChart>();
            var propertyInfo = socialShareChart.GetType().GetProperty(propertyNameShare);

            // Act , Set
            propertyInfo.SetValue(socialShareChart, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            socialShareChart.Share.ShouldBeNull();
            socialShareChart.Share.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (SocialShareChart) => Property (Share) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Class_Invalid_Property_ShareNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShare = "ShareNotPresent";
            var socialShareChart  = Fixture.Create<SocialShareChart>();

            // Act , Assert
            Should.NotThrow(() => socialShareChart.GetType().GetProperty(propertyNameShare));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void SocialShareChart_Share_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShare = "Share";
            var socialShareChart  = Fixture.Create<SocialShareChart>();
            var propertyInfo  = socialShareChart.GetType().GetProperty(propertyNameShare);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (SocialShareChart) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialShareChart_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new SocialShareChart());
        }

        #endregion

        #region General Constructor : Class (SocialShareChart) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialShareChart_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfSocialShareChart = Fixture.CreateMany<SocialShareChart>(2).ToList();
            var firstSocialShareChart = instancesOfSocialShareChart.FirstOrDefault();
            var lastSocialShareChart = instancesOfSocialShareChart.Last();

            // Act, Assert
            firstSocialShareChart.ShouldNotBeNull();
            lastSocialShareChart.ShouldNotBeNull();
            firstSocialShareChart.ShouldNotBeSameAs(lastSocialShareChart);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialShareChart_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstSocialShareChart = new SocialShareChart();
            var secondSocialShareChart = new SocialShareChart();
            var thirdSocialShareChart = new SocialShareChart();
            var fourthSocialShareChart = new SocialShareChart();
            var fifthSocialShareChart = new SocialShareChart();
            var sixthSocialShareChart = new SocialShareChart();

            // Act, Assert
            firstSocialShareChart.ShouldNotBeNull();
            secondSocialShareChart.ShouldNotBeNull();
            thirdSocialShareChart.ShouldNotBeNull();
            fourthSocialShareChart.ShouldNotBeNull();
            fifthSocialShareChart.ShouldNotBeNull();
            sixthSocialShareChart.ShouldNotBeNull();
            firstSocialShareChart.ShouldNotBeSameAs(secondSocialShareChart);
            thirdSocialShareChart.ShouldNotBeSameAs(firstSocialShareChart);
            fourthSocialShareChart.ShouldNotBeSameAs(firstSocialShareChart);
            fifthSocialShareChart.ShouldNotBeSameAs(firstSocialShareChart);
            sixthSocialShareChart.ShouldNotBeSameAs(firstSocialShareChart);
            sixthSocialShareChart.ShouldNotBeSameAs(fourthSocialShareChart);
        }

        #endregion

        #region General Constructor : Class (SocialShareChart) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_SocialShareChart_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var displayName = string.Empty;

            // Act
            var socialShareChart = new SocialShareChart();

            // Assert
            socialShareChart.Share.ShouldBeNull();
            socialShareChart.DisplayName.ShouldBe(displayName);
        }

        #endregion

        #endregion

        #endregion
    }
}