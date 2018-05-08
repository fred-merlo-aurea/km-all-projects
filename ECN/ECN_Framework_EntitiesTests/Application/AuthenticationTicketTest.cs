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
using ECN_Framework_Entities.Application;

namespace ECN_Framework_Entities.Application
{
    [TestFixture]
    public class AuthenticationTicketTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (AuthenticationTicket) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            var userId = Fixture.Create<int>();
            var customerId = Fixture.Create<int>();
            var baseChannelId = Fixture.Create<int>();
            var clientGroupId = Fixture.Create<int>();
            var clientId = Fixture.Create<int>();
            var productId = Fixture.Create<int>();
            var accessKey = Fixture.Create<Guid>();
            var issueDate = Fixture.Create<DateTime>();

            // Act
            authenticationTicket.UserID = userId;
            authenticationTicket.CustomerID = customerId;
            authenticationTicket.BaseChannelID = baseChannelId;
            authenticationTicket.ClientGroupID = clientGroupId;
            authenticationTicket.ClientID = clientId;
            authenticationTicket.ProductID = productId;
            authenticationTicket.AccessKey = accessKey;
            authenticationTicket.IssueDate = issueDate;

            // Assert
            authenticationTicket.UserID.ShouldBe(userId);
            authenticationTicket.CustomerID.ShouldBe(customerId);
            authenticationTicket.BaseChannelID.ShouldBe(baseChannelId);
            authenticationTicket.ClientGroupID.ShouldBe(clientGroupId);
            authenticationTicket.ClientID.ShouldBe(clientId);
            authenticationTicket.ProductID.ShouldBe(productId);
            authenticationTicket.AccessKey.ShouldBe(accessKey);
            authenticationTicket.IssueDate.ShouldBe(issueDate);
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (AccessKey) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_AccessKey_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKey";
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = authenticationTicket.GetType().GetProperty(propertyNameAccessKey);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(authenticationTicket, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (AccessKey) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_AccessKeyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKeyNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameAccessKey));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_AccessKey_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAccessKey = "AccessKey";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameAccessKey);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (BaseChannelID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_BaseChannelID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            authenticationTicket.BaseChannelID = Fixture.Create<int>();
            var intType = authenticationTicket.BaseChannelID.GetType();

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

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (BaseChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_BaseChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelIDNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameBaseChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_BaseChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelID = "BaseChannelID";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameBaseChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (ClientGroupID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_ClientGroupID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            authenticationTicket.ClientGroupID = Fixture.Create<int>();
            var intType = authenticationTicket.ClientGroupID.GetType();

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

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (ClientGroupID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_ClientGroupIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClientGroupID = "ClientGroupIDNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameClientGroupID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_ClientGroupID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClientGroupID = "ClientGroupID";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameClientGroupID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (ClientID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_ClientID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            authenticationTicket.ClientID = Fixture.Create<int>();
            var intType = authenticationTicket.ClientID.GetType();

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

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (ClientID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_ClientIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameClientID = "ClientIDNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameClientID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_ClientID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameClientID = "ClientID";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameClientID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            authenticationTicket.CustomerID = Fixture.Create<int>();
            var intType = authenticationTicket.CustomerID.GetType();

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

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (IssueDate) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_IssueDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDate";
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = authenticationTicket.GetType().GetProperty(propertyNameIssueDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(authenticationTicket, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (IssueDate) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_IssueDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDateNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameIssueDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_IssueDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIssueDate = "IssueDate";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameIssueDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (ProductID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_ProductID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            authenticationTicket.ProductID = Fixture.Create<int>();
            var intType = authenticationTicket.ProductID.GetType();

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

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (ProductID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_ProductIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductIDNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameProductID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_ProductID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProductID = "ProductID";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameProductID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (UserID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_UserID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var authenticationTicket = Fixture.Create<AuthenticationTicket>();
            authenticationTicket.UserID = Fixture.Create<int>();
            var intType = authenticationTicket.UserID.GetType();

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

        #region General Getters/Setters : Class (AuthenticationTicket) => Property (UserID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_Class_Invalid_Property_UserIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserIDNotPresent";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();

            // Act , Assert
            Should.NotThrow(() => authenticationTicket.GetType().GetProperty(propertyNameUserID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void AuthenticationTicket_UserID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUserID = "UserID";
            var authenticationTicket  = Fixture.Create<AuthenticationTicket>();
            var propertyInfo  = authenticationTicket.GetType().GetProperty(propertyNameUserID);

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