using System;
using ecn.MarketingAutomation.Models.PostModels.Controls;

namespace ecn.MarketingAutomation.Models.PostModels
{
    public static class ControlDeserializer
    {
        private const string CategoryCampaign = "Campaign";
        private const string CategoryGroup = "Group";
        private const string CategoryNoClick = "Noclick";
        private const string CategoryNoOpen = "Noopen";
        private const string CategoryOpenNoClick = "Opennoclick";
        private const string CategorySent = "Sent";
        private const string CategoryNotSent = "Notsent";
        private const string CategorySuppressed = "Suppressed";
        private const string CategoryClick = "GEclick";
        private const string CategoryOpen = "GEopen";
        private const string CategoryDirectOpen = "DEopen";
        private const string CategorySubscribe = "Subscribe";
        private const string CategoryUnsubscribe = "Unsubscribe";
        private const string CategoryDirectClick = "DEclick";
        private const string CategoryForm = "form";
        private const string CategoryFormSubmit = "Formsubmit";
        private const string CategoryFormAbandon = "Formabandon";
        private const string CategoryWait = "Wait";
        private const string CategoryStart = "Start";
        private const string CategoryEnd = "End";

        public static ControlBase Deserialize(dynamic dynamicControl)
        {
            switch ((string)dynamicControl.category)
            {
                case CategoryCampaign:
                    return DeserializeCampaignItem(dynamicControl);
                case CategoryGroup:
                    return DeserializeGroup(dynamicControl);
                case CategoryNoClick:
                    return DeserializeNoClick(dynamicControl);
                case CategoryNoOpen:
                    return DeserializeNoOpen(dynamicControl);
                case CategoryOpenNoClick:
                    return DeserializeOpenNoClick(dynamicControl);
                case CategorySent:
                    return DeserializeSent(dynamicControl);
                case CategoryNotSent:
                    return DeserializeNotSent(dynamicControl);
                case CategorySuppressed:
                    return DeserializeSuppressed(dynamicControl);
                case CategoryClick:
                    return DeserializeClick(dynamicControl);
                case CategoryOpen:
                    return DeserializeOpen(dynamicControl);
                case CategoryDirectOpen:
                    return DeserializeDirectOpen(dynamicControl);
                case CategorySubscribe:
                    return DeserializeSubscribe(dynamicControl);
                case CategoryUnsubscribe:
                    return DeserializeUnsubscribe(dynamicControl);
                case CategoryDirectClick:
                    return DeserializeDirectClick(dynamicControl);
                case CategoryForm:
                    return DeserializeForm(dynamicControl);
                case CategoryFormSubmit:
                    return DeserializeFormSubmit(dynamicControl);
                case CategoryFormAbandon:
                    return DeserializeFormAbandon(dynamicControl);
                case CategoryWait:
                    return DeserializeWait(dynamicControl);
                case CategoryStart:
                    return DeserializeStart(dynamicControl);
                case CategoryEnd:
                    return DeserializeEnd(dynamicControl);
            }
            return null;
        }

        private static CampaignItem DeserializeCampaignItem(dynamic control)
        {
            var campaignItem = new CampaignItem
            {
                CampaignID = control.campaign_item_nameID,
                CampaignItemID = control.campaign_itemID,
                CampaignItemName = control.campaign_item,
                CampaignName = control.campaign_item_name,
                ControlID = control.id,
                CustomerID = control.customerID,
                CustomerName = control.customer,
                ECNID = control.campaign_itemID,
                EmailSubject = control.subject,
                ExtraText = string.Empty,
                FromEmail = control.from_email,
                FromName = control.from_name,
                IsDirty = control.isDirty,
                MAControlID = control.ma_controlID,
                MessageID = control.messageId,
                MessageName = control.message,
                ReplyTo = control.reply_to,
                SendTime = Convert.ToDateTime(control.schedule),
                Text = control.control_text,
                xPosition = control.x,
                yPosition = control.y
            };

            DeserializeSelectedGroups(control, campaignItem);
            DeserializeSelectedGroupFilters(control, campaignItem);
            DeserializeSuppressedGroups(control, campaignItem);
            DeserializeSuppressedGroupFilters(control, campaignItem);

            return campaignItem;
        }

