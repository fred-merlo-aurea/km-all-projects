using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Reflection;
using ECN_Framework_Common.Objects;
using System.Linq;
using ECN_Framework.Common;
using ECN_Framework.Common.Interfaces;
using ECN_Framework.Consts;
using ECN_Framework_BusinessLayer.Activity.Report;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;

namespace ecn.communicator.main.blasts.reports
{
    public partial class BlastDeliveryReport : ReportPageBase
    {
        private const string ReportName = "BlastDelivery";

        private IBlastDeliveryProxy _blastDeliveryProxy;

        protected System.Web.UI.WebControls.Label MsgLabel;
        
        public BlastDeliveryReport(
            IBlastDeliveryProxy blastDeliveryProxy,
            IReportDefinitionProvider reportDefinitionProvider,
            IReportContentGenerator reportContentGenerator)
            : base(reportDefinitionProvider, reportContentGenerator)
        {
            _blastDeliveryProxy = blastDeliveryProxy;
        }

        public BlastDeliveryReport()
        {
            _blastDeliveryProxy = new BlastDeliveryProxy();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.REPORTS;
            Master.SubMenu = "delivery report";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            if (!IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastDeliveryReport, KMPlatform.Enums.Access.Download))
                {
                int baseChannelID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
                txtstartDate.Text = DateTime.Now.AddDays(-14).ToString("MM/dd/yyyy");
                txtendDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    KMPlatform.Entity.User user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                    if (!(KM.Platform.User.IsSystemAdministrator(user) || KM.Platform.User.IsChannelAdministrator(user)))
                {
                    lstboxCustomer.Visible = false;
                }
                else
                {
                    lstboxCustomer.Visible = true;
                    List<ECN_Framework_Entities.Accounts.Customer> lstCust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);
                    lstboxCustomer.DataSource = lstCust;
                    lstboxCustomer.DataTextField = "CustomerName";
                    lstboxCustomer.DataValueField = "CustomerID";
                    lstboxCustomer.DataBind();

                    foreach (ListItem li in lstboxCustomer.Items)
                    {
                        li.Selected = true;
                    }
                }
            }
                else
                {
                    throw new ECN_Framework_Common.Objects.SecurityException();
                }
        }
        }

        protected void Page_Unload(object sender, System.EventArgs e)
        {
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            DateTime testDate = new DateTime();
            if (DateTime.TryParse(txtstartDate.Text.Trim(), out testDate))
            {
                if (DateTime.TryParse(txtendDate.Text.Trim(), out testDate))
                {
                    RenderReport(drpExport.SelectedItem.Text);
                }
                else
                {
                    throwECNException("Invalid end date");
                    return;
                }
            }
            else
            {
                throwECNException("Invalid start date");
                return;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Blast, Enums.Method.GetList, message);
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

        private void RenderReport(string exportFormat)
        {
            DateTime startDate;
            DateTime endDate;
            DateTime.TryParse(txtstartDate.Text, out startDate);
            DateTime.TryParse(txtendDate.Text, out endDate);
            var customerIds = string.Empty;
            var isUnique = Convert.ToBoolean(drpIsUnique.SelectedItem.Value);

            if (lstboxCustomer.Visible == true && lstboxCustomer.SelectedItem != null)
            {
                foreach (ListItem li in lstboxCustomer.Items)
                {
                    if (li.Selected)
                    {
                        customerIds += customerIds == string.Empty ? li.Value : ", " + li.Value;
                    }
                }
            }
            else if (lstboxCustomer.Visible == false)
            {
                customerIds = Master.UserSession.CurrentUser.CustomerID.ToString();
            }

            var blastdelivery = _blastDeliveryProxy.Get(customerIds, startDate, endDate, isUnique).ToList();

            foreach (var blast in blastdelivery)
            {
                blast.Spam = (blast.Abuse + blast.Feedback);
                if (blast.Delivered != 0)
                {
                    double spam = blast.Spam;
                    double delivered = blast.Delivered;
                    double value = spam/delivered;
                    blast.SpamPercent = value.ToString("#0.##%");
                }
                blast.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(blast.EmailSubject);
            }

            if (drpExport.SelectedItem.Value.Equals(ReportConsts.OutputTypeXLSDATA, StringComparison.OrdinalIgnoreCase))
            {
                List<ECN_Framework_Entities.Activity.Report.BlastDelivery> newblastdelivery =
                    ECN_Framework_BusinessLayer.Activity.Report.BlastDelivery.GetReportdetails(blastdelivery);

                if (newblastdelivery != null)
                {
                    string tfile = Master.UserSession.CurrentUser.CustomerID + "-BlastDelivery";

                    if (drpExport.SelectedItem.Value.ToUpper() == "XLSDATA")
                    {
                        ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportCSV(newblastdelivery, tfile);
                    }

                    if (drpExport.SelectedItem.Value.ToUpper() == "XLS")
                    {
                        List<string> fakeList = null;
                        ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToTab(ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.GetTabDelimited<ECN_Framework_Entities.Activity.Report.BlastDelivery, string>(newblastdelivery, fakeList), tfile);
                    }
                }
            }
            else
            {
                var dataSource = new ReportDataSource("DS_BlastDelivery", blastdelivery);
                var parameters = new[]
                {
                    new ReportParameter("StartDate", txtstartDate.Text),
                    new ReportParameter("EndDate", txtendDate.Text),
                    new ReportParameter("Type", drpExport.SelectedItem.Value.ToUpper())
                };

                var outputType = drpExport.SelectedItem.Text.ToUpper();
                var outputFileName = string.Format("{0}.{1}", ReportName, drpExport.SelectedItem.Text);
                var stream = GetReportDefinitionStream();
                ReportViewer1.Visible = false;
                CreateReportResponse(ReportViewer1, stream, dataSource, parameters, outputType, outputFileName);
            }
        }

        private Stream GetReportDefinitionStream()
        {
            var assemblyLocation = HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll");
            var reportName = "ECN_Framework_Common.Reports.rpt_BlastDelivery.rdlc";
            var reportDefinitionProvider = GetReportDefinitionProvider();

            return reportDefinitionProvider.GetReportDefinitionStream(assemblyLocation, reportName);
        }
    }
}
