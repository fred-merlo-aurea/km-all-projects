using System.Linq;
using AutoFixture;
using ECN_Framework_Entities.Communicator.Report;
using ECN_Framework_EntitiesTests.ConfigureProject;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Entities.Tests.Communicator.Report
{
    [TestFixture]
    public class EmailPreviewUsageTest : AbstractGenericTest
    {
        #region Category : General

        #region Category : GetterSetter

        #region General Getters/Setters : Class (EmailPreviewUsage) => All Properties and Fields Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_Class_All_Properties_Getter_Settter_Test()
        {
            // Arrange
            var emailPreviewUsage = Fixture.Create<EmailPreviewUsage>();
            var customerId = Fixture.Create<int>();
            var customerName = Fixture.Create<string>();
            var counts = Fixture.Create<int>();

            // Act
            emailPreviewUsage.CustomerID = customerId;
            emailPreviewUsage.CustomerName = customerName;
            emailPreviewUsage.Counts = counts;

            // Assert
            emailPreviewUsage.CustomerID.ShouldBe(customerId);
            emailPreviewUsage.CustomerName.ShouldBe(customerName);
            emailPreviewUsage.Counts.ShouldBe(counts);
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreviewUsage) => Property (Counts) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_Counts_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreviewUsage = Fixture.Create<EmailPreviewUsage>();
            emailPreviewUsage.Counts = Fixture.Create<int>();
            var intType = emailPreviewUsage.Counts.GetType();

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

        #region General Getters/Setters : Class (EmailPreviewUsage) => Property (Counts) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_Class_Invalid_Property_CountsNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCounts = "CountsNotPresent";
            var emailPreviewUsage  = Fixture.Create<EmailPreviewUsage>();

            // Act , Assert
            Should.NotThrow(() => emailPreviewUsage.GetType().GetProperty(propertyNameCounts));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_Counts_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCounts = "Counts";
            var emailPreviewUsage  = Fixture.Create<EmailPreviewUsage>();
            var propertyInfo  = emailPreviewUsage.GetType().GetProperty(propertyNameCounts);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreviewUsage) => Property (CustomerID) (Type : int) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_CustomerID_Property_Int_Type_Verify_Test()
        {
            // Arrange
            var emailPreviewUsage = Fixture.Create<EmailPreviewUsage>();
            emailPreviewUsage.CustomerID = Fixture.Create<int>();
            var intType = emailPreviewUsage.CustomerID.GetType();

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

        #region General Getters/Setters : Class (EmailPreviewUsage) => Property (CustomerID) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_Class_Invalid_Property_CustomerIDNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerIDNotPresent";
            var emailPreviewUsage  = Fixture.Create<EmailPreviewUsage>();

            // Act , Assert
            Should.NotThrow(() => emailPreviewUsage.GetType().GetProperty(propertyNameCustomerID));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_CustomerID_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerID = "CustomerID";
            var emailPreviewUsage  = Fixture.Create<EmailPreviewUsage>();
            var propertyInfo  = emailPreviewUsage.GetType().GetProperty(propertyNameCustomerID);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #region General Getters/Setters : Class (EmailPreviewUsage) => Property (CustomerName) (Type : string) Property Type test 

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_CustomerName_Property_String_Type_Verify_Test()
        {
            // Arrange
            var emailPreviewUsage = Fixture.Create<EmailPreviewUsage>();
            emailPreviewUsage.CustomerName = Fixture.Create<string>();
            var stringType = emailPreviewUsage.CustomerName.GetType();

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

        #region General Getters/Setters : Class (EmailPreviewUsage) => Property (CustomerName) Exists tests

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_Class_Invalid_Property_CustomerNameNotPresent_Access_Using_Reflexion_Doesnt_Throw_Exception_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerNameNotPresent";
            var emailPreviewUsage  = Fixture.Create<EmailPreviewUsage>();

            // Act , Assert
            Should.NotThrow(() => emailPreviewUsage.GetType().GetProperty(propertyNameCustomerName));
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT GetterSetter")]
        public void EmailPreviewUsage_CustomerName_Property_Is_Present_In_Class_As_Public_Test()
        {
            // Arrange
            const string propertyNameCustomerName = "CustomerName";
            var emailPreviewUsage  = Fixture.Create<EmailPreviewUsage>();
            var propertyInfo  = emailPreviewUsage.GetType().GetProperty(propertyNameCustomerName);

            // Act
            var canRead = propertyInfo.CanRead;

            // Assert
            propertyInfo.ShouldNotBeNull();
            canRead.ShouldBeTrue();
        }

        #endregion

        #endregion

        #region Category : Constructor

        #region General Constructor : Class (EmailPreviewUsage) without Parameter No Exception Thrown Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPreviewUsage_Instantiated_Without_Parameter_Throw_No_Exception()
        {
            // AAA : Arrange or Act or Assert
            Should.NotThrow(() => new EmailPreviewUsage());
        }

        #endregion

        #region General Constructor : Class (EmailPreviewUsage) Multiple Creation Test

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPreviewUsage_2_Objects_Creation_Test()
        {
            // Arrange
            var instancesOfEmailPreviewUsage = Fixture.CreateMany<EmailPreviewUsage>(2).ToList();
            var firstEmailPreviewUsage = instancesOfEmailPreviewUsage.FirstOrDefault();
            var lastEmailPreviewUsage = instancesOfEmailPreviewUsage.Last();

            // Act, Assert
            firstEmailPreviewUsage.ShouldNotBeNull();
            lastEmailPreviewUsage.ShouldNotBeNull();
            firstEmailPreviewUsage.ShouldNotBeSameAs(lastEmailPreviewUsage);
        }

        [Test]
        [Author("AUT. Md. Alim Ul Karim")]
        [Category("AUT Constructor")]
        public void Constructor_EmailPreviewUsage_5_Objects_Creation_No_Paramters_Test()
        {
            // Arrange
            var firstEmailPreviewUsage = new EmailPreviewUsage();
            var secondEmailPreviewUsage = new EmailPreviewUsage();
            var thirdEmailPreviewUsage = new EmailPreviewUsage();
            var fourthEmailPreviewUsage = new EmailPreviewUsage();
            var fifthEmailPreviewUsage = new EmailPreviewUsage();
            var sixthEmailPreviewUsage = new EmailPreviewUsage();

            // Act, Assert
            firstEmailPreviewUsage.ShouldNotBeNull();
            secondEmailPreviewUsage.ShouldNotBeNull();
            thirdEmailPreviewUsage.ShouldNotBeNull();
            fourthEmailPreviewUsage.ShouldNotBeNull();
            fifthEmailPreviewUsage.ShouldNotBeNull();
            sixthEmailPreviewUsage.ShouldNotBeNull();
            firstEmailPreviewUsage.ShouldNotBeSameAs(secondEmailPreviewUsage);
            thirdEmailPreviewUsage.ShouldNotBeSameAs(firstEmailPreviewUsage);
            fourthEmailPreviewUsage.ShouldNotBeSameAs(firstEmailPreviewUsage);
            fifthEmailPreviewUsage.ShouldNotBeSameAs(firstEmailPreviewUsage);
            sixthEmailPreviewUsage.ShouldNotBeSameAs(firstEmailPreviewUsage);
            sixthEmailPreviewUsage.ShouldNotBeSameAs(fourthEmailPreviewUsage);
        }

        #endregion

        #endregion

        #endregion
    }
}