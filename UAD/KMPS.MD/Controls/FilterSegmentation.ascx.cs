using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Text;

namespace KMPS.MD.Controls
{
    public partial class FilterSegmentation : System.Web.UI.UserControl
    {
        public event CommandEventHandler lnkCountCommand;
        public event CommandEventHandler lnkCompanyLocationViewCommand;
        public event CommandEventHandler lnkCrossTabReportCommand;
        public event CommandEventHandler lnkDimensionReportCommand;
        public event CommandEventHandler lnkEmailViewCommand;
        public event CommandEventHandler lnkGeoMapsCommand;
        public event CommandEventHandler lnkGeoReportCommand;
        FilterViews fv;

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        FilterSegmentationSave fsSaveControl;
        delegate void HidePanel();
        delegate void LoadSavedFilterSegmentationName(string filtersegmentationname);

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

        private string fvSessionName
        {
            get
            {
                if (ViewState["fvSessionName"] == null)
                {
                    ViewState["fvSessionName"] = "filterview" + Guid.NewGuid();
                }

                return ViewState["fvSessionName"].ToString();
            }
            set
            {
                ViewState["fvSessionName"] = value;
            }
        }

        public int BrandID
        {
            get
            {
                try
                {
                    return (int)this.ViewState["BrandID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["BrandID"] = value;
            }
        }

        public int PubID
        {
            get
            {
                try
                {
                    return (int)this.ViewState["PubID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["PubID"] = value;
            }
        }

        public int UserID
        {
            get
            {
                try
                {
                    return (int)this.ViewState["UserID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["UserID"] = value;
            }
        }
 
        public Enums.ViewType ViewType
        {
            get
            {
                try
                {
                    return (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), this.ViewState["ViewType"].ToString());
                }
                catch
                {
                    return Enums.ViewType.None;
                }
            }
            set
            {
                this.ViewState["ViewType"] = value;
            }
        }

        private static Page CurrentPage
        {
            get
            {
                try
                {
                    return (Page)HttpContext.Current.Handler;
                }
                catch
                {
                    return null;
                }
            }
        }
        private ECNSession _usersession = null;

        public ECNSession UserSession
        {
            get
            {
                return _usersession == null ? ECNSession.CurrentSession() : _usersession;
            }
        }

        private KMPlatform.Object.ClientConnections _clientconnections = null;
        public KMPlatform.Object.ClientConnections clientconnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    KMPlatform.Entity.Client client = new KMPlatform.BusinessLogic.Client().Select(UserSession.ClientID, true);
                    _clientconnections = new KMPlatform.Object.ClientConnections(client);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }

        private void FilterSegmentationSavePopupHide()
        {
            fsSaveControl.Visible = false;
        }

        public int FilterSegmentationID
        {
            get
            {
                try
                {
                    return (int)this.ViewState["FilterSegmentationID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["FilterSegmentationID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
            fsSaveControl = this.NamingContainer.FindControl("FilterSegmentationSave") as FilterSegmentationSave;
            divErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;

            divMessage.Visible = false;
            lblMessage.Text = string.Empty;

            fv = FilterViewCollection;

            if (FilterCollection != null)
            {
                if (FilterCollection.Count > 1)
                {
                    btnSaveFilterSegmentation.Visible = true;
                }
            }

            pnlFilterSegmentation.Visible = true;

            HidePanel deFSNoParam = new HidePanel(FilterSegmentationSavePopupHide);
            this.fsSaveControl.hideFilterSegmentationPopup = deFSNoParam;

            LoadSavedFilterSegmentationName delNoParamFilterSegmentationName = new LoadSavedFilterSegmentationName(LoadFilterSegmentationName);
            this.fsSaveControl.LoadSavedFilterSegmentationName = delNoParamFilterSegmentationName;

            if (!IsPostBack)
            {

            }
            else
            {
                if (fv == null || fv.Count() == 0)
                {
                    if (grdFilterSegmentationCounts.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdFilterSegmentationCounts.Rows.Count; i++)
                        {
                            if (grdFilterSegmentationCounts.Rows[i].RowType == DataControlRowType.DataRow)
                            {
                                LinkButton lnkCount = (LinkButton)grdFilterSegmentationCounts.Rows[i].FindControl("lnkCount");
                                HiddenField hfFilterViewNo = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfFilterViewNo");
                                HiddenField hfFilterViewName = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfFilterViewName");
                                HiddenField hfFilterDescription = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfFilterDescription");
                                HiddenField hfSelectedFilterNo = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSelectedFilterNo");
                                HiddenField hfSuppressedFilterNo = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSuppressedFilterNo");
                                HiddenField hfSelectedFilterOperation = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSelectedFilterOperation");
                                HiddenField hfSuppressedFilterOperation = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSuppressedFilterOperation");

                                filterView f = new filterView();
                                f.FilterViewNo = Convert.ToInt32(hfFilterViewNo.Value);
                                f.FilterViewName = hfFilterViewName.Value;
                                f.FilterDescription = hfFilterDescription.Value;
                                f.SelectedFilterNo = hfSelectedFilterNo.Value;
                                f.SuppressedFilterNo = hfSuppressedFilterNo.Value;
                                f.SelectedFilterOperation = hfSelectedFilterOperation.Value;
                                f.SuppressedFilterOperation = hfSuppressedFilterOperation.Value;
                                f.Count = Convert.ToInt32(lnkCount.Text);

                                fv.Add(f);
                            }
                        }
                    }
                }
            }

            if(fv.Count > 0)
            {
                HiddenField hfHasFilterSegmentation = (HiddenField)Parent.Parent.Parent.Parent.FindControl("hfHasFilterSegmentation");
                hfHasFilterSegmentation.Value = "true";
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grdFilterSegmentationCounts.Rows.Count > 1)
            {
                if (ctrlVenn.VennParams != string.Empty)
                {
                    if (ScriptManager.GetCurrent(CurrentPage).IsInAsyncPostBack)
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), this.ctrlVenn.VennDivID, "renderVenn('#" + this.ctrlVenn.VennDivID + "', [" + ctrlVenn.VennParams + "]);", true);

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), this.ctrlVenn.VennDivID, "renderVenn('#" + this.ctrlVenn.VennDivID +"', [" + ctrlVenn.VennParams + "]);", true);
                    }
                    else
                    {
                        CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), this.ctrlVenn.VennDivID, "renderVenn('#" + this.ctrlVenn.VennDivID + "', [" + ctrlVenn.VennParams + "]);", true);
                    }
                }
            }
        }
        private void DisplayError(string errorMessage)
        {
            lblErrMsg.Text = errorMessage;
            divErrMsg.Visible = true;
        }

