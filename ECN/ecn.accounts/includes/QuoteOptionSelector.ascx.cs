namespace ecn.accounts.includes {
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using ecn.common.classes.billing;
	using ecn.common.classes.license;

    public enum SelectorStatus { List, New, Edit }

	public partial class QuoteOptionSelector : System.Web.UI.UserControl {
		protected ActiveUp.WebControls.ActiveDateTime startDate;
		protected ActiveUp.WebControls.ActiveDateTime endDate;

		public string PanelTitle {
			get { return lblTitle.Text;}
			set { lblTitle.Text = value;}
		}				
		
		public bool ReadOnly {
			get {
				return (bool) Session["ReadOnly"];
			}
			set {
				Session["ReadOnly"] = value;
				btnAdd.Visible = !value;
				dgdQuoteItemList.Columns[dgdQuoteItemList.Columns.Count-1].Visible = !value;
				dgdQuoteItemList.Columns[dgdQuoteItemList.Columns.Count-2].Visible = !value;
			}
		}


		public LicenseTypeEnum QuoteOptionLicenseType {
			get {return (LicenseTypeEnum) Session[LicenseTypeKey];}
			set { Session[LicenseTypeKey] = value;}
		}
				
		public QuoteOptionCollection QuoteOptions {
			get {
				return (QuoteOptionCollection) Session[QuoteOptionsKey];
			}
			set {
				Session[QuoteOptionsKey] = value;				
			}
		}	
		
		public QuoteItemCollection QuoteItems {
			get {				
				return (QuoteItemCollection) Session[QuoteItemsKey];
			}
			set {
				Session[QuoteItemsKey] = value;
				LoadQuoteItems(value);
			}			
		}

		public SelectorStatus Status {
			get {
				if (Session[SelectorStatusKey] == null) {
					Session[SelectorStatusKey] = SelectorStatus.List;
				}
				return (SelectorStatus) Session[SelectorStatusKey];
			}
			set {
				Session[SelectorStatusKey] = value;
			}
		}
		
		
		public event EventHandler OnQuoteItemChanged;

		protected void Page_Load(object sender, System.EventArgs e) {		
			
		}		

		public void AllowAdd(bool allow) {
			btnAdd.Enabled = allow;
		}


		public void SetQuoteItemsForQuote(Quote parent) {
			for(int i=parent.Items.Count-1; i>=0; i--) {
				if (parent.Items[i].LicenseType == QuoteOptionLicenseType) {
					parent.Items.RemoveAt(i);
				}
			}
			int validQuoteItemCount = dgdQuoteItemList.EditItemIndex == dgdQuoteItemList.Items.Count-1? dgdQuoteItemList.Items.Count-1: dgdQuoteItemList.Items.Count;
			for(int i=0; i< validQuoteItemCount; i++) {
				parent.AddItem(QuoteItems[i]);
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

		}
		#endregion

		#region Private Properties & Methods
		private string QuoteOptionsKey {
			get { return string.Format("{0}_QuoteOptionsKey", this.ID);}
		}

		private string LicenseTypeKey {
			get { return string.Format("{0}LicenseTypeKey", this.ID);}
		}

		private string QuoteItemsKey {
			get { return string.Format("{0}_QuoteItemsKey", this.ID);}
		}		

		private string SelectorStatusKey {
			get { return string.Format("{0}_SelectorStatusKey", this.ID);}
		}

		private DataGridItem CurrentEditItem;			
		#endregion

		#region Event Handler	
		protected void btnAdd_Click(object sender, System.EventArgs e) {
			Status = SelectorStatus.New;
			QuoteItems.Add(CreateQuoteItem());
			dgdQuoteItemList.EditItemIndex = QuoteItems.Count-1;
			LoadQuoteItems(QuoteItems);
			SetAccess();
			//			QuantityRequiredFieldValidator.Validate();
			//			if (!QuantityRequiredFieldValidator.IsValid) {
			//				return;
			//			}
			//
			//			QuantityValidator.Validate();
			//			if (!QuantityValidator.IsValid) {
			//				return;
			//			}
			//			if (OnQuoteItemAdded != null) {
			//				QuoteOption option = QuoteOptions.FindByCode(ddlQuoteName.SelectedValue);
			//				QuoteItem item = new QuoteItem((FrequencyEnum) Convert.ToInt32(ddlFrequency.SelectedValue) ,
			//					option.Code, option.Name, option.Description, Convert.ToInt64(txtQuantity.Text),Convert.ToDouble(lblRate.Text), QuoteOptionLicenseType,option.PriceType, chkCustomerCredit.Checked);
			//				item.AddProductFromOption(option);
			//				item.AddProductFeatureFromOption(option);
			//				item.AddServiceFromOption(option);
			//				OnQuoteItemAdded(this, new QuoteItemEventArgs(item));
			//			}
		}


		protected void dgdQuoteItemList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
        {
            var master = (this.Page.Master as ecn.accounts.MasterPages.Accounts);
			switch (e.CommandName) 
            {
				case "Edit":					
					dgdQuoteItemList.EditItemIndex = e.Item.ItemIndex;
					LoadQuoteItems(QuoteItems);
					Status = SelectorStatus.Edit;
					break;
				case "Update":					
					QuoteItem editItem = QuoteItems[e.Item.ItemIndex];					
					GetQuoteItem(editItem, e.Item);					
					dgdQuoteItemList.EditItemIndex = -1;
					LoadQuoteItems(QuoteItems);
					Status = SelectorStatus.List;
					if (OnQuoteItemChanged != null) {
						OnQuoteItemChanged(this, null);
					}
					break;
				case "Cancel":
					if (Status == SelectorStatus.New) {
						QuoteItems.RemoveAt(QuoteItems.Count-1);
					}

					dgdQuoteItemList.EditItemIndex = -1;
					LoadQuoteItems(QuoteItems);
					Status = SelectorStatus.List;
					break;
				case "Delete":
                    QuoteItems[e.Item.ItemIndex].Delete(master.UserSession.CurrentUser.UserID);
					QuoteItems.RemoveAt(e.Item.ItemIndex);
					dgdQuoteItemList.EditItemIndex = -1;
					LoadQuoteItems(QuoteItems);
					Status = SelectorStatus.List;
					if (OnQuoteItemChanged != null) {
						OnQuoteItemChanged(this, null);
					}
					break;
				default:
					throw new ApplicationException(string.Format("Unknow command '{0}'.", e.CommandName));	
			}

			SetAccess();
		}


		protected void dgdQuoteItemList_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer) {
				return;
			}			

			if (e.Item.ItemType == ListItemType.EditItem) {
				CurrentEditItem = e.Item;
				CustomerizeEditItem(e.Item);
				return;
			}
			CustomerizeItem(e.Item);
		}		

		protected void dgdQuoteItemList_OnItemDataBound(object sender, DataGridItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.EditItem) {
				QuoteItem quoteItem = QuoteItems[e.Item.ItemIndex];
				CheckBox chkCustomerCredit = GetCheckbox(e.Item, "chkEditCustomerCredit");
				chkCustomerCredit.Checked = quoteItem.IsCustomerCredit;

				DropDownList ddlCode = GetDropDownList(e.Item, "ddlEditCode");				
				LoadQuoteOptions(ddlCode, QuoteOptions, quoteItem.Code);
			}
		}
			
		private void ddlQuoteName_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (CurrentEditItem == null) {
				throw new ApplicationException("Current Edit Datagrid Item is empty.");
			}
			DropDownList ddlQuoteName = GetDropDownList(CurrentEditItem, "ddlEditCode");
			QuoteOption option = QuoteOptions.FindByCode(ddlQuoteName.SelectedValue);
			Label desc = GetLabel(CurrentEditItem, "lblEditDescription");
			desc.ToolTip = option.Description;

			GetTextBox(CurrentEditItem, "txtEditQuantity").Text = option.Quantity.ToString();
			GetTextBox(CurrentEditItem, "txtEditRate").Text = option.Rate.ToString();
			GetLabel(CurrentEditItem, "lblEditTotal").Text = string.Format("${0:###,##0.00}", option.Quantity * option.Rate);

			InitializeRadioButtonGroup(option);
			SetCustomerCredit(option);
		}

		private void InitializeRadioButtonGroup(QuoteOption option) {
			RadioButton rdoAnnual = GetRadioButton(CurrentEditItem, GetFrequencyControlName("rdoEdit", FrequencyEnum.Annual));
			rdoAnnual.Enabled = (FrequencyEnum.Annual& option.AllowedFrequency) != 0;
			RadioButton rdoQuarter = GetRadioButton(CurrentEditItem, GetFrequencyControlName("rdoEdit", FrequencyEnum.Quarterly));
			rdoQuarter.Enabled = (FrequencyEnum.Quarterly& option.AllowedFrequency) != 0;
			RadioButton rdoMonthly = GetRadioButton(CurrentEditItem, GetFrequencyControlName("rdoEdit", FrequencyEnum.Monthly));
			rdoMonthly.Enabled = (FrequencyEnum.Monthly& option.AllowedFrequency) != 0;
			RadioButton rdoOneTime = GetRadioButton(CurrentEditItem, GetFrequencyControlName("rdoEdit", FrequencyEnum.OneTime));
			rdoOneTime.Enabled = (FrequencyEnum.OneTime& option.AllowedFrequency) != 0;
			
			ArrayList buttons = new ArrayList();
			buttons.Add(rdoAnnual);
			buttons.Add(rdoQuarter);
			buttons.Add(rdoMonthly);
			buttons.Add(rdoOneTime);

			foreach(RadioButton btn in buttons) {						
				btn.Checked = false;
			}

			foreach(RadioButton btn in buttons) {						
				if(btn.Enabled) {
					btn.Checked = true;
					break;
				}
			}
		}
		
		private void SetCustomerCredit(QuoteOption selectedOption) {
			CheckBox chkCustomerCredit = GetCheckbox(CurrentEditItem, "chkEditCustomerCredit");
			if (QuoteOptionLicenseType == LicenseTypeEnum.AnnualTechAccess || QuoteOptionLicenseType == LicenseTypeEnum.Option) {
				chkCustomerCredit.Checked = false;
				chkCustomerCredit.Enabled = false;
				return;
			}			
						
			if (selectedOption.IsCustomerCredit) {
				chkCustomerCredit.Checked = true;
			}
			chkCustomerCredit.Enabled = !selectedOption.IsCustomerCredit;			
		}

		private void LoadQuoteOptions(DropDownList ddlOptions, QuoteOptionCollection options, string selectedValue) {
			ddlOptions.DataSource = options;
			ddlOptions.DataValueField = "Code";
			ddlOptions.DataTextField = "Name";			
			ddlOptions.DataBind();			
			ddlOptions.SelectedIndex = ddlOptions.Items.IndexOf(ddlOptions.Items.FindByValue(selectedValue));
			
			if (options.Count > 0) {
				ddlQuoteName_SelectedIndexChanged(null,null);			
			}			
		}

		private void LoadQuoteItems(QuoteItemCollection quoteItems) {			
			dgdQuoteItemList.DataSource = quoteItems;
			dgdQuoteItemList.DataBind();
		}		
		
		private void CustomerizeEditItem(DataGridItem item) {
			QuoteItem quoteItem = QuoteItems[item.ItemIndex];
			RadioButton rdoFrequency = GetRadioButton(item, GetFrequencyControlName("rdoEdit",quoteItem.Frequency));
			rdoFrequency.Checked = true;			
			
			CheckBox chkIsActive = GetCheckbox(item, "chkEditIsActive");
			chkIsActive.Checked = quoteItem.IsActive;

			TextBox txtQuantity = GetTextBox(item, "txtEditQuantity");
			txtQuantity.Text = quoteItem.Quantity.ToString();
			TextBox txtRate = GetTextBox(item, "txtEditRate");
			txtRate.Text = quoteItem.Rate.ToString();

			((LinkButton)item.FindControl("btnDelete")).Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to delete \"{0}\"?')", quoteItem.Name));

			DropDownList ddlCode = GetDropDownList(item, "ddlEditCode");
			ddlCode.SelectedIndexChanged += new EventHandler(ddlQuoteName_SelectedIndexChanged);
		}		

		private void CustomerizeItem(DataGridItem item) {
			QuoteItem quoteItem = QuoteItems[item.ItemIndex];
			Label lblFrequency = GetLabel(item, GetFrequencyControlName("lbl", quoteItem.Frequency));
			lblFrequency.Text = "X";	
			LinkButton btnDelete = (LinkButton)item.FindControl("btnDelete");
			(btnDelete).Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to delete \"{0}\"?')", quoteItem.Name));
			
			if (quoteItem.IsCustomerCredit) {
				Label lblCustomer = GetLabel(item, "lblCustomerCredit");
				lblCustomer.Text = "X";
				item.Cells[8].CssClass = "CustomerCreditItem";
			}
		}

		private void GetQuoteItem(QuoteItem quoteItem, DataGridItem item) {
			quoteItem.Quantity = Convert.ToInt32(GetTextBox(item, "txtEditQuantity").Text);
			DropDownList ddlName = GetDropDownList(item, "ddlEditCode");
			quoteItem.Code = ddlName.SelectedValue;
			quoteItem.Name = ddlName.SelectedItem.Text;			
			quoteItem.Rate = Convert.ToDouble(GetTextBox(item, "txtEditRate").Text);
			quoteItem.IsCustomerCredit = Convert.ToBoolean(GetCheckbox(item, "chkEditCustomerCredit").Checked);
			quoteItem.IsActive = Convert.ToBoolean(GetCheckbox(item, "chkEditIsActive").Checked);
			FrequencyEnum frequency = FrequencyEnum.BiWeekly;

			QuoteOption option = QuoteOptions[ddlName.SelectedIndex];

			if (GetRadioButton(item, "rdoEditOneTime").Checked) {
				frequency = FrequencyEnum.OneTime;
			}

			if (GetRadioButton(item, "rdoEditMonthly").Checked) {
				frequency = FrequencyEnum.Monthly;
			}

			if (GetRadioButton(item, "rdoEditQuarterly").Checked) {
				frequency = FrequencyEnum.Quarterly;
			}

			if (GetRadioButton(item, "rdoEditAnnual").Checked) {
				frequency = FrequencyEnum.Annual;
			}

			quoteItem.Frequency = frequency;
			quoteItem.Description = option.Description;			
			quoteItem.RemoveProducts();
			quoteItem.AddProductFromOption(option);
			quoteItem.RemoveProductFeatures();
			quoteItem.AddProductFeatureFromOption(option);
			quoteItem.RemoveServices();
			quoteItem.AddServiceFromOption(option);			
		}
		private void SetAccess() {
			btnAdd.Enabled = Status == SelectorStatus.List;			
		}

		#region Find Control Methods
		private Label GetLabel(DataGridItem item, string lblName) {
			return (Label) item.FindControl(lblName);	
		}

		private RadioButton GetRadioButton(DataGridItem item, string rdoName) {
			return (RadioButton) item.FindControl(rdoName);	
		}

		private CheckBox GetCheckbox(DataGridItem item, string chkName) {
			return (CheckBox) item.FindControl(chkName);
		}

		private TextBox GetTextBox(DataGridItem item, string txtName) {
			return (TextBox) item.FindControl(txtName);
		}

		private DropDownList GetDropDownList(DataGridItem item, string ddlName) {
			return (DropDownList) item.FindControl(ddlName);
		}

		private string GetFrequencyControlName(string controlPrefix, FrequencyEnum frequency) {
			switch (frequency) {
				case FrequencyEnum.OneTime:
					return controlPrefix + "OneTime";
				case FrequencyEnum.Monthly:
					return controlPrefix + "Monthly";
				case FrequencyEnum.Quarterly:
					return controlPrefix + "Quarterly";
				case FrequencyEnum.Annual:
					return controlPrefix + "Annual";
			}
			throw new ApplicationException(string.Format("unexpected frequency '{0}'", frequency.ToString()));
		}
		#endregion

		private QuoteItem CreateQuoteItem() {
			if (QuoteOptions.Count == 0) {
				return null;
			}

			QuoteOption option = QuoteOptions[0];
			
			if ((FrequencyEnum.Monthly&option.AllowedFrequency) >0) {
				return new QuoteItem(FrequencyEnum.Monthly, option.Code, option.Name, option.Description, option.Quantity, option.Rate, option.LicenseType, option.PriceType, option.IsCustomerCredit);
			}

			if ((FrequencyEnum.Quarterly&option.AllowedFrequency) >0) {
				return new QuoteItem(FrequencyEnum.Quarterly, option.Code, option.Name, option.Description, option.Quantity, option.Rate, option.LicenseType, option.PriceType, option.IsCustomerCredit);
			}

			if ((FrequencyEnum.Annual&option.AllowedFrequency) >0) {
				return new QuoteItem(FrequencyEnum.Annual, option.Code, option.Name, option.Description, option.Quantity, option.Rate, option.LicenseType, option.PriceType, option.IsCustomerCredit);
			}

			if ((FrequencyEnum.OneTime&option.AllowedFrequency) >0) {
				return new QuoteItem(FrequencyEnum.OneTime, option.Code, option.Name, option.Description, option.Quantity, option.Rate, option.LicenseType, option.PriceType, option.IsCustomerCredit);
			}
			throw new ApplicationException("Can't create new quote item by this quote option. Allowed frequency is empty.");
		}
		#endregion				
	}	
}