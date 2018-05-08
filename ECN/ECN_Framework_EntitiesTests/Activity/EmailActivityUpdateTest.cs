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
using ECN_Framework_Entities.Activity;

namespace ECN_Framework_Entities.Activity
{
    [TestFixture]
    public class EmailActivityUpdateTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailActivityUpdate) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            var updateId = Fixture.Create<int>();
            var oldEmailId = Fixture.Create<int>();
            var oldEmailAddress = Fixture.Create<string>();
            var newEmailId = Fixture.Create<int>();
            var newEmailAddress = Fixture.Create<string>();
            var updateTime = Fixture.Create<DateTime>();
            var applicationSourceId = Fixture.Create<int>();
            var sourceId = Fixture.Create<int>();
            var comments = Fixture.Create<string>();

            // Act
            emailActivityUpdate.UpdateID = updateId;
            emailActivityUpdate.OldEmailID = oldEmailId;
            emailActivityUpdate.OldEmailAddress = oldEmailAddress;
            emailActivityUpdate.NewEmailID = newEmailId;
            emailActivityUpdate.NewEmailAddress = newEmailAddress;
            emailActivityUpdate.UpdateTime = updateTime;
            emailActivityUpdate.ApplicationSourceID = applicationSourceId;
            emailActivityUpdate.SourceID = sourceId;
            emailActivityUpdate.Comments = comments;

            // Assert
            emailActivityUpdate.UpdateID.ShouldBe(updateId);
            emailActivityUpdate.OldEmailID.ShouldBe(oldEmailId);
            emailActivityUpdate.OldEmailAddress.ShouldBe(oldEmailAddress);
            emailActivityUpdate.NewEmailID.ShouldBe(newEmailId);
            emailActivityUpdate.NewEmailAddress.ShouldBe(newEmailAddress);
            emailActivityUpdate.UpdateTime.ShouldBe(updateTime);
            emailActivityUpdate.ApplicationSourceID.ShouldBe(applicationSourceId);
            emailActivityUpdate.SourceID.ShouldBe(sourceId);
            emailActivityUpdate.Comments.ShouldBe(comments);
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (ApplicationSourceID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_ApplicationSourceID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.ApplicationSourceID = Fixture.Create<int>();
            var intType = emailActivityUpdate.ApplicationSourceID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (ApplicationSourceID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_ApplicationSourceIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameApplicationSourceID = "ApplicationSourceIDNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameApplicationSourceID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_ApplicationSourceID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameApplicationSourceID = "ApplicationSourceID";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameApplicationSourceID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (Comments) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Comments_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.Comments = Fixture.Create<string>();
            var stringType = emailActivityUpdate.Comments.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (Comments) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_CommentsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameComments = "CommentsNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameComments));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Comments_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameComments = "Comments";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameComments);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (NewEmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_NewEmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.NewEmailAddress = Fixture.Create<string>();
            var stringType = emailActivityUpdate.NewEmailAddress.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (NewEmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_NewEmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewEmailAddress = "NewEmailAddressNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameNewEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_NewEmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewEmailAddress = "NewEmailAddress";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameNewEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (NewEmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_NewEmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.NewEmailID = Fixture.Create<int>();
            var intType = emailActivityUpdate.NewEmailID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (NewEmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_NewEmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameNewEmailID = "NewEmailIDNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameNewEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_NewEmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameNewEmailID = "NewEmailID";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameNewEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (OldEmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_OldEmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.OldEmailAddress = Fixture.Create<string>();
            var stringType = emailActivityUpdate.OldEmailAddress.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (OldEmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_OldEmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOldEmailAddress = "OldEmailAddressNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameOldEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_OldEmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOldEmailAddress = "OldEmailAddress";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameOldEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (OldEmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_OldEmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.OldEmailID = Fixture.Create<int>();
            var intType = emailActivityUpdate.OldEmailID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (OldEmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_OldEmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOldEmailID = "OldEmailIDNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameOldEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_OldEmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOldEmailID = "OldEmailID";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameOldEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (SourceID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_SourceID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.SourceID = Fixture.Create<int>();
            var intType = emailActivityUpdate.SourceID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (SourceID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_SourceIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSourceID = "SourceIDNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameSourceID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_SourceID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSourceID = "SourceID";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameSourceID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (UpdateID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_UpdateID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            emailActivityUpdate.UpdateID = Fixture.Create<int>();
            var intType = emailActivityUpdate.UpdateID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (UpdateID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_UpdateIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdateID = "UpdateIDNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameUpdateID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_UpdateID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdateID = "UpdateID";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameUpdateID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (UpdateTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_UpdateTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdateTime = "UpdateTime";
            var emailActivityUpdate = Fixture.Create<EmailActivityUpdate>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailActivityUpdate.GetType().GetProperty(propertyNameUpdateTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailActivityUpdate, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityUpdate) => Property (UpdateTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_Class_Invalid_Property_UpdateTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdateTime = "UpdateTimeNotPresent";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();

            // Act , Assert
            Should.NotThrow(() => emailActivityUpdate.GetType().GetProperty(propertyNameUpdateTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityUpdate_UpdateTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdateTime = "UpdateTime";
            var emailActivityUpdate  = Fixture.Create<EmailActivityUpdate>();
            var propertyInfo  = emailActivityUpdate.GetType().GetProperty(propertyNameUpdateTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailActivityUpdate) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityUpdate_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailActivityUpdate());
        }

        #endregion

        #region General Constructor : Class (EmailActivityUpdate) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityUpdate_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailActivityUpdate = Fixture.CreateMany<EmailActivityUpdate>(2).ToList();
            var firstEmailActivityUpdate = instancesOfEmailActivityUpdate.FirstOrDefault();
            var lastEmailActivityUpdate = instancesOfEmailActivityUpdate.Last();

            // Act, Assert
            firstEmailActivityUpdate.ShouldNotBeNull();
            lastEmailActivityUpdate.ShouldNotBeNull();
            firstEmailActivityUpdate.ShouldNotBeSameAs(lastEmailActivityUpdate);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityUpdate_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailActivityUpdate = new EmailActivityUpdate();
            var secondEmailActivityUpdate = new EmailActivityUpdate();
            var thirdEmailActivityUpdate = new EmailActivityUpdate();
            var fourthEmailActivityUpdate = new EmailActivityUpdate();
            var fifthEmailActivityUpdate = new EmailActivityUpdate();
            var sixthEmailActivityUpdate = new EmailActivityUpdate();

            // Act, Assert
            firstEmailActivityUpdate.ShouldNotBeNull();
            secondEmailActivityUpdate.ShouldNotBeNull();
            thirdEmailActivityUpdate.ShouldNotBeNull();
            fourthEmailActivityUpdate.ShouldNotBeNull();
            fifthEmailActivityUpdate.ShouldNotBeNull();
            sixthEmailActivityUpdate.ShouldNotBeNull();
            firstEmailActivityUpdate.ShouldNotBeSameAs(secondEmailActivityUpdate);
            thirdEmailActivityUpdate.ShouldNotBeSameAs(firstEmailActivityUpdate);
            fourthEmailActivityUpdate.ShouldNotBeSameAs(firstEmailActivityUpdate);
            fifthEmailActivityUpdate.ShouldNotBeSameAs(firstEmailActivityUpdate);
            sixthEmailActivityUpdate.ShouldNotBeSameAs(firstEmailActivityUpdate);
            sixthEmailActivityUpdate.ShouldNotBeSameAs(fourthEmailActivityUpdate);
        }

        #endregion

        #region General Constructor : Class (EmailActivityUpdate) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityUpdate_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var applicationSourceId = 96;

            // Act
            var emailActivityUpdate = new EmailActivityUpdate();

            // Assert
            emailActivityUpdate.ApplicationSourceID.ShouldBe(applicationSourceId);
        }

        #endregion

        #endregion

        #endregion
    }
}