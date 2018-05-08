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
    public partial class _bReport : System.Web.UI.Page
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
            // lstBusiness.ClearSelection();
            // lstIndustry.ClearSelection();
            lstGroups.ClearSelection();

            txtQFrom.Text = "";
            txtQTo.Text = "";
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            
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

            //if (exportFormat.ToUpper() != "HTML")
            //    cParams.Add("@Print", "1");
            //else
            //    cParams.Add("@Print", "0");

            report = CRReport.GetReport(Server.MapPath("rpt_3b.rpt"), cParams);
            crv.ReportSource = report;
            crv.Visible = true;

            if (exportFormat.ToUpper() != "HTML")
            {
                CRReport.Export(report, (CRExportEnum)Enum.Parse(typeof(CRExportEnum), exportFormat.ToUpper()), "Report3b." + exportFormat);
            }

        }
    }
}
