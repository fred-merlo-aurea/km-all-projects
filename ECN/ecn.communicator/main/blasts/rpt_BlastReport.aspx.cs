using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Reflection;
using CrystalDecisions.Shared; 
using CrystalDecisions.CrystalReports.Engine; 
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

namespace ecn.communicator.main.blasts
{
	public partial class rpt_BlastReport : System.Web.UI.Page
	{
		private int getBlastID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["BlastID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv))
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.BlastReport, KMPlatform.Enums.Access.View))				
            {
				int requestBlastID = getBlastID();
				if (requestBlastID>0) 
				{
                    RenderReport();
				} 
			}
			else
			{
				Response.Redirect("default.aspx");			
			}
		}

        protected void Page_Unload(object sender, System.EventArgs e)
        {
        }

		private void RenderReport()
		{
            List<ECN_Framework_Entities.Activity.Report.BlastReport> listReport = ECN_Framework_BusinessLayer.Activity.Report.BlastReport.Get(getBlastID());
            foreach(var r in listReport)
            {
                if(r != null)
                    r.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(r.EmailSubject);
            }

            Microsoft.Reporting.WebForms.ReportDataSource rdsBlastReport = new Microsoft.Reporting.WebForms.ReportDataSource("DS_BlastReport", listReport);
            Microsoft.Reporting.WebForms.ReportDataSource rdsBlastReportDetail = new Microsoft.Reporting.WebForms.ReportDataSource("DS_BlastReportDetail", ECN_Framework_BusinessLayer.Activity.Report.BlastReportDetail.Get(getBlastID()));
            
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rdsBlastReport);
            ReportViewer1.LocalReport.DataSources.Add(rdsBlastReportDetail);
            System.Reflection.Assembly assembly = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("~/bin/ECN_Framework_Common.dll"));
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom("ECN_Framework_Common.dll");
            System.IO.Stream stream = assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastPDFReport.rdlc");
            ReportViewer1.LocalReport.LoadReportDefinition(stream);
		    
            System.IO.Stream streamSubRpt = assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastPerformance.rdlc");
            ReportViewer1.LocalReport.LoadSubreportDefinition("rpt_BlastPerformance", streamSubRpt);
            streamSubRpt = assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastResponseDetail.rdlc");
            ReportViewer1.LocalReport.LoadSubreportDefinition("rpt_BlastResponseDetail", streamSubRpt);
            streamSubRpt = assembly.GetManifestResourceStream("ECN_Framework_Common.Reports.rpt_BlastClickReport.rdlc");
            ReportViewer1.LocalReport.LoadSubreportDefinition("rpt_BlastClickReport", streamSubRpt);

            //ReportViewer1.LocalReport.Refresh();
            ReportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(this.ReportViewer1_SubReport);
            ReportViewer1.LocalReport.Refresh();
		  
            Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;

            string exportType = Request.QueryString["ExportType"] ?? string.Empty;
		    switch (exportType.ToUpper())
		    {
                case "EXCEL":
                    bytes = ReportViewer1.LocalReport.Render("EXCEL", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment; filename=BlastOverview_" + getBlastID() + ".xls");
		            break;
                default:
                    bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=BlastOverview_" + getBlastID() + ".PDF");
		            break;
		    }

            Response.Clear();
            Response.Buffer = true;
            Response.BinaryWrite(bytes);
            Response.End();
		}

        private void ReportViewer1_SubReport(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "rpt_BlastPerformance")
            {
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DS_BlastReportPerformance", ECN_Framework_BusinessLayer.Activity.Report.BlastReportPerformance.Get(getBlastID())));
            }
            else if (e.ReportPath == "rpt_BlastResponseDetail")
            {
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DS_BlastResponseDetail", ECN_Framework_BusinessLayer.Activity.Report.BlastResponseDetail.Get(getBlastID())));
            }
            else
            {
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DS_BlastClickReport", ECN_Framework_BusinessLayer.Activity.Report.BlastClickReport.Get(getBlastID())));
            }
        }
	}
}
