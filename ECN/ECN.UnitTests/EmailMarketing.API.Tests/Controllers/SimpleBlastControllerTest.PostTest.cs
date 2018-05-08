using System;
using System.Net;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Controllers.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;


namespace EmailMarketing.API.Tests.Controllers
{
    public partial class SimpleBlastControllerTest
    {
        [Test]
        public void Post_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.IsTestBlast = false;
            InitilizeFakes();
            ShimSimpleBlastController.AllInstances.CleanseInputData_ValidateForeignKeysSimpleBlast = (instance, modelData) => null;

            // Act
            testObject.Post(model);

            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.Created)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Post_TestBlast_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimSimpleBlastController.AllInstances.CleanseInputData_ValidateForeignKeysSimpleBlast = (instance, modelData) => null;

            // Act
            testObject.Post(model);

            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.Created)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Post_TestBlast_WithOldSmartSegment_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();

            // Act
            testObject.Post(model);

            // Assert
            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.Created)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Post_NoSetupInfo_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.Schedule = null;
            model.SendTime = DateTime.Now.AddHours(1);
            InitilizeFakes();

            // Act
            testObject.Post(model);

            // Assert
            // Assert
            _resultCode.ShouldSatisfyAllConditions
            (
                () => _resultCode.ShouldNotBeNull(),
                () => _resultCode.ShouldBe(HttpStatusCode.Created)
            );
            _resultRoute.ShouldSatisfyAllConditions
            (
                () => _resultRoute.ShouldNotBeNull(),
                () => _resultRoute.ShouldBe("DefaultApi")
            );
        }

        [Test]
        public void Post_ContentLayoutException()
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
                testObject.Post(model);
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
        public void Post_NoLicenseException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimLicenseCheck.AllInstances.CurrentString = (instance, id) => "NO LICENSE";
            ShimLicenseCheck.AllInstances.AvailableString = (instance, id) => "NO LICENSE";
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
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.ErrorList.ShouldNotBeNull(),
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldContain("NO LICENSES AVAILABLE")
             );
        }

        [Test]
        public void Post_NoTestLicenseException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimBlast.GetBlastEmailsListCountInt32Int32Int32ListOfCampaignItemBlastFilterStringStringBooleanUser =
                (id1, id2, id3, filters, domain, suppression, test, APIUser) => 20;
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
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.ErrorList.ShouldNotBeNull(),
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldContain("ERROR: The Group list selected for test blast, contains more than the allowed")
            );
        }

        [Test]
        public void Post_NoBlastInfoException()
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
                testObject.Post(model);
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
        public void Post_OldStartTimeException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            model.Schedule = null;
            model.SendTime = new DateTime(2018, 1, 1);
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
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => exception.ErrorList.ShouldNotBeNull(),
                () => exception.ErrorList.Count.ShouldBe(1),
                () => exception.ErrorList[0].ErrorMessage.ShouldBe("StartTime/Schedule.StartTime must be in the future")
            );
        }

        [Test]
        public void Post_UnkownException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimBlast.GetByCampaignItemTestBlastIDInt32UserBoolean = (id, user, ignore) => (new Mock<BlastAbstract>()).Object;
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                testObject.Post(model);
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
                () => exception.Message.ShouldContain("Processing of the HTTP request resulted in an exception")
            );
        }
    }
}
