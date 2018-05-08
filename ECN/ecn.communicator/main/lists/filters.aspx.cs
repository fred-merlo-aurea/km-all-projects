using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager
{
    public partial class filtersplus : ECN_Framework.WebPageHelper
    {

        int recordCount = 0;

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
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "";
            Master.Heading = "Groups > Manage Groups > Smart Forms > Filters";
            Master.HelpContent = "<B>Adding/Editing Filters</B><div id='par1'><ul><li>Find the Group you want to create a filter for.</li><li>Click on the <em>Funnel</em> icon for that group.</li><li>Enter a title for your filter (for example, pet owners)</li><li>Click <em>Create new filter</em>.</li><li>Under filter names, click on the <em>pencil (Add/Edit Filter Attributes)</em> icon to define the filter attributes.</li><li>In the Compare Field section, use the drop down menu and click on profile field to define attributes of your filter.</li><li>In the Comparator section you have the option of making the field equal to (=), contains, ends with, or starts with.</li><li>In the Compare Value field, enter the information you would want the system to filter (for example, dog).</li><li>The Join Filters allow you to select And, or, Or.</li><li>To add, click <em>Add this Filter</em>.</li><li>Repeat this process several times to fully develop the attributes you are looking for (for example, dog, dogs, cat, cats, dog owners, etc.)</li><li>After all fields and attributes have been selected and added, click <em>Preview filtered e-mails</em> button to view emails in your filtered list.</li><li>When filter is complete, Click on <em>Return to Filters List</em>.</li></ul></div>";
            Master.HelpTitle = "Filters Manager";

            if(!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.Edit))
                {
                    btnAddFilter.Visible = true;
                    btnCopyFilter.Visible = true;
                    FilterGrid.Columns[2].Visible = true;
                    FilterGrid.Columns[3].Visible = true;
                }
                else
                {
                    btnAddFilter.Visible = false;
                    btnCopyFilter.Visible = false;
                    FilterGrid.Columns[2].Visible = false;
                    FilterGrid.Columns[3].Visible = false;
                }
            }

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFilter, KMPlatform.Enums.Access.View))	
            {
                if (!IsPostBack)
                {
                    ViewState["SortField"] = "FilterName";
                    ViewState["SortDirection"] = "ASC";
                }
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(getGroupID(), Master.UserSession.CurrentUser);
                GroupNameDisplay.Text = group.GroupName;
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            loadFiltersGrid(getGroupID());
            //if(KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            if (KM.Platform.User.IsAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
            {
                FilterGrid.Columns[4].Visible = true;
            }
            else
            {
                FilterGrid.Columns[4].Visible = false;
            }
        }

        private void loadFiltersGrid(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.Filter> filterList = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(groupID, false, Master.UserSession.CurrentUser,ddlArchiveFilter.SelectedValue.ToString());
            recordCount = filterList.Count;

            DataView dv = CreateView(filterList);

            dv.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();

            FilterGrid.DataSource = dv;

            try
            {
                FilterGrid.DataBind();
            }
            catch
            {
                FilterGrid.PageIndex = 0;
                FilterGrid.DataBind();
            }
            FilterGrid.ShowEmptyTable = true;
            FilterGrid.EmptyTableRowText = "No filters to display";
        }

        private DataView CreateView(List<ECN_Framework_Entities.Communicator.Filter> filterList)
        {
            DataTable dt = new DataTable("Filters");

            DataColumn dc = dt.Columns.Add("FilterID", typeof(Int32));
            dc.AllowDBNull = false;
            dc.Unique = true;
            dt.Columns.Add("CreateDate", typeof(DateTime));
            dt.Columns.Add("FilterName", typeof(String));
            dt.Columns.Add("GroupID", typeof(Int32));
            dt.Columns.Add("Archived", typeof(bool));
            foreach (ECN_Framework_Entities.Communicator.Filter filter in filterList)
            {
                DataRow dr = dt.NewRow();
                dr["FilterID"] = filter.FilterID;
                dr["CreateDate"] = filter.CreatedDate;
                dr["FilterName"] = filter.FilterName;
                dr["GroupID"] = filter.GroupID;
                dr["Archived"] = filter.Archived.HasValue ? filter.Archived.Value : false;
                dt.Rows.Add(dr);
            }
            DataView dv = dt.DefaultView;

            return dv;
        }

        private int getGroupID()
        {
            if (Request.QueryString["GroupID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            }
            else
                return 0;
        }

        private int getFilterID()
        {
            if (Request.QueryString["FilterID"] != null)
            {
                return Convert.ToInt32(Request.QueryString["FilterID"].ToString());
            }
            else
                return 0;
        }

        protected void btnAddFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("filtersplusedit.aspx?GroupID=" + getGroupID().ToString());
        }

        protected void FilterGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDown = (DropDownList)sender;
            this.FilterGrid.PageSize = int.Parse(dropDown.SelectedValue);
        }

        protected void FilterGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                FilterGrid.PageIndex = e.NewPageIndex;
            }
            FilterGrid.DataBind();
        }

        protected void GoToPage_TextChanged(object sender, EventArgs e)
        {
            TextBox txtGoToPage = (TextBox)sender;

            int pageNumber;
            if (int.TryParse(txtGoToPage.Text.Trim(), out pageNumber) && pageNumber > 0 && pageNumber <= this.FilterGrid.PageCount)
            {
                this.FilterGrid.PageIndex = pageNumber - 1;
            }
            else
            {
                this.FilterGrid.PageIndex = 0;
            }
        }

        protected void FilterGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            loadFiltersGrid(getGroupID());
        }

        protected void FilterGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int filterID = Convert.ToInt32(FilterGrid.DataKeys[e.RowIndex].Values[0]);
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Filter.Delete(filterID, Master.UserSession.CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void FilterGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Label lblTotalRecords = (Label)e.Row.FindControl("lblTotalRecords");
                lblTotalRecords.Text = recordCount.ToString();

                Label lblTotalNumberOfPages = (Label)e.Row.FindControl("lblTotalNumberOfPages");
                lblTotalNumberOfPages.Text = FilterGrid.PageCount.ToString();

                TextBox txtGoToPage = (TextBox)e.Row.FindControl("txtGoToPage");
                txtGoToPage.Text = (FilterGrid.PageIndex + 1).ToString();

                DropDownList ddlPageSize = (DropDownList)e.Row.FindControl("ddlPageSize");
                ddlPageSize.SelectedValue = FilterGrid.PageSize.ToString();

                
            }
            else if(e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkIsArchived = (CheckBox)e.Row.FindControl("chkIsArchived");
                DataRowView drv = (DataRowView)e.Row.DataItem;

                chkIsArchived.Checked = drv["Archived"].ToString().ToLower().Equals("true") ? true : false;
                chkIsArchived.Attributes.Add("index", e.Row.RowIndex.ToString());
            }
        }

        public void FilterGrid_Command(Object sender, DataGridCommandEventArgs e)
        {
            int filterID = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "DELETE")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.Filter.Delete(filterID, Master.UserSession.CurrentUser);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }

        protected void btnCopyFilter_Click(object sender, EventArgs e)
        {
            drpSourceGroup.ClearSelection();
            gvFilters.DataSource = null;
            gvFilters.DataBind();

            List<ECN_Framework_Entities.Communicator.Group> groupList =
            ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            var result = (from src in groupList
                          orderby src.GroupName
                          select src).ToList();
            drpSourceGroup.DataSource = result;
            drpSourceGroup.DataTextField = "GroupName";
            drpSourceGroup.DataValueField = "GroupID";
            drpSourceGroup.DataBind();
            drpSourceGroup.Items.Insert(0, new ListItem("--- Select Group Name ---", " "));
            mpeCopyFilter.Show();
        }

        protected void drpSourceGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ECN_Framework_Entities.Communicator.Filter> sourceFilters = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(Convert.ToInt32(drpSourceGroup.SelectedValue.ToString()), true, Master.UserSession.CurrentUser);
            List<ECN_Framework_Entities.Communicator.Filter> destFilters = ECN_Framework_BusinessLayer.Communicator.Filter.GetByGroupID(Convert.ToInt32(Request.QueryString["GroupID"].ToString()), true, Master.UserSession.CurrentUser);

            List<ECN_Framework_Entities.Communicator.Filter> availableFilters = new List<ECN_Framework_Entities.Communicator.Filter>();

            foreach (ECN_Framework_Entities.Communicator.Filter f in sourceFilters)
            {
                if (destFilters.Count(x => x.FilterName.ToLower() == f.FilterName.ToLower()) == 0)
                {
                    availableFilters.Add(f);
                }
            }

            gvFilters.Visible = true;
            gvFilters.DataSource = availableFilters;
            gvFilters.DataBind();

            mpeCopyFilter.Show();
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            int GroupID = Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, Master.UserSession.CurrentUser);
            List<string> missingUDFs = new List<string>();
            List<ECN_Framework_Entities.Communicator.Filter> filtersToCopy = new List<ECN_Framework_Entities.Communicator.Filter>();

            foreach (GridViewRow gvr in gvFilters.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkCopy = (CheckBox)gvr.FindControl("chkCopyFilter");
                    if (chkCopy.Checked)
                    {
                        ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(Convert.ToInt32(gvFilters.DataKeys[gvr.RowIndex].Value.ToString()), Master.UserSession.CurrentUser);
                        filtersToCopy.Add(filter);
                        if (filter.WhereClause.Contains('['))
                        {
                            string[] shortnames = filter.WhereClause.Split('[');
                            //looping through where clause to find UDFs and verify they exist, if they don't building a list to display missing udfs to the user
                            foreach (string s in shortnames)
                            {
                                if (s.Contains(']'))
                                {
                                    string shortname = s.Substring(0, s.IndexOf(']'));

                                    if (!ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Exists(shortname, null, GroupID, Master.UserSession.CurrentCustomer.CustomerID) && !IsProfileField(shortname))
                                    {
                                        if (!missingUDFs.Contains(shortname))
                                            missingUDFs.Add(shortname);

                                    }
                                }
                            }
                        }

                    }
                }
            }
            if (missingUDFs.Count > 0)
            {
                StringBuilder sbUDFs = new StringBuilder();
                foreach (string s in missingUDFs)
                {
                    sbUDFs.Append(s + ",");
                }
                throwECNException("Please add the following UDFs to the group: " + sbUDFs.ToString().TrimEnd(','));
                mpeCopyFilter.Hide();
                return;
            }
            else
            {
                try
                {
                    foreach (ECN_Framework_Entities.Communicator.Filter filter in filtersToCopy)
                    {
                        //Done checking udfs, can copy the filter now
                        ECN_Framework_Entities.Communicator.Filter newFilter = new ECN_Framework_Entities.Communicator.Filter();
                        newFilter.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                        newFilter.CustomerID = Master.UserSession.CurrentCustomer.CustomerID;
                        newFilter.DynamicWhere = filter.DynamicWhere;
                        newFilter.FilterGroupList = filter.FilterGroupList;
                        newFilter.FilterName = filter.FilterName;
                        newFilter.GroupCompareType = filter.GroupCompareType;
                        newFilter.GroupID = GroupID;
                        newFilter.IsDeleted = false;
                        newFilter.WhereClause = filter.WhereClause;
                        foreach (ECN_Framework_Entities.Communicator.FilterGroup fg in filter.FilterGroupList)
                        {
                            fg.FilterGroupID = -1;
                            foreach (ECN_Framework_Entities.Communicator.FilterCondition fc in fg.FilterConditionList)
                            {
                                fc.FilterConditionID = -1;
                            }
                        }
                        ECN_Framework_BusinessLayer.Communicator.Filter.Save(newFilter, Master.UserSession.CurrentUser);
                    }

                }
                catch (ECNException ecn)
                {
                    setECNError(ecn);
                    return;
                }
            }
            loadFiltersGrid(GroupID);
        }

        private bool IsProfileField(string shortname)
        {
            if (FilterBase.CommonFiltersFields.Contains(shortname)
                || FilterBase.NonCommonFiltersFields.Contains(shortname))
            {
                return true;
            }

            return false;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Filter, Enums.Method.Validate, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void ddlArchiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadFiltersGrid(getGroupID());
        }

        protected void chkIsArchived_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkIsArchived = (CheckBox)sender;
            int index = Convert.ToInt32(chkIsArchived.Attributes["index"].ToString());
            int filterID = Convert.ToInt32(FilterGrid.DataKeys[index].Value.ToString());
            ECN_Framework_BusinessLayer.Communicator.Filter.ArchiveFilter(filterID, chkIsArchived.Checked, Master.UserSession.CurrentUser);
            loadFiltersGrid(getGroupID());
        }
    }
}
