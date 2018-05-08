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
    public class UserDepartmentTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (UserDepartment) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();
            var userDepartmentId = Fixture.Create<int>();
            var userId = Fixture.Create<int>();
            var departmentId = Fixture.Create<int?>();
            var isDefaultDept = Fixture.Create<bool>();
            var customerId = Fixture.Create<int?>();

            // Act
            userDepartment.UserDepartmentID = userDepartmentId;
            userDepartment.UserID = userId;
            userDepartment.DepartmentID = departmentId;
            userDepartment.IsDefaultDept = isDefaultDept;
            userDepartment.CustomerID = customerId;

            // Assert
            userDepartment.UserDepartmentID.ShouldBe(userDepartmentId);
            userDepartment.UserID.ShouldBe(userId);
            userDepartment.DepartmentID.ShouldBe(departmentId);
            userDepartment.IsDefaultDept.ShouldBe(isDefaultDept);
            userDepartment.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();
            var random = Fixture.Create<int>();

            // Act , Set
            userDepartment.CustomerID = random;

            // Assert
            userDepartment.CustomerID.ShouldBe(random);
            userDepartment.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();    

            // Act , Set
            userDepartment.CustomerID = null;

            // Assert
            userDepartment.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var userDepartment = Fixture.Create<UserDepartment>();
            var propertyInfo = userDepartment.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(userDepartment, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            userDepartment.CustomerID.ShouldBeNull();
            userDepartment.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var userDepartment  = Fixture.Create<UserDepartment>();

            // Act , Assert
            Should.NotThrow(() => userDepartment.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var userDepartment  = Fixture.Create<UserDepartment>();
            var propertyInfo  = userDepartment.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (DepartmentID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_DepartmentID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();
            var random = Fixture.Create<int>();

            // Act , Set
            userDepartment.DepartmentID = random;

            // Assert
            userDepartment.DepartmentID.ShouldBe(random);
            userDepartment.DepartmentID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_DepartmentID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();    

            // Act , Set
            userDepartment.DepartmentID = null;

            // Assert
            userDepartment.DepartmentID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_DepartmentID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameDepartmentID = "DepartmentID";
            var userDepartment = Fixture.Create<UserDepartment>();
            var propertyInfo = userDepartment.GetType().GetProperty(propertyNameDepartmentID);

            // Act , Set
            propertyInfo.SetValue(userDepartment, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            userDepartment.DepartmentID.ShouldBeNull();
            userDepartment.DepartmentID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (DepartmentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_Class_Invalid_Property_DepartmentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDepartmentID = "DepartmentIDNotPresent";
            var userDepartment  = Fixture.Create<UserDepartment>();

            // Act , Assert
            Should.NotThrow(() => userDepartment.GetType().GetProperty(propertyNameDepartmentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_DepartmentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDepartmentID = "DepartmentID";
            var userDepartment  = Fixture.Create<UserDepartment>();
            var propertyInfo  = userDepartment.GetType().GetProperty(propertyNameDepartmentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (IsDefaultDept) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_IsDefaultDept_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();
            userDepartment.IsDefaultDept = Fixture.Create<bool>();
            var boolType = userDepartment.IsDefaultDept.GetType();

            // Act
            var isTypeBool = typeof(bool) == (boolType);
            var isTypeNullableBool = typeof(bool?) == (boolType);
            var isTypeString = typeof(string) == (boolType);
            var isTypeInt = typeof(int) == (boolType);
            var isTypeDecimal = typeof(decimal) == (boolType);
            var isTypeLong = typeof(long) == (boolType);
            var isTypeDouble = typeof(double) == (boolType);
            var isTypeFloat = typeof(float) == (boolType);
            var isTypeIntNullable = typeof(int?) == (boolType);
            var isTypeDecimalNullable = typeof(decimal?) == (boolType);
            var isTypeLongNullable = typeof(long?) == (boolType);
            var isTypeDoubleNullable = typeof(double?) == (boolType);
            var isTypeFloatNullable = typeof(float?) == (boolType);

            // Assert
            isTypeBool.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableBool.ShouldBeFalse();
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

        #region General Getters/Setters : Class (UserDepartment) => Property (IsDefaultDept) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_Class_Invalid_Property_IsDefaultDeptNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDefaultDept = "IsDefaultDeptNotPresent";
            var userDepartment  = Fixture.Create<UserDepartment>();

            // Act , Assert
            Should.NotThrow(() => userDepartment.GetType().GetProperty(propertyNameIsDefaultDept));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_IsDefaultDept_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDefaultDept = "IsDefaultDept";
            var userDepartment  = Fixture.Create<UserDepartment>();
            var propertyInfo  = userDepartment.GetType().GetProperty(propertyNameIsDefaultDept);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (UserDepartmentID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_UserDepartmentID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();
            userDepartment.UserDepartmentID = Fixture.Create<int>();
            var intType = userDepartment.UserDepartmentID.GetType();

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

        #region General Getters/Setters : Class (UserDepartment) => Property (UserDepartmentID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_Class_Invalid_Property_UserDepartmentIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserDepartmentID = "UserDepartmentIDNotPresent";
            var userDepartment  = Fixture.Create<UserDepartment>();

            // Act , Assert
            Should.NotThrow(() => userDepartment.GetType().GetProperty(propertyNameUserDepartmentID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_UserDepartmentID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserDepartmentID = "UserDepartmentID";
            var userDepartment  = Fixture.Create<UserDepartment>();
            var propertyInfo  = userDepartment.GetType().GetProperty(propertyNameUserDepartmentID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (UserDepartment) => Property (UserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_UserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var userDepartment = Fixture.Create<UserDepartment>();
            userDepartment.UserID = Fixture.Create<int>();
            var intType = userDepartment.UserID.GetType();

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

        #region General Getters/Setters : Class (UserDepartment) => Property (UserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_Class_Invalid_Property_UserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserIDNotPresent";
            var userDepartment  = Fixture.Create<UserDepartment>();

            // Act , Assert
            Should.NotThrow(() => userDepartment.GetType().GetProperty(propertyNameUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserDepartment_UserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserID";
            var userDepartment  = Fixture.Create<UserDepartment>();
            var propertyInfo  = userDepartment.GetType().GetProperty(propertyNameUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (UserDepartment) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UserDepartment_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new UserDepartment());
        }

        #endregion

        #region General Constructor : Class (UserDepartment) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UserDepartment_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfUserDepartment = Fixture.CreateMany<UserDepartment>(2).ToList();
            var firstUserDepartment = instancesOfUserDepartment.FirstOrDefault();
            var lastUserDepartment = instancesOfUserDepartment.Last();

            // Act, Assert
            firstUserDepartment.ShouldNotBeNull();
            lastUserDepartment.ShouldNotBeNull();
            firstUserDepartment.ShouldNotBeSameAs(lastUserDepartment);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UserDepartment_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstUserDepartment = new UserDepartment();
            var secondUserDepartment = new UserDepartment();
            var thirdUserDepartment = new UserDepartment();
            var fourthUserDepartment = new UserDepartment();
            var fifthUserDepartment = new UserDepartment();
            var sixthUserDepartment = new UserDepartment();

            // Act, Assert
            firstUserDepartment.ShouldNotBeNull();
            secondUserDepartment.ShouldNotBeNull();
            thirdUserDepartment.ShouldNotBeNull();
            fourthUserDepartment.ShouldNotBeNull();
            fifthUserDepartment.ShouldNotBeNull();
            sixthUserDepartment.ShouldNotBeNull();
            firstUserDepartment.ShouldNotBeSameAs(secondUserDepartment);
            thirdUserDepartment.ShouldNotBeSameAs(firstUserDepartment);
            fourthUserDepartment.ShouldNotBeSameAs(firstUserDepartment);
            fifthUserDepartment.ShouldNotBeSameAs(firstUserDepartment);
            sixthUserDepartment.ShouldNotBeSameAs(firstUserDepartment);
            sixthUserDepartment.ShouldNotBeSameAs(fourthUserDepartment);
        }

        #endregion

        #region General Constructor : Class (UserDepartment) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_UserDepartment_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var userDepartmentId = -1;
            var userId = -1;
            var departmentId = -1;
            var isDefaultDept = false;

            // Act
            var userDepartment = new UserDepartment();

            // Assert
            userDepartment.UserDepartmentID.ShouldBe(userDepartmentId);
            userDepartment.UserID.ShouldBe(userId);
            userDepartment.DepartmentID.ShouldBe(departmentId);
            userDepartment.IsDefaultDept.ShouldBeFalse();
            userDepartment.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}