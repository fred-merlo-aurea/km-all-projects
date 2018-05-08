using System;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts.View;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts.View
{
    [TestFixture]
    public class ProductInfoTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ProductInfo) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var productInfo = Fixture.Create<ProductInfo>();
            var productId = Fixture.Create<int>();

            // Act
            productInfo.ProductID = productId;

            // Assert
            productInfo.ProductID.ShouldBe(productId);
        }

        #endregion

        #region General Getters/Setters : Class (ProductInfo) => Property (ProductID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_ProductID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productInfo = Fixture.Create<ProductInfo>();
            productInfo.ProductID = Fixture.Create<int>();
            var intType = productInfo.ProductID.GetType();

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

        #region General Getters/Setters : Class (ProductInfo) => Property (ProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductIDNotPresent";
            var productInfo  = Fixture.Create<ProductInfo>();

            // Act , Assert
            Should.NotThrow(() => productInfo.GetType().GetProperty(propertyNameProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductID";
            var productInfo  = Fixture.Create<ProductInfo>();
            var propertyInfo  = productInfo.GetType().GetProperty(propertyNameProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ProductInfo) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductInfo_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var productID = Fixture.Create<int>();

            // Act, Assert
            Should.NotThrow(() => new ProductInfo(productID));
        }

        #endregion

        #region General Constructor : Class (ProductInfo) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductInfo_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfProductInfo = Fixture.CreateMany<ProductInfo>(2).ToList();
            var firstProductInfo = instancesOfProductInfo.FirstOrDefault();
            var lastProductInfo = instancesOfProductInfo.Last();

            // Act, Assert
            firstProductInfo.ShouldNotBeNull();
            lastProductInfo.ShouldNotBeNull();
            firstProductInfo.ShouldNotBeSameAs(lastProductInfo);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductInfo_5_Objects_Creation_1_Paramters_Test()
        {
            // Arrange
            var productID = Fixture.Create<int>();
            var firstProductInfo = new ProductInfo(productID);
            var secondProductInfo = new ProductInfo(productID);
            var thirdProductInfo = new ProductInfo(productID);
            var fourthProductInfo = new ProductInfo(productID);
            var fifthProductInfo = new ProductInfo(productID);
            var sixthProductInfo = new ProductInfo(productID);

            // Act, Assert
            firstProductInfo.ShouldNotBeNull();
            secondProductInfo.ShouldNotBeNull();
            thirdProductInfo.ShouldNotBeNull();
            fourthProductInfo.ShouldNotBeNull();
            fifthProductInfo.ShouldNotBeNull();
            sixthProductInfo.ShouldNotBeNull();
            firstProductInfo.ShouldNotBeSameAs(secondProductInfo);
            thirdProductInfo.ShouldNotBeSameAs(firstProductInfo);
            fourthProductInfo.ShouldNotBeSameAs(firstProductInfo);
            fifthProductInfo.ShouldNotBeSameAs(firstProductInfo);
            sixthProductInfo.ShouldNotBeSameAs(firstProductInfo);
            sixthProductInfo.ShouldNotBeSameAs(fourthProductInfo);
        }

        #endregion

        #endregion

        #endregion
    }
}