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
    public class LinkDetailsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkDetails) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            var emailAddress = Fixture.Create<string>();
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

            // Act
            linkDetails.EmailAddress = emailAddress;
            linkDetails.Title = title;
            linkDetails.FirstName = firstName;
            linkDetails.LastName = lastName;
            linkDetails.FullName = fullName;
            linkDetails.Company = company;
            linkDetails.Occupation = occupation;
            linkDetails.Address = address;
            linkDetails.Address2 = address2;
            linkDetails.City = city;
            linkDetails.State = state;
            linkDetails.Zip = zip;
            linkDetails.Country = country;
            linkDetails.Voice = voice;

            // Assert
            linkDetails.EmailAddress.ShouldBe(emailAddress);
            linkDetails.Title.ShouldBe(title);
            linkDetails.FirstName.ShouldBe(firstName);
            linkDetails.LastName.ShouldBe(lastName);
            linkDetails.FullName.ShouldBe(fullName);
            linkDetails.Company.ShouldBe(company);
            linkDetails.Occupation.ShouldBe(occupation);
            linkDetails.Address.ShouldBe(address);
            linkDetails.Address2.ShouldBe(address2);
            linkDetails.City.ShouldBe(city);
            linkDetails.State.ShouldBe(state);
            linkDetails.Zip.ShouldBe(zip);
            linkDetails.Country.ShouldBe(country);
            linkDetails.Voice.ShouldBe(voice);
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Address = Fixture.Create<string>();
            var stringType = linkDetails.Address.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Address2) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Address2_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Address2 = Fixture.Create<string>();
            var stringType = linkDetails.Address2.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Address2) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_Address2NotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress2 = "Address2NotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameAddress2));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Address2_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress2 = "Address2";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameAddress2);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.City = Fixture.Create<string>();
            var stringType = linkDetails.City.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Company) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Company_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Company = Fixture.Create<string>();
            var stringType = linkDetails.Company.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Company) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_CompanyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompany = "CompanyNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameCompany));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Company_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompany = "Company";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameCompany);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Country) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Country_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Country = Fixture.Create<string>();
            var stringType = linkDetails.Country.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Country) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_CountryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCountry = "CountryNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameCountry));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Country_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCountry = "Country";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameCountry);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.EmailAddress = Fixture.Create<string>();
            var stringType = linkDetails.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (FirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_FirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.FirstName = Fixture.Create<string>();
            var stringType = linkDetails.FirstName.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (FirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_FirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstNameNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_FirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstName";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (FullName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_FullName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.FullName = Fixture.Create<string>();
            var stringType = linkDetails.FullName.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (FullName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_FullNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFullName = "FullNameNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameFullName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_FullName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFullName = "FullName";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameFullName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (LastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_LastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.LastName = Fixture.Create<string>();
            var stringType = linkDetails.LastName.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (LastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_LastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastNameNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_LastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastName";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Occupation) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Occupation_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Occupation = Fixture.Create<string>();
            var stringType = linkDetails.Occupation.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Occupation) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_OccupationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOccupation = "OccupationNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameOccupation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Occupation_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOccupation = "Occupation";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameOccupation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.State = Fixture.Create<string>();
            var stringType = linkDetails.State.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Title) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Title_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Title = Fixture.Create<string>();
            var stringType = linkDetails.Title.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Title) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_TitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTitle = "TitleNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTitle = "Title";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Voice) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Voice_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Voice = Fixture.Create<string>();
            var stringType = linkDetails.Voice.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Voice) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_VoiceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameVoice = "VoiceNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameVoice));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Voice_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameVoice = "Voice";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameVoice);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkDetails) => Property (Zip) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Zip_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkDetails = Fixture.Create<LinkDetails>();
            linkDetails.Zip = Fixture.Create<string>();
            var stringType = linkDetails.Zip.GetType();

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

        #region General Getters/Setters : Class (LinkDetails) => Property (Zip) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Class_Invalid_Property_ZipNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameZip = "ZipNotPresent";
            var linkDetails  = Fixture.Create<LinkDetails>();

            // Act , Assert
            Should.NotThrow(() => linkDetails.GetType().GetProperty(propertyNameZip));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkDetails_Zip_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameZip = "Zip";
            var linkDetails  = Fixture.Create<LinkDetails>();
            var propertyInfo  = linkDetails.GetType().GetProperty(propertyNameZip);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkDetails) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkDetails_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkDetails());
        }

        #endregion

        #region General Constructor : Class (LinkDetails) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkDetails_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkDetails = Fixture.CreateMany<LinkDetails>(2).ToList();
            var firstLinkDetails = instancesOfLinkDetails.FirstOrDefault();
            var lastLinkDetails = instancesOfLinkDetails.Last();

            // Act, Assert
            firstLinkDetails.ShouldNotBeNull();
            lastLinkDetails.ShouldNotBeNull();
            firstLinkDetails.ShouldNotBeSameAs(lastLinkDetails);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkDetails_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkDetails = new LinkDetails();
            var secondLinkDetails = new LinkDetails();
            var thirdLinkDetails = new LinkDetails();
            var fourthLinkDetails = new LinkDetails();
            var fifthLinkDetails = new LinkDetails();
            var sixthLinkDetails = new LinkDetails();

            // Act, Assert
            firstLinkDetails.ShouldNotBeNull();
            secondLinkDetails.ShouldNotBeNull();
            thirdLinkDetails.ShouldNotBeNull();
            fourthLinkDetails.ShouldNotBeNull();
            fifthLinkDetails.ShouldNotBeNull();
            sixthLinkDetails.ShouldNotBeNull();
            firstLinkDetails.ShouldNotBeSameAs(secondLinkDetails);
            thirdLinkDetails.ShouldNotBeSameAs(firstLinkDetails);
            fourthLinkDetails.ShouldNotBeSameAs(firstLinkDetails);
            fifthLinkDetails.ShouldNotBeSameAs(firstLinkDetails);
            sixthLinkDetails.ShouldNotBeSameAs(firstLinkDetails);
            sixthLinkDetails.ShouldNotBeSameAs(fourthLinkDetails);
        }

        #endregion

        #endregion

        #endregion
    }
}