        protected void btnFilterSegmentation_Click(object sender, EventArgs e)
        {
            if (lstSuppressedFilters.GetSelectedIndices().Length < 1 && lstSelectedFilters.GetSelectedIndices().Length < 2)
            {
                divErrMsg.Visible = true;
                lblErrMsg.Text = "Please select more than one Data Segments";
                return;
            }

            List<string> lSelected = new List<string>();
            List<string> lSuppressed = new List<string>();
            string DataSegments = string.Empty;

            lSelected = lstSelectedFilters.GetSelectedIndices()
                .Select(i => lstSelectedFilters.Items[i].Text)
                .ToList();

            lSuppressed = lstSuppressedFilters.GetSelectedIndices()
                .Select(i => lstSuppressedFilters.Items[i].Text)
                .ToList();

            var difference = lSelected.Intersect(lSuppressed);
            foreach (var item in difference)
                DataSegments += DataSegments == string.Empty ? item : ", " + item;

            if (DataSegments != string.Empty && ((lSelected.Count == 1 && lSuppressed.Count == 1)  || (rblSelectedFilterOperation.SelectedValue == rblSuppressedFilterOperation.SelectedValue && (lSelected.SequenceEqual(lSuppressed)))))
            {
                divErrMsg.Visible = true;
                lblErrMsg.Text = DataSegments + " cannot be added in In and NotIn list.";
                return;
            }

            List<string> Selected_FilterNos = new List<string>();
            List<string> Suppressed_FilterNos = new List<string>();
            string Selected_Segments = string.Empty;
            string Suppressed_Segments = string.Empty;

            foreach (ListItem item in lstSelectedFilters.Items)
            {
                if (item.Selected)
                {
                    Selected_FilterNos.Add(item.Value);
                    Selected_Segments += (Selected_Segments == string.Empty) ? item.Text.Split(new string[] { "Data Segment" }, StringSplitOptions.None).Last<string>().Replace('\t', ' ') : ("," + item.Text.Split(new string[] { "Data Segment" }, StringSplitOptions.None).Last<string>().Replace('\t', ' '));
                }
            }
            foreach (ListItem item in lstSuppressedFilters.Items)
            {
                if (item.Selected)
                {
                    Suppressed_FilterNos.Add(item.Value);
                    Suppressed_Segments += (Suppressed_Segments == string.Empty) ? item.Text.Split(new string[] { "Data Segment" }, StringSplitOptions.None).Last<string>().Replace('\t', ' ') : ("," + item.Text.Split(new string[] { "Data Segment" }, StringSplitOptions.None).Last<string>().Replace('\t', ' '));
                }
            }

            filterView fv1 = new filterView();
            fv1.FilterViewNo = fv.Count + 1;
            fv1.SelectedFilterNo = string.Join(",", Selected_FilterNos);
            fv1.SelectedFilterOperation = rblSelectedFilterOperation.SelectedValue;
            fv1.SuppressedFilterNo = string.Join(",", Suppressed_FilterNos);
            if(fv1.SuppressedFilterNo !=  string.Empty)
                fv1.SuppressedFilterOperation = rblSuppressedFilterOperation.SelectedValue;

            if (lstSuppressedFilters.GetSelectedIndices().Count() < 1)
            {
                fv1.FilterDescription = string.Join(",", Selected_FilterNos) + "(" + rblSelectedFilterOperation.SelectedValue + ")";
            }
            else
            {
                string Selected_Segments_Label = string.Join(",", Selected_FilterNos).Contains(",").ToString() == "True" ? "(" + rblSelectedFilterOperation.SelectedValue + ")" : "";
                string Suppressed_Segments_Label = string.Join(",", Suppressed_FilterNos).Contains(",").ToString() == "True" ? "(" + rblSuppressedFilterOperation.SelectedValue + ")" : "";

                fv1.FilterDescription = string.Join(",", Selected_FilterNos) + Selected_Segments_Label + " Not In " + string.Join(",", Suppressed_FilterNos) + Suppressed_Segments_Label;
            }


            for (int i = 0; i < grdFilterSegmentationCounts.Rows.Count; i++)
            {
                if (grdFilterSegmentationCounts.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    //HiddenField hfFilterDescription = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfFilterDescription");
                    HiddenField hfSelectedFilterNo = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSelectedFilterNo");
                    HiddenField hfSuppressedFilterNo = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSuppressedFilterNo");
                    HiddenField hfSelectedFilterOperation = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSelectedFilterOperation");
                    HiddenField hfSuppressedFilterOperation = (HiddenField)grdFilterSegmentationCounts.Rows[i].FindControl("hfSuppressedFilterOperation");

                    if (hfSelectedFilterNo.Value == fv1.SelectedFilterNo && hfSelectedFilterOperation.Value == fv1.SelectedFilterOperation && hfSuppressedFilterNo.Value == fv1.SuppressedFilterNo && hfSuppressedFilterOperation.Value == fv1.SuppressedFilterOperation)
                    {
                        divErrMsg.Visible = true;
                        lblErrMsg.Text = "Filter segment already added.";
                        return;
                    }
                }
            }

            grdFilterSegmentationCounts.DataSource = null;
            grdFilterSegmentationCounts.DataBind();
            
            fv.Add(fv1);

            fv.Execute(FilterCollection);

            FilterViewCollection = fv;

            LoadGrid();

            lstSuppressedFilters.ClearSelection();
            lstSelectedFilters.ClearSelection();
        }

