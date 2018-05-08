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
    public class DataDumpReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (DataDumpReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            var sendTime = Fixture.Create<DateTime>();
            var dayOfWeek = Fixture.Create<string>();
            var year = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var campaignName = Fixture.Create<string>();
            var campaignItemName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var messageName = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var suppressionGroups = Fixture.Create<string>();
            var blastField1 = Fixture.Create<string>();
            var blastField2 = Fixture.Create<string>();
            var blastField3 = Fixture.Create<string>();
            var blastField4 = Fixture.Create<string>();
            var blastField5 = Fixture.Create<string>();
            var usend = Fixture.Create<int>();
            var tbounce = Fixture.Create<int>();
            var tbouncePerc = Fixture.Create<decimal>();
            var delivery = Fixture.Create<int>();
            var successPerc = Fixture.Create<decimal>();
            var sendPerc = Fixture.Create<decimal>();
            var topen = Fixture.Create<int>();
            var opensPercentOfDelivered = Fixture.Create<decimal>();
            var uopen = Fixture.Create<int>();
            var uOpensPerc = Fixture.Create<decimal>();
            var tClick = Fixture.Create<int>();
            var tClickPerc = Fixture.Create<decimal>();
            var uClick = Fixture.Create<int>();
            var uClickPerc = Fixture.Create<decimal>();
            var clickThrough = Fixture.Create<int>();
            var clickThroughPerc = Fixture.Create<decimal>();
            var uSubscribe = Fixture.Create<int>();
            var uSubscribePerc = Fixture.Create<decimal>();
            var tClicksOpensPerc = Fixture.Create<decimal>();
            var uClicksOpensPerc = Fixture.Create<decimal>();
            var trefer = Fixture.Create<int>();
            var treferPerc = Fixture.Create<decimal>();
            var tresend = Fixture.Create<int>();
            var tresendPerc = Fixture.Create<decimal>();
            var suppressed = Fixture.Create<int>();
            var suppressedPerc = Fixture.Create<decimal>();
            var uHardBounce = Fixture.Create<int>();
            var uHardBouncePerc = Fixture.Create<decimal>();
            var uSoftBounce = Fixture.Create<int>();
            var uSoftBouncePerc = Fixture.Create<decimal>();
            var uOtherBounce = Fixture.Create<int>();
            var uOtherBouncePerc = Fixture.Create<decimal>();
            var uMastSup_Unsub = Fixture.Create<int>();
            var uMastSup_UnsubPerc = Fixture.Create<decimal>();
            var uAbuseRpt_Unsub = Fixture.Create<int>();
            var uAbuseRpt_UnsubPerc = Fixture.Create<decimal>();
            var uFeedBack_Unsub = Fixture.Create<int>();
            var uFeedBack_UnsubPerc = Fixture.Create<decimal>();
            var omnitureValues = Fixture.Create<string>();
            var omnitureDomains = Fixture.Create<string>();
            var templateName = Fixture.Create<string>();
            var aBAmount = Fixture.Create<int>();
            var aBIsAmount = Fixture.Create<string>();

            // Act
            dataDumpReport.SendTime = sendTime;
            dataDumpReport.DayOfWeek = dayOfWeek;
            dataDumpReport.Year = year;
            dataDumpReport.BlastID = blastId;
            dataDumpReport.CampaignName = campaignName;
            dataDumpReport.CampaignItemName = campaignItemName;
            dataDumpReport.EmailSubject = emailSubject;
            dataDumpReport.MessageName = messageName;
            dataDumpReport.GroupName = groupName;
            dataDumpReport.SuppressionGroups = suppressionGroups;
            dataDumpReport.BlastField1 = blastField1;
            dataDumpReport.BlastField2 = blastField2;
            dataDumpReport.BlastField3 = blastField3;
            dataDumpReport.BlastField4 = blastField4;
            dataDumpReport.BlastField5 = blastField5;
            dataDumpReport.usend = usend;
            dataDumpReport.tbounce = tbounce;
            dataDumpReport.tbouncePerc = tbouncePerc;
            dataDumpReport.Delivery = delivery;
            dataDumpReport.SuccessPerc = successPerc;
            dataDumpReport.sendPerc = sendPerc;
            dataDumpReport.topen = topen;
            dataDumpReport.OpensPercentOfDelivered = opensPercentOfDelivered;
            dataDumpReport.uopen = uopen;
            dataDumpReport.uOpensPerc = uOpensPerc;
            dataDumpReport.tClick = tClick;
            dataDumpReport.tClickPerc = tClickPerc;
            dataDumpReport.uClick = uClick;
            dataDumpReport.uClickPerc = uClickPerc;
            dataDumpReport.ClickThrough = clickThrough;
            dataDumpReport.ClickThroughPerc = clickThroughPerc;
            dataDumpReport.uSubscribe = uSubscribe;
            dataDumpReport.uSubscribePerc = uSubscribePerc;
            dataDumpReport.tClicksOpensPerc = tClicksOpensPerc;
            dataDumpReport.uClicksOpensPerc = uClicksOpensPerc;
            dataDumpReport.trefer = trefer;
            dataDumpReport.treferPerc = treferPerc;
            dataDumpReport.tresend = tresend;
            dataDumpReport.tresendPerc = tresendPerc;
            dataDumpReport.Suppressed = suppressed;
            dataDumpReport.SuppressedPerc = suppressedPerc;
            dataDumpReport.uHardBounce = uHardBounce;
            dataDumpReport.uHardBouncePerc = uHardBouncePerc;
            dataDumpReport.uSoftBounce = uSoftBounce;
            dataDumpReport.uSoftBouncePerc = uSoftBouncePerc;
            dataDumpReport.uOtherBounce = uOtherBounce;
            dataDumpReport.uOtherBouncePerc = uOtherBouncePerc;
            dataDumpReport.uMastSup_Unsub = uMastSup_Unsub;
            dataDumpReport.uMastSup_UnsubPerc = uMastSup_UnsubPerc;
            dataDumpReport.uAbuseRpt_Unsub = uAbuseRpt_Unsub;
            dataDumpReport.uAbuseRpt_UnsubPerc = uAbuseRpt_UnsubPerc;
            dataDumpReport.uFeedBack_Unsub = uFeedBack_Unsub;
            dataDumpReport.uFeedBack_UnsubPerc = uFeedBack_UnsubPerc;
            dataDumpReport.OmnitureValues = omnitureValues;
            dataDumpReport.OmnitureDomains = omnitureDomains;
            dataDumpReport.TemplateName = templateName;
            dataDumpReport.ABAmount = aBAmount;
            dataDumpReport.ABIsAmount = aBIsAmount;

            // Assert
            dataDumpReport.SendTime.ShouldBe(sendTime);
            dataDumpReport.DayOfWeek.ShouldBe(dayOfWeek);
            dataDumpReport.Year.ShouldBe(year);
            dataDumpReport.BlastID.ShouldBe(blastId);
            dataDumpReport.CampaignName.ShouldBe(campaignName);
            dataDumpReport.CampaignItemName.ShouldBe(campaignItemName);
            dataDumpReport.EmailSubject.ShouldBe(emailSubject);
            dataDumpReport.MessageName.ShouldBe(messageName);
            dataDumpReport.GroupName.ShouldBe(groupName);
            dataDumpReport.SuppressionGroups.ShouldBe(suppressionGroups);
            dataDumpReport.BlastField1.ShouldBe(blastField1);
            dataDumpReport.BlastField2.ShouldBe(blastField2);
            dataDumpReport.BlastField3.ShouldBe(blastField3);
            dataDumpReport.BlastField4.ShouldBe(blastField4);
            dataDumpReport.BlastField5.ShouldBe(blastField5);
            dataDumpReport.usend.ShouldBe(usend);
            dataDumpReport.tbounce.ShouldBe(tbounce);
            dataDumpReport.tbouncePerc.ShouldBe(tbouncePerc);
            dataDumpReport.Delivery.ShouldBe(delivery);
            dataDumpReport.SuccessPerc.ShouldBe(successPerc);
            dataDumpReport.sendPerc.ShouldBe(sendPerc);
            dataDumpReport.topen.ShouldBe(topen);
            dataDumpReport.OpensPercentOfDelivered.ShouldBe(opensPercentOfDelivered);
            dataDumpReport.uopen.ShouldBe(uopen);
            dataDumpReport.uOpensPerc.ShouldBe(uOpensPerc);
            dataDumpReport.tClick.ShouldBe(tClick);
            dataDumpReport.tClickPerc.ShouldBe(tClickPerc);
            dataDumpReport.uClick.ShouldBe(uClick);
            dataDumpReport.uClickPerc.ShouldBe(uClickPerc);
            dataDumpReport.ClickThrough.ShouldBe(clickThrough);
            dataDumpReport.ClickThroughPerc.ShouldBe(clickThroughPerc);
            dataDumpReport.uSubscribe.ShouldBe(uSubscribe);
            dataDumpReport.uSubscribePerc.ShouldBe(uSubscribePerc);
            dataDumpReport.tClicksOpensPerc.ShouldBe(tClicksOpensPerc);
            dataDumpReport.uClicksOpensPerc.ShouldBe(uClicksOpensPerc);
            dataDumpReport.trefer.ShouldBe(trefer);
            dataDumpReport.treferPerc.ShouldBe(treferPerc);
            dataDumpReport.tresend.ShouldBe(tresend);
            dataDumpReport.tresendPerc.ShouldBe(tresendPerc);
            dataDumpReport.Suppressed.ShouldBe(suppressed);
            dataDumpReport.SuppressedPerc.ShouldBe(suppressedPerc);
            dataDumpReport.uHardBounce.ShouldBe(uHardBounce);
            dataDumpReport.uHardBouncePerc.ShouldBe(uHardBouncePerc);
            dataDumpReport.uSoftBounce.ShouldBe(uSoftBounce);
            dataDumpReport.uSoftBouncePerc.ShouldBe(uSoftBouncePerc);
            dataDumpReport.uOtherBounce.ShouldBe(uOtherBounce);
            dataDumpReport.uOtherBouncePerc.ShouldBe(uOtherBouncePerc);
            dataDumpReport.uMastSup_Unsub.ShouldBe(uMastSup_Unsub);
            dataDumpReport.uMastSup_UnsubPerc.ShouldBe(uMastSup_UnsubPerc);
            dataDumpReport.uAbuseRpt_Unsub.ShouldBe(uAbuseRpt_Unsub);
            dataDumpReport.uAbuseRpt_UnsubPerc.ShouldBe(uAbuseRpt_UnsubPerc);
            dataDumpReport.uFeedBack_Unsub.ShouldBe(uFeedBack_Unsub);
            dataDumpReport.uFeedBack_UnsubPerc.ShouldBe(uFeedBack_UnsubPerc);
            dataDumpReport.OmnitureValues.ShouldBe(omnitureValues);
            dataDumpReport.OmnitureDomains.ShouldBe(omnitureDomains);
            dataDumpReport.TemplateName.ShouldBe(templateName);
            dataDumpReport.ABAmount.ShouldBe(aBAmount);
            dataDumpReport.ABIsAmount.ShouldBe(aBIsAmount);
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (ABAmount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ABAmount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.ABAmount = Fixture.Create<int>();
            var intType = dataDumpReport.ABAmount.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (ABAmount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_ABAmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameABAmount = "ABAmountNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameABAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ABAmount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameABAmount = "ABAmount";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameABAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (ABIsAmount) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ABIsAmount_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.ABIsAmount = Fixture.Create<string>();
            var stringType = dataDumpReport.ABIsAmount.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (ABIsAmount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_ABIsAmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameABIsAmount = "ABIsAmountNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameABIsAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ABIsAmount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameABIsAmount = "ABIsAmount";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameABIsAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.BlastField1 = Fixture.Create<string>();
            var stringType = dataDumpReport.BlastField1.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_BlastField1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField1 = "BlastField1NotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameBlastField1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField1 = "BlastField1";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameBlastField1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.BlastField2 = Fixture.Create<string>();
            var stringType = dataDumpReport.BlastField2.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_BlastField2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField2 = "BlastField2NotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameBlastField2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField2 = "BlastField2";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameBlastField2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField3) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField3_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.BlastField3 = Fixture.Create<string>();
            var stringType = dataDumpReport.BlastField3.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_BlastField3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField3 = "BlastField3NotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameBlastField3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField3 = "BlastField3";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameBlastField3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField4) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField4_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.BlastField4 = Fixture.Create<string>();
            var stringType = dataDumpReport.BlastField4.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_BlastField4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField4 = "BlastField4NotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameBlastField4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField4 = "BlastField4";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameBlastField4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField5) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField5_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.BlastField5 = Fixture.Create<string>();
            var stringType = dataDumpReport.BlastField5.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastField5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_BlastField5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastField5 = "BlastField5NotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameBlastField5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastField5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastField5 = "BlastField5";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameBlastField5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.BlastID = Fixture.Create<int>();
            var intType = dataDumpReport.BlastID.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.CampaignItemName = Fixture.Create<string>();
            var stringType = dataDumpReport.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (CampaignName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_CampaignName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.CampaignName = Fixture.Create<string>();
            var stringType = dataDumpReport.CampaignName.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (CampaignName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_CampaignNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignName = "CampaignNameNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameCampaignName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_CampaignName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignName = "CampaignName";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameCampaignName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (ClickThrough) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ClickThrough_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.ClickThrough = Fixture.Create<int>();
            var intType = dataDumpReport.ClickThrough.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (ClickThrough) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_ClickThroughNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThroughNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameClickThrough));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ClickThrough_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThrough";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameClickThrough);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (ClickThroughPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ClickThroughPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.ClickThroughPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.ClickThroughPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (ClickThroughPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_ClickThroughPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPerc = "ClickThroughPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameClickThroughPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_ClickThroughPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThroughPerc = "ClickThroughPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameClickThroughPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (DayOfWeek) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_DayOfWeek_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.DayOfWeek = Fixture.Create<string>();
            var stringType = dataDumpReport.DayOfWeek.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (DayOfWeek) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_DayOfWeekNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDayOfWeek = "DayOfWeekNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameDayOfWeek));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_DayOfWeek_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDayOfWeek = "DayOfWeek";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameDayOfWeek);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (Delivery) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Delivery_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.Delivery = Fixture.Create<int>();
            var intType = dataDumpReport.Delivery.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (Delivery) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_DeliveryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivery = "DeliveryNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameDelivery));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Delivery_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivery = "Delivery";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameDelivery);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.EmailSubject = Fixture.Create<string>();
            var stringType = dataDumpReport.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.GroupName = Fixture.Create<string>();
            var stringType = dataDumpReport.GroupName.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (MessageName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_MessageName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.MessageName = Fixture.Create<string>();
            var stringType = dataDumpReport.MessageName.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (MessageName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_MessageNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessageName = "MessageNameNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameMessageName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_MessageName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessageName = "MessageName";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameMessageName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (OmnitureDomains) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_OmnitureDomains_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.OmnitureDomains = Fixture.Create<string>();
            var stringType = dataDumpReport.OmnitureDomains.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (OmnitureDomains) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_OmnitureDomainsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOmnitureDomains = "OmnitureDomainsNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameOmnitureDomains));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_OmnitureDomains_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOmnitureDomains = "OmnitureDomains";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameOmnitureDomains);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (OmnitureValues) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_OmnitureValues_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.OmnitureValues = Fixture.Create<string>();
            var stringType = dataDumpReport.OmnitureValues.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (OmnitureValues) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_OmnitureValuesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOmnitureValues = "OmnitureValuesNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameOmnitureValues));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_OmnitureValues_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOmnitureValues = "OmnitureValues";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameOmnitureValues);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (OpensPercentOfDelivered) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_OpensPercentOfDelivered_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.OpensPercentOfDelivered = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.OpensPercentOfDelivered.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (OpensPercentOfDelivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_OpensPercentOfDeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpensPercentOfDelivered = "OpensPercentOfDeliveredNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameOpensPercentOfDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_OpensPercentOfDelivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpensPercentOfDelivered = "OpensPercentOfDelivered";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameOpensPercentOfDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (sendPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_sendPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.sendPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.sendPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (sendPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_sendPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamesendPerc = "sendPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNamesendPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_sendPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamesendPerc = "sendPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNamesendPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = dataDumpReport.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(dataDumpReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (SuccessPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SuccessPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.SuccessPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.SuccessPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (SuccessPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_SuccessPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuccessPerc = "SuccessPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameSuccessPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SuccessPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuccessPerc = "SuccessPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameSuccessPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (Suppressed) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Suppressed_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.Suppressed = Fixture.Create<int>();
            var intType = dataDumpReport.Suppressed.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (Suppressed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_SuppressedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressed = "SuppressedNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameSuppressed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Suppressed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressed = "Suppressed";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameSuppressed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (SuppressedPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SuppressedPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.SuppressedPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.SuppressedPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (SuppressedPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_SuppressedPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedPerc = "SuppressedPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameSuppressedPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SuppressedPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedPerc = "SuppressedPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameSuppressedPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (SuppressionGroups) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SuppressionGroups_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.SuppressionGroups = Fixture.Create<string>();
            var stringType = dataDumpReport.SuppressionGroups.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (SuppressionGroups) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_SuppressionGroupsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroups = "SuppressionGroupsNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameSuppressionGroups));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_SuppressionGroups_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressionGroups = "SuppressionGroups";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameSuppressionGroups);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tbounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tbounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tbounce = Fixture.Create<int>();
            var intType = dataDumpReport.tbounce.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tbounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tbounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametbounce = "tbounceNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametbounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tbounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametbounce = "tbounce";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametbounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tbouncePerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tbouncePerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tbouncePerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.tbouncePerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tbouncePerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tbouncePercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametbouncePerc = "tbouncePercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametbouncePerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tbouncePerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametbouncePerc = "tbouncePerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametbouncePerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tClick) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tClick_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tClick = Fixture.Create<int>();
            var intType = dataDumpReport.tClick.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tClick) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametClick = "tClickNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tClick_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametClick = "tClick";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tClickPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tClickPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tClickPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.tClickPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tClickPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tClickPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametClickPerc = "tClickPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametClickPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tClickPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametClickPerc = "tClickPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametClickPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tClicksOpensPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tClicksOpensPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tClicksOpensPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.tClicksOpensPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tClicksOpensPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tClicksOpensPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametClicksOpensPerc = "tClicksOpensPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametClicksOpensPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tClicksOpensPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametClicksOpensPerc = "tClicksOpensPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametClicksOpensPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (TemplateName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_TemplateName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.TemplateName = Fixture.Create<string>();
            var stringType = dataDumpReport.TemplateName.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (TemplateName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_TemplateNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTemplateName = "TemplateNameNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameTemplateName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_TemplateName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTemplateName = "TemplateName";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameTemplateName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (topen) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_topen_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.topen = Fixture.Create<int>();
            var intType = dataDumpReport.topen.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (topen) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_topenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametopen = "topenNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametopen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_topen_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametopen = "topen";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametopen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (trefer) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_trefer_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.trefer = Fixture.Create<int>();
            var intType = dataDumpReport.trefer.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (trefer) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_treferNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametrefer = "treferNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametrefer));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_trefer_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametrefer = "trefer";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametrefer);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (treferPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_treferPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.treferPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.treferPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (treferPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_treferPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametreferPerc = "treferPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametreferPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_treferPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametreferPerc = "treferPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametreferPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tresend) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tresend_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tresend = Fixture.Create<int>();
            var intType = dataDumpReport.tresend.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tresend) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tresendNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametresend = "tresendNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametresend));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tresend_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametresend = "tresend";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametresend);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (tresendPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tresendPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.tresendPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.tresendPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (tresendPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_tresendPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNametresendPerc = "tresendPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNametresendPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_tresendPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNametresendPerc = "tresendPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNametresendPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uAbuseRpt_Unsub) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uAbuseRpt_Unsub_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uAbuseRpt_Unsub = Fixture.Create<int>();
            var intType = dataDumpReport.uAbuseRpt_Unsub.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uAbuseRpt_Unsub) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uAbuseRpt_UnsubNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuAbuseRpt_Unsub = "uAbuseRpt_UnsubNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuAbuseRpt_Unsub));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uAbuseRpt_Unsub_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuAbuseRpt_Unsub = "uAbuseRpt_Unsub";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuAbuseRpt_Unsub);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uAbuseRpt_UnsubPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uAbuseRpt_UnsubPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uAbuseRpt_UnsubPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uAbuseRpt_UnsubPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uAbuseRpt_UnsubPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uAbuseRpt_UnsubPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuAbuseRpt_UnsubPerc = "uAbuseRpt_UnsubPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuAbuseRpt_UnsubPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uAbuseRpt_UnsubPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuAbuseRpt_UnsubPerc = "uAbuseRpt_UnsubPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuAbuseRpt_UnsubPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uClick) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uClick_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uClick = Fixture.Create<int>();
            var intType = dataDumpReport.uClick.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uClick) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uClickNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuClick = "uClickNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuClick));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uClick_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuClick = "uClick";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuClick);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uClickPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uClickPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uClickPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uClickPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uClickPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uClickPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuClickPerc = "uClickPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuClickPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uClickPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuClickPerc = "uClickPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuClickPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uClicksOpensPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uClicksOpensPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uClicksOpensPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uClicksOpensPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uClicksOpensPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uClicksOpensPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuClicksOpensPerc = "uClicksOpensPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuClicksOpensPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uClicksOpensPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuClicksOpensPerc = "uClicksOpensPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuClicksOpensPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uFeedBack_Unsub) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uFeedBack_Unsub_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uFeedBack_Unsub = Fixture.Create<int>();
            var intType = dataDumpReport.uFeedBack_Unsub.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uFeedBack_Unsub) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uFeedBack_UnsubNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuFeedBack_Unsub = "uFeedBack_UnsubNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuFeedBack_Unsub));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uFeedBack_Unsub_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuFeedBack_Unsub = "uFeedBack_Unsub";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuFeedBack_Unsub);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uFeedBack_UnsubPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uFeedBack_UnsubPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uFeedBack_UnsubPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uFeedBack_UnsubPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uFeedBack_UnsubPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uFeedBack_UnsubPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuFeedBack_UnsubPerc = "uFeedBack_UnsubPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuFeedBack_UnsubPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uFeedBack_UnsubPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuFeedBack_UnsubPerc = "uFeedBack_UnsubPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuFeedBack_UnsubPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uHardBounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uHardBounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uHardBounce = Fixture.Create<int>();
            var intType = dataDumpReport.uHardBounce.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uHardBounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uHardBounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuHardBounce = "uHardBounceNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuHardBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uHardBounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuHardBounce = "uHardBounce";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuHardBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uHardBouncePerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uHardBouncePerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uHardBouncePerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uHardBouncePerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uHardBouncePerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uHardBouncePercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuHardBouncePerc = "uHardBouncePercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuHardBouncePerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uHardBouncePerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuHardBouncePerc = "uHardBouncePerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuHardBouncePerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uMastSup_Unsub) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uMastSup_Unsub_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uMastSup_Unsub = Fixture.Create<int>();
            var intType = dataDumpReport.uMastSup_Unsub.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uMastSup_Unsub) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uMastSup_UnsubNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuMastSup_Unsub = "uMastSup_UnsubNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuMastSup_Unsub));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uMastSup_Unsub_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuMastSup_Unsub = "uMastSup_Unsub";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuMastSup_Unsub);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uMastSup_UnsubPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uMastSup_UnsubPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uMastSup_UnsubPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uMastSup_UnsubPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uMastSup_UnsubPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uMastSup_UnsubPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuMastSup_UnsubPerc = "uMastSup_UnsubPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuMastSup_UnsubPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uMastSup_UnsubPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuMastSup_UnsubPerc = "uMastSup_UnsubPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuMastSup_UnsubPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uopen) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uopen_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uopen = Fixture.Create<int>();
            var intType = dataDumpReport.uopen.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uopen) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uopenNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuopen = "uopenNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuopen));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uopen_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuopen = "uopen";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuopen);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uOpensPerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uOpensPerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uOpensPerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uOpensPerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uOpensPerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uOpensPercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuOpensPerc = "uOpensPercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuOpensPerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uOpensPerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuOpensPerc = "uOpensPerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuOpensPerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uOtherBounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uOtherBounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uOtherBounce = Fixture.Create<int>();
            var intType = dataDumpReport.uOtherBounce.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uOtherBounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uOtherBounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuOtherBounce = "uOtherBounceNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuOtherBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uOtherBounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuOtherBounce = "uOtherBounce";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuOtherBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uOtherBouncePerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uOtherBouncePerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uOtherBouncePerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uOtherBouncePerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uOtherBouncePerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uOtherBouncePercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuOtherBouncePerc = "uOtherBouncePercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuOtherBouncePerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uOtherBouncePerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuOtherBouncePerc = "uOtherBouncePerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuOtherBouncePerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (usend) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_usend_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.usend = Fixture.Create<int>();
            var intType = dataDumpReport.usend.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (usend) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_usendNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameusend = "usendNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameusend));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_usend_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameusend = "usend";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameusend);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSoftBounce) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSoftBounce_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uSoftBounce = Fixture.Create<int>();
            var intType = dataDumpReport.uSoftBounce.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSoftBounce) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uSoftBounceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuSoftBounce = "uSoftBounceNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuSoftBounce));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSoftBounce_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuSoftBounce = "uSoftBounce";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuSoftBounce);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSoftBouncePerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSoftBouncePerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uSoftBouncePerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uSoftBouncePerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSoftBouncePerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uSoftBouncePercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuSoftBouncePerc = "uSoftBouncePercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuSoftBouncePerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSoftBouncePerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuSoftBouncePerc = "uSoftBouncePerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuSoftBouncePerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSubscribe) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSubscribe_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uSubscribe = Fixture.Create<int>();
            var intType = dataDumpReport.uSubscribe.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSubscribe) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uSubscribeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuSubscribe = "uSubscribeNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuSubscribe));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSubscribe_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuSubscribe = "uSubscribe";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuSubscribe);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSubscribePerc) (Type : decimal) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSubscribePerc_Property_Decimal_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.uSubscribePerc = Fixture.Create<decimal>();
            var decimalType = dataDumpReport.uSubscribePerc.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (uSubscribePerc) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_uSubscribePercNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameuSubscribePerc = "uSubscribePercNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameuSubscribePerc));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_uSubscribePerc_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameuSubscribePerc = "uSubscribePerc";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameuSubscribePerc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (DataDumpReport) => Property (Year) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Year_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var dataDumpReport = Fixture.Create<DataDumpReport>();
            dataDumpReport.Year = Fixture.Create<int>();
            var intType = dataDumpReport.Year.GetType();

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

        #region General Getters/Setters : Class (DataDumpReport) => Property (Year) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Class_Invalid_Property_YearNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameYear = "YearNotPresent";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();

            // Act , Assert
            Should.NotThrow(() => dataDumpReport.GetType().GetProperty(propertyNameYear));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void DataDumpReport_Year_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameYear = "Year";
            var dataDumpReport  = Fixture.Create<DataDumpReport>();
            var propertyInfo  = dataDumpReport.GetType().GetProperty(propertyNameYear);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (DataDumpReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataDumpReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new DataDumpReport());
        }

        #endregion

        #region General Constructor : Class (DataDumpReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataDumpReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfDataDumpReport = Fixture.CreateMany<DataDumpReport>(2).ToList();
            var firstDataDumpReport = instancesOfDataDumpReport.FirstOrDefault();
            var lastDataDumpReport = instancesOfDataDumpReport.Last();

            // Act, Assert
            firstDataDumpReport.ShouldNotBeNull();
            lastDataDumpReport.ShouldNotBeNull();
            firstDataDumpReport.ShouldNotBeSameAs(lastDataDumpReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_DataDumpReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstDataDumpReport = new DataDumpReport();
            var secondDataDumpReport = new DataDumpReport();
            var thirdDataDumpReport = new DataDumpReport();
            var fourthDataDumpReport = new DataDumpReport();
            var fifthDataDumpReport = new DataDumpReport();
            var sixthDataDumpReport = new DataDumpReport();

            // Act, Assert
            firstDataDumpReport.ShouldNotBeNull();
            secondDataDumpReport.ShouldNotBeNull();
            thirdDataDumpReport.ShouldNotBeNull();
            fourthDataDumpReport.ShouldNotBeNull();
            fifthDataDumpReport.ShouldNotBeNull();
            sixthDataDumpReport.ShouldNotBeNull();
            firstDataDumpReport.ShouldNotBeSameAs(secondDataDumpReport);
            thirdDataDumpReport.ShouldNotBeSameAs(firstDataDumpReport);
            fourthDataDumpReport.ShouldNotBeSameAs(firstDataDumpReport);
            fifthDataDumpReport.ShouldNotBeSameAs(firstDataDumpReport);
            sixthDataDumpReport.ShouldNotBeSameAs(firstDataDumpReport);
            sixthDataDumpReport.ShouldNotBeSameAs(fourthDataDumpReport);
        }

        #endregion

        #endregion

        #endregion
    }
}