using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using System.Data;
using System.Reflection;
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Text.RegularExpressions;
using KM.Common;
using BusinessReport = ECN_Framework_BusinessLayer.Activity.Report;
using EmojiFunctions = ECN_Framework_Common.Functions.EmojiFunctions;
using EntitiesReport = ECN_Framework_Entities.Activity.Report;
using BusinessBlastFieldsName = ECN_Framework_BusinessLayer.Communicator.BlastFieldsName;
using Enums = ECN_Framework_Common.Objects.Enums;

namespace ecn.communicator.main.lists.reports
{
    public partial class DataDumpReport : System.Web.UI.Page
    {
        private const int Factor100 = 100;
        private const int Factor1 = 1;

        private const string ExtensionXls = "xls";
        private const string BinEcnFrameworkCommon = "~/bin/ECN_Framework_Common.dll";
        private const string ResourceDataDumpReport = "ECN_Framework_Common.Reports.rpt_DataDumpReport.rdlc";
        private const string DataSet1 = "DataSet1";
        private const string ExportFormatXls = "XLS";
        private const string ExportFormatXlsData = "xlsdata";
        private const string RenderFormatExcel = "EXCEL";
        private const string AppTypevndMsExcel = "application/vnd.ms-excel";
        private const string HeaderContentDisposition = "content-disposition";
        private const string Field1 = "Field1";
        private const string Field2 = "Field2";
        private const string Field3 = "Field3";
        private const string Field4 = "Field4";
        private const string Field5 = "Field5";
        private const string FieldStartDate = "StartDate";
        private const string FieldEndDate = "EndDate";
        private const int YearDays = 365;
        private const char DelimCommaChar = ',';
        private const string DatePattern = @"^(\d{1,2})/(\d{1,2})/(\d{4}|\d{2})$";
        private const string ErrorRangeExceedsYear = "Date range cannot be more than 1 year.";
        private const string ErrorInvalidDates = "Invalid dates";
        private const string ErrorNoGroupSelected = "Please select at least one group.";

        private delegate void HidePopup();
        
