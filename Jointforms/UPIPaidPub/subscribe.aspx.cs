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
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using upipaidpub.Objects;
using KMPS_JF_Objects.Objects;
using AuthorizeNet.APICore;
using AuthorizeNet.Helpers;
using AuthorizeNet;
using System.Text.RegularExpressions;
using System.IO;
using ecn.communicator.classes;
using AjaxControlToolkit;
using System.Configuration;
using KM.Common;

namespace upipaidpub
{
    public partial class subscribe : System.Web.UI.Page
    {
        private decimal unitPrice = 0.0M;
        private decimal totalPrice = 0.0M;
        private decimal discount = 0.0M;
        private int quantity = 0;
        private string countryCode = String.Empty;
        private string PubCodeList = string.Empty;
        private AuthorizationRequest ppPay = new AuthorizationRequest("", "", 0.00M, "");
        private List<upipaidpub.Objects.Item> itemList = new List<upipaidpub.Objects.Item>();
        private GatewayResponse response = null;
        DataTable dtMagList = new DataTable();
        Publication pub = null;

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
            if (PubCode.ToUpper() == "HMETK")
            {
                drpBillingCountry.Items.Clear();
                drpBillingCountry.Items.Insert(0, new ListItem("United States", "205"));
                drpBillingCountry.Items.Insert(1, new ListItem("Canada", "174"));

                drpShippingCountry.Items.Clear();
                drpShippingCountry.Items.Insert(0, new ListItem("United States", "205"));
                drpShippingCountry.Items.Insert(1, new ListItem("Canada", "174"));
            }
            else
            {
                string sql = "SELECT CountryID, CountryName from Country WHERE CountryID NOT IN (174,205) ORDER BY CountryName ASC";
                DataTable dt = upipaidpub.Objects.DataFunctions.GetDataTable(sql);

                drpBillingCountry.DataSource = dt;
                drpBillingCountry.DataTextField = "CountryName";
                drpBillingCountry.DataValueField = "CountryID";
                drpBillingCountry.DataBind();
                drpBillingCountry.Items.Insert(0, new ListItem("United States", "205"));
                drpBillingCountry.Items.Insert(1, new ListItem("Canada", "174"));

                drpShippingCountry.DataSource = dt;
                drpShippingCountry.DataTextField = "CountryName";
                drpShippingCountry.DataValueField = "CountryID";
                drpShippingCountry.DataBind();
                drpShippingCountry.Items.Insert(0, new ListItem("United States", "205"));
                drpShippingCountry.Items.Insert(1, new ListItem("Canada", "174"));
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Server.MapPath("App_Themes/" + PubCode.ToUpper())))
                {
                    Page.Theme = PubCode;
                }
                else
                {
                    Page.Theme = "Default";
                }
            }
            catch  
            {
                Page.Theme = "Default";
            }
        }

        protected void Page_Load(object sender, EventArgs e)       
        {
            //Uncomment when ready -- Justin Welter 08232016
            //ClientScriptManager csm = Page.ClientScript;
            //btnSecurePayment.Attributes.Add("onclick", "javascript:" + btnSecurePayment.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSecurePayment, ""));
            if (!IsPostBack)
            {
                string path = Request.Url.ToString();

                if (path.StartsWith("http:") && !path.ToLower().Contains("localhost") )
                {
                    path = path.Replace("http:", "https:");
                    Response.Redirect(path);
                }
            }

            if ((PubCode != null && PubCode.Trim().Length == 0) || !WebConfigurationManager.AppSettings["PubCodes"].ToString().Contains(PubCode.ToUpper()))
            {
                phErrorTop.Visible = true;
                lblErrorMessageTop.Text = "Invalid Publication!!";
                container.Visible = false;
                return;
            }

            try
            {
                if (IsCacheClear())
                {
                    if (CacheUtil.IsCacheEnabled())
                    {
                        if (CacheUtil.GetFromCache("Pub_" + PubCode.ToUpper(), "JOINTFORMS") != null)
                        {
                            CacheUtil.RemoveFromCache("Pub_" + PubCode.ToUpper(), "JOINTFORMS");
                        }
                    }

                    pub = Publication.GetPublicationbyID(0, PubCode);
                    UpdateCacheClear();
                }
                else
                    pub = Publication.GetPublicationbyID(0, PubCode);
            }
            catch
            { }

            dtMagList = new DataTable();

            if (PubCode != null && PubCode.Trim().Length > 0)
                dtMagList = GetMagList(PubCode);

            try
            {
                Page.Title = dtMagList.Rows[0]["PageTitle"].ToString();
            }
            catch
            {
                Page.Title = PubCode + " Registration";
            }

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
                SetCountryCode();
            }

            try
            {
                PubCode = Server.HtmlEncode(Request.QueryString["pubcode"].ToString());
            }
            catch { }

            if (!IsPostBack)
            {
                LoadBillingState(countryCode);
                LoadShippingState(countryCode);

                if (pnlBillingState.Visible && Request.QueryString["state"] != null)
                {
                    drpBillingState.SelectedIndex = drpBillingState.Items.IndexOf(drpBillingState.Items.FindByValue(Request.QueryString["state"].ToString().ToUpper()));
                    drpBillingState.Items[drpBillingState.SelectedIndex].Selected = true;
                }

                BindGridMagazines();

                if (GetTotal() == 0.00M)
                {
                    if (Request.QueryString != null)
                    {
                        HttpBrowserCapabilities browser = Request.Browser;
                        string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                        ECNUtils.ECNHttpPost(email.Text, PubCode, Request.QueryString.ToString(), browserInfo);
                        HttpPostAutoSubscription(Request.QueryString.ToString());
                    }

                    SendCompResponseEmail();
                    Response.Redirect(WebConfigurationManager.AppSettings["ThankyoupageLink"].ToString() + "?pubcode=" + PubCode, true);
                }

                LoadFields();

                pnlContactInfo.Visible = Convert.ToBoolean(dtMagList.Rows[0]["ShowContact"].ToString());
                pnlBillingAddress.Visible = Convert.ToBoolean(dtMagList.Rows[0]["ShowBillingAddress"].ToString());
                pnlShippingAddress.Visible = Convert.ToBoolean(dtMagList.Rows[0]["ShowShippingAddress"].ToString());
                pnlBillingToShipping.Visible = pnlBillingAddress.Visible && pnlShippingAddress.Visible;

                pnlBillingState.Visible = countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA";
                pnlShippingState.Visible = countryCode.ToUpper() == "UNITED STATES OF AMERICA" || countryCode.ToUpper() == "UNITED STATES" || countryCode.ToUpper() == "CANADA";
                pnlBillingStateInt.Visible = !pnlBillingState.Visible;
                pnlShippingStateInt.Visible = !pnlShippingState.Visible;
            }

            if (PubCode != null && PubCode.Trim().Length > 0)
                phHeader.Controls.Add(new LiteralControl(dtMagList.Rows[0]["header"].ToString()));

            if (PubCode != null && PubCode.Trim().Length > 0)
                footer.Controls.Add(new LiteralControl(dtMagList.Rows[0]["footer"].ToString()));
        }

        private void HttpPostAutoSubscription(string queryString)
        {
            HttpBrowserCapabilities browser = Request.Browser;
            string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

            string[] autoSubsPost = queryString.Split('&');
            Dictionary<string, string> postFields = new Dictionary<string, string>();

            foreach (string st in autoSubsPost)
            {
                string key = st.Split('=')[0];
                string value = st.Split('=')[1];

                if (!postFields.ContainsKey(key))
                {
                    postFields.Add(key, value);
                }
            }

            Dictionary<int, int> autoSubGroups = GetAutoSubscriptions(pub.PubID);
            foreach (KeyValuePair<int, int> kvp in autoSubGroups)
            {
                try
                {
                    postFields["g"] = kvp.Key.ToString();
                    postFields["c"] = kvp.Value.ToString();
                }
                catch { }

                ECNUtils.ECNHttpPost(email.Text, pub.PubCode, GetQueryStringFromDictionary(postFields), browserInfo);
            }
        }

        private string GetQueryStringFromDictionary(Dictionary<string, string> postFields)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in postFields)
                sb.Append(kvp.Key + "=" + kvp.Value + "&");

            return sb.ToString().TrimEnd('&');
        }

        private Dictionary<int, int> GetAutoSubscriptions(int pubID)
        {
            Dictionary<int, int> dictAutoSubs = new Dictionary<int, int>();
            SqlCommand cmdAutoSubs = new SqlCommand("spGetPubAutoSubscription");
            cmdAutoSubs.CommandType = CommandType.StoredProcedure;
            cmdAutoSubs.Parameters.Add(new SqlParameter("@pubID", pubID.ToString()));
            DataTable dtAutoSubs = upipaidpub.Objects.DataFunctions.GetDataTable(cmdAutoSubs);

            foreach (DataRow dr in dtAutoSubs.Rows)
                dictAutoSubs.Add(Convert.ToInt32(dr["PubAutoGroupID"]), Convert.ToInt32(dr["PubAutoCustID"]));

            return dictAutoSubs;
        }

        private bool CheckPaidProductsEmailValidation(string emailAddress)
        {
            if (pub.RepeatEmails)
            {
                string sql = "SELECT e.EmailID FROM Emails e join EmailGroups eg on eg.EmailID = e.EmailID WHERE e.EmailAddress = @EmailAddress " +
                             " AND e.CustomerID = @CustomerID and eg.GroupID = @GroupID";

                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                cmd.Parameters.AddWithValue("@CustomerID", pub.ECNCustomerID.ToString());
                cmd.Parameters.AddWithValue("@GroupID", pub.ECNDefaultGroupID.ToString());
                DataTable dt = upipaidpub.Objects.DataFunctions.GetDataTable("communicator", cmd);

                if (dt.Rows.Count > 0)
                    return true;
            }

            return false;
        }

        protected void email_TextChanged(object sender, EventArgs e)
        {
            if (CheckPaidProductsEmailValidation(email.Text.Trim()))
            {
                lblEmailValidationText.Text = pub.RepeatEmailsMessage.Replace("%%EmailAddress%%", email.Text.Trim());
                ModalPopupExtenderEmailValidation.Show();
                pnlEmailValidationPopup.Style.Add("display", "block");
                email.Text = "";
            }
        }

        private void LoadFields()
        {
            first.Text = FirstName;
            last.Text = LastName;
            phone.Text = Phone;
            fax.Text = Fax;
            email.Text = Email;
            email.ReadOnly = Email.Trim().Length > 0 ? true : false;

            txtCompany.Text = Company;
            txtBillingAddress1.Text = Address1;
            txtBillingAddress2.Text = Address2;
            txtBillingCity.Text = City;
            txtBillingZip.Text = Zip;

            txtShippingAddress1.Text = Address1;
            txtShippingAddress2.Text = Address2;
            txtShippingCity.Text = City;
            txtShippingZip.Text = Zip;

            string country = Country;

            try
            {
                drpBillingCountry.ClearSelection();
                foreach (ListItem li in drpBillingCountry.Items)
                {
                    if (li.Text.ToUpper() == country.ToUpper())
                    {
                        li.Selected = true;
                    }
                }
            }
            catch { }

            try
            {
                drpShippingCountry.ClearSelection();
                foreach (ListItem li in drpShippingCountry.Items)
                {
                    if (li.Text.ToUpper() == country.ToUpper())
                    {
                        li.Selected = true;
                    }
                }
            }
            catch { }

            try
            {
                drpBillingState.ClearSelection();
                drpBillingState.Items.FindByValue(Request.QueryString["state"].ToString()).Selected = true;

                drpShippingState.ClearSelection();
                drpShippingState.Items.FindByValue(Request.QueryString["state"].ToString()).Selected = true;
            }
            catch { }

            if (pnlBillingStateInt.Visible)
            {
                try
                {
                    txtBillingStateInt.Text = Request.QueryString["user_STATE_INT"].ToString();
                }
                catch { }

                try
                {
                    txtBillingZip.Text = Request.QueryString["user_FORZIP"].ToString();
                }
                catch { }
            }
        }

        private void BindGridMagazines()
        {
            grdMagazines.DataSource = GetMagList(PubCode);
            grdMagazines.DataBind();
        }

        private void UpdateGridView()
        {
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label groupID = (Label)row.FindControl("lblGroupID");
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
                    countryCode = "UNITED STATES OF AMERICA";
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
                    newMagList.ImportRow(dr);

                return newMagList;
            }
        }

        private DataTable GetMagList(string pubCode)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("xml/MagList.xml"));
            DataTable magList = ds.Tables[0];

            if (String.IsNullOrEmpty(pubCode))
            {
                return magList;
            }
            else
            {
                DataTable newMagList = magList.Clone();
                string expression = "pubcode = '" + pubCode + "'";
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
            string sql = "select pf.PFID from PubFormsForCountry pfc join PubForms pf on pf.PFID = pfc.PFID join Country c on c.CountryID = pfc.CountryID where c.CountryName = '" + CountryName + "' and pf.PubID = " + pub.PubID.ToString() + " and pf.ShowPrint = 0";
            DataTable dt = KMPS_JF_Objects.Objects.DataFunctions.GetDataTable(sql);

            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public void CalculatePrice(GridViewRow row)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                Label oneYearPrice = (Label)row.FindControl("lblOneYearPrice");
                Label twoYearPrice = (Label)row.FindControl("lblTwoYearPrice");
                Label threeYearPrice = (Label)row.FindControl("lblThreeYearPrice");
                Label fourYearPrice = (Label)row.FindControl("lblFourYearPrice");
                Label fiveYearPrice = (Label)row.FindControl("lblFiveYearPrice");
                DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");
                Label lblQuantity = (Label)row.FindControl("lblQuantity");

                if (lblQuantity != null && lblQuantity.Visible)
                    quantity = 1;
                else
                    quantity = Convert.ToInt32(ddlQuantity.SelectedItem.Value);


                if (chkPrint.Checked)
                {
                    if (oneYearPrice != null && oneYearPrice.Visible)
                        unitPrice = decimal.Parse(oneYearPrice.Text);
                    else if (twoYearPrice != null && twoYearPrice.Visible)
                        unitPrice = decimal.Parse(twoYearPrice.Text);
                    else if (threeYearPrice != null && threeYearPrice.Visible)
                        unitPrice = decimal.Parse(threeYearPrice.Text);
                    else if (fourYearPrice != null && fourYearPrice.Visible)
                        unitPrice = decimal.Parse(fourYearPrice.Text);
                    else if (fiveYearPrice != null && fiveYearPrice.Visible)
                        unitPrice = decimal.Parse(fiveYearPrice.Text);
                }
                else
                {
                    unitPrice = 0.00M;
                }

                if (oneYearPrice != null && oneYearPrice.Visible)
                    oneYearPrice.Text = unitPrice.ToString();
                else if (twoYearPrice != null && twoYearPrice.Visible)
                    twoYearPrice.Text = unitPrice.ToString();
                else if (threeYearPrice != null && threeYearPrice.Visible)
                    threeYearPrice.Text = unitPrice.ToString();
                else if (fourYearPrice != null && fourYearPrice.Visible)
                    fourYearPrice.Text = unitPrice.ToString();
                else if (fiveYearPrice != null && fiveYearPrice.Visible)
                    fiveYearPrice.Text = unitPrice.ToString();

                totalPrice = unitPrice;
            }
            else if (row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalAmount = (Label)row.FindControl("lblTotalAmount");
                Label lblDiscount = (Label)row.FindControl("lblDiscountPromocodePrice");
                Label lblDiscountLabel = (Label)row.FindControl("lblDiscountPromocode");
                Discount discountObj = new Discount();
                discountObj = CalculatePromoCodeDiscount(totalPrice, quantity);
                discount = discountObj.DiscountAmount;

                totalPrice = totalPrice - discount;
                if (lblDiscount != null && discount > 0.0M)
                {
                    lblDiscount.Visible = true;
                    lblDiscountLabel.Visible = true;

                    lblDiscount.Text = discountObj.DiscountAmount.ToString("0.00");
                }

                if (lblTotalAmount != null)
                    lblTotalAmount.Text = String.Format("{0:0.00}", totalPrice.ToString());
            }
        }

        public bool IsMultiCompPromocodeExists(bool isDiscountForEveryPromoCode)
        {
            SqlCommand cmdMultiCompDisc = new SqlCommand("spGetWebSiteAndPromocodeByGroupID");
            cmdMultiCompDisc.CommandType = CommandType.StoredProcedure;
            cmdMultiCompDisc.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = pub.ECNDefaultGroupID;
            cmdMultiCompDisc.Parameters.Add(new SqlParameter("@Website", SqlDbType.VarChar)).Value = WebSite;
            cmdMultiCompDisc.Parameters.Add(new SqlParameter("@PromoCode", SqlDbType.VarChar)).Value = PromoCode;
            string[] strAutomatedPubs = WebConfigurationManager.AppSettings["AutomatedDiscountPubs"].ToString().Split(',');

            cmdMultiCompDisc.Parameters.Add(new SqlParameter("@DiscountForEveryPromocode", SqlDbType.Bit)).Value = isDiscountForEveryPromoCode;
            DataTable dtMultiComp = upipaidpub.Objects.DataFunctions.GetDataTable("communicator", cmdMultiCompDisc);
            return dtMultiComp.Rows.Count > 0 && dtMultiComp.Rows[0]["Website"].ToString().Length > 0;
        }

        public Discount CalculatePromoCodeDiscount(decimal price, int year)
        {
            XElement doc = XElement.Load(Server.MapPath("xml/Promocodes.xml"));
            var pcode = from p in doc.Descendants("publication")
                        where p.Attribute("pubcode").Value.ToUpper() == PubCode.ToUpper()
                        from pc in p.Descendants("promocode")
                        where pc.Attribute("ID").Value.ToUpper() == PromoCode.ToUpper() && pc.Attribute("year").Value == year.ToString()
                        select new { pub = p, disc = pc };

            bool IsMultiCompPromocodeExist = false;

            Discount discountObj = new Discount();
            foreach (var code in pcode)
            {
                discountObj.DiscountType = (Enums.DiscountType)Enum.Parse(typeof(Enums.DiscountType), code.disc.Attribute("DiscountType").Value.ToUpper());
                discountObj.DiscountPercentage = Convert.ToDecimal(code.disc.Attribute("DiscountPercent").Value);
                discountObj.DiscountAmount = Convert.ToDecimal(code.disc.Attribute("DiscountAmount").Value);
                discountObj.OriginalAmount = Convert.ToDecimal(code.pub.Attribute("OriginalAmount").Value);
                discountObj.Year = Convert.ToInt32(code.disc.Attribute("year").Value);
                discountObj.ID = code.disc.Attribute("ID").Value;
                discountObj.IsComp = Convert.ToBoolean(Convert.ToInt32(code.disc.Attribute("IsComp").Value));
                discountObj.ExpirationDate = Convert.ToDateTime(code.disc.Attribute("Expirationdate").Value);
                discountObj.IsMultiComp = Convert.ToBoolean(Convert.ToInt32(code.disc.Attribute("IsMultiComp").Value));
                discountObj.DiscountForEveryPromoCode = Convert.ToBoolean(Convert.ToInt32(code.pub.Attribute("DiscountForEveryPromoCode").Value));
                discountObj.EveryPromocodeDiscAmount = Convert.ToDecimal(code.pub.Attribute("EveryPromoCodeDiscount").Value);
                discountObj.MultiItemDiscount = Convert.ToBoolean(Convert.ToInt32(code.pub.Attribute("MultiItemDiscount").Value));

                IsMultiCompPromocodeExist = IsMultiCompPromocodeExists(discountObj.DiscountForEveryPromoCode);

                if (discountObj.DiscountForEveryPromoCode && IsMultiCompPromocodeExist && (discountObj.OriginalAmount - discountObj.DiscountAmount) <= discountObj.EveryPromocodeDiscAmount)
                    discountObj.DiscountAmount = Convert.ToDecimal(code.pub.Attribute("EveryPromoCodeDiscount").Value);
                else if (discountObj.DiscountForEveryPromoCode && !IsMultiCompPromocodeExist && discountObj.DiscountType == Enums.DiscountType.PERCENTAGE)
                    discountObj.DiscountAmount = (price * discountObj.DiscountPercentage) / 100.0M;
                else if (discountObj.DiscountForEveryPromoCode && !IsMultiCompPromocodeExist && discountObj.DiscountType == Enums.DiscountType.FLATAMOUNT)
                    discountObj.DiscountAmount = price - discountObj.DiscountAmount;
                else if (discountObj.DiscountType == Enums.DiscountType.PERCENTAGE)
                    discountObj.DiscountAmount = (price * discountObj.DiscountPercentage) / 100.0M;
                else if (discountObj.IsComp)
                    discountObj.DiscountAmount = (price * 100.0M) / discountObj.DiscountPercentage;
                else if (discountObj.IsMultiComp && !IsMultiCompPromocodeExist)
                    discountObj.DiscountAmount = price;
                else
                    discountObj.DiscountAmount = price - discountObj.DiscountAmount;
            }

            return discountObj;
        }

        public void chBoxPrint_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CalculatePrice(row);
                    return;
                }
            }
        }

        public decimal GetTotal()
        {
            Label lblTotAmount = (Label)grdMagazines.FooterRow.FindControl("lblTotalAmount");
            return decimal.Parse(lblTotAmount.Text.Trim('$'));
        }

        #region State Dropdown

        private void addlistitemBillingState(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            drpBillingState.Items.Add(item);
        }

        private void addlistitemShippingState(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            drpShippingState.Items.Add(item);
        }

        #endregion

        private DateTime CheckExpirationDate(int CustomerID, string EmailAddress, int groupID)
        {
            string shortname = "EXPDATE"; 
            string sqlcheck = "SELECT COUNT(groupdatafieldsID) FROM GROUPDATAFIELDS WHERE shortname='" + shortname + "'  and IsDeleted=0 AND groupID =" + groupID ;
            int alreadyexist = Convert.ToInt32(upipaidpub.Objects.DataFunctions.ExecuteScalar("communicator", sqlcheck));

            if (alreadyexist == 0)
            {
                string sqlquery = " INSERT INTO GroupDataFields ( GroupID, ShortName, LongName, IsPublic, IsDeleted) VALUES ( " + groupID + ", '" + shortname + "', '" + shortname + "','N', 0);select @@IDENTITY ";
                try
                {
                    upipaidpub.Objects.DataFunctions.ExecuteScalar("communicator", sqlquery).ToString();
                }
                catch { }
                return DateTime.MinValue;
            }
            else
            {
                string sql = " select edv.DataValue from Emails e join EmailDataValues edv on e.EmailID = edv.EmailID " +
                             " join GroupDatafields gdf on gdf.GroupDatafieldsID = edv.GroupDatafieldsID " +
                             " where e.CustomerID = " + CustomerID + " and EmailAddress = '" + EmailAddress + "' and gdf.ShortName = '"+ shortname +"' and gdf.GroupID=" + groupID.ToString();
                DateTime expDate;
                try
                {
                    expDate = Convert.ToDateTime(upipaidpub.Objects.DataFunctions.ExecuteScalar("communicator", sql).ToString());
                }
                catch { expDate = DateTime.MinValue; }
                return expDate;
            }
        }

        protected void btnSecurePayment_Click(object sender, EventArgs e)
        {
            if (pub.RepeatEmails)
                email.Text = Email;

            if (CheckPaidProductsEmailValidation(email.Text))
            {
                lblEmailValidationText.Text = pub.RepeatEmailsMessage.Replace("%%EmailAddress%%", email.Text.Trim());
                ModalPopupExtenderEmailValidation.Show();
                pnlEmailValidationPopup.Style.Add("display", "block");
                email.Text = "";
                return;
            }

            bool redirectstatus = false;
            bool redirectstatusall = true;
            StringBuilder sb = new StringBuilder();
            int selectioncount = 0;
            String SubscriberID = string.Empty;

            

            if (Page.IsValid)
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                string EncodedResponse = Request.Form["g-Recaptcha-Response"];

                if (ConfigurationManager.AppSettings["ValdiateCaptcha"].ToString().ToLower().Equals("true"))
                {
                    if (!ReCaptchaClass.Validate(EncodedResponse))
                    {
                        phError.Visible = true;
                        lblErrorMessage.Text = "Invalid Captcha";
                        return;
                    }
                }

                GridViewRow row =grdMagazines.Rows[0];
                upipaidpub.Objects.Item item = new upipaidpub.Objects.Item();
                Label groupID = (Label)row.FindControl("lblGroupID");
                Label custID = (Label)row.FindControl("lblcustID");
                Label lblPubCode = (Label)row.FindControl("lblPubCode");
                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                Label lblQuantity = (Label)row.FindControl("lblQuantity");
                DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");
                HiddenField hdDesc = (HiddenField)row.FindControl("hdDescription");
                HiddenField hdTitle = (HiddenField)row.FindControl("hdTitle");
                decimal totalAmt = GetTotal();

                DateTime? finalExpDate = null;
                DateTime expDate = CheckExpirationDate(Convert.ToInt32(custID.Text), Email, Convert.ToInt32(groupID.Text));
                int years = 1;
                if (!lblQuantity.Visible)
                {
                    years = Convert.ToInt32(ddlQuantity.SelectedItem.Value);
                }

                if (expDate == DateTime.MinValue || expDate <= DateTime.Now)
                {
                    finalExpDate=DateTime.Now.AddYears(years);
                }
                else
                {
                    finalExpDate = expDate.AddYears(years);
                }
                sb.Append("&user_expdate=" + finalExpDate.Value.ToString("M/dd/yyyy") + "&");

                if (chkPrint.Checked == true)
                {
                    selectioncount++;

                    if (chkPrint.Checked) { sb.Append("user_DEMO7=A"); }

                    sb.Append("&g=" + groupID.Text);
                    sb.Append("&c=" + custID.Text);
                    sb.Append("&f=html");
                    sb.Append("&user_DEMO39=" + PromoCode);
                    sb.Append("&user_PUBLICATIONCODE=" + lblPubCode.Text.ToString());
                    sb.Append("&website=" + WebSite);
                    sb.Append("&user_PaymentStatus=pending");

                    if (pnlContactInfo.Visible)
                    {
                        sb.Append("&fn=" + first.Text);
                        sb.Append("&ln=" + last.Text);
                        sb.Append("&ph=" + phone.Text);
                        sb.Append("&fax=" + fax.Text);
                        sb.Append("&e=" + email.Text);
                    }
                    else
                    {
                        sb.Append("&fn=" + FirstName);
                        sb.Append("&ln=" + LastName);
                        sb.Append("&ph=" + Phone);
                        sb.Append("&fax=" + Fax);
                        sb.Append("&occupation=" + Occupation);
                        sb.Append("&title=" + TitleQS);
                    }

                    if (pnlShippingAddress.Visible)
                    {
                        sb.Append("&adr=" + txtShippingAddress1.Text);
                        sb.Append("&adr2=" + txtShippingAddress2.Text);
                        sb.Append("&city=" + txtShippingCity.Text);

                        if (countryCode == "UNITED STATES OF AMERICA" || countryCode == "UNITED STATES" || countryCode == "CANADA")
                            sb.Append("&state=" + drpShippingState.Text);

                        sb.Append("&zc=" + txtShippingZip.Text);
                        sb.Append("&ctry=" + drpShippingCountry.SelectedItem.Text);
                    }
                    else
                    {
                        sb.Append("&adr=" + Address1);
                        sb.Append("&adr2=" + Address2);
                        sb.Append("&city=" + City);

                        if (countryCode == "UNITED STATES OF AMERICA" || countryCode == "CANADA")
                            sb.Append("&state=" + State);

                        sb.Append("&zc=" + Zip);
                        sb.Append("&ctry=" + Country);
                    }

                    item.ItemCode = lblPubCode.Text;
                    item.ItemName = hdTitle.Value;
                    item.ItemAmount = totalAmt.ToString();

                    if (lblQuantity != null && lblQuantity.Visible)
                        item.ItemQty = lblQuantity.Text;
                    else if (ddlQuantity != null)
                        item.ItemQty = ddlQuantity.SelectedItem.Value;

                    item.GroupID = int.Parse(groupID.Text);
                    item.CustID = int.Parse(custID.Text);
                    item.Description = hdDesc.Value;
                    itemList.Add(item);                    
                }

                if (selectioncount == 0)
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Please select the magazine you want to subscribe.";
                    return;
                }

                #region VALIDATE CREDIT CARD

                if (redirectstatusall)
                {
                    if (String.IsNullOrEmpty(TransactionID))
                    {
                        try
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
                                TransactionID = response.TransactionID;
                        }
                        catch 
                        {
                            phError.Visible = true;
                            lblErrorMessage.Text = "Error Processing Credit Card.";
                            return;
                        }
                    }
                }

                #endregion

                #region COLLECT Transactional UDF

                Label usLabel = (Label)row.FindControl("lblUsPrice");
                Label canLabel = (Label)row.FindControl("lblCanPrice");
                Label intLabel = (Label)row.FindControl("lblIntPrice");
                Label lblTotal = (Label)row.FindControl("lblTotal");
                Label lblDiscount = (Label)grdMagazines.FooterRow.FindControl("lblDiscountPromocodePrice");
                totalAmt = GetTotal();

                if (chkPrint.Checked == true)
                {
                    sb.Append("&user_PaymentStatus=paid");

                    if (pnlContactInfo.Visible)
                        sb.Append("&e=" + email.Text.Trim());
                    else
                        sb.Append("&e=" + Email);

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

                    sb.Append("&user_t_FullName=" + user_CardHolderName.Text);
                    sb.Append("&user_t_CardType=" + user_CCType.SelectedItem.Value);
                    sb.Append("&user_t_CardNumber=" + "************" + user_CCNumber.Text.Substring(user_CCNumber.Text.Trim().Length - 4, 4)); //pass only last four digit to ECN.
                    sb.Append(String.Format("&user_t_ExpirationDate={0}/{1}", user_Exp_Month.SelectedItem.Value, user_Exp_Year.SelectedItem.Value));
                    sb.Append("&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy"));
                    sb.Append("&user_t_AmountPaid=" + String.Format("{0:0.00}", totalAmt));
                    sb.Append("&user_t_CardBillZip=" + txtZipCardBill.Text);
                    sb.Append("&user_t_TransactionID=" + TransactionID.ToString());

                    if (pnlBillingAddress.Visible)
                    {
                        sb.Append("&user_t_Street=" + txtBillingAddress1.Text);
                        sb.Append("&user_t_Street2=" + txtBillingAddress2.Text);
                        sb.Append("&user_t_City=" + txtBillingCity.Text);
                        sb.Append("&user_t_Zip=" + txtBillingZip.Text);
                        sb.Append("&user_t_Country=" + drpBillingCountry.SelectedItem.Text);
                    }
                    else
                    {
                        sb.Append("&user_t_Street=" + Address1);
                        sb.Append("&user_t_Street2=" + Address2);
                        sb.Append("&user_t_City=" + City);
                        sb.Append("&user_t_Country=" + Country);
                    }

                    if (Zip.Trim().Length > 0 && !pnlBillingAddress.Visible)
                        sb.Append("&user_t_Zip=" + Zip);
                    else if (!pnlBillingAddress.Visible)
                        sb.Append("&user_t_Zip=" + ForZip);

                    if (pnlBillingState.Visible)
                        sb.Append("&user_t_State=" + drpBillingState.SelectedItem.Value);
                    else if (pnlBillingStateInt.Visible)
                        sb.Append("&user_t_State=" + txtBillingStateInt.Text);
                    else if (State.Trim().Length > 0)
                        sb.Append("&user_t_State=" + State);
                    else
                        sb.Append("&user_t_State=" + StateInt.Trim(','));


                    //post shipping details
                    if (pnlShippingAddress.Visible)
                    {
                        sb.Append("&user_SHIPTO_ADDRESS1=" + txtShippingAddress1.Text);
                        sb.Append("&user_SHIPTO_ADDRESS2=" + txtShippingAddress2.Text);
                        sb.Append("&user_SHIPTO_CITY=" + txtShippingCity.Text);

                        if (pnlShippingState.Visible)
                        {
                            sb.Append("&user_SHIPTO_STATE=" + drpShippingState.SelectedItem.Value);
                            sb.Append("&user_SHIPTO_ZIP=" + txtShippingZip.Text);
                        }
                        else
                        {
                            sb.Append("&user_SHIPTO_STATE_INT=" + txtShippingStateInt.Text);
                            sb.Append("&user_SHIPTO_FORZIP=" + txtShippingZip.Text);
                        }
                    }

                    if (lblDiscount != null)
                        sb.Append("&user_promoCodeDiscount=" + lblDiscount.Text.TrimStart('$').TrimStart('-'));
                }
                #endregion

                #region POST TO ECN

                Dictionary<int, int> autoPubs = GetAutoSubscriptions(pub.PubID);
                HttpPostAutoSubscription(Request.QueryString.ToString());

                if (Request.QueryString != null)
                    redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text, Request.QueryString.ToString(), browserInfo) && ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text, sb.ToString(), browserInfo);
                else
                    redirectstatus = ECNUtils.ECNHttpPost(email.Text, lblPubCode.Text, sb.ToString(), browserInfo);

                #endregion

                #region POST TO DI (for HMEDB)

                if (String.Equals(PubCode, "HMEDB", StringComparison.OrdinalIgnoreCase))
                {
                    ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();

                    try
                    {
                        string uri = "https://hme.dimins.com/cgi-bin/dicfg/dicfg.py?";
                        string post_data = string.Format("username={0}&password={1}&expdate={2}", email.Text.Trim(), GetPasswordByCustomerIDAndEmailAddress(), finalExpDate.Value.ToString("yyyy/MM/dd"));

                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                        request.KeepAlive = false;
                        request.Method = "POST";

                        byte[] postBytes = Encoding.ASCII.GetBytes(post_data);

                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = postBytes.Length;

                        Stream requestStream = request.GetRequestStream();

                        requestStream.Write(postBytes, 0, postBytes.Length);

                        requestStream.Close();


                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "HMEDB Data Posting to DI Failed", uri + post_data);
                        }
                        else
                        {
                            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "HMEDB Data Posting to DI Success", uri + post_data);
                        }
                    }
                    catch (Exception ex)
                    {
                        emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "HMEDB Data Posting to DI Failed", email.Text.Trim() + " / " + ex.Message);                    
                    }
                }

                #endregion

                if (!redirectstatus) { redirectstatusall = true; }

                if (redirectstatusall == true)
                {
                    TransactionID = string.Empty;
                    SendResponseEmail(finalExpDate.Value);

                    if (pub.PaidPageThankyouLink.Trim().Length > 0)
                        Response.Redirect(ReplaceCodeSnippets(pub.PaidPageThankyouLink, finalExpDate.Value));
                    else
                        Response.Redirect("upi_thankyou.aspx?pubcode=" + PubCode);
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

        private void SendCompResponseEmail()
        {
            if (pub.CompResponseEmailHTML.Trim().Length > 0)
            {
                string emailBody = ReplaceCodeSnippetsComp(pub.CompResponseEmailHTML);
                ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();

                if (pub.CompResponseEmailHTML.Trim().Length > 0)
                    emailFunctions.SimpleSend(Email, pub.PaidPageFromName + " <" + pub.PaidPageFromEmail + ">", pub.PaidFormResponseEmailSubject, emailBody);
            }
        }

        private void SendResponseEmail(DateTime finalExpDate)
        {
            if (pub.PaidResponseEmail.Trim().Length > 0)
            {
                string adminEmailbody = ReplaceCodeSnippets(pub.PaidResponseEmail, finalExpDate);
                ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();

                if (pub.PaidPageFromEmail.Trim().Length > 0 && pub.PaidFormResponseEmailSubject.Trim().Length > 0)
                    emailFunctions.SimpleSend(Email, pub.PaidPageFromName + " <" + pub.PaidPageFromEmail + ">", pub.PaidFormResponseEmailSubject, adminEmailbody);
                else
                    emailFunctions.SimpleSend(Email, "info@unitedpublications.com", "Receipt/Purchase Confirmation", adminEmailbody);
            }
        }

        private string ReplaceCodeSnippetsComp(string emailBody)
        {
            string body = Regex.Replace(emailBody, "%%REGDATE%%", DateTime.Now.ToString("M/dd/yyyy"), RegexOptions.IgnoreCase);

            body = Regex.Replace(body, "%%FULLNAME%%", FirstName + " " + LastName, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ADDRESS%%", Address1, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ADDRESS2%%", Address2, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%CITY%%", City, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%PASSWORD%%", GetPasswordByCustomerIDAndEmailAddress(), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EMAILADDRESS%%", Email, RegexOptions.IgnoreCase);

            if (pnlBillingState.Visible)
                body = Regex.Replace(body, "%%STATE%%", State, RegexOptions.IgnoreCase);
            else
                body = Regex.Replace(body, "%%STATE%%", StateInt, RegexOptions.IgnoreCase);

            body = Regex.Replace(body, "%%ZIP%%", Zip, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ZIPCARDBILL%%", Zip, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%PRICE%%", "0.00", RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%TOTAL%%", "0.00", RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%SUBTOTAL%%", "0.00", RegexOptions.IgnoreCase);

            body = Regex.Replace(body, "%%TAX%%", "0.00", RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%SHIPPING%%", "0.00", RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%GRANDTOTAL%%", "0.00", RegexOptions.IgnoreCase);
            return body;
        }

        private string ReplaceCodeSnippets(string emailBody, DateTime finalExpDate)
        {
            upipaidpub.Objects.Item orderItem = itemList[0];
            string body = Regex.Replace(emailBody, "%%REGDATE%%", DateTime.Now.ToString("M/dd/yyyy"), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EXPDATE%%", finalExpDate.ToString("M/dd/yyyy"), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EXPDATEYYYYMMDD%%", finalExpDate.ToString("M/dd/yyyy"), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%CCNUMBER%%", "************" + user_CCNumber.Text.Substring(user_CCNumber.Text.Length - 4, 4), RegexOptions.IgnoreCase);

            body = Regex.Replace(body, "%%CCHOLDER%%", user_CardHolderName.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%CCTYPE%%", user_CCType.SelectedItem.Value.ToUpper(), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%CCEXPDATE%%", user_Exp_Month.SelectedItem.Value + "/" + user_Exp_Year.SelectedItem.Value, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%FULLNAME%%", first.Text + " " + last.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ADDRESS%%", txtBillingAddress1.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ADDRESS2%%", txtBillingAddress2.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%CITY%%", txtBillingCity.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%PASSWORD%%", GetPasswordByCustomerIDAndEmailAddress(), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EMAILADDRESS%%", email.Text.Trim(), RegexOptions.IgnoreCase);

            if (pnlBillingState.Visible)
                body = Regex.Replace(body, "%%STATE%%", drpBillingState.SelectedItem.Value, RegexOptions.IgnoreCase);
            else
                body = Regex.Replace(body, "%%STATE%%", txtBillingStateInt.Text, RegexOptions.IgnoreCase);

            body = Regex.Replace(body, "%%ZIP%%", txtBillingZip.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ZIPCARDBILL%%", txtBillingZip.Text, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%CODE%%", orderItem.ItemCode, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%ITEM%%", orderItem.ItemName, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%QUANTITY%%", orderItem.ItemQty, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%PRICE%%", orderItem.ItemAmount, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%TOTAL%%", orderItem.ItemAmount, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%SUBTOTAL%%", orderItem.ItemAmount, RegexOptions.IgnoreCase);

            body = Regex.Replace(body, "%%TAX%%", "0.00", RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%SHIPPING%%", "0.00", RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%GRANDTOTAL%%", orderItem.ItemAmount, RegexOptions.IgnoreCase);
            return body;
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
                string MerchantId = WebConfigurationManager.AppSettings["AuthorizeDotnetLogin"].ToString();
                string Signature = WebConfigurationManager.AppSettings["AuthorizeDotnetKey"].ToString();

                string CardNo = Regex.Replace(user_CCNumber.Text, @"[ -/._#]", "");
                CardNo = CardNo.Trim().Replace(" ", "");
                ppPay.CardNum = CardNo;
                ppPay.ExpDate = user_Exp_Month.Text + user_Exp_Year.Text;
                ppPay.Amount = GetTotal().ToString().TrimStart('$');
                ppPay.AddInvoice(PromoCode);

                if (pnlContactInfo.Visible)
                    ppPay.Company = txtCompany.Text;
                else
                    ppPay.Company = Company;

                var gateway = new Gateway(MerchantId, Signature);
                string orderItems = string.Empty;
                gateway.TestMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["AuthorizeDotNetDemoMode"].ToString());

                string countrCode = "";
                if (drpBillingCountry.SelectedItem.Text.ToUpper() == "UNITED STATES")
                {
                    countrCode = "US";
                }
                else if (drpBillingCountry.SelectedItem.Text.ToUpper() == "CANADA")
                {
                    countrCode = "CA";
                }
                else
                {
                    countrCode = drpBillingCountry.SelectedItem.Text.ToUpper();
                }

                ppPay.AddCardCode(user_CCVerfication.Text);
                ppPay.Country = countrCode;
                ppPay.CustomerIp = System.Web.HttpContext.Current.Request.UserHostAddress;
                ppPay.Phone = phone.Text;
                ppPay.City = txtBillingCity.Text;
                ppPay.Email = email.Text;
                ppPay.Fax = fax.Text;

                if (this.itemList.Count > 0)
                {
                    string SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, email.Text);

                    /* Add Billing Address*/
                    if (countrCode.ToUpper() == "US" || countrCode.ToUpper() == "CA")
                        ppPay.AddCustomer(SubscriberID, email.Text, first.Text, last.Text, txtBillingAddress1.Text + " " + txtBillingAddress2.Text, txtBillingCity.Text, drpBillingState.SelectedItem.Value, txtBillingZip.Text);
                    else
                        ppPay.AddCustomer(SubscriberID, email.Text, first.Text, last.Text, txtBillingAddress1.Text + " " + txtBillingAddress2.Text, txtBillingCity.Text, txtBillingStateInt.Text, txtBillingZip.Text);

                    /* Add shipping Address */
                    if (countrCode.ToUpper() == "US" || countrCode.ToUpper() == "CA")
                        ppPay.AddShipping(SubscriberID, email.Text, first.Text, last.Text, txtShippingAddress1.Text + " " + txtShippingAddress2.Text, txtShippingCity.Text, drpShippingState.SelectedItem.Value, txtShippingZip.Text);
                    else if (pnlShippingAddress.Visible)
                        ppPay.AddShipping(SubscriberID, email.Text, first.Text, last.Text, txtShippingAddress1.Text + " " + txtShippingAddress2.Text, txtShippingCity.Text, txtShippingStateInt.Text, txtShippingZip.Text);

                    ppPay.ShipToCity = txtShippingCity.Text;
                    ppPay.ShipToCountry = drpShippingCountry.SelectedItem.Text;
                }

                int totalQuantity = 0;
                string orderdesc = "";
                string orderNames = "";
                foreach (upipaidpub.Objects.Item item in itemList)
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
                ppPay.AddLineItem(itemID, itemName, itemDesc, totalQuantity, itemPrice, false);
                ppPay.AddMerchantValue("x_Description", itemDesc);
                notifyAuthorizationRequest(ppPay);
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

        private void notifyAuthorizationRequest(AuthorizationRequest requestObject)
        {
            StringBuilder sb = new StringBuilder();

            if(requestObject.CardNum.Length>=15)
                sb.AppendLine("<b>CardNum  :</b>  " + requestObject.CardNum.Substring(requestObject.CardNum.Length - 5, 4) + "<BR>");
            else
                sb.AppendLine("CardNum  :</b>  " + requestObject.CardNum + "<BR>");

            sb.AppendLine("<b>Pubcode  :</b>  " + PubCode + "<BR>");

            sb.AppendLine("<b>ExpDate  :</b>  " + requestObject.ExpDate + "<BR>");
            sb.AppendLine("<b>Amount  :</b>  " + requestObject.Amount + "<BR>");
            sb.AppendLine("<b>Company  :</b>  " + requestObject.Company + "<BR>");
            sb.AppendLine("<b>Invoice  :</b>  " + requestObject.InvoiceNum+ "<BR>");
            sb.AppendLine("<b>AddCardCode  :</b>  " + requestObject.CardCode+ "<BR>");
            sb.AppendLine("<b>Country  :</b>  " + countryCode+ "<BR>");
            sb.AppendLine("<b>CustomerIp  :</b>  " +  requestObject.CustomerIp+ "<BR>");
            sb.AppendLine("<b>Phone  :</b>  " + requestObject.Phone+ "<BR>");
            sb.AppendLine("<b>City  :</b>  " + requestObject.City+ "<BR>");
            sb.AppendLine("<b>Email  :</b>  " + requestObject.Email + "<BR>");
            sb.AppendLine("<b>Fax  :</b>  " +  requestObject.Fax + "<BR>");

            sb.AppendLine("<b>====Customer Information====</b>" + "<BR>");
            sb.AppendLine("<b>CustId  :</b>  " + requestObject.CustId+ "<BR>");
            sb.AppendLine("<b>FirstName  :</b>  " + requestObject.FirstName+ "<BR>");
            sb.AppendLine("<b>LastName  :</b>  " + requestObject.LastName+ "<BR>");            
            sb.AppendLine("<b>Address  :</b>  " + requestObject.Address+ "<BR>");            
            sb.AppendLine("<b>State  :</b>  " + requestObject.State+ "<BR>");            
            sb.AppendLine("<b>Zip  :</b>  " + requestObject.Zip+ "<BR>");

            sb.AppendLine("<b>====Shipping Information====</b>" + "<BR>");
            sb.AppendLine("<b>ShipToFirstName  :</b>  " + requestObject.ShipToFirstName+ "<BR>");
            sb.AppendLine("<b>ShipToLastName  :</b>  " + requestObject.ShipToLastName+ "<BR>");            
            sb.AppendLine("<b>ShipToAddress  :</b>  " + requestObject.ShipToAddress+ "<BR>");  
            sb.AppendLine("<b>ShipToCity  :</b>  " + requestObject.ShipToCity+ "<BR>");
            sb.AppendLine("<b>ShipToState  :</b>  " + requestObject.ShipToState + "<BR>");
            sb.AppendLine("<b>ShipToCountry  :</b>  " + requestObject.ShipToCountry+ "<BR>");
            sb.AppendLine("<b>ShipToZip  :</b>  " + requestObject.ShipToZip+ "<BR>");

            sb.AppendLine("<b>====Misc====</b><BR>");
            sb.AppendLine("<b>TransactionId  :</b>  " + requestObject.TransId+ "<BR>");
            sb.AppendLine("<b>UserAgent  :</b>  " + System.Web.HttpContext.Current.Request.UserAgent+ "<BR>");


            string adminEmailbody = sb.ToString();
            ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();
            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Authorize.Net Transaction Request", adminEmailbody);

        }

        private void notifyTransactionFail(GatewayResponse responseObject, Exception ex)
        {
            StringBuilder sb = new StringBuilder();               
            if (responseObject != null)
            {
                sb.AppendLine("<b>====Response Information====</b>" + "<BR>");
                sb.AppendLine("<b>TransactionID  :</b>  " + responseObject.TransactionID+ "<BR>");
                sb.AppendLine("<b>Amount  :</b>  " + responseObject.Amount+ "<BR>");
                sb.AppendLine("<b>AuthorizationCode  :</b>  " + responseObject.AuthorizationCode+ "<BR>");
                sb.AppendLine("<b>TransactionType  :</b>  " + responseObject.TransactionType+ "<BR>");
                sb.AppendLine("<b>RawResponse  :</b>  " + responseObject.RawResponse.ToString()+ "<BR>");
                sb.AppendLine("<b>ResponseCode  :</b>  " + responseObject.ResponseCode+ "<BR>");
                sb.AppendLine("<b>Error  :</b>  " + responseObject.Error+ "<BR>");
                sb.AppendLine("<b>Message  :</b>  " + responseObject.Message+ "<BR>");

                sb.AppendLine("<b>Description  :</b>  " + responseObject.Description+ "<BR>");
                sb.AppendLine("<b>CardType  :</b>  " + responseObject.CardType+ "<BR>");

                if (responseObject.CardNumber.Length >= 15)
                    sb.AppendLine("<b>CardNum  :</b>  " + responseObject.CardNumber.Substring(responseObject.CardNumber.Length - 5, 4)+ "<BR>");
                else
                    sb.AppendLine("<b>CardNum  :</b>  " + responseObject.CardNumber+ "<BR>");

                sb.AppendLine("<b>CAVResponse  :</b>  " + responseObject.CAVResponse+ "<BR>");
                sb.AppendLine("<b>CCVResponse  :</b>  " + responseObject.CCVResponse+ "<BR>");
                sb.AppendLine("<b>AVSResponse  :</b>  " + responseObject.AVSResponse+ "<BR>");
                sb.AppendLine("<b>Address  :</b>  " + responseObject.Address+ "<BR>");
                sb.AppendLine("<b>City  :</b>  " + responseObject.City+ "<BR>");
                sb.AppendLine("<b>State  :</b>  " + responseObject.State+ "<BR>");
                sb.AppendLine("<b>Country  :</b>  " + responseObject.Country+ "<BR>");
                sb.AppendLine("<b>ZipCode  :</b>  " + responseObject.ZipCode+ "<BR>");

                sb.AppendLine("<b>====Shipping Information====</b>" + "<BR>");
                sb.AppendLine("<b>ShipFirstName  :</b>  " + responseObject.ShipFirstName+ "<BR>");
                sb.AppendLine("<b>ShipLastName  :</b>  " + responseObject.ShipLastName+ "<BR>");
                sb.AppendLine("<b>ShipAddress  :</b>  " + responseObject.ShipAddress+ "<BR>");
                sb.AppendLine("<b>ShipCity  :</b>  " + responseObject.ShipCity+ "<BR>");
                sb.AppendLine("<b>ShipState  :</b>  " + responseObject.ShipState+ "<BR>");
                sb.AppendLine("<b>ShipCountry  :</b>  " + responseObject.ShipCountry+ "<BR>");
                sb.AppendLine("<b>ShipZipCode  :</b>  " + responseObject.ShipZipCode+ "<BR>");
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
            emailFunctions.SimpleSend(ConfigurationManager.AppSettings["Admin_ToEmail"], ConfigurationManager.AppSettings["Admin_FromEmail"], "Authorize.Net Transaction Response-FAIL", adminEmailbody);

        }
        
        private bool IsCacheClear()
        {
            try
            {
                SqlCommand cmdGetCacheClear = new SqlCommand("select IsCacheClear from Publications with (NOLOCK) where PubCode = @PubCode");
                cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
                return Convert.ToBoolean(upipaidpub.Objects.DataFunctions.ExecuteScalar("", cmdGetCacheClear).ToString());
            }
            catch { return false; }
        }

        private void UpdateCacheClear()
        {
            try
            {
                SqlCommand cmdGetCacheClear = new SqlCommand("update Publications set IsCacheClear = 0 where PubCode = @PubCode");
                cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
                upipaidpub.Objects.DataFunctions.Execute(cmdGetCacheClear);
            }
            catch { }
        }

        private string GetPasswordByCustomerIDAndEmailAddress()
        {
            string password = "";

            try
            {
                SqlCommand cmdGetPassword = new SqlCommand("select top 1 Password from Emails with (NOLOCK) where CustomerID = @CustomerID and EmailAddress = @EmailAddress");
                cmdGetPassword.Parameters.AddWithValue("@CustomerID", pub.ECNCustomerID);
                cmdGetPassword.Parameters.AddWithValue("@EmailAddress", email.Text.Trim());
                password = upipaidpub.Objects.DataFunctions.ExecuteScalar("communicator", cmdGetPassword).ToString();
                return password;
            }
            catch { return ""; }
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            user_CardHolderName.Text = "";
            user_CCType.SelectedIndex = user_CCType.Items.IndexOf(user_CCType.Items.FindByValue(""));
            user_CCNumber.Text = "";
            user_Exp_Month.SelectedIndex = user_Exp_Month.Items.IndexOf(user_Exp_Month.Items.FindByValue(""));
            user_Exp_Year.SelectedIndex = user_Exp_Year.Items.IndexOf(user_Exp_Year.Items.FindByValue(""));
            user_CCVerfication.Text = "";
            txtZipCardBill.Text = "";

            first.Text = "";
            last.Text = "";
            phone.Text = "";
            fax.Text = "";
            txtCompany.Text = "";

            txtShippingAddress1.Text = "";
            txtShippingAddress2.Text = "";
            txtShippingCity.Text = "";
            txtShippingStateInt.Text = "";
            txtShippingZip.Text = "";
            drpShippingState.ClearSelection();
            drpShippingCountry.ClearSelection();

            txtBillingAddress1.Text = "";
            txtBillingAddress2.Text = "";
            txtBillingCity.Text = "";
            txtBillingZip.Text = "";
            txtBillingStateInt.Text = "";
            drpBillingState.ClearSelection();
            drpBillingCountry.ClearSelection();
            LoadFields();
            phError.Visible = false;
        }

        protected void btnCopyBillingtoShipping_Click(object sender, EventArgs e)
        {
            txtShippingAddress1.Text = txtBillingAddress1.Text;
            txtShippingAddress2.Text = txtBillingAddress2.Text;
            txtShippingCity.Text = txtBillingCity.Text;
            txtShippingZip.Text = txtBillingZip.Text;

            if (drpBillingCountry.SelectedItem.Text.ToUpper() == "UNITED STATES" || drpBillingCountry.SelectedItem.Text.ToUpper() == "CANADA")
            {
                pnlShippingState.Visible = true;
                pnlShippingStateInt.Visible = false;
                LoadShippingState(drpBillingCountry.SelectedItem.Text);
            }
            else
            {
                pnlShippingState.Visible = false;
                pnlShippingStateInt.Visible = true;
            }

            try
            {
                drpShippingState.ClearSelection();
                drpShippingState.Items.FindByValue(drpBillingState.SelectedItem.Value).Selected = true;
            }
            catch { }

            try
            {
                drpShippingCountry.ClearSelection();
                drpShippingCountry.Items.FindByValue(drpBillingCountry.SelectedItem.Value).Selected = true;
            }
            catch { }

            if (pnlBillingStateInt.Visible && pnlShippingStateInt.Visible)
                txtShippingStateInt.Text = txtBillingStateInt.Text;
        }

        protected void drpQuantity_SelectedIndexChanged(Object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    Label oneYearPrice = (Label)row.FindControl("lblOneYearPrice");
                    Label twoYearPrice = (Label)row.FindControl("lblTwoYearPrice");
                    Label threeYearPrice = (Label)row.FindControl("lblThreeYearPrice");
                    Label fourYearPrice = (Label)row.FindControl("lblFourYearPrice");
                    Label fiveYearPrice = (Label)row.FindControl("lblFiveYearPrice");
                    DropDownList ddlQuantity = (DropDownList)row.FindControl("drpQuantity");
                    Label lblUnitPrice = (Label)row.FindControl("lblUnitPrice");

                    switch (Convert.ToInt32(ddlQuantity.SelectedItem.Value))
                    {
                        case 1:
                            oneYearPrice.Visible = true;
                            twoYearPrice.Visible = false;
                            threeYearPrice.Visible = false;
                            fourYearPrice.Visible = false;
                            fiveYearPrice.Visible = false;
                            lblUnitPrice.Text = String.Format("{0:0.00}", (Convert.ToDecimal(oneYearPrice.Text) / 1M).ToString());
                            break;
                        case 2:
                            oneYearPrice.Visible = false;
                            twoYearPrice.Visible = true;
                            threeYearPrice.Visible = false;
                            fourYearPrice.Visible = false;
                            fiveYearPrice.Visible = false;
                            lblUnitPrice.Text = String.Format("{0:0.00}", (Convert.ToDecimal(twoYearPrice.Text) / 2M).ToString());
                            break;
                        case 3:
                            oneYearPrice.Visible = false;
                            twoYearPrice.Visible = false;
                            threeYearPrice.Visible = true;
                            fourYearPrice.Visible = false;
                            fiveYearPrice.Visible = false;
                            lblUnitPrice.Text = String.Format("{0:0.00}", (Convert.ToDecimal(threeYearPrice.Text) / 3M).ToString());
                            break;
                        case 4:
                            oneYearPrice.Visible = false;
                            twoYearPrice.Visible = false;
                            threeYearPrice.Visible = false;
                            fourYearPrice.Visible = true;
                            fiveYearPrice.Visible = false;
                            lblUnitPrice.Text = String.Format("{0:0.00}", (Convert.ToDecimal(fourYearPrice.Text) / 4M).ToString());
                            break;
                        case 5:
                            oneYearPrice.Visible = false;
                            twoYearPrice.Visible = false;
                            threeYearPrice.Visible = false;
                            fourYearPrice.Visible = false;
                            fiveYearPrice.Visible = true;
                            lblUnitPrice.Text = String.Format("{0:0.00}", (Convert.ToDecimal(fiveYearPrice.Text) / 5M).ToString());
                            break;
                    }

                    CalculatePrice(row);

                    Label lblTotalAmount = (Label)grdMagazines.FooterRow.FindControl("lblTotalAmount");
                    Label lblDiscount = (Label)grdMagazines.FooterRow.FindControl("lblDiscountPromocodePrice");
                    Label lblDiscountLabel = (Label)grdMagazines.FooterRow.FindControl("lblDiscountPromocode");

                    Discount discountObj = new Discount();
                    discountObj = CalculatePromoCodeDiscount(totalPrice, quantity);
                    discount = discountObj.DiscountAmount;

                    if (discount > 0.0M && discountObj.ID.Length > 0)
                        totalPrice = totalPrice - discount;

                    if (lblDiscount != null && discount > 0.0M)
                    {
                        lblDiscount.Visible = true;
                        lblDiscountLabel.Visible = true;
                        lblDiscount.Text = String.Format("{0:0.00}", discount.ToString());
                    }

                    if (lblTotalAmount != null)
                    {
                        if (discount == 0.0M) { lblDiscount.Visible = false; lblDiscountLabel.Visible = false; }
                        lblTotalAmount.Text = String.Format("{0:0.00}", totalPrice.ToString());
                    }
                }
            }
        }

        protected void grdMagazines_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dtMagList = GetMagList(PubCode);
                e.Row.Cells[2].Text = dtMagList.Rows[0]["quantityHeaderText"].ToString();
                e.Row.Cells[3].Text = dtMagList.Rows[0]["priceheaderText"].ToString();
                e.Row.Cells[4].Text = dtMagList.Rows[0]["totalPriceheaderText"].ToString();
            }
        }

        protected void grdMagazines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblPubCode = (Label)e.Row.FindControl("lblPubCode");
            Label lblOneYearPrice = (Label)e.Row.FindControl("lblOneYearPrice");
            Label lblTwoYearPrice = (Label)e.Row.FindControl("lblTwoYearPrice");
            Label lblThreeYearPrice = (Label)e.Row.FindControl("lblThreeYearPrice");
            DropDownList ddlQuantity = (DropDownList)e.Row.FindControl("drpQuantity");
            Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
            DataTable dtpubs = null;
            int quantity = 0;

            if (lblPubCode != null)
            {
                dtpubs = this.GetMagList(lblPubCode.Text);
                quantity = Convert.ToInt32(dtpubs.Rows[0]["quantity"]);
                totalPrice = Convert.ToDecimal(dtpubs.Rows[0]["oneyearprice"]);

                if (quantity == 1 && lblQuantity != null)
                {
                    ddlQuantity.Visible = false;
                    lblQuantity.Visible = true;
                    CalculatePrice(e.Row);
                    return;
                }
            }

            if (ddlQuantity != null && !lblQuantity.Visible)
            {
                if (PubCode.ToUpper() == "HMES" || PubCode.ToUpper() == "HMEDB")
                {
                    for (int i = 1; i <= quantity; i++)
                        ddlQuantity.Items.Add(new ListItem(i == 1 ? i.ToString() + " year" : i.ToString() + " years", i.ToString()));
                }
                else
                {
                    for (int i = 1; i <= quantity; i++)
                        ddlQuantity.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                ddlQuantity.ClearSelection();
                ddlQuantity.Items[0].Selected = true;
            }

            CalculatePrice(e.Row);
        }

        protected void drpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpBillingCountry.SelectedItem.Text.ToUpper() == "UNITED STATES" || drpBillingCountry.SelectedItem.Text.ToUpper() == "CANADA")
            {
                pnlBillingState.Visible = true;
                pnlBillingStateInt.Visible = false;
                LoadBillingState(drpBillingCountry.SelectedItem.Text);
            }
            else
            {
                pnlBillingState.Visible = false;
                pnlBillingStateInt.Visible = true;
            }
        }

        protected void drpShippingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpShippingCountry.SelectedItem.Text.ToUpper() == "UNITED STATES" || drpShippingCountry.SelectedItem.Text.ToUpper() == "CANADA")
            {
                pnlShippingState.Visible = true;
                pnlShippingStateInt.Visible = false;
                LoadShippingState(drpShippingCountry.SelectedItem.Text);
            }
            else
            {
                pnlShippingState.Visible = false;
                pnlShippingStateInt.Visible = true;
            }
        }

        private void LoadBillingState(string cCode)
        {
            drpBillingState.Items.Clear();

            if (cCode.ToUpper() == "UNITED STATES" || cCode.ToUpper() == "UNITED STATES OF AMERICA")
            {
                addlistitemBillingState("", "Select a State", "");
                addlistitemBillingState("AK", "Alaska", "USA");
                addlistitemBillingState("AL", "Alabama", "USA");
                addlistitemBillingState("AR", "Arkansas", "USA");
                addlistitemBillingState("AZ", "Arizona", "USA");
                addlistitemBillingState("CA", "California", "USA");
                addlistitemBillingState("CO", "Colorado", "USA");
                addlistitemBillingState("CT", "Connecticut", "USA");
                addlistitemBillingState("DC", "Washington D.C.", "USA");
                addlistitemBillingState("DE", "Delaware", "USA");
                addlistitemBillingState("FL", "Florida", "USA");
                addlistitemBillingState("GA", "Georgia", "USA");
                addlistitemBillingState("HI", "Hawaii", "USA");
                addlistitemBillingState("IA", "Iowa", "USA");
                addlistitemBillingState("ID", "Idaho", "USA");
                addlistitemBillingState("IL", "Illinois", "USA");
                addlistitemBillingState("IN", "Indiana", "USA");
                addlistitemBillingState("KS", "Kansas", "USA");
                addlistitemBillingState("KY", "Kentucky", "USA");
                addlistitemBillingState("LA", "Louisiana", "USA");
                addlistitemBillingState("MA", "Massachusetts", "USA");
                addlistitemBillingState("MD", "Maryland", "USA");
                addlistitemBillingState("ME", "Maine", "USA");
                addlistitemBillingState("MI", "Michigan", "USA");
                addlistitemBillingState("MN", "Minnesota", "USA");
                addlistitemBillingState("MO", "Missourri", "USA");
                addlistitemBillingState("MS", "Mississippi", "USA");
                addlistitemBillingState("MT", "Montana", "USA");
                addlistitemBillingState("NC", "North Carolina", "USA");
                addlistitemBillingState("ND", "North Dakota", "USA");
                addlistitemBillingState("NE", "Nebraska", "USA");
                addlistitemBillingState("NH", "New Hampshire", "USA");
                addlistitemBillingState("NJ", "New Jersey", "USA");
                addlistitemBillingState("NM", "New Mexico", "USA");
                addlistitemBillingState("NV", "Nevada", "USA");
                addlistitemBillingState("NY", "New York", "USA");
                addlistitemBillingState("OH", "Ohio", "USA");
                addlistitemBillingState("OK", "Oklahoma", "USA");
                addlistitemBillingState("OR", "Oregon", "USA");
                addlistitemBillingState("PA", "Pennsylvania", "USA");
                addlistitemBillingState("PR", "Puerto Rico", "USA");
                addlistitemBillingState("RI", "Rhode Island", "USA");
                addlistitemBillingState("SC", "South Carolina", "USA");
                addlistitemBillingState("SD", "South Dakota", "USA");
                addlistitemBillingState("TN", "Tennessee", "USA");
                addlistitemBillingState("TX", "Texas", "USA");
                addlistitemBillingState("AE", "US Military(AE)", "USA");
                addlistitemBillingState("AP", "US Military(AP)", "USA");
                addlistitemBillingState("AA", "US Military(AA)", "USA");
                addlistitemBillingState("UT", "Utah", "USA");
                addlistitemBillingState("VA", "Virginia", "USA");
                addlistitemBillingState("VT", "Vermont", "USA");
                addlistitemBillingState("WA", "Washington", "USA");
                addlistitemBillingState("WI", "Wisconsin", "USA");
                addlistitemBillingState("WV", "West Virginia", "USA");
                addlistitemBillingState("WY", "Wyoming", "USA");
            }
            else if (cCode.ToUpper() == "CANADA")
            {
                addlistitemBillingState("", "Select a Province", "");
                addlistitemBillingState("AB", "Alberta", "Canada");
                addlistitemBillingState("BC", "British Columbia", "Canada");
                addlistitemBillingState("MB", "Manitoba", "Canada");
                addlistitemBillingState("NB", "New Brunswick", "Canada");
                addlistitemBillingState("NL", "New Foundland and Labrador", "Canada");
                addlistitemBillingState("NS", "Nova Scotia", "Canada");
                addlistitemBillingState("ON", "Ontario", "Canada");
                addlistitemBillingState("PE", "Prince Edward Island", "Canada");
                addlistitemBillingState("QC", "Quebec", "Canada");
                addlistitemBillingState("SK", "Saskatchewan", "Canada");
                addlistitemBillingState("YT", "Yukon Territories", "Canada");
            }
        }

        private void LoadShippingState(string cCode)
        {
            drpShippingState.Items.Clear();

            if (cCode.ToUpper() == "UNITED STATES" || cCode.ToUpper() == "UNITED STATES OF AMERICA")
            {
                addlistitemShippingState("", "Select a State", "");
                addlistitemShippingState("AK", "Alaska", "USA");
                addlistitemShippingState("AL", "Alabama", "USA");
                addlistitemShippingState("AR", "Arkansas", "USA");
                addlistitemShippingState("AZ", "Arizona", "USA");
                addlistitemShippingState("CA", "California", "USA");
                addlistitemShippingState("CO", "Colorado", "USA");
                addlistitemShippingState("CT", "Connecticut", "USA");
                addlistitemShippingState("DC", "Washington D.C.", "USA");
                addlistitemShippingState("DE", "Delaware", "USA");
                addlistitemShippingState("FL", "Florida", "USA");
                addlistitemShippingState("GA", "Georgia", "USA");
                addlistitemShippingState("HI", "Hawaii", "USA");
                addlistitemShippingState("IA", "Iowa", "USA");
                addlistitemShippingState("ID", "Idaho", "USA");
                addlistitemShippingState("IL", "Illinois", "USA");
                addlistitemShippingState("IN", "Indiana", "USA");
                addlistitemShippingState("KS", "Kansas", "USA");
                addlistitemShippingState("KY", "Kentucky", "USA");
                addlistitemShippingState("LA", "Louisiana", "USA");
                addlistitemShippingState("MA", "Massachusetts", "USA");
                addlistitemShippingState("MD", "Maryland", "USA");
                addlistitemShippingState("ME", "Maine", "USA");
                addlistitemShippingState("MI", "Michigan", "USA");
                addlistitemShippingState("MN", "Minnesota", "USA");
                addlistitemShippingState("MO", "Missourri", "USA");
                addlistitemShippingState("MS", "Mississippi", "USA");
                addlistitemShippingState("MT", "Montana", "USA");
                addlistitemShippingState("NC", "North Carolina", "USA");
                addlistitemShippingState("ND", "North Dakota", "USA");
                addlistitemShippingState("NE", "Nebraska", "USA");
                addlistitemShippingState("NH", "New Hampshire", "USA");
                addlistitemShippingState("NJ", "New Jersey", "USA");
                addlistitemShippingState("NM", "New Mexico", "USA");
                addlistitemShippingState("NV", "Nevada", "USA");
                addlistitemShippingState("NY", "New York", "USA");
                addlistitemShippingState("OH", "Ohio", "USA");
                addlistitemShippingState("OK", "Oklahoma", "USA");
                addlistitemShippingState("OR", "Oregon", "USA");
                addlistitemShippingState("PA", "Pennsylvania", "USA");
                addlistitemShippingState("PR", "Puerto Rico", "USA");
                addlistitemShippingState("RI", "Rhode Island", "USA");
                addlistitemShippingState("SC", "South Carolina", "USA");
                addlistitemShippingState("SD", "South Dakota", "USA");
                addlistitemShippingState("TN", "Tennessee", "USA");
                addlistitemShippingState("TX", "Texas", "USA");
                addlistitemShippingState("AE", "US Military(AE)", "USA");
                addlistitemShippingState("AP", "US Military(AP)", "USA");
                addlistitemShippingState("AA", "US Military(AA)", "USA");
                addlistitemShippingState("UT", "Utah", "USA");
                addlistitemShippingState("VA", "Virginia", "USA");
                addlistitemShippingState("VT", "Vermont", "USA");
                addlistitemShippingState("WA", "Washington", "USA");
                addlistitemShippingState("WI", "Wisconsin", "USA");
                addlistitemShippingState("WV", "West Virginia", "USA");
                addlistitemShippingState("WY", "Wyoming", "USA");
            }
            else if (cCode.ToUpper() == "CANADA")
            {
                addlistitemShippingState("", "Select a Province", "");
                addlistitemShippingState("AB", "Alberta", "Canada");
                addlistitemShippingState("BC", "British Columbia", "Canada");
                addlistitemShippingState("MB", "Manitoba", "Canada");
                addlistitemShippingState("NB", "New Brunswick", "Canada");
                addlistitemShippingState("NL", "New Foundland and Labrador", "Canada");
                addlistitemShippingState("NS", "Nova Scotia", "Canada");
                addlistitemShippingState("ON", "Ontario", "Canada");
                addlistitemShippingState("PE", "Prince Edward Island", "Canada");
                addlistitemShippingState("QC", "Quebec", "Canada");
                addlistitemShippingState("SK", "Saskatchewan", "Canada");
                addlistitemShippingState("YT", "Yukon Territories", "Canada");
            }
        }
    }

    public class Discount
    {
        public Discount()
        {
            this.ID = "";
            this.DiscountType = Enums.DiscountType.PERCENTAGE;
            this.DiscountAmount = 0.0M;
            this.OriginalAmount = 0.0M;
            this.DiscountPercentage = 0.0M;
            this.Year = 1;
            this.ExpirationDate = DateTime.MinValue;
            this.IsComp = false;
            this.IsMultiComp = false;
            this.DiscountForEveryPromoCode = false;
            this.MultiItemDiscount = false;
        }

        public string ID { get; set; }
        public Enums.DiscountType DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int Year { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsComp { get; set; }
        public bool IsMultiComp { get; set; }
        public bool DiscountForEveryPromoCode { get; set; }
        public decimal EveryPromocodeDiscAmount { get; set; }
        public bool MultiItemDiscount { get; set; }
    }
}