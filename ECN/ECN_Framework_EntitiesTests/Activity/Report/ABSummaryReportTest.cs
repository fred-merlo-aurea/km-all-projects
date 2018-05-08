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
    public class ABSummaryReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ABSummaryReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            var sendTime = Fixture.Create<DateTime>();
            var blastId = Fixture.Create<int>();
            var sampleId = Fixture.Create<int>();
            var campaignItemName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var sendTotal = Fixture.Create<int>();
            var opens = Fixture.Create<int>();
            var clicks = Fixture.Create<int>();
            var bounce = Fixture.Create<int>();
            var unsubscribe = Fixture.Create<int>();
            var forward = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var campaignItemType = Fixture.Create<string>();
            var winner = Fixture.Create<bool>();
            var layoutName = Fixture.Create<string>();
            var aBWinnerType = Fixture.Create<string>();

            // Act
            aBSummaryReport.SendTime = sendTime;
            aBSummaryReport.BlastID = blastId;
            aBSummaryReport.SampleID = sampleId;
            aBSummaryReport.CampaignItemName = campaignItemName;
            aBSummaryReport.EmailSubject = emailSubject;
            aBSummaryReport.SendTotal = sendTotal;
            aBSummaryReport.Opens = opens;
            aBSummaryReport.Clicks = clicks;
            aBSummaryReport.Bounce = bounce;
            aBSummaryReport.Unsubscribe = unsubscribe;
            aBSummaryReport.Forward = forward;
            aBSummaryReport.Delivered = delivered;
            aBSummaryReport.CampaignItemType = campaignItemType;
            aBSummaryReport.Winner = winner;
            aBSummaryReport.LayoutName = layoutName;
            aBSummaryReport.ABWinnerType = aBWinnerType;

            // Assert
            aBSummaryReport.SendTime.ShouldBe(sendTime);
            aBSummaryReport.BlastID.ShouldBe(blastId);
            aBSummaryReport.SampleID.ShouldBe(sampleId);
            aBSummaryReport.CampaignItemName.ShouldBe(campaignItemName);
            aBSummaryReport.EmailSubject.ShouldBe(emailSubject);
            aBSummaryReport.SendTotal.ShouldBe(sendTotal);
            aBSummaryReport.Opens.ShouldBe(opens);
            aBSummaryReport.Clicks.ShouldBe(clicks);
            aBSummaryReport.Bounce.ShouldBe(bounce);
            aBSummaryReport.Unsubscribe.ShouldBe(unsubscribe);
            aBSummaryReport.Forward.ShouldBe(forward);
            aBSummaryReport.Delivered.ShouldBe(delivered);
            aBSummaryReport.CampaignItemType.ShouldBe(campaignItemType);
            aBSummaryReport.Winner.ShouldBe(winner);
            aBSummaryReport.LayoutName.ShouldBe(layoutName);
            aBSummaryReport.ABWinnerType.ShouldBe(aBWinnerType);
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (ABWinnerType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_ABWinnerType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.ABWinnerType = Fixture.Create<string>();
            var stringType = aBSummaryReport.ABWinnerType.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (ABWinnerType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_ABWinnerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameABWinnerType = "ABWinnerTypeNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameABWinnerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_ABWinnerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameABWinnerType = "ABWinnerType";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameABWinnerType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.BlastID = Fixture.Create<int>();
            var intType = aBSummaryReport.BlastID.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Bounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Bounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Bounce = Fixture.Create<int>();
            var intType = aBSummaryReport.Bounce.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Bounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_BounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounce = "BounceNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Bounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounce = "Bounce";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.CampaignItemName = Fixture.Create<string>();
            var stringType = aBSummaryReport.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (CampaignItemType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_CampaignItemType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.CampaignItemType = Fixture.Create<string>();
            var stringType = aBSummaryReport.CampaignItemType.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (CampaignItemType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_CampaignItemTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemType = "CampaignItemTypeNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameCampaignItemType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_CampaignItemType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemType = "CampaignItemType";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameCampaignItemType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Clicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Clicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Clicks = Fixture.Create<int>();
            var intType = aBSummaryReport.Clicks.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Clicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_ClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicks = "ClicksNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Clicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicks = "Clicks";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Delivered = Fixture.Create<int>();
            var intType = aBSummaryReport.Delivered.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.EmailSubject = Fixture.Create<string>();
            var stringType = aBSummaryReport.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Forward) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Forward_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Forward = Fixture.Create<int>();
            var intType = aBSummaryReport.Forward.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Forward) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_ForwardNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameForward = "ForwardNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameForward));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Forward_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameForward = "Forward";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameForward);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (LayoutName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_LayoutName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.LayoutName = Fixture.Create<string>();
            var stringType = aBSummaryReport.LayoutName.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (LayoutName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_LayoutNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutNameNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameLayoutName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_LayoutName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutName";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameLayoutName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Opens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Opens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Opens = Fixture.Create<int>();
            var intType = aBSummaryReport.Opens.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Opens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_OpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpens = "OpensNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Opens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpens = "Opens";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (SampleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_SampleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.SampleID = Fixture.Create<int>();
            var intType = aBSummaryReport.SampleID.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (SampleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_SampleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleIDNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameSampleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_SampleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleID";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameSampleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = aBSummaryReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(aBSummaryReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (SendTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_SendTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.SendTotal = Fixture.Create<int>();
            var intType = aBSummaryReport.SendTotal.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (SendTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_SendTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotalNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameSendTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_SendTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotal";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameSendTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Unsubscribe) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Unsubscribe_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Unsubscribe = Fixture.Create<int>();
            var intType = aBSummaryReport.Unsubscribe.GetType();

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

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Unsubscribe) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_UnsubscribeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribe = "UnsubscribeNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameUnsubscribe));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Unsubscribe_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribe = "Unsubscribe";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameUnsubscribe);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Winner) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Winner_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var aBSummaryReport = Fixture.Create<ABSummaryReport>();
            aBSummaryReport.Winner = Fixture.Create<bool>();
            var boolType = aBSummaryReport.Winner.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (ABSummaryReport) => Property (Winner) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Class_Invalid_Property_WinnerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWinner = "WinnerNotPresent";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();

            // Act , Assert
            Should.NotThrow(() => aBSummaryReport.GetType().GetProperty(propertyNameWinner));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ABSummaryReport_Winner_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWinner = "Winner";
            var aBSummaryReport  = Fixture.Create<ABSummaryReport>();
            var propertyInfo  = aBSummaryReport.GetType().GetProperty(propertyNameWinner);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ABSummaryReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ABSummaryReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ABSummaryReport());
        }

        #endregion

        #region General Constructor : Class (ABSummaryReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ABSummaryReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfABSummaryReport = Fixture.CreateMany<ABSummaryReport>(2).ToList();
            var firstABSummaryReport = instancesOfABSummaryReport.FirstOrDefault();
            var lastABSummaryReport = instancesOfABSummaryReport.Last();

            // Act, Assert
            firstABSummaryReport.ShouldNotBeNull();
            lastABSummaryReport.ShouldNotBeNull();
            firstABSummaryReport.ShouldNotBeSameAs(lastABSummaryReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ABSummaryReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstABSummaryReport = new ABSummaryReport();
            var secondABSummaryReport = new ABSummaryReport();
            var thirdABSummaryReport = new ABSummaryReport();
            var fourthABSummaryReport = new ABSummaryReport();
            var fifthABSummaryReport = new ABSummaryReport();
            var sixthABSummaryReport = new ABSummaryReport();

            // Act, Assert
            firstABSummaryReport.ShouldNotBeNull();
            secondABSummaryReport.ShouldNotBeNull();
            thirdABSummaryReport.ShouldNotBeNull();
            fourthABSummaryReport.ShouldNotBeNull();
            fifthABSummaryReport.ShouldNotBeNull();
            sixthABSummaryReport.ShouldNotBeNull();
            firstABSummaryReport.ShouldNotBeSameAs(secondABSummaryReport);
            thirdABSummaryReport.ShouldNotBeSameAs(firstABSummaryReport);
            fourthABSummaryReport.ShouldNotBeSameAs(firstABSummaryReport);
            fifthABSummaryReport.ShouldNotBeSameAs(firstABSummaryReport);
            sixthABSummaryReport.ShouldNotBeSameAs(firstABSummaryReport);
            sixthABSummaryReport.ShouldNotBeSameAs(fourthABSummaryReport);
        }

        #endregion

        #endregion

        #endregion
    }
}