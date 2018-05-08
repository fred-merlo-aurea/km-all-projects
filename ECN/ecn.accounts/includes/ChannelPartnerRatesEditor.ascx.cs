namespace ecn.accounts.includes
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes.billing;

	
	///		Summary description for ChannelPartnerRates.
	
	public partial class ChannelPartnerRatesEditor : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}
		
		public string Title {		
			set {
				lblTitle.Text = value;
			}
		}
		
		public RatesKeeper Rates {
			get {
				return (RatesKeeper)(Session["Rates"]);
			}
			set {
				Session["Rates"] = value;
			}
		}
   
		private void DisplayRates(RatesKeeper rates) {
			dltRates.DataSource = rates.RangeAndRates;
			dltRates.DataBind();
			txtDefaultRate.Text = rates.DefaultRate.ToString();
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
