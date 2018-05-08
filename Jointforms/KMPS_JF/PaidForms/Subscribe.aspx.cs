using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using KMPS_JF_Objects.Objects;
using System.IO;
using ecn.communicator.classes;
using System.Text.RegularExpressions;
using KM.Common;
using Enums = KMPS_JF_Objects.Objects.Enums;

namespace KMPS_JF.PaidForms
{
    public partial class Subscribe : System.Web.UI.Page
    {
        private double unitPrice = 0.0;
        private double totalPrice = 0.0;
        private string PubCodeList = string.Empty;
        private ccProcessing ccProcessor = null;
        private List<Item> itemList = new List<Item>();
        private KMPS_JF_Objects.Objects.Utilities util = null;
        private Publication pub = null;
        private List<Magazine> objMagList = null;

        #region PROPERTIES

        public string PubCode
        {
            get
            {
                try
                {
                    if (ViewState["PubCode"] == null || string.IsNullOrEmpty(ViewState["PubCode"].ToString()))
                    {
                        if (Request.QueryString["pubcode"].ToString().Contains(','))
                            return Request.QueryString["pubcode"].ToString().Split(',')[0].ToString();
                        else
                            return Request.QueryString["pubcode"].ToString();
                    }
                    else
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
                    return Request.QueryString["user_demo39"].ToString();
                }
                catch { return String.Empty; }
            }
            set
            {
                ViewState["user_demo39"] = value;
            }
        }

        public string Email
        {
            get
            {
                try
                {
                    return Request.QueryString["e"].ToString();
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

        public int CustomerID
        {
            get
            {
                return Convert.ToInt32(ViewState["CustomerID"]);
            }
            set
            {
                ViewState["CustomerID"] = value;
            }
        }

        public int GroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["GroupID"]);
            }
            set
            {
                ViewState["GroupID"] = value;
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

        #endregion

        #region Variable declaration for ECN Web Request

        //private Groups ECNPostgroup;
        Hashtable hECNPostParams = new Hashtable();

        private Hashtable ECNPosthProfileFields = new Hashtable();
        private Hashtable ECNPosthUDFFields = new Hashtable();

        //private string ECNPostResponse_FromEmail = "";
        //private string ECNPostResponse_UserMsgSubject = "";
        //private string ECNPostResponse_UserMsgBody = "";
        //private string ECNPostResponse_UserScreen = "";
        //private string ECNPostResponse_AdminEmail = "";
        //private string ECNPostResponse_AdminMsgSubject = "";
        //private string ECNPostResponse_AdminMsgBody = "";

        //private int ECNPostCustomerID = 0;
        //private int ECNPostGroupID = 0;
        //private int ECNPostSmartFormID = 0;
        //private int ECNPostEmailID = 0;
        //private int ECNPostBlastID = 0;

        //private string ECNPostEmailAddress = "";
        //private string ECNPostSubscribe = "";
        //private string ECNPostFormat = "";

        //private string ECNPostReturnURL = "";
        //private string ECNPostfromURL = "";
        //private string ECNPostfromIP = "";

        #endregion

        private void LoadCountries()
        {
            if (PubCode.ToUpper() == "FNDR" || PubCode.ToUpper() == "RCHT")
            {
                drpBillingCountry.Items.Clear();
                drpBillingCountry_1.Items.Clear();
                drpBillingCountry.Items.Add(new ListItem("United States", "205"));
                drpBillingCountry_1.Items.Add(new ListItem("United States", "205"));

                drpShippingCountry.Items.Clear();
                drpShippingCountry.Items.Insert(0, new ListItem("United States", "205"));
                return;
            }
            else
            {
                string sql = "SELECT CountryID, CountryName from Country WHERE CountryID NOT IN (174,205) ORDER BY CountryName ASC";
                DataTable dt = DataFunctions.GetDataTable(sql);

                drpBillingCountry.DataSource = dt;
                drpBillingCountry.DataTextField = "CountryName";
                drpBillingCountry.DataValueField = "CountryID";
                drpBillingCountry.DataBind();
                drpBillingCountry.Items.Insert(0, new ListItem("United States", "205"));
                drpBillingCountry.Items.Insert(1, new ListItem("Canada", "174"));

                drpBillingCountry_1.DataSource = dt;
                drpBillingCountry_1.DataTextField = "CountryName";
                drpBillingCountry_1.DataValueField = "CountryID";
                drpBillingCountry_1.DataBind();
                drpBillingCountry_1.Items.Insert(0, new ListItem("United States", "205"));
                drpBillingCountry_1.Items.Insert(1, new ListItem("Canada", "174"));

                drpShippingCountry.DataSource = dt;
                drpShippingCountry.DataTextField = "CountryName";
                drpShippingCountry.DataValueField = "CountryID";
                drpShippingCountry.DataBind();
                drpShippingCountry.Items.Insert(0, new ListItem("United States", "205"));
                drpShippingCountry.Items.Insert(1, new ListItem("Canada", "174"));
            }
        }

        public ListItem FindByText(ListItemCollection items, string text, StringComparison comparisonType)
        {
            ListItem result = items.OfType<ListItem>().
                FirstOrDefault(_string => _string.Text.Equals(text, comparisonType));
            return result;
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


        private void LoadBillingState()
        {
            drpBillingState.Items.Clear();

            if (Region == Enums.Region.US)
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
            else if (Region == Enums.Region.CANADA)
            {
                addlistitemBillingState("", "Select a Province", "");
                addlistitemBillingState("AB", "Alberta", "Canada");
                addlistitemBillingState("BC", "British Columbia", "Canada");
                addlistitemBillingState("MB", "Manitoba", "Canada");
                addlistitemBillingState("NB", "New Brunswick", "Canada");
                addlistitemBillingState("NF", "New Foundland", "Canada");
                addlistitemBillingState("NS", "Nova Scotia", "Canada");
                addlistitemBillingState("ON", "Ontario", "Canada");
                addlistitemBillingState("PE", "Prince Edward Island", "Canada");
                addlistitemBillingState("QC", "Quebec", "Canada");
                addlistitemBillingState("SK", "Saskatchewan", "Canada");
                addlistitemBillingState("YT", "Yukon Territories", "Canada");
            }
        }

        private void LoadShippingState()
        {
            drpShippingState.Items.Clear();

            if (Region == Enums.Region.US)
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
            else if (Region == Enums.Region.CANADA)
            {
                addlistitemShippingState("", "Select a Province", "");
                addlistitemShippingState("AB", "Alberta", "Canada");
                addlistitemShippingState("BC", "British Columbia", "Canada");
                addlistitemShippingState("MB", "Manitoba", "Canada");
                addlistitemShippingState("NB", "New Brunswick", "Canada");
                addlistitemShippingState("NF", "New Foundland", "Canada");
                addlistitemShippingState("NS", "Nova Scotia", "Canada");
                addlistitemShippingState("ON", "Ontario", "Canada");
                addlistitemShippingState("PE", "Prince Edward Island", "Canada");
                addlistitemShippingState("QC", "Quebec", "Canada");
                addlistitemShippingState("SK", "Saskatchewan", "Canada");
                addlistitemShippingState("YT", "Yukon Territories", "Canada");
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Server.MapPath("../App_Themes/" + PubCode.ToUpper())))
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
            //ClientScriptManager csm = Page.ClientScript;
            //btnSecurePayment.Attributes.Add("onclick", "javascript:" + btnSecurePayment.ClientID + ".disabled=true;" + csm.GetPostBackEventReference(btnSecurePayment, ""));
            if (!IsPostBack)
            {
                string path = Request.Url.ToString();

                if (path.StartsWith("http:"))
                {
                    path = path.Replace("http:", "https:");
                    //Response.Redirect(path);
                }
            }

            this.objMagList = this.GetMagList();

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

                if (pub == null || pub.PubCode.Trim().Length == 0)
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Invalid Publication";
                    pnlPaidForm.Visible = false;
                    return;
                }

                Magazine m = this.objMagList.FirstOrDefault(x => x.PubCode.ToUpper() == PubCode.ToUpper());
                pnlContactInfo.Visible = m.ShowContact;
                pnlBillingAddress.Visible = m.ShowBillingAddress;
                pnlShippingAddress.Visible = m.ShowShippingAddress;
                pnlBillingToShipping.Visible = m.ShowShippingAddress && m.ShowBillingAddress;

                if (m.Header.Length > 0)
                    phHeader.Controls.Add(new LiteralControl(m.Header));
                else
                    phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));

