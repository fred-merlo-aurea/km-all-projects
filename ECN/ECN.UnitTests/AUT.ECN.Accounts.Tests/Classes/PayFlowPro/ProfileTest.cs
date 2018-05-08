using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using AUT.ConfigureTestProjects;
using ecn.accounts.classes.PayFlowPro;
using ecn.common.classes.billing;
using NUnit.Framework;
using Shouldly;

namespace AUT.ECN.Accounts.Tests.Classes.PayFlowPro
{
    [TestFixture]
    public class ProfileTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (Profile) => Property (Amount) (Type : double) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Amount_Property_Double_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            profile.Amount = Fixture.Create<double>();
            var doubleType = profile.Amount.GetType();

            // Act
            var isTypeDouble = typeof(double) == (doubleType);
            var isTypeNullableDouble = typeof(double?) == (doubleType);
            var isTypeString = typeof(string) == (doubleType);
            var isTypeInt = typeof(int) == (doubleType);
            var isTypeDecimal = typeof(decimal) == (doubleType);
            var isTypeLong = typeof(long) == (doubleType);
            var isTypeBool = typeof(bool) == (doubleType);
            var isTypeFloat = typeof(float) == (doubleType);
            var isTypeIntNullable = typeof(int?) == (doubleType);
            var isTypeDecimalNullable = typeof(decimal?) == (doubleType);
            var isTypeLongNullable = typeof(long?) == (doubleType);
            var isTypeBoolNullable = typeof(bool?) == (doubleType);
            var isTypeFloatNullable = typeof(float?) == (doubleType);

