namespace ecn.accounts.includes {
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes;
	using ecn.common.classes.billing;

	public partial class QuoteItemEditor : QuoteItemEditorBase
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

            if (option.IsAllowed(FrequencyEnum.OneTime))
                txtAddRate.Enabled = true;
            else
                txtAddRate.Enabled = false;


			txtAddRate.Text = option.Rate.ToString();
			lblAddTotal.Text = option.ItemPrice.ToString();
		}

		protected void ddlEditOptions_SelectedIndexChanged(object sender, System.EventArgs e) {
			DropDownList ddlAddOptions = sender as DropDownList;
			TextBox txtAddRate = ddlAddOptions.Parent.FindControl("txtEditRate") as TextBox;
			Label lblAddTotal = ddlAddOptions.Parent.FindControl("lblEditTotal") as Label;
			QuoteOption option = QuoteOptions.FindByCode(ddlAddOptions.SelectedValue);

            if (option.IsAllowed(FrequencyEnum.OneTime))
                txtAddRate.Enabled = true;
            else
                txtAddRate.Enabled = false;

			txtAddRate.Text = option.Rate.ToString();
			lblAddTotal.Text = option.ItemPrice.ToString();
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
				TextBox txtEditDiscountRate = e.Item.FindControl("txtEditDiscountRate") as TextBox;
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
				txtEditDiscountRate.Text = (item.DiscountRate*100).ToString();
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

