using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace KMPS.MD.Controls
{
    public partial class DataCompareSummary : BaseControl
    {
        public Delegate hideDataCompareSummaryPopup;
        
        public int TotalRecords
        {
            get
            {
                try
                {
                    return (int)ViewState["TotalRecords"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["TotalRecords"] = value;
            }
        }

        public int MatchedNonMatchedRecords
        {
            get
            {
                try
                {
                    return (int)ViewState["MatchedNonMatchedRecords"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["MatchedNonMatchedRecords"] = value;
            }
        }

        public string ProcessCode
        {
            get
            {
                try
                {
                    return (string)ViewState["ProcessCode"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ProcessCode"] = value;
            }
        }

        public int DcTargetCodeID
        {
            get
            {
                try
                {
                    return (int)ViewState["DcTargetCodeID"];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["DcTargetCodeID"] = value;
            }
        }

        public string MatchType
        {
            get
            {
                try
                {
                    return (string)ViewState["MatchType"];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["MatchType"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.mpeSummary.Show();
            divError.Visible = false;
            lblErrorMessage.Text = "";

            if(!IsPostBack)
            {
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (PubID > 0)
                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareSummary(new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), ProcessCode, DcTargetCodeID, MatchType, PubID);
            else if (BrandID > 0)
                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareSummary(new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), ProcessCode, DcTargetCodeID, MatchType, BrandID);
            else
                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareSummary(new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), ProcessCode, DcTargetCodeID, MatchType, 0);

            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("dc_DataCompareSummary", dt);

            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);

            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/AudienceViews/reports/" + "rpt_DataCompareSummary.rdlc");
            ReportParameter[] parameters = new ReportParameter[2];
            parameters[0] = new ReportParameter("TotalRecords", TotalRecords.ToString());
            parameters[1] = new ReportParameter("MatchedNonMatchedRecords", MatchedNonMatchedRecords > 0 ? MatchedNonMatchedRecords.ToString() : MatchedNonMatchedRecords.ToString());
            ReportViewer1.LocalReport.SetParameters(parameters);

            ReportViewer1.LocalReport.Refresh();

            Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;

            bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            Response.ContentType = "application/pdf";

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DataCompareSummary.pdf");
            Response.BinaryWrite(bytes);
            Response.End();
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            hideDataCompareSummaryPopup.DynamicInvoke();
            this.mpeSummary.Hide();
        }
        
        public void loadControls()
        {
            gvDataCompareSummary.DataSource = null;
            gvDataCompareSummary.DataBind();

            lblTotalRecords.Text = TotalRecords.ToString();

            if (MatchedNonMatchedRecords > 0)
                lblMatchedNonMatchedRecords.Text = MatchedNonMatchedRecords.ToString();
            else
                lblMatchedNonMatchedRecords.Text = MatchedNonMatchedRecords.ToString();

            if (TotalRecords > 0)
                lblPercentage.Text = ((int)Math.Round((double)(100 * MatchedNonMatchedRecords) / TotalRecords)).ToString() + "%";
            else
                lblPercentage.Text = "0%";

            DataTable dt = new DataTable();

            if (PubID > 0)
                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareSummary(new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), ProcessCode, DcTargetCodeID, MatchType, PubID);
            else if (BrandID > 0)
                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareSummary(new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), ProcessCode, DcTargetCodeID, MatchType, BrandID);
            else
                dt = new FrameworkUAD.BusinessLogic.DataCompareProfile().GetDataCompareSummary(new KMPlatform.Object.ClientConnections(UserSession.CurrentUser.CurrentClient), ProcessCode, DcTargetCodeID, MatchType, 0);

            gvDataCompareSummary.DataSource = dt;
            gvDataCompareSummary.DataBind();
        }
    }
}