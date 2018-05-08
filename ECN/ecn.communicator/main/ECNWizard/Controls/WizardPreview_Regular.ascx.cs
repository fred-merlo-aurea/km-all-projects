using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using ApplicationSession = ECN_Framework_BusinessLayer.Application.ECNSession;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessAccounts = ECN_Framework_BusinessLayer.Accounts;
using BusinessUser = KMPlatform.BusinessLogic.User;
using CommonEnums = ECN_Framework_Common.Objects.Enums;
using KMEnums = KMPlatform.Enums;
using SecurityException = ECN_Framework_Common.Objects.SecurityException;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardPreview : WizardContentBase, IECNWizard
    {
        private const string ColumnGroupName = "GroupName";
        private const string ColumnFilterName = "FilterName";
        private const string ItemNameNoFilter = "-No Filter Selected-";
        private const string TextNoSuppression = "Transactional Emails - Don't Apply Suppression";
        private const string TextNoFilterSelected = "-No Filter Selected-";
        private const string TargetBlank = "_blank";
        private const int SocialMedia4 = 4;
        int _campaignItemID = 0;
        public int CampaignItemID
        {
            set
            {
                _campaignItemID = value;
            }
            get
            {
                return _campaignItemID;
            }
        }

        string _errormessage = string.Empty;
        public string ErrorMessage
        {
            set
            {
                _errormessage = value;
            }
            get
            {
                return _errormessage;
            }
        }

        public void Initialize()
        {
            if (BusinessUser.HasAccess(
                ApplicationSession.CurrentSession().CurrentUser,
                KMEnums.Services.EMAILMARKETING,
                KMEnums.ServiceFeatures.Blast,
                KMEnums.Access.Edit))
            {
                if (CampaignItemID != 0)
                {
                    if (BusinessAccounts.Customer.HasProductFeature(
                        ApplicationSession.CurrentSession().CurrentUser.CustomerID,
                        KMEnums.Services.EMAILMARKETING,
                        KMEnums.ServiceFeatures.EmailPersonalization))
                    {
                        pnlDynamicPersonalization.Visible = true;
                    }
                    var campaignItem = 
                        BusinessCommunicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                    lblFromEmail.Text = campaignItem.FromEmail;
                    lblReplyTo.Text = campaignItem.ReplyTo;
                    lblFromName.Text = campaignItem.FromName;

                    var layout = BusinessCommunicator.Layout.GetByLayoutID_NoAccessCheck(
                        campaignItem.BlastList[0].LayoutID.Value, 
                        false);
                    lblMessage.Text = layout.LayoutName;
                    lblSubject.Text = campaignItem.BlastList[0].EmailSubject;

                    lblFromEmail_DP.Text = campaignItem.BlastList[0].DynamicFromEmail;
                    lblReplyTo_DP.Text = campaignItem.BlastList[0].DynamicReplyTo;
                    lblFromName_DP.Text = campaignItem.BlastList[0].DynamicFromName;
                    InitializeBlastFields(campaignItem);

                    lblEstimatedSends.Text = SaveNoBlastSuppressions(campaignItem);

                    FillBlastFilter(campaignItem);

                    ProcessSuppression(campaignItem, layout);

                    ProcessMedia();
                }
            }
            else
            {
                throw new SecurityException()
                {
                    SecurityType = CommonEnums.SecurityExceptionType.RoleAccess
                };
            }
        }

        private void ProcessSuppression(CampaignItem campaignItem, Layout layout)
        {
            Guard.NotNull(campaignItem, nameof(campaignItem));

            if (campaignItem.IgnoreSuppression.Value)
            {
                lblSuppresed.Visible = true;
                lblTransactional.Text = TextNoSuppression;
                lblTransactional.Visible = true;
                rpterSuppression.Visible = false;
            }
            else if (campaignItem.SuppressionList.Count > 0)
            {
                FillSuppresionFilter(campaignItem);
            }
            else
            {
                lblSuppresed.Visible = false;
            }

            hlPreviewHTML.NavigateUrl = $"~/main/content/preview.aspx?LayoutID={layout.LayoutID}";
            hlPreviewHTML.Target = TargetBlank;
            hlPreviewTEXT.NavigateUrl = $"~/main/content/preview.aspx?LayoutID={layout.LayoutID}&Format=text";
            hlPreviewTEXT.Target = TargetBlank;
        }

        private void ProcessMedia()
        {
            var listCism = BusinessCommunicator.CampaignItemSocialMedia.GetByCampaignItemID(CampaignItemID);
            if (listCism.Count > 0)
            {
                if (listCism.Any(x => x.SimpleShareDetailID != null))
                {
                    tblSimple.Visible = true;
                    gvSimpleShare.DataSource = listCism.Where(x => x.SimpleShareDetailID != null);
                    gvSimpleShare.DataBind();
                }
                else
                {
                    tblSimple.Visible = false;
                }

                if (listCism.Any(x => x.SimpleShareDetailID == null || x.SocialMediaID == SocialMedia4))
                {
                    tblSubscriber.Visible = true;
                    gvSubscriberShare.DataSource =
                        listCism.Where(x => x.SimpleShareDetailID == null || x.SocialMediaID == SocialMedia4);
                    gvSubscriberShare.DataBind();
                }
                else
                {
                    tblSubscriber.Visible = false;
                }
            }
            else
            {
                tblSocial.Visible = false;
            }
        }

        private void FillSuppresionFilter(CampaignItem campaignItem)
        {
            Guard.NotNull(campaignItem, nameof(campaignItem));

            var dtSuppression = new DataTable();
            dtSuppression.Columns.Add(new DataColumn(ColumnGroupName, typeof(string)));
            dtSuppression.Columns.Add(new DataColumn(ColumnFilterName, typeof(string)));
            foreach (var ciSuppression in campaignItem.SuppressionList)
            {
                var row = dtSuppression.NewRow();
                var comGroup =
                    BusinessCommunicator.Group.GetByGroupID_NoAccessCheck(ciSuppression.GroupID.Value);
                row[ColumnGroupName] = comGroup.GroupName;
                if (ciSuppression.Filters != null && ciSuppression.Filters.Count > 0)
                {
                    foreach (var blastFilter in ciSuppression.Filters)
                    {
                        if (blastFilter.SmartSegmentID != null && !string.IsNullOrWhiteSpace(blastFilter.RefBlastIDs))
                        {
                            var smSegment =
                                BusinessCommunicator.SmartSegment.GetSmartSegmentByID(blastFilter.SmartSegmentID.Value);

                            row[ColumnFilterName] =
                                $"{smSegment.SmartSegmentName} for Blast(s) {blastFilter.RefBlastIDs}";
                            dtSuppression.Rows.Add(row);
                        }
                        else if (blastFilter.FilterID != null)
                        {
                            var filter = BusinessCommunicator.Filter.GetByFilterID_NoAccessCheck(
                                blastFilter.FilterID.Value);
                            row[ColumnFilterName] = filter.FilterName;
                            dtSuppression.Rows.Add(row);
                        }
                        row = dtSuppression.NewRow();
                        row[ColumnGroupName] = comGroup.GroupName;
                    }
                }
                else
                {
                    row[ColumnFilterName] = TextNoFilterSelected;
                    dtSuppression.Rows.Add(row);
                }
            }
            lblTransactional.Visible = false;
            rpterSuppression.Visible = true;
            rpterSuppression.DataSource = dtSuppression;
            rpterSuppression.DataBind();
        }

        private void FillBlastFilter(CampaignItem campaignItem)
        {
            Guard.NotNull(campaignItem, nameof(campaignItem));

            var table = new DataTable();
            table.Columns.Add(new DataColumn(ColumnGroupName, typeof(string)));
            table.Columns.Add(new DataColumn(ColumnFilterName, typeof(string)));
            foreach (var ciBlast in campaignItem.BlastList)
            {
                var row = table.NewRow();
                var groupById = BusinessCommunicator.Group.GetByGroupID_NoAccessCheck(ciBlast.GroupID.Value);
                row[ColumnGroupName] = groupById.GroupName;

                if (ciBlast.Filters != null && ciBlast.Filters.Count > 0)
                {
                    foreach (var ss in ciBlast.Filters.Where(x => x.SmartSegmentID != null).ToList())
                    {
                        var smSegment =
                            BusinessCommunicator.SmartSegment.GetSmartSegmentByID(ss.SmartSegmentID.Value);

                        row[ColumnFilterName] = $"{smSegment.SmartSegmentName} for Blast(s) {ss.RefBlastIDs}";
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row[ColumnGroupName] = groupById.GroupName;
                    }

                    foreach (var currentFilter in ciBlast.Filters.Where(x => x.FilterID != null).ToList())
                    {
                        var filter =
                            BusinessCommunicator.Filter.GetByFilterID_NoAccessCheck(currentFilter.FilterID.Value);
                        row[ColumnFilterName] = filter.FilterName;
                        table.Rows.Add(row);
                        row = table.NewRow();
                        row[ColumnGroupName] = groupById.GroupName;
                    }
                }
                else
                {
                    row[ColumnFilterName] = ItemNameNoFilter;
                    table.Rows.Add(row);
                }
            }

            rpterGroupDetails.DataSource = table;
            rpterGroupDetails.DataBind();
        }

        public bool Save()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID,  true);
            ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            ci.CompletedStep = 5;
            ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            return true;
        }        
    }
}
