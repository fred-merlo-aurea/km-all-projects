using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Linq;
using ECN_Framework_BusinessLayer.Activity.Report;
using System.Text;
using System.IO;
using ECN_Framework.Common;
using ECN_Framework.Common.Interfaces;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists.reports
{
    public partial class groupStatisticsReport : ReportPageBase
    {
        private const string ReportName = "GroupStatisticsReport";

        private delegate void HidePopup();
        private IGroupStatisticsReportProxy _groupStatisticsProxy;

        public groupStatisticsReport(
            IGroupStatisticsReportProxy groupStatisticsProxy,
            IReportDefinitionProvider reportDefinitionProvider,
            IReportContentGenerator reportContentGenerator)
            : base(reportDefinitionProvider, reportContentGenerator)
        {
            _groupStatisticsProxy = groupStatisticsProxy;
        }

        public groupStatisticsReport()
        {
            _groupStatisticsProxy = new GroupStatisticsReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "reports";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";
            phError.Visible = false;
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupStatisticsReport, KMPlatform.Enums.Access.Download))
                {
                    string stDate = DateTime.Now.AddMonths(-12).ToString("MM/dd/yyyy");
                    string edDate = DateTime.Now.ToString("MM/dd/yyyy");
                    txtstartDate.Text = DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy"); ;
                    txtendDate.Text = edDate;
                    chkDetails.Visible = KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupStatisticsReport, KMPlatform.Enums.Access.DownloadDetails);
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            int groupid = -1;
            int.TryParse(hfSelectGroupID.Value.ToString(), out groupid);
            if (groupid > 0)
            {
                RenderReport(drpExport.SelectedItem.Text);
            }
            else
            {
                ECNError ecnError = new ECNError(
                    Enums.Entity.Group, 
                    Enums.Method.None,
                    "Please select a Group.");
                ThrowEcnException(ecnError, phError, lblErrorMessage);
            }
        }
        
        private void RenderReport(string exportFormat)
        {
            phError.Visible = false;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            var selectedGroupId = Convert.ToInt32(hfSelectGroupID.Value);
            var groupStatisticsList = _groupStatisticsProxy.Get(selectedGroupId, startDate, endDate).ToList();
            if (ValidateDateRange(startDate, endDate))
            {
                if (exportFormat.Equals(ReportConsts.OutputTypeXLSDATA, StringComparison.OrdinalIgnoreCase))
                {
                    List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> newgroupStatistics = ECN_Framework_BusinessLayer.Activity.Report.GroupStatisticsReport.GetReportDetails(groupStatisticsList, chkDetails.Checked);

                    String tfile = Master.UserSession.CurrentUser.CustomerID + "-GroupStatisticsReport";

                    StringBuilder sb = ECN_Framework_BusinessLayer.Activity.Report.GroupStatisticsReport.AddDelimiter(newgroupStatistics);

                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToCSV(sb.ToString(), tfile);
                }
                else
                {
                    foreach (var groupStatistic in groupStatisticsList)
                    {
                        groupStatistic.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(groupStatistic.EmailSubject);
                    }

                    var dataSource = new ReportDataSource("DS_GroupStatisticsReport", groupStatisticsList);
                    var parameters = GetReportParameters();
                    var outputType = drpExport.SelectedItem.Text.ToUpper();
                    var outputFileName = string.Format("{0}.{1}", ReportName, drpExport.SelectedItem.Text);
                    var stream = GetReportDefStream("ECN_Framework_Common.Reports.rpt_GroupStatisticsReport.rdlc");

                    if (chkDetails.Checked)
                    {
                        var subRepStream = GetReportDefStream("ECN_Framework_Common.Reports.rpt_Platform_SubReport.rdlc");
                        ReportViewer1.LocalReport.LoadSubreportDefinition("rpt_Platform_SubReport", subRepStream);
                        ReportViewer1.LocalReport.SubreportProcessing += Platform_SubReportProcessing;
                    }
                    ReportViewer1.Visible = false;

                    CreateReportResponse(ReportViewer1, stream, dataSource, parameters, outputType, outputFileName);
                }
            }
            else
            {
                ReportViewer1.Visible = false;
                lblErrorMessage.Text = "Date range cannot be more than 1 year";
                phError.Visible = true;
            }
        }

        private static bool ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.Now;
            TimeSpan dateRange = endDate - startDate;
            TimeSpan yearSpan = new TimeSpan(365, 0, 0, 0);

            if (startDate <= endDate && dateRange < yearSpan)
            {
                return true;
            }
            else
                return false;

        }

        private Stream GetReportDefStream(string reportName)
        {
            var assemblyLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var reportDefinitionProvider = GetReportDefinitionProvider();

            return reportDefinitionProvider.GetReportDefinitionStream(assemblyLocation, reportName);
        }

        private ReportParameter[] GetReportParameters()
        {
            return new[]
            {
                new ReportParameter("GroupName", lblSelectGroupName.Text),
                new ReportParameter("StartDate", txtstartDate.Text),
                new ReportParameter("EndDate", txtendDate.Text),
                new ReportParameter("Details", chkDetails.Checked.ToString())
            };
        }

        void Platform_SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.Parameters["BlastID"] != null)
            {
                int blastID = -1;
                int.TryParse(e.Parameters["BlastID"].Values[0].ToString(), out blastID);
                List<ECN_Framework_Entities.Activity.BlastActivityOpens> openslist = ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByBlastID(blastID).ToList();
                List<ECN_Framework_Entities.Activity.Platforms> plist = ECN_Framework_BusinessLayer.Activity.Platforms.Get();
                List<ECN_Framework_Entities.Activity.EmailClients> eclist = ECN_Framework_BusinessLayer.Activity.EmailClients.Get();
                List<int> platforms = openslist.Select(x => x.PlatformID).Distinct().ToList();
                List<PlatformData> listPlatform = new List<PlatformData>();
                foreach (int i in platforms)
                {
                    if (i != 5)
                    {

                        List<PlatformData> listOrder = new List<PlatformData>();
                        List<ECN_Framework_Entities.Activity.BlastActivityOpens> tempList = openslist.Where(x => x.PlatformID == i).ToList();
                        List<int> listEmailClients = tempList.Select(x => x.EmailClientID).Distinct().ToList();
                        foreach (int j in listEmailClients)
                        {
                            if (j != 15)
                            {
                                PlatformData pd = new PlatformData();
                                pd.PlatformName = plist.First(x => x.PlatformID == i).PlatformName;
                                pd.EmailClientName = eclist.First(x => x.EmailClientID == j).EmailClientName;
                                pd.Column1 = tempList.Where(x => x.EmailClientID == j).Count();
                                pd.Usage = Math.Round(((float)pd.Column1 * 100 / openslist.Count), 2).ToString() + "%";

                                listOrder.Add(pd);

                            }
                        }

                        listPlatform.AddRange(listOrder.OrderByDescending(x => x.Column1).ToList());
                    }
                }

                ReportDataSource rds = new ReportDataSource("DataSet1", listPlatform);

                e.DataSources.Add(rds);
            }
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
    }

    public partial class PlatformData
    {
        public string PlatformName { get; set; }
        public int Column1 { get; set; }
        public string Usage { get; set; }
        public string EmailClientName { get; set; }
    }
}
