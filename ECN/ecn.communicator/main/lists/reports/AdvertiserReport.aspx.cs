using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Reporting.WebForms;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework.Common;
using ECN_Framework.Common.Interfaces;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Common.Objects;
using FrameworkFunctions = ECN_Framework_Common.Functions;

namespace ecn.communicator.main.lists.reports
{
    public partial class AdvertiserReport : ReportPageBase
    {
        private delegate void HidePopup();
        private int _allowedYears = 0;
        private IAdvertiserClickReportProxy _advertiserClickReportProxy;

        public AdvertiserReport(
            IAdvertiserClickReportProxy advertiserClickReportProxy,
            IReportDefinitionProvider reportDefinitionProvider,
            IReportContentGenerator reportContentGenerator)
            : base(reportDefinitionProvider, reportContentGenerator)
        {
            _advertiserClickReportProxy = advertiserClickReportProxy;
        }

        public AdvertiserReport()
        {
            _advertiserClickReportProxy = new AdvertiserClickReportProxy();
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
            string stDate = string.Empty;
            if (Master.UserSession.CurrentBaseChannel.BounceDomain == "kmpsgroupbounce.com")
            {
                //2yrs
                StatsHistoryMsg.Text = "Up to 2 year of statistics history is available.";
                stDate = DateTime.Now.AddMonths(-24).ToString("MM/dd/yyyy");
                _allowedYears = 2;
            }
            else
            {
                StatsHistoryMsg.Text = "Up to 1 year of statistics history is available.";
                stDate = DateTime.Now.AddMonths(-12).ToString("MM/dd/yyyy");
                _allowedYears = 1;
            }
            
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AdvertiserClickReport, KMPlatform.Enums.Access.DownloadDetails))
                {
                    string edDate = DateTime.Now.ToString("MM/dd/yyyy");
                    txtstartDate.Text = DateTime.Now.AddMonths(-6).ToString("MM/dd/yyyy");
                    txtendDate.Text = edDate;

                    rvStateDate.Type = ValidationDataType.Date;
                    rvStateDate.MinimumValue = stDate;
                    rvStateDate.MaximumValue = edDate;
                    rvStateDate.ErrorMessage = "Start date must be between " + stDate + " and " + edDate;

                    rvEndDate.Type = ValidationDataType.Date;
                    rvEndDate.MinimumValue = stDate;
                    rvEndDate.MaximumValue = edDate;
                    rvEndDate.ErrorMessage = "End date must be between " + stDate + " and " + edDate;
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
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
        private void throwECNException(string message, Enums.Entity type)
        {
            ECNError ecnError = new ECNError(type, Enums.Method.None, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            var days = (Convert.ToDateTime(txtendDate.Text) - Convert.ToDateTime(txtstartDate.Text)).Days;
            if (days > (365))
            {
                throwECNException("Search range longer than the max of one year", Enums.Entity.DateRange);
            }
            else
            {
            int groupid = -1;
            int.TryParse(hfSelectGroupID.Value.ToString(), out groupid);
            if (groupid > 0)
            {
                RenderReport(drpExport.SelectedItem.Text);
            }
            else
                    throwECNException("Please select a Group.", Enums.Entity.Group);
            }
        }

        private void RenderReport(string exportFormat)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            var selectedGroupId = Convert.ToInt32(hfSelectGroupID.Value);
            var advertiserClickReportList = _advertiserClickReportProxy.GetList(selectedGroupId, startDate, endDate);

            if (exportFormat.Equals(ReportConsts.OutputTypeXLSDATA, StringComparison.OrdinalIgnoreCase))
            {
                if (advertiserClickReportList != null)
                {
                    var lacr = (from a in advertiserClickReportList
                                select new { a.BlastID, EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(a.EmailSubject), a.Date, LinkAlias = a.Alias, a.LinkURL, a.LinkOwner, a.UniqueCount, a.TotalCount }).ToList();

                    String tfile = Master.UserSession.CurrentUser.CustomerID + "-AdvertiserReport";
                    lacr.ExportCSV(tfile);
                }
            }
            else
            {
                foreach (var report in advertiserClickReportList)
                {
                    report.EmailSubject = FrameworkFunctions.EmojiFunctions.GetSubjectUTF(report.EmailSubject);
                }

                var dataSource = new ReportDataSource("DS_AdvertiserClickReport", advertiserClickReportList);
                var stream = GetReportDefinitionStream();
                var outputType = exportFormat.ToUpper();
                var outputFileName = string.Format("{0}.{1}", "AdvertiserClickReport", exportFormat);
                var parameters = new[]
                {
                    new ReportParameter("GroupName", lblSelectGroupName.Text),
                    new ReportParameter("StartDate", txtstartDate.Text),
                    new ReportParameter("EndDate", txtendDate.Text)
                };

                ReportViewer1.Visible = false;
                CreateReportResponse(ReportViewer1, stream, dataSource, parameters, outputType, outputFileName);
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

        private Stream GetReportDefinitionStream()
        {
            var assemblyLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var reportName = "ECN_Framework_Common.Reports.rpt_AdvertiserClickReport.rdlc";
            var reportDefinitionProvider = GetReportDefinitionProvider();

            return reportDefinitionProvider.GetReportDefinitionStream(assemblyLocation, reportName);
        }
    }
}
