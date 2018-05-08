using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using Microsoft.Reporting.WebForms;
using ECN_Framework_Common.Objects;
using Ecn.Communicator.Main.Interfaces;
using Ecn.Communicator.Main.Helpers;
using ecn.communicator.main.blasts.Interfaces;
using ecn.communicator.main.blasts.Helpers;

namespace ecn.communicator.blastsmanager
{
    public partial class opens : ECN_Framework.WebPageHelper
    {
        private const string BlastOpensReportName = "main\\blasts\\Report\\rpt_BlastOpens.rdlc";
        private const string BlastOpensByTimeReportName = "main\\blasts\\Report\\rpt_BlastOpensByTime.rdlc";
        private const string OpensbyTimeFileNameInHeader = "OpensbyTime";
        private const string EmailClientUsageFileNameInHeader = "EmailClientUsage";
        private const string PDFType = "PDF";
        private const string PDFMimeType = "application/pdf";
        private const string OctetStreamMimeType = "application/octet-stream";

        int _pagerCurrentPage = 1;
        IList<ECN_Framework_Entities.Activity.EmailClients> eclist;
        IList<ECN_Framework_Entities.Activity.Platforms> plist;
        List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> opensbyTimelist = new List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime>();
        List<ECN_Framework_Entities.Activity.BlastActivityOpens> openslist = new List<ECN_Framework_Entities.Activity.BlastActivityOpens>();

        private IEmailClients EmailClients;
        private IPlatforms Platforms;
        private IMasterCommunicator MasterCommunicator;
        private IServer HttpServer;
        private IBlastReport BlastReport;
        private IReportViewer ReportViewer;
        public opens()
        {
            EmailClients = new EmailClientsAdapter();
            Platforms = new PlatformsAdapter();
            eclist = EmailClients.Get();
            plist = Platforms.Get();

            MasterCommunicator = new MasterCommunicatorAdapter(this.Master);
            HttpServer = new ServerAdapter(this.Server);
            BlastReport = new BlastReportAdapter();
            ReportViewer = new ReportViewerAdapter(this.ReportViewer1);
        }
        public opens(IEmailClients emailClients,
            IPlatforms platforms,
            IMasterCommunicator masterCommunicator,
            IServer httpServer,
            IBlastReport blastReport,
            IReportViewer reportViewer)
        {
            EmailClients = emailClients;
            Platforms = platforms;
            eclist = EmailClients.Get();
            plist = Platforms.Get();

            MasterCommunicator = masterCommunicator;
            HttpServer = httpServer;
            BlastReport = blastReport;
            ReportViewer = reportViewer;
        }

        #region Page Properties
        private int getBlastID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getCampaignItemID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["CampaignItemID"].ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private string getISP()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
                    return Request.QueryString["isp"].ToString();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public int pagerCurrentPage
        {
            set
            {
                _pagerCurrentPage = value;
            }
            get
            {
                return _pagerCurrentPage - 1;
            }
        }

        public string getUDFName()
        {
            try
            {
                return Request.QueryString["UDFName"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getUDFData()
        {
            try
            {
                return Request.QueryString["UDFdata"].ToString();
            }
            catch
            {
                return string.Empty;
            }

        }

        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "";
            Master.Heading = "Opens Report";
            Master.HelpContent = "<p><b>Opens</b><br />Lists all email address who opened the email that was sent in the blast.<br /><br />Displays the <br />-&nbsp;&nbsp;<i>Open time</i><br />-&nbsp;&nbsp;the <i>email address</i> of the recepient opened it <br />-&nbsp;&nbsp;the <i>information</i> about the 'email client' the recepient used to open the email.";
            Master.HelpTitle = "Blast Manager";

            if (getBlastID() > 0)
            {
                ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(getBlastID(), false);
                if (b.BlastType.ToLower().Equals("layout") || b.BlastType.ToLower().Equals("noopen"))
                {
                    btnOpensbyTime.Visible = false;
                }
            }
            else if (getCampaignItemID() > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID_NoAccessCheck(getCampaignItemID(), false);
                if (ci.CampaignItemType.ToLower().Equals("layout") || ci.CampaignItemType.ToLower().Equals("noopen"))
                {
                    btnOpensbyTime.Visible = false;
                }
            }
            if (!IsPostBack)
            {//configure how page displays to user
                if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportOpens, KMPlatform.Enums.Access.ViewDetails))
                {
                    if (!(Page.IsPostBack))
                    {
                        btnActiveOpens.Visible = true;
                        btnAllOpens.Visible = true;

                        loadGrid("activeopens");
                    }

                }
                else
                {
                    btnActiveOpens.Visible = false;
                    btnAllOpens.Visible = false;

                    //load the open by time and display
                    openbyTime_CreateChart();
                    loadOpensGrid();
                    ShowHideDownload();
                }
            }
        }

