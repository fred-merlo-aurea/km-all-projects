using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using System.Configuration;
using System.Transactions;
using System.Data;

namespace ecn.communicator.main.blasts
{
    public partial class ManageCampaigns : System.Web.UI.Page
    {
        int RecordCount = 0;
        protected void btnNext_Click(object sender, EventArgs e)
        {
            GridViewRow pagerRow = gvCampaigns.BottomPagerRow;
            Label lblTotalPages = (Label)pagerRow.FindControl("lblTotalNumberOfPages");
            if (lblTotalPages != null)
            {
                if (gvCampaigns.PageIndex + 1 > int.Parse(lblTotalPages.Text)) { }
                else gvCampaigns.PageIndex++;
            }
            LoadGrid();

        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (gvCampaigns.PageIndex-1 < 0) { }
            else gvCampaigns.PageIndex--;
            LoadGrid();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.SubMenu = "manage campaigns";
            Master.Heading = "Blasts/Reporting > Manage Campaigns";
            Master.HelpContent = "";
            Master.HelpTitle = "";
            if (!Page.IsPostBack)
            {
                LoadGrid();

            }
        }

        private void LoadGrid()
        {
            DataTable dtCampaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.GetCampaignDetailsForManageCampaigns(Master.UserSession.CurrentCustomer.CustomerID, txtCampaignNameSearch.Text, ddlArchive.SelectedValue);

            RecordCount = dtCampaigns.Rows.Count;

            gvCampaigns.DataSource = dtCampaigns.DefaultView;
            gvCampaigns.DataBind();

            upGrid.Update();
        }

        protected void gvCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.gvCampaigns.PageSize = int.Parse(dropDown.SelectedValue);
            LoadGrid();
        }
        public void GoToPage_TextChanged(object sender, EventArgs e)
        {
            GridViewRow pagerRow = gvCampaigns.BottomPagerRow;
            TextBox page = (TextBox)pagerRow.Cells[0].FindControl("txtGoToPage");
            gvCampaigns.PageIndex = Convert.ToInt32(page.Text)-1;
            LoadGrid();
        }

        //protected void gvCampaigns_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvCampaigns.PageIndex = e.NewPageIndex;
        //    LoadGrid();
        //}
        public bool IsPageOutOfBounds(int pageSize, int gridPageIndex)
        {
            GridViewRow pagerRow = gvCampaigns.BottomPagerRow;
            Label lblTotalRecords = (Label)pagerRow.FindControl("lblTotalRecords");
            if (int.Parse(lblTotalRecords.Text) <= (pageSize * gridPageIndex))
            {
                return true;
            }
            return false;
        }

        protected void gvCampaigns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecords = (Label)e.Row.FindControl("lblTotalRecords");
                lblTotalRecords.Text = RecordCount.ToString();

                Label lblTotalNumberOfPages = (Label)e.Row.FindControl("lblTotalNumberOfPages");
                lblTotalNumberOfPages.Text = gvCampaigns.PageCount.ToString();

                TextBox txtGoToPage = (TextBox)e.Row.FindControl("txtGoToPage");
                txtGoToPage.Text = (gvCampaigns.PageIndex + 1).ToString();

                DropDownList ddlPageSize = (DropDownList)e.Row.FindControl("ddlPageSize");
                ddlPageSize.SelectedValue = gvCampaigns.PageSize.ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
                
                LinkButton lbPending = (LinkButton)e.Row.FindControl("lbPending");
                LinkButton lbSent = (LinkButton)e.Row.FindControl("lbSent");
                LinkButton lbSaved = (LinkButton)e.Row.FindControl("lbSaved");
                LinkButton imgbtnRename = (LinkButton)e.Row.FindControl("lbEditCampaignName");
                LinkButton imgbtnDelete = (LinkButton)e.Row.FindControl("lbDeleteCampaign");
                CheckBox chkArchive = (CheckBox)e.Row.FindControl("chkArchive");
                System.Web.UI.HtmlControls.HtmlControl liDelete = (System.Web.UI.HtmlControls.HtmlControl)e.Row.FindControl("liDeleteCampaign");

                lbPending.Text = drvCurrent["CIPending"].ToString();
                lbSent.Text = drvCurrent["CISent"].ToString();
                lbSaved.Text = drvCurrent["CISaved"].ToString();
                imgbtnRename.CommandArgument = gvCampaigns.DataKeys[e.Row.RowIndex].Value.ToString();
                chkArchive.Attributes.Add("CID", gvCampaigns.DataKeys[e.Row.RowIndex].Value.ToString());
                chkArchive.Checked = Convert.ToBoolean(drvCurrent["IsArchived"].ToString());
                lbPending.CommandArgument = gvCampaigns.DataKeys[e.Row.RowIndex].Value.ToString();
                lbSent.CommandArgument = gvCampaigns.DataKeys[e.Row.RowIndex].Value.ToString();
                lbSaved.CommandArgument = gvCampaigns.DataKeys[e.Row.RowIndex].Value.ToString();

                if (drvCurrent["CIPending"].ToString().Equals("0"))
                    lbPending.Enabled = false;

