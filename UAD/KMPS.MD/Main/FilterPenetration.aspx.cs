using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using RKLib.ExportData;

namespace KMPS.MD.Main
{
    public partial class FilterPenetration : BrandsPageBase
    {
        Filters fc;
        int iRowsCount = 0;
        int FilterSubtotal = 0;
        delegate void RebuildSubscriberList();
        delegate void HidePanel();
        public Dictionary<int, int> dSummary = new Dictionary<int, int>();
        string ChannelID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Filters";
            Master.SubMenu = "Filter Comparison";
            fc = new Filters(Master.clientconnections, Master.LoggedInUser);
            lblErrorMsg.Text = string.Empty;
            divErrorMsg.Visible = false;
            divPopupMessage.Visible = false;
            lblPopupMessage.Text = string.Empty;

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(btnDownload);
            scriptManager.RegisterPostBackControl(btnDownloadSummary);

            RebuildSubscriberList delNoParam = new RebuildSubscriberList(Reload);
            this.DownloadPanel1.DelMethod = delNoParam;

            HidePanel delNoParam1 = new HidePanel(Hide);
            this.DownloadPanel1.hideDownloadPopup = delNoParam1;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Edit))
                {
                    btnSaveReport.Visible = false;
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Delete))
                {
                    btnDelReport.Visible = false;
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Download))
                {
                    btnDownload.Visible = false;
                    btnDownloadSummary.Visible = false;
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.DownloadDetails))
                {
                    btnDownloadDetails.Visible = false;
                }

                LoadBrands();

                DownloadPanel1.Showexporttoemailmarketing = true;
                DownloadPanel1.Showsavetocampaign = true;
                DownloadPanel1.error = false;
                DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
            }
            else
            {
                LoadGrid();
            }
        }

        private void LoadFilters()
        {
            lstFilter.Items.Clear();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                lstFilter.DataSource = MDFilter.GetByBrandID_TypeAddedinName(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                lstFilter.DataSource = MDFilter.GetNotInBrand_TypeAddedinName(Master.clientconnections);

            lstFilter.DataBind();
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            Session["fc"] = null;
            Session["FilterPenetration"] = null;
            drpdownReports.ClearSelection();

            if (lstFilter.SelectedValue == "")
                DisplayError("Please select Filter");
            else
            {
                if (lstFilter.GetSelectedIndices().Length > 10)
                    DisplayError("Please select no more than 10 filters.");
                else
                    LoadGrid();
            }
        }

        private void LoadGrid()
        {
            grdCrossTab.Visible = false;
            dSummary.Clear();
            grdSummary.DataSource = null;
            grdSummary.DataBind();
            TabContainer1.Visible = false;

            try
            {
                if (lstFilter.GetSelectedIndices().Length <= 10)
                {
                    if (lstFilter.SelectedValue != "")
                    {
                        TabContainer1.Visible = true;
                        GetPenetrationData();
                        if (Session["FilterPenetration"] != null)
                        {
                            DataTable dt = (DataTable)Session["FilterPenetration"];

                            iRowsCount = dt.Rows.Count;

                            if (iRowsCount == 0)
                            {
                                TabContainer1.Visible = false;
                                DisplayError("Selected filter contains no records");
                                return;
                            }
                            else
                            {
                                grdCrossTab.DataSource = dt;
                                grdCrossTab.DataBind();

                                grdCrossTab.Visible = true;
                            }
                        }
                        divErrorMsg.Visible = false;
                    }
                }
            }
            catch (FilterNoRecordsException)
            {
                DisplayError("No Records");
            }
            catch (DuplicateFilterException)
            {
                DisplayError("Duplicate filter.");
            }
            catch (Exception ex)
            {
                DisplayError("Error:" + ex.Message);
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMsg.Text = errorMessage;
            divErrorMsg.Visible = true;
        }

        private void GetPenetrationData()
        {
            if (Session["FilterPenetration"] == null)
            {
                fc.Clear();

                List<string> columnList = new List<string>();
                columnList.Add("s.SubscriptionID|Default");

                foreach (ListItem mylistvalue in lstFilter.Items)
                {
                    if (mylistvalue.Selected)
                    {
                        Filters fcIndividualfilter;
                        Filter  fcSinglefilter = new Filter();
                        try
                        {
                            fcIndividualfilter = MDFilter.LoadFilters(Master.clientconnections, Convert.ToInt32(mylistvalue.Value), Master.LoggedInUser);
                            if (fcIndividualfilter.Count > 1)
                            {
                                List<string> Selected_FilterID = new List<string>();
                                string selectedFilterNo = string.Empty;
                                int BrandID = 0;
                                string filterName = string.Empty;
                                int FilterNo = 0;

                                foreach (Filter f in fcIndividualfilter)
                                {
                                    filterName = f.FilterName;
                                    BrandID = f.BrandID;
                                    FilterNo = f.FilterNo;
                                    selectedFilterNo += selectedFilterNo == string.Empty ? f.FilterNo.ToString() : "," + f.FilterNo.ToString();
                                }

                                StringBuilder Queries = Filter.generateCombinationQuery(fcIndividualfilter, "union", "", selectedFilterNo, "", "", 0, BrandID, Master.clientconnections);

                                DataTable dtSubscription = Subscriber.GetSubscriberData(Master.clientconnections, Queries, columnList, new List<string>(), new List<string>(), new List<string>(), new List<string>(), BrandID, new List<int>(), false, 0);

                                List<Subscriber> Subscriberlist = new List<Subscriber>();

                                Subscriberlist = dtSubscription.AsEnumerable()
                                        .Select(row => new Subscriber
                                        {
                                            SubscriptionID = row.Field<int>(0),
                                        }).ToList();

                                fcSinglefilter.BrandID = BrandID;
                                fcSinglefilter.FilterName = filterName;
                                fcSinglefilter.FilterNo = FilterNo;
                                fcSinglefilter.Count = Subscriberlist.Count();

                                var query = Subscriberlist
                                             .GroupBy(x => new { x.SubscriptionID })
                                             .Select(g => g.First());
                                fcSinglefilter.Subscriber = query.ToList();

                                fcSinglefilter.Executed = true;

                                fcSinglefilter.FilterNo = fc.Count + 1;
                                fc.Add(fcSinglefilter);
                            }
                            else
                            {
                                fcIndividualfilter.FirstOrDefault().FilterNo = fc.Count + 1;
                                fc.Add(fcIndividualfilter.FirstOrDefault());
                            }
                        }
                        catch (Exception ex)
                        {
                            DisplayError(ex.Message);
                        }
                    }
                }

                if (fc.Count > 0)
                {
                    Session["fc"] = fc;

                    DataTable dt = fc.GetCrossTabData("Filters");
                    Session["FilterPenetration"] = dt;
                }
            }
        }

        protected void lnkReport_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblCombination.Text = args[0];
            DetailsDownload(Int32.Parse(args[1]));
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();
            hfBrandID.Value = drpBrand.SelectedValue;

            if (Convert.ToInt32(drpBrand.SelectedValue) >= 0)
            {
                LoadFilters();
                loadsavedreports();
                TabContainer1.Visible = false;
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
        }

        protected void ResetControls()
        {
            hfBrandID.Value = "0";
            lstFilter.Items.Clear();
            drpdownReports.Items.Clear();
            TabContainer1.Visible = false;
        }

        private void Reload()
        {
            LoadGrid();
            fc = (Filters)Session["fc"];
            List<int> SubscriberList = fc.DownloadCrossTabData(lblCombination.Text);
        }

        private void Hide()
        {
            DownloadPanel1.Visible = false;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Download))
            {
                if (lstFilter.SelectedValue != "")
                {
                    GetPenetrationData();
                    if (Session["FilterPenetration"] != null)
                    {
                        DataTable dt = (DataTable)Session["FilterPenetration"];

                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            dt.Columns[i].ColumnName = Regex.Replace(dt.Columns[i].ColumnName, "[^0-9A-Za-z]+", "_");

                            dt.Columns[i].ColumnName = Regex.Replace(dt.Columns[i].ColumnName, @"^[\d-]*", ""); //remove numbers starting in columnName

                        }

                        DataTable dt2 = new DataTable();
                        dt2 = dt.Copy();
                        dt2.Columns.RemoveAt(0);
                        if (dt.Rows.Count > 0)
                        {
                            // Export all the details to xls
                            RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                            objExport.ExportDetails(dt2, Export.ExportFormat.Excel, "FilterComparisonDetails.xls");
                        }
                    }
                }
            }

        }

        private void DetailsDownload(int DownloadCount)
        {
            if (DownloadCount <= 0)
            {
                DownloadPanel1.error = true;
                DownloadPanel1.errormsg = "Download Count must be greater than 0";
                DownloadPanel1.Visible = true;
                DownloadPanel1.isError();
            }

            if (Session["fc"] == null)
            {
                LoadGrid();
            }

            fc = (Filters)Session["fc"];

            List<int> SubscriberList = fc.DownloadCrossTabData(lblCombination.Text);
            DownloadPanel1.SubscriptionID = SubscriberList;
            DownloadPanel1.SubscribersQueries = null;
            DownloadPanel1.Visible = true;
            DownloadPanel1.downloadCount = DownloadCount;
            DownloadPanel1.ViewType = Enums.ViewType.ConsensusView;
            DownloadPanel1.EnableCbIsRecentData = true;
            DownloadPanel1.VisibleCbIsRecentData = true;
            DownloadPanel1.LoadControls();
            DownloadPanel1.ValidateExportPermission();
            DownloadPanel1.LoadDownloadTemplate();
            DownloadPanel1.loadExportFields();
        }

        protected void btnDownloadDetails_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.DownloadDetails))
            {
                if (lstFilter.SelectedValue != "")
                {
                    int Counts = 0;
                    foreach (GridViewRow r in grdCrossTab.Rows)
                    {
                        Counts += Convert.ToInt32(grdCrossTab.DataKeys[r.RowIndex].Values[1].ToString());
                    }

                    lblCombination.Text = "";
                    DetailsDownload(Counts);
                }
            }
        }

        protected void grdCrossTab_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dSummary.Clear();
                FilterSubtotal = 0;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int rcount = 2; rcount < e.Row.Cells.Count - 1; rcount++)
                {
                    if (e.Row.Cells[rcount].Text.Equals("X", StringComparison.OrdinalIgnoreCase))
                        e.Row.Cells[rcount].Text = "<img src='../Images/icon-delete.gif' />";
                }

                if (e.Row.RowIndex == 0)
                {
                    ChannelID = e.Row.Cells[e.Row.Cells.Count - 1].Text;
                    FilterSubtotal = Convert.ToInt32(((LinkButton)e.Row.FindControl("lnkCount")).Text);

                    e.Row.Cells[e.Row.Cells.Count - 1].Width = 50;

                }
                else
                {
                    if (ChannelID == e.Row.Cells[e.Row.Cells.Count - 1].Text)
                    {
                        FilterSubtotal += Convert.ToInt32(((LinkButton)e.Row.FindControl("lnkCount")).Text);
                    }
                    else
                    {
                        dSummary.Add(Convert.ToInt32(ChannelID), FilterSubtotal);
                        ChannelID = e.Row.Cells[e.Row.Cells.Count - 1].Text;
                        Table tblTemp = (Table)this.grdCrossTab.Controls[0];
                        int intIndex = tblTemp.Rows.GetRowIndex(e.Row);
                        GridViewRow gvrSubTotal = CreateGridViewRow(intIndex, e.Row.Cells.Count, FilterSubtotal.ToString(), 15);
                        tblTemp.Controls.AddAt(intIndex, gvrSubTotal);
                        FilterSubtotal = Convert.ToInt32(((LinkButton)e.Row.FindControl("lnkCount")).Text);
                    }

                    if (iRowsCount == e.Row.RowIndex)
                    {
                        Table tblTemp = (Table)this.grdCrossTab.Controls[0];
                        int intIndex = tblTemp.Rows.GetRowIndex(e.Row);
                        GridViewRow gvrLast = CreateGridViewRow(intIndex, e.Row.Cells.Count, FilterSubtotal.ToString(), 15);
                        tblTemp.Controls.AddAt(intIndex, gvrLast);
                    }
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Table tblTemp = (Table)this.grdCrossTab.Controls[0];
                int intIndex = tblTemp.Rows.GetRowIndex(e.Row);
                GridViewRow gvrLast = CreateGridViewRow(intIndex + 1, e.Row.Cells.Count, FilterSubtotal.ToString(), 15);
                tblTemp.Controls.AddAt(intIndex, gvrLast);
                dSummary.Add(Convert.ToInt32(ChannelID), FilterSubtotal);

                var TotalCounts = (from s in dSummary
                                   select s.Value).Sum();

                e.Row.Cells[0].Text = TotalCounts.ToString();
                e.Row.BackColor = System.Drawing.Color.Orange;
                var query = (from s in dSummary
                             select new { s.Key, s.Value, TotalPercent = Math.Round((Double)(s.Value * 100) / TotalCounts, 2) + " %" });

                grdSummary.AutoGenerateColumns = false;
                grdSummary.DataSource = query.ToList();
                grdSummary.DataBind();


                /* PIE CHART BINDING */

                if (query != null)
                {
                    chartSummary.IsSoftShadows = false;
                    chartSummary.Titles[0].Text = "Filter Summary";
                    chartSummary.Legends[0].Enabled = true;

                    Series series1 = chartSummary.Series[0];
                    series1.Points.Clear();

                    series1.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie;
                    series1["PieLabelStyle"] = "Outside";

                    foreach (KeyValuePair<int, int> entry in dSummary)
                    {
                        DataPoint dp = new DataPoint();
                        dp.LegendText = entry.Key.ToString();
                        dp.XValue = entry.Value;
                        dp.SetValueY(entry.Value);
                        dp.Label = "#PERCENT{P1}";
                        series1.Points.Add(dp);
                    }
                }

            }
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
        }

        private GridViewRow CreateGridViewRow(int iCurrentIndex, int iTableColumnSpan, string sTableText, int iTableHeight)
        {
            GridViewRow gvrTemp = new GridViewRow(iCurrentIndex, iCurrentIndex, DataControlRowType.Separator, DataControlRowState.Normal);
            TableCell cellTemp = new TableCell();
            cellTemp.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADD8E6");
            cellTemp.Font.Bold = true;
            cellTemp.HorizontalAlign = HorizontalAlign.Center;
            cellTemp.Text = sTableText;
            cellTemp.Height = Unit.Pixel(iTableHeight);
            gvrTemp.Cells.Add(cellTemp);

            TableCell cellTemp1 = new TableCell();
            cellTemp1.BackColor = System.Drawing.ColorTranslator.FromHtml("#ADD8E6");
            cellTemp1.ColumnSpan = iTableColumnSpan - 1;
            cellTemp1.Text = "";
            cellTemp1.Height = Unit.Pixel(iTableHeight);
            gvrTemp.Cells.Add(cellTemp1);

            return gvrTemp;
        }

        protected void btnDownloadSummary_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Download))
            {
                GetPenetrationData();
                if (Session["FilterPenetration"] != null)
                {
                    DataTable dt = (DataTable)Session["FilterPenetration"];

                    var TotalCounts = (from s in dSummary
                                       select s.Value).Sum();

                    var query = (from s in dSummary
                                 select new { Filters = s.Key.ToString(), Customers = s.Value.ToString(), TotalPercent = Math.Round((Double)(s.Value * 100) / TotalCounts, 2) + " %" }).ToList(); ;

                    query.Add(new { Filters = "Total", Customers = TotalCounts.ToString(), TotalPercent = "100%" });

                    if (query.ToList().Count > 0)
                    {
                        // Export all the details to xls
                        KMPS.MD.Objects.ExportReport.ExportToExcel(query.ToList(), "FilterComparisonSummary");
                    }
                }
            }
        }         

        protected void btnSaveReport_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Edit))
            {
                txtReportName.Text = "";

                if (lstFilter.SelectedValue == "")
                    DisplayError("Please select Filter");
                else
                {
                    if (lstFilter.GetSelectedIndices().Length > 10)
                        DisplayError("Please select no more than 10 filters.");
                    else
                        this.mdlPopSave.Show();
                }
            }
        }

        protected void btnPopupSaveReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (FilterPenetrationReports.ExistsByReportName(Master.clientconnections, txtReportName.Text))
                {
                    divPopupMessage.Visible = true;
                    lblPopupMessage.Text = "Report Name already exists. Please enter different name.";
                    this.mdlPopSave.Show();
                    return;
                }

                FilterPenetrationReports fpr = new FilterPenetrationReports();
                fpr.CreatedUserID = Master.LoggedInUser;
                fpr.ReportName = txtReportName.Text;
                fpr.BrandID = Convert.ToInt32(hfBrandID.Value);
                
                int ReportID =  KMPS.MD.Objects.FilterPenetrationReports.Save(Master.clientconnections, fpr);

                foreach (ListItem item in lstFilter.Items)
                {
                    if (item.Selected)
                    {
                        FilterPenetrationReportsDetails fprd = new FilterPenetrationReportsDetails();

                        fprd.ReportID = ReportID;
                        fprd.FilterID = Convert.ToInt32(item.Value);
                        FilterPenetrationReportsDetails.Save(Master.clientconnections, fprd);
                    }
                }

                this.mdlPopSave.Hide();
                loadsavedreports();
                if (TabContainer1.Visible)
                    LoadGrid();
            }
            catch (Exception ex)
            {
                divPopupMessage.Visible = true;
                lblPopupMessage.Text = ex.Message;
                this.mdlPopSave.Show();
            }
        }

        private void loadsavedreports()
        {
            drpdownReports.Items.Clear();

            List<FilterPenetrationReports> fpr = new List<FilterPenetrationReports>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                fpr = FilterPenetrationReports.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                fpr = FilterPenetrationReports.GetNotInBrand(Master.clientconnections);

            if (fpr.Count > 0)
            {
                drpdownReports.DataSource = fpr;
                drpdownReports.DataValueField = "ReportID";
                drpdownReports.DataTextField = "ReportName";
                drpdownReports.DataBind();
                ListItem item = new ListItem("-Select-", "*");
                drpdownReports.Items.Insert(0, item);
            }
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            lstFilter.ClearSelection();
            List<FilterPenetrationReportsDetails> lfprd = FilterPenetrationReportsDetails.getByFilterID(Master.clientconnections, Convert.ToInt32(drpdownReports.SelectedValue));

            foreach (FilterPenetrationReportsDetails fprd in lfprd)
            {
                foreach (ListItem item in lstFilter.Items)
                {
                    if (Convert.ToInt32(item.Value) == fprd.FilterID)
                        item.Selected = true; 
                }
            }

            Session["fc"] = null;
            Session["FilterPenetration"] = null;
            LoadGrid();
        }

        protected void btnDelReport_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.FilterComparison, KMPlatform.Enums.Access.Delete))
            {
                if (drpdownReports.SelectedIndex != -1)
                {
                    KMPS.MD.Objects.FilterPenetrationReports.Delete(Master.clientconnections, Int32.Parse(drpdownReports.SelectedValue));
                    loadsavedreports();
                    if (TabContainer1.Visible)
                        LoadGrid();
                }
            }
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            LoadFilters();
            loadsavedreports();
        }
    }
}