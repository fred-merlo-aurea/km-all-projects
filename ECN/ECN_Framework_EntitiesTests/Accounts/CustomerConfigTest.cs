using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
{
    [TestFixture]
    public class CustomerConfigTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CustomerConfig) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            var customerConfigId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var productId = Fixture.Create<int?>();
            var configNameId = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.ConfigName>();
            var configName = Fixture.Create<string>();
            var configValue = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            customerConfig.CustomerConfigID = customerConfigId;
            customerConfig.CustomerID = customerId;
            customerConfig.ProductID = productId;
            customerConfig.ConfigNameID = configNameId;
            customerConfig.ConfigName = configName;
            customerConfig.ConfigValue = configValue;
            customerConfig.CreatedUserID = createdUserId;
            customerConfig.UpdatedUserID = updatedUserId;
            customerConfig.IsDeleted = isDeleted;
            customerConfig.ErrorList = errorList;

            // Assert
            customerConfig.CustomerConfigID.ShouldBe(customerConfigId);
            customerConfig.CustomerID.ShouldBe(customerId);
            customerConfig.ProductID.ShouldBe(productId);
            customerConfig.ConfigNameID.ShouldBe(configNameId);
            customerConfig.ConfigName.ShouldBe(configName);
            customerConfig.ConfigValue.ShouldBe(configValue);
            customerConfig.CreatedUserID.ShouldBe(createdUserId);
            customerConfig.CreatedDate.ShouldBeNull();
            customerConfig.UpdatedUserID.ShouldBe(updatedUserId);
            customerConfig.UpdatedDate.ShouldBeNull();
            customerConfig.IsDeleted.ShouldBe(isDeleted);
            customerConfig.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ConfigName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ConfigName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            customerConfig.ConfigName = Fixture.Create<string>();
            var stringType = customerConfig.ConfigName.GetType();

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

        #region General Getters/Setters : Class (CustomerConfig) => Property (ConfigName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_ConfigNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConfigName = "ConfigNameNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameConfigName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ConfigName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConfigName = "ConfigName";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameConfigName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ConfigNameID) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ConfigNameID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameConfigNameID = "ConfigNameID";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameConfigNameID);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ConfigNameID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_ConfigNameIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConfigNameID = "ConfigNameIDNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameConfigNameID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ConfigNameID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConfigNameID = "ConfigNameID";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameConfigNameID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ConfigValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ConfigValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            customerConfig.ConfigValue = Fixture.Create<string>();
            var stringType = customerConfig.ConfigValue.GetType();

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

        #region General Getters/Setters : Class (CustomerConfig) => Property (ConfigValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_ConfigValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConfigValue = "ConfigValueNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameConfigValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ConfigValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConfigValue = "ConfigValue";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameConfigValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerConfig.CreatedUserID = random;

            // Assert
            customerConfig.CreatedUserID.ShouldBe(random);
            customerConfig.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();    

            // Act , Set
            customerConfig.CreatedUserID = null;

            // Assert
            customerConfig.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerConfig.CreatedUserID.ShouldBeNull();
            customerConfig.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CustomerConfigID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CustomerConfigID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            customerConfig.CustomerConfigID = Fixture.Create<int>();
            var intType = customerConfig.CustomerConfigID.GetType();

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

        #region General Getters/Setters : Class (CustomerConfig) => Property (CustomerConfigID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_CustomerConfigIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerConfigID = "CustomerConfigIDNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameCustomerConfigID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CustomerConfigID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerConfigID = "CustomerConfigID";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameCustomerConfigID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerConfig.CustomerID = random;

            // Assert
            customerConfig.CustomerID.ShouldBe(random);
            customerConfig.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();    

            // Act , Set
            customerConfig.CustomerID = null;

            // Assert
            customerConfig.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(customerConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerConfig.CustomerID.ShouldBeNull();
            customerConfig.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ErrorList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_ErrorListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorListNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameErrorList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ErrorList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorList";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerConfig.IsDeleted = random;

            // Assert
            customerConfig.IsDeleted.ShouldBe(random);
            customerConfig.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();    

            // Act , Set
            customerConfig.IsDeleted = null;

            // Assert
            customerConfig.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customerConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerConfig.IsDeleted.ShouldBeNull();
            customerConfig.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ProductID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ProductID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerConfig.ProductID = random;

            // Assert
            customerConfig.ProductID.ShouldBe(random);
            customerConfig.ProductID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ProductID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();    

            // Act , Set
            customerConfig.ProductID = null;

            // Assert
            customerConfig.ProductID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ProductID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameProductID = "ProductID";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameProductID);

            // Act , Set
            propertyInfo.SetValue(customerConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerConfig.ProductID.ShouldBeNull();
            customerConfig.ProductID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (ProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductIDNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductID";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerConfig, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerConfig.UpdatedUserID = random;

            // Assert
            customerConfig.UpdatedUserID.ShouldBe(random);
            customerConfig.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerConfig = Fixture.Create<CustomerConfig>();    

            // Act , Set
            customerConfig.UpdatedUserID = null;

            // Assert
            customerConfig.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customerConfig = Fixture.Create<CustomerConfig>();
            var propertyInfo = customerConfig.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerConfig, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerConfig.UpdatedUserID.ShouldBeNull();
            customerConfig.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerConfig) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var customerConfig  = Fixture.Create<CustomerConfig>();

            // Act , Assert
            Should.NotThrow(() => customerConfig.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerConfig_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customerConfig  = Fixture.Create<CustomerConfig>();
            var propertyInfo  = customerConfig.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CustomerConfig) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerConfig_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerConfig());
        }

        #endregion

        #region General Constructor : Class (CustomerConfig) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerConfig_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCustomerConfig = Fixture.CreateMany<CustomerConfig>(2).ToList();
            var firstCustomerConfig = instancesOfCustomerConfig.FirstOrDefault();
            var lastCustomerConfig = instancesOfCustomerConfig.Last();

            // Act, Assert
            firstCustomerConfig.ShouldNotBeNull();
            lastCustomerConfig.ShouldNotBeNull();
            firstCustomerConfig.ShouldNotBeSameAs(lastCustomerConfig);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerConfig_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCustomerConfig = new CustomerConfig();
            var secondCustomerConfig = new CustomerConfig();
            var thirdCustomerConfig = new CustomerConfig();
            var fourthCustomerConfig = new CustomerConfig();
            var fifthCustomerConfig = new CustomerConfig();
            var sixthCustomerConfig = new CustomerConfig();

            // Act, Assert
            firstCustomerConfig.ShouldNotBeNull();
            secondCustomerConfig.ShouldNotBeNull();
            thirdCustomerConfig.ShouldNotBeNull();
            fourthCustomerConfig.ShouldNotBeNull();
            fifthCustomerConfig.ShouldNotBeNull();
            sixthCustomerConfig.ShouldNotBeNull();
            firstCustomerConfig.ShouldNotBeSameAs(secondCustomerConfig);
            thirdCustomerConfig.ShouldNotBeSameAs(firstCustomerConfig);
            fourthCustomerConfig.ShouldNotBeSameAs(firstCustomerConfig);
            fifthCustomerConfig.ShouldNotBeSameAs(firstCustomerConfig);
            sixthCustomerConfig.ShouldNotBeSameAs(firstCustomerConfig);
            sixthCustomerConfig.ShouldNotBeSameAs(fourthCustomerConfig);
        }

        #endregion

        #region General Constructor : Class (CustomerConfig) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerConfig_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var customerConfigId = -1;
            var configNameId = ECN_Framework_Common.Objects.Accounts.Enums.ConfigName.Unknown;
            var configName = string.Empty;
            var configValue = string.Empty;
            var errorList = new List<ECNError>();

            // Act
            var customerConfig = new CustomerConfig();

            // Assert
            customerConfig.CustomerConfigID.ShouldBe(customerConfigId);
            customerConfig.CustomerID.ShouldBeNull();
            customerConfig.ProductID.ShouldBeNull();
            customerConfig.ConfigNameID.ShouldBe(configNameId);
            customerConfig.ConfigName.ShouldBe(configName);
            customerConfig.ConfigValue.ShouldBe(configValue);
            customerConfig.CreatedUserID.ShouldBeNull();
            customerConfig.CreatedDate.ShouldBeNull();
            customerConfig.UpdatedUserID.ShouldBeNull();
            customerConfig.UpdatedDate.ShouldBeNull();
            customerConfig.IsDeleted.ShouldBeNull();
            customerConfig.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}