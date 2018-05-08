using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;
using KM.Common.Entity;
using KMSite;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessFormDesigner = ECN_Framework_BusinessLayer.FormDesigner;
using CampaignItem = ecn.MarketingAutomation.Models.PostModels.Controls.CampaignItem;
using CommonEnums = ECN_Framework_Common.Objects.Enums;
using EntityCommunicator = ECN_Framework_Entities.Communicator;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using KmEntity = KMPlatform.Entity;

namespace ecn.MarketingAutomation.Controllers
{
    public class AutomationBaseController : BaseController
    {
        private const string SubCategoryGroup = "Group";
        private const string RegularCampaignItemType = "Regular";
        private const string HtmlCampaignItemFormatType = "HTML";
        private const int CompletedItem5 = 5;
        private const string StartText = "Start";
        private const string HtmlBreak = "<br />";
        private const string StatusCode500 = "500";
        private const string StatusCode302 = "302";
        private const string AppSettingKmApplication = "KMCommon_Application";
        private const string ValidationErrorStr = "Validation Errors: ";
        private const string CommonExceptionMessage = "An error occurred";
        private const string MethodNameSave = "MarketingAutomation.Save";
        private const string AutomationNotSaveMessage = "Could not save automation";
        private const string PermissionMessage = "You do not have sufficient permissions to publish an automation.";
        private const string AllowCustomerOverrideNodeName = "AllowCustomerOverride";
        private const string OverrideNodeName = "Override";
        private const string XPathSetting = "/Settings";
        private const int LinkTrackingId3 = 3;
        private const string NameOmniture1 = "Omniture1";
        private const string NameOmniture2 = "Omniture2";
        private const string NameOmniture3 = "Omniture3";
        private const string NameOmniture4 = "Omniture4";
        private const string NameOmniture5 = "Omniture5";
        private const string NameOmniture6 = "Omniture6";
        private const string NameOmniture7 = "Omniture7";
        private const string NameOmniture8 = "Omniture8";
        private const string NameOmniture9 = "Omniture9";
        private const string NameOmniture10 = "Omniture10";
        private const string EventTypeSubmit = "submit";
        private const string EventTypeAbandon = "abandon";
        private const string EventTypeClick = "click";
        private const string EventTypeOpen = "open";
        private const string EventTypeSubscribe = "Subscribe";
        private const string EventTypeNoOpen = "NoOpen";
        private const string StatusN = "N";
        private const string StatusY = "Y";
        private const int CompletedStep5 = 5;
        private const string SegmentNameClick = "click";
        private const string SegmentNameUnClick = "unclick";
        private const string SegmentNameUnOpen = "unopen";
        private const string SegmentNameNotSent = "not sent";
        private const string SegmentNameOpen = "open";
        private const string SegmentNameSent = "sent";
        private const string SegmentNameSuppressed = "suppressed";
        private const decimal DefaultReturnTime = 0.0M;
        private const string ErrorMsgCampaignItem = "Cannot delete Campaign Item because it has already gone out";
        private const string ErrorMsgClick = "Cannot delete Click email because it has already gone out";
        private const string ErrorMsgDirectClick = "Cannot delete Click trigger because it has already gone out";
        private const string ErrorMsgDirectOpen = "Cannot delete Open trigger because it has already gone out";
        private const string ErrorMsgNoClick = "Cannot delete No Click email because it has already gone out";
        private const string ErrorMsgNoOpen = "Cannot delete No Open email because it has already gone out";
        private const string ErrorMsgNotSent = "Cannot delete Not Sent email because it has already gone out";
        private const string ErrorMsgOpen = "Cannot delete Open email because it has already gone out";
        private const string ErrorMsgOpenNoClick = "Cannot delete Open/NoClick email because it has already gone out";
        private const string ErrorMsgSent = "Cannot delete Sent email because it has already gone out";
        private const string ErrorMsgSubscribe = "Cannot delete Subscribe trigger because it has already gone out";
        private const string ErrorMsgSuppressed = "Cannot delete Suppressed email because it has already gone out";
        private const string ErrorMsgUnSubscribe = "Cannot delete Unsubscribe trigger because it has already gone out";
        private const string BlastStatusCodeSent = "sent";
        private const string ActionNameSubscribe = "Subscribe";
        private const string ActionNameUnsubscribe = "Unsubscribe";
        private const string ActionNameTrigger = "Trigger";
        private const string ActionNamePrefix = "NO OPEN on";
        private const string CriteriaS = "S";
        private const string CriteriaU = "U";
        private const string MethodNameGetGroupAndCampaignItem = "GetGroupAndCampaignItem";
        private const string UnableToValidateMessage = "Unable to validate";
        private const string MissingEndControlMessage =
            "Missing End Control. Every automation branch requires an End Control.";
        private const int ParentDirectOrForm0 = 0;
        private const int ParentDirectOrForm1 = 1;
        private const int ParentDirectOrForm2 = 2;
        private const int ParentDirectOrForm3 = 3;
        private const string ErrorMsgClickEmail = "Click Email must have a Wait control as parent";
        private const string ErrorMsgNoClickEmail = "No Click Email must have a Wait control as parent";
        private const string ErrorMsgNoOpenEmail = "No Open Email must have a Wait control as parent";
        private const string ErrorMsgNotSentEmail = "Not Sent Email must have a Wait control as parent";
        private const string ErrorMsgOpenEmail = "Open Email must have a Wait control as parent";
        private const string ErrorMsgOpenNoClickEmail = "Open-No Click Email must have a Wait control as parent";
        private const string ErrorMsgSentEmail = "Sent Email must have a Wait control as parent";
        private const string ErrorMsgSuppressedEmail = "Suppressed Email must have a Wait control as parent";

        protected KmEntity.User CurrentUser
        {
            get
            {
                return ECNSession.CurrentSession().CurrentUser;
            }
        }

        public List<ControlBase> AllControls { get; set; } = new List<ControlBase>();

        protected List<Connector> AllConnectors { get; set; } = new List<Connector>();

        protected int SaveCampaignItem(
            CampaignItem ciObject, 
            bool keepPaused = false)
        {
            if (ciObject.CreateCampaignItem)
            {
                var campaignItem = new EntityCommunicator.CampaignItem();
                if (ciObject.CampaignItemID > 0)
                {
                    campaignItem = BusinessCommunicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(
                        ciObject.CampaignItemID, true);
                }

                SetCampaignItemValues(ciObject, ref campaignItem);

                AttachGroups(ciObject, ref campaignItem);

                foreach (var ciBlast in campaignItem.BlastList)
                {
                    BusinessCommunicator.CampaignItemBlastRefBlast.Delete(ciBlast.CampaignItemBlastID, CurrentUser);
                }

                campaignItem.CampaignItemID = BusinessCommunicator.CampaignItem.Save_UseAmbientTransaction(
                    campaignItem, 
                    CurrentUser);

                var bOptOutMasterSuppression = false;
                if (campaignItem.CampaignItemID > 0 && campaignItem.CampaignItemTemplateID > 0)
                {
                    SaveLinkTrackingParamOptions(campaignItem);
                    var itemTemplate = BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                        campaignItem.CampaignItemTemplateID.Value,
                        ECNSession.CurrentSession().CurrentUser,
                        true);

                    if (itemTemplate.OptOutMasterSuppression != null)
                    {
                        bOptOutMasterSuppression = itemTemplate.OptOutMasterSuppression.Value;
                    }

                    if (itemTemplate.OptOutSpecificGroup != null &&
                        itemTemplate.OptOutSpecificGroup.Value &&
                        itemTemplate.OptoutGroupList.Count > 0)
                    {
                        foreach (var optOutGroup in itemTemplate.OptoutGroupList)
                        {
                            var itemOptOutGroup = new EntityCommunicator.CampaignItemOptOutGroup
                            {
                                CampaignItemID = campaignItem.CampaignItemID,
                                CustomerID = campaignItem.CustomerID,
                                CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                                GroupID = optOutGroup.GroupID
                            };
                            BusinessCommunicator.CampaignItemOptOutGroup.Save(
                                itemOptOutGroup,
                                ECNSession.CurrentSession().CurrentUser);
                        }
                    }
                }

                SaveBlastAndSuppression(campaignItem, bOptOutMasterSuppression, keepPaused);

                return campaignItem.CampaignItemID;
            }
            return ciObject.CampaignItemID;
        }

        private void SaveBlastAndSuppression(
            EntityCommunicator.CampaignItem campaignItem, 
            bool bOptOutMasterSuppression,
            bool keepPaused)
        {
            foreach (var itemBlast in campaignItem.BlastList)
            {
                itemBlast.CampaignItemID = campaignItem.CampaignItemID;
                if (bOptOutMasterSuppression)
                {
                    itemBlast.AddOptOuts_to_MS = true;
                }
            }

            foreach (var itemSuppression in campaignItem.SuppressionList)
            {
                itemSuppression.CampaignItemID = campaignItem.CampaignItemID;
            }

            BusinessCommunicator.CampaignItemBlast.Save_UseAmbientTransaction(
                campaignItem.CampaignItemID,
                campaignItem.BlastList,
                CurrentUser);

            BusinessCommunicator.CampaignItemSuppression.Delete_NoAccessCheck(
                campaignItem.CampaignItemID,
                CurrentUser);

            foreach (var itemSuppression in campaignItem.SuppressionList)
            {
                BusinessCommunicator.CampaignItemSuppression.Save(itemSuppression, CurrentUser);
            }

            //adding this check to get the initial save done.
            BusinessCommunicator.Blast.CreateBlastsFromCampaignItem_UseAmbientTransaction(
                campaignItem.CampaignItemID,
                CurrentUser,
                campaignItem.BlastList.Any(blast => blast.BlastID != null),
                keepPaused);
        }

        private void SetCampaignItemValues(CampaignItem ciObject, ref EntityCommunicator.CampaignItem campaignItem)
        {
            campaignItem.CampaignID = ciObject.CampaignID > 0
                ? ciObject.CampaignID //using existing campaign
                : SaveCampaign(ciObject.CampaignName, ciObject.CustomerID); //create new campaign

            campaignItem.CampaignItemFormatType = HtmlCampaignItemFormatType;
            campaignItem.CampaignItemName = ciObject.CampaignItemName;
            campaignItem.CampaignItemNameOriginal = ciObject.CampaignItemName;
            campaignItem.CampaignItemType = RegularCampaignItemType;
            campaignItem.CompletedStep = CompletedItem5;

            campaignItem.CreatedUserID = CurrentUser.UserID;
            campaignItem.CustomerID = ciObject.CustomerID;
            campaignItem.EnableCacheBuster = true;
            campaignItem.FromEmail = ciObject.FromEmail;
            campaignItem.FromName = ciObject.FromName;

            campaignItem.CampaignItemTemplateID = ciObject.CampaignItemTemplateID > 0
                ? ciObject.CampaignItemTemplateID
                : -1;

            campaignItem.BlastField1 = ciObject.BlastField1;
            campaignItem.BlastField2 = ciObject.BlastField2;
            campaignItem.BlastField3 = ciObject.BlastField3;
            campaignItem.BlastField4 = ciObject.BlastField4;
            campaignItem.BlastField5 = ciObject.BlastField5;
            campaignItem.IgnoreSuppression = false;
            campaignItem.IsDeleted = false;
            campaignItem.IsHidden = false;
            campaignItem.ReplyTo = ciObject.ReplyTo;

            if (ciObject.SubCategory == SubCategoryGroup)
            {
                //Have to update existing campaign items send time
                var parentWait = FindParentWait(ciObject);
                var parentCampaignItem = FindParentCampaignItem(parentWait);
                var tsWait = new TimeSpan(
                    Convert.ToInt32(parentWait.Days),
                    Convert.ToInt32(parentWait.Hours),
                    Convert.ToInt32(parentWait.Minutes),
                    0);
                if (parentCampaignItem.SendTime != null)
                {
                    campaignItem.SendTime = parentCampaignItem.SendTime.Value.AddDays(
                        Convert.ToDouble(tsWait.TotalDays));
                }
            }
            else
            {
                campaignItem.SendTime = ciObject.SendTime;
            }
        }

        private int SaveCampaign(string campaignName, int customerId)
        {
            var campaign = new EntityCommunicator.Campaign
            {
                CampaignName = campaignName,
                CreatedDate = DateTime.Now,
                CreatedUserID = CurrentUser.UserID,
                CustomerID = customerId,
                IsArchived = false,
                IsDeleted = false
            };
            return BusinessCommunicator.Campaign.Save_UseAmbientTransaction(campaign, CurrentUser);
        }

        protected Wait FindParentWait(ControlBase child)
        {
            var startConn = AllConnectors.First(connectors => connectors.to.shapeId == child.ControlID);
            if (startConn != null)
            {
                //start looping through it's children
                return AllControls.OfType<Wait>().First(control => startConn.from.shapeId == control.ControlID);
            }
            return null;
        }