        private void loadGrid(string ReportType)
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "open", ReportType, getISP(), pagerCurrentPage, OpensGrid.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            if (ReportType.ToLower() == "activeopens")
            {
                ActiveGrid.DataSource = ds.Tables[0];
                ActiveGrid.DataBind();
                phActiveGrid.Visible = true;
                phOpensGrid.Visible = false;
                phAllOpens.Visible = false;
                phBrowserStats.Visible = false;
                btnActiveOpens.CssClass = "selected";
                btnOpensbyTime.CssClass = "";
                btnAllOpens.CssClass = "";
                btnBrowserStats.CssClass = "";
            }
            else
            {
                OpensGrid.DataSource = ds.Tables[1];
                OpensGrid.CurrentPageIndex = 0;
                OpensGrid.DataBind();

                OpensPager.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                phActiveGrid.Visible = false;
                phOpensGrid.Visible = false;
                phBrowserStats.Visible = false;
                phAllOpens.Visible = true;
                btnActiveOpens.CssClass = "";
                btnOpensbyTime.CssClass = "";
                btnBrowserStats.CssClass = "";
                btnAllOpens.CssClass = "selected";
                if (ds.Tables[1].Rows.Count > 0)
                {
                    OpensPager.Visible = true;
                    DownloadPanel.Visible = true;
                }
                else
                {
                    OpensPager.Visible = false;
                    DownloadPanel.Visible = false;
                }
            }
            ds.Dispose();
        }

        private void loadChart()
        {
            phActiveGrid.Visible = false;
            phOpensGrid.Visible = false;
            phAllOpens.Visible = false;
            phBrowserStats.Visible = true;
            btnBrowserStats.CssClass = "selected";
            btnOpensbyTime.CssClass = "";
            btnActiveOpens.CssClass = "";
            btnAllOpens.CssClass = "";
            var query = (from o in openslist
                         join ec in eclist on o.EmailClientID equals ec.EmailClientID
                         where o.PlatformID != 5 && o.EmailClientID != 15
                         group o by ec.EmailClientName into gp
                         orderby gp.Count() descending
                         select new { EmailClientName = gp.Key, Usage = (gp.Count()) }).Distinct();

            chartBrowserStats.DataSource = query;

            chartBrowserStats.Series["Series1"].ChartType = SeriesChartType.Pie;
            chartBrowserStats.Series["Series1"].XValueMember = "EmailClientName";
            chartBrowserStats.Series["Series1"].YValueMembers = "Usage";
            chartBrowserStats.Series[0].Label = "#VALX";
            chartBrowserStats.ChartAreas[0].Area3DStyle.Enable3D = true;
            chartBrowserStats.Series[0]["PieLabelStyle"] = "Disabled";
            chartBrowserStats.Legends.Add("Legend1");
            chartBrowserStats.Legends[0].Enabled = true;
            chartBrowserStats.Legends[0].Docking = Docking.Right;
            chartBrowserStats.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
            chartBrowserStats.Legends[0].IsEquallySpacedItems = true;
            chartBrowserStats.Legends[0].TextWrapThreshold = 0;
            chartBrowserStats.Legends[0].IsTextAutoFit = true;
            chartBrowserStats.Legends[0].BackColor = System.Drawing.Color.Transparent;
            chartBrowserStats.Legends[0].ShadowColor = System.Drawing.Color.Transparent;
            chartBrowserStats.AntiAliasing = AntiAliasingStyles.Graphics;
            chartBrowserStats.Series[0].LegendText = "#VALX (#PERCENT)";
            chartBrowserStats.DataBind();
        }

        private void loadPlatformList()
        {
            var querytotalcount = (from op in openslist
                                   select new { TotalCount = op.EmailID });

            lblTotalOpens.Text = "Total Opens: " + querytotalcount.Count().ToString();

            querytotalcount = (from op in openslist
                               where op.PlatformID != 5 && op.EmailClientID != 15
                               select new { TotalCount = op.EmailID });

            var query = (from p in plist
                         join o in openslist on p.PlatformID equals o.PlatformID
                         where o.PlatformID != 5 && o.EmailClientID != 15
                         group o by p.PlatformName into gp
                         orderby gp.Count() descending
                         select new { PlatformName = gp.Key, Opens = (gp.Count()), Usage = (Math.Round(((float)gp.Count() * 100 / querytotalcount.Count()), 2)).ToString() + "%" }).Distinct().ToList();

            dlPlatform.DataSource = query;
            dlPlatform.DataBind();
        }

