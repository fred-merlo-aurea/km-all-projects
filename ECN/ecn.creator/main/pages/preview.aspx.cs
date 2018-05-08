using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.pages {
	
	
	
	public partial class preview : System.Web.UI.Page {
		
		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;
		public static string communicatordb=ConfigurationManager.AppSettings["communicatordb"];
		public static string body				= "";
		public static string Keywords			= "";
		public static string JavaScriptCode	= "";
		public static string StyleSheet		= "";
		public static string pageProps			= "";
		public static string pageTitle			= "";
	
		protected void Page_Load(object sender, System.EventArgs e) {

            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            if (KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
				int requestPageID = getPageID();
				string requestFormat = getFormat();
				if (requestPageID>0) {
                    ECN_Framework.Common.SecurityAccess.canI("Pages",requestPageID.ToString());
					if (requestFormat=="html"){
						ShowPreview(requestPageID);
					}else{
						ShowTextPreview(requestPageID);
					}
				} else {
                    lblPreview.Text = "No PageID Specified";
				}
			}else{
				Response.Redirect("../default.aspx");				
			}
		}

		private int getPageID() {
			int thePageID = 0;
			try {
				thePageID = Convert.ToInt32(Request.QueryString["PageID"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return thePageID;
		}

		private string getFormat() {
			string theFormat = "html";
			try {
				theFormat = Request.QueryString["Format"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theFormat;
		}

		private void ShowPreview(int setPageID){
			string TemplateSource	= "";
			string HeaderCode		= "";
			string FooterCode			= "";
			string QueryValue			= "";
			int Slot1=0;
			int Slot2=0;
			int Slot3=0;
			int Slot4=0;
			int Slot5=0;
			int Slot6=0;
			int Slot7=0;
			int Slot8=0;
			int Slot9=0;

            string sqlQuery =
                " select * from Page p, " + communicatordb + ".dbo.template t, HeaderFooters hf" +
                " where p.PageID=" + setPageID + " " +
                " and p.templateID=t.templateID " +
                " and p.HeaderFooterID = hf.HeaderFooterID " +
                " and p.customerID = " + es.CurrentCustomer.CustomerID.ToString();

			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				TemplateSource	= dr["TemplateSource"].ToString();
				Slot1					= (int)dr["ContentSlot1"];
				Slot2					= (int)dr["ContentSlot2"];
				Slot3					= (int)dr["ContentSlot3"];
				Slot4					= (int)dr["ContentSlot4"];
				Slot5					= (int)dr["ContentSlot5"];
				Slot6					= (int)dr["ContentSlot6"];
				Slot7					= (int)dr["ContentSlot7"];
				Slot8					= (int)dr["ContentSlot8"];
				Slot9					= (int)dr["ContentSlot9"];
				HeaderCode			= dr["HeaderCode"].ToString();
				FooterCode			= dr["FooterCode"].ToString();
				Keywords			= dr["Keywords"].ToString();
				StyleSheet			= dr["StyleSheet"].ToString();
				JavaScriptCode	= dr["JavaScriptCode"].ToString();
				QueryValue			= dr["QueryValue"].ToString();
				pageTitle			= dr["PageName"].ToString();
				pageProps			= dr["PageProperties"].ToString();
			}

			body =TemplateFunctions.HTMLPagePreview(QueryValue, HeaderCode, FooterCode, TemplateSource, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9, es.CurrentCustomer.CustomerID);
            lblPreview.Text = body;
		}

		private void ShowTextPreview(int setPageID){
			string body="";
			string TemplateText="";
			int Slot1=0;
			int Slot2=0;
			int Slot3=0;
			int Slot4=0;
			int Slot5=0;
			int Slot6=0;
			int Slot7=0;
			int Slot8=0;
			int Slot9=0;

			string sqlQuery=
				" select * from Page p, "+communicatordb+".dbo.template t "+
				" where p.PageID="+setPageID+" "+
				" and l.templateID=t.templateID ";
			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				TemplateText=dr["TemplateText"].ToString();
				Slot1=(int)dr["ContentSlot1"];
				Slot2=(int)dr["ContentSlot2"];
				Slot3=(int)dr["ContentSlot3"];
				Slot4=(int)dr["ContentSlot4"];
				Slot5=(int)dr["ContentSlot5"];
				Slot6=(int)dr["ContentSlot6"];
				Slot7=(int)dr["ContentSlot7"];
				Slot8=(int)dr["ContentSlot8"];
				Slot9=(int)dr["ContentSlot9"];
			}

			body=TemplateFunctions.EmailTextBody(TemplateText,Slot1,Slot2,Slot3,Slot4,Slot5,Slot6,Slot7,Slot8,Slot9);
            lblPreview.Text = "<form><textarea cols=80 rows=25 READONLY>" + body + "</textarea></form>";
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
