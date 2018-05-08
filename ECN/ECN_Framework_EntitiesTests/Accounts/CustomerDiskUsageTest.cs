using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Accounts
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
            var usageId = Fixture.Create<int>();
            var channelId = Fixture.Create<int?>();
            var customerId = Fixture.Create<int?>();
            var sizeInBytes = Fixture.Create<string>();
            var dateMonitored = Fixture.Create<DateTime?>();

            // Act
            customerDiskUsage.UsageID = usageId;
            customerDiskUsage.ChannelID = channelId;
            customerDiskUsage.CustomerID = customerId;
            customerDiskUsage.SizeInBytes = sizeInBytes;
            customerDiskUsage.DateMonitored = dateMonitored;

            // Assert
            customerDiskUsage.UsageID.ShouldBe(usageId);
            customerDiskUsage.ChannelID.ShouldBe(channelId);
            customerDiskUsage.CustomerID.ShouldBe(customerId);
            customerDiskUsage.SizeInBytes.ShouldBe(sizeInBytes);
            customerDiskUsage.DateMonitored.ShouldBe(dateMonitored);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (ChannelID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_ChannelID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerDiskUsage.ChannelID = random;

            // Assert
            customerDiskUsage.ChannelID.ShouldBe(random);
            customerDiskUsage.ChannelID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_ChannelID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();    

            // Act , Set
            customerDiskUsage.ChannelID = null;

            // Assert
            customerDiskUsage.ChannelID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_ChannelID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameChannelID = "ChannelID";
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo = customerDiskUsage.GetType().GetProperty(propertyNameChannelID);

            // Act , Set
            propertyInfo.SetValue(customerDiskUsage, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerDiskUsage.ChannelID.ShouldBeNull();
            customerDiskUsage.ChannelID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (ChannelID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_ChannelIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameChannelID = "ChannelIDNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameChannelID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_ChannelID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameChannelID = "ChannelID";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameChannelID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (CustomerID) Nullable Property Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_CustomerID_Property_Data_Without_Null_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            var random = Fixture.Create<int>();

            // Act , Set
            customerDiskUsage.CustomerID = random;

            // Assert
            customerDiskUsage.CustomerID.ShouldBe(random);
            customerDiskUsage.CustomerID.ShouldNotBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_CustomerID_Property_Only_Null_Data_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();    

            // Act , Set
            customerDiskUsage.CustomerID = null;

            // Assert
            customerDiskUsage.CustomerID.ShouldBeNull();
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_CustomerID_Property_Can_Be_Set_To_Null_Using_Reflection_Test()
        {
            // Arrange    
            const string propertyNameCustomerID = "CustomerID";
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo = customerDiskUsage.GetType().GetProperty(propertyNameCustomerID);

            // Act , Set
            propertyInfo.SetValue(customerDiskUsage, null, null);

            // Assert
            propertyInfo.ShouldNotBeNull();
            customerDiskUsage.CustomerID.ShouldBeNull();
            customerDiskUsage.CustomerID.ShouldBe(null);
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (DateMonitored) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_DateMonitored_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateMonitored = "DateMonitored";
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = customerDiskUsage.GetType().GetProperty(propertyNameDateMonitored);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(customerDiskUsage, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (DateMonitored) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_DateMonitoredNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateMonitored = "DateMonitoredNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameDateMonitored));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_DateMonitored_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateMonitored = "DateMonitored";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameDateMonitored);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (SizeInBytes) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_SizeInBytes_Property_String_Type_Verify_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            customerDiskUsage.SizeInBytes = Fixture.Create<string>();
            var stringType = customerDiskUsage.SizeInBytes.GetType();

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

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (SizeInBytes) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_SizeInBytesNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSizeInBytes = "SizeInBytesNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameSizeInBytes));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_SizeInBytes_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSizeInBytes = "SizeInBytes";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameSizeInBytes);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (UsageID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_UsageID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var customerDiskUsage = Fixture.Create<CustomerDiskUsage>();
            customerDiskUsage.UsageID = Fixture.Create<int>();
            var intType = customerDiskUsage.UsageID.GetType();

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

        #region General Getters/Setters : Class (CustomerDiskUsage) => Property (UsageID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_Class_Invalid_Property_UsageIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameUsageID = "UsageIDNotPresent";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();

            // Act , Assert
            Should.NotThrow(() => customerDiskUsage.GetType().GetProperty(propertyNameUsageID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void CustomerDiskUsage_UsageID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameUsageID = "UsageID";
            var customerDiskUsage  = Fixture.Create<CustomerDiskUsage>();
            var propertyInfo  = customerDiskUsage.GetType().GetProperty(propertyNameUsageID);

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
            var usageId = -1;
            var sizeInBytes = string.Empty;

            // Act
            var customerDiskUsage = new CustomerDiskUsage();

            // Assert
            customerDiskUsage.UsageID.ShouldBe(usageId);
            customerDiskUsage.ChannelID.ShouldBeNull();
            customerDiskUsage.CustomerID.ShouldBeNull();
            customerDiskUsage.SizeInBytes.ShouldBe(sizeInBytes);
            customerDiskUsage.DateMonitored.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}