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
using ECN_Framework_Entities.Activity.Report.Undeliverable;

namespace ECN_Framework_Entities.Activity.Report.Undeliverable
{
    [TestFixture]
    public class UndeliverableTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Undeliverable) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            var sendtime = Fixture.Create<DateTime>();
            var emailId = Fixture.Create<int>();
            var emailAddress = Fixture.Create<string>();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var layoutName = Fixture.Create<string>();
            var message = Fixture.Create<string>();

            // Act
            undeliverable.Sendtime = sendtime;
            undeliverable.EmailID = emailId;
            undeliverable.EmailAddress = emailAddress;
            undeliverable.BlastID = blastId;
            undeliverable.EmailSubject = emailSubject;
            undeliverable.LayoutName = layoutName;
            undeliverable.Message = message;

            // Assert
            undeliverable.Sendtime.ShouldBe(sendtime);
            undeliverable.EmailID.ShouldBe(emailId);
            undeliverable.EmailAddress.ShouldBe(emailAddress);
            undeliverable.BlastID.ShouldBe(blastId);
            undeliverable.EmailSubject.ShouldBe(emailSubject);
            undeliverable.LayoutName.ShouldBe(layoutName);
            undeliverable.Message.ShouldBe(message);
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            undeliverable.BlastID = Fixture.Create<int>();
            var intType = undeliverable.BlastID.GetType();

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

        #region General Getters/Setters : Class (Undeliverable) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            undeliverable.EmailAddress = Fixture.Create<string>();
            var stringType = undeliverable.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (Undeliverable) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (EmailID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_EmailID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            undeliverable.EmailID = Fixture.Create<int>();
            var intType = undeliverable.EmailID.GetType();

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

        #region General Getters/Setters : Class (Undeliverable) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            undeliverable.EmailSubject = Fixture.Create<string>();
            var stringType = undeliverable.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (Undeliverable) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (LayoutName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_LayoutName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            undeliverable.LayoutName = Fixture.Create<string>();
            var stringType = undeliverable.LayoutName.GetType();

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

        #region General Getters/Setters : Class (Undeliverable) => Property (LayoutName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_LayoutNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutNameNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameLayoutName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_LayoutName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLayoutName = "LayoutName";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameLayoutName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (Message) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Message_Property_String_Type_Verify_Test()
        {
            // Arrange
            var undeliverable = Fixture.Create<Undeliverable>();
            undeliverable.Message = Fixture.Create<string>();
            var stringType = undeliverable.Message.GetType();

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

        #region General Getters/Setters : Class (Undeliverable) => Property (Message) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_MessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameMessage = "MessageNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Message_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameMessage = "Message";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (Sendtime) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Sendtime_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSendtime = "Sendtime";
            var undeliverable = Fixture.Create<Undeliverable>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = undeliverable.GetType().GetProperty(propertyNameSendtime);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(undeliverable, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Undeliverable) => Property (Sendtime) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Class_Invalid_Property_SendtimeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSendtime = "SendtimeNotPresent";
            var undeliverable  = Fixture.Create<Undeliverable>();

            // Act , Assert
            Should.NotThrow(() => undeliverable.GetType().GetProperty(propertyNameSendtime));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Undeliverable_Sendtime_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSendtime = "Sendtime";
            var undeliverable  = Fixture.Create<Undeliverable>();
            var propertyInfo  = undeliverable.GetType().GetProperty(propertyNameSendtime);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #endregion
    }
}