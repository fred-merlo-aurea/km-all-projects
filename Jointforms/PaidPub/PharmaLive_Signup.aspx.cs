using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using PayPal.Payments.Common;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;

using ecn.common.classes;
using ecn.communicator.classes;

namespace PaidPub
{
    public partial class PharmaLive_Signup : System.Web.UI.Page
    {
        DataTable dtSubscribednewsletters = new DataTable();
        static ArrayList subscribeGroups = new ArrayList();
        string SubscriptionGroupIDs = string.Empty;
        Emails CurrentEmail = null;
        //double totalactualprice = 0;
        //double totaldiscountprice = 0; 
        //double totalprice = 0;

        private int EmailID
        {
            get
            {
                if (ViewState["EmailID"] == null)
                    return 0;
                else
                    return Convert.ToInt32(ViewState["EmailID"]);
            }

            set { ViewState["EmailID"] = value; }
        }

        private string PromoCode
        {
            get
            {
                try
                {
                    return Request.QueryString["pc"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        private string getBlastID
        {
            get
            {
                try
                {
                    if (Request.QueryString["bid"].Trim().ToString() != "")
                    {
                        return Request.QueryString["bid"].Trim().ToString();
                    }
                    else
                    {
                        return Request.Cookies["bid"].Value;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        //private string TransactionID
        //{
        //    get
        //    {
        //        if (ViewState["TransactionID"] == null)
        //            return string.Empty;
        //        else
        //            return ViewState["TransactionID"].ToString();
        //    }

        //    set { ViewState["TransactionID"] = value; }
        //}

        private double PromoDiscount
        {
            get
            {
                if (ViewState["PromoDiscount"] == null)
                    return 0;
                else
                    return (double)ViewState["PromoDiscount"];
            }

            set { ViewState["PromoDiscount"] = value; }
        }

        private string EmailSource
        {
            get
            {
                try
                {
                    return Request.QueryString["EmailSource"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }


        protected void Page_Load(object sender, EventArgs eargs)
        {
            //btnSubmit.Attributes.Add("OnClick", "javascript:return validateForm();");
            phError.Visible = false;

            if (!IsPostBack)
            {
                pnlStep1.Visible = true;
                pnlStep2.Visible = false;
                //btnprevious.Visible = false;
                loadstate();

                if (EmailSource.ToLower() == "f2f")
                {
                    txtpromo.Text = String.Format("X{0}{1}04", DateTime.Now.Year.ToString().Substring(3), DateTime.Now.Month.ToString("d2"));
                }
                else
                {
                    if (PromoCode == string.Empty || PromoCode == "")
                    {
                        txtpromo.Text = String.Format("W{0}{1}06", DateTime.Now.Year.ToString().Substring(3), DateTime.Now.Month.ToString("d2"));
                    }
                    else
                        txtpromo.Text = PromoCode;
                }

                if (PromoCode != string.Empty)
                {
                    DataTable dtPC = DataFunctions.GetDataTable("select * from CANON_PAIDPUB_Promotions where CustomerID = " + lblcustomerID.Text.ToString() + " and code = '" + PromoCode + "'", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

                    if (dtPC.Rows.Count > 0)
                    {
                        PromoDiscount = Convert.ToDouble(dtPC.Rows[0]["Discount"].ToString());
                    }
                }
            }
        }

        protected void loadform(string emailaddress)
        {
            DataTable dtEmail = DataFunctions.GetDataTable("select e.EmailID,isnull(EmailAddress	,'') as EmailAddress,isnull(FirstName,'') as FirstName,isnull(LastName,'') as LastName,isnull(Title,'') as Title,isnull(Company,'') as Company,isnull(Address,'') as Address,isnull(Address2,'') as Address2,isnull(City,'') as City,isnull(State,'') as State,isnull(Zip,'') as Zip,isnull(Country,'') as Country,isnull(Voice,'') as Voice,isnull(Fax,'') as Fax from emails e where e.emailaddress = '" + emailaddress.Replace("'", "''") + "' and customerID = " + lblcustomerID.Text);

            if (dtEmail.Rows.Count > 0)
            {
                EmailID = Convert.ToInt32(dtEmail.Rows[0]["EmailID"]);

                txtemail.Text = emailaddress;

                if (dtEmail.Rows[0]["FirstName"].ToString() != string.Empty)
                    txtfirstname.Text = dtEmail.Rows[0]["FirstName"].ToString();

                if (dtEmail.Rows[0]["LastName"].ToString() != string.Empty)
                    txtlastname.Text = dtEmail.Rows[0]["LastName"].ToString();

                if (dtEmail.Rows[0]["Title"].ToString() != string.Empty)
                    txttitle.Text = dtEmail.Rows[0]["Title"].ToString();

                if (dtEmail.Rows[0]["Company"].ToString() != string.Empty)
                    txtcompany.Text = dtEmail.Rows[0]["Company"].ToString();
                if (dtEmail.Rows[0]["Address"].ToString() != string.Empty)
                    txtaddress.Text = dtEmail.Rows[0]["Address"].ToString();
                if (dtEmail.Rows[0]["Address2"].ToString() != string.Empty)
                    txtaddress2.Text = dtEmail.Rows[0]["Address2"].ToString();
                if (dtEmail.Rows[0]["City"].ToString() != string.Empty)
                    txtcity.Text = dtEmail.Rows[0]["City"].ToString();
                try
                {
                    if (dtEmail.Rows[0]["State"].ToString() != string.Empty)
                    {
                        drpstate.ClearSelection();
                        drpstate.Items.FindByValue(dtEmail.Rows[0]["State"].ToString()).Selected = true;
                    }
                }
                catch { }

                if (dtEmail.Rows[0]["Zip"].ToString() != string.Empty)
                    txtzip.Text = dtEmail.Rows[0]["Zip"].ToString();
                try
                {
                    if (dtEmail.Rows[0]["Country"].ToString() != string.Empty)
                    {
                        drpcountry.ClearSelection();
                        drpcountry.Items.FindByValue(dtEmail.Rows[0]["Country"].ToString()).Selected = true;
                    }
                }
                catch { }

                if (dtEmail.Rows[0]["Voice"].ToString() != string.Empty)
                    txtphone.Text = dtEmail.Rows[0]["Voice"].ToString();

                if (dtEmail.Rows[0]["Fax"].ToString() != string.Empty)
                    txtfax.Text = dtEmail.Rows[0]["Fax"].ToString();

                loadUDFValues();
                loadSubscribedeNewsletters();

                txtemail.Enabled = false;
            }

            //btnSubmit.Enabled = true;
        }
        private void loadUDFValues()
        {
            DataTable dtUDF = DataFunctions.GetDataTable("select distinct shortname, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + EmailID + " and groupID in (" + getpaidSubscriptiongroups() + ")");

            foreach (DataRow dr in dtUDF.Rows)
            {
                try
                {
                    if (dr["shortname"].ToString().ToLower() == "business")
                    {
                        try
                        {
                            user_Business.ClearSelection();
                            user_Business.Items.FindByValue(dr["datavalue"].ToString()).Selected = true;
                        }
                        catch { }
                    }
                    else if (dr["shortname"].ToString().ToLower() == "responsibility")
                    {
                        user_Responsibility.ClearSelection();
                        user_Responsibility.Items.FindByValue(dr["datavalue"].ToString()).Selected = true;
                    }
                }
                catch
                {
                }
            }
        }
        private void loadSubscribedeNewsletters()
        {
            DataTable dteNewsletters = DataFunctions.GetDataTable("select g.groupID, g.groupname, datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID join groups g on g.groupID =gdf.groupID join emailgroups eg on eg.emailID = edv.emailID and eg.groupID = g.groupID where edv.emailID = " + EmailID + " and shortname = 'PaidOrFree' and subscribetypecode='S' and g.groupID in (" + getpaidSubscriptiongroups() + ")");

            CreateSubsribedDataSource();

            foreach (DataRow dr in dteNewsletters.Rows)
            {
                if (!dr.IsNull("datavalue"))
                {
                    //if (dr["datavalue"].ToString().ToUpper() == "PAID")     
                    //{
                    //    DataTable dtDates = DataFunctions.GetDataTable("select  entryID, max(case when shortname = 'startdate' then datavalue end) as startdate, max(case when shortname = 'enddate' then datavalue end)  as enddate from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + EmailID + " and entryID is not null and datavalue <> '' and groupID = " + dr["groupID"] + " and shortname in ('startdate', 'enddate') and emailID in (select emailID from emaildatavalues where	emailID = " + EmailID + " and datavalue='PAID' and groupdatafieldsID in (select groupdatafieldsID from groupdatafields where groupID in (" + dr["groupID"] + ") and shortname ='PaidOrFree')) group by entryID");

                    //    foreach (DataRow r in dtDates.Rows)
                    //    {
                    //        DateTime StartDate = Convert.ToDateTime(r["startdate"].ToString());
                    //        DateTime EndDate = Convert.ToDateTime(r["enddate"].ToString());

                    //        if (DateTime.Now >= StartDate && DateTime.Now <= EndDate)
                    //        {
                    //            HtmlInputCheckBox chk = (HtmlInputCheckBox)Page.FindControl("g_" + dr["groupID"]);
                    //            chk.Visible = false;

                    //            Panel pnl = (Panel)Page.FindControl("pnl_" + dr["groupID"]);
                    //            pnl.Visible = false;

                    //            DataRow drow = dtSubscribednewsletters.NewRow();
                    //            drow["groupID"] = dr["groupID"].ToString();
                    //            drow["groupName"] = dr["groupname"].ToString();
                    //            drow["FreeOrPaid"] = "PAID";
                    //            dtSubscribednewsletters.Rows.Add(drow); 

                    //            break;
                    //        }
                    //    }

                    //    //chk.Checked = true;
                    //}
                    //else 
                    if (dr["datavalue"].ToString().ToUpper() == "FREE")
                    {
                        HtmlInputCheckBox chk = (HtmlInputCheckBox)Page.FindControl("g_" + dr["groupID"] + "_free");
                        if (chk != null)
                        {
                            chk.Visible = true;
                            chk.Checked = true;
                        }

                        DataRow drow = dtSubscribednewsletters.NewRow();
                        drow["groupID"] = dr["groupID"].ToString();
                        drow["groupName"] = dr["groupname"].ToString();
                        drow["FreeOrPaid"] = "FREE";
                        dtSubscribednewsletters.Rows.Add(drow);
                        subscribeGroups.Add(dr["groupID"].ToString());

                    }
                    //else if (dr["datavalue"].ToString().ToUpper() == "COMP" || dr["datavalue"].ToString().ToUpper() == "TRIAL")
                    //{
                    //    DataRow drow = dtSubscribednewsletters.NewRow();
                    //    drow["groupID"] = dr["groupID"].ToString();
                    //    drow["groupName"] = dr["groupname"].ToString();
                    //    drow["FreeOrPaid"] = dr["datavalue"].ToString().ToUpper().Trim();
                    //    dtSubscribednewsletters.Rows.Add(drow);

                    //}
                }
            }
            LoadSubscribedgrid();
        }


        private void CreateSubsribedDataSource()
        {
            DataColumn dc = new DataColumn("groupID");
            dtSubscribednewsletters.Columns.Add(dc);

            DataColumn dc1 = new DataColumn("groupName");
            dtSubscribednewsletters.Columns.Add(dc1);

            DataColumn dc2 = new DataColumn("FreeOrPaid");
            dtSubscribednewsletters.Columns.Add(dc2);
        }

        private void LoadSubscribedgrid()
        {
            if (dtSubscribednewsletters.Rows.Count > 0)
            {
                //gvSubscribed.DataSource = dtSubscribednewsletters;
                //gvSubscribed.DataBind();
                //gvSubscribed.Visible = false; 
                pnlCurrentSubscriptions.Visible = true;
            }
            else
            {
                //pnlCurrentSubscriptions.Visible = false;  
                //gvSubscribed.Visible = false;
            }
        }

        #region State Dropdown
        private void loadstate()
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
            addlistitem("MO", "Missourri", "USA");
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
            addlistitem("OT", "Other", "Foreign");
        }

        private void addlistitem(string value, string text, string group)
        {
            ListItem item = new ListItem(text, value);

            if (group != string.Empty)
                item.Attributes["OptionGroup"] = group;

            drpstate.Items.Add(item);

        }

        #endregion

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            EmailID = 0;
            //bool IsPaidSubscriber = false;

            if (!ValidateEmailAddress(txtE.Text))
            {
                phError.Visible = true;
                lblErrorMessage.Text = "Please enter a valid emailaddress.";
                lblErrorMessage.Visible = true;
                return;
            }

            EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("select distinct e.emailID from emails e join emailgroups eg on e.emailID = eg.emailID join groups g on eg.groupID = g.groupID where e.customerID = " + lblcustomerID.Text + " and g.customerID= " + lblcustomerID.Text + " and subscribetypecode = 'S' and e.emailaddress = '" + txtE.Text.Replace("'", "''") + "'"));

            if (EmailID > 0)
            {
                //try
                //{
                //    IsPaidSubscriber = Convert.ToBoolean(DataFunctions.ExecuteScalar("select top 1 case when edv.emailID > 0 then 1 else 0 end from	emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID  join groups g on g.groupID =gdf.groupID join emailgroups eg on edv.emailID = eg.emailID and eg.groupID = g.groupID  where edv.emailID = " + EmailID + " and subscribetypecode = 'S' and shortname = 'PaidOrFree' and datavalue = 'PAID' and g.groupID in (select groupID from groups where customerID = " + lblcustomerID.Text + ")"));
                //}
                //catch
                //{ }

                //if (IsPaidSubscriber)
                //{
                //    loginCtrl.UserName = txtE.Text;
                //    pnlEmailAddress.Visible = true;
                //    pnlLogin.Visible = true;
                //    pnlInfo.Visible = false;
                //    return;
                //}
                //else
                //{
                //Pre Populate Free ENewsletters.
                loadform(txtE.Text);
                //}
            }


            if (pnlEmailAddress.Visible)
            {
                pnlStep1.Visible = true;
                pnlStep2.Visible = true;
                //btnNext.Text = "Submit";
                pnlEmailAddress.Visible = true;
            }

            //btnprevious.Visible = false;
            pnlInfo.Visible = true;
            txtemail.Text = txtE.Text;
            txtemail.Enabled = false;

        }

        //protected void btnPrevious_Click(object sender, EventArgs e) 
        //{
        //    if (pnlStep1.Visible && pnlStep2.Visible)
        //    {
        //        //pnlStep1.Visible = false;
        //        //pnlStep2.Visible = false;
        //        //pnlEmailAddress.Visible = true;   
        //        //btnPrevious.Visible = true;    
        //        //btnNext.Text = "Submit";
        //        pnlEmailAddress.Visible = true;
        //        pnlStep1.Visible = false;
        //        pnlStep2.Visible = false;
        //        //btnprevious.Visible = false;
        //        btnNext.Visible = false;
        //    }
        //    //else if (pnlStep1.Visible)
        //    //{
        //    //    pnlEmailAddress.Visible = true;
        //    //    pnlStep1.Visible = false;
        //    //    pnlStep2.Visible = false;
        //    //    btnPrevious.Visible = false; 
        //    //    btnNext.Visible = true;  
        //    //    //txtE.Text = txtemail.Text;   
        //    //}
        //    //else if (pnlStep3.Visible)
        //    //{
        //    //    pnlStep1.Visible = false;
        //    //    pnlStep2.Visible = true;
        //    //    pnlStep3.Visible = false;
        //    //    btnPrevious.Visible = true;
        //    //    btnNext.Text = "Next";
        //    //}
        //}

        protected void btnNext_Click(object sender, EventArgs e)
        {
            //bool freeSubscriptionSelected = false;
            //bool paidSubscriptionSelected = false;
            //bool paidrenewSelected = false;
            //bool cancelSelected = false;

            try
            {
                if (pnlStep1.Visible && pnlStep2.Visible)
                {
                    string[] paidgroups = getpaidSubscriptiongroups().Split(',');


                    for (int j = 0; j < paidgroups.Length; j++)
                    {
                        try
                        {
                            //HtmlInputCheckBox chk = (HtmlInputCheckBox)Page.FindControl("g_" + paidgroups[j].ToString());

                            //if (chk != null && chk.Checked) 
                            //{
                            //    paidSubscriptionSelected = true;

                            //    HtmlInputCheckBox chkfree = (HtmlInputCheckBox)Page.FindControl("g_" + paidgroups[j].ToString() + "_free");
                            //    if (chkfree.Checked)
                            //        chkfree.Checked = false;
                            //}
                            //else
                            //{
                            //    HtmlInputCheckBox chkfree = (HtmlInputCheckBox)Page.FindControl("g_" + paidgroups[j].ToString() + "_free");
                            //    if (chkfree.Checked)
                            //    {
                            //        freeSubscriptionSelected = true;
                            //    }
                            //    else if (!chkfree.Checked && subscribeGroups.Contains(paidgroups[j].ToString()))
                            //    {
                            //        DataFunctions.Execute("update emailgroups set subscribetypecode='U', LastChanged=getdate() where groupID= '" + paidgroups[j].ToString() + "' and emailID = " + CurrentEmail.ID());
                            //    }
                            //}

                            HtmlInputCheckBox chkfree = (HtmlInputCheckBox)Page.FindControl("g_" + paidgroups[j].ToString() + "_free");

                            if (!chkfree.Checked && subscribeGroups.Contains(paidgroups[j].ToString()))
                            {
                                DataFunctions.Execute("update emailgroups set subscribetypecode='U', LastChanged=getdate() where groupID= '" + paidgroups[j].ToString() + "' and emailID = " + EmailID);
                            }
                        }
                        catch
                        { }

                    }

                    //foreach (GridViewRow r in gvSubscribed.Rows)
                    //{
                    //    //CheckBox chkRenew = (CheckBox)r.FindControl("chkRenew");
                    //    //if (chkRenew.Checked)
                    //    //{
                    //    //    paidrenewSelected = true;
                    //    //}

                    //    CheckBox chkUnsubscribe = (CheckBox)r.FindControl("chkUnsubscribe");
                    //    if (chkUnsubscribe.Checked)
                    //    {
                    //        cancelSelected = true;
                    //    }
                    //}

                    //if (freeSubscriptionSelected || paidSubscriptionSelected || paidrenewSelected || cancelSelected)  
                    //{
                    //    pnlStep1.Visible = false;
                    //    pnlStep2.Visible = false;
                    //    //btnprevious.Visible = true;

                    //    if (paidSubscriptionSelected || paidrenewSelected)
                    //    {
                    //        divSubscriptionOption.Visible = true;
                    //        pnlCheckout.Visible = true;

                    //        loadpricegrid();
                    //    }
                    //    else
                    //    {
                    //        divSubscriptionOption.Visible = false;
                    //        pnlCheckout.Visible = false;
                    //    }
                    //}
                    //else
                    //{
                    //    phError.Visible = true;
                    //    lblErrorMessage.Text = "Please select one of the newsletters.";
                    //    lblErrorMessage.Visible = true;
                    //    return;
                    //}

                    //if (EmailID > 0)
                    //{
                    //    if (paidSubscriptionSelected || paidrenewSelected)
                    //    {
                    //        pnlPassword.Visible = true;

                    //        //string pwd = DataFunctions.ExecuteScalar("select isnull(password,'') from Emails where emailID = " + EmailID).ToString();

                    //        //if (pwd.Trim().Length == 0)
                    //        //{
                    //        RequiredFieldValidator15.Enabled = true;
                    //        RequiredFieldValidator16.Enabled = true;
                    //        //}
                    //        //else
                    //        //{
                    //        //    RequiredFieldValidator15.Enabled = false;
                    //        //    RequiredFieldValidator16.Enabled = false;
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        pnlPassword.Visible = false;
                    //        RequiredFieldValidator15.Enabled = false;
                    //        RequiredFieldValidator16.Enabled = false;
                    //    }
                    //}
                    //else
                    //{
                    //    if (!paidSubscriptionSelected)
                    //    {
                    //        pnlPassword.Visible = false;
                    //        RequiredFieldValidator15.Enabled = false;
                    //        RequiredFieldValidator16.Enabled = false;
                    //    }
                    //    else
                    //    {
                    //pnlPassword.Visible = true;
                    //RequiredFieldValidator15.Enabled = true;
                    //RequiredFieldValidator16.Enabled = true;
                    //    }
                    //}


                    //if (PromoCode == string.Empty)
                    //{
                    //    gvPrice.Columns[3].Visible = false;
                    //    gvPrice.Columns[4].Visible = false;
                    //}


                    //btnPrevious.Visible = true;
                    //btnNext.Text = "Submit";
                    //btnNext.Visible = true;

                    //else if (pnlStep2.Visible)
                    //{
                    if (!ValidateEmailAddress(txtemail.Text))
                    {
                        phError.Visible = true;
                        lblErrorMessage.Text = "Please enter a valid emailaddress.";
                        lblErrorMessage.Visible = true;
                        return;
                    }

                    if (!chkVerify.Checked)
                    {
                        phError.Visible = true;
                        lblErrorMessage.Text = "Please select the checkbox under subscription agreement to confirm all the information is accurate.";
                        lblErrorMessage.Visible = true;
                        return;
                    }

                    //double TotalAmount = 0;

                    Emails email = new Emails();
                    email.GetEmail(txtemail.Text, Convert.ToInt32(lblcustomerID.Text));

                    if (email.ID() > 0)
                        CurrentEmail = Emails.GetEmailByID(email.ID());

                    //if (pnlCheckout.Visible)
                    //{
                    //    foreach (GridViewRow r in gvPrice.Rows)
                    //    {
                    //        Label lblTotal = (Label)r.FindControl("lblTotal");
                    //        TotalAmount = TotalAmount + Convert.ToDouble(lblTotal.Text.Replace("$", ""));
                    //    }
                    //    if (TotalAmount > 0)
                    //    {
                    //        if (TransactionID == string.Empty)
                    //            TransactionID = ProcessedCreditCard(TotalAmount);

                    //        if (TransactionID != string.Empty)
                    //        {
                    //            foreach (GridViewRow r in gvPrice.Rows)
                    //            {
                    //                int GroupID = Convert.ToInt32(gvPrice.DataKeys[Convert.ToInt32(r.RowIndex)].Value);

                    //                Label lblTotal = (Label)r.FindControl("lblTotal");

                    //                AddtoGroup(GroupID, Convert.ToDouble(lblTotal.Text.Replace("$", "")));
                    //            }

                    //            btnNext.Visible = false;
                    //            //btnPrevious.Visible = false;

                    //            SendEmail();
                    //            try
                    //            {
                    //                WebRequest webRequest = WebRequest.Create("https://www.ecn5.com/ecn.communicator/engines/conversion.aspx?b=" + getBlastID + "&e=" + email.ID() + "&total=" + TotalAmount + "&oLink=http://eforms.kmpsgroup.com/paidpub/pharmalive_thankyou.htm");
                    //                HttpWebResponse my_response = (HttpWebResponse)webRequest.GetResponse();
                    //                my_response.Close();
                    //            }
                    //            catch
                    //            { }
                    //        }

                    //        if (txtPassword.Text.Trim() != "")
                    //        {
                    //            DataFunctions.Execute("update emails set password = '" + txtPassword.Text.Replace("'", "''") + "' where emailID = " + CurrentEmail.ID());
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //WebRequest webRequest = WebRequest.Create("https://www.ecn5.com/ecn.communicator/engines/conversion.aspx?b=" + getBlastID + "&e=" + email.ID() + "&total=" + TotalAmount + "&oLink=http://eforms.kmpsgroup.com/paidpub/pharmalive_thankyou.htm");
                    //HttpWebResponse my_response = (HttpWebResponse)webRequest.GetResponse();
                    //my_response.Close();
                    //}


                    //foreach (GridViewRow r in gvSubscribed.Rows)
                    //{
                    //    CheckBox chkUnsubscribe = (CheckBox)r.FindControl("chkUnsubscribe");
                    //    if (chkUnsubscribe.Checked)
                    //    {
                    //        DataFunctions.Execute("update emailgroups set subscribetypecode='U', LastChanged=getdate() where groupID= '" + gvSubscribed.DataKeys[r.RowIndex].Value.ToString() + "' and emailID = " + CurrentEmail.ID());

                    //        if (r.Cells[1].Text.ToUpper() == "PAID") 
                    //        {
                    //            //send email to admin
                    //            SendCancellationEmailToAdmin(Convert.ToInt32(gvSubscribed.DataKeys[r.RowIndex].Value), r.Cells[0].Text);
                    //        }   
                    //    }
                    //}

                    PostFreeSubscription();
                    //SendEmail();  
                    Response.Redirect("pharmalive_thankyou.htm");
                }
            }
            catch (Exception ex)
            {
                phError.Visible = true;
                lblErrorMessage.Text = ex.ToString();
                lblErrorMessage.Visible = true;
            }


            
        }

        private string getpaidSubscriptiongroups()
        {
            string paidSubscriptionGroupIDs = string.Empty;

            DataTable dt = DataFunctions.GetDataTable(" select groupID from ecn_misc..canon_paidpub_enewsletters where CustomerID = " + lblcustomerID.Text);

            foreach (DataRow dr in dt.Rows)
            {
                paidSubscriptionGroupIDs += (paidSubscriptionGroupIDs == string.Empty ? "" : ",") + dr["groupID"].ToString();
            }

            return paidSubscriptionGroupIDs;
        }

        #region Free Subscription Update

        private void PostFreeSubscription()
        {
            string[] groups = getpaidSubscriptiongroups().Split(',');

            string PostParams = string.Empty;

            PostParams = "?c=" + lblcustomerID.Text;
            PostParams += "&f=html&s=S&SFmode="; //manage -commented on 11/10/2008
            PostParams += "&sfID=" + lblsfID.Text;
            PostParams += "&e=" + txtemail.Text;
            PostParams += "&fn=" + txtfirstname.Text;
            PostParams += "&ln=" + txtlastname.Text;
            PostParams += "&t=" + txttitle.Text;
            PostParams += "&n=" + txtfirstname.Text + " " + txtlastname.Text;
            PostParams += "&compname=" + txtcompany.Text;
            PostParams += "&adr=" + txtaddress.Text;
            PostParams += "&adr2=" + txtaddress2.Text;
            PostParams += "&city=" + txtcity.Text;
            PostParams += "&state=" + drpstate.SelectedItem.Value;
            PostParams += "&zc=" + txtzip.Text;
            PostParams += "&ctry=" + drpcountry.SelectedItem.Value;
            PostParams += "&ph=" + txtphone.Text;
            PostParams += "&fax=" + txtfax.Text;
            PostParams += "&user_demo5=y&user_business=" + user_Business.SelectedItem.Value;
            PostParams += "&user_Responsibility=" + user_Responsibility.SelectedItem.Value;
            PostParams += "&user_Effort_Code=" + txtpromo.Text;
            PostParams += "&user_Verification_Date=" + DateTime.Now.ToString("MM/dd/yyyy").ToString();
            PostParams += "&user_PaidOrFree=FREE";

            for (int j = 0; j < groups.Length; j++)
            {
                try
                {
                    HtmlInputCheckBox c = (HtmlInputCheckBox)Page.FindControl("g_" + groups[j] + "_free");
                    if (c.Checked)
                        PostParams += "&g_" + groups[j] + "=y";
                    else
                        PostParams += "&g_" + groups[j] + "=n";
                }
                catch { }
            }

            //HTTP Post.
            HttpPost(PostParams);
        }

        private void HttpPost(string postparams)
        {
            WebRequest webRequest = WebRequest.Create(String.Format(ConfigurationManager.AppSettings["ECN_ActivityEngine_MultiGroupSubscribe_Path"].ToString(), postparams));
            webRequest.Method = "GET";
            WebResponse WebResp = webRequest.GetResponse();
        }

        #endregion

        //protected void gvPrice_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    double discount = 0;
        //    double linetotal = 0;
        //    double regularprice = 0;
        //    double actualprice = 0;
        //    double savings = 0;

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lblRegularPrice = (Label)e.Row.FindControl("lblRegularPrice");
        //        Label lblSavings = (Label)e.Row.FindControl("lblSavings");
        //        Label lblDiscount = (Label)e.Row.FindControl("lblDiscount");
        //        Label lblTotal = (Label)e.Row.FindControl("lblTotal");

        //        regularprice = Convert.ToDouble(lblRegularPrice.Text.Replace("$", ""));
        //        totalactualprice += regularprice;

        //        savings = Convert.ToDouble(lblSavings.Text.Replace("$", ""));

        //        actualprice = regularprice - savings;

        //        if (PromoDiscount > 0)
        //        {
        //            discount = (actualprice * PromoDiscount) / 100;
        //        }

        //        totaldiscountprice += savings + discount;

        //        linetotal = actualprice - discount;
        //        totalprice += linetotal;

        //        lblDiscount.Text = String.Format("{0:C}", savings + discount); ;
        //        lblTotal.Text = String.Format("{0:C}", linetotal);

        //    }
        //    else if (e.Row.RowType == DataControlRowType.Footer)
        //    {
        //        e.Row.Cells[0].Text = " Total:&nbsp;&nbsp;";
        //        e.Row.Cells[1].Text = String.Format("{0:C}", totalactualprice);
        //        e.Row.Cells[2].Text = String.Format("{0:C}", totaldiscountprice);
        //        e.Row.Cells[3].Text = String.Format("{0:C}", totalprice);
        //    }
        //}

        #region subscribe to groups

        //private void AddtoGroup(int GroupID, double amount)
        //{
        //    Groups current_group = new Groups(GroupID);

        //    if (CurrentEmail == null)
        //        CurrentEmail = CreateEmailRecord(current_group);

        //    SubscribeToGroup(current_group, amount);

        //    //Check for Group Trigger Events 
        //    EmailActivityLog log = new EmailActivityLog(EmailActivityLog.InsertSubscribe(CurrentEmail.ID(), 0, "S"));

        //    log.SetGroup(current_group);
        //    log.SetEmail(new Emails(CurrentEmail.ID()));

        //    EventOrganizer eventer = new EventOrganizer();
        //    eventer.CustomerID(current_group.CustomerID());
        //    eventer.Event(log);
        //}

        private Emails CreateEmailRecord(Groups group)
        {
            int EmailID = 0;

            Emails old_email = group.WhatEmailForCustomer(txtemail.Text);

            SqlDateTime sqldatenull = SqlDateTime.Null;

            if (null == old_email)
            {
                SqlConnection db_connection = new SqlConnection(DataFunctions.connStr);
                SqlCommand InsertCommand = new SqlCommand(null, db_connection);
                InsertCommand.CommandText =
                    "INSERT INTO Emails " +
                    "(EmailAddress,CustomerID,Title,FirstName,LastName,FullName,Company,Occupation,Address,Address2,City,State,Zip,Country,Voice,Mobile,Fax,Website,Age,Income,Gender,User1,User2,User3,User4,User5,User6,Birthdate,UserEvent1,UserEvent1Date,UserEvent2,UserEvent2Date,Notes,DateAdded, DateUpdated)"
                    + " VALUES "
                    + "(@emailAddress,@customer_id,@title,@first_name,@last_name,@full_name,@company,@occupation,@address,@address2,@city,@state,@zip,@country,@voice,@mobile,@fax,@website,@age,@income,@gender,@user1,@user2,@user3,@user4,@user5,@user6,@birthdate,@user_event1,@user_event1_date,@user_event2,@user_event2_date,@notes,@DateAdded,@DateUpdated) SELECT @@IDENTITY";

                InsertCommand.Parameters.Add("@emailAddress", SqlDbType.VarChar, 250).Value = txtemail.Text;
                InsertCommand.Parameters.Add("@customer_id", SqlDbType.Int, 4).Value = lblcustomerID.Text;
                InsertCommand.Parameters.Add("@title", SqlDbType.VarChar, 50).Value = txttitle.Text;
                InsertCommand.Parameters.Add("@first_name", SqlDbType.VarChar, 50).Value = txtfirstname.Text;
                InsertCommand.Parameters.Add("@last_name", SqlDbType.VarChar, 50).Value = txtlastname.Text;
                InsertCommand.Parameters.Add("@full_name", SqlDbType.VarChar, 50).Value = txtfirstname.Text + " " + txtlastname.Text;
                InsertCommand.Parameters.Add("@company", SqlDbType.VarChar, 50).Value = txtcompany.Text;
                InsertCommand.Parameters.Add("@occupation", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@address", SqlDbType.VarChar, 255).Value = txtaddress.Text;
                InsertCommand.Parameters.Add("@address2", SqlDbType.VarChar, 255).Value = txtaddress2.Text;
                InsertCommand.Parameters.Add("@city", SqlDbType.VarChar, 50).Value = txtcity.Text;
                InsertCommand.Parameters.Add("@state", SqlDbType.VarChar, 50).Value = drpstate.SelectedItem.Value;
                InsertCommand.Parameters.Add("@zip", SqlDbType.VarChar, 50).Value = txtzip.Text;
                InsertCommand.Parameters.Add("@country", SqlDbType.VarChar, 50).Value = drpcountry.SelectedItem.Value;
                InsertCommand.Parameters.Add("@voice", SqlDbType.VarChar, 50).Value = txtphone.Text;
                InsertCommand.Parameters.Add("@mobile", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@fax", SqlDbType.VarChar, 50).Value = txtfax.Text;
                InsertCommand.Parameters.Add("@website", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@age", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@income", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@gender", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@user1", SqlDbType.VarChar, 255).Value = "";
                InsertCommand.Parameters.Add("@user2", SqlDbType.VarChar, 255).Value = "";
                InsertCommand.Parameters.Add("@user3", SqlDbType.VarChar, 255).Value = "";
                InsertCommand.Parameters.Add("@user4", SqlDbType.VarChar, 255).Value = "";
                InsertCommand.Parameters.Add("@user5", SqlDbType.VarChar, 255).Value = "";
                InsertCommand.Parameters.Add("@user6", SqlDbType.VarChar, 255).Value = "";
                InsertCommand.Parameters.Add("@birthdate", SqlDbType.DateTime).Value = sqldatenull;
                InsertCommand.Parameters.Add("@user_event1", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@user_event1_date", SqlDbType.DateTime).Value = sqldatenull;
                InsertCommand.Parameters.Add("@user_event2", SqlDbType.VarChar, 50).Value = "";
                InsertCommand.Parameters.Add("@user_event2_date", SqlDbType.DateTime).Value = sqldatenull;
                InsertCommand.Parameters.Add("@notes", SqlDbType.Text).Value = "Paid Subscription. DateAdded: " + DateTime.Now.ToString();
                InsertCommand.Parameters.Add("@DateAdded", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                InsertCommand.Parameters.Add("@DateUpdated", SqlDbType.DateTime).Value = DateTime.Now.ToString();

                InsertCommand.CommandTimeout = 0;
                InsertCommand.Connection.Open();
                EmailID = Convert.ToInt32(InsertCommand.ExecuteScalar());
                InsertCommand.Connection.Close();
            }
            else
            {
                try
                {
                    EmailID = old_email.ID();

                    SqlConnection db_connection = new SqlConnection(DataFunctions.connStr);
                    SqlCommand UpdateCommand = new SqlCommand(null, db_connection);
                    UpdateCommand.CommandText = "UPDATE Emails SET " +
                        " Title = @title, FirstName = @first_name, LastName = @last_name, FullName = @full_name, Company = @company," +
                        " Occupation = @occupation, Address = @address, Address2 = @address2, City = @city, State = @state, Zip = @zip, Country = @country, " +
                        " Voice = @voice, Mobile = @mobile, Fax = @fax, Website = @website, Age = @age, Income = @income, Gender = @gender, " +
                        " User1 = @user1, User2 = @user2, User3 = @user3, User4 = @user4, User5 = @user5, User6 = @user6, Birthdate = @birthdate, " +
                        " UserEvent1 = @user_event1, UserEvent1Date = @user_event1_date, " +
                        " UserEvent2 = @user_event2, UserEvent2Date = @user_event2_date " +
                        " WHERE EmailID = @email_id;";

                    UpdateCommand.Parameters.Add("@email_id", SqlDbType.Int, 4).Value = old_email.ID();
                    UpdateCommand.Parameters.Add("@title", SqlDbType.VarChar, 50).Value = txttitle.Text;
                    UpdateCommand.Parameters.Add("@first_name", SqlDbType.VarChar, 50).Value = txtfirstname.Text;
                    UpdateCommand.Parameters.Add("@last_name", SqlDbType.VarChar, 50).Value = txtlastname.Text;
                    UpdateCommand.Parameters.Add("@full_name", SqlDbType.VarChar, 50).Value = txtfirstname.Text + " " + txtlastname.Text;
                    UpdateCommand.Parameters.Add("@company", SqlDbType.VarChar, 50).Value = txtcompany.Text;
                    UpdateCommand.Parameters.Add("@occupation", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@address", SqlDbType.VarChar, 255).Value = txtaddress.Text;
                    UpdateCommand.Parameters.Add("@address2", SqlDbType.VarChar, 255).Value = txtaddress2.Text;
                    UpdateCommand.Parameters.Add("@city", SqlDbType.VarChar, 50).Value = txtcity.Text;
                    UpdateCommand.Parameters.Add("@state", SqlDbType.VarChar, 50).Value = drpstate.SelectedItem.Value;
                    UpdateCommand.Parameters.Add("@zip", SqlDbType.VarChar, 50).Value = txtzip.Text;
                    UpdateCommand.Parameters.Add("@country", SqlDbType.VarChar, 50).Value = drpcountry.SelectedItem.Value;
                    UpdateCommand.Parameters.Add("@voice", SqlDbType.VarChar, 50).Value = txtphone.Text;
                    UpdateCommand.Parameters.Add("@mobile", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@fax", SqlDbType.VarChar, 50).Value = txtfax.Text;
                    UpdateCommand.Parameters.Add("@website", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@age", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@income", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@gender", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@user1", SqlDbType.VarChar, 255).Value = "";
                    UpdateCommand.Parameters.Add("@user2", SqlDbType.VarChar, 255).Value = "";
                    UpdateCommand.Parameters.Add("@user3", SqlDbType.VarChar, 255).Value = "";
                    UpdateCommand.Parameters.Add("@user4", SqlDbType.VarChar, 255).Value = "";
                    UpdateCommand.Parameters.Add("@user5", SqlDbType.VarChar, 255).Value = "";
                    UpdateCommand.Parameters.Add("@user6", SqlDbType.VarChar, 255).Value = "";
                    UpdateCommand.Parameters.Add("@birthdate", SqlDbType.DateTime).Value = sqldatenull;
                    UpdateCommand.Parameters.Add("@user_event1", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@user_event1_date", SqlDbType.DateTime).Value = sqldatenull;
                    UpdateCommand.Parameters.Add("@user_event2", SqlDbType.VarChar, 50).Value = "";
                    UpdateCommand.Parameters.Add("@user_event2_date", SqlDbType.DateTime).Value = sqldatenull;

                    UpdateCommand.CommandTimeout = 0;
                    UpdateCommand.Connection.Open();

                    UpdateCommand.Prepare();
                    UpdateCommand.ExecuteNonQuery();
                    UpdateCommand.Connection.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("ERROR: " + ex.Message);
                }
            }
            return Emails.GetEmailByID(EmailID);
        }

        //private void SubscribeToGroup(Groups group, double amount)
        //{
        //    group.AttachEmail(CurrentEmail, "html", "S");


        //    //double earnedamount = amount / (12 * Convert.ToInt32(rbYears.SelectedItem.Value));
        //    //double Deferredamount = Math.Round(amount - earnedamount, 2);

        //    Hashtable UDFFields = getGroupdatafields(group.ID());
        //    Hashtable UDFData = new Hashtable();

        //    string startdate = DateTime.Now.ToString("MM/dd/yyyy");
        //    string enddate = DateTime.Now.AddDays(-1).AddYears(Convert.ToInt32(rbYears.SelectedItem.Value)).ToString("MM/dd/yyyy");
        //    string subtype = "New";
        //    try
        //    {
        //        string existingenddate = DataFunctions.ExecuteScalar("select top 1 datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + CurrentEmail.ID().ToString() + " and emailID in (select emailID from emaildatavalues where emailID = " + CurrentEmail.ID().ToString() + " and datavalue='PAID' and groupdatafieldsID in (select groupdatafieldsID from groupdatafields where groupID in (" + group.ID() + ") and shortname ='PaidOrFree')) and edv.groupdatafieldsID in (" + UDFFields["enddate"].ToString() + ") order by modifieddate desc").ToString();

        //        if (existingenddate != string.Empty && existingenddate != "")
        //        {
        //            if (Convert.ToDateTime(existingenddate) > DateTime.Now)
        //            {
        //                startdate = Convert.ToDateTime(existingenddate).AddDays(1).ToString("MM/dd/yyyy");
        //                enddate = Convert.ToDateTime(existingenddate).AddDays(-1).AddYears(Convert.ToInt32(rbYears.SelectedItem.Value)).ToString("MM/dd/yyyy");
        //            }
        //            subtype = "Renew";
        //        }
        //    } 
        //    catch
        //    { }

        //    UDFData.Add(UDFFields["startdate"].ToString(), startdate);
        //    UDFData.Add(UDFFields["enddate"].ToString(), enddate);
        //    UDFData.Add(UDFFields["amountpaid"].ToString(), amount.ToString());
        //    UDFData.Add(UDFFields["earnedamount"].ToString(), "0");
        //    UDFData.Add(UDFFields["Deferredamount"].ToString(), amount.ToString());
        //    UDFData.Add(UDFFields["TotalSent"].ToString(), "1");
        //    UDFData.Add(UDFFields["PromoCode"].ToString(), PromoCode);
        //    UDFData.Add(UDFFields["SubType"].ToString(), subtype);
        //    UDFData.Add(UDFFields["TransactionID"].ToString(), TransactionID);
        //    UDFData.Add(UDFFields["PaymentMethod"].ToString(), "Credit");
        //    UDFData.Add(UDFFields["CardType"].ToString(), drpCreditCard.SelectedItem.Value);
        //    UDFData.Add(UDFFields["CardNumber"].ToString(), txtCardNumber.Text.Substring(txtCardNumber.Text.Length - 4));

        //    string GUID = System.Guid.NewGuid().ToString();

        //    IDictionaryEnumerator en = UDFData.GetEnumerator();

        //    while (en.MoveNext())
        //    {
        //        group.AttachUDFToEmail(CurrentEmail, en.Key.ToString(), en.Value.ToString(), GUID);
        //    }

        //    group.AttachUDFToEmail(CurrentEmail, UDFFields["Business"].ToString(), user_Business.SelectedItem.Value);
        //    group.AttachUDFToEmail(CurrentEmail, UDFFields["Responsibility"].ToString(), user_Responsibility.SelectedItem.Value);
        //    group.AttachUDFToEmail(CurrentEmail, UDFFields["PaidOrFree"].ToString(), "PAID");
        //    group.AttachUDFToEmail(CurrentEmail, UDFFields["Effort_Code"].ToString(), txtpromo.Text);
        //    group.AttachUDFToEmail(CurrentEmail, UDFFields["Verification_Date"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
        //    group.AttachUDFToEmail(CurrentEmail, UDFFields["SubDate"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());

        //}

        public Hashtable getGroupdatafields(int groupID)
        {
            Hashtable cUDF = new Hashtable();

            DataTable dtUDF = DataFunctions.GetDataTable("select groupdatafieldsID, shortname from groupdatafields where groupID=" + groupID);

            foreach (DataRow dr in dtUDF.Rows)
            {
                cUDF.Add(dr["shortname"].ToString(), dr["groupdatafieldsID"].ToString());
            }
            return cUDF;
        }

        #endregion

        //private string ProcessedCreditCard(double Amount)
        //{
        //    string TransactionID = string.Empty;
        //    try
        //    {
        //        string pfRequestID = PayflowUtility.RequestId;

        //        // Create User Data Object
        //        UserInfo pfUserInfo = new UserInfo(ConfigurationManager.AppSettings["vsUser"],
        //            ConfigurationManager.AppSettings["vsVendor"],
        //            ConfigurationManager.AppSettings["vsPartner"],
        //            ConfigurationManager.AppSettings["vsPassword"]);

        //        // Create PFPro Connection Data Object
        //        PayflowConnectionData pfConnection = new PayflowConnectionData();

        //        // Create Invoice
        //        //sunil - 07/05/2006 - added channel name for Verisign Reports
        //        CustomerInfo pfcustomerinfo = new CustomerInfo();
        //        pfcustomerinfo.CustId = "CANON";
        //        pfcustomerinfo.CustCode = "CANON";

        //        Invoice pfinvoice = new Invoice();
        //        pfinvoice.Amt = new Currency(Convert.ToDecimal(Amount)); 
        //        pfinvoice.CustomerInfo = pfcustomerinfo;

        //        BillTo Bill = new BillTo();
        //        Bill.Street = txtaddress.Text;
        //        Bill.Zip = txtzip.Text;
        //        Bill.City = txtcity.Text;
        //        Bill.State = drpstate.SelectedItem.Value;
        //        Bill.FirstName = txtfirstname.Text;
        //        Bill.LastName = txtlastname.Text;
        //        Bill.BillToCountry = drpcountry.SelectedItem.Value;
        //        Bill.Email = txtemail.Text;
        //        Bill.PhoneNum = txtphone.Text;
        //        Bill.Fax = txtfax.Text;
        //        Bill.CompanyName = txtcompany.Text;
        //        pfinvoice.BillTo = Bill;

        //        // Create Payment Device - Credit Card Data Object
        //        CreditCard cc = new CreditCard(txtCardNumber.Text, drpMonth.SelectedItem.Value + drpYear.SelectedItem.Value);
        //        cc.Cvv2 = txtcvNumber.Text;

        //        // Create Card Tender Data Object
        //        CardTender Tender = new CardTender(cc);

        //        // Create new Transaction
        //        SaleTransaction Trans = new SaleTransaction(pfUserInfo, pfConnection, pfinvoice, Tender, pfRequestID);

        //        // Submit the Sales Transaction
        //        Trans.SubmitTransaction();

        //        // If Transaction-Response returns zero means transaction was complete and successfull
        //        if (Trans.Response.TransactionResponse.Result == 0)
        //        {
        //            TransactionID = Trans.Response.TransactionResponse.Pnref.ToString();
        //        }
        //        else
        //        {
        //            throw new Exception("The credit card information you have entered is invalid. Please check the credit card type, credit card number, and expiration date and try again.");
        //        }
        //    }
        //    catch
        //    {
        //        throw new Exception("The credit card information you have entered is invalid. Please check the credit card type, credit card number, and expiration date and try again.");
        //    }
        //    TransactionID = System.Guid.NewGuid().ToString();
        //    return TransactionID;
        //}

        //private void SendCancellationEmailToAdmin(int groupID, string groupname)
        //{
        //    string msgBody = string.Empty;
        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
        //    message.To.Add(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
        //    message.CC.Add(ConfigurationManager.AppSettings["Pharmalive_CCEmail"].ToString());
        //    message.Subject = "Subscription Cancellation.";

        //    msgBody += "Subscription Cancellation<br><br>";
        //    msgBody += "Email Address : " + txtemail.Text + "<br>";
        //    msgBody += "First Name : " + txtfirstname.Text + "<br>";
        //    msgBody += "Last Name: " + txtlastname.Text + "<br>";
        //    msgBody += "Newsletter : " + groupname + "<br>";

        //    double defamount = 0;
        //    try
        //    {
        //        defamount = Convert.ToDouble(DataFunctions.ExecuteScalar("select sum(convert(decimal(10,2),isnull(datavalue,0))) from emaildatavalues  where emailID = " + EmailID + " and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = " + groupID + " and shortname ='Deferredamount')"));
        //    }
        //    catch
        //    {

        //    }

        //    msgBody += "Deferred Amount : " + defamount.ToString() + "<br>";


        //    message.Body = msgBody;
        //    message.IsBodyHtml = true;
        //    message.Priority = MailPriority.High;

        //    SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
        //    smtp.Send(message);
        //}

        private void SendEmail()
        {
            string msgBody = string.Empty;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
            message.To.Add(txtemail.Text);
            message.Subject = "Thank you for your subscription.";

            msgBody += System.Configuration.ConfigurationManager.AppSettings["ResponseEmail"];

            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            ////gvPrice.RenderControl(htmlWrite);

            msgBody = msgBody.Replace("%%Date%%", DateTime.Now.ToShortDateString());
            msgBody = msgBody.Replace("%%firstname%%", txtfirstname.Text);
            msgBody = msgBody.Replace("%%username%%", txtemail.Text);
            //msgBody = msgBody.Replace("%%password%%", txtPassword.Text);

            msgBody = msgBody.Replace("%%orderdate%%", DateTime.Now.ToShortDateString());
            msgBody = msgBody.Replace("%%lastname%%", txtlastname.Text);
            msgBody = msgBody.Replace("%%address%%", txtaddress.Text);
            msgBody = msgBody.Replace("%%address2%%", txtaddress2.Text);
            msgBody = msgBody.Replace("%%city%%", txtcity.Text);
            msgBody = msgBody.Replace("%%state%%", drpstate.SelectedItem.Value);
            msgBody = msgBody.Replace("%%zip%%", txtzip.Text);
            msgBody = msgBody.Replace("%%country%%", drpcountry.SelectedItem.Value);
            msgBody = msgBody.Replace("%%emailaddress%%", txtemail.Text);
            msgBody = msgBody.Replace("%%phone%%", txtphone.Text);


            msgBody = msgBody.Replace("%%pricetable%%", string.Empty);

            message.Body = msgBody;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        private bool ValidateEmailAddress(string emailaddress)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
       + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            if (emailaddress != null) return Regex.IsMatch(emailaddress, MatchEmailPattern);
            else return false;

        }

        //protected void rbYears_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    loadpricegrid();
        //}

        //private void loadpricegrid()
        //{
        //    string[] paidgroups = getpaidSubscriptiongroups().Split(',');

        //    string paidgroupIDs = string.Empty;
        //    int newslettercount = 0;

        //    //foreach (GridViewRow r in gvSubscribed.Rows)
        //    //{
        //        //CheckBox chkRenew = (CheckBox)r.FindControl("chkRenew"); 
        //        //if (chkRenew.Checked)
        //        //{
        //        //    newslettercount++;
        //        //    paidgroupIDs += (paidgroupIDs == string.Empty ? gvSubscribed.DataKeys[r.RowIndex].Value.ToString() : "," + gvSubscribed.DataKeys[r.RowIndex].Value.ToString());
        //        //}
        //    //}

        //    for (int j = 0; j < paidgroups.Length; j++)
        //    {
        //        try
        //        {
        //            HtmlInputCheckBox chk = (HtmlInputCheckBox)Page.FindControl("g_" + paidgroups[j].ToString());

        //            if (chk.Checked)
        //            {
        //                newslettercount++;
        //                paidgroupIDs += (paidgroupIDs == string.Empty ? paidgroups[j].ToString() : "," + paidgroups[j].ToString());
        //            }
        //        }
        //        catch { } 
        //    }

        //    DataTable dtPrices = DataFunctions.GetDataTable("exec sp_getPubPricing " + lblcustomerID.Text + "," + newslettercount.ToString() + ",'" + paidgroupIDs + "', " + rbYears.SelectedItem.Value, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
        //    gvPrice.DataSource = dtPrices;
        //    gvPrice.DataBind();
        //}
    }
}
