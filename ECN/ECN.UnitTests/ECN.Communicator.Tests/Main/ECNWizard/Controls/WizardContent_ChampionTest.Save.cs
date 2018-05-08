using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.ECNWizard.Controls;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard.Controls
{
    public partial class WizardContentChampionTest : PageHelper
    {
        private const string ViewStateProperty = "ViewState";
        private const string SelectedGroupIDKey = "SelectedGroupID";
        private const string SelectedFilterIDKey = "SelectedFilterID";
        private const string SelectedSuppressionGroupsKey = "SelectedSuppressionGroups";
        private const string SampleEmail = "test@test.com";
        private const string SampleEmailFromName = "TestUser";

        private bool _isSampleSaved;
        private Sample _savedSample;
        private bool _isCampaignItemSaved;
        private bool _isBlastCreated;
        private List<CampaignItem> _savedCampaignItems;
        private CampaignItemBlast _savedCampaignItemBlast;
        private int _deletedCampaignItemBlastID;
        private int _deletedCampaignItemId;
        private List<CampaignItemSuppression> _savedCampaignItemSuppression;

        private static object[] CampaignItemSource =
            {
            new CampaignItem
                {
                    CompletedStep = 1,
                    CustomerID = 1,
                    BlastList = new List<CampaignItemBlast>
                    {
                        new CampaignItemBlast { BlastID = 1, CampaignItemBlastID = 1, CampaignItemID = 1 }
                    },
                    SuppressionList = new List<CampaignItemSuppression>()
                    {
                        new CampaignItemSuppression
                        {
                            GroupID = 1,
                            Filters = new List<CampaignItemBlastFilter>
                            {
                                new CampaignItemBlastFilter
                                {
                                    FilterID = 0,
                                    SmartSegmentID = 1,
                                    RefBlastIDs = "1,2"
                                }
                            }
                        }
                    }
                },
            new CampaignItem
                {
                    CompletedStep = 1,
                    CustomerID = 1,
                    BlastList = new List<CampaignItemBlast>(),
                    SuppressionList = new List<CampaignItemSuppression>()
                    {
                        new CampaignItemSuppression
                        {
                            GroupID = 1,
                            Filters = new List<CampaignItemBlastFilter>
                            {
                                new CampaignItemBlastFilter
                                {
                                    FilterID = 0,
                                    SmartSegmentID = 1,
                                    RefBlastIDs = "1,2"
                                }
                            }
                        }
                    }
                }
            };

        [Test]
        public void Save_WhenAbSampleBlastDropDownIsEmpty_ThrowsECNException()
        {
            // Act
            var ecnExp = Should.Throw<ECNException>(() => _control.Save());

            // Assert
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ShouldNotBeNull(),
                () => ecnExp.ErrorList.ShouldNotBeEmpty(),
                () => ecnExp.ErrorList.Count.ShouldBe(1),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("No Sample Selected")));
        }

        [Test]
        public void Save_WhenCheckLosingCampaignIsNotChecked_ThrowsECNException()
        {
            // Arrange
            InitializePageAndControls();

            // Act
            var ecnExp = Should.Throw<ECNException>(() => _control.Save());

            // Assert
            ecnExp.ShouldSatisfyAllConditions(
                () => ecnExp.ShouldNotBeNull(),
                () => ecnExp.ErrorList.ShouldNotBeEmpty(),
                () => ecnExp.ErrorList.Count.ShouldBe(1),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select who to send the winner to")));
        }

        [Test]
        [TestCaseSource(nameof(CampaignItemSource))]
        public void Save_WhenCheckLosingCampaignChecked_SavesRelatedEntities(CampaignItem campaignItem)
        {
            // Arrange
            const string deliveredOrOpened = "TestString";
            InitializePageAndControls();
            SetupFakesIfEmptyWinnerType(deliveredOrOpened);
            SetFakesForSaveMethod();
            SetPageViewState();
            Get<CheckBox>(_privateControlObj, ChkAorB).Checked = true;
            Get<CheckBox>(_privateControlObj, ChkLosingCampaign).Checked = true;
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, child) => campaignItem;
            
            // Act
            var isSaved = _control.Save();

            // Assert
            isSaved.ShouldSatisfyAllConditions(
                () => isSaved.ShouldBeTrue(),
                () => _isBlastCreated.ShouldBeTrue(),
                () => _isCampaignItemSaved.ShouldBeTrue(),
                () => _isSampleSaved.ShouldBeTrue(),
                () => _deletedCampaignItemBlastID.ShouldBe(campaignItem.BlastList.Any() ? campaignItem.BlastList[0].CampaignItemBlastID : 0),
                () => _deletedCampaignItemId.ShouldBe(0),
                () => _savedCampaignItemBlast.ShouldNotBeNull(),
                () => _savedCampaignItems.ShouldNotBeEmpty(),
                () => _savedCampaignItems.Count.ShouldBe(2),
                () => _savedCampaignItemSuppression.ShouldNotBeNull(),
                () => _savedCampaignItemSuppression.ShouldNotBeEmpty(),
                () => _savedCampaignItemSuppression.Count.ShouldBe(1),
                () => _savedSample.ShouldNotBeNull());
        }

        private void SetFakesForSaveMethod()
        {
            ResetVariables();
            ShimSample.SaveSampleUser = (sample, user) =>
            {
                _savedSample = sample;
                _isSampleSaved = true;
            };
            ShimCampaignItem.SaveCampaignItemUser = (ci, user) =>
            {
                _savedCampaignItems.Add(ci);
                _isCampaignItemSaved = true;
                return ci.CampaignItemID;
            };

            ShimCampaignItemBlastFilter.DeleteByCampaignItemBlastIDInt32 = (cblastId) =>
            {
                _deletedCampaignItemBlastID = cblastId;
            };

            ShimCampaignItemSuppression.Delete_NoAccessCheckInt32UserBoolean = (cId, user, b) =>
            {
                _deletedCampaignItemId = cId;
            };
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (cis, user) =>
            {
                _savedCampaignItemSuppression.Add(cis);
                return cis.CampaignItemSuppressionID;
            };
            ShimCampaignItemBlast.SaveCampaignItemBlastUserBoolean = (cblast, user, b) =>
            {
                _savedCampaignItemBlast = cblast;
                return cblast.CampaignItemBlastID;
            };
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (cid, user, b) =>
            {
                _isBlastCreated = true;
            };
        }

        private GroupObject GetGroupObject()
        {
            return new GroupObject
            {
                GroupID = 1,
                filters = new List<CampaignItemBlastFilter>
                {
                    new CampaignItemBlastFilter
                    {
                        CampaignItemBlastID = 1,
                        CampaignItemBlastFilterID = 1,
                        FilterID = 1
                    },
                    new CampaignItemBlastFilter
                    {
                        CampaignItemBlastID = 1,
                        CampaignItemBlastFilterID = 1,
                        RefBlastIDs = "1,2",
                        SmartSegmentID = 1
                    },
                }
            };
        }

        private void ResetVariables()
        {
            _isSampleSaved = false;
            _isBlastCreated = true;
            _savedSample = null;
            _isCampaignItemSaved = false;
            _savedCampaignItemBlast = null;
            _savedCampaignItems = new List<CampaignItem>();
            _savedCampaignItemSuppression = new List<CampaignItemSuppression>();
            _deletedCampaignItemBlastID = 0;
            _deletedCampaignItemId = 0;
        }

        private void SetPageViewState()
        {
            var viewState = Get<StateBag>(_privateControlObj, ViewStateProperty);
            viewState.Add(SelectedGroupIDKey, "1");
            viewState.Add(SelectedFilterIDKey, GetGroupObject());
            viewState.Add(SelectedSuppressionGroupsKey, new List<GroupObject> { GetGroupObject() });
        }
    }
}
