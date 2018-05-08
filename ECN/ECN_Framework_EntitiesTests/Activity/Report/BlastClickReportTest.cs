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
    public class BlastClickReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastClickReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastClickReport = Fixture.Create<BlastClickReport>();
            var clickCount = Fixture.Create<int>();
            var distinctClickCount = Fixture.Create<int>();
            var link = Fixture.Create<string>();

            // Act
            blastClickReport.ClickCount = clickCount;
            blastClickReport.DistinctClickCount = distinctClickCount;
            blastClickReport.Link = link;

            // Assert
            blastClickReport.ClickCount.ShouldBe(clickCount);
            blastClickReport.DistinctClickCount.ShouldBe(distinctClickCount);
            blastClickReport.Link.ShouldBe(link);
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickReport) => Property (ClickCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_ClickCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickReport = Fixture.Create<BlastClickReport>();
            blastClickReport.ClickCount = Fixture.Create<int>();
            var intType = blastClickReport.ClickCount.GetType();

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

        #region General Getters/Setters : Class (BlastClickReport) => Property (ClickCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_Class_Invalid_Property_ClickCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickCount = "ClickCountNotPresent";
            var blastClickReport  = Fixture.Create<BlastClickReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickReport.GetType().GetProperty(propertyNameClickCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_ClickCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickCount = "ClickCount";
            var blastClickReport  = Fixture.Create<BlastClickReport>();
            var propertyInfo  = blastClickReport.GetType().GetProperty(propertyNameClickCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickReport) => Property (DistinctClickCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_DistinctClickCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickReport = Fixture.Create<BlastClickReport>();
            blastClickReport.DistinctClickCount = Fixture.Create<int>();
            var intType = blastClickReport.DistinctClickCount.GetType();

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

        #region General Getters/Setters : Class (BlastClickReport) => Property (DistinctClickCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_Class_Invalid_Property_DistinctClickCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDistinctClickCount = "DistinctClickCountNotPresent";
            var blastClickReport  = Fixture.Create<BlastClickReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickReport.GetType().GetProperty(propertyNameDistinctClickCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_DistinctClickCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDistinctClickCount = "DistinctClickCount";
            var blastClickReport  = Fixture.Create<BlastClickReport>();
            var propertyInfo  = blastClickReport.GetType().GetProperty(propertyNameDistinctClickCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickReport) => Property (Link) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_Link_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickReport = Fixture.Create<BlastClickReport>();
            blastClickReport.Link = Fixture.Create<string>();
            var stringType = blastClickReport.Link.GetType();

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

        #region General Getters/Setters : Class (BlastClickReport) => Property (Link) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_Class_Invalid_Property_LinkNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLink = "LinkNotPresent";
            var blastClickReport  = Fixture.Create<BlastClickReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickReport.GetType().GetProperty(propertyNameLink));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickReport_Link_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLink = "Link";
            var blastClickReport  = Fixture.Create<BlastClickReport>();
            var propertyInfo  = blastClickReport.GetType().GetProperty(propertyNameLink);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastClickReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastClickReport());
        }

        #endregion

        #region General Constructor : Class (BlastClickReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastClickReport = Fixture.CreateMany<BlastClickReport>(2).ToList();
            var firstBlastClickReport = instancesOfBlastClickReport.FirstOrDefault();
            var lastBlastClickReport = instancesOfBlastClickReport.Last();

            // Act, Assert
            firstBlastClickReport.ShouldNotBeNull();
            lastBlastClickReport.ShouldNotBeNull();
            firstBlastClickReport.ShouldNotBeSameAs(lastBlastClickReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastClickReport = new BlastClickReport();
            var secondBlastClickReport = new BlastClickReport();
            var thirdBlastClickReport = new BlastClickReport();
            var fourthBlastClickReport = new BlastClickReport();
            var fifthBlastClickReport = new BlastClickReport();
            var sixthBlastClickReport = new BlastClickReport();

            // Act, Assert
            firstBlastClickReport.ShouldNotBeNull();
            secondBlastClickReport.ShouldNotBeNull();
            thirdBlastClickReport.ShouldNotBeNull();
            fourthBlastClickReport.ShouldNotBeNull();
            fifthBlastClickReport.ShouldNotBeNull();
            sixthBlastClickReport.ShouldNotBeNull();
            firstBlastClickReport.ShouldNotBeSameAs(secondBlastClickReport);
            thirdBlastClickReport.ShouldNotBeSameAs(firstBlastClickReport);
            fourthBlastClickReport.ShouldNotBeSameAs(firstBlastClickReport);
            fifthBlastClickReport.ShouldNotBeSameAs(firstBlastClickReport);
            sixthBlastClickReport.ShouldNotBeSameAs(firstBlastClickReport);
            sixthBlastClickReport.ShouldNotBeSameAs(fourthBlastClickReport);
        }

        #endregion

        #endregion

        #endregion
    }
}