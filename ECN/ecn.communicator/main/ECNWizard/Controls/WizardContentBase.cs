using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Application;
using KMPlatform.Entity;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using BusinessBlast = ECN_Framework_BusinessLayer.Communicator.Blast;
using BusinessContent = ECN_Framework_BusinessLayer.Communicator.Content;
using BusinessBlastFieldsName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName;
using BusinessSocialMedia = ECN_Framework_BusinessLayer.Communicator.SocialMedia;
using BusinessCampaignItemSuppression = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression;
using SocialMediaAuth = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public abstract class WizardContentBase : UserControl
    {
        private const int FacebookSocialMedia = 1;
        private const int TwitterSocialMedia = 2;
        private const int LinkedInSocialMedia = 3;
        private const int FacebookLikeSocialMedia = 4;

        private const string FirstNameToken = "first-name";
        private const string LastNameToken = "last-name";
        private const string FirstNameTokenFacebook = "first_name";
        private const string LastNameTokenFacebook = "last_name";
        private const string FacebookNetworkName = "Facebook";
        private const string Whitespace = " ";
        private const string TwitterNameToken = "name";
        private const string TwitterNetworkName = "Twitter";
        private const string LinkedInNetworkName = "LinkedIn";
        private const string ProfileNetworkLabelName = "lblSocialMediaName";
        private const string UnexpectedResultSetError = "Unexpected result set received";
        private const string NetworkNameLabelName = "lblSocialMedia";

        /// <summary>
        /// pnlBlastFields control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Panel pnlBlastFields;

        /// <summary>
        /// lblBlastFieldHeader control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastFieldHeader;

        /// <summary>
        /// lblBlastField1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastField1;

        /// <summary>
        /// lblBlastFieldValue1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastFieldValue1;

        /// <summary>
        /// lblBlastField2 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastField2;

        /// <summary>
        /// lblBlastFieldValue2 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastFieldValue2;

        /// <summary>
        /// lblBlastField3 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastField3;

        /// <summary>
        /// lblBlastFieldValue3 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastFieldValue3;

        /// <summary>
        /// lblBlastField4 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastField4;

        /// <summary>
        /// lblBlastFieldValue4 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastFieldValue4;

        /// <summary>
        /// lblBlastField5 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastField5;

        /// <summary>
        /// lblBlastFieldValue5 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Label lblBlastFieldValue5;

        protected virtual void gvSimpleShare_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            var cism = e.Row.DataItem as CampaignItemSocialMedia;
            if (cism?.SimpleShareDetailID == null)
            {
                return;
            }

            var profileName = string.Empty;
            var networkName = string.Empty;
            var sma = SocialMediaAuth.GetBySocialMediaAuthID(cism.SocialMediaAuthID.GetValueOrDefault());

            if (cism.SocialMediaID == FacebookSocialMedia)
            {
                var fbProfile = SocialMediaHelper.GetFBUserProfile(sma.Access_Token);
                networkName = FacebookNetworkName;
                profileName = (fbProfile[FirstNameTokenFacebook] ?? string.Empty) +
                              (fbProfile[LastNameTokenFacebook] != null
                                  ? Whitespace + fbProfile[LastNameTokenFacebook]
                                  : string.Empty);
            }
            else if (cism.SocialMediaID == TwitterSocialMedia)
            {
                var dirtyProfile = new OAuthHelper().GetTwitterProfile(sma.UserID, sma.Access_Token, sma.Access_Secret);
                var twUserProfile = SocialMediaHelper.GetJSONDict(dirtyProfile);
                networkName = TwitterNetworkName;
                profileName = (twUserProfile[TwitterNameToken] ?? string.Empty);
            }
            else if (cism.SocialMediaID == LinkedInSocialMedia)
            {
                var liUserProfile = SocialMediaHelper.GetLIUserProfile(sma.Access_Token);
                networkName = LinkedInNetworkName;

                profileName = (liUserProfile[FirstNameToken] ?? string.Empty) +
                              (liUserProfile[LastNameToken] != null
                                  ? Whitespace + liUserProfile[LastNameToken]
                                  : string.Empty);
            }

            var lblSocial = e.Row.FindControl(NetworkNameLabelName) as Label;
            var lblSocialProfile = e.Row.FindControl(ProfileNetworkLabelName) as Label;

            if (lblSocial != null)
            {
                lblSocial.Text = networkName;
            }

            if (lblSocialProfile != null)
            {
                lblSocialProfile.Text = profileName;
            }
        }

        protected virtual void gvSubscriberShare_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            var cism = (CampaignItemSocialMedia)e.Row.DataItem;
            var socialMedia = BusinessSocialMedia.GetSocialMediaByID(cism.SocialMediaID);

            var networkLabel = e.Row.FindControl(NetworkNameLabelName) as Label;
            if (networkLabel != null)
            {
                networkLabel.Text = socialMedia.DisplayName;
            }

            if (!cism.SocialMediaAuthID.HasValue || cism.SocialMediaID != FacebookLikeSocialMedia)
            {
                return;
            }

            var socialMediaAuth = SocialMediaAuth.GetBySocialMediaAuthID(cism.SocialMediaAuthID.Value);
            var profileLabel = e.Row.FindControl(ProfileNetworkLabelName) as Label;
            if (profileLabel != null)
            {
                profileLabel.Text = socialMediaAuth.ProfileName;
            }
        }

        protected string SaveNoBlastSuppressions(CampaignItem campaignItem)
        {
            if (campaignItem == null)
            {
                throw new ArgumentNullException(nameof(campaignItem));
            }

            var xmlGroups = new StringBuilder();
            xmlGroups.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xmlGroups.Append("<NoBlast>");
            foreach (var blast in campaignItem.BlastList)
            {
                xmlGroups.Append($"<Group id=\"{blast.GroupID}\">");
                foreach (var filter in blast.Filters.Where(x => x.SmartSegmentID != null))
                {
                    xmlGroups.Append($"<SmartSegmentID id=\"{filter.SmartSegmentID}\">");
                    xmlGroups.Append($"<RefBlastIDs>{filter.RefBlastIDs}</RefBlastIDs></SmartSegmentID>");
                }

                foreach (var filter in blast.Filters.Where(x => x.FilterID != null))
                {
                    xmlGroups.Append($"<FilterID id=\"{filter.FilterID}\" />");
                }

                xmlGroups.Append("</Group>");
            }

            xmlGroups.Append("<SuppressionGroup>");

            var suppressions = BusinessCampaignItemSuppression.GetByCampaignItemID(
                campaignItem.CampaignItemID,
                ECNSession.CurrentSession().CurrentUser,
                true);
            foreach (var campaignItemSuppression in suppressions)
            {
                xmlGroups.Append($"<Group id=\"{campaignItemSuppression.GroupID}\">");
                foreach (var filter in campaignItemSuppression.Filters.Where(x => x.SmartSegmentID != null))
                {
                    xmlGroups.Append($"<SmartSegmentID id=\"{filter.SmartSegmentID}\">");
                    xmlGroups.Append($"<RefBlastIDs>{filter.RefBlastIDs}</RefBlastIDs></SmartSegmentID>");
                }

                foreach (var filter in campaignItemSuppression.Filters.Where(x => x.FilterID != null))
                {
                    xmlGroups.Append($"<FilterID id=\"{filter.FilterID}\" />");
                }

                xmlGroups.Append("</Group>");
            }

            xmlGroups.Append("</SuppressionGroup>");
            xmlGroups.Append("</NoBlast>");

            var dtSends = BusinessBlast.GetEstimatedSendsCount(
                xmlGroups.ToString(),
                ECNSession.CurrentSession().CurrentCustomer.CustomerID,
                campaignItem.IgnoreSuppression ?? false);

            if(dtSends.Rows.Count == 0 || dtSends.Columns.Count ==0 )
            {
                throw new InvalidOperationException(UnexpectedResultSetError);
            }

            return dtSends.Rows[0][0].ToString();
        }

        protected void InitializeBlastFields(CampaignItem campaignItem)
        {
            pnlBlastFields.Visible = false;
            InitializeBlastField(campaignItem.BlastField1, 1, lblBlastField1, lblBlastFieldValue1);
            InitializeBlastField(campaignItem.BlastField2, 2, lblBlastField2, lblBlastFieldValue2);
            InitializeBlastField(campaignItem.BlastField3, 3, lblBlastField3, lblBlastFieldValue3);
            InitializeBlastField(campaignItem.BlastField4, 4, lblBlastField4, lblBlastFieldValue4);
            InitializeBlastField(campaignItem.BlastField5, 5, lblBlastField5, lblBlastFieldValue5);
        }

        private void InitializeBlastField(string blastFieldValue, int blastFieldId, Label blastLabel, Label blastValueLabel)
        {
            if (string.IsNullOrWhiteSpace(blastFieldValue))
            {
                return;
            }

            pnlBlastFields.Visible = true;
            var blastFieldsName = BusinessBlastFieldsName.GetByBlastFieldID(blastFieldId, ECNSession.CurrentSession().CurrentUser);
            blastLabel.Text = blastFieldsName != null ? blastFieldsName.Name + ":" : $"Field{blastFieldId}:";
            blastValueLabel.Text = blastFieldValue;
        }

        protected List<string> ValidateUdfTopic(
            int? layoutId,
            int? groupId,
            User currentUser,
            List<string> listErrorGroups)
        {
            var layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(Convert.ToInt32(layoutId), currentUser, false);
            var groupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(groupId), currentUser);
            var group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(groupId), currentUser);

            var contentHasTopic = ValidateContentSlot(currentUser, layout.ContentSlot1);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot2);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot3);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot4);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot5);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot6);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot7);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot8);
            contentHasTopic = contentHasTopic || ValidateContentSlot(currentUser, layout.ContentSlot9);

            if (contentHasTopic)
            {
                listErrorGroups.Add(group.GroupName);
                if (groupDataFields.Any(groupDataField =>
                    groupDataField.ShortName.IndexOf("TOPICS", StringComparison.InvariantCultureIgnoreCase) >= 0))
                {
                    listErrorGroups = new List<string>();
                }
            }

            return listErrorGroups;
        }

        private bool ValidateContentSlot(User currentUser, int? layoutContentSlot)
        {
            if (layoutContentSlot != null && layoutContentSlot != 0)
            {
                return ValidateTopicParamExists(Convert.ToInt32(layoutContentSlot), currentUser);
            }

            return false;
        }

        public bool ValidateTopicParamExists(int contentId, User currentUser)
        {
            var content = BusinessContent.GetByContentID(contentId, currentUser, true);
            return BusinessContent.TopicParamExists(content);
        }
    }
}