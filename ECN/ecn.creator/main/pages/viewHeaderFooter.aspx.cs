using System;
using System.Data;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.pages {
	
	
	
	public partial class viewHeaderFooter : System.Web.UI.Page {
		protected System.Web.UI.WebControls.Label LabelPreview;
		//ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;
		public static string body				= "";
		public static string keywords			= "";
		public static string StyleSheet		= "";
		public static string javaScriptCode	= "";
		public static string headerFooterTitle	= "";

        protected void Page_Load(object sender, System.EventArgs e)
        {

            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            if (KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                int requestHFID = getHFID();
                if (requestHFID > 0)
                {
                    ECN_Framework.Common.SecurityAccess.canI("HeaderFooters", requestHFID.ToString());
                    ShowPreview(requestHFID);
                }
                else
                {
                    LabelPreview.Text = "No Header Footer Specified";
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

		private int getHFID() {
			int theHFID = 0;
			try {
				theHFID = Convert.ToInt32(Request.QueryString["HFID"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theHFID;
		}

		private void ShowPreview(int setHFID){
            int customerID = es.CurrentCustomer.CustomerID;
			string HeaderCode		= "";
			string FooterCode			= "";

			string sqlQuery=
				" SELECT * from HeaderFooters hf"+
				" WHERE HeaderFooterID = "+setHFID +
                " AND customerID = " + customerID.ToString();

			DataTable dt = DataFunctions.GetDataTable(sqlQuery);
			foreach ( DataRow dr in dt.Rows ) {
				HeaderCode			= dr["HeaderCode"].ToString();
				FooterCode			= dr["FooterCode"].ToString();
				keywords			= dr["keywords"].ToString();
				StyleSheet			= dr["StyleSheet"].ToString();
				javaScriptCode	= dr["JavaScriptCode"].ToString();
				headerFooterTitle	= dr["HeaderFooterName"].ToString();
			}

            body = TemplateFunctions.HeaderFooterPreview(HeaderCode, FooterCode, customerID);
			//LabelPreview.Text=body;
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
