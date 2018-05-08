using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class EmailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Email) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            var emailId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();
            var customerId = Fixture.Create<int>();
            var title = Fixture.Create<string>();
            var firstName = Fixture.Create<string>();
            var lastName = Fixture.Create<string>();
            var fullName = Fixture.Create<string>();
            var company = Fixture.Create<string>();
            var occupation = Fixture.Create<string>();
            var address = Fixture.Create<string>();
            var address2 = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var zip = Fixture.Create<string>();
            var country = Fixture.Create<string>();
            var voice = Fixture.Create<string>();
            var mobile = Fixture.Create<string>();
            var fax = Fixture.Create<string>();
            var website = Fixture.Create<string>();
            var age = Fixture.Create<string>();
            var income = Fixture.Create<string>();
            var gender = Fixture.Create<string>();
            var user1 = Fixture.Create<string>();
            var user2 = Fixture.Create<string>();
            var user3 = Fixture.Create<string>();
            var user4 = Fixture.Create<string>();
            var user5 = Fixture.Create<string>();
            var user6 = Fixture.Create<string>();
            var birthdate = Fixture.Create<DateTime?>();
            var userEvent1 = Fixture.Create<string>();
            var userEvent1Date = Fixture.Create<DateTime?>();
            var userEvent2 = Fixture.Create<string>();
            var userEvent2Date = Fixture.Create<DateTime?>();
            var notes = Fixture.Create<string>();
            var password = Fixture.Create<string>();
            var bounceScore = Fixture.Create<int?>();
            var softBounceScore = Fixture.Create<int?>();
            var sMSOptIn = Fixture.Create<string>();
            var carrierCode = Fixture.Create<string>();
            var formatTypeCode = Fixture.Create<string>();
            var subscribeTypeCode = Fixture.Create<string>();
            var dateAdded = Fixture.Create<DateTime?>();
            var dateUpdated = Fixture.Create<DateTime?>();

            // Act
            email.EmailID = emailId;
            email.EmailAddress = emailAddress;
            email.CustomerID = customerId;
            email.Title = title;
            email.FirstName = firstName;
            email.LastName = lastName;
            email.FullName = fullName;
            email.Company = company;
            email.Occupation = occupation;
            email.Address = address;
            email.Address2 = address2;
            email.City = city;
            email.State = state;
            email.Zip = zip;
            email.Country = country;
            email.Voice = voice;
            email.Mobile = mobile;
            email.Fax = fax;
            email.Website = website;
            email.Age = age;
            email.Income = income;
            email.Gender = gender;
            email.User1 = user1;
            email.User2 = user2;
            email.User3 = user3;
            email.User4 = user4;
            email.User5 = user5;
            email.User6 = user6;
            email.Birthdate = birthdate;
            email.UserEvent1 = userEvent1;
            email.UserEvent1Date = userEvent1Date;
            email.UserEvent2 = userEvent2;
            email.UserEvent2Date = userEvent2Date;
            email.Notes = notes;
            email.Password = password;
            email.BounceScore = bounceScore;
            email.SoftBounceScore = softBounceScore;
            email.SMSOptIn = sMSOptIn;
            email.CarrierCode = carrierCode;
            email.FormatTypeCode = formatTypeCode;
            email.SubscribeTypeCode = subscribeTypeCode;
            email.DateAdded = dateAdded;
            email.DateUpdated = dateUpdated;

            // Assert
            email.EmailID.ShouldBe(emailId);
            email.EmailAddress.ShouldBe(emailAddress);
            email.CustomerID.ShouldBe(customerId);
            email.Title.ShouldBe(title);
            email.FirstName.ShouldBe(firstName);
            email.LastName.ShouldBe(lastName);
            email.FullName.ShouldBe(fullName);
            email.Company.ShouldBe(company);
            email.Occupation.ShouldBe(occupation);
            email.Address.ShouldBe(address);
            email.Address2.ShouldBe(address2);
            email.City.ShouldBe(city);
            email.State.ShouldBe(state);
            email.Zip.ShouldBe(zip);
            email.Country.ShouldBe(country);
            email.Voice.ShouldBe(voice);
            email.Mobile.ShouldBe(mobile);
            email.Fax.ShouldBe(fax);
            email.Website.ShouldBe(website);
            email.Age.ShouldBe(age);
            email.Income.ShouldBe(income);
            email.Gender.ShouldBe(gender);
            email.User1.ShouldBe(user1);
            email.User2.ShouldBe(user2);
            email.User3.ShouldBe(user3);
            email.User4.ShouldBe(user4);
            email.User5.ShouldBe(user5);
            email.User6.ShouldBe(user6);
            email.Birthdate.ShouldBe(birthdate);
            email.UserEvent1.ShouldBe(userEvent1);
            email.UserEvent1Date.ShouldBe(userEvent1Date);
            email.UserEvent2.ShouldBe(userEvent2);
            email.UserEvent2Date.ShouldBe(userEvent2Date);
            email.Notes.ShouldBe(notes);
            email.Password.ShouldBe(password);
            email.BounceScore.ShouldBe(bounceScore);
            email.SoftBounceScore.ShouldBe(softBounceScore);
            email.SMSOptIn.ShouldBe(sMSOptIn);
            email.CarrierCode.ShouldBe(carrierCode);
            email.FormatTypeCode.ShouldBe(formatTypeCode);
            email.SubscribeTypeCode.ShouldBe(subscribeTypeCode);
            email.DateAdded.ShouldBe(dateAdded);
            email.DateUpdated.ShouldBe(dateUpdated);
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Address = Fixture.Create<string>();
            var stringType = email.Address.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Address2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Address2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Address2 = Fixture.Create<string>();
            var stringType = email.Address2.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Address2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_Address2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress2 = "Address2NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameAddress2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Address2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress2 = "Address2";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameAddress2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Age) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Age_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Age = Fixture.Create<string>();
            var stringType = email.Age.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Age) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_AgeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAge = "AgeNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameAge));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Age_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAge = "Age";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameAge);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Birthdate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Birthdate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameBirthdate = "Birthdate";
            var email = Fixture.Create<Email>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = email.GetType().GetProperty(propertyNameBirthdate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(email, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Birthdate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_BirthdateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBirthdate = "BirthdateNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameBirthdate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Birthdate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBirthdate = "Birthdate";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameBirthdate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (BounceScore) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_BounceScore_Property_Data_Without_Null_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            var random = Fixture.Create<int>();

            // Act , Set
            email.BounceScore = random;

            // Assert
            email.BounceScore.ShouldBe(random);
            email.BounceScore.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_BounceScore_Property_Only_Null_Data_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();

            // Act , Set
            email.BounceScore = null;

            // Assert
            email.BounceScore.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_BounceScore_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameBounceScore = "BounceScore";
            var email = Fixture.Create<Email>();
            var propertyInfo = email.GetType().GetProperty(propertyNameBounceScore);

            // Act , Set
            propertyInfo.SetValue(email, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            email.BounceScore.ShouldBeNull();
            email.BounceScore.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (BounceScore) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_BounceScoreNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBounceScore = "BounceScoreNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameBounceScore));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_BounceScore_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBounceScore = "BounceScore";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameBounceScore);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (CarrierCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_CarrierCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.CarrierCode = Fixture.Create<string>();
            var stringType = email.CarrierCode.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (CarrierCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_CarrierCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCarrierCode = "CarrierCodeNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameCarrierCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_CarrierCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCarrierCode = "CarrierCode";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameCarrierCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.City = Fixture.Create<string>();
            var stringType = email.City.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Company) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Company_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Company = Fixture.Create<string>();
            var stringType = email.Company.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Company) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_CompanyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompany = "CompanyNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameCompany));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Company_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompany = "Company";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameCompany);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Country) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Country_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Country = Fixture.Create<string>();
            var stringType = email.Country.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Country) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_CountryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCountry = "CountryNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameCountry));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Country_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCountry = "Country";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameCountry);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.CustomerID = Fixture.Create<int>();
            var intType = email.CustomerID.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (DateAdded) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_DateAdded_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateAdded = "DateAdded";
            var email = Fixture.Create<Email>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = email.GetType().GetProperty(propertyNameDateAdded);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(email, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (DateAdded) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_DateAddedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateAdded = "DateAddedNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameDateAdded));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_DateAdded_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateAdded = "DateAdded";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameDateAdded);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (DateUpdated) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_DateUpdated_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateUpdated = "DateUpdated";
            var email = Fixture.Create<Email>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = email.GetType().GetProperty(propertyNameDateUpdated);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(email, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (DateUpdated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_DateUpdatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateUpdated = "DateUpdatedNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameDateUpdated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_DateUpdated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateUpdated = "DateUpdated";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameDateUpdated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.EmailAddress = Fixture.Create<string>();
            var stringType = email.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.EmailID = Fixture.Create<int>();
            var intType = email.EmailID.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Fax) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Fax_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Fax = Fixture.Create<string>();
            var stringType = email.Fax.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Fax) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_FaxNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFax = "FaxNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameFax));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Fax_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFax = "Fax";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameFax);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (FirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_FirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.FirstName = Fixture.Create<string>();
            var stringType = email.FirstName.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (FirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_FirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstNameNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_FirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstName";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (FormatTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_FormatTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.FormatTypeCode = Fixture.Create<string>();
            var stringType = email.FormatTypeCode.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (FormatTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_FormatTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFormatTypeCode = "FormatTypeCodeNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameFormatTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_FormatTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFormatTypeCode = "FormatTypeCode";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameFormatTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (FullName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_FullName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.FullName = Fixture.Create<string>();
            var stringType = email.FullName.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (FullName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_FullNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullName = "FullNameNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameFullName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_FullName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullName = "FullName";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameFullName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Gender) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Gender_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Gender = Fixture.Create<string>();
            var stringType = email.Gender.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Gender) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_GenderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGender = "GenderNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameGender));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Gender_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGender = "Gender";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameGender);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Income) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Income_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Income = Fixture.Create<string>();
            var stringType = email.Income.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Income) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_IncomeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIncome = "IncomeNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameIncome));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Income_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIncome = "Income";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameIncome);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (LastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_LastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.LastName = Fixture.Create<string>();
            var stringType = email.LastName.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (LastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_LastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastNameNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_LastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastName";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Mobile) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Mobile_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Mobile = Fixture.Create<string>();
            var stringType = email.Mobile.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Mobile) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_MobileNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMobile = "MobileNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameMobile));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Mobile_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMobile = "Mobile";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameMobile);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Notes) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Notes_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Notes = Fixture.Create<string>();
            var stringType = email.Notes.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Notes) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_NotesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNotes = "NotesNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameNotes));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Notes_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNotes = "Notes";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameNotes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Occupation) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Occupation_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Occupation = Fixture.Create<string>();
            var stringType = email.Occupation.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Occupation) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_OccupationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOccupation = "OccupationNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameOccupation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Occupation_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOccupation = "Occupation";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameOccupation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Password) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Password_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Password = Fixture.Create<string>();
            var stringType = email.Password.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Password) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_PasswordNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePassword = "PasswordNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNamePassword));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Password_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePassword = "Password";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNamePassword);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (SMSOptIn) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SMSOptIn_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.SMSOptIn = Fixture.Create<string>();
            var stringType = email.SMSOptIn.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (SMSOptIn) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_SMSOptInNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSMSOptIn = "SMSOptInNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameSMSOptIn));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SMSOptIn_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSMSOptIn = "SMSOptIn";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameSMSOptIn);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (SoftBounceScore) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SoftBounceScore_Property_Data_Without_Null_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            var random = Fixture.Create<int>();

            // Act , Set
            email.SoftBounceScore = random;

            // Assert
            email.SoftBounceScore.ShouldBe(random);
            email.SoftBounceScore.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SoftBounceScore_Property_Only_Null_Data_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();

            // Act , Set
            email.SoftBounceScore = null;

            // Assert
            email.SoftBounceScore.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SoftBounceScore_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSoftBounceScore = "SoftBounceScore";
            var email = Fixture.Create<Email>();
            var propertyInfo = email.GetType().GetProperty(propertyNameSoftBounceScore);

            // Act , Set
            propertyInfo.SetValue(email, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            email.SoftBounceScore.ShouldBeNull();
            email.SoftBounceScore.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (SoftBounceScore) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_SoftBounceScoreNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSoftBounceScore = "SoftBounceScoreNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameSoftBounceScore));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SoftBounceScore_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSoftBounceScore = "SoftBounceScore";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameSoftBounceScore);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.State = Fixture.Create<string>();
            var stringType = email.State.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (SubscribeTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SubscribeTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.SubscribeTypeCode = Fixture.Create<string>();
            var stringType = email.SubscribeTypeCode.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (SubscribeTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_SubscribeTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscribeTypeCode = "SubscribeTypeCodeNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameSubscribeTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_SubscribeTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscribeTypeCode = "SubscribeTypeCode";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameSubscribeTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Title) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Title_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Title = Fixture.Create<string>();
            var stringType = email.Title.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Title) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_TitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTitle = "TitleNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTitle = "Title";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (User1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.User1 = Fixture.Create<string>();
            var stringType = email.User1.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (User1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_User1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser1 = "User1NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUser1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser1 = "User1";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUser1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (User2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.User2 = Fixture.Create<string>();
            var stringType = email.User2.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (User2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_User2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser2 = "User2NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUser2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser2 = "User2";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUser2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (User3) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User3_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.User3 = Fixture.Create<string>();
            var stringType = email.User3.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (User3) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_User3NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser3 = "User3NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUser3));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User3_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser3 = "User3";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUser3);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (User4) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User4_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.User4 = Fixture.Create<string>();
            var stringType = email.User4.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (User4) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_User4NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser4 = "User4NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUser4));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User4_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser4 = "User4";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUser4);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (User5) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User5_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.User5 = Fixture.Create<string>();
            var stringType = email.User5.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (User5) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_User5NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser5 = "User5NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUser5));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User5_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser5 = "User5";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUser5);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (User6) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User6_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.User6 = Fixture.Create<string>();
            var stringType = email.User6.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (User6) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_User6NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUser6 = "User6NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUser6));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_User6_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUser6 = "User6";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUser6);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (UserEvent1) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent1_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.UserEvent1 = Fixture.Create<string>();
            var stringType = email.UserEvent1.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (UserEvent1) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_UserEvent1NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserEvent1 = "UserEvent1NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUserEvent1));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent1_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserEvent1 = "UserEvent1";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUserEvent1);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (UserEvent1Date) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent1Date_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUserEvent1Date = "UserEvent1Date";
            var email = Fixture.Create<Email>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = email.GetType().GetProperty(propertyNameUserEvent1Date);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(email, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (UserEvent1Date) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_UserEvent1DateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserEvent1Date = "UserEvent1DateNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUserEvent1Date));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent1Date_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserEvent1Date = "UserEvent1Date";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUserEvent1Date);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (UserEvent2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.UserEvent2 = Fixture.Create<string>();
            var stringType = email.UserEvent2.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (UserEvent2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_UserEvent2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserEvent2 = "UserEvent2NotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUserEvent2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserEvent2 = "UserEvent2";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUserEvent2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (UserEvent2Date) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent2Date_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUserEvent2Date = "UserEvent2Date";
            var email = Fixture.Create<Email>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = email.GetType().GetProperty(propertyNameUserEvent2Date);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(email, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (UserEvent2Date) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_UserEvent2DateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserEvent2Date = "UserEvent2DateNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameUserEvent2Date));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_UserEvent2Date_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserEvent2Date = "UserEvent2Date";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameUserEvent2Date);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Voice) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Voice_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Voice = Fixture.Create<string>();
            var stringType = email.Voice.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Voice) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_VoiceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameVoice = "VoiceNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameVoice));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Voice_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameVoice = "Voice";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameVoice);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Website) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Website_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Website = Fixture.Create<string>();
            var stringType = email.Website.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Website) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_WebsiteNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWebsite = "WebsiteNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameWebsite));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Website_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWebsite = "Website";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameWebsite);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Email) => Property (Zip) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Zip_Property_String_Type_Verify_Test()
        {
            // Arrange
            var email = Fixture.Create<Email>();
            email.Zip = Fixture.Create<string>();
            var stringType = email.Zip.GetType();

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

        #region General Getters/Setters : Class (Email) => Property (Zip) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Class_Invalid_Property_ZipNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameZip = "ZipNotPresent";
            var email  = Fixture.Create<Email>();

            // Act , Assert
            Should.NotThrow(() => email.GetType().GetProperty(propertyNameZip));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Email_Zip_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameZip = "Zip";
            var email  = Fixture.Create<Email>();
            var propertyInfo  = email.GetType().GetProperty(propertyNameZip);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Email) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Email_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Email());
        }

        #endregion

        #region General Constructor : Class (Email) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Email_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmail = Fixture.CreateMany<Email>(2).ToList();
            var firstEmail = instancesOfEmail.FirstOrDefault();
            var lastEmail = instancesOfEmail.Last();

            // Act, Assert
            firstEmail.ShouldNotBeNull();
            lastEmail.ShouldNotBeNull();
            firstEmail.ShouldNotBeSameAs(lastEmail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Email_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmail = new Email();
            var secondEmail = new Email();
            var thirdEmail = new Email();
            var fourthEmail = new Email();
            var fifthEmail = new Email();
            var sixthEmail = new Email();

            // Act, Assert
            firstEmail.ShouldNotBeNull();
            secondEmail.ShouldNotBeNull();
            thirdEmail.ShouldNotBeNull();
            fourthEmail.ShouldNotBeNull();
            fifthEmail.ShouldNotBeNull();
            sixthEmail.ShouldNotBeNull();
            firstEmail.ShouldNotBeSameAs(secondEmail);
            thirdEmail.ShouldNotBeSameAs(firstEmail);
            fourthEmail.ShouldNotBeSameAs(firstEmail);
            fifthEmail.ShouldNotBeSameAs(firstEmail);
            sixthEmail.ShouldNotBeSameAs(firstEmail);
            sixthEmail.ShouldNotBeSameAs(fourthEmail);
        }

        #endregion

        #region General Constructor : Class (Email) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Email_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailId = -1;
            var emailAddress = string.Empty;
            var customerId = -1;
            var title = string.Empty;
            var firstName = string.Empty;
            var lastName = string.Empty;
            var fullName = string.Empty;
            var company = string.Empty;
            var occupation = string.Empty;
            var address = string.Empty;
            var address2 = string.Empty;
            var city = string.Empty;
            var state = string.Empty;
            var zip = string.Empty;
            var country = string.Empty;
            var voice = string.Empty;
            var mobile = string.Empty;
            var fax = string.Empty;
            var website = string.Empty;
            var age = string.Empty;
            var income = string.Empty;
            var gender = string.Empty;
            var user1 = string.Empty;
            var user2 = string.Empty;
            var user3 = string.Empty;
            var user4 = string.Empty;
            var user5 = string.Empty;
            var user6 = string.Empty;
            var userEvent1 = string.Empty;
            var userEvent2 = string.Empty;
            var notes = string.Empty;
            var password = string.Empty;
            var sMSOptIn = string.Empty;
            var carrierCode = string.Empty;
            var formatTypeCode = string.Empty;
            var subscribeTypeCode = string.Empty;

            // Act
            var email = new Email();

            // Assert
            email.EmailID.ShouldBe(emailId);
            email.EmailAddress.ShouldBe(emailAddress);
            email.CustomerID.ShouldBe(customerId);
            email.Title.ShouldBe(title);
            email.FirstName.ShouldBe(firstName);
            email.LastName.ShouldBe(lastName);
            email.FullName.ShouldBe(fullName);
            email.Company.ShouldBe(company);
            email.Occupation.ShouldBe(occupation);
            email.Address.ShouldBe(address);
            email.Address2.ShouldBe(address2);
            email.City.ShouldBe(city);
            email.State.ShouldBe(state);
            email.Zip.ShouldBe(zip);
            email.Country.ShouldBe(country);
            email.Voice.ShouldBe(voice);
            email.Mobile.ShouldBe(mobile);
            email.Fax.ShouldBe(fax);
            email.Website.ShouldBe(website);
            email.Age.ShouldBe(age);
            email.Income.ShouldBe(income);
            email.Gender.ShouldBe(gender);
            email.User1.ShouldBe(user1);
            email.User2.ShouldBe(user2);
            email.User3.ShouldBe(user3);
            email.User4.ShouldBe(user4);
            email.User5.ShouldBe(user5);
            email.User6.ShouldBe(user6);
            email.Birthdate.ShouldBeNull();
            email.UserEvent1.ShouldBe(userEvent1);
            email.UserEvent1Date.ShouldBeNull();
            email.UserEvent2.ShouldBe(userEvent2);
            email.UserEvent2Date.ShouldBeNull();
            email.Notes.ShouldBe(notes);
            email.Password.ShouldBe(password);
            email.BounceScore.ShouldBeNull();
            email.SoftBounceScore.ShouldBeNull();
            email.SMSOptIn.ShouldBe(sMSOptIn);
            email.CarrierCode.ShouldBe(carrierCode);
            email.FormatTypeCode.ShouldBe(formatTypeCode);
            email.SubscribeTypeCode.ShouldBe(subscribeTypeCode);
            email.DateAdded.ShouldBeNull();
            email.DateUpdated.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}