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
    public class ProductFeatureInfoTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (ProductFeatureInfo) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            var userActionId = Fixture.Create<int>();
            var actionId = Fixture.Create<int>();
            var active = Fixture.Create<string>();
            var productId = Fixture.Create<int>();
            var productName = Fixture.Create<string>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();
            var displayOrder = Fixture.Create<int?>();

            // Act
            productFeatureInfo.UserActionID = userActionId;
            productFeatureInfo.ActionID = actionId;
            productFeatureInfo.Active = active;
            productFeatureInfo.ProductID = productId;
            productFeatureInfo.ProductName = productName;
            productFeatureInfo.DisplayName = displayName;
            productFeatureInfo.ActionCode = actionCode;
            productFeatureInfo.DisplayOrder = displayOrder;

            // Assert
            productFeatureInfo.UserActionID.ShouldBe(userActionId);
            productFeatureInfo.ActionID.ShouldBe(actionId);
            productFeatureInfo.Active.ShouldBe(active);
            productFeatureInfo.ProductID.ShouldBe(productId);
            productFeatureInfo.ProductName.ShouldBe(productName);
            productFeatureInfo.DisplayName.ShouldBe(displayName);
            productFeatureInfo.ActionCode.ShouldBe(actionCode);
            productFeatureInfo.DisplayOrder.ShouldBe(displayOrder);
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ActionCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ActionCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.ActionCode = Fixture.Create<string>();
            var stringType = productFeatureInfo.ActionCode.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ActionCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_ActionCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionCode = "ActionCodeNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameActionCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ActionCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionCode = "ActionCode";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameActionCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ActionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ActionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.ActionID = Fixture.Create<int>();
            var intType = productFeatureInfo.ActionID.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ActionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_ActionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionID = "ActionIDNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameActionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ActionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionID = "ActionID";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (Active) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Active_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.Active = Fixture.Create<string>();
            var stringType = productFeatureInfo.Active.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (Active) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_ActiveNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActive = "ActiveNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameActive));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Active_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActive = "Active";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameActive);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.DisplayName = Fixture.Create<string>();
            var stringType = productFeatureInfo.DisplayName.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (DisplayOrder) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_DisplayOrder_Property_Data_Without_Null_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            var random = Fixture.Create<int>();

            // Act , Set
            productFeatureInfo.DisplayOrder = random;

            // Assert
            productFeatureInfo.DisplayOrder.ShouldBe(random);
            productFeatureInfo.DisplayOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_DisplayOrder_Property_Only_Null_Data_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();    

            // Act , Set
            productFeatureInfo.DisplayOrder = null;

            // Assert
            productFeatureInfo.DisplayOrder.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_DisplayOrder_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDisplayOrder = "DisplayOrder";
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo = productFeatureInfo.GetType().GetProperty(propertyNameDisplayOrder);

            // Act , Set
            propertyInfo.SetValue(productFeatureInfo, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            productFeatureInfo.DisplayOrder.ShouldBeNull();
            productFeatureInfo.DisplayOrder.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (DisplayOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_DisplayOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayOrder = "DisplayOrderNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameDisplayOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_DisplayOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayOrder = "DisplayOrder";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameDisplayOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ProductID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ProductID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.ProductID = Fixture.Create<int>();
            var intType = productFeatureInfo.ProductID.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductIDNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductID";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ProductName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ProductName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.ProductName = Fixture.Create<string>();
            var stringType = productFeatureInfo.ProductName.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (ProductName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_ProductNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductName = "ProductNameNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameProductName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_ProductName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductName = "ProductName";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameProductName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (UserActionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_UserActionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var productFeatureInfo = Fixture.Create<ProductFeatureInfo>();
            productFeatureInfo.UserActionID = Fixture.Create<int>();
            var intType = productFeatureInfo.UserActionID.GetType();

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

        #region General Getters/Setters : Class (ProductFeatureInfo) => Property (UserActionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_Class_Invalid_Property_UserActionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserActionID = "UserActionIDNotPresent";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();

            // Act , Assert
            Should.NotThrow(() => productFeatureInfo.GetType().GetProperty(propertyNameUserActionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void ProductFeatureInfo_UserActionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserActionID = "UserActionID";
            var productFeatureInfo  = Fixture.Create<ProductFeatureInfo>();
            var propertyInfo  = productFeatureInfo.GetType().GetProperty(propertyNameUserActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (ProductFeatureInfo) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductFeatureInfo_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var userActionID = Fixture.Create<int>();
            var actionID = Fixture.Create<int>();
            var active = Fixture.Create<string>();
            var productID = Fixture.Create<int>();
            var productName = Fixture.Create<string>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();
            var displayOrder = Fixture.Create<int?>();

            // Act, Assert
            Should.NotThrow(() => new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder));
        }

        #endregion

        #region General Constructor : Class (ProductFeatureInfo) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductFeatureInfo_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfProductFeatureInfo = Fixture.CreateMany<ProductFeatureInfo>(2).ToList();
            var firstProductFeatureInfo = instancesOfProductFeatureInfo.FirstOrDefault();
            var lastProductFeatureInfo = instancesOfProductFeatureInfo.Last();

            // Act, Assert
            firstProductFeatureInfo.ShouldNotBeNull();
            lastProductFeatureInfo.ShouldNotBeNull();
            firstProductFeatureInfo.ShouldNotBeSameAs(lastProductFeatureInfo);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_ProductFeatureInfo_5_Objects_Creation_8_Paramters_Test()
        {
            // Arrange
            var userActionID = Fixture.Create<int>();
            var actionID = Fixture.Create<int>();
            var active = Fixture.Create<string>();
            var productID = Fixture.Create<int>();
            var productName = Fixture.Create<string>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();
            var displayOrder = Fixture.Create<int?>();
            var firstProductFeatureInfo = new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder);
            var secondProductFeatureInfo = new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder);
            var thirdProductFeatureInfo = new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder);
            var fourthProductFeatureInfo = new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder);
            var fifthProductFeatureInfo = new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder);
            var sixthProductFeatureInfo = new ProductFeatureInfo(userActionID, actionID, active, productID, productName, displayName, actionCode, displayOrder);

            // Act, Assert
            firstProductFeatureInfo.ShouldNotBeNull();
            secondProductFeatureInfo.ShouldNotBeNull();
            thirdProductFeatureInfo.ShouldNotBeNull();
            fourthProductFeatureInfo.ShouldNotBeNull();
            fifthProductFeatureInfo.ShouldNotBeNull();
            sixthProductFeatureInfo.ShouldNotBeNull();
            firstProductFeatureInfo.ShouldNotBeSameAs(secondProductFeatureInfo);
            thirdProductFeatureInfo.ShouldNotBeSameAs(firstProductFeatureInfo);
            fourthProductFeatureInfo.ShouldNotBeSameAs(firstProductFeatureInfo);
            fifthProductFeatureInfo.ShouldNotBeSameAs(firstProductFeatureInfo);
            sixthProductFeatureInfo.ShouldNotBeSameAs(firstProductFeatureInfo);
            sixthProductFeatureInfo.ShouldNotBeSameAs(fourthProductFeatureInfo);
        }

        #endregion

        #endregion

        #endregion
    }
}