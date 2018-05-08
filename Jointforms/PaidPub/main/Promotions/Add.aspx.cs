using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ecn.common.classes;

namespace PaidPub.main.Promotions
{
    public partial class Add : System.Web.UI.Page
    {
        private int PromotionID
        {
            get
            {
                try { return Convert.ToInt32(Request.QueryString["PromotionID"]); }
                catch { return 0; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;

            if (!IsPostBack)
            {

                txtDiscount.Attributes.Add("onkeypress", "return checkKeyPressForDecimal(this, event)");
                txtDiscount.Attributes.Add("onblur", "validateDecimal(this);");
                txtDiscount.Attributes.Add("decimalSeparator", ".");
                txtDiscount.Attributes.Add("groupSeparator", "");
                txtDiscount.Attributes.Add("decimalDigits", "2");
                txtDiscount.Attributes.Add("style", "text-align:right;padding-right:1px;");

                if (PromotionID > 0)
                    LoadPromotion();
            }

        }

        private void LoadPromotion()
        {
            DataTable dtPromotion = DataFunctions.GetDataTable("select * from CANON_PAIDPUB_Promotions where customerID = " + Session["CustomerID"].ToString() + " and PromotionID = " + PromotionID, ConfigurationManager.ConnectionStrings["conn_misc"].ConnectionString);

            if (dtPromotion.Rows.Count > 0)
            {
                txtPromotionName.Text = dtPromotion.Rows[0]["Name"].ToString();
                txtPromotionCode.Text = dtPromotion.Rows[0]["Code"].ToString();
                txtDesc.Text = dtPromotion.Rows[0]["Description"].ToString();
                txtDiscount.Text = dtPromotion.Rows[0]["Discount"].ToString();

                rbStatus.ClearSelection();

                if (Convert.ToBoolean(dtPromotion.Rows[0]["IsActive"]))
                    rbStatus.Items.FindByValue("1").Selected = true;
                else
                    rbStatus.Items.FindByValue("0").Selected = true;
            }
            else
            {
                lblErrorMessage.Text = "Promotion not exists.";
                lblErrorMessage.Visible = true;
                btnSave.Visible = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sqlquery = string.Empty;

            try
            {
                bool Exists = Convert.ToBoolean(DataFunctions.ExecuteScalar("misc", "if exists (select PromotionID from CANON_PAIDPUB_Promotions where customerID = " + Session["CustomerID"].ToString() + " and promotionID <> " + PromotionID + " and Code = '" + txtPromotionCode.Text.Replace("'", "''")  + "')   select 1 else select 0"));

                if (!Exists)
                {
                    if (PromotionID > 0)
                    {
                        sqlquery = "update CANON_PAIDPUB_Promotions set Name = '" + txtPromotionName.Text.Replace("'", "''") + "', Description = '" + txtDesc.Text.Replace("'", "''")  + "', Code = '" + txtPromotionCode.Text.Replace("'", "''") + "', Discount = " + (txtDiscount.Text.Trim() == "" ? "NULL" : txtDiscount.Text.Trim()) + ",IsActive = " + rbStatus.SelectedItem.Value + " where CustomerID = " + Session["CustomerID"].ToString() + " and PromotionID = " + PromotionID.ToString();
                    }
                    else
                    {
                        sqlquery = "INSERT INTO CANON_PAIDPUB_Promotions VALUES ('" + txtPromotionName.Text.Replace("'", "''") + "','" + txtDesc.Text.Replace("'", "''") + "'," + Session["CustomerID"].ToString() + ",'" + txtPromotionCode.Text.Replace("'", "''") + "'," + (txtDiscount.Text.Trim() == "" ? "NULL" : txtDiscount.Text.Trim()) + "," + rbStatus.SelectedItem.Value + ")";
                    }

                    DataFunctions.Execute("misc", sqlquery);
                    Response.Redirect("default.aspx");
                }
                else
                {
                    lblErrorMessage.Text = "ERROR : Promotion Code already exists.";
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
}
