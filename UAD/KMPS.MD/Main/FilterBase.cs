using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD.Main
{
    public abstract class FilterBase: BrandsPageBase
    {
        private const string SortDirectionFSKey = "SortDirectionFS";
        private const string SortFieldFSKey = "SortFieldFS";

        protected abstract void ResetControls();
        public abstract void LoadGrid();
        public abstract void LoadFilterSegmentationGrid();
        protected abstract string DefaultSearchLabel { get; }
        protected abstract Label SearchLabel { get; }
        protected abstract RadioButtonList ListType { get; }
        protected abstract PlaceHolder FiltersPlaceHolder { get; }
        protected abstract PlaceHolder FilterSegmentationsPlaceHolder { get; }

        protected void rblListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();

            if (ListType.SelectedValue == Enums.ListType.Filters.ToString())
            {
                SearchLabel.Text = DefaultSearchLabel;
                LoadGrid();
                FiltersPlaceHolder.Visible = true;
                FilterSegmentationsPlaceHolder.Visible = false;
            }
            else
            {
                SearchLabel.Text = "Filter Name or Filter Segmentation";
                LoadFilterSegmentationGrid();
                FiltersPlaceHolder.Visible = false;
                FilterSegmentationsPlaceHolder.Visible = true;
            }
        }

        protected string SortFieldFS
        {
            get
            {
                return ViewState[SortFieldFSKey].ToString();
            }
            set
            {
                ViewState[SortFieldFSKey] = value;
            }
        }

        protected string SortDirectionFS
        {
            get
            {
                return ViewState[SortDirectionFSKey].ToString();
            }
            set
            {
                ViewState[SortDirectionFSKey] = value;
            }
        }
    }
}