        private static void DeserializeSuppressedGroupFilters(dynamic control, CampaignItem campaignItem)
        {
            foreach (var groupFilter in control.suppression_groups_filter)
            {
                var filterSelect = new ECN_Objects.FilterSelect
                {
                    CustomerID = campaignItem.CustomerID,
                    FilterID = groupFilter.FilterID,
                    FilterName = groupFilter.FilterName,
                    GroupID = groupFilter.GroupID
                };
                campaignItem.SuppressedGroupFilters.Add(filterSelect);
            }
        }

        private static void DeserializeSuppressedGroups(dynamic control, CampaignItem campaignItem)
        {
            foreach (var suppresionGroup in control.suppression_groups)
            {
                var groupSelect = new ECN_Objects.GroupSelect
                {
                    CustomerID = campaignItem.CustomerID,
                    FolderID = suppresionGroup.FolderID,
                    GroupID = suppresionGroup.GroupID,
                    GroupName = suppresionGroup.GroupName
                };
                campaignItem.SuppressedGroups.Add(groupSelect);
            }
        }

        private static void DeserializeSelectedGroupFilters(dynamic control, CampaignItem campaignItem)
        {
            foreach (var groupFilter in control.groups_filter)
            {
                var filterSelect = new ECN_Objects.FilterSelect
                {
                    CustomerID = campaignItem.CustomerID,
                    FilterID = groupFilter.FilterID,
                    FilterName = groupFilter.FilterName,
                    GroupID = groupFilter.GroupID
                };
                campaignItem.SelectedGroupFilters.Add(filterSelect);
            }
        }

        private static void DeserializeSelectedGroups(dynamic control, CampaignItem campaignItem)
        {
            foreach (var campaignGroup in control.groups)
            {
                var groupSelect = new ECN_Objects.GroupSelect
                {
                    CustomerID = campaignItem.CustomerID,
                    FolderID = campaignGroup.FolderID,
                    GroupID = campaignGroup.GroupID,
                    GroupName = campaignGroup.GroupName
                };
                campaignItem.SelectedGroups.Add(groupSelect);
            }
        }

        private static Group DeserializeGroup(dynamic dynamicGroup)
        {
            return new Group
            {
                ControlID = dynamicGroup.id,
                CustomerID = dynamicGroup.customerID,
                CustomerName = dynamicGroup.customer,
                ECNID = dynamicGroup.groupID,
                ExtraText = string.Empty,
                GroupID = dynamicGroup.groupID,
                GroupName = dynamicGroup.group,
                IsDirty = dynamicGroup.isDirty,
                MAControlID = dynamicGroup.ma_controlID,
                Text = dynamicGroup.control_text,
                xPosition = dynamicGroup.x,
                yPosition = dynamicGroup.y
            };
        }

        private static NoClick DeserializeNoClick(dynamic dynamicNoClick)
        {
            return new NoClick
            {
                ControlID = dynamicNoClick.id,
                ECNID = dynamicNoClick.campaign_itemID,
                EmailSubject = dynamicNoClick.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicNoClick.from_email,
                FromName = dynamicNoClick.from_name,
                IsDirty = dynamicNoClick.isDirty,
                MAControlID = dynamicNoClick.ma_controlID,
                MessageID = dynamicNoClick.messageId,
                MessageName = dynamicNoClick.message,
                ReplyTo = dynamicNoClick.reply_to,
                Text = dynamicNoClick.control_text,
                xPosition = dynamicNoClick.x,
                yPosition = dynamicNoClick.y,
                CampaignItemName = dynamicNoClick.campaign_item_name
            };
        }

        private static NoOpen DeserializeNoOpen(dynamic dynamicNoOpen)
        {
            return new NoOpen
            {
                ControlID = dynamicNoOpen.id,
                ECNID = dynamicNoOpen.campaign_itemID,
                EmailSubject = dynamicNoOpen.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicNoOpen.from_email,
                FromName = dynamicNoOpen.from_name,
                IsDirty = dynamicNoOpen.isDirty,
                MAControlID = dynamicNoOpen.ma_controlID,
                MessageID = dynamicNoOpen.messageId,
                MessageName = dynamicNoOpen.message,
                ReplyTo = dynamicNoOpen.reply_to,
                Text = dynamicNoOpen.control_text,
                xPosition = dynamicNoOpen.x,
                yPosition = dynamicNoOpen.y
            };
        }

