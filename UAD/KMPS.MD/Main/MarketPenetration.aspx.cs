using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization.Charting;
using KMPS.MD.Objects;
using RKLib.ExportData;
using PlatformUser = KM.Platform.User;
using PlatformAccess = KMPlatform.Enums.Access;
using PlatformServices = KMPlatform.Enums.Services;
using PlatformServiceFeatures = KMPlatform.Enums.ServiceFeatures;

namespace KMPS.MD.Main
{
    public partial class MarketPenetration : BrandsPageBase
    {
        private const PlatformServices UAD = PlatformServices.UAD;
        private const PlatformServiceFeatures MarketComparison = PlatformServiceFeatures.MarketComparison;
        private const string SecurityAccessErrorPage = "../SecurityAccessError.aspx";
        Filters fc;
        string ChannelID = "";
        int MarketSubtotal = 0;
        int iRowsCount = 0;
        public Dictionary<int, int> dSummary = new Dictionary<int, int>();
        delegate void RebuildSubscriberList();
        delegate void HidePanel();

        private int CampaignID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampaignID"]);
            }
            set
            {
                ViewState["CampaignID"] = value;
            }
        }

        private int CampaignFilterID
        {
            get
            {
                return Convert.ToInt32(ViewState["CampaignFilterID"]);
            }
            set
            {
                ViewState["CampaignFilterID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = "Markets";
            Master.SubMenu = "Market Comparison";

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
                VerifyAccess();

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

        private void VerifyAccess()
        {
            if (!PlatformUser.HasAccess(Master.UserSession.CurrentUser, UAD, MarketComparison, PlatformAccess.View))
            {
                Response.Redirect(SecurityAccessErrorPage);
            }

            if (!PlatformUser.HasAccess(Master.UserSession.CurrentUser, UAD, MarketComparison, PlatformAccess.Edit))
            {
                btnSaveReport.Visible = false;
            }

            if (!PlatformUser.HasAccess(Master.UserSession.CurrentUser, UAD, MarketComparison, PlatformAccess.Delete))
            {
                btnDelReport.Visible = false;
            }

            if (!PlatformUser.HasAccess(Master.UserSession.CurrentUser, UAD, MarketComparison, PlatformAccess.Download))
            {
                btnDownload.Visible = false;
                btnDownloadSummary.Visible = false;
            }

            if (!PlatformUser.HasAccess(Master.UserSession.CurrentUser, PlatformServices.UAD, PlatformServiceFeatures.MarketComparison, PlatformAccess.DownloadDetails))
            {
                btnDownloadDetails.Visible = false;
            }
        }

        private void LoadMarkets()
        {
            lstMarket.Items.Clear();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                lstMarket.DataSource = Objects.Markets.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                lstMarket.DataSource = Objects.Markets.GetNotInBrand(Master.clientconnections);

            lstMarket.DataBind();
        }

        private void loadsavedreports()
        {
            drpdownReports.Items.Clear();

            List<PenetrationReports> pr = new List<PenetrationReports>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                pr = PenetrationReports.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                pr = PenetrationReports.GetNotInBrand(Master.clientconnections);

            if (pr.Count > 0)
            {
                drpdownReports.DataSource = pr;
                drpdownReports.DataValueField = "ReportID";
                drpdownReports.DataTextField = "ReportName";
                drpdownReports.DataBind();
                ListItem item = new ListItem("-Select-", "*");
                drpdownReports.Items.Insert(0, item);
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            Session["fc"] = null;
            Session["MarketPenetration"] = null;
            ViewState["CHECKBOX_FILTER"] = null;
            drpdownReports.ClearSelection();

            if (lstMarket.SelectedValue == "")
                DisplayError("Please select Market");
            else
            {
                if (lstMarket.GetSelectedIndices().Length > 10)
                    DisplayError("Please select no more than 10 markets.");
                else
                    LoadGrid();
            }
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMsg.Text = errorMessage;
            divErrorMsg.Visible = true;
       }

        private void GetPenetrationData()
        {
            if (Session["MarketPenetration"] == null)
            {
                fc.Clear();

                foreach (ListItem mylistvalue in lstMarket.Items)
                {
                    if (mylistvalue.Selected)
                    {
                        Filter f = new Filter();
                        f.FilterName = mylistvalue.Text;

                        string selectedvalues = string.Empty;

                        Objects.Markets m = Objects.Markets.GetByID(Master.clientconnections, Convert.ToInt32(mylistvalue.Value));
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(m.MarketXML);

                        f.BrandID = m.BrandID == null ? 0 : (Int32)(m.BrandID); 

                        if (m.BrandID > 0)
                            f.Fields.Add(new Field("Brand", m.BrandID.ToString(), m.MarketID.ToString(), "", Enums.FiltersType.Brand, "BRAND"));
                        
                        XmlNode node = doc.SelectSingleNode("//Market/MarketType[@ID ='P']");
                        if (node != null)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                selectedvalues += selectedvalues == string.Empty ? child.Attributes["ID"].Value : "," + child.Attributes["ID"].Value;
                            }
                        }
                        
                        if (selectedvalues != string.Empty)
                              f.Fields.Add(new Field("Product", selectedvalues,  mylistvalue.Text, "", Enums.FiltersType.Product, "Product"));

                        List<MasterGroup> masterGroups = MasterGroup.GetAll(Master.clientconnections);
                        foreach (MasterGroup mg in masterGroups)
                        {
                            selectedvalues = string.Empty;

                            node = doc.SelectSingleNode("//Market/MarketType[@ID ='D']/Group[@ID = '" + mg.ColumnReference.ToString() + "']");

                            if (node != null)
                            {
                                foreach (XmlNode child in node.ChildNodes)
                                {
                                    selectedvalues += selectedvalues == string.Empty ? child.Attributes["ID"].Value : "," + child.Attributes["ID"].Value;
                                }
                                
                                if (selectedvalues.Length > 0)
                                    f.Fields.Add(new Field(mg.DisplayName, selectedvalues, mg.DisplayName, "", Enums.FiltersType.Dimension, mg.ColumnReference));
                            }
                         }

                        node = doc.SelectSingleNode("//Market/FilterType[@ID ='A']");

                        if (node != null)
                        {
                            foreach (XmlNode nodeEntry in node.ChildNodes)
                            {
                                f.Fields.Add(new Field("Adhoc", nodeEntry.ChildNodes[0].Attributes["ID"].Value, "", nodeEntry.ChildNodes[1].Attributes["ID"].Value, Enums.FiltersType.Adhoc, nodeEntry.Attributes["ID"].Value));

                            }
                        }

                        if (f.Fields != null && f.Fields.Count > 0)
                        {
                            f.FilterNo = fc.Count + 1;
                            fc.Add(f);
                        }
                    }
                }

                if (fc.Count > 0)
                {
                    Session["fc"] = fc;

                    DataTable dt = fc.GetCrossTabData("Markets");
                    Session["MarketPenetration"] = dt;
                }
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
                if (lstMarket.GetSelectedIndices().Length <= 10)
                {
                    if (lstMarket.SelectedValue != "")
                    {
                        TabContainer1.Visible = true;
                        GetPenetrationData();
                        if (ViewState["CHECKBOX_FILTER"] != null)
                        {
                            DataTable dt = (DataTable)Session["MarketPenetrationSub"];

                            iRowsCount = dt.Rows.Count;

                            grdCrossTab.DataSource = dt;
                            grdCrossTab.DataBind();

                            grdCrossTab.Visible = true;
                        }
                        else if (Session["MarketPenetration"] != null)
                        {
                            DataTable dt = (DataTable)Session["MarketPenetration"];

                            iRowsCount = dt.Rows.Count;

                            if (iRowsCount == 0)
                            {
                                TabContainer1.Visible = false;
                                DisplayError("Selected market contains no records");
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
                DisplayError("Error:"+ex.Message);
            }
        }        
    
        protected void grdCrossTab_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dSummary.Clear();
                MarketSubtotal = 0;
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
                    ChannelID = e.Row.Cells[e.Row.Cells.Count-1].Text;
                    MarketSubtotal = Convert.ToInt32(((LinkButton)e.Row.FindControl("lnkCount")).Text);

                    e.Row.Cells[e.Row.Cells.Count - 1].Width = 50;

                }
                else
                {
                    if (ChannelID == e.Row.Cells[e.Row.Cells.Count - 1].Text)
                    {
                        MarketSubtotal += Convert.ToInt32(((LinkButton)e.Row.FindControl("lnkCount")).Text);
                    }
                    else
                    {
                        dSummary.Add(Convert.ToInt32(ChannelID), MarketSubtotal);
                        ChannelID = e.Row.Cells[e.Row.Cells.Count - 1].Text;
                        Table tblTemp = (Table)this.grdCrossTab.Controls[0];
                        int intIndex = tblTemp.Rows.GetRowIndex(e.Row);
                        GridViewRow gvrSubTotal = CreateGridViewRow(intIndex, e.Row.Cells.Count, MarketSubtotal.ToString(), 15);
                        tblTemp.Controls.AddAt(intIndex, gvrSubTotal);
                        MarketSubtotal = Convert.ToInt32(((LinkButton)e.Row.FindControl("lnkCount")).Text);
                    }

                    if (iRowsCount == e.Row.RowIndex)
                    {
                        Table tblTemp = (Table)this.grdCrossTab.Controls[0];
                        int intIndex = tblTemp.Rows.GetRowIndex(e.Row);
                        GridViewRow gvrLast = CreateGridViewRow(intIndex, e.Row.Cells.Count, MarketSubtotal.ToString(), 15);
                        tblTemp.Controls.AddAt(intIndex, gvrLast);
                    }
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Table tblTemp = (Table)this.grdCrossTab.Controls[0];
                int intIndex = tblTemp.Rows.GetRowIndex(e.Row);
                GridViewRow gvrLast = CreateGridViewRow(intIndex + 1, e.Row.Cells.Count, MarketSubtotal.ToString(), 15);
                tblTemp.Controls.AddAt(intIndex, gvrLast);
                dSummary.Add(Convert.ToInt32(ChannelID), MarketSubtotal);

                var TotalCounts = (from s in dSummary
                                       select s.Value).Sum();

                e.Row.Cells[0].Text = TotalCounts.ToString();
                e.Row.BackColor = System.Drawing.Color.Orange;
                var query = (from s in dSummary
                                   select new { s.Key, s.Value, TotalPercent = Math.Round((Double)(s.Value*100)/TotalCounts,2) +  " %"});

                grdSummary.AutoGenerateColumns = false; 
                grdSummary.DataSource = query.ToList();
                grdSummary.DataBind();


                /* PIE CHART BINDING */

                if (query != null)
                {
                    chartSummary.IsSoftShadows = false;
                    chartSummary.Titles[0].Text = "Market Summary";
                    chartSummary.Legends[0].Enabled = true;

                    Series series1 = chartSummary.Series[0];
                    series1.Points.Clear();

                    series1.ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Pie;
                    series1["PieLabelStyle"] = "Outside";

                    foreach(KeyValuePair<int,int> entry in dSummary) 

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

        void cbox_CheckedChanged(object sender, EventArgs e)
        {
             CheckBox cbox = (CheckBox)sender;
             ViewState["CHECKBOX_FILTER"] = null;
             Session["MarketPenetrationSub"] = null;
             LoadGrid();
             if (cbox.Checked)
             {                
                 try
                 {                    
                     ViewState["CHECKBOX_FILTER"] = cbox.Text;
                     DataTable dt = new DataTable();
                     dt = ((DataTable)Session["MarketPenetration"]).Copy();

                     for (int i = dt.Rows.Count - 1; i >= 0; i--)
                     {
                         DataRow dr = dt.Rows[i];
                         if (dr[cbox.Text].ToString() != "X")
                             dr.Delete();
                     }
                     Session["MarketPenetrationSub"] = dt;
                     grdCrossTab.DataSource = dt;
                     grdCrossTab.DataBind();
                     
                 }
                 catch (Exception ex)
                 {
                     throw ex;
                 }
            }
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

        protected void lnkReport_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            lblCombination.Text = args[0];
            hfSelectedFilterNo.Value = args[2];
            hfSelectedFilterOperation.Value = args[3];
            hfSuppressedFilterNo.Value = args[4];
            hfSuppressedFilterOperation.Value = args[5];
            DetailsDownload(Int32.Parse(args[1]));
        }
        
        protected void btnSaveReport_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.Edit))
            {
                txtReportName.Text = "";

                if (lstMarket.SelectedValue == "")
                    DisplayError("Please select Market");
                else
                {
                    if (lstMarket.GetSelectedIndices().Length > 10)
                        DisplayError("Please select no more than 10 markets.");
                    else
                        this.mdlPopSave.Show();
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
            LoadMarkets();
            loadsavedreports();
        }

        protected void btnPopupSaveReport_Click(object sender, EventArgs e)
        {

            int user=Master.LoggedInUser;         

            StringBuilder sb = new StringBuilder();
            foreach (ListItem mylistvalue in lstMarket.Items)
            {
                if (mylistvalue.Selected)
                    sb.AppendLine("<Markets MarketID=\"" + mylistvalue.Value + "\"/>");
            }
            sb = sb.Remove(sb.Length - 1, 1);

            try
            {
                KMPS.MD.Objects.PenetrationReports.SaveReport(Master.clientconnections, user, sb.ToString(), txtReportName.Text, Convert.ToInt32(hfBrandID.Value));
                this.mdlPopSave.Hide();
                loadsavedreports();
                if (TabContainer1.Visible)
                    LoadGrid();
            }
            catch (Exception ex)
            {
                divPopupMessage.Visible = true;
                lblPopupMessage.Text =  ex.Message;
                this.mdlPopSave.Show();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.Download))
            {
                if (lstMarket.SelectedValue != "")
                {
                    GetPenetrationData();
                    if (Session["MarketPenetration"] != null)
                    {
                        DataTable dt = (DataTable)Session["MarketPenetration"];

                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            dt.Columns[i].ColumnName = Regex.Replace(dt.Columns[i].ColumnName, "[^0-9A-Za-z]+", "_");
                        }

                        DataTable dt2 = new DataTable();
                        dt2 = dt.Copy();
                        dt2.Columns.RemoveAt(0);
                        if (dt.Rows.Count > 0)
                        {
                            // Export all the details to xls
                            RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                            objExport.ExportDetails(dt2, Export.ExportFormat.Excel, "MarketComparisonDetails.xls");
                        }
                    }
                }
            }
        }

        protected void btnDownloadSummary_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.Download))
            {
                GetPenetrationData();
                if (Session["MarketPenetration"] != null)
                {
                    DataTable dt = (DataTable)Session["MarketPenetration"];

                    var TotalCounts = (from s in dSummary
                                       select s.Value).Sum();

                    var query = (from s in dSummary
                                 select new { Markets = s.Key.ToString(), Customers = s.Value.ToString(), TotalPercent = Math.Round((Double)(s.Value * 100) / TotalCounts, 2) + " %" }).ToList(); ;

                    query.Add(new { Markets = "Total", Customers = TotalCounts.ToString(), TotalPercent = "100%" });

                    if (query.ToList().Count > 0)
                        KMPS.MD.Objects.ExportReport.ExportToExcel(query.ToList(), "MarketComparisonSummary");
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
            else
            {
                if (Session["fc"] == null)
                {
                    LoadGrid();
                }

                fc = (Filters)Session["fc"];

                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = Filter.generateCombinationQuery(fc, hfSelectedFilterOperation.Value, hfSuppressedFilterOperation.Value, hfSelectedFilterNo.Value, hfSuppressedFilterNo.Value, "", 0, Convert.ToInt32(hfBrandID.Value), Master.clientconnections);
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
        }

        protected void btnDownloadDetails_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.DownloadDetails))
            {
                if (lstMarket.SelectedValue != "")
                {
                    int Counts = 0;
                    foreach (GridViewRow r in grdCrossTab.Rows)
                    {
                        Counts += Convert.ToInt32(grdCrossTab.DataKeys[r.RowIndex].Values[1].ToString());
                    }

                    lblCombination.Text = "";

                    if (Session["fc"] == null)
                    {
                        LoadGrid();
                    }

                    fc = (Filters)Session["fc"];
                    string allSelectedFilterNo = string.Empty;

                    foreach (Filter f in fc)
                    {
                        allSelectedFilterNo += allSelectedFilterNo == string.Empty ? f.FilterNo.ToString() : "," + f.FilterNo.ToString();
                    }

                    hfSelectedFilterNo.Value = allSelectedFilterNo;

                    if (hfSelectedFilterNo.Value.Contains(","))
                        hfSelectedFilterOperation.Value = "union";
                    else
                        hfSelectedFilterOperation.Value = "";

                    hfSuppressedFilterNo.Value = "";
                    hfSuppressedFilterOperation.Value = "";
                    DetailsDownload(Counts);
                }
            }
        }

        private void Reload()
        {
            LoadGrid();
            fc = (Filters)Session["fc"];
            DownloadPanel1.SubscribersQueries = Filter.generateCombinationQuery(fc, hfSelectedFilterOperation.Value, "", hfSelectedFilterNo.Value, "", "", 0, Convert.ToInt32(hfBrandID.Value), Master.clientconnections);
        }

        private void Hide()
        {          
            DownloadPanel1.Visible = false;
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {      
            DataTable dt = DataFunctions.getDataTable("select MarketID from PenetrationReports_Markets where ReportID='" + drpdownReports.SelectedValue + "'", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            lstMarket.ClearSelection();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstMarket.Items.FindByValue(dt.Rows[i][0].ToString()).Selected = true;
            }
            Session["fc"] = null;
            Session["MarketPenetration"] = null;
            ViewState["CHECKBOX_FILTER"] = null;
            LoadGrid();
        }

        protected void btnDelReport_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.MarketComparison, KMPlatform.Enums.Access.Delete))
            {
                if (drpdownReports.SelectedIndex != -1)
                {
                    KMPS.MD.Objects.PenetrationReports.DeleteReport(Master.clientconnections, Int32.Parse(drpdownReports.SelectedValue));
                    loadsavedreports();
                    if (TabContainer1.Visible)
                        LoadGrid();
                }
            }
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();
            hfBrandID.Value = drpBrand.SelectedValue;

            if (Convert.ToInt32(drpBrand.SelectedValue) >= 0)
            {
                LoadMarkets();
                loadsavedreports();
                TabContainer1.Visible = false;
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
        }

        protected void ResetControls()
        {
            hfBrandID.Value = "0";
            lstMarket.Items.Clear();
            drpdownReports.Items.Clear();
            TabContainer1.Visible = false;
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            LoadMarkets();
            loadsavedreports();
        }
    }
}