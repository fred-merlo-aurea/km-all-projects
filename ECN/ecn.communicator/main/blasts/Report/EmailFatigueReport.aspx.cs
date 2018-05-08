using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using ECN_Framework.Common;
using ECN_Framework.Consts;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Activity.Report;

namespace ecn.communicator.main.blasts.Report
{
    public partial class EmailFatigueReport : ReportPageBase
    {
        private const string DateRangeInvalidErrorMessage = "Date range must be less than or equal to 31 days to run this report between 8AM and 6PM CST on weekdays.";
        private const string ReportDataSourceName = "DS_EmailFatigueReport";
        private const string ReportDefinitionFile = "rpt_EmailFatigueReport.rdlc";
        private const string ReportOutputFileName = "EmailFatigueReport.xls";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "email fatigue report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";
            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailFatigueReport, KMPlatform.Enums.Access.View))
                {
                    int baseChannelID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
                    txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");

                    if(!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailFatigueReport, KMPlatform.Enums.Access.Download))
                    {
                        ddlExport.Items.Clear();
                        ddlExport.Items.Add(new ListItem() { Text = "HTML", Value = "html", Selected = true });
                    }
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();


            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (ValidToRun())
            {
                var emailFatiguePercents = new List<EmailFatigueReportPercent>();

                DateTime startDate;
                DateTime endDate;
                if (!TryParseDates(out startDate, out endDate))
                {
                    return;
                }

                int filterId;
                string filterField;
                if (ddlFilterField.SelectedIndex > 0)
                {
                    if (ddlFilterValue.SelectedIndex > 0)
                    {
                        filterField = ddlFilterField.SelectedValue;
                        filterId = Convert.ToInt32(ddlFilterValue.SelectedValue);
                    }
                    else
                    {
                        throwECNException("Please select a Filter Value.");
                        return;
                    }
                }
                else
                {
                    throwECNException("Please select a Filter Field.");
                    return;
                }

                var emailFatigue = ECN_Framework_BusinessLayer.Activity.Report.EmailFatigueReport.Get(
                    Master.UserSession.CurrentCustomer.CustomerID,
                    startDate,
                    endDate,
                    filterField,
                    filterId);

                var reportDataSource = GetReportDataSource(emailFatigue, emailFatiguePercents);
                GenerateReport(reportDataSource);
            }
            else
            {
                throwECNException(DateRangeInvalidErrorMessage);
            }
        }

        private void GenerateReport(ReportDataSource reportDataSource)
        {
            if (ddlExport.SelectedValue.Equals(ReportConsts.OutputTypeXLS, StringComparison.CurrentCultureIgnoreCase))
            {
                GenerateXlsReport(reportDataSource);
            }
            else
            {
                GenerateHtmlReport(reportDataSource);
            }
        }

        private ReportDataSource GetReportDataSource(
            List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> emailFatigue,
            List<EmailFatigueReportPercent> emailFatiguePercents)
        {
            ReportDataSource reportDataSource;
            if (ddlOutputType.SelectedValue == "Counts")
            {
                reportDataSource = new ReportDataSource(ReportDataSourceName, emailFatigue);
            }
            else
            {
                ProcessPercents(emailFatigue, emailFatiguePercents);
                reportDataSource = new ReportDataSource(ReportDataSourceName, emailFatiguePercents);
            }

            return reportDataSource;
        }

        private void GenerateHtmlReport(ReportDataSource dataSource)
        {
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dataSource);
            ReportViewer1.LocalReport.ReportPath = GetMappedReportDefinitionLocation(ReportDefinitionFile);
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.Visible = true;

            var parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("Format", ReportConsts.OutputTypeHTML);
            parameters[1] = new ReportParameter("StartDate", txtstartDate.Text);
            parameters[2] = new ReportParameter("EndDate", txtendDate.Text);
            parameters[3] = new ReportParameter("FilterField", ddlFilterField.SelectedValue);
            parameters[4] = new ReportParameter("FilterValue", ddlFilterValue.SelectedValue);
            parameters[5] = new ReportParameter("URL", Request.Url.ToString());
            
            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportViewer1.LocalReport.Refresh();

            Warning[] warnings;
            string[] streamids;
            String mimeType;
            String encoding;
            String extension;

            ReportViewer1.LocalReport.Render(
                ReportConsts.RenderFormatEXCEL,
                string.Empty,
                out mimeType,
                out encoding,
                out extension,
                out streamids,
                out warnings);
        }

        private void GenerateXlsReport(ReportDataSource dataSource)
        {
            var reportLocation = GetMappedReportDefinitionLocation(ReportDefinitionFile);
            var filePath = Server.MapPath(reportLocation);
            ReportViewer1.Visible = false;

            CreateReportResponse(ReportViewer1, filePath, dataSource, null, ReportConsts.OutputTypeXLS, ReportOutputFileName);
        }

        private string GetMappedReportDefinitionLocation(string reportDefinitionName)
        {
            var reportPath = ConfigurationManager.AppSettings["ReportPath"];
            return Server.MapPath(Path.Combine(reportPath, reportDefinitionName));
        }

        private bool TryParseDates(out DateTime startDate, out DateTime endDate)
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;

            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue || startDate > endDate)
            {
                throwECNException("Please select a valid date");
                return false;
            }

            return true;
        }

        private static void ProcessPercents(List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> emailfatigue, List<EmailFatigueReportPercent> efpList)
        {
            int level = 1;
            ECN_Framework_Entities.Activity.Report.EmailFatigueReportPercent efPercentL1 =
                new ECN_Framework_Entities.Activity.Report.EmailFatigueReportPercent();
            foreach (var email in emailfatigue)
            {
                if (level == 1)
                {
                    efPercentL1.Action = email.Action;
                    efPercentL1.Grouping = email.Grouping;
                    efPercentL1.Touch1 = email.Touch1.ToString();
                    efPercentL1.Touch2 = email.Touch2.ToString();
                    efPercentL1.Touch3 = email.Touch3.ToString();
                    efPercentL1.Touch4 = email.Touch4.ToString();
                    efPercentL1.Touch5 = email.Touch5.ToString();
                    efPercentL1.Touch6 = email.Touch6.ToString();
                    efPercentL1.Touch7 = email.Touch7.ToString();
                    efPercentL1.Touch8 = email.Touch8.ToString();
                    efPercentL1.Touch9 = email.Touch9.ToString();
                    efPercentL1.Touch10 = email.Touch10.ToString();
                    efPercentL1.Touch11_20 = email.Touch11_20.ToString();
                    efPercentL1.Touch21_30 = email.Touch21_30.ToString();
                    efPercentL1.Touch31_40 = email.Touch31_40.ToString();
                    efPercentL1.Touch41_50 = email.Touch41_50.ToString();
                    efPercentL1.Touch51Plus = email.Touch51Plus.ToString();
                    efpList.Add(efPercentL1);
                }
                else
                {
                    try
                    {
                        ECN_Framework_Entities.Activity.Report.EmailFatigueReportPercent efPercent =
                            new ECN_Framework_Entities.Activity.Report.EmailFatigueReportPercent
                            {
                                Action = email.Action,
                                Grouping = email.Grouping
                            };

                        if ((efPercent.Touch1 == null || efPercent.Touch1 == "0") &&
                            (efPercentL1.Touch1 == null || efPercentL1.Touch1 == "0"))
                        {
                            efPercent.Touch1 = "0%";
                        }
                        else
                        {
                            efPercent.Touch1 = Math.Round((Convert.ToDecimal(email.Touch1)*100/Convert.ToDecimal(efPercentL1.Touch1))).ToString() + "%";
                        }
                        if ((efPercent.Touch2 == null || efPercent.Touch2 == "0") &&
                            (efPercentL1.Touch2 == null || efPercentL1.Touch2 == "0"))
                        {
                            efPercent.Touch2 = "0%";
                        }
                        else
                        {
                            efPercent.Touch2 = Math.Round((Convert.ToDecimal(email.Touch2)*100/Convert.ToDecimal(efPercentL1.Touch2))).ToString() + "%";
                        }
                        if ((efPercent.Touch3 == null || efPercent.Touch3 == "0") &&
                            (efPercentL1.Touch3 == null || efPercentL1.Touch3 == "0"))
                        {
                            efPercent.Touch3 = "0%";
                        }
                        else
                        {
                            efPercent.Touch3 =  Math.Round((Convert.ToDecimal(email.Touch3)*100/Convert.ToDecimal(efPercentL1.Touch3))).ToString() + "%";
                        }
                        if ((efPercent.Touch4 == null || efPercent.Touch4 == "0") &&
                            (efPercentL1.Touch4 == null || efPercentL1.Touch4 == "0"))
                        {
                            efPercent.Touch4 = "0%";
                        }
                        else
                        {
                            efPercent.Touch4 = Math.Round((Convert.ToDecimal(email.Touch4)*100/Convert.ToDecimal(efPercentL1.Touch4))).ToString() + "%";
                        }
                        if ((efPercent.Touch5 == null || efPercent.Touch5 == "0") &&
                            (efPercentL1.Touch5 == null || efPercentL1.Touch5 == "0"))
                        {
                            efPercent.Touch5 = "0%";
                        }
                        else
                        {
                            efPercent.Touch5 = Math.Round((Convert.ToDecimal(email.Touch5)*100/Convert.ToDecimal(efPercentL1.Touch5))).ToString() + "%";
                        }
                        if ((efPercent.Touch6 == null || efPercent.Touch6 == "0") &&
                            (efPercentL1.Touch6 == null || efPercentL1.Touch6 == "0"))
                        {
                            efPercent.Touch6 = "0%";
                        }
                        else
                        {
                            efPercent.Touch6 = Math.Round((Convert.ToDecimal(email.Touch6)*100/Convert.ToDecimal(efPercentL1.Touch6))).ToString() + "%";
                        }
                        if ((efPercent.Touch7 == null || efPercent.Touch7 == "0") &&
                            (efPercentL1.Touch7 == null || efPercentL1.Touch7 == "0"))
                        {
                            efPercent.Touch7 = "0%";
                        }
                        else
                        {
                            efPercent.Touch7 = Math.Round((Convert.ToDecimal(email.Touch7)*100/Convert.ToDecimal(efPercentL1.Touch7))).ToString() + "%";
                        }
                        if ((efPercent.Touch8 == null || efPercent.Touch8 == "0") &&
                            (efPercentL1.Touch8 == null || efPercentL1.Touch8 == "0"))
                        {
                            efPercent.Touch8 = "0%";
                        }
                        else
                        {
                            efPercent.Touch8 = Math.Round((Convert.ToDecimal(email.Touch8)*100/Convert.ToDecimal(efPercentL1.Touch8))).ToString() + "%";
                        }
                        if ((efPercent.Touch9 == null || efPercent.Touch9 == "0") &&
                            (efPercentL1.Touch9 == null || efPercentL1.Touch9 == "0"))
                        {
                            efPercent.Touch9 = "0%";
                        }
                        else
                        {
                            efPercent.Touch9 = Math.Round((Convert.ToDecimal(email.Touch9)*100/Convert.ToDecimal(efPercentL1.Touch9))).ToString() + "%";
                        }
                        if ((efPercent.Touch10 == null || efPercent.Touch10 == "0") &&
                            (efPercentL1.Touch10 == null || efPercentL1.Touch10 == "0"))
                        {
                            efPercent.Touch10 = "0%";
                        }
                        else
                        {
                            efPercent.Touch10 = Math.Round((Convert.ToDecimal(email.Touch10)*100/Convert.ToDecimal(efPercentL1.Touch10))).ToString() + "%";
                        }
                        if ((efPercent.Touch11_20 == null || efPercent.Touch11_20 == "0") &&
                            (efPercentL1.Touch11_20 == null || efPercentL1.Touch11_20 == "0"))
                        {
                            efPercent.Touch11_20 = "0%";
                        }
                        else
                        {
                            efPercent.Touch11_20 = Math.Round((Convert.ToDecimal(email.Touch11_20)*100/Convert.ToDecimal(efPercentL1.Touch11_20))).ToString() + "%";
                        }
                        if ((efPercent.Touch21_30 == null || efPercent.Touch21_30 == "0") &&
                            (efPercentL1.Touch21_30 == null || efPercentL1.Touch21_30 == "0"))
                        {
                            efPercent.Touch21_30 = "0%";
                        }
                        else
                        {
                            efPercent.Touch21_30 = Math.Round((Convert.ToDecimal(email.Touch21_30)*100/Convert.ToDecimal(efPercentL1.Touch21_30))).ToString() + "%";
                        }
                        if ((efPercent.Touch31_40 == null || efPercent.Touch31_40 == "0") &&
                            (efPercentL1.Touch31_40 == null || efPercentL1.Touch31_40 == "0"))
                        {
                            efPercent.Touch31_40 = "0%";
                        }
                        else
                        {
                            efPercent.Touch31_40 = Math.Round((Convert.ToDecimal(email.Touch31_40)*100/Convert.ToDecimal(efPercentL1.Touch31_40))).ToString() + "%";
                        }
                        if ((efPercent.Touch41_50 == null || efPercent.Touch41_50 == "0") &&
                            (efPercentL1.Touch41_50 == null || efPercentL1.Touch41_50 == "0"))
                        {
                            efPercent.Touch41_50 = "0%";
                        }
                        else
                        {
                            efPercent.Touch41_50 = Math.Round((Convert.ToDecimal(email.Touch41_50)*100/Convert.ToDecimal(efPercentL1.Touch41_50))).ToString() + "%";
                        }
                        if ((efPercent.Touch51Plus == null || efPercent.Touch51Plus == "0") &&
                            (efPercentL1.Touch51Plus == null || efPercentL1.Touch51Plus == "0"))
                        {
                            efPercent.Touch51Plus = "0%";
                        }
                        else
                        {
                            efPercent.Touch51Plus = Math.Round((Convert.ToDecimal(email.Touch51Plus) * 100 / Convert.ToDecimal(efPercentL1.Touch51Plus))).ToString() + "%";
                        }
                        efpList.Add(efPercent);
                    }
                    catch (Exception ecException)
                    {
                    }
                }
                level++;
            }
        }

        protected void ddlFilterField_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                if (ddlFilterField.SelectedValue.Equals("-1"))
                {
                    ddlFilterValue.Items.Clear();
                }
                else
                {
                    if (ddlFilterField.SelectedValue.Equals("MessageTypeID"))
                    {
                        List<ECN_Framework_Entities.Communicator.MessageType> mtList = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID, Master.UserSession.CurrentUser).OrderBy(x => x.Name).ToList();
                        ddlFilterValue.DataSource = mtList;
                        ddlFilterValue.DataTextField = "Name";
                        ddlFilterValue.DataValueField = "MessageTypeID";
                        ddlFilterValue.DataBind();
                    }
                    else if (ddlFilterField.SelectedValue.Equals("GroupID"))
                    {
                        List<ECN_Framework_Entities.Communicator.Group> groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentCustomer.CustomerID).OrderBy(x => x.GroupName).ToList();
                        ddlFilterValue.DataSource = groupList;
                        ddlFilterValue.DataTextField = "GroupName";
                        ddlFilterValue.DataValueField = "GroupID";
                        ddlFilterValue.DataBind();
                    }
                    else if (ddlFilterField.SelectedValue.Equals("CampaignID"))
                    {
                        List<ECN_Framework_Entities.Communicator.Campaign> campaignList = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentCustomer.CustomerID, false).OrderBy(x => x.CampaignName).ToList();
                        ddlFilterValue.DataSource = campaignList;
                        ddlFilterValue.DataTextField = "CampaignName";
                        ddlFilterValue.DataValueField = "CampaignID";
                        ddlFilterValue.DataBind();
                    }

                    ddlFilterValue.Items.Insert(0, new ListItem() { Text = "-SELECT-", Value = "-1", Selected = true });
                }
            
        }

        
        private bool ValidToRun()
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);

            TimeSpan range = endDate - startDate;


            if (range.TotalDays >= 32)
            {
                if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
                else
                {
                    if (now.TimeOfDay.Hours >= 8 && now.TimeOfDay.Hours <= 18)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
                return true;
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
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

        
    }
}