using System;
using System.Collections.Generic;
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
    public class GatewayTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Gateway) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            var gatewayId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var name = Fixture.Create<string>();
            var pubCode = Fixture.Create<string>();
            var typeCode = Fixture.Create<string>();
            var groupId = Fixture.Create<int>();
            var header = Fixture.Create<string>();
            var footer = Fixture.Create<string>();
            var showForgotPassword = Fixture.Create<bool>();
            var forgotPasswordText = Fixture.Create<string>();
            var showSignup = Fixture.Create<bool>();
            var signupText = Fixture.Create<string>();
            var signupURL = Fixture.Create<string>();
            var submitText = Fixture.Create<string>();
            var useStyleFrom = Fixture.Create<string>();
            var style = Fixture.Create<string>();
            var useConfirmation = Fixture.Create<bool>();
            var confirmationMessage = Fixture.Create<string>();
            var confirmationText = Fixture.Create<string>();
            var useRedirect = Fixture.Create<bool>();
            var redirectURL = Fixture.Create<string>();
            var redirectDelay = Fixture.Create<int>();
            var loginOrCapture = Fixture.Create<string>();
            var validateEmail = Fixture.Create<bool>();
            var validatePassword = Fixture.Create<bool>();
            var validateCustom = Fixture.Create<bool>();
            var createdUserId = Fixture.Create<int?>();
            var createdDate = Fixture.Create<DateTime?>();
            var updatedUserId = Fixture.Create<int?>();
            var updatedDate = Fixture.Create<DateTime?>();
            var isDeleted = Fixture.Create<bool>();
            var gatewayValues = Fixture.Create<List<GatewayValue>>();

            // Act
            gateway.GatewayID = gatewayId;
            gateway.CustomerID = customerId;
            gateway.Name = name;
            gateway.PubCode = pubCode;
            gateway.TypeCode = typeCode;
            gateway.GroupID = groupId;
            gateway.Header = header;
            gateway.Footer = footer;
            gateway.ShowForgotPassword = showForgotPassword;
            gateway.ForgotPasswordText = forgotPasswordText;
            gateway.ShowSignup = showSignup;
            gateway.SignupText = signupText;
            gateway.SignupURL = signupURL;
            gateway.SubmitText = submitText;
            gateway.UseStyleFrom = useStyleFrom;
            gateway.Style = style;
            gateway.UseConfirmation = useConfirmation;
            gateway.ConfirmationMessage = confirmationMessage;
            gateway.ConfirmationText = confirmationText;
            gateway.UseRedirect = useRedirect;
            gateway.RedirectURL = redirectURL;
            gateway.RedirectDelay = redirectDelay;
            gateway.LoginOrCapture = loginOrCapture;
            gateway.ValidateEmail = validateEmail;
            gateway.ValidatePassword = validatePassword;
            gateway.ValidateCustom = validateCustom;
            gateway.CreatedUserID = createdUserId;
            gateway.CreatedDate = createdDate;
            gateway.UpdatedUserID = updatedUserId;
            gateway.UpdatedDate = updatedDate;
            gateway.IsDeleted = isDeleted;
            gateway.GatewayValues = gatewayValues;

            // Assert
            gateway.GatewayID.ShouldBe(gatewayId);
            gateway.CustomerID.ShouldBe(customerId);
            gateway.Name.ShouldBe(name);
            gateway.PubCode.ShouldBe(pubCode);
            gateway.TypeCode.ShouldBe(typeCode);
            gateway.GroupID.ShouldBe(groupId);
            gateway.Header.ShouldBe(header);
            gateway.Footer.ShouldBe(footer);
            gateway.ShowForgotPassword.ShouldBe(showForgotPassword);
            gateway.ForgotPasswordText.ShouldBe(forgotPasswordText);
            gateway.ShowSignup.ShouldBe(showSignup);
            gateway.SignupText.ShouldBe(signupText);
            gateway.SignupURL.ShouldBe(signupURL);
            gateway.SubmitText.ShouldBe(submitText);
            gateway.UseStyleFrom.ShouldBe(useStyleFrom);
            gateway.Style.ShouldBe(style);
            gateway.UseConfirmation.ShouldBe(useConfirmation);
            gateway.ConfirmationMessage.ShouldBe(confirmationMessage);
            gateway.ConfirmationText.ShouldBe(confirmationText);
            gateway.UseRedirect.ShouldBe(useRedirect);
            gateway.RedirectURL.ShouldBe(redirectURL);
            gateway.RedirectDelay.ShouldBe(redirectDelay);
            gateway.LoginOrCapture.ShouldBe(loginOrCapture);
            gateway.ValidateEmail.ShouldBe(validateEmail);
            gateway.ValidatePassword.ShouldBe(validatePassword);
            gateway.ValidateCustom.ShouldBe(validateCustom);
            gateway.CreatedUserID.ShouldBe(createdUserId);
            gateway.CreatedDate.ShouldBe(createdDate);
            gateway.UpdatedUserID.ShouldBe(updatedUserId);
            gateway.UpdatedDate.ShouldBe(updatedDate);
            gateway.IsDeleted.ShouldBe(isDeleted);
            gateway.GatewayValues.ShouldBe(gatewayValues);
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ConfirmationMessage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ConfirmationMessage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ConfirmationMessage = Fixture.Create<string>();
            var stringType = gateway.ConfirmationMessage.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ConfirmationMessage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ConfirmationMessageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConfirmationMessage = "ConfirmationMessageNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameConfirmationMessage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ConfirmationMessage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConfirmationMessage = "ConfirmationMessage";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameConfirmationMessage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ConfirmationText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ConfirmationText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ConfirmationText = Fixture.Create<string>();
            var stringType = gateway.ConfirmationText.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ConfirmationText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ConfirmationTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameConfirmationText = "ConfirmationTextNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameConfirmationText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ConfirmationText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameConfirmationText = "ConfirmationText";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameConfirmationText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (CreatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CreatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var gateway = Fixture.Create<Gateway>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = gateway.GetType().GetProperty(propertyNameCreatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(gateway, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (CreatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_CreatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDateNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameCreatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CreatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedDate = "CreatedDate";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameCreatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (CreatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CreatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            var random = Fixture.Create<int>();

            // Act , Set
            gateway.CreatedUserID = random;

            // Assert
            gateway.CreatedUserID.ShouldBe(random);
            gateway.CreatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CreatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();

            // Act , Set
            gateway.CreatedUserID = null;

            // Assert
            gateway.CreatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CreatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCreatedUserID = "CreatedUserID";
            var gateway = Fixture.Create<Gateway>();
            var propertyInfo = gateway.GetType().GetProperty(propertyNameCreatedUserID);

            // Act , Set
            propertyInfo.SetValue(gateway, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            gateway.CreatedUserID.ShouldBeNull();
            gateway.CreatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (CreatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_CreatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserIDNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameCreatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CreatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreatedUserID = "CreatedUserID";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameCreatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.CustomerID = Fixture.Create<int>();
            var intType = gateway.CustomerID.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (Footer) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Footer_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.Footer = Fixture.Create<string>();
            var stringType = gateway.Footer.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (Footer) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_FooterNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFooter = "FooterNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameFooter));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Footer_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFooter = "Footer";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameFooter);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ForgotPasswordText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ForgotPasswordText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ForgotPasswordText = Fixture.Create<string>();
            var stringType = gateway.ForgotPasswordText.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ForgotPasswordText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ForgotPasswordTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameForgotPasswordText = "ForgotPasswordTextNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameForgotPasswordText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ForgotPasswordText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameForgotPasswordText = "ForgotPasswordText";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameForgotPasswordText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (GatewayID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_GatewayID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.GatewayID = Fixture.Create<int>();
            var intType = gateway.GatewayID.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (GatewayID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_GatewayIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGatewayID = "GatewayIDNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameGatewayID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_GatewayID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGatewayID = "GatewayID";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameGatewayID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (GatewayValues) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_GatewayValuesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGatewayValues = "GatewayValuesNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameGatewayValues));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_GatewayValues_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGatewayValues = "GatewayValues";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameGatewayValues);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (GroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_GroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.GroupID = Fixture.Create<int>();
            var intType = gateway.GroupID.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (GroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_GroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupIDNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_GroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupID = "GroupID";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (Header) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Header_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.Header = Fixture.Create<string>();
            var stringType = gateway.Header.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (Header) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_HeaderNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameHeader = "HeaderNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameHeader));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Header_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameHeader = "Header";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameHeader);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (IsDeleted) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_IsDeleted_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.IsDeleted = Fixture.Create<bool>();
            var boolType = gateway.IsDeleted.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (IsDeleted) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_IsDeletedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeletedNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameIsDeleted));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_IsDeleted_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsDeleted = "IsDeleted";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameIsDeleted);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (LoginOrCapture) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_LoginOrCapture_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.LoginOrCapture = Fixture.Create<string>();
            var stringType = gateway.LoginOrCapture.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (LoginOrCapture) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_LoginOrCaptureNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameLoginOrCapture = "LoginOrCaptureNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameLoginOrCapture));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_LoginOrCapture_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameLoginOrCapture = "LoginOrCapture";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameLoginOrCapture);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (Name) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Name_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.Name = Fixture.Create<string>();
            var stringType = gateway.Name.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (Name) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_NameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameName = "NameNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Name_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameName = "Name";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (PubCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_PubCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.PubCode = Fixture.Create<string>();
            var stringType = gateway.PubCode.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (PubCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_PubCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePubCode = "PubCodeNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNamePubCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_PubCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePubCode = "PubCode";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNamePubCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (RedirectDelay) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_RedirectDelay_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.RedirectDelay = Fixture.Create<int>();
            var intType = gateway.RedirectDelay.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (RedirectDelay) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_RedirectDelayNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRedirectDelay = "RedirectDelayNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameRedirectDelay));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_RedirectDelay_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRedirectDelay = "RedirectDelay";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameRedirectDelay);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (RedirectURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_RedirectURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.RedirectURL = Fixture.Create<string>();
            var stringType = gateway.RedirectURL.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (RedirectURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_RedirectURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameRedirectURL = "RedirectURLNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameRedirectURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_RedirectURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameRedirectURL = "RedirectURL";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameRedirectURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ShowForgotPassword) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ShowForgotPassword_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ShowForgotPassword = Fixture.Create<bool>();
            var boolType = gateway.ShowForgotPassword.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ShowForgotPassword) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ShowForgotPasswordNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShowForgotPassword = "ShowForgotPasswordNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameShowForgotPassword));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ShowForgotPassword_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShowForgotPassword = "ShowForgotPassword";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameShowForgotPassword);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ShowSignup) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ShowSignup_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ShowSignup = Fixture.Create<bool>();
            var boolType = gateway.ShowSignup.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ShowSignup) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ShowSignupNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameShowSignup = "ShowSignupNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameShowSignup));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ShowSignup_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameShowSignup = "ShowSignup";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameShowSignup);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (SignupText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_SignupText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.SignupText = Fixture.Create<string>();
            var stringType = gateway.SignupText.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (SignupText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_SignupTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSignupText = "SignupTextNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameSignupText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_SignupText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSignupText = "SignupText";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameSignupText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (SignupURL) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_SignupURL_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.SignupURL = Fixture.Create<string>();
            var stringType = gateway.SignupURL.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (SignupURL) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_SignupURLNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSignupURL = "SignupURLNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameSignupURL));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_SignupURL_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSignupURL = "SignupURL";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameSignupURL);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (Style) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Style_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.Style = Fixture.Create<string>();
            var stringType = gateway.Style.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (Style) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_StyleNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameStyle = "StyleNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameStyle));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Style_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameStyle = "Style";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameStyle);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (SubmitText) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_SubmitText_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.SubmitText = Fixture.Create<string>();
            var stringType = gateway.SubmitText.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (SubmitText) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_SubmitTextNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubmitText = "SubmitTextNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameSubmitText));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_SubmitText_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubmitText = "SubmitText";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameSubmitText);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (TypeCode) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_TypeCode_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.TypeCode = Fixture.Create<string>();
            var stringType = gateway.TypeCode.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (TypeCode) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_TypeCodeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTypeCode = "TypeCodeNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameTypeCode));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_TypeCode_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTypeCode = "TypeCode";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameTypeCode);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UpdatedDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UpdatedDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var gateway = Fixture.Create<Gateway>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = gateway.GetType().GetProperty(propertyNameUpdatedDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(gateway, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UpdatedDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_UpdatedDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDateNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameUpdatedDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UpdatedDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedDate = "UpdatedDate";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameUpdatedDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UpdatedUserID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UpdatedUserID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            var random = Fixture.Create<int>();

            // Act , Set
            gateway.UpdatedUserID = random;

            // Assert
            gateway.UpdatedUserID.ShouldBe(random);
            gateway.UpdatedUserID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UpdatedUserID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();

            // Act , Set
            gateway.UpdatedUserID = null;

            // Assert
            gateway.UpdatedUserID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UpdatedUserID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var gateway = Fixture.Create<Gateway>();
            var propertyInfo = gateway.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act , Set
            propertyInfo.SetValue(gateway, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            gateway.UpdatedUserID.ShouldBeNull();
            gateway.UpdatedUserID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UpdatedUserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_UpdatedUserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserIDNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameUpdatedUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UpdatedUserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUpdatedUserID = "UpdatedUserID";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameUpdatedUserID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UseConfirmation) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UseConfirmation_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.UseConfirmation = Fixture.Create<bool>();
            var boolType = gateway.UseConfirmation.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (UseConfirmation) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_UseConfirmationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUseConfirmation = "UseConfirmationNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameUseConfirmation));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UseConfirmation_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUseConfirmation = "UseConfirmation";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameUseConfirmation);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UseRedirect) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UseRedirect_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.UseRedirect = Fixture.Create<bool>();
            var boolType = gateway.UseRedirect.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (UseRedirect) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_UseRedirectNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUseRedirect = "UseRedirectNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameUseRedirect));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UseRedirect_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUseRedirect = "UseRedirect";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameUseRedirect);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (UseStyleFrom) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UseStyleFrom_Property_String_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.UseStyleFrom = Fixture.Create<string>();
            var stringType = gateway.UseStyleFrom.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (UseStyleFrom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_UseStyleFromNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUseStyleFrom = "UseStyleFromNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameUseStyleFrom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_UseStyleFrom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUseStyleFrom = "UseStyleFrom";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameUseStyleFrom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ValidateCustom) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ValidateCustom_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ValidateCustom = Fixture.Create<bool>();
            var boolType = gateway.ValidateCustom.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ValidateCustom) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ValidateCustomNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValidateCustom = "ValidateCustomNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameValidateCustom));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ValidateCustom_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValidateCustom = "ValidateCustom";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameValidateCustom);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ValidateEmail) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ValidateEmail_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ValidateEmail = Fixture.Create<bool>();
            var boolType = gateway.ValidateEmail.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ValidateEmail) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ValidateEmailNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValidateEmail = "ValidateEmailNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameValidateEmail));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ValidateEmail_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValidateEmail = "ValidateEmail";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameValidateEmail);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Gateway) => Property (ValidatePassword) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ValidatePassword_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var gateway = Fixture.Create<Gateway>();
            gateway.ValidatePassword = Fixture.Create<bool>();
            var boolType = gateway.ValidatePassword.GetType();

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

        #region General Getters/Setters : Class (Gateway) => Property (ValidatePassword) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_Class_Invalid_Property_ValidatePasswordNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameValidatePassword = "ValidatePasswordNotPresent";
            var gateway  = Fixture.Create<Gateway>();

            // Act , Assert
            Should.NotThrow(() => gateway.GetType().GetProperty(propertyNameValidatePassword));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Gateway_ValidatePassword_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameValidatePassword = "ValidatePassword";
            var gateway  = Fixture.Create<Gateway>();
            var propertyInfo  = gateway.GetType().GetProperty(propertyNameValidatePassword);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Gateway) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Gateway_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new Gateway());
        }

        #endregion

        #region General Constructor : Class (Gateway) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Gateway_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfGateway = Fixture.CreateMany<Gateway>(2).ToList();
            var firstGateway = instancesOfGateway.FirstOrDefault();
            var lastGateway = instancesOfGateway.Last();

            // Act, Assert
            firstGateway.ShouldNotBeNull();
            lastGateway.ShouldNotBeNull();
            firstGateway.ShouldNotBeSameAs(lastGateway);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Gateway_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstGateway = new Gateway();
            var secondGateway = new Gateway();
            var thirdGateway = new Gateway();
            var fourthGateway = new Gateway();
            var fifthGateway = new Gateway();
            var sixthGateway = new Gateway();

            // Act, Assert
            firstGateway.ShouldNotBeNull();
            secondGateway.ShouldNotBeNull();
            thirdGateway.ShouldNotBeNull();
            fourthGateway.ShouldNotBeNull();
            fifthGateway.ShouldNotBeNull();
            sixthGateway.ShouldNotBeNull();
            firstGateway.ShouldNotBeSameAs(secondGateway);
            thirdGateway.ShouldNotBeSameAs(firstGateway);
            fourthGateway.ShouldNotBeSameAs(firstGateway);
            fifthGateway.ShouldNotBeSameAs(firstGateway);
            sixthGateway.ShouldNotBeSameAs(firstGateway);
            sixthGateway.ShouldNotBeSameAs(fourthGateway);
        }

        #endregion

        #region General Constructor : Class (Gateway) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Gateway_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var gatewayId = -1;
            var customerId = -1;
            var name = string.Empty;
            var pubCode = string.Empty;
            var typeCode = string.Empty;
            var groupId = -1;
            var header = string.Empty;
            var footer = string.Empty;
            var showForgotPassword = false;
            var forgotPasswordText = string.Empty;
            var showSignup = false;
            var signupText = string.Empty;
            var signupURL = string.Empty;
            var submitText = string.Empty;
            var useStyleFrom = string.Empty;
            var style = string.Empty;
            var useConfirmation = false;
            var confirmationMessage = string.Empty;
            var confirmationText = string.Empty;
            var useRedirect = false;
            var redirectURL = string.Empty;
            var redirectDelay = 0;
            var loginOrCapture = string.Empty;
            var validateEmail = false;
            var validatePassword = false;
            var validateCustom = false;
            var isDeleted = false;
            var gatewayValues = new List<GatewayValue>();

            // Act
            var gateway = new Gateway();

            // Assert
            gateway.GatewayID.ShouldBe(gatewayId);
            gateway.CustomerID.ShouldBe(customerId);
            gateway.Name.ShouldBe(name);
            gateway.PubCode.ShouldBe(pubCode);
            gateway.TypeCode.ShouldBe(typeCode);
            gateway.GroupID.ShouldBe(groupId);
            gateway.Header.ShouldBe(header);
            gateway.Footer.ShouldBe(footer);
            gateway.ShowForgotPassword.ShouldBeFalse();
            gateway.ForgotPasswordText.ShouldBe(forgotPasswordText);
            gateway.ShowSignup.ShouldBeFalse();
            gateway.SignupText.ShouldBe(signupText);
            gateway.SignupURL.ShouldBe(signupURL);
            gateway.SubmitText.ShouldBe(submitText);
            gateway.UseStyleFrom.ShouldBe(useStyleFrom);
            gateway.Style.ShouldBe(style);
            gateway.UseConfirmation.ShouldBeFalse();
            gateway.ConfirmationMessage.ShouldBe(confirmationMessage);
            gateway.ConfirmationText.ShouldBe(confirmationText);
            gateway.UseRedirect.ShouldBeFalse();
            gateway.RedirectURL.ShouldBe(redirectURL);
            gateway.RedirectDelay.ShouldBe(redirectDelay);
            gateway.LoginOrCapture.ShouldBe(loginOrCapture);
            gateway.ValidateEmail.ShouldBeFalse();
            gateway.ValidatePassword.ShouldBeFalse();
            gateway.ValidateCustom.ShouldBeFalse();
            gateway.CreatedUserID.ShouldBeNull();
            gateway.CreatedDate.ShouldBeNull();
            gateway.UpdatedUserID.ShouldBeNull();
            gateway.UpdatedDate.ShouldBeNull();
            gateway.IsDeleted.ShouldBeFalse();
            gateway.GatewayValues.ShouldBeEmpty();
        }

        #endregion

        #endregion

        #endregion
    }
}