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

namespace PharmaLiveReporting.main
{
    public partial class EffortKey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGroups();
            }
        }


        private void LoadGroups()
        {
            lstGroups.DataSource = DataFunctions.GetDataTable("select groupID, convert(varchar,groupID) + ' - ' + groupName as groupname from groups where customerID = 2069 and groupname like '%Newsletter%' order by groupname");
            lstGroups.DataBind();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            RenderReport("HTML");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lstGroups.ClearSelection();

            txtQFrom.Text = "";
            txtQTo.Text = "";
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            RenderReport(drpExport.SelectedValue.ToString());
        }

        protected void btndownloaddetails_Click(object sender, EventArgs e)
        {

        }

        ReportDocument report;
        private void RenderReport(string exportFormat)
        {

            Hashtable cParams = new Hashtable();
            cParams.Add("@groupID", Convert.ToInt32(lstGroups.SelectedItem.Value));
            cParams.Add("@groupName", lstGroups.SelectedItem.Text);
            cParams.Add("@startdate", txtQFrom.Text);
            cParams.Add("@enddate", txtQTo.Text);

            report = CRReport.GetReport(Server.MapPath("rpt_EffortKey.rpt"), cParams);
            crv.ReportSource = report;
            crv.Visible = true;

            if (exportFormat.ToUpper() != "HTML")
            {
                CRReport.Export(report, (CRExportEnum)Enum.Parse(typeof(CRExportEnum), exportFormat.ToUpper()), "EffortKey." + exportFormat);
            }

        }
    }
}