        protected void dlPlatform_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                var querytotalcount = (from op in openslist
                                       where op.PlatformID != 5 && op.EmailClientID != 15
                                       select new { TotalCount = op.EmailID });

                var queryPlatformID = (from p in plist
                                       where p.PlatformName == ((Label)e.Item.FindControl("lblPlatform")).Text
                                       select new { PlatformID = p.PlatformID }).Single();

                var query = (from o in openslist
                             join ec in eclist on o.EmailClientID equals ec.EmailClientID
                             where o.PlatformID == Int32.Parse(queryPlatformID.PlatformID.ToString())
                             group o by ec.EmailClientName into gp
                             orderby gp.Count() descending
                             select new { EmailClientName = gp.Key, Opens = (gp.Count()), Usage = (Math.Round(((float)gp.Count() * 100 / querytotalcount.Count()), 2)).ToString() + "%" }).Distinct();

                ((GridView)e.Item.FindControl("gvEmailClients")).DataSource = query;
                ((GridView)e.Item.FindControl("gvEmailClients")).DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvEmailClients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Equals("0%"))
                {
                    e.Row.Cells[2].Text = "< 0.01%";
                }
            }

        }

        protected void Export_Click(object sender, EventArgs e)
        {
            BrowserStats_ExportReport(new HttpResponseAdapter(Response), dropdownExport.SelectedValue);
        }

        public string browserStats_SaveChart(Chart chart)
        {
            browserStats_CreateChart();
            var txtoutFilePath = HttpServer.MapPath(string.Format("{0}/customers/{1}/downloads/", ConfigurationManager.AppSettings["Images_VirtualPath"], MasterCommunicator.GetCustomerID()));
            var tfile = Guid.NewGuid().ToString() + ".png";
            chart.AntiAliasing = AntiAliasingStyles.Graphics;
            chart.SaveImage(txtoutFilePath + tfile, ChartImageFormat.Png);
            return tfile;
        }

        private void BrowserStats_ExportReport(HttpResponseBase response, string type)
        {
            var filename = browserStats_SaveChart(chartBrowserStats);
            var RDS = new ReportDataSource();
            if (getBlastID() > 0 || getCampaignItemID() > 0)
            {
                RDS = (getBlastID() > 0)
                    ? new ReportDataSource("DataSet1", ECN_Framework_BusinessLayer.Activity.Report.BlastActivityOpensReport.Get(getBlastID(), false))
                    : new ReportDataSource("DataSet1", ECN_Framework_BusinessLayer.Activity.Report.BlastActivityOpensReport.Get(getCampaignItemID(), true));
            }

            InitializeReportViewer(response, type, filename, RDS, BlastOpensReportName, EmailClientUsageFileNameInHeader);
        }

        private void InitializeReportViewer(HttpResponseBase response, string type, string filename, ReportDataSource RDS, string reportPath, string filenameInHeader)
        {
            var txtoutFilePath = string.Format("{0}/customers/{1}/downloads/", ConfigurationManager.AppSettings["Image_DomainPath"], MasterCommunicator.GetCustomerID());
            ReportViewer1.LocalReport.ReportPath = reportPath;
            var ReportParameters = new ReportParameter[1];
            ReportParameters[0] = new ReportParameter("ReportParameter1", txtoutFilePath + filename);
            ReportViewer.SetParameters(ReportParameters);
            var rdsBlastReport = new ReportDataSource("DS_BlastReport", BlastReport.Get(getBlastID()));

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rdsBlastReport);
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            ReportViewer.DataBind();
            ReportViewer1.LocalReport.Refresh();

            //Out params to pass into the Render method 
            Warning[] warnings = null;
            string[] streamids = null;

            var mimeType = type.Equals(PDFType) ? PDFMimeType : OctetStreamMimeType;
            string encoding = null;
            string extension = null;
            var bytes = ReportViewer.Render(type, null, out mimeType, out encoding, out extension, out streamids, out warnings);

            response.Clear();
            response.ContentType = mimeType;
            response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}.{1}", filenameInHeader, extension));
            response.OutputStream.Write(bytes.ToArray(), 0, bytes.Count);
            response.End();
        }

        #region Downloads Open Emails

        public void downloadOpenEmails(object sender, System.EventArgs e)
        {
            string newline = "";
            DataTable emailstable;
            ArrayList columnHeadings = new ArrayList();
            IEnumerator aListEnum = null;

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            string downloadType = DownloadType.SelectedItem.Value.ToString();
            string opensType = OpensTypeRBList.SelectedItem.Value.ToString();

            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-open-emails" + downloadType;
            string outfileName = txtoutFilePath + tfile;

            if (!Directory.Exists(txtoutFilePath))
            {
                Directory.CreateDirectory(txtoutFilePath);
            }
            if (File.Exists(outfileName))
            {
                File.Delete(outfileName);
            }

            TextWriter txtfile = File.AppendText(outfileName);

            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "open", opensType, getISP(), Master.UserSession.CurrentUser);

            string endFile = string.Empty;
            if (downloadType.Equals(".csv") || downloadType.Equals(".txt"))
            {
                endFile = ECN_Framework_Common.Functions.DataTableFunctions.ToCSV(emailstable);
            }
            else if (downloadType.Equals(".xls"))
            {
                endFile = ECN_Framework_Common.Functions.DataTableFunctions.ToTabDelimited(emailstable);
            }


            //for (int i = 0; i < emailstable.Columns.Count; i++)
            //{
            //    columnHeadings.Add(emailstable.Columns[i].ColumnName.ToString());
            //}



            //aListEnum = columnHeadings.GetEnumerator();
            //while (aListEnum.MoveNext())
            //{
            //    newline += aListEnum.Current.ToString() + (downloadType == ".xls" ? "\t" : ", ");
            //}
            //txtfile.WriteLine(newline);



            //foreach (DataRow dr in emailstable.Rows)
            //{
            //    newline = "";
            //    aListEnum.Reset();
            //    while (aListEnum.MoveNext())
            //    {
            //        newline += dr[aListEnum.Current.ToString()].ToString() + (downloadType == ".xls" ? "\t" : ", ");
            //    }
            //    txtfile.WriteLine(newline);
            //}

            txtfile.Write(endFile);
            txtfile.Close();

            if (downloadType == ".xls")
            {
                Response.ContentType = "application/vnd.ms-excel";
            }
            else
            {
                Response.ContentType = "text/csv";
            }
            Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
        }


        #endregion

        protected void btnActiveOpens_Click(object sender, System.EventArgs e)
        {
            loadGrid("activeopens");
        }

        protected void btnOpensbyTime_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportOpens, KMPlatform.Enums.Access.View))
            {
                openbyTime_CreateChart();
                loadOpensGrid();
                ShowHideDownload();
            }
        }

        protected void openbyTime_CreateChart()
        {
            if (getBlastID() > 0 || getCampaignItemID() > 0)
            {
                if (getBlastID() > 0)
                {
                    opensbyTimelist = ECN_Framework_BusinessLayer.Activity.Report.BlastOpensByTime.GetByBlastID(getBlastID());
                }
                else
                {
                    opensbyTimelist = ECN_Framework_BusinessLayer.Activity.Report.BlastOpensByTime.GetByCampaignItemID(getCampaignItemID());
                }
            }

            loadOpensChart();
        }

        protected void btnAllOpens_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportOpens, KMPlatform.Enums.Access.ViewDetails))
            {

                loadGrid("allopens");
                ShowHideDownload();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        protected void btnBrowserStats_Click(object sender, System.EventArgs e)
        {
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportOpens, KMPlatform.Enums.Access.View))
            {
                browserStats_CreateChart();
                loadPlatformList();
                ShowHideDownload();
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }

        }

        protected void browserStats_CreateChart()
        {
            if (getBlastID() > 0 || getCampaignItemID() > 0)
            {
                if (getBlastID() > 0)
                {
                    openslist = ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByBlastID(getBlastID());
                }
                else
                {
                    openslist = ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByCampaignItemID(getCampaignItemID());
                }
            }

            loadChart();
        }


        private void loadOpensChart()
        {
            phActiveGrid.Visible = false;
            phOpensGrid.Visible = true;
            phAllOpens.Visible = false;
            phBrowserStats.Visible = false;
            btnBrowserStats.CssClass = "";
            btnOpensbyTime.CssClass = "selected";
            btnActiveOpens.CssClass = "";
            btnAllOpens.CssClass = "";

            try
            {
                chtOpensbyTime.DataSource = opensbyTimelist;
                chtOpensbyTime.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chtOpensbyTime.BorderlineDashStyle = ChartDashStyle.Solid;
                chtOpensbyTime.BorderWidth = 8;

                chtOpensbyTime.Series.Add("OpensCount");
                chtOpensbyTime.Series["OpensCount"].XValueMember = "Hour";
                chtOpensbyTime.Series["OpensCount"].YValueMembers = "Opens";
                chtOpensbyTime.Series["OpensCount"].ChartType = SeriesChartType.Column;
                chtOpensbyTime.Series["OpensCount"].IsVisibleInLegend = true;
                chtOpensbyTime.Series["OpensCount"].ShadowOffset = 3;
                chtOpensbyTime.Series["OpensCount"].ToolTip = "#VALY{G}";
                chtOpensbyTime.Series["OpensCount"].BorderWidth = 3;
                chtOpensbyTime.Series["OpensCount"].Color = System.Drawing.Color.SteelBlue;
                chtOpensbyTime.Series["OpensCount"].IsValueShownAsLabel = true;

                chtOpensbyTime.ChartAreas.Add("ChartArea1");
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisX.Title = "Hour";
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisY.Title = "Opens Count";
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = true;
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = true;
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtOpensbyTime.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
                chtOpensbyTime.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.Transparent;
                chtOpensbyTime.ChartAreas["ChartArea1"].ShadowColor = System.Drawing.Color.Transparent;

                chtOpensbyTime.Height = 450;
                chtOpensbyTime.Width = 800;

                //Adding Title
                chtOpensbyTime.Titles.Add("Title1");
                chtOpensbyTime.Titles["Title1"].Text = "";

                //Adding legend
                chtOpensbyTime.Legends.Add("Legends1");

                chtOpensbyTime.Legends[0].Enabled = true;
                chtOpensbyTime.Legends[0].Docking = Docking.Bottom;
                chtOpensbyTime.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chtOpensbyTime.Legends[0].IsEquallySpacedItems = true;
                chtOpensbyTime.Legends[0].TextWrapThreshold = 0;
                chtOpensbyTime.Legends[0].IsTextAutoFit = true;
                chtOpensbyTime.Legends[0].BackColor = System.Drawing.Color.Transparent;
                chtOpensbyTime.Legends[0].ShadowColor = System.Drawing.Color.Transparent;


                chtOpensbyTime.DataBind();
            }
            catch (Exception ex)
            {
                lblOpensByTime.Text = ex.Message;
            }

        }

        protected void OpenbyTimeExport_Click(object sender, EventArgs e)
        {
            OpenbyTime_ExportReport(new HttpResponseAdapter(Response), drpExportOpensByTime.SelectedValue);
        }

        public string OpenbyTime_SaveChart(Chart chart)
        {
            openbyTime_CreateChart();
            string txtoutFilePath = HttpServer.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + MasterCommunicator.GetCustomerID() + "/downloads/");
            String tfile = Guid.NewGuid().ToString() + ".png";
            chart.AntiAliasing = AntiAliasingStyles.Graphics;
            chart.SaveImage(txtoutFilePath + tfile, ChartImageFormat.Png);
            return tfile;
        }

        private void OpenbyTime_ExportReport(HttpResponseBase response, string type)
        {
            var filename = OpenbyTime_SaveChart(chtOpensbyTime);
            var RDS = new ReportDataSource();
            if (getBlastID() > 0 || getCampaignItemID() > 0)
            {
                RDS = (getBlastID() > 0)
                    ? new ReportDataSource("DataSet1", ECN_Framework_BusinessLayer.Activity.Report.BlastOpensByTime.GetByBlastID(getBlastID()))
                    : new ReportDataSource("DataSet1", ECN_Framework_BusinessLayer.Activity.Report.BlastOpensByTime.GetByCampaignItemID(getCampaignItemID()));
            }

            InitializeReportViewer(response, type, filename, RDS, BlastOpensByTimeReportName, OpensbyTimeFileNameInHeader);
        }

        private void loadOpensGrid()
        {
            gvOpens.DataSource = opensbyTimelist;
            gvOpens.DataBind();
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        protected void OpensPager_IndexChanged(object sender, EventArgs e)
        {
            pagerCurrentPage = OpensPager.CurrentPage;
            loadGrid("allopens");
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportOpens, KMPlatform.Enums.Access.Download))
            {
                DownloadPanel.Visible = false;
            }
            else
            {
                DownloadPanel.Visible = true;
            }

        }

    }
}