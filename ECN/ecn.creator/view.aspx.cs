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
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator {
	
	
	
	public partial class view : System.Web.UI.Page {
		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
	
		protected void Page_Load(object sender, System.EventArgs e) {
			string requestQuery = getQuery();
			if (requestQuery!="") {
				ShowPreview(requestQuery);
			} else {
				LabelPreview.Text="No Query Specified";
			}
		}

		private string getQuery() {
			string theQuery = "";
			try {
				theQuery = Request.QueryString["BlastID"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theQuery;
		}

		private void ShowPreview(string setQuery){
			string body="";

			string sqlQuery=
				" SELECT p.PageName, p.SourceCode AS PageCode, t.SourceCode AS TemplateCode, t.HeaderCode "+
				" FROM Page p, Template t "+
				" WHERE p.QueryValue='"+setQuery+"' "+
				" AND p.TemplateID = t.TemplateID ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			if (dt.Rows.Count==0) {
				body="Associated Page or Template has been removed.";
				LabelPreview.Text=body;
			} else {
				string PageName="";
				string PageCode="";
				string TemplateCode="";
				string HeaderCode="";
                string ContextPath = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"];
				foreach ( DataRow dr in dt.Rows ) {
					PageName=dr["PageName"].ToString();
					PageCode=dr["PageCode"].ToString();
					TemplateCode=dr["TemplateCode"].ToString();
					HeaderCode=dr["HeaderCode"].ToString();
				}
				body=body+HeaderCode+TemplateCode;
				body=StringFunctions.Replace(body,"%%content%%",PageCode);
				body=StringFunctions.Replace(body,"%%contextpath%%",ContextPath);
				body=StringFunctions.Replace(body,"%%breadcrumb%%",TemplateFunctions.BreadCrumb(setQuery,ContextPath));
				body=StringFunctions.Replace(body,"%%menu%%",TemplateFunctions.MenuList(setQuery,ContextPath,Convert.ToInt32(sc.CustomerID())));
				body=StringFunctions.Replace(body,"%%eventslist%%",TemplateFunctions.EventsList(setQuery));
				body=StringFunctions.Replace(body,"%%newslist%%",TemplateFunctions.NewsList(setQuery));
				body=StringFunctions.Replace(body,"%%prlist%%",TemplateFunctions.prlist(setQuery));
				LabelPreview.Text=body;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
		#endregion
	}
}
