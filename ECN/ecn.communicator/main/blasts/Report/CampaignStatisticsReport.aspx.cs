using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Common;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.blasts.Report
{
    public partial class CampaignStatisticsReport : ReportPageBase
    {
        private const string ReportDataSourceName = "DataSet1";

        private ICampaignStatisticsReportProxy _campaignStatisticsReportProxy;

        public CampaignStatisticsReport(
            ICampaignStatisticsReportProxy campaignStatisticsReportProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _campaignStatisticsReportProxy = campaignStatisticsReportProxy;
        }

        public CampaignStatisticsReport()
        {
            _campaignStatisticsReportProxy = new CampaignStatisticsReportProxy();
        }

        delegate void HidePopup();
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            if(!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.CampaignStatisticsReport, KMPlatform.Enums.Access.Download))
                {
                    LoadData();
                }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
            }
        }
        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        private void LoadData()
        {
            List<ECN_Framework_Entities.Communicator.Campaign> listCampaigns = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NonArchived_NoAccessCheck(Master.UserSession.CurrentCustomer.CustomerID,false);
            ddlCampaigns.DataSource = listCampaigns.OrderBy(x => x.CampaignName);
            ddlCampaigns.DataTextField = "CampaignName";
            ddlCampaigns.DataValueField = "CampaignID";
            ddlCampaigns.DataBind();
            ddlCampaigns.Items.Insert(0, new ListItem() { Text = "--Select--", Value = "-1", Selected = true });

            txtstartDate.Text = DateTime.Now.AddDays(-29).ToString("MM/dd/yyyy");
            txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

        }

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
        }
        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = ctrlgroupsLookup1.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(groupID);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                    }
                    else
                    {

                    }
                    ctrlgroupsLookup1.Visible = false;
                }
            }
            catch { }
            return true;
        }
       
        protected void btnReport_Click(object sender, EventArgs e)
        {
            phError.Visible = false;
            regFrom.Validate();
            regTo.Validate();
            if(!regFrom.IsValid || !regTo.IsValid)
            {
                return;
            }

            DateTime startDate;
            DateTime endDate;

            if (ParseReportingDates(out startDate, out endDate))
            {
                if (IsReportingPeriodValid(startDate, endDate))
                {
                    if (ddlCampaigns.SelectedIndex > 0)
                    {
                        if (drpExport.SelectedValue.Equals("xlsdata", StringComparison.InvariantCultureIgnoreCase))
                        {
                            GenerateReportWithoutDefinitionFile(startDate, endDate);
                        }
                        else
                        {
                            GenerateReportWithDefinitionFile(startDate, endDate);
                        }
                    }
                }
            }
        }

        private bool ParseReportingDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = new DateTime();
            endDate = new DateTime();

            if (!DateTime.TryParse(txtstartDate.Text, out startDate))
            {
                throwECNException("Invalid start date");
                return false;
            }
            
            if (!DateTime.TryParse(txtendDate.Text, out endDate))
            {
                throwECNException("Invalid end date");
                return false;
            }

            return true;
        }

        private bool IsReportingPeriodValid(DateTime startDate, DateTime endDate)
        {
            var reportTimeSpan = endDate.AddHours(23).AddMinutes(59).Subtract(startDate);

            if (startDate.Date <= new DateTime(startDate.Year, 2, 28).Date && DateTime.IsLeapYear(startDate.Year))
            {
                reportTimeSpan = reportTimeSpan.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            else if (endDate.Date > new DateTime(endDate.Year, 2, 28).Date && DateTime.IsLeapYear(endDate.Year))
            {
                reportTimeSpan = reportTimeSpan.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            
            if (Math.Abs(reportTimeSpan.TotalDays) > 365)
            {
                throwECNException("Date range cannot be more than 1 year");
                return false;
            }

            return true;
        }

        private void GenerateReportWithDefinitionFile(DateTime startDate, DateTime endDate)
        {
            int groupId;
            int.TryParse(hfSelectGroupID.Value, out groupId);

            IList<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport> campaignStatistics;
            if (groupId > 0)
            {
                campaignStatistics = _campaignStatisticsReportProxy.Get(
                    Convert.ToInt32(ddlCampaigns.SelectedValue),
                    startDate,
                    endDate,
                    groupId);
            }
            else
            {
                campaignStatistics = _campaignStatisticsReportProxy.Get(
                    Convert.ToInt32(ddlCampaigns.SelectedValue),
                    startDate,
                    endDate);
            }

            foreach (var campaignStatistic in campaignStatistics)
            {
                campaignStatistic.EmailSubject = EmojiFunctions.GetSubjectUTF(campaignStatistic.EmailSubject);
            }

            var dataSource = new ReportDataSource(ReportDataSourceName, campaignStatistics);
            var reportLocation = ConfigurationManager.AppSettings["ReportPath"] + "rpt_CampaignStatistics.rdlc";
            var filePath = Server.MapPath(reportLocation);
            var outputType = drpExport.SelectedValue.ToUpper();
            var outputFileName = $"CampaignStatisticsReport.{drpExport.SelectedValue}";
            var parameters = new[]
            {
                new ReportParameter("CampaignName", ddlCampaigns.SelectedItem.Text),
                new ReportParameter("StartDate", txtstartDate.Text),
                new ReportParameter("EndDate", txtendDate.Text)
            };

            ReportViewer1.Visible = false;
            CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
        }

        private void GenerateReportWithoutDefinitionFile(DateTime startDate, DateTime endDate)
        {
            int groupId;
            int.TryParse(hfSelectGroupID.Value, out groupId);

            List<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport> campaignStatistics;
            if (groupId > 0)
            {
                campaignStatistics = ECN_Framework_BusinessLayer.Activity.Report.CampaignStatisticsReport.Get(
                    Convert.ToInt32(ddlCampaigns.SelectedValue),
                    startDate,
                    endDate,
                    groupId);
            }
            else
            {
                campaignStatistics = ECN_Framework_BusinessLayer.Activity.Report.CampaignStatisticsReport.Get(
                    Convert.ToInt32(ddlCampaigns.SelectedValue),
                    startDate,
                    endDate);
            }
        
            var newList = new List<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport>();

            foreach (var campaignStatistic in campaignStatistics)
            {
                var newReport = new ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport
                    {
                        BlastID = campaignStatistic.BlastID,
                        EmailSubject = EmojiFunctions.GetSubjectUTF(campaignStatistic.EmailSubject),
                        CampaignItemName = campaignStatistic.CampaignItemName,
                        MessageName = campaignStatistic.MessageName,
                        FilterName = campaignStatistic.FilterName,
                        GroupName = campaignStatistic.GroupName,
                        SendTime = campaignStatistic.SendTime,
                        BounceTotal = campaignStatistic.BounceTotal,
                        SendTotal = campaignStatistic.SendTotal,
                        Delivered = campaignStatistic.Delivered,
                        SuppressedTotal = campaignStatistic.SuppressedTotal,
                        TotalClicks = campaignStatistic.TotalClicks,
                        TotalOpens = campaignStatistic.TotalOpens,
                        UniqueClicks = campaignStatistic.UniqueClicks,
                        UniqueOpens = campaignStatistic.UniqueOpens,
                        Unopened = campaignStatistic.SendTotal - campaignStatistic.UniqueOpens,
                        NoClicks = campaignStatistic.SendTotal - campaignStatistic.UniqueClicks,
                        ClickThrough = campaignStatistic.ClickThrough,
                        UnsubscribeTotal = campaignStatistic.UnsubscribeTotal,
                        MasterSuppressed = campaignStatistic.MasterSuppressed,
                        TotalAbuseComplaints = campaignStatistic.TotalAbuseComplaints,
                        TotalISPFeedbackLoops = campaignStatistic.TotalISPFeedbackLoops,
                        SuccessPercentage = CalculatePercentage(campaignStatistic.Delivered, campaignStatistic.SendTotal)
                    };

                newReport.UUnopenedDeliPercentage = CalculatePercentage(newReport.Unopened, campaignStatistic.Delivered);
                newReport.UNoClickOpenPercentage = CalculatePercentage(newReport.NoClicks, campaignStatistic.SendTotal);
                newReport.ClickDeliPercentage = CalculatePercentage(campaignStatistic.TotalClicks, campaignStatistic.SendTotal);
                newReport.TClicksOpenPercentage = CalculatePercentage(campaignStatistic.TotalClicks, campaignStatistic.TotalOpens);
                newReport.OpenDeliPercentage = CalculatePercentage(campaignStatistic.TotalOpens, campaignStatistic.Delivered);
                newReport.UClickDeliPercentage = CalculatePercentage(campaignStatistic.UniqueClicks, campaignStatistic.Delivered);
                newReport.UClicksOpenPercentage = CalculatePercentage(campaignStatistic.UniqueClicks, campaignStatistic.UniqueOpens);
                newReport.UOpenDeliPercentage = CalculatePercentage(campaignStatistic.UniqueOpens, campaignStatistic.Delivered);
                newReport.ClickThroughPercentage = CalculatePercentage(campaignStatistic.ClickThrough, campaignStatistic.Delivered);
                newList.Add(newReport);
            }

            var fileName = $"{Master.UserSession.CurrentUser.CustomerID}-CampaignStatisticsReport";

            if (drpExport.SelectedItem.ToString().Equals("XLSDATA", StringComparison.InvariantCultureIgnoreCase))
            {
                ReportViewerExport.ExportCSV(newList, fileName);
            }
            else
            {
                ReportViewerExport.ExportToExcel(newList, fileName);
            }
        }

        private decimal CalculatePercentage(int numerationFactor, int denumerationFactor)
        {
            if (denumerationFactor <= 0)
            {
                return 0;
            }

            return decimal.Round(
                Convert.ToDecimal(numerationFactor * 100) / Convert.ToDecimal(denumerationFactor),
                2,
                MidpointRounding.AwayFromZero);
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
        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Page, Enums.Method.None, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

    }
}