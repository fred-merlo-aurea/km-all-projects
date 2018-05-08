using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using Microsoft.Reporting.WebForms;
using ECN_Framework_BusinessLayer.Communicator.Report;
using ECN_Framework_BusinessLayer.Communicator.Report.Interfaces;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists.reports
{
    public partial class ListSizeOvertimeReport : ReportPageBase
    {
        private const string ReportName = "ListSizeOverTimeReport";

        private IListSizeOvertimeReportProxy _listSizeOvertimeReportProxy;
        private delegate void HidePopup();
        
        public ListSizeOvertimeReport(
            IListSizeOvertimeReportProxy listSizeOvertimeReportProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _listSizeOvertimeReportProxy = listSizeOvertimeReportProxy;
        }

        public ListSizeOvertimeReport()
        {
            _listSizeOvertimeReportProxy = new ListSizeOvertimeReportProxy();
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
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ListSizeOverTimeReport, KMPlatform.Enums.Access.View))
                {
                    string stDate = DateTime.Now.AddMonths(-12).ToString("MM/dd/yyyy");
                    string edDate = DateTime.Now.ToString("MM/dd/yyyy");

                    txtstartDate.Text = DateTime.Now.AddMonths(-6).ToString("MM/dd/yyyy"); ;
                    txtendDate.Text = edDate;

                    rvStateDate.Type = ValidationDataType.Date;
                    rvStateDate.MinimumValue = stDate;
                    rvStateDate.MaximumValue = edDate;
                    rvStateDate.ErrorMessage = "Start date must be between " + stDate + " and " + edDate;

                    rvEndDate.Type = ValidationDataType.Date;
                    rvEndDate.MinimumValue = stDate;
                    rvEndDate.MaximumValue = edDate;
                    rvEndDate.ErrorMessage = "End date must be between " + stDate + " and " + edDate;
                    if(!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ListSizeOverTimeReport, KMPlatform.Enums.Access.Download))
                    {
                        drpExport.Items.Clear();
                        drpExport.Items.Add(new ListItem() { Text = "HTML", Value = "html", Selected = true });
                    }
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
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
            int groupid = -1;
            int.TryParse(hfSelectGroupID.Value.ToString(), out groupid);
            if (groupid > 0)
            {
                RenderReport(drpExport.SelectedItem.Text);
            }
            else
            {
                var ecnError = new ECNError(
                    Enums.Entity.Group,
                    Enums.Method.None,
                    "Please select a Group.");
                ThrowEcnException(ecnError, phError, lblErrorMessage);
            }
        }

        private void RenderReport(string exportFormat)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            int selectedGroupId;
            if (!int.TryParse(hfSelectGroupID.Value, out selectedGroupId))
            {
                var exceptionMessage = string.Format(
                    "ListSizeOvertimeReport.RenderReport: Value {0} can not be converted to proper group Id.",
                    hfSelectGroupID.Value);
                throw new InvalidOperationException(exceptionMessage);
            }

            var sizeOvertimeReportList = _listSizeOvertimeReportProxy.Get(selectedGroupId, startDate, endDate);
            var dataSource = new ReportDataSource("DataSet1", sizeOvertimeReportList);
            var filePath = Server.MapPath("~/main/lists/Reports/rpt_ListSizeOverTime.rdlc");
            var outputType = exportFormat.ToUpper();
            var outputFileName = string.Format("{0}.{1}", ReportName, exportFormat);
            var parameters = new[]
            {
                new ReportParameter("StartDate", txtstartDate.Text),
                new ReportParameter("EndDate", txtendDate.Text),
                new ReportParameter("GroupName", lblSelectGroupName.Text)
            };

            ReportViewer1.Visible = true;

            if (!exportFormat.Equals(ReportConsts.OutputTypeHTML, StringComparison.OrdinalIgnoreCase))
            {
                CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
            }
        }

    }
}