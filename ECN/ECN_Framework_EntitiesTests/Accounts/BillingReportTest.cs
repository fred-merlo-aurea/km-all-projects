using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
{
    [TestFixture]
    public class BillingReportTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BillingReport) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var billingReportId = Fixture.Create<int>();
            var billingReportName = Fixture.Create<string>();
            var includeMasterFile = Fixture.Create<bool>();
            var includeFulfillment = Fixture.Create<bool>();
            var startDate = Fixture.Create<DateTime?>();
            var endDate = Fixture.Create<DateTime?>();
            var isRecurring = Fixture.Create<bool>();
            var recurrenceType = Fixture.Create<string>();
            var emailBillingRate = Fixture.Create<double>();
            var masterFileRate = Fixture.Create<double?>();
            var fulfillmentRate = Fixture.Create<double?>();
            var fromEmail = Fixture.Create<string>();
            var fromName = Fixture.Create<string>();
            var toEmail = Fixture.Create<string>();
            var subject = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var customerIDs = Fixture.Create<string>();
            var baseChannelId = Fixture.Create<int?>();
            var allCustomers = Fixture.Create<bool?>();
            var blastFields = Fixture.Create<string>();

            // Act
            billingReport.BillingReportID = billingReportId;
            billingReport.BillingReportName = billingReportName;
            billingReport.IncludeMasterFile = includeMasterFile;
            billingReport.IncludeFulfillment = includeFulfillment;
            billingReport.StartDate = startDate;
            billingReport.EndDate = endDate;
            billingReport.IsRecurring = isRecurring;
            billingReport.RecurrenceType = recurrenceType;
            billingReport.EmailBillingRate = emailBillingRate;
            billingReport.MasterFileRate = masterFileRate;
            billingReport.FulfillmentRate = fulfillmentRate;
            billingReport.FromEmail = fromEmail;
            billingReport.FromName = fromName;
            billingReport.ToEmail = toEmail;
            billingReport.Subject = subject;
            billingReport.IsDeleted = isDeleted;
            billingReport.CreatedUserID = createdUserId;
            billingReport.CreatedDate = createdDate;
            billingReport.UpdatedUserID = updatedUserId;
            billingReport.UpdatedDate = updatedDate;
            billingReport.CustomerIDs = customerIDs;
            billingReport.BaseChannelID = baseChannelId;
            billingReport.AllCustomers = allCustomers;
            billingReport.BlastFields = blastFields;

            // Assert
            billingReport.BillingReportID.ShouldBe(billingReportId);
            billingReport.BillingReportName.ShouldBe(billingReportName);
            billingReport.IncludeMasterFile.ShouldBe(includeMasterFile);
            billingReport.IncludeFulfillment.ShouldBe(includeFulfillment);
            billingReport.StartDate.ShouldBe(startDate);
            billingReport.EndDate.ShouldBe(endDate);
            billingReport.IsRecurring.ShouldBe(isRecurring);
            billingReport.RecurrenceType.ShouldBe(recurrenceType);
            billingReport.EmailBillingRate.ShouldBe(emailBillingRate);
            billingReport.MasterFileRate.ShouldBe(masterFileRate);
            billingReport.FulfillmentRate.ShouldBe(fulfillmentRate);
            billingReport.FromEmail.ShouldBe(fromEmail);
            billingReport.FromName.ShouldBe(fromName);
            billingReport.ToEmail.ShouldBe(toEmail);
            billingReport.Subject.ShouldBe(subject);
            billingReport.IsDeleted.ShouldBe(isDeleted);
            billingReport.CreatedUserID.ShouldBe(createdUserId);
            billingReport.CreatedDate.ShouldBe(createdDate);
            billingReport.UpdatedUserID.ShouldBe(updatedUserId);
            billingReport.UpdatedDate.ShouldBe(updatedDate);
            billingReport.CustomerIDs.ShouldBe(customerIDs);
            billingReport.BaseChannelID.ShouldBe(baseChannelId);
            billingReport.AllCustomers.ShouldBe(allCustomers);
            billingReport.BlastFields.ShouldBe(blastFields);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (AllCustomers) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_AllCustomers_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var random = Fixture.Create<bool>();

            // Act , Set
            billingReport.AllCustomers = random;

            // Assert
            billingReport.AllCustomers.ShouldBe(random);
            billingReport.AllCustomers.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_AllCustomers_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();    

            // Act , Set
            billingReport.AllCustomers = null;

            // Assert
            billingReport.AllCustomers.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_AllCustomers_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAllCustomers = "AllCustomers";
            var billingReport = Fixture.Create<BillingReport>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameAllCustomers);

            // Act , Set
            propertyInfo.SetValue(billingReport, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReport.AllCustomers.ShouldBeNull();
            billingReport.AllCustomers.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (AllCustomers) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_AllCustomersNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAllCustomers = "AllCustomersNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameAllCustomers));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_AllCustomers_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAllCustomers = "AllCustomers";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameAllCustomers);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReport.BaseChannelID = random;

            // Assert
            billingReport.BaseChannelID.ShouldBe(random);
            billingReport.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();    

            // Act , Set
            billingReport.BaseChannelID = null;

            // Assert
            billingReport.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var billingReport = Fixture.Create<BillingReport>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(billingReport, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReport.BaseChannelID.ShouldBeNull();
            billingReport.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (BillingReportID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BillingReportID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.BillingReportID = Fixture.Create<int>();
            var intType = billingReport.BillingReportID.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (BillingReportID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_BillingReportIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBillingReportID = "BillingReportIDNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameBillingReportID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BillingReportID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBillingReportID = "BillingReportID";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameBillingReportID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (BillingReportName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BillingReportName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.BillingReportName = Fixture.Create<string>();
            var stringType = billingReport.BillingReportName.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (BillingReportName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_BillingReportNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBillingReportName = "BillingReportNameNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameBillingReportName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BillingReportName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBillingReportName = "BillingReportName";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameBillingReportName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (BlastFields) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BlastFields_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.BlastFields = Fixture.Create<string>();
            var stringType = billingReport.BlastFields.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (BlastFields) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_BlastFieldsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastFields = "BlastFieldsNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameBlastFields));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_BlastFields_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastFields = "BlastFields";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameBlastFields);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var billingReport = Fixture.Create<BillingReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReport.CreatedUserID = random;

            // Assert
            billingReport.CreatedUserID.ShouldBe(random);
            billingReport.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();    

            // Act , Set
            billingReport.CreatedUserID = null;

            // Assert
            billingReport.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var billingReport = Fixture.Create<BillingReport>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(billingReport, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReport.CreatedUserID.ShouldBeNull();
            billingReport.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (CustomerIDs) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CustomerIDs_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.CustomerIDs = Fixture.Create<string>();
            var stringType = billingReport.CustomerIDs.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (CustomerIDs) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_CustomerIDsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerIDs = "CustomerIDsNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameCustomerIDs));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_CustomerIDs_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerIDs = "CustomerIDs";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameCustomerIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (EmailBillingRate) (Type : double) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_EmailBillingRate_Property_Double_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.EmailBillingRate = Fixture.Create<double>();
            var doubleType = billingReport.EmailBillingRate.GetType();

            // Act
            var isTypeDouble = typeof(double) == (doubleType);
            var isTypeNullableDouble = typeof(double?) == (doubleType);
            var isTypeString = typeof(string) == (doubleType);
            var isTypeInt = typeof(int) == (doubleType);
            var isTypeDecimal = typeof(decimal) == (doubleType);
            var isTypeLong = typeof(long) == (doubleType);
            var isTypeBool = typeof(bool) == (doubleType);
            var isTypeFloat = typeof(float) == (doubleType);
            var isTypeIntNullable = typeof(int?) == (doubleType);
            var isTypeDecimalNullable = typeof(decimal?) == (doubleType);
            var isTypeLongNullable = typeof(long?) == (doubleType);
            var isTypeBoolNullable = typeof(bool?) == (doubleType);
            var isTypeFloatNullable = typeof(float?) == (doubleType);

            // Assert
            isTypeDouble.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDouble.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (EmailBillingRate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_EmailBillingRateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailBillingRate = "EmailBillingRateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameEmailBillingRate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_EmailBillingRate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailBillingRate = "EmailBillingRate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameEmailBillingRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (EndDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_EndDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDate";
            var billingReport = Fixture.Create<BillingReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameEndDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (EndDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_EndDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameEndDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_EndDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEndDate = "EndDate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameEndDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (FromEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FromEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.FromEmail = Fixture.Create<string>();
            var stringType = billingReport.FromEmail.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (FromEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_FromEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmailNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameFromEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FromEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromEmail = "FromEmail";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameFromEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (FromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.FromName = Fixture.Create<string>();
            var stringType = billingReport.FromName.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (FromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_FromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromNameNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromName";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (FulfillmentRate) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FulfillmentRate_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var random = Fixture.Create<double>();

            // Act , Set
            billingReport.FulfillmentRate = random;

            // Assert
            billingReport.FulfillmentRate.ShouldBe(random);
            billingReport.FulfillmentRate.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FulfillmentRate_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();    

            // Act , Set
            billingReport.FulfillmentRate = null;

            // Assert
            billingReport.FulfillmentRate.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FulfillmentRate_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameFulfillmentRate = "FulfillmentRate";
            var billingReport = Fixture.Create<BillingReport>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameFulfillmentRate);

            // Act , Set
            propertyInfo.SetValue(billingReport, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReport.FulfillmentRate.ShouldBeNull();
            billingReport.FulfillmentRate.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (FulfillmentRate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_FulfillmentRateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFulfillmentRate = "FulfillmentRateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameFulfillmentRate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_FulfillmentRate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFulfillmentRate = "FulfillmentRate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameFulfillmentRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (IncludeFulfillment) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IncludeFulfillment_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.IncludeFulfillment = Fixture.Create<bool>();
            var boolType = billingReport.IncludeFulfillment.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (IncludeFulfillment) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_IncludeFulfillmentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIncludeFulfillment = "IncludeFulfillmentNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameIncludeFulfillment));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IncludeFulfillment_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIncludeFulfillment = "IncludeFulfillment";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameIncludeFulfillment);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (IncludeMasterFile) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IncludeMasterFile_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.IncludeMasterFile = Fixture.Create<bool>();
            var boolType = billingReport.IncludeMasterFile.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (IncludeMasterFile) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_IncludeMasterFileNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIncludeMasterFile = "IncludeMasterFileNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameIncludeMasterFile));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IncludeMasterFile_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIncludeMasterFile = "IncludeMasterFile";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameIncludeMasterFile);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.IsDeleted = Fixture.Create<bool>();
            var boolType = billingReport.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (IsRecurring) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IsRecurring_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.IsRecurring = Fixture.Create<bool>();
            var boolType = billingReport.IsRecurring.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (IsRecurring) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_IsRecurringNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsRecurring = "IsRecurringNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameIsRecurring));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_IsRecurring_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsRecurring = "IsRecurring";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameIsRecurring);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (MasterFileRate) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_MasterFileRate_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var random = Fixture.Create<double>();

            // Act , Set
            billingReport.MasterFileRate = random;

            // Assert
            billingReport.MasterFileRate.ShouldBe(random);
            billingReport.MasterFileRate.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_MasterFileRate_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();    

            // Act , Set
            billingReport.MasterFileRate = null;

            // Assert
            billingReport.MasterFileRate.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_MasterFileRate_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMasterFileRate = "MasterFileRate";
            var billingReport = Fixture.Create<BillingReport>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameMasterFileRate);

            // Act , Set
            propertyInfo.SetValue(billingReport, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReport.MasterFileRate.ShouldBeNull();
            billingReport.MasterFileRate.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (MasterFileRate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_MasterFileRateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMasterFileRate = "MasterFileRateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameMasterFileRate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_MasterFileRate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMasterFileRate = "MasterFileRate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameMasterFileRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (RecurrenceType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_RecurrenceType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.RecurrenceType = Fixture.Create<string>();
            var stringType = billingReport.RecurrenceType.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (RecurrenceType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_RecurrenceTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRecurrenceType = "RecurrenceTypeNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameRecurrenceType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_RecurrenceType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRecurrenceType = "RecurrenceType";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameRecurrenceType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (StartDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_StartDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var billingReport = Fixture.Create<BillingReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameStartDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (StartDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_StartDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameStartDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_StartDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (Subject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Subject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.Subject = Fixture.Create<string>();
            var stringType = billingReport.Subject.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (Subject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_SubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubject = "SubjectNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Subject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubject = "Subject";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (ToEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_ToEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            billingReport.ToEmail = Fixture.Create<string>();
            var stringType = billingReport.ToEmail.GetType();

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

        #region General Getters/Setters : Class (BillingReport) => Property (ToEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_ToEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameToEmail = "ToEmailNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameToEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_ToEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameToEmail = "ToEmail";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameToEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var billingReport = Fixture.Create<BillingReport>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(billingReport, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();
            var random = Fixture.Create<int>();

            // Act , Set
            billingReport.UpdatedUserID = random;

            // Assert
            billingReport.UpdatedUserID.ShouldBe(random);
            billingReport.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var billingReport = Fixture.Create<BillingReport>();    

            // Act , Set
            billingReport.UpdatedUserID = null;

            // Assert
            billingReport.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var billingReport = Fixture.Create<BillingReport>();
            var propertyInfo = billingReport.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(billingReport, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            billingReport.UpdatedUserID.ShouldBeNull();
            billingReport.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BillingReport) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var billingReport  = Fixture.Create<BillingReport>();

            // Act , Assert
            Should.NotThrow(() => billingReport.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BillingReport_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var billingReport  = Fixture.Create<BillingReport>();
            var propertyInfo  = billingReport.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BillingReport) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReport_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BillingReport());
        }

        #endregion

        #region General Constructor : Class (BillingReport) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReport_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBillingReport = Fixture.CreateMany<BillingReport>(2).ToList();
            var firstBillingReport = instancesOfBillingReport.FirstOrDefault();
            var lastBillingReport = instancesOfBillingReport.Last();

            // Act, Assert
            firstBillingReport.ShouldNotBeNull();
            lastBillingReport.ShouldNotBeNull();
            firstBillingReport.ShouldNotBeSameAs(lastBillingReport);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReport_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBillingReport = new BillingReport();
            var secondBillingReport = new BillingReport();
            var thirdBillingReport = new BillingReport();
            var fourthBillingReport = new BillingReport();
            var fifthBillingReport = new BillingReport();
            var sixthBillingReport = new BillingReport();

            // Act, Assert
            firstBillingReport.ShouldNotBeNull();
            secondBillingReport.ShouldNotBeNull();
            thirdBillingReport.ShouldNotBeNull();
            fourthBillingReport.ShouldNotBeNull();
            fifthBillingReport.ShouldNotBeNull();
            sixthBillingReport.ShouldNotBeNull();
            firstBillingReport.ShouldNotBeSameAs(secondBillingReport);
            thirdBillingReport.ShouldNotBeSameAs(firstBillingReport);
            fourthBillingReport.ShouldNotBeSameAs(firstBillingReport);
            fifthBillingReport.ShouldNotBeSameAs(firstBillingReport);
            sixthBillingReport.ShouldNotBeSameAs(firstBillingReport);
            sixthBillingReport.ShouldNotBeSameAs(fourthBillingReport);
        }

        #endregion

        #region General Constructor : Class (BillingReport) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BillingReport_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var billingReportId = -1;
            var billingReportName = string.Empty;
            var includeFulfillment = false;
            var includeMasterFile = false;
            var isRecurring = false;
            var recurrenceType = string.Empty;
            var emailBillingRate = 0.0;
            var fromEmail = string.Empty;
            var toEmail = string.Empty;
            var fromName = string.Empty;
            var subject = string.Empty;
            var isDeleted = false;
            var customerIDs = string.Empty;
            var baseChannelId = -1;
            var allCustomers = false;
            var blastFields = string.Empty;

            // Act
            var billingReport = new BillingReport();

            // Assert
            billingReport.BillingReportID.ShouldBe(billingReportId);
            billingReport.BillingReportName.ShouldBe(billingReportName);
            billingReport.IncludeFulfillment.ShouldBe(includeFulfillment);
            billingReport.IncludeMasterFile.ShouldBe(includeMasterFile);
            billingReport.StartDate.ShouldBeNull();
            billingReport.EndDate.ShouldBeNull();
            billingReport.IsRecurring.ShouldBe(isRecurring);
            billingReport.RecurrenceType.ShouldBe(recurrenceType);
            billingReport.EmailBillingRate.ShouldBe(emailBillingRate);
            billingReport.MasterFileRate.ShouldBeNull();
            billingReport.FulfillmentRate.ShouldBeNull();
            billingReport.FromEmail.ShouldBe(fromEmail);
            billingReport.ToEmail.ShouldBe(toEmail);
            billingReport.FromName.ShouldBe(fromName);
            billingReport.Subject.ShouldBe(subject);
            billingReport.IsDeleted.ShouldBe(isDeleted);
            billingReport.CreatedDate.ShouldBeNull();
            billingReport.CreatedUserID.ShouldBeNull();
            billingReport.UpdatedDate.ShouldBeNull();
            billingReport.UpdatedUserID.ShouldBeNull();
            billingReport.CustomerIDs.ShouldBe(customerIDs);
            billingReport.BaseChannelID.ShouldBe(baseChannelId);
            billingReport.AllCustomers.ShouldBe(allCustomers);
            billingReport.BlastFields.ShouldBe(blastFields);
        }

        #endregion

        #endregion

        #endregion
    }
}