                if (m.Footer.Length > 0)
                    phFooter.Controls.Add(new LiteralControl(m.Footer));
                else
                    phFooter.Controls.Add(new LiteralControl(pub.FooterHTML));
            }
            catch 
            {
                phError.Visible = true;
                lblErrorMessage.Text = "Invalid Publication";
                pnlPaidForm.Visible = false;
                return;
            }


            util = new KMPS_JF_Objects.Objects.Utilities();
            SetRegion(Country);

            if (!IsPostBack)
            {
                LoadCountries();

                pnlBillingState.Visible = (Region == Enums.Region.US || Region == Enums.Region.CANADA);
                pnlBillingStateInt.Visible = !pnlBillingState.Visible && pnlBillingAddress.Visible;

                pnlShippingState.Visible = (Region == Enums.Region.US || Region == Enums.Region.CANADA) && pnlShippingAddress.Visible;
                pnlShippingStateInt.Visible = !pnlShippingState.Visible && pnlShippingAddress.Visible;

                LoadBillingState();
                LoadShippingState();

                if (pnlBillingState.Visible && State.Trim().Length > 0)
                {
                    drpBillingState.Items.FindByValue(State.Trim().ToUpper()).Selected = true;
                    drpBillingState.Items[drpBillingState.SelectedIndex].Selected = true;
                }

                LoadFormFields();
                ShowHideSelect(true);

                Image imgCover = new Image();
                imgCover.ImageUrl = "~/App_Themes/" + pub.PubCode.ToUpper() + "/images/" + pub.MagCoverImage;
                phlMagCoverImage.Controls.Add(imgCover);


                pnlBillingState.Visible = Region == Enums.Region.US || Region == Enums.Region.CANADA;
                pnlBillingStateInt.Visible = !pnlBillingState.Visible;

                if (pnlBillingState.Visible && State.Trim().Length > 0)
                {
                    drpBillingState.ClearSelection();
                    drpBillingState.Items.FindByValue(State.Trim().ToUpper()).Selected = true;
                }
                else if (pnlBillingState.Visible && State.Trim().Length > 0)
                    txtBillingStateInt.Text = State.Trim();

                grdMagazines.DataSource = this.objMagList;
                grdMagazines.DataBind();
                updateGridView();

                if (pnlBillingStateInt.Visible)
                {
                    txtBillingStateInt.Text = StateInt.Trim();
                    txtBillingZip.Text = ForZip.Trim();
                }
                else
                    txtBillingZip.Text = Zip.Trim();
            }
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
                DataTable dt = DataFunctions.GetDataTable("communicator", cmd);

