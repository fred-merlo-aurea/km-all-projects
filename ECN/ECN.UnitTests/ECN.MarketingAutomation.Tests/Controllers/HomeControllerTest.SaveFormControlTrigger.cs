using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using ECN_Framework_Common.Objects.Communicator;
using Entities = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodSaveFormControlTrigger = "SaveFormControlTrigger";
        private bool _isCampaignSaved;
        private Entities::Campaign _savedCampaign;
        private bool _isCampaignItemSaved;
        private Entities::CampaignItem _savedCampaignItem;
        private bool _isLinkedTrackingOptionsSaved;
        private bool _isCampaignItemOptOutGroupSaved;
        private Entities::CampaignItemOptOutGroup _savedCampaignItemOptOutGroup;
        private bool _isCampaignItemBlastSaved;
        private Entities::CampaignItemBlast _savedCampaignItemBlast;
        private bool _isBlastFromCampaignItemCreated;
        private bool _isLayoutPlanSaved;
        private Entities::LayoutPlans _savedLayoutPlans;
        private bool _isBlastDeletedForLayoutPlan;
        private bool _isTriggerPlanDeletedForBlast;
        private int _deletedBlastLayoutPlanId;
        private int _deletedTriggerPlanId;
        private int _deletedBlastId;
        private bool _isTriggerPlanSaved;
        private Entities::TriggerPlans _savedTriggerPlans;
        private bool _isCampaignItemBlastRefBlastDeleted;
        private bool _isCampaignItemSuppressionDeleted;
        private bool _isCampaignItemSuppressionSaved;

        private static readonly object[] ControlTypesTestSource =
        {
            new CampaignItem { ECNID = 1, CampaignItemTemplateID = 1},
            new CampaignItem {
                ECNID = 1,
                CampaignItemTemplateID = 1,
                ControlType = Enums.MarketingAutomationControlType.Form
            },
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
        public void SaveFormControlTrigger_MultipleCases_SavesAllRelatedEntities(ControlBase parentControl)
        {
            // Arrange
            SetupForValidatePublish();
            SetUpFakesForSaveFormControlTrigger();
            var trigger = new FormSubmit { ECNID = 1, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = parentControl;
            var parentForm = new Form { FormID = 1 };

            // Act
            _privateObject.Invoke(MethodSaveFormControlTrigger, trigger, parentWait, parentCI, parentForm);

            // Assert
            _isCampaignSaved.ShouldSatisfyAllConditions(
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
                () => _deletedBlastLayoutPlanId.ShouldBe(1)
            );
        }

        [Test]
        public void SaveFormControlTrigger_WithFormAbandonTriggerAndLayoutPlainIdLessThanZero_SavesAllRelatedEntities()
        {
            // Arrange
            SetupForValidatePublish();
            SetUpFakesForSaveFormControlTrigger(defaultLayoutPlanId: 0);
            var trigger = new FormAbandon { ECNID = 0, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = ControlTypesTestSource[0];
            var parentForm = new Form { FormID = 1 };

            // Act
            _privateObject.Invoke(MethodSaveFormControlTrigger, trigger, parentWait, parentCI, parentForm);

            // Assert
            _isCampaignSaved.ShouldSatisfyAllConditions(
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
                () => _deletedBlastLayoutPlanId.ShouldBe(0)
            );
        }

        private void SetUpFakesForSaveFormControlTrigger(int defaultLayoutPlanId = 1)
        {
            CleanUpAssignments();
            SetUpBusinessCommunicatorSaveFakes();
            ShimAutomationBaseController.AllInstances.SaveMessageTriggerControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveFormControlTriggerControlBaseControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveTriggerPlanControlBaseControlBaseControlBaseBoolean = null;
            ShimAutomationBaseController.AllInstances.SaveGroupTriggerControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (c, cb) => new CampaignItem { CustomerID = 1 };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (c, cb) => new Group { CustomerID = 1 };

            ShimLayoutPlans.GetByLayoutPlanID_UseAmbientTransactionInt32User =
                (ecnId, user) => GetLayoutPlans(defaultLayoutPlanId);
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (ecnId, b) => new Entities::CampaignItem { };
            ShimCampaignItem.GetByBlastID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (blastId, b) => GetTestCampaignItem();
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (id, user, b) => GetCampaignItemTemplate();
            ShimCampaign.GetByBlastID_UseAmbientTransactionInt32UserBoolean = (id, user, b) => GetCampaign();
            ShimBlast.GetByCampaignItemBlastID_UseAmbientTransactionInt32UserBoolean =
                (blastId, user, t) => GetBlastAbstract();
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (cid, user) => GetTriggerPlans();
            ShimForm.GetByFormID_NoAccessCheckInt32 = (fid) => new ECN_Framework_Entities.FormDesigner.Form { TokenUID = Guid.Empty };
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean = (id, user, b, t) =>
            {
                _isBlastFromCampaignItemCreated = true;
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
        private Entities::BlastChampion GetBlastAbstract()
        {
            return new Entities::BlastChampion
            {
                BlastID = 1,
                CustomerID = 1,
            };
        }

        private Entities::LayoutPlans GetLayoutPlans(int defaultLayoutPlanId)
        {
            return new Entities::LayoutPlans
            {
                LayoutPlanID = defaultLayoutPlanId,
                BlastID = 1,
                Status = "Y",
                CustomerID = 1
            };
        }

        private Entities::CampaignItem GetTestCampaignItem()
        {
            return new Entities::CampaignItem
            {
                CampaignItemID = 1,
                BlastList = new List<Entities::CampaignItemBlast>
                {
                    new Entities::CampaignItemBlast { LayoutID = 1}
                }
            };
        }

        private Entities::TriggerPlans GetTriggerPlans()
        {
            return new Entities::TriggerPlans
            {
                BlastID = 1,
                TriggerPlanID = 1,
                Status = "Y"
            };
        }

        private Entities::CampaignItemTemplate GetCampaignItemTemplate()
        {
            return new Entities::CampaignItemTemplate
            {
                OptOutMasterSuppression = true,
                OptOutSpecificGroup = true,
                OptoutGroupList = new List<Entities::CampaignItemTemplateOptoutGroup>
                {
                    new Entities::CampaignItemTemplateOptoutGroup { GroupID = 1 }
                }
            };
        }

        private Entities::Campaign GetCampaign()
        {
            return new Entities::Campaign
            {
                CustomerID = 1,
                ItemList = new List<Entities::CampaignItem>
                {
                    new Entities::CampaignItem
                    {
                        CampaignItemTemplateID = 1,
                        BlastList = new List<Entities::CampaignItemBlast>
                        {
                            new Entities::CampaignItemBlast { }
                        }
                    }
                },
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
    }
}
