using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using ecn.common.classes.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Controllers.Fakes;
using EmailMarketing.API.Models;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;


namespace EmailMarketing.API.Tests.Controllers
{
    /// <summary>
    ///     Unit tests for <see cref="EmailMarketing.API.Controllers.PersonalizationBlastController"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class PersonalizationBlastControllerTest
    {
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

        private PersonalizationBlastController CreateController()
        {
            var testObject = new PersonalizationBlastController();
            testObject.Request = new HttpRequestMessage();
            return testObject;
        }

        private PersonalizationBlast CreateModel()
        {
            return new PersonalizationBlast
            {
                BlastID = 1,
                EmailFrom = "from@km.com",
                ReplyTo = "reply@km.com",
                EmailSubject = System.Text.RegularExpressions.Regex.Unescape(@"Subject\u2000\uD000\uD001"),
                FilterID = 1,
                GroupID = 1,
                CampaignID = 1,
                SendTime = DateTime.Now.AddHours(1),
                LayoutID = 1,
                BlastField1 = "1",
                BlastField2 = "2",
                BlastField3 = "3",
                BlastField4 = "4",
                BlastField5 = "5",
                OptOutGroupID = 1,
                SuppressionGroupID = 1,
                SuppressionGroupFilterID = 1
            };
        }

        private void InitilizeFakes()
        {
            ShimClient.AllInstances.ECN_SelectInt32Boolean = (instance, id, ignore) => new KMPlatform.Entity.Client();
            KMPlatform.BusinessLogic.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, feature, access) => true;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Group
            {
                CustomerID = 1
            };

            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter
            {
                GroupID = 1,
                FilterID = 1,
                CustomerID = 1
            };

            InitilizeGroupLayoutFakes();
            InitilizeHttpFakes();
            InitilizeCampaignFakes();
        }

        private void InitilizeGroupLayoutFakes()
        {
            ShimLayout.ExistsInt32Int32 = (layoutId, customerId) => true;
            ShimContentFilter.HasDynamicContentInt32 = (id) => false;
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) => new Template { };
            ShimGroup.ExistsInt32Int32 = (groupID, customerID) => true;
            ShimFilter.ExistsInt32Int32 = (filterID, customerID) => true;
            ShimLayout.ValidateLayoutContentInt32 = (layoutID) => new List<string>();
            ShimGroup.ValidateDynamicStringsListOfStringInt32User = (listLY, groupID, user) => { };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (templateList, groupID, user) => { };
            ShimSmartSegment.GetNewIDFromOldIDInt32 = (id) => 1;
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (layoutId, child) => new Layout
            {
                ContentSlot1 = 1,
                TemplateID = 1
            };

            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = true };
            ShimLicenseCheck.AllInstances.CurrentString = (instance, id) => "UNLIMITED";
            ShimCampaign.GetByCampaignIDInt32UserBoolean = (campaignId, user, ignore) => new Campaign { CampaignID = 1 };
            ShimCampaign.GetByCampaignNameStringUserBoolean = (id, name, child) => new Campaign { };
            ShimCampaign.SaveCampaignUser = (campaign, user) => 0;
            ShimGroup.ExistsInt32Int32 = (groupId, CustomerId) => true;
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (cioo, user) => { };
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
                (AuthenticatedUserControllerBase instance, HttpStatusCode statusCode, PersonalizationBlast model, int id, string routeName) =>
                {
                    _resultCode = statusCode;
                    _resultRoute = routeName;
                    return null;
                });
        }

        private void InitilizeCampaignFakes()
        {

            ShimBlastSchedule.CreateScheduleFromXMLStringInt32 = (schedule, id) =>
                new BlastSchedule { BlastScheduleID = 1 };
            ShimBlastSetupInfo.GetNextScheduledBlastSetupInfoInt32Boolean = (id, child) => new BlastSetupInfo { BlastScheduleID = 1 };
            ShimCampaignItem.SaveCampaignItemUser = (campaignItem, user) => 0;
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (campaignItemBlast, user, ignore) => 0;
            ShimCampaignItemTestBlast.InsertCampaignItemTestBlastUserBoolean = (citb, user, ignore) => 0;
            ShimCampaignItemBlastFilter.SaveCampaignItemBlastFilter = (cibf) => 0;
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (cis, user) => 0;
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
