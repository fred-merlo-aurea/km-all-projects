using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KMPS.MD.Objects;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using System.Text;
using System.Data.SqlClient;
using KMPS.MD.Helpers;
using static KMPlatform.Enums;

namespace KMPS.MD.Main
{
    public partial class Questions : BrandsPageBase
    {
        private const string MasterMenuSalesView = "Sales View";
        private const string SecurityAccessErrorPage = "../SecurityAccessError.aspx";
        private const string AdhocFilter = "Adhoc";
        private const string LabelFilterTextControlId = "lblFilterText";
        private const string LabelFilterNameControlId = "lblFiltername";
        private const string LabelFilterValuesControlId = "lblFilterValues";
        private const string LabelSearchConditionControlId = "lblSearchCondition";
        private const string LabelFilterTypeControlId = "lblFilterType";
        private const string LabelGroupControlId = "lblGroup";
        private const string GridFilterValuesControlId = "grdFilterValues";
        private const string LinkDownloadControlId = "lnkdownload";
        private const string LabelViewTypeControlId = "lblViewType";
        private const string LabelProductIdControlId = "lblPubID";
        private const string LabelBrandIdControlId = "lblBrandID";
        private const string FilterNo = "FilterNo";
        Filters fc;
        int GrdRowCount = 0;

        protected override string BrandDefaultEmptyDropDown => "All";

        delegate void RebuildSubscriberList();
        delegate void HidePanel();

        private static void AddFieldsToFilter(GridView gridFilterValues, Filter filter)
        {
            foreach (GridViewRow gridViewRow in gridFilterValues.Rows)
            {
                var filterText = FindLabelControlText(gridViewRow, LabelFilterTextControlId);
                var filtername = FindLabelControlText(gridViewRow, LabelFilterNameControlId);
                var filterValues = FindLabelControlText(gridViewRow, LabelFilterValuesControlId);
                var searchCondition = FindLabelControlText(gridViewRow, LabelSearchConditionControlId);
                var filterTypeText = FindLabelControlText(gridViewRow, LabelFilterTypeControlId);
                var groupName = FindLabelControlText(gridViewRow, LabelGroupControlId);

                var filterType = (Enums.FiltersType)Enum.Parse(typeof(Enums.FiltersType), filterTypeText);

                filter.Fields.Add(
                    new Field(
                        filtername,
                        filterValues,
                        filterText,
                        searchCondition,
                        filterTypeText == string.Empty ? Enums.FiltersType.None : filterType,
                        groupName));
            }
        }

        private static string FindLabelControlText(Control oItem, string controlName)
        {
            return ((Label)oItem.FindControl(controlName)).Text;
        }

        private void Reload()
        {
            DownloadPanel.SubscribersQueries = GetSubscribersQueriesForUserControl();
        }

        private void DownloadPopupHide()
        {
            DownloadPanel.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.Menu = MasterMenuSalesView;
            DownloadPanel.DelMethod = new RebuildSubscriberList(Reload);
            DownloadPanel.hideDownloadPopup = new HidePanel(DownloadPopupHide);
            DownloadPanel.ShowHeaderCheckBox = true;

            lblErrorMessage.Text = string.Empty;
            divError.Visible = false;
            fc = new Filters(Master.clientconnections, Master.LoggedInUser);

            if (!IsPostBack)
            {
                FirstLoad();
            }
            else
            {
                PostBack();
            }
        }

        private void PostBack()
        {
            if (fc != null && fc.Any())
            {
                return;
            }

            if (Convert.ToBoolean(hfReloadValue.Value))
            {
                if (grdFilters.Rows.Count <= 0)
                {
                    return;
                }

                for (var i = 0; i < grdFilters.Rows.Count; i++)
                {
                    if (grdFilters.Rows[i].RowType != DataControlRowType.DataRow)
                    {
                        continue;
                    }

                    var grdFilterValues = (GridView)grdFilters.Rows[i].FindControl(GridFilterValuesControlId);
                    var lnkCount = (LinkButton)grdFilters.Rows[i].FindControl(LinkDownloadControlId);
                    var lblViewType = (Label)grdFilters.Rows[i].FindControl(LabelViewTypeControlId);
                    var lblPubId = (Label)grdFilters.Rows[i].FindControl(LabelProductIdControlId);
                    var lblBrandId = (Label)grdFilters.Rows[i].FindControl(LabelBrandIdControlId);

                    var filter = new Filter
                    {
                        FilterNo = Convert.ToInt32(grdFilters.DataKeys[i].Values[FilterNo].ToString()),
                        Count = Convert.ToInt32(lnkCount.Text),
                        ViewType = (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), lblViewType.Text),
                        PubID = Convert.ToInt32(lblPubId.Text),
                        BrandID = Convert.ToInt32(lblBrandId.Text)
                    };

                    AddFieldsToFilter(grdFilterValues, filter);

                    fc.Add(filter, false);
                }

                fc.Execute();
            }
            else
            {
                hfReloadValue.Value = "true";
            }
        }