        protected CampaignItem FindParentCampaignItem(ControlBase childWait)
        {
            var parentCampaignItem = new CampaignItem();
            var connector = AllConnectors.First(connectors => connectors.to.shapeId == childWait.ControlID);
            var controlCounter = 0;
            var isToContinue = true;
            if (connector != null)
            {
                while (isToContinue)
                {
                    if (AllControls.First(control => connector.from.shapeId == control.ControlID).
                            ControlType == Enums.MarketingAutomationControlType.CampaignItem)
                    {
                        parentCampaignItem = AllControls.OfType<CampaignItem>().First(control => 
                            connector.from.shapeId == control.ControlID);
                        isToContinue = false;
                    }
                    else
                    {
                        connector = AllConnectors.First(connectors => connectors.to.shapeId == connector.from.shapeId);
                    }

                    controlCounter++;
                    if (controlCounter > AllControls.Count)
                    {
                        isToContinue = false;
                    }
                }
                return parentCampaignItem;
            }
            return null;
        }

        private void AttachGroups(CampaignItem ciObject, ref EntityCommunicator.CampaignItem campaignItem)
        {
            campaignItem.BlastList = new List<EntityCommunicator.CampaignItemBlast>();
            campaignItem.SuppressionList = new List<EntityCommunicator.CampaignItemSuppression>();
            foreach (var groupSelect in ciObject.SelectedGroups)
            {
                var campaignItemBlast = new EntityCommunicator.CampaignItemBlast
                {
                    CampaignItemID = campaignItem.CampaignItemID,
                    CreatedUserID = CurrentUser.UserID,
                    CustomerID = ciObject.CustomerID,
                    EmailFrom = campaignItem.FromEmail,
                    EmailSubject = ciObject.EmailSubject,
                    FromName = campaignItem.FromName,
                    GroupID = groupSelect.GroupID,
                    IsDeleted = false,
                    LayoutID = ciObject.MessageID,
                    ReplyTo = campaignItem.ReplyTo
                };

                if (ciObject.SelectedGroupFilters != null &&
                    ciObject.SelectedGroupFilters.Any(filter => filter.GroupID == campaignItemBlast.GroupID))
                {
                    var selectedGroupFilter = ciObject.SelectedGroupFilters.Where(filter => 
                        filter.GroupID == groupSelect.GroupID);
                    foreach (var filterSelect in selectedGroupFilter)
                    {
                        var blastFilter = new EntityCommunicator.CampaignItemBlastFilter
                        {
                            FilterID = filterSelect.FilterID,
                            IsDeleted = false
                        };
                        campaignItemBlast.Filters.Add(blastFilter);
                    }
                }
                campaignItem.BlastList.Add(campaignItemBlast);
            }

            if (ciObject.SuppressedGroups != null)
            {
                foreach (var suppressedGroup in ciObject.SuppressedGroups)
                {
                    var itemSuppression = new EntityCommunicator.CampaignItemSuppression
                    {
                        CreatedUserID = CurrentUser.UserID,
                        CustomerID = ciObject.CustomerID,
                        GroupID = suppressedGroup .GroupID,
                        IsDeleted = false
                    };
                    if (ciObject.SelectedGroupFilters != null && 
                        ciObject.SuppressedGroupFilters != null &&
                        ciObject.SuppressedGroupFilters.Any(filter => filter.GroupID == suppressedGroup .GroupID))
                    {
                        var selectedGroupFilter = ciObject.SelectedGroupFilters.Where(filter =>
                            filter.GroupID == suppressedGroup .GroupID);
                        foreach (var filterSelect in selectedGroupFilter)
                        {
                            var itemBlastFilter = new EntityCommunicator.CampaignItemBlastFilter
                            {
                                FilterID = filterSelect.FilterID,
                                IsDeleted = false
                            };
                            itemSuppression.Filters.Add(itemBlastFilter);
                        }
                    }
                    campaignItem.SuppressionList.Add(itemSuppression);
                }
            }
        }

        protected void SaveStartObject(
            EntityCommunicator.MarketingAutomation marketingAutomation,
            int marketingAutomationId,
            Start startObject)
        {
            //save start object
            if (!marketingAutomation.Controls.Exists(x =>
                x.ControlType == Enums.MarketingAutomationControlType.Start &&
                x.ControlID == startObject.ControlID))
            {
                var startMac = new EntityCommunicator.MAControl
                {
                    ControlType = Enums.MarketingAutomationControlType.Start,
                    ControlID = startObject.ControlID,
                    ExtraText = string.Empty,
                    MarketingAutomationID = marketingAutomationId,
                    Text = StartText,
                    xPosition = startObject.xPosition,
                    yPosition = startObject.yPosition
                };
                BusinessCommunicator.MAControl.Save(startMac);
            }
        }

        protected IList<string> AddStrToResponse(params string[] list)
        {
            var response = new List<string>();
            response.AddRange(list);
            return response;
        }

        protected void DeleteControl(List<EntityCommunicator.MAControl> existingControls,
            EntityCommunicator.MarketingAutomation marketingAutomation,
            Action<EntityCommunicator.MAControl> validateDelete,
            Action<EntityCommunicator.MAControl> deleteEcnObject)
        {
            var existingConnectors = marketingAutomation.Connectors;

            //validate delete on controls before we actually delete
            existingControls.ForEach(validateDelete);

            foreach (var mac in existingControls)
            {
                var connToDelete = existingConnectors
                    .Where(conn => conn.From == mac.ControlID || conn.To == mac.ControlID).ToList();
                foreach (var maconn in connToDelete)
                {
                    BusinessCommunicator.MAConnector.Delete(maconn.ConnectorID);
                }
                deleteEcnObject(mac);
                BusinessCommunicator.MAControl.Delete(mac.MAControlID);
            }
        }

        protected void AutomationSaveAndAddHistory(
            EntityCommunicator.MarketingAutomation marketingAutomation,
            KmEntity.User currentUser,
            string action)
        {
            marketingAutomation.UpdatedUserID = currentUser.UserID;
            BusinessCommunicator.MarketingAutomation.Save(marketingAutomation, currentUser);
            BusinessCommunicator.MarketingAutomationHistory.Insert(
                marketingAutomation.MarketingAutomationID,
                currentUser.UserID,
                action);
        }

