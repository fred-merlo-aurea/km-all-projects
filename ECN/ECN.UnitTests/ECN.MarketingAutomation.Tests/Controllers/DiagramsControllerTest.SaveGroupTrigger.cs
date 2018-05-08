using System;
using System.Reflection;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using NUnit.Framework;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string SaveGroupTriggerMethodName = "SaveGroupTrigger";

        [Test]
        public void SaveGroupTrigger_WhenTriggerTypeSubscribeAndParentWait_SavesRelatedEntities()
        {
            // Arrange
            SetUpFakes();
            var trigger = new Subscribe
            {
                ECNID = 1,
                CampaignItemTemplateID = 1,
                IsCancelled = true
            };
            var parentWait = new Wait { };

            // Act
            var savedLayoutPlanId = _diagramsControllerPrivateObject.Invoke(SaveGroupTriggerMethodName, trigger, parentWait);
            
            // Assert
            _savedCampaignItem.ShouldSatisfyAllConditions(
               () => _isCampaignSaved.ShouldBeTrue(),
               () => _savedCampaign.ShouldNotBeNull(),
               () => _isCampaignItemSaved.ShouldBeTrue(),
               () => _savedCampaignItem.ShouldNotBeNull(),
               () => _isLinkedTrackingOptionsSaved.ShouldBeTrue(),
               () => _isCampaignItemOptOutGroupSaved.ShouldBeTrue(),
               () => _savedCampaignItemOptOutGroup.ShouldNotBeNull(),
               () => _isCampaignItemBlastSaved.ShouldBeTrue(),
               () => _savedCampaignItemBlast.ShouldNotBeNull(),
               () => _isBlastFromCampaignItemCreated.ShouldBeTrue(),
               () => _isLayoutPlanSaved.ShouldBeTrue(),
               () => _savedLayoutPlans.ShouldNotBeNull(),
               () => _isBlastDeletedForLayoutPlan.ShouldBeTrue(),
               () => _deletedBlastLayoutPlanId.ShouldBe(1));
        }
        
        [Test]
        public void SaveGroupTrigger_WhenUnSubscribeTriggerAndParentWaitGroup_SavesRelatedEntities()
        {
            // Arrange
            SetUpFakes(defaultLayoutPlanId: 0);
            var trigger = new Unsubscribe
            {
                ECNID = 0,
                CampaignItemTemplateID = 1
            };
            var parentWait = new Group { GroupID = 1 };
            
            // Act
            var exp = Should.NotThrow(() => 
                _diagramsControllerPrivateObject.Invoke(SaveGroupTriggerMethodName, trigger, parentWait));

            // Assert
            exp.ShouldNotBeNull();
            exp.ShouldSatisfyAllConditions(
               () => _isCampaignSaved.ShouldBeTrue(),
               () => _savedCampaign.ShouldNotBeNull(),
               () => _isCampaignItemSaved.ShouldBeTrue(),
               () => _savedCampaignItem.ShouldNotBeNull(),
               () => _isLinkedTrackingOptionsSaved.ShouldBeTrue(),
               () => _isCampaignItemOptOutGroupSaved.ShouldBeTrue(),
               () => _savedCampaignItemOptOutGroup.ShouldNotBeNull(),
               () => _isCampaignItemBlastSaved.ShouldBeTrue(),
               () => _savedCampaignItemBlast.ShouldNotBeNull(),
               () => _isBlastFromCampaignItemCreated.ShouldBeTrue(),
               () => _isLayoutPlanSaved.ShouldBeTrue(),
               () => _savedLayoutPlans.ShouldNotBeNull(),
               () => _isBlastDeletedForLayoutPlan.ShouldBeFalse(),
               () => _deletedBlastLayoutPlanId.ShouldBe(0));
        }
    }
}