        private static Open_NoClick DeserializeOpenNoClick(dynamic dynamicOpenNoClick)
        {
            return new Open_NoClick
            {
                ControlID = dynamicOpenNoClick.id,
                ECNID = dynamicOpenNoClick.campaign_itemID,
                EmailSubject = dynamicOpenNoClick.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicOpenNoClick.from_email,
                FromName = dynamicOpenNoClick.from_name,
                IsDirty = dynamicOpenNoClick.isDirty,
                MAControlID = dynamicOpenNoClick.ma_controlID,
                MessageID = dynamicOpenNoClick.messageId,
                MessageName = dynamicOpenNoClick.message,
                ReplyTo = dynamicOpenNoClick.reply_to,
                Text = dynamicOpenNoClick.control_text,
                xPosition = dynamicOpenNoClick.x,
                yPosition = dynamicOpenNoClick.y
            };
        }

        private static Sent DeserializeSent(dynamic dynamicSent)
        {
            return new Sent
            {
                ControlID = dynamicSent.id,
                ECNID = dynamicSent.campaign_itemID,
                EmailSubject = dynamicSent.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicSent.from_email,
                FromName = dynamicSent.from_name,
                IsDirty = dynamicSent.isDirty,
                MAControlID = dynamicSent.ma_controlID,
                MessageID = dynamicSent.messageId,
                MessageName = dynamicSent.message,
                ReplyTo = dynamicSent.reply_to,
                Text = dynamicSent.control_text,
                xPosition = dynamicSent.x,
                yPosition = dynamicSent.y
            };
        }

        private static NotSent DeserializeNotSent(dynamic dynamicNotSent)
        {
            return new NotSent
            {
                ControlID = dynamicNotSent.id,
                ECNID = dynamicNotSent.campaign_itemID,
                EmailSubject = dynamicNotSent.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicNotSent.from_email,
                FromName = dynamicNotSent.from_name,
                IsDirty = dynamicNotSent.isDirty,
                MAControlID = dynamicNotSent.ma_controlID,
                MessageID = dynamicNotSent.messageId,
                MessageName = dynamicNotSent.message,
                ReplyTo = dynamicNotSent.reply_to,
                Text = dynamicNotSent.control_text,
                xPosition = dynamicNotSent.x,
                yPosition = dynamicNotSent.y
            };
        }

        private static Suppressed DeserializeSuppressed(dynamic dynamicSuppressed)
        {
            return new Suppressed
            {
                ControlID = dynamicSuppressed.id,
                ECNID = dynamicSuppressed.campaign_itemID,
                EmailSubject = dynamicSuppressed.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicSuppressed.from_email,
                FromName = dynamicSuppressed.from_name,
                IsDirty = dynamicSuppressed.isDirty,
                MAControlID = dynamicSuppressed.ma_controlID,
                MessageID = dynamicSuppressed.messageId,
                MessageName = dynamicSuppressed.message,
                ReplyTo = dynamicSuppressed.reply_to,
                Text = dynamicSuppressed.control_text,
                xPosition = dynamicSuppressed.x,
                yPosition = dynamicSuppressed.y
            };
        }

        private static Click DeserializeClick(dynamic dynamicClick)
        {
            return new Click
            {
                ControlID = dynamicClick.id,
                ECNID = dynamicClick.campaign_itemID,
                EmailSubject = dynamicClick.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicClick.from_email,
                FromName = dynamicClick.from_name,
                IsDirty = dynamicClick.isDirty,
                MAControlID = dynamicClick.ma_controlID,
                MessageID = dynamicClick.messageId,
                MessageName = dynamicClick.message,
                ReplyTo = dynamicClick.reply_to,
                Text = dynamicClick.control_text,
                xPosition = dynamicClick.x,
                yPosition = dynamicClick.y
            };
        }

