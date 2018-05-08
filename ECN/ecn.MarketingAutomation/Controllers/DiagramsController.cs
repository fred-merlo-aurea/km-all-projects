using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using ecn.MarketingAutomation.Models;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Activity;
using ECN_Framework_BusinessLayer.Activity.View;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using Newtonsoft.Json;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessLayerGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using CampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem;
using CommonEnum = ECN_Framework_Common.Objects.Enums;
using EntityCommunicator = ECN_Framework_Entities.Communicator;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using FrameworkCommonObj = ECN_Framework_Common.Objects.Communicator;
using Group = ecn.MarketingAutomation.Models.PostModels.Controls.Group;
using PostModelControl = ecn.MarketingAutomation.Models.PostModels.Controls;

namespace ecn.MarketingAutomation.Controllers
{
    public class DiagramsController : AutomationBaseController
    {
        private const string AutomationMissingMessage = "Automation is missing start or end controls";
        private const string AutomationNotSaveMessage = "Could not save automation";
        private const string DiagramSaveMessage = "Diagram saved successfully";
        private const string ActionSaveControls = "Save Controls";
        private const string ActionPublish = "Publish";
        private const string StatusCode500 = "500";
        private const string StatusCode200 = "200";
        private const string ActionNameIndex = "Index";
        private const string ControllerNameHome = "Home";
        private const string TrueStr = "true";
        private const string ReportTypeSend = "send";
        private const string ReportTypeBounce = "bounce";
        private const string ReportTypeDelivered = "delivered";
        private const string ReportTypeUniqueClicks = "uniqueclicks";
        private const string ReportTypeNoClicks = "noclick";
        private const string ReportTypeNoOpen = "noopen";
        private const string ReportTypeNotSent = "notsent";
        private const string ReportTypeOpen = "open";
        private const string ReportTypeOpenNoClick = "open_noclick";
        private const string ReportTypeSuppressed = "suppressed";
        private const string ProcessedValue = "n";
        private const string ColumnNameDistinctCount = "DistinctCount";
        private const string ColumnNameActionTypeCode = "ActionTypeCode";
        private const string ColumnNameSubscribed = "subscribed";
        private const string ColumnNameUnsubscribed = "unsubscribed";
        private const string ColumnNameSuppressed = "suppressed";
        private const string ColumnNameEmailAddress = "emailaddress";
        private const string ColumnNameEmail = "email";
        private const string ControlTypeClickThrough = "clickthrough";
        private const string ControlTypeNoClick = "noclick";
        private const string ControlTypeNoOpen = "noopen";
        private const string ControlTypeNotSent = "notsent";
        private const string ControlTypeOpen = "open";
        private const string ControlTypeOpenNoClick = "open_noclick";
        private const string ControlTypeSend = "send";
        private const string ControlTypeSuppressed = "suppressed";
        private const string EmptyString = "";
        private const string FileNamePostFixWait = "_waiting_emails";
        private const string FileNamePostFixDelivered = "_delivered";
        private const string FileNamePostFixClicks = "_clicks";
        private const string FileNamePostFixNoClicks = "_noclicks";
        private const string FileNamePostFixNoOpens = "_noopens";
        private const string FileNamePostFixNotSent = "_notsent";
        private const string FileNamePostFixOpens = "_opens";
        private const string FileNamePostFixOpenNoClicks = "_open_noclicks";
        private const string FileNamePostFixSuppressed = "_suppressed";
        private const string FileNamePostFixSubscribes = "_subscribes";
        private const string FileNamePostFixUnsubscribes = "_unsubscribes";
        private const string FileNamePostFixFormAbandon = "_FormAbandon";
        private const string FileNamePostFixFormSubmit = "_FormSubmit";
        private const string FileNamePostFixDirectClicks = "_direct_clicks";
        private const string FileNamePostFixDirectOpens = "_direct_opens";
        private const string FileNamePostFixDirectNoOpens = "_direct_noopens";
        private const string FileNamePostFixGroupEmails = "_group_emails";
        private const string FileNamePostFixSent = "_sent";
        private const string FileNamePostFixFormClick = "_FormClick";
        private const string ContentTypeExcel = "application/vnd.ms-excel";
        private const string ExtensionXls = ".xls";
        private const string FilterProfileOnly = "ProfileOnly";
        private const string SubscribeTypeS = "'S'";

