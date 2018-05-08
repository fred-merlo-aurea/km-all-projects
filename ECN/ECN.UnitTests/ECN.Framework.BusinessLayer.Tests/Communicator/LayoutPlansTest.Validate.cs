using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Accounts.Fakes;
using ECN_Framework_DataLayer.Communicator.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    public partial class LayoutPlansTest
    {
        private IDisposable _shimObject;
        private PrivateType _testedClass;
        private const string TestedClassName = "ECN_Framework_BusinessLayer.Communicator.LayoutPlans";
        private const string TestedClassAssemblyName = "ECN_Framework_BusinessLayer";
        private const string TestedMethod_Validate = "Validate";
        private const string TestedMethod_ValidateUseAmbientTransaction = "Validate_UseAmbientTransaction";
        private const string Action = "Action";
        private const string Event = "Event";
        private const int LayoutId = 2;
        private const int GroupId = 3;
        private const int ExpectedErrorCount = 6;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testedClass = new PrivateType(TestedClassAssemblyName, TestedClassName);
            SetupShims();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Validate_InvalidValue_ShouldThrowException()
        {
            // Arrange
            var layoutPlans = new LayoutPlans();

            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_Validate, new[] { layoutPlans });
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }

        [Test]
        public void Validate_InvalidCustomerId_ShouldThrowException()
        {
            // Arrange
            var layoutPlans = new LayoutPlans
            {
                CustomerID = CustomerId
            };
            ShimCustomer.ExistsInt32 = (_) => false;

            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_Validate, new[] { layoutPlans });
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
            var ecnException = exception.InnerException as ECNException;
            ecnException.ErrorList.Count.ShouldBe(ExpectedErrorCount);
        }

        [Test]
        public void Validate_InvalidLayoutId_ShouldThrowException()
        {
            // Arrange
            var layoutPlans = new LayoutPlans
            {
                CustomerID = CustomerId,
                LayoutID = LayoutId
            };
            ShimLayout.ExistsInt32Int32 = (_, __) => false;

            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_Validate, new[] { layoutPlans });
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
            var ecnException = exception.InnerException as ECNException;
            ecnException.ErrorList.Count.ShouldBe(ExpectedErrorCount);
        }

        [Test]
        public void Validate_InvalidGroupId_ShouldThrowException()
        {
            // Arrange
            var layoutPlans = new LayoutPlans
            {
                CustomerID = CustomerId,
                GroupID = GroupId
            };
            ShimLayout.ExistsInt32Int32 = (_, __) => false;
            ShimGroup.ExistsInt32Int32 = (_, __) => false;

            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_Validate, new[] { layoutPlans });
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
            var ecnException = exception.InnerException as ECNException;
            ecnException.ErrorList.Count.ShouldBe(ExpectedErrorCount);
        }

        [Test]
        public void Validate_ValidValue_ShouldNotThrowException()
        {
            // Arrange, Act, Assert
            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_Validate, new[] { GetLayoutPlans() });
            });
        }

        [Test]
        public void Validate_UseAmbientTransaction_InvalidValue_ShouldThrowException()
        {
            // Arrange
            var layoutPlans = new LayoutPlans();

            // Act, Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateUseAmbientTransaction, new[] { layoutPlans });
            });

            exception.InnerException.ShouldBeOfType<ECNException>();
        }

        [Test]
        public void Validate_UseAmbientTransaction_ValidValue_ShouldNotThrowException()
        {
            // Arrange, Act, Assert
            Should.NotThrow(() =>
            {
                _testedClass.InvokeStatic(TestedMethod_ValidateUseAmbientTransaction, new[] { GetLayoutPlans() });
            });
        }

        private void SetupShims()
        {
            ShimDataFunctions.ExecuteScalarSqlCommandString = (_, __) => 1;
            ShimUser.GetByUserIDInt32Boolean = (_, __) => new KMPlatform.Entity.User();
            ShimUser.ExistsInt32Int32 = (_, __) => true;
        }

        private LayoutPlans GetLayoutPlans()
        {
            var layoutPlans = new LayoutPlans
            {
                CustomerID = 1,
                LayoutPlanID = 2,
                UpdatedUserID = 3,
                BlastID = 4,
                CreatedUserID = 5,
                Period = 0,
                ActionName = Action,
                EventType = Event
            };

            return layoutPlans;
        }
    }
}