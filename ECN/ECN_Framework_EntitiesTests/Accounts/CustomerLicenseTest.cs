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
    public class CustomerLicenseTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerLicense  = new CustomerLicense();
            var cLID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var quoteItemID = Fixture.Create<int>();
            var licenseTypeCode = Fixture.Create<string>();
            var licenseLevel = Fixture.Create<string>();
            var quantity = Fixture.Create<int?>();
            var used = Fixture.Create<int?>();
            var expirationDate = Fixture.Create<DateTime?>();
            var addDate = Fixture.Create<DateTime?>();
            var isActive = Fixture.Create<bool>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            customerLicense.CLID = cLID;
            customerLicense.CustomerID = customerID;
            customerLicense.QuoteItemID = quoteItemID;
            customerLicense.LicenseTypeCode = licenseTypeCode;
            customerLicense.LicenseLevel = licenseLevel;
            customerLicense.Quantity = quantity;
            customerLicense.Used = used;
            customerLicense.ExpirationDate = expirationDate;
            customerLicense.AddDate = addDate;
            customerLicense.IsActive = isActive;
            customerLicense.CreatedUserID = createdUserID;
            customerLicense.CreatedDate = createdDate;
            customerLicense.UpdatedUserID = updatedUserID;
            customerLicense.UpdatedDate = updatedDate;
            customerLicense.IsDeleted = isDeleted;

            // Assert
            customerLicense.CLID.ShouldBe(cLID);
            customerLicense.CustomerID.ShouldBe(customerID);
            customerLicense.QuoteItemID.ShouldBe(quoteItemID);
            customerLicense.LicenseTypeCode.ShouldBe(licenseTypeCode);
            customerLicense.LicenseLevel.ShouldBe(licenseLevel);
            customerLicense.Quantity.ShouldBe(quantity);
            customerLicense.Used.ShouldBe(used);
            customerLicense.ExpirationDate.ShouldBe(expirationDate);
            customerLicense.AddDate.ShouldBe(addDate);
            customerLicense.IsActive.ShouldBe(isActive);
            customerLicense.CreatedUserID.ShouldBe(createdUserID);
            customerLicense.CreatedDate.ShouldBe(createdDate);
            customerLicense.UpdatedUserID.ShouldBe(updatedUserID);
            customerLicense.UpdatedDate.ShouldBe(updatedDate);
            customerLicense.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region bool property type test : CustomerLicense => IsActive

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Bool_Type_Verify_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var boolType = customerLicense.IsActive.GetType();

            // Act
            var isTypeBool = typeof(bool).Equals(boolType);    
            var isTypeNullableBool = typeof(bool?).Equals(boolType);
            var isTypeString = typeof(string).Equals(boolType);
            var isTypeInt = typeof(int).Equals(boolType);
            var isTypeDecimal = typeof(decimal).Equals(boolType);
            var isTypeLong = typeof(long).Equals(boolType);
            var isTypeDouble = typeof(double).Equals(boolType);
            var isTypeFloat = typeof(float).Equals(boolType);
            var isTypeIntNullable = typeof(int?).Equals(boolType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(boolType);
            var isTypeLongNullable = typeof(long?).Equals(boolType);
            var isTypeDoubleNullable = typeof(double?).Equals(boolType);
            var isTypeFloatNullable = typeof(float?).Equals(boolType);


            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_IsActive_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsActive = "IsActive";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constIsActive));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsActive_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constIsActive = "IsActive";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerLicense => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerLicense.IsDeleted = random;

            // Assert
            customerLicense.IsDeleted.ShouldBe(random);
            customerLicense.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();    

            // Act , Set
            customerLicense.IsDeleted = null;

            // Assert
            customerLicense.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var propertyInfo = customerLicense.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customerLicense, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLicense.IsDeleted.ShouldBeNull();
            customerLicense.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerLicense => AddDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AddDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constAddDate = "AddDate";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerLicense.GetType().GetProperty(constAddDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerLicense, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_AddDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constAddDate = "AddDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constAddDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_AddDate_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constAddDate = "AddDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constAddDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerLicense => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerLicense.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerLicense, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerLicense => ExpirationDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ExpirationDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constExpirationDate = "ExpirationDate";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerLicense.GetType().GetProperty(constExpirationDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerLicense, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_ExpirationDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constExpirationDate = "ExpirationDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constExpirationDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ExpirationDate_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constExpirationDate = "ExpirationDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constExpirationDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerLicense => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerLicense.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerLicense, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerLicense => CLID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CLID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var intType = customerLicense.CLID.GetType();

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
        public void CustomerLicense_Class_Invalid_Property_CLID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCLID = "CLID";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constCLID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CLID_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constCLID = "CLID";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constCLID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerLicense => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var intType = customerLicense.CustomerID.GetType();

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
        public void CustomerLicense_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerLicense => QuoteItemID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteItemID_Int_Type_Verify_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var intType = customerLicense.QuoteItemID.GetType();

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
        public void CustomerLicense_Class_Invalid_Property_QuoteItemID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuoteItemID = "QuoteItemID";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constQuoteItemID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteItemID_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constQuoteItemID = "QuoteItemID";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constQuoteItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerLicense => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLicense.CreatedUserID = random;

            // Assert
            customerLicense.CreatedUserID.ShouldBe(random);
            customerLicense.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();    

            // Act , Set
            customerLicense.CreatedUserID = null;

            // Assert
            customerLicense.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var propertyInfo = customerLicense.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerLicense, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLicense.CreatedUserID.ShouldBeNull();
            customerLicense.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerLicense => Quantity

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Data_Without_Null_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLicense.Quantity = random;

            // Assert
            customerLicense.Quantity.ShouldBe(random);
            customerLicense.Quantity.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Only_Null_Data_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();    

            // Act , Set
            customerLicense.Quantity = null;

            // Assert
            customerLicense.Quantity.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constQuantity = "Quantity";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var propertyInfo = customerLicense.GetType().GetProperty(constQuantity);

            // Act , Set
            propertyInfo.SetValue(customerLicense, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLicense.Quantity.ShouldBeNull();
            customerLicense.Quantity.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_Quantity_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuantity = "Quantity";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constQuantity));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constQuantity = "Quantity";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constQuantity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerLicense => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLicense.UpdatedUserID = random;

            // Assert
            customerLicense.UpdatedUserID.ShouldBe(random);
            customerLicense.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();    

            // Act , Set
            customerLicense.UpdatedUserID = null;

            // Assert
            customerLicense.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var propertyInfo = customerLicense.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerLicense, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLicense.UpdatedUserID.ShouldBeNull();
            customerLicense.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : CustomerLicense => Used

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Used_Data_Without_Null_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerLicense.Used = random;

            // Assert
            customerLicense.Used.ShouldBe(random);
            customerLicense.Used.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Used_Only_Null_Data_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();    

            // Act , Set
            customerLicense.Used = null;

            // Assert
            customerLicense.Used.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Used_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUsed = "Used";
            var customerLicense = Fixture.Create<CustomerLicense>();
            var propertyInfo = customerLicense.GetType().GetProperty(constUsed);

            // Act , Set
            propertyInfo.SetValue(customerLicense, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerLicense.Used.ShouldBeNull();
            customerLicense.Used.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerLicense_Class_Invalid_Property_Used_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUsed = "Used";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constUsed));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Used_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constUsed = "Used";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constUsed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerLicense => LicenseLevel

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseLevel_String_Type_Verify_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var stringType = customerLicense.LicenseLevel.GetType();

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
        public void CustomerLicense_Class_Invalid_Property_LicenseLevel_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLicenseLevel = "LicenseLevel";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constLicenseLevel));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseLevel_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constLicenseLevel = "LicenseLevel";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constLicenseLevel);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerLicense => LicenseTypeCode

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseTypeCode_String_Type_Verify_Test()
        {
            // Arrange
            var customerLicense = Fixture.Create<CustomerLicense>();
            var stringType = customerLicense.LicenseTypeCode.GetType();

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
        public void CustomerLicense_Class_Invalid_Property_LicenseTypeCode_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLicenseTypeCode = "LicenseTypeCode";
            var customerLicense  = Fixture.Create<CustomerLicense>();

            // Act , Assert
            Should.NotThrow(() => customerLicense.GetType().GetProperty(constLicenseTypeCode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseTypeCode_Is_Present_In_CustomerLicense_Class_As_Public_Test()
        {
            // Arrange
            const string constLicenseTypeCode = "LicenseTypeCode";
            var customerLicense  = Fixture.Create<CustomerLicense>();
            var propertyInfo  = customerLicense.GetType().GetProperty(constLicenseTypeCode);

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
            Should.NotThrow(() => new CustomerLicense());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<CustomerLicense>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #region General Constructor Pattern : Default Assignment Test

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Instantiated_With_Default_Assignments_NoChange_DefaultValues()
        {
            // Arrange
            var cLID = -1;
            var customerID = -1;
            var quoteItemID = -1;
            string licenseTypeCode = null;
            string licenseLevel = null;
            int? quantity = null;
            int? used = null;
            DateTime? expirationDate = null;
            DateTime? addDate = null;
            var isActive = true;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var customerLicense = new CustomerLicense();    

            // Assert
            customerLicense.CLID.ShouldBe(cLID);
            customerLicense.CustomerID.ShouldBe(customerID);
            customerLicense.QuoteItemID.ShouldBe(quoteItemID);
            customerLicense.LicenseTypeCode.ShouldBeNull();
            customerLicense.LicenseLevel.ShouldBeNull();
            customerLicense.Quantity.ShouldBeNull();
            customerLicense.Used.ShouldBeNull();
            customerLicense.ExpirationDate.ShouldBeNull();
            customerLicense.AddDate.ShouldBeNull();
            customerLicense.IsActive.ShouldBeTrue();
            customerLicense.CreatedUserID.ShouldBeNull();
            customerLicense.CreatedDate.ShouldBeNull();
            customerLicense.UpdatedUserID.ShouldBeNull();
            customerLicense.UpdatedDate.ShouldBeNull();
            customerLicense.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}