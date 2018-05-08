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
using CrystalDecisions.Shared; 
using CrystalDecisions.CrystalReports.Engine;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ecn.collector.main.report
{	
	public partial class ViewTextResponse : System.Web.UI.Page
    {
        KMPlatform.Entity.User CurrentUser = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);		
	
		private int QuestionID 
		{
			get 
			{
				try{return Convert.ToInt32(Request.QueryString["QID"]);}
				catch{return 0;}
			}
		}

        private int OtherText
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["Other"]); }
                catch { return 0; }
            }
        }

		private string Filterstr
		{
			get 
			{
				try{return Request.QueryString["Filterstr"].ToString();}
				catch{return string.Empty;}
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
				LoadResponseGrid();
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

		private void LoadResponseGrid()
		{
			if (QuestionID > 0)
			{
                ECN_Framework_Entities.Collector.Question q = ECN_Framework_BusinessLayer.Collector.Question.GetByQuestionID(QuestionID);
				lblQuestion.Text =q.QuestionText;
                DataTable dtResponses = ECN_Framework_BusinessLayer.Collector.Question.GetTextResponses(QuestionID, OtherText== 0 ? false : true, Filterstr);
				dgResponses.DataSource = dtResponses;
				dgResponses.DataBind();
				ResponsesPager.RecordCount = dtResponses.Rows.Count;
				ResponsesPager.Visible = true;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
            this.Unload += new EventHandler(Page_Unload);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    
			this.lnkToPDF.Click += new System.Web.UI.ImageClickEventHandler(this.lnkToPDF_Click);
			this.lnktoExl.Click += new System.Web.UI.ImageClickEventHandler(this.lnktoExl_Click);

		}
		#endregion

		protected void ResponsesPager_IndexChanged(object sender, System.EventArgs e)
		{
			LoadResponseGrid();
		}

		private void lnkToPDF_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			GenerateReport(CRExportEnum.PDF);
		}

		private void lnktoExl_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			GenerateReport(CRExportEnum.XLS);
		}

		private void GenerateReport(CRExportEnum ExportFormat)
		{
            ECN_Framework_Entities.Collector.Question q = ECN_Framework_BusinessLayer.Collector.Question.GetByQuestionID(QuestionID);
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(q.SurveyID, CurrentUser);

			Hashtable cParams = new Hashtable();
			cParams.Add("@QuestionID", QuestionID);
			cParams.Add("@filterstr",Filterstr);
            cParams.Add("@title", s.SurveyTitle);
			cParams.Add("@question",lblQuestion.Text);
            cParams.Add("@OtherText", OtherText);
			report = CRReport.GetReport(Server.MapPath("rpt_SurveyTextResponse.rpt"), cParams);
			crv.ReportSource = report;
			crv.Visible = true;
			CRReport.Export(report, ExportFormat, "SurveyTextResponse." + ExportFormat.ToString());
		}
	}
}
