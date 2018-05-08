using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using ECN_Framework_BusinessLayer.Communicator;
using Microsoft.Reporting.WebForms;
using ECN_Framework_Common.Objects;
using static ECN_Framework_BusinessLayer.Communicator.Group;

namespace ecn.communicator.main.blasts.Report
{
    public partial class PerformanceByDayAndTimeReport : ECN_Framework.WebPageHelper
    {
        private const string SortColumn = "Sort";
        private const string HourGroupColumn = "HourGroup";
        private const string OpensColumn = "Opens";
        private const string ClicksColumn = "Clicks";
        private const string PerformanceByDayAndTimeReportPath = "main\\blasts\\Report\\rpt_PerformanceByDayAndTime.rdlc";
        private const string LayoutID = "LayoutID";
        private const string NotSelectedValue = "-1";
        private const string StartDate = "StartDate";
        private const string EndDate = "EndDate";
        private const string RateType = "RateType";
        private const string ChartType = "ChartType";
        private const string Weekday = "Weekday";
        private const string DataSet1 = "DataSet1";
        private const string DataSet2 = "DataSet2";
        private const string ContentDispositionHeader = "Content-Disposition";
        private const string ContentDispositionHeaderFormat = "attachment; filename=PerformanceByDayAndTimeReport.{0}";
        private const string ChartArea1 = "ChartArea1";
        private const string ChartArea2 = "ChartArea2";
        private const string Time = "Time";
        private const string Rates = "Rates (%) ";
        private const string Legends1 = "Legends1";
        private const string Legends2 = "Legends2";
        private const string Times = "Times";
        private const string Title1 = "Title1";
        private const string OpenAndClickRatesTitle = "Open and Click Rates for {0}";
        private const string FirstDay = "mon";
        private const string ColumnChartType = "column";
        private const string OpenRatesYAxis = "Open Rates (%)";
        private const string OpenRatesTitle = "Open Rates By Time of Day";
        private const string ClickRatesYAxis = "Click Rates (%)";
        private const string ClickRatesTitle = "Click Rates By Time of Day";
        private const string OpensMetric = "opens";
        private const string FilterNameCampaign = "Campaign";
        private const string FilterNameMessage = "Message";
        private const string FilterNameMessageType = "Message Type";
        private const string FilterNameGroup = "Group";
        private const string FilterNameNone = "None";
        private const string FieldCampaignName = "CampaignName";
        private const string DataValueFieldCampaign = "CampaignID";
        private const string ErrorMessageMakeSureCampaingsAreSet = "Please make sure that you have sent campaigns for this customer.";
        private const string FieldName = "Name";
        private const string DataValueFieldMessageType = "MessageTypeID";
        private const string ErrorMessageMakeSureHaveSavedMessages = "Please make sure that you have saved message types for this basechannel.";
        private const string FieldNameGroupName = "GroupName";
        private const string DataValueFieldGroup = "GroupID";
        private const string ErrorMessageMakeSureYouHavedSavedGroups = "Please make sure that you have saved groups for this basechannel.";
        public readonly string[] HourGroups = 
        {
            "Midnight-6",
            "6-8 AM",
            "8-10 AM",
            "10-12 PM",
            "12-2 PM",
            "2-4 PM",
            "4-6 PM",
            "6-8 PM",
            "8-Midnight"
        };

        public readonly IDictionary<string, string> Days = new Dictionary<string, string>
        {
            {"mon", "Monday"},
            {"tue", "Tuesday"},
            {"wed", "Wednesday"},
            {"thur", "Thursday"},
            {"fri", "Friday"},
            {"sat", "Saturday"},
            {"sun", "Sunday"}
        };

        public readonly IList<Color> SeriesColors = new List<Color>
        {
            Color.Blue,
            Color.Brown,
            Color.Red,
            Color.Plum,
            Color.Yellow,
            Color.Green,
            Color.Black
        };
        
