using System;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class PersonalizationBlastControllerTest
    {
        [Test]
        public void ValidateModelState_Success()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeFalse();
        }

        [Test]
        public void ValidateModelState_NullModelException()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            InitilizeFakes();
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", new object[] { null });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(1),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("no model in request body")
            );
        }

        [Test]
        public void ValidateModelState_EmptyModelException()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = new PersonalizationBlast();
            InitilizeFakes();
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(6),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("EmailFrom is required"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("ReplyTo is required"),
                () => ecnException.ErrorList[2].ErrorMessage.ShouldBe("Email Subject is required"),
                () => ecnException.ErrorList[3].ErrorMessage.ShouldBe("GroupID is required"),
                () => ecnException.ErrorList[4].ErrorMessage.ShouldBe("SendTime is required"),
                () => ecnException.ErrorList[5].ErrorMessage.ShouldBe("LayoutID is required")
            );
        }

        [Test]
        public void ValidateModelState_InvalidSubject_NullGroupFilterExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            model.EmailSubject = "subject&#1";
            InitilizeFakes();
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (groupId) => null;
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => null;
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => false;
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(6),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Email Subject cannot contain html representation of characters"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("GroupID specified does not exist"),
                () => ecnException.ErrorList[2].ErrorMessage.ShouldBe("Filter specified does not exist"),
                () => ecnException.ErrorList[3].ErrorMessage.ShouldBe("Suppression Group specified does not exist"),
                () => ecnException.ErrorList[4].ErrorMessage.ShouldBe("Opt Out Group specified does not exist"),
                () => ecnException.ErrorList[5].ErrorMessage.ShouldBe("LayoutID does not exist or is not accessible")
            );
        }

        [Test]
        public void ValidateModelState_InvalidSubject_NullFilterExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => null;
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(2),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Filter specified does not exist"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("Suppression Group Filter specified does not exist")
            );
        }

        [Test]
        public void ValidateModelState_ArchivedGroupFilterExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (groupId) => new ECN_Framework_Entities.Communicator.Group { Archived = true };
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter { Archived = true, GroupID = 1, FilterID = 1 };
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(5),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("GroupID specified is Archived"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("Filter specified is Archived"),
                () => ecnException.ErrorList[2].ErrorMessage.ShouldBe("Suppression Group specified is Archived"),
                () => ecnException.ErrorList[3].ErrorMessage.ShouldBe("Suppression Group Filter specified is Archived"),
                () => ecnException.ErrorList[4].ErrorMessage.ShouldBe("Opt Out Group specified is Archived")
            );
        }

        [Test]
        public void ValidateModelState_InvalidCustomerIdGroupFilterExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (groupId) => new ECN_Framework_Entities.Communicator.Group { CustomerID = 2 };
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter { CustomerID = 2, GroupID = 1, FilterID = 1 };
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(5),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("GroupID specified does not belong to Customer specified"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("Filter specified does not belong to Customer specified"),
                () => ecnException.ErrorList[2].ErrorMessage.ShouldBe("Suppression Group specified does not belong to Customer specified"),
                () => ecnException.ErrorList[3].ErrorMessage.ShouldBe("Suppression Group Filter specified does not belong to Customer specified"),
                () => ecnException.ErrorList[4].ErrorMessage.ShouldBe("Opt Out Group specified does not belong to Customer specified")
            );
        }

        [Test]
        public void ValidateModelState_InvalidGroupIdFilterExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter { GroupID = 2, FilterID = 1 };
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(2),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Filter specified does not belong to group specified"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("Suppression Group Filter specified does not belong to Suppression Group specified")
            );
        }

        [Test]
        public void ValidateModelState_DeletedFilterExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter { IsDeleted = true, GroupID = 1, CustomerID = 1, FilterID = 1 };
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(2),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Filter specified does not exist"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("Suppression Group Filter specified does not exist")
            );
        }

        [Test]
        public void ValidateModelState_LayoutExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimContentFilter.HasDynamicContentInt32 = (id) => true;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, ignore) => new Layout { Archived = true, ContentSlot1 = 1, TemplateID = 1 };
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, ignore) => new ECN_Framework_Entities.Communicator.Content { ContentSource = "ECN.RSSFEED" };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) => new Template { TemplateSource = "%%publicview%%" };
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(4),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Default Message cannot have dynamic content"),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("Default Message is Archived"),
                () => ecnException.ErrorList[2].ErrorMessage.ShouldBe("Default Message content cannot have RSS Feeds"),
                () => ecnException.ErrorList[3].ErrorMessage.ShouldBe("Default Message Template cannot have an ECN public preview link")
            );
        }

        [Test]
        public void ValidateModelState_PublicContentExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            InitilizeFakes();
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, ignore) => new Layout { ContentSlot1 = 1, TemplateID = 1 };
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, ignore) => new ECN_Framework_Entities.Communicator.Content { ContentSource = "%%publicview%%" };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) => new Template { };
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(1),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Default Message content cannot have KM public preview link")
            );
        }

        [Test]
        public void ValidateModelState_ModelExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var pTestObject = new PrivateObject(testObject);
            var model = CreateModel();
            model.CampaignName = "name";
            model.SendTime = DateTime.Now.AddHours(-1);
            InitilizeFakes();
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("ValidateModelState", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            var ecnException = exception.InnerException as ECNException;
            exceptionOccured.ShouldBeTrue();
            ecnException.ShouldSatisfyAllConditions(
                () => ecnException.ShouldNotBeNull(),
                () => ecnException.ErrorList.Count.ShouldBe(2),
                () => ecnException.ErrorList[0].ErrorMessage.ShouldBe("Must supply CampaignID OR CampaignName, not both."),
                () => ecnException.ErrorList[1].ErrorMessage.ShouldBe("SendTime must be in the future")
            );
        }
    }
}
