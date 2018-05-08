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
using ecn.communicator.classes;
using System.Web;
using Encore.PayPal;
using Encore.PayPal.Nvp;
using System.IO;

using PayPal.Payments.Common;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;
using System.Globalization;

using NXTBookAPI.BusinessLogic;
using NXTBookAPI.Entity;
using System.Net;
using System.Threading;

namespace PaidPub
{
    public partial class paidForm : System.Web.UI.Page
    {
        private double unitPrice = 0.0;
        private double totalPrice = 0.0;
        private string countryCode = String.Empty;

        private AuthorizationRequest AuthorizeNetRequest = new AuthorizationRequest("", "", 0.00M, "");
        private GatewayResponse AuthorizeNetResponse = null;
        private NvpDoDirectPayment PaypalRequest = new NvpDoDirectPayment();
        private TransactionResponse TrxnResponse = null;
        private List<PaidPub.Objects.Item> itemList = new List<PaidPub.Objects.Item>();
        private string Errorlocation = string.Empty;

        #region PROPERTIES

        public string PubCode
        {
            get
            {
                try
                {
                    return Request.QueryString["pubcode"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string TransactionCacheName
        {
            get
            {
                try
                {
                    return txtemail.Text.Trim() + "_" + getFormID().ToString() + "_TransactionProcessed";
                }
                catch
                {
                    return "";
                }
            }
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
                    try
                    {
                        return ViewState["user_promoCode"].ToString();
                    }
                    catch { return string.Empty; }
                }
            }
            set
            {
                ViewState["user_promoCode"] = value;
            }
        }

        public string WebSite
        {
            get
            {
                try
                {
                    return Request.QueryString["website"].ToString();
                }
                catch { return String.Empty; }
            }
        }

        public string Email
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
                catch { return string.Empty; }
            }
        }

