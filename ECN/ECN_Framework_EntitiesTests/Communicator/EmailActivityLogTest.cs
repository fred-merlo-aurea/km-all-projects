using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class EmailActivityLogTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailActivityLog) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            var eAId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var blastId = Fixture.Create<int>();
            var actionTypeCode = Fixture.Create<string>();
            var actionDate = Fixture.Create<DateTime?>();
            var actionValue = Fixture.Create<string>();
            var actionNotes = Fixture.Create<string>();
            var processed = Fixture.Create<string>();
            var customerId = Fixture.Create<int?>();
            var errorList = Fixture.Create<List<ECNError>>();

            // Act
            emailActivityLog.EAID = eAId;
            emailActivityLog.EmailID = emailId;
            emailActivityLog.BlastID = blastId;
            emailActivityLog.ActionTypeCode = actionTypeCode;
            emailActivityLog.ActionDate = actionDate;
            emailActivityLog.ActionValue = actionValue;
            emailActivityLog.ActionNotes = actionNotes;
            emailActivityLog.Processed = processed;
            emailActivityLog.CustomerID = customerId;
            emailActivityLog.ErrorList = errorList;

            // Assert
            emailActivityLog.EAID.ShouldBe(eAId);
            emailActivityLog.EmailID.ShouldBe(emailId);
            emailActivityLog.BlastID.ShouldBe(blastId);
            emailActivityLog.ActionTypeCode.ShouldBe(actionTypeCode);
            emailActivityLog.ActionDate.ShouldBe(actionDate);
            emailActivityLog.ActionValue.ShouldBe(actionValue);
            emailActivityLog.ActionNotes.ShouldBe(actionNotes);
            emailActivityLog.Processed.ShouldBe(processed);
            emailActivityLog.CustomerID.ShouldBe(customerId);
            emailActivityLog.ErrorList.ShouldBe(errorList);
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailActivityLog.GetType().GetProperty(propertyNameActionDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailActivityLog, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_ActionDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDateNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameActionDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameActionDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionNotes) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionNotes_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.ActionNotes = Fixture.Create<string>();
            var stringType = emailActivityLog.ActionNotes.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionNotes) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_ActionNotesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionNotes = "ActionNotesNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameActionNotes));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionNotes_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionNotes = "ActionNotes";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameActionNotes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.ActionTypeCode = Fixture.Create<string>();
            var stringType = emailActivityLog.ActionTypeCode.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_ActionTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCodeNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameActionTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCode";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameActionTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.ActionValue = Fixture.Create<string>();
            var stringType = emailActivityLog.ActionValue.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ActionValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_ActionValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionValue = "ActionValueNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameActionValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ActionValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionValue = "ActionValue";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameActionValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.BlastID = Fixture.Create<int>();
            var intType = emailActivityLog.BlastID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            var random = Fixture.Create<int>();

            // Act , Set
            emailActivityLog.CustomerID = random;

            // Assert
            emailActivityLog.CustomerID.ShouldBe(random);
            emailActivityLog.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();

            // Act , Set
            emailActivityLog.CustomerID = null;

            // Assert
            emailActivityLog.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            var propertyInfo = emailActivityLog.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(emailActivityLog, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailActivityLog.CustomerID.ShouldBeNull();
            emailActivityLog.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (EAID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_EAID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.EAID = Fixture.Create<int>();
            var intType = emailActivityLog.EAID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (EAID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_EAIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAIDNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameEAID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_EAID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEAID = "EAID";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameEAID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.EmailID = Fixture.Create<int>();
            var intType = emailActivityLog.EmailID.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (ErrorList) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_ErrorListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorListNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameErrorList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_ErrorList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorList";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailActivityLog) => Property (Processed) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Processed_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailActivityLog = Fixture.Create<EmailActivityLog>();
            emailActivityLog.Processed = Fixture.Create<string>();
            var stringType = emailActivityLog.Processed.GetType();

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

        #region General Getters/Setters : Class (EmailActivityLog) => Property (Processed) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Class_Invalid_Property_ProcessedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProcessed = "ProcessedNotPresent";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();

            // Act , Assert
            Should.NotThrow(() => emailActivityLog.GetType().GetProperty(propertyNameProcessed));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailActivityLog_Processed_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProcessed = "Processed";
            var emailActivityLog  = Fixture.Create<EmailActivityLog>();
            var propertyInfo  = emailActivityLog.GetType().GetProperty(propertyNameProcessed);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailActivityLog) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityLog_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailActivityLog());
        }

        #endregion

        #region General Constructor : Class (EmailActivityLog) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityLog_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailActivityLog = Fixture.CreateMany<EmailActivityLog>(2).ToList();
            var firstEmailActivityLog = instancesOfEmailActivityLog.FirstOrDefault();
            var lastEmailActivityLog = instancesOfEmailActivityLog.Last();

            // Act, Assert
            firstEmailActivityLog.ShouldNotBeNull();
            lastEmailActivityLog.ShouldNotBeNull();
            firstEmailActivityLog.ShouldNotBeSameAs(lastEmailActivityLog);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityLog_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailActivityLog = new EmailActivityLog();
            var secondEmailActivityLog = new EmailActivityLog();
            var thirdEmailActivityLog = new EmailActivityLog();
            var fourthEmailActivityLog = new EmailActivityLog();
            var fifthEmailActivityLog = new EmailActivityLog();
            var sixthEmailActivityLog = new EmailActivityLog();

            // Act, Assert
            firstEmailActivityLog.ShouldNotBeNull();
            secondEmailActivityLog.ShouldNotBeNull();
            thirdEmailActivityLog.ShouldNotBeNull();
            fourthEmailActivityLog.ShouldNotBeNull();
            fifthEmailActivityLog.ShouldNotBeNull();
            sixthEmailActivityLog.ShouldNotBeNull();
            firstEmailActivityLog.ShouldNotBeSameAs(secondEmailActivityLog);
            thirdEmailActivityLog.ShouldNotBeSameAs(firstEmailActivityLog);
            fourthEmailActivityLog.ShouldNotBeSameAs(firstEmailActivityLog);
            fifthEmailActivityLog.ShouldNotBeSameAs(firstEmailActivityLog);
            sixthEmailActivityLog.ShouldNotBeSameAs(firstEmailActivityLog);
            sixthEmailActivityLog.ShouldNotBeSameAs(fourthEmailActivityLog);
        }

        #endregion

        #region General Constructor : Class (EmailActivityLog) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailActivityLog_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var eAId = 0;
            var emailId = 0;
            var blastId = 0;
            var actionTypeCode = string.Empty;
            var actionValue = string.Empty;
            var actionNotes = string.Empty;
            var processed = string.Empty;
            var errorList = new List<ECNError>();

            // Act
            var emailActivityLog = new EmailActivityLog();

            // Assert
            emailActivityLog.EAID.ShouldBe(eAId);
            emailActivityLog.EmailID.ShouldBe(emailId);
            emailActivityLog.BlastID.ShouldBe(blastId);
            emailActivityLog.ActionTypeCode.ShouldBe(actionTypeCode);
            emailActivityLog.ActionDate.ShouldBeNull();
            emailActivityLog.ActionValue.ShouldBe(actionValue);
            emailActivityLog.ActionNotes.ShouldBe(actionNotes);
            emailActivityLog.Processed.ShouldBe(processed);
            emailActivityLog.CustomerID.ShouldBeNull();
            emailActivityLog.ErrorList.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}