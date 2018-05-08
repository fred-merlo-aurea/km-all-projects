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
    public class BlastClickSummaryTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastClickSummary) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            var campaignItemName = Fixture.Create<string>();
            var uRL = Fixture.Create<string>();
            var totalSent = Fixture.Create<int>();
            var totalDelivered = Fixture.Create<int>();
            var open = Fixture.Create<int>();
            var totalCampaignClicks = Fixture.Create<int>();
            var totalClicks = Fixture.Create<int>();
            var uniqueClicks = Fixture.Create<int>();
            var issueDate = Fixture.Create<string>();

            // Act
            blastClickSummary.CampaignItemName = campaignItemName;
            blastClickSummary.URL = uRL;
            blastClickSummary.TotalSent = totalSent;
            blastClickSummary.TotalDelivered = totalDelivered;
            blastClickSummary.Open = open;
            blastClickSummary.TotalCampaignClicks = totalCampaignClicks;
            blastClickSummary.TotalClicks = totalClicks;
            blastClickSummary.UniqueClicks = uniqueClicks;
            blastClickSummary.IssueDate = issueDate;

            // Assert
            blastClickSummary.CampaignItemName.ShouldBe(campaignItemName);
            blastClickSummary.URL.ShouldBe(uRL);
            blastClickSummary.TotalSent.ShouldBe(totalSent);
            blastClickSummary.TotalDelivered.ShouldBe(totalDelivered);
            blastClickSummary.Open.ShouldBe(open);
            blastClickSummary.TotalCampaignClicks.ShouldBe(totalCampaignClicks);
            blastClickSummary.TotalClicks.ShouldBe(totalClicks);
            blastClickSummary.UniqueClicks.ShouldBe(uniqueClicks);
            blastClickSummary.IssueDate.ShouldBe(issueDate);
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.CampaignItemName = Fixture.Create<string>();
            var stringType = blastClickSummary.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (IssueDate) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_IssueDate_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.IssueDate = Fixture.Create<string>();
            var stringType = blastClickSummary.IssueDate.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (IssueDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_IssueDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDateNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameIssueDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_IssueDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDate";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameIssueDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (Open) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Open_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.Open = Fixture.Create<int>();
            var intType = blastClickSummary.Open.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (Open) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_OpenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpen = "OpenNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameOpen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Open_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpen = "Open";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameOpen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalCampaignClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalCampaignClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.TotalCampaignClicks = Fixture.Create<int>();
            var intType = blastClickSummary.TotalCampaignClicks.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalCampaignClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_TotalCampaignClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalCampaignClicks = "TotalCampaignClicksNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameTotalCampaignClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalCampaignClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalCampaignClicks = "TotalCampaignClicks";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameTotalCampaignClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.TotalClicks = Fixture.Create<int>();
            var intType = blastClickSummary.TotalClicks.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_TotalClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicksNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameTotalClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicks";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameTotalClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalDelivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalDelivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.TotalDelivered = Fixture.Create<int>();
            var intType = blastClickSummary.TotalDelivered.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalDelivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_TotalDeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalDelivered = "TotalDeliveredNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameTotalDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalDelivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalDelivered = "TotalDelivered";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameTotalDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalSent) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalSent_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.TotalSent = Fixture.Create<int>();
            var intType = blastClickSummary.TotalSent.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (TotalSent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_TotalSentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalSent = "TotalSentNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameTotalSent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_TotalSent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalSent = "TotalSent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameTotalSent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (UniqueClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_UniqueClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.UniqueClicks = Fixture.Create<int>();
            var intType = blastClickSummary.UniqueClicks.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (UniqueClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_UniqueClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClicks = "UniqueClicksNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameUniqueClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_UniqueClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueClicks = "UniqueClicks";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameUniqueClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickSummary) => Property (URL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_URL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickSummary = Fixture.Create<BlastClickSummary>();
            blastClickSummary.URL = Fixture.Create<string>();
            var stringType = blastClickSummary.URL.GetType();

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

        #region General Getters/Setters : Class (BlastClickSummary) => Property (URL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_Class_Invalid_Property_URLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameURL = "URLNotPresent";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();

            // Act , Assert
            Should.NotThrow(() => blastClickSummary.GetType().GetProperty(propertyNameURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickSummary_URL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameURL = "URL";
            var blastClickSummary  = Fixture.Create<BlastClickSummary>();
            var propertyInfo  = blastClickSummary.GetType().GetProperty(propertyNameURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastClickSummary) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickSummary_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastClickSummary());
        }

        #endregion

        #region General Constructor : Class (BlastClickSummary) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickSummary_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastClickSummary = Fixture.CreateMany<BlastClickSummary>(2).ToList();
            var firstBlastClickSummary = instancesOfBlastClickSummary.FirstOrDefault();
            var lastBlastClickSummary = instancesOfBlastClickSummary.Last();

            // Act, Assert
            firstBlastClickSummary.ShouldNotBeNull();
            lastBlastClickSummary.ShouldNotBeNull();
            firstBlastClickSummary.ShouldNotBeSameAs(lastBlastClickSummary);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickSummary_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastClickSummary = new BlastClickSummary();
            var secondBlastClickSummary = new BlastClickSummary();
            var thirdBlastClickSummary = new BlastClickSummary();
            var fourthBlastClickSummary = new BlastClickSummary();
            var fifthBlastClickSummary = new BlastClickSummary();
            var sixthBlastClickSummary = new BlastClickSummary();

            // Act, Assert
            firstBlastClickSummary.ShouldNotBeNull();
            secondBlastClickSummary.ShouldNotBeNull();
            thirdBlastClickSummary.ShouldNotBeNull();
            fourthBlastClickSummary.ShouldNotBeNull();
            fifthBlastClickSummary.ShouldNotBeNull();
            sixthBlastClickSummary.ShouldNotBeNull();
            firstBlastClickSummary.ShouldNotBeSameAs(secondBlastClickSummary);
            thirdBlastClickSummary.ShouldNotBeSameAs(firstBlastClickSummary);
            fourthBlastClickSummary.ShouldNotBeSameAs(firstBlastClickSummary);
            fifthBlastClickSummary.ShouldNotBeSameAs(firstBlastClickSummary);
            sixthBlastClickSummary.ShouldNotBeSameAs(firstBlastClickSummary);
            sixthBlastClickSummary.ShouldNotBeSameAs(fourthBlastClickSummary);
        }

        #endregion

        #endregion

        #endregion
    }
}