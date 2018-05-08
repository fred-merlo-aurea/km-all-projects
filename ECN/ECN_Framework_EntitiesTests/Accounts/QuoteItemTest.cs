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
    public class QuoteItemTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (QuoteItem) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            var quoteItemId = Fixture.Create<int>();
            var quoteId = Fixture.Create<int>();
            var code = Fixture.Create<string>();
            var name = Fixture.Create<string>();
            var description = Fixture.Create<string>();
            var quantity = Fixture.Create<int>();
            var rate = Fixture.Create<double>();
            var discountRate = Fixture.Create<double?>();
            var licenseType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum>();
            var priceType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum>();
            var frequencyType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum>();
            var isCustomerCredit = Fixture.Create<bool>();
            var isActive = Fixture.Create<bool>();
            var productIDs = Fixture.Create<string>();
            var productFeatureIDs = Fixture.Create<string>();
            var services = Fixture.Create<string>();
            var recurringProfileId = Fixture.Create<string>();
            var customerId = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            quoteItem.QuoteItemID = quoteItemId;
            quoteItem.QuoteID = quoteId;
            quoteItem.Code = code;
            quoteItem.Name = name;
            quoteItem.Description = description;
            quoteItem.Quantity = quantity;
            quoteItem.Rate = rate;
            quoteItem.DiscountRate = discountRate;
            quoteItem.LicenseType = licenseType;
            quoteItem.PriceType = priceType;
            quoteItem.FrequencyType = frequencyType;
            quoteItem.IsCustomerCredit = isCustomerCredit;
            quoteItem.IsActive = isActive;
            quoteItem.ProductIDs = productIDs;
            quoteItem.ProductFeatureIDs = productFeatureIDs;
            quoteItem.Services = services;
            quoteItem.RecurringProfileID = recurringProfileId;
            quoteItem.CustomerID = customerId;
            quoteItem.CreatedUserID = createdUserId;
            quoteItem.CreatedDate = createdDate;
            quoteItem.UpdatedUserID = updatedUserId;
            quoteItem.UpdatedDate = updatedDate;
            quoteItem.IsDeleted = isDeleted;

            // Assert
            quoteItem.QuoteItemID.ShouldBe(quoteItemId);
            quoteItem.QuoteID.ShouldBe(quoteId);
            quoteItem.Code.ShouldBe(code);
            quoteItem.Name.ShouldBe(name);
            quoteItem.Description.ShouldBe(description);
            quoteItem.Quantity.ShouldBe(quantity);
            quoteItem.Rate.ShouldBe(rate);
            quoteItem.DiscountRate.ShouldBe(discountRate);
            quoteItem.LicenseType.ShouldBe(licenseType);
            quoteItem.PriceType.ShouldBe(priceType);
            quoteItem.FrequencyType.ShouldBe(frequencyType);
            quoteItem.IsCustomerCredit.ShouldBe(isCustomerCredit);
            quoteItem.IsActive.ShouldBe(isActive);
            quoteItem.ProductIDs.ShouldBe(productIDs);
            quoteItem.ProductFeatureIDs.ShouldBe(productFeatureIDs);
            quoteItem.Services.ShouldBe(services);
            quoteItem.RecurringProfileID.ShouldBe(recurringProfileId);
            quoteItem.CustomerID.ShouldBe(customerId);
            quoteItem.CreatedUserID.ShouldBe(createdUserId);
            quoteItem.CreatedDate.ShouldBe(createdDate);
            quoteItem.UpdatedUserID.ShouldBe(updatedUserId);
            quoteItem.UpdatedDate.ShouldBe(updatedDate);
            quoteItem.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (Code) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Code_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.Code = Fixture.Create<string>();
            var stringType = quoteItem.Code.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (Code) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_CodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCode = "CodeNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Code_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCode = "Code";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var quoteItem = Fixture.Create<QuoteItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            quoteItem.CreatedUserID = random;

            // Assert
            quoteItem.CreatedUserID.ShouldBe(random);
            quoteItem.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();    

            // Act , Set
            quoteItem.CreatedUserID = null;

            // Assert
            quoteItem.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var quoteItem = Fixture.Create<QuoteItem>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(quoteItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteItem.CreatedUserID.ShouldBeNull();
            quoteItem.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            quoteItem.CustomerID = random;

            // Assert
            quoteItem.CustomerID.ShouldBe(random);
            quoteItem.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();    

            // Act , Set
            quoteItem.CustomerID = null;

            // Assert
            quoteItem.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var quoteItem = Fixture.Create<QuoteItem>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(quoteItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteItem.CustomerID.ShouldBeNull();
            quoteItem.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (Description) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Description_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.Description = Fixture.Create<string>();
            var stringType = quoteItem.Description.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (Description) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_DescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDescription = "DescriptionNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Description_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDescription = "Description";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (DiscountRate) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_DiscountRate_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            var random = Fixture.Create<double>();

            // Act , Set
            quoteItem.DiscountRate = random;

            // Assert
            quoteItem.DiscountRate.ShouldBe(random);
            quoteItem.DiscountRate.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_DiscountRate_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();    

            // Act , Set
            quoteItem.DiscountRate = null;

            // Assert
            quoteItem.DiscountRate.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_DiscountRate_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDiscountRate = "DiscountRate";
            var quoteItem = Fixture.Create<QuoteItem>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameDiscountRate);

            // Act , Set
            propertyInfo.SetValue(quoteItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteItem.DiscountRate.ShouldBeNull();
            quoteItem.DiscountRate.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (DiscountRate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_DiscountRateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDiscountRate = "DiscountRateNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameDiscountRate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_DiscountRate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDiscountRate = "DiscountRate";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameDiscountRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (FrequencyType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_FrequencyType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFrequencyType = "FrequencyType";
            var quoteItem = Fixture.Create<QuoteItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameFrequencyType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (FrequencyType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_FrequencyTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFrequencyType = "FrequencyTypeNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameFrequencyType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_FrequencyType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFrequencyType = "FrequencyType";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameFrequencyType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (IsActive) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsActive_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.IsActive = Fixture.Create<bool>();
            var boolType = quoteItem.IsActive.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (IsActive) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_IsActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActiveNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameIsActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsActive_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsActive = "IsActive";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameIsActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (IsCustomerCredit) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsCustomerCredit_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.IsCustomerCredit = Fixture.Create<bool>();
            var boolType = quoteItem.IsCustomerCredit.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (IsCustomerCredit) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_IsCustomerCreditNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsCustomerCredit = "IsCustomerCreditNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameIsCustomerCredit));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsCustomerCredit_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsCustomerCredit = "IsCustomerCredit";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameIsCustomerCredit);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quoteItem.IsDeleted = random;

            // Assert
            quoteItem.IsDeleted.ShouldBe(random);
            quoteItem.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();    

            // Act , Set
            quoteItem.IsDeleted = null;

            // Assert
            quoteItem.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var quoteItem = Fixture.Create<QuoteItem>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(quoteItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteItem.IsDeleted.ShouldBeNull();
            quoteItem.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (LicenseType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_LicenseType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameLicenseType = "LicenseType";
            var quoteItem = Fixture.Create<QuoteItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameLicenseType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (LicenseType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_LicenseTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLicenseType = "LicenseTypeNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameLicenseType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_LicenseType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLicenseType = "LicenseType";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameLicenseType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.Name = Fixture.Create<string>();
            var stringType = quoteItem.Name.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (PriceType) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_PriceType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNamePriceType = "PriceType";
            var quoteItem = Fixture.Create<QuoteItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNamePriceType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (PriceType) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_PriceTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePriceType = "PriceTypeNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNamePriceType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_PriceType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePriceType = "PriceType";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNamePriceType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (ProductFeatureIDs) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_ProductFeatureIDs_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.ProductFeatureIDs = Fixture.Create<string>();
            var stringType = quoteItem.ProductFeatureIDs.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (ProductFeatureIDs) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_ProductFeatureIDsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductFeatureIDs = "ProductFeatureIDsNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameProductFeatureIDs));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_ProductFeatureIDs_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductFeatureIDs = "ProductFeatureIDs";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameProductFeatureIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (ProductIDs) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_ProductIDs_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.ProductIDs = Fixture.Create<string>();
            var stringType = quoteItem.ProductIDs.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (ProductIDs) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_ProductIDsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductIDs = "ProductIDsNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameProductIDs));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_ProductIDs_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductIDs = "ProductIDs";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameProductIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (Quantity) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Quantity_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.Quantity = Fixture.Create<int>();
            var intType = quoteItem.Quantity.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (Quantity) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_QuantityNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuantity = "QuantityNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameQuantity));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Quantity_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuantity = "Quantity";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameQuantity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (QuoteID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_QuoteID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.QuoteID = Fixture.Create<int>();
            var intType = quoteItem.QuoteID.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (QuoteID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_QuoteIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuoteID = "QuoteIDNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameQuoteID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_QuoteID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuoteID = "QuoteID";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameQuoteID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (QuoteItemID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_QuoteItemID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.QuoteItemID = Fixture.Create<int>();
            var intType = quoteItem.QuoteItemID.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (QuoteItemID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_QuoteItemIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuoteItemID = "QuoteItemIDNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameQuoteItemID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_QuoteItemID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuoteItemID = "QuoteItemID";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameQuoteItemID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (Rate) (Type : double) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Rate_Property_Double_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.Rate = Fixture.Create<double>();
            var doubleType = quoteItem.Rate.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (Rate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_RateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRate = "RateNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameRate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Rate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRate = "Rate";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (RecurringProfileID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_RecurringProfileID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.RecurringProfileID = Fixture.Create<string>();
            var stringType = quoteItem.RecurringProfileID.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (RecurringProfileID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_RecurringProfileIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRecurringProfileID = "RecurringProfileIDNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameRecurringProfileID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_RecurringProfileID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRecurringProfileID = "RecurringProfileID";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameRecurringProfileID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (Services) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Services_Property_String_Type_Verify_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            quoteItem.Services = Fixture.Create<string>();
            var stringType = quoteItem.Services.GetType();

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

        #region General Getters/Setters : Class (QuoteItem) => Property (Services) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_ServicesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameServices = "ServicesNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameServices));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Services_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameServices = "Services";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameServices);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var quoteItem = Fixture.Create<QuoteItem>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteItem, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();
            var random = Fixture.Create<int>();

            // Act , Set
            quoteItem.UpdatedUserID = random;

            // Assert
            quoteItem.UpdatedUserID.ShouldBe(random);
            quoteItem.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var quoteItem = Fixture.Create<QuoteItem>();    

            // Act , Set
            quoteItem.UpdatedUserID = null;

            // Assert
            quoteItem.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var quoteItem = Fixture.Create<QuoteItem>();
            var propertyInfo = quoteItem.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(quoteItem, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteItem.UpdatedUserID.ShouldBeNull();
            quoteItem.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (QuoteItem) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var quoteItem  = Fixture.Create<QuoteItem>();

            // Act , Assert
            Should.NotThrow(() => quoteItem.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteItem_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var quoteItem  = Fixture.Create<QuoteItem>();
            var propertyInfo  = quoteItem.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (QuoteItem) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuoteItem_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new QuoteItem());
        }

        #endregion

        #region General Constructor : Class (QuoteItem) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuoteItem_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var frequency = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum>();
            var code = Fixture.Create<string>();
            var name = Fixture.Create<string>();
            var description = Fixture.Create<string>();
            var quantity = Fixture.Create<int>();
            var rate = Fixture.Create<double>();
            var licenseType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum>();
            var priceType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum>();

            // Act, Assert
            Should.NotThrow(() => new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType));
        }

        #endregion

        #region General Constructor : Class (QuoteItem) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuoteItem_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfQuoteItem = Fixture.CreateMany<QuoteItem>(2).ToList();
            var firstQuoteItem = instancesOfQuoteItem.FirstOrDefault();
            var lastQuoteItem = instancesOfQuoteItem.Last();

            // Act, Assert
            firstQuoteItem.ShouldNotBeNull();
            lastQuoteItem.ShouldNotBeNull();
            firstQuoteItem.ShouldNotBeSameAs(lastQuoteItem);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuoteItem_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstQuoteItem = new QuoteItem();
            var secondQuoteItem = new QuoteItem();
            var thirdQuoteItem = new QuoteItem();
            var fourthQuoteItem = new QuoteItem();
            var fifthQuoteItem = new QuoteItem();
            var sixthQuoteItem = new QuoteItem();

            // Act, Assert
            firstQuoteItem.ShouldNotBeNull();
            secondQuoteItem.ShouldNotBeNull();
            thirdQuoteItem.ShouldNotBeNull();
            fourthQuoteItem.ShouldNotBeNull();
            fifthQuoteItem.ShouldNotBeNull();
            sixthQuoteItem.ShouldNotBeNull();
            firstQuoteItem.ShouldNotBeSameAs(secondQuoteItem);
            thirdQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            fourthQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            fifthQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            sixthQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            sixthQuoteItem.ShouldNotBeSameAs(fourthQuoteItem);
        }

        #endregion

        #region General Constructor : Class (QuoteItem) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuoteItem_5_Objects_Creation_8_Paramters_Test()
        {
            // Arrange
        	var frequency = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum>();
        	var code = Fixture.Create<string>();
        	var name = Fixture.Create<string>();
        	var description = Fixture.Create<string>();
        	var quantity = Fixture.Create<int>();
        	var rate = Fixture.Create<double>();
        	var licenseType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum>();
        	var priceType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum>();
            var firstQuoteItem = new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType);
            var secondQuoteItem = new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType);
            var thirdQuoteItem = new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType);
            var fourthQuoteItem = new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType);
            var fifthQuoteItem = new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType);
            var sixthQuoteItem = new QuoteItem(frequency, code, name, description, quantity, rate, licenseType, priceType);

            // Act, Assert
            firstQuoteItem.ShouldNotBeNull();
            secondQuoteItem.ShouldNotBeNull();
            thirdQuoteItem.ShouldNotBeNull();
            fourthQuoteItem.ShouldNotBeNull();
            fifthQuoteItem.ShouldNotBeNull();
            sixthQuoteItem.ShouldNotBeNull();
            firstQuoteItem.ShouldNotBeSameAs(secondQuoteItem);
            thirdQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            fourthQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            fifthQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            sixthQuoteItem.ShouldNotBeSameAs(firstQuoteItem);
            sixthQuoteItem.ShouldNotBeSameAs(fourthQuoteItem);
        }

        #endregion

        #region General Constructor : Class (QuoteItem) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_QuoteItem_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var quoteItemId = -1;
            var quoteId = -1;
            var name = string.Empty;
            var description = string.Empty;
            var quantity = -1;
            var rate = -1;
            var licenseType = ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum.AnnualTechAccess;
            var priceType = ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum.OneTime;
            var frequencyType = ECN_Framework_Common.Objects.Accounts.Enums.FrequencyTypeEnum.Annual;
            var isCustomerCredit = false;
            var isActive = true;
            var productIDs = string.Empty;
            var productFeatureIDs = string.Empty;
            var services = string.Empty;
            var recurringProfileId = string.Empty;

            // Act
            var quoteItem = new QuoteItem();

            // Assert
            quoteItem.QuoteItemID.ShouldBe(quoteItemId);
            quoteItem.QuoteID.ShouldBe(quoteId);
            quoteItem.Code.ShouldBeNull();
            quoteItem.Name.ShouldBe(name);
            quoteItem.Description.ShouldBe(description);
            quoteItem.Quantity.ShouldBe(quantity);
            quoteItem.Rate.ShouldBe(rate);
            quoteItem.DiscountRate.ShouldBeNull();
            quoteItem.LicenseType.ShouldBe(licenseType);
            quoteItem.PriceType.ShouldBe(priceType);
            quoteItem.FrequencyType.ShouldBe(frequencyType);
            quoteItem.IsCustomerCredit.ShouldBeFalse();
            quoteItem.IsActive.ShouldBeTrue();
            quoteItem.ProductIDs.ShouldBe(productIDs);
            quoteItem.ProductFeatureIDs.ShouldBe(productFeatureIDs);
            quoteItem.Services.ShouldBe(services);
            quoteItem.RecurringProfileID.ShouldBe(recurringProfileId);
            quoteItem.CustomerID.ShouldBeNull();
            quoteItem.CreatedUserID.ShouldBeNull();
            quoteItem.CreatedDate.ShouldBeNull();
            quoteItem.UpdatedUserID.ShouldBeNull();
            quoteItem.UpdatedDate.ShouldBeNull();
            quoteItem.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}