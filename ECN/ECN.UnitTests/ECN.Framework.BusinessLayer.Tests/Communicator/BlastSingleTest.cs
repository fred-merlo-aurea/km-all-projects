using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using CommBlastSingle = ECN_Framework_BusinessLayer.Communicator.BlastSingle;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public class BlastSingleTest
    {
        private IDisposable _shimObject;
        private User _user;
        private const string CustomerIdInvalid = "CustomerID is invalid";
        private const string BlastIdInvalid = "BlastID is invalid";
        private const string EmailIdNotExists = "EmailID no longer exists";
        private const string TriggerPlanIdInvalid = "TriggerPlanID is invalid";
        private const string BlastTypeNoOpen = "noopen";
        private const int BlastId = 1;
        private const int CustomerId = 2;
        private const int EmailId = 3;
        private const int LayoutPlanId = 4;
        private const int RefBlastId = 5;
        private const int CreatedUserId = 6;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _user = new User();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Validate_CustomerIdNull_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = null,
                BlastID = BlastId
            };

            ShimBlast.GetByBlastIDInt32UserBoolean = (_, __, ___) => new BlastAB();

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate(blastSingle, _user);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(CustomerIdInvalid));
        }

        [Test]
        public void Validate_BlastIdInvalid_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId
            };

            ShimBlast.GetByBlastIDInt32UserBoolean = (_, __, ___) => new BlastAB();
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate(blastSingle, _user);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(6),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(BlastIdInvalid));
        }

        [Test]
        public void Validate_EmailIdNotExists_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId
            };

            ShimBlast.GetByBlastIDInt32UserBoolean = (_, __, ___) => new BlastAB();
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 0;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate(blastSingle, _user);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(6),
                () => exception.ErrorList[1].ErrorMessage.ShouldBe(EmailIdNotExists));
        }

        [Test]
        public void Validate_TriggerPlanInvalid_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId
            };

            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimBlast.GetByBlastIDInt32UserBoolean = (_, __, ___) => new BlastAB { BlastType = BlastTypeNoOpen };
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 0;
            ShimTriggerPlans.ExistsInt32Int32 = (_, __) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate(blastSingle, _user);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(6),
                () => exception.ErrorList[2].ErrorMessage.ShouldBe(TriggerPlanIdInvalid));
        }

        [Test]
        public void Validate_BlastIdNull_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId
            };

            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimBlast.GetByBlastIDInt32UserBoolean = (_, __, ___) => null;
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 0;
            ShimTriggerPlans.ExistsInt32Int32 = (_, __) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate(blastSingle, _user);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(6),
                () => exception.ErrorList[2].ErrorMessage.ShouldBe(BlastIdInvalid));
        }

        [Test]
        public void Validate_ValidData_ReturnsNoError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId,
                LayoutPlanID = LayoutPlanId,
                RefBlastID = RefBlastId,
                CreatedUserID = CreatedUserId
            };

            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimBlast.GetByBlastIDInt32UserBoolean = (_, __, ___) => new BlastAB { BlastType = BlastTypeNoOpen };
            ShimBlast.ExistsInt32Int32 = (_, __) => true;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 1;
            ShimTriggerPlans.ExistsInt32Int32 = (_, __) => true;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => true;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => true;

            // Act, Assert
            Should.NotThrow(() =>
            {
                CommBlastSingle.Validate(blastSingle, _user);
            });
        }

        [Test]
        public void Validate_NoAccessCheck_CustomerIdNull_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = null,
                BlastID = BlastId
            };

            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastAB();

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate_NoAccessCheck(blastSingle);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(CustomerIdInvalid));
        }

        [Test]
        public void Validate_NoAccessCheck_BlastIdInvalid_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId
            };

            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastAB();
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate_NoAccessCheck(blastSingle);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(5),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe(BlastIdInvalid));
        }

        [Test]
        public void Validate_NoAccessCheck_EmailIdNotExists_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId
            };

            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastAB();
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 0;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate_NoAccessCheck(blastSingle);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(5),
                () => exception.ErrorList[1].ErrorMessage.ShouldBe(EmailIdNotExists));
        }

        [Test]
        public void Validate_NoAccessCheck_TriggerPlanInvalid_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId
            };

            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastAB { BlastType = BlastTypeNoOpen };
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 0;
            ShimTriggerPlans.ExistsInt32Int32 = (_, __) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate_NoAccessCheck(blastSingle);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(5),
                () => exception.ErrorList[2].ErrorMessage.ShouldBe(TriggerPlanIdInvalid));
        }

        [Test]
        public void Validate_NoAccessCheck_BlastIdNull_ReturnsError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId
            };

            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => null;
            ShimBlast.ExistsInt32Int32 = (_, __) => false;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 0;
            ShimTriggerPlans.ExistsInt32Int32 = (_, __) => false;

            // Act
            var exception = Should.Throw<ECNException>(() =>
            {
                CommBlastSingle.Validate_NoAccessCheck(blastSingle);
            });

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.Count.ShouldBe(5),
                () => exception.ErrorList[2].ErrorMessage.ShouldBe(BlastIdInvalid));
        }

        [Test]
        public void Validate_NoAccessCheck_ValidData_ReturnsNoError()
        {
            // Arrange
            var blastSingle = new BlastSingle
            {
                CustomerID = CustomerId,
                BlastID = BlastId,
                EmailID = EmailId,
                LayoutPlanID = LayoutPlanId,
                RefBlastID = RefBlastId,
                CreatedUserID = CreatedUserId
            };

            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => false;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (_, __) => new BlastAB { BlastType = BlastTypeNoOpen };
            ShimBlast.ExistsInt32Int32 = (_, __) => true;
            ShimEmail.ExistsInt32Int32 = (_, __) => false;
            ShimEmailHistory.FindMergedEmailIDInt32 = (_) => 1;
            ShimTriggerPlans.ExistsInt32Int32 = (_, __) => true;
            ECN_Framework_BusinessLayer.Accounts.Fakes.ShimCustomer.ExistsInt32 = (_) => true;
            KMPlatform.BusinessLogic.Fakes.ShimUser.ExistsInt32Int32 = (_, __) => true;

            // Act, Assert
            Should.NotThrow(() =>
            {
                CommBlastSingle.Validate_NoAccessCheck(blastSingle);
            });
        }
    }
}
