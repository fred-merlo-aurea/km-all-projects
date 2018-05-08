using System;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Interfaces.Fakes;
using ECNCommunicator = ECN_Framework_Entities.Communicator;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ecn.MarketingAutomation.Controllers.Fakes;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
		private CampaignItem GetCampaignItem()
        {
            return new CampaignItem
            {
                ControlType = MarketingAutomationControlType.CampaignItem,
                Text = "SampleText",
                CreateCampaignItem = false,
                SendTime = DateTime.Now.AddDays(7),
                CampaignItemName = "SampleCampignItem",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Click ConfigureFakesAndGetClickControl()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Click
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Direct_Click ConfigureFakesAndGetDirectClick(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new Direct_Click
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Form ConfigureFakesAndGetForm()
        {
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (f, cb) => GetCampaignItem();
            return new Form
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private FormSubmit ConfigureFakesAndGetFormSubmit(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new FormSubmit
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private FormAbandon ConfigureFakesAndGetFormAbandon(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new FormAbandon
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Direct_Open ConfigureFakesAndGetDirectOpen(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new Direct_Open
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Direct_NoOpen ConfigureFakesAndGetDirectNoOpen(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new Direct_NoOpen
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private End ConfigureFakesAndGetEnd(bool isWaitNull = false)
        {
            return new End
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Group ConfigureFakesAndGetGroup()
        {
            return new Group
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private NoClick ConfigureFakesAndGetNoClick()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new NoClick 
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private NoOpen ConfigureFakesAndGetNoOpen()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new NoOpen
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private NotSent ConfigureFakesAndGetNotSent()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new NotSent
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Open ConfigureFakesAndGetOpen()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Open
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Open_NoClick ConfigureFakesAndGetOpenNoClick()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Open_NoClick
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Sent ConfigureFakesAndGetSent()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Sent
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Start ConfigureFakesAndGetStart()
        {
            return new Start
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Subscribe ConfigureFakesAndGetSubscribe(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, cb) => new Group();
            return new Subscribe
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Suppressed ConfigureFakesAndGetSuppressed()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Suppressed
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Unsubscribe ConfigureFakesAndGetUnsubscribe(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, cb) => new Group();
            return new Unsubscribe
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Wait ConfigureFakesAndGetWait()
        {
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (d, cb) => GetCampaignItem();
            return new Wait
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private StubICampaignItemManager GetStubICampaignItemManager()
        {
            return new StubICampaignItemManager()
            {
                GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, children) => new ECNCommunicator.CampaignItem
                {
                    CampaignItemType = ControlConsts.CampaignItemTypeChampion.ToLower()
                },
                GetByCampaignItemIDInt32UserBoolean = (id, user, children) => null
            };
        }

        private ECNCommunicator.MarketingAutomation GetMarketingAutomationObj()
        {
           return new ECNCommunicator.MarketingAutomation
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3)
            };
        }
    }
}
