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
using ECN_Framework_Entities.Accounts.View;

namespace ECN_Framework_Entities.Accounts.View
{
    [TestFixture]
    public class CustomerDiskUsageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (CustomerDiskUsage) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            var customerName = Fixture.Create<string>();
            var allowedStorage = Fixture.Create<string>();

            // Act
            customerDiskUsage.CustomerName = customerName;
            customerDiskUsage.AllowedStorage = allowedStorage;

            // Assert
            customerDiskUsage.CustomerName.ShouldBe(customerName);
            customerDiskUsage.AllowedStorage.ShouldBe(allowedStorage);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (AllowedStorage) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_AllowedStorage_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            customerDiskUsage.AllowedStorage = Fixture.Create<string>();
            var stringType = customerDiskUsage.AllowedStorage.GetType();

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

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (AllowedStorage) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_AllowedStorageNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameAllowedStorage = "AllowedStorageNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameAllowedStorage));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_AllowedStorage_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameAllowedStorage = "AllowedStorage";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameAllowedStorage);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            customerDiskUsage.CustomerName = Fixture.Create<string>();
            var stringType = customerDiskUsage.CustomerName.GetType();

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

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (CustomerDiskUsage) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerDiskUsage_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new CustomerDiskUsage());
        }

        #endregion

        #region General Constructor : Class (CustomerDiskUsage) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerDiskUsage_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfCustomerDiskUsage = Fixture.CreateMany<CustomerDiskUsage>(2).ToList();
            var firstCustomerDiskUsage = instancesOfCustomerDiskUsage.FirstOrDefault();
            var lastCustomerDiskUsage = instancesOfCustomerDiskUsage.Last();

            // Act, Assert
            firstCustomerDiskUsage.ShouldNotBeNull();
            lastCustomerDiskUsage.ShouldNotBeNull();
            firstCustomerDiskUsage.ShouldNotBeSameAs(lastCustomerDiskUsage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerDiskUsage_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstCustomerDiskUsage = new CustomerDiskUsage();
            var secondCustomerDiskUsage = new CustomerDiskUsage();
            var thirdCustomerDiskUsage = new CustomerDiskUsage();
            var fourthCustomerDiskUsage = new CustomerDiskUsage();
            var fifthCustomerDiskUsage = new CustomerDiskUsage();
            var sixthCustomerDiskUsage = new CustomerDiskUsage();

            // Act, Assert
            firstCustomerDiskUsage.ShouldNotBeNull();
            secondCustomerDiskUsage.ShouldNotBeNull();
            thirdCustomerDiskUsage.ShouldNotBeNull();
            fourthCustomerDiskUsage.ShouldNotBeNull();
            fifthCustomerDiskUsage.ShouldNotBeNull();
            sixthCustomerDiskUsage.ShouldNotBeNull();
            firstCustomerDiskUsage.ShouldNotBeSameAs(secondCustomerDiskUsage);
            thirdCustomerDiskUsage.ShouldNotBeSameAs(firstCustomerDiskUsage);
            fourthCustomerDiskUsage.ShouldNotBeSameAs(firstCustomerDiskUsage);
            fifthCustomerDiskUsage.ShouldNotBeSameAs(firstCustomerDiskUsage);
            sixthCustomerDiskUsage.ShouldNotBeSameAs(firstCustomerDiskUsage);
            sixthCustomerDiskUsage.ShouldNotBeSameAs(fourthCustomerDiskUsage);
        }

        #endregion

        #region General Constructor : Class (CustomerDiskUsage) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_CustomerDiskUsage_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var customerName = string.Empty;
            var allowedStorage = string.Empty;

            // Act
            var customerDiskUsage = new CustomerDiskUsage();

            // Assert
            customerDiskUsage.CustomerName.ShouldBe(customerName);
            customerDiskUsage.AllowedStorage.ShouldBe(allowedStorage);
        }

        #endregion

        #endregion

        #endregion
    }
}