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
using ECN_Framework_Entities.Accounts.Report;

namespace ECN_Framework_Entities.Accounts.Report
{
    [TestFixture]
    public class TotalBlastsForDayTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (TotalBlastsForDay) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            var customerId = Fixture.Create<int>();
            var customerName = Fixture.Create<string>();
            var campaignItemName = Fixture.Create<string>();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var sendTime = Fixture.Create<DateTime>();
            var status = Fixture.Create<string>();
            var isTestBlast = Fixture.Create<string>();
            var sendCount = Fixture.Create<long>();
            var uniqueEmailOpenCount = Fixture.Create<int>();
            var uniqueEmailClickCount = Fixture.Create<int>();

            // Act
            totalBlastsForDay.CustomerID = customerId;
            totalBlastsForDay.CustomerName = customerName;
            totalBlastsForDay.CampaignItemName = campaignItemName;
            totalBlastsForDay.BlastID = blastId;
            totalBlastsForDay.EmailSubject = emailSubject;
            totalBlastsForDay.GroupName = groupName;
            totalBlastsForDay.SendTime = sendTime;
            totalBlastsForDay.Status = status;
            totalBlastsForDay.IsTestBlast = isTestBlast;
            totalBlastsForDay.SendCount = sendCount;
            totalBlastsForDay.UniqueEmailOpenCount = uniqueEmailOpenCount;
            totalBlastsForDay.UniqueEmailClickCount = uniqueEmailClickCount;

