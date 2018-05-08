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
    public class ProductActionInfoTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ProductActionInfo) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            var userActionId = Fixture.Create<int>();
            var actionId = Fixture.Create<int>();
            var active = Fixture.Create<string>();
            var productId = Fixture.Create<int>();
            var websiteAddress = Fixture.Create<string>();
            var productName = Fixture.Create<string>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();

            // Act
            productActionInfo.UserActionID = userActionId;
            productActionInfo.ActionID = actionId;
            productActionInfo.Active = active;
            productActionInfo.ProductID = productId;
            productActionInfo.WebsiteAddress = websiteAddress;
            productActionInfo.ProductName = productName;
            productActionInfo.DisplayName = displayName;
            productActionInfo.ActionCode = actionCode;

            // Assert
            productActionInfo.UserActionID.ShouldBe(userActionId);
            productActionInfo.ActionID.ShouldBe(actionId);
            productActionInfo.Active.ShouldBe(active);
            productActionInfo.ProductID.ShouldBe(productId);
            productActionInfo.WebsiteAddress.ShouldBe(websiteAddress);
            productActionInfo.ProductName.ShouldBe(productName);
            productActionInfo.DisplayName.ShouldBe(displayName);
            productActionInfo.ActionCode.ShouldBe(actionCode);
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ActionCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ActionCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.ActionCode = Fixture.Create<string>();
            var stringType = productActionInfo.ActionCode.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ActionCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_ActionCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionCode = "ActionCodeNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameActionCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ActionCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionCode = "ActionCode";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameActionCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ActionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ActionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.ActionID = Fixture.Create<int>();
            var intType = productActionInfo.ActionID.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ActionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_ActionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionID = "ActionIDNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameActionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ActionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionID = "ActionID";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (Active) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Active_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.Active = Fixture.Create<string>();
            var stringType = productActionInfo.Active.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (Active) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_ActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActive = "ActiveNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Active_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActive = "Active";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.DisplayName = Fixture.Create<string>();
            var stringType = productActionInfo.DisplayName.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ProductID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ProductID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.ProductID = Fixture.Create<int>();
            var intType = productActionInfo.ProductID.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductIDNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductID";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ProductName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ProductName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.ProductName = Fixture.Create<string>();
            var stringType = productActionInfo.ProductName.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (ProductName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_ProductNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductName = "ProductNameNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameProductName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_ProductName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductName = "ProductName";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameProductName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (UserActionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_UserActionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.UserActionID = Fixture.Create<int>();
            var intType = productActionInfo.UserActionID.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (UserActionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_UserActionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserActionID = "UserActionIDNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameUserActionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_UserActionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserActionID = "UserActionID";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameUserActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductActionInfo) => Property (WebsiteAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_WebsiteAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productActionInfo = Fixture.Create<ProductActionInfo>();
            productActionInfo.WebsiteAddress = Fixture.Create<string>();
            var stringType = productActionInfo.WebsiteAddress.GetType();

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

        #region General Getters/Setters : Class (ProductActionInfo) => Property (WebsiteAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_Class_Invalid_Property_WebsiteAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameWebsiteAddress = "WebsiteAddressNotPresent";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();

            // Act , Assert
            Should.NotThrow(() => productActionInfo.GetType().GetProperty(propertyNameWebsiteAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductActionInfo_WebsiteAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameWebsiteAddress = "WebsiteAddress";
            var productActionInfo  = Fixture.Create<ProductActionInfo>();
            var propertyInfo  = productActionInfo.GetType().GetProperty(propertyNameWebsiteAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ProductActionInfo) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductActionInfo_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var userActionID = Fixture.Create<int>();
            var actionID = Fixture.Create<int>();
            var active = Fixture.Create<string>();
            var productID = Fixture.Create<int>();
            var websiteAddress = Fixture.Create<string>();
            var productName = Fixture.Create<string>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();

            // Act, Assert
            Should.NotThrow(() => new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode));
        }

        #endregion

        #region General Constructor : Class (ProductActionInfo) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductActionInfo_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfProductActionInfo = Fixture.CreateMany<ProductActionInfo>(2).ToList();
            var firstProductActionInfo = instancesOfProductActionInfo.FirstOrDefault();
            var lastProductActionInfo = instancesOfProductActionInfo.Last();

            // Act, Assert
            firstProductActionInfo.ShouldNotBeNull();
            lastProductActionInfo.ShouldNotBeNull();
            firstProductActionInfo.ShouldNotBeSameAs(lastProductActionInfo);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductActionInfo_5_Objects_Creation_8_Paramters_Test()
        {
            // Arrange
            var userActionID = Fixture.Create<int>();
            var actionID = Fixture.Create<int>();
            var active = Fixture.Create<string>();
            var productID = Fixture.Create<int>();
            var websiteAddress = Fixture.Create<string>();
            var productName = Fixture.Create<string>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();
            var firstProductActionInfo = new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode);
            var secondProductActionInfo = new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode);
            var thirdProductActionInfo = new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode);
            var fourthProductActionInfo = new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode);
            var fifthProductActionInfo = new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode);
            var sixthProductActionInfo = new ProductActionInfo(userActionID, actionID, active, productID, websiteAddress, productName, displayName, actionCode);

            // Act, Assert
            firstProductActionInfo.ShouldNotBeNull();
            secondProductActionInfo.ShouldNotBeNull();
            thirdProductActionInfo.ShouldNotBeNull();
            fourthProductActionInfo.ShouldNotBeNull();
            fifthProductActionInfo.ShouldNotBeNull();
            sixthProductActionInfo.ShouldNotBeNull();
            firstProductActionInfo.ShouldNotBeSameAs(secondProductActionInfo);
            thirdProductActionInfo.ShouldNotBeSameAs(firstProductActionInfo);
            fourthProductActionInfo.ShouldNotBeSameAs(firstProductActionInfo);
            fifthProductActionInfo.ShouldNotBeSameAs(firstProductActionInfo);
            sixthProductActionInfo.ShouldNotBeSameAs(firstProductActionInfo);
            sixthProductActionInfo.ShouldNotBeSameAs(fourthProductActionInfo);
        }

        #endregion

        #endregion

        #endregion
    }
}