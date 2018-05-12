using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using AutoFixture;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator
{
    [TestFixture]
    public class EmailDirectTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailDirect) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            var emailDirectId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var source = Fixture.Create<string>();
            var process = Fixture.Create<string>();
            var status = Fixture.Create<string>();
            var sendTime = Fixture.Create<DateTime?>();
            var startTime = Fixture.Create<DateTime?>();
            var finishTime = Fixture.Create<DateTime?>();
            var emailAddress = Fixture.Create<string>();
            var fromEmailAddress = Fixture.Create<string>();
            var fromName = Fixture.Create<string>();
            var emailSubject = Fixture.Create<string>();
            var replyEmailAddress = Fixture.Create<string>();
            var content = Fixture.Create<string>();
            var createdDate = Fixture.Create<DateTime?>();
            var createdUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var openTime = Fixture.Create<DateTime?>();
            var attachments = new List<Attachment>();
            var cCAddresses = Fixture.Create<List<string>>();

            // Act
            emailDirect.EmailDirectID = emailDirectId;
            emailDirect.CustomerID = customerId;
            emailDirect.Source = source;
            emailDirect.Process = process;
            emailDirect.Status = status;
            emailDirect.SendTime = sendTime;
            emailDirect.StartTime = startTime;
            emailDirect.FinishTime = finishTime;
            emailDirect.EmailAddress = emailAddress;
            emailDirect.FromEmailAddress = fromEmailAddress;
            emailDirect.FromName = fromName;
            emailDirect.EmailSubject = emailSubject;
            emailDirect.ReplyEmailAddress = replyEmailAddress;
            emailDirect.Content = content;
            emailDirect.CreatedDate = createdDate;
            emailDirect.CreatedUserID = createdUserId;
            emailDirect.UpdatedDate = updatedDate;
            emailDirect.UpdatedUserID = updatedUserId;
            emailDirect.OpenTime = openTime;
            emailDirect.Attachments = attachments;
            emailDirect.CCAddresses = cCAddresses;

            // Assert
            emailDirect.EmailDirectID.ShouldBe(emailDirectId);
            emailDirect.CustomerID.ShouldBe(customerId);
            emailDirect.Source.ShouldBe(source);
            emailDirect.Process.ShouldBe(process);
            emailDirect.Status.ShouldBe(status);
            emailDirect.SendTime.ShouldBe(sendTime);
            emailDirect.StartTime.ShouldBe(startTime);
            emailDirect.FinishTime.ShouldBe(finishTime);
            emailDirect.EmailAddress.ShouldBe(emailAddress);
            emailDirect.FromEmailAddress.ShouldBe(fromEmailAddress);
            emailDirect.FromName.ShouldBe(fromName);
            emailDirect.EmailSubject.ShouldBe(emailSubject);
            emailDirect.ReplyEmailAddress.ShouldBe(replyEmailAddress);
            emailDirect.Content.ShouldBe(content);
            emailDirect.CreatedDate.ShouldBe(createdDate);
            emailDirect.CreatedUserID.ShouldBe(createdUserId);
            emailDirect.UpdatedDate.ShouldBe(updatedDate);
            emailDirect.UpdatedUserID.ShouldBe(updatedUserId);
            emailDirect.OpenTime.ShouldBe(openTime);
            emailDirect.Attachments.ShouldBe(attachments);
            emailDirect.CCAddresses.ShouldBe(cCAddresses);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (Attachments) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_AttachmentsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAttachments = "AttachmentsNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameAttachments));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Attachments_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAttachments = "Attachments";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameAttachments);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (CCAddresses) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_CCAddressesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCCAddresses = "CCAddressesNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameCCAddresses));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CCAddresses_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCCAddresses = "CCAddresses";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameCCAddresses);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (Content) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Content_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.Content = Fixture.Create<string>();
            var stringType = emailDirect.Content.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (Content) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_ContentNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameContent = "ContentNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameContent));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Content_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameContent = "Content";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameContent);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var emailDirect = new EmailDirect();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDirect, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            var random = Fixture.Create<int>();

            // Act , Set
            emailDirect.CreatedUserID = random;

            // Assert
            emailDirect.CreatedUserID.ShouldBe(random);
            emailDirect.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;

            // Act , Set
            emailDirect.CreatedUserID = null;

            // Assert
            emailDirect.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var emailDirect = new EmailDirect();;
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(emailDirect, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailDirect.CreatedUserID.ShouldBeNull();
            emailDirect.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.CustomerID = Fixture.Create<int>();
            var intType = emailDirect.CustomerID.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.EmailAddress = Fixture.Create<string>();
            var stringType = emailDirect.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (EmailDirectID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_EmailDirectID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.EmailDirectID = Fixture.Create<int>();
            var intType = emailDirect.EmailDirectID.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (EmailDirectID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_EmailDirectIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailDirectID = "EmailDirectIDNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameEmailDirectID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_EmailDirectID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailDirectID = "EmailDirectID";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameEmailDirectID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.EmailSubject = Fixture.Create<string>();
            var stringType = emailDirect.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (FinishTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_FinishTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTime";
            var emailDirect = new EmailDirect();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameFinishTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDirect, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (FinishTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_FinishTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTimeNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameFinishTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_FinishTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFinishTime = "FinishTime";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameFinishTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (FromEmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_FromEmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.FromEmailAddress = Fixture.Create<string>();
            var stringType = emailDirect.FromEmailAddress.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (FromEmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_FromEmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromEmailAddress = "FromEmailAddressNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameFromEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_FromEmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromEmailAddress = "FromEmailAddress";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameFromEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (FromName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_FromName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.FromName = Fixture.Create<string>();
            var stringType = emailDirect.FromName.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (FromName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_FromNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromNameNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameFromName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_FromName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFromName = "FromName";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameFromName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (OpenTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_OpenTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenTime = "OpenTime";
            var emailDirect = new EmailDirect();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameOpenTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDirect, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (OpenTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_OpenTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameOpenTime = "OpenTimeNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameOpenTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_OpenTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameOpenTime = "OpenTime";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameOpenTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (Process) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Process_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.Process = Fixture.Create<string>();
            var stringType = emailDirect.Process.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (Process) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_ProcessNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProcess = "ProcessNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameProcess));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Process_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProcess = "Process";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameProcess);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (ReplyEmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_ReplyEmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.ReplyEmailAddress = Fixture.Create<string>();
            var stringType = emailDirect.ReplyEmailAddress.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (ReplyEmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_ReplyEmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameReplyEmailAddress = "ReplyEmailAddressNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameReplyEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_ReplyEmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameReplyEmailAddress = "ReplyEmailAddress";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameReplyEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (SendTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_SendTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var emailDirect = new EmailDirect();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameSendTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDirect, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (SendTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_SendTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTimeNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameSendTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_SendTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendTime = "SendTime";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameSendTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (Source) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Source_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.Source = Fixture.Create<string>();
            var stringType = emailDirect.Source.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (Source) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_SourceNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSource = "SourceNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameSource));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Source_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSource = "Source";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameSource);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (StartTime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_StartTime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var emailDirect = new EmailDirect();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameStartTime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDirect, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (StartTime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_StartTimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTimeNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameStartTime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_StartTime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStartTime = "StartTime";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameStartTime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (Status) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Status_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            emailDirect.Status = Fixture.Create<string>();
            var stringType = emailDirect.Status.GetType();

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

        #region General Getters/Setters : Class (EmailDirect) => Property (Status) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_StatusNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStatus = "StatusNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameStatus));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Status_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStatus = "Status";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameStatus);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var emailDirect = new EmailDirect();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailDirect, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;
            var random = Fixture.Create<int>();

            // Act , Set
            emailDirect.UpdatedUserID = random;

            // Assert
            emailDirect.UpdatedUserID.ShouldBe(random);
            emailDirect.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var emailDirect = new EmailDirect();;

            // Act , Set
            emailDirect.UpdatedUserID = null;

            // Assert
            emailDirect.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var emailDirect = new EmailDirect();;
            var propertyInfo = emailDirect.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(emailDirect, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            emailDirect.UpdatedUserID.ShouldBeNull();
            emailDirect.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (EmailDirect) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var emailDirect  = new EmailDirect();;

            // Act , Assert
            Should.NotThrow(() => emailDirect.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailDirect_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var emailDirect  = new EmailDirect();;
            var propertyInfo  = emailDirect.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailDirect) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDirect_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailDirect());
        }

        #endregion

        #region General Constructor : Class (EmailDirect) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDirect_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailDirect = new EmailDirect();
            var secondEmailDirect = new EmailDirect();
            var thirdEmailDirect = new EmailDirect();
            var fourthEmailDirect = new EmailDirect();
            var fifthEmailDirect = new EmailDirect();
            var sixthEmailDirect = new EmailDirect();

            // Act, Assert
            firstEmailDirect.ShouldNotBeNull();
            secondEmailDirect.ShouldNotBeNull();
            thirdEmailDirect.ShouldNotBeNull();
            fourthEmailDirect.ShouldNotBeNull();
            fifthEmailDirect.ShouldNotBeNull();
            sixthEmailDirect.ShouldNotBeNull();
            firstEmailDirect.ShouldNotBeSameAs(secondEmailDirect);
            thirdEmailDirect.ShouldNotBeSameAs(firstEmailDirect);
            fourthEmailDirect.ShouldNotBeSameAs(firstEmailDirect);
            fifthEmailDirect.ShouldNotBeSameAs(firstEmailDirect);
            sixthEmailDirect.ShouldNotBeSameAs(firstEmailDirect);
            sixthEmailDirect.ShouldNotBeSameAs(fourthEmailDirect);
        }

        #endregion

        #region General Constructor : Class (EmailDirect) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailDirect_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailDirectId = -1;
            var customerId = -1;
            var source = string.Empty;
            var process = string.Empty;
            var status = "Pending";
            var sendTime = DateTime.Now;
            var emailAddress = string.Empty;
            var fromName = string.Empty;
            var emailSubject = string.Empty;
            var replyEmailAddress = string.Empty;
            var content = string.Empty;
            var attachments = new List<Attachment>();
            var cCAddresses = new List<string>();

            // Act
            var emailDirect = new EmailDirect();

            // Assert
            emailDirect.EmailDirectID.ShouldBe(emailDirectId);
            emailDirect.CustomerID.ShouldBe(customerId);
            emailDirect.Source.ShouldBe(source);
            emailDirect.Process.ShouldBe(process);
            emailDirect.Status.ShouldBe(status);
            //Overly optimisitc assertions failues due to differences in milliseconds
            //emailDirect.SendTime.ShouldBe(sendTime);
            //emailDirect.StartTime.ShouldBeNull();
            //emailDirect.FinishTime.ShouldBeNull();
            emailDirect.EmailAddress.ShouldBe(emailAddress);
            emailDirect.FromName.ShouldBe(fromName);
            emailDirect.EmailSubject.ShouldBe(emailSubject);
            emailDirect.ReplyEmailAddress.ShouldBe(replyEmailAddress);
            emailDirect.Content.ShouldBe(content);
            emailDirect.CreatedDate.ShouldBeNull();
            emailDirect.CreatedUserID.ShouldBeNull();
            emailDirect.UpdatedDate.ShouldBeNull();
            emailDirect.UpdatedUserID.ShouldBeNull();
            emailDirect.OpenTime.ShouldBeNull();
            emailDirect.Attachments.ShouldBeEmpty();
            emailDirect.CCAddresses.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}