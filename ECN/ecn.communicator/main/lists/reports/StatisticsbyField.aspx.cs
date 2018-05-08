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
    public partial class StatisticsbyField : ReportPageBase
    {
        private const string ReportName = "StatisticsByField";

        private delegate void HidePopup();
        private IStatisticsByFieldProxy _statisticsByFieldProxy;

        public StatisticsbyField(
            IStatisticsByFieldProxy statisticsByFieldProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _statisticsByFieldProxy = statisticsByFieldProxy;
        }

        public StatisticsbyField()
        {
            _statisticsByFieldProxy = new StatisticsByFieldProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "reports";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;
            ctrlgroupsLookup1.ShowArchiveFilter = false;
            if(!Page.IsPostBack)
            {
                if(!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastStatisticsReport, KMPlatform.Enums.Access.Download))
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
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
        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Group, Enums.Method.None, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
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

                        List<ECN_Framework_Entities.Communicator.BlastAbstract> blastList = ECN_Framework_BusinessLayer.Communicator.Blast.GetByGroupID_NoAccessCheck(Convert.ToInt32(hfSelectGroupID.Value),  false);
                        var resultBlast = (from src in blastList
                                           where src.StatusCode.ToLower().Equals("sent") && src.TestBlast.ToLower().Equals("n") && src.SendTime > DateTime.Now.AddMonths(-6)
                                           orderby src.SendTime descending
                                           select new
                                           {
                                               BlastID = src.BlastID,
                                               EmailSubject = "(" + src.SendTime + ") " + ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(src.EmailSubject)
                                           }).ToList();

                        drpBlast.DataSource = resultBlast;
                        drpBlast.DataBind();

                        drpBlast.Items.Insert(0, new ListItem("Please Select a Blast", "0"));

                        List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(Convert.ToInt32(hfSelectGroupID.Value));
                        var resultGroupDataField = (from src in groupDataFieldsList
                                                    select new
                                                    {
                                                        fieldname = "UDF-" + src.ShortName
                                                    }).ToList();
                        drpFields.DataSource = resultGroupDataField;
                        drpFields.DataBind();

                        drpFields.Items.Insert(0, "TITLE");
                        drpFields.Items.Insert(0, "STATE");
                        drpFields.Items.Insert(0, "COUNTRY");
                        drpFields.Items.Insert(0, "COMPANY");
                        drpFields.Items.Insert(0, "CITY");

                        drpFields.Items.Insert(0, new ListItem(" --- Select Field --- ", "0"));

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
            var blastId = Convert.ToInt32(drpBlast.SelectedItem.Value);
            var fieldName = drpFields.SelectedItem.Value;
            var statisticsByFieldList = _statisticsByFieldProxy.Get(blastId, fieldName);

            if (drpExport.SelectedItem.Value.Equals(ReportConsts.OutputTypeXLSDATA, StringComparison.OrdinalIgnoreCase))
            {
                List<ECN_Framework_Entities.Activity.Report.StatisticsByField> newstatisticsbyField = new List<ECN_Framework_Entities.Activity.Report.StatisticsByField>();

                if (statisticsByFieldList != null)
                {
                    foreach (ECN_Framework_Entities.Activity.Report.StatisticsByField stat in statisticsByFieldList)
                    {
                        ECN_Framework_Entities.Activity.Report.StatisticsByField s = new ECN_Framework_Entities.Activity.Report.StatisticsByField();
                        s.Field = stat.Field;
                        s.USend = stat.USend;
                        s.UHBounce = stat.UHBounce;
                        s.USBounce = stat.USBounce;
                        s.Delivered = stat.USend - stat.UHBounce - stat.USBounce;
                        s.TOpen = stat.TOpen;
                        s.TotalOpenPercentage = stat.USend - stat.UHBounce - stat.USBounce > 0 ? decimal.Round(Convert.ToDecimal(stat.TOpen * 100) / Convert.ToDecimal(stat.USend - stat.UHBounce - stat.USBounce), 2, MidpointRounding.AwayFromZero) : 0;
                        s.UOpen = stat.UOpen;
                        s.UniqueOpenPercentage = stat.USend - stat.UHBounce - stat.USBounce > 0 ? decimal.Round(Convert.ToDecimal(stat.UOpen * 100) / Convert.ToDecimal(stat.USend - stat.UHBounce - stat.USBounce), 2, MidpointRounding.AwayFromZero) : 0;
                        s.TClick = stat.TClick;
                        s.TotalClickPercentage = stat.USend - stat.UHBounce - stat.USBounce > 0 ? decimal.Round(Convert.ToDecimal(stat.TClick * 100) / Convert.ToDecimal(stat.USend - stat.UHBounce - stat.USBounce), 2, MidpointRounding.AwayFromZero) : 0;
                        s.UClick = stat.UClick;
                        s.UniqueClickPercentage = stat.USend - stat.UHBounce - stat.USBounce > 0 ? decimal.Round(Convert.ToDecimal(stat.UClick * 100) / Convert.ToDecimal(stat.USend - stat.UHBounce - stat.USBounce), 2, MidpointRounding.AwayFromZero) : 0;
                        s.ClickThrough = stat.ClickThrough;
                        s.ClickThroughPercentage = s.Delivered > 0 ? decimal.Round(Convert.ToDecimal(stat.ClickThrough * 100) / Convert.ToDecimal(s.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                        newstatisticsbyField.Add(s);
                    }

                    String tfile = Master.UserSession.CurrentUser.CustomerID + "-StatisticsByField";
                    newstatisticsbyField.ExportCSV(tfile);
                }
            }
            else
            {
                var dataSource = new ReportDataSource("DS_StatisticsByField", statisticsByFieldList);
                var filePath = Server.MapPath("~/main/lists/Reports/rpt_StatisticsByField.rdlc");
                var outputType = exportFormat.ToUpper();
                var outputFileName = string.Format("{0}.{1}", ReportName, exportFormat);
                var parameters = new[]
                {
                    new ReportParameter("GroupName", lblSelectGroupName.Text),
                    new ReportParameter("BlastSubject", drpBlast.SelectedItem.Text.Substring(13)),
                    new ReportParameter("Field", drpFields.SelectedItem.Value),
                    new ReportParameter("SendDate", drpBlast.SelectedItem.Text.Substring(1, 10))
                };

                ReportViewer1.Visible = false;
                CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
            }
        }
    }
}
