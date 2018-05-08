using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Entities.Accounts;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using static KMPlatform.Enums;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AccessCheckBaseChannelTest
    {
        private IDisposable _shimObject;
        private const int BaseChannelId0 = 0;
        private const int BaseChannelId1 = 1;
        private const int CustomerId = 1;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void CanAccessByBaseChannel_GenericListAndNullUser_ReturnsTrue()
        {
            // Arrange
            var customers = new List<Customer>();

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(customers, null);

            // Assert
            actual.ShouldBeTrue();
        }

        [Test]
        public void CanAccessByBaseChannel_GenericListAndUser_ReturnsTrue()
        {
            // Arrange
            var customers = new List<Customer>();
            var user = new User();

            var customer = new Customer()
            {
                CustomerID = CustomerId,
                BaseChannelID = BaseChannelId1
            };
            customers.Add(customer);

            ShimCustomer.GetByCustomerIDInt32Boolean =
                (_, __) =>
                {
                    return new Customer()
                    {
                        BaseChannelID = BaseChannelId1
                    };
                };

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(customers, user);

            // Assert
            actual.ShouldBeTrue();
        }

        [Test]
        public void CanAccessByBaseChannel_NonGenericObjectWithoutCustomerID_ReturnsFalse()
        {
            // Arrange
            var customers = string.Empty;

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(customers, null);

            // Assert
            actual.ShouldBeFalse();
        }

        [Test]
        public void CanAccessByBaseChannel_NonGenericObjectDifferentBaseChannel_ReturnsFalse()
        {
            // Arrange
            ShimCustomer.GetByCustomerIDInt32Boolean = (_, __) => new Customer()
            {
                BaseChannelID = BaseChannelId0
            };

            var customer = new Customer() { BaseChannelID = BaseChannelId1 };

            var user = new User() { CustomerID = CustomerId };

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(customer, user);

            // Assert
            actual.ShouldBeFalse();
        }

        [Test]
        public void CanAccessByBaseChannel_GenericListAndUserWithServiceEnums_ReturnsTrue()
        {
            // Arrange
            var customers = new List<Customer>();
            var user = new User();
            var customer = new Customer()
            {
                CustomerID = CustomerId,
                BaseChannelID = BaseChannelId1
            };
            customers.Add(customer);

            ShimCustomer.GetByCustomerIDInt32Boolean =
                (_, __) =>
                {
                    return new Customer()
                    {
                        BaseChannelID = BaseChannelId1
                    };
                };

            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(
                customers,
                Services.CREATOR,
                ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                user);

            // Assert
            actual.ShouldBeTrue();
        }

        [Test]
        public void CanAccessByBaseChannel_NonGenericObjectServiceEnumsWithoutCustomerID_ReturnsFalse()
        {
            // Arrange
            var customers = string.Empty;

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(
                customers,
                Services.CREATOR,
                ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                null);

            // Assert
            actual.ShouldBeFalse();
        }

        [Test]
        public void CanAccessByBaseChannel_NonGenericObjectWithEmptyCustomer_ReturnsFalse()
        {
            // Arrange
            var user = new User() { CustomerID = CustomerId };

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(
                new Customer(),
                Services.CREATOR,
                ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                user);

            // Assert
            actual.ShouldBeFalse();
        }

        [Test]
        public void CanAccessByBaseChannel_NonGenericObjectWithNonEmptyCustomerList_ReturnsTrue()
        {
            // Arrange
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (_, __, ___, ____) => true;

            ShimCustomer.GetByCustomerIDInt32Boolean =
                (_, __) =>
                {
                    return new Customer()
                    {
                        BaseChannelID = BaseChannelId1
                    };
                };

            var user = new User()
            {
                IsActive = true,
                CustomerID = CustomerId
            };

            var customer = new Customer()
            {
                CustomerID = CustomerId,
                BaseChannelID = BaseChannelId1
            };

            // Act
            var actual = AccessCheck.CanAccessByBaseChannel(
                customer,
                Services.CREATOR,
                ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                user);

            // Assert
            actual.ShouldBeTrue();
        }
    }
}