            // Assert
            isTypeDouble.ShouldBeTrue();
            isTypeString.ShouldBeFalse();
            isTypeNullableDouble.ShouldBeFalse();
            isTypeInt.ShouldBeFalse();
            isTypeDecimal.ShouldBeFalse();
            isTypeLong.ShouldBeFalse();
            isTypeBool.ShouldBeFalse();
            isTypeFloat.ShouldBeFalse();
            isTypeIntNullable.ShouldBeFalse();
            isTypeDecimalNullable.ShouldBeFalse();
            isTypeLongNullable.ShouldBeFalse();
            isTypeBoolNullable.ShouldBeFalse();
            isTypeFloatNullable.ShouldBeFalse();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (Amount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_AmountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAmount = "AmountNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameAmount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Amount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAmount = "Amount";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameAmount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (CreditCard) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_CreditCard_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameCreditCard = "CreditCard";
            var profile = Fixture.Create<Profile>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = profile.GetType().GetProperty(propertyNameCreditCard);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(profile, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (CreditCard) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_CreditCardNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCreditCard = "CreditCardNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameCreditCard));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_CreditCard_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCreditCard = "CreditCard";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameCreditCard);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            profile.CustomerName = Fixture.Create<string>();
            var stringType = profile.CustomerName.GetType();

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

        #region General Getters/Setters : Class (Profile) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (Description) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Description_Property_String_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            profile.Description = Fixture.Create<string>();
            var stringType = profile.Description.GetType();

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

        #region General Getters/Setters : Class (Profile) => Property (Description) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_DescriptionNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDescription = "DescriptionNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameDescription));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Description_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDescription = "Description";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameDescription);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (Frequency) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Frequency_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameFrequency = "Frequency";
            var profile = Fixture.Create<Profile>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = profile.GetType().GetProperty(propertyNameFrequency);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(profile, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (Frequency) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_FrequencyNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameFrequency = "FrequencyNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameFrequency));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Frequency_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameFrequency = "Frequency";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameFrequency);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (ID) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_ID_Property_String_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            profile.ID = Fixture.Create<string>();
            var stringType = profile.ID.GetType();

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

        #region General Getters/Setters : Class (Profile) => Property (ID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_IDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameId = "IDNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameId));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_ID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameId = "ID";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameId);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (IsNew) (Type : bool) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_IsNew_Property_Bool_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var boolType = profile.IsNew.GetType();

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

        #region General Getters/Setters : Class (Profile) => Property (IsNew) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_IsNewNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameIsNew = "IsNewNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameIsNew));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_IsNew_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameIsNew = "IsNew";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameIsNew);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (PayPeriod) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_PayPeriod_Property_String_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            var stringType = profile.PayPeriod.GetType();

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

        #region General Getters/Setters : Class (Profile) => Property (PayPeriod) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_PayPeriodNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNamePayPeriod = "PayPeriodNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNamePayPeriod));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_PayPeriod_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNamePayPeriod = "PayPeriod";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNamePayPeriod);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (Profile) => Property (ProfileName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_ProfileName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var profile = Fixture.Create<Profile>();
            profile.ProfileName = Fixture.Create<string>();
            var stringType = profile.ProfileName.GetType();

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

        #region General Getters/Setters : Class (Profile) => Property (ProfileName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_Class_Invalid_Property_ProfileNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameProfileName = "ProfileNameNotPresent";
            var profile  = Fixture.Create<Profile>();

            // Act , Assert
            Should.NotThrow(() => profile.GetType().GetProperty(propertyNameProfileName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void Profile_ProfileName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameProfileName = "ProfileName";
            var profile  = Fixture.Create<Profile>();
            var propertyInfo  = profile.GetType().GetProperty(propertyNameProfileName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (Profile) with Parameter Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Profile_Instantiated_With_Parameter_No_Exception_Thrown_Test()
        {
            // Arrange
            var profileName = Fixture.Create<string>();
            var customerName = Fixture.Create<string>();
            var creditCard = Fixture.Create<CreditCard>();
            var frequency = Fixture.Create<FrequencyEnum>();
            var amount = Fixture.Create<double>();

            // Act, Assert
            Should.NotThrow(() => new Profile(profileName, customerName, creditCard, frequency, amount));
        }

        #endregion

        #region General Constructor : Class (Profile) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Profile_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfProfile = Fixture.CreateMany<Profile>(2).ToList();
            var firstProfile = instancesOfProfile.FirstOrDefault();
            var lastProfile = instancesOfProfile.Last();

            // Act, Assert
            firstProfile.ShouldNotBeNull();
            lastProfile.ShouldNotBeNull();
            firstProfile.ShouldNotBeSameAs(lastProfile);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_Profile_5_Objects_Creation_5_Paramters_Test()
        {
            // Arrange
            var profileName = Fixture.Create<string>();
            var customerName = Fixture.Create<string>();
            var creditCard = Fixture.Create<CreditCard>();
            var frequency = Fixture.Create<FrequencyEnum>();
            var amount = Fixture.Create<double>();
            var firstProfile = new Profile(profileName, customerName, creditCard, frequency, amount);
            var secondProfile = new Profile(profileName, customerName, creditCard, frequency, amount);
            var thirdProfile = new Profile(profileName, customerName, creditCard, frequency, amount);
            var fourthProfile = new Profile(profileName, customerName, creditCard, frequency, amount);
            var fifthProfile = new Profile(profileName, customerName, creditCard, frequency, amount);
            var sixthProfile = new Profile(profileName, customerName, creditCard, frequency, amount);

            // Act, Assert
            firstProfile.ShouldNotBeNull();
            secondProfile.ShouldNotBeNull();
            thirdProfile.ShouldNotBeNull();
            fourthProfile.ShouldNotBeNull();
            fifthProfile.ShouldNotBeNull();
            sixthProfile.ShouldNotBeNull();
            firstProfile.ShouldNotBeSameAs(secondProfile);
            thirdProfile.ShouldNotBeSameAs(firstProfile);
            fourthProfile.ShouldNotBeSameAs(firstProfile);
            fifthProfile.ShouldNotBeSameAs(firstProfile);
            sixthProfile.ShouldNotBeSameAs(firstProfile);
            sixthProfile.ShouldNotBeSameAs(fourthProfile);
        }

        #endregion

        #endregion

        #endregion
    }
}