using System;
using System.Net;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using Moq;
using NUnit.Framework;
using Shouldly;


namespace EmailMarketing.API.Tests.Controllers
{
    public partial class MessageControllerTest
    {
        [Test]
        public void Post_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();

            // Act
            testObject.Post(model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.Created);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Post_WithEmptySlots_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModelWithEmptySlots();
            model.TableOptions = "N";
            InitilizeFakes();

            // Act
            testObject.Post(model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.Created);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Post_WithTableOption_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.TableOptions = "A";
            InitilizeFakes();

            // Act
            testObject.Post(model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.Created);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Post_NoModelException()
        {
            // Arrange
            var testObject = CreateController();
            InitilizeFakes();
            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Post(null);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e as ECNException;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldNotBeNull();
            exception.ErrorList.ShouldNotBeNull();
            exception.ErrorList.Count.ShouldBe(1);
            exception.ErrorList[0].ErrorMessage.ShouldBe("no model in request body");
        }

        [Test]
        public void Post_ContentSlotsNotValidatedException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = false };
            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Post(model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e as ECNException;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldNotBeNull();
            exception.ErrorList.ShouldNotBeNull();
            exception.ErrorList.Count.ShouldBe(1);
            exception.ErrorList[0].ErrorMessage.ShouldContain("ContentID -1,-1,-1,-1,-1,-1,-1,-1,-1 is not validated");
        }

        [Test]
        public void Post_SingleContentSlotNotValidatedException([Range(2, 9, 1)] int slotNumber)
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = slotNumber != id };
            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Post(model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e as ECNException;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldNotBeNull();
            exception.ErrorList.ShouldNotBeNull();
            exception.ErrorList.Count.ShouldBe(1);
            exception.ErrorList[0].ErrorMessage.ShouldBe("ContentID -1 is not validated");
        }

        [Test]
        public void Post_NullContentException([Range(1, 9, 1)] int slotNumber)
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) => slotNumber == id ?
                null : new ECN_Framework_Entities.Communicator.Content { IsValidated = true  }  ;
            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Post(model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e as ECNException;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldNotBeNull();
            exception.ErrorList.ShouldNotBeNull();
            exception.ErrorList.Count.ShouldBe(1);
            exception.ErrorList[0].ErrorMessage.ShouldBe("ContentID for ContentSlot" + slotNumber + " doesn't exist");
        }
    }
}
