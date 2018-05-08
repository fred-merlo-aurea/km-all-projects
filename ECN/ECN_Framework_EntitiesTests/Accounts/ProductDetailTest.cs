using System;
using System.Collections.Generic;
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
    public class ProductDetailTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var productDetail  = new ProductDetail();
            var productDetailID = Fixture.Create<int>();
            var productID = Fixture.Create<int?>();
            var productDetailName = Fixture.Create<string>();
            var productDetailDesc = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var errorList = new List<ECNError>();

            // Act
            productDetail.ProductDetailID = productDetailID;
            productDetail.ProductID = productID;
            productDetail.ProductDetailName = productDetailName;
            productDetail.ProductDetailDesc = productDetailDesc;
            productDetail.CreatedUserID = createdUserID;
            productDetail.UpdatedUserID = updatedUserID;
            productDetail.IsDeleted = isDeleted;
            productDetail.ErrorList = errorList;

            // Assert
            productDetail.ProductDetailID.ShouldBe(productDetailID);
            productDetail.ProductID.ShouldBe(productID);
            productDetail.ProductDetailName.ShouldBe(productDetailName);
            productDetail.ProductDetailDesc.ShouldBe(productDetailDesc);
            productDetail.CreatedUserID.ShouldBe(createdUserID);
            productDetail.UpdatedUserID.ShouldBe(updatedUserID);
            productDetail.IsDeleted.ShouldBe(isDeleted);
            productDetail.ErrorList.ShouldBe(errorList);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : ProductDetail => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var random = Fixture.Create<bool>();

            // Act , Set
            productDetail.IsDeleted = random;

            // Assert
            productDetail.IsDeleted.ShouldBe(random);
            productDetail.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();    

            // Act , Set
            productDetail.IsDeleted = null;

            // Assert
            productDetail.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var productDetail = Fixture.Create<ProductDetail>();
            var propertyInfo = productDetail.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(productDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            productDetail.IsDeleted.ShouldBeNull();
            productDetail.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : ProductDetail => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var productDetail = Fixture.Create<ProductDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = productDetail.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(productDetail, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : ProductDetail => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var productDetail = Fixture.Create<ProductDetail>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = productDetail.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(productDetail, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : ProductDetail => ProductDetailID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductDetailID_Int_Type_Verify_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var intType = productDetail.ProductDetailID.GetType();

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
        public void ProductDetail_Class_Invalid_Property_ProductDetailID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductDetailID = "ProductDetailID";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constProductDetailID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductDetailID_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constProductDetailID = "ProductDetailID";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constProductDetailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : ProductDetail => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            productDetail.CreatedUserID = random;

            // Assert
            productDetail.CreatedUserID.ShouldBe(random);
            productDetail.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();    

            // Act , Set
            productDetail.CreatedUserID = null;

            // Assert
            productDetail.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var productDetail = Fixture.Create<ProductDetail>();
            var propertyInfo = productDetail.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(productDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            productDetail.CreatedUserID.ShouldBeNull();
            productDetail.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : ProductDetail => ProductID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductID_Data_Without_Null_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            productDetail.ProductID = random;

            // Assert
            productDetail.ProductID.ShouldBe(random);
            productDetail.ProductID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductID_Only_Null_Data_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();    

            // Act , Set
            productDetail.ProductID = null;

            // Assert
            productDetail.ProductID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constProductID = "ProductID";
            var productDetail = Fixture.Create<ProductDetail>();
            var propertyInfo = productDetail.GetType().GetProperty(constProductID);

            // Act , Set
            propertyInfo.SetValue(productDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            productDetail.ProductID.ShouldBeNull();
            productDetail.ProductID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_ProductID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductID = "ProductID";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constProductID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductID_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constProductID = "ProductID";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : ProductDetail => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var random = Fixture.Create<int>();

            // Act , Set
            productDetail.UpdatedUserID = random;

            // Assert
            productDetail.UpdatedUserID.ShouldBe(random);
            productDetail.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();    

            // Act , Set
            productDetail.UpdatedUserID = null;

            // Assert
            productDetail.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var productDetail = Fixture.Create<ProductDetail>();
            var propertyInfo = productDetail.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(productDetail, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            productDetail.UpdatedUserID.ShouldBeNull();
            productDetail.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductDetail_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : ProductDetail => ProductDetailDesc

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductDetailDesc_String_Type_Verify_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var stringType = productDetail.ProductDetailDesc.GetType();

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
        public void ProductDetail_Class_Invalid_Property_ProductDetailDesc_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductDetailDesc = "ProductDetailDesc";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constProductDetailDesc));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductDetailDesc_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constProductDetailDesc = "ProductDetailDesc";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constProductDetailDesc);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : ProductDetail => ProductDetailName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductDetailName_String_Type_Verify_Test()
        {
            // Arrange
            var productDetail = Fixture.Create<ProductDetail>();
            var stringType = productDetail.ProductDetailName.GetType();

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
        public void ProductDetail_Class_Invalid_Property_ProductDetailName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductDetailName = "ProductDetailName";
            var productDetail  = Fixture.Create<ProductDetail>();

            // Act , Assert
            Should.NotThrow(() => productDetail.GetType().GetProperty(constProductDetailName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductDetailName_Is_Present_In_ProductDetail_Class_As_Public_Test()
        {
            // Arrange
            const string constProductDetailName = "ProductDetailName";
            var productDetail  = Fixture.Create<ProductDetail>();
            var propertyInfo  = productDetail.GetType().GetProperty(constProductDetailName);

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
            Should.NotThrow(() => new ProductDetail());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<ProductDetail>(2).ToList();
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
            var productDetailID = -1;
            int? productID = null;
            var productDetailName = string.Empty;
            var productDetailDesc = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;
            var errorList = new List<ECNError>();    

            // Act
            var productDetail = new ProductDetail();    

            // Assert
            productDetail.ProductDetailID.ShouldBe(productDetailID);
            productDetail.ProductID.ShouldBeNull();
            productDetail.ProductDetailName.ShouldBe(productDetailName);
            productDetail.ProductDetailDesc.ShouldBe(productDetailDesc);
            productDetail.CreatedUserID.ShouldBeNull();
            productDetail.CreatedDate.ShouldBeNull();
            productDetail.UpdatedUserID.ShouldBeNull();
            productDetail.UpdatedDate.ShouldBeNull();
            productDetail.IsDeleted.ShouldBeNull();
            productDetail.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}