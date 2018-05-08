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
    public class CustomerContactTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CustomerContact) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            var contactId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var firstName = Fixture.Create<string>();
            var lastName = Fixture.Create<string>();
            var address = Fixture.Create<string>();
            var address2 = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var zip = Fixture.Create<string>();
            var phone = Fixture.Create<string>();
            var mobile = Fixture.Create<string>();
            var email = Fixture.Create<string>();
            var createdby = Fixture.Create<string>();
            var updatedby = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            customerContact.ContactID = contactId;
            customerContact.CustomerID = customerId;
            customerContact.FirstName = firstName;
            customerContact.LastName = lastName;
            customerContact.Address = address;
            customerContact.Address2 = address2;
            customerContact.City = city;
            customerContact.State = state;
            customerContact.Zip = zip;
            customerContact.Phone = phone;
            customerContact.Mobile = mobile;
            customerContact.Email = email;
            customerContact.Createdby = createdby;
            customerContact.Updatedby = updatedby;
            customerContact.CreatedUserID = createdUserId;
            customerContact.UpdatedUserID = updatedUserId;
            customerContact.IsDeleted = isDeleted;

            // Assert
            customerContact.ContactID.ShouldBe(contactId);
            customerContact.CustomerID.ShouldBe(customerId);
            customerContact.FirstName.ShouldBe(firstName);
            customerContact.LastName.ShouldBe(lastName);
            customerContact.Address.ShouldBe(address);
            customerContact.Address2.ShouldBe(address2);
            customerContact.City.ShouldBe(city);
            customerContact.State.ShouldBe(state);
            customerContact.Zip.ShouldBe(zip);
            customerContact.Phone.ShouldBe(phone);
            customerContact.Mobile.ShouldBe(mobile);
            customerContact.Email.ShouldBe(email);
            customerContact.Createdby.ShouldBe(createdby);
            customerContact.Updatedby.ShouldBe(updatedby);
            customerContact.CreatedUserID.ShouldBe(createdUserId);
            customerContact.CreatedDate.ShouldBeNull();
            customerContact.UpdatedUserID.ShouldBe(updatedUserId);
            customerContact.UpdatedDate.ShouldBeNull();
            customerContact.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Address = Fixture.Create<string>();
            var stringType = customerContact.Address.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Address2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Address2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Address2 = Fixture.Create<string>();
            var stringType = customerContact.Address2.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Address2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_Address2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress2 = "Address2NotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameAddress2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Address2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress2 = "Address2";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameAddress2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.City = Fixture.Create<string>();
            var stringType = customerContact.City.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (ContactID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_ContactID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.ContactID = Fixture.Create<int>();
            var intType = customerContact.ContactID.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (ContactID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_ContactIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactID = "ContactIDNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameContactID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_ContactID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactID = "ContactID";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameContactID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Createdby) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Createdby_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Createdby = Fixture.Create<string>();
            var stringType = customerContact.Createdby.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Createdby) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_CreatedbyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedby = "CreatedbyNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameCreatedby));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Createdby_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedby = "Createdby";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameCreatedby);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customerContact = Fixture.Create<CustomerContact>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerContact.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerContact, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerContact.CreatedUserID = random;

            // Assert
            customerContact.CreatedUserID.ShouldBe(random);
            customerContact.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();    

            // Act , Set
            customerContact.CreatedUserID = null;

            // Assert
            customerContact.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customerContact = Fixture.Create<CustomerContact>();
            var propertyInfo = customerContact.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerContact, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerContact.CreatedUserID.ShouldBeNull();
            customerContact.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.CustomerID = Fixture.Create<int>();
            var intType = customerContact.CustomerID.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Email) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Email_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Email = Fixture.Create<string>();
            var stringType = customerContact.Email.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Email) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_EmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmail = "EmailNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Email_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmail = "Email";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (FirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_FirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.FirstName = Fixture.Create<string>();
            var stringType = customerContact.FirstName.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (FirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_FirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstNameNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_FirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstName";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerContact.IsDeleted = random;

            // Assert
            customerContact.IsDeleted.ShouldBe(random);
            customerContact.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();    

            // Act , Set
            customerContact.IsDeleted = null;

            // Assert
            customerContact.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var customerContact = Fixture.Create<CustomerContact>();
            var propertyInfo = customerContact.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customerContact, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerContact.IsDeleted.ShouldBeNull();
            customerContact.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (LastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_LastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.LastName = Fixture.Create<string>();
            var stringType = customerContact.LastName.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (LastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_LastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastNameNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_LastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastName";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Mobile) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Mobile_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Mobile = Fixture.Create<string>();
            var stringType = customerContact.Mobile.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Mobile) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_MobileNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMobile = "MobileNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameMobile));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Mobile_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMobile = "Mobile";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameMobile);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Phone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Phone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Phone = Fixture.Create<string>();
            var stringType = customerContact.Phone.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Phone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_PhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePhone = "PhoneNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNamePhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Phone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePhone = "Phone";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNamePhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.State = Fixture.Create<string>();
            var stringType = customerContact.State.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Updatedby) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Updatedby_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Updatedby = Fixture.Create<string>();
            var stringType = customerContact.Updatedby.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Updatedby) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_UpdatedbyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedby = "UpdatedbyNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameUpdatedby));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Updatedby_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedby = "Updatedby";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameUpdatedby);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customerContact = Fixture.Create<CustomerContact>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerContact.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerContact, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerContact.UpdatedUserID = random;

            // Assert
            customerContact.UpdatedUserID.ShouldBe(random);
            customerContact.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();    

            // Act , Set
            customerContact.UpdatedUserID = null;

            // Assert
            customerContact.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customerContact = Fixture.Create<CustomerContact>();
            var propertyInfo = customerContact.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerContact, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerContact.UpdatedUserID.ShouldBeNull();
            customerContact.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerContact) => Property (Zip) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Zip_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerContact = Fixture.Create<CustomerContact>();
            customerContact.Zip = Fixture.Create<string>();
            var stringType = customerContact.Zip.GetType();

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

        #region General Getters/Setters : Class (CustomerContact) => Property (Zip) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Class_Invalid_Property_ZipNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameZip = "ZipNotPresent";
            var customerContact  = Fixture.Create<CustomerContact>();

            // Act , Assert
            Should.NotThrow(() => customerContact.GetType().GetProperty(propertyNameZip));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerContact_Zip_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameZip = "Zip";
            var customerContact  = Fixture.Create<CustomerContact>();
            var propertyInfo  = customerContact.GetType().GetProperty(propertyNameZip);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CustomerContact) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerContact_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerContact());
        }

        #endregion

        #region General Constructor : Class (CustomerContact) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerContact_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCustomerContact = Fixture.CreateMany<CustomerContact>(2).ToList();
            var firstCustomerContact = instancesOfCustomerContact.FirstOrDefault();
            var lastCustomerContact = instancesOfCustomerContact.Last();

            // Act, Assert
            firstCustomerContact.ShouldNotBeNull();
            lastCustomerContact.ShouldNotBeNull();
            firstCustomerContact.ShouldNotBeSameAs(lastCustomerContact);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerContact_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCustomerContact = new CustomerContact();
            var secondCustomerContact = new CustomerContact();
            var thirdCustomerContact = new CustomerContact();
            var fourthCustomerContact = new CustomerContact();
            var fifthCustomerContact = new CustomerContact();
            var sixthCustomerContact = new CustomerContact();

            // Act, Assert
            firstCustomerContact.ShouldNotBeNull();
            secondCustomerContact.ShouldNotBeNull();
            thirdCustomerContact.ShouldNotBeNull();
            fourthCustomerContact.ShouldNotBeNull();
            fifthCustomerContact.ShouldNotBeNull();
            sixthCustomerContact.ShouldNotBeNull();
            firstCustomerContact.ShouldNotBeSameAs(secondCustomerContact);
            thirdCustomerContact.ShouldNotBeSameAs(firstCustomerContact);
            fourthCustomerContact.ShouldNotBeSameAs(firstCustomerContact);
            fifthCustomerContact.ShouldNotBeSameAs(firstCustomerContact);
            sixthCustomerContact.ShouldNotBeSameAs(firstCustomerContact);
            sixthCustomerContact.ShouldNotBeSameAs(fourthCustomerContact);
        }

        #endregion

        #region General Constructor : Class (CustomerContact) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerContact_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var contactId = -1;
            var customerId = -1;
            var firstName = string.Empty;
            var lastName = string.Empty;
            var address = string.Empty;
            var address2 = string.Empty;
            var city = string.Empty;
            var state = string.Empty;
            var zip = string.Empty;
            var phone = string.Empty;
            var mobile = string.Empty;
            var email = string.Empty;
            var createdby = string.Empty;
            var updatedby = string.Empty;

            // Act
            var customerContact = new CustomerContact();

            // Assert
            customerContact.ContactID.ShouldBe(contactId);
            customerContact.CustomerID.ShouldBe(customerId);
            customerContact.FirstName.ShouldBe(firstName);
            customerContact.LastName.ShouldBe(lastName);
            customerContact.Address.ShouldBe(address);
            customerContact.Address2.ShouldBe(address2);
            customerContact.City.ShouldBe(city);
            customerContact.State.ShouldBe(state);
            customerContact.Zip.ShouldBe(zip);
            customerContact.Phone.ShouldBe(phone);
            customerContact.Mobile.ShouldBe(mobile);
            customerContact.Email.ShouldBe(email);
            customerContact.Createdby.ShouldBe(createdby);
            customerContact.Updatedby.ShouldBe(updatedby);
            customerContact.CreatedUserID.ShouldBeNull();
            customerContact.CreatedDate.ShouldBeNull();
            customerContact.UpdatedUserID.ShouldBeNull();
            customerContact.UpdatedDate.ShouldBeNull();
            customerContact.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}