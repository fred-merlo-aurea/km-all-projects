using System.Collections.Generic;
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
    public class ProductTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Product_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var product  = new Product();
            var productID = Fixture.Create<int>();
            var productName = Fixture.Create<string>();
            var hasWebsiteTarget = Fixture.Create<bool>();

            // Act
            product.ProductID = productID;
            product.ProductName = productName;
            product.HasWebsiteTarget = hasWebsiteTarget;

            // Assert
            product.ProductID.ShouldBe(productID);
            product.ProductName.ShouldBe(productName);
            product.HasWebsiteTarget.ShouldBe(hasWebsiteTarget);
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region bool property type test : Product => HasWebsiteTarget

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_HasWebsiteTarget_Bool_Type_Verify_Test()
        {
            // Arrange
            var product = Fixture.Create<Product>();
            var boolType = product.HasWebsiteTarget.GetType();

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
        public void Product_Class_Invalid_Property_HasWebsiteTarget_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constHasWebsiteTarget = "HasWebsiteTarget";
            var product  = Fixture.Create<Product>();

            // Act , Assert
            Should.NotThrow(() => product.GetType().GetProperty(constHasWebsiteTarget));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_HasWebsiteTarget_Is_Present_In_Product_Class_As_Public_Test()
        {
            // Arrange
            const string constHasWebsiteTarget = "HasWebsiteTarget";
            var product  = Fixture.Create<Product>();
            var propertyInfo  = product.GetType().GetProperty(constHasWebsiteTarget);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Product => ProductID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductID_Int_Type_Verify_Test()
        {
            // Arrange
            var product = Fixture.Create<Product>();
            var intType = product.ProductID.GetType();

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
        public void Product_Class_Invalid_Property_ProductID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductID = "ProductID";
            var product  = Fixture.Create<Product>();

            // Act , Assert
            Should.NotThrow(() => product.GetType().GetProperty(constProductID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductID_Is_Present_In_Product_Class_As_Public_Test()
        {
            // Arrange
            const string constProductID = "ProductID";
            var product  = Fixture.Create<Product>();
            var propertyInfo  = product.GetType().GetProperty(constProductID);

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
        public void Product_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var product  = Fixture.Create<Product>();

            // Act , Assert
            Should.NotThrow(() => product.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_Product_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var product  = Fixture.Create<Product>();
            var propertyInfo  = product.GetType().GetProperty(constErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Product => ProductName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductName_String_Type_Verify_Test()
        {
            // Arrange
            var product = Fixture.Create<Product>();
            var stringType = product.ProductName.GetType();

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
        public void Product_Class_Invalid_Property_ProductName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constProductName = "ProductName";
            var product  = Fixture.Create<Product>();

            // Act , Assert
            Should.NotThrow(() => product.GetType().GetProperty(constProductName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ProductName_Is_Present_In_Product_Class_As_Public_Test()
        {
            // Arrange
            const string constProductName = "ProductName";
            var product  = Fixture.Create<Product>();
            var propertyInfo  = product.GetType().GetProperty(constProductName);

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
            Should.NotThrow(() => new Product());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Product>(2).ToList();
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
            var productID = -1;
            var productName = string.Empty;
            var errorList = new List<ECNError>();    

            // Act
            var product = new Product();    

            // Assert
            product.ProductID.ShouldBe(productID);
            product.ProductName.ShouldBe(productName);
            product.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #endregion

        #endregion
    }
}