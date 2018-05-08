using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class QuoteOptionTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var quoteOption  = new QuoteOption();
            var quoteOptionID = Fixture.Create<int>();
            var baseChannelID = Fixture.Create<int>();
            var code = Fixture.Create<string>();
            var name = Fixture.Create<string>();
            var description = Fixture.Create<string>();
            var quantity = Fixture.Create<int>();
            var rate = Fixture.Create<double>();
            var discountRate = Fixture.Create<double?>();
            var licenseType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum>();
            var priceType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum>();
            var isCustomerCredit = Fixture.Create<bool>();
            var productIDs = Fixture.Create<string>();
            var productFeatureIDs = Fixture.Create<string>();
            var services = Fixture.Create<string>();
            var customerID = Fixture.Create<int?>();
            var createdUserID = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserID = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            quoteOption.QuoteOptionID = quoteOptionID;
            quoteOption.BaseChannelID = baseChannelID;
            quoteOption.Code = code;
            quoteOption.Name = name;
            quoteOption.Description = description;
            quoteOption.Quantity = quantity;
            quoteOption.Rate = rate;
            quoteOption.DiscountRate = discountRate;
            quoteOption.LicenseType = licenseType;
            quoteOption.PriceType = priceType;
            quoteOption.IsCustomerCredit = isCustomerCredit;
            quoteOption.ProductIDs = productIDs;
            quoteOption.ProductFeatureIDs = productFeatureIDs;
            quoteOption.Services = services;
            quoteOption.CustomerID = customerID;
            quoteOption.CreatedUserID = createdUserID;
            quoteOption.CreatedDate = createdDate;
            quoteOption.UpdatedUserID = updatedUserID;
            quoteOption.UpdatedDate = updatedDate;
            quoteOption.IsDeleted = isDeleted;

            // Assert
            quoteOption.QuoteOptionID.ShouldBe(quoteOptionID);
            quoteOption.BaseChannelID.ShouldBe(baseChannelID);
            quoteOption.Code.ShouldBe(code);
            quoteOption.Name.ShouldBe(name);
            quoteOption.Description.ShouldBe(description);
            quoteOption.Quantity.ShouldBe(quantity);
            quoteOption.Rate.ShouldBe(rate);
            quoteOption.DiscountRate.ShouldBe(discountRate);
            quoteOption.LicenseType.ShouldBe(licenseType);
            quoteOption.PriceType.ShouldBe(priceType);
            quoteOption.IsCustomerCredit.ShouldBe(isCustomerCredit);
            quoteOption.ProductIDs.ShouldBe(productIDs);
            quoteOption.ProductFeatureIDs.ShouldBe(productFeatureIDs);
            quoteOption.Services.ShouldBe(services);
            quoteOption.CustomerID.ShouldBe(customerID);
            quoteOption.CreatedUserID.ShouldBe(createdUserID);
            quoteOption.CreatedDate.ShouldBe(createdDate);
            quoteOption.UpdatedUserID.ShouldBe(updatedUserID);
            quoteOption.UpdatedDate.ShouldBe(updatedDate);
            quoteOption.IsDeleted.ShouldBe(isDeleted);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region bool property type test : QuoteOption => IsCustomerCredit

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsCustomerCredit_Bool_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var boolType = quoteOption.IsCustomerCredit.GetType();

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
        public void QuoteOption_Class_Invalid_Property_IsCustomerCredit_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsCustomerCredit = "IsCustomerCredit";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constIsCustomerCredit));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsCustomerCredit_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constIsCustomerCredit = "IsCustomerCredit";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constIsCustomerCredit);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : QuoteOption => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var random = Fixture.Create<bool>();

            // Act , Set
            quoteOption.IsDeleted = random;

            // Assert
            quoteOption.IsDeleted.ShouldBe(random);
            quoteOption.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();    

            // Act , Set
            quoteOption.IsDeleted = null;

            // Assert
            quoteOption.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var quoteOption = Fixture.Create<QuoteOption>();
            var propertyInfo = quoteOption.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(quoteOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteOption.IsDeleted.ShouldBeNull();
            quoteOption.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : QuoteOption => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var quoteOption = Fixture.Create<QuoteOption>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteOption.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteOption, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : QuoteOption => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var quoteOption = Fixture.Create<QuoteOption>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteOption.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteOption, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region double property type test : QuoteOption => Rate

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Rate_Double_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var doubleType = quoteOption.Rate.GetType();

            // Act
            var isTypeDouble = typeof(double).Equals(doubleType);    
            var isTypeNullableDouble = typeof(double?).Equals(doubleType);
            var isTypeString = typeof(string).Equals(doubleType);
            var isTypeInt = typeof(int).Equals(doubleType);
            var isTypeDecimal = typeof(decimal).Equals(doubleType);
            var isTypeLong = typeof(long).Equals(doubleType);
            var isTypeBool = typeof(bool).Equals(doubleType);
            var isTypeFloat = typeof(float).Equals(doubleType);
            var isTypeIntNullable = typeof(int?).Equals(doubleType);
            var isTypeDecimalNullable = typeof(decimal?).Equals(doubleType);
            var isTypeLongNullable = typeof(long?).Equals(doubleType);
            var isTypeBoolNullable = typeof(bool?).Equals(doubleType);
            var isTypeFloatNullable = typeof(float?).Equals(doubleType);


            // Assert
            isTypeDouble.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_Rate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRate = "Rate";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constRate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Rate_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constRate = "Rate";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : QuoteOption => DiscountRate

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DiscountRate_Data_Without_Null_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var random = Fixture.Create<double>();

            // Act , Set
            quoteOption.DiscountRate = random;

            // Assert
            quoteOption.DiscountRate.ShouldBe(random);
            quoteOption.DiscountRate.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DiscountRate_Only_Null_Data_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();    

            // Act , Set
            quoteOption.DiscountRate = null;

            // Assert
            quoteOption.DiscountRate.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DiscountRate_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constDiscountRate = "DiscountRate";
            var quoteOption = Fixture.Create<QuoteOption>();
            var propertyInfo = quoteOption.GetType().GetProperty(constDiscountRate);

            // Act , Set
            propertyInfo.SetValue(quoteOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteOption.DiscountRate.ShouldBeNull();
            quoteOption.DiscountRate.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_DiscountRate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDiscountRate = "DiscountRate";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constDiscountRate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_DiscountRate_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constDiscountRate = "DiscountRate";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constDiscountRate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : QuoteOption => LicenseType

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseType_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constLicenseType = "LicenseType";
            var quoteOption = Fixture.Create<QuoteOption>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteOption.GetType().GetProperty(constLicenseType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteOption, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_LicenseType_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constLicenseType = "LicenseType";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constLicenseType));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_LicenseType_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constLicenseType = "LicenseType";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constLicenseType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : QuoteOption => PriceType

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PriceType_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constPriceType = "PriceType";
            var quoteOption = Fixture.Create<QuoteOption>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = quoteOption.GetType().GetProperty(constPriceType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(quoteOption, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_PriceType_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constPriceType = "PriceType";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constPriceType));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_PriceType_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constPriceType = "PriceType";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constPriceType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : QuoteOption => BaseChannelID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Int_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var intType = quoteOption.BaseChannelID.GetType();

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
        public void QuoteOption_Class_Invalid_Property_BaseChannelID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constBaseChannelID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_BaseChannelID_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constBaseChannelID = "BaseChannelID";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : QuoteOption => Quantity

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Int_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var intType = quoteOption.Quantity.GetType();

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
        public void QuoteOption_Class_Invalid_Property_Quantity_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuantity = "Quantity";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constQuantity));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Quantity_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constQuantity = "Quantity";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constQuantity);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : QuoteOption => QuoteOptionID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteOptionID_Int_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var intType = quoteOption.QuoteOptionID.GetType();

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
        public void QuoteOption_Class_Invalid_Property_QuoteOptionID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constQuoteOptionID = "QuoteOptionID";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constQuoteOptionID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_QuoteOptionID_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constQuoteOptionID = "QuoteOptionID";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constQuoteOptionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : QuoteOption => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            quoteOption.CreatedUserID = random;

            // Assert
            quoteOption.CreatedUserID.ShouldBe(random);
            quoteOption.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();    

            // Act , Set
            quoteOption.CreatedUserID = null;

            // Assert
            quoteOption.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var quoteOption = Fixture.Create<QuoteOption>();
            var propertyInfo = quoteOption.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(quoteOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteOption.CreatedUserID.ShouldBeNull();
            quoteOption.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : QuoteOption => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Data_Without_Null_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            quoteOption.CustomerID = random;

            // Assert
            quoteOption.CustomerID.ShouldBe(random);
            quoteOption.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Only_Null_Data_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();    

            // Act , Set
            quoteOption.CustomerID = null;

            // Assert
            quoteOption.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCustomerID = "CustomerID";
            var quoteOption = Fixture.Create<QuoteOption>();
            var propertyInfo = quoteOption.GetType().GetProperty(constCustomerID);

            // Act , Set
            propertyInfo.SetValue(quoteOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteOption.CustomerID.ShouldBeNull();
            quoteOption.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : QuoteOption => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var random = Fixture.Create<int>();

            // Act , Set
            quoteOption.UpdatedUserID = random;

            // Assert
            quoteOption.UpdatedUserID.ShouldBe(random);
            quoteOption.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();    

            // Act , Set
            quoteOption.UpdatedUserID = null;

            // Assert
            quoteOption.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var quoteOption = Fixture.Create<QuoteOption>();
            var propertyInfo = quoteOption.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(quoteOption, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            quoteOption.UpdatedUserID.ShouldBeNull();
            quoteOption.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void QuoteOption_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : QuoteOption => Code

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Code_String_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var stringType = quoteOption.Code.GetType();

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
        public void QuoteOption_Class_Invalid_Property_Code_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCode = "Code";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constCode));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Code_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constCode = "Code";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : QuoteOption => Description

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Description_String_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var stringType = quoteOption.Description.GetType();

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
        public void QuoteOption_Class_Invalid_Property_Description_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constDescription = "Description";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constDescription));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Description_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constDescription = "Description";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : QuoteOption => Name

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Name_String_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var stringType = quoteOption.Name.GetType();

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
        public void QuoteOption_Class_Invalid_Property_Name_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constName = "Name";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Name_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constName = "Name";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : QuoteOption => ProductFeatureIDs

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductFeatureIDs_String_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var stringType = quoteOption.ProductFeatureIDs.GetType();

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
        public void QuoteOption_Class_Invalid_Property_ProductFeatureIDs_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductFeatureIDs = "ProductFeatureIDs";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constProductFeatureIDs));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductFeatureIDs_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constProductFeatureIDs = "ProductFeatureIDs";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constProductFeatureIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : QuoteOption => ProductIDs

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductIDs_String_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var stringType = quoteOption.ProductIDs.GetType();

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
        public void QuoteOption_Class_Invalid_Property_ProductIDs_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductIDs = "ProductIDs";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constProductIDs));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductIDs_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constProductIDs = "ProductIDs";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constProductIDs);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : QuoteOption => Services

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Services_String_Type_Verify_Test()
        {
            // Arrange
            var quoteOption = Fixture.Create<QuoteOption>();
            var stringType = quoteOption.Services.GetType();

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
        public void QuoteOption_Class_Invalid_Property_Services_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constServices = "Services";
            var quoteOption  = Fixture.Create<QuoteOption>();

            // Act , Assert
            Should.NotThrow(() => quoteOption.GetType().GetProperty(constServices));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Services_Is_Present_In_QuoteOption_Class_As_Public_Test()
        {
            // Arrange
            const string constServices = "Services";
            var quoteOption  = Fixture.Create<QuoteOption>();
            var propertyInfo  = quoteOption.GetType().GetProperty(constServices);

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
            Should.NotThrow(() => new QuoteOption());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<QuoteOption>(2).ToList();
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
            var quoteOptionID = -1;
            var baseChannelID = -1;
            var code = string.Empty;
            var name = string.Empty;
            var description = string.Empty;
            var quantity = -1;
            var rate = -1;
            double? discountRate = null;
            var licenseType = ECN_Framework_Common.Objects.Accounts.Enums.LicenseTypeEnum.AnnualTechAccess;
            var priceType = ECN_Framework_Common.Objects.Accounts.Enums.PriceTypeEnum.OneTime;
            var isCustomerCredit = false;
            var productIDs = string.Empty;
            var productFeatureIDs = string.Empty;
            var services = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;    

            // Act
            var quoteOption = new QuoteOption();    

            // Assert
            quoteOption.QuoteOptionID.ShouldBe(quoteOptionID);
            quoteOption.BaseChannelID.ShouldBe(baseChannelID);
            quoteOption.Code.ShouldBe(code);
            quoteOption.Name.ShouldBe(name);
            quoteOption.Description.ShouldBe(description);
            quoteOption.Quantity.ShouldBe(quantity);
            quoteOption.Rate.ShouldBe(rate);
            quoteOption.DiscountRate.ShouldBeNull();
            quoteOption.LicenseType.ShouldBe(licenseType);
            quoteOption.PriceType.ShouldBe(priceType);
            quoteOption.IsCustomerCredit.ShouldBeFalse();
            quoteOption.ProductIDs.ShouldBe(productIDs);
            quoteOption.ProductFeatureIDs.ShouldBe(productFeatureIDs);
            quoteOption.Services.ShouldBe(services);
            quoteOption.CreatedUserID.ShouldBeNull();
            quoteOption.CreatedDate.ShouldBeNull();
            quoteOption.UpdatedUserID.ShouldBeNull();
            quoteOption.UpdatedDate.ShouldBeNull();
            quoteOption.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}