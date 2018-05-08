using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ecn.common.classes;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace ecn.accounts.main.reports
{
    public partial class IR_KMLogoclick : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;
        }

        ReportDocument report = new ReportDocument();
        protected void Page_Unload(object sender, System.EventArgs e) 
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            Hashtable cParams = new Hashtable();

            cParams.Add("@fromdt", txtstartDate.Text);
            cParams.Add("@todt", txtendDate.Text);
            cParams.Add("@showdetails", chkShowDetails.Checked ? "1" : "0");

            report = CRReport.GetReport(Server.MapPath("crystalreport/rpt_IRKMLogoClick.rpt"), cParams);
            crv.ReportSource = report;
            crv.Visible = false;
            CRReport.Export(report, (CRExportEnum)Convert.ToInt32(drpExport.SelectedItem.Value), "IR_report." + drpExport.SelectedItem.Text);


        }
    }
}
