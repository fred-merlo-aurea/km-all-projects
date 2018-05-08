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
    public class BlastClickSummary_SubReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            var name = Fixture.Create<string>();
            var issueDate = Fixture.Create<DateTime>();
            var totalSent = Fixture.Create<int>();
            var totalDelivered = Fixture.Create<int>();
            var open = Fixture.Create<int>();
            var totalClicks = Fixture.Create<int>();

            // Act
            blastClickSummary_SubReport.Name = name;
            blastClickSummary_SubReport.IssueDate = issueDate;
            blastClickSummary_SubReport.TotalSent = totalSent;
            blastClickSummary_SubReport.TotalDelivered = totalDelivered;
            blastClickSummary_SubReport.Open = open;
            blastClickSummary_SubReport.TotalClicks = totalClicks;

            // Assert
            blastClickSummary_SubReport.Name.ShouldBe(name);
            blastClickSummary_SubReport.IssueDate.ShouldBe(issueDate);
            blastClickSummary_SubReport.TotalSent.ShouldBe(totalSent);
            blastClickSummary_SubReport.TotalDelivered.ShouldBe(totalDelivered);
            blastClickSummary_SubReport.Open.ShouldBe(open);
            blastClickSummary_SubReport.TotalClicks.ShouldBe(totalClicks);
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (IssueDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_IssueDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDate";
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastClickSummary_SubReport.GetType().GetProperty(propertyNameIssueDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastClickSummary_SubReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (IssueDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_Invalid_Property_IssueDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDateNotPresent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary_SubReport.GetType().GetProperty(propertyNameIssueDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_IssueDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDate";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();
            var propertyInfo  = blastClickSummary_SubReport.GetType().GetProperty(propertyNameIssueDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            blastClickSummary_SubReport.Name = Fixture.Create<string>();
            var stringType = blastClickSummary_SubReport.Name.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary_SubReport.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();
            var propertyInfo  = blastClickSummary_SubReport.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (Open) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Open_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            blastClickSummary_SubReport.Open = Fixture.Create<int>();
            var intType = blastClickSummary_SubReport.Open.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (Open) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_Invalid_Property_OpenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpen = "OpenNotPresent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary_SubReport.GetType().GetProperty(propertyNameOpen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Open_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpen = "Open";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();
            var propertyInfo  = blastClickSummary_SubReport.GetType().GetProperty(propertyNameOpen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (TotalClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_TotalClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            blastClickSummary_SubReport.TotalClicks = Fixture.Create<int>();
            var intType = blastClickSummary_SubReport.TotalClicks.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (TotalClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_Invalid_Property_TotalClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicksNotPresent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary_SubReport.GetType().GetProperty(propertyNameTotalClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_TotalClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicks";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();
            var propertyInfo  = blastClickSummary_SubReport.GetType().GetProperty(propertyNameTotalClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (TotalDelivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_TotalDelivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            blastClickSummary_SubReport.TotalDelivered = Fixture.Create<int>();
            var intType = blastClickSummary_SubReport.TotalDelivered.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (TotalDelivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_Invalid_Property_TotalDeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalDelivered = "TotalDeliveredNotPresent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary_SubReport.GetType().GetProperty(propertyNameTotalDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_TotalDelivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalDelivered = "TotalDelivered";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();
            var propertyInfo  = blastClickSummary_SubReport.GetType().GetProperty(propertyNameTotalDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (TotalSent) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_TotalSent_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary_SubReport = Fixture.Create<BlastClickSummary_SubReport>();
            blastClickSummary_SubReport.TotalSent = Fixture.Create<int>();
            var intType = blastClickSummary_SubReport.TotalSent.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary_SubReport) => Property (TotalSent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_Class_Invalid_Property_TotalSentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalSent = "TotalSentNotPresent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary_SubReport.GetType().GetProperty(propertyNameTotalSent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_SubReport_TotalSent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalSent = "TotalSent";
            var blastClickSummary_SubReport  = Fixture.Create<BlastClickSummary_SubReport>();
            var propertyInfo  = blastClickSummary_SubReport.GetType().GetProperty(propertyNameTotalSent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastClickSummary_SubReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickSummary_SubReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastClickSummary_SubReport());
        }

        #endregion

        #region General Constructor : Class (BlastClickSummary_SubReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickSummary_SubReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastClickSummary_SubReport = Fixture.CreateMany<BlastClickSummary_SubReport>(2).ToList();
            var firstBlastClickSummary_SubReport = instancesOfBlastClickSummary_SubReport.FirstOrDefault();
            var lastBlastClickSummary_SubReport = instancesOfBlastClickSummary_SubReport.Last();

            // Act, Assert
            firstBlastClickSummary_SubReport.ShouldNotBeNull();
            lastBlastClickSummary_SubReport.ShouldNotBeNull();
            firstBlastClickSummary_SubReport.ShouldNotBeSameAs(lastBlastClickSummary_SubReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickSummary_SubReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastClickSummary_SubReport = new BlastClickSummary_SubReport();
            var secondBlastClickSummary_SubReport = new BlastClickSummary_SubReport();
            var thirdBlastClickSummary_SubReport = new BlastClickSummary_SubReport();
            var fourthBlastClickSummary_SubReport = new BlastClickSummary_SubReport();
            var fifthBlastClickSummary_SubReport = new BlastClickSummary_SubReport();
            var sixthBlastClickSummary_SubReport = new BlastClickSummary_SubReport();

            // Act, Assert
            firstBlastClickSummary_SubReport.ShouldNotBeNull();
            secondBlastClickSummary_SubReport.ShouldNotBeNull();
            thirdBlastClickSummary_SubReport.ShouldNotBeNull();
            fourthBlastClickSummary_SubReport.ShouldNotBeNull();
            fifthBlastClickSummary_SubReport.ShouldNotBeNull();
            sixthBlastClickSummary_SubReport.ShouldNotBeNull();
            firstBlastClickSummary_SubReport.ShouldNotBeSameAs(secondBlastClickSummary_SubReport);
            thirdBlastClickSummary_SubReport.ShouldNotBeSameAs(firstBlastClickSummary_SubReport);
            fourthBlastClickSummary_SubReport.ShouldNotBeSameAs(firstBlastClickSummary_SubReport);
            fifthBlastClickSummary_SubReport.ShouldNotBeSameAs(firstBlastClickSummary_SubReport);
            sixthBlastClickSummary_SubReport.ShouldNotBeSameAs(firstBlastClickSummary_SubReport);
            sixthBlastClickSummary_SubReport.ShouldNotBeSameAs(fourthBlastClickSummary_SubReport);
        }

        #endregion

        #endregion

        #endregion
    }
}