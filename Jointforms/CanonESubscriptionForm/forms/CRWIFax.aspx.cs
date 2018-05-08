using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ecn.common.classes;

using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine; 


namespace CanonESubscriptionForm.forms
{
    public partial class CRWIFax : System.Web.UI.Page
    {
        ReportDocument report = new ReportDocument();
        protected void Page_Unload(object sender, System.EventArgs e)
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }

        private int emailID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["e"]); }
                catch { return 0; }
            }
        }

        private int groupID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["g"]); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable cParams = new Hashtable();
            cParams.Add("@emailID", emailID);
            cParams.Add("@groupID", groupID);

            report = CRReport.GetReport(Server.MapPath("CRWI.rpt"), cParams);
            crv.ReportSource = report;
            crv.Visible = true;

            CRReport.Export(report, CRExportEnum.PDF,"CRW_Fax.PDF");
        }
    }
}
