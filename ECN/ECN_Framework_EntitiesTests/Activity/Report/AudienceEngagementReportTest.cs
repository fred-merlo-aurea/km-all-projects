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
    public class AudienceEngagementReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (AudienceEngagementReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            var sortOrder = Fixture.Create<int>();
            var subscriberType = Fixture.Create<string>();
            var counts = Fixture.Create<int>();
            var percents = Fixture.Create<decimal>();
            var percentage = Fixture.Create<decimal>();
            var description = Fixture.Create<string>();

            // Act
            audienceEngagementReport.SortOrder = sortOrder;
            audienceEngagementReport.SubscriberType = subscriberType;
            audienceEngagementReport.Counts = counts;
            audienceEngagementReport.Percents = percents;
            audienceEngagementReport.Percentage = percentage;
            audienceEngagementReport.Description = description;

            // Assert
            audienceEngagementReport.SortOrder.ShouldBe(sortOrder);
            audienceEngagementReport.SubscriberType.ShouldBe(subscriberType);
            audienceEngagementReport.Counts.ShouldBe(counts);
            audienceEngagementReport.Percents.ShouldNotBeNull();
            audienceEngagementReport.Percentage.ShouldNotBeNull();
            audienceEngagementReport.Description.ShouldBe(description);
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Counts) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Counts_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            audienceEngagementReport.Counts = Fixture.Create<int>();
            var intType = audienceEngagementReport.Counts.GetType();

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

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Counts) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_Invalid_Property_CountsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCounts = "CountsNotPresent";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();

            // Act , Assert
            Should.NotThrow(() => audienceEngagementReport.GetType().GetProperty(propertyNameCounts));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Counts_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCounts = "Counts";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();
            var propertyInfo  = audienceEngagementReport.GetType().GetProperty(propertyNameCounts);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Description) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Description_Property_String_Type_Verify_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            audienceEngagementReport.Description = Fixture.Create<string>();
            var stringType = audienceEngagementReport.Description.GetType();

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

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Description) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_Invalid_Property_DescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDescription = "DescriptionNotPresent";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();

            // Act , Assert
            Should.NotThrow(() => audienceEngagementReport.GetType().GetProperty(propertyNameDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Description_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDescription = "Description";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();
            var propertyInfo  = audienceEngagementReport.GetType().GetProperty(propertyNameDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Percentage) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Percentage_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            audienceEngagementReport.Percentage = Fixture.Create<decimal>();
            var decimalType = audienceEngagementReport.Percentage.GetType();

            // Act
            var isTypeDecimal = typeof(decimal) == (decimalType);
            var isTypeNullableDecimal = typeof(decimal?) == (decimalType);
            var isTypeString = typeof(string) == (decimalType);
            var isTypeInt = typeof(int) == (decimalType);
            var isTypeLong = typeof(long) == (decimalType);
            var isTypeBool = typeof(bool) == (decimalType);
            var isTypeDouble = typeof(double) == (decimalType);
            var isTypeFloat = typeof(float) == (decimalType);
            var isTypeIntNullable = typeof(int?) == (decimalType);
            var isTypeLongNullable = typeof(long?) == (decimalType);
            var isTypeBoolNullable = typeof(bool?) == (decimalType);
            var isTypeDoubleNullable = typeof(double?) == (decimalType);
            var isTypeFloatNullable = typeof(float?) == (decimalType);

            // Assert
            isTypeDecimal.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDecimal.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Percentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_Invalid_Property_PercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePercentage = "PercentageNotPresent";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();

            // Act , Assert
            Should.NotThrow(() => audienceEngagementReport.GetType().GetProperty(propertyNamePercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Percentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePercentage = "Percentage";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();
            var propertyInfo  = audienceEngagementReport.GetType().GetProperty(propertyNamePercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Percents) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Percents_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            audienceEngagementReport.Percents = Fixture.Create<decimal>();
            var decimalType = audienceEngagementReport.Percents.GetType();

            // Act
            var isTypeDecimal = typeof(decimal) == (decimalType);
            var isTypeNullableDecimal = typeof(decimal?) == (decimalType);
            var isTypeString = typeof(string) == (decimalType);
            var isTypeInt = typeof(int) == (decimalType);
            var isTypeLong = typeof(long) == (decimalType);
            var isTypeBool = typeof(bool) == (decimalType);
            var isTypeDouble = typeof(double) == (decimalType);
            var isTypeFloat = typeof(float) == (decimalType);
            var isTypeIntNullable = typeof(int?) == (decimalType);
            var isTypeLongNullable = typeof(long?) == (decimalType);
            var isTypeBoolNullable = typeof(bool?) == (decimalType);
            var isTypeDoubleNullable = typeof(double?) == (decimalType);
            var isTypeFloatNullable = typeof(float?) == (decimalType);

            // Assert
            isTypeDecimal.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDecimal.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (Percents) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_Invalid_Property_PercentsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePercents = "PercentsNotPresent";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();

            // Act , Assert
            Should.NotThrow(() => audienceEngagementReport.GetType().GetProperty(propertyNamePercents));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Percents_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePercents = "Percents";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();
            var propertyInfo  = audienceEngagementReport.GetType().GetProperty(propertyNamePercents);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (SortOrder) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_SortOrder_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            audienceEngagementReport.SortOrder = Fixture.Create<int>();
            var intType = audienceEngagementReport.SortOrder.GetType();

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

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (SortOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_Invalid_Property_SortOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrderNotPresent";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();

            // Act , Assert
            Should.NotThrow(() => audienceEngagementReport.GetType().GetProperty(propertyNameSortOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_SortOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSortOrder = "SortOrder";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();
            var propertyInfo  = audienceEngagementReport.GetType().GetProperty(propertyNameSortOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (SubscriberType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_SubscriberType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var audienceEngagementReport = Fixture.Create<AudienceEngagementReport>();
            audienceEngagementReport.SubscriberType = Fixture.Create<string>();
            var stringType = audienceEngagementReport.SubscriberType.GetType();

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

        #region General Getters/Setters : Class (AudienceEngagementReport) => Property (SubscriberType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_Class_Invalid_Property_SubscriberTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscriberType = "SubscriberTypeNotPresent";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();

            // Act , Assert
            Should.NotThrow(() => audienceEngagementReport.GetType().GetProperty(propertyNameSubscriberType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AudienceEngagementReport_SubscriberType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscriberType = "SubscriberType";
            var audienceEngagementReport  = Fixture.Create<AudienceEngagementReport>();
            var propertyInfo  = audienceEngagementReport.GetType().GetProperty(propertyNameSubscriberType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (AudienceEngagementReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_AudienceEngagementReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new AudienceEngagementReport());
        }

        #endregion

        #region General Constructor : Class (AudienceEngagementReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_AudienceEngagementReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfAudienceEngagementReport = Fixture.CreateMany<AudienceEngagementReport>(2).ToList();
            var firstAudienceEngagementReport = instancesOfAudienceEngagementReport.FirstOrDefault();
            var lastAudienceEngagementReport = instancesOfAudienceEngagementReport.Last();

            // Act, Assert
            firstAudienceEngagementReport.ShouldNotBeNull();
            lastAudienceEngagementReport.ShouldNotBeNull();
            firstAudienceEngagementReport.ShouldNotBeSameAs(lastAudienceEngagementReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_AudienceEngagementReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstAudienceEngagementReport = new AudienceEngagementReport();
            var secondAudienceEngagementReport = new AudienceEngagementReport();
            var thirdAudienceEngagementReport = new AudienceEngagementReport();
            var fourthAudienceEngagementReport = new AudienceEngagementReport();
            var fifthAudienceEngagementReport = new AudienceEngagementReport();
            var sixthAudienceEngagementReport = new AudienceEngagementReport();

            // Act, Assert
            firstAudienceEngagementReport.ShouldNotBeNull();
            secondAudienceEngagementReport.ShouldNotBeNull();
            thirdAudienceEngagementReport.ShouldNotBeNull();
            fourthAudienceEngagementReport.ShouldNotBeNull();
            fifthAudienceEngagementReport.ShouldNotBeNull();
            sixthAudienceEngagementReport.ShouldNotBeNull();
            firstAudienceEngagementReport.ShouldNotBeSameAs(secondAudienceEngagementReport);
            thirdAudienceEngagementReport.ShouldNotBeSameAs(firstAudienceEngagementReport);
            fourthAudienceEngagementReport.ShouldNotBeSameAs(firstAudienceEngagementReport);
            fifthAudienceEngagementReport.ShouldNotBeSameAs(firstAudienceEngagementReport);
            sixthAudienceEngagementReport.ShouldNotBeSameAs(firstAudienceEngagementReport);
            sixthAudienceEngagementReport.ShouldNotBeSameAs(fourthAudienceEngagementReport);
        }

        #endregion

        #endregion

        #endregion
    }
}