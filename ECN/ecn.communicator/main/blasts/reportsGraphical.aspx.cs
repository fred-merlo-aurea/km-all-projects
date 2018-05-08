using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Configuration;
using dotnetCHARTING;

namespace ecn.communicator.blastsmanager {
	
	public partial class reportsGraphical : ECN_Framework.WebPageHelper
    {
        private const string SqlGroupName =
            " SELECT " +
            " 'GroupName' = CASE " +
            " WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN g.GroupName " +
            " ELSE '< GROUP DELETED >' " +
            " END, " +
            " 'GrpNavigateURL' = CASE " +
            " WHEN LEN(ltrim(rtrim(g.GroupName))) <> 0 THEN '../lists/groupeditor.aspx?GroupID='+CONVERT(VARCHAR,g.GroupID) " +
            " ELSE '' " +
            " END, " +
            " 'FilterName' = CASE " +
            " WHEN f.FilterID <> 0 THEN f.FilterName " +
            " ELSE '< NO FILTER / FILTER DELETED >' " +
            " END, " +
            " 'FltrNavigateURL' = CASE " +
            " WHEN f.FilterID <> 0 THEN '../lists/filters.aspx?FilterID='+CONVERT(VARCHAR,f.FilterID) " +
            " ELSE '' " +
            " END, " +
            " 'LayoutName' = CASE " +
            " WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN l.LayoutName " +
            " ELSE '< CAMPAIGN DELETED >' " +
            " END, " +
            " 'LytNavigateURL' = CASE " +
            " WHEN LEN(ltrim(rtrim(l.LayoutName))) <> 0 THEN '../content/layouteditor.aspx?LayoutID='+CONVERT(VARCHAR,l.LayoutID)  " +
            " ELSE '' " +
            " END, " +
            " b.EmailSubject, b.EmailFromName, b.EmailFrom, b.SendTime, b.FinishTime, b.SuccessTotal, b.SendTotal, " +
            " l.SetupCost, l.OutboundCost, l.InboundCost, l.DesignCost, l.OtherCost " +
            " FROM Blasts b LEFT OUTER JOIN Groups g ON b.groupID = g.groupID LEFT OUTER JOIN Filters f ON b.filterID = f.filterID " +
            " JOIN Layouts l on b.LayoutID = l.LayoutID " +
            " WHERE b.BlastID = {0}";
        private const string ProcedureGetGraphicalBlastBounceReportData = "spGetGraphicalBlastBounceReportData";
        private const string ProcedureGetGraphicalBlastReportData = "spGetGraphicalBlastReportData";
        private const string ParameterBlastId = "@blastID";
        private const string RowFilterHardBounce = " BounceCount > 4 AND (BounceType = 'hard' OR BounceType = 'hardbounce')";
        private const string RowFilterSoftBounce = " (BounceType = 'soft' OR BounceType = 'softbounce')";
        private const string DefaultDateFormat = "MM/dd/yyyy";
        private const string MonthDayDateFormat = "MM/dd";
        private const string ConfigChartsTempDirectory = "ChartsTempDirectory";
        private const string ResponseDetailChartTitle = "Responses by day";
        private const string ConfigActivity = "activity";        
        public static readonly string[] ReportTypes = { "open", "click" };

        //Page Not Used

        public static string connStr			= ConfigurationManager.AppSettings["connstring"];
		public static string communicatordb	= ConfigurationManager.AppSettings["communicatordb"];

		#region VARIABLE DECLARATIONS



		//protected System.Web.UI.WebControls.LinkButton lnkConversionTracking;
		



		#endregion

    	ArrayList columnHeadings = new ArrayList();
		IEnumerator aListEnum = null;

		public reportsGraphical() {
			Page.Init += new System.EventHandler(Page_Init);
		}
	
