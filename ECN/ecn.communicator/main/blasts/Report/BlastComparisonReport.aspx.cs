using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using System.Web.UI.DataVisualization.Charting;
using KM.Common;
using Microsoft.Reporting.WebForms;
using Enums = ECN_Framework_Common.Objects.Enums;

namespace ecn.communicator.blasts.reports
{
    using Role = KM.Platform.User;

    public partial class BlastComparisonReport : ECN_Framework.WebPageHelper
    {
        private const string ColumnBlastId = "BlastID";
        private const string ColumnEmailSubject = "EmailSubject";
        private const string ColumnSendTime = "SendTime";
        private const string ColumnTOtalSent = "TotalSent";
        private const string ColumnOpens = "Opens";
        private const string ColumnClicks = "Clicks";
        private const string ColumnBounces = "Bounces";
        private const string ColumnOptsOut = "OptOuts";
        private const string ColumnComplaints = "Complaints";
        private const string ColumnOpensPerc = "OpensPerc";
        private const string ColumnClicksPerc = "ClicksPerc";
        private const string ColumnBouncesPerc = "BouncesPerc";
        private const string ColumnOptOutsPerc = "OptOutsPerc";
        private const string ColumnComplaintsPerc = "ComplaintsPerc";
        private const string ActionTypeCodeOpen = "open";
        private const string ActionTypeCodeClick = "click";
        private const string ActionTypeCodeBounce = "bounce";
        private const string ActionTypeCodeSubscribe = "subscribe";
        private const string ActionTypeCodeComplaint = "complaint";
        private const string SentOnTemplate = "Sent {0} on ";
        private const string PercentString = "%";
        private const string SortSendTimeDesc = "SendTime DESC";
        private const string SortBlastIdDesc = "BlastID DESC";
        private const string ZeroString = "0";
        private const string ZeroPercentString = "0%";
        private const string ErrorSelectAtLeastOneBlast = "Please select at least one blast";
        private const string ChartArea1 = "ChartArea1";
        private const string SeriesToolTip = "#VALY{G}";
        private const string SeriesLabel = "#VALY%";
        private const string BlastComparisonText = "Blast Comparision";
        private const string Legends1 = "Legends1";
        private const string NameBlasts = "Blasts";
        private const string ColumnOptOuts1 = "Opt-outs";
        public ArrayList selectedBlasts = new ArrayList();
        public ArrayList selectedBlastsNames = new ArrayList();
        public ArrayList selectedReportOn = new ArrayList();
        public ArrayList selectedReportOnNames = new ArrayList();
        List<ECN_Framework_Entities.Activity.Report.BlastComparision> bclist = new List<ECN_Framework_Entities.Activity.Report.BlastComparision>();

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
            PanelExport.Visible = false;
            gvChartData.Visible = false;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "chart report";
            Master.Heading = "";
            Master.HelpContent = "<img align='right' src='/ecn.images/images/icoblasts.gif'><b>Summary Reports</b><br>Gives a summary of Clicks, Bounces and Subscription reports.<br /><br /><b>Latest Clicks</b><br />Lists 10 recent URL clicks in the recent blasts sent.<br /><br /><b>Latest Bounces</b><br />Lists 10 latest email address bounced in the rencet Blasts. <i>Blast</i> column lists the email blast for which the bounced email address was assigned.<br /><i>Bounce Type</i> lists the type of bounce, which would be a <i>softBounce</i> (for instance: email Inbox full) or a <i>hardBounce</i> (for instance: email address doesnot exist).<br /><br /><b>Latest Subscription Changes</b><br />Gives a report of 15 recent email addresses subscribed or unsubscribed.";
            Master.HelpTitle = "Blast Manager";
            if (!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastComparisonReport, KMPlatform.Enums.Access.View))
                {
                    fromDateTxt.Text = DateTime.Now.AddMonths(-6).ToString("MM/dd/yyyy");
                    toDateTxt.Text = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
                    loadCustomersDR();
                    loadCampaignsDR();
                    loadUsersDR();
                    loadGroupsDR();
                    loadBlastListBox();
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
            }
            //if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                customerListBox.Enabled = true;
            }
            else
            {
                customerListBox.Enabled = false;
            }
        }

        public void GetData()
        {
            foreach (ListItem item in BlastListBox.Items)
            {
                if (item.Selected)
                {
                    selectedBlasts.Add(item.Value.ToString());
                }
            }
            if (selectedBlasts.Count > 0)
            {
                string sqlparam = "";
                for (int i = 0; i < selectedBlasts.Count; i++)
                {
                    sqlparam += "<Blasts BlastID=\"" + selectedBlasts[i] + "\"/>";
                }
                bclist = ECN_Framework_BusinessLayer.Activity.Report.BlastComparision.Get(sqlparam);
            }

        }

        public void CreateChart()
        {
            try
            {
                GetData();

                if (bclist.Count == 0)
                {
                    throwECNException(ErrorSelectAtLeastOneBlast);
                    return;
                }

                foreach (ListItem item in ReportOn.Items)
                {
                    if (item.Selected)
                    {
                        selectedReportOn.Add(item.Value);
                        selectedReportOnNames.Add(item.Text);
                    }
                }

                AddSeries();

                chtBlastComparision.BorderColor = Color.FromArgb(26, 59, 105);
                chtBlastComparision.BorderlineDashStyle = ChartDashStyle.Solid;
                chtBlastComparision.BorderWidth = 8;
                chtBlastComparision.ChartAreas.Add(ChartArea1);
                chtBlastComparision.ChartAreas[ChartArea1].AxisX.Title = NameBlasts;
                chtBlastComparision.ChartAreas[ChartArea1].AxisX.MajorGrid.Enabled = true;
                chtBlastComparision.ChartAreas[ChartArea1].AxisY.MajorGrid.Enabled = true;
                chtBlastComparision.ChartAreas[ChartArea1].AxisX.MajorGrid.LineColor = Color.LightGray;
                chtBlastComparision.ChartAreas[ChartArea1].AxisY.MajorGrid.LineColor = Color.LightGray;
                chtBlastComparision.ChartAreas[ChartArea1].AxisX.LineColor = Color.LightGray;
                chtBlastComparision.ChartAreas[ChartArea1].AxisY.LineColor = Color.LightGray;
                chtBlastComparision.ChartAreas[ChartArea1].AxisX.Interval = 1;
                chtBlastComparision.Height = 500;
                chtBlastComparision.Width = 900;
                chtBlastComparision.Titles.Add(BlastComparisonText);
                chtBlastComparision.Legends.Add(Legends1);
                chtBlastComparision.Legends[0].Enabled = true;
                chtBlastComparision.Legends[0].Docking = Docking.Bottom;
                chtBlastComparision.Legends[0].Alignment = StringAlignment.Center;
                chtBlastComparision.Legends[0].IsEquallySpacedItems = true;
                chtBlastComparision.Legends[0].TextWrapThreshold = 0;
                chtBlastComparision.Legends[0].IsTextAutoFit = true;

                createGridDataTable();
            }
            catch (Exception ex)
            {
                throwECNException(ex.Message);
            }
        }

        private void AddSeries()
        {
            for (var i = 0; i < selectedReportOn.Count; i++)
            {
                var series = new Series(selectedReportOnNames[i].ToString())
                {
                    ChartType = SeriesChartType.Line,
                    ShadowOffset = 3
                };

                chtBlastComparision.Series.Add(series);
                var query = (from src in bclist
                             where src.ActionTypeCode == selectedReportOn[i].ToString()
                             orderby src.BlastID
                             select new { Blast = src.BlastID, Count = src.Perc }).ToArray();
                var blasts = (from src in bclist
                              select new { src.BlastID })
                    .Distinct()
                    .ToArray();

                series.ShadowOffset = 4;
                series.ToolTip = SeriesToolTip;
                series.MarkerSize = 10;
                series.MarkerStyle = MarkerStyle.Circle;
                series.BorderWidth = 5;

                switch (selectedReportOnNames[i].ToString())
                {
                    case ColumnOpens:
                        series.Color = Color.DodgerBlue;
                        break;
                    case ColumnClicks:
                        series.Color = Color.Goldenrod;
                        break;
                    case ColumnBounces:
                        series.Color = Color.Red;
                        break;
                    case ColumnOptOuts1:
                        series.Color = Color.DarkBlue;
                        break;
                    case ColumnComplaints:
                        series.Color = Color.Gray;
                        break;
                }

                if (!query.Any())
                {
                    for (var index = 0; index < blasts.Length; index++)
                    {
                        series.Points.AddXY(blasts[index].BlastID, 0);
                        series.Label = SeriesLabel;
                    }
                }
                else
                {
                    for (var index = 0; index < query.Length; index++)
                    {
                        series.Points.AddXY(query[index].Blast, Math.Round(Convert.ToDouble(query[index].Count), 2));
                        series.Label = SeriesLabel;
                    }
                }
            }
        }

        public void DrawChart(object sender, System.EventArgs e)
        {
            CreateChart();
            if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastComparisonReport, KMPlatform.Enums.Access.ViewDetails))
            {
                ltbnExport.Visible = true;
            }
            else
            {
                ltbnExport.Visible = false;
            }
        }

        protected void gvChartData_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        private void loadCustomersDR()
        {
            List<ECN_Framework_Entities.Accounts.Customer> customerList =
            ECN_Framework_BusinessLayer.Accounts.Customer.GetCustomersByChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID);
            customerListBox.DataSource = customerList;
            customerListBox.DataBind();
            if (customerListBox.Items.Count > 0)
            {
                customerListBox.SelectedValue = Master.UserSession.CurrentUser.CustomerID.ToString();
            }
        }

        private void loadUsersDR()
        {
            List<KMPlatform.Entity.User> userList = KMPlatform.BusinessLogic.User.GetByCustomerID(Convert.ToInt32(customerListBox.SelectedValue));
            var result = (from src in userList
                          orderby src.UserName
                          select src).ToList();
            usersDR.DataSource = result;
            usersDR.DataBind();
        }

        private void loadCampaignsDR()
        {
            List<ECN_Framework_Entities.Communicator.Campaign> campaignList =
            ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentCustomer.CustomerID,  false);
            var result = (from src in campaignList
                          orderby src.CampaignName
                          select src).ToList();
            campaignsDR.DataSource = result;
            campaignsDR.DataBind();
        }

        private void loadGroupsDR()
        {
            List<ECN_Framework_Entities.Communicator.Group> grpList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID_NoAccessCheck(Convert.ToInt32(customerListBox.SelectedValue));
            var result = (from src in grpList
                          orderby src.GroupName
                          select src).ToList();
            groupsDR.DataSource = result;
            groupsDR.DataBind();
        }

        private void loadBlastListBox()
        {
            int? groupID = null;
            if (Convert.ToInt32(groupsDR.SelectedValue) > 0)
                groupID = Convert.ToInt32(groupsDR.SelectedValue);

            int? userID = null;
            if (Convert.ToInt32(usersDR.SelectedValue) > 0)
                userID = Convert.ToInt32(usersDR.SelectedValue);

            int? campaignID = null;
            if (Convert.ToInt32(campaignsDR.SelectedValue) > 0)
                campaignID = Convert.ToInt32(campaignsDR.SelectedValue);

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            if (DateTime.TryParse(fromDateTxt.Text.Trim(), out startDate))
            {
                if (DateTime.TryParse(toDateTxt.Text.Trim(), out endDate))
                {
                    DataTable dt = ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastComparison(Convert.ToInt32(customerListBox.SelectedItem.Value), startDate,
                                                                                           endDate, userID, groupID, campaignID);

                    foreach(System.Data.DataColumn dc in dt.Columns)
                    {
                        dc.ReadOnly = false;
                    }
                    foreach(DataRow dr in dt.Rows)
                    {
                        dr["EmailSubject"] = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(dr["EmailSubject"].ToString());
                    }

                    BlastListBox.DataSource = dt;
                    BlastListBox.DataBind();
                }
                else
                {
                    throwECNException("Invalid end date");
                    return;
                }

            }
            else
            {
                throwECNException("Invalid start date");
                return;
            }

            
        }

        protected void refreshBlastListBtn_Click(object sender, EventArgs e)
        {
            loadBlastListBox();
        }

        protected void customerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadBlastListBox();
            groupsDR.Items.Clear();
            groupsDR.Items.Insert(0, new ListItem("-- select all --", "0"));
            campaignsDR.Items.Clear();
            campaignsDR.Items.Insert(0, new ListItem("-- select all --", "0"));
            usersDR.Items.Clear();
            usersDR.Items.Insert(0, new ListItem("-- select all --", "0"));
        }

        public DataTable createGridDataTable()
        {
            var dataView = new DataView();
            try
            {
                var dataTable = CreateGridDataTableStructure();

                FillGridDataTable(dataTable);

                dataView = dataTable.DefaultView;
                dataView.Sort = SortSendTimeDesc;

                gvChartData.DataSource = dataView.ToTable();
                gvChartData.DataBind();

                PanelExport.Visible = dataTable.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                throwECNException(ex.ToString());
            }

            return dataView.ToTable();
        }

        private void FillGridDataTable(DataTable dataTable)
        {
            Guard.NotNull(dataTable, nameof(dataTable));

            var query = (from source in bclist
                select new
                {
                    source.BlastID,
                    source.TotalSent,
                    source.EmailSubject,
                    source.SendTime
                }).Distinct();

            foreach (var record in query)
            {
                var blastId = record.BlastID;

                var emailsubject = EmojiFunctions.GetSubjectUTF(record.EmailSubject);
                var sendtime = record.SendTime;
                var totalsent = string.Format(SentOnTemplate, record.TotalSent);
                string open;
                string click;
                string bounce;
                string subscribe;
                string complaint;

                string openperc;
                string clickperc;
                string bounceperc;
                string subscribeperc;
                string complaintperc;

                FillGridFields(blastId, ActionTypeCodeOpen, out openperc, out open);
                FillGridFields(blastId, ActionTypeCodeClick, out clickperc, out click);
                FillGridFields(blastId, ActionTypeCodeBounce, out bounceperc, out bounce);
                FillGridFields(blastId, ActionTypeCodeSubscribe, out subscribeperc, out subscribe);
                FillGridFields(blastId, ActionTypeCodeComplaint, out complaintperc, out complaint);

                dataTable.Rows.Add(
                    blastId,
                    openperc,
                    clickperc,
                    bounceperc,
                    subscribeperc,
                    complaintperc,
                    emailsubject,
                    sendtime,
                    totalsent,
                    open,
                    click,
                    bounce,
                    subscribe,
                    complaint);
            }
        }

        private void FillGridFields(string blastId, string actionTypeCode, out string strFormat, out string strValue)
        {
            var subQuery = (from source in bclist
                where source.BlastID == blastId && source.ActionTypeCode == actionTypeCode
                select new
                {
                    Count = source.DistinctCount,
                    source.Perc
                });

            var firstRecord = subQuery.FirstOrDefault();
            if (firstRecord != null)
            {
                strFormat = string.Concat(firstRecord.Perc, PercentString);
                strValue = firstRecord.Count.ToString();
            }
            else
            {
                strFormat = ZeroPercentString;
                strValue = ZeroString;
            }
        }

        private static DataTable CreateGridDataTableStructure()
        {
            var dataTable = CreateBaseDataTableStructure();

            var emailSubject = new DataColumn(ColumnEmailSubject, typeof(string));
            dataTable.Columns.Add(emailSubject);

            var sendTime = new DataColumn(ColumnSendTime, typeof(DateTime));
            dataTable.Columns.Add(sendTime);

            var totalSent = new DataColumn(ColumnTOtalSent, typeof(string));
            dataTable.Columns.Add(totalSent);

            var opens = new DataColumn(ColumnOpens, typeof(string));
            dataTable.Columns.Add(opens);

            var clicks = new DataColumn(ColumnClicks, typeof(string));
            dataTable.Columns.Add(clicks);

            var bounces = new DataColumn(ColumnBounces, typeof(string));
            dataTable.Columns.Add(bounces);

            var optOuts = new DataColumn(ColumnOptsOut, typeof(string));
            dataTable.Columns.Add(optOuts);

            var complaints = new DataColumn(ColumnComplaints, typeof(string));
            dataTable.Columns.Add(complaints);

            return dataTable;
        }

        private static DataTable CreateBaseDataTableStructure()
        {
            var dataTable = new DataTable();

            var blastId = new DataColumn(ColumnBlastId, typeof(string));
            dataTable.Columns.Add(blastId);

            var opensPerc = new DataColumn(ColumnOpensPerc, typeof(string));
            dataTable.Columns.Add(opensPerc);

            var clicksPerc = new DataColumn(ColumnClicksPerc, typeof(string));
            dataTable.Columns.Add(clicksPerc);

            var bouncesPerc = new DataColumn(ColumnBouncesPerc, typeof(string));
            dataTable.Columns.Add(bouncesPerc);

            var optOutsPerc = new DataColumn(ColumnOptOutsPerc, typeof(string));
            dataTable.Columns.Add(optOutsPerc);

            var complaintsPerc = new DataColumn(ColumnComplaintsPerc, typeof(string));
            dataTable.Columns.Add(complaintsPerc);

            return dataTable;
        }

        public DataTable createGraphDataTable()
        {
            var dataView = new DataView();
            try
            {
                var dataTable = CreateGraphDataTable();

                var query = (from src in bclist
                             select new
                             {
                                 src.BlastID,
                                 src.TotalSent,
                                 src.EmailSubject,
                                 src.SendTime
                             }).Distinct();

                foreach (var record in query)
                {
                    var blastid = record.BlastID;

                    var openperc = FillGraphFields(blastid, ActionTypeCodeOpen);
                    var clickperc = FillGraphFields(blastid, ActionTypeCodeClick);
                    var bounceperc = FillGraphFields(blastid, ActionTypeCodeBounce);
                    var subscribeperc = FillGraphFields(blastid, ActionTypeCodeSubscribe);
                    var complaintperc = FillGraphFields(blastid, ActionTypeCodeComplaint);

                    dataTable.Rows.Add(blastid, openperc, clickperc, bounceperc, subscribeperc, complaintperc);
                }

                dataView = dataTable.DefaultView;
                dataView.Sort = SortBlastIdDesc;
            }
            catch (Exception ex)
            {
                throwECNException(ex.Message);
            }

            return dataView.ToTable();
        }

        private double FillGraphFields(string blastId, string actionTypeCode)
        {
            var query = (from src in bclist
                where src.BlastID == blastId && 
                      src.ActionTypeCode == actionTypeCode
                select src.Perc);

            var perc = query.FirstOrDefault();
            return perc;
        }

        private static DataTable CreateGraphDataTable()
        {
            var dataTable = new DataTable();

            var blastId = new DataColumn(ColumnBlastId, typeof(int));
            dataTable.Columns.Add(blastId);

            var opensPerc = new DataColumn(ColumnOpensPerc, typeof(double));
            dataTable.Columns.Add(opensPerc);

            var clicksPerc = new DataColumn(ColumnClicksPerc, typeof(double));
            dataTable.Columns.Add(clicksPerc);

            var bouncesPerc = new DataColumn(ColumnBouncesPerc, typeof(double));
            dataTable.Columns.Add(bouncesPerc);

            var optOutsPerc = new DataColumn(ColumnOptOutsPerc, typeof(double));
            dataTable.Columns.Add(optOutsPerc);

            var complaintsPerc = new DataColumn(ColumnComplaintsPerc, typeof(double));
            dataTable.Columns.Add(complaintsPerc);
            return dataTable;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }


        protected void Export_Click(object sender, EventArgs e)
        {
            ExportReport(Response, dropdownExport.SelectedValue);
        }

        public string SaveChart(Chart chart)
        {
            CreateChart();
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            String tfile = Guid.NewGuid().ToString() + ".png";
            chart.SaveImage(txtoutFilePath + tfile, ChartImageFormat.Png);
            return tfile;
        }

        //public string SaveChart(Chart chart)
        //{
        //    CreateChart();
        //    string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
        //    String tfile = Guid.NewGuid().ToString() + ".png";
        //    chart.SaveImage(txtoutFilePath + tfile, ChartImageFormat.Png);
        //    return tfile;
        //}

        private void ExportReport(HttpResponse response, string type)
        {
            //string filename = SaveChart(chtBlastComparision);
            //string txtoutFilePath = System.Configuration.ConfigurationManager.AppSettings["Image_DomainPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/";

            ReportViewer1.LocalReport.ReportPath = "main\\blasts\\Report\\rpt_BlastComparision.rdlc";
            ReportParameter[] ReportParameters = new ReportParameter[5];
            ReportParameters[0] = new ReportParameter("ShowOpens", ReportOn.Items.FindByValue("open").Selected.ToString());
            ReportParameters[1] = new ReportParameter("ShowClicks", ReportOn.Items.FindByValue("click").Selected.ToString());
            ReportParameters[2] = new ReportParameter("ShowBounces", ReportOn.Items.FindByValue("bounce").Selected.ToString());
            ReportParameters[3] = new ReportParameter("ShowOptOuts", ReportOn.Items.FindByValue("subscribe").Selected.ToString());
            ReportParameters[4] = new ReportParameter("ShowComplaints", ReportOn.Items.FindByValue("complaint").Selected.ToString());

            ReportViewer1.LocalReport.SetParameters(ReportParameters);

            ReportDataSource gridDataSource = new ReportDataSource();
            ReportDataSource graphDataSource = new ReportDataSource();

            GetData();
            gridDataSource = new ReportDataSource("DataSet1", createGridDataTable());
            graphDataSource = new ReportDataSource("DataSet2", createGraphDataTable());
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(gridDataSource);
            ReportViewer1.LocalReport.DataSources.Add(graphDataSource);
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

            byte[] bytes = ReportViewer1.LocalReport.Render(type, null, out mimeType, out encoding, out extension, out streamids, out warnings); response.Clear();
            response.ContentType = mimeType;
            response.AppendHeader("Content-Disposition", String.Format("attachment; filename=BlastComparision.{0}", extension));
            response.OutputStream.Write(bytes.ToArray(), 0, (int)bytes.Length);
            response.End();
        }
    }
}