        public List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport> currentReport
        {
            get
            {
                List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport> _currentReport = new List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport>();
                _currentReport = (List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport>)ViewState["CurrentReport"];
                return (_currentReport == null) ? null : _currentReport;
            }
            set
            {
                ViewState["CurrentReport"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "performance by date and time";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PerformanceByDayAndTimeReport, KMPlatform.Enums.Access.View))
                {
                    layoutExplorer.enableSelectMode();

                    //Clear and hide the next three dropdowns
                    ddlFilterValue1.Items.Clear();
                    lblFilterValue1.Visible = false;
                    pnlDropDown.Visible = false;
                    pnlLayout.Visible = false;

                    ddlFilterField2.Items.Clear();
                    ddlFilterField2.Visible = false;
                    lblFilterField2.Visible = false;
                    ddlFilterValue2.Items.Clear();
                    ddlFilterValue2.Visible = false;
                    lblFilterValue2.Visible = false;
                    //Clear and hide error message
                    lblErrorMessage.Text = "";
                    phError.Visible = false;

                    txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            phError.Visible = false;
            clearChartArea();

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            if (validateDateRange(startDate, endDate))
            {
                int filter1Value = -1;
                string filter1 = ddlFilterField1.SelectedValue.ToString();
                if (pnlDropDown.Visible)
                    filter1Value = Convert.ToInt32(ddlFilterValue1.SelectedValue);
                else
                    filter1Value = Convert.ToInt32(hfSelectedLayoutA.Value.ToString());

                string filter2 = ddlFilterField2.SelectedValue.ToString();
                int filter2Value = 0;
                if (ddlFilterValue2.SelectedIndex != -1)
                {
                    filter2Value = Convert.ToInt32(ddlFilterValue2.SelectedValue);
                }

                currentReport = ECN_Framework_BusinessLayer.Activity.Report.PerformanceByDayAndTimeReport.Get(Master.UserSession.CurrentCustomer.CustomerID, startDate, endDate, filter1, filter1Value, filter2, filter2Value);
                if (currentReport != null)
                {
                    drawWeekChart(ddlOpensOrClicks.SelectedItem.ToString().ToLower());
                    drawDayChart(ddlDay.SelectedValue.ToLower());
                    ddlOpensOrClicks.Enabled = true;
                    ddlOpensOrClicks.Visible = true;
                    lblOpensOrClicks.Visible = true;
                    ddlLineOrColumn.Enabled = true;
                    ddlLineOrColumn.Visible = true;
                    lblLineOrColumn.Visible = true;
                    ddlDay.Visible = true;
                    ddlDay.Enabled = true;
                    lblDayArea.Visible = true;

                    drpExport.Visible = true;
                    lblExport.Visible = true;
                    btnDownloadReport.Visible = true;
                    btnRefreshCharts.Visible = true;
                }
            }
            else
            {
              
               throwECNException("Please enter a valid date range (i.e. positive and less than or equal to six months).");
               clearChartArea();
            }
        }
        private void drawWeekChart(string metricToShow)
        {
            chtReportByFullWeek.ChartAreas.Clear();
            chtReportByFullWeek.Legends.Clear();
            chtReportByFullWeek.Series.Clear();
            chtReportByFullWeek.Titles.Clear();

            var chart = chtReportByFullWeek.ChartAreas.Add(ChartArea1);
            chart.AxisX.Title = Time;
            chart.AxisX.MajorGrid.Enabled = true;
            chart.AxisY.MajorGrid.Enabled = true;
            chart.AxisX.Interval = 1;
            chart.AxisX.MajorGrid.LineColor = Color.LightGray;
            chart.AxisY.MajorGrid.LineColor = Color.LightGray;
            chart.BackColor = Color.Transparent;
            chart.ShadowColor = Color.Transparent;

            chtReportByFullWeek.Height = 450;
            chtReportByFullWeek.Width = 800;

            // Adding legend
            var legend = chtReportByFullWeek.Legends.Add(Legends1);
            legend.Enabled = true;
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.IsEquallySpacedItems = true;
            legend.TextWrapThreshold = 0;
            legend.IsTextAutoFit = true;
            legend.BackColor = Color.Transparent;
            legend.ShadowColor = Color.Transparent;

            SeriesChartType chartType;
            if (ddlLineOrColumn.SelectedItem.ToString().Equals(ColumnChartType, StringComparison.OrdinalIgnoreCase))
            {
                chartType = SeriesChartType.Column;
            }
            else
            {
                chartType = SeriesChartType.Line;
            }

            string column;
            if (metricToShow == OpensMetric)
            {
                column = OpensColumn;
                chtReportByFullWeek.ChartAreas[ChartArea1].AxisY.Title = OpenRatesYAxis;
                chtReportByFullWeek.Titles.Add(Title1).Text = OpenRatesTitle;
            }
            else
            {
                column = ClicksColumn;
                chtReportByFullWeek.ChartAreas[ChartArea1].AxisY.Title = ClickRatesYAxis;
                chtReportByFullWeek.Titles.Add(Title1).Text = ClickRatesTitle;
            }
            
            chtReportByFullWeek.DataSource = CreateReportDataSource(chartType, column);
            chtReportByFullWeek.DataBind();
        }

        private DataTable CreateReportDataSource(SeriesChartType chartType, string column)
        {
            var dataSource = new DataTable();
            dataSource.Columns.Add(new DataColumn(Times));

            // reportData is a dictionary of day and list of hourGroups
            var reportData = new Dictionary<string, List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport>>();
            var dayIndex = 0;
            foreach (var day in Days.Keys)
            {
                // Initialize data source columns
                dataSource.Columns.Add(new DataColumn(Days[day] + column));

                // Get data series per day
                var hourGroupsItems = currentReport.Where(num => num.DayGroup.ToLower() == day).ToList();
                reportData.Add(day, hourGroupsItems);

                // Initialize report series per day
                var series = chtReportByFullWeek.Series.Add(Days[day]);
                series.XValueMember = Times;
                series.YValueMembers = Days[day] + column;
                series.ChartType = chartType;
                series.IsVisibleInLegend = true;
                series.BorderWidth = 3;
                series.Color = SeriesColors[dayIndex];

                dayIndex++;
            }

            for (var hourGroupIndex = 0; hourGroupIndex < HourGroups.Length; hourGroupIndex++)
            {
                var row = dataSource.NewRow();
                row[Times] = reportData[FirstDay][hourGroupIndex].HourGroup;

                foreach (var day in Days.Keys)
                {
                    if (column == OpensColumn)
                    {
                        row[Days[day] + column] = reportData[day][hourGroupIndex].Opens;
                    }
                    else
                    {
                        row[Days[day] + column] = reportData[day][hourGroupIndex].Clicks;
                    }
                }

                dataSource.Rows.Add(row);
            }

            return dataSource;
        }
        
        //Note that dayToShow must be of the form mon, tue, wed, thur, fri, sat, sun to match the returned sql values for DayGroup 
        private void drawDayChart(string dayToShow)
        {
            chtReportByDay.ChartAreas.Clear();
            chtReportByDay.Legends.Clear();
            chtReportByDay.Series.Clear();
            chtReportByDay.Titles.Clear();
            
            var chartArea = chtReportByDay.ChartAreas.Add(ChartArea2);
            chartArea.AxisX.Title = Time;
            chartArea.AxisY.Title = Rates;
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.BackColor = Color.Transparent;
            chartArea.ShadowColor = Color.Transparent;

            chtReportByDay.AntiAliasing = AntiAliasingStyles.Graphics;
            chtReportByDay.Height = 250;
            chtReportByDay.Width = 545;

            //Adding legend
            var legend = chtReportByDay.Legends.Add(Legends2);
            legend.Enabled = true;
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.IsEquallySpacedItems = true;
            legend.TextWrapThreshold = 0;
            legend.IsTextAutoFit = true;
            legend.BackColor = Color.Transparent;
            legend.ShadowColor = Color.Transparent;
            
            var series = chtReportByDay.Series.Add(OpensColumn);
            series.XValueMember = Times;
            series.YValueMembers = OpensColumn;
            series.ChartType = SeriesChartType.Column;
            series.IsVisibleInLegend = true;
            series.BorderWidth = 3;
            series.Color = Color.Blue;

            series = chtReportByDay.Series.Add(ClicksColumn);
            series.XValueMember = Times;
            series.YValueMembers = ClicksColumn;
            series.ChartType = SeriesChartType.Column;
            series.IsVisibleInLegend = true;
            series.BorderWidth = 3;
            series.Color = Color.OrangeRed;

            var dataSource = CreateChartDataSource(dayToShow, Times);

            chtReportByDay.DataSource = dataSource;
            chtReportByDay.DataBind();
            gvClicksByDay.DataSource = dataSource;
            gvClicksByDay.DataBind();

            //Adding Title
            chtReportByDay.Titles.Add(Title1);
            chtReportByDay.Titles[Title1].Text = string.Format(OpenAndClickRatesTitle, Days[dayToShow]);
        }

        private void HideAndClear()
        {
            clearChartArea();
            //Clear and hide the next three dropdowns
            ddlFilterValue1.Items.Clear();
            hfSelectedLayoutA.Value = "-1";
            lblSelectedLayoutA.Text = "";
            ddlFilterField2.Items.Clear();
            ddlFilterField2.Visible = false;
            lblFilterField2.Visible = false;
            ddlFilterValue2.Items.Clear();
            ddlFilterValue2.Visible = false;
            lblFilterValue2.Visible = false;
            pnlLayout2.Visible = false;
            hfSelectedLayout2.Value = "";
            hfWhichLayout.Value = "";
            lblSelectedLayout2.Text = "";
            //lblSelectedLayout2.Text = "";
            //Clear and hide error message
            lblErrorMessage.Text = "";
            phError.Visible = false;

            btnReport.Enabled = false;
            ddlOpensOrClicks.Enabled = false;
            ddlLineOrColumn.Enabled = false;
            ddlDay.Enabled = false;

            ddlOpensOrClicks.Visible = false;
            lblOpensOrClicks.Visible = false;
            ddlLineOrColumn.Visible = false;
            lblLineOrColumn.Visible = false;
            ddlDay.Visible = false;
            lblDayArea.Visible = false;

            pnlDropDown.Visible = false;
            pnlLayout.Visible = false;

            
        }

        #region Error Handling
        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

     
        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }
        #endregion
        #region ddl Listeners
        protected void imgSelectLayoutA_Click(object sender, EventArgs e)
        {
            layoutExplorer.reset();
            ImageButton send = (ImageButton)sender;
            if (send.CommandArgument.Equals("1"))
                hfWhichLayout.Value = "1";
            else
                hfWhichLayout.Value = "2";
            mpeLayoutExplorer.Show();
        }
        protected void ddlFilterField1_SelectedIndexChanged(object sender, EventArgs e)
        {

            HideAndClear();
            //Populate and reveal the dropdowns according to selection
            if (ddlFilterField1.SelectedItem.ToString() == "Campaign")
            {
                int customerID = Master.UserSession.CurrentCustomer.CustomerID;
                List<ECN_Framework_Entities.Communicator.Campaign> campaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NoAccessCheck(customerID, false);
                if (campaigns.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.Campaign> sortedCampaigns = campaigns.OrderBy(x => x.CampaignName).ToList();
                    ddlFilterValue1.DataSource = sortedCampaigns;
                    ddlFilterValue1.DataTextField = "CampaignName";
                    ddlFilterValue1.DataValueField = "CampaignID";
                    ddlFilterValue1.DataBind();
                    ddlFilterValue1.Items.Insert(0, new ListItem(""));
                    ddlFilterValue1.SelectedIndex = 0;
                    ddlFilterValue1.Enabled = true;
                    lblFilterValue1.Visible = true;
                    pnlDropDown.Visible = true;
                }
                else
                {
                    ddlFilterField1.SelectedIndex = 0;
                    lblErrorMessage.Text = "Please make sure that you have sent campaigns for this customer.";
                    phError.Visible = true;
                }
            }
            else if (ddlFilterField1.SelectedItem.ToString() == "Message")
            {
                pnlLayout.Visible = true;
                lblFilterValue1.Visible = true;

            }

            else if (ddlFilterField1.SelectedItem.ToString() == "Message Type")
            {
                List<ECN_Framework_Entities.Communicator.MessageType> messageTypes = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser);
                if (messageTypes.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.MessageType> sortedMessageTypes = messageTypes.OrderBy(x => x.Name).ToList();
                    ddlFilterValue1.DataSource = sortedMessageTypes;
                    ddlFilterValue1.DataTextField = "Name";
                    ddlFilterValue1.DataValueField = "MessageTypeID";
                    ddlFilterValue1.DataBind();
                    ddlFilterValue1.Items.Insert(0, new ListItem(""));
                    ddlFilterValue1.SelectedIndex = 0;
                    ddlFilterValue1.Enabled = true;
                    lblFilterValue1.Visible = true;
                    pnlDropDown.Visible = true;
                }
                else
                {
                    ddlFilterField1.SelectedIndex = 0;
                    lblErrorMessage.Text = "Please make sure that you have saved message types for this customer.";
                    phError.Visible = true;
                }
            }
            else if (ddlFilterField1.SelectedItem.ToString() == "Group")
            {
                int customerID = Master.UserSession.CurrentCustomer.CustomerID;
                List<ECN_Framework_Entities.Communicator.Group> groups = GetByCustomerID_NoAccessCheck(customerID);
                if (groups.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.Group> sortedGroups = groups.OrderBy(x => x.GroupName).ToList();
                    ddlFilterValue1.DataSource = sortedGroups;
                    ddlFilterValue1.DataTextField = "GroupName";
                    ddlFilterValue1.DataValueField = "GroupID";
                    ddlFilterValue1.DataBind();
                    ddlFilterValue1.Items.Insert(0, new ListItem(""));
                    ddlFilterValue1.SelectedIndex = 0;
                    ddlFilterValue1.Enabled = true;
                    lblFilterValue1.Visible = true;
                    pnlDropDown.Visible = true;
                }
                else
                {
                    ddlFilterField1.SelectedIndex = 0;
                    lblErrorMessage.Text = "Please make sure that you have saved groups for this basechannel.";
                    phError.Visible = true;
                }
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                if (sender is ECN_Framework_Entities.Communicator.Layout)
                {
                    ECN_Framework_Entities.Communicator.Layout layout = (ECN_Framework_Entities.Communicator.Layout)sender;

                    if (hfWhichLayout.Value.Equals("1"))
                    {
                        lblSelectedLayoutA.Text = layout.LayoutName;
                        hfSelectedLayoutA.Value = layout.LayoutID.ToString();
                    }
                    else if (hfWhichLayout.Value.Equals("2"))
                    {
                        lblSelectedLayout2.Text = layout.LayoutName;
                        hfSelectedLayout2.Value = layout.LayoutID.ToString();
                        btnReport.Enabled = true;
                    }
                    clearChartArea();

                    ddlOpensOrClicks.Enabled = false;
                    ddlDay.Enabled = false;
                    ddlLineOrColumn.Enabled = false;

                    ddlOpensOrClicks.Visible = false;
                    lblDayArea.Visible = false;
                    ddlDay.Visible = false;
                    lblDayArea.Visible = false;
                    ddlLineOrColumn.Visible = false;
                    lblLineOrColumn.Visible = false;

                    if (ddlFilterField2.Items.Count == 0)
                    {
                        if (ddlFilterField1.SelectedItem.ToString().ToUpper() == "CAMPAIGN" || ddlFilterField1.SelectedItem.ToString().ToUpper() == "MESSAGE" || ddlFilterField1.SelectedItem.ToString().ToUpper() == "MESSAGE TYPE")
                        {
                            ddlFilterField2.Enabled = true;
                            ddlFilterField2.Visible = true;
                            lblFilterField2.Visible = true;
                            ListItem selectListItem = new ListItem("-SELECT-", "-1");
                            ddlFilterField2.Items.Insert(0, selectListItem);

                            ListItem groupListItem = new ListItem("Group", "GroupID");
                            ddlFilterField2.Items.Insert(1, groupListItem);

                            ListItem noneListItem = new ListItem("None", "None");
                            ddlFilterField2.Items.Insert(2, noneListItem);
                        }
                        else
                        {
                            ddlFilterField2.Enabled = true;
                            ddlFilterField2.Visible = true;
                            lblFilterField2.Visible = true;
                            ListItem selectListItem = new ListItem("-SELECT-", "-1");
                            ddlFilterField2.Items.Insert(0, selectListItem);

                            ListItem campaignListItem = new ListItem("Campaign", "CampaignID");
                            ddlFilterField2.Items.Insert(1, campaignListItem);

                            ListItem messageListItem = new ListItem("Message", "LayoutID");
                            ddlFilterField2.Items.Insert(2, messageListItem);

                            ListItem messageTypeListItem = new ListItem("Message Type", "MessageTypeID");
                            ddlFilterField2.Items.Insert(3, messageTypeListItem);

                            ListItem noneListItem = new ListItem("None", "None");
                            ddlFilterField2.Items.Insert(4, noneListItem);
                        }
                    }
                    mpeLayoutExplorer.Hide();
                    upMain.Update();
                }

                return true;
            }
            catch
            {
                mpeLayoutExplorer.Hide();
                return false;
            }
        }
        protected void ddlFilterValue1_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearChartArea();

