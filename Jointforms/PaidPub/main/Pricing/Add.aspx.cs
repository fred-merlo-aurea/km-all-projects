using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using ecn.common.classes;

namespace PaidPub.main.Pricing
{
    public partial class Add : System.Web.UI.Page
    {
        private int PriceID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["PriceID"]); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AddDecimalValidation(txtRegRate1);
            AddDecimalValidation(txtRegRate2);
            AddDecimalValidation(txtRegRate3);
            AddDecimalValidation(txtActualRate1);
            AddDecimalValidation(txtActualRate2);
            AddDecimalValidation(txtActualRate3);
            AddDecimalValidation(txtAddlDiscount);
            
            if (!IsPostBack)
            {
                drpNewsletter2.Visible = false;
                 drpNewsletter3.Visible = false;
                pnlPricing.Visible = true;
                btnSave.Visible = true;

                DataTable dtPricing = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_eNewsLetters n join ecn5_communicator..groups g on n.groupID = g.groupID where n.customerID = " + Session["CustomerID"].ToString(), ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

                drpNewsletter2.DataSource = dtPricing;
                drpNewsletter2.DataTextField = "GroupName";
                drpNewsletter2.DataValueField = "GroupID";
                drpNewsletter2.DataBind();

                drpNewsletter2.Items.Insert(0, new ListItem("--- select Newsletter ---", ""));

                drpNewsletter3.DataSource = dtPricing;
                drpNewsletter3.DataTextField = "GroupName";
                drpNewsletter3.DataValueField = "GroupID";
                drpNewsletter3.DataBind();

                drpNewsletter3.Items.Insert(0, new ListItem("--- select Newsletter ---", ""));

                if (PriceID > 0)
                {
                    LoadPricing();
                }
            }
        }

        private void LoadPricing()
        {

                DataTable dtPricing = DataFunctions.GetDataTable("select * from ecn_misc..CANON_PAIDPUB_Pricing where customerID = " + Session["CustomerID"].ToString() + " and PriceID = " + PriceID, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

                if (dtPricing.Rows.Count > 0)
                {
                    txtName.Text = dtPricing.Rows[0]["Name"].ToString();
                    txtDescription.Text = dtPricing.Rows[0]["Description"].ToString();
                    try
                    {
                        drpCombo.ClearSelection();
                        drpCombo.Items.FindByValue(dtPricing.Rows[0]["ComboFor"].ToString()).Selected = true;
                    }
                    catch { }

                    if (dtPricing.Rows[0]["ComboFor"].ToString() == "1")
                    {
                        rbWith.Text = "for";
                        rbWithout.Text = "Default Pricing";
                        drpNewsletter2.Visible = false;
                    }
                    else
                    {
                        rbWith.Text = "with";
                        rbWithout.Text = "without";
                    }

                    if (!dtPricing.Rows[0].IsNull("WithNewsletter"))
                    {
                        rbWith.Checked = true;
                        try
                        {
                            drpNewsletter2.Visible = true;
                            drpNewsletter2.ClearSelection();
                            drpNewsletter2.Items.FindByValue(dtPricing.Rows[0]["WithNewsletter"].ToString()).Selected = true;
                        }
                        catch { }
                    }

                    if (!dtPricing.Rows[0].IsNull("WithOutNewsletter"))
                    {
                        rbWithout.Checked = true;
                        try
                        {
                            drpNewsletter3.Visible = true;
                            drpNewsletter3.ClearSelection();
                            drpNewsletter3.Items.FindByValue(dtPricing.Rows[0]["WithOutNewsletter"].ToString()).Selected = true;

                        }
                        catch { }
                    }

                    txtRegRate1.Text = dtPricing.Rows[0]["RegularRate1yr"].ToString();
                    txtActualRate1.Text = dtPricing.Rows[0]["ActualRate1yr"].ToString();
                    txtRegRate2.Text = dtPricing.Rows[0]["RegularRate2yr"].ToString();
                    txtActualRate2.Text = dtPricing.Rows[0]["ActualRate2yr"].ToString();
                    txtRegRate3.Text = dtPricing.Rows[0]["RegularRate3yr"].ToString();
                    txtActualRate3.Text = dtPricing.Rows[0]["ActualRate3yr"].ToString();
                    txtAddlDiscount.Text = dtPricing.Rows[0]["Addldiscount"].ToString();
                }
                else
                {
                    lblErrorMessage.Text = "Newsletter not exists.";
                    lblErrorMessage.Visible = true;
                    btnSave.Visible = false;
                }
            
        }

        private void AddDecimalValidation(TextBox tb)
        {
            tb.Attributes.Add("onkeypress", "return checkKeyPressForDecimal(this, event)");
            tb.Attributes.Add("onblur", "validateDecimal(this);");
            tb.Attributes.Add("decimalSeparator", ".");
            tb.Attributes.Add("groupSeparator", "");
            tb.Attributes.Add("decimalDigits", "2");
            tb.Attributes.Add("style", "text-align:right;padding-right:1px;");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (rbWith.Checked || rbWithout.Checked)
            {
                string sqlquery = string.Empty;

                try
                {
                    Response.Write("if exists (select priceID from CANON_PAIDPUB_Pricing where customerID = " + Session["CustomerID"].ToString() + " and priceID <> " + PriceID + " and ComboFor = " + drpCombo.SelectedItem.Value + " and WithNewsletter " + (rbWith.Checked ? "= " + drpNewsletter2.SelectedItem.Value : " is NULL") + " and WithOutNewsletter " + (rbWithout.Checked ? "=" + drpNewsletter3.SelectedItem.Value : " is NULL") + " )   select 1 else select 0");
                    bool Exists = Convert.ToBoolean(DataFunctions.ExecuteScalar("misc", "if exists (select priceID from CANON_PAIDPUB_Pricing where customerID = " + Session["CustomerID"].ToString() + " and priceID <> " + PriceID + " and ComboFor = " + drpCombo.SelectedItem.Value + " and WithNewsletter " + (rbWith.Checked ? "= " + drpNewsletter2.SelectedItem.Value : " is NULL") + " and WithOutNewsletter " + (rbWithout.Checked ? "=" + drpNewsletter3.SelectedItem.Value : " is NULL") + " )   select 1 else select 0"));

                    if (!Exists)
                    {
                        if (PriceID > 0)
                        {
                            sqlquery = "UPDATE ecn_misc.dbo.CANON_PAIDPUB_Pricing SET Name = '" + txtName.Text.Replace("'", "''") + "',Description = '" + txtDescription.Text.Replace("'", "''") + "', ComboFor = " + drpCombo.SelectedItem.Value + ", CustomerID = " + Session["CustomerID"].ToString() + ", WithNewsletter = " + (rbWith.Checked ? drpNewsletter2.SelectedItem.Value : "NULL") + ", WithOutNewsletter = " + (rbWithout.Checked ? drpNewsletter3.SelectedItem.Value : "NULL") + ",RegularRate1yr = " + (txtRegRate1.Text.Trim() == "" ? "0" : txtRegRate1.Text) + ", ActualRate1yr = " + (txtActualRate1.Text.Trim() == "" ? "0" : txtActualRate1.Text) + ", RegularRate2yr = " + (txtRegRate2.Text.Trim() == "" ? "0" : txtRegRate2.Text) + ", ActualRate2yr = " + (txtActualRate2.Text.Trim() == "" ? "0" : txtActualRate2.Text) + ", RegularRate3yr = " + (txtRegRate3.Text.Trim() == "" ? "0" : txtRegRate3.Text) + ", ActualRate3yr = " + (txtActualRate3.Text.Trim() == "" ? "0" : txtActualRate3.Text) + ", Addldiscount = " + (txtAddlDiscount.Text.Trim() == "" ? "0" : txtAddlDiscount.Text) + ", UpdatedDate = getdate() WHERE PriceID = " + PriceID.ToString();
                        }
                        else
                        {
                            sqlquery = "INSERT INTO CANON_PAIDPUB_Pricing (Name,Description,ComboFor,CustomerID,WithNewsletter,WithOutNewsletter,RegularRate1yr,ActualRate1yr,RegularRate2yr,ActualRate2yr,RegularRate3yr,ActualRate3yr,Addldiscount,CreatedDate,UpdatedDate) VALUES ('" + txtName.Text.Replace("'", "''") + "','" + txtDescription.Text.Replace("'", "''") + "'," + drpCombo.SelectedItem.Value + "," + Session["CustomerID"].ToString() + "," + (rbWith.Checked ? drpNewsletter2.SelectedItem.Value : "NULL") + "," + (rbWithout.Checked ? drpNewsletter3.SelectedItem.Value : "NULL") + "," + (txtRegRate1.Text.Trim() == "" ? "0" : txtRegRate1.Text) + "," + (txtActualRate1.Text.Trim() == "" ? "0" : txtActualRate1.Text) + "," + (txtRegRate2.Text.Trim() == "" ? "0" : txtRegRate2.Text) + "," + (txtActualRate2.Text.Trim() == "" ? "0" : txtActualRate2.Text) + "," + (txtRegRate3.Text.Trim() == "" ? "0" : txtRegRate3.Text) + "," + (txtActualRate3.Text.Trim() == "" ? "0" : txtActualRate3.Text) + "," + (txtAddlDiscount.Text.Trim() == "" ? "0" : txtAddlDiscount.Text) + ",getdate(),getdate())";
                        }

                        
                        DataFunctions.Execute("misc", sqlquery);
                        Response.Redirect("default.aspx");
                    }
                    else
                    {
                        lblErrorMessage.Text = "ERROR : Pricing already exists.";
                        lblErrorMessage.Visible = true;
                    }

                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = "ERROR : " + ex.Message;
                    lblErrorMessage.Visible = true;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void rbWith_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWith.Checked)
            {
                drpNewsletter2.Visible = true;
                drpNewsletter3.Visible = false;
            }
            else
                drpNewsletter2.Visible = false;
        }

        protected void rbWithout_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWithout.Checked)
            {
                drpNewsletter2.Visible = false;
                drpNewsletter3.Visible = true;
            }
            else
                drpNewsletter3.Visible = false;
        }

        protected void drpCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCombo.SelectedItem.Value == "1")
            {
                rbWith.Text = "for";
                rbWithout.Text = "Default Pricing";
                //drpNewsletter2.Visible = false;
            }
            else
            {
                rbWith.Text = "with";
                rbWithout.Text = "without";                
            }
        }
    }
}
