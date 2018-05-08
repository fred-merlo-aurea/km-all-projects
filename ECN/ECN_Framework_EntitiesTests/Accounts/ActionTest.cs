using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_Entities.Accounts
{
    [TestFixture]
    public class ActionTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Action) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            var actionId = Fixture.Create<int>();
            var productId = Fixture.Create<int?>();
            var displayName = Fixture.Create<string>();
            var actionCode = Fixture.Create<string>();
            var displayOrder = Fixture.Create<int?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();

            // Act
            action.ActionID = actionId;
            action.ProductID = productId;
            action.DisplayName = displayName;
            action.ActionCode = actionCode;
            action.DisplayOrder = displayOrder;
            action.CreatedUserID = createdUserId;
            action.UpdatedUserID = updatedUserId;
            action.IsDeleted = isDeleted;

            // Assert
            action.ActionID.ShouldBe(actionId);
            action.ProductID.ShouldBe(productId);
            action.DisplayName.ShouldBe(displayName);
            action.ActionCode.ShouldBe(actionCode);
            action.DisplayOrder.ShouldBe(displayOrder);
            action.CreatedUserID.ShouldBe(createdUserId);
            action.CreatedDate.ShouldBeNull();
            action.UpdatedUserID.ShouldBe(updatedUserId);
            action.UpdatedDate.ShouldBeNull();
            action.IsDeleted.ShouldBe(isDeleted);
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (ActionCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ActionCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            action.ActionCode = Fixture.Create<string>();
            var stringType = action.ActionCode.GetType();

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

        #region General Getters/Setters : Class (Action) => Property (ActionCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_ActionCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionCode = "ActionCodeNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameActionCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ActionCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionCode = "ActionCode";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameActionCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (ActionID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ActionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            action.ActionID = Fixture.Create<int>();
            var intType = action.ActionID.GetType();

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

        #region General Getters/Setters : Class (Action) => Property (ActionID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_ActionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionID = "ActionIDNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameActionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ActionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionID = "ActionID";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var action = Fixture.Create<Action>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = action.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(action, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            var random = Fixture.Create<int>();

            // Act , Set
            action.CreatedUserID = random;

            // Assert
            action.CreatedUserID.ShouldBe(random);
            action.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();    

            // Act , Set
            action.CreatedUserID = null;

            // Assert
            action.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var action = Fixture.Create<Action>();
            var propertyInfo = action.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(action, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            action.CreatedUserID.ShouldBeNull();
            action.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (DisplayName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_DisplayName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            action.DisplayName = Fixture.Create<string>();
            var stringType = action.DisplayName.GetType();

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

        #region General Getters/Setters : Class (Action) => Property (DisplayName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_DisplayNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayNameNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameDisplayName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_DisplayName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayName = "DisplayName";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameDisplayName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (DisplayOrder) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_DisplayOrder_Property_Data_Without_Null_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            var random = Fixture.Create<int>();

            // Act , Set
            action.DisplayOrder = random;

            // Assert
            action.DisplayOrder.ShouldBe(random);
            action.DisplayOrder.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_DisplayOrder_Property_Only_Null_Data_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();    

            // Act , Set
            action.DisplayOrder = null;

            // Assert
            action.DisplayOrder.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_DisplayOrder_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDisplayOrder = "DisplayOrder";
            var action = Fixture.Create<Action>();
            var propertyInfo = action.GetType().GetProperty(propertyNameDisplayOrder);

            // Act , Set
            propertyInfo.SetValue(action, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            action.DisplayOrder.ShouldBeNull();
            action.DisplayOrder.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (DisplayOrder) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_DisplayOrderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDisplayOrder = "DisplayOrderNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameDisplayOrder));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_DisplayOrder_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDisplayOrder = "DisplayOrder";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameDisplayOrder);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (IsDeleted) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_IsDeleted_Property_Data_Without_Null_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            var random = Fixture.Create<bool>();

            // Act , Set
            action.IsDeleted = random;

            // Assert
            action.IsDeleted.ShouldBe(random);
            action.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_IsDeleted_Property_Only_Null_Data_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();    

            // Act , Set
            action.IsDeleted = null;

            // Assert
            action.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_IsDeleted_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameIsDeleted = "IsDeleted";
            var action = Fixture.Create<Action>();
            var propertyInfo = action.GetType().GetProperty(propertyNameIsDeleted);

            // Act , Set
            propertyInfo.SetValue(action, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            action.IsDeleted.ShouldBeNull();
            action.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (ProductID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ProductID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            var random = Fixture.Create<int>();

            // Act , Set
            action.ProductID = random;

            // Assert
            action.ProductID.ShouldBe(random);
            action.ProductID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ProductID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();    

            // Act , Set
            action.ProductID = null;

            // Assert
            action.ProductID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ProductID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameProductID = "ProductID";
            var action = Fixture.Create<Action>();
            var propertyInfo = action.GetType().GetProperty(propertyNameProductID);

            // Act , Set
            propertyInfo.SetValue(action, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            action.ProductID.ShouldBeNull();
            action.ProductID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (ProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductIDNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductID";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var action = Fixture.Create<Action>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = action.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(action, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();
            var random = Fixture.Create<int>();

            // Act , Set
            action.UpdatedUserID = random;

            // Assert
            action.UpdatedUserID.ShouldBe(random);
            action.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var action = Fixture.Create<Action>();    

            // Act , Set
            action.UpdatedUserID = null;

            // Assert
            action.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var action = Fixture.Create<Action>();
            var propertyInfo = action.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(action, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            action.UpdatedUserID.ShouldBeNull();
            action.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Action) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var action  = Fixture.Create<Action>();

            // Act , Assert
            Should.NotThrow(() => action.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Action_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var action  = Fixture.Create<Action>();
            var propertyInfo  = action.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Action) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Action_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Action());
        }

        #endregion

        #region General Constructor : Class (Action) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Action_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfAction = Fixture.CreateMany<Action>(2).ToList();
            var firstAction = instancesOfAction.FirstOrDefault();
            var lastAction = instancesOfAction.Last();

            // Act, Assert
            firstAction.ShouldNotBeNull();
            lastAction.ShouldNotBeNull();
            firstAction.ShouldNotBeSameAs(lastAction);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Action_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstAction = new Action();
            var secondAction = new Action();
            var thirdAction = new Action();
            var fourthAction = new Action();
            var fifthAction = new Action();
            var sixthAction = new Action();

            // Act, Assert
            firstAction.ShouldNotBeNull();
            secondAction.ShouldNotBeNull();
            thirdAction.ShouldNotBeNull();
            fourthAction.ShouldNotBeNull();
            fifthAction.ShouldNotBeNull();
            sixthAction.ShouldNotBeNull();
            firstAction.ShouldNotBeSameAs(secondAction);
            thirdAction.ShouldNotBeSameAs(firstAction);
            fourthAction.ShouldNotBeSameAs(firstAction);
            fifthAction.ShouldNotBeSameAs(firstAction);
            sixthAction.ShouldNotBeSameAs(firstAction);
            sixthAction.ShouldNotBeSameAs(fourthAction);
        }

        #endregion

        #region General Constructor : Class (Action) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Action_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var actionId = -1;
            var displayName = string.Empty;
            var actionCode = string.Empty;

            // Act
            var action = new Action();

            // Assert
            action.ActionID.ShouldBe(actionId);
            action.ProductID.ShouldBeNull();
            action.DisplayName.ShouldBe(displayName);
            action.ActionCode.ShouldBe(actionCode);
            action.DisplayOrder.ShouldBeNull();
            action.CreatedUserID.ShouldBeNull();
            action.CreatedDate.ShouldBeNull();
            action.UpdatedUserID.ShouldBeNull();
            action.UpdatedDate.ShouldBeNull();
            action.IsDeleted.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}