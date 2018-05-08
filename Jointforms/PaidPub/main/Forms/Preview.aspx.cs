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
using System.Text.RegularExpressions;

using ecn.common.classes;
using ecn.communicator.classes;

namespace PaidPub.main.Forms
{
    public partial class Preview : System.Web.UI.Page
    {
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

                            if (Convert.ToInt32(IDs[i]) == Convert.ToInt32(dr["groupID"]))
                            {
                                if (IsPaidorFree.ToUpper() != "PAID")
                                {
                                    DataRow row;
                                    row = dtnewsletter.NewRow();

                                    row["GroupID"] = dr["groupID"].ToString();
                                    row["GroupName"] = dr["GroupName"].ToString();
                                    row["GroupDescription"] = dr["GroupDescription"].ToString();
                                    dtnewsletter.Rows.Add(row);
                                    break;
                                }
                                else
                                {

                                    DataRow drow = dtSubscribednewsletters.NewRow();
                                    drow["groupID"] = dr["groupID"].ToString();
                                    drow["groupName"] = dr["groupname"].ToString();
                                    drow["FreeOrPaid"] = "PAID";
                                    dtSubscribednewsletters.Rows.Add(drow);
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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
                loadCategory();
                pnlEmailAddress.Visible = false;
                pnlLogin.Visible = false;
                pnlStep1.Visible = true;
                pnlStep2.Visible = false;
                btnRegister.Visible = true;
                txtemail.Enabled = false;
        }
        #endregion

        
        #region button clicks

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            pnlStep1.Visible = true;
            pnlStep2.Visible = false;
            btnRegister.Text = "Next";
            btnRegister.Visible = true;
            btnPrevious.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            loadCategory();

            pnlEmailAddress.Visible = false;
            pnlLogin.Visible = true;
            pnlStep1.Visible = false;
            pnlStep2.Visible = false;
            btnRegister.Visible = false;

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
                            divSubscriptionOption.Visible = true;
                            pnlCheckout.Visible = true;
                            loadpricegrid();
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

                   pnlStep1.Visible = false;
                    pnlStep2.Visible = true;
                    btnRegister.Text = "Submit";
                    btnRegister.Visible = false;
                    btnPrevious.Visible = true;
                }
                else
                {
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

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        #endregion

    }
}
