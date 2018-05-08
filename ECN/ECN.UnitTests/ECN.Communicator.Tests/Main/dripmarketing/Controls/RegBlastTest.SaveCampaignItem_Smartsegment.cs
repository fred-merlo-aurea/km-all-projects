using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.blastsmanager;
using ecn.communicator.main.blasts.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using WizardGroup = ecn.communicator.main.ECNWizard.Group;

namespace ECN.Communicator.Tests.Main.dripmarketing.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class RegBlastTest : PageHelper
    {
        private const string SampleGroup = "SampleGroup";
        private const string SampleEmail = "test@test.com";
        private const string GvSelectedGroupsPropertyName = "gvSelectedGroups";
        private const string GvSuppressedPropertyName = "gvSuppressed";
        private const string GroupIDLabel = "lblGroupID";
        private const string LayoutIDLabel = "lblLayoutID";
        private const string TestUser = "TestUser";
        private const string TextBoxSubject = "txtSubject";
        private const string SampleSubject = "SampleSubject";
        private const string KeyMyNodeID = "myNodeID";
        private const string EmailFromDropDown = "drpEmailFrom";
        private const string EmailFromNameDropDown = "drpEmailFromName";
        private const string ReplyToDropDown = "drpReplyTo";
        private const string DynamicEmailFromDropDown = "dyanmicEmailFrom";
        private const string DynamicEmailFromNameDropDown = "dyanmicEmailFromName";
        private const string DynamicReplyToDropDown = "dyanmicReplyToEmail";
        private const string SampleCampaignItem = "SampleCampaignItem";
        private const string ViewStateProperty = "ViewState";
        private const string GroupExplorerField = "groupExplorer1";
        private const string PhErrorControl = "phError";
        private const string LblErrorMessageControl = "lblErrorMessage";

        private regBlast _testEntity;
        private WizardGroup.groupExplorer _groupExplorer;
        private PrivateObject _privateTestObject;
        private PrivateObject _privateGroupExplorerObj;
        private bool _isCampaignItemSaved;
        private CampaignItem _savedCampaignItem;
        private bool _isBlastLitRefDeleted;
        private List<int> _deletedBlastRefId;
        private bool _isCampaignItemBlastSaved;
        private CampaignItemBlast _savedCampaignItemBlast;
        private bool _isCampaignSuppressionDeleted;
        private int _deletedSuppressionCamapaignId;
        private bool _isCampaignSuppressionSaved;
        private List<CampaignItemSuppression> _savedCampaignSuppresseion;
        private bool _isBlastCreated;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            InitializeSessionFakes();
            _testEntity = new regBlast ();
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            _groupExplorer = (WizardGroup.groupExplorer)_privateTestObject.GetFieldOrProperty(GroupExplorerField);
            InitializeAllControls(_groupExplorer);
            _privateGroupExplorerObj = new PrivateObject(_groupExplorer);
        }
        
        [Test]
        public void SaveCampaignItem_smartsegment_WhenCampaignItemNullAndGridRowCountGreaterThanOne_LogsECNException()
        {
            // Arrange
            CampaignItem campaignItem = null;
            var parentCI = new CampaignItem();
            var campaignId = 1;
            SetPageControls();
            var gvSelectedGroups = (GridView)_privateTestObject.GetFieldOrProperty(GvSelectedGroupsPropertyName);
            gvSelectedGroups.DataSource = new List<Group>
            {
                new Group { GroupID = 1, GroupName = SampleGroup },
                new Group { GroupID = 2, GroupName = SampleGroup }
            };
            gvSelectedGroups.DataBind();

            // Act
            var isSaved = _testEntity.saveCampaignItem_smartsegment(campaignItem, parentCI, campaignId);

            // Assert
            isSaved.ShouldSatisfyAllConditions(
                () => isSaved.ShouldBeFalse(),
                () => Get<PlaceHolder>(_privateGroupExplorerObj, PhErrorControl).Visible.ShouldBeTrue(),
                () => Get<Label>(_privateGroupExplorerObj, LblErrorMessageControl).Text.
                            ShouldContain($"The parent CampaignItem is a multi-group blast. Please select only one group."));
        }

        [Test]
        public void SaveCampaignItem_smartsegment_WhenParentSendTimeIsGreaterThanChildSendTime_LogsECNException()
        {
            // Arrange
            CampaignItem campaignItem = GetCampaignItem();
            var parentCI = GetCampaignItem();
            parentCI.SendTime = DateTime.UtcNow.AddDays(1);
            var campaignId = 1;
            SetPageControls();
            SetGvSelectedGroupsGrid();
            SetGvSuppressedGroupsGrid();
            SetFakesForSaveCampaignItem();

            // Act
            var isSaved = _testEntity.saveCampaignItem_smartsegment(campaignItem, parentCI, campaignId);

            // Assert
            isSaved.ShouldSatisfyAllConditions(
               () => isSaved.ShouldBeFalse(),
               () => Get<PlaceHolder>(_privateGroupExplorerObj, PhErrorControl).Visible.ShouldBeTrue(),
               () => Get<Label>(_privateGroupExplorerObj, LblErrorMessageControl).Text.
                            ShouldContain($"The scheduled time for this blast must be greater than the scheduled time of preceeding blasts"));
        }

        [Test]
        public void SaveCampaignItem_smartsegment_WhenValidCampaignItem_SavesRelatedEntities()
        {
            // Arrange
            CampaignItem campaignItem = GetCampaignItem();
            var parentCI = GetCampaignItem();
            parentCI.SendTime = parentCI.SendTime.Value.AddDays(-1);
            var campaignId = 1;
            SetPageControls();
            SetGvSelectedGroupsGrid();
            SetGvSuppressedGroupsGrid();
            SetFakesForSaveCampaignItem();

            // Act
            var isSaved = _testEntity.saveCampaignItem_smartsegment(campaignItem, parentCI, campaignId);

            // Assert
            isSaved.ShouldSatisfyAllConditions(
                () => isSaved.ShouldBeTrue(),
                () => _isBlastCreated.ShouldBeTrue(),
                () => _isBlastLitRefDeleted.ShouldBeTrue(),
                () => _isCampaignItemBlastSaved.ShouldBeTrue(),
                () => _isCampaignItemSaved.ShouldBeTrue(),
                () => _isCampaignSuppressionDeleted.ShouldBeTrue(),
                () => _isCampaignSuppressionSaved.ShouldBeTrue(),
                () => _savedCampaignItemBlast.ShouldNotBeNull(),
                () => _savedCampaignItem.ShouldNotBeNull(),
                () => _savedCampaignSuppresseion.ShouldNotBeNull(),
                () => _deletedBlastRefId.Count.ShouldBe(1),
                () => _deletedBlastRefId.ShouldContain(-1),
                () => _deletedSuppressionCamapaignId.ShouldBe(-1));
        }

        private CampaignItem GetCampaignItem()
        {
            return new CampaignItem
            {
                CampaignID = 1,
                SendTime = DateTime.UtcNow,
                CampaignItemName = SampleCampaignItem,
                BlastList = new List<CampaignItemBlast>
                {
                    new CampaignItemBlast { BlastID = 1 }
                }
            };
        }

        private void SetFakesForSaveCampaignItem()
        {
            _isBlastLitRefDeleted = false;
            _deletedBlastRefId = new List<int>();
            _savedCampaignItem = null;
            _isCampaignItemSaved = false;
            _savedCampaignItemBlast = null;
            _isCampaignItemBlastSaved = false;
            _isCampaignSuppressionDeleted = false;
            _isCampaignSuppressionSaved = false;
            _savedCampaignSuppresseion = new List<CampaignItemSuppression>();
            _isBlastCreated = false;

            var viewState = Get<StateBag>(_privateTestObject, ViewStateProperty);
            viewState.Add(KeyMyNodeID, "1");

            ShimBlastScheduler.AllInstances.SetupScheduleString = (_, __) => new BlastSetupInfo
            {
                BlastScheduleID = 1,
                SendNowIsAmount = true,
                SendNowAmount = 1,
                SendTime = DateTime.UtcNow,
            };

            ShimCustomer.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (_, __, ___) => true;

            ShimCampaignItem.SaveCampaignItemUser = (ci, user) =>
            {
                _savedCampaignItem = ci;
                _isCampaignItemSaved = true;
                return _savedCampaignItem.CampaignItemID;
            };
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (crefId, user, b) =>
            {
                _deletedBlastRefId.Add(crefId);
                _isBlastLitRefDeleted = true;
            };
            ShimCampaignItemBlast.SaveInt32ListOfCampaignItemBlastUser = (cid, cbList, user) =>
            {
                _savedCampaignItemBlast = cbList[0];
                _isCampaignItemBlastSaved = true;
            };
            ShimCampaignItemSuppression.DeleteInt32UserBoolean = (csid, user, b) =>
            {
                _deletedSuppressionCamapaignId = csid;
                _isCampaignSuppressionDeleted = true;
            };
            ShimCampaignItemSuppression.SaveCampaignItemSuppressionUser = (csuppression, user) =>
            {
                _savedCampaignSuppresseion.Add(csuppression);
                _isCampaignSuppressionSaved = true;
                return csuppression.CampaignItemID.Value;
            };
            ShimBlast.CreateBlastsFromCampaignItemInt32UserBoolean = (cid, user, b) =>
            {
                _isBlastCreated = true;
            };
        }

        private void SetGvSelectedGroupsGrid()
        {
            var gvSelectedGroups = (GridView)_privateTestObject.GetFieldOrProperty(GvSelectedGroupsPropertyName);
            gvSelectedGroups.DataSource = new List<Group>
            {
                new Group { GroupID = 1, GroupName = SampleGroup }
            };
            gvSelectedGroups.DataBind();
            gvSelectedGroups.Rows[0].Cells[0].Controls.Add(new Label { ID = GroupIDLabel, Text = "1" });
        }

        private void SetGvSuppressedGroupsGrid()
        {
            var gvSuppressed = (GridView)_privateTestObject.GetFieldOrProperty(GvSuppressedPropertyName);
            gvSuppressed.DataSource = new List<Group>
            {
                new Group { GroupID = 1, GroupName = SampleGroup }
            };
            gvSuppressed.DataBind();
            gvSuppressed.Rows[0].Cells[0].Controls.Add(new Label { ID = GroupIDLabel, Text = "1" });
        }

        private void SetPageControls()
        {
            var drpEmailFrom = Get<DropDownList>(_privateTestObject, EmailFromDropDown);
            drpEmailFrom.Items.Add(new ListItem { Text = SampleEmail, Value = SampleEmail, Selected = true });

            var drpEmailFromName = Get<DropDownList>(_privateTestObject, EmailFromNameDropDown);
            drpEmailFromName.Items.Add(new ListItem { Text = TestUser, Value = TestUser, Selected = true });
            
            var drpReplyTo = Get<DropDownList>(_privateTestObject, ReplyToDropDown);
            drpReplyTo.Items.Add(new ListItem { Text = SampleEmail, Value = SampleEmail, Selected = true });

            var dyanmicEmailFrom = Get<DropDownList>(_privateTestObject, DynamicEmailFromDropDown);
            dyanmicEmailFrom.Items.Add(new ListItem { Text = SampleEmail, Value = SampleEmail, Selected = true });

            var dyanmicEmailFromName = Get<DropDownList>(_privateTestObject, DynamicEmailFromNameDropDown);
            dyanmicEmailFromName.Items.Add(new ListItem { Text = TestUser, Value = TestUser, Selected = true });

            var dyanmicReplyToEmail = Get<DropDownList>(_privateTestObject, DynamicReplyToDropDown);
            dyanmicReplyToEmail.Items.Add(new ListItem { Text = SampleEmail, Value = SampleEmail, Selected = true });

            Get<TextBox>(_privateTestObject, TextBoxSubject).Text = SampleSubject;
            Get<Label>(_privateTestObject, LayoutIDLabel).Text = "1";
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = 1 };
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }
    }
}
