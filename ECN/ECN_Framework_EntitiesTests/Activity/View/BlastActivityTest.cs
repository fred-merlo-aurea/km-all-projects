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
using ECN_Framework_Entities.Activity.View;

namespace ECN_Framework_Entities.Activity.View
{
    [TestFixture]
    public class BlastActivityTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (BlastActivity) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            var emailId = Fixture.Create<int?>();
            var emailAddress = Fixture.Create<string>();
            var blastId = Fixture.Create<int>();
            var emailSubject = Fixture.Create<string>();
            var actionTypeCode = Fixture.Create<string>();
            var actionDate = Fixture.Create<DateTime>();
            var actionValue = Fixture.Create<string>();

            // Act
            blastActivity.EmailID = emailId;
            blastActivity.EmailAddress = emailAddress;
            blastActivity.BlastID = blastId;
            blastActivity.EmailSubject = emailSubject;
            blastActivity.ActionTypeCode = actionTypeCode;
            blastActivity.ActionDate = actionDate;
            blastActivity.ActionValue = actionValue;

            // Assert
            blastActivity.EmailID.ShouldBe(emailId);
            blastActivity.EmailAddress.ShouldBe(emailAddress);
            blastActivity.BlastID.ShouldBe(blastId);
            blastActivity.EmailSubject.ShouldBe(emailSubject);
            blastActivity.ActionTypeCode.ShouldBe(actionTypeCode);
            blastActivity.ActionDate.ShouldBe(actionDate);
            blastActivity.ActionValue.ShouldBe(actionValue);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (ActionDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_ActionDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var blastActivity = Fixture.Create<BlastActivity>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = blastActivity.GetType().GetProperty(propertyNameActionDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(blastActivity, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (ActionDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_ActionDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDateNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameActionDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_ActionDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionDate = "ActionDate";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameActionDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (ActionTypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_ActionTypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            blastActivity.ActionTypeCode = Fixture.Create<string>();
            var stringType = blastActivity.ActionTypeCode.GetType();

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

        #region General Getters/Setters : Class (BlastActivity) => Property (ActionTypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_ActionTypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCodeNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameActionTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_ActionTypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionTypeCode = "ActionTypeCode";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameActionTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (ActionValue) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_ActionValue_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            blastActivity.ActionValue = Fixture.Create<string>();
            var stringType = blastActivity.ActionValue.GetType();

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

        #region General Getters/Setters : Class (BlastActivity) => Property (ActionValue) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_ActionValueNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActionValue = "ActionValueNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameActionValue));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_ActionValue_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActionValue = "ActionValue";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameActionValue);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (BlastID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_BlastID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            blastActivity.BlastID = Fixture.Create<int>();
            var intType = blastActivity.BlastID.GetType();

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

        #region General Getters/Setters : Class (BlastActivity) => Property (BlastID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_BlastIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastIDNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameBlastID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_BlastID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBlastID = "BlastID";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameBlastID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            blastActivity.EmailAddress = Fixture.Create<string>();
            var stringType = blastActivity.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (BlastActivity) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (EmailID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            var random = Fixture.Create<int>();

            // Act , Set
            blastActivity.EmailID = random;

            // Assert
            blastActivity.EmailID.ShouldBe(random);
            blastActivity.EmailID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();    

            // Act , Set
            blastActivity.EmailID = null;

            // Assert
            blastActivity.EmailID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameEmailID = "EmailID";
            var blastActivity = Fixture.Create<BlastActivity>();
            var propertyInfo = blastActivity.GetType().GetProperty(propertyNameEmailID);

            // Act , Set
            propertyInfo.SetValue(blastActivity, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            blastActivity.EmailID.ShouldBeNull();
            blastActivity.EmailID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (EmailID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_EmailIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailIDNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameEmailID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailID = "EmailID";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameEmailID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (BlastActivity) => Property (EmailSubject) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailSubject_Property_String_Type_Verify_Test()
        {
            // Arrange
            var blastActivity = Fixture.Create<BlastActivity>();
            blastActivity.EmailSubject = Fixture.Create<string>();
            var stringType = blastActivity.EmailSubject.GetType();

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

        #region General Getters/Setters : Class (BlastActivity) => Property (EmailSubject) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_Class_Invalid_Property_EmailSubjectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubjectNotPresent";
            var blastActivity  = Fixture.Create<BlastActivity>();

            // Act , Assert
            Should.NotThrow(() => blastActivity.GetType().GetProperty(propertyNameEmailSubject));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void BlastActivity_EmailSubject_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailSubject = "EmailSubject";
            var blastActivity  = Fixture.Create<BlastActivity>();
            var propertyInfo  = blastActivity.GetType().GetProperty(propertyNameEmailSubject);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (BlastActivity) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivity_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new BlastActivity());
        }

        #endregion

        #region General Constructor : Class (BlastActivity) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivity_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfBlastActivity = Fixture.CreateMany<BlastActivity>(2).ToList();
            var firstBlastActivity = instancesOfBlastActivity.FirstOrDefault();
            var lastBlastActivity = instancesOfBlastActivity.Last();

            // Act, Assert
            firstBlastActivity.ShouldNotBeNull();
            lastBlastActivity.ShouldNotBeNull();
            firstBlastActivity.ShouldNotBeSameAs(lastBlastActivity);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivity_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstBlastActivity = new BlastActivity();
            var secondBlastActivity = new BlastActivity();
            var thirdBlastActivity = new BlastActivity();
            var fourthBlastActivity = new BlastActivity();
            var fifthBlastActivity = new BlastActivity();
            var sixthBlastActivity = new BlastActivity();

            // Act, Assert
            firstBlastActivity.ShouldNotBeNull();
            secondBlastActivity.ShouldNotBeNull();
            thirdBlastActivity.ShouldNotBeNull();
            fourthBlastActivity.ShouldNotBeNull();
            fifthBlastActivity.ShouldNotBeNull();
            sixthBlastActivity.ShouldNotBeNull();
            firstBlastActivity.ShouldNotBeSameAs(secondBlastActivity);
            thirdBlastActivity.ShouldNotBeSameAs(firstBlastActivity);
            fourthBlastActivity.ShouldNotBeSameAs(firstBlastActivity);
            fifthBlastActivity.ShouldNotBeSameAs(firstBlastActivity);
            sixthBlastActivity.ShouldNotBeSameAs(firstBlastActivity);
            sixthBlastActivity.ShouldNotBeSameAs(fourthBlastActivity);
        }

        #endregion

        #region General Constructor : Class (BlastActivity) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_BlastActivity_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var emailAddress = string.Empty;

            // Act
            var blastActivity = new BlastActivity();

            // Assert
            blastActivity.EmailID.ShouldBeNull();
            blastActivity.EmailAddress.ShouldBe(emailAddress);
        }

        #endregion

        #endregion

        #endregion
    }
}