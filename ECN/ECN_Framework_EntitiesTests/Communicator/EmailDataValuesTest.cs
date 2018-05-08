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
    public class EmailDataValuesTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailDataValues) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var emailDataValuesId = Fixture.Create<long>();
            var emailId = Fixture.Create<int>();
            var groupDataFieldsId = Fixture.Create<int>();
            var dataValue = Fixture.Create<string>();
            var surveyGridId = Fixture.Create<int?>();
            var entryId = Fixture.Create<Guid?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedUserId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();

            // Act
            emailDataValues.EmailDataValuesID = emailDataValuesId;
            emailDataValues.EmailID = emailId;
            emailDataValues.GroupDataFieldsID = groupDataFieldsId;
            emailDataValues.DataValue = dataValue;
            emailDataValues.SurveyGridID = surveyGridId;
            emailDataValues.EntryID = entryId;
            emailDataValues.CreatedUserID = createdUserId;
            emailDataValues.UpdatedUserID = updatedUserId;
            emailDataValues.CustomerID = customerId;

            // Assert
            emailDataValues.EmailDataValuesID.ShouldBe(emailDataValuesId);
            emailDataValues.EmailID.ShouldBe(emailId);
            emailDataValues.GroupDataFieldsID.ShouldBe(groupDataFieldsId);
            emailDataValues.DataValue.ShouldBe(dataValue);
            emailDataValues.SurveyGridID.ShouldBe(surveyGridId);
            emailDataValues.EntryID.ShouldBe(entryId);
            emailDataValues.CreatedUserID.ShouldBe(createdUserId);
            emailDataValues.CreatedDate.ShouldBeNull();
            emailDataValues.UpdatedUserID.ShouldBe(updatedUserId);
            emailDataValues.UpdatedDate.ShouldBeNull();
            emailDataValues.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDataValues, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var random = Fixture.Create<int>();

            // Act , Set
            emailDataValues.CreatedUserID = random;

            // Assert
            emailDataValues.CreatedUserID.ShouldBe(random);
            emailDataValues.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();

            // Act , Set
            emailDataValues.CreatedUserID = null;

            // Assert
            emailDataValues.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(emailDataValues, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailDataValues.CreatedUserID.ShouldBeNull();
            emailDataValues.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var random = Fixture.Create<int>();

            // Act , Set
            emailDataValues.CustomerID = random;

            // Assert
            emailDataValues.CustomerID.ShouldBe(random);
            emailDataValues.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();

            // Act , Set
            emailDataValues.CustomerID = null;

            // Assert
            emailDataValues.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(emailDataValues, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailDataValues.CustomerID.ShouldBeNull();
            emailDataValues.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (DataValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_DataValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            emailDataValues.DataValue = Fixture.Create<string>();
            var stringType = emailDataValues.DataValue.GetType();

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

        #region General Getters/Setters : Class (EmailDataValues) => Property (DataValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_DataValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDataValue = "DataValueNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameDataValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_DataValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDataValue = "DataValue";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameDataValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (EmailDataValuesID) (Type : long) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_EmailDataValuesID_Property_Long_Type_Verify_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            emailDataValues.EmailDataValuesID = Fixture.Create<long>();
            var longType = emailDataValues.EmailDataValuesID.GetType();

            // Act
            var isTypeLong = typeof(long) == (longType);
            var isTypeNullableLong = typeof(long?) == (longType);
            var isTypeString = typeof(string) == (longType);
            var isTypeInt = typeof(int) == (longType);
            var isTypeDecimal = typeof(decimal) == (longType);
            var isTypeBool = typeof(bool) == (longType);
            var isTypeDouble = typeof(double) == (longType);
            var isTypeFloat = typeof(float) == (longType);
            var isTypeIntNullable = typeof(int?) == (longType);
            var isTypeDecimalNullable = typeof(decimal?) == (longType);
            var isTypeBoolNullable = typeof(bool?) == (longType);
            var isTypeDoubleNullable = typeof(double?) == (longType);
            var isTypeFloatNullable = typeof(float?) == (longType);

            // Assert
            isTypeLong.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableLong.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeDouble.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeDoubleNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (EmailDataValuesID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_EmailDataValuesIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailDataValuesID = "EmailDataValuesIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameEmailDataValuesID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_EmailDataValuesID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailDataValuesID = "EmailDataValuesID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameEmailDataValuesID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            emailDataValues.EmailID = Fixture.Create<int>();
            var intType = emailDataValues.EmailID.GetType();

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

        #region General Getters/Setters : Class (EmailDataValues) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (EntryID) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_EntryID_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameEntryID = "EntryID";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameEntryID);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDataValues, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (EntryID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_EntryIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEntryID = "EntryIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameEntryID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_EntryID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEntryID = "EntryID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameEntryID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (GroupDataFieldsID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_GroupDataFieldsID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            emailDataValues.GroupDataFieldsID = Fixture.Create<int>();
            var intType = emailDataValues.GroupDataFieldsID.GetType();

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

        #region General Getters/Setters : Class (EmailDataValues) => Property (GroupDataFieldsID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_GroupDataFieldsIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupDataFieldsID = "GroupDataFieldsIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameGroupDataFieldsID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_GroupDataFieldsID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupDataFieldsID = "GroupDataFieldsID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameGroupDataFieldsID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (SurveyGridID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_SurveyGridID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var random = Fixture.Create<int>();

            // Act , Set
            emailDataValues.SurveyGridID = random;

            // Assert
            emailDataValues.SurveyGridID.ShouldBe(random);
            emailDataValues.SurveyGridID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_SurveyGridID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();

            // Act , Set
            emailDataValues.SurveyGridID = null;

            // Assert
            emailDataValues.SurveyGridID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_SurveyGridID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSurveyGridID = "SurveyGridID";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameSurveyGridID);

            // Act , Set
            propertyInfo.SetValue(emailDataValues, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailDataValues.SurveyGridID.ShouldBeNull();
            emailDataValues.SurveyGridID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (SurveyGridID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_SurveyGridIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSurveyGridID = "SurveyGridIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameSurveyGridID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_SurveyGridID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSurveyGridID = "SurveyGridID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameSurveyGridID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDataValues, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var random = Fixture.Create<int>();

            // Act , Set
            emailDataValues.UpdatedUserID = random;

            // Assert
            emailDataValues.UpdatedUserID.ShouldBe(random);
            emailDataValues.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailDataValues = Fixture.Create<EmailDataValues>();

            // Act , Set
            emailDataValues.UpdatedUserID = null;

            // Assert
            emailDataValues.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var emailDataValues = Fixture.Create<EmailDataValues>();
            var propertyInfo = emailDataValues.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(emailDataValues, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailDataValues.UpdatedUserID.ShouldBeNull();
            emailDataValues.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDataValues) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var emailDataValues  = Fixture.Create<EmailDataValues>();

            // Act , Assert
            Should.NotThrow(() => emailDataValues.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDataValues_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var emailDataValues  = Fixture.Create<EmailDataValues>();
            var propertyInfo  = emailDataValues.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailDataValues) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDataValues_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailDataValues());
        }

        #endregion

        #region General Constructor : Class (EmailDataValues) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDataValues_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailDataValues = Fixture.CreateMany<EmailDataValues>(2).ToList();
            var firstEmailDataValues = instancesOfEmailDataValues.FirstOrDefault();
            var lastEmailDataValues = instancesOfEmailDataValues.Last();

            // Act, Assert
            firstEmailDataValues.ShouldNotBeNull();
            lastEmailDataValues.ShouldNotBeNull();
            firstEmailDataValues.ShouldNotBeSameAs(lastEmailDataValues);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDataValues_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailDataValues = new EmailDataValues();
            var secondEmailDataValues = new EmailDataValues();
            var thirdEmailDataValues = new EmailDataValues();
            var fourthEmailDataValues = new EmailDataValues();
            var fifthEmailDataValues = new EmailDataValues();
            var sixthEmailDataValues = new EmailDataValues();

            // Act, Assert
            firstEmailDataValues.ShouldNotBeNull();
            secondEmailDataValues.ShouldNotBeNull();
            thirdEmailDataValues.ShouldNotBeNull();
            fourthEmailDataValues.ShouldNotBeNull();
            fifthEmailDataValues.ShouldNotBeNull();
            sixthEmailDataValues.ShouldNotBeNull();
            firstEmailDataValues.ShouldNotBeSameAs(secondEmailDataValues);
            thirdEmailDataValues.ShouldNotBeSameAs(firstEmailDataValues);
            fourthEmailDataValues.ShouldNotBeSameAs(firstEmailDataValues);
            fifthEmailDataValues.ShouldNotBeSameAs(firstEmailDataValues);
            sixthEmailDataValues.ShouldNotBeSameAs(firstEmailDataValues);
            sixthEmailDataValues.ShouldNotBeSameAs(fourthEmailDataValues);
        }

        #endregion

        #region General Constructor : Class (EmailDataValues) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDataValues_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailDataValuesId = -1;
            var emailId = -1;
            var groupDataFieldsId = -1;
            var dataValue = string.Empty;

            // Act
            var emailDataValues = new EmailDataValues();

            // Assert
            emailDataValues.EmailDataValuesID.ShouldBe(emailDataValuesId);
            emailDataValues.EmailID.ShouldBe(emailId);
            emailDataValues.GroupDataFieldsID.ShouldBe(groupDataFieldsId);
            emailDataValues.DataValue.ShouldBe(dataValue);
            emailDataValues.SurveyGridID.ShouldBeNull();
            emailDataValues.EntryID.ShouldBeNull();
            emailDataValues.CreatedUserID.ShouldBeNull();
            emailDataValues.CreatedDate.ShouldBeNull();
            emailDataValues.UpdatedUserID.ShouldBeNull();
            emailDataValues.UpdatedDate.ShouldBeNull();
            emailDataValues.CustomerID.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}