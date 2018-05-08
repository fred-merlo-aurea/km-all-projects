using System;
using System.Net;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using EmailMarketing.API.Controllers.Fakes;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class SimpleBlastControllerTest
    {
        [Test]
        public void Put_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.IsTestBlast = false;
            InitilizeFakes();
            ShimSimpleBlastController.AllInstances.CleanseInputData_ValidateForeignKeysSimpleBlast = (instance, modelData) => null;

            // Act
            testObject.Put(1, model);

            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.OK)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Put_TestBlast_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimSimpleBlastController.AllInstances.CleanseInputData_ValidateForeignKeysSimpleBlast = (instance, modelData) => null;

            // Act
            testObject.Put(1, model);

            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.OK)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Put_TestBlast_WithOldSmartSegment_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();

            // Act
            testObject.Put(1, model);

            // Assert
            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.OK)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Put_NoSetupInfo_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.Schedule = null;
            model.SendTime = DateTime.Now.AddHours(1);
            InitilizeFakes();

            // Act
            testObject.Put(1, model);
            
            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.OK)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Put_ContentLayoutException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { };
            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Put(1, model);
            }
            catch(Exception e)
            {
                exceptionOccured = true;
                exception = e as ECNException;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.ErrorList.ShouldNotBeNull(),
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe("Content for LayoutID is not validated")
            );
        }

        [Test]
        public void Put_NoBlastInfoException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimBlastSetupInfo.GetNextScheduledBlastSetupInfoInt32Boolean = (id, child) => null;
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
                exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.ErrorList.ShouldNotBeNull(),
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldContain("bad schedule")
            );
        }

        [Test]
        public void Put_NullCampaignItemBlastException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimCampaignItemBlast.GetByBlastIDInt32UserBoolean = (id, user, ignore) => null;
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                testObject.Put(1, model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.Message.ShouldBe("Exception of type 'EmailMarketing.API.Exceptions.APIResourceNotFoundException' was thrown.")
            );
        }

        [Test]
        public void Put_NullCampaignBlastException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimBlast.GetByBlastIDInt32UserBoolean = (id, user, ignore) => null;
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                testObject.Put(1, model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.Message.ShouldContain("Exception of type 'EmailMarketing.API.Exceptions.APIResourceNotFoundException' was thrown.")
            );
        }
    }
}