        private List<ECN_Framework_Entities.Communicator.Group> groupList
        {
            get{
                if (ViewState["GroupList"] != null)
                {
                    return (List<ECN_Framework_Entities.Communicator.Group>)ViewState["GroupList"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["GroupList"] = value;
            }
            
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
            if(!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupAttributeReport, KMPlatform.Enums.Access.Download))
                {
                    groupList = new List<ECN_Framework_Entities.Communicator.Group>();
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected void btnReport_Click(object sender, EventArgs args)
        {
            DateTime startDate;
            DateTime endDate;
            var dateRegex = new Regex(DatePattern);
            if (groupList?.Count > 0)
            {
                if (DateTime.TryParse(txtstartDate.Text, out startDate) && 
                    DateTime.TryParse(txtendDate.Text, out endDate) && 
                    dateRegex.IsMatch(txtstartDate.Text.Trim()) && 
                    dateRegex.IsMatch(txtendDate.Text.Trim()))
                {
                    var delta = endDate.Subtract(startDate);
                    if (delta.Days < YearDays)
                    {
                        var builder = new StringBuilder();
                        foreach (var currentGroup in groupList)
                        {
                            builder.AppendFormat("{0},", currentGroup.GroupID);
                        }
                        var groupIds = builder.ToString().TrimEnd(DelimCommaChar);
                        var exportFormat = drpExport.SelectedValue.ToLower();
                        CreateReportFile(exportFormat, startDate, endDate, groupIds);
                    }
                    else
                    {
                        throwECNException(ErrorRangeExceedsYear);
                    }
                }
                else
                {
                    throwECNException(ErrorInvalidDates);
                }
            }
            else
            {
                throwECNException(ErrorNoGroupSelected);
            }
        }

        private void CreateReportFile(string exportFormat, DateTime startDate, DateTime endDate, string groupIds)
        {
            var fileName = $"GroupAttributeReport_{Master.UserSession.CurrentCustomer.CustomerID}_" +
                           $"{DateTime.Now.ToShortDateString()}";
            if (string.Equals(exportFormat, ExtensionXls, StringComparison.OrdinalIgnoreCase))
            {
                var currentUser = Master.UserSession.CurrentUser;
                var blastFieldsName1 = BusinessBlastFieldsName.GetByBlastFieldID(1, currentUser);
                var blastFieldsName2 = BusinessBlastFieldsName.GetByBlastFieldID(2, currentUser);
                var blastFieldsName3 = BusinessBlastFieldsName.GetByBlastFieldID(3, currentUser);
                var blastFieldsName4 = BusinessBlastFieldsName.GetByBlastFieldID(4, currentUser);
                var blastFieldsName5 = BusinessBlastFieldsName.GetByBlastFieldID(5, currentUser);

                var table = CreateReportTable(startDate, endDate, groupIds, Factor1);

                //run PDF or xls
                var assembly = Assembly.LoadFrom(HttpContext.Current.Server.MapPath(BinEcnFrameworkCommon));
                var stream = assembly.GetManifestResourceStream(ResourceDataDumpReport);
                ReportViewer1.LocalReport.LoadReportDefinition(stream);

                var reportDataSource = new ReportDataSource(DataSet1, table);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(reportDataSource);

                var parameters = new ReportParameter[7];
                parameters[0] = new ReportParameter(Field1, blastFieldsName1?.Name ?? Field1);
                parameters[1] = new ReportParameter(Field2, blastFieldsName2?.Name ?? Field2);
                parameters[2] = new ReportParameter(Field3, blastFieldsName3?.Name ?? Field3);
                parameters[3] = new ReportParameter(Field4, blastFieldsName4?.Name ?? Field4);
                parameters[4] = new ReportParameter(Field5, blastFieldsName5?.Name ?? Field5);
                parameters[5] = new ReportParameter(FieldStartDate, txtstartDate.Text);
                parameters[6] = new ReportParameter(FieldEndDate, txtendDate.Text);

                ReportViewer1.LocalReport.SetParameters(parameters);
                ReportViewer1.LocalReport.Refresh();

                var bytes = RenderReport(exportFormat);

                Response.Clear();
                Response.Buffer = true;

                Response.AddHeader(HeaderContentDisposition, $"attachment; filename={fileName}.{exportFormat}");
                Response.BinaryWrite(bytes);
                Response.End();
            }
            else if (exportFormat.Equals(ExportFormatXlsData))
            {
                var table = CreateReportTable(startDate, endDate, groupIds, 100);
                var csv = BusinessReport.ReportViewerExport.GetCSV(table);
                BusinessReport.ReportViewerExport.ExportToCSV(csv, fileName);
            }
        }

        private byte[] RenderReport(string exportFormat)
        {
            byte[] bytes = null;

            if (string.Equals(exportFormat, ExportFormatXls, StringComparison.OrdinalIgnoreCase))
            {
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = null;
                string encoding = null;
                string extension = null;
                bytes = ReportViewer1.LocalReport.Render(
                    RenderFormatExcel,
                    string.Empty,
                    out mimeType,
                    out encoding,
                    out extension,
                    out streamids,
                    out warnings);
                Response.ContentType = AppTypevndMsExcel;
            }

            return bytes;
        }

        private List<EntitiesReport.DataDumpReport> CreateReportTable(
            DateTime startDate, 
            DateTime endDate, 
            string groupIds, 
            int factor)
        {
            var table = BusinessReport.DataDumpReport.GetList(
                    Master.UserSession.CurrentCustomer.CustomerID, 
                    startDate, 
                    endDate, 
                    groupIds);
            foreach (var row in table)
            {
                row.EmailSubject = EmojiFunctions.GetSubjectUTF(row.EmailSubject);
                row.Delivery = row.usend - row.tbounce;

                row.OpensPercentOfDelivered = ReportRound(row.Delivery, row.topen, row.Delivery, factor);
                row.SuccessPerc = ReportRound(row.Delivery, row.Delivery, row.usend, factor);
                row.uOpensPerc = ReportRound(row.Delivery, row.uopen, row.Delivery, factor);
                row.tClickPerc = ReportRound(row.Delivery, row.tClick, row.Delivery, factor);
                row.tClicksOpensPerc = ReportRound(row.Delivery, row.tClick, row.topen, factor);
                row.uClicksOpensPerc = ReportRound(row.uopen, row.uClick, row.uopen, factor);
                row.SuppressedPerc = ReportRound(row.usend, row.Suppressed, row.usend, factor);
                row.uAbuseRpt_UnsubPerc = ReportRound(row.usend, row.uAbuseRpt_Unsub, row.usend, factor);
                row.uFeedBack_UnsubPerc = ReportRound(row.usend, row.uFeedBack_Unsub, row.usend, factor);
                row.uHardBouncePerc = ReportRound(row.usend, row.uHardBounce, row.usend, factor);
                row.uMastSup_UnsubPerc = ReportRound(row.usend, row.uMastSup_Unsub, row.usend, factor);
                row.uOtherBouncePerc = ReportRound(row.usend, row.uOtherBounce, row.usend, factor);
                row.uSoftBouncePerc = ReportRound(row.usend, row.uSoftBounce, row.usend, factor);
                row.uSubscribePerc = ReportRound(row.usend, row.uSubscribe, row.usend, factor);
                row.treferPerc = ReportRound(row.Delivery, row.trefer, row.Delivery, factor);
                row.tresendPerc = ReportRound(row.Delivery, row.tresend, row.Delivery, factor);
                row.tbouncePerc = ReportRound(row.usend, row.tbounce, row.usend, factor);
                row.sendPerc = ReportRound(row.Delivery, row.usend, row.usend, factor);
                row.ClickThroughPerc = ReportRound(row.Delivery, row.ClickThrough, row.Delivery, factor);
            }

            return table;
        }

        private static decimal ReportRound(
            decimal conditionNumber, 
            decimal numerator, 
            decimal denominator, 
            decimal factor = 1)
        {
            const int decimals = 2;
            const int defaultResult = 0;
            var result = conditionNumber > 0 && denominator != 0 ? 
                decimal.Round(
                    (numerator * factor) / denominator,
                    decimals,
                    MidpointRounding.AwayFromZero) :
                defaultResult;
            return result;
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

        protected void gvSelectedGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName.ToLower().Equals("deletegroup"))
            {
                groupList.RemoveAll(x => x.GroupID == Convert.ToInt32(e.CommandArgument.ToString()));
                gvSelectedGroups.DataSource = groupList;
                gvSelectedGroups.DataBind();
            }
        }

        protected void gvSelectedGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.Group currentgroup = (ECN_Framework_Entities.Communicator.Group)e.Row.DataItem;
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDeleteGroup");
                imgbtnDelete.CommandArgument = currentgroup.GroupID.ToString();

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
                        if(groupList.Count(x => x.GroupID == group.GroupID) == 0)
                        {
                            groupList.Add(group);
                        }
                        gvSelectedGroups.DataSource = groupList;
                        gvSelectedGroups.DataBind();
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
}