        private void FirstLoad()
        {
            if (!KM.Platform.User.HasAccess(
                    Master.UserSession.CurrentUser,
                    Services.UAD,
                    ServiceFeatures.SalesView,
                    Access.View))
            {
                Response.Redirect(SecurityAccessErrorPage);
            }

            if (!KM.Platform.User.HasAccess(
                    Master.UserSession.CurrentUser,
                    Services.UAD,
                    ServiceFeatures.SalesView,
                    Access.Download))
            {
                btnDownloadQuestions.Visible = false;
            }

            if (!KM.Platform.User.HasAccess(
                    Master.UserSession.CurrentUser,
                    Services.UAD,
                    ServiceFeatures.SalesView,
                    Access.DownloadSample))
            {
                btnSamplePdf.Visible = false;
            }

            rtvQuestionCategory.DataSource = Objects.QuestionCategory.GetAll(Master.clientconnections);
            rtvQuestionCategory.DataBind();
            rtvQuestionCategory.ExpandAllNodes();

            if (rtvQuestionCategory.Nodes.Count > 0)
            {
                rtvQuestionCategory.Nodes[0]
                    .Expanded = true;
            }

            LoadBrands();

            DownloadPanel.error = false;
            DownloadPanel.Showsavetocampaign = false;
            DownloadPanel.Showexporttoemailmarketing = false;
            DownloadPanel.Showexporttomarketo = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            rtvQuestionCategory.UnselectAllNodes();
            rgQuestions.DataSource = GetData();
            rgQuestions.DataBind();
        }

        public void DisplayError(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            divError.Visible = true;
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtvQuestionCategory.UnselectAllNodes();
            hfBrandID.Value = drpBrand.SelectedValue;
            rgQuestions.DataSource = GetData();
            rgQuestions.DataBind();
        }

