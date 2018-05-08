using System;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;
using System.Configuration;
using ECN_Framework.Accounts.Report;
using ECN_Framework.Common;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using Microsoft.Reporting.WebForms;

namespace ecn.accounts.main.reports
{
    public partial class TotalBlastsForDay : ReportPageBase
    {
        private ITotalBlastsForDayProxy _totalBlastsForDayProxy;

        public TotalBlastsForDay(
            ITotalBlastsForDayProxy totalBlastsForDayProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _totalBlastsForDayProxy = totalBlastsForDayProxy;
        }

        public TotalBlastsForDay()
        {
            _totalBlastsForDayProxy = new TotalBlastsForDayProxy();
        }

        protected override void InitializeTheme()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;
            Master.SubMenu = "total blasts for today";

            if (!IsPostBack)
            {
                if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    Response.Redirect("/ecn.accounts/main/default.aspx");
                }
                txtstartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
               
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate;
                DateTime.TryParse(txtstartDate.Text, out startDate);

                var blasts = _totalBlastsForDayProxy.GetReport(startDate);
                foreach(var blast in blasts)
                {
                    blast.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(
                        blast.EmailSubject);
                }

                var dataSource = new ReportDataSource("DS_TotalBlastsForDay", blasts);
                var fileLocation = ConfigurationManager.AppSettings["ReportPath"] + "rpt_TotalBlastsForDay.rdlc";
                var filePath = Server.MapPath(fileLocation);
                var outputType = drpExport.SelectedItem.Text.ToUpper();
                var outputFileName = string.Format("TotalBlastsForDay.{0}", drpExport.SelectedItem.Text);

                ReportViewer1.Visible = false;
                CreateReportResponse(ReportViewer1, filePath, dataSource, null, outputType, outputFileName);
            }
            catch (ECNException ecn)
            {
                setECNError(ecn);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "TotalBlastsForDay.btnReport_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
            }
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