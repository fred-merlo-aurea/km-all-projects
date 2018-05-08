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
    public class CampaignStatisticsReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CampaignStatisticsReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var sendTime = Fixture.Create<DateTime>();
            var campaignItemName = Fixture.Create<string>();
            var messageName = Fixture.Create<string>();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var filterName = Fixture.Create<string>();
            var sendTotal = Fixture.Create<int>();
            var bounceTotal = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var successPercentage = Fixture.Create<Decimal>();
            var totalOpens = Fixture.Create<int>();
            var openDeliPercentage = Fixture.Create<Decimal>();
            var uniqueOpens = Fixture.Create<int>();
            var uOpenDeliPercentage = Fixture.Create<Decimal>();
            var unopened = Fixture.Create<int>();
            var uUnopenedDeliPercentage = Fixture.Create<Decimal>();
            var totalClicks = Fixture.Create<int>();
            var clickDeliPercentage = Fixture.Create<Decimal>();
            var uniqueClicks = Fixture.Create<int>();
            var uClickDeliPercentage = Fixture.Create<Decimal>();
            var noClicks = Fixture.Create<int>();
            var uNoClickOpenPercentage = Fixture.Create<Decimal>();
            var clickThrough = Fixture.Create<int>();
            var clickThroughPercentage = Fixture.Create<Decimal>();
            var unsubscribeTotal = Fixture.Create<int>();
            var masterSuppressed = Fixture.Create<int>();
            var totalAbuseComplaints = Fixture.Create<int>();
            var totalISPFeedbackLoops = Fixture.Create<int>();
            var tClicksOpenPercentage = Fixture.Create<Decimal>();
            var uClicksOpenPercentage = Fixture.Create<Decimal>();
            var suppressedTotal = Fixture.Create<int>();

            // Act
            campaignStatisticsReport.SendTime = sendTime;
            campaignStatisticsReport.CampaignItemName = campaignItemName;
            campaignStatisticsReport.MessageName = messageName;
            campaignStatisticsReport.BlastID = blastId;
            campaignStatisticsReport.EmailSubject = emailSubject;
            campaignStatisticsReport.GroupName = groupName;
            campaignStatisticsReport.FilterName = filterName;
            campaignStatisticsReport.SendTotal = sendTotal;
            campaignStatisticsReport.BounceTotal = bounceTotal;
            campaignStatisticsReport.Delivered = delivered;
            campaignStatisticsReport.SuccessPercentage = successPercentage;
            campaignStatisticsReport.TotalOpens = totalOpens;
            campaignStatisticsReport.OpenDeliPercentage = openDeliPercentage;
            campaignStatisticsReport.UniqueOpens = uniqueOpens;
            campaignStatisticsReport.UOpenDeliPercentage = uOpenDeliPercentage;
            campaignStatisticsReport.Unopened = unopened;
            campaignStatisticsReport.UUnopenedDeliPercentage = uUnopenedDeliPercentage;
            campaignStatisticsReport.TotalClicks = totalClicks;
            campaignStatisticsReport.ClickDeliPercentage = clickDeliPercentage;
            campaignStatisticsReport.UniqueClicks = uniqueClicks;
            campaignStatisticsReport.UClickDeliPercentage = uClickDeliPercentage;
            campaignStatisticsReport.NoClicks = noClicks;
            campaignStatisticsReport.UNoClickOpenPercentage = uNoClickOpenPercentage;
            campaignStatisticsReport.ClickThrough = clickThrough;
            campaignStatisticsReport.ClickThroughPercentage = clickThroughPercentage;
            campaignStatisticsReport.UnsubscribeTotal = unsubscribeTotal;
            campaignStatisticsReport.MasterSuppressed = masterSuppressed;
            campaignStatisticsReport.TotalAbuseComplaints = totalAbuseComplaints;
            campaignStatisticsReport.TotalISPFeedbackLoops = totalISPFeedbackLoops;
            campaignStatisticsReport.TClicksOpenPercentage = tClicksOpenPercentage;
            campaignStatisticsReport.UClicksOpenPercentage = uClicksOpenPercentage;
            campaignStatisticsReport.SuppressedTotal = suppressedTotal;

            // Assert
            campaignStatisticsReport.SendTime.ShouldBe(sendTime);
            campaignStatisticsReport.CampaignItemName.ShouldBe(campaignItemName);
            campaignStatisticsReport.MessageName.ShouldBe(messageName);
            campaignStatisticsReport.BlastID.ShouldBe(blastId);
            campaignStatisticsReport.EmailSubject.ShouldBe(emailSubject);
            campaignStatisticsReport.GroupName.ShouldBe(groupName);
            campaignStatisticsReport.FilterName.ShouldBe(filterName);
            campaignStatisticsReport.SendTotal.ShouldBe(sendTotal);
            campaignStatisticsReport.BounceTotal.ShouldBe(bounceTotal);
            campaignStatisticsReport.Delivered.ShouldBe(delivered);
            campaignStatisticsReport.SuccessPercentage.ShouldBe(successPercentage);
            campaignStatisticsReport.TotalOpens.ShouldBe(totalOpens);
            campaignStatisticsReport.OpenDeliPercentage.ShouldBe(openDeliPercentage);
            campaignStatisticsReport.UniqueOpens.ShouldBe(uniqueOpens);
            campaignStatisticsReport.UOpenDeliPercentage.ShouldBe(uOpenDeliPercentage);
            campaignStatisticsReport.Unopened.ShouldBe(unopened);
            campaignStatisticsReport.UUnopenedDeliPercentage.ShouldBe(uUnopenedDeliPercentage);
            campaignStatisticsReport.TotalClicks.ShouldBe(totalClicks);
            campaignStatisticsReport.ClickDeliPercentage.ShouldBe(clickDeliPercentage);
            campaignStatisticsReport.UniqueClicks.ShouldBe(uniqueClicks);
            campaignStatisticsReport.UClickDeliPercentage.ShouldBe(uClickDeliPercentage);
            campaignStatisticsReport.NoClicks.ShouldBe(noClicks);
            campaignStatisticsReport.UNoClickOpenPercentage.ShouldBe(uNoClickOpenPercentage);
            campaignStatisticsReport.ClickThrough.ShouldBe(clickThrough);
            campaignStatisticsReport.ClickThroughPercentage.ShouldBe(clickThroughPercentage);
            campaignStatisticsReport.UnsubscribeTotal.ShouldBe(unsubscribeTotal);
            campaignStatisticsReport.MasterSuppressed.ShouldBe(masterSuppressed);
            campaignStatisticsReport.TotalAbuseComplaints.ShouldBe(totalAbuseComplaints);
            campaignStatisticsReport.TotalISPFeedbackLoops.ShouldBe(totalISPFeedbackLoops);
            campaignStatisticsReport.TClicksOpenPercentage.ShouldBe(tClicksOpenPercentage);
            campaignStatisticsReport.UClicksOpenPercentage.ShouldBe(uClicksOpenPercentage);
            campaignStatisticsReport.SuppressedTotal.ShouldBe(suppressedTotal);
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.BlastID = Fixture.Create<int>();
            var intType = campaignStatisticsReport.BlastID.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (BounceTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_BounceTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.BounceTotal = Fixture.Create<int>();
            var intType = campaignStatisticsReport.BounceTotal.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (BounceTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_BounceTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceTotal = "BounceTotalNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameBounceTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_BounceTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceTotal = "BounceTotal";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameBounceTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.CampaignItemName = Fixture.Create<string>();
            var stringType = campaignStatisticsReport.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (ClickDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_ClickDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickDeliPercentage = "ClickDeliPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameClickDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (ClickDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_ClickDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickDeliPercentage = "ClickDeliPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameClickDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_ClickDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickDeliPercentage = "ClickDeliPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameClickDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (ClickThrough) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_ClickThrough_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.ClickThrough = Fixture.Create<int>();
            var intType = campaignStatisticsReport.ClickThrough.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (ClickThrough) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_ClickThroughNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThroughNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameClickThrough));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_ClickThrough_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThrough";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameClickThrough);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (ClickThroughPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_ClickThroughPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (ClickThroughPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_ClickThroughPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameClickThroughPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_ClickThroughPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.Delivered = Fixture.Create<int>();
            var intType = campaignStatisticsReport.Delivered.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.EmailSubject = Fixture.Create<string>();
            var stringType = campaignStatisticsReport.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (FilterName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_FilterName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.FilterName = Fixture.Create<string>();
            var stringType = campaignStatisticsReport.FilterName.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (FilterName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_FilterNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterNameNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameFilterName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_FilterName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterName";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameFilterName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.GroupName = Fixture.Create<string>();
            var stringType = campaignStatisticsReport.GroupName.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (MasterSuppressed) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_MasterSuppressed_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.MasterSuppressed = Fixture.Create<int>();
            var intType = campaignStatisticsReport.MasterSuppressed.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (MasterSuppressed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_MasterSuppressedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMasterSuppressed = "MasterSuppressedNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameMasterSuppressed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_MasterSuppressed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMasterSuppressed = "MasterSuppressed";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameMasterSuppressed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (MessageName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_MessageName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.MessageName = Fixture.Create<string>();
            var stringType = campaignStatisticsReport.MessageName.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (MessageName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_MessageNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessageName = "MessageNameNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameMessageName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_MessageName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessageName = "MessageName";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameMessageName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (NoClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_NoClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.NoClicks = Fixture.Create<int>();
            var intType = campaignStatisticsReport.NoClicks.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (NoClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_NoClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNoClicks = "NoClicksNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameNoClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_NoClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNoClicks = "NoClicks";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameNoClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (OpenDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_OpenDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenDeliPercentage = "OpenDeliPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameOpenDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (OpenDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_OpenDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenDeliPercentage = "OpenDeliPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameOpenDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_OpenDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpenDeliPercentage = "OpenDeliPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameOpenDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SendTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SendTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.SendTotal = Fixture.Create<int>();
            var intType = campaignStatisticsReport.SendTotal.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SendTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_SendTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotalNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameSendTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SendTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotal";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameSendTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SuccessPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SuccessPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSuccessPercentage = "SuccessPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameSuccessPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SuccessPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_SuccessPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuccessPercentage = "SuccessPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameSuccessPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SuccessPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuccessPercentage = "SuccessPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameSuccessPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SuppressedTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SuppressedTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.SuppressedTotal = Fixture.Create<int>();
            var intType = campaignStatisticsReport.SuppressedTotal.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (SuppressedTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_SuppressedTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedTotal = "SuppressedTotalNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameSuppressedTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_SuppressedTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedTotal = "SuppressedTotal";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameSuppressedTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TClicksOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TClicksOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameTClicksOpenPercentage = "TClicksOpenPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameTClicksOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TClicksOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_TClicksOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTClicksOpenPercentage = "TClicksOpenPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameTClicksOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TClicksOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTClicksOpenPercentage = "TClicksOpenPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameTClicksOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalAbuseComplaints) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalAbuseComplaints_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.TotalAbuseComplaints = Fixture.Create<int>();
            var intType = campaignStatisticsReport.TotalAbuseComplaints.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalAbuseComplaints) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_TotalAbuseComplaintsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalAbuseComplaints = "TotalAbuseComplaintsNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameTotalAbuseComplaints));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalAbuseComplaints_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalAbuseComplaints = "TotalAbuseComplaints";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameTotalAbuseComplaints);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.TotalClicks = Fixture.Create<int>();
            var intType = campaignStatisticsReport.TotalClicks.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_TotalClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicksNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameTotalClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicks";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameTotalClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalISPFeedbackLoops) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalISPFeedbackLoops_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.TotalISPFeedbackLoops = Fixture.Create<int>();
            var intType = campaignStatisticsReport.TotalISPFeedbackLoops.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalISPFeedbackLoops) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_TotalISPFeedbackLoopsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalISPFeedbackLoops = "TotalISPFeedbackLoopsNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameTotalISPFeedbackLoops));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalISPFeedbackLoops_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalISPFeedbackLoops = "TotalISPFeedbackLoops";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameTotalISPFeedbackLoops);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalOpens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalOpens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.TotalOpens = Fixture.Create<int>();
            var intType = campaignStatisticsReport.TotalOpens.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (TotalOpens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_TotalOpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalOpens = "TotalOpensNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameTotalOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_TotalOpens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalOpens = "TotalOpens";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameTotalOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UClickDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UClickDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUClickDeliPercentage = "UClickDeliPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameUClickDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UClickDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UClickDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUClickDeliPercentage = "UClickDeliPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUClickDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UClickDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUClickDeliPercentage = "UClickDeliPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUClickDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UClicksOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UClicksOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUClicksOpenPercentage = "UClicksOpenPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameUClicksOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UClicksOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UClicksOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUClicksOpenPercentage = "UClicksOpenPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUClicksOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UClicksOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUClicksOpenPercentage = "UClicksOpenPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUClicksOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UniqueClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UniqueClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.UniqueClicks = Fixture.Create<int>();
            var intType = campaignStatisticsReport.UniqueClicks.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UniqueClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UniqueClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClicks = "UniqueClicksNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUniqueClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UniqueClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueClicks = "UniqueClicks";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUniqueClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UniqueOpens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UniqueOpens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.UniqueOpens = Fixture.Create<int>();
            var intType = campaignStatisticsReport.UniqueOpens.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UniqueOpens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UniqueOpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpens = "UniqueOpensNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUniqueOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UniqueOpens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueOpens = "UniqueOpens";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUniqueOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UNoClickOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UNoClickOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUNoClickOpenPercentage = "UNoClickOpenPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameUNoClickOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UNoClickOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UNoClickOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUNoClickOpenPercentage = "UNoClickOpenPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUNoClickOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UNoClickOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUNoClickOpenPercentage = "UNoClickOpenPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUNoClickOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (Unopened) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Unopened_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.Unopened = Fixture.Create<int>();
            var intType = campaignStatisticsReport.Unopened.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (Unopened) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UnopenedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnopened = "UnopenedNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUnopened));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Unopened_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnopened = "Unopened";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUnopened);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UnsubscribeTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UnsubscribeTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            campaignStatisticsReport.UnsubscribeTotal = Fixture.Create<int>();
            var intType = campaignStatisticsReport.UnsubscribeTotal.GetType();

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

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UnsubscribeTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UnsubscribeTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTotal = "UnsubscribeTotalNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUnsubscribeTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UnsubscribeTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTotal = "UnsubscribeTotal";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUnsubscribeTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UOpenDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UOpenDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUOpenDeliPercentage = "UOpenDeliPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameUOpenDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UOpenDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UOpenDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUOpenDeliPercentage = "UOpenDeliPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUOpenDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UOpenDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUOpenDeliPercentage = "UOpenDeliPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUOpenDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UUnopenedDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UUnopenedDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUUnopenedDeliPercentage = "UUnopenedDeliPercentage";
            var campaignStatisticsReport = Fixture.Create<CampaignStatisticsReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = campaignStatisticsReport.GetType().GetProperty(propertyNameUUnopenedDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(campaignStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CampaignStatisticsReport) => Property (UUnopenedDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_Class_Invalid_Property_UUnopenedDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUUnopenedDeliPercentage = "UUnopenedDeliPercentageNotPresent";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();

            // Act , Assert
            Should.NotThrow(() => campaignStatisticsReport.GetType().GetProperty(propertyNameUUnopenedDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CampaignStatisticsReport_UUnopenedDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUUnopenedDeliPercentage = "UUnopenedDeliPercentage";
            var campaignStatisticsReport  = Fixture.Create<CampaignStatisticsReport>();
            var propertyInfo  = campaignStatisticsReport.GetType().GetProperty(propertyNameUUnopenedDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CampaignStatisticsReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignStatisticsReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CampaignStatisticsReport());
        }

        #endregion

        #region General Constructor : Class (CampaignStatisticsReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignStatisticsReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCampaignStatisticsReport = Fixture.CreateMany<CampaignStatisticsReport>(2).ToList();
            var firstCampaignStatisticsReport = instancesOfCampaignStatisticsReport.FirstOrDefault();
            var lastCampaignStatisticsReport = instancesOfCampaignStatisticsReport.Last();

            // Act, Assert
            firstCampaignStatisticsReport.ShouldNotBeNull();
            lastCampaignStatisticsReport.ShouldNotBeNull();
            firstCampaignStatisticsReport.ShouldNotBeSameAs(lastCampaignStatisticsReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CampaignStatisticsReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCampaignStatisticsReport = new CampaignStatisticsReport();
            var secondCampaignStatisticsReport = new CampaignStatisticsReport();
            var thirdCampaignStatisticsReport = new CampaignStatisticsReport();
            var fourthCampaignStatisticsReport = new CampaignStatisticsReport();
            var fifthCampaignStatisticsReport = new CampaignStatisticsReport();
            var sixthCampaignStatisticsReport = new CampaignStatisticsReport();

            // Act, Assert
            firstCampaignStatisticsReport.ShouldNotBeNull();
            secondCampaignStatisticsReport.ShouldNotBeNull();
            thirdCampaignStatisticsReport.ShouldNotBeNull();
            fourthCampaignStatisticsReport.ShouldNotBeNull();
            fifthCampaignStatisticsReport.ShouldNotBeNull();
            sixthCampaignStatisticsReport.ShouldNotBeNull();
            firstCampaignStatisticsReport.ShouldNotBeSameAs(secondCampaignStatisticsReport);
            thirdCampaignStatisticsReport.ShouldNotBeSameAs(firstCampaignStatisticsReport);
            fourthCampaignStatisticsReport.ShouldNotBeSameAs(firstCampaignStatisticsReport);
            fifthCampaignStatisticsReport.ShouldNotBeSameAs(firstCampaignStatisticsReport);
            sixthCampaignStatisticsReport.ShouldNotBeSameAs(firstCampaignStatisticsReport);
            sixthCampaignStatisticsReport.ShouldNotBeSameAs(fourthCampaignStatisticsReport);
        }

        #endregion

        #endregion

        #endregion
    }
}