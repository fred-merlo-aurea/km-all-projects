namespace ecn.accounts.includes
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes.billing;

	
	///		Summary description for AnnualQuoteOptionSelector.
	
	public partial class AnnualQuoteItemEditor : System.Web.UI.UserControl
	{

		public QuoteOptionCollection QuoteOptions {
			get {
				return (QuoteOptionCollection) Session[QuoteOptionsKey];
			}
			set {
				Session[QuoteOptionsKey] = value;
			}
		}

		public QuoteItem AnnualQuoteItem {
			get {				
				return (QuoteItem) Session[QuoteItemsKey];
			}
			set {
				Session[QuoteItemsKey] = value;				
			}			
		}


		public event EventHandler OnAnnualItemChanged;
		#region Private Properties & Methods
		private string QuoteOptionsKey {
			get { return string.Format("{0}_QuoteOptionsKey", this.ID);}
		}	

		private string QuoteItemsKey {
			get { return string.Format("{0}_QuoteItemsKey", this.ID);}
		}	
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack) {
				InitializeSessionVars();		
			}
		}

		private void InitializeSessionVars() {			
			ddlAnnualTechOptions.DataSource = QuoteOptions;
			ddlAnnualTechOptions.DataTextField = "Name";
			ddlAnnualTechOptions.DataValueField = "Code";
			ddlAnnualTechOptions.DataBind();
			ddlAnnualTechOptions.Items.Insert(0,new ListItem("Please choose one", "-1"));
			ddlAnnualTechOptions.SelectedIndex = 0;

			if (AnnualQuoteItem != null) {
				ListItem item = ddlAnnualTechOptions.Items.FindByValue(AnnualQuoteItem.Code);
				ddlAnnualTechOptions.SelectedIndex = ddlAnnualTechOptions.Items.IndexOf(item);	
				txtDiscountRate.Text = (AnnualQuoteItem.DiscountRate*100).ToString();
			}

			ddlAnnualTechOptions_SelectedIndexChanged(null,null);
		}

		protected void ddlAnnualTechOptions_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (ddlAnnualTechOptions.SelectedIndex != 0) {
				QuoteOption option = QuoteOptions.FindByCode(ddlAnnualTechOptions.SelectedValue);
				txtRate.Text = option.Rate.ToString();			
				AnnualQuoteItem = GetQuoteItem(option);				
			} else {
				txtRate.Text = "0";
				AnnualQuoteItem = null;
			}

			if (OnAnnualItemChanged!=null) {
				OnAnnualItemChanged(this,null);
			}

			
		}

		private QuoteItem GetQuoteItem(QuoteOption option) {		
			if (AnnualQuoteItem == null) {
				QuoteItem item = new QuoteItem(option.AllowedFrequency, option.Code, option.Name, option.Description, option.Quantity, option.Rate, option.LicenseType, option.PriceType, option.IsCustomerCredit);
				item.DiscountRate = Convert.ToDouble(txtDiscountRate.Text)/100;
				item.RemoveProducts();
				item.AddProductFromOption(option);
				item.RemoveProductFeatures();
				item.AddProductFeatureFromOption(option);
				item.RemoveServices();
				item.AddServiceFromOption(option);
				return item;
			}

			AnnualQuoteItem.Frequency = option.AllowedFrequency;
			AnnualQuoteItem.Rate = Convert.ToDouble(txtRate.Text);
			AnnualQuoteItem.DiscountRate = Convert.ToDouble(txtDiscountRate.Text)/100;
			AnnualQuoteItem.IsActive = chkIsActive.Checked;

			AnnualQuoteItem.Code = option.Code;
			AnnualQuoteItem.Name = option.Name;			
			AnnualQuoteItem.Description = option.Description;			
			AnnualQuoteItem.RemoveProducts();
			AnnualQuoteItem.AddProductFromOption(option);
			AnnualQuoteItem.RemoveProductFeatures();
			AnnualQuoteItem.AddProductFeatureFromOption(option);
			AnnualQuoteItem.RemoveServices();
			AnnualQuoteItem.AddServiceFromOption(option);
			return AnnualQuoteItem;			
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

		protected void lnkUpdate_Click(object sender, System.EventArgs e) {
			if (ddlAnnualTechOptions.SelectedIndex != 0) {
				QuoteOption option = QuoteOptions.FindByCode(ddlAnnualTechOptions.SelectedValue);
				txtRate.Text = option.Rate.ToString();			
				AnnualQuoteItem = GetQuoteItem(option);				
			} else {
				txtRate.Text = "0";
				AnnualQuoteItem = null;
			}

			if (OnAnnualItemChanged!=null) {
				OnAnnualItemChanged(this,null);
			}
		}
	}
}