        private void LoadGrid()
        {
            grdFilterSegmentationCounts.DataSource = fv;
            grdFilterSegmentationCounts.DataBind();

            if (fv.Count > 1)
            {
                ctrlVenn.CreateVennForFilterSegmentation(fv);
            }

            if (fv.Count >= 10)
            {
                lblErrMsg.Text = "Only up to 10 filter segments can be added";
                divErrMsg.Visible = true;
                btnFilterSegmentation.Enabled = false;
            }
            else
            {
                lblErrMsg.Text = "";
                divErrMsg.Visible = false;
                btnFilterSegmentation.Enabled = true;
            }

            if (ViewType == Enums.ViewType.CrossProductView)
            {
                if (grdFilterSegmentationCounts.Rows.Count > 0)
                {
                    for (int i = 0; i < grdFilterSegmentationCounts.Rows.Count; i++)
                    {
                        if (grdFilterSegmentationCounts.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            Image imgReport = (Image)grdFilterSegmentationCounts.Rows[i].FindControl("imgReport");
                            imgReport.Visible = false;
                        }
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void grdFilterSegmentation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdFilterValues = (GridView)e.Row.FindControl("grdFilterValues");
                List<Field> grdFilterList = LoadGridFilterValues(Convert.ToInt32(grdFilterSegmentation.DataKeys[e.Row.RowIndex].Value.ToString()));
                grdFilterValues.DataSource = grdFilterList.Distinct().ToList();
                grdFilterValues.DataBind();
            }
        }
        private List<Field> LoadGridFilterValues(int filterNo)
        {
            Filter filter = FilterCollection.SingleOrDefault(f => f.FilterNo == filterNo);
            return filter.Fields;
        }

        protected void grdFilterSegmentationCounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hfSelectedFilterNo = (HiddenField)grdFilterSegmentationCounts.Rows[e.RowIndex].FindControl("hfSelectedFilterNo");
            HiddenField hfSuppressedFilterNo = (HiddenField)grdFilterSegmentationCounts.Rows[e.RowIndex].FindControl("hfSuppressedFilterNo");
            HiddenField hfSelectedFilterOperation = (HiddenField)grdFilterSegmentationCounts.Rows[e.RowIndex].FindControl("hfSelectedFilterOperation");
            HiddenField hfSuppressedFilterOperation = (HiddenField)grdFilterSegmentationCounts.Rows[e.RowIndex].FindControl("hfSuppressedFilterOperation");

            filterView filterview = fv.SingleOrDefault(x => x.SelectedFilterNo == hfSelectedFilterNo.Value && x.SelectedFilterOperation == hfSelectedFilterOperation.Value && x.SuppressedFilterNo == hfSuppressedFilterNo.Value && x.SuppressedFilterOperation == hfSuppressedFilterOperation.Value);
            fv.Remove(filterview);
            fv.Execute(FilterCollection);
            FilterViewCollection = fv;



            LoadGrid();
        }

        private void setDownloadData(string value)
        {
            string[] args = value.Split('/');
            (this.Page as dynamic).lblSelectedFilterNos.Text = args[0];
            (this.Page as dynamic).lblSuppressedFilterNos.Text = args[1];
            (this.Page as dynamic).lblSelectedFilterOperation.Text = args[2];
            (this.Page as dynamic).lblSuppressedFilterOperation.Text = args[3];
            (this.Page as dynamic).lblFilterCombination.Text = args[4];
            (this.Page as dynamic).lblDownloadCount.Text = args[5];
        }

        protected void lnkCompanyLocationView_Command(object sender, CommandEventArgs e)
        {
            if (lnkCompanyLocationViewCommand != null)
            {
                lnkCompanyLocationViewCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        protected void lnkCount_Command(object sender, CommandEventArgs e)
        {
            if (lnkCountCommand != null)
            {
                lnkCountCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        protected void lnkCrossTabReport_Command(object sender, CommandEventArgs e)
        {
            if (lnkCrossTabReportCommand != null)
            {
                lnkCrossTabReportCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        protected void lnkDimensionReport_Command(object sender, CommandEventArgs e)
        {
            if (lnkDimensionReportCommand != null)
            {
                lnkDimensionReportCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        protected void lnkEmailView_Command(object sender, CommandEventArgs e)
        {
            if (lnkEmailViewCommand != null)
            {
                lnkEmailViewCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        protected void lnkGeoMaps_Command(object sender, CommandEventArgs e)
        {
            if (lnkGeoMapsCommand != null)
            {
                lnkGeoMapsCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        protected void lnkGeoReport_Command(object sender, CommandEventArgs e)
        {
            if (lnkGeoReportCommand != null)
            {
                lnkGeoReportCommand.DynamicInvoke(new object[] { sender, e });
            }
        }

        public void LoadControls()
        {
            if ((FilterCollection != null))
            {
                int num = 1;
                foreach (Filter filter in FilterCollection)
                {
                    filter.FilterGroupName = "Data Segment" + num + (filter.FilterName == string.Empty || filter.FilterName == null ? "" : "_" + filter.FilterName);
                    num++;
                }
                grdFilterSegmentation.DataSource = FilterCollection;
                grdFilterSegmentation.DataBind();

                lstSelectedFilters.Items.Clear();
                lstSuppressedFilters.Items.Clear();
                btnFilterSegmentation.Enabled = true;

                foreach (Filter filter in FilterCollection)
                {
                    lstSelectedFilters.Items.Add(new ListItem(filter.FilterGroupName.ToString(), filter.FilterNo.ToString()));
                    lstSuppressedFilters.Items.Add(new ListItem(filter.FilterGroupName.ToString(), filter.FilterNo.ToString()));
                }

                grdFilterSegmentationCounts.DataSource = null;
                grdFilterSegmentationCounts.DataBind();
            }
        }

        public void ResetControls()
        {
            pnlFilterSegmentation.Visible = false;
            grdFilterSegmentation.DataSource = null;
            grdFilterSegmentation.DataBind();
            rblSelectedFilterOperation.SelectedIndex = 0;
            rblSuppressedFilterOperation.SelectedIndex = 0;
            lstSelectedFilters.Items.Clear();
            lstSuppressedFilters.Items.Clear();
            grdFilterSegmentationCounts.DataSource = null;
            grdFilterSegmentationCounts.DataBind();
            FilterViewCollection.Clear();
            btnFilterSegmentation.Enabled = true;
            HiddenField hfHasFilterSegmentation = (HiddenField)Parent.Parent.Parent.Parent.FindControl("hfHasFilterSegmentation");
            hfHasFilterSegmentation.Value = "false";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            rblSelectedFilterOperation.SelectedIndex = 0;
            rblSuppressedFilterOperation.SelectedIndex = 0;
            lstSelectedFilters.ClearSelection();
            lstSuppressedFilters.ClearSelection();
            grdFilterSegmentationCounts.DataSource = null;
            grdFilterSegmentationCounts.DataBind();
            FilterViewCollection.Clear();
            btnFilterSegmentation.Enabled = true;
        }

        protected void btnOpenSaveFSPopup_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.UADFilter, KMPlatform.Enums.Access.Edit))
            {
                bool ischecked = false;
                string FilterViewNos = string.Empty;
                int i = 0;
                foreach (GridViewRow r in grdFilterSegmentationCounts.Rows)
                {
                    CheckBox cb = r.FindControl("cbSelectFilter") as CheckBox;

                    if (cb != null && cb.Checked)
                    {
                        ischecked = true;
                        FilterViewNos += FilterViewNos == string.Empty ? grdFilterSegmentationCounts.DataKeys[i].Values["FilterViewNo"].ToString() : "," + grdFilterSegmentationCounts.DataKeys[i].Values["FilterViewNo"].ToString();
                    }
                    i++;
                }

                if (!ischecked)
                {
                    DisplayError("Please select a filter segmentation to save.");
                    return;
                }

                // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
                fsSaveControl.Mode = string.IsNullOrWhiteSpace(fsSaveControl.Mode) ? "AddNew" : fsSaveControl.Mode;
                fsSaveControl.BrandID = BrandID;
                fsSaveControl.PubID = PubID;
                fsSaveControl.UserID = UserID;
                fsSaveControl.fvSessionName = fvSessionName;
                fsSaveControl.FilterCollection = FilterCollection;
                fsSaveControl.FilterViewCollection = fv;
                fsSaveControl.FilterViewNos = FilterViewNos;
                fsSaveControl.ViewType = ViewType;
                fsSaveControl.LoadControls();
                fsSaveControl.Visible = true;
           }
        }

        public void LoadFilterSegmentationName(string filtersegmentationname)
        {
            foreach (GridViewRow r in grdFilterSegmentationCounts.Rows)
            {
                CheckBox cb = r.FindControl("cbSelectFilter") as CheckBox;
                int filterviewNo = Convert.ToInt32(grdFilterSegmentationCounts.DataKeys[r.RowIndex].Value.ToString());

                if (cb != null && cb.Checked)
                {
                    filterView fview = fv.First(f => f.FilterViewNo == filterviewNo);
                    fview.FilterViewName = filtersegmentationname;
                    fv.Update(fview);

                    cb.Checked = false;
                }
            }

            FilterViewCollection = fv;
            LoadGrid();
        }

        public void LoadFilterSegmenationData()
        {
            btnSaveFilterSegmentation.Visible = true;
            FrameworkUAD.Entity.FilterSegmentation fs = new FrameworkUAD.BusinessLogic.FilterSegmentation().SelectByID(FilterSegmentationID, new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), true);

            FilterViews fv = FilterViewCollection;

            foreach (FrameworkUAD.Entity.FilterSegmentationGroup f in fs.FilterSegmentationGroupList)
            {
                filterView fv1 = new filterView();
                fv1.FilterViewNo = fv.Count + 1;
                fv1.FilterViewName = FilterCollection.FirstOrDefault().FilterName + " - " + fs.FilterSegmentationName;

                string selectedfilterNos = string.Empty;
                string suppressedfilterNos = string.Empty;

                foreach (int s in f.FilterGroupID_Selected)
                {
                    string fNo = FilterCollection.FirstOrDefault(x => x.FilterGroupID == s).FilterNo.ToString();
                    selectedfilterNos += (selectedfilterNos == string.Empty ? "" : ",") + fNo;
                }

                fv1.SelectedFilterNo = selectedfilterNos;

                foreach (int s in f.FilterGroupID_Suppressed)
                {
                    string fNo = FilterCollection.FirstOrDefault(x => x.FilterGroupID == s).FilterNo.ToString();
                    suppressedfilterNos += (suppressedfilterNos == string.Empty ? "" : ",") + fNo;
                }

                fv1.SuppressedFilterNo = suppressedfilterNos;
                fv1.SelectedFilterOperation = f.SelectedOperation;
                fv1.SuppressedFilterOperation = f.SuppressedOperation;

                if (fv1.SuppressedFilterNo == "")
                {
                    fv1.FilterDescription = selectedfilterNos + "(" + fv1.SelectedFilterOperation + ")";
                }
                else
                {
                    string Selected_Segments_Label = fv1.SelectedFilterNo.Contains(",").ToString() == "" ? "(" + fv1.SelectedFilterOperation + ")" : "";
                    string Suppressed_Segments_Label = fv1.SuppressedFilterNo.Contains(",").ToString() == "" ? "(" + fv1.SuppressedFilterOperation + ")" : "";
                    fv1.FilterDescription = selectedfilterNos + Selected_Segments_Label + " Not In " + suppressedfilterNos + Suppressed_Segments_Label;
                }

                fv.Add(fv1);
            }

            grdFilterSegmentationCounts.DataSource = null;
            grdFilterSegmentationCounts.DataBind();
            
            fv.Execute(FilterCollection);
            FilterViewCollection = fv;

            grdFilterSegmentationCounts.DataSource = fv;
            grdFilterSegmentationCounts.DataBind();

            if (fv.Count > 1)
            {
                ctrlVenn.CreateVennForFilterSegmentation(fv);
            }

            if (ViewType == Enums.ViewType.CrossProductView)
            {
                if (grdFilterSegmentationCounts.Rows.Count > 0)
                {
                    for (int i = 0; i < grdFilterSegmentationCounts.Rows.Count; i++)
                    {
                        if (grdFilterSegmentationCounts.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            Image imgReport = (Image)grdFilterSegmentationCounts.Rows[i].FindControl("imgReport");
                            imgReport.Visible = false;
                        }
                    }
                }
            }

            pnlFilterSegmentation.Visible = true;
        }

        public void UpdateFiltersegmentationCounts()
        {
            fv.Execute(FilterCollection);
            FilterViewCollection = fv;

            grdFilterSegmentationCounts.DataSource = fv;
            grdFilterSegmentationCounts.DataBind();
        }
    }
}