        private ECN_Framework_BusinessLayer.Application.ECNSession CurrentSession
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession(); }
        }

        private int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }

        private int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }

        private int CurrentCustomerID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID; }
        }

        public ActionResult Edit(int id = 0)
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.MARKETINGAUTOMATION, KMPlatform.Enums.ServiceFeatures.MarketingAutomation, KMPlatform.Enums.Access.FullAccess))
            {
                ViewBag.Customers = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().BaseChannelID);
                DiagramViewModel dvm = new DiagramViewModel();
                ECN_Framework_Entities.Communicator.MarketingAutomation ma = new ECN_Framework_Entities.Communicator.MarketingAutomation();
                Models.PostModels.MarketingAutomationPostModel mapm = new Models.PostModels.MarketingAutomationPostModel();
                if (id != 0)
                {
                    try
                    {
                        // Load Json Diagram
                        ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(id, true, CurrentUser);

                        mapm.JSONDiagram = ma.JSONDiagram;
                        mapm.EndDate = ma.EndDate.Value;
                        mapm.Goal = ma.Goal;
                        mapm.MarketingAutomationID = ma.MarketingAutomationID;
                        mapm.Name = ma.Name;
                        mapm.StartDate = ma.StartDate.Value;
                        mapm.State = ma.State;
                        mapm.Connectors = Models.PostModels.Connector.GetPostModelFromObject(ma.Connectors);

                        string jsonDiagram = "";
                        Models.PostModels.DiagramJsonObject djo = new Models.PostModels.DiagramJsonObject();

                        djo = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.PostModels.DiagramJsonObject>(ma.JSONDiagram, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

                        mapm.Controls = Models.PostModels.ControlBase.GetModelsFromObject(ma.Controls, ma.Connectors, djo.shapes, CurrentUser);
                        if (mapm.Controls.Count == 0 && mapm.Connectors.Count == 0)
                        {

                            jsonDiagram = Models.PostModels.ControlBase.GetJSONDiagramFromControls(djo.shapes, djo.connections, mapm.MarketingAutomationID);
                        }
                        else
                        {
                            jsonDiagram = Models.PostModels.ControlBase.Serialize(mapm.Controls, mapm.Connectors, id);
                        }

                        mapm.JSONDiagram = jsonDiagram;
                        mapm.CustomerID = CurrentCustomerID;
                        mapm.CustomerName = CurrentSession.CurrentCustomer.CustomerName;
                        mapm.CreatedDate = ma.CreatedDate.Value;
                        mapm.CreatedUserID = ma.CreatedUserID.Value;
                        mapm.IsDeleted = ma.IsDeleted;
                        mapm.JSONDeleted = "";

                        return View(mapm);
                    }
                    catch (ECN_Framework_Common.Objects.SecurityException se)
                    {
                        return RedirectToAction("Index", "Home", new { se = "true" });
                    }
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DownloadEmails(
            int ECNID, 
            string controlType, 
            bool isWait, 
            int mAID, 
            string controlID = EmptyString)
        {
            var dataTable = new DataTable();
            var fileName = string.Empty;

            if (string.IsNullOrWhiteSpace(controlType))
            {
                throw new ArgumentNullException(nameof(controlType));
            }

            var automationControlType = (Enums.MarketingAutomationControlType) Enum.Parse(
                typeof(Enums.MarketingAutomationControlType),
                controlType);
            
            switch (automationControlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                case Enums.MarketingAutomationControlType.Click:
                case Enums.MarketingAutomationControlType.NoClick:
                case Enums.MarketingAutomationControlType.NoOpen:
                case Enums.MarketingAutomationControlType.NotSent:
                case Enums.MarketingAutomationControlType.Open:
                case Enums.MarketingAutomationControlType.Open_NoClick:
                case Enums.MarketingAutomationControlType.Suppressed:
                    dataTable = GetEmailDataCampaignItem(automationControlType, ECNID, isWait, ref fileName);
                    break;
                case Enums.MarketingAutomationControlType.Form:
                    dataTable = GetEmailDataForm(mAID, controlID, out fileName);
                    break;
                case Enums.MarketingAutomationControlType.FormAbandon:
                case Enums.MarketingAutomationControlType.FormSubmit:
                    dataTable = GetEmailDataFormSubmitAbandon(automationControlType, ECNID, isWait, ref fileName);
                    break;
                case Enums.MarketingAutomationControlType.Direct_Click:
                case Enums.MarketingAutomationControlType.Direct_Open:
                case Enums.MarketingAutomationControlType.Direct_NoOpen:
                    dataTable = GetEmailDataDirectOpenNoOpen(automationControlType, ECNID, isWait, ref fileName);
                    break;
                case Enums.MarketingAutomationControlType.Group:
                    dataTable = GetEmailDataGroup(automationControlType, ECNID, out fileName);
                    break;
                case Enums.MarketingAutomationControlType.Sent:
                    dataTable = GetEmailDataSent(automationControlType, ECNID, out fileName);
                    break;
                case Enums.MarketingAutomationControlType.Subscribe:
                case Enums.MarketingAutomationControlType.Unsubscribe:
                    dataTable = GetEmailDataForSubscribe(automationControlType, ECNID, isWait, ref fileName);
                    break;
            }

            var columnCount = dataTable.Columns.Count;
            for (var index = columnCount - 1; index >= 0; index--)
            {
                var dataColumn = dataTable.Columns[index];
                if(!dataColumn.ColumnName.Equals(ColumnNameEmailAddress, StringComparison.OrdinalIgnoreCase) &&
                   !dataColumn.ColumnName.Equals(ColumnNameEmail, StringComparison.OrdinalIgnoreCase))
                {
                    dataTable.Columns.Remove(dataColumn);
                }
            }

            if (!isWait)
            {
                fileName = $"{ECNID}{FileNamePostFixWait}";
            }

            var tabDelimited = DataTableFunctions.ToTabDelimited(dataTable);
            var fileContents = Encoding.ASCII.GetBytes(tabDelimited);

            return File(fileContents, ContentTypeExcel, $"{fileName}{ExtensionXls}");
        }

        private DataTable GetEmailDataCampaignItem(
            Enums.MarketingAutomationControlType controlType,
            int ecnId, 
            bool isWait, 
            ref string fileName)
        {
            var dataTable = default(DataTable);
            var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(ecnId, false);
            var reportType = GetReportTypeForReportDetail(controlType, isWait);

            if (!isWait)
            {
                fileName = $"{ecnId}{GetEmailFilenamePostFix(controlType)}";
            }

            if (campaignItem.CustomerID != null)
            {
                dataTable = BlastActivity.DownloadBlastReportDetails(
                    ecnId,
                    true,
                    reportType,
                    string.Empty,
                    string.Empty,
                    campaignItem.CustomerID.Value,
                    CurrentUser,
                    string.Empty,
                    string.Empty,
                    FilterProfileOnly);
            }

            return dataTable;
        }

        private DataTable GetEmailDataForm(int automationId, string controlId, out string fileName)
        {
            var dataTable = default(DataTable);
            var campaignItem = default(EntityCommunicator.CampaignItem);

            var marketingAutomation = BusinessCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                automationId,
                true,
                CurrentUser);

            var jsonObject = JsonConvert.DeserializeObject<DiagramJsonObject>(
                marketingAutomation.JSONDiagram,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var controls = ControlBase.GetModelsFromObject(
                marketingAutomation.Controls,
                marketingAutomation.Connectors,
                jsonObject.shapes,
                CurrentUser);

            var connectors = jsonObject.connections;
            AllConnectors = connectors;
            AllControls = controls;

            var formControl = controls.OfType<Form>().First(form => form.ControlID == controlId);
            var parentControl = ControlBase.GetParent(formControl, controls, connectors);
            var campaignItemId = 0;
            switch (parentControl.ControlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                case Enums.MarketingAutomationControlType.NoClick:
                case Enums.MarketingAutomationControlType.Click:
                case Enums.MarketingAutomationControlType.NoOpen:
                case Enums.MarketingAutomationControlType.Open_NoClick:
                case Enums.MarketingAutomationControlType.Open:
                case Enums.MarketingAutomationControlType.NotSent:
                case Enums.MarketingAutomationControlType.Sent:
                case Enums.MarketingAutomationControlType.Suppressed:
                    campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(parentControl.ECNID, false);
                    campaignItemId = campaignItem.CampaignItemID;
                    break;
                case Enums.MarketingAutomationControlType.Direct_Click:
                case Enums.MarketingAutomationControlType.Direct_Open:
                case Enums.MarketingAutomationControlType.FormSubmit:
                case Enums.MarketingAutomationControlType.FormAbandon:
                case Enums.MarketingAutomationControlType.Subscribe:
                case Enums.MarketingAutomationControlType.Unsubscribe:
                    var layoutPlans = LayoutPlans.GetByLayoutPlanID(FindParent(formControl).ECNID, CurrentUser);
                    if (layoutPlans.BlastID != null)
                    {
                        campaignItem = CampaignItem.GetByBlastID_NoAccessCheck(layoutPlans.BlastID.Value, false);
                        campaignItemId = campaignItem.CampaignItemID;
                    }
                    break;
                case Enums.MarketingAutomationControlType.Direct_NoOpen:
                    var triggerPlans = TriggerPlans.GetByTriggerPlanID(FindParent(formControl).ECNID, CurrentUser);
                    if (triggerPlans.BlastID != null)
                    {
                        campaignItem = CampaignItem.GetByBlastID_NoAccessCheck(triggerPlans.BlastID.Value, false);
                        campaignItemId = campaignItem.CampaignItemID;
                    }
                    break;
            }

            if (campaignItem?.CustomerID != null)
            {
                dataTable = BlastActivity.DownloadBlastLinkDetails(
                    campaignItemId,
                    formControl.ActualLink,
                    campaignItem.CustomerID.Value,
                    CurrentUser,
                    string.Empty,
                    string.Empty,
                    FilterProfileOnly);
            }
            fileName = $"{campaignItemId}{GetEmailFilenamePostFix(Enums.MarketingAutomationControlType.Form)}";

            return dataTable;
        }

        private DataTable GetEmailDataFormSubmitAbandon(
            Enums.MarketingAutomationControlType controlType,
            int ecnId,
            bool isWait,
            ref string fileName)
        {
            var dataTable = default(DataTable);
            var layoutPlans = LayoutPlans.GetByLayoutPlanID(ecnId, CurrentUser);
            if (!isWait)
            {
                if (layoutPlans.BlastID != null && layoutPlans.CustomerID != null)
                {
                    dataTable = BlastActivity.DownloadBlastReportDetails(
                        layoutPlans.BlastID.Value,
                        false,
                        ReportTypeSend,
                        string.Empty,
                        string.Empty,
                        layoutPlans.CustomerID.Value,
                        CurrentUser,
                        string.Empty,
                        string.Empty,
                        FilterProfileOnly);

                    var campaignId = CampaignItem.GetByBlastID_NoAccessCheck(
                        layoutPlans.BlastID.Value,
                        false).CampaignItemID;

                    fileName = $"{campaignId}{GetEmailFilenamePostFix(controlType)}";
                }
            }
            else
            {
                dataTable = BlastSingle.DownloadEmailLayoutPlanID_Processed(
                    layoutPlans.LayoutPlanID,
                    ProcessedValue,
                    CurrentUser);
            }

            return dataTable;
        }

        private DataTable GetEmailDataDirectOpenNoOpen(
            Enums.MarketingAutomationControlType controlType,
            int ecnId,
            bool isWait,
            ref string fileName)
        {
            var dataTable = default(DataTable);
            var triggerPlans = TriggerPlans.GetByTriggerPlanID(ecnId, CurrentUser);
            if (!isWait)
            {
                if (triggerPlans.BlastID != null && triggerPlans.CustomerID != null)
                {
                    dataTable = BlastActivity.DownloadBlastReportDetails(
                        triggerPlans.BlastID.Value,
                        false,
                        ReportTypeSend,
                        string.Empty,
                        string.Empty,
                        triggerPlans.CustomerID.Value,
                        CurrentUser,
                        string.Empty,
                        string.Empty,
                        FilterProfileOnly);
                }

                fileName = $"{ecnId}{GetEmailFilenamePostFix(controlType)}";
            }
            else
            {
                dataTable = BlastSingle.DownloadEmailLayoutPlanID_Processed(
                    triggerPlans.TriggerPlanID, 
                    ProcessedValue, 
                    CurrentUser);
            }

            return dataTable;
        }

        private DataTable GetEmailDataGroup(
            Enums.MarketingAutomationControlType controlType,
            int ecnId,
            out string fileName)
        {
            var groupObject = BusinessLayerGroup.GetByGroupID(ecnId, CurrentUser);
            var dataTable = EmailGroup.GetGroupEmailProfilesWithUDF(
                ecnId,
                groupObject.CustomerID,
                string.Empty,
                SubscribeTypeS,
                FilterProfileOnly);

            fileName = $"{ecnId}{GetEmailFilenamePostFix(controlType)}";

            return dataTable;
        }

        private DataTable GetEmailDataSent(
            Enums.MarketingAutomationControlType controlType,
            int ecnId,
            out string fileName)
        {
            var dataTable = default(DataTable);
            var campaignItem = CampaignItem.GetByCampaignItemID_NoAccessCheck(ecnId, false);

            if (campaignItem.CustomerID != null)
            {
                dataTable = BlastActivity.DownloadBlastReportDetails(
                    ecnId,
                    true,
                    ReportTypeSend,
                    string.Empty,
                    string.Empty,
                    campaignItem.CustomerID.Value,
                    CurrentUser,
                    string.Empty,
                    string.Empty,
                    FilterProfileOnly);
            }

            fileName = $"{ecnId}{GetEmailFilenamePostFix(controlType)}";

            return dataTable;
        }

        private DataTable GetEmailDataForSubscribe(
            Enums.MarketingAutomationControlType controlType,
            int ecnId,
            bool isWait,
            ref string fileName)
        {
            var dataTable = default(DataTable);
            var layoutPlans = LayoutPlans.GetByLayoutPlanID(ecnId, CurrentUser);
            if (!isWait)
            {
                if (layoutPlans.BlastID != null && layoutPlans.CustomerID != null)
                {
                    dataTable = BlastActivity.DownloadBlastReportDetails(
                        layoutPlans.BlastID.Value,
                        false,
                        ReportTypeSend,
                        string.Empty,
                        string.Empty,
                        layoutPlans.CustomerID.Value,
                        CurrentUser,
                        string.Empty,
                        string.Empty,
                        FilterProfileOnly);

                    var campaignId = CampaignItem.GetByBlastID_NoAccessCheck(
                        layoutPlans.BlastID.Value,
                        false).CampaignItemID;

                    fileName = $"{campaignId}{GetEmailFilenamePostFix(controlType)}";
                }
            }
            else
            {
                dataTable = BlastSingle.DownloadEmailLayoutPlanID_Processed(
                    layoutPlans.LayoutPlanID,
                    ProcessedValue,
                    CurrentUser);
            }

            return dataTable;
        }

        private string GetEmailFilenamePostFix(
            Enums.MarketingAutomationControlType controlType)
        {
            var filePostFix = string.Empty;
            switch (controlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                    filePostFix = FileNamePostFixDelivered;
                    break;
                case Enums.MarketingAutomationControlType.Click:
                    filePostFix = FileNamePostFixClicks;
                    break;
                case Enums.MarketingAutomationControlType.NoClick:
                    filePostFix = FileNamePostFixNoClicks;
                    break;
                case Enums.MarketingAutomationControlType.NoOpen:
                    filePostFix = FileNamePostFixNoOpens;
                    break;
                case Enums.MarketingAutomationControlType.NotSent:
                    filePostFix = FileNamePostFixNotSent;
                    break;
                case Enums.MarketingAutomationControlType.Open:
                    filePostFix = FileNamePostFixOpens;
                    break;
                case Enums.MarketingAutomationControlType.Open_NoClick:
                    filePostFix = FileNamePostFixOpenNoClicks;
                    break;
                case Enums.MarketingAutomationControlType.Suppressed:
                    filePostFix = FileNamePostFixSuppressed;
                    break;
                case Enums.MarketingAutomationControlType.Subscribe:
                    filePostFix = FileNamePostFixSubscribes;
                    break;
                case Enums.MarketingAutomationControlType.Unsubscribe:
                    filePostFix = FileNamePostFixUnsubscribes;
                    break;
                case Enums.MarketingAutomationControlType.FormAbandon:
                    filePostFix = FileNamePostFixFormAbandon;
                    break;
                case Enums.MarketingAutomationControlType.FormSubmit:
                    filePostFix = FileNamePostFixFormSubmit;
                    break;
                case Enums.MarketingAutomationControlType.Direct_Click:
                    filePostFix = FileNamePostFixDirectClicks;
                    break;
                case Enums.MarketingAutomationControlType.Direct_Open:
                    filePostFix = FileNamePostFixDirectOpens;
                    break;
                case Enums.MarketingAutomationControlType.Direct_NoOpen:
                    filePostFix = FileNamePostFixDirectNoOpens;
                    break;
                case Enums.MarketingAutomationControlType.Group:
                    filePostFix = FileNamePostFixGroupEmails;
                    break;
                case Enums.MarketingAutomationControlType.Sent:
                    filePostFix = FileNamePostFixSent;
                    break;
                case Enums.MarketingAutomationControlType.Form:
                    filePostFix = FileNamePostFixFormClick;
                    break;
            }

            return filePostFix;
        }

        private string GetReportTypeForReportDetail(Enums.MarketingAutomationControlType controlType, bool isWait)
        {
            var reportType = string.Empty;
            switch (controlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                    reportType = isWait
                        ? ReportTypeSend
                        : ReportTypeDelivered;
                    break;
                case Enums.MarketingAutomationControlType.Click:
                    reportType = isWait
                        ? ReportTypeUniqueClicks
                        : ReportTypeSend;
                    break;
                case Enums.MarketingAutomationControlType.NoClick:
                    reportType = isWait
                        ? ReportTypeNoClicks
                        : ReportTypeSend;
                    break;
                case Enums.MarketingAutomationControlType.NoOpen:
                    reportType = isWait
                        ? ReportTypeNoOpen
                        : ReportTypeSend;
                    break;
                case Enums.MarketingAutomationControlType.NotSent:
                    reportType = isWait
                        ? ReportTypeNotSent
                        : ReportTypeSend;
                    break;
                case Enums.MarketingAutomationControlType.Open:
                    reportType = isWait
                        ? ReportTypeOpen
                        : ReportTypeSend;
                    break;
                case Enums.MarketingAutomationControlType.Open_NoClick:
                    reportType = isWait
                        ? ReportTypeOpenNoClick
                        : ReportTypeSend;
                    break;
                case Enums.MarketingAutomationControlType.Suppressed:
                    reportType = isWait
                        ? ReportTypeSuppressed
                        : ReportTypeSend;
                    break;
            }

            return reportType;
        }

        public ActionResult Heatmap(int id = 0)
        {
            DiagramViewModel dvm = new DiagramViewModel();
            if (id != 0)
            {
                ECN_Framework_Entities.Communicator.MarketingAutomation ma = ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.GetByMarketingAutomationID(id, true, CurrentUser);
                Models.PostModels.DiagramJsonObject djo = new Models.PostModels.DiagramJsonObject();

                djo = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.PostModels.DiagramJsonObject>(ma.JSONDiagram, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

                List<Models.PostModels.ControlBase> controls = Models.PostModels.ControlBase.GetModelsFromObject(ma.Controls, ma.Connectors, djo.shapes, CurrentUser);
                List<Models.PostModels.Connector> connectors = djo.connections;
                controls = GetHeatMapStats(controls, ref connectors);
                // List<Models.PostModels.Connector> connectors = Models.PostModels.Connector.GetPostModelFromObject(ma.Connectors);
                dvm.Diagram = Models.PostModels.ControlBase.Serialize(controls, connectors, id);
                dvm.EndDate = ma.EndDate;
                dvm.Goal = ma.Goal;
                dvm.Id = ma.MarketingAutomationID;
                dvm.Name = ma.Name;
                dvm.StartDate = ma.StartDate;
                dvm.Status = ma.State;
                dvm.Type = ma.Type;
                // Load Json Diagram
                return View(dvm);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        private List<string> GetCampaignReportList()
        {
            return new List<string>
            {
                FrameworkCommonObj.Enums.MarketingAutomationControlType.CampaignItem.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Click.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.NoClick.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.NoOpen.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.NotSent.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Open.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Open_NoClick.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Sent.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Suppressed.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Wait.ToString()
            };
        }

        private List<string> GetLayoutList()
            {
            return new List<string>
            {
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Click.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Open.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.FormAbandon.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.FormSubmit.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_NoOpen.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Subscribe.ToString(),
                FrameworkCommonObj.Enums.MarketingAutomationControlType.Unsubscribe.ToString()
            };
        }

        private List<string> SuppressCodeList()
                {
            return new List<string>
                    {
                "Threshold",
                "7Day",
                "Priority",
                "List",
                "Group",
                "Channel",
                "Domain",
                "InvalidEmail",
                "Global"
            };
                    }

        private bool GetBlastMarReportData(int campaignItemId, out DataTable dtReport, out DataTable dtReportBounces)
                    {
            dtReport = BlastActivity.GetBlastMAReportDataByCampaignItemID(campaignItemId, ReportTypeSend);
            dtReportBounces = BlastActivity.GetBlastMAReportDataByCampaignItemID(campaignItemId, ReportTypeBounce);
            return true;
        }

        private int GetCampaignItemId_NoAccessCheck<T>(int planId)
                        {
            var blastId = 0;
            if (typeof(LayoutPlans).IsAssignableFrom(typeof(T)))
            {
                var plan = LayoutPlans.GetByLayoutPlanID(planId, CurrentUser);
                if (plan.BlastID != null)
                {
                    blastId = plan.BlastID.Value;
                        }
            }
                        else
                        {
                var plan = TriggerPlans.GetByTriggerPlanID(planId, CurrentUser);
                if (plan.BlastID != null)
                {
                    blastId = plan.BlastID.Value;
                        }
                    }
            return CampaignItem.GetByBlastID_NoAccessCheck(blastId, false).CampaignItemID;
        }

        private int GetCampaignItemId(ControlBase control)
                    {
            var campaignItemId = 0;
            switch (control.ControlType)
                        {
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.CampaignItem:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.NoClick:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Click:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.NoOpen:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Open_NoClick:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Open:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.NotSent:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Sent:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Suppressed:
                    campaignItemId = control.ECNID;
                                break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Click:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Open:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.FormSubmit:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.FormAbandon:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Subscribe:
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Unsubscribe:
                    campaignItemId = GetCampaignItemId_NoAccessCheck<LayoutPlans>(control.ECNID);
                                break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_NoOpen:
                    campaignItemId = GetCampaignItemId_NoAccessCheck<TriggerPlans>(control.ECNID);
                                break;
                        }

            return campaignItemId;
        }

        private int GetEmailDataSends<T>(int planId)
        {
            var sends = 0;
            var dtEmails = new DataTable();
            if (typeof(T) == typeof(LayoutPlans))
            {
                var plan = LayoutPlans.GetByLayoutPlanID(planId, CurrentUser);
                if (plan != null)
                {
                    dtEmails = BlastSingle.DownloadEmailLayoutPlanID_Processed(
                        plan.LayoutPlanID, 
                        ProcessedValue, 
                        CurrentUser);
                }
            }
            else
            {
                var plan = TriggerPlans.GetByTriggerPlanID(planId, CurrentUser);
                if (plan != null)
                {
                    dtEmails = BlastSingle.DownloadEmailLayoutPlanID_Processed(
                        plan.TriggerPlanID, 
                        ProcessedValue, 
                        CurrentUser);
                }
            }
                
            if (dtEmails.Select().Length > 0)
            {
                sends = dtEmails.Select().Length;
            }
            return sends;
        }

        private int GetControlBlastCount(
            bool bBlastStatus,
            ControlBase maParent,
            string type,
            string actionTypeCode = null)
        {
            var sends = 0;
            if (bBlastStatus)
            {
                var dtReport = BlastActivity.GetBlastMAReportDataByCampaignItemID(
                    maParent.ECNID, 
                    type);

                var typeCode = actionTypeCode ?? type;
                var sendsDr = dtReport.Select($"{ColumnNameActionTypeCode} = '{typeCode}'");

                if (sendsDr.Length > 0)
                {
                    sends = Convert.ToInt32(sendsDr[0][ColumnNameDistinctCount].ToString());
                }
            }

            return sends;
        }

        private bool GetBlastStatus(ControlBase maParent, ControlBase child)
        {
            var blastStatus = false;
            var childBlastStatus = false;
            var parentCampaignId = CampaignItem.GetByCampaignItemID_NoAccessCheck(maParent.ECNID, true);
            if (parentCampaignId != null)
            {
                var blast = parentCampaignId.BlastList[0];
                if (blast.BlastID != null && blast.CustomerID != null)
                {
                    blastStatus = Blast.ActiveOrSent((int) blast.BlastID, (int) blast.CustomerID);

                    var childCampaignId = CampaignItem.GetByCampaignItemID_NoAccessCheck(child.ECNID, true);
                    if (childCampaignId != null)
                    {
                        var cBlast = childCampaignId.BlastList[0];
                        if (cBlast.BlastID != null && cBlast.CustomerID != null)
                        {
                            childBlastStatus = Blast.ActiveOrSent((int) cBlast.BlastID, (int) cBlast.CustomerID);
                        }
                    }
                }

                if (childBlastStatus)
                {
                    blastStatus = false;
                }
            }

            return blastStatus;
        }

        private int GetBlastCountForFormControl(
            ControlBase control, 
            ControlBase child,
            List<ControlBase> controls,
            List<Connector> connectors)
        {
            var maParentForm = ControlBase.GetParent(control, controls, connectors);
            var formParent = ControlBase.GetParent(maParentForm, controls, connectors);
            var form = maParentForm as Form;
            var campaignItemId = GetCampaignItemId(formParent);
            var fcCount = BlastActivityClicks.GetUniqueByURL(form.ActualLink, campaignItemId);
            return fcCount > 0 ? GetEmailDataSends<LayoutPlans>(child.ECNID) : 0;
        }

        private void AddControlForGroupType(ControlBase control, ref List<ControlBase> retList)
        {
            var groupControl = control as Group;
            if (groupControl != null)
            {
                var group = BusinessLayerGroup.GetByGroupID_NoAccessCheck(groupControl.ECNID);
                var dtGroupStats = EmailGroup.GetGroupStats(groupControl.ECNID, @group.CustomerID);
                groupControl.Subscribed = Convert.ToInt32(dtGroupStats.Rows[0][ColumnNameSubscribed].ToString());
                groupControl.Unsubscribed = Convert.ToInt32(dtGroupStats.Rows[0][ColumnNameUnsubscribed].ToString());
                groupControl.Suppressed = Convert.ToInt32(dtGroupStats.Rows[0][ColumnNameSuppressed].ToString());
            }

            retList.Add(groupControl);
        }
        
        private void GetBlastReportData(
            List<ControlBase> controls,
            ControlBase control,
            List<Connector> connectors,
            ref int sends,
            ref int bounces)
        {
            var bdtReport = false;
            var dtReport = new DataTable();
            var dtReportBounces = new DataTable();
            if (GetCampaignReportList().Contains(control.ControlType.ToString()))
            {
                bdtReport = GetBlastMarReportData(control.ECNID, out dtReport, out dtReportBounces);
            }
            else if (GetLayoutList().Contains(control.ControlType.ToString()))
            {
                var campaignItemId = 0;
                if (control.ControlType != FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_NoOpen)
                {
                    campaignItemId = GetCampaignItemId_NoAccessCheck<LayoutPlans>(control.ECNID);
                }
                else
                {
                    campaignItemId = GetCampaignItemId_NoAccessCheck<TriggerPlans>(control.ECNID);
                }

                bdtReport = GetBlastMarReportData(campaignItemId, out dtReport, out dtReportBounces);
            }
            else if (control.ControlType == FrameworkCommonObj.Enums.MarketingAutomationControlType.Form)
            {
                var parentControl = ControlBase.GetParent(control, controls, connectors);
                var form = control as Form;
                var campaignItemId = GetCampaignItemId(parentControl);
                if (form != null)
                {
                    sends = BlastActivityClicks.GetUniqueByURL(form.ActualLink, campaignItemId);
                }
                        bdtReport = false;
                    }
               
                    if(bdtReport)
                    {
                var sendsDr = dtReport.Select($"{ColumnNameActionTypeCode} = '{ReportTypeSend}'");
                var bounceDr = dtReportBounces.Select($"{ColumnNameActionTypeCode} = '{ReportTypeBounce}'");
                if (sendsDr.Length > 0)
                {
                    sends = Convert.ToInt32(sendsDr[0][ColumnNameDistinctCount].ToString());
                    }
                if (bounceDr.Length > 0)
                {
                    bounces = Convert.ToInt32(bounceDr[0][ColumnNameDistinctCount].ToString());
                }
            }
        }
               
        private void AddControlForOtherType(ControlBase control, int sends, int bounces, ref List<ControlBase> retList)
        {
                    switch (control.ControlType)
                    {
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.CampaignItem:
                            //unique deliveries
                    AddControl(control as PostModelControl.CampaignItem, sends - bounces, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Click:
                            //unique clicks for parent campaign/ or unique sends for click SS email
                    AddControl(control as Click, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Click:
                            //unique sends for click trigger
                    AddControl(control as Direct_Click, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Form:
                            //unique sends for click trigger
                    var form = (Form)control;
                            form.HeatMapStats = sends;
                            retList.Add(form);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.FormAbandon:
                            //unique sends for click trigger
                    AddControl(control as FormAbandon, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.FormSubmit:
                            //unique sends for click trigger
                    AddControl(control as FormSubmit, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Open:
                            //unique sends for open trigger
                    AddControl(control as Direct_Open, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_NoOpen:
                    AddControl(control as Direct_NoOpen, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Group:
                            //subscribe type code stats for group
                    AddControlForGroupType(control, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.NoClick:
                            //unique sends for NoClick email
                    AddControl(control as NoClick, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.NoOpen:
                            //unique sends for NoOpen email
                    AddControl(control as NoOpen, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.NotSent:
                            //unique sends for NotSent email
                    AddControl(control as NotSent, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Open:
                            //unique sends for Open email
                    AddControl(control as Open, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Open_NoClick:
                            //unique sends for Open/NoClick email
                    AddControl(control as Open_NoClick, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Sent:
                            //unique sends for Sent email
                    AddControl(control as Sent, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Subscribe:
                            //unique sends for Subscribe group trigger email
                    AddControl(control as Subscribe, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Suppressed:
                            //unique sends for Suppressed email
                    AddControl(control as Suppressed, sends, ref retList);
                            break;
                case FrameworkCommonObj.Enums.MarketingAutomationControlType.Unsubscribe:
                            //unique sends for Unsubscribe group trigger email
                    AddControl(control as Unsubscribe, sends, ref retList);
                            break;
                                        }
                                    }

        private void AddControl(CampaignControlBase control, int heatMapStats, ref List<ControlBase> retList)
                                            {
            control.HeatMapStats = heatMapStats;
            retList.Add(control);
                                            }

        private void AddControlForWaitType(
            ControlBase control,
            List<ControlBase> controls,
            ref List<Connector> connectors,
            ref List<ControlBase> retList)
                                            {
            var wait = control as Wait;
            var waitChildConn = GetWaitChildren(wait, controls, connectors);
            foreach (var waitconn in waitChildConn)
                                            {
                var childControl = ControlBase.GetChildconn(waitconn, controls);
                var maParent = ControlBase.GetParent(wait, controls, connectors);
                var sends = 0;
                if (maParent != null)
                                            {
                    var blastStatus = GetBlastStatus(maParent, childControl);
                    switch (childControl.ControlType)
                                            {
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.CampaignItem:
                            sends = blastStatus ? ControlBase.blastLicenseCount_Update(maParent.ECNID) : 0;
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Click:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeClickThrough);
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.NoClick:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeNoClick);
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.NoOpen:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeNoOpen);
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.NotSent:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeNotSent);
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Open:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeOpen);
                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Open_NoClick:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeOpenNoClick);
                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Sent:
                            sends = GetControlBlastCount(blastStatus, maParent, ControlTypeSend);
                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Suppressed:
                            if (blastStatus)
                                            {
                                foreach (var code in SuppressCodeList())
                                                {
                                    sends += GetControlBlastCount(blastStatus, maParent, ControlTypeSuppressed, code);
                                                }
                                            }
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Click:
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_Open:
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Subscribe:
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Unsubscribe:
                            sends = GetEmailDataSends<LayoutPlans>(childControl.ECNID);
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.Direct_NoOpen:
                            sends = GetEmailDataSends<TriggerPlans>(childControl.ECNID);
                                            break;
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.FormAbandon:
                        case FrameworkCommonObj.Enums.MarketingAutomationControlType.FormSubmit:
                            sends = GetBlastCountForFormControl(control, childControl, controls, connectors);
                                                    break;
                                            }
                                                }
                                           
                if (waitChildConn.Count == 1)
                                {
                                    wait.HeatMapStats = sends;
                                }
                                else
                                {
                                    connectors.First(x => x.id == waitconn.id).HeatMapStats = sends;
                                }
                            } //  for each wait child

                            retList.Add(wait);
                    }

        private List<ControlBase> GetHeatMapStats(List<ControlBase> controls, ref List<Connector> connectors)
        {
            var retList = new List<ControlBase>();

            foreach (var control in controls)
            {
                if (GetCampaignReportList().Contains(control.ControlType.ToString()) 
                    || GetLayoutList().Contains(control.ControlType.ToString())
                    || control.ControlType == FrameworkCommonObj.Enums.MarketingAutomationControlType.Form)
                {
                    if (control.ControlType == FrameworkCommonObj.Enums.MarketingAutomationControlType.Wait)
                    {
                        //unique sends for wait
                        AddControlForWaitType(control, controls, ref connectors, ref retList);
                }
                    else
                {
                        var bounces = 0;
                        var sends = 0;
                        
                        //only get dtReport if control is not a Group, Wait, Start, or End control
                        GetBlastReportData(
                            controls,
                            control,
                            connectors,
                            ref sends,
                            ref bounces);

                        AddControlForOtherType(control, sends, bounces, ref retList);
                }
                }
                else if (control.ControlType == FrameworkCommonObj.Enums.MarketingAutomationControlType.Group)
                {
                    AddControlForGroupType(control, ref retList);
                }
                else
                {
                    retList.Add(control);
                }
            }
            return retList;
        }

        public ActionResult Save(MarketingAutomationPostModel maPostModel)
        {
            var response = new List<string>();
            try
            {
                //Only update/Create ECN Objects if Automation is published
                if (maPostModel.State == Enums.MarketingAutomationStatus.Published ||
                    maPostModel.State == Enums.MarketingAutomationStatus.Paused)
                {
                    if (!string.IsNullOrWhiteSpace(maPostModel.JSONDiagram))
                    {
                        //It will always be an update, Probably want to do some initial validation here                
                        maPostModel.Controls = maPostModel.GetAllControls();
                        var marketingAutomation = BusinessCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                            maPostModel.MarketingAutomationID,
                            true,
                            CurrentUser);
                        var diagramObject = JsonConvert.DeserializeObject<DiagramJsonObject>(
                            maPostModel.JSONDiagram,
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

                        var diagramShapes = diagramObject.shapes;
                        AllControls = diagramShapes;
                        AllConnectors = diagramObject.connections;
                        marketingAutomation.JSONDiagram = maPostModel.JSONDiagram;

                        if (diagramShapes.Exists(shape =>
                                shape.ControlType == Enums.MarketingAutomationControlType.Start)
                            && diagramShapes.Exists(shape =>
                                shape.ControlType == Enums.MarketingAutomationControlType.End))
                        {
                            response = SaveAutomationForStartEndType(diagramObject,marketingAutomation,maPostModel);
                            return Json(response);
                        }
                        response.AddRange(AddStrToResponse(StatusCode500, AutomationMissingMessage));
                        return Json(response);
                    }
                    response.AddRange(AddStrToResponse(StatusCode500, AutomationNotSaveMessage));
                    return Json(response);
                }
                else
                {
                    //only need to commit Marketing Automation, MAControls, and MA Connectors to DB
                    try
                    {
                        var marketingAutomation = BusinessCommunicator.MarketingAutomation.GetByMarketingAutomationID(
                            maPostModel.MarketingAutomationID,
                            true,
                            CurrentUser);
                        marketingAutomation.JSONDiagram = maPostModel.JSONDiagram;
                        AutomationSaveAndAddHistory(marketingAutomation, CurrentUser, ActionSaveControls);

                        response.AddRange(AddStrToResponse(
                            StatusCode200, 
                            DiagramSaveMessage,
                            maPostModel.JSONDiagram));

                        return Json(response);
                    }
                    catch (SecurityException)
                    {
                        var homeUrl = new UrlHelper(Request.RequestContext).Action(
                            ActionNameIndex,
                            ControllerNameHome,
                            new { se = TrueStr });
                        SavePublishExceptionResponse<SecurityException>(null, ref response, null, homeUrl);
                        return Json(response);
                    }
                    catch (Exception)
                    {
                        SavePublishExceptionResponse<Exception>(null, ref response, null, null);
                        return Json(response);
                    }
                }
            }
            catch (SecurityException)
            {
                var homeUrl = new UrlHelper(Request.RequestContext).Action(
                    ActionNameIndex,
                    ControllerNameHome,
                    new { se = TrueStr });
                SavePublishExceptionResponse<SecurityException>(null, ref response, null, homeUrl);
                return Json(response);
            }
        }

        private List<string> SaveAutomationForStartEndType( 
            DiagramJsonObject diagramObject,
            EntityCommunicator.MarketingAutomation marketingAutomation,
            MarketingAutomationPostModel maPostModel)
        {
            var response = new List<string>();
            var diagramShapes = diagramObject.shapes;
            try
            {
                var ecnExForValidate = new ECNException(new List<ECNError>(), CommonEnum.ExceptionLayer.Business);
                var startObject = diagramShapes.OfType<Start>()
                    .First(shape => shape.ControlType == Enums.MarketingAutomationControlType.Start);
                //find connectors from start object
                var startConn = diagramObject.connections.Where(conn => conn.from.shapeId == startObject.ControlID)
                    .ToList();
                //start looping through it's children
                var children = diagramShapes.Where(shape => startConn.Any(conn => conn.to.shapeId == shape.ControlID))
                    .ToList();

                ValidatePublish(diagramShapes, marketingAutomation, ref ecnExForValidate);
                if (ecnExForValidate.ErrorList.Count > 0)
                {
                    throw ecnExForValidate;
                }

                //find start object
                using (var scopeMaster = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TimeSpan(0, 6, 0)))
                {
                    //save start object
                    SaveStartObject(marketingAutomation, maPostModel.MarketingAutomationID, startObject);

                    //Delete existing controls that don't exist in the models list of controls/connectors
                    var existingControls = marketingAutomation.Controls.Where(control => 
                        !maPostModel.Controls.Any(mControl => mControl.ControlID == control.ControlID)).ToList();

                    DeleteControl(existingControls, marketingAutomation, ValidateDelete, DeleteECNObject);

                    //Start the recursive save
                    var keepPaused = marketingAutomation.State == Enums.MarketingAutomationStatus.Paused;
                    SaveChildren(startObject, children, maPostModel.MarketingAutomationID, keepPaused);

                    AutomationSaveAndAddHistory(marketingAutomation, CurrentUser, ActionPublish);
                    scopeMaster.Complete();
                }
            }
            catch (SecurityException)
            {
                var homeUrl = new UrlHelper(Request.RequestContext).Action(
                    ActionNameIndex,
                    ControllerNameHome,
                    new { se = TrueStr });
                SavePublishExceptionResponse<SecurityException>(null, ref response, null, homeUrl);
                return response;
            }
            catch (ECNException ecnException)
            {
                SavePublishExceptionResponse(ecnException, ref response, maPostModel.JSONDiagram, null);
                return response;
            }
            catch (Exception ex)
            {
                SavePublishExceptionResponse(ex, ref response, maPostModel.JSONDiagram, null);
                return response;
            }

            var marketingAutomationId = marketingAutomation.MarketingAutomationID;
            var savedControls = BusinessCommunicator.MAControl.GetByMarketingAutomationID(marketingAutomationId);
            var savedConnectors = BusinessCommunicator.MAConnector.GetByMarketingAutomationID(marketingAutomationId);
            var connectors = Connector.GetPostModelFromObject(savedConnectors);
            var controls = ControlBase.GetModelsFromObject(savedControls, savedConnectors, diagramShapes, CurrentUser);
            var responseControls = ControlBase.Serialize(controls, connectors, marketingAutomationId);

            response.AddRange(AddStrToResponse(StatusCode200, DiagramSaveMessage, responseControls));
            return response;
        }
        
        private void DeleteECNObject(EntityCommunicator.MAControl maControl)
        {
            BaseDeleteEcnObject(maControl, false);
        }

        #region Publishing/creating ECN objects
        private List<Models.PostModels.ControlBase> GetChildren(Models.PostModels.Connector connector, List<Models.PostModels.ControlBase> children)
        {
            return children.Where(x => x.ControlID.ToString() == connector.to.shapeId).ToList();
        }

        private List<Models.PostModels.Connector> GetWaitChildren(Models.PostModels.ControlBase wait, List<Models.PostModels.ControlBase> controls, List<Models.PostModels.Connector> connectors)
        {
            List<Models.PostModels.Connector> retList = new List<Models.PostModels.Connector>();
            foreach (Models.PostModels.Connector conn in connectors)
            {
                if (conn.from.shapeId == wait.ControlID)
                {
                    foreach (Models.PostModels.ControlBase con in controls)
                    {
                        if (con.ControlID == conn.to.shapeId)
                        {
                            retList.Add(conn);
                        }
                    }
                }
            }
            return retList;
        }

        private bool IsParentDirectOpen(Models.PostModels.Controls.Wait wait)
        {
            Models.PostModels.Connector startConn = AllConnectors.First(x => x.to.shapeId == wait.ControlID);
            if (startConn != null)
            {
                //start looping through it's children
                return AllControls.Exists(x => startConn.from.shapeId == x.ControlID && x.ControlType == ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open);

            }
            else
            {
                return false;
            }
        }

        private void SaveChildren(ControlBase parent, List<ControlBase> children, int automationId, bool keepPaused)
        {
            BaseSaveChildren(parent, children, automationId, keepPaused, false);
        }

        #endregion
    }
}