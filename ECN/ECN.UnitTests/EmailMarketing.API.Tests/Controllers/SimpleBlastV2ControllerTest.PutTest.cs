using System;
using System.Net;
using System.Linq;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using EmailMarketing.API.Controllers.Fakes;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class SimpleBlastV2ControllerTest
    {
        [Test]
        public void Put_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitializeFakes();

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
        public void Put_NoFilterId_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.FilterID = 0;
            InitializeFakes();

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
            InitializeFakes();
            ShimSimpleBlastV2Controller.AllInstances.CleanseInputData_ValidateForeignKeysSimpleBlastV2 = (instance, modelData) => 1;

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
            InitializeFakes();

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
        public void Put_ContentLayoutException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitializeFakes();
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
            InitializeFakes();
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
            InitializeFakes();
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
            InitializeFakes();
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

        [Test]
        public void Put_NotValidatedContent_ECNException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitializeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (_,__) =>
                    new ECN_Framework_Entities.Communicator.Content() { IsValidated = false };

            var exceptionOccured = false;
            ECNException exception = null;

            // Act
            try
            {
                testObject.Put(1, model);
            }
            catch (ECNException e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            exception.ErrorList.ShouldAllBe(e => e.ErrorMessage.Contains("Content for LayoutID is not validated"));
        }
    }
}
