using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using Shouldly;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string SampleBlast1 = "SampleBlast1";
        private const string SampleBlast2 = "SampleBlast2";
        private const string SampleBlast3 = "SampleBlast3";
        private const string SampleBlast4 = "SampleBlast4";
        private const string SampleBlast5 = "SampleBlast5";
        private const string SampleEmail = "test@sample.com";
        private const string SampleUserName = "SampleUserName";
        private const string SaveCampaignItemMethodName = "SaveCampaignItem";
        private const string SampleCampaignItemName = "SampleCampaignItemName";
        private const string ItemTypeRegular = "Regular";
        private const string ItemFormatTypeHtml = "HTML";

        private bool _isCampaignItemBlastRefBlastDeleted = false;
        private bool _isCampaignItemSuppressionDeleted = false;
        private bool _isCampaignItemSuppressionSaved = false;
        
        [Test]
        public void SaveCampaignItem_WhenExistingCampaign_SavesRelatedEntities()
        {
            // Arrange
            ConfigureSaveCampaignItemFakes();
            var campaignItem = GetCampaignItemObject();
            var isPaused = false;

            // Act
            var savedCampaignItemId = _diagramsControllerPrivateObject.Invoke(SaveCampaignItemMethodName, campaignItem, isPaused);

            // Assert
            savedCampaignItemId.ShouldBe(1);
            _isCampaignItemBlastRefBlastDeleted.ShouldBeTrue();
            _isCampaignItemSaved.ShouldBeTrue();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _isCampaignItemSuppressionDeleted.ShouldBeTrue();
            _isCampaignItemSuppressionSaved.ShouldBeTrue();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isCampaignSaved.ShouldBeFalse();
            _savedCampaignItem.ShouldNotBeNull();
            VerifySavedCampaignItem(_savedCampaignItem, 1);
        }

        [Test]
        public void SaveCampaignItem_WhenNonExistingCampaign_SavesRelatedEntities()
        {
            // Arrange
            ConfigureSaveCampaignItemFakes();
            var campaignItem = GetCampaignItemObject();
            campaignItem.CampaignID = 0;
            campaignItem.CampaignItemTemplateID = 0;
            campaignItem.SubCategory = string.Empty;
            var isPaused = false;

            // Act
            var savedCampaignItemId = _diagramsControllerPrivateObject.Invoke(SaveCampaignItemMethodName, campaignItem, isPaused);

            // Assert
            savedCampaignItemId.ShouldBe(1);
            _isCampaignItemBlastRefBlastDeleted.ShouldBeTrue();
            _isCampaignItemSaved.ShouldBeTrue();
            _isLinkedTrackingOptionsSaved.ShouldBeFalse();
            _isCampaignItemOptOutGroupSaved.ShouldBeFalse();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _isCampaignItemSuppressionDeleted.ShouldBeTrue();
            _isCampaignItemSuppressionSaved.ShouldBeTrue();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            VerifySavedCampaignItem(_savedCampaignItem, -1);
        }

        private void ConfigureSaveCampaignItemFakes()
        {
            CleanUpAssignments();
            ShimAutomationBaseController.AllInstances.SaveCampaignItemCampaignItemBoolean = null;
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (dc, c) => new Wait
            {
                Days = 1,
                Hours = 0,
                Minutes = 0
            };
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (dc, c) => new CampaignItem
            {
                SendTime = DateTime.UtcNow
            };
            ShimAutomationBaseController.AllInstances.AttachGroupsCampaignItemCampaignItemRef =
                (AutomationBaseController dc, CampaignItem ciObject, ref CommunicatorEntities.CampaignItem ci) =>
            {
                ci.BlastList = new List<CommunicatorEntities.CampaignItemBlast>
                {
                    new CommunicatorEntities.CampaignItemBlast{ }
                };
                ci.SuppressionList = new List<CommunicatorEntities.CampaignItemSuppression>
                {
                    new CommunicatorEntities.CampaignItemSuppression{ }
                };
            };
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (id, user, b) => new CommunicatorEntities.CampaignItemTemplate
                {
                    OptOutSpecificGroup = true,
                    OptOutMasterSuppression = true,
                    OptoutGroupList = new List<CommunicatorEntities.CampaignItemTemplateOptoutGroup>
                    {
                        new CommunicatorEntities.CampaignItemTemplateOptoutGroup { GroupID = 1 }
                    }
                };
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, b) => new CommunicatorEntities.CampaignItem { };

            // Saving Fakes
            ShimCampaignItem.Save_UseAmbientTransactionCampaignItemUser = (cp, user) =>
            {
                _isCampaignItemSaved = true;
                _savedCampaignItem = cp;
                _savedCampaignItem.CampaignItemID = 1;
                return _savedCampaignItem.CampaignItemID;
            };
            ShimAutomationBaseController.AllInstances.SaveLinkTrackingParamOptionsCampaignItem = (dc, ci) => { _isLinkedTrackingOptionsSaved = true; };
            ShimAutomationBaseController.AllInstances.SaveCampaignStringInt32 = (dc, n, i) =>
            {
                _isCampaignSaved = true;
                return 1;
            };
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (id, user, b) => { _isCampaignItemBlastRefBlastDeleted = true; };
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (optGroup, user) => { _isCampaignItemOptOutGroupSaved = true; };
            ShimCampaignItemBlast.Save_UseAmbientTransactionInt32ListOfCampaignItemBlastUser = 
                (id, blastList, user) => { _isCampaignItemBlastSaved = true; };
            ShimCampaignItemSuppression.Delete_NoAccessCheckInt32UserBoolean = (id, user, b) => { _isCampaignItemSuppressionDeleted = true; };
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (cis, user) =>
            {
                _isCampaignItemSuppressionSaved = true;
                cis.CampaignItemSuppressionID = 1;
                return cis.CampaignItemSuppressionID;
            };
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean =
                (id, user, b, p) => { _isBlastFromCampaignItemCreated = true; };
        }

        private CampaignItem GetCampaignItemObject()
        {
            return new CampaignItem
            {
                CreateCampaignItem = true,
                CampaignItemID = 1,
                CampaignID = 1,
                CampaignItemName = SampleCampaignItemName,
                CustomerID = 1,
                FromEmail = SampleEmail,
                FromName = SampleUserName,
                CampaignItemTemplateID = 1,
                BlastField1 = SampleBlast1,
                BlastField2 = SampleBlast2,
                BlastField3 = SampleBlast3,
                BlastField4 = SampleBlast4,
                BlastField5 = SampleBlast5,
                SubCategory = GroupSubCategory,
                SelectedGroups = new List<GroupSelect>
                {
                    new GroupSelect { GroupID = 1 }
                },
                SelectedGroupFilters = new List<FilterSelect>
                {
                    new FilterSelect { GroupID = 1 }
                },
                SuppressedGroups = new List<GroupSelect>
                {
                    new GroupSelect { GroupID = 1 }
                },
                SuppressedGroupFilters = new List<FilterSelect>
                {
                    new FilterSelect { GroupID = 1 }
                }
            };
        }

        private void VerifySavedCampaignItem(CommunicatorEntities.CampaignItem item, int templateId)
        {
            item.ShouldSatisfyAllConditions(
              () => item.CampaignItemID.ShouldBe(1),
              () => item.CampaignItemTemplateID.ShouldBe(templateId),
              () => item.FromEmail.ShouldBe(SampleEmail),
              () => item.CompletedStep.ShouldBe(5),
              () => item.CampaignItemType.ShouldBe(ItemTypeRegular),
              () => item.CampaignItemFormatType.ShouldBe(ItemFormatTypeHtml),
              () => item.FromName.ShouldBe(SampleUserName),
              () => item.CampaignItemName.ShouldBe(SampleCampaignItemName),
              () => item.CampaignItemNameOriginal.ShouldBe(SampleCampaignItemName),
              () => item.CampaignID.ShouldBe(1),
              () => item.CustomerID.ShouldBe(1),
              () => item.BlastField1.ShouldBe(SampleBlast1),
              () => item.BlastField2.ShouldBe(SampleBlast2),
              () => item.BlastField3.ShouldBe(SampleBlast3),
              () => item.BlastField4.ShouldBe(SampleBlast4),
              () => item.BlastField5.ShouldBe(SampleBlast5),
              () => item.BlastList.ShouldNotBeEmpty(),
              () => item.BlastList.Count.ShouldBe(1),
              () => item.SuppressionList.ShouldNotBeEmpty(),
              () => item.SuppressionList.Count.ShouldBe(1));
        }
    }
}
