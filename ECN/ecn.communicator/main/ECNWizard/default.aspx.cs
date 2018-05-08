using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ECN_Framework.Communicator;
using ECN_Framework.Common;
using System.Transactions;
using System.Configuration;

namespace ecn.communicator.main.ECNWizard
{
    public partial class _default : ECN_Framework.WebPageHelper
    {
        int SavedRecordCount = 0;
        int PendingRecordCount = 0;
        int ActiveRecordCount = 0;

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
            update1.Update();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.INDEX;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";
            Master.MasterRegisterButtonForPostBack(gvSaved);
            SentCampaigns.EmojiEvent += new EventHandler(DoTwemojiOnGridHandler);

            SentCampaigns.ErrorEvent += (s, eE) => ErrorHandler(s, eE);
            if (!IsPostBack)
            {
                ViewState["p_SortField"] = "DateCreated";
                ViewState["p_SortDirection"] = "DESC";

                ViewState["sc_SortField"] = "Sendtime";
                ViewState["sc_SortDirection"] = "DESC";

                ViewState["s_SortField"] = "Sendtime";
                ViewState["s_SortDirection"] = "DESC";

                loadGrids();

            }
        }

        private void DoTwemojiOnGridHandler(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "pageloaded();", true);
        }

        private void ErrorHandler(object sender, EventArgs e)
        {
            setECNError(SentCampaigns.ecnException);
        }

        #region LoadData

        private void loadCampaignItemTemplates()
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplate> ciTemplateList =
            ECN_Framework_BusinessLayer.Communicator.CampaignItemTemplate.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false, "active");
            var result = (from src in ciTemplateList
                          orderby src.TemplateName
                          select src).ToList();
            drpCampaignItemTemplate.DataSource = result;
            drpCampaignItemTemplate.DataTextField = "TemplateName";
            drpCampaignItemTemplate.DataValueField = "CampaignItemTemplateID";
            drpCampaignItemTemplate.DataBind();
            drpCampaignItemTemplate.Items.Insert(0, new ListItem("-Select-", "-Select-"));

            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignItemTemplate, KMPlatform.Enums.Access.Edit))
                lnkNewTemplate.Visible = true;
            else
                lnkNewTemplate.Visible = false;
        }

        private void loadGrids()
        {
            loadSavedCampaignsItems();
            loadScheduledCampaignsItems();
            loadActiveCampaignItems();
            update1.Update();
        }

        private void loadActiveCampaignItems()
        {
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByStatus(Master.UserSession.CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Active.ToString(), Master.UserSession.CurrentUser);
            ActiveRecordCount = dt.Rows.Count;
            gvActive.DataSource = dt;
            try
            {
                gvActive.DataBind();
            }
            catch
            {
                gvActive.PageIndex = 0;
                gvActive.DataBind();
            }
        }

        private void loadSavedCampaignsItems()
        {
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByStatus(Master.UserSession.CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Saved.ToString(), Master.UserSession.CurrentUser);
            SavedRecordCount = dt.Rows.Count;
            gvSaved.DataSource = dt;
            try
            {
                gvSaved.DataBind();
            }
            catch
            {
                gvSaved.PageIndex = 0;
                gvSaved.DataBind();
            }

        }

        private void loadScheduledCampaignsItems()
        {
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByStatus(Master.UserSession.CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending.ToString(), Master.UserSession.CurrentUser);
            PendingRecordCount = dt.Rows.Count;
            gvPending.DataSource = dt;
            try
            {
                gvPending.DataBind();
            }
            catch
            {
                gvPending.PageIndex = 0;
                gvPending.DataBind();
            }
        }


        #endregion

        #region SavedGrid Events
        protected void gvSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.gvSaved.PageSize = int.Parse(dropDown.SelectedValue);
            loadSavedCampaignsItems();
        }

        protected void gvSaved_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvSaved.PageCount)
            {
                this.gvSaved.PageIndex = pageNumber - 1;
            }
            else
            {
                this.gvSaved.PageIndex = 0;
            }
            loadSavedCampaignsItems();
        }

        protected void gvSaved_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsContent = (Label)e.Row.FindControl("gvSaved_lblTotalRecords");
                lblTotalRecordsContent.Text = SavedRecordCount.ToString();

                Label lblTotalNumberOfPagesContent = (Label)e.Row.FindControl("gvSaved_lblTotalNumberOfPages");
                lblTotalNumberOfPagesContent.Text = gvSaved.PageCount.ToString();

                TextBox txtGoToPageContent = (TextBox)e.Row.FindControl("gvSaved_txtGoToPage");
                txtGoToPageContent.Text = (gvSaved.PageIndex + 1).ToString();

                DropDownList ddlPageSizeContent = (DropDownList)e.Row.FindControl("gvSaved_ddlPageSize");
                ddlPageSizeContent.SelectedValue = gvSaved.PageSize.ToString();
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                try
                {
                    if (dr["CampaignItemType"].ToString().ToLower() == ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString().ToLower())
                    {
                        LinkButton lb = (LinkButton)e.Row.FindControl("CopyCampaignItemBtn");
                        lb.Visible = false;
                    }

                }
                catch { }
            }
        }

        protected void gvSaved_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvSaved.PageIndex = e.NewPageIndex;
            }
            gvSaved.DataBind();
            loadSavedCampaignsItems();
        }

        protected void gvSaved_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteCampaignItem"))
            {
                DeleteCampaignItem(Convert.ToInt32(e.CommandArgument.ToString()));
            }
            else if (e.CommandName.Equals("CopyCampaignItem"))
            {
                CopyCampaignItem(Convert.ToInt32(e.CommandArgument.ToString()));
            }
            DoTwemojiOnGridHandler(sender, e);
        }

        private void CopyCampaignItem(int campaignItemID)
        {
            int copyCampaignItemID = ECN_Framework_BusinessLayer.Communicator.CampaignItem.CopyCampaignItem(campaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(copyCampaignItemID, Master.UserSession.CurrentUser, false);
            string redirectURL = "/ecn.communicator/main/ecnwizard/wizardsetup.aspx?CampaignItemID=" + ci.CampaignItemID + "&campaignItemType=" + ci.CampaignItemType;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redir", "document.location.href = '" + redirectURL + "';", true);
        }
        #endregion

        #region PendingGrid Events
        protected void gvPending_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.gvPending.PageSize = int.Parse(dropDown.SelectedValue);
            loadScheduledCampaignsItems();
        }

        protected void gvPending_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvPending.PageCount)
            {
                this.gvPending.PageIndex = pageNumber - 1;
            }
            else
            {
                this.gvPending.PageIndex = 0;
            }

            loadScheduledCampaignsItems();
        }

        protected void gvPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsContent = (Label)e.Row.FindControl("gvPending_lblTotalRecords");
                lblTotalRecordsContent.Text = PendingRecordCount.ToString();

                Label lblTotalNumberOfPagesContent = (Label)e.Row.FindControl("gvPending_lblTotalNumberOfPages");
                lblTotalNumberOfPagesContent.Text = gvPending.PageCount.ToString();

                TextBox txtGoToPageContent = (TextBox)e.Row.FindControl("gvPending_txtGoToPage");
                txtGoToPageContent.Text = (gvPending.PageIndex + 1).ToString();

                DropDownList ddlPageSizeContent = (DropDownList)e.Row.FindControl("gvPending_ddlPageSize");
                ddlPageSizeContent.SelectedValue = gvPending.PageSize.ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                LinkButton blastDetails = (LinkButton)e.Row.FindControl("lnkbtnBlastDetails");
                blastDetails.CommandArgument = e.Row.RowIndex.ToString();

                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
                imgbtnEdit.CommandArgument = drv["CampaignItemID"].ToString();

                imgbtnEdit.CommandName = drv["CampaignItemType"].ToString();

                ImageButton imgbtnCancel = (ImageButton)e.Row.FindControl("CancelCampaignItemBtn");
                imgbtnCancel.CommandArgument = drv["CampaignItemID"].ToString();
                imgbtnCancel.CommandName = drv["CampaignItemType"].ToString();

                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("DeleteCampaignItemBtn");
                imgbtnDelete.CommandArgument = drv["CampaignitemID"].ToString();
                imgbtnDelete.CommandName = drv["CampaignitemType"].ToString();

            }
        }

        protected void gvPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvPending.PageIndex = e.NewPageIndex;
            }
            gvPending.DataBind();
            loadScheduledCampaignsItems();
        }

        protected void gvPending_Command(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DeleteCampaignItem"))
            {
                DeleteCampaignItem(Convert.ToInt32(e.CommandArgument.ToString()));
            }
            else if (e.CommandName == "BlastDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                loadBlastDetails_Pending(index);
            }
            else if (e.CommandName.Equals("CancelCampaignItem"))
            {
                CancelCampaignItem(Convert.ToInt32(e.CommandArgument.ToString()));

            }
            DoTwemojiOnGridHandler(sender, e);
        }

        private void loadBlastDetails_Pending(int index)
        {

            Panel pnlBlastReport = gvPending.Rows[index].FindControl("pnlBlastReport") as Panel;
            GridView gvBlastDetails = gvPending.Rows[index].FindControl("gvBlastDetails") as GridView;
            int campaignItemID = Convert.ToInt32(((Label)gvPending.Rows[index].FindControl("lblCampaignItemID")).Text);
            LinkButton exp = gvPending.Rows[index].FindControl("lnkbtnBlastDetails") as LinkButton;
            if (gvBlastDetails.Rows.Count > 0)
            {
                if (pnlBlastReport.Visible == true)
                {
                    pnlBlastReport.Visible = false;
                    exp.Text = "+Details";
                    gvPending.Rows[index].Font.Bold = false;

                }
                else if (pnlBlastReport.Visible == false)
                {
                    pnlBlastReport.Visible = true;
                    exp.Text = "-Details";
                    gvPending.Rows[index].Font.Bold = true;
                }
            }
            else
            {
                if (exp.Text.Equals("-Details"))
                {
                    exp.Text = "+Details";
                    gvPending.Rows[index].Font.Bold = false;
                }
                else if (exp.Text.Equals("+Details"))
                {
                    List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList =
                    ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID(campaignItemID, Master.UserSession.CurrentUser, false);
                    var result = (from src in blastList
                                  where src.TestBlast.ToLower() == "n"
                                  select src).ToList();



                    gvBlastDetails.DataSource = result;
                    gvBlastDetails.DataBind();

                    pnlBlastReport.Visible = true;
                    exp.Text = "-Details";
                    gvPending.Rows[index].Font.Bold = true;
                    if (blastList.Count == 0)
                    {
                        pnlBlastReport.Visible = false;
                    }
                }
            }
        }
        #endregion          

        #region ActiveGrid Events
        protected void gvActive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvActive.PageIndex = e.NewPageIndex;
            }
            gvActive.DataBind();
            loadActiveCampaignItems();
            DoTwemojiOnGridHandler(sender, e);
        }
        #endregion

        public void gvBlastDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Adding try-catch for re-usability of the function for gvPending and gvSent
                try
                {
                    if (e.Row.RowIndex >= 0)
                    {
                        Label lblBlastID = (Label)e.Row.FindControl("lblBlastID");
                        ImageButton btnEmailPreview = (ImageButton)e.Row.FindControl("btnEmailPreview");
                        btnEmailPreview.OnClientClick = "javascript:window.open('../blasts/BlastEmailPreview.aspx?blastID=" + lblBlastID.Text + "','BlastEmailPreview','resizable=yes,scrollbars=yes,menubar=yes,titlebar=no,toolbar=no'); return false;";
                    }
                }
                catch { }
            }
        }

        private void DeleteCampaignItem(int campaignItemID)
        {
            try
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(campaignItemID, Master.UserSession.CurrentUser, false);
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.Delete(ci.CampaignID.Value, ci.CampaignItemID, Master.UserSession.CurrentUser);
                loadGrids();

            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        private void CancelCampaignItem(int campaignItemID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.Cancel(campaignItemID, Master.UserSession.CurrentUser, "cancel");
                loadGrids();

            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        #region CREATEBLASTS
        protected void lnkRegular_Click(object sender, EventArgs e)
        {
            hfRedirectURL.Value = "/ecn.communicator/main/ecnwizard/wizardsetup.aspx?campaignItemType=regular";
            loadCampaignItemTemplates();
            modalPopupTemplates.Show();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void lnkAB_Click(object sender, EventArgs e)
        {
            hfRedirectURL.Value = "/ecn.communicator/main/ecnwizard/wizardsetup.aspx?campaignItemType=ab";
            loadCampaignItemTemplates();
            modalPopupTemplates.Show();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void lnkChampion_Click(object sender, EventArgs e)
        {
            hfRedirectURL.Value = "/ecn.communicator/main/ecnwizard/wizardsetup.aspx?campaignItemType=champion";
            loadCampaignItemTemplates();
            modalPopupTemplates.Show();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void lnkCalendar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/blasts/BlastCalendarView.aspx");
        }

        protected void lnkQuickTestBlast_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ecn.communicator/main/ecnwizard/quicktestblast.aspx");
        }
        #endregion

        protected void btnTemplates_Cancel(object sender, EventArgs e)
        {
            rblTemplate.ClearSelection();
            rblTemplate.SelectedValue = "No";
            pnlTemplate.Visible = false;
            modalPopupTemplates.Hide();
            update1.Update();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void btnTemplates_Submit(object sender, EventArgs e)
        {
            string campaignItemTempalate;
            if (rblTemplate.SelectedValue.Equals("No"))
            {
                campaignItemTempalate = string.Empty;
                Response.Redirect(hfRedirectURL.Value + campaignItemTempalate);
            }
            else
            {
                if (!drpCampaignItemTemplate.SelectedValue.Equals("-Select-"))
                {
                    campaignItemTempalate = "&CampaignItemTemplateID=" + drpCampaignItemTemplate.SelectedValue;
                    Response.Redirect(hfRedirectURL.Value + campaignItemTempalate);
                }
            }

        }

        protected void rblTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTemplate.SelectedValue.Equals("No"))
            {
                pnlTemplate.Visible = false;
            }
            else if (rblTemplate.SelectedValue.Equals("Yes"))
            {
                pnlTemplate.Visible = true;
            }
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void lnkNewTemplate_Click(object sender, EventArgs e)
        {

            addTemplate1.LoadControl();
            modalPopupAddTemplates.Show();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void addTemplateSave_Click(object sender, EventArgs e)
        {
            if (addTemplate1.Save() > 0)
            {
                addTemplate1.Reset();
                loadCampaignItemTemplates();
                modalPopupAddTemplates.Hide();
            }
            else
            {
                modalPopupAddTemplates.Show();
            }
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void btnAddTemplateClose_Click(object sender, EventArgs e)
        {
            addTemplate1.Reset();
            modalPopupAddTemplates.Hide();
            update1.Update();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected string EvalWithMaxLength(string fieldName, int maxLength)
        {
            object value = this.Eval(fieldName);
            if (value == null)
                return null;
            string str = value.ToString();
            if (str.Length > maxLength)
                return str.Substring(0, maxLength - 3) + "...";
            else
                return str;
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            //~/main/ecnwizard/wizardsetup.aspx?CampaignItemID={0}&campaignItemType={1}

            ImageButton imgbtnEdit = (ImageButton)sender;
            int campaignItemID = Convert.ToInt32(imgbtnEdit.CommandArgument);
            string campaignItemType = imgbtnEdit.CommandName;
            string link = string.Format("~/main/ecnwizard/wizardsetup.aspx?CampaignItemID={0}&campaignItemType={1}", campaignItemID, campaignItemType);
            if (ECN_Framework_BusinessLayer.Communicator.MarketingAutomation.CheckIfControlExists(campaignItemID, ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType.CampaignItem).Count > 0)
            {
                mpeMACheck.Show();
                btnMACheckContinue.CommandName = campaignItemType;
                btnMACheckContinue.CommandArgument = campaignItemID.ToString();
                DoTwemojiOnGridHandler(sender, e);
            }
            else
            {
                if (campaignItemType.ToLower().Equals("ab"))
                {
                    //show popup
                    btnOkEditAB.CommandArgument = campaignItemID.ToString();
                    btnOkEditAB.CommandName = campaignItemType;
                    mpeEditAB.Show();
                    DoTwemojiOnGridHandler(sender, e);
                }
                else
                {
                    Response.Redirect(link);
                }
            }

        }

        protected void btnCancelCancel_Click(object sender, EventArgs e)
        {
            mpeCancel.Hide();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void btnOKCancel_Click(object sender, EventArgs e)
        {

            int val = Convert.ToInt32(btnOKCancel.CommandArgument.ToString());
            string campaignItemType = btnOKCancel.CommandName.ToString().ToLower();
            if (campaignItemType.Equals("ab"))
            {

                //Find Champion and cancel that as well
                ECN_Framework_Entities.Communicator.CampaignItem ciAB = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(val, false);

                List<ECN_Framework_Entities.Communicator.BlastAbstract> sampleBlasts = ECN_Framework_BusinessLayer.Communicator.Blast.GetBySampleID_NoAccessCheck(ciAB.SampleID.Value, false);
                if (sampleBlasts.Any(x => x.BlastType.ToLower().Equals("champion") && x.StatusCode.ToLower().Equals("pending")))
                {
                    ECN_Framework_Entities.Communicator.BlastAbstract baChamp = sampleBlasts.FirstOrDefault(x => x.BlastType.ToLower().Equals("champion") && x.StatusCode.ToLower().Equals("pending"));
                    ECN_Framework_Entities.Communicator.CampaignItemBlast cibChamp = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID_NoAccessCheck(baChamp.BlastID, false);
                    DeleteCampaignItem(cibChamp.CampaignItemID.Value);
                }
                CancelCampaignItem(val);

            }
            else
            {
                CancelCampaignItem(val);
            }
            mpeCancel.Hide();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void btnOkEditAB_Click(object sender, EventArgs e)
        {
            //delete champion if it exists
            int ABcampaignItemID = Convert.ToInt32(btnOkEditAB.CommandArgument.ToString());
            string campaignItemType = btnOkEditAB.CommandName.ToString();
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(ABcampaignItemID, false);
            List<ECN_Framework_Entities.Communicator.BlastAbstract> sampleblasts = ECN_Framework_BusinessLayer.Communicator.Blast.GetBySampleID_NoAccessCheck(ci.SampleID.Value, false);
            if (sampleblasts.Any(x => x.BlastType.ToLower().Equals("champion") && x.StatusCode.ToLower().Equals("pending")))
            {
                try
                {

                    ECN_Framework_Entities.Communicator.BlastAbstract champ = sampleblasts.FirstOrDefault(x => x.BlastType.ToLower().Equals("champion") && x.StatusCode.ToLower().Equals("pending"));

                    ECN_Framework_Entities.Communicator.CampaignItemBlast champCIB = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID_NoAccessCheck(champ.BlastID, false);
                    DeleteCampaignItem(champCIB.CampaignItemID.Value);


                }
                catch (ECN_Framework_Common.Objects.ECNException ecn)
                {
                    setECNError(ecn);
                }

            }
            string link = string.Format("~/main/ecnwizard/wizardsetup.aspx?CampaignItemID={0}&campaignItemType={1}", ABcampaignItemID, campaignItemType);

            Response.Redirect(link);
        }

        protected void btnCancelEditAB_Click(object sender, EventArgs e)
        {
            mpeEditAB.Hide();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void CancelCampaignItemBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtnCancel = (ImageButton)sender;
            int campaignItemID = Convert.ToInt32(imgbtnCancel.CommandArgument.ToString());
            string campaignItemType = imgbtnCancel.CommandName.ToString().ToLower();
            btnOKCancel.CommandArgument = campaignItemID.ToString();
            btnOKCancel.CommandName = campaignItemType;

            if (campaignItemType.Equals("ab"))
            {
                lblCancelMessage.Text = "Are you sure you want to cancel this CampaignItem? If there is a Champion blast already set up for this AB, it will be automatically deleted.";
            }
            else
            {
                lblCancelMessage.Text = "Are you sure you want to cancel this CampaignItem?";
            }
            pnlCancel.Update();
            mpeCancel.Show();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void DeleteCampaignItemBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtnDelete = (ImageButton)sender;
            int campaignItemID = Convert.ToInt32(imgbtnDelete.CommandArgument.ToString());
            string campaignItemType = imgbtnDelete.CommandName.ToString().ToLower();
            btnDeleteOK.CommandArgument = campaignItemID.ToString();
            btnDeleteOK.CommandName = campaignItemType;


            if (campaignItemType.Equals("ab"))
            {
                lblDeleteMessage.Text = "Are you sure you want to delete this CampaignItem? If there is a Champion blast already set up for this AB, it will be automatically deleted.";
            }
            else
            {
                lblDeleteMessage.Text = "Are you sure you want to delete this CampaignItem?";
            }
            pnlDelete.Update();
            mpeDelete.Show();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void btnDeleteCancel_Click(object sender, EventArgs e)
        {
            mpeDelete.Hide();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void btnDeleteOK_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(btnDeleteOK.CommandArgument.ToString());
            string campaignItemType = btnDeleteOK.CommandName.ToString().ToLower();

            if (campaignItemType.Equals("ab"))
            {

                //Find Champion and cancel that as well
                ECN_Framework_Entities.Communicator.CampaignItem ciAB = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(val, false);

                List<ECN_Framework_Entities.Communicator.BlastAbstract> sampleBlasts = ECN_Framework_BusinessLayer.Communicator.Blast.GetBySampleID_NoAccessCheck(ciAB.SampleID.Value, false);
                if (sampleBlasts.Any(x => x.BlastType.ToLower().Equals("champion") && x.StatusCode.ToLower().Equals("pending")))
                {
                    ECN_Framework_Entities.Communicator.BlastAbstract baChamp = sampleBlasts.FirstOrDefault(x => x.BlastType.ToLower().Equals("champion") && x.StatusCode.ToLower().Equals("pending"));
                    ECN_Framework_Entities.Communicator.CampaignItemBlast cibChamp = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID_NoAccessCheck(baChamp.BlastID, false);
                    DeleteCampaignItem(cibChamp.CampaignItemID.Value);
                }
                DeleteCampaignItem(val);
            }
            else
            {
                DeleteCampaignItem(val);
            }
            mpeDelete.Hide();
            DoTwemojiOnGridHandler(sender, e);
        }

        protected void TabContainer_ActiveTabChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "pageloaded();", true);
        }

        protected void btnMACheckContinue_Click(object sender, EventArgs e)
        {
            Button btnMACheck = (Button)sender;
            int campaignItemID = Convert.ToInt32(btnMACheck.CommandArgument);
            string campaignItemType = btnMACheck.CommandName;
            string link = string.Format("~/main/ecnwizard/wizardsetup.aspx?CampaignItemID={0}&campaignItemType={1}", campaignItemID, campaignItemType);
            if (campaignItemType.ToLower().Equals("ab"))
            {
                //show popup
                btnOkEditAB.CommandArgument = campaignItemID.ToString();
                btnOkEditAB.CommandName = campaignItemType;
                mpeEditAB.Show();
                DoTwemojiOnGridHandler(sender, e);
            }
            else
            {
                Response.Redirect(link);
            }
        }

        protected void btnMACheckCancel_Click(object sender, EventArgs e)
        {
            btnMACheckContinue.CommandName = "";
            btnMACheckContinue.CommandArgument = "";
            mpeMACheck.Hide();
            DoTwemojiOnGridHandler(sender, e);
        }
    }
}