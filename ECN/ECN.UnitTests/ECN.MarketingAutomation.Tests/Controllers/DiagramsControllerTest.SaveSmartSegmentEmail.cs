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

namespace ecn.MarketingAutomation.Tests.Controllers
{
    [TestFixture]
    public partial class DiagramsControllerTest
    {
        private const string SaveSmartSegmentEmailClick = "SaveSmartSegmentEmail_Click";
        private List<Entities.CampaignItemBlast> _blastList;

        [Test]
        [TestCase("Click")]
        [TestCase("NoClick")]
        [TestCase("NoOpen")]
        [TestCase("NotSent")]
        [TestCase("Open")]
        [TestCase("Open_NoClick")]
        [TestCase("Sent")]
        [TestCase("Suppressed")]
        public void SaveSmartSegmentEmail_Click_SmartSegmentParentAndKeepUsed_Saves(string controlType)
        {
            // Arrange
            Controls.CampaignControlBase controlBase;
            ControlBase wait;
            Entities.CampaignItem expectedCampaignItem;
            
            //SaveSmartSegmentEmail_Click is shimmed in main Setup file by another developer, so creating new _shimsContext
            _shimsContext.Dispose();
            _shimsContext = ShimsContext.Create();

            SetupSaveSmartSegmentEmail_Click(controlType, out controlBase, out wait, out expectedCampaignItem);

            // Act
            _diagramsControllerPrivateObject.Invoke(SaveSmartSegmentEmailClick, controlBase, wait, true, false);

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
        private void SetAllConnectorsProperty(List<Connector> allConnectors)
        {
            const string AllConnectorsPropertyName = "AllConnectors";
            _diagramsControllerPrivateObject.SetProperty(AllConnectorsPropertyName, allConnectors);
        }

        private void SetAllControlsProperty(List<ControlBase> allControls)
        {
            const string AllControlsPropertyName = "AllControls";
            _diagramsControllerPrivateObject.SetProperty(AllControlsPropertyName, allControls);
        }

        private void SetupSaveSmartSegmentEmail_Click(string controlType, out Controls.CampaignControlBase smartSegment, out ControlBase wait, out Entities.CampaignItem expectedCampaignItem)
        {
            var defaultId = 1;
            var defaultStringId = "1";
            const string testEmailAddress = "from@test.com";

            var instance = Activator.CreateInstance("ecn.MarketingAutomation", $"ecn.MarketingAutomation.Models.PostModels.Controls.{controlType}");
            smartSegment = (Controls.CampaignControlBase)instance.Unwrap();

            smartSegment.ControlID = defaultStringId;
            smartSegment.ECNID = defaultId;

            smartSegment.FromEmail = testEmailAddress;
            smartSegment.FromName = "from";
            smartSegment.ReplyTo = "replyTo";
            smartSegment.MessageID = defaultId;
            smartSegment.EmailSubject = "subject";
            smartSegment.CampaignItemName = "test";
            smartSegment.CampaignItemTemplateID = defaultId;
            smartSegment.BlastField1 = "BlastField1";
            smartSegment.BlastField2 = "BlastField2";
            smartSegment.BlastField3 = "BlastField3";
            smartSegment.BlastField4 = "BlastField4";
            smartSegment.BlastField5 = "BlastField5";
            smartSegment.CustomerID = defaultId;

            wait = new Controls.Wait
            {
                ControlID = defaultStringId,
                ECNID = defaultId
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
            _blastList = new List<Entities.CampaignItemBlast>
            {
                new Entities.CampaignItemBlast
                {
                    BlastID = defaultId,
                    Filters = new List<Entities.CampaignItemBlastFilter>{
                        new Entities.CampaignItemBlastFilter
                        {
                            IsDeleted = false,
                            RefBlastIDs = defaultStringId,
                            SmartSegmentID = defaultId
                        }
                    }
                }
            };

            expectedCampaignItem = new Entities.CampaignItem
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

            SetAllConnectorsProperty(allConnectors);
            SetAllControlsProperty(allControls);
            
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean = (campaignItemID, getChildren) =>
            {
                return new Entities.CampaignItem
                {
                    CampaignID = defaultId,
                    BlastList = _blastList
                };
            };

            ShimECNSession shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new KMPlatform.Entity.User()
            {
                UserID = defaultId
            };
            ShimECNSession.CurrentSession = () => shimECNSession.Instance;

            ShimSmartSegment.GetSmartSegments = () =>
            {
                return new List<Entities.SmartSegment>() {
                    new Entities.SmartSegment{
                        SmartSegmentID = defaultId,
                        SmartSegmentName =  controlType.ToLower()
                    }
                };
            };

            ShimCampaignItem.Save_UseAmbientTransactionCampaignItemUser = (item, user) =>
            {
                _savedCampaignItem = item;
                return defaultId;
            };
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean = (campaignItemTemplateID, user, getChildren) =>
            {
                return new Entities.CampaignItemTemplate
                {
                    CampaignItemTemplateID = defaultId,
                    CampaignID = defaultId,
                    OptoutGroupList = new List<Entities.CampaignItemTemplateOptoutGroup>
                    {
                        new Entities.CampaignItemTemplateOptoutGroup
                        {
                            GroupID = defaultId
                        }
                    },
                    OptOutSpecificGroup = true,
                    OptOutMasterSuppression = true
                };
            };
            ShimCampaignItemBlast.Save_UseAmbientTransactionInt32ListOfCampaignItemBlastUser = (campaignItemID, newList, user) =>
            {

            };
            ShimCampaignItemSuppression.Delete_NoAccessCheckInt32UserBoolean = (campaignItemID, user, overrideUpdate) =>
            {
            };
            ShimCampaignItemSuppression.Save_UseAmbientTransactionCampaignItemSuppressionUser = (suppression, user) =>
            {
                return defaultId;
            };
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean = (campaignItemID, user, checkFirst, keepPaused) =>
            {
            };
            ShimCampaignItemBlast.GetByCampaignItemIDInt32UserBoolean = (campaignItemID, user, getChildren) =>
            {
                return new List<Entities.CampaignItemBlast>();
            };
            ShimCampaignItemBlastRefBlast.DeleteInt32UserBoolean = (campaignItemBlastID, currentUser, overrideUpdate) => { };
            ShimCampaignItemOptOutGroup.SaveCampaignItemOptOutGroupUser = (campaignItemOptOutGroup, user) =>
            {
                _savedCampaignItemOptOutGroup = campaignItemOptOutGroup;

            };

            var shimDiagramsController = new ShimAutomationBaseController(_diagramsController);
            shimDiagramsController.SaveLinkTrackingParamOptionsCampaignItem = (campaignItem) =>
            {
            };
        }
    }
}