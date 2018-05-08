using System;
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
    public class UserActionTest : AbstractGenericTest
    {
        #region General Category : General

        #region Category : GetterSetter

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserAction_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var userAction  = new UserAction();
            var userActionID = Fixture.Create<int>();
            var userID = Fixture.Create<int?>();
            var actionID = Fixture.Create<int?>();
            var active = Fixture.Create<string>();

            // Act
            userAction.UserActionID = userActionID;
            userAction.UserID = userID;
            userAction.ActionID = actionID;
            userAction.Active = active;

            // Assert
            userAction.UserActionID.ShouldBe(userActionID);
            userAction.UserID.ShouldBe(userID);
            userAction.ActionID.ShouldBe(actionID);
            userAction.Active.ShouldBe(active);   
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region int property type test : UserAction => UserActionID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UserActionID_Int_Type_Verify_Test()
        {
            // Arrange
            var userAction = Fixture.Create<UserAction>();
            var intType = userAction.UserActionID.GetType();

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
        public void UserAction_Class_Invalid_Property_UserActionID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUserActionID = "UserActionID";
            var userAction  = Fixture.Create<UserAction>();

            // Act , Assert
            Should.NotThrow(() => userAction.GetType().GetProperty(constUserActionID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UserActionID_Is_Present_In_UserAction_Class_As_Public_Test()
        {
            // Arrange
            const string constUserActionID = "UserActionID";
            var userAction  = Fixture.Create<UserAction>();
            var propertyInfo  = userAction.GetType().GetProperty(constUserActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : UserAction => ActionID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Data_Without_Null_Test()
        {
            // Arrange
            var userAction = Fixture.Create<UserAction>();
            var random = Fixture.Create<int>();

            // Act , Set
            userAction.ActionID = random;

            // Assert
            userAction.ActionID.ShouldBe(random);
            userAction.ActionID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Only_Null_Data_Test()
        {
            // Arrange
            var userAction = Fixture.Create<UserAction>();    

            // Act , Set
            userAction.ActionID = null;

            // Assert
            userAction.ActionID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constActionID = "ActionID";
            var userAction = Fixture.Create<UserAction>();
            var propertyInfo = userAction.GetType().GetProperty(constActionID);

            // Act , Set
            propertyInfo.SetValue(userAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            userAction.ActionID.ShouldBeNull();
            userAction.ActionID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserAction_Class_Invalid_Property_ActionID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constActionID = "ActionID";
            var userAction  = Fixture.Create<UserAction>();

            // Act , Assert
            Should.NotThrow(() => userAction.GetType().GetProperty(constActionID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_ActionID_Is_Present_In_UserAction_Class_As_Public_Test()
        {
            // Arrange
            const string constActionID = "ActionID";
            var userAction  = Fixture.Create<UserAction>();
            var propertyInfo  = userAction.GetType().GetProperty(constActionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Nullable Property Test : UserAction => UserID

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UserID_Data_Without_Null_Test()
        {
            // Arrange
            var userAction = Fixture.Create<UserAction>();
            var random = Fixture.Create<int>();

            // Act , Set
            userAction.UserID = random;

            // Assert
            userAction.UserID.ShouldBe(random);
            userAction.UserID.ShouldNotBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UserID_Only_Null_Data_Test()
        {
            // Arrange
            var userAction = Fixture.Create<UserAction>();    

            // Act , Set
            userAction.UserID = null;

            // Assert
            userAction.UserID.ShouldBeNull();
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UserID_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string constUserID = "UserID";
            var userAction = Fixture.Create<UserAction>();
            var propertyInfo = userAction.GetType().GetProperty(constUserID);

            // Act , Set
            propertyInfo.SetValue(userAction, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            userAction.UserID.ShouldBeNull();
            userAction.UserID.ShouldBe(null);
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void UserAction_Class_Invalid_Property_UserID_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constUserID = "UserID";
            var userAction  = Fixture.Create<UserAction>();

            // Act , Assert
            Should.NotThrow(() => userAction.GetType().GetProperty(constUserID));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_UserID_Is_Present_In_UserAction_Class_As_Public_Test()
        {
            // Arrange
            const string constUserID = "UserID";
            var userAction  = Fixture.Create<UserAction>();
            var propertyInfo  = userAction.GetType().GetProperty(constUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : UserAction => Active

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Active_String_Type_Verify_Test()
        {
            // Arrange
            var userAction = Fixture.Create<UserAction>();
            var stringType = userAction.Active.GetType();

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
        public void UserAction_Class_Invalid_Property_Active_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string constActive = "Active";
            var userAction  = Fixture.Create<UserAction>();

            // Act , Assert
            Should.NotThrow(() => userAction.GetType().GetProperty(constActive));
        }

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Property_Active_Is_Present_In_UserAction_Class_As_Public_Test()
        {
            // Arrange
            const string constActive = "Active";
            var userAction  = Fixture.Create<UserAction>();
            var propertyInfo  = userAction.GetType().GetProperty(constActive);

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
            Should.NotThrow(() => new UserAction());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var myInstances = Fixture.CreateMany<UserAction>(2).ToList();
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
            var userActionID = -1;
            int? userID = null;
            int? actionID = null;
            var active = string.Empty;    

            // Act
            var userAction = new UserAction();    

            // Assert
            userAction.UserActionID.ShouldBe(userActionID);
            userAction.UserID.ShouldBeNull();
            userAction.ActionID.ShouldBeNull();
            userAction.Active.ShouldBe(active);
        }

        #endregion

        #endregion

        #endregion
    }
}