        protected List<MDFilter> GetData()
        {
            List<MDFilter> mdf = new List<MDFilter>();
            try
            {
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                    mdf = MDFilter.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                else
                {
                    if (Convert.ToInt32(hfBrandID.Value) == -1)
                    {
                        mdf = MDFilter.GetInBrand(Master.clientconnections);
                    }
                    else
                    {
                        mdf = MDFilter.GetNotInBrand(Master.clientconnections);
                    }
                }

                if (mdf != null)
                {
                    if (txtQuestionName.Text != string.Empty)
                    {
                        mdf = mdf.FindAll(x => x.AddtoSalesView == true && x.QuestionName.ToLower().Contains(txtQuestionName.Text.ToLower()));
                    }
                    else
                        mdf = mdf.FindAll(x => x.AddtoSalesView.Equals(true));

                    if (rtvQuestionCategory.SelectedValue != string.Empty)
                    {
                        mdf = mdf.FindAll(x => (x.QuestionCategoryID ?? 0) == Convert.ToInt32(rtvQuestionCategory.SelectedValue));
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            return mdf;
        }

        protected void rtvQuestionCategory_OnNodeClick(object sender, RadTreeNodeEventArgs e)
        {
            rgQuestions.DataSource = GetData(); 
            rgQuestions.DataBind();
        }

        protected void rgQuestions_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgQuestions.DataSource = GetData(); 
        }

        protected void lnkFilterQuestion_Command(object sender, CommandEventArgs e)
        {
            try
            {
                hfSampleSubscriptionID.Value = "0";
                string[] strValue = e.CommandArgument.ToString().Split('/');
                fc = MDFilter.LoadFilters(Master.clientconnections, int.Parse(strValue[0]), Master.LoggedInUser);
                fc.Execute();

                if (fc.Count == 1 )
                {
                    lblCounts.Text = fc.First().Count.ToString();
                }
                else
                {
                    try
                    {
                        lblCounts.Text = fc.FilterComboList.Find(x => x.FilterDescription == "All Union").Count.ToString();
                    }
                    catch
                    {

                    }
                }

                lblQuestionName.Text = strValue[1].ToString();
                LoadGridFilters();

                DataTable dtRecords = new DataTable();
                List<string> Selected_FilterIDs = new List<string>();

                foreach (Filter filter in fc)
                {
                    Selected_FilterIDs.Add(filter.FilterNo.ToString());
                }

                dtRecords = Subscriber.GetSubscriberData(Master.clientconnections, Filter.generateCombinationQuery(fc, "Union", string.Empty, string.Join(",", Selected_FilterIDs), string.Empty, string.Empty, fc.First().PubID, fc.First().BrandID, Master.clientconnections), "s.subscriptionID, isnull(s.score,0) as score");

                if (dtRecords.Rows.Count > 0)
                {
                    var newSort = (from row in dtRecords.AsEnumerable()
                                   orderby row.Field<int>("score") descending
                                   select new
                                   {
                                       subscriptionID = row.Field<int>("subscriptionID")
                                   }).Take(1).ToList();

                    hfSampleSubscriptionID.Value = newSort.FirstOrDefault().subscriptionID.ToString();

                }

                mdlPopupFilter.Show();
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        protected void btnCloseFilterPopup_Click(object sender, EventArgs e)
        {
            hfSampleSubscriptionID.Value = "0";
            grdFilters.DataSource = null;
            grdFilters.DataBind();
            grdFilterCounts.DataSource = null;
            grdFilterCounts.DataBind();
        }

        private void LoadGridFilters()
        {
            grdFilters.DataSource = null;
            grdFilters.DataBind();

            grdFilterCounts.DataSource = null;
            grdFilterCounts.DataBind();

            if (fc.Count > 0)
            {
                grdFilters.DataSource = fc;
                grdFilters.DataBind();

                grdFilterCounts.DataSource = fc.FilterComboList.Where(x => (x.SelectedFilterNo.Split(',').Length > 1 ? Convert.ToInt32(x.SelectedFilterNo.Split(',')[1]) <= 5 : true && Convert.ToInt32(x.SelectedFilterNo.Split(',')[0]) <= 5) && (x.SuppressedFilterNo == null || x.SuppressedFilterNo == string.Empty ? true : Convert.ToInt32(x.SuppressedFilterNo) <= 5));
                grdFilterCounts.DataBind();

                ctrlVenn.Visible = true;
                ctrlVenn.CreateVenn(fc);
            }
        }

        protected void grdFilters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GrdRowCount++;

                Label lblID = (Label)e.Row.FindControl("lblID");
                lblID.Text = GrdRowCount.ToString();
                GridView grdFilterValues = (GridView)e.Row.FindControl("grdFilterValues");
                List<Field> grdFilterList = LoadGridFilterValues(Convert.ToInt32(grdFilters.DataKeys[e.Row.RowIndex].Value.ToString()));
                grdFilterValues.DataSource = grdFilterList.Distinct().ToList();
                grdFilterValues.DataBind();
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (grdFilters.Rows.Count > 1)
            {
                if (ctrlVenn.VennParams != string.Empty)
                {
                    if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
                    {
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), this.ctrlVenn.VennDivID, "renderVenn('#" + this.ctrlVenn.VennDivID + "', [" + ctrlVenn.VennParams + "]);", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(Page), this.ctrlVenn.VennDivID, "renderVenn('#" + this.ctrlVenn.VennDivID + "', [" + ctrlVenn.VennParams + "]);", true);
                    }
                }
            }
        }

        private List<Field> LoadGridFilterValues(int filterNo)
        {
            Filter filter = fc.SingleOrDefault(f => f.FilterNo == filterNo);
            return filter.Fields;
        }

        protected void lnkdownload_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            hfSelectedFilterNos.Value = args[0];
            hfSuppressedFilterNos.Value = args[1];
            hfSelectedFilterOperation.Value = args[2];
            hfSuppressedFilterOperation.Value = args[3];
            hfFilterCombination.Value = args[4];
            hfDownloadCount.Value = args[5];
            ShowDownloadPanel();
            mdlPopupFilter.Show();
        }

