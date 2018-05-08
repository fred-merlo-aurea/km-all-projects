using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ECN_Framework_Entities.Salesforce;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;
using System.Linq;
using System.Text;

namespace ecn.communicator.main.ECNWizard.Controls
{
    public partial class WizardPreview_SF : WizardContentBase, IECNWizard
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
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPersonalization))
                    {
                        pnlDynamicPersonalization.Visible = true;
                    }
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID,  true);
                    lblFromEmail.Text = ci.FromEmail;
                    lblReplyTo.Text = ci.ReplyTo;
                    lblFromName.Text = ci.FromName;

                    ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(ci.BlastList[0].LayoutID.Value,  false);
                    lblMessage.Text = layout.LayoutName;
                    lblSubject.Text = ci.BlastList[0].EmailSubject;

                    lblFromEmail_DP.Text = ci.BlastList[0].DynamicFromEmail;
                    lblReplyTo_DP.Text = ci.BlastList[0].DynamicReplyTo;
                    lblFromName_DP.Text = ci.BlastList[0].DynamicFromName;

                    #region BlastFields
                    pnlBlastFields.Visible = false;
                    if (!string.IsNullOrEmpty(ci.BlastField1))
                    {
                        pnlBlastFields.Visible = true;
                        ECN_Framework_Entities.Communicator.BlastFieldsName bfn1 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(1, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        lblBlastField1.Text = bfn1.Name;
                        lblBlastFieldValue1.Text = ci.BlastField1;
                    }
                    if (!string.IsNullOrEmpty(ci.BlastField2))
                    {
                        pnlBlastFields.Visible = true;
                        ECN_Framework_Entities.Communicator.BlastFieldsName bfn2 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(2, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        lblBlastField2.Text = bfn2.Name;
                        lblBlastFieldValue2.Text = ci.BlastField2;
                    }
                    if (!string.IsNullOrEmpty(ci.BlastField3))
                    {
                        pnlBlastFields.Visible = true;
                        ECN_Framework_Entities.Communicator.BlastFieldsName bfn3 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(3, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        lblBlastField3.Text = bfn3.Name;
                        lblBlastFieldValue3.Text = ci.BlastField3;
                    }
                    if (!string.IsNullOrEmpty(ci.BlastField4))
                    {
                        pnlBlastFields.Visible = true;
                        ECN_Framework_Entities.Communicator.BlastFieldsName bfn4 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(4, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        lblBlastField4.Text = bfn4.Name;
                        lblBlastFieldValue4.Text = ci.BlastField4;
                    }
                    if (!string.IsNullOrEmpty(ci.BlastField5))
                    {
                        pnlBlastFields.Visible = true;
                        ECN_Framework_Entities.Communicator.BlastFieldsName bfn5 = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName.GetByBlastFieldID(5, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        lblBlastField5.Text = bfn5.Name;
                        lblBlastFieldValue5.Text = ci.BlastField5;
                    }
                    #endregion

                    ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
                    SF_Authentication.RefreshSalesForceToken(sfs.RefreshToken, sfs.ConsumerSecret, sfs.ConsumerKey, sfs.SandboxMode.Value);
                    List<SF_Campaign> listCamp = SF_Campaign.GetList(SF_Authentication.Token.access_token, "WHERE ID ='" + ci.SFCampaignID.ToString() + "'").OrderBy(x => x.Name).ToList();
                    lblSFCampaignName.Text = listCamp[0].Name.ToString();

                    hlPreviewHTML.NavigateUrl = "~/main/content/preview.aspx?LayoutID=" + layout.LayoutID;
                    hlPreviewHTML.Target = "_blank";
                    hlPreviewTEXT.NavigateUrl = "~/main/content/preview.aspx?LayoutID=" + layout.LayoutID + "&Format=text";
                    hlPreviewTEXT.Target = "_blank";

                    StringBuilder xmlGroups = new StringBuilder();
                    xmlGroups.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlGroups.Append("<NoBlast>");
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast b in ci.BlastList)
                    {
                        xmlGroups.Append("<Group id=\"" + b.GroupID.ToString() + "\">");
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in b.Filters.Where(x => x.SmartSegmentID != null).ToList())
                        {

                            xmlGroups.Append("<SmartSegmentID id=\"" + cibf.SmartSegmentID + "\">");
                            xmlGroups.Append("<RefBlastIDs>" + cibf.RefBlastIDs + "</RefBlastIDs></SmartSegmentID>");

                        }

                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in b.Filters.Where(x => x.FilterID != null).ToList())
                        {
                            xmlGroups.Append("<FilterID id=\"" + cibf.FilterID.ToString() + "\" />");
                        }

                        xmlGroups.Append("</Group>");
                    }

                    xmlGroups.Append("<SuppressionGroup>");
                    xmlGroups.Append("</SuppressionGroup>");
                    xmlGroups.Append("</NoBlast>");

                    DataTable dtSends = ECN_Framework_BusinessLayer.Communicator.Blast.GetEstimatedSendsCount(xmlGroups.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID, ci.IgnoreSuppression.HasValue ? ci.IgnoreSuppression.Value : false);
                    lblSFEstimatedSends.Text = dtSends.Rows[0][0].ToString();

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
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }

        }

        public bool Save()
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(CampaignItemID, true);
            ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            ci.CompletedStep = 5;
            ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            return true;
        }
    }
}
