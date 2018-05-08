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
    public class ChampionAuditReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ChampionAuditReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            var sendTime = Fixture.Create<DateTime>();
            var sampleId = Fixture.Create<int>();
            var campaignItemName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var sendTotal = Fixture.Create<int>();
            var opens = Fixture.Create<int>();
            var clicks = Fixture.Create<int>();
            var bounce = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var campaignItemType = Fixture.Create<string>();
            var winner = Fixture.Create<bool>();
            var layoutName = Fixture.Create<string>();

            // Act
            championAuditReport.SendTime = sendTime;
            championAuditReport.SampleID = sampleId;
            championAuditReport.CampaignItemName = campaignItemName;
            championAuditReport.EmailSubject = emailSubject;
            championAuditReport.SendTotal = sendTotal;
            championAuditReport.Opens = opens;
            championAuditReport.Clicks = clicks;
            championAuditReport.Bounce = bounce;
            championAuditReport.Delivered = delivered;
            championAuditReport.CampaignItemType = campaignItemType;
            championAuditReport.Winner = winner;
            championAuditReport.LayoutName = layoutName;

            // Assert
            championAuditReport.SendTime.ShouldBe(sendTime);
            championAuditReport.SampleID.ShouldBe(sampleId);
            championAuditReport.CampaignItemName.ShouldBe(campaignItemName);
            championAuditReport.EmailSubject.ShouldBe(emailSubject);
            championAuditReport.SendTotal.ShouldBe(sendTotal);
            championAuditReport.Opens.ShouldBe(opens);
            championAuditReport.Clicks.ShouldBe(clicks);
            championAuditReport.Bounce.ShouldBe(bounce);
            championAuditReport.Delivered.ShouldBe(delivered);
            championAuditReport.CampaignItemType.ShouldBe(campaignItemType);
            championAuditReport.Winner.ShouldBe(winner);
            championAuditReport.LayoutName.ShouldBe(layoutName);
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Bounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Bounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.Bounce = Fixture.Create<int>();
            var intType = championAuditReport.Bounce.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Bounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_BounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounce = "BounceNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Bounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounce = "Bounce";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.CampaignItemName = Fixture.Create<string>();
            var stringType = championAuditReport.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (CampaignItemType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_CampaignItemType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.CampaignItemType = Fixture.Create<string>();
            var stringType = championAuditReport.CampaignItemType.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (CampaignItemType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_CampaignItemTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemType = "CampaignItemTypeNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameCampaignItemType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_CampaignItemType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemType = "CampaignItemType";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameCampaignItemType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Clicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Clicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.Clicks = Fixture.Create<int>();
            var intType = championAuditReport.Clicks.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Clicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_ClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClicks = "ClicksNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Clicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClicks = "Clicks";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.Delivered = Fixture.Create<int>();
            var intType = championAuditReport.Delivered.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.EmailSubject = Fixture.Create<string>();
            var stringType = championAuditReport.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (LayoutName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_LayoutName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.LayoutName = Fixture.Create<string>();
            var stringType = championAuditReport.LayoutName.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (LayoutName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_LayoutNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutNameNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameLayoutName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_LayoutName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutName";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameLayoutName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Opens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Opens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.Opens = Fixture.Create<int>();
            var intType = championAuditReport.Opens.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Opens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_OpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpens = "OpensNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Opens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpens = "Opens";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (SampleID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_SampleID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.SampleID = Fixture.Create<int>();
            var intType = championAuditReport.SampleID.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (SampleID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_SampleIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleIDNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameSampleID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_SampleID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSampleID = "SampleID";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameSampleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = championAuditReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(championAuditReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (SendTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_SendTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.SendTotal = Fixture.Create<int>();
            var intType = championAuditReport.SendTotal.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (SendTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_SendTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotalNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameSendTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_SendTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotal";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameSendTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Winner) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Winner_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var championAuditReport = Fixture.Create<ChampionAuditReport>();
            championAuditReport.Winner = Fixture.Create<bool>();
            var boolType = championAuditReport.Winner.GetType();

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

        #region General Getters/Setters : Class (ChampionAuditReport) => Property (Winner) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Class_Invalid_Property_WinnerNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWinner = "WinnerNotPresent";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();

            // Act , Assert
            Should.NotThrow(() => championAuditReport.GetType().GetProperty(propertyNameWinner));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ChampionAuditReport_Winner_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWinner = "Winner";
            var championAuditReport  = Fixture.Create<ChampionAuditReport>();
            var propertyInfo  = championAuditReport.GetType().GetProperty(propertyNameWinner);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ChampionAuditReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAuditReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new ChampionAuditReport());
        }

        #endregion

        #region General Constructor : Class (ChampionAuditReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAuditReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfChampionAuditReport = Fixture.CreateMany<ChampionAuditReport>(2).ToList();
            var firstChampionAuditReport = instancesOfChampionAuditReport.FirstOrDefault();
            var lastChampionAuditReport = instancesOfChampionAuditReport.Last();

            // Act, Assert
            firstChampionAuditReport.ShouldNotBeNull();
            lastChampionAuditReport.ShouldNotBeNull();
            firstChampionAuditReport.ShouldNotBeSameAs(lastChampionAuditReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ChampionAuditReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstChampionAuditReport = new ChampionAuditReport();
            var secondChampionAuditReport = new ChampionAuditReport();
            var thirdChampionAuditReport = new ChampionAuditReport();
            var fourthChampionAuditReport = new ChampionAuditReport();
            var fifthChampionAuditReport = new ChampionAuditReport();
            var sixthChampionAuditReport = new ChampionAuditReport();

            // Act, Assert
            firstChampionAuditReport.ShouldNotBeNull();
            secondChampionAuditReport.ShouldNotBeNull();
            thirdChampionAuditReport.ShouldNotBeNull();
            fourthChampionAuditReport.ShouldNotBeNull();
            fifthChampionAuditReport.ShouldNotBeNull();
            sixthChampionAuditReport.ShouldNotBeNull();
            firstChampionAuditReport.ShouldNotBeSameAs(secondChampionAuditReport);
            thirdChampionAuditReport.ShouldNotBeSameAs(firstChampionAuditReport);
            fourthChampionAuditReport.ShouldNotBeSameAs(firstChampionAuditReport);
            fifthChampionAuditReport.ShouldNotBeSameAs(firstChampionAuditReport);
            sixthChampionAuditReport.ShouldNotBeSameAs(firstChampionAuditReport);
            sixthChampionAuditReport.ShouldNotBeSameAs(fourthChampionAuditReport);
        }

        #endregion

        #endregion

        #endregion
    }
}