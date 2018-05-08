using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using Microsoft.Reporting.WebForms;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.lists.reports
{
     delegate void HidePopup();
 

    public partial class OnOffReport : ReportPageBase
    {
        private const string ReportName = "OnOffsByField";

        private IOnOffsByFieldProxy _onOffsByFieldProxy;

        public OnOffReport(
            IOnOffsByFieldProxy onOffsByFieldProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _onOffsByFieldProxy = onOffsByFieldProxy;
        }

        public OnOffReport()
        {
            _onOffsByFieldProxy = new OnOffsByFieldProxy();
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
                if(KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.OnOffReport, KMPlatform.Enums.Access.Download))
                {
                    string stDate = DateTime.Now.AddMonths(-6).ToString("MM/dd/yyyy");
                    string edDate = DateTime.Now.ToString("MM/dd/yyyy");

                    txtstartDate.Text = stDate;
                    txtendDate.Text = edDate;

                }
                else
                {
                    throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.RoleAccess };
                }
                
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

                        List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(Convert.ToInt32(hfSelectGroupID.Value.ToString()));
                        var result = (from src in groupDataFieldsList
                                      select new
                                      {
                                          fieldname = "UDF-" + src.ShortName
                                      }).ToList();

                        drpFields.DataSource = result;
                        drpFields.DataBind();

                        drpFields.Items.Insert(0, "TITLE");
                        drpFields.Items.Insert(0, "STATE");
                        drpFields.Items.Insert(0, "COUNTRY");
                        drpFields.Items.Insert(0, "COMPANY");
                        drpFields.Items.Insert(0, "CITY");

                        drpFields.Items.Insert(0, new ListItem(" --- Select Field --- ", ""));

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
                throwECNException("Please select a Group.");
        }

        private void RenderReport(string exportFormat)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            var groupId = Convert.ToInt32(hfSelectGroupID.Value);
            var field = drpFields.SelectedItem.Value;
            var reportType = rbReporttype.SelectedItem.Value;

            var onOffs = _onOffsByFieldProxy.Get(groupId, field, startDate, endDate, reportType);
            
            if (exportFormat.Equals(ReportConsts.OutputTypeXLSDATA, StringComparison.OrdinalIgnoreCase))
            {
                if (onOffs != null)
                {
                    var sessionDataProvider = SafeGetSessionDataProvider(Master.UserSession);
                    var customerId = sessionDataProvider.GetCurrentUser().CustomerID;
                    var fileName = string.Format("{0}-{1}", customerId, ReportName);
                    var onOffsList = onOffs.ToList();
                    onOffsList.ExportCSV(fileName);
                }
            }
            else
            {
                var dataSource = new ReportDataSource("DS_OnOff", onOffs);
                ReportViewer1.Visible = false;

                var filePath = Server.MapPath("~/main/lists/Reports/rpt_OnOffReport.rdlc");
                var outputType = exportFormat.ToUpper();
                var outputFileName = string.Format("OnOffsByField.{0}", exportFormat);
                var parameters = new[]
                {
                    new ReportParameter("GroupName", lblSelectGroupName.Text),
                    new ReportParameter("StartDate", txtstartDate.Text),
                    new ReportParameter("EndDate", txtendDate.Text)
                };

                CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
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

    }
}
