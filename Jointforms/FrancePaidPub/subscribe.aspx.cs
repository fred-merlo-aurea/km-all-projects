using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.Configuration;
using Encore.PayPal.Nvp;
using KMPS_JF_Objects.Objects;
using AuthorizeNet;
using System.Text.RegularExpressions;
using System.Configuration;
using ecn.communicator.classes;
using System.Web;
using System.Net;
namespace PaidPub
{
    public partial class subscribe : System.Web.UI.Page
    {
        private double unitPrice = 0.0;
        private double totalPrice = 0.0;
        private string countryCode = String.Empty;
        private string PubCodeList = string.Empty;
        private AuthorizationRequest ppPay = new AuthorizationRequest("", "", 0.00M, "");
        private List<PaidPub.Objects.Item> itemList = new List<PaidPub.Objects.Item>();
        private GatewayResponse response = null;

        #region PROPERTIES

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
                catch { return string.Empty; }
            }
            set
            {
                ViewState["PubCode"] = value;
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
                catch { return String.Empty; }
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

        private Enums.Region _region = Enums.Region.INTERNATIONAL;
        public Enums.Region Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
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
            string sql = "SELECT CountryID, CountryName from Country WHERE CountryID NOT IN (174,205) ORDER BY CountryName ASC";
            DataTable dt = PaidPub.Objects.DataFunctions.GetDataTable(sql);
            countryName.DataSource = dt;
            countryName.DataTextField = "CountryName";
            countryName.DataValueField = "CountryID";
            countryName.DataBind();
            countryName.Items.Insert(0, new ListItem("United States", "205"));
            countryName.Items.Insert(1, new ListItem("Canada", "174"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Uncomment when ready -- Justin Welter 08232016
            //ClientScriptManager csm = Page.ClientScript;
            //btnSecurePayment.Attributes.Add("onclick", "javascript:" + btnSecurePayment.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSecurePayment, ""));
            if (!IsPostBack)
            {
                LoadCountries();

                try
                {
                    countryCode = Request.QueryString["ctry"].ToString();
                }
                catch
                {
                    countryCode = "USA";
                }
                SetCountryCode();
                ShowHideSelect(true);
            }
            else
            {
                countryCode = countryName.SelectedItem.Text;
                SetCountryCode();
            }

            try
            {
                PubCode = Server.HtmlEncode(Request.QueryString["pubcode"].ToString());
            }
            catch { }

            try
            {
                foreach (ListItem li in countryName.Items)
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
                countryName.Items[0].Selected = true;
            }

            if (!IsPostBack)
            {
                loadstate(countryCode);
                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
                {
                    pnlState1.Visible = true;
                    pnlState2.Visible = false;
                    if (Request.QueryString["state"] != null)
                    {
                        state.SelectedIndex = state.Items.IndexOf(state.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                        state.Items[state.SelectedIndex].Selected = true;
                    }
                }
                else
                {
                    pnlState1.Visible = false;
                    pnlState2.Visible = true;
                    if (Request.QueryString["state"] != null)
                    {
                        txtState.Text = Request.QueryString["state"].ToString();
                    }
                }

                grdMagazines.DataSource = GetMagList();
                grdMagazines.DataBind();
                updateGridView();

                first.Text = Request.QueryString["fn"] != null ? Request.QueryString["fn"].ToString() : "";
                last.Text = Request.QueryString["ln"] != null ? Request.QueryString["ln"].ToString() : "";
                company.Text = Request.QueryString["compname"] != null ? Request.QueryString["compname"].ToString() : "";
                address1.Text = Request.QueryString["adr"] != null ? Request.QueryString["adr"].ToString() : "";
                address2.Text = Request.QueryString["adr2"] != null ? Request.QueryString["adr2"].ToString() : "";
                city.Text = Request.QueryString["city"] != null ? Request.QueryString["city"].ToString() : "";
                zip.Text = Request.QueryString["zc"] != null ? Request.QueryString["zc"].ToString() : "";
                phone.Text = Request.QueryString["ph"] != null ? Request.QueryString["ph"].ToString() : "";
                fax.Text = Request.QueryString["fax"] != null ? Request.QueryString["fax"].ToString() : "";
                email.Text = Request.QueryString["e"] != null ? Request.QueryString["e"].ToString() : "";
                email.ReadOnly = Request.QueryString["e"] != null ? true : false;
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

        public void SetCountryCode()
        {
            if (!countryCode.Equals(String.Empty))
            {
                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "US" || countryCode.ToUpper() == "USA")
                {
                    countryCode = "UNITED STATES";
                }
                else if (countryCode.ToUpper() == "CANADA" || countryCode.ToUpper() == "CA")
                {
                    countryCode = "CANADA";
                }
            }
        }

        private DataTable GetMagList()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("xml/MagList.xml"));
            DataTable magList = ds.Tables[0];
            return magList;
        }

        protected void drpQuantity_SelectedIndexChanged(Object sender, EventArgs e)
        {
            this.countryCode = countryName.SelectedItem.Text;

            foreach (GridViewRow row in grdMagazines.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label lblUsPrice1 = (Label)row.FindControl("lblUsPrice1");
                    Label lblUsPrice2 = (Label)row.FindControl("lblUsPrice2");
                    Label lblCanPrice1 = (Label)row.FindControl("lblCanPrice1");
                    Label lblCanPrice2 = (Label)row.FindControl("lblCanPrice2");
                    Label lblIntPrice1 = (Label)row.FindControl("lblIntPrice1");
                    Label lblIntPrice2 = (Label)row.FindControl("lblIntPrice2");
                    DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");

                    switch (Convert.ToInt32(ddlQuantity.SelectedItem.Value))
                    {
                        case 1:
                            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES")
                            {
                                lblUsPrice1.Visible = true;
                                lblUsPrice2.Visible = false;
                                lblCanPrice1.Visible = false;
                                lblCanPrice2.Visible = false;
                                lblIntPrice1.Visible = false;
                                lblIntPrice2.Visible = false;
                                lblUsPrice1.Text = String.Format("{0:0.00}", (Convert.ToDecimal(lblUsPrice1.Text) / 1M).ToString());
                            }
                            else if (countryCode.ToUpper() == "CANADA")
                            {
                                lblUsPrice1.Visible = false;
                                lblUsPrice2.Visible = false;
                                lblCanPrice1.Visible = true;
                                lblCanPrice2.Visible = false;
                                lblIntPrice1.Visible = false;
                                lblIntPrice2.Visible = false;
                                lblCanPrice1.Text = String.Format("{0:0.00}", (Convert.ToDecimal(lblCanPrice1.Text) / 1M).ToString());
                            }
                            else
                            {
                                lblUsPrice1.Visible = false;
                                lblUsPrice2.Visible = false;
                                lblCanPrice1.Visible = false;
                                lblCanPrice2.Visible = false;
                                lblIntPrice1.Visible = true;
                                lblIntPrice2.Visible = false;
                                lblIntPrice1.Text = String.Format("{0:0.00}", (Convert.ToDecimal(lblIntPrice1.Text) / 1M).ToString());
                            }
                            break;
                        case 2:
                            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES")
                            {
                                lblUsPrice1.Visible = false;
                                lblUsPrice2.Visible = true;
                                lblCanPrice1.Visible = false;
                                lblCanPrice2.Visible = false;
                                lblIntPrice1.Visible = false;
                                lblIntPrice2.Visible = false;
                                lblUsPrice2.Text = String.Format("{0:0.00}", (Convert.ToDecimal(lblUsPrice2.Text) / 1M).ToString());
                            }
                            else if (countryCode.ToUpper() == "CANADA")
                            {
                                lblUsPrice1.Visible = false;
                                lblUsPrice2.Visible = false;
                                lblCanPrice1.Visible = false;
                                lblCanPrice2.Visible = true;
                                lblIntPrice1.Visible = false;
                                lblIntPrice2.Visible = false;
                                lblCanPrice2.Text = String.Format("{0:0.00}", (Convert.ToDecimal(lblCanPrice2.Text) / 1M).ToString());
                            }
                            else
                            {
                                lblUsPrice1.Visible = false;
                                lblUsPrice2.Visible = false;
                                lblCanPrice1.Visible = false;
                                lblCanPrice2.Visible = false;
                                lblIntPrice1.Visible = false;
                                lblIntPrice2.Visible = true;
                                lblIntPrice2.Text = String.Format("{0:0.00}", (Convert.ToDecimal(lblIntPrice2.Text) / 1M).ToString());
                            }
                            break;
                    }
                }
            }
            CalculatePrice();
        }

        private bool IsPrintEditionAvailable(string pubCode, string CountryName)
        {
            Publication pub = Publication.GetPublicationbyID(0, pubCode);
            string sql = "select pf.PFID from PubFormsForCountry pfc join PubForms pf on pf.PFID = pfc.PFID join Country c on c.CountryID = pfc.CountryID where c.CountryName = '" + countryName.SelectedItem.Text.ToString() + "' and pf.PubID = " + pub.PubID.ToString() + " and pf.ShowPrint = 0";
            DataTable dt = PaidPub.Objects.DataFunctions.GetDataTable(sql);

            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public void updateGridView()
        {
            int quantity = 0;
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");
                Label lblQuantity = (Label)row.FindControl("lblQuantity");
                quantity = Convert.ToInt32(ddlQuantity.SelectedItem.Value);

                Label unitPrice = (Label)row.FindControl("lblTotal");
                Label groupID = (Label)row.FindControl("lblGroupID");
                unitPrice.Text = "$0.00";

                Label usLabel1 = (Label)row.FindControl("lblUsPrice1");
                Label canLabel1 = (Label)row.FindControl("lblCanPrice1");
                Label intLabel1 = (Label)row.FindControl("lblIntPrice1");


                Label usLabel2 = (Label)row.FindControl("lblUsPrice2");
                Label canLabel2 = (Label)row.FindControl("lblCanPrice2");
                Label intLabel2 = (Label)row.FindControl("lblIntPrice2");

                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                Label lblPubCode = (Label)row.FindControl("lblPubCode");
                Label lblMessage = (Label)row.FindControl("lblMessage");

                bool isPrintVisible = true;

                try
                {
                    isPrintVisible = IsPrintEditionAvailable(lblPubCode.Text, countryName.SelectedItem.Text);
                }
                catch  
                {
                    isPrintVisible = false;
                }

                chkPrint.Visible = isPrintVisible;
                if (quantity == 1)
                {
                    usLabel1.Visible = countryCode.ToUpper() == "UNITED STATES" && isPrintVisible;
                    canLabel1.Visible = countryCode.ToUpper() == "CANADA" && isPrintVisible;
                    intLabel1.Visible = countryCode.ToUpper() != "UNITED STATES" && countryCode.ToUpper() != "CANADA" && isPrintVisible;
                }
                else
                {
                    usLabel2.Visible = countryCode.ToUpper() == "UNITED STATES" && isPrintVisible;
                    canLabel2.Visible = countryCode.ToUpper() == "CANADA" && isPrintVisible;
                    intLabel2.Visible = countryCode.ToUpper() != "UNITED STATES" && countryCode.ToUpper() != "CANADA" && isPrintVisible;
                }

                if (!isPrintVisible)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Not Available";
                }
            }
        }

