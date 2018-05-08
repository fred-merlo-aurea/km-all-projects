namespace ecn.accounts.includes {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ecn.common.classes.billing;

	public partial class EmailQuoteItemEditor : QuoteItemEditorBase
    {
		protected void Page_Load(object sender, System.EventArgs e) {			
		}

		protected void ddlAddOptions_SelectedIndexChanged(object sender, System.EventArgs e) {
			DropDownList ddlAddOptions = sender as DropDownList;
			
			TextBox txtAddRate = ddlAddOptions.Parent.FindControl("txtAddRate") as TextBox;
			Label lblAddTotal = ddlAddOptions.Parent.FindControl("lblAddTotal") as Label;
			QuoteOption option = QuoteOptions.FindByCode(ddlAddOptions.SelectedValue);
			DropDownList ddlFrequency = ddlAddOptions.Parent.FindControl("ddlAddFrequency") as DropDownList;
			InitializeFrequencyDropDownList(ddlFrequency, option);

			DropDownList ddlEditFrequency= ddlAddOptions.Parent.FindControl("ddlAddFrequency") as DropDownList;
			FrequencyEnum quoteFrequency = (FrequencyEnum) Convert.ToInt32(ddlEditFrequency.SelectedValue);

			double actualRate = GetRateByFrequency(quoteFrequency, option);
			double actualTotal = option.Quantity * GetRateByFrequency(quoteFrequency, option);

			txtAddRate.Text = actualRate.ToString();
			lblAddTotal.Text = actualTotal.ToString();
		}

		protected void ddlAddFrequency_OnSelectedIndexChanged(object sender, System.EventArgs e) {
			DropDownList ddlAddFrequency = sender as DropDownList;

			FrequencyEnum quoteFrequency = (FrequencyEnum) Convert.ToInt32(ddlAddFrequency.SelectedValue);
			DropDownList ddlAddOptions = ddlAddFrequency.Parent.FindControl("ddlAddOptions") as DropDownList;
			TextBox txtAddRate = ddlAddFrequency.Parent.FindControl("txtAddRate") as TextBox;
			Label lblAddTotal = ddlAddFrequency.Parent.FindControl("lblAddTotal") as Label;			
			QuoteOption option = QuoteOptions.FindByCode(ddlAddOptions.SelectedValue);

			double actualRate = GetRateByFrequency(quoteFrequency, option);
			double actualTotal = option.Quantity * GetRateByFrequency(quoteFrequency, option);

			txtAddRate.Text = actualRate.ToString();
			lblAddTotal.Text = actualTotal.ToString();
		}		

		protected void ddlEditOptions_SelectedIndexChanged(object sender, System.EventArgs e) {
			DropDownList ddlAddOptions = sender as DropDownList;

			
			TextBox txtAddRate = ddlAddOptions.Parent.FindControl("txtEditRate") as TextBox;
			Label lblAddTotal = ddlAddOptions.Parent.FindControl("lblEditTotal") as Label;
			QuoteOption option = QuoteOptions.FindByCode(ddlAddOptions.SelectedValue);

			DropDownList ddlEditFrequency= ddlAddOptions.Parent.FindControl("ddlEditFrequency") as DropDownList;
			FrequencyEnum quoteFrequency = (FrequencyEnum) Convert.ToInt32(ddlEditFrequency.SelectedValue);

			double actualRate = GetRateByFrequency(quoteFrequency, option);
			double actualTotal = option.Quantity * GetRateByFrequency(quoteFrequency, option);

			txtAddRate.Text = actualRate.ToString();
			lblAddTotal.Text = actualTotal.ToString();
		}

		protected void ddlEditFrequency_SelectedIndexChanged(object sender, System.EventArgs e) {
			DropDownList ddlEditFrequency = sender as DropDownList;

			FrequencyEnum quoteFrequency = (FrequencyEnum) Convert.ToInt32(ddlEditFrequency.SelectedValue);
			DropDownList ddlEditOptions = ddlEditFrequency.Parent.FindControl("ddlEditType") as DropDownList;
			TextBox txtAddRate = ddlEditFrequency.Parent.FindControl("txtEditRate") as TextBox;
			Label lblAddTotal = ddlEditFrequency.Parent.FindControl("lblEditTotal") as Label;
			QuoteOption option = QuoteOptions.FindByCode(ddlEditOptions.SelectedValue);

			double actualRate = GetRateByFrequency(quoteFrequency, option);
			double actualTotal = option.Quantity * GetRateByFrequency(quoteFrequency, option);

			txtAddRate.Text = actualRate.ToString();
			lblAddTotal.Text = actualTotal.ToString();
		}
		
		
		/// 
		
		/// <param name="frequency"></param>
		/// <param name="quantity"></param>
		/// <returns></returns>
		private double GetRateByFrequency(FrequencyEnum frequency, QuoteOption option) {
			lblDebug.Text = string.Empty;
			// if it's a monthly item, just simply return its rate.
			if (frequency == FrequencyEnum.Monthly) {
				lblDebug.Text = "use the monthly rate.";
				return option.Rate;
			}

			long monthlyQuantity = CalculateMonthlyQuantity(option.Quantity, frequency);
			double rate = GetTheClosestRate(monthlyQuantity);
			
			switch(frequency) {
				case FrequencyEnum.Annual:
					lblDebug.Text += "rate * (1-0.15)";
					return rate * (1-0.15);
				case FrequencyEnum.Quarterly:
					lblDebug.Text += "rate * (1-0.08)";
					return rate * (1-0.08);
				default:
					return rate;
			}
		}

		private long CalculateMonthlyQuantity(long quantity, FrequencyEnum frequency) {
			switch(frequency) {
				case FrequencyEnum.Annual:
					lblDebug.Text += string.Format("{0}/12=>{1}|",quantity, quantity/12);
					return quantity/12;
				case FrequencyEnum.Quarterly:
					lblDebug.Text += string.Format("{0}/3=>{1}|",quantity, quantity/3);
					return quantity/3;
				default:
					return quantity;
			}
		}

		private double GetTheClosestRate(long monthlyQuantity) {
			long distanceToLowerBoundary = int.MaxValue;
			long distanceToUppderBoundary = int.MaxValue;
			int lowerBoundaryIndex = -1, upperBoundaryIndex = -1;
			for(int i=0; i< QuoteOptions.Count; i++ ) {
				QuoteOption option = QuoteOptions[i] as QuoteOption;
				if (option.Quantity >= monthlyQuantity && option.Quantity - monthlyQuantity <= distanceToUppderBoundary) {
					upperBoundaryIndex = i;
					distanceToUppderBoundary = option.Quantity - monthlyQuantity;
				}

				if (monthlyQuantity >= option.Quantity && monthlyQuantity - option.Quantity <= distanceToLowerBoundary) {
					lowerBoundaryIndex = i;
					distanceToLowerBoundary = monthlyQuantity - option.Quantity;
				}
			}

			

			if (lowerBoundaryIndex != -1) {
				lblDebug.Text += string.Format("choose Q:{0}/R:{1}|",  QuoteOptions[lowerBoundaryIndex].Quantity, QuoteOptions[lowerBoundaryIndex].Rate);
				return QuoteOptions[lowerBoundaryIndex].Rate;
			}

			if (upperBoundaryIndex != -1) {
				lblDebug.Text += string.Format("choose Q:{0}/R:{1}|", QuoteOptions[upperBoundaryIndex].Quantity, QuoteOptions[upperBoundaryIndex].Rate);
				return QuoteOptions[upperBoundaryIndex].Rate;
			}

			throw new ApplicationException("Can't find an appropriate rate for this option.");
	}

		private void dltEmailOptions_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.Footer) {
				DropDownList ddlAddOptions = e.Item.FindControl("ddlAddOptions") as DropDownList;

				BindQuoteOptionDDL(ddlAddOptions, null);
				ddlAddOptions_SelectedIndexChanged(ddlAddOptions,null);
				return;
			}

			if (e.Item.ItemType == ListItemType.Item) {
				LinkButton delete = e.Item.FindControl("btnDelete") as LinkButton;
				delete.Attributes.Add("onclick", "return confirm('Are you sure that you want to delete this item?')");
				CustomerizeItem(e.Item);
				return;
			}

			if (e.Item.ItemType == ListItemType.EditItem) {
				DropDownList ddl = e.Item.FindControl("ddlEditType") as DropDownList;
				TextBox txtRate = e.Item.FindControl("txtEditRate") as TextBox;
				TextBox txtDiscountRate = e.Item.FindControl("txtEditDiscountRate") as TextBox;
				Label lblTotal = e.Item.FindControl("lblEditTotal") as Label;
				DropDownList ddlFrequency = e.Item.FindControl("ddlEditFrequency") as DropDownList;

				QuoteItem item = QuoteItems[e.Item.ItemIndex];
				BindQuoteOptionDDL(ddl, item.Code);
				QuoteOption option = QuoteOptions.FindByCode(item.Code);
				InitializeFrequencyDropDownList(ddlFrequency, option);
				ListItem selectedItem = ddlFrequency.Items.FindByValue(((int)item.Frequency).ToString());
				if (selectedItem != null) {
					selectedItem.Selected=true;
				}
				txtRate.Text = item.Rate.ToString();
				txtDiscountRate.Text = (item.DiscountRate*100).ToString();
				lblTotal.Text = item.ItemPrice.ToString();
			}
		}

		private void CustomerizeItem(DataListItem item) {
			QuoteItem quoteItem = QuoteItems[item.ItemIndex];			
			
			if (quoteItem.IsCustomerCredit) {
				Label lblItemPrice = item.FindControl("lblItemPrice") as Label;
				lblItemPrice.CssClass = "CustomerCreditItem";                				
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
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent() {		
			this.dltEmailOptions.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dltEmailOptions_ItemCommand);
			this.dltEmailOptions.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dltEmailOptions_ItemDataBound);

		}
		#endregion		
	}
}

