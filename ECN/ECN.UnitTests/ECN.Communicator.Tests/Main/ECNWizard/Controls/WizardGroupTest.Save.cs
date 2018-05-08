using System.Collections.Generic;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Group.Fakes;
using ecn.communicator.main.ECNWizard.Group;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using NUnit.Framework;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardGroupTest
    {
        private const string PhErrorControl = "phError";
        private const string LblErrorMessageControl = "lblErrorMessage";
        private const string PrePopLayoutIDKey = "PrePopLayoutID";
        private const string SampleDynamicName = "SampleDynamicName";
        private const string SampleName = "SampleName";
        private const string SampleGroupName = "SampleGroupName";

        private bool _isCampaignItemSuppressionDeleted;
        private bool _isBlastsFromCampaignItemCreated;
        private CommunicatorEntities.CampaignItem _savedCampaignItem;
        private List<CommunicatorEntities.CampaignItemBlast> _savedCampaignItemBlastList;
        private List<CommunicatorEntities.CampaignItemSuppression> _savedCampaignItemSuppressionList;
        private List<int> _deletedCampaignItemBlastIDs;
        
        [Test]
        public void Save_WithNoSelectedGroups_SetErrorLabel()
        {
            // Arrange
            SetFakesForSaveMethod();
            ShimgroupExplorer.AllInstances.getSelectedGroups = (g) => new List<GroupObject>();

            // Act
            var isSaved = _testEntity.Save();

            // Assert
            isSaved.ShouldBeFalse();
            Get<PlaceHolder>(_privateGroupExplorerObj, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateGroupExplorerObj, LblErrorMessageControl).Text.ShouldContain($"Group not selected");
        }

        [Test]
        public void Save_WithNoSelectedGroupsAndCampaignItemTypeAB_SetErrorLabel()
        {
            // Arrange
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (cid, b) => new CommunicatorEntities.CampaignItem
            {
                CampaignItemType = "ab"
            };
            ShimgroupExplorer.AllInstances.getSelectedGroups = (g) => new List<GroupObject>
            {
                new GroupObject { GroupID = 1 },
                new GroupObject { GroupID = 2 }
            };

            // Act
            var isSaved = _testEntity.Save();

            // Assert
            isSaved.ShouldBeFalse();
            Get<PlaceHolder>(_privateGroupExplorerObj, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateGroupExplorerObj, LblErrorMessageControl).
                Text.ShouldContain($"Multiple Group Selections are not supported for A/B Blast");
        }

        [Test]
        public void Save_WithInvalidBlastRefId_SetErrorLabel()
        {
            // Arrange
            SetFakesForSaveMethod();
            var groupList = GetGroupList();
            groupList[0].filters[0].RefBlastIDs = "1,2,3,4,5,6";
            ShimgroupExplorer.AllInstances.getSelectedGroups = (g) => groupList;

            // Act
            var isSaved = _testEntity.Save();

            // Assert
            isSaved.ShouldBeFalse();
            Get<PlaceHolder>(_privateGroupExplorerObj, PhErrorControl).Visible.ShouldBeTrue();
            Get<Label>(_privateGroupExplorerObj, LblErrorMessageControl).
                Text.ShouldContain($"You cannot have more than 5 Ref Blasts for Smart Segment Filter");
        }

        [Test]
        public void Save_WithCIBlastListEmpty_SavesRelatedEntities()
        {
            // Arrange
            SetFakesForSaveMethod();
            var campaignItem = GetCampaignItem();
            campaignItem.BlastList = new List<CommunicatorEntities.CampaignItemBlast>();
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (cid, b) => campaignItem;

            // Act
            var isSaved = _testEntity.Save();

            // Assert
            isSaved.ShouldBeTrue();
            _isCampaignItemSuppressionDeleted.ShouldBeTrue();
            _isBlastsFromCampaignItemCreated.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _deletedCampaignItemBlastIDs.ShouldSatisfyAllConditions(
                () => _deletedCampaignItemBlastIDs.ShouldBeEmpty());
            _savedCampaignItemBlastList.ShouldSatisfyAllConditions(
               () => _savedCampaignItemBlastList.Count.ShouldBe(2),
               () => _savedCampaignItemBlastList.ShouldNotBeEmpty());
            _savedCampaignItemSuppressionList.ShouldSatisfyAllConditions(
                () => _savedCampaignItemSuppressionList.ShouldNotBeEmpty(),
                () => _savedCampaignItemSuppressionList.Count.ShouldBe(1));
        }

        [Test]
        public void Save_WithSelectedGroups_SavesRelatedEntities()
        {
            // Arrange
            SetFakesForSaveMethod();

            // Act
            var isSaved = _testEntity.Save();

            // Assert
            isSaved.ShouldBeTrue();
            _isCampaignItemSuppressionDeleted.ShouldBeTrue();
            _isBlastsFromCampaignItemCreated.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _deletedCampaignItemBlastIDs.ShouldSatisfyAllConditions(
                () => _deletedCampaignItemBlastIDs.ShouldNotBeEmpty(),
                () => _deletedCampaignItemBlastIDs.ShouldBe(new int[] { 1, 2 }));
            _savedCampaignItemBlastList.ShouldSatisfyAllConditions(
               () => _savedCampaignItemBlastList.Count.ShouldBe(2),
               () => _savedCampaignItemBlastList.ShouldNotBeEmpty());
            _savedCampaignItemSuppressionList.ShouldSatisfyAllConditions(
                () => _savedCampaignItemSuppressionList.ShouldNotBeEmpty(),
                () => _savedCampaignItemSuppressionList.Count.ShouldBe(1));
        }

        private void SetFakesForSaveMethod()
        {
            _isCampaignItemSuppressionDeleted = false;
            _isBlastsFromCampaignItemCreated = false;
            _savedCampaignItem = null;
            _deletedCampaignItemBlastIDs = new List<int>();
            _savedCampaignItemSuppressionList = new List<CommunicatorEntities.CampaignItemSuppression>();
            _savedCampaignItemBlastList = new List<CommunicatorEntities.CampaignItemBlast>();
            QueryString.Clear();
            QueryString.Add(PrePopLayoutIDKey, "1");

            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (cid, b) => GetCampaignItem();
            ShimgroupExplorer.AllInstances.getSelectedGroups = (g) => GetGroupList();
            ShimgroupExplorer.AllInstances.getSuppressionGroups = (g) => GetGroupList();

            // Save and Delete Fakes
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (cbId, user, b) => 
                { _deletedCampaignItemBlastIDs.Add(cbId); };
            ShimCampaignItem.SaveCampaignItemUser = (ci, user) => 
            {
                _savedCampaignItem = ci;
                return 1;
            };
            ShimCampaignItemBlast.SaveInt32ListOfCampaignItemBlastUser = (cid, lBlast, user) => 
            {
                _savedCampaignItemBlastList.AddRange(lBlast);
            };
            ShimCampaignItemSuppression.Delete_NoAccessCheckInt32UserBoolean = (csupId, user, b) => 
                { _isCampaignItemSuppressionDeleted = true; };
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (csup, user) => 
            {
                _savedCampaignItemSuppressionList.Add(csup);
                return 1;
            };
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (cid, user, b) => { _isBlastsFromCampaignItemCreated = true; };
        }

        private CommunicatorEntities.CampaignItem GetCampaignItem()
        {
            return new CommunicatorEntities.CampaignItem
            {
                CampaignItemType = "ab",
                CompletedStep = 1,
                BlastList = new List<CommunicatorEntities.CampaignItemBlast>
                {
                    new CommunicatorEntities.CampaignItemBlast
                    {
                        BlastID  = 1,
                        CampaignItemBlastID = 1,
                        LayoutID = 1,
                        DynamicFromName = SampleDynamicName,
                        FromName = SampleName
                    },

                    new CommunicatorEntities.CampaignItemBlast
                    {
                        BlastID  = 2,
                        CampaignItemBlastID = 2,
                        LayoutID = 2,
                        DynamicFromName = SampleDynamicName,
                        FromName = SampleName
                    },
                }
            };
        }

        private List<GroupObject> GetGroupList()
        {
            return new List<GroupObject>
            {
                new GroupObject
                {
                    GroupID = 1,
                    GroupName = SampleGroupName,
                    filters = new List<CommunicatorEntities.CampaignItemBlastFilter>
                    {
                        new CommunicatorEntities.CampaignItemBlastFilter
                        {
                            FilterID  = 1,
                            SmartSegmentID = 1,
                            RefBlastIDs = "1"
                        }
                    }
                }
            };
        }
    }
}
