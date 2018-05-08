using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ecn.communicator.main.blasts
{
    public partial class ManageCampaignItems : System.Web.UI.Page
    {
        private int CampaignID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["cid"].ToString());
            }
        }

        private string Status
        {
            get
            {
                return Request.QueryString["type"].ToString();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS;
            Master.SubMenu = "manage campaigns";
            Master.Heading = "Blasts/Reporting > Manage Campaigns > Manage Campaign Items";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!Page.IsPostBack)
            {
                ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(CampaignID, Master.UserSession.CurrentUser, false);
                if (c != null && c.CampaignID > 0)
                    lblCampaignName.Text = c.CampaignName;
                LoadGrid();
            }
        }

        private void DoTwemoji()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "doTwemoji", "pageloaded();", true);
        }

        #region loadingdata/searching

        private void LoadGrid()
        {
            DataTable dtCampaignItems = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetCampaignItemsForCampaignSearch(CampaignID, Status, txtCampaignItemName.Text, gvCampaignItems.PageSize, gvCampaignItems.PageIndex + 1);
            gvCampaignItems.DataSource = dtCampaignItems;
            gvCampaignItems.DataBind();

            int rows = dtCampaignItems.Rows.Count;

            if(dtCampaignItems.Rows.Count > 0)
            {
                rows = Convert.ToInt32(dtCampaignItems.Rows[0]["TotalCount"].ToString());
            }

            var exactPageCount = (double)rows / (double)gvCampaignItems.PageSize;
            var roundUpPageCount = Math.Ceiling((double)exactPageCount);


            lblTotalNumberOfPages.Text = roundUpPageCount.ToString();
            ddlPageSize.SelectedValue = gvCampaignItems.PageSize.ToString();
            txtGoToPage.Text = (gvCampaignItems.PageIndex + 1).ToString();
            lblTotalRecords.Text = rows.ToString();

            pnlGrid.Update();
            DoTwemoji();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            LoadGrid();
            
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtCampaignItemName.Text = string.Empty;
            LoadGrid();
            
        }

        #endregion

        protected void lbEditCampaignItemName_Click(object sender, EventArgs e)
        {
            LinkButton lbEditName = (LinkButton)sender;
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(Convert.ToInt32(lbEditName.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);

            txtName.Text = ci.CampaignItemName;
            btnSaveCampaign.CommandArgument = ci.CampaignItemID.ToString();
            mpeEditCampaignItem.Show();
            upEditCampaignItem.Update();
            DoTwemoji();
        }

        protected void lbMoveCampaignItem_Click(object sender, EventArgs e)
        {
            LinkButton lbMove = (LinkButton)sender;

            btnMoveCampaigmItem.CommandArgument = lbMove.CommandArgument;
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(Convert.ToInt32(lbMove.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
            List<ECN_Framework_Entities.Communicator.Campaign> listC = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser, false);
            ddlCampaigns.DataSource = listC.OrderBy(x => x.CampaignName);
            ddlCampaigns.DataTextField = "CampaignName";
            ddlCampaigns.DataValueField = "CampaignID";
            ddlCampaigns.DataBind();

            ddlCampaigns.SelectedValue = ci.CampaignID.Value.ToString();

            mpeMoveCampaignItem.Show();
            upMoveCampaignItem.Update();
            DoTwemoji();
        }

        #region Paging

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (gvCampaignItems.PageIndex > 0)
            {
                gvCampaignItems.PageIndex--;
                LoadGrid();
            }

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (gvCampaignItems.PageIndex + 1 < Convert.ToInt32(lblTotalNumberOfPages.Text))
            {
                gvCampaignItems.PageIndex++;
                LoadGrid();
            }
        }

        protected void txtGoToPage_TextChanged(object sender, EventArgs e)
        {
            int newPage = Convert.ToInt32(txtGoToPage.Text);
            if (newPage > 0 && newPage <= Convert.ToInt32(lblTotalNumberOfPages.Text))
            {
                gvCampaignItems.PageIndex = newPage - 1;
                LoadGrid();
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCampaignItems.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue.ToString());
            LoadGrid();
        }

        #endregion

        protected void gvCampaignItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton lbMove = (LinkButton)e.Row.FindControl("lbMoveCampaignItem");
                LinkButton lbEditCampaignItemName = (LinkButton)e.Row.FindControl("lbEditCampaignItemName");

                lbMove.CommandArgument = drv["CampaignItemID"].ToString();
                lbEditCampaignItemName.CommandArgument = drv["CampaignItemID"].ToString();

                System.Web.UI.HtmlControls.HtmlControl liMove = (System.Web.UI.HtmlControls.HtmlControl)e.Row.FindControl("liMove");
                if(Status.ToLower().Equals("pending"))
                {
                    liMove.Visible = false;
                }
            }
        }

        protected void btnMoveCampaigmItem_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(Convert.ToInt32(btnMoveCampaigmItem.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
            ci.CampaignID = Convert.ToInt32(ddlCampaigns.SelectedValue.ToString());

            ci.UpdatedUserID = Master.UserSession.CurrentUser.UserID;

            try
            {
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.Move(ci, Master.UserSession.CurrentUser);
            }
            catch(ECN_Framework_Common.Objects.ECNException ecn)
            {
                setECNError(ecn);
                mpeMoveCampaignItem.Hide();
                return;
            }
            mpeMoveCampaignItem.Hide();
            LoadGrid();
        }

        protected void btnCancelMoveCampaignItem_Click(object sender, EventArgs e)
        {
            mpeMoveCampaignItem.Hide();
            DoTwemoji();
        }

        protected void btnSaveCampaign_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(Convert.ToInt32(btnSaveCampaign.CommandArgument.ToString()), Master.UserSession.CurrentUser, false);
            ci.CampaignItemName = txtName.Text.Trim();
            ci.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            try
            {
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.UpdateCampaignItemName(ci);
                mpeEditCampaignItem.Hide();
                LoadGrid();
            }
            catch(ECN_Framework_Common.Objects.ECNException ecn)
            {
                mpeEditCampaignItem.Hide();
                setECNError(ecn);
                DoTwemoji();
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            txtCampaignItemName.Text = string.Empty;
            mpeEditCampaignItem.Hide();
            DoTwemoji();
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
            uplMaster.Update();
        }

        private void throwECNException(string message)
        {
            ECN_Framework_Common.Objects.ECNError ecnError = new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Campaign, ECN_Framework_Common.Objects.Enums.Method.Save, message);
            List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECN_Framework_Common.Objects.ECNException(errorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.WebSite));
        }
    }
}