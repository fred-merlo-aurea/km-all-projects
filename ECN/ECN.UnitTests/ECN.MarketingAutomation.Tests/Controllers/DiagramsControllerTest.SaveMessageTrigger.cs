using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using FormEntities = ECN_Framework_Entities.FormDesigner;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string SaveMessageTriggerMethod = "SaveMessageTrigger";

        private bool _isCampaignSaved = false;
        private CommunicatorEntities.Campaign _savedCampaign = null;
        private bool _isCampaignItemSaved = false;
        private CommunicatorEntities.CampaignItem _savedCampaignItem = null;
        private bool _isLinkedTrackingOptionsSaved = false;
        private bool _isCampaignItemOptOutGroupSaved = false;
        private CommunicatorEntities.CampaignItemOptOutGroup _savedCampaignItemOptOutGroup = null;
        private bool _isCampaignItemBlastSaved = false;
        private CommunicatorEntities.CampaignItemBlast _savedCampaignItemBlast = null;
        private bool _isBlastFromCampaignItemCreated = false;
        private bool _isLayoutPlanSaved = false;
        private CommunicatorEntities.LayoutPlans _savedLayoutPlans = null;
        private bool _isBlastDeletedForLayoutPlan = false;
        private bool _isTriggerPlanDeletedForBlast = false;
        private int _deletedBlastLayoutPlanId = 0;
        private int _deletedTriggerPlanId = 0;
        private int _deletedBlastId = 0;
        private bool _isTriggerPlanSaved = false;
        private CommunicatorEntities.TriggerPlans _savedTriggerPlans = null;

        private static object[] ControlTypesTestSource =
        {
           new CampaignItem { ECNID = 1, CampaignItemTemplateID = 1},
           new Click { ECNID = 1, CampaignItemTemplateID = 1},
           new FormAbandon { ECNID = 1, CampaignItemTemplateID = 1},
           new FormSubmit { ECNID = 1, CampaignItemTemplateID = 1},
           new NoClick { ECNID = 1, CampaignItemTemplateID = 1},
           new NoOpen { ECNID = 1, CampaignItemTemplateID = 1},
           new NotSent { ECNID = 1, CampaignItemTemplateID = 1},
           new Open { ECNID = 1, CampaignItemTemplateID = 1},
           new Open_NoClick { ECNID = 1, CampaignItemTemplateID = 1},
           new Sent { ECNID = 1, CampaignItemTemplateID = 1},
           new Subscribe { ECNID = 1, CampaignItemTemplateID = 1},
           new Suppressed { ECNID = 1, CampaignItemTemplateID = 1},
           new Unsubscribe { ECNID = 1, CampaignItemTemplateID = 1},
           new Direct_Click { ECNID = 1, CampaignItemTemplateID = 1},
           new Direct_Open { ECNID = 1, CampaignItemTemplateID = 1},
           new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1},
        };

        [Test]
        [TestCaseSource(nameof(ControlTypesTestSource))]
        public void SaveMessageTrigger_WithDirectClickTrigger_SavesAllRelatedEntities(ControlBase parentControl)
        {
            // Arrange
            SetUpFakes();
            var trigger = new Direct_Click { ECNID = 1, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = parentControl;

            // Act
            _diagramsControllerPrivateObject.Invoke(SaveMessageTriggerMethod, trigger, parentWait, parentCI);

            // Assert
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaign.ShouldNotBeNull();
            _isCampaignItemSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _savedCampaignItemOptOutGroup.ShouldNotBeNull();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _savedCampaignItemBlast.ShouldNotBeNull();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isLayoutPlanSaved.ShouldBeTrue();
            _savedLayoutPlans.ShouldNotBeNull();
            _isBlastDeletedForLayoutPlan.ShouldBeTrue();
            _deletedBlastLayoutPlanId.ShouldBe(1);
        }

        [Test]
        public void SaveMessageTrigger_WithDirectOpenTriggerAndLayoutPlainIdLessThanZero_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakes(defaultLayoutPlanId: 0);
            var trigger = new Direct_Open { ECNID = 0, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = ControlTypesTestSource[0];

            // Act
            _diagramsControllerPrivateObject.Invoke(SaveMessageTriggerMethod, trigger, parentWait, parentCI);

            // Assert
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaign.ShouldNotBeNull();
            _isCampaignItemSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _savedCampaignItemOptOutGroup.ShouldNotBeNull();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _savedCampaignItemBlast.ShouldNotBeNull();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isLayoutPlanSaved.ShouldBeTrue();
            _savedLayoutPlans.ShouldNotBeNull();
            _isBlastDeletedForLayoutPlan.ShouldBeFalse();
            _deletedBlastLayoutPlanId.ShouldBe(0);
        }

        private void SetUpFakes(int defaultLayoutPlanId = 1)
        {
            CleanUpAssignments();
            ShimAutomationBaseController.AllInstances.SaveMessageTriggerControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveFormControlTriggerControlBaseControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveTriggerPlanControlBaseControlBaseControlBaseBoolean = null;
            ShimAutomationBaseController.AllInstances.SaveGroupTriggerControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (c, cb) => new CampaignItem { CustomerID = 1 };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (c, cb) => new Group { CustomerID = 1 };

            // BusinessLayer.Communicator Get Fakes
            ShimLayoutPlans.GetByLayoutPlanID_UseAmbientTransactionInt32User =
                (ecnId, user) => GetLayoutPlans(defaultLayoutPlanId);
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (ecnId, b) => new CommunicatorEntities.CampaignItem { };
            ShimCampaignItem.GetByBlastID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (blastId, b) => GetTestCampaignItem();
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (id, user, b) => GetCampaignItemTemplate();
            ShimCampaign.GetByBlastID_UseAmbientTransactionInt32UserBoolean = (id, user, b) => GetCampaign();
            ShimBlast.GetByCampaignItemBlastID_UseAmbientTransactionInt32UserBoolean =
                (blastId, user, t) => GetBlastAbstract();
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (cid, user) => GetTriggerPlans();
            ShimForm.GetByFormID_NoAccessCheckInt32 = (fid) => new FormEntities.Form { TokenUID = Guid.Empty };

            // BusinessLayer.Communicatot Save Fakes 
            SetUpBusinessCommunicatorSaveFakes();

            // Create Blast Fakes
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean = (id, user, b, t) =>
            {
                _isBlastFromCampaignItemCreated = true;
            };
        }

        private void SetUpBusinessCommunicatorSaveFakes()
        {
            ShimCampaign.Save_UseAmbientTransactionCampaignUser = (campaign, user) =>
            {
                _isCampaignSaved = true;
                _savedCampaign = campaign;
                _savedCampaign.CampaignID = 1;
                return _savedCampaign.CampaignID;
            };
            ShimCampaignItem.Save_UseAmbientTransactionCampaignItemUser = (campaignItem, user) =>
            {
                _isCampaignItemSaved = true;
                _savedCampaignItem = campaignItem;
                _savedCampaignItem.CampaignItemID = 1;
                return _savedCampaignItem.CampaignItemID;
            };
            ShimAutomationBaseController.AllInstances.SaveLinkTrackingParamOptionsCampaignItem = (dc, campaignItem) =>
            {
                _isLinkedTrackingOptionsSaved = true;
            };
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (campItemOptGroup, user) =>
            {
                _isCampaignItemOptOutGroupSaved = true;
                _savedCampaignItemOptOutGroup = campItemOptGroup;
            };
            ShimCampaignItemBlast.Save_UseAmbientTransactionCampaignItemBlastUser = (cpItemBlast, user) =>
            {
                _isCampaignItemBlastSaved = true;
                _savedCampaignItemBlast = cpItemBlast;
                _savedCampaignItemBlast.CampaignItemBlastID = 1;
                return _savedCampaignItemBlast.CampaignItemBlastID;
            };
            ShimLayoutPlans.Save_UseAmbientTransactionLayoutPlansUser = (layoutPlan, user) =>
            {
                _isLayoutPlanSaved = true;
                _savedLayoutPlans = layoutPlan;
                _savedLayoutPlans.LayoutPlanID = 1;
                return _savedLayoutPlans.LayoutPlanID;
            };
            ShimBlastSingle.DeleteForLayoutPlanIDInt32User = (layoutPlanId, user) =>
            {
                _isBlastDeletedForLayoutPlan = true;
                _deletedBlastLayoutPlanId = layoutPlanId;
            };
            ShimBlastSingle.DeleteForTriggerPlanInt32Int32User = (triggerPlanId, blastId, user) =>
            {
                _isTriggerPlanDeletedForBlast = true;
                _deletedTriggerPlanId = triggerPlanId;
                _deletedBlastId = blastId;
            };
            ShimTriggerPlans.Save_UseAmbientTransactionTriggerPlansUser = (triggerPlan, user) =>
            {
                _isTriggerPlanSaved = true;
                _savedTriggerPlans = triggerPlan;
                _savedTriggerPlans.TriggerPlanID = 1;
                return _savedTriggerPlans.TriggerPlanID;
            };
        }

        private CommunicatorEntities.CampaignItemTemplate GetCampaignItemTemplate()
        {
            return new CommunicatorEntities.CampaignItemTemplate
            {
                OptOutMasterSuppression = true,
                OptOutSpecificGroup = true,
                OptoutGroupList = new List<CommunicatorEntities.CampaignItemTemplateOptoutGroup>
                    {
                        new CommunicatorEntities.CampaignItemTemplateOptoutGroup { GroupID = 1 }
                    }
            };
        }

        private CommunicatorEntities.Campaign GetCampaign()
        {
            return new CommunicatorEntities.Campaign
            {
                CustomerID = 1,
                ItemList = new List<CommunicatorEntities.CampaignItem>
                {
                    new CommunicatorEntities.CampaignItem
                    {
                        CampaignItemTemplateID = 1,
                        BlastList = new List<CommunicatorEntities.CampaignItemBlast>
                        {
                            new CommunicatorEntities.CampaignItemBlast { }
                        }
                    }
                },
            };
        }

        private CommunicatorEntities.BlastChampion GetBlastAbstract()
        {
            return new CommunicatorEntities.BlastChampion
            {
                BlastID = 1,
                CustomerID = 1,
            };
        }

        private CommunicatorEntities.LayoutPlans GetLayoutPlans(int defaultLayoutPlanId)
        {
            return new CommunicatorEntities.LayoutPlans
            {
                LayoutPlanID = defaultLayoutPlanId,
                BlastID = 1,
                Status = "Y",
                CustomerID = 1
            };
        }

        private CommunicatorEntities.CampaignItem GetTestCampaignItem()
        {
            return new CommunicatorEntities.CampaignItem
            {
                CampaignItemID = 1,
                BlastList = new List<CommunicatorEntities.CampaignItemBlast>
                    {
                        new CommunicatorEntities.CampaignItemBlast { LayoutID = 1}
                    }
            };
        }

        private CommunicatorEntities.TriggerPlans GetTriggerPlans()
        {
            return new CommunicatorEntities.TriggerPlans
            {
                BlastID = 1,
                TriggerPlanID = 1,
                Status = "Y"
            };
        }

        private void CleanUpAssignments()
        {
            _isCampaignSaved = false;
            _savedCampaign = null;
            _isCampaignItemSaved = false;
            _savedCampaignItem = null;
            _isLinkedTrackingOptionsSaved = false;
            _isCampaignItemOptOutGroupSaved = false;
            _savedCampaignItemOptOutGroup = null;
            _isCampaignItemBlastSaved = false;
            _savedCampaignItemBlast = null;
            _isBlastFromCampaignItemCreated = false;
            _isLayoutPlanSaved = false;
            _savedLayoutPlans = null;
            _isBlastDeletedForLayoutPlan = false;
            _deletedBlastLayoutPlanId = 0;
            _isTriggerPlanDeletedForBlast = false;
            _deletedTriggerPlanId = 0;
            _deletedBlastId = 0;
            _isTriggerPlanSaved = false;
            _savedTriggerPlans = null;
            _isCampaignItemBlastRefBlastDeleted = false;
            _isCampaignItemSuppressionDeleted = false;
            _isCampaignItemSuppressionSaved = false;
        }
    }
}
