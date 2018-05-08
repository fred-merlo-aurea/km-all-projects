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
using ecn.common.classes;

namespace ecn.digitaledition
{
	/// <summary>
	/// Summary description for index1.
	/// </summary>
	public partial class index : System.Web.UI.Page
	{

		protected System.Web.UI.WebControls.Image imgThumbnail;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblErrorMessage.Visible = false;
			if (!IsPostBack)
			{
				pnllist.Visible= false;
				pnllogin.Visible = true;
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
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.ImageButton1.Click += new System.Web.UI.ImageClickEventHandler(this.ImageButton1_Click);

		}
		#endregion

		private void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			if (txtLogin.Text.ToLower()=="administrator" && txtpassword.Text == "L!4aE2")
			{
			
				DataTable dt = DataFunctions.GetDataTable("select EditionID, EditionName, '" + ConfigurationManager.AppSettings["ImagePath"] +  "/ecn.images/customers/' + convert(varchar,c.customerID) + '/publisher/' + convert(varchar,editionID) + '/150/1.png' as thumbview from edition e join Publication m on e.PublicationID = m.PublicationID join ecn5_accounts..customer c on m.customerID = c.customerID where e.Status = 'Active' and m.active=1");

				DataList1.DataSource = dt;
				DataList1.DataBind();
				pnllist.Visible= true;
				pnllogin.Visible = false;
			}
			else
			{
				lblErrorMessage.Text = "Invalid Username and Password.";
				lblErrorMessage.Visible = true;
			}
		}
	}
}
