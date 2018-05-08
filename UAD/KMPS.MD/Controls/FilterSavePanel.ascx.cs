using System;
using System.Collections.Generic;
using System.Linq;
using KMPS.MD.Objects;
using Telerik.Web.UI;

namespace KMPS.MD.Controls
{
    public partial class FilterPanel : BaseControl
    {
        private const string InvalidFilterNameErrorMessage = "Please enter a valid filter name.";
        private const string FilterAlreadyExistsErrorMessage = "The filter Name you entered already exists. Please save under a different name.";
        private const string PleaseSelectFilterErrorMessage = "Please select filter.";
        private const string PleaseSelectQuestionCategoryErrorMessage = "Please select Question Category.";
        private const string QuestionNameAlreadyExistsErrorMessage = "The Question Name you entered already exists. Please save under a different question name.";
        private const string CanNotOverwriteFilterErrorMessage = "Cannot overwrite existing filter. Existing filter data segment and edited data segment counts are not same.";
        private const string FieldNameDataCompare = "DataCompare";
        private const string ExistingFilterValue = "Existing";
        private const char CommaSeparator = ',';
        private const string EditMode = "Edit";
        private const string AddNewMode = "AddNew";
        public Delegate hideFilterSavePopup;
        delegate void HidePanel();
        delegate void LoadSelectedFilterData(List<int> filterIDs);
        public Delegate LoadSavedFilterName;
        public event EventHandler CausePostBack;
        
        public string FilterIDs
        {
            get
            {
                try
                {
                    return (string)ViewState["FilterIDs"];
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                ViewState["FilterIDs"] = value;
            }
        }

        public Filters FilterCollections
        {
            get
            {
                try
                {
                    return (Filters)Session["FilterCollections"];
                }
                catch
                {
                    return new Filters(clientconnections, UserID);
                }
            }

            set
            {
                Session["FilterCollections"] = new Filters(clientconnections, UserID);
                Session["FilterCollections"] = value;
            }
        }

        public string Mode
        {
            get
            {
                try
                {
                    return (string)ViewState["Mode"];
                }
                catch
                {
                    return string.Empty;
                }
            }

            set
            {
                ViewState["Mode"] = value;
            }
        }

        private void FiltersListHide()
        {
            FiltersList.Visible = false;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;

            HidePanel delFilters = FiltersListHide;
            this.FiltersList.hideShowFilterPopup = delFilters;

            LoadSelectedFilterData delNoParamFilterID = LoadFilterDetails;
            this.FiltersList.LoadSelectedFilterData = delNoParamFilterID;

            this.mdlPopSaveFilter.Show();
       }

        private void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void rbNewExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIsLocked.Enabled = true;
            rddtFilterCategory.Enabled = true;
            cbAddtoSalesView.Enabled = true;
            rddtQuestionCategory.Enabled = true;
            txtQuestionName.ReadOnly = false;
            ImgFilterList.Visible = false;

            if (rbNewExisting.SelectedValue.Equals("New", StringComparison.OrdinalIgnoreCase))
            {
                Mode = "AddNew";
                btnSaveFilter.OnClientClick = null;
                txtFilterName.ReadOnly = false;
            }
            else
            {
                btnSaveFilter.OnClientClick = "if(!confirm('Are you sure you want overwrite existing filter?')) return false;";
                txtFilterName.ReadOnly = false;

                if (!Mode.Equals("Edit", StringComparison.OrdinalIgnoreCase))
                {
                    Mode = "AddExisting";
                    txtFilterName.ReadOnly = true;
                    cbIsLocked.Enabled = false;
                    rddtFilterCategory.Enabled = false;
                    cbAddtoSalesView.Enabled = false;
                    rddtQuestionCategory.Enabled = false;
                    txtQuestionName.ReadOnly = true;
                    ImgFilterList.Visible = true;
                }
            }

            txtFilterName.Text = string.Empty;
            cbIsLocked.Checked = false;
            pnlQuestion.Visible = false;
            cbAddtoSalesView.Checked = false;
            txtQuestionName.Text = string.Empty;
            rddtFilterCategory.Entries.Clear();
            rddtQuestionCategory.Entries.Clear();
            txtNotes.Text = string.Empty;
        }

        protected void cbAddtoSalesView_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAddtoSalesView.Checked)
                pnlQuestion.Visible = true;
            else
                pnlQuestion.Visible = false;

