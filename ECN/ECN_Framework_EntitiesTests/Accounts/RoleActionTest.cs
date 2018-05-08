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
    public class RoleActionTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var roleAction  = new RoleAction();
            var roleActionID = Fixture.Create<int>();
            var roleID = Fixture.Create<int?>();
            var actionID = Fixture.Create<int?>();
            var active = Fixture.Create<string>();
            var customerID = Fixture.Create<int>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            roleAction.RoleActionID = roleActionID;
            roleAction.RoleID = roleID;
            roleAction.ActionID = actionID;
            roleAction.Active = active;
            roleAction.CustomerID = customerID;
            roleAction.CreatedUserID = createdUserID;
            roleAction.UpdatedUserID = updatedUserID;
            roleAction.IsDeleted = isDeleted;
            roleAction.ErrorList = errorList;

            // Assert
            roleAction.RoleActionID.ShouldBe(roleActionID);
            roleAction.RoleID.ShouldBe(roleID);
            roleAction.ActionID.ShouldBe(actionID);
            roleAction.Active.ShouldBe(active);
            roleAction.CustomerID.ShouldBe(customerID);
            roleAction.CreatedUserID.ShouldBe(createdUserID);
            roleAction.UpdatedUserID.ShouldBe(updatedUserID);
            roleAction.IsDeleted.ShouldBe(isDeleted);
            roleAction.ErrorList.ShouldBe(errorList);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : RoleAction => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var random = Fixture.Create<bool>();

            // Act , Set
            roleAction.IsDeleted = random;

            // Assert
            roleAction.IsDeleted.ShouldBe(random);
            roleAction.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();    

            // Act , Set
            roleAction.IsDeleted = null;

            // Assert
            roleAction.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var roleAction = Fixture.Create<RoleAction>();
            var propertyInfo = roleAction.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(roleAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            roleAction.IsDeleted.ShouldBeNull();
            roleAction.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : RoleAction => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var roleAction = Fixture.Create<RoleAction>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = roleAction.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(roleAction, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : RoleAction => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var roleAction = Fixture.Create<RoleAction>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = roleAction.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(roleAction, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : RoleAction => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Int_Type_Verify_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var intType = roleAction.CustomerID.GetType();

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
        public void RoleAction_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : RoleAction => RoleActionID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleActionID_Int_Type_Verify_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var intType = roleAction.RoleActionID.GetType();

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
        public void RoleAction_Class_Invalid_Property_RoleActionID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRoleActionID = "RoleActionID";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constRoleActionID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleActionID_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constRoleActionID = "RoleActionID";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constRoleActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : RoleAction => ActionID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Data_Without_Null_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var random = Fixture.Create<int>();

            // Act , Set
            roleAction.ActionID = random;

            // Assert
            roleAction.ActionID.ShouldBe(random);
            roleAction.ActionID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Only_Null_Data_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();    

            // Act , Set
            roleAction.ActionID = null;

            // Assert
            roleAction.ActionID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constActionID = "ActionID";
            var roleAction = Fixture.Create<RoleAction>();
            var propertyInfo = roleAction.GetType().GetProperty(constActionID);

            // Act , Set
            propertyInfo.SetValue(roleAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            roleAction.ActionID.ShouldBeNull();
            roleAction.ActionID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_ActionID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constActionID = "ActionID";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constActionID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constActionID = "ActionID";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : RoleAction => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var random = Fixture.Create<int>();

            // Act , Set
            roleAction.CreatedUserID = random;

            // Assert
            roleAction.CreatedUserID.ShouldBe(random);
            roleAction.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();    

            // Act , Set
            roleAction.CreatedUserID = null;

            // Assert
            roleAction.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var roleAction = Fixture.Create<RoleAction>();
            var propertyInfo = roleAction.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(roleAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            roleAction.CreatedUserID.ShouldBeNull();
            roleAction.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : RoleAction => RoleID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleID_Data_Without_Null_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var random = Fixture.Create<int>();

            // Act , Set
            roleAction.RoleID = random;

            // Assert
            roleAction.RoleID.ShouldBe(random);
            roleAction.RoleID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleID_Only_Null_Data_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();    

            // Act , Set
            roleAction.RoleID = null;

            // Assert
            roleAction.RoleID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constRoleID = "RoleID";
            var roleAction = Fixture.Create<RoleAction>();
            var propertyInfo = roleAction.GetType().GetProperty(constRoleID);

            // Act , Set
            propertyInfo.SetValue(roleAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            roleAction.RoleID.ShouldBeNull();
            roleAction.RoleID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_RoleID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRoleID = "RoleID";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constRoleID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleID_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constRoleID = "RoleID";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constRoleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : RoleAction => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var random = Fixture.Create<int>();

            // Act , Set
            roleAction.UpdatedUserID = random;

            // Assert
            roleAction.UpdatedUserID.ShouldBe(random);
            roleAction.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();    

            // Act , Set
            roleAction.UpdatedUserID = null;

            // Assert
            roleAction.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var roleAction = Fixture.Create<RoleAction>();
            var propertyInfo = roleAction.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(roleAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            roleAction.UpdatedUserID.ShouldBeNull();
            roleAction.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void RoleAction_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constUpdatedUserID);

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
        public void RoleAction_Class_Invalid_Property_ErrorList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constErrorList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ErrorList_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constErrorList = "ErrorList";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : RoleAction => Active

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Active_String_Type_Verify_Test()
        {
            // Arrange
            var roleAction = Fixture.Create<RoleAction>();
            var stringType = roleAction.Active.GetType();

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
        public void RoleAction_Class_Invalid_Property_Active_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constActive = "Active";
            var roleAction  = Fixture.Create<RoleAction>();

            // Act , Assert
            Should.NotThrow(() => roleAction.GetType().GetProperty(constActive));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Active_Is_Present_In_RoleAction_Class_As_Public_Test()
        {
            // Arrange
            const string constActive = "Active";
            var roleAction  = Fixture.Create<RoleAction>();
            var propertyInfo  = roleAction.GetType().GetProperty(constActive);

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
            Should.NotThrow(() => new RoleAction());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<RoleAction>(2).ToList();
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
            var roleActionID = -1;
            int? roleID = null;
            int? actionID = null;
            var active = string.Empty;
            var customerID = -1;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;
            var errorList = new List<ECNError>();    

            // Act
            var roleAction = new RoleAction();    

            // Assert
            roleAction.RoleActionID.ShouldBe(roleActionID);
            roleAction.RoleID.ShouldBeNull();
            roleAction.ActionID.ShouldBeNull();
            roleAction.Active.ShouldBe(active);
            roleAction.CustomerID.ShouldBe(customerID);
            roleAction.CreatedUserID.ShouldBeNull();
            roleAction.CreatedDate.ShouldBeNull();
            roleAction.UpdatedUserID.ShouldBeNull();
            roleAction.UpdatedDate.ShouldBeNull();
            roleAction.IsDeleted.ShouldBeNull();
            roleAction.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #endregion

        #endregion
    }
}