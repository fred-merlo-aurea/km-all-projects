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
    public class PerformanceByDayAndTimeReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var performanceByDayAndTimeReport = Fixture.Create<PerformanceByDayAndTimeReport>();
            var dayGroup = Fixture.Create<string>();
            var hourGroup = Fixture.Create<string>();
            var opens = Fixture.Create<string>();
            var clicks = Fixture.Create<string>();

            // Act
            performanceByDayAndTimeReport.DayGroup = dayGroup;
            performanceByDayAndTimeReport.HourGroup = hourGroup;
            performanceByDayAndTimeReport.Opens = opens;
            performanceByDayAndTimeReport.Clicks = clicks;

            // Assert
            performanceByDayAndTimeReport.DayGroup.ShouldBe(dayGroup);
            performanceByDayAndTimeReport.HourGroup.ShouldBe(hourGroup);
            performanceByDayAndTimeReport.Opens.ShouldBe(opens);
            performanceByDayAndTimeReport.Clicks.ShouldBe(clicks);
        }

        #endregion

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (Clicks) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Clicks_Property_String_Type_Verify_Test()
        {
            // Arrange
            var performanceByDayAndTimeReport = Fixture.Create<PerformanceByDayAndTimeReport>();
            performanceByDayAndTimeReport.Clicks = Fixture.Create<string>();
            var stringType = performanceByDayAndTimeReport.Clicks.GetType();

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

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (Clicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Class_Invalid_Property_ClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicks = "ClicksNotPresent";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();

            // Act , Assert
            Should.NotThrow(() => performanceByDayAndTimeReport.GetType().GetProperty(propertyNameClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Clicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicks = "Clicks";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();
            var propertyInfo  = performanceByDayAndTimeReport.GetType().GetProperty(propertyNameClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (DayGroup) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_DayGroup_Property_String_Type_Verify_Test()
        {
            // Arrange
            var performanceByDayAndTimeReport = Fixture.Create<PerformanceByDayAndTimeReport>();
            performanceByDayAndTimeReport.DayGroup = Fixture.Create<string>();
            var stringType = performanceByDayAndTimeReport.DayGroup.GetType();

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

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (DayGroup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Class_Invalid_Property_DayGroupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDayGroup = "DayGroupNotPresent";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();

            // Act , Assert
            Should.NotThrow(() => performanceByDayAndTimeReport.GetType().GetProperty(propertyNameDayGroup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_DayGroup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDayGroup = "DayGroup";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();
            var propertyInfo  = performanceByDayAndTimeReport.GetType().GetProperty(propertyNameDayGroup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (HourGroup) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_HourGroup_Property_String_Type_Verify_Test()
        {
            // Arrange
            var performanceByDayAndTimeReport = Fixture.Create<PerformanceByDayAndTimeReport>();
            performanceByDayAndTimeReport.HourGroup = Fixture.Create<string>();
            var stringType = performanceByDayAndTimeReport.HourGroup.GetType();

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

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (HourGroup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Class_Invalid_Property_HourGroupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHourGroup = "HourGroupNotPresent";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();

            // Act , Assert
            Should.NotThrow(() => performanceByDayAndTimeReport.GetType().GetProperty(propertyNameHourGroup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_HourGroup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHourGroup = "HourGroup";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();
            var propertyInfo  = performanceByDayAndTimeReport.GetType().GetProperty(propertyNameHourGroup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (Opens) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Opens_Property_String_Type_Verify_Test()
        {
            // Arrange
            var performanceByDayAndTimeReport = Fixture.Create<PerformanceByDayAndTimeReport>();
            performanceByDayAndTimeReport.Opens = Fixture.Create<string>();
            var stringType = performanceByDayAndTimeReport.Opens.GetType();

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

        #region General Getters/Setters : Class (PerformanceByDayAndTimeReport) => Property (Opens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Class_Invalid_Property_OpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpens = "OpensNotPresent";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();

            // Act , Assert
            Should.NotThrow(() => performanceByDayAndTimeReport.GetType().GetProperty(propertyNameOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void PerformanceByDayAndTimeReport_Opens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpens = "Opens";
            var performanceByDayAndTimeReport  = Fixture.Create<PerformanceByDayAndTimeReport>();
            var propertyInfo  = performanceByDayAndTimeReport.GetType().GetProperty(propertyNameOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (PerformanceByDayAndTimeReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PerformanceByDayAndTimeReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new PerformanceByDayAndTimeReport());
        }

        #endregion

        #region General Constructor : Class (PerformanceByDayAndTimeReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PerformanceByDayAndTimeReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfPerformanceByDayAndTimeReport = Fixture.CreateMany<PerformanceByDayAndTimeReport>(2).ToList();
            var firstPerformanceByDayAndTimeReport = instancesOfPerformanceByDayAndTimeReport.FirstOrDefault();
            var lastPerformanceByDayAndTimeReport = instancesOfPerformanceByDayAndTimeReport.Last();

            // Act, Assert
            firstPerformanceByDayAndTimeReport.ShouldNotBeNull();
            lastPerformanceByDayAndTimeReport.ShouldNotBeNull();
            firstPerformanceByDayAndTimeReport.ShouldNotBeSameAs(lastPerformanceByDayAndTimeReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_PerformanceByDayAndTimeReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstPerformanceByDayAndTimeReport = new PerformanceByDayAndTimeReport();
            var secondPerformanceByDayAndTimeReport = new PerformanceByDayAndTimeReport();
            var thirdPerformanceByDayAndTimeReport = new PerformanceByDayAndTimeReport();
            var fourthPerformanceByDayAndTimeReport = new PerformanceByDayAndTimeReport();
            var fifthPerformanceByDayAndTimeReport = new PerformanceByDayAndTimeReport();
            var sixthPerformanceByDayAndTimeReport = new PerformanceByDayAndTimeReport();

            // Act, Assert
            firstPerformanceByDayAndTimeReport.ShouldNotBeNull();
            secondPerformanceByDayAndTimeReport.ShouldNotBeNull();
            thirdPerformanceByDayAndTimeReport.ShouldNotBeNull();
            fourthPerformanceByDayAndTimeReport.ShouldNotBeNull();
            fifthPerformanceByDayAndTimeReport.ShouldNotBeNull();
            sixthPerformanceByDayAndTimeReport.ShouldNotBeNull();
            firstPerformanceByDayAndTimeReport.ShouldNotBeSameAs(secondPerformanceByDayAndTimeReport);
            thirdPerformanceByDayAndTimeReport.ShouldNotBeSameAs(firstPerformanceByDayAndTimeReport);
            fourthPerformanceByDayAndTimeReport.ShouldNotBeSameAs(firstPerformanceByDayAndTimeReport);
            fifthPerformanceByDayAndTimeReport.ShouldNotBeSameAs(firstPerformanceByDayAndTimeReport);
            sixthPerformanceByDayAndTimeReport.ShouldNotBeSameAs(firstPerformanceByDayAndTimeReport);
            sixthPerformanceByDayAndTimeReport.ShouldNotBeSameAs(fourthPerformanceByDayAndTimeReport);
        }

        #endregion

        #endregion

        #endregion
    }
}