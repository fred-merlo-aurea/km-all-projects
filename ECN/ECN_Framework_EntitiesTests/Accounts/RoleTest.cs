using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Shouldly;
using AutoFixture;
using NUnit.Framework;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_EntitiesTests.Accounts
{
    [TestFixture]
    public class RoleTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Role_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var role = new Role();
            var roleID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var roleName = Fixture.Create<string>();
            var createdUserID = Fixture.Create<int?>();
            var updatedUserID = Fixture.Create<int?>();
            var isDeleted = Fixture.Create<bool?>();
            var actionList = Fixture.Create<List<RoleAction>>();

            // Act
            role.RoleID = roleID;
            role.CustomerID = customerID;
            role.RoleName = roleName;
            role.CreatedUserID = createdUserID;
            role.UpdatedUserID = updatedUserID;
            role.IsDeleted = isDeleted;
            role.actionList = actionList;

            // Assert
            role.RoleID.ShouldBe(roleID);
            role.CustomerID.ShouldBe(customerID);
            role.RoleName.ShouldBe(roleName);
            role.CreatedUserID.ShouldBe(createdUserID);
            role.UpdatedUserID.ShouldBe(updatedUserID);
            role.IsDeleted.ShouldBe(isDeleted);
            role.actionList.ShouldBe(actionList);
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Nullable Property Test : Role => IsDeleted

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Data_Without_Null_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();
            var random = Fixture.Create<bool>();

            // Act , Set
            role.IsDeleted = random;

            // Assert
            role.IsDeleted.ShouldBe(random);
            role.IsDeleted.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Only_Null_Data_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();

            // Act , Set
            role.IsDeleted = null;

            // Assert
            role.IsDeleted.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constIsDeleted = "IsDeleted";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constIsDeleted);

            // Act , Set
            propertyInfo.SetValue(role, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            role.IsDeleted.ShouldBeNull();
            role.IsDeleted.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Role_Class_Invalid_Property_IsDeleted_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constIsDeleted));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_IsDeleted_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constIsDeleted = "IsDeleted";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Role => CreatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var role = Fixture.Create<Role>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = role.GetType().GetProperty(constCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(role, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Role_Class_Invalid_Property_CreatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constCreatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedDate_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedDate = "CreatedDate";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : Role => UpdatedDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var role = Fixture.Create<Role>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = role.GetType().GetProperty(constUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(role, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Role_Class_Invalid_Property_UpdatedDate_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constUpdatedDate));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedDate_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedDate = "UpdatedDate";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Role => CustomerID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Int_Type_Verify_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();
            var intType = role.CustomerID.GetType();

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
        public void Role_Class_Invalid_Property_CustomerID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constCustomerID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CustomerID_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constCustomerID = "CustomerID";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : Role => RoleID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleID_Int_Type_Verify_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();
            var intType = role.RoleID.GetType();

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
        public void Role_Class_Invalid_Property_RoleID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRoleID = "RoleID";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constRoleID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleID_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constRoleID = "RoleID";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constRoleID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Role => CreatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();
            var random = Fixture.Create<int>();

            // Act , Set
            role.CreatedUserID = random;

            // Assert
            role.CreatedUserID.ShouldBe(random);
            role.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();

            // Act , Set
            role.CreatedUserID = null;

            // Assert
            role.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constCreatedUserID = "CreatedUserID";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(role, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            role.CreatedUserID.ShouldBeNull();
            role.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Role_Class_Invalid_Property_CreatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constCreatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_CreatedUserID_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constCreatedUserID = "CreatedUserID";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : Role => UpdatedUserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Data_Without_Null_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();
            var random = Fixture.Create<int>();

            // Act , Set
            role.UpdatedUserID = random;

            // Assert
            role.UpdatedUserID.ShouldBe(random);
            role.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Only_Null_Data_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();

            // Act , Set
            role.UpdatedUserID = null;

            // Assert
            role.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUpdatedUserID = "UpdatedUserID";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(role, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            role.UpdatedUserID.ShouldBeNull();
            role.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Role_Class_Invalid_Property_UpdatedUserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constUpdatedUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UpdatedUserID_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constUpdatedUserID = "UpdatedUserID";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constUpdatedUserID);

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
        public void Role_Class_Invalid_Property_actionList_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constactionList = "actionList";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constactionList));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_actionList_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constactionList = "actionList";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constactionList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : Role => RoleName

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleName_String_Type_Verify_Test()
        {
            // Arrange
            var role = Fixture.Create<Role>();
            var stringType = role.RoleName.GetType();

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
        public void Role_Class_Invalid_Property_RoleName_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constRoleName = "RoleName";
            var role = Fixture.Create<Role>();

            // Act , Assert
            Should.NotThrow(() => role.GetType().GetProperty(constRoleName));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_RoleName_Is_Present_In_Role_Class_As_Public_Test()
        {
            // Arrange
            const string constRoleName = "RoleName";
            var role = Fixture.Create<Role>();
            var propertyInfo = role.GetType().GetProperty(constRoleName);

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
            Should.NotThrow(() => new Role());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<Role>(2).ToList();
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
            var roleID = -1;
            var customerID = -1;
            var roleName = string.Empty;
            int? createdUserID = null;
            DateTime? createdDate = null;
            int? updatedUserID = null;
            DateTime? updatedDate = null;
            bool? isDeleted = null;
            List<RoleAction> actionList = null;

            // Act
            var role = new Role();

            // Assert
            role.RoleID.ShouldBe(roleID);
            role.CustomerID.ShouldBe(customerID);
            role.RoleName.ShouldBe(roleName);
            role.CreatedUserID.ShouldBeNull();
            role.CreatedDate.ShouldBeNull();
            role.UpdatedUserID.ShouldBeNull();
            role.UpdatedDate.ShouldBeNull();
            role.IsDeleted.ShouldBeNull();
            role.actionList.ShouldBe(actionList);
        }

        #endregion

        #endregion

        #endregion
    }
}