        protected void lnkCount_Command(object sender, CommandEventArgs e)
        {
            string temp = e.CommandArgument.ToString();
            string[] args = temp.Split('/');
            hfSelectedFilterNos.Value = args[0];
            hfSuppressedFilterNos.Value = args[1];
            hfSelectedFilterOperation.Value = args[2];
            hfSuppressedFilterOperation.Value = args[3];
            hfFilterCombination.Value = args[4];
            hfDownloadCount.Value = args[5];
            ShowDownloadPanel();
            mdlPopupFilter.Show();
        }

        private void ShowDownloadPanel()
        {
            DownloadPanel.SubscribersQueries = GetSubscribersQueriesForUserControl();

            if (Convert.ToInt32(hfDownloadCount.Value) > 0)
            {
                DownloadPanel.Visible = true;
                DownloadPanel.HeaderText = Utilities.GetHeaderText(fc, hfSelectedFilterNos.Value, hfSuppressedFilterNos.Value, hfSelectedFilterOperation.Value, hfSuppressedFilterOperation.Value, false);
                DownloadPanel.BrandID = Convert.ToInt32(hfBrandID.Value);
                DownloadPanel.PubIDs = fc.First().PubID.ToString().Split(',').Select(int.Parse).ToList();
                DownloadPanel.ViewType = fc.First().ViewType;
                DownloadPanel.EnableCbIsRecentData = false;
                DownloadPanel.VisibleCbIsRecentData = fc.First().ViewType == Enums.ViewType.RecencyView ? true : false;
                DownloadPanel.filterCombination = hfFilterCombination.Value;
                DownloadPanel.downloadCount = Convert.ToInt32(hfDownloadCount.Value);
                DownloadPanel.IsPopupSVFilter = true;
                DownloadPanel.LoadControls();
                DownloadPanel.LoadDownloadTemplate();
                DownloadPanel.loadExportFields();
                DownloadPanel.ValidateExportPermission();
            }
        }

