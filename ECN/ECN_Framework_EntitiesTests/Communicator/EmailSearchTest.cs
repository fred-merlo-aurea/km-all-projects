using System;
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
    public class EmailSearchTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailSearch) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            var totalRowsCount = Fixture.Create<int>();
            var baseChannelName = Fixture.Create<string>();
            var customerName = Fixture.Create<string>();
            var groupName = Fixture.Create<string>();
            var emailAddress = Fixture.Create<string>();
            var subscribe = Fixture.Create<string>();
            var dateCreated = Fixture.Create<DateTime?>();
            var dateModified = Fixture.Create<DateTime?>();

            // Act
            emailSearch.TotalRowsCount = totalRowsCount;
            emailSearch.BaseChannelName = baseChannelName;
            emailSearch.CustomerName = customerName;
            emailSearch.GroupName = groupName;
            emailSearch.EmailAddress = emailAddress;
            emailSearch.Subscribe = subscribe;
            emailSearch.DateCreated = dateCreated;
            emailSearch.DateModified = dateModified;

            // Assert
            emailSearch.TotalRowsCount.ShouldBe(totalRowsCount);
            emailSearch.BaseChannelName.ShouldBe(baseChannelName);
            emailSearch.CustomerName.ShouldBe(customerName);
            emailSearch.GroupName.ShouldBe(groupName);
            emailSearch.EmailAddress.ShouldBe(emailAddress);
            emailSearch.Subscribe.ShouldBe(subscribe);
            emailSearch.DateCreated.ShouldBe(dateCreated);
            emailSearch.DateModified.ShouldBe(dateModified);
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (BaseChannelName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_BaseChannelName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            emailSearch.BaseChannelName = Fixture.Create<string>();
            var stringType = emailSearch.BaseChannelName.GetType();

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

        #region General Getters/Setters : Class (EmailSearch) => Property (BaseChannelName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_BaseChannelNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameBaseChannelName = "BaseChannelNameNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameBaseChannelName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_BaseChannelName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameBaseChannelName = "BaseChannelName";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameBaseChannelName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            emailSearch.CustomerName = Fixture.Create<string>();
            var stringType = emailSearch.CustomerName.GetType();

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

        #region General Getters/Setters : Class (EmailSearch) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (DateCreated) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_DateCreated_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateCreated = "DateCreated";
            var emailSearch = Fixture.Create<EmailSearch>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailSearch.GetType().GetProperty(propertyNameDateCreated);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailSearch, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (DateCreated) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_DateCreatedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateCreated = "DateCreatedNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameDateCreated));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_DateCreated_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateCreated = "DateCreated";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameDateCreated);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (DateModified) Property Type Test Except String

        [Test]
        [ExcludeFromCodeCoverage]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_DateModified_Property_Setting_String_Throw_Argument_Exception_Test()
        {
            // Arrange
            const string propertyNameDateModified = "DateModified";
            var emailSearch = Fixture.Create<EmailSearch>();
            var randomString = Fixture.Create<string>();
            var propertyInfo = emailSearch.GetType().GetProperty(propertyNameDateModified);

            // Act , Assert
            propertyInfo.ShouldNotBeNull();
            Should.Throw<ArgumentException>(() => propertyInfo.SetValue(emailSearch, randomString, null));
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (DateModified) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_DateModifiedNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameDateModified = "DateModifiedNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameDateModified));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_DateModified_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameDateModified = "DateModified";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameDateModified);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (EmailAddress) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_EmailAddress_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            emailSearch.EmailAddress = Fixture.Create<string>();
            var stringType = emailSearch.EmailAddress.GetType();

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

        #region General Getters/Setters : Class (EmailSearch) => Property (EmailAddress) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_EmailAddressNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddressNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameEmailAddress));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_EmailAddress_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameEmailAddress = "EmailAddress";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameEmailAddress);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (GroupName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_GroupName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            emailSearch.GroupName = Fixture.Create<string>();
            var stringType = emailSearch.GroupName.GetType();

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

        #region General Getters/Setters : Class (EmailSearch) => Property (GroupName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_GroupNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupNameNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameGroupName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_GroupName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameGroupName = "GroupName";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameGroupName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (Subscribe) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Subscribe_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            emailSearch.Subscribe = Fixture.Create<string>();
            var stringType = emailSearch.Subscribe.GetType();

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

        #region General Getters/Setters : Class (EmailSearch) => Property (Subscribe) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_SubscribeNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameSubscribe = "SubscribeNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameSubscribe));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Subscribe_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameSubscribe = "Subscribe";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameSubscribe);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailSearch) => Property (TotalRowsCount) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_TotalRowsCount_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailSearch = Fixture.Create<EmailSearch>();
            emailSearch.TotalRowsCount = Fixture.Create<int>();
            var intType = emailSearch.TotalRowsCount.GetType();

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

        #region General Getters/Setters : Class (EmailSearch) => Property (TotalRowsCount) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_Class_Invalid_Property_TotalRowsCountNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameTotalRowsCount = "TotalRowsCountNotPresent";
            var emailSearch  = Fixture.Create<EmailSearch>();

            // Act , Assert
            Should.NotThrow(() => emailSearch.GetType().GetProperty(propertyNameTotalRowsCount));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailSearch_TotalRowsCount_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameTotalRowsCount = "TotalRowsCount";
            var emailSearch  = Fixture.Create<EmailSearch>();
            var propertyInfo  = emailSearch.GetType().GetProperty(propertyNameTotalRowsCount);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailSearch) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailSearch_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailSearch());
        }

        #endregion

        #region General Constructor : Class (EmailSearch) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailSearch_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailSearch = Fixture.CreateMany<EmailSearch>(2).ToList();
            var firstEmailSearch = instancesOfEmailSearch.FirstOrDefault();
            var lastEmailSearch = instancesOfEmailSearch.Last();

            // Act, Assert
            firstEmailSearch.ShouldNotBeNull();
            lastEmailSearch.ShouldNotBeNull();
            firstEmailSearch.ShouldNotBeSameAs(lastEmailSearch);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailSearch_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailSearch = new EmailSearch();
            var secondEmailSearch = new EmailSearch();
            var thirdEmailSearch = new EmailSearch();
            var fourthEmailSearch = new EmailSearch();
            var fifthEmailSearch = new EmailSearch();
            var sixthEmailSearch = new EmailSearch();

            // Act, Assert
            firstEmailSearch.ShouldNotBeNull();
            secondEmailSearch.ShouldNotBeNull();
            thirdEmailSearch.ShouldNotBeNull();
            fourthEmailSearch.ShouldNotBeNull();
            fifthEmailSearch.ShouldNotBeNull();
            sixthEmailSearch.ShouldNotBeNull();
            firstEmailSearch.ShouldNotBeSameAs(secondEmailSearch);
            thirdEmailSearch.ShouldNotBeSameAs(firstEmailSearch);
            fourthEmailSearch.ShouldNotBeSameAs(firstEmailSearch);
            fifthEmailSearch.ShouldNotBeSameAs(firstEmailSearch);
            sixthEmailSearch.ShouldNotBeSameAs(firstEmailSearch);
            sixthEmailSearch.ShouldNotBeSameAs(fourthEmailSearch);
        }

        #endregion

        #region General Constructor : Class (EmailSearch) Default Assignment Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailSearch_Instantiated_With_Default_Assignments_Test()
        {
            // Arrange
            var totalRowsCount = 0;
            var baseChannelName = string.Empty;
            var customerName = string.Empty;
            var groupName = string.Empty;
            var emailAddress = string.Empty;
            var subscribe = string.Empty;

            // Act
            var emailSearch = new EmailSearch();

            // Assert
            emailSearch.TotalRowsCount.ShouldBe(totalRowsCount);
            emailSearch.BaseChannelName.ShouldBe(baseChannelName);
            emailSearch.CustomerName.ShouldBe(customerName);
            emailSearch.GroupName.ShouldBe(groupName);
            emailSearch.EmailAddress.ShouldBe(emailAddress);
            emailSearch.Subscribe.ShouldBe(subscribe);
            emailSearch.DateCreated.ShouldBeNull();
            emailSearch.DateModified.ShouldBeNull();
        }

        #endregion

        #endregion

        #endregion
    }
}