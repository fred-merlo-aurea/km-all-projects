using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using System.Text;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using Encore.PayPal.Nvp;
using wattpaidpub.Objects;
using KMPS_JF_Objects.Objects;
using System.Configuration;

namespace wattpaidpub
{
    public partial class subscribe : System.Web.UI.Page
    {
        private double unitPrice = 0.0;
        private double totalPrice = 0.0;
        private string countryCode = String.Empty;
        private string PubCodeList = string.Empty;
        private NvpDoDirectPayment ppPay = null;
        private List<wattpaidpub.Objects.Item> itemList = new List<wattpaidpub.Objects.Item>();

        public string PubCode
        {
            get
            {
                try 
                { 
                    return ViewState["PubCode"].ToString();    
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
                    return Request.QueryString["promocode"].ToString();
                }
                catch { return String.Empty; }     
            }
            set
            {
                ViewState["promocode"] = value; 
            }
        }

        private void LoadCountries()
        {
            string sql = "SELECT CountryID, CountryName from Country WHERE CountryID NOT IN (174,205) ORDER BY CountryName ASC";  
            DataTable dt = wattpaidpub.Objects.DataFunctions.GetDataTable(sql);  
            countryName.DataSource = dt;
            countryName.DataTextField = "CountryName";
            countryName.DataValueField = "CountryID";
            countryName.DataBind(); 
            countryName.Items.Insert(0, new ListItem("United States","205"));
            countryName.Items.Insert(1, new ListItem("Canada","174"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Uncomment when ready -- Justin Welter 08232016
            //ClientScriptManager csm = Page.ClientScript;
            //btnSecurePayment.Attributes.Add("onclick", "javascript:" + btnSecurePayment.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSecurePayment, ""));
            ppPay = new NvpDoDirectPayment();

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
                pnlState.Visible = countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA";
                loadstate(countryCode);

                if (pnlState.Visible && Request.QueryString["state"] != null)
                {
                    state.SelectedIndex = state.Items.IndexOf(state.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                    state.Items[state.SelectedIndex].Selected = true;  
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

            if (String.IsNullOrEmpty(PubCode))
            {
                return magList;
            }
            else
            {
                DataTable newMagList = magList.Clone();
                string expression = "pubcode = '" + PubCode + "'";
                DataRow[] results = magList.Select(expression);

                foreach (DataRow dr in results)
                {
                    newMagList.ImportRow(dr);
                }

                return newMagList;
            }
        }
                                                                                     
        private bool IsPrintEditionAvailable(string pubCode, string CountryName) 
        {                      
            Publication pub = Publication.GetPublicationbyID(0, pubCode); 
            string sql = "select pf.PFID from PubFormsForCountry pfc join PubForms pf on pf.PFID = pfc.PFID join Country c on c.CountryID = pfc.CountryID where c.CountryName = '" + countryName.SelectedItem.Text.ToString()  + "' and pf.PubID = " + pub.PubID.ToString() + " and pf.ShowPrint = 0";
            DataTable dt = wattpaidpub.Objects.DataFunctions.GetDataTable(sql);         

            if (dt.Rows.Count > 0)
                return false;
            else
                return true;  
        }

        public void updateGridView()
        {
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label unitPrice = (Label)row.FindControl("lblTotal");
                Label groupID = (Label)row.FindControl("lblGroupID");
                unitPrice.Text = "$0.00";

                Label usLabel = (Label)row.FindControl("lblUsPrice");
                Label canLabel = (Label)row.FindControl("lblCanPrice");
                Label intLabel = (Label)row.FindControl("lblIntPrice");
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
                usLabel.Visible = countryCode.ToUpper() == "UNITED STATES" && isPrintVisible;
                canLabel.Visible = countryCode.ToUpper() == "CANADA" && isPrintVisible;
                intLabel.Visible = countryCode.ToUpper() != "UNITED STATES" && countryCode.ToUpper() != "CANADA" && isPrintVisible;

                if (!isPrintVisible)
                {
                    lblMessage.Visible = true; 
                    lblMessage.Text = "Not Available";                   
                }
            }
        }

        protected void ListCountryChanged(object sender, EventArgs e)
        {
            ShowHideSelect(true);
            this.countryCode = countryName.SelectedItem.Text;
            SetCountryCode();
            grdMagazines.DataSource = GetMagList();
            grdMagazines.DataBind();
            updateGridView();
            pnlState.Visible = countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA";
            loadstate(countryCode);
        } 

        public void CalculatePrice()
        {
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label totalLabel = (Label)row.FindControl("lblTotal");              
                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");

                if ((countryCode.ToUpper() == "UNITED STATES OF AMERICA") || (countryCode.ToUpper() == "UNITED STATES"))
                {
                    ShowUSLabel(row, totalLabel,chkPrint);
                }
                else if (countryCode.ToUpper() == "CANADA")
                {
                    ShowCanLabel(row, totalLabel, chkPrint);       
                }
                else
                {
                    ShowIntLabel(row, totalLabel, chkPrint);      
                }
            }
        }

        private void ShowIntLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint)
        {
            Label intLabel = (Label)row.FindControl("lblIntPrice");

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

        private void ShowCanLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint)
        {
            Label canLabel = (Label)row.FindControl("lblCanPrice");

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

        private void ShowUSLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint)
        {
            Label usLabel = (Label)row.FindControl("lblUsPrice");

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

            if ((countryCode.ToUpper() == "UNITED STATES OF AMERICA") || (countryCode.ToUpper() == "UNITED STATES"))
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
                    wattpaidpub.Objects.Item item = new wattpaidpub.Objects.Item();
                    Label groupID = (Label)row.FindControl("lblGroupID");
                    Label custID = (Label)row.FindControl("lblcustID");
                    Label lblPubCode = (Label)row.FindControl("lblPubCode");                
                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));

