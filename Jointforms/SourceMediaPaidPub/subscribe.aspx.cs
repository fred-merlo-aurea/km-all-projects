using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.Configuration;
using KMPS_JF_Objects.Objects;
using AuthorizeNet;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Globalization;
using System.Net.Mime;
using ecn.communicator.classes;
using System.Web;
using System.Xml.Linq;


using SourceMediaPaidPub.Objects;
using SourceMediaPaidPub.Process;
using Magazine = SourceMediaPaidPub.Objects.Magazine;
using Price = SourceMediaPaidPub.Objects.Price;
using System.Net;
using KM.Common;

namespace PaidPub
{
	public partial class subscribe : System.Web.UI.Page
	{
		double totalAmount = 0;
		private string _countryName = String.Empty;
		private List<Objects.Item> itemList = new List<PaidPub.Objects.Item>();
        
        string Errorlocation = string.Empty;

        private AuthorizationRequest AuthorizeNetRequest = new AuthorizationRequest("", "", 0.00M, "");
        private GatewayResponse AuthorizeNetResponse = null;

		private SubscriberProcess _subscriberProcess = new SubscriberProcess();

		private Magazine _magazine;

		private double? _standardPrice = null;

		private double? _premiumPrice = null;

		private double _standardPriceTax = 0.0;

		private double _premiumPriceTax = 0.0;

		private double _totalStandard = 0.0;
		private double _totalPremium = 0.0;


		private List<string> _standardTerms = new List<string>();
		private List<string> _premiumTerms = new List<string>();

		private float _taxesPercentageForZipCode;


		#region PROPERTIES

		public string CountryCodePath
		{
			get { return ConfigurationManager.AppSettings["CountryCodeJson"]; }
		}

		public string FilePath
		{
			get { return ConfigurationManager.AppSettings["MagazineJson"]; }
		}

		public string ApiKey
		{
			get { return ConfigurationManager.AppSettings["TaxjarAPI"]; }
		}

