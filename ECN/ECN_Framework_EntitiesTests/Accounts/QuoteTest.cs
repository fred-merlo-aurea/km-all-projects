using System;
using System.Collections.Generic;
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
    public class QuoteTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Quote) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            var quoteId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var channelId = Fixture.Create<int>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var approveDate = Fixture.Create<DateTime?>();
            var startDate = Fixture.Create<DateTime?>();
            var salutation = Fixture.Create<string>();
            var firstName = Fixture.Create<string>();
            var lastName = Fixture.Create<string>();
            var email = Fixture.Create<string>();
            var phone = Fixture.Create<string>();
            var fax = Fixture.Create<string>();
            var company = Fixture.Create<string>();
            var billType = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int>();
            var updatedUserId = Fixture.Create<int>();
            var accountManagerId = Fixture.Create<int?>();
            var nBDIDs = Fixture.Create<string>();
            var status = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.QuoteStatusEnum>();
            var notes = Fixture.Create<string>();
            var testUserName = Fixture.Create<string>();
            var testPassword = Fixture.Create<string>();
            var internalNotes = Fixture.Create<string>();
            var isDeleted = Fixture.Create<bool?>();
            var itemList = Fixture.Create<List<QuoteItem>>();

            // Act
            quote.QuoteID = quoteId;
            quote.CustomerID = customerId;
            quote.ChannelID = channelId;
            quote.CreatedDate = createdDate;
            quote.UpdatedDate = updatedDate;
            quote.ApproveDate = approveDate;
            quote.StartDate = startDate;
            quote.Salutation = salutation;
            quote.FirstName = firstName;
            quote.LastName = lastName;
            quote.Email = email;
            quote.Phone = phone;
            quote.Fax = fax;
            quote.Company = company;
            quote.BillType = billType;
            quote.CreatedUserID = createdUserId;
            quote.UpdatedUserID = updatedUserId;
            quote.AccountManagerID = accountManagerId;
            quote.NBDIDs = nBDIDs;
            quote.Status = status;
            quote.Notes = notes;
            quote.TestUserName = testUserName;
            quote.TestPassword = testPassword;
            quote.InternalNotes = internalNotes;
            quote.IsDeleted = isDeleted;
            quote.ItemList = itemList;

            // Assert
            quote.QuoteID.ShouldBe(quoteId);
            quote.CustomerID.ShouldBe(customerId);
            quote.ChannelID.ShouldBe(channelId);
            quote.CreatedDate.ShouldBe(createdDate);
            quote.UpdatedDate.ShouldBe(updatedDate);
            quote.ApproveDate.ShouldBe(approveDate);
            quote.StartDate.ShouldBe(startDate);
            quote.Salutation.ShouldBe(salutation);
            quote.FirstName.ShouldBe(firstName);
            quote.LastName.ShouldBe(lastName);
            quote.Email.ShouldBe(email);
            quote.Phone.ShouldBe(phone);
            quote.Fax.ShouldBe(fax);
            quote.Company.ShouldBe(company);
            quote.BillType.ShouldBe(billType);
            quote.CreatedUserID.ShouldBe(createdUserId);
            quote.UpdatedUserID.ShouldBe(updatedUserId);
            quote.AccountManagerID.ShouldBe(accountManagerId);
            quote.NBDIDs.ShouldBe(nBDIDs);
            quote.Status.ShouldBe(status);
            quote.Notes.ShouldBe(notes);
            quote.TestUserName.ShouldBe(testUserName);
            quote.TestPassword.ShouldBe(testPassword);
            quote.InternalNotes.ShouldBe(internalNotes);
            quote.IsDeleted.ShouldBe(isDeleted);
            quote.ItemList.ShouldBe(itemList);
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (AccountManagerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_AccountManagerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            var random = Fixture.Create<int>();

            // Act , Set
            quote.AccountManagerID = random;

            // Assert
            quote.AccountManagerID.ShouldBe(random);
            quote.AccountManagerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_AccountManagerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();    

            // Act , Set
            quote.AccountManagerID = null;

            // Assert
            quote.AccountManagerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_AccountManagerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAccountManagerID = "AccountManagerID";
            var quote = Fixture.Create<Quote>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameAccountManagerID);

            // Act , Set
            propertyInfo.SetValue(quote, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quote.AccountManagerID.ShouldBeNull();
            quote.AccountManagerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (AccountManagerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_AccountManagerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccountManagerID = "AccountManagerIDNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameAccountManagerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_AccountManagerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccountManagerID = "AccountManagerID";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameAccountManagerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (ApproveDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_ApproveDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameApproveDate = "ApproveDate";
            var quote = Fixture.Create<Quote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameApproveDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quote, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (ApproveDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_ApproveDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameApproveDate = "ApproveDateNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameApproveDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_ApproveDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameApproveDate = "ApproveDate";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameApproveDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (BillType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_BillType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.BillType = Fixture.Create<string>();
            var stringType = quote.BillType.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (BillType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_BillTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBillType = "BillTypeNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameBillType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_BillType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBillType = "BillType";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameBillType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (ChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_ChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.ChannelID = Fixture.Create<int>();
            var intType = quote.ChannelID.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (ChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_ChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelID = "ChannelIDNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_ChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChannelID = "ChannelID";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Company) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Company_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.Company = Fixture.Create<string>();
            var stringType = quote.Company.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (Company) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_CompanyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompany = "CompanyNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameCompany));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Company_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompany = "Company";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameCompany);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var quote = Fixture.Create<Quote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quote, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (CreatedUserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_CreatedUserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.CreatedUserID = Fixture.Create<int>();
            var intType = quote.CreatedUserID.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.CustomerID = Fixture.Create<int>();
            var intType = quote.CustomerID.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Email) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Email_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.Email = Fixture.Create<string>();
            var stringType = quote.Email.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (Email) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_EmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmail = "EmailNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Email_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmail = "Email";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Fax) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Fax_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.Fax = Fixture.Create<string>();
            var stringType = quote.Fax.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (Fax) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_FaxNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFax = "FaxNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameFax));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Fax_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFax = "Fax";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameFax);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (FirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_FirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.FirstName = Fixture.Create<string>();
            var stringType = quote.FirstName.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (FirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_FirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstNameNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_FirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstName";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (InternalNotes) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_InternalNotes_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.InternalNotes = Fixture.Create<string>();
            var stringType = quote.InternalNotes.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (InternalNotes) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_InternalNotesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameInternalNotes = "InternalNotesNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameInternalNotes));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_InternalNotes_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameInternalNotes = "InternalNotes";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameInternalNotes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quote.IsDeleted = random;

            // Assert
            quote.IsDeleted.ShouldBe(random);
            quote.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();    

            // Act , Set
            quote.IsDeleted = null;

            // Assert
            quote.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var quote = Fixture.Create<Quote>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(quote, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quote.IsDeleted.ShouldBeNull();
            quote.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (ItemList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_ItemListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameItemList = "ItemListNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameItemList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_ItemList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameItemList = "ItemList";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameItemList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (LastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_LastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.LastName = Fixture.Create<string>();
            var stringType = quote.LastName.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (LastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_LastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastNameNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_LastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastName";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (NBDIDs) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_NBDIDs_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.NBDIDs = Fixture.Create<string>();
            var stringType = quote.NBDIDs.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (NBDIDs) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_NBDIDsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNBDIDs = "NBDIDsNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameNBDIDs));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_NBDIDs_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNBDIDs = "NBDIDs";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameNBDIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Notes) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Notes_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.Notes = Fixture.Create<string>();
            var stringType = quote.Notes.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (Notes) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_NotesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNotes = "NotesNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameNotes));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Notes_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNotes = "Notes";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameNotes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Phone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Phone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.Phone = Fixture.Create<string>();
            var stringType = quote.Phone.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (Phone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_PhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePhone = "PhoneNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNamePhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Phone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePhone = "Phone";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNamePhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (QuoteID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_QuoteID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.QuoteID = Fixture.Create<int>();
            var intType = quote.QuoteID.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (QuoteID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_QuoteIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuoteID = "QuoteIDNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameQuoteID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_QuoteID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuoteID = "QuoteID";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameQuoteID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Salutation) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Salutation_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.Salutation = Fixture.Create<string>();
            var stringType = quote.Salutation.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (Salutation) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_SalutationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSalutation = "SalutationNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameSalutation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Salutation_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSalutation = "Salutation";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameSalutation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (StartDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_StartDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var quote = Fixture.Create<Quote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameStartDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quote, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (StartDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_StartDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDateNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameStartDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_StartDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartDate = "StartDate";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameStartDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Status) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Status_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var quote = Fixture.Create<Quote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameStatus);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quote, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (TestPassword) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_TestPassword_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.TestPassword = Fixture.Create<string>();
            var stringType = quote.TestPassword.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (TestPassword) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_TestPasswordNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTestPassword = "TestPasswordNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameTestPassword));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_TestPassword_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTestPassword = "TestPassword";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameTestPassword);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (TestUserName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_TestUserName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.TestUserName = Fixture.Create<string>();
            var stringType = quote.TestUserName.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (TestUserName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_TestUserNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTestUserName = "TestUserNameNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameTestUserName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_TestUserName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTestUserName = "TestUserName";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameTestUserName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var quote = Fixture.Create<Quote>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quote.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quote, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Quote) => Property (UpdatedUserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_UpdatedUserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quote = Fixture.Create<Quote>();
            quote.UpdatedUserID = Fixture.Create<int>();
            var intType = quote.UpdatedUserID.GetType();

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

        #region General Getters/Setters : Class (Quote) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var quote  = Fixture.Create<Quote>();

            // Act , Assert
            Should.NotThrow(() => quote.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Quote_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var quote  = Fixture.Create<Quote>();
            var propertyInfo  = quote.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Quote) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Quote_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Quote());
        }

        #endregion

        #region General Constructor : Class (Quote) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Quote_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfQuote = Fixture.CreateMany<Quote>(2).ToList();
            var firstQuote = instancesOfQuote.FirstOrDefault();
            var lastQuote = instancesOfQuote.Last();

            // Act, Assert
            firstQuote.ShouldNotBeNull();
            lastQuote.ShouldNotBeNull();
            firstQuote.ShouldNotBeSameAs(lastQuote);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Quote_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstQuote = new Quote();
            var secondQuote = new Quote();
            var thirdQuote = new Quote();
            var fourthQuote = new Quote();
            var fifthQuote = new Quote();
            var sixthQuote = new Quote();

            // Act, Assert
            firstQuote.ShouldNotBeNull();
            secondQuote.ShouldNotBeNull();
            thirdQuote.ShouldNotBeNull();
            fourthQuote.ShouldNotBeNull();
            fifthQuote.ShouldNotBeNull();
            sixthQuote.ShouldNotBeNull();
            firstQuote.ShouldNotBeSameAs(secondQuote);
            thirdQuote.ShouldNotBeSameAs(firstQuote);
            fourthQuote.ShouldNotBeSameAs(firstQuote);
            fifthQuote.ShouldNotBeSameAs(firstQuote);
            sixthQuote.ShouldNotBeSameAs(firstQuote);
            sixthQuote.ShouldNotBeSameAs(fourthQuote);
        }

        #endregion

        #region General Constructor : Class (Quote) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Quote_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var quoteId = -1;
            var customerId = -1;
            var channelId = -1;
            var salutation = string.Empty;
            var firstName = string.Empty;
            var lastName = string.Empty;
            var email = string.Empty;
            var phone = string.Empty;
            var fax = string.Empty;
            var company = string.Empty;
            var billType = string.Empty;
            var createdUserId = -1;
            var updatedUserId = -1;
            var nBDIDs = string.Empty;
            var status = ECN_Framework_Common.Objects.Accounts.Enums.QuoteStatusEnum.Pending;
            var notes = string.Empty;
            var testUserName = string.Empty;
            var testPassword = string.Empty;
            var internalNotes = string.Empty;

            // Act
            var quote = new Quote();

            // Assert
            quote.QuoteID.ShouldBe(quoteId);
            quote.CustomerID.ShouldBe(customerId);
            quote.ChannelID.ShouldBe(channelId);
            quote.CreatedDate.ShouldBeNull();
            quote.UpdatedDate.ShouldBeNull();
            quote.ApproveDate.ShouldBeNull();
            quote.StartDate.ShouldBeNull();
            quote.Salutation.ShouldBe(salutation);
            quote.FirstName.ShouldBe(firstName);
            quote.LastName.ShouldBe(lastName);
            quote.Email.ShouldBe(email);
            quote.Phone.ShouldBe(phone);
            quote.Fax.ShouldBe(fax);
            quote.Company.ShouldBe(company);
            quote.BillType.ShouldBe(billType);
            quote.CreatedUserID.ShouldBe(createdUserId);
            quote.UpdatedUserID.ShouldBe(updatedUserId);
            quote.AccountManagerID.ShouldBeNull();
            quote.NBDIDs.ShouldBe(nBDIDs);
            quote.Status.ShouldBe(status);
            quote.Notes.ShouldBe(notes);
            quote.TestUserName.ShouldBe(testUserName);
            quote.TestPassword.ShouldBe(testPassword);
            quote.InternalNotes.ShouldBe(internalNotes);
            quote.IsDeleted.ShouldBeNull();
            quote.ItemList.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}