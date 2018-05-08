using System;
using System.Collections.Generic;
using System.ComponentModel;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Newtonsoft.Json;
using CommunicatorBusinessLayer = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public class CampaignControlBase : VisualControlBase, ICampaignControl
    {
        private const int DefaultCampaingId = -1;

        protected int _MessageID;
        protected string _MessageName;
        protected string _FromEmail;
        protected string _ReplyTo;
        protected string _FromName;
        protected string _EmailSubject;
        protected int? _HeatMapStats;
        protected string _CampaignItemName;
        protected int _CampaignItemTemplateID;
        protected string _CampaignItemTemplateName;
        protected bool _UseCampaignItemTemplate;
        protected string _BlastField1;
        protected string _BlastField2;
        protected string _BlastField3;
        protected string _BlastField4;
        protected string _BlastField5;
        protected int _CustomerID;

        [JsonProperty(PropertyName = ControlConsts.CustomerIdProperty)]
        public virtual int CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                _CustomerID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.CampaignItemTemplateIdProperty)]
        [DefaultValue(-1)]
        public int CampaignItemTemplateID
        {
            get
            {
                return _CampaignItemTemplateID;
            }
            set
            {
                _CampaignItemTemplateID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.CampaignItemTemplateNameProperty)]
        public string CampaignItemTemplateName
        {
            get
            {
                return _CampaignItemTemplateName;
            }
            set
            {
                _CampaignItemTemplateName = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.UseCampaignItemTemplateProperty)]
        public bool UseCampaignItemTemplate
        {
            get
            {
                return _UseCampaignItemTemplate;
            }
            set
            {
                _UseCampaignItemTemplate = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.BlastField1Property, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string BlastField1
        {
            get
            {
                return _BlastField1;
            }
            set
            {
                _BlastField1 = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.BlastField2Property, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string BlastField2
        {
            get
            {
                return _BlastField2;
            }
            set
            {
                _BlastField2 = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.BlastField3Property, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string BlastField3
        {
            get
            {
                return _BlastField3;
            }
            set
            {
                _BlastField3 = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.BlastField4Property, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string BlastField4
        {
            get
            {
                return _BlastField4;
            }
            set
            {
                _BlastField4 = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.BlastField5Property, DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string BlastField5
        {
            get
            {
                return _BlastField5;
            }
            set
            {
                _BlastField5 = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.MessageIdProperty)]
        public int MessageID
        {
            get
            {
                return _MessageID;
            }
            set
            {
                _MessageID = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.MessageProperty)]
        public string MessageName
        {
            get
            {
                return _MessageName;
            }
            set
            {
                _MessageName = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.FromEmailProperty)]
        public string FromEmail
        {
            get
            {
                return _FromEmail;
            }
            set
            {
                _FromEmail = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.ReplyToProperty)]
        public string ReplyTo
        {
            get
            {
                return _ReplyTo;
            }
            set
            {
                _ReplyTo = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.FromNameProperty)]
        public string FromName
        {
            get
            {
                return _FromName;
            }
            set
            {
                _FromName = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.SubjectProperty)]
        public string EmailSubject
        {
            get
            {
                return _EmailSubject;
            }
            set
            {
                _EmailSubject = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.CampaignItemNameProperty)]
        public string CampaignItemName
        {
            get
            {
                return _CampaignItemName;
            }
            set
            {
                _CampaignItemName = value;
            }
        }

        [JsonProperty(PropertyName = ControlConsts.HeatMapStatsProperty)]
        public int? HeatMapStats
        {
            get
            {
                return _HeatMapStats;
            }
            set
            {
                _HeatMapStats = value;
            }
        }

        protected void ValidateFromName(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(FromName))
            {
                var errorMessage = string.Format(ControlConsts.FromNameEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        protected void ValidateFromEmail(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(FromEmail))
            {
                var errorMessage = string.Format(ControlConsts.FromEmailEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else if (!EmailManager.IsValidEmailAddress(FromEmail))
            {
                var errorMessage = string.Format(ControlConsts.FromEmailInvalidErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        protected void ValidateEmailSubject(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(EmailSubject))
            {
                var errorMessage = string.Format(ControlConsts.EmailSubjectEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else if (EmailSubject.Length > ControlConsts.MaxLength)
            {
                var errorMessage = string.Format(ControlConsts.EmailSubjectTooLongErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }
    
        protected void ValidateReplyTo(ECNException ecnMaster)
        {
            if (string.IsNullOrWhiteSpace(ReplyTo))
            {
                var errorMessage = string.Format(ControlConsts.ReplyToEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else if (!EmailManager.IsValidEmailAddress(ReplyTo))
            {
                var errorMessage = string.Format(ControlConsts.ReplyToInvalidErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        protected void ValidateParent(
            ECNException ecnMaster,
            bool hasCampaignItem, 
            CampaignItem parentCampaignItem,
            Group parentGroup,
            User currentUser)
        {
            if (hasCampaignItem)
            {
                ValidateParentCampaignItem(ecnMaster, parentCampaignItem, currentUser);
            }
            else
            {
                ValidateParentGroup(ecnMaster, parentGroup, currentUser);
            }
        }

        protected void ValidateParentCampaignItem(ECNException ecnMaster, CampaignItem parentCampaignItem, User currentUser)
        {
            if (parentCampaignItem.CreateCampaignItem)
            {
                if (parentCampaignItem.SelectedGroups != null)
                {
                    ValidateSelectedGroups(ecnMaster, parentCampaignItem.SelectedGroups, currentUser);
                }
            }
            else
            {
                var campaignItem = CampaignItemManager.GetByCampaignItemID(
                    parentCampaignItem.CampaignItemID,
                    currentUser,
                    true);

                if (campaignItem != null && campaignItem.BlastList != null)
                {
                    ValidateBlastList(ecnMaster, currentUser, campaignItem.BlastList);
                }
                else
                {
                    var errorMessage = string.Format(ControlConsts.ParentNotSetErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
        }

        protected void ValidateParentGroup(
            ECNException ecnMaster,
            Group parentGroup,
            User currentUser,
            bool checkParentGroupExists = true)
        {
            if (checkParentGroupExists && !GroupManager.Exists(parentGroup.GroupID, parentGroup.CustomerID))
            {
                var errorMessage = string.Format(ControlConsts.GroupDoesNotExistForErrorMessage, parentGroup.GroupName, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }

            try
            {
                if (MessageID > 0)
                {
                    GroupManager.ValidateDynamicTags(parentGroup.GroupID, MessageID, currentUser);
                }
            }
            catch (ECNException eCampaignItem)
            {
                foreach (var er in eCampaignItem.ErrorList)
                {
                    ecnMaster.ErrorList.Add(er);
                }
            }
        }
        
        protected void ValidateBlastList(ECNException ecnMaster, User currentUser, List<ECN_Framework_Entities.Communicator.CampaignItemBlast> blastList)
        {
            foreach (var campaignblast in blastList)
            {
                if (!CommunicatorBusinessLayer.Group.Exists(
                    campaignblast.Blast.GroupID.Value,
                    campaignblast.CustomerID.Value))
                {
                    var errorMessage = string.Format(
                        ControlConsts.GroupDoesNotExistForErrorMessage,
                        campaignblast.Blast.GroupID.Value,
                        Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }

                try
                {
                    if (MessageID > 0)
                    {
                        CommunicatorBusinessLayer.Group.ValidateDynamicTags(
                            campaignblast.Blast.GroupID.Value,
                            MessageID,
                            currentUser);
                    }
                }
                catch (ECNException eCampaignItem)
                {
                    foreach (var error in eCampaignItem.ErrorList)
                    {
                        ecnMaster.ErrorList.Add(error);
                    }
                }
            }
        }

        protected void ValidateSelectedGroups(
            ECNException masterException,
            List<ECN_Objects.GroupSelect> selectedGroups,
            User currentUser)
        {
            foreach (var group in selectedGroups)
            {
                if (!CommunicatorBusinessLayer.Group.Exists(group.GroupID, group.CustomerID))
                {
                    var errorMessage = string.Format(
                        ControlConsts.GroupDoesNotExistForErrorMessage,
                        group.GroupName,
                        Text);
                    masterException.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
                try
                {
                    if (MessageID > 0)
                    {
                        CommunicatorBusinessLayer.Group.ValidateDynamicTags(group.GroupID, MessageID, currentUser);
                    }
                }
                catch (ECNException eCampaignItem)
                {
                    foreach (var error in eCampaignItem.ErrorList)
                    {
                        masterException.ErrorList.Add(error);
                    }
                }
            }
        }

        protected void ValidateActualLink(ECNException ecnMaster, int parentLayoutId, string actualLink)
        {
            if (string.IsNullOrWhiteSpace(actualLink))
            {
                var errorMessage = string.Format(ControlConsts.LinkEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else
            {
                if (!LinkAliasManager.Exists(parentLayoutId, actualLink))
                {
                    var errorMessage = string.Format(ControlConsts.LinkDoesNotExistErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
        }

        protected void ValidateMessageId(ECNException ecnMaster, int customerId)
        {
            if (MessageID > 0)
            {
                if (!LayoutManager.Exists(MessageID, customerId))
                {
                    var errorMessage = string.Format(ControlConsts.MessageDoesNotExistErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
                else
                {
                    if (LayoutManager.IsArchived(MessageID, customerId))
                    {
                        var errorMessage = string.Format(ControlConsts.MessageArchivedErrorMessage, Text);
                        ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                    }
                }

                if (!LayoutManager.IsValidated(MessageID, customerId))
                {
                    var errorMessage = string.Format(ControlConsts.MessageNotValidErrorMessage, Text);
                    ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                }
            }
            else
            {
                var errorMessage = string.Format(ControlConsts.MessageEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }

        protected void ValidateCampaignItem(
            ECNException ecnMaster,
            bool validateIfAlreadyExists = false,
            CampaignItem parentCampaignItem = null,
            Group parentGroup = null)
        {
            if (string.IsNullOrWhiteSpace(CampaignItemName))
            {
                var errorMessage = string.Format(ControlConsts.CampaignItemNameEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
            else if(validateIfAlreadyExists && ECNID <= 0)
            {
                if (parentCampaignItem != null && parentCampaignItem.CampaignID > 0)
                {
                    if (CampaignItemManager.Exists(CampaignItemName, parentCampaignItem.CampaignID, parentCampaignItem.CustomerID))
                    {
                        var errorMessage = string.Format(ControlConsts.CampaignItemAlreadyExistsErrorMessage, Text);
                        ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                    }
                }
                else if (parentGroup != null)
                {
                    if (CampaignManager.Exists(DefaultCampaingId, CampaignItemName, parentGroup.CustomerID))
                    {
                        var errorMessage = string.Format(ControlConsts.CampaignNameAlreadyExistsErrorMessage, Text);
                        ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
                    }
                }
            }

            ValidateCampaignItemTemplate(ecnMaster);
        }

        protected void ValidateCampaignItemTemplate(ECNException ecnMaster)
        {
            if (CampaignItemTemplateID <= 0 && UseCampaignItemTemplate)
            {
                var errorMessage = string.Format(ControlConsts.CampaignTemplateEmptyErrorMessage, Text);
                ecnMaster.ErrorList.Add(GetCampaignItemError(errorMessage));
            }
        }
        
        protected void ValidateSendTime(
            ECNException ecnMaster,
            CampaignItem parentCampaignItem,
            Wait parentWait,
            CommunicatorEntities.MarketingAutomation marketingAutomation)
        {
            ValidateSendTime(
                ecnMaster,
                parentCampaignItem,
                parentWait,
                marketingAutomation,
                ControlConsts.SendTimeOutsideOfAllowedDateRangeErrorMessage,
                ControlConsts.SendTimeInThePastErrorMessage);
        }

        protected void ValidateSendTime(
            ECNException ecnMaster,
            CampaignItem parentCampaignItem,
            Wait parentWait,
            CommunicatorEntities.MarketingAutomation marketingAutomation,
            string outsideRangeErrorMessage,
            string inThePastErrorMessage)
        {
            var sendTime = parentCampaignItem.SendTime.Value.AddDays(Convert.ToDouble(parentWait.WaitTime));
            var allowedDateRangeEnd = marketingAutomation.EndDate.Value.Date;
            var allowedDateRangeStart = marketingAutomation.StartDate.Value.Date;

            if (sendTime.Date > allowedDateRangeEnd ||
                sendTime.Date < allowedDateRangeStart)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(outsideRangeErrorMessage));
            }
            else if (ECNID <= 0 && sendTime < DateTime.Now)
            {
                ecnMaster.ErrorList.Add(GetCampaignItemError(inThePastErrorMessage));
            }
        }

        protected void ValidateEmail(ECNException masterException)
        {
            ValidateFromEmail(masterException);
            ValidateFromName(masterException);
            ValidateEmailSubject(masterException);
            ValidateReplyTo(masterException);
        }

        protected int GetCustomerId(CampaignItem parentCampaignItem, Group parentGroup)
        {
            if (parentCampaignItem != null)
            {
                return parentCampaignItem.CustomerID;
            }

            if (parentGroup != null)
            {
                return parentGroup.CustomerID;
            }

            return -1;
        }
    }
}