                if (drvCurrent["CISent"].ToString().Equals("0"))
                    lbSent.Enabled = false;

                if (drvCurrent["CISaved"].ToString().Equals("0"))
                    lbSaved.Enabled = false;

                if (drvCurrent["CISent"].ToString().Equals("0") && drvCurrent["CIPending"].ToString().Equals("0") && drvCurrent["CISaved"].ToString().Equals("0"))
                {
                    liDelete.Visible = true;
                    imgbtnDelete.CommandArgument = gvCampaigns.DataKeys[e.Row.RowIndex].Value.ToString();
                }
                else
                    liDelete.Visible = false;

                

               
            }
        }

        protected void gvCampaigns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deletecampaign"))
            {
                if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                {
                    List<ECN_Framework_Entities.Communicator.CampaignItem> listCI = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignID(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
                    if (listCI.Count(x => x.IsDeleted == false) > 0)
                    {
                        foreach (ECN_Framework_Entities.Communicator.CampaignItem ci in listCI)
                        {
                            List<ECN_Framework_Entities.Communicator.CampaignItemBlast> listCIB = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(ci.CampaignItemID, Master.UserSession.CurrentUser, true);
                            if (listCIB.Count(x => x.BlastID.HasValue == true) > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cib in listCIB)
                                {
                                    if (cib.Blast != null && (!cib.Blast.StatusCode.ToLower().Equals("cancelled") || !cib.Blast.StatusCode.ToLower().Equals("deleted")))
                                    {
                                        throwECNException("Cannot delete Campaign because it contains Campaign Items");
                                        return;
                                    }
                                }
                            }
                        }
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.Campaign.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                            LoadGrid();
                        }
                        catch (ECNException ecn)
                        {
                            setECNError(ecn);
                            return;
                        }
                    }
                    else if(listCI.Count == 0 || listCI.Count(x => x.IsDeleted == true) == listCI.Count)
                    {
                        try
                        {
                            ECN_Framework_BusinessLayer.Communicator.Campaign.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                            LoadGrid();
                        }
                        catch (ECNException ecn)
                        {
                            setECNError(ecn);
                            return;
                        }
                    }
                }
                else
                {
                    throwECNException("You do not have permission to delete campaigns");
                    return;
                }
            }
            else if (e.CommandName.ToLower().Equals("editcampaign"))
            {
                ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
                txtName.Text = c.CampaignName;             
                btnSaveCampaign.CommandArgument = c.CampaignID.ToString();
                mpeEditCampaign.Show();
            }
            else if (e.CommandName.ToLower().Equals("move"))
            {
                List<ECN_Framework_Entities.Communicator.Campaign> listCampaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser, false).Where(x => x.CampaignID != Convert.ToInt32(e.CommandArgument.ToString())).OrderBy(x => x.CampaignName).ToList();
                ddlCampaigns.DataSource = listCampaigns;
                ddlCampaigns.DataTextField = "CampaignName";
                ddlCampaigns.DataValueField = "CampaignID";
                ddlCampaigns.DataBind();

                btnMoveCampaign.CommandArgument = e.CommandArgument.ToString();

                mpeMove.Show();
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Campaign, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnSaveCampaign_Click(object sender, EventArgs e)
        {
            int campaignID = -1;
            int.TryParse(btnSaveCampaign.CommandArgument.ToString(), out campaignID);

            try
            {
                if (campaignID > 0)
                {
                    ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(campaignID, Master.UserSession.CurrentUser, false);
                    c.CampaignName = txtName.Text.Trim();
                    
                    c.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    c.UpdatedDate = DateTime.Now;
                    ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, Master.UserSession.CurrentUser);

                    LoadGrid();
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                return;
            }
            catch (Exception ecx)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ecx, "btnSaveCampaign_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                throwECNException("An error has occurred");
                return;
            }
        }

        protected void btnMoveCampaign_Click(object sender, EventArgs e)
        {
            int CampaignID = -1;
            int.TryParse(btnMoveCampaign.CommandArgument.ToString(), out CampaignID);

            if (CampaignID > 0)
            {
                List<ECN_Framework_Entities.Communicator.CampaignItem> listCI = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignID(CampaignID, Master.UserSession.CurrentUser, false);

                if (listCI.Count > 0)
                {
                    try
                    {
                        int newCampaignID = -1;
                        int.TryParse(ddlCampaigns.SelectedValue.ToString(), out newCampaignID);
                        using (TransactionScope scope = new TransactionScope())
                        {
                            if (newCampaignID > 0)
                            {
                                foreach (ECN_Framework_Entities.Communicator.CampaignItem ci in listCI)
                                {
                                    ci.CampaignID = newCampaignID;
                                    ci.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.Move(ci, Master.UserSession.CurrentUser);
                                }
                            }
                            scope.Complete();
                        }
                        LoadGrid();
                    }
                    catch (ECNException ecn)
                    {
                        setECNError(ecn);
                        return;
                    }
                    catch (Exception ex)
                    {
                        throwECNException("An error has occurred");
                        KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "btnMoveCampaign_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                        mpeMove.Hide();
                        return;
                    }

                }
                else
                {

                }
            }
        }

        protected void ddlArchive_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvCampaigns.PageIndex = 0;
            LoadGrid();
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtCampaignNameSearch.Text = string.Empty;
            gvCampaigns.PageIndex = 0;
            LoadGrid();
        }

        protected void chkArchive_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox ArchiveCHK = (CheckBox)sender;
            int CampaignID = Convert.ToInt32(ArchiveCHK.Attributes["CID"].ToString());

            ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(CampaignID, Master.UserSession.CurrentUser, false);
            c.IsArchived = ArchiveCHK.Checked;
            c.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            c.UpdatedDate = DateTime.Now;
            ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, Master.UserSession.CurrentUser);
            LoadGrid();
        }

        protected void btnEditCampaignName_Click(object sender, EventArgs e)
        {
            
            LinkButton lbEditButton = (LinkButton)sender;
            ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(Convert.ToInt32(lbEditButton.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
            txtName.Text = c.CampaignName;
            btnSaveCampaign.CommandArgument = c.CampaignID.ToString();
            mpeEditCampaign.Show();
        }

        protected void btnDeleteCampaign_Click(object sender, EventArgs e)
        {
            LinkButton lbDeleteButton = (LinkButton)sender;
            btnDeleteConfirm.CommandArgument = lbDeleteButton.CommandArgument.ToString();
            mpeDelete.Show();
        }

        protected void lbPending_Click(object sender, EventArgs e)
        {
            LinkButton lbPending = (LinkButton)sender;
            //redirect to new campaignitem managment page
            string url = "/ecn.communicator/main/blasts/ManageCampaignItems.aspx?type=pending&cid=" + lbPending.CommandArgument.ToString();
            Response.Redirect(url);
        }

        protected void lbSent_Click(object sender, EventArgs e)
        {
            //redirect to new campaignitem managment page

            LinkButton lbSent = (LinkButton)sender;
            //redirect to new campaignitem managment page
            string url = "/ecn.communicator/main/blasts/ManageCampaignItems.aspx?type=sent&cid=" + lbSent.CommandArgument.ToString();
            Response.Redirect(url);
        }


        protected void btnDeleteConfirm_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            {
                List<ECN_Framework_Entities.Communicator.CampaignItem> listCI = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignID(Convert.ToInt32(btnDeleteConfirm.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
                if (listCI.Count(x => x.IsDeleted == false) > 0)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItem ci in listCI)
                    {
                        List<ECN_Framework_Entities.Communicator.CampaignItemBlast> listCIB = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByCampaignItemID(ci.CampaignItemID, Master.UserSession.CurrentUser, true);
                        if (listCIB.Count(x => x.BlastID.HasValue == true) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast cib in listCIB)
                            {
                                if (cib.Blast != null && (!cib.Blast.StatusCode.ToLower().Equals("cancelled") || !cib.Blast.StatusCode.ToLower().Equals("deleted")))
                                {
                                    mpeDelete.Hide();
                                    throwECNException("Cannot delete Campaign because it contains Campaign Items");
                                    return;
                                }
                            }
                        }
                        List<ECN_Framework_Entities.Communicator.CampaignItemTestBlast> listCITB = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByCampaignItemID(ci.CampaignItemID, Master.UserSession.CurrentUser, true);
                        if(listCITB.Count( x=> x.BlastID.HasValue == true) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemTestBlast cib in listCITB)
                            {
                                if (cib.Blast != null && (!cib.Blast.StatusCode.ToLower().Equals("cancelled") || !cib.Blast.StatusCode.ToLower().Equals("deleted")))
                                {
                                    mpeDelete.Hide();
                                    throwECNException("Cannot delete Campaign because it contains Campaign Items");
                                    return;
                                }
                            }
                        }

                    }
                    try
                    {

                        ECN_Framework_BusinessLayer.Communicator.Campaign.Delete(Convert.ToInt32(btnDeleteConfirm.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                        
                        LoadGrid();
                        mpeDelete.Hide();
                    }
                    catch (ECNException ecn)
                    {
                        setECNError(ecn);
                        mpeDelete.Hide();
                        return;
                    }
                }
                else if (listCI.Count == 0 || listCI.Count(x => x.IsDeleted == true) == listCI.Count)
                {
                    try
                    {
                        ECN_Framework_BusinessLayer.Communicator.Campaign.Delete(Convert.ToInt32(btnDeleteConfirm.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                        LoadGrid();
                        mpeDelete.Hide();
                    }
                    catch (ECNException ecn)
                    {
                        setECNError(ecn);
                        mpeDelete.Hide();
                        return;
                    }
                }
            }
            else
            {
                throwECNException("You do not have permission to delete campaigns");
                mpeDelete.Hide();
                return;
            }
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            mpeDelete.Hide();
        }

        protected void lbSaved_Click(object sender, EventArgs e)
        {
            //redirect to new campaignitem managment page

            LinkButton lbSaved = (LinkButton)sender;
            //redirect to new campaignitem managment page
            string url = "/ecn.communicator/main/blasts/ManageCampaignItems.aspx?type=saved&cid=" + lbSaved.CommandArgument.ToString();
            Response.Redirect(url);
        }

    }
}