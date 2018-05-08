using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ecn.common.classes;

using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine; 

namespace PaidPub.main.Reports
{
    public partial class EarnDefreport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYear();
                drpMonth.ClearSelection();

                drpMonth.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;

                DataTable dtNewsletter = DataFunctions.GetDataTable("select g.GroupID, g.Groupname from ecn_misc..CANON_PAIDPUB_eNewsLetters n join ecn5_communicator..groups g on n.groupID = g.groupID where n.customerID = " + Session["CustomerID"].ToString() + " order by groupname", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

                string groupIDs = string.Empty;
                foreach (DataRow dr in dtNewsletter.Rows)
                {
                    groupIDs += (groupIDs == string.Empty ? dr["groupID"].ToString() : "," + dr["groupID"].ToString());
                }


                drpNewsletters.DataSource = dtNewsletter;
                drpNewsletters.DataTextField = "GroupName";
                drpNewsletters.DataValueField = "GroupID";
                drpNewsletters.DataBind();
                drpNewsletters.Items.Insert(0, new ListItem("ALL", groupIDs));
                drpNewsletters.Items.Insert(0, new ListItem("-- Select Newsletter --", ""));

                //DateTime d = DateTime.Now.AddMonths(-4).AddDays(-1 * (DateTime.Now.Day-1));

                //for (int i = 0; i < 6; i++)
                //{
                //    drpMonth.Items.Insert(0, new ListItem(d.AddMonths(i).AddDays(-1).ToString("MMMM yyyy"), d.AddMonths(i).AddDays(-1).ToShortDateString()));
                //}
            }
        }

        private void LoadYear()
        {
            //Year list can be changed by changing the lower and upper 
            //limits of the For statement    
            for (int intYear = DateTime.Now.Year - 10; intYear <= DateTime.Now.Year + 10; intYear++)
            {
                drpYear.Items.Add(intYear.ToString());
            }

            //Make the current year selected item in the list
            drpYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
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
            RenderReport(drpExport.SelectedItem.Value);
        }

        private void RenderReport(string exportFormat)
        {

            Hashtable cParams = new Hashtable();
            cParams.Add("@groupID", drpNewsletters.SelectedItem.Value);
            cParams.Add("@groupName", drpNewsletters.SelectedItem.Text);
            cParams.Add("@month", drpMonth.SelectedItem.Value);
            cParams.Add("@year", drpYear.SelectedItem.Value);
            cParams.Add("@cachereport", 0);
            
            report = CRReport.GetReport(Server.MapPath("crystalreports/EarnDefr.rpt"), cParams);
            crv.ReportSource = report;
            crv.Visible = true;

            if (exportFormat.ToUpper() != "HTML")
            {
                CRReport.Export(report, (CRExportEnum)Enum.Parse(typeof(CRExportEnum), exportFormat.ToUpper()), "EarnDefr." + exportFormat);
            }
        }
    }
}
