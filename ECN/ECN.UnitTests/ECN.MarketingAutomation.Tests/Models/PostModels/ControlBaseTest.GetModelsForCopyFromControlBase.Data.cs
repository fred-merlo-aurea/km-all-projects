using System.Collections.Generic;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using CommincatorEntities = ECN_Framework_Entities.Communicator;
using ModelControls = ecn.MarketingAutomation.Models.PostModels.Controls;

namespace ECN.MarketinAutomation.Tests.Models.PostModels
{
    public partial class ControlBaseTest
    {
        private ModelControls.CampaignItem ConfigureFakesAndGetCampaignItem()
        {
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (i, c) => new CommincatorEntities.CampaignItem
            {
                CampaignID = 1,
                BlastList = new List<CampaignItemBlast>
                {
                    new CampaignItemBlast
                    {
                        Blast = new BlastChampion
                        {
                            BlastID = 1,
                            LayoutID = 1,
                        },
                        Filters = new List<CampaignItemBlastFilter>
                        {
                            new CampaignItemBlastFilter { FilterID = 1 }
                        },
                        LayoutID = 1,
                        CampaignItemID = 1,
                        GroupID = 1
                    },
                },
                SuppressionList = new List<CampaignItemSuppression>
                {
                    new CampaignItemSuppression
                    {
                       GroupID = 1,
                       Filters = new List<CampaignItemBlastFilter>
                       {
                           new CampaignItemBlastFilter { FilterID = 1 }
                       }
                    },
                }
             };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (i, c) => new Layout { LayoutName = "SampleLayout" };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (i) => new Group() { FolderID = 1 };
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (i) => new Filter { CustomerID = 1, FilterID = 1, GroupID = 1};
            ShimCampaign.GetByCampaignID_NoAccessCheckInt32Boolean = (c, b) => new Campaign { CampaignName = "SampleCampaign", CampaignID = 1 };
            return new ModelControls.CampaignItem
            {
                ControlID = "1",
                ECNID = 1,
                MessageID = 1,
                CampaignItemName = "SampleCampaignItem",
                editable = new shapeEditable()
            };
        }

        private ModelControls.Click ConfigureFakesAndGetClick()
        {
            return new ModelControls.Click
            {
                CampaignItemName = "SampleItemClick",
                editable = new shapeEditable()
            };
        }

        private ModelControls.Direct_Click ConfigureFakesAndGetDirectClick()
        {
            return new ModelControls.Direct_Click
            {
                CampaignItemName = "SampleItemDirectClick",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.Form ConfigureFakesAndGetForm()
        {
            return new ModelControls.Form
            {
                FormName = "SampleItemForm",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.FormAbandon ConfigureFakesAndGetFormAbandon()
        {
            return new ModelControls.FormAbandon
            {
                CampaignItemName = "SampleItemFormAbandon",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.FormSubmit ConfigureFakesAndGetFormSubmit()
        {
            return new ModelControls.FormSubmit
            {
                CampaignItemName = "SampleItemFormSubmit",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.Direct_Open ConfigureFakesAndGetDirectOpen()
        {
            return new ModelControls.Direct_Open
            {
                CampaignItemName = "SampleItemDirectOpen",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.Direct_NoOpen ConfigureFakesAndGetDirectNoOpen()
        {
            return new ModelControls.Direct_NoOpen
            {
                CampaignItemName = "SampleItemDirectNoOpen",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.End ConfigureFakesAndGetEnd()
        {
            return new ModelControls.End
            {
                Text = "SampleItemEnd",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Group ConfigureFakesAndGetGroup()
        {
            return new ModelControls.Group
            {
                GroupName = "SampleItemGroup",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.NoClick ConfigureFakesAndGetNoClick()
        {
            return new ModelControls.NoClick
            {
                CampaignItemName = "SampleItemNoClick",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.NoOpen ConfigureFakesAndGetNoOpen()
        {
            return new ModelControls.NoOpen
            {
                CampaignItemName = "SampleItemNoOpen",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.NotSent ConfigureFakesAndGetNotSent()
        {
            return new ModelControls.NotSent
            {
                CampaignItemName = "SampleItemNotSent",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Open ConfigureFakesAndGetOpen()
        {
            return new ModelControls.Open
            {
                CampaignItemName = "SampleItemOpen",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Open_NoClick ConfigureFakesAndGetOpenNoClick()
        {
            return new ModelControls.Open_NoClick
            {
                CampaignItemName = "SampleItemOpenNoClick",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Sent ConfigureFakesAndGetSent()
        {
            return new ModelControls.Sent
            {
                CampaignItemName = "SampleItemSent",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Start ConfigureFakesAndGetStart()
        {
            return new ModelControls.Start
            {
                Text = "SampleItemStart",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Subscribe ConfigureFakesAndGetSubscribe()
        {
            return new ModelControls.Subscribe
            {
                CampaignItemName = "SampleItemSubscribe",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.Suppressed ConfigureFakesAndGetSuppressed()
        {
            return new ModelControls.Suppressed
            {
                CampaignItemName = "SampleItemSuppressed",
                editable = new shapeEditable(),
            };
        }

        private ModelControls.Unsubscribe ConfigureFakesAndGetUnsubscribe()
        {
            return new ModelControls.Unsubscribe
            {
                CampaignItemName = "SampleItemUnsubscribe",
                editable = new shapeEditable(),
                IsCancelled = true
            };
        }

        private ModelControls.Wait ConfigureFakesAndGetWait()
        {
            return new ModelControls.Wait
            {
                Text = "SampleItemWait",
                editable = new shapeEditable(),
            };
        }
    }
}
