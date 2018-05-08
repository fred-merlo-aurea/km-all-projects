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
    public class EmailGroupTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailGroup) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            var emailGroupId = Fixture.Create<int>();
            var groupId = Fixture.Create<int>();
            var emailId = Fixture.Create<int>();
            var formatTypeCode = Fixture.Create<string>();
            var subscribeTypeCode = Fixture.Create<string>();
            var sMSEnabled = Fixture.Create<bool?>();
            var createdOn = Fixture.Create<DateTime?>();
            var lastChanged = Fixture.Create<DateTime?>();
            var customerId = Fixture.Create<int?>();

            // Act
            emailGroup.EmailGroupID = emailGroupId;
            emailGroup.GroupID = groupId;
            emailGroup.EmailID = emailId;
            emailGroup.FormatTypeCode = formatTypeCode;
            emailGroup.SubscribeTypeCode = subscribeTypeCode;
            emailGroup.SMSEnabled = sMSEnabled;
            emailGroup.CreatedOn = createdOn;
            emailGroup.LastChanged = lastChanged;
            emailGroup.CustomerID = customerId;

            // Assert
            emailGroup.EmailGroupID.ShouldBe(emailGroupId);
            emailGroup.GroupID.ShouldBe(groupId);
            emailGroup.EmailID.ShouldBe(emailId);
            emailGroup.FormatTypeCode.ShouldBe(formatTypeCode);
            emailGroup.SubscribeTypeCode.ShouldBe(subscribeTypeCode);
            emailGroup.SMSEnabled.ShouldBe(sMSEnabled);
            emailGroup.CreatedOn.ShouldBe(createdOn);
            emailGroup.LastChanged.ShouldBe(lastChanged);
            emailGroup.CustomerID.ShouldBe(customerId);
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (CreatedOn) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_CreatedOn_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedOn = "CreatedOn";
            var emailGroup = Fixture.Create<EmailGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailGroup.GetType().GetProperty(propertyNameCreatedOn);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (CreatedOn) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_CreatedOnNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedOn = "CreatedOnNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameCreatedOn));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_CreatedOn_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedOn = "CreatedOn";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameCreatedOn);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            var random = Fixture.Create<int>();

            // Act , Set
            emailGroup.CustomerID = random;

            // Assert
            emailGroup.CustomerID.ShouldBe(random);
            emailGroup.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();

            // Act , Set
            emailGroup.CustomerID = null;

            // Assert
            emailGroup.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var emailGroup = Fixture.Create<EmailGroup>();
            var propertyInfo = emailGroup.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(emailGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailGroup.CustomerID.ShouldBeNull();
            emailGroup.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (EmailGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_EmailGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            emailGroup.EmailGroupID = Fixture.Create<int>();
            var intType = emailGroup.EmailGroupID.GetType();

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

        #region General Getters/Setters : Class (EmailGroup) => Property (EmailGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_EmailGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailGroupID = "EmailGroupIDNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameEmailGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_EmailGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailGroupID = "EmailGroupID";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameEmailGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            emailGroup.EmailID = Fixture.Create<int>();
            var intType = emailGroup.EmailID.GetType();

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

        #region General Getters/Setters : Class (EmailGroup) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (FormatTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_FormatTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            emailGroup.FormatTypeCode = Fixture.Create<string>();
            var stringType = emailGroup.FormatTypeCode.GetType();

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

        #region General Getters/Setters : Class (EmailGroup) => Property (FormatTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_FormatTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFormatTypeCode = "FormatTypeCodeNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameFormatTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_FormatTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFormatTypeCode = "FormatTypeCode";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameFormatTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            emailGroup.GroupID = Fixture.Create<int>();
            var intType = emailGroup.GroupID.GetType();

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

        #region General Getters/Setters : Class (EmailGroup) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (LastChanged) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_LastChanged_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameLastChanged = "LastChanged";
            var emailGroup = Fixture.Create<EmailGroup>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailGroup.GetType().GetProperty(propertyNameLastChanged);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailGroup, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (LastChanged) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_LastChangedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLastChanged = "LastChangedNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameLastChanged));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_LastChanged_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLastChanged = "LastChanged";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameLastChanged);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (SMSEnabled) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_SMSEnabled_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            var random = Fixture.Create<bool>();

            // Act , Set
            emailGroup.SMSEnabled = random;

            // Assert
            emailGroup.SMSEnabled.ShouldBe(random);
            emailGroup.SMSEnabled.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_SMSEnabled_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();

            // Act , Set
            emailGroup.SMSEnabled = null;

            // Assert
            emailGroup.SMSEnabled.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_SMSEnabled_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameSMSEnabled = "SMSEnabled";
            var emailGroup = Fixture.Create<EmailGroup>();
            var propertyInfo = emailGroup.GetType().GetProperty(propertyNameSMSEnabled);

            // Act , Set
            propertyInfo.SetValue(emailGroup, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailGroup.SMSEnabled.ShouldBeNull();
            emailGroup.SMSEnabled.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (SMSEnabled) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_SMSEnabledNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSMSEnabled = "SMSEnabledNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameSMSEnabled));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_SMSEnabled_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSMSEnabled = "SMSEnabled";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameSMSEnabled);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailGroup) => Property (SubscribeTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_SubscribeTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailGroup = Fixture.Create<EmailGroup>();
            emailGroup.SubscribeTypeCode = Fixture.Create<string>();
            var stringType = emailGroup.SubscribeTypeCode.GetType();

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

        #region General Getters/Setters : Class (EmailGroup) => Property (SubscribeTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_Class_Invalid_Property_SubscribeTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscribeTypeCode = "SubscribeTypeCodeNotPresent";
            var emailGroup  = Fixture.Create<EmailGroup>();

            // Act , Assert
            Should.NotThrow(() => emailGroup.GetType().GetProperty(propertyNameSubscribeTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailGroup_SubscribeTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscribeTypeCode = "SubscribeTypeCode";
            var emailGroup  = Fixture.Create<EmailGroup>();
            var propertyInfo  = emailGroup.GetType().GetProperty(propertyNameSubscribeTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailGroup) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailGroup_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailGroup());
        }

        #endregion

        #region General Constructor : Class (EmailGroup) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailGroup_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailGroup = Fixture.CreateMany<EmailGroup>(2).ToList();
            var firstEmailGroup = instancesOfEmailGroup.FirstOrDefault();
            var lastEmailGroup = instancesOfEmailGroup.Last();

            // Act, Assert
            firstEmailGroup.ShouldNotBeNull();
            lastEmailGroup.ShouldNotBeNull();
            firstEmailGroup.ShouldNotBeSameAs(lastEmailGroup);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailGroup_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailGroup = new EmailGroup();
            var secondEmailGroup = new EmailGroup();
            var thirdEmailGroup = new EmailGroup();
            var fourthEmailGroup = new EmailGroup();
            var fifthEmailGroup = new EmailGroup();
            var sixthEmailGroup = new EmailGroup();

            // Act, Assert
            firstEmailGroup.ShouldNotBeNull();
            secondEmailGroup.ShouldNotBeNull();
            thirdEmailGroup.ShouldNotBeNull();
            fourthEmailGroup.ShouldNotBeNull();
            fifthEmailGroup.ShouldNotBeNull();
            sixthEmailGroup.ShouldNotBeNull();
            firstEmailGroup.ShouldNotBeSameAs(secondEmailGroup);
            thirdEmailGroup.ShouldNotBeSameAs(firstEmailGroup);
            fourthEmailGroup.ShouldNotBeSameAs(firstEmailGroup);
            fifthEmailGroup.ShouldNotBeSameAs(firstEmailGroup);
            sixthEmailGroup.ShouldNotBeSameAs(firstEmailGroup);
            sixthEmailGroup.ShouldNotBeSameAs(fourthEmailGroup);
        }

        #endregion

        #region General Constructor : Class (EmailGroup) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailGroup_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailGroupId = -1;
            var groupId = -1;
            var emailId = -1;
            var formatTypeCode = string.Empty;
            var subscribeTypeCode = string.Empty;

            // Act
            var emailGroup = new EmailGroup();

            // Assert
            emailGroup.EmailGroupID.ShouldBe(emailGroupId);
            emailGroup.GroupID.ShouldBe(groupId);
            emailGroup.EmailID.ShouldBe(emailId);
            emailGroup.FormatTypeCode.ShouldBe(formatTypeCode);
            emailGroup.SubscribeTypeCode.ShouldBe(subscribeTypeCode);
            emailGroup.SMSEnabled.ShouldBeNull();
            emailGroup.CustomerID.ShouldBeNull();
            emailGroup.CreatedOn.ShouldBeNull();
            emailGroup.LastChanged.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}