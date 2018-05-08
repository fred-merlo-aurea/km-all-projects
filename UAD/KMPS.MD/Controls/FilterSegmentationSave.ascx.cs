using System;
using System.Collections.Generic;
using System.Linq;
using KM.Common;
using KM.Common.Extensions;
using KMPS.MD.Objects;
using Telerik.Web.UI;
using EntityFilterSegmentation = FrameworkUAD.Entity.FilterSegmentation;
using Enums = KMPS.MD.Objects.Enums;

namespace KMPS.MD.Controls
{
    public partial class FilterSegmentationSave : BaseControl
    {
        private const string ModeAddNew = "AddNew";
        private const string ModeEdit = "Edit";
        private const string ModeAddExisting = "AddExisting";
        private const string CommaString = ",";
        private const char CommaChar = ',';
        private const string NameDataCompare = "DataCompare";

        public Delegate hideFilterSegmentationPopup;
        delegate void HidePanel();
        delegate void LoadSelectedFilterSegmentationData(int filtersegmentationIDs);

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        public Delegate LoadSavedFilterSegmentationName;
        
        public FilterViews FilterViewCollection
        {
            get
            {
                if (Session[fvSessionName] == null)
                {
                    Session[fvSessionName] = new FilterViews(clientconnections, UserSession.UserID);
                }

                return (FilterViews)Session[fvSessionName];
            }
            set
            {
                Session[fvSessionName] = value;
            }
        }

        public string fvSessionName
        {
            get
            {
                return (string)ViewState["fvSessionName"];
            }
            set
            {
                ViewState["fvSessionName"] = value;
            }
        }

        public Filters FilterCollection
        {
            get
            {
                return (Filters)Session[fcSessionName];
            }
            set
            {
                Session[fcSessionName] = value;
            }
        }

