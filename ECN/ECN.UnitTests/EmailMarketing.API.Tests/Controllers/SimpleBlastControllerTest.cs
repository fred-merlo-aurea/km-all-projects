using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Objects.Fakes;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Controllers.Fakes;
using EmailMarketing.API.Models;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;

namespace EmailMarketing.API.Tests.Controllers
{
    /// <summary>
    ///     Unit tests for <see cref="EmailMarketing.API.Controllers.SimpleBlastController"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SimpleBlastControllerTest
    {
        private HttpStatusCode _resultCode;
        private IList<ECNError> _errorList;
        private string _resultRoute;
        private IDisposable _shimObject;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private SimpleBlastController CreateController()
        {
            var testObject = new SimpleBlastController();
            testObject.Request = new HttpRequestMessage();
            return testObject;
        }

        private SimpleBlast CreateModel()
        {
            return new SimpleBlast
            {
                BlastID = 1,
                EmailFrom = "from@km.com",
                ReplyTo = "reply@km.com",
                EmailSubject = "subject",
                FilterID = 1,
                GroupID = 1,
                Schedule = new SimpleBlastSchedule
                {
                    StartDate = DateTime.Now.AddDays(1),
                    EndDate = DateTime.Now.AddDays(2)
                },
                ReferenceBlasts = new int[] { 1, 2 },
                IsTestBlast = true
            };
        }

        private void InitilizeFakes()
        {
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter
            {
                GroupID = 1,
                FilterID = 1,
                CustomerID = 1
            };

            ShimSimpleBlastController.AllInstances.CleanseInputData_ValidateForeignKeysSimpleBlast = (instance, model) => 1;
            ShimSmartSegment.GetNewIDFromOldIDInt32 = (id) => 1;
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
            ShimLicenseCheck.AllInstances.CurrentString = (instance, id) => "UNLIMITED";
            ShimCampaign.GetByCampaignNameStringUserBoolean = (id, name, child) => new Campaign { };
            ShimCampaign.SaveCampaignUser = (campaign, user) => 0;
            InitilizeHttpFakes();
            InitilizeCampaignFakes();
        }

        private void InitilizeHttpFakes()
        {
            ShimHttpRequestMessage.AllInstances.PropertiesGet = (instance) =>
            new Dictionary<string, object>
            {
                { Strings.Headers.CustomerIdHeader, 1 },
                { Strings.Headers.APIAccessKeyHeader, string.Empty},
                { Strings.Properties.APIUserStashKey, new KMPlatform.Entity.User { } },
                { Strings.Properties.APICustomerStashKey, new ECN_Framework_Entities.Accounts.Customer { CustomerID = 1 } },
                { Strings.Properties.APIBaseChannelStashKey, new ECN_Framework_Entities.Accounts.BaseChannel()}
            };
            ShimAuthenticatedUserControllerBase.AllInstances.CreateResponseWithLocationOf1HttpStatusCodeM0Int32String(
                (AuthenticatedUserControllerBase instance, HttpStatusCode statusCode, SimpleBlast model, int id, string routeName) =>
                {
                    _resultCode = statusCode;
                    _resultRoute = routeName;
                    return null;
                });
        }

        private void InitilizeCampaignFakes()
        {
            ShimCampaignItemBlast.GetByBlastIDInt32UserBoolean = (id, user, ignore) => new CampaignItemBlast { BlastID = 1, CampaignItemID = 1 };
            ShimBlast.GetByBlastIDInt32UserBoolean = (id, user, ignore) => new BlastRegular { BlastID = 1 };
            ShimCampaignItem.GetByCampaignItemIDInt32UserBoolean = (id, user, ignore) => new CampaignItem();
            ShimCampaignItemBlastFilter.DeleteByCampaignItemBlastIDInt32 = (id) => { };
            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (schedule, id) =>
                new BlastSchedule { BlastScheduleID = 1 };
            ShimBlastSetupInfo.GetNextScheduledBlastSetupInfoInt32Boolean = (id, child) => new BlastSetupInfo { BlastScheduleID = 1, SendTime = DateTime.Now };
            ShimCampaignItem.SaveCampaignItemUser = (campaignItem, user) => 0;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (campaignItemBlast, user, ignore) => 0;
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (citb, user, ignore) => 0;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (cibf) => 0;
            ShimBlast.GetByCampaignItemTestBlastIDInt32UserBoolean = (id, user, ignore) =>
            {
                var blast = new Mock<BlastAbstract>();
                blast.Object.BlastScheduleID = 1;
                blast.Object.BlastID = 1;
                return blast.Object;
            };
            ShimBlast.UpdateFilterForAPITestBlastsInt32Int32 = (blastID, filterId) => { };
            ShimBlast.GetBlastEmailsListCountInt32Int32Int32ListOfCampaignItemBlastFilterStringStringBooleanUser =
                (id1, id2, id3, filters, domain, suppression, test, APIUser) => 0;
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (id, user, child) => { };
            ShimBlast.GetByCampaignItemBlastIDInt32UserBoolean = (id, user, ignore) =>
            {
                var blast = new Mock<BlastAbstract>();
                blast.Object.BlastScheduleID = 1;
                blast.Object.BlastID = 1;
                return blast.Object;
            };
        }
    }
}