		protected void Page_Load(object sender, System.EventArgs e) {

           Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Blast Reporting";
            Master.HelpContent = "<img align='right' src='/ecn.images/images/icoblasts.gif'><b>Reports</b><br />Gives a report of the Blast in progress.<br />Click on <i>view log</i> to view the log of the emails that has received the blast.<br /><i>Clicks</i> specify the total number of URL clicks in your email by the recepients who received the email. Click on the '[number]' to see who clicked &amp; what link was clicked<br /><i>Bounces</i> specify the number of bounced emails recepients or the email recepients who did not received the blast. Click on the '[number]' to see who did not receive the blast.";
            Master.HelpTitle = "Blast Manager";

            string currentChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();
			int requestBlastID = getBlastID();
			string action		= getAction();

			if(currentChannelID == "28"){ 
				//Special Reporting for INFOUSA CUstomers 
				string mastCustID = "0";
				string selectMasterCustID = "SELECT MasterCustomerID FROM ecn5_accounts..BaseChannels WHERE BaseChannelID =  "+currentChannelID;
				try{
					mastCustID = DataFunctions.ExecuteScalar(selectMasterCustID).ToString();
				}catch{}

                if (!(mastCustID == Master.UserSession.CurrentUser.CustomerID.ToString()))
                {
					Response.Redirect("CH_"+currentChannelID+"_CustomReports.aspx?BlastID="+requestBlastID,true);
				}
			}

            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))
            {
				//if (Page.IsPostBack==false) {
					//lnkConversionTracking.Visible = lnkConversionTracking.Enabled = es.HasProductFeature("ecn.communicator","Conversion Tracking");
					if (requestBlastID>0) {
                        //if (Master.UserSession.CurrentKM.Platform.User.IsChannelAdministrator(user) ||KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                        if(KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                        {
							//do nothing & allow access
						}else{
							ECN_Framework.Common.SecurityAccess.canI("Blasts",requestBlastID.ToString());
						}
						LoadFormData(requestBlastID);
						LoadClicksData(requestBlastID);
					}
				//}
			}else{
				Response.Redirect("../default.aspx");			
			}

			if(!(Page.IsPostBack)) {
				if(action.Equals("report")) {
					DownloadClickReport(requestBlastID);					
				}
			}
		}

		#region All Getters
		public int getBlastID() {
			int theBlastID = 0;
			try {
				theBlastID = Convert.ToInt32(Request.QueryString["BlastID"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theBlastID;
		}

		private string getAction() {
			string theAction = "";
			try {
				theAction = Request.QueryString["action"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theAction;
		}

		private string getActionURL() {
			string theActionURL = "";
			try {
				theActionURL = Request.QueryString["actionURL"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theActionURL;
		}

		private string getISP() {
			string  sISP = "";
			try {
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ISPReporting))
					sISP = Request.QueryString["isp"].ToString();
			}
			catch {

			}
			return sISP;
		}
		#endregion

		

		public DataTable SelectDistinct(DataTable SourceTable) {	
			DataTable dt = new DataTable();
			dt = SourceTable.Clone();
			string lastEmailID = "0", currentEmailID = "";
			DataRow[] dr = SourceTable.Select("", "EmailID ASC");
			foreach (DataRow dr1 in dr) {
				currentEmailID = dr1["EmailID"].ToString();
				if ( !(currentEmailID == lastEmailID) ) {
					try{
						dt.ImportRow(dr1);
					}catch(Exception ex){
						string a = ex.ToString();
					}
				}
				lastEmailID = currentEmailID; 
			}
			return dt;
		}

        #region Data Load
        private void LoadFormData(int setBlastID)
        {
            var setupCost = 0m;
            var outboundCost = 0m;
            var inboundCost = 0m;
            var designCost = 0m;
            var otherCost = 0m;
            var blastSendTime = string.Empty;
            var sqlGroupName = string.Format(SqlGroupName, setBlastID);
            var groupNameTable = DataFunctions.GetDataTable(sqlGroupName);

            foreach (DataRow dataRow in groupNameTable.Rows)
            {
                GroupTo.Text = dataRow["GroupName"].ToString();
                Filter.Text = dataRow["FilterName"].ToString();
                Campaign.Text = dataRow["LayoutName"].ToString();
                EmailSubject.Text = dataRow["EmailSubject"].ToString();
                EmailFrom.Text = $"{dataRow["EmailFromName"]}<br>&lt;{dataRow["EmailFrom"]}&gt;";
                FinishTime.Text = dataRow["FinishTime"].ToString();
                blastSendTime = dataRow["SendTime"].ToString();
                SendTime.Text = blastSendTime;

                try { setupCost = Convert.ToDecimal(dataRow["SetupCost"].ToString()); } catch { setupCost = 0; }
                try { outboundCost = Convert.ToDecimal(dataRow["OutboundCost"].ToString()); } catch { outboundCost = 0; }
                try { inboundCost = Convert.ToDecimal(dataRow["InboundCost"].ToString()); } catch { inboundCost = 0; }
                try { designCost = Convert.ToDecimal(dataRow["DesignCost"].ToString()); } catch { designCost = 0; }
                try { otherCost = Convert.ToDecimal(dataRow["OtherCost"].ToString()); } catch { otherCost = 0; }
            }

            var connectionString = ConfigurationManager.AppSettings[ConfigActivity];
            var reportingTable = GetReportTable(setBlastID, connectionString);

            var sendTotal = reportingTable.Select("ActionTypeCode = 'send' OR ActionTypeCode = 'testsend'").Length;
            var clicksUniqueCount = reportingTable.Select("ActionTypeCode = 'click'").Length;
            var opensUniqueCount = reportingTable.Select("ActionTypeCode = 'open'").Length;
            var bouncesUniqueCount = reportingTable.Select(" ActionTypeCode = 'bounce' AND NOT (ActionValue='resend' OR ActionValue='U')").Length;
            var hardBouncesUniqueCount = reportingTable.Select("ActionTypeCode = 'bounce' AND (ActionValue = 'hard' OR ActionValue = 'hardbounce')").Length;
            var softBouncesUniqueCount = reportingTable.Select("ActionTypeCode = 'bounce' AND (ActionValue = 'soft' OR ActionValue = 'softbounce')").Length;
            var unknownBouncesUniqueCount = reportingTable.Select("ActionTypeCode = 'bounce' AND NOT (ActionValue = 'soft' OR ActionValue = 'softbounce' OR ActionValue = 'hard' OR ActionValue = 'hardbounce') ").Length;
            var subscribesUniqueCount = reportingTable.Select("ActionTypeCode = 'subscribe'").Length;
            var resendsUniqueCount = reportingTable.Select("ActionTypeCode = 'resend'").Length;
            var forwardsUniqueCount = reportingTable.Select("ActionTypeCode = 'refer'").Length;
            var conversionsUniqueCount = reportingTable.Select("ActionTypeCode = 'conversion'").Length;
            var success = sendTotal - bouncesUniqueCount;

            DisplayCountsAndPercentages(sendTotal, clicksUniqueCount, opensUniqueCount, bouncesUniqueCount, success, hardBouncesUniqueCount, softBouncesUniqueCount, unknownBouncesUniqueCount, subscribesUniqueCount, resendsUniqueCount, forwardsUniqueCount);
            DisplaySetupCost(setupCost, outboundCost, inboundCost, designCost, otherCost, sendTotal, bouncesUniqueCount);
            DisplayMessageFees(sendTotal, clicksUniqueCount, opensUniqueCount);
            DisplayROILabels(sendTotal, opensUniqueCount, conversionsUniqueCount);
            DisplayROIChart(success, clicksUniqueCount, opensUniqueCount, bouncesUniqueCount);
            DisplayReponseDetailChart(blastSendTime, reportingTable);
            SetErrorCodeCounts(setBlastID, connectionString);
        }

        private static DataTable GetReportTable(int setBlastID, string connectionString)
        {
            DataTable reportingTable = null;
            try
            {
                var dbConn = new SqlConnection(connectionString);
                var blastRptCmd = new SqlCommand(ProcedureGetGraphicalBlastReportData, dbConn);
                blastRptCmd.CommandTimeout = 0;
                blastRptCmd.CommandType = CommandType.StoredProcedure;
                blastRptCmd.Parameters.Add(new SqlParameter(ParameterBlastId, SqlDbType.Int));
                blastRptCmd.Parameters[ParameterBlastId].Value = setBlastID;

                var blastRptDA = new SqlDataAdapter(blastRptCmd);
                var blastRptDS = new DataSet();
                try
                {
                    dbConn.Open();
                    blastRptDA.Fill(blastRptDS, ProcedureGetGraphicalBlastReportData);
                }
                finally
                {
                    dbConn.Close();
                }
                
                reportingTable = blastRptDS.Tables[0];
            }
            catch
            {
                // TODO: handle empty catch
            }

            return reportingTable;
        }

        private void DisplayCountsAndPercentages(decimal sendTotal, decimal clicksUniqueCount, decimal opensUniqueCount, decimal bouncesUniqueCount, decimal success, decimal hardBouncesUniqueCount, decimal softBouncesUniqueCount, decimal unknownBouncesUniqueCount, decimal subscribesUniqueCount, decimal resendsUniqueCount, decimal forwardsUniqueCount)
        {
            ClicksUnique.Text = clicksUniqueCount + "/" + sendTotal;
            OpensUnique.Text = opensUniqueCount + "/" + sendTotal;
            BouncesUnique.Text = bouncesUniqueCount + "/" + sendTotal;
            HardBouncesUnique.Text = hardBouncesUniqueCount + "/" + sendTotal;
            SoftBouncesUnique.Text = softBouncesUniqueCount + "/" + sendTotal;
            UnknownBouncesUnique.Text = unknownBouncesUniqueCount + "/" + sendTotal;
            SubscribesUnique.Text = subscribesUniqueCount + "/" + sendTotal;
            ResendsUnique.Text = resendsUniqueCount + "/" + sendTotal;
            ForwardsUnique.Text = forwardsUniqueCount + "/" + sendTotal;
            Successful.Text = success + "/" + sendTotal;

            SuccessfulPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : success / sendTotal) * 100, 0).ToString() + "%";
            ClicksPercentage.Text = Decimal.Round((success == 0 ? 0 : clicksUniqueCount / success) * 100, 0).ToString() + "%";
            BouncesPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : bouncesUniqueCount / sendTotal) * 100, 0).ToString() + "%";
            HardBouncesPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : hardBouncesUniqueCount / sendTotal) * 100, 0).ToString() + "%";
            SoftBouncesPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : softBouncesUniqueCount / sendTotal) * 100, 0).ToString() + "%";
            UnknownBouncesPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : unknownBouncesUniqueCount / sendTotal) * 100, 0).ToString() + "%";
            OpensPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : opensUniqueCount / success) * 100, 0).ToString() + "%";
            SubscribesPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : subscribesUniqueCount / success) * 100, 0).ToString() + "%";
            ResendsPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : resendsUniqueCount / success) * 100, 0).ToString() + "%";
            ForwardsPercentage.Text = Decimal.Round((sendTotal == 0 ? 0 : forwardsUniqueCount / success) * 100, 0).ToString() + "%";
        }

        private void DisplaySetupCost(decimal setupCost, decimal outboundCost, decimal inboundCost, decimal designCost, decimal otherCost, int sendTotal, int bouncesUniqueCount)
        {
            SetupSetupCostLbl.Text = Convert.ToDecimal(setupCost).ToString();
            OutboundCostLbl.Text = Math.Round(Convert.ToDecimal(sendTotal * outboundCost), 2).ToString();
            InboundCostLbl.Text = Math.Round(Convert.ToDecimal(bouncesUniqueCount * inboundCost), 2).ToString();
            DesignCostLbl.Text = Convert.ToDecimal(designCost).ToString();
            OtherCostLbl.Text = Convert.ToDecimal(otherCost).ToString();

            try
            {
                TotalSetupLbl.Text = Math.Round(
                    Convert.ToDecimal(SetupSetupCostLbl.Text) +
                    Convert.ToDecimal(OutboundCostLbl.Text) +
                    Convert.ToDecimal(InboundCostLbl.Text) +
                    Convert.ToDecimal(DesignCostLbl.Text) +
                    Convert.ToDecimal(OtherCostLbl.Text), 2)
                    .ToString();
            }
            catch { TotalSetupLbl.Text = "0"; }
        }

        private void DisplayMessageFees(int sendTotal, int clicksUniqueCount, int opensUniqueCount)
        {
            try { EmailsSentLbl.Text = sendTotal.ToString(); } catch { EmailsSentLbl.Text = "0"; }
            try { PerEmailChargeLbl.Text = Math.Round(Convert.ToDecimal(TotalSetupLbl.Text) / Convert.ToDecimal(sendTotal.ToString()), 2).ToString(); } catch { PerEmailChargeLbl.Text = "0"; }
            try { ResponsesLbl.Text = opensUniqueCount.ToString(); } catch { ResponsesLbl.Text = "0"; }
            try { PerResponseLbl.Text = Math.Round(Convert.ToDecimal(TotalSetupLbl.Text) / Convert.ToDecimal(opensUniqueCount.ToString()), 2).ToString(); } catch { PerEmailChargeLbl.Text = "0"; }
            try { PerClickLbl.Text = Math.Round(Convert.ToDecimal(TotalSetupLbl.Text) / Convert.ToDecimal(clicksUniqueCount.ToString()), 2).ToString(); } catch { PerClickLbl.Text = "0"; }
        }

        private void DisplayROILabels(int sendTotal, int opensUniqueCount, int conversionsUniqueCount)
        {
            try { ROI_EmailsSentLbl.Text = sendTotal.ToString(); } catch { ROI_EmailsSentLbl.Text = "0"; }
            try { ROI_TotalResponse.Text = opensUniqueCount.ToString(); } catch { ROI_TotalResponse.Text = "0"; }
            try { ROI_TotalConversion.Text = conversionsUniqueCount.ToString(); } catch { ROI_TotalConversion.Text = "0"; }
            try { ROI_PerResponse.Text = Math.Round(Convert.ToDecimal(TotalSetupLbl.Text) / Convert.ToDecimal(opensUniqueCount.ToString()), 2).ToString(); } catch { ROI_PerResponse.Text = "0"; }
            try { ROI_PerClick.Text = Math.Round(Convert.ToDecimal(TotalSetupLbl.Text) / Convert.ToDecimal(conversionsUniqueCount.ToString()), 2).ToString(); } catch { ROI_PerClick.Text = "0"; }
            try { ROI_TotalInvestment.Text = Math.Round(Convert.ToDecimal(TotalSetupLbl.Text), 2).ToString(); } catch { ROI_SetupFees.Text = "0"; }
        }

        private void DisplayROIChart(decimal success, decimal clicksUniqueCount, decimal opensUniqueCount, decimal bouncesUniqueCount)
        {
            ROI_Chart = LoadChartProerties(ROI_Chart);
            ROI_Chart.Type = ChartType.ComboHorizontal;
            ROI_Chart.ChartArea.XAxis.Label.Text = string.Empty;
            ROI_Chart.ChartArea.YAxis.Label.Text = string.Empty;
            ROI_Chart.TempDirectory = ConfigurationManager.AppSettings[ConfigChartsTempDirectory];
            ROI_Chart.ShadingEffect = true;

            var seriesCollection = new SeriesCollection();
            AddSeries(seriesCollection, "Delivery", "Deliv", success);
            AddSeries(seriesCollection, "Opens", "Open", opensUniqueCount);
            AddSeries(seriesCollection, "Clicks", "Click", clicksUniqueCount);
            AddSeries(seriesCollection, "Bounces", "Bounc", bouncesUniqueCount);
            ROI_Chart.SeriesCollection.Add(seriesCollection);
        }

        private static void AddSeries(SeriesCollection seriesCollection, string name, string elementName, decimal yValue)
        {
            var series = new Series { Name = name };
            series.AddElements(
                new Element
                {
                    Name = elementName,
                    YValue = (double)yValue
                });
            seriesCollection.Add(series);
        }

        private void DisplayReponseDetailChart(string blastSendTime, DataTable reportingDT)
        {
            ResponseDetailChart = LoadChartProerties(ResponseDetailChart);
            ResponseDetailChart.Title = ResponseDetailChartTitle;
            ResponseDetailChart.Type = ChartType.Combo;
            ResponseDetailChart.ChartArea.XAxis.Label.Text = string.Empty;
            ResponseDetailChart.ChartArea.YAxis.Label.Text = string.Empty;
            ResponseDetailChart.TempDirectory = ConfigurationManager.AppSettings[ConfigChartsTempDirectory];
            ResponseDetailChart.ShadingEffect = true;
            
            var date = string.Empty;
            var reportDetailsSeriesCollection = new SeriesCollection();

            foreach (var type in ReportTypes)
            {
                var series = new Series { Name = type.ToUpper() + "S" };
                var clicksDayCount = 0;

                for (var i = 0; i < 10; i++)
                {
                    date = DateTime.Parse(blastSendTime).AddDays(i).ToString(DefaultDateFormat);
                    var monthDayDate = DateTime.Parse(date).ToString(MonthDayDateFormat);

                    var reportDetailClicksRows = reportingDT.Select($" ActionTypeCode = '{type}' AND ActionDateMMDDYYYY = '{date}' ");
                    try { clicksDayCount = reportDetailClicksRows.Length; } catch { clicksDayCount = 0; }

                    series.Elements.Add(
                        new Element
                        {
                            Name = monthDayDate,
                            YValue = Convert.ToDouble(clicksDayCount)
                        });
                }
                reportDetailsSeriesCollection.Add(series);
            }

            ResponseDetailChart.SeriesCollection.Add(reportDetailsSeriesCollection);
        }

        private void SetErrorCodeCounts(int setBlastID, string connectionString)
        {
            DataTable bouncesTable = null;
            try
            {
                var dbConn = new SqlConnection(connectionString);
                var bouncesReportCommand = new SqlCommand(ProcedureGetGraphicalBlastBounceReportData, dbConn)
                {
                    CommandTimeout = 0,
                    CommandType = CommandType.StoredProcedure
                };
                bouncesReportCommand.Parameters.Add(new SqlParameter(ParameterBlastId, SqlDbType.Int));
                bouncesReportCommand.Parameters[ParameterBlastId].Value = setBlastID;

                var bouncesRptDA = new SqlDataAdapter(bouncesReportCommand);
                var bouncesRptDS = new DataSet();
                
                try
                {
                    dbConn.Open();
                    bouncesRptDA.Fill(bouncesRptDS, ProcedureGetGraphicalBlastBounceReportData);
                }
                finally
                {
                    dbConn.Close();
                }                
                
                bouncesTable = bouncesRptDS.Tables[0];

                var hardBounceDV = bouncesTable.DefaultView;
                hardBounceDV.RowFilter = RowFilterHardBounce;
                HardBounceGrid.DataSource = hardBounceDV;
                HardBounceGrid.DataBind();

                var softBounceDV = bouncesTable.DefaultView;
                softBounceDV.RowFilter = RowFilterSoftBounce;
                SoftBounceGrid.DataSource = softBounceDV;
                SoftBounceGrid.DataBind();
            }
            catch (Exception e)
            {
                // TODO: Handle empty catch
            }
        }

        private void LoadClicksData(int setBlastID) {
			string connString = ConfigurationManager.AppSettings[ConfigActivity];
				SqlConnection dbConn		= new SqlConnection(connString);			
				SqlCommand cmd	= new SqlCommand("spGetClicksData",dbConn);
				cmd.CommandTimeout = 0;
				cmd.CommandType	= CommandType.StoredProcedure;		
	
				cmd.Parameters.Add(new SqlParameter(ParameterBlastId, SqlDbType.VarChar));
				cmd.Parameters[ParameterBlastId].Value = setBlastID.ToString();		

				cmd.Parameters.Add(new SqlParameter("@HowMuch", SqlDbType.VarChar));
				cmd.Parameters["@HowMuch"].Value = ClickSelectionDD.SelectedValue.ToString().Trim();
	
				cmd.Parameters.Add(new SqlParameter("@ISP", SqlDbType.VarChar));
				cmd.Parameters["@ISP"].Value = getISP();	

				cmd.Parameters.Add(new SqlParameter("@ReportType", SqlDbType.VarChar));
				cmd.Parameters["@ReportType"].Value = "topclicks";

				cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.VarChar));
				cmd.Parameters["@PageNo"].Value = 0;

				cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.VarChar));
				cmd.Parameters["@PageSize"].Value = 0;


				SqlDataAdapter clickssDA	= new SqlDataAdapter(cmd);
				DataSet ds	= new DataSet();			
				dbConn.Open();

				clickssDA.Fill(ds, "spGetClicksData");
				dbConn.Close();

				ClicksGrid.DataSource =ds.Tables[0].DefaultView;
				ClicksGrid.DataBind();
				ClicksPager.RecordCount = ds.Tables[0].Rows.Count;
			
		}		
		#endregion

		public void DownloadClickReport(int BlastID) {
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            SqlConnection dbConn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings[ConfigActivity]);
			SqlCommand topClicksRptCmd	= new SqlCommand("spClickActivity_DetailedReport",dbConn);
			topClicksRptCmd.CommandTimeout	= 100;
			topClicksRptCmd.CommandType= CommandType.StoredProcedure;

			topClicksRptCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
			topClicksRptCmd.Parameters["@BlastID"].Value = BlastID;

			topClicksRptCmd.Parameters.Add(new SqlParameter("@LinkURL", SqlDbType.VarChar));
			topClicksRptCmd.Parameters["@LinkURL"].Value = getActionURL().ToString();

			SqlDataAdapter da = new SqlDataAdapter(topClicksRptCmd);
			DataSet ds = new DataSet();
			da.Fill(ds, "spClickActivity_DetailedReport");
			dbConn.Close();
			DataTable dt = ds.Tables[0];
			
			string newline	= "";
			string contentType	= "application/vnd.ms-excel";
			string responseFile	= "Clicks.xls";
            string outFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");
            if (!Directory.Exists(outFilePath))
                Directory.CreateDirectory(outFilePath);

			//output txt file format <BlastID>_emails.txt
			DateTime date = DateTime.Now;
			String tfile = BlastID+"_Clicks.XLS";
			string outfileName		= outFilePath+tfile;
			
			if(File.Exists(outfileName)) {
				File.Delete(outfileName);
			}

			TextWriter txtfile= File.AppendText(outfileName);
			
			for(int i=0; i<dt.Columns.Count; i++) {
				columnHeadings.Add(dt.Columns[i].ColumnName.ToString());
			}
			aListEnum = columnHeadings.GetEnumerator();
			while(aListEnum.MoveNext()) {
				newline += aListEnum.Current.ToString()+"\t";
			}
			txtfile.WriteLine(newline);

			foreach ( DataRow dr in dt.Rows ) {
				newline = "";
				aListEnum.Reset();
				while(aListEnum.MoveNext()) {
					newline += dr[aListEnum.Current.ToString()].ToString()+"\t";
				}
				txtfile.WriteLine(newline);
			}

			txtfile.Close();

			Response.ContentType = contentType;
			Response.AddHeader( "content-disposition","attachment; filename="+responseFile);
			Response.WriteFile(outfileName);
			Response.Flush();
			Response.End();

			if(File.Exists(outfileName)) {
				File.Delete(outfileName);
			}
		}

		public void ClickSelectionDD_SelectedIndexChanged(object sender, System.EventArgs e) {}

		#region LoadGraph Props
		private Chart LoadChartProerties(Chart chart){
			chart.LegendBox.CornerBottomLeft = dotnetCHARTING.BoxCorner.Round;
			chart.LegendBox.CornerBottomRight = dotnetCHARTING.BoxCorner.Round;
			chart.LegendBox.CornerTopRight = dotnetCHARTING.BoxCorner.Round;
			chart.LegendBox.CornerTopLeft = dotnetCHARTING.BoxCorner.Round;
			chart.LegendBox.Position=LegendBoxPosition.BottomMiddle;
			chart.LegendBox.Background.Color = System.Drawing.Color.Orange;
			chart.ImageFormat = dotnetCHARTING.ImageFormat.Jpg;
			chart.Mentor = false;
			chart.Debug = false;

			return chart;
		}
		#endregion

		protected void Page_Init(object sender, EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
		}

		#region Web Form Designer generated code
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
		#endregion
       
	}
}
