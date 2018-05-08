using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Entities.Communicator;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardPreview_AB : WizardContentBase, IECNWizard
    {
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
            if (KMPlatform.BusinessLogic.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.Edit))
            {
                if (CampaignItemID != 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
                    lblFromEmailA.Text = ci.BlastList[0].EmailFrom;
                    lblReplyToA.Text = ci.BlastList[0].ReplyTo;
                    lblFromNameA.Text = ci.BlastList[0].FromName;

                    ECN_Framework_Entities.Communicator.Layout layoutA = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ci.BlastList[0].LayoutID.Value, false);
                    lblMessageA.Text = layoutA.LayoutName;
                    lblSubjectA.Text = ci.BlastList[0].EmailSubject;

                    ECN_Framework_Entities.Communicator.Layout layoutB = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ci.BlastList[1].LayoutID.Value, false);
                    lblMessageB.Text = layoutB.LayoutName;
                    lblSubjectB.Text = ci.BlastList[1].EmailSubject;
                    lblFromEmailB.Text = ci.BlastList[1].EmailFrom;
                    lblReplyToB.Text = ci.BlastList[1].ReplyTo;
                    lblFromNameB.Text = ci.BlastList[1].FromName;

                    InitializeBlastFields(ci);

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("GroupName", typeof(string)));
                    dt.Columns.Add(new DataColumn("FilterName", typeof(string)));
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(ci.BlastList[0].GroupID.Value);
                    DataRow dr = dt.NewRow();
                    dr["GroupName"] = group.GroupName;
                    if (ci.BlastList[0].Filters != null && ci.BlastList[0].Filters.Count > 0)
                    {

                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter ss in ci.BlastList[0].Filters.Where(x => x.SmartSegmentID != null).ToList())
                        {
                            ECN_Framework_Entities.Communicator.SmartSegment smSegment =
                               ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(ss.SmartSegmentID.Value);

                            dr["FilterName"] = smSegment.SmartSegmentName + " for Blast(s) " + ss.RefBlastIDs;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr["GroupName"] = group.GroupName;
                        }

                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in ci.BlastList[0].Filters.Where(x => x.FilterID != null))
                        {
                            ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                            DataRow drG = dt.NewRow();

                            dr["FilterName"] = filter.FilterName;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr["GroupName"] = group.GroupName;

                        }
                    }
                    else
                    {
                        dr["FilterName"] = "-No Filter Selected-";
                        dt.Rows.Add(dr);
                    }

                    lblEstimatedSends.Text = SaveNoBlastSuppressions(ci);

                    rpterGroupDetails.DataSource = dt;
                    rpterGroupDetails.DataBind();
                    if (ci.IgnoreSuppression.Value)
                    {
                        lblSuppresed.Visible = true;
                        lblTransactional.Text = "Transactional Emails - Don't Apply Suppression";
                        lblTransactional.Visible = true;
                        rpterSuppression.Visible = false;
                    }
                    else if (ci.SuppressionList.Count > 0)
                    {
                        DataTable dtSuppression = new DataTable();
                        dtSuppression.Columns.Add(new DataColumn("GroupName", typeof(string)));
                        dtSuppression.Columns.Add(new DataColumn("FilterName", typeof(string)));
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppression in ci.SuppressionList)
                        {
                            DataRow drSuppression = dtSuppression.NewRow();
                            ECN_Framework_Entities.Communicator.Group groupSuppression = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(ciSuppression.GroupID.Value);
                            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listCIBF = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID(ciSuppression.CampaignItemSuppressionID);
                            drSuppression["GroupName"] = groupSuppression.GroupName;
                            if (listCIBF.Count > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter ss in listCIBF.Where(x => x.SmartSegmentID != null).ToList())
                                {
                                    ECN_Framework_Entities.Communicator.SmartSegment smSegment =
                                       ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(ss.SmartSegmentID.Value);

                                    drSuppression["FilterName"] = smSegment.SmartSegmentName + " for Blast(s) " + ss.RefBlastIDs;
                                    dtSuppression.Rows.Add(drSuppression);
                                    drSuppression = dtSuppression.NewRow();
                                    drSuppression["GroupName"] = groupSuppression.GroupName;
                                }

                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in listCIBF.Where(x => x.FilterID != null))
                                {
                                    ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(cibf.FilterID.Value);
                                    DataRow drG = dt.NewRow();

                                    drSuppression["FilterName"] = filter.FilterName;
                                    dtSuppression.Rows.Add(drSuppression);
                                    drSuppression = dtSuppression.NewRow();
                                    drSuppression["GroupName"] = groupSuppression.GroupName;

                                }
                            }
                            else
                            {
                                drSuppression["FilterName"] = "-No Filter Selected-";
                                dtSuppression.Rows.Add(drSuppression);
                            }


                        }
                        rpterSuppression.DataSource = dtSuppression;
                        rpterSuppression.DataBind();
                    }
                    else
                    {
                        lblSuppresed.Visible = false;
                    }



                    hlPreviewHTML1.NavigateUrl = "~/main/content/preview.aspx?LayoutID=" + layoutA.LayoutID;
                    hlPreviewHTML1.Target = "_blank";
                    hlPreviewTEXT1.NavigateUrl = "~/main/content/preview.aspx?LayoutID=" + layoutA.LayoutID + "&Format=text";
                    hlPreviewTEXT1.Target = "_blank";

                    hlPreviewHTML2.NavigateUrl = "~/main/content/preview.aspx?LayoutID=" + layoutB.LayoutID;
                    hlPreviewHTML2.Target = "_blank";
                    hlPreviewTEXT2.NavigateUrl = "~/main/content/preview.aspx?LayoutID=" + layoutB.LayoutID + "&Format=text";
                    hlPreviewTEXT2.Target = "_blank";

                    List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> listCISM = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemID(CampaignItemID);
                    if (listCISM.Count > 0)
                    {
                        if (listCISM.Where(x => x.SimpleShareDetailID != null).Count() > 0)
                        {
                            tblSimple.Visible = true;
                            gvSimpleShare.DataSource = listCISM.Where(x => x.SimpleShareDetailID != null);
                            gvSimpleShare.DataBind();
                        }
                        else
                            tblSimple.Visible = false;

                        if (listCISM.Where(x => x.SimpleShareDetailID == null).Count() > 0)
                        {
                            tblSubscriber.Visible = true;
                            gvSubscriberShare.DataSource = listCISM.Where(x => x.SimpleShareDetailID == null);
                            gvSubscriberShare.DataBind();
                        }
                        else
                            tblSubscriber.Visible = false;
                    }
                    else
                    {
                        tblSocial.Visible = false;
                    }
                }
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException() { SecurityType = ECN_Framework_Common.Objects.Enums.SecurityExceptionType.RoleAccess };
            }
        }

        public bool Save()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            ci.CompletedStep = 4;
            ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            return true;
        }        

        protected override void gvSubscriberShare_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Method body do nothing valuable. Just to interface compatibility.
        }
    }
}