        public int FilterSegmentationID
        {
            get
            {
                try
                {
                    return (int)ViewState["FilterSegmentationID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["FilterSegmentationID"] = value;
            }
        }

        public string fcSessionName
        {
            get
            {
                return (string)ViewState["fcSessionName"];
            }
            set
            {
                ViewState["fcSessionName"] = value;
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

        public string FilterViewNos
        {
            get
            {
                try
                {
                    return (string)ViewState["FilterViewNos"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["FilterViewNos"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;

            // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
            HidePanel delFilters = new HidePanel(FiltersListHide);
            this.FiltersList.hideShowFilterPopup = delFilters;

            LoadSelectedFilterSegmentationData delNoParamFilterSegmentationID = new LoadSelectedFilterSegmentationData(LoadFilterSegmentationData);
            this.FiltersList.LoadSelectedFilterSegmentationData = delNoParamFilterSegmentationID;

            this.mdlPopSaveFS.Show();
        }

        private void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        private void ResetControls()
        {
            txtFilterName.Text = string.Empty;
            rddtFilterCategory.Entries.Clear();
            rddtFilterCategory.Enabled = true;
            txtFSName.Text = string.Empty;
            txtNotes.Text = string.Empty;
        }

        public void LoadControls()
        {
            List<KMPS.MD.Objects.FilterCategory> lfc = KMPS.MD.Objects.FilterCategory.GetAll(clientconnections);

            KMPS.MD.Objects.FilterCategory fc = new KMPS.MD.Objects.FilterCategory();

            rddtFilterCategory.DataSource = lfc;
            rddtFilterCategory.DataBind();
            rddtFilterCategory.ExpandAllDropDownNodes();
        }

        public void LoadFilterSegmentationData()
        {
            FrameworkUAD.Entity.FilterSegmentation fs = new FrameworkUAD.Entity.FilterSegmentation();
            fs = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(FilterSegmentationID, clientconnections);
            hfFilterSegmentationID.Value = fs.FilterSegmentationID.ToString();
            txtFSName.Text = fs.FilterSegmentationName;
            txtNotes.Text = fs.Notes;

            MDFilter mdf = new MDFilter();
            mdf = MDFilter.GetByID(clientconnections, Convert.ToInt32(fs.FilterID));
            txtFilterName.Text = mdf.Name;
            rddtFilterCategory.SelectedValue = mdf.FilterCategoryID.ToString();
        }

        protected void rddtFilterCategory_NodeDataBound(object sender, DropDownTreeNodeDataBoundEventArguments e)
        {
            e.DropDownTreeNode.Expanded = true;
        }

        protected void btnSaveFS_Click(object sender, EventArgs e)
        {
            if (txtFilterName.Text.Trim().Length <= 0)
            {
                DisplayError("Please enter a valid filter name.");
                return;
            }

            var args = GetSaveArgs();
            var errorMessage = GetMessageIfFilterExists(args.FilterId);

            if (!errorMessage.IsNullOrWhiteSpace())
            {
                DisplayError(errorMessage);
                return;
            }

            args.DeleteGroup = false;
            if (!TrySaveMdFilter(args))
            {
                return;
            }

            SaveFilterDetails(args);

            if (args.IsAddExisting && args.IsFilterSegmentScheduled)
            {
                DisplayError("Cannot overwrite existing filter segments. One or more filter data segments are scheduled for export. However, the filters data is saved.");
                return;
            }

            SaveFilterSegmentation(args);
            SaveFilterSegmentationGroups(args);

            if (!args.IsEdit)
            {
                LoadSavedFilterSegmentationName.DynamicInvoke($"{txtFilterName.Text} - {txtFSName.Text}");
            }

            ResetControls();
            hideFilterSegmentationPopup.DynamicInvoke();
            mdlPopSaveFS.Hide();
        }

        private void SaveFilterSegmentationGroups(FilterSegmentationSaveArgs args)
        {
            Guard.NotNull(args, nameof(args));
            if (!args.IsAddNew && !args.IsAddExisting)
            {
                return;
            }

            var query = FilterViewNos.Split(CommaChar);

            foreach (var viewNo in query)
            {
                int viewNumber;
                int.TryParse(viewNo, out viewNumber);
                var filterView = FilterViewCollection.Single(f => f.FilterViewNo == viewNumber);

                var fsGroup = new FrameworkUAD.Entity.FilterSegmentationGroup
                {
                    FilterSegmentationID = args.FilterSegmentationId,
                    FilterGroupID_Selected = GetGroupIds(filterView.SelectedFilterNo).ToList(),
                    FilterGroupID_Suppressed = GetGroupIds(filterView.SuppressedFilterNo).ToList(),
                    SelectedOperation = filterView.SelectedFilterOperation,
                    SuppressedOperation = filterView.SuppressedFilterOperation == string.Empty
                        ? null
                        : filterView.SuppressedFilterOperation
                };

                new FrameworkUAD.BusinessLogic.FilterSegmentationGroup().Save(fsGroup, clientconnections);
            }
        }

        private IEnumerable<int> GetGroupIds(string str)
        {
            Guard.NotNull(str, nameof(str));
            var textArray = str.Split(new[] { CommaChar }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var stringItem in textArray)
            {
                int id;
                int.TryParse(stringItem, out id);
                yield return FilterCollection.First(x => x.FilterNo == id).FilterGroupID;
            }
        }

        private void SaveFilterSegmentation(FilterSegmentationSaveArgs args)
        {
            Guard.NotNull(args, nameof(args));

            if (args.IsAddNew)
            {
                args.FilterSegmentation.FilterSegmentationName = txtFSName.Text;
                args.FilterSegmentation.Notes = txtNotes.Text;
                args.FilterSegmentation.FilterID = args.FilterId;
                args.FilterSegmentation.CreatedUserID = UserID;
                args.FilterSegmentation.CreatedDate = DateTime.Now;
            }
            else
            {
                if (args.IsAddExisting)
                {
                    var filterSegmentationgroup = new FrameworkUAD.BusinessLogic.FilterSegmentationGroup();
                    filterSegmentationgroup.DeleteByFilterSegmentationID(args.FilterSegmentationId, clientconnections);
                }
                args.FilterSegmentation = new FrameworkUAD.BusinessLogic.FilterSegmentation()
                    .SelectByID(args.FilterSegmentationId, clientconnections);
                args.FilterSegmentation.FilterSegmentationName = txtFSName.Text;
                args.FilterSegmentation.Notes = txtNotes.Text;
                args.FilterSegmentation.UpdatedUserID = UserID;
                args.FilterSegmentation.UpdatedDate = DateTime.Now;
            }

            args.FilterSegmentationId = new FrameworkUAD.BusinessLogic.FilterSegmentation().Save(args.FilterSegmentation, clientconnections);
        }

        private void SaveFilterDetails(FilterSegmentationSaveArgs args)
        {
            Guard.NotNull(args, nameof(args));

            if (args.IsEdit)
            {
                return;
            }

            var groupIndex = 0;

            foreach (var filter in FilterCollection)
            {
                groupIndex++;

                int filterGroupId;
                if (args.IsAddExisting && !args.DeleteGroup)
                {
                    filterGroupId = args.FilterGroups.ElementAt(groupIndex - 1).FilterGroupID;
                }
                else
                {
                    filterGroupId = FilterGroup.Save(clientconnections, args.FilterId, groupIndex);
                }

                foreach (var field in filter.Fields)
                {
                    if (!field.Name.EqualsIgnoreCase(NameDataCompare))
                    {
                        var filterDetail = new FilterDetails
                        {
                            FilterType = field.FilterType,
                            Group = field.Group,
                            Name = field.Name,
                            Values = field.Values,
                            SearchCondition = field.SearchCondition,
                            FilterGroupID = filterGroupId
                        };

                        FilterDetails.Save(clientconnections, filterDetail);
                    }
                }

                filter.FilterGroupID = filterGroupId;
                FilterCollection.Update(filter);
            }
        }

        private bool TrySaveMdFilter(FilterSegmentationSaveArgs args)
        {
            Guard.NotNull(args, nameof(args));

            var mdFilter = new MDFilter();
            int categoryId;
            int.TryParse(rddtFilterCategory.SelectedValue, out categoryId);

            if (args.IsAddNew)
            {
                mdFilter.CreatedUserID = UserID;
                mdFilter.CreatedDate = DateTime.Now;
                mdFilter.UpdatedUserID = UserID;
                mdFilter.UpdatedDate = DateTime.Now;
                mdFilter.FilterType = ViewType.ToString();
                mdFilter.PubID = PubID;
                mdFilter.BrandID = BrandID;
                mdFilter.IsDeleted = false;
                mdFilter.Name = txtFilterName.Text;
                mdFilter.IsLocked = false;
                mdFilter.FilterCategoryID = categoryId;
                args.FilterId = MDFilter.insert(clientconnections, mdFilter);
            }
            else
            {
                mdFilter = MDFilter.GetByID(clientconnections, args.FilterId);
                if (args.IsEdit)
                {
                    mdFilter.Name = txtFilterName.Text;
                    mdFilter.FilterCategoryID = categoryId;
                }

                mdFilter.UpdatedUserID = UserID;
                mdFilter.UpdatedDate = DateTime.Now;
                MDFilter.insert(clientconnections, mdFilter);

                if (args.IsAddExisting)
                {
                    args.FilterGroups = FilterGroup.getByFilterID(clientconnections, args.FilterId);

                    var query = FilterCollection.Select(filterNo => filterNo);

                    if (args.IsFilterSegmentScheduled)
                    {
                        if (args.FilterGroups.Count != query.Count())
                        {
                            DisplayError("Cannot overwrite existing filter. A filter segment is scheduled for export also existing filter data segment and edited data segment counts are not same.");
                            return false;
                        }

                        FilterDetails.Delete_ByFilterID(clientconnections, args.FilterId);
                    }
                    else
                    {
                        FilterDetails.Delete_ByFilterID(clientconnections, args.FilterId);
                        FilterGroup.Delete_ByFilterID(clientconnections, args.FilterId);
                        args.DeleteGroup = true;
                    }
                }
            }

            return true;
        }

        private FilterSegmentationSaveArgs GetSaveArgs()
        {
            var args = new FilterSegmentationSaveArgs
            {
                IsAddNew = Mode.EqualsIgnoreCase(ModeAddNew),
                IsEdit = Mode.EqualsIgnoreCase(ModeEdit),
                IsAddExisting = Mode.EqualsIgnoreCase(ModeAddExisting),
                FilterSegmentation = new EntityFilterSegmentation(),
                IsFilterSegmentScheduled = false
            };

            if (!args.IsEdit && !args.IsAddExisting)
            {
                return args;
            }

            int id;
            int.TryParse(hfFilterSegmentationID.Value, out id);
            args.FilterSegmentation = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(id, clientconnections);
            args.FilterId = args.FilterSegmentation.FilterID;
            args.FilterSegmentationId = args.FilterSegmentation.FilterSegmentationID;

            if (!args.IsAddExisting)
            {
                return args;
            }

            var filterSegmentationgroup = new FrameworkUAD.BusinessLogic.FilterSegmentationGroup();
            var groupList = filterSegmentationgroup.SelectByFilterSegmentationID(args.FilterSegmentationId, clientconnections);

            if (FilterSchedule.ExistsByFilterSegmentationID(clientconnections, args.FilterSegmentationId))
            {
                var filterScheduleList = FilterSchedule.GetByFilterSegmentationID(clientconnections, args.FilterSegmentationId);
                foreach (var filterSchedule in filterScheduleList)
                {
                    if (groupList.Any(g => g.FilterSegmentationID == filterSchedule.FilterSegmentationID &&
                                   string.Join(CommaString, g.FilterGroupID_Selected.Select(fg => fg.ToString().Trim()))
                                   == string.Join(CommaString, filterSchedule.FilterGroupID_Selected.Select(fg => fg.ToString().Trim())) &&
                                   string.Join(CommaString, g.FilterGroupID_Suppressed.Select(fg => fg.ToString().Trim()))
                                   == string.Join(CommaString, filterSchedule.FilterGroupID_Suppressed.Select(fg => fg.ToString().Trim())) &&
                                   g.SelectedOperation == filterSchedule.SelectedOperation &&
                                   g.SuppressedOperation == filterSchedule.SuppressedOperation))
                    {
                        args.IsFilterSegmentScheduled = true;
                        break;
                    }
                }
            }

            return args;
        }

        private string GetMessageIfFilterExists(int filterId)
        {
            var message = string.Empty;

            if (MDFilter.ExistsByFilterName(clientconnections, filterId, txtFilterName.Text.Trim()))
            {
                message = "The filter Name you entered already exists. Please save under a different name.</br>";
            }

            int id;
            int.TryParse(hfFilterSegmentationID.Value, out id);
            if (new FrameworkUAD.BusinessLogic.FilterSegmentation().ExistsByIDName(id, txtFSName.Text.Trim(), clientconnections))
            {
                message += "The filter segmentation entered already exists. Please save under a different name.";
            }

            return message;
        }

        protected void btnCloseFS_Click(object sender, EventArgs e)
        {
            ResetControls();
            hideFilterSegmentationPopup.DynamicInvoke();
            mdlPopSaveFS.Hide();
        }

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        protected void rbNewExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            rddtFilterCategory.Enabled = true;
            ImgFilterList.Visible = false;

            if (rbNewExisting.SelectedValue.Equals("New", StringComparison.OrdinalIgnoreCase))
            {
                Mode = "AddNew";
                btnSaveFS.OnClientClick = null;
                txtFilterName.ReadOnly = false;
                txtFSName.ReadOnly = false;
            }
            else
            {
                btnSaveFS.OnClientClick = "if(!confirm('Are you sure you want overwrite existing filter segmentation?')) return false;";
                txtFilterName.ReadOnly = false;
                txtFSName.ReadOnly = false;

                if (!Mode.Equals("Edit", StringComparison.OrdinalIgnoreCase))
                {
                    Mode = "AddExisting";
                    txtFilterName.ReadOnly = true;
                    txtFSName.ReadOnly = true;
                    rddtFilterCategory.Enabled = false;
                    ImgFilterList.Visible = true;
                }
            }

            txtFilterName.Text = string.Empty;
            txtFSName.Text = string.Empty;
            rddtFilterCategory.Entries.Clear();
            txtNotes.Text = string.Empty;
        }

        protected void ImgFilterList_Click(object sender, EventArgs e)
        {
            FiltersList.Visible = true;
            FiltersList.FilterRadioButtonOptions = Enums.LoadFilterOptions.FilterSegmentations;
            FiltersList.PubID = PubID;
            FiltersList.BrandID = BrandID;
            FiltersList.ViewType = ViewType;
            FiltersList.UserID = UserID;
            FiltersList.Mode = Mode;
            FiltersList.LoadControls();
        }

        private void FiltersListHide()
        {
            FiltersList.Visible = false;
        }

        public void LoadFilterSegmentationData(int filtersegmentationID)
        {
            try
            {
                var filterSegmentation = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(filtersegmentationID, new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient));
                var filter = MDFilter.GetByID(clientconnections, filterSegmentation.FilterID);

                txtFilterName.Text = filter.Name;
                rddtFilterCategory.SelectedValue = filter.FilterCategoryID.ToString();
                txtFSName.Text = filterSegmentation.FilterSegmentationName;
                txtNotes.Text = filterSegmentation.Notes;
                hfFilterSegmentationID.Value = filterSegmentation.FilterSegmentationID.ToString();
            }
            catch (Exception ex)
            {
                Utilities.Log_Error(Request.RawUrl.ToString(), "LoadFilterSegmentationData", ex);
                DisplayError(ex.Message);
            }
        }
    }
}