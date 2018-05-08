using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.lists
{
    public partial class domainTrackerUsers : System.Web.UI.Page
    {
        int SentRecordCount = 0;

        private int getDomainTrackerID()
        {
            if (Request.QueryString["domainTrackerID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["domainTrackerID"]);
            }
            else
                return -1;
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = ECN_Framework_BusinessLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(getDomainTrackerID(), Master.UserSession.CurrentUser);
                lblDomainName.Text= "User Profiles from "+ domainTracker.Domain;
                loadDomainTrackingUsers();
            }
        }

        #region DomainTracking UsersGrid Events
        protected void gvDomainTrackingUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecordsContent = (Label)e.Row.FindControl("gvDomainTrackingUsers_lblTotalRecords");
                lblTotalRecordsContent.Text = SentRecordCount.ToString();

                Label lblTotalNumberOfPagesContent = (Label)e.Row.FindControl("gvDomainTrackingUsers_lblTotalNumberOfPages");
                lblTotalNumberOfPagesContent.Text = gvDomainTrackingUsers.PageCount.ToString();

                TextBox txtGoToPageContent = (TextBox)e.Row.FindControl("gvDomainTrackingUsers_txtGoToPage");
                txtGoToPageContent.Text = (gvDomainTrackingUsers.PageIndex + 1).ToString();

                DropDownList ddlPageSizeContent = (DropDownList)e.Row.FindControl("gvDomainTrackingUsers_ddlPageSize");
                ddlPageSizeContent.SelectedValue = gvDomainTrackingUsers.PageSize.ToString();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton ProfileDetails = (LinkButton)e.Row.FindControl("lnkbtnProfile");
                ProfileDetails.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvDomainTrackingUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvDomainTrackingUsers.PageIndex = e.NewPageIndex;
            }
            gvDomainTrackingUsers.DataBind();
            loadDomainTrackingUsers();
        }

        protected void gvDomainTrackingUsers_RowCommand(Object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "ProfileDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                loadProfileDetails(index);
            }
        }              

        protected void gvDomainTrackingUsers_ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.gvDomainTrackingUsers.PageSize = int.Parse(dropDown.SelectedValue);
            loadDomainTrackingUsers();
        }

        protected void gvDomainTrackingUsers_txtGoToPage_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPageContent = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPageContent.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.gvDomainTrackingUsers.PageCount)
            {
                this.gvDomainTrackingUsers.PageIndex = pageNumber - 1;
            }
            else
            {
                this.gvDomainTrackingUsers.PageIndex = 0;
            }
            loadDomainTrackingUsers();
        }
        #endregion

        private void loadDomainTrackingUsers()
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerUserProfile> profileList= ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerUserProfile.GetByDomainTrackerID(getDomainTrackerID(),null,null, Master.UserSession.CurrentUser, null, null, null);
            gvDomainTrackingUsers.DataSource = profileList;
            SentRecordCount = profileList.Count;
            try
            {
                gvDomainTrackingUsers.DataBind();
            }
            catch
            {
                gvDomainTrackingUsers.PageIndex = 0;
                gvDomainTrackingUsers.DataBind();
            }
        }

        private void loadProfileDetails(int index)
        {
            Panel pnlProfileReport = gvDomainTrackingUsers.Rows[index].FindControl("pnlProfileReport") as Panel;
            AjaxControlToolkit.TabContainer tabContainer = gvDomainTrackingUsers.Rows[index].FindControl("TabContainer1") as AjaxControlToolkit.TabContainer;
            AjaxControlToolkit.TabPanel TabStandard = tabContainer.FindControl("TabStandard") as AjaxControlToolkit.TabPanel;
            AjaxControlToolkit.TabPanel TabAdditional = tabContainer.FindControl("TabAdditional") as AjaxControlToolkit.TabPanel;
            GridView gvStandardDataPoints = TabStandard.FindControl("gvStandardDataPoints") as GridView;
            GridView gvAdditionalDataPoints = TabAdditional.FindControl("gvAdditionalDataPoints") as GridView;
            int ProfileID = Convert.ToInt32(((Label)gvDomainTrackingUsers.Rows[index].FindControl("lblProfileID")).Text);
            LinkButton exp = gvDomainTrackingUsers.Rows[index].FindControl("lnkbtnProfile") as LinkButton;
            if (exp.Text.Equals("-Details"))
            {
                exp.Text = "+Details";
                gvDomainTrackingUsers.Rows[index].Font.Bold = false;
                pnlProfileReport.Visible = false;
            }
            else if (exp.Text.Equals("+Details"))
            {
                List<ECN_Framework_Entities.DomainTracker.DomainTrackerActivity> activityList = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerActivity.GetByProfileID(ProfileID, getDomainTrackerID(), Master.UserSession.CurrentUser);
                DataTable dt = ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerValue.GetByProfileID(ProfileID, getDomainTrackerID(), Master.UserSession.CurrentUser);
                pnlProfileReport.Visible = true;
                exp.Text = "-Details";
                gvDomainTrackingUsers.Rows[index].Font.Bold = true;
                gvStandardDataPoints.DataSource = activityList;
                gvStandardDataPoints.DataBind();
                gvAdditionalDataPoints.DataSource = dt;
                gvAdditionalDataPoints.DataBind();
            }
        }
    }
}