            rddtQuestionCategory.Entries.Clear();
            txtQuestionName.Text = string.Empty;
        }

        public void LoadFilterData()
        {
            MDFilter mdf = new MDFilter();

            rbNewExisting.ClearSelection();
            rbNewExisting.Visible = false;
            rbNewExisting.Items.FindByValue("Existing").Selected = true;
            mdf = MDFilter.GetByID(clientconnections, Convert.ToInt32(FilterIDs));

            if (mdf.FilterId > 0)
            {
                txtFilterName.Text = mdf.Name;
                cbIsLocked.Checked = mdf.IsLocked;
                rddtFilterCategory.SelectedValue = mdf.FilterCategoryID.ToString();
                txtNotes.Text = mdf.Notes;
                cbAddtoSalesView.Checked = mdf.AddtoSalesView;

                if (cbAddtoSalesView.Checked)
                {
                    pnlQuestion.Visible = true;
                    rddtQuestionCategory.SelectedValue = mdf.QuestionCategoryID.ToString();
                    txtQuestionName.Text = mdf.QuestionName;
                }
                else
                {
                    pnlQuestion.Visible = false;
                }
            }
        }

        private void ResetControls()
        {
            rbNewExisting.SelectedIndex = 0;
            txtFilterName.Text = string.Empty;
            txtFilterName.ReadOnly = false;
            cbIsLocked.Checked = false;
            cbAddtoSalesView.Checked = false;
            pnlQuestion.Visible = false;
            txtQuestionName.Text = string.Empty;
            btnSaveFilter.OnClientClick = null;
            rddtFilterCategory.Entries.Clear();
            rddtQuestionCategory.Entries.Clear();
            rddtFilterCategory.Enabled = true;
            cbAddtoSalesView.Enabled = true;
            rddtQuestionCategory.Enabled = true;
            txtQuestionName.ReadOnly = false;
            ImgFilterList.Visible = false;
            txtNotes.Text = string.Empty;
        }

        public void LoadControls()
        {
            List<FilterCategory> lfc = FilterCategory.GetAll(clientconnections);

            FilterCategory fc = new FilterCategory();

            rddtFilterCategory.DataSource = lfc;
            rddtFilterCategory.DataBind();
            rddtFilterCategory.ExpandAllDropDownNodes();

            rddtQuestionCategory.DataSource = QuestionCategory.GetAll(clientconnections);
            rddtQuestionCategory.DataBind();
            rddtQuestionCategory.ExpandAllDropDownNodes();
        }

        protected void rddtFilterCategory_NodeDataBound(object sender, DropDownTreeNodeDataBoundEventArguments e)
        {
            e.DropDownTreeNode.Expanded = true;
        }

        protected void btnSaveFilter_Click(object sender, EventArgs e)
        {
            try
            {
                var editMode = Mode.Equals(EditMode, StringComparison.OrdinalIgnoreCase);
                var addNewMode = Mode.Equals(AddNewMode, StringComparison.OrdinalIgnoreCase);

                var filterId = 0;

                if (!FilterNameValid(addNewMode, editMode, ref filterId))
                {
                    return;
                }

                var filter = GetMdFilter(addNewMode, filterId);

                filterId = MDFilter.insert(clientconnections, filter);
                var filterGroups = new List<FilterGroup>();
                var deleteGroup = false;

                if (rbNewExisting.SelectedValue.Equals(ExistingFilterValue, StringComparison.OrdinalIgnoreCase) && !editMode)
                {
                    filterGroups = FilterGroup.getByFilterID(clientconnections, Convert.ToInt32(hfFilterID.Value));

                    var split = FilterIDs.Split(CommaSeparator);

                    if (FilterSchedule.ExistsByFilterID(clientconnections, Convert.ToInt32(hfFilterID.Value)))
                    {
                        if (filterGroups.Count != split.Length)
                        {
                            DisplayError(CanNotOverwriteFilterErrorMessage);
                            return;
                        }

                        FilterDetails.Delete_ByFilterID(clientconnections, Convert.ToInt32(hfFilterID.Value));
                    }
                    else
                    {
                       FilterDetails.Delete_ByFilterID(clientconnections, Convert.ToInt32(hfFilterID.Value));
                       FilterGroup.Delete_ByFilterID(clientconnections, Convert.ToInt32(hfFilterID.Value));
                       deleteGroup = true;
                    }
                }

                if (!editMode)
                {
                    SaveFilterDetails(deleteGroup, filterGroups, filterId);
                }

                if (!editMode)
                {
                    LoadSavedFilterName.DynamicInvoke(txtFilterName.Text.Trim());
                    CausePostBack?.Invoke(sender, e);
                }

                ResetControls();
                hideFilterSavePopup.DynamicInvoke();
                mdlPopSaveFilter.Hide();
            }
            catch (Exception exception)
            {
                DisplayError(exception.Message);
            }
        }

        private MDFilter GetMdFilter(bool addNewMode, int filterId)
        {
            MDFilter filter;
            if (addNewMode)
            {
                filter = new MDFilter
                {
                    CreatedUserID = UserID,
                    CreatedDate = DateTime.Now,
                    UpdatedUserID = UserID,
                    UpdatedDate = DateTime.Now,
                    FilterType = ViewType.ToString(),
                    PubID = PubID,
                    BrandID = BrandID,
                    IsDeleted = false
                };
            }
            else
            {
                filter = MDFilter.GetByID(clientconnections, filterId);
                filter.UpdatedUserID = UserID;
                filter.UpdatedDate = DateTime.Now;
            }

            filter.Name = txtFilterName.Text;
            filter.IsLocked = cbIsLocked.Checked;
            filter.FilterCategoryID = string.IsNullOrWhiteSpace(rddtFilterCategory.SelectedValue)
                                          ? 0 
                                          : Convert.ToInt32(rddtFilterCategory.SelectedValue);

            filter.Notes = txtNotes.Text;
            filter.AddtoSalesView = cbAddtoSalesView.Checked;
            if (cbAddtoSalesView.Checked)
            { 
                filter.QuestionName = txtQuestionName.Text;
                filter.QuestionCategoryID = Convert.ToInt32(rddtQuestionCategory.SelectedValue);
            }
            else
            {
                filter.QuestionName = string.Empty;
                filter.QuestionCategoryID = 0;
            }

            return filter;
        }

        private bool FilterNameValid(bool addNewMode, bool editMode, ref int filterId)
        {
            if (addNewMode || editMode)
            {
                var filterName = txtFilterName.Text.Trim();

                if (filterName.Length <= 0)
                {
                    DisplayError(InvalidFilterNameErrorMessage);
                    return false;
                }

                if (editMode)
                {
                    filterId = Convert.ToInt32(FilterIDs);
                }

                if (MDFilter.ExistsByFilterName(clientconnections, filterId, filterName))
                {
                    DisplayError(FilterAlreadyExistsErrorMessage);
                    return false;
                }
            }
            else
            {
                if (Convert.ToInt32(hfFilterID.Value) <= 0)
                {
                    lblErrorMessage.Text = PleaseSelectFilterErrorMessage;
                    divError.Visible = true;
                    return false;
                }

                filterId = Convert.ToInt32(hfFilterID.Value);
            }

            if (cbAddtoSalesView.Checked)
            {
                if (string.IsNullOrWhiteSpace(rddtQuestionCategory.SelectedValue))
                {
                    DisplayError(PleaseSelectQuestionCategoryErrorMessage);
                    return false;
                }

                if (MDFilter.ExistsQuestionName(clientconnections, filterId, txtQuestionName.Text))
                {
                    DisplayError(QuestionNameAlreadyExistsErrorMessage);
                    return false;
                }
            }

            return true;
        }

        private void SaveFilterDetails(bool deleteGroup, IList<FilterGroup> filterGroups, int filterId)
        {
            var split = FilterIDs.Split(CommaSeparator);

            var index = 1;
            foreach (var filterNumber in split)
            {
                int filterGroupId;
                if (rbNewExisting.SelectedValue.Equals(ExistingFilterValue, StringComparison.OrdinalIgnoreCase) && !deleteGroup)
                {
                    filterGroupId = filterGroups[index - 1].FilterGroupID;
                }
                else
                {
                    filterGroupId = FilterGroup.Save(clientconnections, filterId, index);
                }

                var filter = FilterCollections.Single(f => f.FilterNo == Convert.ToInt32(filterNumber));
                foreach (var field in filter.Fields)
                {
                    if (field.Name.Equals(FieldNameDataCompare, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var filterDetails = new FilterDetails
                    {
                        FilterType = field.FilterType,
                        Group = field.Group,
                        Name = field.Name,
                        Values = field.Values,
                        SearchCondition = field.SearchCondition,
                        FilterGroupID = filterGroupId
                    };

                    FilterDetails.Save(clientconnections, filterDetails);
                }

                index++;
            }
        }

        protected void btnCloseFilter_Click(object sender, EventArgs e)
        {
            ResetControls();
            hideFilterSavePopup.DynamicInvoke();
            mdlPopSaveFilter.Hide();
        }

         protected void ImgFilterList_Click(object sender, EventArgs e)
        {
            FiltersList.Visible = true;
            FiltersList.PubID = PubID;
            FiltersList.BrandID = BrandID;
            FiltersList.ViewType = ViewType;
            FiltersList.UserID = UserID;
            FiltersList.Mode = Mode;
            FiltersList.LoadControls();
        }

         public void LoadFilterDetails(List<int> filterIDs)
        {
            hfFilterID.Value = filterIDs.First().ToString();
            MDFilter mdf = MDFilter.GetByID(clientconnections, filterIDs.First());
            txtFilterName.Text = mdf.Name;
            cbIsLocked.Checked = mdf.IsLocked;
            rddtFilterCategory.SelectedValue = mdf.FilterCategoryID.ToString();
            txtNotes.Text = mdf.Notes;
            cbAddtoSalesView.Checked = mdf.AddtoSalesView;
            if (cbAddtoSalesView.Checked)
                pnlQuestion.Visible = true;
            rddtQuestionCategory.SelectedValue = mdf.QuestionCategoryID.ToString();
            txtQuestionName.Text = mdf.QuestionName;
        }
    }
}