        public string FirstName
        {
            get
            {
                try
                {
                    return Request.QueryString["fn"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string LastName
        {
            get
            {
                try
                {
                    return Request.QueryString["ln"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Company
        {
            get
            {
                try
                {
                    return Request.QueryString["compname"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Phone
        {
            get
            {
                try
                {
                    return Request.QueryString["ph"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Fax
        {
            get
            {
                try
                {
                    return Request.QueryString["fax"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Address1
        {
            get
            {
                try
                {
                    return Request.QueryString["adr"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Address2
        {
            get
            {
                try
                {
                    return Request.QueryString["adr2"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string City
        {
            get
            {
                try
                {
                    return Request.QueryString["city"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string State
        {
            get
            {
                try
                {
                    return Request.QueryString["state"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Zip
        {
            get
            {
                try
                {
                    return Request.QueryString["zc"].ToString();
                }
                catch { return string.Empty; }
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
                catch { return String.Empty; }
            }
        }

        public string Country
        {
            get
            {
                try
                {
                    return Request.QueryString["ctry"].ToString();
                }
                catch { return "UNITED STATES"; }
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
                catch { return string.Empty; }
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
                catch { return string.Empty; }
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

        public string VirtualDirectory
        {
            get
            {
                try
                {
                    return WebConfigurationManager.AppSettings["VirtualDirectory"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        #endregion

        private void LoadCountries()
        {
            drpCountry.DataSource = GetCountries();
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataValueField = "CountryID";
            drpCountry.DataBind();
            drpCountry.Items.Insert(0, new ListItem("United States", "205"));
            drpCountry.Items.Insert(1, new ListItem("Canada", "174"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScriptManager csm = Page.ClientScript;
            //btnSecurePayment.Attributes.Add("onclick", "javascript:" + btnSecurePayment.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSecurePayment, ""));
            if (!IsPostBack)
            {
                if (getFormID() == 0)
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "FormID is missing in the URL.";
                    pnlPaidForm.Visible = false;
                    return;
                }

                LoadCountries();
                countryCode = Country;
                loadstate(countryCode);
            }
            else
            {
                countryCode = drpCountry.SelectedItem.Text;
            }

            try
            {
                foreach (ListItem li in drpCountry.Items)
                {
                    if (li.Text.ToUpper() == countryCode.ToUpper())
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
                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA" || countryCode.ToUpper() == "UNITED STATES")
                {
                    pnlState1.Visible = true;
                    pnlState2.Visible = false;
                    pnlState3.Visible = true;
                    pnlState4.Visible = false;

                    if (Request.QueryString["state"] != null)
                    {
                        int indexOf = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                        if (indexOf < 0)
                        {
                            indexOf = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByText(Request.QueryString["state"].ToString()));
                        }
                        drpShippingState.SelectedIndex = indexOf;
                        drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;

                        indexOf = -1;
                        indexOf = drpBillingState.Items.IndexOf(drpBillingState.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                        if (indexOf < 0)
                        {
                            indexOf = drpBillingState.Items.IndexOf(drpBillingState.Items.FindByText(Request.QueryString["state"].ToString()));
                        }
                        drpBillingState.SelectedIndex = indexOf;
                        drpBillingState.Items[drpBillingState.SelectedIndex].Selected = true;
                    }
                }
                else
                {
                    pnlState1.Visible = false;
                    pnlState2.Visible = true;
                    pnlState3.Visible = false;
                    pnlState4.Visible = true;

                    if (Request.QueryString["state"] != null)
                    {
                        txtShippingState.Text = Request.QueryString["state"].ToString();
                        txtBillingState.Text = Request.QueryString["state"].ToString();
                    }
                }

                LoadForm();

                txtfirstname.Text = Request.QueryString["fn"] != null ? Request.QueryString["fn"].ToString() : "";
                txtlastname.Text = Request.QueryString["ln"] != null ? Request.QueryString["ln"].ToString() : "";
                txtcompany.Text = Request.QueryString["compname"] != null ? Request.QueryString["compname"].ToString() : "";

                txtBillingAddress.Text = Request.QueryString["adr"] != null ? Request.QueryString["adr"].ToString() : "";
                txtBillingAddress2.Text = Request.QueryString["adr2"] != null ? Request.QueryString["adr2"].ToString() : "";
                txtBillingCity.Text = Request.QueryString["city"] != null ? Request.QueryString["city"].ToString() : "";
                txtBillingZip.Text = Request.QueryString["zc"] != null ? Request.QueryString["zc"].ToString() : "";

                txtShippingAddress.Text = Request.QueryString["adr"] != null ? Request.QueryString["adr"].ToString() : "";
                txtShippingAddress2.Text = Request.QueryString["adr2"] != null ? Request.QueryString["adr2"].ToString() : "";
                txtShippingCity.Text = Request.QueryString["city"] != null ? Request.QueryString["city"].ToString() : "";
                txtShippingZip.Text = Request.QueryString["zc"] != null ? Request.QueryString["zc"].ToString() : "";
                txtphone.Text = Request.QueryString["ph"] != null ? Request.QueryString["ph"].ToString() : "";
                fax.Text = Request.QueryString["fax"] != null ? Request.QueryString["fax"].ToString() : "";
                txtemail.Text = Request.QueryString["e"] != null ? Request.QueryString["e"].ToString() : "";
                txtemail.ReadOnly = Request.QueryString["e"] != null ? true : false;


            }
            loadHeaderFooter();
        }

        private void loadHeaderFooter()
        {
            int formID = getFormID();
            DataTable dt = GetForm(formID);

            phHeader.Controls.Add(new LiteralControl(dt.Rows[0]["HeaderHTML"].ToString()));
            phFooter.Controls.Add(new LiteralControl(dt.Rows[0]["FooterHTML"].ToString()));
        }

        private void LoadForm()
        {
            int formID = getFormID();
            DataTable dt = GetForm(formID);
            ViewState["FormRedirect"] = dt.Rows[0]["FormRedirect"].ToString();
            ViewState["CustomerID"] = Convert.ToInt32(dt.Rows[0]["CustomerID"]).ToString();
            ViewState["ShowQuantity"] = Convert.ToBoolean(dt.Rows[0]["ShowQuantity"]).ToString();
            ViewState["ShowCountry"] = Convert.ToBoolean(dt.Rows[0]["ShowCountry"]).ToString();
            ViewState["UseTestMode"] = Convert.ToBoolean(dt.Rows[0]["UseTestMode"]).ToString();

            if (Convert.ToBoolean(dt.Rows[0]["UseTestMode"]))
            {
                pnlTESTMODE.Visible = true;
            }

            ViewState["PaymentGateway"] = dt.Rows[0]["PaymentGateway"].ToString();
            ViewState["AuthorizeDotNetLogin"] = dt.Rows[0]["AuthorizeDotNetLogin"].ToString();
            ViewState["AuthorizeDotNetKey"] = dt.Rows[0]["AuthorizeDotNetKey"].ToString();
            ViewState["PayflowAccount"] = dt.Rows[0]["PayflowAccount"].ToString();
            ViewState["PayflowPassword"] = dt.Rows[0]["PayflowPassword"].ToString();
            ViewState["PayflowSignature"] = dt.Rows[0]["PayflowSignature"].ToString();
            ViewState["PayflowPartner"] = dt.Rows[0]["PayflowPartner"].ToString();
            ViewState["PayflowVendor"] = dt.Rows[0]["PayflowVendor"].ToString();

            if (Convert.ToBoolean(dt.Rows[0]["ShowQuantity"]))
                ViewState["QuantityAllowed"] = Convert.ToInt32(dt.Rows[0]["QuantityAllowed"]).ToString();
            if (Convert.ToBoolean(dt.Rows[0]["ShowCountry"]) == true)
            {
                pnlCountry.Visible = true;
            }
            else
            {
                pnlCountry.Visible = false;
            }

            if (Convert.ToBoolean(dt.Rows[0]["AllowPromoCode"]) == true)
            {
                pnlPromoCode.Visible = true;
            }
            else
            {
                pnlPromoCode.Visible = false;
            }

            if (ViewState["PaymentGateway"].ToString().ToLower() == "paypalredirect" || ViewState["PaymentGateway"].ToString().ToLower() == "paypalflowredirect")
            {
                pnlCreditCardDetails.Visible = false;
                //btnSecurePayment.Click += new EventHandler(btnSecurePayment_Click);
            }
            else
            {

                //btnSecurePayment.OnClientClick = "doubleSubmitCheck(); return false;";
                //btnSecurePayment.Click += new EventHandler(btnSecurePayment_Click);
                //btnSecurePayment.Click -= new EventHandler(btnSecurePayment_Click);
            }

            if (ViewState["PaymentGateway"].ToString().ToLower() == "paypalflowredirect")
            {
                //hide billing and shipping data
                bill_ship_pnl.Visible = false;
            }

            if (ViewState["PaymentGateway"].ToString().ToLower() == "paypalredirect" || ViewState["PaymentGateway"].ToString().ToLower() == "paypalflowredirect")
            {
                lblProcessingPayment.Text = "Please wait, redirecting to PayPal";
                lblProcessingPaymentIE.Text = "Please wait, redirecting to PayPal";
            }

            LoadProducts(formID);
        }

        private void LoadProducts(int formID)
        {
            grdMagazines.DataSource = GetProducts(formID);
            grdMagazines.DataBind();
            grdMagazines.Columns[1].Visible = Convert.ToBoolean(ViewState["ShowQuantity"]);
        }

        private int getFormID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["FormID"].ToString());
            }
            catch { return 0; }
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.countryCode = drpCountry.SelectedItem.Text;
            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA" || countryCode.ToUpper() == "UNITED STATES")
            {
                pnlState1.Visible = true;
                pnlState2.Visible = false;
                pnlState3.Visible = true;
                pnlState4.Visible = false;
            }
            else
            {
                pnlState1.Visible = false;
                pnlState2.Visible = true;
                pnlState3.Visible = false;
                pnlState4.Visible = true;
            }
            loadstate(countryCode);
            LoadProducts(getFormID());
        }

        #region State Dropdown
        private void loadstate(string countrycode)
        {
            drpShippingState.Items.Clear();
            drpBillingState.Items.Clear();
            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES")
            {
                addlistitem("", "Select a State", "");
                addlistitem("AK", "Alaska", "USA");
                addlistitem("AL", "Alabama", "USA");
                addlistitem("AR", "Arkansas", "USA");
                addlistitem("AZ", "Arizona", "USA");
                addlistitem("CA", "California", "USA");
                addlistitem("CO", "Colorado", "USA");
                addlistitem("CT", "Connecticut", "USA");
                addlistitem("DC", "Washington D.C.", "USA");
                addlistitem("DE", "Delaware", "USA");
                addlistitem("FL", "Florida", "USA");
                addlistitem("GA", "Georgia", "USA");
                addlistitem("HI", "Hawaii", "USA");
                addlistitem("IA", "Iowa", "USA");
                addlistitem("ID", "Idaho", "USA");
                addlistitem("IL", "Illinois", "USA");
                addlistitem("IN", "Indiana", "USA");
                addlistitem("KS", "Kansas", "USA");
                addlistitem("KY", "Kentucky", "USA");
                addlistitem("LA", "Louisiana", "USA");
                addlistitem("MA", "Massachusetts", "USA");
                addlistitem("MD", "Maryland", "USA");
                addlistitem("ME", "Maine", "USA");
                addlistitem("MI", "Michigan", "USA");
                addlistitem("MN", "Minnesota", "USA");
                addlistitem("MO", "Missouri", "USA");
                addlistitem("MS", "Mississippi", "USA");
                addlistitem("MT", "Montana", "USA");
                addlistitem("NC", "North Carolina", "USA");
                addlistitem("ND", "North Dakota", "USA");
                addlistitem("NE", "Nebraska", "USA");
                addlistitem("NH", "New Hampshire", "USA");
                addlistitem("NJ", "New Jersey", "USA");
                addlistitem("NM", "New Mexico", "USA");
                addlistitem("NV", "Nevada", "USA");
                addlistitem("NY", "New York", "USA");
                addlistitem("OH", "Ohio", "USA");
                addlistitem("OK", "Oklahoma", "USA");
                addlistitem("OR", "Oregon", "USA");
                addlistitem("PA", "Pennsylvania", "USA");
                addlistitem("PR", "Puerto Rico", "USA");
                addlistitem("RI", "Rhode Island", "USA");
                addlistitem("SC", "South Carolina", "USA");
                addlistitem("SD", "South Dakota", "USA");
                addlistitem("TN", "Tennessee", "USA");
                addlistitem("TX", "Texas", "USA");
                addlistitem("UT", "Utah", "USA");
                addlistitem("VA", "Virginia", "USA");
                addlistitem("VT", "Vermont", "USA");
                addlistitem("WA", "Washington", "USA");
                addlistitem("WI", "Wisconsin", "USA");
                addlistitem("WV", "West Virginia", "USA");
                addlistitem("WY", "Wyoming", "USA");
            }
            else if (countryCode.ToUpper() == "CANADA")
            {
                addlistitem("", "Select a Province", "");
                addlistitem("AB", "Alberta", "Canada");
                addlistitem("BC", "British Columbia", "Canada");
                addlistitem("MB", "Manitoba", "Canada");
                addlistitem("NB", "New Brunswick", "Canada");
                addlistitem("NF", "New Foundland", "Canada");
                addlistitem("NS", "Nova Scotia", "Canada");
                addlistitem("ON", "Ontario", "Canada");
                addlistitem("PE", "Prince Edward Island", "Canada");
                addlistitem("QC", "Quebec", "Canada");
                addlistitem("SK", "Saskatchewan", "Canada");
                addlistitem("YT", "Yukon Territories", "Canada");
                //addlistitem("OT", "Other", "Foreign");
            }
        }

        private void addlistitem(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            drpShippingState.Items.Add(item);
            drpBillingState.Items.Add(item);

        }
        #endregion

        protected void btnCopyAddres_Click(object sender, EventArgs e)
        {

            txtShippingAddress.Text = txtBillingAddress.Text;
            txtShippingAddress2.Text = txtBillingAddress2.Text;
            txtShippingCity.Text = txtBillingCity.Text;
            txtShippingState.Text = txtBillingState.Text;

            try
            {
                drpShippingState.ClearSelection();
                drpShippingState.Items.FindByValue(drpBillingState.SelectedItem.Value).Selected = true;
            }
            catch
            { }

            txtShippingZip.Text = txtBillingZip.Text;
        }

        protected void btnSecurePayment_Click(object sender, EventArgs e)
        {

            String SubscriberID = string.Empty;
            string ErrorMessage = "Error processing the payment.";
            List<string> AutoSubpubCodes = new List<string>();

            try
            {
                Errorlocation = "btnsecurepayment - after try.";

                #region Page.IsValid

                if (Page.IsValid)
                {
                    string EncodedResponse = Request.Form["g-Recaptcha-Response"];

                    if (ConfigurationManager.AppSettings["ValidateCaptcha"].ToString().ToLower().Equals("true"))
                    {
                        if (!ReCaptchaClass.Validate(EncodedResponse))
                        {
                            phError.Visible = true;
                            lblErrorMessage.Text = "Invalid Captcha";
                            return;
                        }
                    }

                    // Get query string values to a dictionary

                    HttpBrowserCapabilities browser = Request.Browser;
                    string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                    #region NXTBook post checking

                    string NXTBookPassword = string.Empty; //store in User1 in email table.
                    bool IsNXTBookAPIEnabled = false;

                    if (PubCode.Length > 0)
                    {
                        Errorlocation = "btnsecurepayment - IsNXTBookAPIEnabled  ";

                        IsNXTBookAPIEnabled = NXTBook.IsNXTBookAPIEnabled(PubCode);

                        if (IsNXTBookAPIEnabled)
                        {
                            Errorlocation = "btnsecurepayment - NXTBookAPI is Enabled  ";

                            //Check if Password in User1 - if not, create it.
                            Publication pub = Publication.GetPublicationbyID(0, PubCode);

                            try
                            {
                                Errorlocation = "btnsecurepayment - getting NXTBookPassword from ECN";

                                NXTBookPassword = DataFunctions.ExecuteScalar("communicator", string.Format("select user1 from emails with (NOLOCK) where CustomerID = {0} and emailaddress = '{1}'", pub.ECNCustomerID, txtemail.Text)).ToString();
                            }
                            catch
                            {

                            }

                            if (string.IsNullOrEmpty(NXTBookPassword))
                            {
                                Errorlocation = "btnsecurepayment - generating NXTBookPassword";

                                NXTBookPassword = Utilities.CreatePassword(16);
                            }
                        }
                    }
                    #endregion

                    #region Build item objects

                    Dictionary<string, string> ECNFields = ECNUtils.GetECNProfileFields();

                    Dictionary<int, Dictionary<string, string>> dPurchasedProducts = new Dictionary<int, Dictionary<string, string>>();

                    foreach (GridViewRow row in grdMagazines.Rows)
                    {
                        PaidPub.Objects.Item item = new PaidPub.Objects.Item();

                        CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");

                        if (chkPrint.Checked == true)
                        {
                            Label groupID = (Label)row.FindControl("lblGroupID");
                            Label custID = (Label)row.FindControl("lblCustomerID");
                            Label lblProductID = (Label)row.FindControl("lblProductID");
                            Label lblProductName = (Label)row.FindControl("lblProductName");
                            Label lblPubCode = (Label)row.FindControl("lblPubcode");
                            Label lblProductDesc = (Label)row.FindControl("lblProductDesc");
                            Label lblIsSubscription = (Label)row.FindControl("lblIsSubscription");
                            DropDownList ddlTerm = (DropDownList)row.FindControl("drpTerm");

                            Label lblTotal = (Label)row.FindControl("lblTotal");
                            DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");

                            //string profile = profileSB.ToString() + "&user_PaymentStatus=pending&g=" + groupID.Text + "&c=" + custID.Text + "&user_DEMO39=" + PromoCode + "&user_PUBLICATIONCODE=" + lblPubCode.Text.ToString();
                            Product prod = Product.GetByProductID(Convert.ToInt32(lblProductID.Text.ToString()));

                            string quantity = "";
                            if (ddlQuantity.Items.Count > 0)
                                quantity = ddlQuantity.SelectedValue;

                            string term = "";
                            if (ddlTerm.Items.Count > 0)
                                term = ddlTerm.SelectedValue;

                            double totalAmt = double.Parse(GetProductPrice(prod.ProductID, term, quantity, Convert.ToInt32(drpCountry.SelectedValue.ToString()), PromoCode, false));
                            //double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));

                            item.ItemID = Convert.ToInt32(lblProductID.Text);
                            item.ItemCode = lblPubCode.Text;
                            item.ItemName = lblProductName.Text;
                            item.ItemDescription = lblProductDesc.Text;
                            item.ItemQty = ddlQuantity.Items.Count > 0 ? ddlQuantity.SelectedItem.Value : "1";
                            item.ItemAmount = totalAmt.ToString();
                            item.GroupID = int.Parse(groupID.Text);
                            item.CustID = int.Parse(custID.Text);

                            itemList.Add(item);

                            Dictionary<string, string> dsubscriberdata = new Dictionary<string, string>();

                            dsubscriberdata.Add("issubscription", lblIsSubscription.Text);

                            if (!string.IsNullOrWhiteSpace(txtfirstname.Text))
                                dsubscriberdata.Add("firstname", txtfirstname.Text);
                            if (!string.IsNullOrWhiteSpace(txtlastname.Text))
                                dsubscriberdata.Add("lastname", txtlastname.Text);
                            if (!string.IsNullOrWhiteSpace(txtcompany.Text))
                                dsubscriberdata.Add("company", txtcompany.Text);
                            if (!string.IsNullOrWhiteSpace(txtShippingAddress.Text))
                                dsubscriberdata.Add("address", txtShippingAddress.Text);
                            if (!string.IsNullOrWhiteSpace(txtShippingAddress2.Text))
                                dsubscriberdata.Add("address2", txtShippingAddress2.Text);
                            if (!string.IsNullOrWhiteSpace(txtShippingCity.Text))
                                dsubscriberdata.Add("city", txtShippingCity.Text);

                            if ((countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA") && !string.IsNullOrWhiteSpace(drpShippingState.SelectedValue))
                                dsubscriberdata.Add("state", drpShippingState.SelectedValue);
                            else if (!string.IsNullOrWhiteSpace(txtShippingState.Text))
                                dsubscriberdata.Add("state", txtShippingState.Text);

                            if (!string.IsNullOrWhiteSpace(txtShippingZip.Text))
                                dsubscriberdata.Add("zip", txtShippingZip.Text);
                            if (!string.IsNullOrWhiteSpace(countryCode))
                                dsubscriberdata.Add("country", countryCode);

                            if (!string.IsNullOrWhiteSpace(txtphone.Text))
                                dsubscriberdata.Add("voice", txtphone.Text);
                            if (!string.IsNullOrWhiteSpace(fax.Text))
                                dsubscriberdata.Add("fax", fax.Text);

                            dsubscriberdata.Add("emailaddress", txtemail.Text);
                            dsubscriberdata.Add("format", "html");

                            dsubscriberdata.Add("isnxtbookapienabled", IsNXTBookAPIEnabled.ToString());

                            if (IsNXTBookAPIEnabled)
                            {
                                dsubscriberdata.Add("usr1", NXTBookPassword);
                            }

                            dsubscriberdata.Add("paymentstatus", "paid");
                            dsubscriberdata.Add("paidorfree", "PAID");
                            dsubscriberdata.Add("pubcode", lblPubCode.Text);
                            dsubscriberdata.Add("publicationcode", lblPubCode.Text);

                            //get data from querystring
                            //removing this pubcode check and moving it into payflowpropayment.aspx.cs
                            //if (lblPubCode.Text.ToLower() == PubCode.ToLower())
                            //{
                            foreach (string key in Request.QueryString.AllKeys)
                            {
                                if (key != null
                                    && key.ToLower() != "g"
                                    && key.ToLower() != "c"
                                    && key.ToLower() != "sfid"
                                    && key.ToLower() != "fn"
                                    && key.ToLower() != "ln"
                                    && key.ToLower() != "e"
                                    && key.ToLower() != "adr"
                                    && key.ToLower() != "city"
                                    && key.ToLower() != "state"
                                    && key.ToLower() != "zc"
                                    && key.ToLower() != "ctry"
                                    && key.ToLower() != "ph"
                                    && key.ToLower() != "fax"
                                    && key.ToLower() != "s"
                                    && key.ToLower() != "f"
                                    && key.ToLower() != "formID"
                                    && key.ToLower() != "pfid"
                                    && key.ToLower() != "btx_i"
                                    && key.ToLower() != "btx_m"
                                    && key.ToLower() != "nqcountry"
                                    && key.ToLower() != "pubcode"
                                    && key.ToLower() != "qn"
                                    && key.ToLower() != "qv"
                                    && key.ToLower() != "step"
                                    && key.ToLower() != "t"
                                    && key.ToLower() != "user_paidorfree"
                                    && key.ToLower() != "user_paymentstatus"
                                    && key.ToLower() != "user_publicationcode"
                                    && !dsubscriberdata.ContainsKey(key.ToLower())
                                    && !dsubscriberdata.ContainsKey(key.ToLower().Replace("user_", "")))
                                {
                                    if (key.ToLower().StartsWith("user_"))
                                    {
                                        dsubscriberdata.Add(key.ToLower().Replace("user_", ""), Request.QueryString[key].ToString());
                                    }
                                    else
                                    {
                                        dsubscriberdata.Add(key.ToLower(), Request.QueryString[key].ToString());
                                    }
                                }
                                else if (key.ToLower().Equals("pubcode"))
                                {
                                    dsubscriberdata.Add("cf_pubcode", Request.QueryString[key].ToString());
                                }
                            }
                            //}

                            //Transaction UDF data

                            if (pnlCreditCardDetails.Visible)
                            {

                                dsubscriberdata.Add("t_fullname", user_CardHolderName.Text);

                                if (user_CardHolderName.Text.Trim().Contains(" "))
                                {
                                    string fullName = user_CardHolderName.Text;
                                    string firstName = fullName.Split(new char[] { ' ' })[0];
                                    string lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);

                                    dsubscriberdata.Add("t_firstname", firstName);
                                    dsubscriberdata.Add("t_lastname", lastName);
                                }
                                else
                                {
                                    dsubscriberdata.Add("t_firstname", user_CardHolderName.Text);
                                    dsubscriberdata.Add("t_lastname", "");
                                }

                                dsubscriberdata.Add("t_cardtype", user_CCType.SelectedItem.Value);
                                dsubscriberdata.Add("t_cardnumber", "************" + user_CCNumber.Text.Substring(user_CCNumber.Text.Trim().Length - 4, 4));
                                dsubscriberdata.Add("t_expirationdate", String.Format("{0}/{1}", user_Exp_Month.SelectedItem.Value, user_Exp_Year.SelectedItem.Value));
                            }



                            dsubscriberdata.Add("t_transdate", DateTime.Now.ToString("MM/dd/yyyy"));
                            dsubscriberdata.Add("t_street", txtBillingAddress.Text);
                            dsubscriberdata.Add("t_street2", txtBillingAddress2.Text);
                            dsubscriberdata.Add("t_city", txtBillingCity.Text);

                            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" ||  countryCode.ToUpper() == "CANADA")
                            {
                                dsubscriberdata.Add("t_state", drpBillingState.SelectedValue);
                            }
                            else
                            {
                                dsubscriberdata.Add("t_state", txtBillingState.Text);
                            }

                            dsubscriberdata.Add("t_zip", txtBillingZip.Text);
                            dsubscriberdata.Add("t_country", countryCode);

                            dsubscriberdata.Add("shipto_address1", txtShippingAddress.Text);
                            dsubscriberdata.Add("shipto_address2", txtShippingAddress2.Text);
                            dsubscriberdata.Add("shipto_city", txtShippingCity.Text);

                            if (pnlState1.Visible)
                            {
                                dsubscriberdata.Add("shipto_state", drpShippingState.SelectedItem.Value);
                                dsubscriberdata.Add("shipto_zip", txtShippingZip.Text);
                            }
                            else
                            {
                                dsubscriberdata.Add("shipto_state_int", txtShippingState.Text);
                                dsubscriberdata.Add("shipto_forzip", txtShippingZip.Text);
                            }

                            dsubscriberdata.Add("c", custID.Text);
                            dsubscriberdata.Add("g", groupID.Text);


                            if (Convert.ToBoolean(dsubscriberdata["issubscription"]))
                            {
                                DropDownList drpTerm = (DropDownList)row.FindControl("drpTerm");

                                dsubscriberdata.Add("t_term", drpTerm.SelectedValue);
                                dsubscriberdata.Add("t_termstartdate", DateTime.Now.ToString("MM/dd/yyyy"));
                                dsubscriberdata.Add("t_termenddate", DateTime.Now.AddYears(Convert.ToInt32(drpTerm.SelectedValue)).ToString("MM/dd/yyyy"));
                            }


                            RadioButtonList rblShipping = (RadioButtonList)row.FindControl("rblShipping");
                            bool airMail = Convert.ToBoolean(rblShipping.SelectedValue);
                            if (airMail)
                            {
                                dsubscriberdata.Add("t_airmailpaid", "YES");
                            }

                            dsubscriberdata.Add("t_amountpaid", String.Format("{0:0.00}", totalAmt));
                            dsubscriberdata.Add("t_renewal", IsSubscriptionRenewal(Convert.ToInt32(custID.Text), txtemail.Text, Convert.ToInt32(groupID.Text)).ToString());

                            dPurchasedProducts.Add(Convert.ToInt32(lblProductID.Text), dsubscriberdata);

                        }
                    }
                    #endregion

                    if (itemList.Count == 0)
                    {
                        phError.Visible = true;
                        lblErrorMessage.Text = "Please select the product you wish to purchase.";
                        return;
                    }

                    #region Authorize and Charge the credit card

                    Errorlocation = "btnsecurepayment - Begin Authorize and Charge the credit card";

                    if (String.IsNullOrEmpty(TransactionID) && GetFromCache(TransactionCacheName) == null)
                    {
                        string total = double.Parse(((Label)grdMagazines.FooterRow.FindControl("lblTotalAmount")).Text).ToString().TrimStart('$');
                        decimal totalPrice = 0.0M;
                        foreach (PaidPub.Objects.Item i in itemList)
                        {
                            //calculate actual total
                            decimal current = decimal.Parse(i.ItemAmount);
                            totalPrice = totalPrice + current;
                        }
                        total = totalPrice.ToString().TrimStart('$');
                        if (total == "0")
                            TransactionID = "00000000";
                        else
                        {
                            bool skipPaymentPateway = false;

                            try
                            {
                                skipPaymentPateway = Convert.ToBoolean(ConfigurationManager.AppSettings["skipPaymentPateway"]);

                                if (!Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                                    skipPaymentPateway = false;
                            }
                            catch
                            {
                                skipPaymentPateway = false;
                            }

                            if (!skipPaymentPateway)
                            {
                                if (ViewState["PaymentGateway"].ToString().ToLower() == "paypalflowredirect")
                                {
                                    Errorlocation = "btnsecurepayment - paypalredirect";
                                    ValidateCreditCard_PaypalflowRedirect(dPurchasedProducts);

                                    return;
                                }
                                else if (ViewState["PaymentGateway"].ToString().ToLower() == "paypalredirect")
                                {
                                    Errorlocation = "btnsecurepayment - paypalredirect";

                                    #region paypal redirect

                                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                                    NvpSetExpressCheckout ppSet = new NvpSetExpressCheckout();

                                    if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                                    {
                                        ppSet.Environment = NvpEnvironment.Sandbox;
                                        ppSet.Credentials.Username = WebConfigurationManager.AppSettings["PayflowAccount"].ToString();
                                        ppSet.Credentials.Password = WebConfigurationManager.AppSettings["PayflowPassword"].ToString();
                                        ppSet.Credentials.Signature = WebConfigurationManager.AppSettings["PayflowSignature"].ToString();
                                    }
                                    else
                                    {
                                        ppSet.Credentials.Username = ViewState["PayflowAccount"].ToString();
                                        ppSet.Credentials.Password = ViewState["PayflowPassword"].ToString();
                                        ppSet.Credentials.Signature = ViewState["PayflowSignature"].ToString();
                                    }

                                    string fullName = txtfirstname.Text + " " + txtlastname.Text;
                                    string fullAddress = txtBillingAddress.Text;
                                    string address2 = txtBillingAddress2.Text;

                                    ppSet.Add(NvpSetExpressCheckout.Request._AMT, total);
                                    ppSet.Add(NvpSetExpressCheckout.Request.LANDINGPAGE, NvpLandingPageType.Billing);
                                    ppSet.Add(NvpSetExpressCheckout.Request.PAYMENTACTION, NvpPaymentActionCodeType.Sale);

                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTONAME, fullName);
                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOCITY, txtShippingCity.Text);
                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOZIP, txtShippingZip.Text);
                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET, fullAddress.Replace("+", " "));
                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTREET2, address2);


                                    if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
                                    {
                                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTATE, drpShippingState.SelectedItem.Value);
                                    }
                                    else
                                    {
                                        ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOSTATE, txtShippingState.Text);
                                    }

                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOCOUNTRYCODE, countryCode);
                                    ppSet.Add(NvpSetExpressCheckout.Request.EMAIL, txtemail.Text);
                                    ppSet.Add(NvpSetExpressCheckout.Request.SHIPTOPHONENUM, txtphone.Text);

                                    List<NvpPayItem> NvpPayItemList = new List<NvpPayItem>();

                                    string groupID = string.Empty;

                                    foreach (PaidPub.Objects.Item item in itemList)
                                    {
                                        NvpPayItem npi = new NvpPayItem();
                                        npi.Number = item.ItemID.ToString();
                                        npi.Name = item.ItemCode + " / " + item.ItemName + " / ";
                                        npi.Description = item.ItemDescription;
                                        npi.Quantity = item.ItemQty;
                                        npi.Amount = item.ItemAmount;
                                        //item.Tax = decimal.Parse("0.00").ToString("f");
                                        NvpPayItemList.Add(npi);

                                        groupID += (groupID == string.Empty ? item.GroupID.ToString() : "," + item.GroupID.ToString());
                                    }

                                    ppSet.Add(NvpPayItemList);

                                    ppSet.Add(NvpSetExpressCheckout.Request.SOLUTIONTYPE, NvpSolutionTypeType.Sole);

                                    string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;

                                    ppSet.Add(NvpSetExpressCheckout.Request._RETURNURL, basePath + "/Forms/Payment.aspx?formID=" + getFormID() + "&emailaddress=" + Server.UrlEncode(txtemail.Text));

                                    ppSet.Add(NvpSetExpressCheckout.Request._CANCELURL, basePath + "/Forms/Cancel.aspx?formID=" + getFormID());

                                    if (ppSet.Post())
                                    {
                                        AddToCache(txtemail.Text.ToLower().Trim() + "_" + getFormID().ToString() + "_PurchasedProducts", dPurchasedProducts);

                                        //HttpPost(PostParams.Replace("user_PAIDorFREE=NONQUALIFIED", "user_PAIDorFREE=PENDING") + "&c=" + pub.ECNCustomerID.ToString() + "&ei=" + (EmailID > 0 && hidTRANSACTIONTYPE.Value != "NEW" ? EmailID.ToString() : "") + groupParams + PaidParams + ProfileParams + DemographicParams, false);
                                        Response.Redirect(ppSet.RedirectUrl);
                                    }
                                    else
                                    {
                                        //ppSet post failed
                                        lblErrorMessage.Text = "Payment redirect failed.";
                                        foreach (NvpError p in ppSet.ErrorList)
                                        {
                                            lblErrorMessage.Text += "<BR />" + p.LongMessage;
                                        }

                                        foreach (NvpParameter p in ppSet.ResponseList)
                                        {
                                            if (p.Name.Contains("EXCEPTION"))
                                                lblErrorMessage.Text += "<BR />" + p.Value;
                                        }

                                        lblErrorMessage.Visible = true;
                                        phError.Visible = true;
                                        return;
                                    }
                                    #endregion
                                }
                                else if (!ValidateCreditCard())
                                {
                                    #region Credit Card Payment gateway errors

                                    Errorlocation = "btnsecurepayment - inside !ValidateCreditCard() - Card Validation failed ";

                                    phError.Visible = true;
                                    string msg = string.Empty;

                                    if (ViewState["PaymentGateway"].ToString().Equals("AuthorizeNet", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Errorlocation = "btnsecurepayment - getting AuthorizeNetResponse responsecode and Message ";

                                        if (AuthorizeNetResponse != null)
                                            lblErrorMessage.Text = AuthorizeNetResponse.ResponseCode + " " + AuthorizeNetResponse.Message;
                                        else
                                            lblErrorMessage.Text = "Error Processing Credit Card.";
                                    }
                                    else if (ViewState["PaymentGateway"].ToString().Equals("Paypal", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Errorlocation = "btnsecurepayment - getting Paypal ResponseString";

                                        msg = "Error Processing Credit Card - ";

                                        // msg += PaypalRequest.ResponseString;

                                        for (int i = 0; i < PaypalRequest.ErrorList.Count; i++)
                                        {
                                            msg += PaypalRequest.ErrorList[i].LongMessage + "<br />";
                                        }

                                        lblErrorMessage.Text = msg;
                                    }
                                    else if (ViewState["PaymentGateway"].ToString().Equals("Paypalflow", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Errorlocation = "btnsecurepayment - getting Paypalflow ResponseString";

                                        string RespMsg = string.Empty;

                                        if (TrxnResponse.Result < 0)
                                        {
                                            RespMsg = "There was an error processing your transaction. Please contact Customer Service." + " <BR> Error: " + TrxnResponse.Result.ToString();
                                        }
                                        else if (TrxnResponse.Result == 1 || TrxnResponse.Result == 26)
                                        {
                                            RespMsg = "Account configuration issue.  Please verify your login credentials.";
                                        }
                                        else if (TrxnResponse.Result == 12)
                                        {
                                            RespMsg = "Your transaction was declined.";
                                        }
                                        else if (TrxnResponse.Result == 13)
                                        {
                                            RespMsg = "Your Transaction is pending. Contact Customer Service to complete your order.";
                                        }
                                        else if (TrxnResponse.Result == 23 || TrxnResponse.Result == 24)
                                        {
                                            RespMsg = "Invalid credit card information. Please re-enter.";
                                        }
                                        else if (TrxnResponse.Result == 125)
                                        {
                                            RespMsg = "Your Transactions has been declined. Contact Customer Service.";
                                        }
                                        else if (TrxnResponse.Result == 126)
                                        {
                                            if (TrxnResponse.AVSAddr != "Y" || TrxnResponse.AVSZip != "Y")
                                            {
                                                RespMsg = "Your billing information does not match.  Please re-enter.";
                                            }
                                            else
                                            {
                                                RespMsg = "Your Transaction is Under Review. We will notify you via e-mail if accepted.";
                                            }
                                            RespMsg = "Your Transaction is Under Review. We will notify you via e-mail if accepted.";
                                        }
                                        else if (TrxnResponse.Result == 127)
                                        {
                                            RespMsg = "Your Transaction is Under Review. We will notify you via e-mail if accepted.";
                                        }
                                        else
                                        {
                                            // Error occurred, display normalized message returned.
                                            RespMsg = TrxnResponse.RespMsg;
                                        }

                                        lblErrorMessage.Text = RespMsg;
                                    }
                                    #endregion
                                    return;
                                }
                                else
                                {
                                    if (ViewState["PaymentGateway"].ToString().Equals("AuthorizeNet", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Errorlocation = "btnsecurepayment - AuthorizeNet Process completed - getting TransactionID ";

                                        TransactionID = AuthorizeNetResponse.TransactionID;
                                        AddToCache(TransactionCacheName, AuthorizeNetResponse.TransactionID);
                                    }
                                    else if (ViewState["PaymentGateway"].ToString().Equals("Paypal", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Errorlocation = "btnsecurepayment - Paypal Process completed - getting TransactionID ";

                                        TransactionID = PaypalRequest.Get(NvpDoDirectPayment.Response.TRANSACTIONID.ToString());
                                        AddToCache(TransactionCacheName, PaypalRequest.Get(NvpDoDirectPayment.Response.TRANSACTIONID.ToString()));
                                    }
                                    else if (ViewState["PaymentGateway"].ToString().Equals("Paypalflow", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Errorlocation = "btnsecurepayment - Paypalflow Process completed - getting TrxnResponse.Pnref ";

                                        TransactionID = TrxnResponse.Pnref;
                                        AddToCache(TransactionCacheName, TrxnResponse.Pnref);
                                    }
                                }
                            }
                            else
                            {
                                TransactionID = "PPSkipped-" + System.Guid.NewGuid().ToString().Substring(1, 5);
                            }
                        }
                    }
                    else
                    {
                        //Show popup allowing them to clear their values from cache
                        mpeDuplicateTrans.Show();
                        return;
                    }

                    ErrorMessage = "Payment has been processed. The application has encountered an unknown error. Our technical staff have been automatically notified.";

                    #region Post Data to ECN after successful CC transaction.

                    foreach (KeyValuePair<int, Dictionary<string, string>> product in dPurchasedProducts)
                    {
                        int productID = product.Key;
                        Dictionary<string, string> dSubscribedata = product.Value;

                        Errorlocation = "btnsecurepayment - ECNUtils.ECNHttpPost  transactional UDF - payment is successful";

                        dSubscribedata.Add("t_transactionid", TransactionID);

                        #region Send data to ECN (write also to HTTP POST)

                        try
                        {
                            ECNUtils.SubscribeToGroup(Convert.ToInt32(dSubscribedata["c"]), Convert.ToInt32(dSubscribedata["g"]), dSubscribedata["publicationcode"], dSubscribedata, browserInfo);
                        }
                        catch (Exception ex)
                        {
                            //send notification email if NXTbook API fails.
                            string emailMsg = "Error in Paid forms - Auto Subscribe Groups <br /><br />";

                            emailMsg += "Errorlocation :" + Errorlocation + "<br /><br />";

                            emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                            emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                            emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                            emailMsg += "<b>Exception Details:</b>" + ex.Message;

                            emailMsg += "<b>Pubcode:</b>" + dSubscribedata["publicationcode"];
                            emailMsg += "<b>Data:</b></b>" + string.Join(", ", dSubscribedata.Select(x => string.Format("&{0}={1}", x.Key, x.Value)).ToArray());

                            EmailFunctions emailFunctions = new EmailFunctions();
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Paidforms - DB Update Failed in SubscribeToGroup method", emailMsg);
                        }

                        #endregion

                        #region NXTBook Posting
                        try
                        {
                            //Check for NXTBook Integration & Call NXTBookAPI.
                            if (IsNXTBookAPIEnabled)
                            {
                                Errorlocation = "btnsecurepayment - inside IsNXTBookAPIEnabled ";

                                NXTBook nxtbook = NXTBook.GetbyProductID(productID);

                                if (nxtbook != null)
                                {
                                    drmProfile dp = new NXTBookAPI.Entity.drmProfile();

                                    dp.subscriptionid = nxtbook.SubscriptionID;

                                    dp.update = true;
                                    dp.noupdate = false;
                                    dp.email = txtemail.Text;
                                    dp.password = NXTBookPassword;
                                    //dp.changepassword = "";
                                    dp.firstname = txtfirstname.Text;
                                    dp.lastname = txtlastname.Text;
                                    dp.phone = txtphone.Text;
                                    dp.address1 = txtShippingAddress.Text;
                                    dp.address2 = txtShippingAddress2.Text;
                                    dp.city = txtShippingCity.Text;
                                    dp.country = countryCode;

                                    if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
                                        dp.state = drpShippingState.SelectedValue;
                                    else
                                        dp.state = txtShippingState.Text;
                                    dp.zipcode = txtShippingZip.Text;
                                    dp.organization = txtcompany.Text;
                                    dp.extraparams = "";

                                    dp.access_nbissues = "";
                                    dp.access_firstissue = "";
                                    //dp.access_limitdate = "";


                                    if (Convert.ToBoolean(dSubscribedata["issubscription"]))
                                    {
                                        dp.access_type = "timerestricted";
                                        dp.access_startdate = NXTBook.GetRecentIssueDatebyPubcode(PubCode).ToString("yyyy-MM-dd");
                                        dp.access_enddate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        dp.access_type = "single";
                                        //public List<drmBook> access_issues
                                        drmBook dbook = new drmBook();

                                        //dbook.bookid = nxtbook.BookID;
                                        dbook.url = nxtbook.BookURL + (nxtbook.BookURL.EndsWith("/") ? nxtbook.BookID : "/" + nxtbook.BookID);

                                        dp.access_issues = new List<drmBook>();

                                        dp.access_issues.Add(dbook);
                                    }

                                    Errorlocation = "btnsecurepayment - before drmRESTAPI.SetProfile ";

                                    drmRESTAPI.SetProfile(nxtbook.APIKey, dp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Errorlocation = "btnsecurepayment - in Catch for NXTBook Posting - sending email";


                                //send notification email if NXTbook API fails.
                                string emailMsg = "Error send data to NXTBOOK from Paid form <br /><br />";
                                emailMsg += " Check TrackHTTPPost table for data.";

                                emailMsg += "emailaddress :" + txtemail.Text;
                                emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                                emailMsg += "<b>Error Request:</b>" + BuildRequestErrorString((HttpWebRequest)ex.Data["Request"]) + "<br /><br />";
                                if (ex.Data.Contains("ResponseData"))
                                    emailMsg += "<b>Response Data:</b>" + ex.Data["ResponseData"].ToString() + "<br /><br />";
                                emailMsg += "<b>Exception Details:</b>" + ex.Message;

                                EmailFunctions emailFunctions = new EmailFunctions();
                                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "NXTBook Post in Paid Forms - FAIL", emailMsg);

                                throw ex;
                            }
                            catch
                            {
                            }
                        }
                        #endregion

                        #region 3rd Party posting for each purchased product

                        Errorlocation = "btnsecurepayment - before extPostParams";

                        try
                        {
                            DataTable dt = new DataTable();
                            dt = HttpPost.getPaidFormProductHttpPostParams(productID);

                            if (dt.Rows.Count > 0)
                                buildParamsAndPost(dt, dSubscribedata);
                        }
                        catch (Exception ex)
                        {
                            //send notification email if NXTbook API fails.
                            string emailMsg = "Error in Paid forms - 3rd Party posting <br /><br />";

                            emailMsg += "Errorlocation :" + Errorlocation + "<br /><br />";

                            emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                            emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                            emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                            emailMsg += "<b>Exception Details:</b>" + ex.Message;

                            emailMsg += "<b>Pubcode:</b>" + dSubscribedata["publicationcode"];
                            emailMsg += "<b>Data:</b></b>" + string.Join(", ", dSubscribedata.Select(x => string.Format("&{0}={1}", x.Key, x.Value)).ToArray());

                            EmailFunctions emailFunctions = new EmailFunctions();
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                        }

                        #endregion

                        #region  Auto Subscribe Groups

                        try
                        {
                            Errorlocation = "btnsecurepayment - before Auto Subscribe Groups";

                            //auto subscribe to each group
                            string profileString = BuildProfile().ToString();

                            if (!AutoSubpubCodes.Contains(dSubscribedata["publicationcode"]))
                            {
                                AutoSubpubCodes.Add(dSubscribedata["publicationcode"]);
                                Dictionary<int, int> AutoGroups = new Dictionary<int, int>();

                                Publication p = Publication.GetPublicationbyID(0, dSubscribedata["publicationcode"]);
                                AutoGroups = Publication.GetAutoSubscriptions(p.PubID);

                                foreach (KeyValuePair<int, int> kvp in AutoGroups)
                                {
                                    string profile = profileString + "&g=" + kvp.Key.ToString() + "&c=" + kvp.Value.ToString() + "&sfID=&s=S" + BuildStandalone(p.ECNDefaultGroupID);
                                    ECNUtils.ECNHttpPost(txtemail.Text, dSubscribedata["publicationcode"], profile.ToString(), browserInfo);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            //send notification email if NXTbook API fails.
                            string emailMsg = "Error in Paid forms - Auto Subscribe Groups <br /><br />";

                            emailMsg += "Errorlocation :" + Errorlocation + "<br /><br />";

                            emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                            emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                            emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                            emailMsg += "<b>Exception Details:</b>" + ex.Message;

                            emailMsg += "<b>Pubcode:</b>" + dSubscribedata["publicationcode"];
                            emailMsg += "<b>Data:</b></b>" + string.Join(", ", dSubscribedata.Select(x => string.Format("&{0}={1}", x.Key, x.Value)).ToArray());

                            EmailFunctions emailFunctions = new EmailFunctions();
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                        }

                        #endregion

                    }

                    #endregion

                    BundleAutoSubscribe(TransactionID);
                    if (ViewState["FormRedirect"] != null && ViewState["FormRedirect"].ToString() != null && ViewState["FormRedirect"].ToString() != string.Empty)
                    {
                        Errorlocation = "btnsecurepayment - in FormRedirect";

                        string redirectURL = ViewState["FormRedirect"].ToString();
                        if (!redirectURL.Contains("http://") && !redirectURL.Contains("https://"))
                            redirectURL = "http://" + redirectURL;
                        Response.Redirect(redirectURL, true);
                    }
                    else
                    {
                        Errorlocation = "btnsecurepayment - redirect to Thankyou.";

                        Response.Redirect("forms/thankyou.aspx?formid=" + getFormID() + "&emailaddress=" + Server.UrlEncode(txtemail.Text), true);
                    }

                    #endregion
                }
                else
                {
                    Errorlocation = "btnsecurepayment - Page.IsValid failed - ECN post failed.";

                    phError.Visible = true;

                    string msg = "";
                    // Loop through all validation controls to see which
                    // generated the errors.
                    foreach (IValidator aValidator in this.Validators)
                    {
                        if (!aValidator.IsValid)
                        {
                            msg += "<br />" + aValidator.ErrorMessage;
                        }
                    }
                    lblErrorMessage.Text = msg;
                    return;
                }

                #endregion

            }
            catch (ThreadAbortException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                phError.Visible = true;
                lblErrorMessage.Text = ErrorMessage;

                try
                {
                    //send notification email if NXTbook API fails.
                    string emailMsg = "Error in Paid forms <br /><br />";

                    emailMsg += "Errorlocation :" + Errorlocation + "<br /><br />";

                    emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                    emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                    emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                    emailMsg += "<b>Exception Details:</b>" + ex.Message;

                    EmailFunctions emailFunctions = new EmailFunctions();
                    emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                }
                catch
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Error processing the payment.";
                }
            }
        }

        private string BuildRequestErrorString(HttpWebRequest request)
        {
            StringBuilder sbRequest = new StringBuilder();
            sbRequest.Append("Address:" + request.Address + "<br />");
            sbRequest.Append("Headers:");
            for (int i = 0; i < request.Headers.Count; ++i)
            {
                string header = request.Headers.GetKey(i);
                foreach (string s in request.Headers.GetValues(i))
                {
                    sbRequest.Append(string.Format("{0}: {1}", header, s) + "<br />");
                }
            }




            return sbRequest.ToString();
        }

        private void AddToCache(string Name, object Value)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {

                try
                {
                    KM.Common.CacheUtil.AddToCache(Name, Value);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string emailMsg = "Error in Paid forms <br /><br />";

                        emailMsg += "Errorlocation : Adding Demographics to Cache<br /><br />";

                        emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                        emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                        emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                        emailMsg += "<b>Exception Details:</b>" + ex.Message;

                        EmailFunctions emailFunctions = new EmailFunctions();
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                    }
                    catch { }
                }
            }

        }

        private object GetFromCache(string Name)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {
                try
                {
                    return KM.Common.CacheUtil.GetFromCache(Name);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string emailMsg = "Error in Paid forms <br /><br />";

                        emailMsg += "Errorlocation : Getting " + Name + " Cache object from Cache<br /><br />";

                        emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                        emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                        emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                        emailMsg += "<b>Exception Details:</b>" + ex.Message;

                        EmailFunctions emailFunctions = new EmailFunctions();
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                    }
                    catch { }
                }
            }
            return null;
        }

        private void RemoveFromCache(string Name)
        {
            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch { }


            if (isCacheEnabled)
            {

                try
                {
                    KM.Common.CacheUtil.RemoveFromCache(Name);
                }
                catch (Exception ex)
                {
                    try
                    {
                        string emailMsg = "Error in Paid forms <br /><br />";

                        emailMsg += "Errorlocation : Removing " + Name + " Cache object from Cache<br /><br />";

                        emailMsg += "emailaddress :" + txtemail.Text + "<br /><br />";
                        emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";
                        emailMsg += "<b>Error Response:</b>" + (ex.Data.Contains("Request") ? ex.Data["Request"].ToString() : "") + "<br /><br />";
                        emailMsg += "<b>Exception Details:</b>" + ex.Message;

                        EmailFunctions emailFunctions = new EmailFunctions();
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Exeption in Paid Forms", emailMsg);
                    }
                    catch { }
                }
            }

        }

        private string BuildStandalone(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = @"SELECT	gdf.ShortName, edv.DataValue 
                            FROM EmailDataValues edv WITH(NOLOCK)  
                            join  GroupDatafields gdf WITH(NOLOCK)  on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
                            JOIN Emails e with(nolock) on edv.EmailID = e.EmailID
                            WHERE e.EmailAddress = @emailAddress and gdf.GroupID = @groupID and isnull(gdf.DatafieldSetID,0) = 0";

            cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@emailAddress", txtemail.Text.Trim());
            cmd.Parameters.AddWithValue("@GroupID", groupID.ToString());

            DataTable dtUDFs = DataFunctions.GetDataTable("communicator", cmd);
            StringBuilder sbUdfs = new StringBuilder();
            if (dtUDFs != null && dtUDFs.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUDFs.Rows)
                {
                    sbUdfs.Append("&" + "user_" + dr["ShortName"].ToString() + "=" + Server.UrlEncode(dr["DataValue"].ToString()));
                    //dProfile.Add(dr["ShortName"].ToString().ToUpper(), dr["DataValue"].ToString());
                }
            }

            return sbUdfs.ToString();
        }

        private void buildParamsAndPost(DataTable dtPostParams, Dictionary<string, string> dSubscribedata)
        {
            string postURL = string.Empty;
            StringBuilder postparams = new StringBuilder();

            string paramvalue = string.Empty;

            if (dtPostParams.Rows.Count > 0)
            {
                postURL = dtPostParams.Rows[0]["URL"].ToString();
                foreach (DataRow dr in dtPostParams.Rows)
                {
                    paramvalue = string.Empty;

                    try
                    {
                        if (!dr["ParamValue"].ToString().Equals("CustomValue"))
                        {
                            if (dSubscribedata.ContainsKey(dr["ParamValue"].ToString().ToLower()))
                            {
                                paramvalue = dSubscribedata[dr["ParamValue"].ToString().ToLower()];

                                if (paramvalue != string.Empty && !dr.IsNull("DataType") && dr["DataType"].ToString().ToUpper() == "DATETIME" && !dr.IsNull("DataFormat") && dr["DataFormat"].ToString() != string.Empty)
                                {
                                    paramvalue = Convert.ToDateTime(paramvalue).ToString(dr["DataFormat"].ToString());
                                }

                                postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(paramvalue));
                            }
                            else
                            {
                                postparams.Append("&" + dr["ParamName"].ToString() + "=");
                            }
                        }
                        else
                        {
                            postparams.Append("&" + dr["ParamName"].ToString() + "=" + Server.UrlEncode(dr["CustomValue"].ToString()));
                        }
                    }
                    catch
                    {
                    }
                }
                Utilities.ExternalHttpPost(postURL + "?" + postparams.ToString().TrimStart('&'));
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

            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
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

        private void BundleAutoSubscribe(string transactionID)
        {
            HttpBrowserCapabilities browser = Request.Browser;
            string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12}, EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

            StringBuilder profile = BuildProfile();
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label lblIsBundle = (Label)row.FindControl("lblIsBundle");
                Label lblPubCode = (Label)row.FindControl("lblPubcode");

                if (lblIsBundle.Text.ToLower().Equals("true"))
                {
                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                    if (chkPrint.Checked == true)
                    {
                        Label custID = (Label)row.FindControl("lblCustomerID");
                        Label lblProductID = (Label)row.FindControl("lblProductID");
                        DataTable subscriptionGroup = GetSubscriptionGroups(Convert.ToInt32(lblProductID.Text));
                        foreach (DataRow dr in subscriptionGroup.Rows)
                        {
                            ECNUtils.ECNHttpPost(txtemail.Text, lblPubCode.Text, profile.ToString() + "&g=" + dr["GroupID"].ToString() + "&c=" + dr["CustomerID"].ToString() + "&user_t_Bundle=true&user_t_TransactionID=" + transactionID, browserInfo);
                        }
                    }
                }
            }
        }

        private bool ValidateCreditCard()
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard()";

            if (ViewState["PaymentGateway"].ToString().Equals("AuthorizeNet", StringComparison.OrdinalIgnoreCase))
                return ValidateCreditCard_AuthorizeNet();
            else if (ViewState["PaymentGateway"].ToString().Equals("Paypal", StringComparison.OrdinalIgnoreCase))
                return ValidateCreditCard_Paypal();
            else if (ViewState["PaymentGateway"].ToString().Equals("Paypalflow", StringComparison.OrdinalIgnoreCase))
                return ValidateCreditCard_Paypalflow();

            return false;
        }

        private bool ValidateCreditCard_AuthorizeNet()
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard_AuthorizeNet()";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                string MerchantId;
                string Signature;
                if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                {
                    MerchantId = WebConfigurationManager.AppSettings["AuthorizeDotNetLogin"].ToString();
                    Signature = WebConfigurationManager.AppSettings["AuthorizeDotNetKey"].ToString();
                }
                else
                {
                    MerchantId = ViewState["AuthorizeDotNetLogin"].ToString();
                    Signature = ViewState["AuthorizeDotNetKey"].ToString();
                }
                string countrCode = "";
                int totalQuantity = 0;
                string orderdesc = "";
                string orderNames = "";
                string orderItems = string.Empty;
                if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA" || drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
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
                gateway.TestMode = Convert.ToBoolean(ViewState["UseTestMode"].ToString());

                AuthorizeNetRequest.CardNum = CardNo;
                AuthorizeNetRequest.ExpDate = user_Exp_Month.Text + user_Exp_Year.Text;
                AuthorizeNetRequest.Amount = double.Parse(((Label)grdMagazines.FooterRow.FindControl("lblTotalAmount")).Text).ToString().TrimStart('$');
                AuthorizeNetRequest.AddInvoice(PromoCode);
                AuthorizeNetRequest.AddCardCode(user_CCVerfication.Text);
                AuthorizeNetRequest.Country = countrCode;
                AuthorizeNetRequest.Email = txtemail.Text;
                AuthorizeNetRequest.CustomerIp = System.Web.HttpContext.Current.Request.UserHostAddress;
                AuthorizeNetRequest.City = txtBillingCity.Text;

                if (this.itemList.Count > 0)
                {
                    string SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, txtemail.Text);

                    string BillingStateToUse = "";
                    string ShippingStateToUse = "";
                    if (pnlState3.Visible)
                    {
                        BillingStateToUse = drpBillingState.SelectedItem.Value;
                    }
                    else
                    {
                        BillingStateToUse = txtBillingState.Text;
                    }
                    if (pnlState1.Visible)
                    {
                        ShippingStateToUse = drpShippingState.SelectedItem.Value;
                    }
                    else
                    {
                        ShippingStateToUse = txtShippingState.Text;
                    }


                    AuthorizeNetRequest.AddCustomer(SubscriberID, txtemail.Text, firstName, lastName, txtBillingAddress.Text + " " + txtBillingAddress2.Text, txtBillingCity.Text, BillingStateToUse, txtBillingZip.Text);
                    AuthorizeNetRequest.AddShipping(SubscriberID, txtemail.Text, txtfirstname.Text, txtlastname.Text, txtShippingAddress.Text + " " + txtShippingAddress2.Text, txtShippingCity.Text, ShippingStateToUse, txtShippingZip.Text);

                    AuthorizeNetRequest.ShipToCity = txtShippingCity.Text;
                    AuthorizeNetRequest.ShipToCountry = countrCode;
                }
                foreach (PaidPub.Objects.Item item in itemList)
                {
                    orderItems += item.ItemCode + ",";
                    orderdesc += item.ItemDescription + ",";
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

        private bool ValidateCreditCard_Paypal()
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypal()";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string amount = double.Parse(((Label)grdMagazines.FooterRow.FindControl("lblTotalAmount")).Text).ToString().TrimStart('$');
                if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                {
                    PaypalRequest.Environment = NvpEnvironment.Sandbox;
                    PaypalRequest.Credentials.Username = WebConfigurationManager.AppSettings["PayflowAccount"].ToString();
                    PaypalRequest.Credentials.Password = WebConfigurationManager.AppSettings["PayflowPassword"].ToString();
                    PaypalRequest.Credentials.Signature = WebConfigurationManager.AppSettings["PayflowSignature"].ToString();
                }
                else
                {
                    PaypalRequest.Environment = NvpEnvironment.Live;
                    PaypalRequest.Credentials.Username = ViewState["PayflowAccount"].ToString();
                    PaypalRequest.Credentials.Password = ViewState["PayflowPassword"].ToString();
                    PaypalRequest.Credentials.Signature = ViewState["PayflowSignature"].ToString();
                }

                if (user_CCType.SelectedItem.Value.Equals("MasterCard", StringComparison.OrdinalIgnoreCase))
                    PaypalRequest.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.MasterCard);
                else if (user_CCType.Text.Equals("Visa", StringComparison.OrdinalIgnoreCase))
                    PaypalRequest.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Visa);
                else if (user_CCType.Text.Equals("Amex", StringComparison.OrdinalIgnoreCase))
                    PaypalRequest.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Amex);

                PaypalRequest.Add(NvpDoDirectPayment.Request._ACCT, user_CCNumber.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request._EXPDATE, user_Exp_Month.SelectedItem.Value + user_Exp_Year.SelectedItem.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.CVV2, user_CCVerfication.Text);

                string fullName = user_CardHolderName.Text;
                string[] fullNameDelimited = fullName.Split(new char[] { ' ' });
                string firstName = fullNameDelimited[0];
                string lastName = string.Empty;
                if (fullNameDelimited.Length > 1)
                    lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);

                PaypalRequest.Add(NvpDoDirectPayment.Request._FIRSTNAME, firstName);
                PaypalRequest.Add(NvpDoDirectPayment.Request._LASTNAME, lastName);

                string countrCode = "";
                if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA" || drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
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

                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTONAME, txtfirstname.Text.ToString() + " " + txtlastname.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOSTREET, txtShippingAddress.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOSTREET2, txtShippingAddress2.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOCITY, txtShippingCity.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOZIP, txtShippingZip.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOCOUNTRYCODE, countrCode);
                PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOPHONENUM, txtphone.Text);

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "CANADA" || countryCode.ToUpper() == "UNITED STATES")
                {
                    PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOSTATE, drpShippingState.SelectedItem.Value);
                }
                else
                {
                    PaypalRequest.Add(NvpDoDirectPayment.Request.SHIPTOSTATE, drpShippingState.Text);
                }


                PaypalRequest.Add(NvpDoDirectPayment.Request.PHONENUM, txtphone.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.STREET, txtBillingAddress.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.STREET2, txtBillingAddress2.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.CITY, txtBillingCity.Text);
                if (countrCode.ToUpper() == "US" || countrCode.ToUpper() == "CA")
                {
                    PaypalRequest.Add(NvpDoDirectPayment.Request.STATE, drpBillingState.SelectedItem.Value);
                }
                else
                {
                    PaypalRequest.Add(NvpDoDirectPayment.Request.STATE, txtBillingState.Text);
                }
                PaypalRequest.Add(NvpDoDirectPayment.Request.ZIP, txtShippingZip.Text);
                PaypalRequest.Add(NvpDoDirectPayment.Request.COUNTRYCODE, countrCode);

                PaypalRequest.Add(NvpDoDirectPayment.Request.EMAIL, txtemail.Text);

                //int totalQuantity = 0;
                //string orderdesc = "";
                //string orderNames = "";
                //string orderItems = string.Empty;
                //foreach (PaidPub.Objects.Item item in itemList)
                //{
                //    orderItems += item.ItemCode + ",";
                //    orderdesc += item.Description + ",";
                //    orderNames += item.ItemName + ",";
                //    totalQuantity += Convert.ToInt32(item.ItemQty);
                //}
                //string itemID = orderItems.Trim(',');
                //string itemName = orderNames.Trim(',');
                //string itemDesc = orderdesc.Trim(',');
                //decimal itemPrice = Convert.ToDecimal(ppPay.Amount) / Convert.ToDecimal(totalQuantity);

                PaypalRequest.Add(NvpDoDirectPayment.Request._IPADDRESS, System.Web.HttpContext.Current.Request.UserHostAddress);
                PaypalRequest.Add(NvpDoDirectPayment.Request._PAYMENTACTION, NvpPaymentActionCodeType.Sale);
                PaypalRequest.Add(NvpDoDirectPayment.Request._AMT, amount);
                PaypalRequest.Add(NvpDoDirectPayment.Request.ITEMAMT, amount);
                //PaypalObject.Add(NvpDoDirectPayment.Request.L_NAMEn , "Paid Forms");
                //PaypalObject.Add(NvpDoDirectPayment.Request.L_AMTn + amount, amount);
                //PaypalObject.Add(NvpDoDirectPayment.Request.L_QTYn + "1", "1");

                for (int i = 0; i < itemList.Count; i++)
                {
                    PaypalRequest.Add(NvpDoDirectPayment.Request.L_NAMEn + i.ToString(), itemList[i].ItemDescription + "(" + itemList[i].ItemCode + ")");
                    PaypalRequest.Add(NvpDoDirectPayment.Request.L_AMTn + i.ToString(), itemList[i].ItemAmount);
                    PaypalRequest.Add(NvpDoDirectPayment.Request.L_QTYn + i.ToString(), itemList[i].ItemQty);
                }

                Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypal() - before PaypalRequest.Post()";

                return PaypalRequest.Post();
            }
            catch (Exception ex)
            {
                Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypal() - in Catch";

                string emailMsg = "Error when Processing Credit Card..<br /><br />";
                emailMsg += txtemail.Text;
                emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                emailMsg += "<b>Error Response:</b>" + PaypalRequest.ResponseString + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal Transaction Response-FAIL", emailMsg);

                throw ex;
            }
        }

        /*
        private bool ValidateCreditCard_PaypalflowRedirect(Dictionary<int, Dictionary<string, string>> dPurchasedProducts)
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect()";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PayflowConnectionData Connection = new PayflowConnectionData();

            string partner = string.Empty;
            string vendor = string.Empty;
            string user = string.Empty;
            string password = string.Empty;

            String RequestID = PayflowUtility.RequestId;
            CultureInfo us = new CultureInfo("en-US");

            try
            {
                NameValueCollection requestArray = new NameValueCollection();
                
                string amount = double.Parse(((Label)grdMagazines.FooterRow.FindControl("lblTotalAmount")).Text).ToString().TrimStart('$');
                string urlEndpoint = "";
                if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                {
                    
                    requestArray.Add("PARTNER", WebConfigurationManager.AppSettings["payflowproPartner"].ToString());
                    requestArray.Add("VENDOR", WebConfigurationManager.AppSettings["payflowproVendor"].ToString());
                    requestArray.Add("USER", WebConfigurationManager.AppSettings["payflowproUser"].ToString());
                    requestArray.Add("PWD", WebConfigurationManager.AppSettings["payflowproPassword"].ToString());
                    urlEndpoint = "https://pilot-payflowpro.paypal.com/";
                }
                else
                {
                    requestArray.Add("PARTNER", ViewState["PayflowPartner"].ToString());
                    requestArray.Add("VENDOR", ViewState["PayflowVendor"].ToString());
                    requestArray.Add("USER", ViewState["PayflowAccount"].ToString());
                    requestArray.Add("PWD", ViewState["PayflowPassword"].ToString());
                    urlEndpoint = "https://payflowpro.paypal.com/";
                }

                
                string fullName = txtfirstname.Text + " " + txtlastname.Text;
                string firstName = txtfirstname.Text;
                string lastName = txtlastname.Text;

                if (!amount.Contains("."))
                    amount += ".00";

                requestArray.Add("AMT", amount);
                requestArray.Add("ITEMAMT", amount);
                requestArray.Add("CURRENCY", "USD");
                requestArray.Add("CREATESECURETOKEN", "Y");
                requestArray.Add("TRXTYPE", "S");
                requestArray.Add("SECURETOKENID", genId());
                requestArray.Add("EMAIL", txtemail.Text.Trim());

                string countryCode = "";
                if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA")
                {
                    countryCode = "US";
                }
                else if (drpCountry.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    countryCode = "CA";
                }
                else
                {
                    countryCode = drpCountry.SelectedItem.Text.ToUpper();
                }

                requestArray.Add("BILLTOFIRSTNAME", firstName);
                requestArray.Add("BILLTOLASTNAME", lastName);
                requestArray.Add("BILLTOSTREET", (txtBillingAddress.Text + " " + txtBillingAddress2.Text).Trim());
                requestArray.Add("BILLTOCITY", txtBillingCity.Text);


                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "CANADA")
                {
                    requestArray.Add("BILLTOSTATE", drpBillingState.SelectedItem.Value);
                }
                else
                {
                    requestArray.Add("BILLTOSTATE", txtBillingState.Text);
                }

                requestArray.Add("BILLTOZIP", txtBillingZip.Text);
                requestArray.Add("BILLTOCOUNTRY", countryCode);
                requestArray.Add("BILLTOEMAIL", txtemail.Text);

                requestArray.Add("SHIPTOFIRSTNAME", txtfirstname.Text);
                requestArray.Add("SHIPTOLASTNAME", txtlastname.Text);
                requestArray.Add("SHIPTOSTREET", (txtShippingAddress.Text + " " + txtShippingAddress2.Text).Trim());
                requestArray.Add("SHIPTOCITY", txtShippingCity.Text);

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "CANADA")
                {
                    requestArray.Add("SHIPTOSTATE", drpShippingState.SelectedItem.Value);
                }
                else
                {
                    requestArray.Add("SHIPTOSTATE", drpShippingState.Text);
                }

                requestArray.Add("SHIPTOZIP", txtShippingZip.Text);
                requestArray.Add("SHIPTOCOUNTRY", countryCode);

                string Pubcodes = string.Empty;
                //requestArray.Add("L_QTYn", itemList.Count.ToString());
                int itemIndex = 0;
                foreach (PaidPub.Objects.Item item in itemList)
                {
                    //PayPal.Payments.DataObjects.LineItem li = new PayPal.Payments.DataObjects.LineItem();
                    requestArray.Add("L_QTY" + itemIndex.ToString(), item.ItemQty);
                    if (!item.ItemAmount.Contains("."))
                        item.ItemAmount += ".00";
                    requestArray.Add("L_COST" + itemIndex.ToString(), item.ItemAmount);
                    requestArray.Add("L_NAME" + itemIndex.ToString(), item.ItemName);
                    //li.Name = item.ItemName + "(" + item.ItemCode + ")";
                    //li.ItemNumber = item.ItemCode;
                    //li.Desc = item.ItemDescription;
                    //li.Qty = long.Parse(item.ItemQty);
                    //li.Cost = new Currency(decimal.Parse(item.ItemAmount), "USD");
                    //li.Amt = new Currency(decimal.Parse(item.ItemAmount), "USD"); ;

                    itemIndex++;
                    Pubcodes += (Pubcodes == string.Empty ? item.ItemCode + "($" + item.ItemAmount + ")" : "," + item.ItemCode + "($" + item.ItemAmount + ")");
                }
                requestArray.Add("USER1", Pubcodes);
                requestArray.Add("USER2", (txtfirstname.Text + " " + txtlastname.Text).Trim());
                //Inv.Comment1 = Pubcodes;
                //Inv.Comment2 = txtfirstname.Text + " " + txtlastname.Text;

                

                string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;

                requestArray.Add("RETURNURL", basePath + "/Forms/PayflowProPayment.aspx?formID=" + getFormID() + "&emailaddress=" + Server.UrlEncode(txtemail.Text));
                requestArray.Add("CANCELURL", basePath + "/Forms/Cancel.aspx?formID=" + getFormID());
                requestArray.Add("URLMETHOD", "POST");

                Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - before Trans.SubmitTransaction()";

                Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - after Trans.SubmitTransaction()";
                NameValueCollection resp = run_payflow_call(requestArray, urlEndpoint);
                if (resp["RESULT"] == "0")
                {
                    Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - Trans.SubmitTransaction() Resp in not null";
                    string mode = "";
                    if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                    {
                        mode = "TEST";
                    }
                    else
                    {
                        mode = "LIVE";
                    }
                    AddToCache(txtemail.Text.ToLower().Trim() + "_" + getFormID().ToString() + "_PurchasedProducts", dPurchasedProducts);
                    Response.Redirect("https://payflowlink.paypal.com?SECURETOKEN=" + resp["SECURETOKEN"] + "&SECURETOKENID=" + resp["SECURETOKENID"] + "&MODE=" + mode);

                }
                else
                {
                    Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - in else - sending exception details";

                    string emailMsg = "Error when Processing PayPal flow redirect..<br /><br />";
                    emailMsg += txtemail.Text;
                    emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                    //emailMsg += "<b>Error Response:</b>" + Resp.ResponseString + "<br /><br />";
                    // emailMsg += "<b>Exception Details:</b>" + ex.Message;

                    EmailFunctions emailFunctions = new EmailFunctions();
                    emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal flow redirect-FAIL", emailMsg);

                }

                return false;
            }
            catch (Exception ex)
            {
                Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - in Catch - sending exception details";

                string emailMsg = "Error when Processing PayPal flow Credit Card..<br /><br />";
                emailMsg += txtemail.Text;
                emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                //emailMsg += "<b>Error Response:</b>" + Resp.ResponseString + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal Transaction Response-FAIL", emailMsg);

                throw ex;
            }
        }

        */
        private bool ValidateCreditCard_PaypalflowRedirect(Dictionary<int, Dictionary<string, string>> dPurchasedProducts)
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect()";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PayflowConnectionData Connection = new PayflowConnectionData();

            string partner = string.Empty;
            string vendor = string.Empty;
            string user = string.Empty;
            string password = string.Empty;

            String RequestID = PayflowUtility.RequestId;
            CultureInfo us = new CultureInfo("en-US");

            try
            {
                NameValueCollection requestArray = new NameValueCollection();

                string amount = double.Parse(((Label)grdMagazines.FooterRow.FindControl("lblTotalAmount")).Text).ToString().TrimStart('$');
                string urlEndpoint = "";
                if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                {

                    requestArray.Add("PARTNER", WebConfigurationManager.AppSettings["payflowproPartner"].ToString());
                    requestArray.Add("VENDOR", WebConfigurationManager.AppSettings["payflowproVendor"].ToString());
                    requestArray.Add("USER", WebConfigurationManager.AppSettings["payflowproUser"].ToString());
                    requestArray.Add("PWD", WebConfigurationManager.AppSettings["payflowproPassword"].ToString());
                    urlEndpoint = "https://pilot-payflowpro.paypal.com/";
                }
                else
                {
                    requestArray.Add("PARTNER", ViewState["PayflowPartner"].ToString());
                    requestArray.Add("VENDOR", ViewState["PayflowVendor"].ToString());
                    requestArray.Add("USER", ViewState["PayflowAccount"].ToString());
                    requestArray.Add("PWD", ViewState["PayflowPassword"].ToString());
                    urlEndpoint = "https://payflowpro.paypal.com/";
                }

                string fullName = txtfirstname.Text + " " + txtlastname.Text;
                string firstName = txtfirstname.Text;
                string lastName = txtlastname.Text;

                if (!amount.Contains("."))
                    amount += ".00";

                requestArray.Add("AMT", amount);
                requestArray.Add("ITEMAMT", amount);
                requestArray.Add("CURRENCY", "USD");
                requestArray.Add("CREATESECURETOKEN", "Y");
                requestArray.Add("TRXTYPE", "S");
                requestArray.Add("SECURETOKENID", genId());
                requestArray.Add("EMAIL", txtemail.Text.Trim());
                //requestArray.Add("PHONE", txtphone.Text.Trim());

                string countryCode = "";
                if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA" || drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
                {
                    countryCode = "US";
                }
                else if (drpCountry.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    countryCode = "CA";
                }
                else
                {
                    countryCode = GetCountrCode(drpCountry.SelectedValue.ToString());
                    if (string.IsNullOrWhiteSpace(countryCode))
                        countryCode = drpCountry.SelectedItem.Text.ToUpper();
                }

                requestArray.Add("BILLTOFIRSTNAME", firstName);
                requestArray.Add("BILLTOLASTNAME", lastName);
                requestArray.Add("SHIPTOFIRSTNAME", firstName);
                requestArray.Add("SHIPTOLASTNAME", lastName);

                //requestArray.Add("BILLTOSTREET", (txtBillingAddress.Text + " " + txtBillingAddress2.Text).Trim());

                //requestArray.Add("BILLTOCITY", txtBillingCity.Text);
                requestArray.Add("BILLTOPHONE", txtphone.Text.Trim());



                //if (countryCode.ToUpper() == "US" || countryCode.ToUpper() == "CA")
                //{
                //    requestArray.Add("BILLTOSTATE", drpBillingState.SelectedItem.Value);
                //}
                //else
                //{
                //    requestArray.Add("BILLTOSTATE", txtBillingState.Text);
                //}

                //requestArray.Add("BILLTOZIP", txtBillingZip.Text);
                requestArray.Add("BILLTOCOUNTRY", countryCode);
                requestArray.Add("SHIPTOCOUNTRY", countryCode);
                //requestArray.Add("BILLTOEMAIL", txtemail.Text);

                //requestArray.Add("SHIPTOFIRSTNAME", txtfirstname.Text);
                //requestArray.Add("SHIPTOLASTNAME", txtlastname.Text);
                //requestArray.Add("SHIPTOSTREET", txtShippingAddress.Text);
                //requestArray.Add("SHIPTOSTREET2", txtShippingAddress2.Text);
                //requestArray.Add("SHIPTOCITY", txtShippingCity.Text);

                //if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "CANADA")
                //{
                //    requestArray.Add("SHIPTOSTATE", drpShippingState.SelectedItem.Value);
                //}
                //else
                //{
                //    requestArray.Add("SHIPTOSTATE", drpShippingState.Text);
                //}

                //requestArray.Add("SHIPTOZIP", txtShippingZip.Text);


                string Pubcodes = string.Empty;

                int itemIndex = 0;

                foreach (PaidPub.Objects.Item item in itemList)
                {

                    requestArray.Add("L_QTY" + itemIndex.ToString(), item.ItemQty);
                    if (!item.ItemAmount.Contains("."))
                        item.ItemAmount += ".00";
                    requestArray.Add("L_COST" + itemIndex.ToString(), item.ItemAmount);
                    requestArray.Add("L_NAME" + itemIndex.ToString(), item.ItemName);
                    requestArray.Add("L_DESC" + itemIndex.ToString(), item.ItemDescription);

                    requestArray.Add("QTY" + itemIndex.ToString(), item.ItemQty);
                    if (!item.ItemAmount.Contains("."))
                        item.ItemAmount += ".00";
                    requestArray.Add("COST" + itemIndex.ToString(), item.ItemAmount);
                    requestArray.Add("NAME" + itemIndex.ToString(), item.ItemName);
                    requestArray.Add("DESC" + itemIndex.ToString(), item.ItemDescription);


                    itemIndex++;
                    Pubcodes += (Pubcodes == string.Empty ? item.ItemCode + "($" + item.ItemAmount + ")" : "," + item.ItemCode + "($" + item.ItemAmount + ")");
                }
                requestArray.Add("COMMENT1", Pubcodes);
                requestArray.Add("COMMENT2", (txtfirstname.Text + " " + txtlastname.Text).Trim());

                string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;

                requestArray.Add("RETURNURL", basePath + "/Forms/PayflowProPayment.aspx?formID=" + getFormID() + "&emailaddress=" + Server.UrlEncode(txtemail.Text));
                requestArray.Add("CANCELURL", basePath + "/Forms/Cancel.aspx?formID=" + getFormID());
                requestArray.Add("ERRORURL", basePath + "/Forms/PayflowProPayment.aspx?formID=" + getFormID() + "&emailaddress=" + Server.UrlEncode(txtemail.Text));
                requestArray.Add("URLMETHOD", "POST");

                Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - before Trans.SubmitTransaction()";

                Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - after Trans.SubmitTransaction()";
                SendPaypalRequestEmail(requestArray, txtemail.Text.Trim());
                NameValueCollection resp = run_payflow_call(requestArray, urlEndpoint);
                if (resp["RESULT"] == "0")
                {
                    Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - Trans.SubmitTransaction() Resp in not null";
                    string mode = "";
                    if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                    {
                        mode = "TEST";
                    }
                    else
                    {
                        mode = "LIVE";
                    }
                    AddToCache(txtemail.Text.ToLower().Trim() + "_" + getFormID().ToString() + "_PurchasedProducts", dPurchasedProducts);
                    Response.Redirect("https://payflowlink.paypal.com?SECURETOKEN=" + resp["SECURETOKEN"] + "&SECURETOKENID=" + resp["SECURETOKENID"] + "&MODE=" + mode, false);

                }
                else
                {
                    Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - in else - sending exception details";

                    string emailMsg = "Error when Processing PayPal flow redirect..<br /><br />";
                    emailMsg += txtemail.Text;
                    emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                    //emailMsg += "<b>Error Response:</b>" + Resp.ResponseString + "<br /><br />";
                    // emailMsg += "<b>Exception Details:</b>" + ex.Message;

                    EmailFunctions emailFunctions = new EmailFunctions();
                    emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal flow redirect-FAIL", emailMsg);

                }

                return false;
            }
            catch (Exception ex)
            {
                Errorlocation = "btnsecurepayment - ValidateCreditCard_PaypalflowRedirect() - in Catch - sending exception details";

                string emailMsg = "Error when Processing PayPal flow Credit Card..<br /><br />";
                emailMsg += txtemail.Text;
                emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                //emailMsg += "<b>Error Response:</b>" + Resp.ResponseString + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal Transaction Response-FAIL", emailMsg);

                throw ex;
            }
        }
        private string genId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 16)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return "MySecTokenID-" + result; //add a prefix to avoid confusion with the "SECURETOKEN"
        }

        private void SendPaypalRequestEmail(NameValueCollection request, string EmailAddress)
        {
            try
            {
                string emailMsg = "Paypal Flow Redirect Request  <br /><br />";


                emailMsg += "emailaddress :" + EmailAddress + "<br /><br />";
                emailMsg += "formID :" + getFormID().ToString() + "<br /><br />";
                emailMsg += "URL : " + Request.Url.ToString() + "<br /><br />";


                string nvpstring = "";
                foreach (string key in request)
                {
                    //format:  "PARAMETERNAME[lengthofvalue]=VALUE&".  Never URL encode.
                    var val = request[key];
                    //nvpstring += key + "[ " + val.Length + "]=" + val + "&";
                    nvpstring += "<b>" + key + "</b>" + ":" + val + "<br />";
                }
                emailMsg += "<b>NameValue Collection:</b><br />" + nvpstring;

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Paidforms - PaypalFlow Redirect Request", emailMsg);
            }
            catch (Exception ex)
            {

            }
        }

        private NameValueCollection run_payflow_call(NameValueCollection requestArray, string urlEndpoint)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            String nvpstring = "";
            foreach (string key in requestArray)
            {
                //format:  "PARAMETERNAME[lengthofvalue]=VALUE&".  Never URL encode.
                var val = requestArray[key];
                //nvpstring += key + "[ " + val.Length + "]=" + val + "&";
                nvpstring += key + "=" + val + "&";
            }
            nvpstring = nvpstring.TrimEnd('&');

            HttpWebRequest payReq = (HttpWebRequest)WebRequest.Create(urlEndpoint);
            payReq.Method = "POST";
            payReq.ContentLength = nvpstring.Length;
            payReq.ContentType = "application/x-www-form-urlencoded";

            StreamWriter sw = new StreamWriter(payReq.GetRequestStream());
            sw.Write(nvpstring);
            sw.Close();

            HttpWebResponse payResp = (HttpWebResponse)payReq.GetResponse();
            StreamReader sr = new StreamReader(payResp.GetResponseStream());
            string response = sr.ReadToEnd();
            sr.Close();

            //parse string into array and return
            NameValueCollection dict = new NameValueCollection();
            foreach (string nvp in response.Split('&'))
            {
                string[] keys = nvp.Split('=');
                dict.Add(keys[0], keys[1]);
            }
            return dict;
        }

        private bool ValidateCreditCard_Paypalflow()
        {
            Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypalflow()";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PayflowConnectionData Connection = new PayflowConnectionData();

            string partner = string.Empty;
            string vendor = string.Empty;
            string user = string.Empty;
            string password = string.Empty;

            String RequestID = PayflowUtility.RequestId;
            CultureInfo us = new CultureInfo("en-US");

            try
            {
                string amount = double.Parse(((Label)grdMagazines.FooterRow.FindControl("lblTotalAmount")).Text).ToString().TrimStart('$');
                if (Convert.ToBoolean(ViewState["UseTestMode"].ToString()))
                {
                    Connection = new PayflowConnectionData("pilot-payflowpro.paypal.com", 443, 45, "", 0, "", "");

                    partner = WebConfigurationManager.AppSettings["payflowproPartner"].ToString();
                    vendor = WebConfigurationManager.AppSettings["payflowproVendor"].ToString();
                    user = WebConfigurationManager.AppSettings["payflowproUser"].ToString();
                    password = WebConfigurationManager.AppSettings["payflowproPassword"].ToString();

                }
                else
                {
                    Connection = new PayflowConnectionData("payflowpro.paypal.com", 443, 45, "", 0, "", "");

                    partner = ViewState["PayflowPartner"].ToString();
                    vendor = ViewState["PayflowVendor"].ToString();
                    user = ViewState["PayflowAccount"].ToString();
                    password = ViewState["PayflowPassword"].ToString();
                }

                UserInfo User = new UserInfo(user, vendor, partner, password);


                string fullName = user_CardHolderName.Text;
                string[] fullNameDelimited = fullName.Split(new char[] { ' ' });
                string firstName = fullNameDelimited[0];
                string lastName = string.Empty;
                if (fullNameDelimited.Length > 1)
                    lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length - 1);

                Invoice Inv = new Invoice();
                Currency Amt = new Currency(decimal.Parse(amount), "USD");
                Inv.Amt = Amt;
                Inv.InvoiceDate = DateTime.Now.ToString();

                string countryCode = "";
                if (drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA" || drpCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
                {
                    countryCode = "US";
                }
                else if (drpCountry.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    countryCode = "CA";
                }
                else
                {
                    countryCode = drpCountry.SelectedItem.Text.ToUpper();
                }

                BillTo Bill = new BillTo();
                Bill.BillToFirstName = firstName;
                Bill.BillToLastName = lastName;
                Bill.BillToStreet = txtBillingAddress.Text;
                Bill.BillToStreet2 = txtBillingAddress2.Text;
                Bill.BillToCity = txtBillingCity.Text;

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
                {
                    Bill.BillToState = drpBillingState.SelectedItem.Value;
                }
                else
                {
                    Bill.BillToState = txtBillingState.Text;
                }

                Bill.BillToZip = txtBillingZip.Text;
                Bill.BillToPhone = txtphone.Text;
                Bill.BillToEmail = txtemail.Text;
                Bill.BillToCountry = countryCode;
                Inv.BillTo = Bill;


                ShipTo ship = new ShipTo();

                ship.ShipToEmail = txtemail.Text;
                ship.ShipToFirstName = txtfirstname.Text;
                ship.ShipToLastName = txtlastname.Text;
                ship.ShipToStreet = txtShippingAddress.Text;
                ship.ShipToStreet2 = txtShippingAddress2.Text;
                ship.ShipToCity = txtShippingCity.Text;

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
                {
                    ship.ShipToState = drpShippingState.SelectedItem.Value;
                }
                else
                {
                    ship.ShipToState = drpShippingState.Text;
                }

                ship.ShipToZip = txtShippingZip.Text;
                ship.ShipToPhone = txtphone.Text;
                ship.ShipToCountry = countryCode;

                Inv.ShipTo = ship;

                string Pubcodes = string.Empty;

                foreach (PaidPub.Objects.Item item in itemList)
                {
                    PayPal.Payments.DataObjects.LineItem li = new PayPal.Payments.DataObjects.LineItem();

                    li.Name = item.ItemName + "(" + item.ItemCode + ")";
                    li.ItemNumber = item.ItemCode;
                    li.Desc = item.ItemDescription;
                    li.Qty = long.Parse(item.ItemQty);
                    li.Cost = new Currency(decimal.Parse(item.ItemAmount), "USD");
                    li.Amt = new Currency(decimal.Parse(item.ItemAmount), "USD"); ;

                    Inv.AddLineItem(li);

                    Pubcodes += (Pubcodes == string.Empty ? item.ItemCode + "($" + item.ItemAmount + ")" : "," + item.ItemCode + "($" + item.ItemAmount + ")");
                }

                Inv.Comment1 = Pubcodes;
                Inv.Comment2 = txtfirstname.Text + " " + txtlastname.Text;

                string CardNo = Regex.Replace(user_CCNumber.Text, @"[ -/._#]", "");
                CreditCard cc = new CreditCard(CardNo, user_Exp_Month.SelectedItem.Value + user_Exp_Year.SelectedItem.Text.Substring(2, 2));
                cc.Cvv2 = user_CCVerfication.Text;

                CardTender Card = new CardTender(cc);

                // Create a new Sale Transaction.
                SaleTransaction Trans = new SaleTransaction(
                    User, Connection, Inv, Card, PayflowUtility.RequestId);

                Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypalflow() - before Trans.SubmitTransaction()";

                Response Resp = Trans.SubmitTransaction();

                Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypalflow() - after Trans.SubmitTransaction()";

                if (Resp != null)
                {
                    Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypalflow() - Trans.SubmitTransaction() Resp in not null";

                    // Get the Transaction Response parameters.
                    TrxnResponse = Resp.TransactionResponse;

                    if (TrxnResponse.Result == 0)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                Errorlocation = "btnsecurepayment - ValidateCreditCard_Paypalflow() - in Catch - sending exception details";

                string emailMsg = "Error when Processing PayPal flow Credit Card..<br /><br />";
                emailMsg += txtemail.Text;
                emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;
                //emailMsg += "<b>Error Response:</b>" + Resp.ResponseString + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;

                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "PayPal Transaction Response-FAIL", emailMsg);

                throw ex;
            }
        }

        private void notifyAuthorizationRequest(AuthorizationRequest requestObject)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<b>FormID  :</b>  " + getFormID() + "<BR>");

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
                sb.AppendLine("<b>Country  :</b>  " + countryCode + "<BR>");
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
                EmailFunctions emailFunctions = new EmailFunctions();
                emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Authorize.Net Transaction Request", adminEmailbody);
            }
            catch
            {
                Errorlocation = "btnsecurepayment -  notifyAuthorizationRequest(AuthorizeNetRequest) - in catch";
            }
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
                    sb.AppendLine("<b>CardNum  :</b>  " + responseObject.CardNumber.Substring(responseObject.CardNumber.Length - 5, 4) + "<BR>");
                else
                    sb.AppendLine("<b>CardNum  :</b>  " + responseObject.CardNumber + "<BR>");

                sb.AppendLine("<b>CAVResponse  :</b>  " + responseObject.CAVResponse + "<BR>");
                sb.AppendLine("<b>CCVResponse  :</b>  " + responseObject.CCVResponse + "<BR>");
                sb.AppendLine("<b>AVSResponse  :</b>  " + responseObject.AVSResponse + "<BR>");
                sb.AppendLine("<b>Address  :</b>  " + responseObject.Address + "<BR>");
                sb.AppendLine("<b>City  :</b>  " + responseObject.City + "<BR>");
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
            EmailFunctions emailFunctions = new EmailFunctions();
            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Authorize.Net Transaction Response-FAIL", adminEmailbody);

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
            txtShippingAddress.Text = Request.QueryString["adr"] != null ? Request.QueryString["adr"].ToString() : "";
            txtShippingAddress2.Text = Request.QueryString["adr2"] != null ? Request.QueryString["adr2"].ToString() : "";
            txtShippingCity.Text = Request.QueryString["city"] != null ? Request.QueryString["city"].ToString() : "";
            txtShippingZip.Text = Request.QueryString["zc"] != null ? Request.QueryString["zc"].ToString() : "";
            txtphone.Text = Request.QueryString["ph"] != null ? Request.QueryString["ph"].ToString() : "";
            fax.Text = Request.QueryString["fax"] != null ? Request.QueryString["fax"].ToString() : "";
            txtemail.Text = Request.QueryString["e"] != null ? Request.QueryString["e"].ToString() : "";
            txtemail.ReadOnly = Request.QueryString["e"] != null ? true : false;

            if (Request.QueryString["state"] != null)
            {
                drpShippingState.SelectedIndex = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;
            }
            else
            {
                drpShippingState.SelectedIndex = drpShippingState.Items.IndexOf(drpShippingState.Items.FindByValue(""));
                drpShippingState.Items[drpShippingState.SelectedIndex].Selected = true;
            }

            foreach (GridViewRow grdRow in grdMagazines.Rows)
            {
                CheckBox chkPrint = (CheckBox)grdRow.FindControl("chkPrint");
                chkPrint.Checked = false;
            }

            phError.Visible = false;
        }

        protected void btnApplyPromoCode_Click(object sender, EventArgs e)
        {
            bool promoExists = Exists(txtPromoCode.Text, Convert.ToInt32(ViewState["CustomerID"]));
            if (promoExists)
            {
                ViewState["PromoCode"] = txtPromoCode.Text;
                PromoCode = txtPromoCode.Text;
                lblPromoCode.Text = txtPromoCode.Text.ToUpper();
                txtPromoCode.Text = "";
                btnRemovePromoCode.Visible = true;
                pnlPromoCode.Visible = false;
                updateGridView();
            }
            else
            {
                ViewState["PromoCode"] = null;
                PromoCode = "";
                lblPromoCode.Text = "Invalid Promo Code";
                btnRemovePromoCode.Visible = false;
                pnlPromoCode.Visible = true;
            }
            pnlPromoCodeDetails.Visible = true;
        }

        protected void btnRemovePromoCode_Click(object sender, EventArgs e)
        {
            ViewState["PromoCode"] = null;
            pnlPromoCodeDetails.Visible = false;
            pnlPromoCode.Visible = true;
            updateGridView();
        }

        protected void drpQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGridView();
        }

        protected void drpTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGridView();
        }

        protected void chBoxPrint_CheckedChanged(object sender, EventArgs e)
        {
            CalculatePrice();
        }

        #region GridView
        protected void grdMagazines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                updateGridViewRow(e.Row);
            }
        }

        private void updateGridView()
        {
            foreach (GridViewRow gvRow in grdMagazines.Rows)
            {
                updateGridViewRow(gvRow);
            }
            CalculatePrice();
        }

        private void updateGridViewRow(GridViewRow gvRow)
        {

            Panel pnlTerm = (Panel)gvRow.FindControl("pnlTerm");
            Panel pnlShippingOption = (Panel)gvRow.FindControl("pnlShippingOption");
            Label lblImageName = (Label)gvRow.FindControl("lblImageName");
            Label lblIsSubscription = (Label)gvRow.FindControl("lblIsSubscription");
            Image coverImage = (Image)gvRow.FindControl("lblImageID");
            if (!lblImageName.Text.Equals(string.Empty))
                coverImage.ImageUrl = "Images/" + lblImageName.Text;
            else
                coverImage.Visible = false;
            int ProductID = Convert.ToInt32(((Label)gvRow.FindControl("lblProductID")).Text);
            int PaidFormID = getFormID();
            bool showTerm = Convert.ToBoolean(lblIsSubscription.Text.ToString());
            DropDownList drpTerm = (DropDownList)gvRow.FindControl("drpTerm");
            string term = string.Empty;
            if (showTerm)
            {
                pnlTerm.Visible = true;
                if (drpTerm.Items.Count == 0)
                {
                    DataTable dt = GetProductTerm(ProductID);
                    drpTerm.DataValueField = "TermID";
                    drpTerm.DataTextField = "Term";
                    drpTerm.DataSource = dt;
                    drpTerm.DataBind();
                }
                term = drpTerm.SelectedValue;
            }
            else
                pnlTerm.Visible = false;

            DropDownList drpQuantity = (DropDownList)gvRow.FindControl("drpQuantity");
            if (ViewState["ShowQuantity"].ToString().ToLower().Equals("true"))
            {
                if (drpQuantity.Items.Count == 0)
                {
                    for (int i = 1; i <= Convert.ToInt32(ViewState["QuantityAllowed"].ToString()); i++)
                    {
                        drpQuantity.Items.Add(new ListItem(i.ToString()));
                    }
                }
            }


            string quantity = string.Empty;
            if (ViewState["ShowQuantity"].ToString().ToLower().Equals("true"))
                quantity = drpQuantity.SelectedValue;

            bool showAirmailOption = ShowAirmailOption(ProductID, term, Convert.ToInt32(drpCountry.SelectedValue));
            if (showAirmailOption)
                pnlShippingOption.Visible = true;
            else
                pnlShippingOption.Visible = false;

            RadioButtonList rblShipping = (RadioButtonList)gvRow.FindControl("rblShipping");
            bool airMail = Convert.ToBoolean(rblShipping.SelectedValue) && showAirmailOption;
            string PromoCode = ViewState["PromoCode"] == null ? string.Empty : ViewState["PromoCode"].ToString();

            Label lblPriceExpressShipping = (Label)gvRow.FindControl("lblPriceExpressShipping");
            Label lblPrice = (Label)gvRow.FindControl("lblPrice");
            string price = GetProductPrice(ProductID, term, quantity, Convert.ToInt32(drpCountry.SelectedValue), PromoCode, airMail);
            if (airMail == false)
            {
                lblPrice.Text = price;
                lblPriceExpressShipping.Text = "0";
                lblPriceExpressShipping.Visible = false;
                lblPrice.Visible = true;
            }
            else
            {
                lblPrice.Text = "0";
                lblPriceExpressShipping.Text = price;
                lblPriceExpressShipping.Visible = true;
                lblPrice.Visible = false;
            }
        }



        private void CalculatePrice()
        {
            //bool bundleChecked = false;
            //foreach (GridViewRow row in grdMagazines.Rows)
            //{
            //    Label IsBundleLabel = (Label)row.FindControl("lblIsBundle");
            //    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
            //    if (IsBundleLabel.Text.ToLower().Equals("true") && chkPrint.Checked == true)
            //    {
            //        bundleChecked = true;
            //    }
            //}

            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label totalLabel = (Label)row.FindControl("lblTotal");
                Label lblPrice = (Label)row.FindControl("lblPrice");
                Label lblPriceExpressShipping = (Label)row.FindControl("lblPriceExpressShipping");
                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                //Label IsBundleLabel = (Label)row.FindControl("IsBundle");
                //if (bundleChecked == true && IsBundleLabel.Text.ToLower().Equals("false"))
                //{
                //    chkPrint.Enabled = false;
                //    chkPrint.Checked = false;
                //}
                //else
                //{
                //    chkPrint.Enabled = true;
                //}

                if (chkPrint.Checked)
                {
                    if (lblPrice.Visible)
                        totalLabel.Text = String.Format("{0:C}", double.Parse(lblPrice.Text));
                    else
                        totalLabel.Text = String.Format("{0:C}", double.Parse(lblPriceExpressShipping.Text));
                    unitPrice = double.Parse(totalLabel.Text.TrimStart(new char[] { '$' }));
                    totalPrice += unitPrice;
                }
                else
                {
                    totalLabel.Text = "$0.00";
                    unitPrice = double.Parse(totalLabel.Text.TrimStart(new char[] { '$' }));
                    totalPrice += unitPrice;
                }

                Label lblTotAmount = (Label)grdMagazines.FooterRow.FindControl("lblTotalAmount");
                if (lblTotAmount != null) { lblTotAmount.Text = String.Format("{0:0.00}", totalPrice); }
            }
        }
        #endregion

        #region DataAccess
        private DataTable GetSubscriptionGroups(int ProductID)
        {
            SqlCommand cmd = new SqlCommand("sp_GetSubscriptionGroups");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int)).Value = ProductID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

        private bool Exists(string PromoCode, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand("sp_PromoCodeExists");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PromoCode", SqlDbType.VarChar)).Value = PromoCode;
            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int)).Value = CustomerID;
            bool exists = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
            return exists;
        }

        private DataTable GetForm(int PaidFormID)
        {
            SqlCommand cmd = new SqlCommand("select * from PaidForm where PaidFormID=@PaidFormID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PaidFormID", SqlDbType.Int)).Value = PaidFormID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

        private DataTable GetProducts(int PaidFormID)
        {
            SqlCommand cmd = new SqlCommand("sp_GetProductsByPaidFormID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PaidFormID", SqlDbType.Int)).Value = PaidFormID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

        private DataTable GetProductTerm(int ProductID)
        {
            SqlCommand cmd = new SqlCommand("sp_GetTermsBy_PaidFormID_ProductID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int)).Value = ProductID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt;
        }

        private DataTable GetCountries()
        {
            string sql = "SELECT CountryID, CountryName from Country WHERE CountryID NOT IN (174,205) ORDER BY CountryName ASC";
            DataTable dt = DataFunctions.GetDataTable(sql);
            return dt;
        }

        private string GetCountrCode(string countryID)
        {
            string sql = "SELECT CountryCode from Country WHERE CountryID = " + countryID;
            DataTable dt = DataFunctions.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        private string GetProductPrice(int ProductID, string Term, string Quantity, int CountryID, string PromoCode, bool AirMail)
        {
            SqlCommand cmd = new SqlCommand("sp_GetProductsPrice");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int)).Value = ProductID;
            if (!Term.Equals(string.Empty))
                cmd.Parameters.Add(new SqlParameter("@TermID", SqlDbType.Int)).Value = Convert.ToInt32(Term);
            if (!Quantity.Equals(string.Empty))
                cmd.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int)).Value = Convert.ToInt32(Quantity);
            if (!CountryID.Equals(string.Empty))
                cmd.Parameters.Add(new SqlParameter("@CountryID", SqlDbType.Int)).Value = CountryID;
            if (!PromoCode.Equals(string.Empty))
                cmd.Parameters.Add(new SqlParameter("@PromoCode", SqlDbType.VarChar)).Value = PromoCode;
            cmd.Parameters.Add(new SqlParameter("@AirMail", SqlDbType.Bit)).Value = AirMail;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return dt.Rows[0][0].ToString();
        }

        private bool ShowAirmailOption(int ProductID, string Term, int CountryID)
        {
            SqlCommand cmd = new SqlCommand("sp_ShowAirmailOption");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int)).Value = ProductID;
            if (!Term.Equals(string.Empty))
                cmd.Parameters.Add(new SqlParameter("@TermID", SqlDbType.Int)).Value = Convert.ToInt32(Term);
            if (!CountryID.Equals(string.Empty))
                cmd.Parameters.Add(new SqlParameter("@CountryID", SqlDbType.Int)).Value = CountryID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return Convert.ToInt32(dt.Rows[0][0].ToString()) == 1 ? true : false;
        }

        private bool IsSubscriptionRenewal(int CustomerID, string EmailAddress, int GroupID)
        {
            SqlCommand cmd = new SqlCommand("sp_IsSubscriptionRenewal");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int)).Value = CustomerID;
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.VarChar)).Value = EmailAddress;
            cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = GroupID;
            DataTable dt = DataFunctions.GetDataTable(cmd);
            return Convert.ToInt32(dt.Rows[0][0].ToString()) == 1 ? true : false;
        }
        #endregion

        protected void rblShipping_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGridView();
        }

        protected void btnClearTransCache_Click(object sender, EventArgs e)
        {
            RemoveFromCache(TransactionCacheName);
            ViewState["TransactionID"] = null;
            mpeDuplicateTrans.Hide();
        }
    }
}
