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
    public class GroupStatisticsReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (GroupStatisticsReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            var sendTime = Fixture.Create<DateTime>();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var campaignName = Fixture.Create<string>();
            var tSend = Fixture.Create<int>();
            var uBounce = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var tOpen = Fixture.Create<int>();
            var uOpen = Fixture.Create<int>();
            var tClick = Fixture.Create<int>();
            var uClick = Fixture.Create<int>();
            var clickThrough = Fixture.Create<int>();
            var clickThroughPercentage = Fixture.Create<Decimal>();
            var uUnsubscribe = Fixture.Create<int>();
            var successPercentage = Fixture.Create<Decimal>();
            var deliveredOpensPercentage = Fixture.Create<Decimal>();
            var uniqueOpensPercentage = Fixture.Create<Decimal>();
            var deliveredClicksPercentage = Fixture.Create<Decimal>();
            var uniqueClicksPercentage = Fixture.Create<Decimal>();
            var openClicksPercentage = Fixture.Create<Decimal>();
            var uniqueOpenClicksPercentage = Fixture.Create<Decimal>();
            var browserStats = Fixture.Create<List<ECN_Framework_Entities.Activity.Report.PlatformDetail>>();
            var filter = Fixture.Create<String>();
            var suppressed = Fixture.Create<int>();

            // Act
            groupStatisticsReport.SendTime = sendTime;
            groupStatisticsReport.BlastID = blastId;
            groupStatisticsReport.EmailSubject = emailSubject;
            groupStatisticsReport.CampaignName = campaignName;
            groupStatisticsReport.TSend = tSend;
            groupStatisticsReport.UBounce = uBounce;
            groupStatisticsReport.Delivered = delivered;
            groupStatisticsReport.TOpen = tOpen;
            groupStatisticsReport.UOpen = uOpen;
            groupStatisticsReport.TClick = tClick;
            groupStatisticsReport.UClick = uClick;
            groupStatisticsReport.ClickThrough = clickThrough;
            groupStatisticsReport.ClickThroughPercentage = clickThroughPercentage;
            groupStatisticsReport.UUnsubscribe = uUnsubscribe;
            groupStatisticsReport.SuccessPercentage = successPercentage;
            groupStatisticsReport.DeliveredOpensPercentage = deliveredOpensPercentage;
            groupStatisticsReport.UniqueOpensPercentage = uniqueOpensPercentage;
            groupStatisticsReport.DeliveredClicksPercentage = deliveredClicksPercentage;
            groupStatisticsReport.UniqueClicksPercentage = uniqueClicksPercentage;
            groupStatisticsReport.OpenClicksPercentage = openClicksPercentage;
            groupStatisticsReport.UniqueOpenClicksPercentage = uniqueOpenClicksPercentage;
            groupStatisticsReport.BrowserStats = browserStats;
            groupStatisticsReport.Filter = filter;
            groupStatisticsReport.Suppressed = suppressed;

            // Assert
            groupStatisticsReport.SendTime.ShouldNotBeNull();
            groupStatisticsReport.Date.ShouldNotBeNull();
            groupStatisticsReport.BlastID.ShouldBe(blastId);
            groupStatisticsReport.EmailSubject.ShouldBe(emailSubject);
            groupStatisticsReport.CampaignName.ShouldBe(campaignName);
            groupStatisticsReport.TSend.ShouldBe(tSend);
            groupStatisticsReport.UBounce.ShouldBe(uBounce);
            groupStatisticsReport.Delivered.ShouldBe(delivered);
            groupStatisticsReport.TOpen.ShouldBe(tOpen);
            groupStatisticsReport.UOpen.ShouldBe(uOpen);
            groupStatisticsReport.TClick.ShouldBe(tClick);
            groupStatisticsReport.UClick.ShouldBe(uClick);
            groupStatisticsReport.ClickThrough.ShouldBe(clickThrough);
            groupStatisticsReport.ClickThroughPercentage.ShouldBe(clickThroughPercentage);
            groupStatisticsReport.UUnsubscribe.ShouldBe(uUnsubscribe);
            groupStatisticsReport.SuccessPercentage.ShouldBe(successPercentage);
            groupStatisticsReport.DeliveredOpensPercentage.ShouldBe(deliveredOpensPercentage);
            groupStatisticsReport.UniqueOpensPercentage.ShouldBe(uniqueOpensPercentage);
            groupStatisticsReport.DeliveredClicksPercentage.ShouldBe(deliveredClicksPercentage);
            groupStatisticsReport.UniqueClicksPercentage.ShouldBe(uniqueClicksPercentage);
            groupStatisticsReport.OpenClicksPercentage.ShouldBe(openClicksPercentage);
            groupStatisticsReport.UniqueOpenClicksPercentage.ShouldBe(uniqueOpenClicksPercentage);
            groupStatisticsReport.BrowserStats.ShouldBe(browserStats);
            groupStatisticsReport.Filter.ShouldBe(filter);
            groupStatisticsReport.Suppressed.ShouldBe(suppressed);
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.BlastID = Fixture.Create<int>();
            var intType = groupStatisticsReport.BlastID.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (BrowserStats) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_BrowserStatsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBrowserStats = "BrowserStatsNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameBrowserStats));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_BrowserStats_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBrowserStats = "BrowserStats";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameBrowserStats);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (CampaignName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_CampaignName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.CampaignName = Fixture.Create<string>();
            var stringType = groupStatisticsReport.CampaignName.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (CampaignName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_CampaignNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignName = "CampaignNameNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameCampaignName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_CampaignName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignName = "CampaignName";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameCampaignName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (ClickThrough) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_ClickThrough_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.ClickThrough = Fixture.Create<int>();
            var intType = groupStatisticsReport.ClickThrough.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (ClickThrough) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_ClickThroughNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThroughNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameClickThrough));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_ClickThrough_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThrough";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameClickThrough);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (ClickThroughPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_ClickThroughPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (ClickThroughPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_ClickThroughPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameClickThroughPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_ClickThroughPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (Date) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_DateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDate = "DateNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Date_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDate = "Date";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.Delivered = Fixture.Create<int>();
            var intType = groupStatisticsReport.Delivered.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (DeliveredClicksPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_DeliveredClicksPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDeliveredClicksPercentage = "DeliveredClicksPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameDeliveredClicksPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (DeliveredClicksPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_DeliveredClicksPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDeliveredClicksPercentage = "DeliveredClicksPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameDeliveredClicksPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_DeliveredClicksPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDeliveredClicksPercentage = "DeliveredClicksPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameDeliveredClicksPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (DeliveredOpensPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_DeliveredOpensPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDeliveredOpensPercentage = "DeliveredOpensPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameDeliveredOpensPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (DeliveredOpensPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_DeliveredOpensPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDeliveredOpensPercentage = "DeliveredOpensPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameDeliveredOpensPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_DeliveredOpensPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDeliveredOpensPercentage = "DeliveredOpensPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameDeliveredOpensPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.EmailSubject = Fixture.Create<string>();
            var stringType = groupStatisticsReport.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (Filter) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_FilterNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilter = "FilterNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameFilter));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Filter_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilter = "Filter";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameFilter);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (OpenClicksPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_OpenClicksPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenClicksPercentage = "OpenClicksPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameOpenClicksPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (OpenClicksPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_OpenClicksPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenClicksPercentage = "OpenClicksPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameOpenClicksPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_OpenClicksPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpenClicksPercentage = "OpenClicksPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameOpenClicksPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (SuccessPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_SuccessPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSuccessPercentage = "SuccessPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameSuccessPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (SuccessPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_SuccessPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuccessPercentage = "SuccessPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameSuccessPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_SuccessPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuccessPercentage = "SuccessPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameSuccessPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (Suppressed) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Suppressed_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.Suppressed = Fixture.Create<int>();
            var intType = groupStatisticsReport.Suppressed.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (Suppressed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_SuppressedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressed = "SuppressedNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameSuppressed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Suppressed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressed = "Suppressed";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameSuppressed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (TClick) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_TClick_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.TClick = Fixture.Create<int>();
            var intType = groupStatisticsReport.TClick.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (TClick) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_TClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTClick = "TClickNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameTClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_TClick_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTClick = "TClick";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameTClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (TOpen) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_TOpen_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.TOpen = Fixture.Create<int>();
            var intType = groupStatisticsReport.TOpen.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (TOpen) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_TOpenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTOpen = "TOpenNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameTOpen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_TOpen_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTOpen = "TOpen";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameTOpen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (TSend) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_TSend_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.TSend = Fixture.Create<int>();
            var intType = groupStatisticsReport.TSend.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (TSend) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_TSendNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTSend = "TSendNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameTSend));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_TSend_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTSend = "TSend";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameTSend);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UBounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UBounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.UBounce = Fixture.Create<int>();
            var intType = groupStatisticsReport.UBounce.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UBounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UBounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUBounce = "UBounceNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UBounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUBounce = "UBounce";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UClick) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UClick_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.UClick = Fixture.Create<int>();
            var intType = groupStatisticsReport.UClick.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UClick) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUClick = "UClickNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UClick_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUClick = "UClick";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UniqueClicksPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UniqueClicksPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClicksPercentage = "UniqueClicksPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameUniqueClicksPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UniqueClicksPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UniqueClicksPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClicksPercentage = "UniqueClicksPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUniqueClicksPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UniqueClicksPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueClicksPercentage = "UniqueClicksPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUniqueClicksPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UniqueOpenClicksPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UniqueOpenClicksPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpenClicksPercentage = "UniqueOpenClicksPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameUniqueOpenClicksPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UniqueOpenClicksPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UniqueOpenClicksPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpenClicksPercentage = "UniqueOpenClicksPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUniqueOpenClicksPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UniqueOpenClicksPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueOpenClicksPercentage = "UniqueOpenClicksPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUniqueOpenClicksPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UniqueOpensPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UniqueOpensPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpensPercentage = "UniqueOpensPercentage";
            var groupStatisticsReport = new GroupStatisticsReport();
            var randomString = Fixture.Create<string>();
            var propertyInfo = groupStatisticsReport.GetType().GetProperty(propertyNameUniqueOpensPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(groupStatisticsReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UniqueOpensPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UniqueOpensPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpensPercentage = "UniqueOpensPercentageNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUniqueOpensPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UniqueOpensPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueOpensPercentage = "UniqueOpensPercentage";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUniqueOpensPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UOpen) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UOpen_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.UOpen = Fixture.Create<int>();
            var intType = groupStatisticsReport.UOpen.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UOpen) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UOpenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUOpen = "UOpenNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUOpen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UOpen_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUOpen = "UOpen";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUOpen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UUnsubscribe) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UUnsubscribe_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var groupStatisticsReport = new GroupStatisticsReport();
            groupStatisticsReport.UUnsubscribe = Fixture.Create<int>();
            var intType = groupStatisticsReport.UUnsubscribe.GetType();

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

        #region General Getters/Setters : Class (GroupStatisticsReport) => Property (UUnsubscribe) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_Class_Invalid_Property_UUnsubscribeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUUnsubscribe = "UUnsubscribeNotPresent";
            var groupStatisticsReport  = new GroupStatisticsReport();

            // Act , Assert
            Should.NotThrow(() => groupStatisticsReport.GetType().GetProperty(propertyNameUUnsubscribe));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void GroupStatisticsReport_UUnsubscribe_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUUnsubscribe = "UUnsubscribe";
            var groupStatisticsReport  = new GroupStatisticsReport();
            var propertyInfo  = groupStatisticsReport.GetType().GetProperty(propertyNameUUnsubscribe);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (GroupStatisticsReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupStatisticsReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new GroupStatisticsReport());
        }

        #endregion

        #region General Constructor : Class (GroupStatisticsReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_GroupStatisticsReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGroupStatisticsReport = new GroupStatisticsReport();
            var secondGroupStatisticsReport = new GroupStatisticsReport();
            var thirdGroupStatisticsReport = new GroupStatisticsReport();
            var fourthGroupStatisticsReport = new GroupStatisticsReport();
            var fifthGroupStatisticsReport = new GroupStatisticsReport();
            var sixthGroupStatisticsReport = new GroupStatisticsReport();

            // Act, Assert
            firstGroupStatisticsReport.ShouldNotBeNull();
            secondGroupStatisticsReport.ShouldNotBeNull();
            thirdGroupStatisticsReport.ShouldNotBeNull();
            fourthGroupStatisticsReport.ShouldNotBeNull();
            fifthGroupStatisticsReport.ShouldNotBeNull();
            sixthGroupStatisticsReport.ShouldNotBeNull();
            firstGroupStatisticsReport.ShouldNotBeSameAs(secondGroupStatisticsReport);
            thirdGroupStatisticsReport.ShouldNotBeSameAs(firstGroupStatisticsReport);
            fourthGroupStatisticsReport.ShouldNotBeSameAs(firstGroupStatisticsReport);
            fifthGroupStatisticsReport.ShouldNotBeSameAs(firstGroupStatisticsReport);
            sixthGroupStatisticsReport.ShouldNotBeSameAs(firstGroupStatisticsReport);
            sixthGroupStatisticsReport.ShouldNotBeSameAs(fourthGroupStatisticsReport);
        }

        #endregion

        #endregion

        #endregion
    }
}