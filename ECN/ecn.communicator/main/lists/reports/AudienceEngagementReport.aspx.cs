using System;
using System.Data;
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

namespace ecn.communicator.main.lists.reports
{
    public partial class AudienceEngagementReport : ReportPageBase
    {
        private const string ReportName = "AudienceEngagementReport";

        private IAudienceEngagementReportProxy _audienceEngagementProxy;

        private int getGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["groupID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getClickPercentage()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["cp"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getDays()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["days"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private string getDownloadType()
        {
            try
            {
                return Request.QueryString["dt"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string getProfileFilter()
        {
            try
            {
                return groupexportUDFsettings.selected;
            }
            catch
            {
                return string.Empty;
            }
        }

         delegate void HidePopup();

        public AudienceEngagementReport(
            IAudienceEngagementReportProxy audienceEngagementProxy,
            IReportDefinitionProvider reportDefinitionProvider,
            IReportContentGenerator reportContentGenerator)
            : base(reportDefinitionProvider, reportContentGenerator)
        {
            _audienceEngagementProxy = audienceEngagementProxy;
        }

        public AudienceEngagementReport()
        {
            _audienceEngagementProxy = new AudienceEngagementReportProxy();
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
            groupexportUDFsettings.CanDownloadTrans = false;
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AudienceEngagementReport, KMPlatform.Enums.Access.View))
                {
                    if (getDownloadType().Trim() != string.Empty && getGroupID() > 0)
                    {
                        try
                        {
                            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(getGroupID());
                            if (g != null)
                            {
                                lblSelectGroupName.Text = g.GroupName;
                                hfSelectGroupID.Value = g.GroupID.ToString();
                            }
                        }
                        catch { }

                        txtDays.Text = getDays().ToString();
                        txtClickPercent.Text = getClickPercentage().ToString();
                        ProcessDownload();
                    }
                    else
                    {
                        txtDays.Text = "60";
                        txtClickPercent.Text = "35";
                    }
                    if(!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AudienceEngagementReport, KMPlatform.Enums.Access.Download))
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

        private void ProcessDownload()
        {
            DataTable result = ECN_Framework_BusinessLayer.Activity.Report.AudienceEngagementReport.DownloadList(getGroupID(), getClickPercentage(), getDays(), getDownloadType(), getProfileFilter());
            
            if (result.Rows.Count > 0)
            {               
                //RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Web");
                //objExport.ExportDetails(result, RKLib.ExportData.Export.ExportFormat.CSV, getDownloadType().Replace(" ", "_") + "_" + getGroupID().ToString() + ".csv");
                string csv = ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.GetCsvFromDataTable(result);
                ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToCSV(csv, getDownloadType().Replace(" ", "_") + "_" + getGroupID().ToString());
            }
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
            ECNError ecnError = new ECNError(Enums.Entity.Group, Enums.Method.None, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
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
                throwECNException("Please select a Group.");
        }

        private void RenderReport(string exportFormat)
        {
            if (exportFormat.Equals(ReportConsts.OutputTypeXLSDATA, StringComparison.OrdinalIgnoreCase))
            {
                List<ECN_Framework_Entities.Activity.Report.AudienceEngagementReport> advertiserEngagement = ECN_Framework_BusinessLayer.Activity.Report.AudienceEngagementReport.GetList(Convert.ToInt32(hfSelectGroupID.Value.ToString()), Convert.ToInt32(txtClickPercent.Text), Convert.ToInt32(txtDays.Text), "N", "");
                if (advertiserEngagement != null)
                {
                    var laer = (from a in advertiserEngagement
                                select new { a.SortOrder, a.SubscriberType, a.Counts, a.Percentage, a.Description }).ToList();

                    String tfile = Master.UserSession.CurrentUser.CustomerID + "-AdvertiserEngagementReport";
                    laer.ExportCSV(tfile);
                }
            }
            else 
            {
                var sessionDataProvider = SafeGetSessionDataProvider(Master.UserSession);
                var currentUser = sessionDataProvider.GetCurrentUser();
                var selectedGroupId = Convert.ToInt32(hfSelectGroupID.Value);
                var clickPercent = Convert.ToInt32(txtClickPercent.Text);
                var days = Convert.ToInt32(txtDays.Text);
                var download = "N";
                var userHasAccess = KM.Platform.User.HasAccess(
                    currentUser, 
                    KMPlatform.Enums.Services.EMAILMARKETING,
                    KMPlatform.Enums.ServiceFeatures.AudienceEngagementReport,
                    KMPlatform.Enums.Access.ViewDetails);

                var audienceEngagementList = _audienceEngagementProxy.GetList(
                    selectedGroupId,
                    clickPercent,
                    days,
                    download,
                    string.Empty);
                var dataSource = new ReportDataSource("DS_AudienceEngagementReport", audienceEngagementList);
                var outputType = drpExport.SelectedItem.Text.ToUpper();
                var outputFileName = string.Format("{0}.{1}", ReportName, drpExport.SelectedItem.Text);
                var stream = GetReportDefinitionStream();
                var parameters = GetReportParameters(userHasAccess, exportFormat);

                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.EnableHyperlinks = userHasAccess;

                if (CanCreateReportResponseContent(exportFormat))
                {
                    CreateReportResponse(ReportViewer1, stream, dataSource, parameters, outputType, outputFileName);
                }
            }
        }

        private ReportParameter[] GetReportParameters(bool userHasAccess, string exportFormat)
        {
            return new[]
            {
                new ReportParameter("GroupName", lblSelectGroupName.Text),
                new ReportParameter("Format", userHasAccess ? exportFormat: ""),
                new ReportParameter("ClickPercentage", txtClickPercent.Text),
                new ReportParameter("GroupID", hfSelectGroupID.Value),
                new ReportParameter("Days", txtDays.Text),
                new ReportParameter("URL", Request.Url.ToString()),
                new ReportParameter("ProfileFilter", groupexportUDFsettings.selected),
                new ReportParameter("EnableHyperLinks", userHasAccess.ToString())
            };
        }

        private bool CanCreateReportResponseContent(string exportFormat)
        {
            return exportFormat.Equals(ReportConsts.OutputTypePDF, StringComparison.OrdinalIgnoreCase) || 
                   exportFormat.Equals(ReportConsts.OutputTypeXLS, StringComparison.OrdinalIgnoreCase);
        }

        private Stream GetReportDefinitionStream()
        {
            var assemblyLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var reportName = "ECN_Framework_Common.Reports.rpt_AudienceEngagementReport.rdlc";
            var reportDefinitionProvider = GetReportDefinitionProvider();

            return reportDefinitionProvider.GetReportDefinitionStream(assemblyLocation, reportName);
        }
    }
}
