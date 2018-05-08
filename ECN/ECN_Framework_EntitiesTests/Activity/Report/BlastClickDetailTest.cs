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
    public class BlastClickDetailTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastClickDetail) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            var campaign = Fixture.Create<string>();
            var link = Fixture.Create<string>();
            var firstName = Fixture.Create<string>();
            var lastName = Fixture.Create<string>();
            var title = Fixture.Create<string>();
            var company = Fixture.Create<string>();
            var address = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var postalCode = Fixture.Create<string>();
            var country = Fixture.Create<string>();
            var phone = Fixture.Create<string>();
            var email = Fixture.Create<string>();
            var id = Fixture.Create<int>();

            // Act
            blastClickDetail.Campaign = campaign;
            blastClickDetail.Link = link;
            blastClickDetail.FirstName = firstName;
            blastClickDetail.LastName = lastName;
            blastClickDetail.Title = title;
            blastClickDetail.Company = company;
            blastClickDetail.Address = address;
            blastClickDetail.City = city;
            blastClickDetail.State = state;
            blastClickDetail.PostalCode = postalCode;
            blastClickDetail.Country = country;
            blastClickDetail.Phone = phone;
            blastClickDetail.Email = email;
            blastClickDetail.ID = id;

            // Assert
            blastClickDetail.Campaign.ShouldBe(campaign);
            blastClickDetail.Link.ShouldBe(link);
            blastClickDetail.FirstName.ShouldBe(firstName);
            blastClickDetail.LastName.ShouldBe(lastName);
            blastClickDetail.Title.ShouldBe(title);
            blastClickDetail.Company.ShouldBe(company);
            blastClickDetail.Address.ShouldBe(address);
            blastClickDetail.City.ShouldBe(city);
            blastClickDetail.State.ShouldBe(state);
            blastClickDetail.PostalCode.ShouldBe(postalCode);
            blastClickDetail.Country.ShouldBe(country);
            blastClickDetail.Phone.ShouldBe(phone);
            blastClickDetail.Email.ShouldBe(email);
            blastClickDetail.ID.ShouldBe(id);
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Address = Fixture.Create<string>();
            var stringType = blastClickDetail.Address.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Campaign) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Campaign_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Campaign = Fixture.Create<string>();
            var stringType = blastClickDetail.Campaign.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Campaign) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_CampaignNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCampaign = "CampaignNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameCampaign));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Campaign_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCampaign = "Campaign";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameCampaign);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.City = Fixture.Create<string>();
            var stringType = blastClickDetail.City.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Company) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Company_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Company = Fixture.Create<string>();
            var stringType = blastClickDetail.Company.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Company) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_CompanyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCompany = "CompanyNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameCompany));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Company_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCompany = "Company";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameCompany);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Country) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Country_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Country = Fixture.Create<string>();
            var stringType = blastClickDetail.Country.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Country) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_CountryNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCountry = "CountryNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameCountry));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Country_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCountry = "Country";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameCountry);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Email) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Email_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Email = Fixture.Create<string>();
            var stringType = blastClickDetail.Email.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Email) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_EmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmail = "EmailNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Email_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmail = "Email";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (FirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_FirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.FirstName = Fixture.Create<string>();
            var stringType = blastClickDetail.FirstName.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (FirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_FirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstNameNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_FirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFirstName = "FirstName";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (ID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_ID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.ID = Fixture.Create<int>();
            var intType = blastClickDetail.ID.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameID = "IDNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameID = "ID";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (LastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_LastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.LastName = Fixture.Create<string>();
            var stringType = blastClickDetail.LastName.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (LastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_LastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastNameNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_LastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastName = "LastName";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Link) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Link_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Link = Fixture.Create<string>();
            var stringType = blastClickDetail.Link.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Link) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_LinkNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLink = "LinkNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameLink));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Link_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLink = "Link";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameLink);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Phone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Phone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Phone = Fixture.Create<string>();
            var stringType = blastClickDetail.Phone.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Phone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_PhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePhone = "PhoneNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNamePhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Phone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePhone = "Phone";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNamePhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (PostalCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_PostalCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.PostalCode = Fixture.Create<string>();
            var stringType = blastClickDetail.PostalCode.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (PostalCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_PostalCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePostalCode = "PostalCodeNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNamePostalCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_PostalCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePostalCode = "PostalCode";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNamePostalCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.State = Fixture.Create<string>();
            var stringType = blastClickDetail.State.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Title) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Title_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastClickDetail = Fixture.Create<BlastClickDetail>();
            blastClickDetail.Title = Fixture.Create<string>();
            var stringType = blastClickDetail.Title.GetType();

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

        #region General Getters/Setters : Class (BlastClickDetail) => Property (Title) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Class_Invalid_Property_TitleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTitle = "TitleNotPresent";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();

            // Act , Assert
            Should.NotThrow(() => blastClickDetail.GetType().GetProperty(propertyNameTitle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastClickDetail_Title_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTitle = "Title";
            var blastClickDetail  = Fixture.Create<BlastClickDetail>();
            var propertyInfo  = blastClickDetail.GetType().GetProperty(propertyNameTitle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastClickDetail) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickDetail_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastClickDetail());
        }

        #endregion

        #region General Constructor : Class (BlastClickDetail) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickDetail_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastClickDetail = Fixture.CreateMany<BlastClickDetail>(2).ToList();
            var firstBlastClickDetail = instancesOfBlastClickDetail.FirstOrDefault();
            var lastBlastClickDetail = instancesOfBlastClickDetail.Last();

            // Act, Assert
            firstBlastClickDetail.ShouldNotBeNull();
            lastBlastClickDetail.ShouldNotBeNull();
            firstBlastClickDetail.ShouldNotBeSameAs(lastBlastClickDetail);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastClickDetail_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastClickDetail = new BlastClickDetail();
            var secondBlastClickDetail = new BlastClickDetail();
            var thirdBlastClickDetail = new BlastClickDetail();
            var fourthBlastClickDetail = new BlastClickDetail();
            var fifthBlastClickDetail = new BlastClickDetail();
            var sixthBlastClickDetail = new BlastClickDetail();

            // Act, Assert
            firstBlastClickDetail.ShouldNotBeNull();
            secondBlastClickDetail.ShouldNotBeNull();
            thirdBlastClickDetail.ShouldNotBeNull();
            fourthBlastClickDetail.ShouldNotBeNull();
            fifthBlastClickDetail.ShouldNotBeNull();
            sixthBlastClickDetail.ShouldNotBeNull();
            firstBlastClickDetail.ShouldNotBeSameAs(secondBlastClickDetail);
            thirdBlastClickDetail.ShouldNotBeSameAs(firstBlastClickDetail);
            fourthBlastClickDetail.ShouldNotBeSameAs(firstBlastClickDetail);
            fifthBlastClickDetail.ShouldNotBeSameAs(firstBlastClickDetail);
            sixthBlastClickDetail.ShouldNotBeSameAs(firstBlastClickDetail);
            sixthBlastClickDetail.ShouldNotBeSameAs(fourthBlastClickDetail);
        }

        #endregion

        #endregion

        #endregion
    }
}