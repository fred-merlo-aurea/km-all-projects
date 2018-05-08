using System;
using System.Collections.Generic;
using System.Text;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;
using KMPlatform.Entity;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using FormEntities = ECN_Framework_Entities.FormDesigner;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework_BusinessLayer.Communicator.Interfaces.Fakes;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodSaveLinkTrackingParamOptions = "SaveLinkTrackingParamOptions";
        private const int DummyId = 1;
        private bool _itemSaved;
        private Entities::CampaignItem _campaignItemEntities = new Entities::CampaignItem
        {
            CustomerID = DummyId,
            CampaignItemID = DummyId,
            CampaignItemTemplateID = DummyId
        };
        
        private static readonly object[] linkTrackingParamOptions =
        {
            null,
            new Entities::LinkTrackingParamOption {LTPOID = DummyId},
            new Entities::LinkTrackingParamOption {LTPOID = (-1) * DummyId}
        };

        [Test]
        [TestCaseSource(nameof(linkTrackingParamOptions))]
        public void SaveLinkTrackingParamOptions_AllowCustomerOverrideIsTrue_SaveCampaignItemLinkTracking(
            Entities::LinkTrackingParamOption linkTrackingParamOption)
        {
            // Arrange
            var xmlConfig = new StringBuilder();
            xmlConfig.Append("<Settings><AllowCustomerOverride>True</AllowCustomerOverride>");
            xmlConfig.Append("<Override>True</Override>");
            xmlConfig.Append("</Settings>");
            SetupForSaveLinkTrackingParamOptions(xmlConfig.ToString(), linkTrackingParamOption);

            // Act
            _privateObject.Invoke(MethodSaveLinkTrackingParamOptions, _campaignItemEntities);

            //Assert
            _itemSaved.ShouldBeTrue();
        }

        [Test]
        [TestCaseSource(nameof(linkTrackingParamOptions))]
        public void SaveLinkTrackingParamOptions_AllowCustomerOverrideIsFalse_SaveCampaignItemLinkTracking(
            Entities::LinkTrackingParamOption linkTrackingParamOption)
        {
            // Arrange
            var xmlConfig = new StringBuilder();
            xmlConfig.Append("<Settings><AllowCustomerOverride>True</AllowCustomerOverride>");
            xmlConfig.Append("<Override>False</Override>");
            xmlConfig.Append("</Settings>");
            SetupForSaveLinkTrackingParamOptions(xmlConfig.ToString(), linkTrackingParamOption);

            // Act
            _privateObject.Invoke(MethodSaveLinkTrackingParamOptions, _campaignItemEntities);

            //Assert
            _itemSaved.ShouldBeTrue();
        }

        private void SetupForSaveLinkTrackingParamOptions(
            string xmlConfig, 
            Entities::LinkTrackingParamOption linkTrackingParamOption)
        {
            _itemSaved = false;
            SetupForValidatePublish();
            ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = (_) => GetLinkTrackingParamList();
            var campaignItemTemplate = GetCampaignItemTemplate();
            campaignItemTemplate.Omniture1 = DummyString;
            campaignItemTemplate.Omniture2 = DummyString;
            campaignItemTemplate.Omniture3 = DummyString;
            campaignItemTemplate.Omniture4 = DummyString;
            campaignItemTemplate.Omniture5 = DummyString;
            campaignItemTemplate.Omniture6 = DummyString;
            campaignItemTemplate.Omniture7 = DummyString;
            campaignItemTemplate.Omniture8 = DummyString;
            campaignItemTemplate.Omniture9 = DummyString;
            campaignItemTemplate.Omniture10 = DummyString;
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (_, __, ___) => campaignItemTemplate;
            var linkTrackingSettings = new Entities::LinkTrackingSettings
            {
                XMLConfig = xmlConfig,
                LTSID = DummyId
            };
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (_, __) => linkTrackingSettings;
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (_, __) => linkTrackingSettings;
            ShimLinkTrackingParamOption.GetLTPOIDByCustomerIDInt32StringInt32 = (_, __, ___) => linkTrackingParamOption;
            ShimLinkTrackingParamOption.GetLTPOIDByBaseChannelIDInt32StringInt32 = (_, __, ___) => linkTrackingParamOption;
            ShimCampaignItemLinkTracking.SaveCampaignItemLinkTrackingUser = (_, __) =>
            {
                _itemSaved = true;
                return DummyId;
            };
        }

        private List<Entities::LinkTrackingParam> GetLinkTrackingParamList()
        {
            return new List<Entities::LinkTrackingParam>
            {
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture1)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture2)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture3)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture4)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture5)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture6)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture7)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture8)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture9)
                },
                new Entities::LinkTrackingParam
                {
                    LTPID = DummyId,
                    DisplayName = nameof(Entities::CampaignItemTemplate.Omniture10)
                }
            };
        }
    }
}