                if (dt.Rows.Count > 0)
                    return true;
            }

            return false;
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

        public void SetRegion(string CountryCode)
        {
            if (CountryCode.ToUpper() == "UNITED STATES OF AMERICA" || CountryCode.ToUpper() == "UNITED STATES" || CountryCode.ToUpper() == "US" || CountryCode.ToUpper() == "USA")
                Region = Enums.Region.US;
            else if (CountryCode.ToUpper() == "CANADA" || CountryCode.ToUpper() == "CA")
                Region = Enums.Region.CANADA;
            else if (CountryCode.ToUpper() == "UNITED STATES VIRGIN ISLANDS" || Country.ToUpper() == "PUERTO RICO" || Country.ToUpper() == "GUAM")
                Region = Enums.Region.ISLAND;
            else
                Region = Enums.Region.INTERNATIONAL;
        }

        private List<Magazine> GetMagList()
        {
            return Magazine.GetMagList(Server.MapPath("../xml/MagList.xml"), PubCode);
        }

        protected void grdMagazines_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                int term = Convert.ToInt32(((Label)e.Row.FindControl("lblTerm")).Text);
                DropDownList drpterm = (DropDownList)e.Row.FindControl("drpTerm");
                if (term == 1)
                {
                    drpterm.Items.Add(new ListItem("1 Year(s)", "1"));
                }
                else if (term == 2)
                {
                    drpterm.Items.Add(new ListItem("1 Year(s)", "1"));
                    drpterm.Items.Add(new ListItem("2 Year(s)", "2"));
                }
                else if (term == 3)
                {
                    drpterm.Items.Add(new ListItem("1 Year(s)", "1"));
                    drpterm.Items.Add(new ListItem("2 Year(s)", "2"));
                    drpterm.Items.Add(new ListItem("3 Year(s)", "3"));
                }
                
            }
        }

        private bool IsPrintEditionAvailable(string pubCode, string CountryName)
        {
            Publication pub = Publication.GetPublicationbyID(0, pubCode);
            string sql = "select pf.PFID from PubFormsForCountry pfc join PubForms pf on pf.PFID = pfc.PFID join Country c on c.CountryID = pfc.CountryID where c.CountryName = '" + drpBillingCountry.SelectedItem.Text.ToString() + "' and pf.PubID = " + pub.PubID.ToString() + " and pf.ShowPrint = 0";
            DataTable dt = DataFunctions.GetDataTable(sql);

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

                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                Label lblPubCode = (Label)row.FindControl("lblPubCode");
                Label lblMessage = (Label)row.FindControl("lblMessage");
                Label lblPriceText = (Label)row.FindControl("lblPriceText");

                DropDownList drpTerm = (DropDownList)row.FindControl("drpTerm");
                Label usLabel1 = (Label)row.FindControl("lblUsPrice1");
                Label usLabel2 = (Label)row.FindControl("lblUsPrice2");
                Label usLabel3 = (Label)row.FindControl("lblUsPrice3");

                Label canLabel1 = (Label)row.FindControl("lblCanPrice1");
                Label canLabel2 = (Label)row.FindControl("lblCanPrice2");
                Label canLabel3 = (Label)row.FindControl("lblCanPrice3");

                Label intLabel1 = (Label)row.FindControl("lblIntPrice1");
                Label intLabel2 = (Label)row.FindControl("lblIntPrice2");
                Label intLabel3 = (Label)row.FindControl("lblIntPrice3");

                bool isPrintVisible = true;

                try
                {
                    isPrintVisible = IsPrintEditionAvailable(lblPubCode.Text, drpBillingCountry.SelectedItem.Text);
                }
                catch
                {
                    isPrintVisible = false;
                }

                chkPrint.Visible = isPrintVisible;

                switch (Convert.ToInt32(drpTerm.SelectedItem.Value))
                {
                    case 1:
                        if (lblPubCode.Text.ToUpper() == "HME" || lblPubCode.Text.ToUpper() == "SSN")
                            usLabel1.Visible = (Region == Enums.Region.US || Region == Enums.Region.ISLAND) && isPrintVisible;
                        else
                            usLabel1.Visible = Region == Enums.Region.US && isPrintVisible;
                        canLabel1.Visible = (Region == Enums.Region.CANADA && isPrintVisible);
                        intLabel1.Visible = (Region == Enums.Region.INTERNATIONAL && isPrintVisible);
                        usLabel2.Visible = false;
                        canLabel2.Visible = false;
                        intLabel2.Visible = false;
                        usLabel3.Visible = false;
                        canLabel3.Visible = false;
                        intLabel3.Visible = false;
                        break;
                    case 2:
                        usLabel1.Visible = false;
                        canLabel1.Visible = false;
                        intLabel1.Visible = false;
                        if (lblPubCode.Text.ToUpper() == "HME" || lblPubCode.Text.ToUpper() == "SSN")
                            usLabel2.Visible = (Region == Enums.Region.US || Region == Enums.Region.ISLAND) && isPrintVisible;
                        else
                            usLabel2.Visible = Region == Enums.Region.US && isPrintVisible;
                        canLabel2.Visible = (Region == Enums.Region.CANADA && isPrintVisible);
                        intLabel2.Visible = (Region == Enums.Region.INTERNATIONAL && isPrintVisible);
                        usLabel3.Visible = false;
                        canLabel3.Visible = false;
                        intLabel3.Visible = false;
                        break;
                    case 3:
                        usLabel1.Visible = false;
                        canLabel1.Visible = false;
                        intLabel1.Visible = false;
                        usLabel2.Visible = false;
                        canLabel2.Visible = false;
                        intLabel2.Visible = false;
                        if (lblPubCode.Text.ToUpper() == "HME" || lblPubCode.Text.ToUpper() == "SSN")
                            usLabel3.Visible = (Region == Enums.Region.US || Region == Enums.Region.ISLAND) && isPrintVisible;
                        else
                            usLabel3.Visible = Region == Enums.Region.US && isPrintVisible;
                        canLabel3.Visible = (Region == Enums.Region.CANADA && isPrintVisible);
                        intLabel3.Visible = (Region == Enums.Region.INTERNATIONAL && isPrintVisible);
                        break;
                }

                if (!isPrintVisible)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Not Available";
                }

                if (lblPubCode.Text.ToUpper() == "HME" || lblPubCode.Text.ToUpper() == "SSN")
                    lblPriceText.Visible = true;
                else
                    lblPriceText.Visible = false;
            }
        }

        public void CalculatePrice()
        {
            foreach (GridViewRow row in grdMagazines.Rows)
            {
                Label totalLabel = (Label)row.FindControl("lblTotal");
                CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                DropDownList drpTerm = (DropDownList)row.FindControl("drpTerm");

                if (Region == Enums.Region.US)
                    ShowUSLabel(row, totalLabel, chkPrint, Convert.ToInt32(drpTerm.SelectedValue));
                else if (Region == Enums.Region.CANADA)
                    ShowCanLabel(row, totalLabel, chkPrint, Convert.ToInt32(drpTerm.SelectedValue));
                else
                    ShowIntLabel(row, totalLabel, chkPrint, Convert.ToInt32(drpTerm.SelectedValue));
            }
        }

        private void ShowIntLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint, int term)
        {
            Label intLabel = new Label();
            if(term==1)
                intLabel = (Label)row.FindControl("lblIntPrice1");
            else if (term == 2)
                intLabel = (Label)row.FindControl("lblIntPrice2");
            else if (term == 3)
                intLabel = (Label)row.FindControl("lblIntPrice3");

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
            if (lblTotAmount != null) 
            { 
                lblTotAmount.Text = String.Format("{0:0.00}", totalPrice); 
            }
        }

        private void ShowCanLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint, int term)
        {
            Label canLabel = new Label();
            if (term == 1)
                canLabel = (Label)row.FindControl("lblCanPrice1");
            else if (term == 2)
                canLabel = (Label)row.FindControl("lblCanPrice2");
            else if (term == 3)
                canLabel = (Label)row.FindControl("lblCanPrice3");

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
           
            if (lblTotAmount != null)
            { 
                lblTotAmount.Text = String.Format("{0:0.00}", totalPrice); 
            }
        }

        private void ShowUSLabel(GridViewRow row, Label totalLabel, CheckBox chkPrint, int term)
        {
            Label usLabel = new Label();
            if (term == 1)
                usLabel = (Label)row.FindControl("lblUsPrice1");
            else if (term == 2)
                usLabel = (Label)row.FindControl("lblUsPrice2");
            else if (term == 3)
                usLabel = (Label)row.FindControl("lblUsPrice3");

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
            SetRegion(drpBillingCountry.SelectedItem.Text);
            CalculatePrice();
        }

        public double GetTotal()
        {
            Label lblTotAmount = (Label)grdMagazines.FooterRow.FindControl("lblTotalAmount");
            return double.Parse(lblTotAmount.Text);
        }

        #region State Dropdown

        protected void drpTerm_SelectedIndexChanged(Object sender, EventArgs e)
        {            
            SetRegion(drpBillingCountry.SelectedItem.Text);
            updateGridView();
            CalculatePrice();
            
        }

        protected void drpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpBillingCountry_1.ClearSelection();
            drpBillingCountry_1.Items.FindByValue(drpBillingCountry.SelectedItem.Value).Selected = true;
            SetRegion(drpBillingCountry.SelectedItem.Text.ToUpper());

            drpShippingCountry.ClearSelection();
            drpShippingCountry.Items.FindByValue(drpBillingCountry.SelectedItem.Value).Selected = true;

            if (Region == Enums.Region.US || Region == Enums.Region.CANADA)
            {
                pnlBillingState.Visible = true;
                pnlBillingStateInt.Visible = false;
                LoadBillingState();

                pnlShippingState.Visible = true;
                pnlShippingStateInt.Visible = false;
                LoadShippingState();
            }
            else
            {
                pnlBillingState.Visible = false;
                pnlBillingStateInt.Visible = true;

                pnlShippingState.Visible = false;
                pnlShippingStateInt.Visible = true;
            }

            grdMagazines.DataSource = this.objMagList;
            grdMagazines.DataBind();
            updateGridView();
        }

        protected void drpShippingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRegion(drpShippingCountry.SelectedItem.Text.ToUpper());

            if (Region == Enums.Region.US || Region == Enums.Region.CANADA)
            {
                pnlShippingState.Visible = true;
                pnlShippingStateInt.Visible = false;
                LoadShippingState();
            }
            else
            {
                pnlShippingState.Visible = false;
                pnlShippingStateInt.Visible = true;
            }

            SetRegion(drpBillingCountry.SelectedItem.Text.ToUpper());
        }

        protected void btnCopyBillingtoShipping_Click(object sender, EventArgs e)
        {
            txtShippingAddress1.Text = txtBillingAddress1.Text;
            txtShippingAddress2.Text = txtBillingAddress2.Text;
            txtShippingCity.Text = txtBillingCity.Text;
            txtShippingZip.Text = txtBillingZip.Text;

            SetRegion(drpBillingCountry.SelectedItem.Text.ToUpper());

            if (Region == Enums.Region.US || Region == Enums.Region.CANADA)
            {
                pnlShippingState.Visible = true;
                pnlShippingStateInt.Visible = false;
                LoadShippingState();
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

        private void addlistitem(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            drpBillingState.Items.Add(item);
        }
        #endregion

        private bool IsCacheClear()
        {
            try
            {
                SqlCommand cmdGetCacheClear = new SqlCommand("select IsCacheClear from Publications where PubCode = @PubCode");
                cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
                return Convert.ToBoolean(DataFunctions.ExecuteScalar("", cmdGetCacheClear).ToString());
            }
            catch { return false; }
        }

        private void UpdateCacheClear()
        {
            try
            {
                SqlCommand cmdGetCacheClear = new SqlCommand("update Publications set IsCacheClear = 0 where PubCode = @PubCode");
                cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
                DataFunctions.Execute(cmdGetCacheClear);
            }
            catch { }
        }

        protected void email_TextChanged(object sender, EventArgs e)
        {
            if (CheckPaidProductsEmailValidation(email.Text))
            {
                lblEmailValidationText.Text = pub.RepeatEmailsMessage.Replace("%%EmailAddress%%", email.Text.Trim());
                ModalPopupExtenderEmailValidation.Show();
                pnlEmailValidationPopup.Style.Add("display", "block");
                email.Text = "";
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
                #region Page Valid

                string EncodedResponse = Request.Form["g-Recaptcha-Response"];

                if (!ReCaptchaClass.Validate(EncodedResponse))
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Invalid Captcha";
                    return;
                }

                foreach (GridViewRow row in grdMagazines.Rows)
                {
                    Item item = new Item();
                    Label groupID = (Label)row.FindControl("lblGroupID");
                    Label custID = (Label)row.FindControl("lblcustID");
                    Label lblPubCode = (Label)row.FindControl("lblPubCode");
                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    Label lblTitle = (Label)row.FindControl("lblTitle");
                    double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));

                    if (string.IsNullOrEmpty(PubCode))
                        PubCode = lblPubCode.Text.Trim();

                    if (chkPrint.Checked == true)
                    {
                        selectioncount++;

                        if (chkPrint.Checked) { sb.Append("user_DEMO7=C"); }
                        sb.Append("&c=" + custID.Text);
                        sb.Append("&f=html");
                        sb.Append("&user_PUBLICATIONCODE=" + lblPubCode.Text.ToString());
                        sb.Append("&g=" + groupID.Text);
                        sb.Append("&user_PaymentStatus=pending");

                        //Collect contact information                        
                        if (pnlContactInfo.Visible)
                        {
                            sb.Append("&fn=" + first.Text);
                            sb.Append("&ln=" + last.Text);
                            sb.Append("&compname=" + company.Text);
                            sb.Append("&ph=" + phone.Text);
                            sb.Append("&fax=" + fax.Text);
                            sb.Append("&e=" + email.Text);
                        }
                        else
                        {
                            sb.Append("&fn=" + FirstName);
                            sb.Append("&ln=" + LastName);
                            sb.Append("&compname=" + Company);
                            sb.Append("&ph=" + Phone);
                            sb.Append("&fax=" + Fax);
                            sb.Append("&e=" + Email);
                        }

                        //collect billing address
                        if (pnlShippingAddress.Visible)
                        {
                            sb.Append("&adr=" + txtShippingAddress1.Text);
                            sb.Append("&adr2=" + txtShippingAddress2.Text);
                            sb.Append("&city=" + txtShippingCity.Text);

                            if (Country == "UNITED STATES OF AMERICA" || Country == "UNITED STATES" || Country == "CANADA" && pnlShippingState.Visible)
                                sb.Append("&state=" + drpShippingState.Text);
                            else
                                sb.Append("&STATE_INT=" + txtShippingStateInt.Text);

                            sb.Append("&zc=" + txtShippingZip.Text);
                            sb.Append("&ctry=" + drpShippingCountry.SelectedItem.Text);
                        }

                        item.ItemCode = lblPubCode.Text;
                        item.ItemName = lblTitle.Text;
                        item.ItemAmount = totalAmt.ToString();
                        item.ItemQty = "1";
                        item.GroupID = int.Parse(groupID.Text);
                        item.CustID = int.Parse(custID.Text);
                        itemList.Add(item);

                    } //end if
                } // end foreach

                if (selectioncount == 0)
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Please check the box to order your paid subscription.";
                    return;
                }

                #region AUTHORIZE CREDIT CARD

                if (String.IsNullOrEmpty(TransactionID))
                {
                    if (!ValidateCreditCard())
                    {
                        phError.Visible = true;
                        string msg = ccProcessor.ValidatedMessage;
                        if (msg.Trim().Length > 0)
                            lblErrorMessage.Text = msg;
                        else
                            lblErrorMessage.Text = "The transaction cannot be processed.";
                        return;
                    }
                    else
                    {

                        TransactionID = ccProcessor.TransactionId.ToString();
                    }

                }

                #endregion

                #region COLLECT TRANSACTIONAL UDF

                foreach (GridViewRow row in grdMagazines.Rows)
                {
                    sb = new StringBuilder();

                    Label groupID = (Label)row.FindControl("lblGroupID");
                    Label custID = (Label)row.FindControl("lblcustID");
                    Label usLabel = (Label)row.FindControl("lblUsPrice");
                    Label canLabel = (Label)row.FindControl("lblCanPrice");
                    Label intLabel = (Label)row.FindControl("lblIntPrice");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    DropDownList drpTerm = (DropDownList)row.FindControl("drpTerm");
                    double totalAmt = double.Parse(lblTotal.Text.TrimStart(new char[] { '$' }));
                    CheckBox chkPrint = (CheckBox)row.FindControl("chkPrint");

                    if (chkPrint.Checked == true)
                    {
                        sb.Append("g=" + groupID.Text);
                        sb.Append("&f=html");
                        sb.Append("&c=" + custID.Text);
                        sb.Append("&user_PaymentStatus=paid");
                        sb.Append("&user_PAIDorFREE=PAID");

                        //Collect contact information                        
                        if (pnlContactInfo.Visible)
                        {
                            sb.Append("&e=" + email.Text);
                        }
                        else
                        {
                            sb.Append("&e=" + Email);
                        }

                        string[] cardHolderName = user_CardHolderName.Text.Split(new char[] { ' ' });
                        string fName = "";
                        string lName = "";

                        try
                        {
                            if (cardHolderName.Length == 2)
                            {
                                fName = cardHolderName[0];
                                lName = cardHolderName[1];
                            }
                            else if (cardHolderName.Length == 3)
                            {
                                fName = cardHolderName[0];
                                lName = cardHolderName[2];
                            }
                            else
                            {
                                fName = user_CardHolderName.Text;
                                lName = string.Empty;
                            }
                        }
                        catch
                        {
                            fName = user_CardHolderName.Text;
                            lName = string.Empty;
                        }

                        SaveUDF(Convert.ToInt32(groupID.Text), "t_Term", "t_Term");
                        SaveUDF(Convert.ToInt32(groupID.Text), "t_TermStartDate", "t_TermStartDate");
                        SaveUDF(Convert.ToInt32(groupID.Text), "t_TermEndDate", "t_TermEndDate");

                        sb.Append("&user_t_FullName=" + user_CardHolderName.Text);
                        sb.Append("&user_t_FirstName=" + fName);
                        sb.Append("&user_t_LastName=" + lName);
                        sb.Append("&user_t_Term=" + drpTerm.SelectedValue);
                        sb.Append("&user_t_TermStartDate=" + DateTime.Now.ToString("MM/dd/yyyy"));
                        sb.Append("&user_t_TermEndDate=" + DateTime.Now.AddYears(Convert.ToInt32(drpTerm.SelectedValue)).ToString("MM/dd/yyyy"));
                        sb.Append("&user_PaymentStatus=paid");
                        sb.Append("&user_PAIDorFREE=PAID");


                        sb.Append("&user_t_CardType=" + user_CCType.SelectedItem.Value);
                        sb.Append("&user_t_CardNumber=" + "************" + user_CCNumber.Text.Substring(user_CCNumber.Text.Trim().Length - 4, 4));
                        sb.Append(String.Format("&user_t_ExpirationDate={0}/{1}", user_Exp_Month.SelectedItem.Value, user_Exp_Year.SelectedItem.Value));
                        sb.Append("&user_t_transdate=" + DateTime.Now.ToString("MM/dd/yyyy"));
                        sb.Append("&user_t_AmountPaid=" + String.Format("{0:0.00}", totalAmt));
                        sb.Append("&user_t_TransactionID=" + TransactionID.ToString());

                        //collect billing info
                        if (pnlBillingAddress.Visible)
                        {
                            sb.Append("&user_t_Street=" + txtBillingAddress1.Text);
                            sb.Append("&user_t_Street2=" + txtBillingAddress2.Text);
                            sb.Append("&user_t_City=" + txtBillingCity.Text);

                            if ((Region == Enums.Region.CANADA || Region == Enums.Region.US))
                                sb.Append("&user_t_State=" + drpBillingState.SelectedItem.Value);

                            sb.Append("&user_t_Zip=" + txtBillingZip.Text);
                            sb.Append("&user_t_Country=" + drpBillingCountry.SelectedItem.Text);
                        }


                        //collect Shipping Info
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
                    }

                } //end for loop

                #endregion

                try
                {
                    foreach (Item i in itemList)
                    {
                        HttpPostAutoSubscription(i.ItemCode);
                    }
                }
                catch 
                {

                }

                #region POST TO ECN
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

                if (Request.QueryString != null)
                    redirectstatus = ECNUtils.ECNHttpPost(email.Text, PubCode, Request.QueryString.ToString(), browserInfo) && ECNUtils.ECNHttpPost(email.Text, PubCode, sb.ToString(), browserInfo);
                else
                    redirectstatus = ECNUtils.ECNHttpPost(email.Text, PubCode, sb.ToString(), browserInfo);

                #endregion

                if (!redirectstatus)
                    redirectstatusall = redirectstatus;

                if (redirectstatusall)
                {
                    TransactionID = string.Empty;
                    SendResponseEmail();
                    Response.Redirect("../Forms/Thankyou.aspx?ispaid=1&pubcode=" + PubCode);
                }
                else
                {
                    phError.Visible = true;
                    lblErrorMessage.Text = "Error in posting the data. Please try again!!";
                    return;
                }

            #endregion
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

        private void HttpPostAutoSubscription(string pubcode)
        {
            int AutoSubGroupID = -1;
            try
            {
                HttpBrowserCapabilities browser = Request.Browser;
                string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);
                Publication pub = Publication.GetPublicationbyID(0, pubcode);

                //string[] autoSubsPost = queryString.Split('&');
                #region set up post field data
                Dictionary<string, string> postFields = new Dictionary<string, string>();

                postFields = BuildProfile();

                DataTable UDFs = GetUDFValues(pub.ECNDefaultGroupID);
                if (UDFs != null && UDFs.Rows.Count > 0)
                {
                    foreach (DataRow dr in UDFs.Rows)
                    {
                        postFields.Add("user_" + dr["ShortName"].ToString(), dr["DataValue"].ToString());
                    }
                }
                #endregion


                if (pub != null)
                {
                    //Loop through auto sub groups and post for each one
                    Dictionary<int, int> autoSubGroups = Publication.GetAutoSubscriptions(pub.PubID);
                    foreach (KeyValuePair<int, int> kvp in autoSubGroups)
                    {
                        try
                        {
                            if (postFields.ContainsKey("g"))
                                postFields["g"] = kvp.Key.ToString();
                            else
                                postFields.Add("g", kvp.Key.ToString());

                            if (postFields.ContainsKey("c"))
                                postFields["c"] = kvp.Value.ToString();
                            else
                                postFields.Add("c", kvp.Value.ToString());
                        }
                        catch { }
                        AutoSubGroupID = kvp.Key;
                        ECNUtils.ECNHttpPost(email.Text, pub.PubCode, GetQueryStringFromDictionary(postFields), browserInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error when posting to AutoSub groups");
                sb.AppendLine("Email:" + email.Text);
                sb.AppendLine("FName:" + FirstName);
                sb.AppendLine("LName:" + LastName);                
                sb.AppendLine("Country:" + Country);
                sb.AppendLine("AutoSubGroup:" + AutoSubGroupID.ToString());

                sb.AppendLine("");

                sb.AppendLine(ex.StackTrace);

                KMPS_JF_Objects.Objects.Utilities.SendMail(sb.ToString());
            }
        }

        private Dictionary<string,string> BuildProfile()
        {
            Dictionary<string, string> profile = new Dictionary<string, string>();
            profile.Add("fn", first.Text);
            profile.Add("ln", last.Text);
            profile.Add("compname", company.Text);
            profile.Add("adr", txtShippingAddress1.Text);
            profile.Add("adr2", txtShippingAddress2.Text);
            profile.Add("city", txtShippingCity.Text);
            profile.Add("state", txtShippingStateInt.Visible ? txtShippingStateInt.Text : drpShippingState.SelectedValue.ToString());
            profile.Add("ctry", drpShippingCountry.SelectedValue.ToString());
            profile.Add("ph", phone.Text);
            profile.Add("fax", fax.Text);
            profile.Add("e", email.Text);
            profile.Add("f", "html");
            
            return profile;
        }

        private string GetQueryStringFromDictionary(Dictionary<string, string> postFields)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in postFields)
                sb.Append(kvp.Key + "=" + kvp.Value + "&");

            return sb.ToString().TrimEnd('&');
        }


        private DataTable GetUDFValues(int groupID)
        {
            string sql = @"SELECT	gdf.ShortName, edv.DataValue 
                            FROM EmailDataValues edv WITH(NOLOCK)  
                            join  GroupDatafields gdf WITH(NOLOCK)  on edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
                            join Emails e with(nolock) on edv.EmailID = e.emailID
                            WHERE e.EmailAddress = @emailAddress and gdf.GroupID = @groupID and isnull(gdf.DatafieldSetID,0) = 0";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@emailAddress", email.Text);
            cmd.Parameters.AddWithValue("@GroupID", groupID.ToString());

            DataTable dtUDFs = DataFunctions.GetDataTable("communicator", cmd);

            return dtUDFs;
        }

        private static int SaveUDF(int groupID, string shortname, string longname)
        {
            bool IsPublic = false;
            shortname = DataFunctions.CleanString(shortname.Trim());
            longname = DataFunctions.CleanString(longname.Trim());

            string DatafieldSetID_query = "select DatafieldSetID from DatafieldSets where Name='FormBuilderTransaction' AND groupID =" + groupID;
            int DatafieldSetID = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", DatafieldSetID_query));

            string sqlcheck = "SELECT COUNT(groupdatafieldsID) FROM GROUPDATAFIELDS WHERE shortname='" + shortname + "'  and IsDeleted=0 AND groupID =" + groupID + " and DatafieldSetID = " + DatafieldSetID;
            int alreadyexist = Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlcheck));

            if (alreadyexist == 0)
            {
                string sqlquery = " INSERT INTO GroupDataFields ( GroupID, ShortName, LongName, IsPublic, DatafieldSetID, IsDeleted) VALUES ( " + groupID + ", '" + shortname + "', '" + longname + "'," + (IsPublic ? "'Y'" : "'N'") + ", " + DatafieldSetID + ",0);select @@IDENTITY ";
                return Convert.ToInt32(DataFunctions.ExecuteScalar("communicator", sqlquery).ToString());
            }
            return 0;
        }


        private void SendResponseEmail()
        {
            if (pub.PaidResponseEmail.Trim().Length > 0)
            {
                string adminEmailbody = ReplaceCodeSnippets(pub.PaidResponseEmail);
                ecn.communicator.classes.EmailFunctions emailFunctions = new ecn.communicator.classes.EmailFunctions();

                if (pub.PaidPageFromEmail.Trim().Length > 0 && pub.PaidFormResponseEmailSubject.Trim().Length > 0)
                    emailFunctions.SimpleSend(email.Text, pub.PaidPageFromName + " <" + pub.PaidPageFromEmail + ">", pub.PaidFormResponseEmailSubject, adminEmailbody);
                else
                    emailFunctions.SimpleSend(email.Text, "info@knowledgemarketing.com", "Receipt/Purchase Confirmation", adminEmailbody);
            }
        }

        private string ReplaceCodeSnippets(string emailBody)
        {
            Item orderItem = itemList[0];
            string body = Regex.Replace(emailBody, "%%REGDATE%%", DateTime.Now.ToShortDateString(), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EXPIRATIONDATE%%", DateTime.Now.AddDays(Convert.ToInt32(orderItem.ItemQty) * 365).ToShortDateString(), RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EXPIRATIONDATEYYYYMMDD%%", DateTime.Now.AddDays(Convert.ToInt32(orderItem.ItemQty) * 365).ToString("yyyy/MM/dd"), RegexOptions.IgnoreCase);
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

        private string GetPasswordByCustomerIDAndEmailAddress()
        {
            string password = "";

            try
            {
                SqlCommand cmdGetPassword = new SqlCommand("select top 1 Password from Emails where CustomerID = @CustomerID and EmailAddress = @EmailAddress");
                cmdGetPassword.Parameters.AddWithValue("@CustomerID", pub.ECNCustomerID);
                cmdGetPassword.Parameters.AddWithValue("@EmailAddress", email.Text.Trim());
                password = DataFunctions.ExecuteScalar("communicator", cmdGetPassword).ToString();
                return password;
            }
            catch { return ""; } 
        }

        private string formatPhoneNumber(string phone)
        {
            if (!phone.Contains("-"))
                return phone.Substring(0, 3) + "-" + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);  
            else
                return phone;
        }

        private bool ValidateCreditCard()
        {
            try
            {
                Magazine mg = this.objMagList.FirstOrDefault(x => x.PubCode.ToUpper() == PubCode.ToUpper());

                if (mg.PaymentGateway == ccProcessors.AuthorizeNet)
                    this.ccProcessor = new ccAuthorizeNet();
                else
                    this.ccProcessor = new ccPayPal();

                //collect credit card details
                ccProcessor.CreditCardNumber = user_CCNumber.Text;
                ccProcessor.CreditCardExpirationMonth = user_Exp_Month.SelectedItem.Text;
                ccProcessor.CreditCardExpirationYear = user_Exp_Year.SelectedItem.Text;
                ccProcessor.CreditCardExpiration = user_Exp_Month.Text + "-" + user_Exp_Year.Text;
                ccProcessor.CreditCardExpirationFullYear = user_Exp_Year.SelectedItem.Text;
                ccProcessor.OrderAmount = Convert.ToDecimal(this.GetTotal());
                ccProcessor.SecurityCode = user_CCVerfication.Text;
                ccProcessor.CreditCardType = (CreditCardType)Enum.Parse(typeof(CreditCardType), user_CCType.SelectedItem.Value);
                ccProcessor.IPAddress = Request.UserHostAddress;

                string[] cardHolderName = user_CardHolderName.Text.Split(new char[] { ' ' });
                try
                {
                    if (cardHolderName.Length == 2)
                    {
                        ccProcessor.Firstname = cardHolderName[0];
                        ccProcessor.Lastname = cardHolderName[1];
                    }
                    else if (cardHolderName.Length == 3)
                    {
                        ccProcessor.Firstname = cardHolderName[0];
                        ccProcessor.Lastname = cardHolderName[2];
                    }
                    else
                    {
                        ccProcessor.Firstname = user_CardHolderName.Text;
                        ccProcessor.Lastname = string.Empty;
                    }
                }
                catch
                {
                    ccProcessor.Firstname = user_CardHolderName.Text;
                    ccProcessor.Lastname = string.Empty;
                }

                //collect billing address
                string countrCode = string.Empty;
                ccProcessor.UseTestTransaction = true;
                ccProcessor.Company = company.Text;
                ccProcessor.City = txtBillingCity.Text;
                ccProcessor.Address1 = txtBillingAddress1.Text;
                ccProcessor.Address2 = txtBillingAddress2.Text;
                ccProcessor.Zip = txtBillingZip.Text;
                ccProcessor.Phone = phone.Text;
                ccProcessor.Email = Server.UrlDecode(email.Text);
                ccProcessor.Invoice = PromoCode;
                ccProcessor.Fax = fax.Text;

                SetRegion(drpBillingCountry.SelectedItem.Text);
                if (Region == Enums.Region.US)
                {
                    countrCode = "US";
                    ccProcessor.State = drpBillingState.SelectedItem.Value;
                }
                else if (Region == Enums.Region.CANADA)
                {
                    countrCode = "CA";
                    ccProcessor.State = drpBillingState.SelectedItem.Value;
                }
                else
                {
                    countrCode = drpBillingCountry.SelectedItem.Text.ToUpper();
                    ccProcessor.State = txtBillingStateInt.Text;
                }

                ccProcessor.Country = countrCode;

                //collect shipping Address

                if (pnlShippingAddress.Visible)
                {
                    ccProcessor.ShippingAddressFirstName = first.Text.Trim();
                    ccProcessor.ShippingAddressLastName = last.Text.Trim();
                    ccProcessor.ShippingAddress1 = txtShippingAddress1.Text.Trim();
                    ccProcessor.ShippingAddress2 = txtShippingAddress2.Text.Trim();
                    ccProcessor.ShippingCity = txtShippingCity.Text.Trim();

                    if (pnlShippingState.Visible)
                        ccProcessor.ShippingState = drpShippingState.SelectedItem.Value;
                    else
                        ccProcessor.ShippingState = txtShippingStateInt.Text.Trim();

                    ccProcessor.ShippingZip = txtShippingZip.Text;

                    SetRegion(drpShippingCountry.SelectedItem.Text);
                    if (Region == Enums.Region.US)
                        countrCode = "US";
                    else if (Region == Enums.Region.CANADA)
                        countrCode = "CA";
                    else
                        countrCode = drpShippingCountry.SelectedItem.Text.ToUpper();

                    ccProcessor.ShippingCountry = countrCode;
                }    //else copy billing address to shipping
                else
                {
                    ccProcessor.ShippingAddressFirstName = first.Text.Trim();
                    ccProcessor.ShippingAddressLastName = last.Text.Trim();
                    ccProcessor.ShippingAddress1 = txtBillingAddress1.Text.Trim();
                    ccProcessor.ShippingAddress2 = txtBillingAddress2.Text.Trim();
                    ccProcessor.ShippingCity = txtBillingCity.Text.Trim();

                    if (pnlBillingState.Visible)
                        ccProcessor.ShippingState = drpBillingState.SelectedItem.Value;
                    else
                        ccProcessor.ShippingState = txtBillingStateInt.Text.Trim();

                    ccProcessor.ShippingZip = txtBillingZip.Text;

                    SetRegion(drpBillingCountry.SelectedItem.Text);
                    if (Region == Enums.Region.US)
                        countrCode = "US";
                    else if (Region == Enums.Region.CANADA)
                        countrCode = "CA";
                    else
                        countrCode = drpBillingCountry.SelectedItem.Text.ToUpper();

                    ccProcessor.ShippingCountry = countrCode;
                }

                ccProcessor.OrderDescription = mg.Description;
                ccProcessor.itemList = this.itemList;
                return ccProcessor.ValidateCard(pub);
            }
            catch (Exception ex)
            {
                string emailMsg = "Error when Processing Credit Card..<br /><br />";
                emailMsg += "<b>Error Response:</b>" + ccProcessor.ValidatedResult + "<br /><br />";
                emailMsg += "<b>Exception Details:</b>" + ex.Message;
                KMPS_JF_Objects.Objects.Utilities.SendMail(emailMsg);
                return false;
            }
        }
        
        private void LoadFormFields()
        {
            first.Text = FirstName;
            last.Text = LastName;
            company.Text = Company;
            txtBillingAddress1.Text = Address1;
            txtBillingAddress2.Text = Address2;
            txtBillingCity.Text = City;
            phone.Text = Phone;
            fax.Text = Fax;
            email.Text = Server.UrlDecode(Email);
            email.ReadOnly = Email.Trim().Length > 0 ? true : false;

            if (pnlBillingState.Visible)
            {
                try
                {
                    drpBillingState.ClearSelection();
                    drpBillingState.Items.FindByValue(State.Trim().ToUpper()).Selected = true;
                }
                catch { drpBillingState.Items[0].Selected = true; }
            }
            else if (pnlBillingStateInt.Visible)
                txtBillingStateInt.Text = State.Trim();

            try
            {
                drpBillingCountry.ClearSelection();
                drpBillingCountry_1.ClearSelection();
                this.FindByText(drpBillingCountry.Items, Country, StringComparison.OrdinalIgnoreCase).Selected = true;
                this.FindByText(drpBillingCountry_1.Items, Country, StringComparison.OrdinalIgnoreCase).Selected = true;
            }
            catch { }

            try
            {
                drpShippingCountry.ClearSelection();
                FindByText(drpShippingCountry.Items, Country, StringComparison.OrdinalIgnoreCase).Selected = true;
            }
            catch { }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //clear credit card fields
            user_CardHolderName.Text = "";
            user_CCType.ClearSelection();
            user_CCNumber.Text = "";
            user_Exp_Month.ClearSelection();
            user_Exp_Year.ClearSelection();
            user_CCVerfication.Text = "";


            //clear billing address field

            txtBillingAddress1.Text = "";
            txtBillingAddress2.Text = "";
            txtBillingCity.Text = "";
            drpBillingState.ClearSelection();
            txtBillingStateInt.Text = "";
            txtBillingZip.Text = "";
            drpBillingCountry.ClearSelection();
            drpBillingCountry_1.ClearSelection();


            //clear shipping address            
            txtShippingAddress1.Text = "";
            txtShippingAddress2.Text = "";
            txtShippingCity.Text = "";
            drpShippingState.ClearSelection();
            txtShippingStateInt.Text = "";
            txtShippingZip.Text = "";
            drpShippingCountry.ClearSelection();

            SetRegion(drpBillingCountry.SelectedItem.Text);
            LoadBillingState();

            SetRegion(drpShippingCountry.SelectedItem.Text);

            if (pub.ShowShippingAddress)
                LoadShippingState();

            //clear contact information

            first.Text = "";
            last.Text = "";
            company.Text = "";
            phone.Text = "";
            fax.Text = "";

            phError.Visible = false;

            updateGridView();

            foreach (GridViewRow row in grdMagazines.Rows)
            {
                CheckBox chPrint = (CheckBox)row.FindControl("chkPrint");
                chPrint.Checked = false;
            }
        }
    }
}
