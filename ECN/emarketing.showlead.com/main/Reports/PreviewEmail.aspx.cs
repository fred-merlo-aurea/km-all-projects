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

using ecn.common.classes;

namespace ecn.showcare.wizard.main.Reports
{
	/// <summary>
	/// Summary description for PreviewEmail.
	/// </summary>
	public class PreviewEmail : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblPreview;
	
		private void ShowPreview() {
			// Get BlastID
			string bid = "";
			try {
				bid = Request.QueryString["ID"];
			} catch (Exception err) {
				lblPreview.Text = err.Message;
			}

			if (!bid.Equals(null)) {
				string EmailSubject = "";
				string EmailFrom = "";
				string TableOptions = "";
				string TemplateSource = "";
				int Slot1 = 0;
				int Slot2 = 0;
				int Slot3 = 0;
				int Slot4 = 0;
				int Slot5 = 0;
				int Slot6 = 0;
				int Slot7 = 0;
				int Slot8 = 0;
				int Slot9 = 0;

				string sql = "SELECT b.EmailSubject, b.EmailFrom, l.TableOptions, l.ContentSlot1, l.ContentSlot2, l.ContentSlot3, l.ContentSlot4, "+
					"l.ContentSlot5, l.ContentSlot6, l.ContentSlot7, l.ContentSlot8, l.ContentSlot9, t.TemplateSource "+
					"FROM Blasts b, Layouts l, Templates t "+
					"WHERE b.BlastID="+bid+" AND b.LayoutID=l.LayoutID AND l.TemplateID=t.TemplateID";

				DataTable dt = null;
				try {
					dt = DataFunctions.GetDataTable(sql);
				} catch (Exception err){
					lblPreview.Text = err.Message;
				}

				if ((dt==null) || (dt.Rows.Count < 1)) {
					lblPreview.Text = "Associated Message or Template has been removed.";
				} else {
					EmailSubject = dt.Rows[0]["EmailSubject"].ToString();
					EmailFrom = dt.Rows[0]["EmailFrom"].ToString();
					TemplateSource = dt.Rows[0]["TemplateSource"].ToString();
					TableOptions = dt.Rows[0]["TableOptions"].ToString();
					Slot1 = Convert.ToInt32(dt.Rows[0]["ContentSlot1"]);
					Slot2 = Convert.ToInt32(dt.Rows[0]["ContentSlot2"]);
					Slot3 = Convert.ToInt32(dt.Rows[0]["ContentSlot3"]);
					Slot4 = Convert.ToInt32(dt.Rows[0]["ContentSlot4"]);
					Slot5 = Convert.ToInt32(dt.Rows[0]["ContentSlot5"]);
					Slot6 = Convert.ToInt32(dt.Rows[0]["ContentSlot6"]);
					Slot7 = Convert.ToInt32(dt.Rows[0]["ContentSlot7"]);
					Slot8 = Convert.ToInt32(dt.Rows[0]["ContentSlot8"]);
					Slot9 = Convert.ToInt32(dt.Rows[0]["ContentSlot9"]);

					lblPreview.Text = TemplateFunctions.EmailHTMLBody(TemplateSource, TableOptions, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9);
				}
			} else {
				lblPreview.Text = "<B>Could not find ID.</B> <BR>If the problem persist please contact sysadmin.";
			}
		}
		private void Page_Load(object sender, System.EventArgs e)
		{
			ShowPreview();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
