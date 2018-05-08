using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator;
using ECN.Framework.BusinessLayer.Interfaces;
using Moq;
using NUnit.Framework;
using KMPlatform.Entity;
using ECN_Framework_Entities.Accounts;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class AccessCheckTest
    {
        private const int CustomerIDValue = 1;
        private const int BaseChannelIDValue = 1;
        private Mock<IUser> _user;
        private Mock<ICustomer> _customer;

        [SetUp]
        public void SetUp()
        {
            _user = new Mock<IUser>();
            _user.Setup(x => x.IsSystemAdministrator(It.IsAny<User>())).Returns(false);
            _user.Setup(x => x.IsChannelAdministrator(It.IsAny<User>())).Returns(true);
            _customer = new Mock<ICustomer>();
            _customer.Setup(x => x.GetByCustomerID(It.IsAny<int>(), false))
                .Returns(new Customer()
                {
                    BaseChannelID = BaseChannelIDValue
                });

            AccessCheck.Initialize(_user.Object, _customer.Object);
        }

        [Test]
        public void CanAccessByCustomer_GenericListAndNullUser_ReturnsTrue()
        {
            // Arrange
            var customers = new List<Customer>();

            // Act
            var actual = AccessCheck.CanAccessByCustomer(customers, null);

            // Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void CanAccessByCustomer_NonGenericObjectWithoutCustomerID_ReturnsFalse()
        {
            // Arrange
            var customers = string.Empty;

            // Act
            var actual = AccessCheck.CanAccessByCustomer(customers, null);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void CanAccessByCustomer_NonGenericObjectWithChannelAdminCustomerIDAndEmptyCustomerList_ReturnsFalse()
        {
            // Arrange
            _customer.Setup(x => x.GetByBaseChannelID(BaseChannelIDValue))
                .Returns(new List<Customer>());

            // Act
            var actual = AccessCheck.CanAccessByCustomer(
                new Customer(),
                new User()
                {
                    CustomerID = CustomerIDValue
                });

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void CanAccessByCustomer_NonGenericObjectWithChannelAdminCustomerIDAndNonEmptyCustomerList_ReturnsFalse()
        {
            // Arrange
            var customers = new List<Customer>();
            customers.Add(new Customer()
            {
                CustomerID = CustomerIDValue
            });

            _customer.Setup(x => x.GetByBaseChannelID(BaseChannelIDValue)).Returns(customers);

            // Act
            var actual = AccessCheck.CanAccessByCustomer(
                new Customer()
                {
                    CustomerID = CustomerIDValue
                },
                new User()
                {
                    CustomerID = CustomerIDValue
                });

            // Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void CanAccessByCustomer_GenericListAndNullUserWithServiceEnums_ReturnsTrue()
        {
            // Arrange
            var customers = new List<Customer>();

            // Act
            var actual = AccessCheck.CanAccessByCustomer(
                customers, 
                KMPlatform.Enums.Services.CREATOR, 
                KMPlatform.Enums.ServiceFeatures.Blast, 
                KMPlatform.Enums.Access.Edit, 
                null);

            // Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void CanAccessByCustomer_NonGenericObjectServiceEnumsWithoutCustomerID_ReturnsFalse()
        {
            // Arrange
            var customers = string.Empty;

            // Act
            var actual = AccessCheck.CanAccessByCustomer(
                customers,
                KMPlatform.Enums.Services.CREATOR,
                KMPlatform.Enums.ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                null);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void CanAccessByCustomer_NonGenericObjectWithServiceEnumsAndChannelAdminCustomerIDAndEmptyCustomerList_ReturnsFalse()
        {
            // Arrange
            _user.Setup(x => x.HasAccess(
                    It.IsAny<User>(), 
                    KMPlatform.Enums.Services.CREATOR, 
                    KMPlatform.Enums.ServiceFeatures.Blast, 
                    KMPlatform.Enums.Access.Edit))
                .Returns(true);

            _customer.Setup(x => x.GetByBaseChannelID(BaseChannelIDValue))
                .Returns(new List<Customer>());

            // Act
            var actual = AccessCheck.CanAccessByCustomer(
                new Customer(),
                KMPlatform.Enums.Services.CREATOR,
                KMPlatform.Enums.ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                new User()
                {
                    CustomerID = CustomerIDValue
                });

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void CanAccessByCustomer_NonGenericObjectWithServiceEnumsAndChannelAdminCustomerIDAndNonEmptyCustomerList_ReturnsFalse()
        {
            // Arrange
            _user.Setup(x => x.HasAccess(
                It.IsAny<User>(),
                KMPlatform.Enums.Services.CREATOR,
                KMPlatform.Enums.ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit))
                .Returns(true);

            var customers = new List<Customer>();
            customers.Add(new Customer()
            {
                CustomerID = CustomerIDValue
            });

            _customer.Setup(x => x.GetByBaseChannelID(BaseChannelIDValue)).Returns(customers);

            // Act
            var actual = AccessCheck.CanAccessByCustomer(
                new Customer()
                {
                    CustomerID = CustomerIDValue
                },
                KMPlatform.Enums.Services.CREATOR,
                KMPlatform.Enums.ServiceFeatures.Blast,
                KMPlatform.Enums.Access.Edit,
                new User()
                {
                    CustomerID = CustomerIDValue
                });

            // Assert
            Assert.That(actual, Is.True);
        }
    }
}