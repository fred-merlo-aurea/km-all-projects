using System;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using AutoFixture;
using ECN_Framework_Entities.Accounts.View;
using ECN_Framework_EntitiesTests.ConfigureProject;

namespace ECN_Framework_EntitiesTests.Activity.View
{
    [TestFixture]
    public class ProductInfoTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var productID = Fixture.Create<int>();
            var productInfo  = new ProductInfo(productID);

            // Act
            productInfo.ProductID = productID;

            // Assert
            productInfo.ProductID.ShouldBe(productID);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region int property type test : ProductInfo => ProductID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_ProductID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productInfo = Fixture.Create<ProductInfo>();
            var intType = productInfo.ProductID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string ProductIDPropertyName = "ProductIDNotPresent";
            var productInfo  = Fixture.Create<ProductInfo>();

            // Act , Assert
            Should.NotThrow(() => productInfo.GetType().GetProperty(ProductIDPropertyName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductInfo_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string ProductIDPropertyName = "ProductID";
            var productInfo  = Fixture.Create<ProductInfo>();
            var propertyInfo  = productInfo.GetType().GetProperty(ProductIDPropertyName);

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

        #region General Constructor Pattern : Check constructor creation by throwing or not throwing exception.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductInfo_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange 
            int productID = 10;

            // Assert
            Should.NotThrow(() => new ProductInfo(productID));
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<ProductInfo>(2).ToList();
            var first = myInstances.FirstOrDefault();
            var last = myInstances.Last();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBeSameAs(last);
        }

        #endregion

        #endregion

        #endregion
    }
}