            ddlOpensOrClicks.Enabled = false;
            ddlDay.Enabled = false;
            ddlLineOrColumn.Enabled = false;

            ddlOpensOrClicks.Visible = false;
            lblDayArea.Visible = false;
            ddlDay.Visible = false;
            lblDayArea.Visible = false;
            ddlLineOrColumn.Visible = false;
            lblLineOrColumn.Visible = false;

            if (ddlFilterField2.Items.Count == 0)
            {
                if (ddlFilterField1.SelectedItem.ToString().ToUpper() == "CAMPAIGN" || ddlFilterField1.SelectedItem.ToString().ToUpper() == "MESSAGE" || ddlFilterField1.SelectedItem.ToString().ToUpper() == "MESSAGE TYPE")
                {
                    ddlFilterField2.Enabled = true;
                    ddlFilterField2.Visible = true;
                    lblFilterField2.Visible = true;
                    ListItem selectListItem = new ListItem("-SELECT-", "-1");
                    ddlFilterField2.Items.Insert(0, selectListItem);

                    ListItem groupListItem = new ListItem("Group", "GroupID");
                    ddlFilterField2.Items.Insert(1, groupListItem);

                    ListItem noneListItem = new ListItem("None", "None");
                    ddlFilterField2.Items.Insert(2, noneListItem);
                }
                else
                {
                    ddlFilterField2.Enabled = true;
                    ddlFilterField2.Visible = true;
                    lblFilterField2.Visible = true;
                    ListItem selectListItem = new ListItem("-SELECT-", "-1");
                    ddlFilterField2.Items.Insert(0, selectListItem);

                    ListItem campaignListItem = new ListItem("Campaign", "CampaignID");
                    ddlFilterField2.Items.Insert(1, campaignListItem);

                    ListItem messageListItem = new ListItem("Message", "LayoutID");
                    ddlFilterField2.Items.Insert(2, messageListItem);

                    ListItem messageTypeListItem = new ListItem("Message Type", "MessageTypeID");
                    ddlFilterField2.Items.Insert(3, messageTypeListItem);

                    ListItem noneListItem = new ListItem("None", "None");
                    ddlFilterField2.Items.Insert(4, noneListItem);
                }
            }
        }

        protected void ddlFilterField2_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearChartArea();
            ddlFilterValue2.Items.Clear();
            ddlFilterValue2.Visible = false;

            lblErrorMessage.Text = string.Empty;
            phError.Visible = false;

            btnReport.Enabled = false;
            ddlOpensOrClicks.Enabled = false;
            ddlLineOrColumn.Enabled = false;
            ddlDay.Enabled = false;

            ddlOpensOrClicks.Visible = false;
            lblOpensOrClicks.Visible = false;
            ddlLineOrColumn.Visible = false;
            lblLineOrColumn.Visible = false;
            ddlDay.Visible = false;
            lblDayArea.Visible = false;

            ddlFilterValue2.Visible = false;
            lblFilterValue2.Visible = false;
            pnlLayout2.Visible = false;
            hfSelectedLayout2.Value = string.Empty;
            hfWhichLayout.Value = string.Empty;
            lblSelectedLayout2.Text = string.Empty;

            switch (ddlFilterField2.SelectedItem.ToString())
            {
                case FilterNameCampaign:
                    SetFilterValueCampaign();
                    break;
                case FilterNameMessage:
                    pnlLayout2.Visible = true;
                    lblFilterValue2.Visible = true;
                    break;
                case FilterNameMessageType:
                    SetFilterValueMessageType();
                    break;
                case FilterNameGroup:
                    SetFilterValueGroup();
                    break;
                case FilterNameNone:
                    ddlFilterValue2.Enabled = false;
                    ddlFilterValue2.Visible = false;
                    btnReport.Enabled = true;
                    break;
            }
        }

        private void SetFilterValueGroup()
        {
            var customerId = Master.UserSession.CurrentCustomer.CustomerID;
            var groups = GetByCustomerID_NoAccessCheck(customerId);
            if (groups.Count > 0)
            {
                var sortedGroups = groups
                    .OrderBy(x => x.GroupName)
                    .ToList();

                ddlFilterValue2.DataSource = sortedGroups;
                ddlFilterValue2.DataTextField = FieldNameGroupName;
                ddlFilterValue2.DataValueField = DataValueFieldGroup;
                ddlFilterValue2.DataBind();
                ddlFilterValue2.Items.Insert(0, new ListItem(string.Empty));
                ddlFilterValue2.SelectedIndex = 0;
                ddlFilterValue2.Enabled = true;
                ddlFilterValue2.Visible = true;
                lblFilterValue2.Visible = true;
            }
            else
            {
                ddlFilterField2.SelectedIndex = 0;
                lblErrorMessage.Text = ErrorMessageMakeSureYouHavedSavedGroups;
                phError.Visible = true;
            }
        }

        private void SetFilterValueMessageType()
        {
            var messageTypes = MessageType.GetByBaseChannelID(
                Master.UserSession.CurrentBaseChannel.BaseChannelID,
                Master.UserSession.CurrentUser);

            if (messageTypes.Count > 0)
            {
                var sortedMessageTypes = messageTypes
                    .OrderBy(x => x.Name)
                    .ToList();

                ddlFilterValue2.DataSource = sortedMessageTypes;
                ddlFilterValue2.DataTextField = FieldName;
                ddlFilterValue2.DataValueField = DataValueFieldMessageType;
                ddlFilterValue2.DataBind();
                ddlFilterValue2.Items.Insert(0, new ListItem(string.Empty));
                ddlFilterValue2.SelectedIndex = 0;
                ddlFilterValue2.Enabled = true;
                ddlFilterValue2.Visible = true;
                lblFilterValue2.Visible = true;
            }
            else
            {
                ddlFilterField2.SelectedIndex = 0;
                lblErrorMessage.Text = ErrorMessageMakeSureHaveSavedMessages;
                phError.Visible = true;
            }
        }

        private void SetFilterValueCampaign()
        {
            var customerId = Master.UserSession.CurrentCustomer.CustomerID;
            var campaigns = Campaign.GetByCustomerID_NoAccessCheck(customerId, false);
            if (campaigns.Count > 0)
            {
                var sortedCampaigns = campaigns
                    .OrderBy(campaign => campaign.CampaignName)
                    .ToList();

                ddlFilterValue2.DataSource = sortedCampaigns;
                ddlFilterValue2.DataTextField = FieldCampaignName;
                ddlFilterValue2.DataValueField = DataValueFieldCampaign;
                ddlFilterValue2.DataBind();
                ddlFilterValue2.Items.Insert(0, new ListItem(string.Empty));
                ddlFilterValue2.SelectedIndex = 0;
                ddlFilterValue2.Enabled = true;
                ddlFilterValue2.Visible = true;
                lblFilterValue2.Visible = true;
            }
            else
            {
                ddlFilterField2.SelectedIndex = 0;
                lblErrorMessage.Text = ErrorMessageMakeSureCampaingsAreSet;
                phError.Visible = true;
            }
        }

        protected void ddlFilterValue2_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearChartArea();

            ddlOpensOrClicks.Enabled = false;
            ddlLineOrColumn.Enabled = false;
            ddlDay.Enabled = false;

            ddlOpensOrClicks.Visible = false;
            lblOpensOrClicks.Visible = false;
            ddlLineOrColumn.Visible = false;
            lblLineOrColumn.Visible = false;
            ddlDay.Visible = false;
            lblDayArea.Visible = false;


            if (ddlFilterValue2.SelectedIndex != 0)
            {
                btnReport.Enabled = true;
            }
        }

        protected void ddlOpensOrClicks_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawWeekChart(ddlOpensOrClicks.SelectedItem.ToString().ToLower());
            drawDayChart(ddlDay.SelectedValue.ToString().ToLower());
        }

        protected void ddlLineOrColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawDayChart(ddlDay.SelectedValue.ToString().ToLower());
            drawWeekChart(ddlOpensOrClicks.SelectedItem.ToString().ToLower());
        }
        #endregion
        protected void refreshCharts(object sender, EventArgs e)
        {
            drawWeekChart(ddlOpensOrClicks.SelectedItem.ToString().ToLower());
            drawDayChart(ddlDay.SelectedValue.ToString().ToLower());
        }

        private void clearChartArea()
        {
            //Clear chart areas
            chtReportByFullWeek.ChartAreas.Clear();
            chtReportByFullWeek.Legends.Clear();
            chtReportByFullWeek.Series.Clear();
            chtReportByFullWeek.Titles.Clear();

            ddlLineOrColumn.Visible = false;
            lblLineOrColumn.Visible = false;
            ddlOpensOrClicks.Visible = false;

            ddlDay.Visible = false;

            chtReportByDay.ChartAreas.Clear();
            chtReportByDay.Legends.Clear();
            chtReportByDay.Series.Clear();
            chtReportByDay.Titles.Clear();

            gvClicksByDay.DataSource = null;
            gvClicksByDay.DataBind();

            lblExport.Visible = false;
            drpExport.Visible = false;

            lblOpensOrClicks.Visible = false;
            lblDayArea.Visible = false;

            btnDownloadReport.Visible = false;
            btnRefreshCharts.Visible = false;

        }

        public string performanceByDayAndTime_SaveChart(Chart chart)
        {
            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            String tfile = Guid.NewGuid().ToString() + ".png";
            chart.AntiAliasing = AntiAliasingStyles.Graphics;
            if (!Directory.Exists(txtoutFilePath))
            {
                Directory.CreateDirectory(txtoutFilePath);
            }
            chart.SaveImage(txtoutFilePath + tfile, ChartImageFormat.Png);
            return tfile;
        }
        private void performanceByDayAndTime_ExportReport(HttpResponse response, string type)
        {
            drawDayChart(ddlDay.SelectedValue.ToLower());
            drawWeekChart(ddlOpensOrClicks.SelectedItem.ToString().ToLower());
            
            var reportParameters = new ReportParameter[]
            {
                new ReportParameter(StartDate, txtstartDate.Text),
                new ReportParameter(EndDate, txtendDate.Text),
                new ReportParameter(RateType, ddlOpensOrClicks.SelectedItem.ToString()),
                new ReportParameter(ChartType, ddlLineOrColumn.SelectedItem.ToString().ToLower()),
                new ReportParameter(Weekday, ddlDay.SelectedItem.ToString())
            };

            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.ReportPath = PerformanceByDayAndTimeReportPath;
            ReportViewer1.LocalReport.SetParameters(reportParameters);

            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            var filter1 = ddlFilterField1.SelectedValue;
            var filter1Value = -1;
            if (filter1.Equals(LayoutID))
            {
                int.TryParse(hfSelectedLayoutA.Value, out filter1Value);
            }
            else
            {
                int.TryParse(ddlFilterValue1.SelectedValue, out filter1Value);
            }

            var filter2 = ddlFilterField2.SelectedValue;
            var filter2Value = 0;
            if (!string.IsNullOrWhiteSpace(filter2) && !filter2.Equals(NotSelectedValue))
            {
                if (filter2.Equals(LayoutID))
                {
                    int.TryParse(hfSelectedLayout2.Value, out filter2Value);
                }
                else
                {
                    int.TryParse(ddlFilterValue2.SelectedValue, out filter2Value);
                }
            }

            var dataSource = CreateChartDataSource(ddlDay.SelectedValue, HourGroupColumn);
            dataSource.DefaultView.Sort = SortColumn;
            var rds = new ReportDataSource(DataSet1, dataSource.DefaultView);

            var dtWeek = ECN_Framework_BusinessLayer.Activity.Report.PerformanceByDayAndTimeReport.GetWeekDataTable(ddlOpensOrClicks.SelectedItem.ToString().ToLower(), startDate, endDate, filter1, filter1Value, filter2, filter2Value, Master.UserSession.CurrentCustomer.CustomerID);
            dtWeek.DefaultView.Sort = SortColumn;
            var rds2 = new ReportDataSource(DataSet2, dtWeek.DefaultView);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.DataSources.Add(rds2);
            ReportViewer1.DataBind();
            ReportViewer1.LocalReport.Refresh();

            WriteResponseFromReportViewer(response, type);
        }

        private DataTable CreateChartDataSource(string day, string hourGroupColumn)
        {
            var dataSource = new DataTable();
            dataSource.Columns.Add(new DataColumn(SortColumn, typeof(int)));
            dataSource.Columns.Add(new DataColumn(hourGroupColumn));
            dataSource.Columns.Add(new DataColumn(OpensColumn));
            dataSource.Columns.Add(new DataColumn(ClicksColumn));

            for (var index = 0; index < HourGroups.Length; index++)
            {
                var time = currentReport.First(num => num.HourGroup == HourGroups[index] && num.DayGroup.Equals(day, StringComparison.OrdinalIgnoreCase));
                dataSource.Rows.Add(index, time.HourGroup, time.Opens, time.Clicks);
            }

            return dataSource;
        }

        private void WriteResponseFromReportViewer(HttpResponse response, string type)
        {
            // Out params to pass into the Render method
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            // Call the Render method and save the return value in our byte array 
            var bytes = ReportViewer1.LocalReport.Render(type, null, out mimeType, out encoding, out extension, out streamids, out warnings);

            // Clear the Response object, set the ContentType, attach a header so the browser thinks it's downloading a file, write the byte array to the Response object and tell the bowser we're done by calling the End method
            response.Clear();
            response.ContentType = mimeType;
            response.AppendHeader(ContentDispositionHeader, string.Format(ContentDispositionHeaderFormat, extension));
            response.OutputStream.Write(bytes.ToArray(), 0, bytes.Length);
            response.End();
        }
        
        protected void btnDownloadReport_Click(object sender, EventArgs e)
        {
            performanceByDayAndTime_ExportReport(Response, drpExport.SelectedValue.ToUpper());
        }
        private static bool validateDateRange(DateTime startDate, DateTime endDate)
        {
            DateTime minDate = new DateTime(2000, 1, 1);
            DateTime sixMonthBeforEndDate = endDate.AddMonths(-6);

            if (startDate <= endDate && startDate > minDate && endDate > minDate && sixMonthBeforEndDate <= startDate)
            {
                return true;
            }
            else
                return false;

        }

        protected void btnCloseLayoutExplorer_Click(object sender, EventArgs e)
        {
            mpeLayoutExplorer.Hide();
        }
    }
}