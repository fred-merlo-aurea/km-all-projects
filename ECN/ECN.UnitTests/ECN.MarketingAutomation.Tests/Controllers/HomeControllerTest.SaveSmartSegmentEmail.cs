using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Controls = ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ecn.MarketingAutomation.Controllers;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels;
using Entities = ECN_Framework_Entities.Communicator;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodSaveSmartSegmentEmailClick = "SaveSmartSegmentEmail_Click";
        private const string SmartSegmentFromName = "from";
        private const string SmartSegmentReplyTo = "replyTo";
        private const string SmartSegmentEmailSubject = "subject";
        private List<Entities::CampaignItemBlast> _blastList;

        [Test]
        [TestCase("Click", 1)]
        [TestCase("Click", 0)]
        [TestCase("NoClick", 1)]
        [TestCase("NoOpen", 1)]
        [TestCase("NotSent", 1)]
        [TestCase("Open", 1)]
        [TestCase("Open_NoClick", 1)]
        [TestCase("Sent", 1)]
        [TestCase("Suppressed", 1)]
        public void SaveSmartSegmentEmail_MultipleCases_SmartSegmentParentSaved(string controlType, int ecnId)
        {
            // Arrange
            SetupSaveSmartSegmentEmail_Click(
                controlType,
                out var controlBase,
                out var wait,
                out var expectedCampaignItem,
                ecnId);

            // Act
            _privateObject.Invoke(MethodSaveSmartSegmentEmailClick, controlBase, wait, false, true);

            // Assert
            _savedCampaignItem.ShouldSatisfyAllConditions(
                () => _savedCampaignItem.CampaignItemFormatType.ShouldBe(expectedCampaignItem.CampaignItemFormatType),
                () => _savedCampaignItem.CampaignItemName.ShouldBe(expectedCampaignItem.CampaignItemName),
                () => _savedCampaignItem.CampaignItemNameOriginal.ShouldBe(expectedCampaignItem.CampaignItemNameOriginal),
                () => _savedCampaignItem.CampaignItemType.ShouldBe(expectedCampaignItem.CampaignItemType),
                () => _savedCampaignItem.CompletedStep.ShouldBe(expectedCampaignItem.CompletedStep),
                () => _savedCampaignItem.CampaignItemTemplateID.ShouldBe(expectedCampaignItem.CampaignItemTemplateID),
                () => _savedCampaignItem.CustomerID.ShouldBe(expectedCampaignItem.CustomerID),
                () => _savedCampaignItem.EnableCacheBuster.ShouldBe(expectedCampaignItem.EnableCacheBuster),
                () => _savedCampaignItem.FromEmail.ShouldBe(expectedCampaignItem.FromEmail),
                () => _savedCampaignItem.FromName.ShouldBe(expectedCampaignItem.FromName),
                () => _savedCampaignItem.BlastField1.ShouldBe(expectedCampaignItem.BlastField1),
                () => _savedCampaignItem.BlastField2.ShouldBe(expectedCampaignItem.BlastField2),
                () => _savedCampaignItem.BlastField3.ShouldBe(expectedCampaignItem.BlastField3),
                () => _savedCampaignItem.BlastField4.ShouldBe(expectedCampaignItem.BlastField4),
                () => _savedCampaignItem.BlastField5.ShouldBe(expectedCampaignItem.BlastField5),
                () => _savedCampaignItem.IgnoreSuppression.ShouldBe(expectedCampaignItem.IgnoreSuppression),
                () => _savedCampaignItem.IsDeleted.ShouldBe(expectedCampaignItem.IsDeleted),
                () => _savedCampaignItem.IsHidden.ShouldBe(expectedCampaignItem.IsHidden),
                () => _savedCampaignItem.ReplyTo.ShouldBe(expectedCampaignItem.ReplyTo),
                () => _savedCampaignItem.SendTime.ShouldBe(expectedCampaignItem.SendTime),
                //Blast List Asserts
                () => _savedCampaignItem.BlastList.ShouldAllBe(cItemBlast => expectedCampaignItem.BlastList[0].BlastID == cItemBlast.BlastID),
                () => _savedCampaignItem.BlastList.ShouldAllBe(cItemBlast => expectedCampaignItem.BlastList[0].CampaignItemBlastID == cItemBlast.CampaignItemBlastID),
                () => _savedCampaignItem.BlastList.ShouldAllBe(cItemBlast => expectedCampaignItem.BlastList[0].EmailFrom == cItemBlast.EmailFrom),
                () => _savedCampaignItem.BlastList.ShouldAllBe(cItemBlast => expectedCampaignItem.BlastList[0].EmailSubject == cItemBlast.EmailSubject),
                () => _savedCampaignItem.BlastList.ShouldAllBe(cItemBlast => expectedCampaignItem.BlastList[0].FromName == cItemBlast.FromName),
                () => _savedCampaignItem.BlastList.ForEach(cItemBlast => cItemBlast.Filters.ShouldAllBe(filter => filter.IsDeleted == expectedCampaignItem.BlastList[0].Filters[0].IsDeleted)),
                () => _savedCampaignItem.BlastList.ForEach(cItemBlast => cItemBlast.Filters.ShouldAllBe(filter => filter.RefBlastIDs == expectedCampaignItem.BlastList[0].Filters[0].RefBlastIDs)),
                () => _savedCampaignItem.BlastList.ForEach(cItemBlast => cItemBlast.Filters.ShouldAllBe(filter => filter.SmartSegmentID == expectedCampaignItem.BlastList[0].Filters[0].SmartSegmentID))
                );
        }

        private void SetupSaveSmartSegmentEmail_Click(
            string controlType,
            out Controls::CampaignControlBase smartSegment,
            out ControlBase wait,
            out Entities::CampaignItem expectedCampaignItem,
            int ecnId)
        {
            const int DefaultId = 1;
            const string DefaultStringId = "1";
            const string TestEmailAddress = "from@test.com";

            var instance = Activator.CreateInstance("ecn.MarketingAutomation", $"ecn.MarketingAutomation.Models.PostModels.Controls.{controlType}");
            smartSegment = (Controls::CampaignControlBase)instance.Unwrap();

            smartSegment.ControlID = DefaultStringId;
            smartSegment.ECNID = ecnId;

            smartSegment.FromEmail = TestEmailAddress;
            smartSegment.FromName = SmartSegmentFromName;
            smartSegment.ReplyTo = SmartSegmentReplyTo;
            smartSegment.MessageID = DefaultId;
            smartSegment.EmailSubject = SmartSegmentEmailSubject;
            smartSegment.CampaignItemName = DummyString;
            smartSegment.CampaignItemTemplateID = DefaultId;
            smartSegment.BlastField1 = nameof(Controls::CampaignControlBase.BlastField1);
            smartSegment.BlastField2 = nameof(Controls::CampaignControlBase.BlastField2);
            smartSegment.BlastField3 = nameof(Controls::CampaignControlBase.BlastField3);
            smartSegment.BlastField4 = nameof(Controls::CampaignControlBase.BlastField4);
            smartSegment.BlastField5 = nameof(Controls::CampaignControlBase.BlastField5);
            smartSegment.CustomerID = DefaultId;

            wait = new Controls::Wait
            {
                ControlID = DefaultStringId,
                ECNID = DefaultId
            };
            var allControls = new List<ControlBase>
            {
                smartSegment, wait
            };
            var allConnectors = new List<Connector> {
                new Connector
                {
                    to = new to{ shapeId = smartSegment.ControlID},
                    from  = new from{ shapeId = smartSegment.ControlID}
                }
            };
            _blastList = new List<Entities::CampaignItemBlast>
            {
                new Entities::CampaignItemBlast
                {
                    BlastID = DefaultId,
                    Filters = new List<Entities::CampaignItemBlastFilter>{
                        new Entities::CampaignItemBlastFilter
                        {
                            IsDeleted = false,
                            RefBlastIDs = DefaultStringId,
                            SmartSegmentID = DefaultId
                        }
                    }
                }
            };

            expectedCampaignItem = new Entities::CampaignItem
            {
                FromEmail = smartSegment.FromEmail,
                FromName = smartSegment.FromName,
                ReplyTo = smartSegment.ReplyTo,
                CampaignItemFormatType = "HTML",
                CampaignItemName = smartSegment.CampaignItemName,
                CampaignItemNameOriginal = smartSegment.CampaignItemName,
                CampaignItemTemplateID = smartSegment.CampaignItemTemplateID,
                CampaignItemType = "Regular",
                CompletedStep = 5,
                CustomerID = null,
                BlastField1 = smartSegment.BlastField1,
                BlastField2 = smartSegment.BlastField2,
                BlastField3 = smartSegment.BlastField3,
                BlastField4 = smartSegment.BlastField4,
                BlastField5 = smartSegment.BlastField5,
                BlastList = _blastList,
                EnableCacheBuster = true,
                IgnoreSuppression = false,
                IsDeleted = false,
                IsHidden = false,
                SendTime = DateTime.MinValue
            };

            _privateObject.SetProperty(AllConnectorsProperty, allConnectors);
            _privateObject.SetProperty(AllControlsProperty, allControls);

            SetupShimsForSaveSmartSegmentEmail(controlType, DefaultId);
        }

        private void SetupShimsForSaveSmartSegmentEmail(string controlType, int defaultId)
        {
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (campaignItemID, getChildren) => new Entities::CampaignItem
                {
                    CampaignID = defaultId,
                    BlastList = _blastList,
                    SuppressionList = new List<Entities::CampaignItemSuppression>
                    {
                        new Entities::CampaignItemSuppression()
                    }
                };

            var shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new KMPlatform.Entity.User()
            {
                UserID = defaultId
            };
            ShimECNSession.CurrentSession = () => shimECNSession.Instance;

            ShimSmartSegment.GetSmartSegments = () => new List<Entities::SmartSegment>()
            {
                new Entities::SmartSegment
                {
                    SmartSegmentID = defaultId,
                    SmartSegmentName = controlType.ToLower()
                }
            };

            ShimCampaignItem.Save_UseAmbientTransactionCampaignItemUser = (item, user) =>
            {
                _savedCampaignItem = item;
                return defaultId;
            };
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (id, user, getChildren) =>
                new Entities::CampaignItemTemplate
                {
                    CampaignItemTemplateID = defaultId,
                    CampaignID = defaultId,
                    OptoutGroupList = new List<Entities::CampaignItemTemplateOptoutGroup>
                    {
                        new Entities::CampaignItemTemplateOptoutGroup
                        {
                            GroupID = defaultId
                        }
                    },
                    OptOutSpecificGroup = true,
                    OptOutMasterSuppression = true
                };
            ShimCampaignItemBlast.Save_UseAmbientTransactionInt32ListOfCampaignItemBlastUser =
                (campaignItemID, newList, user) => { };
            ShimCampaignItemSuppression.Delete_NoAccessCheckInt32UserBoolean =
                (campaignItemID, user, overrideUpdate) => { };
            ShimCampaignItemSuppression.Save_UseAmbientTransactionCampaignItemSuppressionUser =
                (suppression, user) => defaultId;
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean =
                (campaignItemID, user, checkFirst, keepPaused) => { };
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (campaignItemID, user, getChildren) =>
                new List<Entities::CampaignItemBlast>();
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (campaignItemBlastID, currentUser, overrideUpdate) => { };
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (campaignItemOptOutGroup, user) =>
            {
                _savedCampaignItemOptOutGroup = campaignItemOptOutGroup;
            };
            var shimHomeController = new ShimAutomationBaseController(_controller);
            shimHomeController.SaveLinkTrackingParamOptionsCampaignItem = (campaignItem) => { };
        }
    }
}