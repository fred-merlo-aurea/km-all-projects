using System;
using System.Reflection;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string ValidateDeleteMethodName = "ValidateDelete";

        [Test]
        public void ValidateDelete_WhenControlTypeCampaignItem_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.CampaignItem
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Campaign Item because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeClick_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Click
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Click email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeDirectClick_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Direct_Click
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Click trigger because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeDirectOpen_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Direct_Open
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() => 
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Open trigger because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeEnd_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.End
            };

            // Act and Assert
            Should.NotThrow(() =>
            _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeGroup_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Group
            };

            // Act and Assert
            Should.NotThrow(() =>
            _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeNoClick_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.NoClick
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete No Click email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeNoOpen_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.NoOpen
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete No Open email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeNotSent_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.NotSent
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Not Sent email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeOpen_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Open
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Open email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeOpen_NoClick_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Open_NoClick
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Open/NoClick email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeSent_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Sent
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Sent email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeStart_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Start
            };

            // Act and Assert
            Should.NotThrow(() =>
            _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeSubscribe_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Subscribe
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Subscribe trigger because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeSuppressed_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Suppressed
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Suppressed email because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeUnsubscribe_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Unsubscribe
            };

            // Act
            var ecnExp = Should.Throw<TargetInvocationException>(() =>
                _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl)).InnerException as ECNException;

            // Assert
            ecnExp.ShouldNotBeNull();
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business),
                () => ecnExp.ErrorList.ShouldBeEmpty(),
                () => ecnExp.Message.ShouldContain("Cannot delete Unsubscribe trigger because it has already gone out"));
        }

        [Test]
        public void ValidateDelete_WhenControlTypeWait_ValidationFailsAndThrowsECNException()
        {
            // Arrange
            SetUpValidateDeleteMethodFakes();
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = MarketingAutomationControlType.Wait
            };

            // Act and Assert
            Should.NotThrow(() =>
            _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl));
        }

        [Test]
        [TestCase(MarketingAutomationControlType.Click)]
        [TestCase(MarketingAutomationControlType.CampaignItem)]
        [TestCase(MarketingAutomationControlType.Direct_Click)]
        [TestCase(MarketingAutomationControlType.Direct_Open)]
        [TestCase(MarketingAutomationControlType.Direct_NoOpen)]
        [TestCase(MarketingAutomationControlType.NoClick)]
        [TestCase(MarketingAutomationControlType.NoOpen)]
        [TestCase(MarketingAutomationControlType.NotSent)]
        [TestCase(MarketingAutomationControlType.Open)]
        [TestCase(MarketingAutomationControlType.Open_NoClick)]
        [TestCase(MarketingAutomationControlType.Sent)]
        [TestCase(MarketingAutomationControlType.Subscribe)]
        [TestCase(MarketingAutomationControlType.Suppressed)]
        [TestCase(MarketingAutomationControlType.Unsubscribe)]
        public void ValidateDelete_WhenValidControls_DoesNotThrowException(MarketingAutomationControlType controlType)
        {
            // Arrange
            SetUpValidateDeleteMethodFakes(daysToAdd: 1, statusCode: "Not Sent");
            var maControl = new CommunicatorEntities.MAControl
            {
                ECNID = 1,
                ControlType = controlType
            };

            // Act and Assert
            Should.NotThrow(() =>
            _diagramsControllerPrivateObject.Invoke(ValidateDeleteMethodName, maControl));
        }

        private void SetUpValidateDeleteMethodFakes(int daysToAdd = -1,string statusCode = "Sent")
        {
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (i, b) => new CommunicatorEntities.CampaignItem
            {
                SendTime = DateTime.Now.AddDays(daysToAdd)
            };

            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (i, u) => new CommunicatorEntities.LayoutPlans
            {
                BlastID = 1
            };
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (i, u) => new CommunicatorEntities.BlastChampion
            {
                StatusCode = statusCode
            };
        }
    }
}
