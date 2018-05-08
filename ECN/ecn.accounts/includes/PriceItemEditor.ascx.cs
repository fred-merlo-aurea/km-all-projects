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
	
	public partial class PriceItemEditor : System.Web.UI.UserControl {
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
			}
		}

		public LicenseTypeEnum QuoteOptionLicenseType {
			get { 
				if (Session[LicenseTypeKey] == null) {
					return LicenseTypeEnum.AnnualTechAccess;
				}
				return (LicenseTypeEnum) Session[LicenseTypeKey];
			}
			set { Session[LicenseTypeKey] = value;}
		}
				
		public QuoteOptionCollection QuoteOptions {
			get {
				return (QuoteOptionCollection) Session[QuoteOptionsKey];
			}
			set {
				Session[QuoteOptionsKey] = value;
				LoadQuoteOptions(value);
			}
		}

		public ArrayList ProductFeatures {
			get { return (ArrayList) Session[ProductFeaturesKey];}
			set { Session[ProductFeaturesKey] = value;}
		}

		public int BaseChannelID {
			get { return (int) Session["BaseChannelID"];}
			set { Session["BaseChannelID"] = value;}
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

		protected string RateFormatString {
			get {				
				if (QuoteOptionLicenseType == LicenseTypeEnum.EmailBlock) {
					return "${0:##,##0.0000}";
				}
				return "${0:##,##0.00}";
			}
		}

		protected void Page_Load(object sender, System.EventArgs e) {
			if (!IsPostBack) {
				ProductFeatures =  ProductFeature.GetAllFeatures();
			}
			if (QuoteOptionLicenseType == LicenseTypeEnum.AnnualTechAccess) {
				dgdQuoteOptionsList.Columns[9].Visible = true;
				dgdQuoteOptionsList.Columns[11].Visible = true;
			}

			if (QuoteOptionLicenseType == LicenseTypeEnum.Option) {
				dgdQuoteOptionsList.Columns[10].Visible = true;
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

		private string ProductFeaturesKey {
			get { return string.Format("{0}_ProductFeaturesKey", this.ID);}
		}
		private string SelectorStatusKey {
			get { return string.Format("{0}_SelectorStatusKey", this.ID);}
		}

		private DataGridItem CurrentEditItem;			
		#endregion

		#region Event Handler	
		protected void btnAdd_Click(object sender, System.EventArgs e) {
			Status = SelectorStatus.New;
			QuoteOptions.Add(CreateQuoteOption());
			dgdQuoteOptionsList.EditItemIndex = QuoteOptions.Count-1;
			LoadQuoteOptions(QuoteOptions);
			SetAccess();		
		}

		protected void dgdQuoteOptionsList_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) 
        {
            var master = (this.Page.Master as ecn.accounts.MasterPages.Accounts);
			switch (e.CommandName) 
            {
				case "Edit":					
					dgdQuoteOptionsList.EditItemIndex = e.Item.ItemIndex;
					LoadQuoteOptions(QuoteOptions);
					Status = SelectorStatus.Edit;
					break;
				case "Update":			
					QuoteOption option = QuoteOptions[e.Item.ItemIndex];					
					GetQuoteOption(option, e.Item);

                    option.Save(master.UserSession.CurrentUser.UserID);

					dgdQuoteOptionsList.EditItemIndex = -1;
					LoadQuoteOptions(QuoteOptions);
					Status = SelectorStatus.List;	
					break;
				case "Cancel":
					if (Status == SelectorStatus.New) {
						QuoteOptions.RemoveAt(QuoteOptions.Count-1);
					}

					dgdQuoteOptionsList.EditItemIndex = -1;
					LoadQuoteOptions(QuoteOptions);
					Status = SelectorStatus.List;
					break;
				case "Delete":
                    QuoteOptions[e.Item.ItemIndex].Delete(master.UserSession.CurrentUser.UserID);
					QuoteOptions.RemoveAt(e.Item.ItemIndex);
					dgdQuoteOptionsList.EditItemIndex = -1;
					LoadQuoteOptions(QuoteOptions);
					Status = SelectorStatus.List;					
					break;
				default:
					throw new ApplicationException(string.Format("Unknow command '{0}'.", e.CommandName));	
			}
			SetAccess();
		}


		protected void dgdQuoteOptionsList_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) {
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

		protected void dgdQuoteOptionsList_ItemDataBind(object sender,System.Web.UI.WebControls.DataGridItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.EditItem) {
				TextBox txtQuantity = GetTextBox(e.Item, "txtEditQuantity");				
				TextBox txtRate = GetTextBox(e.Item, "txtEditRate");										
				TextBox txtEditItemPrice = GetTextBox(e.Item, "txtEditItemPrice");				
				
				txtQuantity.Attributes.Add("OnKeyUp", GetJStoUpdatePriceItemOnChange(txtRate, txtQuantity, txtEditItemPrice));
				txtRate.Attributes.Add("OnKeyUp", GetJStoUpdatePriceItemOnChange(txtRate, txtQuantity, txtEditItemPrice));
			}
		}

		private void LoadQuoteOptions(QuoteOptionCollection quoteOptions) {			
			dgdQuoteOptionsList.DataSource = quoteOptions;
			dgdQuoteOptionsList.DataBind();
		}		
		
		private void CustomerizeEditItem(DataGridItem item) {			
			QuoteOption quoteOption = QuoteOptions[item.ItemIndex];			
			
			CheckBox chkCustomerCredit = GetCheckbox(item, "chkEditCustomerCredit");
			chkCustomerCredit.Checked = quoteOption.IsCustomerCredit;			

			TextBox txtCode = GetTextBox(item, "txtEditCode");
			txtCode.Text = quoteOption.Code;
			TextBox txtName = GetTextBox(item, "txtEditName");
			txtName.Text = quoteOption.Name;
			TextBox txtDescription = GetTextBox(item, "txtEditDescription");
			txtDescription.Text = quoteOption.Description;

			TextBox txtQuantity = GetTextBox(item, "txtEditQuantity");
			txtQuantity.Text = quoteOption.Quantity.ToString();			
			
			TextBox txtRate = GetTextBox(item, "txtEditRate");
			txtRate.Text = quoteOption.Rate.ToString();		
			
			TextBox txtEditItemPrice = GetTextBox(item, "txtEditItemPrice");
			txtEditItemPrice.Text = quoteOption.ItemPrice.ToString();			

			DropDownList ddlEditPriceType = GetDropDownList(item, "ddlEditPriceType");
			ddlEditPriceType.SelectedIndex = ddlEditPriceType.Items.IndexOf(ddlEditPriceType.Items.FindByValue(Convert.ToInt32(quoteOption.PriceType).ToString()));
			
			LoadAllowedFrequency(item, quoteOption);
			//LoadProductsAndFeatures(item, quoteOption);

			if (quoteOption.Services.Count > 0) {
				LoadService(item, quoteOption);
			}

			((LinkButton)item.FindControl("btnDelete")).Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to delete \"{0}\"?')", quoteOption.Name));			
		}

		private string GetJStoUpdatePriceItemOnChange(TextBox rate, TextBox quality, TextBox itemPrice) {
			return string.Format("document.getElementById('{0}').value = document.getElementById('{1}').value * document.getElementById('{2}').value",
				itemPrice.UniqueID, 
				rate.UniqueID, 
				quality.UniqueID);
		}

		const int maxLen = 20;
		private void CustomerizeItem(DataGridItem item) {			
			QuoteOption quoteOption = QuoteOptions[item.ItemIndex];
			Label lblDescription = GetLabel(item, "lblDescription");
			
			lblDescription.Text = quoteOption.Description.Length<maxLen?quoteOption.Description:string.Format("{0}...", quoteOption.Description.Substring(0, maxLen-3));
			Label lblName = GetLabel(item, "lblName");
			lblName.Text = quoteOption.Name.Length<maxLen?quoteOption.Name:string.Format("{0} ...", quoteOption.Name.Substring(0, maxLen-3));

			ShowAllowedFrequency(item, quoteOption);
			ShowProductsAndFeatures(item, quoteOption);
			ShowService(item,quoteOption);

			LinkButton btnDelete = (LinkButton)item.FindControl("btnDelete");
			(btnDelete).Attributes.Add("onclick", string.Format("return confirm('Are you sure you want to delete \"{0}\"?')", quoteOption.Name));		
		}	

		private void ShowAllowedFrequency(DataGridItem item, QuoteOption quoteOption) {
			Label lblAllowedFrequency = GetLabel(item, "lblAllowedFrequency");
			lblAllowedFrequency.Text = quoteOption.AllowedFrequencyNames.Length == 0?"":"...";
			lblAllowedFrequency.Attributes.Add("Title", quoteOption.AllowedFrequencyNames.Replace("/", Environment.NewLine));
		}

		private void ShowProductsAndFeatures(DataGridItem item, QuoteOption quoteOption) {
			Label lblProducts = GetLabel(item, "lblProducts");
			lblProducts.Text = quoteOption.ProductNames.Length == 0?"":"...";
			lblProducts.Attributes.Add("Title", quoteOption.ProductNames.Replace("/", Environment.NewLine));

			Label lblProductFeatures = GetLabel(item, "lblProductFeatures");
			lblProductFeatures.Text = quoteOption.ProductFeatureNames.Length == 0?"":"...";
			lblProductFeatures.Attributes.Add("Title", quoteOption.ProductFeatureNames.Replace("/", Environment.NewLine));
		}

		private void ShowService(DataGridItem item, QuoteOption quoteOption) {
			Label lblServices = GetLabel(item, "lblServices");
			lblServices.Text = quoteOption.Services.Count > 0?"...":"";
			lblServices.Attributes.Add("Title",quoteOption.Services.ToString());
		}

		private void LoadAllowedFrequency(DataGridItem item, QuoteOption quoteOption) {
			CheckBoxList chkEditAllowedFrequency = GetCheckBoxList(item, "chkEditAllowedFrequency");
			foreach(ListItem listItem in chkEditAllowedFrequency.Items) {
				FrequencyEnum f = (FrequencyEnum) Convert.ToInt32(listItem.Value);
				if (quoteOption.IsAllowed(f)) {
					listItem.Selected = true;
				}
			}
		}

		private void LoadProductsAndFeatures(DataGridItem item, QuoteOption quoteOption) {
			CheckBoxList chkEditProducts = GetCheckBoxList(item, "chkEditProducts");
			foreach(Product p in Product.GetAllProducts()) {
				chkEditProducts.Items.Add(new ListItem(p.Name, p.ID.ToString()));
			}
			
			foreach(Product p in quoteOption.Products) {
				chkEditProducts.Items.FindByValue(p.ID.ToString()).Selected = true;				
			}		
			chkEditProducts.DataBind();

			CheckBoxList chkEditProductFeatures = GetCheckBoxList(item, "chkEditProductFeatures");
			foreach(ProductFeature f in ProductFeatures) {
				chkEditProductFeatures.Items.Add(new ListItem(f.Name, f.ID.ToString()));
			}			

			foreach(ProductFeature f in quoteOption.ProductFeatures) {
				chkEditProductFeatures.Items.FindByValue(f.ID.ToString()).Selected = true;
			}	
		}

		private void LoadService(DataGridItem item, QuoteOption quoteOption) {
			TextBox txtEditClientInquirieCount = GetTextBox(item, "txtEditClientInquirieCount");
			txtEditClientInquirieCount.Text = quoteOption.Services[0].AllowedInquirieCount.ToString();

			TextBox txtEditAdditionalRate = GetTextBox(item, "txtEditAdditionalRate");
			txtEditAdditionalRate.Text = quoteOption.Services[0].RateForAdditionalInquiries.ToString();
		}

		private void GetQuoteOption(QuoteOption option, DataGridItem item) {
			option.Code = GetTextBox(item, "txtEditCode").Text;
			option.Name = GetTextBox(item, "txtEditName").Text;
			option.Description = GetTextBox(item, "txtEditDescription").Text;
			option.Rate = Convert.ToDouble(GetTextBox(item,"txtEditRate").Text);
			option.Quantity = Convert.ToInt32(GetTextBox(item, "txtEditQuantity").Text);
			option.IsCustomerCredit = GetCheckbox(item,"chkEditCustomerCredit").Checked;
			option.AllowedFrequency = GetAllowedFrequency(item);
			option.PriceType = (PriceTypeEnum) Convert.ToInt32( GetDropDownList(item, "ddlEditPriceType").SelectedValue);
			option.RemoveProducts();
			option.AddProducts(GetProducts(item));
			option.RemoveProductFeatures();			
			option.AddProductFeatures(GetProductFeatures(item));
			
			if (QuoteOptionLicenseType == LicenseTypeEnum.AnnualTechAccess) {
				option.Services.Clear();
				option.AddService(new Service(Convert.ToInt32(GetTextBox(item, "txtEditClientInquirieCount").Text), Convert.ToInt32(GetTextBox(item, "txtEditAdditionalRate").Text)));
			}
		}

		private FrequencyEnum GetAllowedFrequency(DataGridItem item) {
			int allowedFrequency = 0;
			CheckBoxList chkAllowedFrequency = GetCheckBoxList(item, "chkEditAllowedFrequency");
			foreach(ListItem listItem in chkAllowedFrequency.Items) {
				if (listItem.Selected) {
					allowedFrequency += Convert.ToInt32(listItem.Value);
				}
			}
			return (FrequencyEnum) allowedFrequency;			
		}

		private ArrayList GetProducts(DataGridItem item) {
			ArrayList products = new ArrayList();
			CheckBoxList chkProducts = GetCheckBoxList(item, "chkEditProducts");
			foreach(ListItem listItem in chkProducts.Items) {
				if (listItem.Selected) {
					products.Add(new Product(Convert.ToInt32(listItem.Value), listItem.Text));
				}
			}
			return products;
		}

		private ArrayList GetProductFeatures(DataGridItem item) {
			ArrayList features = new ArrayList();
			CheckBoxList chkProductFeatures = GetCheckBoxList(item, "chkEditProductFeatures");
			foreach(ListItem listItem in chkProductFeatures.Items) {
				if (listItem.Selected) {
					features.Add(GetProductFeatureByID(Convert.ToInt32(listItem.Value)));
				}
			}
			return features;
		}

		private ProductFeature GetProductFeatureByID(int id) {
			foreach(ProductFeature f in ProductFeatures) {
				if (f.ID == id) {
					return f;
				}
			}
			return null;
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
		
		private CheckBoxList GetCheckBoxList(DataGridItem item, string chkName) {
			return (CheckBoxList) item.FindControl(chkName);
		}


		private TextBox GetTextBox(DataGridItem item, string txtName) {
			return (TextBox) item.FindControl(txtName);
		}

		private DropDownList GetDropDownList(DataGridItem item, string ddlName) {
			return (DropDownList) item.FindControl(ddlName);
		}

		#endregion

		private QuoteOption CreateQuoteOption() {			
			QuoteOption option = new QuoteOption("","","",QuoteOptionLicenseType,0,0,PriceTypeEnum.Recurring, FrequencyEnum.Annual|FrequencyEnum.Monthly|FrequencyEnum.Quarterly|FrequencyEnum.OneTime);			
			option.BaseChannelID = BaseChannelID;
			return option;
		}
		#endregion				
	}	
}