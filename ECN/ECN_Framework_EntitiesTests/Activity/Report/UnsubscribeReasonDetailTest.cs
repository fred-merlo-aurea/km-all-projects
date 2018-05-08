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
    public class UnsubscribeReasonDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            var campaignItemName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var emailAddress = Fixture.Create<string>();
            var unsubscribeTime = Fixture.Create<DateTime>();
            var selectedReason = Fixture.Create<string>();

            // Act
            unsubscribeReasonDetail.CampaignItemName = campaignItemName;
            unsubscribeReasonDetail.EmailSubject = emailSubject;
            unsubscribeReasonDetail.GroupName = groupName;
            unsubscribeReasonDetail.EmailAddress = emailAddress;
            unsubscribeReasonDetail.UnsubscribeTime = unsubscribeTime;
            unsubscribeReasonDetail.SelectedReason = selectedReason;

            // Assert
            unsubscribeReasonDetail.CampaignItemName.ShouldBe(campaignItemName);
            unsubscribeReasonDetail.EmailSubject.ShouldBe(emailSubject);
            unsubscribeReasonDetail.GroupName.ShouldBe(groupName);
            unsubscribeReasonDetail.EmailAddress.ShouldBe(emailAddress);
            unsubscribeReasonDetail.UnsubscribeTime.ShouldBe(unsubscribeTime);
            unsubscribeReasonDetail.SelectedReason.ShouldBe(selectedReason);
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            unsubscribeReasonDetail.CampaignItemName = Fixture.Create<string>();
            var stringType = unsubscribeReasonDetail.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReasonDetail.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();
            var propertyInfo  = unsubscribeReasonDetail.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            unsubscribeReasonDetail.EmailAddress = Fixture.Create<string>();
            var stringType = unsubscribeReasonDetail.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReasonDetail.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();
            var propertyInfo  = unsubscribeReasonDetail.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            unsubscribeReasonDetail.EmailSubject = Fixture.Create<string>();
            var stringType = unsubscribeReasonDetail.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReasonDetail.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();
            var propertyInfo  = unsubscribeReasonDetail.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            unsubscribeReasonDetail.GroupName = Fixture.Create<string>();
            var stringType = unsubscribeReasonDetail.GroupName.GetType();

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

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReasonDetail.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();
            var propertyInfo  = unsubscribeReasonDetail.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (SelectedReason) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_SelectedReason_Property_String_Type_Verify_Test()
        {
            // Arrange
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            unsubscribeReasonDetail.SelectedReason = Fixture.Create<string>();
            var stringType = unsubscribeReasonDetail.SelectedReason.GetType();

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

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (SelectedReason) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_Invalid_Property_SelectedReasonNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSelectedReason = "SelectedReasonNotPresent";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReasonDetail.GetType().GetProperty(propertyNameSelectedReason));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_SelectedReason_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSelectedReason = "SelectedReason";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();
            var propertyInfo  = unsubscribeReasonDetail.GetType().GetProperty(propertyNameSelectedReason);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (UnsubscribeTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_UnsubscribeTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTime = "UnsubscribeTime";
            var unsubscribeReasonDetail = Fixture.Create<UnsubscribeReasonDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = unsubscribeReasonDetail.GetType().GetProperty(propertyNameUnsubscribeTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(unsubscribeReasonDetail, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (UnsubscribeReasonDetail) => Property (UnsubscribeTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_Class_Invalid_Property_UnsubscribeTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTime = "UnsubscribeTimeNotPresent";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();

            // Act , Assert
            Should.NotThrow(() => unsubscribeReasonDetail.GetType().GetProperty(propertyNameUnsubscribeTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UnsubscribeReasonDetail_UnsubscribeTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUnsubscribeTime = "UnsubscribeTime";
            var unsubscribeReasonDetail  = Fixture.Create<UnsubscribeReasonDetail>();
            var propertyInfo  = unsubscribeReasonDetail.GetType().GetProperty(propertyNameUnsubscribeTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (UnsubscribeReasonDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UnsubscribeReasonDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new UnsubscribeReasonDetail());
        }

        #endregion

        #region General Constructor : Class (UnsubscribeReasonDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UnsubscribeReasonDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfUnsubscribeReasonDetail = Fixture.CreateMany<UnsubscribeReasonDetail>(2).ToList();
            var firstUnsubscribeReasonDetail = instancesOfUnsubscribeReasonDetail.FirstOrDefault();
            var lastUnsubscribeReasonDetail = instancesOfUnsubscribeReasonDetail.Last();

            // Act, Assert
            firstUnsubscribeReasonDetail.ShouldNotBeNull();
            lastUnsubscribeReasonDetail.ShouldNotBeNull();
            firstUnsubscribeReasonDetail.ShouldNotBeSameAs(lastUnsubscribeReasonDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UnsubscribeReasonDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstUnsubscribeReasonDetail = new UnsubscribeReasonDetail();
            var secondUnsubscribeReasonDetail = new UnsubscribeReasonDetail();
            var thirdUnsubscribeReasonDetail = new UnsubscribeReasonDetail();
            var fourthUnsubscribeReasonDetail = new UnsubscribeReasonDetail();
            var fifthUnsubscribeReasonDetail = new UnsubscribeReasonDetail();
            var sixthUnsubscribeReasonDetail = new UnsubscribeReasonDetail();

            // Act, Assert
            firstUnsubscribeReasonDetail.ShouldNotBeNull();
            secondUnsubscribeReasonDetail.ShouldNotBeNull();
            thirdUnsubscribeReasonDetail.ShouldNotBeNull();
            fourthUnsubscribeReasonDetail.ShouldNotBeNull();
            fifthUnsubscribeReasonDetail.ShouldNotBeNull();
            sixthUnsubscribeReasonDetail.ShouldNotBeNull();
            firstUnsubscribeReasonDetail.ShouldNotBeSameAs(secondUnsubscribeReasonDetail);
            thirdUnsubscribeReasonDetail.ShouldNotBeSameAs(firstUnsubscribeReasonDetail);
            fourthUnsubscribeReasonDetail.ShouldNotBeSameAs(firstUnsubscribeReasonDetail);
            fifthUnsubscribeReasonDetail.ShouldNotBeSameAs(firstUnsubscribeReasonDetail);
            sixthUnsubscribeReasonDetail.ShouldNotBeSameAs(firstUnsubscribeReasonDetail);
            sixthUnsubscribeReasonDetail.ShouldNotBeSameAs(fourthUnsubscribeReasonDetail);
        }

        #endregion

        #endregion

        #endregion
    }
}