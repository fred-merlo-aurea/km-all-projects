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
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_Entities.Accounts
{
    [TestFixture]
    public class CustomerTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Customer) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var customerId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int?>();
            var communicatorChannelId = Fixture.Create<int?>();
            var collectorChannelId = Fixture.Create<int?>();
            var creatorChannelId = Fixture.Create<int?>();
            var publisherChannelId = Fixture.Create<int?>();
            var charityChannelId = Fixture.Create<int?>();
            var customerName = Fixture.Create<string>();
            var activeFlag = Fixture.Create<string>();
            var communicatorLevel = Fixture.Create<string>();
            var collectorLevel = Fixture.Create<string>();
            var creatorLevel = Fixture.Create<string>();
            var publisherLevel = Fixture.Create<string>();
            var charityLevel = Fixture.Create<string>();
            var accountsLevel = Fixture.Create<string>();
            var address = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var country = Fixture.Create<string>();
            var zip = Fixture.Create<string>();
            var salutation = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.Salutation>();
            var contactName = Fixture.Create<string>();
            var firstName = Fixture.Create<string>();
            var lastName = Fixture.Create<string>();
            var contactTitle = Fixture.Create<string>();
            var phone = Fixture.Create<string>();
            var fax = Fixture.Create<string>();
            var email = Fixture.Create<string>();
            var webAddress = Fixture.Create<string>();
            var techContact = Fixture.Create<string>();
            var techEmail = Fixture.Create<string>();
            var techPhone = Fixture.Create<string>();
            var subscriptionsEmail = Fixture.Create<string>();
            var customerType = Fixture.Create<string>();
            var demoFlag = Fixture.Create<string>();
            var accountExecutiveId = Fixture.Create<int?>();
            var accountManagerId = Fixture.Create<int?>();
            var isStrategic = Fixture.Create<bool?>();
            var customer_udf1 = Fixture.Create<string>();
            var customer_udf2 = Fixture.Create<string>();
            var customer_udf3 = Fixture.Create<string>();
            var customer_udf4 = Fixture.Create<string>();
            var customer_udf5 = Fixture.Create<string>();
            var blastConfigId = Fixture.Create<int?>();
            var bounceThreshold = Fixture.Create<int?>();
            var softBounceThreshold = Fixture.Create<int?>();
            var textPowerKWD = Fixture.Create<string>();
            var textPowerWelcomeMsg = Fixture.Create<string>();
            var aBWinnerType = Fixture.Create<string>();
            var billingContact = Fixture.Create<Contact>();
            var generalContant = Fixture.Create<Contact>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();
            var mSCustomerId = Fixture.Create<int?>();
            var defaultBlastAsTest = Fixture.Create<bool?>();
            var testBlastLimit = Fixture.Create<int?>();
            var platformClientId = Fixture.Create<int>();

            // Act
            customer.CustomerID = customerId;
            customer.BaseChannelID = baseChannelId;
            customer.CommunicatorChannelID = communicatorChannelId;
            customer.CollectorChannelID = collectorChannelId;
            customer.CreatorChannelID = creatorChannelId;
            customer.PublisherChannelID = publisherChannelId;
            customer.CharityChannelID = charityChannelId;
            customer.CustomerName = customerName;
            customer.ActiveFlag = activeFlag;
            customer.CommunicatorLevel = communicatorLevel;
            customer.CollectorLevel = collectorLevel;
            customer.CreatorLevel = creatorLevel;
            customer.PublisherLevel = publisherLevel;
            customer.CharityLevel = charityLevel;
            customer.AccountsLevel = accountsLevel;
            customer.Address = address;
            customer.City = city;
            customer.State = state;
            customer.Country = country;
            customer.Zip = zip;
            customer.Salutation = salutation;
            customer.ContactName = contactName;
            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.ContactTitle = contactTitle;
            customer.Phone = phone;
            customer.Fax = fax;
            customer.Email = email;
            customer.WebAddress = webAddress;
            customer.TechContact = techContact;
            customer.TechEmail = techEmail;
            customer.TechPhone = techPhone;
            customer.SubscriptionsEmail = subscriptionsEmail;
            customer.CustomerType = customerType;
            customer.DemoFlag = demoFlag;
            customer.AccountExecutiveID = accountExecutiveId;
            customer.AccountManagerID = accountManagerId;
            customer.IsStrategic = isStrategic;
            customer.customer_udf1 = customer_udf1;
            customer.customer_udf2 = customer_udf2;
            customer.customer_udf3 = customer_udf3;
            customer.customer_udf4 = customer_udf4;
            customer.customer_udf5 = customer_udf5;
            customer.BlastConfigID = blastConfigId;
            customer.BounceThreshold = bounceThreshold;
            customer.SoftBounceThreshold = softBounceThreshold;
            customer.TextPowerKWD = textPowerKWD;
            customer.TextPowerWelcomeMsg = textPowerWelcomeMsg;
            customer.ABWinnerType = aBWinnerType;
            customer.BillingContact = billingContact;
            customer.GeneralContant = generalContant;
            customer.CreatedUserID = createdUserId;
            customer.CreatedDate = createdDate;
            customer.UpdatedUserID = updatedUserId;
            customer.UpdatedDate = updatedDate;
            customer.IsDeleted = isDeleted;
            customer.MSCustomerID = mSCustomerId;
            customer.DefaultBlastAsTest = defaultBlastAsTest;
            customer.TestBlastLimit = testBlastLimit;
            customer.PlatformClientID = platformClientId;

            // Assert
            customer.CustomerID.ShouldBe(customerId);
            customer.BaseChannelID.ShouldBe(baseChannelId);
            customer.CommunicatorChannelID.ShouldBe(communicatorChannelId);
            customer.CollectorChannelID.ShouldBe(collectorChannelId);
            customer.CreatorChannelID.ShouldBe(creatorChannelId);
            customer.PublisherChannelID.ShouldBe(publisherChannelId);
            customer.CharityChannelID.ShouldBe(charityChannelId);
            customer.CustomerName.ShouldBe(customerName);
            customer.ActiveFlag.ShouldBe(activeFlag);
            customer.CommunicatorLevel.ShouldBe(communicatorLevel);
            customer.CollectorLevel.ShouldBe(collectorLevel);
            customer.CreatorLevel.ShouldBe(creatorLevel);
            customer.PublisherLevel.ShouldBe(publisherLevel);
            customer.CharityLevel.ShouldBe(charityLevel);
            customer.AccountsLevel.ShouldBe(accountsLevel);
            customer.Address.ShouldBe(address);
            customer.City.ShouldBe(city);
            customer.State.ShouldBe(state);
            customer.Country.ShouldBe(country);
            customer.Zip.ShouldBe(zip);
            customer.Salutation.ShouldBe(salutation);
            customer.ContactName.ShouldBe(contactName);
            customer.FirstName.ShouldBe(firstName);
            customer.LastName.ShouldBe(lastName);
            customer.ContactTitle.ShouldBe(contactTitle);
            customer.Phone.ShouldBe(phone);
            customer.Fax.ShouldBe(fax);
            customer.Email.ShouldBe(email);
            customer.WebAddress.ShouldBe(webAddress);
            customer.TechContact.ShouldBe(techContact);
            customer.TechEmail.ShouldBe(techEmail);
            customer.TechPhone.ShouldBe(techPhone);
            customer.SubscriptionsEmail.ShouldBe(subscriptionsEmail);
            customer.CustomerType.ShouldBe(customerType);
            customer.DemoFlag.ShouldBe(demoFlag);
            customer.AccountExecutiveID.ShouldBe(accountExecutiveId);
            customer.AccountManagerID.ShouldBe(accountManagerId);
            customer.IsStrategic.ShouldBe(isStrategic);
            customer.customer_udf1.ShouldBe(customer_udf1);
            customer.customer_udf2.ShouldBe(customer_udf2);
            customer.customer_udf3.ShouldBe(customer_udf3);
            customer.customer_udf4.ShouldBe(customer_udf4);
            customer.customer_udf5.ShouldBe(customer_udf5);
            customer.BlastConfigID.ShouldBe(blastConfigId);
            customer.BounceThreshold.ShouldBe(bounceThreshold);
            customer.SoftBounceThreshold.ShouldBe(softBounceThreshold);
            customer.TextPowerKWD.ShouldBe(textPowerKWD);
            customer.TextPowerWelcomeMsg.ShouldBe(textPowerWelcomeMsg);
            customer.ABWinnerType.ShouldBe(aBWinnerType);
            customer.BillingContact.ShouldBe(billingContact);
            customer.GeneralContant.ShouldBe(generalContant);
            customer.CreatedUserID.ShouldBe(createdUserId);
            customer.CreatedDate.ShouldBe(createdDate);
            customer.UpdatedUserID.ShouldBe(updatedUserId);
            customer.UpdatedDate.ShouldBe(updatedDate);
            customer.IsDeleted.ShouldBe(isDeleted);
            customer.MSCustomerID.ShouldBe(mSCustomerId);
            customer.DefaultBlastAsTest.ShouldBe(defaultBlastAsTest);
            customer.TestBlastLimit.ShouldBe(testBlastLimit);
            customer.PlatformClientID.ShouldBe(platformClientId);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (ABWinnerType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ABWinnerType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.ABWinnerType = Fixture.Create<string>();
            var stringType = customer.ABWinnerType.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (ABWinnerType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_ABWinnerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameABWinnerType = "ABWinnerTypeNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameABWinnerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ABWinnerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameABWinnerType = "ABWinnerType";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameABWinnerType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (AccountExecutiveID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountExecutiveID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.AccountExecutiveID = random;

            // Assert
            customer.AccountExecutiveID.ShouldBe(random);
            customer.AccountExecutiveID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountExecutiveID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.AccountExecutiveID = null;

            // Assert
            customer.AccountExecutiveID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountExecutiveID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAccountExecutiveID = "AccountExecutiveID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameAccountExecutiveID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.AccountExecutiveID.ShouldBeNull();
            customer.AccountExecutiveID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (AccountExecutiveID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_AccountExecutiveIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccountExecutiveID = "AccountExecutiveIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameAccountExecutiveID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountExecutiveID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccountExecutiveID = "AccountExecutiveID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameAccountExecutiveID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (AccountManagerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountManagerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.AccountManagerID = random;

            // Assert
            customer.AccountManagerID.ShouldBe(random);
            customer.AccountManagerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountManagerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.AccountManagerID = null;

            // Assert
            customer.AccountManagerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountManagerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameAccountManagerID = "AccountManagerID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameAccountManagerID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.AccountManagerID.ShouldBeNull();
            customer.AccountManagerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (AccountManagerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_AccountManagerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccountManagerID = "AccountManagerIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameAccountManagerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountManagerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccountManagerID = "AccountManagerID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameAccountManagerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (AccountsLevel) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountsLevel_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.AccountsLevel = Fixture.Create<string>();
            var stringType = customer.AccountsLevel.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (AccountsLevel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_AccountsLevelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccountsLevel = "AccountsLevelNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameAccountsLevel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_AccountsLevel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccountsLevel = "AccountsLevel";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameAccountsLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (ActiveFlag) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ActiveFlag_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.ActiveFlag = Fixture.Create<string>();
            var stringType = customer.ActiveFlag.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (ActiveFlag) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_ActiveFlagNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActiveFlag = "ActiveFlagNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameActiveFlag));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ActiveFlag_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActiveFlag = "ActiveFlag";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameActiveFlag);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.Address = Fixture.Create<string>();
            var stringType = customer.Address.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BaseChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BaseChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.BaseChannelID = random;

            // Assert
            customer.BaseChannelID.ShouldBe(random);
            customer.BaseChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BaseChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.BaseChannelID = null;

            // Assert
            customer.BaseChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BaseChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBaseChannelID = "BaseChannelID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameBaseChannelID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.BaseChannelID.ShouldBeNull();
            customer.BaseChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BillingContact) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BillingContact_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBillingContact = "BillingContact";
            var customer = Fixture.Create<Customer>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameBillingContact);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customer, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BillingContact) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_BillingContactNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBillingContact = "BillingContactNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameBillingContact));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BillingContact_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBillingContact = "BillingContact";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameBillingContact);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BlastConfigID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BlastConfigID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.BlastConfigID = random;

            // Assert
            customer.BlastConfigID.ShouldBe(random);
            customer.BlastConfigID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BlastConfigID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.BlastConfigID = null;

            // Assert
            customer.BlastConfigID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BlastConfigID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBlastConfigID = "BlastConfigID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameBlastConfigID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.BlastConfigID.ShouldBeNull();
            customer.BlastConfigID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BlastConfigID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_BlastConfigIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastConfigID = "BlastConfigIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameBlastConfigID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BlastConfigID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastConfigID = "BlastConfigID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameBlastConfigID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BounceThreshold) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BounceThreshold_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.BounceThreshold = random;

            // Assert
            customer.BounceThreshold.ShouldBe(random);
            customer.BounceThreshold.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BounceThreshold_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.BounceThreshold = null;

            // Assert
            customer.BounceThreshold.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BounceThreshold_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBounceThreshold = "BounceThreshold";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameBounceThreshold);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.BounceThreshold.ShouldBeNull();
            customer.BounceThreshold.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (BounceThreshold) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_BounceThresholdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceThreshold = "BounceThresholdNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameBounceThreshold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_BounceThreshold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceThreshold = "BounceThreshold";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameBounceThreshold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CharityChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CharityChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.CharityChannelID = random;

            // Assert
            customer.CharityChannelID.ShouldBe(random);
            customer.CharityChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CharityChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.CharityChannelID = null;

            // Assert
            customer.CharityChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CharityChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCharityChannelID = "CharityChannelID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameCharityChannelID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.CharityChannelID.ShouldBeNull();
            customer.CharityChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CharityChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CharityChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCharityChannelID = "CharityChannelIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCharityChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CharityChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCharityChannelID = "CharityChannelID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCharityChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CharityLevel) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CharityLevel_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CharityLevel = Fixture.Create<string>();
            var stringType = customer.CharityLevel.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CharityLevel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CharityLevelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCharityLevel = "CharityLevelNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCharityLevel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CharityLevel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCharityLevel = "CharityLevel";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCharityLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.City = Fixture.Create<string>();
            var stringType = customer.City.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CollectorChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CollectorChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.CollectorChannelID = random;

            // Assert
            customer.CollectorChannelID.ShouldBe(random);
            customer.CollectorChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CollectorChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.CollectorChannelID = null;

            // Assert
            customer.CollectorChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CollectorChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCollectorChannelID = "CollectorChannelID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameCollectorChannelID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.CollectorChannelID.ShouldBeNull();
            customer.CollectorChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CollectorChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CollectorChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCollectorChannelID = "CollectorChannelIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCollectorChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CollectorChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCollectorChannelID = "CollectorChannelID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCollectorChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CollectorLevel) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CollectorLevel_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CollectorLevel = Fixture.Create<string>();
            var stringType = customer.CollectorLevel.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CollectorLevel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CollectorLevelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCollectorLevel = "CollectorLevelNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCollectorLevel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CollectorLevel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCollectorLevel = "CollectorLevel";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCollectorLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CommunicatorChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CommunicatorChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.CommunicatorChannelID = random;

            // Assert
            customer.CommunicatorChannelID.ShouldBe(random);
            customer.CommunicatorChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CommunicatorChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.CommunicatorChannelID = null;

            // Assert
            customer.CommunicatorChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CommunicatorChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCommunicatorChannelID = "CommunicatorChannelID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameCommunicatorChannelID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.CommunicatorChannelID.ShouldBeNull();
            customer.CommunicatorChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CommunicatorChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CommunicatorChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCommunicatorChannelID = "CommunicatorChannelIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCommunicatorChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CommunicatorChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCommunicatorChannelID = "CommunicatorChannelID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCommunicatorChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CommunicatorLevel) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CommunicatorLevel_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CommunicatorLevel = Fixture.Create<string>();
            var stringType = customer.CommunicatorLevel.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CommunicatorLevel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CommunicatorLevelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCommunicatorLevel = "CommunicatorLevelNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCommunicatorLevel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CommunicatorLevel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCommunicatorLevel = "CommunicatorLevel";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCommunicatorLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (ContactName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ContactName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.ContactName = Fixture.Create<string>();
            var stringType = customer.ContactName.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (ContactName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_ContactNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactName = "ContactNameNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameContactName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ContactName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactName = "ContactName";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameContactName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (ContactTitle) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ContactTitle_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.ContactTitle = Fixture.Create<string>();
            var stringType = customer.ContactTitle.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (ContactTitle) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_ContactTitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactTitle = "ContactTitleNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameContactTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_ContactTitle_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactTitle = "ContactTitle";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameContactTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Country) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Country_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.Country = Fixture.Create<string>();
            var stringType = customer.Country.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (Country) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CountryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCountry = "CountryNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCountry));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Country_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCountry = "Country";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCountry);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customer = Fixture.Create<Customer>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customer, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.CreatedUserID = random;

            // Assert
            customer.CreatedUserID.ShouldBe(random);
            customer.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.CreatedUserID = null;

            // Assert
            customer.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.CreatedUserID.ShouldBeNull();
            customer.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatorChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatorChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.CreatorChannelID = random;

            // Assert
            customer.CreatorChannelID.ShouldBe(random);
            customer.CreatorChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatorChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.CreatorChannelID = null;

            // Assert
            customer.CreatorChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatorChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatorChannelID = "CreatorChannelID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameCreatorChannelID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.CreatorChannelID.ShouldBeNull();
            customer.CreatorChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatorChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CreatorChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatorChannelID = "CreatorChannelIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCreatorChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatorChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatorChannelID = "CreatorChannelID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCreatorChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CreatorLevel) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatorLevel_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CreatorLevel = Fixture.Create<string>();
            var stringType = customer.CreatorLevel.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CreatorLevel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CreatorLevelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatorLevel = "CreatorLevelNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCreatorLevel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CreatorLevel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatorLevel = "CreatorLevel";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCreatorLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (customer_udf1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.customer_udf1 = Fixture.Create<string>();
            var stringType = customer.customer_udf1.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (customer_udf1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_customer_udf1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf1 = "customer_udf1NotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamecustomer_udf1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf1 = "customer_udf1";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamecustomer_udf1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (customer_udf2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.customer_udf2 = Fixture.Create<string>();
            var stringType = customer.customer_udf2.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (customer_udf2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_customer_udf2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf2 = "customer_udf2NotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamecustomer_udf2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf2 = "customer_udf2";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamecustomer_udf2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (customer_udf3) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf3_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.customer_udf3 = Fixture.Create<string>();
            var stringType = customer.customer_udf3.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (customer_udf3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_customer_udf3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf3 = "customer_udf3NotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamecustomer_udf3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf3 = "customer_udf3";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamecustomer_udf3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (customer_udf4) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf4_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.customer_udf4 = Fixture.Create<string>();
            var stringType = customer.customer_udf4.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (customer_udf4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_customer_udf4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf4 = "customer_udf4NotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamecustomer_udf4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf4 = "customer_udf4";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamecustomer_udf4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (customer_udf5) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf5_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.customer_udf5 = Fixture.Create<string>();
            var stringType = customer.customer_udf5.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (customer_udf5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_customer_udf5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf5 = "customer_udf5NotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamecustomer_udf5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_customer_udf5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamecustomer_udf5 = "customer_udf5";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamecustomer_udf5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CustomerID = Fixture.Create<int>();
            var intType = customer.CustomerID.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CustomerName = Fixture.Create<string>();
            var stringType = customer.CustomerName.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (CustomerType) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CustomerType_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.CustomerType = Fixture.Create<string>();
            var stringType = customer.CustomerType.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (CustomerType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_CustomerTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerType = "CustomerTypeNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameCustomerType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_CustomerType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerType = "CustomerType";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameCustomerType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (DefaultBlastAsTest) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_DefaultBlastAsTest_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customer.DefaultBlastAsTest = random;

            // Assert
            customer.DefaultBlastAsTest.ShouldBe(random);
            customer.DefaultBlastAsTest.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_DefaultBlastAsTest_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.DefaultBlastAsTest = null;

            // Assert
            customer.DefaultBlastAsTest.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_DefaultBlastAsTest_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDefaultBlastAsTest = "DefaultBlastAsTest";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameDefaultBlastAsTest);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.DefaultBlastAsTest.ShouldBeNull();
            customer.DefaultBlastAsTest.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (DefaultBlastAsTest) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_DefaultBlastAsTestNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDefaultBlastAsTest = "DefaultBlastAsTestNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameDefaultBlastAsTest));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_DefaultBlastAsTest_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDefaultBlastAsTest = "DefaultBlastAsTest";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameDefaultBlastAsTest);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (DemoFlag) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_DemoFlag_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.DemoFlag = Fixture.Create<string>();
            var stringType = customer.DemoFlag.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (DemoFlag) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_DemoFlagNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDemoFlag = "DemoFlagNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameDemoFlag));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_DemoFlag_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDemoFlag = "DemoFlag";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameDemoFlag);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Email) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Email_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.Email = Fixture.Create<string>();
            var stringType = customer.Email.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (Email) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_EmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmail = "EmailNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Email_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmail = "Email";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Fax) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Fax_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.Fax = Fixture.Create<string>();
            var stringType = customer.Fax.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (Fax) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_FaxNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFax = "FaxNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameFax));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Fax_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFax = "Fax";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameFax);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (FirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_FirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.FirstName = Fixture.Create<string>();
            var stringType = customer.FirstName.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (FirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_FirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstNameNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_FirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstName";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (GeneralContant) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_GeneralContant_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameGeneralContant = "GeneralContant";
            var customer = Fixture.Create<Customer>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameGeneralContant);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customer, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (GeneralContant) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_GeneralContantNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGeneralContant = "GeneralContantNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameGeneralContant));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_GeneralContant_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGeneralContant = "GeneralContant";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameGeneralContant);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customer.IsDeleted = random;

            // Assert
            customer.IsDeleted.ShouldBe(random);
            customer.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.IsDeleted = null;

            // Assert
            customer.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.IsDeleted.ShouldBeNull();
            customer.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (IsStrategic) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsStrategic_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customer.IsStrategic = random;

            // Assert
            customer.IsStrategic.ShouldBe(random);
            customer.IsStrategic.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsStrategic_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.IsStrategic = null;

            // Assert
            customer.IsStrategic.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsStrategic_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsStrategic = "IsStrategic";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameIsStrategic);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.IsStrategic.ShouldBeNull();
            customer.IsStrategic.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (IsStrategic) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_IsStrategicNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsStrategic = "IsStrategicNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameIsStrategic));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_IsStrategic_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsStrategic = "IsStrategic";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameIsStrategic);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (LastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_LastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.LastName = Fixture.Create<string>();
            var stringType = customer.LastName.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (LastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_LastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastNameNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_LastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastName";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (MSCustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_MSCustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.MSCustomerID = random;

            // Assert
            customer.MSCustomerID.ShouldBe(random);
            customer.MSCustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_MSCustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.MSCustomerID = null;

            // Assert
            customer.MSCustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_MSCustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameMSCustomerID = "MSCustomerID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameMSCustomerID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.MSCustomerID.ShouldBeNull();
            customer.MSCustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (MSCustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_MSCustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMSCustomerID = "MSCustomerIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameMSCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_MSCustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMSCustomerID = "MSCustomerID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameMSCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Phone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Phone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.Phone = Fixture.Create<string>();
            var stringType = customer.Phone.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (Phone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_PhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePhone = "PhoneNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamePhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Phone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePhone = "Phone";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamePhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (PlatformClientID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PlatformClientID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.PlatformClientID = Fixture.Create<int>();
            var intType = customer.PlatformClientID.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (PlatformClientID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_PlatformClientIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlatformClientID = "PlatformClientIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamePlatformClientID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PlatformClientID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlatformClientID = "PlatformClientID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamePlatformClientID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (PublisherChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PublisherChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.PublisherChannelID = random;

            // Assert
            customer.PublisherChannelID.ShouldBe(random);
            customer.PublisherChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PublisherChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.PublisherChannelID = null;

            // Assert
            customer.PublisherChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PublisherChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNamePublisherChannelID = "PublisherChannelID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNamePublisherChannelID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.PublisherChannelID.ShouldBeNull();
            customer.PublisherChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (PublisherChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_PublisherChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePublisherChannelID = "PublisherChannelIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamePublisherChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PublisherChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePublisherChannelID = "PublisherChannelID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamePublisherChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (PublisherLevel) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PublisherLevel_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.PublisherLevel = Fixture.Create<string>();
            var stringType = customer.PublisherLevel.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (PublisherLevel) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_PublisherLevelNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePublisherLevel = "PublisherLevelNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNamePublisherLevel));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_PublisherLevel_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePublisherLevel = "PublisherLevel";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNamePublisherLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Salutation) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Salutation_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSalutation = "Salutation";
            var customer = Fixture.Create<Customer>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameSalutation);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customer, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Salutation) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_SalutationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSalutation = "SalutationNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameSalutation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Salutation_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSalutation = "Salutation";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameSalutation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (SoftBounceThreshold) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_SoftBounceThreshold_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.SoftBounceThreshold = random;

            // Assert
            customer.SoftBounceThreshold.ShouldBe(random);
            customer.SoftBounceThreshold.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_SoftBounceThreshold_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.SoftBounceThreshold = null;

            // Assert
            customer.SoftBounceThreshold.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_SoftBounceThreshold_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSoftBounceThreshold = "SoftBounceThreshold";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameSoftBounceThreshold);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.SoftBounceThreshold.ShouldBeNull();
            customer.SoftBounceThreshold.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (SoftBounceThreshold) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_SoftBounceThresholdNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSoftBounceThreshold = "SoftBounceThresholdNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameSoftBounceThreshold));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_SoftBounceThreshold_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSoftBounceThreshold = "SoftBounceThreshold";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameSoftBounceThreshold);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.State = Fixture.Create<string>();
            var stringType = customer.State.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (SubscriptionsEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_SubscriptionsEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.SubscriptionsEmail = Fixture.Create<string>();
            var stringType = customer.SubscriptionsEmail.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (SubscriptionsEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_SubscriptionsEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscriptionsEmail = "SubscriptionsEmailNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameSubscriptionsEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_SubscriptionsEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscriptionsEmail = "SubscriptionsEmail";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameSubscriptionsEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TechContact) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TechContact_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.TechContact = Fixture.Create<string>();
            var stringType = customer.TechContact.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (TechContact) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_TechContactNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTechContact = "TechContactNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameTechContact));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TechContact_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTechContact = "TechContact";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameTechContact);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TechEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TechEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.TechEmail = Fixture.Create<string>();
            var stringType = customer.TechEmail.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (TechEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_TechEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTechEmail = "TechEmailNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameTechEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TechEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTechEmail = "TechEmail";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameTechEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TechPhone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TechPhone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.TechPhone = Fixture.Create<string>();
            var stringType = customer.TechPhone.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (TechPhone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_TechPhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTechPhone = "TechPhoneNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameTechPhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TechPhone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTechPhone = "TechPhone";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameTechPhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TestBlastLimit) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TestBlastLimit_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.TestBlastLimit = random;

            // Assert
            customer.TestBlastLimit.ShouldBe(random);
            customer.TestBlastLimit.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TestBlastLimit_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.TestBlastLimit = null;

            // Assert
            customer.TestBlastLimit.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TestBlastLimit_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameTestBlastLimit = "TestBlastLimit";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameTestBlastLimit);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.TestBlastLimit.ShouldBeNull();
            customer.TestBlastLimit.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TestBlastLimit) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_TestBlastLimitNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTestBlastLimit = "TestBlastLimitNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameTestBlastLimit));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TestBlastLimit_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTestBlastLimit = "TestBlastLimit";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameTestBlastLimit);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TextPowerKWD) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TextPowerKWD_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.TextPowerKWD = Fixture.Create<string>();
            var stringType = customer.TextPowerKWD.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (TextPowerKWD) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_TextPowerKWDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTextPowerKWD = "TextPowerKWDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameTextPowerKWD));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TextPowerKWD_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTextPowerKWD = "TextPowerKWD";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameTextPowerKWD);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (TextPowerWelcomeMsg) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TextPowerWelcomeMsg_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.TextPowerWelcomeMsg = Fixture.Create<string>();
            var stringType = customer.TextPowerWelcomeMsg.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (TextPowerWelcomeMsg) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_TextPowerWelcomeMsgNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTextPowerWelcomeMsg = "TextPowerWelcomeMsgNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameTextPowerWelcomeMsg));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_TextPowerWelcomeMsg_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTextPowerWelcomeMsg = "TextPowerWelcomeMsg";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameTextPowerWelcomeMsg);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customer = Fixture.Create<Customer>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customer, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            var random = Fixture.Create<int>();

            // Act , Set
            customer.UpdatedUserID = random;

            // Assert
            customer.UpdatedUserID.ShouldBe(random);
            customer.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();    

            // Act , Set
            customer.UpdatedUserID = null;

            // Assert
            customer.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customer = Fixture.Create<Customer>();
            var propertyInfo = customer.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customer, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customer.UpdatedUserID.ShouldBeNull();
            customer.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (WebAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_WebAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.WebAddress = Fixture.Create<string>();
            var stringType = customer.WebAddress.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (WebAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_WebAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWebAddress = "WebAddressNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameWebAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_WebAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWebAddress = "WebAddress";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameWebAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Customer) => Property (Zip) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Zip_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customer = Fixture.Create<Customer>();
            customer.Zip = Fixture.Create<string>();
            var stringType = customer.Zip.GetType();

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

        #region General Getters/Setters : Class (Customer) => Property (Zip) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Class_Invalid_Property_ZipNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameZip = "ZipNotPresent";
            var customer  = Fixture.Create<Customer>();

            // Act , Assert
            Should.NotThrow(() => customer.GetType().GetProperty(propertyNameZip));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Customer_Zip_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameZip = "Zip";
            var customer  = Fixture.Create<Customer>();
            var propertyInfo  = customer.GetType().GetProperty(propertyNameZip);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Customer) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Customer_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Customer());
        }

        #endregion

        #region General Constructor : Class (Customer) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Customer_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCustomer = Fixture.CreateMany<Customer>(2).ToList();
            var firstCustomer = instancesOfCustomer.FirstOrDefault();
            var lastCustomer = instancesOfCustomer.Last();

            // Act, Assert
            firstCustomer.ShouldNotBeNull();
            lastCustomer.ShouldNotBeNull();
            firstCustomer.ShouldNotBeSameAs(lastCustomer);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Customer_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCustomer = new Customer();
            var secondCustomer = new Customer();
            var thirdCustomer = new Customer();
            var fourthCustomer = new Customer();
            var fifthCustomer = new Customer();
            var sixthCustomer = new Customer();

            // Act, Assert
            firstCustomer.ShouldNotBeNull();
            secondCustomer.ShouldNotBeNull();
            thirdCustomer.ShouldNotBeNull();
            fourthCustomer.ShouldNotBeNull();
            fifthCustomer.ShouldNotBeNull();
            sixthCustomer.ShouldNotBeNull();
            firstCustomer.ShouldNotBeSameAs(secondCustomer);
            thirdCustomer.ShouldNotBeSameAs(firstCustomer);
            fourthCustomer.ShouldNotBeSameAs(firstCustomer);
            fifthCustomer.ShouldNotBeSameAs(firstCustomer);
            sixthCustomer.ShouldNotBeSameAs(firstCustomer);
            sixthCustomer.ShouldNotBeSameAs(fourthCustomer);
        }

        #endregion

        #region General Constructor : Class (Customer) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Customer_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var customerId = -1;
            var customerName = string.Empty;
            var activeFlag = string.Empty;
            var communicatorLevel = string.Empty;
            var collectorLevel = string.Empty;
            var creatorLevel = string.Empty;
            var publisherLevel = string.Empty;
            var charityLevel = string.Empty;
            var accountsLevel = string.Empty;
            var address = string.Empty;
            var city = string.Empty;
            var state = string.Empty;
            var country = string.Empty;
            var zip = string.Empty;
            var salutation = ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
            var contactName = string.Empty;
            var firstName = string.Empty;
            var lastName = string.Empty;
            var contactTitle = string.Empty;
            var phone = string.Empty;
            var fax = string.Empty;
            var email = string.Empty;
            var webAddress = string.Empty;
            var techContact = string.Empty;
            var techEmail = string.Empty;
            var techPhone = string.Empty;
            var subscriptionsEmail = string.Empty;
            var customerType = string.Empty;
            var demoFlag = string.Empty;
            var customer_udf1 = string.Empty;
            var customer_udf2 = string.Empty;
            var customer_udf3 = string.Empty;
            var customer_udf4 = string.Empty;
            var customer_udf5 = string.Empty;
            var blastConfigId = 3;
            var textPowerKWD = string.Empty;
            var textPowerWelcomeMsg = string.Empty;
            var defaultBlastAsTest = false;
            var platformClientId = -1;

            // Act
            var customer = new Customer();

            // Assert
            customer.CustomerID.ShouldBe(customerId);
            customer.BaseChannelID.ShouldBeNull();
            customer.CommunicatorChannelID.ShouldBeNull();
            customer.CollectorChannelID.ShouldBeNull();
            customer.CreatorChannelID.ShouldBeNull();
            customer.PublisherChannelID.ShouldBeNull();
            customer.CharityChannelID.ShouldBeNull();
            customer.CustomerName.ShouldBe(customerName);
            customer.ActiveFlag.ShouldBe(activeFlag);
            customer.CommunicatorLevel.ShouldBe(communicatorLevel);
            customer.CollectorLevel.ShouldBe(collectorLevel);
            customer.CreatorLevel.ShouldBe(creatorLevel);
            customer.PublisherLevel.ShouldBe(publisherLevel);
            customer.CharityLevel.ShouldBe(charityLevel);
            customer.AccountsLevel.ShouldBe(accountsLevel);
            customer.Address.ShouldBe(address);
            customer.City.ShouldBe(city);
            customer.State.ShouldBe(state);
            customer.Country.ShouldBe(country);
            customer.Zip.ShouldBe(zip);
            customer.Salutation.ShouldBe(salutation);
            customer.ContactName.ShouldBe(contactName);
            customer.FirstName.ShouldBe(firstName);
            customer.LastName.ShouldBe(lastName);
            customer.ContactTitle.ShouldBe(contactTitle);
            customer.Phone.ShouldBe(phone);
            customer.Fax.ShouldBe(fax);
            customer.Email.ShouldBe(email);
            customer.WebAddress.ShouldBe(webAddress);
            customer.TechContact.ShouldBe(techContact);
            customer.TechEmail.ShouldBe(techEmail);
            customer.TechPhone.ShouldBe(techPhone);
            customer.SubscriptionsEmail.ShouldBe(subscriptionsEmail);
            customer.CustomerType.ShouldBe(customerType);
            customer.DemoFlag.ShouldBe(demoFlag);
            customer.AccountExecutiveID.ShouldBeNull();
            customer.AccountManagerID.ShouldBeNull();
            customer.IsStrategic.ShouldBeNull();
            customer.customer_udf1.ShouldBe(customer_udf1);
            customer.customer_udf2.ShouldBe(customer_udf2);
            customer.customer_udf3.ShouldBe(customer_udf3);
            customer.customer_udf4.ShouldBe(customer_udf4);
            customer.customer_udf5.ShouldBe(customer_udf5);
            customer.BlastConfigID.ShouldBe(blastConfigId);
            customer.BounceThreshold.ShouldBeNull();
            customer.SoftBounceThreshold.ShouldBeNull();
            customer.TextPowerKWD.ShouldBe(textPowerKWD);
            customer.TextPowerWelcomeMsg.ShouldBe(textPowerWelcomeMsg);
            customer.BillingContact.ShouldNotBeNull();
            customer.GeneralContant.ShouldNotBeNull();
            customer.CreatedUserID.ShouldBeNull();
            customer.CreatedDate.ShouldBeNull();
            customer.UpdatedUserID.ShouldBeNull();
            customer.UpdatedDate.ShouldBeNull();
            customer.IsDeleted.ShouldBeNull();
            customer.MSCustomerID.ShouldBeNull();
            customer.ABWinnerType.ShouldBeNull();
            customer.DefaultBlastAsTest.ShouldBe(defaultBlastAsTest);
            customer.TestBlastLimit.ShouldBeNull();
            customer.PlatformClientID.ShouldBe(platformClientId);
        }

        #endregion

        #endregion

        #endregion
    }
}