using System;
using System.Collections.Generic;
using System.Fakes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Models;
using Entities = ECN_Framework_Entities.Communicator;
using FakeModels = EmailMarketing.API.Models.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace EmailMarketing.API.Tests.Controllers
{
    public partial class SimpleBlastV2ControllerTest
    {
        private const int DefaultId = 1;
        private SimpleBlastV2Controller _simpleBlastV2Controller;
        private void InitializePostTest()
        {
            var baseUri = new System.Uri("http://localhost");
            _simpleBlastV2Controller = new SimpleBlastV2Controller();
            _simpleBlastV2Controller.Request = new HttpRequestMessage()
            {
                RequestUri = baseUri
            };
            _simpleBlastV2Controller.Configuration = new HttpConfiguration();

            _simpleBlastV2Controller.Configuration.Routes.MapHttpRoute(
                name: Strings.Routing.DefaultApiRouteName,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            _simpleBlastV2Controller.Request.SetConfiguration(_simpleBlastV2Controller.Configuration);

            ShimHttpRequestMessage.AllInstances.PropertiesGet = (instance) =>
            new Dictionary<string, object>
            {
                [Strings.Headers.CustomerIdHeader] = DefaultId,
                [Strings.Headers.APIAccessKeyHeader] = string.Empty,
                [Strings.Properties.APIUserStashKey] = new KMPlatform.Entity.User { },
                [Strings.Properties.APICustomerStashKey] = new ECN_Framework_Entities.Accounts.Customer { CustomerID = DefaultId },
                [Strings.Properties.APIBaseChannelStashKey] = new ECN_Framework_Entities.Accounts.BaseChannel(),
                [HttpPropertyKeys.HttpConfigurationKey] = _simpleBlastV2Controller.Configuration
            };
        }

        [Test]
        [TestCase(false, "UNLIMITED", "NO LICENSE", true, 10, DefaultId)]
        [TestCase(true, "UNLIMITED", "NO LICENSE", true, 10, DefaultId)]
        [TestCase(true, "NO LICENSE", "NO LICENSE", true, 11, DefaultId)]
        [TestCase(true, "UNLIMITED", "NO LICENSE", true, 11, DefaultId)]
        [TestCase(true, "UNLIMITED", "NO LICENSE", false, 10, -1)]
        public void Post_IsContentValidated_ThrowsECNException(
            bool isContentValidated,
            string currentLicense,
            string availableLicense,
            bool isTestBlast,
            int blastEmailsListCount,
            int campaignID)
        {
            //Arrange
            InitializePostTest();
            SetupFakes(isContentValidated, currentLicense, availableLicense, blastEmailsListCount, campaignID);
            var simpleBlastV2Model = GetSimpleBlastV2(isTestBlast, campaignID, null);
            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (xmlSchedule, userId) => null;

            //Act
            //Assert
            Should.Throw<ECNException>(() => _simpleBlastV2Controller.Post(simpleBlastV2Model));
        }

        [Test]
        [TestCase(true, "UNLIMITED", "NO LICENSE", true, 10, -1)]
        public void Post_IsContentValidated_ThrowsHTTPResponseException(
            bool isContentValidated,
            string currentLicense,
            string availableLicense,
            bool isTestBlast,
            int blastEmailsListCount,
            int campaignID)
        {
            //Arrange
            InitializePostTest();
            SetupFakes(isContentValidated, currentLicense, availableLicense, blastEmailsListCount, campaignID);
            var simpleBlastV2Model = GetSimpleBlastV2(isTestBlast, campaignID, null);
            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (xmlSchedule, userId) => new Entities.BlastSchedule
            {
                BlastScheduleID = DefaultId
            };

            //Act
            //Assert
            Should.Throw<HttpResponseException>(() => _simpleBlastV2Controller.Post(simpleBlastV2Model));
        }
        [Test]
        [TestCase(true, "UNLIMITED", "NO LICENSE", false, 10, -1, "test")]
        [TestCase(true, "UNLIMITED", "NO LICENSE", false, 10, -1, null)]
        public async Task Post_IsContentValidated_ReturnsResponse(
            bool isContentValidated,
            string currentLicense,
            string availableLicense,
            bool isTestBlast,
            int blastEmailsListCount,
            int campaignID,
            string campaignName)
        {
            //Arrange
            InitializePostTest();
            SetupFakes(isContentValidated, currentLicense, availableLicense, blastEmailsListCount, campaignID);
            var simpleBlastV2Model = GetSimpleBlastV2(isTestBlast, campaignID, campaignName);
            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (xmlSchedule, userId) => new Entities.BlastSchedule
            {
                BlastScheduleID = DefaultId
            };

            //Act
            var responseMessage = _simpleBlastV2Controller.Post(simpleBlastV2Model);

            //Assert
            var returnedSimpleBlastV2 = await responseMessage.Content.ReadAsAsync<SimpleBlastV2>();
            responseMessage.StatusCode.ShouldBe(HttpStatusCode.Created);
            returnedSimpleBlastV2.ShouldSatisfyAllConditions(
                    () => returnedSimpleBlastV2.BlastID.ShouldBe(DefaultId),
                    () => returnedSimpleBlastV2.CampaignID.ShouldBe(DefaultId),
                    () => returnedSimpleBlastV2.SendTime.ShouldBe(DateTime.MinValue));
        }


        [Test]
        [TestCase(true, "UNLIMITED", "NO LICENSE", false, 10, -1, "test")]
        public async Task Post_EmailSubjectContainsEmoji_ReplaceThemWithInMailSubject(
            bool isContentValidated,
            string currentLicense,
            string availableLicense,
            bool isTestBlast,
            int blastEmailsListCount,
            int campaignID,
            string campaignName)
        {
            //Arrange
            InitializePostTest();
            SetupFakes(isContentValidated, currentLicense, availableLicense, blastEmailsListCount, campaignID);
            var simpleBlastV2Model = GetSimpleBlastV2(isTestBlast, campaignID, campaignName);
            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (xmlSchedule, userId) => new Entities.BlastSchedule
            {
                BlastScheduleID = DefaultId
            };
            var emojiCharacters = $"{'\u2000'} {'\uDB00'}{'\uDFEF'}";
            var testEmailSubject = $"Test Subject {emojiCharacters}";
            simpleBlastV2Model.EmailSubject = testEmailSubject;

            //Act
            var responseMessage = _simpleBlastV2Controller.Post(simpleBlastV2Model);

            // Assert
            simpleBlastV2Model.EmailSubject.ShouldBe("Test Subject \\u2000 \\udb00\\udfef");
        }

        [Test]
        [TestCase(true, "UNLIMITED", "NO LICENSE", false, 10, -1, "test")]
        public async Task Post_ContentNotValidated_ThrowsEcnExceptiom(
            bool isContentValidated,
            string currentLicense,
            string availableLicense,
            bool isTestBlast,
            int blastEmailsListCount,
            int campaignID,
            string campaignName)
        {
            //Arrange
            InitializePostTest();
            SetupFakes(false, currentLicense, availableLicense, blastEmailsListCount, campaignID);
            var simpleBlastV2Model = GetSimpleBlastV2(isTestBlast, campaignID, campaignName);
            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (xmlSchedule, userId) => new Entities.BlastSchedule
            {
                BlastScheduleID = DefaultId
            };

            //Act
            ECNException exception = null;
            try
            {
                _simpleBlastV2Controller.Post(simpleBlastV2Model);
            }
            catch (ECNException ex)
            {
                exception = ex;
            }

            // Assert
            exception.ErrorList.ShouldAllBe(e => e.ErrorMessage.Contains("Content for LayoutID is not validated"));
        }


        private SimpleBlastV2 GetSimpleBlastV2(bool isTestBlast, int campaignId, string campaignName)
        {
            var testEmail = "test@test.com";
            var emojiCharacters = $"{'\u2000'} {'\uDB00'}{'\uDFEF'}";
            var testEmailSubject = $"Test Subject {emojiCharacters}";
            var simpleBlastV2Model = new SimpleBlastV2
            {
                EmailFrom = testEmail,
                ReplyTo = testEmail,
                CampaignName = campaignName,
                CampaignItemName = campaignName,
                EmailSubject = testEmailSubject,
                FilterID = DefaultId,
                GroupID = DefaultId,
                Schedule = new SimpleBlastSchedule
                {
                    StartDate = new DateTime(2018, 1, 1),
                    EndDate = new DateTime(2018, 1, 2)
                },
                ReferenceBlasts = new int[] { 1, 2 },
                IsTestBlast = isTestBlast
            };
            if (campaignId > 0)
            {
                simpleBlastV2Model.CampaignID = campaignId;
            }
            return simpleBlastV2Model;
        }
        private void SetupFakes(
            bool isContentValidated,
            string currentLicense,
            string availableLicense,
            int blastEmailsListCount,
            int campaignId)
        {
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new Entities.Filter
            {
                GroupID = id,
                FilterID = id,
                CustomerID = id
            };
            ShimGroup.ExistsInt32Int32 = (groupId, customerId) => true;
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => true;
            ShimFilter.ExistsInt32Int32 = (filterId, customerId) => false;
            ShimLayout.ValidateLayoutContentInt32 = (layoutId) =>
            {
                return new List<string>();
            };
            ShimGroup.ValidateDynamicStringsListOfStringInt32User = (listLy, groupId, user) => { };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, getChildern) => new Entities.Layout
            {
                ContentSlot1 = DefaultId,
                ContentSlot2 = DefaultId,
                ContentSlot3 = DefaultId,
                ContentSlot4 = DefaultId,
                ContentSlot5 = DefaultId,
                ContentSlot6 = DefaultId,
                ContentSlot7 = DefaultId,
                ContentSlot8 = DefaultId,
                ContentSlot9 = DefaultId
            };
            ShimSmartSegment.SmartSegmentOldExistsInt32 = (filterId) => true;
            ShimBlast.RefBlastsExistsStringInt32DateTime = (blastIDs, customerID, sendTime) => true;
            ShimSmartSegment.GetNewIDFromOldIDInt32 = (smartSegmentId) => smartSegmentId;
            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (contentId, getChildern) => new Entities.Content
            {
                IsValidated = isContentValidated,
                ContentID = DefaultId
            };
            ShimLicenseCheck.AllInstances.CurrentString = (licenseCheck, customerID) => currentLicense;
            ShimLicenseCheck.AllInstances.AvailableString = (licenseCheck, customerID) => availableLicense;
            ShimBlast.GetBlastEmailsListCountInt32Int32Int32ListOfCampaignItemBlastFilterStringStringBooleanUser = (customerID, blastID, groupID, filters,
                                                                                                 blastIDAndBounceDomain, suppressionList, testblast, user) =>
            {
                return blastEmailsListCount;
            };
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (campaignID, user, getChildren) =>
            {
                var campaign = new Entities.Campaign();
                if (campaignID < 0)
                {
                    campaign.CampaignID = DefaultId;
                }
                else
                {
                    campaignID = -1;
                }
                return campaign;
            };
            ShimCampaign.GetByCampaignNameStringUserBoolean = (campaignName, user, getChildren) => new Entities.Campaign
            {
                CampaignID = campaignId,
                CampaignName = campaignName
            };
            ShimCampaign.SaveCampaignUser = (campaign, user) => DefaultId;
            ShimDateTime.NowGet = () => new DateTime(2018, 1, 1);
            ShimCampaignItem.SaveCampaignItemUser = (campaignItem, user) => DefaultId;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (campaignItemBlast, user, ignoreArchivedGroup) => DefaultId;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (campaignItemBlastFilter) => DefaultId;
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (campaignItemBlast, user, isQTB) => DefaultId;
            ShimBlast.GetByCampaignItemTestBlastIDInt32UserBoolean = (campaignItemBlast, user, getChildren) => null;
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (campaignItemId, user, checkFirst) => { };
            ShimBlast.GetByCampaignItemBlastIDInt32UserBoolean = (campaignItemBlastId, user, getChildren) => new Entities.BlastAB
            {
                BlastID = DefaultId
            };

            ShimBlastSetupInfo.GetNextScheduledBlastSetupInfoInt32Boolean = (blastScheduledId, includeToday) => new Entities.BlastSetupInfo
            {
                BlastScheduleID = DefaultId
            };
        }
    }
}