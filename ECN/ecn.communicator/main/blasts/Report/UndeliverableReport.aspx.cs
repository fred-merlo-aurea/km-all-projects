using System;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WebForms;
using ECN_Framework_Entities.Activity.Report.Undeliverable;
using ECN_Framework.Consts;
using ECN_Framework.Common;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;

namespace ecn.communicator.main.blasts.reports
{
    public partial class UndeliverableReport : ReportPageBase
    {
        private IUndeliverableReportProxy _undeliverableReportProxy;

        public UndeliverableReport(
            IUndeliverableReportProxy undeliverableReportProxy,
            IReportContentGenerator reportContentGenerator)
            : base(reportContentGenerator)
        {
            _undeliverableReportProxy = undeliverableReportProxy;
        }

        public UndeliverableReport()
        {
            _undeliverableReportProxy = new UndeliverableReportProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "Undeliverable Report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.UndeliverableReport, KMPlatform.Enums.Access.Download))
                {
                    txtstartDate.Text = DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy");
                    txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    //txtstartDate.Attributes.Add("ReadOnly", "true");
                    //txtendDate.Attributes.Add("ReadOnly", "true");
                }
                else
                    throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        public enum UndeliverableType
        {
            All,
            HardBounces,
            SoftBounces,
            MailBoxFull,
            Unsubscribes
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                IList<IUndeliverable> undeliverableList = new List<IUndeliverable>();
                DateTime startDate = DateTime.Parse(txtstartDate.Text);
                DateTime endDate = DateTime.Parse(txtendDate.Text);
                var customerId = GetCurrentCustomerId();

                switch ((UndeliverableType)Enum.Parse(typeof(UndeliverableType), drpUndeliverableType.SelectedValue))
                {
                    case UndeliverableType.All:
                        undeliverableList = _undeliverableReportProxy.GetAll(startDate, endDate, customerId);
                        break;
                    case UndeliverableType.HardBounces:
                        undeliverableList = _undeliverableReportProxy.GetHardBounces(startDate, endDate, customerId);
                        break;
                    case UndeliverableType.MailBoxFull:
                        undeliverableList = _undeliverableReportProxy.GetMailBoxFull(startDate, endDate, customerId);
                        break;
                    case UndeliverableType.SoftBounces:
                        undeliverableList = _undeliverableReportProxy.GetSoftBounces(startDate, endDate, customerId);
                        break;
                    case UndeliverableType.Unsubscribes:
                        undeliverableList = _undeliverableReportProxy.GetUnsubscribes(startDate, endDate, customerId);
                        break;
                }

                foreach (var u in undeliverableList)
                {
                    u.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(u.EmailSubject);
                }


                if (string.Compare(drpExport.SelectedItem.Value, "XLSDATA", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportCSV<IUndeliverable>(undeliverableList.ToList(), Master.UserSession.CurrentUser.CustomerID + "-UndeliverableReport-" + drpUndeliverableType.SelectedItem.Text);
                }
                else if (string.Compare(drpExport.SelectedItem.Value, "XLS", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var dataSource = new ReportDataSource("DS_Undeliverable", undeliverableList);
                    var filePath = Server.MapPath("rpt_UndeliverableReport.rdlc");
                    var outputType = ReportConsts.OutputTypeXLS;
                    var outputFileName = "UnsubscribeReasonDetail.xls";
                    var parameters = new[]
                    {
                        new ReportParameter("StartDate", startDate.ToShortDateString()),
                        new ReportParameter("EndDate", endDate.ToShortDateString()),
                        new ReportParameter("CustomerID", customerId.ToString()),
                        new ReportParameter("UndeliverableType", drpUndeliverableType.SelectedValue),
                        new ReportParameter("CustomerName", GetCurrentCustomerName()),
                    };

                    ReportViewer1.Visible = true;
                    CreateReportResponse(ReportViewer1, filePath, dataSource, parameters, outputType, outputFileName);
                    Response.ContentType = ResponseConsts.ContentTypeOctetStream;
                }
            }
        }

        private int GetCurrentCustomerId()
        {
            int customerId = 0;

            if (Master != null &&
                Master.UserSession != null &&
                Master.UserSession.CurrentCustomer != null)
            {
                customerId = Master.UserSession.CurrentCustomer.CustomerID;
            }

            return customerId;
        }

        private string GetCurrentCustomerName()
        {
            if (Master != null &&
                Master.UserSession != null &&
                Master.UserSession.CurrentCustomer != null)
            {
                return Master.UserSession.CurrentCustomer.CustomerName;
            }

            return string.Empty;
        }

        protected void EndDateValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator endDateValidator = source as CustomValidator;
            DateTime startDate = DateTime.Parse(txtstartDate.Text);
            DateTime endDate = DateTime.Parse(txtendDate.Text);

            int numberOfDaysSelected = endDate.Subtract(startDate).Days;

            if (numberOfDaysSelected > 30)
            {
                txtendDate.Text = startDate.AddDays(30).ToString("MM/dd/yyyy");
                args.IsValid = false;
                endDateValidator.ErrorMessage = "30 day maximum.";
            }
            else
            {
                if (endDate < startDate)
                {
                    args.IsValid = false;
                    endDateValidator.ErrorMessage = "Invalid end date.";
                }
            }
        }
    }
}
