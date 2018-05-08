using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class QuickTestBlastControllerTest
    {
        private string[] combinationErrorMessages = new string[] 
        {
            "Cannot supply CampaignItemID and CampaignItemName",
            "Cannot supply CampaignItemID and CampaignID",
            "Cannot supply CampaignItemID and CampaignName",
            //Unreachable
            "Cannot supply CampaignID and CampaignName",
            //Unreachable
            "Cannot supply CampaignItemID and CampaignItemName",
            "Cannot supply CampaignID and CampaignName",
            "Must supply either CampaignID or CampaignName",
            "CampaignItemName is required",
            "CampaignItemID or CampaignItemName is required",
            //Unreachable
            "Missing required data"
        };
        private IList<ECNError> _errorList;

        [Test]
        public void CleanseInputData_ValidateForeignKeys_WithCampaignItemId_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.CampaignItemID = 1;
            InitilizeFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            exceptionOccured.ShouldBeFalse();
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_WithCampaignItemName_Success()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.CampaignItemName = "ItemName";
            model.CampaignID = 1;
            InitilizeFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;
            Exception exception = null;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
                exception = e;
            }

            // Assert
            exceptionOccured.ShouldBeFalse();
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_NoModelExceptions()
        {
            // Arrange
            var testObject = CreateController();
            InitilizeFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", new object[] { null });
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldNotBeNull();
            _errorList.Count.ShouldBe(1);
            _errorList[0].ErrorMessage.ShouldBe("bad request");
        }


        [Test]
        [TestCase(0, "", 1, "item", 0)]
        [TestCase(1, "", 1, "", 1)]
        [TestCase(0, "item", 1, "", 2)]
        [TestCase(1, "item", 0, "item", 5)]
        [TestCase(0, "", 0, "item", 6)]
        [TestCase(0, "item", -1, "", 7)]
        [TestCase(0, "", -1, "", 8)]
        public void CleanseInputData_ValidateForeignKeys_IdNameCombinationExceptions(
            int campaignId, string campaignName, int campaignItemId, string campaignItemName, int index)
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            int? cid = campaignId;
            model.CampaignID = cid == -1 ? null : cid;
            model.CampaignName = campaignName;
            cid = campaignItemId;
            model.CampaignItemID = cid == -1 ? null : cid;
            model.CampaignItemName = campaignItemName;
            InitilizeFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldNotBeNull();
            _errorList.Count.ShouldBe(1);
            _errorList[0].ErrorMessage.ShouldBe(combinationErrorMessages[index]);
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_EmptyModelExceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = new QuickTestBlast();
            model.CampaignItemID = 1;
            InitilizeFakes();
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldNotBeNull();
            _errorList.Count.ShouldBe(5);
            _errorList[0].ErrorMessage.ShouldBe("EmailFrom is required");
            _errorList[1].ErrorMessage.ShouldBe("EmailFromName is required");
            _errorList[2].ErrorMessage.ShouldBe("ReplyTo is required");
            _errorList[3].ErrorMessage.ShouldBe("Missing EmailSubject");
            _errorList[4].ErrorMessage.ShouldBe("LayoutID is missing from request");
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys_EmailFormat_Group_Layout_Exceptions()
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.CampaignItemID = 1;
            model.EmailSubject += "\r\n";
            InitilizeFakes();
            ShimGroup.ExistsInt32Int32 = (groupID, customerID) => false;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => null;
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldNotBeNull();
            _errorList.Count.ShouldBe(3);
            _errorList[0].ErrorMessage.ShouldBe("Email Subject contains newline characters");
            _errorList[1].ErrorMessage.ShouldBe("GroupID unknown or inaccessible");
            _errorList[2].ErrorMessage.ShouldBe("LayoutID unknown or inaccessible");
        }

        [Test]
        public void CleanseInputData_ValidateForeignKeys__LayoutNotValidatedExceptions([Range(1,9,1)] int index)
        {
            // Arrange
            var testObject = CreateController();
            var model = CreateModel();
            model.CampaignItemID = 1;
            InitilizeFakes();
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) => index == id ?
                new ECN_Framework_Entities.Communicator.Content { } : new ECN_Framework_Entities.Communicator.Content { IsValidated = true};
            var pTestObject = new PrivateObject(testObject);
            var exceptionOccured = false;

            // Act
            try
            {
                pTestObject.Invoke("CleanseInputData_ValidateForeignKeys", model);
            }
            catch (Exception e)
            {
                exceptionOccured = true;
            }

            // Assert
            exceptionOccured.ShouldBeTrue();
            _errorList.ShouldNotBeNull();
            _errorList.Count.ShouldBe(1);
            _errorList[0].ErrorMessage.ShouldBe("Content for LayoutID is not validated");
        }

        private QuickTestBlastController CreateController()
        {
            var testObject = new QuickTestBlastController();
            testObject.Request = new HttpRequestMessage();
            return testObject;
        }

        private QuickTestBlast CreateModel()
        {
            return new QuickTestBlast
            {
                LayoutID = 1,
                EmailFrom = "from@km.com",
                EmailFromName = "KM",
                ReplyTo = "reply@km.com",
                EmailSubject = "Subject",
                GroupID = 1
            };
        }

        private void InitilizeFakes()
        {
            ShimGroup.ExistsInt32Int32 = (groupID, customerID) => true;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => new Layout
            {
                ContentSlot1 = 1,
                ContentSlot2 = 2,
                ContentSlot3 = 3,
                ContentSlot4 = 4,
                ContentSlot5 = 5,
                ContentSlot6 = 6,
                ContentSlot7 = 7,
                ContentSlot8 = 8,
                ContentSlot9 = 9
            };

            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = true };
            ShimECNException.ConstructorIListOfECNErrorEnumsExceptionLayer = (instance, errorList, layer) => _errorList = errorList;      
            ShimHttpRequestMessage.AllInstances.PropertiesGet = (instance) =>
            new Dictionary<string, object>
            {
                { Strings.Headers.CustomerIdHeader, 1 },
                { Strings.Headers.APIAccessKeyHeader, string.Empty},
                { Strings.Properties.APIUserStashKey, new KMPlatform.Entity.User { } },
                { Strings.Properties.APICustomerStashKey, new ECN_Framework_Entities.Accounts.Customer { CustomerID = 1 } },
                { Strings.Properties.APIBaseChannelStashKey, new ECN_Framework_Entities.Accounts.BaseChannel()}
            };
        }
    }
}
