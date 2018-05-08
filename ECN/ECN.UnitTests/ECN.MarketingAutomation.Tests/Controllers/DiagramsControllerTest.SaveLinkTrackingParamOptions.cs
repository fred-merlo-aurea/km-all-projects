using System;
using System.Collections.Generic;
using System.Reflection;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    [TestFixture]
    public partial class DiagramsControllerTest
    {
        private const string SaveLinkTrackingParamOptions = "SaveLinkTrackingParamOptions";
        private CampaignItemLinkTracking _saveCampaignItemLinkTrackingObject;

        [Test]
        [TestCase("Omniture1", 1, true, true), TestCase("Omniture1", 1, false, true), TestCase("Omniture1", -1, false, true)]
        [TestCase("Omniture2", 1, true, true), TestCase("Omniture2", 1, false, true), TestCase("Omniture2", -1, false, true)]
        [TestCase("Omniture3", 1, true, true), TestCase("Omniture3", 1, false, true), TestCase("Omniture3", -1, false, true)]
        [TestCase("Omniture4", 1, true, true), TestCase("Omniture4", 1, false, true), TestCase("Omniture4", -1, false, true)]
        [TestCase("Omniture5", 1, true, true), TestCase("Omniture5", 1, false, true), TestCase("Omniture5", -1, false, true)]
        [TestCase("Omniture6", 1, true, true), TestCase("Omniture6", 1, false, true), TestCase("Omniture6", -1, false, true)]
        [TestCase("Omniture7", 1, true, true), TestCase("Omniture7", 1, false, true), TestCase("Omniture7", -1, false, true)]
        [TestCase("Omniture8", 1, true, true), TestCase("Omniture8", 1, false, true), TestCase("Omniture8", -1, false, true)]
        [TestCase("Omniture9", 1, true, true), TestCase("Omniture9", 1, false, true), TestCase("Omniture9", -1, false, true)]
        [TestCase("Omniture10", 1, true, true), TestCase("Omniture10", 1, false, true), TestCase("Omniture10", -1, false, true)]
        [TestCase("Omniture1", 1, true, false), TestCase("Omniture1", 1, false, false), TestCase("Omniture1", -1, false, false)]
        [TestCase("Omniture2", 1, true, false), TestCase("Omniture2", 1, false, false), TestCase("Omniture2", -1, false, false)]
        [TestCase("Omniture3", 1, true, false), TestCase("Omniture3", 1, false, false), TestCase("Omniture3", -1, false, false)]
        [TestCase("Omniture4", 1, true, false), TestCase("Omniture4", 1, false, false), TestCase("Omniture4", -1, false, false)]
        [TestCase("Omniture5", 1, true, false), TestCase("Omniture5", 1, false, false), TestCase("Omniture5", -1, false, false)]
        [TestCase("Omniture6", 1, true, false), TestCase("Omniture6", 1, false, false), TestCase("Omniture6", -1, false, false)]
        [TestCase("Omniture7", 1, true, false), TestCase("Omniture7", 1, false, false), TestCase("Omniture7", -1, false, false)]
        [TestCase("Omniture8", 1, true, false), TestCase("Omniture8", 1, false, false), TestCase("Omniture8", -1, false, false)]
        [TestCase("Omniture9", 1, true, false), TestCase("Omniture9", 1, false, false), TestCase("Omniture9", -1, false, false)]
        [TestCase("Omniture10", 1, true, false), TestCase("Omniture10", 1, false, false), TestCase("Omniture10", -1, false, false)]
        public void SaveLinkTrackingParamOptions_CampignItemWithDisplayNameAndLTPOID_ShouldSaveItem(string displayName, int ltpoid, bool isNullLTPO, bool allowCustOverride)
        {
            // Arrange
            CampaignItem campaignItem;
            CampaignItemLinkTracking expectedSaveCampaignItemLinkTrackingObject;
            SetupForSaveLinkTrackingParamOptions(displayName, ltpoid, isNullLTPO, allowCustOverride, out campaignItem, out expectedSaveCampaignItemLinkTrackingObject);

            // Act
            _diagramsControllerPrivateObject.Invoke(SaveLinkTrackingParamOptions, campaignItem);

            // Assert
            _saveCampaignItemLinkTrackingObject.ShouldSatisfyAllConditions(
                () => _saveCampaignItemLinkTrackingObject.CampaignItemID.ShouldBe(expectedSaveCampaignItemLinkTrackingObject.CampaignItemID),
                () => _saveCampaignItemLinkTrackingObject.CustomValue.ShouldBe(expectedSaveCampaignItemLinkTrackingObject.CustomValue),
                () => _saveCampaignItemLinkTrackingObject.LTPID.ShouldBe(expectedSaveCampaignItemLinkTrackingObject.LTPID),
                () => _saveCampaignItemLinkTrackingObject.LTPOID.ShouldBe(expectedSaveCampaignItemLinkTrackingObject.LTPOID)
            );
        }

        private void SetupForSaveLinkTrackingParamOptions(string displayName, int ltpoid, bool isNullLTPO, bool allowCustOverride, out CampaignItem campaignItem, out CampaignItemLinkTracking expectedSaveCampaignItemLinkTrackingObject)
        {
            var defaultId = 1;

            expectedSaveCampaignItemLinkTrackingObject = new CampaignItemLinkTracking
            {
                CustomValue = string.Empty,
                CampaignItemID = defaultId,
                LTPOID = ltpoid,

                LTPID = defaultId
            };
            if (isNullLTPO)
            {
                expectedSaveCampaignItemLinkTrackingObject.LTPOID = -1;
            }

            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (LTID) =>
            {
                return new List<LinkTrackingParam>()
                    {
                        new LinkTrackingParam
                        {
                              DisplayName = displayName,
                              LTID = LTID,
                              LTPID = defaultId
                        }
                    };
            };

            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (CampaignItemTemplateID, user, getChildren) =>
                {
                    var campaingItemTemplate = new CampaignItemTemplate()
                    {
                        CampaignItemTemplateID = CampaignItemTemplateID
                    };
                    campaingItemTemplate.GetType().InvokeMember(displayName,
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                Type.DefaultBinder, campaingItemTemplate, new object[] { "TestValue" });

                    return campaingItemTemplate;
                };

            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (BaseChannelID, LTID) =>
            {
                return new LinkTrackingSettings
                {
                    LTSID = BaseChannelID,
                    XMLConfig = $"<Settings><AllowCustomerOverride>{(allowCustOverride ? "true" : "false")}</AllowCustomerOverride></Settings>"
                };
            };

            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (CustomerID, LTID) =>
            {
                return new LinkTrackingSettings
                {
                    LTSID = CustomerID,
                    XMLConfig = "<Settings><Override>true</Override></Settings>"
                };
            };
            ShimLinkTrackingParamOption.GetLTPOIDByCustomerIDInt32StringInt32 = (LTPID, Value, CustomerID) =>
            {
                if (isNullLTPO)
                {
                    return null;
                }

                return new LinkTrackingParamOption
                {
                    LTPOID = ltpoid,
                    Value = Value,
                    CustomerID = CustomerID
                };
            };
            ShimLinkTrackingParamOption.GetLTPOIDByBaseChannelIDInt32StringInt32 = (LTPID, Value, BaseChannelID) =>
            {
                if (isNullLTPO)
                {
                    return null;
                }

                return new LinkTrackingParamOption
                {
                    LTPOID = ltpoid,
                    Value = Value,
                    BaseChannelID = BaseChannelID
                };
            };
            ShimCampaignItemLinkTracking.SaveCampaignItemLinkTrackingUser = (campaignItemLinkTracking, user) =>
            {
                _saveCampaignItemLinkTrackingObject = campaignItemLinkTracking;
                return defaultId;
            };

            campaignItem = new CampaignItem
            {
                CustomerID = defaultId,
                CampaignItemID = defaultId
            };
            campaignItem.CampaignItemTemplateID = defaultId;

            ShimECNSession shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new User()
            {
                UserID = defaultId
            };
            shimECNSession.Instance.CurrentBaseChannel = new ECN_Framework_Entities.Accounts.BaseChannel
            {
                BaseChannelID = defaultId
            };
            ShimECNSession.CurrentSession = () => shimECNSession.Instance;
        }
    }
}
