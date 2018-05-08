using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using ECN_Framework;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.blastsmanager
{
    public partial class regBlast : System.Web.UI.UserControl
    {
        ECN_Framework_BusinessLayer.Application.ECNSession currentSession = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

        private string BlastFromName
        {
            get
            {
                if (drpEmailFromName.Visible)
                    return drpEmailFromName.SelectedItem.Text;
                else
                    return txtEmailFromName.Text.Trim();
            }
            set
            {
                if (drpEmailFromName.Visible)
                {
                    drpEmailFromName.ClearSelection();
                    try
                    {
                        drpEmailFromName.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }
                    txtEmailFromName.Text = value.ToString().Trim();

                }
                else
                {
                    txtEmailFromName.Text = value.ToString().Trim();
                }
            }
        }

        private string BlastFromEmail
        {
            get
            {
                if (drpEmailFromName.Visible)
                    return drpEmailFrom.SelectedItem.Text;
                else
                    return txtEmailFrom.Text;
            }
            set
            {
                if (drpEmailFromName.Visible)
                {
                    drpEmailFrom.ClearSelection();
                    try
                    {
                        drpEmailFrom.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }

                    txtEmailFrom.Text = value.ToString().Trim();
                }
                else
                {
                    txtEmailFrom.Text = value.ToString().Trim();
                }
            }
        }

        private string BlastReplyToEmail
        {
            get
            {
                if (drpEmailFromName.Visible)
                    return drpReplyTo.SelectedItem.Text;
                else
                    return txtReplyTo.Text;
            }
            set
            {
                if (drpEmailFromName.Visible)
                {
                    drpReplyTo.ClearSelection();
                    try
                    {
                        drpReplyTo.Items.FindByText(value.Trim()).Selected = true;
                    }
                    catch { }
                    txtReplyTo.Text = value.Trim(); ;
                }
                else
                {
                    txtReplyTo.Text = value.Trim();
                }
            }
        }

        public string myNodeID
        {
            get
            {
                Object obj = ViewState["myNodeID"];
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return (String)obj;
                }
            }
            set
            {
                ViewState["myNodeID"] = value;
            }
        }

        public string myparentNodeId
        {
            get
            {
                Object obj = ViewState["myparentNodeId"];
                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (String)obj;
                }
            }
            set
            {
                ViewState["myparentNodeId"] = value;
            }
        }

        public int myparentFilterID
        {
            get
            {
                Object obj = ViewState["myparentFilterID"];
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    return (int)obj;
                }
            }
            set
            {
                ViewState["myparentFilterID"] = value;
            }
        }

        public string myparentFilterType
        {
            get
            {
                Object obj = ViewState["myparentFilterType"];
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return (String)obj;
                }
            }
            set
            {
                ViewState["myparentFilterType"] = value;
            }
        }

        public void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            BlastScheduler1.CanTestBlast = false;
            if (!IsPostBack)
            {
                groupExplorer1.enableSelectMode();
                layoutExplorer1.enableSelectMode();
            }
        }

        public void Reset()
        {
            DataTable dt = new DataTable();
            gvSelectedGroups.DataSource = dt;
            gvSelectedGroups.DataBind();
            lblGroups.Visible = true;
            groupExplorer1.reset();
            layoutExplorer1.reset();
            BlastScheduler1.ResetSchedule();
            lblLayoutName.Text = "-No Message Selected-";
            //schedulerReset();
            txtEmailFrom.Text = "";
            txtEmailFromName.Text = "";
            txtReplyTo.Text = "";
            txtSubject.Text = "";
            btnGroupConfigure.Enabled = true;
        }

        private void schedulerReset()
        {
            BlastScheduler1.CanTestBlast = false;
            BlastScheduler1.CanEmailPreview = false;
            BlastScheduler1.CanScheduleRecurringBlast = true;
            BlastScheduler1.SourceBlastID = 0;
            BlastScheduler1.SetupWizard();
        }

        public void loadData(ECN_Framework_Entities.Communicator.CampaignItem ci)
        {
            if (lblGroups.Text.Equals("-No Group Selected-"))
                lblGroups.Visible = false;
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(ci.BlastList[0].LayoutID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
            lblLayoutName.Text = layout.LayoutName;

            txtEmailFrom.Text = ci.FromEmail;
            txtEmailFromName.Text = ci.FromName;
            txtReplyTo.Text = ci.ReplyTo;
            txtSubject.Text = ci.BlastList[0].EmailSubject;


            List<ECN_Framework_Entities.Communicator.Group> grpList_selected = new List<ECN_Framework_Entities.Communicator.Group>();
            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciblast in ci.BlastList)
            {
                ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(ciblast.GroupID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                grpList_selected.Add(grp);
            }
            List<ECN_Framework_Entities.Communicator.Group> grpList_suppressed = new List<ECN_Framework_Entities.Communicator.Group>();
          
            foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppression in ci.SuppressionList)
            {
                ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(ciSuppression.GroupID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                grpList_suppressed.Add(grp);
            }
            groupExplorer1.setDataFromCampaignItem(ci);
            layoutExplorer1.selectedLayoutID = layout.LayoutID;
            lblLayoutID.Text = layout.LayoutID.ToString();

            gvSelectedGroups.DataSource = grpList_selected;
            gvSelectedGroups.DataBind();
            gvSuppressed.DataSource = grpList_suppressed;
            gvSuppressed.DataBind();
            
            BlastScheduler1.CanScheduleBlast = true;
            BlastScheduler1.RequestBlastID = 0;
            if (ci.BlastList.Count > 0)
            {
                if (ci.BlastList[0].BlastID != null)
                {
                    BlastScheduler1.SourceBlastID = ci.BlastList[0].BlastID.Value;                   
                }
            }
            else
                BlastScheduler1.SourceBlastID = 0;
            BlastScheduler1.SetupWizard();
            BlastScheduler1.LoadBlastSchedule();
        }

        public void Initialize(ECN_Framework_Entities.Communicator.CampaignItem ciParent)
        {
            if (ciParent != null)
            {
                lblGroups.Text = "SMART SEGMENT: " + myparentFilterType;
            }
            else
            {
                lblGroups.Text = "-No Group Selected-";
            }
            lblLayoutName.Text = "-No Message Selected-";
        }

        protected void groupExplorer_Hide(object sender, EventArgs e)
        {
            List<ecn.communicator.main.ECNWizard.Group.GroupObject> selectedGroupsDT = groupExplorer1.getSelectedGroups();
            List<ecn.communicator.main.ECNWizard.Group.GroupObject> suppressionGroupsDT = groupExplorer1.getSuppressionGroups();
            if (selectedGroupsDT.Count > 0)
            {
                if(lblGroups.Text.Equals("-No Group Selected-"))
                    lblGroups.Visible = false;
                gvSelectedGroups.DataSource = selectedGroupsDT;
                gvSelectedGroups.DataBind();
            }
            else
            {                
                lblGroups.Visible = true;
            }

            if (suppressionGroupsDT.Count > 0)
            {
                gvSuppressed.DataSource = suppressionGroupsDT;
                gvSuppressed.DataBind();
            }
            groupExplorer1.reset();
            this.modalPopupGroupExplorer.Hide();
        }

        protected void layoutExplorer_Hide(object sender, EventArgs e)
        {
            int selectedLayoutID = layoutExplorer1.selectedLayoutID;
            if (selectedLayoutID > 0)
            {
                ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(selectedLayoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                lblLayoutName.Text = layout.LayoutName;
                lblLayoutID.Text = selectedLayoutID.ToString();
            }
            this.modalPopupLayoutExplorer.Hide();
        }

        protected void lnkGroupConfigure_Click(object sender, EventArgs e)
        {
            this.modalPopupGroupExplorer.Show();
        }

        protected void lnkMessageConfigure_Click(object sender, EventArgs e)
        {
            this.modalPopupLayoutExplorer.Show();
        }

        protected void btnChangeEnvelope_onclick(object sender, EventArgs e)
        {
            ToggleEnvelopePanelHelper.ToggleEnvelopePanel(
                !drpEmailFrom.Visible,
                new TextBox[] { txtEmailFrom, txtReplyTo, txtEmailFromName },
                new DropDownList[] { drpEmailFrom, drpReplyTo, drpEmailFromName });
        }

        public void drpEmailFrom_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpEmailFromName.ClearSelection();
            drpReplyTo.ClearSelection();
            if (drpEmailFrom.SelectedIndex > -1)
            {
                drpEmailFromName.Items.FindByValue(drpEmailFrom.SelectedValue).Selected = true;
                drpReplyTo.Items.FindByValue(drpEmailFrom.SelectedValue).Selected = true;
            }

            BlastFromName = drpEmailFromName.SelectedItem.Text;
            BlastFromEmail = drpEmailFrom.SelectedItem.Text;
            BlastReplyToEmail = drpReplyTo.SelectedItem.Text;
        }

        public void drpReplyTo_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpEmailFromName.ClearSelection();
            drpEmailFrom.ClearSelection();

            if (drpReplyTo.SelectedIndex > -1)
            {
                drpEmailFromName.Items.FindByValue(drpReplyTo.SelectedValue).Selected = true;
                drpEmailFrom.Items.FindByValue(drpReplyTo.SelectedValue).Selected = true;
            }

            BlastFromName = drpEmailFromName.SelectedItem.Text;
            BlastFromEmail = drpEmailFrom.SelectedItem.Text;
            BlastReplyToEmail = drpReplyTo.SelectedItem.Text;
        }

        public void drpEmailFromName_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            drpEmailFrom.ClearSelection();
            drpReplyTo.ClearSelection();
            if (drpEmailFromName.SelectedIndex > -1)
            {
                drpReplyTo.Items.FindByValue(drpEmailFromName.SelectedValue).Selected = true;
                drpEmailFrom.Items.FindByValue(drpEmailFromName.SelectedValue).Selected = true;
            }

            BlastFromName = drpEmailFromName.SelectedItem.Text;
            BlastFromEmail = drpEmailFrom.SelectedItem.Text;
            BlastReplyToEmail = drpReplyTo.SelectedItem.Text;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Group, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            groupExplorer1.setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        public bool saveCampaignItem_regular(ECN_Framework_Entities.Communicator.CampaignItem ci, int campaignID)
        {
            try
            {
                if (ci == null)
                {
                    ci = new ECN_Framework_Entities.Communicator.CampaignItem();
                    ci.CampaignID = campaignID;
                    ci.CampaignItemName = "CampaignItem-Drip" + " " + DateTime.Now.ToString();
                    ci.CustomerID = currentSession.CurrentCustomer.CustomerID;
                    ci.CreatedUserID = currentSession.CurrentUser.UserID;
                    ci.IsHidden = true;
                }
                else
                {
                    ci.UpdatedUserID = currentSession.CurrentUser.UserID;
                }
                ci.NodeID = myNodeID;
                ci.FromEmail = StringFunctions.Remove(BlastFromEmail, StringFunctions.NonDomain());
                ci.FromName = BlastFromName;
                ci.ReplyTo = BlastReplyToEmail;
                ci.CampaignItemNameOriginal = ci.CampaignItemName;
                string SuppressionGroupIDs = string.Empty;
                string SelectedGroupIDs = string.Empty;

                foreach (GridViewRow gvr in gvSuppressed.Rows)
                {
                    SuppressionGroupIDs = SuppressionGroupIDs + ((Label)gvr.FindControl("lblGroupID")).Text;
                    SuppressionGroupIDs = SuppressionGroupIDs + ",";
                }
                SuppressionGroupIDs = SuppressionGroupIDs.TrimEnd(',');
                foreach (GridViewRow gvr in gvSelectedGroups.Rows)
                {
                    SelectedGroupIDs = SelectedGroupIDs + ((Label)gvr.FindControl("lblGroupID")).Text;
                    SelectedGroupIDs = SelectedGroupIDs + ",";
                }
                SelectedGroupIDs = SelectedGroupIDs.TrimEnd(',');

                StringTokenizer st = new StringTokenizer(SelectedGroupIDs, ',');
                StringTokenizer st1 = new StringTokenizer(SuppressionGroupIDs, ',');

                ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = BlastScheduler1.SetupSchedule("regular");
                if (setupInfo != null)
                {
                    ci.OverrideAmount = setupInfo.SendNowAmount;
                    ci.OverrideIsAmount = setupInfo.SendNowIsAmount;
                    ci.BlastScheduleID = setupInfo.BlastScheduleID;
                    ci.SendTime = setupInfo.SendTime;
                    ci.CompletedStep = 4;
                    ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
                    ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, currentSession.CurrentUser);
                    List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
                    if (!st.FirstToken().Equals(""))
                    {
                        while (st.HasMoreTokens())
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                            ciBlast.CampaignItemID = ci.CampaignItemID;
                            ciBlast.GroupID = Convert.ToInt32(st.NextToken().ToString());
                            ciBlast.EmailSubject = txtSubject.Text;
                            ciBlast.CustomerID = currentSession.CurrentCustomer.CustomerID;
                            ciBlast.CreatedUserID = currentSession.CurrentUser.UserID;
                            ciBlast.UpdatedUserID = currentSession.CurrentUser.UserID;
                            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(currentSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPersonalization))
                            {
                                if (dyanmicEmailFrom.Enabled && !dyanmicEmailFrom.SelectedValue.Equals(""))
                                    ciBlast.DynamicFromEmail = dyanmicEmailFrom.SelectedValue;
                                if (dyanmicEmailFromName.Enabled && !dyanmicEmailFromName.SelectedValue.Equals(""))
                                    ciBlast.DynamicFromName = dyanmicEmailFromName.SelectedValue;
                                if (dyanmicReplyToEmail.Enabled && !dyanmicReplyToEmail.SelectedValue.Equals(""))
                                    ciBlast.DynamicReplyTo = dyanmicReplyToEmail.SelectedValue;

                            }
                            ciBlast.LayoutID = Convert.ToInt32(lblLayoutID.Text);
                            ciBlastList.Add(ciBlast);
                        }
                    }
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(ci.CampaignItemID, ciBlastList, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                   
                    if (!st1.FirstToken().Equals(""))
                    {
                        while (st1.HasMoreTokens())
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppression = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                            ciSuppression.CampaignItemID = ci.CampaignItemID;
                            ciSuppression.GroupID = Convert.ToInt32(st1.NextToken().ToString());
                            ciSuppression.CustomerID = currentSession.CurrentCustomer.CustomerID;
                            ciSuppression.CreatedUserID = currentSession.CurrentUser.UserID;
                            ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Save(ciSuppression, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                        }
                    }
                    ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, currentSession.CurrentUser);
                }
                return true;
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
                return false;
            }
        }        

        public bool saveCampaignItem_smartsegment(ECN_Framework_Entities.Communicator.CampaignItem ci,ECN_Framework_Entities.Communicator.CampaignItem ciParent, int campaignID)
        {
            try
            {
                if (ci == null)
                {
                    ci = new ECN_Framework_Entities.Communicator.CampaignItem();
                    ci.CampaignID = campaignID;
                    ci.CampaignItemName = "CampaignItem-Drip" + " " + DateTime.Now.ToString();
                    ci.CustomerID = currentSession.CurrentCustomer.CustomerID;
                    ci.CreatedUserID = currentSession.CurrentUser.UserID;
                    ci.IsHidden = true;
                }
                else
                {
                    ci.UpdatedUserID = currentSession.CurrentUser.UserID;
                }
                ci.NodeID = myNodeID;
                ci.FromEmail = StringFunctions.Remove(BlastFromEmail, StringFunctions.NonDomain());
                ci.FromName = BlastFromName;
                ci.ReplyTo = BlastReplyToEmail;
                ci.CampaignItemNameOriginal = ci.CampaignItemName;
                string SuppressionGroupIDs = string.Empty;
                string SelectedGroupIDs = string.Empty;

                if (gvSelectedGroups.Rows.Count > 1)
                {
                    throwECNException("The parent CampaignItem is a multi-group blast. Please select only one group.");
                    return false;
                }

                foreach (GridViewRow gvr in gvSuppressed.Rows)
                {
                    SuppressionGroupIDs = SuppressionGroupIDs + ((Label)gvr.FindControl("lblGroupID")).Text;
                    SuppressionGroupIDs = SuppressionGroupIDs + ",";
                }
                SuppressionGroupIDs = SuppressionGroupIDs.TrimEnd(',');
                foreach (GridViewRow gvr in gvSelectedGroups.Rows)
                {
                    SelectedGroupIDs = SelectedGroupIDs + ((Label)gvr.FindControl("lblGroupID")).Text;
                    SelectedGroupIDs = SelectedGroupIDs + ",";
                }
                SelectedGroupIDs = SelectedGroupIDs.TrimEnd(',');

                StringTokenizer st = new StringTokenizer(SelectedGroupIDs, ',');
                StringTokenizer st1 = new StringTokenizer(SuppressionGroupIDs, ',');

                ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = BlastScheduler1.SetupSchedule("regular");
                if (setupInfo != null)
                {
                    if (ciParent.SendTime > ci.SendTime)
                    {
                        throwECNException("The scheduled time for this blast must be greater than the scheduled time of preceeding blasts");
                        return false;
                    }
                    ci.OverrideAmount = setupInfo.SendNowAmount;
                    ci.OverrideIsAmount = setupInfo.SendNowIsAmount;
                    ci.BlastScheduleID = setupInfo.BlastScheduleID;
                    ci.SendTime = setupInfo.SendTime;
                    ci.CompletedStep = 4;
                    ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
                    ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();


                    ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    ciBlast.GroupID = Convert.ToInt32(st.NextToken().ToString());
                    ciBlast.EmailSubject = txtSubject.Text;
                    ciBlast.CustomerID = currentSession.CurrentCustomer.CustomerID;
                    ciBlast.CreatedUserID = currentSession.CurrentUser.UserID;
                    ciBlast.UpdatedUserID = currentSession.CurrentUser.UserID;
                    //ciBlast.SmartSegmentID = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetNewIDFromOldID(myparentFilterID);
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(currentSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPersonalization))
                    {
                        if (dyanmicEmailFrom.Enabled && !dyanmicEmailFrom.SelectedValue.Equals(""))
                            ciBlast.DynamicFromEmail = dyanmicEmailFrom.SelectedValue;
                        if (dyanmicEmailFromName.Enabled && !dyanmicEmailFromName.SelectedValue.Equals(""))
                            ciBlast.DynamicFromName = dyanmicEmailFromName.SelectedValue;
                        if (dyanmicReplyToEmail.Enabled && !dyanmicReplyToEmail.SelectedValue.Equals(""))
                            ciBlast.DynamicReplyTo = dyanmicReplyToEmail.SelectedValue;

                    }
                    ciBlast.LayoutID = Convert.ToInt32(lblLayoutID.Text);
                    string refBlastsIDs = string.Empty;
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciParentBlast in ciParent.BlastList)
                    {
                        refBlastsIDs = refBlastsIDs + ciParentBlast.BlastID.ToString();
                        refBlastsIDs = refBlastsIDs + ",";
                    }
                    refBlastsIDs = refBlastsIDs.TrimEnd(',');

                    char[] delimiter = { ',' };
                    string[] refBlasts = refBlastsIDs.Split(delimiter);

                    List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> cibRefBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();
                    for (int j = 0; j < refBlasts.Length; j++)
                    {
                        ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast cibRefBlast = new ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast();
                        cibRefBlast.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                        cibRefBlast.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                        cibRefBlast.RefBlastID = Convert.ToInt32(refBlasts[j].ToString());
                        cibRefBlastList.Add(cibRefBlast);
                    }
                    ciBlast.RefBlastList = cibRefBlastList;
                    List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();
                    ciBlastList.Add(ciBlast);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, currentSession.CurrentUser);
                        ciBlast.CampaignItemID = ci.CampaignItemID;
                        if (ci.BlastList != null)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cib in ci.BlastList)
                            {
                                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Delete(cib.CampaignItemBlastID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                            }
                        }
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(ci.CampaignItemID, ciBlastList, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Delete(ci.CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        if (!st1.FirstToken().Equals(""))
                        {
                            while (st1.HasMoreTokens())
                            {
                                ECN_Framework_Entities.Communicator.CampaignItemSuppression ciSuppression = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                                ciSuppression.CampaignItemID = ci.CampaignItemID;
                                ciSuppression.GroupID = Convert.ToInt32(st1.NextToken().ToString());
                                ciSuppression.CustomerID = currentSession.CurrentCustomer.CustomerID;
                                ciSuppression.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                                ciSuppression.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                                ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Save(ciSuppression, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                            }
                        }
                        ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(ci.CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        scope.Complete();
                    }
                }
                return true;
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
                return false;
            }
        }
    }
}