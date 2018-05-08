using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class EmailHistoryTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailHistory) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailHistory = Fixture.Create<EmailHistory>();
            var oldEmailId = Fixture.Create<int>();
            var newEmailId = Fixture.Create<int>();
            var action = Fixture.Create<string>();
            var oldGroupId = Fixture.Create<int>();
            var actionTime = Fixture.Create<DateTime>();

            // Act
            emailHistory.OldEmailID = oldEmailId;
            emailHistory.NewEmailID = newEmailId;
            emailHistory.Action = action;
            emailHistory.OldGroupID = oldGroupId;
            emailHistory.ActionTime = actionTime;

            // Assert
            emailHistory.OldEmailID.ShouldBe(oldEmailId);
            emailHistory.NewEmailID.ShouldBe(newEmailId);
            emailHistory.Action.ShouldBe(action);
            emailHistory.OldGroupID.ShouldBe(oldGroupId);
            emailHistory.ActionTime.ShouldBe(actionTime);
        }

        #endregion

        #region General Getters/Setters : Class (EmailHistory) => Property (Action) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Action_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailHistory = Fixture.Create<EmailHistory>();
            emailHistory.Action = Fixture.Create<string>();
            var stringType = emailHistory.Action.GetType();

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

        #region General Getters/Setters : Class (EmailHistory) => Property (Action) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Class_Invalid_Property_ActionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAction = "ActionNotPresent";
            var emailHistory  = Fixture.Create<EmailHistory>();

            // Act , Assert
            Should.NotThrow(() => emailHistory.GetType().GetProperty(propertyNameAction));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Action_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAction = "Action";
            var emailHistory  = Fixture.Create<EmailHistory>();
            var propertyInfo  = emailHistory.GetType().GetProperty(propertyNameAction);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailHistory) => Property (ActionTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_ActionTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTime = "ActionTime";
            var emailHistory = Fixture.Create<EmailHistory>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailHistory.GetType().GetProperty(propertyNameActionTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailHistory, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailHistory) => Property (ActionTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Class_Invalid_Property_ActionTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTime = "ActionTimeNotPresent";
            var emailHistory  = Fixture.Create<EmailHistory>();

            // Act , Assert
            Should.NotThrow(() => emailHistory.GetType().GetProperty(propertyNameActionTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_ActionTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTime = "ActionTime";
            var emailHistory  = Fixture.Create<EmailHistory>();
            var propertyInfo  = emailHistory.GetType().GetProperty(propertyNameActionTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailHistory) => Property (NewEmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_NewEmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailHistory = Fixture.Create<EmailHistory>();
            emailHistory.NewEmailID = Fixture.Create<int>();
            var intType = emailHistory.NewEmailID.GetType();

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

        #region General Getters/Setters : Class (EmailHistory) => Property (NewEmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Class_Invalid_Property_NewEmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewEmailID = "NewEmailIDNotPresent";
            var emailHistory  = Fixture.Create<EmailHistory>();

            // Act , Assert
            Should.NotThrow(() => emailHistory.GetType().GetProperty(propertyNameNewEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_NewEmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewEmailID = "NewEmailID";
            var emailHistory  = Fixture.Create<EmailHistory>();
            var propertyInfo  = emailHistory.GetType().GetProperty(propertyNameNewEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailHistory) => Property (OldEmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_OldEmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailHistory = Fixture.Create<EmailHistory>();
            emailHistory.OldEmailID = Fixture.Create<int>();
            var intType = emailHistory.OldEmailID.GetType();

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

        #region General Getters/Setters : Class (EmailHistory) => Property (OldEmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Class_Invalid_Property_OldEmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOldEmailID = "OldEmailIDNotPresent";
            var emailHistory  = Fixture.Create<EmailHistory>();

            // Act , Assert
            Should.NotThrow(() => emailHistory.GetType().GetProperty(propertyNameOldEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_OldEmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOldEmailID = "OldEmailID";
            var emailHistory  = Fixture.Create<EmailHistory>();
            var propertyInfo  = emailHistory.GetType().GetProperty(propertyNameOldEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailHistory) => Property (OldGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_OldGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailHistory = Fixture.Create<EmailHistory>();
            emailHistory.OldGroupID = Fixture.Create<int>();
            var intType = emailHistory.OldGroupID.GetType();

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

        #region General Getters/Setters : Class (EmailHistory) => Property (OldGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_Class_Invalid_Property_OldGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOldGroupID = "OldGroupIDNotPresent";
            var emailHistory  = Fixture.Create<EmailHistory>();

            // Act , Assert
            Should.NotThrow(() => emailHistory.GetType().GetProperty(propertyNameOldGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailHistory_OldGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOldGroupID = "OldGroupID";
            var emailHistory  = Fixture.Create<EmailHistory>();
            var propertyInfo  = emailHistory.GetType().GetProperty(propertyNameOldGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailHistory) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailHistory_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailHistory());
        }

        #endregion

        #region General Constructor : Class (EmailHistory) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailHistory_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailHistory = Fixture.CreateMany<EmailHistory>(2).ToList();
            var firstEmailHistory = instancesOfEmailHistory.FirstOrDefault();
            var lastEmailHistory = instancesOfEmailHistory.Last();

            // Act, Assert
            firstEmailHistory.ShouldNotBeNull();
            lastEmailHistory.ShouldNotBeNull();
            firstEmailHistory.ShouldNotBeSameAs(lastEmailHistory);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailHistory_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailHistory = new EmailHistory();
            var secondEmailHistory = new EmailHistory();
            var thirdEmailHistory = new EmailHistory();
            var fourthEmailHistory = new EmailHistory();
            var fifthEmailHistory = new EmailHistory();
            var sixthEmailHistory = new EmailHistory();

            // Act, Assert
            firstEmailHistory.ShouldNotBeNull();
            secondEmailHistory.ShouldNotBeNull();
            thirdEmailHistory.ShouldNotBeNull();
            fourthEmailHistory.ShouldNotBeNull();
            fifthEmailHistory.ShouldNotBeNull();
            sixthEmailHistory.ShouldNotBeNull();
            firstEmailHistory.ShouldNotBeSameAs(secondEmailHistory);
            thirdEmailHistory.ShouldNotBeSameAs(firstEmailHistory);
            fourthEmailHistory.ShouldNotBeSameAs(firstEmailHistory);
            fifthEmailHistory.ShouldNotBeSameAs(firstEmailHistory);
            sixthEmailHistory.ShouldNotBeSameAs(firstEmailHistory);
            sixthEmailHistory.ShouldNotBeSameAs(fourthEmailHistory);
        }

        #endregion

        #region General Constructor : Class (EmailHistory) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailHistory_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var oldEmailId = -1;
            var newEmailId = -1;
            var action = string.Empty;
            var oldGroupId = -1;
            var actionTime = new DateTime();

            // Act
            var emailHistory = new EmailHistory();

            // Assert
            emailHistory.OldEmailID.ShouldBe(oldEmailId);
            emailHistory.NewEmailID.ShouldBe(newEmailId);
            emailHistory.Action.ShouldBe(action);
            emailHistory.OldGroupID.ShouldBe(oldGroupId);
            emailHistory.ActionTime.ShouldBe(actionTime);
        }

        #endregion

        #endregion

        #endregion
    }
}