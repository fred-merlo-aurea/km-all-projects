using System;
using System.Net;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Controllers.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class PersonalizationBlastControllerTest
    {
        private HttpStatusCode _resultCode;
        private string _resultRoute;

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
        public void Post_NoCampaignId_CampaignItemName_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.CampaignID = null;
            model.CampaignItemName = "CampaignItemName";
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
        public void Post_WithOldSgementId_CampaignName_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.CampaignID = null;
            model.CampaignName = "CampaignName";
            InitilizeFakes();
            ShimPersonalizationBlastController.AllInstances.CleanseInputData_ValidateForeignKeysPersonalizationBlast
                = (instance, modelData) => 1;

            // Act
            testObject.Post(model);

            // Assert
            _resultCode.ShouldNotBeNull();
            _resultCode.ShouldBe(HttpStatusCode.Created);
            _resultRoute.ShouldNotBeNull();
            _resultRoute.ShouldBe("DefaultApi");
        }

        [Test]
        public void Post_SecurityException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            KMPlatform.BusinessLogic.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = 
                (user, service, feature, access) => false;
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
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(typeof(SecurityException));
            exception.Message.ShouldBe("SECURITY VIOLATION!");
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
            exception.ShouldNotBeNull();
            exception.ErrorList.ShouldNotBeNull();
            exception.ErrorList.Count.ShouldBe(1);
            exception.ErrorList[0].ErrorMessage.ShouldContain("NO LICENSES AVAILABLE");
        }

        [Test]
        public void Post_TempleteSlotsException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) => new Template { SlotsTotal = 2 };
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
            exception.ErrorList[0].ErrorMessage.ShouldContain("Message cannot have multiple slots");
        }

        [Test]
        public void Post_DynamicContentException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimContent.CheckForDynamicTagsString = (content) => true;
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
            exception.ErrorList[0].ErrorMessage.ShouldContain("Message cannot have dynamic tags");
        }

        [Test]
        public void Post_ContentNotValidatedException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new Content { IsValidated = false };
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
            exception.ErrorList[0].ErrorMessage.ShouldContain("Content for LayoutID is not validated");
        }

        [Test]
        public void Post_MultipleContentSlotsException([Range(2, 9, 1)] int slotNumber)
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) =>
            {
                var layout = new Layout { ContentSlot1 = 1, TemplateID = 1 };
                PrivateObject playout = new PrivateObject(layout);
                playout.SetProperty("ContentSlot"+ slotNumber, slotNumber);
                return layout;                
            };

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
            exception.ErrorList[0].ErrorMessage.ShouldContain("Message cannot have multiple slots");
        }

        [Test]
        public void Post_NoCampaignException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (campaignId, user, ignore) => null;
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
            exception.ErrorList[0].ErrorMessage.ShouldContain("CampaignID is invalid or doesn't exist");
        }

        [Test]
        public void Post_ArchivedCampaignException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (campaignId, user, ignore) => new Campaign { CampaignID = 1, IsArchived = true };
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
            exception.ErrorList[0].ErrorMessage.ShouldContain("Campaign is archived");
        }

        [Test]
        public void Post_UnkownException()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            InitilizeFakes();
            ShimBlast.GetByCampaignItemBlastIDInt32UserBoolean = (id, user, ignore) => (new Mock<BlastAbstract>()).Object;
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
            exception.ShouldNotBeNull();
            exception.Message.ShouldContain("Processing of the HTTP request resulted in an exception");
        }
    }
}
