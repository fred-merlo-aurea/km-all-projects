using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ECN_Framework_Entities.Communicator;
using CampaignItem = ecn.MarketingAutomation.Models.PostModels.Controls.CampaignItem;
using CommunicatorBusinessLayer = ECN_Framework_BusinessLayer.Communicator;
using Group = ecn.MarketingAutomation.Models.PostModels.Controls.Group;

namespace ecn.MarketingAutomation.Models.PostModels
{
    public class ControlFactory : IControlFactory
    {
        private const string CampaingItemCopiedName = "Copy of {0}";

        public ControlBase PrepareForCopy(ControlBase control, int marketingAutomationId)
        {
            switch (control.ControlType)
            {
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem:
                    return GetCampaignItemForCopy((CampaignItem) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Click:
                    return GetEstimatedTimeCampaignControlBaseForCopy((Click) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Click:
                    return GetCampaignControlBaseForCopy((Direct_Click) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Form:
                    return GetFormForCopy((Form) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormAbandon:
                    return GetCampaignControlBaseForCopy((FormAbandon) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.FormSubmit:
                    return GetCampaignControlBaseForCopy((FormSubmit) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_Open:
                    return GetCampaignControlBaseForCopy((Direct_Open) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Direct_NoOpen:
                    return GetCampaignControlBaseForCopy((Direct_NoOpen) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.End:
                    return GetEndForCopy((End) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Group:
                    return GetGroupForCopy((Group) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoClick:
                    return GetEstimatedTimeCampaignControlBaseForCopy((NoClick) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NoOpen:
                    return GetEstimatedTimeCampaignControlBaseForCopy((NoOpen) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.NotSent:
                    return GetEstimatedTimeCampaignControlBaseForCopy((NotSent) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open:
                    return GetEstimatedTimeCampaignControlBaseForCopy((Open) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Open_NoClick:
                    return GetEstimatedTimeCampaignControlBaseForCopy((Open_NoClick) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Sent:
                    return GetEstimatedTimeCampaignControlBaseForCopy((Sent) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Start:
                    return GetStartForCopy((Start) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Subscribe:
                    return GetCampaignControlBaseForCopy((Subscribe) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Suppressed:
                    return GetEstimatedTimeCampaignControlBaseForCopy((Suppressed) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Unsubscribe:
                    return GetCampaignControlBaseForCopy((Unsubscribe) control, marketingAutomationId);
                case ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.Wait:
                    return GetWaitForCopy((Wait) control, marketingAutomationId);
            }

            return null;
        }

        private CampaignItem GetCampaignItemForCopy(CampaignItem campaignItemControl, int marketingAutomationId)
        {
            if (!campaignItemControl.CreateCampaignItem)
            {
                campaignItemControl.CreateCampaignItem = true;
                var campaignItemOriginal = CommunicatorBusinessLayer.CampaignItem.GetByCampaignItemID_NoAccessCheck(
                    campaignItemControl.ECNID,
                    true);

                if (campaignItemOriginal != null)
                {
                    var messageId = campaignItemOriginal.BlastList[0].LayoutID.Value;
                    var layout = CommunicatorBusinessLayer.Layout.GetByLayoutID_NoAccessCheck(
                        messageId,
                        false);
                    var campaign = CommunicatorBusinessLayer.Campaign.GetByCampaignID_NoAccessCheck(
                        campaignItemOriginal.CampaignID.Value,
                        false);

                    MapCampaignItemControlProperties(
                        campaignItemControl,
                        campaignItemOriginal,
                        messageId,
                        layout,
                        campaign);

                    MapSelectedGroupsAndFilters(campaignItemControl, campaignItemOriginal);
                    MapSuppressedGroupsAndFilters(campaignItemControl, campaignItemOriginal);
                }
            }

            SetBasicPropertiesOnCopy(campaignItemControl, marketingAutomationId, campaignItemControl.CampaignItemName);

            campaignItemControl.editable.remove = true;
            campaignItemControl.CampaignItemID = -1;
            campaignItemControl.SendTime = DateTime.MinValue;
            campaignItemControl.IsConfigured = false;

            return campaignItemControl;
        }

        private void MapSuppressedGroupsAndFilters(
            CampaignItem campaignItemControl,
            ECN_Framework_Entities.Communicator.CampaignItem campaignItemOriginal)
        {
            campaignItemControl.SuppressedGroups = new List<GroupSelect>();
            campaignItemControl.SuppressedGroupFilters = new List<FilterSelect>();

            foreach (var campaignItemSuppression in campaignItemOriginal.SuppressionList)
            {
                var suppressionGroup = CommunicatorBusinessLayer.Group.GetByGroupID_NoAccessCheck(
                    campaignItemSuppression.GroupID.Value);

                campaignItemControl.SuppressedGroups.Add(new GroupSelect
                {
                    CustomerID = suppressionGroup.CustomerID,
                    FolderID = suppressionGroup.FolderID.HasValue ? suppressionGroup.FolderID.Value : 0,
                    GroupDescription = suppressionGroup.GroupDescription,
                    GroupID = suppressionGroup.GroupID,
                    GroupName = suppressionGroup.GroupName
                });

                foreach (var blastFilter in campaignItemSuppression.Filters)
                {
                    if (blastFilter.FilterID.HasValue)
                    {
                        var filter = CommunicatorBusinessLayer.Filter.GetByFilterID_NoAccessCheck(blastFilter.FilterID.Value);
                        campaignItemControl.SuppressedGroupFilters.Add(new FilterSelect
                        {
                            FilterID = filter.FilterID,
                            CustomerID = filter.CustomerID.Value,
                            FilterName = filter.FilterName,
                            GroupID = filter.GroupID.Value
                        });
                    }
                }
            }
        }

        private void MapSelectedGroupsAndFilters(
            CampaignItem campaignItemControl,
            ECN_Framework_Entities.Communicator.CampaignItem campaignItemOriginal)
        {
            campaignItemControl.SelectedGroups = new List<GroupSelect>();
            campaignItemControl.SelectedGroupFilters = new List<FilterSelect>();

            foreach (var campaignItemBlast in campaignItemOriginal.BlastList)
            {
                var campaignItemBlastGroup = CommunicatorBusinessLayer.Group.GetByGroupID_NoAccessCheck(
                    campaignItemBlast.GroupID.Value);

                campaignItemControl.SelectedGroups.Add(new GroupSelect
                {
                    CustomerID = campaignItemBlastGroup.CustomerID,
                    FolderID = campaignItemBlastGroup.FolderID.HasValue
                        ? campaignItemBlastGroup.FolderID.Value
                        : 0,
                    GroupDescription = campaignItemBlastGroup.GroupDescription,
                    GroupID = campaignItemBlastGroup.GroupID,
                    GroupName = campaignItemBlastGroup.GroupName
                });

                foreach (var campaignItemBlastFilter in campaignItemBlast.Filters)
                {
                    if (campaignItemBlastFilter.FilterID.HasValue)
                    {
                        var filter = CommunicatorBusinessLayer.Filter.GetByFilterID_NoAccessCheck(
                            campaignItemBlastFilter.FilterID.Value);

                        campaignItemControl.SelectedGroupFilters.Add(new FilterSelect
                        {
                            FilterID = filter.FilterID,
                            CustomerID = filter.CustomerID.Value,
                            FilterName = filter.FilterName,
                            GroupID = filter.GroupID.Value
                        });
                    }
                }
            }
        }

        private void MapCampaignItemControlProperties(
            CampaignItem campaignItemControl,
            ECN_Framework_Entities.Communicator.CampaignItem campaignItemOriginal,
            int messageId,
            Layout layout,
            Campaign campaign)
        {
            var blast = campaignItemOriginal.BlastList[0].Blast;

            campaignItemControl.EmailSubject = blast.EmailSubject;
            campaignItemControl.FromEmail = blast.EmailFrom;
            campaignItemControl.FromName = blast.EmailFromName;
            campaignItemControl.ReplyTo = blast.ReplyTo;

            campaignItemControl.MessageID = messageId;
            campaignItemControl.MessageName = layout.LayoutName;
            campaignItemControl.CampaignID = campaign.CampaignID;
            campaignItemControl.CampaignName = campaign.CampaignName;
            campaignItemControl.CreateCampaign = false;
            campaignItemControl.CampaignItemTemplateID =
                campaignItemOriginal.CampaignItemTemplateID.HasValue
                    ? campaignItemOriginal.CampaignItemTemplateID.Value
                    : -1;
            campaignItemControl.BlastField1 = campaignItemOriginal.BlastField1;
            campaignItemControl.BlastField2 = campaignItemOriginal.BlastField2;
            campaignItemControl.BlastField3 = campaignItemOriginal.BlastField3;
            campaignItemControl.BlastField4 = campaignItemOriginal.BlastField4;
            campaignItemControl.BlastField5 = campaignItemOriginal.BlastField5;
            campaignItemControl.UseCampaignItemTemplate = campaignItemControl.CampaignItemTemplateID > 0;
        }

        private End GetEndForCopy(End end, int marketingAutomationId)
        {
            SetBasicPropertiesOnCopy(end, marketingAutomationId, 0);

            end.ExtraText = string.Empty;
            end.Text = string.Empty;
            end.IsConfigured = true;
            end.editable.remove = true;

            return end;
        }

        private Group GetGroupForCopy(Group groupForCory, int marketingAutomationId)
        {
            groupForCory.MarketingAutomationID = marketingAutomationId;
            groupForCory.MAControlID = 0;
            groupForCory.IsConfigured = false;
            groupForCory.editable.remove = true;
            return groupForCory;
        }

        private Start GetStartForCopy(Start start, int marketingAutomationId)
        {
            SetBasicPropertiesOnCopy(start, marketingAutomationId, 0);

            start.ExtraText = string.Empty;
            start.Text = String.Empty;
            start.IsConfigured = true;
            start.editable.remove = true;
            return start;
        }

        private Wait GetWaitForCopy(Wait wait, int marketingAutomationId)
        {
            SetBasicPropertiesOnCopy(wait, marketingAutomationId, 0);

            wait.IsConfigured = false;
            wait.editable.remove = true;
            return wait;
        }

        private Form GetFormForCopy(Form form, int marketingAutomationId)
        {
            SetBasicPropertiesOnCopy(form, marketingAutomationId);

            form.IsConfigured = false;
            form.editable.remove = true;

            return form;
        }

        private T GetCampaignControlBaseForCopy<T>(T control, int marketingAutomationId) 
            where T:CampaignControlBase
        {
            SetBasicPropertiesOnCopy(control, marketingAutomationId, control.CampaignItemName);

            control.IsConfigured = false;
            control.editable.remove = true;
            
            return control;
        }

        private T GetEstimatedTimeCampaignControlBaseForCopy<T>(T control, int marketingAutomationId)
            where T : CampaignControlBase, IEstimatedSendTime
        {
            var result = GetCampaignControlBaseForCopy(control, marketingAutomationId);
            result.EstSendTime = null;

            return result;
        }

        private void SetBasicPropertiesOnCopy(
            CampaignControlBase control,
            int marketingAutomationId,
            string campaingItemName)
        {
            SetBasicPropertiesOnCopy(control, marketingAutomationId);
            control.CampaignItemName = string.Format(CampaingItemCopiedName, campaingItemName);
        }

        private void SetBasicPropertiesOnCopy(
            VisualControlBase control,
            int marketingAutomationId,
            int ecnId = -1)
        {
            control.ECNID = ecnId;
            control.MarketingAutomationID = marketingAutomationId;
            control.MAControlID = 0;
        }
    }
}