                    if (chkPrint.Checked == true)
                    {
                        selectioncount++;

                        if (chkPrint.Checked) { sb.Append("user_DEMO7=A"); }                        

                        sb.Append("&g=" + groupID.Text);
                        sb.Append("&fn=" + first.Text);
                        sb.Append("&ln=" + last.Text);
                        sb.Append("&compname=" + company.Text);
                        sb.Append("&adr=" + address1.Text);
                        sb.Append("&adr2=" + address2.Text);
                        sb.Append("&city=" + city.Text);

                        if (countryCode == "UNITED STATES OF AMERICA" || countryCode == "UNITED STATES" || countryCode == "CANADA")
                        {
                            sb.Append("&state=" + state.Text);
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

                        item.ItemName = lblPubCode.Text;
                        item.ItemAmount = totalAmt.ToString();
                        item.ItemQty = "1";
                        item.GroupID = int.Parse(groupID.Text);
                        item.CustID = int.Parse(custID.Text);
                        itemList.Add(item);

                        redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text, sb.ToString(), browserInfo);

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
                            lblErrorMessage.Text = ppPay.ResponseString;

                            for (int i = 0; i < ppPay.ErrorList.Count; i++)
                            {
                                msg += ppPay.ErrorList[i].LongMessage + "<br />";
                                //msg += ppPay.ErrorList[i].Code + "<br />";
                            }

                            lblErrorMessage.Text = msg;
                            //btnSecurePayment.Enabled = true;
                            //btnSecurePayment.Text = "Process Payment";   
                            return;
                        }
                        else
                        {
                            ViewState["TRANSACTIONID"] = ppPay.Get(NvpDoDirectPayment.Response.TRANSACTIONID.ToString()); // session can be other option
                            TransactionID = ViewState["TRANSACTIONID"].ToString();                                 
                        }
                    }
                }


                // Step 3  - Post the transactional UDF

                foreach (GridViewRow row in grdMagazines.Rows)
                {
                    sb = new StringBuilder();
                    Label groupID = (Label)row.FindControl("lblGroupID");
                    Label custID = (Label)row.FindControl("lblcustID");
                    Label lblPubCode = (Label)row.FindControl("lblPubCode"); 

                    Label usLabel = (Label)row.FindControl("lblUsPrice");
                    Label canLabel = (Label)row.FindControl("lblCanPrice");
                    Label intLabel = (Label)row.FindControl("lblIntPrice");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));
          
                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");

                    if (chkPrint.Checked == true)
                    {
                        sb.Append("&g=" + groupID.Text);
                        sb.Append("&e=" + email.Text);
                        sb.Append("&f=html");
                        sb.Append("&c=" + custID.Text);
                        sb.Append("&user_PaymentStatus=paid");

                        //post to transactional UDF

                        if (user_CardHolderName.Text.Trim().Contains(" "))
                        {
                            sb.Append("&user_t_FirstName=" + user_CardHolderName.Text.Split(new char[] { ' ' })[0]);
                            sb.Append("&user_t_LastName=" + user_CardHolderName.Text.Split(new char[] { ' ' })[1]);
                        }
                        else
                        {
                            sb.Append("&user_t_FirstName=" + user_CardHolderName.Text);
                            sb.Append("&user_t_LastName=" + "");
                        }

                        sb.Append("&user_t_CardType=" + user_CCType.SelectedItem.Value);
                        //sb.Append("&user_t_CardNumber=" + user_CCNumber.Text);
                        
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

                        sb.Append("&user_t_Zip=" + zip.Text);
                        sb.Append("&user_t_Country=" + countryCode);
                        sb.Append("&user_t_TransactionID=" + TransactionID);

                        redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text, sb.ToString(), browserInfo);  

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
                        Response.Redirect("Forms/watt_thankyou.htm");
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
                phError.Visible = true;
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

                if (WebConfigurationManager.AppSettings["PayEnvironment"].ToString().Equals("sandbox", StringComparison.OrdinalIgnoreCase))
                    ppPay.Environment = NvpEnvironment.Sandbox;
                else if (WebConfigurationManager.AppSettings["PayEnvironment"].ToString().Equals("live", StringComparison.OrdinalIgnoreCase))
                    ppPay.Environment = NvpEnvironment.Live;
                else if (WebConfigurationManager.AppSettings["PayEnvironment"].ToString().Equals("betasandbox", StringComparison.OrdinalIgnoreCase))
                    ppPay.Environment = NvpEnvironment.BetaSandbox;

                ppPay.Credentials.Username = WebConfigurationManager.AppSettings["PayflowUserName"].ToString();
                ppPay.Credentials.Password = WebConfigurationManager.AppSettings["PayflowPassword"].ToString();
                ppPay.Credentials.Signature = WebConfigurationManager.AppSettings["PayflowSignature"].ToString();

                ppPay.Add(NvpDoDirectPayment.Request._IPADDRESS, "127.0.0.1");
                ppPay.Add(NvpDoDirectPayment.Request._PAYMENTACTION, NvpPaymentActionCodeType.Sale);
                ppPay.Add(NvpDoDirectPayment.Request._AMT, String.Format("{0:0.00}", GetTotal()));
                ppPay.Add(NvpDoDirectPayment.Request.ITEMAMT, String.Format("{0:0.00}", GetTotal()));

                if (user_CCType.SelectedItem.Value.Equals("MasterCard", StringComparison.OrdinalIgnoreCase))  
                    ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.MasterCard);
                else if (user_CCType.Text.Equals("Visa", StringComparison.OrdinalIgnoreCase))
                    ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Visa);
                else if (user_CCType.Text.Equals("Amex", StringComparison.OrdinalIgnoreCase))
                    ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Amex);

                ppPay.Add(NvpDoDirectPayment.Request._ACCT, user_CCNumber.Text);
                ppPay.Add(NvpDoDirectPayment.Request._EXPDATE, user_Exp_Month.SelectedItem.Value + user_Exp_Year.SelectedItem.Text);

                ppPay.Add(NvpDoDirectPayment.Request.CVV2, user_CCVerfication.Text);

                if (user_CardHolderName.Text.Trim().Contains(" "))
                {
                    ppPay.Add(NvpDoDirectPayment.Request._FIRSTNAME, user_CardHolderName.Text.Split(new char[] { ' ' })[0]);
                    ppPay.Add(NvpDoDirectPayment.Request._LASTNAME, user_CardHolderName.Text.Split(new char[] { ' ' })[1]);
                }
                else
                {
                    ppPay.Add(NvpDoDirectPayment.Request._FIRSTNAME, user_CardHolderName.Text);
                    ppPay.Add(NvpDoDirectPayment.Request._LASTNAME, "");
                }

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")     
                {
                    ppPay.Add(NvpDoDirectPayment.Request.STATE, state.SelectedItem.Value);
                }
                else
                {
                    ppPay.Add(NvpDoDirectPayment.Request.STATE, city.Text);
                }

                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTREET, address1.Text);
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTREET2, address2.Text);
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOCITY, city.Text);
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOZIP, zip.Text);
                string countrCode = string.Empty; 

                if((countryCode.ToUpper() == "UNITED STATES OF AMERICA") || (countryCode.ToUpper() == "UNITED STATES"))
                {
                    countrCode = "US"; 
                }
                else if(countryCode.ToUpper() == "CANADA")     
                {   
                   countrCode = "CA";   
                }
                else
                {
                    countrCode = PubCountry.GetCountryByCountryID(Convert.ToInt32(countryName.SelectedItem.Value)).CountryCode;
                }

                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOCOUNTRYCODE, countrCode);   
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOPHONENUM, phone.Text);

                if (countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA")     
                {
                    ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTATE, state.SelectedItem.Value); 
                }
                else
                {
                    ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTATE, city.Text);
                }

                ppPay.Add(NvpDoDirectPayment.Request.SHIPTONAME, user_CardHolderName.Text.ToString());
                ppPay.Add(NvpDoDirectPayment.Request.PHONENUM, phone.Text);
                ppPay.Add(NvpDoDirectPayment.Request.STREET, address1.Text);
                ppPay.Add(NvpDoDirectPayment.Request.STREET2, address2.Text);
                ppPay.Add(NvpDoDirectPayment.Request.CITY, city.Text);
                ppPay.Add(NvpDoDirectPayment.Request.ZIP, zip.Text);
                ppPay.Add(NvpDoDirectPayment.Request.COUNTRYCODE, countrCode);      

                ppPay.Add(NvpDoDirectPayment.Request.EMAIL, email.Text);

                string SubscriberID = "";

                if (itemList.Count > 0)
                {
                    try
                    {
                        SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, email.Text);  
                    }
                    catch
                    {
                        SubscriberID = "0";   
                    }
                    
                    ppPay.Add(NvpDoDirectPayment.Request.CUSTOM, SubscriberID);                                    
                }

                for (int i = 0; i < itemList.Count; i++)
                {
                    ppPay.Add(NvpDoDirectPayment.Request.L_NAMEn + i.ToString(), itemList[i].ItemName);   
                    ppPay.Add(NvpDoDirectPayment.Request.L_AMTn + i.ToString(), itemList[i].ItemAmount);
                    ppPay.Add(NvpDoDirectPayment.Request.L_QTYn + i.ToString(), itemList[i].ItemQty);
                }

                return ppPay.Post();
            }
            catch (Exception ex)
            {
                string emailMsg = "Error when Processing Credit Card..<br /><br />";
                emailMsg += email.Text;                                                                    
                emailMsg += " Group ID:" + itemList[0].GroupID + "Cust ID:" + itemList[0].CustID;            
                emailMsg += "<b>Error Response:</b>" + ppPay.ResponseString + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;
                Utilities.SendMail(emailMsg);
                return false;
            }
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
    }
}