using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Linq;
using Microsoft.Reporting.WebForms;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.blastsmanager
{
	public partial class bounces_main : ECN_Framework.WebPageHelper
    {
        List<ECN_Framework_Entities.Activity.Report.BounceByDomain> bdlist = new List<ECN_Framework_Entities.Activity.Report.BounceByDomain>();

        int _pagerCurrentPage = 1;

        public int pagerCurrentPage
        {
            set { _pagerCurrentPage = value; }
            get { return _pagerCurrentPage - 1; }
        }

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
            catch (Exception E)
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

        private string getBounceType()
        {
            try
            {
                return BounceType.SelectedItem.Value.ToString();
            }
            catch
            {
                return "*";
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "";
            Master.Heading = "Bounce Reports";
            Master.HelpContent = "<p><b>Bounces</b><br />Lists all email address who did not receive the email that was sent.<br /><br />Displays the <i>Bounce Time</i> the email Bounced, the <i>email address</i> and <i>BounceType</i> which would be a <i>softBounce</i> (for instance: email Inbox full) or a <i>hardBounce</i> (for instance: email address doesnot exist).";
            Master.HelpTitle = "Blast Manager";

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "blastpriv") 
            //|| KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "viewreport") 
            //|| KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser) 
            //|| Master.UserSession.CurrentUser.IsAdmin)
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Delivery_Report))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportBounces, KMPlatform.Enums.Access.View))
            {
                if (!(Page.IsPostBack))
                {
                    if (getUDFData() != string.Empty)
                    {
                        UnsubBouncesButton.Enabled = false;
                        ResendSoftBouncesButton.Enabled = false;
                    }



                    ViewState["sortOrder"] = "";
                    loadBounceTypes();
                    loadBounceTypesGrid(false);
                    if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportBounces, KMPlatform.Enums.Access.ViewDetails))
                    {
                        TabPanel2.Visible = true;
                        TabPanel3.Visible = true;
                        TabPanel4.Visible = true;
                        loadHardBouncesGrid();
                        loadSoftBouncesGrid();
                        loadOthersBouncesGrid();
                    }
                    else
                    {
                        TabPanel2.Visible = false;
                        TabPanel3.Visible = false;
                        TabPanel4.Visible = false;

                    }
                    loadDomainsGrid("PercBounced", "DESC");
                    ShowHideDownload();
                }
            }
            else
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
            }
        }

        public string sortOrder
        {
            get
            {
                if (ViewState["sortOrder"].ToString() == "DESC")
                {
                    ViewState["sortOrder"] = "ASC";
                }
                else
                {
                    ViewState["sortOrder"] = "DESC";
                }

                return ViewState["sortOrder"].ToString();
            }
            set
            {
                ViewState["sortOrder"] = value;
            }
        }

        public string sortField
        {
            get
            {
                return ViewState["sortField"].ToString();
            }
            set
            {
                ViewState["sortField"] = value;
            }
        }

        private void loadBounceTypes()
        {
            try
            {
                string bounceCodes = ConfigurationManager.AppSettings["globalBounceCodes"];
                SortedList bounceCodesSL = new SortedList();
                ECN_Framework_Common.Functions.StringTokenizer st = new ECN_Framework_Common.Functions.StringTokenizer(bounceCodes, ',');
                while (st.HasMoreTokens())
                {
                    string item = st.NextToken();
                    if ((!item.Equals("hardbounce")) || (!item.Equals("softbounce")))
                        BounceType.Items.Add(new ListItem(item.ToUpper(), item.ToLower()));
                }
                BounceType.Items.Insert(0, new ListItem("ALL TYPES", "*"));
            }
            catch
            {

            }
        }

        private DataTable loadBounceTypesGrid(bool isExport)
        {
            DataTable dt = new DataTable();

            if (getBlastID() > 0)
            {
                if (getUDFData() != string.Empty)
                {
                    dt = ECN_Framework_BusinessLayer.Activity.BlastActivityBounces.BlastReportWithUDF(getBlastID(), getUDFName(), getUDFData());
                }
                else
                {
                    dt = ECN_Framework_BusinessLayer.Activity.BlastActivityBounces.BlastReport(getBlastID());
                }
            }
            else
            {
                dt = ECN_Framework_BusinessLayer.Activity.BlastActivityBounces.BlastReportByCampaignItemID(getCampaignItemID());
            }

            BounceTypesGrid.DataSource = dt.DefaultView;
            BounceTypesGrid.DataBind();
            loadChart(dt, isExport);
            return dt;

        }

        protected void Export_Click(object sender, EventArgs e)
        {
            ExportReport(Response, dropdownExport.SelectedValue);
        }

        public string SaveChart(Chart chart)
        {
            loadBounceTypesGrid(true);
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            String tfile = Guid.NewGuid().ToString() + ".png";
            chart.AntiAliasing = AntiAliasingStyles.Graphics;
            chart.SaveImage(txtoutFilePath + tfile, ChartImageFormat.Png);
            return tfile;
        }

        private void ExportReport(HttpResponse response, string type)
        {
            string filename = SaveChart(chartBounceTypes);
            string txtoutFilePath = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/";
            ReportViewer1.LocalReport.ReportPath = "main\\blasts\\Report\\rpt_BlastsBounces.rdlc";
            ReportParameter[] ReportParameters = new ReportParameter[1];
            ReportParameters[0] = new ReportParameter("ReportParameter1", txtoutFilePath + filename);
            ReportViewer1.LocalReport.SetParameters(ReportParameters);
            ReportDataSource RDS = new ReportDataSource("DataSet1", loadBounceTypesGrid(true));
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(RDS);
            ReportViewer1.DataBind();
            ReportViewer1.LocalReport.Refresh();

            //Out params to pass into the Render method 
            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            string mimeType;
            if (type.Equals("PDF"))
            {
                mimeType = "application/pdf";
            }
            else
            {
                mimeType = "application/octet-stream";
            }
            string encoding = null;
            string extension;
            byte[] bytes = ReportViewer1.LocalReport.Render(type, null, out mimeType, out encoding, out extension, out streamids, out warnings);
            response.Clear();
            response.ContentType = mimeType;
            response.AppendHeader("Content-Disposition", String.Format("attachment; filename=Bounces.{0}", extension));
            response.OutputStream.Write(bytes.ToArray(), 0, (int)bytes.Length);
            response.End();
        }

        private void loadChart(DataTable dt, bool isExport)
        {
            try
            {
                chartBounceTypes.DataSource = dt;

                chartBounceTypes.Series["Series1"].ChartType = SeriesChartType.Pie;
                chartBounceTypes.Series["Series1"].XValueMember = "ACTIONVALUE";
                chartBounceTypes.Series["Series1"].YValueMembers = "TOTALBounces";
                chartBounceTypes.Series[0].Label = "#VALX";

                chartBounceTypes.ChartAreas[0].Area3DStyle.Enable3D = true;

                // Set pie labels to be outside the pie chart
                chartBounceTypes.Series[0]["PieLabelStyle"] = "Disabled";

                // Add a legend to the chart and dock it to the bottom-center
                chartBounceTypes.Legends.Add("Legend1");
                chartBounceTypes.Legends[0].Enabled = true;
                chartBounceTypes.Legends[0].Docking = Docking.Bottom;
                chartBounceTypes.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chartBounceTypes.Legends[0].IsEquallySpacedItems = true;
                chartBounceTypes.Legends[0].TextWrapThreshold = 0;
                chartBounceTypes.Legends[0].IsTextAutoFit = true;
                if (!isExport)
                {
                    chartBounceTypes.Legends[0].BackColor = System.Drawing.Color.Transparent;
                    chartBounceTypes.Legends[0].ShadowColor = System.Drawing.Color.Transparent;
                    chartBounceTypes.AntiAliasing = AntiAliasingStyles.Graphics;
                }
                else
                {
                    chartBounceTypes.Legends[0].BackColor = System.Drawing.Color.White;
                    chartBounceTypes.Legends[0].ShadowColor = System.Drawing.Color.White;
                    chartBounceTypes.AntiAliasing = AntiAliasingStyles.All;
                }
                chartBounceTypes.Series[0].LegendText = "#VALX (#PERCENT)";
                chartBounceTypes.DataBind();



            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;

            }
        }

        private void loadHardBouncesGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "bounce", "hardbounce", getISP(), pagerCurrentPage, BouncesPagerHard.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            BouncesGridHard.DataSource = ds.Tables[1].DefaultView;
            BouncesGridHard.CurrentPageIndex = 0;
            BouncesGridHard.DataBind();
            BouncesPagerHard.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        private void loadSoftBouncesGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "bounce", "softbounce", getISP(), pagerCurrentPage, BouncesPagerSoft.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            BouncesGridSoft.DataSource = ds.Tables[1].DefaultView;
            BouncesGridSoft.CurrentPageIndex = 0;
            BouncesGridSoft.DataBind();
            BouncesPagerSoft.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        private void loadOthersBouncesGrid()
        {
            DataSet ds = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "bounce", getBounceType(), getISP(), pagerCurrentPage, BouncesPagerOthers.PageSize, getUDFName(), getUDFData(), Master.UserSession.CurrentUser);

            BouncesGridOthers.DataSource = ds.Tables[1].DefaultView;
            BouncesGridOthers.CurrentPageIndex = 0;
            BouncesGridOthers.DataBind();
            BouncesPagerOthers.RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

        }

        private void loadDomainsGrid(string sortfield, string sortdirection)
        {
            sortField = sortfield;
            bdlist = ECN_Framework_BusinessLayer.Activity.Report.BounceByDomain.Get(getBlastID(), getCampaignItemID());

            if (sortfield.Equals("Domain"))
                bdlist = bdlist.OrderBy(x => x.Domain).ToList();
            else if (sortfield.Equals("Hard"))
                bdlist = bdlist.OrderBy(x => x.Hard).ToList();
            else if (sortfield.Equals("Soft"))
                bdlist = bdlist.OrderBy(x => x.Soft).ToList();
            else if (sortfield.Equals("PercBounced"))
                bdlist = bdlist.OrderBy(x => x.PercBounced).ToList();
            else if (sortfield.Equals("TotalBounces"))
                bdlist = bdlist.OrderBy(x => x.TotalBounces).ToList();
            else if (sortfield.Equals("MessagesSent"))
                bdlist = bdlist.OrderBy(x => x.MessagesSent).ToList();
            else if (sortfield.Equals("Other"))
                bdlist = bdlist.OrderBy(x => x.Other).ToList();



            if (sortdirection.Equals("DESC"))
            {
                bdlist.Reverse();
            }
            bdlist = bdlist.Take(Int32.Parse(dropdownView.SelectedValue)).ToList();

            gvDomains.DataSource = bdlist;
            gvDomains.DataBind();
        }

        protected void gvDomains_Sorting(object sender, GridViewSortEventArgs e)
        {
            loadDomainsGrid(e.SortExpression, sortOrder);
        }

        public void UnsubscribeBounces(object sender, System.EventArgs e)
        {
            if (getBlastID() > 0)
            {
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.UnsubscribeBounces(getBlastID(), getISP(), Master.UserSession.CurrentUser);
            }
            else
            {
                //get blasts by campaign item id
               List<ECN_Framework_Entities.Communicator.BlastAbstract> blasts = ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemID_NoAccessCheck(getCampaignItemID(), false);
                foreach(ECN_Framework_Entities.Communicator.Blast b in blasts)
                {
                    if(b.TestBlast.ToLower().Equals("n"))
                        ECN_Framework_BusinessLayer.Communicator.EmailGroup.UnsubscribeBounces(b.BlastID, getISP(), Master.UserSession.CurrentUser);
                }
            }

            if (getISP() != string.Empty)
            {
                if(getBlastID() > 0)
                    Response.Redirect("reports.aspx?BlastID=" + getBlastID() + "&isp=" + getISP());
                else
                    Response.Redirect("reports.aspx?CampaignItemID=" + getCampaignItemID() + "&isp=" + getISP());
            }
            else
            {
                if(getBlastID() > 0)
                    Response.Redirect("reports.aspx?BlastID=" + getBlastID());
                else
                    Response.Redirect("reports.aspx?CampaignItemID=" + getCampaignItemID());
            }
                

        }

        public void ResendSoftBounces(object sender, System.EventArgs e)
        {
            int blastID = getBlastID();
            ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
            emailFunctions.MSPickupReSendBounceThread(blastID, System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"], Master.UserSession.CurrentBaseChannel.ChannelURL, Master.UserSession.CurrentBaseChannel.BounceDomain);
            Response.Redirect("reports.aspx?BlastID=" + blastID);
        }

        protected void BounceTypeOthers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BouncesGridOthers.CurrentPageIndex = 0;
            BouncesPagerOthers.CurrentPage = 1;
            BouncesPagerOthers.CurrentIndex = 0;
            loadOthersBouncesGrid();
        }

        protected void BouncesPagerHard_IndexChanged(object sender, System.EventArgs e)
        {
            pagerCurrentPage = BouncesPagerHard.CurrentPage;
            loadHardBouncesGrid();
        }

        protected void BouncesPagerOthers_IndexChanged(object sender, System.EventArgs e)
        {
            pagerCurrentPage = BouncesPagerOthers.CurrentPage;
            loadOthersBouncesGrid();
        }

        protected void BouncesPagerSoft_IndexChanged(object sender, System.EventArgs e)
        {
            pagerCurrentPage = BouncesPagerSoft.CurrentPage;
            loadSoftBouncesGrid();
        }

        public void downloadBouncedEmails_Soft(object sender, System.EventArgs e)
        {
            downloadBouncedEmails("softbounce", DownloadTypeSoft.SelectedItem.Value.ToString());
        }

        public void downloadBouncedEmails_Hard(object sender, System.EventArgs e)
        {
            downloadBouncedEmails("hardbounce", DownloadTypeHard.SelectedItem.Value.ToString());
        }

        public void downloadBouncedEmails(string bounceType, string downloadType)
        {
            ArrayList columnHeadings = new ArrayList();
            IEnumerator aListEnum = null;

            DataTable emailstable;
            //string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

            string newline = "";
            DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-bounced-emails" + downloadType;
            //string outfileName = txtoutFilePath + tfile;

            //if (!Directory.Exists(txtoutFilePath))
            //{
            //    Directory.CreateDirectory(txtoutFilePath);
            //}

            //if (File.Exists(outfileName))
            //{
            //    File.Delete(outfileName);
            //}

            //TextWriter txtfile = File.AppendText(outfileName);
            emailstable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.DownloadBlastReportDetails(getBlastID() > 0 ? getBlastID() : getCampaignItemID(), getBlastID() > 0 ? false : true, "bounce", bounceType.Equals("*") ? "" : bounceType, getISP(), Master.UserSession.CurrentUser);

            if(downloadType == ".xls")
            {
                ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToExcelFromDataTbl<DataRow>(emailstable.AsEnumerable().ToList(), tfile);
            }
            else
            {
                string emailsSCV = ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.GetCsvFromDataTable(emailstable);
                if (downloadType == ".csv")
                {
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToCSV(emailsSCV, tfile);
                }
                else
                {
                    
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToTXT(emailsSCV, tfile);
                }
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
            //    string colData = "";
            //    while (aListEnum.MoveNext())
            //    {
            //        colData = CleanString(dr[aListEnum.Current.ToString()].ToString());

            //        newline += colData + (downloadType == ".xls" ? "\t" : ", ");
            //    }
            //    txtfile.WriteLine(newline);
            //}
            //txtfile.Close();

            //if (downloadType == ".xls")
            //{
            //    Response.ContentType = "application/vnd.ms-excel";
            //}
            //else
            //{
            //    Response.ContentType = "text/csv";
            //}
            //Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
            //Response.WriteFile(outfileName);
            //Response.Flush();
            //Response.End();
        }

        private static string CleanString(string text)
        {

            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            return text;
        }

        public void downloadBouncedEmails_Others(object sender, System.EventArgs e)
        {
            downloadBouncedEmails(BounceType.SelectedValue, DownloadTypeOthers.SelectedItem.Value.ToString());
        }

        protected void btnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("reports.aspx?" + (getBlastID() > 0 ? "BlastID=" + getBlastID() : "CampaignItemID=" + getCampaignItemID()) + (getUDFData() != string.Empty ? "&UDFName=" + getUDFName() + "&UDFdata=" + getUDFData() : ""));
        }

        protected void dropdownView_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDomainsGrid(sortField, ViewState["sortOrder"].ToString());
        }

        protected void gvDomains_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Text = e.Row.Cells[6].Text.ToString() + "%";

            }
        }

        protected void ShowHideDownload()
        {
            if (!KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReportBounces, KMPlatform.Enums.Access.Download))
            {
                dropdownExport.Visible = false;
                ltbnExport.Visible = false;
                DownloadPanelHard.Visible = false;
                DownloadPanelSoft.Visible = false;
                DownloadPanelOthers.Visible = false;
            }
        }

    }
}
