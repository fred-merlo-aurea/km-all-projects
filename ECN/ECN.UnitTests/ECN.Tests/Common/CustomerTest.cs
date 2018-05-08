using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ecn.common.classes;
using ecn.common.classes.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Common
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.common.classes.Customer"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class CustomerTest
    {
        private SqlCommand _command;
        private Customer _customer;
        private PrivateObject _customerPrivateObject;
        private const string SqlCommandText = "select 1";
        private const string SalutationText = "Salutation";
        private const string ContactNameText = "Contact Name";
        private const string FirstNameText = "John";
        private const string LastNameText = "Doe";
        private const string FullNameText = "John Doe";
        private const string ContactTitleText = "Contact Title";
        private const string PhoneText = "Phone";
        private const string FaxText = "Fax";
        private const string EmailText = "email@email.com";
        private const string StreetAddressText = "Street Address";
        private const string CityText = "City";
        private const string StateText = "State";
        private const string CountryText = "Country";
        private const string ZipText = "Zip Code";
        private const string WebAddressText = "web.address.com";
        private const string TechContactText = "Tech Contact";
        private const string CustomerTypeText = "Customer Type";
        private const string YesValue = "Y";
        private const int DefaultIdValue = -1;
        private const int NewIdValue = 0;
        private const int BaseChannelId = 100;
        private const int UserId = 1;
        private const int CustomerId = 100;

        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
            _command = new SqlCommand(SqlCommandText);

            _customer = new Customer
            {
                ID = CustomerId,
                BaseChannelID = BaseChannelId,
                BillingContact = SetCustomerBillingContact(),
                GeneralContact = SetCustomerGeneralContact(),

                Name = FullNameText,
                IsActive = YesValue,
                WebAddress = WebAddressText,
                TechContact = TechContactText,
                TechEmail = EmailText,
                TechPhone = PhoneText,
                SubscriptionsEmail = EmailText,
            };
        }

        [TearDown]
        public void CleanUp() => _context.Dispose();

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void SetDefaultValueForId_ShouldCheckConditionToDefaultValueId_ReturnsNewId(int idValue)
        {
            // Arrange
            var newId = idValue == DefaultIdValue ? NewIdValue : idValue;

            // Act
            var result = _customer.GetValueOrDefault(idValue);

            // Assert
            result.ShouldBe(newId);
        }

        [Test]
        public void MapBillingContactParameters_WhenCommandIsNull_ThrowsException()
        {
            // Arrange
            _command = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _customer.MapBillingContactParameters(UserId, _command));
        }

        [Test]
        public void MapBillingContactParameters_ShouldMapAllParameters_ReturnsCommand()
        {
            // Act
            _customer.MapBillingContactParameters(UserId, _command);

            // Assert
            _command.Parameters.ShouldNotBeNull();
            _command.Parameters.ShouldSatisfyAllConditions(
                () => _command.Parameters["@customerID"].Value.ShouldBe(CustomerId),
                () => _command.Parameters["@salutation"].Value.ShouldBe(SalutationText),
                () => _command.Parameters["@contactName"].Value.ShouldBe(ContactNameText),
                () => _command.Parameters["@firstName"].Value.ShouldBe(FirstNameText),
                () => _command.Parameters["@lastName"].Value.ShouldBe(LastNameText),
                () => _command.Parameters["@contactTitle"].Value.ShouldBe(ContactTitleText),
                () => _command.Parameters["@phone"].Value.ShouldBe(PhoneText),
                () => _command.Parameters["@fax"].Value.ShouldBe(FaxText),
                () => _command.Parameters["@email"].Value.ShouldBe(EmailText),
                () => _command.Parameters["@streetAddress"].Value.ShouldBe(StreetAddressText),
                () => _command.Parameters["@city"].Value.ShouldBe(CityText),
                () => _command.Parameters["@state"].Value.ShouldBe(StateText),
                () => _command.Parameters["@Country"].Value.ShouldBe(CountryText),
                () => _command.Parameters["@zip"].Value.ShouldBe(ZipText),
                () => _command.Parameters["@UserID"].Value.ShouldBe(UserId));
        }

        [Test]
        public void MapCustomerParameters_WhenCommandIsNull_ThrowsException()
        {
            // Arrange
            _command = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _customer.MapCustomerParameters(UserId, _command));
        }

        [Test]
        public void MapCustomerGeneralContact_WhenCommandIsNull_ThrowsException()
        {
            // Arrange
            _command = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _customer.MapCustomerGeneralContact(_command));
        }

        [Test]
        public void MapCustomerGeneralContact_ShouldMapAllParameters_ReturnsCommand()
        {
            // Act
            _customer.MapCustomerGeneralContact(_command);

            // Assert
            _command.Parameters.ShouldNotBeNull();
            _command.Parameters.ShouldSatisfyAllConditions(
                () => _command.Parameters["@baseChannelID"].Value.ShouldBe(BaseChannelId),
                () => _command.Parameters["@customerName"].Value.ShouldBe(FullNameText),
                () => _command.Parameters["@activeFlag"].Value.ShouldBe(YesValue),
                () => _command.Parameters["@address"].Value.ShouldBe(StreetAddressText),
                () => _command.Parameters["@city"].Value.ShouldBe(CityText),
                () => _command.Parameters["@state"].Value.ShouldBe(StateText),
                () => _command.Parameters["@country"].Value.ShouldBe(CountryText),
                () => _command.Parameters["@zip"].Value.ShouldBe(ZipText),
                () => _command.Parameters["@salutation"].Value.ShouldBe(SalutationText),
                () => _command.Parameters["@contactName"].Value.ShouldBe(ContactNameText),
                () => _command.Parameters["@firstName"].Value.ShouldBe(FirstNameText),
                () => _command.Parameters["@lastName"].Value.ShouldBe(LastNameText),
                () => _command.Parameters["@contactTitle"].Value.ShouldBe(ContactTitleText),
                () => _command.Parameters["@email"].Value.ShouldBe(EmailText),
                () => _command.Parameters["@phone"].Value.ShouldBe(PhoneText),
                () => _command.Parameters["@fax"].Value.ShouldBe(FaxText));
        }

        [Test]
        public void MapCustomerLevelInformation_WhenCommandIsNull_ThrowsException()
        {
            // Arrange
            _command = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _customer.MapCustomerLevelInformation(_command));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void MapCustomerLevelInformation_ShouldMapAllParameters_ReturnsCommand(int testValue)
        {
            // Arrange
            _customer.CommunicatorChannelID = testValue;
            _customer.CollectorChannelID = testValue;
            _customer.CreatorChannelID = testValue;
            _customer.PublisherChannelID = testValue;
            _customer.CharityChannelID = testValue;
            _customer.AccountLevel = testValue;
            _customer.CommunicatorLevel = testValue;
            _customer.CollectorLevel = testValue;
            _customer.CreatorLevel = testValue;
            _customer.PublisherLevel = testValue;
            _customer.CharityLevel = testValue;

            SetupShimCustomer(testValue);

            // Act
            _customer.MapCustomerLevelInformation(_command);

            // Assert
            _command.Parameters.ShouldNotBeNull();
            _command.Parameters.ShouldSatisfyAllConditions(
                () => _command.Parameters["@webAddress"].Value.ShouldBe(WebAddressText),
                () => _command.Parameters["@techContact"].Value.ShouldBe(TechContactText),
                () => _command.Parameters["@techEmail"].Value.ShouldBe(EmailText),
                () => _command.Parameters["@techPhone"].Value.ShouldBe(PhoneText),
                () => _command.Parameters["@subscriptionsEmail"].Value.ShouldBe(EmailText),
                () => _command.Parameters["@communicatorChannelID"].Value.ShouldBe(_customer.CommunicatorChannelID == DefaultIdValue ? NewIdValue : _customer.CommunicatorChannelID),
                () => _command.Parameters["@collectorChannelID"].Value.ShouldBe(_customer.CollectorChannelID == DefaultIdValue ? NewIdValue : _customer.CollectorChannelID),
                () => _command.Parameters["@creatorChannelID"].Value.ShouldBe(_customer.CreatorChannelID == DefaultIdValue ? NewIdValue : _customer.CreatorChannelID),
                () => _command.Parameters["@publisherChannelID"].Value.ShouldBe(_customer.PublisherChannelID == DefaultIdValue ? NewIdValue : _customer.PublisherChannelID),
                () => _command.Parameters["@charityChannelID"].Value.ShouldBe(_customer.CharityChannelID == DefaultIdValue ? NewIdValue : _customer.CharityChannelID),
                () => _command.Parameters["@accountsLevel"].Value.ShouldBe(_customer.AccountLevel == DefaultIdValue ? NewIdValue : _customer.AccountLevel),
                () => _command.Parameters["@communicatorLevel"].Value.ShouldBe(_customer.CommunicatorLevel == DefaultIdValue ? NewIdValue : _customer.CommunicatorLevel),
                () => _command.Parameters["@collectorLevel"].Value.ShouldBe(_customer.CollectorLevel == DefaultIdValue ? NewIdValue : _customer.CollectorLevel),
                () => _command.Parameters["@creatorLevel"].Value.ShouldBe(_customer.CreatorLevel == DefaultIdValue ? NewIdValue : _customer.CreatorLevel),
                () => _command.Parameters["@publisherLevel"].Value.ShouldBe(_customer.PublisherLevel == DefaultIdValue ? NewIdValue : _customer.PublisherLevel),
                () => _command.Parameters["@charityLevel"].Value.ShouldBe(_customer.CharityLevel == DefaultIdValue ? NewIdValue : _customer.CharityLevel),
                () => _command.Parameters["@CustomerType"].Value.ShouldBe(CustomerTypeText),
                () => _command.Parameters["@demoFlag"].Value.ShouldBe(YesValue));
        }

        [Test]
        public void MapCustomerAccountsInformation_WhenCommandIsNull_ThrowsException()
        {
            // Arrange
            _command = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() => _customer.MapCustomerAccountsInformation(UserId, _command));
        }

        [Test]
        [TestCase(-1, "")]
        [TestCase(0, "")]
        [TestCase(1, "")]
        [TestCase(-1, "Not Empty")]
        [TestCase(0, "Not Empty")]
        [TestCase(1, "Not Empty")]
        public void MapCustomerAccountsInformation_ShouldMapAllParameters_ReturnsCommand(int testValue, string udfValue)
        {
            // Arrange
            _customer.AccountExecutiveID = testValue;
            _customer.AccountManagerID = testValue;
            _customer.IsStrategic = YesValue;
            _customer.customer_udf1 = udfValue;
            _customer.customer_udf2 = udfValue;
            _customer.customer_udf3 = udfValue;
            _customer.customer_udf4 = udfValue;
            _customer.customer_udf5 = udfValue;

            ShimCustomer.GetCustomerByIDInt32 = 
                id => new Customer
                {
                    AccountExecutiveID = testValue
                };

            // Act
            _customer.MapCustomerAccountsInformation(UserId, _command);

            // Assert
            _command.Parameters.ShouldNotBeNull();
            _command.Parameters.ShouldSatisfyAllConditions(
                () => _command.Parameters["@customerID"].Value.ShouldBe(CustomerId),
                () => _command.Parameters["@IsStrategic"].Value.ShouldBe(_customer.IsStrategic == YesValue ? 1 : 0),
                () => _command.Parameters["@AccountExecutiveID"].Value.ShouldBe(_customer.AccountExecutiveID == DefaultIdValue ? NewIdValue : _customer.AccountExecutiveID),
                () => _command.Parameters["@AccountManagerID"].Value.ShouldBe(_customer.AccountManagerID == DefaultIdValue ? NewIdValue : _customer.AccountManagerID),
                () => _command.Parameters["@customer_udf1"].Value.ShouldBe(_customer.customer_udf1 ?? string.Empty),
                () => _command.Parameters["@customer_udf2"].Value.ShouldBe(_customer.customer_udf2 ?? string.Empty),
                () => _command.Parameters["@customer_udf3"].Value.ShouldBe(_customer.customer_udf3 ?? string.Empty),
                () => _command.Parameters["@customer_udf4"].Value.ShouldBe(_customer.customer_udf4 ?? string.Empty),
                () => _command.Parameters["@customer_udf5"].Value.ShouldBe(_customer.customer_udf5 ?? string.Empty),
                () => _command.Parameters["@UserID"].Value.ShouldBe(UserId));
        }

        private static Contact SetCustomerBillingContact()
        {
            return new Contact
            {
                Salutation = SalutationText,
                ContactName = ContactNameText,
                FirstName = FirstNameText,
                LastName = LastNameText,
                ContactTitle = ContactTitleText,
                Phone = PhoneText,
                Fax = FaxText,
                Email = EmailText,
                StreetAddress = StreetAddressText,
                City = CityText,
                State = StateText,
                Country = CountryText,
                Zip = ZipText
            };
        }

        private static Contact SetCustomerGeneralContact()
        {
            return new Contact
            {
                StreetAddress = StreetAddressText,
                City = CityText,
                State = StateText,
                Country = CountryText,
                Zip = ZipText,
                Salutation = SalutationText,
                ContactName = ContactNameText,
                FirstName = FirstNameText,
                LastName = LastNameText,
                ContactTitle = ContactTitleText,
                Email = EmailText,
                Phone = PhoneText,
                Fax = FaxText
            };
        }

        private static void SetupShimCustomer(int testValue)
        {
            ShimCustomer.GetCustomerByIDInt32 = 
                id => new Customer
                {
                    CommunicatorChannelID = testValue,
                    CollectorChannelID = testValue,
                    CreatorChannelID = testValue,
                    PublisherChannelID = testValue,
                    CharityChannelID = testValue,
                    AccountLevel = testValue,
                    CommunicatorLevel = testValue,
                    CollectorLevel = testValue,
                    CreatorLevel = testValue,
                    PublisherLevel = testValue,
                    CharityLevel = testValue,
                    CustomerType = CustomerTypeText,
                    IsDemo = YesValue
                };
        }
    }
}