        protected void SavePublishExceptionResponse<T>(
            T exception,
            ref List<string> response,
            string jsonDiagram,
            string redirectUrl)
        {
            if (typeof(SecurityException).IsAssignableFrom(typeof(T)))
            {
                if (string.IsNullOrWhiteSpace(redirectUrl))
                {
                    response.AddRange(AddStrToResponse(StatusCode500, PermissionMessage, jsonDiagram));
                }
                else
                {
                    response = new List<string>();
                    response.AddRange(AddStrToResponse(StatusCode302, redirectUrl));
                }
            }
            else if (typeof(ECNException).IsAssignableFrom(typeof(T)))
            {
                var ecnException = exception as ECNException;
                if (ecnException != null)
                {
                    //throw popup error saying we can't save certain objects
                    var stringBuilder = new StringBuilder();
                    foreach (var error in ecnException.ErrorList)
                    {
                        stringBuilder.AppendLine($"{error.ErrorMessage}{HtmlBreak}");
                    }

                    response.AddRange(AddStrToResponse(
                        StatusCode500,
                        $"{ValidationErrorStr}{stringBuilder}",
                        jsonDiagram));
                }
            }
            else
            {
                if (exception != null)
                {
                    var exceptionObj = exception as Exception;
                    response.AddRange(AddStrToResponse(
                        StatusCode500,
                        CommonExceptionMessage,
                        jsonDiagram));

                    ApplicationLog.LogCriticalError(
                        exceptionObj,
                        MethodNameSave,
                        Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]));
                }
                else
                {
                    response.AddRange(string.IsNullOrWhiteSpace(jsonDiagram)
                        ? AddStrToResponse(StatusCode500, AutomationNotSaveMessage)
                        : AddStrToResponse(StatusCode500, CommonExceptionMessage, jsonDiagram));
                }
            }
        }

        protected void SaveLinkTrackingParamOptions(EntityCommunicator.CampaignItem campaignItem)
        {
            var trackingParams = BusinessCommunicator.LinkTrackingParam.GetByLinkTrackingID(LinkTrackingId3);
            if (campaignItem.CampaignItemTemplateID != null && campaignItem.CustomerID != null)
            {
                var itemTemplate = BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                    campaignItem.CampaignItemTemplateID.Value,
                    ECNSession.CurrentSession().CurrentUser);

                var allowCustOverride = false;
                var overrideBaseChannel = false;
                var hasBaseSetup = false;
                var hasCustSetup = false;

                var baseTrackingSetting = BusinessCommunicator.LinkTrackingSettings.GetByBaseChannelID_LTID(
                    ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID,
                    LinkTrackingId3);
                if (baseTrackingSetting != null && baseTrackingSetting.LTSID > 0)
                {
                    hasBaseSetup = GetOverrideValueForChannelOrCustomer(
                        baseTrackingSetting.XMLConfig,
                        AllowCustomerOverrideNodeName,
                        out allowCustOverride);
                }

                var custTrackingSetting = BusinessCommunicator.LinkTrackingSettings.GetByCustomerID_LTID(
                    campaignItem.CustomerID.Value,
                    LinkTrackingId3);
                if (custTrackingSetting != null && custTrackingSetting.LTSID > 0)
                {
                    hasCustSetup = GetOverrideValueForChannelOrCustomer(
                        custTrackingSetting.XMLConfig,
                        OverrideNodeName,
                        out overrideBaseChannel);
                }

                // To Get LinkTrackingParamOption by Customer ID
                if (allowCustOverride && overrideBaseChannel && hasCustSetup)
                {
                    SaveTrackingForOmniture(
                        campaignItem.CampaignItemID,
                        itemTemplate,
                        trackingParams,
                        campaignItem.CustomerID.Value,
                        true);
                }
                // To Get LinkTrackingParamOption by BaseChannel ID
                else if (((allowCustOverride && !overrideBaseChannel) || !allowCustOverride) && hasBaseSetup)
                {
                    SaveTrackingForOmniture(
                        campaignItem.CampaignItemID,
                        itemTemplate,
                        trackingParams,
                        ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID,
                        false);
                }
            }
        }

        private void SaveTrackingForOmniture(
            int campaignItemId,
            EntityCommunicator.CampaignItemTemplate itemTemplate,
            IList<EntityCommunicator.LinkTrackingParam> trackingParams,
            int channelOrCustId,
            bool isCustSetup)
        {
            var omniture1Value = itemTemplate.Omniture1;
            if (!string.IsNullOrWhiteSpace(omniture1Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture1).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture1Value, channelOrCustId, isCustSetup);
            }

            var omniture2Value = itemTemplate.Omniture2;
            if (!string.IsNullOrWhiteSpace(omniture2Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture2).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture2Value, channelOrCustId, isCustSetup);
            }

            var omniture3Value = itemTemplate.Omniture3;
            if (!string.IsNullOrWhiteSpace(omniture3Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture3).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture3Value, channelOrCustId, isCustSetup);
            }

            var omniture4Value = itemTemplate.Omniture4;
            if (!string.IsNullOrWhiteSpace(omniture4Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture4).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture4Value, channelOrCustId, isCustSetup);
            }

            var omniture5Value = itemTemplate.Omniture5;
            if (!string.IsNullOrWhiteSpace(omniture5Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture5).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture5Value, channelOrCustId, isCustSetup);
            }

            var omniture6Value = itemTemplate.Omniture6;
            if (!string.IsNullOrWhiteSpace(omniture6Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture6).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture6Value, channelOrCustId, isCustSetup);
            }

            var omniture7Value = itemTemplate.Omniture7;
            if (!string.IsNullOrWhiteSpace(omniture7Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture7).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture7Value, channelOrCustId, isCustSetup);
            }

            var omniture8Value = itemTemplate.Omniture8;
            if (!string.IsNullOrWhiteSpace(omniture8Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture8).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture8Value, channelOrCustId, isCustSetup);
            }

            var omniture9Value = itemTemplate.Omniture9;
            if (!string.IsNullOrWhiteSpace(omniture9Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture9).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture9Value, channelOrCustId, isCustSetup);
            }

            var omniture10Value = itemTemplate.Omniture10;
            if (!string.IsNullOrWhiteSpace(omniture10Value))
            {
                var optionId = trackingParams.First(trackParam => trackParam.DisplayName == NameOmniture10).LTPID;
                SaveCampaignLinkTracking(campaignItemId, optionId, omniture10Value, channelOrCustId, isCustSetup);
            }
        }

        private bool GetOverrideValueForChannelOrCustomer(string xml, string overrideNodeName, out bool isOverridable)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            var rootNode = xmlDocument.SelectSingleNode(XPathSetting);
            if (rootNode != null && rootNode.HasChildNodes)
            {
                bool.TryParse(rootNode[overrideNodeName]?.InnerText, out isOverridable);
                return true;
            }
            else
            {
                isOverridable = false;
                return false;
            }
        }

        private void SaveCampaignLinkTracking(
            int campaignItemId,
            int linkTrackingParamOptionId,
            string omnitureValue,
            int baseChannelOrCustId,
            bool isCustSetup)
        {
            var campaignItemLinkTracking = new EntityCommunicator.CampaignItemLinkTracking
            {
                LTPID = linkTrackingParamOptionId,
                CustomValue = string.Empty,
                CampaignItemID = campaignItemId
            };

            var linkTrackingParamOption = isCustSetup
                ? BusinessCommunicator.LinkTrackingParamOption.GetLTPOIDByCustomerID(
                    linkTrackingParamOptionId,
                    omnitureValue,
                    baseChannelOrCustId)
                : BusinessCommunicator.LinkTrackingParamOption.GetLTPOIDByBaseChannelID(
                    linkTrackingParamOptionId,
                    omnitureValue,
                    baseChannelOrCustId);

            if (linkTrackingParamOption != null && linkTrackingParamOption.LTPOID > 0)
            {
                campaignItemLinkTracking.LTPOID = linkTrackingParamOption.LTPOID;
            }
            else
            {
                campaignItemLinkTracking.LTPOID = -1;
            }

            BusinessCommunicator.CampaignItemLinkTracking.Save(
                campaignItemLinkTracking,
                ECNSession.CurrentSession().CurrentUser);
        }

        protected int SaveFormControlTrigger(
            ControlBase trigger,
            ControlBase parentWait,
            ControlBase parentCi,
            ControlBase parentForm)
        {
            return SaveMessageOrFormTrigger(trigger, parentWait, parentCi, parentForm, false);
        }

        private int GetParentCustomerId(ControlBase parentWait)
        {
            var parentCampaignItem = default(CampaignItem);
            var parentGroup = default(Group);
            try
            {
                parentCampaignItem = FindParentCampaignItem(parentWait);
                parentGroup = FindParentGroup(parentWait);
            }
            catch
            {
                // ignored
            }
            return parentCampaignItem?.CustomerID ?? (parentGroup?.CustomerID ?? -1);
        }

        private void SetLayoutAndCampaignItemId(
            ControlBase control,
            bool isMessageTrigger,
            ref int layoutId,
            ref int campaignItemId)
        {
            switch (control.ControlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                    SetLayoutAndCampaignItemFromControl(control as CampaignItem, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Click:
                    SetLayoutAndCampaignItemFromControl(control as Click, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.NoClick:
                    SetLayoutAndCampaignItemFromControl(control as NoClick, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.NoOpen:
                    SetLayoutAndCampaignItemFromControl(control as NoOpen, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.NotSent:
                    SetLayoutAndCampaignItemFromControl(control as NotSent, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Open:
                    SetLayoutAndCampaignItemFromControl(control as Open, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Open_NoClick:
                    SetLayoutAndCampaignItemFromControl(control as Open_NoClick, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Sent:
                    SetLayoutAndCampaignItemFromControl(control as Sent, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Subscribe:
                    var subscribe = control as Subscribe;
                    SetLayoutAndCampaignItem(subscribe, true, isMessageTrigger, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Suppressed:
                    SetLayoutAndCampaignItemFromControl(control as Suppressed, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Unsubscribe:
                    var unsubscribe = control as Unsubscribe;
                    SetLayoutAndCampaignItem(unsubscribe, true, isMessageTrigger, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Direct_Click:
                    var directClick = control as Direct_Click;
                    SetLayoutAndCampaignItem(directClick, true, isMessageTrigger, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Form:
                    SetLayoutAndCampaignItemFromControl(control as CampaignItem, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Direct_Open:
                    var directOpen = control as Direct_Open;
                    SetLayoutAndCampaignItem(directOpen, true, isMessageTrigger, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.Direct_NoOpen:
                    var directNoOpen = control as Direct_NoOpen;
                    SetLayoutAndCampaignItem(directNoOpen, true, isMessageTrigger, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.FormAbandon:
                    SetLayoutAndCampaignItemIdFromDb(control as FormAbandon, false, out layoutId, out campaignItemId);
                    break;
                case Enums.MarketingAutomationControlType.FormSubmit:
                    SetLayoutAndCampaignItemIdFromDb(control as FormSubmit, false, out layoutId, out campaignItemId);
                    break;
            }
        }

        private void SetLayoutAndCampaignItem(
            CampaignControlBase control,
            bool getChildren,
            bool isMessageTrigger,
            out int layoutId,
            out int campaignItemId)
        {
            if (isMessageTrigger)
            {
                SetLayoutAndCampaignItemFromControl(control, out layoutId, out campaignItemId);
            }
            else
            {
                SetLayoutAndCampaignItemIdFromDb(control, getChildren, out layoutId, out campaignItemId);
            }
        }

        private void SetLayoutAndCampaignItemIdFromDb(
            CampaignControlBase control,
            bool getChildren,
            out int layoutId,
            out int campaignItemId)
        {
            campaignItemId = -1;
            layoutId = -1;
            var blastid = 0;
            if (control.GetType() == typeof(Direct_NoOpen))
            {
                var triggerPlan = BusinessCommunicator.TriggerPlans.GetByTriggerPlanID(control.ECNID, CurrentUser);
                if (triggerPlan.BlastID != null)
                {
                    blastid = triggerPlan.BlastID.Value;
                }
            }
            else
            {
                var layoutPlans = BusinessCommunicator.LayoutPlans.GetByLayoutPlanID_UseAmbientTransaction(
                    control.ECNID,
                    CurrentUser);
                if (layoutPlans.BlastID != null)
                {
                    blastid = layoutPlans.BlastID.Value;
                }
            }

            var campaignItem = BusinessCommunicator.CampaignItem.GetByBlastID_NoAccessCheck_UseAmbientTransaction(
                blastid,
                getChildren);
            if (control.GetType() == typeof(FormSubmit) || control.GetType() == typeof(FormAbandon))
            {
                layoutId = control.MessageID;
            }
            else
            {
                if (campaignItem.BlastList.Any())
                {
                    var blastLayoutId = campaignItem.BlastList[0].LayoutID;
                    if (blastLayoutId != null)
                    {
                        layoutId = blastLayoutId.Value;
                    }
                }
            }
            campaignItemId = campaignItem.CampaignItemID;
        }

        private void SetLayoutAndCampaignItemFromControl(
            CampaignControlBase control,
            out int layoutId,
            out int campaignItemId)
        {
            layoutId = control.MessageID;
            campaignItemId = control.ECNID;
        }

        private void SetCampaignItemBlast<T>(
            T plans,
            out EntityCommunicator.CampaignItem campaignItem,
            out EntityCommunicator.CampaignItemBlast campaignItemBlast,
            int parentCustomerId,
            CampaignControlBase control,
            bool isGroupTrigger = false)
        {
            EntityCommunicator.Campaign campaign;
            var blastId = GetBlastIdForCampaign(plans);
            if (blastId > 0)
            {
                campaign = BusinessCommunicator.Campaign.GetByBlastID_UseAmbientTransaction(
                    blastId,
                    CurrentUser,
                    true);
                campaign.UpdatedUserID = CurrentUser.UserID;
                campaignItem = campaign.ItemList[0];
                campaignItem.UpdatedUserID = CurrentUser.UserID;
                campaignItemBlast = campaignItem.BlastList[0];
                campaignItemBlast.UpdatedUserID = CurrentUser.UserID;
                if (isGroupTrigger)
                {
                    campaign.CampaignName = control.CampaignItemName;
                }
            }
            else
            {
                campaign = new EntityCommunicator.Campaign
                {
                    CampaignName = control.CampaignItemName,
                    CreatedUserID = CurrentUser.UserID
                };
                campaignItem = new EntityCommunicator.CampaignItem
                {
                    CreatedUserID = CurrentUser.UserID
                };
                campaignItemBlast = new EntityCommunicator.CampaignItemBlast
                {
                    CreatedUserID = CurrentUser.UserID
                };
            }

            campaign.CustomerID = parentCustomerId;
            campaign.CampaignID = BusinessCommunicator.Campaign.Save_UseAmbientTransaction(campaign, CurrentUser);

            campaignItem.CustomerID = campaign.CustomerID.Value;
            campaignItem.CampaignID = campaign.CampaignID;
            campaignItem.CampaignItemType = control.GetType() == typeof(Direct_NoOpen)
                ? Enums.CampaignItemType.NoOpen.ToString()
                : Enums.CampaignItemType.Layout.ToString();
            campaignItem.FromEmail = control.FromEmail;
            campaignItem.FromName = control.FromName;
            campaignItem.ReplyTo = control.ReplyTo;
            campaignItem.SendTime = DateTime.Now.AddSeconds(15);
            campaignItem.IsHidden = false;
            campaignItem.CampaignItemName = control.CampaignItemName;
            campaignItem.CampaignItemNameOriginal = control.CampaignItemName;
            campaignItem.CampaignItemTemplateID = control.CampaignItemTemplateID;
            campaignItem.BlastField1 = control.BlastField1;
            campaignItem.BlastField2 = control.BlastField2;
            campaignItem.BlastField3 = control.BlastField3;
            campaignItem.BlastField4 = control.BlastField4;
            campaignItem.BlastField5 = control.BlastField5;
            campaignItem.CampaignItemFormatType = Enums.CampaignItemFormatType.HTML.ToString();
            campaignItem.CampaignItemID = BusinessCommunicator.CampaignItem.Save_UseAmbientTransaction(
                campaignItem,
                CurrentUser);
        }

        private int GetBlastIdForCampaign<T>(T plans)
        {
            var blastId = 0;
            if (typeof(EntityCommunicator.LayoutPlans).IsAssignableFrom(typeof(T)))
            {
                var layoutPlans = plans as EntityCommunicator.LayoutPlans;
                if (layoutPlans?.LayoutPlanID > 0 && 
                    layoutPlans.BlastID.HasValue && 
                    layoutPlans.BlastID.Value > 0)
                {
                    blastId = layoutPlans.BlastID.Value;
                }
            }
            else
            {
                var triggerPlans = plans as EntityCommunicator.TriggerPlans;
                if (triggerPlans?.TriggerPlanID > 0 && 
                    triggerPlans.BlastID.HasValue && 
                    triggerPlans.BlastID.Value > 0)
                {
                    blastId = triggerPlans.BlastID.Value;
                }
            }

            return blastId;
        }

        private bool SetLayoutPlanEventType(
            ref CampaignControlBase control,
            ControlBase trigger,
            EntityCommunicator.LayoutPlans layoutPlans)
        {
            var isCancelled = false;
            if (trigger.ControlType == Enums.MarketingAutomationControlType.FormSubmit)
            {
                var formSubmit = trigger as FormSubmit;
                if (formSubmit != null)
                {
                    isCancelled = formSubmit.IsCancelled;
                    control = formSubmit;
                }
                layoutPlans.EventType = EventTypeSubmit;
            }
            else if (trigger.ControlType == Enums.MarketingAutomationControlType.FormAbandon)
            {
                var formAbandon = trigger as FormAbandon;
                if (formAbandon != null)
                {
                    isCancelled = formAbandon.IsCancelled;
                    control = formAbandon;
                }
                layoutPlans.EventType = EventTypeAbandon;
            }
            else if (trigger.ControlType == Enums.MarketingAutomationControlType.Direct_Click)
            {
                var directClick = trigger as Direct_Click;
                if (directClick != null)
                {
                    layoutPlans.Criteria = !directClick.AnyLink
                        ? directClick.ActualLink
                        : string.Empty;
                    isCancelled = directClick.IsCancelled;
                    control = directClick;
                }
                layoutPlans.EventType = EventTypeClick;
            }
            else if (trigger.ControlType == Enums.MarketingAutomationControlType.Direct_Open)
            {
                var directOpen = trigger as Direct_Open;
                if (directOpen != null)
                {
                    isCancelled = directOpen.IsCancelled;
                    control = directOpen;
                }
                layoutPlans.EventType = EventTypeOpen;
            }
            else if (trigger.ControlType == Enums.MarketingAutomationControlType.Subscribe)
            {
                var subscribe = trigger as Subscribe;
                if (subscribe != null)
                {
                    isCancelled = subscribe.IsCancelled;
                    control = subscribe;
                }
                layoutPlans.EventType = EventTypeSubscribe;
            }
            else if (trigger.ControlType == Enums.MarketingAutomationControlType.Unsubscribe)
            {
                var unsubscribe = trigger as Unsubscribe;
                if (unsubscribe != null)
                {
                    isCancelled = unsubscribe.IsCancelled;
                    control = unsubscribe;
                }
                layoutPlans.EventType = EventTypeSubscribe;
            }

            return isCancelled;
        }

        private EntityCommunicator.LayoutPlans SetUpLayoutPlan(
            ref CampaignControlBase control,
            ref string layoutPlanStatus,
            ref bool bCancelAuto,
            ControlBase trigger,
            ControlBase parentWait,
            ControlBase parentForm,
            int parentCampaignItemId,
            int parentLayoutId)
        {
            var layoutPlans = new EntityCommunicator.LayoutPlans();
            var isCancelled = SetLayoutPlanEventType(ref control, trigger, layoutPlans);
            if (trigger.ECNID > 0)
            {
                layoutPlans = BusinessCommunicator.LayoutPlans.GetByLayoutPlanID_UseAmbientTransaction(
                    trigger.ECNID,
                    CurrentUser);
                layoutPlans.UpdatedUserID = CurrentUser.UserID;
                layoutPlans.UpdatedDate = DateTime.Now;
                layoutPlanStatus = layoutPlans.Status;
                bCancelAuto = isCancelled;
            }
            else
            {
                layoutPlans.CreatedUserID = CurrentUser.UserID;
                layoutPlans.CreatedDate = DateTime.Now;
            }

            var waitTotalDays = default(decimal);
            var wait = parentWait as Wait;
            if (wait != null)
            {
                var tsWait = new TimeSpan(
                    Convert.ToInt32(wait.Days),
                    Convert.ToInt32(wait.Hours),
                    Convert.ToInt32(wait.Minutes),
                    0);
                waitTotalDays = Convert.ToDecimal(tsWait.TotalDays);
            }
            layoutPlans.Period = waitTotalDays;

            var parentFormControl = parentForm as Form;
            if (parentFormControl != null)
            {
                layoutPlans.TokenUID = BusinessFormDesigner.Form.GetByFormID_NoAccessCheck(
                    parentFormControl.FormID).TokenUID;
                layoutPlans.Criteria = parentFormControl.ActualLink;
            }

            layoutPlans.CampaignItemID = parentCampaignItemId;
            layoutPlans.ActionName = trigger.Text;
            layoutPlans.LayoutID = parentLayoutId;
            layoutPlans.Status = isCancelled
                ? StatusN
                : control.ECNID > 0
                    ? layoutPlans.Status
                    : StatusY;
            layoutPlans.GroupID = null;
            return layoutPlans;
        }

        private void CreateBlastForLayout(
            ref EntityCommunicator.LayoutPlans layoutPlans,
            EntityCommunicator.CampaignItem campaignItem,
            EntityCommunicator.CampaignItemBlast campaignItemBlast,
            CampaignControlBase campaignControlBase,
            string layoutPlanStatus,
            bool cancelAuto)
        {
            //campaign item blast
            campaignItemBlast.CampaignItemID = campaignItem.CampaignItemID;
            campaignItemBlast.EmailSubject = campaignControlBase.EmailSubject;
            campaignItemBlast.LayoutID = campaignControlBase.MessageID;
            campaignItemBlast.CampaignItemBlastID = BusinessCommunicator.CampaignItemBlast.Save_UseAmbientTransaction(
                campaignItemBlast, 
                CurrentUser);

            //create blast
            BusinessCommunicator.Blast.CreateBlastsFromCampaignItem_UseAmbientTransaction(
                campaignItem.CampaignItemID, 
                CurrentUser);
            var blast = BusinessCommunicator.Blast.GetByCampaignItemBlastID_UseAmbientTransaction(
                campaignItemBlast.CampaignItemBlastID, 
                CurrentUser, 
                false);
            if (blast.CustomerID != null)
            {
                layoutPlans.CustomerID = blast.CustomerID.Value;
            }
            layoutPlans.BlastID = blast.BlastID;

            if (!layoutPlanStatus.Equals(StatusN, StringComparison.OrdinalIgnoreCase))
            {
                layoutPlans.LayoutPlanID = BusinessCommunicator.LayoutPlans.Save_UseAmbientTransaction(
                    layoutPlans, 
                    CurrentUser);
            }

            if (cancelAuto && layoutPlanStatus.Equals(StatusY, StringComparison.OrdinalIgnoreCase))
            {
                BusinessCommunicator.BlastSingle.DeleteForLayoutPlanID(layoutPlans.LayoutPlanID, CurrentUser);
            }
        }

        protected Group FindParentGroup(ControlBase childWait)
        {
            var parentGroup = new Group();
            var connector = AllConnectors.First(connectors => connectors.to.shapeId == childWait.ControlID);
            var index = 0;
            var isToContinue = true;
            if (connector != null)
            {
                while (isToContinue)
                {
                    if (AllControls.First(control => connector.from.shapeId == control.ControlID).
                            ControlType == Enums.MarketingAutomationControlType.Group)
                    {
                        parentGroup = AllControls.OfType<Group>().First(control =>
                            connector.from.shapeId == control.ControlID);
                        isToContinue = false;
                    }
                    else
                    {
                        connector = AllConnectors.First(connectors => connectors.to.shapeId == connector.from.shapeId);
                    }

                    index++;
                    if (index > AllControls.Count)
                    {
                        isToContinue = false;
                    }
                }
                return parentGroup;
            }
            return null;
        }

        protected int SaveSmartSegmentEmail_Click(
            ControlBase smartSegment, 
            ControlBase parent, 
            bool keepPaused = false, 
            bool isHome = false)
        {
            var wait = parent as Wait;
            if (wait == null)
            {
                throw new ArgumentNullException(nameof(wait));
            }

            var campaignItem = new EntityCommunicator.CampaignItem();
            var parentCampaignItem =
                BusinessCommunicator.CampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(
                    FindParent(wait).ECNID,
                    true);

            if (smartSegment.ECNID > 0)
            {
                campaignItem =
                    BusinessCommunicator.CampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransaction(
                        smartSegment.ECNID,
                        true);
                if (!isHome)
                {
                    campaignItem.UpdatedUserID = CurrentUser.UserID;
                }
            }
            else
            {
                if (parentCampaignItem.CampaignID != null)
                {
                    campaignItem.CampaignID = parentCampaignItem.CampaignID.Value;
                }

                if (!isHome)
                {
                    campaignItem.CreatedUserID = CurrentUser.UserID;
                }
            }

            if (isHome)
            {
                campaignItem.CreatedUserID = CurrentUser.UserID;
            }
            campaignItem.CustomerID = parentCampaignItem.CustomerID;

            var messageId = -1;
            var emailSubject = string.Empty;

            SetCampaignItemForSmartSegmentEmail(wait, smartSegment, ref campaignItem, out messageId, out emailSubject);

            AttachSmartSegmentToBlast(smartSegment, ref campaignItem, parentCampaignItem, messageId, emailSubject);

            foreach (var campaignItemBlast in campaignItem.BlastList)
            {
                BusinessCommunicator.CampaignItemBlastRefBlast.Delete(
                    campaignItemBlast.CampaignItemBlastID, 
                    CurrentUser);
            }

            campaignItem.CampaignItemID = BusinessCommunicator.CampaignItem.Save_UseAmbientTransaction(
                campaignItem,
                CurrentUser);

            var bOptOutMasterSuppression = SaveCampaignItemOptOutGroup(campaignItem);
            SaveBlastAndSuppression(campaignItem, bOptOutMasterSuppression, keepPaused);

            return campaignItem.CampaignItemID;
        }

        private void SetCampaignItemForSmartSegmentEmail(
            Wait wait,
            ControlBase smartSegment,
            ref EntityCommunicator.CampaignItem campaignItem,
            out int messageId,
            out string emailSubject)
        {
            messageId = -1;
            emailSubject = string.Empty;
            if (smartSegment.ControlType == Enums.MarketingAutomationControlType.Click ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.NoClick ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.NoOpen ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.NotSent ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.Open ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.Open_NoClick ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.Sent ||
                smartSegment.ControlType == Enums.MarketingAutomationControlType.Suppressed)
            {
                var controlBase = smartSegment as CampaignControlBase;
                if (controlBase == null)
                {
                    throw new ArgumentNullException(nameof(controlBase));
                }
                messageId = controlBase.MessageID;
                emailSubject = controlBase.EmailSubject;

                campaignItem.CampaignItemFormatType = HtmlCampaignItemFormatType;
                campaignItem.CampaignItemName = controlBase.CampaignItemName;
                campaignItem.CampaignItemNameOriginal = controlBase.CampaignItemName;
                campaignItem.CampaignItemType = RegularCampaignItemType;
                campaignItem.CompletedStep = CompletedStep5;
                campaignItem.CampaignItemTemplateID = controlBase.CampaignItemTemplateID;
                campaignItem.BlastField1 = controlBase.BlastField1;
                campaignItem.BlastField2 = controlBase.BlastField2;
                campaignItem.BlastField3 = controlBase.BlastField3;
                campaignItem.BlastField4 = controlBase.BlastField4;
                campaignItem.BlastField5 = controlBase.BlastField5;
                campaignItem.EnableCacheBuster = true;
                campaignItem.FromEmail = controlBase.FromEmail;
                campaignItem.FromName = controlBase.FromName;
                campaignItem.IgnoreSuppression = false;
                campaignItem.IsDeleted = false;
                campaignItem.IsHidden = false;
                campaignItem.ReplyTo = controlBase.ReplyTo;
            }
            
            // Calculate sendTime
            var actualSendTime = new DateTime();
            var actualsendTimeDec = GetTotalWaitTime(wait, AllControls, AllConnectors, ref actualSendTime);
            actualSendTime = actualSendTime.AddDays(Convert.ToDouble(actualsendTimeDec));
            campaignItem.SendTime = actualSendTime;
        }

        private bool SaveCampaignItemOptOutGroup(EntityCommunicator.CampaignItem campaignItem)
        {
            var bOptOutMasterSuppression = false;
            if (campaignItem.CampaignItemID > 0 && campaignItem.CampaignItemTemplateID > 0)
            {
                SaveLinkTrackingParamOptions(campaignItem);
                var itemTemplate = BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                    campaignItem.CampaignItemTemplateID.Value,
                    ECNSession.CurrentSession().CurrentUser,
                    true);

                if (itemTemplate.OptOutMasterSuppression != null)
                {
                    bOptOutMasterSuppression = itemTemplate.OptOutMasterSuppression.Value;
                }

                if (itemTemplate.OptOutSpecificGroup != null &&
                    itemTemplate.OptOutSpecificGroup.Value &&
                    itemTemplate.OptoutGroupList.Count > 0)
                {
                    foreach (var optOutGroup in itemTemplate.OptoutGroupList)
                    {
                        var itemOptOutGroup = new EntityCommunicator.CampaignItemOptOutGroup
                        {
                            CampaignItemID = campaignItem.CampaignItemID,
                            CustomerID = campaignItem.CustomerID,
                            CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                            GroupID = optOutGroup.GroupID
                        };
                        BusinessCommunicator.CampaignItemOptOutGroup.Save(
                            itemOptOutGroup,
                            ECNSession.CurrentSession().CurrentUser);
                    }
                }
            }

            return bOptOutMasterSuppression;
        }

        private void AttachSmartSegmentToBlast(
            ControlBase smartSegment,
            ref EntityCommunicator.CampaignItem campaignItem,
            EntityCommunicator.CampaignItem parentCampaignItem,
            int messageId,
            string emailSubject)
        {
            // get smart segment to use
            var segments = GetSmartSegmentList(smartSegment.ControlType);

            // clearing out existing blastList cause we should use parents every time.
            campaignItem.BlastList = new List<EntityCommunicator.CampaignItemBlast>();
            campaignItem.SuppressionList = new List<EntityCommunicator.CampaignItemSuppression>();

            // Attach smart segments to campaignitemblast
            foreach (var blast in parentCampaignItem.BlastList)
            {
                blast.LayoutID = messageId;
                blast.CampaignItemBlastID = -1;
                blast.EmailSubject = emailSubject;
                blast.FromName = campaignItem.FromName;
                blast.EmailFrom = campaignItem.FromEmail;
                blast.Filters = new List<EntityCommunicator.CampaignItemBlastFilter>();
                foreach (var segment in segments)
                {
                    if (blast.BlastID != null)
                    {
                        var blastFilter = new EntityCommunicator.CampaignItemBlastFilter
                        {
                            IsDeleted = false,
                            SmartSegmentID = segment.SmartSegmentID,
                            RefBlastIDs = blast.BlastID.Value.ToString()
                        };
                        blast.Filters.Add(blastFilter);
                    }
                }
                blast.BlastID = null;
                blast.Blast = null;
                campaignItem.BlastList.Add(blast);
            }
        }

        private List<EntityCommunicator.SmartSegment> GetSmartSegmentList(
            Enums.MarketingAutomationControlType controlType)
        {
            var segments = BusinessCommunicator.SmartSegment.GetSmartSegments();
            switch (controlType)
            {
                case Enums.MarketingAutomationControlType.Click:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameClick,StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case Enums.MarketingAutomationControlType.NoClick:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameUnClick, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case Enums.MarketingAutomationControlType.NoOpen:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameUnOpen, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case Enums.MarketingAutomationControlType.NotSent:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameNotSent, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case Enums.MarketingAutomationControlType.Open:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameOpen, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case Enums.MarketingAutomationControlType.Open_NoClick:
                    segments = segments.Where(segment =>
                            segment.SmartSegmentName.Equals(SegmentNameOpen, StringComparison.OrdinalIgnoreCase) ||
                            segment.SmartSegmentName.Equals(SegmentNameUnClick, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case Enums.MarketingAutomationControlType.Sent:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameSent, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case Enums.MarketingAutomationControlType.Suppressed:
                    segments = segments.Where(segment => segment.SmartSegmentName.Equals(
                        SegmentNameSuppressed, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
            }

            return segments;
        }
        
        protected ControlBase FindParent(ControlBase child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }
            var connector = AllConnectors.First(connectors => connectors.to.shapeId == child.ControlID);
            return connector != null
                ? AllControls.First(control => connector.from.shapeId == control.ControlID)
                : null;
        }

        protected decimal GetTotalWaitTime(
            Wait waitControl, 
            List<ControlBase> allControls, 
            List<Connector> allConnectors,
            ref DateTime parentSendTime)
        {
            var returnTime = DefaultReturnTime;
            var isToContinue = true;
            var controlCounter = 0;
            var totalControls = allControls.Count;
            parentSendTime = new DateTime();
            // want to include the initial controls wait time
            returnTime += Convert.ToDecimal(
                new TimeSpan(Convert.ToInt32(waitControl.Days),
                    Convert.ToInt32(waitControl.Hours),
                    Convert.ToInt32(waitControl.Minutes), 0).TotalDays);

            var startConnector = allConnectors.First(connectors => connectors.to.shapeId == waitControl.ControlID);
            while (isToContinue)
            {
                if (allControls.First(control => control.ControlID == startConnector.from.shapeId).
                        ControlType == Enums.MarketingAutomationControlType.Wait)
                {
                    var currentWait = allControls.OfType<Wait>().First(control =>
                        control.ControlID == startConnector.from.shapeId);

                    returnTime += Convert.ToDecimal(
                        new TimeSpan(Convert.ToInt32(currentWait.Days),
                            Convert.ToInt32(currentWait.Hours),
                            Convert.ToInt32(currentWait.Minutes), 0).TotalDays);
                }
                else if (allControls.First(control => control.ControlID == startConnector.from.shapeId).
                             ControlType == Enums.MarketingAutomationControlType.CampaignItem)
                {
                    // if we get here, we can stop calculating wait times
                    var parentCampaignItem = allControls.OfType<CampaignItem>().First(control =>
                        control.ControlID == startConnector.from.shapeId);

                    if (parentCampaignItem.SendTime != null)
                    {
                        parentSendTime = parentCampaignItem.SendTime.Value;
                    }
                    isToContinue = false;
                }
                else if (allControls.First(control => control.ControlID == startConnector.from.shapeId).
                             ControlType == Enums.MarketingAutomationControlType.Group)
                {
                    // if we get here, we can stop calculating wait times
                    parentSendTime = DateTime.Now;
                    isToContinue = false;
                }

                if (isToContinue)
                {
                    startConnector = allConnectors.First(connectors =>
                        connectors.to.shapeId == startConnector.from.shapeId);
                }

                controlCounter++;
                if (controlCounter > totalControls)
                {
                    isToContinue = false;
                }
            }

            return returnTime;
        }

        protected int SaveMessageTrigger(ControlBase trigger, ControlBase parentWait, ControlBase parentCi)
        {
            return SaveMessageOrFormTrigger(trigger, parentWait, parentCi, null, true);
        }

        private int SaveMessageOrFormTrigger(
            ControlBase trigger,
            ControlBase parentWait,
            ControlBase parentCi,
            ControlBase parentForm,
            bool isMessageTrigger)
        {
            var controlBase = new CampaignControlBase();
            var layoutPlanStatus = string.Empty;
            var cancelAuto = false;
            var parentCustomerId = GetParentCustomerId(parentWait);
            var parentLayoutId = -1;
            var parentCampaignItemId = -1;

            SetLayoutAndCampaignItemId(parentCi, isMessageTrigger, ref parentLayoutId, ref parentCampaignItemId);

            //set up layout plan
            var layoutPlans = SetUpLayoutPlan(
                ref controlBase,
                ref layoutPlanStatus,
                ref cancelAuto,
                trigger,
                parentWait,
                parentForm,
                parentCampaignItemId,
                parentLayoutId);

            //Create Blast for layout plan
            EntityCommunicator.CampaignItem campaignItem;
            EntityCommunicator.CampaignItemBlast campaignItemBlast;
            SetCampaignItemBlast(layoutPlans, out campaignItem, out campaignItemBlast, parentCustomerId, controlBase);

            var bOptOutMasterSuppression = false;
            if (campaignItem.CampaignItemID > 0 && campaignItem.CampaignItemTemplateID > 0)
            {
                SaveLinkTrackingParamOptions(campaignItem);
                var campaignItemTemplate = BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                    campaignItem.CampaignItemTemplateID.Value,
                    ECNSession.CurrentSession().CurrentUser,
                    true);

                if (campaignItemTemplate.OptOutMasterSuppression != null)
                {
                    bOptOutMasterSuppression = campaignItemTemplate.OptOutMasterSuppression.Value;
                }
                if (campaignItemTemplate.OptOutSpecificGroup != null &&
                    campaignItemTemplate.OptoutGroupList.Count > 0 &&
                    campaignItemTemplate.OptOutSpecificGroup.Value)
                {
                    foreach (var optoutGroup in campaignItemTemplate.OptoutGroupList)
                    {
                        var campaignItemOptOutGroup = new EntityCommunicator.CampaignItemOptOutGroup
                        {
                            CampaignItemID = campaignItem.CampaignItemID,
                            CustomerID = campaignItem.CustomerID,
                            CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                            GroupID = optoutGroup.GroupID
                        };
                        BusinessCommunicator.CampaignItemOptOutGroup.Save(
                            campaignItemOptOutGroup,
                            ECNSession.CurrentSession().CurrentUser);
                    }
                }
            }
            if (bOptOutMasterSuppression)
            {
                campaignItemBlast.AddOptOuts_to_MS = true;
            }
            campaignItemBlast.CustomerID = parentCustomerId;

            CreateBlastForLayout(
                ref layoutPlans,
                campaignItem,
                campaignItemBlast,
                controlBase,
                layoutPlanStatus,
                cancelAuto);

            return layoutPlans.LayoutPlanID;
        }

        protected void ValidateDelete(EntityCommunicator.MAControl controlToDelete)
        {
            switch (controlToDelete.ControlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgCampaignItem);
                    break;
                case Enums.MarketingAutomationControlType.Click:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgClick);
                    break;
                case Enums.MarketingAutomationControlType.Direct_Click:
                    ValidateDeleteByLayoutplan(controlToDelete, ErrorMsgDirectClick);
                    break;
                case Enums.MarketingAutomationControlType.Direct_Open:
                    ValidateDeleteByLayoutplan(controlToDelete, ErrorMsgDirectOpen);
                    break;
                case Enums.MarketingAutomationControlType.End:
                    break;
                case Enums.MarketingAutomationControlType.Group:
                    break;
                case Enums.MarketingAutomationControlType.NoClick:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgNoClick);
                    break;
                case Enums.MarketingAutomationControlType.NoOpen:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgNoOpen);
                    break;
                case Enums.MarketingAutomationControlType.NotSent:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgNotSent);
                    break;
                case Enums.MarketingAutomationControlType.Open:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgOpen);
                    break;
                case Enums.MarketingAutomationControlType.Open_NoClick:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgOpenNoClick);
                    break;
                case Enums.MarketingAutomationControlType.Sent:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgSent);
                    break;
                case Enums.MarketingAutomationControlType.Start:
                    break;
                case Enums.MarketingAutomationControlType.Subscribe:
                    ValidateDeleteByLayoutplan(controlToDelete, ErrorMsgSubscribe);
                    break;
                case Enums.MarketingAutomationControlType.Suppressed:
                    ValidateDeleteByCampaignItem(controlToDelete, ErrorMsgSuppressed);
                    break;
                case Enums.MarketingAutomationControlType.Unsubscribe:
                    ValidateDeleteByLayoutplan(controlToDelete, ErrorMsgUnSubscribe);
                    break;
                case Enums.MarketingAutomationControlType.Wait:
                    break;
            }
        }

        private void ValidateDeleteByCampaignItem(
            EntityCommunicator.MAControl controlToDelete,
            string exceptionMessage)
        {
            if (controlToDelete.ECNID > 0)
            {
                var campaignItem = BusinessCommunicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(
                    controlToDelete.ECNID, true);

                if (campaignItem.SendTime < DateTime.Now)
                {
                    throw new ECNException(exceptionMessage, new List<ECNError>());
                }
            }
        }

        private void ValidateDeleteByLayoutplan(EntityCommunicator.MAControl controlToDelete, string exceptionMessage)
        {
            if (controlToDelete.ECNID > 0)
            {
                var layoutPlans = BusinessCommunicator.LayoutPlans.GetByLayoutPlanID(
                    controlToDelete.ECNID,
                    CurrentUser);
                if (layoutPlans.BlastID != null)
                {
                    var blast = BusinessCommunicator.Blast.GetByBlastID_NoAccessCheck(
                        layoutPlans.BlastID.Value,
                        false);

                    if (blast.StatusCode.Equals(BlastStatusCodeSent, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ECNException(exceptionMessage, new List<ECNError>());
                    }
                }
            }
        }

        protected int SaveGroupTrigger(ControlBase trigger, ControlBase parentWait)
        {
            var campaignControlBase = new CampaignControlBase();
            var layoutPlanStatus = string.Empty;
            var cancelAuto = false;

            var parentGroup = GetParentGroup(parentWait);

            var layoutPlans = SetLayoutPlanForGroupTrigger(
                ref campaignControlBase,
                ref layoutPlanStatus,
                ref cancelAuto,
                trigger,
                parentWait,
                parentGroup);

            //Create Blast for layout plan
            EntityCommunicator.CampaignItem campaignItem = null;
            EntityCommunicator.CampaignItemBlast campaignItemBlast = null;

            SetCampaignItemBlast(
                layoutPlans,
                out campaignItem,
                out campaignItemBlast,
                parentGroup.CustomerID,
                campaignControlBase,
                true);

            var bOptOutMasterSuppression = false;
            if (campaignItem.CampaignItemID > 0 && campaignItem.CampaignItemTemplateID > 0)
            {
                SaveLinkTrackingParamOptions(campaignItem);
                var campaignItemTemplate = BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                    campaignItem.CampaignItemTemplateID.Value,
                    ECNSession.CurrentSession().CurrentUser,
                    true);

                if (campaignItemTemplate.OptOutMasterSuppression != null)
                {
                    bOptOutMasterSuppression = campaignItemTemplate.OptOutMasterSuppression.Value;
                }

                if (campaignItemTemplate.OptOutSpecificGroup != null &&
                    campaignItemTemplate.OptoutGroupList.Count > 0 &&
                    campaignItemTemplate.OptOutSpecificGroup.Value)
                {
                    foreach (var optoutGroup in campaignItemTemplate.OptoutGroupList)
                    {
                        var campaignItemOptOutGroup = new EntityCommunicator.CampaignItemOptOutGroup
                        {
                            CampaignItemID = campaignItem.CampaignItemID,
                            CustomerID = campaignItem.CustomerID,
                            CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                            GroupID = optoutGroup.GroupID
                        };
                        BusinessCommunicator.CampaignItemOptOutGroup.Save(campaignItemOptOutGroup,
                            ECNSession.CurrentSession().CurrentUser);
                    }
                }
            }

            //campaign item blast
            if (bOptOutMasterSuppression)
            {
                campaignItemBlast.AddOptOuts_to_MS = true;
            }
            campaignItemBlast.CustomerID = parentGroup.CustomerID;
            campaignItemBlast.GroupID = parentGroup.GroupID;

            CreateBlastForGroupTrigger(
                layoutPlans,
                campaignItem,
                campaignItemBlast,
                campaignControlBase,
                layoutPlanStatus,
                cancelAuto);

            return layoutPlans.LayoutPlanID;
        }

        private Group GetParentGroup(ControlBase parentWait)
        {
            Group parentGroupObject = null;
            if (parentWait.ControlType == Enums.MarketingAutomationControlType.Wait)
            {
                var wait = parentWait as Wait;
                parentGroupObject = FindParentGroup(wait);
            }
            else if (parentWait.ControlType == Enums.MarketingAutomationControlType.Group)
            {
                parentGroupObject = parentWait as Group;
            }

            return parentGroupObject;
        }

        private EntityCommunicator.LayoutPlans SetLayoutPlanForGroupTrigger(
            ref CampaignControlBase control,
            ref string layoutPlanStatus,
            ref bool cancelAuto,
            ControlBase trigger,
            ControlBase parentWait,
            Group parentGroupObject)
        {
            var layoutPlans = new EntityCommunicator.LayoutPlans();
            var isCancelled = SetLayoutPlanEventType(ref control, trigger, layoutPlans);

            if (trigger.ECNID > 0)
            {
                layoutPlans = BusinessCommunicator.LayoutPlans.GetByLayoutPlanID_UseAmbientTransaction(
                    trigger.ECNID,
                    CurrentUser);
                layoutPlans.UpdatedUserID = CurrentUser.UserID;
                layoutPlans.UpdatedDate = DateTime.Now;
                layoutPlanStatus = layoutPlans.Status;
                cancelAuto = isCancelled;
            }
            else
            {
                layoutPlans.CreatedUserID = CurrentUser.UserID;
                layoutPlans.CreatedDate = DateTime.Now;
            }

            var waitTotalDays = default(decimal);
            var wait = parentWait as Wait;
            if (wait != null)
            {
                var tsWait = new TimeSpan(
                    Convert.ToInt32(wait.Days),
                    Convert.ToInt32(wait.Hours),
                    Convert.ToInt32(wait.Minutes),
                    0);
                waitTotalDays = Convert.ToDecimal(tsWait.TotalDays);
            }
            layoutPlans.Period = waitTotalDays;

            var isSubscribe = control.GetType() == typeof(Subscribe);

            var controlTypeName = isSubscribe
                ? ActionNameSubscribe
                : ActionNameUnsubscribe;
            layoutPlans.ActionName = $"{parentGroupObject.GroupName}_{controlTypeName} {ActionNameTrigger}";

            layoutPlans.CustomerID = parentGroupObject.CustomerID;
            layoutPlans.LayoutID = null;

            layoutPlans.GroupID = parentGroupObject.GroupID;
            layoutPlans.Criteria = isSubscribe
                ? CriteriaS
                : CriteriaU;
            layoutPlans.Status = isCancelled
                ? StatusN
                : control.ECNID > 0
                    ? layoutPlans.Status
                    : StatusY;

            return layoutPlans;
        }

        private void CreateBlastForGroupTrigger(
            EntityCommunicator.LayoutPlans layoutPlans,
            EntityCommunicator.CampaignItem campaignItem,
            EntityCommunicator.CampaignItemBlast campaignItemBlast,
            CampaignControlBase campaignControlBase,
            string layoutPlanStatus,
            bool cancelAuto)
        {
            campaignItemBlast.CampaignItemID = campaignItem.CampaignItemID;
            campaignItemBlast.EmailSubject = campaignControlBase.EmailSubject;
            campaignItemBlast.LayoutID = campaignControlBase.MessageID;
            BusinessCommunicator.CampaignItemBlast.Save_UseAmbientTransaction(campaignItemBlast, CurrentUser);

            //create blast
            BusinessCommunicator.Blast.CreateBlastsFromCampaignItem_UseAmbientTransaction(
                campaignItem.CampaignItemID,
                CurrentUser);

            var blast = BusinessCommunicator.Blast.GetByCampaignItemBlastID_UseAmbientTransaction(
                campaignItemBlast.CampaignItemBlastID,
                CurrentUser,
                false);

            layoutPlans.BlastID = blast.BlastID;

            if (cancelAuto && layoutPlanStatus.Equals(StatusY, StringComparison.OrdinalIgnoreCase))
            {
                BusinessCommunicator.BlastSingle.DeleteForLayoutPlanID(layoutPlans.LayoutPlanID, CurrentUser);
            }

            if (!layoutPlanStatus.Equals(StatusN, StringComparison.OrdinalIgnoreCase))
            {
                layoutPlans.LayoutPlanID = BusinessCommunicator.LayoutPlans.Save_UseAmbientTransaction(
                    layoutPlans,
                    CurrentUser);
            }
        }

        protected void ValidatePublish(
            List<ControlBase> controls,
            EntityCommunicator.MarketingAutomation automation,
            ref ECNException ecnMaster)
        {
            foreach (var controlBase in controls)
            {
                //validate that end control exists
                ValidateEndControlExists(controlBase, ref ecnMaster);
                try
                {
                    switch (controlBase.ControlType)
                    {
                        case Enums.MarketingAutomationControlType.CampaignItem:
                            var campaignItem = controlBase as CampaignItem;
                            if (campaignItem != null)
                            {
                                campaignItem.Validate(CurrentUser);
                            }
                            break;
                        case Enums.MarketingAutomationControlType.Click:
                        case Enums.MarketingAutomationControlType.NoClick:
                        case Enums.MarketingAutomationControlType.NoOpen:
                        case Enums.MarketingAutomationControlType.NotSent:
                        case Enums.MarketingAutomationControlType.Open:
                        case Enums.MarketingAutomationControlType.Open_NoClick:
                        case Enums.MarketingAutomationControlType.Sent:
                        case Enums.MarketingAutomationControlType.Suppressed:
                            ValidateControls(controlBase, automation);
                            break;
                        case Enums.MarketingAutomationControlType.Form:
                            var formObject = controlBase as Form;
                            if (formObject != null)
                            {
                                formObject.Validate(FindParent(controlBase), CurrentUser);
                            }
                            break;
                        case Enums.MarketingAutomationControlType.Direct_Click:
                        case Enums.MarketingAutomationControlType.FormSubmit:
                        case Enums.MarketingAutomationControlType.FormAbandon:
                            ValidateFormActionAndDirectClick(controlBase);
                            break;
                        case Enums.MarketingAutomationControlType.Direct_Open:
                        case Enums.MarketingAutomationControlType.Direct_NoOpen:
                            ValidateDirectOpenAndNoOpen(controlBase, automation);
                            break;
                        case Enums.MarketingAutomationControlType.Group:
                            var groupObject = controlBase as Group;
                            if (groupObject != null)
                            {
                                groupObject.Validate();
                            }
                            break;
                        case Enums.MarketingAutomationControlType.Subscribe:
                        case Enums.MarketingAutomationControlType.Unsubscribe:
                            ValidateSubscribeControl(controlBase);
                            break;
                        case Enums.MarketingAutomationControlType.Wait:
                            var waitObject = controlBase as Wait;
                            if (waitObject != null)
                            {
                                waitObject.Validate(FindParent(waitObject), automation, CurrentUser);
                            }
                            break;
                    }
                }
                catch (ECNException ecnException)
                {
                    foreach (var error in ecnException.ErrorList)
                    {
                        ecnMaster.ErrorList.Add(error);
                    }
                }
                catch (Exception)
                {
                    ecnMaster.ErrorList.Add(new ECNError(
                        CommonEnums.Entity.MarketingAutomation,
                        CommonEnums.Method.Validate,
                        $"{UnableToValidateMessage} {controlBase.Text}"));
                }
            }
        }

        private bool GetGroupAndCampaignItem(
            ControlBase controlBase,
            Wait wait,
            ref CampaignItem campaignItem,
            ref Group groupObject)
        {
            var hasCampaignItem = false;
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[AppSettingKmApplication]);
            if (wait != null)
            {
                try
                {
                    campaignItem = FindParentCampaignItem(wait);
                    if (campaignItem != null)
                    {
                        hasCampaignItem = true;
                    }
                }
                catch (Exception exception)
                {
                    ApplicationLog.LogNonCriticalError(exception, MethodNameGetGroupAndCampaignItem, applicationId);
                }

                try
                {
                    groupObject = FindParentGroup(wait);
                }
                catch (Exception exception)
                {
                    ApplicationLog.LogNonCriticalError(exception, MethodNameGetGroupAndCampaignItem, applicationId);
                }
            }
            else
            {
                try
                {
                    campaignItem = FindParentCampaignItem(controlBase);
                    if (campaignItem != null)
                    {
                        hasCampaignItem = true;
                    }
                }
                catch (Exception exception)
                {
                    ApplicationLog.LogNonCriticalError(exception, MethodNameGetGroupAndCampaignItem, applicationId);
                }

                try
                {
                    groupObject = FindParentGroup(controlBase);
                }
                catch (Exception exception)
                {
                    ApplicationLog.LogNonCriticalError(exception, MethodNameGetGroupAndCampaignItem, applicationId);
                }
            }

            return hasCampaignItem;
        }

        private void ValidateEndControlExists(ControlBase controlBase, ref ECNException ecnMaster)
        {
            if (controlBase.ControlType != Enums.MarketingAutomationControlType.Start &&
                controlBase.ControlType != Enums.MarketingAutomationControlType.End)
            {
                var childConnect = AllConnectors.FirstOrDefault(connector =>
                    connector.from.shapeId == controlBase.ControlID);

                if (childConnect == null)
                {
                    ecnMaster.ErrorList.Add(new ECNError(
                        CommonEnums.Entity.CampaignItem,
                        CommonEnums.Method.Validate,
                        MissingEndControlMessage));
                }
            }
        }

        private void ValidateControls(
            ControlBase controlBase,
            EntityCommunicator.MarketingAutomation automation)
        {
            var waitObject = FindParentWait(controlBase);
            var campaignItem = FindParentCampaignItem(waitObject);
            var control = controlBase as IValidateControl;
            if (control != null)
            {
                control.Validate(waitObject, campaignItem, automation, CurrentUser);
            }
        }

        private void ValidateFormActionAndDirectClick(ControlBase controlBase)
        {
            var campaignItem = default(CampaignItem);
            var groupObject = default(Group);
            var triggerWait = FindParentWait(controlBase);

            var parent = controlBase.ControlType == Enums.MarketingAutomationControlType.Direct_Click
                ? FindParent(triggerWait)
                : FindParent(FindParent(triggerWait));

            var hasItem = GetGroupAndCampaignItem(controlBase, triggerWait, ref campaignItem, ref groupObject);

            var formAndDirectClick = controlBase as IValidateFormAndDirectClick;
            if (formAndDirectClick != null)
            {
                formAndDirectClick.Validate(triggerWait, hasItem, campaignItem, groupObject, parent, CurrentUser);
            }
        }

        private void ValidateDirectOpenAndNoOpen(
            ControlBase controlBase,
            EntityCommunicator.MarketingAutomation automation)
        {
            var campaignItem = default(CampaignItem);
            var groupObject = default(Group);
            var triggerWait = FindParentWait(controlBase);
            var hasItem = GetGroupAndCampaignItem(controlBase, triggerWait, ref campaignItem, ref groupObject);

            var openAndNoOpen = controlBase as IValidateDirectOpenAndNoOpen;
            if (openAndNoOpen != null)
            {
                openAndNoOpen.Validate(triggerWait, hasItem, campaignItem, groupObject, automation, CurrentUser);
            }
        }

        private void ValidateSubscribeControl(ControlBase controlBase)
        {
            var subscribeWait = FindParentWait(controlBase);
            var subscribeGroup = subscribeWait != null
                ? FindParentGroup(subscribeWait)
                : FindParentGroup(controlBase);
            var subscribeControl = controlBase as IValidateSubscribeControl;
            if (subscribeControl != null)
            {
                subscribeControl.Validate(subscribeGroup, CurrentUser);
            }
        }

        protected int SaveTriggerPlan(
            ControlBase trigger, 
            ControlBase parentWait, 
            ControlBase parentDirectObject, 
            bool isHome = false)
        {
            var directNoOpenTrigger = trigger as Direct_NoOpen;
            var layoutPlanCustomerId = 0;
            var triggerPlanStatus = string.Empty;
            var cancelAuto = false;
            var triggerPlans = SetTriggerPlan(
                parentWait,
                directNoOpenTrigger,
                ref layoutPlanCustomerId,
                ref triggerPlanStatus,
                ref cancelAuto);

            EntityCommunicator.CampaignItem campaignItem = null;
            EntityCommunicator.CampaignItemBlast campaignItemBlast = null;
            SetCampaignItemBlast(
                triggerPlans,
                out campaignItem,
                out campaignItemBlast,
                layoutPlanCustomerId,
                directNoOpenTrigger);

            campaignItemBlast.CustomerID = layoutPlanCustomerId;

            var bOptOutMasterSuppression = false;
            if (campaignItem.CampaignItemID > 0 && campaignItem.CampaignItemTemplateID > 0)
            {
                SaveLinkTrackingParamOptions(campaignItem);
                var campaignItemTemplate = BusinessCommunicator.CampaignItemTemplate.GetByCampaignItemTemplateID(
                    campaignItem.CampaignItemTemplateID.Value,
                    ECNSession.CurrentSession().CurrentUser,
                    true);

                if (campaignItemTemplate.OptOutMasterSuppression != null)
                {
                    bOptOutMasterSuppression = campaignItemTemplate.OptOutMasterSuppression.Value;
                }
                if (campaignItemTemplate.OptOutSpecificGroup != null &&
                    campaignItemTemplate.OptOutSpecificGroup.Value &&
                    campaignItemTemplate.OptoutGroupList.Count > 0)
                {
                    foreach (var optgl in campaignItemTemplate.OptoutGroupList)
                    {
                        var itemOptOutGroup = new EntityCommunicator.CampaignItemOptOutGroup
                        {
                            CampaignItemID = campaignItem.CampaignItemID,
                            CustomerID = campaignItem.CustomerID,
                            CreatedUserID = ECNSession.CurrentSession().CurrentUser.UserID,
                            GroupID = optgl.GroupID
                        };
                        BusinessCommunicator.CampaignItemOptOutGroup.Save(
                            itemOptOutGroup,
                            ECNSession.CurrentSession().CurrentUser);
                    }
                }
            }

            if (bOptOutMasterSuppression)
            {
                campaignItemBlast.AddOptOuts_to_MS = true;
            }

            CreateBlastForTriggerPlan(
                triggerPlans,
                campaignItem,
                campaignItemBlast,
                directNoOpenTrigger,
                triggerPlanStatus,
                cancelAuto,
                isHome);

            return triggerPlans.TriggerPlanID;
        }

        private EntityCommunicator.TriggerPlans SetTriggerPlan(
            ControlBase parentWait,
            Direct_NoOpen directNoOpenTrigger,
            ref int layoutPlanCustomerId,
            ref string triggerPlanStatus,
            ref bool cancelAuto)
        {
            var triggerPlans = new EntityCommunicator.TriggerPlans();
            var wait = parentWait as Wait;
            var actionNameText = string.Empty;
            var ecnId = 0;
            var isCancelled = GetDirectOrFormData(wait, out ecnId, out actionNameText);

            var parentLayoutPlans = BusinessCommunicator.LayoutPlans.GetByLayoutPlanID_UseAmbientTransaction(
                ecnId,
                CurrentUser);

            if (parentLayoutPlans.CustomerID != null)
            {
                layoutPlanCustomerId = parentLayoutPlans.CustomerID.Value;
            }

            if (directNoOpenTrigger.ECNID > 0)
            {
                triggerPlans = BusinessCommunicator.TriggerPlans.GetByTriggerPlanID(directNoOpenTrigger.ECNID, CurrentUser);
                triggerPlans.UpdatedDate = DateTime.Now;
                triggerPlans.UpdatedUserID = CurrentUser.UserID;
                triggerPlanStatus = triggerPlans.Status;
                cancelAuto = isCancelled;
            }
            else
            {
                triggerPlans.CreatedDate = DateTime.Now;
                triggerPlans.CreatedUserID = CurrentUser.UserID;
                triggerPlans.Status = StatusY;
            }
            triggerPlans.ActionName = $"{ActionNamePrefix} {actionNameText}";

            var waitTotalDays = default(decimal);
            if (wait != null)
            {
                var tsWait = new TimeSpan(
                    Convert.ToInt32(wait.Days),
                    Convert.ToInt32(wait.Hours),
                    Convert.ToInt32(wait.Minutes),
                    0);
                waitTotalDays = Convert.ToDecimal(tsWait.TotalDays);
            }

            triggerPlans.Period = waitTotalDays;
            triggerPlans.GroupID = 0;
            triggerPlans.EventType = EventTypeNoOpen;
            if (parentLayoutPlans.BlastID != null)
            {
                triggerPlans.refTriggerID = parentLayoutPlans.BlastID.Value;
            }

            triggerPlans.Status = directNoOpenTrigger.IsCancelled 
                ? StatusN 
                : triggerPlans.Status;

            triggerPlans.Criteria = string.Empty;

            return triggerPlans;
        }

        private void CreateBlastForTriggerPlan(
            EntityCommunicator.TriggerPlans triggerPlans,
            EntityCommunicator.CampaignItem campaignItem,
            EntityCommunicator.CampaignItemBlast campaignItemBlast,
            Direct_NoOpen directNoOpenTrigger,
            string triggerPlanStatus,
            bool cancelAuto,
            bool isHome)
        {
            // campaign item blast
            campaignItemBlast.CampaignItemID = campaignItem.CampaignItemID;
            campaignItemBlast.EmailSubject = directNoOpenTrigger.EmailSubject;
            campaignItemBlast.LayoutID = directNoOpenTrigger.MessageID;

            campaignItemBlast.CampaignItemBlastID = BusinessCommunicator.CampaignItemBlast.Save_UseAmbientTransaction(
                campaignItemBlast,
                CurrentUser);

            // create blast
            BusinessCommunicator.Blast.CreateBlastsFromCampaignItem_UseAmbientTransaction(
                campaignItem.CampaignItemID,
                CurrentUser);

            var blast = BusinessCommunicator.Blast.GetByCampaignItemBlastID_UseAmbientTransaction(
                campaignItemBlast.CampaignItemBlastID,
                CurrentUser,
                false);

            if (blast.CustomerID != null)
            {
                triggerPlans.CustomerID = blast.CustomerID.Value;
            }

            triggerPlans.BlastID = blast.BlastID;

            if (isHome)
            {
                if (!triggerPlanStatus.Equals(StatusN, StringComparison.OrdinalIgnoreCase))
                {
                    triggerPlans.TriggerPlanID = BusinessCommunicator.TriggerPlans.Save_UseAmbientTransaction(
                        triggerPlans,
                        CurrentUser);
                }
                if (cancelAuto && triggerPlanStatus.Equals(StatusY, StringComparison.OrdinalIgnoreCase))
                {
                    BusinessCommunicator.BlastSingle.DeleteForTriggerPlan(
                        triggerPlans.TriggerPlanID,
                        blast.BlastID,
                        CurrentUser);
                }
            }
            else
            {
                if (cancelAuto && triggerPlanStatus.Equals(StatusY, StringComparison.OrdinalIgnoreCase))
                {
                    BusinessCommunicator.BlastSingle.DeleteForTriggerPlan(
                        triggerPlans.TriggerPlanID,
                        blast.BlastID,
                        CurrentUser);
                }
                if (!triggerPlanStatus.Equals(StatusN, StringComparison.OrdinalIgnoreCase))
                {
                    triggerPlans.TriggerPlanID = BusinessCommunicator.TriggerPlans.Save_UseAmbientTransaction(
                        triggerPlans,
                        CurrentUser);
                }
            }
        }

        private bool GetDirectOrFormData(Wait wait, out int ecnId, out string actionNameText)
        {
            var isCancelled = false;
            ecnId = 0;
            actionNameText = string.Empty;
            var parentDirectOrForm = GetParentDirectOrForm(wait);
            switch (parentDirectOrForm)
            {
                case ParentDirectOrForm1:
                    var directOpen = FindParentDirectEmail(wait) as Direct_Open;
                    if (directOpen != null)
                    {
                        ecnId = directOpen.ECNID;
                        isCancelled = directOpen.IsCancelled;
                        actionNameText = directOpen.Text;
                    }
                    break;
                case ParentDirectOrForm2:
                    var formAbandon = FindParentDirectEmail(wait) as FormAbandon;
                    if (formAbandon != null)
                    {
                        ecnId = formAbandon.ECNID;
                        isCancelled = formAbandon.IsCancelled;
                        actionNameText = formAbandon.Text;
                    }
                    break;
                case ParentDirectOrForm3:
                    var formSubmit = FindParentDirectEmail(wait) as FormSubmit;
                    if (formSubmit != null)
                    {
                        ecnId = formSubmit.ECNID;
                        isCancelled = formSubmit.IsCancelled;
                        actionNameText = formSubmit.Text;
                    }
                    break;
                default:
                    var directClick = FindParentDirectEmail(wait) as Direct_Click;
                    if (directClick != null)
                    {
                        ecnId = directClick.ECNID;
                        isCancelled = directClick.IsCancelled;
                        actionNameText = directClick.Text;
                    }
                    break;
            }

            return isCancelled;
        }

        private int GetParentDirectOrForm(Wait wait)
        {
            var connector = AllConnectors.First(connectors => connectors.to.shapeId == wait.ControlID);
            if (connector != null)
            {
                if (AllControls.Exists(control => 
                    connector.from.shapeId == control.ControlID &&
                    control.ControlType == Enums.MarketingAutomationControlType.Direct_Open))
                {
                    return ParentDirectOrForm1;
                }

                if (AllControls.Exists(control => 
                    connector.from.shapeId == control.ControlID &&
                    control.ControlType == Enums.MarketingAutomationControlType.FormAbandon))
                {
                    return ParentDirectOrForm2;
                }

                if (AllControls.Exists(control =>
                    connector.from.shapeId == control.ControlID &&
                    control.ControlType == Enums.MarketingAutomationControlType.FormSubmit))
                {
                    return ParentDirectOrForm3;
                }
            }
            return ParentDirectOrForm0;
        }

        protected ControlBase FindParentDirectEmail(Wait wait)
        {
            var connector = AllConnectors.First(connectors => connectors.to.shapeId == wait.ControlID);
            return connector != null 
                ? AllControls.First(control => connector.from.shapeId == control.ControlID) 
                : null;
        }

        protected void BaseDeleteEcnObject(EntityCommunicator.MAControl maControl, bool isHome)
        {
            switch (maControl.ControlType)
            {
                case Enums.MarketingAutomationControlType.CampaignItem:
                case Enums.MarketingAutomationControlType.Click:
                case Enums.MarketingAutomationControlType.NoClick:
                case Enums.MarketingAutomationControlType.NoOpen:
                case Enums.MarketingAutomationControlType.NotSent:
                case Enums.MarketingAutomationControlType.Open:
                case Enums.MarketingAutomationControlType.Open_NoClick:
                case Enums.MarketingAutomationControlType.Sent:
                case Enums.MarketingAutomationControlType.Suppressed:
                    BusinessCommunicator.CampaignItem.Delete(maControl.ECNID, CurrentUser, false);
                    break;
                case Enums.MarketingAutomationControlType.Direct_Click:
                case Enums.MarketingAutomationControlType.Direct_Open:
                case Enums.MarketingAutomationControlType.Subscribe:
                case Enums.MarketingAutomationControlType.Unsubscribe:
                    var layoutPlans = BusinessCommunicator.LayoutPlans.GetByLayoutPlanID(maControl.ECNID, CurrentUser);
                    if (layoutPlans.BlastID != null)
                    {
                        var campaignItem = BusinessCommunicator.CampaignItem.GetByBlastID_NoAccessCheck(
                            layoutPlans.BlastID.Value,
                            false);
                        BusinessCommunicator.CampaignItem.Delete(campaignItem.CampaignItemID, CurrentUser, false);
                        BusinessCommunicator.LayoutPlans.DeleteByLayoutPlanID(layoutPlans.LayoutPlanID, CurrentUser);
                    }
                    break;
                case Enums.MarketingAutomationControlType.Direct_NoOpen:
                    if (!isHome)
                    {
                        var triggerPlans = BusinessCommunicator.TriggerPlans.GetByTriggerPlanID(
                            maControl.ECNID,
                            CurrentUser);
                        if (triggerPlans.BlastID != null)
                        {
                            var campaignItem = BusinessCommunicator.CampaignItem.GetByBlastID_NoAccessCheck(
                                triggerPlans.BlastID.Value,
                                false);
                            BusinessCommunicator.CampaignItem.Delete(campaignItem.CampaignItemID, CurrentUser, false);
                        }

                        BusinessCommunicator.TriggerPlans.Delete(triggerPlans.TriggerPlanID, CurrentUser);
                    }
                    break;
                case Enums.MarketingAutomationControlType.End:
                case Enums.MarketingAutomationControlType.Group:
                case Enums.MarketingAutomationControlType.Start:
                case Enums.MarketingAutomationControlType.Wait:
                    break;
            }
        }

        protected void BaseSaveChildren(
            ControlBase parent,
            List<ControlBase> children,
            int automationId,
            bool keepPaused,
            bool isHome)
        {
            foreach (var startChild in children)
            {
                var isDirty = false;
                var connectorExists = false;
                switch (startChild.ControlType)
                {
                    case Enums.MarketingAutomationControlType.CampaignItem:
                        CampaignItemSaveChild(startChild, automationId, ref isDirty, ref connectorExists, keepPaused);
                        break;
                    case Enums.MarketingAutomationControlType.Direct_Click:
                    case Enums.MarketingAutomationControlType.Direct_Open:
                        DirectOpenClickSaveChild(startChild, automationId, ref isDirty, ref connectorExists, isHome);
                        break;
                    case Enums.MarketingAutomationControlType.Form:
                        FormSaveChild(startChild, automationId, ref isDirty, ref connectorExists, isHome);
                        break;
                    case Enums.MarketingAutomationControlType.FormAbandon:
                    case Enums.MarketingAutomationControlType.FormSubmit:
                        FormAbandonSubmitSaveChild(startChild, automationId, ref isDirty, ref connectorExists, isHome);
                        break;
                    case Enums.MarketingAutomationControlType.Direct_NoOpen:
                        DirectNoOpenSaveChild(startChild, automationId, ref isDirty, ref connectorExists, isHome);
                        break;
                    case Enums.MarketingAutomationControlType.End:
                        EndSaveChild(startChild, automationId, ref connectorExists);
                        break;
                    case Enums.MarketingAutomationControlType.Group:
                        GroupSaveChild(startChild, automationId, ref isDirty, ref connectorExists);
                        break;
                    case Enums.MarketingAutomationControlType.Click:
                    case Enums.MarketingAutomationControlType.NoClick:
                    case Enums.MarketingAutomationControlType.NoOpen:
                    case Enums.MarketingAutomationControlType.NotSent:
                    case Enums.MarketingAutomationControlType.Open:
                    case Enums.MarketingAutomationControlType.Open_NoClick:
                    case Enums.MarketingAutomationControlType.Sent:
                    case Enums.MarketingAutomationControlType.Suppressed:
                        ClickSaveChild(
                            parent,
                            startChild,
                            automationId,
                            ref isDirty,
                            ref connectorExists,
                            keepPaused,
                            isHome);
                        break;
                    case Enums.MarketingAutomationControlType.Subscribe:
                    case Enums.MarketingAutomationControlType.Unsubscribe:
                        SubscribeSaveChild(parent, startChild, automationId, ref isDirty, ref connectorExists, isHome);
                        break;
                    case Enums.MarketingAutomationControlType.Wait:
                        WaitSaveChild(startChild, automationId, ref isDirty, ref connectorExists);
                        break;
                }

                SaveConnector(connectorExists, parent, startChild, automationId);

                var childrenList = GetChildrenFromGlobalList(startChild);
                if (isDirty)
                {
                    childrenList.ForEach(child => child.IsDirty = true);
                }
                BaseSaveChildren(startChild, childrenList, automationId, keepPaused, isHome);
            }
        }

        private void CampaignItemSaveChild(
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists,
            bool keepPaused)
        {
            var campaignItem = startChild as CampaignItem;
            if (campaignItem == null)
            {
                return;
            }

            if (!campaignItem.editable.remove && campaignItem.MAControlID > 0)
            {
                // Cant make edits to ECN object but can update diagram control
                SaveExistingMaControl(startChild, automationId);
                connectorExists = true;
            }
            else
            {
                if (campaignItem.MAControlID > 0)
                {
                    connectorExists = true;
                    // This means its an update if it's Dirty
                    if (campaignItem.IsDirty)
                    {
                        if (campaignItem.CreateCampaignItem)
                        {
                            // pull existing CI in DB
                            campaignItem.ECNID = SaveCampaignItem(campaignItem, keepPaused);
                            forceIsDirty = true;
                        }
                        else if (!campaignItem.CreateCampaignItem && campaignItem.SubCategory == SubCategoryGroup)
                        {
                            // Update sendtime again cause it might have changed
                            CampaignItemUpdateSendTime(campaignItem);
                        }
                    }

                    // moving this outside the isdirty check because it might be an existing campaign item
                    var maControl = BusinessCommunicator.MAControl.GetByControlID(campaignItem.ControlID, automationId);
                    if (!campaignItem.CreateCampaignItem)
                    {
                        maControl.ControlID = campaignItem.ControlID;
                        maControl.ControlType = campaignItem.ControlType;
                        maControl.ECNID = campaignItem.ECNID;
                        maControl.ExtraText = string.Empty;
                        maControl.MarketingAutomationID = automationId;
                    }
                    maControl.Text = campaignItem.Text;
                    maControl.xPosition = campaignItem.xPosition;
                    maControl.yPosition = campaignItem.yPosition;
                    BusinessCommunicator.MAControl.Save(maControl);
                }
                else
                {
                    // New Save
                    if (campaignItem.CampaignItemID > 0 && !campaignItem.CreateCampaignItem)
                    {
                        if (campaignItem.SubCategory == SubCategoryGroup)
                        {
                            // Have to update existing campaign items send time
                            CampaignItemUpdateSendTime(campaignItem);
                        }
                        campaignItem.ECNID = campaignItem.CampaignItemID;
                    }
                    else
                    {
                        campaignItem.ECNID = SaveCampaignItem(campaignItem, keepPaused);
                    }
                    SaveNewMaControl(campaignItem, campaignItem.ECNID, campaignItem.Text, automationId);
                }
                startChild.ECNID = campaignItem.ECNID;
                UpdateGlobalControlListWithEcnid(campaignItem);
            }
        }

        private void CampaignItemUpdateSendTime(CampaignItem campaignItem)
        {
            var parentWait = FindParentWait(campaignItem);
            var parentCampaignItem = FindParentCampaignItem(parentWait);
            if (parentCampaignItem.SendTime != null)
            {
                BusinessCommunicator.CampaignItem.UpdateSendTime(
                    campaignItem.CampaignItemID,
                    parentCampaignItem.SendTime.Value.AddDays(Convert.ToDouble(parentWait.WaitTime)));
            }
        }

        private void DirectOpenClickSaveChild(
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists,
            bool isHome)
        {
            var clickWait = FindParentWait(startChild);
            if (startChild.ECNID > 0)
            {
                connectorExists = true;
                // an Update if the object is dirty
                if (startChild.IsDirty || isHome)
                {
                    SaveMessageTrigger(startChild, clickWait, FindParent(clickWait));
                    forceIsDirty = true;
                }
                SaveExistingMaControl(startChild, automationId);
            }
            else
            {
                // new save                                                       
                startChild.ECNID = SaveMessageTrigger(startChild, clickWait, FindParent(clickWait));
                SaveNewMaControl(startChild, startChild.ECNID, startChild.Text, automationId);
            }

            UpdateGlobalControlListWithEcnid(startChild);
        }

        private void FormSaveChild(
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists,
            bool isHome)
        {
            var formTriggerObject = startChild as Form;
            if (formTriggerObject == null)
            {
                return;
            }

            if (formTriggerObject.ECNID > 0)
            {
                connectorExists = true;
                // an Update if the object is dirty
                if (formTriggerObject.IsDirty || isHome)
                {
                    forceIsDirty = true;
                }
                var maControl = BusinessCommunicator.MAControl.GetByControlID(formTriggerObject.ControlID, automationId);
                maControl.ECNID = formTriggerObject.FormID;
                maControl.Text = formTriggerObject.Text;
                maControl.xPosition = formTriggerObject.xPosition;
                maControl.yPosition = formTriggerObject.yPosition;
                BusinessCommunicator.MAControl.Save(maControl);
            }
            else
            {
                SaveNewMaControl(formTriggerObject, formTriggerObject.FormID, formTriggerObject.Text, automationId);
            }
            startChild.ECNID = formTriggerObject.FormID;
            UpdateGlobalControlListWithEcnid(formTriggerObject);
        }

        private void FormAbandonSubmitSaveChild(
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists,
            bool isHome)
        {
            var parentWait = FindParentWait(startChild);
            var formParent = parentWait != null
                ? FindFormParent(parentWait)
                : FindFormParent(startChild);

            if (startChild.ECNID > 0)
            {
                connectorExists = true;
                // an Update if the object is dirty
                if (startChild.IsDirty || isHome)
                {
                    SaveFormControlTrigger(startChild, parentWait, formParent, FindParent(parentWait));
                    forceIsDirty = true;
                }
                SaveExistingMaControl(startChild, automationId);
            }
            else
            {
                // new save 
                startChild.ECNID = SaveFormControlTrigger(startChild, parentWait, formParent, FindParent(parentWait));
                SaveNewMaControl(startChild, startChild.ECNID, startChild.Text, automationId);
            }

            UpdateGlobalControlListWithEcnid(startChild);
        }

        private void DirectNoOpenSaveChild(
             ControlBase startChild,
             int automationId,
             ref bool forceIsDirty,
             ref bool connectorExists,
             bool isHome)
        {
            var noOpenWait = FindParentWait(startChild);
            if (startChild.ECNID > 0)
            {
                connectorExists = true;

                if (startChild.IsDirty || isHome)
                {
                    forceIsDirty = true;
                    SaveTriggerPlan(startChild, noOpenWait, FindParentDirectEmail(noOpenWait), isHome);
                }
                SaveExistingMaControl(startChild, automationId);
            }
            else
            {
                // new save                                                       
                startChild.ECNID = SaveTriggerPlan(startChild, noOpenWait, FindParentDirectEmail(noOpenWait), isHome);
                SaveNewMaControl(startChild, startChild.ECNID, startChild.Text, automationId);
            }

            UpdateGlobalControlListWithEcnid(startChild);
        }

        private void EndSaveChild(ControlBase startChild, int automationId, ref bool connectorExists)
        {
            if (startChild.MAControlID > 0)
            {
                connectorExists = true;
                SaveExistingMaControl(startChild, automationId);
            }
            else
            {
                SaveNewMaControl(startChild, -1, startChild.Text, automationId);
            }
        }

        private void GroupSaveChild(
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists)
        {
            var groupObject = startChild as Group;
            if (groupObject == null)
            {
                return;
            }
            if (!groupObject.editable.remove && groupObject.MAControlID > 0)
            {
                // Cant make edits to ECN object but can update diagram control
                SaveExistingMaControl(groupObject, automationId);
                connectorExists = true;
            }
            else
            {
                if (groupObject.MAControlID > 0)
                {
                    connectorExists = true;
                    var maControl = BusinessCommunicator.MAControl.GetByControlID(groupObject.ControlID, automationId);
                    if (groupObject.IsDirty)
                    {
                        forceIsDirty = true;
                        maControl.ECNID = groupObject.GroupID;
                        maControl.Text = groupObject.Text;
                    }
                    maControl.xPosition = groupObject.xPosition;
                    maControl.yPosition = groupObject.yPosition;
                    BusinessCommunicator.MAControl.Save(maControl);
                }
                else
                {
                    SaveNewMaControl(groupObject, groupObject.GroupID, groupObject.Text, automationId);
                }
                startChild.ECNID = groupObject.GroupID;
                UpdateGlobalControlListWithEcnid(groupObject);
            }
        }

        private void ClickSaveChild(
            ControlBase parent,
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists,
            bool keepPaused,
            bool isHome)
        {
            if (parent.ControlType != Enums.MarketingAutomationControlType.Wait)
            {
                var errorList = new List<ECNError>
                {
                    new ECNError(
                        CommonEnums.Entity.CampaignItem,
                        CommonEnums.Method.Validate,
                        GetValidateErrorMessage(startChild))
                };
                throw new ECNException(errorList);
            }
            if (!startChild.editable.remove && startChild.MAControlID > 0)
            {
                // Cant make edits to ECN object but can update diagram control
                SaveExistingMaControl(startChild, automationId);
                connectorExists = true;
            }
            else
            {
                if (startChild.ECNID > 0)
                {
                    connectorExists = true;
                    // an Update if the object is dirty
                    forceIsDirty = startChild.IsDirty;
                    if (startChild.IsDirty || isHome)
                    {
                        startChild.ECNID = SaveSmartSegmentEmail_Click(startChild, parent, keepPaused, isHome);
                    }
                    SaveExistingMaControl(startChild, automationId);
                }
                else
                {
                    // New Save                            
                    startChild.ECNID = SaveSmartSegmentEmail_Click(startChild, parent, keepPaused, isHome);
                    SaveNewMaControl(startChild, startChild.ECNID, startChild.Text, automationId);
                }

                UpdateGlobalControlListWithEcnid(startChild);
            }
        }

        private string GetValidateErrorMessage(ControlBase startChild)
        {
            var validateErrorMessage = string.Empty;
            switch (startChild.ControlType)
            {
                case Enums.MarketingAutomationControlType.Click:
                    validateErrorMessage = ErrorMsgClickEmail;
                    break;
                case Enums.MarketingAutomationControlType.NoClick:
                    validateErrorMessage = ErrorMsgNoClickEmail;
                    break;
                case Enums.MarketingAutomationControlType.NoOpen:
                    validateErrorMessage = ErrorMsgNoOpenEmail;
                    break;
                case Enums.MarketingAutomationControlType.NotSent:
                    validateErrorMessage = ErrorMsgNotSentEmail;
                    break;
                case Enums.MarketingAutomationControlType.Open:
                    validateErrorMessage = ErrorMsgOpenEmail;
                    break;
                case Enums.MarketingAutomationControlType.Open_NoClick:
                    validateErrorMessage = ErrorMsgOpenNoClickEmail;
                    break;
                case Enums.MarketingAutomationControlType.Sent:
                    validateErrorMessage = ErrorMsgSentEmail;
                    break;
                case Enums.MarketingAutomationControlType.Suppressed:
                    validateErrorMessage = ErrorMsgSuppressedEmail;
                    break;
            }

            return validateErrorMessage;
        }

        private void SubscribeSaveChild(
            ControlBase parent,
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists,
            bool isHome)
        {
            if (startChild.ECNID > 0)
            {
                connectorExists = true;
                // an Update if the object is dirty
                forceIsDirty = startChild.IsDirty;
                if (startChild.IsDirty || isHome)
                {
                    startChild.ECNID = SaveGroupTrigger(startChild, parent);
                }
                SaveExistingMaControl(startChild, automationId);
            }
            else
            {
                // New Save                            
                startChild.ECNID = SaveGroupTrigger(startChild, parent);
                SaveNewMaControl(startChild, startChild.ECNID, startChild.Text, automationId);
            }
            UpdateGlobalControlListWithEcnid(startChild);
        }

        private void WaitSaveChild(
            ControlBase startChild,
            int automationId,
            ref bool forceIsDirty,
            ref bool connectorExists)
        {
            if (!startChild.editable.remove && startChild.MAControlID > 0)
            {
                // Cant make edits to ECN object but can update diagram control
                SaveExistingMaControl(startChild, automationId);
                connectorExists = true;
                forceIsDirty = startChild.IsDirty;
            }
            else
            {
                if (startChild.MAControlID > 0)
                {
                    SaveExistingMaControl(startChild, automationId);
                    connectorExists = true;
                    forceIsDirty = startChild.IsDirty;
                }
                else
                {
                    SaveNewMaControl(startChild, -1, startChild.Text, automationId);
                }
            }
        }

        private void SaveConnector(
            bool connectorExists,
            ControlBase parent,
            ControlBase startChild,
            int automationId)
        {
            if (!connectorExists)
            {
                // Save Connector
                var control = AllConnectors.First(connector => connector.from.shapeId == parent.ControlID &&
                                                               connector.to.shapeId == startChild.ControlID);
                var maConnector = new EntityCommunicator.MAConnector
                {
                    From = parent.ControlID,
                    To = startChild.ControlID,
                    MarketingAutomationID = automationId,
                    ControlID = control.id
                };
                BusinessCommunicator.MAConnector.Save(maConnector);
            }
        }

        private void SaveNewMaControl(ControlBase startChild, int ecnId, string controlText, int automationId)
        {
            var maControl = new EntityCommunicator.MAControl
            {
                ControlID = startChild.ControlID,
                ControlType = startChild.ControlType,
                ECNID = ecnId,
                ExtraText = string.Empty,
                MarketingAutomationID = automationId,
                Text = controlText,
                xPosition = startChild.xPosition,
                yPosition = startChild.yPosition
            };
            BusinessCommunicator.MAControl.Save(maControl);
        }

        private void SaveExistingMaControl(ControlBase startChild, int automationId)
        {
            var maControl = BusinessCommunicator.MAControl.GetByControlID(startChild.ControlID, automationId);
            maControl.Text = startChild.Text;
            maControl.xPosition = startChild.xPosition;
            maControl.yPosition = startChild.yPosition;
            BusinessCommunicator.MAControl.Save(maControl);
        }

        private void UpdateGlobalControlListWithEcnid(ControlBase toUpdate)
        {
            AllControls.RemoveAll(control => control.ControlID == toUpdate.ControlID);
            AllControls.Add(toUpdate);
        }

        private List<ControlBase> GetChildrenFromGlobalList(ControlBase parent)
        {
            var connectors = AllConnectors.Where(connector => connector.from.shapeId == parent.ControlID).ToList();

            return AllControls.Where(control => connectors.Any(connector => 
                connector.to.shapeId == control.ControlID)).ToList();
        }

        private ControlBase FindFormParent(ControlBase child)
        {
            var connector = AllConnectors.First(connectors => connectors.to.shapeId == child.ControlID);
            if (connector != null)
            {
                var formControl = AllControls.OfType<Form>().First(control => 
                    connector.from.shapeId == control.ControlID);
                if (formControl != null)
                {
                    var formConnector = AllConnectors.First(connectors => 
                        connectors.to.shapeId == formControl.ControlID);

                    return AllControls.First(control => formConnector.from.shapeId == control.ControlID);
                }

                return null;
            }

            return null;
        }
    }
}