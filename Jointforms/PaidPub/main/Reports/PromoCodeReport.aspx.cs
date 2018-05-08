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
    public partial class PromoCodeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

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

                DataTable dtPromos = DataFunctions.GetDataTable("select PromotionID, Name + '(' + Code + ')' as DisplayText from ecn_misc..canon_paidpub_promotions where customerID = " + Session["CustomerID"].ToString() + " order by DisplayText", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
                lstPromoCodes.DataSource = dtPromos;
                lstPromoCodes.DataTextField = "DisplayText";
                lstPromoCodes.DataValueField = "PromotionID";
                lstPromoCodes.DataBind();
                lstPromoCodes.Items.Insert(0, new ListItem("ALL", ""));
            }
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
            cParams.Add("@startdate", txtstartDate.Text);
            cParams.Add("@enddate", txtendDate.Text);
            cParams.Add("@PromoIDs", getListboxValues(lstPromoCodes));

            report = CRReport.GetReport(Server.MapPath("crystalreports/promocode.rpt"), cParams);
            crv.ReportSource = report;
            crv.Visible = true;

            if (exportFormat.ToUpper() != "HTML")
            {
                CRReport.Export(report, (CRExportEnum)Enum.Parse(typeof(CRExportEnum), exportFormat.ToUpper()), "promocode." + exportFormat);
            }
        }

        private string getListboxValues(ListBox lst)
        {
            string selectedvalues = string.Empty;
            foreach (ListItem item in lst.Items)
            {
                if (item.Selected)
                {
                    selectedvalues += selectedvalues == string.Empty ? item.Value : "," + item.Value;
                }
            }
            return selectedvalues;
        }
    }
}
