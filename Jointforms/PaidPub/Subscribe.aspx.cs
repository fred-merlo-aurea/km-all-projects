using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

using PayPal.Payments.Common;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;

using ecn.common.classes;
using ecn.communicator.classes;


namespace PaidPub
{
    public partial class Subscribe : System.Web.UI.Page
    {
        #region local variables

        Emails CurrentEmail = null;
        double totalactualprice = 0;
        double totaldiscountprice = 0;
        double totalprice = 0;
        DataTable dtSubscribednewsletters = new DataTable();

        private int CustomerID
        {
            get
            {
                if (ViewState["CustomerID"] == null)
                    return 0;
                else
                    return (int)ViewState["CustomerID"];
            }

            set { ViewState["CustomerID"] = value; }
        }

        private bool IsTrialForm
        {
            get
            {
                if (ViewState["IsTrialForm"] == null)
                    return false;
                else
                    return (bool)ViewState["IsTrialForm"];
            }

            set { ViewState["IsTrialForm"] = value; }
        }

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

        private string TransactionID
        {
            get
            {
                if (ViewState["TransactionID"] == null)
                    return string.Empty;
                else
                    return ViewState["TransactionID"].ToString();
            }

            set { ViewState["TransactionID"] = value; }
        }

        private string Code
        {
            get
            {
                try { return Request.QueryString["Code"]; }
                catch { return string.Empty; }
            }
        }

        private string PromoCode
        {
            get
            {
                try { return Request.QueryString["pc"].ToString(); }
                catch { return string.Empty; }
            }
        }

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            if (Code != string.Empty)
            {
                DataTable dtForm = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_Forms where Isactive= 1 and Code = '" + Code + "'", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

                if (dtForm.Rows.Count > 0)
                {
                    banner.Controls.Add(new LiteralControl(dtForm.Rows[0]["HeaderHTML"].ToString()));
                    lblDesc.Text = dtForm.Rows[0]["Description"].ToString();
                    footer.InnerHtml = dtForm.Rows[0]["FooterHTML"].ToString();
                    CustomerID = Convert.ToInt32(dtForm.Rows[0]["CustomerID"]);
                    IsTrialForm = Convert.ToBoolean(dtForm.Rows[0]["IsTrial"].ToString());
                    lblenewsletter.Text = dtForm.Rows[0]["newsletterHTML"].ToString();
                }

                dtSubscribednewsletters.Columns.Add(new DataColumn("groupID"));
                dtSubscribednewsletters.Columns.Add(new DataColumn("groupName"));
                dtSubscribednewsletters.Columns.Add(new DataColumn("FreeOrPaid"));

                if (!IsPostBack)
                {
                    btnPrevious.Visible = false;
                    btnRegister.Visible = false;

                    btnRegister.Text = "Next";
                    pnlStep1.Visible = false;
                    pnlStep2.Visible = false;

                    divSubscriptionOption.Visible = false;
                    divTrialSub.Visible = false;
                    pnlCheckout.Visible = false;

                    lblPromotionDesc.Text = "";
                    txtpromo.Text = PromoCode;

                    if (PromoCode != string.Empty)
                    {
                        DataTable dtPC = DataFunctions.GetDataTable("select * from CANON_PAIDPUB_Promotions where CustomerID = " + CustomerID.ToString() + " and code = '" + PromoCode + "'", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
                        if (dtPC.Rows.Count > 0)
                        {
                            lblPromotionDesc.Text = dtPC.Rows[0]["Description"].ToString();
                            PromoDiscount = Convert.ToDouble(dtPC.Rows[0]["Discount"].ToString());
                        }
                    }
                    else
                    {
                        pnlPromoDesc.Visible = false;
                    }
                }
            }
            else
            {
                Response.Redirect("Error.aspx");
            }
        }

