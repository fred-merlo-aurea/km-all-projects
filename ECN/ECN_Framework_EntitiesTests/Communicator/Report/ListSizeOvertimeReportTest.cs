using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator.Report;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Report
{
    [TestFixture]
    public class ListSizeOvertimeReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            var rangeStart = Fixture.Create<DateTime>();
            var rangeEnd = Fixture.Create<DateTime>();
            var added = Fixture.Create<int>();
            var bounced = Fixture.Create<int>();
            var unSubscribed = Fixture.Create<int>();
            var active = Fixture.Create<int>();

            // Act
            listSizeOvertimeReport.RangeStart = rangeStart;
            listSizeOvertimeReport.RangeEnd = rangeEnd;
            listSizeOvertimeReport.Added = added;
            listSizeOvertimeReport.Bounced = bounced;
            listSizeOvertimeReport.UnSubscribed = unSubscribed;
            listSizeOvertimeReport.Active = active;

            // Assert
            listSizeOvertimeReport.RangeStart.ShouldBe(rangeStart);
            listSizeOvertimeReport.RangeEnd.ShouldBe(rangeEnd);
            listSizeOvertimeReport.Added.ShouldBe(added);
            listSizeOvertimeReport.Bounced.ShouldBe(bounced);
            listSizeOvertimeReport.UnSubscribed.ShouldBe(unSubscribed);
            listSizeOvertimeReport.Active.ShouldBe(active);
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (Active) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Active_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            listSizeOvertimeReport.Active = Fixture.Create<int>();
            var intType = listSizeOvertimeReport.Active.GetType();

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

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (Active) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_Invalid_Property_ActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActive = "ActiveNotPresent";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();

            // Act , Assert
            Should.NotThrow(() => listSizeOvertimeReport.GetType().GetProperty(propertyNameActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Active_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActive = "Active";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();
            var propertyInfo  = listSizeOvertimeReport.GetType().GetProperty(propertyNameActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (Added) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Added_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            listSizeOvertimeReport.Added = Fixture.Create<int>();
            var intType = listSizeOvertimeReport.Added.GetType();

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

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (Added) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_Invalid_Property_AddedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAdded = "AddedNotPresent";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();

            // Act , Assert
            Should.NotThrow(() => listSizeOvertimeReport.GetType().GetProperty(propertyNameAdded));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Added_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAdded = "Added";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();
            var propertyInfo  = listSizeOvertimeReport.GetType().GetProperty(propertyNameAdded);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (Bounced) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Bounced_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            listSizeOvertimeReport.Bounced = Fixture.Create<int>();
            var intType = listSizeOvertimeReport.Bounced.GetType();

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

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (Bounced) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_Invalid_Property_BouncedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounced = "BouncedNotPresent";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();

            // Act , Assert
            Should.NotThrow(() => listSizeOvertimeReport.GetType().GetProperty(propertyNameBounced));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Bounced_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounced = "Bounced";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();
            var propertyInfo  = listSizeOvertimeReport.GetType().GetProperty(propertyNameBounced);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (RangeEnd) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_RangeEnd_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameRangeEnd = "RangeEnd";
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = listSizeOvertimeReport.GetType().GetProperty(propertyNameRangeEnd);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(listSizeOvertimeReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (RangeEnd) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_Invalid_Property_RangeEndNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRangeEnd = "RangeEndNotPresent";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();

            // Act , Assert
            Should.NotThrow(() => listSizeOvertimeReport.GetType().GetProperty(propertyNameRangeEnd));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_RangeEnd_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRangeEnd = "RangeEnd";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();
            var propertyInfo  = listSizeOvertimeReport.GetType().GetProperty(propertyNameRangeEnd);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (RangeStart) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_RangeStart_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameRangeStart = "RangeStart";
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = listSizeOvertimeReport.GetType().GetProperty(propertyNameRangeStart);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(listSizeOvertimeReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (RangeStart) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_Invalid_Property_RangeStartNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRangeStart = "RangeStartNotPresent";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();

            // Act , Assert
            Should.NotThrow(() => listSizeOvertimeReport.GetType().GetProperty(propertyNameRangeStart));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_RangeStart_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRangeStart = "RangeStart";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();
            var propertyInfo  = listSizeOvertimeReport.GetType().GetProperty(propertyNameRangeStart);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (UnSubscribed) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_UnSubscribed_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var listSizeOvertimeReport = Fixture.Create<ListSizeOvertimeReport>();
            listSizeOvertimeReport.UnSubscribed = Fixture.Create<int>();
            var intType = listSizeOvertimeReport.UnSubscribed.GetType();

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

        #region General Getters/Setters : Class (ListSizeOvertimeReport) => Property (UnSubscribed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_Class_Invalid_Property_UnSubscribedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnSubscribed = "UnSubscribedNotPresent";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();

            // Act , Assert
            Should.NotThrow(() => listSizeOvertimeReport.GetType().GetProperty(propertyNameUnSubscribed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ListSizeOvertimeReport_UnSubscribed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnSubscribed = "UnSubscribed";
            var listSizeOvertimeReport  = Fixture.Create<ListSizeOvertimeReport>();
            var propertyInfo  = listSizeOvertimeReport.GetType().GetProperty(propertyNameUnSubscribed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ListSizeOvertimeReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ListSizeOvertimeReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ListSizeOvertimeReport());
        }

        #endregion

        #region General Constructor : Class (ListSizeOvertimeReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ListSizeOvertimeReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfListSizeOvertimeReport = Fixture.CreateMany<ListSizeOvertimeReport>(2).ToList();
            var firstListSizeOvertimeReport = instancesOfListSizeOvertimeReport.FirstOrDefault();
            var lastListSizeOvertimeReport = instancesOfListSizeOvertimeReport.Last();

            // Act, Assert
            firstListSizeOvertimeReport.ShouldNotBeNull();
            lastListSizeOvertimeReport.ShouldNotBeNull();
            firstListSizeOvertimeReport.ShouldNotBeSameAs(lastListSizeOvertimeReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ListSizeOvertimeReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstListSizeOvertimeReport = new ListSizeOvertimeReport();
            var secondListSizeOvertimeReport = new ListSizeOvertimeReport();
            var thirdListSizeOvertimeReport = new ListSizeOvertimeReport();
            var fourthListSizeOvertimeReport = new ListSizeOvertimeReport();
            var fifthListSizeOvertimeReport = new ListSizeOvertimeReport();
            var sixthListSizeOvertimeReport = new ListSizeOvertimeReport();

            // Act, Assert
            firstListSizeOvertimeReport.ShouldNotBeNull();
            secondListSizeOvertimeReport.ShouldNotBeNull();
            thirdListSizeOvertimeReport.ShouldNotBeNull();
            fourthListSizeOvertimeReport.ShouldNotBeNull();
            fifthListSizeOvertimeReport.ShouldNotBeNull();
            sixthListSizeOvertimeReport.ShouldNotBeNull();
            firstListSizeOvertimeReport.ShouldNotBeSameAs(secondListSizeOvertimeReport);
            thirdListSizeOvertimeReport.ShouldNotBeSameAs(firstListSizeOvertimeReport);
            fourthListSizeOvertimeReport.ShouldNotBeSameAs(firstListSizeOvertimeReport);
            fifthListSizeOvertimeReport.ShouldNotBeSameAs(firstListSizeOvertimeReport);
            sixthListSizeOvertimeReport.ShouldNotBeSameAs(firstListSizeOvertimeReport);
            sixthListSizeOvertimeReport.ShouldNotBeSameAs(fourthListSizeOvertimeReport);
        }

        #endregion

        #endregion

        #endregion
    }
}