        private StringBuilder GetSubscribersQueriesForUserControl()
        {
            StringBuilder Queries = new StringBuilder();

            try
            {
                Queries = Filter.generateCombinationQuery(fc, hfSelectedFilterOperation.Value, hfSuppressedFilterOperation.Value, hfSelectedFilterNos.Value, hfSuppressedFilterNos.Value, string.Empty, fc.First().PubID, Convert.ToInt32(hfBrandID.Value), Master.clientconnections);
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            return Queries;
        }

        protected void btnSamplePdf_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.SalesView, Access.DownloadSample))
            {
                string companylogo = string.Empty;
                int customerID = Master.UserSession.CustomerID;

                Config c = Config.getCustomerLogo(Master.clientconnections);
                if (c.ConfigID != 0)
                    companylogo = "File:///" + Server.MapPath("../Images/logo/" + customerID + "/") + c.Value;

                string brandlogo = string.Empty;
                if (Convert.ToInt32(hfBrandID.Value) > 0)
                {
                    Brand b = Brand.GetByID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
                    if (b.Logo != string.Empty)
                        brandlogo = "File:///" + Server.MapPath("../Images/logo/" + customerID + "/") + b.Logo;
                }

                Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_Subscriber", Subscriber.Get(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds1 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberDimension", SubscriberDimension.GetSubscriberDimensionForExport(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds2 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberPubs", SubscriberPubs.GetSubscriberPubsForExport(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds3 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberOpenActivity", SubscriberActivity.GetOpenActivity(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)).Take(5));
                Microsoft.Reporting.WebForms.ReportDataSource rds4 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberClickActivity", SubscriberActivity.GetClickActivity(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)).Take(5));
                Microsoft.Reporting.WebForms.ReportDataSource rds5 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberVisitActivity", SubscriberVisitActivity.Get(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value)).Take(5));
                Microsoft.Reporting.WebForms.ReportDataSource rds6 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberAdhoc", SubscriberAdhoc.GetForRecordView(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value)));
                Microsoft.Reporting.WebForms.ReportDataSource rds7 = new Microsoft.Reporting.WebForms.ReportDataSource("DS_SubscriberActivity", SubscriberActivity.Get(Master.clientconnections, Convert.ToInt32(hfSampleSubscriptionID.Value), Convert.ToInt32(hfBrandID.Value)).Take(10));

                ReportViewer2.Visible = false;
                ReportViewer2.LocalReport.DataSources.Clear();
                ReportViewer2.LocalReport.DataSources.Add(rds);
                ReportViewer2.LocalReport.DataSources.Add(rds1);
                ReportViewer2.LocalReport.DataSources.Add(rds2);
                ReportViewer2.LocalReport.DataSources.Add(rds3);
                ReportViewer2.LocalReport.DataSources.Add(rds4);
                ReportViewer2.LocalReport.DataSources.Add(rds5);
                ReportViewer2.LocalReport.DataSources.Add(rds6);
                ReportViewer2.LocalReport.DataSources.Add(rds7);

                ReportViewer2.LocalReport.EnableExternalImages = true;
                ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/main/reports/" + "rpt_SubscriberDetails.rdlc");
                ReportParameter[] parameters = new ReportParameter[4];
                parameters[0] = new ReportParameter("SubscriptionID", Convert.ToInt32(hfSampleSubscriptionID.Value).ToString());
                parameters[1] = new ReportParameter("CompanyLogo", companylogo);
                parameters[2] = new ReportParameter("BrandLogo", brandlogo);
                parameters[3] = new ReportParameter("BrandID", hfBrandID.Value);
                ReportViewer2.LocalReport.SetParameters(parameters);

                ReportViewer2.LocalReport.Refresh();

                Warning[] warnings = null;
                string[] streamids = null;
                String mimeType = null;
                String encoding = null;
                String extension = null;
                Byte[] bytes = null;

                bytes = ReportViewer2.LocalReport.Render("PDF", string.Empty, out mimeType, out encoding, out extension, out streamids, out warnings);
                Response.ContentType = "application/pdf";

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SubscriberDetails.pdf");
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        protected void btnDownloadQuestions_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, Services.UAD, ServiceFeatures.SalesView, Access.Download))
            {
                Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("DS_Filters", GetData());
                ReportViewer1.Visible = false;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);

                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/main/reports/" + "rpt_Questions.rdlc");

                ReportViewer1.LocalReport.Refresh();

                Warning[] warnings = null;
                string[] streamids = null;
                String mimeType = null;
                String encoding = null;
                String extension = null;
                Byte[] bytes = null;

                bytes = ReportViewer1.LocalReport.Render("PDF", string.Empty, out mimeType, out encoding, out extension, out streamids, out warnings);
                Response.ContentType = "application/pdf";

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Questions.pdf");
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        protected void lnkGeoReport_Command(object sender, CommandEventArgs e)
        {
            this.mdlPopupGeo.Show();
            string[] args = e.CommandArgument.ToString().Split('/');

            loadReportGeoData(e.CommandName, Filter.generateCombinationQuery(fc, args[2], args[3], args[0], args[1], string.Empty, 0, 0, Master.clientconnections));

            //lblSelectedFilterNos.Text = args[0];
            //lblSuppressedFilterNos.Text = args[1];
            //lblOperation.Text = args[2];

            rvGeoLocation.Visible = true;
            this.mdlPopupFilter.Show();
            this.mdlPopupGeo.Show();
        }

        protected void lnkGeoMaps_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split('/');

            DataTable locations = GetMap(Filter.generateCombinationQuery(fc, args[2], args[3], args[0], args[1], string.Empty, 0, 0, Master.clientconnections));

            if (locations.Rows.Count > 0)
            {
                var json = JsonHelper.GetJsonStringFromDataTable(locations);
                myMapCoords.Text = json;
            }
            this.mdlPopupGeoMap.Show();
        }
        
        private DataTable GetMap(StringBuilder Queries)
        {
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberGLBySubscriptionID", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Queries", SqlDbType.Text)).Value = Queries.ToString();

            DataTable dt1 = new DataTable();
            dt1 = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));

            string blue = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/KMPS.MD/Images/blue-pin.png";
            List<MapItem> listMap = new List<MapItem>();
            foreach (DataRow dr in dt1.Rows)
            {
                MapItem mi = new MapItem();
                //mi.SubscriberID = Convert.ToInt32(dr["SubscriberID"].ToString());
                mi.SubscriberID = dr["SubscriberID"].ToString();
                mi.Latitude = TruncateDecimal(Convert.ToDecimal(dr["Latitude"].ToString()), 6);
                mi.Longitude = TruncateDecimal(Convert.ToDecimal(dr["Longitude"].ToString()), 6);
                //mi.MarkerImage = blue;
                listMap.Add(mi);
            }
            listMap = listMap.OrderBy(x => x.ZipCode).ToList();
            DataTable dtNew = MapStops(listMap);
            return dtNew;
        }

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }

        private DataTable MapStops(List<MapItem> items)
        {
            //SubscriberID, MapAddress, Latitude, Longitude, '/Images/blue-pin.png' AS 'markerImage', ZipCode 
            DataTable dtNew = new DataTable();
            DataColumn dcSubscriberID = new DataColumn("Sc");
            dtNew.Columns.Add(dcSubscriberID);
            //DataColumn dcMapAddress = new DataColumn("MapAddress");
            //dtNew.Columns.Add(dcMapAddress);
            DataColumn dcLatitude = new DataColumn("Lt");
            dtNew.Columns.Add(dcLatitude);
            DataColumn dcLongitude = new DataColumn("Lg");
            dtNew.Columns.Add(dcLongitude);
            //DataColumn dcmarkerImage = new DataColumn("MI");
            //dtNew.Columns.Add(dcmarkerImage);
            //DataColumn dcZipCode = new DataColumn("ZipCode");
            //dtNew.Columns.Add(dcZipCode);
            dtNew.AcceptChanges();
            foreach (MapItem mi in items)
            {
                DataRow dr = dtNew.NewRow();
                dr["Sc"] = mi.SubscriberID;
                //dr["MapAddress"] = mi.MapAddress;
                dr["Lt"] = mi.Latitude;
                dr["Lg"] = mi.Longitude;
                //dr["MI"] = mi.MarkerImage;
                //dr["ZipCode"] = mi.ZipCode;
                dtNew.Rows.Add(dr);
            }
            dtNew.AcceptChanges();

            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> resultMain = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dtNew.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dtNew.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                resultMain.Add(index.ToString(), result);
                index++;
            }
            dtNew.TableName = "MapPoints";

            return dtNew;
        }

        protected void loadReportGeoData(string type, StringBuilder Queries)
        {
            rvGeoLocation.ProcessingMode = ProcessingMode.Local;
            rvGeoLocation.Visible = true;
            rvGeoLocation.LocalReport.DataSources.Clear();
            switch (type)
            {
                case "GeoCanada":
                    rvGeoLocation.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_Canada.rdlc");
                    rvGeoLocation.LocalReport.DataSources.Add(
                        new ReportDataSource(rvGeoLocation.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Canada", Queries)));
                    break;
                case "GeoDomestic":
                    rvGeoLocation.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_Domestic.rdlc");
                    rvGeoLocation.LocalReport.DataSources.Add(
                        new ReportDataSource(rvGeoLocation.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Domestic", Queries)));
                    break;
                case "GeoInternational":
                    rvGeoLocation.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoBreakdown_by_Country.rdlc");
                    rvGeoLocation.LocalReport.DataSources.Add(
                        new ReportDataSource(rvGeoLocation.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_by_country", Queries)));
                    break;
                case "GeoPermissionCanada":
                    rvGeoLocation.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Canada.rdlc");
                    rvGeoLocation.LocalReport.DataSources.Add(
                        new ReportDataSource(rvGeoLocation.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Canada", Queries)));
                    break;
                case "GeoPermissionDomestic":
                    rvGeoLocation.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_Domestic.rdlc");
                    rvGeoLocation.LocalReport.DataSources.Add(
                        new ReportDataSource(rvGeoLocation.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_Domestic", Queries)));
                    break;
                case "GeoPermissionInternational":
                    rvGeoLocation.LocalReport.ReportPath = Server.MapPath(@"Reports/rpt_GeoPermission_by_Country.rdlc");
                    rvGeoLocation.LocalReport.DataSources.Add(
                        new ReportDataSource(rvGeoLocation.LocalReport.GetDataSourceNames()[0], GeographicalReport.GetGeographicalReportData(Master.clientconnections, "sp_rpt_Qualified_Breakdown_by_country", Queries)));
                    break;
            }
            rvGeoLocation.LocalReport.Refresh();
        }

        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        protected override void LoadPageFilters()
        {
            rgQuestions.DataSource = GetData();
            rgQuestions.DataBind();
        }
    }
}