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
    public class BlastDeliveryTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastDelivery) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            var sendTime = Fixture.Create<DateTime>();
            var customerName = Fixture.Create<string>();
            var campaignName = Fixture.Create<string>();
            var campaignItemName = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var fromEmail = Fixture.Create<string>();
            var blastId = Fixture.Create<int>();
            var blastCategory = Fixture.Create<string>();
            var filterName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var sendTotal = Fixture.Create<int>();
            var delivered = Fixture.Create<int>();
            var softBounceTotal = Fixture.Create<int>();
            var softSendPercentage = Fixture.Create<Decimal>();
            var hardBounceTotal = Fixture.Create<int>();
            var hardSendPercentage = Fixture.Create<Decimal>();
            var bounceTotal = Fixture.Create<int>();
            var bounceSendPercentage = Fixture.Create<Decimal>();
            var uniqueOpens = Fixture.Create<int>();
            var uOpenDeliPercentage = Fixture.Create<Decimal>();
            var mobileOpens = Fixture.Create<int>();
            var unMobileOpenPercentage = Fixture.Create<Decimal>();
            var totalOpens = Fixture.Create<int>();
            var openDeliPercentage = Fixture.Create<Decimal>();
            var uniqueClicks = Fixture.Create<int>();
            var uClickDeliPercentage = Fixture.Create<Decimal>();
            var totalClicks = Fixture.Create<int>();
            var clickDeliPercentage = Fixture.Create<Decimal>();
            var uClickOpenPercentage = Fixture.Create<Decimal>();
            var clickOpenPercentage = Fixture.Create<Decimal>();
            var clickThrough = Fixture.Create<int>();
            var clickThroughPercentage = Fixture.Create<Decimal>();
            var unsubscribeTotal = Fixture.Create<int>();
            var unSubDeliPercentage = Fixture.Create<Decimal>();
            var suppressedTotal = Fixture.Create<int>();
            var field1 = Fixture.Create<string>();
            var field2 = Fixture.Create<string>();
            var field3 = Fixture.Create<string>();
            var field4 = Fixture.Create<string>();
            var field5 = Fixture.Create<string>();
            var spam = Fixture.Create<int>();
            var spamPercent = Fixture.Create<string>();
            var abuse = Fixture.Create<int>();
            var feedback = Fixture.Create<int>();
            var spamCount = Fixture.Create<int>();

            // Act
            blastDelivery.SendTime = sendTime;
            blastDelivery.CustomerName = customerName;
            blastDelivery.CampaignName = campaignName;
            blastDelivery.CampaignItemName = campaignItemName;
            blastDelivery.GroupName = groupName;
            blastDelivery.FromEmail = fromEmail;
            blastDelivery.BlastID = blastId;
            blastDelivery.BlastCategory = blastCategory;
            blastDelivery.FilterName = filterName;
            blastDelivery.EmailSubject = emailSubject;
            blastDelivery.SendTotal = sendTotal;
            blastDelivery.Delivered = delivered;
            blastDelivery.SoftBounceTotal = softBounceTotal;
            blastDelivery.SoftSendPercentage = softSendPercentage;
            blastDelivery.HardBounceTotal = hardBounceTotal;
            blastDelivery.HardSendPercentage = hardSendPercentage;
            blastDelivery.BounceTotal = bounceTotal;
            blastDelivery.BounceSendPercentage = bounceSendPercentage;
            blastDelivery.UniqueOpens = uniqueOpens;
            blastDelivery.UOpenDeliPercentage = uOpenDeliPercentage;
            blastDelivery.MobileOpens = mobileOpens;
            blastDelivery.UnMobileOpenPercentage = unMobileOpenPercentage;
            blastDelivery.TotalOpens = totalOpens;
            blastDelivery.OpenDeliPercentage = openDeliPercentage;
            blastDelivery.UniqueClicks = uniqueClicks;
            blastDelivery.UClickDeliPercentage = uClickDeliPercentage;
            blastDelivery.TotalClicks = totalClicks;
            blastDelivery.ClickDeliPercentage = clickDeliPercentage;
            blastDelivery.UClickOpenPercentage = uClickOpenPercentage;
            blastDelivery.ClickOpenPercentage = clickOpenPercentage;
            blastDelivery.ClickThrough = clickThrough;
            blastDelivery.ClickThroughPercentage = clickThroughPercentage;
            blastDelivery.UnsubscribeTotal = unsubscribeTotal;
            blastDelivery.UnSubDeliPercentage = unSubDeliPercentage;
            blastDelivery.SuppressedTotal = suppressedTotal;
            blastDelivery.Field1 = field1;
            blastDelivery.Field2 = field2;
            blastDelivery.Field3 = field3;
            blastDelivery.Field4 = field4;
            blastDelivery.Field5 = field5;
            blastDelivery.Spam = spam;
            blastDelivery.SpamPercent = spamPercent;
            blastDelivery.Abuse = abuse;
            blastDelivery.Feedback = feedback;
            blastDelivery.SpamCount = spamCount;

            // Assert
            blastDelivery.SendTime.ShouldNotBeNull();
            blastDelivery.CustomerName.ShouldBe(customerName);
            blastDelivery.CampaignName.ShouldBe(campaignName);
            blastDelivery.CampaignItemName.ShouldBe(campaignItemName);
            blastDelivery.Date.ShouldNotBeNull();
            blastDelivery.GroupName.ShouldBe(groupName);
            blastDelivery.FromEmail.ShouldBe(fromEmail);
            blastDelivery.BlastID.ShouldBe(blastId);
            blastDelivery.BlastCategory.ShouldBe(blastCategory);
            blastDelivery.FilterName.ShouldBe(filterName);
            blastDelivery.EmailSubject.ShouldBe(emailSubject);
            blastDelivery.SendTotal.ShouldBe(sendTotal);
            blastDelivery.Delivered.ShouldBe(delivered);
            blastDelivery.SoftBounceTotal.ShouldBe(softBounceTotal);
            blastDelivery.SoftSendPercentage.ShouldBe(softSendPercentage);
            blastDelivery.HardBounceTotal.ShouldBe(hardBounceTotal);
            blastDelivery.HardSendPercentage.ShouldBe(hardSendPercentage);
            blastDelivery.BounceTotal.ShouldBe(bounceTotal);
            blastDelivery.BounceSendPercentage.ShouldBe(bounceSendPercentage);
            blastDelivery.UniqueOpens.ShouldBe(uniqueOpens);
            blastDelivery.UOpenDeliPercentage.ShouldBe(uOpenDeliPercentage);
            blastDelivery.MobileOpens.ShouldBe(mobileOpens);
            blastDelivery.UnMobileOpenPercentage.ShouldBe(unMobileOpenPercentage);
            blastDelivery.TotalOpens.ShouldBe(totalOpens);
            blastDelivery.OpenDeliPercentage.ShouldBe(openDeliPercentage);
            blastDelivery.UniqueClicks.ShouldBe(uniqueClicks);
            blastDelivery.UClickDeliPercentage.ShouldBe(uClickDeliPercentage);
            blastDelivery.TotalClicks.ShouldBe(totalClicks);
            blastDelivery.ClickDeliPercentage.ShouldBe(clickDeliPercentage);
            blastDelivery.UClickOpenPercentage.ShouldBe(uClickOpenPercentage);
            blastDelivery.ClickOpenPercentage.ShouldBe(clickOpenPercentage);
            blastDelivery.ClickThrough.ShouldBe(clickThrough);
            blastDelivery.ClickThroughPercentage.ShouldBe(clickThroughPercentage);
            blastDelivery.UnsubscribeTotal.ShouldBe(unsubscribeTotal);
            blastDelivery.UnSubDeliPercentage.ShouldBe(unSubDeliPercentage);
            blastDelivery.SuppressedTotal.ShouldBe(suppressedTotal);
            blastDelivery.Field1.ShouldBe(field1);
            blastDelivery.Field2.ShouldBe(field2);
            blastDelivery.Field3.ShouldBe(field3);
            blastDelivery.Field4.ShouldBe(field4);
            blastDelivery.Field5.ShouldBe(field5);
            blastDelivery.Spam.ShouldBe(spam);
            blastDelivery.SpamPercent.ShouldBe(spamPercent);
            blastDelivery.Abuse.ShouldBe(abuse);
            blastDelivery.Feedback.ShouldBe(feedback);
            blastDelivery.SpamCount.ShouldBe(spamCount);
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Abuse) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Abuse_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Abuse = Fixture.Create<int>();
            var intType = blastDelivery.Abuse.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Abuse) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_AbuseNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAbuse = "AbuseNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameAbuse));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Abuse_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAbuse = "Abuse";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameAbuse);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (BlastCategory) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BlastCategory_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.BlastCategory = Fixture.Create<string>();
            var stringType = blastDelivery.BlastCategory.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (BlastCategory) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_BlastCategoryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastCategory = "BlastCategoryNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameBlastCategory));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BlastCategory_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastCategory = "BlastCategory";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameBlastCategory);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.BlastID = Fixture.Create<int>();
            var intType = blastDelivery.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (BounceSendPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BounceSendPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceSendPercentage = "BounceSendPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameBounceSendPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (BounceSendPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_BounceSendPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceSendPercentage = "BounceSendPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameBounceSendPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BounceSendPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceSendPercentage = "BounceSendPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameBounceSendPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (BounceTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BounceTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.BounceTotal = Fixture.Create<int>();
            var intType = blastDelivery.BounceTotal.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (BounceTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_BounceTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceTotal = "BounceTotalNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameBounceTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_BounceTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceTotal = "BounceTotal";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameBounceTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.CampaignItemName = Fixture.Create<string>();
            var stringType = blastDelivery.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (CampaignName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_CampaignName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.CampaignName = Fixture.Create<string>();
            var stringType = blastDelivery.CampaignName.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (CampaignName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_CampaignNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignName = "CampaignNameNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameCampaignName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_CampaignName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignName = "CampaignName";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameCampaignName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickDeliPercentage = "ClickDeliPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameClickDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_ClickDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickDeliPercentage = "ClickDeliPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameClickDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickDeliPercentage = "ClickDeliPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameClickDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickOpenPercentage = "ClickOpenPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameClickOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_ClickOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickOpenPercentage = "ClickOpenPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameClickOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickOpenPercentage = "ClickOpenPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameClickOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickThrough) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickThrough_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.ClickThrough = Fixture.Create<int>();
            var intType = blastDelivery.ClickThrough.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickThrough) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_ClickThroughNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThroughNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameClickThrough));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickThrough_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThrough = "ClickThrough";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameClickThrough);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickThroughPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickThroughPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (ClickThroughPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_ClickThroughPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameClickThroughPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_ClickThroughPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClickThroughPercentage = "ClickThroughPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameClickThroughPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.CustomerName = Fixture.Create<string>();
            var stringType = blastDelivery.CustomerName.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Date) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_DateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDate = "DateNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Date_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDate = "Date";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Delivered) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Delivered_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Delivered = Fixture.Create<int>();
            var intType = blastDelivery.Delivered.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Delivered) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_DeliveredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDelivered = "DeliveredNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameDelivered));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Delivered_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDelivered = "Delivered";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameDelivered);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.EmailSubject = Fixture.Create<string>();
            var stringType = blastDelivery.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Feedback) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Feedback_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Feedback = Fixture.Create<int>();
            var intType = blastDelivery.Feedback.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Feedback) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_FeedbackNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFeedback = "FeedbackNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameFeedback));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Feedback_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFeedback = "Feedback";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameFeedback);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Field1 = Fixture.Create<string>();
            var stringType = blastDelivery.Field1.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_Field1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField1 = "Field1NotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameField1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField1 = "Field1";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameField1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Field2 = Fixture.Create<string>();
            var stringType = blastDelivery.Field2.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_Field2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField2 = "Field2NotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameField2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField2 = "Field2";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameField2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field3) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field3_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Field3 = Fixture.Create<string>();
            var stringType = blastDelivery.Field3.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_Field3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField3 = "Field3NotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameField3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField3 = "Field3";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameField3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field4) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field4_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Field4 = Fixture.Create<string>();
            var stringType = blastDelivery.Field4.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_Field4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField4 = "Field4NotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameField4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField4 = "Field4";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameField4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field5) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field5_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Field5 = Fixture.Create<string>();
            var stringType = blastDelivery.Field5.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Field5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_Field5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameField5 = "Field5NotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameField5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Field5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameField5 = "Field5";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameField5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (FilterName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_FilterName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.FilterName = Fixture.Create<string>();
            var stringType = blastDelivery.FilterName.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (FilterName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_FilterNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterNameNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameFilterName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_FilterName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFilterName = "FilterName";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameFilterName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (FromEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_FromEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.FromEmail = Fixture.Create<string>();
            var stringType = blastDelivery.FromEmail.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (FromEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_FromEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmailNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameFromEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_FromEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmail";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameFromEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.GroupName = Fixture.Create<string>();
            var stringType = blastDelivery.GroupName.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (HardBounceTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_HardBounceTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.HardBounceTotal = Fixture.Create<int>();
            var intType = blastDelivery.HardBounceTotal.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (HardBounceTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_HardBounceTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHardBounceTotal = "HardBounceTotalNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameHardBounceTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_HardBounceTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHardBounceTotal = "HardBounceTotal";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameHardBounceTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (HardSendPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_HardSendPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameHardSendPercentage = "HardSendPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameHardSendPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (HardSendPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_HardSendPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHardSendPercentage = "HardSendPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameHardSendPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_HardSendPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHardSendPercentage = "HardSendPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameHardSendPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (MobileOpens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_MobileOpens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.MobileOpens = Fixture.Create<int>();
            var intType = blastDelivery.MobileOpens.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (MobileOpens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_MobileOpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMobileOpens = "MobileOpensNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameMobileOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_MobileOpens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMobileOpens = "MobileOpens";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameMobileOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (OpenDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_OpenDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenDeliPercentage = "OpenDeliPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameOpenDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (OpenDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_OpenDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenDeliPercentage = "OpenDeliPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameOpenDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_OpenDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpenDeliPercentage = "OpenDeliPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameOpenDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SendTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SendTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.SendTotal = Fixture.Create<int>();
            var intType = blastDelivery.SendTotal.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (SendTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SendTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotalNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSendTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SendTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTotal = "SendTotal";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSendTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SoftBounceTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SoftBounceTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.SoftBounceTotal = Fixture.Create<int>();
            var intType = blastDelivery.SoftBounceTotal.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (SoftBounceTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SoftBounceTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSoftBounceTotal = "SoftBounceTotalNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSoftBounceTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SoftBounceTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSoftBounceTotal = "SoftBounceTotal";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSoftBounceTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SoftSendPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SoftSendPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSoftSendPercentage = "SoftSendPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameSoftSendPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SoftSendPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SoftSendPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSoftSendPercentage = "SoftSendPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSoftSendPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SoftSendPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSoftSendPercentage = "SoftSendPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSoftSendPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (Spam) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Spam_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.Spam = Fixture.Create<int>();
            var intType = blastDelivery.Spam.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (Spam) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SpamNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSpam = "SpamNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSpam));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Spam_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSpam = "Spam";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSpam);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SpamCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SpamCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.SpamCount = Fixture.Create<int>();
            var intType = blastDelivery.SpamCount.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (SpamCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SpamCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSpamCount = "SpamCountNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSpamCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SpamCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSpamCount = "SpamCount";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSpamCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SpamPercent) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SpamPercent_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.SpamPercent = Fixture.Create<string>();
            var stringType = blastDelivery.SpamPercent.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (SpamPercent) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SpamPercentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSpamPercent = "SpamPercentNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSpamPercent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SpamPercent_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSpamPercent = "SpamPercent";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSpamPercent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (SuppressedTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SuppressedTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.SuppressedTotal = Fixture.Create<int>();
            var intType = blastDelivery.SuppressedTotal.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (SuppressedTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_SuppressedTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSuppressedTotal = "SuppressedTotalNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameSuppressedTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_SuppressedTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSuppressedTotal = "SuppressedTotal";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameSuppressedTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (TotalClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_TotalClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.TotalClicks = Fixture.Create<int>();
            var intType = blastDelivery.TotalClicks.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (TotalClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_TotalClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicksNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameTotalClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_TotalClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalClicks = "TotalClicks";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameTotalClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (TotalOpens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_TotalOpens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.TotalOpens = Fixture.Create<int>();
            var intType = blastDelivery.TotalOpens.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (TotalOpens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_TotalOpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalOpens = "TotalOpensNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameTotalOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_TotalOpens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalOpens = "TotalOpens";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameTotalOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UClickDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UClickDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUClickDeliPercentage = "UClickDeliPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameUClickDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UClickDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UClickDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUClickDeliPercentage = "UClickDeliPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUClickDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UClickDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUClickDeliPercentage = "UClickDeliPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUClickDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UClickOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UClickOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUClickOpenPercentage = "UClickOpenPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameUClickOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UClickOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UClickOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUClickOpenPercentage = "UClickOpenPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUClickOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UClickOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUClickOpenPercentage = "UClickOpenPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUClickOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UniqueClicks) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UniqueClicks_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.UniqueClicks = Fixture.Create<int>();
            var intType = blastDelivery.UniqueClicks.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (UniqueClicks) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UniqueClicksNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueClicks = "UniqueClicksNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUniqueClicks));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UniqueClicks_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueClicks = "UniqueClicks";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUniqueClicks);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UniqueOpens) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UniqueOpens_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.UniqueOpens = Fixture.Create<int>();
            var intType = blastDelivery.UniqueOpens.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (UniqueOpens) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UniqueOpensNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueOpens = "UniqueOpensNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUniqueOpens));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UniqueOpens_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueOpens = "UniqueOpens";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUniqueOpens);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UnMobileOpenPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UnMobileOpenPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUnMobileOpenPercentage = "UnMobileOpenPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameUnMobileOpenPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UnMobileOpenPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UnMobileOpenPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnMobileOpenPercentage = "UnMobileOpenPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUnMobileOpenPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UnMobileOpenPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnMobileOpenPercentage = "UnMobileOpenPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUnMobileOpenPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UnSubDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UnSubDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUnSubDeliPercentage = "UnSubDeliPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameUnSubDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UnSubDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UnSubDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnSubDeliPercentage = "UnSubDeliPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUnSubDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UnSubDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnSubDeliPercentage = "UnSubDeliPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUnSubDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UnsubscribeTotal) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UnsubscribeTotal_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastDelivery = new BlastDelivery();
            blastDelivery.UnsubscribeTotal = Fixture.Create<int>();
            var intType = blastDelivery.UnsubscribeTotal.GetType();

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

        #region General Getters/Setters : Class (BlastDelivery) => Property (UnsubscribeTotal) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UnsubscribeTotalNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTotal = "UnsubscribeTotalNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUnsubscribeTotal));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UnsubscribeTotal_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTotal = "UnsubscribeTotal";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUnsubscribeTotal);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UOpenDeliPercentage) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UOpenDeliPercentage_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUOpenDeliPercentage = "UOpenDeliPercentage";
            var blastDelivery = new BlastDelivery();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastDelivery.GetType().GetProperty(propertyNameUOpenDeliPercentage);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastDelivery, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastDelivery) => Property (UOpenDeliPercentage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_Class_Invalid_Property_UOpenDeliPercentageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUOpenDeliPercentage = "UOpenDeliPercentageNotPresent";
            var blastDelivery  = new BlastDelivery();

            // Act , Assert
            Should.NotThrow(() => blastDelivery.GetType().GetProperty(propertyNameUOpenDeliPercentage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastDelivery_UOpenDeliPercentage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUOpenDeliPercentage = "UOpenDeliPercentage";
            var blastDelivery  = new BlastDelivery();
            var propertyInfo  = blastDelivery.GetType().GetProperty(propertyNameUOpenDeliPercentage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastDelivery) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastDelivery_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastDelivery());
        }

        #endregion

        #region General Constructor : Class (BlastDelivery) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastDelivery_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastDelivery = new BlastDelivery();
            var secondBlastDelivery = new BlastDelivery();
            var thirdBlastDelivery = new BlastDelivery();
            var fourthBlastDelivery = new BlastDelivery();
            var fifthBlastDelivery = new BlastDelivery();
            var sixthBlastDelivery = new BlastDelivery();

            // Act, Assert
            firstBlastDelivery.ShouldNotBeNull();
            secondBlastDelivery.ShouldNotBeNull();
            thirdBlastDelivery.ShouldNotBeNull();
            fourthBlastDelivery.ShouldNotBeNull();
            fifthBlastDelivery.ShouldNotBeNull();
            sixthBlastDelivery.ShouldNotBeNull();
            firstBlastDelivery.ShouldNotBeSameAs(secondBlastDelivery);
            thirdBlastDelivery.ShouldNotBeSameAs(firstBlastDelivery);
            fourthBlastDelivery.ShouldNotBeSameAs(firstBlastDelivery);
            fifthBlastDelivery.ShouldNotBeSameAs(firstBlastDelivery);
            sixthBlastDelivery.ShouldNotBeSameAs(firstBlastDelivery);
            sixthBlastDelivery.ShouldNotBeSameAs(fourthBlastDelivery);
        }

        #endregion

        #endregion

        #endregion
    }
}