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

namespace PharmaLiveReporting
{
    public partial class _aReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadBusiness();
                //LoadIndustry();
                LoadGroups();
            }
        }

        private void LoadBusiness()
        {
            DataTable dtBusiness = new DataTable();

            DataColumn dc;

            dc = new DataColumn("Text");
            dtBusiness.Columns.Add(dc);

            dc = new DataColumn("Value");
            dtBusiness.Columns.Add(dc);

            AddRow(dtBusiness, "01", "Pharmaceutical manufacturer");
            AddRow(dtBusiness, "11", "Biotechnology company");
            AddRow(dtBusiness, "15", "Generic pharmaceutical manufacturer");
            AddRow(dtBusiness, "09", "Medical equipment manufacturer");
            AddRow(dtBusiness, "12", "Contract research organization");
            AddRow(dtBusiness, "13", "Clinical study site/SMO");
            AddRow(dtBusiness, "14", "Clinical lab");
            AddRow(dtBusiness, "03", "Healthcare communications company");
            AddRow(dtBusiness, "04", "Marketing services company");
            AddRow(dtBusiness, "05", "General business services company");
            AddRow(dtBusiness, "06", "Hospital");
            AddRow(dtBusiness, "07", "Academic/University research institution");
            AddRow(dtBusiness, "08", "Government agency");
            AddRow(dtBusiness, "10", "Data management company");
            AddRow(dtBusiness, "99", "Other support or service");

            lstBusiness.DataSource = dtBusiness;
            lstBusiness.DataBind();

        }

        private void LoadIndustry()
        {
            DataTable dtIndustry = new DataTable();

            DataColumn dc;

            dc = new DataColumn("Text");
            dtIndustry.Columns.Add(dc);

            dc = new DataColumn("Value");
            dtIndustry.Columns.Add(dc);

            AddRow(dtIndustry, "09", "R&D management");
            AddRow(dtIndustry, "10", "Senior management");
            AddRow(dtIndustry, "34", "Finance management");
            AddRow(dtIndustry, "38", "Business strategy");
            AddRow(dtIndustry, "12", "Product management");
            AddRow(dtIndustry, "23", "Marketing,advertising or promotion management");
            AddRow(dtIndustry, "14", "Sales management");
            AddRow(dtIndustry, "15", "Agency account management");
            AddRow(dtIndustry, "19", "Marketing services");
            AddRow(dtIndustry, "20", "Media management (incl. directors/planners)");
            AddRow(dtIndustry, "24", "Medical director/associate medical director");
            AddRow(dtIndustry, "25", "Clinical trials management");
            AddRow(dtIndustry, "26", "Clinical/drug research");
            AddRow(dtIndustry, "28", "Clinical monitoring/CRC/CRA");
            AddRow(dtIndustry, "31", "Clinical documentation preparation");
            AddRow(dtIndustry, "27", "Regulatory affairs");
            AddRow(dtIndustry, "22", "Quality control");
            AddRow(dtIndustry, "33", "Drug safety");
            AddRow(dtIndustry, "32", "Project management");
            AddRow(dtIndustry, "29", "Academic research or professor");
            AddRow(dtIndustry, "30", "Data management or analysis");
            AddRow(dtIndustry, "35", "Licensing");
            AddRow(dtIndustry, "36", "Manufacturing");
            AddRow(dtIndustry, "37", "IT management");
            AddRow(dtIndustry, "99", "Other functions");

            lstIndustry.DataSource = dtIndustry;
            lstIndustry.DataBind();

        }

        private void LoadGroups()
        {
            lstGroups.DataSource = DataFunctions.GetDataTable("select groupID, convert(varchar,groupID) + ' - ' + groupName as groupname from groups where customerID = 2069 and groupname like '%Newsletter%' order by groupname");
            lstGroups.DataBind();
        }

        private void AddRow (DataTable dt, string value, string text)
        {
            DataRow row;

            row = dt.NewRow();
            row["Text"] = value + " - " + text;
            row["Value"] = value;
            dt.Rows.Add(row);
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
                cParams.Add("@groupName",lstGroups.SelectedItem.Text);
                cParams.Add("@startdate", txtQFrom.Text);
                cParams.Add("@enddate", txtQTo.Text);

                //if (exportFormat.ToUpper() != "HTML")
                //    cParams.Add("@Print", "1");
                //else
                //    cParams.Add("@Print", "0");

                report = CRReport.GetReport(Server.MapPath("rpt_3a_1.rpt"), cParams);
                crv.ReportSource = report;
                crv.Visible = true;

                if (exportFormat.ToUpper() != "HTML")
                {
                    CRReport.Export(report, (CRExportEnum)Enum.Parse(typeof(CRExportEnum), exportFormat.ToUpper()), "Report3a." + exportFormat);
                }
           
        }
    }
}