        private void loadCategory()
        {
            DataTable dtForm = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_Forms where Isactive= 1 and Code = '" + Code + "'", ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            if (dtForm.Rows.Count > 0)
            {
                DataTable dtGroups = DataFunctions.GetDataTable("select g.GroupID, g.GroupName, g.GroupDescription, ec.categoryID, isnull(ec.name,'') as categoryname from groups g join ecn_misc..canon_paidpub_enewsletters e on e.groupID = g.groupID left outer join ecn_misc..CANON_PAIDPUB_eNewsLetter_Category ec on e.categoryID = ec.categoryID where g.groupID in (" + dtForm.Rows[0]["GroupIDs"].ToString() + ")", ConfigurationManager.ConnectionStrings["conn_communicator"].ConnectionString);

                DataTable dtnewsletter = new DataTable();

                DataColumn dc = new DataColumn("Categoryname");
                dtnewsletter.Columns.Add(dc);

                dc = new DataColumn("groups");
                dtnewsletter.Columns.Add(dc);

                string[] IDs = dtForm.Rows[0]["GroupIDs"].ToString().Split(',');

                string previousCategory = string.Empty;
                string currentCategory = string.Empty;
                string groupIDs = string.Empty;

                for (int i = 0; i < IDs.Length; i++)
                {
                    try
                    {
                        foreach (DataRow dr in dtGroups.Rows)
                        {
                            if (Convert.ToInt32(IDs[i]) == Convert.ToInt32(dr["groupID"]))
                            {
                                currentCategory = dr["categoryname"].ToString() == string.Empty ? "NOCAT" : dr["categoryname"].ToString();

                                if (previousCategory == string.Empty)
                                    previousCategory = dr["categoryname"].ToString() == string.Empty ? "NOCAT" : dr["categoryname"].ToString();

                                if (previousCategory != currentCategory)
                                {
                                    DataRow row;
                                    row = dtnewsletter.NewRow();

                                    row["Categoryname"] = previousCategory == "NOCAT" ? "" : previousCategory;
                                    row["groups"] = groupIDs;
                                    dtnewsletter.Rows.Add(row);
                                    groupIDs = "";
                                    previousCategory = currentCategory;

                                }
                                groupIDs += (groupIDs == string.Empty ? dr["groupID"].ToString() : "," + dr["groupID"].ToString());

                                break;
                            }
                        }
                    }
                    catch
                    { }

                    if (i == IDs.Length - 1)
                    {

                        DataRow row;
                        row = dtnewsletter.NewRow();

                        row["Categoryname"] = currentCategory;
                        row["groups"] = groupIDs;
                        dtnewsletter.Rows.Add(row);
                    }
                }

                rptCategory.DataSource = dtnewsletter;
                rptCategory.DataBind();
            }
            else
            {
                lblErrorMessage.Text = "Subscription Form not exists.";
                lblErrorMessage.Visible = true;
                btnPrevious.Visible = false;
                btnRegister.Visible = false;
            }
        }

        #region eNewsletter Category
        protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblgroupIDs = (Label)e.Item.FindControl("lblgroupIDs");

                DataTable dtGroups = new DataTable();

                dtGroups = DataFunctions.GetDataTable("select g.GroupID, g.GroupName, g.GroupDescription from groups g join ecn_misc..canon_paidpub_enewsletters e on e.groupID = g.groupID where g.groupID in (" + lblgroupIDs.Text + ")", ConfigurationManager.ConnectionStrings["conn_communicator"].ConnectionString);

                DataTable dtnewsletter = new DataTable();

                dtnewsletter.Columns.Add(new DataColumn("GroupID"));
                dtnewsletter.Columns.Add(new DataColumn("GroupName"));
                dtnewsletter.Columns.Add(new DataColumn("GroupDescription"));

                string[] IDs = lblgroupIDs.Text.ToString().Split(',');

