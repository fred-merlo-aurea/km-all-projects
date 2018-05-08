using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class filters : System.Web.UI.UserControl
    {
        private const string StringFieldType = "String";
        private const string DateFieldType = "Date";

        public int selectedGroupID
        {
            get
            {
                if (Request.QueryString["GroupID"] != null)
                    return Convert.ToInt32(Request.QueryString["GroupID"]);
                else if (ViewState["selectedGroupID"] != null)
                    return (int)ViewState["selectedGroupID"];
                else
                    return 0;
            }
            set
            {
                ViewState["selectedGroupID"] = value;
            }
        }

        public int selectedFilterID
        {
            get
            {
                if (Request.QueryString["FilterID"] != null)
                    return Convert.ToInt32(Request.QueryString["FilterID"]);
                else if (ViewState["selectedFilterID"] != null)
                    return (int)ViewState["selectedFilterID"];
                else
                    return 0;
            }
            set
            {
                ViewState["selectedFilterID"] = value;
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

        private void setECNError_FilterCondition(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError_FilterCondition.Visible = true;
            lblErrorMessage_FilterCondition.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage_FilterCondition.Text = lblErrorMessage_FilterCondition.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void setECNError_FilterGroup(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError_FilterGroup.Visible = true;
            lblErrorMessage_FilterGroup.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage_FilterGroup.Text = lblErrorMessage_FilterGroup.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            phError_FilterCondition.Visible = false;
            phError_FilterGroup.Visible = false;
            if (!IsPostBack)
            {
                loadData();
            }
            if (Request.QueryString["FilterID"] != null)
            {
                btnPreview.Attributes.Add("onclick", "window.open('../lists/filterPreview.aspx?filterID=" + selectedFilterID + "', 'Preview', 'width=550,height=500,resizable=yes,scrollbars=yes,status=yes');");

            }

        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["FilterID"] == null)
            {
                if (selectedFilterID > 0)
                {
                    string script = "window.open('../lists/filterPreview.aspx?filterID=" + selectedFilterID + "', 'Preview', 'width=550,height=500,resizable=yes,scrollbars=yes,status=yes');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "tooltip", script, true);
                }
                else
                {
                    lblErrorMessage.Text = "Please save a filter to preview";
                    phError.Visible = true;
                }
            }
        }

        public void loadData()
        {
            txtDatePicker.Attributes.Add("readonly", "true");
            LoadSessionObject();
            LoadUserFields();
            if (selectedFilterID > 0)
            {
                PopulateTree();
                tvFilter.FindNode(selectedFilterID.ToString()).Select();
                NodeChanged();
                SetupAddFilter(false);
            }
            else
            {
                SetupAddFilter(true);
                HidePanels();
            }
        }

        #region FilterPlusEdit

        private void LoadSessionObject()
        {
            ECN_Framework_Entities.Communicator.Filter filter = null;
            int filterID = selectedFilterID;
            if (filterID > 0)
            {
                filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
            Session["Filter"] = filter;
        }

        private void LoadUserFields()
        {
            ddlField.Items.Clear();


            ddlField.Items.Add(new ListItem("EmailAddress", "EmailAddress"));
            ddlField.Items.Add(new ListItem("FormatTypeCode", "FormatTypeCode"));
            ddlField.Items.Add(new ListItem("SubscribeTypeCode", "SubscribeTypeCode"));
            ddlField.Items.Add(new ListItem("Title", "Title"));
            ddlField.Items.Add(new ListItem("FirstName", "FirstName"));
            ddlField.Items.Add(new ListItem("LastName", "LastName"));
            ddlField.Items.Add(new ListItem("FullName", "FullName"));
            ddlField.Items.Add(new ListItem("Company", "Company"));
            ddlField.Items.Add(new ListItem("Occupation", "Occupation"));
            ddlField.Items.Add(new ListItem("Address", "Address"));
            ddlField.Items.Add(new ListItem("Address2", "Address2"));
            ddlField.Items.Add(new ListItem("City", "City"));
            ddlField.Items.Add(new ListItem("State", "State"));
            ddlField.Items.Add(new ListItem("Zip", "Zip"));
            ddlField.Items.Add(new ListItem("Country", "Country"));
            ddlField.Items.Add(new ListItem("Voice", "Voice"));
            ddlField.Items.Add(new ListItem("Mobile", "Mobile"));
            ddlField.Items.Add(new ListItem("Fax", "Fax"));
            ddlField.Items.Add(new ListItem("Website", "Website"));
            ddlField.Items.Add(new ListItem("Age", "Age"));
            ddlField.Items.Add(new ListItem("Income", "Income"));
            ddlField.Items.Add(new ListItem("Gender", "Gender"));
            ddlField.Items.Add(new ListItem("User1", "User1"));
            ddlField.Items.Add(new ListItem("User2", "User2"));
            ddlField.Items.Add(new ListItem("User3", "User3"));
            ddlField.Items.Add(new ListItem("User4", "User4"));
            ddlField.Items.Add(new ListItem("User5", "User5"));
            ddlField.Items.Add(new ListItem("User6", "User6"));
            ddlField.Items.Add(new ListItem("Birthdate [MM/DD/YYYY]", "Birthdate"));
            ddlField.Items.Add(new ListItem("UserEvent1", "UserEvent1"));
            ddlField.Items.Add(new ListItem("UserEvent1Date [MM/DD/YYYY]", "UserEvent1Date"));
            ddlField.Items.Add(new ListItem("UserEvent2", "UserEvent2"));
            ddlField.Items.Add(new ListItem("UserEvent2Date [MM/DD/YYYY]", "UserEvent2Date"));
            ddlField.Items.Add(new ListItem("Notes", "Notes"));
            ddlField.Items.Add(new ListItem("Profile Added [MM/DD/YYYY]", "CreatedOn"));
            ddlField.Items.Add(new ListItem("Profile Updated [MM/DD/YYYY]", "LastChanged"));
            int i = ddlField.Items.Count;
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(selectedGroupID);

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
            {
                ddlField.Items.Insert(i++, groupDataFields.ShortName);
            }

        }

        private void SetupAddFilter(bool add)
        {
            tvFilter.Visible = !add;
            lbAdd.Enabled = !add;
            if (add)
            {
                btnAddFilter.Text = "Add";
                txtFilterName.Text = "";
                ddlGroupCompareType.SelectedIndex = -1;
                ddlGroupCompareType.Items.FindByText("OR").Selected = true;
            }
            else
            {
                ECN_Framework_Entities.Communicator.Filter filter = (ECN_Framework_Entities.Communicator.Filter)Session["Filter"];
                if (filter != null)
                {
                    btnAddFilter.Text = "Update";
                    txtFilterName.Text = filter.FilterName;
                    ddlGroupCompareType.SelectedIndex = -1;
                    if (filter.GroupCompareType != "")
                    {
                        ddlGroupCompareType.Items.FindByText(filter.GroupCompareType).Selected = true;
                    }
                }
            }
        }

        private void SetupAddFilterGroup(int? filterGroupID)
        {
            ddlConditionCompareType.SelectedIndex = -1;
            if (filterGroupID == null)
            {
                btnAddFilterGroup.Text = "Add";
                txtFilterGroupName.Text = "";
                lblFilterGroupID.Text = "";
                ddlConditionCompareType.Items.FindByText("OR").Selected = true;
            }
            else
            {
                btnAddFilterGroup.Text = "Update";
                ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(Convert.ToInt32(filterGroupID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                lblFilterGroupID.Text = filterGroupID.ToString();
                txtFilterGroupName.Text = filterGroup.Name;
                ddlConditionCompareType.Items.FindByText(filterGroup.ConditionCompareType).Selected = true;
            }
        }

        private void SetupAddFilterCondition(int? filterConditionID, int filterGroupID)
        {
            ddlField.SelectedIndex = -1;
            ddlComparator.SelectedIndex = -1;
            ddlDatePart.SelectedIndex = -1;
            lblRefGroupID.Text = filterGroupID.ToString();
            if (filterConditionID == null)
            {
                btnAddFilterCondition.Text = "Add";
                gvFilterCondition.Visible = true;
                lblFilterConditionID.Text = "";
                txtCompareValue.Text = "";
                cbxNot.Checked = false;
                ddlField.Items[0].Selected = true;
                SetupFieldType(ddlField.SelectedValue);
                LoadComparators(ddlFieldType.SelectedValue);
                ddlComparator.Items[0].Selected = true;
                ddlDatePart.Items[0].Selected = true;
                SetupValueEntry(ddlFieldType.SelectedValue);
            }
            else
            {
                btnAddFilterCondition.Text = "Update";
                gvFilterCondition.Visible = true;
                ECN_Framework_Entities.Communicator.FilterCondition condition = ECN_Framework_BusinessLayer.Communicator.FilterCondition.GetByFilterConditionID(Convert.ToInt32(filterConditionID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                lblFilterConditionID.Text = filterConditionID.ToString();
                txtCompareValue.Text = condition.CompareValue;
                ddlField.Items.FindByValue(condition.Field).Selected = true;
                SetupFieldType(ddlField.SelectedValue);
                ddlFieldType.SelectedIndex = -1;
                ddlFieldType.Items.FindByValue(condition.FieldType).Selected = true;
                LoadComparators(ddlFieldType.SelectedValue);
                ddlComparator.Items.FindByValue(condition.Comparator).Selected = true;
                if (condition.DatePart.Length > 0)
                {
                    ddlDatePart.Items.FindByValue(condition.DatePart).Selected = true;
                }
                if (condition.NotComparator == 0)
                {
                    cbxNot.Checked = false;
                }
                else
                {
                    cbxNot.Checked = true;
                }
                SetupValueEntry(ddlFieldType.SelectedValue);
            }

        }

        private void SetupValueEntry(string fieldType)
        {
            if (fieldType == "Date")
            {
                if (ddlDatePart.SelectedValue.Equals("full"))
                {
                    ibChooseDate.Enabled = true;
                    ibChooseDate.Visible = true;
                    txtCompareValue.Enabled = false;
                    ddlDatePart.Enabled = true;
                    val_CompareValue.Enabled = true;
                }
                else
                {
                    ibChooseDate.Enabled = false;
                    ibChooseDate.Visible = false;
                    txtCompareValue.Enabled = true;
                    ddlDatePart.Enabled = true;
                    val_CompareValue.Enabled = true;
                }
            }
            else if (fieldType == "String")
            {
                ibChooseDate.Enabled = false;
                ibChooseDate.Visible = false;
                txtCompareValue.Enabled = true;
                ddlDatePart.Enabled = false;
                val_CompareValue.Enabled = false;
            }
            else
            {
                ibChooseDate.Enabled = false;
                ibChooseDate.Visible = false;
                txtCompareValue.Enabled = true;
                ddlDatePart.Enabled = false;
                val_CompareValue.Enabled = true;
            }

            if (ddlComparator.SelectedValue.Equals("is empty"))
            {
                ibChooseDate.Enabled = false;
                ibChooseDate.Visible = false;
                val_CompareValue.Enabled = false;
                txtCompareValue.Enabled = false;
            }

        }

        private void HidePanels()
        {
            pnlError.Visible = false;
            pnlGroup.Visible = false;
            pnlCondition.Visible = false;
            lblCurrentName.Text = "";
            dvAddGroup.Visible = false;
            dvAddCondition.Visible = false;
        }

        private void PopulateTree()
        {
            ECN_Framework_Entities.Communicator.Filter filter = (ECN_Framework_Entities.Communicator.Filter)Session["Filter"];
            if (filter != null)
            {
                TreeNode tnTop = new TreeNode(filter.FilterName, filter.FilterID.ToString());
                tnTop.ToolTip = "Filter";
                tvFilter.Nodes.Clear();
                tvFilter.Nodes.Add(tnTop);

                if (filter.FilterGroupList.Count > 0)
                {

                    foreach (ECN_Framework_Entities.Communicator.FilterGroup group in filter.FilterGroupList)
                    {
                        StringBuilder groupName = new StringBuilder();
                        groupName.Append(filter.GroupCompareType + " ");

                        if (group.SortOrder > 1)
                        {
                            groupName.Append(group.Name);
                        }
                        else
                        {
                            groupName = new StringBuilder(group.Name);
                        }
                        TreeNode tnGroup = new TreeNode(groupName.ToString(), group.FilterGroupID.ToString());
                        tnGroup.ToolTip = "FilterGroup";
                        tnTop.ChildNodes.Add(tnGroup);

                        //if (group.FilterConditionList.Count > 0)
                        //{

                        //    foreach (ECN_Framework_Entities.Communicator.FilterCondition condition in group.FilterConditionList)
                        //    {
                        //        StringBuilder conditionName = new StringBuilder();
                        //        conditionName.Append(group.ConditionCompareType + " ");

                        //        if (condition.SortOrder > 1)
                        //        {
                        //            conditionName.Append(condition.Field);
                        //            if (condition.FieldType == "Date")
                        //            {
                        //                conditionName.Append(" (");
                        //                conditionName.Append(condition.DatePart);
                        //                conditionName.Append(")");
                        //            }
                        //            conditionName.Append(" ");
                        //            if (condition.NotComparator == 1)
                        //            {
                        //                conditionName.Append("NOT ");
                        //            }
                        //            conditionName.Append(condition.Comparator);
                        //            conditionName.Append(" ");
                        //            conditionName.Append(condition.CompareValue);
                        //        }
                        //        else
                        //        {
                        //            conditionName = new StringBuilder();
                        //            conditionName.Append(condition.Field);
                        //            if (condition.FieldType == "Date")
                        //            {
                        //                conditionName.Append(" (");
                        //                conditionName.Append(condition.DatePart);
                        //                conditionName.Append(")");
                        //            }
                        //            conditionName.Append(" ");
                        //            if (condition.NotComparator == 1)
                        //            {
                        //                conditionName.Append("NOT ");
                        //            }
                        //            conditionName.Append(condition.Comparator);
                        //            conditionName.Append(" ");
                        //            conditionName.Append(condition.CompareValue);
                        //        }
                        //        TreeNode tnCondition = new TreeNode(conditionName.ToString(), condition.FilterConditionID.ToString());
                        //        tnCondition.ToolTip = "FilterCondition";
                        //        tnGroup.ChildNodes.Add(tnCondition);
                        //    }
                        //}
                    }
                }
                tnTop.ExpandAll();
                SetupAddFilter(false);
            }
            else
            {
                SetupAddFilter(true);
            }
        }

        private void LoadGroupGrid(int filterID)
        {
            ECN_Framework_Entities.Communicator.Filter filter = (ECN_Framework_Entities.Communicator.Filter)Session["Filter"];
            if (filter != null)
            {
                gvFilterGroup.DataSource = filter.FilterGroupList;
            }

            try
            {
                gvFilterGroup.DataBind();
            }
            catch
            {
            }
            gvFilterGroup.ShowEmptyTable = true;
            gvFilterGroup.EmptyTableRowText = "No Filter Groups to display";
        }

        private void LoadConditionGrid(int filterGroupID)
        {
            ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            List<ECN_Framework_Entities.Communicator.FilterCondition> conditionList = filterGroup.FilterConditionList;
            gvFilterCondition.DataSource = conditionList;

            try
            {
                gvFilterCondition.DataBind();
            }
            catch
            {
            }
            gvFilterCondition.ShowEmptyTable = true;
            gvFilterCondition.EmptyTableRowText = "No filter conditions to display";
        }

        protected void btnAddFilterGroup_Click(object sender, EventArgs e)
        {
            int filterID = selectedFilterID;
            ECN_Framework_Entities.Communicator.FilterGroup group = null;
            if (btnAddFilterGroup.Text == "Add")
            {
                group = new ECN_Framework_Entities.Communicator.FilterGroup();
                group.Name = txtFilterGroupName.Text.Trim();
                group.ConditionCompareType = ddlConditionCompareType.SelectedValue;
                group.FilterID = filterID;
                group.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                group.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            else
            {
                group = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(Convert.ToInt32(lblFilterGroupID.Text), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                group.Name = txtFilterGroupName.Text.Trim();
                group.ConditionCompareType = ddlConditionCompareType.SelectedValue;
                group.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }

            if (group.SortOrder == -1)
            {
                ECN_Framework_Entities.Communicator.Filter filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(filterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                if (filter.FilterGroupList.Count == 0)
                {
                    group.SortOrder = 1;
                }
                else
                {
                    var result = (from src in filter.FilterGroupList
                                  group src by src.SortOrder into grp
                                  select new
                                  {
                                      SortOrder = grp.Max(t => t.SortOrder)
                                  }).ToList();

                    group.SortOrder = result[0].SortOrder + 1;
                }

            }
            try
            {
                ECN_Framework_BusinessLayer.Communicator.FilterGroup.Save(group, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                loadData();
                mpeSubFilter.Hide();
            }
            catch (ECNException ex)
            {
                setECNError_FilterGroup(ex);
                mpeSubFilter.Show();
            }
        }

        protected void btnCloseFilterGroup_Click(object sender, EventArgs e)
        {
            mpeSubFilter.Hide();
        }

        private void LoadComparators(string fieldType)
        {
            ddlComparator.Items.Clear();
            switch (fieldType)
            {
                case "String":
                    ddlComparator.Items.Add(new ListItem("equals [=]", "equals"));
                    ddlComparator.Items.Add(new ListItem("is in", "is in"));
                    ddlComparator.Items.Add(new ListItem("contains", "contains"));
                    ddlComparator.Items.Add(new ListItem("starts with", "starts with"));
                    ddlComparator.Items.Add(new ListItem("ends with", "ends with"));
                    ddlComparator.Items.Add(new ListItem("is empty", "is empty"));
                    break;
                case "Date":
                    ddlComparator.Items.Add(new ListItem("equals [=]", "equals"));
                    ddlComparator.Items.Add(new ListItem("greater than [>]", "greater than"));
                    ddlComparator.Items.Add(new ListItem("less than [<]", "less than"));
                    ddlComparator.Items.Add(new ListItem("is empty", "is empty"));
                    break;
                case "Number":
                case "Money":
                    ddlComparator.Items.Add(new ListItem("equals [=]", "equals"));
                    ddlComparator.Items.Add(new ListItem("is in", "is in"));
                    ddlComparator.Items.Add(new ListItem("greater than [>]", "greater than"));
                    ddlComparator.Items.Add(new ListItem("less than [<]", "less than"));
                    ddlComparator.Items.Add(new ListItem("is empty", "is empty"));
                    break;
                default:
                    break;
            }
        }

        private void SetupFieldType(string fieldName)
        {
            var userEvents = new string[]
            {
                "UserEvent1",
                "UserEvent2"
            };

            var users = new string[]
            {
                "User1",
                "User2",
                "User3",
                "User4",
                "User5",
                "User6"
            };

            ddlFieldType.SelectedIndex = -1;
            if (FilterBase.CommonFiltersFields.Contains(fieldName)
                || userEvents.Any(x => string.CompareOrdinal(x, fieldName) == 0))
            {
                ddlFieldType.Items.FindByValue(StringFieldType).Selected = true;
                ddlFieldType.Enabled = false;
            }
            else if (users.Any(x => string.CompareOrdinal(x, fieldName) == 0))
            {
                ddlFieldType.Items.FindByValue(StringFieldType).Selected = true;
                ddlFieldType.Enabled = true;
            }
            else if (FilterBase.NonCommonFiltersFields.Contains(fieldName))
            {
                ddlFieldType.Items.FindByValue(DateFieldType).Selected = true;
                ddlFieldType.Enabled = false;
            }
            else
            {
                ddlFieldType.Items.FindByValue(StringFieldType).Selected = true;
                ddlFieldType.Enabled = true;
            }
        }

        private void NodeChanged()
        {
            if (tvFilter.SelectedNode.ToolTip == "Filter")
            {
                HidePanels();
                pnlGroup.Visible = true;
                lblCurrentName.Text = "Filter Groups";
                dvAddGroup.Visible = true;
                int value = Convert.ToInt32(tvFilter.SelectedNode.Value);
                LoadGroupGrid(value);
                SetupAddFilterGroup(null);
            }
            if (tvFilter.SelectedNode.ToolTip == "FilterGroup")
            {
                HidePanels();
                pnlCondition.Visible = true;
                lblCurrentName.Text = "Filter Conditions";
                dvAddCondition.Visible = true;
                int value = Convert.ToInt32(tvFilter.SelectedNode.Value);
                lblFilterGroupID.Text = tvFilter.SelectedNode.Value;
                gvFilterCondition.Visible = true;
                LoadConditionGrid(value);
                SetupAddFilterCondition(null, value);
            }
            if (tvFilter.SelectedNode.ToolTip == "FilterCondition")
            {
                HidePanels();
                pnlCondition.Visible = true;
                lblCurrentName.Text = "Filter Condition";
                dvAddCondition.Visible = true;
                int value = Convert.ToInt32(tvFilter.SelectedNode.Value);
                gvFilterCondition.Visible = true;
                LoadConditionGrid(Convert.ToInt32(tvFilter.SelectedNode.Parent.Value));
                SetupAddFilterCondition(value, Convert.ToInt32(tvFilter.SelectedNode.Parent.Value));
            }
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

        protected void tvFilter_SelectedNodeChanged(object sender, EventArgs e)
        {
            NodeChanged();
        }

        protected void tvFilter_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {
            e.Node.Selected = true;
        }

        protected void tvFilter_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            e.Node.Selected = true;
        }

        protected void gvFilterGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int filterID = selectedFilterID;
            int filterGroupID = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "Edit")
            {
                SetupAddFilterGroup(filterGroupID);
                pnlEMessage.Visible = false;
                lblEMessage.Text = "";
                mpeSubFilter.Show();
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.FilterGroup.Delete(filterID, filterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    loadData();
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
            else if (e.CommandName == "AddFilterCondition")
            {
                HidePanels();
                pnlCondition.Visible = true;
                lblCurrentName.Text = "Filter Conditions";
                dvAddCondition.Visible = true;
                gvFilterCondition.Visible = true;
                lblFilterGroupID.Text = filterGroupID.ToString();
                LoadConditionGrid(filterGroupID);
                SetupAddFilterCondition(null, filterGroupID);
                mpeFilterCondition.Show();
            }
        }

        protected void gvFilterGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvFilterGroup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Session["Filter"] = null;
            Response.Redirect("filters.aspx?GroupID=" + selectedGroupID.ToString());
        }

        protected void lbAdd_Click(object sender, EventArgs e)
        {
            pnlEMessage.Visible = false;
            lblEMessage.Text = "";
            mpeSubFilter.Show();
            HidePanels();
            pnlGroup.Visible = true;
            lblCurrentName.Text = "Filter Groups";
            dvAddGroup.Visible = true;
            LoadGroupGrid(selectedFilterID);
            SetupAddFilterGroup(null);
        }

        protected void btnAddFilter_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.Filter filter = null;
            if (btnAddFilter.Text == "Add")
            {
                filter = new ECN_Framework_Entities.Communicator.Filter();
                filter.FilterName = txtFilterName.Text.Trim();
                filter.GroupCompareType = ddlGroupCompareType.SelectedValue;
                filter.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                filter.GroupID = selectedGroupID;
                filter.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            else
            {
                filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(selectedFilterID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                filter.FilterName = txtFilterName.Text.Trim();
                filter.GroupCompareType = ddlGroupCompareType.SelectedValue;
                filter.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Filter.Save(filter, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                if (Request.Url.AbsoluteUri.ToString().Contains("filtersplusedit"))
                    Response.Redirect("filtersplusedit.aspx?GroupID=" + selectedGroupID.ToString() + "&FilterID=" + filter.FilterID.ToString());
                else
                {
                    selectedFilterID = filter.FilterID;
                    loadData();
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void btnAddFilterCondition_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.FilterCondition filterCondition = null;
            if (btnAddFilterCondition.Text == "Add")
            {
                filterCondition = new ECN_Framework_Entities.Communicator.FilterCondition();
                filterCondition.FilterGroupID = Convert.ToInt32(lblRefGroupID.Text);
                filterCondition.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                filterCondition.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            }
            else
            {
                filterCondition = ECN_Framework_BusinessLayer.Communicator.FilterCondition.GetByFilterConditionID(Convert.ToInt32(lblFilterConditionID.Text), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                filterCondition.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            filterCondition.Field = ddlField.SelectedValue;
            filterCondition.FieldType = ddlFieldType.SelectedValue;
            if (ddlFieldType.SelectedValue == "Date")
            {
                filterCondition.DatePart = ddlDatePart.SelectedValue;
            }
            if (cbxNot.Checked)
            {
                filterCondition.NotComparator = 1;
            }
            else
            {
                filterCondition.NotComparator = 0;
            }
            filterCondition.Comparator = ddlComparator.SelectedValue;
            filterCondition.CompareValue = txtCompareValue.Text;
            if (filterCondition.SortOrder == -1)
            {
                ECN_Framework_Entities.Communicator.FilterGroup filterGroup = ECN_Framework_BusinessLayer.Communicator.FilterGroup.GetByFilterGroupID(filterCondition.FilterGroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                if (filterGroup.FilterConditionList.Count == 0)
                {
                    filterCondition.SortOrder = 1;
                }
                else
                {
                    var result = (from src in filterGroup.FilterConditionList
                                  group src by src.SortOrder into grp
                                  select new
                                  {
                                      SortOrder = grp.Max(t => t.SortOrder)
                                  }).ToList();

                    filterCondition.SortOrder = result[0].SortOrder + 1;
                }

            }
            try
            {
                ECN_Framework_BusinessLayer.Communicator.FilterCondition.Save(filterCondition, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                loadData();
                mpeFilterCondition.Hide();
            }
            catch (ECNException ex)
            {
                setECNError_FilterCondition(ex);
                mpeFilterCondition.Show();
            }
        }


        protected void btnCloseFilterCondition_Click(object sender, EventArgs e)
        {
            mpeFilterCondition.Hide();
        }

        protected void gvFilterCondition_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int filterID = selectedFilterID;
            int filterConditionID = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "Edit")
            {
                SetupAddFilterCondition(filterConditionID, Convert.ToInt32(lblRefGroupID.Text));
                mpeFilterCondition.Show();
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    ECN_Framework_BusinessLayer.Communicator.FilterCondition.Delete(Convert.ToInt32(lblRefGroupID.Text), filterConditionID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    loadData();
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
        }

        protected void gvFilterCondition_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvFilterCondition_RowEditing(object sender, GridViewEditEventArgs e)
        {


        }

        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupFieldType(ddlField.SelectedValue);
            LoadComparators(ddlFieldType.SelectedValue);
            SetupValueEntry(ddlFieldType.SelectedValue);
            mpeFilterCondition.Show();
        }

        protected void ddlFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComparators(ddlFieldType.SelectedValue);
            SetupValueEntry(ddlFieldType.SelectedValue);
            mpeFilterCondition.Show();
        }

        protected void ddlDatePart_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComparators(ddlFieldType.SelectedValue);
            SetupValueEntry(ddlFieldType.SelectedValue);
            mpeFilterCondition.Show();
        }

        protected void ddlComparator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlComparator.SelectedValue.Equals("is empty"))
            {
                txtCompareValue.Text = "";
                txtCompareValue.Enabled = false;
                val_CompareValue.Enabled = false;
                ibChooseDate.Enabled = false;
                ibChooseDate.Visible = false;
            }
            else
            {
                if (ddlFieldType.SelectedValue == "Date")
                {
                    if (ddlDatePart.SelectedValue.Equals("full"))
                    {
                        ibChooseDate.Enabled = true;
                        ibChooseDate.Visible = true;
                        txtCompareValue.Enabled = false;
                        ddlDatePart.Enabled = true;
                        val_CompareValue.Enabled = true;
                    }
                    else
                    {
                        ibChooseDate.Enabled = false;
                        ibChooseDate.Visible = false;
                        txtCompareValue.Enabled = true;
                        ddlDatePart.Enabled = true;
                        val_CompareValue.Enabled = true;
                    }
                }
                else if (ddlFieldType.SelectedValue == "String")
                {
                    ibChooseDate.Enabled = false;
                    ibChooseDate.Visible = false;
                    txtCompareValue.Enabled = true;
                    ddlDatePart.Enabled = false;
                    val_CompareValue.Enabled = false;
                }
                else
                {
                    ibChooseDate.Enabled = false;
                    ibChooseDate.Visible = false;
                    txtCompareValue.Enabled = true;
                    ddlDatePart.Enabled = false;
                    val_CompareValue.Enabled = true;
                }
            }
            mpeFilterCondition.Show();
        }



        protected void btnSaveDate_Click(object sender, EventArgs e)
        {
            if (rbToday.Checked)
            {
                txtCompareValue.Text = "EXP:Today";
            }
            else if (rbTodayPlus.Checked)
            {
                try
                {
                    Convert.ToInt32(txtDays.Text);
                    txtCompareValue.Text = "EXP:Today[";
                    if (ddlPlusMinus.SelectedValue == "Plus")
                    {
                        txtCompareValue.Text += "+";
                    }
                    else
                    {
                        txtCompareValue.Text += "-";
                    }
                    txtCompareValue.Text += txtDays.Text.Trim() + "]";
                }
                catch (Exception)
                {
                    txtCompareValue.Text = "";
                }
            }
            else if (rbSelect.Checked)
            {
                txtCompareValue.Text = txtDatePicker.Text;
            }
            //LoadComparators("Date");
            mpeFilterCondition.Show();
        }

        protected void rbToday_CheckedChanged(object sender, EventArgs e)
        {
            if (rbToday.Checked)
            {
                ddlPlusMinus.Enabled = false;
                txtDays.Text = "";
                txtDays.Enabled = false;
                txtDatePicker.Text = "";
                btnDatePicker.Enabled = false;
                rfvDatePicker.Enabled = false;
                rfvDays.Enabled = false;
                rvDays.Enabled = false;
                mpeFilterCondition.Show();
                mpeCalendar.Show();
            }
        }

        protected void rbTodayPlus_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodayPlus.Checked)
            {
                ddlPlusMinus.Enabled = true;
                txtDays.Enabled = true;
                txtDatePicker.Text = "";
                btnDatePicker.Enabled = false;
                rfvDatePicker.Enabled = false;
                rfvDays.Enabled = true;
                rvDays.Enabled = true;
                mpeFilterCondition.Show();
                mpeCalendar.Show();
            }
        }

        protected void rbSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelect.Checked)
            {
                ddlPlusMinus.Enabled = false;
                txtDays.Text = "";
                txtDays.Enabled = false;
                btnDatePicker.Enabled = true;
                rfvDatePicker.Enabled = true;
                rfvDays.Enabled = false;
                rvDays.Enabled = false;
                mpeFilterCondition.Show();
                mpeCalendar.Show();
            }
        }

        protected void lbAddCondition_Click(object sender, EventArgs e)
        {
            HidePanels();
            pnlCondition.Visible = true;
            lblCurrentName.Text = "Filter Conditions";
            dvAddCondition.Visible = true;
            gvFilterCondition.Visible = true;
            LoadConditionGrid(Convert.ToInt32(lblFilterGroupID.Text));
            SetupAddFilterCondition(null, Convert.ToInt32(lblFilterGroupID.Text));
            mpeFilterCondition.Show();
        }

        public void reset()
        {
            selectedGroupID = 0;
            selectedFilterID = 0;
            loadData();
        }

        #endregion
    }
}