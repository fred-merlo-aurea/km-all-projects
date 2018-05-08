using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using Shouldly;
using AutoFixture;
using ECN_Framework_Common.Objects;
using NUnit.Framework;
using Moq;
using ECN_Framework_EntitiesTests.ConfigureProject;
using ECN_Framework_Entities.Accounts;

namespace ECN_Framework_Entities.Accounts
{
    [TestFixture]
    public class CustomerPlanTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : Constructor

        #region All getter/setter test

        #region General Getter/Setter Pattern : All getter setter at once.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerPlan  = new CustomerPlan();
            var planID = Fixture.Create<int>();
            var customerID = Fixture.Create<int>();
            var quoteOptionID = Fixture.Create<int>();
            var activationDate = Fixture.Create<DateTime>();
            var cardOwnerName = Fixture.Create<string>();
            var cardNumber = Fixture.Create<string>();
            var cardType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.CardType>();
            var cardExpiration = Fixture.Create<string>();
            var cardVerificationNumber = Fixture.Create<string>();
            var subscriptionType = Fixture.Create<ECN_Framework_Common.Objects.Accounts.Enums.SubscriptionType>();
            var isPhoneSupportIncluded = Fixture.Create<bool>();
            var errorList = new List<ECNError>();

            // Act
            customerPlan.PlanID = planID;
            customerPlan.CustomerID = customerID;
            customerPlan.QuoteOptionID = quoteOptionID;
            customerPlan.ActivationDate = activationDate;
            customerPlan.CardOwnerName = cardOwnerName;
            customerPlan.CardExpiration = cardExpiration;
            customerPlan.CardVerificationNumber = cardVerificationNumber;
            customerPlan.SubscriptionType = subscriptionType;
            customerPlan.IsPhoneSupportIncluded = isPhoneSupportIncluded;
            customerPlan.ErrorList = errorList;

            // Assert
            customerPlan.PlanID.ShouldBe(planID);
            customerPlan.CustomerID.ShouldBe(customerID);
            customerPlan.QuoteOptionID.ShouldBe(quoteOptionID);
            customerPlan.ActivationDate.ShouldBe(activationDate);
            customerPlan.CardOwnerName.ShouldBe(cardOwnerName);
            customerPlan.CardExpiration.ShouldBe(cardExpiration);
            customerPlan.CardVerificationNumber.ShouldBe(cardVerificationNumber);
            customerPlan.SubscriptionType.ShouldBe(subscriptionType);
            customerPlan.IsPhoneSupportIncluded.ShouldBe(isPhoneSupportIncluded);
            customerPlan.ErrorList.ShouldBe(errorList);   
            customerPlan.CardType.ShouldNotBeNull();
        }

        #endregion

        #endregion

        #region Getter/Setter Test

        #region Non-String Property Type Test : CustomerPlan => ActivationDate

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_ActivationDate_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameActivationDate = "ActivationDate";
            var customerPlan = new CustomerPlan();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerPlan.GetType().GetProperty(propertyNameActivationDate);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerPlan, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_ActivationDateNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameActivationDate = "ActivationDateNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameActivationDate));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_ActivationDate_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameActivationDate = "ActivationDate";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameActivationDate);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerPlan => CardExpiration

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardExpiration_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerPlan = new CustomerPlan();;
            customerPlan.CardExpiration = Fixture.Create<string>();
            var stringType = customerPlan.CardExpiration.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_CardExpirationNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCardExpiration = "CardExpirationNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameCardExpiration));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardExpiration_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCardExpiration = "CardExpiration";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameCardExpiration);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_CardNumberNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCardNumber = "CardNumberNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameCardNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardNumber_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCardNumber = "CardNumber";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameCardNumber);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region string property type test : CustomerPlan => CardOwnerName

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardOwnerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerPlan = new CustomerPlan();
            customerPlan.CardOwnerName = Fixture.Create<string>();
            var stringType = customerPlan.CardOwnerName.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_CardOwnerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCardOwnerName = "CardOwnerNameNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameCardOwnerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardOwnerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCardOwnerName = "CardOwnerName";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameCardOwnerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerPlan => CardType

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCardType = "CardType";
            var customerPlan = new CustomerPlan();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerPlan.GetType().GetProperty(propertyNameCardType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerPlan, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_CardTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCardType = "CardTypeNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameCardType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCardType = "CardType";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameCardType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_CardVerificationNumberNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCardVerificationNumber = "CardVerificationNumberNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameCardVerificationNumber));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CardVerificationNumber_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCardVerificationNumber = "CardVerificationNumber";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameCardVerificationNumber);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerPlan => CustomerID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerPlan = new CustomerPlan();;
            var intType = customerPlan.CustomerID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_ErrorListNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorListNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameErrorList));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_ErrorList_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameErrorList = "ErrorList";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameErrorList);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region bool property type test : CustomerPlan => IsPhoneSupportIncluded

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_IsPhoneSupportIncluded_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var customerPlan = new CustomerPlan();;
            var boolType = customerPlan.IsPhoneSupportIncluded.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_IsPhoneSupportIncludedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsPhoneSupportIncluded = "IsPhoneSupportIncludedNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameIsPhoneSupportIncluded));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_IsPhoneSupportIncluded_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsPhoneSupportIncluded = "IsPhoneSupportIncluded";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameIsPhoneSupportIncluded);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerPlan => PlanID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_PlanID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerPlan = new CustomerPlan();;
            var intType = customerPlan.PlanID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_PlanIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePlanID = "PlanIDNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNamePlanID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_PlanID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePlanID = "PlanID";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNamePlanID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region int property type test : CustomerPlan => QuoteOptionID

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_QuoteOptionID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerPlan = new CustomerPlan();;
            var intType = customerPlan.QuoteOptionID.GetType();

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

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_QuoteOptionIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameQuoteOptionID = "QuoteOptionIDNotPresent";
            var customerPlan  = new CustomerPlan();;

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameQuoteOptionID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_QuoteOptionID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameQuoteOptionID = "QuoteOptionID";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameQuoteOptionID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region Non-String Property Type Test : CustomerPlan => SubscriptionType

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_SubscriptionType_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscriptionType = "SubscriptionType";
            var customerPlan = new CustomerPlan();;
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerPlan.GetType().GetProperty(propertyNameSubscriptionType);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerPlan, randomString, null));
        }

        #endregion

        #region General Getter/Setter Pattern : test getter/setter properties.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_Class_Invalid_Property_SubscriptionTypeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscriptionType = "SubscriptionTypeNotPresent";
            var customerPlan  = new CustomerPlan();

            // Act , Assert
            Should.NotThrow(() => customerPlan.GetType().GetProperty(propertyNameSubscriptionType));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerPlan_SubscriptionType_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscriptionType = "SubscriptionType";
            var customerPlan  = new CustomerPlan();;
            var propertyInfo  = customerPlan.GetType().GetProperty(propertyNameSubscriptionType);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion


        #endregion
        #region Category : Constructor

        #region General Constructor Pattern : create and expect no exception.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerPlan_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerPlan());
        }

        #endregion

        #region General Constructor Pattern : Multiple object creation test.

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Multiple_Object_Creation_Test()
        {
            // Arrange
            var first = new CustomerPlan();
            var last = new CustomerPlan();

            // Act, Assert
            first.ShouldNotBeNull();
            last.ShouldNotBeNull();
            first.ShouldNotBe(last);
        }

        #endregion


        #endregion


        #endregion
    }
}