                for (int i = 0; i < IDs.Length; i++)
                {
                    try
                    {
                        foreach (DataRow dr in dtGroups.Rows)
                        {
                            
                            string IsPaidorFree = string.Empty;


                            if (EmailID > 0)
                            {
                                try
                                {
                                    IsPaidorFree = DataFunctions.ExecuteScalar("select isnull(datavalue,'') from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID join groups g on g.groupID =gdf.groupID join emailgroups eg on eg.emailID = edv.emailID and eg.groupID = g.groupID where edv.emailID = " + EmailID + " and shortname = 'PaidOrFree' and subscribetypecode='S' and g.groupID in (" + dr["groupID"].ToString() + ")").ToString();
                                }
                                catch
                                { }
                            }

                            if (Convert.ToInt32(IDs[i]) == Convert.ToInt32(dr["groupID"]))
                            {
                                if (IsPaidorFree.ToUpper() == "FREE" || IsPaidorFree.ToUpper() == "")
                                {
                                    if (IsTrialForm)
                                    {
                                        int noofTrials = 0;

                                        try
                                        {
                                            noofTrials = Convert.ToInt32(DataFunctions.ExecuteScalar("select datavalue from emaildatavalues where emailID = " + EmailID + " and groupdatafieldsID = (select groupdatafieldsID  from groupdatafields where groupID = " + dr["groupID"] + " and shortname = 'NoofTrials')"));
                                        }
                                        catch
                                        {
                                            noofTrials = 0;
                                        }

                                        if (noofTrials < Convert.ToInt32(ConfigurationManager.AppSettings["Pharmalive_TrialCount"].ToString()))
                                        {
                                            DataRow row;
                                            row = dtnewsletter.NewRow();
                                            row["GroupID"] = dr["groupID"].ToString();
                                            row["GroupName"] = dr["GroupName"].ToString();
                                            row["GroupDescription"] = dr["GroupDescription"].ToString();
                                            dtnewsletter.Rows.Add(row);
                                        }
                                    }
                                    else
                                    {
                                        DataRow row;
                                        row = dtnewsletter.NewRow();
                                        row["GroupID"] = dr["groupID"].ToString();
                                        row["GroupName"] = dr["GroupName"].ToString();
                                        row["GroupDescription"] = dr["GroupDescription"].ToString();
                                        dtnewsletter.Rows.Add(row);
                                    }
                                    break;
                                }
                                else if ((IsPaidorFree.ToUpper() == "COMP" || IsPaidorFree.ToUpper() == "TRIAL") && !IsTrialForm)
                                {
                                    DataRow row;
                                    row = dtnewsletter.NewRow();

                                    row["GroupID"] = dr["groupID"].ToString();
                                    row["GroupName"] = dr["GroupName"].ToString();
                                    row["GroupDescription"] = dr["GroupDescription"].ToString();
                                    dtnewsletter.Rows.Add(row);

                                    DataRow drow = dtSubscribednewsletters.NewRow();
                                    drow["groupID"] = dr["groupID"].ToString();
                                    drow["groupName"] = dr["groupname"].ToString();
                                    drow["FreeOrPaid"] = IsPaidorFree.ToUpper().Trim();
                                    dtSubscribednewsletters.Rows.Add(drow);
                                    break;
                                }

                                else if (IsPaidorFree.ToUpper() == "PAID" && !IsTrialForm)
                                {
                                    DataRow drow = dtSubscribednewsletters.NewRow();

                                    if (EmailID > 0)
                                    {
                                        DataTable dtDates = DataFunctions.GetDataTable("select  entryID, max(case when shortname = 'startdate' then datavalue end) as startdate, max(case when shortname = 'enddate' then datavalue end)  as enddate from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + EmailID + " and entryID is not null and datavalue <> ''  and groupID = " + dr["groupID"] + " and shortname in ('startdate', 'enddate') and emailID in (select emailID from emaildatavalues where	emailID = " + EmailID + " and datavalue='PAID' and groupdatafieldsID in (select groupdatafieldsID from groupdatafields where groupID in (" + dr["groupID"] + ") and shortname ='PaidOrFree')) group by entryID");

                                        foreach (DataRow r in dtDates.Rows)
                                        {
                                            DateTime StartDate = Convert.ToDateTime(r["startdate"].ToString());
                                            DateTime EndDate = Convert.ToDateTime(r["enddate"].ToString());

                                            if (DateTime.Now >= StartDate && DateTime.Now <= EndDate)
                                            {
                                                drow["groupID"] = dr["groupID"].ToString();
                                                drow["groupName"] = dr["groupname"].ToString();
                                                drow["FreeOrPaid"] = "PAID";
                                                dtSubscribednewsletters.Rows.Add(drow);

                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataRow row;
                                        row = dtnewsletter.NewRow();

                                        row["GroupID"] = dr["groupID"].ToString();
                                        row["GroupName"] = dr["GroupName"].ToString();
                                        row["GroupDescription"] = dr["GroupDescription"].ToString();
                                        dtnewsletter.Rows.Add(row);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    { }
                }

                if (dtnewsletter.Rows.Count > 0)
                {
                    GridView gvNewsletters = (GridView)e.Item.FindControl("gvNewsletters");
                    gvNewsletters.DataSource = dtnewsletter;
                    gvNewsletters.DataBind();
                }

                if (dtSubscribednewsletters.Rows.Count > 0)
                {
                    gvSubscribed.DataSource = dtSubscribednewsletters;
                    gvSubscribed.DataBind();
                    gvSubscribed.Visible = true;
                    pnlCurrentSubscriptions.Visible = true;
                }
                else
                {
                    pnlCurrentSubscriptions.Visible = false;
                    gvSubscribed.Visible = false;
                }
            }
        }
        #endregion


        #region Price Grid

        protected void rbYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadpricegrid();
        }

        private void loadpricegrid()
        {
            string[] paidgroups = getpaidSubscriptiongroups().Split(',');

            string paidgroupIDs = string.Empty;
            int newslettercount = 0;

            foreach (GridViewRow r in gvSubscribed.Rows)
            {
                CheckBox chkRenew = (CheckBox)r.FindControl("chkRenew");
                if (chkRenew.Checked)
                {
                    newslettercount++;
                    paidgroupIDs += (paidgroupIDs == string.Empty ? gvSubscribed.DataKeys[r.RowIndex].Value.ToString() : "," + gvSubscribed.DataKeys[r.RowIndex].Value.ToString());
                }
            }

            foreach (RepeaterItem ritem in rptCategory.Items)
            {
                GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");

                foreach (GridViewRow r in gvNewsletters.Rows)
                {
                    int GroupID = Convert.ToInt32(gvNewsletters.DataKeys[Convert.ToInt32(r.RowIndex)].Value);

                    CheckBox chkSelected = (CheckBox)r.FindControl("chkSelected");

                    if (chkSelected.Checked)
                    {
                        newslettercount++;
                        paidgroupIDs += (paidgroupIDs == string.Empty ? GroupID.ToString() : "," + GroupID.ToString());
                    }
                }
            }

            DataTable dtPrices = DataFunctions.GetDataTable("exec sp_getPubPricing " + CustomerID + "," + newslettercount.ToString() + ",'" + paidgroupIDs + "', " + rbYears.SelectedItem.Value, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);
            gvPrice.DataSource = dtPrices;
            gvPrice.DataBind();

            if (IsTrialForm)
                gvPrice.Visible = false;
        }

        protected void gvPrice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            double discount = 0;
            double linetotal = 0;
            double regularprice = 0;
            double actualprice = 0;
            double savings = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRegularPrice = (Label)e.Row.FindControl("lblRegularPrice");
                Label lblSavings = (Label)e.Row.FindControl("lblSavings");
                Label lblDiscount = (Label)e.Row.FindControl("lblDiscount");
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");

                regularprice = Convert.ToDouble(lblRegularPrice.Text.Replace("$", ""));

                if (IsTrialForm)
                {
                    totalactualprice += regularprice;

                    lblSavings.Text = String.Format("{0:C}", regularprice); ;

                    actualprice = regularprice - savings;
                    discount = regularprice;
                    totaldiscountprice += regularprice;
                    linetotal = 0;
                    totalprice += 0;

                    lblDiscount.Text = String.Format("{0:C}", regularprice); ;
                    lblTotal.Text = String.Format("{0:C}", 0);

                  
                }
                else
                {
                   
                    totalactualprice += regularprice;

                    savings = Convert.ToDouble(lblSavings.Text.Replace("$", ""));

                    actualprice = regularprice - savings;

                    if (PromoDiscount > 0)
                    {
                        discount = (actualprice * PromoDiscount) / 100;
                    }

                    totaldiscountprice += savings + discount;

                    linetotal = actualprice - discount;
                    totalprice += linetotal;
                    lblDiscount.Text = String.Format("{0:C}", savings + discount); ;
                    lblTotal.Text = String.Format("{0:C}", linetotal);
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = " Total:&nbsp;&nbsp;";
                e.Row.Cells[1].Text = String.Format("{0:C}", totalactualprice);
                e.Row.Cells[2].Text = String.Format("{0:C}", totaldiscountprice);
                e.Row.Cells[3].Text = String.Format("{0:C}", totalprice);
            }
        }
    
        #endregion

        #region Login Events
        public void loginCtrl_LoggingIn(object sender, System.Web.UI.WebControls.LoginCancelEventArgs e)
        {
            if (!ValidateEmailAddress(loginCtrl.UserName))
            {
                lblErrorMessage.Text = "Enter a valid e-mail address.";
                lblErrorMessage.Visible = true;
                e.Cancel = true;
            }
        }

        protected void loginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("select distinct e.emailID from emails e join emailgroups eg on e.emailID = eg.emailID join groups g on eg.groupID = g.groupID where e.customerID = " + CustomerID + " and g.customerID= " + CustomerID + " and subscribetypecode = 'S' and password = '" + loginCtrl.Password.Replace("'", "''") + "'and e.emailaddress = '" + loginCtrl.UserName.Replace("'", "''") + "'"));

            if (EmailID > 0)
            {
                loadCategory();
                loadSubscriberDetails(loginCtrl.UserName);
                pnlEmailAddress.Visible = false;
                pnlLogin.Visible = false;
                pnlStep1.Visible = true;
                pnlStep2.Visible = false;
                btnRegister.Visible = true;

                txtemail.Text = loginCtrl.UserName;
                txtemail.Enabled = false;
            }
        }
        #endregion

        protected void loadSubscriberDetails(string emailaddress)
        {
            DataTable dtEmail = DataFunctions.GetDataTable("select e.EmailID,isnull(EmailAddress	,'') as EmailAddress,isnull(FirstName,'') as FirstName,isnull(LastName,'') as LastName,isnull(Title,'') as Title,isnull(Company,'') as Company,isnull(Address,'') as Address,isnull(Address2,'') as Address2,isnull(City,'') as City,isnull(State,'') as State,isnull(Zip,'') as Zip,isnull(Country,'') as Country,isnull(Voice,'') as Voice,isnull(Fax,'') as Fax from emails e where e.emailaddress = '" + emailaddress.Replace("'", "''") + "' and customerID = " + CustomerID);

            if (dtEmail.Rows.Count > 0)
            {
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
                        drpcountry.ClearSelection();
                    drpcountry.Items.FindByValue(dtEmail.Rows[0]["Country"].ToString()).Selected = true;
                }
                catch { }

                if (dtEmail.Rows[0]["Voice"].ToString() != string.Empty)
                    txtphone.Text = dtEmail.Rows[0]["Voice"].ToString();

                if (dtEmail.Rows[0]["Fax"].ToString() != string.Empty)
                    txtfax.Text = dtEmail.Rows[0]["Fax"].ToString();

                loadUDFValues();
                //loadSubscribedeNewsletters(Convert.ToInt32(dtEmail.Rows[0]["EmailID"]));

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

        #region button clicks

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            pnlStep1.Visible = true;
            pnlStep2.Visible = false;
            btnRegister.Text = "Next";
            btnPrevious.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool IsPaidSubscriber = false;

            if (!ValidateEmailAddress(txtE.Text))
            {
                lblErrorMessage.Text = "Please enter a valid emailaddress.";
                lblErrorMessage.Visible = true;
                return;
            }

                EmailID = Convert.ToInt32(DataFunctions.ExecuteScalar("select distinct e.emailID from emails e join emailgroups eg on e.emailID = eg.emailID join groups g on eg.groupID = g.groupID where e.customerID = " + CustomerID + " and g.customerID= " + CustomerID + " and subscribetypecode = 'S' and e.emailaddress = '" + txtE.Text.Replace("'", "''") + "'"));

                if (EmailID > 0)
                {
                    IsPaidSubscriber = Convert.ToBoolean(DataFunctions.ExecuteScalar("select top 1 case when edv.emailID > 0 then 1 else 0 end from	emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID  join groups g on g.groupID =gdf.groupID join emailgroups eg on edv.emailID = eg.emailID and eg.groupID = g.groupID  where edv.emailID = " + EmailID + " and subscribetypecode='S' and shortname = 'PaidOrFree' and datavalue = 'PAID' and g.groupID in (select groupID from groups where customerID = " + CustomerID + ")"));

                    if (IsPaidSubscriber && !IsTrialForm)
                    {
                        loginCtrl.UserName = txtE.Text;
                        pnlEmailAddress.Visible = false;
                        pnlLogin.Visible = true;
                        pnlStep1.Visible = false;
                        pnlStep2.Visible = false;
                        return;
                    }

                    loadSubscriberDetails(txtE.Text.Replace("'", "''"));
                }

            loadCategory();

            pnlEmailAddress.Visible = false;
            pnlLogin.Visible = false;
            pnlStep1.Visible = true;
            pnlStep2.Visible = false;
            btnRegister.Visible = true;

            txtemail.Text = txtE.Text;
            txtemail.Enabled = false;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            bool paidSubscriptionSelected = false;
            bool paidrenewSelected = false;
            bool cancelSelected = false;

            try
            {
                if (btnRegister.Text.ToUpper() == "NEXT")
                {
                    string GroupIDs = string.Empty;

                    foreach (GridViewRow r in gvSubscribed.Rows)
                    {
                        CheckBox chkRenew = (CheckBox)r.FindControl("chkRenew");
                        if (chkRenew.Checked)
                            paidrenewSelected = true;

                        CheckBox chkUnsubscribe = (CheckBox)r.FindControl("chkUnsubscribe");
                        if (chkUnsubscribe.Checked)
                            cancelSelected = true;
                    }

                    foreach (RepeaterItem ritem in rptCategory.Items)
                    {
                        GridView gvNewsletters = (GridView)ritem.FindControl("gvNewsletters");

                        foreach (GridViewRow r in gvNewsletters.Rows)
                        {
                            CheckBox chkSelected = (CheckBox)r.FindControl("chkSelected");

                            if (chkSelected.Checked)
                            {
                                paidSubscriptionSelected = true;
                                break;
                            }
                        }
                    }

                    if (paidSubscriptionSelected || paidrenewSelected || cancelSelected)
                    {
                        if (paidSubscriptionSelected || paidrenewSelected)
                        {
                            if (IsTrialForm)
                            {
                                divTrialSub.Visible = true;
                                divSubscriptionOption.Visible = false;
                                pnlCheckout.Visible = false;
                                loadpricegrid();
                            }
                            else
                            {
                                divSubscriptionOption.Visible = true;
                                pnlCheckout.Visible = true;
                                loadpricegrid();
                            }
                        }
                        else
                        {
                            divSubscriptionOption.Visible = false;
                            pnlCheckout.Visible = false;
                        }
                    }
                    else
                    {
                        lblErrorMessage.Text = "ERROR : Select 1 or more Newsletters.";
                        lblErrorMessage.Visible = true;
                        return;
                    }

                    //if (EmailID > 0)
                    //{
                    //    RequiredFieldValidator16.Enabled = false;
                    //    RequiredFieldValidator17.Enabled = false;
                    //}

                    pnlStep1.Visible = false;
                    pnlStep2.Visible = true;
                    btnRegister.Text = "Submit";
                    btnPrevious.Visible = true;
                }
                else
                {
                    if (!ValidateEmailAddress(txtemail.Text))
                    {
                        lblErrorMessage.Text = "Please enter a valid emailaddress.";
                        lblErrorMessage.Visible = true;
                        return;
                    }

                    if (!chkVerify.Checked)
                    {
                        lblErrorMessage.Text = "Please select the checkbox to confirm all the information is accurate.";
                        lblErrorMessage.Visible = true;
                        return;
                    }

                    double TotalAmount = 0;

                    Emails email = new Emails();
                    email.GetEmail(txtemail.Text, CustomerID);

                    if (email.ID() > 0)
                        CurrentEmail = Emails.GetEmailByID(email.ID());


                    if (IsTrialForm) //Trial
                    {
                        foreach (GridViewRow r in gvPrice.Rows)
                        {
                            int GroupID = Convert.ToInt32(gvPrice.DataKeys[Convert.ToInt32(r.RowIndex)].Value);

                            Label lblTotal = (Label)r.FindControl("lblTotal");

                            AddtoGroup(GroupID, Convert.ToDouble(lblTotal.Text.Replace("$", "")));
                        }

                        SendEmail();
                    }
                    else if (pnlCheckout.Visible)
                    {
                        foreach (GridViewRow r in gvPrice.Rows)
                        {
                            Label lblTotal = (Label)r.FindControl("lblTotal");
                            TotalAmount = TotalAmount + Convert.ToDouble(lblTotal.Text.Replace("$", ""));
                        }

                        if (TotalAmount > 0)
                        {
                            //if (TransactionID == string.Empty)
                            //    TransactionID = ProcessedCreditCard(TotalAmount);

                            if (TransactionID != string.Empty)
                            {
                                foreach (GridViewRow r in gvPrice.Rows)
                                {
                                    int GroupID = Convert.ToInt32(gvPrice.DataKeys[Convert.ToInt32(r.RowIndex)].Value);

                                    Label lblTotal = (Label)r.FindControl("lblTotal");

                                    AddtoGroup(GroupID, Convert.ToDouble(lblTotal.Text.Replace("$", "")));
                                }

                                SendEmail();
                            }
                        }
                    }

                    WebRequest webRequest = WebRequest.Create("https://www.ecn5.com/ecn.communicator/engines/conversion.aspx?b=" + getBlastID + "&e=" + email.ID() + "&total=" + TotalAmount + "&oLink=http://eforms.kmpsgroup.com/paidpub/subscribe.aspx?step=thankyou");
                    HttpWebResponse my_response = (HttpWebResponse)webRequest.GetResponse();
                    my_response.Close();
 

                    if (txtPassword.Text.Trim() != "")
                    {
                        DataFunctions.Execute("update emails set password = '" + txtPassword.Text.Replace("'", "''") + "' where emailID = " + CurrentEmail.ID());
                    }

                    foreach (GridViewRow r in gvSubscribed.Rows)
                    {
                        CheckBox chkUnsubscribe = (CheckBox)r.FindControl("chkUnsubscribe");
                        if (chkUnsubscribe.Checked)
                        {
                            DataFunctions.Execute("update emailgroups set subscribetypecode='U', LastChanged=getdate() where groupID= '" + gvSubscribed.DataKeys[r.RowIndex].Value.ToString() + "' and emailID = " + CurrentEmail.ID());

                            if (r.Cells[1].Text.ToUpper() == "PAID")
                            {
                                //send email to admin
                                SendCancellationEmailToAdmin(Convert.ToInt32(gvSubscribed.DataKeys[r.RowIndex].Value), r.Cells[0].Text);
                            }
                        }
                    }

                    pnlStep2.Visible = false;

                    btnRegister.Visible = false;
                    btnPrevious.Visible = false;

                    lblErrorMessage.Text = "Thank you for your subscription.";
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
            }
        }
        #endregion

        #region subscribe to groups

        private void AddtoGroup(int GroupID, double amount)
        {
            Groups current_group = new Groups(GroupID);

            if (CurrentEmail == null)
                CurrentEmail = CreateEmailRecord(current_group);

            SubscribeToGroup(current_group, amount);

            //Check for Group Trigger Events 
            EmailActivityLog log = new EmailActivityLog(EmailActivityLog.InsertSubscribe(CurrentEmail.ID(), 0, "S"));

            log.SetGroup(current_group);
            log.SetEmail(new Emails(CurrentEmail.ID()));

            EventOrganizer eventer = new EventOrganizer();
            eventer.CustomerID(current_group.CustomerID());
            eventer.Event(log);
        }

        private Emails CreateEmailRecord(Groups group)
        {
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
                InsertCommand.Parameters.Add("@customer_id", SqlDbType.Int, 4).Value = CustomerID;
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

        private void SubscribeToGroup(Groups group, double amount)
        {
            group.AttachEmail(CurrentEmail, "html", "S");

            //double earnedamount = amount / (12 * Convert.ToInt32(rbYears.SelectedItem.Value));
            //double Deferredamount = Math.Round(amount - earnedamount, 2);

            Hashtable UDFFields = getGroupdatafields(group.ID());
            Hashtable UDFData = new Hashtable();

            // add logic to renew the newsletter.

            string startdate = DateTime.Now.ToString("MM/dd/yyyy");
            string enddate = DateTime.Now.ToString("MM/dd/yyyy");
            string subtype = "New";

            if (IsTrialForm)
            {
                enddate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");
                UDFData.Add(UDFFields["startdate"].ToString(), startdate);
                UDFData.Add(UDFFields["enddate"].ToString(), enddate);
                UDFData.Add(UDFFields["PromoCode"].ToString(), txtpromo.Text);
            }
            else
            {
                enddate = DateTime.Now.AddDays(-1).AddYears(Convert.ToInt32(rbYears.SelectedItem.Value)).ToString("MM/dd/yyyy");
                subtype = "New";
                try
                {
                    //string existingenddate = DataFunctions.ExecuteScalar("select top 1 datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + CurrentEmail.ID().ToString() + " and groupID = " + group.ID().ToString() + " and edv.groupdatafieldsID in (" + UDFFields["enddate"].ToString() + ") order by modifieddate desc").ToString();
                    string existingenddate = DataFunctions.ExecuteScalar("select top 1 datavalue from emaildatavalues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = " + CurrentEmail.ID().ToString() + " and emailID in (select emailID from emaildatavalues where emailID = " + CurrentEmail.ID().ToString() + " and datavalue='PAID' and groupdatafieldsID in (select groupdatafieldsID from groupdatafields where groupID in (" + group.ID() + ") and shortname ='PaidOrFree')) and edv.groupdatafieldsID in (" + UDFFields["enddate"].ToString() + ") order by modifieddate desc").ToString();

                    if (existingenddate != string.Empty && existingenddate != "")
                    {
                        if (Convert.ToDateTime(existingenddate) > DateTime.Now)
                        {
                            startdate = Convert.ToDateTime(existingenddate).AddDays(1).ToString("MM/dd/yyyy");
                            enddate = Convert.ToDateTime(existingenddate).AddDays(-1).AddYears(Convert.ToInt32(rbYears.SelectedItem.Value)).ToString("MM/dd/yyyy");
                        }
                        subtype = "Renew";
                    }
                }
                catch
                { }

                UDFData.Add(UDFFields["startdate"].ToString(), startdate);
                UDFData.Add(UDFFields["enddate"].ToString(), enddate);
                UDFData.Add(UDFFields["amountpaid"].ToString(), amount.ToString());
                UDFData.Add(UDFFields["earnedamount"].ToString(), "0");
                UDFData.Add(UDFFields["Deferredamount"].ToString(), amount.ToString());
                UDFData.Add(UDFFields["TotalSent"].ToString(), "0");
                UDFData.Add(UDFFields["PromoCode"].ToString(), txtpromo.Text);
                UDFData.Add(UDFFields["SubType"].ToString(), subtype);
                UDFData.Add(UDFFields["TransactionID"].ToString(), TransactionID);
                UDFData.Add(UDFFields["PaymentMethod"].ToString(), "Credit");
                UDFData.Add(UDFFields["CardType"].ToString(), drpCreditCard.SelectedItem.Value);
                UDFData.Add(UDFFields["CardNumber"].ToString(), txtCardNumber.Text.Substring(txtCardNumber.Text.Length - 4));
            }

            string GUID = System.Guid.NewGuid().ToString();

            IDictionaryEnumerator en = UDFData.GetEnumerator();

            while (en.MoveNext())
            {
                group.AttachUDFToEmail(CurrentEmail, en.Key.ToString(), en.Value.ToString(), GUID);
            }
            group.AttachUDFToEmail(CurrentEmail, UDFFields["Business"].ToString(), user_Business.SelectedItem.Value);
            group.AttachUDFToEmail(CurrentEmail, UDFFields["Responsibility"].ToString(), user_Responsibility.SelectedItem.Value);
            group.AttachUDFToEmail(CurrentEmail, UDFFields["PaidOrFree"].ToString(), IsTrialForm?"TRIAL":"PAID");
            group.AttachUDFToEmail(CurrentEmail, UDFFields["Effort_Code"].ToString(), txtpromo.Text);
            group.AttachUDFToEmail(CurrentEmail, UDFFields["Verification_Date"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());
            group.AttachUDFToEmail(CurrentEmail, UDFFields["SubDate"].ToString(), DateTime.Now.ToString("MM/dd/yyyy").ToString());

            if (IsTrialForm)
            {
                int noofTrials = 1;

                try
                {
                    noofTrials = Convert.ToInt32(DataFunctions.ExecuteScalar("select datavalue from emaildatavalues where emailID = " + CurrentEmail.ID() + " and groupdatafieldsID = (select groupdatafieldsID  from groupdatafields where groupID = " + group.ID() + " and shortname = 'NoofTrials')"));
                    noofTrials = noofTrials + 1;
                }
                catch
                {
                    noofTrials = 1;
                }

                group.AttachUDFToEmail(CurrentEmail, UDFFields["NoofTrials"].ToString(), noofTrials.ToString());

            }
        }

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
        //    return TransactionID;
        //}
        #endregion
  
        private string getpaidSubscriptiongroups()
        {
            string paidSubscriptionGroupIDs = string.Empty;

            DataTable dt = DataFunctions.GetDataTable(" select groupID from ecn_misc..canon_paidpub_enewsletters where CustomerID = " + CustomerID);

            foreach (DataRow dr in dt.Rows)
            {
                paidSubscriptionGroupIDs += (paidSubscriptionGroupIDs == string.Empty ? "" : ",") + dr["groupID"].ToString();
            }

            return paidSubscriptionGroupIDs;
        }

        #region Other methods

        private bool ValidateEmailAddress(string emailaddress)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
       + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            if (emailaddress != null) return Regex.IsMatch(emailaddress, MatchEmailPattern);
            else return false;

        }

        private void SendCancellationEmailToAdmin(int groupID, string groupname)
        {
            string msgBody = string.Empty;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
            message.To.Add(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
            message.CC.Add(ConfigurationManager.AppSettings["Pharmalive_CCEmail"].ToString());
            message.Subject = "Subscription Cancellation.";

            msgBody += "Subscription Cancellation<br><br>";
            msgBody += "Email Address : " + txtemail.Text + "<br>";
            msgBody += "First Name : " + txtfirstname.Text + "<br>";
            msgBody += "Last Name: " + txtlastname.Text + "<br>";
            msgBody += "Newsletter : " + groupname + "<br>";

            double defamount = 0;
            try
            {
                defamount = Convert.ToDouble(DataFunctions.ExecuteScalar("select sum(convert(decimal(10,2),isnull(datavalue,0))) from emaildatavalues  where emailID = " + EmailID + " and groupdatafieldsID = (select groupdatafieldsID from groupdatafields where groupID = " + groupID + " and shortname ='Deferredamount')"));
            }
            catch
            { 
            
            }

            msgBody += "Deferred Amount : " + defamount.ToString() + "<br>";


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

        private void SendEmail()
        {
            string msgBody = string.Empty;
            MailMessage message = new MailMessage();

            if (IsTrialForm)
            {
                message.From = new MailAddress(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
                message.To.Add(txtemail.Text);
                message.Subject = "Thank you for your trial subscription.";

                msgBody += System.Configuration.ConfigurationManager.AppSettings["ResponseTrialEmail"];

            }
            else
            {
                message.From = new MailAddress(ConfigurationManager.AppSettings["Pharmalive_FromEmail"].ToString());
                message.To.Add(txtemail.Text);
                message.Subject = "Thank you for your subscription.";

                msgBody += System.Configuration.ConfigurationManager.AppSettings["ResponseEmail"];

            }

            msgBody = msgBody.Replace("%%Date%%", DateTime.Now.ToShortDateString());
            msgBody = msgBody.Replace("%%firstname%%", txtfirstname.Text);
            msgBody = msgBody.Replace("%%username%%", txtemail.Text);
            msgBody = msgBody.Replace("%%password%%", txtPassword.Text);

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

            if (!IsTrialForm)
            {
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                gvPrice.RenderControl(htmlWrite);
                msgBody = msgBody.Replace("%%pricetable%%", stringWrite.ToString());
            }
           

            message.Body = msgBody;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }

        #endregion

    }
}
