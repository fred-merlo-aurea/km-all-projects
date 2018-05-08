using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CommTriggerPlans = ECN_Framework_BusinessLayer.Communicator.TriggerPlans;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class TriggerPlansTest
    {
        private IDisposable _shimObject;
        private const string CustomerIdInvalid = "CustomerID is invalid";
        private const string UpdatedUserIdInvalid = "UpdatedUserID is invalid";
        private const string CreatedUserIdInvalid = "CreatedUserID is invalid";
        private const string GroupIdInvalid = "GroupID is invalid";
        private const int CustomerId = 1;
        private const int TrigerPlanId = 2;
        private const int GroupId = 3;
        private const int UpdatedUserId = 4;
        private const int CreatedUserId = 5;
        private const int BlastId = 6;
        private const int RefTriggerId = 7;
        private const decimal Period = 1;
        private const string Event = "Event";
        private const string Action = "Action";

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
        public void Validate_InvalidCustomerId_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans();

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(CustomerIdInvalid));
        }

        [Test]
        public void Validate_CustomerIdNotExists_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId
            };

            ShimCustomer.ExistsInt32 = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(7),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(CustomerIdInvalid));
        }

        [Test]
        public void Validate_CreatedUserIdInvalid_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId
            };

            ShimCustomer.ExistsInt32 = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(7),
                () => exception.ErrorList[1].ErrorMessage.ShouldBe(CreatedUserIdInvalid));
        }

        [Test]
        public void Validate_UpdatedUserIdInvalid_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId,
                TriggerPlanID = TrigerPlanId,
                UpdatedUserID = UpdatedUserId
            };

            ShimCustomer.ExistsInt32 = (_) => false;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => false;
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(8),
                () => exception.ErrorList[2].ErrorMessage.ShouldBe(UpdatedUserIdInvalid));
        }

        [Test]
        public void Validate_GroupIdInvalid_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId,
                GroupID = GroupId
            };

            ShimCustomer.ExistsInt32 = (_) => false;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimGroup.ExistsInt32Int32 = (_, __) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(8),
                () => exception.ErrorList[7].ErrorMessage.ShouldBe(GroupIdInvalid));
        }

        [Test]
        public void Validate_ValidData_ReturnsNoError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId,
                GroupID = GroupId,
                CreatedUserID = CreatedUserId,
                EventType = Event,
                BlastID = BlastId,
                refTriggerID = RefTriggerId,
                Period = Period,
                ActionName = Action
            };

            ShimCustomer.ExistsInt32 = (_) => true;
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (_) => true;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimGroup.ExistsInt32Int32 = (_, __) => true;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlast.ExistsInt32Int32 = (_, __) => true;

            // Act, Assert
            Should.NotThrow(() =>
            {
                CommTriggerPlans.Validate(triggerPlans);
            });
        }

        [Test]
        public void Validate_UseAmbientTransaction_InvalidCustomerId_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans();

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate_UseAmbientTransaction(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(CustomerIdInvalid));
        }

        [Test]
        public void Validate_UseAmbientTransaction_CustomerIdNotExists_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId
            };

            ShimCustomer.ExistsInt32 = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate_UseAmbientTransaction(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(7),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(CustomerIdInvalid));
        }

        [Test]
        public void Validate_UseAmbientTransaction_CreatedUserIdInvalid_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId
            };

            ShimCustomer.ExistsInt32 = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate_UseAmbientTransaction(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(7),
                () => exception.ErrorList[1].ErrorMessage.ShouldBe(CreatedUserIdInvalid));
        }

        [Test]
        public void Validate_UseAmbientTransaction_UpdatedUserIdInvalid_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId,
                TriggerPlanID = TrigerPlanId,
                UpdatedUserID = UpdatedUserId
            };

            ShimCustomer.ExistsInt32 = (_) => false;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => false;
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate_UseAmbientTransaction(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(8),
                () => exception.ErrorList[2].ErrorMessage.ShouldBe(UpdatedUserIdInvalid));
        }

        [Test]
        public void Validate_UseAmbientTransaction_GroupIdInvalid_ReturnsError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId,
                GroupID = GroupId
            };

            ShimCustomer.ExistsInt32 = (_) => false;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimGroup.Exists_UseAmbientTransactionInt32Int32 = (_, __) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommTriggerPlans.Validate_UseAmbientTransaction(triggerPlans);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(8),
                () => exception.ErrorList[7].ErrorMessage.ShouldBe(GroupIdInvalid));
        }

        [Test]
        public void Validate_UseAmbientTransaction_ValidData_ReturnsNoError()
        {
            // Arrange
            var triggerPlans = new TriggerPlans
            {
                CustomerID = CustomerId,
                GroupID = GroupId,
                CreatedUserID = CreatedUserId,
                EventType = Event,
                BlastID = BlastId,
                refTriggerID = RefTriggerId,
                Period = Period,
                ActionName = Action
            };

            ShimCustomer.ExistsInt32 = (_) => true;
            KM.Platform.Fakes.ShimUser.IsSystemAdministratorUser = (_) => true;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimGroup.Exists_UseAmbientTransactionInt32Int32 = (_, __) => true;
            ECN_Framework_BusinessLayer.Communicator.Fakes.ShimBlast.Exists_UseAmbientTransactionInt32Int32 = (_, __) => true;

            // Act, Assert
            Should.NotThrow(() =>
            {
                CommTriggerPlans.Validate_UseAmbientTransaction(triggerPlans);
            });
        }
    }
}
