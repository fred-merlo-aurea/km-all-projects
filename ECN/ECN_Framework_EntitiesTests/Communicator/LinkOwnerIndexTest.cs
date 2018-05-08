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
    public class LinkOwnerIndexTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (LinkOwnerIndex) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var linkOwnerIndexId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var linkOwnerName = Fixture.Create<string>();
            var linkOwnerCode = Fixture.Create<string>();
            var contactFirstName = Fixture.Create<string>();
            var contactLastName = Fixture.Create<string>();
            var contactPhone = Fixture.Create<string>();
            var contactEmail = Fixture.Create<string>();
            var address = Fixture.Create<string>();
            var city = Fixture.Create<string>();
            var state = Fixture.Create<string>();
            var isActive = Fixture.Create<bool?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            linkOwnerIndex.LinkOwnerIndexID = linkOwnerIndexId;
            linkOwnerIndex.CustomerID = customerId;
            linkOwnerIndex.LinkOwnerName = linkOwnerName;
            linkOwnerIndex.LinkOwnerCode = linkOwnerCode;
            linkOwnerIndex.ContactFirstName = contactFirstName;
            linkOwnerIndex.ContactLastName = contactLastName;
            linkOwnerIndex.ContactPhone = contactPhone;
            linkOwnerIndex.ContactEmail = contactEmail;
            linkOwnerIndex.Address = address;
            linkOwnerIndex.City = city;
            linkOwnerIndex.State = state;
            linkOwnerIndex.IsActive = isActive;
            linkOwnerIndex.CreatedUserID = createdUserId;
            linkOwnerIndex.UpdatedUserID = updatedUserId;
            linkOwnerIndex.IsDeleted = isDeleted;

            // Assert
            linkOwnerIndex.LinkOwnerIndexID.ShouldBe(linkOwnerIndexId);
            linkOwnerIndex.CustomerID.ShouldBe(customerId);
            linkOwnerIndex.LinkOwnerName.ShouldBe(linkOwnerName);
            linkOwnerIndex.LinkOwnerCode.ShouldBe(linkOwnerCode);
            linkOwnerIndex.ContactFirstName.ShouldBe(contactFirstName);
            linkOwnerIndex.ContactLastName.ShouldBe(contactLastName);
            linkOwnerIndex.ContactPhone.ShouldBe(contactPhone);
            linkOwnerIndex.ContactEmail.ShouldBe(contactEmail);
            linkOwnerIndex.Address.ShouldBe(address);
            linkOwnerIndex.City.ShouldBe(city);
            linkOwnerIndex.State.ShouldBe(state);
            linkOwnerIndex.IsActive.ShouldBe(isActive);
            linkOwnerIndex.CreatedUserID.ShouldBe(createdUserId);
            linkOwnerIndex.CreatedDate.ShouldBeNull();
            linkOwnerIndex.UpdatedUserID.ShouldBe(updatedUserId);
            linkOwnerIndex.UpdatedDate.ShouldBeNull();
            linkOwnerIndex.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (Address) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Address_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.Address = Fixture.Create<string>();
            var stringType = linkOwnerIndex.Address.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (Address) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_AddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAddress = "AddressNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Address_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAddress = "Address";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (City) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_City_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.City = Fixture.Create<string>();
            var stringType = linkOwnerIndex.City.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (City) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_CityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCity = "CityNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameCity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_City_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCity = "City";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameCity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactEmail) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactEmail_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.ContactEmail = Fixture.Create<string>();
            var stringType = linkOwnerIndex.ContactEmail.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_ContactEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactEmail = "ContactEmailNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameContactEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactEmail = "ContactEmail";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameContactEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactFirstName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactFirstName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.ContactFirstName = Fixture.Create<string>();
            var stringType = linkOwnerIndex.ContactFirstName.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactFirstName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_ContactFirstNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactFirstName = "ContactFirstNameNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameContactFirstName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactFirstName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactFirstName = "ContactFirstName";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameContactFirstName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactLastName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactLastName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.ContactLastName = Fixture.Create<string>();
            var stringType = linkOwnerIndex.ContactLastName.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactLastName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_ContactLastNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactLastName = "ContactLastNameNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameContactLastName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactLastName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactLastName = "ContactLastName";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameContactLastName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactPhone) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactPhone_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.ContactPhone = Fixture.Create<string>();
            var stringType = linkOwnerIndex.ContactPhone.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (ContactPhone) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_ContactPhoneNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContactPhone = "ContactPhoneNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameContactPhone));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_ContactPhone_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContactPhone = "ContactPhone";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameContactPhone);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkOwnerIndex.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkOwnerIndex, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkOwnerIndex.CreatedUserID = random;

            // Assert
            linkOwnerIndex.CreatedUserID.ShouldBe(random);
            linkOwnerIndex.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();

            // Act , Set
            linkOwnerIndex.CreatedUserID = null;

            // Assert
            linkOwnerIndex.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo = linkOwnerIndex.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkOwnerIndex, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkOwnerIndex.CreatedUserID.ShouldBeNull();
            linkOwnerIndex.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.CustomerID = Fixture.Create<int>();
            var intType = linkOwnerIndex.CustomerID.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (IsActive) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsActive_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkOwnerIndex.IsActive = random;

            // Assert
            linkOwnerIndex.IsActive.ShouldBe(random);
            linkOwnerIndex.IsActive.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsActive_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();

            // Act , Set
            linkOwnerIndex.IsActive = null;

            // Assert
            linkOwnerIndex.IsActive.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsActive_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsActive = "IsActive";
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo = linkOwnerIndex.GetType().GetProperty(propertyNameIsActive);

            // Act , Set
            propertyInfo.SetValue(linkOwnerIndex, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkOwnerIndex.IsActive.ShouldBeNull();
            linkOwnerIndex.IsActive.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var random = Fixture.Create<bool>();

            // Act , Set
            linkOwnerIndex.IsDeleted = random;

            // Assert
            linkOwnerIndex.IsDeleted.ShouldBe(random);
            linkOwnerIndex.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();

            // Act , Set
            linkOwnerIndex.IsDeleted = null;

            // Assert
            linkOwnerIndex.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo = linkOwnerIndex.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(linkOwnerIndex, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkOwnerIndex.IsDeleted.ShouldBeNull();
            linkOwnerIndex.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (LinkOwnerCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_LinkOwnerCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.LinkOwnerCode = Fixture.Create<string>();
            var stringType = linkOwnerIndex.LinkOwnerCode.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (LinkOwnerCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_LinkOwnerCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerCode = "LinkOwnerCodeNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameLinkOwnerCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_LinkOwnerCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerCode = "LinkOwnerCode";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameLinkOwnerCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (LinkOwnerIndexID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_LinkOwnerIndexID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.LinkOwnerIndexID = Fixture.Create<int>();
            var intType = linkOwnerIndex.LinkOwnerIndexID.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (LinkOwnerIndexID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_LinkOwnerIndexIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerIndexID = "LinkOwnerIndexIDNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameLinkOwnerIndexID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_LinkOwnerIndexID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerIndexID = "LinkOwnerIndexID";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameLinkOwnerIndexID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (LinkOwnerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_LinkOwnerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.LinkOwnerName = Fixture.Create<string>();
            var stringType = linkOwnerIndex.LinkOwnerName.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (LinkOwnerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_LinkOwnerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerName = "LinkOwnerNameNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameLinkOwnerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_LinkOwnerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLinkOwnerName = "LinkOwnerName";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameLinkOwnerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (State) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_State_Property_String_Type_Verify_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            linkOwnerIndex.State = Fixture.Create<string>();
            var stringType = linkOwnerIndex.State.GetType();

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

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (State) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_StateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameState = "StateNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameState));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_State_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameState = "State";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameState);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = linkOwnerIndex.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(linkOwnerIndex, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var random = Fixture.Create<int>();

            // Act , Set
            linkOwnerIndex.UpdatedUserID = random;

            // Assert
            linkOwnerIndex.UpdatedUserID.ShouldBe(random);
            linkOwnerIndex.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();

            // Act , Set
            linkOwnerIndex.UpdatedUserID = null;

            // Assert
            linkOwnerIndex.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkOwnerIndex = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo = linkOwnerIndex.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(linkOwnerIndex, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            linkOwnerIndex.UpdatedUserID.ShouldBeNull();
            linkOwnerIndex.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (LinkOwnerIndex) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();

            // Act , Assert
            Should.NotThrow(() => linkOwnerIndex.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void LinkOwnerIndex_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var linkOwnerIndex  = Fixture.Create<LinkOwnerIndex>();
            var propertyInfo  = linkOwnerIndex.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (LinkOwnerIndex) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkOwnerIndex_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new LinkOwnerIndex());
        }

        #endregion

        #region General Constructor : Class (LinkOwnerIndex) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkOwnerIndex_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfLinkOwnerIndex = Fixture.CreateMany<LinkOwnerIndex>(2).ToList();
            var firstLinkOwnerIndex = instancesOfLinkOwnerIndex.FirstOrDefault();
            var lastLinkOwnerIndex = instancesOfLinkOwnerIndex.Last();

            // Act, Assert
            firstLinkOwnerIndex.ShouldNotBeNull();
            lastLinkOwnerIndex.ShouldNotBeNull();
            firstLinkOwnerIndex.ShouldNotBeSameAs(lastLinkOwnerIndex);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkOwnerIndex_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstLinkOwnerIndex = new LinkOwnerIndex();
            var secondLinkOwnerIndex = new LinkOwnerIndex();
            var thirdLinkOwnerIndex = new LinkOwnerIndex();
            var fourthLinkOwnerIndex = new LinkOwnerIndex();
            var fifthLinkOwnerIndex = new LinkOwnerIndex();
            var sixthLinkOwnerIndex = new LinkOwnerIndex();

            // Act, Assert
            firstLinkOwnerIndex.ShouldNotBeNull();
            secondLinkOwnerIndex.ShouldNotBeNull();
            thirdLinkOwnerIndex.ShouldNotBeNull();
            fourthLinkOwnerIndex.ShouldNotBeNull();
            fifthLinkOwnerIndex.ShouldNotBeNull();
            sixthLinkOwnerIndex.ShouldNotBeNull();
            firstLinkOwnerIndex.ShouldNotBeSameAs(secondLinkOwnerIndex);
            thirdLinkOwnerIndex.ShouldNotBeSameAs(firstLinkOwnerIndex);
            fourthLinkOwnerIndex.ShouldNotBeSameAs(firstLinkOwnerIndex);
            fifthLinkOwnerIndex.ShouldNotBeSameAs(firstLinkOwnerIndex);
            sixthLinkOwnerIndex.ShouldNotBeSameAs(firstLinkOwnerIndex);
            sixthLinkOwnerIndex.ShouldNotBeSameAs(fourthLinkOwnerIndex);
        }

        #endregion

        #region General Constructor : Class (LinkOwnerIndex) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_LinkOwnerIndex_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var linkOwnerIndexId = -1;
            var customerId = -1;
            var linkOwnerName = string.Empty;
            var linkOwnerCode = string.Empty;
            var contactFirstName = string.Empty;
            var contactLastName = string.Empty;
            var contactPhone = string.Empty;
            var contactEmail = string.Empty;
            var address = string.Empty;
            var city = string.Empty;
            var state = string.Empty;

            // Act
            var linkOwnerIndex = new LinkOwnerIndex();

            // Assert
            linkOwnerIndex.LinkOwnerIndexID.ShouldBe(linkOwnerIndexId);
            linkOwnerIndex.CustomerID.ShouldBe(customerId);
            linkOwnerIndex.LinkOwnerName.ShouldBe(linkOwnerName);
            linkOwnerIndex.LinkOwnerCode.ShouldBe(linkOwnerCode);
            linkOwnerIndex.ContactFirstName.ShouldBe(contactFirstName);
            linkOwnerIndex.ContactLastName.ShouldBe(contactLastName);
            linkOwnerIndex.ContactPhone.ShouldBe(contactPhone);
            linkOwnerIndex.ContactEmail.ShouldBe(contactEmail);
            linkOwnerIndex.Address.ShouldBe(address);
            linkOwnerIndex.City.ShouldBe(city);
            linkOwnerIndex.State.ShouldBe(state);
            linkOwnerIndex.IsActive.ShouldBeNull();
            linkOwnerIndex.CreatedUserID.ShouldBeNull();
            linkOwnerIndex.CreatedDate.ShouldBeNull();
            linkOwnerIndex.UpdatedUserID.ShouldBeNull();
            linkOwnerIndex.UpdatedDate.ShouldBeNull();
            linkOwnerIndex.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}