        private static Open DeserializeOpen(dynamic dynamicOpen)
        {
            return new Open
            {
                ControlID = dynamicOpen.id,
                ECNID = dynamicOpen.campaign_itemID,
                EmailSubject = dynamicOpen.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicOpen.from_email,
                FromName = dynamicOpen.from_name,
                IsDirty = dynamicOpen.isDirty,
                MAControlID = dynamicOpen.ma_controlID,
                MessageID = dynamicOpen.messageId,
                MessageName = dynamicOpen.message,
                ReplyTo = dynamicOpen.reply_to,
                Text = dynamicOpen.control_text,
                xPosition = dynamicOpen.x,
                yPosition = dynamicOpen.y
            };
        }

        private static Direct_Open DeserializeDirectOpen(dynamic dynamicDirectOpen)
        {
            return new Direct_Open
            {
                ControlID = dynamicDirectOpen.id,
                ECNID = dynamicDirectOpen.campaign_itemID,
                EmailSubject = dynamicDirectOpen.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicDirectOpen.from_email,
                FromName = dynamicDirectOpen.from_name,
                IsDirty = dynamicDirectOpen.isDirty,
                MAControlID = dynamicDirectOpen.ma_controlID,
                MessageID = dynamicDirectOpen.messageId,
                MessageName = dynamicDirectOpen.message,
                ReplyTo = dynamicDirectOpen.reply_to,
                Text = dynamicDirectOpen.control_text,
                xPosition = dynamicDirectOpen.x,
                yPosition = dynamicDirectOpen.y
            };
        }

        private static Subscribe DeserializeSubscribe(dynamic dynamicSubscribe)
        {
            return new Subscribe
            {
                ControlID = dynamicSubscribe.id,
                ECNID = dynamicSubscribe.campaign_itemID,
                EmailSubject = dynamicSubscribe.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicSubscribe.from_email,
                FromName = dynamicSubscribe.from_name,
                IsDirty = dynamicSubscribe.isDirty,
                MAControlID = dynamicSubscribe.ma_controlID,
                MessageID = dynamicSubscribe.messageId,
                MessageName = dynamicSubscribe.message,
                ReplyTo = dynamicSubscribe.reply_to,
                Text = dynamicSubscribe.control_text,
                xPosition = dynamicSubscribe.x,
                yPosition = dynamicSubscribe.y
            };
        }

        private static Unsubscribe DeserializeUnsubscribe(dynamic dynamicUnsubscribe)
        {
            return new Unsubscribe
            {
                ControlID = dynamicUnsubscribe.id,
                ECNID = dynamicUnsubscribe.campaign_itemID,
                EmailSubject = dynamicUnsubscribe.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicUnsubscribe.from_email,
                FromName = dynamicUnsubscribe.from_name,
                IsDirty = dynamicUnsubscribe.isDirty,
                MAControlID = dynamicUnsubscribe.ma_controlID,
                MessageID = dynamicUnsubscribe.messageId,
                MessageName = dynamicUnsubscribe.message,
                ReplyTo = dynamicUnsubscribe.reply_to,
                Text = dynamicUnsubscribe.control_text,
                xPosition = dynamicUnsubscribe.x,
                yPosition = dynamicUnsubscribe.y
            };
        }

        private static Direct_Click DeserializeDirectClick(dynamic dynamicDirectClick)
        {
            return new Direct_Click
            {
                ControlID = dynamicDirectClick.id,
                ECNID = dynamicDirectClick.campaign_itemID,
                EmailSubject = dynamicDirectClick.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicDirectClick.from_email,
                FromName = dynamicDirectClick.from_name,
                IsDirty = dynamicDirectClick.isDirty,
                MAControlID = dynamicDirectClick.ma_controlID,
                MessageID = dynamicDirectClick.messageId,
                MessageName = dynamicDirectClick.message,
                ReplyTo = dynamicDirectClick.reply_to,
                Text = dynamicDirectClick.control_text,
                xPosition = dynamicDirectClick.x,
                yPosition = dynamicDirectClick.y,
                AnyLink = dynamicDirectClick.linkRb,
                SpecificLink = dynamicDirectClick.link
            };
        }

