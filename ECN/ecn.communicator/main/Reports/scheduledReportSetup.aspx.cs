using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Reports.ReportSettingsControls;
using ECN_Framework_Common.Objects;
using KM.Common;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using Access = KMPlatform.Enums.Access;
using EntitiesReports = ECN_Framework_Entities.Communicator.Reports;
using Enums = ECN_Framework_Common.Objects.Enums;
using Features = KMPlatform.Enums.ServiceFeatures;
using Services = KMPlatform.Enums.Services;
using KMUser = KM.Platform.User;

namespace ecn.communicator.main.Reports
{
    public partial class scheduledReportSetup : System.Web.UI.Page
    {
        private const string AttributeOptGtoup = "OptGroup";
        private const string AttributeExports = "Exports";
        private const string AttributeReports = "Reports";
        private const string FieldReportName = "ReportName";
        private const string FieldReportId = "ReportID";
        private const string ItemTextSelect = "-Select-";
        private const string ItemValueZero = "0";

        Control ctlReportSettings;
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                if (getReportScheduleID() > 0)
                    Master.Heading = "Edit Schedule for Report";
                else
                    Master.Heading = "Create Schedule for Report";

                LoadReportDropDown();
                setupRecurringControls();

                if (getReportScheduleID() > 0)
                {
                    LoadReportSchedule(getReportScheduleID());
                }
                LoadReportSettingsControl();
            }
            else
            {
                ReloadControl();
            }
        }

        private void LoadReportDropDown()
        {
            var reportList = BusinessCommunicator.Reports.Get(Master.UserSession.CurrentUser);
            reportList = reportList.OrderBy(x => x.IsExport).ThenBy(x => x.ReportName).ToList();

            drpReports.DataTextField = FieldReportName;
            drpReports.DataValueField = FieldReportId;
            var counter = 0;
            foreach (var report in reportList)
            {
                if (report.ShowInSetup)
                {
                    switch (report.ReportID)
                    {
                        case 1:
                            CheckAndAddDrp(Features.BlastDeliveryReport, Access.Download, report, counter);
                            break;
                        case 2:
                            CheckAndAddDrp(Features.EmailPreviewUsageReport, Access.View, report, counter);
                            break;
                        case 3:
                            CheckAndAddDrp(Features.EmailPerformanceByDomainReport, Access.Download, report, counter);
                            break;
                        case 4:
                            CheckAndAddDrp(Features.GroupStatisticsReport, Access.Download, report, counter);
                            break;
                        case 5:
                            CheckAndAddDrp(Features.AdvertiserClickReport, Access.View, report, counter);
                            break;
                        case 6:
                            CheckAndAddDrp(Features.AdvertiserClickReport, Access.DownloadDetails, report, counter);
                            break;
                        case 11:
                            CheckAndAddDrp(Features.GroupAttributeReport, Access.Download, report, counter);
                            break;
                        case 12:
                            CheckAndAddDrp(Features.UnsubscribeReasonReport, Access.DownloadDetails, report, counter);
                            break;
                        case 8:
                            CheckAndAddDrp(Features.GroupExport, Access.FullAccess, report, counter);
                            break;
                        default:
                            AddDrpReport(report, counter);
                            break;
                    }
                    ++counter;
                }
            }

            drpReports.Items.Insert(0, new ListItem(ItemTextSelect, ItemValueZero));
        }

        private void CheckAndAddDrp(Features features, Access access, EntitiesReports report, int counter)
        {
            if (KMUser.HasAccess(Master.UserSession.CurrentUser, Services.EMAILMARKETING, features, access))
            {
                AddDrpReport(report, counter);
            }
        }

        private void AddDrpReport(EntitiesReports report, int counter)
        {
            Guard.NotNull(report, nameof(report));

            var item = new ListItem(report.ReportName, report.ReportID.ToString());
            item.Attributes[AttributeOptGtoup] = 
                report.IsExport ? 
                    AttributeExports : 
                    AttributeReports;
            drpReports.Items.Insert(counter, item);
        }

        private void ReloadControl()
        {
            int reportID = Convert.ToInt32(drpReports.SelectedValue);
            ECN_Framework_Entities.Communicator.Reports report = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(reportID, Master.UserSession.CurrentUser);
            try
            {
                ctlReportSettings = Page.LoadControl("ReportSettingsControls/" + report.ControlName);
                ctlReportSettings.ID = "ReportSettings";
                phReportSettings.Controls.Add(ctlReportSettings);
            }
            catch { }
        }

        void LoadReportSettingsControl()
        {
            dllFormats.Visible = true;
            lblFormat.Visible = true;

            int reportID = Convert.ToInt32(drpReports.SelectedValue);
            phReportSettings.Controls.Clear();
            if (reportID > 0)
            {
                ECN_Framework_Entities.Communicator.Reports report = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(reportID, Master.UserSession.CurrentUser);
                try
                {


                    ctlReportSettings = Page.LoadControl("ReportSettingsControls/" + report.ControlName);
                    ctlReportSettings.ID = "ReportSettings";
                    phReportSettings.Controls.Add(ctlReportSettings);
                    ((IReportSettingsControl)ctlReportSettings).SetParameters(getReportScheduleID());
                    if (report.ControlName.ToLower().Equals("groupexport.ascx"))
                    {
                        Label2.Text = "Confirmation Email";
                        dllFormats.Visible = false;
                        lblFormat.Visible = false;
                        if (dllFormats.Items.FindByValue("xml") == null)
                            dllFormats.Items.Add(new ListItem() { Text = "XML [.xml]", Value = "xml" });

                    }
                    else if (report.ControlName.ToLower().Equals("datadumpreport.ascx"))
                    {
                        ListItem li = dllFormats.Items.FindByValue("xml");
                        dllFormats.Items.Remove(li);
                        ListItem li2 = dllFormats.Items.FindByValue("pdf");
                        dllFormats.Items.Remove(li2);
                    }
                    else
                    {
                        if (dllFormats.Items.FindByValue("xml") == null)
                            dllFormats.Items.Add(new ListItem() { Text = "XML [.xml]", Value = "xml" });

                        Label2.Text = "Envelope";
                    }
                }
                catch (HttpException ex)
                {
                    ctlReportSettings = null;
                }

            }
        }

        private void LoadReportSchedule(int ReportScheduleID)
        {
            ECN_Framework_Entities.Communicator.ReportSchedule ReportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(ReportScheduleID, Master.UserSession.CurrentUser);
            if (ReportSchedule != null)
            {
                if (ECN_Framework_Common.Functions.StringFunctions.HasValue(ReportSchedule.RecurrenceType))
                {
                    pnlRecurrence.Visible = true;
                    pnlRecurring.Visible = true;
                    pnlOneTime.Visible = false;
                    ddlScheduleType.SelectedValue = "Recurring";
                    ddlRecurrence.SelectedValue = ReportSchedule.RecurrenceType.ToString();
                    txtRecurringStartDate.Text = ReportSchedule.StartDate;
                    ddlRecurringStartTime.SelectedValue = ReportSchedule.StartTime;
                    txtRecurringEndDate.Text = ReportSchedule.EndDate == null ? "" : ReportSchedule.EndDate;

                    if (dllFormats.Visible == true)
                        dllFormats.SelectedValue = ReportSchedule.ExportFormat;

                    switch (ddlRecurrence.SelectedValue)
                    {
                        case "Weekly":
                            pnlDays.Visible = true;
                            break;
                        case "Monthly":
                            pnlMonth.Visible = true;
                            break;
                        default:
                            break;
                    }

                    foreach (ListItem item in cbDays.Items)
                    {
                        switch (item.Value)
                        {
                            case "Sunday":
                                item.Selected = ReportSchedule.RunSunday.Value;
                                break;
                            case "Monday":
                                item.Selected = ReportSchedule.RunMonday.Value;
                                break;
                            case "Tuesday":
                                item.Selected = ReportSchedule.RunTuesday.Value;
                                break;
                            case "Wednesday":
                                item.Selected = ReportSchedule.RunWednesday.Value;
                                break;
                            case "Thursday":
                                item.Selected = ReportSchedule.RunThursday.Value;
                                break;
                            case "Friday":
                                item.Selected = ReportSchedule.RunFriday.Value;
                                break;
                            case "Saturday":
                                item.Selected = ReportSchedule.RunSaturday.Value;
                                break;
                        }
                    }

                    cbDays.SelectedValue = ReportSchedule.RunSunday.ToString();
                    cbDays.SelectedValue = ReportSchedule.RunMonday.ToString();
                    cbDays.SelectedValue = ReportSchedule.RunTuesday.ToString();
                    cbDays.SelectedValue = ReportSchedule.RunWednesday.ToString();
                    cbDays.SelectedValue = ReportSchedule.RunThursday.ToString();
                    cbDays.SelectedValue = ReportSchedule.RunFriday.ToString();
                    cbDays.SelectedValue = ReportSchedule.RunSaturday.ToString();
                }
                else
                {
                    pnlOneTime.Visible = true;
                    ddlStartTime.SelectedValue = ReportSchedule.StartTime;
                    txtStartDate.Text = ReportSchedule.StartDate;
                }
                ddlScheduleType.SelectedValue = ReportSchedule.ScheduleType;
                if(drpReports.Items.FindByValue(ReportSchedule.ReportID.ToString()) != null)
                    drpReports.SelectedValue = ReportSchedule.ReportID.ToString();
                else
                {
                    Response.Redirect("~/main/Reports/scheduledreportlist.aspx", false);
                }
                if (ReportSchedule.MonthLastDay.Value == true)
                {
                    drpDayofMonth.SelectedValue = "Last Day";
                }
                else
                {
                    drpDayofMonth.SelectedValue = ReportSchedule.MonthScheduleDay.ToString();
                }
                txtFromEmail.Text = ReportSchedule.FromEmail;
                txtFromName.Text = ReportSchedule.FromName;
                txtSubject.Text = ReportSchedule.EmailSubject;
                txtToEmail.Text = ReportSchedule.ToEmail;
            }
            else
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/error.aspx?E=" + ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink.ToString(), true);
            }
        }

        protected void ddlScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlScheduleType.SelectedValue)
            {
                case "One-Time":
                    pnlOneTime.Visible = true;
                    pnlRecurrence.Visible = false;
                    pnlRecurring.Visible = false;
                    pnlMonth.Visible = false;
                    pnlDays.Visible = false;
                    resetRecurringControls();
                    break;
                case "Recurring":
                    setupRecurringControls();
                    break;
                default:
                    break;
            }
        }

        private void setupRecurringControls()
        {
            pnlOneTime.Visible = false;
            pnlRecurrence.Visible = true;
            pnlRecurring.Visible = true;
            resetOneTimeControls();
        }

        private void resetRecurringControls()
        {
            ddlRecurrence.ClearSelection();
            txtRecurringStartDate.Text = "";
            ddlRecurringStartTime.ClearSelection();
            txtRecurringEndDate.Text = "";
            cbDays.ClearSelection();
            drpDayofMonth.SelectedValue = "1";
        }

        protected void ddlRecurrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlRecurrence.SelectedValue)
            {
                case "Daily":
                    pnlDays.Visible = false;
                    pnlMonth.Visible = false;
                    cbDays.ClearSelection();
                    drpDayofMonth.SelectedValue = "1";
                    break;
                case "Weekly":
                    pnlDays.Visible = true;
                    pnlMonth.Visible = false;
                    drpDayofMonth.SelectedValue = "1";
                    break;
                case "Monthly":
                    pnlDays.Visible = false;
                    pnlMonth.Visible = true;
                    cbDays.ClearSelection();
                    break;
                default:
                    break;
            }
        }

        private void resetOneTimeControls()
        {
            txtStartDate.Text = "";
            ddlStartTime.ClearSelection();
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
            ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region validation checks
                string[] toEmails = txtToEmail.Text.Split(',');

                foreach (string s in toEmails.AsEnumerable())
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(s.Trim()))
                    {
                        throwECNException("Invalid To Email Address:" + s);
                        return;
                    }

                }

                if (!ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(txtFromEmail.Text.Trim()))
                {
                    throwECNException("Invalid From Email Address:" + txtFromEmail.Text.Trim());
                    return;
                }

                DateTime startDate = new DateTime();
                DateTime.TryParse(txtRecurringStartDate.Text, out startDate);
                DateTime endDate = new DateTime();
                DateTime.TryParse(txtRecurringEndDate.Text, out endDate);

                if (startDate < DateTime.Now.Date)
                {
                    throwECNException("Start Date cannot be in the past");
                    return;
                }
                else if (startDate == DateTime.Now.Date)
                {
                    TimeSpan ts = new TimeSpan();
                    TimeSpan.TryParse(ddlRecurringStartTime.SelectedValue.ToString(), out ts);
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    if (ts < currentTime)
                    {
                        throwECNException("Send Time cannot be in the past");
                        return;
                    }
                }

                if (endDate < DateTime.Now.Date)
                {
                    throwECNException("End Date cannot be in the past");
                    return;
                }

                if (drpReports.SelectedIndex == 0)
                {
                    throwECNException("Please select a Report");
                    return;
                }
                
                if (ctlReportSettings != null)
                {
                    if (!((IReportSettingsControl)ctlReportSettings).IsValid())
                    {
                        throwECNException("FTP Credentials are not valid.");
                        return;
           
                    }
                }

                #endregion

                ECN_Framework_Entities.Communicator.ReportSchedule reportSchedule;
                if (getReportScheduleID() > 0)
                {
                    reportSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByReportScheduleID(getReportScheduleID(), Master.UserSession.CurrentUser);
                    reportSchedule.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                }
                else
                {
                    reportSchedule = new ECN_Framework_Entities.Communicator.ReportSchedule();
                    reportSchedule.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                }
                reportSchedule.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                reportSchedule.ReportID = Convert.ToInt32(drpReports.SelectedValue);
                reportSchedule.ScheduleType = ddlScheduleType.SelectedValue;
                if (ddlScheduleType.SelectedValue == "Recurring")
                {
                    if (ddlRecurrence.SelectedValue.Equals("Weekly"))
                    {
                        if (cbDays.SelectedIndex == -1)
                        {
                            throwECNException("Please select Days");
                            return;
                        }
                    }
                    reportSchedule.RecurrenceType = ddlRecurrence.SelectedValue;
                    reportSchedule.StartDate = txtRecurringStartDate.Text;
                    reportSchedule.StartTime = ddlRecurringStartTime.SelectedValue;
                    reportSchedule.EndDate = txtRecurringEndDate.Text == "" ? null : txtRecurringEndDate.Text;
                }
                else
                {
                    reportSchedule.RecurrenceType = null;
                    reportSchedule.StartDate = txtStartDate.Text;
                    reportSchedule.StartTime = ddlStartTime.SelectedValue;
                }

                foreach (ListItem item in cbDays.Items)
                {
                    switch (item.Value)
                    {
                        case "Sunday":
                            reportSchedule.RunSunday = item.Selected;
                            break;
                        case "Monday":
                            reportSchedule.RunMonday = item.Selected;
                            break;
                        case "Tuesday":
                            reportSchedule.RunTuesday = item.Selected;
                            break;
                        case "Wednesday":
                            reportSchedule.RunWednesday = item.Selected;
                            break;
                        case "Thursday":
                            reportSchedule.RunThursday = item.Selected;
                            break;
                        case "Friday":
                            reportSchedule.RunFriday = item.Selected;
                            break;
                        case "Saturday":
                            reportSchedule.RunSaturday = item.Selected;
                            break;
                    }
                }

                if (ddlRecurrence.SelectedValue.Equals("Monthly"))
                {
                    if (drpDayofMonth.SelectedValue == "Last Day")
                    {
                        reportSchedule.MonthLastDay = true;
                        reportSchedule.MonthScheduleDay = null;
                    }
                    else
                    {
                        reportSchedule.MonthLastDay = false;
                        reportSchedule.MonthScheduleDay = Convert.ToInt32(drpDayofMonth.SelectedValue);
                    }
                }
                else
                {
                    reportSchedule.MonthLastDay = false;
                    reportSchedule.MonthScheduleDay = null;
                }
                reportSchedule.FromEmail = txtFromEmail.Text;
                reportSchedule.FromName = txtFromName.Text;
                reportSchedule.EmailSubject = txtSubject.Text;
                reportSchedule.ToEmail = txtToEmail.Text;
                if (ctlReportSettings != null)
                    reportSchedule.ReportParameters = ((IReportSettingsControl)ctlReportSettings).GetParameters();
                else
                    reportSchedule.ReportParameters = null;
                if (dllFormats.Visible == true)
                    reportSchedule.ExportFormat = dllFormats.SelectedValue;

                //clearing out previously scheduled reportqueue objects
                //if (reportSchedule.ReportScheduleID > 0)
                    //ECN_Framework_BusinessLayer.Communicator.ReportQueue.Delete_ReportScheduleID(reportSchedule.ReportScheduleID);

                ECN_Framework_BusinessLayer.Communicator.ReportSchedule.Save(reportSchedule, Master.UserSession.CurrentUser);
                Response.Redirect("scheduledReportList.aspx");
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        private int getReportScheduleID()
        {
            if (Request.QueryString["ReportSchedule"] != null)
                return Convert.ToInt32(Request.QueryString["ReportSchedule"].ToString());
            else
                return -1;
        }

        protected void drpReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            dllFormats.Visible = true;
            lblFormat.Visible = true;

            int reportID = Convert.ToInt32(drpReports.SelectedValue);
            phReportSettings.Controls.Clear();
            if (reportID > 0)
            {
                ECN_Framework_Entities.Communicator.Reports report = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(reportID, Master.UserSession.CurrentUser);

                try
                {

                    ctlReportSettings = Page.LoadControl("ReportSettingsControls/" + report.ControlName);
                    ctlReportSettings.ID = "ReportSettings";
                    phReportSettings.Controls.Add(ctlReportSettings);
                    ((IReportSettingsControl)ctlReportSettings).SetParameters(getReportScheduleID());
                    if (report.ControlName.ToLower().Equals("groupexport.ascx"))
                    {
                        Label2.Text = "Confirmation Email";
                        dllFormats.Visible = false;
                        lblFormat.Visible = false;
                        if (dllFormats.Items.FindByValue("xml") == null)
                            dllFormats.Items.Add(new ListItem() { Text = "XML [.xml]", Value = "xml" });

                    }
                    else if (report.ControlName.ToLower().Equals("datadumpreport.ascx"))
                    {
                        ListItem li = dllFormats.Items.FindByValue("xml");
                        dllFormats.Items.Remove(li);
                        ListItem li2 = dllFormats.Items.FindByValue("pdf");
                        dllFormats.Items.Remove(li2);
                    }
                    else
                    {
                        Label2.Text = "Envelope";
                        if (dllFormats.Items.FindByValue("xml") == null)
                            dllFormats.Items.Add(new ListItem() { Text = "XML [.xml]", Value = "xml" });


                    }

                }
                catch (HttpException ex)
                {
                    ctlReportSettings = null;
                }

            }
        }


        //method to validate the FTP credentials
        public bool CredentialsValid(string login, string password, string ftpURL)
        {
            //try to post to ftp
            ECN_Framework_Common.Functions.FtpFunctions ftp = new ECN_Framework_Common.Functions.FtpFunctions(ftpURL, login, password);
            if (!ftp.ValidateCredentials(login, password, ftpURL, "", ""))
                return false;

            return true;
        }
    }
}