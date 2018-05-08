using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.main.channels;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Classes
{
    [TestFixture]
    public class DateRangeTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DateRange) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var dateRange = Fixture.Create<DateRange>();
            var start = Fixture.Create<DateTime>();
            var end = Fixture.Create<DateTime>();

            // Act
            dateRange.Start = start;
            dateRange.End = end;

            // Assert
            dateRange.Start.ShouldBe(start);
            dateRange.End.ShouldBe(end);
        }

        #endregion

        #region General Getters/Setters : Class (DateRange) => Property (End) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_End_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameEnd = "End";
            var dateRange = Fixture.Create<DateRange>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dateRange.GetType().GetProperty(propertyNameEnd);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dateRange, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DateRange) => Property (End) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_Class_Invalid_Property_EndNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEnd = "EndNotPresent";
            var dateRange  = Fixture.Create<DateRange>();

            // Act , Assert
            Should.NotThrow(() => dateRange.GetType().GetProperty(propertyNameEnd));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_End_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEnd = "End";
            var dateRange  = Fixture.Create<DateRange>();
            var propertyInfo  = dateRange.GetType().GetProperty(propertyNameEnd);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DateRange) => Property (Start) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_Start_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStart = "Start";
            var dateRange = Fixture.Create<DateRange>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dateRange.GetType().GetProperty(propertyNameStart);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dateRange, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DateRange) => Property (Start) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_Class_Invalid_Property_StartNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStart = "StartNotPresent";
            var dateRange  = Fixture.Create<DateRange>();

            // Act , Assert
            Should.NotThrow(() => dateRange.GetType().GetProperty(propertyNameStart));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DateRange_Start_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStart = "Start";
            var dateRange  = Fixture.Create<DateRange>();
            var propertyInfo  = dateRange.GetType().GetProperty(propertyNameStart);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DateRange) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DateRange_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var start = Fixture.Create<DateTime>();
            var end = Fixture.Create<DateTime>();

            // Act, Assert
            Should.NotThrow(() => new DateRange(start, end));
        }

        #endregion

        #region General Constructor : Class (DateRange) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DateRange_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDateRange = Fixture.CreateMany<DateRange>(2).ToList();
            var firstDateRange = instancesOfDateRange.FirstOrDefault();
            var lastDateRange = instancesOfDateRange.Last();

            // Act, Assert
            firstDateRange.ShouldNotBeNull();
            lastDateRange.ShouldNotBeNull();
            firstDateRange.ShouldNotBeSameAs(lastDateRange);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DateRange_5_Objects_Creation_2_Paramters_Test()
        {
            // Arrange
            var start = Fixture.Create<DateTime>();
            var end = Fixture.Create<DateTime>();
            var firstDateRange = new DateRange(start, end);
            var secondDateRange = new DateRange(start, end);
            var thirdDateRange = new DateRange(start, end);
            var fourthDateRange = new DateRange(start, end);
            var fifthDateRange = new DateRange(start, end);
            var sixthDateRange = new DateRange(start, end);

            // Act, Assert
            firstDateRange.ShouldNotBeNull();
            secondDateRange.ShouldNotBeNull();
            thirdDateRange.ShouldNotBeNull();
            fourthDateRange.ShouldNotBeNull();
            fifthDateRange.ShouldNotBeNull();
            sixthDateRange.ShouldNotBeNull();
            firstDateRange.ShouldNotBeSameAs(secondDateRange);
            thirdDateRange.ShouldNotBeSameAs(firstDateRange);
            fourthDateRange.ShouldNotBeSameAs(firstDateRange);
            fifthDateRange.ShouldNotBeSameAs(firstDateRange);
            sixthDateRange.ShouldNotBeSameAs(firstDateRange);
            sixthDateRange.ShouldNotBeSameAs(fourthDateRange);
        }

        #endregion

        #endregion

        #endregion
    }
}