            // Assert
            totalBlastsForDay.CustomerID.ShouldBe(customerId);
            totalBlastsForDay.CustomerName.ShouldBe(customerName);
            totalBlastsForDay.CampaignItemName.ShouldBe(campaignItemName);
            totalBlastsForDay.BlastID.ShouldBe(blastId);
            totalBlastsForDay.EmailSubject.ShouldBe(emailSubject);
            totalBlastsForDay.GroupName.ShouldBe(groupName);
            totalBlastsForDay.SendTime.ShouldBe(sendTime);
            totalBlastsForDay.Status.ShouldBe(status);
            totalBlastsForDay.IsTestBlast.ShouldBe(isTestBlast);
            totalBlastsForDay.SendCount.ShouldBe(sendCount);
            totalBlastsForDay.UniqueEmailOpenCount.ShouldBe(uniqueEmailOpenCount);
            totalBlastsForDay.UniqueEmailClickCount.ShouldBe(uniqueEmailClickCount);
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.BlastID = Fixture.Create<int>();
            var intType = totalBlastsForDay.BlastID.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (CampaignItemName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_CampaignItemName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.CampaignItemName = Fixture.Create<string>();
            var stringType = totalBlastsForDay.CampaignItemName.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (CampaignItemName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_CampaignItemNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemNameNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameCampaignItemName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_CampaignItemName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaignItemName = "CampaignItemName";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameCampaignItemName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.CustomerID = Fixture.Create<int>();
            var intType = totalBlastsForDay.CustomerID.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.CustomerName = Fixture.Create<string>();
            var stringType = totalBlastsForDay.CustomerName.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.EmailSubject = Fixture.Create<string>();
            var stringType = totalBlastsForDay.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.GroupName = Fixture.Create<string>();
            var stringType = totalBlastsForDay.GroupName.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (IsTestBlast) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_IsTestBlast_Property_String_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.IsTestBlast = Fixture.Create<string>();
            var stringType = totalBlastsForDay.IsTestBlast.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (IsTestBlast) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_IsTestBlastNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsTestBlast = "IsTestBlastNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameIsTestBlast));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_IsTestBlast_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsTestBlast = "IsTestBlast";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameIsTestBlast);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (SendCount) (Type : long) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_SendCount_Property_Long_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.SendCount = Fixture.Create<long>();
            var longType = totalBlastsForDay.SendCount.GetType();

            // Act
            var isTypeLong = typeof(long) == (longType);
            var isTypeNullableLong = typeof(long?) == (longType);
            var isTypeString = typeof(string) == (longType);
            var isTypeInt = typeof(int) == (longType);
            var isTypeDecimal = typeof(decimal) == (longType);
            var isTypeBool = typeof(bool) == (longType);
            var isTypeDouble = typeof(double) == (longType);
            var isTypeFloat = typeof(float) == (longType);
            var isTypeIntNullable = typeof(int?) == (longType);
            var isTypeDecimalNullable = typeof(decimal?) == (longType);
            var isTypeBoolNullable = typeof(bool?) == (longType);
            var isTypeDoubleNullable = typeof(double?) == (longType);
            var isTypeFloatNullable = typeof(float?) == (longType);

            // Assert
            isTypeLong.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableLong.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (SendCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_SendCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendCount = "SendCountNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameSendCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_SendCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendCount = "SendCount";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameSendCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = totalBlastsForDay.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(totalBlastsForDay, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (Status) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Status_Property_String_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.Status = Fixture.Create<string>();
            var stringType = totalBlastsForDay.Status.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (UniqueEmailClickCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_UniqueEmailClickCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.UniqueEmailClickCount = Fixture.Create<int>();
            var intType = totalBlastsForDay.UniqueEmailClickCount.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (UniqueEmailClickCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_UniqueEmailClickCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueEmailClickCount = "UniqueEmailClickCountNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameUniqueEmailClickCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_UniqueEmailClickCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueEmailClickCount = "UniqueEmailClickCount";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameUniqueEmailClickCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (UniqueEmailOpenCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_UniqueEmailOpenCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var totalBlastsForDay = Fixture.Create<TotalBlastsForDay>();
            totalBlastsForDay.UniqueEmailOpenCount = Fixture.Create<int>();
            var intType = totalBlastsForDay.UniqueEmailOpenCount.GetType();

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

        #region General Getters/Setters : Class (TotalBlastsForDay) => Property (UniqueEmailOpenCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_Class_Invalid_Property_UniqueEmailOpenCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUniqueEmailOpenCount = "UniqueEmailOpenCountNotPresent";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();

            // Act , Assert
            Should.NotThrow(() => totalBlastsForDay.GetType().GetProperty(propertyNameUniqueEmailOpenCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void TotalBlastsForDay_UniqueEmailOpenCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUniqueEmailOpenCount = "UniqueEmailOpenCount";
            var totalBlastsForDay  = Fixture.Create<TotalBlastsForDay>();
            var propertyInfo  = totalBlastsForDay.GetType().GetProperty(propertyNameUniqueEmailOpenCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (TotalBlastsForDay) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_TotalBlastsForDay_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new TotalBlastsForDay());
        }

        #endregion

        #region General Constructor : Class (TotalBlastsForDay) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_TotalBlastsForDay_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfTotalBlastsForDay = Fixture.CreateMany<TotalBlastsForDay>(2).ToList();
            var firstTotalBlastsForDay = instancesOfTotalBlastsForDay.FirstOrDefault();
            var lastTotalBlastsForDay = instancesOfTotalBlastsForDay.Last();

            // Act, Assert
            firstTotalBlastsForDay.ShouldNotBeNull();
            lastTotalBlastsForDay.ShouldNotBeNull();
            firstTotalBlastsForDay.ShouldNotBeSameAs(lastTotalBlastsForDay);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_TotalBlastsForDay_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstTotalBlastsForDay = new TotalBlastsForDay();
            var secondTotalBlastsForDay = new TotalBlastsForDay();
            var thirdTotalBlastsForDay = new TotalBlastsForDay();
            var fourthTotalBlastsForDay = new TotalBlastsForDay();
            var fifthTotalBlastsForDay = new TotalBlastsForDay();
            var sixthTotalBlastsForDay = new TotalBlastsForDay();

            // Act, Assert
            firstTotalBlastsForDay.ShouldNotBeNull();
            secondTotalBlastsForDay.ShouldNotBeNull();
            thirdTotalBlastsForDay.ShouldNotBeNull();
            fourthTotalBlastsForDay.ShouldNotBeNull();
            fifthTotalBlastsForDay.ShouldNotBeNull();
            sixthTotalBlastsForDay.ShouldNotBeNull();
            firstTotalBlastsForDay.ShouldNotBeSameAs(secondTotalBlastsForDay);
            thirdTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            fourthTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            fifthTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            sixthTotalBlastsForDay.ShouldNotBeSameAs(firstTotalBlastsForDay);
            sixthTotalBlastsForDay.ShouldNotBeSameAs(fourthTotalBlastsForDay);
        }

        #endregion

        #endregion

        #endregion
    }
}