using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.Reports
{
    public partial class scheduledReportList : System.Web.UI.Page
    {
        private const string RunSundayLabelName = "lblRunSunday";
        private const string RunMondayLabelName = "lblRunMonday";
        private const string RunTuesdayLabelName = "lblRunTuesday";
        private const string RunWednesdayLabelName = "lblRunWednesday";
        private const string RunThursdayLabelName = "lblRunThursday";
        private const string RunFridayLabelName = "lblRunFriday";
        private const string RunSaturdayLabelName = "lblRunSaturday";

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "scheduled reports";
            Master.Heading = "Reports > Scheduled Reports";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                List<ECN_Framework_Entities.Communicator.Reports> reportList = ECN_Framework_BusinessLayer.Communicator.Reports.Get(Master.UserSession.CurrentUser);
                //reportList = reportList.OrderBy(x => x.ReportName).ToList();

                //reportList.Sort((x, y) => x.IsExport == y.IsExport ? string.Compare(x.ReportName, y.ReportName) : (x.IsExport ? -1 : 1));
                reportList = reportList.OrderBy(x => x.IsExport).ThenBy(x => x.ReportName).ToList();
                //ddlFilter.DataTextField = "ReportName";
                //ddlFilter.DataValueField = "ReportID";

                //ddlFilter.SelectedIndex = 0;

                LoadReportDropDown();


                loadScheduledReports();
            }
        }

        private void LoadReportDropDown()
        {
            List<ECN_Framework_Entities.Communicator.Reports> reportList = ECN_Framework_BusinessLayer.Communicator.Reports.Get(Master.UserSession.CurrentUser);
            reportList = reportList.OrderBy(x => x.IsExport).ThenBy(x => x.ReportName).ToList();


            //ddlFilter.DataTextField = "ReportName";
            //ddlFilter.DataValueField = "ReportID";
            int j = 0;
            for (int i = 0; i < reportList.Count(); i++)
            {
                if (reportList[i].ShowInSetup == true)
                {
                    if (reportList[i].ReportID == 1)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastDeliveryReport, KMPlatform.Enums.Access.Download))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                           // ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 2)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreviewUsageReport, KMPlatform.Enums.Access.View))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                        //    ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 3)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPerformanceByDomainReport, KMPlatform.Enums.Access.Download))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                          //  ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 4)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupStatisticsReport, KMPlatform.Enums.Access.Download))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                            //ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 5)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AudienceEngagementReport, KMPlatform.Enums.Access.View))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                            //ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 6)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AdvertiserClickReport, KMPlatform.Enums.Access.DownloadDetails))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                            //ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 11)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupAttributeReport, KMPlatform.Enums.Access.Download))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                            //ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 12)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.UnsubscribeReasonReport, KMPlatform.Enums.Access.DownloadDetails))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                            //ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else if (reportList[i].ReportID == 8)
                    {
                        if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupExport, KMPlatform.Enums.Access.FullAccess))
                        {
                            ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                            li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                           // ddlFilter.Items.Insert(j, li);
                            j++;
                        }
                    }
                    else
                    {
                        ListItem li = new ListItem(reportList[i].ReportName, reportList[i].ReportID.ToString());
                        li.Attributes["OptGroup"] = reportList[i].IsExport ? "Exports" : "Reports";
                       // ddlFilter.Items.Insert(j, li);
                        j++;
                    }

                }
            }

           // ddlFilter.Items.Insert(0, new ListItem("All", "0"));
        }
        

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("scheduledReportSetup.aspx");
        }

        private void loadScheduledReports()
        {
            List<ECN_Framework_Entities.Communicator.ReportSchedule> ReportScheduleList = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
            ReportScheduleList = ReportScheduleList.Where(x => Convert.ToDateTime(x.EndDate) > DateTime.Now.AddDays(-1) && x.ReportID != 9 && x.ReportID != 10).ToList();

            gvScheduledReports.DataSource = ReportScheduleList;
            gvScheduledReports.DataBind();

            if (ReportScheduleList.Count > 0)
            {
                pnlNoReports.Visible = false;
                pnlReportList.Visible = true;
            }
            else
            {
                pnlNoReports.Visible = true;
                pnlReportList.Visible = false;
            }
        }

        protected void gvScheduledReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string ReportScheduleID = e.CommandArgument.ToString();
           
            if (e.CommandName=="ReportScheduleEdit")
            {
                Response.Redirect("~/main/Reports/scheduledReportSetup.aspx?ReportSchedule=" + ReportScheduleID, false);
                
            }
            else if (e.CommandName == "ReportScheduleDelete")
            {
                ECN_Framework_BusinessLayer.Communicator.ReportSchedule.Delete(Convert.ToInt32(ReportScheduleID), Master.UserSession.CurrentUser);
                loadScheduledReports();
            }
            else if (e.CommandName == "ReportDetails")
            {
                int ID = Convert.ToInt32(gvScheduledReports.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString());

                Panel ReportDetails = (Panel)gvScheduledReports.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("pnlBlastReport");
                GridView gvReportDetails = (GridView)gvScheduledReports.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("gvReportDetails");
                
                LinkButton lbDetails = (LinkButton)gvScheduledReports.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lnkbtnReportDetails");
                if (gvReportDetails.Rows.Count > 0)
                {

                    if (ReportDetails.Visible)
                    {
                        ReportDetails.Visible = false;

                        lbDetails.Text = "+Details";

                    }
                    else
                    {
                        lbDetails.Text = "-Details";
                        ReportDetails.Visible = true;

                    }

                }
                else
                {
                    if (lbDetails.Text.Equals("-Details"))
                    {
                        lbDetails.Text = "+Details";
                        ReportDetails.Visible = false;
                    }
                    else if (lbDetails.Text.Equals("+Details"))
                    {
                        lbDetails.Text = "-Details";
                        List<ECN_Framework_Entities.Communicator.ReportQueue> reportHistory = ECN_Framework_BusinessLayer.Communicator.ReportQueue.GetReportHistory(Convert.ToInt32(ID));
                        
                        ReportDetails.Visible = true;
                        reportHistory.Sort(delegate(ECN_Framework_Entities.Communicator.ReportQueue x, ECN_Framework_Entities.Communicator.ReportQueue y)
                        {
                            if (x.SendTime == y.SendTime) return 0;
                            if (x.SendTime < y.SendTime) return -1;
                            else return 1;
                        });
                        gvReportDetails.DataSource = reportHistory;
                        gvReportDetails.DataBind();

                        
                    }

                }
                //show ResendReport link based on the Report Status.
                //for (int i = 0; i < gvReportDetails.Rows.Count; i++)
                //{
                //    if (gvReportDetails.Rows[i].Cells[2].Text.ToLower() != "failed") gvReportDetails.Rows[i].Cells[3].Controls.Clear();
                //}
            }
        }

        protected void gvScheduledReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.ReportSchedule rs = (ECN_Framework_Entities.Communicator.ReportSchedule)e.Row.DataItem;
                
                Label lblScheduleType = (Label)e.Row.FindControl("lblScheduleType");
                Label lblRecurrenceType = (Label)e.Row.FindControl("lblRecurrenceType");
                Label lblNextScheduledDate = (Label)e.Row.FindControl("lblNextScheduledDate");
                Label lblStartTime = (Label)e.Row.FindControl("lblStartTime");
                Label lblEndDate = (Label)e.Row.FindControl("lblEndDate");
                Label lblStartDate = (Label)e.Row.FindControl("lblStartDate");
                Label lblReportName = (Label)e.Row.FindControl("lblReportName");
                LinkButton lbDetails = (LinkButton)e.Row.FindControl("lnkbtnReportDetails");
                lbDetails.CommandArgument = e.Row.RowIndex.ToString();
                ImageButton imgbtnParamDelete = (ImageButton)e.Row.FindControl("imgbtnParamDelete");
                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

                ECN_Framework_Entities.Communicator.Reports rep = ECN_Framework_BusinessLayer.Communicator.Reports.GetByReportID(rs.ReportID.Value, Master.UserSession.CurrentUser);

                lblReportName.Text = rep.ReportName;
                switch(rs.ReportID)
                {
                    case 1:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastDeliveryReport, KMPlatform.Enums.Access.Download);
                        break;
                    case 2:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPreviewUsageReport, KMPlatform.Enums.Access.View);
                        break;
                    case 3:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailPerformanceByDomainReport, KMPlatform.Enums.Access.Download);
                        break;
                    case 4:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupStatisticsReport, KMPlatform.Enums.Access.Download);
                        break;
                    case 5:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AudienceEngagementReport, KMPlatform.Enums.Access.View);
                        break;
                    case 6:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.AdvertiserClickReport, KMPlatform.Enums.Access.DownloadDetails);
                        break;
                    case 8:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupExport, KMPlatform.Enums.Access.FullAccess);
                        break;
                    case 11:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupAttributeReport, KMPlatform.Enums.Access.Download);
                        break;
                    case 12:
                        imgbtnEdit.Visible = imgbtnParamDelete.Visible = KMPlatform.BusinessLogic.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.UnsubscribeReasonReport, KMPlatform.Enums.Access.DownloadDetails);
                        break;

                }

                DateTime scheduledTime = DateTime.Now;
                DateTime ScheduleTime;
                if (Convert.ToDateTime(lblStartDate.Text) > DateTime.Now)
                    ScheduleTime = Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text);
                else
                    ScheduleTime = DateTime.Now;

                if (lblScheduleType.Text.Equals("Recurring"))
                {
                    lblScheduleType.Text = lblRecurrenceType.Text;

                    if (lblRecurrenceType.Text.ToLower() == "daily")
                    {
                        #region Daily
                        DateTime Today = DateTime.Now;
                        if (Convert.ToDateTime(lblStartDate.Text) > DateTime.Now)
                        {
                            scheduledTime = Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text);
                        }
                        else
                        {
                            scheduledTime = Convert.ToDateTime(Today.ToString("MM/dd/yyyy") + " " + lblStartTime.Text);
                            if (scheduledTime < DateTime.Now)
                                scheduledTime = scheduledTime.AddDays(1);
                        }
                        #endregion
                    }
                    else if (lblRecurrenceType.Text.ToLower() == "weekly")
                    {
                        var activeDayOfWeek = GetActiveDayOfWeekFromScheduledReportsGridRow(e.Row);
                        var startTime = TimeSpan.Zero;
                        TimeSpan.TryParse(lblStartTime.Text, out startTime);
                        var referenceDate = DateTime.Today.AddTicks(startTime.Ticks);

                        scheduledTime = CalculateScheduledDate(activeDayOfWeek, referenceDate);

                        if (DateTime.Now < Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text))
                        {
                            while (scheduledTime < Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text))
                            {
                                scheduledTime = scheduledTime.AddDays(7);
                            }
                        }
                        else
                        {
                            if (scheduledTime < DateTime.Now)
                            {
                                scheduledTime = scheduledTime.AddDays(7);
                            }
                        }
                    }
                    else if (lblRecurrenceType.Text.ToLower() == "monthly")
                    {
                        #region Monthly
                        Label lblMonthLastDay = (Label)e.Row.FindControl("lblMonthLastDay");
                        Label lblMonthScheduleDay = (Label)e.Row.FindControl("lblMonthScheduleDay");

                        int today = 0;
                        DateTime MonthScheduleDay;
                        today = DateTime.Now.Day;
                        today = today * -1;
                        if (lblMonthLastDay.Text.ToLower() == "false")
                        {
                            MonthScheduleDay = DateTime.Now.AddDays(today).AddDays(Convert.ToInt32(lblMonthScheduleDay.Text));
                            scheduledTime = Convert.ToDateTime(MonthScheduleDay.ToString("MM/dd/yyyy") + " " + lblStartTime.Text);
                            if (DateTime.Now < Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text))
                            {
                                while (scheduledTime < Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text))
                                {
                                    scheduledTime = scheduledTime.AddMonths(1);
                                }
                            }
                            else
                            {
                                if (scheduledTime < DateTime.Now)
                                {
                                    scheduledTime = scheduledTime.AddMonths(1);
                                }
                            }
                        }
                        else
                        {
                            //Get to first of current month and add one month to get to first of next month
                            MonthScheduleDay = DateTime.Now.AddDays(today + 1).AddMonths(1);

                            //Substract one day to get to last day of current month
                            MonthScheduleDay = MonthScheduleDay.AddDays(-1);

                            scheduledTime = Convert.ToDateTime(MonthScheduleDay.ToString("MM/dd/yyyy") + " " + lblStartTime.Text);

                            if (DateTime.Now < Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text))
                            {
                                while (scheduledTime < Convert.ToDateTime(Convert.ToDateTime(lblStartDate.Text).ToString("MM/dd/yyyy") + " " + lblStartTime.Text))
                                {
                                    scheduledTime = scheduledTime.AddDays(1);
                                    scheduledTime = scheduledTime.AddMonths(1);
                                    scheduledTime = scheduledTime.AddDays(-1);
                                }
                            }
                            else
                            {
                                if (scheduledTime < DateTime.Now)
                                {
                                    scheduledTime = scheduledTime.AddMonths(1);
                                }
                            }
                        }
                        #endregion
                    }

                    if (Convert.ToDateTime(lblEndDate.Text).AddDays(1) > scheduledTime)
                        lblNextScheduledDate.Text = scheduledTime.ToString();
                }
            }
        }

        protected void gvReportDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.ToLower().Equals("resend"))
            {
                ecn.controls.ecnGridView gvCurrent = (ecn.controls.ecnGridView)sender;
                int rQID = Convert.ToInt32(gvCurrent.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ReportQueueID"].ToString());
                int ReportScheduleID = Convert.ToInt32(gvCurrent.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ReportScheduleID"].ToString());
                ECN_Framework_BusinessLayer.Communicator.ReportQueue.Resend(rQID);
                GridViewRow gvr = gvCurrent.Rows[Convert.ToInt32(e.CommandArgument.ToString())];
                List<ECN_Framework_Entities.Communicator.ReportQueue> reportHistory = ECN_Framework_BusinessLayer.Communicator.ReportQueue.GetReportHistory(ReportScheduleID);
                gvCurrent.DataSource = reportHistory;
                gvCurrent.DataBind();

            }
        }

        //protected void lnkResendReport_Click(object sender, EventArgs e)
        //{
        //    ecn.controls.ecnGridView gvCurrent = (ecn.controls.ecnGridView)((LinkButton)sender).NamingContainer;
        //    //GridViewRowCommand row = gvCurrent.RowCommand

        //    //int rQID = Convert.ToInt32(gvCurrent.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ReportQueueID"].ToString());
            
        //    ////int ReportScheduleID = Convert.ToInt32(gvCurrent.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Values["ReportScheduleID"].ToString());
        //    ////ECN_Framework_BusinessLayer.Communicator.ReportQueue.Resend(rQID);
        //    ////GridViewRow gvr = gvCurrent.Rows[Convert.ToInt32(e.CommandArgument.ToString())];
        //    ////List<ECN_Framework_Entities.Communicator.ReportQueue> reportHistory = ECN_Framework_BusinessLayer.Communicator.ReportQueue.GetReportHistory(ReportScheduleID);
        //    ////gvCurrent.DataSource = reportHistory;
            
        //    //gvCurrent.DataBind();

        //}

        protected void gvReportDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.ReportQueue rq = (ECN_Framework_Entities.Communicator.ReportQueue)e.Row.DataItem;
                LinkButton lbResend = (LinkButton)e.Row.FindControl("lnkResendReport");
                if (rq.Status.ToLower().Equals("failed"))
                {
                    lbResend.Visible = true;
                    lbResend.CommandArgument = e.Row.RowIndex.ToString();
                }
                else
                    lbResend.Visible = false;
            }
        }

        //protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    List<ECN_Framework_Entities.Communicator.ReportSchedule> lstSchedule = ECN_Framework_BusinessLayer.Communicator.ReportSchedule.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
        //    if(ddlFilter.SelectedIndex != 0)
        //    {
        //        lstSchedule = lstSchedule.Where(x => x.ReportID == Convert.ToInt32(ddlFilter.SelectedValue.ToString())).ToList();
        //    }
        //    var reuslt = (from src in lstSchedule
        //                  where Convert.ToDateTime(src.EndDate) > DateTime.Now.AddDays(-1) && src.ReportID != 9 && src.ReportID != 10
        //                  select new
        //                  {
        //                      ReportScheduleID = src.ReportScheduleID,
        //                      ReportName = src.Report.ReportName,
        //                      ScheduleType = src.ScheduleType,
        //                      CreatedDate = src.CreatedDate,
        //                      EmailSubject = src.EmailSubject,
        //                      StartTime = src.StartTime,
        //                      StartDate = src.StartDate,
        //                      EndDate = src.EndDate,
        //                      MonthLastDay = src.MonthLastDay,
        //                      MonthScheduleDay = src.MonthScheduleDay,
        //                      RecurrenceType = src.RecurrenceType,
        //                      RunSunday = src.RunSunday,
        //                      RunMonday = src.RunMonday,
        //                      RunTuesday = src.RunTuesday,
        //                      RunWednesday = src.RunWednesday,
        //                      RunThursday = src.RunThursday,
        //                      RunFriday = src.RunFriday,
        //                      RunSaturday = src.RunSaturday
        //                  }).ToList();
        //    gvScheduledReports.DataSource = reuslt;
        //    gvScheduledReports.DataBind();

        //    if (reuslt.Count > 0)
        //    {
        //        pnlNoReports.Visible = false;
        //        pnlReportList.Visible = true;
        //    }
        //    else
        //    {
        //        pnlNoReports.Visible = true;
        //        pnlReportList.Visible = false;
        //    }
        //}

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {

        }

        private DayOfWeek GetActiveDayOfWeekFromScheduledReportsGridRow(GridViewRow gridRow)
        {
            var lblRunSunday = gridRow.FindControl(RunSundayLabelName) as Label;
            var lblRunMonday = gridRow.FindControl(RunMondayLabelName) as Label;
            var lblRunTuesday = gridRow.FindControl(RunTuesdayLabelName) as Label;
            var lblRunWednesday = gridRow.FindControl(RunWednesdayLabelName) as Label;
            var lblRunThursday = gridRow.FindControl(RunThursdayLabelName) as Label;
            var lblRunFriday = gridRow.FindControl(RunFridayLabelName) as Label;
            var lblRunSaturday = gridRow.FindControl(RunSaturdayLabelName) as Label;

            var runDay = DayOfWeek.Sunday;
            if (lblRunSaturday != null && lblRunSaturday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Saturday;
            }
            else if (lblRunFriday != null && lblRunFriday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Friday;
            }
            else if (lblRunThursday != null && lblRunThursday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Thursday;
            }
            else if (lblRunWednesday != null && lblRunWednesday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Wednesday;
            }
            else if (lblRunTuesday != null && lblRunTuesday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Tuesday;
            }
            else if (lblRunMonday != null && lblRunMonday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Monday;
            }
            else if (lblRunSunday != null && lblRunSunday.Text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                runDay = DayOfWeek.Sunday;
            }
            else
            {
                runDay = DateTime.Today.DayOfWeek;
            }

            return runDay;
        }

        private DateTime CalculateScheduledDate(DayOfWeek selectedDayOfWeek, DateTime referenceDate)
        {
            const int totalDaysInWeek = 7;

            var referenceDayOfWeek = referenceDate.DayOfWeek;
            var daysToBeAdded = selectedDayOfWeek - referenceDayOfWeek < 0
                ? selectedDayOfWeek - referenceDayOfWeek + totalDaysInWeek
                : selectedDayOfWeek - referenceDayOfWeek;

            var calculatedDate = referenceDate.AddDays(daysToBeAdded);

            return calculatedDate;
        }
    }
}