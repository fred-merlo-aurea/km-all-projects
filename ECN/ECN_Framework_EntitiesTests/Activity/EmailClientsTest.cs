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
using ECN_Framework_Entities.Activity;

namespace ECN_Framework_Entities.Activity
{
    [TestFixture]
    public class EmailClientsTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailClients) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailClients = Fixture.Create<EmailClients>();
            var emailClientId = Fixture.Create<int>();
            var emailClientName = Fixture.Create<string>();

            // Act
            emailClients.EmailClientID = emailClientId;
            emailClients.EmailClientName = emailClientName;

            // Assert
            emailClients.EmailClientID.ShouldBe(emailClientId);
            emailClients.EmailClientName.ShouldBe(emailClientName);
        }

        #endregion

        #region General Getters/Setters : Class (EmailClients) => Property (EmailClientID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_EmailClientID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailClients = Fixture.Create<EmailClients>();
            emailClients.EmailClientID = Fixture.Create<int>();
            var intType = emailClients.EmailClientID.GetType();

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

        #region General Getters/Setters : Class (EmailClients) => Property (EmailClientID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_Class_Invalid_Property_EmailClientIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailClientID = "EmailClientIDNotPresent";
            var emailClients  = Fixture.Create<EmailClients>();

            // Act , Assert
            Should.NotThrow(() => emailClients.GetType().GetProperty(propertyNameEmailClientID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_EmailClientID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailClientID = "EmailClientID";
            var emailClients  = Fixture.Create<EmailClients>();
            var propertyInfo  = emailClients.GetType().GetProperty(propertyNameEmailClientID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailClients) => Property (EmailClientName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_EmailClientName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailClients = Fixture.Create<EmailClients>();
            emailClients.EmailClientName = Fixture.Create<string>();
            var stringType = emailClients.EmailClientName.GetType();

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

        #region General Getters/Setters : Class (EmailClients) => Property (EmailClientName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_Class_Invalid_Property_EmailClientNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailClientName = "EmailClientNameNotPresent";
            var emailClients  = Fixture.Create<EmailClients>();

            // Act , Assert
            Should.NotThrow(() => emailClients.GetType().GetProperty(propertyNameEmailClientName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailClients_EmailClientName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailClientName = "EmailClientName";
            var emailClients  = Fixture.Create<EmailClients>();
            var propertyInfo  = emailClients.GetType().GetProperty(propertyNameEmailClientName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailClients) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailClients_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailClients());
        }

        #endregion

        #region General Constructor : Class (EmailClients) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailClients_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailClients = Fixture.CreateMany<EmailClients>(2).ToList();
            var firstEmailClients = instancesOfEmailClients.FirstOrDefault();
            var lastEmailClients = instancesOfEmailClients.Last();

            // Act, Assert
            firstEmailClients.ShouldNotBeNull();
            lastEmailClients.ShouldNotBeNull();
            firstEmailClients.ShouldNotBeSameAs(lastEmailClients);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailClients_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailClients = new EmailClients();
            var secondEmailClients = new EmailClients();
            var thirdEmailClients = new EmailClients();
            var fourthEmailClients = new EmailClients();
            var fifthEmailClients = new EmailClients();
            var sixthEmailClients = new EmailClients();

            // Act, Assert
            firstEmailClients.ShouldNotBeNull();
            secondEmailClients.ShouldNotBeNull();
            thirdEmailClients.ShouldNotBeNull();
            fourthEmailClients.ShouldNotBeNull();
            fifthEmailClients.ShouldNotBeNull();
            sixthEmailClients.ShouldNotBeNull();
            firstEmailClients.ShouldNotBeSameAs(secondEmailClients);
            thirdEmailClients.ShouldNotBeSameAs(firstEmailClients);
            fourthEmailClients.ShouldNotBeSameAs(firstEmailClients);
            fifthEmailClients.ShouldNotBeSameAs(firstEmailClients);
            sixthEmailClients.ShouldNotBeSameAs(firstEmailClients);
            sixthEmailClients.ShouldNotBeSameAs(fourthEmailClients);
        }

        #endregion

        #endregion

        #endregion
    }
}