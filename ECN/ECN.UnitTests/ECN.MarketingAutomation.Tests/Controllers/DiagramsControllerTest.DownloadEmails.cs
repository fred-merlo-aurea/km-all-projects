using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using Controls = ecn.MarketingAutomation.Models.PostModels.Controls;
using Entities = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Newtonsoft.Json;
using PostModels = ecn.MarketingAutomation.Models.PostModels;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    [TestFixture]
    public partial class DiagramsControllerTest
    {
        private const string _downloadEmailsMethodName = "DownloadEmails";
        private DataTable _downloadEmailsTestDataTable;
        private List<Entities.MAControl> _maControlsForDownloadEmails;
        private List<Entities.MAConnector> _maConnectorsForDownloadEmails;
        private List<PostModels.ControlBase> _allControlsForDownloadEmails;
        private List<PostModels.Connector> _allConnectosForDownloadEmails;

        [Test]
        [TestCase("CampaignItem", true)]
        [TestCase("Click", true)]
        [TestCase("Direct_Click", true)]
        [TestCase("FormAbandon", true)]
        [TestCase("FormSubmit", true)]
        [TestCase("Form", true)]
        [TestCase("Form", true, MarketingAutomationControlType.FormSubmit)]
        [TestCase("Form", true, MarketingAutomationControlType.Direct_NoOpen)]
        [TestCase("Direct_Open", true)]
        [TestCase("Direct_NoOpen", true)]
        [TestCase("Group", true)]
        [TestCase("NoClick", true)]
        [TestCase("NoOpen", true)]
        [TestCase("NotSent", true)]
        [TestCase("Open", true)]
        [TestCase("Open_NoClick", true)]
        [TestCase("Sent", true)]
        [TestCase("Subscribe", true)]
        [TestCase("Suppressed", true)]
        [TestCase("Unsubscribe", true)]
        [TestCase("CampaignItem", false)]
        [TestCase("Click", false)]
        [TestCase("Direct_Click", false)]
        [TestCase("Form", false)]
        [TestCase("FormAbandon", false)]
        [TestCase("FormSubmit", false)]
        [TestCase("Direct_Open", false)]
        [TestCase("Direct_NoOpen", false)]
        [TestCase("Group", false)]
        [TestCase("NoClick", false)]
        [TestCase("NoOpen", false)]
        [TestCase("NotSent", false)]
        [TestCase("Open", false)]
        [TestCase("Open_NoClick", false)]
        [TestCase("Sent", false)]
        [TestCase("Subscribe", false)]
        [TestCase("Suppressed", false)]
        [TestCase("Unsubscribe", false)]
        public void DownloadEmails_ControlType_ReturnsFileContentResult(string controlType, bool isWait, MarketingAutomationControlType parentControlType = MarketingAutomationControlType.CampaignItem)
        {
            // Arrange 
            int ecnid;
            int mAID;
            string controlId;
            //SaveSmartSegmentEmail_Click is shimmed in main Setup file by another developer, so creating new _shimsContext
            _shimsContext.Dispose();
            _shimsContext = ShimsContext.Create();
            _setupForDownloadEmails(out ecnid, out mAID, out controlId, parentControlType);
            var expectedFileContent = ASCIIEncoding.ASCII.GetBytes("\"emailaddress\"\t\"email\"\r\n\"test@test.com\"\t\"test@test.com\"\r\n\"test@test.com\"\t\"test@test.com\"\r\n");
            var expectedFileContentResult = new FileContentResult(expectedFileContent, "application/vnd.ms-excel");

            // Act
            FileContentResult  fileContentResult=(FileContentResult)_diagramsControllerPrivateObject.Invoke(_downloadEmailsMethodName, ECNID, controlType, isWait, mAID, controlId);

            // Assert
            fileContentResult.ShouldSatisfyAllConditions(
                    () => fileContentResult.FileContents.Length.ShouldBe(expectedFileContentResult.FileContents.Length),
                    () => fileContentResult.FileContents.ShouldAllBe(returnedByte => expectedFileContentResult.FileContents.Contains(returnedByte)),
                    () => fileContentResult.ContentType.ShouldBe(expectedFileContentResult.ContentType)
                );
        }
        private void _setAllConnectorsProperty(List<PostModels.Connector> allConnectors)
        {
            const string AllConnectorsPropertyName = "AllConnectors";
            _diagramsControllerPrivateObject.SetProperty(AllConnectorsPropertyName, allConnectors);
        }
        private void _setAllControlsProperty(List<PostModels.ControlBase> allControls)
        {
            const string AllControlsPropertyName = "AllControls";
            _diagramsControllerPrivateObject.SetProperty(AllControlsPropertyName, allControls);
        }
        private void _setupForDownloadEmails(out int ecnid, out int mAID, out string controlId, MarketingAutomationControlType parentControlType)
        {
            var defaultId = 1;
            var defaultStringId = "1";

            ecnid = defaultId;
            mAID = defaultId;
            controlId = defaultStringId;
            _downloadEmailsTestDataTable = new DataTable();
            const string testEmailAddress = "test@test.com";
            _downloadEmailsTestDataTable.Columns.AddRange(new DataColumn[] {
                    new DataColumn("emailaddress"),
                    new DataColumn("email"),
                    new DataColumn("IDColumn1"),
                    new DataColumn("IDColumn2"),
                });
            _downloadEmailsTestDataTable.Rows.Add(testEmailAddress, testEmailAddress, defaultId, defaultId);
            _downloadEmailsTestDataTable.Rows.Add(testEmailAddress, testEmailAddress, defaultId, defaultId);

            var instance = Activator.CreateInstance("ecn.MarketingAutomation", $"ecn.MarketingAutomation.Models.PostModels.Controls.{parentControlType.ToString()}");
            var parentControl= (Controls.CampaignControlBase)instance.Unwrap();
            parentControl.ControlID = "2";

            _setAllConnectorsProperty(_allConnectosForDownloadEmails);
            _setAllControlsProperty(_allControlsForDownloadEmails);
            
            _allControlsForDownloadEmails = new List<PostModels.ControlBase>
            {
                new Controls.Form
                {
                    ControlID = defaultStringId,
                    ActualLink = "test link"
                }, parentControl
            };
            _allConnectosForDownloadEmails = new List<PostModels.Connector>
            {
               new PostModels.Connector
               {
                   from = new PostModels.from{shapeId = "2" },
                   to = new PostModels.to{ shapeId = defaultStringId}
               }
            };

            var wait = new Entities.MAControl
            {
                ControlID = defaultStringId,
                ECNID = defaultId
            };
            var smartSegment = new Entities.MAControl
            {
                ControlID = defaultStringId,
                ECNID = defaultId,
                ControlType = parentControlType
            };
            _maControlsForDownloadEmails = new List<Entities.MAControl>
            {
                smartSegment, wait
            };
            _maConnectorsForDownloadEmails = new List<Entities.MAConnector> {
                new Entities.MAConnector
                {
                    To = smartSegment.ControlID,
                    From  = smartSegment.ControlID
                }
            };

            ShimCampaignItem.GetByCampaignItemID_NoAccessCheckInt32Boolean = (campaignItemID, getChildren) =>
            {
                return new Entities.CampaignItem
                {
                    CustomerID = defaultId
                };
            };
            ShimBlastActivity.DownloadBlastReportDetailsInt32BooleanStringStringStringInt32UserStringStringStringBoolean = (id, isCampaignItem, reportType, filterType, isp, customerID, user, startDate, endDate, profileFilter, onlyUnique) =>
            {
                return _downloadEmailsTestDataTable;
            };
            ShimBlastSingle.DownloadEmailLayoutPlanID_ProcessedInt32StringUser = (layoutPlanID, processed, user) =>
            {
                return _downloadEmailsTestDataTable;
            };
            
            ShimMarketingAutomation.GetByMarketingAutomationIDInt32BooleanUser = (maid, getChildren, user) =>
            {
                return new Entities.MarketingAutomation
                {
                    Controls = _maControlsForDownloadEmails,
                    Connectors = _maConnectorsForDownloadEmails,  
                    JSONDiagram = $"{{shapes: [], connections: {JsonConvert.SerializeObject(_allConnectosForDownloadEmails)}}}"
                };
            };
            ShimBlastActivity.DownloadBlastLinkDetailsInt32StringInt32UserStringStringStringBoolean = (id, linkAlias, customerID, user, startDate, endDate, profileFilter, onlyUnique) =>
            {
                return _downloadEmailsTestDataTable;
            };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (blastID, getChildren) =>
            {
                return new Entities.CampaignItem
                {
                    CampaignItemID = defaultId,
                    CustomerID = defaultId
                };
            };

            ShimControlBase.GetModelsFromObjectListOfMAControlListOfMAConnectorListOfControlBaseUser = (controls, connectors, originalControls, user) =>
            {
                return _allControlsForDownloadEmails;
            };
            ShimECNSession shimECNSession = new ShimECNSession();
            shimECNSession.Instance.CurrentUser = new User()
            {
                UserID = defaultId
            };
            ShimECNSession.CurrentSession = () => shimECNSession;

            ShimLayoutPlans.GetByLayoutPlanIDInt32User = (layoutPlanID, user) =>
            {
                return new Entities.LayoutPlans
                {
                    BlastID = defaultId,
                    CustomerID = defaultId,
                    LayoutPlanID = defaultId
                };
            };
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (triggerPlanID, user) =>
            {
                return new Entities.TriggerPlans
                {
                    BlastID = defaultId,
                    CustomerID = defaultId,
                    TriggerPlanID = defaultId
                };
            };
            ShimGroup.GetByGroupIDInt32User = (groupID, user) =>
            {
                return new Entities.Group
                {
                    CustomerID = defaultId
                };
            };
            ShimEmailGroup.GetGroupEmailProfilesWithUDFInt32Int32StringStringString = (groupID, customerID, filter, subscribeType, profFilter) =>
            {
                return _downloadEmailsTestDataTable;
            };
        }
    }
}