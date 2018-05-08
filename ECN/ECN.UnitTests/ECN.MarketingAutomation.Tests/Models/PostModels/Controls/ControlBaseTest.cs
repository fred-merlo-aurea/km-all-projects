using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;
using CampaignItem = ecn.MarketingAutomation.Models.PostModels.Controls.CampaignItem;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using Group = ecn.MarketingAutomation.Models.PostModels.Controls.Group;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ControlBaseTest
    {
        private const decimal DefaultHeight = 40;
        private const decimal DefaultWidth = 140;
        private const string AllignCenterMiddle = "center middle";
        private const string DefaultContentColorBlue = "#142a52";
        private const decimal DefaulLargeWidth = 150;
        protected internal const int SampleCustomerId = 123;

        protected CampaignItem _parentCampaignItem;
        protected Form _parentForm;
        protected Group _parentGroup;
        protected Wait _parentWait;
        protected Mock<IGroupManager> _groupManagerMock;
        protected Mock<ILayoutManager> _layoutManagerMock;
        protected Mock<ILinkAliasManager> _linkAliasManagerMock;
        protected Mock<IEmailManager> _emailManagerMock;
        protected Mock<ICampaignItemManager> _campaignItemManagerMock;
        protected Mock<ICampaignManager> _campaignManagerMock;
        protected Mock<ILayoutPlansManager> _layoutPlansManagerMock;

        protected void InitParentControls()
        {
            _parentCampaignItem = new CampaignItem()
            {
                CustomerID = SampleCustomerId,
                CampaignID = 123,
                SendTime = DateTime.Today.AddDays(1),
                CreateCampaignItem = true
            };
            _parentForm = new Form();
            _parentGroup = new Group();
            _parentWait = new Wait();
        }

        protected void InitMocks()
        {
            _groupManagerMock = GetGroupManagerMock();
            _layoutManagerMock = GetLayoutManagerMock();
            _linkAliasManagerMock = GetLinkAliasManagerMock();
            _emailManagerMock = GetEmailManagerMock();
            _campaignItemManagerMock = GetCampaignItemMock(GetCampaignItem());
            _campaignManagerMock = GetCampaignMock();
            _layoutPlansManagerMock = GetLayoutPlansMock();
        }

        protected void InitValidVisualControlProperties(IVisualControl control)
        {
            control.Text = "SampleControlText";
        }

        protected void InitValidCampaignControlProperties(ICampaignControl control)
        {
            InitValidVisualControlProperties(control);
            control.CampaignItemName = "SampleCampaignItem";
            control.UseCampaignItemTemplate = true;
            control.CampaignItemTemplateID = 123;
            control.EmailSubject = "SampleSubject";
            control.FromEmail = "SampleFromEmail";
            control.FromName = "SampleFromName";
            control.ReplyTo = "SampleReplyTo";
            control.MessageID = 123;
        }

        protected void AssertCampaignControlBaseDefaultValues(
            ICampaignControl control,
            Enums.MarketingAutomationControlType type,
            string text = null,
            string contentColor = DefaultContentColorBlue,
            string contentAllign = null,
            decimal width = DefaulLargeWidth)
        {
            control.ControlType.ShouldBe(type);
            control.Text.ShouldBe(text);
            control.width.ShouldBe(width);
            control.height.ShouldBe(ControlConsts.DefaultHeight);
            control.content.text.ShouldBe(string.Empty);
            control.content.color.ShouldBe(contentColor);
            control.content.align.ShouldBe(contentAllign);
            control.fill.ShouldBe(ControlConsts.DefaultFillColorWhite);
            control.type.ShouldBe(ControlConsts.DefaultTypeRectangle);
            control.CustomerID.ShouldBe(0);
            control.CampaignItemTemplateID.ShouldBe(0);
            control.CampaignItemTemplateName.ShouldBeNull();
            control.UseCampaignItemTemplate.ShouldBe(false);
            control.BlastField1.ShouldBeNull();
            control.BlastField2.ShouldBeNull();
            control.BlastField3.ShouldBeNull();
            control.BlastField4.ShouldBeNull();
            control.BlastField5.ShouldBeNull();
            control.MessageID.ShouldBe(0);
            control.MessageName.ShouldBeNull();
            control.FromEmail.ShouldBeNull();
            control.ReplyTo.ShouldBeNull();
            control.FromName.ShouldBeNull();
            control.EmailSubject.ShouldBeNull();
            control.CampaignItemName.ShouldBeNull();
            control.HeatMapStats.ShouldBeNull();
        }

        protected void UpdateCampaignControlBaseWithTestValues(ICampaignControl control)
        {
            control.ControlType = Enums.MarketingAutomationControlType.Click;
            control.MAControlID = 123;
            control.ControlID = "SampleControlId";
            control.ECNID = 234;
            control.ExtraText = "SampleExtraText";
            control.IsDirty = true;
            control.MarketingAutomationID = 345;
            control.Text = "SampleText";
            control.xPosition = 1.23M;
            control.yPosition = 2.34M;
            control.editable.remove = false;
            control.editable.snap = true;
            control.CustomerID = 345;
            control.CampaignItemTemplateID = 456;
            control.CampaignItemTemplateName = "SampleCampaignItemTemplateName";
            control.UseCampaignItemTemplate = true;
            control.BlastField1 = "SampleBlastField1";
            control.BlastField2 = "SampleBlastField2";
            control.BlastField3 = "SampleBlastField3";
            control.BlastField4 = "SampleBlastField4";
            control.BlastField5 = "SampleBlastField5";
            control.MessageID = 567;
            control.MessageName = "SampleMessageName";
            control.FromEmail = "SampleFromEmail";
            control.ReplyTo = "SampleReplyTo";
            control.FromName = "SampleFromName";
            control.EmailSubject = "SampleEmailSubject";
            control.CampaignItemName = "SampleCampaignItemName";
            control.HeatMapStats = 678;
        }

        protected void AssertVisualControlBaseDefaultValues(
            IVisualControl control,
            Enums.MarketingAutomationControlType type,
            string text = null,
            string contentText = null,
            string contentAllign = AllignCenterMiddle,
            string contentColor = DefaultContentColorBlue,
            decimal width = DefaultWidth,
            decimal height = DefaultHeight)
        {
            control.ControlType.ShouldBe(type);
            control.Text.ShouldBe(text);
            control.width.ShouldBe(width);
            control.height.ShouldBe(height);
            control.content.text.ShouldBe(contentText);
            control.content.color.ShouldBe(contentColor);
            control.content.align.ShouldBe(contentAllign);
            control.fill.ShouldBe(ControlConsts.DefaultFillColorWhite);
            control.type.ShouldBe(ControlConsts.DefaultTypeRectangle);
        }

        protected void UpdateVisualControlBaseWithTestValues(IVisualControl control)
        {
            control.ControlType = Enums.MarketingAutomationControlType.Click;
            control.MAControlID = 123;
            control.ControlID = "SampleControlId";
            control.ECNID = 234;
            control.ExtraText = "SampleExtraText";
            control.IsDirty = true;
            control.MarketingAutomationID = 345;
            control.Text = "SampleText";
            control.xPosition = 1.23M;
            control.yPosition = 2.34M;
            control.editable.remove = false;
            control.editable.snap = true;
        }

        protected Mock<ILayoutManager> GetLayoutManagerMock(
            bool existsResult = true,
            bool isArchivedResult = false,
            bool isValidatedResult = true)
        {
            var layoutManagerMock = new Mock<ILayoutManager>();
            layoutManagerMock
                .Setup(mock => mock.Exists(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(existsResult);
            layoutManagerMock
                .Setup(mock => mock.IsArchived(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(isArchivedResult);
            layoutManagerMock
                .Setup(mock => mock.IsValidated(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(isValidatedResult);
            return layoutManagerMock;
        }

        protected Mock<IGroupManager> GetGroupManagerMock(bool existsResult = true, bool isArchived = false)
        {
            var groupManagerMock = new Mock<IGroupManager>();

            groupManagerMock
                .Setup(mock => mock.Exists(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(existsResult);

            groupManagerMock
                .Setup(mock => mock.IsArchived(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(isArchived);

            groupManagerMock
                .Setup(mock => mock.ValidateDynamicTags(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<User>()))
                .Callback((int groupId, int layoutId, User user) => { });

            return groupManagerMock;
        }

        protected Mock<ILinkAliasManager> GetLinkAliasManagerMock(bool existsResult = true)
        {
            var linkAliasManager = new Mock<ILinkAliasManager>();
            linkAliasManager
                .Setup(mock => mock.Exists(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(existsResult);
            return linkAliasManager;
        }

        protected Mock<IEmailManager> GetEmailManagerMock(bool isValidEmailResult = true)
        {
            var emailManagerMock = new Mock<IEmailManager>();
            emailManagerMock
                .Setup(mock => mock.IsValidEmailAddress(It.IsAny<string>()))
                .Returns(isValidEmailResult);
            return emailManagerMock;
        }

        protected Mock<ICampaignItemManager> GetCampaignItemMock(
            CommunicatorEntities.CampaignItem getByIdResult,
            bool existsResult = false)
        {
            var campaingItemManagerMock = new Mock<ICampaignItemManager>();

            campaingItemManagerMock
                .Setup(mock => mock.GetByCampaignItemID(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<bool>()))
                .Returns(getByIdResult);

            campaingItemManagerMock
                .Setup(mock => mock.GetByCampaignItemID_NoAccessCheck(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(getByIdResult);

            campaingItemManagerMock
                .Setup(mock => mock.Exists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(existsResult);
            return campaingItemManagerMock;
        }

        protected Mock<ICampaignManager> GetCampaignMock(bool existsResult = false)
        {
            var campaignManagerMock = new Mock<ICampaignManager>();

            campaignManagerMock
                .Setup(mock => mock.Exists(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(existsResult);
            return campaignManagerMock;
        }

        protected Mock<ILayoutPlansManager> GetLayoutPlansMock(bool existsResult = false)
        {
            var layoutPlansMock = new Mock<ILayoutPlansManager>();

            layoutPlansMock
                .Setup(mock => mock.Exists(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(existsResult);
            return layoutPlansMock;
        }

        protected bool ContainsError(ECNException exception, string errorMessage)
        {
            exception.ShouldNotBeNull();
            exception.ErrorList.ShouldNotBeNull();
            return exception.ErrorList.Any(error =>
                error.Entity == ECN_Framework_Common.Objects.Enums.Entity.CampaignItem &&
                error.Method == ECN_Framework_Common.Objects.Enums.Method.Validate &&
                error.ErrorMessage == errorMessage);
        }

        protected void ValidateException(
            ECNException exception,
            ECN_Framework_Common.Objects.Enums.Entity errorEntity,
            string expectedErrorMessage)
        {
            exception.ShouldNotBeNull();

            exception.ShouldSatisfyAllConditions(
                () => exception.ErrorList.ShouldNotBeNull(),
                () => exception.ErrorList.Any(error =>
                    error.Entity == errorEntity &&
                    error.Method == ECN_Framework_Common.Objects.Enums.Method.Validate &&
                    error.ErrorMessage == expectedErrorMessage).ShouldBeTrue());
        }

        protected CommunicatorEntities.MarketingAutomation GetMarketingAutomation()
        {
            return new CommunicatorEntities.MarketingAutomation
            {
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today.AddDays(2)
            };
        }

        protected CommunicatorEntities.CampaignItem GetCampaignItem()
        {
            var campaignItem = new CommunicatorEntities.CampaignItem
            {
                BlastList = new List<CommunicatorEntities.CampaignItemBlast>
                {
                    new CommunicatorEntities.CampaignItemBlast
                    {
                        Blast = new CommunicatorEntities.BlastRegular
                        {
                            EmailFrom = "SampleEmailFrom@SampleDomain.com",
                            EmailFromName = "SampleEmailFromName",
                            EmailSubject = "SampleEmailSubject",
                            ReplyTo = "SampleReplyTo"
                        }
                    }
                }
            };
            return campaignItem;
        }
    }
}
