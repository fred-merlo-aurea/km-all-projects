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
    public class CustomerProductTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CustomerProduct) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            var customerProductId = Fixture.Create<int>();
            var customerId = Fixture.Create<int?>();
            var productDetailId = Fixture.Create<int?>();
            var active = Fixture.Create<string>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            customerProduct.CustomerProductID = customerProductId;
            customerProduct.CustomerID = customerId;
            customerProduct.ProductDetailID = productDetailId;
            customerProduct.Active = active;
            customerProduct.CreatedUserID = createdUserId;
            customerProduct.UpdatedUserID = updatedUserId;
            customerProduct.CreatedDate = createdDate;
            customerProduct.UpdatedDate = updatedDate;
            customerProduct.IsDeleted = isDeleted;

            // Assert
            customerProduct.CustomerProductID.ShouldBe(customerProductId);
            customerProduct.CustomerID.ShouldBe(customerId);
            customerProduct.ProductDetailID.ShouldBe(productDetailId);
            customerProduct.Active.ShouldBe(active);
            customerProduct.CreatedUserID.ShouldBe(createdUserId);
            customerProduct.UpdatedUserID.ShouldBe(updatedUserId);
            customerProduct.CreatedDate.ShouldBe(createdDate);
            customerProduct.UpdatedDate.ShouldBe(updatedDate);
            customerProduct.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (Active) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Active_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            customerProduct.Active = Fixture.Create<string>();
            var stringType = customerProduct.Active.GetType();

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

        #region General Getters/Setters : Class (CustomerProduct) => Property (Active) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_ActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActive = "ActiveNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Active_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActive = "Active";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerProduct, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerProduct.CreatedUserID = random;

            // Assert
            customerProduct.CreatedUserID.ShouldBe(random);
            customerProduct.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();    

            // Act , Set
            customerProduct.CreatedUserID = null;

            // Assert
            customerProduct.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerProduct, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerProduct.CreatedUserID.ShouldBeNull();
            customerProduct.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerProduct.CustomerID = random;

            // Assert
            customerProduct.CustomerID.ShouldBe(random);
            customerProduct.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();    

            // Act , Set
            customerProduct.CustomerID = null;

            // Assert
            customerProduct.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(customerProduct, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerProduct.CustomerID.ShouldBeNull();
            customerProduct.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (CustomerProductID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CustomerProductID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            customerProduct.CustomerProductID = Fixture.Create<int>();
            var intType = customerProduct.CustomerProductID.GetType();

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

        #region General Getters/Setters : Class (CustomerProduct) => Property (CustomerProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_CustomerProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerProductID = "CustomerProductIDNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameCustomerProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_CustomerProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerProductID = "CustomerProductID";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameCustomerProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            var random = Fixture.Create<bool>();

            // Act , Set
            customerProduct.IsDeleted = random;

            // Assert
            customerProduct.IsDeleted.ShouldBe(random);
            customerProduct.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();    

            // Act , Set
            customerProduct.IsDeleted = null;

            // Assert
            customerProduct.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(customerProduct, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerProduct.IsDeleted.ShouldBeNull();
            customerProduct.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (ProductDetailID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_ProductDetailID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerProduct.ProductDetailID = random;

            // Assert
            customerProduct.ProductDetailID.ShouldBe(random);
            customerProduct.ProductDetailID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_ProductDetailID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();    

            // Act , Set
            customerProduct.ProductDetailID = null;

            // Assert
            customerProduct.ProductDetailID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_ProductDetailID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameProductDetailID = "ProductDetailID";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameProductDetailID);

            // Act , Set
            propertyInfo.SetValue(customerProduct, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerProduct.ProductDetailID.ShouldBeNull();
            customerProduct.ProductDetailID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (ProductDetailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_ProductDetailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductDetailID = "ProductDetailIDNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameProductDetailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_ProductDetailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductDetailID = "ProductDetailID";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameProductDetailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerProduct, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerProduct.UpdatedUserID = random;

            // Assert
            customerProduct.UpdatedUserID.ShouldBe(random);
            customerProduct.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerProduct = Fixture.Create<CustomerProduct>();    

            // Act , Set
            customerProduct.UpdatedUserID = null;

            // Assert
            customerProduct.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customerProduct = Fixture.Create<CustomerProduct>();
            var propertyInfo = customerProduct.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(customerProduct, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerProduct.UpdatedUserID.ShouldBeNull();
            customerProduct.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerProduct) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var customerProduct  = Fixture.Create<CustomerProduct>();

            // Act , Assert
            Should.NotThrow(() => customerProduct.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerProduct_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var customerProduct  = Fixture.Create<CustomerProduct>();
            var propertyInfo  = customerProduct.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CustomerProduct) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerProduct_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerProduct());
        }

        #endregion

        #region General Constructor : Class (CustomerProduct) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerProduct_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCustomerProduct = Fixture.CreateMany<CustomerProduct>(2).ToList();
            var firstCustomerProduct = instancesOfCustomerProduct.FirstOrDefault();
            var lastCustomerProduct = instancesOfCustomerProduct.Last();

            // Act, Assert
            firstCustomerProduct.ShouldNotBeNull();
            lastCustomerProduct.ShouldNotBeNull();
            firstCustomerProduct.ShouldNotBeSameAs(lastCustomerProduct);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerProduct_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCustomerProduct = new CustomerProduct();
            var secondCustomerProduct = new CustomerProduct();
            var thirdCustomerProduct = new CustomerProduct();
            var fourthCustomerProduct = new CustomerProduct();
            var fifthCustomerProduct = new CustomerProduct();
            var sixthCustomerProduct = new CustomerProduct();

            // Act, Assert
            firstCustomerProduct.ShouldNotBeNull();
            secondCustomerProduct.ShouldNotBeNull();
            thirdCustomerProduct.ShouldNotBeNull();
            fourthCustomerProduct.ShouldNotBeNull();
            fifthCustomerProduct.ShouldNotBeNull();
            sixthCustomerProduct.ShouldNotBeNull();
            firstCustomerProduct.ShouldNotBeSameAs(secondCustomerProduct);
            thirdCustomerProduct.ShouldNotBeSameAs(firstCustomerProduct);
            fourthCustomerProduct.ShouldNotBeSameAs(firstCustomerProduct);
            fifthCustomerProduct.ShouldNotBeSameAs(firstCustomerProduct);
            sixthCustomerProduct.ShouldNotBeSameAs(firstCustomerProduct);
            sixthCustomerProduct.ShouldNotBeSameAs(fourthCustomerProduct);
        }

        #endregion

        #region General Constructor : Class (CustomerProduct) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerProduct_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var customerProductId = -1;
            var active = string.Empty;

            // Act
            var customerProduct = new CustomerProduct();

            // Assert
            customerProduct.CustomerProductID.ShouldBe(customerProductId);
            customerProduct.CustomerID.ShouldBeNull();
            customerProduct.ProductDetailID.ShouldBeNull();
            customerProduct.Active.ShouldBe(active);
            customerProduct.CreatedUserID.ShouldBeNull();
            customerProduct.CreatedDate.ShouldBeNull();
            customerProduct.UpdatedUserID.ShouldBeNull();
            customerProduct.UpdatedDate.ShouldBeNull();
            customerProduct.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}