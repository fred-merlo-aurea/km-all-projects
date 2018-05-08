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
        public void Put_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();

            // Act
            testObject.Put(1, model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.OK);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Put_WithEmptySlots_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModelWithEmptySlots();
            model.TableOptions = "N";
            InitilizeFakes();

            // Act
            testObject.Put(1, model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.OK);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Put_WithTableOption_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.TableOptions = "A";
            InitilizeFakes();

            // Act
            testObject.Put(1, model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.OK);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Put_NoModelException()
        {
            // Arrange
            var testObject = CreateController();
            InitilizeFakes();
            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Put(1, null);
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
        public void Put_ContentSlotsNotValidatedException()
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
                testObject.Put(1, model);
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
            exception.ErrorList[0].ErrorMessage.ShouldContain("ContentID  -1,-1,-1,-1,-1,-1,-1,-1,-1 is not validated");
        }

        [Test]
        public void Put_SingleContentSlotNotValidatedException([Range(2, 9, 1)] int slotNumber)
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
                testObject.Put(1, model);
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
            exception.ErrorList[0].ErrorMessage.ShouldBe("ContentID  -1 is not validated");
        }

        //[Test]
        //public void Put_ContentNotValidatedException([Range(1, 9, 1)] int slotNumber)
        //{
        //    // Arrange
        //    var testObject = CreateController();
        //    var model = CreateModel();
        //    InitilizeFakes();
        //    ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) => slotNumber == id ?
        //        null : new ECN_Framework_Entities.Communicator.Content { IsValidated = true  }  ;
        //    var exceptionOccured = false;
        //    ECNException exception = null;

        //    // Act
        //    try
        //    {
        //        testObject.Put(1, model);
        //    }
        //    catch (Exception e)
        //    {
        //        exceptionOccured = true;
        //        exception = e as ECNException;
        //    }

        //    // Assert
        //    exceptionOccured.ShouldBeTrue();
        //    exception.ShouldNotBeNull();
        //    exception.ErrorList.ShouldNotBeNull();
        //    exception.ErrorList.Count.ShouldBe(1);
        //    exception.ErrorList[0].ErrorMessage.ShouldBe("ContentID for ContentSlot" + slotNumber + " doesn't exist");
        //}
    }
}