        private static Form DeserializeForm(dynamic dynamicForm)
        {
            return new Form
            {
                ControlID = dynamicForm.id,
                ECNID = dynamicForm.formId,
                ExtraText = string.Empty,
                IsDirty = dynamicForm.isDirty,
                MAControlID = dynamicForm.ma_controlID,
                FormID = dynamicForm.formId,
                FormName = dynamicForm.formname,
                Text = dynamicForm.control_text,
                xPosition = dynamicForm.x,
                yPosition = dynamicForm.y,
                AnyLink = dynamicForm.linkRb,
                SpecificLink = dynamicForm.link
            };
        }

        private static FormSubmit DeserializeFormSubmit(dynamic dynamicFormSubmit)
        {
            return new FormSubmit
            {
                ControlID = dynamicFormSubmit.id,
                ECNID = dynamicFormSubmit.campaign_itemID,
                EmailSubject = dynamicFormSubmit.subject,
                ExtraText = "",
                FromEmail = dynamicFormSubmit.from_email,
                FromName = dynamicFormSubmit.from_name,
                IsDirty = dynamicFormSubmit.isDirty,
                MAControlID = dynamicFormSubmit.ma_controlID,
                MessageID = dynamicFormSubmit.messageId,
                MessageName = dynamicFormSubmit.message,
                ReplyTo = dynamicFormSubmit.reply_to,
                Text = dynamicFormSubmit.control_text,
                xPosition = dynamicFormSubmit.x,
                yPosition = dynamicFormSubmit.y,
                AnyLink = dynamicFormSubmit.linkRb,
                SpecificLink = dynamicFormSubmit.link
            };
        }

        private static FormAbandon DeserializeFormAbandon(dynamic dynamicFormAbandon)
        {
            return new FormAbandon
            {
                ControlID = dynamicFormAbandon.id,
                ECNID = dynamicFormAbandon.campaign_itemID,
                EmailSubject = dynamicFormAbandon.subject,
                ExtraText = string.Empty,
                FromEmail = dynamicFormAbandon.from_email,
                FromName = dynamicFormAbandon.from_name,
                IsDirty = dynamicFormAbandon.isDirty,
                MAControlID = dynamicFormAbandon.ma_controlID,
                MessageID = dynamicFormAbandon.messageId,
                MessageName = dynamicFormAbandon.message,
                ReplyTo = dynamicFormAbandon.reply_to,
                Text = dynamicFormAbandon.control_text,
                xPosition = dynamicFormAbandon.x,
                yPosition = dynamicFormAbandon.y,
                AnyLink = dynamicFormAbandon.linkRb,
                SpecificLink = dynamicFormAbandon.link
            };
        }

        private static Wait DeserializeWait(dynamic dynamicWait)
        {
            var timeSpan = new TimeSpan(dynamicWait.days, dynamicWait.hours, dynamicWait.minutes, 0);
            return new Wait
            {
                ControlID = dynamicWait.id,
                ECNID = -1,
                ExtraText = string.Empty,
                IsDirty = dynamicWait.isDirty,
                MAControlID = dynamicWait.ma_controlID,
                Text = dynamicWait.control_text,
                WaitTime = Convert.ToDecimal(timeSpan.TotalDays),
                xPosition = dynamicWait.x,
                yPosition = dynamicWait.y
            };
        }

        private static Start DeserializeStart(dynamic dynamicStart)
        {
            return new Start
            {
                ControlID = dynamicStart.id,
                ECNID = -1,
                ExtraText = string.Empty,
                IsDirty = dynamicStart.isDirty,
                MAControlID = dynamicStart.ma_controlID,
                Text = ControlConsts.ControlTextStart,
                xPosition = dynamicStart.x,
                yPosition = dynamicStart.y
            };
        }

        private static End DeserializeEnd(dynamic dynamicEnd)
        {
            return new End
            {
                ControlID = dynamicEnd.id,
                ECNID = -1,
                ExtraText = String.Empty,
                IsDirty = dynamicEnd.isDirty,
                MAControlID = dynamicEnd.ma_controlID,
                Text = ControlConsts.ControlTextEnd,
                xPosition = dynamicEnd.x,
                yPosition = dynamicEnd.y
            };
        }
    }
}