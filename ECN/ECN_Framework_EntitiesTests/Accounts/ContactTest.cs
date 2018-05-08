using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class ContactTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var contact  = new Contact();
            var billingContactID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var salutation = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.Salutation>();
            var contactName = Fixture.Create<string>();
            var firstName = Fixture.Create<string>();
            var lastName = Fixture.Create<string>();
            var contactTitle = Fixture.Create<string>();
            var phone = Fixture.Create<string>();
            var fax = Fixture.Create<string>();
            var email = Fixture.Create<string>();
            var streetAddress = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var country = Fixture.Create<string>();
            var zip = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            contact.BillingContactID = billingContactID;
            contact.CustomerID = customerID;
            contact.Salutation = salutation;
            contact.ContactName = contactName;
            contact.FirstName = firstName;
            contact.LastName = lastName;
            contact.ContactTitle = contactTitle;
            contact.Phone = phone;
            contact.Fax = fax;
            contact.Email = email;
            contact.StreetAddress = streetAddress;
            contact.City = city;
            contact.State = state;
            contact.Country = country;
            contact.Zip = zip;
            contact.CreatedUserID = createdUserID;
            contact.UpdatedUserID = updatedUserID;
            contact.IsDeleted = isDeleted;

            // Assert
            contact.BillingContactID.ShouldBe(billingContactID);
            contact.CustomerID.ShouldBe(customerID);
            contact.Salutation.ShouldBe(salutation);
            contact.ContactName.ShouldBe(contactName);
            contact.FirstName.ShouldBe(firstName);
            contact.LastName.ShouldBe(lastName);
            contact.ContactTitle.ShouldBe(contactTitle);
            contact.Phone.ShouldBe(phone);
            contact.Fax.ShouldBe(fax);
            contact.Email.ShouldBe(email);
            contact.StreetAddress.ShouldBe(streetAddress);
            contact.City.ShouldBe(city);
            contact.State.ShouldBe(state);
            contact.Country.ShouldBe(country);
            contact.Zip.ShouldBe(zip);
            contact.CreatedUserID.ShouldBe(createdUserID);
            contact.UpdatedUserID.ShouldBe(updatedUserID);
            contact.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : Contact => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var random = Fixture.Create<bool>();

            // Act , Set
            contact.IsDeleted = random;

            // Assert
            contact.IsDeleted.ShouldBe(random);
            contact.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();    

            // Act , Set
            contact.IsDeleted = null;

            // Assert
            contact.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var contact = Fixture.Create<Contact>();
            var propertyInfo = contact.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(contact, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contact.IsDeleted.ShouldBeNull();
            contact.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Contact => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var contact = Fixture.Create<Contact>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contact.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contact, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Contact => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var contact = Fixture.Create<Contact>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contact.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contact, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Contact => Salutation

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Salutation_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constSalutation = "Salutation";
            var contact = Fixture.Create<Contact>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = contact.GetType().GetProperty(constSalutation);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(contact, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_Salutation_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constSalutation = "Salutation";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constSalutation));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Salutation_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constSalutation = "Salutation";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constSalutation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Contact => BillingContactID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillingContactID_Int_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var intType = contact.BillingContactID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_BillingContactID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBillingContactID = "BillingContactID";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constBillingContactID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BillingContactID_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constBillingContactID = "BillingContactID";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constBillingContactID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Contact => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Int_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var intType = contact.CustomerID.GetType();

            // Act
            var isTypeInt = typeof(int).Equals(intType);    
            var isTypeNullableInt = typeof(int?).Equals(intType);
            var isTypeString = typeof(string).Equals(intType);
            var isTypeDecimal = typeof(decimal).Equals(intType);
            var isTypeLong = typeof(long).Equals(intType);
            var isTypeBool = typeof(bool).Equals(intType);
            var isTypeDouble = typeof(double).Equals(intType);
            var isTypeFloat = typeof(float).Equals(intType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(intType);
            var isTypeLongNullable = typeof(long?).Equals(intType);
            var isTypeBoolNullable = typeof(bool?).Equals(intType);
            var isTypeDoubleNullable = typeof(double?).Equals(intType);
            var isTypeFloatNullable = typeof(float?).Equals(intType);


            // Assert
            isTypeInt.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Contact => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var random = Fixture.Create<int>();

            // Act , Set
            contact.CreatedUserID = random;

            // Assert
            contact.CreatedUserID.ShouldBe(random);
            contact.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();    

            // Act , Set
            contact.CreatedUserID = null;

            // Assert
            contact.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var contact = Fixture.Create<Contact>();
            var propertyInfo = contact.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(contact, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contact.CreatedUserID.ShouldBeNull();
            contact.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Contact => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var random = Fixture.Create<int>();

            // Act , Set
            contact.UpdatedUserID = random;

            // Assert
            contact.UpdatedUserID.ShouldBe(random);
            contact.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();    

            // Act , Set
            contact.UpdatedUserID = null;

            // Assert
            contact.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var contact = Fixture.Create<Contact>();
            var propertyInfo = contact.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(contact, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            contact.UpdatedUserID.ShouldBeNull();
            contact.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => City

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_City_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.City.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_City_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCity = "City";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constCity));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_City_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constCity = "City";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => ContactName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ContactName_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.ContactName.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_ContactName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constContactName = "ContactName";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constContactName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ContactName_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constContactName = "ContactName";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constContactName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => ContactTitle

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ContactTitle_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.ContactTitle.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_ContactTitle_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constContactTitle = "ContactTitle";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constContactTitle));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ContactTitle_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constContactTitle = "ContactTitle";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constContactTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => Country

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Country_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.Country.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_Country_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCountry = "Country";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constCountry));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Country_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constCountry = "Country";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constCountry);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => Email

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Email_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.Email.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_Email_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constEmail = "Email";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constEmail));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Email_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constEmail = "Email";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => Fax

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Fax_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.Fax.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_Fax_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constFax = "Fax";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constFax));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Fax_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constFax = "Fax";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constFax);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => FirstName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_FirstName_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.FirstName.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_FirstName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constFirstName = "FirstName";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constFirstName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_FirstName_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constFirstName = "FirstName";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => LastName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LastName_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.LastName.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_LastName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLastName = "LastName";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constLastName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LastName_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constLastName = "LastName";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => Phone

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Phone_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.Phone.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_Phone_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constPhone = "Phone";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constPhone));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Phone_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constPhone = "Phone";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constPhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => State

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_State_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.State.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_State_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constState = "State";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constState));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_State_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constState = "State";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => StreetAddress

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StreetAddress_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.StreetAddress.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_StreetAddress_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constStreetAddress = "StreetAddress";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constStreetAddress));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_StreetAddress_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constStreetAddress = "StreetAddress";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constStreetAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Contact => Zip

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Zip_String_Type_Verify_Test()
        {
            // Arrange
            var contact = Fixture.Create<Contact>();
            var stringType = contact.Zip.GetType();

            // Act
            var isTypeString = typeof(string).Equals(stringType);    
            var isTypeInt = typeof(int).Equals(stringType);
            var isTypeDecimal = typeof(decimal).Equals(stringType);
            var isTypeLong = typeof(long).Equals(stringType);
            var isTypeBool = typeof(bool).Equals(stringType);
            var isTypeDouble = typeof(double).Equals(stringType);
            var isTypeFloat = typeof(float).Equals(stringType);

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Contact_Class_Invalid_Property_Zip_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constZip = "Zip";
            var contact  = Fixture.Create<Contact>();

            // Act , Assert
            Should.NotThrow(() => contact.GetType().GetProperty(constZip));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Zip_Is_Present_In_Contact_Class_As_Public_Test()
        {
            // Arrange
            const string constZip = "Zip";
            var contact  = Fixture.Create<Contact>();
            var propertyInfo  = contact.GetType().GetProperty(constZip);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region General Category : General

        #region Category : Contructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Contact());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Contact>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Check constructor creation by throwing or not throwing exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Parameter_Should_Not_Throw_Exception()
        {
            // Arrange or Act
            Should.NotThrow(() => new Contact());
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Parameter_No_Exception_Thrown()
        {
            // Arrange or Act
            Should.NotThrow(() => new Contact());
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var billingContactID = -1;
            var customerID = -1;
            var salutation = ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
            var contactName = string.Empty;
            var firstName = string.Empty;
            var lastName = string.Empty;
            var contactTitle = string.Empty;
            var phone = string.Empty;
            var fax = string.Empty;
            var email = string.Empty;
            var streetAddress = string.Empty;
            var city = string.Empty;
            var state = string.Empty;
            var country = string.Empty;
            var zip = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var contact = new Contact();    

            // Assert
            contact.BillingContactID.ShouldBe(billingContactID);
            contact.CustomerID.ShouldBe(customerID);
            contact.Salutation.ShouldBe(salutation);
            contact.ContactName.ShouldBe(contactName);
            contact.FirstName.ShouldBe(firstName);
            contact.LastName.ShouldBe(lastName);
            contact.ContactTitle.ShouldBe(contactTitle);
            contact.Phone.ShouldBe(phone);
            contact.Fax.ShouldBe(fax);
            contact.Email.ShouldBe(email);
            contact.StreetAddress.ShouldBe(streetAddress);
            contact.City.ShouldBe(city);
            contact.State.ShouldBe(state);
            contact.Country.ShouldBe(country);
            contact.Zip.ShouldBe(zip);
            contact.CreatedUserID.ShouldBeNull();
            contact.CreatedDate.ShouldBeNull();
            contact.UpdatedUserID.ShouldBeNull();
            contact.UpdatedDate.ShouldBeNull();
            contact.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}