        protected void ListCountryChanged(object sender, EventArgs e)
        {
            hfProductCount.Value = "0";
            ShowHideSelect(true);
            this.countryCode = countryName.SelectedItem.Text;
            SetCountryCode();
            grdMagazines.DataSource = GetMagList();
            grdMagazines.DataBind();
            updateGridView();
            if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")
            {
                pnlState1.Visible = true;
                pnlState2.Visible = false;
            }
            else
            {
                pnlState1.Visible = false;
                pnlState2.Visible = true;
            }
            loadstate(countryCode);
        }

        public void CalculatePrice()
        {
            int quantity = 0;
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label totalLabel = (Label)row.FindControl("lblTotal");
                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");
                Label lblQuantity = (Label)row.FindControl("lblQuantity");
                quantity = Convert.ToInt32(ddlQuantity.SelectedItem.Value);

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES")
                {
                    ShowUSLabel(row, totalLabel, chkPrint, quantity);
                }
                else if (countryCode.ToUpper() == "CANADA")
                {
                    ShowCanLabel(row, totalLabel, chkPrint, quantity);
                }
                else
                {
                    ShowIntLabel(row, totalLabel, chkPrint, quantity);
                }
            }
        }

        private void ShowIntLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint, int quantity)
        {
            Label intLabel = new Label();
            if (quantity == 1)
            {
                intLabel = (Label)row.FindControl("lblIntPrice1");
            }
            else
            {
                intLabel = (Label)row.FindControl("lblIntPrice2");
            }

            if (chkPrint.Checked)
            {
                totalLabel.Text = String.Format("{0:C}", double.Parse(intLabel.Text));
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

        private void ShowCanLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint, int quantity)
        {
            Label canLabel = new Label();

            if (quantity == 1)
            {
                canLabel = (Label)row.FindControl("lblCanPrice1");
            }
            else
            {
                canLabel = (Label)row.FindControl("lblCanPrice2");
            }

            if (chkPrint.Checked)
            {
                totalLabel.Text = String.Format("{0:C}", double.Parse(canLabel.Text));
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

        private void ShowUSLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint, int quantity)
        {
            Label usLabel = new Label();
            if (quantity == 1)
            {
                usLabel = (Label)row.FindControl("lblUsPrice1");
            }
            else
            {
                usLabel = (Label)row.FindControl("lblUsPrice2");
            }

            if (chkPrint.Checked)
            {
                totalLabel.Text = String.Format("{0:C}", double.Parse(usLabel.Text));
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

        public void chBoxPrint_CheckedChanged(object sender, EventArgs e)
        {
            CalculatePrice();
        }

        public double GetTotal()
        {
            Label lblTotAmount = (Label)grdMagazines.FooterRow.FindControl("lblTotalAmount");
            return double.Parse(lblTotAmount.Text);
        }

        #region State Dropdown
        private void loadstate(string countrycode)
        {
            state.Items.Clear();

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

            state.Items.Add(item);

        }
        #endregion

        protected void btnSecurePayment_Click(object sender, EventArgs e)
        {
            bool redirectstatus = false;
            bool redirectstatusall = true;
            StringBuilder sb;
            int selectioncount = 0;
            string TransactionID = string.Empty;
            String SubscriberID = string.Empty;

            //Step 1 - Post to regular Fields

            if (Page.IsValid)
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

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

                foreach (GridViewRow row in grdMagazines.Rows)
                {
                    sb = new StringBuilder();
                    PaidPub.Objects.Item item = new PaidPub.Objects.Item();
                    Label groupID = (Label)row.FindControl("lblGroupID");
                    Label custID = (Label)row.FindControl("lblcustID");
                    Label lblPubCode = (Label)row.FindControl("lblPubCode");
                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");
                    double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));

                    if (chkPrint.Checked == true)
                    {
                        selectioncount++;

                        if (chkPrint.Checked) { sb.Append("user_DEMO7=A&"); }

                        sb.Append("g=" + groupID.Text);
                        sb.Append("&fn=" + first.Text);
                        sb.Append("&ln=" + last.Text);
                        sb.Append("&compname=" + company.Text);
                        sb.Append("&adr=" + address1.Text);
                        sb.Append("&adr2=" + address2.Text);
                        sb.Append("&city=" + city.Text);

                        if (countryCode == "UNITED STATES OF AMERICA"  || countryCode == "UNITED STATES" || countryCode == "CANADA")
                        {
                            sb.Append("&state=" + state.Text);
                        }
                        else
                        {
                            sb.Append("&state=" + txtState.Text);
                        }

                        sb.Append("&zc=" + zip.Text);
                        sb.Append("&ctry=" + countryCode);
                        sb.Append("&ph=" + phone.Text);
                        sb.Append("&fax=" + fax.Text);
                        sb.Append("&e=" + email.Text);
                        sb.Append("&c=" + custID.Text);
                        sb.Append("&f=html");
                        sb.Append("&user_PaymentStatus=pending");
                        sb.Append("&user_DEMO39=" + PromoCode);
                        sb.Append("&user_PUBLICATIONCODE=" + lblPubCode.Text.ToString());

                        item.ItemCode = lblPubCode.Text;
                        item.ItemName = lblPubCode.Text;
                        item.ItemQty = ddlQuantity.SelectedItem.Value;
                        item.ItemAmount = totalAmt.ToString();
                        item.GroupID = int.Parse(groupID.Text);
                        item.CustID = int.Parse(custID.Text);
                        item.Description = lblPubCode.Text;
                        itemList.Add(item);

                        if (lblPubCode.Text.ToLower().Equals(PubCode.ToLower()))
                        {
                            if (Request.QueryString != null)
                                redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text.ToString(), Request.QueryString.ToString(), browserInfo) && ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text.ToString(), sb.ToString(), browserInfo);
                            else
                                redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text.ToString() , sb.ToString(), browserInfo);
                        }
                        else
                        {
                            redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text.ToString(), sb.ToString(), browserInfo);
                        }

                        if (!redirectstatus) { redirectstatusall = redirectstatus; }

                    } //end if
                } // end foreach

                if (selectioncount == 0)
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Please select the magazine you want to subscribe.";
                    return;
                }

                //Step 2 - Authorize and charge the credit card

                if (redirectstatusall)
                {
                    TransactionID = ViewState["TRANSACTIONID"] == null ? "" : ViewState["TRANSACTIONID"].ToString();

                    if (String.IsNullOrEmpty(TransactionID))
                    {
                        if (!ValidateCreditCard())
                        {
                            phError.Visible = true;
                            string msg = string.Empty;

                            if (response != null)
                                lblErrorMessage.Text = response.ResponseCode + " " + response.Message;
                            else
                                lblErrorMessage.Text = "Error Processing Credit Card.";


                            return;
                        }
                        else
                        {
                            TransactionID = response.TransactionID;
                        }
                    }
                }


                // Step 3  - Post the transactional UDF

                foreach (GridViewRow row in grdMagazines.Rows)
                {
                    sb = new StringBuilder();
                    Label groupID = (Label)row.FindControl("lblGroupID");
                    Label custID = (Label)row.FindControl("lblcustID");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    Label lblPubCode = (Label)row.FindControl("lblPubCode");
                    DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");

                    double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));

                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");

                    if (chkPrint.Checked == true)
                    {
                        sb.Append("g=" + groupID.Text);
                        sb.Append("&e=" + email.Text);
                        sb.Append("&f=html");
                        sb.Append("&c=" + custID.Text);
                        sb.Append("&user_PaymentStatus=paid");
                        sb.Append("&user_PAIDorFREE=PAID");

                        //post to transactional UDF

                        sb.Append("&user_t_Term=" + ddlQuantity.SelectedValue);
                        sb.Append("&user_t_TermStartDate=" + DateTime.Now.ToString("MM/dd/yyyy"));
                        sb.Append("&user_t_TermEndDate=" + DateTime.Now.AddYears(Convert.ToInt32(ddlQuantity.SelectedValue)).ToString("MM/dd/yyyy"));

                        sb.Append("&user_t_FullName=" + user_CardHolderName.Text.Trim());

                        if (user_CardHolderName.Text.Trim().Contains(" "))
                        {
                            string fullName=user_CardHolderName.Text;
                            string firstName =fullName.Split(new char[] { ' ' })[0];
                            string lastName = fullName.Substring(firstName.Length + 1, fullName.Length - firstName.Length -1);
                            sb.Append("&user_t_FirstName=" + firstName);
                            sb.Append("&user_t_LastName=" + lastName);
                        }
                        else
                        {
                            sb.Append("&user_t_FirstName=" + user_CardHolderName.Text);
                            sb.Append("&user_t_LastName=" + "");
                        }

                        sb.Append("&user_t_CardType=" + user_CCType.SelectedItem.Value);
                        sb.Append("&user_t_CardNumber=" + "************" + user_CCNumber.Text.Substring(user_CCNumber.Text.Trim().Length - 4, 4));
                        sb.Append(String.Format("&user_t_ExpirationDate={0}/{1}", user_Exp_Month.SelectedItem.Value, user_Exp_Year.SelectedItem.Value));
                        sb.Append("&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy"));
                        sb.Append("&user_t_AmountPaid=" + String.Format("{0:0.00}", totalAmt));
                        sb.Append("&user_t_Street=" + address1.Text);
                        sb.Append("&user_t_Street2=" + address2.Text);
                        sb.Append("&user_t_City=" + city.Text);

                        if (countryCode == "UNITED STATES OF AMERICA" || countryCode == "UNITED STATES" || countryCode == "CANADA")
                        {
                            sb.Append("&user_t_State=" + state.SelectedItem.Value);
                        }
                        else
                        {
                            sb.Append("&user_t_State=" + txtState.Text);
                        }

                        sb.Append("&user_t_Zip=" + zip.Text);
                        sb.Append("&user_t_Country=" + countryCode);
                        sb.Append("&user_t_TransactionID=" + TransactionID);

                        sb.Append("&user_SHIPTO_ADDRESS1=" + address1.Text);
                        sb.Append("&user_SHIPTO_ADDRESS2=" + address2.Text);
                        sb.Append("&user_SHIPTO_CITY=" + city.Text);

                        if (pnlState1.Visible)
                        {
                            sb.Append("&user_SHIPTO_STATE=" + state.SelectedItem.Value);
                            sb.Append("&user_SHIPTO_ZIP=" + zip.Text);
                        }
                        else
                        {
                            sb.Append("&user_SHIPTO_STATE_INT=" + txtState.Text);
                            sb.Append("&user_SHIPTO_FORZIP=" + zip.Text);
                        }



                        redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text.ToString(), sb.ToString(), browserInfo); 

                        if (!redirectstatus) { redirectstatusall = redirectstatus; }
                    }

                } //end for loop

                if (redirectstatusall == true)
                {
                    ViewState["TRANSACTIONID"] = string.Empty;

                    if (Request.QueryString["rURL"] != null)
                    {
                        string rRUL = Request.QueryString["rURL"];
                        if (!rRUL.Contains("http"))
                            rRUL = "http://" + rRUL;

                        if (rRUL.Contains("http%3a%2f%2f"))
                            rRUL = rRUL.Replace("http%3a%2f%2f", "http://");


                        Response.Redirect(rRUL + "?pubcode=" + PubCode + "&emailaddress=" + email.Text);
                    }
                    else
                    {
                        Response.Redirect("Forms/thankyou.htm");
                    }
                }
                else
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Error in posting the data. Please try again!!";
                    return;
                }
            }
            else
            {
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

        private bool ValidateCreditCard()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string MerchantId = WebConfigurationManager.AppSettings["AuthorizeDotnetLogin"].ToString();
                string Signature = WebConfigurationManager.AppSettings["AuthorizeDotnetKey"].ToString();
                string countrCode = "";
                int totalQuantity = 0;
                string orderdesc = "";
                string orderNames = "";
                string orderItems = string.Empty;
                if (countryName.SelectedItem.Text.ToUpper() == "UNITED STATES OF AMERICA" || countryName.SelectedItem.Text.ToUpper() == "UNITED STATES")
                {
                    countrCode = "US";
                }
                else if (countryName.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    countrCode = "CA";
                }
                else
                {
                    countrCode = countryName.SelectedItem.Text.ToUpper();
                }
                string CardNo = Regex.Replace(user_CCNumber.Text, @"[ -/._#]", "");
                CardNo = CardNo.Trim().Replace(" ", "");
                var gateway = new Gateway(MerchantId, Signature);
                gateway.TestMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["AuthorizeDotNetDemoMode"].ToString()); 
            
                ppPay.CardNum = CardNo;
                ppPay.ExpDate = user_Exp_Month.Text + user_Exp_Year.Text;
                ppPay.Amount = GetTotal().ToString().TrimStart('$');
                ppPay.AddInvoice(PromoCode);
                ppPay.Company = company.Text;
                ppPay.AddCardCode(user_CCVerfication.Text);
                ppPay.Country = countrCode;
                ppPay.CustomerIp = System.Web.HttpContext.Current.Request.UserHostAddress;
                ppPay.Phone = phone.Text;
                ppPay.City = city.Text;
                ppPay.Email = email.Text;
                ppPay.Fax = fax.Text;

                if (this.itemList.Count > 0)
                {
                    string SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, email.Text);

                    if (countrCode.ToUpper() == "US" || countrCode.ToUpper() == "CA")
                    {
                        ppPay.AddCustomer(SubscriberID, email.Text, first.Text, last.Text, address1.Text + " " + address2.Text, city.Text, state.SelectedItem.Value, zip.Text);
                        ppPay.AddShipping(SubscriberID, email.Text, first.Text, last.Text, address1.Text + " " + address2.Text, city.Text, state.SelectedItem.Value, zip.Text);
                    }
                    else
                    {
                        ppPay.AddCustomer(SubscriberID, email.Text, first.Text, last.Text, address1.Text + " " + address2.Text, city.Text, txtState.Text, zip.Text);
                        ppPay.AddShipping(SubscriberID, email.Text, first.Text, last.Text, address1.Text + " " + address2.Text, city.Text, txtState.Text, zip.Text);
                    }
                    ppPay.ShipToCity = city.Text;
                    ppPay.ShipToCountry = countrCode;
                }          
                foreach (PaidPub.Objects.Item item in itemList)
                {
                    orderItems += item.ItemCode + ",";
                    orderdesc += item.Description + ",";
                    orderNames += item.ItemName + ",";
                    totalQuantity += Convert.ToInt32(item.ItemQty);
                }

                string itemID = orderItems.Trim(',');
                string itemName = orderNames.Trim(',');
                string itemDesc = orderdesc.Trim(',');
                decimal itemPrice = Convert.ToDecimal(ppPay.Amount) / Convert.ToDecimal(totalQuantity);
                ppPay.AddLineItem("France Media", "Paid Publications", itemDesc, 1, Convert.ToDecimal(ppPay.Amount), false);
                ppPay.AddMerchantValue("x_Description", itemDesc);               
                response = (GatewayResponse)gateway.Send(ppPay);
                if (!response.Approved)
                {
                    notifyTransactionFail(response, null);
                }

                return response.Approved;
            }
            catch (Exception ex)
            {
                notifyTransactionFail(response, ex);
                throw ex;
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

            first.Text = Request.QueryString["fn"] != null ? Request.QueryString["fn"].ToString() : "";
            last.Text = Request.QueryString["ln"] != null ? Request.QueryString["ln"].ToString() : "";
            company.Text = Request.QueryString["compname"] != null ? Request.QueryString["compname"].ToString() : "";
            address1.Text = Request.QueryString["adr"] != null ? Request.QueryString["adr"].ToString() : "";
            address2.Text = Request.QueryString["adr2"] != null ? Request.QueryString["adr2"].ToString() : "";
            city.Text = Request.QueryString["city"] != null ? Request.QueryString["city"].ToString() : "";
            zip.Text = Request.QueryString["zc"] != null ? Request.QueryString["zc"].ToString() : "";
            phone.Text = Request.QueryString["ph"] != null ? Request.QueryString["ph"].ToString() : "";
            fax.Text = Request.QueryString["fax"] != null ? Request.QueryString["fax"].ToString() : "";
            email.Text = Request.QueryString["e"] != null ? Request.QueryString["e"].ToString() : "";
            email.ReadOnly = Request.QueryString["e"] != null ? true : false;

            if (Request.QueryString["state"] != null)
            {
                state.SelectedIndex = state.Items.IndexOf(state.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                state.Items[state.SelectedIndex].Selected = true;
            }
            else
            {
                state.SelectedIndex = state.Items.IndexOf(state.Items.FindByValue(""));
                state.Items[state.SelectedIndex].Selected = true;
            }

            foreach (GridViewRow grdRow in grdMagazines.Rows)
            {
                CheckBox chkPrint = (CheckBox)grdRow.FindControl("chkPrint");
                chkPrint.Checked = false;
            }

            phError.Visible = false;
        }

        protected void grdMagazines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            this.countryCode = countryName.SelectedItem.Text;
            Label lblPubCode = (Label)e.Row.FindControl("lblPubCode");
            Label lblUsPrice1 = (Label)e.Row.FindControl("lblUsPrice1");
            Label lblUsPrice2 = (Label)e.Row.FindControl("lblUsPrice2");
            Label lblCanPrice1 = (Label)e.Row.FindControl("lblCanPrice1");
            Label lblCanPrice2 = (Label)e.Row.FindControl("lblCanPrice2");
            Label lblIntPrice1 = (Label)e.Row.FindControl("lblIntPrice1");
            Label lblIntPrice2 = (Label)e.Row.FindControl("lblIntPrice2");

            DropDownList ddlQuantity = (DropDownList)e.Row.FindControl("drpQuantity");
            Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
            DataTable dtpubs = null;
            int quantity = 0;
            int rowIndex = Convert.ToInt32(hfProductCount.Value);
            if (lblPubCode != null)
            {
                dtpubs = this.GetMagList();
                quantity = Convert.ToInt32(dtpubs.Rows[rowIndex]["quantity"]);
                if (quantity == 1)
                {
                    if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES")
                    {
                        totalPrice = Convert.ToDouble(dtpubs.Rows[rowIndex]["uspriceone"]);
                    }
                    else if (countryCode.ToUpper() == "CANADA")
                    {
                        totalPrice = Convert.ToDouble(dtpubs.Rows[rowIndex]["canpriceone"]);
                    }
                    else
                    {
                        totalPrice = Convert.ToDouble(dtpubs.Rows[rowIndex]["intpriceone"]);
                    }
                }
                else
                {
                    if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES")
                    {
                        totalPrice = Convert.ToDouble(dtpubs.Rows[rowIndex]["uspricetwo"]);
                    }
                    else if (countryCode.ToUpper() == "CANADA")
                    {
                        totalPrice = Convert.ToDouble(dtpubs.Rows[rowIndex]["canpricetwo"]);
                    }
                    else
                    {
                        totalPrice = Convert.ToDouble(dtpubs.Rows[rowIndex]["intpricetwo"]);
                    }
                }

                if (ddlQuantity != null && !lblQuantity.Visible)
                {
                    for (int i = 1; i <= quantity; i++)
                        ddlQuantity.Items.Add(new ListItem(i == 1 ? i.ToString() + " year" : i.ToString() + " years", i.ToString()));

                    ddlQuantity.ClearSelection();
                    ddlQuantity.Items[0].Selected = true;
                }
                rowIndex = rowIndex + 1;
                hfProductCount.Value = rowIndex.ToString();
            }
        }
    }
}