		public string ConnectionString
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;
				
			}
		}


		public string PubCode
		{
			get
			{
				try
				{
					if (Request.QueryString["pubcode"].ToString().Contains(','))
						return Request.QueryString["pubcode"].ToString().Split(',')[0].ToString();
					else
						return Request.QueryString["pubcode"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
			set { ViewState["PubCode"] = value; }
		}

		public string PromoCode
		{
			get
			{
				try
				{
					return Request.QueryString["user_promoCode"].ToString();
				}
				catch
				{
					return String.Empty;
				}
			}
			set { ViewState["user_promoCode"] = value; }
		}

		public string WebSite
		{
			get
			{
				try
				{
					return Request.QueryString["website"].ToString();
				}
				catch
				{
					return String.Empty;
				}
			}
		}

		public string QueryStringEmail
		{
			get
			{
				try
				{
					string emailAddress = Request.QueryString["e"].ToString();
					if (emailAddress.Contains(","))
					{
						emailAddress = emailAddress.Split(',')[0];
					}
					return emailAddress;
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringFirstName
		{
			get
			{
				try
				{
					return Request.QueryString["fn"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringLastName
		{
			get
			{
				try
				{
					return Request.QueryString["ln"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringCompany
		{
			get
			{
				try
				{
					return Request.QueryString["compname"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringPhone
		{
			get
			{
				try
				{
					return Request.QueryString["ph"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringFax
		{
			get
			{
				try
				{
					return Request.QueryString["fax"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringAddress1
		{
			get
			{
				try
				{
					return Request.QueryString["adr"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringAddress2
		{
			get
			{
				try
				{
					return Request.QueryString["adr2"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringCity
		{
			get
			{
				try
				{
					return Request.QueryString["city"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringState
		{
			get
			{
				try
				{
					return Request.QueryString["state"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string QueryStringZip
		{
			get
			{
				try
				{
					return Request.QueryString["zc"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string TitleQS
		{
			get
			{
				try
				{
					return Request.QueryString["TITLE"].ToString();
				}
				catch
				{
					return String.Empty;
				}
			}
		}

		public string Occupation
		{
			get
			{
				try
				{
					return Request.QueryString["OCCUPATION"].ToString();
				}
				catch
				{
					return String.Empty;
				}
			}
		}

		private Enums.Region _region = Enums.Region.INTERNATIONAL;

		public string Filter
		{
			get { return " and emails.emailaddress = '" + txtemail.Text + "' "; }

		}


		public Enums.Region Region
		{
			get { return _region; }
			set { _region = value; }
		}

		public string Country
		{
			get
			{
				try
				{
					return Request.QueryString["ctry"].ToString();
				}
				catch
				{
					return "UNITED STATES OF AMERICA";
				}
			}
		}

		public string StateInt
		{
			get
			{
				try
				{
					return Request.QueryString["user_STATE_INT"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string ForZip
		{
			get
			{
				try
				{
					return Request.QueryString["user_FORZIP"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}
	
		public string VirtualDirectory
		{
			get
			{
				try
				{
					return WebConfigurationManager.AppSettings["VirtualDirectory"].ToString();
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public string TransactionID
		{
			get
			{
				try
				{
					return ViewState["TRANSACTIONID"].ToString();
				}
				catch { return string.Empty; }
			}
			set
			{
				ViewState["TRANSACTIONID"] = value;
			}
		}

		public bool IsStandardVisible
		{
			get
			{
				return rbStandardPrint.Checked;
			}
			set
			{
				lblStandardTaxes.Visible = value;
				lbltotalStandards.Visible = value;
			}
		}

		public bool IsPremiumVisible
		{
			get { return rbPremiumPrint.Checked; }
			set
			{
				lblPremiumTaxes.Visible = value;
				lbltotalPremiums.Visible = value;
			}
		}

		public string ShippingZip
		{
			get { return txtShippingZip.Text; }
			set { txtShippingZip.Text = value; }
		}

		public string BillingZip
		{
			get { return txtBillingZip.Text; }
			set { txtBillingZip.Text = value; }
		}

		public TextBox Txtemail
		{
			set { txtemail = value; }
			get { return txtemail; }
		}

		public TextBox UserCardHolderName
		{
			set { user_CardHolderName = value; }
			get { return user_CardHolderName; }
		}

		public DropDownList UserCcType
		{
			set { user_CCType = value; }
			get { return user_CCType; }
		}

		public TextBox UserCcNumber
		{
			set { user_CCNumber = value; }
			get { return user_CCNumber; }
		}

		public DropDownList UserExpMonth
		{
			set { user_Exp_Month = value; }
			get { return user_Exp_Month; }
		}

		public DropDownList UserExpYear
		{
			set { user_Exp_Year = value; }
			get { return user_Exp_Year; }
		}

		#endregion

		public void LoadCountries(string filePath)
		{
			var countries = _subscriberProcess.GetCountries(filePath);

			var usIndex = countries.FindIndex(x => x.Countrycode == "840");
			var canadaIndex = countries.FindIndex(x => x.Countrycode == "124");
			
			// USA as first item
			var usItem = countries[usIndex];
			countries[usIndex] = countries[0];
			countries[0] = usItem;

			// Canada as second item
			var canadaItem = countries[canadaIndex];
			countries[canadaIndex] = countries[1];
			countries[1] = canadaItem;
			
			drpCountry.DataSource = countries;
			drpCountry.DataTextField = "Name";
			drpCountry.DataValueField = "ISOCode";
			drpCountry.DataBind();
		}
		
		protected void Page_Load(object sender, EventArgs e)
		{
            //Uncomment when ready -- Justin Welter 08232016
            //ClientScriptManager csm = Page.ClientScript;
            //btnSecurePayment.Attributes.Add("onclick", "javascript:" + btnSecurePayment.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSecurePayment, ""));
			MaintainScrollPositionOnPostBack = true;
			if (emptyPubCode()) return;

			if (!IsPostBack)
			{
				LoadCountries(CountryCodePath);

				try
				{
					_countryName = Country;
				}
				catch
				{
					_countryName = "USA";
				}
				_countryName = _subscriberProcess.SetCountryCode(_countryName);

				ShowHideSelect(true);
			}
			else
			{
				_countryName = _subscriberProcess.SetCountryCode(drpCountry.SelectedItem.Text);
			}

			try
			{
				PubCode = Server.HtmlEncode(PubCode);
			}
			catch
			{
			}

			try
			{
				foreach (ListItem li in drpCountry.Items)
				{
					if (li.Text.ToUpper() == _countryName.ToUpper())
					{
						li.Selected = true;
						break;
					}
				}
			}
			catch
			{
				drpCountry.Items[0].Selected = true;
			}

			if (!IsPostBack)
			{
				Publication pub = new Publication();
				loadShippingStates(_countryName);
				loadBillingStates(_countryName);
				if (_countryName.ToUpper() == Constants.UNITEDSTATESOFAMERICA || _countryName.ToUpper() == Constants.CANADA)
				{
					panelDropDownShippingState.Visible = true;
					panelTextBoxShippingState.Visible = false;
					if (Request.QueryString["state"] != null)
					{
						drpShippingState.SelectedIndex =
							drpShippingState.Items.IndexOf(
								drpShippingState.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
						drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;
					}
				}
				else
				{
					panelDropDownShippingState.Visible = false;
					panelTextBoxShippingState.Visible = true;
					if (Request.QueryString["state"] != null)
					{
						txtShippingState.Text = Request.QueryString["state"].ToString();
					}
				}

				try
				{
					pub = getPublicationByPubCode();

					if (pub == null || pub.PubCode.Trim().Length == 0)
					{
						phError.Visible = true;
						pnlSubscriptionPageDisplay.Visible = false;
						lblErrorMessage.Text = "Invalid Publication";
						return;
					}
					else
					{
						phError.Visible = false;
						pnlSubscriptionPageDisplay.Visible = true;

						phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));
						phFooter.Controls.Add(new LiteralControl(pub.FooterHTML));
					}
					
				}
				catch (Exception ex)
				{
					phError.Visible = true;
					pnlSubscriptionPageDisplay.Visible = false;
					lblErrorMessage.Text = "Invalid Publication";
					return;
				}


				setQueryStringsToControls();

				if (!String.IsNullOrWhiteSpace(QueryStringEmail) && !String.IsNullOrWhiteSpace(QueryStringZip))
				{
					setDefaultPricesOnForm(pub);	
				}

				try
				{
					pub = getPublicationByPubCode();
				}
				catch (Exception ex)
				{
					phError.Visible = true;
					pnlSubscriptionPageDisplay.Visible = false;
					lblErrorMessage.Text = "Invalid Publication";
					return;
				}
				setDefaultPricesOnForm(pub);
				
			}
		}


		protected void txtemail_OnTextChanged(object sender, EventArgs e)
		{
			Publication pub;
			try
			{
				pub = getPublicationByPubCode();
			}
			catch (Exception ex)
			{
				phError.Visible = true;
				pnlSubscriptionPageDisplay.Visible = false;
				lblErrorMessage.Text = "Invalid Publication";
				return;
			}
			
			setDefaultPricesOnForm(pub);
		}

		public static bool Contains(string source, string toCheck, StringComparison comp)
		{
			return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
		}
		private void setDefaultPricesOnForm(Publication pub)
		{
			_magazine = _subscriberProcess.GetMagazineList(FilePath);

			SourcemediaPubcode sourceMediaPubCode = _magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

			ECNLastYearAmountsTermDatesPremiumChecks ecnLastYearAmountsTermDatesPremiumChecks = null;

			ecnLastYearAmountsTermDatesPremiumChecks = _subscriberProcess.GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, Filter, ConnectionString);

			if (sourceMediaPubCode != null)
			{
				phError.Visible = false;
				pnlSubscriptionPageDisplay.Visible = true;

				lblStandardCoverImage.ImageUrl = sourceMediaPubCode.StandardCoverImage;
				lblPremiumCoverImage.ImageUrl = sourceMediaPubCode.PremiumCoverImage;

				getVisibilityOfPremiumsAndStandardSubscriptions(sourceMediaPubCode.HasPremium, ecnLastYearAmountsTermDatesPremiumChecks.IsPremium);
			}
			else
			{
				phError.Visible = true;
				pnlSubscriptionPageDisplay.Visible = false;
				lblErrorMessage.Text = "Invalid Publication";
				return;

			}
			try
			{

				var amountPaidLastYear = ecnLastYearAmountsTermDatesPremiumChecks.LastYearAmounts.Count > 0 ? ecnLastYearAmountsTermDatesPremiumChecks.LastYearAmounts.Max() : 0.0;

				getDefaultPricesAndAmountPaidLastYear(sourceMediaPubCode, amountPaidLastYear);

				SetStandardPricesAndTotals();

				SetPremiumTaxesAndTotals();

				IsPremiumVisible = false;

				IsStandardVisible = false;
			}
			catch (Exception ex)
			{
				makeErrorVisibleAndHideSubscriptionPanelDisplay();
				return;
			}
		}

		private void getVisibilityOfPremiumsAndStandardSubscriptions(bool sourceMediaPubCodePremium, bool mediaDefPremium)
		{
			plPremium.Visible = sourceMediaPubCodePremium;
			plStandard.Visible = !mediaDefPremium;
		}

		private void setQueryStringsToControls()
		{
			txtfirstname.Text = QueryStringFirstName;
			txtlastname.Text = QueryStringLastName;
			txtcompany.Text = QueryStringCompany;

			txtBillingAddress.Text = QueryStringAddress1;
			txtBillingAddress2.Text = QueryStringAddress2;
			txtBillingCity.Text = QueryStringCity;
			txtBillingZip.Text = QueryStringZip;

			txtShippingAddress.Text = QueryStringAddress1;
			txtShippingAddress2.Text = QueryStringAddress2;
			txtShippingCity.Text = QueryStringCity;
			txtShippingZip.Text = QueryStringZip;

			drpBillingState.SelectedIndex = drpBillingState.Items.IndexOf(drpBillingState.Items.FindByValue(QueryStringState));
			drpBillingState.Items[drpBillingState.SelectedIndex].Selected = true;


			drpShippingState.SelectedIndex = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(QueryStringState));
			drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;


			txtphone.Text = QueryStringPhone;
			fax.Text = QueryStringFax;
			txtemail.Text = QueryStringEmail;

			if (!String.IsNullOrWhiteSpace(txtShippingZip.Text))
			{
				var ctryName = this.drpCountry.SelectedItem.Text;
				ShippingZip = txtShippingZip.Text;
				var addressComponent = populateSelectedShippingState();

				bindGridData(ctryName, addressComponent);
			}
		}

		private bool displayErrorMessageBasedOnEmailEmpty()
		{
			if (String.IsNullOrWhiteSpace(QueryStringEmail))
			{
				makeErrorVisibleAndHideSubscriptionPanelDisplay();
			}
			return makeErrorNotVisibleAndDisplaySubscriotionDisplayPanel();
		}

		private bool makeErrorNotVisibleAndDisplaySubscriotionDisplayPanel()
		{
			phError.Visible = false;
			pnlSubscriptionPageDisplay.Visible = true;
			return false;
		}

		private void makeErrorVisibleAndHideSubscriptionPanelDisplay()
		{
			pnlSubscriptionPageDisplay.Visible = false;
			phError.Visible = true;
			lblErrorMessage.Text = Constants.ErrorMessageFormsOnlyForRequal;
		}

		private bool emptyPubCode()
		{
			if (String.IsNullOrWhiteSpace(PubCode))
			{
				pnlSubscriptionPageDisplay.Visible = false;
				phError.Visible = true;
				lblErrorMessage.Text = Constants.ErrorMessageInavlaidSubscription; 
				return true;
			}
			return makeErrorNotVisibleAndDisplaySubscriotionDisplayPanel();
		}

		private Publication getPublicationByPubCode()
		{
			Publication pub = new Publication();
			if (IsCacheClear())
			{
                if (CacheUtil.IsCacheEnabled())
                {
                    if (CacheUtil.GetFromCache("Pub_" + PubCode.ToUpper(), "JOINTFORMS") != null)
                    {
                        CacheUtil.RemoveFromCache("Pub_" + PubCode.ToUpper(), "JOINTFORMS");
                    }
                }
				UpdateCacheClear();
			}
			pub = Publication.GetPublicationbyID(0, PubCode);
			return pub;
		}

		private double getAmountPaidLastYear(List<Double> lastYearAmounts  )
		{
			var amountPaidLastYear = lastYearAmounts.Count > 0 ? lastYearAmounts.Max() : 0.0;

			return amountPaidLastYear;
		}

		private void getDefaultPricesAndAmountPaidLastYear(SourcemediaPubcode sourcemediaPubcode, double amountPaidLastYear)
		{
			var countryName = drpCountry.SelectedItem.Text;
			var countryCode = _subscriberProcess.GetByCountryCodeByName(countryName, CountryCodePath);

			getCountresPriceTerms(sourcemediaPubcode, amountPaidLastYear, countryCode);

			bindStandardPremiumAndPriceTexts(sourcemediaPubcode);

			if (_standardTerms.Count == 0 || _premiumTerms.Count == 0)
			{
				return;
			}
			defaultBindingsForTerms();
		}

		private void defaultBindingsForTerms()
		{
			drpStandardTerm.SelectedValue = _standardTerms[0];
			drpPremiumTerm.SelectedValue = _premiumTerms[0];

			if (!rbStandardPrint.Checked && !rbPremiumPrint.Checked)
			{
				IsStandardVisible = false;

				IsPremiumVisible = false;
			}

			if (rbStandardPrint.Checked)
			{
				IsStandardVisible = true;
				IsPremiumVisible = false;
			}

			if (rbPremiumPrint.Checked)
			{
				IsStandardVisible = false;
				IsPremiumVisible = true;
			}
		}

		private void getCountresPriceTerms(SourcemediaPubcode sourcemediaPubcode, double amountPaidLastYear,
			string countryCode, string selectedStandardTerm = "", string selectedPremiumTerm = "", string selectedStateCode = "",
			string selectedZipCode = "")
		{
			Taxjar taxjar = null;

			var prices = _subscriberProcess.GetPricesFromAmountPaidLastYear(sourcemediaPubcode, amountPaidLastYear);

			if (prices == null)
			{
				return;
			}

			var pricesForCountries = _subscriberProcess.GetPricesFromLastYearForCountry(prices, countryCode);

			if (pricesForCountries.Count == 0)
			{
				return;
			}

			_standardTerms = _subscriberProcess.GetStandardTermsFromPriceForCountries(pricesForCountries);

			_premiumTerms = _subscriberProcess.GetPremiumTermsFromPriceForCountries(pricesForCountries);
			
			int standardTerm = String.IsNullOrWhiteSpace(selectedStandardTerm) ? 1 : Convert.ToInt32(selectedStandardTerm);
			int premiumTerm = String.IsNullOrWhiteSpace(selectedPremiumTerm) ? 1 : Convert.ToInt32(selectedPremiumTerm);

			if (!String.IsNullOrWhiteSpace(QueryStringZip))
			{
				selectedZipCode = QueryStringZip;
			}

			if(!String.IsNullOrWhiteSpace(txtShippingZip.Text))
			{
				selectedZipCode = txtShippingZip.Text;
			}

			if (String.IsNullOrWhiteSpace(selectedZipCode))
			{
				taxjar = _subscriberProcess.GetTaxjarInfo(ApiKey, QueryStringZip, countryCode);
			}
			else
			{
				taxjar = _subscriberProcess.GetTaxjarInfo(ApiKey, selectedZipCode, countryCode);
			}


			var standardSubsdcription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, standardTerm.ToString(), sourcemediaPubcode, Filter, countryCode, Price.PriceType.Standard, ConnectionString, DateTime.Now, selectedZipCode, true);

			var premiumSubscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, premiumTerm.ToString(), sourcemediaPubcode, Filter, countryCode, Price.PriceType.Premium, ConnectionString, DateTime.Now, selectedZipCode, true);
			
			_standardPriceTax = standardSubsdcription.TaxAmount;

			_totalStandard = standardSubsdcription.TotalAmount;

			_standardPrice = standardSubsdcription.ItemPrice;

			if (sourcemediaPubcode.HasPremium)
			{
				_premiumPriceTax = premiumSubscription.TaxAmount;
				_totalPremium = premiumSubscription.TotalAmount;
				_premiumPrice = premiumSubscription.ItemPrice;
			}
			
		}


		private void bindStandardPremiumAndPriceTexts(SourcemediaPubcode sourcemediaPubcode)
		{
			drpStandardTerm.DataSource = _standardTerms;
			drpStandardTerm.DataBind();

			drpPremiumTerm.DataSource = _premiumTerms;
			drpPremiumTerm.DataBind();

			SetStandardPricesAndTotals();

			if (sourcemediaPubcode.HasPremium)
			{

				SetPremiumTaxesAndTotals();
			}
		}

		private bool IsCacheClear()
		{
			try
			{
				SqlCommand cmdGetCacheClear = new SqlCommand("select IsCacheClear from Publications where PubCode = @PubCode");
				cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
				return Convert.ToBoolean(KMPS_JF_Objects.Objects.DataFunctions.ExecuteScalar("", cmdGetCacheClear).ToString());
			}
			catch
			{
				return false;
			}
		}

		private void UpdateCacheClear()
		{
			try
			{
				SqlCommand cmdGetCacheClear = new SqlCommand("update Publications set IsCacheClear = 0 where PubCode = @PubCode");
				cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
				KMPS_JF_Objects.Objects.DataFunctions.Execute(cmdGetCacheClear);
			}
			catch
			{
			}
		}


		private void ShowHideSelect(bool status)
		{
			if (status == false && pnlCountry.Visible == true)
			{
				pnlCountry.Visible = !status;
			}
			else
			{
				pnlCountry.Visible = status;
			}
		}

		private List<SourceMediaPaidPub.Objects.MagazineXML> GetMagList()
		{
			List<SourceMediaPaidPub.Objects.MagazineXML> MagazineList = new List<SourceMediaPaidPub.Objects.MagazineXML>();

			MagazineList = (
				from e in XDocument.Load(Server.MapPath("xml/MagList.xml")).Root.Elements("Magazine")
				select new SourceMediaPaidPub.Objects.MagazineXML
				{
					CoverImage = (string) e.Element("CoverImage"),
					PubCode = (string) e.Element("PubCode"),
					GroupID = (int) e.Element("GroupID"),
					CustomerID = (int) e.Element("CustomerID"),
					Title = (string) e.Element("Title"),
					Term = (int) e.Element("Term"),
					IsPremium = (bool) e.Element("IsPremium"),
				})
				.ToList();

			return MagazineList.FindAll(x => x.PubCode.ToLower() == PubCode.ToLower());
		}

		protected void drpTerm_SelectedIndexChanged(Object sender, EventArgs e)
		{
			this._countryName = drpCountry.SelectedItem.Text;

			//foreach (GridViewRow row in grdMagazines.Rows)
			//{
			//    if (row.RowType == DataControlRowType.DataRow)
			//    {
			//        Label lblPrice1 = (Label)row.FindControl("lblPrice1");
			//        Label lblPrice2 = (Label)row.FindControl("lblPrice2");
			//        DropDownList drpTerm = (DropDownList)row.FindControl("drpTerm");

			//        switch (Convert.ToInt32(drpTerm.SelectedItem.Value))
			//        {
			//            case 1:
			//                lblPrice1.Visible = true;
			//                break;
			//            case 2:
			//                lblPrice2.Visible = true;
			//                break;
			//        }
			//    }
			//}
			//CalculatePrice();
		}

		private bool IsPrintEditionAvailable(string pubCode, string CountryName)
		{
			Publication pub = Publication.GetPublicationbyID(0, pubCode);
			string sql =
				"select pf.PFID from PubFormsForCountry pfc join PubForms pf on pf.PFID = pfc.PFID join Country c on c.CountryID = pfc.CountryID where c.CountryName = '" +
				drpCountry.SelectedItem.Text.ToString() + "' and pf.PubID = " + pub.PubID.ToString() + " and pf.ShowPrint = 0";
			DataTable dt = PaidPub.Objects.DataFunctions.GetDataTable(sql);

			if (dt.Rows.Count > 0)
				return false;
			else
				return true;
		}

		private void updateGridView(string countryName)
		{
			var countryCode = _subscriberProcess.GetByCountryCodeByName(_countryName, CountryCodePath);

			var pub = getPublicationByPubCode();

			List<double> lastYearAmounts;

			try
			{
				var ecnLastYearAmountsAndTermDates = _subscriberProcess.GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, Filter, ConnectionString);

				var amountPaidLastYear = ecnLastYearAmountsAndTermDates.LastYearAmounts.Count > 0 ? ecnLastYearAmountsAndTermDates.LastYearAmounts.Max() : 0.0;

				var magazine = _subscriberProcess.GetMagazineList(FilePath);

				SourcemediaPubcode sourceMediaPubCode = magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

				getDefaultPricesAndAmountPaidLastYear(sourceMediaPubCode, amountPaidLastYear);
			}
			catch (Exception ex)
			{
				makeErrorVisibleAndHideSubscriptionPanelDisplay();
				return;
			}
		}

		#region State Dropdown

		private void loadShippingStates(string countryName)
		{
			drpShippingState.Items.Clear();

			var items = _subscriberProcess.AddDefaultStates(countryName);

			foreach (var item in items)
			{
				drpShippingState.Items.Add(item);
			}
		}

		private void loadBillingStates(string countryName)
		{
			drpBillingState.Items.Clear();

			var items = _subscriberProcess.AddDefaultStates(countryName);

			foreach (var item in items)
			{
				drpBillingState.Items.Add(item);
			}
		}

		#endregion


		protected void btnSecurePayment_Click(object sender, EventArgs e)
		{

			var countryCode = _subscriberProcess.GetByCountryCodeByName(drpCountry.SelectedItem.Text, CountryCodePath);
			var taxjar = _subscriberProcess.GetTaxjarInfo(ApiKey, txtShippingZip.Text, countryCode);

			if (displayErrorMessageBasedOnEmailEmpty()) return;

			var addressComponent = getAddressComponent(BillingZip);

			if (panelDropDownBillingState.Visible)
			{
				if (drpBillingState.SelectedItem.Value != addressComponent.ShortName)
				{
					lblErrorMessage.Text = Constants.ChooseCorrectZipStateForBilling;
					phError.Visible = true;
					return;
				}
			}
			else
			{
				if (txtBillingState.Text != addressComponent.ShortName)
				{
					lblErrorMessage.Text = Constants.ChooseCorrectZipStateForBilling;
					phError.Visible = true;
					return;
				}
			}
		
			phError.Visible = false;
			var magazine = _subscriberProcess.GetMagazineList(FilePath);
			SourcemediaPubcode sourceMediaPubCode = magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

			bool redirectstatus = false;
			StringBuilder sb;
			StringBuilder profileSB = BuildProfile();
			
			double taxAmount = 0;
			int term = 0;
			string subscriptiontype = string.Empty;


			if (Page.IsValid)
			{
				HttpBrowserCapabilities browser = Request.Browser;
				string browserInfo =
					String.Format(
						"OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},Supports JavaScript={13}",
						browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta,
						browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript,
						browser.JavaScript);
				Dictionary<string, string> qString = new Dictionary<string, string>();
				string finalQstring = string.Empty;


				var pub = Publication.GetPublicationbyID(0, PubCode);

				int groupID = pub.ECNDefaultGroupID;
				int customerID = pub.ECNCustomerID;


				foreach (string key in Request.QueryString.AllKeys)
				{
					if (key.ToLower() != "g" && key.ToLower() != "c" && key.ToLower() != "sfid" && !qString.ContainsKey(key.ToLower()))
					{
						qString.Add(key.ToLower(), Request.QueryString[key].ToString());
						finalQstring += "&" + key + "=" + Request.QueryString[key].ToString();
					}
				}

				#region Post Regular Fields

				var selectedStandardTerm = ViewState["SelectedStandardTerm"] == null
					? drpStandardTerm.SelectedValue
					: ViewState["SelectedStandardTerm"].ToString();
				var selectedPremiumTerm = ViewState["PremiumStandardTerm"] == null
					? drpPremiumTerm.SelectedValue
					: ViewState["PremiumStandardTerm"].ToString();

				if (rbStandardPrint.Checked == true)
				{

					subscriptiontype = "Standard";
					term = int.Parse(selectedStandardTerm);
					totalAmount = double.Parse(ViewState["TotalStandards"].ToString(), NumberStyles.Currency);
					taxAmount = double.Parse(ViewState["StandardTaxes"].ToString(), NumberStyles.Currency);
				}
				else if (rbPremiumPrint.Checked == true)
				{

					subscriptiontype = "Premium";
					term = int.Parse(selectedPremiumTerm);
					totalAmount = double.Parse(ViewState["TotalPremium"].ToString(), NumberStyles.Currency);
					taxAmount = double.Parse(ViewState["PremiumPriceTax"].ToString(), NumberStyles.Currency);
				}
				else
				{
					phError.Visible = true;
					lblErrorMessage.Text = "Please select the magazine you want to subscribe.";
					return;
				}

				phError.Visible = false;


				string profile = profileSB.ToString() + "&user_PaymentStatus=pending&g=" + groupID + "&c=" + customerID +
				                 "&user_DEMO39=" + PromoCode + "&user_PUBLICATIONCODE=" + PubCode;

				PaidPub.Objects.Item localitem = new PaidPub.Objects.Item();


				//item.ItemID = 0;
				localitem.ItemCode = PubCode;
				localitem.ItemName = PubCode + " " + subscriptiontype;
				localitem.Description = PubCode + " " + subscriptiontype;
				localitem.ItemQty = term.ToString();
				localitem.ItemAmount = totalAmount.ToString();
				localitem.GroupID = groupID;
				localitem.CustID = customerID;
				itemList.Add(localitem);

				string qStringText = finalQstring + "&g=" + groupID + "&c=" + customerID;

				redirectstatus = ECNUtils.ECNHttpPost(txtemail.Text, PubCode, qStringText, browserInfo) &&
				                 ECNUtils.ECNHttpPost(txtemail.Text, PubCode, profile.ToString(), browserInfo);

				#endregion


				#region Authorize and Charge the credit cardCr

				if (redirectstatus)
				{
					if (String.IsNullOrEmpty(TransactionID))
					{
						if (totalAmount == 0)
							TransactionID = "00000000";
						else
						{
                            if (!ValidateCreditCard())
                            {
                                phError.Visible = true;
                                string msg = string.Empty;

                                Errorlocation = "btnsecurepayment - getting AuthorizeNetResponse responsecode and Message ";

                                if (AuthorizeNetResponse != null)
                                    lblErrorMessage.Text = AuthorizeNetResponse.ResponseCode + " " + AuthorizeNetResponse.Message;
                                else
                                    lblErrorMessage.Text = "Error Processing Credit Card.";

                                return;
                            }
                            else
                            {

                                Errorlocation = "btnsecurepayment - AuthorizeNet Process completed - getting TransactionID ";

                                TransactionID = AuthorizeNetResponse.TransactionID;

                            }

						}
					}
				}

				#endregion

				#region Post the transactional UDF

				var currentDateTime = _subscriberProcess.GetCurrentDateTime(DateTime.Now);

				var shippingZipCode = txtShippingZip.Text;

				if (totalAmount > 0)
				{
					bool isSubscriptionRenewal = sourceMediaPubCode.IsSubscriptionRenewal(Convert.ToInt32(customerID), txtemail.Text,
							Convert.ToInt32(groupID));

					Price.PriceType selectedType;
					selectedType = Price.PriceType.Standard;

					if (rbStandardPrint.Checked)
					{
						selectedType = Price.PriceType.Standard;
					} 
					else if (rbPremiumPrint.Checked)
					{
						selectedType = Price.PriceType.Premium;
					}


					var subscription = _subscriberProcess.CalculateTotalTaxAmountTermEndDate(taxjar, term.ToString(), sourceMediaPubCode,Filter, countryCode, selectedType, ConnectionString, currentDateTime, shippingZipCode, false);

					var ecnLastYearAmountsAndTermDates = _subscriberProcess.GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, Filter, ConnectionString);

					var defaultPostParams = new PostDataParams(groupID: groupID.ToString()
							, term: term
							, customerID: customerID.ToString()
							, email: txtemail.Text
							, cardHolderName: user_CardHolderName.Text
							, cardTypeValue: user_CCType.SelectedItem.Value
							, creditCardNumber: user_CCNumber.Text
							, userExpirationMonth: user_Exp_Month.SelectedItem.Value
							, userExpirationYear: user_Exp_Year.SelectedItem.Value
							, totalAmount: subscription.TotalAmount
							, taxAmount: subscription.TaxAmount
							, billingAddress: txtBillingAddress.Text
							, billingAddress2: txtBillingAddress2.Text
							, billingCity: txtBillingCity.Text
							, shippingAddress: txtShippingAddress.Text
							, shippingAddress2: txtShippingAddress2.Text
							, shippingCity: txtShippingCity.Text
							, selectedshippingState: drpShippingState.SelectedValue
							, shippingZip: txtShippingZip.Text
							, countryCode: drpCountry.SelectedItem.Text
							, selectedBillingState: drpBillingState.SelectedValue
							, billingZip: txtBillingZip.Text
							, transactionID: TransactionID
							, isSubscriptionRenewal: isSubscriptionRenewal
							, termStartDate: subscription.TermStartDate
							, termEndDate: subscription.TermEndDate
							, itemPrice: subscription.ItemPrice
							, isPremium: ecnLastYearAmountsAndTermDates.IsPremium);

					//post to transactional UDF
					sb = _subscriberProcess.DefaultPostData(defaultPostParams);
					redirectstatus = ECNUtils.ECNHttpPost(txtemail.Text, subscription.PubCode, sb.ToString(), browserInfo);
					

					if (subscription.PremiumPubCodeDTOs.Count > 0 && redirectstatus)
					{
						foreach (var premiumCodeDto in subscription.PremiumPubCodeDTOs)
						{
							var premiumCodeParams = new PostDataParams(groupID: premiumCodeDto.Key.ToString()
							, term: term
							, customerID: premiumCodeDto.Value.CustomerId.ToString()
							, email: txtemail.Text
							, cardHolderName: user_CardHolderName.Text
							, cardTypeValue: user_CCType.SelectedItem.Value
							, creditCardNumber: user_CCNumber.Text
							, userExpirationMonth: user_Exp_Month.SelectedItem.Value
							, userExpirationYear: user_Exp_Year.SelectedItem.Value
							, totalAmount: premiumCodeDto.Value.TotalPrice
							, taxAmount: premiumCodeDto.Value.TaxPrice
							, billingAddress: txtBillingAddress.Text
							, billingAddress2: txtBillingAddress2.Text
							, billingCity: txtBillingCity.Text
							, shippingAddress: txtShippingAddress.Text
							, shippingAddress2: txtShippingAddress2.Text
							, shippingCity: txtShippingCity.Text
							, selectedshippingState: drpShippingState.SelectedValue
							, shippingZip: txtShippingZip.Text
							, countryCode: drpCountry.SelectedItem.Text
							, selectedBillingState: drpBillingState.SelectedValue
							, billingZip: txtBillingZip.Text
							, transactionID: TransactionID
							, isSubscriptionRenewal: isSubscriptionRenewal
							, termStartDate: subscription.TermStartDate
							, termEndDate: subscription.TermEndDate
							,itemPrice:premiumCodeDto.Value.ItemPrice
							, isPremium: ecnLastYearAmountsAndTermDates.IsPremium);

							//post to transactional UDF
							sb = _subscriberProcess.DefaultPostData(premiumCodeParams);
							redirectstatus = ECNUtils.ECNHttpPost(txtemail.Text, premiumCodeDto.Value.PubCode, sb.ToString(), browserInfo);
						}
						
					}

					#endregion


					if (redirectstatus)
					{
                        string TYRedirectQS = String.Format("Thankyou.aspx?pubcode={0}&emailaddress={5}&city={2}&state={3}&zip={4}&address={1}", PubCode, txtShippingAddress.Text, txtShippingCity.Text, drpShippingState.SelectedValue, txtShippingZip.Text, txtemail.Text);
                        Response.Redirect(TYRedirectQS, true);
					}
					else
					{
						phError.Visible = true;
						lblErrorMessage.Text = "Error posting the data. Please try again!!";
						return;
					}
				}
			}
		
}
		private NameValueCollection getPayPallCollection(string payPalInfo)
		{
			NameValueCollection payPalCollection = new System.Collections.Specialized.NameValueCollection();
			string[] ArrayReponses = payPalInfo.Split('&');

			for (int i = 0; i < ArrayReponses.Length; i++)
			{
				string[] Temp = ArrayReponses[i].Split('=');
				payPalCollection.Add(Temp[0], Temp[1]);
			}
			return payPalCollection;
		}

		private string showPayPalInfo(NameValueCollection collection)
		{
			string payPalInfo = "";
			foreach (string key in collection.AllKeys)
			{
				payPalInfo += "<br /><span class=\"bold-text\">" +
					key + ":</span> " + collection[key];
			}
			return payPalInfo;
		}


		private string formatPhoneNumber(string phone)
		{
			if (!phone.Contains("-"))
			{
				return phone.Substring(0, 3) + "-" + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
			}
			else
			{
				return phone;
			}
		}

        //private bool ValidateCreditCard_PayPal()
        //{
        //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //    PayflowConnectionData Connection = new PayflowConnectionData();

        //    string partner = string.Empty;
        //    string vendor = string.Empty;
        //    string user = string.Empty;
        //    string password = string.Empty;

        //    CultureInfo us = new CultureInfo("en-US");

        //    try
        //    {
        //        string amount = totalAmount.ToString();

        //        {
        //            Connection = new PayflowConnectionData("pilot-payflowpro.paypal.com", 443, 45, "", 0, "", "");

        //            partner = WebConfigurationManager.AppSettings["payflowproPartner"].ToString();
        //            vendor = WebConfigurationManager.AppSettings["payflowproVendor"].ToString();
        //            user = WebConfigurationManager.AppSettings["payflowproUser"].ToString();
        //            password = WebConfigurationManager.AppSettings["payflowproPassword"].ToString();

        //        }

        //        UserInfo User = new UserInfo(user, vendor, partner, password);


        //        string fullName = user_CardHolderName.Text;
        //        string[] fullNameDelimited = fullName.Split(new char[] { ' ' });
        //        string firstName = fullNameDelimited[0];
        //        string lastName = string.Empty;
        //        if (fullNameDelimited.Length > 1)
        //        {
        //            lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);
        //        }

        //        Invoice Inv = new Invoice();
        //        Currency Amt = new Currency(decimal.Parse(amount), "USD");
        //        Inv.Amt = Amt;
        //        Inv.InvoiceDate = DateTime.Now.ToString();

        //        string countryCode = "";
        //        if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA")
        //        {
        //            countryCode = "US";
        //        }
        //        else if (drpCountry.SelectedItem.Text.ToUpper() == "CANADA")
        //        {
        //            countryCode = "CA";
        //        }
        //        else
        //        {
        //            countryCode = drpCountry.SelectedItem.Text.ToUpper();
        //        }

        //        BillTo Bill = new BillTo();
        //        Bill.BillToFirstName = firstName;
        //        Bill.BillToLastName = lastName;
        //        Bill.BillToStreet = txtBillingAddress.Text;
        //        Bill.BillToStreet2 = txtBillingAddress2.Text;
        //        Bill.BillToCity = txtBillingCity.Text;

        //        if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "CANADA")
        //        {
        //            Bill.BillToState = drpBillingState.SelectedItem.Value;
        //        }
        //        else
        //        {
        //            Bill.BillToState = txtBillingState.Text;
        //        }

        //        Bill.BillToZip = txtBillingZip.Text;
        //        Bill.BillToPhone = txtphone.Text;
        //        Bill.BillToEmail = txtemail.Text;
        //        Bill.BillToCountry = countryCode;
        //        Inv.BillTo = Bill;


        //        ShipTo ship = new ShipTo();

        //        ship.ShipToEmail = txtemail.Text;
        //        ship.ShipToFirstName = txtfirstname.Text;
        //        ship.ShipToLastName = txtlastname.Text;
        //        ship.ShipToStreet = txtShippingAddress.Text;
        //        ship.ShipToStreet2 = txtShippingAddress2.Text;
        //        ship.ShipToCity = txtShippingCity.Text;

        //        if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "CANADA")
        //        {
        //            ship.ShipToState = drpShippingState.SelectedItem.Value;
        //        }
        //        else
        //        {
        //            ship.ShipToState = drpShippingState.Text;
        //        }

        //        ship.ShipToZip = txtShippingZip.Text;
        //        ship.ShipToPhone = txtphone.Text;
        //        ship.ShipToCountry = countryCode;

        //        Inv.ShipTo = ship;

        //        string Pubcodes = string.Empty;

        //        foreach (PaidPub.Objects.Item item in itemList)
        //        {
        //            PayPal.Payments.DataObjects.LineItem li = new PayPal.Payments.DataObjects.LineItem();

        //            li.Name = item.ItemName + "(" + item.ItemCode + ")";
        //            li.ItemNumber = item.ItemCode;
        //            li.Desc = item.Description;
        //            li.Qty = long.Parse(item.ItemQty);
        //            li.Cost = new Currency(decimal.Parse(item.ItemAmount), "USD");
        //            li.Amt = new Currency(decimal.Parse(item.ItemAmount), "USD"); ;

        //            Inv.AddLineItem(li);

        //            Pubcodes += (Pubcodes == string.Empty ? item.ItemCode + "($" + item.ItemAmount + ")" : "," + item.ItemCode + "($" + item.ItemAmount + ")");
        //        }

        //        Inv.Comment1 = Pubcodes;
        //        Inv.Comment2 = txtfirstname.Text + " " + txtlastname.Text;

        //        string CardNo = Regex.Replace(user_CCNumber.Text, @"[ -/._#]", "");
        //        CreditCard cc = new CreditCard(CardNo, user_Exp_Month.SelectedItem.Value + user_Exp_Year.SelectedItem.Text.Substring(2, 2));
        //        cc.Cvv2 = user_CCVerfication.Text;

        //        CardTender Card = new CardTender(cc);

        //        // Create a new Sale Transaction.
        //        SaleTransaction Trans = new SaleTransaction(
        //            User, Connection, Inv, Card, PayflowUtility.RequestId);

        //        Response Resp = Trans.SubmitTransaction();

        //        if (Resp != null)
        //        {
        //            // Get the Transaction Response parameters.
        //            TrxnResponse = Resp.TransactionResponse;
        //            if (TrxnResponse.Result == 0)
        //                return true;
        //            else
        //                return false;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        string emailMsg = "Error when Processing PayPal flow Credit Card..<br /><br />";
        //        emailMsg += txtemail.Text;
        //        emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
        //        //emailMsg += "<b>Error Response:</b>" + Resp.ResponseString + "<br /><br />";
        //        emailMsg += "<b>Exception Details:</b>" + ex.Message;

        //        EmailFunctions emailFunctions = new EmailFunctions();
        //        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal Transaction Response-FAIL", emailMsg);

        //        throw ex;
        //    }
        //}

        

        private bool ValidateCreditCard()
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard_AuthorizeNet()";

            try
            {
                string MerchantId;
                string Signature;

                MerchantId = WebConfigurationManager.AppSettings["AuthorizeDotNetLogin"].ToString();
                Signature = WebConfigurationManager.AppSettings["AuthorizeDotNetKey"].ToString();
             
                string countrCode = "";
                int totalQuantity = 0;
                string orderdesc = "";
                string orderNames = "";
                string orderItems = string.Empty;
                if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA")
                {
                    countrCode = "US";
                }
                else if (drpCountry.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    countrCode = "CA";
                }
                else
                {
                    countrCode = drpCountry.SelectedItem.Text.ToUpper();
                }
                string fullName = user_CardHolderName.Text;
                string[] fullNameDelimited = fullName.Split(new char[] { ' ' });
                string firstName = fullNameDelimited[0];
                string lastName = string.Empty;
                if (fullNameDelimited.Length > 1)
                    lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);

                string CardNo = Regex.Replace(user_CCNumber.Text, @"[ -/._#]", "");
                CardNo = CardNo.Trim().Replace(" ", "");
                var gateway = new Gateway(MerchantId, Signature);
                gateway.TestMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["AuthorizeDotNetDemoMode"].ToString());

                AuthorizeNetRequest.CardNum = CardNo;
                AuthorizeNetRequest.ExpDate = user_Exp_Month.Text + user_Exp_Year.Text;
                AuthorizeNetRequest.Amount = totalAmount.ToString();
                AuthorizeNetRequest.AddInvoice(PromoCode);
                AuthorizeNetRequest.AddCardCode(user_CCVerfication.Text);
                AuthorizeNetRequest.Country = countrCode;
                AuthorizeNetRequest.Email = txtemail.Text;
                AuthorizeNetRequest.CustomerIp = System.Web.HttpContext.Current.Request.UserHostAddress;
                AuthorizeNetRequest.City = txtBillingCity.Text;

                if (this.itemList.Count > 0)
                {
                    string SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, txtemail.Text);

                    if (countrCode.ToUpper() == "US" || countrCode.ToUpper() == "CA")
                    {
                        AuthorizeNetRequest.AddCustomer(SubscriberID, txtemail.Text, firstName, lastName, txtBillingAddress.Text + " " + txtBillingAddress2.Text, txtBillingCity.Text, drpBillingState.SelectedItem.Value, txtBillingZip.Text);
                        AuthorizeNetRequest.AddShipping(SubscriberID, txtemail.Text, txtfirstname.Text, txtlastname.Text, txtShippingAddress.Text + " " + txtShippingAddress2.Text, txtShippingCity.Text, drpShippingState.SelectedItem.Value, txtShippingZip.Text);
                    }
                    else
                    {
                        AuthorizeNetRequest.AddCustomer(SubscriberID, txtemail.Text, firstName, lastName, txtBillingAddress.Text + " " + txtBillingAddress2.Text, txtBillingCity.Text, txtBillingState.Text, txtBillingZip.Text);
                        AuthorizeNetRequest.AddShipping(SubscriberID, txtemail.Text, txtfirstname.Text, txtlastname.Text, txtShippingAddress.Text + " " + txtShippingAddress2.Text, txtShippingCity.Text, txtShippingState.Text, txtShippingZip.Text);
                    }
                    AuthorizeNetRequest.ShipToCity = txtShippingCity.Text;
                    AuthorizeNetRequest.ShipToCountry = countrCode;
                }
                foreach (PaidPub.Objects.Item item in itemList)
                {
                    orderItems += item.ItemCode + ",";
                    orderdesc += item.Description + ",";
                    orderNames += item.ItemName + ",";
                    totalQuantity += Convert.ToInt32(item.ItemQty);
                }

                string itemID = (orderItems.Trim(',').Length > 30 ? orderItems.Trim(',').Substring(0, 30) : orderItems.Trim(','));
                string itemName = (orderNames.Trim(',').Length > 30 ? orderNames.Trim(',').Substring(0, 30) : orderNames.Trim(','));
                string itemDesc = orderdesc.Trim(',');
                decimal itemPrice = Convert.ToDecimal(AuthorizeNetRequest.Amount) / Convert.ToDecimal(totalQuantity);
                AuthorizeNetRequest.AddLineItem(itemID, itemName, itemDesc, 1, Convert.ToDecimal(AuthorizeNetRequest.Amount), false);
                AuthorizeNetRequest.AddMerchantValue("x_Description", itemDesc);

                Errorlocation = "btnsecurepayment - ValidateCreditCard_AuthorizeNet() - before notifyAuthorizationRequest(AuthorizeNetRequest)";

                notifyAuthorizationRequest(AuthorizeNetRequest);

                Errorlocation = "btnsecurepayment - ValidateCreditCard_AuthorizeNet() - before (GatewayResponse)gateway.Send(AuthorizeNetRequest)";

                AuthorizeNetResponse = (GatewayResponse)gateway.Send(AuthorizeNetRequest);

                Errorlocation = "btnsecurepayment - ValidateCreditCard_AuthorizeNet() - after (GatewayResponse)gateway.Send(AuthorizeNetRequest)";

                if (!AuthorizeNetResponse.Approved)
                {
                    Errorlocation = "btnsecurepayment - ValidateCreditCard_AuthorizeNet() - !AuthorizeNetResponse.Approved - sending TransactionFail Email";

                    notifyTransactionFail(AuthorizeNetResponse, null);
                }

                return AuthorizeNetResponse.Approved;
            }
            catch (Exception ex)
            {
                notifyTransactionFail(AuthorizeNetResponse, ex);
                throw ex;
            }
        }

        private void notifyAuthorizationRequest(AuthorizationRequest requestObject)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<b>Source Media Paid Forms  :</b> <BR>");

                sb.AppendLine("<b>URL : </b>" + Request.Url.ToString() + "<br /><br />");
                sb.AppendLine("<b>Pubcode  :</b>  " + PubCode + "<BR>");

                sb.AppendLine("=============CARD INFO==========<BR><BR>");

                if (requestObject.CardNum.Length >= 15)
                    sb.AppendLine("<b>Card Number  :</b>  " + requestObject.CardNum.Substring(requestObject.CardNum.Length - 5, 4) + "<BR>");
                else
                    sb.AppendLine("Card Number  :</b>  " + requestObject.CardNum + "<BR>");
                sb.AppendLine("<b>Card Type  :</b>  " + user_CCType.SelectedItem.Value + "<BR>");
                sb.AppendLine("<b>Expiration Date  :</b>  " + requestObject.ExpDate + "<BR>");
                sb.AppendLine("<b>Card Code  :</b>  " + requestObject.CardCode + "<BR>");
                sb.AppendLine("<b>Amount  :</b>  " + requestObject.Amount + "<BR>");

                sb.AppendLine("<BR/>");

                sb.AppendLine("<b>====Customer Information====</b>" + "<BR>");
                sb.AppendLine("<b>CustId  :</b>  " + requestObject.CustId + "<BR>");
                sb.AppendLine("<b>Email  :</b>  " + requestObject.Email + "<BR>");
                sb.AppendLine("<b>FirstName  :</b>  " + requestObject.FirstName + "<BR>");
                sb.AppendLine("<b>LastName  :</b>  " + requestObject.LastName + "<BR>");
                sb.AppendLine("<b>Address  :</b>  " + requestObject.Address + "<BR>");
                sb.AppendLine("<b>City  :</b>  " + requestObject.City + "<BR>");
                sb.AppendLine("<b>State  :</b>  " + requestObject.State + "<BR>");
                sb.AppendLine("<b>Zip  :</b>  " + requestObject.Zip + "<BR>");
                sb.AppendLine("<b>Country  :</b>  " + requestObject.Country + "<BR>");
                sb.AppendLine("<b>Phone  :</b>  " + requestObject.Phone + "<BR>");
                sb.AppendLine("<b>Fax  :</b>  " + requestObject.Fax + "<BR>");

                sb.AppendLine("<b>====Shipping Information====</b>" + "<BR>");
                sb.AppendLine("<b>ShipToFirstName  :</b>  " + requestObject.ShipToFirstName + "<BR>");
                sb.AppendLine("<b>ShipToLastName  :</b>  " + requestObject.ShipToLastName + "<BR>");
                sb.AppendLine("<b>ShipToAddress  :</b>  " + requestObject.ShipToAddress + "<BR>");
                sb.AppendLine("<b>ShipToCity  :</b>  " + requestObject.ShipToCity + "<BR>");
                sb.AppendLine("<b>ShipToState  :</b>  " + requestObject.ShipToState + "<BR>");
                sb.AppendLine("<b>ShipToCountry  :</b>  " + requestObject.ShipToCountry + "<BR>");
                sb.AppendLine("<b>ShipToZip  :</b>  " + requestObject.ShipToZip + "<BR>");

                sb.AppendLine("<b>====Misc====</b><BR>");
                sb.AppendLine("<b>Company  :</b>  " + requestObject.Company + "<BR>");
                sb.AppendLine("<b>Invoice  :</b>  " + requestObject.InvoiceNum + "<BR>");
                sb.AppendLine("<b>CustomerIp  :</b>  " + requestObject.CustomerIp + "<BR>");
                sb.AppendLine("<b>TransactionId  :</b>  " + requestObject.TransId + "<BR>");
                sb.AppendLine("<b>UserAgent  :</b>  " + System.Web.HttpContext.Current.Request.UserAgent + "<BR>");


                string adminEmailbody = sb.ToString();
                ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Authorize.Net Transaction Request", adminEmailbody);
            }
            catch
            {
                Errorlocation = "btnsecurepayment -  notifyAuthorizationRequest(AuthorizeNetRequest) - in catch";
            }
        }

		private StringBuilder BuildProfile()
		{
			StringBuilder profile = new StringBuilder();
			profile.Append("fn=" + txtfirstname.Text);
			profile.Append("&ln=" + txtlastname.Text);
			profile.Append("&compname=" + txtcompany.Text);
			profile.Append("&adr=" + txtShippingAddress.Text);
			profile.Append("&adr2=" + txtShippingAddress2.Text);
			profile.Append("&city=" + txtShippingCity.Text);
			var countryCode = _subscriberProcess.GetByCountryCodeByName(_countryName, CountryCodePath);

			if (_countryName.ToUpper() == "UNITED STATES OF AMERICA" || _countryName.ToUpper() == "CANADA")
				profile.Append("&state=" + drpShippingState.SelectedValue);
			else
				profile.Append("&state=" + txtShippingState.Text);

			profile.Append("&zc=" + txtShippingZip.Text);
			profile.Append("&ctry=" + countryCode);
			profile.Append("&ph=" + txtphone.Text);
			profile.Append("&fax=" + fax.Text);
			profile.Append("&e=" + txtemail.Text);
			profile.Append("&f=html");
			return profile;
		}
		private void notifyTransactionFail(GatewayResponse responseObject, Exception ex)
		{
			StringBuilder sb = new StringBuilder();
			if (responseObject != null)
			{
				sb.AppendLine("<b>====Response Information====</b>" + "<BR>");
				sb.AppendLine("<b>TransactionID  :</b>  " + responseObject.TransactionID + "<BR>");
				sb.AppendLine("<b>Amount  :</b>  " + responseObject.Amount + "<BR>");
				sb.AppendLine("<b>AuthorizationCode  :</b>  " + responseObject.AuthorizationCode + "<BR>");
				sb.AppendLine("<b>TransactionType  :</b>  " + responseObject.TransactionType + "<BR>");
				sb.AppendLine("<b>RawResponse  :</b>  " + responseObject.RawResponse.ToString() + "<BR>");
				sb.AppendLine("<b>ResponseCode  :</b>  " + responseObject.ResponseCode + "<BR>");
				sb.AppendLine("<b>Error  :</b>  " + responseObject.Error + "<BR>");
				sb.AppendLine("<b>Message  :</b>  " + responseObject.Message + "<BR>");

				sb.AppendLine("<b>Description  :</b>  " + responseObject.Description + "<BR>");
				sb.AppendLine("<b>CardType  :</b>  " + responseObject.CardType + "<BR>");

				if (responseObject.CardNumber.Length >= 15)
					sb.AppendLine("<b>CardNum  :</b>  " + responseObject.CardNumber.Substring(responseObject.CardNumber.Length - 5, 4) +
					              "<BR>");
				else
					sb.AppendLine("<b>CardNum  :</b>  " + responseObject.CardNumber + "<BR>");

				sb.AppendLine("<b>CAVResponse  :</b>  " + responseObject.CAVResponse + "<BR>");
				sb.AppendLine("<b>CCVResponse  :</b>  " + responseObject.CCVResponse + "<BR>");
				sb.AppendLine("<b>AVSResponse  :</b>  " + responseObject.AVSResponse + "<BR>");
				sb.AppendLine("<b>Address  :</b>  " + responseObject.Address + "<BR>");
				sb.AppendLine("<b>QueryStringCity  :</b>  " + responseObject.City + "<BR>");
				sb.AppendLine("<b>State  :</b>  " + responseObject.State + "<BR>");
				sb.AppendLine("<b>Country  :</b>  " + responseObject.Country + "<BR>");
				sb.AppendLine("<b>ZipCode  :</b>  " + responseObject.ZipCode + "<BR>");

				sb.AppendLine("<b>====Shipping Information====</b>" + "<BR>");
				sb.AppendLine("<b>ShipFirstName  :</b>  " + responseObject.ShipFirstName + "<BR>");
				sb.AppendLine("<b>ShipLastName  :</b>  " + responseObject.ShipLastName + "<BR>");
				sb.AppendLine("<b>ShipAddress  :</b>  " + responseObject.ShipAddress + "<BR>");
				sb.AppendLine("<b>ShipCity  :</b>  " + responseObject.ShipCity + "<BR>");
				sb.AppendLine("<b>ShipState  :</b>  " + responseObject.ShipState + "<BR>");
				sb.AppendLine("<b>ShipCountry  :</b>  " + responseObject.ShipCountry + "<BR>");
				sb.AppendLine("<b>ShipZipCode  :</b>  " + responseObject.ShipZipCode + "<BR>");
			}
			else
			{
				sb.AppendLine("<b>GatewayResponse is NULL</b>" + "<BR>");
			}

			if (ex != null)
			{
				sb.AppendLine("<b>Exception Message</b>" + "<BR>");
				sb.AppendLine(ex.Message + "<BR>");

				sb.AppendLine("<b>Exception Stack Trace</b>" + "<BR>");
				sb.AppendLine(ex.StackTrace + "<BR>");
			}
			string adminEmailbody = sb.ToString();
            ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
			emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"],
				ConfigurationManager.AppSettings["Admin_FromEmail"], "Authorize.Net Transaction Response-FAIL", adminEmailbody);
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			user_CardHolderName.Text = "";
			user_CCType.SelectedIndex = user_CCType.Items.IndexOf(user_CCType.Items.FindByValue(""));
			user_CCNumber.Text = "";
			user_Exp_Month.SelectedIndex = user_Exp_Month.Items.IndexOf(user_Exp_Month.Items.FindByValue(""));
			user_Exp_Year.SelectedIndex = user_Exp_Year.Items.IndexOf(user_Exp_Year.Items.FindByValue(""));
			user_CCVerfication.Text = "";

			txtfirstname.Text = Request.QueryString["fn"] != null ? Request.QueryString["fn"].ToString() : "";
			txtlastname.Text = Request.QueryString["ln"] != null ? Request.QueryString["ln"].ToString() : "";
			txtcompany.Text = Request.QueryString["compname"] != null ? Request.QueryString["compname"].ToString() : "";
			txtBillingAddress.Text = Request.QueryString["adr"] != null ? Request.QueryString["adr"].ToString() : "";
			txtBillingAddress2.Text = Request.QueryString["adr2"] != null ? Request.QueryString["adr2"].ToString() : "";
			txtBillingCity.Text = Request.QueryString["city"] != null ? Request.QueryString["city"].ToString() : "";
			txtBillingState.Text = Request.QueryString["zc"] != null ? Request.QueryString["zc"].ToString() : "";
			txtphone.Text = Request.QueryString["ph"] != null ? Request.QueryString["ph"].ToString() : "";
			fax.Text = Request.QueryString["fax"] != null ? Request.QueryString["fax"].ToString() : "";
			txtemail.Text = Request.QueryString["e"] != null ? Request.QueryString["e"].ToString() : "";
			txtemail.ReadOnly = Request.QueryString["e"] != null ? true : false;

			drpBillingState.SelectedIndex = 0;
			drpShippingState.SelectedIndex = 0;


			txtShippingAddress.Text = String.Empty;
			txtShippingAddress2.Text = String.Empty;
			txtShippingCity.Text = String.Empty;
			txtShippingZip.Text = String.Empty;


			if (Request.QueryString["state"] != null)
			{
				drpShippingState.SelectedIndex =
					drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
				drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;
			}
			else
			{
				drpShippingState.SelectedIndex = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(""));
				drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;
			}

			phError.Visible = false;
		}

		protected void grdMagazines_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				Label lblIsPremium = (Label) e.Row.FindControl("lblIsPremium");
				Label lblPrice1 = (Label) e.Row.FindControl("lblPrice1");
				Label lblPrice2 = (Label) e.Row.FindControl("lblPrice2");

				DropDownList drpTerm = (DropDownList) e.Row.FindControl("drpTerm");
				Label lblTerm = (Label) e.Row.FindControl("lblTerm");
				SourceMediaPaidPub.Objects.MagazineXML magazineXml = null;

				int term = 0;
				int rowIndex = Convert.ToInt32(hfProductCount.Value);

				magazineXml = GetMagList().Find(x => x.IsPremium == bool.Parse(lblIsPremium.Text));

				term = Convert.ToInt32(magazineXml.Term);

				if (drpTerm != null && !lblTerm.Visible)
				{
					for (int i = 1; i <= term; i++)
						drpTerm.Items.Add(new ListItem(i == 1 ? i.ToString() + " year" : i.ToString() + " years", i.ToString()));

					drpTerm.ClearSelection();
					drpTerm.Items[0].Selected = true;
				}
				rowIndex = rowIndex + 1;
				hfProductCount.Value = rowIndex.ToString();
			}
		}

		protected void ListStandardTermChanged(object sender, EventArgs e)
		{
			var selectedStandardTerm = drpStandardTerm.SelectedItem.Text;
			ViewState["SelectedStandardTerm"] = selectedStandardTerm;

			var pub = getPublicationByPubCode();


			try
			{
				var ecnLastYearAmountsAndTermDates = _subscriberProcess.GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, Filter, ConnectionString);

				var amountPaidLastYear = ecnLastYearAmountsAndTermDates.LastYearAmounts.Count > 0 ? ecnLastYearAmountsAndTermDates.LastYearAmounts.Max() : 0.0;

				var magazine = _subscriberProcess.GetMagazineList(FilePath);

				var ctryName = drpCountry.SelectedItem.Text;

				var countryCode = _subscriberProcess.GetByCountryCodeByName(ctryName, CountryCodePath);


				SourcemediaPubcode sourceMediaPubCode = magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

				getDefaultPricesAndAmountPaidLastYear(sourceMediaPubCode, amountPaidLastYear);

				string stateCode;

				stateCode = drpBillingState.SelectedItem == null ? String.Empty : drpBillingState.SelectedItem.Value;

				var zipCode = txtShippingZip.Text;

				getCountresPriceTerms(sourceMediaPubCode, amountPaidLastYear, countryCode, selectedStandardTerm, String.Empty,
					stateCode, zipCode);
				drpStandardTerm.DataSource = _standardTerms;
				drpStandardTerm.SelectedValue = _standardTerms.SingleOrDefault(x => x.Equals(selectedStandardTerm));
				drpStandardTerm.DataBind();

				SetStandardPricesAndTotals();

			}
			catch (Exception ex)
			{
				makeErrorVisibleAndHideSubscriptionPanelDisplay();
			}
		}

		protected void ListPremiumTermChanged(object sender, EventArgs e)
		{
			var premiumStandardTerm = drpPremiumTerm.SelectedItem.Text;
			ViewState["PremiumStandardTerm"] = premiumStandardTerm;

			var pub = getPublicationByPubCode();
			try
			{
				var ecnLastYearAmountsAndTermDates = _subscriberProcess.GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, Filter, ConnectionString);

				var amountPaidLastYear = ecnLastYearAmountsAndTermDates.LastYearAmounts.Count > 0 ? ecnLastYearAmountsAndTermDates.LastYearAmounts.Max() : 0.0;

				var magazine = _subscriberProcess.GetMagazineList(FilePath);

				var ctryName = this.drpCountry.SelectedItem.Text;

				var countryCode = _subscriberProcess.GetByCountryCodeByName(ctryName, CountryCodePath);

				SourcemediaPubcode sourceMediaPubCode = magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

				string stateCode;

				stateCode = drpShippingState.SelectedItem == null ? String.Empty : drpShippingState.SelectedItem.Value;

				var zipCode = txtShippingZip.Text;

				getCountresPriceTerms(sourceMediaPubCode, amountPaidLastYear, countryCode, String.Empty, premiumStandardTerm,
					stateCode, zipCode);

				drpPremiumTerm.DataSource = _premiumTerms;
				drpPremiumTerm.SelectedValue = _premiumTerms.SingleOrDefault(x => x.Equals(premiumStandardTerm));
				drpPremiumTerm.DataBind();

				SetPremiumTaxesAndTotals();
			}
			catch (Exception ex)
			{
				makeErrorVisibleAndHideSubscriptionPanelDisplay();
			}
		
		}

		protected void ListCountryChanged(object sender, EventArgs e)
		{
			hfProductCount.Value = "0";
			ShowHideSelect(true);
			this._countryName = drpCountry.SelectedItem.Text;

			updateGridView(_countryName);

			if (_countryName.ToUpper() == Constants.UNITEDSTATESOFAMERICA || _countryName.ToUpper() == Constants.CANADA)
			{
				panelDropDownShippingState.Visible = true;
				panelTextBoxShippingState.Visible = false;

				panelDropDownBillingState.Visible = true;
				panelTextBoxBillingState.Visible= false;

			}
			else
			{
				panelDropDownShippingState.Visible = false;
				panelTextBoxShippingState.Visible = true;

				panelDropDownBillingState.Visible = false;
				panelTextBoxBillingState.Visible =true;

			}

			loadShippingStates(_countryName);
			loadBillingStates(_countryName);
			txtBillingZip.Text = String.Empty;
			txtShippingZip.Text = String.Empty;

			ShippingZip = String.Empty;
		}


		protected void rbStandardCheckChanged(object sender, EventArgs e)
		{
			rbPremiumPrint.Checked = false;
			IsStandardVisible = true;
			IsPremiumVisible = false;
			
		}

		protected void rbPremimCheckChanged(object sender, EventArgs e)
		{
			rbStandardPrint.Checked = false;
			IsPremiumVisible = true;
			IsStandardVisible = false;
		}

		protected void stateChange(object sender, EventArgs e)
		{
			MaintainScrollPositionOnPostBack = true;
			if (rbStandardPrint.Checked)
			{
				IsStandardVisible = true;
				IsPremiumVisible = false;
			}
			else if (rbPremiumPrint.Checked)
			{
				IsPremiumVisible = true;
				IsStandardVisible = false;
			}
			else
			{
				IsPremiumVisible = false;
				IsStandardVisible = false;
			}

			var stateDropDown = (DropDownList) sender;

			if (stateDropDown.ID == "drpShippingState")
			{
				txtShippingZip.Text = String.Empty;
				txtShippingZip.Focus();
				lblStandardTaxes.Text = String.Empty;
				ViewState["StandardTaxes"] = String.Empty;
				lbltotalStandards.Text = String.Empty;
				ViewState["TotalStandards"] = String.Empty;
				lblPremiumTaxes.Text = String.Empty;
				ViewState["PremiumPriceTax"] = String.Empty;
				lbltotalPremiums.Text = String.Empty;
				ViewState["TotalPremium"] = String.Empty;
			}
			else if (stateDropDown.ID == "drpBillingState")
			{
				txtBillingZip.Text = String.Empty;
				txtBillingZip.Focus();
			}
		}

		protected void ZipTextChanged(object sender, EventArgs e)
		{
			MaintainScrollPositionOnPostBack = true;
			if (displayErrorMessageBasedOnEmailEmpty()) return;

			var txtBox = (TextBox) sender;

			var ctryName = this.drpCountry.SelectedItem.Text;

			var isValid = _subscriberProcess.IsValidUSandCanadianZipCodes(txtBox.Text);

			AddressComponent addressComponent = new AddressComponent();

			if (!isValid)
			{
				if (panelDropDownShippingState.Visible)
				{
					drpShippingState.SelectedIndex = 0;
					return;
				}
				txtShippingState.Text = String.Empty;
				return;
			}
			if (txtBox.ID == "txtShippingZip")
			{
				ShippingZip = txtBox.Text;
				addressComponent = populateSelectedShippingState();

				bindGridData(ctryName, addressComponent);
			}
			if (txtBox.ID == "txtBillingZip")
			{
				BillingZip = txtBox.Text;
				addressComponent = populateBillingStates();
			}
		}

		private void bindGridData(string ctryName, AddressComponent addressComponent)
		{
			var pub = getPublicationByPubCode();

			List<double> lastYearAmounts;

			try
			{
				var ecnLastYearAmountsAndTermDates = _subscriberProcess.GetLastYearAmountAndTermStartDateAndEndDateFromECN(pub, Filter, ConnectionString);

				var amountPaidLastYear = ecnLastYearAmountsAndTermDates.LastYearAmounts.Count > 0 ? ecnLastYearAmountsAndTermDates.LastYearAmounts.Max() : 0.0;

				var magazine = _subscriberProcess.GetMagazineList(FilePath);

				var countryCode = _subscriberProcess.GetByCountryCodeByName(ctryName, CountryCodePath);


				SourcemediaPubcode sourceMediaPubCode = magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

				getCountresPriceTerms(sourceMediaPubCode, amountPaidLastYear, countryCode, string.Empty, string.Empty,
					addressComponent.ShortName, ShippingZip);

				if (_standardTerms.Count == 0)
				{
					return;
				}

				drpStandardTerm.DataSource = _standardTerms;
				drpStandardTerm.DataBind();

				SetStandardPricesAndTotals();

				if (sourceMediaPubCode != null && sourceMediaPubCode.HasPremium)
				{
					drpPremiumTerm.DataSource = _premiumTerms;
					drpPremiumTerm.DataBind();
					if (_premiumTerms.Count == 0)
					{
						return;
					}
					SetPremiumTaxesAndTotals();
				}
			}
			catch (Exception ex)
			{
				makeErrorVisibleAndHideSubscriptionPanelDisplay();
			}
		}

		private void SetStandardPricesAndTotals()
		{
			lblStandardPrice.Text = string.Format("{0:C}", _standardPrice);
			lblStandardTaxes.Text = string.Format("{0:C}", _standardPriceTax);
			lbltotalStandards.Text = string.Format("{0:C}", _totalStandard);
			ViewState["StandardPrice"] = _standardPrice;
			ViewState["StandardTaxes"] = _standardPriceTax;
			ViewState["TotalStandards"] = _totalStandard;
		}

		private void SetPremiumTaxesAndTotals()
		{
			lblPremiumPrice.Text = string.Format("{0:C}", _premiumPrice);
			lblPremiumTaxes.Text = string.Format("{0:C}", _premiumPriceTax);
			lbltotalPremiums.Text = string.Format("{0:C}", _totalPremium);
			ViewState["PremiumPrice"] = _premiumPrice;
			ViewState["PremiumPriceTax"] = _premiumPriceTax;
			ViewState["TotalPremium"] = _totalPremium;

		}

		private AddressComponent populateBillingStates()
		{
			var addressComponent = getAddressComponent(BillingZip);
			drpBillingState.SelectedIndex = drpBillingState.Items.IndexOf(drpBillingState.Items.FindByValue(addressComponent.ShortName));
			if (drpBillingState.SelectedIndex != -1)
			{
				drpBillingState.Items[drpShippingState.SelectedIndex].Selected = true;
			}
			else
			{
				panelTextBoxBillingState.Visible = true;
			}
			return addressComponent;
		}

		private AddressComponent getAddressComponent(string zip)
		{
			var googleApi = _subscriberProcess.GetGoogleAPIInfo(zip);

			var addressComponent = getStateFromZipCode(googleApi);
			return addressComponent;
		}

		private AddressComponent populateSelectedShippingState()
		{
			var addressComponent = getAddressComponent(ShippingZip);
		
			drpShippingState.SelectedIndex = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(addressComponent.ShortName));
			if (drpShippingState.SelectedIndex != -1)
			{
				drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;
			}
			else
			{
				panelTextBoxShippingState.Visible = true;
			}

			return addressComponent;
		}

		private AddressComponent getStateFromZipCode(GoogleAPI googleApi)
		{
			AddressComponent addressComponent = null;
			foreach (var zipState in googleApi.ZipStates)
			{
				foreach (var adc in zipState.AddressComponents)
				{
					if (adc.types.Contains("administrative_area_level_1") && adc.types.Contains("political"))
					{
						return adc;
					}
				}
			}
			return null;
		}

		protected void btnCopyAddres_Click(object sender, EventArgs e)
		{
			var ctryName = this.drpCountry.SelectedItem.Text;
			Shipping shipping = _subscriberProcess.CopyAddress(txtBillingAddress.Text, txtBillingAddress2.Text,
				txtBillingCity.Text, txtBillingZip.Text, ctryName);
			txtShippingAddress.Text = shipping.ShippingAddress;
			txtShippingAddress2.Text = shipping.ShippingAddress2;
			txtShippingCity.Text = shipping.ShippingCity;
			txtShippingZip.Text = shipping.ShippingZip;

			if (!panelDropDownShippingState.Visible)
			{
				txtShippingState.Text = txtBillingState.Text;
			}

			if (!String.IsNullOrWhiteSpace(shipping.ShippingZip)){
				populateSelectedShippingState();
				var addressComponent = getAddressComponent(shipping.ShippingZip);
				bindGridData(ctryName, addressComponent);
			}
	
		}
	}
}