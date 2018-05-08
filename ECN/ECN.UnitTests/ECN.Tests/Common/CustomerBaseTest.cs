using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data.SqlClient;
using ecn.common.classes;
using ecn.common.classes.Fakes;
using ECN.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Common
{
    [TestFixture]
    public class CustomerBaseTest
    {
        private const string NameSample = "John Doe";
        private const string EmailSample = "john@doe.com";
        private const string BillingContactSample = "m@m.com";
        private const int LevelSample = 3;
        private const int BaseChannelId = 39;
        private const string PickupPathSample = @"c:\dir1";
        private const string MailingIPSample = "10.0.0.1";
        private const string DbSample = "maindb";
        public static readonly int ExistingId = 17;

        private IDisposable _context;
        private CustomerBase _customer;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp() => _context.Dispose();

        [Test]
        public void BillingContact_Null_ReturnsRetrieved()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var billingContact = new Contact { Email = BillingContactSample };
            ShimCustomerBase.AllInstances.GetBillingContactInt32 = (_, __) => billingContact;

            // Act
            var result = _customer.BillingContact;

            // Assert
            result.ShouldBeSameAs(billingContact);
        }

        [Test]
        public void TechContact_Null_ReturnsRetrieved()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { TechContact = EmailSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.TechContact;

            // Assert
            result.ShouldBeSameAs(EmailSample);
        }

        [Test]
        public void TechEmail_Null_ReturnsRetrieved()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { TechEmail = EmailSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.TechEmail;

            // Assert
            result.ShouldBeSameAs(EmailSample);
        }

        [Test]
        public void TechPhone_Null_ReturnsRetrieved()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { TechPhone = EmailSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.TechPhone;

            // Assert
            result.ShouldBeSameAs(EmailSample);
        }

        [Test]
        public void SubscriptionsEmail_Null_ReturnsDefault()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { SubscriptionsEmail = EmailSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.SubscriptionsEmail;

            // Assert
            result.ShouldBeSameAs(CustomerBase.SubscriptionDefaultEmail);
        }

        [Test]
        public void SubscriptionsEmail_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { SubscriptionsEmail = EmailSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            _customer.SubscriptionsEmail = EmailSample;
            var result = _customer.SubscriptionsEmail;

            // Assert
            result.ShouldBeSameAs(EmailSample);
        }

        [Test]
        public void Name_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { Name = NameSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.Name;

            // Assert
            result.ShouldBeSameAs(NameSample);
        }

        [Test]
        public void CollectorLevel_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { CollectorLevel = LevelSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.CollectorLevel;

            // Assert
            result.ShouldBe(LevelSample);
        }

        [Test]
        public void CreatorLevel_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { CreatorLevel = LevelSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.CreatorLevel;

            // Assert
            result.ShouldBe(LevelSample);
        }

        [Test]
        public void AccountLevel_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { AccountLevel = LevelSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.AccountLevel;

            // Assert
            result.ShouldBe(LevelSample);
        }

        [Test]
        public void BaseChannelID_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, CustomerBase.IdNone);
            var loadedCustomer = new CustomerBase { BaseChannelID = BaseChannelId };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.BaseChannelID;

            // Assert
            result.ShouldBe(BaseChannelId);
        }

        [Test]
        public void GeneralContact_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var contact = new Contact { Email = BillingContactSample };
            var loadedCustomer = new CustomerBase { GeneralContact = contact };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.GeneralContact;

            // Assert
            result.ShouldBeSameAs(contact);
        }

        [Test]
        public void WebAddress_Null_ReturnsSet()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { WebAddress = EmailSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            // Act
            var result = _customer.WebAddress;

            // Assert
            result.ShouldBe(EmailSample);
        }

        [Test]
        public void AssertCustomerIsSaved_NotSaved_Exception()
        {
            // Arrange
            _customer = new CustomerBase(CustomerBase.IdNone, BaseChannelId);
            var privateObj = new PrivateObject(_customer);

            // Act, Assert
            Should.Throw<Exception>(
                () => privateObj.Invoke("AssertCustomerIsSaved", "Assert Message"));
        }

        [Test]
        public void AssertCustomerIsSaved_Saved_NoException()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var privateObj = new PrivateObject(_customer);

            // Act, Assert
            privateObj.Invoke("AssertCustomerIsSaved", "Assert Message");
        }

        [Test]
        public void CreatePickupConfig_Saved_MethodsCalled()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);

            var pickupPathCalled = string.Empty;
            ShimCustomerConfig.CreatePickupConfigInt32String = (_, pickupPath) =>
                {
                    pickupPathCalled = pickupPath;
                };

            var mailingIpCalled = string.Empty;
            ShimCustomerConfig.CreateComConfigInt32StringString = (_, __, mailingIp) =>
            {
                mailingIpCalled = mailingIp;
            };

            // Act
            _customer.CreatePickupConfig(PickupPathSample, MailingIPSample);

            // Assert
            mailingIpCalled.ShouldSatisfyAllConditions(
                () => mailingIpCalled.ShouldBe(MailingIPSample),
                () => pickupPathCalled.ShouldBe(PickupPathSample));
            
        }

        [Test]
        public void CreateMasterSupressionGroup_Saved_MethodsCalled()
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);

            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var settings = new NameValueCollection {{CustomerBase.AppSettingcommunicatordb, DbSample}};
                return settings;
            };

            var queryCalled = string.Empty;
            ShimDataFunctions.ExecuteString = query =>
            {
                queryCalled = query;
                return 0;
            };

            // Act
            _customer.CreateMasterSupressionGroup();

            // Assert
            queryCalled.ShouldSatisfyAllConditions(
                () => queryCalled.ShouldContain(DbSample),
                () => queryCalled.ShouldContain(ExistingId.ToString()));

        }

        [Test]
        public void CustomerNameExists_Saved_MethodsCalled([Values(false, true)] bool shouldExists)
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var loadedCustomer = new CustomerBase { Name = NameSample };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            var queryCalled = string.Empty;
            ShimDataFunctions.ExecuteScalarString = query =>
            {
                queryCalled = query;
                return shouldExists ? 1 : 0;
            };

            // Act
            var exists =  _customer.CustomerNameExists();

            // Assert
            queryCalled.ShouldSatisfyAllConditions(
                () => exists.ShouldBe(shouldExists),
                () => queryCalled.ShouldContain(BaseChannelId.ToString()),
                () => queryCalled.ShouldContain(NameSample));

        }

        [Test]
        public void Validate_Saved_MethodsCalled([Values(null, NameSample)] string name)
        {
            // Arrange
            _customer = new CustomerBase(ExistingId, BaseChannelId);
            var contact = new Contact { Email = BillingContactSample };
            var loadedCustomer = new CustomerBase { Name = name, GeneralContact = contact };
            ShimCustomerBase.AllInstances.LoadedCustomerBaseGet = _ => loadedCustomer;

            var billingContact = new Contact { Email = BillingContactSample };
            ShimCustomerBase.AllInstances.GetBillingContactInt32 = (_, __) => billingContact;

            // Act
            var validationResult = _customer.Validate();

            // Assert
            if (string.IsNullOrWhiteSpace(name))
            {
                validationResult.ShouldNotBeEmpty();
            }
            else
            {
                validationResult.ShouldBeEmpty();
            }
        }
    }
}
